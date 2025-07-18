using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels;

public partial class SpentInTimePeriodReportItemViewModel : SpentInTimePeriodBreakdownBaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<SpentInTimePeriodReportYearlyBreakdownViewModel> _yearlyBreakdown = [];
}