using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using BudgetPlanner.Models.Request.Transaction;
using BudgetPlanner.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

namespace BudgetPlanner.Controls;

public class OpenBankingTransactions : TemplatedControl
{

    public static readonly DirectProperty<OpenBankingTransactions, IEnumerable<TransactionItemViewModel>> TransactionsProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactions, IEnumerable<TransactionItemViewModel>>(nameof(Transactions), p => p.Transactions, (p, v) => p.Transactions = v);

    private IEnumerable<TransactionItemViewModel> _transactions = new List<TransactionItemViewModel>();
    public IEnumerable<TransactionItemViewModel> Transactions
    {
        get => _transactions;
        set => SetAndRaise(TransactionsProperty, ref _transactions, value);
    }

    public static readonly DirectProperty<OpenBankingTransactions, TransactionItemViewModel?> SelectedItemProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactions, TransactionItemViewModel?>(nameof(SelectedItem), p => p.SelectedItem, (p, v) => p.SelectedItem = v);

    private TransactionItemViewModel? _selectedItem;
    public TransactionItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set => SetAndRaise(SelectedItemProperty, ref _selectedItem, value);
    }

    public static readonly DirectProperty<OpenBankingTransactions, ICommand?> AddCustomClassificationCommandProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransactions, ICommand?>(nameof(AddCustomClassificationCommand), p => p.AddCustomClassificationCommand, (p, v) => p.AddCustomClassificationCommand = v);

    private ICommand? _addCustomClassificationCommand;
    public ICommand? AddCustomClassificationCommand
    {
        get => _addCustomClassificationCommand;
        set => SetAndRaise(AddCustomClassificationCommandProperty, ref _addCustomClassificationCommand, value);
    }

    //internal static readonly DirectProperty<OpenBankingTransactions, TransactionItemViewModel> AddCustomClassificationCommandParameterProperty =
    //AvaloniaProperty.RegisterDirect<OpenBankingTransactions, TransactionItemViewModel>(nameof(AddCustomClassificationCommandParameter), p => p.AddCustomClassificationCommandParameter, (p, v) => p.AddCustomClassificationCommandParameter = v);

    //internal TransactionItemViewModel _addCustomClassificationCommandParameter = new();
    //internal TransactionItemViewModel AddCustomClassificationCommandParameter
    //{
    //    get => _addCustomClassificationCommandParameter;
    //    set => SetAndRaise(AddCustomClassificationCommandParameterProperty, ref _addCustomClassificationCommandParameter, value);
    //}

}