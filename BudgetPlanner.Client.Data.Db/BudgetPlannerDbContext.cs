using BudgetPlanner.Client.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Client.Data.Db
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

        public DbSet<AuthState> AuthState { get; set; }
    }
}
