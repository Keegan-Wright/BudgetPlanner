using System.Text.Json.Serialization;

namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingAccountBalance
    {
        public string Currency { get; set; }
        public decimal Available { get; set; }
        public decimal Current { get; set; }

        [JsonPropertyName("update_timestamp ")]
        public DateTime UpdateTimestamp { get; set; }
    }
}
