using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BudgetPlanner.Messages
{
    public class LoadingMessageChangedMessage : ValueChangedMessage<string>
    {
        public LoadingMessageChangedMessage(string loadingMessage) : base(loadingMessage)
        {

        }
    }
}
