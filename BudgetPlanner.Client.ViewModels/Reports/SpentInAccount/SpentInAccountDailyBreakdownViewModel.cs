using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels;

public partial class SpentInAccountDailyBreakdownViewModel : ReportItemStatsBaseViewModel
{
    [ObservableProperty]
    private int _day;
}