using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using BudgetPlanner.Messages;
using BudgetPlanner.Services;
using BudgetPlanner.ViewModels;
using BudgetPlanner.Views;
using CommunityToolkit.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

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
            var services = new ServiceCollection();
            // You can split registrations across multiple methods or classes, but you need to remember to call them all
            ConfigureServices(services);
            ConfigureViews(services);
            ConfigureViewModels(services);
            var provider = services.BuildServiceProvider(); ; // Warning in MEDI 7.0, fixed in 8.0

            Ioc.Default.ConfigureServices(provider);

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

        [Singleton(typeof(MainViewModel))]
        [Singleton(typeof(ExpensesViewModel))]
        [Singleton(typeof(DebtViewModel))]
        internal static partial void ConfigureViewModels(IServiceCollection services);

        [Singleton(typeof(MainWindow))]
        [Singleton(typeof(ExpensesView))]
        [Singleton(typeof(DebtView))]
        internal static partial void ConfigureViews(IServiceCollection services);

        [Singleton(typeof(NavigationService), typeof(INavigationService))]
        internal static partial void ConfigureServices(IServiceCollection services);
    }
}