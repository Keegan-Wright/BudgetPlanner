using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.Auth;

public class RegisterResponse
{
    [Description("Indicates if registration was successful")]
    public required bool Success { get; set; }

    [Description("List of error messages if registration failed")]
    public IEnumerable<String>? Errors { get; set; }
}