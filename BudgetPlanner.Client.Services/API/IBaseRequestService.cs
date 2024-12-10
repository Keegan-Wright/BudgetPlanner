namespace BudgetPlanner.Client.Services;

public interface IBaseRequestService
{
    Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken = default);
    Task<TResponse> PostAsync<TResponse,TRequest>(string url, TRequest? requestBody, CancellationToken cancellationToken = default);
    
    IAsyncEnumerable<TResponse> GetAsyncEnumerable<TResponse>(string url, CancellationToken cancellationToken = default);
    IAsyncEnumerable<TResponse> PostAsyncEnumerableAsync<TResponse,TRequest>(string url, TRequest? requestBody, CancellationToken cancellationToken = default);
    
    
}