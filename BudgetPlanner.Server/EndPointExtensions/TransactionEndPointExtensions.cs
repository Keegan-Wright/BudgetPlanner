using System.ComponentModel;
using BudgetPlanner.Server.Services.Transactions;
using BudgetPlanner.Shared.Models.Request.Transaction;
using BudgetPlanner.Shared.Models.Response.Transaction;
using Microsoft.AspNetCore.OpenApi;

namespace BudgetPlanner.Server.EndPoints;

public static class TransactionEndPointExtensions
{
    /// <summary>
    /// Maps the transactions endpoints to the application
    /// </summary>
    /// <param name="app">The web application instance</param>
    public static void MapTransactionsEndPoint(this WebApplication app)
    {
        var transactionsGroup = app.MapGroup("/Transactions")
            .WithTags("Transactions")
            .WithDescription("API entry point for Transaction based operations")
            .WithSummary("Auth Based Operations")
            .RequireAuthorization();

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
        })
            .WithSummary("Get Transaction Category Filters")
            .WithDescription("Retrieves a stream of transaction category filters")
            .Produces<IAsyncEnumerable<TransactionCategoryFilterResponse>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        
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
        })
            .WithSummary("Get Transaction Type Filters")
            .WithDescription("Retrieves a stream of transaction type filters")
            .Produces<IAsyncEnumerable<TransactionTypeFilterResponse>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        
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
        })
            .WithSummary("Get Transaction Provider Filters")
            .WithDescription("Retrieves a stream of transaction provider filters")
            .Produces<IAsyncEnumerable<TransactionProviderFilterResponse>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        
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
        })
            .WithSummary("Get Transaction Account Filters")
            .WithDescription("Retrieves a stream of transaction account filters")
            .Produces<IAsyncEnumerable<TransactionAccountFilterResponse>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        
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
        })
            .WithSummary("Get Transaction Tag Filters")
            .WithDescription("Retrieves a stream of transaction tag filters")
            .Produces<IAsyncEnumerable<TransactionTagFilterResponse>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        transactionsGroup.MapPost("/GetTransactions", async ([Description("The request containing the filtering criteria for transactions")] FilteredTransactionsRequest request, ITransactionsService transactionsService) =>
        {
            async IAsyncEnumerable<TransactionResponse> TransactionAccountStream()
            {
                await foreach (var transaction in transactionsService.GetAllTransactionsAsync(request, request.SyncType))
                {
                    yield return transaction;
                }
            }
            return TransactionAccountStream();
        })
            .WithSummary("Get Filtered Transactions")
            .WithDescription("Retrieves a stream of transactions filtered by the specified criteria")
            .Accepts<FilteredTransactionsRequest>("application/json")
            .Produces<IAsyncEnumerable<TransactionResponse>>(200, "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status400BadRequest);
    }
}