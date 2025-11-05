using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PositionServices
{
    public class GenericPositionDataManager
    {
        public static void GetData(ref GenericPositionData genericPositionData)
        {
            try
            {
                DataSet ds = GetOpenPositionsAndTransactionsFromDB(genericPositionData);
                if (ds != null)
                {
                    if (ds.Tables["Table"] != null)
                        genericPositionData.Positions = PranaPositionDataManager.GetTaxlotsFromDataTable(ds.Tables[0]);

                    if (ds.Tables["Table1"] != null)
                        genericPositionData.Transactions = PranaPositionDataManager.GetTaxlotsFromDataTable(ds.Tables[1]);

                    if (ds.Tables["Table2"] != null)
                        genericPositionData.UnallocatedTrades = PranaPositionDataManager.GetTaxlotsFromDataTable(ds.Tables[2]);

                    if (ds.Tables["Table3"] != null)
                    {
                        genericPositionData.GenericDayEndDataValue.StartOfDayAccountWiseCash = GetAccountWiseValuesInBaseCurrencyFromDataTable(ds.Tables[3]);
                    }

                    //if (ds.Tables["Table4"] != null)
                    //{
                    //    genericPositionData.GenericDayEndDataValue.DayAccountWiseCash = GetAccountWiseValuesInBaseCurrencyFromDataTable(ds.Tables[4]);
                    //}

                    if (genericPositionData.IsBetaNeeded && ds.Tables["Table5"] != null)
                    {
                        genericPositionData.SymbolWiseBeta = GetSymbolWiseBetaFromDataTable(ds.Tables[5]);

                        if (genericPositionData.IsAccrualsNeeded)
                        {
                            if (ds.Tables["Table6"] != null)
                            {
                                genericPositionData.GenericDayEndDataValue.StartOfDayAccountWiseAccruals = GetAccountWiseAccrualsInBaseCurrencyFromDataTable(ds.Tables[6]);
                            }

                            //if (ds.Tables["Table7"] != null)
                            //{
                            //    genericPositionData.GenericDayEndDataValue.DayAccountWiseAccruals = GetAccountWiseValuesInBaseCurrencyFromDataTable(ds.Tables[7]);
                            //}
                        }
                    }
                    else
                    {
                        if (genericPositionData.IsAccrualsNeeded)
                        {
                            if (ds.Tables["Table5"] != null)
                            {
                                genericPositionData.GenericDayEndDataValue.StartOfDayAccountWiseAccruals = GetAccountWiseAccrualsInBaseCurrencyFromDataTable(ds.Tables[5]);
                            }

                            //if (ds.Tables["Table6"] != null)
                            //{
                            //    genericPositionData.GenericDayEndDataValue.DayAccountWiseAccruals = GetAccountWiseValuesInBaseCurrencyFromDataTable(ds.Tables[6]);
                            //}
                        }
                    }

                    if (genericPositionData.IsFxRateNeeded)
                    {
                        genericPositionData.CurrentOffsetAdjustedAUECDates = TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates;
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
        }

        private static Dictionary<string, double> GetSymbolWiseBetaFromDataTable(DataTable dataTable)
        {
            Dictionary<string, double> symbolWiseBeta = new Dictionary<string, double>();
            try
            {
                if (dataTable != null)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (!symbolWiseBeta.ContainsKey(row["Symbol"].ToString()))
                        {
                            symbolWiseBeta.Add(row["Symbol"].ToString(), Double.Parse(row["Beta"].ToString()));
                        }
                    }
                    return symbolWiseBeta;
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
            return symbolWiseBeta;
        }

        private static DataSet GetOpenPositionsAndTransactionsFromDB(GenericPositionData genericPositionData)
        {
            DataSet dsOpenPositionsAndTransactions = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetFundOpenPositionsAndTransactions";
                queryData.CommandTimeout = 200;

                string ForPositionsAllAUECDatesString = string.Empty;
                string ForTransactionsAllAUECDatesString = string.Empty;
                if (genericPositionData.IsTransactionsIncludedInPositions)
                {
                    if (genericPositionData.IsSameDateInAllAUEC)
                    {
                        ForTransactionsAllAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(genericPositionData.GivenDate);
                    }
                    else
                    {
                        ForTransactionsAllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(genericPositionData.GivenDate);
                    }
                }
                else
                {
                    if (genericPositionData.IsSameDateInAllAUEC)
                    {
                        ForPositionsAllAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(genericPositionData.GivenDate.AddDays(-1));
                        ForTransactionsAllAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(genericPositionData.GivenDate);
                    }
                    else
                    {
                        ForPositionsAllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(genericPositionData.GivenDate.AddDays(-1));
                        ForTransactionsAllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(genericPositionData.GivenDate);
                    }
                }

                queryData.DictionaryDatabaseParameter.Add("@ForPositionsAllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ForPositionsAllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = ForPositionsAllAUECDatesString
                });
                queryData.DictionaryDatabaseParameter.Add("@ForTransactionsAllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ForTransactionsAllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = ForTransactionsAllAUECDatesString
                });
                queryData.DictionaryDatabaseParameter.Add("@AssetIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AssetIds",
                    ParameterType = DbType.String,
                    ParameterValue = genericPositionData.CommaSapratedAssetIDs
                });
                queryData.DictionaryDatabaseParameter.Add("@FundIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundIds",
                    ParameterType = DbType.String,
                    ParameterValue = genericPositionData.CommaSapratedAccountIds
                });
                queryData.DictionaryDatabaseParameter.Add("@Symbols", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Symbols",
                    ParameterType = DbType.String,
                    ParameterValue = genericPositionData.Symbols
                });
                queryData.DictionaryDatabaseParameter.Add("@CustomConditions", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CustomConditions",
                    ParameterType = DbType.String,
                    ParameterValue = genericPositionData.CustomConditions
                });
                queryData.DictionaryDatabaseParameter.Add("@IsPositionsIncludeTransactions", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@IsPositionsIncludeTransactions",
                    ParameterType = DbType.Boolean,
                    ParameterValue = genericPositionData.IsTransactionsIncludedInPositions
                });
                queryData.DictionaryDatabaseParameter.Add("@IsAccrualsNeeded", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@IsAccrualsNeeded",
                    ParameterType = DbType.Boolean,
                    ParameterValue = genericPositionData.IsAccrualsNeeded
                });

                DateTime mostLeadingDateTime = TimeZoneHelper.GetInstance().MostLeadingAUECDateTime(false);
                queryData.DictionaryDatabaseParameter.Add("@DateForCashValues", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@DateForCashValues",
                    ParameterType = DbType.DateTime,
                    ParameterValue = mostLeadingDateTime
                });
                queryData.DictionaryDatabaseParameter.Add("@IsBetaNeeded", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@IsBetaNeeded",
                    ParameterType = DbType.Boolean,
                    ParameterValue = genericPositionData.IsBetaNeeded
                });
                queryData.DictionaryDatabaseParameter.Add("@DateTimeStringForBetaPositions", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@DateTimeStringForBetaPositions",
                    ParameterType = DbType.String,
                    ParameterValue = TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedCurrentDateTimeString()
                });

                dsOpenPositionsAndTransactions = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return dsOpenPositionsAndTransactions;
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

        private static Dictionary<int, double> GetAccountWiseValuesInBaseCurrencyFromDataTable(DataTable dt)
        {
            Dictionary<int, double> accountWiseInitialCashInBaseCurrency = new Dictionary<int, double>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!accountWiseInitialCashInBaseCurrency.ContainsKey(Int32.Parse(row["FundID"].ToString())))
                            accountWiseInitialCashInBaseCurrency.Add(Int32.Parse(row["FundID"].ToString()), double.Parse(row["Cash"].ToString()));
                    }
                    return accountWiseInitialCashInBaseCurrency;
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
            return accountWiseInitialCashInBaseCurrency;
        }

        private static Dictionary<int, Dictionary<int, double>> GetAccountWiseAccrualsInBaseCurrencyFromDataTable(DataTable dt)
        {
            Dictionary<int, Dictionary<int, double>> accountWiseInitialCashInBaseCurrency = new Dictionary<int, Dictionary<int, double>>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int fundid = Int32.Parse(row["FundID"].ToString());
                        int currencyid = Int32.Parse(row["CurrencyID"].ToString());
                        if (!accountWiseInitialCashInBaseCurrency.ContainsKey(fundid))
                        {
                            Dictionary<int, double> currencyCash = new Dictionary<int, double>();
                            currencyCash.Add(currencyid, double.Parse(row["Cash"].ToString()));
                            accountWiseInitialCashInBaseCurrency.Add(fundid, currencyCash);
                        }
                        else if (!accountWiseInitialCashInBaseCurrency[fundid].ContainsKey(currencyid))
                        {
                            accountWiseInitialCashInBaseCurrency[fundid].Add(currencyid, double.Parse(row["Cash"].ToString()));
                        }
                    }
                    return accountWiseInitialCashInBaseCurrency;
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
            return accountWiseInitialCashInBaseCurrency;
        }

        /// <summary>
        /// Bharat (31 December 2013)
        /// </summary>
        /// <param name="riskTransactions">Transaction Taxlots with Account-Symbol grouping</param>
        /// <returns>Dictionary of AccountWise Cash Impact</returns>
        public static Dictionary<int, double> CalculateAccountWiseCashImpact(List<TaxLot> riskTransactions)
        {
            Dictionary<int, double> accountWiseCashImpact = new Dictionary<int, double>();
            try
            {
                if (riskTransactions != null && riskTransactions.Count > 0)
                {
                    foreach (TaxLot taxlot in riskTransactions)
                    {
                        //-1 for cash impact ---- in case of buy, cash decreases and in case of sell, cash increases
                        if (!accountWiseCashImpact.ContainsKey(taxlot.Level1ID))
                        {
                            accountWiseCashImpact.Add(taxlot.Level1ID, -1 * BusinessLogic.Calculations.GetNotional(taxlot.Quantity, taxlot.AvgPrice, taxlot.ContractMultiplier, taxlot.SideMultiplier));
                        }
                        else
                        {
                            accountWiseCashImpact[taxlot.Level1ID] += -1 * BusinessLogic.Calculations.GetNotional(taxlot.Quantity, taxlot.AvgPrice, taxlot.ContractMultiplier, taxlot.SideMultiplier);
                        }
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
            return accountWiseCashImpact;
        }

        /// <summary>
        /// Bharat (31 December 2013)
        /// </summary>
        /// <param name="accountWiseCash">AccountWise StartOfDayCash</param>
        /// <param name="accountWiseCashImpact">AccountWise Cash Impact</param>
        /// <returns>Merged Dictionary of AccountWise Cash and AccountWise Cash Impact</returns>
        public static Dictionary<int, double> AddCashAndCashImpact(Dictionary<int, double> accountWiseCash, Dictionary<int, double> accountWiseCashImpact)
        {
            Dictionary<int, double> finalAccountWiseCash = new Dictionary<int, double>();
            try
            {
                if (accountWiseCash != null && accountWiseCash.Count > 0 && accountWiseCashImpact != null && accountWiseCashImpact.Count > 0)
                {
                    finalAccountWiseCash = accountWiseCash;
                    foreach (KeyValuePair<int, double> kvp in accountWiseCashImpact)
                    {
                        if (finalAccountWiseCash.ContainsKey(kvp.Key))
                        {
                            finalAccountWiseCash[kvp.Key] += kvp.Value;
                        }
                        else
                        {
                            finalAccountWiseCash.Add(kvp.Key, kvp.Value);
                        }
                    }
                }
                else if (accountWiseCash != null && accountWiseCash.Count > 0)
                {
                    return accountWiseCash;
                }
                else if (accountWiseCashImpact != null && accountWiseCashImpact.Count > 0)
                {
                    return accountWiseCashImpact;
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
            return finalAccountWiseCash;
        }
    }
}