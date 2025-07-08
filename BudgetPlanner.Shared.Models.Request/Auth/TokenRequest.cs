using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Request.Auth;

public class TokenRequest
{
    [Description("Refresh token used to obtain a new access token")]
    public required Guid RefreshToken { get; set; }
}