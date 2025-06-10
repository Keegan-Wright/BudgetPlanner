using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Response;

public class GenericSuccessResponse
{
    [Description("Indicates if the operation was successful")]
    public bool Success { get; set; }
}