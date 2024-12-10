using BudgetPlanner.Shared.Models.Request.HouseholdMember;
using BudgetPlanner.Shared.Models.Response;

namespace BudgetPlanner.Client.Services.HouseholdMember;

public interface IHouseholdMemberRequestService
{
    IAsyncEnumerable<Data.Models.HouseholdMember> GetHouseholdMembersAsync();

    Task<Data.Models.HouseholdMember> AddHouseholdMemberAsync(AddHouseholdMemberRequest categoryToAdd);
    Task<GenericSuccessResponse> DeleteHouseholdMemberAsync(Guid id);
}