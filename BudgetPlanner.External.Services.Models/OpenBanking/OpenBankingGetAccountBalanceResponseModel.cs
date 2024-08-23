namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class OpenBankingGetAccountBalanceResponseModel
    {
        public IAsyncEnumerable<OpenBankingAccountBalance> Results { get; set; }
        public string Status { get; set; }
    }

}
