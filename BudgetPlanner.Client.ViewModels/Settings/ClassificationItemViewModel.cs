using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class ClassificationItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Guid _classificationId;
        
        [ObservableProperty]
        private string? _classification;

        
    }
}
