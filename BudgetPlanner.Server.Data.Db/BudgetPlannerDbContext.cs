using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using BudgetPlanner.Server.Data.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BudgetPlanner.Server.Data.Db
{
    public class BudgetPlannerDbContext : DbContext
    {
        public BudgetPlannerDbContext(DbContextOptions<BudgetPlannerDbContext> options) : base(options)
        {
        }


        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        public DbSet<HouseholdMember> HouseholdMembers { get; set; }
        public DbSet<BudgetCategory> BudgetCategories { get; set; }
        public DbSet<Debt> Debts { get; set; }

        public DbSet<OpenBankingProvider> OpenBankingProviders { get; set; }
        public DbSet<OpenBankingProviderScopes> OpenBankingProviderScopes { get; set; }
        public DbSet<OpenBankingAccount> OpenBankingAccounts { get; set; }
        public DbSet<OpenBankingAccountBalance> OpenBankingAccountBalances { get; set; }
        public DbSet<OpenBankingTransaction> OpenBankingTransactions { get; set; }
        public DbSet<OpenBankingAccessToken> OpenBankingAccessTokens { get; set; }
        public DbSet<OpenBankingStandingOrder> OpenBankingStandingOrders { get; set; }
        public DbSet<OpenBankingSynronisation> OpenBankingSyncronisations { get; set; }
        public DbSet<OpenBankingDirectDebit> OpenBankingDirectDebits { get; set; }
        public DbSet<OpenBankingTransactionClassifications> OpenBankingTransactionClassifications { get; set; }
        public DbSet<CustomClassification> CustomClassifications { get; set; }

    }
}
