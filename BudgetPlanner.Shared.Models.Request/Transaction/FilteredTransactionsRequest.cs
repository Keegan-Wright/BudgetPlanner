﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetPlanner.Shared.Enums;
using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Request.Transaction
{
    public class FilteredTransactionsRequest
    {
        [Description("List of account identifiers to filter transactions")]
        public IList<Guid>? AccountIds { get; set; } = [];
        [Description("List of banking provider identifiers to filter transactions")]
        public IList<Guid>? ProviderIds { get; set; } = [];
        [Description("List of transaction categories to filter transactions")]
        public IList<string>? Categories { get; set; } = [];
        [Description("List of transaction types to filter transactions")]
        public IList<string>? Types { get; set; } = [];
        [Description("Search term to filter transactions by description or other text fields")]
        public string? SearchTerm { get; set; }
        [Description("Start date for filtering transactions")]
        public DateTime? FromDate { get; set; }
        [Description("End date for filtering transactions")]
        public DateTime? ToDate { get; set; }
        [Description("List of tags to filter transactions")]
        public IList<string>? Tags { get; set; } = [];
        

        [Description("Type of synchronization to perform")]
        public SyncTypes SyncType { get; set; }
        
    }
}
