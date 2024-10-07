namespace BudgetPlanner.Data.Models
{
    public class OpenBankingAccountBalance : BaseEntity
    {
        public required string Currency { get; set; }
        public required decimal Available { get; set; }
        public required decimal Current { get; set; }
        public required string OpenBankingAccountId { get; set; }

        public Guid AccountId { get; set; }
        public OpenBankingAccount Account { get; set; }
    }
}
