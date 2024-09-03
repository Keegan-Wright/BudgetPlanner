using BudgetPlanner.Enums;
using BudgetPlanner.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Services.Accounts
{
    public interface IAccountsService
    {
        IAsyncEnumerable<AccountAndTransactionsResponse> GetAccountsAndMostRecentTransactionsAsync(int transactionsToReturn, SyncTypes syncFlags = SyncTypes.All, IProgress<string>? progress = null);
    }

}
