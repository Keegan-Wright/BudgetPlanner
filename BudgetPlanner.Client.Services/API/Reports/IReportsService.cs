using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Response.Reports;

namespace BudgetPlanner.Client.Services.Reports;

public interface IReportsService
{
    IAsyncEnumerable<SpentInTimePeriodReportResponse> GetSpentInTimePeriodReportAsync(BaseReportRequest request);
    IAsyncEnumerable<CategoryBreakdownReportResponse> GetCategoryBreakdownReportAsync(BaseReportRequest request);
    IAsyncEnumerable<AccountBreakdownReportResponse> GetAccountBreakdownReportAsync(BaseReportRequest request);
}