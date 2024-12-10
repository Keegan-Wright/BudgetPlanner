using BudgetPlanner.Shared.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Server.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<SpentInTimePeriodResponse> GetSpentInTimePeriod(DateTime from, DateTime to);
        Task<SpentInTimePeriodResponse> GetSpentInTimePeriod(DateTime date);
        IAsyncEnumerable<UpcomingPaymentsResponse> GetUpcomingPaymentsAsync(int numberToFetch);
    }
}
