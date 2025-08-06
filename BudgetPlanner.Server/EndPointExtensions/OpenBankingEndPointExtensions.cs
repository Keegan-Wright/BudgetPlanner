using System.ComponentModel;
using System.Security.Claims;
using BudgetPlanner.Server.External.Services.Models.OpenBanking;
using BudgetPlanner.Server.Services.OpenBanking;
using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.OpenBanking;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.OpenBanking;
using Microsoft.AspNetCore.OpenApi;

namespace BudgetPlanner.Server.EndPoints;

public static class OpenBankingEndPointExtensions
{
    /// <summary>
    /// Maps the Open Banking endpoints to the application
    /// </summary>
    /// <param name="app">The web application instance</param>
    public static void MapOpenBankingEndPoint(this WebApplication app)
    {
        var openBankingGroup = app.MapGroup("/OpenBanking")
            .WithTags("Open Banking")
            .WithSummary("Open Banking Management")
            .WithDescription("Endpoints for managing open banking providers, vendors, and synchronization")
            .RequireAuthorization();

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
        })
            .WithSummary("Get Open Banking Providers")
            .WithDescription("Retrieves a stream of available open banking providers for the client")
            .Produces<IAsyncEnumerable<ExternalOpenBankingProvider>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        openBankingGroup.MapPost("/AddVendor", async ([Description("The request containing the access code for the vendor")] AddVendorRequestModel request, IOpenBankingService openBankingService) =>
        {
            return new GenericSuccessResponse()
            {
                Success = await openBankingService.AddVendorViaAccessCodeAsync(request.AccessCode)
            };
        })
            .WithSummary("Add Open Banking Vendor")
            .WithDescription("Adds a new open banking vendor using an access code")
            .Accepts<AddVendorRequestModel>("application/json")
            .Produces<GenericSuccessResponse>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);

        openBankingGroup.MapPost("/Sync", async ([Description("The types of data to synchronize")] SyncTypes syncTypes, IOpenBankingService openBankingService) =>
        {
            await openBankingService.PerformSyncAsync(syncTypes);
        })
            .WithSummary("Sync Open Banking Data")
            .WithDescription("Performs synchronization for the specified sync types")
            .Accepts<SyncTypes>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);

        openBankingGroup.MapPost("/GetAuthUrl",
            ([Description("The request containing the provider setup details")] GetProviderSetupUrlRequestModel request, IOpenBankingService openBankingService) =>
            {
                return new AuthUrlResponse() { AuthUrl = openBankingService.BuildAuthUrl(request) };
            })
            .WithSummary("Get Open Banking Auth URL")
            .WithDescription("Retrieves the authentication URL for setting up an open banking provider")
            .Accepts<GetProviderSetupUrlRequestModel>("application/json")
            .Produces<AuthUrlResponse>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);
    }
}