using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using BudgetPlanner.ViewModels;
using System;
using System.Collections.Generic;

namespace BudgetPlanner.Controls;

public class OpenBankingTransaction : TemplatedControl
{

    public static readonly DirectProperty<OpenBankingTransaction, TransactionItemViewModel> TransactionProperty =
AvaloniaProperty.RegisterDirect<OpenBankingTransaction, TransactionItemViewModel>(nameof(Transaction), p => p.Transaction, (p, v) => p.Transaction = v);

    private TransactionItemViewModel _transaction = new TransactionItemViewModel();
    public TransactionItemViewModel Transaction
    {
        get => _transaction;
        set => SetAndRaise(TransactionProperty, ref _transaction, value);
    }
}