using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.ExpnlService.DataDumper
{
    /// <summary>
    /// This static class will clean data before saving to database
    /// It will also do calculation if any required
    /// </summary>
    internal static class DataCleaner
    {

        //Data table for Indices Return Data
        private static DataTable _outputTable = GetIndicesDataTable();

        /// <summary>
        /// Get Indices DataTable
        /// </summary>
        /// <returns></returns>
        internal static DataTable GetIndicesDataTable()
        {
            DataTable table = new DataTable("IndicesPerformance");
            table.Columns.Add("IndexSymbol");
            table.Columns.Add("Caption");
            table.Columns.Add("Performance");
            return table;
        }

        /// <summary>
        /// Get Clean Realtime Data from Dictionary for dump
        /// </summary>
        /// <param name="_incomingCache"></param>
        /// <returns></returns>
        internal static ExposurePnlCacheItemList GetCleanedData(Dictionary<int, BusinessObjects.ExposurePnlCacheItemList> _incomingCache)
        {
            ExposurePnlCacheItemList expnlCacheItemdata = new ExposurePnlCacheItemList();
            try
            {
                List<ExposurePnlCacheItemList> data = new List<ExposurePnlCacheItemList>();
                data = _incomingCache.Values.ToList();
                data.ForEach(x =>
                {
                    List<ExposurePnlCacheItem> expnl = x.Cast<ExposurePnlCacheItem>().ToList();
                    expnl.ForEach(y =>
                        {
                            // check for allocated trades or not
                            y.Asset = y.IsSwap == true ? "EquitySwap" : y.Asset;
                            y.Level1Name = y.Level1ID != -1 ? CachedDataManager.GetInstance.GetAccountText(y.Level1ID) : string.Empty;
                            y.CurrencySymbol = y.CurrencyID != 0 && y.CurrencyID != -1 ? CachedDataManager.GetInstance.GetCurrencyText(y.CurrencyID) : string.Empty;
                            y.Exchange = CachedDataManager.GetInstance.GetExchangeText(y.ExchangeID);
                            expnlCacheItemdata.Add(y);
                        });

                });
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
            return expnlCacheItemdata;
        }

        /// <summary>
        /// Get Pivot table 
        /// </summary>
        /// <param name="inputTable"></param>
        /// <returns></returns>
        internal static DataTable GetCleanTable(DataTable inputTable)
        {
            try
            {
                _outputTable.Clear();

                if (inputTable != null && inputTable.Rows.Count > 0)
                {
                    // Add rows by looping columns        
                    for (int rCount = 0; rCount <= inputTable.Columns.Count - 1; rCount++)
                    {
                        DataRow newRow = _outputTable.NewRow();

                        newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                        newRow[1] = inputTable.Columns[rCount].Caption.ToString();

                        for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                        {
                            string colValue = inputTable.Rows[cCount][rCount].ToString();
                            newRow[cCount + 2] = colValue;
                        }
                        _outputTable.Rows.Add(newRow);
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

            return _outputTable;
        }
    }
}
