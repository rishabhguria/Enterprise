using Infragistics.Windows.Editors;
using Prana.LogManager;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    public class PrecisionFormatDisplayConverter : IValueConverter
    {
        #region IValueConverter methods

        /// <summary>
        /// Converts a value to percision format.
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
            object editorValue = (value == null) ? 0.0 : value;
            try
            {
                XamNumericEditor editor = (XamNumericEditor)parameter;
                return (editor.Tag != null) ? string.Format(editor.Tag.ToString(), editorValue) : editorValue;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return editorValue;
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

        #endregion IValueConverter methods
    }
}
