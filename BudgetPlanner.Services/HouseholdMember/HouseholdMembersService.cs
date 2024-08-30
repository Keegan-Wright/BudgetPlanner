using BudgetPlanner.Data.Db;
using BudgetPlanner.Data.Models;
using BudgetPlanner.Models.Request.HouseholdMember;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Services.Budget
{
    public class HouseholdMembersService : IHouseholdMembersService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;

        public HouseholdMembersService(BudgetPlannerDbContext budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
        }

        public async Task<HouseholdMember> AddHouseholdMemberAsync(AddHouseholdMemberRequest categoryToAdd)
        {
            var householdMember = new HouseholdMember()
            {
                FirstName = categoryToAdd.FirstName,
                LastName = categoryToAdd.LastName,
                Income = categoryToAdd.Income,
                Created = DateTime.Now
            };

            await _budgetPlannerDbContext.HouseholdMembers.AddAsync(householdMember);
            await _budgetPlannerDbContext.SaveChangesAsync();

            return householdMember;
        }

        public async Task<bool> DeleteHouseholdMemberAsync(Guid id)
        {
            var householdMemberToDelete = await _budgetPlannerDbContext.BudgetCategories.FindAsync(id);

            if (householdMemberToDelete is null)
            {
                _budgetPlannerDbContext.BudgetCategories.Remove(householdMemberToDelete!);

                return await _budgetPlannerDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async IAsyncEnumerable<HouseholdMember> GetHouseholdMembersAsync()
        {
            await foreach(var category in _budgetPlannerDbContext.HouseholdMembers.AsAsyncEnumerable())
            {
                yield return category;
            }
        }
    }
}
