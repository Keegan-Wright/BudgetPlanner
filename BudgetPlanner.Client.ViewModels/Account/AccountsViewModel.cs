using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Shared.Enums;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class AccountsViewModel : PageViewModel
    {
        private readonly IAccountsRequestService _accountsRequestService;
        public AccountsViewModel(IAccountsRequestService accountsService)
        {
            _accountsRequestService = accountsService;

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

            await foreach (var account in _accountsRequestService.GetAccountsAndMostRecentTransactionsAsync(5, syncFlags, progress))
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
