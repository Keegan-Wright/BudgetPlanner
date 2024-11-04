using BudgetPlanner.Enums;
using BudgetPlanner.Models.Request.Classifications;
using BudgetPlanner.Models.Request.Transaction;
using BudgetPlanner.Models.Response.Classifications;
using BudgetPlanner.Models.Response.Transaction;

namespace BudgetPlanner.Services.Classifications
{
    public interface IClassificationService
    {
        IAsyncEnumerable<ClassificationsResponse> GetAllClassificationsAsync();
        IAsyncEnumerable<ClassificationsResponse> GetAllCustomClassificationsAsync();
        IAsyncEnumerable<ClassificationsResponse> GetAllExternalClassificationsAsync();
        Task<GetClassificationResponse> GetClassificationAsync(Guid id);
        Task<ClassificationsResponse> AddCustomClassificationAsync(AddClassificationsRequest classification);
    }
}
