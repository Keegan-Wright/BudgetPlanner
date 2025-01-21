using FluentValidation;

namespace BudgetPlanner.Client.ViewModels.Validators;

public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterViewModelValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm password is required");
        RuleFor(x => x.ConfirmPassword).Matches(x => x.Password).WithMessage("Passwords do not match");
    }
}