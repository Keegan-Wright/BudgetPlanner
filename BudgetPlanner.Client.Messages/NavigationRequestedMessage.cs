using BudgetPlanner.Client.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Client.Messages
{
    public class NavigationRequestedMessage : ValueChangedMessage<ViewModelBase>
    {
        public NavigationRequestedMessage(ViewModelBase viewModel) : base(viewModel)
        {
                
        }
    }
}
