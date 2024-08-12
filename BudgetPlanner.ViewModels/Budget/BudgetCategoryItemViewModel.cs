using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
{
    public partial class BudgetCategoryItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private decimal _availbleFunds;

        [ObservableProperty]
        private decimal _monthlyStart;

        [ObservableProperty]
        private decimal _savingsGoal;

        [ObservableProperty]
        private DateTime? _goalCompletionDate;

    }
}
