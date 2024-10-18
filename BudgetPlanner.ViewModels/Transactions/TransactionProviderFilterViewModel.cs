using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
{
    public partial class TransactionProviderFilterViewModel : ViewModelBase
    {
        [ObservableProperty]
        public Guid _providerId;

        [ObservableProperty]
        public string _providerName;

    }

    
}
