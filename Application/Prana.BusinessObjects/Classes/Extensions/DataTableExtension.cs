using Prana.LogManager;
using System;
using System.Data;
using System.Linq;

namespace Prana.BusinessObjects
{
    public static class DataTableExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnString">columns with specific Keywords</param>
        public static void AlphabeticColumnSort(this DataTable dt)
        {
            try
            {
                var columnArray = new DataColumn[dt.Columns.Count];
                dt.Columns.CopyTo(columnArray, 0);
                var ordinal = -1;
                foreach (var orderedColumn in columnArray.OrderBy(c => c.ColumnName))
                {
                    orderedColumn.SetOrdinal(++ordinal);
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
        public static DataTable SortColumns(this DataTable dt, string sortingString)
        {
            try
            {
                DataView dataview = dt.DefaultView;
                dataview.Sort = sortingString;
                dt = dataview.ToTable();
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
            return dt;
        }
    }
}
