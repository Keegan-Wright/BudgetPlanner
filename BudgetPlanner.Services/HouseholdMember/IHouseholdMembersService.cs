using BudgetPlanner.Data.Models;
using BudgetPlanner.Models.Request.Budget;
using BudgetPlanner.Models.Request.HouseholdMember;

namespace BudgetPlanner.Services.Budget
{
    public interface IHouseholdMembersService
    {
        IAsyncEnumerable<HouseholdMember> GetHouseholdMembersAsync();

        Task<HouseholdMember> AddHouseholdMemberAsync(AddHouseholdMemberRequest categoryToAdd);
        Task<bool> DeleteHouseholdMemberAsync(Guid id);
    }
}
