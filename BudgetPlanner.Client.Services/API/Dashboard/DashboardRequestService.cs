using BudgetPlanner.Shared.Models.Request.Dashboard;
using BudgetPlanner.Shared.Models.Response;

namespace BudgetPlanner.Client.Services.Dashboard;

public class DashboardRequestService : BaseRequestService, IDashboardRequestService
{
    public DashboardRequestService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        BaseRoute = "dashboard";
    }

    public sealed override string BaseRoute { get; init; }
    public async Task<SpentInTimePeriodResponse> GetSpentInTimePeriodAsync(DateTime from, DateTime to)
    {
        
        var response = await PostAsync<SpentInTimePeriodRequest,SpentInTimePeriodResponse>("spentintimeperiod", new()
        {
            StartDate = from,
            EndDate = to
        }); 
        
        return response;
    }

    public async Task<SpentInTimePeriodResponse> GetSpentInTimePeriodAsync(DateTime date)
    {
        var response = await PostAsync<SpentInTimePeriodRequest,SpentInTimePeriodResponse>("spentintimeperiod", new()
        {
            StartDate = date,
            EndDate = date
        }); 
        
        return response;
    }

    public async IAsyncEnumerable<UpcomingPaymentsResponse> GetUpcomingPaymentsAsync(int numberToFetch)
    {
        var enumerable = GetAsyncEnumerable<UpcomingPaymentsResponse>($"upcomingpayments/{numberToFetch}");

        await foreach (var upcomingPayment in enumerable)
        {
            yield return upcomingPayment;
        }
        
    }
}