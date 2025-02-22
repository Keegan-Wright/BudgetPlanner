using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using BudgetPlanner.Server.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BudgetPlanner.Server.Data.Db
{
    public class BudgetPlannerDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IDesignTimeDbContextFactory<BudgetPlannerDbContext>
    {
        public BudgetPlannerDbContext()
        {
            
        }

        public BudgetPlannerDbContext(DbContextOptions<BudgetPlannerDbContext> options) : base(options)
        {
        }

        // User Related
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        
        
        // Client Related
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

        public BudgetPlannerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BudgetPlannerDbContext>();
            optionsBuilder.UseNpgsql("");

            return new BudgetPlannerDbContext(optionsBuilder.Options);
        }

        public IQueryable<ApplicationUser> IsolateToUser(Guid userId)
        {
            return ApplicationUsers.Where(x => x.Id == userId);
        }
    }
}
