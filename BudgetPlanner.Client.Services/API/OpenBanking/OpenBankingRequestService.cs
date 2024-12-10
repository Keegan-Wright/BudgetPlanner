using BudgetPlanner.Server.External.Services.Models.OpenBanking;
using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.OpenBanking;

namespace BudgetPlanner.Client.Services.OpenBanking;

public class OpenBankingRequestService : BaseRequestService, IOpenBankingRequestService
{
    public OpenBankingRequestService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public override string BaseRoute { get; init; }
    public IAsyncEnumerable<ExternalOpenBankingProvider> GetOpenBankingProvidersForClientAsync()
    {
        throw new NotImplementedException();
    }

    public string BuildAuthUrl(GetProviderSetupUrlRequestModel setupProviderRequestModel)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddVendorViaAccessCodeAsync(string accessCode)
    {
        throw new NotImplementedException();
    }

    public Task PerformSyncAsync(SyncTypes syncFlags, IProgress<string>? progress = null)
    {
        throw new NotImplementedException();
    }
}