using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Shared.Models.Request.Classifications
{
    public class AddClassificationsRequest
    {
        public required string Tag { get; set; }
    }

    public class AddCustomClassificationsToTransactionRequest
    {
        public required Guid TransactionId { get; set; }
        public IEnumerable<SelectedCustomClassificationsRequest> Classifications { get; set; }
    }
    public class SelectedCustomClassificationsRequest
    {
        public required Guid ClassificationId { get; set; }
    }
}
