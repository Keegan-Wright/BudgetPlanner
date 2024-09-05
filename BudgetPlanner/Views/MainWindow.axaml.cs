using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using BudgetPlanner.Messages;
using BudgetPlanner.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace BudgetPlanner.Views
{
    public partial class MainWindow : Window, IRecipient<ThemeChangeRequestedMessage>
    {
        public MainWindow()
        {

            WeakReferenceMessenger.Default.Register(this);
            InitializeComponent();
            
        }

        public void Receive(ThemeChangeRequestedMessage message)
        {
            var app = Application.Current;
            if (app is not null)
            {
                var theme = app.ActualThemeVariant;
                app.RequestedThemeVariant = theme == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;
            }
        }
    }
}