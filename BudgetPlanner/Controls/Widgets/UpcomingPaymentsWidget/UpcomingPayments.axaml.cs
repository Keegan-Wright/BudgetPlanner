using Avalonia;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Controls;
using BudgetPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetPlanner.Controls;

public class UpcomingPayments : TemplatedControl
{
    public static readonly DirectProperty<UpcomingPayments, IEnumerable<UpcomingPaymentViewModel>?> PaymentsProperty =
        AvaloniaProperty.RegisterDirect<UpcomingPayments, IEnumerable<UpcomingPaymentViewModel>?>(nameof(Payments), p => p.Payments, (p, v) => p.Payments = v);

    private IEnumerable<UpcomingPaymentViewModel> _payments = new List<UpcomingPaymentViewModel>();
    public IEnumerable<UpcomingPaymentViewModel> Payments
    {
        get => _payments;
        set => SetAndRaise(PaymentsProperty, ref _payments, value);
    }


}