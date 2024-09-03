using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetPlanner.Services.OpenBanking;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.ViewModels
{
    public partial class DashboardViewModel : ViewModelBase
    {
        private readonly IOpenBankingService _openBankingService;

        public DashboardViewModel(IOpenBankingService openBankingService)
        {
            _openBankingService = openBankingService;
        }

        [ObservableProperty]
        private string _title = "Dashboard";

        [ObservableProperty]
        private UpcomingPaymentsWidgetViewModel _upcomingPayments;

        [RelayCommand]
        public async Task GetDashboardStats()
        {
            var accountCounter = 0;
            var totalBalance = 0m;
            var availableBalance = 0m;
            var pendingTransactions = 0;
            var totalTransactions = 0;
            var totalDirectDebits = 0;
            var totalStandingOrders = 0;

            await foreach (var account in _openBankingService.GetOpenBankingAccountsAsync())
            {
                accountCounter++;

                await foreach (var balance in _openBankingService.GetOpenBankingAccountBalanceAsync(account.Provider.ProviderId, account.AccountId))
                {
                    availableBalance += balance.Available;
                    totalBalance += balance.Current;
                }

                await foreach (var _ in _openBankingService.GetOpenBankingAccountPendingTransactionsAsync(account.Provider.ProviderId, account.AccountId))
                {
                    pendingTransactions++;
                    totalTransactions++;
                }

                await foreach (var _ in _openBankingService.GetOpenBankingAccountTransactionsAsync(account.Provider.ProviderId, account.AccountId))
                {
                    totalTransactions++;
                }


                await foreach (var _ in _openBankingService.GetOpenBankingAccountDirectDebitsAsync(account.Provider.ProviderId, account.AccountId))
                {
                    totalDirectDebits++;
                }

                await foreach (var _ in _openBankingService.GetOpenBankingAccountStandingOrdersAsync(account.Provider.ProviderId, account.AccountId))
                {
                    totalStandingOrders++;
                }


            }
        }


    }
}