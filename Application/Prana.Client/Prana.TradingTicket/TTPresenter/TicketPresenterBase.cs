using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.LiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.ExposurePnlCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Rebalancer;
using Prana.ServiceConnector;
using Prana.TradeManager.Forms;
using Prana.TradingTicket.Forms;
using Prana.TradingTicket.OrderManager;
using Prana.TradingTicket.TTView;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.TradingTicket.TTPresenter
{
    internal delegate void ConnectionInvokeDelegate(object sender, EventArgs e);

    internal delegate void PublishInvokeDelegate(MessageData e, string topicName);

    public abstract class TicketPresenterBase : IPublishing, IDisposable
    {
        /// <summary>
        /// The symbol action
        /// </summary>
        internal SecMasterConstants.SecurityActions SymbolAction = SecMasterConstants.SecurityActions.SEARCH;
        /// <summary>
        /// The default symbology
        /// </summary>
        internal ApplicationConstants.SymbologyCodes DefaultSymbology = SymbologyHelper.DefaultSymbology;
        /// <summary>
        /// The _order request
        /// </summary>
        protected OrderSingle _orderRequest = new OrderSingle();

        protected bool _enableTradeFlowLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableTradeFlowLogging"));

        internal EventHandler PriceForComplianceNotAvailable;

        /// <summary>
        /// The _trade manager
        /// </summary>
        protected static TradeManager.TradeManager _tradeManager;
        /// <summary>
        /// subscription proxy
        /// </summary>
        DuplexProxyBase<ISubscription> _subscribeProxy;

        /// <summary>
        /// The _accountqty
        /// </summary>
        protected AccountQty _accountqty = null;
        /// <summary>
        /// The _allocation proxy
        /// </summary>
        protected ProxyBase<IAllocationManager> _allocationProxy;
        /// <summary>
        /// The _asset identifier
        /// </summary>
        protected int _assetID = int.MinValue;
        /// <summary>
        /// The _auec identifier
        /// </summary>
        protected int _auecID = int.MinValue;
        /// <summary>
        /// The _enabled l1
        /// </summary>
        protected bool _enabledL1 = true;
        /// <summary>
        /// The _is pricing available
        /// </summary>
        protected Boolean _isPricingAvailable;
        /// <summary>
        /// The _login user
        /// </summary>
        protected CompanyUser _loginUser;
        /// <summary>
        /// The _market data eanbled
        /// </summary>
        protected bool _marketDataEanbled;
        /// <summary>
        /// The _market price
        /// </summary>
        protected double _marketPrice;
        /// <summary>
        /// The _secmaster object
        /// </summary>
        protected SecMasterBaseObj _secmasterObj;
        /// <summary>
        /// The _security master
        /// </summary>
        protected ISecurityMasterServices _securityMaster;
        /// <summary>
        /// The _symbol
        /// </summary>
        protected string _symbol = string.Empty;
        /// <summary>
        /// The binary formatter
        /// </summary>
        protected PranaBinaryFormatter BinaryFormatter = new PranaBinaryFormatter();
        /// <summary>
        /// The i ticket view
        /// </summary>
        protected ITicketView iTicketView;
        /// <summary>
        /// The _user trading ticket UI prefs
        /// </summary>
        protected TradingTicketUIPrefs _userTradingTicketUiPrefs;
        /// <summary>
        /// The _company trading ticket UI prefs
        /// </summary>
        protected TradingTicketUIPrefs _companyTradingTicketUiPrefs;
        /// <summary>
        /// The _prev auec
        /// </summary>
        protected int _prevAUEC = int.MinValue;
        IOrderSingleProvider _orderSingleProvider;
        private int _counterPartyId = int.MinValue;

        internal bool UseQuantityFieldAsNotional { get; set; }

        /// <summary>
        /// Gets or sets the algo strategy identifier.
        /// </summary>
        /// <value>
        /// The algo strategy identifier.
        /// </value>
        internal string AlgoStrategyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the algo strategy.
        /// </summary>
        /// <value>
        /// The name of the algo strategy.
        /// </value>
        internal string AlgoStrategyName { get; set; }

        /// <summary>
        /// Gets or sets the asset identifier.
        /// </summary>
        /// <value>
        /// The asset identifier.
        /// </value>
        internal int AssetID
        {
            get { return _assetID; }
            set
            {
                _assetID = value;
                BindOrderSide();
            }
        }

        /// <summary>
        /// Gets or sets the trading ticket UI prefs.
        /// </summary>
        /// <value>
        /// The trading ticket UI prefs.
        /// </value>
        internal TradingTicketUIPrefs TradingTicketUiPrefs
        {
            get { return _userTradingTicketUiPrefs; }
        }

        /// <summary>
        /// Gets or sets the company trading ticket UI prefs.
        /// </summary>
        /// <value>
        /// The company trading ticket UI prefs.
        /// </value>
        internal TradingTicketUIPrefs CompanyTradingTicketUiPrefs
        {
            get { return _companyTradingTicketUiPrefs; }
        }

        /// <summary>
        /// Gets or sets the auec identifier.
        /// </summary>
        /// <value>
        /// The auec identifier.
        /// </value>
        internal int AuecID
        {
            set
            {
                if (_prevAUEC != value)
                {
                    _auecID = value;
                    if (_auecID != int.MinValue)
                    {
                        iTicketView.SetAlgoDetails(_auecID);
                    }
                }
                else
                {
                    _auecID = value;
                }
                _prevAUEC = value;
            }
            get { return _auecID; }
        }

        /// <summary>
        /// Gets or sets the counter party identifier.
        /// </summary>
        /// <value>
        /// The counter party identifier.
        /// </value>
        internal int CounterPartyId
        {
            get { return _counterPartyId; }
            set { _counterPartyId = value; }
        }
        /// <summary>
        /// Gets or sets the currency identifier.
        /// </summary>
        /// <value>
        /// The currency identifier.
        /// </value>
        internal int CurrencyId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pricing available.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pricing available; otherwise, <c>false</c>.
        /// </value>
        internal bool IsPricingAvailable
        {
            set { _isPricingAvailable = value; }
        }

        /// <summary>
        /// Gets or sets the login user.
        /// </summary>
        /// <value>
        /// The login user.
        /// </value>
        internal CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
                if (_loginUser != null && !_loginUser.MarketDataTypes.Contains(LiveFeedConstants.LevelOne) && _enabledL1)
                {
                    if (iTicketView != null)
                        iTicketView.RefreshControl(true);
                    _enabledL1 = false;
                }
                else
                {
                    _marketDataEanbled = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [market data eanbled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [market data eanbled]; otherwise, <c>false</c>.
        /// </value>
        internal bool MarketDataEanbled
        {
            get { return _marketDataEanbled; }
        }

        /// <summary>
        /// Gets or sets the market price.
        /// </summary>
        /// <value>
        /// The market price.
        /// </value>
        internal double MarketPrice
        {
            get { return _marketPrice; }
            set { _marketPrice = value; }
        }

        /// <summary>
        /// Sets the security master.
        /// </summary>
        /// <value>
        /// The security master.
        /// </value>
        internal ISecurityMasterServices SecurityMaster
        {
            set
            {
                if (_securityMaster == null)
                {
                    _securityMaster = value;
                    _securityMaster.SecMstrDataResponse += _securityMaster_SecMstrDataResponse;
                    _securityMaster.Disconnected += _securityMaster_Disconnected;
                    _securityMaster.Connected += _securityMaster_Connected;
                    _securityMaster.SecMstrDataSymbolSearcResponse += _securityMaster_SecMstrDataSymbolSearcResponse;
                }
            }
        }

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        internal string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
            }
        }

        /// <summary>
        /// The _secmaster object
        /// </summary>
        internal SecMasterBaseObj SecmasterObj
        {
            get { return _secmasterObj; }
        }

        internal int UnderlyingID
        {
            get { return _secmasterObj != null ? _secmasterObj.UnderLyingID : int.MinValue; }
        }
        /// <summary>
        /// The _underlying symbol
        /// </summary>
        internal string UnderlyingSymbol { get; set; }

        /// <summary>
        /// The _multiplier
        /// </summary>
        internal double Multiplier { get; set; }

        internal int FundId { get; set; }

        /// <summary>
        /// Adds the specified object i trading ticket view.
        /// </summary>
        /// <param name="objITradingTicketView">The object i trading ticket view.</param>
        internal virtual void Add(ITicketView objITicketView)
        {
            iTicketView = objITicketView;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Binds the commission basis combo.
        /// </summary>
        protected void BindCommissionBasisCombo()
        {
            try
            {
                IList list = EnumHelper.ToList(typeof(CalculationBasis));
                for (int i = 0; i < 4; i++)
                {
                    list.RemoveAt(2);
                }
                iTicketView.FillCommissionBasis(list);
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
        /// Fills the combo boxes.
        /// </summary>
        /// <param name="cpList">The cp list.</param>
        internal virtual void FillComboBoxes()
        {
            try
            {
                ValueList cpList = new ValueList();
                //Getting funds for with allocation schemes
                DataTable accountsData = FillAccountCombo();

                //If IsShowMasterFundonTT true then filter the broker list based on Accounts and AUEC
                if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    cpList = GetDataForAllocationAndBrokerCombo(FundId, cpList, accountsData);
                }
                else
                {
                    //brokers based on asset and underlying (OLD workflow)
                    cpList = TTHelperManager.GetInstance().GetCounterparties(_assetID, UnderlyingID);
                }

                if (cpList == null)
                {
                    iTicketView.SetLabelMessage(TradingTicketConstants.MSG_PLEASE_CHECK_IF_YOU_HAVE_PERMISSION_TO_TRADE);
                    return;
                }
                if (accountsData != null)
                    iTicketView.FillAccountCombo(accountsData);

                iTicketView.SetLabelMessage(string.Empty);
                BindCommissionBasisCombo();
                FillBrokeCombo(cpList);
                FillVenueCombo();
                BindOrderType();
                BindTIF();
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
        /// Fills the venue combo.
        /// </summary>
        internal void FillVenueCombo()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(TradingTicketConstants.LIT_VALUE);
                dt.Columns.Add(TradingTicketConstants.LIT_DISPLAY);

                //Getting Venues By CounterParty ID
                ValueList venueList = TTHelperManager.GetInstance().GetVenuesByCounterPartyID(CounterPartyId, _assetID, UnderlyingID, _auecID);

                foreach (ValueListItem t in venueList.ValueListItems)
                {
                    dt.Rows.Add(t.DataValue, t.DisplayText);
                }

                iTicketView.FillVenueCombo(dt);
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
        /// Gets the and update account wise quantity.
        /// </summary>
        internal void GetAndUpdateAccountWiseQuantity()
        {
            try
            {
                if (_accountqty == null || _accountqty.IsDisposed)
                {
                    _accountqty = new AccountQty(_symbol, FundId);
                    _accountqty.AllocationManager = _allocationProxy.InnerChannel;
                }
                UpdatePMDataOnCustom();
                _accountqty.TotalAllocationQty = iTicketView.TargetQuantity;
                _accountqty.LoadPositions();
                _accountqty.SetAllocationPercentage(iTicketView.AccountText);
                _accountqty.StartPosition = FormStartPosition.CenterParent;

                DialogResult dgResult = _accountqty.ShowDialog((Form)iTicketView);
                if (dgResult == DialogResult.OK)
                {
                    string error = _accountqty.Error;
                    if (!UseQuantityFieldAsNotional)
                        iTicketView.SetLabelMessage(String.Empty);
                    if (!string.IsNullOrEmpty(error))
                    {
                        iTicketView.SetMessageBoxText(error);
                        return;
                    }
                    if (_accountqty.SumPercentage == 100)
                    {
                        iTicketView.AddCustomPreferenceToAccountCombo(_accountqty.AllocationOperationPreference.OperationPreferenceId);
                        //FillSettlementCurrency();
                    }
                }
                else if (dgResult == DialogResult.Cancel)
                {
                    string error = _accountqty.Error;
                    if (!UseQuantityFieldAsNotional)
                        iTicketView.SetLabelMessage(String.Empty);
                    if (!string.IsNullOrEmpty(error))
                    {
                        iTicketView.SetMessageBoxText(error);
                        return;
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

        /// <summary>
        /// Updates the pm data on custom.
        /// </summary>
        protected virtual void UpdatePMDataOnCustom()
        {
        }

        /// <summary>
        /// Gets the order from ticket.
        /// </summary>
        /// <returns></returns>
        internal OrderSingle GetOrderFromTicket()
        {
            if (_orderRequest == null)
            {
                _orderRequest = new OrderSingle();
            }
            try
            {
                IGetOrderSingle getOrder = _orderSingleProvider.GetProvider(_secmasterObj);
                getOrder.GetOrderFromTicket(iTicketView, this, _orderRequest);
                return _orderRequest;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _orderRequest;
        }

        /// <summary>
        /// Gets the name of the receiver unique.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public abstract string getReceiverUniqueName();

        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        /// <param name="loginUser">The login user.</param>
        internal virtual void InitControl(CompanyUser loginUser)
        {
            try
            {
                _loginUser = loginUser;
                _tradeManager = TradeManager.TradeManager.GetInstance();
                DefaultSymbology = SymbologyHelper.DefaultSymbology;
                _userTradingTicketUiPrefs = TradingTktPrefs.UserTradingTicketUiPrefs;
                _companyTradingTicketUiPrefs = TradingTktPrefs.CompanyTradingTicketUiPrefs;
                ExposurePnlCacheManager.UpdateSymbolPositonAndExpose += TicketPresenterBase_UpdateSymbolPositonExposeAndPNL;
                _allocationProxy = new ProxyBase<IAllocationManager>(TradingTicketConstants.LIT_ALLOCATION_END_POINT_ADDRESS_NEW);
                _orderSingleProvider = new OrderSingleProvider();
                MakeProxy();
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
        /// Publishes the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="topicName">Name of the topic.</param>
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (UIValidation.GetInstance().validate((Form)iTicketView))
                {
                    if (((Form)iTicketView).InvokeRequired)
                    {
                        PublishInvokeDelegate publishDelegate = Publish;
                        ((Form)iTicketView).BeginInvoke(publishDelegate, e, topicName);
                    }
                    else
                    {
                        if (e.TopicName == Topics.Topic_SecurityMaster)
                        {
                            Object[] dataList = (Object[])e.EventData;
                            int symbology = (int)DefaultSymbology;

                            foreach (object obj in dataList)
                            {
                                SecMasterBaseObj secMasterObj = (SecMasterBaseObj)obj;
                                UpdateSecurityDetails(secMasterObj);
                            }
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
        }

        /// <summary>
        /// Sends the validated symbol to sm.
        /// </summary>
        /// <param name="e">The <see cref="string"/> instance containing the event data.</param>
        internal void SendValidatedSymbolToSM(EventArgs<string> e)
        {
            try
            {
                if (_securityMaster != null && _securityMaster.IsConnected)
                {
                    Symbol = e.Value;
                    _assetID = int.MinValue;
                    _secmasterObj = null;
                    if (!string.IsNullOrEmpty(e.Value))
                    {
                        SymbolAction = SecMasterConstants.SecurityActions.ADD;
                        SecMasterRequestObj reqObj = new SecMasterRequestObj();

                        String symbol = e.Value.Trim();

                        if (DefaultSymbology >= ApplicationConstants.SymbologyCodes.TickerSymbol)
                        {
                            reqObj.AddData(symbol, DefaultSymbology);
                        }
                        else
                        {
                            if (symbol.Length == 12)
                            {
                                iTicketView.SetLabelMessage(String.Empty);
                                reqObj.AddData(symbol);
                            }
                            else
                            {
                                if (iTicketView != null)
                                {
                                    iTicketView.SetLabelMessage(TradingTicketConstants.MSG_BBGID_IS_INVALID);
                                    return;
                                }
                            }
                        }
                        reqObj.HashCode = GetHashCode();
                        if (iTicketView is ITradingTicketView && CachedDataManager.GetInstance.IsEquityOptionManualValidation())
                            reqObj.UseOptionManualvalidation = true;
                        reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                        reqObj.SymbolDataRowCollection[0].UnderlyingSymbol = this.UnderlyingSymbol;
                        if (iTicketView != null)
                        {
                            if (AuecID != int.MinValue)
                                AuecID = int.MinValue;
                            iTicketView.AwaitSymbolValidation();
                            iTicketView.StopLiveFeed();
                        }
                        _securityMaster.SendRequest(reqObj);
                    }
                }
                else
                {
                    if (iTicketView != null)
                    {
                        iTicketView.SetLabelMessage(TradingTicketConstants.MSG_TRADE_SERVER_CONNECTED);
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

        /// <summary>
        /// Sets the sec master object for ticket.
        /// </summary>
        /// <param name="secmasterObj">The secmaster object.</param>
        internal virtual void SetSecMasterObjForTicket(SecMasterBaseObj secmasterObj)
        {
            try
            {
                _secmasterObj = secmasterObj;
                IGetOrderSingle getOrder = _orderSingleProvider.GetProvider(_secmasterObj);
                getOrder.SetSecMasterObjForTicket(_secmasterObj, this);
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
        /// Symbols the lookup click.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        internal void SymbolLookupClick(string symbol)
        {
            try
            {
                ListEventAargs args = new ListEventAargs();
                Dictionary<String, String> argDict = new Dictionary<string, string>();
                SecMasterUIObj secMasterUI = new SecMasterUIObj();
                secMasterUI.TickerSymbol = secMasterUI.BloombergSymbol = secMasterUI.FactSetSymbol = secMasterUI.ActivSymbol = string.Empty;

                switch (DefaultSymbology)
                {
                    case ApplicationConstants.SymbologyCodes.TickerSymbol:
                        argDict.Add(TradingTicketConstants.LIT_SEARCH_CRITERIA, SecMasterConstants.SearchCriteria.Ticker.ToString());
                        secMasterUI.TickerSymbol = symbol.Trim();
                        break;
                    case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                        argDict.Add(TradingTicketConstants.LIT_SEARCH_CRITERIA, SecMasterConstants.SearchCriteria.Bloomberg.ToString());
                        secMasterUI.BloombergSymbol = symbol.Trim();
                        break;
                    case ApplicationConstants.SymbologyCodes.FactSetSymbol:
                        argDict.Add(TradingTicketConstants.LIT_SEARCH_CRITERIA, SecMasterConstants.SearchCriteria.FactSetSymbol.ToString());
                        secMasterUI.FactSetSymbol = symbol.Trim();
                        break;
                    case ApplicationConstants.SymbologyCodes.ActivSymbol:
                        argDict.Add(TradingTicketConstants.LIT_SEARCH_CRITERIA, SecMasterConstants.SearchCriteria.ActivSymbol.ToString());
                        secMasterUI.ActivSymbol = symbol.Trim();
                        break;
                }

                if (SymbolAction == SecMasterConstants.SecurityActions.ADD)
                {
                    argDict.Add(TradingTicketConstants.LIT_SECMASTER, BinaryFormatter.Serialize(secMasterUI));
                }
                else
                {
                    argDict.Add(TradingTicketConstants.LIT_SYMBOL, symbol.Trim());
                }
                argDict.Add(TradingTicketConstants.LIT_ACTION, SymbolAction.ToString());
                args.argsObject = argDict;

                if (iTicketView != null)
                    iTicketView.LaunchSymbolLookupMethod(args);
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

        /// <summary>
        /// Symbols the search.
        /// </summary>
        /// <param name="e">The <see cref="string"/> instance containing the event data.</param>
        internal void SymbolSearch(EventArgs<string> e)
        {
            if (_securityMaster != null)
            {
                try
                {
                    SecMasterSymbolSearchReq searchReq = new SecMasterSymbolSearchReq(e.Value, DefaultSymbology)
                    {
                        HashCode = GetHashCode()
                    };

                    if (_securityMaster != null && _securityMaster.IsConnected)
                    {
                        _securityMaster.searchSymbols(searchReq);
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

        internal abstract List<int> GetAccountsForExpnlRequest();

        /// <summary>
        /// Handles the UpdateSymbolPositonAndExpose event of the TradingTicketPresenter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        internal void TicketPresenterBase_UpdateSymbolPositonExposeAndPNL(object sender, EventArgs e)
        {
            try
            {
                if (_marketDataEanbled && _enabledL1)
                {
                    if (Symbol != null || Symbol != string.Empty)
                    {
                        List<int> accountIds = GetAccountsForExpnlRequest();
                        StringBuilder errorMessage = new StringBuilder();
                        double position = 0;
                        Dictionary<int, decimal> dictAccountWisePosition = ExpnlServiceConnector.GetInstance().GetPositionForSymbolAndAccounts(Symbol, accountIds, ref errorMessage);
                        if (dictAccountWisePosition != null)
                            position = Convert.ToDouble(dictAccountWisePosition.Values.Sum());

                        double exposure = 0;
                        double dayPNL = 0;
                        if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                        {
                            Dictionary<int, decimal> dictAccountWiseExposure = ExpnlServiceConnector.GetInstance().GetGrossExposureForSymbolAndAccounts(Symbol, accountIds, ref errorMessage);
                            if (dictAccountWiseExposure != null)
                                exposure = Convert.ToDouble(dictAccountWiseExposure.Values.Sum());
                            Dictionary<int, decimal> dictAccountWiseDayPNL = ExpnlServiceConnector.GetInstance().GetDayPNLForSymbolAndAccounts(Symbol, accountIds, ref errorMessage);
                            if (dictAccountWiseDayPNL != null)
                                dayPNL = Convert.ToDouble(dictAccountWiseDayPNL.Values.Sum());
                        }
                        iTicketView.UpdateSymbolPositionExposeAndPNL(position, exposure, dayPNL);
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

        /// <summary>
        /// Update Broker List By Account Filter
        /// </summary>
        /// <param name="accountIds"></param>
        internal void UpdateBrokerListByAccountFilter(List<int> accountIds)
        {
            try
            {
                if (_auecID > 0)
                {
                    ValueList cpList = TTHelperManager.GetInstance().GetCounterparties(_auecID);
                    ValueList cpAccountList = TTHelperManager.GetInstance().GetCounterpartiesFilterByAccount(accountIds, cpList);
                    cpList = cpAccountList;
                    FillBrokeCombo(cpList);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Update Broker List By Account Filter
        /// </summary>
        /// <param name="accountIds"></param>
        internal void UpdateBrokerListByAllocationPrefFilter()
        {
            try
            {
                if (_accountqty != null && _accountqty.AllocationOperationPreference != null)
                {
                    List<int> accountIds = _accountqty.AllocationOperationPreference.GetSelectedAccountsList();
                    ValueList cpList = TTHelperManager.GetInstance().GetCounterparties(_auecID);
                    ValueList cpAccountList = TTHelperManager.GetInstance().GetCounterpartiesFilterByAccount(accountIds, cpList);
                    cpList = cpAccountList;
                    FillBrokeCombo(cpList);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Validates the symbol set by user.
        /// </summary>
        internal void ValidateSymbolSetByUser()
        {
            try
            {
                if (!string.IsNullOrEmpty(_symbol))
                {
                    iTicketView.SetSymbolL1Strip(_symbol);
                    iTicketView.UpdateCaption();
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

        protected abstract bool ValidateQuantityAndOrderSide();
        protected virtual bool GetTradingTicket()
        {
            try
            {
                if (!ValidateQuantityAndOrderSide())
                    return false;
                if (String.IsNullOrEmpty(iTicketView.OrderType))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SELECT_A_VALID_ORDER_TYPE);
                    return false;
                }
                if (iTicketView.IsUseCustodianAsExecutingBroker)
                {
                    DataTable _unmappedAccountsData = new DataTable();
                    _unmappedAccountsData.Columns.Add("Account Name");

                    foreach (KeyValuePair<int, int> kvp in iTicketView.AccountBrokerMapping)
                    {
                        if(kvp.Value < 0)
                        {
                            string accountName = CachedDataManager.GetInstance.GetAccountText(kvp.Key);
                            _unmappedAccountsData.Rows.Add(accountName);
                        }
                    }
                    if (_unmappedAccountsData.Rows.Count > 0)
                    {
                        MessageWithGridPopup messageBox = new MessageWithGridPopup(string.Empty);
                        messageBox.ShowDialog("Please select brokers for the following accounts", _unmappedAccountsData, MessageBoxButtons.OK);
                        return false;
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(iTicketView.Brokerid))
                    {
                        iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SELECT_A_VALID_COUNTER_PARTY);
                        return false;
                    }
                }
                if (String.IsNullOrEmpty(iTicketView.VenueId))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SELECT_A_VALID_VENUE);
                    return false;
                }
                if (String.IsNullOrEmpty(iTicketView.Account))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SELECT_A_VALID__ACCOUNT);
                    return false;
                }
                if (String.IsNullOrEmpty(iTicketView.TIF))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SELECT_A_VALID_TIF);
                    return false;
                }
                String tif = String.IsNullOrEmpty(iTicketView.TIF) ? String.Empty : TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(iTicketView.TIF);
                if (!String.IsNullOrEmpty(tif) && tif == FIXConstants.TIF_GTD)

                {
                    DateTime dt;
                    if (iTicketView.ExpireTime == null || string.IsNullOrEmpty(iTicketView.ExpireTime) || iTicketView.ExpireTime.Equals("N/A"))
                    {
                        iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_VALID_GTDDATE);
                        return false;
                    }
                    else if (iTicketView.ExpireTime != null && DateTime.TryParse(iTicketView.ExpireTime, out dt) && dt.Date < DateTime.Now.Date)
                    {
                        iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_VALID_EXPIRY_DATE_GREATER_THANEUALTO_CURRENTDATE);
                        return false;
                    }
                }
                if ((String.IsNullOrEmpty(iTicketView.TradingAccount)) || Int32.Parse(iTicketView.TradingAccount) == Int32.MinValue)
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SELECT_A_VALID_TRADING_ACCOUNT);
                    return false;
                }
                if (String.IsNullOrEmpty(iTicketView.HandlingInstruction))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SELECT_A_VALID_HANDLING_INSTRUCTION);
                    return false;
                }
                if (String.IsNullOrEmpty(iTicketView.ExecutionInstructions))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SELECT_A_VALID_EXECUTION_INSTRUCTION);
                    return false;
                }
                if (CurrencyId == int.MinValue)
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_CURRENCY_INFORMATION_NOT_AVAILABLE);
                    return false;
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
            return true;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (_allocationProxy != null)
                    {
                        _allocationProxy.Dispose();
                    }
                    if (_securityMaster != null)
                    {
                        _securityMaster.SecMstrDataResponse -= _securityMaster_SecMstrDataResponse;
                        _securityMaster.Disconnected -= _securityMaster_Disconnected;
                        _securityMaster.Connected -= _securityMaster_Connected;
                        _securityMaster.SecMstrDataSymbolSearcResponse -= _securityMaster_SecMstrDataSymbolSearcResponse;
                    }
                    if (_subscribeProxy != null)
                    {
                        _subscribeProxy.InnerChannel.UnSubscribe(Topics.Topic_SecurityMaster);
                        _subscribeProxy.Dispose();
                        _subscribeProxy = null;
                    }
                    if (_accountqty != null)
                    {
                        _accountqty.Dispose();
                    }
                    ExposurePnlCacheManager.UpdateSymbolPositonAndExpose -= TicketPresenterBase_UpdateSymbolPositonExposeAndPNL;
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

        /// <summary>
        /// Binds the order side.
        /// </summary>
        protected void BindOrderSide()
        {
            try
            {
                DataTable sides = CachedDataManager.GetInstance.GetOrderSides(_assetID).Copy();
                iTicketView.FillOrderSide(sides);
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

        /// <summary>
        /// Binds the type of the order.
        /// </summary>
        protected void BindOrderType()
        {
            try
            {
                DataTable types = CachedDataManager.GetInstance.GetOrderTypes().Copy();
                iTicketView.FillOrderType(types);
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
        /// Binds the tif.
        /// </summary>
        protected void BindTIF()
        {
            try
            {
                DataTable tifs = CachedDataManager.GetInstance.GetTIFS().Copy();
                iTicketView.FillTIF(tifs);
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
        /// Creates the pricing service proxy.
        /// </summary>

        /// <summary>
        /// Fills the account combo.
        /// </summary>
        /// <returns></returns>
        protected DataTable FillAccountCombo()
        {
            try
            {
                DataTable accountsAndAllocationDefaults = GetAccountAndAllocationPrefTable();
                DataTable accountsAndAllocationDefaultsCloned = accountsAndAllocationDefaults.Clone();
                DataRow r;

                foreach (DataRow row in accountsAndAllocationDefaults.Rows)
                {
                    r = accountsAndAllocationDefaultsCloned.NewRow();
                    r[0] = row[0];
                    r[1] = row[1];
                    r[2] = row[2];
                    accountsAndAllocationDefaultsCloned.Rows.Add(r);
                }

                DataTable sortTable = accountsAndAllocationDefaultsCloned.Clone();
                //for select entry in drop down
                r = sortTable.NewRow();
                r[0] = accountsAndAllocationDefaultsCloned.Rows[0][0];
                r[1] = accountsAndAllocationDefaultsCloned.Rows[0][1];
                r[2] = accountsAndAllocationDefaultsCloned.Rows[0][2];
                sortTable.Rows.Add(r);
                string selectRowValueID = r[OrderFields.PROPERTY_LEVEL1ID].ToString();

                DataView dv = accountsAndAllocationDefaultsCloned.DefaultView;
                dv.Sort = OrderFields.PROPERTY_LEVEL1NAME;
                accountsAndAllocationDefaultsCloned = dv.ToTable();

                foreach (DataRow row in accountsAndAllocationDefaultsCloned.Rows.Cast<DataRow>().Where(row => row[OrderFields.PROPERTY_LEVEL1ID].ToString() != selectRowValueID))
                {
                    r = sortTable.NewRow();
                    r[0] = row[0];
                    r[1] = row[1];
                    r[2] = row[2];
                    sortTable.Rows.Add(r);
                }

                if (sortTable.Rows[0].ItemArray[0].ToString().Equals(ApplicationConstants.C_COMBO_SELECT))
                    sortTable.Rows[0].ItemArray[0] = TradingTicketConstants.LIT_UNALLOCATED;

                return sortTable;

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

        /// <summary>
        /// Fills the broke combo.
        /// </summary>
        /// <param name="cpList">The cp list.</param>
        protected void FillBrokeCombo(ValueList cpList)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(TradingTicketConstants.LIT_VALUE);
                dt.Columns.Add(TradingTicketConstants.LIT_DISPLAY);
                cpList.SortStyle = ValueListSortStyle.Ascending;
                foreach (ValueListItem t in cpList.ValueListItems)
                {
                    dt.Rows.Add(t.DataValue, t.DisplayText);
                }
                iTicketView.FillBrokerComboValue(dt);
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
        /// Gets the account and allocation preference table.
        /// </summary>
        /// <returns></returns>
        protected DataTable GetAccountAndAllocationPrefTable()
        {
            try
            {
                DataTable accountsAndAllocationRules = new DataTable();
                accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_LEVEL1ID);
                accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_LEVEL1NAME);
                accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE);

                AccountCollection _userAccounts = CachedDataManager.GetInstance.GetUserAccounts();

                if (_userAccounts != null)
                {
                    foreach (Account userAccount in _userAccounts)
                    {
                        DataRow accountRow = accountsAndAllocationRules.NewRow();
                        accountRow[OrderFields.PROPERTY_LEVEL1ID] = userAccount.AccountID;
                        accountRow[OrderFields.PROPERTY_LEVEL1NAME] = userAccount.Name;
                        if (userAccount.AccountID != int.MinValue)
                        {
                            accountRow[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE] = false;
                        }
                        else
                        {
                            accountRow[OrderFields.PROPERTY_LEVEL1NAME] = ApplicationConstants.C_LIT_UNALLOCATED;
                            accountRow[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE] = true;
                        }
                        accountsAndAllocationRules.Rows.Add(accountRow);
                    }
                    // Prana-9688: If trade server hang at the very same moment when TT is opened, then below pref is not able to be fetched via proxy and gives error.
                    // which interrupts further flow and accounts are not binded which led to Object reference error while accessing value of binded accounts.
                    // So applied handling here not to bother the further executions.
                    try
                    {
                        Dictionary<int, string> preferences = _allocationProxy.InnerChannel.GetAllocationPreferences(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID,
                                AllocationSubModulePermission.IsLevelingPermitted,
                                AllocationSubModulePermission.IsProrataByNavPermitted);
                        if (preferences != null)
                        {
                            foreach (int prefId in preferences.Keys)
                            {
                                DataRow accountRow = accountsAndAllocationRules.NewRow();
                                accountRow[OrderFields.PROPERTY_LEVEL1ID] = prefId;
                                accountRow[OrderFields.PROPERTY_LEVEL1NAME] = preferences[prefId];
                                accountRow[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE] = true;
                                accountsAndAllocationRules.Rows.Add(accountRow);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        if (rethrow)
                        {
                            throw;
                        }
                    }
                }
                return accountsAndAllocationRules;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Get Data  For Allocation And Broker Combo
        /// </summary>
        /// <param name="fundId"></param>
        /// <param name="cpList"></param>
        /// <param name="accountsData"></param>
        /// <returns></returns>
        protected ValueList GetDataForAllocationAndBrokerCombo(int fundId, ValueList cpList, DataTable accountsData)
        {
            List<int> accountIds = new List<int>();
            cpList = TTHelperManager.GetInstance().GetCounterparties(_auecID);
            //if master-fund selected then filter accounts for selected master fund and brokers further filter on Accounts and AUEC
            if (fundId > 0)
            {
                int maxRows = accountsData.Rows.Count;
                maxRows -= 1;
                for (int i = maxRows; i >= 0; i--)
                {
                    int accountId = Convert.ToInt32(accountsData.Rows[i][OrderFields.PROPERTY_LEVEL1ID].ToString());
                    bool isDefaultRule = Convert.ToBoolean(accountsData.Rows[i][OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE].ToString());
                    if (!isDefaultRule || accountId < 0)
                    {

                        var fundIdForAccount = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(accountId);
                        if (accountId > 0 && fundId != fundIdForAccount)
                        {

                            accountsData.Rows.RemoveAt(i);
                        }
                        else
                        {
                            accountIds.Add(accountId);
                        }
                    }
                    else
                    {
                        AllocationOperationPreference tempAllocationOperationPreference = ServiceManager.Instance.AllocationManager.InnerChannel.GetPreferenceById(accountId);
                        if (tempAllocationOperationPreference != null && tempAllocationOperationPreference.TargetPercentage.Count > 0)
                        {
                            List<int> accountIdsforAllocationScheme = tempAllocationOperationPreference.GetSelectedAccountsList();
                            List<int> accountsOfSelectedFund = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation()[fundId];
                            if (accountIdsforAllocationScheme.All(x => accountsOfSelectedFund.Contains(x)))
                            {
                                accountIds.AddRange(accountIdsforAllocationScheme);
                            }
                            else
                            {
                                accountsData.Rows.RemoveAt(i);
                            }
                        }
                        else
                        {
                            accountsData.Rows.RemoveAt(i);
                        }

                    }
                }
            }
            else
            {
                //if master-fund NOT selected then Show all accounts and filter brokers further
                int maxRows = accountsData.Rows.Count;
                maxRows -= 1;
                for (int i = maxRows; i >= 0; i--)
                {
                    int accountId = Convert.ToInt32(accountsData.Rows[i][OrderFields.PROPERTY_LEVEL1ID].ToString());
                    var fundIdForAccount = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(accountId);
                    if (fundIdForAccount > 0 || accountId == int.MinValue)
                    {
                        accountIds.Add(accountId);
                    }
                    else
                    {
                        accountsData.Rows.RemoveAt(i);

                    }
                }
            }

            //brokers filter on Accounts and AUEC
            ValueList cpAccountList = TTHelperManager.GetInstance().GetCounterpartiesFilterByAccount(accountIds, cpList);
            cpList = cpAccountList;
            return cpList;
        }

        /// <summary>
        /// create proxy for security master publishing service subscription
        /// </summary>
        protected void MakeProxy()
        {
            try
            {
                _subscribeProxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _subscribeProxy.Subscribe(Topics.Topic_SecurityMaster, null);
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

        /// <summary>
        /// Updates the security details.
        /// </summary>
        /// <param name="secMasterObj">The sec master object.</param>
        protected abstract void UpdateSecurityDetails(SecMasterBaseObj secMasterObj);

        /// <summary>
        /// Validate Trade With Algo Controls
        /// </summary>
        /// <returns></returns>
        protected abstract string ValidateTradeWithAlgoControls();

        /// <summary>
        /// Handles the Connected event of the _securityMaster control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void _securityMaster_Connected(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate((Form)iTicketView))
                {
                    if (((Form)iTicketView).InvokeRequired)
                    {
                        ConnectionInvokeDelegate connectionStatusDelegate = _securityMaster_Connected;
                        ((Form)iTicketView).BeginInvoke(connectionStatusDelegate, sender, e);
                    }
                    else
                    {
                        iTicketView.SetLabelMessage(TradingTicketConstants.MSG_TRADE_SERVER_CONNECTED);
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

        /// <summary>
        /// Handles the Disconnected event of the _securityMaster control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void _securityMaster_Disconnected(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate((Form)iTicketView))
                {
                    if (((Form)iTicketView).InvokeRequired)
                    {
                        ConnectionInvokeDelegate connectionStatusDelegate = _securityMaster_Disconnected;
                        ((Form)iTicketView).BeginInvoke(connectionStatusDelegate, sender, e);
                    }
                    else
                    {
                        iTicketView.SetLabelMessage(TradingTicketConstants.MSG_TRADE_SERVER_DISCONNECTED);
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

        /// <summary>
        /// Handles the SecMstrDataResponse event of the _securityMaster control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{SecMasterBaseObj}"/> instance containing the event data.</param>
        protected void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: TradingTicketPresenter._securityMaster_SecMstrDataResponse() entered for Symbol: {0}, Time: {1}", e.Value.TickerSymbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                }

                SecMasterBaseObj secMasterObj = e.Value;
                if (secMasterObj.AssetID == (int)AssetCategory.FX || secMasterObj.AssetID == (int)AssetCategory.FXForward)
                {
                    if (FXandFXFWDSymbolGenerator.IsValidFxAndFwdSymbol(secMasterObj))
                    {
                        UpdateSecurityDetails(secMasterObj);
                    }
                }
                else
                {
                    UpdateSecurityDetails(secMasterObj);
                }

                if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: TradingTicketPresenter._securityMaster_SecMstrDataResponse() exited for Symbol: {0}, Time: {1}", e.Value.TickerSymbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
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

        /// <summary>
        /// Handles the SecMstrDataSymbolSearcResponse event of the _securityMaster control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{SecMasterSymbolSearchRes}"/> instance containing the event data.</param>
        protected void _securityMaster_SecMstrDataSymbolSearcResponse(object sender, EventArgs<SecMasterSymbolSearchRes> e)
        {
            try
            {
                iTicketView.UpdateDropDown(e.Value.StartWith, e.Value.Result);
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
    }
}