using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ExpnlService
{
    public class DatabaseManager
    {
        #region Private variables
        private static DatabaseManager _instance;
        private int _companyID;
        public int CompanyID
        {
            get { return _companyID; }
        }

        ConversionRate _defaultConversionrate;
        #endregion

        #region Singleton Constructor Implementation.
        private DatabaseManager()
        {
            try
            {
                _companyID = GetCompanyID();
                _defaultConversionrate = new ConversionRate();
                _defaultConversionrate.RateValue = 1;
                _defaultConversionrate.ConversionMethod = Operator.M;
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

        public static DatabaseManager GetInstance()
        {
            //ensure if Thread Safe
            if (_instance == null)
            {
                _instance = new DatabaseManager();
            }
            return _instance;
        }
        #endregion

        private static int GetCompanyID()
        {

            int result = int.MinValue;

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetUniqueCompanyID";

                result = Convert.ToInt32(Prana.DatabaseManager.DatabaseManager.ExecuteScalar(queryData));
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

            return result;
        }

        #region Helper Methods

        public static DataSet GetIndexSymbols()
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetIndexSymbols";
            queryData.CommandTimeout = 300;

            try
            {
                ds = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return ds;
        }
        #endregion

        #region Get UserMark
        public static DataSet GetMarkPriceForDatesAndSymbols(string indices, string allDates)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetMarkPriceForDatesAndSymbols";
            queryData.DictionaryDatabaseParameter.Add("@indices", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@indices",
                ParameterType = DbType.String,
                ParameterValue = indices
            });
            queryData.DictionaryDatabaseParameter.Add("@allDates", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@allDates",
                ParameterType = DbType.String,
                ParameterValue = allDates
            });

            try
            {
                ds = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            return ds;
        }

        #endregion

        #region Get Saved Additional Account Items

        #region Get Account Master Account relation
        public static Dictionary<int, int> GetCompanyAccountMasterFundRelation()
        {
            Dictionary<int, int> accountMasterFundRelation = new Dictionary<int, int>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetCompanyFundMasterFundRelationShip";

            try
            {
                using (IDataReader reader = Prana.DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        if (reader[0] != DBNull.Value && reader[2] != DBNull.Value)
                        {
                            accountMasterFundRelation.Add(int.Parse(reader[0].ToString()), int.Parse(reader[2].ToString()));
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
            return accountMasterFundRelation;
        }
        #endregion

        #region Get Strategy Master Strategy relation
        public static Dictionary<int, int> GetCompanyStrategyMasterStrategyRelation()
        {
            Dictionary<int, int> strategyMasterStrategyRelation = new Dictionary<int, int>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetCompanyStrategyMasterStrategyRelationShip";

            try
            {
                using (IDataReader reader = Prana.DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        if (reader[0] != DBNull.Value && reader[2] != DBNull.Value)
                        {
                            strategyMasterStrategyRelation.Add(int.Parse(reader[0].ToString()), int.Parse(reader[2].ToString()));
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
            return strategyMasterStrategyRelation;
        }
        #endregion

        /// <summary>
        /// Nav dictionary is filled in such a way that if the NAV not found for a particular date, then it goes back in time
        ///and fill the NAV corresponding to the found backdate
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        public static void GetStartofDayNAVValues(ref Dictionary<int, NAVStruct> accountwiseNAVDict)
        {
            accountwiseNAVDict = new Dictionary<int, NAVStruct>();

            //NAV value is required only for a day if there, and has no implications of different AUECs.
            //-1 Day added as NAV will be picked for previous day
            //DateTime currentDate = Prana.Utilities.DateTimeUtilities.BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(ExposurePnLScheduler.GetInstance().LatestDate, -1, 1);
            //NAV is pick up similar to cash
            DateTime currentDate = TimeZoneHelper.GetInstance().MostLeadingAUECDateTime(true).AddDays(-1);
            object[] parameter = new object[1];
            parameter[0] = currentDate.Date;

            try
            {
                double totalNAV = 0.0;
                using (IDataReader reader = Prana.DatabaseManager.DatabaseManager.ExecuteReader("GetNAVValueForDateNew", parameter))
                {
                    while (reader.Read())
                    {
                        NAVStruct navStruct = new NAVStruct();
                        int AccountID = int.MinValue;
                        if (reader[0] != DBNull.Value)
                        {
                            AccountID = int.Parse(reader[0].ToString());
                            //navStruct.NAVIndicator = Convert.ToInt16(reader[2].ToString());
                        }
                        if (reader[1] != DBNull.Value)
                        {
                            navStruct.NAVValue = Convert.ToDouble(reader[1].ToString());
                            totalNAV += navStruct.NAVValue;
                            //navStruct.NAVIndicator = Convert.ToInt16(reader[2].ToString());
                        }
                        if (AccountID != int.MinValue)
                        {
                            if (accountwiseNAVDict.ContainsKey(AccountID))
                            {
                                accountwiseNAVDict[AccountID] = navStruct;
                            }
                            else
                            {
                                accountwiseNAVDict.Add(AccountID, navStruct);
                            }
                        }
                    }
                    NAVStruct navStructForConsolidatedData = new NAVStruct();
                    navStructForConsolidatedData.NAVValue = totalNAV;
                    if (accountwiseNAVDict.ContainsKey(int.MinValue))
                    {
                        accountwiseNAVDict[int.MinValue] = navStructForConsolidatedData;
                    }
                    else
                    {
                        accountwiseNAVDict.Add(int.MinValue, navStructForConsolidatedData);
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
        }

        public static void GetShadowNAVValues(ref Dictionary<int, NAVStruct> accountwiseShadowNAVDict)
        {
            accountwiseShadowNAVDict = new Dictionary<int, NAVStruct>();

            //NAV value is required only for a day if there, and has no implications of different AUECs.
            DateTime currentDate = TimeZoneHelper.GetInstance().MostLeadingAUECDateTime(true).Date;
            object[] parameter = new object[1];
            parameter[0] = currentDate.Date;

            try
            {
                double totalNAV = 0.0;
                using (IDataReader reader = Prana.DatabaseManager.DatabaseManager.ExecuteReader("GetNAVValueForDateNew", parameter))
                {
                    while (reader.Read())
                    {
                        NAVStruct navStruct = new NAVStruct();
                        int AccountID = int.MinValue;
                        if (reader[0] != DBNull.Value)
                        {
                            AccountID = int.Parse(reader[0].ToString());
                            //navStruct.NAVIndicator = Convert.ToInt16(reader[2].ToString());
                        }
                        if (reader[1] != DBNull.Value)
                        {
                            navStruct.NAVValue = Convert.ToDouble(reader[1].ToString());
                            totalNAV += navStruct.NAVValue;
                            //navStruct.NAVIndicator = Convert.ToInt16(reader[2].ToString());
                        }
                        if (AccountID != int.MinValue)
                        {
                            if (accountwiseShadowNAVDict.ContainsKey(AccountID))
                            {
                                accountwiseShadowNAVDict[AccountID] = navStruct;
                            }
                            else
                            {
                                accountwiseShadowNAVDict.Add(AccountID, navStruct);
                            }
                        }
                    }
                    NAVStruct navStructForConsolidatedData = new NAVStruct();
                    navStructForConsolidatedData.NAVValue = totalNAV;
                    if (accountwiseShadowNAVDict.ContainsKey(int.MinValue))
                    {
                        accountwiseShadowNAVDict[int.MinValue] = navStructForConsolidatedData;
                    }
                    else
                    {
                        accountwiseShadowNAVDict.Add(int.MinValue, navStructForConsolidatedData);
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
        }

        public static void GetCashProjections(int CompanyId, ref Dictionary<int, double> accountWiseCashProjections, DateTime dateOfCash)
        {
            try
            {
                //NAV value is required only for a day if there, and has no implications of different AUECs.
                object[] parameter = new object[2];
                parameter[0] = CompanyId;
                parameter[1] = dateOfCash;

                try
                {
                    double totalCash = 0.0;

                    using (IDataReader reader = Prana.DatabaseManager.DatabaseManager.ExecuteReader("PM_GetFundWiseCashValue", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            int level1ID = int.Parse(row[0].ToString());
                            double cashProjected = 0.0;
                            if (row[1] != DBNull.Value)
                            {
                                cashProjected = double.Parse(row[1].ToString());
                                totalCash += cashProjected;
                            }
                            else
                            {
                                cashProjected = 0;
                            }

                            if (!accountWiseCashProjections.ContainsKey(level1ID))
                            {
                                accountWiseCashProjections.Add(level1ID, cashProjected);
                            }
                            else
                            {
                                accountWiseCashProjections[level1ID] = cashProjected;
                            }
                        }
                    }
                    if (accountWiseCashProjections.ContainsKey(int.MinValue))
                    {
                        accountWiseCashProjections[int.MinValue] = totalCash;
                    }
                    else
                    {
                        accountWiseCashProjections.Add(int.MinValue, totalCash);
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

        // this method returns the Performance numbers (MTD,QTD,YTD PnL Numbers) and Performance Returns(MTD,QTD,YTD PnL Returns)
        internal void GetYesterdayKeyReturnsAndPnLNumbers(ref Dictionary<int, KeyReturns> accountwiseReturnsDict)
        {
            accountwiseReturnsDict = new Dictionary<int, KeyReturns>();

            //1 is NASDAQ AUECID as in Cash SP.
            //DateTime currentDate = Prana.Utilities.DateTimeUtilities.BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(ExposurePnLScheduler.GetInstance().LatestDate, -1, 1);
            DateTime currentDate = TimeZoneHelper.GetInstance().MostLeadingAUECDateTime(true).AddDays(-1);

            //NAV value is required only for a day if there, and has no implications of different AUECs.
            // -1 Day added as NAV will be picked for previous daycx
            //DateTime currentDate = ExposurePnLScheduler.GetInstance().LatestDate.AddDays(-1);
            object[] parameter = new object[3];
            parameter[0] = currentDate;
            parameter[1] = "";
            parameter[2] = 0;

            try
            {
                using (IDataReader reader = Prana.DatabaseManager.DatabaseManager.ExecuteReader("PMGetDailyPerformanceNumberValuesDateWise", parameter))
                {
                    while (reader.Read())
                    {
                        int accountKey = int.Parse(reader[1].ToString());

                        if (!accountwiseReturnsDict.ContainsKey(accountKey))
                        {
                            accountwiseReturnsDict.Add(accountKey, new KeyReturns(double.Parse(reader[2].ToString()), double.Parse(reader[3].ToString()), double.Parse(reader[4].ToString()), double.Parse(reader[5].ToString()), double.Parse(reader[6].ToString()), double.Parse(reader[7].ToString())));
                        }
                        else
                        {
                            accountwiseReturnsDict[accountKey] = new KeyReturns(double.Parse(reader[2].ToString()), double.Parse(reader[3].ToString()), double.Parse(reader[4].ToString()), double.Parse(reader[5].ToString()), double.Parse(reader[6].ToString()), double.Parse(reader[7].ToString()));
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
        }
        #endregion Get Saved Additional Account Items

        #region Clearance related methods
        public void SaveClearanceTime(DataTable auecIDClearanceTimeTable)
        {
            try
            {
                System.IO.MemoryStream msSerializedXML = null;
                msSerializedXML = new System.IO.MemoryStream();
                auecIDClearanceTimeTable.WriteXml(msSerializedXML);
                string xmlStr = System.Text.ASCIIEncoding.ASCII.GetString(msSerializedXML.ToArray());

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveandUpdatePMClearance";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = xmlStr
                });

                Prana.DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
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

        public void SaveBaseTimeZoneAndBaseTimeZoneTime(string timeZone, DateTime baseTime)
        {
            object[] parameter = new object[2];

            parameter[0] = timeZone;
            parameter[1] = baseTime;

            try
            {
                Prana.DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SavePMClearanceBaseSettings", parameter);
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

        public static TimeZoneAndTime GetBaseTimeZoneAndBaseTimeZoneTime()
        {
            TimeZoneAndTime timeZoneAndTime = new TimeZoneAndTime();
            try
            {
                QueryData querydata = new QueryData();
                querydata.StoredProcedureName = "P_GetPMClearanceBaseSettings";

                using (IDataReader reader = Prana.DatabaseManager.DatabaseManager.ExecuteReader(querydata))
                {
                    while (reader.Read())
                    {
                        timeZoneAndTime.TimeZone = reader[0].ToString();
                        timeZoneAndTime.BaseTime = DateTime.Parse(reader[1].ToString());
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
            return timeZoneAndTime;
        }
        #endregion

        public static Dictionary<string, SymbolPriceWithDate> GetBetaValueDateWise(string AUECString)
        {
            Dictionary<string, SymbolPriceWithDate> symbolWiseBeta = null;

            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetBetaForDate";
            queryData.CommandTimeout = 300;
            queryData.DictionaryDatabaseParameter.Add("@AllAUECDatesString", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@AllAUECDatesString",
                ParameterType = DbType.String,
                ParameterValue = AUECString
            });

            try
            {
                ds = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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

            if (ds != null && ds.Tables != null)
            {
                DataTable betaValues = ds.Tables[0];
                symbolWiseBeta = new Dictionary<string, SymbolPriceWithDate>(betaValues.Rows.Count);

                for (int i = 0; i < betaValues.Rows.Count; i++)
                {
                    SymbolPriceWithDate betaObj = new SymbolPriceWithDate();
                    double beta = 1;
                    if (betaValues.Rows[i][0] != null && betaValues.Rows[i][0].ToString() != string.Empty)
                    {
                        beta = double.Parse(betaValues.Rows[i][0].ToString());
                    }
                    int indicator = Convert.ToInt32(betaValues.Rows[i][1].ToString());
                    DateTime dateActual = DateTime.Parse(betaValues.Rows[i][2].ToString());
                    DateTime daterequired = DateTime.Parse(betaValues.Rows[i][4].ToString());
                    string uppercaseSymbol = betaValues.Rows[i][3].ToString().Trim().ToUpper();
                    if (!symbolWiseBeta.ContainsKey(uppercaseSymbol))
                    {
                        betaObj.Price = beta;
                        betaObj.Indicator = indicator;
                        betaObj.DateActual = dateActual;
                        betaObj.DateRequired = daterequired;
                        betaObj.Symbol = uppercaseSymbol;
                        symbolWiseBeta.Add(uppercaseSymbol, betaObj);
                    }
                    else
                    {
                        symbolWiseBeta[uppercaseSymbol].Price = beta;
                        symbolWiseBeta[uppercaseSymbol].Indicator = indicator;
                        symbolWiseBeta[uppercaseSymbol].DateActual = dateActual;
                        symbolWiseBeta[uppercaseSymbol].DateRequired = daterequired;
                        symbolWiseBeta[uppercaseSymbol].Symbol = uppercaseSymbol;
                    }
                }
            }
            return symbolWiseBeta;
        }

        public static Dictionary<string, double> GetOutstandingsValueDateWise(string AUECString)
        {
            Dictionary<string, double> symbolWiseOutstandings = null;
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetSharesOutsandingForDate";
            queryData.DictionaryDatabaseParameter.Add("@AllAUECDatesString", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@AllAUECDatesString",
                ParameterType = DbType.String,
                ParameterValue = AUECString
            });

            try
            {
                ds = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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

            if (ds != null && ds.Tables != null)
            {
                DataTable outstandings = ds.Tables[0];
                symbolWiseOutstandings = new Dictionary<string, double>(outstandings.Rows.Count);

                for (int i = 0; i < outstandings.Rows.Count; i++)
                {
                    if (!symbolWiseOutstandings.ContainsKey(outstandings.Rows[i][2].ToString().Trim().ToUpper()))
                    {
                        symbolWiseOutstandings.Add(outstandings.Rows[i][2].ToString().Trim().ToUpper(), double.Parse(outstandings.Rows[i][0].ToString()));
                    }
                }
            }
            return symbolWiseOutstandings;
        }

        internal Dictionary<int, ConversionRate> GetYesterdayFXRates(string yesterdayAUECString)
        {
            Dictionary<int, ConversionRate> yesterdayFXRates = new Dictionary<int, ConversionRate>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetFXConversionRatesForYesterday";
            queryData.CommandTimeout = 300;
            queryData.DictionaryDatabaseParameter.Add("@YesterdayAUECDatesString", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@YesterdayAUECDatesString",
                ParameterType = DbType.String,
                ParameterValue = yesterdayAUECString
            });

            int FromCurrencyID = 0;
            int Rate = 2;
            int ConversionMethod = 3;
            int Date = 4;
            int FXeSignalSymbol = 5;

            try
            {
                using (IDataReader reader = Prana.DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        int keyCurrencyID = 0;
                        double rateValue = 0;
                        Operator conversionMethod = Operator.M;
                        DateTime date = DateTimeConstants.MinValue;
                        string fxeSignalSymbol = string.Empty;

                        // level 2 ID
                        if (row[FromCurrencyID] != DBNull.Value)
                        {
                            keyCurrencyID = int.Parse(row[FromCurrencyID].ToString());
                        }

                        if (row[Rate] != DBNull.Value)
                        {
                            rateValue = Convert.ToDouble(row[Rate].ToString());
                        }

                        if (row[ConversionMethod] != DBNull.Value)
                        {
                            conversionMethod = (Operator)int.Parse(row[ConversionMethod].ToString());
                        }

                        if (row[Date] != DBNull.Value)
                        {
                            date = Convert.ToDateTime(row[Date].ToString());
                        }

                        if (row[FXeSignalSymbol] != DBNull.Value)
                        {
                            fxeSignalSymbol = row[FXeSignalSymbol].ToString();
                        }

                        ConversionRate rate = new ConversionRate();
                        rate.ConversionMethod = conversionMethod;
                        rate.Date = date;
                        rate.FXeSignalSymbol = fxeSignalSymbol;
                        rate.RateValue = rateValue;

                        if (!yesterdayFXRates.ContainsKey(keyCurrencyID))
                        {
                            yesterdayFXRates.Add(keyCurrencyID, rate);
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
            return yesterdayFXRates;
        }

        internal Prana.BusinessObjects.Classes.EPNL_Business_Objects.PMPrefData GetPMPrefDataFromDB()
        {
            Prana.BusinessObjects.Classes.EPNL_Business_Objects.PMPrefData prefData = new Prana.BusinessObjects.Classes.EPNL_Business_Objects.PMPrefData();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetPMPreferencesOnEPNL";

            try
            {
                using (IDataReader reader = Prana.DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value)
                        {
                            prefData.UseClosingMark = bool.Parse(row[1].ToString());
                        }
                        if (row[1] != DBNull.Value)
                        {
                            prefData.XPercentofAvgVolume = double.Parse(row[2].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return prefData;
        }

        internal PMUIPrefs GetCompanyPMPreferences()
        {
            PMUIPrefs uiPrefs = new PMUIPrefs();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = _companyID;
                try
                {
                    using (IDataReader reader = Prana.DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyPMUIPrefs", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);

                            if (row[0] != DBNull.Value)
                            {
                                uiPrefs.NumberOfCustomViewsAllowed = Convert.ToInt32(row[0].ToString());
                            }
                            if (row[1] != DBNull.Value)
                            {
                                uiPrefs.NumberOfVisibleColumnsAllowed = Convert.ToInt32(row[1].ToString());
                            }
                            if (row[2] != DBNull.Value)
                            {
                                uiPrefs.FetchDataFromHistoricalDb = Convert.ToBoolean(row[2].ToString());
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
            return uiPrefs;
        }

        internal Dictionary<int, PMCalculationPrefs> GetCompanyPMCalculationPreferences()
        {
            Dictionary<int, PMCalculationPrefs> _pmCalculationPrefCollection = new Dictionary<int, PMCalculationPrefs>();
            PMCalculationPrefs pmCalcPrefs;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = _companyID;
                using (IDataReader reader = Prana.DatabaseManager.DatabaseManager.ExecuteReader("GetPMCalculationPrefValues", parameter))
                {
                    while (reader.Read())
                    {
                        pmCalcPrefs = new PMCalculationPrefs();

                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[1] != DBNull.Value)
                        {
                            pmCalcPrefs.HighWaterMark = Convert.ToDouble(row[1].ToString());
                        }
                        if (row[2] != DBNull.Value)
                        {
                            pmCalcPrefs.StopOut = Convert.ToDouble(row[2].ToString());
                        }
                        if (row[3] != DBNull.Value)
                        {
                            pmCalcPrefs.TraderPayoutPercent = Convert.ToDouble(row[3].ToString());
                        }
                        if (row[0] != DBNull.Value)
                        {
                            if (!_pmCalculationPrefCollection.ContainsKey(Convert.ToInt32(row[0].ToString())))
                            {
                                _pmCalculationPrefCollection.Add(Convert.ToInt32(row[0].ToString()), pmCalcPrefs);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _pmCalculationPrefCollection;
        }

        internal Dictionary<int, double> GetStartOfMonthCapitalAccountValues(DateTime latestDate)
        {
            Dictionary<int, double> startOfMonthCapitalAccountCollection = null;
            DataSet ds = null;
            DateTime fromDate = new DateTime(latestDate.Year, latestDate.Month, 01);

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "GetStartOfMonthCAValuesDateWise";
                queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FromDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = fromDate
                });

                ds = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (ds != null && ds.Tables != null)
                {
                    DataTable dtStartOfMonthCA = ds.Tables[0];
                    startOfMonthCapitalAccountCollection = new Dictionary<int, double>(dtStartOfMonthCA.Rows.Count);

                    for (int i = 0; i < dtStartOfMonthCA.Rows.Count; i++)
                    {
                        if (!startOfMonthCapitalAccountCollection.ContainsKey(Convert.ToInt32(dtStartOfMonthCA.Rows[i][0])))
                        {
                            startOfMonthCapitalAccountCollection.Add(Convert.ToInt32(dtStartOfMonthCA.Rows[i][0]), double.Parse(dtStartOfMonthCA.Rows[i][1].ToString()));
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
            return startOfMonthCapitalAccountCollection;
        }

        internal Dictionary<int, double> GetUserDefinedMTDPnLValues(DateTime latestDate)
        {
            Dictionary<int, double> userDefinedMTDPnLCollection = null;
            DataSet ds = null;
            DateTime fromDate = latestDate.AddDays(-1);
            try
            {
                // first date of month MTD PnL is 0
                if (latestDate.Day.Equals(1))
                {
                    userDefinedMTDPnLCollection = new Dictionary<int, double>();
                    return userDefinedMTDPnLCollection;
                }

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "GetUserDefinedMTDPnLValuesDateWise";
                queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FromDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = fromDate
                });

                ds = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (ds != null && ds.Tables != null)
                {
                    DataTable dtUserDefinedMTDPnL = ds.Tables[0];
                    userDefinedMTDPnLCollection = new Dictionary<int, double>(dtUserDefinedMTDPnL.Rows.Count);

                    for (int i = 0; i < dtUserDefinedMTDPnL.Rows.Count; i++)
                    {
                        if (!userDefinedMTDPnLCollection.ContainsKey(Convert.ToInt32(dtUserDefinedMTDPnL.Rows[i][0])))
                        {
                            userDefinedMTDPnLCollection.Add(Convert.ToInt32(dtUserDefinedMTDPnL.Rows[i][0]), double.Parse(dtUserDefinedMTDPnL.Rows[i][1].ToString()));
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
            return userDefinedMTDPnLCollection;
        }

        internal DataTable GetUserAccountMapping()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("FundID", typeof(int));

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetUserFundMapping";

            try
            {
                using (IDataReader reader = Prana.DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        DataRow row = dt.NewRow();

                        row[0] = Convert.ToInt32(reader[0].ToString());
                        row[1] = Convert.ToInt32(reader[1].ToString());
                        dt.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dt;

        }

        public static Dictionary<string, SymbolPriceWithDate> GetBetaValueDateWiseForZeroBetas(Dictionary<string, DateTime> dateWiseSymbol)
        {
            Dictionary<string, SymbolPriceWithDate> symbolWiseBeta = null;
            try
            {
                List<string> symbols = new List<string>();
                List<string> auecDates = new List<string>();
                foreach (KeyValuePair<string, DateTime> entry in dateWiseSymbol)
                {
                    if (!symbols.Contains(entry.Key))
                    {
                        symbols.Add(entry.Key);
                        auecDates.Add(entry.Value.ToString());
                    }
                }
                string symbolString = string.Join(",", symbols.ToArray());
                string dateString = string.Join(",", auecDates.ToArray());
                DataSet ds = null;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "GetBetaForDateForZeroBetas";
                queryData.CommandTimeout = 300;
                queryData.DictionaryDatabaseParameter.Add("@symbollist", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@symbollist",
                    ParameterType = DbType.String,
                    ParameterValue = symbolString
                });
                queryData.DictionaryDatabaseParameter.Add("@auecDateList", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@auecDateList",
                    ParameterType = DbType.String,
                    ParameterValue = dateString
                });

                try
                {
                    ds = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
                if (ds != null && ds.Tables != null)
                {
                    DataTable betaValues = ds.Tables[0];
                    symbolWiseBeta = new Dictionary<string, SymbolPriceWithDate>(betaValues.Rows.Count);
                    for (int i = 0; i < betaValues.Rows.Count; i++)
                    {
                        SymbolPriceWithDate betaObj = new SymbolPriceWithDate();
                        double beta = 1;
                        if (betaValues.Rows[i][0] != null && betaValues.Rows[i][0].ToString() != string.Empty)
                        {
                            beta = double.Parse(betaValues.Rows[i][0].ToString());
                        }
                        int indicator = Convert.ToInt32(betaValues.Rows[i][1].ToString());
                        DateTime dateActual = DateTime.Parse(betaValues.Rows[i][2].ToString());
                        DateTime dateRequired = DateTime.Parse(betaValues.Rows[i][4].ToString());
                        string uppercaseSymbol = betaValues.Rows[i][3].ToString().Trim().ToUpper();
                        if (!symbolWiseBeta.ContainsKey(uppercaseSymbol))
                        {
                            betaObj.Price = beta;
                            betaObj.Indicator = indicator;
                            betaObj.DateActual = dateActual;
                            betaObj.DateRequired = dateRequired;
                            betaObj.Symbol = uppercaseSymbol;
                            symbolWiseBeta.Add(uppercaseSymbol, betaObj);
                        }
                        else
                        {
                            symbolWiseBeta[uppercaseSymbol].Price = beta;
                            symbolWiseBeta[uppercaseSymbol].Indicator = indicator;
                            symbolWiseBeta[uppercaseSymbol].DateActual = dateActual;
                            symbolWiseBeta[uppercaseSymbol].DateRequired = dateRequired;
                            symbolWiseBeta[uppercaseSymbol].Symbol = uppercaseSymbol;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return symbolWiseBeta;
        }

        internal static DataSet GetAvgVolumeSymbolWise(DateTime fromDate, int days)
        {
            try
            {
                return Prana.DatabaseManager.DatabaseManager.ExecuteDataSet("spGetAverageVolume",
                    new object[] { fromDate, days, 1 }, ConfigurationHelper.HISTORICAL_DATA_CONNECTION_STRING);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        internal static Dictionary<int, double> GetAccountWiseNra()
        {
            try
            {
                Dictionary<int, double> accountWiseNra = new Dictionary<int, double>();
                DataSet ds = null;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetFundWiseNRA";

                ds = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int key = Convert.ToInt32(dr["FundId"].ToString());
                    double nra = Convert.ToDouble(dr["FundNRA"].ToString());
                    if (accountWiseNra.ContainsKey(key))
                        accountWiseNra[key] = nra;
                    else
                        accountWiseNra.Add(key, nra);
                }
                return accountWiseNra;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }
    }
}