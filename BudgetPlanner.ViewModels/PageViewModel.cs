using BudgetPlanner.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FluentValidation;
using System.Collections.ObjectModel;

namespace BudgetPlanner.ViewModels
{
    public class PageViewModel : ViewModelBase
    {
        public void SetLoading(bool loading, string? loadingMessage = "")
        {
            WeakReferenceMessenger.Default.Send(new LoadingStateChangedMessage(loading));
            SetLoadingMessage(loadingMessage);
        }

        public void SetLoadingMessage(string? loadingMessage)
        {
            WeakReferenceMessenger.Default.Send(new LoadingMessageChangedMessage(loadingMessage));
        }
    }

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
}
