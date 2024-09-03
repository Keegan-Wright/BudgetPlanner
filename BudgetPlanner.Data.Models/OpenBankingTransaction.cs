
namespace BudgetPlanner.Data.Models
{
    public class OpenBankingTransaction : BaseEntity
    {
        public required string OpenBankingAccountId { get; set; }
        public required string Description { get; set; }
        public required string TransactionType { get; set; }
        public required string TransactionCategory { get; set; }
        public required decimal Amount { get; set; }
        public required string Currency { get; set; }
        public required string TransactionId { get; set; }
        public required DateTime TransactionTime { get; set; }
        public required bool Pending { get; set; }
    }

    public class OpenBankingSyncronisation : BaseEntity
    {
        public required int SyncronisationTypeFlag { get; set; }
        public required DateTime SyncronisedAt { get; set; }

    }
}
