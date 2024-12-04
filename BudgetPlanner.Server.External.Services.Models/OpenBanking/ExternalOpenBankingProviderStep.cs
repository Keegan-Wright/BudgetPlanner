namespace BudgetPlanner.Server.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingProviderStep
    {
        public string Title { get; set; }
        public IAsyncEnumerable<ExternalOpenBankingProviderStepField> Fields { get; set; }
    }


}
