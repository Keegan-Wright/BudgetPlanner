using BudgetPlanner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.ViewModels
{
    public partial class ExpensesViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        public ExpensesViewModel() { 
       
            navigationService = new NavigationService();
        }

        [ObservableProperty]
        private string _title = "Expenses Page";
        //https://github.com/AvaloniaUI/Avalonia.Samples/tree/main/src/Avalonia.Samples/Routing/BasicViewLocatorSample#base-class

        [RelayCommand]
        public void ToDebts()
        {
            navigationService.RequestNavigation(new DebtViewModel());
        }
    }
}
