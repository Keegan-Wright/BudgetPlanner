﻿namespace BudgetPlanner.Client.Data.Models
{
    public class OpenBankingSynronisation : BaseEntity
    {
        public required int SyncronisationType { get; set; }
        public required DateTime SyncronisationTime { get; set; }

        public Guid ProviderId { get; set; }
        public OpenBankingProvider Provider { get; set; }

        public Guid AccountId { get; set; }
        public OpenBankingAccount Account { get; set; }
        public string OpenBankingAccountId { get; set; }
    }
}
