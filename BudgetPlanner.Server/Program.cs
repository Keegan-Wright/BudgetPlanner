using System.Reflection;
using System.Text;
using System.Text.Unicode;
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Server.DI;
using BudgetPlanner.Server.EndPoints;
using BudgetPlanner.Server.Models.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Sentry.OpenTelemetry;

namespace BudgetPlanner.Server;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        
        // Exclude from openapi gen as it throws errors
        if (Assembly.GetEntryAssembly()?.GetName().Name != "GetDocument.Insider")
        {
            
            builder.AddServiceDefaults();
            
            builder.WebHost.UseSentry(options =>
            {
                options.Dsn = builder.Configuration.GetValue<string>("SENTRY_DSN");
                options.Debug = builder.Configuration.GetValue<bool>("SENTRY_DEBUG");
                options.AutoSessionTracking = builder.Configuration.GetValue<bool>("SENTRY_AUTO_SESSION_TRACKING");;
                options.TracesSampleRate = builder.Configuration.GetValue<double>("SENTRY_TRACES_SAMPLE_RATE");
                options.ProfilesSampleRate = builder.Configuration.GetValue<double>("SENTRY_PROFILES_SAMPLE_RATE");
                options.Release = builder.Configuration.GetValue<string>("SENTRY_RELEASE");
                options.CaptureFailedRequests = builder.Configuration.GetValue<bool>("SENTRY_CAPTURE_FAILED_REQUESTS");
                options.UseOpenTelemetry();
                options.AddDiagnosticSourceIntegration();
                options.AddEntityFramework();
            });
            
            builder.AddNpgsqlDbContext<BudgetPlannerDbContext>(connectionName: "budgetPlannerPostgresDb", options =>
            {
                options.DisableRetry= false;
                options.CommandTimeout = 0;
            });

        }

        
        


        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
            })
            .AddEntityFrameworkStores<BudgetPlannerDbContext>()
            .AddDefaultTokenProviders();
        
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("AUTH_ISSUER"),
                    ValidAudience = builder.Configuration.GetValue<string>("AUTH_AUDIENCE"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(builder.Configuration.GetValue<string>("AUTH_SIGNING_KEY")))),
                };
            });

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        builder.Services.AddCors(o =>
        {
            o.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });
        


        
        builder.Services.AddInternalServices();
        builder.Services.AddExternalServices();
        builder.Services.AddClaimsPrincipalServices();

        builder.Services.AddOpenApi();
        
        var trueLayerConfig = new TrueLayerOpenBankingConfiguration();
        trueLayerConfig.BaseAuthUrl = builder.Configuration.GetValue<string>("OPEN_BANKING_TRUELAYER_BASE_AUTH_URL");
        trueLayerConfig.BaseDataUrl = builder.Configuration.GetValue<string>("OPEN_BANKING_TRUELAYER_BASE_DATA_URL");
        trueLayerConfig.AuthRedirectUrl = builder.Configuration.GetValue<string>("OPEN_BANKING_TRUELAYER_AUTH_REDIRECT_URL");
        trueLayerConfig.ClientId = builder.Configuration.GetValue<string>("OPEN_BANKING_TRUELAYER_CLIENT_ID");
        trueLayerConfig.ClientSecret = builder.Configuration.GetValue<Guid>("OPEN_BANKING_TRUELAYER_CLIENT_SECRET");
        
        builder.Services.AddSingleton(trueLayerConfig);
        
        var app = builder.Build();

        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseCors();

        app.MapOpenApi();
        app.MapScalarApiReference(x =>
        {
            x.WithClientButton(false);
            x.WithTagSorter(TagSorter.Alpha);
            x.WithOperationSorter(OperationSorter.Method);
        });
        
        app.UseExceptionHandler(exceptionHandlerApp 
            => exceptionHandlerApp.Run(async context 
                => await Results.Problem()
                    .ExecuteAsync(context)));
        
        
        await EnsureDbMigratedAsync(app);
        
        MapEndPoints(app);


        await app.RunAsync();
        
        
    }

    private static void MapEndPoints(WebApplication app)
    {
        app.MapOpenBankingEndPoint();
        app.MapDashboardEndPoint();
        app.MapAccountsEndPoint();
        app.MapBudgetCategoriesEndPoint();
        app.MapCalendarEndPoint();
        app.MapClassificationEndPoint();
        app.MapHouseholdMembersEndpoint();
        app.MapTransactionsEndPoint();
        app.MapAuthEndPoint();
        app.MapReportingEndpoint();
    }

    private static async Task EnsureDbMigratedAsync(WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        using BudgetPlannerDbContext? db = scope.ServiceProvider.GetService<BudgetPlannerDbContext>();

        if (db != null)
        {
            try
            {
                await db.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                await db.Database.EnsureCreatedAsync();
            }

        }
    }
}