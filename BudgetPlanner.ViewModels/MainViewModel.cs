using BudgetPlanner.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
            WeakReferenceMessenger.Default.Register<NavigationRequestedMessage>(this, (r, m) => { CurrentPage = m.Value; });
        }

        [ObservableProperty]
        private bool _sideMenuExpanded = true;


        [ObservableProperty]
        private string _greeting = "Welcome to Avalonia!";

        [ObservableProperty]
        private ViewModelBase _currentPage= new ExpensesViewModel();




        [RelayCommand]
        public void TogglePane()
        {
            SideMenuExpanded = !SideMenuExpanded;
        }



    }
}
