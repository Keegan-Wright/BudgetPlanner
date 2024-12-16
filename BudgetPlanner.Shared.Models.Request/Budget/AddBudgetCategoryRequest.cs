using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Shared.Models.Request.Budget
{
    public class AddBudgetCategoryRequest
    {
        public required string Name { get; set; }
        public decimal AvailableFunds { get; set; }
        public decimal MonthlyStart { get; set; }
        public decimal SavingsGoal { get; set; }
        public DateTime? GoalCompletionDate { get; set; }

    }
}
