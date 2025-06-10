using System.ComponentModel;
using BudgetPlanner.Server.Services.Dashboard;
using BudgetPlanner.Shared.Models.Request.Dashboard;
using BudgetPlanner.Shared.Models.Response;
using Microsoft.AspNetCore.OpenApi;

namespace BudgetPlanner.Server.EndPoints;

public static class DashboardEndpointExtensions
{
    /// <summary>
    /// Maps the dashboard endpoints to the application
    /// </summary>
    /// <param name="app">The web application instance</param>
    public static void MapDashboardEndPoint(this WebApplication app)
    {
        var dashboardGroup = app.MapGroup("/Dashboard")
            .WithTags("Dashboard")
            .WithSummary("Dashboard Management")
            .WithDescription("Endpoints for managing dashboard data including spending analysis and upcoming payments")
            .RequireAuthorization();

        dashboardGroup.MapPost("/SpentInTimePeriod",
            async ([Description("The request containing the start and end dates for the spending period")] SpentInTimePeriodRequest request, IDashboardService dashboardService) => 
                await dashboardService.GetSpentInTimePeriod(request.StartDate, request.EndDate))
            .WithSummary("Get Spending in Time Period")
            .WithDescription("Retrieves the total amount spent within a specified time period")
            .Accepts<SpentInTimePeriodRequest>("application/json")
            .Produces<decimal>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);

        dashboardGroup.MapGet("/UpcomingPayments/{numberToFetch}", async ([Description("The maximum number of upcoming payments to retrieve")] int numberToFetch, IDashboardService dashboardService) =>
        {
            async IAsyncEnumerable<UpcomingPaymentsResponse> UpcomingPaymentsStream()
            {
                await foreach (var payment in dashboardService.GetUpcomingPaymentsAsync(numberToFetch))
                {
                    yield return payment;
                }
            }
            return UpcomingPaymentsStream();
        })
            .WithSummary("Get Upcoming Payments")
            .WithDescription("Retrieves a stream of upcoming payments, limited by the specified count")
            .Produces<IAsyncEnumerable<UpcomingPaymentsResponse>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);
    }
}