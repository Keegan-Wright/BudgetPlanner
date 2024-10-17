using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
{
    public partial class TransactionTagFilterViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _tag;
    }



}
