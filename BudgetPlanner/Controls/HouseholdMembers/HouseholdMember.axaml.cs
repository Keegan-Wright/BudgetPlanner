using Avalonia;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Controls;
using BudgetPlanner.ViewModels;

namespace BudgetPlanner.Controls;

public class HouseholdMember : TemplatedControl
{
    public static readonly DirectProperty<HouseholdMember, HouseholdMemberListItemViewModel> MemberProperty =
AvaloniaProperty.RegisterDirect<HouseholdMember, HouseholdMemberListItemViewModel>(nameof(Member), p => p.Member, (p, v) => p.Member = v);

    private HouseholdMemberListItemViewModel _member = new HouseholdMemberListItemViewModel();
    public HouseholdMemberListItemViewModel Member
    {
        get => _member;
        set => SetAndRaise(MemberProperty, ref _member, value);
    }
}