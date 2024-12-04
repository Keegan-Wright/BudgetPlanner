using BudgetPlanner.Enums;
using BudgetPlanner.Shared.Models.Request.Classifications;
using BudgetPlanner.Shared.Models.Request.Transaction;
using BudgetPlanner.Shared.Models.Response.Classifications;
using BudgetPlanner.Shared.Models.Response.Transaction;

namespace BudgetPlanner.Client.Services.Classifications
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
