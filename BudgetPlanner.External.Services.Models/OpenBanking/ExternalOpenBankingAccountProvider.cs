using System.Text.Json.Serialization;

namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingAccountProvider
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("provider_id")]
        public string ProviderId { get; set; }

        [JsonPropertyName("logo_uri")]
        public string LogoUri { get; set; }
    }

}
