using BudgetPlanner.Client.Data.Db;
using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Response.Reports;

namespace BudgetPlanner.Client.Services.Reports;

public interface IReportsService
{
    IAsyncEnumerable<SpentInTimePeriodReportResponse> GetSpentInTimePeriodReportAsync(BaseReportRequest request);
    IAsyncEnumerable<CategoryBreakdownReportResponse> GetCategoryBreakdownReportAsync(BaseReportRequest request);
    IAsyncEnumerable<AccountBreakdownReportResponse> GetAccountBreakdownReportAsync(BaseReportRequest request);
}

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

    public async IAsyncEnumerable<CategoryBreakdownReportResponse> GetCategoryBreakdownReportAsync(BaseReportRequest request)
    {
        await foreach (var accountAndTransaction in PostAsyncEnumerableAsync<BaseReportRequest, CategoryBreakdownReportResponse>("GetCategoryBreakdownInTimePeriod",request))
        {
            yield return accountAndTransaction;
        }
    }

    public async IAsyncEnumerable<AccountBreakdownReportResponse> GetAccountBreakdownReportAsync(BaseReportRequest request)
    {
        await foreach (var accountAndTransaction in PostAsyncEnumerableAsync<BaseReportRequest, AccountBreakdownReportResponse>("GetAccountBreakdownInTimePeriod",request ))
        {
            yield return accountAndTransaction;
        }
    }
}