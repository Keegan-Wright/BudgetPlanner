using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels;

public partial class CalendarItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private DateTime _date;
    
    [ObservableProperty]
    private ObservableCollection<CalendarGoalItemViewModel> _goals = [];
    
    [ObservableProperty]
    private ObservableCollection<CalendarEventItemViewModel> _events = [];
    
    [ObservableProperty]
    private ObservableCollection<CalendarTransactionItemViewModel> _transactions = [];
}