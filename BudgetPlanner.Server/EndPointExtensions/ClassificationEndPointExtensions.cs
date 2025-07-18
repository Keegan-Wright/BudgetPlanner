using System.ComponentModel;
using BudgetPlanner.Server.Services.Classifications;
using BudgetPlanner.Shared.Models.Request.Classifications;
using BudgetPlanner.Shared.Models.Response.Classifications;
using Microsoft.AspNetCore.OpenApi;

namespace BudgetPlanner.Server.EndPoints;

public static class ClassificationEndPointExtensions
{
    /// <summary>
    /// Maps the classification endpoints to the application
    /// </summary>
    /// <param name="app">The web application instance</param>
    public static void MapClassificationEndPoint(this WebApplication app)
    {
        var classificationsGroup = app.MapGroup("/Classifications")
            .WithTags("Classifications")
            .WithSummary("Classification Management")
            .WithDescription("Endpoints for managing transaction classifications and custom classification rules")
            .RequireAuthorization();

        classificationsGroup.MapGet("/GetAll", async (IClassificationService classificationService) =>
        {
            async IAsyncEnumerable<ClassificationsResponse> ClassificationsStream()
            {
                await foreach (var classification in classificationService.GetAllCustomClassificationsAsync())
                {
                    yield return classification;
                }
            }
            
            return ClassificationsStream();
        })
            .WithSummary("Get All Classifications")
            .WithDescription("Retrieves a stream of all custom classifications")
            .Produces<IAsyncEnumerable<ClassificationsResponse>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        
        classificationsGroup.MapGet("/Get/id", async ([Description("The unique identifier of the classification to retrieve")] Guid id, IClassificationService classificationService) =>
        {
            return await classificationService.GetClassificationAsync(id);
        })
            .WithSummary("Get Classification by ID")
            .WithDescription("Retrieves a specific classification by its ID")
            .Produces<ClassificationsResponse>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

        classificationsGroup.MapPost("/AddClassification",
            async ([Description("The request containing the classification details to add")] AddClassificationsRequest request, IClassificationService classificationService) =>
            {
                return await classificationService.AddCustomClassificationAsync(request);
            })
            .WithSummary("Add Classification")
            .WithDescription("Creates a new custom classification")
            .Accepts<AddClassificationsRequest>("application/json")
            .Produces<ClassificationsResponse>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);
        
        classificationsGroup.MapPost("/AddClassificationsToTransaction",
            async ([Description("The request containing the transaction ID and classifications to apply")] AddCustomClassificationsToTransactionRequest request, IClassificationService classificationService) =>
            {
                await classificationService.AddCustomClassificationsToTransactionAsync(request);
            })
            .WithSummary("Add Classifications to Transaction")
            .WithDescription("Applies custom classifications to a specific transaction")
            .Accepts<AddCustomClassificationsToTransactionRequest>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        classificationsGroup.MapDelete("/DeleteClassification/{id}",
            async ([Description("The unique identifier of the classification to delete")] Guid id, IClassificationService classificationService) =>
            {
                await classificationService.RemoveCustomClassificationAsync(id);
            })
            .WithSummary("Delete Classification")
            .WithDescription("Removes a custom classification by its ID")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }
}