using BudgetPlanner.Shared.Models.Request.Classifications;
using BudgetPlanner.Shared.Models.Response.Classifications;

namespace BudgetPlanner.Client.Services.Classifications;

public class ClassificationsRequestService : BaseRequestService, IClassificationsRequestService
{
    public ClassificationsRequestService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public override string BaseRoute { get; init; }
    public IAsyncEnumerable<ClassificationsResponse> GetAllCustomClassificationsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<GetClassificationResponse> GetClassificationAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ClassificationsResponse> AddCustomClassificationAsync(AddClassificationsRequest classification)
    {
        throw new NotImplementedException();
    }

    public Task AddCustomClassificationsToTransactionAsync(AddCustomClassificationsToTransactionRequest requestModel)
    {
        throw new NotImplementedException();
    }

    public Task RemoveCustomClassificationAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}