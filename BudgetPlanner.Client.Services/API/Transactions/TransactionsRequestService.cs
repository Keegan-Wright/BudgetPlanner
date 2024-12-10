using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.Transaction;
using BudgetPlanner.Shared.Models.Response.Transaction;

namespace BudgetPlanner.Client.Services.Transactions;

public class TransactionsRequestService : BaseRequestService,ITransactionsRequestService
{
    public TransactionsRequestService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        BaseRoute = "Transactions";
    }

    public override string BaseRoute { get; init; }

    public async IAsyncEnumerable<TransactionResponse> GetAllTransactionsAsync(FilteredTransactionsRequest filteredTransactionsRequest,
        SyncTypes syncTypes = SyncTypes.All)
    {
        filteredTransactionsRequest.SyncType = syncTypes; // hack for now
        await foreach(var transaction in PostAsyncEnumerableAsync<FilteredTransactionsRequest, TransactionResponse>("GetTransactions", filteredTransactionsRequest))
        {
            yield return transaction;
        }
    }

    public async IAsyncEnumerable<TransactionAccountFilterResponse> GetAccountsForTransactionFiltersAsync()
    {
        await foreach (var account in GetAsyncEnumerable<TransactionAccountFilterResponse>("AccountFilters"))
        {
            yield return account;
        }
    }

    public async IAsyncEnumerable<TransactionProviderFilterResponse> GetProvidersForTransactionFiltersAsync()
    {
        await foreach (var provider in GetAsyncEnumerable<TransactionProviderFilterResponse>("ProviderFilters"))
        {
            yield return provider;
        }
    }

    public async IAsyncEnumerable<TransactionTypeFilterResponse> GetTypesForTransactionFiltersAsync()
    {
        await foreach (var transactionType in GetAsyncEnumerable<TransactionTypeFilterResponse>("TypeFilters"))
        {
            yield return transactionType;
        }
    }

    public async IAsyncEnumerable<TransactionCategoryFilterResponse> GetCategoriesForTransactionFiltersAsync()
    {
        await foreach (var category in GetAsyncEnumerable<TransactionCategoryFilterResponse>("CategoryFilters"))
        {
            yield return category;
        }
    }

    public async IAsyncEnumerable<TransactionTagFilterResponse> GetTagsForTransactionFiltersAsync()
    {
        await foreach (var filter in GetAsyncEnumerable<TransactionTagFilterResponse>("TagFilters"))
        {
            yield return filter;
        }
    }
}