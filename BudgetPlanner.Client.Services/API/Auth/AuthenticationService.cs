using BudgetPlanner.Shared.Models.Request.Auth;
using BudgetPlanner.Shared.Models.Response;

namespace BudgetPlanner.Client.Services.Auth;

public class AuthenticationService : BaseRequestService, IAuthenticationService
{
    public AuthenticationService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        BaseRoute = "auth";
    }
    
    public sealed override string BaseRoute { get; init; }
    
    public async Task<GenericSuccessResponse> RegisterAsync(RegisterRequest request)
    {
        var response = await PostAsync<RegisterRequest, GenericSuccessResponse>("Register", request);
        return response;
    }

    public async Task<GenericSuccessResponse> LoginAsync(LoginRequest request)
    {
        var response = await PostAsync<LoginRequest, GenericSuccessResponse>("Login", request);
        return response;
    }
}