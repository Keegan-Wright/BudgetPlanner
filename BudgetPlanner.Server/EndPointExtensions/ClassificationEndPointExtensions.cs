using BudgetPlanner.Server.Services.Classifications;
using BudgetPlanner.Shared.Models.Request.Classifications;
using BudgetPlanner.Shared.Models.Response.Classifications;

namespace BudgetPlanner.Server.EndPoints;

public static class ClassificationEndPointExtensions
{
    public static void MapClassificationEndPoint(this WebApplication app)
    {
        var classificationsGroup = app.MapGroup("/Classifications").RequireAuthorization();;

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
        });
        
        classificationsGroup.MapGet("/Get/id", async (Guid id, IClassificationService classificationService) =>
        {
            return await classificationService.GetClassificationAsync(id);
        });

        classificationsGroup.MapPost("/AddClassification",
            async (AddClassificationsRequest request, IClassificationService classificationService) =>
            {
                return await classificationService.AddCustomClassificationAsync(request);
            });
        
        classificationsGroup.MapPost("/AddClassificationsToTransaction",
            async (AddCustomClassificationsToTransactionRequest request, IClassificationService classificationService) =>
            {
                await classificationService.AddCustomClassificationsToTransactionAsync(request);
        });

        classificationsGroup.MapDelete("/DeleteClassification/{id}",
            async (Guid id, IClassificationService classificationService) =>
            {
                await classificationService.RemoveCustomClassificationAsync(id);
            });
    }
}