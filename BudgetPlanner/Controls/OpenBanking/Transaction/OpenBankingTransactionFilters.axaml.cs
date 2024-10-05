using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using BudgetPlanner.ViewModels;
using System;
using System.Collections.Generic;

namespace BudgetPlanner.Controls;

public class OpenBankingTransactionFilters : TemplatedControl
{

    public static readonly DirectProperty<OpenBankingTransactionFilters, IEnumerable<TransactionAccountFilterViewModel>> AccountsProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, IEnumerable<TransactionAccountFilterViewModel>>(nameof(Accounts), p => p.Accounts, (p, v) => p.Accounts = v);

    private IEnumerable<TransactionAccountFilterViewModel> _accounts = new List<TransactionAccountFilterViewModel>();
    public IEnumerable<TransactionAccountFilterViewModel> Accounts
    {
        get => _accounts;
        set => SetAndRaise(AccountsProperty, ref _accounts, value);
    }


    public static readonly DirectProperty<OpenBankingTransactionFilters, IEnumerable<TransactionProviderFilterViewModel>> ProvidersProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, IEnumerable<TransactionProviderFilterViewModel>>(nameof(Providers), p => p.Providers, (p, v) => p.Providers = v);

    private IEnumerable<TransactionProviderFilterViewModel> _providers = new List<TransactionProviderFilterViewModel>();
    public IEnumerable<TransactionProviderFilterViewModel> Providers
    {
        get => _providers;
        set => SetAndRaise(ProvidersProperty, ref _providers, value);
    }


    public static readonly DirectProperty<OpenBankingTransactionFilters, IEnumerable<TransactionCategoryFilterViewModel>> CategoriesProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, IEnumerable<TransactionCategoryFilterViewModel>>(nameof(Categories), p => p.Categories, (p, v) => p.Categories = v);

    private IEnumerable<TransactionCategoryFilterViewModel> _categories = new List<TransactionCategoryFilterViewModel>();
    public IEnumerable<TransactionCategoryFilterViewModel> Categories
    {
        get => _categories;
        set => SetAndRaise(CategoriesProperty, ref _categories, value);
    }


    public static readonly DirectProperty<OpenBankingTransactionFilters, IEnumerable<TransactionTypeFilterViewModel>> TypesProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, IEnumerable<TransactionTypeFilterViewModel>>(nameof(Types), p => p.Types, (p, v) => p.Types = v);

    private IEnumerable<TransactionTypeFilterViewModel> _types = new List<TransactionTypeFilterViewModel>();
    public IEnumerable<TransactionTypeFilterViewModel> Types
    {
        get => _types;
        set => SetAndRaise(TypesProperty, ref _types, value);
    }

    public static readonly DirectProperty<OpenBankingTransactionFilters, string> SearchTermProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, string>(nameof(SearchTerm), p => p.SearchTerm, (p, v) => p.SearchTerm= v);

    private string _searchTerm = "";
    public string SearchTerm
    {
        get => _searchTerm;
        set => SetAndRaise(SearchTermProperty, ref _searchTerm, value);
    }


    public static readonly RoutedEvent<RoutedEventArgs> FilterButtonClickedEvent =
    RoutedEvent.Register<OpenBankingTransactionFilters, RoutedEventArgs>(nameof(FilterButtonClicked), RoutingStrategies.Direct);

    public event EventHandler<RoutedEventArgs> FilterButtonClicked
    {
        add => AddHandler(FilterButtonClickedEvent, value);
        remove => RemoveHandler(FilterButtonClickedEvent, value);
    }

    protected virtual void OnValueChanged()
    {
        RoutedEventArgs args = new RoutedEventArgs(FilterButtonClickedEvent);
        RaiseEvent(args);
    }

}