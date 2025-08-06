using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels;

public partial class SpentInTimePeriodDailyBreakdownViewModel : ReportItemStatsBaseViewModel
{
    [ObservableProperty]
    private int _day;
}