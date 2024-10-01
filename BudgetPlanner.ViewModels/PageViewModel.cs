using BudgetPlanner.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.ViewModels
{
    public class PageViewModel : ViewModelBase
    {
        public void SetLoading(bool loading, string? loadingMessage = "")
        {
            WeakReferenceMessenger.Default.Send(new LoadingStateChangedMessage(loading));
            SetLoadingMessage(loadingMessage);
        }

        public void SetLoadingMessage(string? loadingMessage)
        {
            WeakReferenceMessenger.Default.Send(new LoadingMessageChangedMessage(loadingMessage));
        }
    }
}
