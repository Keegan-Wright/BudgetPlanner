using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Reports;

public class SpentInAccountReportMonthlyBreakdownResponse : SpentInAccountReportSharedResponse
{
    [Description("Month number (1-12) for which the spending breakdown is provided")]
    public string Month { get; set; }
    
    [Description("Daily breakdown of spending for the specified month")]
    public IList<SpentInAccountReportDailyBreakdownResponse> DailyBreakdown { get; set; } = [];

}