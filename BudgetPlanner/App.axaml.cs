using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using BudgetPlanner.DI;
using BudgetPlanner.Messages;
using BudgetPlanner.Services;
using BudgetPlanner.ViewModels;
using BudgetPlanner.Views;
using BudgetPlanner.Data.Db;
using CommunityToolkit.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace BudgetPlanner
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            ServiceCollection services = BuildServices();

            var provider = services.BuildServiceProvider();


            Ioc.Default.ConfigureServices(provider);


            var db = Ioc.Default.GetRequiredService<BudgetPlannerDbContext>();

            try
            {
                db.Database.Migrate();
            }
            catch (Exception)
            {
                _ = db.Database.EnsureCreated();
            }

            var mainWindow = Ioc.Default.GetRequiredService<MainWindow>();
            var mainViewModel = Ioc.Default.GetRequiredService<MainViewModel>();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);

                desktop.MainWindow = mainWindow;
                desktop.MainWindow.DataContext = mainViewModel;

            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = mainWindow;
                singleViewPlatform.MainView.DataContext = mainViewModel;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static ServiceCollection BuildServices()
        {
            var services = new ServiceCollection();

            services.AddWindows();
            services.AddViews();
            services.AddViewModels();
            services.AddServices();

            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var DbPath = System.IO.Path.Join(path, "BudgetPlanner.db");


            services.AddDbContext<BudgetPlannerDbContext>();
            //services.AddDbContext<BudgetPlannerDbContext>(dbOptions =>
            //{
            //    dbOptions.UseSqlite($"Data Source={DbPath}", sqliteOptionsAction: options =>
            //     {
            //         _ = options.MigrationsAssembly("BudgetPlanner.Data.SqliteMigrations");
            //         _ = options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
            //     });
            //});


            return services;
        }

        //[Singleton(typeof(MainViewModel))]
        //[Singleton(typeof(ExpensesViewModel))]
        //[Singleton(typeof(DebtViewModel))]
        //internal static partial void ConfigureViewModels(IServiceCollection services)

        //[Singleton(typeof(MainWindow))]
        //[Singleton(typeof(ExpensesView))]
        //[Singleton(typeof(DebtView))]
        //internal static partial void ConfigureViews(IServiceCollection services);

        //[Singleton(typeof(NavigationService), typeof(INavigationService))]
        //internal static partial void ConfigureServices(IServiceCollection services);

    }
}