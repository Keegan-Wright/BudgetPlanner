using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.Reports;

namespace BudgetPlanner.Server.Services.Reports;

public interface IReportService
{
    IAsyncEnumerable<SpentInTimePeriodReportResponse> GetSpentInTimePeriodReportAsync(BaseReportRequest request);
    IAsyncEnumerable<CategoryBreakdownReportResponse> GetCategoryBreakdownReportAsync(BaseReportRequest request);
    IAsyncEnumerable<AccountBreakdownReportResponse> GetAccountBreakdownReportAsync(BaseReportRequest request);
}