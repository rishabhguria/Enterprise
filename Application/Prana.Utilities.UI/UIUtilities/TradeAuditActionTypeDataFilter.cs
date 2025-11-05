using Infragistics.Win;
using Prana.BusinessObjects;
using System.ComponentModel;

namespace Prana.Utilities.UI.UIUtilities
{
    public class TradeAuditActionTypeDataFilter : Infragistics.Win.IEditorDataFilter
    {
        public object Convert(Infragistics.Win.EditorDataFilterConvertArgs conversionArgs)
        {
            TradeAuditActionTypeConverter ac = TypeDescriptor.GetConverter(typeof(TradeAuditActionType.ActionType)) as TradeAuditActionTypeConverter;
            switch (conversionArgs.Direction)
            {
                // Convert the value to a string to show in the grid
                case ConversionDirection.OwnerToEditor:
                    string convertedValue = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, conversionArgs.Value, typeof(string));
                    conversionArgs.Handled = true;
                    return convertedValue;
                // Convert the value back to the original type
                case ConversionDirection.EditorToOwner:
                    TradeAuditActionType.ActionType action = (TradeAuditActionType.ActionType)ac.ConvertFromString(null, System.Globalization.CultureInfo.CurrentCulture, conversionArgs.Value.ToString());
                    conversionArgs.Handled = true;
                    return action;
            }
            return conversionArgs.Value;
        }
    }
}
