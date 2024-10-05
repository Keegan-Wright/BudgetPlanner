using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Models.Response.Transaction
{
    public class TransactionResponse
    {
        public required Guid TransactionId { get; set; }
        public required string Description { get; set; }
        public required string TransactionType { get; set; }
        public required string TransactionCategory { get; set; }
        public required decimal Amount { get; set; }
        public required string Currency { get; set; }
        public required DateTime TransactionTime { get; set; }
        public required bool Pending { get; set; }
    }
}
