using BudgetPlanner.Services;
using BudgetPlanner.Services.Budget;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.ViewModels
{
    public partial class BudgetCategoriesViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IBudgetCategoriesService _budgetsService;
        public BudgetCategoriesViewModel(INavigationService navigationService, IBudgetCategoriesService budgetsService)
        {
            _navigationService = navigationService;
            _budgetsService = budgetsService;

            Initialise();
        }

        private async void Initialise()
        {
            await GetBudgetCategories();
        }

        [ObservableProperty]
        private string _title = "Your Budgets";

        [ObservableProperty]
        private bool _loading = false;


        [ObservableProperty]
        private ICollection<BudgetCategoryItemViewModel> _budgetCategories = [];


        [RelayCommand]
        public async Task GetBudgetCategories()
        {
            Loading = true;

            await foreach (var category in _budgetsService.GetBudgetItemsAsync())
            {
                BudgetCategories.Add(new BudgetCategoryItemViewModel()
                {
                    Name = category.Name,
                    AvailbleFunds = category.AvailbleFunds,
                    GoalCompletionDate = category.GoalCompletionDate,
                    MonthlyStart = category.MonthlyStart,
                    SavingsGoal = category.SavingsGoal
                });
            }

            Loading = false;
        }
    }
}
