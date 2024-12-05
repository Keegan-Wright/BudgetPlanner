using System.Text.Json;
using System.Text.Json.Serialization;
using BudgetPlanner.Server.EndPoints;

namespace BudgetPlanner.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);
        builder.AddServiceDefaults();
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
        });

        var app = builder.Build();
        
        
        app.MapOpenBankingEndPoint();
        

        app.Run();
    }
}