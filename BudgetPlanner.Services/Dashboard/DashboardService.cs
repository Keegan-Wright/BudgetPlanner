using BudgetPlanner.Data.Db;
using BudgetPlanner.Enums;
using BudgetPlanner.Models.Response;
using BudgetPlanner.Services.OpenBanking;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
        private readonly IOpenBankingService _openBankingService;


        public DashboardService(BudgetPlannerDbContext budgetPlannerDbContext, IOpenBankingService openBankingService)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
            _openBankingService = openBankingService;
        }

        public async Task<SpentInTimePeriodResponse> GetSpentInTimePeriod(DateTime from, DateTime to)
        {
            return await GetTotalSpendInTimePeriod(from, to);
        }

        public async Task<SpentInTimePeriodResponse> GetSpentInTimePeriod(DateTime date)
        {
            return await GetTotalSpendInTimePeriod(date, date);
        }



        public async IAsyncEnumerable<UpcomingPaymentsResponse> GetUpcomingPaymentsAsync(int numberToFetch)
        {
            var upcomingPayments = new List<UpcomingPaymentsResponse>();

            upcomingPayments.AddRange(await _budgetPlannerDbContext.OpenBankingStandingOrders
                .Where(x => x.NextPaymentDate > DateTime.Now)
                .Select(x => new UpcomingPaymentsResponse()
                {
                    Amount = x.NextPaymentAmount,
                    PaymentDate = x.NextPaymentDate,
                    PaymentName = x.Payee,
                    PaymentType = "Standing Order"
                }).ToListAsync());


            upcomingPayments.AddRange(await _budgetPlannerDbContext.OpenBankingDirectDebits
                .Where(x => x.PreviousPaymentAmount != 0)
                .Where(x => x.PreviousPaymentTimeStamp < DateTime.Now)
                .Select(x => new UpcomingPaymentsResponse()
                {
                    Amount = x.PreviousPaymentAmount,
                    PaymentDate = x.PreviousPaymentTimeStamp.AddMonths(1),
                    PaymentName = x.Name,
                    PaymentType = "Direct Debit"
                }).ToListAsync());


            await foreach (var upcomingPayment in upcomingPayments
                .OrderBy(x => x.PaymentDate)
                .Take(numberToFetch)
                .ToAsyncEnumerable())
            {
                yield return upcomingPayment;
            }
        }

        private async Task<SpentInTimePeriodResponse> GetTotalSpendInTimePeriod(DateTime from, DateTime to)
        {
            var query = _budgetPlannerDbContext.OpenBankingTransactions.Where(x => x.TransactionTime >= from && x.TransactionTime <= to);
            var items = await query.Select(x => x.Amount).ToListAsync();

            return new SpentInTimePeriodResponse()
            {
                TotalIn = items.Where(x => !decimal.IsNegative(x)).Sum(),
                TotalOut = items.Where(x => decimal.IsNegative(x)).Sum()
            };
        }
    }
}
