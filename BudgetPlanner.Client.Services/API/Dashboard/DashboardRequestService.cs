using BudgetPlanner.Shared.Models.Response;

namespace BudgetPlanner.Client.Services.Dashboard;

public class DashboardRequestService : BaseRequestService, IDashboardRequestService
{
    public DashboardRequestService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public override string BaseRoute { get; init; }
    public Task<SpentInTimePeriodResponse> GetSpentInTimePeriod(DateTime from, DateTime to)
    {
        throw new NotImplementedException();
    }

    public Task<SpentInTimePeriodResponse> GetSpentInTimePeriod(DateTime date)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<UpcomingPaymentsResponse> GetUpcomingPaymentsAsync(int numberToFetch)
    {
        throw new NotImplementedException();
    }
}