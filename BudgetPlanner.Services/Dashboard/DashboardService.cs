using BudgetPlanner.Data.Db;
using BudgetPlanner.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;

        public DashboardService(BudgetPlannerDbContext budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
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
    }
}
