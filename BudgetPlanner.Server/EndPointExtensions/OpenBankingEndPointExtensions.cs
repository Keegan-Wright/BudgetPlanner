using BudgetPlanner.Server.Services.Accounts;
using BudgetPlanner.Server.Services.Dashboard;
using BudgetPlanner.Shared.Models.Request.Account;
using BudgetPlanner.Shared.Models.Request.Dashboard;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.Account;

namespace BudgetPlanner.Server.EndPoints;

public static class OpenBankingEndPointExtensions
{
    public static void MapOpenBankingEndPoint(this WebApplication app)
    {
        var openBankingGroup = app.MapGroup("/OpenBanking");

        openBankingGroup.MapGet("/GetOpenBankingProviders", () =>
        {

        });

    }
}

public static class AccountsEndpointExtensions
{
    public static void MapAccountsEndPoint(this WebApplication app)
    {
        var accountsGroup = app.MapGroup("/Accounts");

        accountsGroup.MapPost("AccountsAndLatestTransactions", async (AccountAndTransactionsRequest request, IAccountsService accountsService) =>
        {
            async IAsyncEnumerable<AccountAndTransactionsResponse> AccountsAndLatestTransactionsStream()
            {
                await foreach (var accountAndTransaction in accountsService.GetAccountsAndMostRecentTransactionsAsync(
                                   request.TransactionsCount, request.SyncTypes))
                {
                    yield return accountAndTransaction;
                }
            }
            
            return AccountsAndLatestTransactionsStream();

        });
    }
}

public static class DashboardEndpointExtensions
{
    public static void MapDashboardEndPoint(this WebApplication app)
    {
        var dashboardGroup = app.MapGroup("/Dashboard");

        dashboardGroup.MapPost("/SpentInTimePeriod",
            async (SpentInTimePeriodRequest request, IDashboardService dashboardService) => await dashboardService.GetSpentInTimePeriod(request.StartDate, request.EndDate));

        dashboardGroup.MapGet("/UpcomingPayments/{numberToFetch}", async (int numberToFetch, IDashboardService dashboardService) =>
        {
            async IAsyncEnumerable<UpcomingPaymentsResponse> UpcomingPaymentsStream()
            {
                await foreach (var payment in dashboardService.GetUpcomingPaymentsAsync(numberToFetch))
                {
                    yield return payment;
                }
            }
            return UpcomingPaymentsStream();
        });
    }
}