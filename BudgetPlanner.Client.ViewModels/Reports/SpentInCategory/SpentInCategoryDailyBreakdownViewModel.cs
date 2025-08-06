using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels;

public partial class SpentInCategoryDailyBreakdownViewModel : ReportItemStatsBaseViewModel
{
    [ObservableProperty]
    private int _day;
}