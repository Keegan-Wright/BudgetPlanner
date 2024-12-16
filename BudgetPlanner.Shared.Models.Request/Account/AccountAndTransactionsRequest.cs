using BudgetPlanner.Shared.Enums;

namespace BudgetPlanner.Shared.Models.Request.Account;

public class AccountAndTransactionsRequest
{
    public int TransactionsCount { get; set; }
    public SyncTypes SyncTypes { get; set; }
}