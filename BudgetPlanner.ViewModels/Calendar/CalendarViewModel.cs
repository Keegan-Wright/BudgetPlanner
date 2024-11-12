using BudgetPlanner.Models.Response.Calendar;
using BudgetPlanner.Services.Calendar;

namespace BudgetPlanner.ViewModels;

public partial class CalendarViewModel : PageViewModel
{
    private readonly ICalendarService _calenderService;
    public CalendarViewModel(ICalendarService calenderService)
    {
        _calenderService = calenderService;
        InitialiseAsync();
    }
    
    private async void InitialiseAsync()
    {
        SetLoading(true, "Loading current month");

        await RunOnBackgroundThreadAsync(async () => await LoadCurrentMonthAsync());

        SetLoading(false);
    }

    private async Task LoadCurrentMonthAsync()
    {
        var monthItems = new List<CalendarItemsResponse>();

        await foreach (var item in _calenderService.GetMonthItemsAsync(DateTime.Now.Month, DateTime.Now.Year))
        {
            monthItems.Add(item);
        }

        

    }
}