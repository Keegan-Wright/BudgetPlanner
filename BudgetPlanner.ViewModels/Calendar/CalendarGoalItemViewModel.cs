using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels;

public partial class CalendarGoalItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _name;
    
    [ObservableProperty]
    private DateTime? _goalCompletionDate;
}