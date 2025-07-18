using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BudgetPlanner.Client.Controls;

public class HeaderAndValueCard : TemplatedControl
{
    public static readonly DirectProperty<HeaderAndValueCard, string> TitleProperty =
        AvaloniaProperty.RegisterDirect<HeaderAndValueCard, string>(nameof(Title), p => p.Title, (p, v) => p.Title = v);
    
    
    public static readonly DirectProperty<HeaderAndValueCard, string> TextProperty =
        AvaloniaProperty.RegisterDirect<HeaderAndValueCard, string>(nameof(Text), p => p.Text, (p, v) => p.Text = v);

    
    public static readonly DirectProperty<HeaderAndValueCard,string> TextClassesProperty =
        AvaloniaProperty.RegisterDirect<HeaderAndValueCard, string>(nameof(TextClasses), p => p.TextClasses, (p, v) => p.TextClasses = v);
    

    private string _title = "";
    private string _text = "";
    private string _textClasses = "";

    public string Title
    {
        get => _title;
        set => SetAndRaise(TitleProperty, ref _title, value);
    }
    
    public string TextClasses
    {
        get => _textClasses;
        set => SetAndRaise(TextClassesProperty, ref _textClasses, value);
    }
    
    public string Text
    {
        get => _text;
        set => SetAndRaise(TextProperty, ref _text, value);
    }
}