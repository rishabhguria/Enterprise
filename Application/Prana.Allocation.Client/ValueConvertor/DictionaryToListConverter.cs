using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Helper;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    class DictionaryToListConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts the dictionary to list.
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
            ObservableCollection<object> collection = new ObservableCollection<object>();
            try
            {
                List<int> selectedItemsInts = new List<int>();
                Dictionary<int, string> valuesDictionary = new Dictionary<int, string>();
                List<string> selectedItemsStrings = new List<string>();
                Dictionary<string, string> stringValuesDictionary = new Dictionary<string, string>();
                switch (parameter.ToString())
                {
                    case AllocationClientConstants.EXCHANGE_LIST:
                        selectedItemsInts = (List<int>)value;
                        valuesDictionary = CachedDataManager.GetInstance.GetAllExchanges();
                        break;

                    case AllocationClientConstants.ORDER_SIDE_LIST:
                        selectedItemsStrings = (List<string>)value;
                        stringValuesDictionary = CommonAllocationMethods.GetOrderSides();
                        break;

                    case AllocationClientConstants.ASSET_LIST:
                        selectedItemsInts = (List<int>)value;
                        valuesDictionary = CachedDataManager.GetInstance.GetAllAssets();
                        break;

                    case AllocationClientConstants.ACCOUNT_LIST:
                        selectedItemsInts = (List<int>)value;
                        valuesDictionary = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                        break;
                }
                if (selectedItemsInts != null && selectedItemsInts.Count > 0)
                {
                    foreach (int key in selectedItemsInts)
                    {
                        collection.Add(new KeyValuePair<int, string>(key, valuesDictionary[key]));
                    }

                }
                else if (selectedItemsStrings != null && selectedItemsStrings.Count > 0)
                {
                    foreach (string key in selectedItemsStrings)
                    {
                        collection.Add(new KeyValuePair<string, string>(key, stringValuesDictionary[key]));
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return collection;
        }

        /// <summary>
        /// Converts the list to dictionary.
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
            List<int> collectionIntList = new List<int>();
            List<string> collectionStringList = new List<string>();
            try
            {
                ObservableCollection<object> valueCollection = (ObservableCollection<object>)value;
                if (parameter.ToString().Equals(AllocationClientConstants.ORDER_SIDE_LIST))
                {
                    foreach (object obj in valueCollection)
                    {
                        KeyValuePair<string, string> keyValuePair = (KeyValuePair<string, string>)obj;
                        collectionStringList.Add(keyValuePair.Key);
                    }
                }
                else
                {
                    foreach (object obj in valueCollection)
                    {
                        KeyValuePair<int, string> keyValuePair = (KeyValuePair<int, string>)obj;
                        collectionIntList.Add(keyValuePair.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            if (collectionIntList.Count > 0)
                return collectionIntList;
            else
                return collectionStringList;
        }

        #endregion
    }
}
