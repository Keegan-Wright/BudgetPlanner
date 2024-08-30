namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingAccountConnectionResponseModel
    {
        public IAsyncEnumerable<ExternalOpenBankingAccountConnection> Results { get; set; }
    }

}
