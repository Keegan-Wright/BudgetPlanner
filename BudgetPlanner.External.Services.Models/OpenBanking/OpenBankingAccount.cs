using System.Text.Json.Serialization;

namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class OpenBankingAccount
    {
        [JsonPropertyName("update_timestamp")]
        public DateTime UpdateTimestamp { get; set; }

        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }

        [JsonPropertyName("account_type")]
        public string AccountType { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        public string Currency { get; set; }

        [JsonPropertyName("account_number")]
        public OpenBankingAccountNumber AccountNumber { get; set; }

        public OpenBankingAccountProvider Provider { get; set; }
    }

}
