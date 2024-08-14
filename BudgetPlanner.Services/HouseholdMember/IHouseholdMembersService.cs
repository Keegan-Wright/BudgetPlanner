using BudgetPlanner.Data.Models;
using BudgetPlanner.RequestModels.Budget;
using BudgetPlanner.RequestModels.HouseholdMember;

namespace BudgetPlanner.Services.Budget
{
    public interface IHouseholdMembersService
    {
        IAsyncEnumerable<HouseholdMember> GetHouseholdMembersAsync();

        Task<HouseholdMember> AddHouseholdMemberAsync(AddHouseholdMemberRequest categoryToAdd);
        Task<bool> DeleteHouseholdMemberAsync(Guid id);
    }
}
