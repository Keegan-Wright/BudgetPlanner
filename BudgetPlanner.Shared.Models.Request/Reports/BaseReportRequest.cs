using BudgetPlanner.Shared.Enums;

namespace BudgetPlanner.Shared.Models.Request.Reports;

public class BaseReportRequest
{
    public IList<Guid>? AccountIds { get; set; }
    public IList<Guid>? ProviderIds { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public IList<string>? TransactionTypes { get; set; }
    public IList<string>? Categories { get; set; }
    public required SyncTypes SyncType { get; set; }
}