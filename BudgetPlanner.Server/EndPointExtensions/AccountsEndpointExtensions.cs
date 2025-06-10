using BudgetPlanner.Server.Services.Accounts;
using BudgetPlanner.Shared.Models.Request.Account;
using BudgetPlanner.Shared.Models.Response.Account;
using Microsoft.AspNetCore.OpenApi;
using System.ComponentModel;

namespace BudgetPlanner.Server.EndPoints;

public static class AccountsEndpointExtensions
{
    /// <summary>
    /// Maps the accounts endpoints to the application
    /// </summary>
    /// <param name="app">The web application instance</param>
    public static void MapAccountsEndPoint(this WebApplication app)
    {
        var accountsGroup = app.MapGroup("/Accounts")
            .WithTags("Accounts")
            .WithSummary("Account Management")
            .WithDescription("Endpoints for managing user accounts and their transactions")
            .RequireAuthorization();

        accountsGroup.MapPost("AccountsAndLatestTransactions",
            ([Description("The request containing the number of transactions to fetch and sync types")] AccountAndTransactionsRequest request, IAccountsService accountsService, HttpContext context) =>
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
            })
            .WithSummary("Get Accounts with Latest Transactions")
            .WithDescription("Retrieves a stream of accounts with their most recent transactions")
            .Accepts<AccountAndTransactionsRequest>("application/json")
            .Produces<IAsyncEnumerable<AccountAndTransactionsResponse>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);
    }
}