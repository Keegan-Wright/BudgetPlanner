using BudgetPlanner.Messages;
using BudgetPlanner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Net.Http.Headers;

namespace BudgetPlanner.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService;
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            WeakReferenceMessenger.Default.Register<NavigationRequestedMessage>(this, (r, m) => { CurrentPage = m.Value; });

        }

        [ObservableProperty]
        private bool _sideMenuExpanded = true;


        [ObservableProperty]
        private string _greeting = "Welcome to Avalonia!";

        [ObservableProperty]
        private ViewModelBase? _currentPage = Ioc.Default.GetService<ExpensesViewModel>();




        [RelayCommand]
        public void TogglePane()
        {
            SideMenuExpanded = !SideMenuExpanded;
        }



    }
}
