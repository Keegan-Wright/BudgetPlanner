using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetPlanner.Extensions;
using BudgetPlanner.Services.Dashboard;
using BudgetPlanner.Services.OpenBanking;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.ViewModels
{
    public partial class DashboardViewModel : ViewModelBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            InitialiseAsync();
        }

        private async void InitialiseAsync()
        {
            SetLoading(true, "Loading Dashboard Data...");

            await LoadUpcomingPayments();
            await LoadSpendingPeriods();
            SetLoading(false);
        }

        private async Task LoadSpendingPeriods()
        {
            var today = DateTime.Today;
            SpentToday = await _dashboardService.GetSpentInTimePeriod(today);
            SpentThisWeek = await _dashboardService.GetSpentInTimePeriod(today.StartOfWeek(DayOfWeek.Monday), today);
            SpentThisMonth = await _dashboardService.GetSpentInTimePeriod(today.StartOfMonth(), today);
            SpentThisYear = await _dashboardService.GetSpentInTimePeriod(today.StartOfYear(), today);
        }

        private async Task LoadUpcomingPayments()
        {
            UpcomingPayments = new UpcomingPaymentsWidgetViewModel();

            await foreach (var upcomingPayment in _dashboardService.GetUpcomingPaymentsAsync(5))
            {
                UpcomingPayments.Payments.Add(new UpcomingPaymentViewModel()
                {
                    Amount = upcomingPayment.Amount,
                    PaymentDate = upcomingPayment.PaymentDate,
                    PaymentName = upcomingPayment.PaymentName,
                    PaymentType = upcomingPayment.PaymentType
                });
            }
        }

        [ObservableProperty]
        private string _title = "Dashboard";

        [ObservableProperty]
        private UpcomingPaymentsWidgetViewModel _upcomingPayments;

        [ObservableProperty]
        private decimal _spentToday;
        
        [ObservableProperty]
        private decimal _spentThisWeek;
        
        [ObservableProperty]
        private decimal _spentThisMonth;
        
        [ObservableProperty]
        private decimal _spentThisYear;

    }
}