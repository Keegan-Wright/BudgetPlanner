using BudgetPlanner.Shared.Enums;
using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Request.Reports;

public class BaseReportRequest
{
    [Description("List of account identifiers to include in the report")]
    public IList<Guid>? AccountIds { get; set; }

    [Description("List of banking provider identifiers to include in the report")]
    public IList<Guid>? ProviderIds { get; set; }

    [Description("Start date for the report period")]
    public DateTime FromDate { get; set; }

    [Description("End date for the report period")]
    public DateTime ToDate { get; set; }

    [Description("List of transaction types to include in the report")]
    public IList<string>? TransactionTypes { get; set; }

    [Description("List of transaction categories to include in the report")]
    public IList<string>? Categories { get; set; }


    [Description("Type of synchronization to perform")]
    public required SyncTypes SyncType { get; set; }
}