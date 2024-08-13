using System.Runtime.CompilerServices;
using BudgetPlanner.Messages.Budget;
using BudgetPlanner.Services;
using BudgetPlanner.Services.Budget;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.ViewModels
{
    public partial class BudgetCategoriesViewModel : ViewModelBase, IRecipient<BudgetCategoriesChangedMessage>
    {
        private readonly INavigationService _navigationService;
        private readonly IBudgetCategoriesService _budgetsService;
        public BudgetCategoriesViewModel(INavigationService navigationService, IBudgetCategoriesService budgetsService)
        {
            _navigationService = navigationService;
            _budgetsService = budgetsService;

            WeakReferenceMessenger.Default.Register(this);

            InitialiseAsync();
        }

        private async void InitialiseAsync()
        {
            await GetBudgetCategoriesAsync();
        }




        [ObservableProperty]
        private ICollection<BudgetCategoryListItemViewModel> _budgetCategories = [];


        [RelayCommand]
        public async Task GetBudgetCategoriesAsync()
        {
            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            SetLoading(true, "Loading Categories...");

            BudgetCategories.Clear(); 

            await foreach (var category in _budgetsService.GetBudgetItemsAsync())
            {
                BudgetCategories.Add(new BudgetCategoryListItemViewModel()
                {
                    Name = category.Name,
                    AvailableFunds = category.AvailableFunds,
                    GoalCompletionDate = category.GoalCompletionDate,
                    MonthlyStart = category.MonthlyStart,
                    SavingsGoal = category.SavingsGoal
                });
            }

            SetLoading(false);
        }

        [RelayCommand]
        public void NavigateToAddBudgetCategory()
        {
            _navigationService.RequestNavigation<AddBudgetCategoryViewModel>();
        }

        public async void Receive(BudgetCategoriesChangedMessage message)
        {
            if (message.Value)
            {
                await LoadCategories();
            }
        }
    }
}
