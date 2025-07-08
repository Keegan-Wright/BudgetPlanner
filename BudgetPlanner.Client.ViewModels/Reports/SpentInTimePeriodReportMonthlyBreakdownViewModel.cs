using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace BudgetPlanner.Client.ViewModels;

public partial class SpentInTimePeriodReportMonthlyBreakdownViewModel : SpentInTimePeriodBreakdownBaseViewModel
{
    [ObservableProperty] 
    private string _month;
    
    [ObservableProperty]
    private ObservableCollection<SpentInTimePeriodDailyBreakdownViewModel> _dailyBreakdown = [];
    
    [ObservableProperty] private ObservableCollection<ISeries> _dailyBreakdownSeries = [];

    [ObservableProperty]
    private Axis _axisX = new Axis();

    [RelayCommand]
    public async Task UpdateGraphData()
    {
        AxisX = new Axis()
        {
            Name = "Months",
            Labels = ["1","2","3","4","5","6","7","8","9","10","11","12"]
        };
        
        var totalInSeries = new LineSeries<decimal>
        {
            Name = "Total In",
            Values = new ObservableCollection<decimal>(DailyBreakdown.Select(x => x.TotalIn))
        };
        
        var totalOutSeries = new LineSeries<decimal>
        {
            Name = "Total Out",
            Values = new ObservableCollection<decimal>(DailyBreakdown.Select(x => x.TotalDif))
        };
        
        var difSeries = new LineSeries<decimal>
        {
            Name = "Difference",
            Values = new ObservableCollection<decimal>(DailyBreakdown.Select(x => x.TotalDif))
        };
        
        var transactions = new LineSeries<int>
        {
            Name = "Transactions",
            Values = new ObservableCollection<int>(DailyBreakdown.Select(x => x.TotalTransactions))
        };
        
        DailyBreakdownSeries = new ObservableCollection<ISeries>
        {
            totalInSeries,
            totalOutSeries,
            difSeries,
            transactions
        };
    }
}