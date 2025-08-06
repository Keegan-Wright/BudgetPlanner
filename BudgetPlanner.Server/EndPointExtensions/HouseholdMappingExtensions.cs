using System.ComponentModel;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Server.Services.Budget;
using BudgetPlanner.Shared.Models.Request.HouseholdMember;
using BudgetPlanner.Shared.Models.Response;
using Microsoft.AspNetCore.OpenApi;

namespace BudgetPlanner.Server.EndPoints;

public static class HouseholdMappingExtensions
{
    /// <summary>
    /// Maps the household members endpoints to the application
    /// </summary>
    /// <param name="app">The web application instance</param>
    public static void MapHouseholdMembersEndpoint(this WebApplication app)
    {
        var householdMembersGroup = app.MapGroup("/HouseholdMember")
            .WithTags("Household Members")
            .WithSummary("Household Member Management")
            .WithDescription("Endpoints for managing household members and their associations")
            .RequireAuthorization();

        householdMembersGroup.MapPost("/AddHouseholdMember",
            async ([Description("The request containing the household member details to add")] AddHouseholdMemberRequest request, IHouseholdMembersService householdMembersService) => 
                await householdMembersService.AddHouseholdMemberAsync(request))
            .WithSummary("Add Household Member")
            .WithDescription("Creates a new household member")
            .Accepts<AddHouseholdMemberRequest>("application/json")
            .Produces<HouseholdMember>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);

        householdMembersGroup.MapGet("/GetAll", async (IHouseholdMembersService householdMembersService) =>
        {
            async IAsyncEnumerable<HouseholdMember> BudgetHouseholdMembersStream()
            {
                await foreach (var budgetCategory in householdMembersService.GetHouseholdMembersAsync())
                {
                    yield return budgetCategory;
                }
            }
            return BudgetHouseholdMembersStream();
        })
            .WithSummary("Get All Household Members")
            .WithDescription("Retrieves a stream of all household members")
            .Produces<IAsyncEnumerable<HouseholdMember>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        householdMembersGroup.MapDelete("/DeleteHouseholdMember/{id}",
            async ([Description("The unique identifier of the household member to delete")] Guid id, IHouseholdMembersService budgetCategoriesService) =>
            {
                return new GenericSuccessResponse()
                    { Success = await budgetCategoriesService.DeleteHouseholdMemberAsync(id) };
            })
            .WithSummary("Delete Household Member")
            .WithDescription("Removes a household member by their ID")
            .Produces<GenericSuccessResponse>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }
}