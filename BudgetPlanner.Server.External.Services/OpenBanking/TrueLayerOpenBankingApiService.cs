
using BudgetPlanner.Server.Models.Configuration;
using BudgetPlanner.Server.External.Services.Models.OpenBanking;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using BudgetPlanner.Server.External.Services.Models.OpenBanking;
using Sentry;

namespace BudgetPlanner.Server.External.Services.OpenBanking
{
    public class TrueLayerOpenBankingApiService : IOpenBankingApiService
    {
        private readonly TrueLayerOpenBankingConfiguration _trueLayerConfiguration;

        public TrueLayerOpenBankingApiService(TrueLayerOpenBankingConfiguration trueLayerConfiguration)
        {
            _trueLayerConfiguration = trueLayerConfiguration;
        }

        public string BuildAuthUrl(IEnumerable<string> providerIds, IEnumerable<string> scopes)
        {
            var sb = new StringBuilder();

            sb.Append(_trueLayerConfiguration.BaseAuthUrl);
            sb.Append($"?response_type=code&client_id={_trueLayerConfiguration.ClientId}");
            sb.Append("&");
            sb.Append($"scope={Uri.EscapeDataString(string.Join(" ", scopes))}");
            sb.Append(Uri.EscapeDataString(" "));


            sb.Append($"&redirect_uri={_trueLayerConfiguration.AuthRedirectUrl}");
            sb.Append($"&providers={Uri.EscapeDataString(string.Join(" ", providerIds))}");

            sb.Append(Uri.EscapeDataString(" "));

            sb.Append("uk-oauth-all");

            return sb.ToString();
        }

        public async Task<ExternalOpenBankingAccessResponseModel> ExchangeCodeForAccessTokenAsync(string vendorAccessCode)
        {
            using var httpClient = await BuildHttpClient(_trueLayerConfiguration.BaseAuthUrl);

            var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", _trueLayerConfiguration.ClientId),
                new KeyValuePair<string, string>("client_secret", _trueLayerConfiguration.ClientSecret.ToString()),
                new KeyValuePair<string, string>("redirect_uri", _trueLayerConfiguration.AuthRedirectUrl),
                new KeyValuePair<string, string>("code", vendorAccessCode)
            };

            HttpContent content = new FormUrlEncodedContent(formData);

            var response = await httpClient.PostAsync("connect/token", content);

            var responseBody = await response.Content.ReadFromJsonAsync<ExternalOpenBankingAccessResponseModel>();

            return responseBody;
        }

        public async Task<ExternalOpenBankingAccessResponseModel> GetAccessTokenByRefreshTokenAsync(string refreshToken)
        {
            using var httpClient = await BuildHttpClient(_trueLayerConfiguration.BaseAuthUrl);

            var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_id", _trueLayerConfiguration.ClientId),
                new KeyValuePair<string, string>("client_secret", _trueLayerConfiguration.ClientSecret.ToString()),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
            };

            HttpContent content = new FormUrlEncodedContent(formData);

            var response = await httpClient.PostAsync("connect/token", content);

            var responseBody = await response.Content.ReadFromJsonAsync<ExternalOpenBankingAccessResponseModel>();

