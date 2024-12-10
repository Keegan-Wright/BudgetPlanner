using BudgetPlanner.Client.Messages.Budget;
using BudgetPlanner.Shared.Models.Request.HouseholdMember;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.HouseholdMember;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class AddHouseholdMemberViewModel : PageViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IHouseholdMemberRequestService _householdMembersRequestService;

        public AddHouseholdMemberViewModel(INavigationService navigationService, IHouseholdMemberRequestService householdMembersRequestService)
        {
            _navigationService = navigationService;
            _householdMembersRequestService = householdMembersRequestService;
        }

        [ObservableProperty]
        private string _firstName = string.Empty;

        [ObservableProperty]
        private string _lastName = string.Empty;

        [ObservableProperty]
        private decimal _income;


        [RelayCommand]
        public async Task AddHouseholdMemberAsync()
        {
            SetLoading(true, "Adding Household Member");

            var householdMemberToAdd = new AddHouseholdMemberRequest() {
                FirstName = FirstName,
                LastName = LastName,
                Income = Income
            };

            await _householdMembersRequestService.AddHouseholdMemberAsync(householdMemberToAdd);

            WeakReferenceMessenger.Default.Send(new HouseholdMembersChangedMessage(true));

            SetLoading(false);

            NavigateToHouseholdMembers();
        }


        [RelayCommand]
        public void Cancel()
        {
            NavigateToHouseholdMembers();
        }

        private void NavigateToHouseholdMembers()
        {
            _navigationService.RequestNavigation<HouseholdMembersViewModel>();
        }
    }
}
