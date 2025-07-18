using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Server.Services.Budget;
using BudgetPlanner.Shared.Models.Request.HouseholdMember;
using BudgetPlanner.Shared.Models.Response;

namespace BudgetPlanner.Server.EndPoints;

public static class HouseholdMappingExtensions
{
    public static void MapHouseholdMembersEndpoint(this WebApplication app)
    {
        var householdMembersGroup = app.MapGroup("/HouseholdMember").RequireAuthorization();;

        householdMembersGroup.MapPost("/AddHouseholdMember",
            async (AddHouseholdMemberRequest request, IHouseholdMembersService householdMembersService) => await householdMembersService.AddHouseholdMemberAsync(request));

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
        });

        householdMembersGroup.MapDelete("/DeleteHouseholdMember/{id}",
            async (Guid id, IHouseholdMembersService budgetCategoriesService) =>
            {
                return new GenericSuccessResponse()
                    { Success = await budgetCategoriesService.DeleteHouseholdMemberAsync(id) };
            });
    }
}