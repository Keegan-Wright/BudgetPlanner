using BudgetPlanner.Messages;
using BudgetPlanner.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.Services
{
    public class NavigationService : INavigationService
    {
        public void RequestNavigation(ViewModelBase viewModel)
        {
            WeakReferenceMessenger.Default.Send(new NavigationRequestedMessage(viewModel));
        }
    }
}
