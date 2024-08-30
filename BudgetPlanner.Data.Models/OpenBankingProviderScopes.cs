namespace BudgetPlanner.Data.Models
{
    public class OpenBankingProviderScopes : BaseEntity
    {
        public required string Scope { get; set; }
        public required Guid ProviderId { get; set; }
    }
}