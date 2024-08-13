using BudgetPlanner.Data.Models;
using BudgetPlanner.RequestModels.Budget;

namespace BudgetPlanner.Services.Budget
{
    public interface IBudgetCategoriesService
    {
        IAsyncEnumerable<BudgetCategory> GetBudgetItemsAsync();

        Task<BudgetCategory> AddBudgetCategoryAsync(AddBudgetCategoryRequest categoryToAdd);
        Task<bool> DeleteBudgetCategoryAsync(Guid id);
    }
}
