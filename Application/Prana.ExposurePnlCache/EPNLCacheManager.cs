using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.SocketCommunication;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

namespace Prana.ExposurePnlCache
{
    public class ExposurePnlCacheManager : IDisposable
    {
        public static string view = null;
        private List<PropertyInfo> _dynamicColumnPropertyList;
        private List<string> _lsDynamicColumn;
        private System.Timers.Timer _clientStatusResponseTimer = null;
        private Prana.Utilities.MiscUtilities.PranaBinaryFormatter _binaryFormatter;
        private int bunchCount = 0;
        private bool _isUpdating = false;
        private bool isGroupingColumnValueChanged = false;
        private ExposurePnlCacheItemList _listofTaxLots = null;
        private string _compressedRowId = string.Empty;
        private ExposurePnlCacheItem _latestOrderToProcess = null;
        private bool _isColumnListChangedInUpdations = false;
        private byte[] byteArrayDisposable = new byte[1];
        private List<ExposurePnlCacheItem> _latestBunch = null;
        private List<string> _accountList;
        private bool _datareceivedForFirstTime = false;
        private Dictionary<string, DistinctAccountSetWiseSummaryCollection> _distinctPermissionSetWiseSummary = new Dictionary<string, DistinctAccountSetWiseSummaryCollection>();
        private bool _enablePMLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnablePMLogging"));

        public string SelectedTabKey = String.Empty;
        public List<string> PMCurrentViewGroupedColumns = new List<string>();

        private DataTable _consolidationViewSummary = null;
        public DataTable ConsolidationViewSummary
        {
            get { return _consolidationViewSummary; }
        }

        Dictionary<string, DataTable> _accountWiseSummary = null;
        public Dictionary<string, DataTable> AccountWiseSummary
        {
            get { return _accountWiseSummary; }
        }

        private ExposurePnlCacheBindableDictionary _pmAccountView = new ExposurePnlCacheBindableDictionary();
        public ExposurePnlCacheBindableDictionary PMAccountView
        {
            get { return _pmAccountView; }
        }

        static private CompanyUser _loginUser;
        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set { _loginUser = value; }
        }

        private bool _isInitialised = false;
        public bool IsInitialised
        {
            get { return _isInitialised; }
            set { _isInitialised = value; }
        }

        private static ICommunicationManager _exposurePnlCommunicationManager;
        public ICommunicationManager ExposureAndPnlCommunicationManagerInstance
        {
            set { _exposurePnlCommunicationManager = value; }
        }

        StringDictionary _currentOrderIDs;
        StatusMonitor _currentStatus;
        public class StatusMonitor
        {
            public List<string> latestUpdatedIDs = new List<string>();
            public List<string> lsItemIDsToRequestCompleteData = new List<string>();
        }

        public event EventHandler<EventArgs<bool>> ExposurePnlCacheSummaryChanged;
        public event EventHandler PMDataBinded;
        public event EventHandler<EventArgs<ExposurePnlCacheItemList, string, string>> TaxlotsReceived;
        public event EventHandler<EventArgs<List<string>>> SetPMViewPreferencesList;
        public event EventHandler<EventArgs<DataTable, List<int>>> UpdateOrderSummaryTable;
        public static event EventHandler<EventArgs> UpdateSymbolPositonAndExpose;

        private static ExposurePnlCacheManager _exposurePnlCacheManager = null;
        private ExposurePnlCacheManager()
        {
            _binaryFormatter = new Prana.Utilities.MiscUtilities.PranaBinaryFormatter();
            _accountWiseSummary = new Dictionary<string, DataTable>();
            _dynamicColumnPropertyList = new List<PropertyInfo>();
            _consolidationViewSummary = CommonHelper.OrderSummaryTable;
            _currentOrderIDs = new StringDictionary();
            _latestBunch = new List<ExposurePnlCacheItem>();
            double dd = Convert.ToDouble(ConfigurationManager.AppSettings["ClientBusyStatusInterval"]);
            _clientStatusResponseTimer = new System.Timers.Timer(dd);
            _clientStatusResponseTimer.Elapsed += new System.Timers.ElapsedEventHandler(_clientStatusResponseTimer_Elapsed);
            _clientStatusResponseTimer.Interval = dd;
            _clientStatusResponseTimer.Enabled = true;
            _currentStatus = new StatusMonitor();
        }

