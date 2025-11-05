using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinToolTip;
using Prana.Admin.BLL;
using Prana.AlgoStrategyControls;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.LiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.ClientPreferences;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using Prana.TradingTicket.Classes;
using Prana.TradingTicket.TTPresenter;
using Prana.TradingTicket.TTView;
using Prana.Utilities.UI.ExtensionUtilities;
using Prana.Utilities.UI.Forms;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;
using Prana.Utilities;

namespace Prana.TradingTicket.Forms
{
    public partial class QuickTradingTicket : Form, IQuickTradingTicketView, IQuickTradingTicket
    {
        public QuickTradingTicket()
        {
            InitializeComponent();
            if (!(DesignMode))
            {
                qttPresenter.Add(this);
            }
            if (!ModuleManager.CheckModulePermissioning(PranaModules.SECURITY_MASTER_MODULE, PranaModules.SECURITY_MASTER_MODULE))
                btnSymbolLookup.Enabled = false;

            CreateSMSyncPoxy();
        }

        #region fields
        private MainFormTitleHelper _titleHelper;
        private AlgoControlPopUp _algoControlPopUp = null;
        private QuickTTPrefs _quickTTPrefs;
        private QTTFieldPreference _qTTFieldPreference;
        private int _softCalculationBasis = 4;
        private bool _isNegativeHotButtons = false;
        private decimal _position = 0;
        private bool _isPriceUpdated = false;
        private decimal _ask = 0;
        private decimal _bid = 0;
        private decimal _fxRate = decimal.Zero;
        private decimal _notionalLocal = decimal.Zero;
        private decimal _notionalBase = decimal.Zero;
        private int? _strategy = int.MinValue;
        private string _orderSide = string.Empty;
        private string _notes = string.Empty;
        private string[] _sides = new string[4];
        private int _handlingInstructions;
        private int _executionInstructions;
        private string _fxOperator = Operator.M.ToString();
        private int _calculationBasis = 4;
        private string _brokerNotes = string.Empty;
        private CompanyUser _loginUser;
        private string _tradingAccount = string.Empty;
        private DateTime _tradeDate = DateTime.Now;
        private CancellationTokenSource validationTokenSource = new CancellationTokenSource();
        private int _validationTimeout = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIGKEY_QuickTTValidationTimeout));
        private QTTBlotterLinkingData _linkingData = null;
        private bool _isLinked = false;
        private IList _comissionBasisValues = null;

        public event EventHandler LaunchSymbolLookup;
        public event EventHandler FormClosedHandler;
        public event EventHandler<EventArgs<string>> SendInstanceName;
        public event EventHandler<QTTBlotterLinkingData> HighlightSymbolOnBlotter;
        public event EventHandler<QTTBlotterLinkingData> DeHighlightSymbolOnBlotter;

        ProxyBase<ISecMasterSyncServices> _secMasterSyncService = null;

        /// <summary>
        /// The tt presenter
        /// </summary>
        private QuickTradingTicketPresenter qttPresenter = new QuickTradingTicketPresenter();
        #endregion

        #region Props
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

        public string AccountText
        {
            get
            {
                return cmbAllocation.Text;
            }
        }

        public string Broker
        {
            get
            {
                return cmbBroker.Text;
            }
        }

        public string Brokerid
        {
            get
            {
                return (string)cmbBroker.Value;
            }
        }

        public string OrderType
        {
            get
            {
                return (string)cmbOrderType.Value;
            }
        }

        public decimal Price
        {
            get
            {
                return nmrcPrice.Value;
            }
        }

        public decimal Quantity
        {
            get
            {
                return nmrcQuantity.Value;
            }
        }

        public decimal Stop
        {
            get
            {
                return nmrcStop.Value;
            }
        }

        public string SymbolText
        {
            get
            {
                return pranaSymbolCtrl.Text;
            }
        }
        private Dictionary<string, string> _tagValueDictionary = new Dictionary<string, string>();

        public Dictionary<string, string> TagValueDictionary
        {
            get { return _tagValueDictionary; }

        }
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
                return string.Empty;
            }
        }

        public DateTime TradeDate
        {
            get { return _tradeDate; }
        }


        public string TradingAccount
        {
            get
            {
                return _tradingAccount;
            }
        }

        public string Venue
        {
            get
            {
                return cmbVenue.Text;
            }
        }

        public string VenueId
        {
            get
            {
                return (string)cmbVenue.Value;
            }
        }

        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
                qttPresenter.LoginUser = value;
            }
        }

        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                qttPresenter.SecurityMaster = value;
            }
        }
        public int QTTIndex { get; set; }

        public AlgoStrategyControl AlgoStrategyControlProperty
        {
            get
            {
                return null;
            }
        }

        public string BrokerNotes
        {
            get
            {
                return _brokerNotes;
            }
        }

        public CalculationBasis CommissionBasis
        {
            get
            {
                return (CalculationBasis)((KeyValuePair<Enum, string>)_comissionBasisValues[_calculationBasis]).Key;
            }
        }

        public decimal CommissionRate
        {
            get
            {
                return 0;
            }
        }

        public string FxOperator
        {
            get
            {
                return _fxOperator;
            }
        }

        public string ExecutionInstructions
        {
            get
            {
                return _executionInstructions.ToString();
            }
        }

        public decimal FxRate
        {
            get
            {
                return 0;
            }
        }

        public string HandlingInstruction
        {
            get
            {
                return _handlingInstructions.ToString();
            }
        }

        public decimal Limit
        {
            get
            {
                return nmrcPrice.Value;
            }
        }

        public string Notes
        {
            get
            {
                return _notes;
            }
        }

        public string OrderSide
        {
            get
            {
                return _orderSide;
            }
        }

        public CalculationBasis SoftCommissionBasis
        {
            get
            {
                return (CalculationBasis)((KeyValuePair<Enum, string>)_comissionBasisValues[_softCalculationBasis]).Key;
            }
        }

        public decimal SoftCommissionRate
        {
            get
            {
                return 0;
            }
        }
        public int? Strategy
        {
            get
            {
                return _strategy;
            }
        }

        public decimal TargetQuantity
        {
            get
            {
                return nmrcQuantity.Value;
            }
        }

        public string TradeAttribute1
        {
            get
            {
                return string.Empty;
            }
        }

        public string TradeAttribute2
        {
            get
            {
                return string.Empty;
            }
        }

        public string TradeAttribute3
        {
            get
            {
                return string.Empty;
            }
        }

        public string TradeAttribute4
        {
            get
            {
                return string.Empty;
            }
        }

        public string TradeAttribute5
        {
            get
            {
                return string.Empty;
            }
        }

        public string TradeAttribute6
        {
            get
            {
                return string.Empty;
            }
        }

        public bool IsSwap
        {
            get
            {
                return false;
            }
        }

        public bool IsUseCustodianAsExecutingBroker
        {
            get
            {
                return false;
            }
        }

        public Dictionary<int, int> AccountBrokerMapping
        {
            get
            {
                return null;
            }
        }

        public CtrlSwapParameters CtrlSwapParameter
        {
            get
            {
                return null;
            }
        }

        public OTCTradeData OTCParameters
        {
            get
            {
                return null;
            }
        }

        public int SettlementCurrency
        {
            get
            {
                return int.MinValue;
            }
        }
        #endregion

        #region Button Click Events

        /// <summary>
        /// Handles the Click event of the BtnHotQty3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnHotQty3_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateQtyFromHotButtons(2);
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
        /// Handles the Click event of the BtnHotQty2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnHotQty2_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateQtyFromHotButtons(1);
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
        /// Handles the Click event of the BtnHotQty1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnHotQty1_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateQtyFromHotButtons(0);
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
        /// Updates the qty from hot buttons.
        /// </summary>
        /// <param name="buttonIndex">Index of the button.</param>
        private void UpdateQtyFromHotButtons(int buttonIndex)
        {
            try
            {
                if (_isNegativeHotButtons)
                {
                    decimal qty = nmrcQuantity.Value - _quickTTPrefs.HotButtonQuantities[buttonIndex];
                    nmrcQuantity.Value = qty > decimal.Zero ? qty : decimal.Zero;
                }
                else
                {
                    decimal qty = nmrcQuantity.Value + _quickTTPrefs.HotButtonQuantities[buttonIndex];
                    nmrcQuantity.Value = qty < nmrcQuantity.Maximum ? qty : nmrcQuantity.Maximum;
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
        /// Handles the Click event of the BtnPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnPosition_Click(object sender, EventArgs e)
        {
            try
            {
                nmrcQuantity.Value = _position;
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
        /// Handles the Click event of the BtnPlusMinus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnPlusMinus_Click(object sender, EventArgs e)
        {
            try
            {
                _isNegativeHotButtons = !_isNegativeHotButtons;
                btnPlusMinus.Text = _isNegativeHotButtons ? "-" : "+";
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
        /// Handles the Click event of the BtnCustomAllocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnCustomAllocation_Click(object sender, EventArgs e)
        {
            try
            {
                qttPresenter.GetAndUpdateAccountWiseQuantity();
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
        /// Handles the Click event of the BtnBuy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnBuy_Click(object sender, EventArgs e)
        {
            try
            {
                _orderSide = _sides[0];
                SendOrder();
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
        /// Handles the Click event of the BtnSell control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnSell_Click(object sender, EventArgs e)
        {
            try
            {
                _orderSide = _sides[1];
                SendOrder();
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
        /// Handles the Click event of the BtnSellShort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnSellShort_Click(object sender, EventArgs e)
        {
            try
            {
                _orderSide = _sides[2];
                SendOrder();
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
        /// Handles the Click event of the BtnBuyToCover control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnBuyToCover_Click(object sender, EventArgs e)
        {
            try
            {
                _orderSide = _sides[3];
                SendOrder();
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
        /// Sends the order.
        /// </summary>
        private void SendOrder()
        {
            try
            {
                qttPresenter.IsTradeSending = true;
                if (!ValidateControlsBeforeTrade())
                {
                    SetLabelMessage(TradingTicketConstants.MSG_SOME_VALUE_ARE_INVALID);
                    return;
                }
                SetLabelMessage(String.Empty);
                if (qttPresenter.CreateNewLiveOrder())
                {
                    SetLabelMessage(TradingTicketConstants.MSG_TRADE_SENT_SUCCESSFULLY);
                    nmrcQuantity.Value = 0;
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
            finally
            {
                qttPresenter.IsTradeSending = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnBid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnBid_Click(object sender, EventArgs e)
        {
            try
            {
                nmrcPrice.Value = _bid;
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
        /// Handles the Click event of the BtnMid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnMid_Click(object sender, EventArgs e)
        {
            try
            {
                nmrcPrice.Value = (_ask + _bid) / 2;
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
        /// Handles the Click event of the BtnAsk control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnAsk_Click(object sender, EventArgs e)
        {
            try
            {
                nmrcPrice.Value = _ask;
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
        /// Hits the event when btnAlgo is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlgo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_algoControlPopUp == null)
                {
                    _algoControlPopUp = new AlgoControlPopUp();
                    int _defaultAlgoType = int.MinValue;
                    if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionAlgoType.ContainsKey(Convert.ToInt32(qttPresenter.CounterPartyId)))
                    {
                        _defaultAlgoType = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionAlgoType[Convert.ToInt32(qttPresenter.CounterPartyId)];
                    }
                    if (btnAlgo.Text != "NONE" && !string.IsNullOrEmpty(qttPresenter.AlgoStrategyId))
                    {
                        _defaultAlgoType = Convert.ToInt32(qttPresenter.AlgoStrategyId);

                    }
                    _algoControlPopUp.OnOkevent += AlgoStrategyTradeDetails_OnOkEvent;
                    _algoControlPopUp.OnOkEvent1 += AlgoStrategyDictionaryDetails_OnOkEvent;
                    _algoControlPopUp.Bind(qttPresenter.CounterPartyId.ToString(), CachedDataManager.GetInstance.GetUnderLyingText(qttPresenter.UnderlyingID), _defaultAlgoType, cmbBroker.Value.ToString(), TagValueDictionary, qttPresenter.AuecID);
                    _algoControlPopUp.FormClosed += algoControlPopUp_FormClosed;
                    _algoControlPopUp.ShowDialog(this);

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
        /// Algoes the strategy dictionary details on ok event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void AlgoStrategyDictionaryDetails_OnOkEvent(object sender, Dictionary<string, string> e)
        {
            try
            {
                _tagValueDictionary = e;
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
        /// Hits the event when Ok button is clicked on AlgoControlPopup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void AlgoStrategyTradeDetails_OnOkEvent(object sender, AlgoStrategyControl e)
        {
            try
            {

                qttPresenter.AlgoStrategyId = e.strategyId;
                qttPresenter.AlgoStrategyName = e.strategyName;
                btnAlgo.Text = e.strategyName;

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
        /// Hits the event when the algoControlPopUp is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void algoControlPopUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_algoControlPopUp != null)
            {
                _algoControlPopUp.FormClosed -= algoControlPopUp_FormClosed;
                _algoControlPopUp = null;
            }
        }

        #endregion

        #region Leave/ValueChanged

        /// <summary>
        /// Handles the ValueChanged event of the cmbOrderType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void CmbOrderType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    if (cmbOrderType.Value == null)
                    {
                        return;
                    }
                    nmrcStop.Enabled = false;
                    lblPrice.Text = "Price";
                    switch (cmbOrderType.Value.ToString())
                    {
                        case FIXConstants.ORDTYPE_Limit:
                            lblPrice.Text = "Limit";
                            break;

                        case FIXConstants.ORDTYPE_Stop:
                            nmrcStop.Enabled = true;
                            break;

                        case FIXConstants.ORDTYPE_Stoplimit:
                            lblPrice.Text = "Limit";
                            nmrcStop.Enabled = true;
                            break;

                        case FIXConstants.ORDTYPE_Pegged:
                        case FIXConstants.ORDTYPE_LimitOrBetter:
                        case FIXConstants.ORDTYPE_LimitWithOrWithout:
                        case FIXConstants.ORDTYPE_OnBasis:
                        case FIXConstants.ORDTYPE_WithOrWithout:
                        case FIXConstants.ORDTYPE_Market:
                        case FIXConstants.ORDTYPE_MarketOnClose:
                        default:
                            break;
                    }
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
        /// Handles the ValueChanged event of the cmbVenue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void CmbVenue_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int venueID = int.MinValue;
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    if (_qTTFieldPreference == null)
                    {
                        qttPresenter.AlgoStrategyId = string.Empty;
                        qttPresenter.AlgoStrategyName = string.Empty;
                    }
                    if (cmbVenue.Value != null && cmbBroker.Value != null)
                    {
                        int.TryParse(cmbVenue.Value.ToString(), out venueID);

                        AlgoControlSetup();
                    }
                }
                if (_quickTTPrefs.UseVenueForLinking)
                {
                    _linkingData.VenueID = venueID;
                    if (_isLinked)
                    {
                        if (HighlightSymbolOnBlotter != null)
                        {
                            HighlightSymbolOnBlotter(this, _linkingData);
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
        /// Handles the ValueChanged event of the nmrcQuantity control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void nmrcQuantity_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateNotionalValues();
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

                if (nmrcPrice.Text != Convert.ToDecimal(0).ToString())
                {
                    errorProvider.SetError(nmrcPrice, String.Empty);
                }
                UpdateNotionalValues();
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
        /// Fills the commission basis.
        /// </summary>
        /// <param name="list">The list.</param>
        public void FillCommissionBasis(IList list)
        {
            try
            {
                _comissionBasisValues = list;
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
        /// Handles the ValueChanged event of the cmbBroker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void CmbBroker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                _tagValueDictionary = new Dictionary<string, string>();
                int brokerID = -1;
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    if (cmbBroker.Value != null)
                    {
                        if (int.TryParse(cmbBroker.Value.ToString(), out brokerID))
                        {
                            qttPresenter.CounterPartyId = brokerID;

                            _calculationBasis = (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis.ContainsKey(qttPresenter.CounterPartyId)
                                ? TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis[qttPresenter.CounterPartyId] : 4);

                            _softCalculationBasis = (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseSoftCommissionBasis.ContainsKey(qttPresenter.CounterPartyId)
                                ? TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseSoftCommissionBasis[qttPresenter.CounterPartyId] : 4);

                            //Set Execution Intruction combo Value, Broker wise
                            if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(qttPresenter.CounterPartyId))
                            {
                                _executionInstructions = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[qttPresenter.CounterPartyId];
                            }
                            //Set Execution Intruction combo Value, User wise
                            else if (TradingTktPrefs.UserTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                            {
                                _executionInstructions = TradingTktPrefs.UserTradingTicketUiPrefs.ExecutionInstruction.Value;
                            }
                            //Set Execution Intruction combo Value, Company wise
                            else if (TradingTktPrefs.CompanyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                            {
                                _executionInstructions = TradingTktPrefs.CompanyTradingTicketUiPrefs.ExecutionInstruction.Value;
                            }
                            qttPresenter.FillVenueCombo();
                            UpdateVenueCombo(qttPresenter.CompanyTradingTicketUiPrefs);

                            btnBroker.Visible = true;
                            if (TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(brokerID) == PranaInternalConstants.ConnectionStatus.CONNECTED)
                            {
                                btnBroker.BackColor = Color.FromArgb(104, 156, 46);
                            }
                            else
                            {
                                btnBroker.BackColor = Color.FromArgb(140, 5, 5);
                            }
                        }
                    }
                }
                _linkingData.BrokerID = brokerID;
                if (_isLinked)
                {
                    if (HighlightSymbolOnBlotter != null)
                    {
                        HighlightSymbolOnBlotter(this, _linkingData);
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

        #endregion

        /// <summary>
        /// Links the with blotter.
        /// </summary>
        public void ToggleBlotterLinking()
        {
            try
            {
                Color foreColor, backColor;

                if (_isLinked)
                {
                    _isLinked = false;
                    if (DeHighlightSymbolOnBlotter != null)
                        DeHighlightSymbolOnBlotter(this, _linkingData);
                    foreColor = Color.FromArgb(155, 187, 89);
                    backColor = Color.FromArgb(33, 44, 57);
                }
                else
                {
                    _isLinked = true;
                    if (HighlightSymbolOnBlotter != null)
                        HighlightSymbolOnBlotter(this, _linkingData);
                    foreColor = _linkingData.ForeColor;
                    backColor = _linkingData.BackColor;
                }
                UpdateFormBorderColors(foreColor, backColor);
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
        private void QuickTradingTicket_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {
                if (DeHighlightSymbolOnBlotter != null)
                    DeHighlightSymbolOnBlotter(this, _linkingData);
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
                if (cmbVenue.Value != null && cmbVenue.Text.ToLower().Equals("algo") && CachedDataManager.GetInstance.IsAlgoBrokerFromID(qttPresenter.CounterPartyId))
                {
                    btnAlgo.Visible = true;

                    if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionAlgoType.ContainsKey(qttPresenter.CounterPartyId))
                    {
                        string algoID = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionAlgoType[qttPresenter.CounterPartyId].ToString();
                        string text = AlgoControlsDictionary.GetInstance().GetAlgoStrategyText(algoID);
                        if (text != "N.A." && text != "Algo Type")
                        {
                            qttPresenter.AlgoStrategyId = algoID;
                            qttPresenter.AlgoStrategyName = text;
                            btnAlgo.Text = text;
                            if (_tagValueDictionary.Count == 0)
                            {
                                AlgoStrategyControls.AlgoStrategy algoStrategy = AlgoControlsDictionary.GetInstance().GetAlgoStrategyDatils(qttPresenter.CounterPartyId.ToString(), algoID.ToString());
                                Dictionary<string, AlgoStrategyUserControl> selectedStrategyCtrls = AlgoStrategyControls.AlgoControlsDictionary.GetInstance().
                                    GetSelectedStrategyControls(qttPresenter.CounterPartyId.ToString(), algoID, CachedDataManager.GetInstance.GetUnderLyingText(qttPresenter.UnderlyingID));

                                foreach (var control in selectedStrategyCtrls)
                                {
                                    string fixTag = control.Key;
                                    string value = string.Empty;
                                    var kvp = control.Value.GetFixValue();
                                    if (kvp != null && kvp.ContainsKey(fixTag))
                                        value = control.Value.GetFixValue()[fixTag];
                                    if (!string.IsNullOrEmpty(value))
                                        _tagValueDictionary.Add(control.Key, control.Value.GetFixValue().Values.FirstOrDefault());
                                }
                                _tagValueDictionary.AddRangeThreadSafely(algoStrategy.StrategyTagValues);
                            }
                        }
                    }
                }
                else
                {
                    qttPresenter.AlgoStrategyId = string.Empty;
                    qttPresenter.AlgoStrategyName = string.Empty;
                    _tagValueDictionary = new Dictionary<string, string>();
                    btnAlgo.Text = "NONE";
                    btnAlgo.Visible = false;
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
        /// Displays the message after timeout.
        /// </summary>
        /// <param name="validatioToken">The token.</param>
        private async void DisplayMessageAfterTimeout(CancellationToken validationToken)
        {
            try
            {
                await Tasks.Task.Delay(TimeSpan.FromSeconds(_validationTimeout));
                if (!validationToken.IsCancellationRequested)
                {
                    validationTokenSource = null;
                    SetLabelMessage(TradingTicketConstants.MSG_SYMBOL_FAILED_TO_VALIDATE_QTT + " " + SymbolText);
                }
            }
            catch (Tasks.TaskCanceledException)
            { }
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
        /// Sets the color of the buttons.
        /// </summary>
        private void SetControlColors()
        {
            try
            {
                btnBuy.ButtonStyle = UIElementButtonStyle.Button3D;
                btnBuy.BackColor = Color.FromArgb(104, 156, 46);
                btnBuy.ForeColor = Color.White;
                btnBuy.UseAppStyling = false;
                btnBuy.UseOsThemes = DefaultableBoolean.False;

                btnSellShort.ButtonStyle = UIElementButtonStyle.Button3D;
                btnSellShort.BackColor = Color.FromArgb(55, 67, 85);
                btnSellShort.ForeColor = Color.White;
                btnSellShort.UseAppStyling = false;
                btnSellShort.UseOsThemes = DefaultableBoolean.False;

                btnBuyToCover.ButtonStyle = UIElementButtonStyle.Button3D;
                btnBuyToCover.BackColor = Color.FromArgb(72, 99, 160);
                btnBuyToCover.ForeColor = Color.White;
                btnBuyToCover.UseAppStyling = false;
                btnBuyToCover.UseOsThemes = DefaultableBoolean.False;

                btnPosition.ButtonStyle = UIElementButtonStyle.Button3D;
                btnPosition.BackColor = Color.FromArgb(55, 67, 85);
                btnPosition.ForeColor = Color.White;
                btnPosition.UseAppStyling = false;
                btnPosition.UseOsThemes = DefaultableBoolean.False;

                btnPlusMinus.ButtonStyle = UIElementButtonStyle.Button3D;
                btnPlusMinus.BackColor = Color.FromArgb(55, 67, 85);
                btnPlusMinus.ForeColor = Color.White;
                btnPlusMinus.UseAppStyling = false;
                btnPlusMinus.UseOsThemes = DefaultableBoolean.False;

                btnHotQty1.ButtonStyle = UIElementButtonStyle.Button3D;
                btnHotQty1.BackColor = Color.FromArgb(55, 67, 85);
                btnHotQty1.ForeColor = Color.White;
                btnHotQty1.UseAppStyling = false;
                btnHotQty1.UseOsThemes = DefaultableBoolean.False;

                btnHotQty2.ButtonStyle = UIElementButtonStyle.Button3D;
                btnHotQty2.BackColor = Color.FromArgb(55, 67, 85);
                btnHotQty2.ForeColor = Color.White;
                btnHotQty2.UseAppStyling = false;
                btnHotQty2.UseOsThemes = DefaultableBoolean.False;

                btnHotQty3.ButtonStyle = UIElementButtonStyle.Button3D;
                btnHotQty3.BackColor = Color.FromArgb(55, 67, 85);
                btnHotQty3.ForeColor = Color.White;
                btnHotQty3.UseAppStyling = false;
                btnHotQty3.UseOsThemes = DefaultableBoolean.False;

                btnAlgo.ButtonStyle = UIElementButtonStyle.Button3D;
                btnAlgo.BackColor = Color.FromArgb(55, 67, 85);
                btnAlgo.ForeColor = Color.White;
                btnAlgo.UseAppStyling = false;
                btnAlgo.UseOsThemes = DefaultableBoolean.False;

                btnBid.ButtonStyle = UIElementButtonStyle.Button3D;
                btnBid.BackColor = Color.FromArgb(55, 67, 85);
                btnBid.ForeColor = Color.White;
                btnBid.UseAppStyling = false;
                btnBid.UseOsThemes = DefaultableBoolean.False;

                btnMid.ButtonStyle = UIElementButtonStyle.Button3D;
                btnMid.BackColor = Color.FromArgb(55, 67, 85);
                btnMid.ForeColor = Color.White;
                btnMid.UseAppStyling = false;
                btnMid.UseOsThemes = DefaultableBoolean.False;

                btnAsk.ButtonStyle = UIElementButtonStyle.Button3D;
                btnAsk.BackColor = Color.FromArgb(55, 67, 85);
                btnAsk.ForeColor = Color.White;
                btnAsk.UseAppStyling = false;
                btnAsk.UseOsThemes = DefaultableBoolean.False;

                btnBroker.BackColor = Color.FromArgb(140, 5, 5);
                btnBroker.ForeColor = Color.White;
                btnBroker.UseAppStyling = false;
                btnBroker.UseOsThemes = DefaultableBoolean.False;

                btnSell.ButtonStyle = UIElementButtonStyle.Button3D;
                btnSell.BackColor = Color.FromArgb(140, 5, 5);
                btnSell.ForeColor = Color.White;
                btnSell.UseAppStyling = false;
                btnSell.UseOsThemes = DefaultableBoolean.False;

                lblErrorMsg.UseAppStyling = false;
                lblErrorMsg.UseOsThemes = DefaultableBoolean.False;
                lblErrorMsg.Appearance.ForeColor = Color.Red;
                lblErrorMsg.Appearance.ForeColorDisabled = Color.Red;


                this.qttL1Strip.BackColor = Color.FromArgb(209, 210, 212);
                this.QuickTradingTicket_Fill_Panel.BackColor = Color.FromArgb(209, 210, 212);
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
        /// Adds the custom preference to account combo.
        /// </summary>
        /// <param name="prefId"></param>
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
                UpdateComboValue(cmbAllocation, string.Empty);
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
        /// Updates the combo value.
        /// </summary>
        /// <param name="cmb">The CMB.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="changeType">Type of the change.</param>
        private void UpdateComboValue(UltraComboEditor cmb, string id)
        {
            try
            {
                if (ValueListUtilities.CheckIfValueExistsInValuelist(cmb.ValueList, id))
                {
                    cmb.Value = id;
                }
                else if (cmb.ValueList != null && cmb.ValueList.ValueListItems.Count > 0)
                {
                    cmb.SelectedIndex = 0;
                }
                else
                    cmb.Value = null;
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

                qttPresenter.CounterPartyId = (cmbBroker.Value != null) ? int.Parse(cmbBroker.Value.ToString()) : int.MinValue;
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

        private void cmb_Leave(object sender, System.EventArgs e)
        {
            CheckIfErrorExists((UltraComboEditor)sender);
        }

        private void cmbAllocation_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //Since -1 and int.MinValue are used in code to denote unallocated therefore initializing with 0
                int accountID = 0;
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    if (ValueListUtilities.CheckIfValueExistsInValuelist(cmbAllocation.ValueList, int.MinValue.ToString()))
                    {
                        if (cmbAllocation.Value == null)
                        {
                            cmbAllocation.Value = int.MinValue;
                            accountID = int.MinValue;
                        }
                        else
                        {
                            accountID = Convert.ToInt32(cmbAllocation.Value.ToString());
                            if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                            {
                                //Update Brokers list
                                List<int> accountsList = new List<int>();

                                //Check Is Default Allocation Pref selected 
                                if (accountID > 0 && CheckIsDefaultAllocationPref())
                                {
                                    ///Update Broker list based on Allocation Pref 
                                    qttPresenter.UpdateBrokerListByAllocationPrefFilter();
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
                                    qttPresenter.UpdateBrokerListByAccountFilter(accountsList);
                                }
                                UpdateBrokerCombo(qttPresenter.TradingTicketUiPrefs, qttPresenter.CompanyTradingTicketUiPrefs);
                            }
                        }
                    }
                    qttPresenter.TicketPresenterBase_UpdateSymbolPositonExposeAndPNL(this, null);
                }
                if (_quickTTPrefs.UseAccountForLinking)
                {
                    _linkingData.AccountID = accountID;
                    if (_isLinked)
                    {
                        if (HighlightSymbolOnBlotter != null)
                        {
                            HighlightSymbolOnBlotter(this, _linkingData);
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
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isDefaultAllocationPref;
        }

        /// <summary>
        /// Checks if error exists.
        /// </summary>
        /// <param name="ultraComboEditor">The ultra combo editor.</param>
        /// <returns></returns>
        private bool CheckIfErrorExists(UltraComboEditor ultraComboEditor)
        {
            bool isValidated = true;
            if (ultraComboEditor.Value != null)
            {
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

        public void ResetTicket()
        {
            try
            {
                nmrcPrice.Value = 0;
                nmrcStop.Value = 0;
                nmrcQuantity.Value = 0;
                cmbAllocation.Value = null;
                cmbBroker.Value = null;
                cmbVenue.Value = null;
                cmbTIF.Value = null;
                cmbOrderType.Value = null;
                _isPriceUpdated = false;
                _ask = 0;
                _bid = 0;
                _fxRate = 0;
                _notionalLocal = 0;
                _notionalBase = 0;
                lblNotionBaseCalc.Text = String.Empty;
                lblNotionLocCalc.Text = String.Empty;
                lblRoundLotValue.Text = String.Empty;
                toggleSwitchRoundLot.Checked = false;
                UpdateCaption();
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
        /// Refreshes the control.
        /// </summary>
        public void RefreshControl(bool refreshOption)
        {
            try
            {
                errorProvider.SetError(nmrcPrice, String.Empty);
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
                if (UIValidation.GetInstance().validate(lblErrorMsg))
                {
                    if (lblErrorMsg.InvokeRequired)
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
                        lblErrorMsg.Text = errMessage;
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
        /// Sets the validationTokenSource
        /// </summary>
        public void SetValidationTokenSource()
        {
            try
            {
                if (validationTokenSource != null)
                    validationTokenSource.Cancel();
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
        public void SetMessageBoxText(string message)
        {
            MessageBox.Show(this, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Awaits the symbol validation.
        /// </summary>
        public void AwaitSymbolValidation()
        {
            try
            {
                SetLabelMessage(TradingTicketConstants.MSG_VALIDATING_SYMBOL);
                if (validationTokenSource != null)
                    validationTokenSource.Cancel();
                validationTokenSource = new CancellationTokenSource();
                DisplayMessageAfterTimeout(validationTokenSource.Token);
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
        /// Stops the live feed.
        /// </summary>
        public void StopLiveFeed()
        {
            try
            {
                qttL1Strip.StopMarketData();
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
        /// Quicks the trading ticket enabled.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void QuickTradingTicketEnabled(bool value)
        {

            try
            {
                if (value)
                    btnSymbolLookup.TabStop = false;
                else
                    btnSymbolLookup.TabStop = true;
                tblPnlMainControls.Enabled = value;
                tblPnlButtonControls.Enabled = value;
                btnBroker.Visible = value;
                lblErrorMsg.Text = String.Empty;
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
        /// update live feed caption.
        /// </summary>
        private delegate void UpdateCaptionDelegate();

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
                        string companyName = qttPresenter.SecmasterObj != null ? qttPresenter.SecmasterObj.LongName + " - " : string.Empty;
                        _titleHelper.RightText = companyName + _quickTTPrefs.InstanceNames[QTTIndex];
                        this.ultraFormManager1.DrawFilter = null;
                        this.ultraFormManager1.DrawFilter = _titleHelper;
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
        /// Handles the Load event of the QuickTradingTicket control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void QuickTradingTicket_Load(object sender, EventArgs e)
        {
            try
            {
                InitControl();
                this.Text = _quickTTPrefs.InstanceNames[QTTIndex];
                _titleHelper = new MainFormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.TitleFont, CustomThemeHelper.UsedFont, 45);
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_QUICK_TRADING_TICKET);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = _titleHelper;
                    this.ultraFormManager1.CreationFilter = new QTTCreationFilter(this);
                }

                _linkingData = new QTTBlotterLinkingData(QTTIndex, _quickTTPrefs.InstanceForeColors[QTTIndex], _quickTTPrefs.InstanceBackColors[QTTIndex]);

                SetHotButtonQuantitites();
                SetControlColors();
                cmbBroker.DrawFilter = new BrokerComboDrawFilter();
                //set visibility of prices strip to false, if permission is not given, PRANA - 26800
                if (!LoginUser.MarketDataTypes.Contains(LiveFeedConstants.LevelOne))
                {
                    qttL1Strip.Visible = false;
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
        /// Loads the colors from prefs.
        /// </summary>
        private void UpdateFormBorderColors(Color foreColor, Color backColor)
        {
            try
            {
                this.ultraFormManager1.FormStyleSettings.FormBorderAppearance.BackColor = backColor;
                this.ultraFormManager1.FormStyleSettings.FormBorderAppearance.BorderColor = backColor;
                this.ultraFormManager1.FormStyleSettings.FormBorderAppearance.BorderColor2 = backColor;
                this.ultraFormManager1.FormStyleSettings.FormBorderAppearance.BorderColor3DBase = backColor;
                this.ultraFormManager1.FormStyleSettings.CaptionAreaAppearance.BackColor = backColor;
                this.ultraFormManager1.FormStyleSettings.CaptionAreaAppearance.BorderColor = backColor;
                this.ultraFormManager1.FormStyleSettings.CaptionAreaAppearance.BorderColor2 = backColor;
                this.ultraFormManager1.FormStyleSettings.CaptionAreaAppearance.BorderColor3DBase = backColor;
                _titleHelper.ForeColor = foreColor;
                this.ultraFormManager1.DrawFilter = null;
                this.ultraFormManager1.DrawFilter = _titleHelper;

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
        /// Sets the hot button quantitites.
        /// </summary>
        private void SetHotButtonQuantitites()
        {
            try
            {
                btnHotQty1.Text = QTTHelper.GetLabelFromQuantity(_quickTTPrefs.HotButtonQuantities[0]);
                btnHotQty2.Text = QTTHelper.GetLabelFromQuantity(_quickTTPrefs.HotButtonQuantities[1]);
                btnHotQty3.Text = QTTHelper.GetLabelFromQuantity(_quickTTPrefs.HotButtonQuantities[2]);
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
        /// Initializes the control.
        /// </summary>
        private void InitControl()
        {
            try
            {
                qttPresenter.InitControl(LoginUser);
                qttPresenter.PriceForComplianceNotAvailable += this.PriceNotAvailableForCompliance;
                qttL1Strip.L1DataResponse += onL1Response;
                qttL1Strip.FXDataResponse += onFXL1Response;
                _quickTTPrefs = DeepCopyHelper.Clone(TradingTktPrefs.QuickTTPrefs);
                _qTTFieldPreference = TradingTktPrefs.QTTFieldPreference[QTTIndex];
                FormClosed += QuickTradingTicket_FormClosed;
                pranaSymbolCtrl.SymbolSearch += pranaSymbolCtrl_SymbolSearch;
                pranaSymbolCtrl.SymbolEntered += pranaSymbolCtrl_SymbolEntered;
                TradeManagerExtension.GetInstance().CounterPartyStatusUpdate += QuickTradingTicket_CounterPartyStatusUpdate;

                QuickTradingTicketEnabled(false);
                ActiveControl = pranaSymbolCtrl;

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
        /// Prices the not available for compliance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void PriceNotAvailableForCompliance(object sender, EventArgs e)
        {
            try
            {
                nmrcPrice.Focus();
                errorProvider.SetIconPadding(nmrcPrice, -35);
                errorProvider.SetError(nmrcPrice, TradingTicketConstants.MSG_PRICES_NOT_AVAILABLE);
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

                    pranaSymbolCtrl.PrevSymbolEntered = e.Value;
                    qttPresenter.UnderlyingSymbol = e.Value2;
                    StopLiveFeed();
                    _linkingData.Symbol = String.Empty;
                    _algoControlPopUp = null;
                    btnAlgo.Text = "NONE";

                    if (_isLinked)
                    {
                        if (HighlightSymbolOnBlotter != null)
                        {
                            HighlightSymbolOnBlotter(this, _linkingData);
                        }
                    }

                    RefreshControl(false);
                    QuickTradingTicketEnabled(false);

                    if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: TradingTicket.pranaSymbolCtrl_SymbolEntered() > Symbol validation request initiated from TT for Symbol: {0}, Time: {1}", e.Value, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                    }

                    qttPresenter.SendValidatedSymbolToSM(e);

                    ResetTicket();
                    if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: TradingTicket.pranaSymbolCtrl_SymbolEntered() > TT level1 strip request initiated from TT for Symbol: {0}, Time: {1}", e.Value, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
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
                qttPresenter.SymbolSearch(e);
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
        /// On the FX l1 response.
        /// </summary>
        /// <param name="l1Data">The l1 data.</param>
        private void onFXL1Response(SymbolData l1Data)
        {
            try
            {
                if (l1Data != null && CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                {
                    _fxRate = (decimal)l1Data.LastPrice;
                    UpdateNotionalValues();
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
                if (l1Data != null && CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                {
                    if (l1Data.LastPrice > 0)
                        qttPresenter.MarketPrice = l1Data.LastPrice;
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

        private void UpdatePricesFromLiveFeed(double ask, double bid, double lastPrice)
        {
            try
            {
                if (!CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                    return;
                _ask = (decimal)ask;
                _bid = (decimal)bid;
                qttPresenter.IsPricingAvailable = true;
                if (!_isPriceUpdated && lastPrice > double.Epsilon)
                {
                    _isPriceUpdated = true;
                    nmrcPrice.Value = (decimal)lastPrice;
                    nmrcStop.Value = (decimal)lastPrice;
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

        private void QuickTradingTicket_CounterPartyStatusUpdate(object sender, EventArgs<CounterPartyDetails> e)
        {

            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            QuickTradingTicket_CounterPartyStatusUpdate(sender, e);
                        }));
                    }
                    else
                    {
                        if (e.Value != null && e.Value.OriginatorType != PranaServerConstants.OriginatorType.Allocation && e.Value.CounterPartyID == qttPresenter.CounterPartyId)
                        {
                            if (e.Value.ConnStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                                btnBroker.BackColor = Color.FromArgb(104, 156, 46);
                            else
                                btnBroker.BackColor = Color.FromArgb(140, 5, 5);
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

        private void QuickTradingTicket_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (FormClosedHandler != null)
                    FormClosedHandler(this, e);
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

        private void UltraFormManager1_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            var element = e.Element.GetDescendant(typeof(ButtonUIElement));
            if (element != null)
            {
                if (element.Rect.X == this.Width - 96)
                {
                    element.ToolTipItem = new ToolTipItem("Save Layout");
                    Image img = global::Prana.TradingTicket.Properties.Resources.SaveLayout_Hover;
                    ((ButtonUIElement)element).Appearance.ImageBackground = img;
                }
                else if (element.Rect.X == this.Width - 118)
                {
                    element.ToolTipItem = new ToolTipItem("Link");
                    Image img = global::Prana.TradingTicket.Properties.Resources.Link_Hover;
                    ((ButtonUIElement)element).Appearance.ImageBackground = img;
                }
            }
        }

        private void ultraFormManager1_MouseLeaveElement(object sender, UIElementEventArgs e)
        {
            var element = e.Element.GetDescendant(typeof(ButtonUIElement));
            if (element != null)
            {
                if (element.Rect.X == this.Width - 96)
                {
                    element.ToolTipItem = new ToolTipItem("Save Layout");
                    Image img = global::Prana.TradingTicket.Properties.Resources.SaveLayout;
                    ((ButtonUIElement)element).Appearance.ImageBackground = img;
                }
                else if (element.Rect.X == this.Width - 118)
                {
                    element.ToolTipItem = new ToolTipItem("Link");
                    Image img = global::Prana.TradingTicket.Properties.Resources.Link;
                    ((ButtonUIElement)element).Appearance.ImageBackground = img;
                }
            }
        }

        /// <summary>
        /// Saves the filed preference.
        /// </summary>
        internal void SaveFiledPref()
        {
            try
            {
                if (qttPresenter.SecmasterObj != null)
                {
                    QTTSaveLayoutPopup popup = new QTTSaveLayoutPopup();
                    DialogResult result = popup.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        string newName = ApplicationConstants.CONST_QUICKTT_PREFIX + popup.InstanceName + ")";
                        if (SendInstanceName != null)
                            SendInstanceName(this, new EventArgs<string>(newName));
                        _quickTTPrefs.InstanceNames[QTTIndex] = newName;
                        _qTTFieldPreference = new QTTFieldPreference();
                        _qTTFieldPreference.Allocation = Int32.Parse(cmbAllocation.Value.ToString());
                        _qTTFieldPreference.Broker = Int32.Parse(cmbBroker.Value.ToString());
                        _qTTFieldPreference.Venue = Int32.Parse(cmbVenue.Value.ToString());
                        _qTTFieldPreference.TIF = cmbTIF.Value.ToString();
                        _qTTFieldPreference.OrderTypeTagValue = cmbOrderType.Value.ToString();
                        _qTTFieldPreference.AlgoStrategyID = qttPresenter.AlgoStrategyId;
                        _qTTFieldPreference.AlgoStrategyName = qttPresenter.AlgoStrategyName;
                        foreach (KeyValuePair<string, string> ele in _tagValueDictionary)
                        {
                            _qTTFieldPreference.Tags.Add(ele.Key, ele.Value);
                        }
                        TradingTktPrefs.QTTFieldPreference[QTTIndex] = _qTTFieldPreference;
                        TradingTktPrefs.SaveQTTFieldPreference();
                        TradingTktPrefs.QuickTTPrefs = DeepCopyHelper.Clone(_quickTTPrefs);
                        UpdateCaption();
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

        public Form Reference()
        {
            return this;
        }

        /// <summary>
        /// Get and Set increment value for quantity field
        /// </summary>
        private decimal qtyIncrement { get; set; }

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
                    ResetTicket();
                    int accountID;
                    if (_qTTFieldPreference != null)
                    {
                        accountID = _qTTFieldPreference.Allocation;
                    }
                    else if (userTradingTicketuserUiPrefs.Account.HasValue)
                    {
                        accountID = userTradingTicketuserUiPrefs.Account.Value;
                    }
                    else if (companyTradingTicketUiPrefs.Account.HasValue)
                    {
                        accountID = companyTradingTicketUiPrefs.Account.Value;
                    }
                    else
                    {
                        accountID = -1;
                    }
                    UpdateComboValue(cmbAllocation, accountID.ToString());

                    qttPresenter.UseQuantityFieldAsNotional = false;
                    UpdateBrokerCombo(userTradingTicketuserUiPrefs, companyTradingTicketUiPrefs);
                    UpdateVenueCombo(companyTradingTicketUiPrefs);

                    string orderType = string.Empty;
                    if (_qTTFieldPreference != null)
                    {
                        orderType = _qTTFieldPreference.OrderTypeTagValue;
                    }
                    else if (userTradingTicketuserUiPrefs.OrderType.HasValue)
                    {
                        orderType = userTradingTicketuserUiPrefs.OrderType.ToString();
                    }
                    else if (companyTradingTicketUiPrefs.OrderType.HasValue)
                    {
                        orderType = companyTradingTicketUiPrefs.OrderType.ToString();
                    }
                    UpdateComboValue(cmbOrderType, orderType);

                    string tif = string.Empty;
                    if (_qTTFieldPreference != null)
                    {
                        tif = _qTTFieldPreference.TIF;
                    }
                    else if (userTradingTicketuserUiPrefs.TimeInForce.HasValue)
                    {
                        tif = userTradingTicketuserUiPrefs.TimeInForce.ToString();
                    }
                    else if (companyTradingTicketUiPrefs.TimeInForce.HasValue)
                    {
                        tif = companyTradingTicketUiPrefs.TimeInForce.ToString();
                    }
                    UpdateComboValue(cmbTIF, tif);

                    if (_qTTFieldPreference != null)
                    {
                        if (_qTTFieldPreference.Tags != null)
                        {
                            _tagValueDictionary = _qTTFieldPreference.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                        }
                        if (!string.IsNullOrEmpty(_qTTFieldPreference.AlgoStrategyID))
                        {
                            qttPresenter.AlgoStrategyId = _qTTFieldPreference.AlgoStrategyID;
                        }
                        if (!string.IsNullOrEmpty(_qTTFieldPreference.AlgoStrategyName))
                        {
                            btnAlgo.Text = _qTTFieldPreference.AlgoStrategyName;
                            qttPresenter.AlgoStrategyName = _qTTFieldPreference.AlgoStrategyName;
                        }
                    }

                    if (userTradingTicketuserUiPrefs.HandlingInstruction.HasValue)
                    {
                        _handlingInstructions = userTradingTicketuserUiPrefs.HandlingInstruction.Value;
                    }
                    else if (companyTradingTicketUiPrefs.HandlingInstruction.HasValue)
                    {
                        _handlingInstructions = companyTradingTicketUiPrefs.HandlingInstruction.Value;
                    }
                    int broker = cmbBroker.Value != null ? Convert.ToInt32(cmbBroker.Value) : int.MinValue;
                    if (broker > 0 && TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(broker))
                    {
                        _executionInstructions = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(broker)];
                    }
                    else if (userTradingTicketuserUiPrefs.ExecutionInstruction.HasValue)
                    {
                        _executionInstructions = userTradingTicketuserUiPrefs.ExecutionInstruction.Value;
                    }
                    else if (companyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                    {
                        _executionInstructions = companyTradingTicketUiPrefs.ExecutionInstruction.Value;
                    }

                    int tradingAccountID = int.MinValue;
                    if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                    {
                        tradingAccountID = CachedDataManager.GetTradingAccountForMasterFund(qttPresenter.FundId);
                        if (tradingAccountID != -1)
                            _tradingAccount = tradingAccountID.ToString();
                    }
                    if (tradingAccountID == -1 || !CachedDataManager.GetInstance.IsShowMasterFundonTT())
                    {
                        if (int.TryParse(userTradingTicketuserUiPrefs.TradingAccount.ToString(), out tradingAccountID))
                        {
                            _tradingAccount = tradingAccountID.ToString();
                        }
                        else if (int.TryParse(companyTradingTicketUiPrefs.TradingAccount.ToString(), out tradingAccountID))
                        {
                            _tradingAccount = tradingAccountID.ToString();
                        }
                    }

                    decimal quantity;
                    if (Decimal.TryParse(userTradingTicketuserUiPrefs.Quantity.ToString(), out quantity))
                    {
                        nmrcQuantity.Value = quantity;
                    }
                    else if (Decimal.TryParse(companyTradingTicketUiPrefs.Quantity.ToString(), out quantity))
                    {
                        nmrcQuantity.Value = quantity;
                    }
                    else
                    {
                        nmrcQuantity.Value = 0;
                    }
                    nmrcPrice.Value = 0;
                    nmrcStop.Value = 0;


                    int strategyID = int.MinValue;
                    if (int.TryParse(userTradingTicketuserUiPrefs.Strategy.ToString(), out strategyID))
                    {
                        _strategy = strategyID;
                    }
                    else if (int.TryParse(companyTradingTicketUiPrefs.Strategy.ToString(), out strategyID))
                    {
                        _strategy = strategyID;
                    }

                    Decimal quantityIncrement = Decimal.MinValue;
                    if (Decimal.TryParse(userTradingTicketuserUiPrefs.IncrementOnQty.ToString(), out quantityIncrement))
                    {
                        qtyIncrement = quantityIncrement;
                    }
                    else if (Decimal.TryParse(companyTradingTicketUiPrefs.IncrementOnQty.ToString(), out quantityIncrement))
                    {
                        qtyIncrement = quantityIncrement;
                    }

                    bool isUseRoundLot = TradingTktPrefs.UserTradingTicketUiPrefs.IsUseRoundLots;
                    string strRoundLot = qttPresenter.SecmasterObj.RoundLot.ToString();
                    lblRoundLotValue.Text = strRoundLot.Contains(".") ? strRoundLot.TrimEnd('0').TrimEnd('.') : strRoundLot;
                    toggleSwitchRoundLot.Checked = isUseRoundLot;
                    nmrcQuantity.Increment = isUseRoundLot ? qttPresenter.SecmasterObj.RoundLot : qtyIncrement;

                    Decimal priceLimitIncrement = Decimal.MinValue;
                    if (Decimal.TryParse(userTradingTicketuserUiPrefs.IncrementOnLimit.ToString(), out priceLimitIncrement))
                    {
                        nmrcPrice.Increment = priceLimitIncrement;
                        nmrcPrice.Increment = priceLimitIncrement;
                    }
                    else if (Decimal.TryParse(companyTradingTicketUiPrefs.IncrementOnLimit.ToString(), out priceLimitIncrement))
                    {
                        nmrcPrice.Increment = priceLimitIncrement;
                        nmrcPrice.Increment = priceLimitIncrement;
                    }

                    Decimal stopPriceIncrement = Decimal.MinValue;
                    if (Decimal.TryParse(userTradingTicketuserUiPrefs.IncrementOnStop.ToString(), out stopPriceIncrement))
                    {
                        nmrcStop.Increment = stopPriceIncrement;
                        nmrcStop.Increment = stopPriceIncrement;
                    }
                    else if (Decimal.TryParse(companyTradingTicketUiPrefs.IncrementOnStop.ToString(), out stopPriceIncrement))
                    {
                        nmrcStop.Increment = stopPriceIncrement;
                        nmrcStop.Increment = stopPriceIncrement;
                    }

                }

                _notes = TradingTktPrefs.TTGeneralPrefs.DefaultInternalComments;
                _brokerNotes = TradingTktPrefs.TTGeneralPrefs.DefaultBrokerComments;
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

        private void toggleSwitchRoundLot_Click(object sender, EventArgs e)
        {
            try
            {
                bool isUseRoundLot = !toggleSwitchRoundLot.Checked;
                nmrcQuantity.Increment = isUseRoundLot ? qttPresenter.SecmasterObj.RoundLot : qtyIncrement;
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
        /// Updates the venue combo.
        /// </summary>
        /// <param name="companyTradingTicketUiPrefs">The company trading ticket UI prefs.</param>
        private void UpdateVenueCombo(TradingTicketUIPrefs companyTradingTicketUiPrefs)
        {
            try
            {
                int venueId;
                if (_qTTFieldPreference != null)
                {
                    venueId = _qTTFieldPreference.Venue;
                }
                else if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue.ContainsKey(Convert.ToInt32(cmbBroker.Value)) &&
                    ValueListUtilities.CheckIfValueExistsInValuelist(cmbVenue.ValueList, TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue[Convert.ToInt32(cmbBroker.Value)].ToString()))
                {
                    venueId = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue[Convert.ToInt32(cmbBroker.Value)];
                }
                else if (companyTradingTicketUiPrefs.Venue.HasValue && ValueListUtilities.CheckIfValueExistsInValuelist(cmbVenue.ValueList, companyTradingTicketUiPrefs.Venue.ToString()))
                {
                    venueId = companyTradingTicketUiPrefs.Venue.Value;
                }
                else if (cmbVenue.ValueList != null && cmbVenue.ValueList.ValueListItems.Count > 0)
                {
                    venueId = Convert.ToInt32(cmbVenue.ValueList.ValueListItems[0].DataValue);
                }
                else
                {
                    venueId = int.MinValue;
                }
                UpdateComboValue(cmbVenue, venueId.ToString());
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
        /// <param name="userTradingTicketuserUiPrefs">The user trading ticketuser UI prefs.</param>
        /// <param name="companyTradingTicketUiPrefs">The company trading ticket UI prefs.</param>
        private void UpdateBrokerCombo(TradingTicketUIPrefs userTradingTicketuserUiPrefs, TradingTicketUIPrefs companyTradingTicketUiPrefs)
        {
            try
            {
                int brokerID;
                if (_qTTFieldPreference != null)
                {
                    brokerID = _qTTFieldPreference.Broker;
                }
                else if (userTradingTicketuserUiPrefs.Broker.HasValue)
                {
                    brokerID = userTradingTicketuserUiPrefs.Broker.Value;
                }
                else if (companyTradingTicketUiPrefs.Broker.HasValue)
                {
                    brokerID = companyTradingTicketUiPrefs.Broker.Value;
                }
                else if (cmbBroker.ValueList != null && cmbBroker.ValueList.ValueListItems.Count > 0)
                    brokerID = Convert.ToInt32(cmbBroker.ValueList.ValueListItems[0].DataValue);
                else
                    brokerID = int.MinValue;

                if (brokerID > 0)
                {
                    if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis.ContainsKey(brokerID))
                    {
                        _calculationBasis = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseCommissionBasis[brokerID];
                    }
                    if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseSoftCommissionBasis.ContainsKey(brokerID))
                    {
                        _softCalculationBasis = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseSoftCommissionBasis[brokerID];
                    }
                    if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(brokerID))
                    {
                        _executionInstructions = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[brokerID]; ;
                    }
                }
                UpdateComboValue(cmbBroker, brokerID.ToString());
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
                        qttPresenter.AssetID = secMasterObj.AssetID;
                        if (validationTokenSource != null)
                        {
                            validationTokenSource.Cancel();
                            validationTokenSource = null;
                        }
                        QuickTradingTicketEnabled(true);
                        if (qttPresenter.AssetID != int.MinValue && qttPresenter.UnderlyingID != int.MinValue)
                        {
                            if (!secMasterObj.IsSecApproved)
                            {
                                lblErrorMsg.Text = TradingTicketConstants.MSG_SECURITY_NOT_APPROVED;
                                qttPresenter.SymbolAction = SecMasterConstants.SecurityActions.APPROVE;
                            }
                            else
                            {
                                ActiveControl = nmrcQuantity;
                                lblErrorMsg.Text = String.Empty;
                                qttPresenter.SymbolAction = SecMasterConstants.SecurityActions.SEARCH;

                                _linkingData.Symbol = secMasterObj.TickerSymbol;
                                if (_isLinked)
                                {
                                    if (HighlightSymbolOnBlotter != null)
                                    {
                                        HighlightSymbolOnBlotter(this, _linkingData);
                                    }
                                }

                                qttPresenter.FillComboBoxes();

                                if (secMasterObj.CurrencyID != CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                                    RequestFXSnapshot(secMasterObj.CurrencyID);
                                else
                                    _fxRate = 1;
                                UpdateNotionalValues();
                                switch (CachedDataManager.GetInstance.GetCurrencyText(secMasterObj.CurrencyID))
                                {
                                    case "EUR":
                                    case "GBP":
                                    case "NZD":
                                    case "AUD":
                                    case "USD":
                                        _fxOperator = Operator.M.ToString();
                                        break;

                                    default:
                                        _fxOperator = Operator.D.ToString();
                                        break;
                                }
                                if (pranaSymbolCtrl.Text.Trim().ToUpper() == secMasterObj.RequestedSymbol.ToUpper())
                                {
                                    if (qttPresenter.MarketDataEanbled)
                                    {
                                        qttPresenter.TicketPresenterBase_UpdateSymbolPositonExposeAndPNL(this, null);
                                    }

                                }
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
        /// Requests the fx snapshot.
        /// </summary>
        /// <param name="currencyID">The currency identifier.</param>
        private void RequestFXSnapshot(int currencyID)
        {
            ConversionRate conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID())
                .GetConversionRateFromCurrencies(currencyID, CachedDataManager.GetInstance.GetCompanyBaseCurrencyID(), 0);

            if (conversionRate != null)
            {
                if (conversionRate.RateValue > double.Epsilon)
                {
                    _fxRate = (decimal)conversionRate.RateValue;
                }
                else if (!String.IsNullOrEmpty(conversionRate.FXeSignalSymbol))
                {
                    qttL1Strip.RequestFXForNotional(conversionRate.FXeSignalSymbol);
                }
            }

        }

        /// <summary>
        /// Handles the MouseHover event of the lblNotionBaseCalc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LblNotionBaseCalc_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lblNotionBaseCalc.Text))
                {
                    UltraToolTipInfo toolTipInfo = new UltraToolTipInfo(_notionalBase.ToString("#,#.00"), ToolTipImage.Default, null, DefaultableBoolean.Default);
                    toolTipManager.SetUltraToolTip(lblNotionBaseCalc, toolTipInfo);
                    toolTipManager.ShowToolTip(lblNotionBaseCalc);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseHover event of the lblNotionLocCalc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LblNotionLocCalc_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lblNotionLocCalc.Text))
                {
                    UltraToolTipInfo toolTipInfo = new UltraToolTipInfo(_notionalLocal.ToString("#,#.00"), ToolTipImage.Default, null, DefaultableBoolean.Default);
                    toolTipManager.SetUltraToolTip(lblNotionLocCalc, toolTipInfo);
                    toolTipManager.ShowToolTip(lblNotionLocCalc);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseHover event of the lblRoundLotValue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LblRoundLotValue_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lblRoundLotValue.Text))
                {
                    UltraToolTipInfo toolTipInfo = new UltraToolTipInfo(lblRoundLotValue.Text, ToolTipImage.Default, null, DefaultableBoolean.Default);
                    toolTipManager.SetUltraToolTip(lblRoundLotValue, toolTipInfo);
                    toolTipManager.ShowToolTip(lblRoundLotValue);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the notional values.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void UpdateNotionalValues()
        {
            _notionalLocal = Quantity * Price * (decimal)qttPresenter.Multiplier;

            if (_notionalLocal > 999999999m)
            {
                lblNotionLocCalc.Text = (Math.Round((_notionalLocal / 1000000000m), 2)).ToString("#,##0.00") + "B";
            }
            else
            {
                lblNotionLocCalc.Text = Math.Round(_notionalLocal).ToString("#,##0.00");
            }

            if (_fxRate > decimal.Zero)
            {
                _notionalBase = _fxOperator == Operator.D.ToString() ? _notionalLocal / _fxRate : _notionalLocal * _fxRate;

                if (_notionalBase > 999999999m)
                {
                    lblNotionBaseCalc.Text = (Math.Round((_notionalBase / 1000000000m), 2)).ToString("#,##0.00") + "B";
                }
                else
                {
                    lblNotionBaseCalc.Text = Math.Round(_notionalBase).ToString("#,##0.00");
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sides">The sides.</param>
        private delegate void FillOrderSideDelegate(DataTable sides);
        public void FillOrderSide(DataTable sides)
        {
            try
            {
                FillOrderSideDelegate mi = FillOrderSide;
                if (UIValidation.GetInstance().validate(btnBuy))
                {
                    if (btnBuy.InvokeRequired)
                    {
                        BeginInvoke(mi, sides);
                    }
                    else
                    {
                        int rowCount = 4;
                        if (sides.Rows.Count < rowCount)
                        {
                            rowCount = sides.Rows.Count;
                            btnSellShort.Visible = false;
                            btnBuyToCover.Visible = false;
                        }
                        else
                        {
                            btnSellShort.Visible = true;
                            btnBuyToCover.Visible = true;
                        }
                        for (int i = 0; i < rowCount; i++)
                        {
                            string sideID = sides.Rows[i][OrderFields.PROPERTY_ORDER_SIDEID].ToString();
                            string sideText = sides.Rows[i][OrderFields.PROPERTY_ORDER_SIDE].ToString();
                            switch (sideID)
                            {
                                case "1"://Buy
                                case "9"://Buy to Open
                                    _sides[0] = sideID;
                                    btnBuy.Text = sideText;
                                    break;
                                case "2"://Sell
                                case "12"://Sell to Close
                                    _sides[1] = sideID;
                                    btnSell.Text = sideText;
                                    break;
                                case "5"://Sell short
                                case "11"://Sell to Open
                                    _sides[2] = sideID;
                                    btnSellShort.Text = sideText;
                                    break;
                                case "10"://Buy to Close
                                    _sides[3] = sideID;
                                    btnBuyToCover.Text = sideText;
                                    break;
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
        /// Gets all.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            IEnumerable<Control> enumerable = controls as IList<Control> ?? controls.ToList();
            return enumerable.SelectMany(ctrl => GetAll(ctrl, type)).Concat(enumerable).Where(c => c.GetType() == type);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="l1data">The l1data.</param>
        private delegate void Level1SnapshotResponseSameThread(SymbolData l1data);

        /// <summary>
        /// Sets the algo details.
        /// </summary>
        /// <param name="auecID">The auec identifier.</param>
        public void SetAlgoDetails(int auecID)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        private delegate void SetSymbolL1StripDeglate(string symbol);
        /// <summary>
        /// Sets the symbol l1 strip.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public void SetSymbolL1Strip(string symbol)
        {
            try
            {
                SetSymbolL1StripDeglate setSymbolL1Strip = SetSymbolL1Strip;
                if (UIValidation.GetInstance().validate(qttL1Strip))
                {
                    if (qttL1Strip.InvokeRequired)
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
                        qttPresenter.IsPricingAvailable = false;
                        string tickerSymbol = symbol;
                        List<string> requestedSymbols = new List<string>();

                        string underlyingSymbol = qttPresenter.UnderlyingSymbol;
                        requestedSymbols.Add(symbol);
                        if (!symbol.Equals(underlyingSymbol))
                            requestedSymbols.Add(underlyingSymbol);
                        qttL1Strip.RequestSnapshotForCompliance(requestedSymbols);

                        qttL1Strip.Symbol = tickerSymbol;
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
        ///
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        private delegate void UpdateSymbolPositionExposeAndPNLDeglate(double position, double exposure, double pnl);
        /// <summary>
        /// Updates the symbol position and expose.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="exposure">The exposure.</param>
        public void UpdateSymbolPositionExposeAndPNL(double position, double exposure, double pnl)
        {
            try
            {
                if (UIValidation.GetInstance().validate(qttL1Strip))
                {
                    UpdateSymbolPositionExposeAndPNLDeglate updateSymbolPositionExposeAndPNL = UpdateSymbolPositionExposeAndPNL;
                    if (qttL1Strip.InvokeRequired)
                    {
                        try
                        {
                            BeginInvoke(updateSymbolPositionExposeAndPNL, position, exposure, pnl);
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
                        qttL1Strip.updatePositionExposureAndPNL(position, exposure, pnl);
                        _position = (decimal)Math.Abs(position);
                        if (_position > nmrcQuantity.Maximum)
                            _position = nmrcQuantity.Maximum;
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

        public void SetAUECDateInTicket(DateTime dateTime)
        {
            _tradeDate = dateTime;
        }

        private void BtnSymbolLookup_MouseLeave(object sender, EventArgs e)
        {
            this.btnSymbolLookup.BackgroundImage = Properties.Resources.SecurityMaster;
        }

        private void BtnSymbolLookup_MouseEnter(object sender, EventArgs e)
        {
            this.btnSymbolLookup.BackgroundImage = Properties.Resources.SecurityMaster_Hover;
        }

        /// <summary>
        /// Handles the Click event of the BtnSymbolLookup control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnSymbolLookup_Click(object sender, EventArgs e)
        {
            try
            {
                qttPresenter.SymbolLookupClick(pranaSymbolCtrl.Text);
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
