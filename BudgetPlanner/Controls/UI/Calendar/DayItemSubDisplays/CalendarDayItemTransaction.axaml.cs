using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using BudgetPlanner.ViewModels;

namespace BudgetPlanner.Controls;

public class CalendarDayItemTransaction : TemplatedControl
{
    public static readonly DirectProperty<CalendarDayItemTransaction, CalendarTransactionItemViewModel> TransactionProperty =
        AvaloniaProperty.RegisterDirect<CalendarDayItemTransaction, CalendarTransactionItemViewModel>(nameof(Transaction),
            p => p.Transaction, (p, v) => p.Transaction = v);
    
    private CalendarTransactionItemViewModel _transaction = new CalendarTransactionItemViewModel();
    
    public CalendarTransactionItemViewModel Transaction
    {
        get => _transaction;
        set => SetAndRaise(TransactionProperty, ref _transaction, value);
    }
}