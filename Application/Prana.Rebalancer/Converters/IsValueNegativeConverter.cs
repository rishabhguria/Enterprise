using Prana.LogManager;
using System;
using System.Windows.Data;

namespace Prana.Rebalancer.Converters
{
    class IsValueNegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    decimal output;
                    if (decimal.TryParse(value.ToString(), out output))
                        return output < 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
