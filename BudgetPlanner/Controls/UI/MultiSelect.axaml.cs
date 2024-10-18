using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Selection;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using Avalonia.Metadata;
using Avalonia.Reactive;
using Avalonia.VisualTree;
using BudgetPlanner.ViewModels;


namespace BudgetPlanner.Controls;


[TemplatePart("PART_Popup", typeof(Popup), IsRequired = true)]
[PseudoClasses(pcDropdownOpen, pcPressed)]
public class MultiSelect : SelectingItemsControl
{
    internal const string pcDropdownOpen = ":dropdownopen";
    internal const string pcPressed = ":pressed";

    /// <summary>
    /// The default value for the <see cref="ItemsControl.ItemsPanel"/> property.
    /// </summary>
    private static readonly FuncTemplate<Panel?> DefaultPanel =
        new(() => new VirtualizingStackPanel());

    /// <summary>
    /// Defines the <see cref="IsDropDownOpen"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsDropDownOpenProperty =
        AvaloniaProperty.Register<MultiSelect, bool>(nameof(IsDropDownOpen));

    /// <summary>
    /// Defines the <see cref="MaxDropDownHeight"/> property.
    /// </summary>
    public static readonly StyledProperty<double> MaxDropDownHeightProperty =
        AvaloniaProperty.Register<MultiSelect, double>(nameof(MaxDropDownHeight), 200);

    /// <summary>
    /// Defines the <see cref="SelectionBoxItem"/> property.
    /// </summary>
    public static readonly DirectProperty<MultiSelect, object?> SelectionBoxItemProperty =
        AvaloniaProperty.RegisterDirect<MultiSelect, object?>(nameof(SelectionBoxItem), o => o.SelectionBoxItem);

    /// <summary>
    /// Defines the <see cref="PlaceholderText"/> property.
    /// </summary>
    public static readonly StyledProperty<string?> PlaceholderTextProperty =
        AvaloniaProperty.Register<MultiSelect, string?>(nameof(PlaceholderText));

    /// <summary>
    /// Defines the <see cref="PlaceholderForeground"/> property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
        AvaloniaProperty.Register<MultiSelect, IBrush?>(nameof(PlaceholderForeground));

    /// <summary>
    /// Defines the <see cref="HorizontalContentAlignment"/> property.
    /// </summary>
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        ContentControl.HorizontalContentAlignmentProperty.AddOwner<MultiSelect>();

    /// <summary>
    /// Defines the <see cref="VerticalContentAlignment"/> property.
    /// </summary>
    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
        ContentControl.VerticalContentAlignmentProperty.AddOwner<MultiSelect>();

    /// <summary>
    /// Defines the <see cref="SelectionBoxItemTemplate"/> property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> SelectionBoxItemTemplateProperty =
        AvaloniaProperty.Register<MultiSelect, IDataTemplate?>(
            nameof(SelectionBoxItemTemplate), defaultBindingMode: BindingMode.TwoWay, coerce: CoerceSelectionBoxItemTemplate);


    public static readonly DirectProperty<MultiSelect, IList?> ItemsSelectedProperty =
AvaloniaProperty.RegisterDirect<MultiSelect, IList?>(nameof(ItemsSelected), p => p.ItemsSelected, (p, v) => p.ItemsSelected = v);

    private IList? _itemsSelected;
    public IList? ItemsSelected
    {
        get => _itemsSelected;
        set => SetAndRaise(ItemsSelectedProperty, ref _itemsSelected, value);
    }

    private static IDataTemplate? CoerceSelectionBoxItemTemplate(AvaloniaObject obj, IDataTemplate? template)
    {
        if (template is not null) return template;
        if (obj is MultiSelect MultiSelect && template is null)
        {
            return MultiSelect.ItemTemplate;
        }
        return template;
    }

    private Popup? _popup;
    private object? _selectionBoxItem;
    private readonly CompositeDisposable _subscriptionsOnOpen = new CompositeDisposable();

    /// <summary>
    /// Initializes static members of the <see cref="MultiSelect"/> class.
    /// </summary>

    static MultiSelect()
    {
        ItemsPanelProperty.OverrideDefaultValue<MultiSelect>(DefaultPanel);
        FocusableProperty.OverrideDefaultValue<MultiSelect>(true);
        IsTextSearchEnabledProperty.OverrideDefaultValue<MultiSelect>(true);
        SelectionModeProperty.OverrideDefaultValue<MultiSelect>(SelectionMode.Multiple | SelectionMode.Toggle | SelectionMode.Single);
    }

    /// <summary>
    /// Occurs after the drop-down (popup) list of the <see cref="MultiSelect"/> closes.
    /// </summary>
    public event EventHandler? DropDownClosed;

    /// <summary>
    /// Occurs after the drop-down (popup) list of the <see cref="MultiSelect"/> opens.
    /// </summary>
    public event EventHandler? DropDownOpened;

