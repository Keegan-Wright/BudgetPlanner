using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


var desktopClient = builder.AddProject<Projects.BudgetPlanner_Client_Desktop>("BudgetPlannerClientDesktop");
var server = builder.AddProject<Projects.BudgetPlanner_Server>("BudgetPlannerServer");


builder.Build().Run();