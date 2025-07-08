using System.Collections.ObjectModel;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Reports;
using BudgetPlanner.Client.Services.Transactions;
using BudgetPlanner.Shared.Models.Request.Transaction;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.Client.ViewModels;

public abstract partial class BaseReportPageViewModel<TReportResponse> : PageViewModel
{
    internal readonly IReportsService _reportsService;
    internal readonly INavigationService _navigationService;
    internal readonly ITransactionsRequestService _transactionsRequestService;
    
    
    
    protected BaseReportPageViewModel(IReportsService reportsService, INavigationService navigationService, ITransactionsRequestService transactionsRequestService)
    {
        _reportsService = reportsService;
        _navigationService = navigationService;
        _transactionsRequestService = transactionsRequestService;
    }
    
    
    
    [ObservableProperty]
    private ObservableCollection<TReportResponse> _reportItems = [];
    
    [ObservableProperty]
    private DateTime? _dateFrom;
    
    [ObservableProperty]
    private DateTime? _dateTo;
    
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

    
    [RelayCommand]
    public abstract Task LoadReportAsync(FilteredTransactionsRequest searchCriteria);
}