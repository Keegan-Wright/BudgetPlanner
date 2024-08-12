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
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_databaseConfiguration.ConnectionString}", sqliteOptions =>
            {
                sqliteOptions.MigrationsAssembly("BudgetPlanner.Data.SqliteMigrations");
            });

        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }


        public DbSet<HouseholdMember> HouseholdMembers { get; set; }
        public DbSet<BudgetCategory> BudgetCategories { get; set; }

        public DbSet<Debt> Debts { get; set; }

    }

    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; }
    }
}
