using ExportGridsData;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Infragistics.Win.UltraWinSchedule;
using Prana.AlgoStrategyControls;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ShortLocate.Preferences;
using Prana.TradeManager.Extension;
using Prana.TradingTicket.Classes;
using Prana.TradingTicket.TTPresenter;
using Prana.TradingTicket.TTView;
using Prana.Utilities.UI;
using Prana.Utilities.UI.ExtensionUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DayOfWeek = System.DayOfWeek;

namespace Prana.TradingTicket.Forms
{
    /// <summary>
    /// http://www.dreamincode.net/forums/topic/342849-introducing-mvp-model-view-presenter-pattern-winforms/ (We have used this MTT)
    /// https://www.codeproject.com/Articles/31210/Model-View-Presenter 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    /// <seealso cref="Prana.Interfaces.IMultiTradingTicket" />
    /// <seealso cref="Prana.TradingTicket.TTView.IMultiTradingTicketView" />
    /// <seealso cref="System.IDisposable" />
    public partial class MultiTradingTicket : Form, IMultiTradingTicket, IMultiTradingTicketView, IExportGridData
    {
        #region Private Fields

        /// <summary>
        /// The _dictordersbindedsymbolWise symbol wise dictionary to update live feed price in the Limit,Price and Stop columns
        /// </summary>
        private Dictionary<string, List<UltraGridRow>> _dictOrdersBindedSymbolwise;

        /// <summary>
        /// The list of orders which have been binded to the lower multitrade grid. Consider using orders binded 
        /// symbolwise dictionary if need to search for particular symbol.
        /// </summary>
        private OrderBindingList _listOrdersBinded;

        /// <summary>
        /// Local cache which stores the preferences for price-symbol 
        /// </summary>
        private PriceSymbolValidation _priceSymbolSettings;

        private CompanyUser _loginUser;
        private ISecurityMasterServices _securityMaster = null;
        private TradingTicketParent _tradingTicketParent;
        private MTTPresenter _mttPresenter;
        private TranferTradeRules _transferTradeRules;
        private ShortLocateUIPreferences _shortLocatePreferences = null;
        private ctrlShortLocatePrefDataManager Dataobj = new ctrlShortLocatePrefDataManager();
        private AlgoControlPopUp _algoControlPopUp = null;
        private OrderSingle _currentOrder = null;
        private int _expireRowIndex = -1;
        private Dictionary<string, string> _tagValueDictionary = new Dictionary<string, string>();
        private string _algoStrategyID = String.Empty;
        private string _algoStrategyName = String.Empty;
        private string _expireTime = String.Empty;
        private AccountQty _accountqty = null;
        private bool[] _rowsFilterOunt;
        private bool buttonUpdateWasClicked = false;
        private bool isBulkParameterChanged = false;
        private int _mouseXPosition = 0;
        private int _mouseYPosition = 0;

        #endregion

        public MultiTradingTicket()
        {
            InitializeComponent();
        }

        #region Public Fields

        public event EventHandler<EventArgs> UnwireEventsInPresenter;
        public event EventHandler<EventArgs> BindAllDropDowns;
        public event EventHandler FormClosedHandler;
        public event EventHandler<EventArgs> GetPrice;
        public event EventHandler<EventArgs> TagDatabaseManagerWork;
        public event EventHandler<EventArgs<TradingTicketType>> TradeClick;
        public event EventHandler<EventArgs<int>> UpdateVenueOnBrokerChange;
        public event EventHandler<EventArgs<string>> UpdateVenueOnBrokerChangeBulk;
        public event EventHandler<EventArgs<int>> UpdateVenueForFirstRowOnBrokerChange;
        public event EventHandler<EventArgs<int>> UpdateBrokerOnAccountChange;

        public List<UltraGridRow> CheckedUltraGridRows
        {
            get
            {
                return grdTrades.Rows.Where(row => (bool)row.Cells[TradingTicketConstants.COLUMN_SELECTCHECKBOX].Value).ToList();
            }
        }

        public Dictionary<string, List<UltraGridRow>> DictOrdersBindedSymbolwise
        {
            get { return _dictOrdersBindedSymbolwise; }
        }

        public OrderBindingList ListOrdersBinded
        {
            get { return _listOrdersBinded; }
            set { _listOrdersBinded = value; }
        }

        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set { _loginUser = value; }
        }

