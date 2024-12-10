using BudgetPlanner.Client.Data.Models;
using BudgetPlanner.Shared.Models.Request.Budget;

namespace BudgetPlanner.Client.Services.BugetCategories;

public interface IBudgetCategoriesRequestService
{
    IAsyncEnumerable<BudgetCategory> GetBudgetItemsAsync();

    Task<BudgetCategory> AddBudgetCategoryAsync(AddBudgetCategoryRequest categoryToAdd);
    Task<bool> DeleteBudgetCategoryAsync(Guid id);
}