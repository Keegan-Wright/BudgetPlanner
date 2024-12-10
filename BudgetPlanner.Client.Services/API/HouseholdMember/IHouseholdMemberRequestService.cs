using BudgetPlanner.Shared.Models.Request.HouseholdMember;

namespace BudgetPlanner.Client.Services.HouseholdMember;

public interface IHouseholdMemberRequestService
{
    IAsyncEnumerable<Data.Models.HouseholdMember> GetHouseholdMembersAsync();

    Task<Data.Models.HouseholdMember> AddHouseholdMemberAsync(AddHouseholdMemberRequest categoryToAdd);
    Task<bool> DeleteHouseholdMemberAsync(Guid id);
}