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
using BudgetPlanner.Client.ViewModels;


namespace BudgetPlanner.Client.Controls;


public class ValidationContainer : UserControl
{

    public static readonly StyledProperty<int> ScrollViewerMaxHeightProperty =
        AvaloniaProperty.Register<ValidationContainer, int>(nameof(ScrollViewerMaxHeight));

    public int ScrollViewerMaxHeight
    {
        get => GetValue(ScrollViewerMaxHeightProperty);
        set => SetValue(ScrollViewerMaxHeightProperty, value);
    }


    public static readonly StyledProperty<int> ScrollViewerHeightProperty =
        AvaloniaProperty.Register<ValidationContainer, int>(nameof(ScrollViewerHeight));

    public int ScrollViewerHeight
    {
        get => GetValue(ScrollViewerHeightProperty);
        set => SetValue(ScrollViewerHeightProperty, value);
    }


    public static readonly StyledProperty<ObservableCollection<string>?> ValidationErrorsProperty =
        AvaloniaProperty.Register<ValidationContainer, ObservableCollection<string>?>(nameof(ValidationErrors));

    public ObservableCollection<string>? ValidationErrors
    {
        get => GetValue(ValidationErrorsProperty);
        set => SetValue(ValidationErrorsProperty, value);
    }


    public void RemoveValidationItem(string validationItem)
    {
        if(ValidationErrors.Contains(validationItem))
            ValidationErrors.Remove(validationItem);
    }

    static ValidationContainer()
    {
    }
}