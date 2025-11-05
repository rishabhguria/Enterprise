using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Prana.Rebalancer.Converters
{
    /// <summary>
    /// Used for converting AccountID in viewModel to accountName in view
    /// </summary>
    public class AccountIDToAccountNameConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && !String.IsNullOrEmpty(value.ToString()))
                {
                    return CachedDataManager.GetInstance.GetAccounts().ContainsKey(Int32.Parse(value.ToString()))
                        ? CachedDataManager.GetInstance.GetAccounts()[Int32.Parse(value.ToString())]
                        : string.Empty;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                return string.Empty;
            }
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
    }
}
