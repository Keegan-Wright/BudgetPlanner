namespace BudgetPlanner.Client.Services;

public interface IBaseRequestService
{
    Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken = default);
    Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest? requestBody, CancellationToken cancellationToken = default);
    Task PostAsync<TRequest>(string url, TRequest? requestBody, CancellationToken cancellationToken = default);
    
    IAsyncEnumerable<TResponse> GetAsyncEnumerable<TResponse>(string url, CancellationToken cancellationToken = default);
    IAsyncEnumerable<TResponse> PostAsyncEnumerableAsync<TRequest,TResponse>(string url, TRequest? requestBody, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string url, CancellationToken cancellationToken = default);


}