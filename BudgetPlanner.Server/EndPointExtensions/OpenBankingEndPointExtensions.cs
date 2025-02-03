using System.Security.Claims;
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
        var openBankingGroup = app.MapGroup("/OpenBanking").RequireAuthorization();;

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

        openBankingGroup.MapPost("/AddVendor", async (AddVendorRequestModel request, IOpenBankingService openBankingService) =>
        {
            return new GenericSuccessResponse()
            {
                Success = await openBankingService.AddVendorViaAccessCodeAsync(request.AccessCode)
            };
        });

        openBankingGroup.MapPost("/Sync", async (SyncTypes syncTypes, IOpenBankingService openBankingService, ClaimsPrincipal principal) =>
        {
            await openBankingService.PerformSyncAsync(syncTypes, Guid.Parse(principal.Claims.First(x => x.Type == ClaimTypes.Sid).Value));
        });

        openBankingGroup.MapPost("/GetAuthUrl",
            (GetProviderSetupUrlRequestModel request, IOpenBankingService openBankingService) =>
            {
                return new AuthUrlResponse() { AuthUrl = openBankingService.BuildAuthUrl(request) };
            });
    }
}