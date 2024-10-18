using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using BudgetPlanner.DI;
using BudgetPlanner.Services;
using BudgetPlanner.ViewModels;
using BudgetPlanner.Views;
using BudgetPlanner.Data.Db;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;
using BudgetPlanner.Models.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Sentry;
using System.Diagnostics;
using BudgetPlanner.Handlers;
using BudgetPlanner.States;

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
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
                _ = db.Database.EnsureCreated();
            }

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);

                desktop.MainWindow = Ioc.Default.GetRequiredService<MainWindow>();
                desktop.MainWindow.DataContext = Ioc.Default.GetRequiredService<MainViewModel>();
                ApplicationState.IsDesktopBasedLifetime = true;
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                ApplicationState.IsDesktopBasedLifetime = false;

                singleViewPlatform.MainView = new MainView
                {
                    DataContext = new MainViewModel(Ioc.Default.GetRequiredService<INavigationService>())
                };

            }

            base.OnFrameworkInitializationCompleted();


            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            ErrorHandler.HandleError(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ErrorHandler.HandleError(e.ExceptionObject as Exception);
        }

        private static ServiceCollection BuildServices()
        {

            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var DbPath = Path.Join(path, "BudgetPlanner.db");

            var assembly = Assembly.GetExecutingAssembly();
            var appSettingPath = $"{assembly.GetName().Name}.appsettings.json";
            using var stream = assembly.GetManifestResourceStream(appSettingPath);


            IConfiguration config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            
            
            var services = new ServiceCollection();

            services.AddSingleton(config);


            SentrySdk.Init(options =>
            {
                options.Dsn = config["Sentry:Dsn"];
                options.Debug = true;
                options.AutoSessionTracking = true;
                options.TracesSampleRate = 1.0;
                options.ProfilesSampleRate = 1.0;
                options.Release = "0.0.1";
                options.CaptureFailedRequests = true;

                options.AddDiagnosticSourceIntegration();
                options.AddEntityFramework();
            });

            var trueLayerConfig = new TrueLayerOpenBankingConfiguration();

            config.GetSection("OpenBanking:TrueLayer").Bind(trueLayerConfig);
            services.AddSingleton(trueLayerConfig);


            services.AddWindows();
            services.AddViews();
            services.AddViewModels();
            services.AddServices();
            services.AddExternalServices();

            services.AddSingleton(new DatabaseConfiguration()
            {
                ConnectionString = DbPath
            });

            services.AddDbContext<BudgetPlannerDbContext>();

            return services;
        }
    }
}