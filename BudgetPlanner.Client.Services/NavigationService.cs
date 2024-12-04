using BudgetPlanner.Client.Messages;
using BudgetPlanner.Client.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.Client.Services
{
    public class NavigationService : INavigationService
    {
        public void RequestNavigation<TViewModel>(object? navigationData = null)
            where TViewModel : ViewModelBase
        {
            var viewModel = Ioc.Default.GetRequiredService<TViewModel>();
            viewModel.NavigationData = navigationData;
            WeakReferenceMessenger.Default.Send(new NavigationRequestedMessage(viewModel));
        }
    }
}
