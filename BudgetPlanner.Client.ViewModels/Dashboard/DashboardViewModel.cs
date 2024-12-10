using BudgetPlanner.Shared.Extensions;
using BudgetPlanner.Client.Services.Dashboard;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class DashboardViewModel : PageViewModel
    {
        private readonly IDashboardRequestService _dashboardRequestService;

        public DashboardViewModel(IDashboardRequestService dashboardRequestService)
        {
            _dashboardRequestService = dashboardRequestService;
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

            var spentToday = await _dashboardRequestService.GetSpentInTimePeriodAsync(today);
            var spentThisWeek = await _dashboardRequestService.GetSpentInTimePeriodAsync(today.StartOfWeek(DayOfWeek.Monday), today);
            var spentThisMonth = await _dashboardRequestService.GetSpentInTimePeriodAsync(today.StartOfMonth(), today);
            var spentThisYear = await _dashboardRequestService.GetSpentInTimePeriodAsync(today.StartOfYear(), today);

            SpentToday = new SpentInTimePeriodWidgetViewModel() { TotalIn = spentToday.TotalIn, TotalOut = spentToday.TotalOut };
            SpentThisWeek = new SpentInTimePeriodWidgetViewModel() { TotalIn = spentThisWeek.TotalIn, TotalOut = spentThisWeek.TotalOut };
            SpentThisMonth = new SpentInTimePeriodWidgetViewModel() { TotalIn = spentThisMonth.TotalIn, TotalOut = spentThisMonth.TotalOut };
            SpentThisYear = new SpentInTimePeriodWidgetViewModel() { TotalIn = spentThisYear.TotalIn, TotalOut = spentThisYear.TotalOut };
        }

        private async Task LoadUpcomingPayments()
        {
            UpcomingPayments = new UpcomingPaymentsWidgetViewModel();

            await foreach (var upcomingPayment in _dashboardRequestService.GetUpcomingPaymentsAsync(5))
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