using System.Security.Claims;
using BudgetPlanner.Shared.Models.Request.Auth;
using BudgetPlanner.Shared.Models.Response.Auth;

namespace BudgetPlanner.Server.Services.Auth;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task LogoutAsync(ClaimsPrincipal contextUser);
    Task<TokenResponse> ProcessRefreshTokenAsync(TokenRequest request, ClaimsPrincipal contextUser);
}