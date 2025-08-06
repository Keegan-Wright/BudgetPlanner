using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Request.OpenBanking;

public class AddVendorRequestModel
{
    [Description("Access code received from the banking provider for authentication")]
    public string AccessCode { get; set; }
}