using BudgetPlanner.Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class ExpensesViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public ExpensesViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [ObservableProperty]
        private string _title = "Expenses Page";
        //https://github.com/AvaloniaUI/Avalonia.Samples/tree/main/src/Avalonia.Samples/Routing/BasicViewLocatorSample#base-class

        [RelayCommand]
        public void ToDebts()
        {
            _navigationService.RequestNavigation<DebtViewModel>();
        }
    }
}
