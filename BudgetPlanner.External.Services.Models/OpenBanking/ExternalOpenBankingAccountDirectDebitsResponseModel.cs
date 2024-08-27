namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingAccountDirectDebitsResponseModel
    {
        public IAsyncEnumerable<ExternalOpenBankingDirectDebit> Results { get; set; }
        public string Status { get; set; }
    }

}
