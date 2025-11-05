//this file is same as OptionModelUserInputCache.cs in 
//Prana.OptionCalculator.CalculationComponent indexer
//pricing server

using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.CommonDataCache
{
    public static class PricingInfoCache
    {

        private static Dictionary<string, UserOptModelInput> _dictOMIData;
        static PricingInfoCache()
        {
            _dictOMIData = new Dictionary<string, UserOptModelInput>();
            //Refresh();
        }



        public static Dictionary<string, UserOptModelInput> DictOmiData
        {
            get { return _dictOMIData; }
            set { _dictOMIData = value; }
        }

        public static UserOptModelInput GetUserOMIData(string symbol)
        {
            if (_dictOMIData.ContainsKey(symbol))
            {
                return _dictOMIData[symbol];
            }
            else
            {
                return null;
            }
        }
        public static void Refresh()
        {
            try
            {
                _dictOMIData.Clear();
                DataTable dtOMIData = GetOptionModelUserDataFromDB();
                foreach (DataRow dr in dtOMIData.Rows)
                {
                    UserOptModelInput userOMI = new UserOptModelInput();
                    string symbol = dr["Symbol"].ToString();
                    userOMI.HistoricalVolUsed = Convert.ToBoolean(dr["HistoricalVolatilityUsed"].ToString());
                    userOMI.Volatility = Convert.ToDouble(dr["UserVolatility"].ToString()) / 100;
                    userOMI.VolatilityUsed = Convert.ToBoolean(dr["UserVolatilityUsed"].ToString());
                    userOMI.IntRate = Convert.ToDouble(dr["UserInterestRate"].ToString());
                    userOMI.IntRateUsed = Convert.ToBoolean(dr["UserInterestRateUsed"].ToString());
                    userOMI.Dividend = Convert.ToDouble(dr["UserDividend"].ToString());
                    userOMI.DividendUsed = Convert.ToBoolean(dr["UserDividendUsed"].ToString());
                    userOMI.StockBorrowCost = Convert.ToDouble(dr["UserStockBorrowCost"].ToString());
                    userOMI.StockBorrowCostUsed = Convert.ToBoolean(dr["UserStockBorrowCostUsed"].ToString());
                    userOMI.Delta = Convert.ToDouble(dr["UserDelta"].ToString());
                    userOMI.DeltaUsed = Convert.ToBoolean(dr["UserDeltaUsed"].ToString());
                    userOMI.LastPrice = Convert.ToDouble(dr["UserLastPrice"].ToString());
                    userOMI.LastPriceUsed = Convert.ToBoolean(dr["UserLastPriceUsed"].ToString());

                    _dictOMIData.Add(symbol, userOMI);
                }
            }
            #region Catch
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
            #endregion
        }
        public static System.Data.DataTable GetOptionModelUserDataFromDB()
        {
            DataSet ds = new DataSet();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetOptionModelTable";

            try
            {
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            #region Catch
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
            #endregion

            return ds.Tables[0];
        }
    }
}