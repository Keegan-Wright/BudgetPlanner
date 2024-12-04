using Avalonia;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.ViewModels;

namespace BudgetPlanner.Client.Controls;

public class BudgetCategory : TemplatedControl
{
    public static readonly DirectProperty<BudgetCategory, BudgetCategoryListItemViewModel> CategoryProperty =
AvaloniaProperty.RegisterDirect<BudgetCategory, BudgetCategoryListItemViewModel>(nameof(Category), p => p.Category, (p, v) => p.Category = v);

    private BudgetCategoryListItemViewModel _category = new BudgetCategoryListItemViewModel();
    public BudgetCategoryListItemViewModel Category
    {
        get => _category;
        set => SetAndRaise(CategoryProperty, ref _category, value);
    }

}