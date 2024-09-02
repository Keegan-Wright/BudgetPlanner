using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using BudgetPlanner.ViewModels;
using System.Collections.Generic;

namespace BudgetPlanner.Controls;

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


    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DataContext)
        {
            Payment = change.GetNewValue<UpcomingPaymentViewModel>();
        }
    }
}