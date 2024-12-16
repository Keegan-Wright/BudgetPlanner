using Avalonia;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.ViewModels;
using System.Collections.Generic;

namespace BudgetPlanner.Client.Controls;

public class OpenBankingAccount : TemplatedControl
{
    public static readonly DirectProperty<OpenBankingAccount, AccountItemViewModel> AccountProperty =
AvaloniaProperty.RegisterDirect<OpenBankingAccount, AccountItemViewModel>(nameof(Account), p => p.Account, (p, v) => p.Account = v);

    private AccountItemViewModel _account = new AccountItemViewModel();
    public AccountItemViewModel Account
    {
        get => _account;
        set => SetAndRaise(AccountProperty, ref _account, value);
    }

}