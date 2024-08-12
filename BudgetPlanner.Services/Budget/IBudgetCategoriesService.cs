using BudgetPlanner.Data.Models;

namespace BudgetPlanner.Services.Budget
{
    public interface IBudgetCategoriesService
    {
        IAsyncEnumerable<BudgetCategory> GetBudgetItemsAsync();
    }
}
