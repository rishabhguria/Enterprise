using Prana.LogManager;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Prana.Rebalancer.Converters
{
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool visibility = Boolean.Parse(value.ToString());
                if (visibility)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
