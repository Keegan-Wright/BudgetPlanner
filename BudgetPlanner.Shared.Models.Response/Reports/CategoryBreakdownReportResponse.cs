namespace BudgetPlanner.Shared.Models.Response.Reports;

public class CategoryBreakdownReportResponse : CategoryBreakdownReportItemResponse
{
    public IAsyncEnumerable<CategoryBreakdownReportItemResponse> SubItems { get; set; }
}