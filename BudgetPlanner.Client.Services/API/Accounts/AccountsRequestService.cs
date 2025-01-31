using BudgetPlanner.Client.Data.Db;
using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.Account;
using BudgetPlanner.Shared.Models.Response.Account;

namespace BudgetPlanner.Client.Services;

public class AccountsRequestService : BaseRequestService, IAccountsRequestService
{
    public AccountsRequestService(IHttpClientFactory httpClientFactory, BudgetPlannerDbContext budgetPlannerDbContext) : base(httpClientFactory, budgetPlannerDbContext)
    {
        BaseRoute = "accounts";
    }

    public sealed override string BaseRoute { get; init; }

    public async IAsyncEnumerable<AccountAndTransactionsResponse> GetAccountsAndMostRecentTransactionsAsync(int transactionsToReturn, SyncTypes syncFlags)
    {
        await foreach (var accountAndTransaction in PostAsyncEnumerableAsync<AccountAndTransactionsRequest, AccountAndTransactionsResponse>("AccountsAndLatestTransactions",new()
                       {
                           SyncTypes = syncFlags,
                           TransactionsCount = transactionsToReturn,
                       } ))
        {
            yield return accountAndTransaction;
        }
    }
}