using BudgetPlanner.Messages;
using BudgetPlanner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.ViewModels
{
    public partial class MainViewModel : ViewModelBase, IRecipient<NavigationRequestedMessage>
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


        public void Receive(NavigationRequestedMessage message)
        {
            CurrentPage = message.Value;
        }
    }
}
