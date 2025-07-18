using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using BudgetPlanner.Client.Messages.Budget;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.HouseholdMember;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class HouseholdMembersViewModel : PageViewModel, IRecipient<HouseholdMembersChangedMessage>
    {
        private readonly INavigationService _navigationService;
        private readonly IHouseholdMemberRequestService _householdMemberRequestService;
        public HouseholdMembersViewModel(INavigationService navigationService, IHouseholdMemberRequestService householdMemberRequestService)
        {
            _navigationService = navigationService;
            _householdMemberRequestService = householdMemberRequestService;

            WeakReferenceMessenger.Default.Register(this);

        }

        [RelayCommand]
        private async Task InitialiseAsync()
        {
            await RunOnBackgroundThreadAsync(LoadHouseholdMembersAsync());
        }




        [ObservableProperty]
        private ObservableCollection<HouseholdMemberListItemViewModel> _householdMembers = [];


        [RelayCommand]
        private async Task GetHouseholdMembersAsync()
        {
            await LoadHouseholdMembersAsync();
        }

        private async Task LoadHouseholdMembersAsync()
        {
            SetLoading(true, "Loading Categories");

            HouseholdMembers.Clear(); 

            await foreach (var householdMember in _householdMemberRequestService.GetHouseholdMembersAsync())
            {
                HouseholdMembers.Add(new HouseholdMemberListItemViewModel()
                {
                    FirstName = householdMember.FirstName,
                    LastName = householdMember.LastName,
                    Income = householdMember.Income
                });
            }

            SetLoading(false);
        }

        [RelayCommand]
        private void NavigateToAddHouseholdMember()
        {
            _navigationService.RequestNavigation<AddHouseholdMemberViewModel>();
        }

        public async void Receive(HouseholdMembersChangedMessage message)
        {
            if (message.Value)
            {
                await LoadHouseholdMembersAsync();
            }
        }
    }
}
