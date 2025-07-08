using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Transaction
{
    public class TransactionProviderFilterResponse
    {
        [Description("Unique identifier of the transaction provider")]
        public required Guid ProviderId { get; set; }

        [Description("Name of the transaction provider")]
        public required string ProviderName { get; set; }
    }
}
