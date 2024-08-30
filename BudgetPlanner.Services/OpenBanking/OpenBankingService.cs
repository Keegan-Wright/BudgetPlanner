using BudgetPlanner.Data.Db;
using BudgetPlanner.Data.Models;
using BudgetPlanner.External.Services.Models.OpenBanking;
using BudgetPlanner.External.Services.OpenBanking;
using BudgetPlanner.Models.Request.OpenBanking;
using BudgetPlanner.States;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Text;

namespace BudgetPlanner.Services.OpenBanking
{
    public class OpenBankingService : IOpenBankingService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
        private readonly IOpenBankingApiService _openBankingApiService;

        public OpenBankingService(BudgetPlannerDbContext budgetPlannerDbContext, IOpenBankingApiService openBankingApiService)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
            _openBankingApiService = openBankingApiService;
        }


        public async IAsyncEnumerable<ExternalOpenBankingAccount> GetOpenBankingAccountsAsync(string providerId)
        {
            if (ApplicationState.HasInternetConnection)
            {
                await foreach (var provider in GetOpenBankingProvidersAsync(providerId))
                {
                    await foreach (var account in ProcessAccountItem(provider))
                    {
                        yield return account;
                    }
                }
            }
        }

        public async IAsyncEnumerable<ExternalOpenBankingAccount> GetOpenBankingAccountsAsync()
        {
            if (ApplicationState.HasInternetConnection)
            {
                await foreach (var provider in GetOpenBankingProvidersAsync())
                {
                    await foreach (var account in ProcessAccountItem(provider))
                    {
                        yield return account;
                    }
                }
            }
        }

        public async IAsyncEnumerable<ExternalOpenBankingAccountBalance> GetOpenBankingAccountBalanceAsync(string openBankingProviderId, string accountId)
        {
            if (ApplicationState.HasInternetConnection)
            {
                var provider = await _budgetPlannerDbContext.OpenBankingProviders.FirstAsync(x => x.OpenBankingProviderId.ToLower() == openBankingProviderId.ToLower());

                var providerScopes = await _budgetPlannerDbContext.OpenBankingProviderScopes.Where(x => x.ProviderId == provider.Id).ToListAsync();


                if(!providerScopes.Any(x => x.Scope.ToLower() == "balance"))
                {
                    yield break;
                }


                await EnsureAuthenticatedAsync(provider);

                var authToken = await GetAccessTokenAsync(provider);

                var accountBalance = await _openBankingApiService.GetAccountBalanceAsync(accountId, authToken);

                if(accountBalance.Results is null)
                {
                    yield break;
                }

                await foreach (var balance in accountBalance.Results)
                {
                    await UpdateOrCreateAccountBalance(balance, accountId);
                    yield return balance;
                }

            }
        }

        public async IAsyncEnumerable<ExternalOpenBankingAccountTransaction> GetOpenBankingAccountTransactionsAsync(string openBankingProviderId, string accountId)
        {
            if (ApplicationState.HasInternetConnection)
            {
                var provider = await _budgetPlannerDbContext.OpenBankingProviders.FirstAsync(x => x.OpenBankingProviderId == openBankingProviderId);

                var providerScopes = await _budgetPlannerDbContext.OpenBankingProviderScopes.Where(x => x.ProviderId == provider.Id).ToListAsync();


                if (!providerScopes.Any(x => x.Scope.ToLower() == "transactions"))
                {
                    yield break;
                }

                await EnsureAuthenticatedAsync(provider);

                var authToken = await GetAccessTokenAsync(provider);

                var transactions = await _openBankingApiService.GetAccountTransactionsAsync(accountId, authToken);

                await foreach (var transaction in transactions.Results)
                {
                    await UpdateOrCreateAccountTransaction(transaction, accountId, false);

                    yield return transaction;
                }

            }
        }

        public async IAsyncEnumerable<ExternalOpenBankingAccountTransaction> GetOpenBankingAccountPendingTransactionsAsync(string openBankingProviderId, string accountId)
        {
            if (ApplicationState.HasInternetConnection)
            {
                var provider = await _budgetPlannerDbContext.OpenBankingProviders.FirstAsync(x => x.OpenBankingProviderId == openBankingProviderId);
                var providerScopes = await _budgetPlannerDbContext.OpenBankingProviderScopes.Where(x => x.ProviderId == provider.Id).ToListAsync();


                if (!providerScopes.Any(x => x.Scope.ToLower() == "transactions"))
                {
                    yield break;
                }
                await EnsureAuthenticatedAsync(provider);

                var authToken = await GetAccessTokenAsync(provider);

                var transactions = await _openBankingApiService.GetAccountPendingTransactionsAsync(accountId, authToken);

                await foreach (var transaction in transactions.Results)
                {
                    await UpdateOrCreateAccountTransaction(transaction, accountId, true);
                    yield return transaction;
                }

            }
        }

        public async IAsyncEnumerable<OpenBankingProvider> GetOpenBankingProvidersAsync()
        {
            await foreach (var provider in _budgetPlannerDbContext.OpenBankingProviders.AsAsyncEnumerable())
            {
                yield return provider;
            }
        }

        public async IAsyncEnumerable<OpenBankingProvider> GetOpenBankingProvidersAsync(string providerId)
        {
            await foreach (var provider in _budgetPlannerDbContext.OpenBankingProviders.Where(x => x.OpenBankingProviderId == providerId).AsAsyncEnumerable())
            {
                yield return provider;
            }
        }
        public async IAsyncEnumerable<ExternalOpenBankingAccountStandingOrder> GetOpenBankingAccountStandingOrdersAsync(string openBankingProviderId, string accountId)
        {
            if (ApplicationState.HasInternetConnection)
            {
                var provider = await _budgetPlannerDbContext.OpenBankingProviders.FirstAsync(x => x.OpenBankingProviderId == openBankingProviderId);
                var providerScopes = await _budgetPlannerDbContext.OpenBankingProviderScopes.Where(x => x.ProviderId == provider.Id).ToListAsync();


                if (!providerScopes.Any(x => x.Scope.ToLower() == "standing_orders"))
                {
                    yield break;
                }
                await EnsureAuthenticatedAsync(provider);

                var authToken = await GetAccessTokenAsync(provider);

                var standingOrders = await _openBankingApiService.GetAccountStandingOrdersAsync(accountId, authToken);


                await foreach (var standingOrder in standingOrders.Results)
                {
                    await UpdateOrCreateAccountStandingOrder(standingOrder, accountId);
                    yield return standingOrder;
                }

            }
        }

        public async IAsyncEnumerable<ExternalOpenBankingDirectDebit> GetOpenBankingAccountDirectDebitsAsync(string openBankingProviderId, string accountId)
        {
            if (ApplicationState.HasInternetConnection)
            {
                var provider = await _budgetPlannerDbContext.OpenBankingProviders.FirstAsync(x => x.OpenBankingProviderId == openBankingProviderId);
                var providerScopes = await _budgetPlannerDbContext.OpenBankingProviderScopes.Where(x => x.ProviderId == provider.Id).ToListAsync();


                if (!providerScopes.Any(x => x.Scope.ToLower() == "direct_debits"))
                {
                    yield break;
                }
                await EnsureAuthenticatedAsync(provider);

                var authToken = await GetAccessTokenAsync(provider);

                var directDebits = await _openBankingApiService.GetAccountDirectDebitsAsync(accountId, authToken);

                await foreach (var directDebit in directDebits.Results)
                {
                    await UpdateOrCreateAccountDirectDebit(directDebit, accountId);
                    yield return directDebit;
                }

            }
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

        public async Task<bool> AddVendorViaAccessCode(string accessCode)
        {
            var providerAccessToken = await _openBankingApiService.ExchangeCodeForAccessTokenAsync(accessCode);
            var providerInformation = await _openBankingApiService.GetProviderInformation(providerAccessToken.AccessToken);

            await CreateNewProvider(accessCode, providerAccessToken, providerInformation);

            return true;
        }

        private async Task CreateNewProvider(string accessCode, ExternalOpenBankingAccessResponseModel providerAccessToken, ExternalOpenBankingAccountConnectionResponseModel providerInformation)
        {
            await foreach (var externalProvider in providerInformation.Results)
            {
                var provider = new OpenBankingProvider()
                {
                    AccessCode = accessCode,
                    Name = externalProvider.Provider.DisplayName,
                    OpenBankingProviderId = externalProvider.Provider.ProviderId,
                    Created = DateTime.Now,
                    Logo = []
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

                await foreach(var scope in externalProvider.Scopes)
                {
                    var providerScope = new OpenBankingProviderScopes()
                    {
                        Scope = scope,
                        Created = DateTime.Now,
                        ProviderId = provider.Id
                    };

                    await _budgetPlannerDbContext.AddAsync(scope);
                    await _budgetPlannerDbContext.SaveChangesAsync();
                }

                await _budgetPlannerDbContext.AddAsync(accessToken);
                await _budgetPlannerDbContext.SaveChangesAsync();


                await BulkLoadProvider(externalProvider.Provider.ProviderId);
            }
        }

        private async Task BulkLoadProvider(string providerId)
        {
            await foreach (var account in GetOpenBankingAccountsAsync(providerId))
            {
                await foreach (var _ in GetOpenBankingAccountStandingOrdersAsync(providerId, account.AccountId))
                {

                }

                await foreach (var _ in GetOpenBankingAccountDirectDebitsAsync(providerId, account.AccountId))
                {

                }

                await foreach (var _ in GetOpenBankingAccountBalanceAsync(providerId, account.AccountId))
                {

                }
                await foreach (var _ in GetOpenBankingAccountPendingTransactionsAsync(providerId, account.AccountId))
                {

                }
                await foreach (var _ in GetOpenBankingAccountTransactionsAsync(providerId, account.AccountId))
                {

                }
            }
        }
        private async IAsyncEnumerable<ExternalOpenBankingAccount> ProcessAccountItem(OpenBankingProvider provider)
        {
            var providerScopes = await _budgetPlannerDbContext.OpenBankingProviderScopes.Where(x => x.ProviderId == provider.Id).ToListAsync();

            if (!providerScopes.Any(x => x.Scope.ToLower() == "accounts"))
            {
                yield break;
            }

            await EnsureAuthenticatedAsync(provider);

            var authToken = await GetAccessTokenAsync(provider);

            var accounts = await _openBankingApiService.GetAllAccountsAsync(authToken);

            await foreach (var account in accounts.Results)
            {
                await UpdateOrCreateAccount(account, provider.OpenBankingProviderId);

                yield return account;
            }
        }


        private async Task<string> GetAccessTokenAsync(OpenBankingProvider provider)
        {

            if (ApplicationState.HasInternetConnection)
            {
                var accessToken = await _budgetPlannerDbContext.OpenBankingAccessTokens.Where(x => x.ProviderId == provider.Id).OrderByDescending(x => x.Created).FirstOrDefaultAsync();

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
            if (await _budgetPlannerDbContext.OpenBankingAccessTokens.Where(x => x.ProviderId == provider.Id).CountAsync() == 0)
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
                await _budgetPlannerDbContext.SaveChangesAsync();
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
            await _budgetPlannerDbContext.SaveChangesAsync();
        }

        private async Task UpdateOrCreateAccountTransaction(ExternalOpenBankingAccountTransaction transaction, string accountId, bool isPendingTransaction)
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
                };

                await _budgetPlannerDbContext.AddAsync(newTransaction);
            }
            await _budgetPlannerDbContext.SaveChangesAsync();
        }

        private async Task UpdateOrCreateAccount(ExternalOpenBankingAccount account, string openBankingProviderId)
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
                    OpenBankingProviderId = openBankingProviderId
                };

                await _budgetPlannerDbContext.AddAsync(newAccount);
            }
            await _budgetPlannerDbContext.SaveChangesAsync();
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
            await _budgetPlannerDbContext.SaveChangesAsync();
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
            await _budgetPlannerDbContext.SaveChangesAsync();
        }

    }
}
