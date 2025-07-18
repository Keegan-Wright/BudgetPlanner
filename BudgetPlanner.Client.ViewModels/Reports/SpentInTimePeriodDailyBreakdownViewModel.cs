using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels;

public partial class SpentInTimePeriodDailyBreakdownViewModel : SpentInTimePeriodBreakdownBaseViewModel
{
    [ObservableProperty]
    private int _day;
}