using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Request.Auth;

public class LoginRequest
{
    [Description("Username or email for authentication")]
    public required string Username { get; set; }

    [Description("User's password for authentication")]
    public required string Password { get; set; }
}