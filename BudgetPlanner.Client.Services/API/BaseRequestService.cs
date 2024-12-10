using System.Net.Http.Json;
using BudgetPlanner.Shared.Services.Base;

namespace BudgetPlanner.Client.Services;

public abstract class BaseRequestService : InstrumentedService, IBaseRequestService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public abstract string BaseRoute {get; init;}
    private string apiEntryPoint = "/api/";
    
    public BaseRequestService(IHttpClientFactory httpClientFactory)
    {

    }

    public async Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken = default)
    {
        using var client = _httpClientFactory.CreateClient("apiClient");
        
        var response = await client.GetAsync($"{apiEntryPoint}{BaseRoute}{url}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        
        return responseContent;
    }

    public async Task<TResponse> PostAsync<TResponse, TRequest>(string url, TRequest? requestBody, CancellationToken cancellationToken = default)
    {
        using var client = _httpClientFactory.CreateClient("apiClient");
        var response = await client.PostAsJsonAsync<TRequest>($"{apiEntryPoint}{BaseRoute}{url}",requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        
        return responseContent;
    }

    public async IAsyncEnumerable<TResponse> GetAsyncEnumerable<TResponse>(string url, CancellationToken cancellationToken = default)
    {
        using var client = _httpClientFactory.CreateClient("apiClient");
        var response = await client.GetAsync($"{apiEntryPoint}{BaseRoute}{url}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseContent = response.Content.ReadFromJsonAsAsyncEnumerable<TResponse>(cancellationToken: cancellationToken);

        await foreach (var responseItem in responseContent)
        {
            yield return responseItem;
        }
    }

    public async IAsyncEnumerable<TResponse> PostAsyncEnumerableAsync<TResponse, TRequest>(string url, TRequest? requestBody,
        CancellationToken cancellationToken = default)
    {
        using var client = _httpClientFactory.CreateClient("apiClient");
        var response = await client.PostAsJsonAsync($"{apiEntryPoint}{BaseRoute}{url}",requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseContent = response.Content.ReadFromJsonAsAsyncEnumerable<TResponse>(cancellationToken: cancellationToken);

        await foreach (var responseItem in responseContent)
        {
            yield return responseItem;
        }
    }
}