using Prana.Allocation.Client.Constants;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    /// <summary>
    /// This class converts dictionary value to enum value
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class DictionaryToEnumConverter : IValueConverter
    {
        /// <summary>
        /// Converts a dictionary to enum.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>();
            try
            {
                //if value is select from UI, then setting commission basis to Auto as auto is not used from Bulk change 
                if (value.ToString().Equals(ApplicationConstants.C_COMBO_SELECT) || value.ToString().Equals(CalculationBasis.Auto.ToString()))
                {
                    keyValuePair = new KeyValuePair<int, string>(int.MinValue, value.ToString());
                }
                else
                {
                    switch (parameter.ToString())
                    {
                        case AllocationClientConstants.CALCULATION_BASIS:
                            object key = CachedDataManager.GetInstance.GetDictionaryKeyFromValue(value, AllocationClientConstants.CALCULATION_BASIS);
                            if (key != null)
                                keyValuePair = new KeyValuePair<int, string>((int)key, value.ToString());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return keyValuePair;
        }

        /// <summary>
        /// Converts a enum to dictionary.
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
            KeyValuePair<int, string> keyValuePair = (KeyValuePair<int, string>)value;
            try
            {
                switch (parameter.ToString())
                {
                    case AllocationClientConstants.CALCULATION_BASIS:
                        if (keyValuePair.Value.Equals(ApplicationConstants.C_COMBO_SELECT))
                            return CalculationBasis.Auto;
                        else
                            return keyValuePair.Value;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return keyValuePair.Value;
        }
    }
}
