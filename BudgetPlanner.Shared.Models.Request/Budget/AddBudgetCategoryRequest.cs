using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Request.Budget
{
    public class AddBudgetCategoryRequest
    {
        [Description("Name of the budget category")]
        public required string Name { get; set; }

        [Description("Current available funds in the budget category")]
        public decimal AvailableFunds { get; set; }

        [Description("Starting amount for the monthly budget")]
        public decimal MonthlyStart { get; set; }

        [Description("Target savings goal for the budget category")]
        public decimal SavingsGoal { get; set; }

        [Description("Target date for achieving the savings goal")]
        public DateTime? GoalCompletionDate { get; set; }
    }
}
