using BudgetPlanner.Messages;
using BudgetPlanner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Net.Http.Headers;

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
        private ViewModelBase? _currentPage = Ioc.Default.GetService<HouseholdMembersViewModel>();




        [RelayCommand]
        public void TogglePane()
        {
            SideMenuExpanded = !SideMenuExpanded;
        }

        public void Receive(NavigationRequestedMessage message)
        {
            CurrentPage = message.Value;
        }
    }
}
