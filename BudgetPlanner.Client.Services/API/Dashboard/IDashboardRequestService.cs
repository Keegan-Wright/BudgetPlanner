using BudgetPlanner.Shared.Models.Response;

namespace BudgetPlanner.Client.Services.Dashboard;

public interface IDashboardRequestService
{

        Task<SpentInTimePeriodResponse> GetSpentInTimePeriodAsync(DateTime from, DateTime to);
        Task<SpentInTimePeriodResponse> GetSpentInTimePeriodAsync(DateTime date);
        IAsyncEnumerable<UpcomingPaymentsResponse> GetUpcomingPaymentsAsync(int numberToFetch);
}