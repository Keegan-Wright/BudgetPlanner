using BudgetPlanner.Services.Classifications;
using CommunityToolkit.Mvvm.ComponentModel;
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

        public ClassificationSettingsViewModel(IClassificationService classificationService)
        {
            _classificationService = classificationService;

            InitaliseAsync();
        }

        [ObservableProperty]
        private ObservableCollection<ClassificationItemViewModel> _customClassifications = [];

        private async void InitaliseAsync()
        {
            await RunOnBackgroundThreadAsync(async () => await LoadDataAsync());
        }

        private async Task LoadDataAsync()
        {
            await foreach (var item in _classificationService.GetAllCustomClassificationsAsync())
            {

            }
        }
    }

    public partial class ClassificationItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _classification;

        [ObservableProperty]
        private bool _isCustomClassification;
        
    }
}
