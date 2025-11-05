using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Prana.AlgoStrategyControls;
using Prana.Blotter.Classes;
using Prana.Blotter.Controls;
using Prana.Blotter.Forms;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Rebalancer;
using Prana.Rebalancer.PercentTradingTool.ViewModel;
using Prana.ShortLocate.Preferences;
using Prana.TradeManager.Extension;
using Prana.TradeManager.Extension.CacheStore;
using Prana.TradingTicket.Forms;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using MessageBox = System.Windows.Forms.MessageBox;
using UIElement = Infragistics.Win.UIElement;

namespace Prana.Blotter
{
    public partial class WorkingSubBlotterGrid : UserControl
    {
        private SortedList<DateTime, QTTBlotterLinkingData> _linkingDataList = new SortedList<DateTime, QTTBlotterLinkingData>();
        private ShortLocateUIPreferences _shortLocatePreferences = null;
        private ctrlShortLocatePrefDataManager Dataobj = new ctrlShortLocatePrefDataManager();
        private CompanyUser _loginUser;
        private BlotterPreferenceData _blotterPreferenceData = null;
        private bool _isManualOrdersOverrideEnabled = true;
        MultiBrokerSubOrders subOrders = null;
        public event EventHandler<EventArgs<string>> UpdateStatusBar = null;
        public event EventHandler<EventArgs<string>> UpdateCountStatusBar = null;
        public event EventHandler DisableRolloverButton;
        public event EventHandler UpdateOnRolloverComplete;
        /// <summary>
        /// The _allocation proxy
        /// </summary>
        public ProxyBase<IAllocationManager> _allocationProxy;

        /// <summary>
        /// Occurs when [highlight symbol send on bloter main].
        /// </summary>
        public event EventHandler<EventArgs<string>> HighlightSymbolSendOnBloterMain = null;
        internal static Prana.TradeManager.TradeManager _tradeManager;
        internal static Prana.TradeManager.TradeManager TradeManagerInstance
        {
            get
            {
                if (_tradeManager == null)
                {
                    _tradeManager = TradeManager.TradeManager.GetInstance();
                }
                return _tradeManager;
            }
        }

        private OrderFields.BlotterTypes _blotterType = OrderFields.BlotterTypes.WorkingSubs;
        public OrderFields.BlotterTypes BlotterType
        {
            get { return _blotterType; }
            set { _blotterType = value; }
        }

        private string _key = string.Empty;
        public string Key
        {
            set { _key = value; }
            get { return _key; }
        }

        /// <summary>
        /// The view allocation details window
        /// </summary>
        ViewAllocationDetailsWindow viewAllocationDetailsWindow = null;

        /// <summary>
        /// Occurs when [go to allocation clicked].
        /// </summary>
        public virtual event EventHandler<EventArgs<string, DateTime, DateTime>> GoToAllocationClicked = null;

