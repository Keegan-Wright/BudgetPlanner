using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Transaction
{
    public class TransactionTypeFilterResponse
    {
        [Description("Type name for filtering transactions")]
        public required string TransactionType { get; set; }
    }
}
