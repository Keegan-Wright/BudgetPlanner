using System.ComponentModel;
using BudgetPlanner.Server.Services.Calendar;
using BudgetPlanner.Shared.Models.Response.Calendar;
using Microsoft.AspNetCore.OpenApi;

namespace BudgetPlanner.Server.EndPoints;

public static class CalendarEndPointExtensions
{
    /// <summary>
    /// Maps the calendar endpoints to the application
    /// </summary>
    /// <param name="app">The web application instance</param>
    public static void MapCalendarEndPoint(this WebApplication app)
    {
        var calendarGroup = app.MapGroup("/Calendar")
            .WithTags("Calendar")
            .WithSummary("Calendar Management")
            .WithDescription("Endpoints for managing calendar items and monthly views")
            .RequireAuthorization();

        calendarGroup.MapGet("/GetMonth/{month}/{year}", async ([Description("The month number (1-12) to retrieve calendar items for")] int month, [Description("The year to retrieve calendar items for")] int year, ICalendarService calendarService) =>
        {
            async IAsyncEnumerable<CalendarItemsResponse> BudgetCategoriesStream()
            {
                await foreach (var monthItem in calendarService.GetMonthItemsAsync(month, year))
                {
                    yield return monthItem;
                }
            }
            return BudgetCategoriesStream();
        })
            .WithSummary("Get Month Calendar Items")
            .WithDescription("Retrieves a stream of calendar items for a specific month and year")
            .Produces<IAsyncEnumerable<CalendarItemsResponse>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);
    }
}