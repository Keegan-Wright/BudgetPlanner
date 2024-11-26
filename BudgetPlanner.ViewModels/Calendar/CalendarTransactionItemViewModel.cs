using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels;

public partial class CalendarTransactionItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private decimal _amount;
    
    [ObservableProperty]
    private string _description;
    
    [ObservableProperty]
    private string _transactionType;
    
    [ObservableProperty]
    private DateTime _transactionTime;
}