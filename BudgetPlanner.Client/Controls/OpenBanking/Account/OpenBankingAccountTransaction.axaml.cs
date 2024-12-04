using Avalonia;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.ViewModels;
using System.Collections.Generic;

namespace BudgetPlanner.Client.Controls;

public class OpenBankingAccountTransaction : TemplatedControl
{
    public static readonly DirectProperty<OpenBankingAccountTransaction, AccountItemTransactionViewModel> TransactionProperty =
AvaloniaProperty.RegisterDirect<OpenBankingAccountTransaction, AccountItemTransactionViewModel>(nameof(Transaction), p => p.Transaction, (p, v) => p.Transaction = v);

    private AccountItemTransactionViewModel _transaction = new AccountItemTransactionViewModel();
    public AccountItemTransactionViewModel Transaction
    {
        get => _transaction;
        set => SetAndRaise(TransactionProperty, ref _transaction, value);
    }
}