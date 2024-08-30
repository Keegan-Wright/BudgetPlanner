using BudgetPlanner.Data.Db;
using BudgetPlanner.Data.Models;
using BudgetPlanner.External.Services.Models.OpenBanking;
using BudgetPlanner.Models.Request.OpenBanking;
using BudgetPlanner.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Services.OpenBanking
{
    public interface IOpenBankingService
    {
        IAsyncEnumerable<OpenBankingProvider> GetOpenBankingProvidersAsync();
        IAsyncEnumerable<ExternalOpenBankingAccount> GetOpenBankingAccountsAsync(string openBankingProviderId);
        IAsyncEnumerable<ExternalOpenBankingAccount> GetOpenBankingAccountsAsync();
        IAsyncEnumerable<ExternalOpenBankingAccountBalance> GetOpenBankingAccountBalanceAsync(string openBankingProviderId, string accountId);
        IAsyncEnumerable<ExternalOpenBankingAccountTransaction> GetOpenBankingAccountTransactionsAsync(string openBankingProviderId, string accountId);
        IAsyncEnumerable<ExternalOpenBankingAccountTransaction> GetOpenBankingAccountPendingTransactionsAsync(string openBankingProviderId, string accountId);
        IAsyncEnumerable<ExternalOpenBankingAccountStandingOrder> GetOpenBankingAccountStandingOrdersAsync(string openBankingProviderId, string accountId);
        IAsyncEnumerable<ExternalOpenBankingDirectDebit> GetOpenBankingAccountDirectDebitsAsync(string openBankingProviderId, string accountId);
        IAsyncEnumerable<ExternalOpenBankingProvider> GetOpenBankingProvidersForClientAsync();
        string BuildAuthUrl(GetProviderSetupUrlRequestModel setupProviderRequestModel);
        Task<bool> AddVendorViaAccessCode(string accessCode);
    }
}
