using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Models.Request.Transaction
{
    public class FilteredTransactionsRequest
    {
        public IList<Guid>? AccountIds { get; set; } = [];
        public IList<Guid>? ProviderIds { get; set; } = [];
        public IList<string>? Categories { get; set; } = [];
        public IList<string>? Types { get; set; } = [];
        public string? SearchTerm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public IList<string>? Tags { get; set; } = [];
    }
}
