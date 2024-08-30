using BudgetPlanner.External.Services.OpenBanking;
using BudgetPlanner.Services;
using BudgetPlanner.Services.Accounts;
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

            services.AddSingleton<SetupProviderViewModel>();

            services.AddSingleton<DashboardViewModel>();
            services.AddSingleton<AccountsViewModel>();
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

            services.AddSingleton<SetupProviderView>();

            services.AddSingleton<DashboardView>();

            services.AddSingleton<AccountsView>();


        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IBudgetCategoriesService, BudgetCategoriesService>();
            services.AddSingleton<IHouseholdMembersService,  HouseholdMembersService>();
            services.AddSingleton<IOpenBankingService, OpenBankingService>();
            services.AddSingleton<IAccountsService,  AccountsService>();
        }

        public static void AddExternalServices(this IServiceCollection services)
        {
            services.AddSingleton<IOpenBankingApiService, TrueLayerOpenBankingApiService>();
        }
    }
}
