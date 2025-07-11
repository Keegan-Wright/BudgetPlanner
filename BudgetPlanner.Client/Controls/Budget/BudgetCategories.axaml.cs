using Avalonia;
using Avalonia.Controls.Primitives;
using BudgetPlanner.Client.Controls;
using BudgetPlanner.Client.ViewModels;
using System.Collections.Generic;

namespace BudgetPlanner.Client.Controls;

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