namespace BudgetPlanner.Server.EndPoints;

public static class AuthEndPointExtensions
{
    public static void MapAuthEndPoint(this WebApplication app)
    {
        var authGroup = app.MapGroup("/Auth");


        authGroup.MapPost("/Login", async (context) =>
        {

        });
        
        authGroup.MapPost("/Logout", async (context) =>
        {

        });

        
    }
}