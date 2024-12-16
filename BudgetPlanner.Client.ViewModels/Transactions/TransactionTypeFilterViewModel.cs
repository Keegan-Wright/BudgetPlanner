using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class TransactionTypeFilterViewModel : ViewModelBase
    {
        [ObservableProperty]
        public string _transactionType;

    }

    
}
