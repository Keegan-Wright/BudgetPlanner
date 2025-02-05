using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Client.ViewModels.Validators
{
    public class AddCustomClassificationsToTransactionViewModelValidator : AbstractValidator<AddCustomClassificationsToTransactionViewModel>
    {
        public AddCustomClassificationsToTransactionViewModelValidator()
        {
            RuleFor(x => x.CustomClassifications.Where(x => x.Checked).Count()).GreaterThan(0).WithMessage("At least one classification is required");
            RuleFor(x => x.NavigationData).NotNull().WithMessage("A transaction must be selected");
        }
    }

}
