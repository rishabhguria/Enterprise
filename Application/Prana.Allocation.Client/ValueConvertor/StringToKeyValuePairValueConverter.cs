using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Helper;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    class StringToKeyValuePairValueConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a string to key value pair.
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
            KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>();
            try
            {
                switch (parameter.ToString())
                {
                    case AllocationUIConstants.ORDER_SIDE:
                        Dictionary<string, string> orderSides = CommonAllocationMethods.GetOrderSides();
                        foreach (KeyValuePair<string, string> side in orderSides)
                        {
                            if (side.Value.ToLower().Equals(value.ToString().ToLower()))
                                return side;
                        }
                        break;

                    case AllocationUIConstants.TRANSACTION_TYPE:
                        Dictionary<string, string> transactionType = CachedDataManager.GetInstance.DictTransactionType;
                        foreach (KeyValuePair<string, string> tt in transactionType)
                        {
                            if (tt.Key.ToLower().Equals(value.ToString().ToLower()))
                                return tt;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return keyValuePair;
        }

        /// <summary>
        /// Converts a key value pair to string.
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
            KeyValuePair<string, string> keyValuePair = (KeyValuePair<string, string>)value;
            try
            {
                if (value != null && parameter != null)
                {
                    switch (parameter.ToString())
                    {
                        case AllocationUIConstants.ORDER_SIDE:
                            return keyValuePair.Value;

                        case AllocationUIConstants.TRANSACTION_TYPE:
                            return keyValuePair.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return string.Empty;
        }

        #endregion
    }
}
