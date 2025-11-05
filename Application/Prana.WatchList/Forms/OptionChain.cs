using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.WatchList.Classes;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Prana.WatchList.Forms
{
    public partial class OptionChain : Form, ILiveFeedCallback, INotifyPropertyChanged, IDisposable
    {
        #region Properties
        private int _watchListTabNumber;
        public int WatchListTabNumber
        {
            get
            {
                return _watchListTabNumber;
            }
            set
            {
                _watchListTabNumber = value;
            }
        }

        private string _symbol;
        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && _symbol != value && ChangeSelectedSymbol(_symbol, value))
                {
                    _symbol = value;
                    if (ultraSymbolTextEditor.Text != value)
                        ultraSymbolTextEditor.Text = value;

                    OnPropertyChanged();

                    EnableDisableControls(false);
                    GetSymbolSecMasterData(_symbol);

                }
            }
        }

        private double? _last;
        public double? Last
        {
            get
            {
                return _last;
            }
            set
            {
                _last = value;
                OnPropertyChanged();
            }
        }

        private double? _bid;
        public double? Bid
        {
            get
            {
                return _bid;
            }
            set
            {
                _bid = value;
                OnPropertyChanged();
            }
        }

        private double? _ask;
        public double? Ask
        {
            get
            {
                return _ask;
            }
            set
            {
                _ask = value;
                OnPropertyChanged();
            }
        }

        private double? _change;
        public double? Change
        {
            get
            {
                return _change;
            }
            set
            {
                _change = value;
                OnPropertyChanged();
            }
        }

        private double? _changePercentage;
        public double? ChangePercentage
        {
            get
            {
                return _changePercentage;
            }
            set
            {
                _changePercentage = value;
                OnPropertyChanged();
            }
        }

        private DuplexProxyBase<IPricingService> _pricingServiceProxy = null;
        private ISecurityMasterServices _securityMaster = null;
        #endregion

        #region Variables and Constants
        private const string Group_Call = "Call";
        private const string Group_Put = "Put";
        private const string CulturalFormatting = "#,##0";
        private const string CulturalDecimalFormatting = "#,##0.00";
        private const string Col_ExpirationDate = "ExpirationDate";
        private const string Col_StrikePrice = "StrikePrice";
        private const string Col_Call_Symbol = "Call_Symbol";
        private const string Col_Call_Select = "Call_Select";
        private const string Col_Call_Last = "Call_Last";
        private const string Col_Call_Change = "Call_Change";
        private const string Col_Call_Bid = "Call_Bid";
        private const string Col_Call_Ask = "Call_Ask";
        private const string Col_Call_Volume = "Call_Volume";
        private const string Col_Call_OpenInt = "Call_OpenInt";
        private const string Col_Put_Symbol = "Put_Symbol";
        private const string Col_Put_Select = "Put_Select";
        private const string Col_Put_Last = "Put_Last";
        private const string Col_Put_Change = "Put_Change";
        private const string Col_Put_Bid = "Put_Bid";
        private const string Col_Put_Ask = "Put_Ask";
        private const string Col_Put_Volume = "Put_Volume";
        private const string Col_Put_OpenInt = "Put_OpenInt";
        private int subscribedOptions;
        private HashSet<string> loadedOptions = new HashSet<string>();

        private readonly Color UPCOLOR = Color.Green;
        private readonly Color DOWNCOLOR = Color.Red;
        private readonly Color NOCHANGECOLOR = Color.Black;
        private int MinNumberOfStrikes;
        private int MaxNumberOfStrikes;
        private BindingList<OptionChainDataModel> optionChainData;
        private SecMasterBaseObj _symbolSecMasterBaseObj = null;
        #endregion

        #region Events

        /// <summary>
        /// Occurs when [send symbol to TT].
        /// </summary>
        public event EventHandler<EventArgs<string, string>> SendSymbolToTT;

        /// <summary>
        /// Occurs when [send symbols to MTT].
        /// </summary>
        public event EventHandler<EventArgs<OrderBindingList>> SendSymbolToMTT;

        /// <summary>
        /// Occurs when [Options added to WatchList].
        /// </summary>
        public event EventHandler<EventArgs<int, List<string>>> AddOptionsToWatchList;

        #endregion

        #region Delegates
        private delegate void SnapshotResponseReceived(SymbolData symbolData, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData);
        private delegate void OptionChainResponseReceived(string symbol, List<OptionStaticData> data);
        #endregion

        public OptionChain(int watchListTabNumber, string symbol, ISecurityMasterServices securityMaster)
        {
            InitializeComponent();
            this._securityMaster = securityMaster;
            this._watchListTabNumber = watchListTabNumber;
            this._symbol = symbol;
        }

        /// <summary>
        /// This method sets the theme and initialization for this form
        /// </summary>
        private async void OptionChain_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_OPTIONCHAIN);
                ultraOptionChainFormManager.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                ultraOptionChainFormManager.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, Text, CustomThemeHelper.UsedFont);
                this.Load -= OptionChain_Load;

                await CreatePricingServiceProxy().ConfigureAwait(false);

                CustomThemeHelper.SetThemePropertiesComponent(ultraOptionsGrid, CustomThemeHelper.THEME_STYLESETNAME_OPTIONCHAIN_GRID);
                CustomThemeHelper.SetThemePropertiesComponent(optionChainStatusStrip, CustomThemeHelper.THEME_STYLESETNAME_OPTIONCHAIN);

                optionChainData = new BindingList<OptionChainDataModel>();

                MinNumberOfStrikes = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIGKEY_OptionChainMinNumberOfStrikes));
                MaxNumberOfStrikes = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIGKEY_OptionChainMaxNumberOfStrikes));
                if (MinNumberOfStrikes < 1)
                    MinNumberOfStrikes = 1;
                if (MaxNumberOfStrikes < 2)
                    MaxNumberOfStrikes = 2;
                if (MaxNumberOfStrikes < MinNumberOfStrikes)
                    MaxNumberOfStrikes = MinNumberOfStrikes + 1;

                BindAndFillUI();
                GetSymbolSecMasterData(Symbol);

                ultraSymbolTextEditor.Focus();
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

        private bool ChangeSelectedSymbol(string oldSymbol, string newSymbol)
        {
            try
            {
                if (WatchListTabNumber != int.MinValue)
                {
                    DialogResult confirmationResult = new CustomMessageBox("Change Symbol!", "Do you want to change the symbol from " + oldSymbol + " to " + newSymbol + " ?", false, string.Empty, FormStartPosition.CenterParent, MessageBoxButtons.OKCancel).ShowDialog();
                    if (confirmationResult != DialogResult.OK)
                        return false;
                }

                UnsubscribeL1Data().ConfigureAwait(false);
                UnsubscribeOptionChainData().ConfigureAwait(false);

                BindGridData();
                Last = Bid = Ask = Change = ChangePercentage = null;
                return true;
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

        private async void BindAndFillUI()
        {
            try
            {
                await FillComboboxes();

                this.ultraNumberOfStrikesNumericEditor.Value = MinNumberOfStrikes;
                this.ultraUnderlyingSymbolBidValueLabel.Appearance.ForeColor = Color.Blue;
                this.ultraUnderlyingSymbolAskValueLabel.Appearance.ForeColor = Color.Red;

                BindDataToLabel(this.ultraUnderlyingSymbolLastValueLabel, "Last");
                BindDataToLabel(this.ultraUnderlyingSymbolBidValueLabel, "Bid");
                BindDataToLabel(this.ultraUnderlyingSymbolAskValueLabel, "Ask");
                BindDataToLabel(this.ultraUnderlyingSymbolChangeValueLabel, "Change");
                BindDataToLabel(this.ultraUnderlyingSymbolChangePercentageValueLabel, "ChangePercentage");

                BindEvents();

                if (!string.IsNullOrEmpty(Symbol))
                    ultraSymbolTextEditor.Text = Symbol;

                BindGridData();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void EnableDisableControls(bool isEnabled)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        EnableDisableControls(isEnabled);
                    }));
                    return;
                }

                if (ultraExpirationComboEditor.Enabled != isEnabled)
                    ultraExpirationComboEditor.Enabled = isEnabled;
                if (ultraCallsAndPutsComboEditor.Enabled != isEnabled)
                    ultraCallsAndPutsComboEditor.Enabled = isEnabled;
                if (ultraNumberOfStrikesNumericEditor.Enabled != isEnabled)
                    ultraNumberOfStrikesNumericEditor.Enabled = isEnabled;
                if (ultraRadioFiltersGroupBox.Enabled != isEnabled)
                    ultraRadioFiltersGroupBox.Enabled = isEnabled;
                if (ultraFetchDataButton.Enabled != isEnabled)
                    ultraFetchDataButton.Enabled = isEnabled;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private async System.Threading.Tasks.Task FillComboboxes()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                DateTime date, currentDate = DateTime.Now;

                for (date = currentDate; date <= currentDate.AddMonths(11); date = date.AddMonths(1))
                    ultraExpirationComboEditor.Items.Add(new Infragistics.Win.ValueListItem(date, date.ToString("MMMM yyyy")));
                ultraExpirationComboEditor.SelectedIndex = 0;
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

        private void BindEvents()
        {
            try
            {
                this.ultraUnderlyingSymbolExpandableGroupBox.ExpandedStateChanging += ultraUnderlyingSymbolExpandableGroupBox_ExpandedStateChanging;
                this.ultraSymbolTextEditor.Leave += ultraSymbolTextEditor_Leave;
                this.ultraNumberOfStrikesNumericEditor.ValueChanged += ultraEditor_ValueChanged;
                this.ultraStrikeRangeRadioButton.CheckedChanged += ultraStrikesRangeRadioButton_CheckedChanged;
                this.ultraPercentFromAtTheMoneyRadioButton.CheckedChanged += ultraPercentFromAtTheMoneyRadioButton_CheckedChanged;
                this.ultraUpperStrikeRangeNumericEditor.ValueChanged += ultraEditor_ValueChanged;
                this.ultraLowerStrikeRangeNumericEditor.ValueChanged += ultraEditor_ValueChanged;
                this.ultraPercentFromAtTheMoneyNumericEditor.ValueChanged += ultraEditor_ValueChanged;
                this.ultraCallsAndPutsComboEditor.Leave += ultraCallsAndPutsComboEditor_Leave;
                this.ultraExpirationComboEditor.Leave += ultraExpirationComboEditor_Leave;
                this.ultraSymbolTextEditor.KeyUp += ultraSymbolTextEditor_KeyUp;
                this.ultraOptionsGrid.InitializeRow += ultraOptionsGrid_InitializeRow;
                this.ultraOptionsGrid.MouseDown += ultraOptionsGrid_MouseDown;
                this.ultraOptionsGrid.DoubleClickCell += ultraOptionsGrid_DoubleClickCell;
                this.ultraGridContextMenuStrip.Opening += ultraGridContextMenuStrip_Opening;
                this.ultraFetchDataButton.Click += ultraFetchDataButton_Click;
                this.buyMenuItem.Click += buyMenuItem_Click;
                this.sellMenuItem.Click += sellMenuItem_Click;
                this.addOptionsToWatchListMenuItem.Click += addOptionsToWatchListMenuItem_Click;
                this.expandCollapseMenuItem.Click += expandCollapseMenuItem_Click;
                this.clearFilterMenuItem.Click += clearFilterMenuItem_Click;
                this.FormClosed += OptionChain_Closed;
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

        private void UnbindEvents()
        {
            try
            {
                if (_securityMaster != null)
                    this._securityMaster.SecMstrDataResponse -= _securityMaster_SecMstrDataResponse;
                this.ultraUnderlyingSymbolExpandableGroupBox.ExpandedStateChanging -= ultraUnderlyingSymbolExpandableGroupBox_ExpandedStateChanging;
                this.ultraSymbolTextEditor.Leave -= ultraSymbolTextEditor_Leave;
                this.ultraNumberOfStrikesNumericEditor.ValueChanged -= ultraEditor_ValueChanged;
                this.ultraStrikeRangeRadioButton.CheckedChanged -= ultraStrikesRangeRadioButton_CheckedChanged;
                this.ultraPercentFromAtTheMoneyRadioButton.CheckedChanged -= ultraPercentFromAtTheMoneyRadioButton_CheckedChanged;
                this.ultraUpperStrikeRangeNumericEditor.ValueChanged -= ultraEditor_ValueChanged;
                this.ultraLowerStrikeRangeNumericEditor.ValueChanged += ultraEditor_ValueChanged;
                this.ultraPercentFromAtTheMoneyNumericEditor.ValueChanged -= ultraEditor_ValueChanged;
                this.ultraCallsAndPutsComboEditor.Leave -= ultraCallsAndPutsComboEditor_Leave;
                this.ultraExpirationComboEditor.Leave += ultraExpirationComboEditor_Leave;
                this.ultraSymbolTextEditor.KeyUp -= ultraSymbolTextEditor_KeyUp;
                this.ultraOptionsGrid.InitializeRow -= ultraOptionsGrid_InitializeRow;
                this.ultraOptionsGrid.MouseDown -= ultraOptionsGrid_MouseDown;
                this.ultraOptionsGrid.DoubleClickCell -= ultraOptionsGrid_DoubleClickCell;
                this.ultraGridContextMenuStrip.Opening -= ultraGridContextMenuStrip_Opening;
                this.ultraFetchDataButton.Click -= ultraFetchDataButton_Click;
                this.buyMenuItem.Click -= buyMenuItem_Click;
                this.sellMenuItem.Click -= sellMenuItem_Click;
                this.addOptionsToWatchListMenuItem.Click -= addOptionsToWatchListMenuItem_Click;
                this.expandCollapseMenuItem.Click -= expandCollapseMenuItem_Click;
                this.clearFilterMenuItem.Click -= clearFilterMenuItem_Click;
                this.FormClosed -= OptionChain_Closed;
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

        private void GetSymbolSecMasterData(string symbol)
        {
            try
            {
                if (_securityMaster != null && !string.IsNullOrEmpty(symbol))
                {
                    SecMasterRequestObj reqObj = new SecMasterRequestObj();
                    reqObj.AddData(symbol, SymbologyHelper.DefaultSymbology);
                    reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    reqObj.HashCode = GetHashCode();
                    _securityMaster.SecMstrDataResponse += _securityMaster_SecMstrDataResponse;
                    _securityMaster.SendRequest(reqObj);
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

        private async void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (e != null && e.Value != null)
                {
                    if (_symbol.Equals(e.Value.SymbologyMapping[(int)SymbologyHelper.DefaultSymbology]))
                    {
                        this._securityMaster.SecMstrDataResponse -= _securityMaster_SecMstrDataResponse;
                        ChangeFormTitle(e.Value.LongName);
                        _symbolSecMasterBaseObj = e.Value;

                        EnableDisableControls(true);
                        this.BeginInvoke(new MethodInvoker(delegate
                        {
                            ultraExpirationComboEditor.Focus();
                        }));

                        await SubscribeL1Data().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private async System.Threading.Tasks.Task SubscribeL1Data()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                if (_pricingServiceProxy != null)
                {
                    _pricingServiceProxy.InnerChannel.RequestSymbol_TTandPTT(_symbolSecMasterBaseObj.TickerSymbol, null, false);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        private async System.Threading.Tasks.Task UnsubscribeL1Data()

        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                if (_pricingServiceProxy != null && Symbol != null)
                    _pricingServiceProxy.InnerChannel.RemoveSymbol_TTandPTT(_symbolSecMasterBaseObj.TickerSymbol);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        private async System.Threading.Tasks.Task SubscribeOptionChainData()
        {
            try
            {
                if (_pricingServiceProxy != null)
                {
                    OptionChainFilter optionChainFilter = new OptionChainFilter();
                    optionChainFilter.ExpirationDate = (DateTime)ultraExpirationComboEditor.Value;
                    optionChainFilter.OptionTypeFilter = (OptionTypeFilter)ultraCallsAndPutsComboEditor.Value;
                    optionChainFilter.MaxNumberOfStrikes = Convert.ToInt32(ultraNumberOfStrikesNumericEditor.Value);
                    if (ultraPercentFromAtTheMoneyRadioButton.Checked)
                        optionChainFilter.StrikeTolerancePercentage = Convert.ToInt32(ultraPercentFromAtTheMoneyNumericEditor.Value);
                    else
                    {
                        optionChainFilter.LowerStrike = Convert.ToDouble(ultraLowerStrikeRangeNumericEditor.Value);
                        optionChainFilter.UpperStrike = Convert.ToDouble(ultraUpperStrikeRangeNumericEditor.Value);
                    }
                    if (Last != null)
                        optionChainFilter.UnderlyingSymbolLastTradedPrice = (double)Last;

                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Subscribing OptionChain for Symbol: {0}", Symbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                    await _pricingServiceProxy.InnerChannel.SubscribeStaticOptionChain(_symbolSecMasterBaseObj.TickerSymbol, optionChainFilter);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        private async System.Threading.Tasks.Task UnsubscribeOptionChainData()
        {
            try
            {
                if (_pricingServiceProxy != null && _symbolSecMasterBaseObj != null)
                    await _pricingServiceProxy.InnerChannel.UnsubscribeStaticOptionChain(_symbolSecMasterBaseObj.TickerSymbol);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        private async System.Threading.Tasks.Task SendOrder(Constants.TradeType orderType)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                IEnumerable<OptionChainDataModel> selectedCallOptions = optionChainData.Where(x => x.Call_Select == true);
                IEnumerable<OptionChainDataModel> selectedPutOptions = optionChainData.Where(x => x.Put_Select == true);

                if (selectedCallOptions.Count() + selectedPutOptions.Count() > 1)
                {
                    OrderBindingList orderList = new OrderBindingList();

                    foreach (OptionChainDataModel optionModel in selectedCallOptions)
                    {
                        OrderSingle order = CreateOrderSingle(optionModel.Call_Symbol, orderType == Constants.TradeType.BuyToOpen, optionModel);
                        orderList.Add(order);
                    }
                    foreach (OptionChainDataModel optionModel in selectedPutOptions)
                    {
                        OrderSingle order = CreateOrderSingle(optionModel.Put_Symbol, orderType == Constants.TradeType.BuyToOpen, optionModel);
                        orderList.Add(order);
                    }

                    if (SendSymbolToMTT != null)
                        SendSymbolToMTT(null, new EventArgs<OrderBindingList>(orderList));
                }
                else
                {
                    if (selectedCallOptions.Count() == 1)
                        SendSymbolToTradingTicket(selectedCallOptions.First(), true, orderType);
                    else
                        SendSymbolToTradingTicket(selectedPutOptions.First(), false, orderType);
                }

                SetToolStripStatusMessage("Option(s) sent successfully.");
                ClearSelectedOptions();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #region UI Methods
        private async void ultraOptionsGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                UltraWinGridUtils.EnableFixedFilterRow(e);

                UltraGridBand band = e.Layout.Bands[0];
                band.Layout.Override.FilterRowPrompt = string.Empty;
                band.Layout.Override.FilterRowAppearance.BackColor = Color.Transparent;
                band.Layout.Override.RowAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

                if (band.Groups.Count == 0)
                {
                    UltraGridGroup callGroup = band.Groups.Add(Group_Call);
                    callGroup.Header.Caption = Group_Call;
                    callGroup.Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                    callGroup.Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                    callGroup.Header.Appearance.FontData.SizeInPoints = 10;
                    UltraGridGroup noGroup = band.Groups.Add("noGroup");
                    noGroup.Header.Caption = "";
                    UltraGridGroup putGroup = band.Groups.Add(Group_Put);
                    putGroup.Header.Caption = Group_Put;
                    putGroup.Header.Appearance.FontData.SizeInPoints = 10;
                    putGroup.Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                    putGroup.Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

                    ColumnsCollection columns = band.Columns;

                    foreach (UltraGridColumn column in columns)
                    {
                        column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                        column.Width = 82;

                        switch (column.Header.Caption)
                        {
                            case Col_ExpirationDate:
                                column.Group = noGroup;
                                column.Header.Caption = "Expiration Date";
                                band.SortedColumns.Add("ExpirationDate", false, true);
                                band.SortedColumns["ExpirationDate"].GroupByMode = GroupByMode.Text;
                                band.SortedColumns["ExpirationDate"].Format = "MMMM dd, yyyy";
                                break;
                            case Col_StrikePrice:
                                column.Group = noGroup;
                                column.Header.Caption = "Strike Price";
                                column.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                                column.Format = CulturalDecimalFormatting;
                                column.CellAppearance.BackColor = Color.FromArgb(88, 88, 90);
                                column.CellAppearance.ForeColor = Color.White;
                                column.SortIndicator = SortIndicator.Ascending;
                                column.Width = 110;
                                break;
                            case Col_Call_Select:
                                column.Group = callGroup;
                                column.Header.Caption = "";
                                column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                                column.Width = 25;
                                column.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Never;
                                column.Header.VisiblePosition = 0;
                                column.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                                column.CellClickAction = CellClickAction.Edit;
                                column.FilterOperatorLocation = FilterOperatorLocation.Hidden;
                                column.FilterClearButtonVisible = Infragistics.Win.DefaultableBoolean.False;
                                break;
                            case Col_Call_Last:
                                column.Group = callGroup;
                                column.Header.Caption = "Last";
                                column.Format = CulturalDecimalFormatting;
                                break;
                            case Col_Call_Change:
                                column.Group = callGroup;
                                column.Header.Caption = "Change";
                                column.Format = CulturalDecimalFormatting;
                                break;
                            case Col_Call_Bid:
                                column.Group = callGroup;
                                column.Header.Caption = "Bid";
                                column.Format = CulturalDecimalFormatting;
                                column.CellAppearance.ForeColor = Color.Blue;
                                break;
                            case Col_Call_Ask:
                                column.Group = callGroup;
                                column.Header.Caption = "Ask";
                                column.Format = CulturalDecimalFormatting;
                                column.CellAppearance.ForeColor = Color.Red;
                                break;
                            case Col_Call_Volume:
                                column.Group = callGroup;
                                column.Header.Caption = "Volume";
                                column.Format = CulturalFormatting;
                                break;
                            case Col_Call_OpenInt:
                                column.Group = callGroup;
                                column.Header.Caption = "Open Int.";
                                column.Format = CulturalFormatting;
                                break;
                            case Col_Put_Select:
                                column.Group = putGroup;
                                column.Header.Caption = "";
                                column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                                column.Width = 25;
                                column.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Never;
                                column.Header.VisiblePosition = 0;
                                column.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                                column.CellClickAction = CellClickAction.Edit;
                                column.FilterOperatorLocation = FilterOperatorLocation.Hidden;
                                column.FilterClearButtonVisible = Infragistics.Win.DefaultableBoolean.False;
                                break;
                            case Col_Put_Last:
                                column.Group = putGroup;
                                column.Header.Caption = "Last";
                                column.Format = CulturalDecimalFormatting;
                                column.Width = 82;
                                break;
                            case Col_Put_Change:
                                column.Group = putGroup;
                                column.Header.Caption = "Change";
                                column.Format = CulturalDecimalFormatting;
                                break;
                            case Col_Put_Bid:
                                column.Group = putGroup;
                                column.Header.Caption = "Bid";
                                column.Format = CulturalDecimalFormatting;
                                column.CellAppearance.ForeColor = Color.Blue;
                                break;
                            case Col_Put_Ask:
                                column.Group = putGroup;
                                column.Header.Caption = "Ask";
                                column.Format = CulturalDecimalFormatting;
                                column.CellAppearance.ForeColor = Color.Red;
                                break;
                            case Col_Put_Volume:
                                column.Group = putGroup;
                                column.Header.Caption = "Volume";
                                column.Format = CulturalFormatting;
                                break;
                            case Col_Put_OpenInt:
                                column.Group = putGroup;
                                column.Header.Caption = "Open Int.";
                                column.Format = CulturalFormatting;
                                break;
                        }
                    }

                    ultraOptionsGrid.Rows.FilterRow.Cells[Col_StrikePrice].Appearance.BackColor = Color.Transparent;
                    ultraOptionsGrid.Rows.FilterRow.Cells[Col_StrikePrice].Appearance.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private async void ultraOptionsGrid_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                e.Cancel = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ultraGridContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                bool shouldCancelOpening = true;

                ultraGridContextMenuStrip.Items[0].Visible = false;
                ultraGridContextMenuStrip.Items[1].Visible = false;
                ultraGridContextMenuStrip.Items[2].Visible = false;
                ultraGridContextMenuStrip.Items[4].Visible = false;

                if (optionChainData.FirstOrDefault(x => x.Call_Select == true || x.Put_Select == true) != null)
                {
                    ultraGridContextMenuStrip.Items[0].Visible = true;
                    ultraGridContextMenuStrip.Items[1].Visible = true;
                    if (WatchListTabNumber != int.MinValue)
                        ultraGridContextMenuStrip.Items[2].Visible = true;

                    shouldCancelOpening = false;
                }

                if (ultraGridContextMenuStrip.Items[3].Available)
                    shouldCancelOpening = false;

                if (this.ultraOptionsGrid.DisplayLayout.Bands[0].ColumnFilters.Cast<ColumnFilter>().FirstOrDefault(x => x.FilterConditions.Count > 0) != null)
                {
                    ultraGridContextMenuStrip.Items[4].Visible = true;
                    shouldCancelOpening = false;
                }

                if (shouldCancelOpening)
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private async void ultraOptionsGrid_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                OptionChainDataModel data = e.Row.ListObject as OptionChainDataModel;

                ChangeTickColor(data.Call_Tick, e.Row.Cells[Col_Call_Last].Appearance);
                ChangeTickColor(data.Call_Tick, e.Row.Cells[Col_Call_Change].Appearance);
                ChangeTickColor(data.Put_Tick, e.Row.Cells[Col_Put_Last].Appearance);
                ChangeTickColor(data.Put_Tick, e.Row.Cells[Col_Put_Change].Appearance);

                if (string.IsNullOrEmpty(e.Row.Cells[Col_Call_Symbol].Value.ToString()))
                    ShowHideGridCell(e.Row.Cells[Col_Call_Select], true);
                if (string.IsNullOrEmpty(e.Row.Cells[Col_Put_Symbol].Value.ToString()))
                    ShowHideGridCell(e.Row.Cells[Col_Put_Select], true);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ultraOptionsGrid_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    System.Drawing.Point mousePoint = new System.Drawing.Point(e.X, e.Y);
                    Infragistics.Win.UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    if (element == null)
                    {
                        ultraOptionsGrid.ActiveRow = null;
                    }

                    if (element.UIRoleResolved.Name == "GridGroupByRow")
                    {
                        UltraGridGroupByRow groupByRow = element.GetContext(typeof(UltraGridGroupByRow)) as UltraGridGroupByRow;
                        if (groupByRow != null)
                            groupByRow.Activate();

                        if (groupByRow.Expanded)
                            ultraGridContextMenuStrip.Items[3].Text = "Collapse";
                        else
                            ultraGridContextMenuStrip.Items[3].Text = "Expand";

                        ultraGridContextMenuStrip.Items[3].Available = true;
                    }
                    else if (element.UIRoleResolved.Name == "GridCell" || element.UIRoleResolved.Name == "Button")
                    {
                        UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                        if (cell != null && cell.Row.ParentRow != null)
                        {
                            cell.Row.Activate();
                            cell.Row.Selected = true;

                            ultraGridContextMenuStrip.Items[3].Text = "Collapse";
                            ultraGridContextMenuStrip.Items[3].Available = true;
                        }
                        else
                            ultraGridContextMenuStrip.Items[3].Available = false;
                    }
                    else
                        ultraGridContextMenuStrip.Items[3].Available = false;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ultraOptionsGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            try
            {
                if (e.Cell.Row.ListObject != null)
                {
                    switch (e.Cell.Column.Key)
                    {
                        case Col_Call_Last:
                            SendSymbolToTradingTicket((OptionChainDataModel)e.Cell.Row.ListObject, true, Constants.TradeType.None);
                            break;
                        case Col_Call_Bid:
                            SendSymbolToTradingTicket((OptionChainDataModel)e.Cell.Row.ListObject, true, Constants.TradeType.SellToOpen);
                            break;
                        case Col_Call_Ask:
                            SendSymbolToTradingTicket((OptionChainDataModel)e.Cell.Row.ListObject, true, Constants.TradeType.BuyToOpen);
                            break;
                        case Col_Put_Last:
                            SendSymbolToTradingTicket((OptionChainDataModel)e.Cell.Row.ListObject, false, Constants.TradeType.None);
                            break;
                        case Col_Put_Bid:
                            SendSymbolToTradingTicket((OptionChainDataModel)e.Cell.Row.ListObject, false, Constants.TradeType.SellToOpen);
                            break;
                        case Col_Put_Ask:
                            SendSymbolToTradingTicket((OptionChainDataModel)e.Cell.Row.ListObject, false, Constants.TradeType.BuyToOpen);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private async void buyMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await SendOrder(Constants.TradeType.BuyToOpen);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private async void sellMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await SendOrder(Constants.TradeType.SellToOpen);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private async void addOptionsToWatchListMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                if (AddOptionsToWatchList != null)
                {
                    List<string> selectedOptions = new List<string>();

                    IEnumerable<OptionChainDataModel> selectedCallOptions = optionChainData.Where(x => x.Call_Select == true);
                    foreach (OptionChainDataModel option in selectedCallOptions)
                        selectedOptions.Add(option.Call_Symbol);

                    IEnumerable<OptionChainDataModel> selectedPutOptions = optionChainData.Where(x => x.Put_Select == true);
                    foreach (OptionChainDataModel option in selectedPutOptions)
                        selectedOptions.Add(option.Put_Symbol);

                    AddOptionsToWatchList(null, new EventArgs<int, List<string>>(WatchListTabNumber, selectedOptions));

                    SetToolStripStatusMessage("Option(s) added to watchlist.");
                    ClearSelectedOptions();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ClearSelectedOptions()
        {
            try
            {
                IEnumerable<OptionChainDataModel> selectedOptionChainDataModels = optionChainData.Where(x => x.Call_Select == true || x.Put_Select == true);
                foreach (OptionChainDataModel option in selectedOptionChainDataModels)
                    option.Call_Select = option.Put_Select = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private async void expandCollapseMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                if (ultraOptionsGrid.ActiveRow is UltraGridGroupByRow)
                {
                    if (!ultraOptionsGrid.ActiveRow.Expanded)
                        ultraOptionsGrid.ActiveRow.ExpandAll();
                    else
                        ultraOptionsGrid.ActiveRow.CollapseAll();
                }
                else
                {
                    if (!ultraOptionsGrid.ActiveRow.ParentRow.Expanded)
                        ultraOptionsGrid.ActiveRow.ParentRow.ExpandAll();
                    else
                        ultraOptionsGrid.ActiveRow.ParentRow.CollapseAll();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private async void clearFilterMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                this.ultraOptionsGrid.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private async void ultraFetchDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateUserInputs() && CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                {
                    if (ultraPercentFromAtTheMoneyRadioButton.Checked && (Last == null || Last == 0))
                    {
                        SetToolStripStatusMessage("Can't fetch options as last price is not available.");
                        return;
                    }

                    EnableDisableFetchButton(false);
                    SetToolStripStatusMessage("Loading options ...");
                    BindGridData();

                    await UnsubscribeOptionChainData();
                    await SubscribeOptionChainData();
                }
                else if (!CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                {
                    SetToolStripStatusMessage(ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ultraPercentFromAtTheMoneyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ultraPercentFromAtTheMoneyNumericEditor.Enabled = !ultraPercentFromAtTheMoneyNumericEditor.Enabled;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ultraStrikesRangeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ultraLowerStrikeRangeNumericEditor.Enabled = !ultraLowerStrikeRangeNumericEditor.Enabled;
                ultraUpperStrikeRangeNumericEditor.Enabled = !ultraUpperStrikeRangeNumericEditor.Enabled;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void SetToolStripStatusMessage(string text, Color textColor = default(Color))
        {
            try
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        SetToolStripStatusMessage(text);
                    }));
                    return;
                }

                if (!string.IsNullOrEmpty(text))
                    optionChainToolStripStatusLabel.Text = "[" + DateTime.Now + "] " + text;
                else
                    optionChainToolStripStatusLabel.Text = string.Empty;

                optionChainToolStripStatusLabel.ForeColor = textColor;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ultraEditor_ValueChanged(object sender, EventArgs e)
        {
            ValidateUserInputs();
        }

        private bool ValidateUserInputs()
        {
            try
            {
                this.optionChainFiltersErrorProvider.Clear();

                if (ultraNumberOfStrikesNumericEditor.Value < MinNumberOfStrikes || ultraNumberOfStrikesNumericEditor.Value > MaxNumberOfStrikes)
                {
                    this.optionChainFiltersErrorProvider.SetError(this.ultraNumberOfStrikesNumericEditor, string.Format("# of strikes should be in range of {0}-{1}.", MinNumberOfStrikes, MaxNumberOfStrikes));
                    this.ultraNumberOfStrikesNumericEditor.Focus();
                    return false;
                }
                else if (ultraPercentFromAtTheMoneyRadioButton.Checked && (ultraPercentFromAtTheMoneyNumericEditor.Value < 1 || ultraPercentFromAtTheMoneyNumericEditor.Value > 100))
                {
                    this.optionChainFiltersErrorProvider.SetError(this.ultraPercentFromAtTheMoneyNumericEditor, "Value should be in range of 0-100%.");
                    this.ultraPercentFromAtTheMoneyNumericEditor.Focus();
                    return false;
                }
                else if (ultraStrikeRangeRadioButton.Checked)
                {
                    if (ultraLowerStrikeRangeNumericEditor.Value < 0 || ultraLowerStrikeRangeNumericEditor.Value > 99999)
                    {
                        this.optionChainFiltersErrorProvider.SetError(this.ultraLowerStrikeRangeNumericEditor, "Value should be greater than 0.");
                        this.ultraLowerStrikeRangeNumericEditor.Focus();
                        return false;
                    }
                    else if (ultraUpperStrikeRangeNumericEditor.Value < 0 || ultraUpperStrikeRangeNumericEditor.Value > 99999)
                    {
                        this.optionChainFiltersErrorProvider.SetError(this.ultraUpperStrikeRangeNumericEditor, "Value should be greater than 0.");
                        this.ultraUpperStrikeRangeNumericEditor.Focus();
                        return false;
                    }

                    if (ultraLowerStrikeRangeNumericEditor.Value >= ultraUpperStrikeRangeNumericEditor.Value)
                    {
                        if (ultraLowerStrikeRangeNumericEditor.Focused)
                            this.optionChainFiltersErrorProvider.SetError(this.ultraLowerStrikeRangeNumericEditor, "This value should be lower than the upper strike.");
                        else
                            this.optionChainFiltersErrorProvider.SetError(this.ultraUpperStrikeRangeNumericEditor, "This value should be higher than the lower strike.");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return false;
        }

        private void ultraSymbolTextEditor_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (Symbol != ultraSymbolTextEditor.Text)
                    EnableDisableControls(false);
                else
                    EnableDisableControls(true);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ultraSymbolTextEditor_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Symbol != ultraSymbolTextEditor.Text)
                {
                    WatchListTabNumber = int.MinValue;
                    Symbol = ultraSymbolTextEditor.Text;

                    SetToolStripStatusMessage(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ultraExpirationComboEditor_Leave(object sender, EventArgs e)
        {
            try
            {
                foreach (Infragistics.Win.ValueListItem item in ultraExpirationComboEditor.ValueList.ValueListItems)
                    if (item.DisplayText == ultraExpirationComboEditor.Text)
                        return;

                ultraExpirationComboEditor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ultraCallsAndPutsComboEditor_Leave(object sender, EventArgs e)
        {
            try
            {
                foreach (Infragistics.Win.ValueListItem item in ultraCallsAndPutsComboEditor.ValueList.ValueListItems)
                    if (item.DisplayText == ultraCallsAndPutsComboEditor.Text)
                        return;

                ultraCallsAndPutsComboEditor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ultraUnderlyingSymbolExpandableGroupBox_ExpandedStateChanging(object sender, CancelEventArgs e)
        {
            if (ultraUnderlyingSymbolExpandableGroupBox.Expanded)
            {
                ultraOptionsGrid.Height += 60;
                ultraOptionsGrid.Location = new Point(6, 95);
            }
            else
            {
                ultraOptionsGrid.Location = new Point(6, 155);
                ultraOptionsGrid.Height -= 60;
            }
        }

        private async void OptionChain_Closed(object sender, FormClosedEventArgs e)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                try
                {
                    UnbindEvents();

                    await UnsubscribeL1Data();
                    await UnsubscribeOptionChainData();

                    _pricingServiceProxy.Dispose();
                }
                catch (Exception ex)
                {
                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                }
            }).ConfigureAwait(false);
        }
        #endregion

        #region Helping Methods
        private void BindGridData(BindingList<OptionChainDataModel> optionChainBindingList = null)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        BindGridData(optionChainBindingList);
                    }));
                    return;
                }

                if (optionChainBindingList == null)
                {
                    lock (optionChainData)
                        optionChainData.Clear();
                    lock (loadedOptions)
                        loadedOptions.Clear();

                    ultraOptionsGrid.DataSource = new BindingList<OptionChainDataModel>();
                }
                else
                    ultraOptionsGrid.DataSource = optionChainBindingList;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ChangeFormTitle(string symbolDescription)
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

                if (!string.IsNullOrEmpty(symbolDescription) && this.Text != symbolDescription + " - Option Chain")
                    this.Text = symbolDescription + " - Option Chain";
                else if (string.IsNullOrEmpty(symbolDescription))
                    this.Text = "Option Chain";
                else
                    return;

                ultraOptionChainFormManager.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, Text, CustomThemeHelper.UsedFont);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void BindDataToLabel(UltraLabel labelControl, string propertyName)
        {
            try
            {
                Binding binding = new Binding("Text", this, propertyName);
                binding.FormatString = CulturalDecimalFormatting;
                binding.FormattingEnabled = true;
                binding.NullValue = string.Empty;
                labelControl.DataBindings.Add(binding);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ChangeTickColor(string tick, Infragistics.Win.AppearanceBase appearance)
        {
            try
            {
                switch (tick)
                {
                    case "UP_TICK":
                    case "UP_UNCHANGED":
                        if (appearance.ForeColor != UPCOLOR)
                            appearance.ForeColor = UPCOLOR;
                        break;
                    case "DOWN_TICK":
                    case "DOWN_UNCHANGED":
                        if (appearance.ForeColor != DOWNCOLOR)
                            appearance.ForeColor = DOWNCOLOR;
                        break;
                    case "NO_TICK":
                        if (appearance.ForeColor != NOCHANGECOLOR)
                            appearance.ForeColor = NOCHANGECOLOR;
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ShowHideGridCell(UltraGridCell cell, bool isHidden)
        {
            try
            {
                if (cell.Hidden != isHidden)
                    cell.Hidden = isHidden;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void EnableDisableFetchButton(bool isEnabled)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        EnableDisableFetchButton(isEnabled);
                    }));
                    return;
                }

                if (ultraFetchDataButton.Enabled != isEnabled)
                    ultraFetchDataButton.Enabled = isEnabled;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private OrderSingle CreateOrderSingle(string symbol, bool isBuyTrade, OptionChainDataModel optionChainData)
        {
            var order = new OrderSingle();
            try
            {
                if (!string.IsNullOrEmpty(symbol) && optionChainData != null)
                {
                    TradingTicketUIPrefs userTradingTicketUiPrefs = TradingTktPrefs.UserTradingTicketUiPrefs;
                    TradingTicketUIPrefs companyTradingTicketUiPrefs = TradingTktPrefs.CompanyTradingTicketUiPrefs;
                    CounterPartyWiseCommissionBasis CommisionUserTTUiPrefs = TradingTktPrefs.CpwiseCommissionBasis;
                    order.AUECID = _symbolSecMasterBaseObj.AUECID;
                    int assetID = Convert.ToInt32(CachedDataManager.GetInstance.GetAssetIdByAUECId(order.AUECID));
                    int underlyingID = Convert.ToInt32(CachedDataManager.GetInstance.GetUnderlyingID(order.AUECID));

                    Infragistics.Win.ValueList userBrokerList = TTHelperManager.GetInstance().GetCounterparties(assetID, underlyingID, order.AUECID);
                    var isInUserBrokerList = companyTradingTicketUiPrefs.Broker.HasValue ? userBrokerList.ValueListItems.Cast<Infragistics.Win.ValueListItem>().Any(valueitem => valueitem.DataValue.Equals(companyTradingTicketUiPrefs.Broker.Value)) : false;

                    int counterPartyID = 0;

                    if (symbol == optionChainData.Call_Symbol)
                    {
                        order.BloombergSymbol = optionChainData.Call_BloombergSymbol;
                        order.FactSetSymbol = optionChainData.Call_FactSetSymbol;
                        order.ActivSymbol = optionChainData.Call_ActivSymbol;
                    }
                    else
                    {
                        order.BloombergSymbol = optionChainData.Put_BloombergSymbol;
                        order.FactSetSymbol = optionChainData.Put_FactSetSymbol;
                        order.ActivSymbol = optionChainData.Put_ActivSymbol;
                    }

                    if (userTradingTicketUiPrefs != null && companyTradingTicketUiPrefs != null)
                    {
                        int accountID;
                        if (int.TryParse(userTradingTicketUiPrefs.Account.ToString(), out accountID))
                        {
                            order.Account = accountID.ToString();
                            order.Level1ID = accountID;
                        }
                        else if (int.TryParse(companyTradingTicketUiPrefs.Account.ToString(), out accountID))
                        {
                            order.Account = accountID.ToString();
                            order.Level1ID = accountID;
                        }
                        counterPartyID = userTradingTicketUiPrefs.Broker.HasValue ? userTradingTicketUiPrefs.Broker.Value : ((isInUserBrokerList) ? companyTradingTicketUiPrefs.Broker.Value : int.MinValue);

                        if (companyTradingTicketUiPrefs != null && userTradingTicketUiPrefs.Broker.HasValue && CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue.ContainsKey(userTradingTicketUiPrefs.Broker.Value))
                        {
                            order.VenueID = CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue[userTradingTicketUiPrefs.Broker.Value];
                        }
                        // Venue shouldn't be empty if Broker is assigned and a corresponding Broker-Venue exists
                        else if (counterPartyID != 0 && CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue.ContainsKey(counterPartyID))
                        {
                            order.VenueID = CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue[counterPartyID];
                        }
                        else if (companyTradingTicketUiPrefs.Venue.HasValue && isInUserBrokerList)
                        {
                            order.VenueID = companyTradingTicketUiPrefs.Venue.Value;
                        }
                        else
                        {
                            order.VenueID = 0;
                        }

                        if (userTradingTicketUiPrefs.TimeInForce.HasValue)
                        {
                            order.TIF = TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(userTradingTicketUiPrefs.TimeInForce.Value.ToString());
                        }
                        else if (companyTradingTicketUiPrefs.TimeInForce.HasValue)
                        {
                            order.TIF = TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(companyTradingTicketUiPrefs.TimeInForce.Value.ToString());
                        }

                        if (userTradingTicketUiPrefs.HandlingInstruction.HasValue)
                        {
                            order.HandlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionValueBasedOnId(userTradingTicketUiPrefs.HandlingInstruction.Value.ToString());
                        }
                        else if (companyTradingTicketUiPrefs.HandlingInstruction.HasValue)
                        {
                            order.HandlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionValueBasedOnId(companyTradingTicketUiPrefs.HandlingInstruction.Value.ToString());
                        }

                        if (userTradingTicketUiPrefs.TradingAccount.HasValue)
                        {
                            order.TradingAccountID = userTradingTicketUiPrefs.TradingAccount.Value;
                        }
                        else if (companyTradingTicketUiPrefs.TradingAccount.HasValue)
                        {
                            order.TradingAccountID = companyTradingTicketUiPrefs.TradingAccount.Value;
                        }

                        if (userTradingTicketUiPrefs.Broker.HasValue)
                        {
                            if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(Convert.ToInt32(userTradingTicketUiPrefs.Broker)))
                            {
                                order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(userTradingTicketUiPrefs.Broker)].ToString());
                            }
                            else
                            {
                                if (userTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                                {
                                    order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(userTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                                }
                                else if (companyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                                {
                                    order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(companyTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                                }

                            }
                        }
                        else if (companyTradingTicketUiPrefs.Broker.HasValue)
                        {
                            if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(Convert.ToInt32(userTradingTicketUiPrefs.Broker)))
                            {
                                order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(userTradingTicketUiPrefs.Broker)].ToString());
                            }
                            else
                            {
                                if (userTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                                {
                                    order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(userTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                                }
                                else if (companyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                                {
                                    order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(companyTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                                }
                            }
                        }
                        else
                        {
                            if (userTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                            {
                                order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(userTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                            }
                            else if (companyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                            {
                                order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(companyTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                            }
                        }

                        if (userTradingTicketUiPrefs.Strategy.HasValue)
                        {
                            order.Level2ID = int.Parse(userTradingTicketUiPrefs.Strategy.ToString());
                            order.Strategy = userTradingTicketUiPrefs.Strategy.ToString();
                        }
                        else if (companyTradingTicketUiPrefs.Strategy.HasValue)
                        {
                            order.Level2ID = int.Parse(companyTradingTicketUiPrefs.Strategy.ToString());
                            order.Strategy = companyTradingTicketUiPrefs.Strategy.ToString();
                        }

                        if (userTradingTicketUiPrefs.OrderType.HasValue)
                        {
                            order.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValueBasedOnID(userTradingTicketUiPrefs.OrderType.Value.ToString());
                        }
                        else if (companyTradingTicketUiPrefs.OrderType.HasValue)
                        {
                            order.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValueBasedOnID(companyTradingTicketUiPrefs.OrderType.Value.ToString());
                        }

                        if (userTradingTicketUiPrefs.IsSettlementCurrencyBase.HasValue)
                        {
                            if (userTradingTicketUiPrefs.IsSettlementCurrencyBase.Value)
                            {
                                order.SettlementCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                            }
                        }
                        else if (companyTradingTicketUiPrefs.IsSettlementCurrencyBase.HasValue)
                        {
                            if (companyTradingTicketUiPrefs.IsSettlementCurrencyBase.Value)
                            {
                                order.SettlementCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                            }
                        }
                    }
                    order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(counterPartyID);
                    order.Venue = CachedDataManager.GetInstance.GetVenueText(order.VenueID);
                    order.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(order.TradingAccountID);
                    order.CounterPartyID = counterPartyID;
                    order.Price = 0;
                    order.Quantity = 0;
                    order.ShortRebate = 0;
                    order.Symbol = symbol;
                    order.ContractMultiplier = _symbolSecMasterBaseObj.Multiplier;
                    if (order.TransactionSource == TransactionSource.None)
                    {
                        order.TransactionSource = TransactionSource.TradingTicket;
                        order.TransactionSourceTag = (int)TransactionSource.TradingTicket;
                    }

                    if ((AssetCategory)_symbolSecMasterBaseObj.AssetID == AssetCategory.Equity || (AssetCategory)_symbolSecMasterBaseObj.AssetID == AssetCategory.Indices)
                    {
                        order.AssetID = (int)AssetCategory.EquityOption;
                    }
                    else if ((AssetCategory)_symbolSecMasterBaseObj.AssetID == AssetCategory.Future)
                    {
                        order.AssetID = (int)AssetCategory.FutureOption;
                    }


                    int exchangeID = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(order.AUECID);
                    int currencyID = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(order.AUECID);
                    string orderSideId = isBuyTrade ? FIXConstants.SIDE_Buy_Open : FIXConstants.SIDE_Sell_Open;
                    order.OrderSideTagValue = orderSideId;
                    order.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(orderSideId);
                    order.UnderlyingID = underlyingID;
                    order.ExchangeID = exchangeID;
                    order.CurrencyID = currencyID;
                    order.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(currencyID);
                    switch (order.CurrencyName)
                    {
                        case "EUR":
                        case "GBP":
                        case "NZD":
                        case "AUD":
                            order.FXConversionMethodOperator = "M";
                            break;

                        default:
                            order.FXConversionMethodOperator = "D";
                            break;
                    }
                    if (order.CurrencyID == CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                        order.FXConversionMethodOperator = "M";
                    if (optionChainData != null && optionChainData.ExpirationDate != DateTimeConstants.MinValue)
                    {
                        order.ExpirationDate = optionChainData.ExpirationDate;
                        order.MaturityDay = optionChainData.ExpirationDate.Day.ToString();
                        order.MaturityMonthYear = optionChainData.ExpirationDate.ToString();
                        if (order.DiscretionOffset == double.Epsilon)
                        {
                            order.DiscretionOffset = 0.0;
                        }
                    }
                    return order;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return new OrderSingle();
        }

        private void SendSymbolToTradingTicket(OptionChainDataModel optionDataModel, bool shouldSendCallSymbol, Constants.TradeType tradeType)
        {
            try
            {
                if (optionDataModel != null && SendSymbolToTT != null)
                {
                    switch (SymbologyHelper.DefaultSymbology)
                    {
                        case ApplicationConstants.SymbologyCodes.TickerSymbol:
                            if (shouldSendCallSymbol)
                            {
                                if (!string.IsNullOrEmpty(optionDataModel.Call_Symbol))
                                    SendSymbolToTT(null, new EventArgs<string, string>(optionDataModel.Call_Symbol, tradeType.ToString()));
                            }
                            else if (!string.IsNullOrEmpty(optionDataModel.Put_Symbol))
                                SendSymbolToTT(null, new EventArgs<string, string>(optionDataModel.Put_Symbol, tradeType.ToString()));
                            break;
                        case ApplicationConstants.SymbologyCodes.ActivSymbol:
                            if (shouldSendCallSymbol)
                            {
                                if (!string.IsNullOrEmpty(optionDataModel.Call_ActivSymbol))
                                    SendSymbolToTT(null, new EventArgs<string, string>(optionDataModel.Call_ActivSymbol, tradeType.ToString()));
                            }
                            else if (!string.IsNullOrEmpty(optionDataModel.Put_ActivSymbol))
                                SendSymbolToTT(null, new EventArgs<string, string>(optionDataModel.Put_ActivSymbol, tradeType.ToString()));
                            break;
                        case ApplicationConstants.SymbologyCodes.FactSetSymbol:
                            if (shouldSendCallSymbol)
                            {
                                if (!string.IsNullOrEmpty(optionDataModel.Call_FactSetSymbol))
                                    SendSymbolToTT(null, new EventArgs<string, string>(optionDataModel.Call_FactSetSymbol, tradeType.ToString()));
                            }
                            else if (!string.IsNullOrEmpty(optionDataModel.Put_FactSetSymbol))
                                SendSymbolToTT(null, new EventArgs<string, string>(optionDataModel.Put_FactSetSymbol, tradeType.ToString()));
                            break;
                        case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                            if (shouldSendCallSymbol)
                            {
                                if (!string.IsNullOrEmpty(optionDataModel.Call_BloombergSymbol))
                                    SendSymbolToTT(null, new EventArgs<string, string>(optionDataModel.Call_BloombergSymbol, tradeType.ToString()));
                            }
                            else if (!string.IsNullOrEmpty(optionDataModel.Put_BloombergSymbol))
                                SendSymbolToTT(null, new EventArgs<string, string>(optionDataModel.Put_BloombergSymbol, tradeType.ToString()));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private async System.Threading.Tasks.Task CreatePricingServiceProxy()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                _pricingServiceProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion

        #region ILiveFeedCallback Members
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            try
            {
                if (data.Symbol == _symbolSecMasterBaseObj.TickerSymbol)
                {
                    if (InvokeRequired)
                    {
                        this.BeginInvoke(new MethodInvoker(delegate
                        {
                            SnapshotResponse(data);
                        }));
                        return;
                    }

                    Last = data.LastPrice;
                    Bid = data.Bid;
                    Ask = data.Ask;
                    Change = data.Change;
                    ChangePercentage = data.PctChange;

                    ChangeTickColor(data.LastTick, ultraUnderlyingSymbolLastValueLabel.Appearance);
                    ChangeTickColor(data.LastTick, ultraUnderlyingSymbolChangeValueLabel.Appearance);
                    ChangeTickColor(data.LastTick, ultraUnderlyingSymbolChangePercentageValueLabel.Appearance);

                    return;
                }

                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Received Snapshot response for Symbol: {0}", data.Symbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);

                OptionChainDataModel optionChainDataModelObject = optionChainData.FirstOrDefault(x => x.Call_Symbol == data.Symbol || x.Put_Symbol == data.Symbol);

                if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && optionChainDataModelObject != null)
                {
                    this.ultraOptionsGrid.SuspendRowSynchronization();
                    if (optionChainDataModelObject.Call_Symbol == data.Symbol)
                    {
                        optionChainDataModelObject.Call_Tick = data.LastTick;
                        optionChainDataModelObject.Call_Ask = data.Ask;
                        optionChainDataModelObject.Call_Bid = data.Bid;
                        optionChainDataModelObject.Call_Change = data.Change;
                        optionChainDataModelObject.Call_Last = data.LastPrice;
                        optionChainDataModelObject.Call_OpenInt = data.OpenInterest;
                        optionChainDataModelObject.Call_Volume = data.TradeVolume;
                    }
                    else
                    {
                        optionChainDataModelObject.Put_Tick = data.LastTick;
                        optionChainDataModelObject.Put_Ask = data.Ask;
                        optionChainDataModelObject.Put_Bid = data.Bid;
                        optionChainDataModelObject.Put_Change = data.Change;
                        optionChainDataModelObject.Put_Last = data.LastPrice;
                        optionChainDataModelObject.Put_OpenInt = data.OpenInterest;
                        optionChainDataModelObject.Put_Volume = data.TradeVolume;
                    }
                    this.ultraOptionsGrid.ResumeRowSynchronization();

                    lock (loadedOptions)
                    {
                        if (!loadedOptions.Contains(data.Symbol))
                        {
                            loadedOptions.Add(data.Symbol);

                            if (loadedOptions.Count == 1)
                            {
                                BindGridData(optionChainData);
                                SetToolStripStatusMessage(string.Format("1/{0} option loaded ...", subscribedOptions));
                            }
                            else if (loadedOptions.Count == subscribedOptions)
                            {
                                SetToolStripStatusMessage("Options successfully loaded.");
                                EnableDisableFetchButton(true);
                            }
                            else
                            {
                                SetToolStripStatusMessage(string.Format("{0}/{1} options loaded ...", loadedOptions.Count, subscribedOptions));
                            }
                        }
                    }
                }
                else if (!CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                {
                    SetToolStripStatusMessage(ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE);
                    EnableDisableFetchButton(true);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
            try
            {
                if (UIValidation.GetInstance().validate(ultraOptionsGrid))
                {
                    if (ultraOptionsGrid.InvokeRequired)
                    {
                        OptionChainResponseReceived mi = new OptionChainResponseReceived(OptionChainResponse);
                        ultraOptionsGrid.BeginInvoke(mi, new object[] { symbol, data });
                    }
                    else
                    {
                        if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                        {
                            subscribedOptions = data.Count;

                            if (subscribedOptions == 0)
                            {
                                BindGridData();
                                SetToolStripStatusMessage("No options found!", DOWNCOLOR);
                                EnableDisableFetchButton(true);

                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("No options found for Symbol: {0}", Symbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                                return;
                            }
                            else
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Received strikes for {0} options", data.Count), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                            }

                            foreach (OptionStaticData dataItem in data)
                            {
                                OptionChainDataModel optionChainDataModelObject = optionChainData.FirstOrDefault(x => x.ExpirationDate == dataItem.ExpirationDate && x.StrikePrice == dataItem.StrikePrice);

                                if (!string.IsNullOrEmpty(dataItem.Symbol))
                                {
                                    if (optionChainDataModelObject == null)
                                    {
                                        optionChainDataModelObject = new OptionChainDataModel();
                                        optionChainDataModelObject.UnderlyingSymbol = symbol;
                                        optionChainDataModelObject.ExpirationDate = dataItem.ExpirationDate;
                                        optionChainDataModelObject.StrikePrice = dataItem.StrikePrice;

                                        optionChainData.Add(optionChainDataModelObject);
                                    }

                                    if (dataItem.PutOrCall == OptionType.CALL)
                                    {
                                        optionChainDataModelObject.Call_Symbol = dataItem.Symbol;
                                        optionChainDataModelObject.Call_BloombergSymbol = dataItem.BloombergSymbol;
                                        optionChainDataModelObject.Call_FactSetSymbol = dataItem.FactSetSymbol;
                                        optionChainDataModelObject.Call_ActivSymbol = dataItem.ActivSymbol;
                                    }
                                    else if (dataItem.PutOrCall == OptionType.PUT)
                                    {
                                        optionChainDataModelObject.Put_Symbol = dataItem.Symbol;
                                        optionChainDataModelObject.Put_BloombergSymbol = dataItem.BloombergSymbol;
                                        optionChainDataModelObject.Put_FactSetSymbol = dataItem.FactSetSymbol;
                                        optionChainDataModelObject.Put_ActivSymbol = dataItem.ActivSymbol;
                                    }
                                }
                                else
                                    subscribedOptions--;
                            }

                            if (subscribedOptions == 0)
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("An error occured, please check logs for more information.", Symbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                                SetToolStripStatusMessage("An error occured, please check logs for more information.", Color.Red);
                                EnableDisableFetchButton(true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public void LiveFeedConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void LiveFeedDisConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Dispose
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (components != null)
                        components.Dispose();
                }

                base.Dispose(disposing);
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
    }
}