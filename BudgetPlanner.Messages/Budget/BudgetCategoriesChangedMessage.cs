using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetPlanner.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BudgetPlanner.Messages.Budget
{
    public class BudgetCategoriesChangedMessage : ValueChangedMessage<bool>
    {
        public BudgetCategoriesChangedMessage(bool shouldReload) : base(shouldReload)
        {

        }
    }

}
