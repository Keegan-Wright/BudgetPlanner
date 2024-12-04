using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class OpenBankingProviderScopeViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _name;


        [ObservableProperty]
        private bool _checked;

    }
}
