using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Prana.Rebalancer.Converters
{
    internal class EnumToEnumerationDescriptionConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a enum value to enumeration description.
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
                if (value != null)
                {
                    return EnumHelper.GetDescription((AlertType)Enum.Parse(typeof(AlertType), value.ToString()));
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return value;
        }

        #endregion

        /// <summary>
        /// TODO convert back to original.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
