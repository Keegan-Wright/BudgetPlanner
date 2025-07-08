using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Reports;

public class SpentInTimePeriodReportYearlyBreakdownResponse : SpentInTimePeriodReportSharedResponse
{
    [Description("Year for which the spending breakdown is provided")]
    public int Year { get; set; }

    [Description("Monthly breakdown of spending for the specified year")]
    public IList<SpentInTimePeriodReportMonthlyBreakdownResponse> MonthlyBreakdown { get; set; } = [];
}

