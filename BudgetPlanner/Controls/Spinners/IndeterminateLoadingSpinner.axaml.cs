using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Runtime.CompilerServices;

namespace BudgetPlanner.Controls;

public partial class IndeterminateLoadingSpinner : UserControl
{
    public static readonly AttachedProperty<bool> LoadingProperty = AvaloniaProperty.RegisterAttached<IndeterminateLoadingSpinner, bool>(nameof(Loading), typeof(IndeterminateLoadingSpinner));
    public bool Loading { get; set; }


    public static readonly AttachedProperty<string> LoadingMessageProperty = AvaloniaProperty.RegisterAttached<IndeterminateLoadingSpinner, string>(nameof(LoadingMessage), typeof(IndeterminateLoadingSpinner));
    public bool LoadingMessage { get; set; }

    public IndeterminateLoadingSpinner()
    {
        InitializeComponent();
    }
}