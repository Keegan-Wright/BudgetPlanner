using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels;

public partial class CalendarEventItemViewModel : ViewModelBase
{
    [ObservableProperty] 
    private string _description;
}