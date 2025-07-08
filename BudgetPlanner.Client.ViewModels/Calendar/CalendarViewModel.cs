using System.Collections.ObjectModel;
using BudgetPlanner.Shared.Models.Response.Calendar;
using BudgetPlanner.Client.Services.Calendar;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.Client.ViewModels;

public partial class CalendarViewModel : PageViewModel
{
    private readonly ICalendarRequestService _calenderRequestService;
    public CalendarViewModel(ICalendarRequestService calenderRequestService)
    {
        _calenderRequestService = calenderRequestService;
    }

    [ObservableProperty]
    private DateTimeOffset? _selectedDate;
    
    [ObservableProperty]
    private ObservableCollection<IEnumerable<CalendarItemViewModel>> _calendarMonthItems = [];

    [RelayCommand]
    private async Task RefreshCalendarAsync()
    {
        await RunOnBackgroundThreadAsync(LoadSelectedCalendarMonthAsync());
    }
    
    [RelayCommand]
    private async Task InitialiseAsync()
    {
        await RunOnBackgroundThreadAsync(LoadSelectedCalendarMonthAsync());
    }

    private async Task LoadSelectedCalendarMonthAsync()
    {
        SetLoading(true, "Loading calender month");
        SelectedDate = DateTimeOffset.Now;
        
        var calenderItems = new List<CalendarItemViewModel>();
        CalendarMonthItems.Clear();
        await foreach (var item in _calenderRequestService.GetMonthItemsAsync(SelectedDate!.Value.Month, SelectedDate!.Value.Year))
        {
            var calenderItem = new CalendarItemViewModel
            {
                Date = item.Date
            };

            foreach (var goal in item.Goals)
            {
                calenderItem.Goals.Add(new CalendarGoalItemViewModel()
                {
                    Name = goal.Name,
                    GoalCompletionDate = goal.GoalCompletionDate
                });
            }
            
            foreach (var calendarEvent in item.Events)
            {
                calenderItem.Events.Add(new CalendarEventItemViewModel()
                {
                    
                });
            }
            
            foreach (var transaction in item.Transactions)
            {
                calenderItem.Transactions.Add(new CalendarTransactionItemViewModel()
                {
                    Amount = transaction.Amount,
                    Description = transaction.Description,
                    TransactionTime = transaction.TransactionTime,
                    TransactionType = transaction.TransactionType
                });
            }
            
            
            calenderItems.Add(calenderItem);
        }

        foreach (var item in calenderItems.Chunk(7))
        {
            CalendarMonthItems.Add(item);
        }
        
        SetLoading(false);
    }
}