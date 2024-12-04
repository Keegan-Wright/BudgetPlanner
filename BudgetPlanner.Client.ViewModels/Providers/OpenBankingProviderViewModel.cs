using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class OpenBankingProviderViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _providerId;

        [ObservableProperty]
        private ICollection<OpenBankingProviderScopeViewModel> _scopes;

        [ObservableProperty]
        private string _logo;

        [ObservableProperty]
        private string _displayName;
        
        [ObservableProperty]
        private bool _checked;
    }
}
