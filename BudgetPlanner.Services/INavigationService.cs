using BudgetPlanner.ViewModels;

namespace BudgetPlanner.Services
{
    public interface INavigationService
    {
        void RequestNavigation<TViewModel>(object? navigationData = null) where TViewModel : ViewModelBase;
    }
}
