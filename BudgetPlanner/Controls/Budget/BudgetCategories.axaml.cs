using Avalonia;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Controls;
using BudgetPlanner.ViewModels;
using System.Collections.Generic;

namespace BudgetPlanner.Controls;

public class BudgetCategories : TemplatedControl
{
    public static readonly DirectProperty<BudgetCategories, IEnumerable<BudgetCategoryListItemViewModel>> CategoriesProperty =
AvaloniaProperty.RegisterDirect<BudgetCategories, IEnumerable<BudgetCategoryListItemViewModel>>(nameof(Categories), p => p.Categories, (p, v) => p.Categories = v);

    private IEnumerable<BudgetCategoryListItemViewModel> _categories = new List<BudgetCategoryListItemViewModel>();
    public IEnumerable<BudgetCategoryListItemViewModel> Categories
    {
        get => _categories;
        set => SetAndRaise(CategoriesProperty, ref _categories, value);
    }

}