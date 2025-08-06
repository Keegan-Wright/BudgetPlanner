using BudgetPlanner.Client.Data.Db;
using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Response.Reports;

namespace BudgetPlanner.Client.Services.Reports;

public class ReportsService : BaseRequestService, IReportsService
{
    
    public sealed override string BaseRoute { get; init; }
    
    public ReportsService(IHttpClientFactory httpClientFactory, BudgetPlannerDbContext budgetPlannerDbContext) : base(httpClientFactory, budgetPlannerDbContext)
    {
        BaseRoute = "Reports";
    }

    public async IAsyncEnumerable<SpentInTimePeriodReportResponse> GetSpentInTimePeriodReportAsync(BaseReportRequest request)
    {
        await foreach (var accountAndTransaction in PostAsyncEnumerableAsync<BaseReportRequest, SpentInTimePeriodReportResponse>("GetSpentInTimePeriod", request))
        {
            yield return accountAndTransaction;
        }
    }

    public async IAsyncEnumerable<SpentInCategoryReportResponse> GetCategoryBreakdownReportAsync(BaseReportRequest request)
    {
        await foreach (var accountAndTransaction in PostAsyncEnumerableAsync<BaseReportRequest, SpentInCategoryReportResponse>("GetCategoryBreakdownInTimePeriod",request))
        {
            yield return accountAndTransaction;
        }
    }

    public async IAsyncEnumerable<SpentInAccountReportResponse> GetAccountBreakdownReportAsync(BaseReportRequest request)
    {
        await foreach (var accountAndTransaction in PostAsyncEnumerableAsync<BaseReportRequest, SpentInAccountReportResponse>("GetAccountBreakdownInTimePeriod",request ))
        {
            yield return accountAndTransaction;
        }
    }
}