namespace BudgetPlanner.Shared.Models.Response.Reports;

public class SpentInTimePeriodReportResponse : SpentInTimePeriodReportSharedResponse
{
    public IList<SpentInTimePeriodReportYearlyBreakdownResponse> YearlyBreakdown { get; set; }
}