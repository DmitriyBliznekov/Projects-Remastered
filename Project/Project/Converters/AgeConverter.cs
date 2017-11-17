using System;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Project.Converters
{
    /// <inheritdoc />
    /// <summary>
    /// Convert number of age to format as "number of age + word", where word is (год, лет, года)
    /// </summary>
    public class AgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (!IsTextAllowed(value.ToString()))
                return "Ошибка";
            return value + AgeDetermination(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string AgeDetermination(string age)
        {
            if (age.EndsWith("1") && !age.EndsWith("11"))
                return " год";
            if ((age.EndsWith("2") && !age.EndsWith("12")) ||
                (age.EndsWith("3") && !age.EndsWith("13")) ||
                (age.EndsWith("4") && !age.EndsWith("14")))
                return " года";
            return string.IsNullOrEmpty(age) ? string.Empty : " лет";
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]");
            return !regex.IsMatch(text);
        }
    }
}
