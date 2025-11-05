using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.PM.Client.UI
{
    class ColumnMapping
    {
        static Dictionary<string, string> _dictColumnsNamePair = new Dictionary<string, string>();

        internal static string GetColumnNameForApprovedChanged(string ColumnName)
        {
            string mappedColumnName = string.Empty;
            try
            {
                if (_dictColumnsNamePair.Count == 0)
                {
                    FillColumnsNamePair();
                }
                if (_dictColumnsNamePair.Keys.Contains(ColumnName, StringComparer.InvariantCultureIgnoreCase))
                {
                    mappedColumnName = _dictColumnsNamePair[ColumnName];
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
            return mappedColumnName;
        }

        private static void FillColumnsNamePair()
        {
            try
            {
                _dictColumnsNamePair.Add(ClosingConstants.COL_ExecutedQty, ClosingConstants.ApprovedChangesColumnQuantity);
                _dictColumnsNamePair.Add(ClosingConstants.COL_AveragePrice, ClosingConstants.ApprovedChangesColumnAvgPX);
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
    }
}
