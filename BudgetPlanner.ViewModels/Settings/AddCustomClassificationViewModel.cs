using BudgetPlanner.Handlers;
using BudgetPlanner.Models.Request.Classifications;
using BudgetPlanner.Services;
using BudgetPlanner.Services.Classifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.ViewModels
{
    public partial class AddCustomClassificationViewModel : PageViewModel
    {
        private readonly IClassificationService _classificationService;
        private readonly INavigationService _navigationService;

        public AddCustomClassificationViewModel(IClassificationService classificationService, INavigationService navigationService)
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
                SetLoading(true, "Adding Custom Classification");
                await _classificationService.AddCustomClassificationAsync(new AddClassificationsRequest() { Tag = CustomTag });
                SetLoading(false);
                NavigateBackToCustomClassificationSettings();

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
