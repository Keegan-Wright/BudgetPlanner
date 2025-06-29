namespace BudgetPlanner.Shared.Models.Response.Reports;

public class AccountBreakdownReportResponse : AccountBreakdownReportItemResponse
{

    public IAsyncEnumerable<AccountBreakdownReportItemResponse> SubItems { get; set; }
}