using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Request.Classifications;

public class AddCustomClassificationsToTransactionRequest
{
    [Description("Unique identifier of the transaction to classify")]
    public required Guid TransactionId { get; set; }

    [Description("Collection of classifications to apply to the transaction")]
    public IEnumerable<SelectedCustomClassificationsRequest> Classifications { get; set; }
}