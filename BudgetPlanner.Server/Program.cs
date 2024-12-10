using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.DI;
using BudgetPlanner.Server.EndPoints;
using BudgetPlanner.Server.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace BudgetPlanner.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);
        builder.AddServiceDefaults();
        
        builder.AddNpgsqlDbContext<BudgetPlannerDbContext>(connectionName: "budgetPlannerPostgresDb", options =>
        {
            options.DisableRetry= false;
            options.CommandTimeout = 30;  
        });

        builder.Services.AddInternalServices();
        builder.Services.AddExternalServices();
        
        
        var trueLayerConfig = new TrueLayerOpenBankingConfiguration();
        trueLayerConfig.BaseAuthUrl = builder.Configuration.GetValue<string>("OPEN_BANKING_TRUELAYER_BASE_AUTH_URL");
        trueLayerConfig.BaseDataUrl = builder.Configuration.GetValue<string>("OPEN_BANKING_TRUELAYER_BASE_DATA_URL");
        trueLayerConfig.AuthRedirectUrl = builder.Configuration.GetValue<string>("OPEN_BANKING_TRUELAYER_AUTH_REDIRECT_URL");
        trueLayerConfig.ClientId = builder.Configuration.GetValue<string>("OPEN_BANKING_TRUELAYER_CLIENT_ID");
        trueLayerConfig.ClientSecret = builder.Configuration.GetValue<Guid>("OPEN_BANKING_TRUELAYER_CLIENT_SECRET");
        
        builder.Services.AddSingleton(trueLayerConfig);
        
        
        SentrySdk.Init(options =>
        {
            options.Dsn = builder.Configuration.GetValue<string>("SENTRY_DSN");
            options.Debug = builder.Configuration.GetValue<bool>("SENTRY_DEBUG");
            options.AutoSessionTracking = builder.Configuration.GetValue<bool>("SENTRY_AUTO_SESSION_TRACKING");;
            options.TracesSampleRate = builder.Configuration.GetValue<double>("SENTRY_TRACES_SAMPLE_RATE");
            options.ProfilesSampleRate = builder.Configuration.GetValue<double>("SENTRY_PROFILES_SAMPLE_RATE");
            options.Release = builder.Configuration.GetValue<string>("SENTRY_RELEASE");
            options.CaptureFailedRequests = builder.Configuration.GetValue<bool>("SENTRY_CAPTURE_FAILED_REQUESTS");

            options.AddDiagnosticSourceIntegration();
            options.AddEntityFramework();
        });
        var app = builder.Build();
        
        
        EnsureDbMigrated(app);
        
        MapEndPoints(app);
        
        

        app.Run();
        
        
    }

    private static void MapEndPoints(WebApplication app)
    {
        app.MapOpenBankingEndPoint();
    }

    private static void EnsureDbMigrated(WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        using BudgetPlannerDbContext? db = scope.ServiceProvider.GetService<BudgetPlannerDbContext>();

        if (db != null)
        {
            try
            {
                db.Database.Migrate();
            }
            catch (Exception)
            {
                _ = db.Database.EnsureCreated();
            }

        }
    }
}