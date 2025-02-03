using System.Security.Claims;
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.Services.OpenBanking;
using BudgetPlanner.Shared.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Server.Services.Dashboard
{
    public class DashboardService : BaseService, IDashboardService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
        private readonly IOpenBankingService _openBankingService;


        public DashboardService(BudgetPlannerDbContext budgetPlannerDbContext, IOpenBankingService openBankingService, ClaimsPrincipal user) : base(user, budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
            _openBankingService = openBankingService;
        }

        public async Task<SpentInTimePeriodResponse> GetSpentInTimePeriod(DateTime from, DateTime to)
        {
            return await GetTotalSpendInTimePeriod(from, to);
        }
        
        public async IAsyncEnumerable<UpcomingPaymentsResponse> GetUpcomingPaymentsAsync(int numberToFetch)
        {
            var upcomingPayments = new List<UpcomingPaymentsResponse>();

            upcomingPayments.AddRange(await _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x=> x.Providers).ThenInclude(x => x.Accounts).ThenInclude(x => x.StandingOrders)
                .SelectMany(x => x.Providers.SelectMany(c => c.Accounts).SelectMany(r => r.StandingOrders))
                .AsNoTracking()
                .Where(x => x.NextPaymentDate > DateTime.Now.ToUniversalTime())
                .Select(x => new UpcomingPaymentsResponse()
                {
                    Amount = x.NextPaymentAmount,
                    PaymentDate = x.NextPaymentDate,
                    PaymentName = x.Payee,
                    PaymentType = "Standing Order"
                }).ToListAsync());


            upcomingPayments.AddRange(await _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.Providers).ThenInclude(x => x.Accounts).ThenInclude(x => x.DirectDebits)
                .SelectMany(x => x.Providers.SelectMany(c => c.Accounts).SelectMany(r => r.DirectDebits))
                .AsNoTracking()
                .Where(x => x.PreviousPaymentAmount != 0)
                .Where(x => x.PreviousPaymentTimeStamp < DateTime.Now.ToUniversalTime())
                .Select(x => new UpcomingPaymentsResponse()
                {
                    Amount = x.PreviousPaymentAmount,
                    PaymentDate = x.PreviousPaymentTimeStamp.AddMonths(1),
                    PaymentName = x.Name,
                    PaymentType = "Direct Debit"
                }).ToListAsync());


            await foreach (var upcomingPayment in upcomingPayments
                .Where(x => x.PaymentDate > DateTime.Now.ToUniversalTime())
                .OrderBy(x => x.PaymentDate)
                .Take(numberToFetch)
                .ToAsyncEnumerable())
            {
                yield return upcomingPayment;
            }
        }

        private async Task<SpentInTimePeriodResponse> GetTotalSpendInTimePeriod(DateTime from, DateTime to)
        {
            var query = _budgetPlannerDbContext.IsolateToUser(UserId)
                .Include(x => x.Providers).ThenInclude(x => x.Accounts).ThenInclude(x => x.Transactions)
                .SelectMany(x => x.Providers.SelectMany(c => c.Accounts).SelectMany(r => r.Transactions))
                .AsNoTracking().Where(x => (x.TransactionTime >= from.AddDays(-1).ToUniversalTime() && x.TransactionTime <= to.AddDays(1).ToUniversalTime()) && x.TransactionCategory != "TRANSFER");
            var items = await query.Select(x => x.Amount).ToListAsync();

            return new SpentInTimePeriodResponse()
            {
                TotalIn = items.Where(x => !decimal.IsNegative(x)).Sum(),
                TotalOut = items.Where(x => decimal.IsNegative(x)).Sum()
            };
        }
    }
}
