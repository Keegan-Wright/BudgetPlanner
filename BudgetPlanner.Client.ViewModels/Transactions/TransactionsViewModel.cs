using Avalonia.Threading;
using BudgetPlanner.Client.Handlers;
using BudgetPlanner.Shared.Models.Request.Transaction;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Transactions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BudgetPlanner.Shared.Enums;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class TransactionsViewModel : PageViewModel
    {
        private readonly ITransactionsRequestService _transactionsRequestService;
        private readonly INavigationService _navigationService;
        public TransactionsViewModel(ITransactionsRequestService transactionsRequestService, INavigationService navigationService)
        {
            _transactionsRequestService = transactionsRequestService;
            _navigationService = navigationService;
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

                [RelayCommand]
        public async Task LoadFilterItemsAsync()
        {
            
            SetLoading(true, "Loading filter items");
            await foreach (var provider in _transactionsRequestService.GetProvidersForTransactionFiltersAsync())
            {
                var viewModel = new TransactionProviderFilterViewModel()
                {
                    ProviderId = provider.ProviderId,
                    ProviderName = provider.ProviderName,
                };

                ProviderFilterItems.Add(viewModel);
            }

            await foreach (var account in _transactionsRequestService.GetAccountsForTransactionFiltersAsync())
            {
                var viewModel = new TransactionAccountFilterViewModel()
                {
                    AccountId = account.AccountId,
                    AccountName = account.AccountName,
                };

                AccountFilterItems.Add(viewModel);
            }

            await foreach (var type in _transactionsRequestService.GetTypesForTransactionFiltersAsync())
            {
                var viewModel = new TransactionTypeFilterViewModel()
                {
                    TransactionType = type.TransactionType
                };

                TypeFilterItems.Add(viewModel);
            }

            await foreach (var category in _transactionsRequestService.GetCategoriesForTransactionFiltersAsync())
            {
                var viewModel = new TransactionCategoryFilterViewModel()
                {
                    TransactionCategory = category.TransactionCategory
                };
                CategoryFilterItems.Add(viewModel);
            }

            await foreach(var tag in _transactionsRequestService.GetTagsForTransactionFiltersAsync())
            {
                var viewModel = new TransactionTagFilterViewModel()
                {
                    Tag = tag.Tag
                };
                TagFilterItems.Add(viewModel);
            }
            
            SetLoading(false);
        }
     

        private async Task LoadTransactionsAsync(FilteredTransactionsRequest searchCriteria)
        {
            SetLoading(true, "Loading Transactions");
            try
            {
                Dispatcher.UIThread.Invoke(Transactions.Clear);

                await foreach (var transaction in _transactionsRequestService.GetAllTransactionsAsync(searchCriteria, SyncTypes.Transactions | SyncTypes.PendingTransactions))
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
            catch(Exception? ex)
            {
                ErrorHandler.HandleError(ex);
            }


            SetLoading(false);
        }


    }

    
}