    /// <summary>
    /// Gets or sets a value indicating whether the dropdown is currently open.
    /// </summary>
    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum height for the dropdown list.
    /// </summary>
    public double MaxDropDownHeight
    {
        get => GetValue(MaxDropDownHeightProperty);
        set => SetValue(MaxDropDownHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the item to display as the control's content.
    /// </summary>
    public object? SelectionBoxItem
    {
        get => _selectionBoxItem;
        protected set => SetAndRaise(SelectionBoxItemProperty, ref _selectionBoxItem, value);
    }

    /// <summary>
    /// Gets or sets the PlaceHolder text.
    /// </summary>
    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the Brush that renders the placeholder text.
    /// </summary>
    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal alignment of the content within the control.
    /// </summary>
    public HorizontalAlignment HorizontalContentAlignment
    {
        get => GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical alignment of the content within the control.
    /// </summary>
    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the DataTemplate used to display the selected item. This has a higher priority than <see cref="ItemsControl.ItemTemplate"/> if set.
    /// </summary>
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IDataTemplate? SelectionBoxItemTemplate
    {
        get => GetValue(SelectionBoxItemTemplateProperty);
        set => SetValue(SelectionBoxItemTemplateProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateSelectionBoxItem(SelectedItem);
    }

    new internal void InvalidateMirrorTransform()
    {
        base.InvalidateMirrorTransform();
        UpdateFlowDirection();
    }

    new protected internal Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MultiSelectItem();
    }

    new protected internal bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MultiSelectItem>(item, out recycleKey);
    }

    /// <inheritdoc/>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Handled)
            return;

        if ((e.Key == Key.F4 && e.KeyModifiers.HasFlag(KeyModifiers.Alt) == false) ||
            ((e.Key == Key.Down || e.Key == Key.Up) && e.KeyModifiers.HasFlag(KeyModifiers.Alt)))
        {
            SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);
            e.Handled = true;
        }
        else if (IsDropDownOpen && e.Key == Key.Escape)
        {
            SetCurrentValue(IsDropDownOpenProperty, false);
            e.Handled = true;
        }
        else if (!IsDropDownOpen && (e.Key == Key.Enter || e.Key == Key.Space))
        {
            SetCurrentValue(IsDropDownOpenProperty, true);
            e.Handled = true;
        }
        else if (IsDropDownOpen && (e.Key == Key.Enter || e.Key == Key.Space))
        {
            SelectFocusedItem();
            SetCurrentValue(IsDropDownOpenProperty, false);
            e.Handled = true;
        }
        // Ignore key buttons, if they are used for XY focus.
        //else if (!IsDropDownOpen
        //         && !XYFocusHelpers.IsAllowedXYNavigationMode(this, e.KeyDeviceType))
        //{
        //    if (e.Key == Key.Down)
        //    {
        //        e.Handled = SelectNext();
        //    }
        //    else if (e.Key == Key.Up)
        //    {
        //        e.Handled = SelectPrevious();
        //    }
        //}
        // This part of code is needed just to acquire initial focus, subsequent focus navigation will be done by ItemsControl.
        else if (IsDropDownOpen && SelectedIndex < 0 && ItemCount > 0 &&
                 (e.Key == Key.Up || e.Key == Key.Down) && IsFocused == true)
        {
            var firstChild = Presenter?.Panel?.Children.FirstOrDefault(c => CanFocus(c));
            if (firstChild != null)
            {
                e.Handled = firstChild.Focus(NavigationMethod.Directional);
            }
        }
    }

