using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using BudgetPlanner.Data.Models;

namespace BudgetPlanner.Data.Db
{
    public class BudgetPlannerDbContext : DbContext
    {
        //public BudgetPlannerDbContext(DbContextOptions<BudgetPlannerDbContext> options) : base(options)
        //{
        //}
        public BudgetPlannerDbContext()
        {
            
        }
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source=BudgetManager.Db", sqliteOptions =>
            {
                sqliteOptions.MigrationsAssembly("BudgetPlanner.Data.SqliteMigrations");
            });

        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }


        public DbSet<HouseholdMember> HouseholdMembers { get; set; }
        public DbSet<BudgetCategory> BudgetCategories { get; set; }

        public DbSet<Debt> Debts { get; set; }

    }

}
