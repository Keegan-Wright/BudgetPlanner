using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using BudgetPlanner.Data.Models;
using Microsoft.Extensions.Options;

namespace BudgetPlanner.Data.Db
{
    public class BudgetPlannerDbContext : DbContext
    {
        private readonly DatabaseConfiguration _databaseConfiguration;
        public BudgetPlannerDbContext(DatabaseConfiguration dbConfig)
        {
            _databaseConfiguration = dbConfig;
        }
        public BudgetPlannerDbContext()
        {
            
        }
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_databaseConfiguration?.ConnectionString ?? "BudgetPlanner.Client.db"}", sqliteOptions =>
            {
                sqliteOptions.MigrationsAssembly("BudgetPlanner.Client.Data.SqliteMigrations");
            });

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
