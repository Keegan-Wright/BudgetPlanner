namespace BudgetPlanner.Shared.Models.Response.Transaction
{
    public class TransactionAccountFilterResponse
    {
        public required Guid AccountId { get; set; }
        public required string AccountName { get; set; }
    }
}
