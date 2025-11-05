using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.NewLiveFeed;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.MarketDataAdapter.Common;
using Prana.OptionCalculator.Common;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Prana.ESignalAdapter
{
    /// <summary>
    /// Esignal Manager to make LiveFeed request & process received response
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    /// <seealso cref="Prana.BusinessObjects.ILiveFeedAdapter" />
    public partial class ESignalAdapter : ILiveFeedAdapter, IDisposable
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static ESignalAdapter _instance = null;

        /// <summary>
        /// The level1 timer interval multiple
        /// </summary>
        private int _connectionRetryDelay = Convert.ToInt32(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_LiveFeed, ConfigurationHelper.CONFIGKEY_LiveFeed_ConnectionRetryDelay));

        /// <summary>
        /// The database connection
        /// </summary>
        private IntPtr DBConnection = IntPtr.Zero;

        /// <summary>
        /// The connection status
        /// </summary>
        private bool _connectionStatus = false;

        /// <summary>
        /// The dictionary symbol currencies
        /// </summary>
        private Dictionary<string, string> _dictSymbolCurrencies = new Dictionary<string, string>();

        /// <summary>
        /// The e signal user limit mapper
        /// </summary>
        private Dictionary<int, string> _eSignalUserLimitMapper = null;

        /// <summary>
        /// The turn around identifier
        /// </summary>
        string _turnAroundId = null;

        /// <summary>
        /// The publish lock
        /// </summary>
        private object _publishLock = new object();

        /// <summary>
        /// The proxy publishing
        /// </summary>
        private ProxyBase<IPublishing> _proxyPublishing = null;

        /// <summary>
        /// The dictionary of option chain
        /// </summary>
        private Dictionary<string, List<OptionStaticData>> _dictOfOptionChain = new Dictionary<string, List<OptionStaticData>>();

        /// <summary>
        /// The dictionary option chain requests
        /// </summary>
        private Dictionary<string, List<string>> _dictOptionChainRequests = new Dictionary<string, List<string>>();

        /// <summary>
        /// The symbol maxlength
        /// </summary>
        private const int SYMBOL_MAXLENGTH = 55;

        /// <summary>
        /// The enable esignal logging
        /// </summary>
        private bool _enableMarketDataLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableMarketDataLogging"));

        /// <summary>
        /// Requests the single symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="isSnapshot">if set to <c>true</c> [is snapshot].</param>
        /// <returns></returns>
        private int RequestSingleSymbol(string symbol, bool isSnapshot)
        {
            int lret = int.MinValue;
            try
            {
                string upperSymbol = symbol.Trim().ToUpper();
                if (upperSymbol.Equals(string.Empty))
                    return lret;
                int requestType = isSnapshot ? (int)DBCAPI.RequestType.eREQUESTSNAPSHOT : (int)DBCAPI.RequestType.eADVISE;
                if (DBConnection != IntPtr.Zero)
                    lret = NativeMethods.DbcAddDMSymbol(DBConnection, upperSymbol, requestType);
                return lret;
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
                return lret;
            }
        }

        /// <summary>
        /// Returns the bool whether the respective symbol is internation or not.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>
        ///   <c>true</c> if [is international symbol] [the specified symbol]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsInternationalSymbol(string symbol)
        {
            bool response = false;
            try
            {
                int isTrue = NativeMethods.DbcIsInternationalSymbol(symbol);
                response = isTrue.Equals(1);
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
            return response;
        }

        /// <summary>
        /// Creates the option static data object from string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        private OptionStaticData CreateOptionStaticDataObjFromString(string str)
        {
            OptionStaticData opdata = new OptionStaticData();
            string[] splitarray = str.Split('|');
            string year = string.Empty;
            string month = string.Empty;
            string date = string.Empty;
            try
            {
                //response is for equity options..
                if (splitarray.Length == 12)
                {
                    opdata.Symbol = splitarray[1];
                    string CFICode = splitarray[3];
                    char[] ch = CFICode.ToCharArray(1, 1);
                    if (ch[0] == 'P')
                    {
                        opdata.PutOrCall = OptionType.PUT;
                    }
                    else
                    {
                        opdata.PutOrCall = OptionType.CALL;
                    }

                    opdata.StrikePrice = double.Parse(splitarray[5]);
                    year = splitarray[7].Substring(0, 4);
                    month = splitarray[7].Substring(4, 2);
                    date = splitarray[7].Substring(6, 2);

                    opdata.ExpirationDate = new DateTime(int.Parse(year), int.Parse(month), int.Parse(date));
                    opdata.UnderlyingSymbol = splitarray[9];
                }
                //response is for future options...
                else if (splitarray.Length == 10)
                {
                    opdata.Symbol = splitarray[1];
                    string CFICode = splitarray[3];
                    string[] symbolArr = opdata.Symbol.Split(' ');
                    if (symbolArr.Length > 1)
                    {
                        // eg: ES G1C375 & ES G1P375 is future option
                        if (symbolArr[1].Length >= 2)
                        {
                            if (symbolArr[1].ToUpper().Contains("P"))
                            {
                                opdata.PutOrCall = OptionType.PUT;
                            }
                            else if (symbolArr[1].ToUpper().Contains("C"))
                            {
                                opdata.PutOrCall = OptionType.CALL;
                            }
                        }
                    }
                    opdata.StrikePrice = double.Parse(splitarray[5]);
                    year = splitarray[7].Substring(4, 4);
                    month = splitarray[7].Substring(0, 2);
                    date = splitarray[7].Substring(2, 2);

                    opdata.ExpirationDate = new DateTime(int.Parse(year), int.Parse(month), int.Parse(date));
                    opdata.UnderlyingSymbol = splitarray[9];
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
            return opdata;
        }

        /// <summary>
        /// Creates the publishing proxy.
        /// </summary>
        private void CreatePublishingProxy()
        {
            try
            {
                _proxyPublishing = new ProxyBase<IPublishing>("PricingPublishingEndpointAddress");
            }
            catch (Exception)
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

        /// <summary>
        /// Publishes the preparation.
        /// </summary>
        /// <param name="topicName">Name of the topic.</param>
        /// <param name="publishData">The publish data.</param>
        private void PublishPreparation(string topicName, object publishData)
        {
            try
            {
                List<object> listData = new List<object>();
                listData.Add(publishData);

                MessageData messageData = new MessageData();
                messageData.EventData = listData;
                messageData.TopicName = topicName;
                CentralizePublish(messageData);
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
        /// Centralizes the publish.
        /// </summary>
        /// <param name="msgData">The MSG data.</param>
        private void CentralizePublish(MessageData msgData)
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
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
                    });
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

        #region ILiveFeedAdapter Methods
        /// <summary>
        /// Connects this instance.
        /// </summary>
        public void Connect()
        {
            try
            {
                MarketDataAdapterExtension.ClearCache();
                ConnectDataManager();
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
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
        }

        /// <summary>
        /// Gets the continuous data.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public void GetContinuousData(string symbol)
        {
            try
            {
                if (symbol != null) // can add the check for the vaidity of the symbol
                {
                    RequestSingleSymbol(symbol, false);
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Symbol Is Invalid", LoggingConstants.CATEGORY_WARNING, 1, 1, System.Diagnostics.TraceEventType.Warning);
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
        /// Gets the available live feed.
        /// </summary>
        /// <returns></returns>
        public List<SymbolData> GetAvailableLiveFeed()
        {
            return MarketDataAdapterExtension.GetAvailableLiveFeed();
        }

        /// <summary>
        /// Gets the snap shot data.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="symbologyCode">The symbology code.</param>
        /// <param name="completeInfo">if set to <c>true</c> [complete information].</param>
        public void GetSnapShotData(string symbol, ApplicationConstants.SymbologyCodes symbologyCode, bool completeInfo)
        {
            try
            {
                if (_enableMarketDataLogging)
                {
                    if (Prana.BusinessObjects.UserSettingConstants.IsDebugModeEnabled)
                    {
                        Logger.LoggerWrite("Snapshot Request Received on eSignal for Symbol : " + symbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        InformationReporter.GetInstance.Write("Snapshot Request Received on eSignal for Symbol : " + symbol + " Time : " + DateTime.UtcNow);
                    }
                    else
                    {
                        Logger.LoggerWrite("Snapshot Request Received on eSignal for Symbol : " + symbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                }

                int lret = -1;
                if (symbol != null) // can add the check for the vaidity of the symbol
                {
                    if (symbologyCode.Equals(ApplicationConstants.SymbologyCodes.IDCOOptionSymbol))
                    {
                        // symbol is option symbol always
                        //K is the key formatting character
                        //A means that the input is eSignal format
                        //B means that the input is IDCO-22
                        //Z means that we want to get OSI-21 and IDCO-22 format in the snapshot

                        string transformedSymbol = symbol + " " + eSignBin.OPTIONREQUESTFORMAT_IDCO;
                        lret = RequestSingleSymbol(transformedSymbol, true);
                    }
                    else
                    {
                        // symbol is must be ticker & in esignal terminology.

                        if (eSignalHelper.IsOptionTickerSymbol(symbol))
                        {
                            // if the symbol is option symbol

                            if (completeInfo)
                            {
                                lret = RequestSingleSymbol(symbol, true);
                            }
                            else
                            {
                                //K is the key formatting character
                                //A means that the input is eSignal format (Ticker symbol is eSignal symbol)
                                //B means that the input is IDCO-22
                                //Z means that we want to get OSI-21 and IDCO-22 format in the snapshot
                                string transformedSymbol = symbol + " " + eSignBin.OPTIONREQUESTFORMAT_ESIGNAL;
                                lret = RequestSingleSymbol(transformedSymbol, true);
                            }
                        }
                        else if (eSignalHelper.IsFutureOrFutureOptionSymbol(symbol))
                        {
                            lret = RequestSingleSymbol(symbol, false);
                        }
                        else if (eSignalHelper.IsInternationalFutureSymbol(symbol))
                        {
                            lret = RequestSingleSymbol(symbol, false);
                        }
                        else
                        {
                            // the symbol is non option symbol.
                            lret = RequestSingleSymbol(symbol, true);
                        }
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Symbol Is Invalid", LoggingConstants.CATEGORY_WARNING, 1, 1, System.Diagnostics.TraceEventType.Warning);
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

        /// <summary>
        /// Deletes the symbol.
        /// </summary>
        /// <param name="Symbol">The symbol.</param>
        public void DeleteSymbol(string Symbol)
        {
            try
            {
                NativeMethods.DbcDeleteDMSymbol(DBConnection, Symbol);
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
        /// Gets the option chain.
        /// </summary>
        /// <param name="underlyingSymbol">The underlying symbol.</param>
        /// <param name="month">The month.</param>
        /// <param name="lowerstrike">The lowerstrike.</param>
        /// <param name="upperstrike">The upperstrike.</param>
        /// <param name="turnaroundid">The turnaroundid.</param>
        /// <param name="CategoryCode">The category code.</param>
        public void GetOptionChain(string underlyingSymbol, OptionChainFilter optionChainFilter)
        {
            try
            {
                string symbolWithoutID = string.Empty;
                string finalsymbol = string.Empty;
                string yearmonth = string.Empty;
                string lowstrike = string.Empty;
                string higstrike = string.Empty;

                if (underlyingSymbol == null)
                    return;
                if (optionChainFilter.LowerStrike == double.MinValue && optionChainFilter.UpperStrike == double.MinValue && optionChainFilter.ExpirationDate != null)
                {
                    finalsymbol = "LIST_" + underlyingSymbol + " *O [B10|R" + optionChainFilter.TurnAroundID + "]";
                }
                else
                {
                    //LIST_INTC *O (april2011 STRIKE>20000 STRANGE<25000) [B100]

                    if (optionChainFilter.LowerStrike != double.MinValue)
                    {
                        optionChainFilter.LowerStrike *= 1000;
                        lowstrike = " STRIKE>" + optionChainFilter.LowerStrike.ToString();
                    }
                    if (optionChainFilter.UpperStrike != double.MinValue)
                    {
                        optionChainFilter.UpperStrike *= 1000;
                        higstrike = " STRANGE<" + optionChainFilter.UpperStrike.ToString();
                    }

                    if (optionChainFilter.ExpirationDate != null)
                    {
                        string year = optionChainFilter.ExpirationDate.Year.ToString();
                        string monthname = eSignalHelper.GetMonthName(optionChainFilter.ExpirationDate.Month).ToUpper();
                        yearmonth = monthname + year;
                    }

                    if (optionChainFilter.CategoryCode.Equals(AssetCategory.EquityOption))
                    {
                        symbolWithoutID = "LIST_" + underlyingSymbol + " *O (" + yearmonth + lowstrike + higstrike + ") ";
                        finalsymbol = "LIST_" + underlyingSymbol + " *O (" + yearmonth + lowstrike + higstrike + ") " + "[B100|R" + optionChainFilter.TurnAroundID + "]";
                    }
                    else if (optionChainFilter.CategoryCode.Equals(AssetCategory.FutureOption))
                    {
                        symbolWithoutID = "LIST_" + underlyingSymbol + " *FO (" + yearmonth + lowstrike + higstrike + ") ";
                        finalsymbol = "LIST_" + underlyingSymbol + " *FO (" + yearmonth + lowstrike + higstrike + ") " + "[B100|R" + optionChainFilter.TurnAroundID + "]";
                    }

                    //MUKUL 20121010: placed a check on the length of the option chain request symbol as 55 is the maximum allowed length of the symbol returned by esignal for which snapshot has been requested otherwise the symbol is trimmed
                    //due to which the containskey check fails in _dictOptionChainRequests in update status event as the trimmed symbol doesnt matches with the actual symbol so while adding in the cache i have trimmed
                    // this to max length allowed....
                    if (!string.IsNullOrEmpty(symbolWithoutID) && symbolWithoutID.Length > SYMBOL_MAXLENGTH)
                    {
                        symbolWithoutID = symbolWithoutID.Substring(0, SYMBOL_MAXLENGTH);
                    }
                }
                lock (_dictOptionChainRequests)
                {
                    if (!_dictOptionChainRequests.ContainsKey(symbolWithoutID))
                    {
                        List<string> listIDs = new List<string>();
                        listIDs.Add(optionChainFilter.TurnAroundID);
                        _dictOptionChainRequests.Add(symbolWithoutID, listIDs);
                    }
                    else
                    {
                        _dictOptionChainRequests[symbolWithoutID].Add(optionChainFilter.TurnAroundID);
                    }
                }
                RequestSingleSymbol(finalsymbol, true);
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
                return;
            }
        }

        /// <summary>
        /// Give info for multiple symbols whether they are internationl or not.
        /// </summary>
        /// <param name="symbols">The symbols.</param>
        /// <returns></returns>
        public Dictionary<string, bool> CheckIfInternationalSymbols(List<string> symbols)
        {
            Dictionary<string, bool> symbolLocationDict = new Dictionary<string, bool>();
            try
            {
                bool isInternationaSymbol = false;
                foreach (string symbol in symbols)
                {
                    if (!symbolLocationDict.ContainsKey(symbol))
                    {
                        isInternationaSymbol = IsInternationalSymbol(symbol);
                        symbolLocationDict.Add(symbol, isInternationaSymbol);
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
            return symbolLocationDict;
        }

        /// <summary>
        /// Occurs when [connected].
        /// </summary>
        public event EventHandler<EventArgs<bool>> Connected;

        /// <summary>
        /// Occurs when [disconnected].
        /// </summary>
        public event EventHandler<EventArgs<bool>> Disconnected;

        /// <summary>
        /// Occurs when [continuous data response].
        /// </summary>
        public event EventHandler<Data> ContinuousDataResponse;

        /// <summary>
        /// Occurs when [snap shot data response].
        /// </summary>
        public event EventHandler<Data> SnapShotDataResponse;

        /// <summary>
        /// Occurs when [option chain response].
        /// </summary>
        public event EventHandler<EventArgs<string, List<OptionStaticData>>> OptionChainResponse;
        #endregion

        #region Common Methods
        /// <summary>
        /// Updates snapshotData by processing LRT data type
        /// </summary>
        /// <param name="sData">The s data.</param>
        /// <param name="nData">The n data.</param>
        private void dataManager_UpdateLastRecord(IntPtr sData, uint nData)
        {
            IntPtr[] ptrArr = new IntPtr[1];
            IntPtr pLrt = IntPtr.Zero;
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            try
            {
                long lret = NativeMethods.LrtCreate(nData, ptr);
                if (lret != LastRecordType.LRT_RET_OK)
                {
                    return;
                }

                Marshal.Copy(ptr, ptrArr, 0, 1);
                pLrt = ptrArr[0];
                lret = NativeMethods.LrtLoadData(pLrt, sData, nData);
                if (lret != LastRecordType.LRT_RET_OK)
                {
                    return;
                }

                // When processing LRT data structures you must move the LRT pointer to
                // the first position within the data structure before starting any type
                // of data search.
                string symbol = string.Empty;
                NativeMethods.LrtMoveFirst(pLrt);
                lret = NativeMethods.LrtFind(pLrt, LastRecordType.LRT_TYPE_KEY, LastRecordType.LRT_FMT_CSTRING);
                if (lret == LastRecordType.LRT_RET_OK)
                    symbol = GetLRTString(pLrt);
                if (symbol.Equals(eSignBin.ROSCOMMAND_TICK_PROFILEUSER) || symbol.Equals(eSignBin.ROSCOMMAND_OTHER_PROFILEUSER))
                {
                    NativeMethods.LrtMoveFirst(pLrt);
                    lret = NativeMethods.LrtFind(pLrt, LastRecordType.LRT_TYPE_OTHERAUTHS, LastRecordType.LRT_FMT_CSTRINGARRAY);
                    if (lret == LastRecordType.LRT_RET_OK)
                    {
                        List<string> strings = GetStringList(pLrt);
                        if (strings != null && strings.Count > 0)
                        {
                            OnServicesResponse(strings);
                        }
                        return;
                    }
                    NativeMethods.LrtMoveFirst(pLrt);
                    lret = NativeMethods.LrtFind(pLrt, LastRecordType.LRT_TYPE_TICKAUTHS_EXT, LastRecordType.LRT_FMT_CSTRINGARRAY);
                    if (lret == LastRecordType.LRT_RET_OK)
                    {
                        List<string> strings = GetStringList(pLrt);
                        if (strings != null && strings.Count > 0)
                        {
                            OnServicesResponse(strings);
                        }
                        return;
                    }
                }

                SymbolData snapShotData = MarketDataAdapterExtension.GetSnapShotSymbolData(symbol);

                if (snapShotData == null)
                {
                    NativeMethods.LrtMoveFirst(pLrt);

                    // The pointer is at the first position in the LRT data structure.
                    // Indicate the type of data to find within the LRT data structure.
                    // In this case, search for category data.
                    lret = NativeMethods.LrtFind(pLrt, LastRecordType.LRT_TYPE_CATEGORY, LastRecordType.LRT_FMT_BYTE);
                    // The category data has been located.
                    // Determine the category for this LRT data structure.

                    int categoryCode = int.MinValue;
                    if (lret == LastRecordType.LRT_RET_OK)
                    {
                        categoryCode = Convert.ToInt32(NativeMethods.LrtGetByte(pLrt));
                    }

                    int subcategoryCode = int.MinValue;
                    NativeMethods.LrtMoveFirst(pLrt);
                    lret = NativeMethods.LrtFind(pLrt, LastRecordType.LRT_TYPE_SUBCATEGORY, LastRecordType.LRT_FMT_BYTE);
                    if (lret == LastRecordType.LRT_RET_OK)
                    {
                        subcategoryCode = Convert.ToInt32(NativeMethods.LrtGetByte(pLrt));
                    }

                    string underlyingSymbol = string.Empty;
                    NativeMethods.LrtMoveFirst(pLrt);
                    lret = NativeMethods.LrtFind(pLrt, LastRecordType.LRT_TYPE_UNDERLYING, LastRecordType.LRT_FMT_CSTRING);
                    if (lret == LastRecordType.LRT_RET_OK)
                    {
                        underlyingSymbol = GetLRTString(pLrt);
                    }

                    switch (categoryCode)
                    {
                        case eSignBin.CATEGORY_STOCK:
                            snapShotData = new EquitySymbolData();
                            snapShotData.CategoryCode = AssetCategory.Equity;
                            break;

                        case eSignBin.CATEGORY_STOCKOPTION:
                            snapShotData = new OptionSymbolData();
                            snapShotData.CategoryCode = AssetCategory.EquityOption;
                            snapShotData.UnderlyingSymbol = underlyingSymbol;
                            if (string.IsNullOrWhiteSpace(snapShotData.UnderlyingSymbol)) return;
                            break;

                        case eSignBin.CATEGORY_FUTURE:
                            if (subcategoryCode == eSignBin.SUBCATEGORY_FX)
                            {
                                snapShotData = new FxSymbolData();
                                snapShotData.CategoryCode = AssetCategory.FX;
                                snapShotData.UnderlyingSymbol = underlyingSymbol;
                            }
                            else
                            {
                                snapShotData = new FutureSymbolData();
                                snapShotData.CategoryCode = AssetCategory.Future;
                                snapShotData.UnderlyingSymbol = underlyingSymbol;
                            }
                            break;

                        case eSignBin.CATEGORY_FUTUREOPTION:
                            snapShotData = new OptionSymbolData();
                            snapShotData.CategoryCode = AssetCategory.FutureOption;
                            snapShotData.UnderlyingSymbol = underlyingSymbol;
                            break;

                        case eSignBin.CATEGORY_INDICE:
                            snapShotData = new IndexSymbolData();
                            snapShotData.CategoryCode = AssetCategory.Indices;
                            snapShotData.UnderlyingSymbol = underlyingSymbol;
                            break;

                        default:
                            return;
                    }
                }
                snapShotData.Symbol = symbol;
                bool isSnapshotData = false;
                NativeMethods.LrtMoveFirst(pLrt);
                DateTime expiration = DateTimeConstants.MinValue;
                bool firstLrtProcessed = false;
                while (true)
                {
                    if (firstLrtProcessed)
                        NativeMethods.LrtMoveNext(pLrt);
                    else
                        firstLrtProcessed = true;
                    uint iType = NativeMethods.LrtGetType(pLrt);
                    switch (iType)
                    {
                        case LastRecordType.LRT_TYPE_END:
                            NativeMethods.LrtMoveFirst(pLrt);
                            snapShotData.ExpirationDate = expiration;
                            UpdateLastRecordOnTask(snapShotData, isSnapshotData);
                            return;

                        case LastRecordType.LRT_TYPE_VWAP:
                            snapShotData.VWAP = NativeMethods.LrtGetDouble(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_STATUS:
                            string status = GetLRTString(pLrt);
                            isSnapshotData = (status[0] & 0x02) == 0;
                            break;

                        case LastRecordType.LRT_TYPE_KEY:
                            snapShotData.Symbol = GetLRTString(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_CUSIP:
                            snapShotData.CusipNo = GetLRTString(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_AVERAGEVOLUME:
                            snapShotData.AverageVolume20Day = NativeMethods.LrtGetDouble(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_PORC:
                            char putorcall = Convert.ToChar(NativeMethods.LrtGetByte(pLrt));
                            if (putorcall == 'P')
                            {
                                ((OptionSymbolData)snapShotData).PutOrCall = OptionType.PUT;
                            }
                            else
                            {
                                ((OptionSymbolData)snapShotData).PutOrCall = OptionType.CALL;
                            }
                            break;

                        case LastRecordType.LRT_TYPE_OPENINTEREST:
                            snapShotData.OpenInterest = NativeMethods.LrtGetDouble(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_EXPIRATIONDATE:
                            {
                                string Date = GetLRTString(pLrt);
                                string strExpirationMonthYear = string.Empty;
                                string strExpirationDate = string.Empty;
                                int intMonthYear = 0;

                                if (!String.IsNullOrEmpty(Date) && Date.Length == 8)
                                {
                                    strExpirationMonthYear = Date.Substring(0, 6);
                                    strExpirationDate = Date.Substring(6, 2);
                                    intMonthYear = Convert.ToInt32(strExpirationMonthYear);
                                    expiration = new DateTime(Convert.ToInt32(strExpirationMonthYear.Substring(0, 4)), Convert.ToInt32(strExpirationMonthYear.Substring(4, 2)), Convert.ToInt32(strExpirationDate)); //,23,59,59);
                                }
                            }
                            break;

                        case LastRecordType.LRT_TYPE_DATE_EXPIRATION:
                            {
                                int year = 1800;
                                int month = 1;
                                int date = 1;
                                string tmpDate1 = string.Empty;
                                tmpDate1 = NativeMethods.LrtGetLong(pLrt).ToString();

                                if (tmpDate1.Length == 8)
                                {
                                    year = Convert.ToInt32(tmpDate1.Substring(4, 4));
                                    month = Convert.ToInt32(tmpDate1.Substring(0, 2));
                                    date = Convert.ToInt32(tmpDate1.Substring(2, 2));
                                }
                                else if (tmpDate1.Length == 7)
                                {
                                    ///Assuming that the data will come in mmddyyyy or mddyyyy but not in mmdyyyy
                                    const int YEARLENGTH = 4;
                                    const int DATELENGTH = 2;
                                    string yearStr = tmpDate1.Substring(tmpDate1.Length - YEARLENGTH, 4);
                                    string dateMonthStr = tmpDate1.Substring(0, tmpDate1.Length - YEARLENGTH);
                                    string monthStr = dateMonthStr.Substring(0, dateMonthStr.Length - DATELENGTH).PadLeft(2, '0');
                                    string dateStr = dateMonthStr.Substring(dateMonthStr.Length - 2, DATELENGTH);

                                    date = Convert.ToInt32(dateStr);
                                    month = Convert.ToInt32(monthStr);
                                    year = Convert.ToInt32(yearStr);
                                }
                                expiration = new DateTime(year, month, date);
                            }
                            break;

                        case LastRecordType.LRT_TYPE_STRIKE:
                            ((OptionSymbolData)snapShotData).StrikePrice = NativeMethods.LrtGetDouble(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_LAST:
                            snapShotData.LastPrice = GetCIDFieldData(pLrt).m_value;
                            break;

                        case LastRecordType.LRT_TYPE_TOTALVOL:
                            snapShotData.TotalVolume = Convert.ToInt64(NativeMethods.LrtGetLong(pLrt));
                            break;

                        case LastRecordType.LRT_TYPE_ACCVOL:
                            snapShotData.TradeVolume = Convert.ToInt64(NativeMethods.LrtGetDouble(pLrt));
                            break;

                        case LastRecordType.LRT_TYPE_BID:
                            {
                                cidFieldData cfd = GetCIDFieldData(pLrt);
                                snapShotData.Bid = cfd.m_value;
                                snapShotData.BidSize = (long)cfd.m_size;
                                snapShotData.BidExchange = cfd.m_exg;
                                break;
                            }

                        case LastRecordType.LRT_TYPE_ASK:
                            {
                                cidFieldData cfd = GetCIDFieldData(pLrt);
                                snapShotData.Ask = cfd.m_value;
                                snapShotData.AskSize = (long)cfd.m_size;
                                snapShotData.AskExchange = cfd.m_exg;
                                break;
                            }
                        case LastRecordType.LRT_TYPE_HIGH:
                            snapShotData.High = GetCIDFieldData(pLrt).m_value;
                            break;

                        case LastRecordType.LRT_TYPE_LOW:
                            snapShotData.Low = GetCIDFieldData(pLrt).m_value;
                            break;

                        case LastRecordType.LRT_TYPE_OPEN:
                            snapShotData.Open = GetCIDFieldData(pLrt).m_value;
                            break;

                        case LastRecordType.LRT_TYPE_52WEEKHIGH:
                            {
                                cidFieldData cfd = GetCIDFieldData(pLrt);
                                snapShotData.High52W = cfd.m_value;
                                break;
                            }
                        case LastRecordType.LRT_TYPE_52WEEKLOW:
                            {
                                cidFieldData cfd = GetCIDFieldData(pLrt);
                                snapShotData.Low52W = cfd.m_value;
                                break;
                            }
                        case LastRecordType.LRT_TYPE_SHARES:
                            snapShotData.SharesOutstanding = NativeMethods.LrtGetLong(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_PREV:
                            snapShotData.Previous = GetCIDFieldData(pLrt).m_value;
                            break;

                        case LastRecordType.LRT_TYPE_COMPANY:
                            snapShotData.FullCompanyName = GetLRTString(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_OPTION_TYPE:
                            string tmp1 = GetLRTString(pLrt);
                            ((OptionSymbolData)snapShotData).CFICode = tmp1;
                            char[] ch = tmp1.ToCharArray(1, 1);
                            if (ch[0] == 'P')
                            {
                                ((OptionSymbolData)snapShotData).PutOrCall = OptionType.PUT;
                            }
                            else
                            {
                                ((OptionSymbolData)snapShotData).PutOrCall = OptionType.CALL;
                            }
                            break;

                        case LastRecordType.LRT_TYPE_SYMBOL_OSI:
                            ((OptionSymbolData)snapShotData).OSIOptionSymbol = GetLRTString(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_SYMBOL_IDCO:
                            ((OptionSymbolData)snapShotData).IDCOOptionSymbol = GetLRTString(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_STOCKOPTION_SHORTSYMBOL:
                            ((OptionSymbolData)snapShotData).OpraSymbol = GetLRTString(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_DATETIMEUPDATE:
                            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            dtDateTime = dtDateTime.AddSeconds(NativeMethods.LrtGetLong(pLrt));
                            snapShotData.UpdateTime = dtDateTime;
                            break;

                        case LastRecordType.LRT_TYPE_UPDOWNTICKS:
                            string lastFourUpDownTicks = string.Empty;
                            lastFourUpDownTicks = GetLRTString(pLrt);
                            snapShotData.LastTick = (lastFourUpDownTicks.Length == 4) ? lastFourUpDownTicks.Substring(3, 1) : "";
                            break;

                        case LastRecordType.LRT_TYPE_BETA:
                            snapShotData.Beta_5yrMonthly = NativeMethods.LrtGetDouble(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_CHANGE:
                            snapShotData.Change = NativeMethods.LrtGetDouble(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_VOL10DAVG:
                            snapShotData.Volume10DAvg = NativeMethods.LrtGetDouble(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_DIVYIELD:
                            //Reverted back to divide by 100 as eSignal is giving it in %
                            //float pctDivYield = Convert.ToSingle(NativeMethods.LrtGetDouble(pLrt));
                            //float absoluteDivYieldValue = (float)((pctDivYield * 1.0) / 100);
                            //snapShotlevel1Data.DividendYield = absoluteDivYieldValue;

                            //You do not need to divide the results by 100.  You can just use the value from NativeMethods.LrtGetDouble(pLrt).
                            //For example, if you try it with IDC, you will get back 2.3606.  This is in percentage (multiplied by 100).
                            //(i.e. ~$0.20 per share x 4 quarters = $0.80 (value from LRT_TYPE_DIVANN); $0.80 / today's closing price is ~ 2.4%).

                            //Here Dividend Yield is coming in %. If we need to input it in decimals then respective modules will take care of it.
                            snapShotData.DividendYield = Convert.ToSingle(NativeMethods.LrtGetDouble(pLrt));
                            break;

                        case LastRecordType.LRT_TYPE_DIVIDEND:
                            snapShotData.Dividend = Convert.ToSingle(NativeMethods.LrtGetDouble(pLrt));
                            break;

                        case LastRecordType.LRT_TYPE_XDIVDATE:
                            DateTime xDivDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            xDivDate = xDivDate.AddSeconds(NativeMethods.LrtGetLong(pLrt));
                            snapShotData.XDividendDate = xDivDate;
                            break;

                        case LastRecordType.LRT_TYPE_DIVINTERVAL:
                            snapShotData.DividendInterval = NativeMethods.LrtGetLong(pLrt);
                            break;

                        case LastRecordType.LRT_TYPE_DIV2AMT:
                            snapShotData.DividendAmtRate = Convert.ToSingle(NativeMethods.LrtGetDouble(pLrt));
                            break;

                        case LastRecordType.LRT_TYPE_DIVANN:
                            snapShotData.AnnualDividend = Convert.ToDouble(NativeMethods.LrtGetDouble(pLrt));
                            break;

                        case LastRecordType.LRT_TYPE_TURNAROUND:
                            _turnAroundId = NativeMethods.LrtGetLong(pLrt).ToString();
                            if ((!_dictOfOptionChain.ContainsKey(_turnAroundId)) && (_dictOfOptionChain.Count == 0))
                            {
                                List<OptionStaticData> listOfOptionData = new List<OptionStaticData>();
                                _dictOfOptionChain.Add(_turnAroundId, listOfOptionData);
                            }
                            break;

                        case LastRecordType.LRT_TYPE_LIST_BUNDLE:
                            List<string> strList = GetStringList(pLrt);

                            foreach (string str in strList)
                            {
                                OptionStaticData opdata = CreateOptionStaticDataObjFromString(str);
                                if (opdata.Symbol != string.Empty)
                                {
                                    _dictOfOptionChain[_turnAroundId].Add(opdata);
                                }
                            }
                            break;

                        case LastRecordType.LRT_TYPE_CURRENCYCODEASCII:
                            {
                                snapShotData.CurencyCode = GetLRTString(pLrt);
                            }
                            break;

                        case LastRecordType.LRT_TYPE_LISTEXG:
                            snapShotData.ListedExchange = GetLRTString(pLrt);
                            break;
                        //Multiplier in case of Future/Future Options
                        case LastRecordType.LRT_TYPE_POINT_VALUE:
                            snapShotData.Multiplier = NativeMethods.LrtGetLong(pLrt);
                            break;
                        default:
                            break;
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
            finally
            {
                if (pLrt != IntPtr.Zero)
                    NativeMethods.LrtRelease(pLrt);
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Updates the last record on task.
        /// </summary>
        /// <param name="snapShotData">The snap shot data.</param>
        /// <param name="isSnapshotData">if set to <c>true</c> [is snapshot data].</param>
        private void UpdateLastRecordOnTask(SymbolData snapShotData, bool isSnapshotData)
        {
            try
            {
                if (snapShotData != null)
                {
                    MarketDataAdapterExtension.AddToSnapShotSymbolDataCollection(ref snapShotData, MarketDataProvider.Esignal);

                    Data obj = new Data();
                    obj.Info = snapShotData;
                    if (obj.Info != null)
                    {
                        if (isSnapshotData)
                        {
                            if (SnapShotDataResponse != null)
                                SnapShotDataResponse(this, obj);
                        }
                        else if (ContinuousDataResponse != null)
                        {
                            ContinuousDataResponse(this, obj);
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
        /// Event for ESignal Response.
        /// </summary>
        private IntPtr eSignalResponseEvent;

        /// <summary>
        /// Prevents a default instance of the <see cref="ESignalAdapter"/> class from being created.
        /// </summary>
        private ESignalAdapter()
        {
            CreatePublishingProxy();
            LoadEsignalUserLimitMapper();
        }

        /// <summary>
        /// The locker object
        /// </summary>
        private static object _lockerObject = new object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static ESignalAdapter GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock (_lockerObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new ESignalAdapter();
                        }
                    }
                }
                return _instance;
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
                return null;
            }
        }

        /// <summary>
        /// If data manager is not connected then one should call this method
        /// This method will return true if the connection request was successful and false
        /// if the connection request was unsuccessful.
        /// It successful connection request doesn't mean the data manager is connected and
        /// ready to transfer the data to and fro.
        /// </summary>
        /// <returns>
        /// Returns true if connection successful else false
        /// </returns>
        private bool ConnectDataManager()
        {
            try
            {
                // -- Get the username and password for the connection
                if (!PublicConst.ParseINI())
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Could not retrieve user credentials.", LoggingConstants.CATEGORY_WARNING, 1, 1, System.Diagnostics.TraceEventType.Warning);
                    return false;
                }

                SetAppIdentifiers("`Nirvana", 1, 10, 12, 2009, "Nirvana");
                return EstablishDataManagerConnection();
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
        }

        /// <summary>
        /// Establishes the data manager connection.
        /// </summary>
        /// <returns></returns>
        private bool EstablishDataManagerConnection()
        {
            try
            {
                int lret = OpenConnection(PublicConst.g_internetAddress.ToString(), PublicConst.g_username.ToString(), PublicConst.g_password.ToString());

                if (lret == 0)
                    return true;
                else
                    return false;
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
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        private int OpenConnection(string host, string username, string password)
        {
            try
            {
                if (eSignalResponseEvent == IntPtr.Zero)
                {
                    eSignalResponseEvent = NativeMethods.CreateEvent(IntPtr.Zero, false, true, "eSignalResponseEvent");
                    NativeMethods.SetEvent(eSignalResponseEvent);
                    FiredEventHandler();
                }

                if (DBConnection != IntPtr.Zero)
                {
                    NativeMethods.DbcCloseConnection(DBConnection);
                    DBConnection = IntPtr.Zero;
                    dataManager_Disconnected();
                }
                return NativeMethods.DbcOpenConnection(host, username, password, (int)DBCAPI.DBCAPI_DATA_MANAGER, ref DBConnection, eSignalResponseEvent, IntPtr.Zero, DBCAPI.WM_USER_DATA_MANAGER, DBCAPI.DBCAPI_FLAG_REMOTEDM | DBCAPI.DBCAPI_FLAG_WINROSMT);
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
            return -1;
        }

        /// <summary>
        /// Sets the app identifiers.
        /// </summary>
        /// <param name="ProgramName">Name of the program.</param>
        /// <param name="build">The build.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="year">The year.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private int SetAppIdentifiers(string ProgramName, ushort build, int month, int day, ushort year, string type)
        {
            try
            {
                dbc_application_version AppVersion;
                dbc_api_version2 ApiVersion = new dbc_api_version2();
                AppVersion = new dbc_application_version { szProgramName = ProgramName, wBuildNumber = build };

                AppVersion.sDate.nMonth = Convert.ToByte(month);
                AppVersion.sDate.nDay = Convert.ToByte(day);
                AppVersion.sDate.nYear = year;
                AppVersion.szMisc = type;

                return NativeMethods.DbcInitializeEx2(ref AppVersion, ref ApiVersion);
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
                return -1;
            }
        }

        /// <summary>
        /// Datas the manager update status.
        /// </summary>
        /// <param name="sData">The s data.</param>
        /// <param name="nData">The n data.</param>
        private void dataManager_UpdateStatus(IntPtr sData, uint nData)
        {
            try
            {
                DM_MESSAGE_EX dmMessage = (DM_MESSAGE_EX)Marshal.PtrToStructure(sData, typeof(DM_MESSAGE_EX));
                int status = dmMessage.iStatusType;
                string s1 = dmMessage.szText1;
                string s2 = dmMessage.szText2;
                string message = string.Empty;
                switch (status)
                {
                    case (int)DBCAPI.DMMessageStatusTypes.eSTATUS_NOT_AUTHORIZED:
                        message = "This account is not authorized to use the Level 1 service.";
                        break;

                    case (int)DBCAPI.DMMessageStatusTypes.eSTATUS_NO_DATA:
                        if (s2 == "Not Ent.")
                            message = "This account is not authorized to receive data for the requested symbol: ";
                        else
                            message = "The Level 1 service does not have any data available for the requested symbol: ";
                        break;

                    case (int)DBCAPI.DMMessageStatusTypes.eSTATUS_NOT_AUTHORIZED_FOR_SYMBOL:
                        message = "This account is not authorized to receive data for the requested symbol: ";
                        break;

                    case (int)DBCAPI.DMMessageStatusTypes.eSTATUS_UNKNOWN:
                        message = "An error has occurred, but the source of the error is not known.";
                        break;

                    case (int)DBCAPI.DMMessageStatusTypes.eSTATUS_END_OF_LIST:
                        if (OptionChainResponse != null)
                        {
                            Dictionary<string, List<OptionStaticData>> OptionChain = new Dictionary<string, List<OptionStaticData>>();
                            OptionChain = DeepCopyHelper.Clone<Dictionary<string, List<OptionStaticData>>>(_dictOfOptionChain);
                            _dictOfOptionChain.Clear();
                            _turnAroundId = null;
                            if (OptionChain.Count > 0)
                            {
                                //OptionChainResponse(this, new EventArgs<Dictionary<string, List<OptionStaticData>>>(OptionChain));
                                lock (_dictOptionChainRequests)
                                {
                                    if (_dictOptionChainRequests.ContainsKey(s1))
                                    {
                                        _dictOptionChainRequests.Remove(s1);
                                    }
                                }
                            }
                            else
                            {
                                lock (_dictOptionChainRequests)
                                {
                                    if (_dictOptionChainRequests.ContainsKey(s1))
                                    {
                                        List<string> listReqIDs = _dictOptionChainRequests[s1];
                                        foreach (string id in listReqIDs)
                                        {
                                            OptionChain.Add(id, new List<OptionStaticData>());
                                        }
                                        //OptionChainResponse(this, new EventArgs<Dictionary<string, List<OptionStaticData>>>(OptionChain));
                                        _dictOptionChainRequests.Remove(s1);
                                    }
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
                if (s2 == "NO DATA" || s2 == "NO PSWD")
                {
                    dataManager_Disconnected();
                }
                if (s1 == "Good")
                {
                    dataManager_Connected();
                }
                if (!string.IsNullOrEmpty(message) && _enableMarketDataLogging)
                {
                    if (Prana.BusinessObjects.UserSettingConstants.IsDebugModeEnabled)
                    {
                        Logger.LoggerWrite(message + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        InformationReporter.GetInstance.Write(message + " Time : " + DateTime.UtcNow);
                    }
                    else
                    {
                        Logger.LoggerWrite(message + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
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
        /// Datas the manager disconnected.
        /// </summary>
        private void dataManager_Disconnected()
        {
            try
            {
                if (_connectionStatus)
                {
                    _connectionStatus = false;
                    if (Disconnected != null)
                    {
                        Disconnected(this, (new EventArgs<bool>(false)));
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
        /// Datas the manager connected.
        /// </summary>
        private void dataManager_Connected()
        {
            try
            {
                if (!_connectionStatus)
                {
                    _connectionStatus = true;
                    string dataTypes = LastRecordType.TYPE_THELAST.ToString() + "," +
                                       LastRecordType.LRT_TYPE_TOPIC.ToString() + "," +
                                       eSignBin.ROSCOMMAND_DEFAULT.ToString() + "," +
                                       LastRecordType.LRT_TYPE_TOPICFMT.ToString() + "," +
                                       "200726";

                    long lRet = NativeMethods.DbcAddDMSymbol(DBConnection, dataTypes, (int)DBCAPI.RequestType.eREQUEST);

                    if (Connected != null)
                    {
                        Connected(this, (new EventArgs<bool>(true)));
                    }
                    Logger.LoggerWrite("eSignal connected with userName : " + PublicConst.g_username + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
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

        private static bool OpenEvent(out IntPtr evt)
        {
            uint unEventPermissions = 2031619;
            // Same as EVENT_ALL_ACCESS value in the Win32 realm
            evt = NativeMethods.OpenEvent(unEventPermissions, false, "eSignalResponseEvent");
            if (evt == IntPtr.Zero)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Fire the ESignal Response event.
        /// </summary>
        private void FiredEventHandler()
        {
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    IntPtr evt;
                    if (OpenEvent(out evt))
                    {
                        AutoResetEvent arEvt = new AutoResetEvent(false);
                        arEvt.SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle(evt, true);
                        WaitHandle[] waitHandles = new WaitHandle[] { arEvt };
                        while (true)
                        {
                            bool waitResult = WaitHandle.WaitAll(waitHandles, 500, false);
                            if (waitResult)
                            {
                                OnEsignalResponse();
                            }
                        }
                    }
                });
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

        /// <summary>
        /// Called when [esignal response].
        /// </summary>
        [HandleProcessCorruptedStateExceptions]
        private void OnEsignalResponse()
        {
            IntPtr szKey = Marshal.AllocHGlobal(256);//char  *sKey = new char[nKey];				// Allocates memory for holding the key associated with the returned message
            uint nAllocatedData = 16 * 1024;	// Used to set the initial memory space for the data received to 16kb
            uint nData = nAllocatedData;		// The size of the data packet associated with the returned message; used to reallocate memory if needed
            IntPtr sData = Marshal.AllocHGlobal(Convert.ToInt32(nData));//char *sData = new char[nData];				// Allocates memory for holding the data packet associated with the returned message
            try
            {
                int iType = 0;								// Will hold the type of message returned from the API
                int iStatus = 0;							// Will hold the message status
                uint nKey = 256;					// Sets the length of key (ie instrument, or symbol) to 256 characters
                bool exitWorker = false;

                while (!exitWorker)
                {
                    int i;
                    i = NativeMethods.DbcGetMessageMx(DBConnection, ref iType, ref iStatus, szKey, ref nKey, sData, ref nData, true);
                    if (i != DBCAPI.DBCAPI_SUCCESS)
                    {
                        if (i == DBCAPI.DBCAPI_ERROR_WRONG_SIZE)
                        {
                            Marshal.FreeHGlobal(sData);
                            sData = Marshal.AllocHGlobal(Convert.ToInt32(nData));

                            if (sData == null)
                                break;

                            nAllocatedData = nData;
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }

                    switch (iType)
                    {
                        case DBCAPI.DBCAPI_NOTIFY_CONNECT:
                            switch (iStatus)
                            {
                                case DBCAPI.DBCAPI_ERROR_WRONG_USERNAMEPASSWORD:
                                    dataManager_Disconnected();
                                    RetryConnectionAsyncUsingAwait();
                                    exitWorker = true;
                                    break;

                                case DBCAPI.DBCAPI_SUCCESS:
                                    dataManager_Connected();
                                    break;

                                default:
                                    dataManager_Disconnected();
                                    exitWorker = true;
                                    break;
                            }
                            break;

                        case DBCAPI.DBCAPI_NOTIFY_DISCONNECT:
                            exitWorker = true;
                            dataManager_Disconnected();
                            if (iStatus.Equals(DBCAPI.DBCAPI_ERROR_ADDRESS_CHANGE))
                                RetryConnectionAsyncUsingAwait();
                            break;

                        case DBCAPI.DBCAPI_DM_LRT:
                            dataManager_UpdateLastRecord(sData, nData);
                            break;

                        case DBCAPI.DBCAPI_DM_MESSAGE:
                            dataManager_UpdateStatus(sData, nData);
                            break;

                        default:
                            break;
                    }
                    iType = 0;
                    iStatus = 0;
                    Marshal.FreeHGlobal(sData);
                    nData = nAllocatedData;
                    sData = Marshal.AllocHGlobal(Convert.ToInt32(nAllocatedData));
                    Marshal.FreeHGlobal(szKey);
                    nKey = 256;
                    szKey = Marshal.AllocHGlobal(256);
                }
            }
            catch (AccessViolationException)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("ESignalAdapter: AccessViolationException has occurred.", LoggingConstants.CATEGORY_ERROR, 1, 1, TraceEventType.Error);
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
            finally
            {
                Marshal.FreeHGlobal(szKey);
                Marshal.FreeHGlobal(sData);
            }
        }

        /// <summary>
        /// Retries the connection asynchronous using await.
        /// </summary>
        /// <param name="facno">The facno.</param>
        private async void RetryConnectionAsyncUsingAwait()
        {
            await System.Threading.Tasks.Task.Delay(_connectionRetryDelay);
            EstablishDataManagerConnection();
        }

        /// <summary>
        /// Gets the LRT string.
        /// </summary>
        /// <param name="pLrt">The p LRT.</param>
        /// <returns></returns>
        private string GetLRTString(IntPtr pLrt)
        {
            int size = 2048;
            IntPtr szKey = Marshal.AllocHGlobal(size + 1);
            string returnValue = string.Empty;
            try
            {
                NativeMethods.LrtGetString(pLrt, szKey, ref size);
                if (size > 0)
                {
                    returnValue = Marshal.PtrToStringAnsi(szKey);
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
                returnValue = string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(szKey);
            }
            return returnValue;
        }

        /// <summary>
        /// Gets the cid field data.
        /// </summary>
        /// <param name="pLrt">The p LRT.</param>
        /// <returns></returns>
        private cidFieldData GetCIDFieldData(IntPtr pLrt)
        {
            IntPtr pf = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(cidFieldData)));
            try
            {
                NativeMethods.LrtGetFieldCid(pLrt, pf);
                return (cidFieldData)Marshal.PtrToStructure(pf, typeof(cidFieldData));
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
                return default(cidFieldData);
            }
            finally
            {
                Marshal.FreeHGlobal(pf);
            }
        }

        /// <summary>
        /// Gets the string list.
        /// </summary>
        /// <param name="pLrt">The p LRT.</param>
        /// <returns></returns>
        private List<string> GetStringList(IntPtr pLrt)
        {
            uint icount = 100000;
            IntPtr buf = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * (int)icount);
            List<string> strList = new List<string>();
            try
            {
                buf = NativeMethods.LrtGetStringArray(pLrt, buf, ref icount);
                IntPtr[] destination = new IntPtr[icount];
                Marshal.Copy(buf, destination, 0, (int)icount);
                foreach (IntPtr poi in destination)
                {
                    if (poi.Equals(IntPtr.Zero))
                        break;
                    string str = Marshal.PtrToStringAnsi(poi);
                    strList.Add(str);
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
            finally
            {
                Marshal.FreeHGlobal(buf);
            }
            return strList;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _proxyPublishing.Dispose();

            if (disposing)
            {
                _instance.Dispose();
            }
        }

        ~ESignalAdapter()
        {
            Dispose(false);
        }
        #endregion

        /// <summary>
        /// Loads the esignal user limit mapper.
        /// </summary>
        private void LoadEsignalUserLimitMapper()
        {
            try
            {
                _eSignalUserLimitMapper = new Dictionary<int, string>();
                NameValueCollection esignalSymbolLimits = ConfigurationHelper.Instance.GetSectionBySectionName(ConfigurationHelper.SECTION_EsignalSymbolLimits);
                foreach (string a in esignalSymbolLimits)
                {
                    int key = int.MinValue;
                    if (Int32.TryParse(a, out key))
                        _eSignalUserLimitMapper[key] = esignalSymbolLimits[a];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Calculates the user advise limit.
        /// </summary>
        /// <param name="symbolLimitBits">The symbol limit bits.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void CalculateUserAdviseLimit(HashSet<string> s)
        {
            try
            {
                int symbolLimitComp = 0;
                if (s.Contains("SC1"))
                {
                    symbolLimitComp += eSignBin.SC1BITVALUE;
                }
                if (s.Contains("SC2"))
                {
                    symbolLimitComp += eSignBin.SC2BITVALUE;
                }
                if (s.Contains("SC3"))
                {
                    symbolLimitComp += eSignBin.SC3BITVALUE;
                }
                if (s.Contains("SC4"))
                {
                    symbolLimitComp += eSignBin.SC4BITVALUE;
                }
                if (symbolLimitComp > 0 && _eSignalUserLimitMapper != null && _eSignalUserLimitMapper.ContainsKey(symbolLimitComp))
                {
                    PublishPreparation(Topics.Topic_DMSymbolLimitServicesResponse, _eSignalUserLimitMapper[symbolLimitComp]);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the OnServicesResponse event of the _eSignalManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="resp">The <see cref="EventArgs{List{System.String}}"/> instance containing the event data.</param>
        private void OnServicesResponse(List<string> resp)
        {
            try
            {
                HashSet<string> symbolLimitBits = new HashSet<string>();
                List<DMServiceData> services = new List<DMServiceData>();
                foreach (string service in resp)
                {
                    string[] fullName = service.Split(',');
                    if (fullName.Length == 2)
                    {
                        services.Add(new DMServiceData() { ShortName = fullName[0], Longname = fullName[1] });
                        if (fullName[1].StartsWith("Symbol Count "))
                        {
                            symbolLimitBits.Add(fullName[0]);
                        }
                    }
                }

                if (symbolLimitBits.Count > 0)
                {
                    CalculateUserAdviseLimit(symbolLimitBits);
                }

                if (services.Count > 0)
                {
                    PublishPreparation(Topics.Topic_DMServicesResponse, services);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get Live Data Directly From Feed
        /// </summary>
        /// <returns></returns>
        public System.Threading.Tasks.Task<object> GetLiveDataDirectlyFromFeed()
        {
            return null;
        }

        public Dictionary<string, string> GetUserInformation()
        {
            throw new NotImplementedException();
        }

        public List<Dictionary<string, string>> GetUserPermissionsInformation()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, int> GetSubscriptionInformation()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetTickersLastStatusCode()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set Debug Enable Disable
        /// </summary>
        /// <param name="isDebugEnable"></param>
        /// <param name="pctTolerance"></param>
        public void SetDebugEnableDisable(bool isDebugEnable, double pctTolerance)
        {

        }

        /// <summary>
        /// Update Security Details
        /// </summary>
        /// <param name="secMasterList"></param>
        public void UpdateSecurityDetails(BusinessObjects.SecurityMasterBusinessObjects.SecMasterbaseList secMasterList)
        {

        }
    }
}