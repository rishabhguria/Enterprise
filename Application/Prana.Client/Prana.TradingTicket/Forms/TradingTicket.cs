using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinSchedule;
using Prana.Admin.BLL;
using Prana.AlgoStrategyControls;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.LiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ShortLocate.Classes;
using Prana.SM.OTC;
using Prana.SM.OTC.View;
using Prana.TradeManager.Extension;
using Prana.TradingTicket.TTPresenter;
using Prana.TradingTicket.TTView;
using Prana.Utilities.UI.ExtensionUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using DayOfWeek = System.DayOfWeek;

namespace Prana.TradingTicket.Forms
{
    /// <summary>
    /// Trading Ticket
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    /// <seealso cref="Prana.Interfaces.ITradingTicket" />
    /// <seealso cref="Prana.TradingTicket.TTView.ITradingTicketView" />
    public partial class TradingTicket : Form, ITradingTicket, ITradingTicketView
    {

        /// <summary>
        /// The _login user
        /// </summary>
        private CompanyUser _loginUser;
        /// <summary>
        /// The max_ qty
        /// </summary>
        private decimal Max_Qty = 999999999;

        /// <summary>
        /// The tt presenter
        /// </summary>
        private TradingTicketPresenter ttPresenter = new TradingTicketPresenter();
        private ShortLocate.Controls.ShortLocateList borrowParameter = new ShortLocate.Controls.ShortLocateList();

        /// <summary>
        /// The watch list column
        /// </summary>
        private string _watchListColumn = string.Empty;

        System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
        private bool _enableTradeFlowLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableTradeFlowLogging"));

        /// <summary>
        /// Sets true if tradingTicket is closed after executing a trade.
        /// </summary>
        private bool _tradingTicketClosedAfterExecutingTrade = false;

        /// <summary>
        /// Sets true if custodian is selected as executing broker.
        /// </summary>
        private bool _isUseCustodianAsExecutingBroker = false;

        private Dictionary<int, int> _accountBrokerMapping = new Dictionary<int, int>();

        /// <summary>
        /// Store stage order allocation view for rebalancer and import trades
        /// </summary>
        private StagedOrderAllocationView _stagedOrderAllocationView = null;

        /// <summary>
        /// The dictionary for account broker mapping for selected fund
        /// </summary>
        public Dictionary<int, int> AccountBrokerMapping
        {
            get
            {
                return _accountBrokerMapping;
            }
        }

