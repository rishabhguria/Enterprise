using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Prana.Rebalancer.Converters
{
    public class IsPercentageNegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    string valueStr = value.ToString();
                    if (valueStr.Contains("%"))
                        valueStr = valueStr.Substring(0, valueStr.IndexOf("%"));
                    
                    decimal output;
                    if (decimal.TryParse(valueStr, out output))
                        return output < 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
