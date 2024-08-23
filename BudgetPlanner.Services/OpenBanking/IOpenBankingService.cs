using BudgetPlanner.Data.Models;
using BudgetPlanner.External.Services.Models.OpenBanking;
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
        IAsyncEnumerable<OpenBankingAccount> GetOpenBankingAccountsAsync(string providerId);
        IAsyncEnumerable<OpenBankingAccount> GetOpenBankingAccountsAsync();
    }
}
