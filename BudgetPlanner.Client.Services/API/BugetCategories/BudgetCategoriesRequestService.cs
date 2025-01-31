using BudgetPlanner.Client.Data.Db;
using BudgetPlanner.Client.Data.Models;
using BudgetPlanner.Shared.Models.Request.Budget;
using BudgetPlanner.Shared.Models.Response;

namespace BudgetPlanner.Client.Services.BugetCategories;

public class BudgetCategoriesRequestService : BaseRequestService, IBudgetCategoriesRequestService
{
    public BudgetCategoriesRequestService(IHttpClientFactory httpClientFactory, BudgetPlannerDbContext budgetPlannerDbContext) : base(httpClientFactory, budgetPlannerDbContext)
    {
        BaseRoute = "budgetcategories";
    }

    public sealed override string BaseRoute { get; init; }
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

    public async Task<GenericSuccessResponse> DeleteBudgetCategoryAsync(Guid id)
    {
        return await DeleteAsync<GenericSuccessResponse>($"DeleteCategory/{id}");
    }
}