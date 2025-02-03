 using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using BudgetPlanner.Client.DI;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.ViewModels;
using BudgetPlanner.Client.Data.Db;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;
using BudgetPlanner.Server.Models.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Sentry;
using System.Diagnostics;
using Avalonia.Controls;
using BudgetPlanner.Client.Views;
using BudgetPlanner.Client.Handlers;
using BudgetPlanner.Client.Services.Auth;
using BudgetPlanner.Client.States;
using Microsoft.Extensions.Hosting;

namespace BudgetPlanner.Client
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
                    DataContext = new MainViewModel(Ioc.Default.GetRequiredService<INavigationService>(), Ioc.Default.GetRequiredService<IAuthenticationService>())
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

            SQLitePCL.Batteries.Init();

            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var DbPath = Path.Join(path, "BudgetPlanner.Client.db");

            var assembly = Assembly.GetExecutingAssembly();
            var appSettingPath = $"{assembly.GetName().Name}.appsettings.json";
            using var stream = assembly.GetManifestResourceStream(appSettingPath);


            IConfiguration config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            
            
            
            var services = new ServiceCollection();

            services.AddHttpClient("apiClient",client =>
            {
                // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
                // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
                client.BaseAddress = new(Environment.GetEnvironmentVariable("services__BudgetPlannerServer__http__0"));
            });
            
            
            
            services.AddServiceDefaults();
            
            services.AddSingleton(config);

            if (!Design.IsDesignMode)
            {
                SentrySdk.Init(options =>
                {
                    options.Dsn =   Environment.GetEnvironmentVariable("SENTRY_DSN");
                    options.Debug = bool.Parse(Environment.GetEnvironmentVariable("SENTRY_DEBUG"));
                    options.AutoSessionTracking = bool.Parse(Environment.GetEnvironmentVariable("SENTRY_AUTO_SESSION_TRACKING"));
                    options.TracesSampleRate = double.Parse(Environment.GetEnvironmentVariable("SENTRY_TRACES_SAMPLE_RATE"));
                    options.ProfilesSampleRate = double.Parse(Environment.GetEnvironmentVariable("SENTRY_PROFILES_SAMPLE_RATE"));
                    options.Release = Environment.GetEnvironmentVariable("SENTRY_RELEASE");
                    options.CaptureFailedRequests = bool.Parse(Environment.GetEnvironmentVariable("SENTRY_CAPTURE_FAILED_REQUESTS"));

                    options.AddDiagnosticSourceIntegration();
                    options.AddEntityFramework();
                });
            }
            
            services.AddWindows();
            services.AddViews();
            services.AddViewModels();
            services.AddClientServices();
            services.AddExternalServices();
            services.AddValidators();

            services.AddSingleton(new DatabaseConfiguration()
            {
                ConnectionString = DbPath
            });

            services.AddDbContext<BudgetPlannerDbContext>();
            
            return services;
        }
    }
}