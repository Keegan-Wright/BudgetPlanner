using BudgetPlanner.Data.Models;
using BudgetPlanner.Shared.Models.Request.Budget;
using BudgetPlanner.Shared.Models.Request.HouseholdMember;

namespace BudgetPlanner.Client.Services.Budget
{
    public interface IHouseholdMembersService
    {
        IAsyncEnumerable<HouseholdMember> GetHouseholdMembersAsync();

        Task<HouseholdMember> AddHouseholdMemberAsync(AddHouseholdMemberRequest categoryToAdd);
        Task<bool> DeleteHouseholdMemberAsync(Guid id);
    }
}
