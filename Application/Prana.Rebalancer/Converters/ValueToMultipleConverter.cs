using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.LogManager;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Prana.Rebalancer.Converters
{
    internal class ValueToMultipleConverter : IValueConverter
    {
        /// <summary>
        /// To convert Actual and Threshold result values to multiple or not.
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
                double numericValue;
                if (value.ToString().Contains(ComplainceConstants.CONST_SEPARATOR_CHAR))
                {
                    return ComplainceConstants.CONST_MULTIPLE;
                }
                else if (double.TryParse(value.ToString(), out numericValue))
                {
                    return string.Format(ComplainceConstants.CONST_PRECISION_FORMAT, numericValue);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return value;
        }

        /// <summary>
        /// To convert back.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
