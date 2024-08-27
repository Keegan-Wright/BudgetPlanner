namespace BudgetPlanner.Data.Models
{
    public class OpenBankingAccount : BaseEntity
    {
        public required string OpenBankingAccountId { get; set; }
        public required string AccountType { get; set; }
        public required string DisplayName { get; set; }
        public required string Currency { get; set; }
        public required string OpenBankingProviderId { get; set; }
    }
}