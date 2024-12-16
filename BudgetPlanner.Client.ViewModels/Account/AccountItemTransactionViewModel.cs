using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class AccountItemTransactionViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private decimal _amount;

        [ObservableProperty]
        private DateTime _time;

        [ObservableProperty]
        private string _status;
    }
}
