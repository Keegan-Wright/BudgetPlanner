﻿using BudgetPlanner.Shared.Extensions;
using BudgetPlanner.Server.External.Services.Models.OpenBanking;
using BudgetPlanner.Shared.Models.Request.OpenBanking;
using System.Collections.Concurrent;
using System.Security.Claims;
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Server.External.Services.OpenBanking;
using BudgetPlanner.Shared.Enums;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Server.Services.OpenBanking
{
    public class OpenBankingService : BaseService, IOpenBankingService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
        private readonly IOpenBankingApiService _openBankingApiService;
        private readonly int _syncMins = 5;

        public OpenBankingService(BudgetPlannerDbContext budgetPlannerDbContext, IOpenBankingApiService openBankingApiService, ClaimsPrincipal user) : base(user, budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
            _openBankingApiService = openBankingApiService;
        }



        public async IAsyncEnumerable<OpenBankingProvider> GetOpenBankingProvidersAsync()
        {
            await foreach (var provider in _budgetPlannerDbContext.IsolateToUser(UserId)
                               .Include(x => x.Providers)
                               .ThenInclude(x => x.Accounts)
                               .ThenInclude(x => x.Transactions)
                               .Include(x => x.Providers).ThenInclude(x => x.Accounts).ThenInclude(x => x.AccountBalance)
                               .Include(x => x.Providers).ThenInclude(x => x.Scopes)
                               .Include(x => x.Providers).ThenInclude(x => x.Syncronisations)
                               .SelectMany(x => x.Providers).AsAsyncEnumerable())
            {
                yield return provider;
            }
        }

        public async Task<OpenBankingProvider> GetOpenBankingProviderByIdAsync(string providerId)
        {
            var provider = await _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.Providers)
                .SelectMany(x => x.Providers)
                .Where(x => x.OpenBankingProviderId == providerId)
                .FirstOrDefaultAsync();

            return provider;
        }



        public async IAsyncEnumerable<ExternalOpenBankingProvider> GetOpenBankingProvidersForClientAsync()
        {

                await foreach (var provider in _openBankingApiService.GetAvailableProvidersAsync().OrderBy(x => x.DisplayName))
                {
                    yield return provider;
                }
        }

        public string BuildAuthUrl(GetProviderSetupUrlRequestModel setupProviderRequestModel)
        {
            return _openBankingApiService.BuildAuthUrl(setupProviderRequestModel.ProviderIds, setupProviderRequestModel.Scopes);
        }

        public async Task<bool> AddVendorViaAccessCodeAsync(string accessCode)
        {
            var providerAccessToken = await _openBankingApiService.ExchangeCodeForAccessTokenAsync(accessCode);
            var providerInformation = await _openBankingApiService.GetProviderInformation(providerAccessToken.AccessToken);

            await CreateNewProvider(accessCode, providerAccessToken, providerInformation);

            return true;
        }

        public async Task PerformSyncAsync(SyncTypes syncFlags)
        {
            await foreach (var provider in GetOpenBankingProvidersAsync())
                    {
                        await BulkLoadProviderAsync(provider, syncFlags);
                    }
                    await _budgetPlannerDbContext.SaveChangesAsync();
                    
        }


        private async Task CreateNewProvider(string accessCode, ExternalOpenBankingAccessResponseModel providerAccessToken, ExternalOpenBankingAccountConnectionResponseModel providerInformation)
        {
            var user = await _budgetPlannerDbContext.IsolateToUser(UserId).FirstOrDefaultAsync();
            await foreach (var externalProvider in providerInformation.Results)
            {

                    using var logoClient = new HttpClient();
                    var providerLogo = await logoClient.GetStreamAsync(externalProvider.Provider.LogoUri);

                    using var ms = new MemoryStream();
                    await providerLogo.CopyToAsync(ms);

                    var provider = new OpenBankingProvider()
                    {
                        AccessCode = accessCode,
                        Name = externalProvider.Provider.DisplayName,
                        OpenBankingProviderId = externalProvider.Provider.ProviderId,
                        Created = DateTime.Now.ToUniversalTime(),
                        Logo = ByteArrayHelpers.ConvertSvgStreamToPngStream(ms.ToArray()).ToArray(),
                        Scopes = []
                    };

                    if (user.Providers is null)
                    {
                        user.Providers = [];
                    }
                    
                    user.Providers.Add(provider);

                    await _budgetPlannerDbContext.SaveChangesAsync();

                    var accessToken = new OpenBankingAccessToken()
                    {
                        AccessToken = providerAccessToken.AccessToken,
                        ProviderId = provider.Id,
                        ExpiresIn = providerAccessToken.ExpiresIn,
                        RefreshToken = providerAccessToken.RefreshToken,
                        Created = DateTime.Now.ToUniversalTime()
                    };

                    await foreach (var scope in externalProvider.Scopes)
                    {
                        var providerScope = new OpenBankingProviderScopes()
                        {
                            Scope = scope,
                            Created = DateTime.Now.ToUniversalTime(),
                            ProviderId = provider.Id
                        };

                        provider.Scopes.Add(providerScope);
                    }

                    await _budgetPlannerDbContext.AddAsync(accessToken);
                    await _budgetPlannerDbContext.SaveChangesAsync();


                    await BulkLoadProviderAsync(provider);
            }
        }

        private async Task BulkLoadProviderAsync(OpenBankingProvider provider, SyncTypes syncFlags = SyncTypes.All)
        {
            var providerScopes = provider.Scopes ?? [];
            
            var providerSyncs = provider.Syncronisations?.Where(x =>
                x.SyncronisationTime > DateTime.Now.AddMinutes(-_syncMins).ToUniversalTime()) ?? [];

            provider.Accounts ??= [];
            provider.Scopes ??= [];
            provider.Syncronisations ??= [];
            

            
            
            var transactionsForProvider = provider.Accounts?.SelectMany(x => x.Transactions)
                .OrderByDescending(x => x.TransactionTime)
                .GroupBy(x => x.AccountId)
                .Select(x => x.FirstOrDefault()).ToList() ?? [];


            await EnsureAuthenticatedAsync(provider);

            var authToken = await GetAccessTokenAsync(provider);

            var accountsResponse = await _openBankingApiService.GetAllAccountsAsync(authToken);


            var providerAccounts = new ConcurrentBag<ExternalOpenBankingAccount>();
            var providerBalances = new ConcurrentBag<(string AccountId, ExternalOpenBankingAccountBalance Balances)>();
            var providerTransactions = new ConcurrentBag<(string AccountId, ExternalOpenBankingAccountTransaction Transactions)>();
            var providerPendingTransactions = new ConcurrentBag<(string AccountId , ExternalOpenBankingAccountTransaction Transactions)>();
            var providerStandingOrders = new ConcurrentBag<(string AccountId, ExternalOpenBankingAccountStandingOrder StandingOrders)>();
            var providerDirectDebits = new ConcurrentBag<(string AccountId , ExternalOpenBankingDirectDebit DirectDebits)>();
            var performedSyncs = new ConcurrentBag<OpenBankingSynronisation>();

            await Parallel.ForEachAsync(accountsResponse.Results, async (account, cts) =>
            {
                providerAccounts.Add(account);

                var relevantSyncs = providerSyncs.Where(x => x.OpenBankingAccountId == account.AccountId);


                var standingOrdersTask = GetOpenBankingStandingOrdersForAccountAsync(provider, syncFlags, account, authToken, performedSyncs, relevantSyncs);
                var directDebitsTask = GetOpenBankingDirectDebitsForAccountAsync(provider, syncFlags, account, authToken, performedSyncs, relevantSyncs);
                var balanceTask = GetOpenBankingAccountBalanceAsync(provider, syncFlags, account, authToken, performedSyncs, relevantSyncs);

                var latestTransactionForAccount = transactionsForProvider.FirstOrDefault(x => x?.Account.OpenBankingAccountId == account.AccountId);

                var transactionsTask = GetOpenBankingTransactionsForAccountAsync(provider, syncFlags, account, authToken, performedSyncs, relevantSyncs, latestTransactionForAccount);
                var pendingTransactionsTask = GetOpenBankingPendingTransactionsForAccountAsync(provider, syncFlags, account, authToken, performedSyncs, relevantSyncs, latestTransactionForAccount);

                await Task.WhenAll(standingOrdersTask, directDebitsTask, balanceTask, transactionsTask, pendingTransactionsTask);

                var standingOrdersResults = await standingOrdersTask;
                var directDebitsResults = await directDebitsTask;
                var balanceResults = await balanceTask;
                var transactionsResults = await transactionsTask;
                var pendingTransactionsResults = await pendingTransactionsTask;

                var standingOrdersCollationTask = CollateAccountStandingOrdersAsync(standingOrdersResults, account.AccountId, providerStandingOrders);
                var directDebitsCollationTask = CollateAccountDirectDebitsAsync(directDebitsResults, account.AccountId, providerDirectDebits);
                var collateBalanceTask = CollateAccountBalanceAsync(balanceResults, account.AccountId, providerBalances);
                var collateTransactionsTask = CollateAccountTransactionsAsync(transactionsResults, account.AccountId, providerTransactions);
                var collatePendingTransactionsTask = CollateAccountPendingTransactionsAsync(pendingTransactionsResults, account.AccountId, providerPendingTransactions);

                await Task.WhenAll(standingOrdersCollationTask, directDebitsCollationTask, collateBalanceTask, collateTransactionsTask, collatePendingTransactionsTask);

            });

            var strategy = _budgetPlannerDbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
     
                using var dbTrans = await _budgetPlannerDbContext.Database.BeginTransactionAsync();

                var accountsEdits = new List<OpenBankingAccount>();
                await foreach (var account in providerAccounts.ToAsyncEnumerable())
                {
                    accountsEdits.Add(UpdateOrCreateAccount(account, provider));
                }

                await _budgetPlannerDbContext.BulkInsertOrUpdateAsync(accountsEdits);

                var accountsForProvider = provider.Accounts;
                var balanceEdits = new List<OpenBankingAccountBalance>();
                await foreach (var balance in providerBalances.ToAsyncEnumerable())
                {
                    var accountToUse =
                        accountsForProvider.FirstOrDefault(x => x.OpenBankingAccountId == balance.AccountId);
                    
                    balanceEdits.Add(UpdateOrCreateAccountBalance(balance.Balances, accountToUse));
                }

                await _budgetPlannerDbContext.BulkInsertOrUpdateAsync(balanceEdits);

                var standingOrderEdits = new List<OpenBankingStandingOrder>();
                await foreach (var standingOrder in providerStandingOrders.ToAsyncEnumerable())
                {
                    var accountToUse =
                        accountsForProvider.FirstOrDefault(x => x.OpenBankingAccountId == standingOrder.AccountId);
                    standingOrderEdits.Add(
                        UpdateOrCreateAccountStandingOrder(standingOrder.StandingOrders, accountToUse));
                }

                await _budgetPlannerDbContext.BulkInsertOrUpdateAsync(standingOrderEdits);

                var directDebitEdits = new List<OpenBankingDirectDebit>();
                await foreach (var directDebit in providerDirectDebits.ToAsyncEnumerable())
                {
                    var accountToUse =
                        accountsForProvider.FirstOrDefault(x => x.OpenBankingAccountId == directDebit.AccountId);
                    directDebitEdits.Add(
                        UpdateOrCreateAccountDirectDebit(directDebit.DirectDebits, accountToUse));
                }

                await _budgetPlannerDbContext.BulkInsertOrUpdateAsync(directDebitEdits);


                var transactionsEdits = new List<OpenBankingTransaction>();
                await foreach (var transaction in providerTransactions.ToAsyncEnumerable())
                {
                    var accountToUse =
                        accountsForProvider.FirstOrDefault(x => x.OpenBankingAccountId == transaction.AccountId);
                    transactionsEdits.Add(await UpdateOrCreateAccountTransaction(transaction.Transactions, accountToUse,
                        false, provider));
                }

                await foreach (var transaction in providerPendingTransactions.ToAsyncEnumerable())
                {
                    var accountToUse =
                        accountsForProvider.FirstOrDefault(x => x.OpenBankingAccountId == transaction.AccountId);
                    transactionsEdits.Add(await UpdateOrCreateAccountTransaction(transaction.Transactions, accountToUse,
                        true, provider));
                }


                await _budgetPlannerDbContext.BulkInsertOrUpdateAsync(transactionsEdits);

                if (transactionsEdits.Any())
                {
                    foreach (var transaction in transactionsEdits.Where(x => x.Classifications is not null))
                    {
                        transaction.Classifications ??= [];
                        foreach (var classification in transaction.Classifications)
                        {
                            classification.Transaction = transaction;
                        }
                    }

                    var classifications = transactionsEdits.Select(x => x.Classifications);

                    var classificationItems = classifications.Where(x => x is not null && x.Any()).SelectMany(x => x);

                    await _budgetPlannerDbContext.BulkInsertOrUpdateAsync(classificationItems);

                }

                foreach (var sync in performedSyncs)
                {
                    var account =
                        accountsForProvider.FirstOrDefault(x => x.OpenBankingAccountId == sync.OpenBankingAccountId);

                    sync.Account = account;
                    sync.AccountId = account.Id;
                    provider.Syncronisations.Add(sync);
                }

                await _budgetPlannerDbContext.BulkInsertAsync(performedSyncs);

                await dbTrans.CommitAsync();
            });
           

        }

        private async Task CollateAccountPendingTransactionsAsync(ExternalOpenBankingAccountTransactionsResponseModel? pendingTransactionsResults, string accountId, ConcurrentBag<(string, ExternalOpenBankingAccountTransaction)> providerPendingTransactions)
        {
            if (pendingTransactionsResults == null)
            {
                return;
            }
            else if(pendingTransactionsResults.Results != null)
            {

                await foreach (var pendingTransaction in pendingTransactionsResults.Results)
                {
                    providerPendingTransactions.Add((accountId, pendingTransaction));
                }

            }
        }

        private async Task CollateAccountTransactionsAsync(ExternalOpenBankingAccountTransactionsResponseModel? transactionsResults, string accountId, ConcurrentBag<(string, ExternalOpenBankingAccountTransaction)> providerTransactions)
        {
            if (transactionsResults == null)
            {
                return;
            }
            else if (transactionsResults.Results != null)
            {
                await foreach (var transaction in transactionsResults.Results)
                {
                    providerTransactions.Add((accountId, transaction));
                }
            }
        }

        private async Task CollateAccountBalanceAsync(ExternalOpenBankingGetAccountBalanceResponseModel? balanceResults, string accountId, ConcurrentBag<(string, ExternalOpenBankingAccountBalance)> providerBalances)
        {
            if (balanceResults == null)
            {
                return;
            }
            else if (balanceResults.Results != null)
            {
                await foreach (var accountBalance in balanceResults.Results)
                {
                    providerBalances.Add((accountId, accountBalance));
                }
            }
        }

        private async Task CollateAccountDirectDebitsAsync(ExternalOpenBankingAccountDirectDebitsResponseModel? directDebitsResults, string accountId, ConcurrentBag<(string AccountId, ExternalOpenBankingDirectDebit DirectDebits)> providerDirectDebits)
        {
            if (directDebitsResults == null)
            {
                return;
            }
            else if (directDebitsResults.Results != null)
            {
                await foreach (var directDebit in directDebitsResults.Results)
                {
                    providerDirectDebits.Add((accountId, directDebit));
                }
            }
        }

        private async Task CollateAccountStandingOrdersAsync(ExternalOpenBankingAccountStandingOrdersResponseModel? standingOrdersResults, string accountId, ConcurrentBag<(string AccountId, ExternalOpenBankingAccountStandingOrder StandingOrders)> providerStandingOrders)
        {
            if (standingOrdersResults == null)
            {
                return;
            }
            else if(standingOrdersResults.Results != null)
            {
                await foreach (var standingOrder in standingOrdersResults.Results)
                {
                    providerStandingOrders.Add((accountId, standingOrder));
                }

            }
        }

        private async Task<ExternalOpenBankingAccountStandingOrdersResponseModel?> GetOpenBankingStandingOrdersForAccountAsync(OpenBankingProvider provider, SyncTypes syncFlags, ExternalOpenBankingAccount account, string authToken, ConcurrentBag<OpenBankingSynronisation> performedSyncs, IEnumerable<OpenBankingSynronisation> relevantSyncs)
        {
            if (ShouldSynchronise(syncFlags, relevantSyncs, SyncTypes.StandingOrders))
            {
                var standingOrders = await _openBankingApiService.GetAccountStandingOrdersAsync(account.AccountId, authToken);
                performedSyncs.Add(CreateSyncLog(provider, account.AccountId, SyncTypes.StandingOrders));
                return standingOrders;
            }

            return null;
        }

        private async Task<ExternalOpenBankingAccountDirectDebitsResponseModel?> GetOpenBankingDirectDebitsForAccountAsync(OpenBankingProvider provider, SyncTypes syncFlags, ExternalOpenBankingAccount account, string authToken, ConcurrentBag<OpenBankingSynronisation> performedSyncs, IEnumerable<OpenBankingSynronisation> relevantSyncs)
        {
            if (ShouldSynchronise(syncFlags, relevantSyncs, SyncTypes.DirectDebits))
            {
                var directDebits = await _openBankingApiService.GetAccountDirectDebitsAsync(account.AccountId, authToken);
                performedSyncs.Add(CreateSyncLog(provider, account.AccountId, SyncTypes.DirectDebits));
                return directDebits;
            }


            return null;
        }

        private async Task<ExternalOpenBankingGetAccountBalanceResponseModel?> GetOpenBankingAccountBalanceAsync(OpenBankingProvider provider, SyncTypes syncFlags, ExternalOpenBankingAccount account, string authToken, ConcurrentBag<OpenBankingSynronisation> performedSyncs, IEnumerable<OpenBankingSynronisation> relevantSyncs)
        {
            if (ShouldSynchronise(syncFlags, relevantSyncs, SyncTypes.Balance))
            {
                var balance = await _openBankingApiService.GetAccountBalanceAsync(account.AccountId, authToken);
                performedSyncs.Add(CreateSyncLog(provider, account.AccountId, SyncTypes.Balance));
                return balance;
            }

            return null;
        }

        private async Task<ExternalOpenBankingAccountTransactionsResponseModel?> GetOpenBankingTransactionsForAccountAsync(OpenBankingProvider provider, SyncTypes syncFlags, ExternalOpenBankingAccount account, string authToken, ConcurrentBag<OpenBankingSynronisation> performedSyncs, IEnumerable<OpenBankingSynronisation> relevantSyncs, OpenBankingTransaction? latestTransactionForAccount)
        {
            if (ShouldSynchronise(syncFlags, relevantSyncs, SyncTypes.Transactions))
            {
                var transactions = await _openBankingApiService.GetAccountTransactionsAsync(account.AccountId, authToken, latestTransactionForAccount?.TransactionTime);
                performedSyncs.Add(CreateSyncLog(provider, account.AccountId, SyncTypes.Transactions));
                return transactions;
            }
            return null;
        }

        private async Task<ExternalOpenBankingAccountTransactionsResponseModel?> GetOpenBankingPendingTransactionsForAccountAsync(OpenBankingProvider provider, SyncTypes syncFlags, ExternalOpenBankingAccount account, string authToken, ConcurrentBag<OpenBankingSynronisation> performedSyncs, IEnumerable<OpenBankingSynronisation> relevantSyncs, OpenBankingTransaction? latestTransactionForAccount)
        {
            if (ShouldSynchronise(syncFlags, relevantSyncs, SyncTypes.Transactions))
            {
                var transactions = await _openBankingApiService.GetAccountPendingTransactionsAsync(account.AccountId, authToken, latestTransactionForAccount?.TransactionTime);
                performedSyncs.Add(CreateSyncLog(provider, account.AccountId, SyncTypes.PendingTransactions));
                return transactions;
            }
            return null;
        }

        private OpenBankingSynronisation CreateSyncLog(OpenBankingProvider provider, string accountId, SyncTypes syncType)
        {
            return new OpenBankingSynronisation()
            {
                Created = DateTime.Now.ToUniversalTime(),
                SyncronisationTime = DateTime.Now.ToUniversalTime(),
                SyncronisationType = (int)syncType,
                ProviderId = provider.Id,
                Provider = provider,
                OpenBankingAccountId = accountId,
                Id = Guid.NewGuid()
            };
        }

        private static bool ShouldSynchronise(SyncTypes syncFlags, IEnumerable<OpenBankingSynronisation> relevantSyncs, SyncTypes typeToCheck)
        {
            return (syncFlags.HasFlag(SyncTypes.All) || syncFlags.HasFlag(typeToCheck)) && !relevantSyncs.Any(x => x.SyncronisationType == (int)typeToCheck);
        }

        private async Task<string> GetAccessTokenAsync(OpenBankingProvider provider)
        {

                var accessToken = await _budgetPlannerDbContext.OpenBankingAccessTokens
                    .AsNoTracking()
                    .Where(x => x.ProviderId == provider.Id)
                    .OrderByDescending(x => x.Created)
                    .FirstOrDefaultAsync();

                if (accessToken.Created.AddSeconds(accessToken.ExpiresIn) > DateTime.Now.ToUniversalTime())
                {
                    return accessToken.AccessToken;
                }
                else
                {
                    var refreshTokenResponse = await _openBankingApiService.GetAccessTokenByRefreshTokenAsync(accessToken.RefreshToken);

                    var newAccessToken = new OpenBankingAccessToken()
                    {
                        AccessToken = refreshTokenResponse.AccessToken,
                        ExpiresIn = refreshTokenResponse.ExpiresIn,
                        ProviderId = provider.Id,
                        RefreshToken = refreshTokenResponse.RefreshToken,
                        Created = DateTime.Now.ToUniversalTime()
                    };

                    await _budgetPlannerDbContext.AddAsync(newAccessToken);
                    await _budgetPlannerDbContext.SaveChangesAsync();

                    return newAccessToken.AccessToken;
                }
        }

        private async Task EnsureAuthenticatedAsync(OpenBankingProvider provider)
        {
            // User hasn't authenticated yet
            if (await _budgetPlannerDbContext.OpenBankingAccessTokens
                .AsNoTracking()
                .Where(x => x.ProviderId == provider.Id)
                .CountAsync() == 0)
            {
                var response = await _openBankingApiService.ExchangeCodeForAccessTokenAsync(provider.AccessCode);

                var accessToken = new OpenBankingAccessToken()
                {
                    AccessToken = response.AccessToken,
                    ExpiresIn = response.ExpiresIn,
                    ProviderId = provider.Id,
                    RefreshToken = response.RefreshToken,
                    Created = DateTime.Now.ToUniversalTime()
                };

                await _budgetPlannerDbContext.AddAsync(accessToken);
            }
        }

        private OpenBankingAccountBalance UpdateOrCreateAccountBalance(
            ExternalOpenBankingAccountBalance balance, OpenBankingAccount account)
        {
            var trackedBalance = account.AccountBalance;

            if (trackedBalance is not null)
            {
                trackedBalance.Current = balance.Current;
                trackedBalance.Available = balance.Available;
                trackedBalance.Updated = DateTime.Now.ToUniversalTime();
            }
            else
            {
                var newAccountBalance = new OpenBankingAccountBalance()
                {
                    Available = balance.Available,
                    Currency = balance.Currency,
                    Current = balance.Current,
                    Created = DateTime.Now.ToUniversalTime(),
                    AccountId = account.Id,
                    Account = account,
                    Id = Guid.NewGuid()
                };
                account.AccountBalance = newAccountBalance;

                return newAccountBalance;
            }
            return trackedBalance;
        }

        private async Task<OpenBankingTransaction> UpdateOrCreateAccountTransaction(ExternalOpenBankingAccountTransaction transaction, OpenBankingAccount account, bool isPendingTransaction, OpenBankingProvider provider)
        {
            account.Transactions ??= [];
            provider.Transactions ??= [];
            
            var trackedTransaction = account.Transactions.FirstOrDefault(x => x.TransactionId == transaction.TransactionId);

            if (trackedTransaction is not null)
            {
                trackedTransaction.TransactionType = transaction.TransactionType;
                trackedTransaction.TransactionId = transaction.TransactionId;
                trackedTransaction.Currency = transaction.Currency;
                trackedTransaction.TransactionCategory = transaction.TransactionCategory;
                trackedTransaction.Amount = trackedTransaction.Amount;
                trackedTransaction.Created = trackedTransaction.Created;
                trackedTransaction.Description = trackedTransaction.Description;
                trackedTransaction.Updated = DateTime.Now.ToUniversalTime();
                trackedTransaction.Pending = isPendingTransaction;
                trackedTransaction.TransactionTime = transaction.Timestamp;
            }
            else
            {
                var newTransaction = new OpenBankingTransaction()
                {
                    TransactionType = transaction.TransactionType,
                    TransactionId = transaction.TransactionId,
                    Currency = transaction.Currency,
                    Amount = transaction.Amount,
                    Description = transaction.Description,
                    Created = DateTime.Now.ToUniversalTime(),
                    TransactionCategory = transaction.TransactionCategory,
                    TransactionTime = transaction.Timestamp,
                    Pending = isPendingTransaction,
                    ProviderId = provider.Id,
                    AccountId = account.Id,
                    Account = account,
                    Id = Guid.NewGuid()

                };
                provider.Transactions.Add(newTransaction);
                account.Transactions.Add(newTransaction);

                newTransaction.Classifications ??= [];

                await foreach (var classification in transaction.TransactionClassification)
                {
                    newTransaction.Classifications.Add(new OpenBankingTransactionClassifications() { Classification = classification, TransactionId = newTransaction.Id, Id = Guid.NewGuid() });
                }

                return newTransaction;
            }
            return trackedTransaction;
        }

        private OpenBankingAccount UpdateOrCreateAccount(ExternalOpenBankingAccount account, OpenBankingProvider provider)
        {
            var trackedAccount = provider.Accounts.FirstOrDefault(x => x.OpenBankingAccountId == account.AccountId);

            if (trackedAccount is not null)
            {
                trackedAccount.Updated = DateTime.Now.ToUniversalTime();
            }
            else
            {
                var newAccount = new OpenBankingAccount()
                {
                    OpenBankingAccountId = account.AccountId,
                    Id = Guid.NewGuid(),
                    AccountType = account.AccountType,
                    Currency = account.Currency,
                    DisplayName = account.DisplayName,
                    Created = DateTime.Now.ToUniversalTime(),
                    ProviderId = provider.Id
                };
                provider.Accounts.Add(newAccount);
                
                return newAccount;
            }
            return trackedAccount;
        }

        private OpenBankingStandingOrder UpdateOrCreateAccountStandingOrder(ExternalOpenBankingAccountStandingOrder standingOrder, OpenBankingAccount account)
        {
            account.StandingOrders ??= [];
            
            var trackedStandingOrder = account.StandingOrders.FirstOrDefault(x => x.Reference == standingOrder.Reference && x.Payee == standingOrder.Payee);

            if (trackedStandingOrder is not null)
            {
                trackedStandingOrder.Frequency = standingOrder.Frequency;
                trackedStandingOrder.Currency = standingOrder.Currency;
                trackedStandingOrder.Reference = standingOrder.Reference;
                trackedStandingOrder.Payee = standingOrder.Payee;
                trackedStandingOrder.FinalPaymentAmount = standingOrder.FinalPaymentAmount;
                trackedStandingOrder.FirstPaymentDate = standingOrder.FirstPaymentDate;
                trackedStandingOrder.NextPaymentDate = standingOrder.NextPaymentDate;
                trackedStandingOrder.FinalPaymentDate = standingOrder.FinalPaymentDate;
                trackedStandingOrder.NextPaymentAmount = standingOrder.NextPaymentAmount;
                trackedStandingOrder.Status = standingOrder.Status;
                trackedStandingOrder.Updated = DateTime.Now.ToUniversalTime();
                trackedStandingOrder.Timestamp = standingOrder.Timestamp;
                trackedStandingOrder.FirstPaymentAmount = standingOrder.FirstPaymentAmount;
            }
            else
            {
                var newStandingOrder = new OpenBankingStandingOrder()
                {
                    Currency = standingOrder.Currency,
                    FinalPaymentAmount = standingOrder.FinalPaymentAmount,
                    FinalPaymentDate = standingOrder.FinalPaymentDate,
                    FirstPaymentAmount = standingOrder.FirstPaymentAmount,
                    FirstPaymentDate = standingOrder.FirstPaymentDate,
                    Frequency = standingOrder.Frequency,
                    NextPaymentAmount = standingOrder.NextPaymentAmount,
                    NextPaymentDate = standingOrder.NextPaymentDate,
                    Payee = standingOrder.Payee,
                    Reference = standingOrder.Reference,
                    Status = standingOrder.Status,
                    Timestamp = standingOrder.Timestamp,
                    Created = DateTime.Now.ToUniversalTime(),
                    AccountId = account.Id,
                    Account = account,
                    Id = Guid.NewGuid()
                };
                account.StandingOrders.Add(newStandingOrder);
                return newStandingOrder;
            }
            return trackedStandingOrder;
        }
        private OpenBankingDirectDebit UpdateOrCreateAccountDirectDebit(ExternalOpenBankingDirectDebit directDebit, OpenBankingAccount account)
        {
            account.DirectDebits ??= [];
            var trackedDirectDebit =  account.DirectDebits.FirstOrDefault(x => x.OpenBankingDirectDebitId == directDebit.DirectDebitId);

            if (trackedDirectDebit is not null)
            {
                trackedDirectDebit.OpenBankingDirectDebitId = directDebit.DirectDebitId;
                trackedDirectDebit.TimeStamp = directDebit.Timestamp;
                trackedDirectDebit.Name = directDebit.Name;
                trackedDirectDebit.Status = directDebit.Status;
                trackedDirectDebit.PreviousPaymentTimeStamp = directDebit.PreviousPaymentTimestamp;
                trackedDirectDebit.PreviousPaymentAmount = directDebit.PreviousPaymentAmount;
                trackedDirectDebit.Currency = directDebit.Currency;
                trackedDirectDebit.Updated = DateTime.Now.ToUniversalTime();
            }
            else
            {
                var newDirectDebit = new OpenBankingDirectDebit()
                {
                    Currency = directDebit.Currency,
                    Name = directDebit.Name,
                    OpenBankingDirectDebitId = directDebit.DirectDebitId,
                    PreviousPaymentAmount = directDebit.PreviousPaymentAmount,
                    PreviousPaymentTimeStamp = directDebit.PreviousPaymentTimestamp,
                    Status = directDebit.Status,
                    TimeStamp = directDebit.Timestamp,
                    Created = DateTime.Now.ToUniversalTime(),
                    AccountId = account.Id,
                    Account = account,
                    Id = Guid.NewGuid()
                };
                
                account.DirectDebits.Add(newDirectDebit);
                return newDirectDebit;
            }
            return trackedDirectDebit;
        }


    }
}
