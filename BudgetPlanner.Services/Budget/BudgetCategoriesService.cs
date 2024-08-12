using BudgetPlanner.Data.Db;
using BudgetPlanner.Data.Models;

namespace BudgetPlanner.Services.Budget
{
    public class BudgetCategoriesService : IBudgetCategoriesService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;

        public BudgetCategoriesService(BudgetPlannerDbContext budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
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