        public ArrayList DisplayColumns
        {
            set
            {
                try
                {
                    if (dgBlotter != null)
                    {
                        foreach (UltraGridBand band in dgBlotter.DisplayLayout.Bands)
                        {
                            ColumnsCollection columns = band.Columns;
                            int columnOrder = 0;

                            foreach (UltraGridColumn column in columns)
                            {
                                if (!value.Contains(column.Key))
                                {
                                    // hide this column
                                    band.Columns[column.Key].Hidden = true;
                                }
                                else
                                {
                                    // show this column
                                    band.Columns[column.Key].Hidden = false;
                                }
                            }

                            foreach (object column in value)
                            {
                                if (band.Columns.Exists(column.ToString()))
                                {
                                    band.Columns[column.ToString()].Header.VisiblePosition = columnOrder++;
                                }
                            }
                        }
                        dgBlotter.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                        SetDefaultFilters();
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

        public WorkingSubBlotterGrid()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        #region Initialize Grid
        /// <summary>
        /// Initializes the contol.
        /// </summary>
        /// <param name="blotterOrderColl">The blotter order coll.</param>
        /// <param name="key">The key.</param>
        /// <param name="loginUser">The login user.</param>
        /// <param name="blotterColumnPrefs">The blotter column prefs.</param>
        /// <param name="blotterPreferenceData">The blotter color prefs.</param>
        public virtual void InitContol(OrderBindingList blotterOrderColl, string key, CompanyUser loginUser, BlotterPreferenceData blotterPreferenceData)
        {
            try
            {
                _isManualOrdersOverrideEnabled = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsManualOrdersOverrideEnabled"));
                _loginUser = loginUser;
                _key = key;
                _blotterPreferenceData = blotterPreferenceData;
                dgBlotter.DisplayLayout.MaxBandDepth = 2;
                dgBlotter.DataSource = blotterOrderColl;

                BlotterOrderCollections.GetInstance().OrderCollectionIndexChanged += new EventHandler<BlotterOrderCollections.IndexEventArgs>(WorkingSubBlotterGrid_OrderCollectionIndexChanged);
                BlotterOrderCollections.GetInstance().WorkingSubCollectionIndexChanged += new EventHandler<BlotterOrderCollections.IndexEventArgs>(WorkingSubBlotterGrid_WorkingSubCollectionIndexChanged);
                _allocationProxy = new ProxyBase<IAllocationManager>(BlotterConstants.LIT_ALLOCATION_END_POINT_ADDRESS_NAME);

                this.dgBlotter.ContextMenu = contextMenu;

                foreach (UltraGridBand band in dgBlotter.DisplayLayout.Bands)
                {
                    if (band.Index < 2)
                    {
                        SetColumnFormats(band);
                    }
                    if (band.Index > 0)
                    {
                        band.ColHeadersVisible = false;
                    }
                }

                SetDefaultFilters();
                SetUpTransferTradeMenuItem();

                #region Hide Unnecessary Columns
                if (this._blotterType == OrderFields.BlotterTypes.DynamicTab)
                {
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_PERCENTEXECUTED].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_PERCENTEXECUTED].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_DAY_EXECUTED_QUANTITY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_DAY_EXECUTED_QUANTITY].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_START_OF_DAY_QUANTITY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_START_OF_DAY_QUANTITY].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_DAY_AVERAGE_PRICE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_DAY_AVERAGE_PRICE].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_UNSENT_QTY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_UNSENT_QTY].Hidden = true;
                }
                else if (_blotterType == OrderFields.BlotterTypes.Summary)
                {
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_STATUS_WITHOUTROLLOVER].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_STATUS_WITHOUTROLLOVER].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALLOCATIONSTATUS].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALLOCATIONSTATUS].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_STRATEGY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_STRATEGY].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ACCOUNT].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ACCOUNT].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALLOCATION_SCHEME_NAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALLOCATION_SCHEME_NAME].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_MASTERFUND].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_MASTERFUND].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_PERCENTEXECUTED].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_PERCENTEXECUTED].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_DAY_EXECUTED_QUANTITY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_DAY_EXECUTED_QUANTITY].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_START_OF_DAY_QUANTITY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_START_OF_DAY_QUANTITY].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_DAY_AVERAGE_PRICE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_DAY_AVERAGE_PRICE].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_UNSENT_QTY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_UNSENT_QTY].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTION_TIME_LAST_FILL].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTION_TIME_LAST_FILL].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_PERCENT_COMPLETED].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_PERCENT_COMPLETED].Hidden = true;
                }
                else if (_blotterType == OrderFields.BlotterTypes.Orders)
                {
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_STATUS_WITHOUTROLLOVER].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORDER_STATUS_WITHOUTROLLOVER].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALLOCATIONSTATUS].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ALLOCATIONSTATUS].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_STRATEGY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_STRATEGY].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTED_QTY].Header.Caption = OrderFields.CAPTION_TOTAL_EXECUTED_QTY;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_UNEXECUTED_QUANTITY].Header.Caption = OrderFields.CAPTION_LEAVES;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_QUANTITY].Header.Caption = OrderFields.CAPTION_TARGET_QTY;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTION_TIME_LAST_FILL].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTION_TIME_LAST_FILL].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_PERCENT_COMPLETED].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_PERCENT_COMPLETED].Hidden = true;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_START_OF_DAY_QUANTITY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_START_OF_DAY_QUANTITY].Hidden = true;
                }
                #endregion

                if (_blotterType == OrderFields.BlotterTypes.Summary)
                {
                    SummarySettings();
                    HideColumnsBandSummary();
                    btnExpansion.Text = "+";
                }
                else
                {
                    this.btnExpansion.Visible = false;
                    this.lblExpansion.Visible = false;
                    this.btnCollapse.Visible = false;
                    this.lblCollapseALL.Visible = false;

                    //No need Multiband for Orders, Working Sub and Sub Order blotter grid
                    dgBlotter.DisplayLayout.ViewStyle = ViewStyle.SingleBand;
                }

                dgBlotter.AllowDrop = true;

                if (_blotterPreferenceData.WrapHeader)
                {
                    dgBlotter.DisplayLayout.Override.WrapHeaderText = DefaultableBoolean.True;
                }
                else
                {
                    dgBlotter.DisplayLayout.Override.WrapHeaderText = DefaultableBoolean.False;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the WorkingSubCollectionIndexChanged event of the WorkingSubBlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Prana.TradeManager.BlotterOrderCollections.IndexEventArgs"/> instance containing the event data.</param>
        void WorkingSubBlotterGrid_WorkingSubCollectionIndexChanged(object sender, BlotterOrderCollections.IndexEventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { WorkingSubBlotterGrid_WorkingSubCollectionIndexChanged(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        if (this._blotterType != OrderFields.BlotterTypes.Summary)
                        {
                            if (dgBlotter.Rows != null && dgBlotter.Rows.GetRowWithListIndex(e.Index) != null)
                            {
                                UpdateRowColors(dgBlotter.Rows[e.Index]);
                            }
                        }
                        if (dgBlotter.Rows != null && dgBlotter.Rows.Count == 1)
                            SetDefaultFilters();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                        throw;
                }
            }
        }

        /// <summary>
        /// Handles the OrderCollectionIndexChanged event of the WorkingSubBlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Prana.TradeManager.BlotterOrderCollections.IndexEventArgs"/> instance containing the event data.</param>
        void WorkingSubBlotterGrid_OrderCollectionIndexChanged(object sender, BlotterOrderCollections.IndexEventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { WorkingSubBlotterGrid_OrderCollectionIndexChanged(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        if (this._blotterType != OrderFields.BlotterTypes.Summary)
                        {
                            if (dgBlotter.Rows != null && dgBlotter.Rows.GetRowWithListIndex(e.Index) != null)
                            {
                                UpdateRowColors(dgBlotter.Rows[e.Index]);
                            }
                            if (dgBlotter.Rows != null && dgBlotter.Rows.Count == 1)
                            {
                                SetDefaultFilters();
                            }
                        }

                        // the following code is required bcoz the row index in all order collection is not same 
                        // for a sub order...change in open blotter for a sub does not reflect back in 
                        // order blotter sub. So we update colors for all subs for gives parent order.
                        if (this._blotterType == OrderFields.BlotterTypes.Orders)
                        {
                            UltraGridRow row = dgBlotter.Rows.GetRowWithListIndex(e.Index);
                            if (row != null && row.ChildBands != null && row.ChildBands.Count > 0)
                            {
                                foreach (UltraGridRow subRow in row.ChildBands[0].Rows)
                                {
                                    UpdateRowColors(subRow);
                                    subRow.ParentRow.ExpansionIndicator = ShowExpansionIndicator.Always;
                                }
                            }
                            if (dgBlotter.Rows != null && dgBlotter.Rows.Count == 1)
                                SetDefaultFilters();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                        throw;
                }
            }
        }

        /// <summary>
        /// Sets up transfer trade menu item.
        /// </summary>
        private void SetUpTransferTradeMenuItem()
        {
            try
            {
                menuTransferTrade.MenuItems.Clear();
                SortedDictionary<string, int> _tradingAccUsers = BlotterCacheManager.GetInstance().GetAllUsers();
                foreach (KeyValuePair<string, int> entry in _tradingAccUsers)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Text = entry.Key;
                    menuItem.Tag = entry.Value;
                    menuItem.Name = Convert.ToString(entry.Value);
                    menuItem.Click += new EventHandler(menuOtherUsers_Click); ;
                    menuTransferTrade.MenuItems.Add(menuItem);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the column formats.
        /// </summary>
        /// <param name="band">The band.</param>
        private void SetColumnFormats(UltraGridBand band)
        {
            try
            {
                _shortLocatePreferences = Dataobj.GetShortLocatePreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                //Setting All Columns Text Alignment to Center
                foreach (UltraGridColumn col in band.Columns.All)
                    col.CellAppearance.TextHAlign = HAlign.Right;

                band.Columns[OrderFields.PROPERTY_QUANTITY].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[OrderFields.PROPERTY_PRICE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[OrderFields.PROPERTY_AVGPRICE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[OrderFields.PROPERTY_LAST_SHARES].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[OrderFields.PROPERTY_LASTPRICE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[OrderFields.PROPERTY_STOP_PRICE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[OrderFields.PROPERTY_PEG_DIFF].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[OrderFields.PROPERTY_LEAVES_QUANTITY].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[OrderFields.PROPERTY_STRIKE_PRICE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[OrderFields.PROPERTY_UNSENT_QTY].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[OrderFields.PROPERTY_EXECUTED_QTY].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[OrderFields.PROPERTY_COMMISSIONRATE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[OrderFields.PROPERTY_COMMISSIONAMT].CellAppearance.TextHAlign = HAlign.Right;

                band.Columns[OrderFields.PROPERTY_Transaction_Source].Header.Caption = OrderFields.CAPTION_TRANSACTION_Source;
                band.Columns[OrderFields.PROPERTY_LEAVES_QUANTITY].Header.Caption = OrderFields.CAPTION_LEAVES_QUANTITY;
                band.Columns[OrderFields.PROPERTY_ORDER_SIDE].Header.Caption = OrderFields.CAPTION_ORDER_SIDE;
                band.Columns[OrderFields.PROPERTY_ORDER_TYPE].Header.Caption = OrderFields.CAPTION_ORDER_TYPE;
                band.Columns[OrderFields.PROPERTY_EXECUTED_QTY].Header.Caption = OrderFields.CAPTION_EXECUTED_QTY;
                band.Columns[OrderFields.PROPERTY_AVGPRICE].Header.Caption = OrderFields.CAPTION_AVG_FILL_PRICE_LOCAL;
                band.Columns[OrderFields.PROPERTY_PRICE].Header.Caption = OrderFields.CAPTION_LIMITPX;
                band.Columns[OrderFields.PROPERTY_ASSET_NAME].Header.Caption = OrderFields.CAPTION_ASSET_NAME;
                band.Columns[OrderFields.PROPERTY_IMPORTFILENAME].Header.Caption = OrderFields.CAPTION_IMPORTED_FILE_NAME;
                band.Columns[OrderFields.PROPERTY_LAST_SHARES].Header.Caption = OrderFields.CAPTION_GRID_LAST_SHARES;
                band.Columns[OrderFields.PROPERTY_PUT_CALL].Header.Caption = OrderFields.CAPTION_PUT_CALL;
                band.Columns[OrderFields.PROPERTY_BORROWERBROKER].Header.Caption = OrderFields.CAPTION_BORROWERBROKER;
                band.Columns[OrderFields.PROPERTY_BORROWERID].Header.Caption = OrderFields.CAPTION_BORROWERID;
                if (_shortLocatePreferences.Rebatefees == ShortLocateRebateFee.BPS.ToString())
                    band.Columns[OrderFields.PROPERTY_ShortRebate].Header.Caption = OrderFields.CAPTION_BORROWERRATEBPS;
                else
                    band.Columns[OrderFields.PROPERTY_ShortRebate].Header.Caption = OrderFields.CAPTION_BORROWERRATECENT;
                band.Columns[OrderFields.PROPERTY_TRANSACTION_TIME].Header.Caption = OrderFields.CAPTION_TRANSACTION_TIME;
                band.Columns[OrderFields.PROPERTY_TRANSACTION_TIME].Format = DateTimeConstants.NirvanaDateTimeFormat;
                band.Columns[OrderFields.PROPERTY_EXPIRETIME].Format = DateTimeConstants.DateformatForClosing;
                band.Columns[OrderFields.PROPERTY_EXPIRETIME].NullText = "N/A";
                band.Columns[OrderFields.PROPERTY_EXPIRETIME].Header.Caption = OrderFields.CAPTION_EXPIRETIME;
                if (_blotterType == OrderFields.BlotterTypes.WorkingSubs || _blotterType == OrderFields.BlotterTypes.SubOrders || this._blotterType == OrderFields.BlotterTypes.DynamicTab)
                {
                    band.Columns[OrderFields.PROPERTY_ORDER_STATUS].Header.Caption = OrderFields.CAPTION_ORDER_STATUS_WITHROLLOVER;
                    band.Columns[OrderFields.PROPERTY_ORDER_STATUS_WITHOUTROLLOVER].Header.Caption = OrderFields.CAPTION_ORDER_STATUS;
                }
                else
                {
                    band.Columns[OrderFields.PROPERTY_ORDER_STATUS].Header.Caption = OrderFields.CAPTION_ORDER_STATUS;
                }
                band.Columns[OrderFields.PROPERTY_EXECUTION_TIME_LAST_FILL].Header.Caption = OrderFields.CAPTION_EXECUTION_TIME_LAST_FILL;
                band.Columns[OrderFields.PROPERTY_PERCENT_COMPLETED].Header.Caption = OrderFields.CAPTION_PERCENTAGE_COMPLETED;
                band.Columns[OrderFields.PROPERTY_ALGOSTRATEGYNAME].Header.Caption = OrderFields.CAPTION_ALGOSTRATEGYNAME;
                band.Columns[OrderFields.PROPERTY_COUNTERPARTY_NAME].Header.Caption = ApplicationConstants.CONST_BROKER;
                band.Columns[OrderFields.PROPERTY_USER].Header.Caption = OrderFields.CAPTION_CURRENT_USER;
                band.Columns[OrderFields.PROPERTY_ACTUAL_USER].Header.Caption = OrderFields.CAPTION_ACTUAL_USER;
                band.Columns[OrderFields.PROPERTY_UNSENT_QTY].Header.Caption = OrderFields.CAPTION_UNCOMMITTED_QTY;
                band.Columns[OrderFields.PROPERTY_STRIKE_PRICE].Header.Caption = OrderFields.CAPTION_STRIKE_PRICE;
                band.Columns[OrderFields.PROPERTY_STOP_PRICE].Header.Caption = OrderFields.CAPTION_STOP_PRICE;
                band.Columns[OrderFields.PROPERTY_LASTPRICE].Header.Caption = BlotterConstants.CAPTION_LAST_FILL_PRICE_LOCAL;
                band.Columns[OrderFields.PROPERTY_PEG_DIFF].Header.Caption = OrderFields.CAPTION_PEG;
                band.Columns[OrderFields.PROPERTY_UNDERLYING_NAME].Header.Caption = OrderFields.CAPTION_UNDERLYING_COUNTRY;
                band.Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL].Header.Caption = OrderFields.CAPTION_UNDERLYINGSYMBOL;
                band.Columns[OrderFields.PROPERTY_TRADING_ACCOUNT].Header.Caption = OrderFields.CAPTION_TRADER;
                band.Columns[OrderFields.PROPERTY_PROCESSDATE].Header.Caption = OrderFields.CAPTION_PROCESS_DATE;
                band.Columns[OrderFields.PROPERTY_PROCESSDATE].CellActivation = Activation.NoEdit;
                band.Columns[OrderFields.PROPERTY_TRANSACTION_TIME].CellActivation = Activation.NoEdit;
                band.Columns[OrderFields.PROPERTY_EXPIRETIME].CellActivation = Activation.NoEdit;
                band.Columns[OrderFields.PROPERTY_NOTIONALVALUE].Header.Caption = BlotterConstants.CAPTION_PRINCIPAL_AMOUNT_LOCAL;
                band.Columns[OrderFields.PROPERTY_NOTIONALVALUEBASE].Header.Caption = BlotterConstants.CAPTION_PRINCIPAL_AMOUNT_BASE;
                band.Columns[OrderFields.PROPERTY_AVGPRICEBASE].Header.Caption = OrderFields.CAPTION_AVG_FILL_PRICE_BASE;
                band.Columns[OrderFields.PROPERTY_COMMISSIONAMT].Header.Caption = OrderFields.CAPTION_COMMISSION;
                band.Columns[OrderFields.PROPERTY_COMMISSIONAMT].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_COMMISSIONRATE].Header.Caption = OrderFields.CAPTION_COMMISSION_RATE;
                band.Columns[OrderFields.PROPERTY_COMMISSIONRATE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_PERCENTEXECUTED].Header.Caption = OrderFields.CAPTION_PERCENTAGE_EXECUTED;
                band.Columns[OrderFields.PROPERTY_DAY_AVERAGE_PRICE].Header.Caption = OrderFields.CAPTION_DAY_AVERAGE_PRICE;
                band.Columns[OrderFields.PROPERTY_DAY_EXECUTED_QUANTITY].Header.Caption = OrderFields.CAPTION_DAY_EXECUTED_QUANTITY;
                band.Columns[OrderFields.PROPERTY_START_OF_DAY_QUANTITY].Header.Caption = OrderFields.CAPTION_START_OF_DAY_QUANTITY;
                band.Columns[OrderFields.PROPERTY_UNEXECUTED_QUANTITY].Header.Caption = BlotterConstants.CAPTION_UNEXECUTED_QUANTITY;
                band.Columns[OrderFields.PROPERTY_CounterCurrency].Header.Caption = BlotterConstants.CAPTION_CounterCurrency;
                band.Columns[OrderFields.PROPERTY_CounterCurrencyAmount].Header.Caption = BlotterConstants.CAPTION_CounterCurrencyAmount;
                band.Columns[OrderFields.PROPERTY_CALCBASIS].Header.Caption = OrderFields.CAPTION_CALCULATION_BASIS;
                band.Columns[OrderFields.PROPERTY_INTERNALCOMMENTS].Header.Caption = OrderFields.CAPTION_INTERNAL_COMMENTS;
                band.Columns[OrderFields.PROPERTY_MASTERFUND].Header.Caption = CachedDataManager.GetInstance.IsShowmasterFundAsClient() ? "Client" : OrderFields.CAPTION_MASTER_FUND;
                band.Columns[OrderFields.PROPERTY_BLOOMBERGSYMBOL].Header.Caption = OrderFields.CAPTION_BLOOMBERG_SYMBOL;
                band.Columns[OrderFields.PROPERTY_BLOOMBERGSYMBOLEXCODE].Header.Caption = OrderFields.CAPTION_BLOOMBERG_SYMBOL_WithExchangeCode;
                band.Columns[OrderFields.PROPERTY_FACTSETSYMBOL].Header.Caption = OrderFields.CAPTION_FACTSET_SYMBOL;
                band.Columns[OrderFields.PROPERTY_ACTIVSYMBOL].Header.Caption = OrderFields.CAPTION_ACTIV_SYMBOL;
                band.Columns[OrderFields.PROPERTY_SEDOLSYMBOL].Header.Caption = OrderFields.CAPTION_SEDOLSYMBOL;
                band.Columns[OrderFields.PROPERTY_COMPANYNAME].Header.Caption = OrderFields.CAPTION_COMPANYNAME;
                band.Columns[OrderFields.PROPERTY_ALLOCATIONSTATUS].Header.Caption = OrderFields.CAPTION_ALLOCATION_STATUS;
                band.Columns[OrderFields.PROPERTY_ALLOCATION_SCHEME_NAME].Header.Caption = OrderFields.CAPTION_ALLOCATION_SCHEME_NAME;
                band.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE1].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE_1);
                band.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE2].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE_2);
                band.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE3].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE_3);
                band.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE4].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE_4);
                band.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE5].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE_5);
                band.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE6].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE_6);
                for (int i = 7; i <= 45; i++)
                {
                    band.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE + i].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE + i);
                }

                band.Columns[OrderFields.PROPERTY_REBALANCER_FILE_NAME].Header.Caption = OrderFields.CAPTION_REBALANCER_FILE_NAME;
                band.Columns[OrderFields.PROPERTY_COMMISSIONRATE].Hidden = true;
                band.Columns[OrderFields.PROPERTY_COMMISSIONAMT].Hidden = true;
                band.Columns[OrderFields.PROPERTY_CALCBASIS].Hidden = true;

                band.Columns[OrderFields.PROPERTY_COMMISSIONRATE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[OrderFields.PROPERTY_COMMISSIONAMT].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[OrderFields.PROPERTY_CALCBASIS].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[OrderFields.PROPERTY_QUANTITY].Format = ApplicationConstants.FORMAT_QTY;
                band.Columns[OrderFields.PROPERTY_PRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_AVGPRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_EXECUTED_QTY].Format = ApplicationConstants.FORMAT_QTY;
                band.Columns[OrderFields.PROPERTY_UNEXECUTED_QUANTITY].Format = ApplicationConstants.FORMAT_UNEXECUTED_QTY;
                band.Columns[OrderFields.PROPERTY_LAST_SHARES].Format = ApplicationConstants.FORMAT_QTY;
                band.Columns[OrderFields.PROPERTY_LASTPRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_LEAVES_QUANTITY].Format = ApplicationConstants.FORMAT_QTY;
                band.Columns[OrderFields.PROPERTY_UNSENT_QTY].Format = ApplicationConstants.FORMAT_QTY;
                band.Columns[OrderFields.PROPERTY_STOP_PRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_STRIKE_PRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_NOTIONALVALUE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_NOTIONALVALUEBASE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_FXRATE].Format = ApplicationConstants.FORMAT_RATE;
                band.Columns[OrderFields.PROPERTY_AVGPRICEBASE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_CounterCurrencyAmount].Format = ApplicationConstants.FORMAT_AVGPRICE;
                band.Columns[OrderFields.PROPERTY_PERCENT_COMPLETED].Format = ApplicationConstants.FORMAT_QTY;
                band.Columns[OrderFields.PROPERTY_DAY_AVERAGE_PRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_DAY_EXECUTED_QUANTITY].Format = ApplicationConstants.FORMAT_QTY;
                band.Columns[OrderFields.PROPERTY_START_OF_DAY_QUANTITY].Format = ApplicationConstants.FORMAT_QTY;

                if (band.Columns.Exists(OrderFields.PROPERTY_PUT_CALL))
                {
                    UltraGridColumn colPutOrCall = band.Columns[OrderFields.PROPERTY_PUT_CALL];
                    List<EnumerationValue> PutOrCallType = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(OptionType));
                    ValueList PutOrCallValueList = new ValueList();
                    foreach (EnumerationValue value in PutOrCallType)
                    {
                        PutOrCallValueList.ValueListItems.Add(value.Value, value.DisplayText);
                    }
                    colPutOrCall.ValueList = PutOrCallValueList;
                    colPutOrCall.CellActivation = Activation.NoEdit;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the default filters.
        /// </summary>
        private void SetDefaultFilters()
        {
            try
            {
                switch (_blotterType)
                {
                    case OrderFields.BlotterTypes.WorkingSubs:
                    case OrderFields.BlotterTypes.SubOrders:
                    case OrderFields.BlotterTypes.DynamicTab:
                        dgBlotter.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_ORDER_STATUSTAGVALUE].FilterConditions.Add(FilterComparisionOperator.NotEquals, "");
                        break;

                    case OrderFields.BlotterTypes.Orders:
                        dgBlotter.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_ORDER_STATUSTAGVALUE].LogicalOperator = FilterLogicalOperator.And;
                        dgBlotter.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_ORDER_STATUSTAGVALUE].FilterConditions.Add(FilterComparisionOperator.NotEquals, "");
                        break;

                    case OrderFields.BlotterTypes.Summary:
                        dgBlotter.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_ORDER_STATUSTAGVALUE].FilterConditions.Add(FilterComparisionOperator.NotEquals, "");
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion

        #region Grid Coloring
        /// <summary>
        /// Update row colors based on orderstatus and side
        /// </summary>
        /// <param name="row"/>       
        private void UpdateRowColors(UltraGridRow row)//(int rowIndex, string orderStatus, string side)
        {
            try
            {
                if (row != null)
                {
                    if (row.Cells != null)
                    {
                        OrderSingle currentOrder = (OrderSingle)row.ListObject;
                        if (currentOrder != null)
                        {
                            QTTBlotterLinkingData linkingData = GetLinkingDataForOrder(currentOrder);
                            if (linkingData != null)
                            {
                                row.Appearance.ForeColor = linkingData.ForeColor;
                                row.Appearance.BackColor = linkingData.BackColor;
                            }
                            else
                            {
                                row.Appearance.ResetBackColor();
                                if (_blotterPreferenceData != null)
                                {
                                    UpdateTextColorOnOrderSide(row);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                        throw;
                }
            }
        }

        /// <summary>
        /// Gets the linking data for order.
        /// </summary>
        /// <param name="currentOrder">The current order.</param>
        /// <returns></returns>
        private QTTBlotterLinkingData GetLinkingDataForOrder(OrderSingle currentOrder)
        {
            QTTBlotterLinkingData output = null;
            try
            {
                foreach (QTTBlotterLinkingData linkingData in _linkingDataList.Values)
                {
                    string brokerName = CachedDataManager.GetInstance.GetCounterPartyText(linkingData.BrokerID);
                    if (currentOrder.Symbol.Equals(linkingData.Symbol) && linkingData.BrokerID > 0 && currentOrder.CounterPartyName.Equals(brokerName)
                                      && (!linkingData.VenueID.HasValue || currentOrder.VenueID == linkingData.VenueID)
                                      && (!linkingData.AccountID.HasValue || currentOrder.Level1ID == linkingData.AccountID))
                    {
                        output = linkingData;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return output;
        }

        /// <summary>
        /// Updates the text color on order side.
        /// </summary>
        /// <param name="row">The row.</param>
        private void UpdateTextColorOnOrderSide(UltraGridRow row)
        {
            try
            {
                if (row.Appearance != null)
                {
                    UltraGridCell orderSideCell = row.Cells[OrderFields.PROPERTY_ORDER_SIDE];
                    if (orderSideCell != null)
                    {
                        string orderSide = TagDatabaseManager.GetInstance.GetOrderSideValue(orderSideCell.Text.Trim());
                        switch (orderSide)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Open:
                                if (_blotterPreferenceData.BuyOrder != null)
                                {
                                    //Purpose: http://jira.nirvanasolutions.com:8080/browse/PRANA-12034
                                    if (row.Cells.Contains(row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID]))
                                        row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Appearance.ForeColor = _blotterPreferenceData.BuyOrder;

                                    row.Appearance.ForeColor = _blotterPreferenceData.BuyOrder;
                                }
                                break;

                            case FIXConstants.SIDE_Buy_Closed:
                                if (_blotterPreferenceData.CoverOrder != null)
                                {
                                    //Purpose: http://jira.nirvanasolutions.com:8080/browse/PRANA-12034
                                    if (row.Cells.Contains(row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID]))
                                        row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Appearance.ForeColor = _blotterPreferenceData.CoverOrder;

                                    row.Appearance.ForeColor = _blotterPreferenceData.CoverOrder;
                                }
                                break;
                            case FIXConstants.SIDE_Sell:
                            case FIXConstants.SIDE_Sell_Closed:
                                if (_blotterPreferenceData.SellOrder != null)
                                {
                                    //Purpose: http://jira.nirvanasolutions.com:8080/browse/PRANA-12034
                                    if (row.Cells.Contains(row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID]))
                                        row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Appearance.ForeColor = _blotterPreferenceData.SellOrder;

                                    row.Appearance.ForeColor = _blotterPreferenceData.SellOrder;
                                }
                                break;
                            case FIXConstants.SIDE_SellShort:
                            case FIXConstants.SIDE_Sell_Open:
                                if (_blotterPreferenceData.ShortOrder != null)
                                {
                                    //Purpose: http://jira.nirvanasolutions.com:8080/browse/PRANA-12034
                                    if (row.Cells.Contains(row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID]))
                                        row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Appearance.ForeColor = _blotterPreferenceData.ShortOrder;

                                    row.Appearance.ForeColor = _blotterPreferenceData.ShortOrder;
                                }
                                break;

                            default:
                                if (_blotterPreferenceData.CoverOrder != null)
                                {
                                    //Purpose: http://jira.nirvanasolutions.com:8080/browse/PRANA-12034
                                    if (row.Cells.Contains(row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID]))
                                        row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Appearance.ForeColor = _blotterPreferenceData.CoverOrder;

                                    row.Appearance.ForeColor = _blotterPreferenceData.CoverOrder;
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
        }
        #endregion

        #region Context Menu Events
        /// <summary>
        /// Occurs when [trade click].
        /// </summary>
        public virtual event EventHandler TradeClick = null;

        /// <summary>
        /// Gets the reload order.
        /// </summary>
        /// <param name="oldOrdSgl">The old ord SGL.</param>
        /// <returns></returns>
        protected OrderSingle GetReloadOrder(OrderSingle oldOrdSgl, bool isStageOrder)
        {
            OrderSingle orderRequest = new OrderSingle();
            try
            {
                orderRequest.CopyBasicDetails(oldOrdSgl);
                if (oldOrdSgl.TransactionSource == TransactionSource.PST || oldOrdSgl.TransactionSource == TransactionSource.Rebalancer)
                    orderRequest.Level1ID = int.MinValue;
                else
                    orderRequest.Level1ID = oldOrdSgl.Level1ID;
                orderRequest.Level2ID = oldOrdSgl.Level2ID;
                orderRequest.Strategy = oldOrdSgl.Strategy;
                orderRequest.TIF = oldOrdSgl.TIF;
                orderRequest.ExpireTime = oldOrdSgl.ExpireTime;
                orderRequest.HandlingInstruction = oldOrdSgl.HandlingInstruction;
                orderRequest.ExecutionInstruction = oldOrdSgl.ExecutionInstruction;
                orderRequest.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;
                orderRequest.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderRequest.OrderStatusTagValue);
                orderRequest.TransactionSource = TransactionSource.Blotter;
                orderRequest.TransactionSourceTag = (int)TransactionSource.Blotter;
                if (!(orderRequest.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit || orderRequest.OrderTypeTagValue == FIXConstants.ORDTYPE_Stoplimit))
                    orderRequest.Price = oldOrdSgl.Price;
                if (!(orderRequest.OrderTypeTagValue == FIXConstants.ORDTYPE_Stop || orderRequest.OrderTypeTagValue == FIXConstants.ORDTYPE_Stoplimit))
                    orderRequest.StopPrice = oldOrdSgl.StopPrice;
                if (isStageOrder && oldOrdSgl.OrderStatus.Equals("PartiallyFilled"))
                    orderRequest.CumQty = Math.Abs(oldOrdSgl.CumQty + oldOrdSgl.LeavesQty);
                else
                    orderRequest.CumQty = oldOrdSgl.Quantity;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return orderRequest;

        }

        /// <summary>
        /// Handles the Click event of the menuRepeatTrade control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void menuRepeatTrade_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (TradeClick != null)
                {
                    OrderSingle or = null;
                    //if there is an active row then get the order details, else launch a new tkt.
                    if (dgBlotter.ActiveRow != null)
                    {
                        if (dgBlotter.ActiveRow.ToString() != BlotterConstants.PROPERTY_ULTRA_GRID_GROUP_BY_ROW)
                        {
                            or = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                            OrderSingle orderRequest = GetReloadOrder(or, false);
                            TradeClick(orderRequest, e);
                        }
                    }
                    else
                    {
                        TradeClick(or, e);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Launch Trading Ticket on context menu click
        /// </summary>
        /// <param name="sender">OrderRequest-- carries the symbol, Quantity, Venue, OrderSide, TargetSubID 
        ///  from the active row </param>
        /// <param name="e"></param>
        protected virtual void menuTrade_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (TradeClick != null)
                {
                    OrderSingle or = null;
                    //if there is an active row then get the order details, else launch a new tkt.
                    if (dgBlotter.ActiveRow != null)
                    {
                        if (dgBlotter.ActiveRow.ToString() != BlotterConstants.PROPERTY_ULTRA_GRID_GROUP_BY_ROW)
                        {
                            or = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                            or.ModifiedUserId = _loginUser.CompanyUserID;

                            OrderSingle orderRequest = (OrderSingle)or.Clone();
                            orderRequest.MsgType = FIXConstants.MSGOrder;
                            orderRequest.PranaMsgType = (int)OrderFields.PranaMsgTypes.InternalOrder;

                            ValidationManagerExtension.GetOrderDetails(orderRequest);

                            // we picked the Order object from the row, hence contains all IDs. 
                            // as this will be a new trade,we set the OrderIDs to default value for a new trade
                            orderRequest.ClOrderID = string.Empty;
                            orderRequest.ParentClOrderID = string.Empty;
                            orderRequest.OrigClOrderID = string.Empty;
                            orderRequest.StagedOrderID = string.Empty;
                            TradeClick(orderRequest, e);
                        }
                    }
                    else
                    {
                        TradeClick(or, e);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the update linking from QTT.
        /// </summary>
        /// <param name="linkingData">The linking data.</param>
        internal void AddUpdateLinkingFromQTT(QTTBlotterLinkingData linkingData)
        {
            try
            {
                var kvp = _linkingDataList.FirstOrDefault(x => x.Value.Index == linkingData.Index);
                if (kvp.Value == null)
                    _linkingDataList.Add(DateTime.Now, linkingData);
                else
                    _linkingDataList[kvp.Key] = linkingData;
                dgBlotter_AfterSortChange(null, null);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the update linking from QTT.
        /// </summary>
        /// <param name="linkingData">The linking data.</param>
        internal void DeleteLinkingWithQTT(int index)
        {
            try
            {
                var kvp = _linkingDataList.FirstOrDefault(x => x.Value.Index == index);
                if (kvp.Value != null)
                {
                    _linkingDataList.Remove(kvp.Key);
                    dgBlotter_AfterSortChange(null, null);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Click event of the menuCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void menuCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (dgBlotter.ActiveRow != null)
                {
                    if (dgBlotter.ActiveRow.ToString() != BlotterConstants.PROPERTY_ULTRA_GRID_GROUP_BY_ROW)
                    {
                        OrderSingle or = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                        TranferTradeRules transferTradeRules = CachedDataManager.GetInstance.GetTransferTradeRules();

                        //based on the company level preferences saved in admin, whether all the users or only owner has thr right to cancel
                        if (!transferTradeRules.IsAllowAllUserToCancelReplaceRemove)
                        {
                            if (or.CompanyUserID.Equals(_loginUser.CompanyUserID))
                            {
                                OrderSingle orRequest = (OrderSingle)or.Clone();
                                orRequest.ModifiedUserId = _loginUser.CompanyUserID;
                                orRequest.MsgType = FIXConstants.MSGOrderCancelRequest;
                                orRequest.TransactionTime = DateTime.Now.ToUniversalTime();
                                ValidationManagerExtension.GetOrderDetails(orRequest);
                                string orderDetails = ValidationManager.GetOrderText(orRequest);

                                //Added a check before cancelling so to avoid 3 different messages..
                                //For further details : http://jira.nirvanasolutions.com:8080/browse/PRANA-1122
                                bool isCancellable = Prana.TradeManager.ValidationManager.ISOrderCancellable(orRequest) || Prana.TradeManager.ValidationManager.IsOrderStatusPendingComplianceApproval(orRequest);
                                if (isCancellable)
                                {
                                    OrderSingle cancelOrder = (OrderSingle)orRequest.Clone();
                                    cancelOrder.MsgType = FIXConstants.MSGOrderCancelRequestFroze;
                                    TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(cancelOrder);

                                    if (MessageBox.Show("Are you sure you want to Cancel Order: " + orderDetails, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                    {
                                        var isSuccess = TradeManagerInstance.SendBlotterTrades(orRequest);
                                        if (isSuccess)
                                        {
                                            BlotterAuditTrailManager.GetInstance().AddAuditTrailCollection(orRequest, TradeAuditActionType.ActionType.SubOrderCancelRequested, _loginUser.CompanyUserID, "Sub Order cancel requested");
                                            BlotterAuditTrailManager.GetInstance().SaveAuditTrailData();
                                        }
                                    }
                                    else
                                    {
                                        cancelOrder.MsgType = FIXConstants.MSGOrderCancelRequestUnFroze;
                                        TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(cancelOrder);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Can't cancel order in " + or.OrderStatus + " state", "Warning!");
                                }
                            }
                            else
                            {
                                UpdateBlotterStatusBar("You do not have permissions to cancel this Trade");
                            }
                        }
                        else       //if all the user have the right to cancel
                        {
                            OrderSingle orRequest = (OrderSingle)or.Clone();
                            orRequest.ModifiedUserId = _loginUser.CompanyUserID;
                            orRequest.MsgType = FIXConstants.MSGOrderCancelRequest;
                            orRequest.TransactionTime = DateTime.Now.ToUniversalTime();
                            ValidationManagerExtension.GetOrderDetails(orRequest);
                            string orderDetails = ValidationManager.GetOrderText(orRequest);

                            //Added a check before cancelling so to avoid 3 different messages..
                            //For further details : http://jira.nirvanasolutions.com:8080/browse/PRANA-1122
                            bool isCancellable = Prana.TradeManager.ValidationManager.ISOrderCancellable(orRequest) || Prana.TradeManager.ValidationManager.IsOrderStatusPendingComplianceApproval(orRequest);
                            if (isCancellable)
                            {
                                OrderSingle cancelOrder = (OrderSingle)orRequest.Clone();
                                cancelOrder.MsgType = FIXConstants.MSGOrderCancelRequestFroze;
                                TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(cancelOrder);

                                if (MessageBox.Show("Are you sure you want to Cancel Order: " + orderDetails, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                {
                                    var isSuccess = TradeManagerInstance.SendBlotterTrades(orRequest);
                                    if (isSuccess)
                                    {
                                        BlotterAuditTrailManager.GetInstance().AddAuditTrailCollection(orRequest, TradeAuditActionType.ActionType.SubOrderCancelRequested, _loginUser.CompanyUserID, "Sub Order cancel requested");
                                        BlotterAuditTrailManager.GetInstance().SaveAuditTrailData();
                                    }
                                }
                                else
                                {
                                    cancelOrder.MsgType = FIXConstants.MSGOrderCancelRequestUnFroze;
                                    TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(cancelOrder);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Can't cancel order in " + or.OrderStatus + " state", "Warning!");
                            }
                        }
                    }
                }
                else
                {
                    UpdateBlotterStatusBar("Please select an order to Cancel!");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the audit trail for allocation.
        /// </summary>
        /// <param name="resp">The resp.</param>
        private void AddAuditTrailForAllocation(AllocationResponse resp)
        {
            try
            {
                if (dgBlotter.ActiveRow != null)
                {
                    if (dgBlotter.ActiveRow.ToString() != BlotterConstants.PROPERTY_ULTRA_GRID_GROUP_BY_ROW)
                    {
                        OrderSingle orRequest = (OrderSingle)dgBlotter.ActiveRow.ListObject;

                        resp.OldAllocationGroups.ForEach(x =>
                        {
                            var unallocatedTaxlots = x.TaxLots.ToList();
                            foreach (TaxLot taxlot in unallocatedTaxlots)
                            {
                                BlotterAuditTrailManager.GetInstance().AddDeletedTaxlotsFromGroupToAuditEntry(taxlot, orRequest, _loginUser.CompanyUserID);
                            }
                        });

                        BlotterAuditTrailManager.GetInstance().AddTaxlotsFromGroupToAuditEntry(resp.GroupList, orRequest, _loginUser.CompanyUserID);

                        BlotterAuditTrailManager.GetInstance().SaveAuditTrailData();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Occurs when [Edit/Replace Order click].
        /// </summary>
        public virtual event EventHandler ReplaceOrEditOrderClicked = null;

        /// <summary>
        /// Handles the Click event of the mnuReplaceOrEditOrder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void menuEditOrReplaceOrder_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                if (dgBlotter.ActiveRow != null)
                {
                    if (ReplaceOrEditOrderClicked != null)
                    {
                        if (dgBlotter.ActiveRow.ToString() != BlotterConstants.PROPERTY_ULTRA_GRID_GROUP_BY_ROW)
                        {
                            TranferTradeRules transferTradeRules = CachedDataManager.GetInstance.GetTransferTradeRules();
                            OrderSingle or = (OrderSingle)dgBlotter.ActiveRow.ListObject;

                            if (or.OrderStatus.Equals("Cancelled"))
                            {
                                UpdateBlotterStatusBar("Cannot Replace order in Cancelled state.");
                            }
                            else if (or.OrderStatus.Equals("RollOver"))
                            {
                                UpdateBlotterStatusBar("Cannot Replace order in RollOver state.");
                            }
                            else
                            {
                                //if only owner has the right to cancel or Replace Order a trade
                                if (!transferTradeRules.IsAllowAllUserToCancelReplaceRemove)
                                {
                                    if (or.CompanyUserID.Equals(_loginUser.CompanyUserID))
                                    {

                                        OrderSingle orRequest = (OrderSingle)or.Clone();
                                        orRequest.ModifiedUserId = _loginUser.CompanyUserID;
                                        orRequest.MsgType = FIXConstants.MSGOrderCancelReplaceRequest;

                                        ValidationManagerExtension.GetOrderDetails(orRequest);
                                        if (Prana.TradeManager.ValidationManager.IsOrderReplaceable(orRequest, or.CompanyUserID))
                                        {
                                            OrderSingle replaceOrder = (OrderSingle)orRequest.Clone();
                                            replaceOrder.MsgType = FIXConstants.MSGOrderCancelRequestFroze;
                                            TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(replaceOrder);
                                        }
                                        if (orRequest.AlgoStrategyID != String.Empty && orRequest.AlgoStrategyID != int.MinValue.ToString())
                                        {
                                            ValidationManager.SetAlgoReplaceOrderProperties(orRequest);
                                        }
                                        ReplaceOrEditOrderClicked(orRequest, e);
                                    }
                                    else
                                    {
                                        UpdateBlotterStatusBar("You do not have permissions to Replace this Trade");
                                    }
                                }
                                else
                                {
                                    OrderSingle orRequest = (OrderSingle)or.Clone();
                                    orRequest.ModifiedUserId = _loginUser.CompanyUserID;
                                    orRequest.MsgType = FIXConstants.MSGOrderCancelReplaceRequest;

                                    ValidationManagerExtension.GetOrderDetails(orRequest);
                                    if (Prana.TradeManager.ValidationManager.IsOrderReplaceable(orRequest, or.CompanyUserID))
                                    {
                                        OrderSingle replaceOrder = (OrderSingle)orRequest.Clone();
                                        replaceOrder.MsgType = FIXConstants.MSGOrderCancelRequestFroze;
                                        TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(replaceOrder);
                                    }
                                    if (orRequest.AlgoStrategyID != String.Empty && orRequest.AlgoStrategyID != int.MinValue.ToString())
                                    {
                                        ValidationManager.SetAlgoReplaceOrderProperties(orRequest);
                                    }
                                    ReplaceOrEditOrderClicked(orRequest, e);
                                }
                            }
                        }
                    }
                    else
                    {
                        UpdateBlotterStatusBar("Please select an order to Replace !");
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Occurs when [launch audit trail].
        /// </summary>
        public event EventHandler LaunchAuditTrail = null;

        /// <summary>
        /// Need to open only on the right click on the OpenBlotter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAuditTrail_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (LaunchAuditTrail != null)
                {
                    OrderSingle order = new OrderSingle();
                    if (dgBlotter.ActiveRow != null)
                    {
                        if (!(dgBlotter.ActiveRow is UltraGridGroupByRow))
                        {
                            order = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                            order.MsgType = FIXConstants.MSGOrderStatusRequest;
                            LaunchAuditTrail(order, e);
                        }
                    }
                    else
                    {
                        LaunchAuditTrail(order, e);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Click event of the menuOtherUsers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void menuOtherUsers_Click(object sender, EventArgs e)
        {
            try
            {
                OrderBindingList orders = null;
                if (this.BlotterType == OrderFields.BlotterTypes.Orders)
                {
                    orders = ((OrderBlotterGrid)this).GetCheckedRows();
                }
                else
                {
                    if (dgBlotter.ActiveRow != null)
                    {
                        if (dgBlotter.ActiveRow.ToString() != BlotterConstants.PROPERTY_ULTRA_GRID_GROUP_BY_ROW)
                        {
                            OrderSingle _order = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                            orders = new OrderBindingList() { _order };
                        }
                    }
                }
                if (orders != null)
                {
                    TranferTradeRules transferTradeRules = CachedDataManager.GetInstance.GetTransferTradeRules();
                    // To check if all the users have the permission to transfer the trade
                    if (!transferTradeRules.IsAllowUserToTansferTrade)
                    {
                        if (orders.All(x => x.CompanyUserID.Equals(_loginUser.CompanyUserID)))
                        {
                            string message = "Do you want to transfer this order and it's sub orders?";
                            if (orders.Count > 1)
                                message = "Do you want to transfer these orders and their sub orders?";
                            bool isSendSub = (this.BlotterType == OrderFields.BlotterTypes.Orders && orders.Any(ord => ord.OrderCollection != null && ord.OrderCollection.Count > 0) && MessageBox.Show(message, "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes);

                            foreach (OrderSingle ord in orders)
                            {
                                if (isSendSub && ord.OrderCollection != null && ord.OrderCollection.Count > 0)
                                {
                                    foreach (OrderSingle sub in ord.OrderCollection)
                                    {
                                        // if sub not already transferred to other user
                                        if (sub.CompanyUserID == _loginUser.CompanyUserID)
                                        {
                                            SendTUTrades(sub, int.Parse(((MenuItem)sender).Tag.ToString()), true);
                                        }
                                    }
                                }
                                SendTUTrades(ord, int.Parse(((MenuItem)sender).Tag.ToString()));
                            }
                        }
                        else
                        {
                            MessageBox.Show(_loginUser.ShortName + " does not have permission to Transfer this Trade", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //UpdateBlotterStatusBar("You do not have permissions to Transfer this Trade");
                            return;
                        }
                    }
                    //if only the owner have the permission to transfer the trade
                    else
                    {

                        bool isSendSub = (this.BlotterType == OrderFields.BlotterTypes.Orders && orders.Any(ord => ord.OrderCollection != null && ord.OrderCollection.Count > 0)
                            && MessageBox.Show("Do you want to transfer sub orders also?", "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes);
                        foreach (OrderSingle ord in orders)
                        {
                            // send child TU
                            if (isSendSub && ord.OrderCollection != null && ord.OrderCollection.Count > 0)
                            {
                                foreach (OrderSingle sub in ord.OrderCollection)
                                {
                                    SendTUTrades(sub, int.Parse(((MenuItem)sender).Tag.ToString()), true);
                                }

                            }
                            SendTUTrades(ord, int.Parse(((MenuItem)sender).Tag.ToString()));
                        }
                    }
                    UpdateBlotterStatusBar("Order transferred to user : " + (((MenuItem)sender).Text));
                    BlotterAuditTrailManager.GetInstance().SaveAuditTrailData();

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sends the tu trades.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="companyUserID">The company user identifier.</param>
        private void SendTUTrades(OrderSingle order, int companyUserID, bool isSubOrder = false)
        {
            try
            {
                OrderSingle orderRequest = (OrderSingle)order.Clone();
                orderRequest.MsgType = FIXConstants.MSGTransferUser;
                orderRequest.PranaMsgType = (int)OrderFields.PranaMsgTypes.MsgTransferUser;
                orderRequest.ModifiedUserId = _loginUser.CompanyUserID;
                var originalUser = orderRequest.CompanyUserID;

                ValidationManagerExtension.GetOrderDetails(orderRequest);
                orderRequest.CompanyUserID = companyUserID;
                orderRequest.ClOrderID = order.ParentClOrderID;
                orderRequest.TransactionTime = DateTime.Now.ToUniversalTime();
                orderRequest.ExecutionTimeLastFill = DateTime.Now.ToUniversalTime().ToString(DateTimeConstants.NirvanaDateTimeFormat);
                DateTime currentDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));
                if ((order.TIF == FIXConstants.TIF_GTC || order.TIF == FIXConstants.TIF_GTD) && order.AUECLocalDate.Date != currentDate.Date)
                {
                    MessageBox.Show("Cannot transfer the order(s) after 1st day of order execution", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var tradeSuccessful = TradeManagerInstance.SendBlotterTrades(orderRequest);
                    if (tradeSuccessful)
                    {
                        string comment = string.Empty;

                        var action = TradeAuditActionType.ActionType.OrderTransferToUser;
                        if (!isSubOrder && this.BlotterType == OrderFields.BlotterTypes.Orders)
                        {
                            comment = "Order Transferred from User " + CachedDataManager.GetInstance.GetUserText(originalUser) + " to " + CachedDataManager.GetInstance.GetUserText(companyUserID);
                            action = TradeAuditActionType.ActionType.OrderTransferToUser;
                        }
                        else
                        {
                            comment = "Sub-Order Transferred from User " + CachedDataManager.GetInstance.GetUserText(originalUser) + " to " + CachedDataManager.GetInstance.GetUserText(companyUserID);
                            action = TradeAuditActionType.ActionType.SubOrderTransferToUser;
                        }

                        BlotterAuditTrailManager.GetInstance().AddAuditTrailCollection(orderRequest, action, _loginUser.CompanyUserID, comment);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Occurs when [launch add fills].
        /// </summary>
        public event EventHandler LaunchAddFills = null;

        /// <summary>
        /// Launch AddFills Form on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>This event is being listened to in the BlotterTabNew-- Ashish</remarks>
        /// For a manual fill we have used a string "ManualFills" as message type.
        private void menuAddFills_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (dgBlotter.ActiveRow != null)
                {
                    if (dgBlotter.ActiveRow.ToString() != BlotterConstants.PROPERTY_ULTRA_GRID_GROUP_BY_ROW)
                    {
                        if (LaunchAddFills != null)
                        {
                            OrderSingle or = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                            or.MsgType = "ManualFills";
                            ValidationManagerExtension.GetOrderDetails(or);
                            LaunchAddFills(or, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        internal void UpdateBlotterStatusBar(string message)
        {
            try
            {
                if (UpdateStatusBar != null)
                    UpdateStatusBar(this, new EventArgs<string>(message));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Update Blotter Count StatusBar
        /// </summary>
        /// <param name="message"></param>
        internal void UpdateBlotterCountStatusBar(string message)
        {
            try
            {
                if (UpdateCountStatusBar != null)
                    UpdateCountStatusBar(this, new EventArgs<string>(message));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Click event of the menuSaveLayout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menuSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                string startPath = System.Windows.Forms.Application.StartupPath;
                string blotterPreferencesPath = startPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID.ToString();
                if (!Directory.Exists(blotterPreferencesPath))
                {
                    Directory.CreateDirectory(blotterPreferencesPath);
                }
                string blotterFile = blotterPreferencesPath + "\\" + _key + "BlotterGridLayout.xml";
                this.dgBlotter.DisplayLayout.SaveAsXml(blotterFile, PropertyCategories.All);
                UpdateBlotterStatusBar(BlotterUICommonMethods.SplitCamelCase(_key) + " grid layout saved");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This method is called to save layout of a particular blotter grid.
        /// This saves the calling grid layout
        /// </summary>
        public virtual void SaveLayoutBlotterGrid(PranaUltraGrid grid, string key, string blotterPreferencesPath)
        {
            try
            {
                string blotterFile = blotterPreferencesPath + "\\" + key + "BlotterGridLayout.xml";
                grid.DisplayLayout.SaveAsXml(blotterFile, PropertyCategories.All);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Occurs when [rename tab].
        /// </summary>
        public event EventHandler<EventArgs<string>> RenameTab = null;

        /// <summary>
        /// Handles the Click event of the menuRenameTab control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menuRenameTab_Click(object sender, EventArgs e)
        {
            try
            {
                if (RenameTab != null)
                    RenameTab(this, new EventArgs<string>(this.Key));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Occurs when [remove tab].
        /// </summary>
        public event EventHandler<EventArgs<string>> RemoveTab = null;

        /// <summary>
        /// Handles the Click event of the menuRemoveTab control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menuRemoveTab_Click(object sender, EventArgs e)
        {
            try
            {
                if (RemoveTab != null)
                    RemoveTab(this, new EventArgs<string>(this.Key));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
        #endregion

        #region Grid Events
        public virtual void dgBlotter_AfterRowExpanded(object sender, RowEventArgs e)
        { }

        /// <summary>
        /// Handles the InitializeRow event of the dgBlotter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeRowEventArgs"/> instance containing the event data.</param>
        protected virtual void dgBlotter_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (!e.ReInitialize)
                {
                    UpdateRowColors(e.Row);
                    UltraGridRow row = e.Row;

                    OrderSingle order = row.ListObject as OrderSingle;
                    if (order != null)
                    {
                        if (order.OrderCollection == null)
                        {
                            row.ExpansionIndicator = ShowExpansionIndicator.Never;
                            if (row.ParentCollection != null && row.ParentRow != null)
                            {
                                row.ParentRow.ExpansionIndicator = ShowExpansionIndicator.Always;
                            }
                        }
                    }

                    if (order != null)
                    {
                        if (order.SecurityType == FIXConstants.SECURITYTYPE_Options)
                        {
                            row.ToolTipText = string.Format("Strike Price = {0}, PutOrCall={1}, Maturity MonthYear={2}, UndelyingSymbol={3} ", row.Cells["StrikePrice"].Text, row.Cells["PutOrCall"].Text, row.Cells["MaturityMonthYear"].Text, row.Cells["UnderlyingSymbol"].Text);
                        }
                        else
                        {
                            row.ToolTipText = string.Format("Symbol {0}  ", row.Cells["Symbol"].Text);
                        }
                    }

                    OrderSingle orderRow = (OrderSingle)e.Row.ListObject;

                    if (orderRow != null)
                    {
                        //Upate Source Column Value
                        if (e.Row.Cells.Exists(OrderFields.PROPERTY_SOURCE))
                        {
                            if ((OrderFields.PranaMsgTypes)orderRow.PranaMsgType == OrderFields.PranaMsgTypes.ORDManual || (OrderFields.PranaMsgTypes)orderRow.PranaMsgType == OrderFields.PranaMsgTypes.ORDManualSub)
                                e.Row.Cells[OrderFields.PROPERTY_SOURCE].Value = BlotterConstants.PROPERTY_MANUAL;
                            else
                                e.Row.Cells[OrderFields.PROPERTY_SOURCE].Value = BlotterConstants.PROPERTY_FIX;
                        }

                        if (e.Row.Cells.Exists(OrderFields.PROPERTY_PUT_CALL))
                        {
                            int value = -1;
                            Int32.TryParse(e.Row.Cells[OrderFields.PROPERTY_PUT_CALL].Text, out value);
                            if (value == int.MinValue)
                            {
                                e.Row.Cells[OrderFields.PROPERTY_PUT_CALL].Value = 2;
                            }
                        }
                        if (e.Row.Cells.Exists(OrderFields.PROPERTY_EXPIRETIME) && string.IsNullOrEmpty(e.Row.Cells[OrderFields.PROPERTY_EXPIRETIME].Value.ToString()))
                        {
                            e.Row.Cells[OrderFields.PROPERTY_EXPIRETIME].Value = "N/A";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the InitializeLayout event of the dgBlotter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        protected virtual void dgBlotter_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    dgBlotter.DisplayLayout.MaxBandDepth = 2;

                    foreach (UltraGridBand band in e.Layout.Bands)
                    {
                        if (band.Index > 1)
                        {
                            band.Hidden = true;
                        }
                    }

                    //Add Unbound Columns
                    AddUnboundColumnsOrderGrid();

                    #region Load Layout
                    if (Directory.Exists(BlotterPreferenceManager.GetInstance().BlotterPreferencesPath))
                    {
                        string filePath = BlotterPreferenceManager.GetInstance().BlotterPreferencesPath + "\\" + this.Key + BlotterPreferenceManager.GetInstance().GridPreferenceFileName;
                        if (File.Exists(filePath))
                        {
                            dgBlotter.DisplayLayout.LoadFromXml(@filePath, PropertyCategories.All);
                        }
                        else
                        {
                            if (BlotterType == OrderFields.BlotterTypes.Orders || BlotterType == OrderFields.BlotterTypes.DynamicTabOrders)
                                DisplayColumns = BlotterConstants.OrderBlotterColumns;
                            else if (BlotterType == OrderFields.BlotterTypes.Summary)
                                DisplayColumns = BlotterConstants.SummaryBlotterColumns;
                            else
                                DisplayColumns = BlotterConstants.WorkingSubBlotterColumns;
                        }
                    }
                    #endregion

                    #region Column Chooser
                    if (this._blotterType != OrderFields.BlotterTypes.Summary)
                    {
                        e.Layout.Override.RowSelectors = DefaultableBoolean.True;
                        e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                        // remove All and 2nd OrderBinding List that appears in column chooser
                        BandsCollection bandsColl = dgBlotter.DisplayLayout.Bands;
                        foreach (UltraGridBand band in bandsColl)
                        {
                            if (band.Index > 0)
                            {
                                band.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                                band.ColHeadersVisible = false;
                                band.GroupHeadersVisible = false;
                            }
                        }
                    }

                    BandsCollection bands = dgBlotter.DisplayLayout.Bands;

                    foreach (UltraGridBand band in bands)
                    {
                        ColumnsCollection columns = band.Columns;
                        foreach (UltraGridColumn column in columns)
                        {
                            if (!OrderFields.DisplayableBlotterColumnList.Contains(column.Key))
                            {
                                band.Columns[column.Key].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                                band.Columns[column.Key].Hidden = true;
                            }
                            else
                            {
                                band.Columns[column.Key].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                                //    band.Columns[column.Key].Hidden = false;
                            }
                        }
                    }
                    #endregion

                    e.Layout.Override.DefaultRowHeight = 20;

                    #region Settlement Currency Fields
                    ValueList fxConversionMethodOperatorList = new ValueList();
                    List<EnumerationValue> fxConversionMethodOperator = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.Operator));
                    foreach (EnumerationValue var in fxConversionMethodOperator)
                    {
                        if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                        {
                            fxConversionMethodOperatorList.ValueListItems.Add(var.Value, var.DisplayText);
                        }
                    }

                    if (e.Layout.Bands[0].Columns.Exists(OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR))
                    {
                        e.Layout.Bands[0].Columns[OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].Header.Caption = OrderFields.CAPTION_FX_CONVERSION_METHOD_OPERATOR;
                        e.Layout.Bands[0].Columns[OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].ValueList = fxConversionMethodOperatorList;
                        e.Layout.Bands[0].Columns[OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].CellActivation = Activation.NoEdit;
                    }

                    Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                    ValueList currencies = new ValueList();
                    foreach (KeyValuePair<int, string> item in dictCurrencies)
                    {
                        currencies.ValueListItems.Add(item.Key, item.Value);
                    }
                    currencies.ValueListItems.Add(0, ApplicationConstants.C_COMBO_NONE);

                    if (e.Layout.Bands[0].Columns.Exists(OrderFields.PROPERTY_SETTLEMENTCURRENCYID))
                    {
                        e.Layout.Bands[0].Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].ValueList = currencies;
                        e.Layout.Bands[0].Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Header.Caption = OrderFields.CAPTION_SETTLEMENT_CURRENCY;
                        e.Layout.Bands[0].Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].CellActivation = Activation.NoEdit;
                    }
                    //Added By: Sachin Mishra  Purpose: Jira-Prana-7952
                    if (e.Layout.Bands[1].Columns.Exists(OrderFields.PROPERTY_SETTLEMENTCURRENCYID))
                    {
                        e.Layout.Bands[1].Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].ValueList = currencies;
                        e.Layout.Bands[1].Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Header.Caption = OrderFields.CAPTION_SETTLEMENT_CURRENCY;
                    }
                    if (e.Layout.Bands[0].Columns.Exists(OrderFields.PROPERTY_FXRATE))
                    {
                        e.Layout.Bands[0].Columns[OrderFields.PROPERTY_FXRATE].Header.Caption = OrderFields.CAPTION_FX_RATE;
                    }
                    #endregion

                    DataTable dataTableTIF = TagDatabase.GetInstance().TIF;
                    ValueList timeInForce = new ValueList();
                    foreach (DataRow item in dataTableTIF.Rows)
                    {
                        timeInForce.ValueListItems.Add(Convert.ToString(item[1]), Convert.ToString(item[2]));
                    }
                    timeInForce.ValueListItems.Add(0, ApplicationConstants.C_COMBO_NONE);

                    if (e.Layout.Bands[0].Columns.Exists(OrderFields.PROPERTY_TIF_TAGVALUE))
                    {
                        e.Layout.Bands[0].Columns[OrderFields.PROPERTY_TIF_TAGVALUE].ValueList = timeInForce;
                        e.Layout.Bands[0].Columns[OrderFields.PROPERTY_TIF_TAGVALUE].CellActivation = Activation.NoEdit;
                    }

                    if (e.Layout.Bands[1].Columns.Exists(OrderFields.PROPERTY_TIF_TAGVALUE))
                    {
                        e.Layout.Bands[1].Columns[OrderFields.PROPERTY_TIF_TAGVALUE].ValueList = timeInForce;
                        e.Layout.Bands[1].Columns[OrderFields.PROPERTY_TIF_TAGVALUE].CellActivation = Activation.NoEdit;
                    }
                    // Set the auto-size mode to AllRowsInBand for the whole grid.
                    e.Layout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.AllRowsInBand;

                    //Setting Selected Appearances Enabled to False
                    e.Layout.Override.SelectedAppearancesEnabled = DefaultableBoolean.False;

                    this.dgBlotter.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                    e.Layout.UseFixedHeaders = true;
                    e.Layout.Bands[0].Columns[0].Header.Fixed = true;

                    if (this.dgBlotter.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_TRANSACTION_TIME].FilterConditions.All.Count() > 0)
                    {
                        if (this.dgBlotter.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_TRANSACTION_TIME].FilterConditions[0].ComparisionOperator.Equals(FilterComparisionOperator.StartsWith))
                        {
                            this.dgBlotter.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_TRANSACTION_TIME].FilterConditions.Clear();
                            this.dgBlotter.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_TRANSACTION_TIME].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.NirvanaDateTimeFormat_WithoutTime));
                        }
                    }

                    if (this.dgBlotter.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_PROCESSDATE].FilterConditions.All.Count() > 0)
                    {
                        if (this.dgBlotter.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_PROCESSDATE].FilterConditions[0].ComparisionOperator.Equals(FilterComparisionOperator.StartsWith))
                        {
                            this.dgBlotter.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_PROCESSDATE].FilterConditions.Clear();
                            this.dgBlotter.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_PROCESSDATE].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.DateformatForClosing));
                        }
                    }
                    menuSaveLayout_Click(this, null);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the dgBlotter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void dgBlotter_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                System.Drawing.Point mousePoint = new System.Drawing.Point(e.X, e.Y);
                UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                if (element == null)
                {
                    dgBlotter.ActiveRow = null;
                }

                UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                if (e.Button == MouseButtons.Right)
                {
                    if (cell != null)
                    {
                        cell.Row.Activate();
                    }

                    // check if the following code is required
                    UltraGridGroupByRow groupByRow = element.GetContext(typeof(UltraGridGroupByRow)) as UltraGridGroupByRow;
                    if (groupByRow != null)
                    {
                        groupByRow.Activate();
                        groupByRow.Selected = true;
                    }
                }
                else
                {
                    if (HighlightSymbolSendOnBloterMain != null && cell != null)
                    {
                        string symbol = cell.Row.Cells["Symbol"].Value.ToString();
                        HighlightSymbolSendOnBloterMain(null, new EventArgs<string>(symbol));
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the AfterColPosChanged event of the dgBlotter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AfterColPosChangedEventArgs"/> instance containing the event data.</param>
        private void dgBlotter_AfterColPosChanged(object sender, AfterColPosChangedEventArgs e)
        {
            try
            {
                if (this._blotterType == OrderFields.BlotterTypes.Orders)
                {
                    string headerName = e.ColumnHeaders[0].Column.Key;
                    if (dgBlotter.DisplayLayout.Bands[1].Columns.Exists(headerName))
                    {
                        dgBlotter.DisplayLayout.Bands[1].Columns[headerName].Header.VisiblePosition = e.ColumnHeaders[0].VisiblePosition;
                        dgBlotter.DisplayLayout.Bands[1].Columns[headerName].Hidden = e.ColumnHeaders[0].Column.Hidden;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
        #endregion

        #region Context Menu PopUp Settings.
        private void contextMenu_Popup(object sender, System.EventArgs e)
        {
            try
            {
                //Remove Cancel and Cancel Replace from Summary and Closed Blotters
                //But before this universally check for their presence and if they dont exist
                string _orderStatus = string.Empty;
                ContextMenuSetup();
                if (dgBlotter.ActiveRow != null)
                {
                    if (dgBlotter.ActiveRow.ToString() != BlotterConstants.PROPERTY_ULTRA_GRID_GROUP_BY_ROW)
                    {
                        OrderSingle selectedOrder = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                        if (dgBlotter.ActiveRow is UltraGridGroupByRow)
                        {
                            foreach (System.Windows.Forms.MenuItem var in contextMenu.MenuItems)
                            {
                                if (var.Text == "Save Layout")
                                {
                                    var.Visible = true;
                                }
                                else
                                {
                                    var.Visible = false;
                                }
                            }
                            return;
                        }
                        _orderStatus = TagDatabaseManager.GetInstance.GetOrderStatusValue(selectedOrder.OrderStatus);
                        //Add Items(Other Users corresponding to the same trading account as the order in consideration) to the Transfer to User MenuItem
                        AddTransferUsersToMenu(dgBlotter.ActiveRow);
                        // Disable the Reload option when we select multiple trades from order grid
                        if (this._blotterType == OrderFields.BlotterTypes.Orders)
                        {
                            var checkBoxCount = 0;
                            menuRepeatTrade.Enabled = true;
                            foreach (var row in dgBlotter.Rows)
                            {
                                if (row.Cells[164].Value.Equals(true))
                                    checkBoxCount++;
                            }
                            if (checkBoxCount > 1)
                                menuRepeatTrade.Enabled = false;
                        }
                        if (this._blotterType == OrderFields.BlotterTypes.Orders || this._blotterType == OrderFields.BlotterTypes.DynamicTabOrders)
                        {
                            //Set up for Order Blotter
                            ContextMenuSetupOrderBlotter(dgBlotter.ActiveRow);
                            mnuRemoveOrder.Visible = true;
                        }
                        // Set up the context menu for Foriegn Trades (Orders from other users but same Trading Acc)
                        if (selectedOrder.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManual ||
                           selectedOrder.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub)
                        {
                            //Setting for Manual Trades
                            ContextMenuSetupManualOrdrs(_orderStatus);
                        }
                        if (selectedOrder.CompanyUserID != _loginUser.CompanyUserID)
                        {
                            //if manual override, all operations would be normally available
                            //else consider other user orders as foreign
                            if (!_isManualOrdersOverrideEnabled)
                            {
                                ContextMenuSetupForiegnOrders();
                                return;
                            }
                        }
                        if (selectedOrder.AlgoStrategyID != String.Empty && selectedOrder.AlgoStrategyID != int.MinValue.ToString())
                        {
                            ContextMenuSetupForAlgoOrders(selectedOrder);
                        }
                        menuViewAllocation.Visible = selectedOrder.OriginalAllocationPreferenceID != 0 && ((int)selectedOrder.TransactionSource == (int)TransactionSource.PST);
                        menuViewAllocationStagedOrder.Visible = GetVisibilityOfViewAllocation(selectedOrder);
                        //Need to show View Allocation and go to Allocation menu only for Working sub and Sub orders blotter grid.
                        if (this._blotterType == OrderFields.BlotterTypes.WorkingSubs || this._blotterType == OrderFields.BlotterTypes.SubOrders || this._blotterType == OrderFields.BlotterTypes.DynamicTab)
                        {
                            menuAllocate.Visible = true;
                            menuItemGoToAllocation.Visible = true;
                            menuItemRemoveManualExecution.Visible = true;
                        }
                        if (Prana.BusinessLogic.OrderInformation.IsMultiDayOrderHistory(selectedOrder))
                        {
                            menuAuditTrail.Visible = false;
                        }
                    }
                    else
                    {
                        menuAuditTrail.Visible = false;
                        menuTrade.Visible = false;
                        menuRepeatTrade.Visible = false;
                        menuCancel.Visible = false;
                        menuEditOrReplaceOrder.Visible = false;
                        menuTransferTrade.Visible = false;
                        menuAddFills.Visible = false;
                        menuSaveLayout.Visible = true;
                        menuViewAllocation.Visible = false;
                        menuViewAllocationStagedOrder.Visible = false;
                        menuDivider1.Visible = false;
                    }
                }
                else
                {
                    menuAuditTrail.Visible = false;
                    menuTrade.Visible = false;
                    menuRepeatTrade.Visible = false;
                    menuCancel.Visible = false;
                    menuEditOrReplaceOrder.Visible = false;
                    menuTransferTrade.Visible = false;
                    menuAddFills.Visible = false;
                    menuSaveLayout.Visible = true;
                    menuViewAllocation.Visible = false;
                    menuViewAllocationStagedOrder.Visible = false;
                    menuDivider1.Visible = false;
                }
            }

            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets whether View Allocation option should be visible or not.
        /// </summary>
        /// <param name="selectedOrder"></param>
        /// <returns></returns>
        private bool GetVisibilityOfViewAllocation(OrderSingle selectedOrder)
        {
            try
            {
                //Need to show View Allocation option for staged orders only
                if (this._blotterType != OrderFields.BlotterTypes.Orders && this._blotterType != OrderFields.BlotterTypes.DynamicTabOrders)
                {
                    return false;
                }
                //Need to show View Allocation option only for simple calculated pref, custom TT pref or prefs created during import, merge, RB allocation
                if (!menuViewAllocation.Visible && string.IsNullOrEmpty(CachedDataManager.GetInstance.GetAccount(selectedOrder.Level1ID)))
                {
                    if (selectedOrder.TransactionSource == TransactionSource.TradeImport)
                    {
                        return true;
                    }
                    AllocationOperationPreference operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, selectedOrder.Level1ID);
                    CheckListWisePreference appliedGeneralRule = operationPreference != null ? GetAppliedGeneralRule(selectedOrder, operationPreference) : null;
                    if (operationPreference != null && ((appliedGeneralRule != null && appliedGeneralRule.Rule.RuleType.Equals(MatchingRuleType.None))
                        || (appliedGeneralRule == null && operationPreference.DefaultRule.RuleType == MatchingRuleType.None)))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// Checks if General rule is trigerred for order.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="operationPreference"></param>
        /// <returns></returns>
        private CheckListWisePreference GetAppliedGeneralRule(OrderSingle order, AllocationOperationPreference operationPreference)
        {
            CheckListWisePreference appliedGeneralRule = null;
            try
            {
                foreach (CheckListWisePreference pref in operationPreference.CheckListWisePreference.Values)
                {
                    if (pref == null)
                        continue;
                    bool containsExchange = pref.ExchangeOperator == CustomOperator.All ? true : pref.ExchangeList.Contains(CachedDataManager.GetInstance.GetExchangeIdFromAUECId(order.AUECID));
                    if ((pref.ExchangeOperator == CustomOperator.Exclude && containsExchange) || (pref.ExchangeOperator == CustomOperator.Include && !containsExchange))
                        continue;

                    bool containsAsset = pref.AssetOperator == CustomOperator.All ? true : pref.AssetList.Contains(order.AssetID);
                    if ((pref.AssetOperator == CustomOperator.Exclude && containsAsset) || (pref.AssetOperator == CustomOperator.Include && !containsAsset))
                        continue;

                    bool containsOrderSide = pref.OrderSideOperator == CustomOperator.All ? true : pref.OrderSideList.Contains(order.OrderSideTagValue);
                    if ((pref.OrderSideOperator == CustomOperator.Exclude && containsOrderSide) || (pref.OrderSideOperator == CustomOperator.Include && !containsOrderSide))
                        continue;

                    if (order.AssetID == (int)AssetCategory.Future || order.AssetID == (int)AssetCategory.FutureOption)
                    {
                        string root = order.Symbol.Split(' ')[0];
                        bool containsPR = pref.PROperator == CustomOperator.All ? true : pref.PRList.Contains(root);
                        if ((pref.PROperator == CustomOperator.Exclude && containsPR) || (pref.PROperator == CustomOperator.Include && !containsPR))
                            continue;
                    }
                    appliedGeneralRule = pref;
                    break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return appliedGeneralRule;
        }

        private void ContextMenuSetupForAlgoOrders(OrderSingle selectedOrder)
        {
            try
            {
                menuShowDetails.Visible = true;
                menuEditOrReplaceOrder.Visible = true;
                menuCancel.Visible = true;

                AlgoStrategy algoStrategy = AlgoControlsDictionary.GetInstance().GetAlgoStrategyDatils(selectedOrder.CounterPartyID.ToString(), selectedOrder.AlgoStrategyID);

                if (selectedOrder.OrderStatusTagValue == CustomFIXConstants.ORDSTATUS_AlgoPreviousPendingReplace || selectedOrder.OrderStatusTagValue == CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected || selectedOrder.OrderStatusTagValue == CustomFIXConstants.ORDSTATUS_Aborted)
                {
                    menuAuditTrail.Visible = false;
                    menuEditOrReplaceOrder.Visible = false;
                    menuTrade.Visible = false;
                    menuCancel.Visible = false;
                }

                if (selectedOrder.OrderStatusTagValue == CustomFIXConstants.ORDSTATUS_Aborted)
                {
                    menuTransferTrade.Visible = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the transfer users to menu.
        /// </summary>
        /// <param name="row">The row.</param>
        private void AddTransferUsersToMenu(UltraGridRow row)
        {
            try
            {
                OrderSingle oCurrent = (OrderSingle)row.ListObject;
                int auecID = oCurrent.AUECID;
                int tradingAccID = oCurrent.TradingAccountID;
                Dictionary<int, string> _tradingAccUsers = BlotterCacheManager.GetInstance().GetUsersByTradingAccountIDandAUECID(tradingAccID, auecID);

                foreach (MenuItem mnuItem in menuTransferTrade.MenuItems)
                {
                    if (_tradingAccUsers.ContainsKey(int.Parse(mnuItem.Tag.ToString())))
                    {
                        mnuItem.Visible = true;
                    }
                    else
                    {
                        mnuItem.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// If the order does not belong to this user. Change the context menu accordingly
        /// </summary>
        private void ContextMenuSetupForiegnOrders()
        {
            menuCancel.Visible = false;
            menuEditOrReplaceOrder.Visible = false;
            menuTrade.Visible = false;
            menuRepeatTrade.Visible = false;
            menuTransferTrade.Visible = false;
            menuAddFills.Visible = false;
        }

        private void ContextMenuSetupManualOrdrs(string orderStatus)
        {
            menuAddFills.Visible = true;
            menuTrade.Visible = false;
            menuRepeatTrade.Visible = true;
            menuEditOrReplaceOrder.Visible = true;

            if (orderStatus == FIXConstants.ORDSTATUS_Cancelled || orderStatus == FIXConstants.ORDSTATUS_RollOver)
            {
                menuAddFills.Visible = false;
            }
        }

        private void ContextMenuSetupOrderBlotter(UltraGridRow row)
        {
            if (row.ParentRow == null)
            {
                menuTrade.Visible = true;
                menuCancel.Text = "Cancel (All Subs)";
                menuTrade.Text = "Trade (New Sub)";
            }
            else
            {
                menuTrade.Visible = false;
                menuAuditTrail.Visible = true;
            }
        }

        /// <summary>
        /// Initial Setup of context menu. Add Cancel, Edit/Replace Order, Audit Trail and Trade buttons
        /// Remove Subblotter and Manual Fills button (if contained already)
        /// </summary>
        private void ContextMenuSetup()
        {
            try
            {
                menuAuditTrail.Visible = true;
                menuTrade.Visible = false;
                menuRepeatTrade.Visible = true;
                menuCancel.Visible = true;
                menuEditOrReplaceOrder.Visible = true;
                menuEditOrReplaceOrder.Text = _blotterType == OrderFields.BlotterTypes.Orders || _blotterType == OrderFields.BlotterTypes.DynamicTabOrders ? "Edit Order(s)" : "Replace";
                menuTransferTrade.Visible = true;
                menuAddFills.Visible = false;
                menuSaveLayout.Visible = true;
                menuCancel.Text = "Cancel";
                menuShowDetails.Visible = false;
                menuItemGoToAllocation.Visible = false;
                menuItemRemoveManualExecution.Visible = false;
                menuAllocate.Visible = false;
                mnuRemoveOrder.Visible = false;
                menuDivider1.Visible = true;
                menuDivider2.Visible = this._blotterType == OrderFields.BlotterTypes.DynamicTab || this._blotterType == OrderFields.BlotterTypes.DynamicTabOrders;
                menuRemoveTab.Visible = this._blotterType == OrderFields.BlotterTypes.DynamicTab || this._blotterType == OrderFields.BlotterTypes.DynamicTabOrders;
                menuRenameTab.Visible = this._blotterType == OrderFields.BlotterTypes.DynamicTab || this._blotterType == OrderFields.BlotterTypes.DynamicTabOrders;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void menuFilters_Click(object sender, EventArgs e)
        {
            try
            {
                if (menuFilters.Checked)
                {
                    this.menuFilters.Checked = false;
                }
                else
                {
                    this.menuFilters.Checked = true;
                    this.ultraDockManager1.ShowAll(false);
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

        #region Rollover All
        /// <summary>
        /// RollOver All SubOrders
        /// </summary>
        protected void RolloverAllSubOrders()
        {
            try
            {
                if (_loginUser.CompanyUserID == TradeManagerExtension.GetInstance().BlotterClearanceCommonData.RolloverPermittedUserID)
                {
                    if (DisableRolloverButton != null)
                    {
                        DisableRolloverButton(this, new EventArgs());
                    }

                    foreach (UltraGridRow existingRow in this.dgBlotter.Rows.GetFilteredInNonGroupByRows())
                    {
                        OrderSingle currentOrder = (OrderSingle)existingRow.ListObject;
                        if (currentOrder.OrderCollection != null && currentOrder.OrderCollection.Count > 0)
                        {
                            foreach (OrderSingle subOrder in currentOrder.OrderCollection)
                            {
                                if (ValidationManager.ISOrderRolloverable(subOrder) && BlotterCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.ContainsKey(subOrder.AUECID) &&
                                    BlotterCommonCache.GetInstance().DictRolloverPermittedAUEC.ContainsKey(subOrder.AUECID) && BlotterCommonCache.GetInstance().DictRolloverPermittedAUEC[subOrder.AUECID])
                                {
                                    TradeManagerExtension.GetInstance().SendOrderForRollOver(subOrder, _loginUser.CompanyUserID);
                                }
                            }
                        }
                    }

                    if (UpdateOnRolloverComplete != null)
                    {
                        UpdateOnRolloverComplete(this, new EventArgs());
                    }
                }
                else
                {
                    UpdateBlotterStatusBar("You do not have permission to Rollover sub orders.");
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

        #region Summary Blotter Methods
        protected virtual void SummarySettings()
        { }

        private void HideColumnsBandSummary()
        {
            try
            {
                ColumnsCollection columns = dgBlotter.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != OrderFields.PROPERTY_QUANTITY
                        && column.Key != OrderFields.PROPERTY_SYMBOL
                        && column.Key != OrderFields.PROPERTY_AVGPRICE
                        && column.Key != OrderFields.PROPERTY_EXECUTED_QTY
                        && column.Key != OrderFields.PROPERTY_ORDER_SIDE
                        && column.Key != OrderFields.PROPERTY_ORDER_STATUS
                        && column.Key != OrderFields.PROPERTY_NOTIONALVALUE
                        )
                    {
                        column.Hidden = true;
                    }
                    else
                    {
                        column.Hidden = false;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion

        #region Export to Excel
        /// <summary>
        /// To Export Grid data to a WorkBook in Excel
        /// </summary>
        /// <param name="key"></param>/param>
        /// <param name="workBook"></param>/param>
        /// <param name="fileName"></param>/param>
        /// <param name="tabname"></param>/param>
        /// <returns>workbook</returns>
        public Infragistics.Documents.Excel.Workbook OnExportToExcel(Infragistics.Documents.Excel.Workbook workBook, string fileName, string tabname)
        {
            try
            {
                this.ultraGridExcelExporter1.InitializeColumn += ultragridExcelExporter_InitializeColumn;
                if (workBook == null)
                {
                    workBook = this.ultraGridExcelExporter1.Export(this.dgBlotter, fileName);
                }

                tabname = tabname.ToUpper();
                workBook.Worksheets.Add(tabname);
                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[tabname];
                this.ultraGridExcelExporter1.Export(this.dgBlotter, workBook);

                this.ultraGridExcelExporter1.InitializeColumn -= ultragridExcelExporter_InitializeColumn;

                return workBook;
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

        #endregion

        #region Button Expand/Collapse All
        /// <summary>
        /// Expand or collapse all orders in summary blotter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExpansion_Click(object sender, System.EventArgs e)
        {
            if (btnExpansion.Text == "-")
            {
                btnExpansion.Text = "+";
                this.dgBlotter.Rows.CollapseAll(true);
            }
            else
            {
                btnExpansion.Text = "-";
                this.dgBlotter.Rows.ExpandAll(true);
            }
        }

        private void btnCollapse_Click(object sender, System.EventArgs e)
        {
            this.dgBlotter.Rows.CollapseAll(true);
        }

        private bool checkNumeric(Type dataType)
        {
            if (dataType == null)
                return false;

            return (dataType == typeof(int)
                    || dataType == typeof(double)
                    || dataType == typeof(long)
                    || dataType == typeof(short)
                    || dataType == typeof(float)
                    || dataType == typeof(Int16)
                    || dataType == typeof(Int32)
                    || dataType == typeof(Int64)
                    || dataType == typeof(uint)
                    || dataType == typeof(UInt16)
                    || dataType == typeof(UInt32)
                    || dataType == typeof(UInt64)
                    || dataType == typeof(byte)
                    || dataType == typeof(sbyte)
                    || dataType == typeof(Single)
                   );
        }

        private void ultragridExcelExporter_InitializeColumn(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.InitializeColumnEventArgs e)
        {
            if (checkNumeric(e.Column.DataType))
                e.ExcelFormatStr = "#0.0000";

        }
        #endregion

        private void dgBlotter_InitializeRowsCollection(object sender, InitializeRowsCollectionEventArgs e)
        {
            try
            {
                if (dgBlotter.DisplayLayout != null)
                {
                    BandsCollection bandsColl = dgBlotter.DisplayLayout.Bands;
                    if (bandsColl != null && bandsColl.Count > 3)
                    {
                        foreach (UltraGridBand band in bandsColl)
                        {
                            if (band != null && band.Index > 0)
                            {
                                band.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                                band.ColHeadersVisible = false;
                                band.GroupHeadersVisible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the AfterPaneButtonClick event of the ultraDockManager1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinDock.PaneButtonEventArgs"/> instance containing the event data.</param>
        private void ultraDockManager1_AfterPaneButtonClick(object sender, Infragistics.Win.UltraWinDock.PaneButtonEventArgs e)
        {
            try
            {
                if (e.Pane.Text == "Row Filter")
                {
                    if (e.Button == Infragistics.Win.UltraWinDock.PaneButton.Close)
                        this.contextMenu.MenuItems[9].Checked = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the AfterSortChange event of the dgBlotter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BandEventArgs"/> instance containing the event data.</param>
        private void dgBlotter_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                foreach (UltraGridRow existingRow in dgBlotter.Rows)
                {
                    UpdateRowColors(existingRow);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        protected virtual void menuShowDetails_Click(object sender, EventArgs e)
        {
            try
            {
                OrderSingle or = null;
                //if there is an active row then get the order details, else launch a new tkt.
                if (dgBlotter.ActiveRow != null)
                {
                    if (dgBlotter.ActiveRow.ToString() != BlotterConstants.PROPERTY_ULTRA_GRID_GROUP_BY_ROW)
                    {
                        or = (OrderSingle)dgBlotter.ActiveRow.ListObject;

                        DataTable dt = FixSymbolValueTable.GetAlgoDetailsInDataTable(or.CounterPartyID.ToString(), or.AlgoStrategyID, or.AlgoProperties.TagValueDictionary);
                        OrderInformation orderInformation = new OrderInformation();
                        orderInformation.ShowData(dt);
                        orderInformation.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the BeforeColumnChooserDisplayed event of the dgBlotter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BeforeColumnChooserDisplayedEventArgs"/> instance containing the event data.</param>
        private void dgBlotter_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.dgBlotter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the DoubleClickRow event of the dgBlotter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs"/> instance containing the event data.</param>
        private void dgBlotter_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            try
            {
                if (e.Row.ListObject == null)
                    return;

                if (e.Row != null && e.Row.Cells["PranaMsgType"].Value.ToString() == ((int)OrderFields.PranaMsgTypes.ORDStaged).ToString())
                    return;

                subOrders = MultiBrokerSubOrders.GetInstance();
                subOrders.SetUp(Convert.ToString(e.Row.Cells["ParentClOrderID"].Value));
                subOrders.Show();
                subOrders.BringToFront();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Unwires the events.
        /// </summary>
        public virtual void UnwireEvents()
        {
            try
            {
                if (viewAllocationDetailsWindow != null)
                {
                    viewAllocationDetailsWindow.Closing -= delegate { viewAllocationDetailsWindow = null; };
                    viewAllocationDetailsWindow = null;
                }
                if (_allocationProxy != null)
                    _allocationProxy.Dispose();
                BlotterOrderCollections.GetInstance().OrderCollectionIndexChanged -= new EventHandler<BlotterOrderCollections.IndexEventArgs>(WorkingSubBlotterGrid_OrderCollectionIndexChanged);
                BlotterOrderCollections.GetInstance().WorkingSubCollectionIndexChanged -= new EventHandler<BlotterOrderCollections.IndexEventArgs>(WorkingSubBlotterGrid_WorkingSubCollectionIndexChanged);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Load event of the WorkingSubBlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void WorkingSubBlotterGrid_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.ApplyTheme)
                {
                    Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
                    Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
                    Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
                    Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
                    Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
                    Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
                    Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
                    Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
                    Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
                    Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
                    Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();

                    appearance1.BackColor = System.Drawing.Color.Gold;
                    appearance1.BorderColor = System.Drawing.Color.Black;
                    appearance1.ForeColor = System.Drawing.Color.Black;
                    this.dgBlotter.DisplayLayout.Override.ActiveRowAppearance = appearance1;

                    appearance2.BackColor = System.Drawing.Color.Transparent;
                    this.dgBlotter.DisplayLayout.Override.GroupByRowAppearance = appearance2;

                    appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                    this.dgBlotter.DisplayLayout.Override.HeaderAppearance = appearance3;

                    appearance4.BackColor = System.Drawing.Color.Black;
                    appearance4.ForeColor = System.Drawing.Color.Lime;
                    this.dgBlotter.DisplayLayout.Override.RowAlternateAppearance = appearance4;

                    appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
                    appearance5.ForeColor = System.Drawing.Color.Lime;
                    this.dgBlotter.DisplayLayout.Override.RowAppearance = appearance5;

                    appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                    this.dgBlotter.DisplayLayout.Override.RowSelectorAppearance = appearance6;

                    appearance7.BackColor = System.Drawing.Color.Gold;
                    appearance7.ForeColor = System.Drawing.Color.Black;
                    this.dgBlotter.DisplayLayout.Override.SelectedCellAppearance = appearance7;

                    appearance8.BackColor = System.Drawing.Color.Transparent;
                    this.dgBlotter.DisplayLayout.Override.SelectedRowAppearance = appearance8;

                    appearance9.BackColor = System.Drawing.SystemColors.Info;
                    this.dgBlotter.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance9;

                    appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                    scrollBarLook1.Appearance = appearance10;
                    this.dgBlotter.DisplayLayout.ScrollBarLook = scrollBarLook1;
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the color of the buttons.
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnExpansion.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExpansion.ForeColor = System.Drawing.Color.White;
                btnExpansion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExpansion.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExpansion.UseAppStyling = false;
                btnExpansion.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
                btnCollapse.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnCollapse.ForeColor = System.Drawing.Color.White;
                btnCollapse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCollapse.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCollapse.UseAppStyling = false;
                btnCollapse.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Select event of the menuTransferTrade control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void menuTransferTrade_Select(object sender, System.EventArgs e)
        {
            try
            {
                OrderSingle _order = null;
                if (dgBlotter.ActiveRow != null)
                {
                    if (dgBlotter.ActiveRow.ToString() != BlotterConstants.PROPERTY_ULTRA_GRID_GROUP_BY_ROW)
                    {
                        _order = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                        // Added a null check for Order and to check if Menuitems contain CompanyUser Id, PRANA-11870
                        if (_order != null)
                        {
                            if (this.menuTransferTrade.MenuItems.ContainsKey(_order.CompanyUserID.ToString()))
                                this.menuTransferTrade.MenuItems[_order.CompanyUserID.ToString()].Visible = false;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the BeforeCustomRowFilterDialog event of the dgBlotter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BeforeCustomRowFilterDialogEventArgs"/> instance containing the event data.</param>
        private void dgBlotter_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        /// <summary>
        /// Handles the Click event of the menuViewAllocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menuViewAllocation_Click(object sender, EventArgs e)
        {
            try
            {
                if (viewAllocationDetailsWindow == null)
                {
                    viewAllocationDetailsWindow = new ViewAllocationDetailsWindow();
                }
                OrderSingle selectedOrder = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                ViewAllocationDetailsViewModel viewAllocationDetailsViewModel = new ViewAllocationDetailsViewModel
                {
                    AllocationPrefID = selectedOrder.OriginalAllocationPreferenceID,
                    OrderSideId = selectedOrder.OrderSideTagValue
                };
                viewAllocationDetailsWindow.ViewAllocationDetailsViewModel = viewAllocationDetailsViewModel;
                viewAllocationDetailsViewModel.AllocationDetailsUILoaded.Execute(null);
                viewAllocationDetailsWindow.Closing += delegate { viewAllocationDetailsWindow = null; };
                BringFormToFront(viewAllocationDetailsWindow);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Click event of the menuViewAllocationStagedOrder control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuViewAllocationStagedOrder_Click(object sender, EventArgs e)
        {
            try
            {
                OrderSingle selectedOrder = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                AllocationOperationPreference operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, selectedOrder.Level1ID);
                if (operationPreference != null)
                {
                    CheckListWisePreference appliedGeneralRule = GetAppliedGeneralRule(selectedOrder, operationPreference);
                    StagedOrderAllocationView allocationDetails = new StagedOrderAllocationView();
                    allocationDetails.IsComingFromBlotter = true;
                    allocationDetails.AllocationDetails = appliedGeneralRule != null ? appliedGeneralRule.TargetPercentage : operationPreference.TargetPercentage;
                    allocationDetails.LoadAllocationData(Convert.ToDecimal(selectedOrder.Quantity));
                    allocationDetails.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Menu Allocate Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAllocate_Click(object sender, EventArgs e)
        {
            try
            {
                OrderSingle selectedOrder = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                DataTable allocationDetails = BlotterCacheManager.GetInstance().GetAllocationDetailsByClOrderID(ValidationManagerExtension.GetSelectedOrderClOrderIDs(selectedOrder));
                if (selectedOrder != null && selectedOrder.OrderStatusWithoutRollover.Equals(Prana.BusinessObjects.Compliance.Constants.PreTradeConstants.MsgTradePending))
                {
                    DialogResult dr = MessageBox.Show("Allocation cannot be done on orders awaiting Compliance Approval", "Nirvana Alert", MessageBoxButtons.OK);
                    return;
                }
                if (allocationDetails.Rows.Count == 0)
                {
                    DialogResult dr = MessageBox.Show("The order has been deleted from Allocation," + Environment.NewLine + "and thus we cannot perform this operation", "Nirvana Alert", MessageBoxButtons.OK);
                    return;
                }
                decimal totalCumQty = 0.0m;
                allocationDetails.AsEnumerable().ToList().ForEach(row =>
                {
                    if (row[OrderFields.PROPERTY_EXECUTED_QTY] != DBNull.Value)
                        totalCumQty += Convert.ToDecimal(row[OrderFields.PROPERTY_EXECUTED_QTY]);
                });
                if (totalCumQty == 0.0m)
                {
                    UpdateBlotterStatusBar("No fills available for the order to allocate.");
                    return;
                }
                List<int> accountIDs = new List<int>();
                if (allocationDetails.Rows.Count > 0 && !allocationDetails.Rows[0][BlotterConstants.CAPTION_FUND_ID].Equals(DBNull.Value))
                    accountIDs = allocationDetails.AsEnumerable().Select(r => r.Field<int>(BlotterConstants.CAPTION_FUND_ID)).ToList();
                Dictionary<int, string> accountList = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                bool isPermittedAccounts = accountIDs.All(account => accountList.ContainsKey(account));
                if (isPermittedAccounts)
                    OpenAndBindDataAllocationView(selectedOrder, allocationDetails, accountList);
                else
                    UpdateBlotterStatusBar("View of this allocation is not permitted. If you believe this message is displayed erroneously. Please contact your Nirvana Account Representative.");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Opens the and bind data allocation view.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="totalQuantity">The total quantity.</param>
        /// <param name="allocationDetails">The allocation details.</param>
        private void OpenAndBindDataAllocationView(OrderSingle order, DataTable allocationDetails, Dictionary<int, string> userAccountList)
        {
            try
            {
                ViewAllocationDetails allocationViewControl;
                Form formAllocationView;

                allocationViewControl = new ViewAllocationDetails();
                formAllocationView = new Form();
                allocationViewControl.BindData(order, allocationDetails, userAccountList, _loginUser);
                allocationViewControl.UpdateAuditAndStatusBar += allocationViewControl_UpdateStatusBar;

                UltraPanel ultraPanel1 = new UltraPanel();
                ultraPanel1.ClientArea.SuspendLayout();
                ultraPanel1.SuspendLayout();
                ultraPanel1.Dock = DockStyle.Fill;
                ultraPanel1.Name = "ultraPanel1";
                ultraPanel1.ClientArea.Controls.Add(allocationViewControl);
                allocationViewControl.Dock = DockStyle.Fill;

                formAllocationView.Controls.Add(ultraPanel1);
                formAllocationView.ShowIcon = false;
                formAllocationView.Text = "Allocation";
                formAllocationView.Name = "BlotterAllocate";
                formAllocationView.Size = new System.Drawing.Size(743, 468);
                formAllocationView.StartPosition = FormStartPosition.CenterParent;
                formAllocationView.MaximumSize = formAllocationView.MinimumSize = new System.Drawing.Size(743, 468);
                formAllocationView.MaximizeBox = false;
                formAllocationView.MinimizeBox = false;

                CustomThemeHelper.AddUltraFormManagerToDynamicForm(formAllocationView);
                CustomThemeHelper.SetThemeProperties(formAllocationView, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_WATCHLIST);

                formAllocationView.ShowInTaskbar = false;
                ultraPanel1.ClientArea.ResumeLayout(false);
                ultraPanel1.ClientArea.PerformLayout();
                ultraPanel1.ResumeLayout(false);
                formAllocationView.ShowDialog(this.Parent);
                allocationViewControl.UpdateAuditAndStatusBar -= allocationViewControl_UpdateStatusBar;
                formAllocationView.Dispose();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the UpdateStatusBar event of the allocationViewControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}"/> instance containing the event data.</param>
        private void allocationViewControl_UpdateStatusBar(object sender, EventArgs<AllocationResponse> e)
        {
            try
            {
                string result = e.Value.Response;
                if (string.IsNullOrEmpty(e.Value.Response))
                {
                    result = "Order successfully allocated.";
                    AddAuditTrailForAllocation(e.Value);
                }
                UpdateBlotterStatusBar(result);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Brings the form to front.
        /// </summary>
        /// <param name="window">The window.</param>
        private void BringFormToFront(Window window)
        {
            try
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
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Click event of the menuItemGoToAllocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menuItemGoToAllocation_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgBlotter.ActiveRow != null)
                {
                    if (dgBlotter.ActiveRow.ToString() != BlotterConstants.PROPERTY_ULTRA_GRID_GROUP_BY_ROW)
                    {
                        OrderSingle selectedOrder = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                        string parentClOrderId = ValidationManagerExtension.GetSelectedOrderClOrderIDs(selectedOrder);
                        if (selectedOrder.CumQty > 0 && !String.IsNullOrEmpty(parentClOrderId))
                        {
                            DataTable details = BlotterCacheManager.GetInstance().GetGroupIdDateByClOrderID(parentClOrderId);

                            //Added check for Row[0] only, because as per my understanding "Go to Allocation" functionality will work on Active row (Means only one Parent ClOrder Id will send at a time)
                            if (details.Rows.Count > 0 && details.Rows[0][OrderFields.PROPERTY_AUECLOCALDATE] != DBNull.Value)
                            {
                                List<string> groupIds = new List<string>();
                                List<DateTime> tradeDates = new List<DateTime>();

                                //TODO: Need to check and remove this loop, because "Go to Allocation" functionality will work on Active row (Means only one Parent ClOrder Id will send at a time)
                                foreach (DataRow row in details.Rows)
                                {
                                    string groupId = row[OrderFields.CAPTION_GROUPID].ToString();
                                    DateTime tradeDate = Convert.ToDateTime(row[OrderFields.PROPERTY_AUECLOCALDATE]).Date;

                                    if (!groupIds.Contains(groupId))
                                        groupIds.Add(groupId);

                                    if (!tradeDates.Contains(tradeDate))
                                        tradeDates.Add(tradeDate);
                                }
                                if (GoToAllocationClicked != null)
                                    GoToAllocationClicked(this, new EventArgs<string, DateTime, DateTime>(String.Join(",", groupIds), tradeDates.Min(), tradeDates.Max()));
                            }
                            else
                                UpdateBlotterStatusBar(BlotterConstants.MSG_NO_ALLOCATION_TO_VIEW);
                        }
                        else
                            UpdateBlotterStatusBar(BlotterConstants.MSG_NO_ALLOCATION_TO_VIEW);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Click event of the menuItemRemoveManualExecution control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menuItemRemoveManualExecution_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGridRow row = dgBlotter.ActiveRow;
                OrderSingle ord = (OrderSingle)row.ListObject;
                if (ord.IsManualOrder)
                {
                    if (!CachedDataManager.GetInstance.ValidateNAVLockDate(ord.AUECLocalDate))
                    {
                        MessageBox.Show("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                            + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    bool result = Prana.Allocation.ClientLibrary.DataAccess.AllocationClientDataManager.GetInstance.RemoveManualExecution(ord.ClOrderID, ord.AUECLocalDate);
                    if (ord.OrderStatusTagValue != FIXConstants.ORDSTATUS_New && !result)
                    {
                        MessageBox.Show("This order is grouped with another group." + Environment.NewLine + "Please ungroup first.", "Alert", MessageBoxButtons.OK);
                        return;
                    }
                    else
                    {
                        BlotterAuditTrailManager.GetInstance().AddAuditTrailCollection(ord, TradeAuditActionType.ActionType.SubOrderRemoveManualExcecution, _loginUser.CompanyUserID, " Sub-Order Manual Excecution Removed");
                        BlotterAuditTrailManager.GetInstance().SaveAuditTrailData();
                    }
                    List<int> tradingAccountIds = new List<int> { ord.TradingAccountID };
                    BlotterCacheManager.GetInstance().HideSubOrderFromBlotter(ord.ClOrderID, _loginUser.CompanyUserID, tradingAccountIds);
                    if (ord != null && ord.ShortLocateParameter != null)
                    {
                        ord.ShortLocateParameter.BorrowQuantity = -ord.Quantity;
                        Prana.ShortLocate.Classes.ShortLocateDataManager.GetInstance.UpdateShortLocateData(ord.ShortLocateParameter);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

        }

        /// <summary>
        /// Adds the unbound columns order grid.
        /// </summary>
        private void AddUnboundColumnsOrderGrid()
        {
            try
            {
                //Add Unbound Columns in Working Sub and Sub Order Grid after setting DataType
                if (this._blotterType == OrderFields.BlotterTypes.WorkingSubs || this._blotterType == OrderFields.BlotterTypes.SubOrders || this._blotterType == OrderFields.BlotterTypes.DynamicTab)
                {
                    if (!dgBlotter.DisplayLayout.Bands[0].Columns.Exists(OrderFields.PROPERTY_SOURCE))
                    {
                        dgBlotter.DisplayLayout.Bands[0].Columns.Add(OrderFields.PROPERTY_SOURCE, OrderFields.PROPERTY_SOURCE);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        protected virtual void menuRemoveOrderClicked(object sender, EventArgs e)
        { }

        private void menuRemoveFilter_Click(object sender, EventArgs e)
        {
            try
            {
                dgBlotter.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        void dgBlotter_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
        {
            try
            {
                if (e.Column.Key.Equals(OrderFields.PROPERTY_TRANSACTION_TIME) || e.Column.Key.Equals(OrderFields.PROPERTY_PROCESSDATE))
                {
                    e.ValueList.ValueListItems.Insert(4, "(Today)", "(Today)");
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

        void dgBlotter_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(OrderFields.PROPERTY_TRANSACTION_TIME) || e.Column.Key.Equals(OrderFields.PROPERTY_PROCESSDATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    dgBlotter.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    if (e.Column.Key.Equals(OrderFields.PROPERTY_PROCESSDATE))
                        dgBlotter.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
                    else
                        dgBlotter.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.NirvanaDateTimeFormat_WithoutTime));

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
        /// Set Grid Band Single Band/ MultiBand
        /// </summary>
        /// <param name="viewStyle"></param>
        internal void SetGridBand(ViewStyle viewStyleForOrdersGrid, bool isHiddenBand)
        {
            try
            {
                dgBlotter.DisplayLayout.ViewStyle = viewStyleForOrdersGrid;
                dgBlotter.DisplayLayout.Bands[1].Hidden = isHiddenBand;
                dgBlotter.DisplayLayout.Bands[1].ColHeadersVisible = !isHiddenBand;
                if (!isHiddenBand)
                {
                    dgBlotter.DisplayLayout.Override.HeaderPlacement = HeaderPlacement.OncePerRowIsland;
                }
                else
                {
                    dgBlotter.DisplayLayout.Override.HeaderPlacement = HeaderPlacement.FixedOnTop;
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
