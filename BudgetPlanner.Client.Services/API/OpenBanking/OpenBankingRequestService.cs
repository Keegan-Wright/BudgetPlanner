using BudgetPlanner.Server.External.Services.Models.OpenBanking;
using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.OpenBanking;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.OpenBanking;

namespace BudgetPlanner.Client.Services.OpenBanking;

public class OpenBankingRequestService : BaseRequestService, IOpenBankingRequestService
{
    public OpenBankingRequestService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        BaseRoute = "OpenBanking";
    }

    public sealed override string BaseRoute { get; init; }
    public async IAsyncEnumerable<ExternalOpenBankingProvider> GetOpenBankingProvidersForClientAsync()
    {
        await foreach (var provider in GetAsyncEnumerable<ExternalOpenBankingProvider>("GetProviders"))
        {
            yield return provider;
        }
    }

    public async Task<AuthUrlResponse> BuildAuthUrl(GetProviderSetupUrlRequestModel setupProviderRequestModel)
    {
        return await PostAsync<GetProviderSetupUrlRequestModel, AuthUrlResponse>("GetAuthUrl", setupProviderRequestModel);
    }

    public async Task<GenericSuccessResponse> AddVendorViaAccessCodeAsync(AddVendorRequestModel requestModel)
    {
        return await PostAsync<AddVendorRequestModel, GenericSuccessResponse>("AddVendor", requestModel);
    }

    public async Task PerformSyncAsync(SyncTypes syncFlags, IProgress<string>? progress = null)
    {
        await PostAsync("Sync", syncFlags);
    }
}