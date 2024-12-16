using BudgetPlanner.Shared.Models.Request.Classifications;
using BudgetPlanner.Shared.Models.Response.Classifications;

namespace BudgetPlanner.Server.Services.Classifications
{
    public interface IClassificationService
    {
        IAsyncEnumerable<ClassificationsResponse> GetAllCustomClassificationsAsync();
        Task<GetClassificationResponse> GetClassificationAsync(Guid id);
        Task<ClassificationsResponse> AddCustomClassificationAsync(AddClassificationsRequest classification);
        Task AddCustomClassificationsToTransactionAsync (AddCustomClassificationsToTransactionRequest requestModel);
        Task RemoveCustomClassificationAsync(Guid id);
    }
}
