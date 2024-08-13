using BudgetPlanner.Services;
using BudgetPlanner.Services.Budget;
using BudgetPlanner.ViewModels;
using BudgetPlanner.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IBudgetCategoriesService, BudgetCategoriesService>();
        }
    }
}
