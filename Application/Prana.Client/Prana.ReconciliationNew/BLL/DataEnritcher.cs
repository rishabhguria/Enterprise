using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ReconciliationNew
{
    public class DataEnritcher
    {
        /// <summary>
        /// Input: datatable, columns and formula expression to add
        /// Output: datatable , custom columns added
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="listColumns"></param>
        internal static DataTable AddCustomColumns(DataTable dt, List<ColumnInfo> listColumns)
        {
            try
            {
                foreach (ColumnInfo column in listColumns)
                {
                    if (!string.IsNullOrEmpty(column.ColumnName) && column.IsSelected && !string.IsNullOrEmpty(column.FormulaExpression))
                    {
                        DataColumn customColumn = new DataColumn();
                        customColumn.ColumnName = column.ColumnName;
                        customColumn.Expression = column.FormulaExpression;

                        dt.Columns.Add(customColumn);

                    }

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dt;
        }
    }
}
