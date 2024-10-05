using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
{
    public partial class TransactionCategoryFilterViewModel : ViewModelBase
    {
        [ObservableProperty]
        public string _transactionCategory;

    }

    
}
