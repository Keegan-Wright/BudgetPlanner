using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Auth;

public class LoginResponse
{
    [Description("JWT access token for authentication")]
    public string? AccessToken { get; set; }

    [Description("Refresh token for obtaining new access tokens")]
    public Guid? RefreshToken { get; set; }

    [Description("Indicates if login was successful")]
    public required bool Success { get; set; }

    [Description("List of error messages if login failed")]
    public IEnumerable<String>? Errors { get; set; }
}