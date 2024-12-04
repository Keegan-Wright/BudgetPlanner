using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BudgetPlanner.Client.Messages
{
    public class LoadingStateChangedMessage : ValueChangedMessage<bool>
    {
        public LoadingStateChangedMessage(bool loadingState) : base(loadingState)
        {

        }
    }
}
