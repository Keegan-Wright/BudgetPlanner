
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Shared.Models.Request.HouseholdMember;

namespace BudgetPlanner.Server.Services.Budget
{
    public interface IHouseholdMembersService
    {
        IAsyncEnumerable<HouseholdMember> GetHouseholdMembersAsync();

        Task<HouseholdMember> AddHouseholdMemberAsync(AddHouseholdMemberRequest categoryToAdd);
        Task<bool> DeleteHouseholdMemberAsync(Guid id);
    }
}
