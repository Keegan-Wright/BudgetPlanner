namespace BudgetPlanner.Shared.Models.Response.Reports;

public class SpentInTimePeriodReportYearlyBreakdownResponse : SpentInTimePeriodReportSharedResponse
{
    public int Year { get; set; }
    public IList<SpentInTimePeriodReportMonthlyBreakdownResponse> MonthlyBreakdown { get; set; }
}