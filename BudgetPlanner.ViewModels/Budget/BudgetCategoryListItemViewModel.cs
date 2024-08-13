using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.ViewModels
{
    public partial class BudgetCategoryListItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private decimal _availableFunds;

        [ObservableProperty]
        private decimal _monthlyStart;

        [ObservableProperty]
        private decimal _savingsGoal;

        [ObservableProperty]
        private DateTime? _goalCompletionDate;
    }
}
