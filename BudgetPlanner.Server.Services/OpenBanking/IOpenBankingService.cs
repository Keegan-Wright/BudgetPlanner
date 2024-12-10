using BudgetPlanner.Server.External.Services.Models.OpenBanking;
using BudgetPlanner.Shared.Models.Request.OpenBanking;
using BudgetPlanner.Shared.Enums;

namespace BudgetPlanner.Server.Services.OpenBanking
{
    public interface IOpenBankingService
    {
        IAsyncEnumerable<ExternalOpenBankingProvider> GetOpenBankingProvidersForClientAsync();
        string BuildAuthUrl(GetProviderSetupUrlRequestModel setupProviderRequestModel);
        Task<bool> AddVendorViaAccessCodeAsync(string accessCode);
        Task PerformSyncAsync(SyncTypes syncFlags, IProgress<string>? progress = null);
    }
}
