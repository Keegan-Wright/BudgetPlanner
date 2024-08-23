using BudgetPlanner.Data.Db;
using BudgetPlanner.Data.Models;
using BudgetPlanner.External.Services.Models.OpenBanking;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Services.OpenBanking
{
    public class OpenBankingService : IOpenBankingService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
        private readonly IOpenBankingApiService _openBankingApiService;

        public OpenBankingService(BudgetPlannerDbContext budgetPlannerDbContext, IOpenBankingApiService openBankingApiService)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
            _openBankingApiService = openBankingApiService;
        }


        public IAsyncEnumerable<OpenBankingAccount> GetOpenBankingAccountsAsync(string providerId)
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<OpenBankingAccount> GetOpenBankingAccountsAsync()
        {
            await foreach (var provider in GetOpenBankingProvidersAsync())
            {
                await EnsureAuthenticatedAsync(provider);

                var authToken = await GetAccessTokenAsync(provider);

                var accounts = await _openBankingApiService.GetAllAccountsAsync(authToken);

                await foreach (var account in accounts.Results)
                {
                    yield return account;
                }
            }
        }


        public async IAsyncEnumerable<OpenBankingProvider> GetOpenBankingProvidersAsync()
        {
            await foreach (var provider in _budgetPlannerDbContext.OpenBankingProviders.AsAsyncEnumerable())
            {
                yield return provider;
            }
        }

        private async Task<string> GetAccessTokenAsync(OpenBankingProvider provider)
        {
            var accessToken = await _budgetPlannerDbContext.OpenBankingAccessTokens.Where(x => x.ProviderId == provider.Id).OrderByDescending(x => x.Created).FirstOrDefaultAsync();

            if (accessToken.Created.AddSeconds(accessToken.ExpiresIn) > DateTime.Now)
            {
                return accessToken.AccessToken;
            }
            else
            {
                var refreshTokenResponse = await _openBankingApiService.GetAccessTokenByRefreshTokenAsync(accessToken.RefreshToken);

                var newAccessToken = new OpenBankingAccessToken()
                {
                    AccessToken = refreshTokenResponse.AccessToken,
                    ExpiresIn = refreshTokenResponse.ExpiresIn,
                    ProviderId = provider.Id,
                    RefreshToken = refreshTokenResponse.RefreshToken,
                    Created = DateTime.Now
                };

                await _budgetPlannerDbContext.AddAsync(newAccessToken);
                await _budgetPlannerDbContext.SaveChangesAsync();

                return newAccessToken.AccessToken;
            }
        }
        private async Task EnsureAuthenticatedAsync(OpenBankingProvider provider)
        {
            // User hasn't authenticated yet
            if (await _budgetPlannerDbContext.OpenBankingAccessTokens.Where(x => x.ProviderId == provider.Id).CountAsync() == 0)
            {
                var response = await _openBankingApiService.ExchangeCodeForAccessTokenAsync(provider.AccessCode);

                var accessToken = new OpenBankingAccessToken()
                {
                    AccessToken = response.AccessToken,
                    ExpiresIn = response.ExpiresIn,
                    ProviderId = provider.Id,
                    RefreshToken = response.RefreshToken,
                    Created = DateTime.Now
                };

                await _budgetPlannerDbContext.AddAsync(accessToken);
                await _budgetPlannerDbContext.SaveChangesAsync();
            }
        }


    }
}
