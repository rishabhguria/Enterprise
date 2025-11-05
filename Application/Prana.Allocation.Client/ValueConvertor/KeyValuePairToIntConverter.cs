using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    class KeyValuePairToIntConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a key value pair to int.
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
            KeyValuePair<int, string> selectedItems = new KeyValuePair<int, string>();
            try
            {
                Dictionary<int, string> accountDictionary = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict();
                accountDictionary.Add(-1, "Select");
                int key = (int)value;
                if (accountDictionary.ContainsKey(key))
                    selectedItems = new KeyValuePair<int, string>(key, accountDictionary[key]);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return selectedItems;
        }

        /// <summary>
        /// Converts a int to key value pair.
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
            int preferenceAccountId = -1;
            try
            {
                KeyValuePair<int, string> selectedItems = (KeyValuePair<int, string>)value;
                preferenceAccountId = selectedItems.Key;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return preferenceAccountId;
        }

        #endregion
    }
}
