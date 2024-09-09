using BudgetPlanner.Data.Db;
using BudgetPlanner.Models.Response;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BudgetPlanner.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;

        public DashboardService(BudgetPlannerDbContext budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
        }

        public async Task<decimal> GetSpentInTimePeriod(DateTime from, DateTime to)
        {
            return await GetTotalSpendInTimePeriod(from, to);
        }

        public async Task<decimal> GetSpentInTimePeriod(DateTime date)
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
                .Where(x => x.PreviousPaymentTimeStamp < DateTime.Now)
                .Select(x => new UpcomingPaymentsResponse()
            {
                Amount = x.PreviousPaymentAmount,
                PaymentDate = x.PreviousPaymentTimeStamp.AddMonths(1),
                PaymentName = x.Name,
                PaymentType = "Direct Debit"
            }).ToListAsync());


            await foreach(var upcomingPayment in upcomingPayments
                .OrderBy(x => x.PaymentDate)
                .Take(numberToFetch)                                    
                .ToAsyncEnumerable())
            {
                yield return upcomingPayment;
            }
        }

        private async Task<decimal> GetTotalSpendInTimePeriod(DateTime from, DateTime to)
        {
            var query = _budgetPlannerDbContext.OpenBankingTransactions.Where(x => x.TransactionTime >= from &&  x.TransactionTime <= to);
            var items = await query.Select(x => x.Amount).ToListAsync();

            return items.Sum();
        }
    }
}
