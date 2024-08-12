using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.ClientModels.Budget
{
    public class BudgetCategory
    {
        public required string Name { get; set; }
        public decimal AvailbleFunds { get; set; }
        public decimal MonthlyStart { get; set; }
        public decimal SavingsGoal { get; set; }
        public DateTime? GoalCompletionDate { get; set; }

    }
}
