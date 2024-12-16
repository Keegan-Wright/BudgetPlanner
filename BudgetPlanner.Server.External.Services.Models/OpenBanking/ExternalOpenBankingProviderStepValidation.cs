using System.Text.Json.Serialization;

namespace BudgetPlanner.Server.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingProviderStepValidation
    {
        public string Regex { get; set; }

        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; }
    }


}
