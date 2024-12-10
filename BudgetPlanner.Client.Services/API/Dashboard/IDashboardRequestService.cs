using BudgetPlanner.Shared.Models.Response;

namespace BudgetPlanner.Client.Services.Dashboard;

public interface IDashboardRequestService
{

        Task<SpentInTimePeriodResponse> GetSpentInTimePeriod(DateTime from, DateTime to);
        Task<SpentInTimePeriodResponse> GetSpentInTimePeriod(DateTime date);
        IAsyncEnumerable<UpcomingPaymentsResponse> GetUpcomingPaymentsAsync(int numberToFetch);
}