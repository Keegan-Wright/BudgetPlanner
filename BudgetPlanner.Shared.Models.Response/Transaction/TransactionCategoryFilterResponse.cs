﻿using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Transaction
{
    public class TransactionCategoryFilterResponse
    {
        [Description("Category name for filtering transactions")]
        public required string TransactionCategory { get; set; }
    }
}
