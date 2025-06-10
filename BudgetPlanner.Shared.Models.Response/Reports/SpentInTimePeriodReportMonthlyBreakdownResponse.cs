using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Reports;

public class SpentInTimePeriodReportMonthlyBreakdownResponse : SpentInTimePeriodReportSharedResponse
{
    [Description("Month number (1-12) for which the spending breakdown is provided")]
    public int Month { get; set; }
}