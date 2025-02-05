using BudgetPlanner.Server.Services.Transactions;
using BudgetPlanner.Shared.Models.Request.Transaction;
using BudgetPlanner.Shared.Models.Response.Transaction;

namespace BudgetPlanner.Server.EndPoints;

public static class TransactionEndPointExtensions
{
    public static void MapTransactionsEndPoint(this WebApplication app)
    {
        var transactionsGroup = app.MapGroup("/Transactions").RequireAuthorization();;

        transactionsGroup.MapGet("/CategoryFilters", async (ITransactionsService transactionsService) =>
        {
            async IAsyncEnumerable<TransactionCategoryFilterResponse> TransactionCategoriesStream()
            {
                await foreach (var categoryFilter in transactionsService.GetCategoriesForTransactionFiltersAsync())
                {
                    yield return categoryFilter;
                }
            }
            return TransactionCategoriesStream();
        });
        
        transactionsGroup.MapGet("/TypeFilters", async (ITransactionsService transactionsService) =>
        {
            async IAsyncEnumerable<TransactionTypeFilterResponse> TransactionTypeStream()
            {
                await foreach (var transactionTypeFilter in transactionsService.GetTypesForTransactionFiltersAsync())
                {
                    yield return transactionTypeFilter;
                }
            }
            return TransactionTypeStream();
        });
        
        transactionsGroup.MapGet("/ProviderFilters", async (ITransactionsService transactionsService) =>
        {
            async IAsyncEnumerable<TransactionProviderFilterResponse> TransactionProviderStream()
            {
                await foreach (var providerFilter in transactionsService.GetProvidersForTransactionFiltersAsync())
                {
                    yield return providerFilter;
                }
            }
            return TransactionProviderStream();
        });
        
        transactionsGroup.MapGet("/AccountFilters", async (ITransactionsService transactionsService) =>
        {
            async IAsyncEnumerable<TransactionAccountFilterResponse> TransactionAccountStream()
            {
                await foreach (var accountFilter in transactionsService.GetAccountsForTransactionFiltersAsync())
                {
                    yield return accountFilter;
                }
            }
            return TransactionAccountStream();
        });
        
        transactionsGroup.MapGet("/TagFilters", async (ITransactionsService transactionsService) =>
        {
            async IAsyncEnumerable<TransactionTagFilterResponse> TransactionAccountStream()
            {
                await foreach (var tagFilter in transactionsService.GetTagsForTransactionFiltersAsync())
                {
                    yield return tagFilter;
                }
            }
            return TransactionAccountStream();
        });


        transactionsGroup.MapPost("/GetTransactions", async (FilteredTransactionsRequest request, ITransactionsService transactionsService) =>
        {
            async IAsyncEnumerable<TransactionResponse> TransactionAccountStream()
            {
                await foreach (var transaction in transactionsService.GetAllTransactionsAsync(request, request.SyncType))
                {
                    yield return transaction;
                }
            }
            return TransactionAccountStream();
        });


    }
}