using System.Security.Claims;
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Shared.Models.Request.HouseholdMember;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Server.Services.Budget
{
    public class HouseholdMembersService : BaseService, IHouseholdMembersService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;

        public HouseholdMembersService(BudgetPlannerDbContext budgetPlannerDbContext, ClaimsPrincipal user) : base(user, budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
        }

        public async Task<HouseholdMember> AddHouseholdMemberAsync(AddHouseholdMemberRequest categoryToAdd)
        {
            var user = await _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.HouseholdMembers).FirstOrDefaultAsync();
            
            var householdMember = new HouseholdMember()
            {
                FirstName = categoryToAdd.FirstName,
                LastName = categoryToAdd.LastName,
                Income = categoryToAdd.Income,
                Created = DateTime.Now.ToUniversalTime()
            };

            user.HouseholdMembers.Add(householdMember);
            await _budgetPlannerDbContext.SaveChangesAsync();

            return householdMember;
        }

        public async Task<bool> DeleteHouseholdMemberAsync(Guid id)
        {
            var user = await _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.HouseholdMembers).FirstAsync();
            
            var householdMember = user.HouseholdMembers.FirstOrDefault(x => x.Id == id);

            if (householdMember != null)
            {
                _budgetPlannerDbContext.HouseholdMembers.Remove(householdMember);
                await _budgetPlannerDbContext.SaveChangesAsync();
            }

            return false;
        }

        public async IAsyncEnumerable<HouseholdMember> GetHouseholdMembersAsync()
        {
            await foreach(var householdMember in _budgetPlannerDbContext.IsolateToUser(UserId)
                              .Include(x => x.HouseholdMembers)
                              .SelectMany(x => x.HouseholdMembers)
                              .AsAsyncEnumerable())
            {
                yield return householdMember;
            }
        }
    }
}
