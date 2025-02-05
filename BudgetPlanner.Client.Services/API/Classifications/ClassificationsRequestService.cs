using BudgetPlanner.Client.Data.Db;
using BudgetPlanner.Shared.Models.Request.Classifications;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.Classifications;

namespace BudgetPlanner.Client.Services.Classifications;

public class ClassificationsRequestService : BaseRequestService, IClassificationsRequestService
{
    public ClassificationsRequestService(IHttpClientFactory httpClientFactory, BudgetPlannerDbContext budgetPlannerDbContext) : base(httpClientFactory, budgetPlannerDbContext)
    {
        BaseRoute = "classifications";
    }

    public sealed override string BaseRoute { get; init; }
    public async IAsyncEnumerable<ClassificationsResponse> GetAllCustomClassificationsAsync()
    {
        await foreach (var classification in GetAsyncEnumerable<ClassificationsResponse>("GetAll"))
        {
            yield return classification;
        }
    }

    public async Task<GetClassificationResponse> GetClassificationAsync(Guid id)
    {
        return await GetAsync<GetClassificationResponse>($"Get/{id}");
    }

    public async Task<ClassificationsResponse> AddCustomClassificationAsync(AddClassificationsRequest classification)
    {
        return await PostAsync<AddClassificationsRequest, ClassificationsResponse>("AddClassification", classification);
    }

    public async Task AddCustomClassificationsToTransactionAsync(AddCustomClassificationsToTransactionRequest requestModel)
    {
        await PostAsync<AddCustomClassificationsToTransactionRequest>("AddClassificationsToTransaction", requestModel);
    }

    public async Task RemoveCustomClassificationAsync(Guid id)
    {
        await DeleteAsync<GenericSuccessResponse>($"DeleteClassification/{id}");
    }
}