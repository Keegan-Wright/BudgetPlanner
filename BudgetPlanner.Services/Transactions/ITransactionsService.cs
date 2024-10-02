using BudgetPlanner.Enums;
using BudgetPlanner.Models.Request.Transaction;
using BudgetPlanner.Models.Response.Transaction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Services.Transactions
{
    public interface ITransactionsService
    {
        IAsyncEnumerable<TransactionResponse> GetAllTransactionsAsync(SyncTypes syncTypes = SyncTypes.All);
        IAsyncEnumerable<TransactionResponse> GetAllTransactionsAsync(FilteredTransactionsRequest filteredTransactionsRequest, SyncTypes syncTypes = SyncTypes.All);

    }
}
