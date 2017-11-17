using System;
using System.Globalization;
using System.Windows.Data;

namespace Project.Converters
{
    /// <summary>
    /// Enable or disable chosen element
    /// </summary>
    public class HiddenElementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}