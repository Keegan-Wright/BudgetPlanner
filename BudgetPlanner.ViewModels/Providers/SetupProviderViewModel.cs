using BudgetPlanner.Data.Models;
using BudgetPlanner.Models.Request.OpenBanking;
using BudgetPlanner.Services.OpenBanking;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetPlanner.ViewModels
{
    public partial class SetupProviderViewModel : ViewModelBase
    {
        private readonly IOpenBankingService _openBankingService;
        public SetupProviderViewModel(IOpenBankingService openBankingService)
        {
            _openBankingService = openBankingService;

            InitaliseAsync();
        }

        [ObservableProperty]
        private ICollection<OpenBankingProviderViewModel> _openBankingProviders = [];


        private async void InitaliseAsync()
        {
            SetLoading(true, "Loading Providers...");

            await foreach (var provider in _openBankingService.GetOpenBankingProvidersForClientAsync()) 
            {
                var scopes = new Collection<OpenBankingProviderScopeViewModel>();

                await foreach (var scope in provider.Scopes) {

                    scopes.Add(new OpenBankingProviderScopeViewModel()
                    {
                        Checked = true, // Check all scopes by default
                        Name = scope
                    });
                }

                OpenBankingProviders.Add(new OpenBankingProviderViewModel()
                {
                    Name = provider.DisplayName,
                    Logo = provider.LogoUrl,
                    Scopes = scopes,
                    ProviderId = provider.ProviderId,
                    Checked = false // Don't enable
                });
            }

            SetLoading(false);

            OpenBankingProviders.FirstOrDefault(X => X.Name.ToLower().Contains("halifax")).Checked = true;
            OpenBankingProviders.FirstOrDefault(X => X.Name.ToLower().Contains("monzo")).Checked = true;
            BuildAuthenticationUrl();
        }

        [RelayCommand]
        public void BuildAuthenticationUrl()
        {
            var selectedProviders = OpenBankingProviders.Where(x => x.Checked);

            var requestModel = new GetProviderSetupUrlRequestModel();
            requestModel.ProviderIds = selectedProviders.Select(x => x.ProviderId);
            requestModel.Scopes = selectedProviders.SelectMany(x => x.Scopes).Where(x => x.Checked).Distinct().Select(x => x.Name);

            var url = _openBankingService.BuildAuthUrl(requestModel);
        }
    }
}
