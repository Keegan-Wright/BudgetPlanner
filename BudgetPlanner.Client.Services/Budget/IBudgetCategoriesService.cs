using BudgetPlanner.Data.Models;
using BudgetPlanner.Shared.Models.Request.Budget;

namespace BudgetPlanner.Client.Services.Budget
{
    public interface IBudgetCategoriesService
    {
        IAsyncEnumerable<BudgetCategory> GetBudgetItemsAsync();

        Task<BudgetCategory> AddBudgetCategoryAsync(AddBudgetCategoryRequest categoryToAdd);
        Task<bool> DeleteBudgetCategoryAsync(Guid id);
    }
}
