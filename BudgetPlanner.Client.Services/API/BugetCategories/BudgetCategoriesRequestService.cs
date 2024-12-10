using BudgetPlanner.Client.Data.Models;
using BudgetPlanner.Shared.Models.Request.Budget;

namespace BudgetPlanner.Client.Services.BugetCategories;

public class BudgetCategoriesRequestService : BaseRequestService, IBudgetCategoriesRequestService
{
    public BudgetCategoriesRequestService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        BaseRoute = "budgetcategories";
    }

    public override string BaseRoute { get; init; }
    public async IAsyncEnumerable<BudgetCategory> GetBudgetItemsAsync()
    {
        await foreach (var budgetCategory in GetAsyncEnumerable<BudgetCategory>("GetAll"))
        {
            yield return budgetCategory;
        }
    }

    public async Task<BudgetCategory> AddBudgetCategoryAsync(AddBudgetCategoryRequest categoryToAdd)
    {
        return await PostAsync<AddBudgetCategoryRequest, BudgetCategory>("AddCategory", categoryToAdd);
    }

    public async Task<bool> DeleteBudgetCategoryAsync(Guid id)
    {
        return await DeleteAsync($"DeleteCategory/{id}");
    }
}