using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Reports;
using BudgetPlanner.Shared.Models.Response.Reports;

namespace BudgetPlanner.Client.ViewModels;

public partial class AccountBreakdownReportViewModel : BaseReportPageViewModel<AccountBreakdownReportResponse>
{
    public AccountBreakdownReportViewModel(IReportsService reportsService, INavigationService navigationService) : base(reportsService, navigationService)
    {
    }

    protected override Task LoadReportOptionsAsync()
    {
        throw new NotImplementedException();
    }

    protected override Task LoadReportAsync()
    {
        throw new NotImplementedException();
    }
}