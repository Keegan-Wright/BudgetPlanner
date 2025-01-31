using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Server.Services.Auth;
using BudgetPlanner.Shared.Models.Request.Auth;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BudgetPlanner.Server.EndPoints;

public static class AuthEndPointExtensions
{
    public static void MapAuthEndPoint(this WebApplication app)
    {
        var authGroup = app.MapGroup("/Auth");


        authGroup.MapPost("/Login", async (IAuthService authService ,LoginRequest request) =>
        {
            return await authService.LoginAsync(request);
        });

        
        authGroup.MapPost("/Register", async (IAuthService authService, RegisterRequest request) =>
        {
            return await authService.RegisterAsync(request);
        });

        authGroup.MapPost("/Logout", async (IAuthService authService, HttpContext context) =>
        {
            await authService.LogoutAsync(context.User);
        });

        authGroup.MapPost("/Token", async (IAuthService authService, TokenRequest request, HttpContext context) =>
        {
            return await authService.ProcessRefreshTokenAsync(request, context.User);
        });
    }
}