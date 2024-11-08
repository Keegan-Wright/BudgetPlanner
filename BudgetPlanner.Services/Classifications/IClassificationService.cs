using BudgetPlanner.Enums;
using BudgetPlanner.Models.Request.Classifications;
using BudgetPlanner.Models.Request.Transaction;
using BudgetPlanner.Models.Response.Classifications;
using BudgetPlanner.Models.Response.Transaction;

namespace BudgetPlanner.Services.Classifications
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
