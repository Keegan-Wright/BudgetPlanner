namespace BudgetPlanner.Shared.Models.Response.Auth;

public class TokenResponse
{
    public required string? AccessToken { get; set; }
    public required Guid? RefreshToken { get; set; }
}