        public PriceSymbolValidation PriceSymbolSettings
        {
            get { return _priceSymbolSettings; }
            set { _priceSymbolSettings = value; }
        }

        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
            }
        }

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

        public bool IsMTTSettingUp { get; set; }
        public AccountQty BulkAccountQty { get; set; }
        public bool IsEditOrders { get; set; }

        private List<SymbolData> _liveFeed = new List<SymbolData>();
        #endregion

        private void MultiTradingTicket_CounterPartyStatusUpdate(object sender, EventArgs<CounterPartyDetails> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            MultiTradingTicket_CounterPartyStatusUpdate(sender, e);
                        }));
                    }
                    else
                    {
                        if (e.Value != null && e.Value.OriginatorType != PranaServerConstants.OriginatorType.Allocation)
                        {
                            foreach (UltraGridRow row in grdTrades.Rows)
                            {
                                OrderSingle order = row.ListObject as OrderSingle;
                                if (e.Value.CounterPartyID == order.CounterPartyID)
                                {
                                    if (e.Value.ConnStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                                        row.Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Appearance.ForeColor = Color.Green;
                                    else
                                        row.Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Appearance.ForeColor = Color.Red;
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds the numeric up down to the grid.
        /// </summary>
        public void AddNumericUpDown()
        {
            try
            {
                AddNumericUpDownToSpecificColumn(OrderFields.PROPERTY_COMMISSIONRATE);
                AddNumericUpDownToSpecificColumn(OrderFields.PROPERTY_SOFTCOMMISSIONRATE);
                AddNumericUpDownToSpecificColumn(OrderFields.PROPERTY_FXRATE);
                AddNumericUpDownToSpecificColumn(OrderFields.PROPERTY_QUANTITY);
                AddNumericUpDownToSpecificColumn(OrderFields.PROPERTY_EXECUTED_QTY);
                AddNumericUpDownToSpecificColumn(OrderFields.PROPERTY_FXRATE);
                AddNumericUpDownToSpecificColumn(OrderFields.PROPERTY_STOP_PRICE);
                AddNumericUpDownToSpecificColumn(TradingTicketConstants.COLUMN_LIMITPRICE);
                AddNumericUpDownToSpecificColumn(TradingTicketConstants.COLUMN_MARKETPRICE);
                AddNumericUpDownToSpecificColumn(OrderFields.PROPERTY_AVGPRICE);
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
        /// Handles the Click event of the BtnCustomAllocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnCustomAllocation_Click(object sender, EventArgs e)
        {
            try
            {
                SetAccountWiseAllocationPercent();
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
        ///  Set Account Wise Allocation Percent
        /// </summary>
        private void SetAccountWiseAllocationPercent()
        {
            try
            {
                if (_accountqty == null || _accountqty.IsDisposed)
                {
                    _accountqty = new AccountQty(TradingTicketConstants.MTT_BULK_UPDATE_SYMBOL);
                    _accountqty.AllocationManager = _mttPresenter.AllocationProxy.InnerChannel;
                    _accountqty.RemoveColumnAllocatedQuantity();
                }
                _accountqty.TotalAllocationQty = 0;
                _accountqty.LoadPositions();
                _accountqty.SetAllocationPercentage(cmbAllocation.Text);
                _accountqty.StartPosition = FormStartPosition.CenterParent;

                DialogResult dgResult = _accountqty.ShowDialog();
                if (dgResult == DialogResult.OK)
                {
                    string error = _accountqty.Error;
                    if (!string.IsNullOrEmpty(error))
                    {
                        MessageBox.Show(error, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (_accountqty.SumPercentage == 100)
                    {
                        AddCustomPreferenceToAccountCombo(_accountqty.AllocationOperationPreference.OperationPreferenceId);
                    }
                }
                else if (dgResult == DialogResult.Cancel)
                {
                    string error = _accountqty.Error;
                    if (!string.IsNullOrEmpty(error))
                    {
                        MessageBox.Show(error, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Add Custom Preference to Account ComboBox
        /// </summary>
        /// <param name="prefId">The prefId.</param>
        private void AddCustomPreferenceToAccountCombo(int prefId)
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
        /// Adds the drop down to given index for column.
        /// </summary>
        /// <param name="dropDown">The drop down.</param>
        /// <param name="index">The index.</param>
        /// <param name="columnName">Name of the column.</param>
        public void AddDropDownToGivenIndexForColumn(ValueList valueList, int index, string columnName)
        {
            try
            {
                if (columnName == OrderFields.PROPERTY_ORDER_SIDE)
                {
                    OrderSingle order = grdTrades.Rows[index].ListObject as OrderSingle;

                    if (order.Quantity - order.UnsentQty > 0)
                    {
                        grdTrades.Rows[index].Cells[columnName].Activation = Activation.NoEdit;
                    }
                }
                grdTrades.Rows[index].Cells[columnName].ValueList = valueList;
                grdTrades.DisplayLayout.Bands[0].Columns[columnName].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                grdTrades.DisplayLayout.Bands[0].Columns[columnName].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                grdTrades.DisplayLayout.Bands[0].Columns[columnName].AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith;
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
        /// Adds the drop down for given column.
        /// </summary>
        /// <param name="dropdown">The dropdown.</param>
        /// <param name="columnName">Name of the column.</param>
        public void AddDropDownForGivenColumn(ValueList valueList, string columnName)
        {
            try
            {
                grdTrades.DisplayLayout.Bands[0].Columns[columnName].ValueList = valueList;
                if (columnName.Equals(OrderFields.PROPERTY_SOFTCOMMISSIONCALCBASIS) || columnName.Equals(OrderFields.PROPERTY_CALCBASIS))
                    grdTrades.DisplayLayout.Bands[0].Columns[columnName].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                else
                    grdTrades.DisplayLayout.Bands[0].Columns[columnName].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                grdTrades.DisplayLayout.Bands[0].Columns[columnName].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                grdTrades.DisplayLayout.Bands[0].Columns[columnName].AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith;
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
        /// Gets order single based on given index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public UltraGridRow GetOrderSingleBasedOnIndex(int index)
        {
            UltraGridRow urGridRow = null;
            try
            {
                if (grdTrades.Rows.Count > index)
                    urGridRow = grdTrades.Rows[index];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return urGridRow;
        }

        /// <summary>
        /// Get the row from grid based on given index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public UltraGridRow GetRowFromIndex(int index)
        {
            try
            {
                if (grdTrades.Rows.Count > index)
                {
                    return grdTrades.Rows[index];
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

        public Form Reference()
        {
            return this;
        }

        /// <summary>
        /// Closes the multi trading ticket and clear data source.
        /// </summary>
        public void ClearMultiTradingTicket()
        {
            try
            {
                if (UnwireEventsInPresenter != null)
                {
                    UnwireEventsInPresenter(this, EventArgs.Empty);
                }
                _mttPresenter = null;
                if (ListOrdersBinded != null)
                    ListOrdersBinded.Clear();
                if (_dictOrdersBindedSymbolwise != null)
                    _dictOrdersBindedSymbolwise.Clear();
                if (grdTrades.DataSource != null)
                    grdTrades.DataSource = null;
                grdTrades.ResetDisplayLayout();
                grdTrades.Layouts.Clear();
                _isGridEdited = false;
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
        /// Resets the multirading ticket.
        /// As we can only have one instance of MTT, if its already opened then reset MTT.
        /// Clear every private fields in this method
        /// </summary>
        /// <param name="isFormLoadRequired">if set to <c>true</c> [then MTT is already opened and we need to clear details and then assign the incoming data source].</param>
        public void ResetMultiTradingTicket(bool isFormLoadRequired)
        {
            try
            {
                if (isFormLoadRequired)
                {
                    MultiTradingTicket_Shown(null, null);
                }
                else
                {
                    ClearMultiTradingTicket();
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
        /// Sets the status bar message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SetStatusBarMessage(string message)
        {
            try
            {
                if (ultraStatusBar.InvokeRequired)
                {
                    MethodInvoker del = delegate
                    {
                        ultraStatusBar.Text = message;
                    };
                    Invoke(del);
                    return;
                }
                ultraStatusBar.Text = DateTime.Now.ToString() + ": " + message;
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
        /// Sets the trade attribute captions.
        /// As TradeAttribute name can be changed from allocation, so to update the caption accordingly.
        /// </summary>
        /// <param name="lblTA1">The trade attribute label a1.</param>
        /// <param name="lblTA2">The trade attribute t a2.</param>
        /// <param name="lblTA3">The trade attribute t a3.</param>
        /// <param name="lblTA4">The trade attribute t a4.</param>
        /// <param name="lblTA5">The trade attribute t a5.</param>
        /// <param name="lblTA6">The trade attribute t a6.</param>
        public void SetTradeAttributeCaptions(string lblTA1, string lblTA2, string lblTA3, string lblTA4, string lblTA5, string lblTA6)
        {
            try
            {
                if (InvokeRequired)
                {
                    MethodInvoker del = delegate
                    {
                        SetTradeAttributeCaptions(lblTA1, lblTA2, lblTA3, lblTA4, lblTA5, lblTA6);
                    };
                    BeginInvoke(del);
                    return;
                }
                grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE1].Header.Caption = lblTA1;
                grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE2].Header.Caption = lblTA2;
                grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE3].Header.Caption = lblTA3;
                grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE4].Header.Caption = lblTA4;
                grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE5].Header.Caption = lblTA5;
                grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE6].Header.Caption = lblTA6;

                lblTradeAttribute1.Text = lblTA1;
                lblTradeAttribute2.Text = lblTA2;
                lblTradeAttribute3.Text = lblTA3;
                lblTradeAttribute4.Text = lblTA4;
                lblTradeAttribute5.Text = lblTA5;
                lblTradeAttribute6.Text = lblTA6;

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

        public void SetLimitIncrement(decimal LimitIncrement)
        {
            try
            {
                UltraNumericEditor numLimit = (UltraNumericEditor)grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_LIMITPRICE].EditorComponent;
                numLimit.SpinIncrement = LimitIncrement;
                numLimit.MaskInput = "nnn,nnn,nnn.nnnn";
                UltraNumericEditor numPrice = (UltraNumericEditor)grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_AVGPRICE].EditorComponent;
                numPrice.MaskInput = "nnn,nnn,nnn.nnnn";
                numPrice.SpinIncrement = LimitIncrement;
                nmrcLimit.Increment = LimitIncrement;
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
        public void SetStopIncrement(decimal StopIncrement)
        {
            try
            {
                UltraNumericEditor num = (UltraNumericEditor)grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_STOP_PRICE].EditorComponent;
                num.SpinIncrement = StopIncrement;
                num.MaskInput = "nnn,nnn,nnn.nnnn";
                nmrcStop.Increment = StopIncrement;
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
        public void SetQuantityIncrement(decimal qtyIncrement)
        {
            try
            {
                UltraNumericEditor num = (UltraNumericEditor)grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_QUANTITY].EditorComponent;
                num.SpinIncrement = qtyIncrement;
                num.MaskInput = "nnn,nnn,nnn.nnnn";
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

        private bool CheckIfErrorExists(UltraComboEditor ultraComboEditor)
        {
            bool isValidated = true;
            if (ultraComboEditor.Value != null)
            {
                buttonUpdateWasClicked = false;
                isBulkParameterChanged = true;
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

        /// <summary>
        ///Assign price based on snapshot response.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SnapshotResponse(SymbolData data)
        {
            try
            {
                if (IsMTTSettingUp)
                {
                    _liveFeed.Add(data);
                    return;
                }

                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker del = delegate
                        {
                            UpdateLiveFeedPrices(data);
                        };
                        BeginInvoke(del);
                        return;
                    }
                    UpdateLiveFeedPrices(data);
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

        private void UpdateInitialPrices()
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker del = delegate
                        {
                            UpdateInitialPrices();
                        };
                        BeginInvoke(del);
                        return;
                    }
                    foreach (SymbolData data in _liveFeed)
                        UpdateLiveFeedPrices(data);
                    _liveFeed.Clear();
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
        /// Updates the live feed prices.
        /// </summary>
        /// <param name="data">The data.</param>
        private void UpdateLiveFeedPrices(SymbolData data)
        {
            try
            {
                if (data != null)
                {
                    if (_dictOrdersBindedSymbolwise.ContainsKey(data.Symbol))
                    {
                        bool isGridEdited = _isGridEdited;
                        List<UltraGridRow> listOrdersToUpdate = _dictOrdersBindedSymbolwise[data.Symbol];
                        foreach (UltraGridRow ulr in listOrdersToUpdate)
                        {
                            //Change the value in the limit text box only if it has ZERO
                            //We have set limit text box to ZERO whenever the symbol changes
                            // In case if last price is 0 from live feed, we need to pickup side dependent values.
                            OrderSingle ordUpdateRow = ulr.ListObject as OrderSingle;
                            if (ulr.Cells[OrderFields.PROPERTY_ORDER_SIDE].Value != null)
                            {
                                double value = 0;
                                switch (ordUpdateRow.OrderSideTagValue)
                                {
                                    case FIXConstants.SIDE_Buy:
                                    case FIXConstants.SIDE_BuyMinus:
                                    case FIXConstants.SIDE_Buy_Closed:
                                    case FIXConstants.SIDE_Buy_Open:
                                        if (data.Ask == 0.0 && TradingTktPrefs.TTGeneralPrefs.IsPopulatelastPriceInPriceWhenAskORBidIsZero)
                                        {
                                            value = data.LastPrice;
                                        }
                                        else if (data.Ask != double.MinValue)
                                        {
                                            value = data.Ask;
                                        }
                                        ulr.Cells[OrderFields.PROPERTY_AVGPRICE].Value = value;

                                        if (ulr.Cells[OrderFields.PROPERTY_ORDER_TYPE].Text != TradingTicketConstants.LIT_LIMIT && ulr.Cells[OrderFields.PROPERTY_ORDER_TYPE].Text != TradingTicketConstants.LIT_STOP && ulr.Cells[OrderFields.PROPERTY_ORDER_TYPE].Text != TradingTicketConstants.LIT_STOP_LIMIT)
                                            ulr.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Value = value;

                                        break;

                                    case FIXConstants.SIDE_Sell:
                                    case FIXConstants.SIDE_SellShort:
                                    case FIXConstants.SIDE_SellShortExempt:
                                    case FIXConstants.SIDE_Sell_Closed:
                                    case FIXConstants.SIDE_Sell_Open:
                                        if (data.Bid == 0.0 && TradingTktPrefs.TTGeneralPrefs.IsPopulatelastPriceInPriceWhenAskORBidIsZero)
                                        {
                                            value = data.LastPrice;
                                        }
                                        else if (data.Bid != double.MinValue)
                                        {
                                            value = data.Bid;
                                        }
                                        //Refer to:-[CI-7061]  ulr.Cells[TradingTicketConstants.LIT_PRICE.ToUpper()].Value = value;
                                        ulr.Cells[OrderFields.PROPERTY_AVGPRICE].Value = value;
                                        if (ulr.Cells[OrderFields.PROPERTY_ORDER_TYPE].Text != TradingTicketConstants.LIT_LIMIT && ulr.Cells[OrderFields.PROPERTY_ORDER_TYPE].Text != TradingTicketConstants.LIT_STOP && ulr.Cells[OrderFields.PROPERTY_ORDER_TYPE].Text != TradingTicketConstants.LIT_STOP_LIMIT)
                                            ulr.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Value = value;

                                        break;
                                    default:
                                        if (data.LastPrice != double.MinValue)
                                        {
                                            if (ulr.Cells[OrderFields.PROPERTY_ORDER_TYPE].Text != TradingTicketConstants.LIT_LIMIT && ulr.Cells[OrderFields.PROPERTY_ORDER_TYPE].Text != TradingTicketConstants.LIT_STOP && ulr.Cells[OrderFields.PROPERTY_ORDER_TYPE].Text != TradingTicketConstants.LIT_STOP_LIMIT)
                                                ulr.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Value = data.LastPrice;
                                            ulr.Cells[OrderFields.PROPERTY_AVGPRICE].Value = data.LastPrice;
                                        }
                                        break;
                                }
                            }


                            if (_priceSymbolSettings != null)
                            {
                                if (data.LastPrice != double.MinValue)
                                {
                                    ulr.Cells[TradingTicketConstants.COLUMN_MARKETPRICE].Value = data.LastPrice;
                                }

                                if (_priceSymbolSettings.ValidateSymbolCheck)
                                {
                                    if (data.Symbol == ordUpdateRow.Symbol && data.UnderlyingCategory != Underlying.None)
                                    {
                                        if (ordUpdateRow.AssetID != (int)data.CategoryCode || ordUpdateRow.UnderlyingID != (int)data.UnderlyingCategory)
                                        {
                                            ordUpdateRow.AUECID = int.MinValue;
                                            return;
                                        }
                                    }
                                }
                            }

                            if (data.CategoryCode == AssetCategory.EquityOption)
                            {
                                OptionSymbolData optiondata = data as OptionSymbolData;
                                if (data != null)
                                {
                                    if (optiondata.PutOrCall != OptionType.NONE)
                                    {
                                        ordUpdateRow.PutOrCalls = optiondata.PutOrCall.ToString();
                                    }
                                    if (optiondata.StrikePrice != double.MinValue)
                                    {
                                        ordUpdateRow.StrikePrice = optiondata.StrikePrice;
                                    }
                                    if (optiondata.ExpirationDate != DateTimeConstants.MinValue)
                                    {
                                        ordUpdateRow.ExpirationDate = optiondata.ExpirationDate;
                                        ordUpdateRow.MaturityDay = optiondata.ExpirationDate.ToString();
                                    }
                                }
                            }
                        }
                        _isGridEdited = isGridEdited;
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
        /// Enable or Disable MTT Components.
        /// </summary>
        /// <param name="isMTTEnabled">Shows if MTT is enabled or disabled</param>
        /// <returns></returns>
        public void EnableOrDisableMTT(bool isMTTEnabled)
        {
            try
            {
                if (grdTrades.InvokeRequired)
                {
                    grdTrades.Invoke((MethodInvoker)delegate ()
                    {
                        EnableOrDisableMTT(isMTTEnabled);
                        return;
                    });
                }
                grdTrades.Enabled = isMTTEnabled;
                grpBoxBulkUpdate.Enabled = isMTTEnabled;
                tblOrderButtons.Enabled = isMTTEnabled;
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
        /// The function is called when the form is first called from NirvanaMain.
        /// </summary>
        /// <param name="orderList">The list of orders selected by the user to trade</param>
        public void SetMTTFromNirvanaMain(OrderBindingList orderList, bool isEdit)
        {
            try
            {
                foreach (OrderSingle orderSingle in orderList)
                {
                    orderSingle.CumQty = orderSingle.Quantity;
                    orderSingle.AvgPrice = 0.0;
                }
                if (orderList.Count == 0)
                {
                    ultraPanel1.Enabled = false;
                    grdTrades.Enabled = false;
                }
                else
                {
                    ultraPanel1.Enabled = true;
                    grdTrades.Enabled = true;
                }
                IsEditOrders = isEdit;
                ListOrdersBinded = orderList;
                grdTrades.DisplayLayout.MaxBandDepth = 1;
                grdTrades.DataSource = _listOrdersBinded;
                _dictOrdersBindedSymbolwise = new Dictionary<string, List<UltraGridRow>>();
                foreach (UltraGridRow ulr in grdTrades.Rows)
                {
                    if (_dictOrdersBindedSymbolwise.ContainsKey(ulr.Cells[OrderFields.PROPERTY_SYMBOL].Value.ToString()))
                    {
                        _dictOrdersBindedSymbolwise[ulr.Cells[OrderFields.PROPERTY_SYMBOL].Value.ToString()].Add(ulr);
                    }
                    else
                    {
                        List<UltraGridRow> tempList = new List<UltraGridRow>();
                        tempList.Add(ulr);
                        _dictOrdersBindedSymbolwise.Add(ulr.Cells[OrderFields.PROPERTY_SYMBOL].Value.ToString(), tempList);
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
        /// Updates the value of a cell
        /// </summary>
        /// <param name="index"></param>
        /// <param name="columnName"></param>
        /// <param name="result"></param>
        public void UpdateCell(int index, string columnName, string result)
        {
            try
            {
                if (grdTrades.InvokeRequired)
                {
                    MethodInvoker del = delegate
                    {
                        UpdateCell(index, columnName, result);
                    };
                    BeginInvoke(del);
                    return;
                }
                grdTrades.Rows[index].Cells[columnName].Value = result;

                if (!string.IsNullOrEmpty(result))
                {
                    if (columnName == OrderFields.PROPERTY_COUNTERPARTY_NAME && result == "Default Broker(s)")
                    {
                        return;
                    }
                    string ColumnError = string.Empty;
                    if (grdTrades.Rows[index].Cells[columnName].ValueListResolved != null)
                    {
                        ColumnError = "Invalid " + grdTrades.DisplayLayout.Bands[0].Columns[columnName].Header.Caption + "!";
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(grdTrades.Rows[index].Cells[columnName].ValueListResolved, grdTrades.Rows[index].Cells[columnName].Text))
                        {
                            grdTrades.Rows[index].DataErrorInfo.SetColumnError(columnName, null);
                            if (columnName.Equals(OrderFields.PROPERTY_COUNTERPARTY_NAME))
                            {
                                if (UpdateVenueOnBrokerChange != null)
                                    UpdateVenueOnBrokerChange(this, new EventArgs<int>(index));
                            }
                        }
                        else
                        {
                            grdTrades.Rows[index].Cells[columnName].Value = null;
                            if (!(columnName.Equals(OrderFields.PROPERTY_STRATEGY) && string.IsNullOrEmpty(grdTrades.Rows[index].Cells[columnName].Text)))
                                grdTrades.Rows[index].DataErrorInfo.SetColumnError(columnName, ColumnError);
                            if (columnName.Equals(OrderFields.PROPERTY_COUNTERPARTY_NAME))
                            {
                                grdTrades.Rows[index].Cells[OrderFields.PROPERTY_VENUE].Value = null;
                            }
                            if (columnName.Equals(OrderFields.PROPERTY_ACCOUNT))
                            {
                                grdTrades.Rows[index].Cells[OrderFields.PROPERTY_ACCOUNT].Value = TradingTicketConstants.LIT_UNALLOCATED;
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
        /// Updates the order list with sm data.
        /// </summary>
        /// <param name="orderList">The order list.</param>
        //public async void UpdateOrderListWithSMData(OrderBindingList orderList)
        //{
        //    try
        //    {
        //        if (_securityMaster != null && _securityMaster.IsConnected)
        //        {
        //            foreach (OrderSingle orderSingle in orderList)
        //            {
        //                SecMasterRequestObj reqObj = new SecMasterRequestObj();
        //                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
        //                reqObj.AddData(orderSingle.Symbol, (int)ApplicationConstants.SymbologyCodes.TickerSymbol);
        //                List<SecMasterBaseObj> secMasterBaseObjList = _securityMaster.SendRequestList(reqObj);
        //                if (secMasterBaseObjList.Count == 0)
        //                {
        //                    await System.Threading.Tasks.Task.Delay(100);
        //                    if (_securityMaster != null && _securityMaster.IsConnected)
        //                        secMasterBaseObjList = _securityMaster.SendRequestList(reqObj);
        //                    else return;
        //                }
        //                foreach (SecMasterBaseObj secMasterBaseObj in secMasterBaseObjList)
        //                {
        //                    orderSingle.ExchangeID = secMasterBaseObj.ExchangeID;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Adds the check box select to the grid.
        /// </summary>
        private void AddCheckBoxinGrid()
        {
            try
            {
                if (!grdTrades.DisplayLayout.Bands[0].Columns.Exists(TradingTicketConstants.COLUMN_SELECTCHECKBOX))
                    grdTrades.DisplayLayout.Bands[0].Columns.Add(TradingTicketConstants.COLUMN_SELECTCHECKBOX, "");
                else
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_SELECTCHECKBOX].Header.Caption = String.Empty;
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_SELECTCHECKBOX].DataType = typeof(bool);
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_SELECTCHECKBOX].Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_SELECTCHECKBOX].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_SELECTCHECKBOX].CellActivation = Activation.AllowEdit;
                SetCheckBoxAtFirstPosition(TradingTicketConstants.COLUMN_SELECTCHECKBOX);

                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_SELECTCHECKBOX].SetHeaderCheckedState(grdTrades.Rows, true);
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
        /// Handles the AfterHeaderCheckState event of the grdTrades control.
        /// It is used to check only filtered rows on the grid.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AfterHeaderCheckStateChangedEventArgs"/> instance containing the event data.</param>
        private void grdTrades_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                UltraGridRow[] Rows = grdTrades.Rows.GetFilteredOutNonGroupByRows();

                if (_rowsFilterOunt != null)
                {
                    for (int i = 0; i < Rows.Length; i++)
                    {
                        Rows[i].Cells["SelectCheckbox"].Value = _rowsFilterOunt[i];
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
        /// Handles the BeforeHeaderCheckState event of the grdTrades control.
        /// It is used to check only filtered rows on the grid.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BeforeHeaderCheckStateChangedEventArgs"/> instance containing the event data.</param>
        private void grdTrades_BeforeHeaderCheckStateChanged(object sender, BeforeHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                UltraGridRow[] RowsFilterOut = grdTrades.Rows.GetFilteredOutNonGroupByRows();
                _rowsFilterOunt = new bool[RowsFilterOut.Length];
                for (int i = 0; i < RowsFilterOut.Length; i++)
                {
                    _rowsFilterOunt[i] = (bool)RowsFilterOut[i].Cells["SelectCheckbox"].Value;
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
        /// Adds the column to grid.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="columnCaption">The column caption.</param>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="dataType">Type of the data.</param>
        private void AddColumnToGrid(string columnName, string columnCaption, string groupName, Type dataType)
        {
            try
            {
                if (!grdTrades.DisplayLayout.Bands[0].Columns.Exists(columnName))
                    grdTrades.DisplayLayout.Bands[0].Columns.Add(columnName, columnCaption);
                else
                    grdTrades.DisplayLayout.Bands[0].Columns[columnName].Header.Caption = columnCaption;
                grdTrades.DisplayLayout.Bands[0].Columns[columnName].DataType = dataType;
                grdTrades.DisplayLayout.Bands[0].Columns[columnName].CellActivation = Activation.ActivateOnly;
                if (!String.IsNullOrEmpty(groupName))
                    grdTrades.DisplayLayout.Bands[0].Columns[columnName].Group = grdTrades.DisplayLayout.Bands[0].Groups[groupName];
                grdTrades.DisplayLayout.Bands[0].Columns[columnName].Hidden = false;
                grdTrades.DisplayLayout.Bands[0].Columns[columnName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
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
        /// Adds the numeric up down to specific column.
        /// </summary>
        /// <param name="columnKey">The column key.</param>
        private void AddNumericUpDownToSpecificColumn(string columnKey)
        {
            try
            {
                if (!grdTrades.DisplayLayout.Bands[0].Columns.Exists(columnKey)) return;
                UltraNumericEditor ultraNumEditor = new UltraNumericEditor
                {
                    SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always,
                    PromptChar = ' ',
                    NumericType = NumericType.Double,
                    Nullable = false
                };
                grdTrades.DisplayLayout.Bands[0].Columns[columnKey].UseEditorMaskSettings = false;
                grdTrades.DisplayLayout.Bands[0].Columns[columnKey].PromptChar = ' ';
                grdTrades.DisplayLayout.Bands[0].Columns[columnKey].Format = "###,###,##0.0000";
                grdTrades.DisplayLayout.Bands[0].Columns[columnKey].EditorComponent = ultraNumEditor;
                grdTrades.DisplayLayout.Bands[0].Columns[columnKey].CellActivation = Activation.AllowEdit;
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
        /// Allows the edit column.
        /// </summary>
        /// <param name="grd">The Grid.</param>
        /// <param name="columnName">Name of the column.</param>
        private void AllowEditColumn(UltraGrid grd, string columnName)
        {
            try
            {
                if (grd.DisplayLayout.Bands[0].Columns.Exists(columnName))
                {
                    grd.DisplayLayout.Bands[0].Columns[columnName].CellActivation = Activation.AllowEdit;
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
        /// Handles the Click event of the btnDoneAway control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnDoneAway_Click(object sender, EventArgs e)
        {
            try
            {
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && ComplianceCacheManager.GetPreTradeCheck(_loginUser.CompanyUserID) && ComplianceCacheManager.GetApplyToManualPermission(_loginUser.CompanyUserID))
                {
                    MessageBox.Show(ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE_COMPLIANCE, ClientLevelConstants.HEADER_MARKET_DATA_ALERT, MessageBoxButtons.OK);
                    return;
                }

                if (TradeClick != null)
                {
                    TradeClick(this, new EventArgs<TradingTicketType>(TradingTicketType.Manual));
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
        /// Handles the Click event of the btnStage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnStage_Click(object sender, EventArgs e)
        {
            try
            {
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && ComplianceCacheManager.GetPreTradeCheckStaging(_loginUser.CompanyUserID))
                {
                    MessageBox.Show(ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE_COMPLIANCE, ClientLevelConstants.HEADER_MARKET_DATA_ALERT, MessageBoxButtons.OK);
                    return;
                }
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && ComplianceCacheManager.GetPreTradeCheckStaging(_loginUser.CompanyUserID))
                {
                    CustomMessageBox popUpMessage = new CustomMessageBox(ClientLevelConstants.HEADER_MARKET_DATA_ALERT, ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE_COMPLIANCE, false, string.Empty, FormStartPosition.CenterScreen);
                    popUpMessage.ShowDialog();
                    return;
                }

                if (TradeClick != null)
                {
                    TradeClick(this, new EventArgs<TradingTicketType>(TradingTicketType.Stage));
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
        /// Handles the Click event of the btnReplace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnReplace_Click(object sender, EventArgs e)
        {
            try
            {
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && ComplianceCacheManager.GetPreTradeCheckStaging(_loginUser.CompanyUserID))
                {
                    MessageBox.Show(ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE_COMPLIANCE, ClientLevelConstants.HEADER_MARKET_DATA_ALERT, MessageBoxButtons.OK);
                    return;
                }
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && ComplianceCacheManager.GetPreTradeCheckStaging(_loginUser.CompanyUserID))
                {
                    CustomMessageBox popUpMessage = new CustomMessageBox(ClientLevelConstants.HEADER_MARKET_DATA_ALERT, ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE_COMPLIANCE, false, string.Empty, FormStartPosition.CenterScreen);
                    popUpMessage.ShowDialog();
                    return;
                }

                if (TradeClick != null)
                {
                    TradeClick(this, new EventArgs<TradingTicketType>(TradingTicketType.Stage));
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
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Click event of the btnRefreshPrice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnRefreshPrice_Click(object sender, EventArgs e)
        {
            try
            {
                if (MarketDataValidation.CheckMarketDataPermissioning())
                {
                    if (GetPrice != null && grdTrades.Rows != null)
                    {
                        GetPrice(sender, EventArgs.Empty);
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
        /// Handles the Click event of the btnSend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (buttonUpdateWasClicked == false && isBulkParameterChanged)
                {
                    Prana.TradeManager.PromptWindow promptWindow = new Prana.TradeManager.PromptWindow("Some field values were changed in the bulk update section but are not COMMITTED", "Warning");
                    promptWindow.SetButtonAttributes();
                    promptWindow.ShowInTaskbar = false;
                    promptWindow.ShowDialog();
                    if (promptWindow.ShouldTrade)
                    {
                        BtnCommit_Click(sender, EventArgs.Empty);
                        buttonUpdateWasClicked = false;
                        isBulkParameterChanged = false;
                    }
                    else
                    {
                        return;
                    }
                }
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && ComplianceCacheManager.GetPreTradeCheck(_loginUser.CompanyUserID))
                {
                    MessageBox.Show(ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE_COMPLIANCE, ClientLevelConstants.HEADER_MARKET_DATA_ALERT, MessageBoxButtons.OK);
                    return;
                }

                if (TradeClick != null)
                {
                    TradeClick(this, new EventArgs<TradingTicketType>(TradingTicketType.Live));
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

        private BulkOrderSingle GetDataFromBulkUI()
        {
            BulkOrderSingle bulkOrder = new BulkOrderSingle();
            try
            {
                bulkOrder.OrderSide = string.IsNullOrEmpty(cmbOrderSide.Text) ? string.Empty : cmbOrderSide.Text;
                bulkOrder.OrderSideTagValue = string.IsNullOrEmpty(bulkOrder.OrderSide) ? string.Empty : TagDatabaseManager.GetInstance.GetOrderSideTagValueBasedOnId(cmbOrderSide.Value.ToString());
                bulkOrder.Account = string.IsNullOrEmpty(cmbAllocation.Text) ? string.Empty : cmbAllocation.Text;
                bulkOrder.Level1ID = string.IsNullOrEmpty(bulkOrder.Account) ? -1 : Convert.ToInt32(cmbAllocation.Value.ToString());
                bulkOrder.CounterPartyName = string.IsNullOrEmpty(cmbBroker.Text) ? string.Empty : cmbBroker.Text;
                bulkOrder.CounterPartyID = string.IsNullOrEmpty(bulkOrder.CounterPartyName) ? int.MinValue : int.Parse(cmbBroker.Value.ToString());
                bulkOrder.Venue = string.IsNullOrEmpty(cmbvenue.Text) ? string.Empty : cmbvenue.Text;
                bulkOrder.VenueID = string.IsNullOrEmpty(bulkOrder.Venue) ? int.MinValue : int.Parse(cmbvenue.Value.ToString());
                bulkOrder.OrderType = string.IsNullOrEmpty(cmbOrderType.Text) ? string.Empty : cmbOrderType.Text;
                bulkOrder.OrderTypeTagValue = string.IsNullOrEmpty(bulkOrder.OrderType) ? string.Empty : TagDatabaseManager.GetInstance.GetOrderTypeValueBasedOnID(cmbOrderType.Value.ToString());
                bulkOrder.TIF = string.IsNullOrEmpty(cmbTIF.Text) ? string.Empty : cmbTIF.Text;

                if (!string.IsNullOrEmpty(cmbTIF.Text))
                {
                    string tifId = String.IsNullOrEmpty(cmbTIF.Value.ToString()) ? String.Empty : TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(cmbTIF.Value.ToString()); ;
                    if (tifId.Equals(FIXConstants.TIF_GTD))
                    {
                        bulkOrder.ExpireTime = _expireTime;
                    }
                    else
                        bulkOrder.ExpireTime = TradingTicketConstants.CAPTION_NA_SLASH;
                }

                bulkOrder.Strategy = string.IsNullOrEmpty(cmbStrategy.Text) ? string.Empty : cmbStrategy.Text;
                bulkOrder.Level2ID = string.IsNullOrEmpty(bulkOrder.Strategy) ? int.MinValue : int.Parse(cmbStrategy.Value.ToString());
                bulkOrder.LimitPrice = Convert.ToDouble(nmrcLimit.Value);
                bulkOrder.StopPrice = Convert.ToDouble(nmrcStop.Value);

                bulkOrder.CalcBasis = string.IsNullOrEmpty(cmbCommission.Text) ? null : (CalculationBasis?)cmbCommission.Value;
                bulkOrder.CommissionRate = Convert.ToDouble(nmrcCommission.Value);
                bulkOrder.SoftCommissionCalcBasis = string.IsNullOrEmpty(cmbSoft.Text) ? null : (CalculationBasis?)cmbSoft.Value;
                bulkOrder.SoftCommissionRate = Convert.ToDouble(nmrcSoft.Value);
                bulkOrder.TradeAttribute1 = string.IsNullOrEmpty(cmbTradeAttribute1.Text) ? string.Empty : cmbTradeAttribute1.Text;
                bulkOrder.TradeAttribute2 = string.IsNullOrEmpty(cmbTradeAttribute2.Text) ? string.Empty : cmbTradeAttribute2.Text;
                bulkOrder.TradeAttribute3 = string.IsNullOrEmpty(cmbTradeAttribute3.Text) ? string.Empty : cmbTradeAttribute3.Text;
                bulkOrder.TradeAttribute4 = string.IsNullOrEmpty(cmbTradeAttribute4.Text) ? string.Empty : cmbTradeAttribute4.Text;
                bulkOrder.TradeAttribute5 = string.IsNullOrEmpty(cmbTradeAttribute5.Text) ? string.Empty : cmbTradeAttribute5.Text;
                bulkOrder.TradeAttribute6 = string.IsNullOrEmpty(cmbTradeAttribute6.Text) ? string.Empty : cmbTradeAttribute6.Text;
                bulkOrder.TradingAccountName = string.IsNullOrEmpty(cmbTrader.Text) ? string.Empty : cmbTrader.Text;
                bulkOrder.TradingAccountID = string.IsNullOrEmpty(bulkOrder.TradingAccountName) ? int.MinValue : int.Parse(cmbTrader.Value.ToString());
                bulkOrder.HandlingInstruction = string.IsNullOrEmpty(cmbHandling.Text) ? string.Empty : cmbHandling.Value.ToString();
                bulkOrder.ExecutionInstruction = string.IsNullOrEmpty(cmbExecution.Text) ? string.Empty : cmbExecution.Value.ToString();
                if (cmbvenue.Text.ToLower().Equals("algo") && CachedDataManager.GetInstance.IsAlgoBrokerFromID(CachedDataManager.GetInstance.GetCounterPartyID(cmbBroker.Text)))
                {
                    bulkOrder.AlgoStrategyID = string.IsNullOrEmpty(_algoStrategyID) ? int.MinValue.ToString() : _algoStrategyID;
                    bulkOrder.AlgoStrategyName = string.IsNullOrEmpty(_algoStrategyName) ? "None" : _algoStrategyName;
                    bulkOrder.TagValueDictionary = _tagValueDictionary;
                }

                if (_accountqty != null && _accountqty.AllocationOperationPreference != null && _accountqty.AllocationOperationPreference.OperationPreferenceId.ToString().Equals(cmbAllocation.Value))
                {
                    bulkOrder.IsCustom = true;
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
            return bulkOrder;
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            IEnumerable<Control> enumerable = controls as IList<Control> ?? controls.ToList();
            return enumerable.SelectMany(ctrl => GetAll(ctrl, type)).Concat(enumerable).Where(c => c.GetType() == type);
        }

        int errorCount = 0;
        private bool ValidateControlsBeforeBulkUpdate()
        {
            bool isValidated = true;
            errorCount = 0;

            try
            {
                var ultraComobEditors = GetAll(this, typeof(UltraComboEditor));
                errorProvider.Clear();
                foreach (UltraComboEditor ucmbEditor in ultraComobEditors.Cast<UltraComboEditor>().Where(ucmbEditor => !ucmbEditor.Name.Equals("cmbTradeAttribute1") && !ucmbEditor.Name.Equals("cmbTradeAttribute2") && !ucmbEditor.Name.Equals("cmbTradeAttribute3") && !ucmbEditor.Name.Equals("cmbTradeAttribute4") && !ucmbEditor.Name.Equals("cmbTradeAttribute5") && !ucmbEditor.Name.Equals("cmbTradeAttribute6")))
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
                        errorCount++;
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
        /// Handles the Click event of the btnCommit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnCommit_Click(object sender, System.EventArgs e)
        {
            try
            {
                buttonUpdateWasClicked = true;
                if (ValidateControlsBeforeBulkUpdate())
                {
                    SetStatusBarMessage(string.Empty);
                    BulkOrderSingle BulkUIObject = GetDataFromBulkUI();
                    bool IsOrderSideDisabled = false;
                    if (CheckedUltraGridRows.Count > 0)
                    {
                        if (BulkUIObject.Level1ID != -1)
                        {
                            ValueList vl = (ValueList)grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ACCOUNT].ValueList;
                            foreach (ValueListItem value in vl.ValueListItems)
                            {
                                if (value.DisplayText == TradingTicketConstants.LIT_CUSTOM)
                                {
                                    vl.ValueListItems.Remove(value);
                                    break;
                                }
                            }
                            if (BulkUIObject.IsCustom)
                            {
                                vl.ValueListItems.Add(BulkUIObject.Level1ID, TradingTicketConstants.LIT_CUSTOM);
                                grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ACCOUNT].ValueList = vl;
                                BulkAccountQty = _accountqty;
                            }
                            else
                            {
                                BulkAccountQty = null;
                            }
                        }
                        foreach (var row in CheckedUltraGridRows)
                        {
                            OrderSingle rowObject = row.ListObject as OrderSingle;
                            if (rowObject != null)
                            {
                                if (!string.IsNullOrEmpty(BulkUIObject.OrderSideTagValue))
                                {
                                    if (grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_ORDER_SIDE].Activation != Activation.NoEdit)
                                    {
                                        grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_ORDER_SIDE].Value = string.IsNullOrEmpty(BulkUIObject.OrderSide) ? rowObject.OrderSide : BulkUIObject.OrderSide;
                                        rowObject.OrderSideTagValue = BulkUIObject.OrderSideTagValue;
                                    }
                                    else
                                        IsOrderSideDisabled = true;
                                }

                                if (BulkUIObject.Level1ID != -1)
                                {
                                    if (grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_ACCOUNT].Activation != Activation.NoEdit)
                                    {
                                        grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_ACCOUNT].Value = string.IsNullOrEmpty(BulkUIObject.Account) ? rowObject.Account : BulkUIObject.Account;
                                        rowObject.Level1ID = BulkUIObject.Level1ID;
                                        rowObject.OriginalAllocationPreferenceID = BulkUIObject.IsCustom ? BulkUIObject.Level1ID : int.MinValue;
                                    }
                                }
                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Value = string.IsNullOrEmpty(BulkUIObject.CounterPartyName) ? rowObject.CounterPartyName : BulkUIObject.CounterPartyName;
                                rowObject.CounterPartyID = BulkUIObject.CounterPartyID != int.MinValue ? BulkUIObject.CounterPartyID : rowObject.CounterPartyID;

                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_VENUE].Value = string.IsNullOrEmpty(BulkUIObject.Venue) ? rowObject.Venue : BulkUIObject.Venue;
                                rowObject.VenueID = BulkUIObject.VenueID != int.MinValue ? BulkUIObject.VenueID : rowObject.VenueID;

                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_ORDER_TYPE].Value = string.IsNullOrEmpty(BulkUIObject.OrderType) ? rowObject.OrderType : BulkUIObject.OrderType;
                                rowObject.OrderTypeTagValue = string.IsNullOrEmpty(BulkUIObject.OrderTypeTagValue) ? rowObject.OrderTypeTagValue : BulkUIObject.OrderTypeTagValue;

                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value = string.IsNullOrEmpty(BulkUIObject.TIF) ? rowObject.TIF : BulkUIObject.TIF;
                                if (!string.IsNullOrEmpty(BulkUIObject.TIF))
                                {
                                    grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_EXPIRETIME].Value = BulkUIObject.ExpireTime;
                                    if ((BulkUIObject.TIF.Equals("GTC") || BulkUIObject.TIF.Equals("Good Till Date")))
                                    {
                                        ChangeOrderTypeAsPerPreference(row, rowObject, BulkUIObject);
                                    }
                                }

                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_STRATEGY].Value = string.IsNullOrEmpty(BulkUIObject.Strategy) ? rowObject.Strategy : BulkUIObject.Strategy;
                                rowObject.Level2ID = BulkUIObject.Level2ID != int.MinValue ? BulkUIObject.Level2ID : rowObject.Level2ID;

                                grdTrades.Rows[row.Index].Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Value = BulkUIObject.LimitPrice != 0.0 ? BulkUIObject.LimitPrice : rowObject.Price;
                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_STOP_PRICE].Value = BulkUIObject.StopPrice != 0.0 ? BulkUIObject.StopPrice : rowObject.StopPrice;

                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_CALCBASIS].Value = BulkUIObject.CalcBasis != null ? BulkUIObject.CalcBasis : rowObject.CalcBasis;
                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_COMMISSIONRATE].Value = BulkUIObject.CommissionRate != 0.0 ? BulkUIObject.CommissionRate : rowObject.CommissionRate;
                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_SOFTCOMMISSIONCALCBASIS].Value = BulkUIObject.SoftCommissionCalcBasis != null ? BulkUIObject.SoftCommissionCalcBasis : rowObject.SoftCommissionCalcBasis;
                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Value = BulkUIObject.SoftCommissionRate != 0.0 ? BulkUIObject.SoftCommissionRate : rowObject.SoftCommissionRate;

                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_TRADEATTRIBUTE1].Value = string.IsNullOrEmpty(BulkUIObject.TradeAttribute1) ? rowObject.TradeAttribute1 : BulkUIObject.TradeAttribute1;
                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_TRADEATTRIBUTE2].Value = string.IsNullOrEmpty(BulkUIObject.TradeAttribute2) ? rowObject.TradeAttribute2 : BulkUIObject.TradeAttribute2;
                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_TRADEATTRIBUTE3].Value = string.IsNullOrEmpty(BulkUIObject.TradeAttribute3) ? rowObject.TradeAttribute3 : BulkUIObject.TradeAttribute3;
                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_TRADEATTRIBUTE4].Value = string.IsNullOrEmpty(BulkUIObject.TradeAttribute4) ? rowObject.TradeAttribute4 : BulkUIObject.TradeAttribute4;
                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_TRADEATTRIBUTE5].Value = string.IsNullOrEmpty(BulkUIObject.TradeAttribute5) ? rowObject.TradeAttribute5 : BulkUIObject.TradeAttribute5;
                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_TRADEATTRIBUTE6].Value = string.IsNullOrEmpty(BulkUIObject.TradeAttribute6) ? rowObject.TradeAttribute6 : BulkUIObject.TradeAttribute6;

                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_TRADING_ACCOUNT].Value = string.IsNullOrEmpty(BulkUIObject.TradingAccountName) ? rowObject.TradingAccountName : BulkUIObject.TradingAccountName;
                                rowObject.TradingAccountID = BulkUIObject.TradingAccountID != int.MinValue ? BulkUIObject.TradingAccountID : rowObject.TradingAccountID;
                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_HANDLING_INST_TagValue].Value = string.IsNullOrEmpty(BulkUIObject.HandlingInstruction) ? rowObject.HandlingInstruction : BulkUIObject.HandlingInstruction;
                                grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_EXECUTION_INST_TagValue].Value = string.IsNullOrEmpty(BulkUIObject.ExecutionInstruction) ? rowObject.ExecutionInstruction : BulkUIObject.ExecutionInstruction;

                                if (BulkUIObject.Venue.ToLower().Equals("algo") && CachedDataManager.GetInstance.IsAlgoBrokerFromID(CachedDataManager.GetInstance.GetCounterPartyID(BulkUIObject.CounterPartyName)))
                                {
                                    grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_ALGOSTRATEGYNAME].Value = BulkUIObject.AlgoStrategyName;
                                    rowObject.AlgoStrategyID = BulkUIObject.AlgoStrategyID;
                                    rowObject.AlgoProperties.TagValueDictionary = BulkUIObject.TagValueDictionary;
                                }

                                if (BulkUIObject.CounterPartyID != int.MinValue)
                                {
                                    rowObject.IsUseCustodianBroker = false;
                                }
                            }
                            row.Refresh();
                        }
                        grdTrades.UpdateData();
                        BtnClear_Click(null, null);
                        SetStatusBarMessage("Fields updated successfully for the selected orders");
                        if (IsOrderSideDisabled)
                        {
                            MessageBox.Show(TradingTicketConstants.MSG_BULK_UPDATE, "Bulk Update Alert", MessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        SetStatusBarMessage("Please check any row to update");
                    }
                }
                else
                {
                    SetStatusBarMessage("Could not process bulk update request due to " + errorCount.ToString() + " error(s).");
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

        private void ChangeOrderTypeAsPerPreference(UltraGridRow row, OrderSingle order, BulkOrderSingle BulkUIObject)
        {
            try
            {
                if (BulkUIObject == null || string.IsNullOrEmpty(BulkUIObject.OrderTypeTagValue))
                {
                    TranferTradeRules transferTradeRules = CachedDataManager.GetInstance.GetTransferTradeRules();
                    if (transferTradeRules != null && transferTradeRules.IsDefaultOrderTypeLimitForMultiDay && !order.OrderTypeTagValue.Equals(FIXConstants.ORDTYPE_Limit))
                    {
                        grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_ORDER_TYPE].Value = FIXConstants.ORDTYPE_Limit;
                    }
                }
                else
                {
                    grdTrades.Rows[row.Index].Cells[OrderFields.PROPERTY_ORDER_TYPE].Value = BulkUIObject.OrderType;
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
        /// Handles the Click event of the btnClear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnClear_Click(object sender, System.EventArgs e)
        {
            isBulkParameterChanged = false;
            var ultraComobEditors = GetAll(this, typeof(UltraComboEditor));
            foreach (UltraComboEditor ucmbEditor in ultraComobEditors.Cast<UltraComboEditor>())
            {
                ucmbEditor.Value = null;
                ucmbEditor.Enabled = true;
            }
            if (!(_tradingTicketParent == TradingTicketParent.WatchList || _tradingTicketParent == TradingTicketParent.OptionChain || IsEditOrders))
            {
                cmbOrderSide.Enabled = false;
            }
            var numericUpDownEx = GetAll(this, typeof(PranaNumericUpDown));
            foreach (PranaNumericUpDown numericUpDownExEditor in numericUpDownEx)
            {
                numericUpDownExEditor.Value = 0;
            }
            nmrcLimit.Enabled = false;
            nmrcStop.Enabled = false;
            _algoStrategyName = string.Empty;
            _algoStrategyID = string.Empty;
            _tagValueDictionary = new Dictionary<string, string>();
            btnAlgo.Visible = false;
            btnAlgo.Text = "NONE";
            SetStatusBarMessage(string.Empty);
            btnExpireTime.Visible = false;
            btnExpireTime.Text = TradingTicketConstants.CAPTION_NA_SLASH;
            _expireTime = TradingTicketConstants.CAPTION_NA_SLASH;
            if (_accountqty != null && _accountqty.AllocationOperationPreference != null)
            {
                DataTable dt = cmbAllocation.DataSource as DataTable;
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.ItemArray.Contains(_accountqty.AllocationOperationPreference.OperationPreferenceId.ToString()))
                        {
                            row.Delete();
                            break;
                        }
                    }
                }
                _accountqty = null;
            }
        }

        /// <summary>
        /// Don't allow edit column.
        /// </summary>
        /// <param name="grd">The GRD.</param>
        /// <param name="columnName">Name of the column.</param>
        private void DisAllowEditColumn(UltraGrid grd, string columnName)
        {
            try
            {
                if (grd.DisplayLayout.Bands[0].Columns.Exists(columnName))
                {
                    grd.DisplayLayout.Bands[0].Columns[columnName].CellActivation = Activation.ActivateOnly;
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
        bool _isGridEdited = false;
        int DropdownIndex = int.MinValue;
        /// <summary>
        /// Handles the AfterCellUpdate event of the grdTrades control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void grdTrades_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                _isGridEdited = true;
                string ColumnError = string.Empty;
                string ColumnExpireTimeError = string.Empty;
                string ColumnExpireTimeDateValidationError = string.Empty;
                string ColumnBrokerValidationError = string.Empty;
                string ColumnKey = e.Cell.Column.Key;
                string ColumnExpireTimeKey = string.Empty;
                int output;
                OrderSingle orderSingleCurrentRow = e.Cell.Row.ListObject as OrderSingle;

                switch (e.Cell.Column.Key)
                {
                    case TradingTicketConstants.COLUMN_LIMITPRICE:
                        grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_AVGPRICE].Value = grdTrades.Rows[e.Cell.Row.Index].Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Value;
                        orderSingleCurrentRow.Price = Convert.ToDouble(grdTrades.Rows[e.Cell.Row.Index].Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Value);
                        break;
                    case OrderFields.PROPERTY_STRATEGY:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_STRATEGY + "!";
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(e.Cell.ValueListResolved, e.Cell.Text) || string.IsNullOrEmpty(e.Cell.Text))
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_STRATEGY, null);
                        else
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_STRATEGY, ColumnError);

                        break;
                    case OrderFields.PROPERTY_VENUE:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_VENUE + "!";
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(e.Cell.ValueListResolved, e.Cell.Text))
                        {
                            orderSingleCurrentRow.VenueID = Convert.ToInt32(e.Cell.ValueListResolved.GetValue(e.Cell.Text, ref DropdownIndex));
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_VENUE, null);
                        }
                        else
                        {
                            orderSingleCurrentRow.VenueID = 0;
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_VENUE, ColumnError);
                        }
                        orderSingleCurrentRow.Venue = grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_VENUE].Text;
                        UpdateAlgoType(grdTrades.Rows[e.Cell.Row.Index]);
                        break;

                    case OrderFields.PROPERTY_LEVEL1ID:
                        Int32.TryParse(e.Cell.Value.ToString(), out output);
                        orderSingleCurrentRow.Level1ID = output;
                        break;

                    case OrderFields.PROPERTY_ACCOUNT:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_ALLOCATION + "!";
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(e.Cell.ValueListResolved, e.Cell.Text))
                        {
                            orderSingleCurrentRow.Level1ID = Convert.ToInt32(e.Cell.ValueListResolved.GetValue(e.Cell.Text, ref DropdownIndex));
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_ACCOUNT, null);
                            if (CachedDataManager.GetInstance.IsShowMasterFundonTT() && UpdateBrokerOnAccountChange != null)
                            {
                                e.Cell.Row.Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Value = null;
                                UpdateBrokerOnAccountChange(this, new EventArgs<int>(e.Cell.Row.Index));
                            }
                            if (!IsMTTSettingUp && orderSingleCurrentRow.IsUseCustodianBroker)
                            {
                                ResetErrorInBrokerColumn(e.Cell.Row.Index);
                                Dictionary<int, int> accountBrokerMapping = GetAccountBrokerMapping(orderSingleCurrentRow.Level1ID, orderSingleCurrentRow, e.Cell.Row.Index);
                                bool isSetCustodianBroker = false;
                                if (accountBrokerMapping != null)
                                {
                                    int unmappedAccounts = accountBrokerMapping.Count(kvp => CachedDataManager.GetInstance.GetCounterPartyByAccountId(kvp.Key) == int.MinValue);
                                    isSetCustodianBroker = unmappedAccounts != accountBrokerMapping.Count;
                                }
                                if (isSetCustodianBroker)
                                {
                                    SetUnmappedBrokerError(e.Cell.Row.Index, accountBrokerMapping);
                                    orderSingleCurrentRow.AccountBrokerMapping = JsonHelper.SerializeObject(accountBrokerMapping);
                                    grdTrades.Rows[e.Cell.Row.Index].Cells[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Activation = Activation.AllowEdit;
                                    grdTrades.Rows[e.Cell.Row.Index].Cells[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Value = TradingTicketConstants.CAPTION_ACCOUNTBROKER;
                                }
                                else
                                {
                                    e.Cell.Row.Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Value = null;
                                    orderSingleCurrentRow.IsUseCustodianBroker = false;
                                    orderSingleCurrentRow.AccountBrokerMapping = String.Empty;
                                }
                            }
                        }
                        else
                        {
                            if (!IsMTTSettingUp && orderSingleCurrentRow.IsUseCustodianBroker)
                            {
                                ResetErrorInBrokerColumn(e.Cell.Row.Index);
                                grdTrades.Rows[e.Cell.Row.Index].Cells[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Activation = Activation.Disabled;
                                grdTrades.Rows[e.Cell.Row.Index].Cells[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Value = TradingTicketConstants.CAPTION_NA_SLASH;
                            }
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_ACCOUNT, ColumnError);
                        }
                        orderSingleCurrentRow.Account = grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_ACCOUNT].Text;
                        break;

                    case OrderFields.PROPERTY_ORDER_SIDE:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_ORDER_SIDE + "!";
                        orderSingleCurrentRow.OrderSide = grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_ORDER_SIDE].Text;
                        orderSingleCurrentRow.OrderSideTagValue = TagDatabaseManager.GetInstance.GetOrderSideValue(grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_ORDER_SIDE].Text);
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(e.Cell.ValueListResolved, e.Cell.Text))
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_ORDER_SIDE, null);
                        else
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_ORDER_SIDE, ColumnError);

                        break;

                    case OrderFields.PROPERTY_ORDER_TYPE:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_ORDER_TYPE + "!";
                        orderSingleCurrentRow.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValue(grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_ORDER_TYPE].Text);
                        ChangeOrderTypeBasedFields(e.Cell.Row, orderSingleCurrentRow);
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(e.Cell.ValueListResolved, e.Cell.Text))
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_ORDER_TYPE, null);
                        else
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_ORDER_TYPE, ColumnError);

                        break;

                    case OrderFields.PROPERTY_COUNTERPARTY_NAME:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_BROKER + "!";
                        if (!IsMTTSettingUp && orderSingleCurrentRow.IsUseCustodianBroker)
                        {
                            ResetErrorInBrokerColumn(e.Cell.Row.Index);
                            orderSingleCurrentRow.IsUseCustodianBroker = false;
                            orderSingleCurrentRow.AccountBrokerMapping = String.Empty;
                            grdTrades.Rows[e.Cell.Row.Index].Cells[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Activation = Activation.Disabled;
                            grdTrades.Rows[e.Cell.Row.Index].Cells[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Value = TradingTicketConstants.CAPTION_NA_SLASH;
                        }
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(e.Cell.ValueListResolved, e.Cell.Text))
                        {
                            orderSingleCurrentRow.CounterPartyID = Convert.ToInt32(e.Cell.ValueListResolved.GetValue(e.Cell.Text, ref DropdownIndex));
                            orderSingleCurrentRow.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(orderSingleCurrentRow.CounterPartyID);
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_COUNTERPARTY_NAME, null);
                            if (UpdateVenueOnBrokerChange != null)
                                UpdateVenueOnBrokerChange(this, new EventArgs<int>(e.Cell.Row.Index));
                            if (TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(orderSingleCurrentRow.CounterPartyID) == BusinessObjects.PranaInternalConstants.ConnectionStatus.CONNECTED)
                                grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Appearance.ForeColor = Color.Green;
                            else
                                grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Appearance.ForeColor = Color.Red;
                        }
                        else
                        {
                            if (IsMTTSettingUp && orderSingleCurrentRow.IsUseCustodianBroker && e.Cell.Text == "Default Broker(s)")
                            {
                                grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Appearance.ForeColor = Color.Black;
                                grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_COUNTERPARTY_NAME, null);
                                break;
                            }
                            orderSingleCurrentRow.CounterPartyID = int.MinValue;
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_COUNTERPARTY_NAME, ColumnError);
                        }
                        break;

                    case OrderFields.PROPERTY_TRADING_ACCOUNT:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_TRADER + "!";
                        orderSingleCurrentRow.TradingAccountName = grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TRADING_ACCOUNT].Text;
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(e.Cell.ValueListResolved, e.Cell.Text))
                        {
                            orderSingleCurrentRow.TradingAccountID = Convert.ToInt32(e.Cell.ValueListResolved.GetValue(e.Cell.Text, ref DropdownIndex));
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_TRADING_ACCOUNT, null);
                        }
                        else
                        {
                            orderSingleCurrentRow.TradingAccountID = int.MinValue;
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_TRADING_ACCOUNT, ColumnError);
                        }
                        break;

                    case TradingTicketConstants.COLUMN_SETTLEMENT_CURRENCYNAME:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_SETTLEMENT_CURRENCY + "!";
                        if (e.Cell.ValueListResolved != null && ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(e.Cell.ValueListResolved, e.Cell.Text.ToUpper()))
                        {
                            orderSingleCurrentRow.SettlementCurrencyID = e.Cell.ValueListResolved != null ? Convert.ToInt32(e.Cell.ValueListResolved.GetValue(e.Cell.Text.ToUpper(), ref DropdownIndex)) : CachedDataManager.GetInstance.GetCurrencyID(e.Cell.Text.ToUpper());
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(TradingTicketConstants.COLUMN_SETTLEMENT_CURRENCYNAME, null);
                        }
                        else
                        {
                            orderSingleCurrentRow.SettlementCurrencyID = 0;
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(TradingTicketConstants.COLUMN_SETTLEMENT_CURRENCYNAME, ColumnError);
                        }
                        break;

                    case OrderFields.PROPERTY_TIF_TAGVALUE:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_TIF + "!";
                        orderSingleCurrentRow.TIF = grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Text;
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(e.Cell.ValueListResolved, e.Cell.Text))
                        {
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_TIF_TAGVALUE, null);
                            ColumnExpireTimeKey = grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXPIRETIME].Column.Key;
                            ColumnExpireTimeError = "Invalid " + OrderFields.CAPTION_EXPIRETIME + "!";
                            ColumnExpireTimeDateValidationError = "The expiry date selected is already past date. Please select a valid Expiry Date.";
                            UpdateExpiryDate(grdTrades.Rows[e.Cell.Row.Index]);
                            if (!IsEditOrders)
                            {
                                if (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value != null && (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.Equals("Good Till Date") || (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.Equals(FIXConstants.TIF_GTD))))
                                {
                                    string expireTimeText = grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXPIRETIME].Text;
                                    DateTime dt;
                                    if (expireTimeText.Equals(TradingTicketConstants.CAPTION_NA_SLASH) || string.IsNullOrEmpty(expireTimeText))
                                    {
                                        grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_EXPIRETIME, ColumnExpireTimeError);
                                    }
                                    else if (DateTime.TryParse(expireTimeText, out dt) && dt.Date < DateTime.Now.Date)
                                    {
                                        grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_EXPIRETIME, ColumnExpireTimeDateValidationError);
                                    }
                                    else
                                    {
                                        grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_EXPIRETIME, null);
                                    }
                                }
                                else if (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value != null)
                                {
                                    grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXPIRETIME].Value = TradingTicketConstants.CAPTION_NA_SLASH;
                                    grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_EXPIRETIME, null);
                                }
                            }
                            if (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value != null && (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.Equals("Good Till Date") || grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.Equals("GTC")))
                            {
                                ChangeOrderTypeAsPerPreference(grdTrades.Rows[e.Cell.Row.Index], orderSingleCurrentRow, null);
                            }
                        }
                        else
                        {
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_TIF_TAGVALUE, ColumnError);
                            if (!IsEditOrders)
                            {
                                grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_EXPIRETIME, null);
                                grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXPIRETIME].Value = TradingTicketConstants.CAPTION_NA_SLASH;
                            }
                        }
                        break;

                    case OrderFields.PROPERTY_EXECUTED_QTY:
                        double executedQty = 0, qty = 0;
                        Double.TryParse(grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXECUTED_QTY].Text.Trim(), out executedQty);
                        Double.TryParse(grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_QUANTITY].Text.Trim(), out qty);
                        if (executedQty > qty)
                        {
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXECUTED_QTY].Value = qty;
                            MessageBox.Show(TradingTicketConstants.MSG_GREATHER_EXQTY_QUANTITY, TradingTicketConstants.LIT_ERROR);
                        }
                        break;

                    case OrderFields.PROPERTY_QUANTITY:
                        double Qty = 0, MTTLeavesQty = 0;
                        Double.TryParse(grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_QUANTITY].Text.Trim(), out Qty);
                        if (_tradingTicketParent == TradingTicketParent.WatchList || _tradingTicketParent == TradingTicketParent.OptionChain)
                        {
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_QUANTITY_MTT].Value = Qty;
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXECUTED_QTY].Value = Qty;
                            break;
                        }
                        Double.TryParse(grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_QUANTITY_MTT].Text.Trim(), out MTTLeavesQty);
                        if (IsEditOrders)
                        {
                            //If Qty set is less than original qty - UnsentQty, then update the Qty to MTTLeavesQty and show error
                            if (Qty < MTTLeavesQty - orderSingleCurrentRow.UnsentQty)
                            {
                                grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_QUANTITY].Value = MTTLeavesQty;
                                grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXECUTED_QTY].Value = MTTLeavesQty;
                                MessageBox.Show(TradingTicketConstants.MSG_GREATHER_INMARKET_QUANTITY, TradingTicketConstants.LIT_ERROR);
                            }
                            else
                            {
                                grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXECUTED_QTY].Value = Qty;
                            }
                        }
                        //If MTTLeaveQty is less than Qty set, then update the Qty to MTTLeavesQty and show error
                        else if (MTTLeavesQty < Qty)
                        {
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_QUANTITY].Value = MTTLeavesQty;
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXECUTED_QTY].Value = MTTLeavesQty;
                            MessageBox.Show(TradingTicketConstants.MSG_GREATER_QUANTITY, TradingTicketConstants.LIT_ERROR);
                        }
                        else
                        {
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXECUTED_QTY].Value = Qty;
                        }
                        break;
                    case OrderFields.PROPERTY_SOFTCOMMISSIONCALCBASIS:
                        if (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_SOFTCOMMISSIONCALCBASIS].Text == TradingTicketConstants.C_LIT_AUTO)
                        {
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Value = 0.0;
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Activation = Activation.NoEdit;
                        }
                        else
                        {
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Value = 0.0;
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Value = Convert.ToDouble(grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Value);
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Activation = Activation.AllowEdit;
                        }
                        break;
                    case OrderFields.PROPERTY_CALCBASIS:
                        if (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_CALCBASIS].Text == TradingTicketConstants.C_LIT_AUTO)
                        {
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_COMMISSIONRATE].Value = 0.0;
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_COMMISSIONRATE].Activation = Activation.NoEdit;
                        }
                        else
                        {
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_COMMISSIONRATE].Value = 0.0;
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_COMMISSIONRATE].Value = Convert.ToDouble(grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_COMMISSIONRATE].Value);
                            grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_COMMISSIONRATE].Activation = Activation.AllowEdit;
                        }
                        break;
                    case OrderFields.PROPERTY_COMMISSIONRATE:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_COMMISSION_RATE + "!";
                        if (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_CALCBASIS].Text == TradingTicketConstants.C_LIT_AUTO && Convert.ToDouble(grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_COMMISSIONRATE].Value) != 0.0)
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_COMMISSIONRATE, ColumnError);
                        else
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_COMMISSIONRATE, null);
                        break;
                    case OrderFields.PROPERTY_SOFTCOMMISSIONRATE:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_SOFT_RATE + "!";
                        if (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_SOFTCOMMISSIONCALCBASIS].Text == TradingTicketConstants.C_LIT_AUTO && Convert.ToDouble(grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Value) != 0.0)
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_SOFTCOMMISSIONRATE, ColumnError);
                        else
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_SOFTCOMMISSIONRATE, null);
                        break;
                    case OrderFields.PROPERTY_EXECUTION_INST_TagValue:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_EXECUTION_INSTRUCTIONS + "!";
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(e.Cell.ValueListResolved, e.Cell.Text))
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_EXECUTION_INST_TagValue, null);
                        else
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_EXECUTION_INST_TagValue, ColumnError);
                        break;
                    case OrderFields.PROPERTY_HANDLING_INST_TagValue:
                        ColumnError = "Invalid " + TradingTicketConstants.CAPTION_HANDLING_INSTRUCTIONS + "!";
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(e.Cell.ValueListResolved, e.Cell.Text))
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_HANDLING_INST_TagValue, null);
                        else
                            grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_HANDLING_INST_TagValue, ColumnError);

                        break;
                    case OrderFields.PROPERTY_EXPIRETIME:
                        grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXPIRETIME].Activation = Activation.NoEdit;
                        if (!IsEditOrders)
                        {
                            if (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value != null && grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value != null && (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.Equals("Good Till Date")
                             || grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.Equals(FIXConstants.TIF_GTD)))
                            {
                                ColumnError = "Invalid " + OrderFields.CAPTION_EXPIRETIME + "!";
                                string expireTime = grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXPIRETIME].Text;
                                if (expireTime.Equals(TradingTicketConstants.CAPTION_NA_SLASH) || string.IsNullOrEmpty(expireTime))
                                {
                                    grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_EXPIRETIME, ColumnError);
                                }
                                else { grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_EXPIRETIME, null); }
                            }
                            else if (grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value != null)
                            {
                                grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_EXPIRETIME].Value = TradingTicketConstants.CAPTION_NA_SLASH;
                                grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_EXPIRETIME, null);
                            }
                        }
                        break;
                }
                if (e.Cell.Row.Index != -1)
                {
                    SetGridError(e.Cell.Row.Index, ColumnError, ColumnKey);
                    if (ColumnKey.Equals("TIF") && !string.IsNullOrEmpty(ColumnExpireTimeKey))
                    {
                        SetGridError(e.Cell.Row.Index, ColumnExpireTimeError, ColumnExpireTimeKey);
                        SetGridError(e.Cell.Row.Index, ColumnExpireTimeDateValidationError, ColumnExpireTimeKey);
                    }
                    if (ColumnKey.Equals(OrderFields.PROPERTY_ACCOUNT))
                    {
                        SetGridError(e.Cell.Row.Index, ColumnBrokerValidationError, grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Column.Key);
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
        /// Set Error In Broker Column when default broker is not defined for unmapped accounts
        /// </summary>
        public void SetUnmappedBrokerError(int index, Dictionary<int, int> accountBrokerMapping)
        {
            try
            {
                int unmappedAccounts = accountBrokerMapping.Count(pair => pair.Value == int.MinValue);
                string ColumnBrokerValidationError = "Default Brokers are not mapped for " + unmappedAccounts + " account(s)";
                if (unmappedAccounts > 0)
                {
                    grdTrades.Rows[index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_COUNTERPARTY_NAME, ColumnBrokerValidationError);
                }
                if (IsMTTSettingUp)
                {
                    SetGridError(index, ColumnBrokerValidationError, grdTrades.Rows[index].Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Column.Key);
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
        /// Reset Error In Broker Column
        /// </summary>
        private void ResetErrorInBrokerColumn(int index)
        {
            try
            {
                var columnKey = grdTrades.Rows[index].Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Column.Key;
                var errorValue = grdTrades.Rows[index].DataErrorInfo.GetColumnError(columnKey);
                grdTrades.Rows[index].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_COUNTERPARTY_NAME, null);
                SetGridError(index, errorValue, columnKey);
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
        /// Get Account Broker Mapping for Allocation
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetAccountBrokerMapping(int prefId, OrderSingle order, int index)
        {
            Dictionary<int, int> accountBrokerMapping = null;
            try
            {
                bool isValidPref = false;
                AllocationOperationPreference operationPreference = null;
                if (!String.IsNullOrEmpty(CachedDataManager.GetInstance.GetAccount(prefId)))
                {
                    isValidPref = true;
                }
                else
                {
                    operationPreference = _mttPresenter.AllocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, prefId);
                    if (TradeManager.TradeManager.GetInstance().IsAllocationPrefValidForCustodianBroker(order, operationPreference))
                    {
                        isValidPref = true;
                    }
                }
                if (isValidPref)
                {
                    ValueList cpList = GetCounterParties(index);
                    accountBrokerMapping = TradeManager.TradeManager.GetInstance().GetAccountBrokerMappingForSelectedFund(order.Level1ID, cpList, operationPreference, order);
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
            return accountBrokerMapping;
        }

        void SetGridError(int Index, string ColumnError, string ColumnKey)
        {
            try
            {
                var ErrorValue = grdTrades.Rows[Index].DataErrorInfo.GetColumnError(ColumnKey);
                if ((ErrorValue == null || !ErrorValue.Equals(string.Empty)))
                {
                    if (ErrorValue != null)
                    {
                        if (!grdTrades.Rows[Index].DataErrorInfo.RowError.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Contains(ErrorValue))
                        {
                            grdTrades.Rows[Index].DataErrorInfo.RowError = grdTrades.Rows[Index].DataErrorInfo.RowError + ErrorValue + Environment.NewLine;
                        }
                    }
                    else
                    {
                        string[] Errors = grdTrades.Rows[Index].DataErrorInfo.RowError.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        if (grdTrades.Rows[Index].DataErrorInfo.RowError.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Contains(ColumnError))
                        {
                            string NewError = string.Empty;
                            foreach (var error in Errors)
                            {
                                if (!error.Equals(ColumnError))
                                {
                                    NewError = NewError + error + Environment.NewLine;
                                }
                            }
                            if (NewError.Equals(String.Empty))
                                grdTrades.Rows[Index].DataErrorInfo.ClearErrors();
                            else
                                grdTrades.Rows[Index].DataErrorInfo.RowError = NewError;
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

        public void UpgradeGridAfterVenueChange(int index, int VenueID)
        {
            try
            {
                grdTrades.Rows[index].Cells[OrderFields.PROPERTY_VENUE].Value = VenueID;
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
        private void ChangeVenueInFirstRowBasedOnBroker(CellEventArgs e)
        {
            try
            {
                //if (e.Cell.Column.Key == OrderFields.PROPERTY_COUNTERPARTY_NAME && e.Cell.Row.Index == 0)
                //{
                //    grdTrades.Rows[0].Cells[OrderFields.PROPERTY_VENUE].Activation = Activation.AllowEdit;
                if (UpdateVenueForFirstRowOnBrokerChange != null)
                    UpdateVenueForFirstRowOnBrokerChange(this, new EventArgs<int>(e.Cell.Row.Index));
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
        }

        /// <summary>
        /// Handles the BeforeCellListDropDown event of the grdTrades control.
        /// used to set the drop down width equal to the column width
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelableCellEventArgs"/> instance containing the event data.</param>
        private void grdTrades_BeforeCellListDropDown(object sender, CancelableCellEventArgs e)
        {
            try
            {
                ValueList valueList = e.Cell.ValueListResolved as ValueList;
                if (valueList != null && e.Cell.Column.Key.Equals(OrderFields.PROPERTY_COUNTERPARTY_NAME))
                {
                    for (int k = 0; k < valueList.ValueListItems.Count; k++)
                    {
                        if (TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(CachedDataManager.GetInstance.GetCounterPartyID(valueList.ValueListItems[k].DisplayText)) == PranaInternalConstants.ConnectionStatus.CONNECTED)
                            valueList.ValueListItems[k].Appearance.ForeColor = Color.Green;
                        else
                            valueList.ValueListItems[k].Appearance.ForeColor = Color.Red;
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
        /// Handles the BeforeColumnChooserDisplayed event of the grdTrades control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BeforeColumnChooserDisplayedEventArgs"/> instance containing the event data.</param>
        private void grdTrades_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (FindForm()).AddCustomColumnChooser(grdTrades);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adjust the UI for expanded/collapsed state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grpBoxCommisionAttribute_expandedStateChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (grpBoxCommisionAttribute.Expanded == false)
                {
                    grpBoxBulkUpdate.Height = 136;
                    grpBoxCommisionAttribute.Height = 44;
                    grdTrades.Location = new System.Drawing.Point(0, 148);
                    grdTrades.Height = this.FindForm().WindowState == FormWindowState.Maximized ? 500 : 156;
                }
                else
                {
                    grpBoxBulkUpdate.Height = 207;
                    grpBoxCommisionAttribute.Height = 122;
                    grdTrades.Location = new System.Drawing.Point(0, 216);
                    grdTrades.Height = this.FindForm().WindowState == FormWindowState.Maximized ? 432 : 88;
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
        /// Handles the InitializeLayout event of the grdTrades control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdTrades_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_COMMISSIONRATE].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_FXRATE].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_QUANTITY].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTED_QTY].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_FXRATE].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_STOP_PRICE].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_AVGPRICE].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                e.Layout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.None;
                e.Layout.UseFixedHeaders = true;
                e.Layout.Bands[0].Columns[0].Header.Fixed = true;

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
        /// Loads the grid layout.
        /// </summary>
        private void LoadGridLayout()
        {
            try
            {
                if (_loginUser != null)
                {
                    string startPath = Application.StartupPath;
                    string multiTradePreferencesPath = startPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID;
                    string multiTradePrefFile = multiTradePreferencesPath + "\\" + TradingTicketConstants.MULTI_TRADING_TICKET_FILE;
                    if (File.Exists(multiTradePrefFile))
                    {
                        grdTrades.DisplayLayout.LoadFromXml(multiTradePrefFile, PropertyCategories.All);
                    }
                }
            }
            catch (Exception e)
            {
                Exception ex = new Exception(TradingTicketConstants.MSG_LAYOUT_LOAD_ERROR, e);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the FormClosed event of the MultiTradingTicket control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        private void MultiTradingTicket_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                InstanceManager.ReleaseInstance(typeof(MultiTradingTicket));
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

        /// <summary>
        /// Changes the order type based fields.
        /// </summary>
        /// <param name="ulRow">The ul row.</param>
        /// <param name="OrderSingleRow">The order single row.</param>
        private void ChangeOrderTypeBasedFields(UltraGridRow ulRow, OrderSingle OrderSingleRow)
        {
            try
            {
                switch (OrderSingleRow.OrderTypeTagValue)
                {
                    case FIXConstants.ORDTYPE_Limit:
                        ulRow.Cells[Global.OrderFields.PROPERTY_STOP_PRICE].Activation = Activation.NoEdit;
                        ulRow.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Activation = Activation.AllowEdit;
                        break;

                    case FIXConstants.ORDTYPE_Market:
                        ulRow.Cells[Global.OrderFields.PROPERTY_STOP_PRICE].Activation = Activation.NoEdit;
                        ulRow.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Activation = Activation.NoEdit;
                        break;

                    case FIXConstants.ORDTYPE_Pegged:
                        ulRow.Cells[Global.OrderFields.PROPERTY_STOP_PRICE].Activation = Activation.NoEdit;
                        ulRow.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Activation = Activation.NoEdit;
                        break;

                    case FIXConstants.ORDTYPE_Stop:
                        ulRow.Cells[Global.OrderFields.PROPERTY_STOP_PRICE].Activation = Activation.AllowEdit;
                        ulRow.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Activation = Activation.NoEdit;
                        break;

                    case FIXConstants.ORDTYPE_Stoplimit:
                        ulRow.Cells[Global.OrderFields.PROPERTY_STOP_PRICE].Activation = Activation.AllowEdit;
                        ulRow.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Activation = Activation.AllowEdit;
                        break;

                    case FIXConstants.ORDTYPE_MarketOnClose:
                        ulRow.Cells[Global.OrderFields.PROPERTY_STOP_PRICE].Activation = Activation.NoEdit;
                        ulRow.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Activation = Activation.NoEdit;
                        break;

                    default:
                        ulRow.Cells[Global.OrderFields.PROPERTY_STOP_PRICE].Activation = Activation.NoEdit;
                        ulRow.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Activation = Activation.NoEdit;
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
        /// Handles the Shown event of the MultiTradingTicket control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MultiTradingTicket_Shown(object sender, EventArgs e)
        {
            try
            {
                SetStatusBarMessage("Loading Data...");
                IsMTTSettingUp = true;
                EnableOrDisableMTT(false);
                SetUpMultiTradingTicketUI();
                InstanceManager.RegisterInstance(this);
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
        /// Handles the Click event of the saveLayout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void saveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                string startPath = Application.StartupPath;
                string multiTradePreferencesPath = startPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID;
                if (!Directory.Exists(multiTradePreferencesPath))
                {
                    Directory.CreateDirectory(multiTradePreferencesPath);
                }

                string multiTradePrefFile = multiTradePreferencesPath + "\\" + TradingTicketConstants.MULTI_TRADING_TICKET_FILE;
                grdTrades.DisplayLayout.SaveAsXml(multiTradePrefFile, PropertyCategories.All);
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
        /// Sets the color of the buttons.
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnAlgo.ButtonStyle = UIElementButtonStyle.Button3D;
                btnAlgo.BackColor = Color.FromArgb(55, 67, 85);
                btnAlgo.ForeColor = Color.White;
                btnAlgo.UseAppStyling = false;
                btnAlgo.UseOsThemes = DefaultableBoolean.False;

                btnExpireTime.ButtonStyle = UIElementButtonStyle.Button3D;
                btnExpireTime.BackColor = Color.FromArgb(55, 67, 85);
                btnExpireTime.ForeColor = Color.White;
                btnExpireTime.UseAppStyling = false;
                btnExpireTime.UseOsThemes = DefaultableBoolean.False;

                btnStage.BackColor = Color.FromArgb(55, 67, 85);
                btnStage.ForeColor = Color.White;
                btnStage.Font = new Font(TradingTicketConstants.LIT_FONT_NAME, 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                btnStage.ButtonStyle = UIElementButtonStyle.Button3D;
                btnStage.UseAppStyling = false;
                btnStage.UseOsThemes = DefaultableBoolean.False;

                btnReplace.BackColor = Color.FromArgb(140, 5, 5);
                btnReplace.ForeColor = Color.White;
                btnReplace.Font = new Font(TradingTicketConstants.LIT_FONT_NAME, 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                btnReplace.ButtonStyle = UIElementButtonStyle.Button3D;
                btnReplace.UseAppStyling = false;
                btnReplace.UseOsThemes = DefaultableBoolean.False;

                btnCancel.BackColor = Color.FromArgb(55, 67, 85);
                btnCancel.ForeColor = Color.White;
                btnCancel.Font = new Font(TradingTicketConstants.LIT_FONT_NAME, 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                btnCancel.ButtonStyle = UIElementButtonStyle.Button3D;
                btnCancel.UseAppStyling = false;
                btnCancel.UseOsThemes = DefaultableBoolean.False;

                btnSend.BackColor = Color.FromArgb(104, 156, 46);
                btnSend.ForeColor = Color.White;
                btnSend.Font = new Font(TradingTicketConstants.LIT_FONT_NAME, 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                btnSend.ButtonStyle = UIElementButtonStyle.Button3D;
                btnSend.UseAppStyling = false;
                btnSend.UseOsThemes = DefaultableBoolean.False;

                btnRefreshPrice.BackColor = Color.FromArgb(55, 67, 85);
                btnRefreshPrice.ForeColor = Color.White;
                btnRefreshPrice.Font = new Font(TradingTicketConstants.LIT_FONT_NAME, 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                btnRefreshPrice.ButtonStyle = UIElementButtonStyle.Button3D;
                btnRefreshPrice.UseAppStyling = false;
                btnRefreshPrice.UseOsThemes = DefaultableBoolean.False;

                btnDoneAway.BackColor = Color.FromArgb(72, 99, 160);
                btnDoneAway.ForeColor = Color.White;
                btnDoneAway.Font = new Font(TradingTicketConstants.LIT_FONT_NAME, 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                btnDoneAway.ButtonStyle = UIElementButtonStyle.Button3D;
                btnDoneAway.UseAppStyling = false;
                btnDoneAway.UseOsThemes = DefaultableBoolean.False;

                btnClear.BackColor = Color.FromArgb(55, 67, 85);
                btnClear.ForeColor = Color.White;
                btnClear.Font = new Font(TradingTicketConstants.LIT_FONT_NAME, 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                btnClear.ButtonStyle = UIElementButtonStyle.Button3D;
                btnClear.UseAppStyling = false;
                btnClear.UseOsThemes = DefaultableBoolean.False;

                btnCommit.BackColor = Color.FromArgb(104, 156, 46);
                btnCommit.ForeColor = Color.White;
                btnCommit.Font = new Font(TradingTicketConstants.LIT_FONT_NAME, 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                btnCommit.ButtonStyle = UIElementButtonStyle.Button3D;
                btnCommit.UseAppStyling = false;
                btnCommit.UseOsThemes = DefaultableBoolean.False;
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
        /// Sets the buttons placement.
        /// </summary>
        private void SetButtonsPlacement()
        {
            try
            {
                tblOrderButtons.ColumnStyles.Clear();
                tblOrderButtons.Controls.Clear();
                if (_tradingTicketParent == TradingTicketParent.WatchList || _tradingTicketParent == TradingTicketParent.OptionChain)
                {
                    tblOrderButtons.ColumnCount = 6;

                    tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
                    tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
                    tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
                    tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
                    tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    tblOrderButtons.Controls.Add(btnRefreshPrice, 1, 0);
                    tblOrderButtons.Controls.Add(btnStage, 2, 0);
                    tblOrderButtons.Controls.Add(btnDoneAway, 3, 0);
                    tblOrderButtons.Controls.Add(btnSend, 4, 0);
                }
                else
                {
                    if (IsEditOrders)
                    {
                        tblOrderButtons.ColumnCount = 4;
                        tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
                        tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
                        tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        tblOrderButtons.Controls.Add(btnReplace, 1, 0);
                        tblOrderButtons.Controls.Add(btnCancel, 2, 0);
                    }
                    else
                    {
                        tblOrderButtons.ColumnCount = 5;
                        tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
                        tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
                        tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
                        tblOrderButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        tblOrderButtons.Controls.Add(btnRefreshPrice, 1, 0);
                        tblOrderButtons.Controls.Add(btnDoneAway, 2, 0);
                        tblOrderButtons.Controls.Add(btnSend, 3, 0);
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
        /// Sets the selection CheckBox at first position.
        /// </summary>
        /// <param name="checkboxName">Name of the checkbox.</param>
        private void SetCheckBoxAtFirstPosition(String checkboxName)
        {
            try
            {
                grdTrades.DisplayLayout.Bands[0].Columns[checkboxName].Hidden = false;
                grdTrades.DisplayLayout.Bands[0].Columns[checkboxName].Header.VisiblePosition = 0;
                grdTrades.DisplayLayout.Bands[0].Columns[checkboxName].Width = 45;
                grdTrades.DisplayLayout.Bands[0].Columns[checkboxName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                grdTrades.DisplayLayout.Bands[0].Columns[checkboxName].Header.Fixed = true;
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

        public void DeleteRowsFromGrid()
        {
            try
            {
                if (grdTrades.InvokeRequired)
                {
                    grdTrades.Invoke((MethodInvoker)delegate ()
                    {
                        DeleteRowsFromGrid();
                        return;
                    });
                }
                foreach (int index in deleteTradeIndexes)
                {
                    if (grdTrades.Rows.Count > index)
                    {
                        if (IsEditOrders)
                            grdTrades.Rows[index].Delete(false);
                        else
                        {
                            double mttLeavesQty = 0;
                            double qty = 0;
                            Double.TryParse(grdTrades.Rows[index].Cells[OrderFields.PROPERTY_QUANTITY_MTT].Text.Trim(), out mttLeavesQty);
                            Double.TryParse(grdTrades.Rows[index].Cells[OrderFields.PROPERTY_QUANTITY].Text.Trim(), out qty);
                            if (mttLeavesQty - qty == 0)
                            {
                                grdTrades.Rows[index].Delete(false);
                            }
                            else
                            {
                                grdTrades.Rows[index].Cells[OrderFields.PROPERTY_QUANTITY_MTT].Value = mttLeavesQty - qty;
                                grdTrades.Rows[index].Cells[OrderFields.PROPERTY_QUANTITY].Value = mttLeavesQty - qty;
                            }
                        }
                    }
                }
                deleteTradeIndexes.Clear();
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

        private List<int> deleteTradeIndexes = new List<int>();

        /// <summary>
        /// Deletes the trade from grid when MTTLeavesQty gets zero.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="columnName">Name of the column.</param>
        public void MarkTradeForDeletion(int index)
        {
            try
            {
                lock (deleteTradeIndexes)
                {
                    deleteTradeIndexes.Add(index);
                    //if (deleteTradeIndexes.Count > 99)
                    //    DeleteRowsFromGrid();
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
        /// Sets the sequence of columns.
        /// If no order of columns is saved as xml, then sequence of column should be same as in TT.
        /// </summary>
        private void SetSequenceOfColumns()
        {
            try
            {
                string[] columns =
                {
                    OrderFields.PROPERTY_SYMBOL, OrderFields.PROPERTY_ORDER_SIDE,OrderFields.PROPERTY_ACCOUNT,  OrderFields.PROPERTY_COUNTERPARTY_NAME, TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING,
                    OrderFields.PROPERTY_VENUE, OrderFields.PROPERTY_ALGOSTRATEGYNAME, OrderFields.PROPERTY_QUANTITY,OrderFields.PROPERTY_TIF_TAGVALUE,OrderFields.PROPERTY_EXPIRETIME,
                    OrderFields.PROPERTY_ORDER_TYPE, OrderFields.PROPERTY_AVGPRICE, OrderFields.PROPERTY_STOP_PRICE, TradingTicketConstants.COLUMN_LIMITPRICE
                };
                for (int i = 0; i < columns.Length; i++)
                {
                    grdTrades.DisplayLayout.Bands[0].Columns[columns[i]].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[columns[i]].Header.VisiblePosition = i + 1;
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
        /// Sets the grid column layout.
        /// </summary>
        private void SetGridColumnLayout()
        {
            try
            {
                _shortLocatePreferences = Dataobj.GetShortLocatePreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                grdTrades.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                grdTrades.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;
                grdTrades.DisplayLayout.Override.SupportDataErrorInfo = SupportDataErrorInfo.RowsAndCells;
                grdTrades.DisplayLayout.Override.RowSizing = RowSizing.Free;
                grdTrades.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;

                string startPath = Application.StartupPath;
                string multiTradePreferencesPath = startPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID;
                string multiTradePrefFile = multiTradePreferencesPath + "\\" + TradingTicketConstants.MULTI_TRADING_TICKET_FILE;

                if (File.Exists(multiTradePrefFile))
                {
                    UpdateUnexecutedQty();

                    if (_tradingTicketParent == TradingTicketParent.WatchList || _tradingTicketParent == TradingTicketParent.OptionChain || IsEditOrders)
                        grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_SIDE].CellActivation = Activation.AllowEdit;
                    else
                        grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_SIDE].CellActivation = Activation.NoEdit;

                    if (_shortLocatePreferences.Rebatefees == ShortLocateRebateFee.BPS.ToString())
                        grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ShortRebate].Header.Caption = OrderFields.CAPTION_BORROWERRATEBPS;
                    else
                        grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ShortRebate].Header.Caption = OrderFields.CAPTION_BORROWERRATECENT;

                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_LIMITPRICE].CellActivation = Activation.ActivateOnly;

                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_MARKETPRICE].CellActivation = Activation.ActivateOnly;

                    if (grdTrades.DisplayLayout.Bands[0].Columns.Exists(OrderFields.PROPERTY_AUECLOCALDATE))
                    {
                        UltraDateTimeEditor ultraDT = new UltraDateTimeEditor();
                        grdTrades.DisplayLayout.Bands[0].Columns[Global.OrderFields.PROPERTY_AUECLOCALDATE].EditorComponent = ultraDT;
                    }

                    if (!grdTrades.DisplayLayout.Bands[0].Columns.Exists(TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING))
                    {
                        SetAccountBrokerMappingColumn();
                        grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Header.VisiblePosition = grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].Header.VisiblePosition + 1;
                    }
                }
                else
                {
                    foreach (UltraGridColumn ultraGridColumn in grdTrades.DisplayLayout.Bands[0].Columns)
                    {
                        ultraGridColumn.Hidden = true;
                        ultraGridColumn.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }

                    grdTrades.DisplayLayout.Bands[0].Columns.Add(TradingTicketConstants.COLUMN__DUMMYCHECKBOX, TradingTicketConstants.COLUMN__DUMMYCHECKBOX);
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN__DUMMYCHECKBOX].DataType = typeof(bool);
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN__DUMMYCHECKBOX].Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN__DUMMYCHECKBOX].CellActivation = Activation.Disabled;
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN__DUMMYCHECKBOX].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN__DUMMYCHECKBOX].Hidden = true;

                    grdTrades.DisplayLayout.Bands[0].Columns.Add(OrderFields.PROPERTY_QUANTITY_MTT, OrderFields.PROPERTY_QUANTITY_MTT);
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_QUANTITY_MTT].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_QUANTITY_MTT].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    UpdateUnexecutedQty();

                    grdTrades.DisplayLayout.Bands[0].Columns.Add(TradingTicketConstants.COLUMN_SETTLEMENT_CURRENCYNAME, TradingTicketConstants.CAPTION_SETTLEMENT_CURRENCY);
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_SETTLEMENT_CURRENCYNAME].Header.Caption = TradingTicketConstants.CAPTION_SETTLEMENT_CURRENCY;
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_SETTLEMENT_CURRENCYNAME].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_SETTLEMENT_CURRENCYNAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    if (_tradingTicketParent == TradingTicketParent.WatchList || _tradingTicketParent == TradingTicketParent.OptionChain || IsEditOrders)
                        grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_SIDE].CellActivation = Activation.AllowEdit;
                    else
                        grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_SIDE].CellActivation = Activation.NoEdit;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_SIDE].Header.Caption = TradingTicketConstants.CAPTION_ORDER_SIDE;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_SIDE].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_SIDE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_TYPE].Header.Caption = TradingTicketConstants.CAPTION_ORDER_TYPE;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_TYPE].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_TYPE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE1].Header.Caption = TradingTicketConstants.CAPTION_TradeAttribute1;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE1].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE1].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE2].Header.Caption = TradingTicketConstants.CAPTION_TradeAttribute2;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE2].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE2].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE3].Header.Caption = TradingTicketConstants.CAPTION_TradeAttribute3;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE3].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE3].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE4].Header.Caption = TradingTicketConstants.CAPTION_TradeAttribute4;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE4].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE4].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE5].Header.Caption = TradingTicketConstants.CAPTION_TradeAttribute5;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE5].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE5].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE6].Header.Caption = TradingTicketConstants.CAPTION_TradeAttribute6;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE6].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADEATTRIBUTE6].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].Header.Caption = TradingTicketConstants.CAPTION_BROKER;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_STRATEGY].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_STRATEGY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ACCOUNT].Header.Caption = TradingTicketConstants.CAPTION_ALLOCATION;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ACCOUNT].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ACCOUNT].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.CAPTION_SYMBOL].Header.Caption = TradingTicketConstants.CAPTION_SYMBOL;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.CAPTION_SYMBOL].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.CAPTION_SYMBOL].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.CAPTION_SYMBOL].CellActivation = Activation.NoEdit;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_FACTSETSYMBOL].Header.Caption = OrderFields.CAPTION_FACTSET_SYMBOL;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_FACTSETSYMBOL].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_FACTSETSYMBOL].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_FACTSETSYMBOL].CellActivation = Activation.NoEdit;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ACTIVSYMBOL].Header.Caption = OrderFields.CAPTION_ACTIV_SYMBOL;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ACTIVSYMBOL].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ACTIVSYMBOL].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ACTIVSYMBOL].CellActivation = Activation.NoEdit;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_QUANTITY].Header.Caption = TradingTicketConstants.CAPTION_QUANTITY;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_QUANTITY].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_QUANTITY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTED_QTY].Header.Caption = TradingTicketConstants.CAPTION_TARGET_QUANTITY;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTED_QTY].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTED_QTY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_BORROWERID].Header.Caption = OrderFields.CAPTION_BORROWERID;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_BORROWERID].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_BORROWERID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    if (_shortLocatePreferences.Rebatefees == ShortLocateRebateFee.BPS.ToString())
                        grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ShortRebate].Header.Caption = OrderFields.CAPTION_BORROWERRATEBPS;
                    else
                        grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ShortRebate].Header.Caption = OrderFields.CAPTION_BORROWERRATECENT;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ShortRebate].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ShortRebate].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_BORROWERBROKER].Header.Caption = OrderFields.CAPTION_BORROWERBROKER;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_BORROWERBROKER].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_BORROWERBROKER].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TIF_TAGVALUE].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TIF_TAGVALUE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_VENUE].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_VENUE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADING_ACCOUNT].Header.Caption = TradingTicketConstants.CAPTION_TRADER;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADING_ACCOUNT].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRADING_ACCOUNT].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    //grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CURRENCYNAME].Header.Caption = TradingTicketConstants.CAPTION_SETTLEMENT_CURRENCY;
                    //grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CURRENCYNAME].Hidden = false;
                    //grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CURRENCYNAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_FXRATE].Header.Caption = TradingTicketConstants.CAPTION_FX_RATE;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_FXRATE].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_FXRATE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;


                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].Header.Caption = TradingTicketConstants.CAPTION_FX_OPERATOR;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_COMMISSIONRATE].Header.Caption = TradingTicketConstants.CAPTION_COMMISSION_RATE;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_COMMISSIONRATE].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_COMMISSIONRATE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Header.Caption = TradingTicketConstants.CAPTION_SOFT_RATE;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SOFTCOMMISSIONCALCBASIS].Header.Caption = TradingTicketConstants.CAPTION_SOFT_BASIS;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SOFTCOMMISSIONCALCBASIS].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SOFTCOMMISSIONCALCBASIS].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CALCBASIS].Header.Caption = TradingTicketConstants.CAPTION_COMMISSION_BASIS;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CALCBASIS].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CALCBASIS].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_HANDLING_INST_TagValue].Header.Caption = TradingTicketConstants.CAPTION_HANDLING_INSTRUCTIONS;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_HANDLING_INST_TagValue].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_HANDLING_INST_TagValue].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTION_INST_TagValue].Header.Caption = TradingTicketConstants.CAPTION_EXECUTION_INSTRUCTIONS;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTION_INST_TagValue].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTION_INST_TagValue].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_STOP_PRICE].Header.Caption = TradingTicketConstants.CAPTION_STOP;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_STOP_PRICE].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_STOP_PRICE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_AVGPRICE].Header.Caption = TradingTicketConstants.CAPTION_PRICE;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_AVGPRICE].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_AVGPRICE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    AddColumnToGrid(TradingTicketConstants.COLUMN_LIMITPRICE, TradingTicketConstants.CAPTION_LIMIT, "", typeof(double));
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_LIMITPRICE].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    AddColumnToGrid(TradingTicketConstants.COLUMN_MARKETPRICE, TradingTicketConstants.CAPTION_MARKETPRICE, "", typeof(double));
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_MARKETPRICE].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_MARKETPRICE].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_MARKETPRICE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TEXT].Header.Caption = TradingTicketConstants.CAPTION_BROKER_NOTES;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TEXT].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TEXT].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_INTERNALCOMMENTS].Header.Caption = TradingTicketConstants.CAPTION_NOTES;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_INTERNALCOMMENTS].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_INTERNALCOMMENTS].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_IMPORTFILENAME].Header.Caption = OrderFields.PROPERTY_IMPORTFILENAME;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_IMPORTFILENAME].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_IMPORTFILENAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_IMPORTFILENAME].CellActivation = Activation.NoEdit;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALGOSTRATEGYNAME].Header.Caption = TradingTicketConstants.CAPTION_ALGOTYPE;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALGOSTRATEGYNAME].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALGOSTRATEGYNAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALGOSTRATEGYNAME].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALGOSTRATEGYNAME].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALGOSTRATEGYNAME].CellButtonAppearance.BackColor = Color.FromArgb(55, 67, 85);
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALGOSTRATEGYNAME].CellButtonAppearance.BackColor2 = Color.FromArgb(55, 67, 85);
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALGOSTRATEGYNAME].CellButtonAppearance.ForeColor = Color.White;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALGOSTRATEGYNAME].CellButtonAppearance.TextHAlign = HAlign.Center;

                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXPIRETIME].Header.Caption = OrderFields.CAPTION_EXPIRETIME;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXPIRETIME].Hidden = false;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXPIRETIME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXPIRETIME].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXPIRETIME].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXPIRETIME].CellButtonAppearance.BackColor = Color.FromArgb(55, 67, 85);
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXPIRETIME].CellButtonAppearance.BackColor2 = Color.FromArgb(55, 67, 85);
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXPIRETIME].CellButtonAppearance.ForeColor = Color.White;
                    grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXPIRETIME].CellButtonAppearance.TextHAlign = HAlign.Center;


                    if (grdTrades.DisplayLayout.Bands[0].Columns.Exists(OrderFields.PROPERTY_AUECLOCALDATE))
                    {
                        UltraDateTimeEditor ultraDT = new UltraDateTimeEditor();
                        grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_AUECLOCALDATE].Header.Caption = TradingTicketConstants.CAPTION_TRADE_DATE;
                        grdTrades.DisplayLayout.Bands[0].Columns[Global.OrderFields.PROPERTY_AUECLOCALDATE].EditorComponent = ultraDT;
                        grdTrades.DisplayLayout.Bands[0].Columns[Global.OrderFields.PROPERTY_AUECLOCALDATE].CellActivation = Activation.AllowEdit;
                        grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_AUECLOCALDATE].Hidden = true;
                        grdTrades.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_AUECLOCALDATE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    }                    
                    SetAccountBrokerMappingColumn();
                    SetSequenceOfColumns();
                }

                // Hide trade attribute columns (PROPERTY_TRADEATTRIBUTE7 to PROPERTY_TRADEATTRIBUTE45 and exclude them from the column chooser
                for (int i = 7; i <= 45; i++)
                {
                    string colName = OrderFields.PROPERTY_TRADEATTRIBUTE + i;
                    grdTrades.DisplayLayout.Bands[0].Columns[colName].Hidden = true;
                    grdTrades.DisplayLayout.Bands[0].Columns[colName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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
        /// Set property of Account <> Broker Mapping column
        /// </summary>
        private void SetAccountBrokerMappingColumn()
        {
            try
            {
                grdTrades.DisplayLayout.Bands[0].Columns.Add(TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING);
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Header.Caption = TradingTicketConstants.CAPTION_ACCOUNTBROKERMAPPING;
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Hidden = false;
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].CellButtonAppearance.BackColor = Color.FromArgb(55, 67, 85);
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].CellButtonAppearance.BackColor2 = Color.FromArgb(55, 67, 85);
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].CellButtonAppearance.ForeColor = Color.White;
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].CellButtonAppearance.TextHAlign = HAlign.Center;
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
        /// Updates the unexecuted qty in starting equal to Quantity
        /// </summary>
        void UpdateUnexecutedQty()
        {
            try
            {
                foreach (UltraGridRow row in grdTrades.Rows)
                {
                    row.Cells[OrderFields.PROPERTY_QUANTITY_MTT].Value = row.Cells[OrderFields.PROPERTY_QUANTITY].Value;
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
        /// Binds the data for strategy combo on bulk update UI
        /// </summary>
        /// <param name="values">the dropdown</param>
        /// <param name="columnName">name of the column</param>
        public void BindBulkCombo(StrategyCollection values, string columnName)
        {
            try
            {
                cmbStrategy.DisplayMember = TradingTicketConstants.LIT_NAME;
                cmbStrategy.ValueMember = TradingTicketConstants.LIT_STRATEGY_ID;
                cmbStrategy.DataSource = values;
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
        /// Algo control is set when value of venue is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cmbvenue_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    AlgoControlSetup();
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
        /// Update Venue when broker value gets changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbBroker_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    if (CachedDataManager.GetInstance.GetCounterPartyID(cmbBroker.Text) != int.MinValue && UpdateVenueOnBrokerChangeBulk != null)
                        UpdateVenueOnBrokerChangeBulk(this, new EventArgs<string>(cmbBroker.Text));
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
        /// This function is used to set visibilty of button algo
        /// </summary>
        private void AlgoControlSetup()
        {
            try
            {
                if (cmbvenue.Value != null && cmbvenue.Text.ToLower().Equals("algo"))
                {
                    if (CachedDataManager.GetInstance.IsAlgoBrokerFromID(CachedDataManager.GetInstance.GetCounterPartyID(cmbBroker.Text)))
                        btnAlgo.Enabled = true;
                    else
                        btnAlgo.Enabled = false;
                    btnAlgo.Visible = true;
                }
                else
                {
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
        /// Set data source for Broker,Venue combo on bulk Update UI  and column Broker,Venue for first row based on AUECID
        /// </summary>
        /// <param name="values">The drop down.</param>
        /// <param name="columnName">Name of the column.</param>
        public void BindBulkCombo(ValueList values, string columnName)
        {
            try
            {
                if (InvokeRequired)
                {
                    MethodInvoker del = delegate
                    {
                        BindBulkCombo(values, columnName);
                    };
                    BeginInvoke(del);
                    return;
                }
                switch (columnName)
                {
                    case OrderFields.PROPERTY_VENUE:
                        cmbvenue.ValueList = values;
                        cmbvenue.Value = null;
                        break;
                    case OrderFields.PROPERTY_COUNTERPARTY_NAME:
                        cmbBroker.ValueList = values;
                        cmbBroker.DrawFilter = new BrokerComboDrawFilter();
                        cmbBroker.Value = null;
                        break;
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

        private void cmb_ValueChanged(object sender, System.EventArgs e)
        {
            CheckIfErrorExists((UltraComboEditor)sender);
        }

        private void cmbTIF_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                btnExpireTime.Visible = false;
                _expireTime = TradingTicketConstants.CAPTION_NA_SLASH;
                btnExpireTime.Text = TradingTicketConstants.CAPTION_NA_SLASH;
                if (CheckIfErrorExists((UltraComboEditor)sender) && cmbTIF.Value != null)
                {
                    string tifStringValue = TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(cmbTIF.Value.ToString());
                    if (tifStringValue.Equals(FIXConstants.TIF_GTD))
                    {
                        btnExpireTime.Visible = true;
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

        public void BindBulkCombo(DataTable values, string columnName)
        {
            try
            {
                if (InvokeRequired)
                {
                    MethodInvoker del = delegate
                    {
                        BindBulkCombo(values, columnName);
                    };
                    BeginInvoke(del);
                    return;
                }
                switch (columnName)
                {
                    case OrderFields.PROPERTY_ORDER_SIDE:
                        if (_tradingTicketParent == TradingTicketParent.WatchList || _tradingTicketParent == TradingTicketParent.OptionChain
                             || IsEditOrders)
                        {
                            cmbOrderSide.Enabled = true;
                            cmbOrderSide.DataSource = values;
                            cmbOrderSide.DisplayMember = OrderFields.PROPERTY_ORDER_SIDE;
                            cmbOrderSide.ValueMember = OrderFields.PROPERTY_ORDER_SIDEID;
                            cmbOrderSide.DataBind();
                        }
                        break;
                    case OrderFields.PROPERTY_ACCOUNT:
                        cmbAllocation.Value = null;
                        cmbAllocation.DataSource = null;
                        cmbAllocation.DataSource = values;
                        cmbAllocation.DisplayMember = OrderFields.PROPERTY_LEVEL1NAME;
                        cmbAllocation.ValueMember = OrderFields.PROPERTY_LEVEL1ID;
                        cmbAllocation.DataBind();
                        break;
                    case OrderFields.PROPERTY_ORDER_TYPE:
                        cmbOrderType.Value = null;
                        cmbOrderType.DataSource = null;
                        cmbOrderType.DataSource = values;
                        cmbOrderType.DisplayMember = OrderFields.PROPERTY_ORDER_TYPE;
                        cmbOrderType.ValueMember = OrderFields.PROPERTY_ORDER_TYPE_ID;
                        cmbOrderType.DataBind();
                        break;
                    case OrderFields.PROPERTY_TIF_TAGVALUE:
                        cmbTIF.Value = null;
                        cmbTIF.DataSource = null;
                        cmbTIF.DataSource = values;
                        cmbTIF.DisplayMember = OrderFields.PROPERTY_TIF;
                        cmbTIF.ValueMember = OrderFields.PROPERTY_TIFID;
                        cmbTIF.DataBind();
                        break;
                    case OrderFields.PROPERTY_TRADING_ACCOUNT:
                        cmbTrader.Value = null;
                        cmbTrader.DataSource = null;
                        cmbTrader.DataSource = values;
                        cmbTrader.DisplayMember = TradingTicketConstants.LIT_DISPLAY;
                        cmbTrader.ValueMember = TradingTicketConstants.LIT_VALUE;
                        cmbTrader.DataBind();
                        break;
                    case OrderFields.PROPERTY_EXECUTION_INST_TagValue:
                        cmbExecution.Value = null;
                        cmbExecution.DataSource = null;
                        cmbExecution.DataSource = values;
                        cmbExecution.DisplayMember = OrderFields.CAPTION_EXECUTION_INST;
                        cmbExecution.ValueMember = OrderFields.PROPERTY_EXECUTION_INSTID;
                        cmbExecution.DataBind();
                        break;
                    case OrderFields.PROPERTY_HANDLING_INST_TagValue:
                        cmbHandling.Value = null;
                        cmbHandling.DataSource = null;
                        cmbHandling.DataSource = values;
                        cmbHandling.DisplayMember = OrderFields.PROPERTY_HANDLING_INST;
                        cmbHandling.ValueMember = OrderFields.PROPERTY_HANDLING_INSTID;
                        cmbHandling.DataBind();
                        break;
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
        /// Fills the commission basis.
        /// </summary>
        /// <param name="list">The list.</param>
        public void FillBulkCommissionBasis(IList list)
        {
            try
            {
                cmbCommission.DataSource = list;
                cmbCommission.DisplayMember = TradingTicketConstants.LIT_VALUE_SMALL;
                cmbCommission.ValueMember = TradingTicketConstants.LIT_KEY;

                cmbSoft.DataSource = list;
                cmbSoft.DisplayMember = TradingTicketConstants.LIT_VALUE_SMALL;
                cmbSoft.ValueMember = TradingTicketConstants.LIT_KEY;
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
        public void SetBulkTradeAttributeValueList(BindableValueList[] vls)
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
                    if (cmbTradeAttribute1.DataSource == null)
                    {
                        cmbTradeAttribute1.ValueList = vls[0];
                        cmbTradeAttribute2.ValueList = vls[1];
                        cmbTradeAttribute3.ValueList = vls[2];
                        cmbTradeAttribute4.ValueList = vls[3];
                        cmbTradeAttribute5.ValueList = vls[4];
                        cmbTradeAttribute6.ValueList = vls[5];
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
        /// Sets the property for first row.
        /// </summary>
        private void SetPropertiesForBulkUI()
        {
            try
            {
                grdTrades.AfterCellListCloseUp += delegate { grdTrades.UpdateData(); };

                grdTrades.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdTrades_AfterCellUpdate);

                if (grdTrades.Rows.Count > 0)
                {
                    grdTrades.ActiveRow = grdTrades.Rows[0];
                }

                if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    cmbTradeAttribute6.Value = null;
                    cmbTradeAttribute6.Enabled = false;
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
        /// Sets up multi trading ticket UI.
        /// </summary>
        private async void SetUpMultiTradingTicketUI()
        {
            try
            {
                _mttPresenter = new MTTPresenter(this);
                _mttPresenter.PriceForComplianceNotAvailable += PriceNotAvailableForCompliance;
                TradeManagerExtension.GetInstance().CounterPartyStatusUpdate += MultiTradingTicket_CounterPartyStatusUpdate;
                FormClosed += MultiTradingTicket_FormClosed;
                FormClosing += MultiTradingTicket_FormClosing;
                Disposed += MultiTradingTicket_Disposed;
                _transferTradeRules = CachedDataManager.GetInstance.GetTransferTradeRules();
                SetButtonsColor();
                SetButtonsPlacement();
                LoadGridLayout();
                SetGridColumnLayout();
                AddCheckBoxinGrid();
                AddNumericUpDown();
                ValidateAllTransferTradeRules(grdTrades);
                btnRefreshPrice_Click(null, null);
                await System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    if (BindAllDropDowns != null)
                    {
                        BindAllDropDowns(this, EventArgs.Empty);
                    }
                });
                SetPropertiesForBulkUI();
                if (CustomThemeHelper.ApplyTheme)
                {
                    CustomThemeHelper.SetThemeProperties(grpBoxBulkUpdate, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_TRADING_TICKET);
                    CustomThemeHelper.SetThemeProperties(grdTrades, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_MULTI_TRADING_TICKET);
                    ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, Text, CustomThemeHelper.UsedFont);
                }
                DoMiscSetup();
                //SetVenueIfVenueEmpty();
                SetStatusBarMessage("5.Finishing up.");
                if (TagDatabaseManagerWork != null)
                {
                    TagDatabaseManagerWork(this, EventArgs.Empty);
                }
                EnableOrDisableAccountBrokerMappingCell();
                SetStatusBarMessage("Data Loaded.");
                _isGridEdited = false;
                IsMTTSettingUp = false;
                UpdateInitialPrices();
                EnableOrDisableMTT(true);
                BringToFront();
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

        private void MultiTradingTicket_Disposed(object sender, EventArgs e)
        {
            try
            {
                TradeManagerExtension.GetInstance().CounterPartyStatusUpdate -= MultiTradingTicket_CounterPartyStatusUpdate;
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
        /// Handles the FormClosing event of the MultiTradingTicket Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void MultiTradingTicket_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_isGridEdited && IsEditOrders)
                {
                    DialogResult userResponse = MessageBox.Show("Do you want to cancel without replacing orders?", "Multi Trading Ticket", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (userResponse != DialogResult.Yes)
                    {
                        e.Cancel = true;
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

        private void GrdTrades_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            e.Cancel = true;
        }
        private void GrdTrades_CellDataError(object sender, Infragistics.Win.UltraWinGrid.CellDataErrorEventArgs e)
        {
            try
            {
                e.RestoreOriginalValue = false;
                e.StayInEditMode = false;
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
        /// GrdTrades MouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdTrades_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                _mouseXPosition = e.Location.X;
                _mouseYPosition = e.Location.Y;
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
        /// Hits the event when Algo button click on grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTrades_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_ALGOSTRATEGYNAME))
                {
                    if (_algoControlPopUp == null)
                    {
                        _currentOrder = e.Cell.Row.ListObject as OrderSingle;
                        _algoControlPopUp = new AlgoControlPopUp();
                        int _defaultAlgoType = int.MinValue;

                        if (_currentOrder.AlgoStrategyName != TradingTicketConstants.CAPTION_NONE)
                        {
                            _defaultAlgoType = Convert.ToInt32(_currentOrder.AlgoStrategyID);
                        }
                        _algoControlPopUp.OnOkevent += AlgoStrategyTradeDetails_OnOkEvent;
                        _algoControlPopUp.OnOkEvent1 += AlgoStrategyDictionaryDetails_OnOkEvent;
                        _algoControlPopUp.Bind(_currentOrder.CounterPartyID.ToString(), CachedDataManager.GetInstance.GetUnderLyingText(_currentOrder.UnderlyingID), _defaultAlgoType, _currentOrder.CounterPartyID.ToString(), _currentOrder.AlgoProperties.TagValueDictionary, _currentOrder.AUECID);
                        _algoControlPopUp.FormClosed += algoControlPopUp_FormClosed;
                        _algoControlPopUp.StartPosition = FormStartPosition.CenterParent;
                        _algoControlPopUp.ShowDialog(this);
                    }
                }
                if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_EXPIRETIME))
                {
                    if (dtExpireTime != null)
                    {
                        _expireRowIndex = e.Cell.Row.Index;
                        _currentOrder = e.Cell.Row.ListObject as OrderSingle;
                        SetExpiryDate(_currentOrder.ExpireTime);
                        if (grpBoxCommisionAttribute.Expanded == true)
                            _mouseYPosition = _mouseYPosition + 68;
                        pnlExpireTime.Location = new System.Drawing.Point(_mouseXPosition, _mouseYPosition - 20);
                        dtExpireTime.DroppedDown = true;
                    }
                }
                if (e.Cell.Column.Key.Equals(TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING))
                {
                    _currentOrder = e.Cell.Row.ListObject as OrderSingle;
                    Dictionary<int, int> accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(_currentOrder.AccountBrokerMapping);
                    BrokersConnectionStatus brokersConnectionStatus = new BrokersConnectionStatus(accountBrokerMapping, cmbBroker.ValueList);
                    brokersConnectionStatus.ShowDialog();
                    ResetErrorInBrokerColumn(e.Cell.Row.Index);
                    _currentOrder.AccountBrokerMapping = JsonHelper.SerializeObject(accountBrokerMapping);
                    SetUnmappedBrokerError(e.Cell.Row.Index, accountBrokerMapping);
                    var errorValue = grdTrades.Rows[e.Cell.Row.Index].DataErrorInfo.GetColumnError(OrderFields.PROPERTY_COUNTERPARTY_NAME);
                    if (errorValue != String.Empty)
                        SetGridError(e.Cell.Row.Index, errorValue, grdTrades.Rows[e.Cell.Row.Index].Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Column.Key);
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
        /// Hits the event when Algo button click on Bulk update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlgo_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_algoControlPopUp == null)
                {
                    _algoControlPopUp = new AlgoControlPopUp();
                    int _defaultAlgoType = int.MinValue;
                    if (btnAlgo.Text != "NONE")
                    {
                        _defaultAlgoType = Convert.ToInt32(_algoStrategyID);
                    }
                    _algoControlPopUp.IsBulkMTT = true;
                    _algoControlPopUp.OnOkevent += AlgoStrategyTradeDetails_OnOkEvent;
                    _algoControlPopUp.Bind(cmbBroker.Value.ToString(), string.Empty, _defaultAlgoType, cmbBroker.Value.ToString(), _tagValueDictionary, int.MinValue, "BulkUpdate");
                    _algoControlPopUp.Height = 240;
                    _algoControlPopUp.Width = 265;
                    _algoControlPopUp.FormClosed += algoControlPopUp_FormClosed;
                    _algoControlPopUp.StartPosition = FormStartPosition.CenterParent;
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

        private void BtnExpireTime_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (dtExpireTime != null)
                {
                    SetExpiryDate(btnExpireTime.Text);
                    pnlExpireTime.Location = new System.Drawing.Point(pnlTIF.Location.X + pnlTIF.Width + 8, pnlTIF.Location.Y + 58);
                    dtExpireTime.DroppedDown = true;
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
                if (_currentOrder != null)
                {
                    _currentOrder.AlgoProperties.TagValueDictionary = e;
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
        /// Hits the event when Ok button is clicked on AlgoControlPopup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void AlgoStrategyTradeDetails_OnOkEvent(object sender, AlgoStrategyControl e)
        {
            try
            {
                if (_currentOrder != null)
                {
                    _currentOrder.AlgoStrategyID = e.strategyId;
                    _currentOrder.AlgoStrategyName = e.strategyName;
                }
                else
                {
                    _algoStrategyID = e.strategyId;
                    if (e.strategyName == "Algo Type")
                    {
                        _algoStrategyName = "";
                        btnAlgo.Text = "NONE";
                    }
                    else
                    {
                        _algoStrategyName = e.strategyName;
                        btnAlgo.Text = e.strategyName;
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


        private void SetExpiryDateInOrder(string expiryDate)
        {
            try
            {
                if (_currentOrder != null)
                {
                    _currentOrder.ExpireTime = TradingTicketConstants.CAPTION_NA_SLASH;
                    string selectedDate = expiryDate;
                    if (!string.IsNullOrEmpty(selectedDate))
                    {
                        DateTime dtValue;
                        if (DateTime.TryParse(selectedDate, out dtValue))
                        {
                            _currentOrder.ExpireTime = dtValue.ToString("MM/dd/yyyy");
                            if (_expireRowIndex != -1)
                            {
                                grdTrades.Rows[_expireRowIndex].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_EXPIRETIME, null);
                                SetGridError(_expireRowIndex, "Invalid " + OrderFields.CAPTION_EXPIRETIME + "!", OrderFields.PROPERTY_EXPIRETIME);
                                SetGridError(_expireRowIndex, "The expiry date selected is already past date. Please select a valid Expiry Date.", OrderFields.PROPERTY_EXPIRETIME);
                            }
                        }
                    }
                    else
                    {
                        grdTrades.Rows[_expireRowIndex].DataErrorInfo.SetColumnError(OrderFields.PROPERTY_EXPIRETIME, "Invalid " + OrderFields.CAPTION_EXPIRETIME + "!");

                    }
                }
                else
                {
                    _expireTime = TradingTicketConstants.CAPTION_NA_SLASH; ;
                    btnExpireTime.Text = TradingTicketConstants.CAPTION_NA_SLASH;
                    DateTime dtValue;
                    if (DateTime.TryParse(expiryDate, out dtValue))
                    {
                        _expireTime = dtValue.ToString("MM/dd/yyyy");
                        btnExpireTime.Text = dtValue.ToString("MM/dd/yyyy");
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
                _currentOrder = null;

            }
        }


        /// <summary>
        /// Sets the value of column Algo Type when Venue change for a row
        /// </summary>
        /// <param name="index">The index.</param>
        public void UpdateAlgoType(UltraGridRow row)
        {
            try
            {
                if (row.Cells[OrderFields.PROPERTY_VENUE].Value.Equals(OrderFields.CAPTION_ALGOSTRATEGYNAME))
                {
                    OrderSingle order = row.ListObject as OrderSingle;
                    if (order.AlgoStrategyName.Equals(TradingTicketConstants.CAPTION_NA_DOT) || order.AlgoStrategyName.Equals(TradingTicketConstants.CAPTION_NA_SLASH))
                    {
                        row.Cells[OrderFields.PROPERTY_ALGOSTRATEGYNAME].Value = TradingTicketConstants.CAPTION_NONE;
                    }
                    else
                    {
                        row.Cells[OrderFields.PROPERTY_ALGOSTRATEGYNAME].Value = order.AlgoStrategyName;
                    }
                    row.Cells[OrderFields.PROPERTY_ALGOSTRATEGYNAME].Activation = Activation.ActivateOnly;
                }
                else
                {
                    row.Cells[OrderFields.PROPERTY_ALGOSTRATEGYNAME].Value = TradingTicketConstants.CAPTION_NA_SLASH;
                    row.Cells[OrderFields.PROPERTY_ALGOSTRATEGYNAME].Activation = Activation.Disabled;
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
        public void UpdateExpiryDate(UltraGridRow row)
        {
            try
            {
                if (row.Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value != null && (row.Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.Equals("Good Till Date") || (row.Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.Equals(FIXConstants.TIF_GTD))))
                {
                    OrderSingle order = row.ListObject as OrderSingle;
                    if (!order.ExpireTime.Equals(TradingTicketConstants.CAPTION_NA_SLASH) && !string.IsNullOrEmpty(order.ExpireTime))
                    {
                        dtExpireTime.Value = order.ExpireTime;
                        DateTime selectedDate = Convert.ToDateTime(order.ExpireTime);
                        row.Cells[OrderFields.PROPERTY_EXPIRETIME].Value = selectedDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        row.Cells[OrderFields.PROPERTY_EXPIRETIME].Value = TradingTicketConstants.CAPTION_NA_SLASH;
                    }
                    row.Cells[OrderFields.PROPERTY_EXPIRETIME].Activation = Activation.ActivateOnly;
                }
                else if (row.Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value != null)
                {
                    row.Cells[OrderFields.PROPERTY_EXPIRETIME].Value = TradingTicketConstants.CAPTION_NA_SLASH;
                    row.Cells[OrderFields.PROPERTY_EXPIRETIME].Activation = Activation.Disabled;
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
        /// Does the misc setup.
        /// </summary>
        private void DoMiscSetup()
        {
            try
            {
                foreach (UltraGridRow ulRow in grdTrades.DisplayLayout.Rows)
                {
                    OrderSingle orderSingle = ulRow.ListObject as OrderSingle;
                    if (orderSingle != null)
                    {

                        #region limit price
                        ulRow.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].Value = orderSingle.Price;
                        ulRow.Cells[OrderFields.PROPERTY_AVGPRICE].Value = orderSingle.Price;
                        if (!_transferTradeRules.IsAllowUserToChangeOrderType)
                        {
                            if (orderSingle.OrderType.Equals(TradingTicketConstants.LIT_LIMIT))
                            {
                                ulRow.Cells[OrderFields.PROPERTY_ORDER_TYPE].Activation = Activation.Disabled;
                                if (orderSingle.OrderSide.Equals(TradingTicketConstants.LIT_BUY) || orderSingle.OrderSide.Equals(TradingTicketConstants.LIT_BUY_TO_COVER) || orderSingle.OrderSide.Equals(TradingTicketConstants.LIT_BUY_TO_CLOSE))
                                {
                                    ((UltraNumericEditor)ulRow.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].EditorComponentResolved).MaxValue = orderSingle.Price;
                                }
                                else
                                {
                                    ((UltraNumericEditor)ulRow.Cells[TradingTicketConstants.COLUMN_LIMITPRICE].EditorComponentResolved).MinValue = orderSingle.Price;
                                }
                            }
                        }
                        #endregion

                        #region fx columns
                        if ((int)ulRow.Cells[OrderFields.PROPERTY_CURRENCYID].Value == CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                        {
                            if (_tradingTicketParent == TradingTicketParent.WatchList || _tradingTicketParent == TradingTicketParent.OptionChain)
                            {
                                ulRow.Cells[OrderFields.PROPERTY_FXRATE].Value = 1.0m;
                            }
                            ulRow.Cells[OrderFields.PROPERTY_FXRATE].Activation = Activation.NoEdit;
                            ulRow.Cells[OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].Value = Operator.M.ToString();
                            ulRow.Cells[OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].Activation = Activation.NoEdit;

                        }
                        else
                        {
                            ulRow.Cells[OrderFields.PROPERTY_FXRATE].Activation = Activation.AllowEdit;
                            ulRow.Cells[OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].Activation = Activation.AllowEdit;
                        }
                        #endregion

                        #region account strategy
                        ulRow.Cells[OrderFields.PROPERTY_AUECLOCALDATE].Value = DateTime.Now.ToLongTimeString();
                        if (orderSingle.Level1ID < 0)
                        {
                            orderSingle.Account = TradingTicketConstants.LIT_UNALLOCATED;
                        }
                        if (orderSingle.OriginalAllocationPreferenceID > 0)
                        {
                            if (orderSingle.TransactionSource == TransactionSource.PST || orderSingle.TransactionSource == TransactionSource.TradeImport ||
                            orderSingle.TransactionSource == TransactionSource.Rebalancer)
                            {
                                ulRow.Cells[OrderFields.PROPERTY_ACCOUNT].Activation = Activation.NoEdit;
                                ulRow.Cells[OrderFields.PROPERTY_STRATEGY].Activation = Activation.NoEdit;
                            }
                        }
                        #endregion

                        #region softcommisionbasis
                        if (ulRow.Cells[OrderFields.PROPERTY_SOFTCOMMISSIONCALCBASIS].Text == TradingTicketConstants.C_LIT_AUTO)
                        {
                            ulRow.Cells[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Value = 0;
                            ulRow.Cells[OrderFields.PROPERTY_SOFTCOMMISSIONRATE].Activation = Activation.NoEdit;
                        }
                        if (ulRow.Cells[OrderFields.PROPERTY_CALCBASIS].Text == TradingTicketConstants.C_LIT_AUTO)
                        {
                            ulRow.Cells[OrderFields.PROPERTY_COMMISSIONRATE].Value = 0;
                            ulRow.Cells[OrderFields.PROPERTY_COMMISSIONRATE].Activation = Activation.NoEdit;
                        }
                        #endregion

                        #region algotype
                        UpdateAlgoType(ulRow);
                        #endregion

                        UpdateExpiryDate(ulRow);
                        #region brokercolor
                        if (TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(orderSingle.CounterPartyID) == BusinessObjects.PranaInternalConstants.ConnectionStatus.CONNECTED)
                            ulRow.Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Appearance.ForeColor = Color.Green;
                        else
                            ulRow.Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Appearance.ForeColor = Color.Red;
                        #endregion
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
        /// Enable account broker mapping cell if trade is custodian else disable the cell 
        /// </summary>
        public void EnableOrDisableAccountBrokerMappingCell()
        {
            try
            {
                foreach (UltraGridRow ulRow in grdTrades.DisplayLayout.Rows)
                {
                    OrderSingle orderSingle = ulRow.ListObject as OrderSingle;
                    if (orderSingle.IsUseCustodianBroker)
                    {
                        ulRow.Cells[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Value = TradingTicketConstants.CAPTION_ACCOUNTBROKER;
                    }
                    else
                    {
                        ulRow.Cells[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Activation = Activation.Disabled;
                        ulRow.Cells[TradingTicketConstants.COLUMN_ACCOUNTBROKERMAPPING].Value = TradingTicketConstants.CAPTION_NA_SLASH;
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
        /// Validates all transfer trade rules.
        /// </summary>
        /// <param name="grd">The GRD.</param>
        private void ValidateAllTransferTradeRules(UltraGrid grd)
        {
            try
            {
                if (_transferTradeRules.IsVenueCPChange)
                {
                    AllowEditColumn(grd, OrderFields.PROPERTY_COUNTERPARTY_NAME);
                    AllowEditColumn(grd, OrderFields.PROPERTY_VENUE);
                }
                else
                {
                    DisAllowEditColumn(grd, OrderFields.PROPERTY_COUNTERPARTY_NAME);
                    DisAllowEditColumn(grd, OrderFields.PROPERTY_VENUE);
                }
                if (_transferTradeRules.IsAccountChange)
                    AllowEditColumn(grd, OrderFields.PROPERTY_ACCOUNT);
                else
                    DisAllowEditColumn(grd, OrderFields.PROPERTY_ACCOUNT);
                if (_transferTradeRules.IsStrategyChange)
                    AllowEditColumn(grd, OrderFields.PROPERTY_STRATEGY);
                else
                    DisAllowEditColumn(grd, OrderFields.PROPERTY_STRATEGY);
                if (_transferTradeRules.IsTradingAccChange)
                    AllowEditColumn(grd, OrderFields.PROPERTY_TRADING_ACCOUNT);
                else
                    DisAllowEditColumn(grd, OrderFields.PROPERTY_TRADING_ACCOUNT);
                if (_transferTradeRules.IsHandlingInstrChange)
                    AllowEditColumn(grd, OrderFields.PROPERTY_HANDLING_INST_TagValue);
                else
                    DisAllowEditColumn(grd, OrderFields.PROPERTY_HANDLING_INST_TagValue);
                if (_transferTradeRules.IsExecutionInstrChange)
                    AllowEditColumn(grd, OrderFields.PROPERTY_EXECUTION_INST_TagValue);
                else
                    DisAllowEditColumn(grd, OrderFields.PROPERTY_EXECUTION_INST_TagValue);
                if (_transferTradeRules.IsTIFChange)
                    AllowEditColumn(grd, OrderFields.PROPERTY_TIF_TAGVALUE);
                else
                    DisAllowEditColumn(grd, OrderFields.PROPERTY_TIF_TAGVALUE);
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
        /// Handles the Click event of the removeFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void removeFilter_Click(object sender, System.EventArgs e)
        {
            try
            {
                grdTrades.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                grdTrades.ActiveRowScrollRegion.Scroll(RowScrollAction.Top);
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

        private void grdTrades_FilterRow(object sender, FilterRowEventArgs e)
        {
            try
            {
                if (e.Row.Fixed)
                {
                    e.RowFilteredOut = false;
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
        /// Handles the AfterRowsDeleted event of the grdTrades control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void grdTrades_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                if (grdTrades.Rows != null)
                    if (grdTrades.Rows.Count == 0)
                    {
                        ClearMultiTradingTicket();
                        this.Close();
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
        /// Handles the BeforeCustomRowFilterDialog event of the grdTrades control.
        /// Used to theme the CustomRowFilterDialog
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BeforeCustomRowFilterDialogEventArgs"/> instance containing the event data.</param>
        private void grdTrades_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            try
            {
                e.CustomRowFiltersDialog.PaintDynamicForm();
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

        #region Dispose Methods

        ///// <summary>
        ///// Releases unmanaged and - optionally - managed resources.
        ///// </summary>
        ///// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (components != null)
                        components.Dispose();

                    if (_mttPresenter != null)
                        _mttPresenter.Dispose();

                    if (_dictOrdersBindedSymbolwise != null)
                        _dictOrdersBindedSymbolwise = null;

                    if (_listOrdersBinded != null)
                        _listOrdersBinded = null;

                    if (_priceSymbolSettings != null)
                        _priceSymbolSettings = null;

                    if (_loginUser != null)
                        _loginUser = null;

                    if (_securityMaster != null)
                        _securityMaster = null;

                    if (_mttPresenter != null)
                        _mttPresenter = null;

                    if (_transferTradeRules != null)
                        _transferTradeRules = null;


                    if (_algoControlPopUp != null)
                        _algoControlPopUp.Dispose();

                    if (_accountqty != null)
                        _accountqty.Dispose();

                    base.Dispose(isDisposing);
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
        /// Set error on status bar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        private void PriceNotAvailableForCompliance(object sender, EventArgs e)
        {
            try
            {
                errorProvider.SetIconAlignment(ultraStatusBar, ErrorIconAlignment.MiddleLeft);
                errorProvider.SetIconPadding(ultraStatusBar, -15);
                errorProvider.SetError(ultraStatusBar, TradingTicketConstants.MSG_PRICES_NOT_AVAILABLE);
                SetStatusBarMessage("     " + TradingTicketConstants.MSG_PRICES_NOT_AVAILABLE);
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
                System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                if (ComplianceCacheManager.GetPreTradeCheck(_loginUser.CompanyUserID))
                    ToolTip1.SetToolTip(this.btnSend, "Price Required for Compliance check");
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

        private void CmbCommission_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    nmrcCommission.Enabled = cmbCommission.Text != TradingTicketConstants.C_LIT_AUTO;
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

        private void CmbSoft_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender))
                {
                    nmrcSoft.Enabled = cmbSoft.Text != TradingTicketConstants.C_LIT_AUTO;
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

        private void CmbAllocation_ValueChanged(object sender, System.EventArgs e)
        {
            if (CheckIfErrorExists((UltraComboEditor)sender))
            {
                if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    Dictionary<int, string> cpList1 = MTTHelperManager.GetInstance().GetCounterparties();
                    ValueList cpListBulk = new ValueList();
                    foreach (KeyValuePair<int, string> counterParty in cpList1)
                    {
                        cpListBulk.ValueListItems.Add(counterParty.Key, counterParty.Value);
                    }
                    if (cmbAllocation.Value == null)
                    {
                        if (cpListBulk != null)
                        {
                            cpListBulk.SortStyle = ValueListSortStyle.Ascending;
                            BindBulkCombo(cpListBulk, OrderFields.PROPERTY_COUNTERPARTY_NAME);
                        }
                    }
                    else
                    {
                        cmbBroker.Value = null;
                        var accountID = Convert.ToInt32(cmbAllocation.Value.ToString());
                        ValueList cpList = cpListBulk;
                        if (_accountqty != null && _accountqty.AllocationOperationPreference != null && _accountqty.AllocationOperationPreference.OperationPreferenceId.ToString().Equals(cmbAllocation.Value))
                        {
                            List<int> accountIds = _accountqty.AllocationOperationPreference.GetSelectedAccountsList();
                            cpList = TTHelperManager.GetInstance().GetCounterpartiesFilterByAccount(accountIds, cpList);
                            if (cpList != null)
                            {
                                cpList.SortStyle = ValueListSortStyle.Ascending;
                                BindBulkCombo(cpList, OrderFields.PROPERTY_COUNTERPARTY_NAME);
                            }
                        }
                        else
                        {
                            AllocationOperationPreference operationPreference = _mttPresenter.AllocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, accountID);

                            if (operationPreference != null)
                            {
                                List<int> accountIds = operationPreference.GetSelectedAccountsList();
                                cpList = TTHelperManager.GetInstance().GetCounterpartiesFilterByAccount(accountIds, cpList);

                            }
                            else
                            {
                                if (accountID > 0)
                                {
                                    cpList = MTTHelperManager.GetInstance().GetCounterpartiesFilterByAccount(accountID, cpList);
                                }
                            }
                            if (cpList != null)
                            {
                                cpList.SortStyle = ValueListSortStyle.Ascending;
                                BindBulkCombo(cpList, OrderFields.PROPERTY_COUNTERPARTY_NAME);
                            }
                        }
                    }
                }
            }
        }

        private void CmbOrderType_ValueChanged(object sender, System.EventArgs e)
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

        //For Automation
        public void SelectAllOrders()
        {
            if (grdTrades.Rows != null)
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_SELECTCHECKBOX].SetHeaderCheckedState(grdTrades.Rows, true);
        }
        public void UnselectAllOrders()
        {
            if (grdTrades.Rows != null)
                grdTrades.DisplayLayout.Bands[0].Columns[TradingTicketConstants.COLUMN_SELECTCHECKBOX].SetHeaderCheckedState(grdTrades.Rows, false);
        }

        public void UpgradeGridAfterBrokerChange(int index, int BrokerID)
        {
            try
            {
                grdTrades.Rows[index].Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Value = BrokerID;
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

        /// DtExpireDate AfterDropDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtExpireDate_AfterDropDown(object sender, EventArgs e)
        {
            try
            {
                UltraCalendarInfo ultraCalendarInfo1 = (UltraCalendarInfo)this.dtExpireTime.CalendarInfo;
                ultraCalendarInfo1.MinDate = DateTime.Now.AddDays(-1);
                ultraCalendarInfo1.MaxDate = DateTime.Now.AddDays(365);
                foreach (var day in ultraCalendarInfo1.DaysOfWeek)
                {
                    if (day.LongDescriptionResolved.ToString() == System.DayOfWeek.Sunday.ToString() || day.LongDescriptionResolved.ToString() == System.DayOfWeek.Saturday.ToString())
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
        /// Set default Date of the calander
        /// </summary>
        /// <param name="dt"></param>
        public void SetExpiryDate(string dt)
        {
            try
            {
                DateTime dtValue;
                dtExpireTime.Value = "";
                if (DateTime.TryParse(dt, out dtValue))
                {
                    this.dtExpireTime.TextChanged -= DtExpireDate_TextChanged;
                    if (dtValue.Date >= DateTime.Now.Date)
                        dtExpireTime.Value = dtValue;
                    this.dtExpireTime.TextChanged += DtExpireDate_TextChanged;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
        }
        private void DtExpireDate_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                _currentOrder = null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }


        /// <summary>
        /// DtExpireDate TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtExpireDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtExpireTime.Value != null && !string.IsNullOrEmpty(dtExpireTime.Value.ToString()))
                {

                    SetExpiryDateInOrder(dtExpireTime.Value.ToString());
                    dtExpireTime.DroppedDown = false;
                    _currentOrder = null;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }

        }

        /// <summary>
        /// Returns Counter Parties valuelist for the row at given index
        /// </summary>
        public ValueList GetCounterParties(int index)
        {
            ValueList valueList = new ValueList();
            try
            {
                valueList = (ValueList)grdTrades.Rows[index].Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].ValueListResolved;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return valueList;
        }

        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            try
            {
                string folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                if (gridName == "grdTrades")
                {
                    exporter.Export(grdTrades, filePath);
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