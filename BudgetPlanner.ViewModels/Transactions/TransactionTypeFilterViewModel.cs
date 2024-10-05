using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
{
    public partial class TransactionTypeFilterViewModel : ViewModelBase
    {
        [ObservableProperty]
        public string _transactionType;

    }

    
}
