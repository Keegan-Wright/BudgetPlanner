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
    }
}
