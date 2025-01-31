using BudgetPlanner.Shared.Models.Request.Auth;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.Auth;

namespace BudgetPlanner.Client.Services.Auth;

public interface IAuthenticationService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    
    Task LogoutAsync();
    Task<TokenResponse> RefreshTokenAsync(TokenRequest request);
}