        void _clientStatusResponseTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_currentStatus != null)
                SendClientStatus(_currentStatus.lsItemIDsToRequestCompleteData);
        }

        public static ExposurePnlCacheManager GetInstance()
        {
            try
            {
                if (_exposurePnlCacheManager == null)
                {
                    _exposurePnlCacheManager = new ExposurePnlCacheManager();
                    _exposurePnlCacheManager._isInitialised = false;
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
            return _exposurePnlCacheManager;
        }

        public void PauseUpdates()
        {
            if (_exposurePnlCommunicationManager != null)
            {
                _exposurePnlCommunicationManager.MessageReceived -= new MessageReceivedDelegate(ProcessIncomingOrders);
            }
        }

        public void ResumeUpdates()
        {
            if (_exposurePnlCommunicationManager != null)
            {
                _exposurePnlCommunicationManager.MessageReceived += new MessageReceivedDelegate(ProcessIncomingOrders);
                SendClientStatus(_currentStatus.lsItemIDsToRequestCompleteData);
            }
        }

        public void SendPMPreferences(ExPNLPreferenceMsgType prefreneceType, string subMessage)
        {
            try
            {
                if (_exposurePnlCommunicationManager != null && _exposurePnlCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    _exposurePnlCommunicationManager.SendMessage(PranaMessageFormatter.CreatePMPreferenceMessage(prefreneceType, _loginUser.CompanyUserID.ToString(), subMessage));
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

        private void SendClientStatus(List<string> lsItemIDsToRequestCompleteData)
        {
            try
            {
                if (_exposurePnlCommunicationManager != null && _exposurePnlCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    _exposurePnlCommunicationManager.SendMessage(PranaMessageFormatter.CreateClientStatusMessage(_loginUser.CompanyUserID.ToString(), false, lsItemIDsToRequestCompleteData));
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

        public void SendDataRefreshMsg(string inputmessage, int userID)
        {
            try
            {
                string message = string.Empty;
                if (_exposurePnlCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    message = PranaMessageFormatter.CreateExPnlRefreshDataMsg(inputmessage, userID);

                    _exposurePnlCommunicationManager.SendMessage(message);
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

        public void SendPreferencesUpdateMsg(bool useClosingMark, double xpercent)
        {
            string message = string.Empty;
            if (_exposurePnlCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                message = PranaMessageFormatter.CreatePrefUpdateMsg(useClosingMark, xpercent);
                _exposurePnlCommunicationManager.SendMessage(message);
            }
        }

        private void OnExposurePnlCacheSummaryChanged()
        {
            try
            {
                if (ExposurePnlCacheSummaryChanged != null)
                {
                    ExposurePnlCacheSummaryChanged(this, new EventArgs<bool>(_datareceivedForFirstTime));
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

        private void OnTaxLotsReceived(ExposurePnlCacheItemList taxlotlist, string taxlotReqCallerGridName, string compressedRowID)
        {
            try
            {
                if (TaxlotsReceived != null)
                {
                    TaxlotsReceived(this, new EventArgs<ExposurePnlCacheItemList, string, string>(taxlotlist, taxlotReqCallerGridName, compressedRowID));
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

        public void Initialise(CompanyUser loginUser, ICommunicationManager exposurePnlCommunicationManager)
        {
            try
            {
                if (!_isInitialised)
                {
                    _loginUser = loginUser;
                    _exposurePnlCommunicationManager = exposurePnlCommunicationManager;
                    if (_exposurePnlCommunicationManager != null)
                    {
                        _exposurePnlCommunicationManager.Connected += new EventHandler(_exposurePnlCommunicationManager_Connected);
                        _exposurePnlCommunicationManager.Disconnected += new EventHandler(_exposurePnlCommunicationManager_Disconnected);
                        _exposurePnlCommunicationManager.MessageReceived += new MessageReceivedDelegate(ProcessIncomingOrders);
                    }

                    _pmAccountView = new ExposurePnlCacheBindableDictionary();
                    _isInitialised = true;

                    _accountList = WindsorContainerManager.GetAccountsForTheUser(_loginUser.CompanyUserID);
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

        public void DoWork(Object state)
        {
            try
            {
                object[] array = state as object[];
                ExPNLPreferenceMsgType prefMSGType = (ExPNLPreferenceMsgType)array[0];
                string uiChangeInfo = (string)array[1];

                //Duplicate column addition through groupby/columnAdd is already taking care by the grid
                //Grid Take cares that event don't get fire in case of duplicate column
                switch (prefMSGType)
                {
                    case ExPNLPreferenceMsgType.GroupByColumnAdded:
                    case ExPNLPreferenceMsgType.SelectedColumnAdded:
                        //Checking if newly selected column is dynamic, As we are not sending the static column request                        
                        if (_lsDynamicColumn != null && _lsDynamicColumn.Contains(uiChangeInfo))
                            SendPMPreferences(prefMSGType, uiChangeInfo);
                        break;
                    default:
                        SendPMPreferences(prefMSGType, uiChangeInfo);
                        break;
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

        public void PMPrefUpdated(ExPNLPreferenceMsgType prefMSGType, string info)
        {
            DoWork(new object[] { prefMSGType, info });
        }

        private void ProcessIncomingOrders(object sender, EventArgs<QueueMessage> ea)
        {
            try
            {
                QueueMessage qMsg = ea.Value;
                _clientStatusResponseTimer.Stop();
                _clientStatusResponseTimer.Start();
                _clientStatusResponseTimer.Enabled = true;

                if (!_isUpdating)
                {
                    switch (qMsg.MsgType)
                    {
                        #region Message Header Received
                        case PranaMessageConstants.MSG_Header:
                            string unZippedHeaderMsg = qMsg.Message.ToString();
                            qMsg = null;
                            string[] columnNames = unZippedHeaderMsg.Split(',');
                            unZippedHeaderMsg = null;
                            ExposurePnlCacheItem e = new ExposurePnlCacheItem();
                            lock (_dynamicColumnPropertyList)
                            {
                                _dynamicColumnPropertyList.Clear();
                                foreach (string columnName in columnNames)
                                {
                                    if (!string.IsNullOrEmpty(columnName))
                                    {
                                        PropertyInfo existingProp = e.GetType().GetProperty(columnName);
                                        _dynamicColumnPropertyList.Add(existingProp);
                                    }
                                }
                                if (_pmAccountView != null)
                                    _pmAccountView.DynamicColumnPropertyList = DeepCopyHelper.Clone<List<PropertyInfo>>(_dynamicColumnPropertyList);
                            }
                            columnNames = null;
                            e = null;
                            break;
                        #endregion

                        #region Message Start Received
                        case PranaMessageConstants.MSG_ExpPNLStartOfMessage:
                            lock (_dynamicColumnPropertyList)
                            {
                                _isColumnListChangedInUpdations = false;
                            }
                            bunchCount = 0;
                            if (_enablePMLogging)
                                Logger.LoggerWrite("Start of message : Receiving ExposurePnlCacheItem as queue message. Count : " + bunchCount, LoggingConstants.CATEGORY_GENERAL);
                            break;
                        #endregion

                        #region Grid Data Received
                        case PranaMessageConstants.MSG_EPNlItemList:
                            ++bunchCount;
                            if (_enablePMLogging)
                            {
                                Logger.LoggerWrite("Received Bunch Number : " + bunchCount, LoggingConstants.CATEGORY_GENERAL);
                            }
                            string unZippedMsg = CompressionHelper.UnZip(qMsg.Message.ToString(), byteArrayDisposable);
                            qMsg = null;
                            string[] arr = unZippedMsg.Split(Seperators.SEPERATOR_4);

                            unZippedMsg = null;

                            for (int i = 0; i < arr.Length - 1; i++)
                            {
                                int status = int.Parse(arr[i].Substring(0, 1));
                                string[] str = arr[i].Split(Seperators.SEPERATOR_2);
                                string id;
                                if (status == 1)
                                {
                                    id = str[1];
                                    _latestOrderToProcess = new ExposurePnlCacheItem();
                                    lock (_dynamicColumnPropertyList)
                                    {
                                        _latestOrderToProcess.UpdateDynamicData(ref _dynamicColumnPropertyList, arr[i], ref _isColumnListChangedInUpdations);
                                        _latestOrderToProcess.PricingSource = EnumHelper.GetDescription((PricingSource)Enum.Parse(typeof(PricingSource), _latestOrderToProcess.PricingSource));
                                        _latestOrderToProcess.PricingStatus = EnumHelper.GetDescription((PricingStatus)Enum.Parse(typeof(PricingStatus), _latestOrderToProcess.PricingStatus));
                                    }
                                }
                                else
                                {
                                    id = str[1];
                                    _latestOrderToProcess = new ExposurePnlCacheItem(arr[i]);

                                    if (0 != _latestOrderToProcess.VsCurrencyID && _latestOrderToProcess.VsCurrencyID != -1)
                                    {
                                        _latestOrderToProcess.VsCurrencySymbol = CachedDataManager.GetInstance.GetCurrencyText(_latestOrderToProcess.VsCurrencyID);
                                    }
                                    else
                                    {
                                        _latestOrderToProcess.VsCurrencySymbol = string.Empty;
                                    }

                                    if (0 != _latestOrderToProcess.CounterCurrencyID && _latestOrderToProcess.CounterCurrencyID != -1)
                                    {
                                        _latestOrderToProcess.CounterCurrencySymbol = CachedDataManager.GetInstance.GetCurrencyText(_latestOrderToProcess.CounterCurrencyID);
                                    }
                                    else
                                    {
                                        _latestOrderToProcess.CounterCurrencySymbol = string.Empty;
                                    }

                                    if (0 != _latestOrderToProcess.LeadCurrencyID && _latestOrderToProcess.LeadCurrencyID != -1)
                                    {
                                        _latestOrderToProcess.LeadCurrencySymbol = CachedDataManager.GetInstance.GetCurrencyText(_latestOrderToProcess.LeadCurrencyID);
                                    }
                                    else
                                    {
                                        _latestOrderToProcess.LeadCurrencySymbol = string.Empty;
                                    }
                                    if (_latestOrderToProcess.UnderlyingID != -1)
                                    {
                                        _latestOrderToProcess.Underlying = CachedDataManager.GetInstance.GetUnderLyingText(_latestOrderToProcess.UnderlyingID);
                                    }
                                    if (_latestOrderToProcess.ExchangeID != -1)
                                    {
                                        _latestOrderToProcess.Exchange = CachedDataManager.GetInstance.GetExchangeText(_latestOrderToProcess.ExchangeID);
                                    }
                                    if (_latestOrderToProcess.CurrencyID != -1)
                                    {
                                        _latestOrderToProcess.CurrencySymbol = CachedDataManager.GetInstance.GetCurrencyText(_latestOrderToProcess.CurrencyID);
                                    }
                                    if (_latestOrderToProcess.MasterFundID != -1)
                                    {
                                        _latestOrderToProcess.MasterFund = _latestOrderToProcess.MasterFundID == int.MinValue ? ApplicationConstants.C_Multiple : CachedDataManager.GetInstance.GetMasterFund(_latestOrderToProcess.MasterFundID);
                                    }
                                    else if (_latestOrderToProcess.MasterFundID == -1)
                                    {
                                        _latestOrderToProcess.MasterFund = CachedDataManager.GetInstance.IsShowMasterFundonTT() && !string.IsNullOrEmpty(_latestOrderToProcess.TradeAttribute6) ? _latestOrderToProcess.TradeAttribute6 : string.Empty;
                                    }
                                    if (_latestOrderToProcess.MasterStrategyID != -1)
                                    {
                                        _latestOrderToProcess.MasterStrategy = CachedDataManager.GetInstance.GetMasterStrategy(_latestOrderToProcess.MasterStrategyID);
                                    }
                                    if (_latestOrderToProcess.Level2ID != -1)
                                    {
                                        _latestOrderToProcess.Level2Name = _latestOrderToProcess.Level2ID == int.MinValue ? ApplicationConstants.C_Multiple : CachedDataManager.GetInstance.GetStrategyText(_latestOrderToProcess.Level2ID);
                                    }
                                    if (_latestOrderToProcess.Level1ID != -1)
                                    {
                                        if (_latestOrderToProcess.Level1ID == int.MinValue)
                                        {
                                            _latestOrderToProcess.Level1Name = ApplicationConstants.C_Multiple;
                                        }
                                        else
                                        {
                                            _latestOrderToProcess.Level1Name = CachedDataManager.GetInstance.GetAccountText(_latestOrderToProcess.Level1ID);
                                            if (CachedDataManager.HasDataSource(_latestOrderToProcess.Level1ID))
                                            {
                                                _latestOrderToProcess.DataSourceNameIDValue = CachedDataManager.GetDatasource(_latestOrderToProcess.Level1ID).ShortName;
                                            }
                                        }
                                    }

                                    if (_latestOrderToProcess.OrderSideTagValue != ApplicationConstants.C_Multiple)
                                    {
                                        _latestOrderToProcess.SideName = TagDatabaseManager.GetInstance.GetOrderSideText(_latestOrderToProcess.OrderSideTagValue);
                                    }
                                    else
                                    {
                                        _latestOrderToProcess.SideName = ApplicationConstants.C_Multiple;
                                    }

                                    if (_latestOrderToProcess.Symbol == ApplicationConstants.C_Multiple)
                                    {
                                        _latestOrderToProcess.UDAAsset = ApplicationConstants.C_Multiple;
                                        _latestOrderToProcess.UDACountry = ApplicationConstants.C_Multiple;
                                        _latestOrderToProcess.UDASector = ApplicationConstants.C_Multiple;
                                        _latestOrderToProcess.UDASecurityType = ApplicationConstants.C_Multiple;
                                        _latestOrderToProcess.UDASubSector = ApplicationConstants.C_Multiple;

                                    }

                                    if (_latestOrderToProcess.Asset.Equals("Equity") && _latestOrderToProcess.IsSwap)
                                    {
                                        _latestOrderToProcess.Asset = "EquitySwap";
                                    }
                                    if (_latestOrderToProcess.TransactionType != ApplicationConstants.C_Multiple)
                                        _latestOrderToProcess.TransactionType = CachedDataManager.GetInstance.GetTransactionTypeNameByAcronym(_latestOrderToProcess.TransactionType);

                                    _latestOrderToProcess.PricingSource = EnumHelper.GetDescription((PricingSource)Enum.Parse(typeof(PricingSource), _latestOrderToProcess.PricingSource));
                                    _latestOrderToProcess.PricingStatus = EnumHelper.GetDescription((PricingStatus)Enum.Parse(typeof(PricingStatus), _latestOrderToProcess.PricingStatus));
                                }
                                //If PM is closed in between of this loop, then based on following check we are stopping processing
                                if (_currentOrderIDs == null)
                                    return;
                                if (!_currentOrderIDs.ContainsKey(id))
                                {
                                    _currentOrderIDs.Add(id, null);
                                }
                                _latestBunch.Add(_latestOrderToProcess);
                                str = null;
                            }
                            arr = null;
                            UpdateCurrentdataFromIncomingOrders(_latestBunch, ref _currentStatus, PMCurrentViewGroupedColumns, ref isGroupingColumnValueChanged);
                            _latestBunch.Clear();
                            break;
                        #endregion

                        #region Summary Received
                        case PranaMessageConstants.MSG_ExpPNLCalcSummary:
                            DistinctAccountSetWiseSummaryCollection summary = (DistinctAccountSetWiseSummaryCollection)_binaryFormatter.DeSerialize(qMsg.Message.ToString());

                            if (!String.IsNullOrEmpty(summary.TabKey))
                            {
                                if (!_accountWiseSummary.ContainsKey(summary.TabKey))//Add this account to lookup
                                {
                                    _accountWiseSummary.Add(summary.TabKey, CommonHelper.GetOrderSummaryTableFromObject(summary.ConsolidationDashBoardSummary));
                                    _distinctPermissionSetWiseSummary.Add(summary.TabKey, summary);
                                }
                                else
                                {
                                    _accountWiseSummary[summary.TabKey] = CommonHelper.GetOrderSummaryTableFromObject(summary.ConsolidationDashBoardSummary);
                                    _distinctPermissionSetWiseSummary[summary.TabKey] = summary;
                                }
                            }

                            break;
                        #endregion

                        #region Message End Received
                        case PranaMessageConstants.MSG_ExpPNLEndOfMessage:
                            _isUpdating = false;
                            OnExposurePnlCacheSummaryChanged();
                            UpdateUIFromLatestOrderIDCollection();
                            if (PMDataBinded != null && isGroupingColumnValueChanged)
                            {
                                isGroupingColumnValueChanged = false;
                                PMDataBinded(null, null);
                            }
                            if (_currentStatus != null)
                            {
                                SendClientStatus(_currentStatus.lsItemIDsToRequestCompleteData);
                            }
                            if (UpdateSymbolPositonAndExpose != null)
                            {
                                UpdateSymbolPositonAndExpose(this, new EventArgs());
                            }
                            _datareceivedForFirstTime = false;
                            if (_latestBunch != null)
                            {
                                _latestBunch.Clear();
                            }
                            if (_currentOrderIDs != null)
                            {
                                _currentOrderIDs.Clear();
                            }
                            if (_currentStatus != null)
                            {
                                _currentStatus.latestUpdatedIDs.Clear();
                                _currentStatus.lsItemIDsToRequestCompleteData.Clear();
                            }
                            if (_enablePMLogging)
                                Logger.LoggerWrite("End of Message : Received ExposurePnlCacheItem as queue message. Count : " + bunchCount, LoggingConstants.CATEGORY_GENERAL);
                            break;
                        #endregion

                        #region Dynamic Column List, received on Refresh, Only these values are sent repeatedly
                        case PranaMessageConstants.MSG_ExpPNLDynamicColumnList:
                            string[] dynamicColList = qMsg.Message.ToString().Split(Seperators.SEPERATOR_2);
                            _lsDynamicColumn = new List<string>(dynamicColList);
                            if (SetPMViewPreferencesList != null)
                                SetPMViewPreferencesList(this, new EventArgs<List<string>>(_lsDynamicColumn));
                            // Commented it because we are updating it on Header Received.
                            //UpdateDynamicColPropList();

                            //if (_lsPMPreference != null)
                            //    SendPMPreferences(ExPNLPreferenceMsgType.NewPreferences,_lsPMPreference.ToString());

                            if (_enablePMLogging)
                                Logger.LoggerWrite("Dynamic Column list received : Cleaning current dictionary and binding list", LoggingConstants.CATEGORY_GENERAL);

                            //Inform the form about the updated structure of the ordersummarytable
                            if (UpdateOrderSummaryTable != null)
                            {
                                UpdateOrderSummaryTable(this, new EventArgs<DataTable, List<int>>(CommonHelper.OrderSummaryAccountTable, null));
                            }

                            break;
                        #endregion

                        #region Indices
                        case PranaMessageConstants.MSG_IndicesReturnSummary:
                            DataTable dtIndicesReturn = (DataTable)_binaryFormatter.DeSerialize(qMsg.Message.ToString());
                            if (dtIndicesReturn != null)
                            {
                                UpdateIndicesReturnIntoSummary(dtIndicesReturn);
                            }
                            break;

                        case PranaMessageConstants.MSG_ExpPNLIndexColumns:

                            DataTable dtIndex = (DataTable)_binaryFormatter.DeSerialize(qMsg.Message.ToString());
                            CommonHelper.AddIndicesColumnsToSummary(dtIndex);
                            UpdateBindedOrderSummaryStructure();

                            //Inform the form about the updated structure of the ordersummarytable
                            if (UpdateOrderSummaryTable != null)
                            {
                                UpdateOrderSummaryTable(this, new EventArgs<DataTable, List<int>>(CommonHelper.OrderSummaryAccountTable, null));
                            }

                            break;
                        #endregion

                        #region Filter Change Sending to Expnl
                        case PranaMessageConstants.MSG_FilterDetails:
                            PMPrefUpdated(ExPNLPreferenceMsgType.FilterValueChanged, SelectedTabKey);
                            break;
                        #endregion

                        #region Taxlot Form - Start
                        case PranaMessageConstants.MSG_EPNlTaxLotListStart:
                            _listofTaxLots = new ExposurePnlCacheItemList();
                            _compressedRowId = qMsg.Message.ToString();
                            break;
                        #endregion

                        #region Taxlot Form - Taxlot List
                        case PranaMessageConstants.MSG_EPNlTaxLotList:
                            byte[] byteArray = new byte[1];
                            string unTaxLotsZippedMsg = CompressionHelper.UnZip(qMsg.Message.ToString(), byteArray);
                            byteArray = null;
                            string[] arrTaxLots = unTaxLotsZippedMsg.Split(Seperators.SEPERATOR_4);
                            //TODO: fill collection from incoming taxlots and open a UI
                            ExposurePnlCacheItem taxLot;
                            for (int i = 0; i < arrTaxLots.Length - 1; i++)
                            {
                                taxLot = new ExposurePnlCacheItem(arrTaxLots[i]);
                                if (0 != taxLot.VsCurrencyID)
                                {
                                    taxLot.VsCurrencySymbol = CachedDataManager.GetInstance.GetCurrencyText(taxLot.VsCurrencyID);
                                }
                                else
                                {
                                    taxLot.VsCurrencySymbol = string.Empty;
                                }

                                if (0 != taxLot.CounterCurrencyID)
                                {
                                    taxLot.CounterCurrencySymbol = CachedDataManager.GetInstance.GetCurrencyText(taxLot.CounterCurrencyID);
                                }
                                else
                                {
                                    taxLot.CounterCurrencySymbol = string.Empty;
                                }

                                if (0 != taxLot.LeadCurrencyID && taxLot.LeadCurrencyID != -1)
                                {
                                    taxLot.LeadCurrencySymbol = CachedDataManager.GetInstance.GetCurrencyText(taxLot.LeadCurrencyID);
                                }
                                else
                                {
                                    taxLot.VsCurrencySymbol = string.Empty;
                                }
                                taxLot.Underlying = CachedDataManager.GetInstance.GetUnderLyingText(taxLot.UnderlyingID);
                                taxLot.Exchange = CachedDataManager.GetInstance.GetExchangeText(taxLot.ExchangeID);
                                taxLot.CurrencySymbol = CachedDataManager.GetInstance.GetCurrencyText(taxLot.CurrencyID);
                                taxLot.MasterFund = CachedDataManager.GetInstance.GetMasterFund(taxLot.MasterFundID);
                                taxLot.MasterStrategy = CachedDataManager.GetInstance.GetMasterStrategy(taxLot.MasterStrategyID);
                                taxLot.Level2Name = CachedDataManager.GetInstance.GetStrategyText(taxLot.Level2ID);
                                taxLot.Level1Name = CachedDataManager.GetInstance.GetAccountText(taxLot.Level1ID);
                                if (CachedDataManager.HasDataSource(taxLot.Level1ID))
                                {
                                    taxLot.DataSourceNameIDValue = CachedDataManager.GetDatasource(taxLot.Level1ID).ShortName;
                                }
                                taxLot.SideName = TagDatabaseManager.GetInstance.GetOrderSideText(taxLot.OrderSideTagValue);
                                if (taxLot.Asset.Equals("Equity") && taxLot.IsSwap)
                                {
                                    taxLot.Asset = "EquitySwap";
                                }
                                _listofTaxLots.Add(taxLot);
                            }
                            break;
                        #endregion

                        #region Taxlot Form - End
                        case PranaMessageConstants.MSG_EPNlTaxLotListEnd:
                            string taxlotReqCallerGridName = qMsg.Message.ToString();
                            OnTaxLotsReceived(_listofTaxLots, taxlotReqCallerGridName, _compressedRowId);
                            _listofTaxLots = null;
                            _compressedRowId = string.Empty;
                            break;
                        #endregion

                        #region Refresh Response
                        case PranaMessageConstants.MSG_EPNlRefreshResponse:
                            _datareceivedForFirstTime = true;
                            OnExposurePnlCacheSummaryChanged();
                            _datareceivedForFirstTime = false;
                            break;
                            #endregion
                    }
                }
                qMsg = null;
                if (_latestOrderToProcess != null)
                    _latestOrderToProcess = null;
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

        private void UpdateBindedOrderSummaryStructure()
        {
            try
            {
                _consolidationViewSummary = CommonHelper.OrderSummaryTable;
                if (_accountWiseSummary != null)
                {
                    List<string> accountKeys = new List<string>((IEnumerable<string>)_accountWiseSummary.Keys);
                    foreach (string key in accountKeys)
                    {
                        if (!string.IsNullOrEmpty(key))
                        {
                            _accountWiseSummary[key] = CommonHelper.OrderSummaryTable;
                        }
                        else
                        {
                            _accountWiseSummary[key] = CommonHelper.OrderSummaryAccountTable;
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

        private void UpdateIndicesReturnIntoSummary(DataTable indicesReturn)
        {
            try
            {
                if (indicesReturn != null)
                {
                    UpdateSummary(_consolidationViewSummary, indicesReturn);
                }
                foreach (KeyValuePair<string, DataTable> keyValue in _accountWiseSummary)
                {
                    UpdateSummary(keyValue.Value, indicesReturn);
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

        private void UpdateSummary(DataTable orderSummary, DataTable dtIndices)
        {
            try
            {
                foreach (DataColumn col in dtIndices.Columns)
                {
                    if (orderSummary != null)
                    {
                        if (!CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                        {
                            orderSummary.Rows[0][col.ColumnName] = 0;
                        }
                        else if (orderSummary.Columns.Contains(col.ColumnName))
                        {
                            orderSummary.Rows[0][col.ColumnName] = dtIndices.Rows[0][col];
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

        private void UpdateCurrentdataFromIncomingOrders(List<ExposurePnlCacheItem> newOrders, ref StatusMonitor currentStatus, List<string> pmCurrentViewGroupedColumns, ref bool isGroupingColumnValueChanged)
        {
            try
            {
                if (_pmAccountView != null)
                    _pmAccountView.UpdateCurrentdataFromIncomingOrders(newOrders, ref currentStatus, pmCurrentViewGroupedColumns, ref isGroupingColumnValueChanged);
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

        private void UpdateUIFromLatestOrderIDCollection()
        {
            try
            {
                if (_pmAccountView != null)
                    _pmAccountView.UpdateUIFromLatestOrderIDCollection(_currentOrderIDs, _currentStatus.latestUpdatedIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        void _exposurePnlCommunicationManager_Disconnected(object sender, EventArgs e)
        {
            _isUpdating = false;
            _isInitialised = false;
            if (_clientStatusResponseTimer != null && _clientStatusResponseTimer.Enabled)
                _clientStatusResponseTimer.Stop();
        }

        void _exposurePnlCommunicationManager_Connected(object sender, EventArgs e)
        {
            try
            {
                if (_clientStatusResponseTimer != null && !_clientStatusResponseTimer.Enabled)
                    _clientStatusResponseTimer.Start();
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

        public void ClearData()
        {
            try
            {
                PauseUpdates();
                _exposurePnlCommunicationManager.MessageReceived -= new MessageReceivedDelegate(ProcessIncomingOrders);
                _exposurePnlCommunicationManager.Connected -= new EventHandler(_exposurePnlCommunicationManager_Connected);
                _exposurePnlCommunicationManager.Disconnected -= new EventHandler(_exposurePnlCommunicationManager_Disconnected);
                _exposurePnlCommunicationManager = null;
                _loginUser = null;

                _clientStatusResponseTimer.Elapsed -= new System.Timers.ElapsedEventHandler(_clientStatusResponseTimer_Elapsed);
                _clientStatusResponseTimer = null;
                _binaryFormatter = null;
                view = null;
                bunchCount = 0;

                if (_dynamicColumnPropertyList != null)
                    _dynamicColumnPropertyList.Clear();
                _dynamicColumnPropertyList = null;

                if (_lsDynamicColumn != null)
                    _lsDynamicColumn.Clear();
                _lsDynamicColumn = null;

                if (_consolidationViewSummary != null)
                    _consolidationViewSummary.Clear();
                _consolidationViewSummary = null;

                if (_accountWiseSummary != null)
                    _accountWiseSummary.Clear();
                _accountWiseSummary = null;

                if (_pmAccountView != null)
                {
                    _pmAccountView.Clear();
                    _pmAccountView.Dispose();
                    _pmAccountView = null;
                }

                if (_currentOrderIDs != null)
                    _currentOrderIDs.Clear();
                _currentOrderIDs = null;
                _currentStatus = null;
                _exposurePnlCacheManager = null;

                if (_latestBunch != null)
                    _latestBunch.Clear();
                _latestBunch = null;

                if (_accountList != null)
                    _accountList.Clear();
                _accountList = null;

                if (_listofTaxLots != null)
                    _listofTaxLots.Clear();
                _listofTaxLots = null;
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

        public IList<ExposurePnlCacheItem> getPositionForAccountsbySymbol(string symbol)
        {
            return _pmAccountView.getPositionForAccountsbySymbol(symbol);
        }

        public IList<string> getSymbolsForAccounts()
        {
            return _pmAccountView.getSymbolsForAccounts();
        }

        public Dictionary<string, string> getBloombergSymbolToTickerForAccounts()
        {
            return _pmAccountView.getBloombergSymbolToTickerForAccounts();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (_pmAccountView != null)
                    {
                        _pmAccountView.Dispose();
                        _pmAccountView = null;
                    }
                    if (_clientStatusResponseTimer != null)
                    {
                        _clientStatusResponseTimer.Dispose();
                        _clientStatusResponseTimer = null;
                    }
                    if (_consolidationViewSummary != null)
                    {
                        _consolidationViewSummary.Dispose();
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

        public void OverrideGridColumns(string tabKey, List<int> fundIDs, bool isTabChangeFromUI = false)
        {
            try
            {
                if (_distinctPermissionSetWiseSummary.ContainsKey(tabKey))
                {
                    string lastUpdateKey = String.Empty;
                    if (!isTabChangeFromUI)
                        _pmAccountView.SuspendListChangeEvent();
                    Parallel.ForEach(_pmAccountView.ExposurePnlCacheItemDictionary.Values.Where(item => fundIDs.Contains(item.Level1ID)), exposurePnlCacheItem =>
                                            {
                                                DynamicSummaryCalculator.FillOrderWithSummaryValues(_distinctPermissionSetWiseSummary[tabKey], exposurePnlCacheItem, PMCurrentViewGroupedColumns, ref isGroupingColumnValueChanged);
                                                lastUpdateKey = _pmAccountView.ExposurePnlCacheItemDictionary.FirstOrDefault(kvp => kvp.Value == exposurePnlCacheItem).Key;
                                            });
                    if (!isTabChangeFromUI)
                        _pmAccountView.ResumeListChangeEvent();

                    List<string> updatedIDs = new List<string>();
                    if (!String.IsNullOrEmpty(lastUpdateKey))
                        _pmAccountView.AddOrUpdateList(lastUpdateKey, updatedIDs);
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

        public void SendTaxLotRequestMsg(string groupedRowID, string callerGridName, int accountID, List<int> filteredAccountList)
        {
            try
            {
                string message = string.Empty;
                if (_exposurePnlCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    message = PranaMessageFormatter.CreateTaxLotRequest(_loginUser.CompanyUserID.ToString(), groupedRowID, callerGridName, accountID, filteredAccountList);
                    _exposurePnlCommunicationManager.SendMessage(message);
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
    }
}