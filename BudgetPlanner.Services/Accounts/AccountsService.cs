using BudgetPlanner.Data.Db;
using BudgetPlanner.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Services.Accounts
{
    public class AccountsService : IAccountsService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;

        public AccountsService(BudgetPlannerDbContext budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
        }

        public async IAsyncEnumerable<AccountAndTransactionsResponse> GetAccountsAndMostRecentTransactions(int transactionsToReturn)
        {
            await foreach (var account in _budgetPlannerDbContext.OpenBankingAccounts.AsAsyncEnumerable())
            {
                var accountBalance = await _budgetPlannerDbContext.OpenBankingAccountBalances.FirstOrDefaultAsync(x => x.OpenBankingAccountId == account.OpenBankingAccountId);


                var transactions = await _budgetPlannerDbContext.OpenBankingTransactions
                                            .Where(x => x.OpenBankingAccountId == account.OpenBankingAccountId)
                                            .OrderByDescending(x => x.TransactionTime)
                                            .Take(transactionsToReturn)
                                            .ToListAsync();

                var provider = await _budgetPlannerDbContext.OpenBankingProviders.FirstOrDefaultAsync(x => x.OpenBankingProviderId == account.OpenBankingProviderId);

                var response = new AccountAndTransactionsResponse()
                {
                    AccountBalance = accountBalance.Current,
                    AccountName = account.DisplayName,
                    AccountType = account.AccountType,
                    AvailableBalance = accountBalance.Available,
                    Logo = provider.Logo,
                    Transactions = transactions.Select(transaction => new AccountTransactionResponse()
                    {
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        Status = transaction.Pending ? "Pending" : "Complete",
                        Time = transaction.TransactionTime
                    }).ToAsyncEnumerable()
                };

                yield return response;

            }
        }
    }

}
