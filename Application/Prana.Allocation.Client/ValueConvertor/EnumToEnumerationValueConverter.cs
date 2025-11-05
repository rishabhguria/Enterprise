using Prana.Allocation.Client.Enums;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    class EnumToEnumerationValueConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a enum to enumeration alue.
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
            try
            {
                if (value != null)
                {
                    if (value is CustomOperator)
                    {
                        CustomOperator customOperator = (CustomOperator)value;
                        EnumerationValue enumValue = new EnumerationValue(customOperator.ToString(), (int)customOperator);
                        return enumValue;
                    }
                    else if (value is AllocationBaseType)
                    {
                        AllocationBaseType baseType = (AllocationBaseType)value;
                        EnumerationValue enumValue = new EnumerationValue(EnumHelper.GetDescription(baseType), (int)baseType);
                        return enumValue;
                    }
                    else if (value is Operator)
                    {
                        Operator baseType = (Operator)value;
                        EnumerationValue enumValue = new EnumerationValue(EnumHelper.GetDescription(baseType), (int)baseType);
                        return enumValue;
                    }
                    else if (value is MatchingRuleType)
                    {
                        MatchingRuleType ruleType = (MatchingRuleType)value;
                        EnumerationValue enumValue = new EnumerationValue(EnumHelper.GetDescription(ruleType), (int)ruleType);
                        return enumValue;
                    }
                    else if (Enum.IsDefined(typeof(DefaultBoolean), CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToString())))
                    {
                        DefaultBoolean defaultBoolean = (DefaultBoolean)Enum.Parse(typeof(DefaultBoolean), CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToString()));
                        EnumerationValue enumValue = new EnumerationValue(defaultBoolean.ToString(), (int)defaultBoolean);
                        return enumValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return new EnumerationValue();
        }

        /// <summary>
        /// Converts a enumeration value to enum.
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
                    EnumerationValue enumValue = (EnumerationValue)value;
                    if (Enum.IsDefined(typeof(CustomOperator), enumValue.DisplayText))
                    {
                        CustomOperator customOperator = (CustomOperator)enumValue.Value;
                        return customOperator;
                    }
                    else if (EnumHelper.IsDescriptionDefined(typeof(AllocationBaseType), enumValue.DisplayText))
                    {
                        AllocationBaseType baseType = (AllocationBaseType)enumValue.Value;
                        return baseType;
                    }
                    else if (EnumHelper.IsDescriptionDefined(typeof(Operator), enumValue.DisplayText))
                    {
                        Operator baseType = (Operator)enumValue.Value;
                        return baseType;
                    }
                    else if (EnumHelper.IsDescriptionDefined(typeof(MatchingRuleType), enumValue.DisplayText))
                    {
                        MatchingRuleType baseType = (MatchingRuleType)enumValue.Value;
                        return baseType;
                    }
                    else if (Enum.IsDefined(typeof(DefaultBoolean), enumValue.DisplayText))
                    {
                        DefaultBoolean boolValue = (DefaultBoolean)enumValue.Value;
                        return boolValue.ToString().ToLower();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return value;
        }

        #endregion
    }
}
