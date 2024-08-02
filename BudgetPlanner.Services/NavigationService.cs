using BudgetPlanner.Messages;
using BudgetPlanner.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.Services
{
    public class NavigationService : INavigationService
    {
        public void RequestNavigation<TViewModel>()
            where TViewModel : ViewModelBase
        {
            var viewModel = Ioc.Default.GetRequiredService<TViewModel>();
            WeakReferenceMessenger.Default.Send(new NavigationRequestedMessage(viewModel));
        }
    }
}
