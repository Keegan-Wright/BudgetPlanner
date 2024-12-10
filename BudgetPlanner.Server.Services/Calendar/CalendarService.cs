using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Shared.Models.Response.Calendar;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Server.Services.Calendar;

public class CalendarService : ICalendarService
{
    private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
    
    public CalendarService(BudgetPlannerDbContext budgetPlannerDbContext)
    {
        _budgetPlannerDbContext = budgetPlannerDbContext;
    }
    
    public async IAsyncEnumerable<CalendarItemsResponse> GetMonthItemsAsync(int month, int year)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var startDate = new DateTime(year, month, 1);
        var endDate = new DateTime(year, month, daysInMonth);
        
        var transactions = await _budgetPlannerDbContext.OpenBankingTransactions
            .Where(x => x.TransactionTime >= startDate && x.TransactionTime < endDate)
            .Select(x => new CalendarTransactionItemResponse(){ Description = x.Description, Amount = x.Amount, TransactionType = x.TransactionType, TransactionTime = x.TransactionTime })
            .GroupBy(x => x.TransactionTime)
            .ToListAsync();
        
        List<string> events = ["Monthly events"];
        var goals = await _budgetPlannerDbContext.BudgetCategories
            .Where(x => x.GoalCompletionDate >= startDate && x.GoalCompletionDate < endDate)
            .Select(x => new CalendarGoalItemResponse(){ Name = x.Name, GoalCompletionDate = x.GoalCompletionDate})
            .GroupBy(x => x.GoalCompletionDate)
            .ToListAsync();

        for (int i = 0; i < daysInMonth; i++)
        {
            var date = startDate.AddDays(i);

            var response = new CalendarItemsResponse()
            {
                Date = date,
                Transactions = transactions.Where(x => x.Key.Date == date.Date).SelectMany(x => x),
                Goals = goals.Where(x => x.Key == date).SelectMany(x => x),
                
            };

            yield return response;
        }
        
    }
}