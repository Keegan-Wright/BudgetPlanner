using BudgetPlanner.Client.Data.Models;
using BudgetPlanner.Shared.Models.Request.Budget;
using BudgetPlanner.Shared.Models.Response;

namespace BudgetPlanner.Client.Services.BugetCategories;

public interface IBudgetCategoriesRequestService
{
    IAsyncEnumerable<BudgetCategory> GetBudgetItemsAsync();

    Task<BudgetCategory> AddBudgetCategoryAsync(AddBudgetCategoryRequest categoryToAdd);
    Task<GenericSuccessResponse> DeleteBudgetCategoryAsync(Guid id);
}