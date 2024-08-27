using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
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
        private string _name;

        [ObservableProperty]
        private bool _checked;
    }
}
