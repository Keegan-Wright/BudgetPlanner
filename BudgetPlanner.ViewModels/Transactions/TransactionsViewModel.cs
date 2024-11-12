using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using BudgetPlanner.Enums;
using BudgetPlanner.Handlers;
using BudgetPlanner.Models.Request.Transaction;
using BudgetPlanner.Models.Response.Transaction;
using BudgetPlanner.Services;
using BudgetPlanner.Services.Transactions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private readonly INavigationService _navigationService;
        public TransactionsViewModel(ITransactionsService transactionsService, INavigationService navigationService)
        {
            _transactionsService = transactionsService;
            _navigationService = navigationService;
            InitialiseAsync();

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
        private ObservableCollection<TransactionTagFilterViewModel> _tagFilterItems = [];


        [RelayCommand]
        public async Task SearchTransactionsAsync(FilteredTransactionsRequest searchCriteria)
        {
;
            await RunOnBackgroundThreadAsync(async () => await LoadTransactionsAsync(searchCriteria));
        }

        [RelayCommand]
        public void NavigateToAddCustomClassificationToTransaction(TransactionItemViewModel transactionItemViewModel)
        {
            _navigationService.RequestNavigation<AddCustomClassificationsToTransactionViewModel>(transactionItemViewModel);
        }

        private async void InitialiseAsync()
        {
            SetLoading(true, "Loading filter items");

            await RunOnBackgroundThreadAsync(async () => await LoadFilterItemsAsync());

            SetLoading(false);
        }

        private async Task LoadTransactionsAsync(FilteredTransactionsRequest searchCriteria)
        {
            SetLoading(true, "Loading Transactions");
            try
            {
                Dispatcher.UIThread.Invoke(Transactions.Clear);

                await foreach (var transaction in _transactionsService.GetAllTransactionsAsync(searchCriteria, SyncTypes.Transactions | SyncTypes.PendingTransactions))
                {
                    var viewModel = new TransactionItemViewModel()
                    {
                        Amount = transaction.Amount,
                        Currency = transaction.Currency,
                        Description = transaction.Description,
                        Pending = transaction.Pending,
                        TransactionCategory = transaction.TransactionCategory,
                        TransactionId = transaction.TransactionId,
                        TransactionDate = DateOnly.FromDateTime(transaction.TransactionTime),
                        TransactionType = transaction.TransactionType,
                        Tags = transaction.Tags.Select(tag => new TransactionTagFilterViewModel
                        {
                            Tag = tag
                        })
                    };

                    Dispatcher.UIThread.Invoke(() => Transactions.Add(viewModel));
                }
            }
            catch(Exception ex)
            {
                ErrorHandler.HandleError(ex);
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

            await foreach(var tag in _transactionsService.GetTagsForTransactionFiltersAsync())
            {
                var viewModel = new TransactionTagFilterViewModel()
                {
                    Tag = tag.Tag
                };
                TagFilterItems.Add(viewModel);
            }
        }
    }

    
}
