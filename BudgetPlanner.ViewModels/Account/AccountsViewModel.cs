using Avalonia.Media.Imaging;
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
    public partial class AccountsViewModel : ViewModelBase
    {
        private readonly IAccountsService _accountsService;
        public AccountsViewModel(IAccountsService accountsService)
        {
            _accountsService = accountsService;

            InitaliseAsync();
        }

        private async void InitaliseAsync()
        {
            SetLoading(true);
            var progress = new Progress<string>(s => SetLoadingMessage(s));

            var syncFlags = SyncTypes.Account | SyncTypes.Balance | SyncTypes.Transactions | SyncTypes.PendingTransactions;

            await foreach (var account in _accountsService.GetAccountsAndMostRecentTransactionsAsync(5, syncFlags, progress))
            {

                var accountToAdd = new AccountItemViewModel()
                {
                    AccountBalance = account.AccountBalance,
                    AccountName = account.AccountName,
                    AccountType = account.AccountType,
                    AvailableBalance = account.AvailableBalance,
                    Logo = account.Logo.Length != 0 ? new Bitmap(ByteArrayHelpers.ConvertSvgStreamToPngStream(account.Logo)) : null
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

        [ObservableProperty]
        private ObservableCollection<AccountItemViewModel> _accounts = [];
    }
}
