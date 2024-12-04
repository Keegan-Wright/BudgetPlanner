﻿namespace BudgetPlanner.Server.Models.Configuration
{
    public class TrueLayerOpenBankingConfiguration
    {
        public string BaseAuthUrl { get; set; }
        public string BaseDataUrl { get; set; }
        public string AuthRedirectUrl { get; set; }
        public string ClientId { get; set; }
        public Guid ClientSecret { get; set; }

    }
}
