namespace BudgetPlanner.Shared.Models.Response.Account
{
    public class AccountTransactionResponse
    {
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime Time { get; set; }

        public string Status { get; set; }
    }
}
