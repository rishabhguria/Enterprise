using Prana.LogManager;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Prana.HeatMapControlsWPF
{
    public class BackGroundColorConvertor : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double val = System.Convert.ToDouble(value);
                byte alphaAdjusted = (byte)(255 * Math.Abs(val));
                //var converter = new System.Windows.Media.BrushConverter();

                if (val > 0d)
                {
                    return new SolidColorBrush(Color.FromArgb(alphaAdjusted, 0, 255, 0));
                }
                if (val < 0d)
                {
                    return new SolidColorBrush(Color.FromArgb(alphaAdjusted, 255, 0, 0));
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return true;
        }
    }
}
