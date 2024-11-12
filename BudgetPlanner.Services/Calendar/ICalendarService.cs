using BudgetPlanner.Models.Response.Calendar;

namespace BudgetPlanner.Services.Calendar;

public interface ICalendarService
{
    IAsyncEnumerable<CalendarItemsResponse> GetMonthItemsAsync(int month, int year);
}