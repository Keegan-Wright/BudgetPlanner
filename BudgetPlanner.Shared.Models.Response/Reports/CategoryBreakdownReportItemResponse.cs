namespace BudgetPlanner.Shared.Models.Response.Reports;

public class CategoryBreakdownReportItemResponse
{
    public decimal TotalIn { get; set; }
    public decimal TotalOut { get; set; }
    public decimal Total { get; set; }
    public decimal TotalTransactions { get; set; }
    public DateTime Date { get; set; }
    public string CategoryName { get; set; }
}