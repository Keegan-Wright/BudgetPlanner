using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Request.Classifications;

public class SelectedCustomClassificationsRequest
{
    [Description("Unique identifier of the classification to apply")]
    public required Guid ClassificationId { get; set; }
}