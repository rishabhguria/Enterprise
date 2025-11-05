using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.XMLUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Prana.OptionCalculator.Common
{
    public static class OptionModelUserInputCache
    {
        static ProxyBase<IPublishing> _proxyPublishing = null;
        private static object _publishLock = new object();
        private static void CreatePublishingProxy()
        {
            try
            {
                _proxyPublishing = new ProxyBase<IPublishing>("PricingPublishingEndpointAddress");
            }
            catch //(Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        static ProxyBase<ISecMasterSyncServices> _secMasterServices = null;
        public static ProxyBase<ISecMasterSyncServices> SecMasterServices
        {
            set { _secMasterServices = value; }
        }

        private static Dictionary<string, UserOptModelInput> _dictOMIData;

        /// <summary>
        /// Divya:02042013:  Key > proxy Symbol, Value > List of Symbol info for which this proxy Symbol is used.
        /// Symbol Info contains the information of symbol as well as its underlying. 
        /// </summary>
        private static Dictionary<string, List<SymbolInfo>> _dictProxyWiseSymbols;

        private static object _locker = new object();
        private static object _dbLocker = new object();
        private const string const_BloombergSymbolExCode = "BloombergSymbolWithExchangeCode";

        public static object Clone()
        {
            try
            {
                lock (_locker)
                {
                    return DeepCopyHelper.Clone(_dictOMIData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not clone", ex);
            }
        }

        static OptionModelUserInputCache()
        {
            _dictOMIData = new Dictionary<string, UserOptModelInput>();
            _dictProxyWiseSymbols = new Dictionary<string, List<SymbolInfo>>();
            FillOMIDictionaryFromDB(true);
            GetOMIPreferencesFromDB();
            CreatePublishingProxy();
        }

        public static UserOptModelInput GetUserOMIData(string symbol)
        {
            lock (_locker)
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
        }

        private static void GetOMIPreferencesFromDB()
        {
            try
            {
                LiveFeedPreferences Prefereces = OptionModelDataManager.GetOMIPrefDataFromDB();
                FeedPriceChooser.SetLiveFeedPreferences(Prefereces);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static LiveFeedPreferences GetLiveFeedPreferences()
        {
            LiveFeedPreferences preferences = new LiveFeedPreferences();
            try
            {
                preferences.SelectedFeedPrice = FeedPriceChooser.SelectedFeedPrice;
                preferences.OptionSelectedFeedPrice = FeedPriceChooser.OptionSelectedFeedPrice;
                preferences.OverrideWithOthers = FeedPriceChooser.OverrideWithOthers;
                preferences.OverrideWithOptions = FeedPriceChooser.OverrideWithOptions;
                //preferences.LastIfMidZero = FeedPriceChooser.LastIfMidZero;
                //preferences.LastIfMidZeroForOptions = FeedPriceChooser.LastIfMidZeroForOptions;
                //preferences.LastIfAskBidOrMidZeroForOptions = FeedPriceChooser.LastIfAskBidOrMidZeroForOptions;
                //preferences.LastIfAskBidOrMidZero = FeedPriceChooser.LastIfAskBidOrMidZero;
                preferences.UseClosingMark = FeedPriceChooser.UseClosingMark;
                preferences.UseDefaultDelta = FeedPriceChooser.UseDefaultDelta;
                preferences.OverrideConditionOptions = FeedPriceChooser.OverrideConditionOptions;
                preferences.OverrideConditionOthers = FeedPriceChooser.OverrideConditionOthers;
                preferences.PriceBarOptions = FeedPriceChooser.PriceBarOptions;
                preferences.PriceBarOthers = FeedPriceChooser.PriceBarOthers;
                preferences.OverrideCheckOptions = FeedPriceChooser.OverrideCheckOptions;
                preferences.OverrideCheckOthers = FeedPriceChooser.OverrideCheckOthers;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return preferences;
        }

        public static void UpdateLiveFeedPreferences(LiveFeedPreferences Preferences)
        {
            try
            {
                FeedPriceChooser.SetLiveFeedPreferences(Preferences);
                PublishData(OMIPublishType.OMIPreferences);
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

        internal static bool IsExistDatainCache(string symbol)
        {
            try
            {
                lock (_locker)
                {
                    if (_dictOMIData.ContainsKey(symbol))
                        return true;
                    else
                        return false;
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
            return false;
        }

        internal static List<SymbolInfo> GetSymbolsForProxy(string ProxySymbol)
        {
            try
            {
                lock (_locker)
                {
                    if (_dictProxyWiseSymbols.ContainsKey(ProxySymbol))
                    {
                        return _dictProxyWiseSymbols[ProxySymbol];
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
            return null;
        }

        // This is called on Refreshing PI.
        public static List<UserOptModelInput> GetOMICollection(bool fetchZeroPositionData, string symmbols)
        {
            try
            {
                FillOMIDictionaryFromDB(fetchZeroPositionData, symmbols);
                return new List<UserOptModelInput>(_dictOMIData.Values);
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

        // This is called on starting Pricing Server/ Refreshing PI.
        private static void FillOMIDictionaryFromDB(bool fetchZeroPositionData, string symbols = "")
        {
            try
            {
                // This method is being used to save the Dirty data if any.
                // In any case, If dirty data exists in the cache, we are saving that before clearing the cache.
                SaveOMIDataToDB();
                DataSet dtOMIData = GetOMIdataFromDB(symbols, fetchZeroPositionData);
                UpdatePIDataWithCustomSymbols(dtOMIData);
                lock (_locker)
                {
                    UpdateCacheFromDataSet(dtOMIData);
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

        private static void UpdatePIDataWithCustomSymbols(DataSet dtOMIData)
        {
            try
            {
                DataSet customPISymbols = new DataSet();
                Dictionary<string, SecMasterBaseObj> dictCustomSecuritiesData = new Dictionary<string, SecMasterBaseObj>();
                if (_secMasterServices == null || _secMasterServices.InnerChannel == null)
                {
                    CreateSecMasterServicesProxy();
                }
                customPISymbols = OptionModelDataManager.GetPIDataForCustomSymbols();
                if (_secMasterServices != null && _secMasterServices.InnerChannel != null)
                {
                    try
                    {
                        dictCustomSecuritiesData = _secMasterServices.InnerChannel.GetSecMasterSymbolData(GetCustomPISymbols(customPISymbols), Prana.Global.ApplicationConstants.SymbologyCodes.TickerSymbol);
                    }
                    catch
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("TradeService not connected", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }
                }
                FillSecMasterDetailsToCustomPIData(customPISymbols, dictCustomSecuritiesData, dtOMIData);
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

        private static List<string> GetCustomPISymbols(DataSet customPISymbols)
        {
            List<string> customSymbols = new List<string>();
            try
            {
                if (customPISymbols != null && customPISymbols.Tables.Count > 0 && customPISymbols.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = customPISymbols.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!customSymbols.Contains(row[0].ToString()))
                        {
                            customSymbols.Add(row[0].ToString());
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
            return customSymbols;
        }

        private static void FillSecMasterDetailsToCustomPIData(DataSet customPISymbols, Dictionary<string, SecMasterBaseObj> dictCustomSecuritiesData, DataSet dtOMIData)
        {
            try
            {
                if (customPISymbols != null && customPISymbols.Tables.Count > 0 && customPISymbols.Tables[0].Rows.Count > 0 && dictCustomSecuritiesData.Count > 0)
                {
                    DataTable dt = customPISymbols.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        string symbol = row["Symbol"].ToString();
                        row["OSISymbol"] = dictCustomSecuritiesData[symbol].OSIOptionSymbol;
                        row["IDCOSymbol"] = dictCustomSecuritiesData[symbol].IDCOOptionSymbol;
                        row["UnderLyingSymbol"] = dictCustomSecuritiesData[symbol].UnderLyingSymbol;
                        row["SecurityDescription"] = dictCustomSecuritiesData[symbol].LongName;
                        row["ProxySymbol"] = dictCustomSecuritiesData[symbol].ProxySymbol;
                        row["BloombergSymbol"] = dictCustomSecuritiesData[symbol].BloombergSymbol;
                        row["AuecID"] = dictCustomSecuritiesData[symbol].AUECID;
                        row["AssetID"] = Convert.ToInt32(dictCustomSecuritiesData[symbol].AssetID.ToString());
                        row["VsCurrencyID"] = 0;
                        row["LeadCurrencyID"] = 0;
                        row["IsHistorical"] = false;
                        row["ExpirationDate"] = DateTime.MinValue;
                        row["StrikePrice"] = 0.0;
                        row[const_BloombergSymbolExCode] = dictCustomSecuritiesData[symbol].BloombergSymbolWithExchangeCode;
                    }
                    DataTable dt1 = CloneDataTable(dt);
                    foreach (DataRow row1 in dt1.Rows)
                    {
                        if (dtOMIData != null && dtOMIData.Tables.Count > 0)
                        {
                            dtOMIData.Tables[0].ImportRow(row1);
                            dtOMIData.Tables[0].AcceptChanges();
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
        }

        private static DataTable CloneDataTable(DataTable dt)
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = dt.Clone();
                dataTable.Columns["AssetID"].DataType = typeof(Int32);
                dataTable.Columns["ExpirationDate"].DataType = typeof(DateTime);
                dataTable.Columns["StrikePrice"].DataType = typeof(double);
                dataTable.Columns["VsCurrencyID"].DataType = typeof(Int32);
                dataTable.Columns["LeadCurrencyID"].DataType = typeof(Int32);
                dataTable.Columns["IsHistorical"].DataType = typeof(Boolean);
                dataTable.Columns["ManualInput"].DataType = typeof(Boolean);
                dataTable.Columns["AuecID"].DataType = typeof(Int32);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow newRow = dataTable.NewRow();
                        newRow["Symbol"] = row["Symbol"];
                        newRow["SecurityDescription"] = row["SecurityDescription"];
                        newRow["HistoricalVolatility"] = row["HistoricalVolatility"];
                        newRow["HistoricalVolatilityUsed"] = row["HistoricalVolatilityUsed"];
                        newRow["UserVolatility"] = row["UserVolatility"];
                        newRow["UserVolatilityUsed"] = row["UserVolatilityUsed"];
                        newRow["UserInterestRate"] = row["UserInterestRate"];
                        newRow["UserInterestRateUsed"] = row["UserInterestRateUsed"];
                        newRow["UserDividend"] = row["UserDividend"];
                        newRow["UserDividendUsed"] = row["UserDividendUsed"];
                        newRow["UserStockBorrowCost"] = row["UserStockBorrowCost"];
                        newRow["UserStockBorrowCostUsed"] = row["UserStockBorrowCostUsed"];
                        newRow["UserDelta"] = row["UserDelta"];
                        newRow["UserDeltaUsed"] = row["UserDeltaUsed"];
                        newRow["UserLastPrice"] = row["UserLastPrice"];
                        newRow["UserLastPriceUsed"] = row["UserLastPriceUsed"];
                        newRow["UserForwardPoints"] = row["UserForwardPoints"];
                        newRow["UserForwardPointsUsed"] = row["UserForwardPointsUsed"];
                        newRow["UserTheoreticalPriceUsed"] = row["UserTheoreticalPriceUsed"];
                        newRow["UserProxySymbolUsed"] = row["UserProxySymbolUsed"];
                        newRow["UserSharesOutstandingUsed"] = row["UserSharesOutstandingUsed"];
                        newRow["UserSharesOutstanding"] = row["UserSharesOutstanding"];
                        newRow["UserClosingMarkUsed"] = row["UserClosingMarkUsed"];
                        newRow["SMUserSharesOutstanding"] = row["SMUserSharesOutstanding"];
                        newRow["SMUserSharesOutstandingUsed"] = row["SMUserSharesOutstandingUsed"];
                        newRow["ManualInput"] = row["ManualInput"];
                        newRow["OSISymbol"] = row["OSISymbol"];
                        newRow["IDCOSymbol"] = row["IDCOSymbol"];
                        newRow["PSSymbol"] = row["PSSymbol"];
                        newRow["AssetID"] = Convert.ToInt32(row["AssetID"].ToString());
                        newRow["UnderLyingSymbol"] = row["UnderLyingSymbol"];
                        newRow["ExpirationDate"] = Convert.ToDateTime(row["ExpirationDate"].ToString());
                        newRow["StrikePrice"] = row["StrikePrice"];
                        newRow["PutorCall"] = row["PutorCall"];
                        newRow["VsCurrencyID"] = Convert.ToInt32(row["VsCurrencyID"].ToString());
                        newRow["LeadCurrencyID"] = Convert.ToInt32(row["LeadCurrencyID"].ToString());
                        newRow["ProxySymbol"] = row["ProxySymbol"];
                        newRow["BloombergSymbol"] = row["BloombergSymbol"];
                        newRow["IsHistorical"] = Convert.ToBoolean(row["IsHistorical"]);
                        newRow["AuecID"] = Convert.ToInt32(row["AuecID"].ToString());
                        newRow[const_BloombergSymbolExCode] = row[const_BloombergSymbolExCode];
                        dataTable.Rows.Add(newRow);
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
            return dataTable;
        }

        public static void SaveOMIDataToDB()
        {
            try
            {
                lock (_locker)
                {
                    List<UserOptModelInput> listToSave = new List<UserOptModelInput>();
                    foreach (UserOptModelInput userOMI in _dictOMIData.Values)
                    {
                        if (userOMI.IsDirtyToSave)
                        {
                            listToSave.Add(userOMI);
                        }
                    }
                    string Xml = XMLUtilities.SerializeToXML(listToSave);
                    OptionModelDataManager.SaveOptionModelUserDataToDB(Xml);
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

        private static DataSet GetOMIdataFromDB(string Symbols, bool fetchZeroPositionData)
        {
            DataSet returnDataSet = new DataSet();
            try
            {
                lock (_dbLocker)
                {
                    returnDataSet = OptionModelDataManager.GetOptionModelUserDataFromDB(Symbols, fetchZeroPositionData);
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
            return returnDataSet;
        }

        #region OMI Cache Update
        // This is called whenever we change any value from PI and hitting save.
        public static void UpdateOMICache(DataTable dataTableOMIData, ref List<ProxyDataEventArgs> listProxyData)
        {
            try
            {
                lock (_locker)
                {
                    UserOptModelInput userOMI = null;
                    foreach (DataRow dr in dataTableOMIData.Rows)
                    {
                        string symbol = dr["Symbol"].ToString().ToUpper();

                        if (_dictOMIData.ContainsKey(symbol))
                        {
                            userOMI = _dictOMIData[symbol];
                        }
                        else
                        {
                            userOMI = new UserOptModelInput();
                        }
                        userOMI.UnderlyingSymbol = dr["UnderlyingSymbol"].ToString().ToUpper();
                        userOMI.Symbol = symbol;
                        userOMI.Bloomberg = dr["Bloomberg"].ToString().ToUpper();
                        userOMI.BloombergSymbolWithExchangeCode = dr[const_BloombergSymbolExCode].ToString().ToUpper();
                        userOMI.OSIOptionSymbol = dr["OSIOptionSymbol"].ToString().ToUpper();
                        userOMI.IDCOOptionSymbol = dr["IDCOOptionSymbol"].ToString().ToUpper();
                        userOMI.SecurityDescription = dr["SecurityDescription"].ToString();
                        userOMI.HistoricalVol = Convert.ToDouble(dr["HistoricalVol"].ToString()) / 100;
                        userOMI.HistoricalVolUsed = Convert.ToBoolean(dr["HistoricalVolUsed"].ToString());
                        if (!(dr["Volatility"].Equals(System.DBNull.Value)))
                        {
                            userOMI.Volatility = Convert.ToDouble(dr["Volatility"].ToString()) / 100;
                        }
                        userOMI.VolatilityUsed = Convert.ToBoolean(dr["VolatilityUsed"].ToString());
                        if (!(dr["IntRate"].Equals(System.DBNull.Value)))
                        {
                            userOMI.IntRate = Convert.ToDouble(dr["IntRate"].ToString()) / 100;
                        }
                        userOMI.IntRateUsed = Convert.ToBoolean(dr["IntRateUsed"].ToString());

                        /// Dividend yield converted into absolute terms. Have to multiply for 100 while displaying on the UI.
                        /// OMI data picked up from UI
                        if (!(dr["Dividend"].Equals(System.DBNull.Value)))
                        {
                            userOMI.Dividend = Convert.ToDouble(dr["Dividend"].ToString()) / 100;
                        }

                        userOMI.DividendUsed = Convert.ToBoolean(dr["DividendUsed"].ToString());
                        if (!(dr["StockBorrowCost"].Equals(System.DBNull.Value)))
                        {
                            userOMI.StockBorrowCost = Convert.ToDouble(dr["StockBorrowCost"].ToString()) / 100;
                        }

                        userOMI.StockBorrowCostUsed = Convert.ToBoolean(dr["StockBorrowCostUsed"].ToString());
                        if (!(dr["Delta"].Equals(System.DBNull.Value)))
                        {
                            userOMI.Delta = Convert.ToDouble(dr["Delta"].ToString());
                        }
                        userOMI.DeltaUsed = Convert.ToBoolean(dr["DeltaUsed"].ToString());
                        if (!(dr["LastPrice"].Equals(System.DBNull.Value)))
                        {
                            userOMI.LastPrice = Convert.ToDouble(dr["LastPrice"].ToString());
                        }
                        if (!(dr["ForwardPoints"].Equals(System.DBNull.Value)))
                        {
                            userOMI.ForwardPoints = Convert.ToDouble(dr["ForwardPoints"].ToString());
                        }

                        userOMI.ForwardPointsUsed = Convert.ToBoolean(dr["ForwardPointsUsed"].ToString());

                        userOMI.LastPriceUsed = Convert.ToBoolean(dr["LastPriceUsed"].ToString());
                        userOMI.AssetID = Convert.ToInt32(dr["AssetID"].ToString());
                        if (!(dr["TheoreticalPriceUsed"].Equals(System.DBNull.Value)))
                        {
                            userOMI.TheoreticalPriceUsed = Convert.ToBoolean(dr["TheoreticalPriceUsed"].ToString());
                        }
                        else
                        {
                            userOMI.TheoreticalPriceUsed = false;
                        }

                        if (!(dr["SharesOutstanding"].Equals(System.DBNull.Value)))
                        {
                            userOMI.SharesOutstanding = Convert.ToDouble(dr["SharesOutstanding"].ToString());
                        }
                        userOMI.SharesOutstandingUsed = Convert.ToBoolean(dr["SharesOutstandingUsed"].ToString());

                        if (!(dr["SMSharesOutstanding"].Equals(System.DBNull.Value)))
                        {
                            userOMI.SMSharesOutstanding = Convert.ToDouble(dr["SMSharesOutstanding"].ToString());
                        }
                        userOMI.SMSharesOutstandingUsed = Convert.ToBoolean(dr["SMSharesOutstandingUsed"].ToString());

                        if (!(dr["ClosingMarkUsed"].Equals(System.DBNull.Value)))
                        {
                            userOMI.ClosingMarkUsed = Convert.ToBoolean(dr["ClosingMarkUsed"].ToString());
                        }
                        else
                        {
                            userOMI.ClosingMarkUsed = false;
                        }

                        if (!(dr["ManualInput"].Equals(System.DBNull.Value)))
                        {
                            userOMI.ManualInput = Convert.ToBoolean(dr["ManualInput"].ToString());
                        }
                        else
                        {
                            userOMI.ManualInput = false;
                        }

                        if (!(dr["ProxySymbol"].Equals(System.DBNull.Value)))
                        {
                            userOMI.ProxySymbol = dr["ProxySymbol"].ToString();
                        }


                        if (!(dr["ProxySymbolUsed"].Equals(System.DBNull.Value)))
                        {
                            //lock is specifically released here as it was causing deadlock..
                            if (userOMI.ProxySymbolUsed != Boolean.Parse(dr["ProxySymbolUsed"].ToString()))
                            {
                                userOMI.ProxySymbolUsed = Convert.ToBoolean(dr["ProxySymbolUsed"].ToString());
                                if (listProxyData != null)
                                {
                                    ProxyDataEventArgs arg = new ProxyDataEventArgs();
                                    arg.Symbol = userOMI.Symbol;
                                    arg.ProxySymbol = userOMI.ProxySymbol;
                                    arg.UseProxySymbol = userOMI.ProxySymbolUsed;
                                    if (userOMI.ProxySymbol != string.Empty)
                                    {
                                        listProxyData.Add(arg);
                                    }
                                }
                            }

                            if (userOMI.ProxySymbol != null && !userOMI.ProxySymbol.Equals(string.Empty))
                            {
                                List<SymbolInfo> listSymbolinfo = new List<SymbolInfo>();
                                SymbolInfo symInfo = new SymbolInfo();
                                if (_dictProxyWiseSymbols.ContainsKey(userOMI.ProxySymbol))
                                {
                                    if (userOMI.ProxySymbolUsed)
                                    {
                                        symInfo.Symbol = userOMI.Symbol;
                                        symInfo.UnderlyingSymbol = userOMI.UnderlyingSymbol;
                                        _dictProxyWiseSymbols[userOMI.ProxySymbol].Add(symInfo);
                                    }
                                    else
                                    {
                                        foreach (SymbolInfo info in _dictProxyWiseSymbols[userOMI.ProxySymbol])
                                        {
                                            if (info.Symbol == userOMI.Symbol)
                                            {
                                                listSymbolinfo.Add(info);
                                            }
                                        }
                                        foreach (SymbolInfo inf in listSymbolinfo)
                                        {
                                            _dictProxyWiseSymbols[userOMI.ProxySymbol].Remove(inf);
                                        }
                                    }
                                }
                                else
                                {
                                    if (userOMI.ProxySymbolUsed)
                                    {
                                        symInfo.Symbol = userOMI.Symbol;
                                        symInfo.UnderlyingSymbol = userOMI.UnderlyingSymbol;
                                        listSymbolinfo.Add(symInfo);
                                        _dictProxyWiseSymbols.Add(userOMI.ProxySymbol, listSymbolinfo);
                                    }
                                }
                            }
                            else
                            {
                                userOMI.ProxySymbolUsed = false;
                            }

                            //if (userOMI.LastPriceUsed || userOMI.DeltaUsed || userOMI.DividendUsed || userOMI.IntRateUsed || userOMI.VolatilityUsed || userOMI.HistoricalVolUsed)
                            //{
                            //    _dictOMIData.Add(symbol, userOMI);
                            //}

                            if (_dictOMIData.ContainsKey(symbol))
                            {
                                _dictOMIData[symbol] = userOMI;
                            }
                            else
                            {
                                _dictOMIData.Add(symbol, userOMI);
                            }
                        }
                    }
                    PublishData(OMIPublishType.OMIData);
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

        public static void GetCustomDataInformation()
        {
            SaveOMIDataToDB();
            DataSet dtOMIData = GetOMIdataFromDB(string.Empty, true);
            UpdatePIDataWithCustomSymbols(dtOMIData);
        }

        // This is called on Publish. From Trading Data and changes to security master from Symbol Lookup
        public static void UpdateCacheFromOMICollection(List<UserOptModelInput> OMIImportCollection)
        {
            try
            {
                lock (_locker)
                {
                    foreach (UserOptModelInput userOMI in OMIImportCollection)
                    {
                        UserOptModelInput originalUserOMI = new UserOptModelInput();
                        if (_dictOMIData.ContainsKey(userOMI.Symbol))
                        {
                            originalUserOMI = _dictOMIData[userOMI.Symbol];
                            if (userOMI.DeltaUsed)
                            {
                                originalUserOMI.Delta = userOMI.Delta;
                                originalUserOMI.DeltaUsed = userOMI.DeltaUsed;
                            }
                            if (userOMI.DividendUsed)
                            {
                                originalUserOMI.Dividend = userOMI.Dividend / 100;
                                originalUserOMI.DividendUsed = userOMI.DividendUsed;
                            }
                            if (userOMI.StockBorrowCostUsed)
                            {
                                originalUserOMI.StockBorrowCost = userOMI.StockBorrowCost / 100;
                                originalUserOMI.StockBorrowCostUsed = userOMI.StockBorrowCostUsed;
                            }
                            if (userOMI.ForwardPointsUsed)
                            {
                                originalUserOMI.ForwardPoints = userOMI.ForwardPoints;
                                originalUserOMI.ForwardPointsUsed = userOMI.ForwardPointsUsed;
                            }
                            if (userOMI.HistoricalVolUsed)
                            {
                                originalUserOMI.HistoricalVol = userOMI.HistoricalVol / 100;
                                originalUserOMI.HistoricalVolUsed = userOMI.HistoricalVolUsed;
                            }

                            if (userOMI.VolatilityUsed)
                            {
                                originalUserOMI.Volatility = userOMI.Volatility / 100;
                                originalUserOMI.VolatilityUsed = userOMI.HistoricalVolUsed;
                            }
                            if (userOMI.IntRateUsed)
                            {
                                originalUserOMI.IntRate = userOMI.IntRate / 100;
                                originalUserOMI.IntRateUsed = userOMI.IntRateUsed;
                            }
                            if (userOMI.LastPriceUsed)
                            {
                                originalUserOMI.LastPrice = userOMI.LastPrice;
                                originalUserOMI.LastPriceUsed = userOMI.LastPriceUsed;

                            }
                            if (userOMI.ProxySymbolUsed)
                            {
                                originalUserOMI.ProxySymbolUsed = userOMI.ProxySymbolUsed;
                            }
                            if (userOMI.TheoreticalPriceUsed)
                            {
                                originalUserOMI.TheoreticalPriceUsed = userOMI.TheoreticalPriceUsed;
                            }
                            if (userOMI.SharesOutstandingUsed)
                            {
                                originalUserOMI.SharesOutstanding = userOMI.SharesOutstanding;
                                originalUserOMI.SharesOutstandingUsed = userOMI.SharesOutstandingUsed;
                            }
                            if (userOMI.ClosingMarkUsed)
                            {
                                originalUserOMI.ClosingMarkUsed = userOMI.ClosingMarkUsed;
                            }
                            if (userOMI.ManualInput)
                            {
                                originalUserOMI.ManualInput = userOMI.ManualInput;
                            }
                            if (userOMI.IsDirtyToSave)
                                originalUserOMI.IsDirtyToSave = userOMI.IsDirtyToSave;
                            // Kuldeep A.: In case of historical trades, if they are retraded then they have this flag value "TRUE", so updating it, 
                            // otherwise they will not be shown on PI.
                            originalUserOMI.IsHistorical = userOMI.IsHistorical;

                            originalUserOMI.PersistenceStatus = userOMI.PersistenceStatus;
                        }
                        else
                        {
                            _dictOMIData.Add(userOMI.Symbol, userOMI);
                        }

                        if (!string.IsNullOrEmpty(userOMI.ProxySymbol))
                        {
                            List<SymbolInfo> listSymbolinfo = new List<SymbolInfo>();
                            SymbolInfo symInfo = new SymbolInfo();
                            if (_dictProxyWiseSymbols.ContainsKey(userOMI.ProxySymbol))
                            {
                                if (userOMI.ProxySymbolUsed)
                                {
                                    symInfo.Symbol = userOMI.Symbol;
                                    symInfo.UnderlyingSymbol = userOMI.UnderlyingSymbol;
                                    _dictProxyWiseSymbols[userOMI.ProxySymbol].Add(symInfo);
                                }
                                else
                                {
                                    foreach (SymbolInfo info in _dictProxyWiseSymbols[userOMI.ProxySymbol])
                                    {
                                        if (info.Symbol == userOMI.Symbol)
                                        {
                                            listSymbolinfo.Add(info);
                                        }
                                    }
                                    foreach (SymbolInfo inf in listSymbolinfo)
                                    {
                                        _dictProxyWiseSymbols[userOMI.ProxySymbol].Remove(inf);
                                    }
                                }
                            }
                            else
                            {
                                if (userOMI.ProxySymbolUsed)
                                {
                                    symInfo.Symbol = userOMI.Symbol;
                                    symInfo.UnderlyingSymbol = userOMI.UnderlyingSymbol;
                                    listSymbolinfo.Add(symInfo);
                                    _dictProxyWiseSymbols.Add(userOMI.ProxySymbol, listSymbolinfo);
                                }
                            }
                        }
                    }
                    PublishData(OMIPublishType.OMIData);
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

        // This is called on refresh OMI/restart of pricing server and on closing data.
        public static void UpdateCacheFromDataSet(DataSet dtOMIData)
        {
            try
            {
                if (dtOMIData != null && dtOMIData.Tables != null && dtOMIData.Tables.Count > 0)
                {
                    UserOptModelInput userOMI = null;
                    _dictOMIData.Clear();
                    foreach (DataRow dr in dtOMIData.Tables[0].Rows)
                    {
                        string symbol = dr["Symbol"].ToString().ToUpper();
                        if (!_dictOMIData.ContainsKey(symbol))
                        {
                            userOMI = new UserOptModelInput();
                            userOMI.UnderlyingSymbol = dr["UnderlyingSymbol"].ToString().ToUpper();
                            userOMI.Symbol = symbol;
                            userOMI.HistoricalVol = Convert.ToDouble(dr["HistoricalVolatility"].ToString()) / 100;
                            userOMI.HistoricalVolUsed = Convert.ToBoolean(dr["HistoricalVolatilityUsed"].ToString());
                            userOMI.Volatility = Convert.ToDouble(dr["UserVolatility"].ToString()) / 100;
                            userOMI.VolatilityUsed = Convert.ToBoolean(dr["UserVolatilityUsed"].ToString());
                            userOMI.IntRate = Convert.ToDouble(dr["UserInterestRate"].ToString()) / 100;
                            userOMI.IntRateUsed = Convert.ToBoolean(dr["UserInterestRateUsed"].ToString());
                            userOMI.ForwardPointsUsed = Convert.ToBoolean(dr["UserForwardPointsUsed"].ToString());
                            /// Dividend yield converted into absolute terms. Have to multiply for 100 while displaying on the UI.
                            /// OMI data picked up from DB
                            userOMI.Dividend = Convert.ToDouble(dr["UserDividend"].ToString()) / 100;
                            userOMI.DividendUsed = Convert.ToBoolean(dr["UserDividendUsed"].ToString());
                            userOMI.StockBorrowCost = Convert.ToDouble(dr["UserStockBorrowCost"].ToString()) / 100;
                            userOMI.StockBorrowCostUsed = Convert.ToBoolean(dr["UserStockBorrowCostUsed"].ToString());
                            userOMI.Delta = Convert.ToDouble(dr["UserDelta"].ToString());
                            userOMI.DeltaUsed = Convert.ToBoolean(dr["UserDeltaUsed"].ToString());
                            userOMI.LastPrice = Convert.ToDouble(dr["UserLastPrice"].ToString());
                            userOMI.LastPriceUsed = Convert.ToBoolean(dr["UserLastPriceUsed"].ToString());
                            userOMI.AssetID = Convert.ToInt32(dr["AssetID"].ToString());
                            userOMI.ForwardPoints = Convert.ToDouble(dr["UserForwardPoints"].ToString());
                            userOMI.TheoreticalPriceUsed = Convert.ToBoolean(dr["UserTheoreticalPriceUsed"].ToString());
                            userOMI.ProxySymbolUsed = Convert.ToBoolean(dr["UserProxySymbolUsed"].ToString());
                            userOMI.ProxySymbol = dr["ProxySymbol"].ToString();
                            userOMI.StrikePrice = Convert.ToDouble(dr["StrikePrice"].ToString());
                            try
                            {
                                userOMI.LeadCurrencyID = Convert.ToInt16(dr["VsCurrencyID"].ToString());
                                userOMI.LeadCurrencyID = Convert.ToInt16(dr["LeadCurrencyID"].ToString());
                            }
                            catch (Exception)
                            {

                            }
                            userOMI.SecurityDescription = Convert.ToString(dr["SecurityDescription"].ToString());
                            userOMI.Bloomberg = Convert.ToString(dr["BloombergSymbol"].ToString());
                            userOMI.OSIOptionSymbol = Convert.ToString(dr["OSISymbol"].ToString());
                            userOMI.IDCOOptionSymbol = Convert.ToString(dr["IDCOSymbol"].ToString());
                            userOMI.IsHistorical = Convert.ToBoolean(dr["IsHistorical"].ToString());
                            userOMI.SharesOutstanding = Convert.ToDouble(dr["UserSharesOutstanding"].ToString());
                            userOMI.SharesOutstandingUsed = Convert.ToBoolean(dr["UserSharesOutstandingUsed"].ToString());
                            userOMI.SMSharesOutstanding = Convert.ToDouble(dr["SMUserSharesOutstanding"].ToString());
                            userOMI.SMSharesOutstandingUsed = Convert.ToBoolean(dr["SMUserSharesOutstandingUsed"].ToString());
                            userOMI.ClosingMarkUsed = Convert.ToBoolean(dr["UserClosingMarkUsed"].ToString());
                            userOMI.AuecID = Convert.ToInt32(dr["AuecID"]);
                            userOMI.ManualInput = Convert.ToBoolean(dr["ManualInput"].ToString());
                            userOMI.BloombergSymbolWithExchangeCode = Convert.ToString(dr[const_BloombergSymbolExCode].ToString());
                            string callorPut = Convert.ToString(dr["PutorCall"]);

                            if (!string.IsNullOrEmpty(callorPut) && callorPut != " ")
                            {
                                if (callorPut.Equals("CALL"))
                                {
                                    userOMI.PutorCall = OptionType.CALL;
                                }
                                else if (callorPut.Equals("PUT"))
                                {
                                    userOMI.PutorCall = OptionType.PUT;
                                }
                            }
                            userOMI.ExpirationDate = Convert.ToDateTime(dr["ExpirationDate"].ToString());
                            _dictOMIData.Add(symbol, userOMI);

                            if (!string.IsNullOrEmpty(userOMI.ProxySymbol))
                            {
                                List<SymbolInfo> listSymbolinfo = new List<SymbolInfo>();
                                SymbolInfo symInfo = new SymbolInfo();
                                if (_dictProxyWiseSymbols.ContainsKey(userOMI.ProxySymbol))
                                {
                                    if (userOMI.ProxySymbolUsed)
                                    {
                                        symInfo.Symbol = userOMI.Symbol;
                                        symInfo.UnderlyingSymbol = userOMI.UnderlyingSymbol;
                                        _dictProxyWiseSymbols[userOMI.ProxySymbol].Add(symInfo);
                                    }
                                    else
                                    {
                                        foreach (SymbolInfo info in _dictProxyWiseSymbols[userOMI.ProxySymbol])
                                        {
                                            if (info.Symbol == userOMI.Symbol)
                                            {
                                                listSymbolinfo.Add(info);
                                            }
                                        }
                                        foreach (SymbolInfo inf in listSymbolinfo)
                                        {
                                            _dictProxyWiseSymbols[userOMI.ProxySymbol].Remove(inf);
                                        }
                                    }
                                }
                                else
                                {
                                    if (userOMI.ProxySymbolUsed)
                                    {
                                        symInfo.Symbol = userOMI.Symbol;
                                        symInfo.UnderlyingSymbol = userOMI.UnderlyingSymbol;
                                        listSymbolinfo.Add(symInfo);
                                        _dictProxyWiseSymbols.Add(userOMI.ProxySymbol, listSymbolinfo);
                                    }
                                }
                            }
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

        public static void UpdateCacheFromImportOMICollection(List<UserOptModelInput> OMIImportCollection)
        {
            try
            {
                lock (_locker)
                {
                    foreach (UserOptModelInput userOMI in OMIImportCollection)
                    {
                        UserOptModelInput originalUserOMI = new UserOptModelInput();
                        if (_dictOMIData.ContainsKey(userOMI.Symbol))
                        {
                            originalUserOMI = _dictOMIData[userOMI.Symbol];
                            originalUserOMI.Delta = userOMI.Delta;
                            originalUserOMI.DeltaUsed = userOMI.DeltaUsed;
                            originalUserOMI.Dividend = userOMI.Dividend / 100;
                            originalUserOMI.DividendUsed = userOMI.DividendUsed;
                            originalUserOMI.StockBorrowCost = userOMI.StockBorrowCost / 100;
                            originalUserOMI.StockBorrowCostUsed = userOMI.StockBorrowCostUsed;
                            originalUserOMI.ForwardPoints = userOMI.ForwardPoints;
                            originalUserOMI.ForwardPointsUsed = userOMI.ForwardPointsUsed;
                            originalUserOMI.HistoricalVolUsed = userOMI.HistoricalVolUsed;
                            originalUserOMI.Volatility = userOMI.Volatility / 100;
                            originalUserOMI.VolatilityUsed = userOMI.VolatilityUsed;
                            originalUserOMI.IntRate = userOMI.IntRate / 100;
                            originalUserOMI.IntRateUsed = userOMI.IntRateUsed;
                            originalUserOMI.LastPrice = userOMI.LastPrice;
                            originalUserOMI.LastPriceUsed = userOMI.LastPriceUsed;
                            originalUserOMI.TheoreticalPriceUsed = userOMI.TheoreticalPriceUsed;
                            originalUserOMI.SharesOutstanding = userOMI.SharesOutstanding;
                            originalUserOMI.SharesOutstandingUsed = userOMI.SharesOutstandingUsed;
                        }
                        else
                        {
                            _dictOMIData.Add(userOMI.Symbol, userOMI);
                        }
                    }
                    PublishData(OMIPublishType.OMIData);
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
        #endregion

        private static void PublishData(OMIPublishType omiPublishType)
        {
            try
            {
                if (_proxyPublishing != null && _dictOMIData != null && _dictOMIData.Count > 0)
                {
                    MessageData e = new MessageData();

                    List<object> temp = new List<object>();
                    temp.Add(omiPublishType);
                    switch (omiPublishType)
                    {
                        case OMIPublishType.OMIData:
                            temp.Add(_dictOMIData.Values.ToList());
                            break;
                        case OMIPublishType.OMIPreferences:
                            temp.Add(GetLiveFeedPreferences());
                            break;
                    }

                    e.EventData = temp;
                    e.TopicName = Topics.Topic_OMIData;
                    //_proxyPublishing.InnerChannel.Publish(e, Topics.Topic_OMIData);
                    CentralizePublish(e);
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
        private static void CentralizePublish(MessageData msgData)
        {
            try
            {
                lock (_publishLock)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            _proxyPublishing.InnerChannel.Publish(msgData, msgData.TopicName);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
                    });
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
        private static void CreateSecMasterServicesProxy()
        {
            try
            {
                _secMasterServices = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static int GetcurrencyIdforSymbol(string symbol)
        {
            try
            {
                if (_secMasterServices != null)
                    return _secMasterServices.InnerChannel.GetCurrencyIdForSymbol(symbol);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }
    }

    public struct SymbolInfo
    {
        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _underlyingSymbol;
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }
    }
}
