using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
{
    public partial class SpentInTimePeriodWidgetViewModel : ViewModelBase
    {

        [ObservableProperty]
        private decimal _totalOut;

        [ObservableProperty]
        private decimal _totalIn;

        public decimal Normalised => decimal.Add(TotalOut, TotalIn);
    }
}
