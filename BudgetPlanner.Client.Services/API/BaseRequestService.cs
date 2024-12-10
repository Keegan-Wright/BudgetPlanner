using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using BudgetPlanner.Shared.Services.Base;

namespace BudgetPlanner.Client.Services;

public abstract class BaseRequestService : InstrumentedService, IBaseRequestService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public abstract string BaseRoute {get; init;}
    
    public BaseRequestService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken = default)
    {
        using var client = _httpClientFactory.CreateClient("apiClient");
        
        var response = await client.GetAsync($"{BaseRoute}/{url}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        
        return responseContent;
    }

    public async Task<TResponse> PostAsync<TRequest,TResponse>(string url, TRequest? requestBody, CancellationToken cancellationToken = default)
    {
        using var client = _httpClientFactory.CreateClient("apiClient");
        var response = await client.PostAsJsonAsync<TRequest>($"{BaseRoute}/{url}",requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        
        return responseContent;
    }

    public async IAsyncEnumerable<TResponse> GetAsyncEnumerable<TResponse>(string url, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var client = _httpClientFactory.CreateClient("apiClient");
        var response = await client.GetAsync($"{BaseRoute}/{url}", cancellationToken);
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
        using var client = _httpClientFactory.CreateClient("apiClient");
        var response = await client.PostAsJsonAsync($"{BaseRoute}/{url}",requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseContent = response.Content.ReadFromJsonAsAsyncEnumerable<TResponse>(cancellationToken: cancellationToken);

        await foreach (var responseItem in responseContent)
        {
            yield return responseItem;
        }
    }
}