using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.ViewModels;
using System.Collections.Generic;

namespace BudgetPlanner.Client.Controls;

public class UpcomingPayment : TemplatedControl
{
    public static readonly DirectProperty<UpcomingPayment, UpcomingPaymentViewModel> PaymentProperty =
        AvaloniaProperty.RegisterDirect<UpcomingPayment, UpcomingPaymentViewModel>(nameof(Payment), p => p.Payment, (p, v) => p.Payment = v);

    private UpcomingPaymentViewModel _payment = new UpcomingPaymentViewModel();
    public UpcomingPaymentViewModel Payment
    {
        get => _payment;
        set => SetAndRaise(PaymentProperty, ref _payment, value);
    }
}