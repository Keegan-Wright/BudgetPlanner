namespace BudgetPlanner.Data.Models
{
    public class BudgetCategory : BaseEntity 
    {
        public required string Name { get; set; }
        public decimal AvailbleFunds { get; set; }
        public decimal MonthlyStart { get; set; }
        public decimal SavingsGoal { get; set; }
        public DateTime? GoalCompletionDate { get; set; }
    }
}
