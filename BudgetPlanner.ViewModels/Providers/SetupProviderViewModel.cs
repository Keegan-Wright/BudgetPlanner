using BudgetPlanner.Data.Models;
using BudgetPlanner.Models.Request.OpenBanking;
using BudgetPlanner.Services.OpenBanking;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Drawing.Printing;

namespace BudgetPlanner.ViewModels
{
    public partial class SetupProviderViewModel : PageViewModel
    {
        private readonly IOpenBankingService _openBankingService;
        public SetupProviderViewModel(IOpenBankingService openBankingService)
        {
            _openBankingService = openBankingService;

            InitaliseAsync();
        }

        [ObservableProperty]
        private ObservableCollection<OpenBankingProviderViewModel> _openBankingProviders = [];

        [ObservableProperty]
        private string _openBankingAuthUrl;


        [ObservableProperty]
        private string _openBankingCode;



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
        }

        [RelayCommand]
        public void BuildAuthenticationUrl()
        {
            var selectedProviders = OpenBankingProviders.Where(x => x.Checked);

            var requestModel = new GetProviderSetupUrlRequestModel();
            requestModel.ProviderIds = selectedProviders.Select(x => x.ProviderId);
            requestModel.Scopes = selectedProviders.SelectMany(x => x.Scopes).Where(x => x.Checked).Select(x => x.Name).Distinct();

            OpenBankingAuthUrl = _openBankingService.BuildAuthUrl(requestModel);
        }


        [RelayCommand]
        public async Task AddProviderCommand()
        {
            SetLoading(true, "Adding Provider");
            await _openBankingService.AddVendorViaAccessCodeAsync(OpenBankingCode);
            SetLoading(false);
        }

        private void ResetSelections()
        {

            OpenBankingAuthUrl = string.Empty;
            OpenBankingCode = string.Empty;
            var providers = OpenBankingProviders.Where(x => x.Checked);
            foreach (var provider in providers)
            {
                provider.Checked = false;
            }
        }


    }
}
