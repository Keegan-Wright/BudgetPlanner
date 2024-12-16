using BudgetPlanner.Server.Services.Dashboard;
using BudgetPlanner.Shared.Models.Request.Dashboard;
using BudgetPlanner.Shared.Models.Response;

namespace BudgetPlanner.Server.EndPoints;

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