using BudgetPlanner.Data.Db;
using BudgetPlanner.Data.Models;
using BudgetPlanner.Enums;
using BudgetPlanner.Models.Request.Transaction;
using BudgetPlanner.Models.Response.Transaction;
using BudgetPlanner.Services.Base;
using BudgetPlanner.Services.OpenBanking;

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

        public async IAsyncEnumerable<TransactionResponse> GetAllTransactionsAsync(SyncTypes syncTypes = SyncTypes.All)
        {
            await foreach (var transaction in GetTransactionResponsesAsync(null, syncTypes))
            {
                var transactionResponse = new TransactionResponse();
                yield return transactionResponse;
            }
        }

        public async IAsyncEnumerable<TransactionResponse> GetAllTransactionsAsync(FilteredTransactionsRequest filteredTransactionsRequest, SyncTypes syncTypes = SyncTypes.All)
        {
            await foreach (var transaction in GetTransactionResponsesAsync(filteredTransactionsRequest, syncTypes))
            {
                var transactionResponse = new TransactionResponse();
                yield return transactionResponse;
            }
        }

        private async IAsyncEnumerable<OpenBankingTransaction> GetTransactionResponsesAsync(FilteredTransactionsRequest? filteredTransactionsRequest, SyncTypes syncTypes)
        {
            if(filteredTransactionsRequest == null)
            {
                await foreach(var entity in _budgetPlannerDbContext.OpenBankingTransactions.OrderByDescending(x => x.TransactionTime).GetPagedEntitiesAsync(10))
                {
                    yield return entity;
                }
            }
            else
            {
                var query = _budgetPlannerDbContext.OpenBankingTransactions.AsQueryable();

                if(filteredTransactionsRequest.AccountId is not null)
                {
                    query.Where(x => x.OpenBankingAccountId == filteredTransactionsRequest.AccountId);
                }


                await foreach (var entity in query.GetPagedEntitiesAsync(10))
                {
                    yield return entity;
                }
            }
        }
    }
}
