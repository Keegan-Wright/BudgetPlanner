using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.ViewModels;

namespace BudgetPlanner.Client.Controls;

public class CalendarDayItemEvent : TemplatedControl
{
    public static readonly DirectProperty<CalendarDayItemEvent, CalendarEventItemViewModel> EventProperty =
        AvaloniaProperty.RegisterDirect<CalendarDayItemEvent, CalendarEventItemViewModel>(nameof(Event),
            p => p.Event, (p, v) => p.Event = v);
    
    private CalendarEventItemViewModel _event = new CalendarEventItemViewModel();
    
    public CalendarEventItemViewModel Event
    {
        get => _event;
        set => SetAndRaise(EventProperty, ref _event, value);
    }
}