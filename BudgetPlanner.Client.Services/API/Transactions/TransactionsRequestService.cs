using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.Transaction;
using BudgetPlanner.Shared.Models.Response.Transaction;

namespace BudgetPlanner.Client.Services.Transactions;

public class TransactionsRequestService : BaseRequestService,ITransactionsRequestService
{
    public TransactionsRequestService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public override string BaseRoute { get; init; }

    public IAsyncEnumerable<TransactionResponse> GetAllTransactionsAsync(FilteredTransactionsRequest filteredTransactionsRequest,
        SyncTypes syncTypes = SyncTypes.All)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<TransactionAccountFilterResponse> GetAccountsForTransactionFiltersAsync(SyncTypes syncTypes = SyncTypes.Account)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<TransactionProviderFilterResponse> GetProvidersForTransactionFiltersAsync()
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<TransactionTypeFilterResponse> GetTypesForTransactionFiltersAsync()
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<TransactionCategoryFilterResponse> GetCategoriesForTransactionFiltersAsync()
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<TransactionTagFilterResponse> GetTagsForTransactionFiltersAsync()
    {
        throw new NotImplementedException();
    }
}