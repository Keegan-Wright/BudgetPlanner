var builder = DistributedApplication.CreateBuilder(args);

//ar username = builder.AddParameter("username", value: "budgetPlanner", secret: false);
//ar password = builder.AddParameter("password", value:"Test", secret: false);



var sentryConfig = builder.Configuration.GetSection("Sentry");
var openBankingConfig = builder.Configuration.GetSection("OpenBanking");

var postgres = builder.AddPostgres("budgetPlannerPostgres")
    .WithDataVolume(isReadOnly: false);

var postgresDb = postgres.AddDatabase("budgetPlannerPostgresDb");


var server = builder.AddProject<Projects.BudgetPlanner_Server>("BudgetPlannerServer")
    
    .WithEnvironment("SENTRY_DSN", sentryConfig["Dsn"])
    .WithEnvironment("SENTRY_DEBUG", sentryConfig["Debug"])
    .WithEnvironment("SENTRY_AUTO_SESSION_TRACKING", sentryConfig["AutoSessionTracking"])
    .WithEnvironment("SENTRY_TRACES_SAMPLE_RATE", sentryConfig["TracesSampleRate"])
    .WithEnvironment("SENTRY_PROFILES_SAMPLE_RATE", sentryConfig["ProfilesSampleRate"])
    .WithEnvironment("SENTRY_RELEASE", sentryConfig["Release"])
    .WithEnvironment("SENTRY_CAPTURE_FAILED_REQUESTS", sentryConfig["CaptureFailedRequests"])
    
    .WithEnvironment("OPEN_BANKING_TRUELAYER_BASE_AUTH_URL", openBankingConfig["TrueLayer:BaseAuthUrl"])
    .WithEnvironment("OPEN_BANKING_TRUELAYER_BASE_DATA_URL", openBankingConfig["TrueLayer:BaseDataUrl"])
    .WithEnvironment("OPEN_BANKING_TRUELAYER_AUTH_REDIRECT_URL", openBankingConfig["TrueLayer:AuthRedirectUrl"])
    .WithEnvironment("OPEN_BANKING_TRUELAYER_CLIENT_ID", openBankingConfig["TrueLayer:ClientId"])
    .WithEnvironment("OPEN_BANKING_TRUELAYER_CLIENT_SECRET", openBankingConfig["TrueLayer:ClientSecret"])
    
    .WithReference(postgresDb)
    .WaitFor(postgresDb);


//var desktopClient = builder.AddProject<Projects.BudgetPlanner_Client_Desktop>("BudgetPlannerClientDesktop")
//    .WithReference(server)
//    .WaitFor(server);



builder.Build().Run();