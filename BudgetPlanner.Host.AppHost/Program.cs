using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

//ar username = builder.AddParameter("username", value: "budgetPlanner", secret: false);
//ar password = builder.AddParameter("password", value:"Test", secret: false);


var sentryConfig = builder.Configuration.GetSection("Sentry");
var openBankingConfig = builder.Configuration.GetSection("OpenBanking");

var postgres = builder.AddPostgres("budgetPlannerPostgres")
    .WithDataVolume(isReadOnly: false);

var postgresDb = postgres.AddDatabase("budgetPlannerPostgresDb");


var server = builder.AddProject<Projects.BudgetPlanner_Server>("BudgetPlannerServer")
    .AddSentry(sentryConfig)
    .AddOpenBanking(openBankingConfig)
    .WithReference(postgresDb)
    .WaitFor(postgresDb);


//var desktopClient = builder.AddProject<Projects.BudgetPlanner_Client_Desktop>("BudgetPlannerClientDesktop")
//    .WithReference(server)
//    .WaitFor(server);


builder.Build().Run();