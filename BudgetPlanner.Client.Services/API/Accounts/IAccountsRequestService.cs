using BudgetPlanner.Shared.Enums;

namespace BudgetPlanner.Client.Services;

public interface IAccountsRequestService
{
    IAsyncEnumerable<BudgetPlanner.Shared.Models.Response.Account.AccountAndTransactionsResponse>
        GetAccountsAndMostRecentTransactionsAsync(int transactionsToReturn, SyncTypes syncFlags,
            Progress<string> progress);
}