using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ReconciliationNew
{
    public static class UltraGridBaseSorter
    {
        /// <summary>
        /// Genric sorting helper for ultragrid
        /// Input: Ultragrid,dictionary of (column,sorting order)
        /// Output: Ultragrid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="dictSortingSummary"></param>
        /// <returns></returns>
        internal static UltraGrid Sort(UltraGrid grid, Dictionary<string, SortingOrder> dictSortingSummary)
        {
            try
            {
                grid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
                int columnIndex = int.MinValue;
                foreach (KeyValuePair<string, SortingOrder> column in dictSortingSummary)
                {
                    columnIndex = grid.DisplayLayout.Bands[0].Columns.IndexOf(column.Key);

                    if (columnIndex != -1 && columnIndex != int.MinValue)
                    {
                        if (column.Value.Equals(SortingOrder.Ascending))
                        {
                            grid.DisplayLayout.Bands[0].Columns[column.Key].SortIndicator = SortIndicator.Ascending;
                        }
                        else
                        {
                            grid.DisplayLayout.Bands[0].Columns[column.Key].SortIndicator = SortIndicator.Descending;
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
            return grid;
        }
    }
}
