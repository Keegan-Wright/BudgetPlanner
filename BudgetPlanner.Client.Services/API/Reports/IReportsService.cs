using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Response.Reports;

namespace BudgetPlanner.Client.Services.Reports;

public interface IReportsService
{
    IAsyncEnumerable<SpentInTimePeriodReportResponse> GetSpentInTimePeriodReportAsync(BaseReportRequest request);
    IAsyncEnumerable<SpentInCategoryReportResponse> GetCategoryBreakdownReportAsync(BaseReportRequest request);
    IAsyncEnumerable<SpentInAccountReportResponse> GetAccountBreakdownReportAsync(BaseReportRequest request);
}