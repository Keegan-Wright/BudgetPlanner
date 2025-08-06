using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using BudgetPlanner.Client.Data.Db;
using BudgetPlanner.Client.Data.Models;
using BudgetPlanner.Shared.Models.Request.Auth;
using BudgetPlanner.Shared.Models.Response.Auth;
using BudgetPlanner.Shared.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Client.Services;

public abstract class BaseRequestService : InstrumentedService, IBaseRequestService
{
    private readonly IHttpClientFactory _httpClientFactory;
    protected readonly BudgetPlannerDbContext _budgetPlannerDbContext;
    public abstract string BaseRoute {get; init;}
    
    public BaseRequestService(IHttpClientFactory httpClientFactory, BudgetPlannerDbContext budgetPlannerDbContext)
    {
        _httpClientFactory = httpClientFactory;
        _budgetPlannerDbContext = budgetPlannerDbContext;
    }

    public async Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken = default)
    {
        var authState = await _budgetPlannerDbContext.AuthState.FirstOrDefaultAsync();
        using var client = CreateClient(authState?.AccessToken);
        
        var response = await client.GetAsync($"{BaseRoute}/{url}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await ConsumeRefreshToken(client, authState);
            response = await client.GetAsync($"{BaseRoute}/{url}", cancellationToken);
        }
        
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        
        return responseContent;
    }
    
    public async Task<TResponse> PostAsync<TRequest,TResponse>(string url, TRequest? requestBody, CancellationToken cancellationToken = default)
    {
        var authState = await _budgetPlannerDbContext.AuthState.FirstOrDefaultAsync();
        using var client = CreateClient(authState?.AccessToken);
        
        var response = await client.PostAsJsonAsync<TRequest>($"{BaseRoute}/{url}",requestBody, cancellationToken);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await ConsumeRefreshToken(client, authState);
            response = await client.PostAsJsonAsync<TRequest>($"{BaseRoute}/{url}",requestBody, cancellationToken);
        }
        
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        
        return responseContent;
    }

    public async Task PostAsync<TRequest>(string url, TRequest? requestBody, CancellationToken cancellationToken = default)
    {
        var authState = await _budgetPlannerDbContext.AuthState.FirstOrDefaultAsync();
        using var client = CreateClient(authState?.AccessToken);
        
        var response = await client.PostAsJsonAsync<TRequest>($"{BaseRoute}/{url}",requestBody, cancellationToken);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await ConsumeRefreshToken(client, authState);
            response = await client.PostAsJsonAsync<TRequest>($"{BaseRoute}/{url}",requestBody, cancellationToken);
        }
        
        response.EnsureSuccessStatusCode();
    }

    public async Task PostAsync(string url, CancellationToken cancellationToken = default)
    {
        var authState = await _budgetPlannerDbContext.AuthState.FirstOrDefaultAsync();
        using var client = CreateClient(authState?.AccessToken);
        
        var response = await client.PostAsync($"{BaseRoute}/{url}",null, cancellationToken);
        
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await ConsumeRefreshToken(client, authState);
            response = await client.PostAsync($"{BaseRoute}/{url}",null, cancellationToken);
        }
        
        response.EnsureSuccessStatusCode();
    }

    public async IAsyncEnumerable<TResponse> GetAsyncEnumerable<TResponse>(string url, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var authState = await _budgetPlannerDbContext.AuthState.FirstOrDefaultAsync();
        using var client = CreateClient(authState?.AccessToken);
        
        var response = await client.GetAsync($"{BaseRoute}/{url}", cancellationToken);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await ConsumeRefreshToken(client, authState);
            response = await client.GetAsync($"{BaseRoute}/{url}", cancellationToken);
        }
        
        response.EnsureSuccessStatusCode();
        var responseContent = response.Content.ReadFromJsonAsAsyncEnumerable<TResponse>(cancellationToken: cancellationToken);

        await foreach (var responseItem in responseContent)
        {
            yield return responseItem;
        }
    }

    public async IAsyncEnumerable<TResponse> PostAsyncEnumerableAsync<TRequest,TResponse>(string url, TRequest? requestBody,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var authState = await _budgetPlannerDbContext.AuthState.FirstOrDefaultAsync();
        using var client = CreateClient(authState?.AccessToken);
        
        var response = await client.PostAsJsonAsync($"{BaseRoute}/{url}",requestBody, cancellationToken);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await ConsumeRefreshToken(client, authState);
            response = await client.PostAsJsonAsync($"{BaseRoute}/{url}",requestBody, cancellationToken);
        }
        
        response.EnsureSuccessStatusCode();
        var responseContent = response.Content.ReadFromJsonAsAsyncEnumerable<TResponse>(cancellationToken: cancellationToken);

        await foreach (var responseItem in responseContent)
        {
            yield return responseItem;
        }
    }

    public async Task<TResponse> DeleteAsync<TResponse>(string url, CancellationToken cancellationToken = default)
    {
        var authState = await _budgetPlannerDbContext.AuthState.FirstOrDefaultAsync();
        using var client = CreateClient(authState?.AccessToken);
        
        var response = await client.DeleteAsync($"{BaseRoute}/{url}", cancellationToken);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await ConsumeRefreshToken(client, authState);
            response = await client.DeleteAsync($"{BaseRoute}/{url}", cancellationToken);
        }
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
    }
    
    private HttpClient CreateClient(string? authStateAccessToken, string? baseRouteOverride = null)
    {
        var client =  _httpClientFactory.CreateClient("apiClient");
        if(authStateAccessToken != null)
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authStateAccessToken}");
        
        return client;
    }
    
    private async Task ConsumeRefreshToken(HttpClient client, AuthState authState)
    {
        var authResponse = await client.PostAsJsonAsync($"auth/token",new TokenRequest(){ RefreshToken = authState.RefreshToken}, CancellationToken.None);
        authResponse.EnsureSuccessStatusCode();
        
        var rsp = await authResponse.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: CancellationToken.None);
        
        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {rsp.AccessToken}");
        
        authState.RefreshToken = rsp.RefreshToken.Value;
        authState.AccessToken = rsp.AccessToken;
        await _budgetPlannerDbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
    }

}