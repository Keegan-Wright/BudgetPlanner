using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Selection;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using Avalonia.Metadata;
using Avalonia.Reactive;
using Avalonia.VisualTree;
using BudgetPlanner.ViewModels;


namespace BudgetPlanner.Controls;


public class ValidationContainerItem : UserControl
{
    public static readonly StyledProperty<string?> ValidationErrorProperty =
    AvaloniaProperty.Register<ValidationContainerItem, string?>(nameof(ValidationError));

    public string? ValidationError
    {
        get => GetValue(ValidationErrorProperty);
        set => SetValue(ValidationErrorProperty, value);
    }

    static ValidationContainerItem()
    {
    }
}