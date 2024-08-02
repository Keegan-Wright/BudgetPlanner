using BudgetPlanner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.ViewModels
{
    public partial class DebtViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public DebtViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [ObservableProperty]
        private string _title = "Debt Page";
        //https://github.com/AvaloniaUI/Avalonia.Samples/tree/main/src/Avalonia.Samples/Routing/BasicViewLocatorSample#base-class

        [RelayCommand]
        public void ToExpenses()
        {
            _navigationService.RequestNavigation<ExpensesViewModel>();
        }
    }
}
