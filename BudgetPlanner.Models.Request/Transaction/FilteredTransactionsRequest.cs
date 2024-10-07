using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Models.Request.Transaction
{
    public class FilteredTransactionsRequest
    {
        public string? AccountId { get; set; }
        public Guid? ProviderId { get; set; }
        public string? Category { get; set; }
        public string? Type { get; set; }
        public string? SearchTerm { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}
