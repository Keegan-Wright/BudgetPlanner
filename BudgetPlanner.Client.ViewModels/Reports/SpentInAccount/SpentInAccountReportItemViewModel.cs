using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels;

public partial class SpentInAccountReportItemViewModel : ReportItemStatsBaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<SpentInAccountReportYearlyBreakdownViewModel> _yearlyBreakdown = [];

    [ObservableProperty] 
    private  string _account;
}