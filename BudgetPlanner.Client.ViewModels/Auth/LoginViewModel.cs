using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Auth;
using BudgetPlanner.Shared.Models.Request.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;

namespace BudgetPlanner.Client.ViewModels;

public partial class LoginViewModel : ValidateablePageViewModel<LoginViewModel>
{
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;
    
    [ObservableProperty]
    private string _username;
    
    [ObservableProperty]
    private string _password;

    public LoginViewModel(IAuthenticationService authenticationService, INavigationService navigationService, IValidator<LoginViewModel> validator) : base(validator)
    {
        _authenticationService = authenticationService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    public async Task LoginAsync()
    {
        await ValidateAndExecute(this, async () =>
        {
            SetLoading(true, "Logging in");
            var loginResponse = await _authenticationService.LoginAsync(new LoginRequest()
            {
                Password = Password,
                Username = Username
            });

            if (loginResponse.Success)
            {
                _navigationService.RequestNavigation<DashboardViewModel>();
            }
            
            SetLoading(false);
        });
    }

    [RelayCommand]
    public void NavigateToRegisterAsync()
    {
        _navigationService.RequestNavigation<RegisterViewModel>();
    }

}