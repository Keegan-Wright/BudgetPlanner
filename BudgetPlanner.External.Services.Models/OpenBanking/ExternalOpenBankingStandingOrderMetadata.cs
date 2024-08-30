using System.Text.Json.Serialization;

namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingStandingOrderMetadata
    {
        [JsonPropertyName("provider_account_id")]
        public string ProviderAccountId { get; set; }
    }

}
