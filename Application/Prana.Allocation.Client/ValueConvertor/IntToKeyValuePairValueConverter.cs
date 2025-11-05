using Prana.Allocation.Client.Constants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    class IntToKeyValuePairValueConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a int to key value pair.
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
            KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>();
            try
            {
                string val = (string)value;
                if (parameter.Equals(AllocationUIConstants.CAPTION_COUNTERPARTY_NAME))
                {
                    Dictionary<int, string> brokers = CachedDataManager.GetInstance.GetUserCounterParties();
                    foreach (KeyValuePair<int, string> broker in brokers)
                    {
                        if (broker.Value.Equals(val))
                            return broker;
                    }
                    if (keyValuePair.Key == 0 && keyValuePair.Value == null)
                        return new KeyValuePair<int, string>(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                }
                if (parameter.Equals(AllocationUIConstants.BorrowerBroker))
                {
                    Dictionary<int, string> brokers = CachedDataManager.GetInstance.GetAllThirdPartiesWithShortName();
                    foreach (KeyValuePair<int, string> broker in brokers)
                    {
                        if (broker.Value.Equals(val))
                            return broker;
                    }
                    if (keyValuePair.Key == 0 && keyValuePair.Value == null)
                        return new KeyValuePair<int, string>(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                }
                if (parameter.Equals(AllocationUIConstants.VENUE))
                {
                    Dictionary<int, string> venues = CommonDataCache.CachedDataManager.GetInstance.GetAllVenues();
                    foreach (KeyValuePair<int, string> venue in venues)
                    {
                        if (venue.Value.Equals(val))
                            return venue;
                    }
                    if (keyValuePair.Key == 0 && keyValuePair.Value == null)
                        return new KeyValuePair<int, string>(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                }
                if (parameter.Equals(AllocationUIConstants.SETTLEMENT_CURRENCY))
                {
                    Dictionary<int, string> settlCurrency = CachedDataManager.GetInstance.GetAllCurrencies();
                    foreach (KeyValuePair<int, string> currency in settlCurrency)
                    {
                        if (currency.Value.Equals(val))
                            return currency;
                    }
                }

                return keyValuePair;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return keyValuePair;
        }

        /// <summary>
        /// Converts a key value pair to int.
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
            try
            {
                if (value != null)
                {
                    KeyValuePair<int, string> keyValuePair = (KeyValuePair<int, string>)value;

                    string val = keyValuePair.Value;
                    return val;
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
