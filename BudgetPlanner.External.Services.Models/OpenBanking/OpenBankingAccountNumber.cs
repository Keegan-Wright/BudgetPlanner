using System.Text.Json.Serialization;

namespace BudgetPlanner.External.Services.Models.OpenBanking
{
    public class OpenBankingAccountNumber
    {
        public string Iban { get; set; }

        [JsonPropertyName("swift_bic")]
        public string SwiftBic { get; set; }
        public string Number { get; set; }

        [JsonPropertyName("sort_code")]
        public string SortCode { get; set; }
    }

}
