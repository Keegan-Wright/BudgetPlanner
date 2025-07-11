﻿using BudgetPlanner.Shared.Models.Request.OpenBanking;
using BudgetPlanner.Client.Services.OpenBanking;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class SetupProviderViewModel : PageViewModel
    {
        private readonly IOpenBankingRequestService _openBankingRequestService;
        public SetupProviderViewModel(IOpenBankingRequestService openBankingRequestService)
        {
            _openBankingRequestService = openBankingRequestService;

            InitialiseAsync();
        }

        [ObservableProperty]
        private ObservableCollection<OpenBankingProviderViewModel> _openBankingProviders = [];

        [ObservableProperty]
        private string _openBankingAuthUrl;


        [ObservableProperty]
        private string _openBankingCode;



        private async void InitialiseAsync()
        {
            SetLoading(true, "Loading Providers");

            await foreach (var provider in _openBankingRequestService.GetOpenBankingProvidersForClientAsync()) 
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
                    DisplayName = $"{provider.DisplayName} - {provider.Country}",
                    Logo = provider.LogoUrl,
                    Scopes = scopes,
                    ProviderId = provider.ProviderId,
                    Checked = false // Don't enable
                });
            }

            SetLoading(false);
        }

        [RelayCommand]
        public async void BuildAuthenticationUrl()
        {
            var selectedProviders = OpenBankingProviders.Where(x => x.Checked);

            var requestModel = new GetProviderSetupUrlRequestModel();
            requestModel.ProviderIds = selectedProviders.Select(x => x.ProviderId);
            requestModel.Scopes = selectedProviders.SelectMany(x => x.Scopes).Where(x => x.Checked).Select(x => x.Name).Distinct();

            OpenBankingAuthUrl = (await _openBankingRequestService.BuildAuthUrl(requestModel)).AuthUrl;
        }


        [RelayCommand]
        public async Task AddProviderCommand()
        {
            SetLoading(true, "Adding Provider");
            var requestModel = new AddVendorRequestModel()
            {
                AccessCode = OpenBankingCode
            };
            await _openBankingRequestService.AddVendorViaAccessCodeAsync(requestModel);
            ResetSelections();
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
