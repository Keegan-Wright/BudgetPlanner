using BudgetPlanner.Shared.Enums;
using System.ComponentModel;

namespace BudgetPlanner.Shared.Models.Request.Account;

public class AccountAndTransactionsRequest
{
    [Description("Number of transactions to retrieve")]
    public int TransactionsCount { get; set; }

    [Description("Type of synchronization to perform")]
    public SyncTypes SyncTypes { get; set; }
}