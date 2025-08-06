using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace BudgetPlanner.Client.Controls;

public class SpentInTimePeriod : TemplatedControl
{
    public static readonly DirectProperty<SpentInTimePeriod, decimal> TotalOutProperty =
        AvaloniaProperty.RegisterDirect<SpentInTimePeriod, decimal>(nameof(TotalOut), p => p.TotalOut, (p, v) => p.TotalOut = v);

    public static readonly DirectProperty<SpentInTimePeriod, decimal> TotalInProperty =
    AvaloniaProperty.RegisterDirect<SpentInTimePeriod, decimal>(nameof(TotalIn), p => p.TotalIn, (p, v) => p.TotalIn = v);

    public static readonly DirectProperty<SpentInTimePeriod, decimal> NormalisedProperty =
AvaloniaProperty.RegisterDirect<SpentInTimePeriod, decimal>(nameof(Normalised), p => p.Normalised, (p, v) => p.Normalised = v);


    public static readonly DirectProperty<SpentInTimePeriod, string> TitleProperty =
        AvaloniaProperty.RegisterDirect<SpentInTimePeriod, string>(nameof(Title), p => p.Title, (p, v) => p.Title = v);
 
  
    

    private decimal _totalOut = new decimal();
    private decimal _totalIn = new decimal();
    private decimal _normalised = new decimal();
    private string _title = "";


    public decimal TotalOut
    {
        get => _totalOut;
        set
        {
            SetAndRaise(TotalOutProperty, ref _totalOut, value);
        }
    }

    public decimal TotalIn
    {
        get => _totalIn;
        set
        {
            SetAndRaise(TotalInProperty, ref _totalIn, value);
        }
    }
    

    public decimal Normalised
    {
        get => _normalised;
        set
        {
            SetAndRaise(NormalisedProperty, ref _normalised, value);
        }
    }

    public string Title
    {
        get => _title;
        set => SetAndRaise(TitleProperty, ref _title, value);
    }
}