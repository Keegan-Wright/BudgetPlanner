
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Shared.Models.Request.Budget;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Server.Services.Budget
{
    public class BudgetCategoriesService : IBudgetCategoriesService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;

        public BudgetCategoriesService(BudgetPlannerDbContext budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
        }

        public async Task<BudgetCategory> AddBudgetCategoryAsync(AddBudgetCategoryRequest categoryToAdd)
        {
            var budgetCategory = new BudgetCategory()
            {
                Name = categoryToAdd.Name,
                AvailableFunds = categoryToAdd.AvailableFunds,
                Created = DateTime.Now.ToUniversalTime(),
                GoalCompletionDate = categoryToAdd.GoalCompletionDate,
                MonthlyStart = categoryToAdd.MonthlyStart,
                SavingsGoal = categoryToAdd.SavingsGoal,
            };

            await _budgetPlannerDbContext.BudgetCategories.AddAsync(budgetCategory);
            await _budgetPlannerDbContext.SaveChangesAsync();

            return budgetCategory;
        }

        public async Task<bool> DeleteBudgetCategoryAsync(Guid id)
        {
            var budgetCategoryToDelete = await _budgetPlannerDbContext.BudgetCategories.FindAsync(id);

            if (budgetCategoryToDelete is null)
            {
                _budgetPlannerDbContext.BudgetCategories.Remove(budgetCategoryToDelete!);

                return await _budgetPlannerDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async IAsyncEnumerable<BudgetCategory> GetBudgetItemsAsync()
        {
            await foreach(var category in _budgetPlannerDbContext.BudgetCategories.AsAsyncEnumerable())
            {
                yield return category;
            }
        }
    }
}
