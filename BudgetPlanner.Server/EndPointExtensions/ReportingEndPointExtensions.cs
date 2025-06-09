using BudgetPlanner.Server.Services.Reports;
using BudgetPlanner.Server.Services.Transactions;
using BudgetPlanner.Shared.Models.Response.Transaction;

namespace BudgetPlanner.Server.EndPoints;

public static class ReportingEndPointExtensions
{
    public static void MapReportingEndpoint(this WebApplication app)
    {
        var transactionsGroup = app.MapGroup("/Reports").RequireAuthorization();;

        transactionsGroup.MapGet("/GetSpentInTimePeriod", async (IReportService reportService) =>
        {
            async IAsyncEnumerable<TransactionCategoryFilterResponse> GetSpentInTimePeriod()
            {
                await foreach (var categoryFilter in reportService.GetSpentInTimePeriod())
                {
                    yield return categoryFilter;
                }
            }
            return TransactionCategoriesStream();
        });
        

    }
}