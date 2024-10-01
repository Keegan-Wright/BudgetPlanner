using BudgetPlanner.Enums;
using BudgetPlanner.Messages;
using BudgetPlanner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BudgetPlanner.ViewModels
{
    public partial class MainViewModel : ViewModelBase, IRecipient<NavigationRequestedMessage>, IRecipient<LoadingMessageChangedMessage>, IRecipient<LoadingStateChangedMessage>, IRecipient<ErrorOccuredMessage>, IDisposable
    {

        private readonly INavigationService _navigationService;
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            SetupNavigationItems();

            WeakReferenceMessenger.Default.Register<NavigationRequestedMessage>(this);
            WeakReferenceMessenger.Default.Register<LoadingMessageChangedMessage>(this);
            WeakReferenceMessenger.Default.Register<LoadingStateChangedMessage>(this);
            WeakReferenceMessenger.Default.Register<ErrorOccuredMessage>(this);
        }

        private void SetupNavigationItems()
        {
            NavigationItems =
            [
                new NavigationItemViewModel() { DisplayName = "Dashboard", RouteType = Enums.AppRoutes.Dashboard},
                new NavigationItemViewModel() { DisplayName = "Provider Setup", RouteType = Enums.AppRoutes.ProviderSetup},
                new NavigationItemViewModel() { DisplayName = "Household Members", RouteType = Enums.AppRoutes.HouseholdMembers},
                new NavigationItemViewModel() { DisplayName = "Budget Categories", RouteType = Enums.AppRoutes.BudgetCategories},
                new NavigationItemViewModel() { DisplayName = "Debts", RouteType = Enums.AppRoutes.Debts},
                new NavigationItemViewModel() { DisplayName = "Accounts", RouteType = Enums.AppRoutes.Accounts},
            ];

            SelectedNavigationItem = NavigationItems[0];
        }

        [ObservableProperty]
        private bool _sideMenuExpanded = true;

        [ObservableProperty]
        private bool _loading;

        [ObservableProperty]
        private string? _loadingMessage;

        [ObservableProperty]
        private bool _errorOccured;

        [ObservableProperty]
        private ObservableCollection<NavigationItemViewModel> _navigationItems;


        [ObservableProperty]
        private NavigationItemViewModel _selectedNavigationItem;


        public NavigationItemViewModel electedNavigationItem;

        [ObservableProperty]

        private ViewModelBase? _currentPage = Ioc.Default.GetService<DashboardViewModel>();


        private bool disposedValue;



        [RelayCommand]
        public void TogglePane()
        {
            SideMenuExpanded = !SideMenuExpanded;
        }


        partial void OnSelectedNavigationItemChanged(NavigationItemViewModel value)
        {
            SideMenuExpanded = false;

            switch (value.RouteType)
            {
                case AppRoutes.Dashboard:
                    _navigationService.RequestNavigation<DashboardViewModel>();
                    break;
                case AppRoutes.ProviderSetup:
                    _navigationService.RequestNavigation<SetupProviderViewModel>();
                    break;
                case AppRoutes.HouseholdMembers:
                    _navigationService.RequestNavigation<HouseholdMembersViewModel>();
                    break;
                case AppRoutes.BudgetCategories:
                    _navigationService.RequestNavigation<BudgetCategoriesViewModel>();
                    break;
                case AppRoutes.Debts:
                    _navigationService.RequestNavigation<DebtViewModel>();
                    break;
                case AppRoutes.Accounts:
                    _navigationService.RequestNavigation<AccountsViewModel>();
                    break;
                default:
                    throw new NotImplementedException($"Navigation for type {value.RouteType} is not implemented");
            }
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

        public void Receive(LoadingMessageChangedMessage message)
        {
            LoadingMessage = message.Value;
        }

        public void Receive(LoadingStateChangedMessage message)
        {
            Loading = message.Value;
        }

        public void Receive(ErrorOccuredMessage message)
        {
            ErrorOccured = message.Value;
        }
    }
}
