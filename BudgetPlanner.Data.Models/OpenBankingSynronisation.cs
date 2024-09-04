namespace BudgetPlanner.Data.Models
{
    public class OpenBankingSynronisation : BaseEntity
    {
        public required int SyncronisationType { get; set; }
        public required DateTime SyncronisationTime { get; set; }
        public required string OpenBankingProviderId { get; set; }
        public required string OpenBankingAccountId { get; set; }
    }
}
