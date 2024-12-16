using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BudgetPlanner.Client.Messages
{
    public class ThemeChangeRequestedMessage : ValueChangedMessage<bool>
    {
        public ThemeChangeRequestedMessage(bool value) : base(value)
        {
        }
    }
}
