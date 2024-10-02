using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ShippingScheduleMVVM.Converters
{
    public class MouseEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is UIElement sender && value is EventArgs e)
            {
                return new EventParemeters(e, sender);
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
