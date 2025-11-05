using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.CommonDatabaseAccess
{
    public class PositionDataManager
    {
        static DataSet dtOpenPositions = new DataSet();
        static bool refresh = true;

        public static DataSet GetSymbolsForOpenPositionsAccountsAndDate(List<int> accountIds, DateTime dateForOpenPositions, int SMBatchID)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetSymbolsForOpenPositionsTillDateAndFunds";
                queryData.CommandTimeout = 900;
                queryData.DictionaryDatabaseParameter.Add("@Date", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Date",
                    ParameterType = DbType.DateTime,
                    ParameterValue = dateForOpenPositions
                });
                queryData.DictionaryDatabaseParameter.Add("@FundIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundIds",
                    ParameterType = DbType.String,
                    ParameterValue = String.Join<int>(",", accountIds)
                });
                queryData.DictionaryDatabaseParameter.Add("@SMBatchID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@SMBatchID",
                    ParameterType = DbType.Int32,
                    ParameterValue = SMBatchID
                });

                DataSet openSymbols = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return openSymbols;
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
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolPks">comma separated symbolpks</param>
        /// <returns></returns>
        public static DataSet GetSymbolsForSymbolPK(string symbolPks)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetSymbolDataForSymbolPks";
                queryData.CommandTimeout = 900;
                queryData.DictionaryDatabaseParameter.Add("@SymbolPK", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@SymbolPK",
                    ParameterType = DbType.String,
                    ParameterValue = symbolPks
                });

                DataSet symbolPKSymbols = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return symbolPKSymbols;
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
            return null;
        }
        public static string GetAllAUECLocalDatesFromUTCStr(DateTime theDateTime)
        {
            StringBuilder AllAUECLocalDatesFromUTC = new StringBuilder();
            try
            {
                Dictionary<int, Prana.BusinessObjects.TimeZone> AllAUECTimeZones = KeyValueDataManager.auecIDTimeZones;
                // Add UTC date to the string by default with 0 AUECID
                AllAUECLocalDatesFromUTC.Append("0");
                AllAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                AllAUECLocalDatesFromUTC.Append(theDateTime.Date);
                AllAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);

                foreach (KeyValuePair<int, Prana.BusinessObjects.TimeZone> AUECTimeZone in AllAUECTimeZones)
                {
                    int AuecID = AUECTimeZone.Key;
                    AllAUECLocalDatesFromUTC.Append(AuecID);
                    AllAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                    AllAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(theDateTime, DataManagerInternalRepository.KeyValueDataManager.GetAUECTimeZone(AuecID)).Date);
                    AllAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
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
            return AllAUECLocalDatesFromUTC.ToString();
        }
        public static DataSet GetOpenPositions()
        {

            try
            {
                if (refresh)
                {
                    string ToAllAUECDatesString = GetAllAUECLocalDatesFromUTCStr(DateTime.UtcNow);

                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "PMGetFundOpenPositionsForDateBase_New";
                    queryData.CommandTimeout = 900;
                    queryData.DictionaryDatabaseParameter.Add("@ToAllAUECDatesString", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@ToAllAUECDatesString",
                        ParameterType = DbType.String,
                        ParameterValue = ToAllAUECDatesString
                    });

                    dtOpenPositions = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                    refresh = false;
                }
                return dtOpenPositions;
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
            return null;

        }
        public static void Refresh()
        {
            refresh = true;
        }
        public static List<PranaPosition> GetPositions()
        {
            List<PranaPosition> coll = new List<PranaPosition>();
            try
            {

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
            return coll;
        }
        public static DataSet GetBenchMarks()
        {

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetPSBenchMarks";

                DataSet productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return productsDataSet;
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
            return null;

        }

        //static Dictionary<string, string> _dictPSSymbols = null;
        //public static void SetPSSymbols(Dictionary<string, string> dictPSSymbols)
        //{
        //    _dictPSSymbols = dictPSSymbols;
        //}

        //public static string GetPSSymbol(PranaPosition pranaPos)
        //{
        //    string psSymbol = string.Empty;
        //    if (_dictPSSymbols != null)
        //    {
        //        if (_dictPSSymbols.ContainsKey(pranaPos.Symbol))
        //        {
        //            psSymbol = _dictPSSymbols[pranaPos.Symbol];
        //        }
        //    }
        //    return psSymbol;
        //}

        //private static void FillPositionBasicDetails(DataRow row, PranaPosition pranaPos)
        //{
        //    try
        //    {
        //        pranaPos.Symbol = row["Symbol"].ToString();
        //        pranaPos.OrderSideTagValue = row["SideID"].ToString().Trim();
        //        pranaPos.Quantity = double.Parse(row["OpenQuantity"].ToString());
        //        if (!Prana.CommonDataCache.NameValueFiller.IsLongSide(pranaPos.OrderSideTagValue))
        //        {
        //            pranaPos.Quantity = -pranaPos.Quantity;
        //        }
        //        pranaPos.Strategy = row["Level2ID"] != DBNull.Value ? Prana.CommonDataCache.CachedDataManager.GetInstance.GetStrategyText(int.Parse(row["Level2ID"].ToString())) : string.Empty;
        //        pranaPos.Account = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(int.Parse(row["FundID"].ToString()));
        //        pranaPos.UnderlyingSymbol = row["UnderlyingSymbol"] != DBNull.Value ? row["UnderlyingSymbol"].ToString() : string.Empty;

        //        #region Merged UDA data from data row - omshiv, Nov 2013, UDA Merged to Sec Master

        //        if (row.Table.Columns.Contains("AssetName") && row["AssetName"] != DBNull.Value)
        //            //  pranaPos.UDAAsset = row["AssetName"].ToString();

        //            if (row.Table.Columns.Contains("SecurityTypeName") && row["SecurityTypeName"] != DBNull.Value)
        //                pranaPos.SecurityTypeName = row["SecurityTypeName"].ToString();

        //        if (row.Table.Columns.Contains("SectorName") && row["SectorName"] != DBNull.Value)
        //            pranaPos.SectorName = row["SectorName"].ToString();

        //        if (row.Table.Columns.Contains("SubSectorName") && row["SubSectorName"] != DBNull.Value)
        //            pranaPos.SubSectorName = row["SubSectorName"].ToString();

        //        if (row.Table.Columns.Contains("CountryName") && row["CountryName"] != DBNull.Value)
        //            pranaPos.CountryName = row["CountryName"].ToString();

        //        #endregion
        //         //commnted by: OMshiv , NOv 2103, Removed UDA symbol data cache
        //       // Prana.BusinessObjects.SecurityMasterBusinessObjects.UDAData udaData= CachedDataManager.GetInstance.GetUDAData(pranaPos.Symbol);
        //       // if (udaData != null)
        //        //{
        //        //pranaPos.SecurityTypeName = row["CompanyName"].ToString(); ;
        //        //pranaPos.SectorName = row["CompanyName"].ToString(); ;
        //        //pranaPos.SubSectorName = row["CompanyName"].ToString(); ;
        //        //pranaPos.CountryName = row["CompanyName"].ToString(); ;
        //       // }
        //      //  else
        //       // { 
        //    //        pranaPos.SecurityTypeName = "Undefined";
        //         //   pranaPos.CountryName = "Undefined";
        //         //   pranaPos.SectorName = "Undefined";
        //       //     pranaPos.SubSectorName = "Undefined";
        //       // }

        //        pranaPos.AssetID = int.Parse(row["AssetID"].ToString());
        //        pranaPos.AssetName = CommonDataCache.CachedDataManager.GetInstance.GetAssetText(pranaPos.AssetID);
        //        pranaPos.AvgPrice = float.Parse(row["AveragePrice"].ToString());
        //        pranaPos.SecurityName = row["CompanyName"].ToString();
        //        //pranaPos.PutOrCall = Convert.ToChar(row["PutOrCall"].ToString());
        //        if (row["Multiplier"] != System.DBNull.Value)
        //        {
        //            pranaPos.Multiplier = double.Parse(row["Multiplier"].ToString());
        //        }
        //        if (row["Delta"] != System.DBNull.Value)
        //        {
        //            pranaPos.Delta = float.Parse(row["Delta"].ToString());
        //        }

        //        pranaPos.SideMultiplier = Calculations.GetSideMultilpier(pranaPos.OrderSideTagValue);
        //        pranaPos.PositionType= (pranaPos.SideMultiplier == 1 )? "Long" : "Short";
        //        string putOrCall = row["PutOrCall"].ToString();
        //        pranaPos.PutOrCall = (String.IsNullOrEmpty(putOrCall)) ? ' ' : char.Parse(putOrCall.Substring(0, 1));
        //        pranaPos.StrikePrice = double.Parse(row["StrikePrice"].ToString());
        //        pranaPos.ExpirationDate = Convert.ToDateTime(row["ExpirationDate"].ToString());
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //public static PranaPositionWithGreekColl GetPositionsWithGreeks()
        //{
        //    PranaPositionWithGreekColl coll = new PranaPositionWithGreekColl();
        //    try
        //    {
        //        DataSet ds = GetOpenPositions();
        //        if (ds != null)
        //        {
        //            DataTable dt = ds.Tables[0];
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                PranaPosition pranaPosition = new PranaPositionWithGreeks();
        //                FillPositionBasicDetails(row, pranaPosition);
        //                PranaPositionWithGreeks pranaPositionWithGreeks = (PranaPositionWithGreeks)pranaPosition;
        //                if (pranaPositionWithGreeks.Quantity != 0)
        //                {
        //                    if (pranaPositionWithGreeks.AssetID == (int)AssetCategory.Equity)
        //                    {
        //                        //pranaPositionWithGreeks.Delta = 1;
        //                        pranaPositionWithGreeks.UnderlyingSymbol = pranaPositionWithGreeks.Symbol;
        //                    }
        //                    else
        //                    {
        //                        pranaPositionWithGreeks.UnderlyingSymbol = row["UnderLyingSymbol"].ToString();
        //                    }
        //                    coll.Add(pranaPositionWithGreeks);
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return coll;
        //}
    }
}
