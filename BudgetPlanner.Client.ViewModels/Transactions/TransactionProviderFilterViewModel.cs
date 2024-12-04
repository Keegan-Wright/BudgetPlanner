using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class TransactionProviderFilterViewModel : ViewModelBase
    {
        [ObservableProperty]
        public Guid _providerId;

        [ObservableProperty]
        public string _providerName;

    }

    
}
