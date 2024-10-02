using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ShippingScheduleMVVM.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is a valid DateTime
            if (value is DateTime dateTime)
            {
                // Convert the DateTime value to the desired format (Portuguese)
                string formattedDate = dateTime.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("pt-PT"));
                return formattedDate;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

