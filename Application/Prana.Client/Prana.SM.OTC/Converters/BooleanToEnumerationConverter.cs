using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Prana.SM.OTC.Converters
{
    /// <summary>
    /// Used to convert Boolean value to Enumeration Yes/No
    /// </summary>
    public class BooleanToEnumerationConverter : IValueConverter
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
                if (value is bool)
                {
                    if ((bool)value)
                        return new EnumerationValue(PTTCombineAccountTotalValue.Yes.ToString(), (int)PTTCombineAccountTotalValue.Yes);
                    return new EnumerationValue(PTTCombineAccountTotalValue.No.ToString(), (int)PTTCombineAccountTotalValue.No);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new EnumerationValue();
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
            try
            {
                if (value != null)
                {
                    EnumerationValue enumValue = (EnumerationValue)value;
                    if (Enum.IsDefined(typeof(PTTCombineAccountTotalValue), enumValue.DisplayText))
                    {
                        PTTCombineAccountTotalValue pstConsolidationType = (PTTCombineAccountTotalValue)enumValue.Value;
                        return pstConsolidationType == PTTCombineAccountTotalValue.Yes;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }
    }
}
