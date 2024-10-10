using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using BudgetPlanner.Models.Request.Transaction;
using BudgetPlanner.ViewModels;
using System;
using System.Collections.Generic;
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


    internal static readonly DirectProperty<OpenBankingTransactionFilters, TransactionProviderFilterViewModel?> SelectedProviderProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, TransactionProviderFilterViewModel?>(nameof(SelectedProvider), p => p.SelectedProvider, (p, v) => p.SelectedProvider = v);

    internal TransactionProviderFilterViewModel? _selectedProvider;
    internal TransactionProviderFilterViewModel? SelectedProvider
    {
        get => _selectedProvider;
        set => SetAndRaise(SelectedProviderProperty, ref _selectedProvider, value);
    }

    internal static readonly DirectProperty<OpenBankingTransactionFilters, TransactionAccountFilterViewModel?> SelectedAccountProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, TransactionAccountFilterViewModel?>(nameof(SelectedAccount), p => p.SelectedAccount, (p, v) => p.SelectedAccount = v);

    internal TransactionAccountFilterViewModel? _selectedAccount;
    internal TransactionAccountFilterViewModel? SelectedAccount
    {
        get => _selectedAccount;
        set => SetAndRaise(SelectedAccountProperty, ref _selectedAccount, value);
    }


    internal static readonly DirectProperty<OpenBankingTransactionFilters, TransactionCategoryFilterViewModel?> SelectedCategoryProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, TransactionCategoryFilterViewModel?>(nameof(SelectedCategory), p => p.SelectedCategory, (p, v) => p.SelectedCategory = v);

    internal TransactionCategoryFilterViewModel? _selectedCategory;
    internal TransactionCategoryFilterViewModel? SelectedCategory
    {
        get => _selectedCategory;
        set => SetAndRaise(SelectedCategoryProperty, ref _selectedCategory, value);
    }

    internal static readonly DirectProperty<OpenBankingTransactionFilters, TransactionTypeFilterViewModel?> SelectedTypeProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactionFilters, TransactionTypeFilterViewModel?>(nameof(SelectedType), p => p.SelectedType, (p, v) => p.SelectedType = v);

    internal TransactionTypeFilterViewModel? _selectedType;
    internal TransactionTypeFilterViewModel? SelectedType
    {
        get => _selectedType;
        set => SetAndRaise(SelectedTypeProperty, ref _selectedType, value);
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


        if (change.Property == SelectedProviderProperty)
        {
            CommandParameter.ProviderId = change.GetNewValue<TransactionProviderFilterViewModel?>()?.ProviderId;
        }

        if (change.Property == SelectedAccountProperty)
        {
            CommandParameter.AccountId = change.GetNewValue<TransactionAccountFilterViewModel?>()?.AccountId;
        }
        if (change.Property == SelectedCategoryProperty)
        {
            CommandParameter.Category = change.GetNewValue<TransactionCategoryFilterViewModel?>()?.TransactionCategory;
        }
        if (change.Property == SelectedTypeProperty)
        {
            CommandParameter.Type = change.GetNewValue<TransactionTypeFilterViewModel?>()?.TransactionType;
        }
        if (change.Property == SearchTermProperty)
        {
            CommandParameter.SearchTerm = change.GetNewValue<string?>();
        }

    }
}