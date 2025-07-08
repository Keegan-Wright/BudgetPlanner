using System.Globalization;
using System.Security.Claims;
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Server.Services.OpenBanking;
using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Response;
using BudgetPlanner.Shared.Models.Response.Reports;
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

    public async IAsyncEnumerable<SpentInTimePeriodReportResponse> GetSpentInTimePeriodReportAsync(BaseReportRequest request)
    {
        await _openBankingService.PerformSyncAsync(request.SyncTypes);

        var query = GetQueryByBaseReportRequest(request);

        var openBankingTransactions = new List<OpenBankingTransaction>();
        
        await foreach (var transaction in query.OrderByDescending(x => x.TransactionTime)
                           .ToAsyncEnumerable())
        {
            openBankingTransactions.Add(transaction);
        }
        
        var totalIn = openBankingTransactions.Where(x => !Decimal.IsNegative(x.Amount)).Sum(x => x.Amount);
        var totalOut = openBankingTransactions.Where(x => Decimal.IsNegative(x.Amount)).Sum(x => x.Amount);
        var totalTransactions = openBankingTransactions.Count;
        


        foreach (var yearlyGrouping in openBankingTransactions.GroupBy(x => x.TransactionTime.Year))
        {
            
            var rsp = new SpentInTimePeriodReportResponse
            {
                TotalIn = totalIn,
                TotalOut = totalOut,
                TotalTransactions = totalTransactions
            };
            
            var yearGrp = new SpentInTimePeriodReportYearlyBreakdownResponse()
            {
                Year = yearlyGrouping.Key,
                TotalIn = yearlyGrouping.Where(x => !Decimal.IsNegative(x.Amount)).Sum(x => x.Amount),
                TotalOut = yearlyGrouping.Where(x => Decimal.IsNegative(x.Amount)).Sum(x => x.Amount),
                TotalTransactions = yearlyGrouping.Count(),
            };

            foreach (var monthlyGrouping in yearlyGrouping.GroupBy(x => x.TransactionTime.Month))
            {
                var monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(monthlyGrouping.Key);
                var monthGrp = new SpentInTimePeriodReportMonthlyBreakdownResponse()
                {
                    Month = monthName,
                    TotalIn = monthlyGrouping.Where(x => !Decimal.IsNegative(x.Amount)).Sum(x => x.Amount),
                    TotalOut = monthlyGrouping.Where(x => Decimal.IsNegative(x.Amount)).Sum(x => x.Amount),
                    TotalTransactions = monthlyGrouping.Count(),
                };
                
                
                var dayGrp = monthlyGrouping.GroupBy(x => x.TransactionTime.Day);
                foreach (var dayGrouping in dayGrp)
                {
                    monthGrp.DailyBreakdown.Add(new SpentInTimePeriodReportDailyBreakdownResponse()
                    {
                        Day = dayGrouping.Key,
                        TotalIn = dayGrouping.Where(x => !Decimal.IsNegative(x.Amount)).Sum(x => x.Amount),
                        TotalOut = dayGrouping.Where(x => Decimal.IsNegative(x.Amount)).Sum(x => x.Amount),
                        TotalTransactions = dayGrouping.Count()
                    });
                }
                
                yearGrp.MonthlyBreakdown.Add(monthGrp);
            }
            
            rsp.YearlyBreakdown.Add(yearGrp);
            yield return rsp;
        }
        
    }


    public async IAsyncEnumerable<CategoryBreakdownReportResponse> GetCategoryBreakdownReportAsync(BaseReportRequest request)
    { 
        var query = GetQueryByBaseReportRequest(request);

        var openBankingTransactions = new List<OpenBankingTransaction>();
        
        await foreach (var transaction in query.OrderByDescending(x => x.TransactionTime)
                           .ToAsyncEnumerable())
        {
            openBankingTransactions.Add(transaction);
        }

        var categoryGroups = openBankingTransactions.GroupBy(x => x.TransactionCategory);

        foreach (var group in categoryGroups)
        {
            var grpTotal = group.Sum(x => x.Amount);
            var grpTotalIn = group.Where(x => x.Amount > 0).Sum(x => x.Amount);
            var grpTotalOut = group.Where(x => x.Amount < 0).Sum(x => x.Amount);
            var grpCount = group.Count();

            var rsp = new CategoryBreakdownReportResponse()
            {
                Total = grpTotal,
                TotalIn = grpTotalIn,
                TotalOut = grpTotalOut,
                TotalTransactions = grpCount,
                CategoryName = group.Key
            };
            
            var subBreakdowns = new List<CategoryBreakdownReportItemResponse>();

            foreach (var yearGrp in group.GroupBy(x =>  x.TransactionTime.Year))
            {
                foreach (var monthGrp in yearGrp.GroupBy(x => x.TransactionTime.Month))
                {
                    subBreakdowns.Add(new CategoryBreakdownReportItemResponse()
                    {
                        CategoryName = group.Key,
                        Total = monthGrp.Sum(x => x.Amount),
                        TotalIn = monthGrp.Where(x => x.Amount > 0).Sum(x => x.Amount),
                        TotalOut = monthGrp.Where(x => x.Amount < 0).Sum(x => x.Amount),
                        TotalTransactions = monthGrp.Count(),
                        Date = new DateTime(yearGrp.Key, monthGrp.Key, 1)
                    });
                }
            }
            
            rsp.SubItems = subBreakdowns.ToAsyncEnumerable();

            yield return rsp;

        }
    }

    public async IAsyncEnumerable<AccountBreakdownReportResponse> GetAccountBreakdownReportAsync(BaseReportRequest request)
    {
        var query = GetQueryByBaseReportRequest(request);

        var openBankingTransactions = new List<OpenBankingTransaction>();

        await foreach (var transaction in query.OrderByDescending(x => x.TransactionTime)
                           .ToAsyncEnumerable())
        {
            openBankingTransactions.Add(transaction);
        }

        var accountGroups = openBankingTransactions.GroupBy(x => x.Account.DisplayName);
        ;

        foreach (var group in accountGroups)
        {
            var grpTotal = group.Sum(x => x.Amount);
            var grpTotalIn = group.Where(x => x.Amount > 0).Sum(x => x.Amount);
            var grpTotalOut = group.Where(x => x.Amount < 0).Sum(x => x.Amount);
            var grpCount = group.Count();

            var rsp = new AccountBreakdownReportResponse()
            {
                Total = grpTotal,
                TotalIn = grpTotalIn,
                TotalOut = grpTotalOut,
                TotalTransactions = grpCount,
                AccountName = group.Key
            };

            var subBreakdowns = new List<AccountBreakdownReportItemResponse>();

            foreach (var yearGrp in group.GroupBy(x => x.TransactionTime.Year))
            {
                foreach (var monthGrp in yearGrp.GroupBy(x => x.TransactionTime.Month))
                {
                    subBreakdowns.Add(new AccountBreakdownReportItemResponse()
                    {
                        AccountName = group.Key,
                        Total = monthGrp.Sum(x => x.Amount),
                        TotalIn = monthGrp.Where(x => x.Amount > 0).Sum(x => x.Amount),
                        TotalOut = monthGrp.Where(x => x.Amount < 0).Sum(x => x.Amount),
                        TotalTransactions = monthGrp.Count(),
                        Date = new DateTime(yearGrp.Key, monthGrp.Key, 1)
                    });
                }
            }

            rsp.SubItems = subBreakdowns.ToAsyncEnumerable();

            yield return rsp;

        }
    }


    private IQueryable<OpenBankingTransaction> GetQueryByBaseReportRequest(BaseReportRequest request)
    {
        var query = _budgetPlannerDbContext.IsolateToUser(UserId)
            
            .Include(x => x.Providers)
            .ThenInclude(x => x.Accounts)
            .ThenInclude(x => x.Transactions)
            .SelectMany(x => x.Providers.SelectMany(c => c.Transactions))
            .AsNoTracking();


            if (request.AccountIds is not null && request.AccountIds.Any())
            {
                query = query.Where(x => request.AccountIds.Contains(x.Account.Id));
            }

            if (request.Types is not null && request.Types.Any())
            {
                query = query.Where(x => request.Types.Contains(x.TransactionType));
            }

            if (request.Categories is not null && request.Categories.Any())
            {
                query = query.Where(x => request.Categories.Contains(x.TransactionCategory));
            }

            if (request.Tags is not null && request.Tags.Any())
            {
                query = query.Where(x => request.Tags.Any(y => x.Classifications.Any(c => c.Classification == y)));
            }

            if (request.ProviderIds is not null && request.ProviderIds.Any())
            {
                
                query = query.Where(x => request.ProviderIds.Contains(x.Provider.Id))
                    .Select(x => x);
            }

            if (request.SearchTerm is not null)
            {
                query = query.Where(x => EF.Functions.Like(x.Description.ToLower(), $"%{request.SearchTerm.ToLower()}%"));

            }

            if (request.FromDate is not null)
            {
                query = query.Where(x => x.TransactionTime >= request.FromDate);
            }

            if (request.ToDate is not null)
            {
                query = query.Where(x => x.TransactionTime <= request.ToDate);
            }
            
            query = query.Where(x => x.TransactionCategory != "TRANSFER");
            
        
        return query;
    }
}