using System.Text.Json.Serialization;

namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class OpenBankingAccountBalance
    {
        public string Currency { get; set; }
        public float Available { get; set; }
        public float Current { get; set; }

        [JsonPropertyName("update_timestamp ")]
        public DateTime UpdateTimestamp { get; set; }
    }

}
