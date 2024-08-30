namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingListAllAccountsResponseModel
    {
        public IAsyncEnumerable<ExternalOpenBankingAccount> Results { get; set; }
        public string Status { get; set; }
    }

}
