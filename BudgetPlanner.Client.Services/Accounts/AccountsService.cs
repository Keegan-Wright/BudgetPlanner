using BudgetPlanner.Client.Services.OpenBanking;
using BudgetPlanner.Data.Db;
using BudgetPlanner.Enums;
using BudgetPlanner.Shared.Models.Response.Account;
using BudgetPlanner.Shared.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Client.Services.Accounts
{
    public class AccountsService : InstrumentedService, IAccountsService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
        private readonly IOpenBankingService _openBankingService;

        public AccountsService(BudgetPlannerDbContext budgetPlannerDbContext, IOpenBankingService openBankingService)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
            _openBankingService = openBankingService;
        }

        public async IAsyncEnumerable<AccountAndTransactionsResponse> GetAccountsAndMostRecentTransactionsAsync(int transactionsToReturn, SyncTypes syncFlags = SyncTypes.All, IProgress<string>? progress = null)
        {

            var transaction = GetSentryTransaction(nameof(GetAccountsAndMostRecentTransactionsAsync), "Loading");


            var syncSpan = GetTransactionChild(transaction, "Open Banking Sync", $"Syncronise open banking data for all providers with the following scopes {syncFlags}");

            await _openBankingService.PerformSyncAsync(syncFlags, progress);

            FinishTransactionChildTrace(syncSpan);

            var dataProcessingSpan = GetTransactionChild(transaction, "Open Banking Build For Client", "Loading internal data for the client");

            await foreach (var account in _budgetPlannerDbContext.OpenBankingAccounts.AsNoTracking().AsAsyncEnumerable())
            {
                var accountBalance = await _budgetPlannerDbContext.OpenBankingAccountBalances
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Account.OpenBankingAccountId == account.OpenBankingAccountId);


                var transactions = await _budgetPlannerDbContext.OpenBankingTransactions
                                            .AsNoTracking()
                                            .Where(x => x.Account.OpenBankingAccountId == account.OpenBankingAccountId)
                                            .OrderByDescending(x => x.TransactionTime)
                                            .Take(transactionsToReturn)
                                            .ToListAsync();

                var provider = await _budgetPlannerDbContext.OpenBankingProviders
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == account.ProviderId);

                var response = new AccountAndTransactionsResponse()
                {
                    AccountBalance = accountBalance.Current,
                    AccountName = account.DisplayName,
                    AccountType = account.AccountType,
                    AvailableBalance = accountBalance.Available,
                    Logo = provider.Logo,
                    Transactions = transactions?.Select(transaction => new AccountTransactionResponse()
                    {
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        Status = transaction.Pending ? "Pending" : "Complete",
                        Time = transaction.TransactionTime
                    }).ToAsyncEnumerable()
                };

                yield return response;

            }

            FinishTransactionChildTrace(dataProcessingSpan);

            FinishTransaction(transaction);
        }
    }

}
