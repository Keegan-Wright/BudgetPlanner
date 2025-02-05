namespace BudgetPlanner.Shared.Models.Response.Auth;

public class LoginResponse
{
    public  string? AccessToken { get; set; }
    public  Guid? RefreshToken { get; set; }
    public required bool Success { get; set; }
    public IEnumerable<String>? Errors { get; set; }
}