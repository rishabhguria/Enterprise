using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities.FTPUtility;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.OptionCalculator.Common
{
    public static class FilePricingCache
    {
        private static ConcurrentDictionary<string, SymbolData> _dictCachedData;
        private static ConcurrentDictionary<string, SecMasterBaseObj> _dictSymbols;
        private static object _locker = new object();
        private static ThirdPartyFtp _thirdPartyFtp = GetFTPDetails();
        private static System.Threading.Timer filePricingTimer = null;
        private static int _allPricesFileReadInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIGKEY_AllPricesFileReadInterval));
        /// <summary>
        /// The is FTP logging enabled
        /// </summary>
        private static bool _isFTPLoggingEnabled = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsFTPLoggingEnabled"));

        private static int mode = Convert.ToInt32(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_AppSetting, ConfigurationHelper.CONFIGKEY_PricingFileRetrievalMode));

        private static int _initialFilePricingDelay = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("InitialFilePricingDelay"));
        private static string _allLivePricesFilePath = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIGKEY_AllLivePricesFilePath);
        private static string _LivePricesFilePath = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIGKEY_LatestLivePricesFilePath);
        private static int _livePricesFileReadInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIGKEY_LivePricesFileReadInterval));
        private static Prana.Global.ApplicationConstants.SymbologyCodes _symbologyForLivePricesFile = (Prana.Global.ApplicationConstants.SymbologyCodes)(Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_SymbologyForLivePricesFile)));
        private static bool _isLiveFeedAvailable = false;
        /// <summary>
        /// Occurs when [connected].
        /// </summary>
        public static event EventHandler<EventArgs<bool>> Connected;

        private static List<string> _lstLatestPricesTemplate = null;
        private static List<string> _lstAllPricesTemplate = null;
        private static bool _isFirstTick = true;
        private static List<string> _lstLatestPricesFileProcessed = new List<string>();

        public enum PricesFileType : ushort
        {
            AllPrices = 0,
            LatestPrices = 1
        }

        static FilePricingCache()
        {
            _dictCachedData = new ConcurrentDictionary<string, SymbolData>();
            _dictSymbols = new ConcurrentDictionary<string, SecMasterBaseObj>();
            CreateSecMasterServicesProxy();

            #region Start File Pricing Timer
            filePricingTimer = new System.Threading.Timer(filePricingTimer_TimerTickHandler, null, _initialFilePricingDelay, _livePricesFileReadInterval);
            #endregion
        }

        private static readonly object _timerLocker = new object();
        private static void filePricingTimer_TimerTickHandler(object state)
        {
            try
            {
                lock (_timerLocker)
                {
                    if (_isFirstTick)
                    {
                        _isFirstTick = false;
                        string directory = Path.Combine(Application.StartupPath, "PricingImport\\");

                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        else
                        {
                            FileInfo[] files = new DirectoryInfo(directory).GetFiles();
                            foreach (FileInfo file in files)
                            {
                                file.Delete();
                            }
                        }
                        UpdateCacheFromPricesFileAsync(_allLivePricesFilePath, _allPricesFileReadInterval, PricesFileType.AllPrices);
                    }
                    else
                        UpdateCacheFromPricesFileAsync(_LivePricesFilePath, _livePricesFileReadInterval, PricesFileType.LatestPrices);
                }
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

        static ProxyBase<ISecMasterSyncServices> _secMasterServices = null;
        public static ProxyBase<ISecMasterSyncServices> SecMasterServices
        {
            set { _secMasterServices = value; }
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

        #region File Pricing Cache Update

        public static bool UpdateCacheFromPricesFileAsync(string pricesFilePath, int pricesFileReadInterval, PricesFileType pricesFileFormat)
        {
            bool result = false;
            try
            {
                result = UpdateCacheFromPricesFile(pricesFilePath, pricesFileReadInterval, pricesFileFormat);
                //bool result = await BG_RunRevaluationProcess(endDate, userID, fundIDs).ConfigureAwait(false);
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

        /// <summary>
        /// Read all prices and latest prices files
        /// </summary>
        /// <param name="pricesFilePath"></param>
        /// <param name="pricesFileReadInterval"></param>
        /// <param name="pricesFileTemplate"></param>
        /// <param name="lstAlreadyProcessedFiles"></param>
        public static bool UpdateCacheFromPricesFile(string pricesFilePath, int pricesFileReadInterval, PricesFileType pricesFileFormat)
        {
            try
            {
                List<string> lstFiles;
                if (pricesFileFormat.Equals(PricesFileType.AllPrices))
                {
                    List<string> lstAllPricesFileProcessed = new List<string>();
                    lstFiles = FTPHelper.GetAllFilesOnFTPForTimeInterval(_thirdPartyFtp, pricesFilePath, pricesFileReadInterval, ref lstAllPricesFileProcessed, mode, _isFTPLoggingEnabled);
                }
                else
                {
                    lstFiles = FTPHelper.GetAllFilesOnFTPForTimeInterval(_thirdPartyFtp, pricesFilePath, pricesFileReadInterval, ref _lstLatestPricesFileProcessed, mode, _isFTPLoggingEnabled);
                }
                if (!_isLiveFeedAvailable && lstFiles.Count > 0 && Connected != null)
                {
                    _isLiveFeedAvailable = true;
                    Connected(null, (new EventArgs<bool>(true)));
                }
                foreach (string filePath in lstFiles)
                {
                    DataTable dTable = FTPHelper.GetDataTableFromFTP(_thirdPartyFtp, filePath, _isFTPLoggingEnabled);
                    if (dTable != null && dTable.Rows.Count > 0)
                    {
                        if (dTable != null && dTable.Rows.Count > 0)
                        {
                            List<string> lstNonCachedSymbols = new List<string>();
                            foreach (DataRow row in dTable.Rows)
                            {
                                if (row[0] != DBNull.Value && !string.IsNullOrEmpty(row[0].ToString()))
                                {
                                    List<string> lstPricingTemplate = GetPricesFileTemplate(pricesFileFormat, dTable);
                                    string bbgSymbol = row[0].ToString().Trim();
                                    //string symbol = 
                                    if (_dictCachedData.ContainsKey(bbgSymbol))
                                    {
                                        SymbolData data = _dictCachedData[bbgSymbol];
                                        if (data.CategoryCode != AssetCategory.None)
                                            PricingSymbolDataMapper.ParseDataRowToSymbolData(_dictCachedData[bbgSymbol], row, lstPricingTemplate, _symbologyForLivePricesFile);
                                        else
                                            _dictCachedData.TryRemove(bbgSymbol, out data);
                                    }
                                    if (!_dictCachedData.ContainsKey(bbgSymbol))
                                    {
                                        SymbolData data = null;
                                        if (_dictSymbols.ContainsKey(bbgSymbol))
                                        {
                                            AssetCategory assetCat = (AssetCategory)_dictSymbols[bbgSymbol].AssetID;
                                            switch (assetCat)
                                            {
                                                case AssetCategory.Equity:// equity
                                                case AssetCategory.PrivateEquity:// private equity
                                                case AssetCategory.CreditDefaultSwap:// credit default swap
                                                    data = new EquitySymbolData();
                                                    data.CategoryCode = AssetCategory.Equity;
                                                    break;

                                                case AssetCategory.EquityOption:// equity option
                                                    data = new OptionSymbolData();
                                                    data.CategoryCode = AssetCategory.EquityOption;
                                                    SecMasterOptObj optData = (SecMasterOptObj)_dictSymbols[bbgSymbol];
                                                    if (optData != null)
                                                    {
                                                        data.StrikePrice = optData.StrikePrice;
                                                        data.ExpirationDate = optData.ExpirationDate;
                                                        data.PutOrCall = (OptionType)optData.PutOrCall;
                                                    }
                                                    break;

                                                case AssetCategory.FutureOption:// furure option
                                                    data = new OptionSymbolData();
                                                    data.CategoryCode = AssetCategory.FutureOption;
                                                    break;

                                                case AssetCategory.FXOption: // fx option
                                                    data = new OptionSymbolData();
                                                    data.CategoryCode = AssetCategory.FXOption;
                                                    break;

                                                case AssetCategory.Future: // future
                                                    data = new FutureSymbolData();
                                                    data.CategoryCode = AssetCategory.Future;
                                                    break;

                                                case AssetCategory.FX: //fx
                                                    data = new FxContractSymbolData();
                                                    data.CategoryCode = AssetCategory.FX;
                                                    break;

                                                case AssetCategory.FXForward:// fxforward
                                                    data = new FxForwardContractSymbolData();
                                                    data.CategoryCode = AssetCategory.FXForward;
                                                    break;

                                                case AssetCategory.Indices: // indices
                                                    data = new IndexSymbolData();
                                                    data.CategoryCode = AssetCategory.Indices;
                                                    break;

                                                case AssetCategory.FixedIncome:// fixedincome
                                                    data = new FixedIncomeSymbolData();
                                                    data.CategoryCode = AssetCategory.FixedIncome;
                                                    break;

                                                case AssetCategory.ConvertibleBond:// convertible bond
                                                    data = new FixedIncomeSymbolData();
                                                    data.CategoryCode = AssetCategory.ConvertibleBond;
                                                    break;
                                                default:
                                                    data = new EquitySymbolData();
                                                    data.CategoryCode = AssetCategory.Equity;
                                                    break;
                                            }
                                            data.UnderlyingSymbol = _dictSymbols[bbgSymbol].UnderLyingSymbol;

                                            data.AUECID = _dictSymbols[bbgSymbol].AUECID;
                                            data.ListedExchange = CachedDataManager.GetInstance.GetAUECText(data.AUECID);
                                        }
                                        else
                                        {
                                            data = new SymbolData();
                                        }
                                        PricingSymbolDataMapper.ParseDataRowToSymbolData(data, row, lstPricingTemplate, _symbologyForLivePricesFile);
                                        _dictCachedData.TryAdd(bbgSymbol, data);
                                    }
                                    if (_dictSymbols.ContainsKey(bbgSymbol))
                                    {
                                        _dictCachedData[bbgSymbol].Symbol = _dictSymbols[bbgSymbol].TickerSymbol;
                                        //_dictCachedData[bbgSymbol].CategoryCode = _dictSymbols[bbgSymbol].AssetCategory;
                                    }
                                    else if (!bbgSymbol.EndsWith(" Curncy"))
                                    {
                                        lstNonCachedSymbols.Add(bbgSymbol);
                                    }
                                }
                            }
                            UpdatePricingSymbolsCache(lstNonCachedSymbols);
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
                return false;
            }
            return true;
        }

        /// <summary>
        /// Update pricing symbols cache after processing of each single file
        /// Dictionary is of Tikcer symbol + Requested symbol (Based on symbology)
        /// </summary>
        /// <param name="lstNonCachedSymbols"></param>
        private static void UpdatePricingSymbolsCache(List<string> lstNonCachedSymbols)
        {
            try
            {
                //proxy is hosted on trade server,if trade server is not started then Innerchannel will be zero.
                if (_secMasterServices == null || _secMasterServices.InnerChannel == null)
                {
                    CreateSecMasterServicesProxy();
                }
                if (_secMasterServices != null && _secMasterServices.InnerChannel != null)
                {
                    Dictionary<string, SecMasterBaseObj> dictNewlyResponsedSymbols = new Dictionary<string, SecMasterBaseObj>();
                    try
                    {
                        dictNewlyResponsedSymbols = _secMasterServices.InnerChannel.GetSecMasterSymbolData(lstNonCachedSymbols, _symbologyForLivePricesFile);
                    }
                    catch
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("TradeService not connected", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }

                    foreach (KeyValuePair<string, SecMasterBaseObj> kvp in dictNewlyResponsedSymbols)
                    {
                        if (!_dictSymbols.ContainsKey(kvp.Key) && kvp.Value != null)
                        {
                            lock (_locker)
                            {
                                _dictSymbols.TryAdd(kvp.Key, kvp.Value);
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

        /// <summary>
        /// Return temaple based on file type
        /// </summary>
        /// <param name="pricesFileFormat"></param>
        /// <param name="dTable"></param>
        /// <returns></returns>
        private static List<string> GetPricesFileTemplate(PricesFileType pricesFileFormat, DataTable dTable)
        {
            List<string> lstPricingTemplate = new List<string>();
            try
            {
                if (pricesFileFormat.Equals(PricesFileType.LatestPrices))
                {
                    //initialize latest prices file template
                    if (_lstLatestPricesTemplate == null)
                    {
                        _lstLatestPricesTemplate = ((from dc in dTable.Columns.Cast<DataColumn>() select dc.ColumnName).ToArray()).ToList<string>();
                    }
                    if (_lstLatestPricesTemplate != null)
                    {
                        lstPricingTemplate = _lstLatestPricesTemplate;
                    }
                }
                else
                {
                    //initialize latest prices file template
                    if (_lstAllPricesTemplate == null)
                    {
                        _lstAllPricesTemplate = ((from dc in dTable.Columns.Cast<DataColumn>() select dc.ColumnName).ToArray()).ToList<string>();
                    }
                    if (_lstAllPricesTemplate != null)
                    {
                        lstPricingTemplate = _lstAllPricesTemplate;
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
            return lstPricingTemplate;
        }

        #endregion
        private static ThirdPartyFtp GetFTPDetails()
        {
            ThirdPartyFtp thirdPartyFtp = new ThirdPartyFtp();
            try
            {
                thirdPartyFtp.FtpName = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_FtpName); ;
                thirdPartyFtp.FtpType = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_FtpType);
                thirdPartyFtp.Host = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_Host); ;
                thirdPartyFtp.Port = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_Port));
                thirdPartyFtp.UsePassive = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_UsePassive));
                //thirdPartyFtp.UseSsl = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_UseSsl));
                thirdPartyFtp.UserName = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_UserName);
                thirdPartyFtp.PassPhrase = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_PassPhrase);
                thirdPartyFtp.Password = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_Password);
                thirdPartyFtp.KeyFingerPrint = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_KeyFile);
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
            return thirdPartyFtp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictLiveFeedData"></param>
        public static void UpdatePricesFromCache(Dictionary<string, SymbolData> dictLiveFeedData)
        {
            try
            {
                foreach (KeyValuePair<string, SymbolData> item in _dictCachedData)
                {
                    string requestedSymbol = item.Key;
                    string tickerSymbol = string.Empty;
                    if (_dictSymbols.ContainsKey(requestedSymbol))
                    {
                        tickerSymbol = _dictSymbols[requestedSymbol].TickerSymbol;
                    }
                    else if (requestedSymbol.EndsWith(" Curncy") && requestedSymbol.Length == 10)
                    {
                        string[] tokens = requestedSymbol.Split(' ');
                        if (tokens.Length == 2)
                        {
                            bool isHigherCur = false;
                            string cur = tokens[0];
                            switch (cur)
                            {
                                case "EUR":
                                case "GBP":
                                case "NZD":
                                case "AUD":
                                    isHigherCur = true;
                                    break;
                            }
                            item.Value.CategoryCode = AssetCategory.Forex;
                            item.Value.ListedExchange = "FX_EXCHANGE";
                            SymbolData symbolData1 = (SymbolData)DeepCopyHelper.Clone(item.Value);
                            symbolData1.Symbol = cur + "-USD";
                            if (!isHigherCur)
                            {
                                symbolData1.SelectedFeedPrice = symbolData1.SelectedFeedPrice == 0d ? 0d : 1 / symbolData1.SelectedFeedPrice;
                                symbolData1.LastPrice = symbolData1.LastPrice == 0d ? 0d : 1 / symbolData1.LastPrice;
                                symbolData1.Ask = symbolData1.Ask == 0d ? 0d : 1 / symbolData1.Ask;
                                symbolData1.Bid = symbolData1.Bid == 0d ? 0d : 1 / symbolData1.Bid;
                                symbolData1.Mid = symbolData1.Mid == 0d ? 0d : 1 / symbolData1.Mid;
                                symbolData1.iMid = symbolData1.iMid == 0d ? 0d : 1 / symbolData1.iMid;
                                symbolData1.High = symbolData1.High == 0d ? 0d : 1 / symbolData1.High;
                                symbolData1.Low = symbolData1.Low == 0d ? 0d : 1 / symbolData1.Low;
                                symbolData1.Previous = symbolData1.Previous == 0d ? 0d : 1 / symbolData1.Previous;
                            }
                            dictLiveFeedData[symbolData1.Symbol] = symbolData1;
                            SymbolData symbolData2 = (SymbolData)DeepCopyHelper.Clone(item.Value);
                            symbolData2.Symbol = "USD-" + cur;
                            if (isHigherCur)
                            {
                                symbolData2.SelectedFeedPrice = symbolData2.SelectedFeedPrice == 0d ? 0d : 1 / symbolData2.SelectedFeedPrice;
                                symbolData2.LastPrice = symbolData2.LastPrice == 0d ? 0d : 1 / symbolData2.LastPrice;
                                symbolData2.Ask = symbolData2.Ask == 0d ? 0d : 1 / symbolData2.Ask;
                                symbolData2.Bid = symbolData2.Bid == 0d ? 0d : 1 / symbolData2.Bid;
                                symbolData2.Mid = symbolData2.Mid == 0d ? 0d : 1 / symbolData2.Mid;
                                symbolData2.iMid = symbolData2.iMid == 0d ? 0d : 1 / symbolData2.iMid;
                                symbolData2.High = symbolData2.High == 0d ? 0d : 1 / symbolData2.High;
                                symbolData2.Low = symbolData2.Low == 0d ? 0d : 1 / symbolData2.Low;
                                symbolData2.Previous = symbolData2.Previous == 0d ? 0d : 1 / symbolData2.Previous;
                            }
                            dictLiveFeedData[symbolData2.Symbol] = symbolData2;
                        }
                    }
                    // string tickerSymbol = item.Value.Symbol.Trim().ToUpper();
                    if (dictLiveFeedData.ContainsKey(tickerSymbol))
                    {
                        FillSymbolData(dictLiveFeedData, tickerSymbol, requestedSymbol);
                    }
                    else if (!string.IsNullOrEmpty(tickerSymbol))
                    {
                        //add symbol data that does not exist
                        SymbolData symbolData = (SymbolData)DeepCopyHelper.Clone(item.Value);
                        symbolData.Symbol = tickerSymbol;
                        dictLiveFeedData.Add(tickerSymbol, symbolData);
                    }
                }
                //SymbolDataHandling(dictLiveFeedData, _dictCachedData);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictLiveFeedData"></param>
        /// <param name="tickerSymbol"></param>
        /// <param name="requestedSymbol"></param>
        private static void FillSymbolData(Dictionary<string, SymbolData> dictLiveFeedData, string tickerSymbol, string requestedSymbol)
        {
            try
            {
                if (_lstLatestPricesTemplate != null && _lstLatestPricesTemplate.Count > 0)
                {
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "SYMBOL"))
                    {
                        dictLiveFeedData[tickerSymbol].Symbol = _dictCachedData[requestedSymbol].Symbol;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "BBGID"))
                    {
                        dictLiveFeedData[tickerSymbol].BBGID = _dictCachedData[requestedSymbol].BBGID;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "SEDOLSYMBOL"))
                    {
                        dictLiveFeedData[tickerSymbol].SedolSymbol = _dictCachedData[requestedSymbol].SedolSymbol;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "REUTERSYMBOL"))
                    {
                        dictLiveFeedData[tickerSymbol].ReuterSymbol = _dictCachedData[requestedSymbol].ReuterSymbol;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "BLOOMBERGSYMBOL"))
                    {
                        dictLiveFeedData[tickerSymbol].BloombergSymbol = _dictCachedData[requestedSymbol].BloombergSymbol;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "FULLCOMPANYNAME"))
                    {
                        dictLiveFeedData[tickerSymbol].FullCompanyName = _dictCachedData[requestedSymbol].FullCompanyName;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "CATEGORYCODE"))
                    {
                        dictLiveFeedData[tickerSymbol].CategoryCode = _dictCachedData[requestedSymbol].CategoryCode;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "DELTASOURCE"))
                    {
                        dictLiveFeedData[tickerSymbol].DeltaSource = _dictCachedData[requestedSymbol].DeltaSource;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "PRICINGPROVIDER"))
                    {
                        dictLiveFeedData[tickerSymbol].MarketDataProvider = _dictCachedData[requestedSymbol].MarketDataProvider;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "UNDERLYINGCATEGORY"))
                    {
                        dictLiveFeedData[tickerSymbol].UnderlyingCategory = _dictCachedData[requestedSymbol].UnderlyingCategory;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "ASK"))
                    {
                        dictLiveFeedData[tickerSymbol].Ask = _dictCachedData[requestedSymbol].Ask;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "BID"))
                    {
                        dictLiveFeedData[tickerSymbol].Bid = _dictCachedData[requestedSymbol].Bid;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "BIDSIZE"))
                    {
                        dictLiveFeedData[tickerSymbol].BidSize = _dictCachedData[requestedSymbol].BidSize;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "BIDEXCHANGE"))
                    {
                        dictLiveFeedData[tickerSymbol].BidExchange = _dictCachedData[requestedSymbol].BidExchange;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "ASKSIZE"))
                    {
                        dictLiveFeedData[tickerSymbol].AskSize = _dictCachedData[requestedSymbol].AskSize;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "ASKEXCHANGE"))
                    {
                        dictLiveFeedData[tickerSymbol].AskExchange = _dictCachedData[requestedSymbol].AskExchange;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "MID"))
                    {
                        dictLiveFeedData[tickerSymbol].Mid = _dictCachedData[requestedSymbol].Mid;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "IMID"))
                    {
                        dictLiveFeedData[tickerSymbol].iMid = _dictCachedData[requestedSymbol].iMid;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "DELAYED"))
                    {
                        dictLiveFeedData[tickerSymbol].PricingStatus = _dictCachedData[requestedSymbol].PricingStatus;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "MARKETCAPITALIZATION"))
                    {
                        dictLiveFeedData[tickerSymbol].MarketCapitalization = _dictCachedData[requestedSymbol].MarketCapitalization;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "SHARESOUTSTANDING"))
                    {
                        dictLiveFeedData[tickerSymbol].SharesOutstanding = _dictCachedData[requestedSymbol].SharesOutstanding;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "DIVIDEND"))
                    {
                        dictLiveFeedData[tickerSymbol].Dividend = _dictCachedData[requestedSymbol].Dividend;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "XDIVIDENDDATE"))
                    {
                        dictLiveFeedData[tickerSymbol].XDividendDate = _dictCachedData[requestedSymbol].XDividendDate;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "DIVIDENDINTERVAL"))
                    {
                        dictLiveFeedData[tickerSymbol].DividendInterval = _dictCachedData[requestedSymbol].DividendInterval;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "DIVIDENDAMTRATE"))
                    {
                        dictLiveFeedData[tickerSymbol].DividendAmtRate = _dictCachedData[requestedSymbol].DividendAmtRate;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "DIVDISTRIBUTIONDATE"))
                    {
                        dictLiveFeedData[tickerSymbol].DivDistributionDate = _dictCachedData[requestedSymbol].DivDistributionDate;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "ANNUALDIVIDEND"))
                    {
                        dictLiveFeedData[tickerSymbol].AnnualDividend = _dictCachedData[requestedSymbol].AnnualDividend;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "DIVIDENDYIELD"))
                    {
                        dictLiveFeedData[tickerSymbol].DividendYield = _dictCachedData[requestedSymbol].DividendYield;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "FINALDIVIDENDYIELD"))
                    {
                        dictLiveFeedData[tickerSymbol].FinalDividendYield = _dictCachedData[requestedSymbol].FinalDividendYield;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "STOCKBORROWCOST"))
                    {
                        dictLiveFeedData[tickerSymbol].StockBorrowCost = _dictCachedData[requestedSymbol].StockBorrowCost;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "CONVERSIONMETHOD"))
                    {
                        dictLiveFeedData[tickerSymbol].ConversionMethod = _dictCachedData[requestedSymbol].ConversionMethod;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "FORWARDPOINTS"))
                    {
                        dictLiveFeedData[tickerSymbol].ForwardPoints = _dictCachedData[requestedSymbol].ForwardPoints;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "PUTORCALL"))
                    {
                        dictLiveFeedData[tickerSymbol].PutOrCall = _dictCachedData[requestedSymbol].PutOrCall;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "THEORETICALPRICE"))
                    {
                        dictLiveFeedData[tickerSymbol].TheoreticalPrice = _dictCachedData[requestedSymbol].TheoreticalPrice;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "THETA"))
                    {
                        dictLiveFeedData[tickerSymbol].Theta = _dictCachedData[requestedSymbol].Theta;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "VEGA"))
                    {
                        dictLiveFeedData[tickerSymbol].Vega = _dictCachedData[requestedSymbol].Vega;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "RHO"))
                    {
                        dictLiveFeedData[tickerSymbol].Rho = _dictCachedData[requestedSymbol].Rho;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "GAMMA"))
                    {
                        dictLiveFeedData[tickerSymbol].Gamma = _dictCachedData[requestedSymbol].Gamma;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "EXPIRATIONDATE"))
                    {
                        dictLiveFeedData[tickerSymbol].ExpirationDate = _dictCachedData[requestedSymbol].ExpirationDate;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "STRIKEPRICE"))
                    {
                        dictLiveFeedData[tickerSymbol].StrikePrice = _dictCachedData[requestedSymbol].StrikePrice;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "DAYSTOEXPIRATION"))
                    {
                        dictLiveFeedData[tickerSymbol].DaysToExpiration = _dictCachedData[requestedSymbol].DaysToExpiration;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "IMPLIEDVOL"))
                    {
                        dictLiveFeedData[tickerSymbol].ImpliedVol = _dictCachedData[requestedSymbol].ImpliedVol;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "FINALIMPLIEDVOL"))
                    {
                        dictLiveFeedData[tickerSymbol].FinalImpliedVol = _dictCachedData[requestedSymbol].FinalImpliedVol;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "INTERESTRATE"))
                    {
                        dictLiveFeedData[tickerSymbol].InterestRate = _dictCachedData[requestedSymbol].InterestRate;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "FINALINTERESTRATE"))
                    {
                        dictLiveFeedData[tickerSymbol].FinalInterestRate = _dictCachedData[requestedSymbol].FinalInterestRate;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "OPENINTEREST"))
                    {
                        dictLiveFeedData[tickerSymbol].OpenInterest = _dictCachedData[requestedSymbol].OpenInterest;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "UNDERLYINGDATA"))
                    {
                        dictLiveFeedData[tickerSymbol].UnderlyingData = _dictCachedData[requestedSymbol].UnderlyingData;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "CFICODE"))
                    {
                        dictLiveFeedData[tickerSymbol].CFICode = _dictCachedData[requestedSymbol].CFICode;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "OSIOPTIONSYMBOL"))
                    {
                        dictLiveFeedData[tickerSymbol].OSIOptionSymbol = _dictCachedData[requestedSymbol].OSIOptionSymbol;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "IDCOOPTIONSYMBOL"))
                    {
                        dictLiveFeedData[tickerSymbol].IDCOOptionSymbol = _dictCachedData[requestedSymbol].IDCOOptionSymbol;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "OPRASYMBOL"))
                    {
                        dictLiveFeedData[tickerSymbol].OpraSymbol = _dictCachedData[requestedSymbol].OpraSymbol;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "REQUESTEDSYMBOLOGY"))
                    {
                        dictLiveFeedData[tickerSymbol].RequestedSymbology = _dictCachedData[requestedSymbol].RequestedSymbology;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "LASTPRICE"))
                    {
                        dictLiveFeedData[tickerSymbol].LastPrice = _dictCachedData[requestedSymbol].LastPrice;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "PRICINGSOURCE"))
                    {
                        dictLiveFeedData[tickerSymbol].PricingSource = _dictCachedData[requestedSymbol].PricingSource;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "UPDATETIME"))
                    {
                        dictLiveFeedData[tickerSymbol].UpdateTime = _dictCachedData[requestedSymbol].UpdateTime;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "LASTTICK"))
                    {
                        dictLiveFeedData[tickerSymbol].LastTick = _dictCachedData[requestedSymbol].LastTick;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "TRADEVOLUME"))
                    {
                        dictLiveFeedData[tickerSymbol].TradeVolume = _dictCachedData[requestedSymbol].TradeVolume;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "CHANGE"))
                    {
                        dictLiveFeedData[tickerSymbol].Change = _dictCachedData[requestedSymbol].Change;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "PCTCHANGE"))
                    {
                        dictLiveFeedData[tickerSymbol].PctChange = _dictCachedData[requestedSymbol].PctChange;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "MARKPRICE"))
                    {
                        dictLiveFeedData[tickerSymbol].MarkPrice = _dictCachedData[requestedSymbol].MarkPrice;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "VWAP"))
                    {
                        dictLiveFeedData[tickerSymbol].VWAP = _dictCachedData[requestedSymbol].VWAP;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "DELTA"))
                    {
                        dictLiveFeedData[tickerSymbol].Delta = _dictCachedData[requestedSymbol].Delta;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "PREVIOUS"))
                    {
                        dictLiveFeedData[tickerSymbol].Previous = _dictCachedData[requestedSymbol].Previous;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "OPEN"))
                    {
                        dictLiveFeedData[tickerSymbol].Open = _dictCachedData[requestedSymbol].Open;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "HIGH52W"))
                    {
                        dictLiveFeedData[tickerSymbol].High52W = _dictCachedData[requestedSymbol].High52W;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "LOW52W"))
                    {
                        dictLiveFeedData[tickerSymbol].Low52W = _dictCachedData[requestedSymbol].Low52W;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "HIGH"))
                    {
                        dictLiveFeedData[tickerSymbol].High = _dictCachedData[requestedSymbol].High;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "LOW"))
                    {
                        dictLiveFeedData[tickerSymbol].Low = _dictCachedData[requestedSymbol].Low;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "TOTALVOLUME"))
                    {
                        dictLiveFeedData[tickerSymbol].TotalVolume = _dictCachedData[requestedSymbol].TotalVolume;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "AvgVolumeAvgVolume"))
                    {
                        dictLiveFeedData[tickerSymbol].AvgVolume = _dictCachedData[requestedSymbol].AvgVolume;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "PREFERENCEDPRICE"))
                    {
                        dictLiveFeedData[tickerSymbol].PreferencedPrice = _dictCachedData[requestedSymbol].PreferencedPrice;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "AUECID"))
                    {
                        dictLiveFeedData[tickerSymbol].AUECID = _dictCachedData[requestedSymbol].AUECID;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "EXCHANGEID"))
                    {
                        dictLiveFeedData[tickerSymbol].ExchangeID = _dictCachedData[requestedSymbol].ExchangeID;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "CUSIPNO"))
                    {
                        dictLiveFeedData[tickerSymbol].CusipNo = _dictCachedData[requestedSymbol].CusipNo;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "ISIN"))
                    {
                        dictLiveFeedData[tickerSymbol].ISIN = _dictCachedData[requestedSymbol].ISIN;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "LISTEDEXCHANGE"))
                    {
                        dictLiveFeedData[tickerSymbol].ListedExchange = _dictCachedData[requestedSymbol].ListedExchange;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "CURENCYCODE"))
                    {
                        dictLiveFeedData[tickerSymbol].CurencyCode = _dictCachedData[requestedSymbol].CurencyCode;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "AVERAGEVOLUME20DAY"))
                    {
                        dictLiveFeedData[tickerSymbol].AverageVolume20Day = _dictCachedData[requestedSymbol].AverageVolume20Day;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "UNDERLYINGSYMBOL"))
                    {
                        dictLiveFeedData[tickerSymbol].UnderlyingSymbol = _dictCachedData[requestedSymbol].UnderlyingSymbol;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "VOLUME10DAVG"))
                    {
                        dictLiveFeedData[tickerSymbol].Volume10DAvg = _dictCachedData[requestedSymbol].Volume10DAvg;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "BETA_5YRMONTHLY"))
                    {
                        dictLiveFeedData[tickerSymbol].Beta_5yrMonthly = _dictCachedData[requestedSymbol].Beta_5yrMonthly;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "SPREAD"))
                    {
                        dictLiveFeedData[tickerSymbol].Spread = _dictCachedData[requestedSymbol].Spread;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "GAPOPEN"))
                    {
                        dictLiveFeedData[tickerSymbol].GapOpen = _dictCachedData[requestedSymbol].GapOpen;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "ISCHANGEDTOHIGHERCURRENCY"))
                    {
                        dictLiveFeedData[tickerSymbol].IsChangedToHigherCurrency = _dictCachedData[requestedSymbol].IsChangedToHigherCurrency;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "SELECTEDFEEDPRICE"))
                    {
                        dictLiveFeedData[tickerSymbol].SelectedFeedPrice = _dictCachedData[requestedSymbol].SelectedFeedPrice;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "MARKPRICESTR"))
                    {
                        dictLiveFeedData[tickerSymbol].MarkPriceStr = _dictCachedData[requestedSymbol].MarkPriceStr;
                    }
                    if (_lstLatestPricesTemplate.Exists(field => field.ToUpper() == "YESTERDAYCLOSINGMARK"))
                    {
                        dictLiveFeedData[tickerSymbol].MarkPrice = _dictCachedData[requestedSymbol].MarkPrice;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictLiveFeedDataDict"></param>
        /// <param name="dictCachedData"></param>
        public static void SymbolDataHandling(Dictionary<string, SymbolData> dictLiveFeedDataDict, ConcurrentDictionary<string, SymbolData> dictCachedData)
        {
            try
            {
                foreach (SymbolData temp in dictCachedData.Values)
                {
                    SymbolData data = null;
                    OptionSymbolData dataOpt = null;
                    if (!dictLiveFeedDataDict.ContainsKey(temp.Symbol))
                    {
                        if (data != null)
                        {
                            data.Symbol = temp.Symbol;
                            if (MarkCacheManager.LatesMarkPrices != null && MarkCacheManager.LatesMarkPrices.ContainsKey(data.Symbol))
                            {
                                data.MarkPrice = MarkCacheManager.LatesMarkPrices[data.Symbol].MarkPrice;
                                data.MarkPriceStr = MarkCacheManager.LatesMarkPrices[data.Symbol].MarkPriceStr;
                                // copying the mark price in selected price if not OMI overriden and price not coming from LiveFeed...
                                data.SelectedFeedPrice = data.MarkPrice;
                                // TODO
                                data.PricingSource = temp.PricingSource;
                            }
                        }
                        if (dataOpt != null)
                        {
                            dataOpt.Symbol = temp.Symbol;
                            dataOpt.UnderlyingSymbol = temp.UnderlyingSymbol;
                            if (MarkCacheManager.LatesMarkPrices != null && MarkCacheManager.LatesMarkPrices.ContainsKey(dataOpt.Symbol))
                            {
                                dataOpt.MarkPrice = MarkCacheManager.LatesMarkPrices[dataOpt.Symbol].MarkPrice;
                                dataOpt.MarkPriceStr = MarkCacheManager.LatesMarkPrices[dataOpt.Symbol].MarkPriceStr;
                                dataOpt.SelectedFeedPrice = dataOpt.MarkPrice;
                                // TODO
                                data.PricingSource = temp.PricingSource;
                            }
                        }
                    }
                    if (data != null)
                    {
                        data.SharesOutstanding = (long)temp.SharesOutstanding;
                        data.ForwardPoints = temp.ForwardPoints;
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
    }
}