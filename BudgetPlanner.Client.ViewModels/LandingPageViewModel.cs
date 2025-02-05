using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Auth;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.Client.ViewModels;

public partial class LandingPageViewModel : PageViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;
    public LandingPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
    {
        _navigationService = navigationService;
        _authenticationService = authenticationService;
    }
    
    [RelayCommand]
    private async Task DetermineStartPage()
    {
        SetLoading(true, "Determining your path");

        var hasAuthenticated = await _authenticationService.HasAuthenticated();
        SetLoading(false);
        if (hasAuthenticated)
        {
            _navigationService.RequestNavigation<DashboardViewModel>();
        }
        else
        {
            _navigationService.RequestNavigation<LoginViewModel>();
        }
    }
}