using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
{
    public partial class UpcomingPaymentViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _paymentName;

        [ObservableProperty]
        private decimal _amount;

        [ObservableProperty]
        private DateTime _paymentDate;

        [ObservableProperty]
        private string _paymentType;
    }

}
