using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.Transaction;
using BudgetPlanner.Shared.Models.Response.Transaction;

namespace BudgetPlanner.Client.Services.Transactions;

public interface ITransactionsRequestService
{
    IAsyncEnumerable<TransactionResponse> GetAllTransactionsAsync(FilteredTransactionsRequest filteredTransactionsRequest, SyncTypes syncTypes = SyncTypes.All);
    IAsyncEnumerable<TransactionAccountFilterResponse> GetAccountsForTransactionFiltersAsync(SyncTypes syncTypes = SyncTypes.Account);
    IAsyncEnumerable<TransactionProviderFilterResponse> GetProvidersForTransactionFiltersAsync();
    IAsyncEnumerable<TransactionTypeFilterResponse> GetTypesForTransactionFiltersAsync();
    IAsyncEnumerable<TransactionCategoryFilterResponse> GetCategoriesForTransactionFiltersAsync();
    IAsyncEnumerable<TransactionTagFilterResponse> GetTagsForTransactionFiltersAsync();
}