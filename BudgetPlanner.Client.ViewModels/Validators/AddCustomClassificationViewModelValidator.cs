using BudgetPlanner.Client.Data.Db;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Client.ViewModels.Validators
{
    public class AddCustomClassificationViewModelValidator : AbstractValidator<AddCustomClassificationViewModel>
    {
        public AddCustomClassificationViewModelValidator(BudgetPlannerDbContext budgetPlannerDbContext)
        {
            
            RuleFor(x => x.CustomTag).NotNull().NotEmpty().WithMessage("Classification is required");

            RuleFor(x => x.CustomTag).MustAsync(async (tag, cancellation) =>
            {
                var existingTag = await budgetPlannerDbContext.CustomClassifications.FirstOrDefaultAsync(x => x.Tag.ToLower() == tag.ToLower());
                return existingTag == null;
            }).WithMessage("Classification must be unique");
        }
    }

}
