namespace BudgetPlanner.Shared.Models.Request.Auth;

public class TokenRequest
{
    public required Guid RefreshToken { get; set; }
}