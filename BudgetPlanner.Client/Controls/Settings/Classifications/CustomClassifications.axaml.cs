using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using BudgetPlanner.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace BudgetPlanner.Client.Controls;

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
    
    public static readonly DirectProperty<CustomClassifications, ClassificationItemViewModel?> SelectedItemProperty =
        AvaloniaProperty.RegisterDirect<CustomClassifications, ClassificationItemViewModel?>(nameof(SelectedItem), p => p.SelectedItem, (p, v) => p.SelectedItem = v);

    private ClassificationItemViewModel? _selectedItem;
    public ClassificationItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set => SetAndRaise(SelectedItemProperty, ref _selectedItem, value);
    }
    
    public static readonly DirectProperty<CustomClassifications, ICommand?> DeleteCustomClassificationCommandProperty =
        AvaloniaProperty.RegisterDirect<CustomClassifications, ICommand?>(nameof(DeleteCustomClassificationCommand), p => p.DeleteCustomClassificationCommand, (p, v) => p.DeleteCustomClassificationCommand = v);

    private ICommand? _deleteCustomClassificationCommand;
    public ICommand? DeleteCustomClassificationCommand
    {
        get => _deleteCustomClassificationCommand;
        set => SetAndRaise(DeleteCustomClassificationCommandProperty, ref _deleteCustomClassificationCommand, value);
    }


}