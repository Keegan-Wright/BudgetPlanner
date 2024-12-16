using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Response.Account;

namespace BudgetPlanner.Server.Services.Accounts
{
    public interface IAccountsService
    {
        IAsyncEnumerable<AccountAndTransactionsResponse> GetAccountsAndMostRecentTransactionsAsync(int transactionsToReturn, SyncTypes syncFlags = SyncTypes.All);
    }

}