        /// <summary>
        /// Is Update Broker Based On Custodian Preference
        /// </summary>
        private bool _isUpdateBrokerBasedOnCustodianPreference = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TradingTicket" /> class.
        /// </summary>
        public TradingTicket()
        {
            try
            {
                InitializeComponent();
                if (!(DesignMode))
                {
                    ttPresenter.Add(this);
                }

                SetFundView();
                SetTradingTicketViewForOTC();

                if (!ModuleManager.CheckModulePermissioning(PranaModules.SECURITY_MASTER_MODULE, PranaModules.SECURITY_MASTER_MODULE))
                    btnSymbolLookup.Enabled = false;

                CreateSMSyncPoxy();
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
        ///
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public delegate void SetSymbolL1StripDeglate(string symbol);

        /// <summary>
        ///
        /// </summary>
        /// <param name="sides">The sides.</param>
        private delegate void FillOrderSideDelegate(DataTable sides);

        /// <summary>
        ///
        /// </summary>
        /// <param name="l1data">The l1data.</param>
        private delegate void Level1SnapshotResponseSameThread(SymbolData l1data);

        /// <summary>
        /// option CheckBoxe checked
        /// </summary>
        private delegate void SetCheckBoxDelegate();

        /// <summary>
        /// update live feed caption.
        /// </summary>
        private delegate void UpdateCaptionDelegate();

        /// <summary>
        /// Set Swap Check Box
        /// </summary>
        /// <param name="isSwap">if set to <c>true</c> [is swap].</param>
        private delegate void SetSwapCheckBox(bool isSwap);

        /// <summary>
        /// Occurs when [form closed handler].
        /// </summary>
        public event EventHandler FormClosedHandler;

        /// <summary>
        /// Occurs when [launch symbol lookup].
        /// </summary>
        public event EventHandler LaunchSymbolLookup;

        ProxyBase<ISecMasterSyncServices> _secMasterSyncService = null;

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public string Account
        {
            get
            {
                return (string)cmbAllocation.Value;
            }
            set
            {
                cmbAllocation.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public string AccountText
        {
            get
            {
                return (string)cmbAllocation.Text;
            }
        }

        public Dictionary<string, string> TagValueDictionary
        {
            get { return null; }
        }
        /// <summary>
        /// Gets the algo strategy control property.
        /// </summary>
        /// <value>
        /// The algo strategy control property.
        /// </value>
        public AlgoStrategyControl AlgoStrategyControlProperty
        {
            get
            {
                return algoStrategyControl;
            }
        }

        /// <summary>
        /// Gets the broker.
        /// </summary>
        /// <value>
        /// The broker.
        /// </value>
        public string Broker
        {
            get
            {
                return (string)cmbBroker.Text;
            }
        }

        private TradingTicketParent _tradingTicketParent;
        public TradingTicketParent TradingTicketParent
        {
            get
            {
                return _tradingTicketParent;
            }
            set
            {
                _tradingTicketParent = value;
            }
        }

        /// <summary>
        /// Gets or sets the brokerid.
        /// </summary>
        /// <value>
        /// The brokerid.
        /// </value>
        public string Brokerid
        {
            get
            {
                return (string)cmbBroker.Value;
            }
        }

        /// <summary>
        /// Gets or sets the broker notes.
        /// </summary>
        /// <value>
        /// The broker notes.
        /// </value>
        public string BrokerNotes
        {
            get
            {
                return txtBrokerNotes.Text;
            }
        }

        /// <summary>
        /// Gets or sets the commission basis.
        /// </summary>
        /// <value>
        /// The commission basis.
        /// </value>
        public CalculationBasis CommissionBasis
        {
            get
            {
                return cmbCommissionBasis.Value == null ? CalculationBasis.Auto : (CalculationBasis)Enum.Parse(typeof(CalculationBasis), cmbCommissionBasis.Value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the commission rate.
        /// </summary>
        /// <value>
        /// The commission rate.
        /// </value>
        public decimal CommissionRate
        {
            get
            {
                return nmrcCommissionRate.Value;
            }
        }

        /// <summary>
        /// Gets the deal in.
        /// </summary>
        /// <value>
        /// The deal in.
        /// </value>
        public string DealIn
        {
            get
            {
                return cmbDealIn.Value == null ? int.MinValue.ToString() : cmbDealIn.Value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the Fx Operator.
        /// </summary>
        /// <value>
        /// The Fx Operator.
        /// </value>
        public string FxOperator
        {
            get
            {
                return (string)cmbFxOperator.Value;
            }
        }


        /// <summary>
        /// Gets or sets the execution instructions.
        /// </summary>
        /// <value>
        /// The execution instructions.
        /// </value>
        public string ExecutionInstructions
        {
            get
            {
                return (string)cmbExecutionInstructions.Value;
            }
        }

        /// <summary>
        /// Gets or sets the fx rate.
        /// </summary>
        /// <value>
        /// The fx rate.
        /// </value>
        public decimal FxRate
        {
            get
            {
                return nmrcFXRate.Value;
            }
        }



        private bool _isTTSourceDependentOnAnotherUIs = false;

        /// <summary>
        /// Gets or sets the fx rate.
        /// </summary>
        /// <value>
        /// The fx rate.
        /// </value>
        public bool IsTTSourceDependentOnAnotherUIs
        {
            get
            {
                return _isTTSourceDependentOnAnotherUIs;
            }
            set
            {
                _isTTSourceDependentOnAnotherUIs = value;
            }
        }/// <summary>
         /// Gets or sets the handling instruction.
         /// </summary>
         /// <value>
         /// The handling instruction.
         /// </value>
        public string HandlingInstruction
        {
            get
            {
                return (string)cmbHandlingInstructions.Value;
            }
        }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        /// <value>
        /// The limit.
        /// </value>
        public decimal Limit
        {
            get
            {
                return nmrcLimit.Value;
            }
        }

        /// <summary>
        /// Gets or sets the login user.
        /// </summary>
        /// <value>
        /// The login user.
        /// </value>
        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
                ttPresenter.LoginUser = value;
            }
        }

        public bool isShowTargetQTY { get; set; }
        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>


        public bool IsShowTargetQTY
        {
            get { return isShowTargetQTY; }
            set { isShowTargetQTY = value; }
        }

        public string Notes
        {
            get
            {
                return txtNotes.Text;
            }
        }

        /// <summary>
        /// Gets or sets the order side.
        /// </summary>
        /// <value>
        /// The order side.
        /// </value>
        public string OrderSide
        {
            get
            {
                return (string)cmbOrderSide.Value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the order.
        /// </summary>
        /// <value>
        /// The type of the order.
        /// </value>
        public string OrderType
        {
            get
            {
                return (string)cmbOrderType.Value;
            }
        }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price
        {
            get
            {
                return nmrcPrice.Value;
            }
        }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public decimal Quantity
        {
            get
            {
                return nmrcQuantity.Value;
            }
        }

        /// <summary>
        /// Sets the security master.
        /// </summary>
        /// <value>
        /// The security master.
        /// </value>
        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                ttPresenter.SecurityMaster = value;
                pranaOptionCtrl.SecurityMaster = value;
            }
        }

        /// <summary>
        /// Gets or sets the settlement currency.
        /// </summary>
        /// <value>
        /// The settlement currency.
        /// </value>
        public int SettlementCurrency
        {
            get
            {
                return cmbSettlementCurrency.Value == null ? int.MinValue : Int32.Parse(cmbSettlementCurrency.Value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the soft commission basis.
        /// </summary>
        /// <value>
        /// The soft commission basis.
        /// </value>
        public CalculationBasis SoftCommissionBasis
        {
            get
            {
                return cmbSoftCommissionBasis.Value == null ? CalculationBasis.Auto : (CalculationBasis)Enum.Parse(typeof(CalculationBasis), cmbSoftCommissionBasis.Value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the soft commission rate.
        /// </summary>
        /// <value>
        /// The soft commission rate.
        /// </value>
        public decimal SoftCommissionRate
        {
            get
            {
                return nmrcSoftRate.Value;
            }
        }

        /// <summary>
        /// Gets or sets the stop.
        /// </summary>
        /// <value>
        /// The stop.
        /// </value>
        public decimal Stop
        {
            get
            {
                return nmrcStop.Value;
            }
        }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>
        /// The strategy.
        /// </value>
        public int? Strategy
        {
            get
            {
                return cmbStrategy.Value == null ? int.MinValue : Int32.Parse(cmbStrategy.Value.ToString());
            }
        }

        /// <summary>
        /// Gets the symbol text.
        /// </summary>
        /// <value>
        /// The symbol text.
        /// </value>
        public string SymbolText
        {
            get
            {
                return pranaSymbolCtrl.Text;
            }
        }

        /// <summary>
        /// Gets or sets the target quantity.
        /// </summary>
        /// <value>
        /// The target quantity.
        /// </value>
        public decimal TargetQuantity
        {
            get
            {
                return nmrcTargetQuantity.Value;
            }
        }

        /// <summary>
        /// Gets or sets the tif.
        /// </summary>
        /// <value>
        /// The tif.
        /// </value>
        public string TIF
        {
            get
            {
                return (string)cmbTIF.Value;
            }
        }


        public string ExpireTime
        {
            get
            {
                return (dtExpireTime.Value == null || string.IsNullOrEmpty(dtExpireTime.Value.ToString())) ? string.Empty : dtExpireTime.Value.ToString();
            }
        }
        /// <summary>
        /// Gets or sets the trade attribute1.
        /// </summary>
        /// <value>
        /// The trade attribute1.
        /// </value>
        public string TradeAttribute1
        {
            get
            {
                return cmbTradeAttribute1.Text;
            }
        }

        /// <summary>
        /// Gets or sets the trade attribute2.
        /// </summary>
        /// <value>
        /// The trade attribute2.
        /// </value>
        public string TradeAttribute2
        {
            get
            {
                return cmbTradeAttribute2.Text;
            }
        }

        /// <summary>
        /// Gets or sets the trade attribute3.
        /// </summary>
        /// <value>
        /// The trade attribute3.
        /// </value>
        public string TradeAttribute3
        {
            get
            {
                return cmbTradeAttribute3.Text;
            }
        }

        /// <summary>
        /// Gets or sets the trade attribute4.
        /// </summary>
        /// <value>
        /// The trade attribute4.
        /// </value>
        public string TradeAttribute4
        {
            get
            {
                return cmbTradeAttribute4.Text;
            }
        }

        /// <summary>
        /// Gets or sets the trade attribute5.
        /// </summary>
        /// <value>
        /// The trade attribute5.
        /// </value>
        public string TradeAttribute5
        {
            get
            {
                return cmbTradeAttribute5.Text;
            }
        }

        /// <summary>
        /// Gets or sets the trade attribute6.
        /// </summary>
        /// <value>
        /// The trade attribute6.
        /// </value>
        public string TradeAttribute6
        {
            get
            {
                return cmbTradeAttribute6.Text;
            }
        }

        /// <summary>
        /// Gets or sets the trade date.
        /// </summary>
        /// <value>
        /// The trade date.
        /// </value>
        public DateTime TradeDate
        {
            get
            {
                return (DateTime)dtTradeDate.Value;
            }
        }

        /// <summary>
        /// Gets or sets the trading account.
        /// </summary>
        /// <value>
        /// The trading account.
        /// </value>
        public string TradingAccount
        {
            get
            {
                return (string)cmbTradingAccount.Value;
            }
        }

        /// <summary>
        /// Gets the venue.
        /// </summary>
        /// <value>
        /// The venue.
        /// </value>
        public string Venue
        {
            get
            {
                return cmbVenue.Text;
            }
        }

        /// <summary>
        /// Gets the venue identifier.
        /// </summary>
        /// <value>
        /// The venue identifier.
        /// </value>
        public string VenueId
        {
            get
            {
                return (string)cmbVenue.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is swap.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is swap; otherwise, <c>false</c>.
        /// </value>
        public bool IsSwap
        {
            get
            {
                return chkBoxSwap.Checked;
            }

        }

        /// <summary>
        /// Gets the control swap parameter.
        /// </summary>
        /// <value>
        /// The control swap parameter.
        /// </value>
        public CtrlSwapParameters CtrlSwapParameter
        {
            get
            {
                return ctrlSwapParameters1;
            }
        }

        /// <summary>
        /// otc Trade View UI instance
        /// </summary>
        OTCTradeDetailsView otcTradeViewUI = null;

        /// <summary>
        /// Is New OTC Work flow Enabled 
        /// </summary>
        private bool _isNewOTCWorkflowEnabled;

        /// <summary>
        /// Gets and Set OTC swap parameter
        /// </summary>
        private OTCTradeData _otcParameters;
        public OTCTradeData OTCParameters
        {
            get { return _otcParameters; }
        }

        /// <summary>
        /// Get and Set increment value for quantity and target quantity field
        /// </summary>
        private decimal increment { get; set; }

        public bool IsUseCustodianAsExecutingBroker
        {
            get { return _isUseCustodianAsExecutingBroker; }
        }

        /// <summary>
        /// Create SM Sync Proxy 
        /// </summary>
        private void CreateSMSyncPoxy()
        {
            try
            {
                _secMasterSyncService = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
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
        /// Remove PTT to account combo and enable it
        /// </summary>
        public void RemovePTTFromAccountCombo()
        {
            try
            {
                btnViewAllocationDetails.Visible = false;
                DataTable dt = cmbAllocation.DataSource as DataTable;
                int indexOfPTTRow = -1;
                if (dt != null)
                    foreach (DataRow dataRow in dt.Rows.Cast<DataRow>().Where(dataRow => dataRow[1].ToString() == "PTT"))
                    {
                        indexOfPTTRow = dt.Rows.IndexOf(dataRow);
                        break;
                    }

                if (indexOfPTTRow != -1)
                {
                    dt.Rows.RemoveAt(indexOfPTTRow);
                }
                cmbAllocation.Value = int.MinValue;
                cmbAllocation.Refresh();
                cmbAllocation.Enabled = true;
                btnAccountQty.Enabled = true;
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
        /// Calls the symbol control text entered event.
        /// </summary>
        public void SetIsSwap(bool isSwap)
        {
            try
            {
                SetSwapCheckBox mi = SetIsSwap;
                if (UIValidation.GetInstance().validate(this))
                {
                    if (chkBoxSwap.InvokeRequired)
                    {
                        BeginInvoke(mi, isSwap);
                    }
                    else
                    {
                        chkBoxSwap.Checked = isSwap;
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
        /// Calls the symbol control text entered event.
        /// </summary>
        public void CallSymbolControlTextEnteredEvent(string symbol)
        {
            pranaSymbolCtrl_SymbolEntered(this, new EventArgs<string, string>(symbol, ""));
        }

        /// <summary>
        /// Closes the ticket.
        /// </summary>
        public void CloseTicket()
        {
            try
            {
                _tradingTicketClosedAfterExecutingTrade = true;
                Close();
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
        /// Draws the control.
        /// </summary>
        /// <param name="secMasterObj">The sec master object.</param>
        public void DrawControl(SecMasterBaseObj secMasterObj)
        {
            try
            {
                DrawControlBySecMasterData mi = DrawControl;
                ttPresenter.AssetID = secMasterObj.AssetID;
                if (UIValidation.GetInstance().validate(this))
                {
                    if (pranaSymbolCtrl.InvokeRequired)
                    {
                        try
                        {
                            BeginInvoke(mi, secMasterObj);
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
                    else
                    {
                        TradingTicketEnabled(true);
                        ttPresenter.IsShowTargetQTY = IsShowTargetQTY;
                        if (ttPresenter.AssetID != int.MinValue && ttPresenter.UnderlyingID != int.MinValue)
                        {
                            if (!_isNewOTCWorkflowEnabled)
                                chkBoxSwap.Visible = (AssetCategory)Int32.Parse(ttPresenter.AssetID.ToString()) == AssetCategory.Equity;

                            if (!secMasterObj.IsSecApproved)
                            {
                                lblErrorMessage.Text = TradingTicketConstants.MSG_SECURITY_NOT_APPROVED;
                                ttPresenter.SymbolAction = SecMasterConstants.SecurityActions.APPROVE;
                            }
                            else
                            {
                                lblErrorMessage.Text = String.Empty;
                                ttPresenter.SymbolAction = SecMasterConstants.SecurityActions.SEARCH;
                            }

                            //set fundId 
                            if (CachedDataManager.GetInstance.IsShowMasterFundonTT() && cmbFunds.SelectedIndex >= 0)
                            {
                                int fundId = Convert.ToInt32(cmbFunds.SelectedItem.DataValue.ToString());
                                ttPresenter.FundId = fundId;
                            }

                            ttPresenter.FillComboBoxes();
                            if (pranaSymbolCtrl.Text.Trim().ToUpper() == secMasterObj.RequestedSymbol.ToUpper() ||
                                pranaSymbolCtrl.Text.Trim().ToUpper() == secMasterObj.FactSetSymbol.ToUpper() ||
                                pranaSymbolCtrl.Text.Trim().ToUpper() == secMasterObj.ActivSymbol.ToUpper() ||
                                pranaSymbolCtrl.Text.Trim().ToUpper() == secMasterObj.BloombergSymbol.ToUpper() ||
                                pranaSymbolCtrl.Text.Trim().ToUpper() == secMasterObj.BloombergSymbolWithExchangeCode.ToUpper())
                            {
                                if (cmbSettlementCurrency.Value == null)
                                {
                                    cmbSettlementCurrency.Value = secMasterObj.CurrencyID;
                                }

                                RepositionControlsBasedOnAsset(secMasterObj);

                                SettlementDetailsSetup();

                                if (ttPresenter.IncomingOrderRequest != null)
                                {
                                    SetTradingTicket(ttPresenter.IncomingOrderRequest, ttPresenter.AllocationPrefId);
                                }

                                SetCustodianBrokerPrefernce();

                                if (ttPresenter.MarketDataEanbled)
                                {
                                    ttPresenter.TicketPresenterBase_UpdateSymbolPositonExposeAndPNL(this, null);
                                }

                                if (ttPresenter.IncomingOrderRequest == null)
                                {
                                    btnCreateOrder.Enabled = true;
                                    // btnDoneAway.Enabled = true;
                                    btnSend.Enabled = true;
                                }
                            }
                            if (ttPresenter.IncomingOrderRequest != null && ttPresenter.IncomingOrderRequest.TransactionSource == TransactionSource.Blotter)
                            {
                                if (ttPresenter.IncomingOrderRequest.OrderTypeTagValue == FIXConstants.ORDTYPE_Stop || ttPresenter.IncomingOrderRequest.OrderTypeTagValue == FIXConstants.ORDTYPE_Stoplimit)
                                    nmrcStop.Focus();
                                if (ttPresenter.IncomingOrderRequest.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit)
                                    nmrcLimit.Focus();
                            }

                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(secMasterObj.Comments))
                            {
                                SetLabelMessage(TradingTicketConstants.MSG_SECURITY_NOT_VALID);
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
        /// Set Custodian Broker Prefrence on TT load
        /// </summary>
        public void SetCustodianBrokerPrefernce()
        {
            try
            {
                if (TradingTktPrefs.TTGeneralPrefs.IsUseCustodianAsExecutingBroker)
                {
                    if (ttPresenter.IncomingOrderRequest != null && !ttPresenter.IsComingPM)
                    {
                        string msgType = ttPresenter.IncomingOrderRequest.MsgType;
                        if ((OrderFields.PranaMsgTypes)ttPresenter.IncomingOrderRequest.PranaMsgType == OrderFields.PranaMsgTypes.ORDStaged ||
                            (msgType != FIXConstants.MSGOrderCancelReplaceRequest && msgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX &&
                           msgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew && msgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrder))
                        {
                            _isUseCustodianAsExecutingBroker = ttPresenter.IncomingOrderRequest.IsUseCustodianBroker;
                            if (_isUseCustodianAsExecutingBroker)
                            {
                                UpdateUIAsPerCustodianBrokerPref();
                            }
                            else
                            {
                                btnPadlock.Visible = true;
                                btnPadlock.Appearance.Image = global::Prana.TradingTicket.Properties.Resources.lock_open;
                            }
                        }
                        if ((OrderFields.PranaMsgTypes)ttPresenter.IncomingOrderRequest.PranaMsgType == OrderFields.PranaMsgTypes.ORDStaged && msgType == FIXConstants.MSGOrderCancelReplaceRequest)
                        {
                            btnPadlock.Enabled = false;
                            btnMultiBrokerConnectionStatus.Enabled = false;
                            UpdateUIAsPerCustodianBrokerPref();
                        }
                    }
                    else
                    {
                        _isUseCustodianAsExecutingBroker = ttPresenter.IsAllocationValidForCustodianBrokerPref(Convert.ToInt32(cmbAllocation.Value));
                        UpdateUIAsPerCustodianBrokerPref();
                    }
                }
                _isUpdateBrokerBasedOnCustodianPreference = true;
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
        /// Enables the prana symbol control.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void EnablePranaSymbolControl(bool value)
        {
            pranaSymbolCtrl.Enabled = value;
            chkBoxOption.Enabled = value;
            grbBoxOptionControl.Enabled = value;
            grbBoxOptionControl.Expanded = value;
        }

        /// <summary>
        /// Fills the account combo.
        /// </summary>
        /// <param name="sortTable">The sort table.</param>
        public void FillAccountCombo(DataTable sortTable)
        {
            try
            {
                cmbAllocation.Value = null;
                cmbAllocation.DataSource = null;
                cmbAllocation.DataSource = sortTable;
                cmbAllocation.DisplayMember = OrderFields.PROPERTY_LEVEL1NAME;
                cmbAllocation.ValueMember = OrderFields.PROPERTY_LEVEL1ID;
                cmbAllocation.DataBind();
                if (cmbAllocation.Value == null && sortTable.Rows.Count > 0)
                {
                    UpdateAccountComboValue(int.MinValue, ChangeType.NoTrade);
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
        /// Fills the broker combo value.
        /// </summary>
        /// <param name="dt">The dt.</param>
        public void FillBrokerComboValue(DataTable dt)
        {
            try
            {
                cmbBroker.Value = null;
                cmbBroker.DataSource = null;
                cmbBroker.DataSource = dt;
                cmbBroker.DisplayMember = TradingTicketConstants.LIT_DISPLAY;
                cmbBroker.ValueMember = TradingTicketConstants.LIT_VALUE;
                cmbBroker.DataBind();

                ttPresenter.CounterPartyId = (cmbBroker.Value != null) ? int.Parse(cmbBroker.Value.ToString()) : int.MinValue;
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
        /// Fills the commission basis.
        /// </summary>
        /// <param name="list">The list.</param>
        public void FillCommissionBasis(IList list)
        {
            try
            {
                cmbCommissionBasis.DataSource = list;
                cmbCommissionBasis.DisplayMember = TradingTicketConstants.LIT_VALUE_SMALL;
                cmbCommissionBasis.ValueMember = TradingTicketConstants.LIT_KEY;

                cmbSoftCommissionBasis.DataSource = list;
                cmbSoftCommissionBasis.DisplayMember = TradingTicketConstants.LIT_VALUE_SMALL;
                cmbSoftCommissionBasis.ValueMember = TradingTicketConstants.LIT_KEY;
                cmbCommissionBasis.SelectedIndex = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis.ContainsKey(ttPresenter.CounterPartyId) ? TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis[ttPresenter.CounterPartyId] : 4;
                cmbSoftCommissionBasis.SelectedIndex = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseSoftCommissionBasis.ContainsKey(ttPresenter.CounterPartyId) ? TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseSoftCommissionBasis[ttPresenter.CounterPartyId] : 4;
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
        /// Fills the execution instructions.
        /// </summary>
        /// <param name="executionInstrucitons">The execution instruction.</param>
        public void FillExecutionInstructions(DataTable executionInstrucitons)
        {
            try
            {
                cmbExecutionInstructions.Value = null;
                cmbExecutionInstructions.DataSource = null;
                cmbExecutionInstructions.DataSource = executionInstrucitons;
                cmbExecutionInstructions.DisplayMember = OrderFields.CAPTION_EXECUTION_INST;
                cmbExecutionInstructions.ValueMember = OrderFields.PROPERTY_EXECUTION_INSTID;
                cmbExecutionInstructions.DataBind();
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
        /// Fills the handling instruction.
        /// </summary>
        /// <param name="handlingInstrucitons">The handling instrucitons.</param>
        public void FillHandlingInstruction(DataTable handlingInstrucitons)
        {
            try
            {
                cmbHandlingInstructions.Value = null;
                cmbHandlingInstructions.DataSource = null;
                cmbHandlingInstructions.DataSource = handlingInstrucitons;
                cmbHandlingInstructions.DisplayMember = OrderFields.PROPERTY_HANDLING_INST;
                cmbHandlingInstructions.ValueMember = OrderFields.PROPERTY_HANDLING_INSTID;
                cmbHandlingInstructions.DataBind();
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
        /// Fills the order side.
        /// </summary>
        /// <param name="sides">The sides.</param>
        public void FillOrderSide(DataTable sides)
        {
            try
            {
                FillOrderSideDelegate mi = FillOrderSide;
                if (UIValidation.GetInstance().validate(cmbOrderSide))
                {
                    if (cmbOrderSide.InvokeRequired)
                    {
                        BeginInvoke(mi, sides);
                    }
                    else
                    {
                        string perviousValue = string.Empty;
                        if (cmbOrderSide.Value != null)
                        {
                            perviousValue = cmbOrderSide.Value.ToString();
                        }

                        cmbOrderSide.Value = null;
                        cmbOrderSide.DataSource = null;
                        cmbOrderSide.DataSource = sides;
                        cmbOrderSide.DisplayMember = OrderFields.PROPERTY_ORDER_SIDE;
                        cmbOrderSide.ValueMember = OrderFields.PROPERTY_ORDER_SIDEID;
                        cmbOrderSide.DataBind();
                        if (perviousValue != string.Empty)
                        {
                            cmbOrderSide.Value = perviousValue;
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
        /// Fills the type of the order.
        /// </summary>
        /// <param name="types">The types.</param>
        public void FillOrderType(DataTable types)
        {
            try
            {
                cmbOrderType.Value = null;
                cmbOrderType.DataSource = null;
                cmbOrderType.DataSource = types;
                cmbOrderType.DisplayMember = OrderFields.PROPERTY_ORDER_TYPE;
                cmbOrderType.ValueMember = OrderFields.PROPERTY_ORDER_TYPE_ID;
                cmbOrderType.DataBind();
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
        /// <param name="currencies">The currencies.</param>
        public void FillSettlementCurrency(ValueList currencies)
        {
            try
            {
                if (cmbSettlementCurrency.ValueList != null)
                {
                    object settlCurrencyValue = null;
                    if (cmbSettlementCurrency.Value != null)
                    {
                        settlCurrencyValue = cmbSettlementCurrency.Value;
                    }
                    cmbSettlementCurrency.ValueList = currencies;
                    cmbSettlementCurrency.Value = settlCurrencyValue;
                    cmbSettlementCurrency.Enabled = cmbSettlementCurrency.Items.Count == 1 ? false : true;
                    if (cmbSettlementCurrency.Value == null && ttPresenter != null && ttPresenter.SecmasterObj != null)
                    {
                        cmbSettlementCurrency.Value = ttPresenter.SecmasterObj.CurrencyID;
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
        /// Fills the startegy combo.
        /// </summary>
        /// <param name="strategies">The strategies.</param>
        public void FillStartegyCombo(StrategyCollection strategies)
        {
            try
            {
                if (strategies.Contains(int.MinValue))
                {
                    strategies.RemoveAt(strategies.IndexOf(int.MinValue));
                }
                cmbStrategy.Value = null;
                cmbStrategy.DataSource = null;
                cmbStrategy.DataSource = strategies;
                cmbStrategy.DisplayMember = TradingTicketConstants.LIT_NAME;
                cmbStrategy.ValueMember = TradingTicketConstants.LIT_STRATEGY_ID;
                cmbStrategy.DataBind();
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
        /// Fills the tif.
        /// </summary>
        /// <param name="tifs">The tifs.</param>
        public void FillTIF(DataTable tifs)
        {
            try
            {
                cmbTIF.Value = null;
                cmbTIF.DataSource = null;
                cmbTIF.DataSource = tifs;
                cmbTIF.DisplayMember = OrderFields.PROPERTY_TIF;
                cmbTIF.ValueMember = OrderFields.PROPERTY_TIFID;
                cmbTIF.DataBind();
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
        /// <param name="newDt">The new dt.</param>
        public void FillTradingAccountCombo(DataTable newDt)
        {
            try
            {
                cmbTradingAccount.Value = null;
                cmbTradingAccount.DataSource = null;
                cmbTradingAccount.DataSource = newDt;
                cmbTradingAccount.DisplayMember = TradingTicketConstants.LIT_DISPLAY;
                cmbTradingAccount.ValueMember = TradingTicketConstants.LIT_VALUE;
                cmbTradingAccount.DataBind();
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
        /// Fills the fx operator combo.
        /// </summary>
        /// <param name="FxOperator">The fx operator.</param>
        public void FillFxOperatorCombo(ValueList FxOperator)
        {
            try
            {
                if (cmbFxOperator.ValueList == null) return;
                cmbFxOperator.ValueList = FxOperator;
                var item = cmbFxOperator.Items[0];
                cmbFxOperator.SelectedItem = item;
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
        /// <param name="dt">The dt.</param>
        public void FillVenueCombo(DataTable dt)
        {
            try
            {
                cmbVenue.Value = null;
                cmbVenue.DataSource = null;
                cmbVenue.DataSource = dt;
                cmbVenue.DisplayMember = TradingTicketConstants.LIT_DISPLAY;
                cmbVenue.ValueMember = TradingTicketConstants.LIT_VALUE;
                cmbVenue.DataBind();
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
        /// Gets all.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            IEnumerable<Control> enumerable = controls as IList<Control> ?? controls.ToList();
            return enumerable.SelectMany(ctrl => GetAll(ctrl, type)).Concat(enumerable).Where(c => c.GetType() == type);
        }

        /// <summary>
        /// Launches the symbol lookup method.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void LaunchSymbolLookupMethod(ListEventAargs args)
        {
            try
            {
                if (LaunchSymbolLookup != null)
                {
                    LaunchSymbolLookup(this, args);
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
        /// Level1s the snapshot response.
        /// </summary>
        /// <param name="l1data">The l1data.</param>
        public void Level1SnapshotResponse(SymbolData l1data)
        {
            try
            {
                if (Disposing)
                {
                    return;
                }
                string tickerSymbol = ttPresenter.Symbol;
                if (ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward || ttPresenter.AssetID == (int)AssetCategory.Forex)
                {
                    ConversionRate conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetConversionRateFromCurrencies(ttPresenter.LeadCurrencyId, ttPresenter.VsCurrencyId, 0);
                    if (conversionRate != null)
                    {
                        if (!String.IsNullOrEmpty(conversionRate.FXeSignalSymbol))
                        {
                            tickerSymbol = conversionRate.FXeSignalSymbol;
                        }
                    }
                }
                if (oneSymbolL1Strip.Symbol != tickerSymbol)
                {
                    return;
                }
                oneSymbolL1Strip.onSnapshotResponse(l1data);

                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        Level1SnapshotResponseSameThread delegatesamethread = Level1SnapshotResponse;
                        BeginInvoke(delegatesamethread, l1data);
                    }
                    else
                    {
                        //Check if the data received is of the same symbol as mentioned in the ticket
                        if (l1data != null)
                        {
                            if (l1data.Symbol == ttPresenter.Symbol)
                            {
                                if (l1data != null && l1data.LastPrice > 0)
                                {
                                    if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                                        ttPresenter.MarketPrice = l1data.LastPrice;

                                    UpdateNotionalValueOnCaption();
                                    UpdateCaption();
                                }
                                UpdatePricesFromLiveFeed(l1data.Ask, l1data.Bid, l1data.LastPrice);
                                ttPresenter.UpdateOrderRequest(l1data);
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
        /// Updates the prices from live feed.
        /// </summary>
        /// <param name="askPrice">The ask price.</param>
        /// <param name="bidPrice">The bid price.</param>
        /// <param name="lastPrice">The last price.</param>
        private void UpdatePricesFromLiveFeed(double askPrice, double bidPrice, double lastPrice)
        {
            try
            {
                if (!CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                    return;

                //Change the value in the limit text box only if it has ZERO
                //We have set limit text box to ZERO whenever the symbol changes
                // In case if last price is 0 from live feed, we need to pickup side dependent values.
                double limitPrice = 0.0;
                Double.TryParse(nmrcLimit.Text, out limitPrice);
                if (limitPrice == 0.0)
                {
                    if (cmbOrderSide.Value != null)
                    {
                        decimal value = 0;
                        switch (TagDatabaseManager.GetInstance.GetOrderSideTagValueBasedOnId(cmbOrderSide.Value.ToString()))
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_BuyMinus:
                            case FIXConstants.SIDE_Buy_Closed:
                            case FIXConstants.SIDE_Buy_Open:

                                if (askPrice == 0.0 && TradingTktPrefs.TTGeneralPrefs.IsPopulatelastPriceInPriceWhenAskORBidIsZero)
                                {
                                    value = Convert.ToDecimal(lastPrice);
                                }
                                else if (askPrice != double.MinValue)
                                {
                                    value = Convert.ToDecimal(askPrice);
                                }
                                nmrcLimit.Value = value;
                                nmrcPrice.Value = value;
                                break;

                            case FIXConstants.SIDE_Sell:
                            case FIXConstants.SIDE_SellShort:
                            case FIXConstants.SIDE_SellShortExempt:
                            case FIXConstants.SIDE_Sell_Closed:
                            case FIXConstants.SIDE_Sell_Open:
                                if (bidPrice == 0.0 && TradingTktPrefs.TTGeneralPrefs.IsPopulatelastPriceInPriceWhenAskORBidIsZero)
                                {
                                    value = Convert.ToDecimal(lastPrice);

                                }
                                else if (bidPrice != double.MinValue)
                                {
                                    value = Convert.ToDecimal(bidPrice);
                                }
                                nmrcLimit.Value = value;
                                nmrcPrice.Value = value;
                                break;

                            default:
                                if (lastPrice != double.MinValue)
                                {
                                    nmrcLimit.Value = Convert.ToDecimal(lastPrice);
                                    nmrcPrice.Value = Convert.ToDecimal(lastPrice);
                                }
                                break;
                        }
                        if (ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward || ttPresenter.AssetID == (int)AssetCategory.Forex)
                        {
                            if (ttPresenter.LeadCurrencyId != ttPresenter.CompanyBaseCurrencyID)
                                nmrcFXRate.Value = value;
                            else
                                nmrcFXRate.Value = Math.Round(value != 0 ? 1 / value : 0, 4);
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

        /// <summary>
        /// References this instance.
        /// </summary>
        /// <returns></returns>
        public Form Reference()
        {
            return this;
        }

        /// <summary>
        /// Refreshes the control.
        /// </summary>
        public void RefreshControl(bool refreshOption)
        {
            try
            {
                ttPresenter.IsShowAlgoControls = false;
                var ultraComobEditors = GetAll(this, typeof(UltraComboEditor));
                foreach (UltraComboEditor ucmbEditor in ultraComobEditors.Cast<UltraComboEditor>().Where(ucmbEditor => !ucmbEditor.Name.Equals("ultraComboEditorSymbology") && !ucmbEditor.Name.Equals("txtSymbol") && !ucmbEditor.Name.Equals("cmbOptionType") && !ucmbEditor.Name.Equals("cmbFunds")))
                {
                    ucmbEditor.Value = null;
                }

                foreach (UltraComboEditor ucmbEditor in ultraComobEditors.Cast<UltraComboEditor>().Where(ucmbEditor => !ucmbEditor.Name.Equals("ultraComboEditorSymbology") && !ucmbEditor.Name.Equals("txtSymbol") && !ucmbEditor.Name.Equals("cmbOptionType") && !ucmbEditor.Name.Equals("cmbTradeAttribute1") && !ucmbEditor.Name.Equals("cmbTradeAttribute2") && !ucmbEditor.Name.Equals("cmbTradeAttribute3") && !ucmbEditor.Name.Equals("cmbTradeAttribute4") && !ucmbEditor.Name.Equals("cmbTradeAttribute5") && !ucmbEditor.Name.Equals("cmbTradeAttribute6") && !ucmbEditor.Name.Equals("cmbOptionType") && !ucmbEditor.Name.Equals("cmbFunds")))
                {
                    ucmbEditor.DataSource = null;
                    ucmbEditor.Enabled = true;
                }
                var ultraNumericEditors = GetAll(this, typeof(UltraNumericEditor));
                foreach (UltraNumericEditor unmrcEditor in ultraNumericEditors)
                {
                    unmrcEditor.Value = 0;
                }
                var numericUpDownEx = GetAll(this, typeof(PranaNumericUpDown));
                foreach (PranaNumericUpDown numericUpDownExEditor in numericUpDownEx)
                {
                    numericUpDownExEditor.Value = 0;
                }
                txtBrokerNotes.Text = String.Empty;
                txtNotes.Text = String.Empty;
                strategyControl1.Visible = false;
                strategyControl1.Reset();
                chkBoxSwap.Checked = false;
                chkBoxSwap.Visible = false;
                btnViewAllocationDetails.Visible = false;
                btnShortLocateList.Visible = false;
                ChangeFormTitle();
                ttPresenter.OrderRequest = new OrderSingle();
                grbBoxStrategyControl.Enabled = false;
                grbBoxStrategyControl.Expanded = false;
                errorProvider.SetError(nmrcPrice, String.Empty);
                _isTTSourceDependentOnAnotherUIs = false;
                SetExpireTimeVisibility(false);
                oneSymbolL1Strip.setRoundLotButton(false);
                if (refreshOption)
                    RefreshOptionControl();
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
        /// Selects the combo side.
        /// </summary>
        /// <param name="OrderSideTagValue">The order side tag value.</param>
        /// <param name="openOrClose">The open or close.</param>
        public void SelectComboSide(string OrderSideTagValue, string openOrClose)
        {
            try
            {
                string orderSideID = String.Empty;
                switch (openOrClose)
                {
                    case FIXConstants.Open:
                    case TradingTicketConstants.LIT_OPEN:
                        if (OrderSideTagValue == FIXConstants.SIDE_Buy)
                        {
                            orderSideID = TagDatabaseManager.GetInstance.GetOrderSideIdBasedOnSideTagValue(FIXConstants.SIDE_Buy_Open);
                        }
                        else if (OrderSideTagValue == FIXConstants.SIDE_Sell)
                        {
                            orderSideID = TagDatabaseManager.GetInstance.GetOrderSideIdBasedOnSideTagValue(FIXConstants.SIDE_Sell_Open);
                        }
                        else
                        {
                            orderSideID = TagDatabaseManager.GetInstance.GetOrderSideIdBasedOnSideTagValue(OrderSideTagValue);
                        }
                        break;

                    case FIXConstants.Close:
                    case TradingTicketConstants.LIT_CLOSE:
                        if (OrderSideTagValue == FIXConstants.SIDE_Buy)
                        {
                            orderSideID = TagDatabaseManager.GetInstance.GetOrderSideIdBasedOnSideTagValue(FIXConstants.SIDE_Buy_Closed);
                        }
                        else if (OrderSideTagValue == FIXConstants.SIDE_Sell)
                        {
                            orderSideID = TagDatabaseManager.GetInstance.GetOrderSideIdBasedOnSideTagValue(FIXConstants.SIDE_Sell_Closed);
                        }
                        else
                        {
                            orderSideID = TagDatabaseManager.GetInstance.GetOrderSideIdBasedOnSideTagValue(OrderSideTagValue);
                        }
                        break;

                    default:
                        orderSideID = TagDatabaseManager.GetInstance.GetOrderSideIdBasedOnSideTagValue(OrderSideTagValue);
                        break;
                }

                if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbOrderSide.ValueList, orderSideID))
                {
                    cmbOrderSide.Value = orderSideID;
                    OpenShortLocatePopup();
                }
                else
                    cmbOrderSide.Value = null;

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
        /// Sets the algo details.
        /// </summary>
        /// <param name="auecID">The auec identifier.</param>
        public void SetAlgoDetails(int auecID)
        {
            try
            {
                if (CachedDataManager.GetInstance.IsAlgoBrokerFromID(ttPresenter.CounterPartyId))
                {
                    OrderSingle orderAlgoDetails = new OrderSingle { AUECID = auecID };
                    algoStrategyControl.SetAlgoDetails(orderAlgoDetails);
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
        /// Sets the combo side value.
        /// </summary>
        /// <param name="or">The or.</param>
        public void SetComboSideValue(OrderSingle or)
        {
            try
            {
                SelectComboSide(or.OrderSideTagValue, or.OpenClose);
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
        /// Sets the dealIn combo value.
        /// </summary>
        /// <param name="currencyList">The currency list.</param>
        public void SetDealInComboValue(Dictionary<int, string> currencyList)
        {
            try
            {
                cmbDealIn.Items.Clear();
                foreach (KeyValuePair<int, string> var in currencyList)
                {
                    cmbDealIn.Items.Add(var.Key, var.Value);
                }
                cmbDealIn.SelectedIndex = 0;
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
        ///
        /// </summary>
        /// <param name="errMessage">The Error Message.</param>
        public delegate void SetLabelMessageDelegate(string errMessage);
        /// <summary>
        /// Sets the error message.
        /// </summary>
        /// <param name="errMessage">The error message.</param>
        public void SetLabelMessage(string errMessage)
        {
            try
            {
                SetLabelMessageDelegate setLabelMessageDelegate = SetLabelMessage;
                if (UIValidation.GetInstance().validate(lblErrorMessage))
                {
                    if (lblErrorMessage.InvokeRequired)
                    {
                        try
                        {
                            BeginInvoke(setLabelMessageDelegate, errMessage);
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
                    else
                    {
                        lblErrorMessage.Text = errMessage;
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
        /// Awaits the symbol validation.
        /// </summary>
        public void AwaitSymbolValidation()
        {
            SetLabelMessage(TradingTicketConstants.MSG_SYMBOL_MAY_NOT_EXIXSTS_ADD_SYMBOL_LOOKUP);
        }

        /// <summary>
        /// Sets the message box text.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SetMessageBoxText(string message)
        {
            try
            {
                ValidateMessageBox(message);
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
        /// Sets the message box text and get dialog result.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="messageBoxButtons">The message box buttons.</param>
        /// <returns></returns>
        public DialogResult SetMessageBoxTextAndGetDialogResult(string errorMessage, string caption, MessageBoxButtons messageBoxButtons)
        {
            return ValidateMessageBox(errorMessage, caption, messageBoxButtons);
        }

        /// <summary>
        /// Sets the Custom Message Box message and header.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="message"></param>
        public void SetCustomMessageBoxHeaderAndText(string header, string message)
        {
            try
            {
                CustomMessageBox popUpMessage = new CustomMessageBox(header, message, false, string.Empty, FormStartPosition.CenterScreen);
                popUpMessage.ShowDialog();
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
        /// Sets the prana symbol control text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetPranaSymbolControlText(string text)
        {
            try
            {
                pranaSymbolCtrl.SymbolEntered -= pranaSymbolCtrl_SymbolEntered;
                pranaSymbolCtrl.Text = text;
                CallSymbolControlTextEnteredEvent(text);
                pranaSymbolCtrl.SymbolEntered += pranaSymbolCtrl_SymbolEntered;

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
        /// Sets the prana symbol control text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetPranaFundDefaultValue(OrderSingle order)
        {
            try
            {
                if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    cmbFunds.ValueChanged -= cmbFunds_ValueChanged;
                    if (!string.IsNullOrEmpty(order.MasterFund) && !order.MasterFund.Equals("-") && !order.MasterFund.Equals("Multiple"))
                    {
                        cmbFunds.Text = order.MasterFund;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(order.TradeAttribute6))
                            cmbFunds.Text = order.TradeAttribute6;
                        else
                            cmbFunds.Text = "Multiple";
                    }
                    if (order.TradingAccountID > 0)
                        UpdateTradingAccountCombo(order.TradingAccountID, ChangeType.NoTrade);
                    cmbFunds.ValueChanged += cmbFunds_ValueChanged;
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
        /// Sets the symbol l1 strip.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public void SetSymbolL1Strip(string symbol)
        {
            try
            {
                SetSymbolL1StripDeglate setSymbolL1Strip = SetSymbolL1Strip;
                if (UIValidation.GetInstance().validate(oneSymbolL1Strip))
                {
                    if (oneSymbolL1Strip.InvokeRequired)
                    {
                        try
                        {
                            BeginInvoke(setSymbolL1Strip, symbol);
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
                    else
                    {
                        ttPresenter.IsPricingAvailable = false;
                        string tickerSymbol = symbol;
                        List<string> requestedSymbols = new List<string>();

                        if (ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward || ttPresenter.AssetID == (int)AssetCategory.Forex)
                        {
                            ConversionRate conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetConversionRateFromCurrencies(ttPresenter.LeadCurrencyId, ttPresenter.VsCurrencyId, 0);
                            if (conversionRate != null)
                            {
                                if (!String.IsNullOrEmpty(conversionRate.FXeSignalSymbol))
                                {
                                    tickerSymbol = conversionRate.FXeSignalSymbol;
                                }
                            }

                            string currencyText = CachedDataManager.GetInstance.GetCurrencyText(ttPresenter.VsCurrencyId);
                            string companyBaseCurrency = CachedDataManager.GetInstance.GetCurrencyText(ttPresenter.CompanyBaseCurrencyID);
                            if (!currencyText.Equals(companyBaseCurrency))
                                requestedSymbols.Add(currencyText + "-" + companyBaseCurrency);
                        }

                        string underlyingSymbol = ttPresenter.UnderlyingSymbol;
                        requestedSymbols.Add(symbol);
                        if (!symbol.Equals(underlyingSymbol))
                            requestedSymbols.Add(underlyingSymbol);
                        oneSymbolL1Strip.RequestSnapshotForCompliance(requestedSymbols);

                        oneSymbolL1Strip.Symbol = tickerSymbol;
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
        /// Sets the ticket preferences.
        /// </summary>
        /// <param name="userTradingTicketuserUiPrefs">The user trading ticketuser UI prefs.</param>
        /// <param name="companyTradingTicketUiPrefs">The company trading ticket UI prefs.</param>
        public void SetTicketPreferences(TradingTicketUIPrefs userTradingTicketuserUiPrefs, TradingTicketUIPrefs companyTradingTicketUiPrefs)
        {
            try
            {
                if (userTradingTicketuserUiPrefs != null && companyTradingTicketUiPrefs != null)
                {
                    if (this.TradingTicketParent != TradingTicketParent.ShortLocate && (cmbOrderSide.Value == null || userTradingTicketuserUiPrefs.DefAssetSides.Count > 0 || companyTradingTicketUiPrefs.DefAssetSides.Count > 0))
                    {
                        if (string.IsNullOrEmpty(ttPresenter.OrderRequest.OrderSideTagValue))
                        {
                            if (!(_watchListColumn.Equals("Ask") || _watchListColumn.Equals("Bid") || _watchListColumn.Equals("BuyToOpen") || _watchListColumn.Equals("SellToOpen")))
                                SetPreferredOrderSide(userTradingTicketuserUiPrefs, companyTradingTicketUiPrefs);
                            else
                            {
                                string tagValue = string.Empty;
                                switch (_watchListColumn)
                                {
                                    case "Ask":
                                        tagValue = GetTagValueOnAssetForAsk(); ;
                                        break;
                                    case "Bid":
                                        tagValue = GetTagValueOnAssetForBid();
                                        break;

                                    case "BuyToOpen":
                                        tagValue = FIXConstants.SIDE_Buy_Open;
                                        break;
                                    case "SellToOpen":
                                        tagValue = FIXConstants.SIDE_Sell_Open;
                                        break;
                                }
                                _watchListColumn = string.Empty;
                                UpdateSideCombo(TagDatabaseManager.GetInstance.GetOrderSideIdBasedOnSideTagValue(tagValue), ChangeType.NoTrade);
                            }
                        }
                        else
                        {
                            UpdateSideCombo(TagDatabaseManager.GetInstance.GetOrderSideIdBasedOnSideTagValue(ttPresenter.OrderRequest.OrderSideTagValue), ChangeType.NoTrade);
                        }
                    }
                    int accountID;
                    if (int.TryParse(userTradingTicketuserUiPrefs.Account.ToString(), out accountID))
                    {
                        UpdateAccountComboValue(accountID, ChangeType.NoTrade);
                    }
                    else if (int.TryParse(companyTradingTicketUiPrefs.Account.ToString(), out accountID))
                    {
                        UpdateAccountComboValue(accountID, ChangeType.NoTrade);
                    }


                    if (ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward)
                    {
                        btnQuantity.Enabled = false;
                    }
                    else if (!btnQuantity.Enabled)
                    {
                        btnQuantity.Enabled = true;
                    }


                    if (userTradingTicketuserUiPrefs.QuantityType == QuantityTypeOnTT.Quantity || ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward)
                    {
                        ttPresenter.UseQuantityFieldAsNotional = false;
                    }
                    else
                    {
                        //UseQuantityFieldAsNotional will be use only if dollarAmountPermission is given to TT
                        if (Boolean.Parse(TradingTktPrefs.DollarAmountPermission.ToString()))
                            ttPresenter.UseQuantityFieldAsNotional = true;
                    }
                    //Quantity button will be hide if TT do not have the dollar Amount permission
                    if (!Boolean.Parse(TradingTktPrefs.DollarAmountPermission.ToString()))
                    {
                        btnQuantity.Visible = false;
                        if (!(ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward || ttPresenter.AssetID == (int)AssetCategory.Forex))
                            lblQuantity.Text = TradingTicketConstants.CAPTION_QUANTITY;
                        lblTargetQuantity.Text = TradingTicketConstants.CAPTION_TARGET_QUANTITY;
                    }
                    else
                    {
                        btnQuantity.Text = ttPresenter.UseQuantityFieldAsNotional ? TradingTicketConstants.LIT_DOLLAR : TradingTicketConstants.LIT_QUANTITY;
                        if (!(ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward || ttPresenter.AssetID == (int)AssetCategory.Forex))
                            lblQuantity.Text = ttPresenter.UseQuantityFieldAsNotional ? TradingTicketConstants.CAPTION_DOLLAR_QUANTITY : TradingTicketConstants.CAPTION_QUANTITY;
                        lblTargetQuantity.Text = ttPresenter.UseQuantityFieldAsNotional ? TradingTicketConstants.CAPTION_DOLLAR_TARGET_QUANTITY : TradingTicketConstants.CAPTION_TARGET_QUANTITY;
                        SetNotionalQuantityMessage();
                    }

                    if (userTradingTicketuserUiPrefs.Broker.HasValue && (ttPresenter.IncomingOrderRequest == null || _isTTSourceDependentOnAnotherUIs))
                    {
                        if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis.ContainsKey(Convert.ToInt16(userTradingTicketuserUiPrefs.Broker)))
                        {
                            cmbCommissionBasis.SelectedIndex = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis[Convert.ToInt16(userTradingTicketuserUiPrefs.Broker)];
                        }

                        if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseSoftCommissionBasis.ContainsKey(Convert.ToInt16(userTradingTicketuserUiPrefs.Broker)))
                        {
                            cmbSoftCommissionBasis.SelectedIndex = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis[Convert.ToInt16(userTradingTicketuserUiPrefs.Broker)];
                        }
                        if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(Convert.ToInt32(userTradingTicketuserUiPrefs.Broker)))
                        {
                            if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbExecutionInstructions.ValueList, TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(userTradingTicketuserUiPrefs.Broker)].ToString()))
                            {
                                cmbExecutionInstructions.Value = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(userTradingTicketuserUiPrefs.Broker)];
                            }
                        }

                        UpdateBrokerCombo(int.Parse(userTradingTicketuserUiPrefs.Broker.ToString()), ChangeType.NoTrade);

                    }
                    else if (companyTradingTicketUiPrefs.Broker.HasValue && ttPresenter.IncomingOrderRequest == null)
                    {
                        if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis.ContainsKey(Convert.ToInt16(companyTradingTicketUiPrefs.Broker)))
                        {
                            cmbCommissionBasis.SelectedIndex = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis[Convert.ToInt16(companyTradingTicketUiPrefs.Broker)];
                        }
                        if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseSoftCommissionBasis.ContainsKey(Convert.ToInt16(companyTradingTicketUiPrefs.Broker)))
                        {
                            cmbSoftCommissionBasis.SelectedIndex = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis[Convert.ToInt16(companyTradingTicketUiPrefs.Broker)];
                        }
                        if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(Convert.ToInt32(companyTradingTicketUiPrefs.Broker)))
                        {
                            if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbExecutionInstructions.ValueList, TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(companyTradingTicketUiPrefs.Broker)].ToString()))
                            {
                                cmbExecutionInstructions.Value = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(companyTradingTicketUiPrefs.Broker)]; ;
                            }
                        }
                        UpdateBrokerCombo(int.Parse(companyTradingTicketUiPrefs.Broker.ToString()), ChangeType.NoTrade);
                    }
                    /* else if (cmbBroker.Value == null && cmbBroker.ValueList != null && cmbBroker.ValueList.ValueListItems.Count > 0)
                     {
                         UpdateComboValue(cmbBroker, cmbBroker.ValueList.ValueListItems[0].DataValue.ToString(), cmbBroker.ValueList.ValueListItems[0].DisplayText, ChangeType.NoTrade);
                     }*/
                    else if (cmbBroker.ValueList.ValueListItems.Count == 0)
                    {
                        cmbBroker.Value = null;
                    }

                    if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue.ContainsKey(Convert.ToInt32(cmbBroker.Value)) && ValueListUtilities.CheckIfValueExistsInValuelist(cmbVenue.ValueList, TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue[Convert.ToInt32(cmbBroker.Value)].ToString()))
                    {
                        UpdateVenueCombo(TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue[Convert.ToInt32(cmbBroker.Value)], ChangeType.NoTrade);
                    }
                    else if (companyTradingTicketUiPrefs.Venue.HasValue && ValueListUtilities.CheckIfValueExistsInValuelist(cmbVenue.ValueList, companyTradingTicketUiPrefs.Venue.ToString()))
                    {
                        UpdateVenueCombo(int.Parse(companyTradingTicketUiPrefs.Venue.ToString()), ChangeType.NoTrade);
                    }
                    else if (cmbVenue.Value == null && cmbVenue.ValueList != null && cmbVenue.ValueList.ValueListItems.Count > 0)
                    {
                        UpdateComboValue(cmbVenue, cmbVenue.ValueList.ValueListItems[0].DataValue.ToString(), cmbVenue.ValueList.ValueListItems[0].DisplayText, ChangeType.NoTrade);
                    }
                    else if (cmbVenue.ValueList.ValueListItems.Count == 0)
                    {
                        cmbVenue.Value = null;
                    }
                    if (!string.IsNullOrEmpty(ttPresenter.OrderRequest.OrderTypeTagValue))
                    {
                        UpdateOrderTypeCombo(TagDatabaseManager.GetInstance.GetOrderTypeIdBasedOnTagValue(ttPresenter.OrderRequest.OrderTypeTagValue), (ChangeType)(ttPresenter.OrderRequest.ChangeType));
                    }
                    else
                    {
                        if (_watchListColumn.Equals("Last"))
                        {
                            _watchListColumn = string.Empty;
                            UpdateOrderTypeCombo(TagDatabaseManager.GetInstance.GetOrderTypeIdBasedOnTagValue(FIXConstants.ORDTYPE_Market), ChangeType.NoTrade);
                        }
                        else if (userTradingTicketuserUiPrefs.OrderType.HasValue)
                        {
                            UpdateOrderTypeCombo(userTradingTicketuserUiPrefs.OrderType.ToString(), ChangeType.NoTrade);
                        }
                        else if (companyTradingTicketUiPrefs.OrderType.HasValue)
                        {
                            UpdateOrderTypeCombo(companyTradingTicketUiPrefs.OrderType.ToString(), ChangeType.NoTrade);
                        }
                    }

                    if (!string.IsNullOrEmpty(ttPresenter.OrderRequest.TIF))
                    {
                        UpdateTIFCombo(TagDatabaseManager.GetInstance.GetTIFIdBasedOnTagValue(ttPresenter.OrderRequest.TIF), ChangeType.NoTrade);
                    }
                    else
                    {
                        if (userTradingTicketuserUiPrefs.TimeInForce.HasValue)
                        {
                            UpdateTIFCombo(userTradingTicketuserUiPrefs.TimeInForce.ToString(), ChangeType.NoTrade);
                        }
                        else if (companyTradingTicketUiPrefs.TimeInForce.HasValue)
                        {
                            UpdateTIFCombo(companyTradingTicketUiPrefs.TimeInForce.ToString(), ChangeType.NoTrade);
                        }
                    }

                    if (!string.IsNullOrEmpty(ttPresenter.OrderRequest.HandlingInstruction))
                    {
                        cmbHandlingInstructions.Value = TagDatabaseManager.GetInstance.GetHandlingInstructionIdBasedOnTagValue(ttPresenter.OrderRequest.HandlingInstruction);
                    }
                    else
                    {
                        if (userTradingTicketuserUiPrefs.HandlingInstruction.HasValue)
                        {
                            if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbHandlingInstructions.ValueList, userTradingTicketuserUiPrefs.HandlingInstruction.ToString()))
                            {
                                cmbHandlingInstructions.Value = userTradingTicketuserUiPrefs.HandlingInstruction;
                            }
                            else
                            {
                                cmbHandlingInstructions.Value = null;
                            }
                        }
                        else if (companyTradingTicketUiPrefs.HandlingInstruction.HasValue)
                        {
                            if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbHandlingInstructions.ValueList, companyTradingTicketUiPrefs.HandlingInstruction.ToString()))
                            {
                                cmbHandlingInstructions.Value = companyTradingTicketUiPrefs.HandlingInstruction;
                            }
                            else
                            {
                                cmbHandlingInstructions.Value = null;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(ttPresenter.OrderRequest.ExecutionInstruction))
                    {
                        cmbExecutionInstructions.Value = TagDatabaseManager.GetInstance.GetExecutionInstructionIdBasedOnTagValue(ttPresenter.OrderRequest.ExecutionInstruction);
                    }
                    else
                    {

                        int? broker = userTradingTicketuserUiPrefs.Broker ?? companyTradingTicketUiPrefs.Broker;
                        if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(Convert.ToInt32(broker)))
                        {
                            if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbExecutionInstructions.ValueList, TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(broker)].ToString()))
                            {
                                cmbExecutionInstructions.Value = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(broker)];
                            }
                            else
                            {
                                cmbExecutionInstructions.Value = null;
                            }
                        }
                        else if (userTradingTicketuserUiPrefs.ExecutionInstruction.HasValue)
                        {
                            if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbExecutionInstructions.ValueList, userTradingTicketuserUiPrefs.ExecutionInstruction.ToString()))
                            {
                                cmbExecutionInstructions.Value = userTradingTicketuserUiPrefs.ExecutionInstruction;
                            }
                            else
                            {
                                cmbExecutionInstructions.Value = null;
                            }
                        }
                        else if (companyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                        {
                            if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbExecutionInstructions.ValueList, companyTradingTicketUiPrefs.ExecutionInstruction.ToString()))
                            {
                                cmbExecutionInstructions.Value = companyTradingTicketUiPrefs.ExecutionInstruction;
                            }
                            else
                            {
                                cmbExecutionInstructions.Value = null;
                            }
                        }
                    }

                    if (ttPresenter.OrderRequest.TradingAccountID != int.MinValue)
                    {
                        UpdateTradingAccountCombo(ttPresenter.OrderRequest.TradingAccountID, (ChangeType)(ttPresenter.OrderRequest.ChangeType));
                    }
                    else
                    {
                        int tradingAccountID = int.MinValue;
                        if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                        {
                            int masterfundId = Convert.ToInt32(cmbFunds.Value);
                            tradingAccountID = CachedDataManager.GetTradingAccountForMasterFund(masterfundId);
                            if (tradingAccountID != -1)
                                UpdateTradingAccountCombo(tradingAccountID, ChangeType.NoTrade);
                        }
                        if (tradingAccountID == -1 || !CachedDataManager.GetInstance.IsShowMasterFundonTT())
                        {
                            if (int.TryParse(userTradingTicketuserUiPrefs.TradingAccount.ToString(), out tradingAccountID))
                            {
                                UpdateTradingAccountCombo(tradingAccountID, ChangeType.NoTrade);
                            }
                            else if (int.TryParse(companyTradingTicketUiPrefs.TradingAccount.ToString(), out tradingAccountID))
                            {
                                UpdateTradingAccountCombo(tradingAccountID, ChangeType.NoTrade);
                            }
                        }
                    }

                    if (ttPresenter.OrderRequest.Quantity != double.MinValue && ttPresenter.OrderRequest.Quantity != 0)
                    {
                        nmrcQuantity.Value = Convert.ToDecimal(ttPresenter.OrderRequest.Quantity);
                    }
                    else
                    {
                        decimal quantity;
                        if (Decimal.TryParse(userTradingTicketuserUiPrefs.Quantity.ToString(), out quantity))
                        {
                            nmrcQuantity.Value = quantity;
                        }
                        else if (Decimal.TryParse(companyTradingTicketUiPrefs.Quantity.ToString(), out quantity))
                        {
                            nmrcQuantity.Value = quantity;
                        }
                    }

                    int strategyID = int.MinValue;
                    if (int.TryParse(userTradingTicketuserUiPrefs.Strategy.ToString(), out strategyID))
                    {
                        if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbStrategy.ValueList, strategyID.ToString()))
                            cmbStrategy.Value = strategyID;
                        else
                            cmbStrategy.Value = null;
                    }
                    else if (int.TryParse(companyTradingTicketUiPrefs.Strategy.ToString(), out strategyID))
                    {
                        if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbStrategy.ValueList, strategyID.ToString()))
                            cmbStrategy.Value = strategyID;
                        else
                            cmbStrategy.Value = null;
                    }

                    Decimal quantityIncrement = Decimal.MinValue;
                    if (Decimal.TryParse(userTradingTicketuserUiPrefs.IncrementOnQty.ToString(), out quantityIncrement))
                    {
                        increment = quantityIncrement;
                    }
                    else if (Decimal.TryParse(companyTradingTicketUiPrefs.IncrementOnQty.ToString(), out quantityIncrement))
                    {
                        increment = quantityIncrement;
                    }

                    bool isUseRoundLot;
                    isUseRoundLot = TradingTktPrefs.UserTradingTicketUiPrefs.IsUseRoundLots;
                    oneSymbolL1Strip.setRoundLotButton(isUseRoundLot, ttPresenter.SecmasterObj.RoundLot);

                    if (ttPresenter.OrderRequest.MsgType == FIXConstants.MSGOrderCancelReplaceRequest || ttPresenter.OrderRequest.MsgType == FIXConstants.MSGOrder)
                    {
                        if (ttPresenter.OrderRequest.MsgType == FIXConstants.MSGOrderCancelReplaceRequest)
                            nmrcTargetQuantity.Value = Convert.ToDecimal(ttPresenter.OrderRequest.CumQty);
                        if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbStrategy.ValueList, ttPresenter.OrderRequest.Level2ID.ToString()))
                            cmbStrategy.Value = ttPresenter.OrderRequest.Level2ID;
                        UpdateAccountComboValue(ttPresenter.OrderRequest.Level1ID, (ChangeType)(ttPresenter.OrderRequest.ChangeType));
                    }
                    else
                    {
                        if (ttPresenter.EnterTargetQuantityInPercentage)
                        {
                            nmrcTargetQuantity.Value = ApplicationConstants.PERCENTAGEVALUE;
                        }
                        else
                        {
                            nmrcTargetQuantity.Value = ttPresenter.IsTargetQuantitySameAsTotalQty ? nmrcQuantity.Value : 0;
                        }
                    }

                    Decimal priceLimitIncrement = Decimal.MinValue;
                    if (Decimal.TryParse(userTradingTicketuserUiPrefs.IncrementOnLimit.ToString(), out priceLimitIncrement))
                    {
                        nmrcLimit.Increment = priceLimitIncrement;
                        nmrcPrice.Increment = priceLimitIncrement;
                    }
                    else if (Decimal.TryParse(companyTradingTicketUiPrefs.IncrementOnLimit.ToString(), out priceLimitIncrement))
                    {
                        nmrcLimit.Increment = priceLimitIncrement;
                        nmrcPrice.Increment = priceLimitIncrement;
                    }

                    Decimal stopPriceIncrement = Decimal.MinValue;
                    if (Decimal.TryParse(userTradingTicketuserUiPrefs.IncrementOnStop.ToString(), out stopPriceIncrement))
                    {
                        nmrcStop.Increment = stopPriceIncrement;
                        nmrcStop.Increment = stopPriceIncrement;
                        nmrcSettlementPrice.Increment = stopPriceIncrement;
                    }
                    else if (Decimal.TryParse(companyTradingTicketUiPrefs.IncrementOnStop.ToString(), out stopPriceIncrement))
                    {
                        nmrcStop.Increment = stopPriceIncrement;
                        nmrcStop.Increment = stopPriceIncrement;
                        nmrcSettlementPrice.Increment = stopPriceIncrement;
                    }

                    if (userTradingTicketuserUiPrefs.IsSettlementCurrencyBase.HasValue)
                    {
                        cmbSettlementCurrency.Value = (bool)userTradingTicketuserUiPrefs.IsSettlementCurrencyBase ? ttPresenter.CompanyBaseCurrencyID : ttPresenter.CurrencyId;
                    }
                    else if (companyTradingTicketUiPrefs.IsSettlementCurrencyBase.HasValue)
                    {
                        cmbSettlementCurrency.Value = (bool)companyTradingTicketUiPrefs.IsSettlementCurrencyBase ? ttPresenter.CompanyBaseCurrencyID : ttPresenter.CurrencyId;
                    }
                }
                txtNotes.Text = TradingTktPrefs.TTGeneralPrefs.DefaultInternalComments;
                txtBrokerNotes.Text = TradingTktPrefs.TTGeneralPrefs.DefaultBrokerComments;
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
        /// Updates the UI by showing/hiding btnPadlock, btnBrokerConnectionStatus, btnMultiBrokerConnectionStatus according to custodian broker preference.
        /// </summary>
        private void UpdateUIAsPerCustodianBrokerPref()
        {
            try
            {
                btnPadlock.Visible = true;
                btnPadlock.Appearance.Image = _isUseCustodianAsExecutingBroker ? global::Prana.TradingTicket.Properties.Resources.lock_closed : global::Prana.TradingTicket.Properties.Resources.lock_open;
                cmbBroker.Enabled = !_isUseCustodianAsExecutingBroker;
                cmbBroker.Value = null;
                cmbBroker.NullText = _isUseCustodianAsExecutingBroker ? "Default Broker(s)" : "Select Broker";
                btnMultiBrokerConnectionStatus.Visible = _isUseCustodianAsExecutingBroker;
                btnBrokerConnectionStatus.Visible = !btnMultiBrokerConnectionStatus.Visible;
                btnMultiBrokerConnectionStatus.Enabled = btnPadlock.Enabled;
                cmbVenue.Enabled = !_isUseCustodianAsExecutingBroker;
                if (!_isUseCustodianAsExecutingBroker)
                {
                    if (TradingTktPrefs.UserTradingTicketUiPrefs.Broker.HasValue)
                    {
                        UpdateBrokerCombo(int.Parse(TradingTktPrefs.UserTradingTicketUiPrefs.Broker.ToString()), ChangeType.NoTrade);
                    }
                    else if (TradingTktPrefs.CompanyTradingTicketUiPrefs.Broker.HasValue)
                    {
                        UpdateBrokerCombo(int.Parse(TradingTktPrefs.CompanyTradingTicketUiPrefs.Broker.ToString()), ChangeType.NoTrade);
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(TradingTicketConstants.LIT_VALUE);
                    dt.Columns.Add(TradingTicketConstants.LIT_DISPLAY);
                    dt.Rows.Add("1", "Drops");
                    FillVenueCombo(dt);
                    cmbVenue.Text = "Drops";
                    _accountBrokerMapping = ttPresenter.GetAccountBrokerMappingForSelectedFund(Convert.ToInt32(cmbAllocation.Value), cmbBroker.ValueList);
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
        /// Get Tag Value On Asset For Ask click
        /// </summary>
        /// <returns></returns>
        private string GetTagValueOnAssetForAsk()
        {
            var tagValue = FIXConstants.SIDE_Buy;

            try
            {
                AssetCategory assetCategory = Mapper.GetBaseAssetCategory((AssetCategory)ttPresenter.AssetID);
                switch (assetCategory)
                {

                    case AssetCategory.Option:
                        tagValue = FIXConstants.SIDE_Buy_Open;
                        break;
                    case AssetCategory.Future:
                        tagValue = FIXConstants.SIDE_Buy;
                        break;
                    case AssetCategory.FX:
                        break;

                }
            }
            catch (Exception)
            {

                throw;
            }

            return tagValue;
        }

        /// <summary>
        /// Get Tag Value On Asset For Bid click
        /// </summary>
        /// <returns></returns>
        private string GetTagValueOnAssetForBid()
        {
            var tagValue = FIXConstants.SIDE_SellShort;

            try
            {
                AssetCategory assetCategory = Mapper.GetBaseAssetCategory((AssetCategory)ttPresenter.AssetID);
                switch (assetCategory)
                {

                    case AssetCategory.Option:
                        tagValue = FIXConstants.SIDE_Sell_Open;
                        break;
                    case AssetCategory.Future:
                        tagValue = FIXConstants.SIDE_Sell;
                        break;
                    case AssetCategory.FX:
                        break;

                }
            }
            catch (Exception)
            {

                throw;
            }

            return tagValue;
        }

        private void SetPreferredOrderSide(TradingTicketUIPrefs userTradingTicketuserUiPrefs, TradingTicketUIPrefs companyTradingTicketUiPrefs)
        {
            try
            {
                if (userTradingTicketuserUiPrefs.DefAssetSides.Count > 0)
                {
                    DefAssetSide userOrderSide = userTradingTicketuserUiPrefs.DefAssetSides.First(DefAssetSide => DefAssetSide.Asset == ttPresenter.AssetID);
                    if (userOrderSide.OrderSide != null)
                    {
                        cmbOrderSide.Value = ValueListUtilities.CheckIfValueExistsInValuelist(cmbOrderSide.ValueList, userOrderSide.OrderSide.ToString()) ? userOrderSide.OrderSide : null;
                    }
                    else
                    {
                        DefAssetSide companyOrderSide = companyTradingTicketUiPrefs.DefAssetSides.First(DefAssetSide => DefAssetSide.Asset == ttPresenter.AssetID);
                        if (companyOrderSide.OrderSide != null)
                        {
                            cmbOrderSide.Value = ValueListUtilities.CheckIfValueExistsInValuelist(cmbOrderSide.ValueList, companyOrderSide.OrderSide.ToString()) ? companyOrderSide.OrderSide : null;
                        }
                        else
                        {
                            cmbOrderSide.Value = null;
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

        /// <summary>
        /// Sets the trade attribute labels.
        /// </summary>
        /// <param name="lblTA1">The label t a1.</param>
        /// <param name="lblTA2">The label t a2.</param>
        /// <param name="lblTA3">The label t a3.</param>
        /// <param name="lblTA4">The label t a4.</param>
        /// <param name="lblTA5">The label t a5.</param>
        /// <param name="lblTA6">The label t a6.</param>
        public void SetTradeAttributeLabels(string lblTA1, string lblTA2, string lblTA3, string lblTA4, string lblTA5, string lblTA6)
        {
            try
            {
                lblTradeAttribute1.Text = lblTA1;
                lblTradeAttribute2.Text = lblTA2;
                lblTradeAttribute3.Text = lblTA3;
                lblTradeAttribute4.Text = lblTA4;
                lblTradeAttribute5.Text = lblTA5;
                lblTradeAttribute6.Text = lblTA6;
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
        /// Sets the trade attribute value list.
        /// </summary>
        /// <param name="vls">The VLS.</param>
        public void SetTradeAttributeValueList(BindableValueList[] vls)
        {
            try
            {
                if (vls != null)
                {
                    vls[0].BindingContextControl = cmbTradeAttribute1;
                    vls[1].BindingContextControl = cmbTradeAttribute2;
                    vls[2].BindingContextControl = cmbTradeAttribute3;
                    vls[3].BindingContextControl = cmbTradeAttribute4;
                    vls[4].BindingContextControl = cmbTradeAttribute5;
                    vls[5].BindingContextControl = cmbTradeAttribute6;
                    cmbTradeAttribute1.ValueList = vls[0];
                    cmbTradeAttribute2.ValueList = vls[1];
                    cmbTradeAttribute3.ValueList = vls[2];
                    cmbTradeAttribute4.ValueList = vls[3];
                    cmbTradeAttribute5.ValueList = vls[4];
                    cmbTradeAttribute6.ValueList = vls[5];
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

        private double GetTargetQuantityInNumericOrPercentage(double targetQuantity, double quantityValue)
        {
            double returnValue = targetQuantity;
            try
            {
                if (ttPresenter.EnterTargetQuantityInPercentage)
                {
                    if (quantityValue > 0)
                        returnValue = targetQuantity / quantityValue * ApplicationConstants.PERCENTAGEVALUE;
                    else
                        returnValue = 0;
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

            return returnValue;
        }

        /// <summary>
        /// Stops the live feed.
        /// </summary>
        public void StopLiveFeed()
        {
            try
            {
                oneSymbolL1Strip.StopMarketData();
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
        /// Tradings the form setting.
        /// </summary>
        /// <param name="or">The or.</param>
        /// <param name="allocationPrefID">The allocation preference identifier.</param>
        public void SetTradingTicketFromNirvanaMain(OrderSingle or, int allocationPrefID = 0, Dictionary<int, double> accountWithPostions = null)
        {
            try
            {
                ttPresenter.AllocationPrefId = allocationPrefID;
                if (!String.IsNullOrEmpty(or.Symbol))
                {
                    ttPresenter.SetIncomingOrderRequest(or);
                    if (accountWithPostions != null)
                        ttPresenter.AccountWithPMPostionsForCustomPref = accountWithPostions;
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
        /// Tradings the form setting.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="watchlistColumn">The watchlist column.</param>
        public void TradingFormSetting(String symbol, string watchlistColumn)
        {
            try
            {
                ttPresenter.Symbol = symbol;
                pranaSymbolCtrl.Text = symbol;
                CallSymbolControlTextEnteredEvent(symbol);
                _watchListColumn = watchlistColumn;
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
        /// Getting preferred Symbol based on selected Symbolbology from Ticker
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public string GetPreferredSymbolFromTicker(String symbol)
        {
            string convertedSymbol = string.Empty;
            try
            {
                if (SymbologyHelper.DefaultSymbology != ApplicationConstants.SymbologyCodes.TickerSymbol)
                {
                    Dictionary<string, SecMasterBaseObj> symbolObjList = _secMasterSyncService.InnerChannel.GetSecMasterSymbolData(new List<string>() { symbol }, ApplicationConstants.SymbologyCodes.TickerSymbol);
                    if (symbolObjList != null && symbolObjList.ContainsKey(symbol))
                    {
                        switch (SymbologyHelper.DefaultSymbology)
                        {
                            case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                                convertedSymbol = symbolObjList[symbol].BloombergSymbol.ToUpper();
                                break;
                            case ApplicationConstants.SymbologyCodes.FactSetSymbol:
                                convertedSymbol = symbolObjList[symbol].FactSetSymbol.ToUpper();
                                break;
                            case ApplicationConstants.SymbologyCodes.ActivSymbol:
                                convertedSymbol = symbolObjList[symbol].ActivSymbol.ToUpper();
                                break;
                            default:
                                convertedSymbol = symbolObjList[symbol].TickerSymbol.ToUpper();
                                break;
                        }
                    }
                }
                else
                {
                    convertedSymbol = symbol.ToUpper();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return convertedSymbol;
        }

        /// <summary>
        /// Trading ticket enabled or not.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void TradingTicketEnabled(bool value)
        {
            try
            {
                tblPnlTTMainControls.Enabled = value;
                btnBrokerConnectionStatus.Visible = value;
                btnMultiBrokerConnectionStatus.Visible = false;
                btnPadlock.Visible = false;
                cmbBroker.NullText = "Select Broker";
                chkBoxSwap.Enabled = value;
                lblErrorMessage.Text = String.Empty;
                cmbFunds.Enabled = true;
                chkBoxCFD.Enabled = value;
                chkBoxConvertiableBond.Enabled = value;
                chkBoxEquitySwap.Enabled = value;
                btnOTCControl.Enabled = value;
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
        /// Updates the account combo value.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="changeType">Type of the change.</param>
        public void UpdateAccountComboValue(int id, ChangeType changeType, string accountIdsList = "")
        {
            try
            {
                if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbAllocation.ValueList, id.ToString()))
                    UpdateComboValue(cmbAllocation, id.ToString(), String.Empty, changeType);
                else if (!ttPresenter.AddCustomPreferenceToAccountCombo(id, accountIdsList))
                {
                    cmbAllocation.Value = null;
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
        /// Updates the caption.
        /// </summary>
        public void UpdateCaption()
        {
            try
            {
                UpdateCaptionDelegate mi = UpdateCaption;
                if (UIValidation.GetInstance().validate(lblOrderType))
                {
                    if (lblOrderType.InvokeRequired)
                    {
                        BeginInvoke(mi, null);
                    }
                    else
                    {
                        string w = ttPresenter.NotionalValue.ToString("c");
                        string companyName = ttPresenter.SecmasterObj != null ? ttPresenter.SecmasterObj.LongName : string.Empty;

                        if (ttPresenter.NotionalValue > 0)
                            Text = companyName + TradingTicketConstants.MSG_NOTIONAL + w.Substring(1);
                        else
                            Text = companyName;

                        if (ttPresenter.OrderRequest == null)
                        {
                            nmrcFXRate.Value = 0.0m;
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
        /// Updates the drop down.
        /// </summary>
        /// <param name="startWith">The start with.</param>
        /// <param name="items">The items.</param>
        public void UpdateDropDown(string startWith, IList<string> items)
        {
            try
            {
                pranaSymbolCtrl.updateDropDown(startWith, items);
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
        /// Updates the symbol position and expose.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="exposure">The exposure.</param>
        public void UpdateSymbolPositionExposeAndPNL(double position, double exposure, double pnl)
        {
            try
            {
                if (UIValidation.GetInstance().validate(pranaSymbolCtrl))
                {
                    oneSymbolL1Strip.updatePositionAndExposure(position, exposure);
                    if (chkBoxSwap.Checked)
                    {
                        CheckBoxOptionChecked();
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
        /// Wires the side change event.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void WireSideChangeEvent(bool value)
        {
            try
            {
                if (value)
                    cmbOrderSide.ValueChanged += cmbOrderSide_ValueChanged;
                else
                    cmbOrderSide.ValueChanged -= cmbOrderSide_ValueChanged;
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
        /// Algoes the strategy changed.
        /// </summary>
        /// <param name="strategyID">The strategy identifier.</param>
        internal void AlgoStrategyChanged(string strategyID, string strategyName)
        {
            try
            {
                ttPresenter.AlgoStrategyId = strategyID;
                ttPresenter.AlgoStrategyName = strategyName;
                if (strategyID != int.MinValue.ToString())
                {
                    algoStrategyControl.Hide();
                    algoStrategyControl.BackColor = this.BackColor;
                    algoStrategyControl.CreateAlgoControls(cmbBroker.Value.ToString(), strategyID, CachedDataManager.GetInstance.GetUnderLyingText(ttPresenter.UnderlyingID), btnSend.Visible, false);


                    if (AlgoStrategyControlProperty != null && cmbOrderSide.Value != null)
                    {
                        AlgoStrategyControlProperty.EnableAlgoControlsBasedTTFields(cmbOrderSide.Value.ToString(), "OrderSideTagValue");
                    }
                    if (AlgoStrategyControlProperty != null && cmbOrderType.Value != null)
                    {
                        AlgoStrategyControlProperty.EnableAlgoControlsBasedTTFields(cmbOrderType.Value.ToString(), "OrderTypeValue");
                    }

                    if (ttPresenter.AuecID != int.MinValue)
                    {
                        OrderSingle order = new OrderSingle { AUECID = ttPresenter.AuecID };
                        algoStrategyControl.SetAlgoDetails(order);
                        if (ttPresenter.OrderRequest.CounterPartyID.ToString() == cmbBroker.Value.ToString())
                        {
                            algoStrategyControl.SetSelectedStrategyFixTagValues(ttPresenter.OrderRequest, null);
                        }
                    }


                }
                else if (strategyID == int.MinValue.ToString())
                {
                    algoStrategyControl.Hide();
                    algoStrategyControl.CustomMessage = string.Empty;

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
        /// Updates the notional value on caption.
        /// </summary>
        internal void UpdateNotionalValueOnCaption()
        {
            try
            {
                double NotionalValue = 0.0;
                if (cmbOrderType.Value == null)
                {
                    return;
                }
                switch (cmbOrderType.Value.ToString())
                {
                    case FIXConstants.ORDTYPE_Limit:
                    case FIXConstants.ORDTYPE_LimitOnClose:
                    case FIXConstants.ORDTYPE_LimitOrBetter:
                    case FIXConstants.ORDTYPE_LimitWithOrWithout:
                        if (ttPresenter.UseQuantityFieldAsNotional)
                        {
                            if (double.Parse(nmrcPrice.Text) != 0)
                            {
                                NotionalValue = Math.Floor(Double.Parse(nmrcQuantity.Text) / Double.Parse(nmrcPrice.Text)) * Double.Parse(nmrcLimit.Text) * ttPresenter.Multiplier;
                            }
                        }
                        else
                        {
                            NotionalValue = Double.Parse(nmrcQuantity.Text) * Double.Parse(nmrcLimit.Text) * ttPresenter.Multiplier;
                        }
                        break;

                    case FIXConstants.ORDTYPE_Stop:
                    case FIXConstants.ORDTYPE_Stoplimit:
                        if (ttPresenter.UseQuantityFieldAsNotional)
                        {
                            if (double.Parse(nmrcPrice.Text) != 0)
                            {
                                NotionalValue = Double.Parse(nmrcStop.Text) * Math.Floor(Double.Parse(nmrcQuantity.Text) / Double.Parse(nmrcPrice.Text)) * ttPresenter.Multiplier;
                            }
                        }
                        else
                        {
                            NotionalValue = Double.Parse(nmrcStop.Text) * Double.Parse(nmrcQuantity.Text) * ttPresenter.Multiplier;
                        }
                        break;

                    case FIXConstants.ORDTYPE_Market:
                    case FIXConstants.ORDTYPE_Pegged:
                        if (ttPresenter.UseQuantityFieldAsNotional)
                        {
                            if (double.Parse(nmrcPrice.Text) != 0)
                            {
                                NotionalValue = ttPresenter.MarketPrice * Math.Floor(Double.Parse(nmrcQuantity.Text) / Double.Parse(nmrcPrice.Text)) * ttPresenter.Multiplier;
                            }
                        }
                        else
                        {
                            NotionalValue = ttPresenter.MarketPrice * Double.Parse(nmrcQuantity.Text) * ttPresenter.Multiplier;
                        }
                        break;

                    default:
                        NotionalValue = 0.0;
                        break;
                }
                oneSymbolL1Strip.UpdateNotionalValue(NotionalValue);
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
        /// Sets the trading ticket.
        /// </summary>
        /// <param name="or">The or.</param>
        /// <param name="allocationPrefID">The allocation preference identifier.</param>
        internal void SetTradingTicket(OrderSingle or, int allocationPrefID = 0)
        {
            try
            {
                if (or != null)
                {
                    TranferTradeRules transfertraderules = CachedDataManager.GetInstance.GetTransferTradeRules();
                    _isTTSourceDependentOnAnotherUIs = true;
                    btnQuantity.Enabled = false;
                    ttPresenter.UseQuantityFieldAsNotional = false;
                    btnQuantity.Text = ttPresenter.UseQuantityFieldAsNotional ? TradingTicketConstants.LIT_DOLLAR : TradingTicketConstants.LIT_QUANTITY;
                    lblQuantity.Text = ttPresenter.UseQuantityFieldAsNotional ? TradingTicketConstants.CAPTION_DOLLAR_QUANTITY : TradingTicketConstants.CAPTION_QUANTITY;
                    lblTargetQuantity.Text = ttPresenter.UseQuantityFieldAsNotional ? TradingTicketConstants.CAPTION_DOLLAR_TARGET_QUANTITY : TradingTicketConstants.CAPTION_TARGET_QUANTITY;
                    SetNotionalQuantityMessage();

                    cmbBroker.Enabled = true;
                    cmbVenue.Enabled = true;
                    cmbTradingAccount.Enabled = true;
                    cmbOrderSide.Enabled = true;
                    nmrcCommissionRate.Value = Convert.ToDecimal(or.CommissionRate);
                    nmrcSoftRate.Value = Convert.ToDecimal(or.SoftCommissionRate);
                    ttPresenter.TicketType = TradingTicketType.Manual;

                    if (ttPresenter.TradingTicketParentType == TradingTicketParent.ShortLocate)
                        if (or.Account != int.MinValue.ToString())
                            cmbAllocation.Value = or.Account;

                    SetControlsVisibilityBasedOnMessageType(or, transfertraderules);

                    SetIndexOfCommissionComboBox(or);

                    if (!string.IsNullOrEmpty(or.OrderTypeTagValue))
                    {
                        UpdateOrderTypeCombo(TagDatabaseManager.GetInstance.GetOrderTypeIdBasedOnTagValue(or.OrderTypeTagValue), (ChangeType)(or.ChangeType));
                    }
                    else
                    {
                        cmbOrderType.Value = null;
                    }

                    if (or.TradingAccountID != int.MinValue)
                    {
                        UpdateTradingAccountCombo(or.TradingAccountID, (ChangeType)(or.ChangeType));
                    }
                    else
                    {
                        cmbTradingAccount.Value = null;
                    }

                    if (or.MsgType == FIXConstants.MSGOrder && or.Quantity > 0)
                    {
                        Max_Qty = Convert.ToDecimal(or.UnsentQty == double.MinValue ? or.Quantity : or.UnsentQty);
                    }
                    SetControlsVisibilityBasedOnCancelOrReplaceMessage(or, transfertraderules);

                    if (or.MsgType == FIXConstants.MSGOrderCancelReplaceRequest && or.Account == OrderFields.PROPERTY_MULTIPLE)
                    {
                        bool isManualPref = or.AllocationSchemeName.Contains(TradingTicketConstants.ALLOCATION_PREF_CUSTOM);
                        if (!isManualPref && or.AllocationSchemeName.Equals(TradingTicketType.Manual.ToString()))
                            AddMultipleAcountToAccountCombo();
                    }

                    SetControlsVisibilityBasedOnPranaMessageType(or, transfertraderules);

                    txtBrokerNotes.Text = or.Text;
                    if (or.Price != double.Epsilon)
                    {
                        nmrcLimit.Value = Convert.ToDecimal(or.Price);
                        // Added this to show the value Avg Price in Replace window of TT instead of showing 0,	PRANA-11002
                    }
                    if (or.AvgPrice != double.Epsilon)
                        nmrcPrice.Value = Convert.ToDecimal(or.AvgPrice);
                    if (or.StopPrice != double.Epsilon)
                    {
                        nmrcStop.Value = Convert.ToDecimal(or.StopPrice);
                    }
                    if (or.StopPrice != double.Epsilon)
                    {
                        //txtDisplay.Value = or.StopPrice;
                    }
                    if (!string.IsNullOrEmpty(or.HandlingInstruction))
                    {
                        if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbHandlingInstructions.ValueList, TagDatabaseManager.GetInstance.GetHandlingInstructionIdBasedOnTagValue(or.HandlingInstruction)))
                        {
                            cmbHandlingInstructions.Value = TagDatabaseManager.GetInstance.GetHandlingInstructionIdBasedOnTagValue(or.HandlingInstruction);
                        }
                        else
                            cmbHandlingInstructions.Value = null;
                    }
                    else
                    {
                        cmbHandlingInstructions.Value = null;
                    }
                    if (!string.IsNullOrEmpty(or.ExecutionInstruction))
                    {
                        if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbExecutionInstructions.ValueList, TagDatabaseManager.GetInstance.GetExecutionInstructionIdBasedOnTagValue(or.ExecutionInstruction)))
                        {
                            cmbExecutionInstructions.Value = TagDatabaseManager.GetInstance.GetExecutionInstructionIdBasedOnTagValue(or.ExecutionInstruction);
                        }
                        else
                            cmbExecutionInstructions.Value = null;
                    }
                    else
                    {
                        cmbExecutionInstructions.Value = null;
                    }

                    if (!string.IsNullOrEmpty(or.TIF))
                    {
                        UpdateTIFCombo(TagDatabaseManager.GetInstance.GetTIFIdBasedOnTagValue(or.TIF), (ChangeType)(or.ChangeType));
                    }
                    else
                    {
                        cmbTIF.Value = null;
                    }

                    if (or.TIF == FIXConstants.TIF_GTD && or.ExpireTime != null && !or.ExpireTime.Equals("N/A") && !string.IsNullOrEmpty(or.ExpireTime))
                    {
                        DateTime dtValue;
                        if (DateTime.TryParse(or.ExpireTime, out dtValue))
                        {
                            this.dtExpireTime.TextChanged -= DtExpireDate_TextChanged;
                            dtExpireTime.Value = dtValue;
                            this.dtExpireTime.TextChanged += DtExpireDate_TextChanged;
                            string dateString = dtValue.ToString("MM/dd/yyyy");
                            lblExpireTime.Text = dateString;
                        }
                    }

                    if (or.OrderSideTagValue != null && or.OrderSideTagValue.Trim().Length > 0)
                    {
                        SelectComboSide(or.OrderSideTagValue, or.OpenClose);
                    }
                    if ((or.MsgType == FIXConstants.MSGOrderCancelReplaceRequest) || (or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub) || (or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub) || (or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSubChild)
                        || (or.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX) || (or.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew)
                        || (or.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrder))
                    {
                        //Update the property from PranaBasicMessage
                        nmrcFXRate.Value = Convert.ToDecimal(or.FXRate);

                        // Added a condition in check for Manual Sub Order also so that Quantity can be replaced with any quantity less than total and greater than executed, PRANA-11045
                        if (or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub || or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSubChild || or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub)
                        {
                            if (ttPresenter.CheckOrderInDictionary(or.StagedOrderID))
                            {
                                nmrcQuantity.Maximum = 999999999m;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(or.ListID))
                    {
                        cmbOrderSide.Enabled = false;
                        nmrcQuantity.Enabled = false;
                        cmbTradingAccount.Enabled = false;
                        btnCreateOrder.Enabled = false;
                    }

                    cmbTradeAttribute1.Text = or.TradeAttribute1;
                    cmbTradeAttribute2.Text = or.TradeAttribute2;
                    cmbTradeAttribute3.Text = or.TradeAttribute3;
                    cmbTradeAttribute4.Text = or.TradeAttribute4;
                    cmbTradeAttribute5.Text = or.TradeAttribute5;
                    cmbTradeAttribute6.Text = or.TradeAttribute6;

                    if (or.SettlementCurrencyID != double.Epsilon && or.SettlementCurrencyID > 0 && pranaSymbolCtrl.Text == or.Symbol)
                    {
                        cmbSettlementCurrency.Value = or.SettlementCurrencyID;
                    }
                    UpdateAccountCombo(or);

                    ttPresenter.SetOrderRequest(or);
                    UpdateBrokerCombo(or.CounterPartyID, (ChangeType)(or.ChangeType));

                    if (or.VenueID != 0 && (or.VenueID != int.MinValue || or.CounterPartyID == int.MinValue))
                        UpdateVenueCombo(or.VenueID, (ChangeType)(or.ChangeType));

                    txtNotes.Text = or.InternalComments;






                    if (CachedDataManager.GetInstance.IsAlgoBrokerFromID(or.CounterPartyID))
                    {
                        if (or.AlgoStrategyID != String.Empty && or.AlgoStrategyID != int.MinValue.ToString())
                        {
                            strategyControl1.SetStrategyID(or.AlgoStrategyID);

                        }
                    }
                    if (or.MsgType == FIXConstants.MSGOrderSingle && or.SwapParameters != null && !_isNewOTCWorkflowEnabled)
                    {
                        chkBoxSwap.Visible = true;
                        chkBoxSwap.Enabled = true;
                        chkBoxSwap.Checked = true;
                    }
                    if (or != null && !string.IsNullOrEmpty(or.FXConversionMethodOperator) &&
                        (ttPresenter.DefaultSymbology == ApplicationConstants.SymbologyCodes.TickerSymbol && pranaSymbolCtrl.Text.ToUpper() == or.Symbol.ToUpper()
                        || ttPresenter.DefaultSymbology == ApplicationConstants.SymbologyCodes.BloombergSymbol && pranaSymbolCtrl.Text.ToUpper() == or.BloombergSymbol.ToUpper()
                        || ttPresenter.DefaultSymbology == ApplicationConstants.SymbologyCodes.FactSetSymbol && pranaSymbolCtrl.Text.ToUpper() == or.FactSetSymbol.ToUpper()
                        || ttPresenter.DefaultSymbology == ApplicationConstants.SymbologyCodes.ActivSymbol && pranaSymbolCtrl.Text.ToUpper() == or.ActivSymbol.ToUpper()
                        ))
                        cmbFxOperator.Value = or.FXConversionMethodOperator;
                    ttPresenter.IsManualOrder = or.IsManualOrder;
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
        /// Handles the Click event of the btnAccountQty control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnAccountQty_Click(object sender, EventArgs e)
        {
            try
            {
                if (_stagedOrderAllocationView != null)
                {
                    _stagedOrderAllocationView.LoadAllocationData(Quantity);
                    _stagedOrderAllocationView.ShowDialog();
                }
                else
                    ttPresenter.GetAndUpdateAccountWiseQuantity();
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
        /// Handles the Click event of the btnMultiBrokerConnectionStatus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnMultiBrokerConnectionStatus_Click(object sender, EventArgs e)
        {
            try
            {
                BrokersConnectionStatus brokersConnectionStatus = new BrokersConnectionStatus(_accountBrokerMapping, cmbBroker.ValueList);
                brokersConnectionStatus.ShowDialog();
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

        /// Handles the Click event of the btnPadlock control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnPadlock_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbAllocation.ValueList, cmbAllocation.Value.ToString()))
                {
                    if (ttPresenter.IsAllocationValidForCustodianBrokerPref(Convert.ToInt32(cmbAllocation.Value)))
                    {
                        _isUseCustodianAsExecutingBroker = !_isUseCustodianAsExecutingBroker;
                        UpdateUIAsPerCustodianBrokerPref();
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
        /// Handles the Click event of the btnCreateOrder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime starttime = DateTime.Now;
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("Time- " + DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss:ffff tt") + " Step Start Region btnCreateOrder_Click.", LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                }

                ttPresenter.IsTradeSending = true;
                btnDoneAway.Enabled = false;
                btnSend.Enabled = false;
                btnCreateOrder.Enabled = false;
                if (btnShortLocateList.Visible)
                {
                    ttPresenter.OrderRequest.ShortLocateParameter = shortLocateList1.GetShortLocateParameter();
                    if (ttPresenter.OrderRequest.ShortLocateParameter != null)
                    {
                        ttPresenter.OrderRequest.LocateReqd = true;
                        ttPresenter.OrderRequest.BorrowerID = ttPresenter.OrderRequest.ShortLocateParameter.BorrowerId;
                        ttPresenter.OrderRequest.ShortRebate = ttPresenter.OrderRequest.ShortLocateParameter.BorrowRate;
                        ttPresenter.OrderRequest.BorrowerBroker = ttPresenter.OrderRequest.ShortLocateParameter.Broker;
                        btnShortLocateList.Enabled = false;
                        ttPresenter.OrderRequest.CumQty = 0.0;
                    }
                    else
                        btnShortLocateList.Enabled = false;
                }
                if ((!(ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward)) && !ttPresenter.UseQuantityFieldAsNotional)
                {
                    SetLabelMessage(String.Empty);
                }

                if (!ValidateControlsBeforeTrade())
                {
                    SetLabelMessage(TradingTicketConstants.MSG_SOME_VALUE_ARE_INVALID);
                    return;
                }

                ttPresenter.TicketType = TradingTicketType.Stage;
                ttPresenter.CreateStageAndLiveOrder();
                if (!ttPresenter.IsTradeSuccessful)
                {
                    btnCreateOrder.Enabled = true;
                    if (IsMultiDayOrder())
                    {
                        btnDoneAway.Enabled = false;
                    }
                    else
                        btnDoneAway.Enabled = true;
                    btnSend.Enabled = true;
                    if (btnShortLocateList.Visible == true)
                        btnShortLocateList.Enabled = true;
                    if (ComplianceCommon.IsDisableRequireOnTT)
                    {
                        ResetTicket();
                        ComplianceCommon.IsDisableRequireOnTT = false;
                    }
                }
                else
                {
                    if (shortLocateList1.grdShortLocateList.DataSource != null)
                        shortLocateList1.grdShortLocateList.DataSource = null;
                    bottomTabControl.Tabs[TradingTicketConstants.TAB_BorrowParameter_Key].Visible = false;
                }

                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        TimeSpan varTime = (DateTime)starttime - (DateTime)DateTime.Now;
                        Logger.LoggerWrite("Time- " + DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss:ffff tt") + " Step End Region btnCreateOrder_Click Total Time Taken in seconds- " + varTime.TotalSeconds, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
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
            finally
            {
                ttPresenter.IsTradeSending = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDoneAway control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnDoneAway_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime starttime = DateTime.Now;
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("Time- " + DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss:ffff tt") + " Step Start Region btnDoneAway_Click.", LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                }
                ttPresenter.IsTradeSending = true;

                if (dtTradeDate.Value == null)
                    dtTradeDate.Value = DateTime.Now;

                DateTime date = (DateTime)dtTradeDate.Value;
                if (!CachedDataManager.GetInstance.ValidateNAVLockDate(date))
                {
                    MessageBox.Show("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                        + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (btnShortLocateList.Visible)
                {
                    ttPresenter.OrderRequest.ShortLocateParameter = shortLocateList1.GetShortLocateParameter();
                    if (ttPresenter.OrderRequest.ShortLocateParameter != null)
                    {
                        if (ttPresenter.OrderRequest.ShortLocateParameter.BorrowQuantity > ttPresenter.OrderRequest.ShortLocateParameter.BorrowSharesAvailable)
                        {
                            string msg = (ttPresenter.OrderRequest.ShortLocateParameter.Broker + " only available borrow quanity of " + ttPresenter.OrderRequest.ShortLocateParameter.BorrowSharesAvailable + " but request is to trade " + ttPresenter.OrderRequest.ShortLocateParameter.BorrowQuantity + ". Do you want to proceed?");
                            Prana.TradeManager.PromptWindow promptWin = new Prana.TradeManager.PromptWindow(msg, "NIRVANA");
                            promptWin.ShowDialog();
                            if (!promptWin.ShouldTrade)
                            {
                                promptWin.Close();
                                return;
                            }
                            promptWin.Close();
                        }
                        ttPresenter.OrderRequest.LocateReqd = true;
                        ttPresenter.OrderRequest.BorrowerID = ttPresenter.OrderRequest.ShortLocateParameter.BorrowerId;
                        ttPresenter.OrderRequest.ShortRebate = ttPresenter.OrderRequest.ShortLocateParameter.BorrowRate;
                        ttPresenter.OrderRequest.BorrowerBroker = ttPresenter.OrderRequest.ShortLocateParameter.Broker;
                        ttPresenter.OrderRequest.CumQty = 0.0;
                        btnShortLocateList.Enabled = false;
                    }
                    else
                        btnShortLocateList.Enabled = false;
                }

                btnDoneAway.Enabled = false;
                btnSend.Enabled = false;
                btnCreateOrder.Enabled = false;
                if ((!(ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward)) && !ttPresenter.UseQuantityFieldAsNotional)
                {
                    SetLabelMessage(String.Empty);
                }

                if (!ValidateControlsBeforeTrade())
                {
                    SetLabelMessage(TradingTicketConstants.MSG_SOME_VALUE_ARE_INVALID);
                    return;
                }

                ttPresenter.TicketType = TradingTicketType.Manual;
                if (cmbAllocation.DataSource != null)
                {
                    if (cmbAllocation.Value == null)
                    {
                        if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbAllocation.ValueList, int.MinValue.ToString()))
                        {
                            cmbAllocation.Value = int.MinValue;
                        }
                    }
                    ttPresenter.CreateNewManualOrderOrUpdateExistingOrder((DateTime)dtTradeDate.Value);
                    if (!ttPresenter.IsTradeSucceeded)
                    {
                        if (pranaSymbolCtrl.Enabled)
                            btnCreateOrder.Enabled = true;
                        btnDoneAway.Enabled = true;
                        btnSend.Enabled = true;
                        if (btnShortLocateList.Visible == true)
                            btnShortLocateList.Enabled = true;
                    }
                    else
                    {
                        otcTradeViewUI = null;
                        _otcParameters = null;
                        bottomTabControl.Tabs[TradingTicketConstants.TAB_BorrowParameter_Key].Visible = false;
                        if (shortLocateList1.grdShortLocateList.DataSource != null)
                            shortLocateList1.grdShortLocateList.DataSource = null;
                    }
                }


                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        TimeSpan varTime = (DateTime)starttime - (DateTime)DateTime.Now;
                        Logger.LoggerWrite("Time- " + DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss:ffff tt") + " Step End Region btnDoneAway_Click Total Time Taken in seconds- " + varTime.TotalSeconds, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
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
            finally
            {
                ttPresenter.IsTradeSending = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime starttime = DateTime.Now;
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("Time- " + DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss:ffff  tt") + " Step Start Region btnSend_Click.", LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                }

                ttPresenter.IsTradeSending = true;
                if (btnShortLocateList.Visible)
                {
                    ttPresenter.OrderRequest.ShortLocateParameter = shortLocateList1.GetShortLocateParameter();
                    if (ttPresenter.OrderRequest.ShortLocateParameter != null)
                    {
                        if (ttPresenter.OrderRequest.ShortLocateParameter.BorrowQuantity > ttPresenter.OrderRequest.ShortLocateParameter.BorrowSharesAvailable)
                        {
                            string msg = (ttPresenter.OrderRequest.ShortLocateParameter.Broker + " only available borrow quanity of " + ttPresenter.OrderRequest.ShortLocateParameter.BorrowSharesAvailable + " but request is to trade " + ttPresenter.OrderRequest.ShortLocateParameter.BorrowQuantity + ". Do you want to proceed?");
                            Prana.TradeManager.PromptWindow promptWin = new Prana.TradeManager.PromptWindow(msg, "NIRVANA");
                            promptWin.ShowDialog();
                            if (!promptWin.ShouldTrade)
                            {
                                promptWin.Close();
                                return;
                            }
                            promptWin.Close();
                        }
                        ttPresenter.OrderRequest.LocateReqd = true;
                        ttPresenter.OrderRequest.BorrowerID = ttPresenter.OrderRequest.ShortLocateParameter.BorrowerId;
                        ttPresenter.OrderRequest.ShortRebate = ttPresenter.OrderRequest.ShortLocateParameter.BorrowRate;
                        ttPresenter.OrderRequest.BorrowerBroker = ttPresenter.OrderRequest.ShortLocateParameter.Broker;
                        ttPresenter.OrderRequest.CumQty = 0.0;
                        btnShortLocateList.Enabled = false;
                    }
                    else
                        btnShortLocateList.Enabled = false;
                }
                btnDoneAway.Enabled = false;
                btnSend.Enabled = false;
                btnCreateOrder.Enabled = false;
                if ((!(ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward)) && !ttPresenter.UseQuantityFieldAsNotional)
                {
                    SetLabelMessage(String.Empty);
                }

                if (!ValidateControlsBeforeTrade())
                {
                    SetLabelMessage(TradingTicketConstants.MSG_SOME_VALUE_ARE_INVALID);
                    return;
                }
                ttPresenter.TicketType = TradingTicketType.Live;
                if (dtTradeDate.Value == null)
                    dtTradeDate.Value = DateTime.Now;
                if ((ttPresenter.IncomingOrderRequest == null) || (ttPresenter.TradingTicketParentType == TradingTicketParent.PTT) || (ttPresenter.TradingTicketParentType == TradingTicketParent.PM))
                {
                    ttPresenter.CreateStageAndLiveOrder();
                }
                else
                {
                    if (dtTradeDate.Value == null)
                        dtTradeDate.Value = DateTime.Now;
                    ttPresenter.CreateNewManualOrderOrUpdateExistingOrder((DateTime)dtTradeDate.Value);
                }
                if (!ttPresenter.IsTradeSuccessful || !ttPresenter.IsTradeSucceeded)
                {
                    if (pranaSymbolCtrl.Enabled)
                        btnCreateOrder.Enabled = true;
                    if (IsMultiDayOrder())
                    {
                        btnDoneAway.Enabled = false;
                    }
                    else
                        btnDoneAway.Enabled = true;
                    btnSend.Enabled = true;
                    if (btnShortLocateList.Visible == true)
                        btnShortLocateList.Enabled = true;
                    else
                        bottomTabControl.Tabs[TradingTicketConstants.TAB_BorrowParameter_Key].Visible = false;
                }
                else
                {
                    btnCreateOrder.Enabled = false;
                    otcTradeViewUI = null;
                    _otcParameters = null;
                    bottomTabControl.Tabs[TradingTicketConstants.TAB_BorrowParameter_Key].Visible = false;
                }

                if (ttPresenter.IsTradeSuccessful)
                {
                    if (shortLocateList1.grdShortLocateList.DataSource != null)
                        shortLocateList1.grdShortLocateList.DataSource = null;
                }

                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        TimeSpan varTime = (DateTime)starttime - (DateTime)DateTime.Now;
                        Logger.LoggerWrite("Time- " + DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss:ffff tt") + " Step End Region btnSend_Click Total Time Taken in seconds- " + varTime.TotalSeconds, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
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
            finally
            {
                ttPresenter.IsTradeSending = false;
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {

            try
            {
                DateTime starttime = DateTime.Now;
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("Time- " + DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss:ffff tt") + " Step Start Region btnReplace_Click.", LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                }

                if ((!(ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward)) && !ttPresenter.UseQuantityFieldAsNotional)
                {
                    SetLabelMessage(String.Empty);
                }

                if (!ValidateControlsBeforeTrade())
                {
                    SetLabelMessage(TradingTicketConstants.MSG_SOME_VALUE_ARE_INVALID);
                    return;
                }

                if (ttPresenter.IncomingOrderRequest != null && ttPresenter.IncomingOrderRequest.MsgType == FIXConstants.MSGOrderCancelReplaceRequest
                    && (OrderFields.PranaMsgTypes)ttPresenter.IncomingOrderRequest.PranaMsgType == OrderFields.PranaMsgTypes.ORDStaged)
                {
                    ttPresenter.TicketType = TradingTicketType.Stage;
                }
                else if (ttPresenter.IncomingOrderRequest != null && (ttPresenter.IncomingOrderRequest.MsgType == FIXConstants.MSGOrderCancelReplaceRequest &&
                    (OrderFields.PranaMsgTypes)ttPresenter.IncomingOrderRequest.PranaMsgType == OrderFields.PranaMsgTypes.ORDNewSub || (OrderFields.PranaMsgTypes)ttPresenter.IncomingOrderRequest.PranaMsgType == OrderFields.PranaMsgTypes.ORDNewSubChild))
                {
                    ttPresenter.TicketType = TradingTicketType.Live;
                }
                else
                {
                    ttPresenter.TicketType = TradingTicketType.Manual;
                }

                if (cmbAllocation.DataSource != null)
                {
                    if (cmbAllocation.Value == null)
                    {
                        if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbAllocation.ValueList, int.MinValue.ToString()))
                        {
                            cmbAllocation.Value = int.MinValue;
                        }
                    }
                    if (dtTradeDate.Value == null)
                        dtTradeDate.Value = DateTime.Now;

                    String originalValues = " Original Quantity:" + ttPresenter.IncomingOrderRequest.Quantity + ", Avg Price:" + ttPresenter.IncomingOrderRequest.AvgPrice + ", Order Type:" + ttPresenter.IncomingOrderRequest.OrderType + ",Trade Date:" + ttPresenter.IncomingOrderRequest.AUECLocalDate;

                    ttPresenter.CreateNewManualOrderOrUpdateExistingOrder((DateTime)dtTradeDate.Value);


                    if (ttPresenter.IsTradeSucceeded)
                    {
                        String newValues = "Replaced Quantity:" + ttPresenter.IncomingOrderRequest.Quantity + ", Avg Price:" + ttPresenter.IncomingOrderRequest.AvgPrice + ", Order Type:" + ttPresenter.IncomingOrderRequest.OrderType + ",Trade Date:" + ttPresenter.IncomingOrderRequest.AUECLocalDate;
                        TradeAuditActionType.ActionType action = string.IsNullOrEmpty(ttPresenter.IncomingOrderRequest.StagedOrderID) ? TradeAuditActionType.ActionType.OrderReplaced : TradeAuditActionType.ActionType.SubOrderReplaced;
                        AddOrderDataAuditEntryAndSaveInDB(ttPresenter.IncomingOrderRequest, action, ttPresenter.IncomingOrderRequest.CompanyUserID, originalValues, newValues);
                    }
                    if (_enableTradeFlowLogging)
                    {
                        try
                        {
                            TimeSpan varTime = (DateTime)starttime - (DateTime)DateTime.Now;
                            Logger.LoggerWrite("Time- " + DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss:ffff tt") + " Step End Region btnReplace_Click Total Time Taken in seconds- " + varTime.TotalSeconds, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information, true);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
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
        /// Add Audit Trail Collection
        /// </summary>
        /// <param name="orRequest"></param>
        /// <param name="action"></param>
        private void AddOrderDataAuditEntryAndSaveInDB(OrderSingle order, TradeAuditActionType.ActionType action, int userId, string originalValues, string newValues)
        {
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    List<TradeAuditEntry> auditCollection = new List<TradeAuditEntry>();
                    TradeAuditEntry audit = new TradeAuditEntry()
                    {
                        Action = action,
                        AUECLocalDate = DateTime.Now,
                        OriginalDate = order.AUECLocalDate,
                        CompanyUserId = userId,
                        GroupID = string.Empty,
                        TaxLotID = string.Empty,
                        ParentClOrderID = order.ParentClOrderID,
                        ClOrderID = order.ClOrderID,
                        Symbol = order.Symbol,
                        Level1ID = order.Level1ID,
                        OrderSideTagValue = order.OrderSideTagValue,
                        OriginalValue = originalValues,
                        Comment = newValues,
                        Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.Trade
                    };
                    auditCollection.Add(audit);
                    AuditManager.Instance.SaveAuditList(auditCollection);
                });

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

        private void cmbAllocation_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbAllocation.ValueList, int.MinValue.ToString()))
                    {
                        if (cmbAllocation.Value == null)
                        {
                            cmbAllocation.Value = int.MinValue;
                        }
                        else if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                        {
                            //Update Brokers list
                            List<int> accountsList = new List<int>();
                            var accountID = Convert.ToInt32(cmbAllocation.Value.ToString());

                            //Check Is Default Allocation Pref selected 
                            if (accountID > 0 && CheckIsDefaultAllocationPref())
                            {
                                ///Update Broker list based on Allocation Pref 
                                ttPresenter.UpdateBrokerListByAllocationPrefFilter();
                            }
                            else
                            {
                                //Update Broker list based on account selection
                                if (accountID > 0)
                                    accountsList.Add(accountID);
                                else
                                {
                                    foreach (var item in cmbAllocation.ValueList.ValueListItems)
                                    {
                                        if (Convert.ToInt32(item.DataValue) > 0)
                                            accountsList.Add(Convert.ToInt32(item.DataValue));
                                    }
                                }
                                ttPresenter.UpdateBrokerListByAccountFilter(accountsList);
                            }

                            if (ttPresenter.TradingTicketUiPrefs.Broker.HasValue)
                                UpdateBrokerCombo(int.Parse(ttPresenter.TradingTicketUiPrefs.Broker.ToString()), ChangeType.NoTrade);
                            else if (ttPresenter.CompanyTradingTicketUiPrefs.Broker.HasValue && cmbBroker.Value == null)
                                UpdateBrokerCombo(int.Parse(ttPresenter.CompanyTradingTicketUiPrefs.Broker.ToString()), ChangeType.NoTrade);

                        }
                        if (TradingTktPrefs.TTGeneralPrefs.IsUseCustodianAsExecutingBroker && _isUpdateBrokerBasedOnCustodianPreference)
                        {
                            _isUseCustodianAsExecutingBroker = btnPadlock.Enabled ? ttPresenter.IsAllocationValidForCustodianBrokerPref(Convert.ToInt32(cmbAllocation.Value)) : ttPresenter.AreAllSelectedAccountsMapped(Convert.ToInt32(cmbAllocation.Value));
                            UpdateUIAsPerCustodianBrokerPref();
                        }
                    }
                }
                else if (btnPadlock.Visible && _isUseCustodianAsExecutingBroker)
                {
                    _isUseCustodianAsExecutingBroker = false;
                    UpdateUIAsPerCustodianBrokerPref();
                }

                SetStrategyComboVisibility();

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
        /// Check Is Default Allocation Pref selected
        /// </summary>
        /// <returns></returns>
        private bool CheckIsDefaultAllocationPref()
        {
            bool isDefaultAllocationPref = false;
            try
            {
                if (cmbAllocation.DataSource != null && cmbAllocation.Value != null)
                {
                    DataTable dt = cmbAllocation.DataSource as DataTable;
                    if (dt != null)
                    {
                        foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => row[OrderFields.PROPERTY_LEVEL1ID] == cmbAllocation.Value))
                        {
                            if (Boolean.Parse(row[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE].ToString()))
                            {
                                isDefaultAllocationPref = true;
                                return isDefaultAllocationPref;
                            }
                        }
                    }
                }
                return isDefaultAllocationPref;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetStrategyComboVisibility()
        {
            try
            {
                if (cmbAllocation.DataSource != null && cmbAllocation.Value != null)
                {
                    DataTable dt = cmbAllocation.DataSource as DataTable;
                    if (dt != null)
                    {
                        foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => row[OrderFields.PROPERTY_LEVEL1ID] == cmbAllocation.Value))
                        {
                            if (Boolean.Parse(row[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE].ToString()))
                            {
                                if (cmbStrategy.DataSource != null)
                                    cmbStrategy.Value = null;
                                cmbStrategy.Enabled = false;
                            }
                            else
                            {
                                cmbStrategy.Enabled = true;
                            }

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

        private bool ValidateControlsBeforeTrade()
        {
            bool isValidated = true;

            try
            {
                var ultraComobEditors = GetAll(this, typeof(UltraComboEditor));
                errorProvider.Clear();
                foreach (UltraComboEditor ucmbEditor in ultraComobEditors.Cast<UltraComboEditor>().Where(ucmbEditor => !ucmbEditor.Name.Equals("txtSymbol") && !ucmbEditor.Name.Equals("cmbOptionType") && !ucmbEditor.Name.Equals("cmbTradeAttribute1") && !ucmbEditor.Name.Equals("cmbTradeAttribute2") && !ucmbEditor.Name.Equals("cmbTradeAttribute3") && !ucmbEditor.Name.Equals("cmbTradeAttribute4") && !ucmbEditor.Name.Equals("cmbTradeAttribute5") && !ucmbEditor.Name.Equals("cmbTradeAttribute6") && !ucmbEditor.Name.Equals("cmbOptionType")))
                {
                    if (ucmbEditor.Value == null)
                    {
                        continue;
                    }
                    if (!ValueListUtilities.CheckIfValueExistsInValuelist(ucmbEditor.ValueList, ucmbEditor.Value.ToString()))
                    {
                        errorProvider.SetIconPadding(ucmbEditor, -35);
                        errorProvider.SetError(ucmbEditor, TradingTicketConstants.MSG_VALUE_IS_INVALID_FOR_FIELD);
                        isValidated = false;
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
            return isValidated;
        }

        /// <summary>
        /// Handles the Click event of the btnGetLimitPrice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnGetLimitPrice_Click(object sender, EventArgs e)
        {
            try
            {
                ttPresenter.GetLimitPice();
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


        /// Handles the Click event of the btnSymbolLookup control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnSymbolLookup_Click(object sender, EventArgs e)
        {
            try
            {
                ttPresenter.SymbolLookupClick(pranaSymbolCtrl.Text);
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

        private void btnSymbolLookup_LostFocus(object sender, EventArgs e)
        {
            this.btnSymbolLookup.BackgroundImage = global::Prana.TradingTicket.Properties.Resources.SecurityMaster;
        }

        private void btnSymbolLookup_GotFocus(object sender, EventArgs e)
        {
            this.btnSymbolLookup.BackgroundImage = global::Prana.TradingTicket.Properties.Resources.SecurityMaster_Hover;
        }

        private void btnSymbolLookup_MouseLeave(object sender, EventArgs e)
        {
            this.btnSymbolLookup.BackgroundImage = global::Prana.TradingTicket.Properties.Resources.SecurityMaster;
        }

        private void btnSymbolLookup_MouseEnter(object sender, EventArgs e)
        {
            this.btnSymbolLookup.BackgroundImage = global::Prana.TradingTicket.Properties.Resources.SecurityMaster_Hover;
        }

        /// <summary>
        /// Handles the Click event of the btnTargetQuantityPercentage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnTargetQuantityPercentage_Click(object sender, EventArgs e)
        {
            try
            {
                ttPresenter.EnterTargetQuantityInPercentage = !ttPresenter.EnterTargetQuantityInPercentage;
                btnTargetQuantityPercentage.Text = ttPresenter.EnterTargetQuantityInPercentage ? TradingTicketConstants.LIT_PERCENTAGE : TradingTicketConstants.LIT_NUMBER;
                if (ttPresenter.EnterTargetQuantityInPercentage)
                {
                    if (nmrcQuantity.Value > 0)
                    {
                        decimal targetQuantity = Math.Round(nmrcTargetQuantity.Value, nmrcQuantity.DecimalPlaces);
                        decimal quantity = Math.Round(nmrcQuantity.Value, nmrcQuantity.DecimalPlaces);
                        decimal targetQuantityValue = Math.Round(((targetQuantity / quantity) * 100), nmrcQuantity.DecimalPlaces);
                        nmrcTargetQuantity.DecimalPlaces = nmrcQuantity.DecimalPlaces;
                        nmrcTargetQuantity.Maximum = 100;
                        nmrcTargetQuantity.Value = targetQuantityValue;
                    }
                    else
                        nmrcTargetQuantity.Value = 0.0m;
                }
                else
                {
                    decimal quantity = Math.Round(nmrcQuantity.Value, nmrcQuantity.DecimalPlaces);
                    decimal targetQuantityValue = Math.Floor(nmrcTargetQuantity.Value / 100 * nmrcQuantity.Value);
                    nmrcTargetQuantity.DecimalPlaces = nmrcQuantity.DecimalPlaces;
                    nmrcTargetQuantity.Maximum = quantity;
                    nmrcTargetQuantity.Value = targetQuantityValue;
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
        /// Show the calculated quantity when notioanl is entered in the quantity field.
        /// </summary>
        public void SetNotionalQuantityMessage()
        {
            try
            {
                if (!(ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward))
                {
                    if (ttPresenter.UseQuantityFieldAsNotional)
                    {
                        if (Double.Parse(nmrcPrice.Text) > 0 && Double.Parse(nmrcQuantity.Text) > 0)
                        {
                            SetLabelMessage(String.Format(TradingTicketConstants.MSG_NOTIONAL_AS_QUANTITY_MESSAGE, cmbOrderSide.Text, Math.Floor((Double.Parse(nmrcQuantity.Text) / Double.Parse(nmrcPrice.Text))), pranaSymbolCtrl.Text));
                        }
                        else if (Double.Parse(nmrcPrice.Text) > 0)
                        {
                            SetLabelMessage(TradingTicketConstants.MSG_VALID_AMOUNT_IN_DOLLARS);
                        }
                        else
                        {
                            SetLabelMessage(TradingTicketConstants.MSG_VALID_PRICE_IN_DOLLARS);
                        }
                    }
                    else
                    {
                        SetLabelMessage(String.Empty);
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
        /// Handles the Click event of the btnQuantity1 control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuantity_Click(object sender, EventArgs e)
        {
            try
            {
                ttPresenter.UseQuantityFieldAsNotional = !ttPresenter.UseQuantityFieldAsNotional;

                btnQuantity.Text = ttPresenter.UseQuantityFieldAsNotional ? TradingTicketConstants.LIT_DOLLAR : TradingTicketConstants.LIT_QUANTITY;
                lblQuantity.Text = ttPresenter.UseQuantityFieldAsNotional ? TradingTicketConstants.CAPTION_DOLLAR_QUANTITY : TradingTicketConstants.CAPTION_QUANTITY;
                lblTargetQuantity.Text = ttPresenter.UseQuantityFieldAsNotional ? TradingTicketConstants.CAPTION_DOLLAR_TARGET_QUANTITY : TradingTicketConstants.CAPTION_TARGET_QUANTITY;


                SetNotionalQuantityMessage();

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
        /// Handles the Click event of the btnViewAllocationDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnViewAllocationDetails_Click(object sender, EventArgs e)
        {
            try
            {
                ttPresenter.OpenViewAllocationDetailsForm();
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
        /// Handles the Click event of the btnShortLocateList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnShortLocateList_Click(object sender, EventArgs e)
        {
            try
            {
                OpenShortLocateListPopUpForm();
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
        /// Changes the icon for theme.
        /// </summary>
        private void ChangeIconForTheme()
        {
            try
            {
                ComponentResourceManager resources = new ComponentResourceManager(typeof(TradingTicket));
                if (CustomThemeHelper.ApplyTheme)
                    Icon = ((Icon)(resources.GetObject("TTIconThemeOn")));
                else
                    Icon = ((Icon)(resources.GetObject("TTIconThemeOff")));
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
        /// CheckBoxes the option checked.
        /// </summary>
        private void CheckBoxOptionChecked()
        {
            try
            {
                SetCheckBoxDelegate mi = CheckBoxOptionChecked;
                if (UIValidation.GetInstance().validate(chkBoxSwap))
                {
                    if (chkBoxOption.InvokeRequired)
                    {
                        BeginInvoke(mi, null);
                    }
                    else
                    {
                        chkBoxOption.Enabled = !chkBoxSwap.Checked;
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
        /// Handles the ValueChanged event of the cmbBroker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void cmbBroker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    if (cmbBroker.Value == null)
                    {
                        return;
                    }
                    int brokerID = int.MinValue;

                    if (int.TryParse(cmbBroker.Value.ToString(), out brokerID))
                    {
                        ttPresenter.CounterPartyId = brokerID;

                        cmbCommissionBasis.SelectedIndex = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis.ContainsKey(ttPresenter.CounterPartyId) ? TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis[ttPresenter.CounterPartyId] : 4;

                        cmbSoftCommissionBasis.SelectedIndex = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseSoftCommissionBasis.ContainsKey(ttPresenter.CounterPartyId) ? TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseSoftCommissionBasis[ttPresenter.CounterPartyId] : 4;

                        //Set Execution Intruction combo Value, Broker wise
                        if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(ttPresenter.CounterPartyId))
                        {
                            if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbExecutionInstructions.ValueList, TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[ttPresenter.CounterPartyId].ToString()))
                            {
                                cmbExecutionInstructions.Value = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[ttPresenter.CounterPartyId];
                            }
                        }
                        //Set Execution Intruction combo Value, User wise
                        else if (TradingTktPrefs.UserTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                        {
                            if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbExecutionInstructions.ValueList, TradingTktPrefs.UserTradingTicketUiPrefs.ExecutionInstruction.Value.ToString()))
                            {
                                cmbExecutionInstructions.Value = TradingTktPrefs.UserTradingTicketUiPrefs.ExecutionInstruction.Value;
                            }
                        }
                        //Set Execution Intruction combo Value, Company wise
                        else if (TradingTktPrefs.CompanyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                        {
                            if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbExecutionInstructions.ValueList, TradingTktPrefs.CompanyTradingTicketUiPrefs.ExecutionInstruction.Value.ToString()))
                            {
                                cmbExecutionInstructions.Value = TradingTktPrefs.CompanyTradingTicketUiPrefs.ExecutionInstruction.Value;
                            }
                        }
                        ttPresenter.FillVenueCombo();
                        if (ttPresenter.IncomingOrderRequest == null || _isTTSourceDependentOnAnotherUIs)
                        {

                            //Set venue combo Value, user wise then Company wise then default
                            if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue.ContainsKey(Convert.ToInt32(cmbBroker.Value)) && ValueListUtilities.CheckIfValueExistsInValuelist(cmbVenue.ValueList, TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue[Convert.ToInt32(cmbBroker.Value)].ToString()))
                            {
                                UpdateVenueCombo(TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue[Convert.ToInt32(cmbBroker.Value)], ChangeType.NoTrade);
                            }
                            else if (ttPresenter.CompanyTradingTicketUiPrefs.Venue.HasValue && cmbVenue.Value == null && ValueListUtilities.CheckIfValueExistsInValuelist(cmbVenue.ValueList, ttPresenter.CompanyTradingTicketUiPrefs.Venue.ToString()))
                            {
                                UpdateVenueCombo(int.Parse(ttPresenter.CompanyTradingTicketUiPrefs.Venue.ToString()), ChangeType.NoTrade);
                            }
                            else if (cmbVenue.Value == null && cmbVenue.ValueList != null && cmbVenue.ValueList.ValueListItems.Count > 0)
                            {
                                UpdateComboValue(cmbVenue, cmbVenue.ValueList.ValueListItems[0].DataValue.ToString(), cmbVenue.ValueList.ValueListItems[0].DisplayText, ChangeType.NoTrade);
                            }
                            else
                            {
                                cmbVenue.Value = null;
                            }

                        }
                        btnBrokerConnectionStatus.Visible = true;
                        if (TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(brokerID) == PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            btnBrokerConnectionStatus.BackColor = Color.FromArgb(104, 156, 46);
                        }
                        else
                        {
                            btnBrokerConnectionStatus.BackColor = Color.FromArgb(140, 5, 5);
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
        /// Algob Control Setup
        /// </summary>
        private void AlgoControlSetup()
        {
            try
            {

                if (ttPresenter.IsShowAlgoControls && cmbVenue.Value != null && cmbVenue.Text.ToLower().Equals("algo") && CachedDataManager.GetInstance.IsAlgoBrokerFromID(ttPresenter.CounterPartyId))
                {
                    bottomTabControl.Tabs[TradingTicketConstants.TAB_ALGO_KEY].Visible = true;
                    bottomTabControl.Tabs[TradingTicketConstants.TAB_ALGO_KEY].Selected = true;
                    grbBoxStrategyControl.Enabled = true;
                    grbBoxStrategyControl.Expanded = true;
                    grbBoxStrategyControl.Visible = true;
                    DrawAlgoControls();
                }
                else
                {
                    bottomTabControl.Tabs[TradingTicketConstants.TAB_ALGO_KEY].Visible = false;
                    bottomTabControl.Tabs[TradingTicketConstants.TAB_COMMISION_KEY].Selected = true;
                    strategyControl1.Visible = false;
                    grbBoxStrategyControl.Enabled = false;
                    grbBoxStrategyControl.Expanded = false;
                    if (ttPresenter.IsShowAlgoControls)
                        AdjustTTControlsSizeDynamically(true);
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
        /// Handles the ValueChanged event of the cmbCommissionBasis control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void cmbCommissionBasis_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    nmrcCommissionRate.Enabled = cmbCommissionBasis.Text != TradingTicketConstants.C_LIT_AUTO;
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
        /// Set Quantity Label Delegate
        /// </summary>
        /// <param name="text">The text.</param>
        delegate void SetQuantityLabelDelegate(string text);

        /// <summary>
        /// This method is use to change the text of Quantity label on changes in deal in combo box.
        /// </summary>
        /// <param name="text">string</param>
        private void SetQuantityLabel(string text)
        {
            try
            {
                if (this.lblQuantity.InvokeRequired)
                {
                    SetQuantityLabelDelegate d = new SetQuantityLabelDelegate(SetQuantityLabel);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.lblQuantity.Text = text;
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
        /// Set Counter Currency Amount Label Delegate
        /// </summary>
        /// <param name="text">The text.</param>
        delegate void SetCounterCurrencyAmountLabelDelegate(string text);

        /// <summary>
        /// This method is use to change the text of Counter Currency Amount label on changes in deal in combo box.
        /// </summary>
        /// <param name="text">string</param>
        private void SetCounterCurrencyAmountLabel(string text)
        {
            try
            {
                if (this.lblCCA.InvokeRequired)
                {
                    SetCounterCurrencyAmountLabelDelegate d = new SetCounterCurrencyAmountLabelDelegate(SetCounterCurrencyAmountLabel);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.lblCCA.Text = text;
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
        /// Handles the ValueChanged event of the cmbDealIn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void cmbDealIn_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    SetQuantityLabel(string.Format("Quantity({0})", ((UltraComboEditor)sender).Text));
                    string counterCurrency = CachedDataManager.GetInstance.GetCurrencyID(((UltraComboEditor)sender).Text) == ttPresenter.LeadCurrencyId ? CachedDataManager.GetInstance.GetCurrencyText(ttPresenter.VsCurrencyId) : CachedDataManager.GetInstance.GetCurrencyText(ttPresenter.LeadCurrencyId);
                    SetCounterCurrencyAmountLabel(string.Format("Currency({0})", counterCurrency));
                    Double priceForCalculation = 0;
                    Double limitPriceForCalculation = 0;
                    Double.TryParse(nmrcPrice.Text, out priceForCalculation);
                    Double.TryParse(nmrcLimit.Text, out limitPriceForCalculation);
                    if (cmbDealIn.Value != null)
                    {

                        ttPresenter.CurrencyId = int.Parse(cmbDealIn.Value.ToString());
                        if (cmbFxOperator.Value != null)
                        {
                            cmbFxOperator.Value = Operator.M.ToString();
                            if (ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward || ttPresenter.AssetID == (int)AssetCategory.Forex)
                            {
                                decimal value = decimal.Parse(nmrcPrice.Value.ToString());
                                if (ttPresenter.LeadCurrencyId != ttPresenter.CompanyBaseCurrencyID)
                                    nmrcFXRate.Value = value;
                                else
                                    nmrcFXRate.Value = value != 0 ? Math.Round(1 / value, 4) : 0;
                            }
                        }

                        decimal position = Quantity;
                        if (position == 0 || Price == 0)
                        {
                            nmrcCCA.Value = (decimal)0.0;
                            return;
                        }

                        if (OrderSide == FIXConstants.SIDE_Buy || OrderSide == "9" || OrderSide == "10")
                            position = position * -1;

                        if (CachedDataManager.GetInstance.GetCurrencyText(ttPresenter.LeadCurrencyId).Equals((((UltraComboEditor)sender).Text)))
                            nmrcCCA.Value = Math.Round(position * Price, 4);
                        else
                            nmrcCCA.Value = Math.Round(position / Price, 4);
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
        /// Handles the ValueChanged event of the cmbFxOperator control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void cmbFxOperator_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    CalculateAutoCalculatedFields(SettlementCachePreferences.SettlementAutoCalculateField);
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
        /// Handles the ValueChanged event of the cmbOrderSide control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void cmbOrderSide_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    if (cmbOrderSide.Value == null)
                    {
                        return;
                    }

                    if (ttPresenter.Symbol != string.Empty)
                    {
                        btnGetLimitPrice_Click(sender, e);
                    }

                    if (ttPresenter.OrderRequest.TransactionSource == TransactionSource.PST)
                    {
                        ttPresenter.ResetPTTDetailsAndReloadTT(ttPresenter.OrderRequest, false);
                    }
                    if (ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward || ttPresenter.AssetID == (int)AssetCategory.Forex)
                    {
                        decimal position = Quantity;

                        if (position == 0 || Price == 0)
                        {
                            nmrcCCA.Value = (decimal)0.0;
                            return;
                        }
                        if (((UltraComboEditor)sender).Text.Contains("Buy"))
                            position = position * -1;
                        if (ttPresenter.LeadCurrencyId.ToString().Equals(DealIn))
                            nmrcCCA.Value = Math.Round(position * Price, 4);
                        else
                            nmrcCCA.Value = Math.Round(position / Price, 4);
                    }
                }
                SetNotionalQuantityMessage();



                if (AlgoStrategyControlProperty != null && cmbOrderSide.Value != null)
                {
                    AlgoStrategyControlProperty.EnableAlgoControlsBasedTTFields(cmbOrderSide.Value.ToString(), "OrderSideTagValue");
                }
                // OpenShortLocatePopup();
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

        private void OpenShortLocatePopup(bool IsParentTT = false)
        {
            try
            {
                if (cmbOrderSide.Text.Contains("Sell short") && ModuleManager.CheckModulePermissioning(PranaModules.SHORTLOCATE_MODULE, PranaModules.SHORTLOCATE_MODULE))
                {
                    bool isSymbolpresentinshortlocate = ShortLocateDataManager.GetInstance.IsSymbolAvailableForShortLocate(ttPresenter.Symbol, "", "", (cmbFunds.SelectedItem != null && cmbFunds.SelectedIndex > 0) ? cmbFunds.SelectedItem.ToString() : string.Empty, 0);
                    if (isSymbolpresentinshortlocate)
                    {
                        if (!btnReplace.Visible)
                        {
                            if (IsParentTT)
                                ttPresenter.TradingTicketParentType = TradingTicketParent.None;
                            bottomTabControl.Tabs[TradingTicketConstants.TAB_BorrowParameter_Key].Visible = true;
                            bottomTabControl.Tabs[TradingTicketConstants.TAB_BorrowParameter_Key].Selected = true;
                            BindingList<ShortLocateListParameter> order = new BindingList<ShortLocateListParameter>();
                            shortLocateList1.BindData(order, true);
                            shortLocateList1.grdShortLocateList.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
                            CustomThemeHelper.SetThemeProperties(shortLocateList1, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_DAILY_PM_CLIENTUI);
                            if (ttPresenter.TradingTicketParentType != TradingTicketParent.ShortLocate)
                            {
                                OpenShortLocateListPopUpForm();
                            }
                            else
                            {
                                shortLocateList1.BindData(new BindingList<ShortLocateListParameter>() { ttPresenter.IncomingOrderRequest.ShortLocateParameter }, true);
                                shortLocateList1.grdShortLocateList.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
                            }
                            shortLocateList1.grdShortLocateList.DisplayLayout.Bands[0].Columns["NirvanaLocateID"].Hidden = true;
                            btnShortLocateList.Visible = true;
                        }
                    }
                    else
                    {
                        bottomTabControl.Tabs[TradingTicketConstants.TAB_BorrowParameter_Key].Visible = false;
                        btnShortLocateList.Visible = true;
                    }
                }
                else
                {
                    bottomTabControl.Tabs[TradingTicketConstants.TAB_BorrowParameter_Key].Visible = false;
                    btnShortLocateList.Visible = false;
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

        ShortLocateListPopUp formshortLocateListPopup = null;
        private void OpenShortLocateListPopUpForm()
        {
            try
            {
                BindingList<ShortLocateOrder> NewOrderList = new BindingList<ShortLocateOrder>();
                BindingList<ShortLocateListParameter> order = new BindingList<ShortLocateListParameter>();
                if (formshortLocateListPopup == null)
                {
                    formshortLocateListPopup = new ShortLocateListPopUp();
                    formshortLocateListPopup.Bind(this.SymbolText, (cmbFunds.SelectedItem != null && cmbFunds.SelectedIndex > 0) ? cmbFunds.SelectedItem.ToString() : string.Empty);
                    formshortLocateListPopup.ShowDialog(this);
                    if (formshortLocateListPopup.SelectedDataShortLocateParameter != null)
                    {
                        ShortLocateListParameter dr = formshortLocateListPopup.SelectedDataShortLocateParameter;
                        nmrcQuantity.Value = Convert.ToDecimal(dr.BorrowQuantity);
                        Dictionary<int, string> dictBrokers = CommonDataCache.CachedDataManager.GetInstance.GetAllCounterParties();
                        int BrokerID = dictBrokers.FirstOrDefault(x => x.Value == dr.Broker).Key;
                        if (ShortLocateDataManager.BrokerAccountMapping.ContainsKey(BrokerID.ToString()))
                            cmbAllocation.Value = ShortLocateDataManager.BrokerAccountMapping[BrokerID.ToString()];

                        order.Add(dr);
                        shortLocateList1.BindData(order, true);
                        if (!ShortLocateDataManager.GetInstance.IsSymbolAvailableForShortLocate(SymbolText, dr.Broker, dr.BorrowerId, (cmbFunds.SelectedItem != null && cmbFunds.SelectedIndex > 0) ? cmbFunds.SelectedItem.ToString() : string.Empty, dr.NirvanaLocateID))
                        {
                            ShortLocateOrder NewOrder = new ShortLocateOrder();
                            NewOrder.Ticker = SymbolText;
                            NewOrder.Broker = dr.Broker;
                            NewOrder.ClientMasterfund = (cmbFunds.SelectedItem != null && cmbFunds.SelectedIndex > 0) ? cmbFunds.SelectedItem.ToString() : string.Empty;
                            NewOrder.TradeQuantity = 0;
                            NewOrder.BorrowSharesAvailable = dr.BorrowSharesAvailable;
                            NewOrder.BorrowRate = dr.BorrowRate;
                            NewOrder.BorrowerId = dr.BorrowerId;
                            NewOrder.BorrowedShare = 0;
                            NewOrder.BorrowedRate = 0;
                            NewOrder.SODBorrowRate = dr.BorrowRate;
                            NewOrder.SODBorrowshareAvailable = dr.BorrowSharesAvailable;
                            NewOrder.StatusSource = "API";
                            NewOrderList.Add(NewOrder);
                            ShortLocateDataManager.GetInstance.SaveShortLocateData(NewOrderList, TransactionSource.TradingTicket);

                            var slCollection = ShortLocateDataManager.GetInstance.GetShortLocateCollection(NewOrder.ClientMasterfund, false);
                            if (slCollection.Count > 0)
                            {
                                ShortLocateListParameter NewDataObj = new ShortLocateListParameter();
                                NewDataObj.BorrowerId = dr.BorrowerId;
                                NewDataObj.BorrowQuantity = dr.BorrowQuantity;
                                NewDataObj.BorrowRate = dr.BorrowRate;
                                NewDataObj.BorrowSharesAvailable = dr.BorrowSharesAvailable;
                                NewDataObj.Broker = dr.Broker;
                                NewDataObj.NirvanaLocateID = slCollection[slCollection.Count - 1].NirvanaLocateID;
                                BindingList<ShortLocateListParameter> NewList = new BindingList<ShortLocateListParameter>();
                                NewList.Add(NewDataObj);
                                shortLocateList1.BindData(NewList, true);
                            }
                        }
                        shortLocateList1.grdShortLocateList.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
                    }
                    if (formshortLocateListPopup.SelectedDataShortLocateParameter == null && shortLocateList1.grdShortLocateList.ActiveRow == null)
                        bottomTabControl.Tabs[TradingTicketConstants.TAB_BorrowParameter_Key].Visible = false;
                    else
                        bottomTabControl.Tabs[TradingTicketConstants.TAB_BorrowParameter_Key].Visible = true;
                    formshortLocateListPopup = null;
                    btnShortLocateList.Visible = true;
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
        /// Handles the ValueChanged event of the cmbOrderType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void cmbOrderType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    if (cmbOrderType.Value == null)
                    {
                        return;
                    }
                    nmrcLimit.Enabled = false;
                    nmrcStop.Enabled = false;
                    switch (cmbOrderType.Value.ToString())
                    {
                        case FIXConstants.ORDTYPE_Limit:
                            nmrcStop.Enabled = false;
                            nmrcLimit.Enabled = true;
                            break;

                        case FIXConstants.ORDTYPE_Market:
                            nmrcLimit.Enabled = false;
                            nmrcStop.Enabled = false;
                            break;

                        case FIXConstants.ORDTYPE_Pegged:
                            nmrcLimit.Enabled = true;
                            nmrcStop.Enabled = false;
                            break;

                        case FIXConstants.ORDTYPE_Stop:
                            nmrcLimit.Enabled = false;
                            nmrcStop.Enabled = true;
                            break;

                        case FIXConstants.ORDTYPE_Stoplimit:
                            nmrcLimit.Enabled = true;
                            nmrcStop.Enabled = true;
                            break;

                        case FIXConstants.ORDTYPE_MarketOnClose:
                            break;

                        default:
                            nmrcStop.Enabled = false;
                            break;
                    }
                    UpdateNotionalValueOnCaption();
                    UpdateCaption();
                    if (AlgoStrategyControlProperty != null && cmbOrderType.Value != null)
                    {
                        AlgoStrategyControlProperty.EnableAlgoControlsBasedTTFields(cmbOrderType.Value.ToString(), "OrderTypeValue");
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
        /// Handles the ValueChanged event of the cmbSettlementCurrency control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void cmbSettlementCurrency_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int settlementCurrency;
                if (cmbSettlementCurrency.Value != null && int.TryParse(cmbSettlementCurrency.Value.ToString(), out settlementCurrency) && settlementCurrency == ttPresenter.CurrencyId && SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.FXRate)
                {
                    nmrcFXRate.Enabled = true;
                    CalculateAutoCalculatedFields(SettlementAutoCalculateField.SettlementPrice);
                }
                else
                {
                    if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.FXRate)
                    {
                        nmrcFXRate.Enabled = false;
                    }
                    else
                    {
                        nmrcFXRate.Enabled = true;
                    }
                    CalculateAutoCalculatedFields(SettlementCachePreferences.SettlementAutoCalculateField);
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
        /// Handles the ValueChanged event of the nmrcPrice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void nmrcPrice_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                nmrcPrice.Increment = nmrcLimit.Increment;
                if (nmrcPrice.Text != Convert.ToDecimal(0).ToString())
                {
                    errorProvider.SetError(nmrcPrice, String.Empty);
                    ToolTip1.SetToolTip(this.btnCreateOrder, String.Empty);
                    ToolTip1.SetToolTip(this.btnSend, String.Empty);
                }
                if (chkBoxSwap.Checked)
                {
                    UpdateNotionalValueOnSwapControl();
                    double avgPrice = 0.0;
                    Double.TryParse(nmrcPrice.Text, out avgPrice);
                    ctrlSwapParameters1.UpdateCostBasis(avgPrice);
                }
                if (ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward || ttPresenter.AssetID == (int)AssetCategory.Forex)
                {
                    decimal position = Quantity;
                    decimal localPrice = ((PranaNumericUpDown)sender).Value;
                    if (position == 0 || localPrice == 0)
                    {
                        nmrcCCA.Value = (decimal)0.0;
                        return;
                    }

                    if (OrderSide == FIXConstants.SIDE_Buy || OrderSide == "9" || OrderSide == "10")
                        position = position * -1;
                    if (ttPresenter.LeadCurrencyId.ToString().Equals(DealIn))
                        nmrcCCA.Value = Math.Round(position * localPrice, 4);
                    else
                        nmrcCCA.Value = Math.Round(position / localPrice, 4);
                }

                SetNotionalQuantityMessage();

                int settlementCurrency;
                if (cmbSettlementCurrency.Value != null && int.TryParse(cmbSettlementCurrency.Value.ToString(), out settlementCurrency) && settlementCurrency == ttPresenter.CurrencyId && SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.FXRate)
                {
                    CalculateAutoCalculatedFields(SettlementAutoCalculateField.SettlementPrice);
                }
                else
                {
                    CalculateAutoCalculatedFields(SettlementCachePreferences.SettlementAutoCalculateField);
                }

                if (ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward || ttPresenter.AssetID == (int)AssetCategory.Forex)
                {
                    if (ttPresenter.LeadCurrencyId != ttPresenter.CompanyBaseCurrencyID)
                        nmrcFXRate.Value = nmrcPrice.Value;
                    else
                        nmrcFXRate.Value = nmrcPrice.Value != 0 ? Math.Round(1 / nmrcPrice.Value, 4) : 0;
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

        private void nmrcFXRate_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                int settlementCurrency;
                if (!(cmbSettlementCurrency.Value != null && int.TryParse(cmbSettlementCurrency.Value.ToString(), out settlementCurrency) && settlementCurrency == ttPresenter.CurrencyId && SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.FXRate))
                {
                    CalculateAutoCalculatedFields(SettlementCachePreferences.SettlementAutoCalculateField);
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

        private void nmrcSettlementPrice_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                int settlementCurrency;
                if (cmbSettlementCurrency.Value != null && int.TryParse(cmbSettlementCurrency.Value.ToString(), out settlementCurrency) && settlementCurrency == ttPresenter.CurrencyId && SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.FXRate)
                {
                    CalculateAutoCalculatedFields(SettlementAutoCalculateField.AveragePrice);
                }
                else
                {
                    CalculateAutoCalculatedFields(SettlementCachePreferences.SettlementAutoCalculateField);
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

        private void CalculateAutoCalculatedFields(SettlementAutoCalculateField settlementAutoCalculateField)
        {
            try
            {
                decimal avgPrice = nmrcPrice.Value;
                decimal fxRate = nmrcFXRate.Value;
                decimal settlementPrice = nmrcSettlementPrice.Value;

                int settlementCurrency;
                if (cmbSettlementCurrency.Value != null && int.TryParse(cmbSettlementCurrency.Value.ToString(), out settlementCurrency) && settlementCurrency == ttPresenter.CurrencyId)
                {
                    fxRate = 1;
                }

                if (ttPresenter.CurrencyId != ttPresenter.CompanyBaseCurrencyID)
                {
                    if (settlementAutoCalculateField == SettlementAutoCalculateField.AveragePrice)
                    {
                        nmrcPrice.ValueChanged -= nmrcPrice_ValueChanged;
                        if (fxRate != 0)
                        {
                            nmrcPrice.Value = settlementPrice / fxRate;
                        }
                        else
                        {
                            nmrcPrice.Value = 0;
                        }
                        nmrcPrice.ValueChanged += nmrcPrice_ValueChanged;

                        nmrcLimit.Value = nmrcPrice.Value;
                    }
                    else if (settlementAutoCalculateField == SettlementAutoCalculateField.SettlementPrice)
                    {
                        nmrcSettlementPrice.ValueChanged -= nmrcSettlementPrice_ValueChanged;
                        if (FxOperator == Operator.M.ToString())
                        {
                            nmrcSettlementPrice.Value = avgPrice * fxRate;
                        }
                        else if (FxOperator == Operator.D.ToString() && fxRate != 0)
                        {
                            nmrcSettlementPrice.Value = avgPrice / fxRate;
                        }
                        else
                            nmrcSettlementPrice.Value = 0;

                        nmrcSettlementPrice.ValueChanged += nmrcSettlementPrice_ValueChanged;
                    }
                    else if (settlementAutoCalculateField == SettlementAutoCalculateField.FXRate && !(ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward || ttPresenter.AssetID == (int)AssetCategory.Forex))
                    {
                        nmrcFXRate.ValueChanged -= nmrcFXRate_ValueChanged;
                        if (avgPrice != 0)
                        {
                            nmrcFXRate.Value = settlementPrice / avgPrice;
                        }
                        else
                        {
                            nmrcFXRate.Value = 0;
                        }
                        nmrcFXRate.ValueChanged += nmrcFXRate_ValueChanged;
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

        private void SettlementDetailsSetup()
        {
            try
            {
                switch (ttPresenter.AssetID)
                {
                    case (int)AssetCategory.FX:
                    case (int)AssetCategory.FXForward:
                        nmrcFXRate.Enabled = false;
                        cmbFxOperator.Enabled = false;
                        break;
                    default:
                        if (ttPresenter.CurrencyId == ttPresenter.CompanyBaseCurrencyID)
                        {
                            nmrcFXRate.Value = 1.0m;
                            nmrcFXRate.Enabled = false;
                            cmbFxOperator.Enabled = false;
                        }
                        else
                        {
                            nmrcFXRate.Value = 0.0m;
                            nmrcFXRate.Enabled = true;
                            cmbFxOperator.Enabled = true;
                            if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.FXRate)
                            {
                                nmrcFXRate.Enabled = false;
                            }
                            else if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.AveragePrice)
                            {
                                nmrcPrice.Enabled = false;
                            }
                            else if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.SettlementPrice)
                            {
                                nmrcSettlementPrice.Enabled = false;
                            }
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
        }

        /// <summary>
        /// Handles the ValueChanged event of the cmbSoftBasis control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void cmbSoftBasis_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    nmrcSoftRate.Enabled = cmbSoftCommissionBasis.Text != TradingTicketConstants.C_LIT_AUTO;
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
        /// Handles the ValueChanged event of the cmbVenue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void cmbVenue_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    if (cmbVenue.Value == null)
                    {
                        return;
                    }
                    if (cmbBroker.Value == null)
                    {
                        return;
                    }

                    if (cmbVenue.Value != null)
                    {
                        ttPresenter.AlgoStrategyId = string.Empty;
                        AlgoControlSetup();
                    }
                    int venueId = int.MinValue;
                    if (int.TryParse(cmbVenue.Value.ToString(), out venueId))
                    {
                        ttPresenter.UpdateOrderSideAndUpdatePTTDetails();
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
        /// Draws the algo controls.
        /// </summary>
        private void DrawAlgoControls()
        {
            try
            {
                strategyControl1.Visible = true;
                int _defaultAlgoType = int.MinValue;
                if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionAlgoType.ContainsKey(Convert.ToInt32(ttPresenter.CounterPartyId)))
                {
                    _defaultAlgoType = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionAlgoType[Convert.ToInt32(ttPresenter.CounterPartyId)];
                }

                strategyControl1.SetStrategies(ttPresenter.CounterPartyId.ToString(), CachedDataManager.GetInstance.GetUnderLyingText(ttPresenter.UnderlyingID));



                //If Replace then do not set the default 
                if (ttPresenter.IncomingOrderRequest == null || _isTTSourceDependentOnAnotherUIs)
                {
                    strategyControl1.Reset();
                    strategyControl1.SetStrategyID(_defaultAlgoType.ToString());
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
        /// Gets the order side.
        /// </summary>
        /// <returns></returns>
        private string GetOppositeOrderSide()
        {
            try
            {
                return cmbOrderSide.Text == TradingTicketConstants.MSG_BUY ? TradingTicketConstants.MSG_SELL : TradingTicketConstants.MSG_BUY;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return TradingTicketConstants.MSG_BUY;
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        private void InitControl()
        {
            try
            {
                ttPresenter.TradingTicketParentType = _tradingTicketParent;
                ttPresenter.InitControl(LoginUser);
                ttPresenter.PriceForComplianceNotAvailable += this.PriceNotAvailableForCompliance;
                Disposed += tradingTicket_Disposed;
                oneSymbolL1Strip.L1DataResponse += onL1Response;
                oneSymbolL1Strip.ToggleRoundLotSwitch += toggleRoundLotSwitch;
                FormClosed += TradingTicket_FormClosed;
                pranaOptionCtrl.SetUp(SymbologyHelper.DefaultSymbology, TradingTktPrefs.TTGeneralPrefs.DefaultOptionType);
                if (SymbologyHelper.DefaultSymbology == ApplicationConstants.SymbologyCodes.ActivSymbol || SymbologyHelper.DefaultSymbology == ApplicationConstants.SymbologyCodes.FactSetSymbol)
                    chkBoxOption.Visible = false;
                if (!(SymbologyHelper.DefaultSymbology == ApplicationConstants.SymbologyCodes.ActivSymbol || SymbologyHelper.DefaultSymbology == ApplicationConstants.SymbologyCodes.FactSetSymbol))
                    chkBoxOption.Checked = TradingTktPrefs.TTGeneralPrefs.IsShowOptionDetails;
                if (chkBoxOption.Checked)
                {
                    pranaSymbolCtrl.Enabled = false;
                }
                pranaSymbolCtrl.SymbolSearch += pranaSymbolCtrl_SymbolSearch;
                pranaSymbolCtrl.SymbolEntered += pranaSymbolCtrl_SymbolEntered;
                pranaOptionCtrl.ValidateSymbol += pranaOptionCtrl_ValidateSymbol;
                strategyControl1.StrategyValueChanged += strategyControl1_StrategyValueChanged;
                algoStrategyControl.ClickedSendButtonEvent += algoStrategyControl_SendButtonClicked;
                TradeManagerExtension.GetInstance().CounterPartyStatusUpdate += TradingTicket_CounterPartyStatusUpdate;
                strategyControl1.Visible = false;
                algoStrategyControl.SetUp();
                TradingTicketEnabled(false);
                ActiveControl = pranaSymbolCtrl;
                if (ttPresenter.CompanyTradingTicketUiPrefs.IsShowTargetQTY.HasValue)
                {
                    if (!(bool)ttPresenter.CompanyTradingTicketUiPrefs.IsShowTargetQTY)
                    {
                        IsShowTargetQTY = (bool)ttPresenter.CompanyTradingTicketUiPrefs.IsShowTargetQTY;
                        SetTTControlsBasedonPref(false);
                    }
                    else
                    {
                        IsShowTargetQTY = true;
                    }
                }

                oneSymbolL1Strip.GetTotalQty = new LiveFeed.UI.Controls.OneSymbolL1Strip.GetTotalQtyDelegate(getOrderQty);
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
        /// Set TT Controls Based on Pref show hide Target QTY
        /// </summary>
        private void SetTTControlsBasedonPref(bool isShowTargetQTY)
        {
            try
            {

                tblPnlOthers.Controls.Remove(lblVenue);
                tblPnlOthers.Controls.Remove(cmbVenue);
                tblPnlOthers.Controls.Remove(cmbTradingAccount);
                tblPnlOthers.Controls.Remove(cmbHandlingInstructions);
                tblPnlOthers.Controls.Remove(cmbExecutionInstructions);
                tblPnlOthers.Controls.Remove(lblExecutionInstructions);
                tblPnlOthers.Controls.Remove(lblHandlingInstructions);
                tblPnlOthers.Controls.Remove(lblTradingAccount);

                tblPnlOthers.Controls.Add(cmbTradingAccount, 2, 1);
                tblPnlOthers.Controls.Add(cmbHandlingInstructions, 1, 1);
                tblPnlOthers.Controls.Add(cmbExecutionInstructions, 0, 1);
                tblPnlOthers.Controls.Add(lblExecutionInstructions, 0, 0);
                tblPnlOthers.Controls.Add(lblHandlingInstructions, 1, 0);
                tblPnlOthers.Controls.Add(lblTradingAccount, 2, 0);

                tblPnlTTMainControls.Controls.Add(cmbVenue, 4, 2);
                tblPnlTTMainControls.Controls.Add(lblVenue, 4, 1);

                if (isShowTargetQTY)
                {
                    PnlTargetQuantity.Visible = true;
                    nmrcTargetQuantity.Visible = true;
                    tblPnlTTMainControls.Controls.Remove(PnlTargetQuantity);
                    tblPnlTTMainControls.Controls.Remove(nmrcTargetQuantity);
                    tblPnlTTMainControls.Controls.Remove(lblStrategy);
                    tblPnlTTMainControls.Controls.Remove(cmbStrategy);
                    tblPnlTTMainControls.Controls.Remove(lblTradeDate);
                    tblPnlTTMainControls.Controls.Remove(dtTradeDate);
                    tblPnlTTMainControls.Controls.Remove(lblFXRate);
                    tblPnlTTMainControls.Controls.Remove(nmrcFXRate);
                    tblPnlTTMainControls.Controls.Remove(lblFxOperator);
                    tblPnlTTMainControls.Controls.Remove(cmbFxOperator);

                    tblPnlTTMainControls.Controls.Add(PnlTargetQuantity, 0, 5);
                    tblPnlTTMainControls.Controls.Add(nmrcTargetQuantity, 0, 6);
                    tblPnlTTMainControls.Controls.Add(lblStrategy, 1, 5);
                    tblPnlTTMainControls.Controls.Add(cmbStrategy, 1, 6);
                    tblPnlTTMainControls.Controls.Add(lblTradeDate, 2, 5);
                    tblPnlTTMainControls.Controls.Add(dtTradeDate, 2, 6);
                    tblPnlTTMainControls.Controls.Add(lblFXRate, 3, 5);
                    tblPnlTTMainControls.Controls.Add(nmrcFXRate, 3, 6);
                    tblPnlTTMainControls.Controls.Add(lblFxOperator, 4, 5);
                    tblPnlTTMainControls.Controls.Add(cmbFxOperator, 4, 6);

                }
                else
                {
                    PnlTargetQuantity.Visible = false;
                    nmrcTargetQuantity.Visible = false;
                    tblPnlTTMainControls.Controls.Remove(cmbAllocation);
                    tblPnlTTMainControls.Controls.Remove(ultraPanel2);
                    tblPnlTTMainControls.Controls.Remove(cmbBroker);
                    tblPnlTTMainControls.Controls.Remove(PnlBroker);

                    tblPnlTTMainControls.Controls.Add(cmbAllocation, 2, 2);
                    tblPnlTTMainControls.Controls.Add(ultraPanel2, 2, 1);
                    tblPnlTTMainControls.Controls.Add(cmbBroker, 3, 2);
                    tblPnlTTMainControls.Controls.Add(PnlBroker, 3, 1);
                }

                //this.cmbOrderSide.TabIndex = 6;
                //this.nmrcQuantity.TabIndex = 7;
                //this.cmbAllocation.TabIndex = 8;
                //this.cmbBroker.TabIndex = 9;
                //this.cmbVenue.TabIndex = 10;
                //this.cmbTIF.TabIndex = 11;
                //this.cmbOrderType.TabIndex = 12;
                //this.nmrcStop.TabIndex = 13;
                //this.nmrcLimit.TabIndex = 14;
                //this.nmrcPrice.TabIndex = 15;
                //this.nmrcTargetQuantity.TabIndex = 16;

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


        private double getOrderQty()
        {
            double targetQuantity = 0.0;
            try
            {
                if (ttPresenter.UseQuantityFieldAsNotional)
                {
                    if (double.Parse(nmrcPrice.Text) != 0)
                    {
                        targetQuantity = Math.Floor(Double.Parse(nmrcQuantity.Text) / Double.Parse(nmrcPrice.Text));
                    }
                }
                else
                {
                    Double.TryParse(nmrcQuantity.Text, out targetQuantity);
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
            return targetQuantity * ttPresenter.Multiplier;
        }

        /// <summary>
        /// Handles the ValidateSymbol event of the pranaOptionCtrl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}"/> instance containing the event data.</param>
        private void pranaOptionCtrl_ValidateSymbol(object sender, EventArgs<string, string> e)
        {
            try
            {
                pranaSymbolCtrl_SymbolEntered(sender, e);
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
        /// Handles the Leave event of the nmrcQuantity control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void nmrcQuantity_Leave(object sender, EventArgs e)
        {
            try
            {
                if (nmrcQuantity.Value == 0.0m)
                {
                    UpdateNotionalValueOnCaption();
                }
                if (shortLocateList1.grdShortLocateList.Rows.Count > 0)
                {
                    shortLocateList1.grdShortLocateList.Rows[0].Cells["BorrowQuantity"].Value = nmrcQuantity.Value;
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
        /// Handles the ValueChanged event of the nmrcQuantity control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void nmrcQuantity_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateNotionalValueOnCaption();
                UpdateCaption();
                if (nmrcTargetQuantity.Enabled)
                    SetTargetQuantity();
                if ((ttPresenter.AssetID == (int)AssetCategory.FX || ttPresenter.AssetID == (int)AssetCategory.FXForward || ttPresenter.AssetID == (int)AssetCategory.Forex))
                {
                    decimal position = ((PranaNumericUpDown)sender).Value;
                    if (position == 0 || Price == 0)
                    {
                        nmrcCCA.Value = (decimal)0.0;
                        return;
                    }

                    if (OrderSide == FIXConstants.SIDE_Buy || OrderSide == "9" || OrderSide == "10")
                        position = position * -1;
                    if (ttPresenter.LeadCurrencyId.ToString().Equals(DealIn))
                        nmrcCCA.Value = Math.Round(position * Price, 4);
                    else
                        nmrcCCA.Value = Math.Round(position / Price, 4);
                }
                SetNotionalQuantityMessage();
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
        /// Handles the ValueChanged event of the nmrcTargetQuantity control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void nmrcTargetQuantity_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ttPresenter.EnterTargetQuantityInPercentage)
                {
                    double targetQuantity = 0.0;
                    Double.TryParse(nmrcTargetQuantity.Value.ToString(), out targetQuantity);
                    if (targetQuantity > ApplicationConstants.PERCENTAGEVALUE)
                    {
                        SetLabelMessage(TradingTicketConstants.MSG_TARGET_QUANTITY_CAN_NOT_BE_GREATHER_THAN_HUNDRED);
                        nmrcTargetQuantity.Value = ApplicationConstants.PERCENTAGEVALUE;
                    }
                }
                UpdateNotionalValueOnSwapControl();
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

        private void UpdateNotionalValueOnSwapControl()
        {
            try
            {
                if (chkBoxSwap.Checked)
                {
                    double targetQuantity = 0.0;
                    Double.TryParse(nmrcTargetQuantity.Text, out targetQuantity);
                    double targetQuantityForNotioanl = 0.0;

                    if (ttPresenter.EnterTargetQuantityInPercentage)
                    {
                        Double.TryParse(nmrcQuantity.Text, out targetQuantityForNotioanl);
                        if (targetQuantity > ApplicationConstants.PERCENTAGEVALUE)
                        {
                            nmrcTargetQuantity.Value = ApplicationConstants.PERCENTAGEVALUE;
                        }
                        targetQuantityForNotioanl = targetQuantityForNotioanl * targetQuantity / ApplicationConstants.PERCENTAGEVALUE;
                    }
                    else
                    {
                        targetQuantityForNotioanl = targetQuantity;
                    }

                    double avgPrice = 0.0;
                    Double.TryParse(nmrcPrice.Text, out avgPrice);
                    ctrlSwapParameters1.UpdateNotionalValue(avgPrice * targetQuantityForNotioanl);
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
        /// Ons the l1 response.
        /// </summary>
        /// <param name="l1Data">The l1 data.</param>
        private void onL1Response(SymbolData l1Data)
        {
            try
            {
                if (l1Data != null && l1Data.LastPrice > 0)
                {

                    if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                        ttPresenter.MarketPrice = l1Data.LastPrice;

                    UpdateNotionalValueOnCaption();
                    UpdateCaption();
                    //In case of options, the prices are not sent by esignal when symbol is validated, instead they are obtained in continous data request, so updating the prices if market price is 0, PRANA-26800
                    decimal currentPrice = decimal.MinValue;
                    if (decimal.TryParse(nmrcPrice.Text, out currentPrice) && currentPrice == 0.0M)
                        UpdatePricesFromLiveFeed(l1Data.Ask, l1Data.Bid, l1Data.LastPrice);
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

        private void toggleRoundLotSwitch(bool isUseRoundLot)
        {
            try
            {
                nmrcQuantity.Increment = isUseRoundLot ? ttPresenter.SecmasterObj.RoundLot : increment;
                nmrcTargetQuantity.Increment = isUseRoundLot ? ttPresenter.SecmasterObj.RoundLot : increment;
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
        /// Handles the SymbolEntered event of the pranaSymbolCtrl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}" /> instance containing the event data.</param>
        private void pranaSymbolCtrl_SymbolEntered(object sender, EventArgs<string, string> e)
        {

            try
            {
                if (!IsHandleCreated)
                {
                    CreateHandle();
                }
                if (UIValidation.GetInstance().validate(this))
                {
                    if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: TradingTicket.pranaSymbolCtrl_SymbolEntered() > Symbol entered by user on TT: {0}, Time: {1}", e.Value, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                    }

                    if (ttPresenter.IncomingOrderRequest != null && ttPresenter.IncomingOrderRequest.Symbol != e.Value && ttPresenter.IncomingOrderRequest.BloombergSymbol != null && ttPresenter.IncomingOrderRequest.BloombergSymbol.ToUpper() != e.Value
                       && ttPresenter.IncomingOrderRequest.FactSetSymbol != null && ttPresenter.IncomingOrderRequest.FactSetSymbol != e.Value && ttPresenter.IncomingOrderRequest.ActivSymbol != null && ttPresenter.IncomingOrderRequest.ActivSymbol.ToUpper() != e.Value && ttPresenter.IncomingOrderRequest.TransactionSource == TransactionSource.PST)
                    {
                        DialogResult userResponse = MessageBox.Show(TradingTicketConstants.MSG_REMOVE_PTT, "TradingTicket", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (userResponse == DialogResult.No)
                        {
                            pranaSymbolCtrl.Text = ttPresenter.IncomingOrderRequest.Symbol;
                            pranaSymbolCtrl.PrevSymbolEntered = ttPresenter.IncomingOrderRequest.Symbol;
                            return;
                        }
                    }
                    pranaSymbolCtrl.PrevSymbolEntered = e.Value;
                    ttPresenter.UnderlyingSymbol = e.Value2;
                    RefreshControl(false);
                    TradingTicketEnabled(false);

                    if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: TradingTicket.pranaSymbolCtrl_SymbolEntered() > Symbol validation request initiated from TT for Symbol: {0}, Time: {1}", e.Value, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                    }
                    ttPresenter.SendValidatedSymbolToSM(e);

                    if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: TradingTicket.pranaSymbolCtrl_SymbolEntered() > TT level1 strip request initiated from TT for Symbol: {0}, Time: {1}", e.Value, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                    }

                    if (otcTradeViewUI != null)
                    {
                        otcTradeViewUI.Close();
                        otcTradeViewUI = null;
                        _otcParameters = null;
                    }
                    if (!LoginUser.MarketDataTypes.Contains(LiveFeedConstants.LevelOne))
                    {
                        oneSymbolL1Strip.Visible = true;
                        oneSymbolL1Strip.setL1InvisibleExceptRoundLot();
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
        /// Handles the SymbolSearch event of the pranaSymbolCtrl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}" /> instance containing the event data.</param>
        private void pranaSymbolCtrl_SymbolSearch(object sender, EventArgs<string> e)
        {
            try
            {
                ttPresenter.SymbolSearch(e);
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
        /// Processes a command key.
        /// </summary>
        /// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the Win32 message to process.</param>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
        /// <returns>
        /// true if the keystroke was processed and consumed by the control; otherwise, false to allow further processing.
        /// </returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                switch (keyData)
                {
                    case (Keys.Alt | Keys.C):
                        if (btnCreateOrder.Enabled)
                            btnCreateOrder_Click(btnCreateOrder, EventArgs.Empty);
                        return true;
                    case (Keys.Alt | Keys.D):
                        if (btnDoneAway.Enabled)
                            btnDoneAway_Click(btnDoneAway, null);
                        return true;
                    case (Keys.Alt | Keys.S):
                        if (btnSend.Enabled)
                            btnSend_Click(btnSend, null);
                        return true;
                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        /// <summary>
        /// Repositions the controls based on asset.
        /// </summary>
        /// <param name="secMasterObj">The sec master object.</param>
        private void RepositionControlsBasedOnAsset(SecMasterBaseObj secMasterObj)
        {
            try
            {
                switch (secMasterObj.AssetCategory)
                {
                    case AssetCategory.FX:
                    case AssetCategory.FXForward:
                        lblDealIn.Visible = true;
                        cmbDealIn.Visible = true;
                        chkBoxSwap.Visible = false;
                        tblPnlTTMainControls.Controls.Remove(PnlTargetQuantity);
                        tblPnlTTMainControls.Controls.Remove(nmrcTargetQuantity);
                        tblPnlTTMainControls.Controls.Remove(lblDealIn);
                        tblPnlTTMainControls.Controls.Remove(cmbDealIn);
                        tblPnlTTMainControls.Controls.Remove(lblOrderSide);
                        tblPnlTTMainControls.Controls.Remove(cmbOrderSide);
                        tblPnlTTMainControls.Controls.Remove(pnlNotionalQuantity);
                        tblPnlTTMainControls.Controls.Remove(nmrcQuantity);
                        tblPnlTTMainControls.Controls.Add(lblDealIn, 0, 1);
                        tblPnlTTMainControls.Controls.Add(cmbDealIn, 0, 2);
                        tblPnlTTMainControls.Controls.Add(lblOrderSide, 1, 1);
                        tblPnlTTMainControls.Controls.Add(cmbOrderSide, 1, 2);
                        tblPnlTTMainControls.Controls.Add(pnlNotionalQuantity, 2, 1);
                        tblPnlTTMainControls.Controls.Add(nmrcQuantity, 2, 2);

                        SetTTControlsBasedonPref(false);

                        tblPnlTTMainControls.Controls.Remove(lblVenue);
                        tblPnlTTMainControls.Controls.Remove(cmbVenue);


                        tblPnlOthers.Controls.Remove(lblVenue);
                        tblPnlOthers.Controls.Remove(cmbVenue);
                        tblPnlOthers.Controls.Remove(lblExecutionInstructions);
                        tblPnlOthers.Controls.Remove(cmbExecutionInstructions);
                        tblPnlOthers.Controls.Remove(cmbHandlingInstructions);
                        tblPnlOthers.Controls.Remove(lblHandlingInstructions);
                        tblPnlOthers.Controls.Remove(lblTradingAccount);
                        tblPnlOthers.Controls.Remove(cmbTradingAccount);


                        tblPnlOthers.Controls.Add(lblVenue, 0, 0);
                        tblPnlOthers.Controls.Add(cmbVenue, 0, 1);
                        tblPnlOthers.Controls.Add(lblExecutionInstructions, 1, 0);
                        tblPnlOthers.Controls.Add(cmbExecutionInstructions, 1, 1);
                        tblPnlOthers.Controls.Add(cmbHandlingInstructions, 2, 1);
                        tblPnlOthers.Controls.Add(lblHandlingInstructions, 2, 0);
                        tblPnlOthers.Controls.Add(lblTradingAccount, 3, 0);
                        tblPnlOthers.Controls.Add(cmbTradingAccount, 3, 1);


                        tblPnlTTMainControls.Controls.Remove(lblStrategy);
                        tblPnlTTMainControls.Controls.Remove(lblTradeDate);
                        tblPnlTTMainControls.Controls.Remove(cmbStrategy);
                        tblPnlTTMainControls.Controls.Remove(dtTradeDate);
                        tblPnlTTMainControls.Controls.Remove(lblFXRate);
                        tblPnlTTMainControls.Controls.Remove(nmrcFXRate);
                        tblPnlTTMainControls.Controls.Remove(lblFxOperator);
                        tblPnlTTMainControls.Controls.Remove(cmbFxOperator);

                        tblPnlTTMainControls.Controls.Add(lblStrategy, 0, 5);
                        tblPnlTTMainControls.Controls.Add(lblTradeDate, 1, 5);
                        tblPnlTTMainControls.Controls.Add(cmbStrategy, 0, 6);
                        tblPnlTTMainControls.Controls.Add(dtTradeDate, 1, 6);
                        tblPnlTTMainControls.Controls.Add(lblFXRate, 2, 5);
                        tblPnlTTMainControls.Controls.Add(nmrcFXRate, 2, 6);
                        tblPnlTTMainControls.Controls.Add(lblFxOperator, 3, 5);
                        tblPnlTTMainControls.Controls.Add(cmbFxOperator, 3, 6);

                        tblPnlTTMainControls.Controls.Add(lblCCA, 4, 5);
                        tblPnlTTMainControls.Controls.Add(nmrcCCA, 4, 6);
                        //nmrcTargetQuantity.TabIndex = 29;
                        this.cmbVenue.TabIndex = 1;
                        nmrcQuantity.TabIndex = 8;
                        cmbOrderSide.TabIndex = 7;
                        cmbDealIn.TabIndex = 6;
                        bottomTabControl.Tabs[TradingTicketConstants.TAB_SETTLEMENT_KEY].Visible = false;
                        break;
                    case AssetCategory.Equity:
                        lblDealIn.Visible = false;
                        cmbDealIn.Visible = false;
                        if (!_isNewOTCWorkflowEnabled)
                            chkBoxSwap.Visible = true;
                        if (ttPresenter.CurrencyId != ttPresenter.CompanyBaseCurrencyID)
                        {
                            bottomTabControl.Tabs[TradingTicketConstants.TAB_SETTLEMENT_KEY].Visible = true;
                        }
                        else
                        {
                            bottomTabControl.Tabs[TradingTicketConstants.TAB_SETTLEMENT_KEY].Visible = false;
                        }
                        tblPnlTTMainControls.Controls.Remove(lblDealIn);
                        tblPnlTTMainControls.Controls.Remove(cmbDealIn);
                        tblPnlTTMainControls.Controls.Remove(lblCCA);
                        tblPnlTTMainControls.Controls.Remove(nmrcCCA);
                        tblPnlTTMainControls.Controls.Add(lblOrderSide, 0, 1);
                        tblPnlTTMainControls.Controls.Add(cmbOrderSide, 0, 2);
                        tblPnlTTMainControls.Controls.Add(pnlNotionalQuantity, 1, 1);
                        tblPnlTTMainControls.Controls.Add(nmrcQuantity, 1, 2);

                        if (IsShowTargetQTY)
                        {
                            SetTTControlsBasedonPref(true);
                        }
                        else
                        {
                            SetTTControlsBasedonPref(false);
                        }

                        tblPnlTTMainControls.Controls.Add(lblDealIn, 4, 5);
                        tblPnlTTMainControls.Controls.Add(cmbDealIn, 4, 6);
                        cmbOrderSide.TabIndex = 6;
                        nmrcQuantity.TabIndex = 7;
                        this.cmbVenue.TabIndex = 11;
                        //nmrcTargetQuantity.TabIndex = 8;
                        cmbDealIn.TabIndex = 29;
                        break;
                    default:
                        lblDealIn.Visible = false;
                        cmbDealIn.Visible = false;
                        chkBoxSwap.Visible = false;
                        if (ttPresenter.CurrencyId != ttPresenter.CompanyBaseCurrencyID)
                        {
                            bottomTabControl.Tabs[TradingTicketConstants.TAB_SETTLEMENT_KEY].Visible = true;
                        }
                        else
                        {
                            bottomTabControl.Tabs[TradingTicketConstants.TAB_SETTLEMENT_KEY].Visible = false;
                        }
                        tblPnlTTMainControls.Controls.Remove(lblDealIn);
                        tblPnlTTMainControls.Controls.Remove(cmbDealIn);
                        tblPnlTTMainControls.Controls.Remove(lblCCA);
                        tblPnlTTMainControls.Controls.Remove(nmrcCCA);
                        tblPnlTTMainControls.Controls.Add(lblOrderSide, 0, 1);
                        tblPnlTTMainControls.Controls.Add(cmbOrderSide, 0, 2);
                        tblPnlTTMainControls.Controls.Add(pnlNotionalQuantity, 1, 1);
                        tblPnlTTMainControls.Controls.Add(nmrcQuantity, 1, 2);

                        if (IsShowTargetQTY)
                        {

                            SetTTControlsBasedonPref(true);
                        }
                        else
                        {
                            SetTTControlsBasedonPref(false);
                        }
                        tblPnlTTMainControls.Controls.Add(lblDealIn, 4, 5);
                        tblPnlTTMainControls.Controls.Add(cmbDealIn, 4, 6);
                        cmbOrderSide.TabIndex = 6;
                        nmrcQuantity.TabIndex = 7;
                        //nmrcTargetQuantity.TabIndex = 8;
                        this.cmbVenue.TabIndex = 11;
                        cmbDealIn.TabIndex = 29;
                        break;
                }
                StringBuilder sbDescription = new StringBuilder();
                sbDescription.Append(secMasterObj.LongName);
                ChangeFormTitle(sbDescription.ToString());
                if (!_isNewOTCWorkflowEnabled && CachedDataManager.GetInstance.IsShowMasterFundonTT())
                    this.lblHiddenStrategyAdjust.Size = new System.Drawing.Size(200, 90);
                else if (_isNewOTCWorkflowEnabled && !CachedDataManager.GetInstance.IsShowMasterFundonTT())
                    lblHiddenStrategyAdjust.Size = new System.Drawing.Size(200, 100);
                else if (!_isNewOTCWorkflowEnabled && !CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    this.lblHiddenStrategyAdjust.Size = new System.Drawing.Size(200, 137);
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
        /// Sets the color of the buttons.
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnSend.ButtonStyle = UIElementButtonStyle.Button3D;
                btnSend.BackColor = Color.FromArgb(104, 156, 46);
                btnSend.ForeColor = Color.White;
                btnSend.UseAppStyling = false;
                btnSend.UseOsThemes = DefaultableBoolean.False;

                btnCreateOrder.ButtonStyle = UIElementButtonStyle.Button3D;
                btnCreateOrder.BackColor = Color.FromArgb(55, 67, 85);
                btnCreateOrder.ForeColor = Color.White;
                btnCreateOrder.UseAppStyling = false;
                btnCreateOrder.UseOsThemes = DefaultableBoolean.False;

                btnDoneAway.ButtonStyle = UIElementButtonStyle.Button3D;
                btnDoneAway.BackColor = Color.FromArgb(72, 99, 160);
                btnDoneAway.ForeColor = Color.White;
                btnDoneAway.UseAppStyling = false;
                btnDoneAway.UseOsThemes = DefaultableBoolean.False;

                btnBrokerConnectionStatus.BackColor = Color.FromArgb(140, 5, 5);
                btnBrokerConnectionStatus.ForeColor = Color.White;
                btnBrokerConnectionStatus.UseAppStyling = false;
                btnBrokerConnectionStatus.UseOsThemes = DefaultableBoolean.False;

                btnReplace.ButtonStyle = UIElementButtonStyle.Button3D;
                btnReplace.BackColor = Color.FromArgb(140, 5, 5);
                btnReplace.ForeColor = Color.White;
                btnReplace.UseAppStyling = false;
                btnReplace.UseOsThemes = DefaultableBoolean.False;

                btnShortLocateList.ButtonStyle = UIElementButtonStyle.Button3D;
                btnShortLocateList.BackColor = Color.DimGray;
                btnShortLocateList.ForeColor = Color.White;
                btnShortLocateList.UseAppStyling = false;
                btnShortLocateList.UseOsThemes = DefaultableBoolean.False;

                BackColor = Color.FromArgb(232, 232, 232);
                oneSymbolL1Strip.ForeColor = Color.Black;
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
        /// Sets the controls visibility based on cancel or replace message.
        /// </summary>
        /// <param name="or">The or.</param>
        /// <param name="transfertraderules">The transfertraderules.</param>
        private void SetControlsVisibilityBasedOnCancelOrReplaceMessage(OrderSingle or, TranferTradeRules transfertraderules)
        {
            try
            {
                if (or.MsgType == FIXConstants.MSGOrderCancelReplaceRequest || or.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrder)
                {
                    chkBoxOption.Checked = false;
                    pranaSymbolCtrl.Enabled = false;
                    chkBoxSwap.Visible = true;
                    if (or.SwapParameters != null)
                        ctrlSwapParameters1.SetSwapParams(or);
                    if ((((OrderFields.PranaMsgTypes)or.PranaMsgType) == OrderFields.PranaMsgTypes.ORDManual) || (((OrderFields.PranaMsgTypes)or.PranaMsgType) == OrderFields.PranaMsgTypes.ORDManualSub))
                    {
                        cmbBroker.Enabled = true;
                        cmbVenue.Enabled = true;
                        nmrcTargetQuantity.Enabled = false;
                        btnTargetQuantityPercentage.Enabled = false;
                        dtTradeDate.Enabled = false;
                        nmrcPrice.Enabled = false;
                        nmrcTargetQuantity.Maximum = (int)or.CumQty;
                        nmrcTargetQuantity.Value = (int)or.CumQty;
                        cmbSettlementCurrency.Enabled = false;
                        nmrcSettlementPrice.Enabled = false;
                    }
                    if (transfertraderules.IsApplyLimitRulesForReplacingStagedOrders &&
                        (((OrderFields.PranaMsgTypes)or.PranaMsgType) == OrderFields.PranaMsgTypes.ORDStaged))
                    {
                        cmbOrderType.Enabled = false;
                        if (or.OrderType.Equals(TradingTicketConstants.LIT_LIMIT))
                        {
                            if (or.OrderSide.Equals(TradingTicketConstants.LIT_BUY) ||
                                or.OrderSide.Equals(TradingTicketConstants.LIT_BUY_TO_COVER) ||
                                or.OrderSide.Equals(TradingTicketConstants.LIT_BUY_TO_CLOSE))
                            {
                                nmrcLimit.Maximum = Convert.ToDecimal(or.Price);
                            }
                            else
                            {
                                nmrcLimit.Minimum = Convert.ToDecimal(or.Price);
                            }
                        }
                    }
                    //Order text is CreateSub for Live trade and is blank for Stage sub order
                    else if (transfertraderules.IsApplyLimitRulesForReplacingSubOrders &&
                             ((((OrderFields.PranaMsgTypes)or.PranaMsgType) == OrderFields.PranaMsgTypes.ORDManualSub) ||
                              (((OrderFields.PranaMsgTypes)or.PranaMsgType) == OrderFields.PranaMsgTypes.ORDNewSubChild)))
                    // ORDManualSub = 5, ORDNewSub=2
                    {
                        cmbOrderType.Enabled = false;
                        if (or.OrderType.Equals(TradingTicketConstants.LIT_LIMIT))
                        {
                            if (or.OrderSide.Equals(TradingTicketConstants.LIT_BUY) ||
                                or.OrderSide.Equals(TradingTicketConstants.LIT_BUY_TO_COVER) ||
                                or.OrderSide.Equals(TradingTicketConstants.LIT_BUY_TO_CLOSE))
                            {
                                nmrcLimit.Maximum = Convert.ToDecimal(or.Price);
                            }
                            else
                            {
                                nmrcLimit.Minimum = Convert.ToDecimal(or.Price);
                            }
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

        /// <summary>
        /// Sets the type of the controls visibility based on message.
        /// </summary>
        /// <param name="or">The or.</param>
        /// <param name="transfertraderules">The transfertraderules.</param>
        private void SetControlsVisibilityBasedOnMessageType(OrderSingle or, TranferTradeRules transfertraderules)
        {
            try
            {
                if (or.MsgType != FIXConstants.MSGOrderCancelReplaceRequest && or.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX &&
                    or.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew && or.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrder)
                    return;

                btnCreateOrder.Visible = false;
                btnCreateOrder.Enabled = false;
                btnSend.Visible = false;
                btnSend.Enabled = false;
                nmrcTargetQuantity.Enabled = false;
                btnTargetQuantityPercentage.Enabled = false;
                btnReplace.BackgroundImage = null;
                btnReplace.BackColor = Color.FromArgb(140, 5, 5);
                btnDoneAway.Visible = false;
                btnDoneAway.Enabled = false;
                btnReplace.Visible = true;
                btnReplace.Enabled = true;
                btnDoneAway.Text = TradingTicketConstants.BUTTON_CAPTION_REPLACE;
                ttPresenter.TicketType = TradingTicketType.None;
                cmbOrderSide.Enabled = false;
                chkBoxOption.Checked = false;
                pranaSymbolCtrl.Enabled = false;
                //To Handle case, If Order Message type is Replace Request and Prana Message type is Stagged
                if (or.MsgType == FIXConstants.MSGOrderCancelReplaceRequest && (OrderFields.PranaMsgTypes)or.PranaMsgType == OrderFields.PranaMsgTypes.ORDStaged)
                    return;

                cmbBroker.Enabled = false;
                cmbVenue.Enabled = false;
                cmbTradingAccount.Enabled = false;
                cmbAllocation.Enabled = false;
                btnAccountQty.Enabled = false;
                cmbStrategy.Enabled = false;
                if (transfertraderules.IsApplyLimitRulesForReplacingOtherOrders && or.PranaMsgType != 5 && or.PranaMsgType != 3)
                {
                    bool boolLimitMaxMinValue = true;
                    if (or.PranaMsgType == 2)
                    {
                        //Order text is CreateSub for Live trade and is blank for Stage sub order
                        boolLimitMaxMinValue = or.InternalComments.Equals(TradingTicketConstants.LIT_CREATE_SUB);
                    }
                    if (boolLimitMaxMinValue)
                    {
                        cmbOrderType.Enabled = false;
                        if (or.OrderType.Equals(TradingTicketConstants.LIT_LIMIT))
                        {
                            if (or.OrderSide.Equals(TradingTicketConstants.LIT_BUY) || or.OrderSide.Equals(TradingTicketConstants.LIT_BUY_TO_COVER) ||
                                or.OrderSide.Equals(TradingTicketConstants.LIT_BUY_TO_CLOSE))
                            {
                                nmrcLimit.Maximum = Convert.ToDecimal(or.Price);
                            }
                        }
                    }
                }
                cmbTradeAttribute1.Enabled = false;
                cmbTradeAttribute2.Enabled = false;
                cmbTradeAttribute3.Enabled = false;
                cmbTradeAttribute4.Enabled = false;
                cmbTradeAttribute5.Enabled = false;
                cmbTradeAttribute6.Enabled = false;
                nmrcSettlementPrice.Enabled = false;
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
        /// Sets the type of the controls visibility based on prana message.
        /// </summary>
        /// <param name="or">The or.</param>
        /// <param name="transfertraderules">The transfertraderules.</param>
        private void SetControlsVisibilityBasedOnPranaMessageType(OrderSingle or, TranferTradeRules transfertraderules)
        {
            try
            {
                switch ((OrderFields.PranaMsgTypes)or.PranaMsgType)
                {
                    case OrderFields.PranaMsgTypes.ORDNewSub:
                    case OrderFields.PranaMsgTypes.ORDNewSubChild:
                    case OrderFields.PranaMsgTypes.ORDManualSub:
                        UpdateAccountComboValue(or.Level1ID, (ChangeType)(or.ChangeType));

                        if (or.MsgType == FIXConstants.MSGOrderCancelReplaceRequest)
                        {
                            if (or.Account != OrderFields.PROPERTY_MULTIPLE)
                            {
                                int accountId = CachedDataManager.GetInstance.GetAccountID(or.Account);
                                if (accountId != or.Level1ID && accountId != int.MinValue)
                                    cmbAllocation.Text = or.Account;
                                else if (or.Account.Equals(OrderFields.PROPERTY_DASH))
                                    cmbAllocation.Text = ApplicationConstants.C_LIT_UNALLOCATED;
                            }
                            else
                            {
                                bool isManualPref = or.AllocationSchemeName.Contains(TradingTicketConstants.ALLOCATION_PREF_CUSTOM);
                                if (!isManualPref && or.AllocationSchemeName.Equals(TradingTicketType.Manual.ToString()))
                                    cmbAllocation.Text = or.Account;
                            }
                        }
                        if (or.Level2ID == int.MinValue)
                            cmbStrategy.Value = null;
                        else
                            cmbStrategy.Value = or.Level2ID;
                        cmbTradingAccount.Enabled = false;
                        chkBoxOption.Checked = false;
                        pranaSymbolCtrl.Enabled = false;
                        if (or.SwapParameters == null)
                        {
                            chkBoxSwap.Visible = false;
                            chkBoxSwap.Enabled = false;
                        }
                        if ((OrderFields.PranaMsgTypes)or.PranaMsgType == OrderFields.PranaMsgTypes.ORDManualSub)
                        {
                            cmbBroker.Enabled = true;
                            cmbVenue.Enabled = true;
                        }
                        cmbOrderSide.Enabled = false;
                        btnCreateOrder.Enabled = false;

                        //https://jira.nirvanasolutions.com:8443/browse/PRANA-21240
                        if (transfertraderules.IsAccountChange && or.MsgType != FIXConstants.MSGOrderCancelReplaceRequest)
                        {
                            cmbAllocation.Enabled = true;
                            btnAccountQty.Enabled = true;
                            cmbFunds.Enabled = true;
                        }
                        else
                        {
                            cmbAllocation.Enabled = false;
                            btnAccountQty.Enabled = false;
                            cmbFunds.Enabled = false;
                        }

                        //https://jira.nirvanasolutions.com:8443/browse/PRANA-20738
                        if (transfertraderules.IsVenueCPChange && or.MsgType != FIXConstants.MSGOrderCancelReplaceRequest)
                        {
                            cmbBroker.Enabled = true;
                            cmbVenue.Enabled = true;
                            cmbFunds.Enabled = true;
                        }
                        else
                        {
                            cmbBroker.Enabled = false;
                            cmbVenue.Enabled = false;
                            cmbFunds.Enabled = false;
                        }
                        cmbTradingAccount.Enabled = transfertraderules.IsTradingAccChange;
                        cmbTIF.Enabled = transfertraderules.IsTIFChange;
                        cmbHandlingInstructions.Enabled = transfertraderules.IsHandlingInstrChange;
                        cmbStrategy.Enabled = transfertraderules.IsStrategyChange;
                        SetStrategyComboVisibility();
                        cmbExecutionInstructions.Enabled = transfertraderules.IsExecutionInstrChange;
                        if (transfertraderules.IsAllowUserToChangeOrderType &&
                            or.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingReplace)
                        {
                            cmbOrderType.Enabled = true;
                        }
                        else
                        {
                            if (or.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingReplace)
                            {
                                cmbOrderType.Enabled = false;
                                if (or.OrderType.Equals(TradingTicketConstants.LIT_LIMIT))
                                {
                                    if (or.OrderSide.Equals(TradingTicketConstants.LIT_BUY) ||
                                        or.OrderSide.Equals(TradingTicketConstants.LIT_BUY_TO_COVER) ||
                                        or.OrderSide.Equals(TradingTicketConstants.LIT_BUY_TO_CLOSE))
                                    {
                                        nmrcLimit.Maximum = Convert.ToDecimal(or.Price);
                                    }
                                    else
                                    {
                                        nmrcLimit.Minimum = Convert.ToDecimal(or.Price);
                                    }
                                }
                            }
                        }
                        nmrcQuantity.Maximum = Convert.ToDecimal(or.Quantity);
                        if (or.MsgType == FIXConstants.MSGOrder)
                        {
                            nmrcQuantity.Maximum = Max_Qty;
                            nmrcQuantity.Value = Max_Qty;
                            nmrcTargetQuantity.Maximum = Max_Qty;
                            nmrcTargetQuantity.Value = Max_Qty;
                        }
                        else
                        {
                            nmrcQuantity.Value = Convert.ToDecimal(or.Quantity);
                            nmrcTargetQuantity.Maximum = Convert.ToDecimal(or.CumQty);
                            nmrcTargetQuantity.Value = Convert.ToDecimal(or.CumQty);
                        }
                        //    nmrcQuantity.Enabled = false;
                        break;

                    case OrderFields.PranaMsgTypes.ORDStaged:
                        if ((OrderFields.PranaMsgTypes)or.PranaMsgType != OrderFields.PranaMsgTypes.ORDCancelStaged)
                        {
                            this.cmbBroker.Enabled = true;
                            cmbVenue.Enabled = true;
                        }
                        if (or.Quantity != double.Epsilon)
                        {
                            nmrcQuantity.Value = Convert.ToDecimal(or.Quantity);
                        }
                        if (or.CumQty != double.Epsilon)
                        {
                            nmrcTargetQuantity.Maximum = Convert.ToDecimal(or.Quantity) - Convert.ToDecimal(or.UnsentQty);
                            nmrcTargetQuantity.Value = Convert.ToDecimal(or.Quantity) - Convert.ToDecimal(or.UnsentQty);
                        }
                        UpdateAccountComboValue(or.Level1ID, (ChangeType)(or.ChangeType));
                        break;

                    case OrderFields.PranaMsgTypes.InternalOrder:
                        Max_Qty = 999999999;
                        if (or.Quantity != double.Epsilon && or.Quantity != 0.0)
                        {
                            nmrcQuantity.Value = Convert.ToDecimal(or.Quantity);
                        }
                        if (or.CumQty != double.Epsilon)
                        {
                            if (or.TransactionSource != TransactionSource.PST)
                            {
                                nmrcTargetQuantity.Maximum = Convert.ToDecimal(or.CumQty);
                                nmrcTargetQuantity.Value = Convert.ToDecimal(or.CumQty);
                            }
                            else
                            {
                                nmrcTargetQuantity.Maximum = Convert.ToDecimal(or.Quantity);
                                nmrcTargetQuantity.Value = Convert.ToDecimal(or.Quantity);
                            }
                        }
                        ttPresenter.MarketPrice = or.Price;
                        if (or.Level2ID != int.MinValue)
                        {
                            cmbStrategy.Value = or.Level2ID;
                        }
                        else
                        {
                            cmbStrategy.Value = null;
                        }
                        if (or.Level1ID != int.MinValue)
                        {
                            UpdateAccountComboValue(or.Level1ID, (ChangeType)(or.ChangeType), or.Account);
                        }
                        // the following code to differentiate between messages coming from watchlist etc.
                        // and messages coming from already existing orders (Blotter)
                        // required to find out what account strat. shud be displayed
                        // distinguish between int.minval for new orders vs. unallocated orders
                        if (or.ClOrderID != string.Empty)
                        {
                            UpdateAccountComboValue(or.Level1ID, (ChangeType)(or.ChangeType));
                            if (or.Level2ID != int.MinValue)
                            {
                                cmbStrategy.Value = or.Level2ID;
                            }
                            else
                            {
                                cmbStrategy.Value = null;
                            }
                        }
                        if (or.TransactionSource == TransactionSource.PM)
                        {
                            nmrcQuantity.Value = Convert.ToDecimal(ttPresenter.TradingTicketUiPrefs.Quantity);
                            or.CumQty = Convert.ToDouble(ttPresenter.TradingTicketUiPrefs.Quantity);
                        }
                        break;

                    default:
                        Max_Qty = 999999999;
                        if (or.Quantity != double.Epsilon)
                        {
                            nmrcQuantity.Value = Convert.ToDecimal(or.Quantity);
                        }
                        if (or.CumQty != double.Epsilon)
                        {
                            nmrcTargetQuantity.Maximum = Convert.ToDecimal(or.CumQty);
                            nmrcTargetQuantity.Value = Convert.ToDecimal(or.CumQty);
                        }
                        UpdateAccountComboValue(or.Level1ID, (ChangeType)(or.ChangeType));
                        if (or.Level2ID != int.MinValue)
                        {
                            cmbStrategy.Value = or.Level2ID;
                        }
                        else
                        {
                            cmbStrategy.Value = null;
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
        }

        /// <summary>
        /// Handles the CounterPartyAvailabilityUpdate event of the TradingTicket control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{CounterPartyDetails, System.Boolean}"/> instance containing the event data.</param>
        void TradingTicket_CounterPartyStatusUpdate(object sender, EventArgs<CounterPartyDetails> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            TradingTicket_CounterPartyStatusUpdate(sender, e);
                        }));
                    }
                    else
                    {
                        if (e.Value != null && e.Value.OriginatorType != PranaServerConstants.OriginatorType.Allocation && e.Value.CounterPartyID == ttPresenter.CounterPartyId)
                        {
                            lblBrokerMessage.Text = e.Value.ConnStatus == PranaInternalConstants.ConnectionStatus.CONNECTED ? string.Empty : TradingTicketConstants.MSG_FIX_CONNECTION_DOWN;

                            if (e.Value.ConnStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                                btnBrokerConnectionStatus.BackColor = Color.FromArgb(104, 156, 46);
                            else
                                btnBrokerConnectionStatus.BackColor = Color.FromArgb(140, 5, 5);
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
        /// <summary>
        /// Sets the index of commission ComboBox.
        /// </summary>
        /// <param name="or">The or.</param>
        private void SetIndexOfCommissionComboBox(OrderSingle or)
        {
            try
            {
                if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbCommissionBasis.ValueList, or.CalcBasis.ToString()))
                {
                    cmbCommissionBasis.SelectedIndex = ValueListUtilities.GetListIndexFromValue(cmbCommissionBasis.ValueList, or.CalcBasis.ToString());
                }
                else
                {
                    cmbCommissionBasis.SelectedIndex =
                       TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis.ContainsKey(or.CounterPartyID)
                            ? TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis[or.CounterPartyID]
                            : 4;
                }

                if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbSoftCommissionBasis.ValueList, or.SoftCommissionCalcBasis.ToString()))
                {
                    cmbSoftCommissionBasis.SelectedIndex = ValueListUtilities.GetListIndexFromValue(cmbSoftCommissionBasis.ValueList,
                        or.SoftCommissionCalcBasis.ToString());
                }
                else
                {
                    cmbSoftCommissionBasis.SelectedIndex =
                        TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseSoftCommissionBasis.ContainsKey(or.CounterPartyID)
                            ? TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis[or.CounterPartyID]
                            : 4;
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
        /// Sets the target quantity.
        /// </summary>
        private void SetTargetQuantity()
        {
            try
            {
                if (!ttPresenter.IsTargetQuantitySameAsTotalQty) return;
                if (nmrcTargetQuantity.Enabled)
                {
                    if (ttPresenter.EnterTargetQuantityInPercentage)
                    {
                        nmrcTargetQuantity.Maximum = ApplicationConstants.PERCENTAGEVALUE;
                        nmrcTargetQuantity.Value = ApplicationConstants.PERCENTAGEVALUE;
                        UpdateNotionalValueOnSwapControl();
                    }
                    else
                    {
                        if (nmrcQuantity.Value > nmrcTargetQuantity.Maximum)
                        {
                            nmrcTargetQuantity.DecimalPlaces = nmrcQuantity.DecimalPlaces;
                            nmrcTargetQuantity.Maximum = nmrcQuantity.Value;
                            nmrcTargetQuantity.Value = nmrcQuantity.Value;
                            //SetLabelMessage(TradingTicketConstants.MSG_QUANTITY_CAN_NOT_BE_GREATHER_THAN + nmrcTargetQuantity.MaxValue);
                        }
                        else
                        {
                            nmrcTargetQuantity.DecimalPlaces = nmrcQuantity.DecimalPlaces;
                            nmrcTargetQuantity.Maximum = nmrcQuantity.Value;
                            nmrcTargetQuantity.Value = nmrcQuantity.Value;
                        }
                    }
                    //nmrcTargetQuantity.Focus();
                    // nmrcTargetQuantity_Leave(nmrcTargetQuantity,EventArgs.Empty);
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
        /// Handles the StrategyValueChanged event of the strategyControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}" /> instance containing the event data.</param>
        private void strategyControl1_StrategyValueChanged(object sender, EventArgs<string> e)
        {
            try
            {
                AlgoStrategyChanged(e.Value, strategyControl1.GetStrategyName());
                AdjustTTControlsSizeDynamically(false);
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
        /// Adjust TT Controls Size Dynamically
        /// </summary>
        private void AdjustTTControlsSizeDynamically(bool isDefault)
        {
            if (!isDefault)
            {
                this.Height = 502;
                int algoPanelHeight = algoStrategyControl.MaxPanelHeight;
                if ((algoPanelHeight - 120) >= 0)
                {
                    this.Height = (this.Height + (algoPanelHeight - 120)) >= 754 ? 754 : (this.Height + (algoPanelHeight - 120)) + 12;
                    this.bottomTabControl.Height = algoPanelHeight + 63;
                }
                algoStrategyControl.MaxPanelHeight = 120;
                if (!string.IsNullOrEmpty(algoStrategyControl.CustomMessage))
                {
                    this.Height = this.Height + 15;
                    lblAlgoMessage.Visible = true;
                    lblAlgoMessage.Text = algoStrategyControl.CustomMessage;
                }
                else if (string.IsNullOrEmpty(algoStrategyControl.CustomMessage))
                {
                    lblAlgoMessage.Visible = false;
                }
                if (ttPresenter.AlgoStrategyId != int.MinValue.ToString())
                    algoStrategyControl.Show();
            }
            else
            {
                this.Height = 492;
                this.bottomTabControl.Height = 163;
                lblAlgoMessage.Visible = false;

            }
        }


        private void algoStrategyControl_SendButtonClicked(object sender, EventArgs<string> e)
        {
            try
            {
                this.btnSend_Click(this, null);
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
        /// Handles the Disposed event of the tradingTicket control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void tradingTicket_Disposed(object sender, EventArgs e)
        {
            TradeManagerExtension.GetInstance().CounterPartyStatusUpdate -= TradingTicket_CounterPartyStatusUpdate;
            ttPresenter.Dispose();
        }

        /// <summary>
        /// Handles the FormClosed event of the TradingTicket control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs" /> instance containing the event data.</param>
        private void TradingTicket_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (FormClosedHandler != null)
                    FormClosedHandler(this, e);
                if (btnReplace.Visible && !_tradingTicketClosedAfterExecutingTrade)
                {
                    UnFrozeReplaceOrder(ttPresenter.OrderRequest, e);
                }
                _tradingTicketClosedAfterExecutingTrade = false;
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
        /// Handles the Load event of the TradingTicket control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void TradingTicket_Load(object sender, EventArgs e)
        {
            try
            {
                InitControl();
                var numericUpDownEx = GetAll(this, typeof(PranaNumericUpDown));
                foreach (PranaNumericUpDown numericUpDownExEditor in numericUpDownEx)
                {
                    numericUpDownExEditor.AutoSelect = true;
                }

                CustomThemeHelper.SetThemeProperties(FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_TRADING_TICKET);
                ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, Text, CustomThemeHelper.UsedFont);

                tblPnlMain.BackColor = Color.FromArgb(209, 210, 212);
                oneSymbolL1Strip.BackColor = Color.FromArgb(209, 210, 212);
                if (!(DesignMode))
                {
                    grbBoxOptionControl.Enabled = chkBoxOption.Checked;
                    grbBoxOptionControl.Expanded = chkBoxOption.Checked;
                    grbBoxOptionControl.Visible = chkBoxOption.Checked;
                }
                ChangeIconForTheme();
                SetButtonsColor();
                cmbBroker.DrawFilter = new BrokerComboDrawFilter();
                //set visibility of prices strip to false, if permission is not given, PRANA-26800
                if (!LoginUser.MarketDataTypes.Contains(LiveFeedConstants.LevelOne))
                    SetLevelOneStripVisibility(false);
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
        /// Updates the account combo for PTT, RB and Import.
        /// </summary>
        /// <param name="or">The or.</param>
        private void UpdateAccountCombo(OrderSingle or)
        {
            try
            {
                // in case of PTT or Imported trades disable combo Account and change selected text also use Level1ID for Allocation in Accounts
                if (or.TransactionSource != TransactionSource.PST && or.TransactionSource != TransactionSource.TradeImport && or.TransactionSource != TransactionSource.Rebalancer) return;
                DataTable dt = cmbAllocation.DataSource as DataTable;
                if (dt != null)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = or.Level1ID;

                    if (or.TransactionSourceTag == 3)
                        dr[1] = "Custom Fixed";
                    else if (or.TransactionSourceTag == (int)TransactionSource.Rebalancer)
                        dr[1] = "Rebal";
                    else
                        dr[1] = "PTT";
                    dr[2] = "True";
                    dt.Rows.Add(dr);
                    cmbAllocation.Value = or.Level1ID;
                }

                cmbAllocation.Enabled = false;
                cmbFunds.Enabled = false;
                btnAccountQty.Enabled = false;
                if (or.TransactionSourceTag != 3 && or.TransactionSourceTag != 14)
                    btnViewAllocationDetails.Visible = true;
                chkBoxOption.Enabled = true;
                if ((or.TransactionSource == TransactionSource.TradeImport || or.TransactionSource == TransactionSource.Rebalancer) &&
                    (ttPresenter.IncomingOrderRequest.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDStaged ||
                      (or.MsgType != FIXConstants.MSGOrderCancelReplaceRequest && or.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX &&
                      or.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew && or.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrder)))
                {
                    AllocationOperationPreference allocationOperationPreference = ttPresenter.GetSimpleCalculatedPreference(or.Level1ID);
                    if (allocationOperationPreference != null)
                    {
                        _stagedOrderAllocationView = new StagedOrderAllocationView();
                        _stagedOrderAllocationView.AllocationDetails = allocationOperationPreference.TargetPercentage;
                        btnAccountQty.Enabled = true;
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
        /// Updates the broker combo.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="changeType">Type of the change.</param>
        private void UpdateBrokerCombo(int id, ChangeType changeType)
        {
            try
            {
                string name = String.Empty;
                if (id != int.MinValue && !String.IsNullOrEmpty(CachedDataManager.GetInstance.GetCounterPartyText(id)))
                {
                    name = CachedDataManager.GetInstance.GetCounterPartyText(id);
                }
                if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbBroker.ValueList, id.ToString()))
                    UpdateComboValue(cmbBroker, id.ToString(), name, changeType);
                else
                    cmbBroker.Value = null;
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
        /// Updates the combo value.
        /// </summary>
        /// <param name="cmb">The CMB.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="changeType">Type of the change.</param>
        private void UpdateComboValue(UltraComboEditor cmb, string id, string name, ChangeType changeType)
        {
            try
            {
                cmb.Value = id;
                int comboValue = 0;
                if (cmb.Value != null) int.TryParse(cmb.Value.ToString(), out comboValue);

                if (cmb.Value == null || comboValue == int.MinValue)
                {
                    if (changeType == ChangeType.Trade)
                    {
                        DataTable dt = (DataTable)cmb.DataSource;
                        DataRow dr = dt.NewRow();
                        dr[cmb.ValueMember] = id;
                        dr[cmb.DisplayMember] = name;
                        dt.Rows.Add(dr);
                        cmb.Value = id;
                        cmb.Enabled = false;
                    }
                    else
                    {
                        cmb.Value = int.MinValue;
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
        /// Updates the order type combo.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="changeType">Type of the change.</param>
        private void UpdateOrderTypeCombo(string id, ChangeType changeType)
        {
            try
            {
                string name = string.Empty;
                if (!String.IsNullOrEmpty(id))
                {
                    name = TagDatabaseManager.GetInstance.GetOrderTypeTextBasedOnID(id);
                }
                if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbOrderType.ValueList, id))
                    UpdateComboValue(cmbOrderType, id, name, changeType);
                else
                    cmbOrderType.Value = null;
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
        /// Updates the side combo.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="changeType">Type of the change.</param>
        private void UpdateSideCombo(string id, ChangeType changeType)
        {
            try
            {
                string name = string.Empty;
                if (!String.IsNullOrEmpty(id))
                {
                    name = TagDatabaseManager.GetInstance.GetOrderSideTextBasedOnID(id);
                }
                if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbOrderSide.ValueList, id))
                    UpdateComboValue(cmbOrderSide, id, name, changeType);
                else
                    cmbOrderSide.Value = null;
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
        /// Updates the tif combo.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="changeType">Type of the change.</param>
        private void UpdateTIFCombo(string id, ChangeType changeType)
        {
            try
            {
                string name = String.Empty;
                if (!String.IsNullOrEmpty(TagDatabaseManager.GetInstance.GetTIFTextBasedOnID(id)))
                {
                    name = TagDatabaseManager.GetInstance.GetTIFTextBasedOnID(id);
                }
                if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbTIF.ValueList, id))
                    UpdateComboValue(cmbTIF, id, name, changeType);
                else
                    cmbTIF.Value = null;
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
        /// Updates the trading account combo.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="changeType">Type of the change.</param>
        private void UpdateTradingAccountCombo(int id, ChangeType changeType)
        {
            try
            {
                string name = String.Empty;
                if (!String.IsNullOrEmpty(CachedDataManager.GetInstance.GetTradingAccountText(id)))
                {
                    name = CachedDataManager.GetInstance.GetTradingAccountText(id);
                }
                if (cmbTradingAccount.Value == null)
                {
                    if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbTradingAccount.ValueList, id.ToString()))
                        UpdateComboValue(cmbTradingAccount, id.ToString(), name, changeType);
                    else
                        cmbTradingAccount.Value = null;
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
        /// Updates the venue combo.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="changeType">Type of the change.</param>
        private void UpdateVenueCombo(int id, ChangeType changeType)
        {
            try
            {
                string name = String.Empty;
                if (id != int.MinValue && !String.IsNullOrEmpty(CachedDataManager.GetInstance.GetVenueText(id)))
                {
                    name = CachedDataManager.GetInstance.GetVenueText(id);
                    UpdateComboValue(cmbVenue, id.ToString(), name, changeType);
                }
                else
                {
                    cmbVenue.Value = null;
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
        /// Validates the message box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="mbButtons">The mb buttons.</param>
        /// <returns></returns>
        private DialogResult ValidateMessageBox(string message, string caption, MessageBoxButtons mbButtons)
        {
            return MessageBox.Show(this, message, caption, mbButtons, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Validates the message box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        private void ValidateMessageBox(string message, string caption = "")
        {
            try
            {
                ValidateMessageBox(message, caption, MessageBoxButtons.OK);
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
        /// cmb Funds Selection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbFunds_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    if (cmbFunds.Value == null)
                    {
                        cmbFunds.Text = "Multiple";
                        return;
                    }
                    int fundId = int.MinValue;
                    if (int.TryParse(cmbFunds.Value.ToString(), out fundId))
                    {
                        ttPresenter.FundId = fundId;
                        ttPresenter.UpdateAccountsListFilter(fundId);
                        ttPresenter.UpdateTradingAccountList(fundId);
                        int tradingAccountID = Int32.MinValue;
                        tradingAccountID = CachedDataManager.GetTradingAccountForMasterFund(fundId);
                        if (tradingAccountID != -1)
                            UpdateTradingAccountCombo(tradingAccountID, ChangeType.NoTrade);
                        if (ttPresenter.TradingTicketUiPrefs != null && ttPresenter.TradingTicketUiPrefs.Broker.HasValue)
                            UpdateBrokerCombo(int.Parse(ttPresenter.TradingTicketUiPrefs.Broker.ToString()), ChangeType.NoTrade);
                        else if (ttPresenter.CompanyTradingTicketUiPrefs != null && ttPresenter.CompanyTradingTicketUiPrefs.Broker.HasValue && cmbBroker.Value == null)
                            UpdateBrokerCombo(int.Parse(ttPresenter.CompanyTradingTicketUiPrefs.Broker.ToString()), ChangeType.NoTrade);
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
        /// Handles the CheckedChanged event of the chkBoxOption control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void chkBoxOption_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                grbBoxOptionControl.Enabled = chkBoxOption.Checked;
                grbBoxOptionControl.Expanded = chkBoxOption.Checked;
                AdjustAlgoStrategyContolBasedOnPref();

                pranaOptionCtrl.EnableValidate = chkBoxOption.Checked;
                pranaOptionCtrl.setSymbol(pranaSymbolCtrl.Text);
                pranaSymbolCtrl.Enabled = !chkBoxOption.Checked;

                SetUIOnCheckBoxChanged("Option");

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
        /// Adjust Algo Strategy Contol Based on Pref
        /// </summary>
        private void AdjustAlgoStrategyContolBasedOnPref()
        {
            try
            {

                lblHiddenStrategyAdjust.Visible = !chkBoxOption.Checked;
                if (chkBoxOption.Checked && _isNewOTCWorkflowEnabled && CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    this.lblHiddenStrategyAdjust.Size = new System.Drawing.Size(200, 50);
                }
                else if (!chkBoxOption.Checked && _isNewOTCWorkflowEnabled && !CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    this.lblHiddenStrategyAdjust.Size = new System.Drawing.Size(200, 100);
                }
                else if (!chkBoxOption.Checked && !_isNewOTCWorkflowEnabled && !CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    this.lblHiddenStrategyAdjust.Size = new System.Drawing.Size(200, 137);
                    this.lblHiddenStrategyAdjust.Visible = true;
                }
                else if (chkBoxOption.Checked && !_isNewOTCWorkflowEnabled && !CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    this.lblHiddenStrategyAdjust.Size = new System.Drawing.Size(200, 41);
                    this.lblHiddenStrategyAdjust.Visible = true;
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
        /// Set Fund View
        /// </summary>
        private void SetFundView()
        {
            try
            {
                //IsShowMasterFundonTT preference is true then Mastrefund (Fund) drop down
                //Show label and set label based on preference IsShowmasterFundAsClient

                if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    cmbTradeAttribute6.Enabled = false;
                    lblFunds.Visible = true;
                    cmbFunds.Visible = true;
                    Dictionary<int, string> fundsDict = new Dictionary<int, string>();
                    fundsDict.Add(int.MinValue, "Multiple");

                    //Getting masterfunds from cache and then sorting alphabatical order
                    var fundsDictForCache = CommonDataCache.CachedDataManager.GetInstance.GetUserMasterFunds();
                    var sortedDict = fundsDictForCache.OrderBy(x => x.Value).ToDictionary(x => x.Key, y => y.Value);
                    foreach (var item in sortedDict)
                    {
                        fundsDict.Add(item.Key, item.Value);
                    }
                    var fundsValueList = fundsDict.ToValueList();
                    if (fundsValueList != null)
                    {
                        cmbFunds.ValueList = fundsValueList;
                        cmbFunds.SelectedText = "Multiple";
                    }

                    lblFunds.Text = CachedDataManager.GetInstance.IsShowmasterFundAsClient() ? "Client" : "Master Fund";
                }
                else
                {
                    lblFunds.Visible = false;
                    cmbFunds.Visible = false;
                    cmbTradeAttribute6.Enabled = true;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Set Trading Ticket View For OTC
        /// </summary>
        private void SetTradingTicketViewForOTC()
        {
            _isNewOTCWorkflowEnabled = CachedDataManager.GetInstance.IsNewOTCWorkflow;

            chkBoxCFD.Visible = _isNewOTCWorkflowEnabled;
            chkBoxEquitySwap.Visible = _isNewOTCWorkflowEnabled;
            chkBoxConvertiableBond.Visible = _isNewOTCWorkflowEnabled;
        }

        /// <summary>
        /// Set UI On Check Box Changed
        /// </summary>
        /// <param name="control"></param>
        private void SetUIOnCheckBoxChanged(string control)
        {
            if (otcTradeViewUI != null)
            {
                DialogResult userResponse = MessageBox.Show(TradingTicketConstants.MSG_REMOVE_OTCTEMPLATEUI, "TradingTicket", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (userResponse == DialogResult.Yes)
                {

                    otcTradeViewUI.Close();
                    otcTradeViewUI = null;
                    _otcParameters = null;
                }
                else
                {
                    if (control.Equals("EquitySwap"))
                    {
                        control = "CFD";
                        StrategyControlAlignment(true);
                    }
                    else if (control.Equals("CFD"))
                    {
                        control = "EquitySwap";
                        StrategyControlAlignment(true);
                    }
                    else if (control.Equals("Option"))
                    {
                        control = chkBoxEquitySwap.Checked ? "EquitySwap" : "CFD";

                    }
                }
            }
            grbBoxOptionControl.Visible = false;
            btnOTCControl.Visible = false;
            switch (control)
            {
                case "Option":
                    grbBoxOptionControl.Visible = chkBoxOption.Checked;
                    if (_isNewOTCWorkflowEnabled)
                    {
                        chkBoxCFD.CheckedChanged -= chkBoxCFD_CheckedChanged;
                        chkBoxEquitySwap.CheckedChanged -= chkBoxEquitySwap_CheckedChanged;
                        chkBoxConvertiableBond.CheckedChanged -= chkBoxConvertiableBond_CheckedChanged;

                        chkBoxEquitySwap.Checked = false;
                        chkBoxCFD.Checked = false;
                        chkBoxConvertiableBond.Checked = false;

                        chkBoxCFD.CheckedChanged += chkBoxCFD_CheckedChanged;
                        chkBoxEquitySwap.CheckedChanged += chkBoxEquitySwap_CheckedChanged;
                        chkBoxConvertiableBond.CheckedChanged += chkBoxConvertiableBond_CheckedChanged;

                    }
                    else
                    {
                        chkBoxSwap.Enabled = !chkBoxOption.Checked;
                    }

                    break;
                case "EquitySwap":
                    chkBoxOption.CheckedChanged -= chkBoxOption_CheckedChanged;
                    chkBoxCFD.CheckedChanged -= chkBoxCFD_CheckedChanged;
                    chkBoxConvertiableBond.CheckedChanged -= chkBoxConvertiableBond_CheckedChanged;
                    chkBoxConvertiableBond.Checked = false;
                    chkBoxCFD.Checked = false;
                    chkBoxOption.Checked = false;
                    btnOTCControl.Visible = chkBoxEquitySwap.Checked;
                    btnOTCControl.Text = "Swap Details";
                    pranaSymbolCtrl.Enabled = true;
                    chkBoxOption.CheckedChanged += chkBoxOption_CheckedChanged;
                    chkBoxCFD.CheckedChanged += chkBoxCFD_CheckedChanged;
                    chkBoxConvertiableBond.CheckedChanged += chkBoxConvertiableBond_CheckedChanged;

                    break;
                case "CFD":
                    chkBoxOption.CheckedChanged -= chkBoxOption_CheckedChanged;
                    chkBoxEquitySwap.CheckedChanged -= chkBoxEquitySwap_CheckedChanged;
                    chkBoxConvertiableBond.CheckedChanged -= chkBoxConvertiableBond_CheckedChanged;
                    chkBoxConvertiableBond.Checked = false;
                    chkBoxEquitySwap.Checked = false;
                    chkBoxOption.Checked = false;
                    btnOTCControl.Visible = chkBoxCFD.Checked;
                    btnOTCControl.Text = "CFD Details";
                    pranaSymbolCtrl.Enabled = true;
                    chkBoxOption.CheckedChanged += chkBoxOption_CheckedChanged;
                    chkBoxEquitySwap.CheckedChanged += chkBoxEquitySwap_CheckedChanged;
                    chkBoxConvertiableBond.CheckedChanged += chkBoxConvertiableBond_CheckedChanged;

                    break;
                case "ConvertiableBond":
                    chkBoxOption.CheckedChanged -= chkBoxOption_CheckedChanged;
                    chkBoxEquitySwap.CheckedChanged -= chkBoxEquitySwap_CheckedChanged;
                    chkBoxCFD.CheckedChanged -= chkBoxCFD_CheckedChanged;
                    chkBoxCFD.Checked = false;
                    chkBoxEquitySwap.Checked = false;
                    chkBoxOption.Checked = false;
                    btnOTCControl.Visible = chkBoxConvertiableBond.Checked;
                    btnOTCControl.Text = "Convertiable Bond";
                    pranaSymbolCtrl.Enabled = true;
                    chkBoxOption.CheckedChanged += chkBoxOption_CheckedChanged;
                    chkBoxEquitySwap.CheckedChanged += chkBoxEquitySwap_CheckedChanged;
                    chkBoxCFD.CheckedChanged += chkBoxCFD_CheckedChanged;


                    break;
                case "Swap":
                    chkBoxSwap.Enabled = true;
                    chkBoxEquitySwap.Checked = false;
                    chkBoxCFD.Checked = false;
                    chkBoxOption.Checked = false;
                    chkBoxConvertiableBond.Checked = false;
                    otcTradeViewUI = null;
                    _otcParameters = null;
                    break;
            }
        }

        /// <summary>
        ///  Handles the CheckedChanged event of the chkBoxCFD control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBoxCFD_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                SetUIOnCheckBoxChanged("CFD");
                StrategyControlAlignment(chkBoxCFD.Checked || btnOTCControl.Visible ? true : false);
                lblHiddenStrategyAdjust.Visible = true;
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
        ///  Handles the CheckedChanged event of the chkBoxCFD control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBoxConvertiableBond_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                SetUIOnCheckBoxChanged("ConvertiableBond");
                StrategyControlAlignment(chkBoxConvertiableBond.Checked || btnOTCControl.Visible ? true : false);
                lblHiddenStrategyAdjust.Visible = true;
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

        private void StrategyControlAlignment(bool ischecked)
        {
            if (ischecked)
                if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                    this.lblHiddenStrategyAdjust.Size = new System.Drawing.Size(200, 9);
                else
                    this.lblHiddenStrategyAdjust.Size = new System.Drawing.Size(200, 67);
            else
                if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                this.lblHiddenStrategyAdjust.Size = new System.Drawing.Size(200, 42);
            else
                this.lblHiddenStrategyAdjust.Size = new System.Drawing.Size(200, 100);

        }

        /// <summary>
        ///  Handles the CheckedChanged event of the chkBoxEquitySwap control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBoxEquitySwap_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {

                btnOTCControl.Visible = true;
                SetUIOnCheckBoxChanged("EquitySwap");
                lblHiddenStrategyAdjust.Visible = true;
                StrategyControlAlignment(chkBoxEquitySwap.Checked || btnOTCControl.Visible ? true : false);

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
        /// Handles the OptionGenerated event of the pranaOptionCtrl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}"/> instance containing the event data.</param>
        private void pranaOptionCtrl_OptionGenerated(object sender, EventArgs<string> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            pranaOptionCtrl_OptionGenerated(sender, e);
                        }));
                    }
                    else
                    {
                        pranaSymbolCtrl.Text = e.Value;
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
        /// Handles the CheckedChanged event of the chkBoxSwap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void chkBoxSwap_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bottomTabControl.Tabs[TradingTicketConstants.TAB_SWAP_KEY].Visible = true;
                bottomTabControl.Tabs[TradingTicketConstants.TAB_SWAP_KEY].Selected = true;
                if (chkBoxSwap.Checked && ttPresenter.AssetID == (int)AssetCategory.Equity)
                {
                    double avgPrice = 0.0;
                    Double.TryParse(nmrcPrice.Text, out avgPrice);
                    UpdateNotionalValueOnSwapControl();
                    ctrlSwapParameters1.UpdateCostBasis(avgPrice);
                    ctrlSwapParameters1.UpdateOrigTransDate(DateTime.Parse(dtTradeDate.Value.ToString()));
                }
                else
                {
                    bottomTabControl.Tabs[TradingTicketConstants.TAB_SWAP_KEY].Visible = false;
                }
                CheckBoxOptionChecked();
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


        private void btnOTCControl_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (otcTradeViewUI == null)
                {
                    otcTradeViewUI = new OTCTradeDetailsView();
                    var dataContext = otcTradeViewUI.DataContext as OTCTradeDetailsViewModel;
                    if (dataContext != null)
                    {
                        var instrumentType = InstrumentType.EquitySwap;

                        if (chkBoxEquitySwap.Checked)
                            instrumentType = InstrumentType.EquitySwap;
                        else if (chkBoxCFD.Checked)
                            instrumentType = InstrumentType.CFD;
                        else if (chkBoxConvertiableBond.Checked)
                            instrumentType = InstrumentType.ConvertibleBond;

                        dataContext.SetData(SymbolText, TradeDate, instrumentType);
                        dataContext.OnSaveOTCParamsEvent += OTCTradeDetails_OnSaveOTCParamsEvent;

                    }
                    otcTradeViewUI.Closed += otcTradeViewUI_Closed;
                    BringFormToFront(otcTradeViewUI);
                }
                else
                {
                    otcTradeViewUI.WindowState = System.Windows.WindowState.Normal;
                    otcTradeViewUI.Activate();
                    otcTradeViewUI.Show();

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

        private void BringFormToFront(System.Windows.Window window)
        {
            if (!window.IsVisible)
            {
                window.ShowInTaskbar = false;
                ElementHost.EnableModelessKeyboardInterop(window);
                new System.Windows.Interop.WindowInteropHelper(window) { Owner = Handle };
                window.Show();
                window.Activate();
            }
        }

        void otcTradeViewUI_Closed(object sender, EventArgs e)
        {
            try
            {
                if (otcTradeViewUI != null)
                {
                    otcTradeViewUI.Closed -= otcTradeViewUI_Closed;
                    otcTradeViewUI = null;
                }
                _otcParameters = null;
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
        /// Action On Save OTC Params Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OTCTradeDetails_OnSaveOTCParamsEvent(object sender, OTCTradeData e)
        {
            try
            {
                _otcParameters = new OTCTradeData();
                _otcParameters = e;
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
        /// Handles the ExpandedStateChanged event of the grbBoxOptionControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void grbBoxOptionControl_ExpandedStateChanged(object sender, EventArgs e)
        {
            try
            {
                pranaSymbolCtrl.Enabled = !grbBoxOptionControl.Expanded;
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

        private void RefreshOptionControl()
        {
            try
            {
                if (ttPresenter != null)
                {
                    pranaOptionCtrl.RefreshControl(ttPresenter.DefaultSymbology, TradingTktPrefs.TTGeneralPrefs.DefaultOptionType);
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

        void dtTradeDate_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (chkBoxSwap.Checked)
                {
                    ctrlSwapParameters1.UpdateOrigTransDate(DateTime.Parse(dtTradeDate.Value.ToString()));
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

        void nmrcLimit_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                UpdateNotionalValueOnCaption();
                UpdateCaption();
                nmrcPrice.Value = nmrcLimit.Value;
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

        void nmrcLimit_Leave(object sender, System.EventArgs e)
        {
            try
            {
                if (nmrcLimit.Value == 0.0m)
                {
                    UpdateNotionalValueOnCaption();
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

        private void SetLevelOneStripVisibility(bool levelOneStripVisible)
        {
            try
            {
                oneSymbolL1Strip.Visible = levelOneStripVisible;
                btnGetLimitPrice.Visible = levelOneStripVisible;

                if (levelOneStripVisible)
                    this.tblPnlTTMainControls.RowStyles[0].Height = 12;
                else
                    this.tblPnlTTMainControls.RowStyles[0].Height = 2;
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

        private bool CheckIfErrorExists(UltraComboEditor ultraComboEditor)
        {
            bool isValidated = true;
            if (ultraComboEditor.Value != null)
            {
                if (ultraComboEditor.Name.Contains("cmbTradeAttribute"))
                {
                    return isValidated;
                }
                if (!ValueListUtilities.CheckIfValueExistsInValuelist(ultraComboEditor.ValueList, ultraComboEditor.Value.ToString()))
                {
                    errorProvider.SetIconPadding(ultraComboEditor, -35);
                    errorProvider.SetError(ultraComboEditor, TradingTicketConstants.MSG_VALUE_IS_INVALID_FOR_FIELD);
                    isValidated = false;
                }
                else
                {
                    errorProvider.SetError(ultraComboEditor, String.Empty);
                }
            }
            else
            {
                errorProvider.SetError(ultraComboEditor, String.Empty);
            }
            return isValidated;
        }

        private void SetExpireTimeVisibility(bool isVisible)
        {
            try
            {
                dtExpireTime.Visible = isVisible;
                btnExpireTime.Visible = isVisible;
                lblExpireTime.Visible = isVisible;
                btnDoneAway.Enabled = !isVisible;
                if (!isVisible)
                {
                    dtExpireTime.Value = "";
                    lblExpireTime.Text = "";
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
        /// cmbTIF ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTIF_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender) && cmbTIF.Value != null)
                {
                    SetExpireTimeVisibility(false);
                    if (IsMultiDayOrder())
                    {
                        btnDoneAway.Enabled = false;
                        string tifStringValue = TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(cmbTIF.Value.ToString());
                        TranferTradeRules transfertraderules = CachedDataManager.GetInstance.GetTransferTradeRules();
                        if (tifStringValue.Equals(FIXConstants.TIF_GTD))
                            SetExpireTimeVisibility(true);
                        if (transfertraderules != null && transfertraderules.IsDefaultOrderTypeLimitForMultiDay)
                            UpdateOrderTypeCombo(FIXConstants.ORDTYPE_Limit, ChangeType.NoTrade);

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

        private bool IsMultiDayOrder()
        {
            try
            {
                if (cmbTIF.Value != null)
                {
                    string tifStringValue = TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(cmbTIF.Value.ToString());
                    if (!string.IsNullOrEmpty(tifStringValue) && (tifStringValue.Equals(FIXConstants.TIF_GTC) || tifStringValue.Equals(FIXConstants.TIF_GTD)))
                    {
                        return true;
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
            return false;
        }

        private void cmb_ValueChanged(object sender, System.EventArgs e)
        {
            CheckIfErrorExists((UltraComboEditor)sender);
        }

        private void cmb_Leave(object sender, System.EventArgs e)
        {
            CheckIfErrorExists((UltraComboEditor)sender);
        }

        private void cmbOrderSide_Leave(object sender, System.EventArgs e)
        {
            CheckIfErrorExists((UltraComboEditor)sender);
            OpenShortLocatePopup(true);
        }

        public void ResetTicket()
        {
            try
            {
                ttPresenter.IsShowAlgoControls = false;
                var ultraComobEditors = GetAll(this, typeof(UltraComboEditor));
                foreach (UltraComboEditor ucmbEditor in ultraComobEditors.Cast<UltraComboEditor>().Where(ucmbEditor => !ucmbEditor.Name.Equals("cmbTradeAttribute1") && !ucmbEditor.Name.Equals("cmbTradeAttribute2") && !ucmbEditor.Name.Equals("cmbTradeAttribute3") && !ucmbEditor.Name.Equals("cmbTradeAttribute4") && !ucmbEditor.Name.Equals("cmbTradeAttribute5") && !ucmbEditor.Name.Equals("cmbTradeAttribute6") && !ucmbEditor.Name.Equals("cmbOptionType") && !ucmbEditor.Name.Equals("cmbFunds") && !ucmbEditor.Name.Equals("ultraComboEditorSymbology")))
                {
                    ucmbEditor.Value = null;
                    ucmbEditor.DataSource = null;
                    ucmbEditor.Enabled = true;
                }
                var ultraNumericEditors = GetAll(this, typeof(UltraNumericEditor));
                foreach (UltraNumericEditor unmrcEditor in ultraNumericEditors.Cast<UltraNumericEditor>())
                {
                    unmrcEditor.Value = 0;
                    unmrcEditor.Enabled = true;
                }
                var numericUpDownEx = GetAll(this, typeof(PranaNumericUpDown));
                foreach (PranaNumericUpDown numericUpDownExEditor in numericUpDownEx.Cast<PranaNumericUpDown>())
                {
                    numericUpDownExEditor.Value = 0;
                    numericUpDownExEditor.Enabled = true;
                }
                var ultraButton = GetAll(this, typeof(UltraButton));
                foreach (UltraButton uButton in ultraButton.Cast<UltraButton>())
                {
                    uButton.Enabled = true;
                }

                txtBrokerNotes.Text = String.Empty;
                txtNotes.Text = String.Empty;
                pranaSymbolCtrl.Enabled = true;
                strategyControl1.Visible = false;
                algoStrategyControl.Visible = false;
                _isTTSourceDependentOnAnotherUIs = false;
                strategyControl1.Reset();
                chkBoxSwap.Checked = false;
                chkBoxSwap.Visible = false;
                chkBoxCFD.CheckedChanged -= chkBoxCFD_CheckedChanged;
                chkBoxEquitySwap.CheckedChanged -= chkBoxEquitySwap_CheckedChanged;
                chkBoxConvertiableBond.CheckedChanged -= chkBoxConvertiableBond_CheckedChanged;
                chkBoxCFD.Checked = false;
                chkBoxEquitySwap.Checked = false;
                chkBoxCFD.Enabled = false;
                chkBoxConvertiableBond.Enabled = false;
                chkBoxEquitySwap.Enabled = false;
                btnOTCControl.Enabled = false;
                chkBoxCFD.CheckedChanged += chkBoxCFD_CheckedChanged;
                chkBoxEquitySwap.CheckedChanged += chkBoxEquitySwap_CheckedChanged;
                chkBoxConvertiableBond.CheckedChanged += chkBoxConvertiableBond_CheckedChanged;
                btnViewAllocationDetails.Visible = false;
                btnShortLocateList.Visible = false;
                lblDealIn.Visible = false;
                cmbDealIn.Visible = false;
                chkBoxSwap.Visible = false;
                if (SymbologyHelper.DefaultSymbology != ApplicationConstants.SymbologyCodes.ActivSymbol && SymbologyHelper.DefaultSymbology != ApplicationConstants.SymbologyCodes.FactSetSymbol)
                {
                    chkBoxOption.Checked = false;
                    chkBoxOption.Visible = true;
                }
                oneSymbolL1Strip.setRoundLotButton(false);
                grbBoxStrategyControl.Enabled = false;
                grbBoxStrategyControl.Expanded = false;
                bottomTabControl.Tabs[TradingTicketConstants.TAB_SETTLEMENT_KEY].Visible = false;
                tblPnlTTMainControls.Controls.Add(lblOrderSide, 0, 1);
                tblPnlTTMainControls.Controls.Add(cmbOrderSide, 0, 2);
                tblPnlTTMainControls.Controls.Add(pnlNotionalQuantity, 1, 1);
                tblPnlTTMainControls.Controls.Add(nmrcQuantity, 1, 2);
                if (IsShowTargetQTY)
                {
                    SetTTControlsBasedonPref(true);
                }
                else
                {
                    SetTTControlsBasedonPref(false);
                }
                tblPnlTTMainControls.Controls.Add(lblDealIn, 4, 5);
                tblPnlTTMainControls.Controls.Add(cmbDealIn, 4, 6);
                cmbOrderSide.TabIndex = 6;
                nmrcQuantity.TabIndex = 7;
                //nmrcTargetQuantity.TabIndex = 8;
                cmbDealIn.TabIndex = 29;
                btnDoneAway.Text = TradingTicketConstants.BUTTON_CAPTION_DONE_AWAY;
                btnDoneAway.Visible = true;
                btnDoneAway.Enabled = true;
                btnReplace.Visible = false;
                btnReplace.Enabled = false;
                btnSend.Enabled = true;
                btnSend.Visible = true;
                btnCreateOrder.Visible = true;
                btnCreateOrder.Enabled = true;
                tblPnlTTMainControls.Enabled = false;
                btnBrokerConnectionStatus.Visible = false;
                btnMultiBrokerConnectionStatus.Visible = false;
                cmbBroker.NullText = "Select Broker";
                btnPadlock.Visible = false;
                ttPresenter.OrderRequest = new OrderSingle();
                ttPresenter.TradingTicketParentType = _tradingTicketParent;
                ttPresenter.SetIncomingOrderRequest(null);
                pranaSymbolCtrl.PrevSymbolEntered = String.Empty;
                _isUpdateBrokerBasedOnCustodianPreference = false;
                ChangeFormTitle();
                _watchListColumn = string.Empty;
                SetExpireTimeVisibility(false);
                AdjustTTControlsSizeDynamically(true);
                _stagedOrderAllocationView = null;
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

        public void AddMultipleAcountToAccountCombo()
        {
            try
            {
                int prefId = 0;
                DataTable dt = cmbAllocation.DataSource as DataTable;

                if (!(dt.AsEnumerable().Any(row => prefId == int.Parse(row.Field<String>(OrderFields.PROPERTY_LEVEL1ID)))))
                {
                    if (dt != null)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = prefId;
                        dr[1] = OrderFields.PROPERTY_MULTIPLE;
                        dr[2] = "True";
                        dt.Rows.Add(dr);
                    }
                }
                cmbAllocation.Value = prefId;
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

        public void AddCustomPreferenceToAccountCombo(int prefId)
        {
            try
            {
                DataTable dt = cmbAllocation.DataSource as DataTable;

                if (!(dt.AsEnumerable().Any(row => prefId == int.Parse(row.Field<String>(OrderFields.PROPERTY_LEVEL1ID)))))
                {
                    if (dt != null)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = prefId;
                        dr[1] = TradingTicketConstants.LIT_CUSTOM;
                        dr[2] = "True";
                        dt.Rows.Add(dr);
                    }
                }
                cmbAllocation.Value = prefId;
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

        public delegate void SetAUECDateInDateControlDelegate(DateTime dt);

        public void SetAUECDateInTicket(DateTime dt)
        {
            try
            {
                SetAUECDateInDateControlDelegate setAUECDateInDateControlDelegate = SetAUECDateInTicket;
                if (UIValidation.GetInstance().validate(dtTradeDate))
                {
                    if (dtTradeDate.InvokeRequired)
                    {
                        try
                        {
                            BeginInvoke(setAUECDateInDateControlDelegate, dt);
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
                    else
                    {
                        dtTradeDate.Value = dt;
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
        /// Sets error on price field.
        /// </summary>
        private void PriceNotAvailableForCompliance(object sender, EventArgs e)
        {
            try
            {
                nmrcPrice.Focus();
                errorProvider.SetIconPadding(nmrcPrice, -35);
                errorProvider.SetError(nmrcPrice, TradingTicketConstants.MSG_PRICES_NOT_AVAILABLE);
                ToolTip1.SetToolTip(this.btnCreateOrder, TradingTicketConstants.MSG_PRICES_NOT_AVAILABLE);
                ToolTip1.SetToolTip(this.btnSend, TradingTicketConstants.MSG_PRICES_NOT_AVAILABLE);
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
        /// Message on mouse hover over send/create button depending on compliance permission.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        private void message_MouseHover(object sender, EventArgs e)
        {
            try
            {
                bool CompliancePriceCheckRequired = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("CompliancePriceCheckRequired"));
                if (CompliancePriceCheckRequired && ComplianceCacheManager.GetPreTradeCheckStaging(_loginUser.CompanyUserID) && nmrcPrice.Value == 0)
                    ToolTip1.SetToolTip(this.btnCreateOrder, TradingTicketConstants.MSG_PRICES_NOT_AVAILABLE);
                if (CompliancePriceCheckRequired && ComplianceCacheManager.GetPreTradeCheck(_loginUser.CompanyUserID) && nmrcPrice.Value == 0)
                    ToolTip1.SetToolTip(this.btnSend, TradingTicketConstants.MSG_PRICES_NOT_AVAILABLE);
                else
                {
                    ToolTip1.SetToolTip(this.btnCreateOrder, String.Empty);
                    ToolTip1.SetToolTip(this.btnSend, String.Empty);
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

        private void ChangeFormTitle(string symbolDescription = "")
        {
            try
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        ChangeFormTitle(symbolDescription);
                    }));
                    return;
                }

                if (!string.IsNullOrEmpty(symbolDescription) && this.Text != symbolDescription + " - Trading Ticket")
                    this.Text = symbolDescription + " - Trading Ticket";
                else if (this.Text != "Trading Ticket")
                    this.Text = "Trading Ticket";
                else
                    return;

                ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, Text, CustomThemeHelper.UsedFont);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        /// <summary>
        /// UnFroze the replace order
        /// </summary>
        /// <param name="OrderSingle"></param>
        /// <param name="e"></param>
        private void UnFrozeReplaceOrder(object OrderSingle, System.EventArgs e)
        {
            try
            {
                if (OrderSingle != null)
                {
                    OrderSingle or = (OrderSingle)OrderSingle;
                    OrderSingle replacelOrder = (OrderSingle)or.Clone();
                    replacelOrder.MsgType = FIXConstants.MSGOrderCancelRequestUnFroze;
                    TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(replacelOrder);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
        private void DtExpireDate_AfterDropDown(object sender, EventArgs e)
        {
            try
            {
                UltraCalendarInfo ultraCalendarInfo1 = (UltraCalendarInfo)this.dtExpireTime.CalendarInfo;
                ultraCalendarInfo1.MinDate = DateTime.Now.AddDays(-1);
                ultraCalendarInfo1.MaxDate = DateTime.Now.AddDays(365);
                DateTime dt;
                if (dtExpireTime != null && dtExpireTime.Value != null && DateTime.TryParse(dtExpireTime.Value.ToString(), out dt) && dt.Date.Date < DateTime.Now.Date)
                {
                    dtExpireTime.Value = DateTime.Now.Date;
                }
                foreach (var day in ultraCalendarInfo1.DaysOfWeek)
                {
                    if (day.LongDescriptionResolved.ToString() == DayOfWeek.Sunday.ToString() || day.LongDescriptionResolved.ToString() == DayOfWeek.Saturday.ToString())
                        day.Enabled = false;
                    else
                        day.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// btnExpireTime_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void btnExpireTime_Click(object sender, EventArgs e)
        {
            try
            {
                dtExpireTime.DroppedDown = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtExpireDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtExpireTime.Value != null && !string.IsNullOrEmpty(dtExpireTime.Value.ToString()))
                {
                    DateTime selectedDate = Convert.ToDateTime(dtExpireTime.Value);
                    string dateString = selectedDate.ToString("MM/dd/yyyy");
                    lblExpireTime.Text = dateString;
                    dtExpireTime.DroppedDown = false;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }

        }

    }
}