using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Rebalancer;
using Prana.Rebalancer.PercentTradingTool.ViewModel;
using Prana.TradeManager;
using Prana.TradeManager.Extension;
using Prana.TradingTicket.Forms;
using Prana.TradingTicket.TTView;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;

namespace Prana.TradingTicket.TTPresenter
{
    /// <summary>
    /// presenter class which contains logic for communication with business object and binding logic  
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="Prana.Interfaces.ILiveFeedCallback" />
    /// <seealso cref="IPublishing" />
    internal class TradingTicketPresenter : TicketPresenterBase, ILiveFeedCallback
    {
        public bool IsTradeSending = false;
        private bool isShowTargetQTY;

        /// <summary>
        /// The _pricing services proxy
        /// </summary>
        protected DuplexProxyBase<IPricingService> _pricingServicesProxy;

        BindableValueList[] vls = null;
        /// <summary>
        /// The _outgoing order request
        /// </summary>
        protected OrderSingle _outgoingOrderRequest = new OrderSingle();

        /// <summary>
        /// The view allocation details window
        /// </summary>
        ViewAllocationDetailsWindow viewAllocationDetailsWindow;

        /// <summary>
        /// The _is save checked in preference
        /// </summary>Supp
        protected bool _isSaveCheckedInPref;

        /// <summary>
        /// The _company base currency identifier
        /// </summary>
        protected int _companyBaseCurrencyID = int.MinValue;

        // this field captures the Ticker symbol when TT is opened from PTT(first time). then if symbol is changed it is compared to the newly entered symbol
        // to check if TT UI Control need to be refreshed or not
        /// <summary>
        /// The PTT symbol
        /// </summary>
        protected string pttSymbol = String.Empty;

        /// <summary>
        /// The _lead currency identifier
        /// </summary>
        protected int _leadCurrencyID = int.MinValue;
        /// <summary>
        /// The _VS currency identifier
        /// </summary>
        protected int _vsCurrencyID = int.MinValue;
        /// <summary>
        /// The _is target quantity same as total qty
        /// </summary>
        protected bool _isTargetQuantitySameAsTotalQty = true;
        /// <summary>
        /// The min_ qty
        /// </summary>
        protected decimal Min_Qty = 0;
        /// <summary>
        /// The max_ qty
        /// </summary>
        protected decimal Max_Qty = 999999999;
        protected bool _isShowAlgoControls = false;

        /// <summary>
        /// The _incoming order request
        /// </summary>
        protected OrderSingle _incomingOrderRequest;
        /// <summary>
        /// The _trading ticket type
        /// </summary>
        protected TradingTicketType _tradingTicketType = TradingTicketType.Manual;

        /// <summary>
        /// The _order single PTT
        /// </summary>
        protected OrderSingle _orderSinglePTT = null;
        /// <summary>
        /// The price symbol settings
        /// </summary>
        protected PriceSymbolValidation priceSymbolSettings;
        /// <summary>
        /// Gets the company base currency identifier.
        /// </summary>
        /// <value>
        /// The company base currency identifier.
        /// </value>
        internal int CompanyBaseCurrencyID
        {
            get { return _companyBaseCurrencyID; }
        }

        internal bool IsTradeSuccessful { get; set; }

        internal bool IsComingPM { get; set; }
        internal bool IsComingPMForIncrease { get; set; }
        internal bool IsTradeSucceeded { get; set; }

        internal bool EnterTargetQuantityInPercentage { get; set; }

        internal Dictionary<int, double> AccountWithPMPostionsForCustomPref { get; set; }

