using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using BudgetPlanner.ViewModels;
using System;
using System.Collections.Generic;

namespace BudgetPlanner.Controls;

public class CustomClassifications : TemplatedControl
{

    public static readonly DirectProperty<CustomClassifications, IEnumerable<ClassificationItemViewModel>> ClassificationsProperty =
AvaloniaProperty.RegisterDirect<CustomClassifications, IEnumerable<ClassificationItemViewModel>>(nameof(Classifications), p => p.Classifications, (p, v) => p.Classifications = v);

    private IEnumerable<ClassificationItemViewModel> _classifications = new List<ClassificationItemViewModel>();
    public IEnumerable<ClassificationItemViewModel> Classifications
    {
        get => _classifications;
        set => SetAndRaise(ClassificationsProperty, ref _classifications, value);
    }
}