using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class DashboardViewModel : PageViewModel
    {
        private readonly IDashboardService _dashboardService;

        public DashboardViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            InitialiseAsync();
        }

        private async void InitialiseAsync()
        {
            SetLoading(true, "Loading Dashboard Data");

            await LoadUpcomingPayments();
            await LoadSpendingPeriods();
            SetLoading(false);
        }
  
        private async Task LoadSpendingPeriods()
        {
            var today = DateTime.Today;

            var spentToday = await _dashboardService.GetSpentInTimePeriod(today);
            var spentThisWeek = await _dashboardService.GetSpentInTimePeriod(today.StartOfWeek(DayOfWeek.Monday), today);
            var spentThisMonth = await _dashboardService.GetSpentInTimePeriod(today.StartOfMonth(), today);
            var spentThisYear = await _dashboardService.GetSpentInTimePeriod(today.StartOfYear(), today);

            SpentToday = new SpentInTimePeriodWidgetViewModel() { TotalIn = spentToday.TotalIn, TotalOut = spentToday.TotalOut };
            SpentThisWeek = new SpentInTimePeriodWidgetViewModel() { TotalIn = spentThisWeek.TotalIn, TotalOut = spentThisWeek.TotalOut };
            SpentThisMonth = new SpentInTimePeriodWidgetViewModel() { TotalIn = spentThisMonth.TotalIn, TotalOut = spentThisMonth.TotalOut };
            SpentThisYear = new SpentInTimePeriodWidgetViewModel() { TotalIn = spentThisYear.TotalIn, TotalOut = spentThisYear.TotalOut };
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
        private SpentInTimePeriodWidgetViewModel _spentToday;
        
        [ObservableProperty]
        private SpentInTimePeriodWidgetViewModel _spentThisWeek;
        
        [ObservableProperty]
        private SpentInTimePeriodWidgetViewModel _spentThisMonth;
        
        [ObservableProperty]
        private SpentInTimePeriodWidgetViewModel _spentThisYear;
    }
}