        internal bool IsManualOrder { get; set; }
        /// <summary>
        /// IsShowTargetQTY
        /// </summary>
        internal bool IsShowTargetQTY
        {
            get { return isShowTargetQTY; }
            set { isShowTargetQTY = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is show algo controls.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is show algo controls; otherwise, <c>false</c>.
        /// </value>
        internal bool IsShowAlgoControls
        {
            get { return _isShowAlgoControls; }
            set { _isShowAlgoControls = value; }
        }

        /// <summary>
        /// Gets or sets the incoming order request.
        /// </summary>
        /// <value>
        /// The incoming order request.
        /// </value>
        internal OrderSingle IncomingOrderRequest
        {
            get { return _incomingOrderRequest; }
        }

        /// <summary>
        /// Gets or sets the notional value.
        /// </summary>
        /// <value>
        /// The notional value.
        /// </value>
        internal double NotionalValue { get; set; }

        internal TradingTicketParent TradingTicketParentType { get; set; }

        /// <summary>
        /// Gets or sets the order request.
        /// </summary>
        /// <value>
        /// The order request.
        /// </value>
        internal OrderSingle OrderRequest
        {
            get { return _orderRequest; }
            set { _orderRequest = value; }
        }

        /// <summary>
        /// Gets or sets the type of the ticket.
        /// </summary>
        /// <value>
        /// The type of the ticket.
        /// </value>
        internal TradingTicketType TicketType
        {
            get { return _tradingTicketType; }
            set { _tradingTicketType = value; }
        }

        /// <summary>
        /// The _lead currency identifier
        /// </summary>
        internal int LeadCurrencyId
        {
            get { return _leadCurrencyID; }
            set { _leadCurrencyID = value; }
        }

        /// <summary>
        /// The _VS currency identifier
        /// </summary>
        internal int VsCurrencyId
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is target quantity same as total qty.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is target quantity same as total qty; otherwise, <c>false</c>.
        /// </value>
        internal bool IsTargetQuantitySameAsTotalQty
        {
            get { return _isTargetQuantitySameAsTotalQty; }
            set { _isTargetQuantitySameAsTotalQty = value; }
        }

        /// <summary>
        /// Gets or sets the allocation preference identifier.
        /// </summary>
        /// <value>
        /// The allocation preference identifier.
        /// </value>
        internal int AllocationPrefId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is Clean Details after is checked preference.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save Clean Details; otherwise, <c>false</c>.
        /// </value>
        internal bool CleanDetailsAfterTrade { get; set; }

        /// <summary>
        /// The i trading ticket view
        /// </summary>
        protected new ITradingTicketView iTicketView;

        /// <summary>
        /// Adds the specified object i trading ticket view.
        /// </summary>
        /// <param name="objITradingTicketView">The object i trading ticket view.</param>
        internal override void Add(ITicketView objITradingTicketView)
        {
            base.Add(objITradingTicketView);
            iTicketView = (ITradingTicketView)objITradingTicketView;
        }

        protected void CreatePricingServiceProxy()
        {
            _pricingServicesProxy = new DuplexProxyBase<IPricingService>(TradingTicketConstants.LIT_PRICING_SERVICE_ADDRESS, this);
        }

        /// <summary>
        /// Gets the name of the receiver unique.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string getReceiverUniqueName()
        {
            try
            {
                return "TradingTicket";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Sends the symbology missing message.
        /// </summary>
        /// <param name="symbolcon">The symbolcon.</param>
        /// <param name="symbol">The symbol.</param>
        protected void SendSymbologyMissingMessage(string symbolcon, string symbol)
        {
            try
            {
                try
                {
                    ListEventAargs args = new ListEventAargs();

                    Dictionary<String, String> argDict = new Dictionary<string, string>();
                    SecMasterUIObj secMasterUI = new SecMasterUIObj();

                    //Set symbology based args
                    int symbology = (int)DefaultSymbology;
                    if (symbology == 5)
                    {
                        argDict.Add(TradingTicketConstants.LIT_SEARCH_CRITERIA, SecMasterConstants.SearchCriteria.Bloomberg.ToString());
                        secMasterUI.BloombergSymbol = symbol.Trim();
                        secMasterUI.TickerSymbol = string.Empty;
                    }
                    else
                    {
                        argDict.Add(TradingTicketConstants.LIT_SEARCH_CRITERIA, SecMasterConstants.SearchCriteria.Ticker.ToString());
                        secMasterUI.TickerSymbol = symbol.Trim();
                        secMasterUI.BloombergSymbol = string.Empty;
                    }
                    argDict.Add(TradingTicketConstants.LIT_SYMBOL, symbol.Trim());

                    //set action for Add/ Search/ Approve
                    argDict.Add(TradingTicketConstants.LIT_ACTION, SecMasterConstants.SecurityActions.UPDATE.ToString());
                    argDict.Add(TradingTicketConstants.LIT_SYMBOLOGY_CODE, symbolcon);

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
        /// Gets the limit pice.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        internal void GetLimitPice()
        {
            try
            {
                if (AssetID == (int)AssetCategory.FX || AssetID == (int)AssetCategory.FXForward || AssetID == (int)AssetCategory.Forex)
                {
                    fxInfo fxReqObj = new fxInfo();
                    fxReqObj.PranaSymbol = _symbol;
                    fxReqObj.FromCurrencyID = _leadCurrencyID;
                    fxReqObj.ToCurrencyID = _vsCurrencyID;
                    fxReqObj.CategoryCode = (AssetCategory)AssetID;
                    List<fxInfo> listFxSymbols = new List<fxInfo> { fxReqObj };
                    _pricingServicesProxy.InnerChannel.RequestSnapshot(listFxSymbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, null, true);
                }
                else
                    _pricingServicesProxy.InnerChannel.RequestSnapshot(new List<String> { _symbol }, ApplicationConstants.SymbologyCodes.TickerSymbol, false);
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

        /// <summary>
        /// Updates the order request.
        /// </summary>
        /// <param name="l1data">The l1data.</param>
        internal void UpdateOrderRequest(SymbolData l1data)
        {
            try
            {
                if (priceSymbolSettings != null)
                {
                    if (priceSymbolSettings.ValidateSymbolCheck)
                    {
                        if (l1data.Symbol == Symbol && l1data.UnderlyingCategory != Underlying.None)
                        {
                            //Check if the AssetID and Underlying ID received from Feed is the same as the ticket that is opened
                            if (AssetID != (int)l1data.CategoryCode || UnderlyingID != (int)l1data.UnderlyingCategory)
                            {
                                AuecID = int.MinValue;
                                return;
                            }
                        }
                    }
                }
                if (l1data.CategoryCode == AssetCategory.EquityOption)
                {
                    OptionSymbolData optionl1data = l1data as OptionSymbolData;

                    if (optionl1data != null && optionl1data.PutOrCall != OptionType.NONE)
                    {
                        _orderRequest.PutOrCalls = optionl1data.PutOrCall.ToString();
                    }
                    if (optionl1data != null && optionl1data.StrikePrice != double.MinValue)
                    {
                        _orderRequest.StrikePrice = optionl1data.StrikePrice;
                    }
                    if (optionl1data != null && optionl1data.ExpirationDate != DateTimeConstants.MinValue)
                    {
                        _orderRequest.ExpirationDate = optionl1data.ExpirationDate;
                        _orderRequest.MaturityDay = optionl1data.ExpirationDate.ToString();
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
        /// Sets the order request.
        /// </summary>
        /// <param name="order">The order.</param>
        internal void SetOrderRequest(OrderSingle order)
        {
            try
            {
                _orderRequest = order;
                if (_orderRequest.TransactionSource == TransactionSource.PST)
                    _orderSinglePTT = DeepCopyHelper.Clone(_orderRequest);
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

        internal bool AddCustomPreferenceToAccountCombo(int prefID, string accountIdsList = "")
        {
            bool isPreferenceAvailable = false;
            try
            {
                AllocationOperationPreference operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, prefID);
                if (operationPreference != null && operationPreference.OperationPreferenceName.Contains(TradingTicketConstants.ALLOCATION_PREF_CUSTOM))
                {
                    isPreferenceAvailable = true;
                    if (_accountqty == null || _accountqty.IsDisposed)
                    {
                        _accountqty = new AccountQty(_symbol, FundId);
                        _accountqty.AllocationManager = _allocationProxy.InnerChannel;
                    }
                    _accountqty.AllocationOperationPreference = operationPreference;
                    _accountqty.IsReloadOrderRequest = true;
                    _accountqty.StartPosition = FormStartPosition.CenterParent;
                    _accountqty.TotalAllocationQty = iTicketView.TargetQuantity;
                    iTicketView.AddCustomPreferenceToAccountCombo(operationPreference.OperationPreferenceId);
                }
                if (prefID == -1)//From PM, Account as Multiple
                {
                    _accountqty = new AccountQty(_symbol, FundId);
                    _accountqty.AllocationManager = _allocationProxy.InnerChannel;
                    _accountqty.isComingFromPM = true;
                    _accountqty.AccountWithPostions = AccountWithPMPostionsForCustomPref;
                    _accountqty.TotalAllocationQty = iTicketView.TargetQuantity;
                    if (_incomingOrderRequest.PMType == PMType.Increase)
                        _accountqty.isComingFromPMForIncrease = true;
                    _accountqty.LoadPositions();
                    _accountqty.createAllocationPreferecne();
                    _accountqty.StartPosition = FormStartPosition.CenterParent;
                    if (_accountqty.SumPercentage == 100)
                    {
                        iTicketView.AddCustomPreferenceToAccountCombo(_accountqty.AllocationOperationPreference.OperationPreferenceId);
                        FillSettlementCurrency();
                    }
                    isPreferenceAvailable = true;
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
            return isPreferenceAvailable;
        }

        /// <summary>
        /// Determines whether selected selected allocation is simple account or simple calculated pref.
        /// </summary>
        /// <returns></returns>
        public bool IsAllocationValidForCustodianBrokerPref(int prefId)
        {
            try
            {
                AllocationOperationPreference operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, prefId);
                if (!String.IsNullOrEmpty(CachedDataManager.GetInstance.GetAccount(prefId)) || TradeManager.TradeManager.GetInstance().IsAllocationPrefValidForCustodianBroker(IncomingOrderRequest, operationPreference))
                {
                    return true;
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
            return false;
        }

        /// <summary>
        /// This method is to check if all the selected accounts are mapped
        /// </summary>
        /// <param name="prefId"></param>
        /// <returns></returns>
        public bool AreAllSelectedAccountsMapped(int prefId)
        {
            try
            {
                AllocationOperationPreference operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, prefId);
                Dictionary<int, int> accountWiseExecutingBroker = CachedDataManager.GetInstance.GetAccountWiseExecutingBrokerMapping();
                if (operationPreference == null)
                {
                    return accountWiseExecutingBroker.ContainsKey(prefId) && accountWiseExecutingBroker[prefId] != int.MinValue;
                }
                foreach(var accountid in operationPreference.TargetPercentage.Keys)
                {
                    if(!accountWiseExecutingBroker.ContainsKey(accountid) || accountWiseExecutingBroker[accountid] == int.MinValue)
                    {
                        return false;
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
            return true;
        }

        /// <summary>
        /// Gets the account broker mapping for selected fund
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, int> GetAccountBrokerMappingForSelectedFund(int fundId, ValueList brokersValueList)
        {
            Dictionary<int, int> accountBrokerMapping = new Dictionary<int, int>();
            try
            {
                AllocationOperationPreference operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, fundId);
                accountBrokerMapping = TradeManager.TradeManager.GetInstance().GetAccountBrokerMappingForSelectedFund(fundId, brokersValueList, operationPreference, _incomingOrderRequest);
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return accountBrokerMapping;
        }

        /// <summary>
        /// Determines whether [is live feed connected].
        /// </summary>
        /// <returns></returns>
        protected bool IsLiveFeedConnected()
        {
            try
            {
                return _pricingServicesProxy.InnerChannel.IsLiveFeedActive;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        internal void CreateStageAndLiveOrder()
        {
            try
            {
                bool CompliancePriceCheckRequired = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("CompliancePriceCheckRequired"));

                if (CompliancePriceCheckRequired && _tradingTicketType == TradingTicketType.Live && iTicketView.Price == Convert.ToDecimal(0) && ComplianceCacheManager.GetPreTradeCheck(_loginUser.CompanyUserID))
                {
                    if (this.PriceForComplianceNotAvailable != null)
                    {
                        this.PriceForComplianceNotAvailable(this, new EventArgs());
                    }
                    IsTradeSuccessful = false;
                    return;
                }

                if (CompliancePriceCheckRequired &&  _tradingTicketType == TradingTicketType.Stage && iTicketView.Price == Convert.ToDecimal(0) && ComplianceCacheManager.GetPreTradeCheckStaging(_loginUser.CompanyUserID))
                {
                    if (this.PriceForComplianceNotAvailable != null)
                    {
                        this.PriceForComplianceNotAvailable(this, new EventArgs());
                    }
                    IsTradeSuccessful = false;
                    return;
                }
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && ((_tradingTicketType == TradingTicketType.Stage && ComplianceCacheManager.GetPreTradeCheckStaging(_loginUser.CompanyUserID)) || (_tradingTicketType == TradingTicketType.Live && ComplianceCacheManager.GetPreTradeCheck(_loginUser.CompanyUserID))))
                {
                    iTicketView.SetMessageBoxTextAndGetDialogResult(ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE_COMPLIANCE, ClientLevelConstants.HEADER_MARKET_DATA_ALERT, MessageBoxButtons.OK);
                    return;
                }
                /* if (!_isPricingAvailable && ComplianceCacheManager.GetPreTradeCheckStaging(_loginUser.CompanyUserID) && !_isUnallocatedTrade)
                {
                    DialogResult dr = iTicketView.SetMessageBoxTextAndGetDialogResult(TradingTicketConstants.MSG_ALLOW_TRADE, TradingTicketConstants.HEADER_PRICES_NOT_AVAILABLE, MessageBoxButtons.YesNo);
                    if (dr != DialogResult.Yes)
                        return;
                } */

                if (AuecID == int.MinValue)
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SYMBOL_CANNOT_VERIFIED);
                    return;
                }


                OrderSingle or = null;
                bool isOrderValidated=false;
                bool isValidTicket = _tradingTicketType == TradingTicketType.Live ? GetTradingTicket() : GetTradingTicketForStage();
                if (isValidTicket)
                {
                    or = GetOrderFromTicket();

                    string error = ValidateTradeWithAlgoControls();

                    if (error != string.Empty)
                    {
                        iTicketView.SetMessageBoxText(error);
                        IsTradeSuccessful = false;
                        return;
                    }
                    or.IsManualOrder = false;
                    if (_tradingTicketType == TradingTicketType.Stage)
                        or.IsStageRequired = false;
                    else if (or != null)
                        or.IsStageRequired = true;
                }
                bool checkPreTrade = true; //if live trade is sending out as stage then dont check compliance. default check is true and permission
                if (or == null)
                {
                    IsTradeSuccessful = false;
                    return;
                }
                AllocationOperationPreference aop = or != null ? _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, or.Level1ID) : null;
                if (or != null && or.TransactionSource == TransactionSource.PST)
                {
                    //or.PSTAllocationPreferenceID = or.Level1ID;
                    if (aop != null && aop.TargetPercentage.Count == 1 && (aop.CheckListWisePreference == null || aop.CheckListWisePreference.Count == 0))
                    {
                        or.Level1ID = aop.TargetPercentage.Keys.First();
                    }
                }

                if (IncomingOrderRequest == null && _tradingTicketType == TradingTicketType.Live)
                {
                    if (or.MsgType != FIXConstants.MSGOrderCancelReplaceRequest)
                    {
                        or.IsStageRequired = true;
                    }
                    checkPreTrade = false;//if live trade is sending out as stage then dont check compliance.
                }

                if (priceSymbolSettings.RiskCtrlCheck || priceSymbolSettings.ValidateSymbolCheck)
                {

                    if (!IsLiveFeedConnected())
                    {
                        if (iTicketView.SetMessageBoxTextAndGetDialogResult(GetErrorString(), String.Empty, MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (priceSymbolSettings.RiskCtrlCheck) // if to validate price
                        {
                            if (or.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit)// if selected order type is Limit
                            {
                                if (!TradeManagerCore.GetInstance().IsWithinLimits(or, _marketPrice))
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
                IsTradeSuccessful = true;
                or.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                or.ClientTime = DateTime.Now.ToUniversalTime().ToLongTimeString();
                or.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingNew.ToString());
                or.AUECID = AuecID;
                SetExpireTime(or, null);
                or.IsPricingAvailable = _isPricingAvailable;

                    if (or.TransactionSource == TransactionSource.None || or.TransactionSource == TransactionSource.FIX || or.TransactionSource == TransactionSource.PM || or.TransactionSource == TransactionSource.Blotter)
                    {
                        or.TransactionSource = TransactionSource.TradingTicket;
                        or.TransactionSourceTag = (int)TransactionSource.TradingTicket;
                    }

                if (checkPreTrade && ComplianceCacheManager.GetPreTradeCheckStaging(_loginUser.CompanyUserID) && _tradingTicketType == TradingTicketType.Stage)
                {
                    if (ValidationManager.ValidateOrder(or, _loginUser.CompanyUserID, true))
                    {
                        isOrderValidated = true;
                        List<OrderSingle> orders = new List<OrderSingle>() { or };
                        if (or.AssetID != (int)AssetCategory.FX && !ComplianceCommon.ValidateOrderInCompliance_New(orders, (Form)iTicketView, _loginUser.CompanyUserID, true))
                        {
                            IsTradeSuccessful = false;
                            return;
                        }
                    }
                    else
                    {
                        IsTradeSuccessful = false;
                        return;
                    }
                }
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow Out1] Before SendBlotterTrades In TradingTicketPresenter, Fix Message: userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ",Symbol: " + Convert.ToString(or.Symbol) + " , OrderID: " + Convert.ToString(or.OrderID) + " , Quantity: " + Convert.ToString(or.Quantity) + " , Transaction Time: " + Convert.ToString(or.TransactionTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                }
                bool isTradingRulesPassed = true;
                if (_tradingTicketType != TradingTicketType.Stage && (TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(or.CounterPartyID) == PranaInternalConstants.ConnectionStatus.CONNECTED || (or.IsUseCustodianBroker && TradeManagerExtension.GetInstance().CheckAllFixConnectionsStatus(or, iTicketView.AccountBrokerMapping).Count == 0)))
                {
                    isTradingRulesPassed = TradingRulesValidator.ValidateCompanyTradingRules(or, (double)iTicketView.Price, UseQuantityFieldAsNotional);
                }

                if(!isTradingRulesPassed ||
                   (iTicketView.IsUseCustodianAsExecutingBroker && !_tradeManager.SendBlotterMultipleTrades(or, iTicketView.AccountBrokerMapping, aop, _tradingTicketType == TradingTicketType.Stage, true, isOrderValidated)) ||
                   (!iTicketView.IsUseCustodianAsExecutingBroker && !_tradeManager.SendBlotterTrades(or, _marketPrice, true ,isOrderValidated)))
                {
                    IsTradeSucceeded = false;
                    IsTradeSuccessful = false;
                }

                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow Out1] After SendBlotterTrades In TradingTicketPresenter, Fix Message: userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ", Symbol: " + Convert.ToString(or.Symbol) + " , OrderID: " + Convert.ToString(or.OrderID) + " , Quantity: " + Convert.ToString(or.Quantity) + " , Transaction Time: " + Convert.ToString(or.TransactionTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                }
                if (IsTradeSuccessful)
                {
                    if (!_isSaveCheckedInPref)
                        iTicketView.CloseTicket();

                    //IF the ticket is not closed remember to reset the PranaMsgType else 
                    // it would create problems on the next trade.
                    if (_isSaveCheckedInPref)
                    {
                        _orderRequest.PranaMsgType = (int)OrderFields.PranaMsgTypes.InternalOrder;
                        or.PranaMsgType = (int)OrderFields.PranaMsgTypes.InternalOrder;
                        if (or.TransactionSource == TransactionSource.PST && IsTradeSuccessful)
                        {
                            ResetPTTDetailsAndReloadTT(or, true);
                        }
                        if (CleanDetailsAfterTrade)
                            iTicketView.ResetTicket();
                    }
                }
            }
            catch (Exception ex)
            {
                IsPricingAvailable = false;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void SetExpireTime(OrderSingle or, OrderSingle replaceOrder)
        {
            try
            {

                if (or.TIF == FIXConstants.TIF_GTD && !string.IsNullOrEmpty(iTicketView.ExpireTime))
                {
                    DateTime dtValue;
                    if (DateTime.TryParse(iTicketView.ExpireTime, out dtValue))
                    {
                        DateTime Dt = dtValue.Date;
                        DateTime TimeStamp = Prana.ClientCommon.MarketStartEndClearanceTimes.GetInstance().GetAUECMarketEndTime(or.AUECID);
                        if (replaceOrder != null)
                            replaceOrder.ExpireTime = new DateTime(Dt.Year, Dt.Month, Dt.Day, TimeStamp.Hour, TimeStamp.Minute, TimeStamp.Second).ToString();
                        else
                            or.ExpireTime = new DateTime(Dt.Year, Dt.Month, Dt.Day, TimeStamp.Hour, TimeStamp.Minute, TimeStamp.Second).ToString();
                    }
                }
                else
                {
                    if (replaceOrder != null)
                        replaceOrder.ExpireTime = String.Empty;
                    if (or != null)
                        or.ExpireTime = String.Empty;


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
        /// Validate Trade With Algo Controls
        /// </summary>
        /// <returns></returns>
        protected override string ValidateTradeWithAlgoControls()
        {
            try
            {
                if (CachedDataManager.GetInstance.IsAlgoBrokerFromID(CounterPartyId))
                {
                    int algoId = 0;
                    int.TryParse(AlgoStrategyId, out algoId);
                    if (AlgoStrategyId != string.Empty && algoId != int.MinValue)
                    {
                        _orderRequest.AlgoProperties.TagValueDictionary = iTicketView.AlgoStrategyControlProperty.GetSelectedStrategyFixTagValues();
                        string error = iTicketView.AlgoStrategyControlProperty.ValidateAlgoStrategy(_orderRequest, null);
                        return error;
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        internal void CreateNewManualOrderOrUpdateExistingOrder(DateTime dtTradeDate)
        {
            try
            {
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && ((_tradingTicketType == TradingTicketType.Live && ComplianceCacheManager.GetPreTradeCheck(_loginUser.CompanyUserID)) || (_tradingTicketType == TradingTicketType.Manual && ComplianceCacheManager.GetPreTradeCheck(_loginUser.CompanyUserID) && ComplianceCacheManager.GetApplyToManualPermission(_loginUser.CompanyUserID))))
                {
                    iTicketView.SetMessageBoxTextAndGetDialogResult(ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE_COMPLIANCE, ClientLevelConstants.HEADER_MARKET_DATA_ALERT, MessageBoxButtons.OK);
                    return;
                }
                bool CompliancePriceCheckRequired = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("CompliancePriceCheckRequired"));
                
                if (CompliancePriceCheckRequired && _tradingTicketType == TradingTicketType.Live && iTicketView.Price == Convert.ToDecimal(0) && ComplianceCacheManager.GetPreTradeCheck(_loginUser.CompanyUserID))
                {
                    if (this.PriceForComplianceNotAvailable != null)
                    {
                        this.PriceForComplianceNotAvailable(this, new EventArgs());
                    }
                    IsTradeSuccessful = false;
                    return;
                }

                /* if (!_isPricingAvailable && ComplianceCacheManager.GetPreTradeCheck(_loginUser.CompanyUserID) && !_isUnallocatedTrade)
                {
                    DialogResult dr =
                     iTicketView.SetMessageBoxTextAndGetDialogResult
                     (TradingTicketConstants.MSG_ALLOW_TRADE, TradingTicketConstants.HEADER_PRICES_NOT_AVAILABLE, MessageBoxButtons.YesNo);
                    if (dr != DialogResult.Yes)
                        return;
                } */

                if (AuecID == int.MinValue)
                {

                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SYMBOL_NOT_VALIDATED);
                    return;
                }
                OrderSingle or = null;
                bool isValidTicket = _tradingTicketType == TradingTicketType.Stage ? GetTradingTicketForStage() : GetTradingTicket();

                if (isValidTicket)
                {
                    or = GetOrderFromTicket();

                    string error = ValidateTradeWithAlgoControls();

                    if (error != string.Empty)
                    {
                        iTicketView.SetMessageBoxText(error);
                        IsTradeSuccessful = false;
                        return;
                    }

                    if (or.CounterPartyName.ToUpper().Equals("NIRV") && or.Venue.ToUpper().Equals("STAG"))
                    {
                        return;
                    }
                }
                if (or == null)
                {
                    IsTradeSucceeded = false;
                }
                if (or != null)
                {
                    if (or.MsgType == FIXConstants.MSGOrderCancelReplaceRequest && (or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub || or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSubChild || or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub || or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManual) && _tradeManager.WorkingSubOrderDictionary.ContainsKey(or.StagedOrderID) && _tradeManager.WorkingSubOrderDictionary.ContainsKey(or.ParentClOrderID))
                    {
                        if (!CachedDataManager.GetInstance.ValidateNAVLockDate(or.AUECLocalDate))
                        {
                            System.Windows.Forms.MessageBox.Show("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (Convert.ToDouble(iTicketView.Quantity) - _tradeManager.WorkingSubOrderDictionary[or.ParentClOrderID].Quantity >= _tradeManager.WorkingSubOrderDictionary[or.StagedOrderID].UnsentQty)
                        {
                            OrderSingle replaceOrder = (OrderSingle)_tradeManager.WorkingSubOrderDictionary[or.StagedOrderID].Clone();
                            replaceOrder.Quantity += Convert.ToDouble(iTicketView.Quantity) - _tradeManager.WorkingSubOrderDictionary[or.ParentClOrderID].Quantity - _tradeManager.WorkingSubOrderDictionary[or.StagedOrderID].UnsentQty;
                            replaceOrder.ClientTime = DateTime.Now.ToLongTimeString();
                            replaceOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingReplace.ToString());
                            replaceOrder.OrigClOrderID = _tradeManager.WorkingSubOrderDictionary[or.StagedOrderID].ClOrderID;
                            replaceOrder.MsgType = FIXConstants.MSGOrderCancelReplaceRequest;
                            replaceOrder.TransactionTime = DateTime.Now.ToUniversalTime();
                            replaceOrder.CumQty = 0;
                            replaceOrder.AvgPrice = 0;
                            replaceOrder.TIF = or.TIF;
                            SetExpireTime(or, replaceOrder);

                            if (_enableTradeFlowLogging)
                            {
                                try
                                {
                                    Logger.LoggerWrite("[Trade-Flow Out1] Before SendBlotterTrades(replaceOrder) In TradingTicketPresenter, Fix Message: userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ", Symbol: " + Convert.ToString(replaceOrder.Symbol) + " , OrderID: " + Convert.ToString(replaceOrder.OrderID) + " , Quantity: " + Convert.ToString(replaceOrder.Quantity) + " , Transaction Time: " + Convert.ToString(replaceOrder.TransactionTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                }
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                                }
                            }
                            _tradeManager.SendBlotterTrades(replaceOrder, _marketPrice, true);
                            if (_enableTradeFlowLogging)
                            {
                                try
                                {
                                    Logger.LoggerWrite("[Trade-Flow Out1] After SendBlotterTrades(replaceOrder) In TradingTicketPresenter, Fix Message: userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ", Symbol: " + Convert.ToString(replaceOrder.Symbol) + " , OrderID: " + Convert.ToString(replaceOrder.OrderID) + " , Quantity: " + Convert.ToString(replaceOrder.Quantity) + " , Transaction Time: " + Convert.ToString(replaceOrder.TransactionTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                }
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                                }
                            }
                        }
                    }
                    //get IsNAVLockingEnabled or not from cache
                    Boolean isAccountNAVLockingEnabled = CachedDataManager.GetInstance.IsNAVLockingEnabled();
                    if (isAccountNAVLockingEnabled)
                    {
                        //if account selected then only check NAV locked or not for selected account - omshiv, March 2014
                        if (or.Level1ID != int.MinValue)
                        {
                            DateTime tradeDate = DateTime.Now;
                            //if manual trade then get date from date control on TT
                            if (_tradingTicketType == TradingTicketType.Manual)
                            {
                                tradeDate = dtTradeDate;
                            }
                            bool isTradeAllowed = NAVLockManager.GetInstance.ValidateTrade(or.Level1ID, tradeDate);
                            if (!isTradeAllowed)
                            {
                                System.Windows.Forms.MessageBox.Show(TradingTicketConstants.MSG_NAV_IS_LOCKED_FOR_SELECTED_ACCOUNT, TradingTicketConstants.HEADER_PRANA_WARNING, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show(TradingTicketConstants.MSG_SELECT_A_ACCOUNT, TradingTicketConstants.HEADER_WARNING, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    if (or.UnderlyingSymbol == string.Empty || or.UnderlyingSymbol == TradingTicketConstants.LIT_NOT_AVAILABLE)
                    {
                        or.UnderlyingSymbol = UnderlyingSymbol;
                    }
                    AllocationOperationPreference aop = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, or.Level1ID);
                    if (or.TransactionSource == TransactionSource.PST)
                    {
                        //or.OriginalAllocationPreferenceID = or.Level1ID;
                        if (aop != null && aop.TargetPercentage.Count == 1 && (aop.CheckListWisePreference == null || aop.CheckListWisePreference.Count == 0))
                        {
                            or.Level1ID = aop.TargetPercentage.Keys.First();
                        }
                    }

                    // the following variable is to check whether trade manager has forwarded the trade or not
                    IsTradeSucceeded = true;
                    if (priceSymbolSettings.RiskCtrlCheck || priceSymbolSettings.ValidateSymbolCheck)
                    {
                        if (!IsLiveFeedConnected())
                        {
                            if (iTicketView.SetMessageBoxTextAndGetDialogResult(GetErrorString(), String.Empty, MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                // If user clicks on 'NO' i.e., trade is not succeeded so set the boolean IsTradeSucceeded to false
                                IsTradeSucceeded = false;
                                return;
                            }
                        }
                        else
                        {
                            if (priceSymbolSettings.RiskCtrlCheck)
                            {
                                if (or.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit || or.OrderTypeTagValue == FIXConstants.ORDTYPE_LimitOnClose ||
                                or.OrderTypeTagValue == FIXConstants.ORDTYPE_LimitOrBetter || or.OrderTypeTagValue == FIXConstants.ORDTYPE_LimitWithOrWithout)
                                {
                                    if (!TradeManagerCore.GetInstance().IsWithinLimits(or, _marketPrice))
                                    {
                                        IsTradeSucceeded = false;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    or.ClientTime = DateTime.Now.ToLongTimeString();
                    or.NirvanaProcessDate = DateTime.Now;

                    //Done this work as the work done in this jira:https://jira.nirvanasolutions.com:8443/browse/PRANA-32027
                    //was causing issue in this jira:https://jira.nirvanasolutions.com:8443/browse/PRANA-36156
                    if (iTicketView.TradingTicketParent == TradingTicketParent.PM || (iTicketView.TradingTicketParent == TradingTicketParent.Blotter && IncomingOrderRequest.TransactionSource == TransactionSource.Blotter))
                    {
                        if (or.CumQty != 0)
                            or.CumQty = 0;
                    }

                    or.IsPricingAvailable = _isPricingAvailable;
                    if ((IncomingOrderRequest == null && or.MsgType != FIXConstants.MSGOrderCancelReplaceRequest) ||
                        iTicketView.TradingTicketParent == TradingTicketParent.PTT || iTicketView.TradingTicketParent == TradingTicketParent.PM || iTicketView.TradingTicketParent == TradingTicketParent.ShortLocate || (iTicketView.TradingTicketParent == TradingTicketParent.Blotter && IncomingOrderRequest.TransactionSource == TransactionSource.Blotter))
                    {
                        or.IsStageRequired = true;
                        or.IsManualOrder = true;
                        or.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                    }

                    if (or.TransactionSource == TransactionSource.None || or.TransactionSource == TransactionSource.FIX || or.TransactionSource == TransactionSource.PM || or.TransactionSource == TransactionSource.Blotter)
                    {
                        or.TransactionSource = TransactionSource.TradingTicket;
                        or.TransactionSourceTag = (int)TransactionSource.TradingTicket;
                    }
                    if (_tradingTicketType == TradingTicketType.Manual || (IsManualOrder && _tradingTicketType != TradingTicketType.Live))
                    {
                        or.IsManualOrder = true;

                    }
                    else
                        or.IsManualOrder = false;

                    or.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingNew.ToString());
                    or.AUECID = AuecID;
                    SetExpireTime(or, null);

                    if (_enableTradeFlowLogging)
                    {
                        try
                        {
                            Logger.LoggerWrite("[Trade-Flow Out1] Before SendBlotterTrades(CreateNewManualOrderOrUpdateExistingOrder) In TradingTicketPresenter, Fix Message: userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ", Symbol: " + Convert.ToString(or.Symbol) + " , OrderID: " + Convert.ToString(or.OrderID) + " , Quantity: " + Convert.ToString(or.Quantity) + " , Transaction Time: " + Convert.ToString(or.TransactionTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
                    }

                    bool isTradingRulesPassed = true;
                    if (_tradingTicketType != TradingTicketType.Stage)
                    {
                        isTradingRulesPassed = TradingRulesValidator.ValidateCompanyTradingRules(or, (double)iTicketView.Price, UseQuantityFieldAsNotional);
                    }
                                       
                    bool shouldReset = (!isTradingRulesPassed ||
                    (iTicketView.IsUseCustodianAsExecutingBroker && !_tradeManager.SendBlotterMultipleTrades(or, iTicketView.AccountBrokerMapping, aop, _tradingTicketType == TradingTicketType.Stage, true)) ||
                    (!iTicketView.IsUseCustodianAsExecutingBroker && !_tradeManager.SendBlotterTrades(or, _marketPrice, true)));

                    if (shouldReset)
                    {
                        IsTradeSucceeded = false;
                        if (_tradeManager.IsTradeMergedRemovedEdited)
                        {
                            iTicketView.ResetTicket();
                            iTicketView.TradingTicketParent = TradingTicketParent.None;
                            _tradeManager.IsTradeMergedRemovedEdited = false;
                        }
                    }
                   
                    if (_enableTradeFlowLogging)
                    {
                        try
                        {
                            Logger.LoggerWrite("[Trade-Flow Out1] After SendBlotterTrades(CreateNewManualOrderOrUpdateExistingOrder) In TradingTicketPresenter, Fix Message: userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ", Symbol: " + Convert.ToString(or.Symbol) + " , OrderID: " + Convert.ToString(or.OrderID) + " , Quantity: " + Convert.ToString(or.Quantity) + " , Transaction Time: " + Convert.ToString(or.TransactionTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
                    }
                    if ((or.MsgType == FIXConstants.MSGOrderCancelReplaceRequest)
                        || (or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub)
                        || (or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub)
                         || (or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSubChild))
                    {
                        //if trade not successful, do not close the ticket
                        if (IsTradeSucceeded)
                        {
                            if (iTicketView != null)
                            {
                                iTicketView.CloseTicket();
                            }
                        }
                    }
                    else
                    {
                        //IF the ticket is not closed remember to reset the PranaMsgType else 
                        // it would create problems on the next trade.
                        if (IsTradeSucceeded)
                        {
                            if (_isSaveCheckedInPref)
                            {
                                _orderRequest.PranaMsgType = (int)OrderFields.PranaMsgTypes.InternalOrder;
                                or.PranaMsgType = (int)OrderFields.PranaMsgTypes.InternalOrder;
                                if (or.TransactionSource == TransactionSource.PST)
                                {
                                    ResetPTTDetailsAndReloadTT(or, true);
                                }
                                if (CleanDetailsAfterTrade)
                                    iTicketView.ResetTicket();
                            }
                            else
                            {
                                iTicketView.CloseTicket();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                IsTradeSucceeded = false;
                IsPricingAvailable = false;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }       

        /// <summary>
        /// Updates the pm data on custom.
        /// </summary>
        protected override void UpdatePMDataOnCustom()
        {
            if (IsComingPM)
            {
                _accountqty.isComingFromPM = true;
                _accountqty.AccountWithPostions = AccountWithPMPostionsForCustomPref;
                _accountqty.isComingFromPMForIncrease = IsComingPMForIncrease ? true : false;
            }
        }

        /// <summary>
        /// Gets the error string.
        /// </summary>
        /// <returns></returns>
        protected string GetErrorString()
        {
            try
            {
                string errorString = TradingTicketConstants.MSG_DATA_MANAGER_NOT_CONNECTED;
                if (priceSymbolSettings != null && priceSymbolSettings.ValidateSymbolCheck)
                {
                    errorString += TradingTicketConstants.LIT_SYMBOL + TradingTicketConstants.LIT_SPACE;
                }
                if (priceSymbolSettings != null && priceSymbolSettings.RiskCtrlCheck)
                {
                    errorString += TradingTicketConstants.LIT_PRICE + TradingTicketConstants.LIT_SPACE;
                }
                errorString += TradingTicketConstants.MSG_COULD_NOT_VALIDATED_PROCEED_ANAWAYS;
                return errorString;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return String.Empty;
        }

        protected override bool GetTradingTicket()
        {
            try
            {
                if (UseQuantityFieldAsNotional)
                {
                    if (double.Parse(iTicketView.Price.ToString()) <= 0)
                    {
                        iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_VALID_PRICE_IN_DOLLARS);
                        return false;
                    }

                    if (double.Parse(iTicketView.Quantity.ToString()) <= 0)
                    {
                        iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_VALID_AMOUNT_IN_DOLLARS);
                        return false;
                    }

                    if (Math.Floor(double.Parse(iTicketView.Quantity.ToString()) / double.Parse(iTicketView.Price.ToString())) < 1)
                    {
                        iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_PRICE_GREATER_THAN_AMOUNT);
                        return false;
                    }

                    if (EnterTargetQuantityInPercentage)
                    {
                        if (Math.Floor(((Double.Parse(iTicketView.TargetQuantity.ToString()) * Double.Parse(iTicketView.Quantity.ToString())) / ApplicationConstants.PERCENTAGEVALUE) / Double.Parse(iTicketView.Price.ToString())) < 1)
                        {
                            iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_PRICE_GREATER_THAN_TARGET_AMOUNT);
                            return false;
                        }
                    }
                    else
                    {
                        if (Math.Floor(Double.Parse(iTicketView.TargetQuantity.ToString()) / Double.Parse(iTicketView.Price.ToString())) < 1)
                        {
                            iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_PRICE_GREATER_THAN_TARGET_AMOUNT);
                            return false;
                        }
                    }
                }

                if (!base.GetTradingTicket())
                    return false;

                if ((iTicketView.Quantity < iTicketView.TargetQuantity))
                {
                    SetMessageBasedOnOrderRequest();
                    return false;
                }
                if (iTicketView.SettlementCurrency == int.MinValue || String.IsNullOrEmpty(iTicketView.SettlementCurrency.ToString()))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SELECT_A_VALID_SETTLEMENT_CURRENCY);
                    return false;
                }

                if (IncomingOrderRequest == null && iTicketView.TargetQuantity <= 0 && TicketType == TradingTicketType.Live)
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_TARGET_QUANTITY_CANNOT_ZERO_FOR_LIVE);
                    return false;
                }


                if (!validateSecMasterObj())
                {
                    return false;
                }

                if (iTicketView.IsSwap)
                {
                    if (iTicketView.CtrlSwapParameter.GetSelectedParams(SwapValidate.Trade) == null)
                    {
                        return false;
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
            return true;
        }

        private bool GetTradingTicketForStage()
        {
            try
            {
                if (UseQuantityFieldAsNotional)
                {
                    if (double.Parse(iTicketView.Price.ToString()) <= 0)
                    {
                        iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_VALID_PRICE_IN_DOLLARS);
                        return false;
                    }

                    if (double.Parse(iTicketView.Quantity.ToString()) <= 0)
                    {
                        iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_VALID_AMOUNT_IN_DOLLARS);
                        return false;
                    }

                    if (Math.Floor(double.Parse(iTicketView.Quantity.ToString()) / double.Parse(iTicketView.Price.ToString())) < 1)
                    {
                        iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_PRICE_GREATER_THAN_AMOUNT);
                        return false;
                    }

                    if (EnterTargetQuantityInPercentage)
                    {
                        if (Math.Floor(((Double.Parse(iTicketView.TargetQuantity.ToString()) * Double.Parse(iTicketView.Quantity.ToString())) / ApplicationConstants.PERCENTAGEVALUE) / Double.Parse(iTicketView.Price.ToString())) < 1)
                        {
                            iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_PRICE_GREATER_THAN_TARGET_AMOUNT);
                            return false;
                        }
                    }
                    else
                    {
                        if (Math.Floor(Double.Parse(iTicketView.TargetQuantity.ToString()) / Double.Parse(iTicketView.Price.ToString())) < 1)
                        {
                            iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_PRICE_GREATER_THAN_TARGET_AMOUNT);
                            return false;
                        }
                    }
                }

                if (!ValidateQuantityAndOrderSide())
                    return false;
                if ((iTicketView.Quantity < iTicketView.TargetQuantity))
                {
                    SetMessageBasedOnOrderRequest();
                    return false;
                }
                if ((String.IsNullOrEmpty(iTicketView.TradingAccount)) || Int32.Parse(iTicketView.TradingAccount) == Int32.MinValue)
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SELECT_A_VALID_TRADING_ACCOUNT);
                    return false;
                }
                if (!validateSecMasterObj())
                {
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
        /// Validates the sec master object.
        /// </summary>
        /// <returns></returns>
        protected bool validateSecMasterObj()
        {
            try
            {
                if (TicketType == TradingTicketType.Live)
                {
                    if (iTicketView.IsUseCustodianAsExecutingBroker)
                    {
                        foreach (int brokerId in iTicketView.AccountBrokerMapping.Values)
                        {
                            string counterPartyID = brokerId.ToString();
                            string vernueID = iTicketView.VenueId;

                            string symbolcon = CachedDataManager.GetInstance.GetSymbolConvertionForCounterPartyVenues(counterPartyID, vernueID);
                            if (symbolcon != null && !ValidateSymbology(symbolcon, iTicketView.SymbolText))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        string counterPartyID = iTicketView.Brokerid;
                        string vernueID = iTicketView.VenueId;

                        string symbolcon = CachedDataManager.GetInstance.GetSymbolConvertionForCounterPartyVenues(counterPartyID, vernueID);
                        return symbolcon == null || ValidateSymbology(symbolcon, iTicketView.SymbolText);
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
            return true;
        }

        /// <summary>
        /// Validates the symbology.
        /// </summary>
        /// <param name="symbolcon">The symbolcon.</param>
        /// <param name="symbol">The symbol.</param>
        /// <returns></returns>
        internal bool ValidateSymbology(string symbolcon, string symbol)
        {
            try
            {
                switch (symbolcon)
                {
                    case TradingTicketConstants.LIT_SEDOL:
                        //condition added to skip SEDOL verification for FX, EquityOption and Future, PRANA-10557
                        //condition added to skip SEDOL verification for FutureOption, PRANA-11135
                        if ((_secmasterObj.AssetCategory != AssetCategory.FX && _secmasterObj.AssetCategory != AssetCategory.FXForward && _secmasterObj.AssetCategory != AssetCategory.EquityOption && _secmasterObj.AssetCategory != AssetCategory.FutureOption) && string.IsNullOrEmpty(_secmasterObj.SedolSymbol))
                        {
                            DialogResult dialogResult = iTicketView.SetMessageBoxTextAndGetDialogResult(TradingTicketConstants.MSG_MISSING_SEDOL_IN_SECURITY_MASTER_UPDATE, TradingTicketConstants.HEADER_MESSAGE, MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                SendSymbologyMissingMessage(symbolcon, symbol);
                            }
                            return false;
                        }
                        break;

                    case TradingTicketConstants.LIT_CUSIP:
                    // condition added to skip Cusip verification for FX and Future, PRANA-10557
                    case TradingTicketConstants.LIT_CINS:
                        if ((_secmasterObj.AssetCategory != AssetCategory.EquityOption && _secmasterObj.AssetCategory != AssetCategory.FX && _secmasterObj.AssetCategory != AssetCategory.FXForward && _secmasterObj.AssetCategory != AssetCategory.FutureOption && _secmasterObj.AssetCategory != AssetCategory.Future && _secmasterObj.AssetCategory != AssetCategory.FXOption) && (_secmasterObj.CusipSymbol == null || _secmasterObj.CusipSymbol == string.Empty))
                        {
                            DialogResult dialogResult = iTicketView.SetMessageBoxTextAndGetDialogResult(TradingTicketConstants.MSG_MISSING_CUSIP_IN_SECURITY_MASTER_UPDATE, TradingTicketConstants.HEADER_MESSAGE, MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                SendSymbologyMissingMessage(symbolcon, symbol);
                            }
                            return false;
                        }
                        break;

                    case TradingTicketConstants.LIT_RIC:

                        if (string.IsNullOrEmpty(_secmasterObj.ReutersSymbol))
                        {
                            DialogResult dialogResult = iTicketView.SetMessageBoxTextAndGetDialogResult(TradingTicketConstants.MSG_MISSING_RIC_IN_SECURITY_MASTER_UPDATE, TradingTicketConstants.HEADER_MESSAGE, MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                SendSymbologyMissingMessage(symbolcon, symbol);
                            }
                            return false;
                        }
                        break;

                    case TradingTicketConstants.LIT_ISIN:
                        //condition added to skip ISIN verification for FX, EquityOption and Future, PRANA-10557 ,FutureOption, PRANA-11135
                        if ((_secmasterObj.AssetCategory != AssetCategory.FX && _secmasterObj.AssetCategory != AssetCategory.FXForward && _secmasterObj.AssetCategory != AssetCategory.EquityOption && _secmasterObj.AssetCategory != AssetCategory.FutureOption) && string.IsNullOrEmpty(_secmasterObj.ISINSymbol))
                        {
                            DialogResult dialogResult = iTicketView.SetMessageBoxTextAndGetDialogResult(TradingTicketConstants.MSG_MISSING_ISIN_IN_SECURITY_MASTER_UPDATE, TradingTicketConstants.HEADER_MESSAGE, MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                SendSymbologyMissingMessage(symbolcon, symbol);
                            }
                            return false;
                        }
                        break;

                    case TradingTicketConstants.LIT_OSI:
                        //condition removed to skip OSI verification for FutureOption, PRANA-11135
                        if ((_secmasterObj.AssetCategory == AssetCategory.EquityOption || _secmasterObj.AssetCategory == AssetCategory.Option || _secmasterObj.AssetCategory == AssetCategory.FXOption) && string.IsNullOrEmpty(_secmasterObj.OSIOptionSymbol))
                        {
                            DialogResult dialogResult = iTicketView.SetMessageBoxTextAndGetDialogResult(TradingTicketConstants.MSG_MISSING_OSI_IN_SECURITY_MASTER_UPDATE, TradingTicketConstants.HEADER_MESSAGE, MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                SendSymbologyMissingMessage(symbolcon, symbol);
                            }
                            return false;
                        }
                        break;
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

        private void SetMessageBasedOnOrderRequest()
        {
            try
            {
                if (_incomingOrderRequest != null)
                {
                    iTicketView.SetMessageBoxText((OrderFields.PranaMsgTypes)_incomingOrderRequest.PranaMsgType == OrderFields.PranaMsgTypes.ORDStaged
                        ? TradingTicketConstants.MSG_GREATHER_INMARKET_QUANTITY : IsShowTargetQTY ? TradingTicketConstants.MSG_GREATHER_TARGET_QUANTITY : TradingTicketConstants.MSG_GREATHER_TARGET_QUANTITY_NEW);
                }
                else
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_GREATHER_EXQTY_QUANTITY);
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

        internal void UpdateTradingAccountList(int fundId)
        {
            try
            {
                if (_auecID > 0)
                {
                    FillTradingAccountCombo();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Sets the ticket preferences.
        /// </summary>
        protected void SetTicketPreferences()
        {
            try
            {
                priceSymbolSettings = TradingTktPrefs.PriceSymbolValidationData;
                _isTargetQuantitySameAsTotalQty = !priceSymbolSettings.SetExecutedQtytoZero;
                _isSaveCheckedInPref = TradingTktPrefs.TTGeneralPrefs.IsSaveChecked;
                CleanDetailsAfterTrade = TradingTktPrefs.TTGeneralPrefs.CleanDetailsAfterTrade;
                _isShowAlgoControls = true;
                iTicketView.SetTicketPreferences(_userTradingTicketUiPrefs, _companyTradingTicketUiPrefs);

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


        internal override void FillComboBoxes()
        {
            try
            {
                base.FillComboBoxes();

                if (IncomingOrderRequest != null)
                {
                    if (IncomingOrderRequest.PMType == PMType.Increase)
                    {
                        IsComingPM = true;
                        IsComingPMForIncrease = true;
                    }
                    if ((IncomingOrderRequest.PMType == PMType.Close))
                        IsComingPM = true;
                }
                FillStartegyCombo();
                FillTradingAccountCombo();
                FillSettlementCurrency();
                BindExecInstr();
                BindHandlingInstr();
                BindCommissionBasisCombo();
                FillFxOperatorCombo();
                SetTicketPreferences();
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
        /// Binds the execute instr.
        /// </summary>
        protected void BindExecInstr()
        {
            try
            {
                DataTable executionInstrucitons = CachedDataManager.GetInstance.GetExecutionInstruction().Copy();
                iTicketView.FillExecutionInstructions(executionInstrucitons);

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
        /// Binds the handling instr.
        /// </summary>
        protected void BindHandlingInstr()
        {
            try
            {
                DataTable handlingInstrucitons = CachedDataManager.GetInstance.GetHandlingInstruction().Copy();
                handlingInstrucitons.Columns[0].ColumnName = OrderFields.PROPERTY_HANDLING_INSTID;
                handlingInstrucitons.Columns[1].ColumnName = OrderFields.PROPERTY_HANDLING_INST;
                iTicketView.FillHandlingInstruction(handlingInstrucitons);
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

        protected void FillFxOperatorCombo()
        {
            try
            {

                ValueList FxOperator = new ValueList();
                List<EnumerationValue> FxOperatorEnumList = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Operator));
                foreach (EnumerationValue var in FxOperatorEnumList.Where(var => !var.Value.Equals((int)Operator.Multiple)))
                {
                    FxOperator.ValueListItems.Add(var.DisplayText, var.DisplayText);
                }
                switch (CachedDataManager.GetInstance.GetCurrencyText(this.CurrencyId))
                {
                    case "EUR":
                    case "GBP":
                    case "NZD":
                    case "AUD":
                        FxOperator.SortStyle = ValueListSortStyle.Descending;
                        break;

                    default:
                        FxOperator.SortStyle = ValueListSortStyle.Ascending;
                        break;
                }
                if (this.CurrencyId == this.CompanyBaseCurrencyID)
                {
                    FxOperator.SortStyle = ValueListSortStyle.Descending;
                }
                iTicketView.FillFxOperatorCombo(FxOperator);

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
        /// Fills the settlement currency.
        /// </summary>
        protected void FillSettlementCurrency()
        {
            try
            {
                Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                ValueList currencies = new ValueList();
                foreach (KeyValuePair<int, string> item in dictCurrencies)
                {
                    if (item.Key == this.CompanyBaseCurrencyID || item.Key == this.CurrencyId)
                    {
                        currencies.ValueListItems.Add(item.Key, item.Value);
                    }
                }
                iTicketView.FillSettlementCurrency(currencies);
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
        /// Fills the startegy combo.
        /// </summary>
        protected void FillStartegyCombo()
        {
            try
            {
                StrategyCollection strategies = CachedDataManager.GetInstance.GetUserStrategies();
                iTicketView.FillStartegyCombo(strategies);
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
        /// Fills the trading account combo.
        /// </summary>
        protected void FillTradingAccountCombo()
        {
            try
            {
                TradingAccountCollection tradingAccounts = CachedDataManager.GetInstance.GetUserTradingAccounts();
                DataTable dt = new DataTable();
                dt.Columns.Add(TradingTicketConstants.LIT_VALUE);
                dt.Columns.Add(TradingTicketConstants.LIT_DISPLAY);
                int TradingAccountNumber = CachedDataManager.GetTradingAccountForMasterFund(FundId);

                if (TradingAccountNumber < 0)
                {
                    foreach (TradingAccount tradingAccount in tradingAccounts)
                    {
                        dt.Rows.Add(tradingAccount.TradingAccountID, tradingAccount.Name);
                    }
                }
                else
                {
                    foreach (TradingAccount tradingAccount in tradingAccounts)
                    {
                        if (tradingAccount.TradingAccountID == TradingAccountNumber)
                            dt.Rows.Add(tradingAccount.TradingAccountID, tradingAccount.Name);
                    }
                }

                DataView dv = dt.DefaultView;
                dv.Sort = TradingTicketConstants.LIT_DISPLAY;
                DataTable newDt = dv.ToTable();
                iTicketView.FillTradingAccountCombo(newDt);
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
        /// Update Accounts List 
        /// </summary>
        /// <param name="fundId"></param>
        internal void UpdateAccountsListFilter(int fundId)
        {
            try
            {
                if (_auecID > 0)
                {
                    ValueList cpList = new ValueList();

                    //Getting funds for with allocation schemes
                    DataTable accountsData = FillAccountCombo();
                    cpList = GetDataForAllocationAndBrokerCombo(fundId, cpList, accountsData);
                    if (accountsData != null)
                        iTicketView.FillAccountCombo(accountsData);

                    FillBrokeCombo(cpList);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates the security details.
        /// </summary>
        /// <param name="secMasterObj">The sec master object.</param>
        protected override void UpdateSecurityDetails(SecMasterBaseObj secMasterObj)
        {
            try
            {
                bool isBloombergMDP = false;
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI)
                {
                    if (iTicketView.SymbolText.Trim().ToUpper() == secMasterObj.BloombergSymbolWithExchangeCode.ToUpper())
                        isBloombergMDP = true;
                }
                if (isBloombergMDP || iTicketView.SymbolText.Trim().ToUpper() == secMasterObj.SymbologyMapping[(int)DefaultSymbology].ToUpper())
                {
                    _assetID = secMasterObj.AssetID;
                    if (!IsTradeSending)
                    {
                        SetSecMasterObjForTicket(secMasterObj);
                        Symbol = secMasterObj.TickerSymbol;
                        ValidateSymbolSetByUser();
                        _accountqty = null;
                        //Added check for Imported stagedorders
                        //https://jira.nirvanasolutions.com:8443/browse/PRANA-25709 
                        if (!secMasterObj.RequestedSymbol.Equals(pttSymbol) && _incomingOrderRequest != null && _incomingOrderRequest.TransactionSourceTag != 3 && _incomingOrderRequest.TransactionSourceTag != 14 && _incomingOrderRequest.TransactionSourceTag != 18 && _incomingOrderRequest.TransactionSourceTag != 19)
                        {
                            pttSymbol = String.Empty;
                            _incomingOrderRequest.OriginalAllocationPreferenceID = 0;
                            _incomingOrderRequest.TransactionSource = TransactionSource.None;
                            _incomingOrderRequest.TransactionSourceTag = (int)TransactionSource.None;
                        }
                        if (_incomingOrderRequest != null && _incomingOrderRequest.SwapParameters != null)
                            iTicketView.SetIsSwap(true);
                        iTicketView.DrawControl(secMasterObj);
                        iTicketView.SetAUECDateInTicket(Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(secMasterObj.AUECID)));
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

        internal override void InitControl(CompanyUser loginUser)
        {
            try
            {
                base.InitControl(loginUser);
                CreatePricingServiceProxy();
                _companyBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                BindingTAs();
                SetTradeAttrabute();
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
        /// Sets the trade attrabute.
        /// </summary>
        protected void SetTradeAttrabute()
        {

            try
            {
                string lblTA1 = CachedDataManager.GetInstance.GetAttributeNameForValue(TradingTicketConstants.CAPTION_TradeAttribute1);
                string lblTA2 = CachedDataManager.GetInstance.GetAttributeNameForValue(TradingTicketConstants.CAPTION_TradeAttribute2);
                string lblTA3 = CachedDataManager.GetInstance.GetAttributeNameForValue(TradingTicketConstants.CAPTION_TradeAttribute3);
                string lblTA4 = CachedDataManager.GetInstance.GetAttributeNameForValue(TradingTicketConstants.CAPTION_TradeAttribute4);
                string lblTA5 = CachedDataManager.GetInstance.GetAttributeNameForValue(TradingTicketConstants.CAPTION_TradeAttribute5);
                string lblTA6 = CachedDataManager.GetInstance.GetAttributeNameForValue(TradingTicketConstants.CAPTION_TradeAttribute6);
                iTicketView.SetTradeAttributeLabels(lblTA1, lblTA2, lblTA3, lblTA4, lblTA5, lblTA6);
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

        protected void BindingTAs()
        {
            try
            {
                List<string>[] attribLists = _allocationProxy.InnerChannel.GetTradeAttributes();
                TradeAttributesCache.updateCache(attribLists, true);
                vls = TradeAttributesCache.getValueList((Form)iTicketView);
                iTicketView.SetTradeAttributeValueList(vls);
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
        internal override void SetSecMasterObjForTicket(SecMasterBaseObj secmasterObj)
        {
            try
            {
                base.SetSecMasterObjForTicket(secmasterObj);
                if (_secmasterObj.AssetID == (int)AssetCategory.FX || _secmasterObj.AssetID == (int)AssetCategory.FXForward)
                {
                    Dictionary<int, string> currencyList = new Dictionary<int, string>(2);
                    if (LeadCurrencyId != int.MinValue)
                        currencyList.Add(LeadCurrencyId, CachedDataManager.GetInstance.GetCurrencyText(LeadCurrencyId));

                    if (VsCurrencyId != int.MinValue)
                        currencyList.Add(VsCurrencyId, CachedDataManager.GetInstance.GetCurrencyText(VsCurrencyId));
                    iTicketView.SetDealInComboValue(currencyList);
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

        internal void SetIncomingOrderRequest(OrderSingle order)
        {
            try
            {
                _incomingOrderRequest = order;
                if (_incomingOrderRequest != null)
                {
                    string symbol = iTicketView.GetPreferredSymbolFromTicker(_incomingOrderRequest.Symbol);
                    if (_incomingOrderRequest.TransactionSource == TransactionSource.PST)
                        pttSymbol = symbol;
                    iTicketView.SetPranaSymbolControlText(symbol);
                    iTicketView.SetPranaFundDefaultValue(_incomingOrderRequest);
                    if (_incomingOrderRequest.MsgType == FIXConstants.MSGOrderCancelReplaceRequest || _incomingOrderRequest.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX ||
                      _incomingOrderRequest.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew || _incomingOrderRequest.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrder)
                    {
                        iTicketView.EnablePranaSymbolControl(false);
                    }
                    else if (_incomingOrderRequest.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub || _incomingOrderRequest.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSubChild || _incomingOrderRequest.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub)
                    {
                        iTicketView.EnablePranaSymbolControl(false);
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
        /// Updates the order side and update PTT details.
        /// </summary>
        internal void UpdateOrderSideAndUpdatePTTDetails()
        {
            try
            {

                if (_orderSinglePTT != null)
                {
                    bool isSideAvailable = false;
                    foreach (DataRow dRow in CachedDataManager.GetInstance.GetOrderSides(_assetID).Copy().Rows)
                    {

                        if (TagDatabaseManager.GetInstance.GetOrderSideTagValueBasedOnId(dRow[OrderFields.PROPERTY_ORDER_SIDEID].ToString())
                            .Equals(_orderSinglePTT.OrderSideTagValue))
                        {
                            isSideAvailable = true;
                        }
                    }
                    if (isSideAvailable)
                    {
                        iTicketView.SetComboSideValue(_orderSinglePTT);
                    }
                    else
                    {
                        ResetPTTDetailsAndReloadTT(_orderRequest, false);
                    }
                    iTicketView.WireSideChangeEvent(true);
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
        /// Resets the PTT details and reload tt.
        /// </summary>
        /// <param name="or">The or.</param>
        /// <param name="isTradeOrStaged">if set to <c>true</c> [is trade or staged].</param>
        internal void ResetPTTDetailsAndReloadTT(OrderSingle or, bool isTradeOrStaged)
        {
            try
            {
                DialogResult userResponse;
                if (isTradeOrStaged)
                {
                    userResponse = DialogResult.Yes;
                }
                else
                {
                    userResponse = iTicketView.SetMessageBoxTextAndGetDialogResult(TradingTicketConstants.MSG_PTT_ORDER_REMOVE, TradingTicketConstants.HEADER_TRADING_TICKET, MessageBoxButtons.YesNo);
                }
                if (userResponse == DialogResult.Yes)
                {
                    or.OriginalAllocationPreferenceID = 0;
                    or.TransactionSource = TransactionSource.None;
                    or.TransactionSourceTag = (int)TransactionSource.None;
                    _orderRequest.OriginalAllocationPreferenceID = 0;
                    _orderRequest.TransactionSource = TransactionSource.None;
                    _orderRequest.TransactionSourceTag = (int)TransactionSource.None;
                    iTicketView.RemovePTTFromAccountCombo();
                    _orderSinglePTT = null;
                }
                else
                {
                    RetrieveOrderFromPTT();
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
        /// Retrieves the order from PTT.
        /// </summary>
        protected void RetrieveOrderFromPTT()
        {
            try
            {
                iTicketView.WireSideChangeEvent(false);
                iTicketView.SetComboSideValue(_orderSinglePTT);
                iTicketView.WireSideChangeEvent(true);
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
        /// Opens the view allocation details form.
        /// </summary>
        internal void OpenViewAllocationDetailsForm()
        {
            try
            {
                if (viewAllocationDetailsWindow == null)
                {
                    viewAllocationDetailsWindow = new ViewAllocationDetailsWindow();
                }
                ViewAllocationDetailsViewModel viewAllocationDetailsViewModel = new ViewAllocationDetailsViewModel();
                viewAllocationDetailsViewModel.AllocationPrefID = _orderRequest.OriginalAllocationPreferenceID;
                viewAllocationDetailsViewModel.Symbol = _orderRequest.Symbol;
                viewAllocationDetailsViewModel.OrderSideId = _orderRequest.OrderSideTagValue;
                viewAllocationDetailsWindow.ViewAllocationDetailsViewModel = viewAllocationDetailsViewModel;
                viewAllocationDetailsViewModel.AllocationDetailsUILoaded.Execute(null);
                viewAllocationDetailsWindow.Closed += viewAllocationDetailsWindow_Closed;
                BringFormToFront(viewAllocationDetailsWindow);
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
        /// Brings the form to front.
        /// </summary>
        /// <param name="window">The window.</param>
        protected void BringFormToFront(Window window)
        {
            if (!window.IsVisible)
            {
                window.ShowInTaskbar = false;
                ElementHost.EnableModelessKeyboardInterop(window);
                new WindowInteropHelper(window) { Owner = ((Form)iTicketView).Handle };
                window.Show();
                window.Activate();
            }
        }

        protected override bool ValidateQuantityAndOrderSide()
        {
            try
            {
                if (_orderRequest.MsgType != FIXConstants.MSGOrderCancelReplaceRequest && _orderRequest.MsgType != FIXConstants.MSGExecutionReport &&
                       _orderRequest.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX && _orderRequest.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrder)
                {
                    _orderRequest.MsgType = FIXConstants.MSGOrder;
                }

                if (iTicketView.Quantity == 0.0m)
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_VALID_QUANTITY);
                    return false;
                }
                Regex rg1 = new Regex(@"^\d*\.{0,1}\d+$");

                if (!(rg1.IsMatch(iTicketView.Quantity.ToString())))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_NUMERIC_QUANTITY);
                    return false;
                }
                if (Max_Qty > 0 && !(iTicketView.Quantity <= Max_Qty))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_GREATER_QUANTITY);
                    return false;
                }
                if (!(iTicketView.Quantity >= Min_Qty))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_LESSER_QUANTITY);
                    return false;
                }
                // added check for null values for OrderSide, OrderType, CounterParty, Account, Trading Account and Settlement Currency, PRANA-10048
                if (String.IsNullOrEmpty(iTicketView.OrderSide))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SELECT_A_VALID_ORDER_SIDE);
                    return false;
                }
                //string tif = String.IsNullOrEmpty(iTicketView.TIF) ? String.Empty : TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(iTicketView.TIF);

                //if (!String.IsNullOrEmpty(tif) && tif == FIXConstants.TIF_GTD)
                //{
                //    if (iTicketView.ExpireTime == null || string.IsNullOrEmpty(iTicketView.ExpireTime) || iTicketView.ExpireTime.Equals("N/A"))
                //    {
                //        iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_VALID_GTDDATE);
                //        return false;
                //    }
                //}

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
        /// Gets the accounts for expnl request.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        internal override List<int> GetAccountsForExpnlRequest()
        {
            return CachedDataManager.GetInstance.GetAllUserAccountList();
        }

        /// <summary>
        /// Checks the order in dictionary.
        /// </summary>
        /// <param name="stagedOrderID">The staged order identifier.</param>
        /// <returns></returns>
        internal bool CheckOrderInDictionary(string stagedOrderID)
        {
            try
            {
                return _tradeManager.WorkingSubOrderDictionary.ContainsKey(stagedOrderID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Handles the Closed event of the viewAllocationDetailsWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void viewAllocationDetailsWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                viewAllocationDetailsWindow = null;
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

        //// <summary>
        /// Determines whether the given allocation preference id is a simple calculated preference or not.
        /// </summary>
        /// <param name="prefId">The ID of the preference to check.</param>
        public AllocationOperationPreference GetSimpleCalculatedPreference(int prefId)
        {
            try
            {
                AllocationOperationPreference allocationOperationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, prefId);
                if (allocationOperationPreference != null && allocationOperationPreference.DefaultRule.RuleType == MatchingRuleType.None 
                    && allocationOperationPreference.DefaultRule.MatchClosingTransaction == MatchClosingTransactionType.None && allocationOperationPreference.CheckListWisePreference.Count == 0)
                {
                    return allocationOperationPreference;
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
            return null;
        }

        protected override void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    base.Dispose(isDisposing);
                    if (_pricingServicesProxy != null)
                    {
                        _pricingServicesProxy.Dispose();
                    }
                    if (vls != null)
                    {
                        foreach (BindableValueList v in vls)
                        {
                            if (v != null)
                            {
                                v.DataSource = null;
                                v.Dispose();
                            }
                        }
                    }
                    if (_accountqty != null)
                    {
                        _accountqty.Dispose();
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

        #region ILiveFeedCallback members

        /// <summary>
        /// Snapshots the response.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            try
            {
                if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: TradingTicketPresenter.SnapshotResponse() > Entered for Symbol: {0}, Time: {1}", data.Symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                }

                _isPricingAvailable = true;
                if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && data != null && data.LastPrice > 0)
                {
                    _marketPrice = data.LastPrice;
                }
                if (data != null && data.LastPrice == 0 && data.Ask == 0 && data.Bid == 0)
                    _isPricingAvailable = false;
                iTicketView.Level1SnapshotResponse(data);

                if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: TradingTicketPresenter.SnapshotResponse() > Exited for Symbol: {0}, Time: {1}", data.Symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
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
        /// Lives the feed connected.
        /// </summary>
        public void LiveFeedConnected()
        {
        }

        /// <summary>
        /// Lives the feed dis connected.
        /// </summary>
        public void LiveFeedDisConnected()
        {
        }

        /// <summary>
        /// Options the chain response.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="data">The data.</param>
        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        #endregion
    }
}

