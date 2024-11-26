using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using BudgetPlanner.ViewModels;

namespace BudgetPlanner.Controls;

public class CalenderWeekItem : TemplatedControl
{
    
    public static readonly DirectProperty<CalenderWeekItem, IEnumerable<CalendarItemViewModel>> WeekItemsProperty =
        AvaloniaProperty.RegisterDirect<CalenderWeekItem, IEnumerable<CalendarItemViewModel>>(nameof(WeekItems),
            p => p.WeekItems, (p, v) => p.WeekItems = v);
    
    private IEnumerable<CalendarItemViewModel> _weekItems = new List<CalendarItemViewModel>();
    
    public IEnumerable<CalendarItemViewModel> WeekItems
    {
        get => _weekItems;
        set => SetAndRaise(WeekItemsProperty, ref _weekItems, value);
    }
}