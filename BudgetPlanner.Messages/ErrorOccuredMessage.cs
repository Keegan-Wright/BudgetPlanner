using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BudgetPlanner.Messages
{
    public class ErrorOccuredMessage : ValueChangedMessage<bool>
    {
        public ErrorOccuredMessage(bool hasError) : base(hasError)
        {

        }
    }
}
