using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    class TextToListConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts text to list.
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
            string prValues = string.Empty;
            try
            {
                if (value != null)
                {
                    List<string> prList = new List<string>();
                    prList = (List<string>)value;
                    prValues = string.Join(",", prList.ToArray());
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return prValues;
        }

        /// <summary>
        /// Converts list to text.
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
            List<string> prList = new List<string>();
            try
            {
                if (value != null)
                {
                    string prValues = (string)value;
                    prList = prValues.Split(',').ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return prList;
        }

        #endregion
    }
}
