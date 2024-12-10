using BudgetPlanner.Shared.Models.Response.Calendar;

namespace BudgetPlanner.Client.Services.Calendar;

public class CalendarRequestService : BaseRequestService, ICalendarRequestService
{
    public CalendarRequestService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public override string BaseRoute { get; init; }
    public IAsyncEnumerable<CalendarItemsResponse> GetMonthItemsAsync(int month, int year)
    {
        throw new NotImplementedException();
    }
}