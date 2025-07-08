using System.ComponentModel;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Server.Services.Budget;
using BudgetPlanner.Server.Services.Dashboard;
using BudgetPlanner.Shared.Models.Request.Budget;
using BudgetPlanner.Shared.Models.Request.Dashboard;
using BudgetPlanner.Shared.Models.Response;
using Microsoft.AspNetCore.OpenApi;

namespace BudgetPlanner.Server.EndPoints;

public static class BudgetCategoriesEndpointExtensions
{
    /// <summary>
    /// Maps the budget categories endpoints to the application
    /// </summary>
    /// <param name="app">The web application instance</param>
    public static void MapBudgetCategoriesEndPoint(this WebApplication app)
    {
        var dashboardGroup = app.MapGroup("/BudgetCategories")
            .WithTags("Budget Categories")
            .WithSummary("Budget Category Management")
            .WithDescription("Endpoints for managing budget categories")
            .RequireAuthorization();

        dashboardGroup.MapPost("/AddCategory",
            async ([Description("The request containing the budget category details to add")] AddBudgetCategoryRequest request, IBudgetCategoriesService budgetCategoriesService) => 
                await budgetCategoriesService.AddBudgetCategoryAsync(request))
            .WithSummary("Add Budget Category")
            .WithDescription("Creates a new budget category")
            .Accepts<AddBudgetCategoryRequest>("application/json")
            .Produces<BudgetCategory>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);

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
        })
            .WithSummary("Get All Budget Categories")
            .WithDescription("Retrieves a stream of all budget categories")
            .Produces<IAsyncEnumerable<BudgetCategory>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        dashboardGroup.MapDelete("/DeleteCategory/{id}",
            async ([Description("The unique identifier of the budget category to delete")] Guid id, IBudgetCategoriesService budgetCategoriesService) =>
            {
                return new GenericSuccessResponse()
                    { Success = await budgetCategoriesService.DeleteBudgetCategoryAsync(id) };
            })
            .WithSummary("Delete Budget Category")
            .WithDescription("Deletes a budget category by its ID")
            .Produces<GenericSuccessResponse>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }
}