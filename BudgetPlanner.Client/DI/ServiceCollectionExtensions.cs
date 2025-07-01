using BudgetPlanner.Client.Views;
using BudgetPlanner.Server.External.Services.OpenBanking;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Auth;
using BudgetPlanner.Client.Services.BugetCategories;
using BudgetPlanner.Client.Services.Calendar;
using BudgetPlanner.Client.Services.Classifications;
using BudgetPlanner.Client.Services.Dashboard;
using BudgetPlanner.Client.Services.HouseholdMember;
using BudgetPlanner.Client.Services.OpenBanking;
using BudgetPlanner.Client.Services.Reports;
using BudgetPlanner.Client.Services.Transactions;
using BudgetPlanner.Client.ViewModels;
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
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<LandingPageViewModel>();
            services.AddTransient<SpentInTimePeriodReportViewModel>();
            services.AddTransient<AccountBreakdownReportViewModel>();
            services.AddTransient<CategoryBreakdownReportViewModel>();
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
            services.AddTransient<LoginView>();
            services.AddTransient<RegisterView>();
            services.AddTransient<LandingPageView>();
            services.AddTransient<SpentInTimePeriodReportView>();
        }

        public static void AddClientServices(this IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
            
            services.AddSingleton<IAccountsRequestService, AccountsRequestService>();
            services.AddSingleton<IBudgetCategoriesRequestService, BudgetCategoriesRequestService>();
            services.AddSingleton<ICalendarRequestService, CalendarRequestService>();
            services.AddSingleton<IClassificationsRequestService, ClassificationsRequestService>();
            services.AddSingleton<IDashboardRequestService, DashboardRequestService>();
            services.AddSingleton<IHouseholdMemberRequestService, HouseholdMemberRequestService>();
            services.AddSingleton<IOpenBankingRequestService, OpenBankingRequestService>();
            services.AddSingleton<ITransactionsRequestService, TransactionsRequestService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IReportsService, ReportsService>();

        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<AddCustomClassificationsToTransactionViewModel>, AddCustomClassificationsToTransactionViewModelValidator>();
            services.AddTransient<IValidator<AddCustomClassificationViewModel>, AddCustomClassificationViewModelValidator>();
            
            services.AddTransient<IValidator<LoginViewModel>, LoginViewModelValidator>();
            services.AddTransient<IValidator<RegisterViewModel>, RegisterViewModelValidator>();
        }

        public static void AddExternalServices(this IServiceCollection services)
        {
            services.AddSingleton<IOpenBankingApiService, TrueLayerOpenBankingApiService>();
        }
    }
}
