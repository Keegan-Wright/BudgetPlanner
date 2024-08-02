using BudgetPlanner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
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
        private readonly INavigationService navigationService;
        public DebtViewModel()
        {

            navigationService = new NavigationService();
        }

        [ObservableProperty]
        private string _title = "Debt Page";
        //https://github.com/AvaloniaUI/Avalonia.Samples/tree/main/src/Avalonia.Samples/Routing/BasicViewLocatorSample#base-class

        [RelayCommand]
        public void ToExpenses()
        {
            navigationService.RequestNavigation(new ExpensesViewModel());
        }
    }
}
