using BudgetPlanner.Messages.Budget;
using BudgetPlanner.Models.Request.Budget;
using BudgetPlanner.Models.Request.HouseholdMember;
using BudgetPlanner.Services;
using BudgetPlanner.Services.Budget;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.ViewModels
{
    public partial class AddHouseholdMemberViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IHouseholdMembersService _householdMembersService;

        public AddHouseholdMemberViewModel(INavigationService navigationService, IHouseholdMembersService householdMembersService)
        {
            _navigationService = navigationService;
            _householdMembersService = householdMembersService;
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
            SetLoading(true, "Adding Household Member...");

            var householdMemberToAdd = new AddHouseholdMemberRequest() {
                FirstName = FirstName,
                LastName = LastName,
                Income = Income
            };

            await _householdMembersService.AddHouseholdMemberAsync(householdMemberToAdd);

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
