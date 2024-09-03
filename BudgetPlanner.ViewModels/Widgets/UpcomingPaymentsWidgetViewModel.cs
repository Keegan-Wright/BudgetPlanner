using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.ViewModels
{
    public partial class UpcomingPaymentsWidgetViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<UpcomingPaymentViewModel> _payments = [];
    }

}
