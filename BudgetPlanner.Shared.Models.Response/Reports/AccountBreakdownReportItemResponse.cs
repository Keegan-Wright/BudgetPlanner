namespace BudgetPlanner.Shared.Models.Response.Reports;

public class AccountBreakdownReportItemResponse
{
    public decimal Total { get; set; }
    public decimal TotalIn { get; set; }
    public decimal TotalOut { get; set; }
    public int TotalTransactions { get; set; }
    public string AccountName { get; set; }
    public DateTime Date { get; set; }
}