using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Request.Dashboard;

public class SpentInTimePeriodRequest
{
    [Description("Start date of the time period to analyze")]
    public DateTime StartDate { get; set; }

    [Description("End date of the time period to analyze")]
    public DateTime EndDate { get; set; }
}