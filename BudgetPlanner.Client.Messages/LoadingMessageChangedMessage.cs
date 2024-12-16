using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BudgetPlanner.Client.Messages
{
    public class LoadingMessageChangedMessage : ValueChangedMessage<string>
    {
        public LoadingMessageChangedMessage(string loadingMessage) : base(loadingMessage)
        {

        }
    }
}
