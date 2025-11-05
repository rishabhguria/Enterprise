using Prana.Allocation.Client.Constants;
using Prana.LogManager;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    class IntegerToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converter for Integer to String Value
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
                if (parameter.ToString().Equals(AllocationUIConstants.PUT_OR_CALL))
                {
                    if ((int)value == 1)
                        return "C";
                    else if ((int)value == 0)
                        return "P";
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

