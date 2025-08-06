using System.Collections.ObjectModel;
using System.Globalization;
using BudgetPlanner.Shared.Models.Response.Reports;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace BudgetPlanner.Client.ViewModels;

public partial class SpentInCategoryReportYearlyBreakdownViewModel : ReportItemStatsBaseViewModel
{
    [ObservableProperty]
    private int _year;
    
    [ObservableProperty]
    private ObservableCollection<SpentInCategoryReportMonthlyBreakdownViewModel> _monthlyBreakdown = [];

    [ObservableProperty] private ObservableCollection<ISeries> _monthlyBreakdownSeries = [];

    [ObservableProperty]
    private Axis _axisX = new Axis();

    [RelayCommand]
    public async Task UpdateGraphData()
    {
        if (MonthlyBreakdownSeries.Any())
            return;
        
        var totalInSeries = new LineSeries<decimal>
        {
            Name = "Total In",
            Values = new ObservableCollection<decimal>(MonthlyBreakdown.Select(x => x.TotalIn))
        };
        
        var totalOutSeries = new LineSeries<decimal>
        {
            Name = "Total Out",
            Values = new ObservableCollection<decimal>(MonthlyBreakdown.Select(x => x.TotalDif))
        };
        
        var difSeries = new LineSeries<decimal>
        {
            Name = "Difference",
            Values = new ObservableCollection<decimal>(MonthlyBreakdown.Select(x => x.TotalDif))
        };
        
        var transactions = new LineSeries<int>
        {
            Name = "Transactions",
            Values = new ObservableCollection<int>(MonthlyBreakdown.Select(x => x.TotalTransactions))
        };
        
        MonthlyBreakdownSeries = new ObservableCollection<ISeries>
        {
            totalInSeries,
            totalOutSeries,
            difSeries,
            transactions
        };

        AxisX = new Axis()
        {
            Name = "Months",
            Labels = DateTimeFormatInfo.CurrentInfo.MonthNames
        };

    }
}