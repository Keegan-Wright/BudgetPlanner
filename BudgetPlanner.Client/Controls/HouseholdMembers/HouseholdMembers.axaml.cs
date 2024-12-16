using Avalonia;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.Controls;
using BudgetPlanner.Client.ViewModels;
using System.Collections.Generic;

namespace BudgetPlanner.Client.Controls;

public class HouseholdMembers : TemplatedControl
{
    public static readonly DirectProperty<HouseholdMembers, IEnumerable<HouseholdMemberListItemViewModel>> MembersProperty =
AvaloniaProperty.RegisterDirect<HouseholdMembers, IEnumerable<HouseholdMemberListItemViewModel>>(nameof(Members), p => p.Members, (p, v) => p.Members = v);

    private IEnumerable<HouseholdMemberListItemViewModel> _members = new List<HouseholdMemberListItemViewModel>();
    public IEnumerable<HouseholdMemberListItemViewModel> Members
    {
        get => _members;
        set => SetAndRaise(MembersProperty, ref _members, value);
    }

}