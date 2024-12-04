using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels;

public partial class CalendarEventItemViewModel : ViewModelBase
{
    [ObservableProperty] 
    private string _description;
}