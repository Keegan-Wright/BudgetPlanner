using Avalonia;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.ViewModels;
using System.Collections.Generic;

namespace BudgetPlanner.Client.Controls;

public class OpenBankingAccountTransactions : TemplatedControl
{
    public static readonly DirectProperty<OpenBankingAccountTransactions, IEnumerable<AccountItemTransactionViewModel>> TransactionsProperty =
AvaloniaProperty.RegisterDirect<OpenBankingAccountTransactions, IEnumerable<AccountItemTransactionViewModel>>(nameof(Transactions), p => p.Transactions, (p, v) => p.Transactions = v);

    private IEnumerable<AccountItemTransactionViewModel> _transactions = new List<AccountItemTransactionViewModel>();
    public IEnumerable<AccountItemTransactionViewModel> Transactions
    {
        get => _transactions;
        set => SetAndRaise(TransactionsProperty, ref _transactions, value);
    }
}