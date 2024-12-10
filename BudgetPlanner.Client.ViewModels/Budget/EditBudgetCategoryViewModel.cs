using BudgetPlanner.Client.Messages.Budget;
using BudgetPlanner.Shared.Models.Request.Budget;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.BugetCategories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class EditBudgetCategoryViewModel : PageViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IBudgetCategoriesRequestService _budgetCategoriesRequestService;

        public EditBudgetCategoryViewModel(INavigationService navigationService, IBudgetCategoriesRequestService budgetCategoriesRequestService)
        {
            _navigationService = navigationService;
            _budgetCategoriesRequestService = budgetCategoriesRequestService;
        }

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private decimal _availableFunds;

        [ObservableProperty]
        private decimal _monthlyStart;

        [ObservableProperty]
        private decimal _savingsGoal;

        [ObservableProperty]
        private DateTime? _goalCompletionDate;


        [RelayCommand]
        public async Task EditBudgetCategoryAsync()
        {
            SetLoading(true, "Editing Category");

            var categoryToAdd = new AddBudgetCategoryRequest()
            {
                Name = Name,
                AvailableFunds = AvailableFunds,
                GoalCompletionDate = GoalCompletionDate,
                MonthlyStart = MonthlyStart,
                SavingsGoal = SavingsGoal
            };

            await _budgetCategoriesRequestService.AddBudgetCategoryAsync(categoryToAdd);

            WeakReferenceMessenger.Default.Send(new BudgetCategoriesChangedMessage(true));

            SetLoading(false);

            NavigateToBudgetCategories();
        }


        [RelayCommand]
        public void Cancel()
        {
            NavigateToBudgetCategories();
        }

        private void NavigateToBudgetCategories()
        {
            _navigationService.RequestNavigation<BudgetCategoriesViewModel>();
        }

    }

}
