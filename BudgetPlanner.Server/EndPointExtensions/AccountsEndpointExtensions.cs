using BudgetPlanner.Server.Services.Accounts;
using BudgetPlanner.Shared.Models.Request.Account;
using BudgetPlanner.Shared.Models.Response.Account;

namespace BudgetPlanner.Server.EndPoints;

public static class AccountsEndpointExtensions
{
    public static void MapAccountsEndPoint(this WebApplication app)
    {
        var accountsGroup = app.MapGroup("/Accounts").RequireAuthorization();

        accountsGroup.MapPost("AccountsAndLatestTransactions",
            async (AccountAndTransactionsRequest request, IAccountsService accountsService, HttpContext context) =>
            {
                async IAsyncEnumerable<AccountAndTransactionsResponse> AccountsAndLatestTransactionsStream()
                {
                    await foreach (var accountAndTransaction in accountsService
                                       .GetAccountsAndMostRecentTransactionsAsync(
                                           request.TransactionsCount, request.SyncTypes))
                    {
                        yield return accountAndTransaction;
                    }
                }

                return AccountsAndLatestTransactionsStream();

            });
    }
}