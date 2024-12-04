using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class TransactionAccountFilterViewModel : ViewModelBase
    {
        [ObservableProperty]
        public Guid _accountId;

        [ObservableProperty]
        public string _accountName;

    }

    
}
