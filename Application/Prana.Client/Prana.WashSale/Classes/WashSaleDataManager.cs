using Prana.LogManager;
using Prana.WashSale.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Prana.WashSale.Classes
{
    internal class WashSaleDataManager
    {
        static string _accountIds = string.Empty;
        static string _assetClassIds = string.Empty;
        static string _currencyIds = string.Empty;
        /// <summary>
        /// Gets Wash Sale Grid Data
        /// </summary>
        /// <param name="assetIds"></param>
        /// <param name="assetClassIds"></param>
        /// <param name="currencyIds"></param>
        internal static List<WashSaleTrades> GetWashSaleData(string accountIds, string assetClassIds, string currencyIds)
        {
            _accountIds = accountIds;
            _assetClassIds = assetClassIds;
            _currencyIds = currencyIds;
            DataSet result = null;
            List<WashSaleTrades> wsList = new List<WashSaleTrades>();
            try
            {
                object[] parameter = new object[3];
                parameter[0] = accountIds;
                parameter[1] = assetClassIds;
                parameter[2] = currencyIds;

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(WashSaleConstants.CONST_WASHSALE_GETOPENTAXLOT_STOREDPROCEDURE, parameter, WashSaleConstants.CONST_WASHSALE_CONNECTION_STRING, WashSaleConstants.CONST_WASHSALE_SQL_TIMEOUT);
                if (result != null && result.Tables.Count > 0)
                {
                    DataTable requestDataTable = result.Tables[0];
                    wsList = GetWashSaleObjectList(requestDataTable);
                }
                return wsList;
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
                return null;
            }
        }
        /// <summary>
        /// Get Wasle sale 's object into list.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<WashSaleTrades> GetWashSaleObjectList(DataTable dataTable)
        {
            try
            {
                List<WashSaleTrades> washSaleList = new List<WashSaleTrades>();
                foreach (DataRow row in dataTable.Rows)
                {
                    WashSaleTrades washSaleObj = new WashSaleTrades();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_TAXLOT_ID))
                        washSaleObj.TaxlotID = row[WashSaleConstants.CONST_TAXLOT_ID].ToString();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_TYPE_OF_TRANSACTION))
                        washSaleObj.TypeOfTransaction = row[WashSaleConstants.CONST_TYPE_OF_TRANSACTION].ToString();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_TRADE_DATE))
                    {
                        DateTime tradeDate;
                        DateTime.TryParse(row[WashSaleConstants.CONST_TRADE_DATE].ToString(), out tradeDate);
                        washSaleObj.TradeDate = tradeDate;
                    }
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_ORIGINAL_PURCHASE_DATE))
                    {
                        DateTime orgPurDate;
                        DateTime.TryParse(row[WashSaleConstants.CONST_ORIGINAL_PURCHASE_DATE].ToString(), out orgPurDate);
                        washSaleObj.OriginalPurchaseDate = orgPurDate;
                    }
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_ACCOUNT))
                        washSaleObj.Account = row[WashSaleConstants.CONST_ACCOUNT].ToString();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_SIDE))
                        washSaleObj.Side = row[WashSaleConstants.CONST_SIDE].ToString();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_ASSET))
                        washSaleObj.Asset = row[WashSaleConstants.CONST_ASSET].ToString();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_CURRENCY))
                        washSaleObj.Currency = row[WashSaleConstants.CONST_CURRENCY].ToString();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_BROKER))
                        washSaleObj.Broker = row[WashSaleConstants.CONST_BROKER].ToString();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_SYMBOL))
                        washSaleObj.Symbol = row[WashSaleConstants.CONST_SYMBOL].ToString();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_BLOOMBERG_SYMBOL))
                        washSaleObj.BloombergSymbol = row[WashSaleConstants.CONST_BLOOMBERG_SYMBOL].ToString();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_CUSIP))
                        washSaleObj.CUSIP = row[WashSaleConstants.CONST_CUSIP].ToString();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_ISSUER))
                        washSaleObj.Issuer = row[WashSaleConstants.CONST_ISSUER].ToString();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_UNDERLYING_SYMBOL))
                        washSaleObj.UnderlyingSymbol = row[WashSaleConstants.CONST_UNDERLYING_SYMBOL].ToString();
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_QUANTITY))
                        washSaleObj.Quantity = Convert.ToDouble(row[WashSaleConstants.CONST_QUANTITY]);
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_UNIT_COST_LOCAL))
                        washSaleObj.UnitCostLocal = Convert.ToDouble(row[WashSaleConstants.CONST_UNIT_COST_LOCAL]);
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_TOTAL_COST_LOCAL))
                        washSaleObj.TotalCostLocal = Convert.ToDouble(row[WashSaleConstants.CONST_TOTAL_COST_LOCAL]);
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_TOTAL_COST))
                        washSaleObj.TotalCost = Convert.ToDouble(row[WashSaleConstants.CONST_TOTAL_COST]);
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS) && !DBNull.Value.Equals(row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS]))
                        washSaleObj.WashSaleAdjustedRealizedLoss = decimal.Parse(row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture);
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD) && !DBNull.Value.Equals(row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD]))
                        washSaleObj.WashSaleAdjustedHoldingsPeriod = Int32.Parse(row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].ToString());
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS) && !DBNull.Value.Equals(row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS]))
                        washSaleObj.WashSaleAdjustedCostBasis = decimal.Parse(row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture);
                    if (dataTable.Columns.Contains(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE) && !DBNull.Value.Equals(row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE]))
                        washSaleObj.WashSaleAdjustedHoldingsStartDate = DateTime.Parse(row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE].ToString());

                    washSaleList.Add(washSaleObj);
                }
                return washSaleList;
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
                return null;
            }
        }
        /// <summary>
        /// Save Wash Sale Data into DB
        /// </summary>
        public static void SaveWashSaleTaxlotData()
        {
            try
            {
                WashSaleTradesFiltersUC.IsGetDataOrUploadOrSaveClick = true;
                WashSaleTradesButtonUC.DisableGridData(null, null);
                DataTable dt = new DataTable();
                dt.Columns.Add(WashSaleConstants.CONST_TAXLOT_ID);
                dt.Columns.Add(WashSaleConstants.CONST_TYPE_OF_TRANSACTION);
                dt.Columns.Add(WashSaleConstants.CONST_TRADE_DATE);
                dt.Columns.Add(WashSaleConstants.CONST_ORIGINAL_PURCHASE_DATE);
                dt.Columns.Add(WashSaleConstants.CONST_ACCOUNT);
                dt.Columns.Add(WashSaleConstants.CONST_SIDE);
                dt.Columns.Add(WashSaleConstants.CONST_ASSET);
                dt.Columns.Add(WashSaleConstants.CONST_CURRENCY);
                dt.Columns.Add(WashSaleConstants.CONST_BROKER);
                dt.Columns.Add(WashSaleConstants.CONST_SYMBOL);
                dt.Columns.Add(WashSaleConstants.CONST_BLOOMBERG_SYMBOL);
                dt.Columns.Add(WashSaleConstants.CONST_CUSIP);
                dt.Columns.Add(WashSaleConstants.CONST_ISSUER);
                dt.Columns.Add(WashSaleConstants.CONST_UNDERLYING_SYMBOL);
                dt.Columns.Add(WashSaleConstants.CONST_QUANTITY);
                dt.Columns.Add(WashSaleConstants.CONST_UNIT_COST_LOCAL);
                dt.Columns.Add(WashSaleConstants.CONST_TOTAL_COST_LOCAL);
                dt.Columns.Add(WashSaleConstants.CONST_TOTAL_COST);
                dt.Columns.Add(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS);
                dt.Columns.Add(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD);
                dt.Columns.Add(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS);
                dt.Columns.Add(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE);

                if (WashSaleTradesGridUC._washSaleDataCache.Count > 0)
                {
                    for (int i = 0; i < WashSaleTradesGridUC._washSaleDataCache.Count; i++)
                    {
                        var row = dt.NewRow();
                        row[WashSaleConstants.CONST_TAXLOT_ID] = WashSaleTradesGridUC._washSaleDataCache[i].TaxlotID;
                        row[WashSaleConstants.CONST_TYPE_OF_TRANSACTION] = WashSaleTradesGridUC._washSaleDataCache[i].TypeOfTransaction;
                        row[WashSaleConstants.CONST_TRADE_DATE] = WashSaleTradesGridUC._washSaleDataCache[i].TradeDate;
                        row[WashSaleConstants.CONST_ORIGINAL_PURCHASE_DATE] = WashSaleTradesGridUC._washSaleDataCache[i].OriginalPurchaseDate;
                        row[WashSaleConstants.CONST_ACCOUNT] = WashSaleTradesGridUC._washSaleDataCache[i].Account;
                        row[WashSaleConstants.CONST_SIDE] = WashSaleTradesGridUC._washSaleDataCache[i].Side;
                        row[WashSaleConstants.CONST_ASSET] = WashSaleTradesGridUC._washSaleDataCache[i].Asset;
                        row[WashSaleConstants.CONST_CURRENCY] = WashSaleTradesGridUC._washSaleDataCache[i].Currency;
                        row[WashSaleConstants.CONST_BROKER] = WashSaleTradesGridUC._washSaleDataCache[i].Broker;
                        row[WashSaleConstants.CONST_SYMBOL] = WashSaleTradesGridUC._washSaleDataCache[i].Symbol;
                        row[WashSaleConstants.CONST_BLOOMBERG_SYMBOL] = WashSaleTradesGridUC._washSaleDataCache[i].BloombergSymbol;
                        row[WashSaleConstants.CONST_CUSIP] = WashSaleTradesGridUC._washSaleDataCache[i].CUSIP;
                        row[WashSaleConstants.CONST_ISSUER] = WashSaleTradesGridUC._washSaleDataCache[i].Issuer;
                        row[WashSaleConstants.CONST_UNDERLYING_SYMBOL] = WashSaleTradesGridUC._washSaleDataCache[i].UnderlyingSymbol;
                        row[WashSaleConstants.CONST_QUANTITY] = WashSaleTradesGridUC._washSaleDataCache[i].Quantity;
                        row[WashSaleConstants.CONST_UNIT_COST_LOCAL] = WashSaleTradesGridUC._washSaleDataCache[i].UnitCostLocal;
                        row[WashSaleConstants.CONST_TOTAL_COST_LOCAL] = WashSaleTradesGridUC._washSaleDataCache[i].TotalCostLocal;
                        row[WashSaleConstants.CONST_TOTAL_COST] = WashSaleTradesGridUC._washSaleDataCache[i].TotalCost;
                        if (WashSaleTradesGridUC._washSaleDataCache[i].WashSaleAdjustedRealizedLoss != null && WashSaleTradesGridUC._washSaleDataCache[i].WashSaleAdjustedRealizedLoss.Equals(WashSaleConstants.CONST_BLANK))
                            row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS] = null;
                        else
                            row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS] = WashSaleTradesGridUC._washSaleDataCache[i].WashSaleAdjustedRealizedLoss;
                        if (WashSaleTradesGridUC._washSaleDataCache[i].WashSaleAdjustedHoldingsPeriod != null && WashSaleTradesGridUC._washSaleDataCache[i].WashSaleAdjustedHoldingsPeriod.Equals(WashSaleConstants.CONST_BLANK))
                            row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD] = null;
                        else
                            row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD] = WashSaleTradesGridUC._washSaleDataCache[i].WashSaleAdjustedHoldingsPeriod;
                        if (WashSaleTradesGridUC._washSaleDataCache[i].WashSaleAdjustedCostBasis != null && WashSaleTradesGridUC._washSaleDataCache[i].WashSaleAdjustedCostBasis.Equals(WashSaleConstants.CONST_BLANK))
                            row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS] = null;
                        else
                            row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS] = WashSaleTradesGridUC._washSaleDataCache[i].WashSaleAdjustedCostBasis;
                        if (WashSaleTradesGridUC._washSaleDataCache[i].WashSaleAdjustedHoldingsStartDate != null && WashSaleTradesGridUC._washSaleDataCache[i].WashSaleAdjustedHoldingsStartDate.Equals(DateTime.MinValue))
                            row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE] = null;
                        else
                            row[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE] = WashSaleTradesGridUC._washSaleDataCache[i].WashSaleAdjustedHoldingsStartDate;
                        dt.Rows.Add(row);
                    }
                }
                dt.TableName = WashSaleConstants.CONST_WASHSALE_TABLENAME;
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                string xmlWashSaleData = ds.GetXml();
                object[] parameter = new object[4];
                parameter[0] = xmlWashSaleData;
                parameter[1] = _accountIds;
                parameter[2] = _assetClassIds;
                parameter[3] = _currencyIds;
                DatabaseManager.DatabaseManager.ExecuteDataSet(WashSaleConstants.CONST_WASHSALE_SAVETABLE_STOREDPROCEDURE, parameter,WashSaleConstants.CONST_WASHSALE_CONNECTION_STRING);
                WashSaleTradesFiltersUC.IsGetDataOrUploadOrSaveClick = false;
                WashSaleTradesButtonUC.DisableGridData(null, null);
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
