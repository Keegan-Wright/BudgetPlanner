using Avalonia;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.Controls;
using BudgetPlanner.Client.ViewModels;
using System.Collections.Generic;

namespace BudgetPlanner.Client.Controls;

public class IndeterminateLoadingSpinner : TemplatedControl
{
    public static readonly DirectProperty<IndeterminateLoadingSpinner, bool> LoadingProperty =
        AvaloniaProperty.RegisterDirect<IndeterminateLoadingSpinner, bool>(nameof(Loading), p => p.Loading, (p, v) => p.Loading = v);

    private bool _loading = false;
    public bool Loading
    {
        get => _loading;
        set => SetAndRaise(LoadingProperty, ref _loading, value);
    }


    public static readonly StyledProperty<string> LoadingMessageProperty = AvaloniaProperty.Register<IndeterminateLoadingSpinner, string>(nameof(LoadingMessage));
    public string LoadingMessage
    {
        get => GetValue(LoadingMessageProperty);
        set => SetValue(LoadingMessageProperty, value);
    }

}