﻿using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class AccountItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _accountName;

        [ObservableProperty]
        private Bitmap _logo;

        [ObservableProperty]
        private string _accountType;
        [ObservableProperty]
        private decimal _accountBalance;

        [ObservableProperty]
        private decimal _availableBalance;

        [ObservableProperty]
        private ICollection<AccountItemTransactionViewModel> _transactions = [];

        public string AccountDisplayName => $"{AccountName} ({AccountType})";
    }
}
