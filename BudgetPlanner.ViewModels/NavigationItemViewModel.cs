using BudgetPlanner.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.ViewModels
{
    public partial class NavigationItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _displayName;

        [ObservableProperty]
        private AppRoutes? _routeType;

        [ObservableProperty]
        private string _iconCode;

        [ObservableProperty]
        private ICollection<NavigationItemViewModel> _subItems;
    }
}
