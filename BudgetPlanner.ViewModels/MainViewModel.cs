using BudgetPlanner.Messages;
using BudgetPlanner.Services;
using BudgetPlanner.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Net.NetworkInformation;

namespace BudgetPlanner.ViewModels
{
    public partial class MainViewModel : ViewModelBase, IRecipient<NavigationRequestedMessage>, IDisposable
    {

        private readonly INavigationService _navigationService;
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            WeakReferenceMessenger.Default.Register(this);
        }




        [ObservableProperty]
        private bool _sideMenuExpanded = true;


        [ObservableProperty]
        private string _greeting = "Welcome to Avalonia!";

        [ObservableProperty]
        private ViewModelBase? _currentPage = Ioc.Default.GetService<DashboardViewModel>();
        private bool disposedValue;

        [RelayCommand]
        public void TogglePane()
        {
            SideMenuExpanded = !SideMenuExpanded;
        }

        [RelayCommand]
        public void NavigateToDebts()
        {
            _navigationService.RequestNavigation<DebtViewModel>();
        }

        [RelayCommand]
        public void NavigateToBudgetCategories()
        {
            _navigationService.RequestNavigation<BudgetCategoriesViewModel>();

        }

        [RelayCommand]
        public void NavigateToHouseholdMembers()
        {
            _navigationService.RequestNavigation<HouseholdMembersViewModel>();
        }

        [RelayCommand]
        public void NavigateToDashboard()
        {
            _navigationService.RequestNavigation<DashboardViewModel>();
        }

        [RelayCommand]
        public void NavigateToProviderSetup()
        {
            _navigationService.RequestNavigation<SetupProviderViewModel>();
        }

        [RelayCommand]
        public void NavigateToAccounts()
        {
            _navigationService.RequestNavigation<AccountsViewModel>();
        }

        public void Receive(NavigationRequestedMessage message)
        {
            CurrentPage = message.Value;
        }

        [RelayCommand]
        public void ChangeTheme()
        {
            WeakReferenceMessenger.Default.Send(new ThemeChangeRequestedMessage(true));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
