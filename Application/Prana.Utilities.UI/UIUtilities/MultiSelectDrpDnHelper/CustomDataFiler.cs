using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System.Collections.Generic;

namespace Prana.Utilities.UI
{
    public class CustomDataFiler : IEditorDataFilter
    {
        public object Convert(EditorDataFilterConvertArgs conversionArgs)
        {

            if (conversionArgs.Direction == ConversionDirection.EditorToDisplay)
            {
                var cell = (UltraGridCell)conversionArgs.Context;
                List<object> values = cell.Value as List<object>;
                if (values != null)
                {
                    conversionArgs.Handled = true;
                    if (values.Count == 0)
                    {
                        return "";
                    }
                    if (values.Count == ((UltraCombo)cell.EditorComponentResolved).Rows.Count)
                    {
                        return "All";
                    }
                    return "Multiple";
                }

            }
            return conversionArgs.Value;
        }
    }
}
