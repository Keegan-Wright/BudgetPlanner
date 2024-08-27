
using BudgetPlanner.Configuration.Models;
using BudgetPlanner.External.Services.Models.OpenBanking;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BudgetPlanner.Services.OpenBanking
{
    public class TrueLayerOpenBankingApiService : IOpenBankingApiService
    {
        private readonly TrueLayerOpenBankingConfiguration _trueLayerConfiguration;

        public TrueLayerOpenBankingApiService(TrueLayerOpenBankingConfiguration trueLayerConfiguration)
        {
            _trueLayerConfiguration = trueLayerConfiguration;
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

        public async Task<ExternalOpenBankingAccountTransactionsResponseModel> GetAccountPendingTransactionsAsync(string accountId, string authToken)
        {
            using var httpClient = await BuildHttpClient(_trueLayerConfiguration.BaseDataUrl, authToken);

            var response = await httpClient.GetAsync($"v1/accounts/{accountId}/transactions/pending");

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

        public async Task<ExternalOpenBankingAccountTransactionsResponseModel> GetAccountTransactionsAsync( string accountId, string authToken)
        {
            using var httpClient = await BuildHttpClient(_trueLayerConfiguration.BaseDataUrl, authToken);

            var response = await httpClient.GetAsync($"v1/accounts/{accountId}/transactions");

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

        private async Task<HttpClient> BuildHttpClient(string baseUrl, string? authHeader = null)
        {
            var httpClient = new HttpClient();
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
