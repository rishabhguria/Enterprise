using System;
using System.Globalization;
using System.Windows.Data;

namespace Prana.SM.OTC.Converters
{
    public class NullableValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
                return 0;

            return value;
        }

        #endregion
    }
}
