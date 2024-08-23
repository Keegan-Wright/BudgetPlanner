namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class OpenBankingAccountTransactionsResponseModel
    {
        public IAsyncEnumerable<OpenBankingAccountTransaction> Results { get; set; }
        public string Status { get; set; }
    }

}
