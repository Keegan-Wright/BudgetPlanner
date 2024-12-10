using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Response.Account;

namespace BudgetPlanner.Client.Services;

public class AccountsRequestService : BaseRequestService, IAccountsRequestService
{
    public AccountsRequestService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        BaseRoute = "accounts";
    }

    public sealed override string BaseRoute { get; init; }

    public IAsyncEnumerable<AccountAndTransactionsResponse> GetAccountsAndMostRecentTransactionsAsync(int transactionsToReturn, SyncTypes syncFlags,
        Progress<string> progress)
    {
        throw new NotImplementedException();
    }
}