            return responseBody;

        }

        public async Task<ExternalOpenBankingGetAccountBalanceResponseModel> GetAccountBalanceAsync(string accountId, string authToken)
        {
            using var httpClient = await BuildHttpClient(_trueLayerConfiguration.BaseDataUrl, authToken);
            var response = await httpClient.GetAsync($"v1/accounts/{accountId}/balance");

            var responseBody = await response.Content.ReadFromJsonAsync<ExternalOpenBankingGetAccountBalanceResponseModel>();

            return responseBody;

        }

        public async Task<ExternalOpenBankingAccountDirectDebitsResponseModel> GetAccountDirectDebitsAsync(string accountId, string authToken)
        {
            using var httpClient = await BuildHttpClient(_trueLayerConfiguration.BaseDataUrl, authToken);

            var response = await httpClient.GetAsync($"v1/accounts/{accountId}/direct_debits");

            var responseBody = await response.Content.ReadFromJsonAsync<ExternalOpenBankingAccountDirectDebitsResponseModel>();

            return responseBody;
        }

        public async Task<ExternalOpenBankingAccountTransactionsResponseModel> GetAccountPendingTransactionsAsync(string accountId, string authToken, DateTime? transactionsStartingDate = null)
        {
            using var httpClient = await BuildHttpClient(_trueLayerConfiguration.BaseDataUrl, authToken);

            var urlBuilder = new StringBuilder();
            urlBuilder.Append($"v1/accounts/{accountId}/transactions/pending");

            if (transactionsStartingDate.HasValue)
            {
                urlBuilder.Append($"?from={transactionsStartingDate:yyyy-MM-dd}&to={DateTime.Now:yyyy-MM-dd}");
            }

            var response = await httpClient.GetAsync(urlBuilder.ToString());

            var responseBody = await response.Content.ReadFromJsonAsync<ExternalOpenBankingAccountTransactionsResponseModel>();

            return responseBody;

        }

        public async Task<ExternalOpenBankingAccountStandingOrdersResponseModel> GetAccountStandingOrdersAsync(string accountId, string authToken)
        {
            using var httpClient = await BuildHttpClient(_trueLayerConfiguration.BaseDataUrl, authToken);

            var response = await httpClient.GetAsync($"v1/accounts/{accountId}/standing_orders");

            var responseBody = await response.Content.ReadFromJsonAsync<ExternalOpenBankingAccountStandingOrdersResponseModel>();

            return responseBody;
        }

        public async Task<ExternalOpenBankingAccountTransactionsResponseModel> GetAccountTransactionsAsync(string accountId, string authToken, DateTime? transactionsStartingDate = null)
        {
            using var httpClient = await BuildHttpClient(_trueLayerConfiguration.BaseDataUrl, authToken);

            var urlBuilder = new StringBuilder();
            urlBuilder.Append($"v1/accounts/{accountId}/transactions");

            if (transactionsStartingDate.HasValue)
            {
                urlBuilder.Append($"?from={transactionsStartingDate:yyyy-MM-dd}&to={DateTime.Now:yyyy-MM-dd}");
            }

            var response = await httpClient.GetAsync(urlBuilder.ToString());

            var responseBody = await response.Content.ReadFromJsonAsync<ExternalOpenBankingAccountTransactionsResponseModel>();

            return responseBody;

        }

        public async Task<ExternalOpenBankingListAllAccountsResponseModel> GetAllAccountsAsync(string authToken)
        {
            using var httpClient = await BuildHttpClient(_trueLayerConfiguration.BaseDataUrl, authToken);

            var response = await httpClient.GetAsync("v1/accounts");

            var responseBody = await response.Content.ReadFromJsonAsync<ExternalOpenBankingListAllAccountsResponseModel>();

            return responseBody;
        }


        public async IAsyncEnumerable<ExternalOpenBankingProvider> GetAvailableProvidersAsync()
        {
            using var httpClient = await BuildHttpClient(_trueLayerConfiguration.BaseAuthUrl);

            var response = await httpClient.GetAsync($"api/providers?client_id={_trueLayerConfiguration.ClientId}");

            await foreach (var provider in response.Content.ReadFromJsonAsAsyncEnumerable<ExternalOpenBankingProvider>())
            {
                yield return provider;
            }
        }

        public async Task<ExternalOpenBankingAccountConnectionResponseModel> GetProviderInformation(string authToken)
        {
            using var httpClient = await BuildHttpClient(_trueLayerConfiguration.BaseDataUrl, authToken);

            var response = await httpClient.GetAsync("v1/me");

            var responseBody = await response.Content.ReadFromJsonAsync<ExternalOpenBankingAccountConnectionResponseModel>();

            return responseBody;
        }

        private async Task<HttpClient> BuildHttpClient(string baseUrl, string? authHeader = null)
        {
            var httpHandler = new SentryHttpMessageHandler();
            var httpClient = new HttpClient(httpHandler);
            httpClient.BaseAddress = new Uri(baseUrl);

            if (!string.IsNullOrEmpty(authHeader))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authHeader);
            }

            var dnsEntries = await Dns.GetHostEntryAsync(Dns.GetHostName());
            httpClient.DefaultRequestHeaders.Add("x-PSU-IP", dnsEntries.AddressList.FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString());

            httpClient.Timeout = TimeSpan.FromMinutes(5);

            return httpClient;
        }
    }
}
