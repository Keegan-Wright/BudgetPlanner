using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Reports;

public class SpentInAccountReportResponse : SpentInAccountReportSharedResponse
{
    [Description("Yearly breakdown of spending for the specified time period")]
    public IList<SpentInAccountReportYearlyBreakdownResponse> YearlyBreakdown { get; set; } = [];
    
    public string AccountName { get; set; }
}