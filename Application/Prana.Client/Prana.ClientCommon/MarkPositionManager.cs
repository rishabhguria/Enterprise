using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Data;

namespace Prana.ClientCommon
{
    public class MarkPositionManager
    {
        private static int _errorNumber = 0;
        private static string _errorMessage = string.Empty;

        //Module: Close Trade/PM
        /// <summary>
        /// Gets the MarkPrice for the specified Symbol and Date.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double GetMarkPriceForSymbolAndDate(string symbol, DateTime date)
        {
            //DataSet ds = null;
            double todaysMarkPrice = 0;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetMarkPriceForSymbolAndDate";
            queryData.DictionaryDatabaseParameter.Add("@Symbol", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Symbol",
                ParameterType = DbType.String,
                ParameterValue = symbol
            });
            queryData.DictionaryDatabaseParameter.Add("@day", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@day",
                ParameterType = DbType.Date,
                ParameterValue = date
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
                todaysMarkPrice = Convert.ToDouble(DatabaseManager.DatabaseManager.ExecuteScalar(queryData));
                //ds = db.ExecuteDataSet(commandSP);
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

            return todaysMarkPrice;

        }

        /// <summary>
        /// Gets all stored month mark prices.
        /// </summary>
        /// <returns></returns>
        public static MonthMarkPriceList GetAllStoredMonthMarkPricesForCurrentMonth()
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllStoredMonthMarkPricesForCurrentMonth";

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
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

            return FillMonthMarkPricesList(ds);

        }

        /// <summary>
        /// Fills the month mark prices list for all open positions.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        private static MonthMarkPriceList FillMonthMarkPricesList(DataSet ds)
        {
            MonthMarkPriceList monthMarkPricesListForAllOpenPositions = new MonthMarkPriceList();

            const int symbol = 0;
            const int finalMarkPrice = 1;
            const int month = 2;


            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    MonthMarkPrice monthMarkPriceItem = new MonthMarkPrice();

                    try
                    {
                        if (!(row[symbol] is System.DBNull))
                        {
                            monthMarkPriceItem.Symbol = row[symbol].ToString();
                        }
                        if (!(row[finalMarkPrice] is System.DBNull))
                        {
                            monthMarkPriceItem.FinalMarkPrice = Convert.ToDouble(row[finalMarkPrice].ToString());
                        }
                        if (!(row[month] is System.DBNull))
                        {
                            monthMarkPriceItem.Month = Convert.ToString(row[month]);
                        }
                        monthMarkPricesListForAllOpenPositions.Add(monthMarkPriceItem);

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

            return monthMarkPricesListForAllOpenPositions;
        }

        #region MarkPrice_Dated_20071224_And_ForexCoversion_Code
        /// <summary>
        /// Returns data table having symbols and mark prices for the asked date(s).
        /// </summary>
        /// <param name="date">Not used for fetching result from database.</param>
        /// <param name="orderSeqNumber">Not used for fetching result from database.</param>
        /// <param name="typeOfAllocation">Not used for fetching result from database.</param>
        /// <param name="fromDate">Starting date from where the data is to be picked.</param>
        /// <param name="toDate">Ending date from where the data is to be picked.</param>
        /// <param name="dateMethodology">Data regarding day or month methodology</param>
        /// <returns></returns>
        public static DataTable GetMarkPricesForGivenDate(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllSymbolsMarkPriceForDayInSystem_Updated";

            queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });
            queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
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

