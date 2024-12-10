﻿namespace BudgetPlanner.Shared.Enums
{
    [Flags]
    public enum SyncTypes
    {
        Account = 1,
        Balance = 2,
        DirectDebits = 4,
        StandingOrders = 8,
        Transactions = 16,
        PendingTransactions = 32,
        All = 64,
    }
}
