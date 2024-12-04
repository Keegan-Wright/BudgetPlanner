namespace BudgetPlanner.Shared.Models.Response.Transaction
{
    public class TransactionProviderFilterResponse
    {
        public required Guid ProviderId { get; set; }
        public required string ProviderName { get; set; }
    }
}
