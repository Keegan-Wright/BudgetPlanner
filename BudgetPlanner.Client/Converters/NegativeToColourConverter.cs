using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace BudgetPlanner.Client.Converters
{
    public class NegativeToColourConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            SolidColorBrush brush = new SolidColorBrush();
            decimal decimalValue = Decimal.Parse(value.ToString());
            if (decimalValue != null && decimal.IsNegative(decimalValue))
            {
                brush.Color = Colors.Red;
            }
            else
            {
                brush.Color = Colors.Green;
            }
            return brush;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
