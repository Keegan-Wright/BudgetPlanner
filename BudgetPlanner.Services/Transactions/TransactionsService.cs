using BudgetPlanner.Data.Db;
using BudgetPlanner.Data.Models;
using BudgetPlanner.Enums;
using BudgetPlanner.Models.Request.Transaction;
using BudgetPlanner.Models.Response.Transaction;
using BudgetPlanner.Services.Base;
using BudgetPlanner.Services.OpenBanking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BudgetPlanner.Services.Transactions
{
    public class TransactionsService : InstrumentedService, ITransactionsService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
        private readonly IOpenBankingService _openBankingService;

        public TransactionsService(BudgetPlannerDbContext budgetPlannerDbContext, IOpenBankingService openBankingService)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
            _openBankingService = openBankingService;
        }

        public async IAsyncEnumerable<TransactionAccountFilterResponse> GetAccountsForTransactionFiltersAsync(SyncTypes syncTypes = SyncTypes.Account)
        {
            await _openBankingService.PerformSyncAsync(syncTypes);
            var query = _budgetPlannerDbContext.OpenBankingAccounts
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

            await foreach (var transaction in GetTransactionResponsesAsync(filteredTransactionsRequest, syncTypes))
            {
                yield return transaction;
            }
        }

        public async IAsyncEnumerable<TransactionCategoryFilterResponse> GetCategoriesForTransactionFiltersAsync()
        {
            var query = _budgetPlannerDbContext.OpenBankingTransactions.Select(x => x.TransactionCategory).Distinct();
            await foreach (var category in query.AsAsyncEnumerable())
            {
                yield return new TransactionCategoryFilterResponse() { TransactionCategory = category };
            }
        }

        public async IAsyncEnumerable<TransactionProviderFilterResponse> GetProvidersForTransactionFiltersAsync()
        {
            var query = _budgetPlannerDbContext.OpenBankingProviders
                .Select(x => new TransactionProviderFilterResponse() { ProviderId = x.Id, ProviderName = x.Name });

            await foreach (var provider in query.AsAsyncEnumerable())
            {
                yield return provider;
            }
        }

        public async IAsyncEnumerable<TransactionTypeFilterResponse> GetTypesForTransactionFiltersAsync()
        {
            var query = _budgetPlannerDbContext.OpenBankingTransactions
                .Select(x => new TransactionTypeFilterResponse() { TransactionType = x.TransactionType })
                .Distinct();

            await foreach (var type in query.AsAsyncEnumerable())
            {
                yield return type;
            }
        }


        private async IAsyncEnumerable<TransactionResponse> GetTransactionResponsesAsync(FilteredTransactionsRequest filteredTransactionsRequest, SyncTypes syncTypes)
        {
            var transactionsQuery = _budgetPlannerDbContext.OpenBankingTransactions.AsQueryable();

            if (filteredTransactionsRequest.AccountId is not null)
            {
                transactionsQuery = transactionsQuery.Where(x => x.Account.Id == filteredTransactionsRequest.AccountId);
            }

            if (filteredTransactionsRequest.Type is not null)
            {
                transactionsQuery = transactionsQuery.Where(x => x.TransactionType == filteredTransactionsRequest.Type);
            }

            if (filteredTransactionsRequest.Category is not null)
            {
                transactionsQuery =  transactionsQuery.Where(x => x.TransactionCategory == filteredTransactionsRequest.Category);
            }

            if (filteredTransactionsRequest.ProviderId is not null)
            {
                transactionsQuery =  transactionsQuery.Join(_budgetPlannerDbContext.OpenBankingAccounts,
                    transaction => transaction.Account.OpenBankingAccountId,
                    account => account.OpenBankingAccountId,
                    (transaction, account) => new
                    {
                        Transaction = transaction,
                        Account = account,
                    })
                    .Join(_budgetPlannerDbContext.OpenBankingProviders,
                     ta => ta.Account.ProviderId,
                    p => p.Id,
                    (ta, p) => new
                    {
                        ta.Transaction,
                        Provider = p
                    }
                    )
                    .Where(x => x.Provider.Id == filteredTransactionsRequest.ProviderId)
                    .Select(x => x.Transaction);
            }

            if (filteredTransactionsRequest.SearchTerm is not null)
            {
                transactionsQuery = transactionsQuery.Where(x => EF.Functions.Like(x.Description, $"%{filteredTransactionsRequest.SearchTerm}%"));

            }

            if (filteredTransactionsRequest.FromDate is not null)
            {
                transactionsQuery = transactionsQuery.Where(x => x.TransactionTime >= filteredTransactionsRequest.FromDate);
            }

            if (filteredTransactionsRequest.ToDate is not null)
            {
                transactionsQuery = transactionsQuery.Where(x => x.TransactionTime <= filteredTransactionsRequest.ToDate);
            }

            transactionsQuery =  transactionsQuery.OrderByDescending(x => x.TransactionTime);

            await foreach (var entity in GetTransactionsSelect(transactionsQuery).GetPagedEntitiesAsync(10))
            {
                yield return entity;
            }

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
            });
        }
    }
}
