using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Selection;
using Avalonia.Interactivity;
using BudgetPlanner.Models.Request.Transaction;
using BudgetPlanner.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

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

    public static readonly DirectProperty<OpenBankingTransactionFilters, ICommand?> SearchCommandProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, ICommand?>(nameof(SearchCommand), p => p.SearchCommand, (p, v) => p.SearchCommand = v);

    private ICommand? _searchCommand;
    public ICommand? SearchCommand
    {
        get => _searchCommand;
        set => SetAndRaise(SearchCommandProperty, ref _searchCommand, value);
    }

    internal static readonly DirectProperty<OpenBankingTransactionFilters,FilteredTransactionsRequest> CommandParameterProperty =
    AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, FilteredTransactionsRequest>(nameof(CommandParameter), p => p.CommandParameter, (p,v) => p.CommandParameter = v);

    internal FilteredTransactionsRequest _commandParameter = new();
    internal FilteredTransactionsRequest CommandParameter
    {
        get => _commandParameter;
        set => SetAndRaise(CommandParameterProperty, ref _commandParameter, value);
    }


    internal static readonly DirectProperty<OpenBankingTransactionFilters, IList?> SelectedProvidersProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, IList?>(nameof(SelectedProviders), p => p.SelectedProviders, (p, v) => p.SelectedProviders = v);

    internal IList? _selectedProviders;
    internal IList? SelectedProviders
    {
        get => _selectedProviders;
        set => SetAndRaise(SelectedProvidersProperty, ref _selectedProviders, value);
    }


    internal static readonly DirectProperty<OpenBankingTransactionFilters, IList?> SelectedAccountsProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, IList?>(nameof(SelectedAccounts), p => p.SelectedAccounts, (p, v) => p.SelectedAccounts = v);

    internal IList? _selectedAccounts;
    internal IList? SelectedAccounts
    {
        get => _selectedAccounts;
        set => SetAndRaise(SelectedAccountsProperty, ref _selectedAccounts, value);
    }

    internal static readonly DirectProperty<OpenBankingTransactionFilters, IList?> SelectedCategoriesProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, IList?>(nameof(SelectedCategories), p => p.SelectedCategories, (p, v) => p.SelectedCategories = v);

    internal IList? _selectedCategories;
    internal IList? SelectedCategories
    {
        get => _selectedCategories;
        set => SetAndRaise(SelectedCategoriesProperty, ref _selectedCategories, value);
    }


    internal static readonly DirectProperty<OpenBankingTransactionFilters, IList?> SelectedTypesProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, IList?>(nameof(SelectedTypes), p => p.SelectedTypes, (p, v) => p.SelectedTypes = v);

    internal IList? _selectedTypes;
    internal IList? SelectedTypes
    {
        get => _selectedTypes;
        set => SetAndRaise(SelectedTypesProperty, ref _selectedTypes, value);
    }

    internal static readonly DirectProperty<OpenBankingTransactionFilters, string?> SearchTermProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, string?>(nameof(SearchTerm), p => p.SearchTerm, (p, v) => p.SearchTerm = v);

    private string? _searchTerm;
    public string? SearchTerm
    {
        get => _searchTerm;
        set => SetAndRaise(SearchTermProperty, ref _searchTerm, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SelectedProvidersProperty)
        {
            var items = change.GetNewValue<IList?>();
            CommandParameter.ProviderIds.Clear();
            if (items is null)
                return;
            foreach (var item in items)
            {
                CommandParameter.ProviderIds.Add((item as TransactionProviderFilterViewModel).ProviderId);
            }
        }

        if (change.Property == SelectedAccountsProperty)
        {
            var items = change.GetNewValue<IList?>();
            CommandParameter.AccountIds.Clear();


            if (items is null)
                return;
            foreach (var item in items)
            {
                CommandParameter.AccountIds.Add((item as TransactionAccountFilterViewModel).AccountId);
            }
        }

        if (change.Property == SelectedCategoriesProperty)
        {
            var items = change.GetNewValue<IList?>();
            CommandParameter.Categories.Clear();
            if (items is null)
                return;
            foreach (var item in items)
            {
                CommandParameter.Categories.Add((item as TransactionCategoryFilterViewModel).TransactionCategory);
            }
        }

        if (change.Property == SelectedTypesProperty)
        {
            var items = change.GetNewValue<IList?>();
            CommandParameter.Types.Clear();
            if (items is null)
                return;
            foreach (var item in items)
            {
                CommandParameter.Types.Add((item as TransactionTypeFilterViewModel).TransactionType);
            }
        }


        if (change.Property == SearchTermProperty)
        {
            CommandParameter.SearchTerm = change.GetNewValue<string?>();
        }

    }
}