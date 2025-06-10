using System.ComponentModel;
using BudgetPlanner.Server.Services.Reports;
using BudgetPlanner.Server.Services.Transactions;
using BudgetPlanner.Shared.Models.Request.Dashboard;
using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.Reports;
using BudgetPlanner.Shared.Models.Response.Transaction;
using Microsoft.AspNetCore.OpenApi;

namespace BudgetPlanner.Server.EndPoints;

public static class ReportingEndPointExtensions
{
    /// <summary>
    /// Maps the reporting endpoints to the application
    /// </summary>
    /// <param name="app">The web application instance</param>
    public static void MapReportingEndpoint(this WebApplication app)
    {
        var transactionsGroup = app.MapGroup("/Reports")
            .WithTags("Reports")
            .WithSummary("Reporting Management")
            .WithDescription("Endpoints for generating and retrieving financial reports")
            .RequireAuthorization();

        transactionsGroup.MapGet("/GetSpentInTimePeriod", async ([Description("The request containing the time period and filtering criteria for the report")] BaseReportRequest request, IReportService reportService) =>
        {
            return await reportService.GetSpentInTimePeriodReportAsync(request);
        })
            .WithSummary("Get Spent In Time Period Report")
            .WithDescription("Retrieves a report of spending within a specified time period")
            .Accepts<BaseReportRequest>("application/json")
            .Produces<SpentInTimePeriodReportResponse>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);
    }
}