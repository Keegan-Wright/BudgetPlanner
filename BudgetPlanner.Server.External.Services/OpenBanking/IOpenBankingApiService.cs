﻿using BudgetPlanner.Server.External.Services.Models.OpenBanking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BudgetPlanner.Server.External.Services.OpenBanking
{
    public interface IOpenBankingApiService
    {
        Task<ExternalOpenBankingAccessResponseModel> ExchangeCodeForAccessTokenAsync(string vendorAccessCode);
        Task<ExternalOpenBankingAccessResponseModel> GetAccessTokenByRefreshTokenAsync(string refreshToken);
        Task<ExternalOpenBankingListAllAccountsResponseModel> GetAllAccountsAsync(string authToken);
        Task<ExternalOpenBankingGetAccountBalanceResponseModel> GetAccountBalanceAsync(string accountId, string authToken);
        Task<ExternalOpenBankingAccountTransactionsResponseModel> GetAccountTransactionsAsync(string accountId, string authToken, DateTime? transactionsStartingDate);
        Task<ExternalOpenBankingAccountTransactionsResponseModel> GetAccountPendingTransactionsAsync(string accountId, string authToken, DateTime? transactionsStartingDate);
        Task<ExternalOpenBankingAccountStandingOrdersResponseModel> GetAccountStandingOrdersAsync(string accountId, string authToken);
        Task<ExternalOpenBankingAccountDirectDebitsResponseModel> GetAccountDirectDebitsAsync(string accountId, string authToken);
        IAsyncEnumerable<ExternalOpenBankingProvider> GetAvailableProvidersAsync();
        string BuildAuthUrl(IEnumerable<string> providerIds, IEnumerable<string> scopes);
        Task<ExternalOpenBankingAccountConnectionResponseModel> GetProviderInformation(string accessToken);
    }
}
