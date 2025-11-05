using FactSet.Datafeed;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using Prana.MarketDataAdapter.Common;
using Prana.Utilities.StringUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.FactSetAdapter
{
    public class FactSetManager : ILiveFeedAdapter, IDisposable
    {
        private RTConsumer _rtConsumer;
        public RTConsumer RTConsumer
        {
            get { return _rtConsumer; }
            set { _rtConsumer = value; }
        }

        private RTConsumer _rtConsumerSupport;
        public RTConsumer RTConsumerSupport
        {
            get { return _rtConsumerSupport; }
            set { _rtConsumerSupport = value; }
        }

        private Dictionary<string, RTConsumer.RTSubscription> _dictSubscriptions = new Dictionary<string, RTConsumer.RTSubscription>();
        private Dictionary<string, FactSetConstants.AccessType> _dictRealTimeEntitlement = new Dictionary<string, FactSetConstants.AccessType>();
        private int _configMaxRetryCount = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("MaxFactSetSymbolRetryCount"));

        private const string encryptionKey = @"sblw-3hn8-sqoy19";
        private string _clientConnectionUsername = string.Empty;
        private string _clientConnectionPassword = string.Empty;
        private string _clientConnectionHost = string.Empty;
        private string _clientConnectionPort = string.Empty;

        private string _supportConnectionUsername = string.Empty;
        private string _supportConnectionPassword = string.Empty;

        private string _configFactSetDataService = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_FactSetSettings, "FactSetDataService");
        private string _configFactSetOptionChainService = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_FactSetSettings, "FactSetOptionChainService");
        private bool _configEnableMarketDataSimulationForAutomation = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableMarketDataSimulationForAutomation"]);

        private const string _delayedSuffix = ":D";

        private static object _lockerObject = new object();

        private Dictionary<string, string> _dictOCCAndFactSetMapping = new Dictionary<string, string>();
        private Dictionary<string, string> _dictCUSIPAndFactSetMapping = new Dictionary<string, string>();

        private Dictionary<string, OptionChainFilter> _dictOptionChainFilters = new Dictionary<string, OptionChainFilter>();

        private Dictionary<string, List<OptionStaticData>> _dictOptionStaticData = new Dictionary<string, List<OptionStaticData>>();

        private Dictionary<string, int> _dictSymbolSnapshotRetryCount = new Dictionary<string, int>();
        private Dictionary<string, SymbolData> _dictSymbolDefaultResponse = new Dictionary<string, SymbolData>();
        private readonly Dictionary<string, object> _symbolLocks = new Dictionary<string, object>();
        private readonly object _symbolLocksGlobalLock = new object();

        private static FactSetManager _instance = null;
        public static FactSetManager GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock (_lockerObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new FactSetManager();
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

        #region Constructor
        private FactSetManager()
        {
            LoadCredentials();
            MarketDataAdapterExtension.CreateSecMasterServicesProxy();
        }
        #endregion

        #region Private Methods
        private void ClientConnectionHandler(object sender, ConnectCompletedEventArgs e)
        {
            try
            {
                //_rtConsumer = sender as RTConsumer;

                if (e.Cancelled)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("FactSet connection was cancelled: {0}", e.CtrlType), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);

                    ConnectionStatus(false);
                }
                else if (e.Error != null)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Error occurred while establishing a connection with FactSet. Error: {0}", e.Error.Message), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);

                    ConnectionStatus(false);
                }
                else if (e.CtrlType == FactSet.Datafeed.ControlType.DISCONNECTED)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Error occurred while establishing a connection with FactSet: {0}, Error: {1}, ErrorDescription: {2}", e.CtrlType, e.CntrlMsg.Error, e.CntrlMsg.ErrorDescription), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);

                    ConnectionStatus(false);
                }
                else
                {
                    if (e.IsConnected)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Connected to FactSet: {0}, {1}, {2}", _clientConnectionUsername, _rtConsumer.ConnectedToHost, e.CtrlType), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);

                        StringBuilder sb = new StringBuilder();
                        foreach (var svc in _rtConsumer.Services)
                        {
                            sb.Append(svc + "\t");
                        }
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Connected Services: {0}", sb), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);

                        ConnectionStatus(true);
                    }
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Disconnected from FactSet while making connection: {0}, {1}", e.CtrlType, e.CntrlMsg.ErrorDescription), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);

                        ConnectionStatus(false);
                    }
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

        private void SupportConnectionHandler(object sender, ConnectCompletedEventArgs e)
        {
            try
            {
                //_rtConsumerSupport = sender as RTConsumer;

                if (e.Cancelled)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Nirvana Support: FactSet connection was cancelled: {0}", e.CtrlType), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);

                    //ConnectionStatus(false);
                }
                else if (e.Error != null)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Nirvana Support: Error occurred while establishing a connection with FactSet. Error: {0}", e.Error.Message), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);

                    //ConnectionStatus(false);
                }
                else if (e.CtrlType == FactSet.Datafeed.ControlType.DISCONNECTED)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Nirvana Support: Error occurred while establishing a connection with FactSet: {0}, Error: {1}, ErrorDescription: {2}", e.CtrlType, e.CntrlMsg.Error, e.CntrlMsg.ErrorDescription), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);

                    //ConnectionStatus(false);
                }
                else
                {
                    if (e.IsConnected)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Nirvana Support: Connected to FactSet: {0}, {1}, {2}", _supportConnectionUsername, _rtConsumerSupport.ConnectedToHost, e.CtrlType), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);

                        StringBuilder sb = new StringBuilder();
                        foreach (var svc in _rtConsumerSupport.Services)
                        {
                            sb.Append(svc + "\t");
                        }
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Nirvana Support: Connected Services: {0}", sb), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);

                        //ConnectionStatus(true);
                    }
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Nirvana Support: Disconnected from FactSet while making connection: {0}, {1}", e.CtrlType, e.CntrlMsg.ErrorDescription), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);

                        //ConnectionStatus(false);
                    }
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

        private void ClientDispatchCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LogAndDisplayOnInformationReporter.GetInstance.Write("FactSet DispatchCompleted Error: " + e.Error.Message, LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
                Disconnect();
            }
            else
            {
                LogAndDisplayOnInformationReporter.GetInstance.Write("FactSet Dispatch completed", LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
            }
        }

        private void SupportDispatchCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LogAndDisplayOnInformationReporter.GetInstance.Write("Nirvana Support: FactSet DispatchCompleted Error: " + e.Error.Message, LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
                Disconnect();
            }
            else
            {
                LogAndDisplayOnInformationReporter.GetInstance.Write("Nirvana Support: FactSet Dispatch completed", LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
            }
        }

        private void SecurityResponseHandler(RTConsumer.RTSubscription rtSubscription, RTMessage rtMessage)
        {
            try
            {
                if (rtMessage.IsClosed || (rtMessage.IsComplete && rtSubscription.IsSnapshot))
                {
                    try
                    {
                        _rtConsumer.Cancel(rtSubscription);
                    }
                    catch (Exception ex)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write("Error while cancelling subscription. rtSubscription: " + rtSubscription.ToString() + "\n rtMessage: " + rtMessage.ToString() + "\n Exception: " + ex.Message + "\n StackTrace: " + ex.StackTrace, LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Error);
                    }
                }

                if (rtMessage.IsError)
                {
                    //No use, manual button given on UI
                    //if (rtMessage.Error == ErrorCode.NotFound)
                    //{
                    //    MarketDataAdapterExtension.RemoveFactSetSymbolInformation(marketDataSymbolResponse.TickerSymbol);
                    //}
                    if (rtMessage.Error == ErrorCode.Access)
                    {
                        bool isRetry = false;

                        lock (_dictRealTimeEntitlement)
                        {
                            if (_dictRealTimeEntitlement.ContainsKey(rtMessage.Key.Replace(_delayedSuffix, string.Empty)) && _dictRealTimeEntitlement[rtMessage.Key.Replace(_delayedSuffix, string.Empty)] == FactSetConstants.AccessType.Realtime)
                            {
                                _dictRealTimeEntitlement[rtMessage.Key.Replace(_delayedSuffix, string.Empty)] = FactSetConstants.AccessType.Delayed;
                                isRetry = true;
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Error in FactSet response so switching to AccessType.Delayed from AccessType.Realtime: FactSetSymbol: {0}, IsSnapshotData: {1}, ErrorCode: {2}, ErrorDescription: {3}", rtMessage.Key, rtSubscription.IsSnapshot, rtMessage.Error, rtMessage.ErrorDescription), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Warning);
                            }
                            else
                            {
                                if (_dictRealTimeEntitlement.ContainsKey(rtMessage.Key.Replace(_delayedSuffix, string.Empty)))
                                {
                                    _dictRealTimeEntitlement[rtMessage.Key.Replace(_delayedSuffix, string.Empty)] = FactSetConstants.AccessType.Denied;
                                }

                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Error in FactSet response: FactSetSymbol: {0}, IsSnapshotData: {1}, ErrorCode: {2}, ErrorDescription: {3}", rtMessage.Key, rtSubscription.IsSnapshot, rtMessage.Error, rtMessage.ErrorDescription), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Warning);
                            }
                        }

                        if (!rtMessage.Key.EndsWith(_delayedSuffix))
                        {
                            isRetry = true;
                        }

                        if (isRetry)
                        {
                            if (rtSubscription.IsSnapshot)
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SnapShotData Request for FactSetSymbol: {0}", rtMessage.Key + _delayedSuffix), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SnapShotData Request for FactSetSymbol: {0}", rtMessage.Key), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);

                                RTRequest req = new RTRequest(_configFactSetDataService, rtMessage.Key + _delayedSuffix, true);
                                _rtConsumer.MakeRequest(req, SecurityResponseHandler);
                            }
                            else
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("ContinuousData Request for FactSetSymbol: {0}", rtMessage.Key + _delayedSuffix), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SnapShotData Request for FactSetSymbol: {0}", rtMessage.Key), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);

                                RTRequest req = new RTRequest(_configFactSetDataService, rtMessage.Key + _delayedSuffix);
                                _rtConsumer.MakeRequest(req, SecurityResponseHandler);
                            }
                        }
                    }
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Error in FactSet response: FactSetSymbol: {0}, IsSnapshotData: {1}, ErrorCode: {2}, ErrorDescription: {3}", rtMessage.Key, rtSubscription.IsSnapshot, rtMessage.Error, rtMessage.ErrorDescription), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Warning);
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Error in FactSet response: FactSetSymbol: {0}, IsSnapshotData: {1}, ErrorCode: {2}, ErrorDescription: {3}", rtMessage.Key, rtSubscription.IsSnapshot, rtMessage.Error, rtMessage.ErrorDescription), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Warning);
                    }
                }
                else
                {
                    string keyFactSet = rtMessage.Key.Replace(_delayedSuffix, string.Empty);
                    object symbolLock = GetSymbolLock(keyFactSet);

                    lock (symbolLock)
                    {
                        FillSymbolDataFromRTMessage(rtMessage, rtSubscription.IsSnapshot, rtSubscription);
                    }
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

        private void OptionChainResponseHandler(RTConsumer.RTSubscription rtSubscription, RTMessage rtMessage)
        {
            try
            {
                if (rtMessage.IsClosed || (rtMessage.IsComplete && rtSubscription.IsSnapshot))
                {
                    try
                    {
                        _rtConsumer.Cancel(rtSubscription);
                    }
                    catch (Exception ex)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write("Error while cancelling subscription. rtSubscription: " + rtSubscription.ToString() + "\n rtMessage: " + rtMessage.ToString() + "\n Exception: " + ex.Message + "\n StackTrace: " + ex.StackTrace, LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Error);
                    }
                }

                if (!rtMessage.IsError)
                {
                    MarketDataSymbolResponse underlyingSymbol = null;
                    OptionStaticData optionStaticData = new OptionStaticData()
                    {
                        FactSetSymbol = rtMessage.Key.Replace(_delayedSuffix, string.Empty)
                    };

                    var fld = rtMessage.GetField(RTFieldId.UNDERLYING_SECURITY);
                    if (fld != null && !fld.IsEmpty)
                    {
                        underlyingSymbol = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(fld.Value);
                        if (underlyingSymbol != null)
                        {
                            optionStaticData.UnderlyingSymbol = underlyingSymbol.TickerSymbol;
                        }
                        else
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Underlying symbol not found for {0}, FactSetSymbol: {1}", fld.Value, rtMessage.Key.Replace(_delayedSuffix, string.Empty)), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                            return;
                        }
                    }

                    fld = rtMessage.GetField(RTFieldId.CONTRACT_SIZE);
                    if (fld != null && !fld.IsEmpty) optionStaticData.ContractSize = Convert.ToInt64(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.PUT_CALL);
                    if (fld != null && !fld.IsEmpty)
                    {
                        if (fld.Value.Equals("P"))
                            optionStaticData.PutOrCall = OptionType.PUT;
                        else
                            optionStaticData.PutOrCall = OptionType.CALL;
                    }

                    fld = rtMessage.GetField(RTFieldId.EXPIRATION_DATE);
                    if (fld != null && !fld.IsEmpty) optionStaticData.ExpirationDate = DateTime.ParseExact(fld.Value, "yyyyMMdd", CultureInfo.InvariantCulture);

                    fld = rtMessage.GetField(RTFieldId.STRIKE_PRICE);
                    if (fld != null && !fld.IsEmpty) optionStaticData.StrikePrice = Convert.ToDouble(fld.Value);

                    string bbgSymbol;
                    OptionDetail optionDetail = MarketDataAdapterExtension.GenerateOptionDataFromMarketDataSymbol(underlyingSymbol, optionStaticData, out bbgSymbol);
                    MarketDataSymbolResponse marketDataSymbolResponse = new MarketDataSymbolResponse()
                    {
                        TickerSymbol = optionDetail.Symbol,
                        AUECID = optionDetail.AUECID,
                        FactSetSymbol = rtMessage.Key.Replace(_delayedSuffix, string.Empty)
                    };
                    if (optionDetail.AssetCategory == AssetCategory.Equity || optionDetail.AssetCategory == AssetCategory.Indices)
                        marketDataSymbolResponse.AssetCategory = AssetCategory.EquityOption;
                    else if (optionDetail.AssetCategory == AssetCategory.Future)
                        marketDataSymbolResponse.AssetCategory = AssetCategory.FutureOption;

                    MarketDataAdapterExtension.AddMarketDataForTickerSymbolToCache(optionDetail.Symbol, marketDataSymbolResponse);
                    if (!string.IsNullOrEmpty(optionDetail.Symbol))
                        optionStaticData.Symbol = optionDetail.Symbol;
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Unable to generate ticker symbol for {0} with AUECID: {1} and Underlying Symbol: {2}", marketDataSymbolResponse.FactSetSymbol, marketDataSymbolResponse.AUECID, underlyingSymbol.TickerSymbol), LoggingConstants.CATEGORY_ERROR, 1, 1, TraceEventType.Error);
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Unable to generate ticker symbol for {0} with AUECID: {1} and Underlying Symbol: {2}", marketDataSymbolResponse.FactSetSymbol, marketDataSymbolResponse.AUECID, underlyingSymbol.TickerSymbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                    }

                    if (!string.IsNullOrEmpty(bbgSymbol))
                        optionStaticData.BloombergSymbol = bbgSymbol;
                    int chain_current = 0;
                    fld = rtMessage.GetField(RTFieldId.CHAIN_CURRENT);
                    if (fld != null && !fld.IsEmpty) chain_current = Convert.ToInt32(fld.Value);

                    int chain_total = 0;
                    fld = rtMessage.GetField(RTFieldId.CHAIN_TOTAL);
                    if (fld != null && !fld.IsEmpty) chain_total = Convert.ToInt32(fld.Value);

                    lock (_dictOptionStaticData)
                    {
                        if (_dictOptionStaticData.ContainsKey(optionStaticData.UnderlyingSymbol))
                        {
                            _dictOptionStaticData[optionStaticData.UnderlyingSymbol].Add(optionStaticData);
                        }
                        else
                        {
                            List<OptionStaticData> list = new List<OptionStaticData>();
                            list.Add(optionStaticData);

                            _dictOptionStaticData[optionStaticData.UnderlyingSymbol] = list;
                        }
                    }

                    if (chain_current == chain_total)
                    {
                        if (OptionChainResponse != null)
                        {
                            OptionChainFilter optionChainFilter = null;
                            lock (_dictOptionChainFilters)
                            {
                                if (_dictOptionChainFilters.ContainsKey(optionStaticData.UnderlyingSymbol))
                                {
                                    optionChainFilter = _dictOptionChainFilters[optionStaticData.UnderlyingSymbol];
                                    _dictOptionChainFilters.Remove(optionStaticData.UnderlyingSymbol);
                                }
                            }

                            if (optionChainFilter != null)
                            {
                                lock (_dictOptionStaticData)
                                {
                                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Applying Filters on {0} options of {1}", _dictOptionStaticData[optionStaticData.UnderlyingSymbol].Count, optionStaticData.UnderlyingSymbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);

                                    List<OptionStaticData> filteredOptionChain = ApplyFilterConditions(_dictOptionStaticData[optionStaticData.UnderlyingSymbol], optionChainFilter);
                                    _dictOptionStaticData.Remove(optionStaticData.UnderlyingSymbol);

                                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Filtered {0} options of {1}", filteredOptionChain.Count, optionStaticData.UnderlyingSymbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                                    OptionChainResponse(this, new EventArgs<string, List<OptionStaticData>>(optionStaticData.UnderlyingSymbol, filteredOptionChain));
                                }
                            }
                        }
                    }
                }
                else
                {
                    switch (rtMessage.ErrorDescription)
                    {
                        case "Symbol not found":
                            MarketDataSymbolResponse underlyingSymbol = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(rtMessage.Key);
                            OptionChainResponse(this, new EventArgs<string, List<OptionStaticData>>(underlyingSymbol.TickerSymbol, new List<OptionStaticData>()));
                            LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("OptionChainData Response sent error Symbol: {0} not found, FactSetSymbol: {1}", underlyingSymbol.TickerSymbol, rtMessage.Key), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                            break;
                        default:
                            LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("OptionChainData Response sent error ({0}) for  FactSet Symbol: {1} not found.", rtMessage.ErrorDescription, rtMessage.Key), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                            break;
                    }
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

        private List<OptionStaticData> ApplyFilterConditions(List<OptionStaticData> optionChainData, OptionChainFilter optionChainFilter)
        {
            try
            {
                List<OptionStaticData> filteredOptionChain = new List<OptionStaticData>();
                Dictionary<DateTime, List<OptionStaticData>> expirationDateWiseOptionStaticData = new Dictionary<DateTime, List<OptionStaticData>>();

                if (optionChainFilter.LowerStrike == 0 && optionChainFilter.UpperStrike == 0)
                {
                    optionChainFilter.LowerStrike = (optionChainFilter.UnderlyingSymbolLastTradedPrice - (optionChainFilter.UnderlyingSymbolLastTradedPrice * optionChainFilter.StrikeTolerancePercentage / 100));
                    optionChainFilter.UpperStrike = (optionChainFilter.UnderlyingSymbolLastTradedPrice + (optionChainFilter.UnderlyingSymbolLastTradedPrice * optionChainFilter.StrikeTolerancePercentage / 100));
                }

                foreach (OptionStaticData osd in optionChainData)
                {
                    if ((optionChainFilter.ExpirationDate != new DateTime() && optionChainFilter.ExpirationDate.Month == osd.ExpirationDate.Month && optionChainFilter.ExpirationDate.Year == osd.ExpirationDate.Year)
                        && ((osd.PutOrCall == OptionType.CALL && optionChainFilter.OptionTypeFilter != OptionTypeFilter.PUT) || (osd.PutOrCall == OptionType.PUT && optionChainFilter.OptionTypeFilter != OptionTypeFilter.CALL)))
                    {
                        if (osd.StrikePrice >= optionChainFilter.LowerStrike && osd.StrikePrice <= optionChainFilter.UpperStrike)
                        {
                            if (expirationDateWiseOptionStaticData.ContainsKey(osd.ExpirationDate))
                            {
                                if (expirationDateWiseOptionStaticData[osd.ExpirationDate].FindIndex(o => o.StrikePrice == osd.StrikePrice && o.PutOrCall == osd.PutOrCall) == -1)
                                    (expirationDateWiseOptionStaticData[osd.ExpirationDate] as List<OptionStaticData>).Add(osd);
                            }
                            else
                                expirationDateWiseOptionStaticData.Add(osd.ExpirationDate, new List<OptionStaticData>() { osd });
                        }
                    }
                }

                foreach (KeyValuePair<DateTime, List<OptionStaticData>> mapping in expirationDateWiseOptionStaticData)
                {
                    mapping.Value.Sort((p, n) => p.StrikePrice.CompareTo(n.StrikePrice));

                    if (mapping.Value.Count > (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes))
                    {
                        int maxLowerNumberOfOptions, maxUpperNumberOfOptions;

                        if (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT)
                        {
                            if (optionChainFilter.MaxNumberOfStrikes % 2 == 0)
                                maxLowerNumberOfOptions = maxUpperNumberOfOptions = optionChainFilter.MaxNumberOfStrikes;
                            else
                            {
                                maxLowerNumberOfOptions = optionChainFilter.MaxNumberOfStrikes + 1;
                                maxUpperNumberOfOptions = optionChainFilter.MaxNumberOfStrikes - 1;
                            }
                        }
                        else
                        {
                            if (optionChainFilter.MaxNumberOfStrikes % 2 == 0)
                                maxLowerNumberOfOptions = maxUpperNumberOfOptions = optionChainFilter.MaxNumberOfStrikes / 2;
                            else
                            {
                                maxLowerNumberOfOptions = (optionChainFilter.MaxNumberOfStrikes + 1) / 2;
                                maxUpperNumberOfOptions = (optionChainFilter.MaxNumberOfStrikes - 1) / 2;
                            }
                        }

                        int pivot = mapping.Value.FindIndex(o => o.StrikePrice > optionChainFilter.UnderlyingSymbolLastTradedPrice);
                        int lowerStrikeOptionsCount = mapping.Value.Where(o => o.StrikePrice < optionChainFilter.UnderlyingSymbolLastTradedPrice).Count();
                        int upperStrikeOptionsCount = mapping.Value.Where(o => o.StrikePrice > optionChainFilter.UnderlyingSymbolLastTradedPrice).Count();

                        if (pivot == -1)
                        {
                            if (lowerStrikeOptionsCount == 0)
                            {
                                pivot = 0;
                                if (upperStrikeOptionsCount > (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes))
                                    upperStrikeOptionsCount = (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes);
                            }
                            else if (upperStrikeOptionsCount == 0)
                            {
                                pivot = mapping.Value.Count;
                                if (lowerStrikeOptionsCount > (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes))
                                    lowerStrikeOptionsCount = (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes);
                            }
                        }
                        else if (lowerStrikeOptionsCount > maxLowerNumberOfOptions && upperStrikeOptionsCount < maxUpperNumberOfOptions)
                        {
                            lowerStrikeOptionsCount += maxUpperNumberOfOptions - upperStrikeOptionsCount;

                            if (lowerStrikeOptionsCount + upperStrikeOptionsCount > (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes))
                                lowerStrikeOptionsCount = (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes) - upperStrikeOptionsCount;
                        }
                        else if (lowerStrikeOptionsCount < maxLowerNumberOfOptions && upperStrikeOptionsCount > maxUpperNumberOfOptions)
                        {
                            upperStrikeOptionsCount += maxLowerNumberOfOptions - lowerStrikeOptionsCount;

                            if (lowerStrikeOptionsCount + upperStrikeOptionsCount > (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes))
                                upperStrikeOptionsCount = (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes) - lowerStrikeOptionsCount;
                        }
                        else if (lowerStrikeOptionsCount >= maxLowerNumberOfOptions && upperStrikeOptionsCount >= maxUpperNumberOfOptions)
                        {
                            lowerStrikeOptionsCount = maxLowerNumberOfOptions;
                            upperStrikeOptionsCount = maxUpperNumberOfOptions;
                        }

                        for (int lowerIndex = pivot - 1; lowerIndex >= 0 && lowerStrikeOptionsCount > 0; lowerIndex--, lowerStrikeOptionsCount--)
                            filteredOptionChain.Add(mapping.Value[lowerIndex]);

                        for (int upperIndex = pivot; upperIndex < mapping.Value.Count && upperStrikeOptionsCount > 0; upperIndex++, upperStrikeOptionsCount--)
                            filteredOptionChain.Add(mapping.Value[upperIndex]);
                    }
                    else
                    {
                        filteredOptionChain.AddRange(mapping.Value);
                    }
                }

                return filteredOptionChain;
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

            return null;
        }

        private void ConnectionStatus(bool isConnected)
        {
            if (isConnected)
            {
                if (Connected != null)
                {
                    Connected(this, (new EventArgs<bool>(true)));
                }
            }
            else
            {
                if (Disconnected != null)
                {
                    Disconnected(this, (new EventArgs<bool>(false)));
                }
            }
        }

        private void FillSymbolDataFromRTMessage(RTMessage rtMessage, bool isSnapshotData, RTConsumer.RTSubscription rtSubscription)
        {
            try
            {
                SymbolData snapShotData = MarketDataAdapterExtension.GetSnapShotSymbolData(rtMessage.Key.Replace(_delayedSuffix, string.Empty));

                if (snapShotData == null)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("FactSet security initial response received: {0}, IsSnapshotData: {1}", rtMessage.Key, isSnapshotData), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);

                    FactSetConstants.FactSetSecurityType factSetSecurityType = FactSetConstants.FactSetSecurityType.None;

                    var fld = rtMessage.GetField(RTFieldId.SECURITY_TYPE);
                    if (fld != null && !fld.IsEmpty) factSetSecurityType = (FactSetConstants.FactSetSecurityType)(Convert.ToInt32(fld.Value));

                    switch (factSetSecurityType)
                    {
                        case FactSetConstants.FactSetSecurityType.Cash:
                        case FactSetConstants.FactSetSecurityType.GlobalCashDeposits:
                            snapShotData = new CashSymbolData();
                            snapShotData.CategoryCode = AssetCategory.Cash;
                            break;
                        case FactSetConstants.FactSetSecurityType.USInvestmentTrustsDebt:
                        case FactSetConstants.FactSetSecurityType.USMoneyMarketFunds:
                        case FactSetConstants.FactSetSecurityType.CorporateBonds:
                        case FactSetConstants.FactSetSecurityType.GovernmentTreasuryBondPrices:
                        case FactSetConstants.FactSetSecurityType.MMAMunicipalBonds:
                        case FactSetConstants.FactSetSecurityType.GovernmentTreasuryBondYields:
                        case FactSetConstants.FactSetSecurityType.GovernmentStrips:
                        case FactSetConstants.FactSetSecurityType.USGovernmentAgencyBonds:
                        case FactSetConstants.FactSetSecurityType.USAnnuities:
                        case FactSetConstants.FactSetSecurityType.EquityLinkedSecurities:
                        case FactSetConstants.FactSetSecurityType.MunicipalBonds:
                        case FactSetConstants.FactSetSecurityType.LoanCertificates:
                        case FactSetConstants.FactSetSecurityType.NonUSMortgageBonds:
                        case FactSetConstants.FactSetSecurityType.MunicipalBond:
                        case FactSetConstants.FactSetSecurityType.EODBenchmarkBonds:
                        case FactSetConstants.FactSetSecurityType.FinraTraceBonds:
                        case FactSetConstants.FactSetSecurityType.EvaluatedBonds:
                        case FactSetConstants.FactSetSecurityType.USMortgageBackedSecurities:
                        case FactSetConstants.FactSetSecurityType.SBABackedSecuritiesToBeAnnounced:
                        case FactSetConstants.FactSetSecurityType.SBABackedSecuritiesTradedInPool:
                        case FactSetConstants.FactSetSecurityType.AssetBackedSecurities:
                        case FactSetConstants.FactSetSecurityType.CollateralizedMortgageObligations:
                        case FactSetConstants.FactSetSecurityType.InvestmentCertificates:
                            snapShotData = new FixedIncomeSymbolData();
                            snapShotData.CategoryCode = AssetCategory.FixedIncome;
                            break;
                        case FactSetConstants.FactSetSecurityType.Future:
                        case FactSetConstants.FactSetSecurityType.Metals:
                            snapShotData = new FutureSymbolData();
                            snapShotData.CategoryCode = AssetCategory.Future;

                            fld = rtMessage.GetField(RTFieldId.ORDER_LOT_SIZE);
                            if (fld != null && !fld.IsEmpty) snapShotData.Multiplier = Convert.ToInt64(fld.Value);

                            fld = rtMessage.GetField(RTFieldId.PRIMARY_MARKET);
                            if (fld != null && !fld.IsEmpty) snapShotData.ListedExchange = fld.Value;
                            break;
                        case FactSetConstants.FactSetSecurityType.FutureOption:
                            LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Option Response received for FactSetSymbol: {0}", rtMessage.Key), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);

                            snapShotData = new FutureOptionSymbolData();
                            snapShotData.CategoryCode = AssetCategory.FutureOption;

                            fld = rtMessage.GetField(RTFieldId.UNDERLYING_SECURITY);
                            if (fld != null && !fld.IsEmpty)
                            {
                                MarketDataSymbolResponse fssrFO = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(fld.Value);

                                if (fssrFO != null)
                                    snapShotData.UnderlyingSymbol = fssrFO.TickerSymbol;
                            }

                            if (string.IsNullOrWhiteSpace(snapShotData.UnderlyingSymbol))
                            {
                                fld = rtMessage.GetField(RTFieldId.OPT_ROOT_SYMBOL);
                                if (fld != null && !fld.IsEmpty) snapShotData.UnderlyingSymbol = fld.Value;
                            }

                            if (string.IsNullOrWhiteSpace(snapShotData.UnderlyingSymbol)) return;

                            fld = rtMessage.GetField(RTFieldId.CONTRACT_SIZE);
                            if (fld != null && !fld.IsEmpty) snapShotData.Multiplier = Convert.ToInt64(fld.Value);

                            fld = rtMessage.GetField(RTFieldId.ISO_COUNTRY_EXCHANGE);
                            if (fld != null && !fld.IsEmpty) snapShotData.ListedExchange = fld.Value;
                            break;
                        case FactSetConstants.FactSetSecurityType.FX:
                        case FactSetConstants.FactSetSecurityType.SpotPrice:
                            snapShotData = new FxSymbolData();
                            snapShotData.CategoryCode = AssetCategory.FX;
                            snapShotData.ListedExchange = "FX";

                            fld = rtMessage.GetField(RTFieldId.ORDER_LOT_SIZE);
                            if (fld != null && !fld.IsEmpty) snapShotData.Multiplier = Convert.ToInt64(fld.Value);
                            break;
                        case FactSetConstants.FactSetSecurityType.Option:
                        case FactSetConstants.FactSetSecurityType.USStructuredProducts:
                        case FactSetConstants.FactSetSecurityType.GlobalWarrants:
                            LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Option Response received for FactSetSymbol: {0}", rtMessage.Key), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);

                            FactSetConstants.FactSetOptionType factSetOptionType = FactSetConstants.FactSetOptionType.None;

                            fld = rtMessage.GetField(RTFieldId.OPTION_TYPE);
                            if (fld != null && !fld.IsEmpty) factSetOptionType = (FactSetConstants.FactSetOptionType)(Convert.ToInt32(fld.Value));

                            snapShotData = new OptionSymbolData();
                            snapShotData.CategoryCode = AssetCategory.EquityOption;

                            if (factSetOptionType != FactSetConstants.FactSetOptionType.None)
                            {
                                fld = rtMessage.GetField(RTFieldId.UNDERLYING_SECURITY);
                                if (fld != null && !fld.IsEmpty)
                                {
                                    MarketDataSymbolResponse fssrEO = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(fld.Value);

                                    if (fssrEO != null)
                                        snapShotData.UnderlyingSymbol = fssrEO.TickerSymbol;
                                    else
                                    {
                                        fssrEO = MarketDataAdapterExtension.GetTickerSymbolFromMarketData(new SymbolData()
                                        {
                                            MarketDataProvider = MarketDataProvider.FactSet,
                                            CategoryCode = AssetCategory.Equity,
                                            FactSetSymbol = fld.Value
                                        });

                                        if (fssrEO != null)
                                            snapShotData.UnderlyingSymbol = fssrEO.TickerSymbol;
                                    }
                                }

                                if (string.IsNullOrWhiteSpace(snapShotData.UnderlyingSymbol))
                                {
                                    fld = rtMessage.GetField(RTFieldId.UNDERLYING_SYMBOL);
                                    if (fld != null && !fld.IsEmpty)
                                    {
                                        MarketDataSymbolResponse fssrEO = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(fld.Value);

                                        if (fssrEO != null)
                                            snapShotData.UnderlyingSymbol = fssrEO.TickerSymbol;
                                        else
                                        {
                                            fssrEO = MarketDataAdapterExtension.GetTickerSymbolFromMarketData(new SymbolData()
                                            {
                                                MarketDataProvider = MarketDataProvider.FactSet,
                                                CategoryCode = AssetCategory.Equity,
                                                FactSetSymbol = fld.Value
                                            });

                                            if (fssrEO != null)
                                                snapShotData.UnderlyingSymbol = fssrEO.TickerSymbol;
                                        }
                                    }
                                }

                                if (string.IsNullOrWhiteSpace(snapShotData.UnderlyingSymbol))
                                {
                                    fld = rtMessage.GetField(RTFieldId.OPT_ROOT_SYMBOL);
                                    if (fld != null && !fld.IsEmpty) snapShotData.UnderlyingSymbol = fld.Value;
                                }

                                if (string.IsNullOrWhiteSpace(snapShotData.UnderlyingSymbol)) return;

                                fld = rtMessage.GetField(RTFieldId.CONTRACT_SIZE);
                                if (fld != null && !fld.IsEmpty) snapShotData.Multiplier = Convert.ToInt64(fld.Value);

                                fld = rtMessage.GetField(RTFieldId.ISO_COUNTRY_EXCHANGE);
                                if (fld != null && !fld.IsEmpty) snapShotData.ListedExchange = fld.Value;
                            }
                            else
                            {
                                fld = rtMessage.GetField(RTFieldId.UNDERLYING_SECURITY);
                                if (fld != null && !fld.IsEmpty)
                                {
                                    MarketDataSymbolResponse fssrEO = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(fld.Value);

                                    if (fssrEO != null)
                                        snapShotData.UnderlyingSymbol = fssrEO.TickerSymbol;
                                    else
                                    {
                                        fssrEO = MarketDataAdapterExtension.GetTickerSymbolFromMarketData(new SymbolData()
                                        {
                                            MarketDataProvider = MarketDataProvider.FactSet,
                                            CategoryCode = AssetCategory.Equity,
                                            FactSetSymbol = fld.Value
                                        });

                                        if (fssrEO != null)
                                            snapShotData.UnderlyingSymbol = fssrEO.TickerSymbol;
                                    }
                                }

                                if (string.IsNullOrWhiteSpace(snapShotData.UnderlyingSymbol))
                                {
                                    fld = rtMessage.GetField(RTFieldId.UNDERLYING_SYMBOL);
                                    if (fld != null && !fld.IsEmpty)
                                    {
                                        MarketDataSymbolResponse fssrEO = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(fld.Value);

                                        if (fssrEO != null)
                                            snapShotData.UnderlyingSymbol = fssrEO.TickerSymbol;
                                        else
                                        {
                                            fssrEO = MarketDataAdapterExtension.GetTickerSymbolFromMarketData(new SymbolData()
                                            {
                                                MarketDataProvider = MarketDataProvider.FactSet,
                                                CategoryCode = AssetCategory.Equity,
                                                FactSetSymbol = fld.Value
                                            });

                                            if (fssrEO != null)
                                                snapShotData.UnderlyingSymbol = fssrEO.TickerSymbol;
                                        }
                                    }
                                }

                                if (string.IsNullOrWhiteSpace(snapShotData.UnderlyingSymbol))
                                {
                                    fld = rtMessage.GetField(RTFieldId.OPT_ROOT_SYMBOL);
                                    if (fld != null && !fld.IsEmpty) snapShotData.UnderlyingSymbol = fld.Value;
                                }

                                if (string.IsNullOrWhiteSpace(snapShotData.UnderlyingSymbol)) return;

                                fld = rtMessage.GetField(RTFieldId.CONTRACT_SIZE);
                                if (fld != null && !fld.IsEmpty) snapShotData.Multiplier = Convert.ToInt64(fld.Value);

                                fld = rtMessage.GetField(RTFieldId.ISO_COUNTRY_EXCHANGE);
                                if (fld != null && !fld.IsEmpty) snapShotData.ListedExchange = fld.Value;
                            }
                            break;
                        case FactSetConstants.FactSetSecurityType.Indices:
                        case FactSetConstants.FactSetSecurityType.GlobalMarketStatistics:
                        case FactSetConstants.FactSetSecurityType.GlobalConsumerPriceIndex:
                        case FactSetConstants.FactSetSecurityType.ETFStatistics:
                            snapShotData = new IndexSymbolData();
                            snapShotData.CategoryCode = AssetCategory.Indices;

                            fld = rtMessage.GetField(RTFieldId.ORDER_LOT_SIZE);
                            if (fld != null && !fld.IsEmpty) snapShotData.Multiplier = Convert.ToInt64(fld.Value);

                            fld = rtMessage.GetField(RTFieldId.PRIMARY_MARKET);
                            if (fld != null && !fld.IsEmpty) snapShotData.ListedExchange = fld.Value;
                            break;
                        case FactSetConstants.FactSetSecurityType.IMMForwardRates:
                            snapShotData = new FxForwardContractSymbolData();
                            snapShotData.CategoryCode = AssetCategory.FXForward;
                            break;
                        case FactSetConstants.FactSetSecurityType.FXOptions:
                        case FactSetConstants.FactSetSecurityType.USFlexOptions:
                            LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Option Response received for FactSetSymbol: {0}", rtMessage.Key), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);

                            factSetOptionType = FactSetConstants.FactSetOptionType.None;

                            fld = rtMessage.GetField(RTFieldId.OPTION_TYPE);
                            if (fld != null && !fld.IsEmpty) factSetOptionType = (FactSetConstants.FactSetOptionType)(Convert.ToInt32(fld.Value));

                            if (factSetOptionType != FactSetConstants.FactSetOptionType.None)
                            {
                                snapShotData = new OptionSymbolData();
                                snapShotData.CategoryCode = AssetCategory.FXOption;

                                fld = rtMessage.GetField(RTFieldId.UNDERLYING_SECURITY);
                                if (fld != null && !fld.IsEmpty)
                                {
                                    MarketDataSymbolResponse fssrEO = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(fld.Value);

                                    if (fssrEO != null)
                                        snapShotData.UnderlyingSymbol = fssrEO.TickerSymbol;
                                }

                                if (string.IsNullOrWhiteSpace(snapShotData.UnderlyingSymbol))
                                {
                                    fld = rtMessage.GetField(RTFieldId.OPT_ROOT_SYMBOL);
                                    if (fld != null && !fld.IsEmpty) snapShotData.UnderlyingSymbol = fld.Value;
                                }

                                if (string.IsNullOrWhiteSpace(snapShotData.UnderlyingSymbol)) return;

                                fld = rtMessage.GetField(RTFieldId.CONTRACT_SIZE);
                                if (fld != null && !fld.IsEmpty) snapShotData.Multiplier = Convert.ToInt64(fld.Value);

                                fld = rtMessage.GetField(RTFieldId.ISO_COUNTRY_EXCHANGE);
                                if (fld != null && !fld.IsEmpty) snapShotData.ListedExchange = fld.Value;
                            }
                            break;
                        case FactSetConstants.FactSetSecurityType.AlternativeInvestmentProducts:
                            snapShotData = new EquitySymbolData();
                            snapShotData.CategoryCode = AssetCategory.PrivateEquity;
                            break;
                        case FactSetConstants.FactSetSecurityType.Equity:
                        case FactSetConstants.FactSetSecurityType.USSecurities:
                        case FactSetConstants.FactSetSecurityType.OpenAndClosedEndFunds:
                        case FactSetConstants.FactSetSecurityType.USInvestmentTrusts:
                        case FactSetConstants.FactSetSecurityType.ShortTermGovernmentSecurities:
                        case FactSetConstants.FactSetSecurityType.MediumTermGovernmentSecurities:
                        case FactSetConstants.FactSetSecurityType.LongTermGovernmentSecurities:
                        case FactSetConstants.FactSetSecurityType.GlobalInflationProtectedSecurities:
                        case FactSetConstants.FactSetSecurityType.GlobalMarketMovers:
                        case FactSetConstants.FactSetSecurityType.USOpenEndMutualFunds:
                        case FactSetConstants.FactSetSecurityType.USClosedEndFunds:
                        case FactSetConstants.FactSetSecurityType.ExchangeTradedManagedFunds:
                        case FactSetConstants.FactSetSecurityType.MutualFundsCollectiveInterestTrust:
                        case FactSetConstants.FactSetSecurityType.MutualFundSeparatelyManagedAccount:
                        case FactSetConstants.FactSetSecurityType.MutualFundUnifiedManagedAccount:
                        case FactSetConstants.FactSetSecurityType.MutualFundSeparateAccounts:
                        case FactSetConstants.FactSetSecurityType.USOTC:
                            snapShotData = new EquitySymbolData();
                            snapShotData.CategoryCode = AssetCategory.Equity;

                            fld = rtMessage.GetField(RTFieldId.ORDER_LOT_SIZE);
                            if (fld != null && !fld.IsEmpty) snapShotData.Multiplier = Convert.ToInt64(fld.Value);

                            fld = rtMessage.GetField(RTFieldId.PRIMARY_MARKET);
                            if (fld != null && !fld.IsEmpty) snapShotData.ListedExchange = fld.Value;
                            break;
                        default:
                            lock (_dictSymbolSnapshotRetryCount)
                            {
                                if (!_dictSymbolSnapshotRetryCount.ContainsKey(rtMessage.Key))
                                {
                                    //add new symbol to dictionary to keep the retry count
                                    _dictSymbolSnapshotRetryCount[rtMessage.Key] = 1;
                                }
                                if (_dictSymbolSnapshotRetryCount[rtMessage.Key] <= _configMaxRetryCount)
                                {
                                    //retry for the failed response
                                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Unable to fetch default details for {0} from {1}, sending the {2} snapshot retry attempt", rtMessage.Key, "FactSet", _dictSymbolSnapshotRetryCount[rtMessage.Key]), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);
                                    _dictSymbolSnapshotRetryCount[rtMessage.Key]++;
                                    RTRequest req = new RTRequest(_configFactSetDataService, rtMessage.Key);
                                    _rtConsumer.MakeRequest(req, SecurityResponseHandler);
                                }
                                else
                                {
                                    if (_dictSymbolDefaultResponse.ContainsKey(rtMessage.Key))
                                        snapShotData = _dictSymbolDefaultResponse[rtMessage.Key];
                                    else
                                    {
                                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Unable to identify Asset from FactSet security response. So processed with Equity Asset in system: {0}", rtMessage), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Error);

                                        snapShotData = new EquitySymbolData();
                                        snapShotData.CategoryCode = AssetCategory.Equity;
                                        _dictSymbolDefaultResponse.Add(rtMessage.Key, snapShotData);
                                    }
                                    fld = rtMessage.GetField(RTFieldId.ORDER_LOT_SIZE);
                                    if (fld != null && !fld.IsEmpty) snapShotData.Multiplier = Convert.ToInt64(fld.Value);

                                    fld = rtMessage.GetField(RTFieldId.PRIMARY_MARKET);
                                    if (fld != null && !fld.IsEmpty) snapShotData.ListedExchange = fld.Value;

                                }

                            }
                            break;
                    }
                    // If  security type is option and OPTION_TYPE is null so adding a check to prevent the error
                    if (snapShotData != null)
                        snapShotData.ExpirationDate = DateTimeConstants.MinValue;
                }
                else
                {
                    if (snapShotData.CategoryCode == AssetCategory.FXOption || snapShotData.CategoryCode == AssetCategory.FutureOption || snapShotData.CategoryCode == AssetCategory.EquityOption)
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Continous Option Response received for Symbol: {0}, FactSetSymbol: {1}", snapShotData.Symbol, snapShotData.FactSetSymbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                }

                if (snapShotData != null)
                {
                    var fld = rtMessage.GetField(RTFieldId.VWAP);
                    if (fld != null && !fld.IsEmpty) snapShotData.VWAP = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.CUSIP);
                    if (fld != null && !fld.IsEmpty) snapShotData.CusipNo = fld.Value;

                    fld = rtMessage.GetField(RTFieldId.SEDOL);
                    if (fld != null && !fld.IsEmpty) snapShotData.SedolSymbol = fld.Value;

                    fld = rtMessage.GetField(RTFieldId.ISIN);
                    if (fld != null && !fld.IsEmpty) snapShotData.ISIN = fld.Value;

                    fld = rtMessage.GetField(RTFieldId.AVG_5DAY_VOL);
                    if (fld != null && !fld.IsEmpty) snapShotData.Volume10DAvg = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.AVG_30DAY_VOL);
                    if (fld != null && !fld.IsEmpty) snapShotData.AverageVolume20Day = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.PUT_CALL);
                    if (fld != null && !fld.IsEmpty)
                    {
                        if (fld.Value.Equals("P"))
                            snapShotData.PutOrCall = OptionType.PUT;
                        else
                            snapShotData.PutOrCall = OptionType.CALL;
                    }

                    fld = rtMessage.GetField(RTFieldId.OPEN_INTEREST);
                    if (fld != null && !fld.IsEmpty) snapShotData.OpenInterest = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.EXPIRATION_DATE);
                    if (fld != null && !fld.IsEmpty) snapShotData.ExpirationDate = DateTime.ParseExact(fld.Value, "yyyyMMdd", CultureInfo.InvariantCulture);

                    fld = rtMessage.GetField(RTFieldId.STRIKE_PRICE);
                    if (fld != null && !fld.IsEmpty) snapShotData.StrikePrice = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.LAST_1);
                    if (fld != null && !fld.IsEmpty) snapShotData.LastPrice = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.CUM_VOL);
                    if (fld != null && !fld.IsEmpty) snapShotData.TotalVolume = Convert.ToInt64(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.CUM_VOL);
                    if (fld != null && !fld.IsEmpty) snapShotData.TradeVolume = Convert.ToInt64(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.BID_1);
                    if (fld != null && !fld.IsEmpty) snapShotData.Bid = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.BID_VOL_1);
                    if (fld != null && !fld.IsEmpty) snapShotData.BidSize = Convert.ToInt64(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.RT_MARKET_CAP);
                    if (fld != null && !fld.IsEmpty) snapShotData.MarketCapitalization = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.BID_EXCH);
                    if (fld != null && !fld.IsEmpty) snapShotData.BidExchange = fld.Value;

                    fld = rtMessage.GetField(RTFieldId.ASK_1);
                    if (fld != null && !fld.IsEmpty) snapShotData.Ask = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.MID_1);
                    if (fld != null && !fld.IsEmpty) snapShotData.Mid = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.ASK_VOL_1);
                    if (fld != null && !fld.IsEmpty) snapShotData.AskSize = Convert.ToInt64(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.ASK_EXCH);
                    if (fld != null && !fld.IsEmpty) snapShotData.AskExchange = fld.Value;

                    fld = rtMessage.GetField(RTFieldId.HIGH_1);
                    if (fld != null && !fld.IsEmpty) snapShotData.High = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.LOW_1);
                    if (fld != null && !fld.IsEmpty) snapShotData.Low = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.HIGH_52WEEK);
                    if (fld != null && !fld.IsEmpty) snapShotData.High52W = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.LOW_52WEEK);
                    if (fld != null && !fld.IsEmpty) snapShotData.Low52W = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.OPEN_1);
                    if (fld != null && !fld.IsEmpty) snapShotData.Open = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.DELTA);
                    if (fld != null && !fld.IsEmpty) snapShotData.Delta = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.SHARES_OUTSTANDING);
                    if (fld != null && !fld.IsEmpty) snapShotData.SharesOutstanding = Convert.ToInt64(Convert.ToDecimal(fld.Value) * 1000000);

                    fld = rtMessage.GetField(RTFieldId.COUNTRY_CODE);
                    if (fld != null && !fld.IsEmpty) snapShotData.CountryID = MarketDataAdapterExtension.GetCountryIdFromFactsetCode(fld.Value);// = Convert.ToInt64(Convert.ToDecimal(fld.Value) * 1000000);

                    fld = rtMessage.GetField(RTFieldId.PREV_CLOSE);
                    if (fld != null && !fld.IsEmpty) snapShotData.Previous = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.DESCRIPTION);
                    if (fld != null && !fld.IsEmpty) snapShotData.FullCompanyName = fld.Value;

                    fld = rtMessage.GetField(RTFieldId.OPT_OCC_ID);
                    if (fld != null && !fld.IsEmpty)
                    {
                        string[] symbolPart = fld.Value.Split('#');

                        if (symbolPart.Length > 1)
                        {
                            snapShotData.OSIOptionSymbol = symbolPart[0].PadRight(6) + symbolPart[1];
                        }
                    }

                    //fld = rtMessage.GetField(RTFieldId.);
                    //if (fld != null && !fld.IsEmpty) ((OptionSymbolData)snapShotData).IDCOOptionSymbol = fld.Value;

                    //fld = rtMessage.GetField(RTFieldId.);
                    //if (fld != null && !fld.IsEmpty) ((OptionSymbolData)snapShotData).OpraSymbol = fld.Value;

                    DateTime updateDate = DateTimeConstants.MinValue;
                    fld = rtMessage.GetField(RTFieldId.LAST_DATE_1);
                    if (fld != null && !fld.IsEmpty) DateTime.TryParseExact(fld.Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out updateDate);
                    if (updateDate == System.DateTime.MinValue || updateDate == DateTimeConstants.MinValue)
                        updateDate = System.DateTime.Today;

                    fld = rtMessage.GetField(RTFieldId.LAST_TIME_1);
                    if (fld != null && !fld.IsEmpty) snapShotData.UpdateTime = (updateDate + TimeSpan.ParseExact(fld.Value, "hhmmssfff", CultureInfo.InvariantCulture)).ToUniversalTime();

                    fld = rtMessage.GetField(RTFieldId.LAST_TICK_1);
                    if (fld != null && !fld.IsEmpty)
                    {
                        try
                        {
                            snapShotData.LastTick = MarketDataAdapterExtension.GetEnumDescription((FactSetConstants.FactSetTickDirection)(Convert.ToInt32(fld.Value)));
                        }
                        catch
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Error in parsing LAST_TICK_1 field from FactSet security response: {0}, LAST_TICK_1: {1}", rtMessage.Key.Replace(_delayedSuffix, string.Empty), Convert.ToInt32(fld.Value)), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Error);
                        }
                    }

                    //fld = rtMessage.GetField(RTFieldId.);
                    //if (fld != null && !fld.IsEmpty) snapShotData.Beta_5yrMonthly = Convert.ToDouble(fld.Value);

                    //Change is not coming from FactSet, so applying custom logic
                    snapShotData.Change = snapShotData.LastPrice - snapShotData.Previous;

                    fld = rtMessage.GetField(RTFieldId.DIVIDEND_YIELD);
                    if (fld != null && !fld.IsEmpty) snapShotData.DividendYield = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.DIV_PRICE);
                    if (fld != null && !fld.IsEmpty) snapShotData.Dividend = Convert.ToDouble(fld.Value);

                    DateTime xDividendDate = DateTimeConstants.MinValue;
                    fld = rtMessage.GetField(RTFieldId.EX_DATE);
                    if (fld != null && !fld.IsEmpty) DateTime.TryParseExact(fld.Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out xDividendDate);
                    snapShotData.XDividendDate = xDividendDate;

                    //fld = rtMessage.GetField(RTFieldId.);
                    //if (fld != null && !fld.IsEmpty) snapShotData.DividendInterval = Convert.ToInt64(fld.Value);

                    //fld = rtMessage.GetField(RTFieldId.);
                    //if (fld != null && !fld.IsEmpty) snapShotData.DividendAmtRate = float.Parse(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.DIVIDEND_PAYOUT);
                    if (fld != null && !fld.IsEmpty) snapShotData.AnnualDividend = Convert.ToDouble(fld.Value);

                    fld = rtMessage.GetField(RTFieldId.PRICE_CURRENCY);
                    if (fld != null && !fld.IsEmpty) snapShotData.CurencyCode = fld.Value;

                    string factSetKey = rtMessage.Key.Replace(_delayedSuffix, string.Empty);

                    snapShotData.RequestedSymbology = ApplicationConstants.SymbologyCodes.FactSetSymbol;
                    snapShotData.FactSetSymbol = factSetKey;
                    snapShotData.MarketDataProvider = MarketDataProvider.FactSet;

                    MarketDataSymbolResponse marketDataSymbolResponse = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(factSetKey);

                    if (marketDataSymbolResponse != null)
                    {
                        if (snapShotData.CategoryCode == AssetCategory.Future && marketDataSymbolResponse.AssetCategory != AssetCategory.Future)
                        {
                            MarketDataAdapterExtension.RemoveMarketDataSymbolInformation(marketDataSymbolResponse.TickerSymbol);
                            return;
                        }
                    }

                    // Options
                    if (marketDataSymbolResponse == null && (snapShotData.CategoryCode == AssetCategory.EquityOption || snapShotData.CategoryCode == AssetCategory.FutureOption))
                    {
                        lock (_dictOCCAndFactSetMapping)
                        {
                            if (_dictOCCAndFactSetMapping.ContainsKey(factSetKey))
                            {
                                marketDataSymbolResponse = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(_dictOCCAndFactSetMapping[factSetKey]);
                            }
                            else
                            {
                                fld = rtMessage.GetField(RTFieldId.OPT_OCC_ID);
                                if (fld != null && !fld.IsEmpty)
                                {
                                    marketDataSymbolResponse = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(fld.Value);

                                    if (marketDataSymbolResponse != null)
                                    {
                                        _dictOCCAndFactSetMapping.Add(factSetKey, fld.Value);
                                    }
                                }
                            }
                        }
                    }

                    // Fixed Income
                    if (marketDataSymbolResponse == null && snapShotData.CategoryCode == AssetCategory.FixedIncome)
                    {
                        lock (_dictCUSIPAndFactSetMapping)
                        {
                            if (_dictCUSIPAndFactSetMapping.ContainsKey(factSetKey))
                            {
                                marketDataSymbolResponse = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(_dictCUSIPAndFactSetMapping[factSetKey]);
                            }
                            else
                            {
                                fld = rtMessage.GetField(RTFieldId.CUSIP);
                                if (fld != null && !fld.IsEmpty)
                                {
                                    marketDataSymbolResponse = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(fld.Value);

                                    if (marketDataSymbolResponse != null)
                                    {
                                        _dictCUSIPAndFactSetMapping.Add(factSetKey, fld.Value);
                                    }
                                }
                            }
                        }
                    }

                    if (marketDataSymbolResponse == null)
                    {
                        marketDataSymbolResponse = MarketDataAdapterExtension.GetTickerSymbolFromMarketData(snapShotData);

                        if (marketDataSymbolResponse != null && !string.IsNullOrEmpty(marketDataSymbolResponse.TickerSymbol))
                        {
                            MarketDataAdapterExtension.AddMarketDataForTickerSymbolToCache(marketDataSymbolResponse.TickerSymbol, marketDataSymbolResponse);
                        }
                    }

                    if (marketDataSymbolResponse != null && !string.IsNullOrEmpty(marketDataSymbolResponse.TickerSymbol))
                    {
                        lock (_dictSubscriptions)
                            if (!isSnapshotData && !_dictSubscriptions.ContainsKey(marketDataSymbolResponse.TickerSymbol))
                                return;

                        snapShotData.Symbol = marketDataSymbolResponse.TickerSymbol;

                        if (string.IsNullOrWhiteSpace(snapShotData.ListedExchange) && marketDataSymbolResponse.AUECID > 0)
                            snapShotData.ListedExchange = MarketDataAdapterExtension.GetExchangeName(marketDataSymbolResponse.AUECID);

                        MarketDataAdapterExtension.AddToSnapShotSymbolDataCollection(ref snapShotData, MarketDataProvider.FactSet);

                        Data obj = new Data();
                        obj.Info = snapShotData;
                        if (obj.Info != null)
                        {
                            if (isSnapshotData)
                            {
                                MarketDataAdapterExtension.SecurityValidationLogging(string.Format("SecurityValidationLogging: FactSetManager.FillSymbolDataFromRTMessage() > SnapshotData released from Adapter for Symbol: {0}, Time: {1}", snapShotData.Symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")));

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
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Error in parsing FactSet security response: {0}, Response: {1}", rtMessage.Key.Replace(_delayedSuffix, string.Empty), rtMessage), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Error);

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private string UpdateSymbolBasedOnEntitlement(string factSetSymbol)
        {
            lock (_dictRealTimeEntitlement)
            {
                if (_dictRealTimeEntitlement.ContainsKey(factSetSymbol))
                {
                    if (_dictRealTimeEntitlement[factSetSymbol] == FactSetConstants.AccessType.Delayed)
                    {
                        return _delayedSuffix;
                    }
                }
                else
                {
                    _dictRealTimeEntitlement.Add(factSetSymbol, FactSetConstants.AccessType.Realtime);
                }
            }
            return string.Empty;
        }
        #endregion
        private object GetSymbolLock(string symbol)
        {
            lock (_symbolLocksGlobalLock)
            {
                if (!_symbolLocks.TryGetValue(symbol, out var locker))
                {
                    locker = new object();
                    _symbolLocks[symbol] = locker;
                }
                return locker;
            }
        }

        #region Credential Management
        private void LoadCredentials()
        {
            try
            {

                string credentialsFilePath = Path.Combine(new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.ToString(), ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_FactSetSettings, "FactSetCredentialDetails"));

                if (File.Exists(credentialsFilePath))
                {
                    string encryptedProperties = File.ReadAllText(credentialsFilePath, Encoding.UTF8);
                    string decryptedProperties = TripleDESEncryptDecrypt.TripleDESDecryption(encryptedProperties, encryptionKey);
                    if (!string.IsNullOrWhiteSpace(decryptedProperties))
                    {
                        string[] decryptedPropertiesValue = decryptedProperties.Split(Seperators.SEPERATOR_6);

                        _clientConnectionUsername = decryptedPropertiesValue[0];
                        _clientConnectionPassword = decryptedPropertiesValue[1];
                        _clientConnectionHost = decryptedPropertiesValue[2];
                        _clientConnectionPort = decryptedPropertiesValue[3];

                        _supportConnectionUsername = decryptedPropertiesValue[4];
                        _supportConnectionPassword = decryptedPropertiesValue[5];
                    }
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

        public List<string> GetCredentials()
        {
            return new List<string>()
            {
                _clientConnectionUsername,
                _clientConnectionPassword,
                _clientConnectionHost,
                _clientConnectionPort,

                _supportConnectionUsername,
                _supportConnectionPassword
            };
        }

        public bool UpdateCredentials(string _clientUsername, string _clientPassword, string _clientHost, string _clientPort, string _supportUsername, string _supportPassword)
        {
            try
            {
                _clientConnectionUsername = _clientUsername;
                _clientConnectionPassword = _clientPassword;
                _clientConnectionHost = _clientHost;
                _clientConnectionPort = _clientPort;

                _supportConnectionUsername = _supportUsername;
                _supportConnectionPassword = _supportPassword;

                string credentials = _clientUsername + Seperators.SEPERATOR_6 + _clientPassword + Seperators.SEPERATOR_6 + _clientHost + Seperators.SEPERATOR_6 + _clientPort +
                    Seperators.SEPERATOR_6 + _supportUsername + Seperators.SEPERATOR_6 + _supportPassword;

                string credentialsFilePath = Path.Combine(new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.ToString(), ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_FactSetSettings, "FactSetCredentialDetails"));

                if (!File.Exists(credentialsFilePath))
                {
                    if (string.IsNullOrWhiteSpace(credentialsFilePath))
                        throw new Exception("FactSet credentials storage path is not valid. Please contact administrator.");
                    else if (!Directory.Exists(credentialsFilePath))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(credentialsFilePath));
                    }
                }
                File.WriteAllText(credentialsFilePath, TripleDESEncryptDecrypt.TripleDESEncryption(credentials, encryptionKey));
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessException ex = new UnauthorizedAccessException("Access to FactSet credentials storage is denied. Please contact administrator.");
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw ex;
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
        #endregion

        #region ILiveFeedAdapter Methods
        public void Connect()
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation)
                {
                    MarketDataAdapterExtension.ClearCache();
                    Disconnect();

                    #region Client's Server-level Connection
                    if (!string.IsNullOrWhiteSpace(_clientConnectionUsername) && !string.IsNullOrWhiteSpace(_clientConnectionPassword) && !string.IsNullOrWhiteSpace(_clientConnectionHost))
                    {
                        _rtConsumer = new RTConsumer();
                        _rtConsumer.ConnInfo = !string.IsNullOrEmpty(_clientConnectionPort) ?
                             RTConsumer.MakeConnInfo(_clientConnectionUsername, _clientConnectionPassword, _clientConnectionHost, int.Parse(_clientConnectionPort)) :
                             RTConsumer.MakeConnInfo(_clientConnectionUsername, _clientConnectionPassword, _clientConnectionHost);

                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Connecting to FactSet on {0}:XXXXXX@{1}", _clientConnectionUsername, _clientConnectionHost), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);

                        _rtConsumer.ConnectCompleted += new EventHandler<ConnectCompletedEventArgs>(ClientConnectionHandler);
                        _rtConsumer.DispatchCompleted += ClientDispatchCompleted;
                        _rtConsumer.OptionsGreeksEnabled = false;
                        _rtConsumer.SetSendUnchangedFields(false);
                        _rtConsumer.ConnectAsync();
                    }
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("FactSet \"Client's Server-level Connection\" credentials missing.", LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
                    }
                    #endregion

                    #region Support's Server-level Connection
                    if (!string.IsNullOrWhiteSpace(_supportConnectionUsername) && !string.IsNullOrWhiteSpace(_supportConnectionPassword) && !string.IsNullOrWhiteSpace(_clientConnectionHost))
                    {
                        _rtConsumerSupport = new RTConsumer();
                        _rtConsumerSupport.ConnInfo = !string.IsNullOrEmpty(_clientConnectionPort) ?
                             RTConsumer.MakeConnInfo(_supportConnectionUsername, _supportConnectionPassword, _clientConnectionHost, int.Parse(_clientConnectionPort)) :
                             RTConsumer.MakeConnInfo(_supportConnectionUsername, _supportConnectionPassword, _clientConnectionHost);

                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Nirvana Support: Connecting to FactSet on {0}:XXXXXX@{1}", _supportConnectionUsername, _supportConnectionPassword), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);

                        _rtConsumerSupport.ConnectCompleted += new EventHandler<ConnectCompletedEventArgs>(SupportConnectionHandler);
                        _rtConsumerSupport.DispatchCompleted += SupportDispatchCompleted;
                        _rtConsumerSupport.OptionsGreeksEnabled = false;
                        _rtConsumerSupport.SetSendUnchangedFields(false);
                        _rtConsumerSupport.ConnectAsync();
                    }
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("FactSet \"Support's Server-level Connection\" credentials missing", LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
                    }
                    #endregion
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

        public void Disconnect()
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation)
                {
                    if (_rtConsumer != null && !string.IsNullOrWhiteSpace(_rtConsumer.ConnectedToHost) && _rtConsumer.IsConnected)
                    {
                        lock (_dictSubscriptions)
                        {
                            _dictSubscriptions.Clear();
                        }

                        _rtConsumer.Disconnect();

                        ConnectionStatus(false);

                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Disconnected from FactSet on {0}", _rtConsumer.ConnectedToHost), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);
                        _rtConsumer = null;
                    }

                    if (_rtConsumerSupport != null && !string.IsNullOrWhiteSpace(_rtConsumerSupport.ConnectedToHost) && _rtConsumerSupport.IsConnected)
                    {
                        _rtConsumerSupport.Disconnect();

                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Nirvana Support: Disconnected from FactSet on {0}", _rtConsumerSupport.ConnectedToHost), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);
                        _rtConsumerSupport = null;
                    }
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

        public void GetContinuousData(string tickerSymbol)
        {
            try
            {
                MarketDataAdapterExtension.SecurityValidationLogging(string.Format("SecurityValidationLogging: FactSetManager.GetContinuousData() entered: {0}, Time: {1}", tickerSymbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")));

                if (!_configEnableMarketDataSimulationForAutomation && !string.IsNullOrWhiteSpace(tickerSymbol))
                {
                    if (_rtConsumer != null)
                    {
                        MarketDataSymbolResponse marketDataSymbolResponse = MarketDataAdapterExtension.GetMarketDataSymbolInformationFromTickerSymbol(tickerSymbol);

                        if (marketDataSymbolResponse != null && !string.IsNullOrWhiteSpace(marketDataSymbolResponse.FactSetSymbol))
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("ContinuousData Request: {0}, FactSetSymbol: {1}", tickerSymbol, marketDataSymbolResponse.FactSetSymbol), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);

                            if (!_dictRealTimeEntitlement.ContainsKey(marketDataSymbolResponse.FactSetSymbol) || (_dictRealTimeEntitlement.ContainsKey(marketDataSymbolResponse.FactSetSymbol) && _dictRealTimeEntitlement[marketDataSymbolResponse.FactSetSymbol] != FactSetConstants.AccessType.Denied))
                            {
                                lock (_dictSubscriptions)
                                {
                                    if (!_dictSubscriptions.ContainsKey(tickerSymbol))
                                    {
                                        RTRequest req = new RTRequest(_configFactSetDataService, marketDataSymbolResponse.FactSetSymbol + UpdateSymbolBasedOnEntitlement(marketDataSymbolResponse.FactSetSymbol));
                                        _dictSubscriptions.Add(tickerSymbol, _rtConsumer.MakeRequest(req, SecurityResponseHandler));
                                    }
                                    else
                                    {
                                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("ContinuousData duplicate request received: {0}, FactSetSymbol: {1}", tickerSymbol, marketDataSymbolResponse.FactSetSymbol), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("FactSet not connected so unable to send ContinuousData request for Symbol: {0}", tickerSymbol), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Error);
                    }
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

        public void GetSnapShotData(string symbol, ApplicationConstants.SymbologyCodes symbologyCode, bool completeInfo)
        {
            try
            {
                MarketDataAdapterExtension.SecurityValidationLogging(string.Format("SecurityValidationLogging: FactSetManager.GetSnapShotData() entered: {0}, Time: {1}", symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")));

                if (!_configEnableMarketDataSimulationForAutomation && !string.IsNullOrWhiteSpace(symbol))
                {
                    if (_rtConsumer != null)
                    {
                        if (symbologyCode == ApplicationConstants.SymbologyCodes.TickerSymbol)
                        {
                            MarketDataSymbolResponse marketDataSymbolResponse = MarketDataAdapterExtension.GetMarketDataSymbolInformationFromTickerSymbol(symbol);

                            if (marketDataSymbolResponse != null && !string.IsNullOrWhiteSpace(marketDataSymbolResponse.FactSetSymbol))
                            {
                                if (!_dictRealTimeEntitlement.ContainsKey(marketDataSymbolResponse.FactSetSymbol) || (_dictRealTimeEntitlement.ContainsKey(marketDataSymbolResponse.FactSetSymbol) && _dictRealTimeEntitlement[marketDataSymbolResponse.FactSetSymbol] != FactSetConstants.AccessType.Denied))
                                {
                                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SnapShotData Request: {0}, FactSetSymbol: {1}", symbol, marketDataSymbolResponse.FactSetSymbol), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);

                                    RTRequest req = new RTRequest(_configFactSetDataService, marketDataSymbolResponse.FactSetSymbol + UpdateSymbolBasedOnEntitlement(marketDataSymbolResponse.FactSetSymbol), true);
                                    _rtConsumer.MakeRequest(req, SecurityResponseHandler);
                                }
                            }
                        }
                        else if (symbologyCode == ApplicationConstants.SymbologyCodes.FactSetSymbol)
                        {
                            if (!_dictRealTimeEntitlement.ContainsKey(symbol) || (_dictRealTimeEntitlement.ContainsKey(symbol) && _dictRealTimeEntitlement[symbol] != FactSetConstants.AccessType.Denied))
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SnapShotData Request of FactSetSymbol: {0}", symbol), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);

                                RTRequest req = new RTRequest(_configFactSetDataService, symbol + UpdateSymbolBasedOnEntitlement(symbol), true);
                                _rtConsumer.MakeRequest(req, SecurityResponseHandler);
                            }
                        }
                    }
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("FactSet not connected so unable to send SnapShotData request for Symbol: {0}", symbol), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Error);
                    }
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

        public void GetOptionChain(string underlyingSymbol, OptionChainFilter optionChainFilter)
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation && !string.IsNullOrWhiteSpace(underlyingSymbol))
                {
                    MarketDataSymbolResponse marketDataSymbolResponse = MarketDataAdapterExtension.GetMarketDataSymbolInformationFromTickerSymbol(underlyingSymbol);

                    if (marketDataSymbolResponse != null && !string.IsNullOrWhiteSpace(marketDataSymbolResponse.FactSetSymbol))
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("OptionChainData Request: {0}, FactSetSymbol: {1}", underlyingSymbol, marketDataSymbolResponse.FactSetSymbol), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("OptionChainData Request: {0}, FactSetSymbol: {1}", underlyingSymbol, marketDataSymbolResponse.FactSetSymbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);

                        lock (_dictOptionChainFilters)
                        {
                            if (_dictOptionChainFilters.ContainsKey(underlyingSymbol))
                            {
                                _dictOptionChainFilters[underlyingSymbol] = optionChainFilter;
                            }
                            else
                            {
                                _dictOptionChainFilters.Add(underlyingSymbol, optionChainFilter);
                            }
                        }

                        RTRequest req = new RTRequest(_configFactSetOptionChainService, marketDataSymbolResponse.FactSetSymbol, true);
                        _rtConsumer.MakeRequest(req, OptionChainResponseHandler);
                    }
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

        public void DeleteSymbol(string tickerSymbol)
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation && !string.IsNullOrWhiteSpace(tickerSymbol))
                {
                    lock (_dictSubscriptions)
                    {
                        if (_dictSubscriptions.ContainsKey(tickerSymbol))
                        {
                            if (!string.IsNullOrWhiteSpace(_rtConsumer.ConnectedToHost) && _rtConsumer.IsConnected)
                            {
                                _rtConsumer.Cancel(_dictSubscriptions[tickerSymbol]);

                                MarketDataAdapterExtension.RemoveMarketDataSymbolInformation(tickerSymbol);

                                _dictSubscriptions[tickerSymbol] = null;
                                _dictSubscriptions.Remove(tickerSymbol);

                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Symbol: {0} subscription stopped", tickerSymbol), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
                            }
                            else
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("No active connection with Factset. So unable to delete Symbol: {0}", tickerSymbol), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
                            }
                        }
                        else
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Symbol: {0} was not subscribed, so unable to delete it", tickerSymbol), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
                        }
                    }
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

        public List<SymbolData> GetAvailableLiveFeed()
        {
            return MarketDataAdapterExtension.GetAvailableLiveFeed();
        }

        public Task<object> GetLiveDataDirectlyFromFeed()
        {
            return null;
        }

        public Dictionary<string, bool> CheckIfInternationalSymbols(List<string> symbols)
        {
            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("FactSet CheckIfInternationalSymbols not implemented yet", LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Warning);
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
            return new Dictionary<string, int>();
        }

        public Dictionary<string, string> GetTickersLastStatusCode()
        {
            return new Dictionary<string, string>();
        }

        ///<inheritdoc/>
        public void SetDebugEnableDisable(bool isDebugEnable, double pctTolerance)
        {

        }
        ///<inheritdoc/>
        public void UpdateSecurityDetails(BusinessObjects.SecurityMasterBusinessObjects.SecMasterbaseList secMasterList)
        {

        }

        public event EventHandler<EventArgs<bool>> Connected;

        public event EventHandler<EventArgs<bool>> Disconnected;

        public event EventHandler<Data> ContinuousDataResponse;

        public event EventHandler<Data> SnapShotDataResponse;

        public event EventHandler<EventArgs<string, List<OptionStaticData>>> OptionChainResponse;
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _rtConsumer.Dispose();
                    _rtConsumerSupport.Dispose();
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
        #endregion
    }
}
