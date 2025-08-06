using System.Collections.ObjectModel;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Reports;
using BudgetPlanner.Client.Services.Transactions;
using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Request.Transaction;
using BudgetPlanner.Shared.Models.Response.Reports;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace BudgetPlanner.Client.ViewModels;

public partial class SpentInTimePeriodReportViewModel : BaseReportPageViewModel<SpentInTimePeriodReportItemViewModel>
{
    public SpentInTimePeriodReportViewModel(IReportsService reportsService, INavigationService navigationService, ITransactionsRequestService transactionsRequestService) : base(reportsService, navigationService, transactionsRequestService)
    {
        
    }


    public override async Task LoadReportAsync(FilteredTransactionsRequest searchCriteria)
    {
        SetLoading(true, "Loading report...");
        ReportItems.Clear();

        var request = new BaseReportRequest
        {
            SyncTypes = SyncTypes.All,
            FromDate = searchCriteria.FromDate,
            ToDate = searchCriteria.ToDate,
            AccountIds = searchCriteria.AccountIds,
            Categories = searchCriteria.Categories,
            ProviderIds = searchCriteria.ProviderIds,
            Types = searchCriteria.Types,
            Tags = searchCriteria.Tags,
            SearchTerm = searchCriteria.SearchTerm
        };

        await foreach (var reportItem in _reportsService.GetSpentInTimePeriodReportAsync(request))
        {
            var newItem = new SpentInTimePeriodReportItemViewModel
            {
                TotalIn = reportItem.TotalIn,
                TotalOut = reportItem.TotalOut,
                TotalTransactions = reportItem.TotalTransactions
            };

            foreach (var yearlyItem in reportItem.YearlyBreakdown)
            {
                var newYearlyItem = new SpentInTimePeriodReportYearlyBreakdownViewModel
                {
                    TotalIn = yearlyItem.TotalIn,
                    TotalOut = yearlyItem.TotalOut,
                    TotalTransactions = yearlyItem.TotalTransactions,
                    Year = yearlyItem.Year
                };

                foreach (var monthlyItem in yearlyItem.MonthlyBreakdown)
                {
                    var newMonthlyItem = new SpentInTimePeriodReportMonthlyBreakdownViewModel
                    {
                        Month = monthlyItem.Month,
                        TotalIn = monthlyItem.TotalIn,
                        TotalOut = monthlyItem.TotalOut,
                        TotalTransactions = monthlyItem.TotalTransactions
                    };

                    foreach (var dailyItem in monthlyItem.DailyBreakdown)
                    {
                        var newDailyItem = new SpentInTimePeriodDailyBreakdownViewModel()
                        {
                            Day = dailyItem.Day,
                            TotalIn = dailyItem.TotalIn,
                            TotalOut = dailyItem.TotalOut,
                            TotalTransactions = dailyItem.TotalTransactions
                        };
                        
                        newMonthlyItem.DailyBreakdown.Add(newDailyItem);
                    }
                    
                    newYearlyItem.MonthlyBreakdown.Add(newMonthlyItem);
                }
                
                newItem.YearlyBreakdown.Add(newYearlyItem);
            }
            
            ReportItems.Add(newItem);;
        }
        
        UpdateTotals();
        UpdateOverviewPie();
        
        SetLoading(false);
    }

    public override async Task LoadFilterItemsAsync()
    {
        await base.LoadFilterItemsAsync();
    }

    private void UpdateOverviewPie()
    {
        TotalOverviewPieSeries.Clear();

        TotalOverviewPieSeries.Add(new PieSeries<decimal>
        {
            Values = new List<decimal> { TotalIn},
            Name = "Income",
            DataLabelsSize = 22,
            DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
            DataLabelsFormatter = point => "ddd",
            ToolTipLabelFormatter =  t => "Test ddd"
        });
        
        TotalOverviewPieSeries.Add(new PieSeries<decimal>
        {
            Name = "Outgoing",
            Values = new List<decimal> { decimal.Abs(TotalOut)},
            DataLabelsSize = 22,
            DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
            DataLabelsFormatter = point => 
                "Test",
            ToolTipLabelFormatter =  t => "Test ddd"
        });
        
        TotalOverviewPieSeries.Add(new PieSeries<decimal>
        {
            Name = "Dif",
            Values = new List<decimal> { decimal.Abs(TotalDif)},
            DataLabelsSize = 22,
            DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
            DataLabelsFormatter = point => 
                "Test",
            ToolTipLabelFormatter =  t => "Test ddd"
        });
    }

    private void UpdateTotals()
    {
        TotalIn = 0;
        TotalDif = 0;
        TotalOut = 0;
        
        foreach (var report in ReportItems)
        {
            TotalIn += report.YearlyBreakdown.Sum(x => x.TotalIn);
            TotalOut += report.YearlyBreakdown.Sum(x => x.TotalOut);
            TotalDif += decimal.Add(TotalOut, TotalIn);
        }
        

    }

}