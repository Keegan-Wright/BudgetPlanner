namespace BudgetPlanner.Shared.Models.Response.Auth;

public class RegisterResponse
{ public required bool Success { get; set; }
    public IEnumerable<String>? Errors { get; set; }
}