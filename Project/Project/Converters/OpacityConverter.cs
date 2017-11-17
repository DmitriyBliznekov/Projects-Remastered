using System;
using System.Globalization;
using System.Windows.Data;

namespace Project.Converters
{
    /// <summary>
    /// Set opacity of element to 50% if there are no selected elements
    /// </summary>
    public class OpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? 1 : 0.5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}