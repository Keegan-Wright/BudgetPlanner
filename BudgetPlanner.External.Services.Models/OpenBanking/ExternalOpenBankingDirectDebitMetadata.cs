using System.Text.Json.Serialization;

namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingDirectDebitMetadata
    {
        [JsonPropertyName("provider_mandate_identification")]
        public string ProviderMandateIdentification { get; set; }

        [JsonPropertyName("provider_account_id")]
        public string ProviderAccountId { get; set; }
    }

}
