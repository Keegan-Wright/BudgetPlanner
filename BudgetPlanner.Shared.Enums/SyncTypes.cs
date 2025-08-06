﻿using System.Text.Json.Serialization;

namespace BudgetPlanner.Shared.Enums
{
    [Flags, JsonConverter(typeof(JsonStringEnumConverter<SyncTypes>))]
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
