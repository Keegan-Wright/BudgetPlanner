using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response.OpenBanking;

public class AuthUrlResponse
{
    [Description("URL for authenticating with the banking provider")]
    public string AuthUrl { get; set; }
}