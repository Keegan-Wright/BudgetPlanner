using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Converters
{
    public class StatusToColourConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            SolidColorBrush brush = new SolidColorBrush();
            string status = value as string;
            if (status != null && status.Equals("Pending"))
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
