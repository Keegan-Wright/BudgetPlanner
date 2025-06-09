using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.Reports;

namespace BudgetPlanner.Server.Services.Reports;

public interface IReportService
{
    Task<SpentInTimePeriodReportResponse> GetSpentInTimePeriodReportAsync(BaseReportRequest request);
    Task<CategoryBreakdownReportResponse> GetCategoryBreakdownReportAsync(BaseReportRequest request);
    Task<SpentInTimePeriodResponse> AccountBreakdowmReportAsync(BaseReportRequest request);
}