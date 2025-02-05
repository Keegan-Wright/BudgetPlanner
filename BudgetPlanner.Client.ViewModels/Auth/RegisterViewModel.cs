using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Auth;
using BudgetPlanner.Shared.Models.Request.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;

namespace BudgetPlanner.Client.ViewModels;

public partial class RegisterViewModel : ValidateablePageViewModel<RegisterViewModel>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly INavigationService _navigationService;
    
    public RegisterViewModel(IAuthenticationService authenticationService, IValidator<RegisterViewModel> validator, INavigationService navigationService) : base(validator)
    {
        _authenticationService = authenticationService;
        _navigationService = navigationService;
    }
    
    [ObservableProperty]
    private string _username;
    
    [ObservableProperty]
    private string _email;
    
    [ObservableProperty]
    private string _firstName;
    
    [ObservableProperty]
    private string _lastName;
    
    [ObservableProperty]
    private string _password;
    
    [ObservableProperty]
    private string _confirmPassword;

    [RelayCommand]
    public async Task RegisterAsync()
    {
        await ValidateAndExecute(this, async () =>
        {
            SetLoading(true, "Registering");
            var registerResponse = await _authenticationService.RegisterAsync(new RegisterRequest()
            {
                Email = Email,
                FirstName = FirstName,
                LastName = _lastName,
                Password = Password,
                ConfirmPassword = ConfirmPassword,
                Username = Username
            });

            if (registerResponse.Success)
            {
                _navigationService.RequestNavigation<LoginViewModel>();
            }
            
            SetLoading(false);
        });
    }

    [RelayCommand]
    public void NavigateToLoginAsync()
    {
        _navigationService.RequestNavigation<LoginViewModel>();
    }
}