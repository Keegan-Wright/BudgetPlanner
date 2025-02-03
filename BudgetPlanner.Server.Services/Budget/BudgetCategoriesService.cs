
using System.Security.Claims;
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Shared.Models.Request.Budget;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Server.Services.Budget
{
    public class BudgetCategoriesService : BaseService, IBudgetCategoriesService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;

        public BudgetCategoriesService(BudgetPlannerDbContext budgetPlannerDbContext, ClaimsPrincipal user) : base(user, budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
        }

        public async Task<BudgetCategory> AddBudgetCategoryAsync(AddBudgetCategoryRequest categoryToAdd)
        {
            var user = await _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.BudgetCategories).FirstAsync();
            
            var budgetCategory = new BudgetCategory()
            {
                Name = categoryToAdd.Name,
                AvailableFunds = categoryToAdd.AvailableFunds,
                Created = DateTime.Now.ToUniversalTime(),
                GoalCompletionDate = categoryToAdd.GoalCompletionDate,
                MonthlyStart = categoryToAdd.MonthlyStart,
                SavingsGoal = categoryToAdd.SavingsGoal,
            };
            
            user.BudgetCategories.Add(budgetCategory);

            await _budgetPlannerDbContext.SaveChangesAsync();

            return budgetCategory;
        }

        public async Task<bool> DeleteBudgetCategoryAsync(Guid id)
        {
            var user = await _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.BudgetCategories).FirstAsync();
            
            var budgetCategory = user.BudgetCategories.FirstOrDefault(x => x.Id == id);

            if (budgetCategory != null)
            {
                _budgetPlannerDbContext.BudgetCategories.Remove(budgetCategory);
                await _budgetPlannerDbContext.SaveChangesAsync();
            }

            return false;
        }

        public async IAsyncEnumerable<BudgetCategory> GetBudgetItemsAsync()
        {
            await foreach(var category in _budgetPlannerDbContext.IsolateToUser(UserId)
                              .Include(x => x.BudgetCategories)
                              .SelectMany(x => x.BudgetCategories)
                              .AsAsyncEnumerable())
            {
                yield return category;
            }
        }
    }
}
