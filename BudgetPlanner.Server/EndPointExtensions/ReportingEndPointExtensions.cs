using System.ComponentModel;
using BudgetPlanner.Server.Services.Reports;
using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Response.Reports;
using Microsoft.AspNetCore.Mvc;

namespace BudgetPlanner.Server.EndPoints;

public static class ReportingEndPointExtensions
{
    /// <summary>
    /// Maps the reporting endpoints to the application
    /// </summary>
    /// <param name="app">The web application instance</param>
    public static void MapReportingEndpoint(this WebApplication app)
    {
        var reportsGroup = app.MapGroup("/Reports")
            .WithTags("Reports")
            .WithSummary("Reporting Management")
            .WithDescription("Endpoints for generating and retrieving financial reports")
            .RequireAuthorization();

        reportsGroup.MapPost("/GetSpentInTimePeriod",
            async (
                [Description("The request containing the time period and filtering criteria for the report")] [FromBody]
                BaseReportRequest request, [FromServices] IReportService reportService) =>
            {
              async IAsyncEnumerable<SpentInTimePeriodReportResponse> GetSpentInTimePeriodReportStream()
              {
                  await foreach (var report in reportService.GetSpentInTimePeriodReportAsync(request))
                  {
                      yield return report;
                  }
              }
              return GetSpentInTimePeriodReportStream();
            })
            .WithSummary("Get Spent In Time Period Report")
            .WithDescription("Retrieves a report of spending within a specified time period")
            .Accepts<BaseReportRequest>("application/json")
            .Produces<SpentInTimePeriodReportResponse>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);
        
        reportsGroup.MapPost("/GetCategoryBreakdownInTimePeriod",
                async (
                    [Description("The request containing the time period and filtering criteria for the report")]
                    [FromBody]
                    BaseReportRequest request, [FromServices] IReportService reportService) =>
                {
                    async IAsyncEnumerable<SpentInCategoryReportResponse> GetCategoryBreakdownInTimePeriodReportStream()
                    {
                        await foreach (var report in reportService.GetCategoryBreakdownReportAsync(request))
                        {
                            yield return report;
                        }
                    }
                    return GetCategoryBreakdownInTimePeriodReportStream();
                })
            .WithSummary("Get Category Breakdown In Time Period")
            .WithDescription("Retrieves a report of spending for categories within a specified time period")
            .Accepts<BaseReportRequest>("application/json")
            .Produces<SpentInTimePeriodReportResponse>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);
        
        
        reportsGroup.MapPost("/GetAccountBreakdownInTimePeriod",
                async (
                    [Description("The request containing the time period and filtering criteria for the report")]
                    [FromBody]
                    BaseReportRequest request, [FromServices] IReportService reportService) =>
                {
                    async IAsyncEnumerable<SpentInAccountReportResponse> GetAccountBreakdownInTimePeriodReportStream()
                    {
                        await foreach (var report in reportService.GetAccountBreakdownReportAsync(request))
                        {
                            yield return report;
                        }
                    }
                    return GetAccountBreakdownInTimePeriodReportStream();
                })
            .WithSummary("Get Account Breakdown In Time Period")
            .WithDescription("Retrieves a report of spending for accounts within a specified time period")
            .Accepts<BaseReportRequest>("application/json")
            .Produces<SpentInTimePeriodReportResponse>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);


    }
}