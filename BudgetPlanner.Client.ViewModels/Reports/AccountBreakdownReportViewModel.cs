using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Reports;
using BudgetPlanner.Client.Services.Transactions;
using BudgetPlanner.Shared.Models.Request.Transaction;
using BudgetPlanner.Shared.Models.Response.Reports;

namespace BudgetPlanner.Client.ViewModels;

public partial class AccountBreakdownReportViewModel : BaseReportPageViewModel<AccountBreakdownReportResponse>
{
    public AccountBreakdownReportViewModel(IReportsService reportsService, INavigationService navigationService, ITransactionsRequestService transactionsRequestService) : base(reportsService, navigationService, transactionsRequestService)
    {
    }

    public override Task LoadReportAsync(FilteredTransactionsRequest searchCriteria)
    {
        throw new NotImplementedException();
    }
}