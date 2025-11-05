using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Utilities.UI.MiscUtilities
{
    public class DataFilterHelper : IEditorDataFilter
    {
        Dictionary<string, string> _dictColumnFormats;

        public DataFilterHelper()
        {
            _dictColumnFormats = new Dictionary<string, string>();

        }
        public void SetValues(string columnName, string format)
        {
            try
            {
                if (_dictColumnFormats.ContainsKey(columnName))
                {
                    _dictColumnFormats[columnName] = format;
                }
                else
                {
                    _dictColumnFormats.Add(columnName, format);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public object Convert(EditorDataFilterConvertArgs conversionArgs)
        {
            try
            {
                if (conversionArgs.Direction == ConversionDirection.OwnerToEditor)
                {
                    UltraGridCell cell = conversionArgs.Context as UltraGridCell;
                    if (cell != null && _dictColumnFormats.ContainsKey(cell.Column.Key))
                    {
                        conversionArgs.Handled = true;
                        decimal dValue;
                        if (Decimal.TryParse(conversionArgs.Value.ToString(), out dValue))
                        {
                            return dValue.ToString(_dictColumnFormats[cell.Column.Key]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return conversionArgs.Value;
        }
    }
}
