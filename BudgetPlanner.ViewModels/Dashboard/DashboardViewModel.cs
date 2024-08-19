using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
{
    public partial class DashboardViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _title = "Dashboard";
    }
}
