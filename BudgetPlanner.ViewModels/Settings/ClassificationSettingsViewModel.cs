using Avalonia.Threading;
using BudgetPlanner.Services;
using BudgetPlanner.Services.Classifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.ViewModels
{
    public partial class ClassificationSettingsViewModel : PageViewModel
    {
        private readonly IClassificationService _classificationService;
        private readonly INavigationService _navigationService;

        public ClassificationSettingsViewModel(IClassificationService classificationService, INavigationService navigationService)
        {
            _classificationService = classificationService;
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
            await _classificationService.RemoveCustomClassificationAsync(item.ClassificationId);
            
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
            await foreach (var item in _classificationService.GetAllCustomClassificationsAsync())
            {
                Dispatcher.UIThread.Invoke(() => CustomClassifications.Add(new() { Classification = item.Tag, ClassificationId = item.ClassificationId }));
            }
            SetLoading(false);
        }

        
    }
}
