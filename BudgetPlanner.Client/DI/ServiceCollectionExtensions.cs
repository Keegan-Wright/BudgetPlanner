using BudgetPlanner.Client.Views;
using BudgetPlanner.Server.External.Services.OpenBanking;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.ViewModels;
using BudgetPlanner.Client.ViewModels.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetPlanner.Client.DI
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
