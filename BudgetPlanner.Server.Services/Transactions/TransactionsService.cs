using System.Security.Claims;
using BudgetPlanner.Server.Services.OpenBanking;
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.Transaction;
using BudgetPlanner.Shared.Models.Response.Transaction;
using BudgetPlanner.Shared.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Server.Services.Transactions
{
    public class TransactionsService : BaseService, ITransactionsService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
        private readonly IOpenBankingService _openBankingService;

        public TransactionsService(BudgetPlannerDbContext budgetPlannerDbContext, IOpenBankingService openBankingService, ClaimsPrincipal user) : base(user, budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
            _openBankingService = openBankingService;
        }

        public async IAsyncEnumerable<TransactionAccountFilterResponse> GetAccountsForTransactionFiltersAsync(SyncTypes syncTypes = SyncTypes.Account)
        {
            await _openBankingService.PerformSyncAsync(syncTypes);
            var query = _budgetPlannerDbContext.IsolateToUser(UserId).Include(x => x.Providers).ThenInclude(x => x.Accounts)
                .SelectMany(x => x.Providers.SelectMany(c => c.Accounts))
                .AsNoTracking()
                .Select(x => new TransactionAccountFilterResponse() { AccountId = x.Id, AccountName = x.DisplayName });

            var accounts = query.AsAsyncEnumerable();

            await foreach (var account in accounts)
            {
                yield return account;
            }
        }

        public async IAsyncEnumerable<TransactionResponse> GetAllTransactionsAsync(FilteredTransactionsRequest filteredTransactionsRequest, SyncTypes syncTypes = SyncTypes.All)
        {
            await _openBankingService.PerformSyncAsync(syncTypes);
            await foreach (var transaction in GetTransactionResponsesAsync(filteredTransactionsRequest, syncTypes))
            {
                yield return transaction;
            }
        }

        public async IAsyncEnumerable<TransactionCategoryFilterResponse> GetCategoriesForTransactionFiltersAsync()
        {
            var query = _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.Providers).ThenInclude(x => x.Accounts).ThenInclude(x => x.Transactions)
                .SelectMany(x => x.Providers.SelectMany(c => c.Accounts).SelectMany(r => r.Transactions))
                .Select(x => x.TransactionCategory).Distinct();
            
            await foreach (var category in query.AsAsyncEnumerable())
            {
                yield return new TransactionCategoryFilterResponse() { TransactionCategory = category };
            }
        }

        public async IAsyncEnumerable<TransactionProviderFilterResponse> GetProvidersForTransactionFiltersAsync()
        {
            var query = _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.Providers).ThenInclude(x => x.Accounts)
                .SelectMany(x => x.Providers)
                .Select(x => new TransactionProviderFilterResponse() { ProviderId = x.Id, ProviderName = x.Name }).Distinct();

            await foreach (var provider in query.AsAsyncEnumerable())
            {
                yield return provider;
            }
        }

        public async IAsyncEnumerable<TransactionTypeFilterResponse> GetTypesForTransactionFiltersAsync()
        {
            var query = _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.Providers).ThenInclude(x => x.Accounts).ThenInclude(x => x.Transactions)
                .SelectMany(x => x.Providers.SelectMany(x => x.Accounts).SelectMany(c => c.Transactions))
                .Select(x => new TransactionTypeFilterResponse() { TransactionType = x.TransactionType })
                .Distinct();

            await foreach (var type in query.AsAsyncEnumerable())
            {
                yield return type;
            }
        }

        public async IAsyncEnumerable<TransactionTagFilterResponse> GetTagsForTransactionFiltersAsync()
        {
            var query = _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.Providers).ThenInclude(x => x.Accounts).ThenInclude(x => x.Transactions). ThenInclude(x => x.Classifications)
                .SelectMany(x => x.Providers.SelectMany(c => c.Accounts).SelectMany(r => r.Transactions).SelectMany(t => t.Classifications))
                .Select(x => new TransactionTagFilterResponse() { Tag = x.Classification })
                .Distinct();

            await foreach (var type in query.AsAsyncEnumerable())
            {
                yield return type;
            }
        }

        private async IAsyncEnumerable<TransactionResponse> GetTransactionResponsesAsync(FilteredTransactionsRequest filteredTransactionsRequest, SyncTypes syncTypes)
        {
            var transactionsQuery = _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.Providers).ThenInclude(x => x.Accounts).ThenInclude(x => x.Transactions).ThenInclude(x => x.Classifications)
                .SelectMany(x => x.Providers.SelectMany(c => c.Accounts).SelectMany(r => r.Transactions))
                .AsQueryable();
            
            transactionsQuery = ApplyTransactionRequestFiltering(filteredTransactionsRequest, transactionsQuery);


            await foreach (var entity in GetTransactionsSelect(transactionsQuery).OrderByDescending(x => x.TransactionTime).GetPagedEntitiesAsync(10))
            {
                yield return entity;
            }

        }

        private IQueryable<OpenBankingTransaction> ApplyTransactionRequestFiltering(FilteredTransactionsRequest filteredTransactionsRequest, IQueryable<OpenBankingTransaction> transactionsQuery)
        {
            if (filteredTransactionsRequest.AccountIds is not null && filteredTransactionsRequest.AccountIds.Any())
            {
                transactionsQuery = transactionsQuery.Where(x => filteredTransactionsRequest.AccountIds.Contains(x.Account.Id));
            }

            if (filteredTransactionsRequest.Types is not null && filteredTransactionsRequest.Types.Any())
            {
                transactionsQuery = transactionsQuery.Where(x => filteredTransactionsRequest.Types.Contains(x.TransactionType));
            }

            if (filteredTransactionsRequest.Categories is not null && filteredTransactionsRequest.Categories.Any())
            {
                transactionsQuery = transactionsQuery.Where(x => filteredTransactionsRequest.Categories.Contains(x.TransactionCategory));
            }

            if (filteredTransactionsRequest.Tags is not null && filteredTransactionsRequest.Tags.Any())
            {
                transactionsQuery = transactionsQuery.Where(x => filteredTransactionsRequest.Tags.Any(y => x.Classifications.Any(c => c.Classification == y)));
            }

            if (filteredTransactionsRequest.ProviderIds is not null && filteredTransactionsRequest.ProviderIds.Any())
            {
                
                    transactionsQuery = transactionsQuery.Where(x => filteredTransactionsRequest.ProviderIds.Contains(x.Provider.Id))
                    .Select(x => x);
            }

            if (filteredTransactionsRequest.SearchTerm is not null)
            {
                transactionsQuery = transactionsQuery.Where(x => EF.Functions.Like(x.Description.ToLower(), $"%{filteredTransactionsRequest.SearchTerm.ToLower()}%"));

            }

            if (filteredTransactionsRequest.FromDate is not null)
            {
                transactionsQuery = transactionsQuery.Where(x => x.TransactionTime >= filteredTransactionsRequest.FromDate);
            }

            if (filteredTransactionsRequest.ToDate is not null)
            {
                transactionsQuery = transactionsQuery.Where(x => x.TransactionTime <= filteredTransactionsRequest.ToDate);
            }



            return transactionsQuery;
        }

        private IQueryable<TransactionResponse> GetTransactionsSelect(IQueryable<OpenBankingTransaction> query)
        {
            
            return query.Select(transaction => new TransactionResponse()
            {
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                Description = transaction.Description,
                Pending = transaction.Pending,
                TransactionCategory = transaction.TransactionCategory,
                TransactionId = transaction.Id,
                TransactionTime = transaction.TransactionTime,
                TransactionType = transaction.TransactionType,
                Tags = transaction.Classifications.Select(x => x.Classification)
            });
        }
    }
}
