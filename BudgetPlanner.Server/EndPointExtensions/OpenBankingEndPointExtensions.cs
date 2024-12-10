namespace BudgetPlanner.Server.EndPoints;

public static class OpenBankingEndPointExtensions
{
    public static void MapOpenBankingEndPoint(this WebApplication app)
    {
        var openBankingGroup = app.MapGroup("/OpenBanking");

        openBankingGroup.MapGet("/GetOpenBankingProviders", () =>
        {

        });

    }
}