using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Reports;
using BudgetPlanner.Shared.Enums;
using BudgetPlanner.Shared.Models.Request.Reports;
using BudgetPlanner.Shared.Models.Response.Reports;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels;

public partial class SpentInTimePeriodReportViewModel : BaseReportPageViewModel<SpentInTimePeriodReportResponse>
{

    [ObservableProperty]
    private decimal _totalIn;
    
    [ObservableProperty]
    private decimal _totalOut;
    
    [ObservableProperty]
    private decimal _totalDif;
    
    
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
         
         TotalIn = ReportItems.Sum(x => x.TotalIn);
         TotalOut = ReportItems.Sum(x => x.TotalOut);
         TotalDif = decimal.Add(TotalOut, TotalIn);
         
         SetLoading(false);
    }
}