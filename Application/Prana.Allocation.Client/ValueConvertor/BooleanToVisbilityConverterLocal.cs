using Prana.LogManager;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    public class BooleanToVisbilityConverterLocal : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Convert bool value to Visibility Hidden behaviour
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool visibility = Boolean.Parse(value.ToString());
                if (visibility)
                {
                    return Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
