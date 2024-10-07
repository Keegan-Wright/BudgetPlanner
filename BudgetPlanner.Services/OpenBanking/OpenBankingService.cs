using BudgetPlanner.Data.Db;
using BudgetPlanner.Data.Models;
using BudgetPlanner.Enums;
using BudgetPlanner.Extensions;
using BudgetPlanner.External.Services.Models.OpenBanking;
using BudgetPlanner.External.Services.OpenBanking;
using BudgetPlanner.Handlers;
using BudgetPlanner.Models.Request.OpenBanking;
using BudgetPlanner.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Json.Internal;
using Sentry;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;

namespace BudgetPlanner.Services.OpenBanking
{
    public class OpenBankingService : IOpenBankingService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
        private readonly IOpenBankingApiService _openBankingApiService;
        private readonly int _syncMins = 5;

        public OpenBankingService(BudgetPlannerDbContext budgetPlannerDbContext, IOpenBankingApiService openBankingApiService)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
            _openBankingApiService = openBankingApiService;
        }



        public async IAsyncEnumerable<OpenBankingProvider> GetOpenBankingProvidersAsync()
        {
            await foreach (var provider in _budgetPlannerDbContext.OpenBankingProviders.AsNoTracking().AsAsyncEnumerable())
            {
                yield return provider;
            }
        }

        public async Task<OpenBankingProvider> GetOpenBankingProviderByIdAsync(string providerId)
        {
            var provider = await _budgetPlannerDbContext.OpenBankingProviders.FirstAsync(x => x.OpenBankingProviderId == providerId);

            return provider;
        }



        public async IAsyncEnumerable<ExternalOpenBankingProvider> GetOpenBankingProvidersForClientAsync()
        {
            if (ApplicationState.HasInternetConnection)
            {
                await foreach (var provider in _openBankingApiService.GetAvailableProvidersAsync())
                {
                    yield return provider;
                }
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

        public async Task PerformSyncAsync(SyncTypes syncFlags, IProgress<string>? progress = null)
        {
            if (ApplicationState.HasInternetConnection)
            {

                try
                {
                    await foreach (var provider in GetOpenBankingProvidersAsync())
                    {
                        progress?.Report($"Processing your {provider.Name} banking information...");
                        await BulkLoadProviderAsync(provider, syncFlags);
                    }
                    await _budgetPlannerDbContext.SaveChangesAsync();

                }
                catch (Exception ex) { 
                    ErrorHandler.HandleError(ex);
                }
            }
        }


        private async Task CreateNewProvider(string accessCode, ExternalOpenBankingAccessResponseModel providerAccessToken, ExternalOpenBankingAccountConnectionResponseModel providerInformation)
        {
            await foreach (var externalProvider in providerInformation.Results)
            {
                try
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
                        Created = DateTime.Now,
                        Logo = ByteArrayHelpers.ConvertSvgStreamToPngStream(ms.ToArray()).ToArray()

                    };
                    await _budgetPlannerDbContext.AddAsync(provider);

                    await _budgetPlannerDbContext.SaveChangesAsync();

                    var accessToken = new OpenBankingAccessToken()
                    {
                        AccessToken = providerAccessToken.AccessToken,
                        ProviderId = provider.Id,
                        ExpiresIn = providerAccessToken.ExpiresIn,
                        RefreshToken = providerAccessToken.RefreshToken,
                        Created = DateTime.Now
                    };

                    await foreach (var scope in externalProvider.Scopes)
                    {
                        var providerScope = new OpenBankingProviderScopes()
                        {
                            Scope = scope,
                            Created = DateTime.Now,
                            ProviderId = provider.Id
                        };

                        await _budgetPlannerDbContext.AddAsync(providerScope);
                        await _budgetPlannerDbContext.SaveChangesAsync();
                    }

                    await _budgetPlannerDbContext.AddAsync(accessToken);
                    await _budgetPlannerDbContext.SaveChangesAsync();


                    await BulkLoadProviderAsync(provider);

                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError(ex);
                }
            }
        }

        private async Task BulkLoadProviderAsync(OpenBankingProvider provider, SyncTypes syncFlags = SyncTypes.All)
        {

            if (!ApplicationState.HasInternetConnection)
                return;

            var providerScopes = await _budgetPlannerDbContext.OpenBankingProviderScopes
                .AsNoTracking()
                .Where(x => x.ProviderId == provider.Id).ToListAsync();

            var providerSyncs = await _budgetPlannerDbContext.OpenBankingSyncronisations
                                    .AsNoTracking()
                                   .Where(x => x.SyncronisationTime > DateTime.Now.AddMinutes(-_syncMins)
                                    && x.ProviderId == provider.Id)
                                   .OrderByDescending(x => x.SyncronisationTime).ToListAsync();

            var transactionsForProvider = await _budgetPlannerDbContext.OpenBankingTransactions
                .AsNoTracking()
                .Where(x => x.ProviderId == provider.Id)
                .OrderByDescending(x => x.TransactionTime)
                .GroupBy(x => x.OpenBankingAccountId)
                .Select(x => x.FirstOrDefault())
                .ToListAsync();


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

                var relevantSyncs = providerSyncs.Where(x => x.AccountId == account.AccountId);


                var standingOrdersTask = GetOpenBankingStandingOrdersForAccountAsync(provider, syncFlags, account, authToken, performedSyncs, relevantSyncs);
                var directDebitsTask = GetOpenBankingDirectDebitsForAccountAsync(provider, syncFlags, account, authToken, performedSyncs, relevantSyncs);
                var balanceTask = GetOpenBankingAccountBalanceAsync(provider, syncFlags, account, authToken, performedSyncs, relevantSyncs);

                var latestTransactionForAccount = transactionsForProvider.FirstOrDefault(x => x?.OpenBankingAccountId == account.AccountId);

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

            await foreach (var account in providerAccounts.ToAsyncEnumerable())
            {
                await UpdateOrCreateAccount(account, provider.Id);
            }

            await foreach (var balance in providerBalances.ToAsyncEnumerable())
            {
                await UpdateOrCreateAccountBalance(balance.Balances, balance.AccountId);
            }

            await foreach (var standingOrder in providerStandingOrders.ToAsyncEnumerable())
            {
                await UpdateOrCreateAccountStandingOrder(standingOrder.StandingOrders, standingOrder.AccountId);
            }

            await foreach (var directDebit in providerDirectDebits.ToAsyncEnumerable())
            {
                await UpdateOrCreateAccountDirectDebit(directDebit.DirectDebits, directDebit.AccountId);
            }

            await foreach (var transaction in providerTransactions.ToAsyncEnumerable())
            {
                await UpdateOrCreateAccountTransaction(transaction.Transactions, transaction.AccountId, false, provider.Id);
            }

            await foreach (var transaction in providerPendingTransactions.ToAsyncEnumerable())
            {
                await UpdateOrCreateAccountTransaction(transaction.Transactions, transaction.AccountId, true, provider.Id);
            }

            await _budgetPlannerDbContext.AddRangeAsync(performedSyncs);
            await _budgetPlannerDbContext.SaveChangesAsync();

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
                performedSyncs.Add(CreateSyncLog(provider.Id, account.AccountId, SyncTypes.StandingOrders));
                return standingOrders;
            }

            return null;
        }

        private async Task<ExternalOpenBankingAccountDirectDebitsResponseModel?> GetOpenBankingDirectDebitsForAccountAsync(OpenBankingProvider provider, SyncTypes syncFlags, ExternalOpenBankingAccount account, string authToken, ConcurrentBag<OpenBankingSynronisation> performedSyncs, IEnumerable<OpenBankingSynronisation> relevantSyncs)
        {
            if (ShouldSynchronise(syncFlags, relevantSyncs, SyncTypes.DirectDebits))
            {
                var directDebits = await _openBankingApiService.GetAccountDirectDebitsAsync(account.AccountId, authToken);
                performedSyncs.Add(CreateSyncLog(provider.Id, account.AccountId, SyncTypes.DirectDebits));
                return directDebits;
            }


            return null;
        }

        private async Task<ExternalOpenBankingGetAccountBalanceResponseModel?> GetOpenBankingAccountBalanceAsync(OpenBankingProvider provider, SyncTypes syncFlags, ExternalOpenBankingAccount account, string authToken, ConcurrentBag<OpenBankingSynronisation> performedSyncs, IEnumerable<OpenBankingSynronisation> relevantSyncs)
        {
            if (ShouldSynchronise(syncFlags, relevantSyncs, SyncTypes.Balance))
            {
                var balance = await _openBankingApiService.GetAccountBalanceAsync(account.AccountId, authToken);
                performedSyncs.Add(CreateSyncLog(provider.Id, account.AccountId, SyncTypes.Balance));
                return balance;
            }

            return null;
        }

        private async Task<ExternalOpenBankingAccountTransactionsResponseModel?> GetOpenBankingTransactionsForAccountAsync(OpenBankingProvider provider, SyncTypes syncFlags, ExternalOpenBankingAccount account, string authToken, ConcurrentBag<OpenBankingSynronisation> performedSyncs, IEnumerable<OpenBankingSynronisation> relevantSyncs, OpenBankingTransaction? latestTransactionForAccount)
        {
            if (ShouldSynchronise(syncFlags, relevantSyncs, SyncTypes.Transactions))
            {
                var transactions = await _openBankingApiService.GetAccountTransactionsAsync(account.AccountId, authToken, latestTransactionForAccount?.TransactionTime);
                performedSyncs.Add(CreateSyncLog(provider.Id, account.AccountId, SyncTypes.Transactions));
                return transactions;
            }
            return null;
        }

        private async Task<ExternalOpenBankingAccountTransactionsResponseModel?> GetOpenBankingPendingTransactionsForAccountAsync(OpenBankingProvider provider, SyncTypes syncFlags, ExternalOpenBankingAccount account, string authToken, ConcurrentBag<OpenBankingSynronisation> performedSyncs, IEnumerable<OpenBankingSynronisation> relevantSyncs, OpenBankingTransaction? latestTransactionForAccount)
        {
            if (ShouldSynchronise(syncFlags, relevantSyncs, SyncTypes.Transactions))
            {
                var transactions = await _openBankingApiService.GetAccountPendingTransactionsAsync(account.AccountId, authToken, latestTransactionForAccount?.TransactionTime);
                performedSyncs.Add(CreateSyncLog(provider.Id, account.AccountId, SyncTypes.PendingTransactions));
                return transactions;
            }
            return null;
        }









        private OpenBankingSynronisation CreateSyncLog(Guid providerId, string accountId, SyncTypes syncType)
        {
            return new OpenBankingSynronisation()
            {
                Created = DateTime.Now,
                SyncronisationTime = DateTime.Now,
                SyncronisationType = (int)syncType,
                ProviderId = providerId,
                AccountId = accountId
            };
        }

        private static bool ShouldSynchronise(SyncTypes syncFlags, IEnumerable<OpenBankingSynronisation> relevantSyncs, SyncTypes typeToCheck)
        {
            return (syncFlags.HasFlag(SyncTypes.All) || syncFlags.HasFlag(typeToCheck)) && !relevantSyncs.Any(x => x.SyncronisationType == (int)typeToCheck);
        }

        private async Task<string> GetAccessTokenAsync(OpenBankingProvider provider)
        {

            if (ApplicationState.HasInternetConnection)
            {
                var accessToken = await _budgetPlannerDbContext.OpenBankingAccessTokens
                    .AsNoTracking()
                    .Where(x => x.ProviderId == provider.Id)
                    .OrderByDescending(x => x.Created)
                    .FirstOrDefaultAsync();

                if (accessToken.Created.AddSeconds(accessToken.ExpiresIn) > DateTime.Now)
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
                        Created = DateTime.Now
                    };

                    await _budgetPlannerDbContext.AddAsync(newAccessToken);
                    await _budgetPlannerDbContext.SaveChangesAsync();

                    return newAccessToken.AccessToken;
                }
            }
            return string.Empty;
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
                    Created = DateTime.Now
                };

                await _budgetPlannerDbContext.AddAsync(accessToken);
            }
        }

        private async Task UpdateOrCreateAccountBalance(ExternalOpenBankingAccountBalance balance, string accountId)
        {
            var trackedBalance = await _budgetPlannerDbContext.OpenBankingAccountBalances.FirstOrDefaultAsync(x => x.OpenBankingAccountId == accountId);

            if (trackedBalance is not null)
            {
                trackedBalance.Current = balance.Current;
                trackedBalance.Available = balance.Available;
                trackedBalance.Updated = DateTime.Now;
            }
            else
            {
                var newAccountBalance = new OpenBankingAccountBalance()
                {
                    Available = balance.Available,
                    Currency = balance.Currency,
                    Current = balance.Current,
                    OpenBankingAccountId = accountId,
                    Created = DateTime.Now
                };

                await _budgetPlannerDbContext.AddAsync(newAccountBalance);
            }
            //await _budgetPlannerDbContext.SaveChangesAsync();
        }

        private async Task UpdateOrCreateAccountTransaction(ExternalOpenBankingAccountTransaction transaction, string accountId, bool isPendingTransaction, Guid providerId)
        {
            var trackedTransaction = await _budgetPlannerDbContext.OpenBankingTransactions.FirstOrDefaultAsync(x => x.TransactionId == transaction.TransactionId);

            if (trackedTransaction is not null)
            {
                trackedTransaction.TransactionType = transaction.TransactionType;
                trackedTransaction.TransactionId = transaction.TransactionId;
                trackedTransaction.Currency = transaction.Currency;
                trackedTransaction.TransactionCategory = transaction.TransactionCategory;
                trackedTransaction.Amount = trackedTransaction.Amount;
                trackedTransaction.Created = trackedTransaction.Created;
                trackedTransaction.Description = trackedTransaction.Description;
                trackedTransaction.OpenBankingAccountId = accountId;
                trackedTransaction.Updated = DateTime.Now;
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
                    OpenBankingAccountId = accountId,
                    Created = DateTime.Now,
                    TransactionCategory = transaction.TransactionCategory,
                    TransactionTime = transaction.Timestamp,
                    Pending = isPendingTransaction,
                    ProviderId = providerId
                };

                await _budgetPlannerDbContext.AddAsync(newTransaction);
            }
            await _budgetPlannerDbContext.SaveChangesAsync();
        }

        private async Task UpdateOrCreateAccount(ExternalOpenBankingAccount account, Guid providerId)
        {
            var trackedAccount = await _budgetPlannerDbContext.OpenBankingAccounts.FirstOrDefaultAsync(x => x.OpenBankingAccountId == account.AccountId);

            if (trackedAccount is not null)
            {
                trackedAccount.Updated = DateTime.Now;
            }
            else
            {
                var newAccount = new OpenBankingAccount()
                {
                    OpenBankingAccountId = account.AccountId,
                    AccountType = account.AccountType,
                    Currency = account.Currency,
                    DisplayName = account.DisplayName,
                    Created = DateTime.Now,
                    ProviderId = providerId
                };

                await _budgetPlannerDbContext.AddAsync(newAccount);
            }
            //await _budgetPlannerDbContext.SaveChangesAsync();
        }

        private async Task UpdateOrCreateAccountStandingOrder(ExternalOpenBankingAccountStandingOrder standingOrder, string accountId)
        {
            var trackedStandingOrder = await _budgetPlannerDbContext.OpenBankingStandingOrders.FirstOrDefaultAsync(x => x.Reference == standingOrder.Reference && x.Payee == standingOrder.Payee);

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
                trackedStandingOrder.Updated = DateTime.Now;
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
                    Created = DateTime.Now
                };

                await _budgetPlannerDbContext.AddAsync(newStandingOrder);
            }
            //await _budgetPlannerDbContext.SaveChangesAsync();
        }
        private async Task UpdateOrCreateAccountDirectDebit(ExternalOpenBankingDirectDebit directDebit, string accountId)
        {
            var trackedDirectDebit = await _budgetPlannerDbContext.OpenBankingDirectDebits.FirstOrDefaultAsync(x => x.OpenBankingDirectDebitId == directDebit.DirectDebitId);

            if (trackedDirectDebit is not null)
            {
                trackedDirectDebit.OpenBankingDirectDebitId = directDebit.DirectDebitId;
                trackedDirectDebit.TimeStamp = directDebit.Timestamp;
                trackedDirectDebit.Name = directDebit.Name;
                trackedDirectDebit.Status = directDebit.Status;
                trackedDirectDebit.PreviousPaymentTimeStamp = directDebit.PreviousPaymentTimestamp;
                trackedDirectDebit.PreviousPaymentAmount = directDebit.PreviousPaymentAmount;
                trackedDirectDebit.Currency = directDebit.Currency;
                trackedDirectDebit.Updated = DateTime.Now;
            }
            else
            {
                var newStandingOrder = new OpenBankingDirectDebit()
                {
                    Currency = directDebit.Currency,
                    Name = directDebit.Name,
                    OpenBankingDirectDebitId = directDebit.DirectDebitId,
                    PreviousPaymentAmount = directDebit.PreviousPaymentAmount,
                    PreviousPaymentTimeStamp = directDebit.PreviousPaymentTimestamp,
                    Status = directDebit.Status,
                    TimeStamp = directDebit.Timestamp,
                    Created = DateTime.Now
                };

                await _budgetPlannerDbContext.AddAsync(newStandingOrder);
            }
            //await _budgetPlannerDbContext.SaveChangesAsync();
        }


    }
}
