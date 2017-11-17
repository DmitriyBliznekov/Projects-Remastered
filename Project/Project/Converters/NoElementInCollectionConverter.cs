using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Project.Converters
{
    /// <inheritdoc />
    /// <summary>
    /// Hide element, which presents collection, if count of elements equals 0
    /// </summary>
    public class NoElementInCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return int.Parse(value.ToString()) > 0 ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}