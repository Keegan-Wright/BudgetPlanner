using BudgetPlanner.Shared.Models.Request.Auth;
using BudgetPlanner.Shared.Models.Response;

namespace BudgetPlanner.Client.Services.Auth;

public interface IAuthenticationService
{
    Task<GenericSuccessResponse> RegisterAsync(RegisterRequest request);
    Task<GenericSuccessResponse> LoginAsync(LoginRequest request);
}