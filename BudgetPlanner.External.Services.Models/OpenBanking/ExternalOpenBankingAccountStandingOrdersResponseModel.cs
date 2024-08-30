namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingAccountStandingOrdersResponseModel
    {
        public IAsyncEnumerable<ExternalOpenBankingAccountStandingOrder>? Results { get; set; }
        public string Status { get; set; }
    }

}
