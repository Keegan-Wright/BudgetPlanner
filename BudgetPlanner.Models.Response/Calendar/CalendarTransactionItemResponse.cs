namespace BudgetPlanner.Models.Response.Calendar;

public class CalendarTransactionItemResponse
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string TransactionType { get; set; }
    public DateTime TransactionTime { get; set; }
}