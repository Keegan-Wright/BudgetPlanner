using BudgetPlanner.Messages.Budget;
using BudgetPlanner.Models.Request.Budget;
using BudgetPlanner.Services;
using BudgetPlanner.Services.Budget;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.ViewModels
{
    public partial class EditBudgetCategoryViewModel : PageViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IBudgetCategoriesService _budgetsService;

        public EditBudgetCategoryViewModel(INavigationService navigationService, IBudgetCategoriesService budgetsService)
        {
            _navigationService = navigationService;
            _budgetsService = budgetsService;
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

            await _budgetsService.AddBudgetCategoryAsync(categoryToAdd);

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
