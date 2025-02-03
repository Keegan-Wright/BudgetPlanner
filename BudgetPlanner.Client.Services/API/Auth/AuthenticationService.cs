using BudgetPlanner.Client.Data.Db;
using BudgetPlanner.Client.Data.Models;
using BudgetPlanner.Shared.Models.Request.Auth;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.Auth;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Client.Services.Auth;

public class AuthenticationService : BaseRequestService, IAuthenticationService
{
    public AuthenticationService(IHttpClientFactory httpClientFactory, BudgetPlannerDbContext budgetPlannerDbContext) : base(httpClientFactory, budgetPlannerDbContext)
    {
        BaseRoute = "auth";
    }
    
    public sealed override string BaseRoute { get; init; }
    
    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var response = await PostAsync<RegisterRequest, RegisterResponse>("Register", request);
        return response;
    }
    
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var response = await PostAsync<LoginRequest, LoginResponse>("Login", request);

        if (response.Errors is null)
        {
            var  authState = new AuthState()
            {
                RefreshToken = response.RefreshToken.GetValueOrDefault(),
                AccessToken = response.AccessToken,
                Created = DateTime.UtcNow
            };
            await _budgetPlannerDbContext.AuthState.ExecuteDeleteAsync();
                await _budgetPlannerDbContext.AuthState.AddAsync(authState);
            await _budgetPlannerDbContext.SaveChangesAsync();
        }
        
        return response;
    }

    public async Task LogoutAsync()
    {
        await PostAsync("Logout");
        await _budgetPlannerDbContext.AuthState.ExecuteDeleteAsync();
    }

    public async Task<TokenResponse> RefreshTokenAsync(TokenRequest request)
    {
        var response = await PostAsync<TokenRequest, TokenResponse>("Token", request);
        
        var authState = _budgetPlannerDbContext.AuthState.FirstOrDefault();
        authState.RefreshToken = response.RefreshToken.GetValueOrDefault();
        authState.AccessToken = response.AccessToken;
        await _budgetPlannerDbContext.SaveChangesAsync();
        
        return response;
    }
}