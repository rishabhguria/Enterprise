using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Prana.Rebalancer.Converters
{
    /// <summary>
    /// Converts the number precision to the format string
    /// </summary>
    class PrecisionFormatConverter : IValueConverter
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
            int precisionDigit = Int32.Parse(value.ToString());
            PTTValueType pstValueType = (PTTValueType)parameter;
            StringBuilder precisionFormat = new StringBuilder();
            switch (pstValueType)
            {
                case PTTValueType.Field:
                    precisionFormat.Append("###,###,###0.");
                    break;
                case PTTValueType.Summary:
                    precisionFormat.Append("{0:###,#.");
                    break;
                case PTTValueType.FieldPercentage:
                    precisionFormat.Append("###,###,###0.##");
                    break;
                case PTTValueType.FixedField:
                    precisionFormat.Append("###,###,###,###0.####");
                    break;
            }

            string endPrecisionFormat = pstValueType == PTTValueType.Field || pstValueType == PTTValueType.FieldPercentage || pstValueType == PTTValueType.FixedField ? String.Empty : "}";
            try
            {

                for (int i = 0; i < precisionDigit; i++)
                {
                    precisionFormat.Append("#");
                }
                precisionFormat.Append(endPrecisionFormat);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return precisionFormat.ToString();
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