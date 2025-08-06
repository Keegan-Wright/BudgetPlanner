using Aspire.Hosting;
using BudgetPlanner.Host.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

//ar username = builder.AddParameter("username", value: "budgetPlanner", secret: false);
//ar password = builder.AddParameter("password", value:"Test", secret: false);


var sentryConfig = builder.Configuration.GetSection("Sentry");
var openBankingConfig = builder.Configuration.GetSection("OpenBanking");
var authConfig = builder.Configuration.GetSection("Auth");

var postgres = builder.AddPostgres("budgetPlannerPostgres")
    .WithDataVolume(isReadOnly: false);

var postgresDb = postgres.AddDatabase("budgetPlannerPostgresDb");


var server = builder.AddProject<Projects.BudgetPlanner_Server>("BudgetPlannerServer")
    .AddSentry(sentryConfig)
    .AddOpenBanking(openBankingConfig)
    .AddAuth(authConfig)
    .WithReference(postgresDb)
    .WaitFor(postgresDb);


var desktopClient = builder.AddProject<Projects.BudgetPlanner_Client_Desktop>("BudgetPlannerClientDesktop")
    .AddSentry(sentryConfig)
    .WithReference(server)
    .WaitFor(server);



await builder.Build().RunAsync();