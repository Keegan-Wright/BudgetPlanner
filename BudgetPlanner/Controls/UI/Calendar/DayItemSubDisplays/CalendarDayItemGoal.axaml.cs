using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using BudgetPlanner.ViewModels;

namespace BudgetPlanner.Controls;

public class CalendarDayItemGoal : TemplatedControl
{
    public static readonly DirectProperty<CalendarDayItemGoal, CalendarGoalItemViewModel> GoalProperty =
        AvaloniaProperty.RegisterDirect<CalendarDayItemGoal, CalendarGoalItemViewModel>(nameof(Goal),
            p => p.Goal, (p, v) => p.Goal = v);
    
    private CalendarGoalItemViewModel _goal = new CalendarGoalItemViewModel();
    
    public CalendarGoalItemViewModel Goal
    {
        get => _goal;
        set => SetAndRaise(GoalProperty, ref _goal, value);
    }
}