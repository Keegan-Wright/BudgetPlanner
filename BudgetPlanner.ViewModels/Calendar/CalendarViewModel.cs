using System.Collections.ObjectModel;
using BudgetPlanner.Models.Response.Calendar;
using BudgetPlanner.Services.Calendar;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels;

public partial class CalendarViewModel : PageViewModel
{
    private readonly ICalendarService _calenderService;
    public CalendarViewModel(ICalendarService calenderService)
    {
        _calenderService = calenderService;
        InitialiseAsync();
    }

    [ObservableProperty] private ObservableCollection<IEnumerable<CalendarItemViewModel>> _calendarMonthItems = [];
    
    
    private async void InitialiseAsync()
    {
        SetLoading(true, "Loading current month");

        await RunOnBackgroundThreadAsync(async () => await LoadCurrentMonthAsync());

        SetLoading(false);
    }

    private async Task LoadCurrentMonthAsync()
    {
        var calenderItems = new List<CalendarItemViewModel>();
        await foreach (var item in _calenderService.GetMonthItemsAsync(DateTime.Now.Month, DateTime.Now.Year))
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