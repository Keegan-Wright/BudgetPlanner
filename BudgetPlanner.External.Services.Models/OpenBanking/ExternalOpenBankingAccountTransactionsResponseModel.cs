namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingAccountTransactionsResponseModel
    {
        public IAsyncEnumerable<ExternalOpenBankingAccountTransaction> Results { get; set; }
        public string Status { get; set; }
    }
}
