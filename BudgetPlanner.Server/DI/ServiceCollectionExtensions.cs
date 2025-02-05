using System.Security.Claims;
using BudgetPlanner.Server.External.Services.OpenBanking;
using BudgetPlanner.Server.Services.Accounts;
using BudgetPlanner.Server.Services.Auth;
using BudgetPlanner.Server.Services.Budget;
using BudgetPlanner.Server.Services.Calendar;
using BudgetPlanner.Server.Services.Classifications;
using BudgetPlanner.Server.Services.Dashboard;
using BudgetPlanner.Server.Services.OpenBanking;
using BudgetPlanner.Server.Services.Transactions;

namespace BudgetPlanner.Server.DI;

public static class ServiceCollectionExtensions
{
    public static void AddInternalServices(this IServiceCollection services)
    {
        services.AddScoped<IBudgetCategoriesService, BudgetCategoriesService>();
        services.AddScoped<IHouseholdMembersService,  HouseholdMembersService>();
        services.AddScoped<IOpenBankingService, OpenBankingService>();
        services.AddScoped<IAccountsService,  AccountsService>();
        services.AddScoped<IDashboardService,  DashboardService>();
        services.AddScoped<ITransactionsService, TransactionsService>();
        services.AddScoped<IClassificationService, ClassificationService>();
        services.AddScoped<ICalendarService, CalendarService>();
        services.AddScoped<IAuthService, AuthService>();
    }
    
    public static void AddExternalServices(this IServiceCollection services)
    {
        services.AddSingleton<IOpenBankingApiService, TrueLayerOpenBankingApiService>();
    }

    public static void AddClaimsPrincipalServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        //services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ClaimsPrincipal>(s => s.GetRequiredService<IHttpContextAccessor>().HttpContext.User);
    }
}