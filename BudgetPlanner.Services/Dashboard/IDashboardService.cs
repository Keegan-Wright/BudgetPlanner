using BudgetPlanner.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<decimal> GetSpentInTimePeriod(DateTime from, DateTime to);
        Task<decimal> GetSpentInTimePeriod(DateTime date);
        IAsyncEnumerable<UpcomingPaymentsResponse> GetUpcomingPaymentsAsync(int numberToFetch);
    }
}
