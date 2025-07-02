using System.Collections.ObjectModel;
using BudgetPlanner.Client.Services;
using BudgetPlanner.Client.Services.Reports;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.Client.ViewModels;

public abstract partial class BaseReportPageViewModel<TReportResponse> : PageViewModel
{
    internal readonly IReportsService _reportsService;
    internal readonly INavigationService _navigationService;
    
    
    
    protected BaseReportPageViewModel(IReportsService reportsService, INavigationService navigationService)
    {
        _reportsService = reportsService;
        _navigationService = navigationService;
        
        InitialiseAsync();
    }
    
    
    
    [ObservableProperty]
    private ObservableCollection<TReportResponse> _reportItems = [];
    
    [ObservableProperty]
    private DateTime? _dateFrom;
    
    [ObservableProperty]
    private DateTime? _dateTo;
    
    
    
    
    
    private async void InitialiseAsync()
    {
        SetLoading(true, "Loading report options");
        await RunOnBackgroundThreadAsync(async () => await LoadReportOptionsAsync());

        SetLoading(false);
    }

    
    protected abstract Task LoadReportOptionsAsync();
    
    [RelayCommand]
    protected abstract Task LoadReportAsync();
}