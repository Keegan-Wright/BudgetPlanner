using System.Runtime.CompilerServices;
using BudgetPlanner.Messages.Budget;
using BudgetPlanner.Services;
using BudgetPlanner.Services.Budget;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.ViewModels
{
    public partial class HouseholdMembersViewModel : PageViewModel, IRecipient<HouseholdMembersChangedMessage>
    {
        private readonly INavigationService _navigationService;
        private readonly IHouseholdMembersService _householdMembersService;
        public HouseholdMembersViewModel(INavigationService navigationService, IHouseholdMembersService householdMembersService)
        {
            _navigationService = navigationService;
            _householdMembersService = householdMembersService;

            WeakReferenceMessenger.Default.Register(this);

            InitialiseAsync();
        }

        private async void InitialiseAsync()
        {
            await GetHouseholdMembersAsync();
        }




        [ObservableProperty]
        private ICollection<HouseholdMemberListItemViewModel> _householdMembers = [];


        [RelayCommand]
        public async Task GetHouseholdMembersAsync()
        {
            await LoadHouseholdMembersAsync();
        }

        private async Task LoadHouseholdMembersAsync()
        {
            SetLoading(true, "Loading Categories...");

            HouseholdMembers.Clear(); 

            await foreach (var householdMember in _householdMembersService.GetHouseholdMembersAsync())
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
        public void NavigateToAddHouseholdMember()
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
