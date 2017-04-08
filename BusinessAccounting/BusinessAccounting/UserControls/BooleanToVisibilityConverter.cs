using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BusinessAccounting.UserControls
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = Visibility.Visible;
            try
            {
                if (value != null)
                {
                    visibility = (bool)value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return visibility;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return (value != null) && (value == (object) Visibility.Visible);

        }
    }
}
