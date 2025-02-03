using System.Security.Claims;
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Shared.Models.Response.Calendar;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Server.Services.Calendar;

public class CalendarService : BaseService, ICalendarService
{
    private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
    
    public CalendarService(BudgetPlannerDbContext budgetPlannerDbContext, ClaimsPrincipal user) : base(user, budgetPlannerDbContext)
    {
        _budgetPlannerDbContext = budgetPlannerDbContext;
    }
    
    public async IAsyncEnumerable<CalendarItemsResponse> GetMonthItemsAsync(int month, int year)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var startDate = new DateTime(year, month, 1).ToUniversalTime();
        var endDate = new DateTime(year, month, daysInMonth).ToUniversalTime();
        
        
        
        var transactions = await _budgetPlannerDbContext.IsolateToUser(UserId)
            .Include(x => x.Accounts).ThenInclude(x => x.Transactions)
            .SelectMany(x => x.Accounts.SelectMany(c => c.Transactions))
            .Where(x => x.TransactionTime >= startDate && x.TransactionTime < endDate)
            .Select(x => new CalendarTransactionItemResponse(){ Description = x.Description, Amount = x.Amount, TransactionType = x.TransactionType, TransactionTime = x.TransactionTime })
            .GroupBy(x => x.TransactionTime)
            .ToListAsync();
        
        List<string> events = ["Monthly events"];
        var goals = await _budgetPlannerDbContext.IsolateToUser(UserId)
            .Include(x => x.BudgetCategories)
            .SelectMany(x => x.BudgetCategories)
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