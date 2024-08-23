namespace BudgetPlanner.Data.Models
{
    public class OpenBankingProvider : BaseEntity
    {
        public required string Name { get; set; }
        public required string AccessCode { get; set; }
    }

}
