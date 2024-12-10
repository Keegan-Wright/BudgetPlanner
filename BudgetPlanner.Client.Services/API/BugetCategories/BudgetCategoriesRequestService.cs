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
    public IAsyncEnumerable<BudgetCategory> GetBudgetItemsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<BudgetCategory> AddBudgetCategoryAsync(AddBudgetCategoryRequest categoryToAdd)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteBudgetCategoryAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}