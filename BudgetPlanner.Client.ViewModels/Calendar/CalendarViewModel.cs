using System.Collections.ObjectModel;
using BudgetPlanner.Shared.Models.Response.Calendar;
using BudgetPlanner.Client.Services.Calendar;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.Client.ViewModels;

public partial class CalendarViewModel : PageViewModel
{
    private readonly ICalendarService _calenderService;
    public CalendarViewModel(ICalendarService calenderService)
    {
        _calenderService = calenderService;
        InitialiseAsync();
    }

    [ObservableProperty]
    private DateTimeOffset? _selectedDate;
    
    [ObservableProperty]
    private ObservableCollection<IEnumerable<CalendarItemViewModel>> _calendarMonthItems = [];

    [RelayCommand]
    public async Task RefreshCalendarAsync()
    {
        SetLoading(true, "Loading selected month");
        await RunOnBackgroundThreadAsync(async () => await LoadSelectedCalendarMonthAsync());
        SetLoading(false);
    }
    
    private async void InitialiseAsync()
    {
        SetLoading(true, "Loading current month");
        SelectedDate = DateTimeOffset.Now;
        await RunOnBackgroundThreadAsync(async () => await LoadSelectedCalendarMonthAsync());

        SetLoading(false);
    }

    private async Task LoadSelectedCalendarMonthAsync()
    {
        var calenderItems = new List<CalendarItemViewModel>();
        CalendarMonthItems.Clear();
        await foreach (var item in _calenderService.GetMonthItemsAsync(SelectedDate!.Value.Month, SelectedDate!.Value.Year))
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
    }
}