using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using BudgetPlanner.Client.Messages.Budget;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.BugetCategories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class BudgetCategoriesViewModel : PageViewModel, IRecipient<BudgetCategoriesChangedMessage>
    {
        private readonly INavigationService _navigationService;
        private readonly IBudgetCategoriesRequestService _budgetCategoriesRequestService;
        public BudgetCategoriesViewModel(INavigationService navigationService, IBudgetCategoriesRequestService budgetCategoriesRequestService)
        {
            _navigationService = navigationService;
            _budgetCategoriesRequestService = budgetCategoriesRequestService;

            WeakReferenceMessenger.Default.Register(this);

            InitialiseAsync();
        }

        private async void InitialiseAsync()
        {
            await GetBudgetCategoriesAsync();
        }




        [ObservableProperty]
        private ObservableCollection<BudgetCategoryListItemViewModel> _budgetCategories = [];


        [RelayCommand]
        public async Task GetBudgetCategoriesAsync()
        {
            await LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            SetLoading(true, "Loading Categories");

            BudgetCategories.Clear(); 

            await foreach (var category in _budgetCategoriesRequestService.GetBudgetItemsAsync())
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
                await LoadCategoriesAsync();
            }
        }
    }
}
