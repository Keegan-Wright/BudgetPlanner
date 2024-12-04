using BudgetPlanner.Data.Db;
using BudgetPlanner.Data.Models;
using BudgetPlanner.Enums;
using BudgetPlanner.Server.External.Services.Models.OpenBanking;
using BudgetPlanner.Shared.Models.Request.OpenBanking;
using BudgetPlanner.Client.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Client.Services.OpenBanking
{
    public interface IOpenBankingService
    {
        IAsyncEnumerable<ExternalOpenBankingProvider> GetOpenBankingProvidersForClientAsync();
        string BuildAuthUrl(GetProviderSetupUrlRequestModel setupProviderRequestModel);
        Task<bool> AddVendorViaAccessCodeAsync(string accessCode);
        Task PerformSyncAsync(SyncTypes syncFlags, IProgress<string>? progress = null);
    }
}
