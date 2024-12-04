using Avalonia;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.ViewModels;
using System.Collections.Generic;

namespace BudgetPlanner.Client.Controls;

public class OpenBankingAccounts : TemplatedControl
{
    public static readonly DirectProperty<OpenBankingAccounts, IEnumerable<AccountItemViewModel>> AccountsProperty =
    AvaloniaProperty.RegisterDirect<OpenBankingAccounts, IEnumerable<AccountItemViewModel>>(nameof(Accounts), p => p.Accounts, (p, v) => p.Accounts = v);

    private IEnumerable<AccountItemViewModel> _accounts = new List<AccountItemViewModel>();
    public IEnumerable<AccountItemViewModel> Accounts
    {
        get => _accounts;
        set => SetAndRaise(AccountsProperty, ref _accounts, value);
    }
}