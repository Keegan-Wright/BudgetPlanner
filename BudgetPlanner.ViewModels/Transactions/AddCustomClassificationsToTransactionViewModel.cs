using Avalonia.Threading;
using BudgetPlanner.Handlers;
using BudgetPlanner.Models.Request.Classifications;
using BudgetPlanner.Services;
using BudgetPlanner.Services.Classifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace BudgetPlanner.ViewModels
{
    public partial class AddCustomClassificationsToTransactionViewModel : ValidateablePageViewModel<AddCustomClassificationsToTransactionViewModel>
    {
        private readonly IClassificationService _classificationService;
        private readonly INavigationService _navigationService;

        public AddCustomClassificationsToTransactionViewModel(IClassificationService classificationService, INavigationService navigationService, IValidator<AddCustomClassificationsToTransactionViewModel> validator) : base(validator)
        {
            _classificationService = classificationService;
            _navigationService = navigationService;

            InitaliseAsync();
        }

        private async void InitaliseAsync()
        {
            SetLoading(true, "Loading Classifications");

            await RunOnBackgroundThreadAsync(async () => await LoadClassificationsAsync());

            SetLoading(false);
        }

        [ObservableProperty]
        private ObservableCollection<CustomClassificationSelectionItemViewModel> _customClassifications = [];

        private async Task LoadClassificationsAsync()
        {
            await foreach (var classification in _classificationService.GetAllCustomClassificationsAsync())
            {
                Dispatcher.UIThread.Invoke(() => CustomClassifications.Add(new CustomClassificationSelectionItemViewModel()
                {
                    ClassificationId = classification.ClassificationId,
                    Checked = false,
                    ClassificationName = classification.Tag
                }));


            }
        }


        [RelayCommand]
        public void ReturnToTransactions()
        {
            NavigateBackToTransactions();
        }

        [RelayCommand]
        public async Task AddCustomTagsToTransaction()
        {
            try
            {
                await ValidateAndExecute(this, async () =>
                {
                    SetLoading(true, "Adding Custom Classifications");
                    await RunOnBackgroundThreadAsync(async () =>
                    {
                        var selectedClassifications = CustomClassifications.Where(x => x.Checked).Select(x => new SelectedCustomClassificationsRequest() { ClassificationId = x.ClassificationId });
                        var selectedTransaction = NavigationData as TransactionItemViewModel;

                        var requestModel = new AddCustomClassificationsToTransactionRequest() { TransactionId = selectedTransaction.TransactionId, Classifications = selectedClassifications };

                        await _classificationService.AddCustomClassificationsToTransactionAsync(requestModel);
                    });
                    SetLoading(false);
                    NavigateBackToTransactions();
                });

            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }

        private void NavigateBackToTransactions()
        {
            _navigationService.RequestNavigation<TransactionsViewModel>();
        }
    }

}
