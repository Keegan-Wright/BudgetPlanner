namespace BudgetPlanner.Models.Response.Calendar;

public class CalendarItemsResponse
{
    public DateTime Date { get; set; }
    public IEnumerable<CalendarTransactionItemResponse> Transactions { get; set; } = [];
    public IEnumerable<CalendarGoalItemResponse> Goals { get; set; } = [];
    public IEnumerable<CalendarEventItemResponse> Events { get; set; } = [];
}