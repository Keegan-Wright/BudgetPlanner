using BudgetPlanner.ViewModels;

namespace BudgetPlanner.Services
{
    public interface INavigationService
    {
        void RequestNavigation(ViewModelBase viewModel);
    }
}
