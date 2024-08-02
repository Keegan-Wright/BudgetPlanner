using Avalonia;
using Avalonia.Controls;
using BudgetPlanner.ViewModels;

namespace BudgetPlanner.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();

#endif
        }
    }
}