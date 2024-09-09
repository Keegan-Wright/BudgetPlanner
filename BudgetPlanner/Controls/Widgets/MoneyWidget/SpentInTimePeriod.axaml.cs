using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using BudgetPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BudgetPlanner.Controls;

public class SpentInTimePeriod : TemplatedControl
{
    public static readonly DirectProperty<SpentInTimePeriod, decimal> TotalSpentProperty =
        AvaloniaProperty.RegisterDirect<SpentInTimePeriod, decimal>(nameof(TotalSpent), p => p.TotalSpent, (p, v) => p.TotalSpent = v);

    public static readonly DirectProperty<SpentInTimePeriod, string> TitleProperty =
        AvaloniaProperty.RegisterDirect<SpentInTimePeriod, string>(nameof(Title), p => p.Title, (p, v) => p.Title = v);

    private decimal _totalSpent = new decimal();
    private string _title = "";

    public string FormattedTotalSpent => $"£{TotalSpent} {Title}";

    public decimal TotalSpent
    {
        get => _totalSpent;
        set => SetAndRaise(TotalSpentProperty, ref _totalSpent, value);
    }

    public string Title
    {
        get => _title;
        set => SetAndRaise(TitleProperty, ref _title, value);
    }
}