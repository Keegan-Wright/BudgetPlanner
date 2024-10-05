using Avalonia.Controls;
using Avalonia.Interactivity;
using BudgetPlanner.Models.Response.Transaction;
using BudgetPlanner.Services.Transactions;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.ViewModels
{
    public partial class TransactionsViewModel : PageViewModel
    {
        private readonly ITransactionsService _transactionsService;
        public TransactionsViewModel(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;

            InitaliseAsync();
        }

        [ObservableProperty]
        private ObservableCollection<TransactionItemViewModel> _transactions = [];

        [ObservableProperty]
        private ObservableCollection<TransactionProviderFilterViewModel> _providerFilterItems = [];

        [ObservableProperty]
        private ObservableCollection<TransactionAccountFilterViewModel> _accountFilterItems = [];

        [ObservableProperty]
        private ObservableCollection<TransactionCategoryFilterViewModel> _categoryFilterItems = [];

        [ObservableProperty]
        private ObservableCollection<TransactionTypeFilterViewModel> _typeFilterItems = [];

        [ObservableProperty]
        private string _searchTerm = string.Empty;


        public void CommonClickHandler(object sender, RoutedEventArgs e)
        {
            var source = e.Source as Control;
            switch (source.Name)
            {
                case "YesButton":
                    // do something here ...
                    break;
                case "NoButton":
                    // do something ...
                    break;
                case "CancelButton":
                    // do something ...
                    break;
            }
            e.Handled = true;
        }

        private async void InitaliseAsync()
        {
            SetLoading(true, "Loading filter items");

            await RunOnBackgroundThreadAsync(async () => await LoadFilterItemsAsync());

            SetLoading(false);

            await LoadTransactionsAsync();
        }

        private async Task LoadTransactionsAsync()
        {
            SetLoading(true, "Loading Transactions");
            await foreach (var transaction in _transactionsService.GetAllTransactionsAsync())
            {
                var viewModel = new TransactionItemViewModel()
                {
                    Amount = transaction.Amount,
                    Currency = transaction.Currency,
                    Description = transaction.Description,
                    Pending = transaction.Pending,
                    TransactionCategory = transaction.TransactionCategory,
                    TransactionId = transaction.TransactionId,
                    TransactionTime = transaction.TransactionTime,
                    TransactionType = transaction.TransactionType,
                };

                Transactions.Add(viewModel);
            }
            SetLoading(false);
        }

        private async Task LoadFilterItemsAsync()
        {
            await foreach (var provider in _transactionsService.GetProvidersForTransactionFiltersAsync())
            {
                var viewModel = new TransactionProviderFilterViewModel()
                {
                    ProviderId = provider.ProviderId,
                    ProviderName = provider.ProviderName,
                };

                ProviderFilterItems.Add(viewModel);
            }

            await foreach (var account in _transactionsService.GetAccountsForTransactionFiltersAsync())
            {
                var viewModel = new TransactionAccountFilterViewModel()
                {
                    AccountId = account.AccountId,
                    AccountName = account.AccountName,
                };

                AccountFilterItems.Add(viewModel);
            }

            await foreach (var type in _transactionsService.GetTypesForTransactionFiltersAsync())
            {
                var viewModel = new TransactionTypeFilterViewModel()
                {
                    TransactionType = type.TransactionType
                };

                TypeFilterItems.Add(viewModel);
            }

            await foreach (var category in _transactionsService.GetCategoriesForTransactionFiltersAsync())
            {
                var viewModel = new TransactionCategoryFilterViewModel()
                {
                    TransactionCategory = category.TransactionCategory
                };
                CategoryFilterItems.Add(viewModel);
            }
        }
    }

    
}
