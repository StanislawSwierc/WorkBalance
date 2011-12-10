using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Diagnostics.Contracts;
using System.Windows.Media;

namespace WorkBalance.Converters
{
    public class BrushToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // TODO: Comment out that contract
            Contract.Requires(value != null);
            Contract.Requires(value is Brush);
            Contract.Requires(targetType.IsAssignableFrom(typeof(Color)));

            var brush = value as SolidColorBrush;
            return brush != null ? brush.Color : Color.FromRgb(255, 255, 255);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
