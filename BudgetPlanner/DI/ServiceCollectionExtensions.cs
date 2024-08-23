using BudgetPlanner.Services;
using BudgetPlanner.Services.Budget;
using BudgetPlanner.Services.OpenBanking;
using BudgetPlanner.ViewModels;
using BudgetPlanner.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetPlanner.DI
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<ExpensesViewModel>();
            services.AddSingleton<DebtViewModel>();

            services.AddSingleton<BudgetCategoriesViewModel>();
            services.AddSingleton<AddBudgetCategoryViewModel>();
            services.AddSingleton<EditBudgetCategoryViewModel>();

            services.AddSingleton<HouseholdMembersViewModel>();
            services.AddSingleton<AddHouseholdMemberViewModel>();

            services.AddSingleton<DashboardViewModel>();
        }

        public static void AddWindows(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
        }

        public static void AddViews(this IServiceCollection services)
        {
            services.AddSingleton<MainView>();
            services.AddSingleton<ExpensesView>();
            services.AddSingleton<DebtView>();
            services.AddSingleton<BudgetCategoriesView>();
            services.AddSingleton<AddBudgetCategoryView>();

            services.AddSingleton<HouseholdMembersView>();
            services.AddSingleton<AddHouseholdMemberView>();

            services.AddSingleton<DashboardView>();


        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IBudgetCategoriesService, BudgetCategoriesService>();
            services.AddSingleton<IHouseholdMembersService,  HouseholdMembersService>();
            services.AddSingleton<IOpenBankingService, OpenBankingService>();
        }

        public static void AddExternalServices(this IServiceCollection services)
        {
            services.AddSingleton<IOpenBankingApiService, TrueLayerOpenBankingApiService>();
        }
    }
}
