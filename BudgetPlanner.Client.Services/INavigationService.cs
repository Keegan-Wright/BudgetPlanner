using BudgetPlanner.Client.ViewModels;

namespace BudgetPlanner.Client.Services
{
    public interface INavigationService
    {
        void RequestNavigation<TViewModel>(object? navigationData = null) where TViewModel : ViewModelBase;
    }
}
