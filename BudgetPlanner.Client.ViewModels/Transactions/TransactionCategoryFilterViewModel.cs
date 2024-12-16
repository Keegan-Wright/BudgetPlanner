using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class TransactionCategoryFilterViewModel : ViewModelBase
    {
        [ObservableProperty]
        public string _transactionCategory;

    }

    
}
