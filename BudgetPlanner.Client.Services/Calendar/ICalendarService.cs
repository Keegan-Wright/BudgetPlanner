using BudgetPlanner.Shared.Models.Response.Calendar;

namespace BudgetPlanner.Client.Services.Calendar;

public interface ICalendarService
{
    IAsyncEnumerable<CalendarItemsResponse> GetMonthItemsAsync(int month, int year);
}