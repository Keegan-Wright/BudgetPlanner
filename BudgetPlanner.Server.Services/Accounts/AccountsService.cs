using System.Security.Claims;
using BudgetPlanner.Server.Services.OpenBanking;
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Response.Account;
using BudgetPlanner.Shared.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Server.Services.Accounts
{
    public class AccountsService : BaseService, IAccountsService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
        private readonly IOpenBankingService _openBankingService;

        public AccountsService(BudgetPlannerDbContext budgetPlannerDbContext, IOpenBankingService openBankingService, ClaimsPrincipal user) : base(user, budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
            _openBankingService = openBankingService;
        }

        public async IAsyncEnumerable<AccountAndTransactionsResponse> GetAccountsAndMostRecentTransactionsAsync(int transactionsToReturn, SyncTypes syncFlags = SyncTypes.All)
        {

            var transaction = GetSentryTransaction(nameof(GetAccountsAndMostRecentTransactionsAsync), "Loading");


            var syncSpan = GetTransactionChild(transaction, "Open Banking Sync", $"Syncronise open banking data for all providers with the following scopes {syncFlags}");
           
            await _openBankingService.PerformSyncAsync(syncFlags);

            FinishTransactionChildTrace(syncSpan);

            var a = _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.Providers).ThenInclude(a => a.Accounts).ThenInclude(x => x.AccountBalance)
                .Include(x => x.Providers).ThenInclude(x => x.Accounts).ThenInclude(x => x.Transactions)
                .SelectMany(x => x.Providers.SelectMany(c => c.Accounts))
                .AsNoTracking()
                .ToQueryString();
            
            var dataProcessingSpan = GetTransactionChild(transaction, "Open Banking Build For Client", "Loading internal data for the client");
            
            await foreach (var account in _budgetPlannerDbContext.IsolateToUser(UserId)
                               .Include(x => x.Providers).ThenInclude(x => x.Accounts).ThenInclude(x => x.Provider)
                               .Include(x => x.Providers).ThenInclude(a => a.Accounts).ThenInclude(x => x.AccountBalance)
                               .Include(x => x.Providers).ThenInclude(x => x.Accounts).ThenInclude(x => x.Transactions)
                               .SelectMany(x => x.Providers.SelectMany(c => c.Accounts))
                               .AsNoTracking()
                               .AsAsyncEnumerable())
            {
                var response = new AccountAndTransactionsResponse()
                {
                    AccountBalance = account.AccountBalance?.Current ?? 0,
                    AccountName = account.DisplayName,
                    AccountType = account.AccountType,
                    AvailableBalance = account.AccountBalance.Available,
                    Logo = account.Provider.Logo,
                    Transactions = account.Transactions?.OrderByDescending(x => x.TransactionTime).Take(transactionsToReturn).Select(transaction => new AccountTransactionResponse()
                    {
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        Status = transaction.Pending ? "Pending" : "Complete",
                        Time = transaction.TransactionTime
                    }).ToAsyncEnumerable()
                };

                yield return response;

            }

            FinishTransactionChildTrace(dataProcessingSpan);

            FinishTransaction(transaction);
        }
    }

}
