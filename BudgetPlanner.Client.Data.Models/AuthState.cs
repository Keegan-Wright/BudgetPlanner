namespace BudgetPlanner.Client.Data.Models;

public class AuthState : BaseEntity
{
    public required string AccessToken { get; set; }
    public required Guid RefreshToken { get; set; }
}