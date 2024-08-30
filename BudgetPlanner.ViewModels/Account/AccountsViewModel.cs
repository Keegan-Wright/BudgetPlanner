using BudgetPlanner.Services.Accounts;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

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
            await foreach(var account in _accountsService.GetAccountsAndMostRecentTransactions(5))
            {
                var accountToAdd = new AccountItemViewModel()
                {
                    AccountBalance = account.AccountBalance,
                    AccountName = account.AccountName,
                    AccountType = account.AccountType,
                    AvailableBalance = account.AvailableBalance
                };

                await foreach(var transaction in account.Transactions)
                {
                    var transactionToAdd = new AccountItemTransactionViewModel()
                    {
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        Status = transaction.Status,
                        Time = transaction.Time,
                    };
                    accountToAdd.Transactions.Add(transactionToAdd);
                }

                Accounts.Add(accountToAdd);
            }
        }

        [ObservableProperty]
        private ICollection<AccountItemViewModel> _accounts = [];
    }
}
