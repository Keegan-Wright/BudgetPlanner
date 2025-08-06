using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels;

public partial class SpentInCategoryReportItemViewModel : ReportItemStatsBaseViewModel
{
    
    [ObservableProperty]
    private string _category;
    
    [ObservableProperty]
    private ObservableCollection<SpentInCategoryReportYearlyBreakdownViewModel> _yearlyBreakdown = [];
}