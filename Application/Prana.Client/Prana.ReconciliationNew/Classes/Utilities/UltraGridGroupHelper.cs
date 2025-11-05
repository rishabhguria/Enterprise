using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ReconciliationNew
{
    public static class UltraGridGroupHelper
    {
        /// <summary>
        /// Genric grouping helper for ultragrid
        /// Input: Ultragrid,List of columns
        /// Output: Ultragrid
        /// modified by: sachin mishra,10 Feb 2015 purpose: Add try catch block
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="lstColumns"></param>
        /// <returns></returns>
        internal static UltraGrid Group(UltraGrid grid, List<string> lstColumns)
        {
            try
            {
                grid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                grid.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
                grid.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
                grid.DisplayLayout.Bands[0].SortedColumns.Clear();
                int columnIndex = int.MinValue;
                foreach (string columnName in lstColumns)
                {
                    columnIndex = grid.DisplayLayout.Bands[0].Columns.IndexOf(columnName);
                    if ((columnIndex != -1) && (columnIndex != int.MinValue))
                    {
                        grid.DisplayLayout.Bands[0].SortedColumns.Add(columnName, false, true);
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
            return grid;
        }
    }
}
