using Prana.Allocation.Client.Constants;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    class EnumToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converter for Enum to String Value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String Description = string.Empty;
            try
            {
                if (parameter.ToString().Equals(AllocationUIConstants.GROUP_STATUS))
                {
                    if (value is PostTradeEnums.Status)
                    {
                        PostTradeEnums.Status baseType = (PostTradeEnums.Status)value;
                        Description = EnumHelper.GetDescription(baseType);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return Description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
