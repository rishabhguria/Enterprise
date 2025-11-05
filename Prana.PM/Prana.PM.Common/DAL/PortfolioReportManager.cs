using Prana.BusinessLogic;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.PM.DAL
{
    public class PortfolioReportManager : IPMInteraction
    {
        private static int _errorNumber;
        private static string _errorMessage = string.Empty;

        private static PortfolioReportManager _portfolioReportManagerInst = null;
        /// <summary>
        /// Singleton Implementation
        /// </summary>
        /// <returns></returns>
        public static PortfolioReportManager GetInstance()
        {
            if (_portfolioReportManagerInst == null)
            {
                _portfolioReportManagerInst = new PortfolioReportManager();
            }
            return _portfolioReportManagerInst;
        }

        public NetPositionList GetRealizedPLPositions(DateTime startDate, DateTime endDate, int companyID, int userID)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetRealizedPLPositions";

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
                queryData.DictionaryDatabaseParameter.Add("@CompanyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CompanyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });
                queryData.DictionaryDatabaseParameter.Add("@UserID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@UserID",
                    ParameterType = DbType.Int32,
                    ParameterValue = userID
                });
                queryData.DictionaryDatabaseParameter.Add("@StartDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@StartDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = startDate
                });
                queryData.DictionaryDatabaseParameter.Add("@EndDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@EndDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = endDate
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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

            return FillRealizedPLPositions(ds);

        }

        private NetPositionList FillRealizedPLPositions(DataSet ds)
        {
            NetPositionList openRealizedPLPositionList = new NetPositionList();

            int positionId = 0;
            int accountName = 1;
            int symbol = 2;

            int netPosition = 3;
            int strategyName = 4;
            int positionStartDate = 5;
            int positionState = 6;
            int realizedPL = 7;
            int positionType = 8;
            int accountID = 9;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    Position realizedPLPosition = new Position();

                    try
                    {
                        if (!(row[positionId] is System.DBNull))
                        {
                            // realizedPLPosition.ID = (Guid)(row[positionId]);
                        }
                        if (!(row[accountName] is System.DBNull))
                        {
                            realizedPLPosition.AccountValue.FullName = row[accountName].ToString();
                        }
                        if (!(row[realizedPL] is System.DBNull))
                        {
                            realizedPLPosition.CostBasisRealizedPNL = Math.Round(double.Parse(row[realizedPL].ToString()), 4);
                        }
                        if (!(row[symbol] is System.DBNull))
                        {
                            realizedPLPosition.Symbol = row[symbol].ToString();
                        }
                        if (!(row[positionState] is System.DBNull))
                        {
                            //realizedPLPosition.Status = (PostionStatus)int.Parse(row[positionState].ToString());
                        }
                        if (!(row[netPosition] is System.DBNull))
                        {
                            //realizedPLPosition.OpenQty = long.Parse(row[netPosition].ToString());
                        }
                        if (!(row[strategyName] is System.DBNull))
                        {
                            realizedPLPosition.Strategy = row[strategyName].ToString();
                        }
                        if (!(row[positionStartDate] is System.DBNull))
                        {
                            //realizedPLPosition.StartDate = DateTime.Parse(row[positionStartDate].ToString());
                        }
                        if (!(row[positionType] is System.DBNull))
                        {
                            //realizedPLPosition.PositionType = (PositionType)row[positionType];
                        }
                        if (!(row[accountID] is System.DBNull))
                        {
                            realizedPLPosition.AccountValue.ID = int.Parse(row[accountID].ToString());
                        }


                        openRealizedPLPositionList.Add(realizedPLPosition);
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

            return openRealizedPLPositionList;
        }

        public NetPositionList GetDailyPositions(DateTime _date, int companyID, int userID)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetDailyPositions";
            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
                queryData.DictionaryDatabaseParameter.Add("@CompanyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CompanyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });
                queryData.DictionaryDatabaseParameter.Add("@UserID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@UserID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });
                queryData.DictionaryDatabaseParameter.Add("@Date", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Date",
                    ParameterType = DbType.DateTime,
                    ParameterValue = _date
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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

            return FillDailyPositions(ds);

        }

        private static NetPositionList FillDailyPositions(DataSet ds)
        {
            NetPositionList dailyPositionList = new NetPositionList();

            int positionId = 0;
            int accountName = 1;
            int symbol = 2;

            int netPosition = 3;
            int strategyName = 4;
            int positionCloseDate = 5;
            int positionType = 6;
            int realizedPL = 7;
            int accountID = 8;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    Position dailyPosition = new Position();

                    try
                    {
                        if (!(row[positionId] is System.DBNull))
                        {
                            //dailyPosition.ID = (Guid)(row[positionId]);
                        }
                        if (!(row[accountName] is System.DBNull))
                        {
                            dailyPosition.AccountValue.FullName = row[accountName].ToString();
                        }
                        if (!(row[realizedPL] is System.DBNull))
                        {
                            dailyPosition.CostBasisRealizedPNL = Math.Round(double.Parse(row[realizedPL].ToString()), 4);
                        }
                        if (!(row[symbol] is System.DBNull))
                        {
                            dailyPosition.Symbol = row[symbol].ToString();
                        }
                        if (!(row[positionType] is System.DBNull))
                        {
                            // dailyPosition.PositionType = (PositionType)row[positionType];
                        }
                        if (!(row[netPosition] is System.DBNull))
                        {
                            // dailyPosition.OpenQty = long.Parse(row[netPosition].ToString());
                        }
                        if (!(row[strategyName] is System.DBNull))
                        {
                            dailyPosition.Strategy = row[strategyName].ToString();
                        }
                        if (!(row[positionCloseDate] is System.DBNull))
                        {
                            // dailyPosition.StartDate = DateTime.Parse(row[positionCloseDate].ToString());
                        }
                        if (!(row[accountID] is System.DBNull))
                        {
                            dailyPosition.AccountValue.ID = int.Parse(row[accountID].ToString());
                        }


                        dailyPositionList.Add(dailyPosition);
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

            return dailyPositionList;
        }

        public OpenTaxLotList GetCurrentOpenAllocatedData(int companyID)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetTotalOpenAllocatedDataForBaskets";
            XMLSaveManager.AddOutErrorParameters(queryData);
            //System.Data.Common.DbCommand commandSP = db.GetStoredProcCommand("PMGetTotalOpenAllocatedData");
            try
            {
                string AllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(DateTime.UtcNow);

                queryData.DictionaryDatabaseParameter.Add("@AllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = AllAUECDatesString
                });
                queryData.DictionaryDatabaseParameter.Add("@CompanyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CompanyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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

            return FillOpenTaxLot(ds);
        }

        /// <summary>
        /// Fills the open tax lot.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        private OpenTaxLotList FillOpenTaxLot(DataSet ds)
        {
            OpenTaxLotList openTaxLotList = new OpenTaxLotList();


            int AccountID = 0;
            int Symbol = 1;
            int Quantity = 2;

            // int AUECID = 3;
            int PositionType = 4;
            int AssetID = 5;
            int UnderLyingID = 6;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    OpenTaxlot taxLot = new OpenTaxlot();

                    try
                    {
                        if (!(row[AccountID] is System.DBNull))
                        {
                            taxLot.AccountID = int.Parse(row[AccountID].ToString());
                            taxLot.AccountName = CachedDataManager.GetInstance.GetAccountText(taxLot.AccountID);

                        }
                        if (!(row[Symbol] is System.DBNull))
                        {
                            taxLot.Symbol = Convert.ToString(row[Symbol]);
                        }
                        if (!(row[Quantity] is System.DBNull))
                        {
                            taxLot.Quantity = Convert.ToInt64(row[Quantity]);
                        }
                        if (!(row[AssetID] is System.DBNull))
                        {
                            taxLot.AssetID = Convert.ToInt32(row[AssetID]);
                            taxLot.AssetName = CachedDataManager.GetInstance.GetAssetText(taxLot.AssetID);
                        }
                        if (!(row[UnderLyingID] is System.DBNull))
                        {
                            taxLot.UnderlyingID = Convert.ToInt32(row[UnderLyingID]);
                        }
                        if (!(row[PositionType] is System.DBNull))
                        {
                            taxLot.PositionType = (PositionType)row[PositionType];
                        }


                        openTaxLotList.Add(taxLot);
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

            return openTaxLotList;
        }

        //Bhupesh Commented 20-02-2008
        //Not used for now. As this functionality is used to generate DailyEquityValueReport which is not asked for in the Yunzei release.
        public NetPositionList GetCurrentOpenAllocatedDataForExport(int companyID)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetTotalOpenAllocatedDataForExport";
            XMLSaveManager.AddOutErrorParameters(queryData);
            //System.Data.Common.DbCommand commandSP = db.GetStoredProcCommand("PMGetTotalOpenAllocatedData");
            try
            {
                string AllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(DateTime.UtcNow);
                queryData.DictionaryDatabaseParameter.Add("@AllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = AllAUECDatesString
                });
                queryData.DictionaryDatabaseParameter.Add("@CompanyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CompanyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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

            return FillOpenTaxLotForExport(ds);
        }

        /// <summary>
        /// Fills the open tax lot.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        private NetPositionList FillOpenTaxLotForExport(DataSet ds)
        {
            NetPositionList openTaxLotList = new NetPositionList();


            int AccountID = 0;
            int Symbol = 1;
            int Quantity = 2;
            //int PositionType = 3;
            int AveragePrice = 3;
            int TaxLotID = 4;
            int Side = 5;

            int Commission = 6;
            int Fees = 7;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    Position taxLotORPosition = new Position();

                    try
                    {
                        if (!(row[AccountID] is System.DBNull))
                        {
                            taxLotORPosition.AccountID = int.Parse(row[AccountID].ToString());
                            taxLotORPosition.AccountValue.FullName = CachedDataManager.GetInstance.GetAccountText(taxLotORPosition.AccountID);

                        }
                        if (!(row[Symbol] is System.DBNull))
                        {
                            taxLotORPosition.Symbol = Convert.ToString(row[Symbol]);
                        }
                        if (!(row[Quantity] is System.DBNull))
                        {
                            // taxLotORPosition.PositionStartQty = Convert.ToInt64(row[Quantity]);
                        }
                        //if (!(row[PositionType] is System.DBNull))
                        //{
                        //    taxLotORPosition.PositionType = (PositionType)row[PositionType];
                        //}

                        if (!(row[AveragePrice] is System.DBNull))
                        {
                            taxLotORPosition.OpenAveragePrice = double.Parse(row[AveragePrice].ToString());
                        }
                        if (!(row[TaxLotID] is System.DBNull))
                        {
                            taxLotORPosition.ID = (row[TaxLotID]).ToString();
                        }
                        if (!(row[Side] is System.DBNull))
                        {
                            taxLotORPosition.Side = row[Side].ToString();
                        }
                        if (!(row[Commission] is System.DBNull))
                        {
                            //taxLotORPosition.OpenCommission = double.Parse((row[Commission]).ToString());
                        }
                        if (!(row[Fees] is System.DBNull))
                        {
                            //taxLotORPosition.OtherBrokerOpenFees = double.Parse(row[Fees].ToString());
                        }

                        openTaxLotList.Add(taxLotORPosition);
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

            return openTaxLotList;
        }

        public List<string> GetSymbolListForOpenPositionsAndTrades(int companyID)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetTotalOpenAllocatedDataForExport";
            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
                string AllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(DateTime.UtcNow);
                queryData.DictionaryDatabaseParameter.Add("@AllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = AllAUECDatesString
                });
                queryData.DictionaryDatabaseParameter.Add("@CompanyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CompanyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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
            NetPositionList openPositionsAndTrades = FillOpenTaxLotForExport(ds);

            List<string> listSymbol = new List<string>();
            foreach (Position position in openPositionsAndTrades)
            {
                listSymbol.Add(position.Symbol);
            }

            return listSymbol;
        }

        public AllocatedTradesList GetHistoryPositions(StringBuilder positionIDStringBuilder, int companyID, int userID)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetHistoryPositions";
            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
                queryData.DictionaryDatabaseParameter.Add("@CompanyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CompanyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });
                queryData.DictionaryDatabaseParameter.Add("@UserID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@UserID",
                    ParameterType = DbType.Int32,
                    ParameterValue = userID
                });
                queryData.DictionaryDatabaseParameter.Add("@PositionIDStringBuilder", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@PositionIDStringBuilder",
                    ParameterType = DbType.String,
                    ParameterValue = positionIDStringBuilder.ToString()
                });
                //db.AddInParameter(commandSP, "@EndDate", DbType.DateTime, endDate);

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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

            return FillHistoryPositions(ds);

        }

        private static AllocatedTradesList FillHistoryPositions(DataSet ds)
        {
            AllocatedTradesList historyPositionList = new AllocatedTradesList();
            //int result = int.MinValue;

            int positionId = 0;
            int positionStartDate = 1;
            int lastCloseTradeTime = 2;

            int symbol = 3;
            int realizedPNL = 4;
            int startQuantity = 5;
            int netPosition = 6;
            int isPosition = 7;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    AllocatedTrade historyPosition = new AllocatedTrade();

                    try
                    {
                        if (!(row[positionId] is System.DBNull))
                        {
                            //historyPosition.PositiontTaxlotID = (Guid)(row[positionId]);
                        }
                        if (!(row[positionStartDate] is System.DBNull))
                        {
                            historyPosition.PositionStartDate = DateTime.Parse(row[positionStartDate].ToString());
                        }
                        if (!(row[lastCloseTradeTime] is System.DBNull))
                        {
                            historyPosition.ParentPositionEndDate = DateTime.Parse(row[lastCloseTradeTime].ToString());
                        }
                        if (!(row[symbol] is System.DBNull))
                        {
                            historyPosition.Symbol = row[symbol].ToString();
                        }
                        if (!(row[realizedPNL] is System.DBNull))
                        {
                            historyPosition.CostBasisRealizedPNL = Math.Round(double.Parse(row[realizedPNL].ToString()), 4);
                        }
                        if (!(row[startQuantity] is System.DBNull))
                        {
                            historyPosition.StartQty = long.Parse(row[startQuantity].ToString());
                        }
                        if (!(row[netPosition] is System.DBNull))
                        {
                            historyPosition.ParentPositionBalanceQuantity = long.Parse(row[netPosition].ToString());
                        }
                        if (!(row[isPosition] is System.DBNull))
                        {
                            historyPosition.Side = row[isPosition].ToString();
                        }


                        historyPositionList.Add(historyPosition);
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

            return historyPositionList;
        }

    }
}
