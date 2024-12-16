using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using BudgetPlanner.Client.ViewModels;

namespace BudgetPlanner.Client.Controls;

public class CalendarMonthItem : TemplatedControl
{
    public static readonly DirectProperty<CalendarMonthItem, IEnumerable<IEnumerable<CalendarItemViewModel>>> WeeksInMonthProperty =
        AvaloniaProperty.RegisterDirect<CalendarMonthItem, IEnumerable<IEnumerable<CalendarItemViewModel>>>(nameof(WeeksInMonth),
            p => p.WeeksInMonth, (p, v) => p.WeeksInMonth = v);
    
    private IEnumerable<IEnumerable<CalendarItemViewModel>> _weeksInMonth = new List<IEnumerable<CalendarItemViewModel>>();
    
    public IEnumerable<IEnumerable<CalendarItemViewModel>> WeeksInMonth
    {
        get => _weeksInMonth;
        set => SetAndRaise(WeeksInMonthProperty, ref _weeksInMonth, value);
    }
}