using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentValidation;

namespace BudgetPlanner.Client.ViewModels;

public partial class ValidateablePageViewModel<TValidationModel> : PageViewModel
{
    private protected readonly IValidator<TValidationModel> _validator;
    public ValidateablePageViewModel(IValidator<TValidationModel> validator)
    {
        _validator = validator;
    }

    [ObservableProperty]
    public ObservableCollection<string> _errors = [];

    [ObservableProperty]
    public bool _hasErrors;

    public async Task ValidateAndExecute(TValidationModel  model, Action action)
    {
        var validationResult = await _validator.ValidateAsync(model);
            
        if (validationResult.IsValid)
        {
            HasErrors = false;
            Errors.Clear();

            action.Invoke();
        }
        else
        {
            Errors.Clear();
            foreach (var error in validationResult.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
            HasErrors = true;
        }
    }
}