using BudgetPlanner.External.Services.Models.OpenBanking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BudgetPlanner.Services.OpenBanking
{
    public interface IOpenBankingApiService
    {
        Task<OpenBankingAccessResponseModel> ExchangeCodeForAccessTokenAsync(string vendorAccessCode);
        Task<OpenBankingAccessResponseModel> GetAccessTokenByRefreshTokenAsync(string refreshToken);
        Task<OpenBankingListAllAccountsResponseModel> GetAllAccountsAsync(string authToken);
        Task<OpenBankingGetAccountBalanceResponseModel> GetAccountBalanceAsync(string accountId, string authToken);
        Task<OpenBankingAccountTransactionsResponseModel> GetAccountTransactionsAsync(string accountId, string authToken);
        Task<OpenBankingAccountTransactionsResponseModel> GetAccountPendingTransactionsAsync(string accountId, string authToken);
    }
}
