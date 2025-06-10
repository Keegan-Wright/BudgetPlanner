using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Reports;

public class SpentInTimePeriodReportSharedResponse
{
    [Description("Total number of transactions in the time period")]
    public int TotalTransactions { get; set; }

    [Description("Total amount spent in the time period")]
    public decimal TotalSpent { get; set; }
}