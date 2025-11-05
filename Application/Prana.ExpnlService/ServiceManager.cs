using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.ExpnlService.DataDumper;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.QueueManager;
using Prana.MonitoringProcessor;
using Prana.SocketCommunication;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace Prana.ExpnlService
{
    public class ServiceManager : ILiveFeedCallback, IMarketDataPermissionServiceCallback, IDisposable
    {
        private static Dictionary<string, UserWisePermissions> _dictUserWisePermissions = new Dictionary<string, UserWisePermissions>();
        private static InstantDataRequestManager _instantDataRequestManager = new InstantDataRequestManager();
        private static ProxyBase<IPranaPositionServices> _positionManagementServices = null;
        private static ServiceManager _serviceManager = null;
        private int _chunkSize = Convert.ToInt32(ConfigurationManager.AppSettings["MessageChunkSize"]);
        private ServerCustomCommunicationManager _clientBroadCastingManager = null;
        private string _compressedRowID = string.Empty;
        private List<IGroupingComponent> _compressionComponent;
        private ExPnlCache _exPNLCache = null;
        private IQueueProcessor _inQueueClientBroadCasting = null;
        private bool _isSending = false;
        private object _lockerObj = null;
        private object _lockForSelectedColumnList;
        private IQueueProcessor _outQueueClientBroadCasting = null;
        private DuplexProxyBase<IPricingService> _pricingServiceProxy = null;
        private List<string> _selectedCompressionViews = new List<string>();
        private string _taxlotReqCallerGridName = string.Empty;
        private PranaBinaryFormatter binaryFormatter = null;
        private Dictionary<string, bool> _dictUserWiseStartUpDataSentSuccessfully = new Dictionary<string, bool>();

        private ServiceManager()
        {
            try
            {
                _lockerObj = new object();
                _lockForSelectedColumnList = new object();
                binaryFormatter = new PranaBinaryFormatter();
                _compressionComponent = new List<IGroupingComponent>();
                CreateUserAccountPermissionDict();
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

        public delegate void UserPreferencesUpdated(UserPreferencesEventArgs updatedPrefs);

        public event EventHandler ERefreshData;
        public event EventHandler LogOnScreenToMain;
        public event EventHandler<UserPreferencesEventArgs> PreferencesUpdated;
        public event EventHandler UserDataRefreshCompleted;
        public event EventHandler UserDataRefreshedRejected;
        public event EventHandler UserInitiatedDataRefreshed;

        public static ProxyBase<IPranaPositionServices> PositionManagementServices
        {
            set { _positionManagementServices = value; }
        }

        public ServerCustomCommunicationManager ClientBroadCastingManager
        {
            get { return _clientBroadCastingManager; }
            set { _clientBroadCastingManager = value; }
        }

        public List<IGroupingComponent> ICompressor
        {
            get { return _compressionComponent; }
        }

        public bool IsSending
        {
            get { return _isSending; }
        }

        public static void CreatePositionManagementProxy()
        {
            try
            {
                _positionManagementServices = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
                DataManager.GetInstance().PositionManagementServices = _positionManagementServices;
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
        /// Singleton instance of ServiceManager
        /// </summary>
        /// <returns></returns>
        public static ServiceManager GetInstance()
        {
            if (_serviceManager == null)
            {
                _serviceManager = new ServiceManager();
            }
            return _serviceManager;
        }

        public Dictionary<int, DateTime> GetDBClearanceTime()
        {
            try
            {
                return TimeZoneHelper.GetInstance().ClearanceTime;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public List<int> GetAccountsForTheUser(string user)
        {
            if (_dictUserWisePermissions.ContainsKey(user))
            {
                return _dictUserWisePermissions[user].AllowedAccounts;
            }
            return null;
        }

        public void HandlePMPreferences(string msg)
        {
            try
            {
                //will need a lock here if list changes at runtime.
                string userID;
                ExPNLPreferenceMsgType prefMSGType;
                string subMessage;

                PranaMessageFormatter.FromPMPreferenceMessage(msg, out prefMSGType, out userID, out subMessage);

                if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(subMessage))
                {
                    lock (_lockForSelectedColumnList)
                    {
                        ViewManager.GetInstance().HandlePMPreferences(userID, prefMSGType, subMessage);
                    }
                }
                if (prefMSGType == ExPNLPreferenceMsgType.GroupByColumnAdded || prefMSGType == ExPNLPreferenceMsgType.SelectedColumnAdded || prefMSGType == ExPNLPreferenceMsgType.SelectedViewChanged)
                {
                    _instantDataRequestManager.HandleInstantDataSending(userID, prefMSGType, subMessage);
                }
                else if (prefMSGType == ExPNLPreferenceMsgType.SelectedViewCopied)
                {
                    List<String> userIDToSend = new List<string> { userID };
                    SendControlMessageToClient(PranaMessageConstants.MSG_ExpPNLDynamicColumnList, userIDToSend);
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

        public bool Start()
        {
            bool isStarted = false;
            try
            {
                CreatePositionManagementProxy();
                CreatePricingServiceProxy();
                CreateCashManagementProxy();
                CreateSecMasterSyncProxy();
                CreateMarketDataPermissionServiceProxy();

                //Client BroadCasting Related Connection
                _inQueueClientBroadCasting = new QueueProcessor(HandlerType.ExposurePnlHandler);
                _inQueueClientBroadCasting.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(_inQueueClientBroadCasting_MessageQueued);
                _outQueueClientBroadCasting = new QueueProcessor(HandlerType.ExposurePnlHandler);
                _clientBroadCastingManager = ServerCustomCommunicationManager.GetInstance();

                IQueueProcessor outMonitoringQueue = new QueueProcessor(HandlerType.MonitoringServices);

                List<IQueueProcessor> inQueueProcessorList = new List<IQueueProcessor>();
                List<IQueueProcessor> outQueueProcessorList = new List<IQueueProcessor>();

                inQueueProcessorList.Add(_inQueueClientBroadCasting);
                outQueueProcessorList.Add(outMonitoringQueue);
                outQueueProcessorList.Add(_outQueueClientBroadCasting);

                FillDynamicColumnListFromXMLFile();
                FillColumnDataAsZeroListFromXMLFile();

                List<string> tradingAccounts = WindsorContainerManager.GetAllTradingAccounts();

                foreach (string viewsToInitialize in _selectedCompressionViews)
                {
                    _compressionComponent.Add(CompressionViewFactory.GetInstance().GetView(viewsToInitialize));
                }

                //get instance of cache
                _exPNLCache = ExPnlCache.Instance;

                //initiate cache
                _exPNLCache.Start();

                SetSummaryDistinctsAccountSets();

                _exPNLCache.LogAUECDatesToManager += new EventHandler(_exPNLCache_LogAUECDatesToManager);
                _compressionComponent[0].DataCompressed += new EventHandler(_compressionComponent_DataCompressed);
                this.ClientBroadCastingManager.Disconnected += new ConnectionMessageReceivedDelegate(ClientBroadCastingManager_Disconnected);
                isStarted = true;

                // start Monitoring Services
                PranaMonitoringProcessor.GetInstance.Initlise(outMonitoringQueue);
                _exPNLCache.StartCalculations();
                _clientBroadCastingManager.Initialise(inQueueProcessorList, outQueueProcessorList, tradingAccounts);    // , "Exposure & PNL Server"); // set the of server and stsrt listnenig to client       :removed as parameters not used in method, Microsoft Managed Rules
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                isStarted = false;
                if (rethrow)
                {
                    throw;
                }
            }
            return isStarted;
        }

        #region Real-time data dump Process
        /// <summary>
        /// Add Account_Symbol Compression
        /// </summary>
        internal void AddAccountSymbolCompression()
        {
            try
            {
                #region Touch Data Dumper Code for handling data compressed event

                //Handling event of data compressed in case account-symbol already is not setup
                //This handles event only for one element
                //Case 1: ExPnL is running on Taxlot compression
                //      Wire-up DataCompressed Event of _compressionComponent's 1st Element
                //Case 2: ExPnL is running on fund_symbol compression
                //      Do nothing for calculation
                //      However in same dataCompressed event handler handle data for dumper
                //      Also check the runtime impact of user subscription

                //Adding account-Symbol Compression in case it is not selected.
                //This _selectedCompressionViews object contains list of views to be calculated however always 
                //first element in object is used 
                if (!_selectedCompressionViews.Contains(CompressionViewFactory.ACCOUNTSYMBOL_COMPRESSION) || CachedDataManager.GetInstance.IsFilePricingForTouch())
                    _compressionComponent.Add(CompressionViewFactory.GetInstance().GetView(CompressionViewFactory.ACCOUNTSYMBOL_COMPRESSION));

                IGroupingComponent accountSymbolGroupingComponent = _compressionComponent
                    .LastOrDefault(x => x.GroupingComponentName == CompressionViewFactory.ACCOUNTSYMBOL_COMPRESSION);

                if (accountSymbolGroupingComponent != null)
                    accountSymbolGroupingComponent.DataCompressed += accountSymbolGroupingComponent_DataCompressed;

                #endregion
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
        /// Remove Account Symbol Compression
        /// </summary>
        internal void RemoveAccountSymbolCompression()
        {
            try
            {
                //Stop the compression if data-dumper added it.

                IGroupingComponent accountSymbolGroupingComponent = _compressionComponent
                    .FirstOrDefault(x => x.GroupingComponentName == CompressionViewFactory.ACCOUNTSYMBOL_COMPRESSION);

                if (accountSymbolGroupingComponent != null)
                {
                    accountSymbolGroupingComponent.DataCompressed -= accountSymbolGroupingComponent_DataCompressed;
                    if (_compressionComponent.Count > 1)
                    {
                        _compressionComponent.Remove(accountSymbolGroupingComponent);
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

        private void accountSymbolGroupingComponent_DataCompressed(object sender, EventArgs e)
        {
            try
            {

                IGroupingComponent accountSymbolGroupingComponent = (IGroupingComponent)sender;

                //Need to check if below operation requires different thread or not
                Dictionary<int, ExposurePnlCacheItemList> calculatedCompressedOrders = accountSymbolGroupingComponent.GetCompressedData().OutputCompressedData;
                AccountSymbolCacheManager.Instance.SaveIncomingData(calculatedCompressedOrders);
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        private void SetSummaryDistinctsAccountSets()
        {
            try
            {
                //Adding Accounts to DistinctAccountPermissionSets
                Dictionary<int, string> accounts = new Dictionary<int, string>(CachedDataManager.GetInstance.GetAccounts());
                foreach (int accountID in accounts.Keys)
                {
                    List<int> tempAccounts = new List<int>();
                    tempAccounts.Add(accountID);
                    SessionManager.DistinctAccountPermissionSets.Add(SessionManager.UniqueAutoIncrementID, tempAccounts);
                    SessionManager.AccountAndDistinctAccountPermissionSetsMapping.Add(accountID, SessionManager.UniqueAutoIncrementID);
                    SessionManager.UniqueAutoIncrementID++;
                }

                //Adding UnallocatedAccountID to DistinctAccountPermissionSets
                int unallocatedAccountID = -1;
                List<int> unallocatedAccountIDList = new List<int>();
                unallocatedAccountIDList.Add(unallocatedAccountID);
                SessionManager.DistinctAccountPermissionSets.Add(SessionManager.UniqueAutoIncrementID, unallocatedAccountIDList);
                SessionManager.AccountAndDistinctAccountPermissionSetsMapping.Add(unallocatedAccountID, SessionManager.UniqueAutoIncrementID);
                SessionManager.UniqueAutoIncrementID++;
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

        public void Stop()
        {
            try
            {
                if (_compressionComponent != null && _compressionComponent.Count > 0)
                {
                    if (_compressionComponent[0] != null)
                    {
                        _compressionComponent[0].DataCompressed -= new EventHandler(_compressionComponent_DataCompressed);
                        _compressionComponent[0].Dispose();
                    }
                }
                _compressionComponent.Clear();
                if (_exPNLCache != null)
                {
                    _exPNLCache.LogAUECDatesToManager -= new EventHandler(_exPNLCache_LogAUECDatesToManager);
                    _exPNLCache.StopCalculations();
                    _exPNLCache.Stop();
                    lock (_lockerObj)
                    {
                        _instantDataRequestManager.ExPNLUserStatus.Clear();
                        _exPNLSubscriptionLookup.Clear();
                    }
                    _dictUserWiseStartUpDataSentSuccessfully.Clear();
                }
                if (_clientBroadCastingManager != null)
                {
                    _clientBroadCastingManager.DisConnectAllClients();
                }
                if (_positionManagementServices != null)
                    _positionManagementServices.Dispose();
                if (_pricingServiceProxy != null)
                    _pricingServiceProxy.Dispose();
                if (_cashManagementServices != null)
                    _cashManagementServices.Dispose();
                _dataRefreshing = false;
                _isSending = false;
            }
            catch (Exception ex)
            {
                _dataRefreshing = false;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal void SetRunningView(string compressionName)
        {
            try
            {
                _selectedCompressionViews.Clear();
                _selectedCompressionViews.Add(compressionName);
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

        private void _compressionComponent_DataCompressed(object sender, EventArgs e)
        {
            try
            {
                if (!_isSending)
                {
                    if (_exPNLSubscriptionLookup != null && _exPNLSubscriptionLookup.Count > 0 && _dictUserWiseStartUpDataSentSuccessfully != null && _dictUserWiseStartUpDataSentSuccessfully.Count > 0)
                    {
                        Dictionary<string, bool> tempDict = new Dictionary<string, bool>(_dictUserWiseStartUpDataSentSuccessfully);
                        foreach (KeyValuePair<string, bool> kvp in tempDict)
                        {
                            if (!kvp.Value && !kvp.Key.Equals(ApplicationConstants.TRADE_SERVER_ID_FOR_EXPNL) && !kvp.Key.Equals(ApplicationConstants.TRADE_SERVER_UI_ID_FOR_EXPNL))
                                SendStartUpDataToClient(kvp.Key);
                        }
                    }
                    SendContinuousDataToUsers();
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
        }

        private void _exPNLCache_LogAUECDatesToManager(object sender, EventArgs e)
        {
            try
            {
                if (LogOnScreenToMain != null)
                {
                    LogOnScreenToMain(sender, EventArgs.Empty);
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
        }

        private void _inQueueClientBroadCasting_MessageQueued(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                string msg = e.Value.Message.ToString();
                switch (PranaMessageFormatter.GetMessageType(msg))
                {
                    case PranaMessageConstants.MSG_ExpPNLSubscription:
                        break;

                    case PranaMessageConstants.MSG_ExpPNLRefreshData:
                        HandleExPnlRefreshData(msg);
                        break;

                    case PranaMessageConstants.MSG_ExpPNLUserBusy:
                        HandleExPnlUserBusy(msg);
                        break;

                    case PranaMessageConstants.MSG_ExpPNLUpdatePreferences:
                        HandleUserPrefUpdated(msg);
                        break;

                    case PranaMessageConstants.MSG_EPNlTaxLotList:
                        HandleTaxLotsRequest(msg);
                        break;

                    case PranaMessageConstants.MSG_PMPreferences:
                        HandlePMPreferences(msg);
                        break;

                    case PranaMessageConstants.MSG_FilterDetails:
                        HandleFilterDetails(msg);
                        break;

                    default:
                        throw new Exception("Message Type Not Handled");
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
        }

        private void HandleFilterDetails(string msg)
        {
            try
            {
                lock (_lockerObj)
                {
                    string userID = string.Empty;
                    List<int> accountList = new List<int>();
                    List<int> prevAccountList = new List<int>();
                    string subMsgs = String.Empty;
                    string tabKey = string.Empty;
                    PranaMessageFormatter.FromFilterChangedRequest(msg, ref userID, ref tabKey, ref accountList, ref prevAccountList, ref subMsgs);
                    if (prevAccountList.Count != 1 || prevAccountList[0] != 0)
                        SessionManager.DeleteDynamicDistinctAccountPermissionSet(userID, tabKey, prevAccountList);
                    SessionManager.AddDynamicDistinctAccountPermissionSets(userID, tabKey, accountList);
                    _instantDataRequestManager.HandleInstantDataSending(userID, ExPNLPreferenceMsgType.FilterValueChanged, subMsgs);
                    ExPnlCache.Instance.DoWork(null);
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

        private void CreatePricingServiceProxy()
        {
            try
            {
                _pricingServiceProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
                LiveFeedManager.Instance.PricingServiceProxy = _pricingServiceProxy;
                DataManager.GetInstance().PricingServiceProxy = _pricingServiceProxy;
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

        private void CreateUserAccountPermissionDict()
        {
            try
            {
                DataTable dt = DatabaseManager.GetInstance().GetUserAccountMapping();

                foreach (DataRow dr in dt.Rows)
                {
                    UserWisePermissions userWisePermissions;
                    if (!_dictUserWisePermissions.ContainsKey(dr[0].ToString()))
                    {
                        userWisePermissions = new UserWisePermissions();
                        userWisePermissions.AllowedAccounts.Add(Convert.ToInt32(dr[1]));
                        _dictUserWisePermissions.Add(dr[0].ToString(), userWisePermissions);
                    }
                    else
                    {
                        userWisePermissions = _dictUserWisePermissions[dr[0].ToString()];
                        userWisePermissions.AllowedAccounts.Add(Convert.ToInt32(dr[1]));
                    }
                }
                foreach (KeyValuePair<int, string> kvpAccounts in CachedDataManager.GetInstance.GetAccounts())
                {
                    UserWisePermissions userWisePermissions;
                    if (!_dictUserWisePermissions.ContainsKey(ApplicationConstants.TRADE_SERVER_ID_FOR_EXPNL))
                    {
                        userWisePermissions = new UserWisePermissions();
                        userWisePermissions.AllowedAccounts.Add(kvpAccounts.Key);
                        _dictUserWisePermissions.Add(ApplicationConstants.TRADE_SERVER_ID_FOR_EXPNL, userWisePermissions);
                    }
                    else
                    {
                        userWisePermissions = _dictUserWisePermissions[ApplicationConstants.TRADE_SERVER_ID_FOR_EXPNL];
                        userWisePermissions.AllowedAccounts.Add(kvpAccounts.Key);
                    }

                    if (!_dictUserWisePermissions.ContainsKey(ApplicationConstants.TRADE_SERVER_UI_ID_FOR_EXPNL))
                    {
                        userWisePermissions = new UserWisePermissions();
                        userWisePermissions.AllowedAccounts.Add(kvpAccounts.Key);
                        _dictUserWisePermissions.Add(ApplicationConstants.TRADE_SERVER_UI_ID_FOR_EXPNL, userWisePermissions);
                    }
                    else
                    {
                        userWisePermissions = _dictUserWisePermissions[ApplicationConstants.TRADE_SERVER_UI_ID_FOR_EXPNL];
                        userWisePermissions.AllowedAccounts.Add(kvpAccounts.Key);
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

        #region Cash Management Proxy Section

        private static ProxyBase<ICashManagementService> _cashManagementServices = null;
        public static ProxyBase<ICashManagementService> CashManagementServices
        {
            set { _cashManagementServices = value; }
        }

        public static void CreateCashManagementProxy()
        {
            try
            {
                _cashManagementServices = new ProxyBase<ICashManagementService>("TradeCashServiceEndpointAddress");
                DataManager.GetInstance().CashMgmtService = _cashManagementServices;
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

        #endregion Cash Management Proxy Section

        #region SecmasterSync-Proxy

        private static ProxyBase<ISecMasterSyncServices> _secMasterSyncService = null;
        public static ProxyBase<ISecMasterSyncServices> SecMasterSyncService
        {
            set { _secMasterSyncService = value; }
        }

        public static void CreateSecMasterSyncProxy()
        {
            try
            {
                _secMasterSyncService = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
                DataManager.GetInstance().SecMasterSyncService = _secMasterSyncService;
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

        #endregion SecmasterSync-Proxy

        #region MarketDataPermissionService Proxy
        private DuplexProxyBase<IMarketDataPermissionService> _marketDataPermissionServiceProxy = null;

        private void CreateMarketDataPermissionServiceProxy()
        {
            try
            {
                _marketDataPermissionServiceProxy = new DuplexProxyBase<IMarketDataPermissionService>("PricingMarketDataPermissionServiceEndpointAddress", this);
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

        public void PermissionCheckResponse(int companyUserID, bool isPermitted)
        {
            try
            {
                string username = companyUserID.ToString();
                if (CachedDataManager.GetInstance.GetAllUsersName().ContainsKey(Convert.ToInt32(companyUserID)))
                {
                    username = CachedDataManager.GetInstance.GetAllUsersName()[Convert.ToInt32(companyUserID)];
                }
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Response of Market Data Permission for User: " + username + ", IsPermitted: " + isPermitted, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);

                lock (_lockerObj)
                {
                    if (_dictUserWisePermissions.ContainsKey(companyUserID.ToString()))
                    {
                        _dictUserWisePermissions[companyUserID.ToString()].IsMarketDataPermissionEnabled = isPermitted;
                    }
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
        #endregion


        #region Communication Connect/Disconnected

        private void ClientBroadCastingManager_Disconnected(object sender, EventArgs<ConnectionProperties> e)
        {
            try
            {
                lock (_lockerObj)
                {
                    ConnectionProperties connProperties = e.Value;
                    if (_instantDataRequestManager.ExPNLUserStatus.ContainsKey(connProperties.IdentifierID))
                    {
                        _instantDataRequestManager.ExPNLUserStatus.Remove(connProperties.IdentifierID);
                    }
                    if (_exPNLSubscriptionLookup.ContainsKey(connProperties.IdentifierID))
                    {
                        _exPNLSubscriptionLookup.Remove(connProperties.IdentifierID);
                        SessionManager.RemoveClient(connProperties.IdentifierID);
                    }
                    if (_instantDataRequestManager.PendingInstantDataRequestDict.ContainsKey(connProperties.IdentifierID))
                    {
                        _instantDataRequestManager.PendingInstantDataRequestDict.Remove(connProperties.IdentifierID);
                    }
                    if (_dictUserWiseStartUpDataSentSuccessfully.ContainsKey(connProperties.IdentifierID))
                    {
                        _dictUserWiseStartUpDataSentSuccessfully.Remove(connProperties.IdentifierID);
                    }

                    System.Threading.Tasks.Task.Run(new Action(() =>
                    {
                        if (_marketDataPermissionServiceProxy != null && !connProperties.IdentifierID.Equals(ApplicationConstants.TRADE_SERVER_ID_FOR_EXPNL))
                        {
                            string username = connProperties.IdentifierID;
                            if (CachedDataManager.GetInstance.GetAllUsersName().ContainsKey(Convert.ToInt32(connProperties.IdentifierID)))
                            {
                                username = CachedDataManager.GetInstance.GetAllUsersName()[Convert.ToInt32(connProperties.IdentifierID)];
                            }

                            try
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Removing subscription Market Data Permission for User: " + username, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                _marketDataPermissionServiceProxy.InnerChannel.RemoveSubscriptionToGetPermissionFromCache(Convert.ToInt32(connProperties.IdentifierID), "Expnl");
                            }
                            catch (Exception exp)
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Error occured in removing subscription of Market Data Permission for User: " + username + ", Message: " + exp.Message, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                            }
                        }
                    }));
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

        #endregion Communication Connect/Disconnected

        #region Expnl Messages Handling received from Clients

        private static Dictionary<string, List<int>> _exPNLSubscriptionLookup = new Dictionary<string, List<int>>();
        private bool _dataRefreshing = false;

        private List<string> _lsItemIDsToSendCompleteData;

        private DateTime _refreshTime = new DateTime();

        private string[] refreshUserName = new String[3];

        public void AddExPnlSubscription(string userID)
        {
            try
            {
                lock (_lockerObj)
                {
                    if (!string.IsNullOrWhiteSpace(userID))
                    {
                        int unallocatedAccountID = -1;

                        List<int> listOfaccounts;
                        if (_dictUserWisePermissions.ContainsKey(userID))
                        {
                            listOfaccounts = _dictUserWisePermissions[userID].AllowedAccounts;

                            if (listOfaccounts.Count == (CachedDataManager.GetInstance.GetAccounts().Count))
                            {
                                listOfaccounts.Add(unallocatedAccountID);
                            }

                            System.Threading.Tasks.Task.Run(new Action(() =>
                            {
                                if (_marketDataPermissionServiceProxy != null && !userID.Equals(ApplicationConstants.TRADE_SERVER_ID_FOR_EXPNL))
                                {
                                    string username = userID;
                                    if (CachedDataManager.GetInstance.GetAllUsersName().ContainsKey(Convert.ToInt32(userID)))
                                    {
                                        username = CachedDataManager.GetInstance.GetAllUsersName()[Convert.ToInt32(userID)];
                                    }

                                    try
                                    {
                                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Adding subscription of Market Data Permission for User: " + username, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                        _marketDataPermissionServiceProxy.InnerChannel.AddSubscriptionToGetPermissionFromCache(Convert.ToInt32(userID), "Expnl");
                                    }
                                    catch (Exception exp)
                                    {
                                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Error occured in adding subscription of Market Data Permission for User: " + username + ", Message: " + exp.Message, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                    }
                                }
                            }));
                        }
                        else
                        {
                            throw new Exception("user: " + userID + " does not have permission to see the data.");
                        }

                        if (_exPNLSubscriptionLookup.ContainsKey(userID))
                        {
                            _exPNLSubscriptionLookup[userID] = listOfaccounts;
                            if (_instantDataRequestManager.ExPNLUserStatus.ContainsKey(userID))
                            {
                                _instantDataRequestManager.ExPNLUserStatus[userID].UserBusyStatus = false;
                            }
                        }
                        else
                        {
                            _exPNLSubscriptionLookup.Add(userID, listOfaccounts);

                            UserStatus userStatus = new UserStatus();
                            userStatus.UserBusyStatus = false;
                            if (!_instantDataRequestManager.ExPNLUserStatus.ContainsKey(userID))
                            {
                                _instantDataRequestManager.ExPNLUserStatus.Add(userID, userStatus);
                            }
                            else
                            {
                                _instantDataRequestManager.ExPNLUserStatus[userID] = userStatus;
                            }
                            SessionManager.AddDistinctAccountPermissionSet(userID, listOfaccounts);
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

        public void HandleExPnlSubscription(string userID)
        {
            try
            {
                _compressionComponent[0].DataCompressed -= new EventHandler(_compressionComponent_DataCompressed);
                try
                {
                    if (_dictUserWiseStartUpDataSentSuccessfully.ContainsKey(userID))
                    {
                        _dictUserWiseStartUpDataSentSuccessfully[userID] = false;
                    }
                    else
                    {
                        _dictUserWiseStartUpDataSentSuccessfully.Add(userID, false);
                    }

                    AddExPnlSubscription(userID);
                    //On adding a new user, interrupt the normal working and first send Whole Data To this particular user
                    if (!userID.Equals(ApplicationConstants.TRADE_SERVER_ID_FOR_EXPNL) && !userID.Equals(ApplicationConstants.TRADE_SERVER_UI_ID_FOR_EXPNL))
                    {
                        List<String> userIDToSend = new List<string> { userID };
                        SendControlMessageToClient(PranaMessageConstants.MSG_ExpPNLDynamicColumnList, userIDToSend);
                        SendStartUpDataToClient(userID);
                    }
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                }
                _compressionComponent[0].DataCompressed += new EventHandler(_compressionComponent_DataCompressed);
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

        public void RefreshData()
        {
            try
            {
                string str = "EX_Refresh" + Seperators.SEPERATOR_2 + "ExPnl Button" + Seperators.SEPERATOR_2 + "1000";
                HandleExPnlRefreshData(str);
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

        private void HandleExPnlRefreshData(string message)
        {
            try
            {
                lock (_lockerObj)
                {
                    if (!_dataRefreshing)
                    {
                        //unwrap event on which new calculated Data is sent So that no calculated Data is sent till new Data is picked
                        _compressionComponent[0].DataCompressed -= new EventHandler(_compressionComponent_DataCompressed);
                        _dataRefreshing = true;
                        refreshUserName = message.Split(Seperators.SEPERATOR_2);
                        string userID = refreshUserName[2];
                        List<string> connectedUserList = new List<string>(_exPNLSubscriptionLookup.Count);
                        foreach (string connectedUserID in _exPNLSubscriptionLookup.Keys)
                        {
                            if (connectedUserID != userID)
                            {
                                connectedUserList.Add(connectedUserID);
                            }
                        }
                        refreshUserName = message.Split(Seperators.SEPERATOR_2);
                        UserInitiatedDataRefreshed(message, EventArgs.Empty);
                        System.ComponentModel.BackgroundWorker t = new System.ComponentModel.BackgroundWorker();
                        t.DoWork += new System.ComponentModel.DoWorkEventHandler(t_DoWork);
                        t.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(t_RunWorkerCompleted);
                        t.RunWorkerAsync();
                    }
                    else
                    {
                        string message1 = message + Seperators.SEPERATOR_2 + refreshUserName[1];
                        UserDataRefreshedRejected(message1, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (_lockerObj)
                {
                    _dataRefreshing = false;
                }
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void HandleExPnlUserBusy(string message)
        {
            try
            {
                lock (_lockerObj)
                {
                    string userID = string.Empty;
                    bool clientBusy = true;
                    List<string> lsRequestedItemIDs = new List<string>();
                    PranaMessageFormatter.FromClientStatusMessage(message, ref userID, ref clientBusy, ref lsRequestedItemIDs);

                    _instantDataRequestManager.SetUserFreeUpTime(userID, clientBusy);
                    if (lsRequestedItemIDs != null && lsRequestedItemIDs.Count > 0)
                    {
                        if (_lsItemIDsToSendCompleteData == null)
                            _lsItemIDsToSendCompleteData = new List<string>();

                        foreach (string iDsToSendCompleteData in lsRequestedItemIDs)
                            if (!_lsItemIDsToSendCompleteData.Contains(iDsToSendCompleteData))
                                _lsItemIDsToSendCompleteData.Add(iDsToSendCompleteData);
                    }
                    Logger.LoggerWrite("User id :" + userID + "isBusy = " + clientBusy.ToString(), LoggingConstants.CATEGORY_GENERAL);
                    if (_instantDataRequestManager.PendingInstantDataRequestDict.Count > 0)
                    {
                        _instantDataRequestManager.SendInstantDataForQueuedRequests(userID);
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

        private void HandleUserPrefUpdated(string msg)
        {
            try
            {
                UserPreferencesEventArgs updatedPrefs = new UserPreferencesEventArgs();
                bool useClosingMark = false;
                double xPercentOfAvgVolume = 100;
                PranaMessageFormatter.FromPrefUpdateMsg(msg, ref useClosingMark, ref xPercentOfAvgVolume);
                updatedPrefs.UseClosingMark = useClosingMark;
                updatedPrefs.XPercentOfAvgVolume = xPercentOfAvgVolume;
                if (PreferencesUpdated != null)
                {
                    PreferencesUpdated(this, updatedPrefs);
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

        private void SendRefreshResponse(List<string> connectedUserList)
        {
            try
            {
                SendControlMessageToClient(PranaMessageConstants.MSG_EPNlRefreshResponse, connectedUserList);
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

        private void t_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                _refreshTime = DateTime.Now;
                ExPnlCache.Instance.RefreshExPNLData(null);

                #region Compliance Section

                //refresh data on esper
                if (ERefreshData != null)
                {
                    ERefreshData(this, new EventArgs());
                }
                #endregion Compliance Section
            }
            catch (Exception ex)
            {
                lock (_lockerObj)
                {
                    _dataRefreshing = false;
                }
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void t_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                Dictionary<string, bool> tempDict = new Dictionary<string, bool>(_dictUserWiseStartUpDataSentSuccessfully);
                foreach (string userID in tempDict.Keys)
                {
                    _dictUserWiseStartUpDataSentSuccessfully[userID] = false;
                }
                //will need a lock here if list changes at runtime. Will happen when user can change his subscription from client side
                _compressionComponent[0].DataCompressed += new EventHandler(_compressionComponent_DataCompressed);
            }
            catch (Exception ex)
            {
                lock (_lockerObj)
                {
                    _dataRefreshing = false;
                }
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion Expnl Messages Handling received from Clients

        private void HandleTaxLotsRequest(string msg)
        {
            try
            {
                string userID = string.Empty;
                string groupedRowID = string.Empty;
                int accountID = 0;
                List<int> accountList = new List<int>();
                string callerGridName = string.Empty;
                PranaMessageFormatter.FromClientTaxLotRequest(msg, ref userID, ref groupedRowID, ref callerGridName, ref accountID, ref accountList);
                _taxlotReqCallerGridName = callerGridName;
                int distinctAccountPermissionKey = SessionManager.DistinctAccountPermissionSets.FirstOrDefault(x => IsContentEqualInLists(x.Value, accountList)).Key;
                SendTaxlotDataToUsers(userID, groupedRowID, accountID, distinctAccountPermissionKey);
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

        private string ConvertToString(List<string> columnNames)
        {
            try
            {
                StringBuilder columnNamesString = new StringBuilder();
                foreach (string columnName in columnNames)
                {
                    columnNamesString.Append(columnName);
                    columnNamesString.Append(",");
                }
                columnNamesString.Remove(columnNamesString.Length - 1, 1);
                return columnNamesString.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        private void GetExposurePnlCacheItemListChunks(ref List<ExposurePnlCacheItemList> listToSndToClients, ExposurePnlCacheItemList epnlOrderCollection, int chunkSize)
        {
            try
            {
                ExposurePnlCacheItemList cacheItemListToSendToClient = new ExposurePnlCacheItemList();
                listToSndToClients.Add(cacheItemListToSendToClient);
                if (epnlOrderCollection != null)
                {
                    for (int i = 0, counterForChunking = 0; i < epnlOrderCollection.Count; i++, counterForChunking++)
                    {
                        ExposurePnlCacheItem itemToSendToClient = new ExposurePnlCacheItem();
                        itemToSendToClient = epnlOrderCollection[i];
                        if (itemToSendToClient.LeveragedFactor.Equals(0D) && itemToSendToClient.Asset.Equals(AssetCategory.Equity.ToString()))
                        {
                            string message = "Leverage Factor zero for Equity ExposurePnlCacheItem. Details :" + Environment.NewLine + LogExtensions.ToLoggerString(itemToSendToClient);
                            Logger.LoggerWrite(message, LoggingConstants.CATEGORY_GENERAL);
                        }
                        if (counterForChunking < chunkSize)
                        {
                            cacheItemListToSendToClient.Add(itemToSendToClient);
                        }
                        else
                        {
                            counterForChunking = -1;
                            cacheItemListToSendToClient = new ExposurePnlCacheItemList();
                            cacheItemListToSendToClient.Add(itemToSendToClient);
                            listToSndToClients.Add(cacheItemListToSendToClient);
                        }
                        //this is needed as the new Order now being sent to user will only send it's dynamic data from Next time
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

        #region Normal Workflow while only Dynamic Data is Sent

        private void SendControlMessageToClient(string ctrlMsg, List<string> freeUsers)
        {
            try
            {
                if (freeUsers.Count == 0)
                {
                    return;
                }
                if (ctrlMsg != string.Empty)
                {
                    if (ctrlMsg == PranaMessageConstants.MSG_ExpPNLDynamicColumnList)
                    {
                        StringBuilder dynamicColumnList = new StringBuilder();
                        foreach (string dynamicColumn in _dynamicFields)
                        {
                            dynamicColumnList.Append(dynamicColumn);
                            dynamicColumnList.Append(Seperators.SEPERATOR_2);
                        }
                        QueueMessage queueMessage = new QueueMessage(ctrlMsg, freeUsers, dynamicColumnList.ToString());
                        _outQueueClientBroadCasting.SendMessage(queueMessage);
                    }
                    else if (ctrlMsg == PranaMessageConstants.MSG_IndicesReturnSummary)
                    {
                        if (_exPNLCache.DTIndicesReturn.Rows.Count > 0)
                        {
                            QueueMessage queueMessageSummary = new QueueMessage(ctrlMsg, freeUsers, binaryFormatter.Serialize(_exPNLCache.DTIndicesReturn));
                            _outQueueClientBroadCasting.SendMessage(queueMessageSummary);
                        }
                    }
                    else if (ctrlMsg == PranaMessageConstants.MSG_ExpPNLIndexColumns)
                    {
                        QueueMessage queueMessage = new QueueMessage(ctrlMsg, freeUsers, binaryFormatter.Serialize(_exPNLCache.DTIndicesReturn));
                        _outQueueClientBroadCasting.SendMessage(queueMessage);
                    }
                    else if (ctrlMsg == PranaMessageConstants.MSG_EPNlTaxLotListEnd)
                    {
                        QueueMessage queueMessage = new QueueMessage(ctrlMsg, freeUsers, _taxlotReqCallerGridName);
                        _outQueueClientBroadCasting.SendMessage(queueMessage);
                    }
                    else if (ctrlMsg == PranaMessageConstants.MSG_EPNlTaxLotListStart)
                    {
                        QueueMessage queueMessage = new QueueMessage(ctrlMsg, freeUsers, _compressedRowID);
                        _outQueueClientBroadCasting.SendMessage(queueMessage);
                    }
                    else
                    {
                        QueueMessage queueMessage = new QueueMessage(ctrlMsg, freeUsers, String.Empty);
                        _outQueueClientBroadCasting.SendMessage(queueMessage);
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

        private bool SendDashBoardData(bool isStartUpData)
        {
            bool isDashboardDataSent = false;
            try
            {
                lock (_lockerObj)
                {
                    foreach (KeyValuePair<int, DistinctAccountSetWiseSummaryCollection> accountSummary in _compressionComponent[0].GetCalculatedSummaries().OutputAccountSetWiseConsolidatedSummary)
                    {
                        foreach (KeyValuePair<string, List<int>> connectedUser in _exPNLSubscriptionLookup)
                        {
                            if (!connectedUser.Key.Equals(ApplicationConstants.TRADE_SERVER_ID_FOR_EXPNL) && !connectedUser.Key.Equals(ApplicationConstants.TRADE_SERVER_UI_ID_FOR_EXPNL) && SessionManager.DynamicDistinctAccountPermissionSets.Values.Contains(accountSummary.Key))
                            {
                                int dynamicDistinctSummaryKey = SessionManager.DynamicDistinctAccountPermissionSets.FirstOrDefault(x => x.Value == accountSummary.Key).Key;
                                if (SessionManager.DynamicDistinctAccountPermissionSetsAndUsersMapping.ContainsKey(dynamicDistinctSummaryKey))
                                {
                                    List<Tuple<string, string>> userAndTabList = SessionManager.DynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctSummaryKey];
                                    if (userAndTabList.Any(userAndTab => userAndTab.Item2 == connectedUser.Key))
                                    {
                                        if (_instantDataRequestManager.ExPNLUserStatus.ContainsKey(connectedUser.Key))
                                        {
                                            if (!_instantDataRequestManager.ExPNLUserStatus[connectedUser.Key].UserBusyStatus)
                                            {
                                                if (isStartUpData && _dictUserWiseStartUpDataSentSuccessfully.ContainsKey(connectedUser.Key) && _dictUserWiseStartUpDataSentSuccessfully[connectedUser.Key])
                                                {
                                                    continue;
                                                }
                                                IEnumerable<Tuple<string, string>> userAndTabTupleList = userAndTabList.Where(userAndTab => userAndTab.Item2 == connectedUser.Key);
                                                foreach (Tuple<string, string> userAndTabTuple in userAndTabTupleList)
                                                {
                                                    if (_dictUserWisePermissions.ContainsKey(userAndTabTuple.Item2) && _dictUserWisePermissions[userAndTabTuple.Item2].IsMarketDataPermissionEnabled)
                                                    {
                                                        accountSummary.Value.TabKey = userAndTabTuple.Item1;
                                                        SendAccountSummaryChunkToClient(accountSummary.Value, new List<string> { userAndTabTuple.Item2 });
                                                    }
                                                    else
                                                    {
                                                        DistinctAccountSetWiseSummaryCollection zeroSummary = new DistinctAccountSetWiseSummaryCollection();
                                                        zeroSummary.TabKey = userAndTabTuple.Item1;
                                                        SendAccountSummaryChunkToClient(zeroSummary, new List<string> { userAndTabTuple.Item2 });
                                                    }
                                                    isDashboardDataSent = true;
                                                }
                                            }
                                        }
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
            return isDashboardDataSent;
        }

        private void SendAccountSummaryChunkToClient(DistinctAccountSetWiseSummaryCollection summaries, List<string> freeUsers)
        {
            try
            {
                if (summaries != null)
                {
                    QueueMessage queueMessageSummary = new QueueMessage(PranaMessageConstants.MSG_ExpPNLCalcSummary, freeUsers, binaryFormatter.Serialize(summaries));
                    _outQueueClientBroadCasting.SendMessage(queueMessageSummary);
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

        private void SendContinuousDataToUsers()
        {
            try
            {
                lock (_lockerObj)
                {
                    if (_clientBroadCastingManager.ConnectedUsers.Count > 0)
                    {
                        _instantDataRequestManager.ProcessTimeSW.Start();
                        if (_compressionComponent[0].GetCompressedData() == null)
                        {
                            return;
                        }
                        _isSending = true;

                        Dictionary<int, ExposurePnlCacheItemList> calculatedCompressedOrders = _compressionComponent[0].GetCompressedData().OutputCompressedData;
                        List<ExposurePnlCacheItemList> listOfOrdersToSendToClient = new List<ExposurePnlCacheItemList>();
                        List<string> currentlyFreeUsers = new List<string>();

                        foreach (KeyValuePair<string, List<int>> connectedUser in _exPNLSubscriptionLookup.Where(x => !x.Key.Equals(ApplicationConstants.TRADE_SERVER_ID_FOR_EXPNL) && !x.Key.Equals(ApplicationConstants.TRADE_SERVER_UI_ID_FOR_EXPNL)))
                        {
                            if (_instantDataRequestManager.ExPNLUserStatus.ContainsKey(connectedUser.Key))
                            {
                                if (!_instantDataRequestManager.ExPNLUserStatus[connectedUser.Key].UserBusyStatus)
                                {
                                    lock (_lockForSelectedColumnList)
                                    {
                                        //In case of Dynamic Data, if selected columns list is not ready for a client then we are not sending any message to user
                                        if (!ViewManager.GetInstance().DicUserPMPreference.ContainsKey(connectedUser.Key) && _dictUserWiseStartUpDataSentSuccessfully.ContainsKey(connectedUser.Key) && _dictUserWiseStartUpDataSentSuccessfully[connectedUser.Key])
                                            continue;
                                        if (ViewManager.GetInstance().DicUserPMPreference.ContainsKey(connectedUser.Key) && ViewManager.GetInstance().DicUserPMPreference[connectedUser.Key] == null && _dictUserWiseStartUpDataSentSuccessfully.ContainsKey(connectedUser.Key) && _dictUserWiseStartUpDataSentSuccessfully[connectedUser.Key])
                                            continue;
                                        if (_dictUserWiseStartUpDataSentSuccessfully.ContainsKey(connectedUser.Key) && !_dictUserWiseStartUpDataSentSuccessfully[connectedUser.Key])
                                            continue;
                                    }
                                    currentlyFreeUsers.Add(connectedUser.Key);
                                }
                            }
                        }
                        if (currentlyFreeUsers.Count == 0)
                        {
                            _isSending = false;
                            return;
                        }

                        #region Sending Messages to Client
                        SendControlMessageToClient(PranaMessageConstants.MSG_ExpPNLStartOfMessage, currentlyFreeUsers);

                        #region Sending Dashboard Data
                        bool isDashboardDataSent = SendDashBoardData(false);
                        #endregion Sending Dashboard Data

                        #region Sending Grid Data
                        if (isDashboardDataSent)
                        {
                            foreach (KeyValuePair<int, ExposurePnlCacheItemList> temp in calculatedCompressedOrders)
                            {
                                GetExposurePnlCacheItemListChunks(ref listOfOrdersToSendToClient, temp.Value, _chunkSize);

                                List<string> permissionedFreeUsersForAccount = new List<string>();
                                foreach (KeyValuePair<string, List<int>> connectedUser in _exPNLSubscriptionLookup)
                                {
                                    int key = temp.Key;

                                    //check whether the user have the permissions for this account or not
                                    if (connectedUser.Value.Contains(key))
                                    {
                                        if (_instantDataRequestManager.ExPNLUserStatus.ContainsKey(connectedUser.Key))
                                        {
                                            if (!_instantDataRequestManager.ExPNLUserStatus[connectedUser.Key].UserBusyStatus)
                                            {
                                                permissionedFreeUsersForAccount.Add(connectedUser.Key);
                                            }
                                        }
                                    }
                                }

                                if (listOfOrdersToSendToClient.Count > 0)
                                {
                                    SendOrdersToClient(ref listOfOrdersToSendToClient, permissionedFreeUsersForAccount, true, PranaMessageConstants.MSG_EPNlItemList, null);
                                    listOfOrdersToSendToClient.Clear();
                                }
                                temp.Value.IsUpdated = false;
                            }
                        }
                        #endregion Sending Grid Data

                        foreach (string freeUser in currentlyFreeUsers)
                        {
                            _instantDataRequestManager.SetUserFreeUpTime(freeUser, true);
                        }

                        if (_exPNLCache.DTIndicesReturn != null)
                        {
                            SendControlMessageToClient(PranaMessageConstants.MSG_IndicesReturnSummary, currentlyFreeUsers);
                        }
                        SendControlMessageToClient(PranaMessageConstants.MSG_ExpPNLEndOfMessage, currentlyFreeUsers);
                        #endregion Sending Messages to Client

                        if (_dataRefreshing)
                        {
                            SendRefreshResponse(currentlyFreeUsers);
                            _dataRefreshing = false;
                        }
                        _isSending = false;
                        //Resetting the state so that new/updated orders completly sent only once.
                        DataManager.GetInstance().ResetUncalculatedDataState();
                        _instantDataRequestManager.ProcessTime = _instantDataRequestManager.ProcessTimeSW.ElapsedMilliseconds;
                        _instantDataRequestManager.ProcessTimeSW.Reset();
                        _instantDataRequestManager.ProcessTimeSW.Stop();
                    }
                    _isSending = false;
                    if (_dataRefreshing)
                    {
                        _dataRefreshing = false;
                        if (UserDataRefreshCompleted != null)
                            UserDataRefreshCompleted(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (_lockerObj)
                {
                    _instantDataRequestManager.ProcessTimeSW.Reset();
                    _instantDataRequestManager.ProcessTimeSW.Stop();
                    List<string> connectedUserList = new List<string>((IEnumerable<string>)_exPNLSubscriptionLookup.Keys);
                    if (true == _dataRefreshing)
                    {
                        SendRefreshResponse(connectedUserList);
                        _dataRefreshing = false;
                    }
                    _isSending = false;
                }
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SendInstantDataToUsers(List<string> columnNames, string userID)
        {
            try
            {
                lock (_lockerObj)
                {
                    Dictionary<int, ExposurePnlCacheItemList> calculatedCompressedOrders = new Dictionary<int, ExposurePnlCacheItemList>();
                    List<string> users = new List<string>();
                    users.Add(userID);
                    if (_compressionComponent[0].GetCompressedData() != null && _compressionComponent != null && _compressionComponent.Count > 0 && _compressionComponent[0] != null)
                    {
                        calculatedCompressedOrders = _compressionComponent[0].GetCompressedData().OutputCompressedData;
                    }
                    else
                    {
                        string logMessage = "Either compression component or it's data is null.";
                        LogAndDisplayOnInformationReporter.GetInstance.Write(logMessage, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }

                    List<ExposurePnlCacheItemList> listOfOrdersToSendToClient = new List<ExposurePnlCacheItemList>();
                    if (columnNames != null)
                    {
                        columnNames.Insert(0, "HasBeenSentToUser");
                        columnNames.Insert(1, "ID");
                    }
                    SendControlMessageToClient(PranaMessageConstants.MSG_ExpPNLStartOfMessage, users);
                    foreach (KeyValuePair<int, ExposurePnlCacheItemList> temp in calculatedCompressedOrders)
                    {
                        // need to make listoforderstosendtoclient by account wise orders i.e make it dictionary.
                        GetExposurePnlCacheItemListChunks(ref listOfOrdersToSendToClient, temp.Value, _chunkSize);
                        if (listOfOrdersToSendToClient.Count > 0)
                        {
                            // make a logic here to check about user is subscribed to which accounts
                            List<string> PermissionedFreeUsersForThisAccount = new List<string>();

                            if (_exPNLSubscriptionLookup.ContainsKey(userID) && _exPNLSubscriptionLookup[userID].Contains(temp.Key))
                                PermissionedFreeUsersForThisAccount.Add(userID);
                            SendOrdersToClient(ref listOfOrdersToSendToClient, PermissionedFreeUsersForThisAccount, true, PranaMessageConstants.MSG_EPNlItemList, columnNames);
                            listOfOrdersToSendToClient.Clear();
                        }
                        temp.Value.IsUpdated = false;
                    }
                    SendControlMessageToClient(PranaMessageConstants.MSG_ExpPNLEndOfMessage, users);
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

        private void SendTaxlotDataToUsers(string userID, string compressedRowID, int accountID, int distinctAccountPermissionKey)
        {
            try
            {
                if (_clientBroadCastingManager.ConnectedUsers.Count > 0)
                {
                    if (compressedRowID != string.Empty)
                    {
                        _compressedRowID = compressedRowID;
                        List<ExposurePnlCacheItemList> listOftaxLotsToSendToClient = new List<ExposurePnlCacheItemList>();
                        ExposurePnlCacheItemList taxlotList = _compressionComponent[0].GetContainingTaxLots(compressedRowID, accountID, distinctAccountPermissionKey);

                        GetExposurePnlCacheItemListChunks(ref listOftaxLotsToSendToClient, taxlotList, _chunkSize);
                        List<string> clientToSendTaxLots = new List<string>();
                        clientToSendTaxLots.Add(userID);
                        SendControlMessageToClient(PranaMessageConstants.MSG_EPNlTaxLotListStart, clientToSendTaxLots);
                        SendOrdersToClient(ref listOftaxLotsToSendToClient, clientToSendTaxLots, false, PranaMessageConstants.MSG_EPNlTaxLotList, null);
                        SendControlMessageToClient(PranaMessageConstants.MSG_EPNlTaxLotListEnd, clientToSendTaxLots);
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

        private void SendStartUpDataToClient(string userID)
        {
            try
            {
                List<string> freeUsers = new List<string>();
                freeUsers.Add(userID);

                if (_exPNLCache.DTIndicesReturn != null)
                {
                    SendControlMessageToClient(PranaMessageConstants.MSG_ExpPNLIndexColumns, freeUsers);
                }

                Dictionary<int, ExposurePnlCacheItemList> compressedOrdersData = null;
                CompressedDataDictionaries compressedDataDictionaries = null;

                if (_compressionComponent != null && _compressionComponent.Count > 0 && _compressionComponent[0] != null)
                {
                    compressedDataDictionaries = _compressionComponent[0].GetCompressedData();
                    if (compressedDataDictionaries != null)
                    {
                        compressedOrdersData = compressedDataDictionaries.OutputCompressedData;
                    }
                }

                if (compressedOrdersData != null)
                {
                    if (_exPNLCache.DTIndicesReturn != null)
                    {
                        SendControlMessageToClient(PranaMessageConstants.MSG_ExpPNLIndexColumns, freeUsers);
                    }
                    SendControlMessageToClient(PranaMessageConstants.MSG_ExpPNLStartOfMessage, freeUsers);

                    #region Sending Dashboard Data
                    bool isDashboardDataSent = SendDashBoardData(true);
                    #endregion Sending Dashboard Data

                    #region Sending Grid Data
                    if (isDashboardDataSent)
                    {
                        List<int> listOfAllowedaccounts = _dictUserWisePermissions[userID].AllowedAccounts;
                        foreach (int accountID in listOfAllowedaccounts)
                        {
                            if (compressedOrdersData.ContainsKey(accountID))
                            {
                                ExposurePnlCacheItemList temp = compressedOrdersData[accountID];
                                List<ExposurePnlCacheItemList> listOfOrdersToSendToClient = new List<ExposurePnlCacheItemList>();

                                if (temp.Count > 0)
                                {
                                    GetExposurePnlCacheItemListChunks(ref listOfOrdersToSendToClient, temp, _chunkSize);
                                }
                                SendOrdersToClient(ref listOfOrdersToSendToClient, freeUsers, false, PranaMessageConstants.MSG_EPNlItemList, null);
                            }
                        }
                    }
                    #endregion Sending Grid Data

                    lock (_lockerObj)
                    {
                        _instantDataRequestManager.SetUserFreeUpTime(userID, true);
                    }
                    if (_exPNLCache.DTIndicesReturn != null && isDashboardDataSent)
                    {
                        SendControlMessageToClient(PranaMessageConstants.MSG_IndicesReturnSummary, freeUsers);
                    }
                    SendControlMessageToClient(PranaMessageConstants.MSG_ExpPNLEndOfMessage, freeUsers);
                    if (isDashboardDataSent)
                    {
                        _dictUserWiseStartUpDataSentSuccessfully[userID] = true;
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

        private void SendOrdersToClient(ref List<ExposurePnlCacheItemList> exposurePnlCacheItemList, List<string> freeUsers, bool isDynamicData, string msgType, List<string> columnNames)
        {
            try
            {
                if (freeUsers.Count == 0)
                {
                    return;
                }
                // Lock Must be start from here
                lock (_lockForSelectedColumnList)
                {
                    Dictionary<string, string> DicUserWiseHeaderdMsg;

                    // Sending header to client for each free user.
                    foreach (string freeUser in freeUsers)
                    {
                        QueueMessage queueMessage = null;
                        DicUserWiseHeaderdMsg = new Dictionary<string, string>();
                        // In case of instant data request header is being sent only for the columns added.
                        if (columnNames != null)
                        {
                            DicUserWiseHeaderdMsg.Add(freeUser, ConvertToString(columnNames));
                            queueMessage = new QueueMessage(PranaMessageConstants.MSG_Header, DicUserWiseHeaderdMsg);
                        }
                        else
                        {
                            if (ViewManager.GetInstance().DicUserPMPreference.ContainsKey(freeUser))
                            {
                                DicUserWiseHeaderdMsg.Add(freeUser, ConvertToString(ViewManager.GetInstance().DicUserPMPreference[freeUser].DynamicColumnsToUpdate));
                                if (Prana.BusinessObjects.UserSettingConstants.IsDebugModeEnabled)
                                {
                                    string username = freeUser;
                                    if (CachedDataManager.GetInstance.GetAllUsersName().ContainsKey(Convert.ToInt32(freeUser)))
                                    {
                                        username = CachedDataManager.GetInstance.GetAllUsersName()[Convert.ToInt32(freeUser)];
                                    }
                                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Sending data to '" + username + "' for View : '" + ViewManager.GetInstance().DicUserPMPreference[freeUser].SelectedView.Replace("CustomView_", "") + "' and Columns are :" + DicUserWiseHeaderdMsg[freeUser], LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                }
                                queueMessage = new QueueMessage(PranaMessageConstants.MSG_Header, DicUserWiseHeaderdMsg);
                            }
                        }
                        if (queueMessage != null)
                            _outQueueClientBroadCasting.SendMessage(queueMessage);
                        queueMessage = null;
                        DicUserWiseHeaderdMsg = null;
                    }

                    List<string> newIDsSent = new List<string>();
                    if (isDynamicData)
                    {
                        Dictionary<string, StringBuilder> dictUsersDynamicData;
                        foreach (ExposurePnlCacheItemList exposurePnlCacheItemcollection in exposurePnlCacheItemList)
                        {
                            dictUsersDynamicData = new Dictionary<string, StringBuilder>();
                            foreach (ExposurePnlCacheItem item in exposurePnlCacheItemcollection)
                            {
                                if (_lsItemIDsToSendCompleteData != null && _lsItemIDsToSendCompleteData.Contains(item.ID))
                                {
                                    item.HasBeenSentToUser = 0;
                                    _lsItemIDsToSendCompleteData.Remove(item.ID);
                                }
                                foreach (string freeUser in freeUsers)
                                {
                                    if (ViewManager.GetInstance().DicUserPMPreference.ContainsKey(freeUser))
                                    {
                                        if (!dictUsersDynamicData.ContainsKey(freeUser))
                                            dictUsersDynamicData.Add(freeUser, new StringBuilder());

                                        if (item.HasBeenSentToUser == 1)
                                        {
                                            if (ViewManager.GetInstance().DicUserPMPreference[freeUser].DynamicColumnsToUpdate != null)
                                            {
                                                if (_dictUserWisePermissions.ContainsKey(freeUser) && _dictUserWisePermissions[freeUser].IsMarketDataPermissionEnabled)
                                                {
                                                    // list columnNames will be NULL in case if we need to send data for all dynamic column, and then we get them from Dynamic column list.
                                                    if (columnNames == null)
                                                    {
                                                        dictUsersDynamicData[freeUser].Append(item.GetDynamicDataString(ViewManager.GetInstance().DicUserPMPreference[freeUser].DynamicColumnsToUpdate));
                                                    }
                                                    else
                                                    {
                                                        // It is the case for Instant data request raised for Column Addition.
                                                        dictUsersDynamicData[freeUser].Append(item.GetDynamicDataString(columnNames));
                                                    }
                                                }
                                                else
                                                {
                                                    if (columnNames == null)
                                                    {
                                                        dictUsersDynamicData[freeUser].Append(item.GetDynamicDataString2(ViewManager.GetInstance().DicUserPMPreference[freeUser].DynamicColumnsToUpdate, _listColumnDataAsZero));
                                                    }
                                                    else
                                                    {
                                                        // It is the case for Instant data request raised for Column Addition.
                                                        dictUsersDynamicData[freeUser].Append(item.GetDynamicDataString2(columnNames, _listColumnDataAsZero));
                                                    }
                                                }
                                                dictUsersDynamicData[freeUser].Append(Seperators.SEPERATOR_4);
                                            }
                                        }
                                        else
                                        {
                                            newIDsSent.Add(item.ID);
                                            if (_dictUserWisePermissions.ContainsKey(freeUser) && _dictUserWisePermissions[freeUser].IsMarketDataPermissionEnabled)
                                            {
                                                dictUsersDynamicData[freeUser].Append(item.ToString());
                                            }
                                            else
                                            {
                                                dictUsersDynamicData[freeUser].Append(item.ApplyMarketDataPermission(_listColumnDataAsZero).ToString());
                                            }
                                            dictUsersDynamicData[freeUser].Append(Seperators.SEPERATOR_4);
                                        }
                                    }
                                }
                            }

                            if (dictUsersDynamicData.Count > 0)
                            {
                                byte[] byteArray = new byte[1];
                                Dictionary<string, string> DicUserCompressedMsg = new Dictionary<string, string>();
                                foreach (string userID in dictUsersDynamicData.Keys)
                                    DicUserCompressedMsg.Add(userID, CompressionHelper.Zip(dictUsersDynamicData[userID].ToString(), byteArray));
                                byteArray = null;

                                QueueMessage queueMessage = new QueueMessage(msgType, DicUserCompressedMsg);
                                _outQueueClientBroadCasting.SendMessage(queueMessage);
                                DicUserCompressedMsg = null;
                                queueMessage = null;
                                dictUsersDynamicData = null;
                            }
                        }
                        if (newIDsSent.Count > 0)
                            ExPnlCache.Instance.UpdateNewlySentOrderStatus();
                    }
                    else
                    {
                        Dictionary<string, StringBuilder> dicUsersData = new Dictionary<string, StringBuilder>();
                        foreach (ExposurePnlCacheItemList exposurePnlCacheItemcollection in exposurePnlCacheItemList)
                        {
                            foreach (ExposurePnlCacheItem item in exposurePnlCacheItemcollection)
                            {
                                foreach (string freeUser in freeUsers)
                                {
                                    item.HasBeenSentToUser = 0;

                                    if (_dictUserWisePermissions.ContainsKey(freeUser) && _dictUserWisePermissions[freeUser].IsMarketDataPermissionEnabled)
                                    {
                                        if (dicUsersData.ContainsKey(freeUser))
                                        {
                                            dicUsersData[freeUser].Append(item.ToString());
                                            dicUsersData[freeUser].Append(Seperators.SEPERATOR_4);
                                        }
                                        else
                                        {
                                            dicUsersData.Add(freeUser, new StringBuilder());
                                            dicUsersData[freeUser].Append(item.ToString());
                                            dicUsersData[freeUser].Append(Seperators.SEPERATOR_4);
                                        }
                                    }
                                    else
                                    {
                                        if (dicUsersData.ContainsKey(freeUser))
                                        {
                                            dicUsersData[freeUser].Append(item.ApplyMarketDataPermission(_listColumnDataAsZero).ToString());
                                            dicUsersData[freeUser].Append(Seperators.SEPERATOR_4);
                                        }
                                        else
                                        {
                                            dicUsersData.Add(freeUser, new StringBuilder());
                                            dicUsersData[freeUser].Append(item.ApplyMarketDataPermission(_listColumnDataAsZero).ToString());
                                            dicUsersData[freeUser].Append(Seperators.SEPERATOR_4);
                                        }
                                    }
                                }
                            }
                            if (dicUsersData.Count > 0)
                            {
                                byte[] byteArray = new byte[1];
                                Dictionary<string, string> DicUserCompressedMsg = new Dictionary<string, string>();
                                foreach (string userID in dicUsersData.Keys)
                                {
                                    DicUserCompressedMsg.Add(userID, CompressionHelper.Zip(dicUsersData[userID].ToString(), byteArray));
                                }
                                byteArray = null;
                                QueueMessage queueMessage = new QueueMessage(msgType, DicUserCompressedMsg);
                                _outQueueClientBroadCasting.SendMessage(queueMessage);
                                queueMessage = null;
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

        #endregion Normal Workflow while only Dynamic Data is Sent

        #region Properties And Methods for updating calculation Parameters While Running

        public void UpdateCalculationInterval(int calculationRefreshInterval)
        {
            try
            {
                if (_exPNLCache != null)
                {
                    _exPNLCache.UpdateCalculationInterval(calculationRefreshInterval);
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

        public void UpdateClearance(Dictionary<int, DateTime> updatedClearanceTime)
        {
            try
            {
                ExPnlCache.Instance.UpdateClearance(updatedClearanceTime);
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

        #endregion Properties And Methods for updating calculation Parameters While Running

        #region Methods and properties for Dynamic Columns

        private static List<string> _dynamicFields = new List<string>();
        private void FillDynamicColumnListFromXMLFile()
        {
            try
            {
                _dynamicFields.Clear();
                string xmlpath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\DynamicColumns.xml";

                XmlTextReader myxmlreader = new XmlTextReader(xmlpath);
                _dynamicFields = new List<string>();
                while (myxmlreader.Read())
                {
                    myxmlreader.MoveToContent();

                    if (myxmlreader.NodeType == XmlNodeType.Element)
                    {
                        switch (myxmlreader.Name)
                        {
                            case "FieldName":
                                string dynamicColumnName = myxmlreader.ReadString();
                                if (!_dynamicFields.Contains(dynamicColumnName))
                                {
                                    _dynamicFields.Add(dynamicColumnName);
                                }
                                break;
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
        #endregion

        #region Methods and properties for ColumnData As Zero
        private static List<string> _listColumnDataAsZero = new List<string>();
        private void FillColumnDataAsZeroListFromXMLFile()
        {
            try
            {
                _listColumnDataAsZero.Clear();

                _listColumnDataAsZero.Add("FxRate");
                _listColumnDataAsZero.Add("AskPrice");
                _listColumnDataAsZero.Add("BidPrice");
                _listColumnDataAsZero.Add("DividendYield");
                _listColumnDataAsZero.Add("LastPrice");
                _listColumnDataAsZero.Add("ClosingPrice");
                _listColumnDataAsZero.Add("HighPrice");
                _listColumnDataAsZero.Add("LowPrice");
                _listColumnDataAsZero.Add("MidPrice");
                _listColumnDataAsZero.Add("PercentageChange");
                _listColumnDataAsZero.Add("NetExposure");
                _listColumnDataAsZero.Add("Exposure");
                _listColumnDataAsZero.Add("DeltaAdjPosition");
                _listColumnDataAsZero.Add("NetExposureInBaseCurrency");
                _listColumnDataAsZero.Add("ExposureInBaseCurrency");
                _listColumnDataAsZero.Add("BetaAdjExposure");
                _listColumnDataAsZero.Add("BetaAdjExposureInBaseCurrency");
                _listColumnDataAsZero.Add("ExposureBPInBaseCurrency");
                _listColumnDataAsZero.Add("DayPnL");
                _listColumnDataAsZero.Add("DayPnLBP");
                _listColumnDataAsZero.Add("DayPnLInBaseCurrency");
                _listColumnDataAsZero.Add("Delta");
                _listColumnDataAsZero.Add("UnderlyingStockPrice");
                _listColumnDataAsZero.Add("DayInterest");
                _listColumnDataAsZero.Add("TotalInterest");
                _listColumnDataAsZero.Add("CostBasisUnrealizedPnLInBaseCurrency");
                _listColumnDataAsZero.Add("MarketValue");
                _listColumnDataAsZero.Add("MarketValueInBaseCurrency");
                _listColumnDataAsZero.Add("PercentageGainLoss");
                _listColumnDataAsZero.Add("Beta");
                _listColumnDataAsZero.Add("CashImpact");
                _listColumnDataAsZero.Add("MarketCapitalization");
                _listColumnDataAsZero.Add("SharesOutstanding");
                _listColumnDataAsZero.Add("AverageVolume20Day");
                _listColumnDataAsZero.Add("PercentageAverageVolumeDeltaAdjusted");
                _listColumnDataAsZero.Add("CashImpactInBaseCurrency");
                _listColumnDataAsZero.Add("Volatility");
                _listColumnDataAsZero.Add("GrossExposure");
                _listColumnDataAsZero.Add("BetaAdjGrossExposure");
                _listColumnDataAsZero.Add("GrossMarketValue");
                _listColumnDataAsZero.Add("SelectedFeedPrice");
                _listColumnDataAsZero.Add("SelectedFeedPriceInBaseCurrency");
                _listColumnDataAsZero.Add("PercentageAverageVolume");
                _listColumnDataAsZero.Add("TradeDayPnl");
                _listColumnDataAsZero.Add("FxDayPnl");
                _listColumnDataAsZero.Add("FxCostBasisPnl");
                _listColumnDataAsZero.Add("TradeCostBasisPnl");
                _listColumnDataAsZero.Add("PercentageGainLossCostBasis");
                _listColumnDataAsZero.Add("UnderlyingValueForOptions");
                _listColumnDataAsZero.Add("AverageVolume20DayUnderlyingSymbol");
                _listColumnDataAsZero.Add("GrossExposureLocal");
                _listColumnDataAsZero.Add("PercentDayPnLGrossMV");
                _listColumnDataAsZero.Add("PercentDayPnLNetMV");
                _listColumnDataAsZero.Add("DeltaAdjPositionLME");
                _listColumnDataAsZero.Add("Premium");
                _listColumnDataAsZero.Add("PremiumDollar");
                _listColumnDataAsZero.Add("PercentageUnderlyingChange");
                _listColumnDataAsZero.Add("YesterdayMarketValue");
                _listColumnDataAsZero.Add("YesterdayMarketValueInBaseCurrency");
                _listColumnDataAsZero.Add("YesterdayFXRate");
                _listColumnDataAsZero.Add("ChangeInUnderlyingPrice");
                _listColumnDataAsZero.Add("FxRateDisplay");
                _listColumnDataAsZero.Add("NetNotionalForCostBasisBreakEven");
                _listColumnDataAsZero.Add("PercentNetExposureInBaseCurrency");
                _listColumnDataAsZero.Add("PercentGrossExposureInBaseCurrency");
                _listColumnDataAsZero.Add("PercentExposureInBaseCurrency");
                _listColumnDataAsZero.Add("PercentUnderlyingGrossExposureInBaseCurrency");
                _listColumnDataAsZero.Add("PercentBetaAdjGrossExposureInBaseCurrency");
                _listColumnDataAsZero.Add("UnderlyingGrossExposureInBaseCurrency");
                _listColumnDataAsZero.Add("UnderlyingGrossExposure");
                _listColumnDataAsZero.Add("DayReturn");
                _listColumnDataAsZero.Add("StartOfDayNAV");
                _listColumnDataAsZero.Add("PercentGrossMarketValueInBaseCurrency");
                _listColumnDataAsZero.Add("BetaAdjGrossExposureUnderlying");
                _listColumnDataAsZero.Add("BetaAdjGrossExposureUnderlyingInBaseCurrency");
                _listColumnDataAsZero.Add("NAV");
                _listColumnDataAsZero.Add("PercentagePNLContribution");
                _listColumnDataAsZero.Add("PercentNetMarketValueInBaseCurrency");
                _listColumnDataAsZero.Add("PositionSideExposure");
                _listColumnDataAsZero.Add("PositionSideMV");
                _listColumnDataAsZero.Add("NavTouch");
                _listColumnDataAsZero.Add("NetNotionalValue");
                _listColumnDataAsZero.Add("NetNotionalValueInBaseCurrency");
                _listColumnDataAsZero.Add("PricingSource");
                _listColumnDataAsZero.Add("PricingStatus");
                _listColumnDataAsZero.Add("PositionSideExposureBoxed");
                _listColumnDataAsZero.Add("LastUpdatedUTC");
                _listColumnDataAsZero.Add("LeveragedFactor");
                _listColumnDataAsZero.Add("DeltaSource");
                _listColumnDataAsZero.Add("EarnedDividendBase");
                _listColumnDataAsZero.Add("EarnedDividendLocal");
                _listColumnDataAsZero.Add("CostBasisUnrealizedPnL");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region ILiveFeedCallback Members
        public void LiveFeedConnected()
        {
        }

        public void LiveFeedDisConnected()
        {
        }

        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }
        #endregion ILiveFeedCallback Members

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_clientBroadCastingManager != null)
                    _clientBroadCastingManager.Dispose();
                if (_exPNLCache != null)
                    _exPNLCache.Dispose();
                if (_pricingServiceProxy != null)
                    _pricingServiceProxy.Dispose();
                if (_marketDataPermissionServiceProxy != null)
                    _marketDataPermissionServiceProxy.Dispose();
            }
        }
        #endregion IDisposable Members

        internal void SendInstantDynamicFilterData(List<string> subMsgs, string userID)
        {
            try
            {
                lock (_lockerObj)
                {
                    if (_compressionComponent[0].GetCompressedData() != null && _compressionComponent != null && _compressionComponent.Count > 0 && _compressionComponent[0] != null)
                    {
                        List<string> users = new List<string>();
                        users.Add(userID);
                        Dictionary<int, ExposurePnlCacheItemList> calculatedCompressedOrders = _compressionComponent[0].GetCompressedData().OutputCompressedData;
                        List<ExposurePnlCacheItemList> listOfOrdersToSendToClient = new List<ExposurePnlCacheItemList>();
                        SendControlMessageToClient(PranaMessageConstants.MSG_ExpPNLStartOfMessage, users);

                        if (calculatedCompressedOrders != null)
                        {
                            foreach (KeyValuePair<int, ExposurePnlCacheItemList> temp in calculatedCompressedOrders)
                            {
                                // need to make listoforderstosendtoclient by account wise orders i.e make it dictionary.
                                GetExposurePnlCacheItemListChunks(ref listOfOrdersToSendToClient, temp.Value, _chunkSize);
                                if (listOfOrdersToSendToClient.Count > 0)
                                {
                                    // make a logic here to check about user is subscribed to which accounts
                                    List<string> PermissionedFreeUsersForThisAccount = new List<string>();

                                    if (_exPNLSubscriptionLookup.ContainsKey(userID) && _exPNLSubscriptionLookup[userID].Contains(temp.Key))
                                        PermissionedFreeUsersForThisAccount.Add(userID);
                                    SendOrdersToClient(ref listOfOrdersToSendToClient, PermissionedFreeUsersForThisAccount, true, PranaMessageConstants.MSG_EPNlItemList, null);
                                    listOfOrdersToSendToClient.Clear();
                                }
                                temp.Value.IsUpdated = false;
                            }
                        }
                        SendFilterDataToUsers(subMsgs, userID);
                        SendControlMessageToClient(PranaMessageConstants.MSG_ExpPNLEndOfMessage, users);

                    }
                    else
                    {
                        string logMessage = "Either compression component or it's data is null.";
                        LogAndDisplayOnInformationReporter.GetInstance.Write(logMessage, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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
        }

        internal void SendFilterDataToUsers(List<string> subMsgs, string userID)
        {
            try
            {
                if (_dictUserWisePermissions.ContainsKey(userID) && _dictUserWisePermissions[userID].IsMarketDataPermissionEnabled && subMsgs.Count > 0)
                {
                    string subMSG = subMsgs[0];
                    string[] subMessage = subMSG.Split(Seperators.SEPERATOR_5);
                    string tabKey = subMessage[0];
                    string[] allAccountList = subMessage[1].Split(Seperators.SEPERATOR_6);
                    List<int> accountList = new List<int>();
                    if (!string.IsNullOrEmpty(allAccountList[0]))
                    {
                        accountList = allAccountList[0].Split(Seperators.SEPERATOR_14).Select(Int32.Parse).ToList();
                    }
                    Dictionary<int, DistinctAccountSetWiseSummaryCollection> accountSetWiseSummary = _compressionComponent[0].GetCalculatedSummaries().OutputAccountSetWiseConsolidatedSummary;
                    int key = SessionManager.DistinctAccountPermissionSets.FirstOrDefault(
                        x => IsContentEqualInLists(x.Value, accountList)).Key;
                    if (accountSetWiseSummary.ContainsKey(key))
                    {
                        accountSetWiseSummary[key].TabKey = tabKey;
                        SendAccountSummaryChunkToClient(accountSetWiseSummary[key], new List<string> { userID });
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

        internal static bool IsContentEqualInLists(List<int> list1, List<int> list2)
        {
            return list1.Count == list2.Count // assumes unique values in each list
                && new HashSet<int>(list1).SetEquals(list2);
        }
    }
}