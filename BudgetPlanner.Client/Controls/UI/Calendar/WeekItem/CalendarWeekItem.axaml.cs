using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.ViewModels;

namespace BudgetPlanner.Client.Controls;

public class CalendarWeekItem : TemplatedControl
{
    
    public static readonly DirectProperty<CalendarWeekItem, IEnumerable<CalendarItemViewModel>> WeekItemsProperty =
        AvaloniaProperty.RegisterDirect<CalendarWeekItem, IEnumerable<CalendarItemViewModel>>(nameof(WeekItems),
            p => p.WeekItems, (p, v) => p.WeekItems = v);
    
    private IEnumerable<CalendarItemViewModel> _weekItems = new List<CalendarItemViewModel>();
    
    public IEnumerable<CalendarItemViewModel> WeekItems
    {
        get => _weekItems;
        set => SetAndRaise(WeekItemsProperty, ref _weekItems, value);
    }
}