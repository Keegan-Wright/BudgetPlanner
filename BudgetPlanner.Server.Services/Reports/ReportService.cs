using System.Security.Claims;
using System.Security.Cryptography.Xml;
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Server.Services.OpenBanking;
using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.Reports;
using BudgetPlanner.Shared.Models.Response.Transaction;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Server.Services.Reports;

public class ReportService : BaseService, IReportService
{
    private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
    private readonly IOpenBankingService _openBankingService;

    
    public ReportService(ClaimsPrincipal user, BudgetPlannerDbContext budgetPlannerDbContext, IOpenBankingService openBankingService) : base(user, budgetPlannerDbContext)
    {
        _budgetPlannerDbContext = budgetPlannerDbContext;
        _openBankingService = openBankingService;
    }

    public async Task<SpentInTimePeriodReportResponse> GetSpentInTimePeriodReportAsync(BaseReportRequest request)
    {
        await _openBankingService.PerformSyncAsync(request.SyncType);

        var query = GetQueryByBaseReportRequest(request);

        var openBankingTransactions = new List<OpenBankingTransaction>();
        
        await foreach (var transaction in query.OrderByDescending(x => x.TransactionTime)
                           .ToAsyncEnumerable())
        {
            openBankingTransactions.Add(transaction);
        }
        
        var totalSpent = openBankingTransactions.Sum(x => x.Amount);
        var totalTransactions = openBankingTransactions.Count;
        
        var rsp = new SpentInTimePeriodReportResponse
        {
            TotalSpent = totalSpent,
            TotalTransactions = totalTransactions
        };

        foreach (var yearlyGrouping in openBankingTransactions.GroupBy(x => x.TransactionTime.Year))
        {
            var yearGrp = new SpentInTimePeriodReportYearlyBreakdownResponse()
            {
                Year = yearlyGrouping.Key,
                TotalSpent = yearlyGrouping.Sum(x => x.Amount),
                TotalTransactions = yearlyGrouping.Count(),
            };

            foreach (var monthlyGrouping in yearlyGrouping.GroupBy(x => x.TransactionTime.Month))
            {
                var monthGrp = new SpentInTimePeriodReportMonthlyBreakdownResponse()
                {
                    Month = monthlyGrouping.Key,
                    TotalSpent = monthlyGrouping.Sum(x => x.Amount),
                    TotalTransactions = yearlyGrouping.Count(),
                };
                yearGrp.MonthlyBreakdown.Add(monthGrp);
            }
            
            rsp.YearlyBreakdown.Add(yearGrp);
        }
        
        return rsp;
    }


    public Task<CategoryBreakdownReportResponse> GetCategoryBreakdownReportAsync(BaseReportRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<SpentInTimePeriodResponse> AccountBreakdowmReportAsync(BaseReportRequest request)
    {
        throw new NotImplementedException();
    }
    
    
    private IQueryable<OpenBankingTransaction> GetQueryByBaseReportRequest(BaseReportRequest request)
    {
        var query = _budgetPlannerDbContext.IsolateToUser(UserId)
            
            .Include(x => x.Providers)
            .ThenInclude(x => x.Accounts)
            .ThenInclude(x => x.Transactions)
            .SelectMany(x => x.Providers.SelectMany(c => c.Transactions))
            .AsNoTracking();


        if (request.ProviderIds != null)
        {
            query = query.Where(x => request.ProviderIds.Contains(x.Provider.Id));
        }

        if (request.AccountIds != null)
        {
            query = query.Where(x => request.AccountIds.Contains(x.Account.Id));
        }

        if (request.Categories != null)
        {
            query = query.Where(x => request.Categories.Contains(x.TransactionCategory));
        }
        
        if (request.TransactionTypes != null)
        {
            query = query.Where(x => request.TransactionTypes.Contains(x.TransactionType));
        }
        
        query = query.Where(x => x.TransactionTime >= request.FromDate && x.TransactionTime <= request.ToDate);
        
        return query;
    }
}