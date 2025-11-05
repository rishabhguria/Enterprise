using Prana.LogManager;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Prana.Rebalancer.Converters
{
    public class SystemColorToSolidBrushConverter : IValueConverter
    {

        /// <summary>
        /// Converts a System color to solid brush color value
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted color value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {

                System.Drawing.Color color = (System.Drawing.Color)value;
                System.Windows.Media.Color converted = Color.FromArgb(color.A, color.R, color.G, color.B);
                return new SolidColorBrush(converted);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                return string.Empty;
            }
        }

        /// <summary>
        /// Converts back solid brush color to system color
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
            return null;
        }

    }
}
