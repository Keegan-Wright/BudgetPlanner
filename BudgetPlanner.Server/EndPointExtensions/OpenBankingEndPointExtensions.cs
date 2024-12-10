using BudgetPlanner.Server.External.Services.Models.OpenBanking;
using BudgetPlanner.Server.Services.OpenBanking;
using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.OpenBanking;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.OpenBanking;

namespace BudgetPlanner.Server.EndPoints;

public static class OpenBankingEndPointExtensions
{
    public static void MapOpenBankingEndPoint(this WebApplication app)
    {
        var openBankingGroup = app.MapGroup("/OpenBanking");

        openBankingGroup.MapGet("/GetProviders", async  (IOpenBankingService openBankingService) =>
        {
            async IAsyncEnumerable<ExternalOpenBankingProvider> OpenBankingProvidersStream()
            {
                await foreach (var provider in openBankingService.GetOpenBankingProvidersForClientAsync())
                {
                    yield return provider;
                }
            }
            return OpenBankingProvidersStream();
        });

        openBankingGroup.MapPost("/AddVendor", async (string accessCode, IOpenBankingService openBankingService) =>
        {
            return new GenericSuccessResponse()
            {
                Success = await openBankingService.AddVendorViaAccessCodeAsync(accessCode)
            };
        });

        openBankingGroup.MapPost("/Sync", async (SyncTypes syncTypes, IOpenBankingService openBankingService) =>
        {
            await openBankingService.PerformSyncAsync(syncTypes);
        });

        openBankingGroup.MapPost("/GetAuthUrl",
            (GetProviderSetupUrlRequestModel request, IOpenBankingService openBankingService) =>
            {
                return new AuthUrlResponse() { AuthUrl = openBankingService.BuildAuthUrl(request) };
            });
    }
}