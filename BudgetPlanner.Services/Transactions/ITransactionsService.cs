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
        IAsyncEnumerable<TransactionResponse> GetAllTransactionsAsync(FilteredTransactionsRequest filteredTransactionsRequest, SyncTypes syncTypes = SyncTypes.All);
        IAsyncEnumerable<TransactionAccountFilterResponse> GetAccountsForTransactionFiltersAsync(SyncTypes syncTypes = SyncTypes.Account);
        IAsyncEnumerable<TransactionProviderFilterResponse> GetProvidersForTransactionFiltersAsync();
        IAsyncEnumerable<TransactionTypeFilterResponse> GetTypesForTransactionFiltersAsync();
        IAsyncEnumerable<TransactionCategoryFilterResponse> GetCategoriesForTransactionFiltersAsync();

    }
}