            return FillDataTable(ds);

        }

        private static DataTable FillDataTable(DataSet ds)
        {
            //DayMarkPriceList dayMarkPricesListForAllSymbolsInSystem = new DayMarkPriceList();

            //const int symbol = 0;
            //const int applicationMarkPrice = 1;
            //const int finalMarkPrice = 2;
            //const int markPriceID = 3;
            //const int dayDate = 4;

            DataTable dt = new DataTable();
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            if (ds != null && ds.Tables != null)
            {
                try
                {
                    dt = ds.Tables[0];
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

            return dt;
        }


        public static DataTable GetBetaValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllSymbolsBeta";

            queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });
            queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
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

            return FillDataTable(ds);

        }



        public static DataTable GetTradingVolDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllSymbolsTradingVol";

            queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });
            queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
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

            return FillDataTable(ds);

        }


        public static DataTable GetDeltaValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllSymbolsDelta";

            queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });
            queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
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

            return FillDataTable(ds);

        }

        /// <summary>
        /// Saves the mark prices in database.
        /// </summary>
        /// <param name="dtMarkPrices">Table having symbol and respective mark prices for the date(s).</param>
        /// <returns>Number of rows affected by saving in database.</returns>
        public static int SaveMarkPrices(DataTable dtMarkPrices)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtMarkPrices.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PM_SaveMarkPrices";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtBeta"></param>
        /// <returns></returns>
        public static int SaveBeta(DataTable dtBeta)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtBeta.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PM_SaveDailyBeta";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtTradingVol"></param>
        /// <returns></returns>
        public static int SaveTradingVolume(DataTable dtTradingVol)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtTradingVol.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PM_SaveDailyTradingVol";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtDelta"></param>
        /// <returns></returns>
        public static int SaveDelta(DataTable dtDelta)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtDelta.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PM_SaveDailyDelta";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;

        }

        public static int SaveOutStandings(DataTable dtOutStanding)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtOutStanding.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PM_SaveDailyOutStanding";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;

        }

        /// <summary>
        /// Saves the forex rate with their respective date.
        /// </summary>
        /// <param name="dtForexRate">Table having all the information regarding forex rate for the to and from currency specified along with their respective date.</param>
        /// <returns>Number of rows affected by saving in database.</returns>
        public static int SaveForexRate(DataTable dtForexRate)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtForexRate.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMSaveForexRateNew";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;

        }

        /// <summary>
        /// Saves the NAV values respective date.
        /// </summary>
        /// <param name="dtForexRate">Table having all the information regarding forex rate for the to and from currency specified along with their respective date.</param>
        /// <returns>Number of rows affected by saving in database.</returns>
        public static int SaveNAVValues(DataTable dtNAVValue)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtNAVValue.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMSaveNAVValue";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;

        }

        /// <summary>
        /// Returns data table having forex conversion data
        /// </summary>
        /// <param name="fromDate">Starting date range from where the data is to be taken.</param>
        /// <param name="toDate">Ending date range from where the data is to be taken.</param>
        /// <param name="dateMethodology"></param>
        /// <returns>Data regarding day or month methodology.</returns>
        public static DataTable GetConversionRateDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetConversionRateDateWiseNew";

            queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });
            queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
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

            return FillConversionRateDateWise(ds);

        }

        private static DataTable FillConversionRateDateWise(DataSet ds)
        {

            DataTable dt = new DataTable();
            if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null)
            {
                try
                {
                    dt = ds.Tables[0];
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

            return dt;
        }

        public static DataTable GetNAVValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetNavValuesDateWise";

            queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });
            queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
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

            return FillDataTable(ds);

        }

        public static DataTable GetPerformanceNumberValueDateWise(DateTime fromDate)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetDailyPerformanceNumberValuesDateWise";
            queryData.DictionaryDatabaseParameter.Add("@date", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@date",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
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
            return FillDataTable(ds);

        }






        /// <summary>
        /// Returns distinct currencies.
        /// </summary>
        /// <returns>Currencies information regarding their name, symbol and id.</returns>
        public static Prana.BusinessObjects.CurrencyCollection GetCurrenciesWithSymbol()
        {

            CurrencyCollection currencyCollection = new CurrencyCollection();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCurrenciesWithSymbol";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        currencyCollection.Add(FillCurrencyWithSymbol(row, 0));
                    }
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
            currencyCollection.Insert(0, new Prana.BusinessObjects.Currency(0, ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));
            return currencyCollection;
        }

        private static Currency FillCurrencyWithSymbol(object[] row, int offset)
        {

            if (offset < 0)
            {
                offset = 0;
            }

            Currency currency = null;
            try
            {
                if (row != null)
                {
                    currency = new Currency();

                    int CURRENCY_ID = offset + 0;
                    int CURRENCY_NAME = offset + 1;
                    int CURRENCY_SYMBOL = offset + 2;

                    currency.CurrencyID = Convert.ToInt32(row[CURRENCY_ID]);
                    currency.CurrencyName = Convert.ToString(row[CURRENCY_NAME]);
                    currency.Symbol = Convert.ToString(row[CURRENCY_SYMBOL]);
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
            return currency;
        }


        #endregion

        #region DailyCash

        public static DataTable GetDailyCash(DateTime fromDate)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetDailyCash";
            queryData.DictionaryDatabaseParameter.Add("@date", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@date",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
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

            return FillDataTable(ds);

        }



        /// <summary>
        /// Saves the cash value in database.
        /// </summary>
        /// <param name="dtDailyCashValues">Table having cash value for a given date.</param>
        /// <returns>Number of rows affected by saving in database.</returns>
        public static int SaveDailyCashValues(DataTable dtDailyCashValues)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtDailyCashValues.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PM_SaveDailyCashValues";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;

        }

        public static int SavePerformanceNumberValues(DataTable dtPerformanceNumberValuesTemp)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtPerformanceNumberValuesTemp.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PM_SavePerformanceNumberValues";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;

        }

        public static int DeleteDailyCashValue(int accountID, int localCurrencyID, int baseCurrencyID, DateTime date)
        {
            int deletedRow = int.MinValue;
            try
            {
                Object[] parameter = new object[4];
                parameter[0] = accountID;
                parameter[1] = localCurrencyID;
                parameter[2] = baseCurrencyID;
                parameter[3] = date;

                deletedRow = int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("PMDeleteDailyCashValue", parameter).ToString());
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
            return deletedRow;
        }

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static DataTable GetPositions(DateTime todayDate, String allocationSPName)
        {
            DataSet dsPositions = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = allocationSPName;
                queryData.CommandTimeout = 500;
                queryData.DictionaryDatabaseParameter.Add("@date", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@date",
                    ParameterType = DbType.DateTime,
                    ParameterValue = todayDate
                });

                dsPositions = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

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
            return dsPositions.Tables[0]; ;
        }



    }
}
