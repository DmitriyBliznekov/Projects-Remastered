using System;
using System.Windows.Data;

namespace Project.Converters
{
    /// <summary>
    /// Converter replace "0" to "мужской" and "1" to "женский"
    /// </summary>
    public class GenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
                return value.ToString() == "0" ? "мужской" : "женский";
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}