    /// <inheritdoc/>
    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        if (!e.Handled)
        {
            if (!IsDropDownOpen)
            {
                if (IsFocused)
                {
                    e.Handled = e.Delta.Y < 0 ? SelectNext() : SelectPrevious();
                }
            }
            else
            {
                e.Handled = true;
            }
        }
    }

    /// <inheritdoc/>
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
            {
                e.Handled = true;
                return;
            }
        }

        if (IsDropDownOpen)
        {
            // When a drop-down is open with OverlayDismissEventPassThrough enabled and the control
            // is pressed, close the drop-down
            SetCurrentValue(IsDropDownOpenProperty, false);
            e.Handled = true;
        }
        else
        {
            PseudoClasses.Set(pcPressed, true);
        }
    }

    /// <inheritdoc/>
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
            {
                if (UpdateSelectionFromEventSource(e.Source))
                {
                    //_popup?.Close();
                    e.Handled = true;

                    var container = GetContainerFromEventSource(source);
                    var child = container.GetVisualChildren().First();
                    SetIsSelected(child as Control, Selection.SelectedItems.Contains(container.DataContext));


                    ItemsSelected = null;
                    ItemsSelected = SelectedItems;
                    

                    UpdateSelectionBoxItem($"{Selection.SelectedItems.Count()} Items selected");
                }
            }
            else if (PseudoClasses.Contains(pcPressed))
            {
                SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);
                e.Handled = true;
            }
        }

        PseudoClasses.Set(pcPressed, false);
        base.OnPointerReleased(e);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        if (_popup != null)
        {
            _popup.Opened -= PopupOpened;
            _popup.Closed -= PopupClosed;
        }

        _popup = e.NameScope.Get<Popup>("PART_Popup");
        _popup.Opened += PopupOpened;
        _popup.Closed += PopupClosed;
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        Debug.WriteLine(change.Property);
        if (change.Property == IsDropDownOpenProperty)
        {
            PseudoClasses.Set(pcDropdownOpen, change.GetNewValue<bool>());
        }
        else if (change.Property == ItemTemplateProperty)
        {
            CoerceValue(SelectionBoxItemTemplateProperty);
        }
        base.OnPropertyChanged(change);
    }


    internal void ItemFocused(MultiSelectItem dropDownItem)
    {
        if (IsDropDownOpen && dropDownItem.IsFocused && dropDownItem.IsArrangeValid)
        {
            dropDownItem.BringIntoView();
        }
    }

    private void PopupClosed(object? sender, EventArgs e)
    {
        _subscriptionsOnOpen.Clear();

        DropDownClosed?.Invoke(this, EventArgs.Empty);
    }

    private void PopupOpened(object? sender, EventArgs e)
    {
        TryFocusSelectedItem();

        _subscriptionsOnOpen.Clear();

        _subscriptionsOnOpen.Add(this.GetObservable(IsVisibleProperty).Subscribe(IsVisibleChanged));

        foreach (var parent in this.GetVisualAncestors().OfType<Control>())
        {
            _subscriptionsOnOpen.Add(parent.GetObservable(IsVisibleProperty).Subscribe(IsVisibleChanged));
        }

        UpdateFlowDirection();

        DropDownOpened?.Invoke(this, EventArgs.Empty);
    }

    private void IsVisibleChanged(bool isVisible)
    {
        if (!isVisible && IsDropDownOpen)
        {
            SetCurrentValue(IsDropDownOpenProperty, false);
        }
    }

    private void TryFocusSelectedItem()
    {
        var selectedIndex = SelectedIndex;
        if (IsDropDownOpen && selectedIndex != -1)
        {
            var container = ContainerFromIndex(selectedIndex);

            if (container == null && SelectedIndex != -1)
            {
                ScrollIntoView(Selection.SelectedIndex);
                container = ContainerFromIndex(selectedIndex);
            }

            if (container != null && CanFocus(container))
            {
                container.Focus();
            }
        }
    }

    private bool CanFocus(Control control) => control.Focusable && control.IsEffectivelyEnabled && control.IsVisible;

    private void UpdateSelectionBoxItem(object? item)
    {
        var contentControl = item as ContentControl;

        if (contentControl != null)
        {
            item = contentControl.Content;
        }

        var control = item as Control;

        if (control != null)
        {
            if (VisualRoot is object)
            {
                control.Measure(Size.Infinity);

                SelectionBoxItem = new Rectangle
                {
                    Width = control.DesiredSize.Width,
                    Height = control.DesiredSize.Height,
                    Fill = new VisualBrush
                    {
                        Visual = control,
                        Stretch = Stretch.None,
                        AlignmentX = AlignmentX.Left,
                    }
                };
            }

            UpdateFlowDirection();
        }
        else
        {
            if (ItemTemplate is null && SelectionBoxItemTemplate is null && DisplayMemberBinding is { } binding)
            {
                var template = new FuncDataTemplate<object?>((_, _) =>
                new TextBlock
                {
                    [TextBlock.DataContextProperty] = item,
                    [!TextBlock.TextProperty] = binding,
                });
                var text = template.Build(item);
                SelectionBoxItem = text;
            }
            else
            {
                SelectionBoxItem = item;
            }

        }
    }

    private void UpdateFlowDirection()
    {
        if (SelectionBoxItem is Rectangle rectangle)
        {
            if ((rectangle.Fill as VisualBrush)?.Visual is Visual content)
            {
                var flowDirection = content.GetVisualParent()?.FlowDirection ?? FlowDirection.LeftToRight;
                rectangle.FlowDirection = flowDirection;
            }
        }
    }

    private void SelectFocusedItem()
    {
        foreach (var dropdownItem in GetRealizedContainers())
        {
            if (dropdownItem.IsFocused)
            {
                SelectedIndex = IndexFromContainer(dropdownItem);
                break;
            }
        }
    }

    private bool SelectNext() => MoveSelection(SelectedIndex, 1, WrapSelection);
    private bool SelectPrevious() => MoveSelection(SelectedIndex, -1, WrapSelection);

    private bool MoveSelection(int startIndex, int step, bool wrap)
    {
        static bool IsSelectable(object? o) => (o as AvaloniaObject)?.GetValue(IsEnabledProperty) ?? true;

        var count = ItemCount;

        for (int i = startIndex + step; i != startIndex; i += step)
        {
            if (i < 0 || i >= count)
            {
                if (wrap)
                {
                    if (i < 0)
                        i += count;
                    else if (i >= count)
                        i %= count;
                }
                else
                {
                    return false;
                }
            }

            var item = ItemsView[i];
            var container = ContainerFromIndex(i);

            if (IsSelectable(item) && IsSelectable(container))
            {
                SelectedIndex = i;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Clears the selection
    /// </summary>
    public void Clear()
    {
        SelectedItem = null;
        SelectedIndex = -1;
    }

}