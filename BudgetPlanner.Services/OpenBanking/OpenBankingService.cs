using BudgetPlanner.Data.Db;
using BudgetPlanner.Data.Models;
using BudgetPlanner.External.Services.Models.OpenBanking;
using BudgetPlanner.States;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

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


        public IAsyncEnumerable<ExternalOpenBankingAccount> GetOpenBankingAccountsAsync(string providerId)
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<ExternalOpenBankingAccount> GetOpenBankingAccountsAsync()
        {
            if (ApplicationState.HasInternetConnection)
            {
                await foreach (var provider in GetOpenBankingProvidersAsync())
                {
                    await EnsureAuthenticatedAsync(provider);

                    var authToken = await GetAccessTokenAsync(provider);

                    var accounts = await _openBankingApiService.GetAllAccountsAsync(authToken);

                    await foreach (var account in accounts.Results)
                    {
                        await UpdateOrCreateAccount(account, provider.OpenBankingProviderId);

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

                await EnsureAuthenticatedAsync(provider);

                var authToken = await GetAccessTokenAsync(provider);

                var accountBalance = await _openBankingApiService.GetAccountBalanceAsync(accountId, authToken);

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
        public async IAsyncEnumerable<ExternalOpenBankingAccountStandingOrder> GetOpenBankingAccountStandingOrdersAsync(string openBankingProviderId, string accountId)
        {
            if (ApplicationState.HasInternetConnection)
            {
                var provider = await _budgetPlannerDbContext.OpenBankingProviders.FirstAsync(x => x.OpenBankingProviderId == openBankingProviderId);

                await EnsureAuthenticatedAsync(provider);

                var authToken = await GetAccessTokenAsync(provider);

                var standingOrders = await _openBankingApiService.GetAccountStandingOrdersAsync(accountId, authToken);

                await foreach (var standingOrder in standingOrders.Results)
                {
                    await UpdateOrCreateAccountStandingOrder(standingOrder, accountId, true);
                    yield return standingOrder;
                }

            }
        }

        public async IAsyncEnumerable<ExternalOpenBankingDirectDebit> GetOpenBankingAccountDirectDebitsAsync(string openBankingProviderId, string accountId)
        {
            if (ApplicationState.HasInternetConnection)
            {
                var provider = await _budgetPlannerDbContext.OpenBankingProviders.FirstAsync(x => x.OpenBankingProviderId == openBankingProviderId);

                await EnsureAuthenticatedAsync(provider);

                var authToken = await GetAccessTokenAsync(provider);

                var directDebits = await _openBankingApiService.GetAccountDirectDebitsAsync(accountId, authToken);

                await foreach (var directDebit in directDebits.Results)
                {
                    await UpdateOrCreateAccountDirectDebit(directDebit, accountId, true);
                    yield return directDebit;
                }

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

        private async Task UpdateOrCreateAccountStandingOrder(ExternalOpenBankingAccountStandingOrder standingOrder, string accountId, bool v)
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
        private async Task UpdateOrCreateAccountDirectDebit(ExternalOpenBankingDirectDebit directDebit, string accountId, bool v)
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
