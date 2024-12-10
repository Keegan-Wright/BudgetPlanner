using BudgetPlanner.Server.Services.Calendar;
using BudgetPlanner.Shared.Models.Response.Calendar;

namespace BudgetPlanner.Server.EndPoints;

public static class CalendarEndPointExtensions
{
    public static void MapCalendarEndPoint(this WebApplication app)
    {
        var calendarGroup = app.MapGroup("/Calendar");

        calendarGroup.MapGet("/GetMonth/{month}/{year}", async (int month, int year, ICalendarService calendarService) =>
        {
            async IAsyncEnumerable<CalendarItemsResponse> BudgetCategoriesStream()
            {
                await foreach (var monthItem in calendarService.GetMonthItemsAsync(month, year))
                {
                    yield return monthItem;
                }
            }
            return BudgetCategoriesStream();
        });
    }
}