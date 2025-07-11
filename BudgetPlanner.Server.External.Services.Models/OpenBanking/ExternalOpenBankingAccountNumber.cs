﻿using System.Text.Json.Serialization;

namespace BudgetPlanner.Server.External.Services.Models.OpenBanking
{
    public class ExternalOpenBankingAccountNumber
    {
        public string Iban { get; set; }

        [JsonPropertyName("swift_bic")]
        public string SwiftBic { get; set; }
        public string Number { get; set; }

        [JsonPropertyName("sort_code")]
        public string SortCode { get; set; }
    }

}
