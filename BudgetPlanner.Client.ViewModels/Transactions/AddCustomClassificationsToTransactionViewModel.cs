using Avalonia.Threading;
using BudgetPlanner.Client.Handlers;
using BudgetPlanner.Shared.Models.Request.Classifications;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Classifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class AddCustomClassificationsToTransactionViewModel : ValidateablePageViewModel<AddCustomClassificationsToTransactionViewModel>
    {
        private readonly IClassificationsRequestService _classificationsRequestService;
        private readonly INavigationService _navigationService;

        public AddCustomClassificationsToTransactionViewModel(IClassificationsRequestService classificationsRequestService, INavigationService navigationService, IValidator<AddCustomClassificationsToTransactionViewModel> validator) : base(validator)
        {
            _classificationsRequestService = classificationsRequestService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task InitialiseAsync()
        {
            await RunOnBackgroundThreadAsync(LoadClassificationsAsync());
        }

        [ObservableProperty]
        private ObservableCollection<CustomClassificationSelectionItemViewModel> _customClassifications = [];

        private async Task LoadClassificationsAsync()
        {
            
            SetLoading(true, "Loading Classifications");
            await foreach (var classification in _classificationsRequestService.GetAllCustomClassificationsAsync())
            {
                Dispatcher.UIThread.Invoke(() => CustomClassifications.Add(new CustomClassificationSelectionItemViewModel()
                {
                    ClassificationId = classification.ClassificationId,
                    Checked = false,
                    ClassificationName = classification.Tag
                }));


            }
            SetLoading(false);
        }


        [RelayCommand]
        private void ReturnToTransactions()
        {
            NavigateBackToTransactions();
        }

        [RelayCommand]
        private async Task AddCustomTagsToTransaction()
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

                        await _classificationsRequestService.AddCustomClassificationsToTransactionAsync(requestModel);
                    });
                    SetLoading(false);
                    NavigateBackToTransactions();
                });

            }
            catch (Exception? ex)
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
