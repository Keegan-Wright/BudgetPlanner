using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Shared.Models.Request.HouseholdMember
{
    public class AddHouseholdMemberRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public decimal Income { get; set; }
    }
}
