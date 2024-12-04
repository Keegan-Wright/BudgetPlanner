using Avalonia.Media.Imaging;
using Avalonia.Threading;
using BudgetPlanner.Enums;
using BudgetPlanner.Extensions;
using BudgetPlanner.Services.Accounts;
using CommunityToolkit.Mvvm.ComponentModel;
using Svg;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Drawing.Imaging;

namespace BudgetPlanner.ViewModels
{
    public partial class AccountsViewModel : PageViewModel
    {
        private readonly IAccountsService _accountsService;
        public AccountsViewModel(IAccountsService accountsService)
        {
            _accountsService = accountsService;

            InitialiseAsync();
        }

        private async void InitialiseAsync()
        {
            await RunOnBackgroundThreadAsync(async () => await LoadDataAsync());
        }

        [ObservableProperty]
        private ObservableCollection<AccountItemViewModel> _accounts = [];

        private async Task LoadDataAsync()
        {
            var progress = new Progress<string>(s => SetLoadingMessage(s));

            var syncFlags = SyncTypes.Account | SyncTypes.Balance | SyncTypes.Transactions | SyncTypes.PendingTransactions;

            SetLoading(true);

            await foreach (var account in _accountsService.GetAccountsAndMostRecentTransactionsAsync(5, syncFlags, progress))
            {
                using var logo = new MemoryStream(account.Logo);

                var accountToAdd = new AccountItemViewModel()
                {
                    AccountBalance = account.AccountBalance,
                    AccountName = account.AccountName,
                    AccountType = account.AccountType,
                    AvailableBalance = account.AvailableBalance,
                    Logo = new Bitmap(logo)
                };

                await foreach (var transaction in account.Transactions)
                {
                    var transactionToAdd = new AccountItemTransactionViewModel()
                    {
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        Status = transaction.Status,
                        Time = transaction.Time
                    };
                    accountToAdd.Transactions.Add(transactionToAdd);
                }

                Accounts.Add(accountToAdd);

            }

            SetLoading(false);

        }
    }
}
