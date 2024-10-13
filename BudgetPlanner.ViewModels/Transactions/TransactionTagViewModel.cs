using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
{
    public partial class TransactionTagViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _tag;
    }



}
