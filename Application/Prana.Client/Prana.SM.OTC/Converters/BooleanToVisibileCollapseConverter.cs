using Prana.LogManager;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Prana.SM.OTC.Converters
{
    public class BooleanToVisibileCollapseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                bool isReverse = parameter != null && parameter.ToString().Equals("Reverse");
                if (value is Boolean && (bool)value)
                {
                    return isReverse ? Visibility.Collapsed : Visibility.Visible;
                }
                return isReverse ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
