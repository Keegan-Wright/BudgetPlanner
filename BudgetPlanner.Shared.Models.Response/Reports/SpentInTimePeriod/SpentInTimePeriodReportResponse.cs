using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Reports;

public class SpentInTimePeriodReportResponse : SpentInTimePeriodReportSharedResponse
{
    [Description("Yearly breakdown of spending for the specified time period")]
    public IList<SpentInTimePeriodReportYearlyBreakdownResponse> YearlyBreakdown { get; set; } = [];
}