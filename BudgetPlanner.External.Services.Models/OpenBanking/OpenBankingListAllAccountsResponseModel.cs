namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class OpenBankingListAllAccountsResponseModel
    {
        public IAsyncEnumerable<OpenBankingAccount> Results { get; set; }
        public string Status { get; set; }
    }

}
