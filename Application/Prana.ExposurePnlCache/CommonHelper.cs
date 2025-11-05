using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ExposurePnlCache
{
    public class CommonHelper
    {
        static IVariableColumnsTable temp = null;

        private static DataTable _orderSummary = null;

        public static DataTable OrderSummaryTable
        {
            get
            {
                if (_orderSummary != null)
                {
                    return _orderSummary.Copy();
                }
                else
                {
                    return null;
                }
            }
        }

        private static DataTable _orderSummaryAccountTable;

        public static DataTable OrderSummaryAccountTable
        {
            get { return _orderSummaryAccountTable.Copy(); }
        }

        static CommonHelper()
        {
            try
            {
                temp = new ExPNLBindableViewTableCreator(new BaseDataTable());
                temp.ObjectForDataTable = typeof(ExposureAndPnlOrderSummary);
                ((ExPNLBindableViewTableCreator)temp).LoadColumnsFromXML();
                _orderSummary = temp.TableWithColums;
                if (_orderSummary != null)
                {
                    _orderSummary.TableName = "OrderSummary";
                }
                _orderSummaryAccountTable = temp.TableWithColums;
                if (_orderSummaryAccountTable != null)
                {
                    _orderSummaryAccountTable.TableName = "OrderSummary";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #region Code For Indices Updates
        internal static void AddIndicesColumnsToSummary(DataTable indexTable)
        {
            try
            {
                foreach (DataColumn col in indexTable.Columns)
                {
                    if (!_orderSummary.Columns.Contains(col.ColumnName))
                    {
                        DataColumn newCol = new DataColumn();
                        newCol.ColumnName = col.ColumnName;
                        newCol.Caption = col.Caption;
                        newCol.DataType = col.DataType;
                        //All returns are added with 2 decimal points and %
                        newCol.ExtendedProperties.Add("Format", "#,0.00%");
                        newCol.ExtendedProperties.Add("Hidden", false);
                        newCol.ExtendedProperties.Add("OnlyInColChooser", true);
                        newCol.ExtendedProperties.Add("IsIndexColumn", true);
                        _orderSummary.Columns.Add(newCol);
                    }
                    if (!_orderSummaryAccountTable.Columns.Contains(col.ColumnName))
                    {
                        DataColumn newColForAccount = new DataColumn();

                        newColForAccount.ColumnName = col.ColumnName;
                        newColForAccount.Caption = col.Caption;
                        newColForAccount.DataType = col.DataType;
                        //All returns are added with 2 decimal points and %
                        newColForAccount.ExtendedProperties.Add("Format", "#,0.00%");
                        newColForAccount.ExtendedProperties.Add("Hidden", false);
                        newColForAccount.ExtendedProperties.Add("OnlyInColChooser", true);
                        newColForAccount.ExtendedProperties.Add("IsIndexColumn", true);
                        _orderSummaryAccountTable.Columns.Add(newColForAccount);
                    }

                }
                if (_orderSummary.Rows.Count > 0)
                {
                    InitializeRow(_orderSummary.Rows[0]);
                }
                if (_orderSummaryAccountTable.Rows.Count > 0)
                {
                    InitializeRow(_orderSummaryAccountTable.Rows[0]);
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

        private static void ClearAllIndicesColumns()
        {
            //DataColumn[] tempColumnsArr = new DataColumn[_orderSummary.Columns.Count];
            //_orderSummary.Columns.CopyTo(tempColumnsArr, 0);
            //for (int i = 0; i < tempColumnsArr.Length; i++)
            //{
            //    object isIndexCol = tempColumnsArr[i].ExtendedProperties["IsIndexColumn"];
            //    if (isIndexCol != null && Convert.ToBoolean(isIndexCol))
            //    {
            //        _orderSummary.Columns.Remove(tempColumnsArr[i].ColumnName);
            //        _orderSummaryAccountTable.Columns.Remove(tempColumnsArr[i].ColumnName);
            //    }
            //}

            List<string> columnsToRemove = new List<string>();
            foreach (DataColumn col in _orderSummary.Columns)
            {
                object isIndexCol = _orderSummary.ExtendedProperties["IsIndexColumn"];
                if (isIndexCol != null && Convert.ToBoolean(isIndexCol))
                {
                    columnsToRemove.Add(col.ColumnName);
                }
            }
            foreach (string colToBeDeleted in columnsToRemove)
            {
                _orderSummary.Columns.Remove(colToBeDeleted);
            }
            columnsToRemove.Clear();
            foreach (DataColumn col in _orderSummaryAccountTable.Columns)
            {
                object isIndexCol = _orderSummaryAccountTable.ExtendedProperties["IsIndexColumn"];
                if (isIndexCol != null && Convert.ToBoolean(isIndexCol))
                {
                    columnsToRemove.Add(col.ColumnName);
                }
            }
            foreach (string colToBeDeleted in columnsToRemove)
            {
                _orderSummaryAccountTable.Columns.Remove(colToBeDeleted);
            }
            columnsToRemove = null;
        }

        #endregion

        #region Update Current Data From Incoming data
        public static DataTable GetOrderSummaryTableFromObject(ExposureAndPnlOrderSummary summary)
        {

            DataTable dtOrderSummary = _orderSummaryAccountTable.Clone();


            try
            {
                // Due to reflection, this funciton was taking a lot of time. So assigned the summary values property by property to make it faster
                DataRow summaryRow = dtOrderSummary.NewRow();
                summaryRow["BetaAdjustedExposure"] = summary.BetaAdjustedExposure;
                summaryRow["CashInflow"] = summary.CashInflow;
                summaryRow["CashOutflow"] = summary.CashOutflow;
                summaryRow["CashProjected"] = summary.CashProjected;
                summaryRow["CostBasisPNL"] = summary.CostBasisPNL;
                summaryRow["DayPnL"] = summary.DayPnL;
                summaryRow["DayPnLLong"] = summary.DayPnLLong;
                summaryRow["DayPnLShort"] = summary.DayPnLShort;
                summaryRow["DayReturn"] = summary.DayReturn;
                summaryRow["EarnedDividendBase"] = summary.EarnedDividendBase;
                summaryRow["GrossExposure"] = summary.GrossExposure;
                summaryRow["GrossMarketValue"] = summary.GrossMarketValue;
                summaryRow["Level1ID"] = summary.Level1ID;
                summaryRow["LongExposure"] = summary.LongExposure;
                summaryRow["LongMarketValue"] = summary.LongMarketValue;
                summaryRow["NAVString"] = summary.NAVString;
                summaryRow["NetAssetValue"] = summary.NetAssetValue;
                summaryRow["NetExposure"] = summary.NetExposure;
                summaryRow["NetMarketValue"] = summary.NetMarketValue;
                summaryRow["NetPercentBetaAdjExposure"] = summary.NetPercentBetaAdjExposure;
                summaryRow["NetPercentCashProjected"] = summary.NetPercentCashProjected;
                summaryRow["NetPercentExposure"] = summary.NetPercentExposure;
                summaryRow["NetPercentExposure"] = summary.NetPercentExposure;
                summaryRow["NetPercentGrossMktValue"] = summary.NetPercentGrossMktValue;
                summaryRow["NetPercentLongMktValue"] = summary.NetPercentLongMktValue;
                summaryRow["NetPercentShortMktValue"] = summary.NetPercentShortMktValue;
                summaryRow["NetPosition"] = summary.NetPosition;
                summaryRow["PNLContributionPercentageSummary"] = summary.PNLContributionPercentageSummary;
                summaryRow["PositionSideMV"] = summary.PositionSideMV;
                summaryRow["PositionSideExposure"] = summary.PositionSideExposure;
                summaryRow["ShortExposure"] = summary.ShortExposure;
                summaryRow["ShortMarketValue"] = summary.ShortMarketValue;
                summaryRow["StartOfDayCash"] = summary.StartOfDayCash;
                summaryRow["UnderlyingValueForOptions"] = summary.UnderlyingValueForOptions;
                summaryRow["YesterdayNAV"] = summary.YesterdayNAV;
                summaryRow["PercentNetMarketValue"] = summary.PercentNetMarketValue;
                summaryRow["NetPercentExposureGross"] = summary.NetPercentExposureGross;
                summaryRow["UnderlyingGrossExposure"] = summary.UnderlyingGrossExposure;
                summaryRow["PercentUnderlyingGrossExposure"] = summary.PercentUnderlyingGrossExposure;
                summaryRow["StartOfDayAccruals"] = summary.StartOfDayAccruals;
                if (bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIGKEY_IsPerformanceNumberColumnsEnabled)))
                {
                    summaryRow["YTDReturn"] = summary.YTDReturn;
                    summaryRow["QTDReturn"] = summary.QTDReturn;
                    summaryRow["MTDReturn"] = summary.MTDReturn;
                    summaryRow["MTDPnL"] = summary.MTDPnL;
                    summaryRow["QTDPnL"] = summary.QTDPnL;
                    summaryRow["YTDPnL"] = summary.YTDPnL;
                }
                summaryRow["LongDebitLimit"] = summary.LongDebitLimit;
                summaryRow["ShortCreditLimit"] = summary.ShortCreditLimit;
                summaryRow["LongDebitBalance"] = summary.LongDebitBalance;
                summaryRow["ShortCreditBalance"] = summary.ShortCreditBalance;
                summaryRow["UnderlyingLongExposure"] = summary.UnderlyingLongExposure;
                summaryRow["UnderlyingShortExposure"] = summary.UnderlyingShortExposure;
                summaryRow["PercentUnderlyingLongExposure"] = summary.PercentUnderlyingLongExposure;
                summaryRow["PercentUnderlyingShortExposure"] = summary.PercentUnderlyingShortExposure;
                summaryRow["PercentNetMarketValueGrossMV"] = summary.PercentNetMarketValueGrossMV;
                summaryRow["DayReturnGrossMarketValue"] = summary.DayReturnGrossMarketValue;
                summaryRow["YesterdayMarketValue"] = summary.YesterdayMarketValue;
                summaryRow["BetaAdjustedGrossExposure"] = summary.BetaAdjustedGrossExposure;
                summaryRow["BetaAdjustedLongExposure"] = summary.BetaAdjustedLongExposure;
                summaryRow["BetaAdjustedShortExposure"] = summary.BetaAdjustedShortExposure;
                summaryRow["BetaAdjustedGrossExposureUnderlying"] = summary.BetaAdjustedGrossExposureUnderlying;
                summaryRow["NetPercentBetaAdjustedGrossExposureUnderlying"] = summary.NetPercentBetaAdjustedGrossExposureUnderlying;
                summaryRow["BetaAdjustedLongExposureUnderlying"] = summary.BetaAdjustedLongExposureUnderlying;
                summaryRow["NetPercentBetaAdjustedLongExposureUnderlying"] = summary.NetPercentBetaAdjustedLongExposureUnderlying;
                summaryRow["BetaAdjustedShortExposureUnderlying"] = summary.BetaAdjustedShortExposureUnderlying;
                summaryRow["NetPercentBetaAdjustedShortExposureUnderlying"] = summary.NetPercentBetaAdjustedShortExposureUnderlying;
                summaryRow["DayPnLFX"] = summary.DayPnLFX;
                summaryRow["NetPercentDayPnLLong"] = summary.NetPercentDayPnLLong;
                summaryRow["NetPercentDayPnLShort"] = summary.NetPercentDayPnLShort;
                summaryRow["NetPercentDayPnLFX"] = summary.NetPercentDayPnLFX;
                summaryRow["LongShortExposureRatioUnderlying"] = summary.LongShortExposureRatioUnderlying;
                summaryRow["DayAccruals"] = summary.DayAccruals;
                dtOrderSummary.Rows.Add(summaryRow);
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

            return dtOrderSummary;

        }

        //internal static DataTable GetSummaryTableForConsolidationViewFromObject(ExposureAndPnlOrderSummary summary, Dictionary<int, DataTable> accountWiseSummary)
        //{
        //    DataTable dtOrderSummary = _orderSummary.Clone();
        //    dtOrderSummary.Rows.Add(dtOrderSummary.NewRow());
        //    try
        //    {
        //        foreach (PropertyInfo property in _propertiesOfSummary)
        //        {
        //            // We can safely avoid this check because we know that datatable has been built from summary object only
        //            if (dtOrderSummary.Columns.Contains(property.Name))
        //                dtOrderSummary.Rows[0][property.Name] = property.GetValue(summary, null);
        //        }
        //            }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //    return dtOrderSummary;
        //}
        #endregion

        private static void InitializeRow(DataRow row)
        {
            foreach (DataColumn col in row.Table.Columns)
            {
                if (col.DataType == typeof(System.Double))
                {
                    row[col] = 0;
                    continue;
                }
                if (col.DataType == typeof(System.String))
                {
                    row[col] = String.Empty;
                    continue;
                }
                if (col.DataType == typeof(System.Int32))
                {
                    row[col] = 0;
                    continue;
                }
                if (col.DataType == typeof(PositionType))
                {
                    row[col] = PositionType.Long;
                    continue;
                }
            }
        }
    }
}
