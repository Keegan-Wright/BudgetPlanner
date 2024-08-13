using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private bool _loading;

        [ObservableProperty]
        private string? _loadingMessage;


        public void SetLoading(bool loading, string? loadingMessage = "")
        {
            Loading = loading;
            LoadingMessage = loadingMessage;
        }
    }
}
