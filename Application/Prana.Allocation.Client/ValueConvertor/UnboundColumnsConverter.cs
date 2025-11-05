using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Client.Helper;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace Prana.Allocation.Client.ValueConvertor
{
    public class UnboundColumnsConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding" /> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value.If the method returns null, the valid null value is used.A return value of <see cref="T:System.Windows.DependencyProperty" />.<see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding" />.<see cref="F:System.Windows.Data.Binding.DoNothing" /> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (values[3] != DependencyProperty.UnsetValue)
                {
                    CalculatedFields calculatedFields = new CalculatedFields();
                    calculatedFields.FieldName = values[0].ToString();
                    calculatedFields.AssetsWithCommissionInNetAmount = (List<int>)values[1];
                    calculatedFields.PrecisionFormat = values[2].ToString();
                    calculatedFields.AvgPrice = System.Convert.ToDouble(values[3]);
                    calculatedFields.FxConversionOperator = values[8].ToString();
                    calculatedFields.Fxrate = System.Convert.ToDouble(values[4]);
                    calculatedFields.TotalCommissionAndFee = System.Convert.ToDouble(values[5]);
                    calculatedFields.CumQty = System.Convert.ToDouble(values[6]);
                    calculatedFields.ContractMultiplier = System.Convert.ToDouble(values[7]);
                    calculatedFields.SideMultiplier = Prana.BusinessLogic.Calculations.GetSideMultilpier(values[9].ToString());
                    calculatedFields.AssetId = System.Convert.ToInt32(values[10]);
                    calculatedFields.AccruedInterest = System.Convert.ToDouble(values[11]);
                    calculatedFields.AuecLocaDateTime = System.Convert.ToDateTime(values[12]);
                    calculatedFields.CurrencyId = System.Convert.ToInt32(values[13]);
                    calculatedFields.VSCurrencyId = System.Convert.ToInt32(values[14]);
                    calculatedFields.LeadCurrencyId = System.Convert.ToInt32(values[15]);
                    return FieldCalculator.CalculateFieldValue(calculatedFields);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
