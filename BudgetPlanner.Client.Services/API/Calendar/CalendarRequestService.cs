using BudgetPlanner.Client.Data.Db;
using BudgetPlanner.Shared.Models.Response.Calendar;

namespace BudgetPlanner.Client.Services.Calendar;

public class CalendarRequestService : BaseRequestService, ICalendarRequestService
{
    public CalendarRequestService(IHttpClientFactory httpClientFactory, BudgetPlannerDbContext budgetPlannerDbContext) : base(httpClientFactory, budgetPlannerDbContext)
    {
        BaseRoute = "calendar";
    }

    public sealed override string BaseRoute { get; init; }
    public async IAsyncEnumerable<CalendarItemsResponse> GetMonthItemsAsync(int month, int year)
    {
        await foreach (var calendarItem in GetAsyncEnumerable<CalendarItemsResponse>($"GetMonth/{month}/{year}"))
        {
            yield return calendarItem;
        }
    }
}