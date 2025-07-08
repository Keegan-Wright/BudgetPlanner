using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace BudgetPlanner.Client.Converters;

public class DecimalToCurrencyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is decimal decimalValue)
        {
            return decimalValue.ToString("C", CultureInfo.CurrentCulture);
        }
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            if (decimal.TryParse(stringValue, NumberStyles.Currency, culture, out decimal result))
            {
                return result;
            }
        }
        return value; 
    }
}