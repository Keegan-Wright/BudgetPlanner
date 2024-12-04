using BudgetPlanner.Client.Handlers;
using BudgetPlanner.Shared.Models.Request.Classifications;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Classifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class AddCustomClassificationViewModel : ValidateablePageViewModel<AddCustomClassificationViewModel>
    {
        private readonly IClassificationService _classificationService;
        private readonly INavigationService _navigationService;

        public AddCustomClassificationViewModel(IClassificationService classificationService, INavigationService navigationService, IValidator<AddCustomClassificationViewModel> validator) : base(validator)
        {
            _classificationService = classificationService;
            _navigationService = navigationService;
        }

        [ObservableProperty]
        public string _customTag;


        [RelayCommand]
        public void ReturnToCustomClassificationSettings()
        {
            NavigateBackToCustomClassificationSettings();
        }

        [RelayCommand]
        public async Task AddCustomClassification()
        {
            try
            {
                await ValidateAndExecute(this, async () =>
                {
                    SetLoading(true, "Adding Custom Classification");
                    await _classificationService.AddCustomClassificationAsync(new AddClassificationsRequest() { Tag = CustomTag });
                    SetLoading(false);
                    NavigateBackToCustomClassificationSettings();

                });
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }

        private void NavigateBackToCustomClassificationSettings()
        {
            _navigationService.RequestNavigation<ClassificationSettingsViewModel>();
        }
    }
}
