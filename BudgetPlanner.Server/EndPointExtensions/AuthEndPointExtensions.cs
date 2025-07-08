using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Server.Services.Auth;
using BudgetPlanner.Shared.Models.Request.Auth;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.IdentityModel.Tokens;

namespace BudgetPlanner.Server.EndPoints;

public static class AuthEndPointExtensions
{
    /// <summary>
    /// Maps the authentication endpoints to the application
    /// </summary>
    /// <param name="app">The web application instance</param>
    public static void MapAuthEndPoint(this WebApplication app)
    {
        var authGroup = app.MapGroup("/Auth")
            .WithTags("Authorization")
            .WithSummary("Authentication")
            .WithDescription("Endpoints for user authentication and authorization");

        authGroup.MapPost("/Login", async (IAuthService authService, [Description("The login credentials containing username and password")] LoginRequest request) =>
        {
            return await authService.LoginAsync(request);
        })
        .WithSummary("User Login")
        .WithDescription("Authenticates a user and returns a JWT token")
        .Accepts<LoginRequest>("application/json")
        .Produces<LoginResponse>(200, "application/json")
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status400BadRequest);

        authGroup.MapPost("/Register", async (IAuthService authService, [Description("The registration details containing user information")] RegisterRequest request) =>
        {
            return await authService.RegisterAsync(request);
        })
        .WithSummary("User Registration")
        .WithDescription("Registers a new user account")
        .Accepts<RegisterRequest>("application/json")
        .Produces<RegisterResponse>(200, "application/json")
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict);

        authGroup.MapPost("/Logout", async (IAuthService authService, [Description("The HTTP context containing the current user's session information")] HttpContext context) =>
        {
            await authService.LogoutAsync(context.User);
        })
        .WithSummary("User Logout")
        .WithDescription("Logs out the current user and invalidates their session")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .RequireAuthorization();

        authGroup.MapPost("/Token", async (IAuthService authService, [Description("The token refresh request containing the refresh token")] TokenRequest request, [Description("The HTTP context containing the current user's session information")] HttpContext context) =>
        {
            return await authService.ProcessRefreshTokenAsync(request, context.User);
        })
        .WithSummary("Refresh Token")
        .WithDescription("Refreshes the user's authentication token using a refresh token")
        .Accepts<TokenRequest>("application/json")
        .Produces<TokenResponse>(200, "application/json")
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status400BadRequest);
    }
}