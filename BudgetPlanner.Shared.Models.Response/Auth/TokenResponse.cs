using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Auth;

public class TokenResponse
{
    [Description("New JWT access token for authentication")]
    public required string? AccessToken { get; set; }

    [Description("New refresh token for obtaining future access tokens")]
    public required Guid? RefreshToken { get; set; }
}