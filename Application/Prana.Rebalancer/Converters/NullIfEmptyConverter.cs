using System;
using System.Globalization;
using System.Windows.Data;

namespace Prana.Rebalancer.Converters
{
    public class NullIfEmptyConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
                return null;

            return value;
        }

        #endregion
    }
}