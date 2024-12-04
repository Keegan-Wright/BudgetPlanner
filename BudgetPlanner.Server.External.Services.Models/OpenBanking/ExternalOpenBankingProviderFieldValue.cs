using System.Text.Json.Serialization;

namespace BudgetPlanner.Server.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingProviderFieldValue
    {
        public string Value { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }
    }


}
