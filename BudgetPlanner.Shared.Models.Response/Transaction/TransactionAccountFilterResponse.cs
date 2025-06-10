﻿using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Transaction
{
    public class TransactionAccountFilterResponse
    {
        [Description("Unique identifier of the account")]
        public required Guid AccountId { get; set; }

        [Description("Name of the account")]
        public required string AccountName { get; set; }
    }
}
