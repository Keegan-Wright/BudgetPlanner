namespace BudgetPlanner.Server.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingGetAccountBalanceResponseModel
    {
        public IAsyncEnumerable<ExternalOpenBankingAccountBalance> Results { get; set; }
        public string Status { get; set; }
    }

}
