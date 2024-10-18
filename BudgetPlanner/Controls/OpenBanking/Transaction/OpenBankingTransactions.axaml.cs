using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using BudgetPlanner.ViewModels;
using System;
using System.Collections.Generic;

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
}