using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class TransactionTagFilterViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _tag;
    }



}
