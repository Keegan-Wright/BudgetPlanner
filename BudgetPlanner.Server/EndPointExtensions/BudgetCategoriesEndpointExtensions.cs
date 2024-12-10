using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Server.Services.Budget;
using BudgetPlanner.Server.Services.Dashboard;
using BudgetPlanner.Shared.Models.Request.Budget;
using BudgetPlanner.Shared.Models.Request.Dashboard;
using BudgetPlanner.Shared.Models.Response;

namespace BudgetPlanner.Server.EndPoints;

public static class BudgetCategoriesEndpointExtensions
{
    public static void MapBudgetCategoriesEndPoint(this WebApplication app)
    {
        var dashboardGroup = app.MapGroup("/BudgetCategories");

        dashboardGroup.MapPost("/AddCategory",
            async (AddBudgetCategoryRequest request, IBudgetCategoriesService budgetCategoriesService) => await budgetCategoriesService.AddBudgetCategoryAsync(request));

        dashboardGroup.MapGet("/GetAll", async (IBudgetCategoriesService budgetCategoriesService) =>
        {
            async IAsyncEnumerable<BudgetCategory> BudgetCategoriesStream()
            {
                await foreach (var budgetCategory in budgetCategoriesService.GetBudgetItemsAsync())
                {
                    yield return budgetCategory;
                }
            }
            return BudgetCategoriesStream();
        });

        dashboardGroup.MapDelete("/DeleteCategory/{id}",
            async (Guid id, IBudgetCategoriesService budgetCategoriesService) =>
            {
                return new GenericSuccessResponse()
                    { Success = await budgetCategoriesService.DeleteBudgetCategoryAsync(id) };
            });
    }
}