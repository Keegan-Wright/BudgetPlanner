using BudgetPlanner.Server.External.Services.Models.OpenBanking;
using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.OpenBanking;

namespace BudgetPlanner.Client.Services.OpenBanking;

public interface IOpenBankingRequestService
{
    IAsyncEnumerable<ExternalOpenBankingProvider> GetOpenBankingProvidersForClientAsync();
    string BuildAuthUrl(GetProviderSetupUrlRequestModel setupProviderRequestModel);
    Task<bool> AddVendorViaAccessCodeAsync(string accessCode);
    Task PerformSyncAsync(SyncTypes syncFlags, IProgress<string>? progress = null);
}