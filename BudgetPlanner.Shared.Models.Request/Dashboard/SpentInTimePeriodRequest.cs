namespace BudgetPlanner.Shared.Models.Request.Dashboard;

public class SpentInTimePeriodRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}