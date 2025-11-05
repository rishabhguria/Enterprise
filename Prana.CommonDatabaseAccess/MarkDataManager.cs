using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.CommonDatabaseAccess
{
    public class MarkDataManager : IMarkDataManager
    {
        private static int _errorNumber = 0;
        private static string _errorMessage = string.Empty;
        private static bool _isFundWiseMarkPrice = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("isFundWiseMarkPriceRequired"));
        private static Dictionary<int, List<int>> _masterFundSubAccountAssociation;
        private static Dictionary<int, string> _masterFunds;
        private static DataTable _company;
        private static int _companyID;

        /// <summary>
        /// Gets the MarkPrice for the specified Symbol and Date.
        /// Module: Close Trade/PM
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public double GetMarkPriceForSymbolAndDate(string symbol, DateTime date, int accountId)
        {
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
            queryData.DictionaryDatabaseParameter.Add("@account", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@account",
                ParameterType = DbType.Int32,
                ParameterValue = accountId
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
                todaysMarkPrice = Convert.ToDouble(DatabaseManager.DatabaseManager.ExecuteScalar(queryData));
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
        /// Gets the mark price for taxlot list.
        /// </summary>
        /// <param name="TaxLotList">The tax lot list.</param>
        /// <returns></returns>
        public DataTable GetMarkPricesForSymbolsAndDates(DataTable dtMarkPrices)
        {
            DataSet ds = new DataSet();
            try
            {
                ds.Tables.Add(dtMarkPrices.Copy());
                string generatedXml = string.Empty;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetAllMarkPricesForSymbolsAndDates";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
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
        /// Gets the mark prices for symbols and exact dates.
        /// </summary>
        /// <param name="dtMarkPrices">The dt mark prices.</param>
        /// <returns></returns>
        public DataSet GetMarkPricesForSymbolsAndExactDates(DataTable dtMarkPrices)
        {
            DataSet ds = new DataSet();
            try
            {
                ds.Tables.Add(dtMarkPrices.Copy());
                string generatedXml = string.Empty;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetAllMarkPricesForSymbolsAndExactDates";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
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
            return ds;
        }

        /// <summary>
        /// Gets all stored month mark prices.
        /// </summary>
        /// <returns></returns>
        public MonthMarkPriceList GetAllStoredMonthMarkPricesForCurrentMonth()
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

        public Dictionary<string, double> GetSplitFactors(string AUECString)
        {
            Dictionary<string, double> symbolWiseSplitFactor = null;
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetTodaysSplitFactor";
            queryData.DictionaryDatabaseParameter.Add("@AllAUECDatesString", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@AllAUECDatesString",
                ParameterType = DbType.String,
                ParameterValue = AUECString
            });

            try
            {
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (ds != null && ds.Tables != null)
                {
                    DataTable splitFactordt = ds.Tables[0];
                    symbolWiseSplitFactor = new Dictionary<string, double>(splitFactordt.Rows.Count);

                    for (int i = 0; i < splitFactordt.Rows.Count; i++)
                    {
                        string uppercaseSymbol = splitFactordt.Rows[i]["Symbol"].ToString().Trim().ToUpper();
                        if (!symbolWiseSplitFactor.ContainsKey(uppercaseSymbol))
                        {
                            double splitFactor = double.Parse(splitFactordt.Rows[i]["SplitFactor"].ToString());
                            symbolWiseSplitFactor.Add(uppercaseSymbol, splitFactor);
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
            return symbolWiseSplitFactor;
        }

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
        public DataTable GetMarkPricesForGivenDate(DateTime date, int dateMethodology, bool isFxFXForwardData, bool getClosedData)
        {
            DataSet ds = null;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllSymbolsMarkPriceForDayInSystem_Updated";
            queryData.CommandTimeout = 300;
            queryData.DictionaryDatabaseParameter.Add("@Date", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Date",
                ParameterType = DbType.DateTime,
                ParameterValue = date
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });
            queryData.DictionaryDatabaseParameter.Add("@isFxFXForwardData", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@isFxFXForwardData",
                ParameterType = DbType.Boolean,
                ParameterValue = isFxFXForwardData
            });
            queryData.DictionaryDatabaseParameter.Add("@GetSameDayClosedData", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@GetSameDayClosedData",
                ParameterType = DbType.Boolean,
                ParameterValue = getClosedData
            });
            queryData.DictionaryDatabaseParameter.Add("@isFundWiseMarkPrice", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@isFundWiseMarkPrice",
                ParameterType = DbType.Boolean,
                ParameterValue = _isFundWiseMarkPrice
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

            sw.Stop();
            return FillDataTable(ds);
        }

        /// <summary>
        /// CreatedBY: Bharat Raturi
        /// get the details of the mark prices for date range
        /// </summary>
        /// <param name="xmlAccounts"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="dateMethodology"></param>
        /// <param name="isFxFXForwardData"></param>
        /// <returns></returns>
        public DataTable GetMarkPricesForGivenDateRange(string xmlAccounts, DateTime startDate, DateTime endDate, int dateMethodology, bool isFxFXForwardData, int filter)
        {
            DataSet ds = null;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllSymbolsMarkPriceForDayInSystem_Updated_Filter";
            queryData.CommandTimeout = 600;
            queryData.DictionaryDatabaseParameter.Add("@fundIDs", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@fundIDs",
                ParameterType = DbType.String,
                ParameterValue = xmlAccounts
            });
            queryData.DictionaryDatabaseParameter.Add("@Date", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Date",
                ParameterType = DbType.DateTime,
                ParameterValue = startDate
            });
            queryData.DictionaryDatabaseParameter.Add("@isFxFXForwardData", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@isFxFXForwardData",
                ParameterType = DbType.Boolean,
                ParameterValue = isFxFXForwardData
            });
            queryData.DictionaryDatabaseParameter.Add("@endDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@endDate",
                ParameterType = DbType.DateTime,
                ParameterValue = endDate
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });
            queryData.DictionaryDatabaseParameter.Add("@filter", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@filter",
                ParameterType = DbType.Int32,
                ParameterValue = filter
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
            sw.Stop();
            return FillDataTable(ds);
        }

        /// <summary>
        /// Method to get the list of pricing rules
        /// </summary>
        /// <returns>The list of pricing rules</returns>
        public List<PricingRule> GetPricingRules()
        {
            List<PricingRule> pricingRuleList = new List<PricingRule>();

            try
            {
                QueryData dbPricingRule = new QueryData();
                dbPricingRule.StoredProcedureName = "P_GetAllPricingRulesForMarkPrice";

                using (IDataReader drPricing = DatabaseManager.DatabaseManager.ExecuteReader(dbPricingRule))
                {
                    while (drPricing.Read())
                    {
                        PricingRule objPriceRule = new PricingRule();

                        if (drPricing.GetValue(0) != DBNull.Value)
                        {
                            objPriceRule.RuleID = drPricing.GetInt32(0);
                        }
                        if (drPricing.GetValue(1) != DBNull.Value)
                        {
                            objPriceRule.AccountID = drPricing.GetInt32(1);
                        }
                        if (drPricing.GetValue(2) != DBNull.Value)
                        {
                            objPriceRule.AssetClassID = drPricing.GetInt32(2);
                        }
                        if (drPricing.GetValue(3) != DBNull.Value)
                        {
                            objPriceRule.ExchangeID = drPricing.GetInt32(3);
                        }
                        if (drPricing.GetValue(4) != DBNull.Value)
                        {
                            objPriceRule.PricingDataTypeID = drPricing.GetInt32(4);
                        }
                        if (drPricing.GetValue(5) != DBNull.Value)
                        {
                            objPriceRule.SourceID = drPricing.GetInt32(5);
                        }
                        if (drPricing.GetValue(6) != DBNull.Value)
                        {
                            objPriceRule.SecondarySourceID = drPricing.GetInt32(6);
                        }
                        if (drPricing.GetValue(7) != DBNull.Value)
                        {
                            objPriceRule.CompanyID = drPricing.GetInt32(7);
                        }
                        if (drPricing.GetValue(8) != DBNull.Value)
                        {
                            objPriceRule.SecondarySource = drPricing.GetString(8);
                        }
                        if (drPricing.GetValue(9) != DBNull.Value)
                        {
                            objPriceRule.IsPricingPolicy = drPricing.GetBoolean(9);
                        }
                        if (drPricing.GetValue(10) != DBNull.Value)
                        {
                            objPriceRule.PricingPolicyID = drPricing.GetInt32(10);
                        }

                        pricingRuleList.Add(objPriceRule);
                    }
                }
                return pricingRuleList;
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

        public DataTable GetBusinessAdjustedDayMarkPriceForGivenDate(DateTime date)
        {
            DataSet ds = null;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllSymbolsBusinessAdjustedDayMarkPriceForGivenDate";
            queryData.CommandTimeout = 600;
            queryData.DictionaryDatabaseParameter.Add("@Date", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Date",
                ParameterType = DbType.DateTime,
                ParameterValue = date
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

            sw.Stop();
            return FillDataTable(ds);
        }

        public DataTable GetBusinessAdjustedDayMarkPriceForGivenDateCH(DateTime date)
        {
            DataSet ds = null;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetFundSymbolsDayMarkPriceForGivenDate";
            queryData.CommandTimeout = 600;
            queryData.DictionaryDatabaseParameter.Add("@Date", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Date",
                ParameterType = DbType.DateTime,
                ParameterValue = date
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

            sw.Stop();
            return FillDataTable(ds);
        }

        /// <summary>
        /// Narendra Kumar Jangir 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable FetchMarkPricesAccountWiseForLastBusinessDay()
        {
            DataSet ds = null;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_FetchMarkPricesFundWiseForLastBusinessDay";
            queryData.CommandTimeout = 600;

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

            sw.Stop();
            return FillDataTable(ds);
        }

        private static DataTable FillDataTable(DataSet ds)
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

        public DataTable GetBetaValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
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
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });
            queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
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

        public DataTable GetTradingVolDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
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
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });
            queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
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

        public DataTable GetDeltaValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
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
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });
            queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
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

        public DataTable GetOutSatndingDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllSymbolsOutStanding";
            queryData.CommandTimeout = 300;
            queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });
            queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
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

        public DataTable GetVolatilityValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllSymbolsVolatility";
            queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });
            queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
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
        /// Get VWAP Value Date Wise
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="dateMethodology"></param>
        /// <returns></returns>
        public DataTable GetVWAPValueDateWise(DateTime fromDate, int dateMethodology, bool getSameDayClosedDataOnDV)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllSymbolsVWAP";
            queryData.DictionaryDatabaseParameter.Add("@Date", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Date",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });
            queryData.DictionaryDatabaseParameter.Add("@GetSameDayClosedData", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@GetSameDayClosedData",
                ParameterType = DbType.Boolean,
                ParameterValue = getSameDayClosedDataOnDV
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



        public DataTable GetDividendYieldValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllSymbolsDividendYield";
            queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });
            queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
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
        /// Get the Data from DB
        /// </summary>
        /// <param name="requestField">parameter to decide SP</param>
        /// <returns>return the DataSet containg Field Data</returns>
        public DataSet GetSAPIRequestFieldData(string requestField)
        {
            DataSet dsSAPI = new DataSet();
            try
            {
                string spName = string.Empty;
                if (requestField.Equals("Snapshot"))
                    spName = "P_GetSAPISnapshotRequestFields";
                else
                    spName = "P_GetSAPISubscriptionRequestFields";
                DatabaseManager.QueryData queryData = new DatabaseManager.QueryData();
                queryData.StoredProcedureName = spName;
                queryData.CommandTimeout = 200;

                dsSAPI = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dsSAPI;
        }

        /// <summary>
        /// Save the Data in DB
        /// </summary>
        /// <param name="saveDataSetTemp">DataSet that need to be stored in DB</param>
        /// <param name="requestField">Parameter to decide the SP</param>
        public void SaveSAPIRequestFieldData(DataSet saveDataSetTemp, string requestField)
        {
            try
            {
                string spName = string.Empty;
                if (requestField.Equals("Snapshot"))
                    spName = "P_SaveSAPISnapshotRequestFields";
                else
                    spName = "P_SaveSAPISubscriptionRequestFields";

                DatabaseManager.QueryData queryData = new DatabaseManager.QueryData();
                queryData.StoredProcedureName = spName;
                queryData.CommandTimeout = 200;
                queryData.DictionaryDatabaseParameter.Add("@xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@xml",
                    ParameterType = DbType.Xml,
                    ParameterValue = saveDataSetTemp.GetXml()
                });

                DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Saves the mark prices in database. if isAutoApproved true then it will set approved in DB
        /// </summary>
        /// <param name="dtMarkPrices">Table having symbol and respective mark prices for the date(s).</param>
        /// <returns>Number of rows affected by saving in database.</returns>
        public int SaveMarkPrices(DataTable dtMarkPrices, bool isAutoApproved)
        {
            AddNonExistingColumns(dtMarkPrices);
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtMarkPrices.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PM_SaveMarkPrices_Updated";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });
                queryData.DictionaryDatabaseParameter.Add("@isAutoApproved", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@isAutoApproved",
                    ParameterType = DbType.Boolean,
                    ParameterValue = isAutoApproved
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
        /// Added by: Bharat raturi, 23 may 2014
        /// purpose: Add the columns that do not exist in the datatable
        /// this is required for the daily valuation mark price save cause that does not pass Source, Pricetype and IsApproved column
        /// for now the values are hard-coded
        /// </summary>
        /// <param name="dtMarkPrices">Mark price datatable </param>
        private static void AddNonExistingColumns(DataTable dtMarkPrices)
        {
            if (!dtMarkPrices.Columns.Contains("IsApproved"))
            {
                dtMarkPrices.Columns.Add("IsApproved", typeof(int));
                {
                    foreach (DataRow dr in dtMarkPrices.Rows)
                    {
                        dr["IsApproved"] = 1;
                    }
                }
            }

            if (!dtMarkPrices.Columns.Contains("Source"))
            {
                dtMarkPrices.Columns.Add("Source", typeof(int));
                {
                    foreach (DataRow dr in dtMarkPrices.Rows)
                    {
                        dr["Source"] = 1;
                    }
                }
            }
        }

        public int SaveBeta(DataTable dtBeta)
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
                queryData.CommandTimeout = 6000;
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

        public int SaveTradingVolume(DataTable dtTradingVol)
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

        public int SaveDelta(DataTable dtDelta)
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

        public int SaveOutStandings(DataTable dtOutStanding)
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
        /// Saves the NAV values respective date.
        /// </summary>
        /// <param name="dtForexRate">Table having all the information regarding forex rate for the to and from currency specified along with their respective date.</param>
        /// <returns>Number of rows affected by saving in database.</returns>
        public int SaveNAVValues(DataTable dtNAVValue)
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

        public int SaveVolatility(DataTable dtVolatility)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtVolatility.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PM_SaveDailyVolatility";
                queryData.CommandTimeout = 6000;
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
        /// Saves the vwap.
        /// </summary>
        /// <param name="dtVWAP">The dt vwap.</param>
        /// <returns></returns>
        public int SaveVWAP(DataTable dtVWAP)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtVWAP.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PM_SaveDailyVWAP";
                queryData.CommandTimeout = 6000;
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

        public int SaveDividendYield(DataTable dtDividendYield)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtDividendYield.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PM_SaveDailyDividendYield";
                queryData.CommandTimeout = 6000;
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

        public DataTable GetNAVValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
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
                _company = DataManagerInternalRepository.KeyValueDataManager.GetCompany();
                _companyID = int.Parse(_company.Rows[0]["CompanyID"].ToString());
                _masterFundSubAccountAssociation = DataManagerInternalRepository.KeyValueDataManager.GetCompanyMasterFundSubAccountAssociation(_companyID);
                _masterFunds = DataManagerInternalRepository.KeyValueDataManager.GetAllMasterFunds();
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                if (ds != null && ds.Tables.Count > 0)
                {
                    var dt = ds.Tables[0];
                    if (!dt.Columns.Contains("MasterFund"))
                        dt.Columns.Add("MasterFund", typeof(System.String));

                    foreach (DataRow row in dt.Rows)
                    {
                        var accountId = Convert.ToInt32(row["FundID"].ToString());
                        row["MasterFund"] = GetMasterFund(accountId);
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
            return FillDataTable(ds);
        }

        private static string GetMasterFund(int accountID)
        {
            try
            {                
                foreach (KeyValuePair<int, List<int>> kvp in _masterFundSubAccountAssociation)
                {
                    if (kvp.Value.Contains(accountID))
                    {
                        if (_masterFunds.ContainsKey(kvp.Key))
                        {
                            return _masterFunds[kvp.Key];
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
            return string.Empty;
        }

        public DataTable GetPerformanceNumberValueDateWise(DateTime fromDate)
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

        public DataTable GetStartOfMonthCapitalAccountValuesDateWise(DateTime fromDate, int dateMethodology)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetStartOfMonthCapitalAccountValuesDateWise";
            queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
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

        public int SaveStartOfMonthCapitalAccountValues(DataTable dtStartOfMonthCapitalAccountValue)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtStartOfMonthCapitalAccountValue.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMSaveStartOfMonthCapitalAccountValue";
                queryData.CommandTimeout = 6000;
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

        public int DeleteStartOfMonthCapitalAccountValues(DateTime deletionDate)
        {
            int rowAffected = 0;
            try
            {
                Object[] parameter = new object[1];
                parameter[0] = deletionDate;

                rowAffected = int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("PMDeleteStartOfMonthCapitalAccountValue", parameter).ToString());
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
            return rowAffected;
        }

        public DataTable GetUserDefinedMTDPnLValuesDateWise(DateTime fromDate, int dateMethodology)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetUserDefinedMTDPnLValuesDateWise";
            queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
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

        public int SaveUserDefinedMTDPnLValues(DataTable dtUserDefinedMTDPnLValue)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtUserDefinedMTDPnLValue.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMSaveUserDefinedMTDPnLValue";
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

        public int DeleteUserDefinedMTDPnLValues(DateTime deletionDate)
        {
            int rowAffected = 0;
            try
            {
                Object[] parameter = new object[1];
                parameter[0] = deletionDate;

                rowAffected = int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("PMDeleteUserDefinedMTDPnLValue", parameter).ToString());
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
            return rowAffected;
        }

        /// <summary>
        /// Returns distinct currencies.
        /// </summary>
        /// <returns>Currencies information regarding their name, symbol and id.</returns>
        public Prana.BusinessObjects.CurrencyCollection GetCurrenciesWithSymbol()
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
            return currency;
        }

        public DataTable GetCollateralInterest(DateTime fromDate, int dateMethodology)
        {
            DataSet ds = null;

            /*Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand commandSP = db.GetStoredProcCommand("PMGetCollateralInterest");
            db.AddInParameter(commandSP, "@FromDate", DbType.DateTime, fromDate.Date);
            db.AddInParameter(commandSP, "@Type", DbType.Int32, dateMethodology);
            XMLSaveManager.AddOutErrorParameters(db, commandSP);*/
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetCollateralInterest";
            queryData.DictionaryDatabaseParameter.Add("@fromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@fromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate.Date
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.String,
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

        public DataTable GetDailyCash(DateTime fromDate, int dateMethodology)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetDailyCashMonthWise";
            queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FromDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate.Date
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
        /// Saves the cash value in database.
        /// </summary>
        /// <param name="dtDailyCashValues">Table having cash value for a given date.</param>
        /// <returns>Number of rows affected by saving in database.</returns>
        public int SaveDailyCashValues(DataTable dtDailyCashValues)
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

        public int SavePerformanceNumberValues(DataTable dtPerformanceNumberValuesTemp)
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

        public int DeleteDailyCashValue(int accountID, int localCurrencyID, int baseCurrencyID, DateTime date)
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

        public DataTable GetConversionRateDateWise(DateTime fromDate, int dateMethodology)
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
                ParameterValue = fromDate
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

            DataTable dataTable = FillDataTable(ds);
            return dataTable;
        }

        /// <summary>
        /// Function to get account wise conversion rate
        /// </summary>
        /// <param name="xmlAccount"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="dateMethodology"></param>
        /// <returns></returns>
        public DataTable GetAccountWiseConversionRate(string xmlAccount, DateTime fromDate, DateTime toDate, int dateMethodology, int filter)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetFundWiseConversionRate";
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
            queryData.DictionaryDatabaseParameter.Add("@fundIDs", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@fundIDs",
                ParameterType = DbType.String,
                ParameterValue = xmlAccount
            });
            queryData.DictionaryDatabaseParameter.Add("@filter", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@filter",
                ParameterType = DbType.Int32,
                ParameterValue = filter
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

        public int SaveForexRate(DataTable dtForexRate)
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
                queryData.CommandTimeout = 600;
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

        public int SaveStandardCurrencyPair(DataTable dtForexRate)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtForexRate.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveStandardCurrencyPair";
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
        /// To save forex conversion rate with account details
        /// </summary>
        /// <param name="dtForexRate"></param>
        /// <returns></returns>
        public int SaveForexRateWithAccount(DataTable dtForexRate)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtForexRate.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMSaveForexRateWithFund";
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
        /// added by: Bharat Raturi, 20 may 2014
        /// Get unapproved mark prices from the database
        /// </summary>
        /// <returns>Datatable holding the mark prices</returns>
        public DataTable GetUnapprovedMarkPricesFromDB(DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable();
            try
            {
                object[] param = { startDate, endDate };
                dt = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetUnApprovedMarkPrices", param).Tables[0];
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

        /// <summary>
        /// Added by: Bharat Raturi, 23 may 2014
        /// Approve the mark prices and save them in DB
        /// </summary>
        /// <param name="xmlMarkPrice">mark price XMl</param>
        /// <returns>number of rows affected</returns>
        public DataSet ApproveMarkPricesinDB(String xmlMarkPrice)
        {
            DataSet newMarkPriceDS = new DataSet();
            try
            {
                object[] param = { xmlMarkPrice };
                newMarkPriceDS = DatabaseManager.DatabaseManager.ExecuteDataSet("P_ApproveNewMarkPrices", param);
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
            return newMarkPriceDS;
        }

        /// <summary>
        /// added by: Bharat Raturi, 23 may 2014
        /// Rescind the mark prices
        /// </summary>
        /// <param name="xmlMarkPrice">Mark rpice XML</param>
        /// <returns>number of rows affected</returns>
        public int RescindMarkPricesinDB(string xmlMarkPrice)
        {
            int i = 0;
            try
            {
                object[] param = { xmlMarkPrice };
                i = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_RescindNewMarkPrices", param);
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
            return i;
        }

        /// <summary>
        /// Modified by sachin mishra purpose -CHMW-2905 Pricing Policy Implementation 
        /// </summary>
        public List<PricePolicyDetails> GetPriceRuleDetailFromDB()
        {
            List<PricePolicyDetails> pricingPolicyList = new List<PricePolicyDetails>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetPricingPolicyDetail";

                using (IDataReader drPricing = DatabaseManager.DatabaseManager.ExecuteReader(queryData, ApplicationConstants.PranaConnectionString))
                {
                    while (drPricing.Read())
                    {
                        PricePolicyDetails objPriceRule = new PricePolicyDetails();

                        if (drPricing.GetValue(0) != DBNull.Value)
                        {
                            objPriceRule.PricingID = drPricing.GetInt32(0);
                        }
                        if (drPricing.GetValue(1) != DBNull.Value)
                        {
                            objPriceRule.IsActive = drPricing.GetBoolean(1);
                        }
                        if (drPricing.GetValue(2) != DBNull.Value)
                        {
                            objPriceRule.PolicyName = drPricing.GetString(2);
                        }
                        if (drPricing.GetValue(3) != DBNull.Value)
                        {
                            objPriceRule.SPName = drPricing.GetString(3);
                        }
                        if (drPricing.GetValue(4) != DBNull.Value)
                        {
                            objPriceRule.IsFileAvailable = drPricing.GetBoolean(4);
                        }
                        if (drPricing.GetValue(5) != DBNull.Value)
                        {
                            objPriceRule.FolderPath = drPricing.GetString(5);
                        }
                        if (drPricing.GetValue(6) != DBNull.Value)
                        {
                            objPriceRule.FilePath = drPricing.GetString(6);
                        }

                        pricingPolicyList.Add(objPriceRule);
                    }
                }
                return pricingPolicyList;
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
        /// Modified by sachin mishra purpose -CHMW-2905 Pricing Policy Implementation 
        /// </summary>
        public List<PricingPolicyDetailsFromSP> GetPricePolicyDetailSPFromDB(int accountID, DateTime dateTime, string spName)
        {
            List<PricingPolicyDetailsFromSP> pricingPolicyDetailList = new List<PricingPolicyDetailsFromSP>();

            try
            {
                string dateTimeFormat = dateTime.ToString();
                string dateDBFormat = Convert.ToDateTime(dateTimeFormat).ToString("yyyy-MM-dd");
                object[] parameters = new object[2];
                parameters[0] = dateDBFormat;
                parameters[1] = accountID;
                using (IDataReader drPricing = DatabaseManager.DatabaseManager.ExecuteReader(spName, parameters))
                {
                    while (drPricing.Read())
                    {
                        PricingPolicyDetailsFromSP objPriceRule = new PricingPolicyDetailsFromSP();

                        if (drPricing.GetValue(0) != DBNull.Value)
                        {
                            objPriceRule.AccountID = drPricing.GetInt32(0);
                        }

                        if (drPricing.GetValue(1) != DBNull.Value)
                        {
                            objPriceRule.Symbol = drPricing.GetString(1);
                        }
                        if (drPricing.GetValue(2) != DBNull.Value)
                        {
                            objPriceRule.PricingField = drPricing.GetString(2);
                        }

                        pricingPolicyDetailList.Add(objPriceRule);
                    }
                }
                return pricingPolicyDetailList;
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

        public DataSet GetPricePolicyDetailSPFromDB(string spName, int accountID, string filePath, string folderPath, DateTime startDate, DateTime endDate)
        {
            DataSet pricingPolicyDetail = new DataSet();

            try
            {
                String fullFilePath = folderPath + filePath;
                string dateTimeFormat = startDate.ToString();
                string dateDBFormat = Convert.ToDateTime(dateTimeFormat).ToString("yyyy-MM-dd");

                string endDateFormat = endDate.ToString();
                string endDateDBFormat = Convert.ToDateTime(endDateFormat).ToString("yyyy-MM-dd");

                object[] parameters = new object[4];
                parameters[0] = fullFilePath;
                parameters[1] = dateDBFormat;
                parameters[2] = endDateDBFormat;
                parameters[3] = accountID;

                pricingPolicyDetail = DatabaseManager.DatabaseManager.ExecuteDataSet(spName, parameters);

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
            return pricingPolicyDetail;
        }

        /// <summary>
        /// Gets the collateral price date wise.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="dateMethodology">The date methodology.</param>
        /// <param name="getSameDayClosedDataOnDV">if set to <c>true</c> [get same day closed data on dv].</param>
        /// <returns></returns>
        public DataTable GetCollateralPriceDateWise(DateTime fromDate, int dateMethodology, bool getSameDayClosedDataOnDV, bool isOnlyFixedIncomeSymbols)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllSymbolsCollateralPrice";
            queryData.DictionaryDatabaseParameter.Add("@Date", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Date",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });
            queryData.DictionaryDatabaseParameter.Add("@Type", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Type",
                ParameterType = DbType.Int32,
                ParameterValue = dateMethodology
            });
            queryData.DictionaryDatabaseParameter.Add("@GetSameDayClosedData", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@GetSameDayClosedData",
                ParameterType = DbType.Boolean,
                ParameterValue = getSameDayClosedDataOnDV
            });
            queryData.DictionaryDatabaseParameter.Add("@IsOnlyFixedIncomeSymbols", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@IsOnlyFixedIncomeSymbols",
                ParameterType = DbType.Boolean,
                ParameterValue = isOnlyFixedIncomeSymbols
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
        /// Saves the collateral values.
        /// </summary>
        /// <param name="dtCollateralPriceValue">The dt collateral price value.</param>
        /// <returns></returns>
        public int SaveCollateralValues(DataTable dtCollateralPriceValue)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtCollateralPriceValue.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PM_SaveDailyCollateralPrice";
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
    }
}