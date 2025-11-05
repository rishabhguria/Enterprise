using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    class DictionaryValueToKeyConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a dictionary value to key.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null && parameter != null)
                {
                    return CachedDataManager.GetInstance.GetDictionaryValueFromKey(value, parameter);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return value;
        }

        /// <summary>
        /// Converts a dictionary key to value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null && parameter != null)
                {
                    return CachedDataManager.GetInstance.GetDictionaryKeyFromValue(value, parameter);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return value;
        }

        #endregion
    }
}
