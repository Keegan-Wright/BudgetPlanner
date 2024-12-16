using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class HouseholdMemberListItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _firstName;

        [ObservableProperty]
        private string? _lastName;

        [ObservableProperty]
        private decimal? _income;


    }
}
