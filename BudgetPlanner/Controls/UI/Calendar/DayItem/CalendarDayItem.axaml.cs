using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using BudgetPlanner.ViewModels;

namespace BudgetPlanner.Controls;

public class CalendarDayItem : TemplatedControl
{
    public static readonly DirectProperty<CalendarDayItem, CalendarItemViewModel> DayProperty =
        AvaloniaProperty.RegisterDirect<CalendarDayItem, CalendarItemViewModel>(nameof(Day),
            p => p.Day, (p, v) => p.Day = v);
    
    private CalendarItemViewModel _day = new CalendarItemViewModel();
    
    public CalendarItemViewModel Day
    {
        get => _day;
        set => SetAndRaise(DayProperty, ref _day, value);
    }
}