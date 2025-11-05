using Prana.LogManager;
using System;
using System.Text;
using System.Windows.Data;

namespace Prana.Rebalancer.Converters
{
    public class RebalancerPrecisionFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                int precesion = 4;
                if (value.ToString().Equals("0"))
                {
                    return 0;
                }
                StringBuilder sb = new StringBuilder();

                sb.Append("{0:#.");
                for (int index = 0; index < precesion; ++index)
                {
                    sb.Append("#");
                }
                sb.Append("}");

                return string.Format(sb.ToString(), value);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
