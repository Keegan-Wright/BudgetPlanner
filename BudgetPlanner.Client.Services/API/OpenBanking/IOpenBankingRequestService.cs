using BudgetPlanner.Server.External.Services.Models.OpenBanking;
using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.OpenBanking;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.OpenBanking;

namespace BudgetPlanner.Client.Services.OpenBanking;

public interface IOpenBankingRequestService
{
    IAsyncEnumerable<ExternalOpenBankingProvider> GetOpenBankingProvidersForClientAsync();
    Task<AuthUrlResponse> BuildAuthUrl(GetProviderSetupUrlRequestModel setupProviderRequestModel);
    Task<GenericSuccessResponse> AddVendorViaAccessCodeAsync(AddVendorRequestModel requestModel);
    Task PerformSyncAsync(SyncTypes syncFlags, IProgress<string>? progress = null);
}