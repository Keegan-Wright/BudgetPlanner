using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels;

public partial class ReportItemStatsBaseViewModel : ViewModelBase
{
    [ObservableProperty] 
    private int _totalTransactions;

    [ObservableProperty] 
    private decimal _totalIn;

    [ObservableProperty]
    private decimal _totalOut;

    public decimal TotalDif => decimal.Add(TotalOut, TotalIn);
}