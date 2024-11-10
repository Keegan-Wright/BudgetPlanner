using BudgetPlanner.External.Services.OpenBanking;
using BudgetPlanner.Services;
using BudgetPlanner.Services.Accounts;
using BudgetPlanner.Services.Budget;
using BudgetPlanner.Services.Classifications;
using BudgetPlanner.Services.Dashboard;
using BudgetPlanner.Services.OpenBanking;
using BudgetPlanner.Services.Transactions;
using BudgetPlanner.ViewModels;
using BudgetPlanner.ViewModels.Validators;
using BudgetPlanner.Views;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetPlanner.DI
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddViewModels(this IServiceCollection services)
        {
            services.AddTransient<MainViewModel>();
            services.AddTransient<ExpensesViewModel>();
            services.AddTransient<DebtViewModel>();
            services.AddTransient<BudgetCategoriesViewModel>();
            services.AddTransient<AddBudgetCategoryViewModel>();
            services.AddTransient<EditBudgetCategoryViewModel>();
            services.AddTransient<HouseholdMembersViewModel>();
            services.AddTransient<AddHouseholdMemberViewModel>();
            services.AddTransient<SetupProviderViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<AccountsViewModel>();
            services.AddTransient<TransactionsViewModel>();
            services.AddTransient<ClassificationSettingsViewModel>();
            services.AddTransient<AddCustomClassificationViewModel>();
            services.AddTransient<AddCustomClassificationsToTransactionViewModel>();
            services.AddTransient<CalendarViewModel>();
        }

        public static void AddWindows(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
        }

        public static void AddViews(this IServiceCollection services)
        {
            services.AddTransient<MainView>();
            services.AddTransient<ExpensesView>();
            services.AddTransient<DebtView>();
            services.AddTransient<BudgetCategoriesView>();
            services.AddTransient<AddBudgetCategoryView>();
            services.AddTransient<HouseholdMembersView>();
            services.AddTransient<AddHouseholdMemberView>();
            services.AddTransient<SetupProviderView>();
            services.AddTransient<DashboardView>();
            services.AddTransient<AccountsView>();
            services.AddTransient<ClassificationSettingsView>();
            services.AddTransient<AddCustomClassificationView>();
            services.AddTransient<AddCustomClassificationsToTransactionView>();
            services.AddTransient<CalendarView>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IBudgetCategoriesService, BudgetCategoriesService>();
            services.AddSingleton<IHouseholdMembersService,  HouseholdMembersService>();
            services.AddSingleton<IOpenBankingService, OpenBankingService>();
            services.AddSingleton<IAccountsService,  AccountsService>();
            services.AddSingleton<IDashboardService,  DashboardService>();
            services.AddSingleton<ITransactionsService, TransactionsService>();
            services.AddSingleton<IClassificationService, ClassificationService>();

        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<AddCustomClassificationsToTransactionViewModel>, AddCustomClassificationsToTransactionViewModelValidator>();
            services.AddTransient<IValidator<AddCustomClassificationViewModel>, AddCustomClassificationViewModelValidator>();
        }

        public static void AddExternalServices(this IServiceCollection services)
        {
            services.AddSingleton<IOpenBankingApiService, TrueLayerOpenBankingApiService>();
        }
    }
}
