using Avalonia.Threading;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Classifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class ClassificationSettingsViewModel : PageViewModel
    {
        private readonly IClassificationsRequestService _classificationsRequestService;
        private readonly INavigationService _navigationService;

        public ClassificationSettingsViewModel(IClassificationsRequestService classificationsRequestService, INavigationService navigationService)
        {
            _classificationsRequestService = classificationsRequestService;
            _navigationService = navigationService;

            InitialiseAsync();
        }

        [ObservableProperty]
        private ObservableCollection<ClassificationItemViewModel> _customClassifications = [];


        [RelayCommand]
        public void NavigateToAddCustomClassification()
        {
            _navigationService.RequestNavigation<AddCustomClassificationViewModel>();
        }

        [RelayCommand]
        public async Task DeleteCustomClassificationAsync(ClassificationItemViewModel item)
        {
            SetLoading(true, "Deleting Custom Classification");
            await _classificationsRequestService.RemoveCustomClassificationAsync(item.ClassificationId);
            
            await RunOnBackgroundThreadAsync(async () => await LoadDataAsync());
        }
        private async void InitialiseAsync()
        {
            await RunOnBackgroundThreadAsync(async () => await LoadDataAsync());
        }

        private async Task LoadDataAsync()
        {
            SetLoading(true, "Loading Custom Classifications");

            Dispatcher.UIThread.Invoke(() => CustomClassifications.Clear());
            await foreach (var item in _classificationsRequestService.GetAllCustomClassificationsAsync())
            {
                Dispatcher.UIThread.Invoke(() => CustomClassifications.Add(new() { Classification = item.Tag, ClassificationId = item.ClassificationId }));
            }
            SetLoading(false);
        }

        
    }
}
