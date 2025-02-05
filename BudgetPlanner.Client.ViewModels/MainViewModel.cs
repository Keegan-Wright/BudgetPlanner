using BudgetPlanner.Enums;
using BudgetPlanner.Client.Messages;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using BudgetPlanner.Client.Services.Auth;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class MainViewModel : ViewModelBase, IRecipient<NavigationRequestedMessage>, IRecipient<LoadingMessageChangedMessage>, IRecipient<LoadingStateChangedMessage>, IRecipient<ErrorOccuredMessage>, IDisposable
    {

        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authenticationService;
        public MainViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
        {
            _navigationService = navigationService;
            _authenticationService = authenticationService;

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
                new NavigationItemViewModel() { DisplayName = "Dashboard", RouteType = AppRoutes.Dashboard},
                new NavigationItemViewModel() { DisplayName = "Provider Setup", RouteType = AppRoutes.ProviderSetup},
                new NavigationItemViewModel() { DisplayName = "Household Members", RouteType = AppRoutes.HouseholdMembers},
                new NavigationItemViewModel() { DisplayName = "Budget Categories", RouteType = AppRoutes.BudgetCategories},
                new NavigationItemViewModel() { DisplayName = "Debts", RouteType = AppRoutes.Debts},
                new NavigationItemViewModel() { DisplayName = "Accounts", RouteType = AppRoutes.Accounts},
                new NavigationItemViewModel() { DisplayName = "Transactions", RouteType = AppRoutes.Transactions},
                new NavigationItemViewModel() { DisplayName = "Calendar", RouteType = AppRoutes.Calendar},
                new NavigationItemViewModel() { DisplayName = "Settings", 
                    SubItems = [ 
                        new NavigationItemViewModel() { DisplayName = "Classifications", RouteType = AppRoutes.SettingsClassifications },
                        new NavigationItemViewModel() { DisplayName = "Account", SubItems = [
                        new NavigationItemViewModel() { DisplayName = "Logout", RouteType = AppRoutes.Logout },]}
                    ]}
            ];
            
        }

        [ObservableProperty]
        private bool _sideMenuExpanded = true;

        [ObservableProperty]
        private bool _loading;

        [ObservableProperty]
        private bool _showSideMenu;


        [ObservableProperty]
        private string? _loadingMessage;

        [ObservableProperty]
        private bool _errorOccured;

        [ObservableProperty]
        private ObservableCollection<NavigationItemViewModel> _navigationItems;


        [ObservableProperty]
        private NavigationItemViewModel _selectedNavigationItem;



        [ObservableProperty]

        private ViewModelBase? _currentPage = Ioc.Default.GetService<LandingPageViewModel>();


        private bool disposedValue;



        [RelayCommand]
        public void TogglePane()
        {
            SideMenuExpanded = !SideMenuExpanded;
        }


        partial void OnSelectedNavigationItemChanged(NavigationItemViewModel value)
        {
            if (ApplicationState.IsDesktopBasedLifetime.HasValue && !ApplicationState.IsDesktopBasedLifetime.Value)
                SideMenuExpanded = false;

            if (value.RouteType == null)
                return;

            _showSideMenu = true;
            
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
                case AppRoutes.Transactions:
                    _navigationService.RequestNavigation<TransactionsViewModel>();
                    break;
                case AppRoutes.Calendar:
                    _navigationService.RequestNavigation<CalendarViewModel>();
                    break;
                case AppRoutes.SettingsClassifications:
                    _navigationService.RequestNavigation<ClassificationSettingsViewModel>();
                    break;
                case AppRoutes.Logout:
                    _ = _authenticationService.LogoutAsync();
                    _navigationService.RequestNavigation<LoginViewModel>();
                    break;
                default:
                    throw new NotImplementedException($"Navigation for type {value.RouteType} is not implemented");
            }
        }

        public void Receive(NavigationRequestedMessage message)
        {

            var type = message.Value.GetType();

            if (type == typeof(LandingPageViewModel) || type == typeof(LoginViewModel) ||
                type == typeof(RegisterViewModel))
            {
                ShowSideMenu = false;
                SideMenuExpanded = false;
            }
            else
            {
                ShowSideMenu = true;
            }

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

        public async void Receive(ErrorOccuredMessage message)
        {
            ErrorOccured = message.Value;

            RemoveError();

        }

        private async void RemoveError()
        {
            await Task.Delay(1000 * 10);
            ErrorOccured = false;
        }
    }
}
