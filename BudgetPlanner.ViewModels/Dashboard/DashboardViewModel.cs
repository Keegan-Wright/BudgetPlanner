using System;
using System.Collections.Generic;
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

        [RelayCommand]
        public async Task TestOpenBanking()
        {
            await foreach (var account in _openBankingService.GetOpenBankingAccountsAsync())
            {
                var a = account;
            }
        }

    }
}