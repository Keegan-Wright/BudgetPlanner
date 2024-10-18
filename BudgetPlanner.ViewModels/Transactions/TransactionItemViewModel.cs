using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
{
    public partial class TransactionItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Guid _transactionId;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private string _transactionType;

        [ObservableProperty]
        private string _transactionCategory;

        [ObservableProperty]
        private decimal _amount;

        [ObservableProperty]
        private string _currency;

        [ObservableProperty]
        private DateOnly _transactionDate;

        [ObservableProperty]
        private bool _pending;

        [ObservableProperty]
        private IEnumerable<TransactionTagFilterViewModel> _tags;

    }



}
