using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Reports;
using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Response.Reports;

namespace BudgetPlanner.Client.ViewModels;

public partial class SpentInTimePeriodReportViewModel : BaseReportPageViewModel<SpentInTimePeriodReportResponse>
{
    public SpentInTimePeriodReportViewModel(IReportsService reportsService, INavigationService navigationService) : base(reportsService, navigationService)
    {

    }

    protected override async Task LoadReportOptionsAsync()
    {
        //throw new NotImplementedException();
    }

    protected override async Task LoadReportAsync()
    {
        SetLoading(true, "testing");
         await foreach (var reportItem in _reportsService.GetSpentInTimePeriodReportAsync(new BaseReportRequest(){SyncTypes = SyncTypes.All, FromDate = DateTime.Now.AddYears(-1).ToUniversalTime(), ToDate = DateTime.Now.ToUniversalTime()}))
         {
             ReportItems.Add(reportItem);
         }
         SetLoading(false);
    }
}