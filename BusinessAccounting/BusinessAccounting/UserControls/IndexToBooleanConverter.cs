using System;
using System.Windows.Data;

namespace BusinessAccounting.UserControls
{
    [ValueConversion(typeof(int), typeof(bool))]
    public class IndexToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int val;
            int.TryParse(value?.ToString(), out val);
            return val == 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
