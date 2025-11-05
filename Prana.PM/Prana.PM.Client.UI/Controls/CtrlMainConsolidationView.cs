#region Using directives

using ExportGridsData;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Infragistics.Win.UltraWinMaskedEdit;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.ExposurePnlCache;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.PM.Client.UI.Classes;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ColumnHeader = Infragistics.Win.UltraWinGrid.ColumnHeader;

#endregion Using directives

namespace Prana.PM.Client.UI.Controls
{
    public partial class CtrlMainConsolidationView : UserControl, IExportGridData
    {
        private DateTime _lastRefreshSortTime = DateTimeConstants.MinValue;
        private string _senderID = ConfigurationHelper.Instance.GetAppSettingValueByKey("PMSenderID");
        private string _mailServer = ConfigurationHelper.Instance.GetAppSettingValueByKey("PMMailServer");
        private int _mailServerSMTPPort = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("PMMailServerSMTPPort"));
        private string _senderPWD = ConfigurationHelper.Instance.GetAppSettingValueByKey("PMSenderPWD");
        private bool _enableSSL = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("PMEnableSSL"));
        private string _receiverIDs = ConfigurationHelper.Instance.GetAppSettingValueByKey("PMReceiverIDs");
        private string _mailSubject = ConfigurationHelper.Instance.GetAppSettingValueByKey("PMMailSubject");
        private int _errorRefreshInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("PMErrorRefreshInterval"));

        private PMAppearances _pmAppearances;
        private ExcelAndPrintUtilities _excelUtil = new ExcelAndPrintUtilities();
        private Infragistics.Win.Appearance _negativeAppearace;
        private Infragistics.Win.Appearance _positiveAppearace;
        private Infragistics.Win.Appearance _neutralAppearace;
        public string _tabNameForExportGrid = string.Empty;
        private bool isFilteredColumnIsMasterFund;
        private List<string> filteredMasterFundList = new List<string>();
        private bool addAccountNamesBasedOnMasterFund = false;
        private const string SUB_MODULE_NAME = "CustomView";
        private PMUIPrefs _uiPrefs = new PMUIPrefs();
        private Dictionary<string, object> _pttDataDictionary = new Dictionary<string, object>();
        /// <summary>
        /// The is show export pm
        /// </summary>
        private bool isShowExportPM = false;
        public event EventHandler<EventArgs<bool>> ShowDashboardEvent;
        #region Constructor

        public CtrlMainConsolidationView()
        {
            try
            {
                InitializeComponent();
                if (CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI && CommonDataCache.CachedDataManager.IsMarketDataBlocked)
                    exportToolStripMenuItem.Enabled = false;
                if (!ModuleManager.CheckModulePermissioning(PranaModules.TRADING_TICKET_MODULE, PranaModules.TRADING_TICKET_MODULE))
                    exitAllToolStripMenuItem.Enabled = false;
                if (!ModuleManager.CheckModulePermissioning(PranaModules.TRADING_TICKET_MODULE, PranaModules.TRADING_TICKET_MODULE))
                    IncreasePositionStripMenuItem.Enabled = false;
                if (!ModuleManager.CheckModulePermissioning(pttToolStripMenuItem.Text, pttToolStripMenuItem.Text))
                    pttToolStripMenuItem.Enabled = false;
                if (!ModuleManager.CheckModulePermissioning(pricingInputToolStripMenuItem.Text, pricingInputToolStripMenuItem.Text))
                    pricingInputToolStripMenuItem.Enabled = false;
                _secMasterSyncService = new Prana.WCFConnectionMgr.ProxyBase<Prana.Interfaces.ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
                _dynamicUDACache = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
                isShowExportPM = (Prana.Admin.BLL.ModuleManager.CompanyModulesPermittedToUser.Cast<Prana.Admin.BLL.Module>().Any(module => module.ModuleName.Equals(PranaModules.PORTFOLIO_MANAGEMENT_MODULE) && module.IsShowExport == true));
                InstanceManager.RegisterInstance(this);
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

        #endregion Constructor

        public void ClearData()
        {
            try
            {
                grdConsolidation.DisplayLayout.Bands[0].Summaries.Clear();
                grdConsolidation.DisplayLayout.Bands.Dispose();
                addNewViewToolStripMenuItem.Click -= addNewConsolidationViewToolStripMenuItem_Click;
                deleteViewToolStripMenuItem.Click -= deleteViewToolStripMenuItem_Click;
                renameViewToolStripMenuItem.Click -= renameViewToolStripMenuItem_Click;
                expandCollapseToolStripMenuItem.Click -= expandCollapseToolStripMenuItem_Click;
                asDefaultToolStripMenuItem.Click -= asDefaultToolStripMenuItem_Click;
                currentToolStripMenuItem.Click -= currentToolStripMenuItem_Click;
                forAllToolStripMenuItem1.Click -= forAllToolStripMenuItem1_Click;
                clearFiltersToolStripMenuItem.Click -= clearFiltersToolStripMenuItem_Click;
                showSummaryToolStripMenuItem.Click -= showSummaryToolStripMenuItem_Click;
                hideSummaryToolStripMenuItem.Click -= hideSummaryToolStripMenuItem_Click;
                showHideDashboardToolStripMenuItem.Click -= showHideDashboardToolStripMenuItem_Click;
                //addToolStripMenuItem.Click -= addToolStripMenuItem_Click;
                SetToolStripMenuItem.Click -= delegate (object sender, EventArgs e) { pttToolStripMenuItem_Click(sender, e, "Set"); };
                IncreaseToolStripMenuItem.Click -= delegate (object sender, EventArgs e) { pttToolStripMenuItem_Click(sender, e, "Increase"); };
                DecreaseToolStripMenuItem.Click -= delegate (object sender, EventArgs e) { pttToolStripMenuItem_Click(sender, e, "Decrease"); };
                exitAllToolStripMenuItem.Click -= ClosePosition_Click;
                IncreasePositionStripMenuItem.Click -= IncreasePositionStripMenuItem_Click;
                exportToExcelToolStripMenuItem.Click -= exportToExcelToolStripMenuItem_Click;
                exportToCSVToolStripMenuItem.Click -= exportToCSVToolStripMenuItem_Click;
                grdConsolidation.InitializePrint -= grdConsolidation_InitializePrint;
                grdConsolidation.MouseDown -= grdConsolidation_MouseDown;
                grdConsolidation.MouseWheel -= grdConsolidation_MouseWheel;
                grdConsolidation.MouseUp -= grdConsolidation_MouseUp;
                grdConsolidation.BeforeSortChange -= grdConsolidation_BeforeSortChange;
                grdConsolidation.InitializeLayout -= grdConsolidation_InitializeLayout;
                grdConsolidation.BeforeRowFilterDropDown -= grdConsolidation_BeforeRowFilterDropDown;
                grdConsolidation.InitializeGroupByRow -= grdConsolidation_InitializeGroupByRow;
                grdConsolidation.AfterColPosChanged -= grdConsolidation_AfterColPosChanged;
                grdConsolidation.BeforeColumnChooserDisplayed -= grdConsolidation_BeforeColumnChooserDisplayed;
                grdConsolidation.AfterRowActivate -= grdConsolidation_AfterRowActivate;
                grdConsolidation.InitializeRow -= grdConsolidation_InitializeRow;
                grdConsolidation.MouseClick -= grdConsolidation_MouseClick;
                grdConsolidation.KeyDown -= grdConsolidation_KeyDown;
                grdConsolidation.AfterSortChange -= grdConsolidation_AfterSortChange;
                grdConsolidation.BeforeRowDeactivate -= grdConsolidation_BeforeRowDeactivate;
                grdConsolidation.DoubleClickRow -= grdConsolidation_DoubleClickRow;
                grdConsolidation.SummaryValueChanged -= grdConsolidation_SummaryValueChanged;

                expandCollapseLevel1ToolStripMenuItem.Click -= expandCollapseLevel1ToolStripMenuItem_Click;
                expandCollapseLevel2ToolStripMenuItem.Click -= expandCollapseLevel2ToolStripMenuItem_Click;
                expandCollapseLevel3ToolStripMenuItem.Click -= expandCollapseLevel3ToolStripMenuItem_Click;
                expandCollapseLevel4ToolStripMenuItem.Click -= expandCollapseLevel4ToolStripMenuItem_Click;
                expandCollapseLevel5ToolStripMenuItem.Click -= expandCollapseLevel5ToolStripMenuItem_Click;
                grdConsolidation.DisplayLayout.Bands.Dispose();
                grdConsolidation.Dispose();
                grdConsolidation = null;
                _allowedGroupedSortedColumnAlphaNumeric = null;
                _allowedGroupedSortedColumnDates = null;
                _allowedGroupedSortedColumnNumeric = null;
                _allowedGroupedSortedColumnNumericCumText = null;
                _allowedGroupedSortedColumnText = null;
                _columnSorted = null;
                _CurrentUltraGridGroupByRow = null;
                _customSorterAlphaNumeric = null;
                _customSorterDate = null;
                _customSorterDateForRow = null;
                _customSorterText = null;
                _customSorterTextForRow = null;
                _customViewPreference.Dispose();
                _customViewPreference = null;

                _exPnlBindableView.GridData.RowsToUpdateColor -= GridData_RowsToUpdateColor;
                _exPnlBindableView.Dispose();
                _exPnlBindableView = null;
                _accountFilterKey = null;

                if (_pmAppearances != null)
                    _pmAppearances.Dispose();
                _pmAppearances = null;
                GC.SuppressFinalize(this);
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

        private int _splitterDistance = int.MinValue;

        public int SplitterDistanceFromTop
        {
            get { return _splitterDistance; }
            set { _splitterDistance = value; }
        }

        #region Global Variables

        private bool _isAccountView;
        private string m_strSearchString;

        public bool IsAccountView
        {
            set { _isAccountView = value; }
        }

        private ExPnlBindableView _exPnlBindableView;

        public ExPnlBindableView ExPnlBindableView
        {
            get { return _exPnlBindableView; }
            set
            {
                _exPnlBindableView = value;
            }
        }

        internal void SetRowColorBasis(string rowColorBasis)
        {
            if (_exPnlBindableView != null)
            {
                _exPnlBindableView.RowColorBasis = rowColorBasis;
            }
        }

        private void GridData_RowsToUpdateColor(object sender, ExposurePnlCacheBindableDictionary.IndexEventArgs e)
        {
            try
            {
                if (grdConsolidation.Rows != null)
                {
                    if (grdConsolidation.Rows.Count > e.Index)
                    {
                        UltraGridRow row = grdConsolidation.Rows.GetRowWithListIndex(e.Index);
                        if (row != null)
                        {
                            row.Refresh(RefreshRow.FireInitializeRow, true);
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

        SerializableDictionary<string, Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects.DynamicUDA> _dynamicUDACache = new SerializableDictionary<string, Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects.DynamicUDA>();

        Prana.WCFConnectionMgr.ProxyBase<Prana.Interfaces.ISecMasterSyncServices> _secMasterSyncService = null;

        public Prana.WCFConnectionMgr.ProxyBase<Prana.Interfaces.ISecMasterSyncServices> SecMasterSyncService
        {
            set { _secMasterSyncService = value; }
        }

        private UltraGridRow _CurrentUltraGridGroupByRow;
        private CustomViewPreferences _customViewPreference;
        private DataTable _tblView = new DataTable();
        //private int _curentUserid = int.MinValue;

        #endregion Global Variables

        #region Event Definitions

        public event EventHandler<EventArgs<OrderSingle, Dictionary<int, double>>> TradeClick;

        public event EventHandler PricingInputClick;

        public event EventHandler AddNewConsolidationView;

        public event AfterColPosChangedEventHandler AfterColPositionChanged;

        public event EventHandler DeleteViewClick;

        public event EventHandler RenameViewClick;

        public event EventHandler<EventArgs<string, PTTMasterFundOrAccount, List<string>, string>> SendPercentTradingDataToPM;

        public event EventHandler AppearanceClick;

        #endregion Event Definitions

        #region Initialize the control

        private bool _isInitialized;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }

        //public void ClearConsolidatedViewSortedColumnsList()
        //{
        //    try
        //    {
        //        if (_customViewPreference.GroupByColumnsCollection.Count > 0)
        //        {
        //            grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Clear();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        public void ClearPMAppearanceCache()
        {
            PMAppearanceManager.Dispose();
        }

        public void SetSymbolColumnForAccountview()
        {
            try
            {
                if (_customViewPreference.GroupByColumnsCollection.Count > 0)
                {
                    //Set GroupBy Columns
                    foreach (string item in _customViewPreference.GroupByColumnsCollection)
                    {
                        if (grdConsolidation.DisplayLayout.Bands[0].Columns.Exists(item))
                        {
                            grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Add(item, false, true);
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

        public void InitControl(bool isGroupByAllowed, bool showGroupByBox, CustomViewPreferences preferences, ExPnlBindableView exPnlBindableView)
        {
            try
            {
                cmbPositionCategory.Enabled = false;
                //ultraGridExcelExporter = new UltraGridExcelExporter(components);

                if (_pmAppearances == null)
                {
                    _pmAppearances = PMAppearanceManager.PMAppearance;
                }
                _customViewPreference = preferences;
                _exPnlBindableView = exPnlBindableView;

                if (_exPnlBindableView != null)
                {
                    _exPnlBindableView.RowColorBasis = _pmAppearances.RowColorbasis;
                }
                if (_exPnlBindableView != null)
                    _exPnlBindableView.GridData.RowsToUpdateColor += GridData_RowsToUpdateColor;

                _isGroupByAllowed = isGroupByAllowed;

                _showGroupByBox = showGroupByBox;

                if (!_isInitialized)
                {
                    try
                    {
                        grdConsolidation.Name = _parentKey;
                        SetupBinding();
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
                    _isInitialized = true;
                }
                if (CustomThemeHelper.ApplyTheme)
                {
                    splitContainer1.Panel1.BackColor = Color.FromArgb(33, 44, 57);
                    splitContainer1.BorderStyle = BorderStyle.None;
                }
                else
                {
                    splitContainer1.Panel1.BackColor = Color.Transparent;
                }
                if (!_customViewPreference.IsDashboardVisible)
                    showHideDashboardToolStripMenuItem.Text = "Show Dashboard";
                else
                    showHideDashboardToolStripMenuItem.Text = "Hide Dashboard";
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
        /// Highlights the symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        internal void HighlightSymbol(string symbol)
        {
            try
            {
                UltraGridRow row = null;
                foreach (UltraGridRow oRow in grdConsolidation.Rows)
                {
                    if (!oRow.IsGroupByRow && oRow.Cells != null)
                    {
                        string strCellValue = oRow.Cells[PMConstants.COL_Symbol].GetText(MaskMode.Raw);
                        if (strCellValue.Equals(symbol))
                        {
                            row = oRow;
                            break;
                        }
                    }
                    else if (oRow.Description.Equals(symbol))
                    {
                        row = oRow;
                        break;
                    }
                }
                if (row != null)
                {
                    grdConsolidation.ActiveRow = row;
                    row.Selected = true;
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

        private BindingSource bindingSource;

        private void SetupBinding()
        {
            try
            {
                DataColumn dcviewType = new DataColumn("viewType");
                DataColumn dcviewName = new DataColumn("viewName");
                _tblView.Columns.Add(dcviewType);
                _tblView.Columns.Add(dcviewName);
                DataRow viewRowAccount = _tblView.NewRow();
                viewRowAccount[0] = "Account";
                viewRowAccount[1] = "Account";
                _tblView.Rows.Add(viewRowAccount);

                cmbPositionCategory.DataSource = null;
                cmbPositionCategory.DataSource = _tblView;
                cmbPositionCategory.ValueMember = "viewType";
                cmbPositionCategory.DisplayMember = "viewName";
                cmbPositionCategory.DisplayLayout.Bands[0].Columns["viewType"].Hidden = true;
                cmbPositionCategory.DisplayLayout.CaptionVisible = DefaultableBoolean.False;
                cmbPositionCategory.DataBind();
                grdConsolidation.DataBindings.Clear();
                grdConsolidation.DataMember = "GridData.Values";
                grdConsolidation.DataSource = null;
                bindingSource = new BindingSource();
                bindingSource.DataSource = _exPnlBindableView;
                grdConsolidation.DataSource = bindingSource;
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

        #endregion Initialize the control

        public void RequestAccountData()
        {
            try
            {
                if (cmbPositionCategory.Rows != null && cmbPositionCategory.Rows.Count > 0)
                {
                    cmbPositionCategory.SelectedRow = cmbPositionCategory.Rows[0]; //Account
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

        private string _accountFilterKey = string.Empty;

        /// <summary>
        /// Set Strategy filter key, this will be used for account filtering
        /// </summary>
        public string AccountFilterKey
        {
            set { _accountFilterKey = value; }
        }

        private bool _isGroupByAllowed;

        /// <summary>
        /// Gets or sets a value indicating whether group is allowed  on grid or not by allowed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is group by allowed; otherwise, <c>false</c>.
        /// </value>
        public bool IsGroupByAllowed
        {
            set
            {
                _isGroupByAllowed = value;
            }
        }

        private bool _showGroupByBox;

        public bool ShowGroupByBox
        {
            get { return _showGroupByBox; }
            set { _showGroupByBox = value; }
        }

        private bool _hideExpandCollapseButtonPanel;

        public bool HideExpandCollapseButtonPanel
        {
            get { return _hideExpandCollapseButtonPanel; }
            set
            {
                _hideExpandCollapseButtonPanel = value;
                splitContainer1.Panel1Collapsed = true;
            }
        }

        private ExPNLData _positionTypes;

        public ExPNLData PositionTypes
        {
            get { return _positionTypes; }
            set
            {
                _positionTypes = value;
                SetAccountStrategyNameCol();
            }
        }

        private string _parentKey;

        public string ParentKey
        {
            get { return _parentKey; }
            set { _parentKey = value; }
        }

        public bool AddViewEnabled
        {
            set
            {
                if (contextMenuStrip != null)
                {
                    if (contextMenuStrip.Items.ContainsKey("addNewConsolidationViewToolStripMenuItem"))
                    {
                        contextMenuStrip.Items["addNewConsolidationViewToolStripMenuItem"].Enabled = value;
                    }
                }
            }
        }

        public bool DeleteViewEnabled
        {
            set
            {
                if (contextMenuStrip != null)
                {
                    if (contextMenuStrip.Items.ContainsKey("deleteViewToolStripMenuItem"))
                    {
                        contextMenuStrip.Items["deleteViewToolStripMenuItem"].Enabled = value;
                    }
                }
            }
        }

        public bool RenameViewEnabled
        {
            set
            {
                if (contextMenuStrip != null)
                {
                    if (contextMenuStrip.Items.ContainsKey("renameViewToolStripMenuItem"))
                    {
                        contextMenuStrip.Items["renameViewToolStripMenuItem"].Enabled = value;
                    }
                }
            }
        }

        private void grdConsolidation_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                SetValueForNullableColumns(PMConstants.COL_AvgPrice, false);
                SetValueForNullableColumns(PMConstants.COL_FXRate, false);
                SetValueForNullableColumns(PMConstants.COL_ClosingPrice, false);
                SetValueForNullableColumns(PMConstants.COL_CostBasisBreakEven, false);
                SetValueForNullableColumns(PMConstants.COL_FXRateDisplay, false);
                SetValueForNullableColumns(PMConstants.COL_TradeDate, true);
                SetValueForNullableColumns(PMConstants.COL_ExpirationDate, true);

                _negativeAppearace = e.Layout.Appearances.Add("Negative");
                _positiveAppearace = e.Layout.Appearances.Add("Positive");
                _neutralAppearace = e.Layout.Appearances.Add("Neutral");
                LoadPreferencesAndColumns();
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

        public void LoadPreferencesAndColumns([Optional, DefaultParameterValue(null)] CustomViewPreferences customViewPreferences)
        {
            try
            {
                if (customViewPreferences != null)
                {
                    _customViewPreference = customViewPreferences;
                    grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Clear();
                }
                int rowPaddingNumber;
                UltraGridLayout gridLayout = grdConsolidation.DisplayLayout;
                string rowPadding = ConfigurationHelper.Instance.GetAppSettingValueByKey("RowPaddingOnPM");
                if (!string.IsNullOrEmpty(rowPadding) && int.TryParse(rowPadding, out rowPaddingNumber))
                    gridLayout.Override.CellPadding = rowPaddingNumber;
                else
                    gridLayout.Override.CellPadding = 0;
                if (_pmAppearances.RowColorbasis.Equals("0"))
                {
                    _negativeAppearace.ForeColor = Color.FromArgb(_pmAppearances.OrderSideSellColor);
                    _positiveAppearace.ForeColor = Color.FromArgb(_pmAppearances.OrderSideBuyColor);
                }
                else if (_pmAppearances.RowColorbasis.Equals("1"))
                {
                    _negativeAppearace.ForeColor = Color.FromArgb(_pmAppearances.DayPnlNegativeColor);
                    // Create an appearance for positive values
                    _positiveAppearace.ForeColor = Color.FromArgb(_pmAppearances.DayPnlPositiveColor);
                }
                SetGridBackColor();
                _neutralAppearace.ForeColor = !CustomThemeHelper.ApplyTheme ? Color.White : Color.Black;
                UpdateGridPreferences(false, false);

                #region Visible Columns

                gridLayout.Bands[0].Columns[PMConstants.COL_PricingSource].Header.Caption = PMConstants.CAP_PricingSource;
                gridLayout.Bands[0].Columns[PMConstants.COL_PricingSource].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_DeltaSource].Header.Caption = PMConstants.CAP_DeltaSource;
                gridLayout.Bands[0].Columns[PMConstants.COL_DeltaSource].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_ForwardPoints].Header.Caption = PMConstants.CAP_ForwardPoints;
                gridLayout.Bands[0].Columns[PMConstants.COL_ForwardPoints].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_Symbol].Header.Caption = PMConstants.CAP_Symbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_Symbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_ProxySymbol].Header.Caption = PMConstants.CAP_ProxySymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_ProxySymbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPrice].Header.Caption = PMConstants.CAP_SelectedFeedPrice;
                gridLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPrice].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_SelectedFeedPrice);

                gridLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPriceInBaseCurrency].Header.Caption = PMConstants.CAP_SelectedFeedPriceInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPriceInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPriceInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_SelectedFeedPriceInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingStockPrice].Header.Caption = PMConstants.CAP_UnderlyingStockPrice;
                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingStockPrice].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingStockPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_UnderlyingStockPrice);

                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingValueForOptions].Header.Caption = PMConstants.CAP_UnderlyingValueForOptions;
                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingValueForOptions].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_UnderlyingValueForOptions);

                gridLayout.Bands[0].Columns[PMConstants.COL_FullSecurityName].Header.Caption = PMConstants.CAP_FullSecurityName;
                gridLayout.Bands[0].Columns[PMConstants.COL_FullSecurityName].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_MasterFund].Header.Caption = CachedDataManager.GetInstance.IsShowmasterFundAsClient() ? PMConstants.CAP_CLIENT : PMConstants.CAP_MasterFund;


                gridLayout.Bands[0].Columns[PMConstants.COL_SettlementDate].Header.Caption = PMConstants.CAP_SettlementDate;
                gridLayout.Bands[0].Columns[PMConstants.COL_SettlementDate].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_SettlementDate].Format = DateTimeConstants.NirvanaDateTimeFormat;

                gridLayout.Bands[0].Columns[PMConstants.COL_TradeDayPnl].Header.Caption = PMConstants.CAP_TradeDayPnl;
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeDayPnl].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeDayPnl].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_TradeDayPnl);

                gridLayout.Bands[0].Columns[PMConstants.COL_FxDayPnl].Header.Caption = PMConstants.CAP_FxDayPnl;
                gridLayout.Bands[0].Columns[PMConstants.COL_FxDayPnl].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_FxDayPnl].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_FxDayPnl);

                gridLayout.Bands[0].Columns[PMConstants.COL_FxCostBasisPnl].Header.Caption = PMConstants.CAP_FxCostBasisPnl;
                gridLayout.Bands[0].Columns[PMConstants.COL_FxCostBasisPnl].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_FxCostBasisPnl].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_FxCostBasisPnl);

                gridLayout.Bands[0].Columns[PMConstants.COL_TradeCostBasisPnl].Header.Caption = PMConstants.CAP_TradeCostBasisPnl;
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeCostBasisPnl].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeCostBasisPnl].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_TradeCostBasisPnl);

                gridLayout.Bands[0].Columns[PMConstants.COL_IDCOSymbol].Header.Caption = PMConstants.CAP_IDCOSymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_IDCOSymbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_OSISymbol].Header.Caption = PMConstants.CAP_OSISymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_OSISymbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_SEDOLSymbol].Header.Caption = PMConstants.CAP_SEDOLSymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_SEDOLSymbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CUSIPSymbol].Header.Caption = PMConstants.CAP_CUSIPSymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_CUSIPSymbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_BloombergSymbol].Header.Caption = PMConstants.CAP_BloombergSymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_BloombergSymbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_BloombergSymbolWithExchangeCode].Header.Caption = PMConstants.CAP_BloombergSymbolWithExchangeCode;
                gridLayout.Bands[0].Columns[PMConstants.COL_BloombergSymbolWithExchangeCode].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_FactSetSymbol].Header.Caption = PMConstants.CAP_FactSetSymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_FactSetSymbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_ActivSymbol].Header.Caption = PMConstants.CAP_ActivSymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_ActivSymbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_ISINSymbol].Header.Caption = PMConstants.CAP_ISINSymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_ISINSymbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_ReutersSymbol].Header.Caption = PMConstants.CAP_ReutersSymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_ReutersSymbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_StartTradeDate].Header.Caption = PMConstants.CAP_StartTradeDate;
                gridLayout.Bands[0].Columns[PMConstants.COL_StartTradeDate].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_StartTradeDate].Format = DateTimeConstants.DateFormat;

                gridLayout.Bands[0].Columns[PMConstants.COL_CostBasisBreakEven].Header.Caption = PMConstants.CAP_CostBasisBreakEven;
                gridLayout.Bands[0].Columns[PMConstants.COL_CostBasisBreakEven].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_PositionSideExposureBoxed].Header.Caption = PMConstants.CAP_PositionSideExposureBoxed;
                gridLayout.Bands[0].Columns[PMConstants.COL_PositionSideExposureBoxed].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_ExDividendDate].Header.Caption = PMConstants.CAP_ExDividendDate;
                gridLayout.Bands[0].Columns[PMConstants.COL_ExDividendDate].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_ExDividendDate].Format = "G";

                gridLayout.Bands[0].Columns[PMConstants.COL_InternalComments].Header.Caption = PMConstants.CAP_InternalComments;

                gridLayout.Bands[0].Columns[PMConstants.COL_Quantity].Header.Caption = PMConstants.CAP_Quantity;
                gridLayout.Bands[0].Columns[PMConstants.COL_Quantity].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_Quantity].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_Quantity);

                gridLayout.Bands[0].Columns[PMConstants.COL_AvgPrice].Header.Caption = PMConstants.CAP_AvgPrice;
                gridLayout.Bands[0].Columns[PMConstants.COL_AvgPrice].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_AvgPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_AvgPrice);

                gridLayout.Bands[0].Columns[PMConstants.COL_AverageVolume20Day].Header.Caption = PMConstants.CAP_AverageVolume20Day;
                gridLayout.Bands[0].Columns[PMConstants.COL_AverageVolume20Day].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_AverageVolume20Day].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_AverageVolume20Day);

                gridLayout.Bands[0].Columns[PMConstants.COL_AverageVolume20DayUnderlyingSymbol].Header.Caption = PMConstants.CAP_AverageVolume20DayUnderlyingSymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_AverageVolume20DayUnderlyingSymbol].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_AverageVolume20DayUnderlyingSymbol);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageAverageVolumeDeltaAdjusted].Header.Caption = PMConstants.CAP_PercentageAverageVolumeDeltaAdjusted;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageAverageVolumeDeltaAdjusted].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageAverageVolumeDeltaAdjusted].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentageAverageVolumeDeltaAdjusted);

                gridLayout.Bands[0].Columns[PMConstants.COL_SharesOutstanding].Header.Caption = PMConstants.CAP_SharesOutstanding;
                gridLayout.Bands[0].Columns[PMConstants.COL_SharesOutstanding].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_SharesOutstanding].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_SharesOutstanding);

                gridLayout.Bands[0].Columns[PMConstants.COL_MarketCapitalization].Header.Caption = PMConstants.CAP_MarketCapitalization;
                gridLayout.Bands[0].Columns[PMConstants.COL_MarketCapitalization].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_MarketCapitalization].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_MarketCapitalization);

                gridLayout.Bands[0].Columns[PMConstants.COL_OrderSideTagValue].Header.Caption = PMConstants.CAP_OrderSideTagValue;
                gridLayout.Bands[0].Columns[PMConstants.COL_OrderSideTagValue].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_OrderSideTagValue].Hidden = true;
                gridLayout.Bands[0].Columns[PMConstants.COL_OrderSideTagValue].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                gridLayout.Bands[0].Columns[PMConstants.COL_SideMultiplier].Header.Caption = PMConstants.CAP_SideMultiplier;
                gridLayout.Bands[0].Columns[PMConstants.COL_SideMultiplier].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageAverageVolume].Header.Caption = PMConstants.CAP_PercentageAverageVolume;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageAverageVolume].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageAverageVolume].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentageAverageVolume);

                gridLayout.Bands[0].Columns[PMConstants.COL_NetExposure].Header.Caption = PMConstants.CAP_NetExposure;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetExposure].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetExposure].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_NetExposure);

                gridLayout.Bands[0].Columns[PMConstants.COL_Exposure].Header.Caption = PMConstants.CAP_Exposure;
                gridLayout.Bands[0].Columns[PMConstants.COL_Exposure].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_Exposure].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_Exposure);

                gridLayout.Bands[0].Columns[PMConstants.COL_ExposureInBaseCurrency].Header.Caption = PMConstants.CAP_ExposureInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_ExposureInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_ExposureInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_ExposureInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_DeltaAdjPosition].Header.Caption = PMConstants.CAP_DeltaAdjPosition;
                gridLayout.Bands[0].Columns[PMConstants.COL_DeltaAdjPosition].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_DeltaAdjPosition].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DeltaAdjPosition);

                gridLayout.Bands[0].Columns[PMConstants.COL_DayPnL].Header.Caption = PMConstants.CAP_DayPnL;
                gridLayout.Bands[0].Columns[PMConstants.COL_DayPnL].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_DayPnL].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DayPnL);

                gridLayout.Bands[0].Columns[PMConstants.COL_NetExposureInBaseCurrency].Header.Caption = PMConstants.CAP_NetExposureInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetExposureInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetExposureInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_NetExposureInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_GrossExposure].Header.Caption = PMConstants.CAP_GrossExposure;
                gridLayout.Bands[0].Columns[PMConstants.COL_GrossExposure].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_GrossExposure].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_GrossExposure);

                gridLayout.Bands[0].Columns[PMConstants.COL_DayPnLInBaseCurrency].Header.Caption = PMConstants.CAP_DayPnLInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_DayPnLInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_DayPnLInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DayPnLInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_TradeDate].Header.Caption = PMConstants.CAP_TradeDate;
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeDate].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeDate].Format = DateTimeConstants.NirvanaDateTimeFormat;

                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayMarkPriceStr].Header.Caption = PMConstants.CAP_YesterdayMarkPriceStr;
                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayMarkPriceStr].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayMarkPriceStr].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_YesterdayMarkPriceStr);

                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayUnderlyingMarkPriceStr].Header.Caption = PMConstants.CAP_YesterdayUnderlyingMarkPriceStr;
                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayUnderlyingMarkPriceStr].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayUnderlyingMarkPriceStr].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_YesterdayUnderlyingMarkPriceStr);

                gridLayout.Bands[0].Columns[PMConstants.COL_LastPrice].Header.Caption = PMConstants.CAP_LastPrice;
                gridLayout.Bands[0].Columns[PMConstants.COL_LastPrice].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_LastPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_LastPrice);

                gridLayout.Bands[0].Columns[PMConstants.COL_Delta].Header.Caption = PMConstants.CAP_Delta;
                gridLayout.Bands[0].Columns[PMConstants.COL_Delta].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_Delta].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_Delta);

                gridLayout.Bands[0].Columns[PMConstants.COL_FXRate].Header.Caption = PMConstants.CAP_FXRate;
                gridLayout.Bands[0].Columns[PMConstants.COL_FXRate].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_FXRate].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_FXRate);

                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayFXRate].Header.Caption = PMConstants.CAP_YesterdayFXRate;
                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayFXRate].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayFXRate].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_YesterdayFXRate);

                gridLayout.Bands[0].Columns[PMConstants.COL_VsCurrencySymbol].Header.Caption = PMConstants.CAP_VsCurrencySymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_VsCurrencySymbol].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_VsCurrencySymbol].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_VsCurrencySymbol);

                gridLayout.Bands[0].Columns[PMConstants.COL_LeadCurrencySymbol].Header.Caption = PMConstants.CAP_LeadCurrencySymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_LeadCurrencySymbol].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_LeadCurrencySymbol].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_LeadCurrencySymbol);

                gridLayout.Bands[0].Columns[OrderFields.PROPERTY_LEVEL1NAME].Header.Caption = OrderFields.CAPTION_LEVEL1NAME;
                gridLayout.Bands[0].Columns[OrderFields.PROPERTY_LEVEL1NAME].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[OrderFields.PROPERTY_LEVEL2NAME].Header.Caption = OrderFields.CAPTION_LEVEL2NAME;
                gridLayout.Bands[0].Columns[OrderFields.PROPERTY_LEVEL2NAME].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_Multiplier].Header.Caption = PMConstants.CAP_Multiplier;
                gridLayout.Bands[0].Columns[PMConstants.COL_Multiplier].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_Multiplier].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_Multiplier);

                gridLayout.Bands[0].Columns[PMConstants.COL_CurrencySymbol].Header.Caption = PMConstants.CAP_CurrencySymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_CurrencySymbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_UserName].Header.Caption = PMConstants.CAP_UserName;
                gridLayout.Bands[0].Columns[PMConstants.COL_UserName].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CounterPartyName].Header.Caption = PMConstants.CAP_CounterPartyName;
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterPartyName].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_ContractType].Header.Caption = PMConstants.CAP_ContractType;
                gridLayout.Bands[0].Columns[PMConstants.COL_ContractType].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_StrikePrice].Header.Caption = PMConstants.CAP_StrikePrice;
                gridLayout.Bands[0].Columns[PMConstants.COL_StrikePrice].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_StrikePrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_StrikePrice);

                gridLayout.Bands[0].Columns[PMConstants.COL_ExpirationDate].Header.Caption = PMConstants.CAP_ExpirationDate;
                gridLayout.Bands[0].Columns[PMConstants.COL_ExpirationDate].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_ExpirationDate].Format = DateTimeConstants.NirvanaDateTimeFormat;

                gridLayout.Bands[0].Columns[PMConstants.COL_SideName].Header.Caption = PMConstants.CAP_SideName;
                gridLayout.Bands[0].Columns[PMConstants.COL_SideName].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_Asset].Header.Caption = PMConstants.CAP_Asset;
                gridLayout.Bands[0].Columns[PMConstants.COL_Asset].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_Underlying].Header.Caption = PMConstants.CAP_Underlying;
                gridLayout.Bands[0].Columns[PMConstants.COL_Underlying].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_Exchange].Header.Caption = PMConstants.CAP_Exchange;
                gridLayout.Bands[0].Columns[PMConstants.COL_Exchange].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_ClosingPrice].Header.Caption = PMConstants.CAP_ClosingPrice;
                gridLayout.Bands[0].Columns[PMConstants.COL_ClosingPrice].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_ClosingPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_ClosingPrice);

                gridLayout.Bands[0].Columns[PMConstants.COL_DividendYield].Header.Caption = PMConstants.CAP_DividendYield;
                gridLayout.Bands[0].Columns[PMConstants.COL_DividendYield].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_DividendYield].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DividendYield);

                gridLayout.Bands[0].Columns[PMConstants.COL_BidPrice].Header.Caption = PMConstants.CAP_BidPrice;
                gridLayout.Bands[0].Columns[PMConstants.COL_BidPrice].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_BidPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_BidPrice);

                gridLayout.Bands[0].Columns[PMConstants.COL_MidPrice].Header.Caption = PMConstants.CAP_MidPrice;
                gridLayout.Bands[0].Columns[PMConstants.COL_MidPrice].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_MidPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_MidPrice);

                gridLayout.Bands[0].Columns[PMConstants.COL_AskPrice].Header.Caption = PMConstants.CAP_AskPrice;
                gridLayout.Bands[0].Columns[PMConstants.COL_AskPrice].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_AskPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_AskPrice);

                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingSymbol].Header.Caption = PMConstants.CAP_UnderlyingSymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingSymbol].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_DataSourceNameIDValue].Header.Caption = PMConstants.CAP_DataSourceNameIDValue;
                gridLayout.Bands[0].Columns[PMConstants.COL_DataSourceNameIDValue].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_FXRateOnTradeDateStr].Header.Caption = PMConstants.CAP_FXRateOnTradeDateStr;
                gridLayout.Bands[0].Columns[PMConstants.COL_FXRateOnTradeDateStr].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_FXRateOnTradeDateStr].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_FXRateOnTradeDateStr);

                gridLayout.Bands[0].Columns[PMConstants.COL_CostBasisUnRealizedPNL].Header.Caption = PMConstants.CAP_CostBasisUnrealizedPnL;
                gridLayout.Bands[0].Columns[PMConstants.COL_CostBasisUnRealizedPNL].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_CostBasisUnRealizedPNL].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CostBasisUnrealizedPnL);

                gridLayout.Bands[0].Columns[PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency].Header.Caption = PMConstants.CAP_CostBasisUnrealizedPnLInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CostBasisUnrealizedPnLInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_EarnedDividendLocal].Header.Caption = PMConstants.CAP_EarnedDividendLocal;
                gridLayout.Bands[0].Columns[PMConstants.COL_EarnedDividendLocal].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_EarnedDividendLocal].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_EarnedDividendLocal);

                gridLayout.Bands[0].Columns[PMConstants.COL_EarnedDividendBase].Header.Caption = PMConstants.CAP_EarnedDividendBase;
                gridLayout.Bands[0].Columns[PMConstants.COL_EarnedDividendBase].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_EarnedDividendBase].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_EarnedDividendBase);

                gridLayout.Bands[0].Columns[PMConstants.COL_GrossExposureLocal].Header.Caption = PMConstants.CAP_GrossExposureLocal;
                gridLayout.Bands[0].Columns[PMConstants.COL_GrossExposureLocal].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_GrossExposureLocal].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_GrossExposureLocal);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentDayPnLGrossMV].Header.Caption = PMConstants.CAP_PercentDayPnLGrossMV;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentDayPnLGrossMV].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentDayPnLGrossMV);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentDayPnLNetMV].Header.Caption = PMConstants.CAP_PercentDayPnLNetMV;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentDayPnLNetMV].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentDayPnLNetMV);

                gridLayout.Bands[0].Columns[PMConstants.COL_ExpirationMonth].Header.Caption = PMConstants.CAP_ExpirationMonth;
                gridLayout.Bands[0].Columns[PMConstants.COL_ExpirationMonth].Format = "MMMM yyyy";
                gridLayout.Bands[0].Columns[PMConstants.COL_ExpirationMonth].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_DeltaAdjPositionLME].Header.Caption = PMConstants.CAP_DeltaAdjPositionLME;
                gridLayout.Bands[0].Columns[PMConstants.COL_DeltaAdjPositionLME].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DeltaAdjPositionLME);

                gridLayout.Bands[0].Columns[PMConstants.COL_Premium].Header.Caption = PMConstants.CAP_Premium;
                gridLayout.Bands[0].Columns[PMConstants.COL_Premium].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_Premium);

                gridLayout.Bands[0].Columns[PMConstants.COL_PremiumDollar].Header.Caption = PMConstants.CAP_PremiumDollar;
                gridLayout.Bands[0].Columns[PMConstants.COL_PremiumDollar].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PremiumDollar);

                gridLayout.Bands[0].Columns[PMConstants.COL_TransactionType].Header.Caption = PMConstants.CAP_TransactionType;
                gridLayout.Bands[0].Columns[PMConstants.COL_TransactionType].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_TransactionSide].Header.Caption = PMConstants.CAP_TransactionSide;
                gridLayout.Bands[0].Columns[PMConstants.COL_TransactionSide].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_UDAAsset].Header.Caption = PMConstants.CAP_UDAAsset;
                gridLayout.Bands[0].Columns[PMConstants.COL_UDAAsset].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_UDACountry].Header.Caption = PMConstants.CAP_UDACountry;
                gridLayout.Bands[0].Columns[PMConstants.COL_UDACountry].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_UDASector].Header.Caption = PMConstants.CAP_UDASector;
                gridLayout.Bands[0].Columns[PMConstants.COL_UDASector].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_UDASecurityType].Header.Caption = PMConstants.CAP_UDASecurityType;
                gridLayout.Bands[0].Columns[PMConstants.COL_UDASecurityType].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_UDASubSector].Header.Caption = PMConstants.CAP_UDASubSector;
                gridLayout.Bands[0].Columns[PMConstants.COL_UDASubSector].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_DayInterest].Header.Caption = PMConstants.CAP_DayInterest;
                gridLayout.Bands[0].Columns[PMConstants.COL_DayInterest].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_DayInterest].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DayInterest);

                gridLayout.Bands[0].Columns[PMConstants.COL_TotalInterest].Header.Caption = PMConstants.CAP_TotalInterest;
                gridLayout.Bands[0].Columns[PMConstants.COL_TotalInterest].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_TotalInterest].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_TotalInterest);

                gridLayout.Bands[0].Columns[PMConstants.COL_MarketValue].Header.Caption = PMConstants.CAP_MarketValue;
                gridLayout.Bands[0].Columns[PMConstants.COL_MarketValue].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_MarketValue].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_MarketValue);

                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayMarketValue].Header.Caption = PMConstants.CAP_YesterdayMarketValue;
                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayMarketValue].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayMarketValue].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_YesterdayMarketValue);

                gridLayout.Bands[0].Columns[PMConstants.COL_MarketValueInBaseCurrency].Header.Caption = PMConstants.CAP_MarketValueInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_MarketValueInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_MarketValueInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_MarketValueInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayMarketValueInBaseCurrency].Header.Caption = PMConstants.CAP_YesterdayMarketValueInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayMarketValueInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_YesterdayMarketValueInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_YesterdayMarketValueInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_GrossMarketValue].Header.Caption = PMConstants.CAP_GrossMarketValue;
                gridLayout.Bands[0].Columns[PMConstants.COL_GrossMarketValue].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_GrossMarketValue].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_GrossMarketValue);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageGainLoss].Header.Caption = PMConstants.CAP_PercentageGainLoss;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageGainLoss].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageGainLoss].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentageGainLoss);

                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjExposure].Header.Caption = PMConstants.CAP_BetaAdjExposure;
                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjExposure].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjExposure].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_BetaAdjExposure);

                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjExposureInBaseCurrency].Header.Caption = PMConstants.CAP_BetaAdjExposureInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjExposureInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjExposureInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_BetaAdjExposureInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjGrossExposure].Header.Caption = PMConstants.CAP_BetaAdjGrossExposure;
                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjGrossExposure].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjGrossExposure].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_BetaAdjGrossExposure);

                gridLayout.Bands[0].Columns[PMConstants.COL_Volatility].Header.Caption = PMConstants.CAP_Volatility;
                gridLayout.Bands[0].Columns[PMConstants.COL_Volatility].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_Volatility].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_Volatility);

                gridLayout.Bands[0].Columns[PMConstants.COL_LastUpdatedUTC].Header.Caption = PMConstants.CAP_LastUpdatedUTC;
                gridLayout.Bands[0].Columns[PMConstants.COL_LastUpdatedUTC].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_LastUpdatedUTC].Format = DateTimeConstants.NirvanaDateTimeFormat;

                gridLayout.Bands[0].Columns[PMConstants.COL_ExposureBPInBaseCurrency].Header.Caption = PMConstants.CAP_ExposureBPInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_ExposureBPInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_ExposureBPInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_ExposureBPInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_Beta].Header.Caption = PMConstants.CAP_Beta;
                gridLayout.Bands[0].Columns[PMConstants.COL_Beta].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_Beta].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_Beta);

                gridLayout.Bands[0].Columns[PMConstants.COL_CashImpact].Header.Caption = PMConstants.CAP_CashImpact;
                gridLayout.Bands[0].Columns[PMConstants.COL_CashImpact].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_CashImpact].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CashImpact);

                gridLayout.Bands[0].Columns[PMConstants.COL_CashImpactInBaseCurrency].Header.Caption = PMConstants.CAP_CashImpactInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_CashImpactInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_CashImpactInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CashImpactInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageChange].Header.Caption = PMConstants.CAP_PercentageChange;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageChange].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageChange].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentageChange);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageUnderlyingChange].Header.Caption = PMConstants.CAP_PercentageUnderlyingChange;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageUnderlyingChange].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageUnderlyingChange].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentageUnderlyingChange);

                gridLayout.Bands[0].Columns[PMConstants.COL_ChangeInUnderlyingPrice].Header.Caption = PMConstants.CAP_ChangeInUnderlyingPrice;
                gridLayout.Bands[0].Columns[PMConstants.COL_ChangeInUnderlyingPrice].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_ChangeInUnderlyingPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_ChangeInUnderlyingPrice);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageGainLossCostBasis].Header.Caption = PMConstants.CAP_PercentageGainLossCostBasis;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageGainLossCostBasis].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentageGainLossCostBasis].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentageGainLossCostBasis);

                gridLayout.Bands[0].Columns[PMConstants.COL_LeveragedFactor].Header.Caption = PMConstants.CAP_LeveragedFactor;
                gridLayout.Bands[0].Columns[PMConstants.COL_LeveragedFactor].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_LeveragedFactor].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_LeveragedFactor);

                gridLayout.Bands[0].Columns[PMConstants.COL_TradeAttribute1].Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(PMConstants.CAP_TradeAttribute1);
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeAttribute2].Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(PMConstants.CAP_TradeAttribute2);
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeAttribute3].Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(PMConstants.CAP_TradeAttribute3);
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeAttribute4].Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(PMConstants.CAP_TradeAttribute4);
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeAttribute5].Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(PMConstants.CAP_TradeAttribute5);
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeAttribute6].Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(PMConstants.CAP_TradeAttribute6);

                for (int i = 7; i <= 45; i++)
                {
                    gridLayout.Bands[0].Columns[PMConstants.COL_TradeAttribute + i].Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(PMConstants.CAP_TradeAttribute + i);
                }

                #region MasterStrategyAssociation
                gridLayout.Bands[0].Columns[PMConstants.COL_MasterStrategy].Header.Caption = PMConstants.CAP_MasterStrategy;
                gridLayout.Bands[0].Columns[PMConstants.COL_MasterStrategy].CellAppearance.TextHAlign = HAlign.Right;

                bool showMasterStrategy = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("MasterStrategyAssociation"));
                if (showMasterStrategy)
                {
                    gridLayout.Bands[0].Columns[PMConstants.COL_MasterStrategy].Hidden = false;
                    gridLayout.Bands[0].Columns[PMConstants.COL_MasterStrategy].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                else
                {
                    gridLayout.Bands[0].Columns[PMConstants.COL_MasterStrategy].Hidden = true;
                    gridLayout.Bands[0].Columns[PMConstants.COL_MasterStrategy].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }

                #endregion MasterStrategyAssociation

                gridLayout.Bands[0].Columns[PMConstants.COL_Analyst].Header.Caption = PMConstants.CAP_Analyst;
                gridLayout.Bands[0].Columns[PMConstants.COL_Analyst].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CountryOfRisk].Header.Caption = PMConstants.CAP_CountryOfRisk;
                gridLayout.Bands[0].Columns[PMConstants.COL_CountryOfRisk].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA1].Header.Caption = _dynamicUDACache[PMConstants.COL_CustomUDA1].HeaderCaption.ToString();
                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA1].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA2].Header.Caption = _dynamicUDACache[PMConstants.COL_CustomUDA2].HeaderCaption.ToString();
                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA2].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA3].Header.Caption = _dynamicUDACache[PMConstants.COL_CustomUDA3].HeaderCaption.ToString();
                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA3].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA4].Header.Caption = _dynamicUDACache[PMConstants.COL_CustomUDA4].HeaderCaption.ToString();
                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA4].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA5].Header.Caption = _dynamicUDACache[PMConstants.COL_CustomUDA5].HeaderCaption.ToString();
                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA5].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA6].Header.Caption = _dynamicUDACache[PMConstants.COL_CustomUDA6].HeaderCaption.ToString();
                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA6].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA7].Header.Caption = _dynamicUDACache[PMConstants.COL_CustomUDA7].HeaderCaption.ToString();
                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA7].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA8].Header.Caption = _dynamicUDACache[PMConstants.COL_CustomUDA8].HeaderCaption.ToString();
                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA8].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA9].Header.Caption = _dynamicUDACache[PMConstants.COL_CustomUDA9].HeaderCaption.ToString();
                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA9].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA10].Header.Caption = _dynamicUDACache[PMConstants.COL_CustomUDA10].HeaderCaption.ToString();
                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA10].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA11].Header.Caption = _dynamicUDACache[PMConstants.COL_CustomUDA11].HeaderCaption.ToString();
                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA11].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA12].Header.Caption = _dynamicUDACache[PMConstants.COL_CustomUDA12].HeaderCaption.ToString();
                gridLayout.Bands[0].Columns[PMConstants.COL_CustomUDA12].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_Issuer].Header.Caption = PMConstants.CAP_Issuer;
                gridLayout.Bands[0].Columns[PMConstants.COL_Issuer].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_LiquidTag].Header.Caption = PMConstants.CAP_LiquidTag;
                gridLayout.Bands[0].Columns[PMConstants.COL_LiquidTag].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_MarketCap].Header.Caption = PMConstants.CAP_MarketCap;
                gridLayout.Bands[0].Columns[PMConstants.COL_MarketCap].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_Region].Header.Caption = PMConstants.CAP_Region;
                gridLayout.Bands[0].Columns[PMConstants.COL_Region].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_RiskCurrency].Header.Caption = PMConstants.CAP_RiskCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_RiskCurrency].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_UcitsEligibleTag].Header.Caption = PMConstants.CAP_UcitsEligibleTag;
                gridLayout.Bands[0].Columns[PMConstants.COL_UcitsEligibleTag].CellAppearance.TextHAlign = HAlign.Right;

                gridLayout.Bands[0].Columns[PMConstants.COL_FXRateDisplay].Header.Caption = PMConstants.CAP_FXRateDisplay;
                gridLayout.Bands[0].Columns[PMConstants.COL_FXRateDisplay].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_FXRateDisplay].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_FXRateDisplay);

                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalForCostBasisBreakEven].Header.Caption = PMConstants.CAP_NetNotionalForCostBasisBreakEven;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalForCostBasisBreakEven].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalForCostBasisBreakEven].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_NetNotionalForCostBasisBreakEven);
                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalForCostBasisBreakEven].Hidden = true;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalForCostBasisBreakEven].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentNetExposureInBaseCurrency].Header.Caption = PMConstants.CAP_PercentNetExposureInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentNetExposureInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentNetExposureInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentNetExposureInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentGrossExposureInBaseCurrency].Header.Caption = PMConstants.CAP_PercentGrossExposureInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentGrossExposureInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentGrossExposureInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentGrossExposureInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentExposureInBaseCurrency].Header.Caption = PMConstants.CAP_PercentExposureInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentExposureInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentExposureInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentExposureInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentUnderlyingGrossExposureInBaseCurrency].Header.Caption = PMConstants.CAP_PercentUnderlyingGrossExposureInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentUnderlyingGrossExposureInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentUnderlyingGrossExposureInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentUnderlyingGrossExposureInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentBetaAdjGrossExposureInBaseCurrency].Header.Caption = PMConstants.CAP_PercentBetaAdjGrossExposureInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentBetaAdjGrossExposureInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentBetaAdjGrossExposureInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentBetaAdjGrossExposureInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_PositionSideExposureUnderlying].Header.Caption = PMConstants.CAP_PositionSideExposureUnderlying;
                gridLayout.Bands[0].Columns[PMConstants.COL_PositionSideExposureUnderlying].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PositionSideExposureUnderlying].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PositionSideExposureUnderlying);

                gridLayout.Bands[0].Columns[PMConstants.COL_PositionSideMV].Header.Caption = PMConstants.CAP_PositionSideMV;
                gridLayout.Bands[0].Columns[PMConstants.COL_PositionSideMV].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PositionSideMV].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PositionSideMV);

                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingGrossExposureInBaseCurrency].Header.Caption = PMConstants.CAP_UnderlyingGrossExposureInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingGrossExposureInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingGrossExposureInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_UnderlyingGrossExposureInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingGrossExposure].Header.Caption = PMConstants.CAP_UnderlyingGrossExposure;
                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingGrossExposure].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_UnderlyingGrossExposure].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_UnderlyingGrossExposure);

                gridLayout.Bands[0].Columns[PMConstants.COL_DayReturn].Header.Caption = PMConstants.CAP_DayReturn;
                gridLayout.Bands[0].Columns[PMConstants.COL_DayReturn].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_DayReturn].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DayReturn);

                gridLayout.Bands[0].Columns[PMConstants.COL_StartOfDayNAV].Header.Caption = PMConstants.CAP_StartOfDayNAV;
                gridLayout.Bands[0].Columns[PMConstants.COL_StartOfDayNAV].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_StartOfDayNAV].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_StartOfDayNAV);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentGrossMarketValueInBaseCurrency].Header.Caption = PMConstants.CAP_PercentGrossMarketValueInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentGrossMarketValueInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentGrossMarketValueInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentGrossMarketValueInBaseCurrency);


                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjGrossExposureUnderlying].Header.Caption = PMConstants.CAP_BetaAdjGrossExposureUnderlying;
                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjGrossExposureUnderlying].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjGrossExposureUnderlying].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_BetaAdjGrossExposureUnderlying);

                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjGrossExposureUnderlyingInBaseCurrency].Header.Caption = PMConstants.CAP_BetaAdjGrossExposureUnderlyingInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjGrossExposureUnderlyingInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_BetaAdjGrossExposureUnderlyingInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_BetaAdjGrossExposureUnderlyingInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_NAV].Header.Caption = PMConstants.CAP_NAV;
                gridLayout.Bands[0].Columns[PMConstants.COL_NAV].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_NAV].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_NAV);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentagePNLContribution].Header.Caption = PMConstants.CAP_PercentagePNLContribution;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentagePNLContribution].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentagePNLContribution].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentagePNLContribution);

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentNetMarketValueInBaseCurrency].Header.Caption = PMConstants.CAP_PercentNetMarketValueInBaseCurrency;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentNetMarketValueInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentNetMarketValueInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentNetMarketValueInBaseCurrency);

                gridLayout.Bands[0].Columns[PMConstants.COL_PositionSideExposure].Header.Caption = PMConstants.CAP_PositionSideExposure;
                gridLayout.Bands[0].Columns[PMConstants.COL_PositionSideExposure].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PositionSideExposure].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PositionSideExposure);

                gridLayout.Bands[0].Columns[PMConstants.COL_NavTouch].Header.Caption = PMConstants.CAP_NavTouch;
                gridLayout.Bands[0].Columns[PMConstants.COL_NavTouch].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_NavTouch].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_NavTouch);

                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalValue].Header.Caption = PMConstants.CAP_NetNotionalValueBase;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalValue].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalValue].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_NetNotionalValueBase);
                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalValue].Hidden = true;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalValue].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalValueBase].Header.Caption = PMConstants.CAP_NetNotionalValue;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalValueBase].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalValueBase].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_NetNotionalValue);
                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalValueBase].Hidden = true;
                gridLayout.Bands[0].Columns[PMConstants.COL_NetNotionalValueBase].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencySymbol].Header.Caption = PMConstants.CAP_CounterCurrencySymbol;
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencySymbol].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencySymbol].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CounterCurrencySymbol);
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencySymbol].Hidden = true;

                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyAmount].Header.Caption = PMConstants.CAP_CounterCurrencyAmount;
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyAmount].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyAmount].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CounterCurrencyAmount);
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyAmount].Hidden = true;

                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyCostBasisPnL].Header.Caption = PMConstants.CAP_CounterCurrencyCostBasisPnL;
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyCostBasisPnL].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyCostBasisPnL].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CounterCurrencyCostBasisPnL);
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyCostBasisPnL].Hidden = true;

                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyDayPnL].Header.Caption = PMConstants.CAP_CounterCurrencyDayPNL;
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyDayPnL].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyDayPnL].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CounterCurrencyCostBasisPnL);
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyDayPnL].Hidden = true;

                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyID].Hidden = true;
                gridLayout.Bands[0].Columns[PMConstants.COL_CounterCurrencyID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                gridLayout.Bands[0].Columns[PMConstants.COL_ItmOtm].Header.Caption = PMConstants.CAP_ItmOtm;
                gridLayout.Bands[0].Columns[PMConstants.COL_ItmOtm].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_ItmOtm].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_ItmOtm);
                gridLayout.Bands[0].Columns[PMConstants.COL_ItmOtm].Hidden = true;

                gridLayout.Bands[0].Columns[PMConstants.COL_PercentOfITMOTM].Header.Caption = PMConstants.CAP_PercentOfITMOTM;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentOfITMOTM].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentOfITMOTM].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentOfITMOTM);
                gridLayout.Bands[0].Columns[PMConstants.COL_PercentOfITMOTM].Hidden = true;

                gridLayout.Bands[0].Columns[PMConstants.COL_IntrinsicValue].Header.Caption = PMConstants.CAP_IntrinsicValue;
                gridLayout.Bands[0].Columns[PMConstants.COL_IntrinsicValue].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_IntrinsicValue].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_IntrinsicValue);
                gridLayout.Bands[0].Columns[PMConstants.COL_IntrinsicValue].Hidden = true;

                gridLayout.Bands[0].Columns[PMConstants.COL_DaysToExpiry].Header.Caption = PMConstants.CAP_DaysToExpiry;
                gridLayout.Bands[0].Columns[PMConstants.COL_DaysToExpiry].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_DaysToExpiry].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DaysToExpiry);
                gridLayout.Bands[0].Columns[PMConstants.COL_DaysToExpiry].Hidden = true;

                gridLayout.Bands[0].Columns[PMConstants.COL_GainLossIfExerciseAssign].Header.Caption = PMConstants.CAP_GainLossIfExerciseAssign;
                gridLayout.Bands[0].Columns[PMConstants.COL_GainLossIfExerciseAssign].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_GainLossIfExerciseAssign].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_GainLossIfExerciseAssign);
                gridLayout.Bands[0].Columns[PMConstants.COL_GainLossIfExerciseAssign].Hidden = true;

                gridLayout.Bands[0].Columns[PMConstants.COL_DayTradedPosition].Header.Caption = PMConstants.CAP_DayTradedPosition;
                gridLayout.Bands[0].Columns[PMConstants.COL_DayTradedPosition].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_DayTradedPosition].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DayTradedPosition);

                gridLayout.Bands[0].Columns[PMConstants.COL_TradeVolume].Header.Caption = PMConstants.CAP_TradeVolume;
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeVolume].CellAppearance.TextHAlign = HAlign.Right;
                gridLayout.Bands[0].Columns[PMConstants.COL_TradeVolume].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_TradeVolume);

                gridLayout.Bands[0].Columns[PMConstants.COL_PricingStatus].Header.Caption = PMConstants.CAP_PricingStatus;
                gridLayout.Bands[0].Columns[PMConstants.COL_PricingStatus].CellAppearance.TextHAlign = HAlign.Right;

                SetupPricingStatusColumnVisibility(gridLayout.Bands[0].Columns[PMConstants.COL_PricingStatus]);

                #endregion Visible Columns

                if (_isGroupByAllowed)
                {
                    gridLayout.Override.AllowGroupBy = DefaultableBoolean.True;
                }
                else
                {
                    gridLayout.Override.AllowGroupBy = DefaultableBoolean.False;
                    gridLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
                }
                //if show group by box set hidden false and vice versa
                gridLayout.GroupByBox.Hidden = !_showGroupByBox;
                if (_customViewPreference != null)
                {
                    if (_customViewPreference.GroupByColumnsCollection.Count > 0)
                    {
                        //Set GroupBy Columns
                        grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Clear();
                        foreach (string item in _customViewPreference.GroupByColumnsCollection)
                        {
                            if (grdConsolidation.DisplayLayout.Bands[0].Columns.Exists(item))
                            {
                                PreferenceGridColumn prefGridCol = _customViewPreference.SelectedColumnsCollection.FirstOrDefault(x => x.Name == item);
                                if (prefGridCol.SortIndicator.Equals(SortIndicator.Ascending))
                                    grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Add(item, false, true);
                                if (prefGridCol.SortIndicator.Equals(SortIndicator.Descending))
                                    grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Add(item, true, true);

                            }
                        }
                    }
                    RowSummarySettingsForAccountView();

                    ColumnFiltersCollection columnFilters = grdConsolidation.DisplayLayout.Bands[0].ColumnFilters;
                    //TODO : When we apply the custom filters we need to change the code so the filters won't be on a common field
                    columnFilters.ClearAllFilters();

                    foreach (UltraGridColumn col in grdConsolidation.DisplayLayout.Bands[0].Columns)
                    {
                        col.Hidden = true;
                    }
                    int visibleColumns = 0;
                    foreach (PreferenceGridColumn selectedCols in _customViewPreference.SelectedColumnsCollection)
                    {
                        UltraGridColumn col;
                        if (grdConsolidation.DisplayLayout.Bands[0].Columns.Exists(selectedCols.Name))
                        {
                            col = grdConsolidation.DisplayLayout.Bands[0].Columns[selectedCols.Name];
                            if (col != null)
                            {
                                if (!selectedCols.Hidden)
                                {
                                    visibleColumns = visibleColumns + 1;
                                    col.Hidden = visibleColumns > _uiPrefs.NumberOfVisibleColumnsAllowed;
                                }
                                else
                                {
                                    col.Hidden = true;
                                }

                                if (selectedCols.Name.Equals(PMConstants.COL_PricingStatus))
                                {
                                    SetupPricingStatusColumnVisibility(col);
                                }

                                col.HiddenWhenGroupBy = DefaultableBoolean.False;
                                col.Header.Fixed = selectedCols.IsHeaderFixed;

                                if (!col.Hidden && selectedCols.SortIndicator != SortIndicator.None && !grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Contains(col))
                                {
                                    if (selectedCols.SortIndicator.Equals(SortIndicator.Ascending))
                                        grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Add(col, false);
                                    if (selectedCols.SortIndicator.Equals(SortIndicator.Descending))
                                        grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Add(col, true);
                                    CreateColumnListForGroupBySorting(col);
                                    ApplySortingAfterGroupBy(col);
                                }

                                grdConsolidation.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                                if (selectedCols.Width == 0)
                                {
                                    col.AutoSizeMode = ColumnAutoSizeMode.Default;
                                }
                                else
                                {
                                    col.Width = selectedCols.Width;
                                }
                                col.Header.VisiblePosition = selectedCols.Position;
                                if (selectedCols.FilterConditionList.Count > 0)
                                {
                                    foreach (FilterCondition filCond in selectedCols.FilterConditionList)
                                    {
                                        if ((selectedCols.Name.Equals(PMConstants.COL_TradeDate) || selectedCols.Name.Equals(PMConstants.COL_StartTradeDate) || selectedCols.Name.Equals(PMConstants.COL_ExpirationDate)) && selectedCols.FilterConditionList.Count == 1 && selectedCols.FilterConditionList[0].ComparisionOperator == FilterComparisionOperator.StartsWith && selectedCols.FilterConditionList[0].CompareValue.Equals("(Today)"))
                                        {
                                            if (selectedCols.Name.Equals(PMConstants.COL_TradeDate))
                                                grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.NirvanaDateTimeFormat_WithoutTime));
                                            else if (selectedCols.Name.Equals(PMConstants.COL_StartTradeDate))
                                                grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.DateFormat));
                                            else if (selectedCols.Name.Equals(PMConstants.COL_ExpirationDate))
                                                grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.NirvanaDateTimeFormat_WithoutTime));
                                        }
                                        else if ((selectedCols.Name.Equals(PMConstants.COL_TradeDate) || selectedCols.Name.Equals(PMConstants.COL_StartTradeDate)) && selectedCols.FilterConditionList.Count == 1 && selectedCols.FilterConditionList[0].ComparisionOperator == FilterComparisionOperator.LessThan && selectedCols.FilterConditionList[0].CompareValue.Equals("(BeforeToday)"))
                                        {
                                            grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(FilterComparisionOperator.LessThan, DateTime.Now.ToString(DateTimeConstants.DateFormat));
                                        }
                                        else
                                        {
                                            grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(filCond);
                                        }
                                    }
                                    grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[col].LogicalOperator = selectedCols.FilterLogicalOperator;
                                }
                            }
                        }
                    }

                    if (_customViewPreference.SplitterPosition != int.MinValue)
                    {
                        _splitterDistance = _customViewPreference.SplitterPosition;
                    }

                    //Bharat Kumar Jangir (09/04/2015)
                    //http://www.infragistics.com/community/forums/t/101784.aspx
                    grdConsolidation.DisplayLayout.Bands[0].Columns[PMConstants.COL_PositionSideExposureBoxed].SortComparer = new SortComparer();
                }

                ShowHideGroupLevelButtons();

                foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (var.IsGroupByColumn)
                    {
                        _count++;
                    }
                }
                _previousGroupingCount = _count;

                if (grdConsolidation.Rows.Count > 0)
                {
                    grdConsolidation.Rows.CollapseAll(true);
                }
                //ImplementAccountFilter();
                setMDColumns();

                if (_pmAppearances.WrapHeader)
                    UpdateHeaderWrapHeader(true);
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

        private static void SetupPricingStatusColumnVisibility(UltraGridColumn col)
        {
            try
            {
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI)
                {
                    col.Hidden = false;
                    col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                else
                {
                    col.Hidden = true;
                    col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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
        /// Create ColumnList For GroupBy Sorting
        /// </summary>
        /// <param name="var"></param>
        private void CreateColumnListForGroupBySorting(UltraGridColumn var)
        {
            try
            {
                if (var.ExcludeFromColumnChooser == ExcludeFromColumnChooser.Default ||
                        var.ExcludeFromColumnChooser == ExcludeFromColumnChooser.False)
                {
                    if (!grdConsolidation.DisplayLayout.Bands[0].Summaries.Exists(var.Key) || _isExportGridNeeded)
                    {
                        switch (var.Key)
                        {
                            case PMConstants.COL_DeltaAdjPosition:
                            case PMConstants.COL_NetExposure:
                            case PMConstants.COL_MarketValue:
                            case PMConstants.COL_MarketValueInBaseCurrency:
                            case PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency:
                            case PMConstants.COL_DayInterest:
                            case PMConstants.COL_TotalInterest:
                            case PMConstants.COL_CostBasisUnRealizedPNL:
                            case PMConstants.COL_BetaAdjExposure:
                            case PMConstants.COL_FxCostBasisPnl:
                            case PMConstants.COL_FxDayPnl:
                            case PMConstants.COL_TradeCostBasisPnl:
                            case PMConstants.COL_TradeDayPnl:
                            case PMConstants.COL_UnderlyingValueForOptions:
                            case PMConstants.COL_DeltaAdjPositionLME:
                            case PMConstants.COL_PremiumDollar:
                            case PMConstants.COL_StrikeGapExposure:
                            case PMConstants.COL_YesterdayMarketValue:
                            case PMConstants.COL_YesterdayMarketValueInBaseCurrency:
                            case PMConstants.COL_Exposure:
                            case PMConstants.COL_ExposureInBaseCurrency:
                            case PMConstants.COL_NetExposureInBaseCurrency:
                            case PMConstants.COL_BetaAdjExposureInBaseCurrency:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_GrossExposure:
                            case PMConstants.COL_GrossMarketValue:
                            case PMConstants.COL_GrossExposureLocal:
                            case PMConstants.COL_UnderlyingGrossExposure:
                            case PMConstants.COL_BetaAdjGrossExposureUnderlying:

                            //These are the plane summable column so should be moved to plain sum calculator moved 
                            case PMConstants.COL_PercentDayPnLGrossMV:
                            case PMConstants.COL_PercentDayPnLNetMV:
                            case PMConstants.COL_BetaAdjGrossExposure:
                            case PMConstants.COL_PercentGrossExposureInBaseCurrency:
                            case PMConstants.COL_PercentUnderlyingGrossExposureInBaseCurrency:
                            case PMConstants.COL_PercentBetaAdjGrossExposureInBaseCurrency:

                            case PMConstants.COL_PercentGrossMarketValueInBaseCurrency:
                            case PMConstants.COL_BetaAdjGrossExposureUnderlyingInBaseCurrency:
                            case PMConstants.COL_UnderlyingGrossExposureInBaseCurrency:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_DayPnL:
                            case PMConstants.COL_CashImpact:
                            case PMConstants.COL_EarnedDividendLocal:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_DayPnLInBaseCurrency:
                            case PMConstants.COL_CashImpactInBaseCurrency:
                            case PMConstants.COL_EarnedDividendBase:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_AvgPrice:
                            case PMConstants.COL_CostBasisBreakEven:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_ExposureBPInBaseCurrency:
                            case PMConstants.COL_PercentExposureInBaseCurrency:
                            case PMConstants.COL_DayReturn:
                            case PMConstants.COL_PercentagePNLContribution:
                            case PMConstants.COL_PercentNetExposureInBaseCurrency:
                            case PMConstants.COL_PercentNetMarketValueInBaseCurrency:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_PercentageAverageVolumeDeltaAdjusted:
                            case PMConstants.COL_Quantity:
                            case PMConstants.COL_PercentageAverageVolume:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_Volatility:
                                _allowedGroupedSortedColumnNumericCumText.Add(var.Key);
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_Delta:
                            case PMConstants.COL_LeveragedFactor:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_PercentageGainLoss:
                            case PMConstants.COL_PercentageGainLossCostBasis:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_NetNotionalValue:
                            case PMConstants.COL_NetNotionalForCostBasisBreakEven:
                            case PMConstants.COL_NetNotionalValueBase:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_SharesOutstanding:
                            case PMConstants.COL_AverageVolume20Day:
                            case PMConstants.COL_AverageVolume20DayUnderlyingSymbol:
                            case PMConstants.COL_MarketCapitalization:
                            case PMConstants.COL_Beta:
                            case PMConstants.COL_PercentageChange:
                            case PMConstants.COL_ClosingPrice:
                            case PMConstants.COL_LastPrice:
                            case PMConstants.COL_SelectedFeedPrice:
                            case PMConstants.COL_SelectedFeedPriceInBaseCurrency:
                            case PMConstants.COL_UnderlyingStockPrice:
                            case PMConstants.COL_Premium:
                            case PMConstants.COL_PercentageUnderlyingChange:
                            case PMConstants.COL_ChangeInUnderlyingPrice:
                            case PMConstants.COL_AskPrice:
                            case PMConstants.COL_BidPrice:
                            case PMConstants.COL_MidPrice:
                            case PMConstants.COL_DividendYield:
                            case PMConstants.COL_YesterdayFXRate:
                            case PMConstants.COL_FXRateDisplay:
                            case PMConstants.COL_FXRate:
                            case PMConstants.COL_ForwardPoints:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_Symbol:
                            case PMConstants.COL_CurrencySymbol:
                            case PMConstants.COL_FullSecurityName:
                            case OrderFields.PROPERTY_LEVEL1NAME:
                            case OrderFields.PROPERTY_LEVEL2NAME:
                            case PMConstants.COL_DataSourceNameIDValue:
                            case PMConstants.COL_Asset:
                            case PMConstants.COL_UDAAsset:
                            case PMConstants.COL_UDACountry:
                            case PMConstants.COL_UDASector:
                            case PMConstants.COL_UDASecurityType:
                            case PMConstants.COL_UDASubSector:
                            case PMConstants.COL_Underlying:
                            case PMConstants.COL_Exchange:
                            case PMConstants.COL_SideName:
                            case PMConstants.COL_TransactionSide:
                            case PMConstants.COL_VsCurrencySymbol:
                            case PMConstants.COL_MasterFund:
                            case PMConstants.COL_MasterStrategy:
                            case PMConstants.COL_UserName:
                            case PMConstants.COL_CounterPartyName:
                            case PMConstants.COL_ContractType:
                            case PMConstants.COL_LeadCurrencySymbol:
                            case PMConstants.COL_IDCOSymbol:
                            case PMConstants.COL_OSISymbol:
                            case PMConstants.COL_SEDOLSymbol:
                            case PMConstants.COL_CUSIPSymbol:
                            case PMConstants.COL_BloombergSymbol:
                            case PMConstants.COL_BloombergSymbolWithExchangeCode:
                            case PMConstants.COL_ISINSymbol:
                            case PMConstants.COL_TransactionType:
                            case PMConstants.COL_ReutersSymbol:
                            case PMConstants.COL_InternalComments:
                            case PMConstants.COL_PositionSideExposureUnderlying:
                            case PMConstants.COL_PositionSideMV:
                            case PMConstants.COL_PositionSideExposure:

                                _allowedGroupedSortedColumnText.Add(var.Key);
                                break;

                            case PMConstants.COL_YesterdayMarkPriceStr:
                            case PMConstants.COL_FXRateOnTradeDateStr:
                            case PMConstants.COL_YesterdayUnderlyingMarkPriceStr:
                                _allowedGroupedSortedColumnText.Add(var.Key);
                                break;

                            case PMConstants.COL_StrikePrice:
                            case PMConstants.COL_SideMultiplier:
                                _allowedGroupedSortedColumnNumericCumText.Add(var.Key);
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_LastUpdatedUTC:
                            case PMConstants.COL_TradeDate:
                            case PMConstants.COL_ExDividendDate:
                                _allowedGroupedSortedColumnText.Add(var.Key);
                                break;

                            case PMConstants.COL_Multiplier:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_StartTradeDate:
                                _allowedGroupedSortedColumnText.Add(var.Key);
                                break;

                            case PMConstants.COL_ExpirationDate:
                                _allowedGroupedSortedColumnDates.Add(var.Key);
                                break;

                            case PMConstants.COL_ExpirationMonth:
                                _allowedGroupedSortedColumnDates.Add(var.Key);
                                break;

                            case PMConstants.COL_StrikeGapRisk:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            case PMConstants.COL_PositionSideExposureBoxed:
                                break;

                            case PMConstants.COL_NAV:
                            case PMConstants.COL_StartOfDayNAV:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;
                            case PMConstants.COL_NavTouch:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                break;

                            default:
                                _allowedGroupedSortedColumnText.Add(var.Key);
                                break;
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


        /// <summary>
        /// Apply Sorting After GroupBy
        /// </summary>
        /// <param name="sortColumn"></param>
        private void ApplySortingAfterGroupBy(UltraGridColumn sortColumn)
        {
            try
            {
                if (_allowedGroupedSortedColumnText.Contains(sortColumn.Key))
                {
                    if (!sortColumn.IsGroupByColumn
                        && !_customViewPreference.GroupByColumnsCollection.Contains(sortColumn.Key))
                    {
                        _customSorterText.Column = sortColumn.Key;
                        _customSorterText.SortIndicator = sortColumn.SortIndicator;
                        foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                        int count = 0;
                        foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                        {
                            //In case the level 1 groupby column is sorted descending, then reverse the sorting to be applied.
                            if (var.IsGroupByColumn && count < 1)
                            {
                                if (var.SortIndicator == SortIndicator.Descending)
                                {
                                    if (_customSorterText.SortIndicator == SortIndicator.Ascending)
                                    {
                                        _customSorterText.SortIndicator = SortIndicator.Descending;
                                    }
                                    else if (_customSorterText.SortIndicator == SortIndicator.Descending)
                                    {
                                        _customSorterText.SortIndicator = SortIndicator.Ascending;
                                    }
                                }
                                count++;
                                //Overrides the default sorting applied on level1 grouping 
                                //by assigning the custom sorting to Group By Comparer of level1 group by column.
                                var.GroupByComparer = _customSorterText;
                            }
                            if (_count == 0 && _allowedGroupedSortedColumnNumericCumText.Contains(sortColumn.Key))
                            {
                                var.SortComparer = _customSorterTextForRow;
                            }
                        }
                        grdConsolidation.DisplayLayout.Bands[0].SortedColumns[sortColumn.Key].GroupByComparer = _customSorterText;
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                    }
                }
                else if (_allowedGroupedSortedColumnAlphaNumeric.Contains(sortColumn.Key))
                {
                    if (!sortColumn.IsGroupByColumn
                        && !_customViewPreference.GroupByColumnsCollection.Contains(sortColumn.Key))
                    {
                        _customSorterAlphaNumeric.Column = sortColumn.Key;
                        _customSorterAlphaNumeric.SortIndicator = sortColumn.SortIndicator;
                        sortColumn.GroupByComparer = _customSorterAlphaNumeric;
                        int count = 0;
                        foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                        {
                            //In case the level 1 groupby column is sorted descending, then reverse the sorting to be applied.
                            if (var.IsGroupByColumn && count < 1)
                            {
                                if (var.SortIndicator == SortIndicator.Descending)
                                {
                                    if (_customSorterAlphaNumeric.SortIndicator == SortIndicator.Ascending)
                                    {
                                        _customSorterAlphaNumeric.SortIndicator = SortIndicator.Descending;
                                    }
                                    else if (_customSorterAlphaNumeric.SortIndicator == SortIndicator.Descending)
                                    {
                                        _customSorterAlphaNumeric.SortIndicator = SortIndicator.Ascending;
                                    }
                                }
                                count++;
                                //Overrides the default sorting applied on level1 grouping 
                                //by assigning the custom sorting to Group By Comparer of level1 group by column.
                                var.GroupByComparer = _customSorterAlphaNumeric;
                            }
                            if (_count == 0 && _allowedGroupedSortedColumnNumericCumText.Contains(sortColumn.Key))
                            {
                                var.SortComparer = _customSorterTextForRow;
                            }
                        }
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                    }
                }
                else if (_allowedGroupedSortedColumnDates.Contains(sortColumn.Key))
                {
                    if (!sortColumn.IsGroupByColumn
                        && !_customViewPreference.GroupByColumnsCollection.Contains(sortColumn.Key))
                    {
                        _customSorterDate.Column = sortColumn.Key;
                        _customSorterDate.SortIndicator = sortColumn.SortIndicator;
                        sortColumn.GroupByComparer = _customSorterDate;
                        foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                        {
                            if (_count == 0)
                            {
                                var.SortComparer = _customSorterDateForRow;
                            }
                            else
                            {
                                var.GroupByComparer = _customSorterDate;
                            }
                        }
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.SortComparer = null;
                            var.GroupByComparer = null;
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

        private void ShowHideGroupLevelButtons()
        {
            try
            {
                if (_customViewPreference != null)
                {
                    int count = grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Cast<UltraGridColumn>().Count(col => col.IsGroupByColumn);

                    if (count != _customViewPreference.GroupByColumnsCollection.Count)
                    {
                        //=> something may be wrong in grouping
                        _customViewPreference.GroupByColumnsCollection.Clear();
                        foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                        {
                            if (var.IsGroupByColumn)
                            {
                                _customViewPreference.GroupByColumnsCollection.Add(var.Key);
                            }
                        }
                    }

                    switch (_customViewPreference.GroupByColumnsCollection.Count)
                    {
                        case 0:

                            expandCollapseLevel1ToolStripMenuItem.Visible = false;
                            expandCollapseLevel2ToolStripMenuItem.Visible = false;
                            expandCollapseLevel3ToolStripMenuItem.Visible = false;
                            expandCollapseLevel4ToolStripMenuItem.Visible = false;
                            expandCollapseLevel5ToolStripMenuItem.Visible = false;
                            break;

                        case 1:

                            expandCollapseLevel1ToolStripMenuItem.Visible = true;
                            expandCollapseLevel2ToolStripMenuItem.Visible = false;
                            expandCollapseLevel3ToolStripMenuItem.Visible = false;
                            expandCollapseLevel4ToolStripMenuItem.Visible = false;
                            expandCollapseLevel5ToolStripMenuItem.Visible = false;
                            break;

                        case 2:

                            expandCollapseLevel1ToolStripMenuItem.Visible = true;
                            expandCollapseLevel2ToolStripMenuItem.Visible = true;
                            expandCollapseLevel3ToolStripMenuItem.Visible = false;
                            expandCollapseLevel4ToolStripMenuItem.Visible = false;
                            expandCollapseLevel5ToolStripMenuItem.Visible = false;
                            break;

                        case 3:


                            expandCollapseLevel1ToolStripMenuItem.Visible = true;
                            expandCollapseLevel2ToolStripMenuItem.Visible = true;
                            expandCollapseLevel3ToolStripMenuItem.Visible = true;
                            expandCollapseLevel4ToolStripMenuItem.Visible = false;
                            expandCollapseLevel5ToolStripMenuItem.Visible = false;
                            break;

                        case 4:


                            expandCollapseLevel1ToolStripMenuItem.Visible = true;
                            expandCollapseLevel2ToolStripMenuItem.Visible = true;
                            expandCollapseLevel3ToolStripMenuItem.Visible = true;
                            expandCollapseLevel4ToolStripMenuItem.Visible = true;
                            expandCollapseLevel5ToolStripMenuItem.Visible = false;
                            break;

                        default:


                            expandCollapseLevel1ToolStripMenuItem.Visible = true;
                            expandCollapseLevel2ToolStripMenuItem.Visible = true;
                            expandCollapseLevel3ToolStripMenuItem.Visible = true;
                            expandCollapseLevel4ToolStripMenuItem.Visible = true;
                            expandCollapseLevel5ToolStripMenuItem.Visible = true;
                            break;
                    }
                }
                else
                {


                    expandCollapseLevel1ToolStripMenuItem.Visible = false;
                    expandCollapseLevel2ToolStripMenuItem.Visible = false;
                    expandCollapseLevel3ToolStripMenuItem.Visible = false;
                    expandCollapseLevel4ToolStripMenuItem.Visible = false;
                    expandCollapseLevel5ToolStripMenuItem.Visible = false;
                }
                CollapseLevel1();
                CollapseLevel2();
                CollapseLevel3();
                CollapseLevel4();
                CollapseLevel5();
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

        //Initialize row is fired when datasource of grid changes, in our application it is not firing on each data update.
        // thus we fire it explicitely whenever we have to change the row color

        private void grdConsolidation_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e == null || e.Row == null || !e.Row.Band.Columns.Exists(PMConstants.COL_OrderSideTagValue))
                {
                    return;
                }
                object cellValue;
                if (_pmAppearances.RowColorbasis.Equals("1")) // Day Pnl
                {
                    cellValue = e.Row.GetCellValue(e.Row.Band.Columns[PMConstants.COL_DayPnLInBaseCurrency]);
                    if (cellValue != null)
                    {
                        double dayPnL = Convert.ToDouble(cellValue);
                        if (dayPnL > 0.0)
                        {
                            e.Row.Appearance = grdConsolidation.DisplayLayout.Appearances["Positive"];
                        }
                        else if (dayPnL < 0.0)
                        {
                            e.Row.Appearance = grdConsolidation.DisplayLayout.Appearances["Negative"];
                        }
                        else
                        {
                            e.Row.Appearance = grdConsolidation.DisplayLayout.Appearances["Neutral"];
                        }
                    }
                }
                else if (_pmAppearances.RowColorbasis.Equals("0"))
                {
                    cellValue = e.Row.GetCellValue(e.Row.Band.Columns[PMConstants.COL_OrderSideTagValue]);

                    if (cellValue != null)
                    {
                        string orderSideTagValue = cellValue.ToString();
                        switch (orderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Closed:
                            case FIXConstants.SIDE_Buy_Open:
                            case FIXConstants.SIDE_BuyMinus:
                                e.Row.Appearance = grdConsolidation.DisplayLayout.Appearances["Positive"];
                                break;

                            case FIXConstants.SIDE_SellShort:
                            case FIXConstants.SIDE_Sell:
                            case FIXConstants.SIDE_Sell_Open:
                            case FIXConstants.SIDE_Sell_Closed:
                            case FIXConstants.SIDE_SellPlus:
                            case FIXConstants.SIDE_SellShortExempt:
                                e.Row.Appearance = grdConsolidation.DisplayLayout.Appearances["Negative"];
                                break;

                            default:
                                e.Row.Appearance = grdConsolidation.DisplayLayout.Appearances["Neutral"];
                                break;
                        }
                    }
                }
                else if (_pmAppearances.RowColorbasis.Equals("2"))
                {
                    e.Row.Appearance = grdConsolidation.DisplayLayout.Appearances["Neutral"];
                }

                if (e.Row.ListObject != null && (((ExposurePnlCacheItem)e.Row.ListObject).Asset == AssetCategory.Equity.ToString() || ((ExposurePnlCacheItem)e.Row.ListObject).Asset == AssetCategory.PrivateEquity.ToString() || ((ExposurePnlCacheItem)e.Row.ListObject).Asset == AssetCategory.CreditDefaultSwap.ToString()))
                {
                    e.Row.Cells["ExpirationMonth"].Value = DateTimeConstants.MinValue;
                    ValueList valuelist = new ValueList();
                    valuelist.ValueListItems.Add(new ValueListItem(DateTimeConstants.MinValue, "Non-Expiring Positions"));
                    e.Row.Cells["ExpirationMonth"].ValueList = valuelist;
                    e.Row.Update();
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

        private void SetValueForNullableColumns(string columnKey, bool isMultiple)
        {
            try
            {
                if (grdConsolidation.DisplayLayout.Bands[0].Columns.Exists(columnKey))
                {
                    if (columnKey != PMConstants.COL_ExpirationDate)
                        grdConsolidation.DisplayLayout.Bands[0].Columns[columnKey].NullText = isMultiple ? ApplicationConstants.C_Multiple : ApplicationConstants.C_Dash;
                    else
                        grdConsolidation.DisplayLayout.Bands[0].Columns[columnKey].NullText = isMultiple ? ApplicationConstants.C_NotAvailable : ApplicationConstants.C_Dash;
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
        /// Added by: Bharat raturi, 21 aug 2014
        /// Update the summary of the views
        /// </summary>
        /// <param name="prefMSGType"></param>
        /// <param name="info"></param>
        public void SummarySettings(ExPNLPreferenceMsgType prefMSGType, string info)
        {
            try
            {
                //This part will clear the summary of the old column
                if (prefMSGType == ExPNLPreferenceMsgType.SelectedViewChanged && info.ToLower().Equals("true"))
                {
                    grdConsolidation.DisplayLayout.Bands[0].Summaries.Clear();
                    return;
                }
                switch (prefMSGType)
                {
                    case ExPNLPreferenceMsgType.SelectedViewChanged:
                        //create summary for the newly selected tab
                        foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].Columns)
                        {
                            if (var.Hidden || var.Key.Equals(PMConstants.COL_TradeDate))
                                continue;
                            CreateColumnSummary(var);
                        }
                        break;

                    case ExPNLPreferenceMsgType.SelectedColumnDeleted:
                        //delete the column summary that has been removed from the view
                        if (grdConsolidation.DisplayLayout.Bands[0].Summaries.Exists(info))
                        {
                            grdConsolidation.DisplayLayout.Bands[0].Summaries.Remove(grdConsolidation.DisplayLayout.Bands[0].Summaries[info]);
                        }
                        break;

                    case ExPNLPreferenceMsgType.SelectedColumnAdded:
                        //add the summary of the column added
                        if (!grdConsolidation.DisplayLayout.Bands[0].Columns[info].Hidden && !grdConsolidation.DisplayLayout.Bands[0].Columns[info].Key.Equals(PMConstants.COL_TradeDate))
                            CreateColumnSummary(grdConsolidation.DisplayLayout.Bands[0].Columns[info]);
                        break;
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

        private bool _isExportGridNeeded;

        /// <summary>
        /// Added by: Bharat raturi, 21 aug 2014
        /// Update the column summaries
        /// </summary>
        private void CreateColumnSummary(UltraGridColumn var)
        {
            try
            {
                if (var.ExcludeFromColumnChooser == ExcludeFromColumnChooser.Default ||
                        var.ExcludeFromColumnChooser == ExcludeFromColumnChooser.False)
                {
                    if (!grdConsolidation.DisplayLayout.Bands[0].Summaries.Exists(var.Key) || _isExportGridNeeded)
                    {
                        ICustomSummaryCalculator respectiveCalculator;
                        string caption = PMConstantsHelper.GetCaptionByColumnName(var.Key);
                        string displayFormat;
                        switch (var.Key)
                        {
                            case PMConstants.COL_DeltaAdjPosition:
                            case PMConstants.COL_NetExposure:
                            case PMConstants.COL_MarketValue:
                            case PMConstants.COL_MarketValueInBaseCurrency:
                            case PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency:
                            case PMConstants.COL_DayInterest:
                            case PMConstants.COL_TotalInterest:
                            case PMConstants.COL_CostBasisUnRealizedPNL:
                            case PMConstants.COL_BetaAdjExposure:
                            case PMConstants.COL_FxCostBasisPnl:
                            case PMConstants.COL_FxDayPnl:
                            case PMConstants.COL_TradeCostBasisPnl:
                            case PMConstants.COL_TradeDayPnl:
                            case PMConstants.COL_UnderlyingValueForOptions:
                            case PMConstants.COL_DeltaAdjPositionLME:
                            case PMConstants.COL_PremiumDollar:
                            case PMConstants.COL_StrikeGapExposure:
                            case PMConstants.COL_YesterdayMarketValue:
                            case PMConstants.COL_YesterdayMarketValueInBaseCurrency:
                            case PMConstants.COL_Exposure:
                            case PMConstants.COL_ExposureInBaseCurrency:
                            case PMConstants.COL_NetExposureInBaseCurrency:
                            case PMConstants.COL_BetaAdjExposureInBaseCurrency:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.OrderExposure(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_GrossExposure:
                            case PMConstants.COL_GrossMarketValue:
                            case PMConstants.COL_GrossExposureLocal:
                            case PMConstants.COL_UnderlyingGrossExposure:
                            case PMConstants.COL_BetaAdjGrossExposureUnderlying:

                            //These are the plane summable column so should be moved to plain sum calculator moved 
                            case PMConstants.COL_PercentDayPnLGrossMV:
                            case PMConstants.COL_PercentDayPnLNetMV:
                            case PMConstants.COL_BetaAdjGrossExposure:
                            case PMConstants.COL_PercentGrossExposureInBaseCurrency:
                            case PMConstants.COL_PercentUnderlyingGrossExposureInBaseCurrency:
                            case PMConstants.COL_PercentBetaAdjGrossExposureInBaseCurrency:

                            case PMConstants.COL_PercentGrossMarketValueInBaseCurrency:
                            case PMConstants.COL_BetaAdjGrossExposureUnderlyingInBaseCurrency:
                            case PMConstants.COL_UnderlyingGrossExposureInBaseCurrency:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.GrossCalculator(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_DayPnL:
                            case PMConstants.COL_CashImpact:
                            case PMConstants.COL_EarnedDividendLocal:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.OrderPNL(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_DayPnLInBaseCurrency:
                            case PMConstants.COL_CashImpactInBaseCurrency:
                            case PMConstants.COL_EarnedDividendBase:
                            case PMConstants.COL_GainLossIfExerciseAssign:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.OrderPNL(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_AvgPrice:
                            case PMConstants.COL_CostBasisBreakEven:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.OrderTotalsSummary(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_ExposureBPInBaseCurrency:
                            case PMConstants.COL_PercentExposureInBaseCurrency:
                            case PMConstants.COL_DayReturn:
                            case PMConstants.COL_PercentagePNLContribution:
                            case PMConstants.COL_PercentNetExposureInBaseCurrency:
                            case PMConstants.COL_PercentNetMarketValueInBaseCurrency:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.PNLContributionBPSummary(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_PercentageAverageVolumeDeltaAdjusted:
                            case PMConstants.COL_Quantity:
                            case PMConstants.COL_PercentageAverageVolume:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.PositionSummary(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_Volatility:
                                _allowedGroupedSortedColumnNumericCumText.Add(var.Key);
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.OrderDelta(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_Delta:
                            case PMConstants.COL_LeveragedFactor:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.OrderDelta(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_PercentageGainLoss:
                            case PMConstants.COL_PercentageGainLossCostBasis:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.PercentagePositionLongSummary();
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_NetNotionalValue:
                            case PMConstants.COL_NetNotionalForCostBasisBreakEven:
                            case PMConstants.COL_NetNotionalValueBase:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.OrderNotional(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_SharesOutstanding:
                            case PMConstants.COL_AverageVolume20Day:
                            case PMConstants.COL_AverageVolume20DayUnderlyingSymbol:
                            case PMConstants.COL_MarketCapitalization:
                            case PMConstants.COL_Beta:
                            case PMConstants.COL_PercentageChange:
                            case PMConstants.COL_ClosingPrice:
                            case PMConstants.COL_LastPrice:
                            case PMConstants.COL_SelectedFeedPrice:
                            case PMConstants.COL_SelectedFeedPriceInBaseCurrency:
                            case PMConstants.COL_UnderlyingStockPrice:
                            case PMConstants.COL_Premium:
                            case PMConstants.COL_PercentageUnderlyingChange:
                            case PMConstants.COL_ChangeInUnderlyingPrice:
                            case PMConstants.COL_AskPrice:
                            case PMConstants.COL_BidPrice:
                            case PMConstants.COL_MidPrice:
                            case PMConstants.COL_DividendYield:
                            case PMConstants.COL_YesterdayFXRate:
                            case PMConstants.COL_FXRateDisplay:
                            case PMConstants.COL_FXRate:
                            case PMConstants.COL_ForwardPoints:
                            case PMConstants.COL_TradeVolume:
                            case PMConstants.COL_PercentOfITMOTM:
                            case PMConstants.COL_IntrinsicValue:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.IdenticalNumberSummary(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_Symbol:
                            case PMConstants.COL_CurrencySymbol:
                            case PMConstants.COL_FullSecurityName:
                            case OrderFields.PROPERTY_LEVEL1NAME:
                            case OrderFields.PROPERTY_LEVEL2NAME:
                            case PMConstants.COL_DataSourceNameIDValue:
                            case PMConstants.COL_Asset:
                            case PMConstants.COL_UDAAsset:
                            case PMConstants.COL_UDACountry:
                            case PMConstants.COL_UDASector:
                            case PMConstants.COL_UDASecurityType:
                            case PMConstants.COL_UDASubSector:
                            case PMConstants.COL_Underlying:
                            case PMConstants.COL_Exchange:
                            case PMConstants.COL_SideName:
                            case PMConstants.COL_TransactionSide:
                            case PMConstants.COL_VsCurrencySymbol:
                            case PMConstants.COL_MasterFund:
                            case PMConstants.COL_MasterStrategy:
                            case PMConstants.COL_UserName:
                            case PMConstants.COL_CounterPartyName:
                            case PMConstants.COL_ContractType:
                            case PMConstants.COL_LeadCurrencySymbol:
                            case PMConstants.COL_IDCOSymbol:
                            case PMConstants.COL_OSISymbol:
                            case PMConstants.COL_SEDOLSymbol:
                            case PMConstants.COL_CUSIPSymbol:
                            case PMConstants.COL_BloombergSymbol:
                            case PMConstants.COL_BloombergSymbolWithExchangeCode:
                            case PMConstants.COL_ISINSymbol:
                            case PMConstants.COL_TransactionType:
                            case PMConstants.COL_ReutersSymbol:
                            case PMConstants.COL_InternalComments:
                            case PMConstants.COL_PositionSideExposureUnderlying:
                            case PMConstants.COL_PositionSideMV:
                            case PMConstants.COL_PositionSideExposure:

                                _allowedGroupedSortedColumnText.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.TextSummary(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_YesterdayMarkPriceStr:
                            case PMConstants.COL_FXRateOnTradeDateStr:
                            case PMConstants.COL_YesterdayUnderlyingMarkPriceStr:
                                _allowedGroupedSortedColumnText.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.TextSummary(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_StrikePrice:
                            case PMConstants.COL_SideMultiplier:
                                _allowedGroupedSortedColumnNumericCumText.Add(var.Key);
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.IdenticalNumberSummary(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_LastUpdatedUTC:
                            case PMConstants.COL_TradeDate:
                            case PMConstants.COL_ExDividendDate:
                                _allowedGroupedSortedColumnText.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.TextSummary(var.Key);
                                displayFormat = "{0}";
                                break;

                            case PMConstants.COL_Multiplier:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.IdenticalNumberSummary(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_StartTradeDate:
                                _allowedGroupedSortedColumnText.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.StartDateSummary();
                                displayFormat = "{0:d}";
                                break;

                            case PMConstants.COL_ExpirationDate:
                                _allowedGroupedSortedColumnDates.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.TextSummary(var.Key);
                                displayFormat = "{0}";
                                break;

                            case PMConstants.COL_ExpirationMonth:
                                _allowedGroupedSortedColumnDates.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.DateSummary(var.Key);
                                displayFormat = "{0:MMMM yyyy}";
                                break;

                            case PMConstants.COL_StrikeGapRisk:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.GroupingWiseSummary(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            case PMConstants.COL_PositionSideExposureBoxed:
                                respectiveCalculator = new CustomSummariesFactory.TextSummary(var.Key);
                                displayFormat = "{0}";
                                break;

                            case PMConstants.COL_NAV:
                            case PMConstants.COL_StartOfDayNAV:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.IdenticalNumberSummary(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;
                            case PMConstants.COL_NavTouch:
                            case PMConstants.COL_DayTradedPosition:
                                _allowedGroupedSortedColumnAlphaNumeric.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.IdenticalColumnWiseNumberSummary(var.Key);
                                displayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                                break;

                            default:
                                _allowedGroupedSortedColumnText.Add(var.Key);
                                respectiveCalculator = new CustomSummariesFactory.TextSummary(var.Key);
                                displayFormat = "{0}";
                                break;
                        }
                        var s = !_isExportGridNeeded ? grdConsolidation.DisplayLayout.Bands[0].Summaries.Add(var.Key, SummaryType.Custom, respectiveCalculator, var, SummaryPosition.UseSummaryPositionColumn, var) : grid.DisplayLayout.Bands[0].Summaries.Add(var.Key, SummaryType.Custom, respectiveCalculator, var, SummaryPosition.UseSummaryPositionColumn, var);

                        grdConsolidation.SummaryValueChanged -= grdConsolidation_SummaryValueChanged;
                        grdConsolidation.SummaryValueChanged += grdConsolidation_SummaryValueChanged;
                        s.DisplayFormat = displayFormat;
                        s.Appearance.TextHAlign = HAlign.Right;
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

        /// <summary>
        /// Handles the SummaryValueChanged event of the grdConsolidation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SummaryValueChangedEventArgs"/> instance containing the event data.</param>
        void grdConsolidation_SummaryValueChanged(object sender, SummaryValueChangedEventArgs e)
        {
            try
            {
                if (_groupByColumnList != null && _groupByColumnList.Count > 0 && e.SummaryValue.Key.Equals(_groupByColumnList[0])
                    && e.SummaryValue.SummaryText.Equals(PMConstants.SUMMARY_MULTIPLE) && e.SummaryValue.ParentRows.ParentRow != null)
                {
                    string str = "Multiple summary calculated for grouped column." + Environment.NewLine +
                        "Grouped Columns: " + string.Join(",", _groupByColumnList) + Environment.NewLine;
                    if (CommonDataCache.CachedDataManager.GetInstance.LoggedInUser != null)
                    {
                        str += "Client Name: " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyName + Environment.NewLine +
                                                "User: " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName + Environment.NewLine;
                    }
                    LogExtensions.LoggerWriteMessage(LoggingConstants.CATEGORY_GENERAL, str);
                    if ((DateTime.Now - _lastRefreshSortTime).TotalSeconds > _errorRefreshInterval)
                    {
                        CaptureCurrentUIState();
                        grdConsolidation.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                        RestoreCurrentUIState();
                        SendErrorEmail(str);
                        _lastRefreshSortTime = DateTime.Now;
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
        /// Sends the error email.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private async void SendErrorEmail(string msg)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                        EmailsHelper.MailSend(_mailSubject, msg, _senderID, "Nirvana Support", _senderPWD, _receiverIDs.Split(','), _mailServerSMTPPort, _mailServer, _enableSSL, true)
                    );
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

        #region Context Menu items here!

        public void SaveLayout(bool clearGroupByCollection, string key, ref CustomViewPreferences currentDefaultPreferences)
        {
            try
            {
                SaveLayout(clearGroupByCollection, key, _customViewPreference.FilterDetails);
                currentDefaultPreferences = _customViewPreference;
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

        public void SaveLayout(bool clearGroupByCollection, string key, GridColumnFilterDetails filterDetails)
        {
            try
            {
                if (filterDetails != null)
                {
                    var stringList = filterDetails.DynamicFilterConditionList.ConvertAll(obj => obj.ToString());
                    if (stringList.Count == 3 && stringList.Contains(ApplicationConstants.FilterDetails_DBNullAccount))
                    {
                        filterDetails.DynamicFilterConditionList.Clear();
                        FilterCondition filterCondition = new FilterCondition(FilterComparisionOperator.Equals, "(Unallocated)");
                        filterDetails.DynamicFilterConditionList.Add(filterCondition);
                    }
                    if (stringList.Count == 3 && stringList.Contains(ApplicationConstants.FilterDetails_DBNullMasterFund))
                    {
                        filterDetails.DynamicFilterConditionList.Clear();
                        FilterCondition filterCondition = new FilterCondition(FilterComparisionOperator.Equals, "(Unallocated)");
                        filterDetails.DynamicFilterConditionList.Add(filterCondition);
                    }
                }
                CustomViewPreferences customViewPreference = GetLayout(clearGroupByCollection, key);
                if (filterDetails == null)
                    filterDetails = new GridColumnFilterDetails();
                customViewPreference.FilterDetails = filterDetails;
                PMPrefrenceManager.GetInstance(SUB_MODULE_NAME).SetCustomViewPreference(customViewPreference, key);
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

        public CustomViewPreferences GetLayout(bool clearGroupByCollection, string key)
        {
            try
            {
                if (_customViewPreference != null)
                {
                    List<PreferenceGridColumn> colsCollection = new List<PreferenceGridColumn>();

                    //add columns collection !!
                    bool isCustomTab = IsCustomTab(key);

                    foreach (UltraGridColumn column in grdConsolidation.DisplayLayout.Bands[0].Columns)
                    {
                        if (!column.Hidden || grdConsolidation.DisplayLayout.Bands[0].ColumnFilters.Exists(column.Key))
                        {
                            List<FilterCondition> filterConditionList = new List<FilterCondition>();
                            FilterConditionsCollection filterConditionsColl = grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[column].FilterConditions;
                            var logicalOperator = grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[column].LogicalOperator;

                            foreach (FilterCondition filterCond in filterConditionsColl)
                            {
                                FilterCondition filterCondClone = new FilterCondition(filterCond.ComparisionOperator, filterCond.CompareValue);
                                if (filterCondClone.CompareValue != null)
                                {
                                    Type type = filterCondClone.CompareValue.GetType();
                                    if (type.BaseType != null && type.BaseType == typeof(Enum))
                                    {
                                        filterCondClone.CompareValue = filterCondClone.CompareValue.ToString();
                                    }

                                    if (((column.Key.Equals(PMConstants.COL_TradeDate) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.NirvanaDateTimeFormat_WithoutTime))) || (column.Key.Equals(PMConstants.COL_StartTradeDate) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateFormat))) || (column.Key.Equals(PMConstants.COL_ExpirationDate) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.NirvanaDateTimeFormat_WithoutTime)))) && filterCondClone.ComparisionOperator == FilterComparisionOperator.StartsWith)
                                    {
                                        filterCondClone.CompareValue = "(Today)";
                                    }

                                    if (((column.Key.Equals(PMConstants.COL_TradeDate) || column.Key.Equals(PMConstants.COL_StartTradeDate)) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateFormat))) && filterCondClone.ComparisionOperator == FilterComparisionOperator.LessThan)
                                    {
                                        filterCondClone.CompareValue = "(BeforeToday)";
                                    }

                                    if (filterCondClone.ToString().Contains(ApplicationConstants.FilterDetails_DBNullAccount))
                                    {
                                        FilterCondition filterCondition = new FilterCondition(FilterComparisionOperator.Equals, "(Unallocated)");
                                        filterConditionList.Add(filterCondition);
                                        break;
                                    }
                                    filterConditionList.Add(filterCondClone);
                                }
                            }
                            SortIndicator sort = column.SortIndicator;
                            PreferenceGridColumn prefCol = new PreferenceGridColumn(column.Key, column.Header.VisiblePosition, column.Width, column.Header.Fixed, filterConditionList, column.Formula, column.DataType.ToString(), logicalOperator, sort, column.Hidden);
                            colsCollection.Add(prefCol);
                        }
                        else if (column.Formula != null)
                        {
                            List<FilterCondition> filterConditionList = new List<FilterCondition>();
                            FilterLogicalOperator logicalOperator = FilterLogicalOperator.And;
                            if (isCustomTab)
                            {
                                FilterConditionsCollection filterConditionsColl = grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[column].FilterConditions;
                                logicalOperator = grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[column].LogicalOperator;

                                foreach (FilterCondition filterCond in filterConditionsColl)
                                {
                                    if (filterCond.CompareValue != null)
                                    {
                                        Type type = filterCond.CompareValue.GetType();
                                        if (type.BaseType != null && type.BaseType == typeof(Enum))
                                        {
                                            filterCond.CompareValue = filterCond.CompareValue.ToString();
                                        }
                                        filterConditionList.Add(filterCond);
                                    }
                                }
                            }
                            SortIndicator sort = column.SortIndicator;
                            PreferenceGridColumn prefCol = new PreferenceGridColumn(column.Key, -1, column.Width, column.Header.Fixed, filterConditionList, column.Formula, column.DataType.ToString(), logicalOperator, sort, column.Hidden);
                            colsCollection.Add(prefCol);
                        }
                        colsCollection.Sort();
                        _customViewPreference.SelectedColumnsCollection = colsCollection;

                        //Set the order of groupby cols correctly
                        if (grdConsolidation.DisplayLayout.UIElement.ChildElements.Count > 1) //if count is one the it is main consolidation view. In which case there is no group box
                        {
                            GroupByBoxUIElement GrpElem = (GroupByBoxUIElement)grdConsolidation.DisplayLayout.UIElement.ChildElements[0];
                            if (clearGroupByCollection)
                            {
                                _customViewPreference.GroupByColumnsCollection.Clear();
                            }

                            grdConsolidation.Refresh();

                            foreach (UIElement elm in GrpElem.ChildElements)
                            {
                                ColumnHeader obj = elm.GetContext() as ColumnHeader;
                                if (obj != null)
                                {
                                    string groupByColName = PMConstantsHelper.GetColumnNameByCaption(obj.Caption);
                                    _customViewPreference.GroupByColumnsCollection.Add(groupByColName);
                                    ShowHideGroupLevelButtons();
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
            return _customViewPreference;
        }

        private static bool IsCustomTab(string key)
        {
            bool isCustomTab = false;
            string[] strArr = key.Split('_');
            if (strArr.Length >= 3) //
            {
                if (strArr[1] == SUB_MODULE_NAME)
                {
                    isCustomTab = true;
                }
            }
            return isCustomTab;
        }

        private TradingTicketUIPrefs _userTradingTicketUiPrefs = TradingTktPrefs.UserTradingTicketUiPrefs;
        private void OnIncreasePositionClick()
        {
            try
            {
                Dictionary<int, double> accountWithPostion = new Dictionary<int, double>();
                ExposurePnlCacheItem consolidatedInfo = null;
                if (grdConsolidation.ActiveRow != null)
                {
                    consolidatedInfo = (ExposurePnlCacheItem)grdConsolidation.ActiveRow.ListObject;
                }
                if (null != consolidatedInfo)
                {
                    var order = new OrderSingle
                    {
                    };
                    string orderSide;
                    switch (consolidatedInfo.OrderSideTagValue)
                    {
                        case FIXConstants.SIDE_Buy:
                            orderSide = FIXConstants.SIDE_Buy;
                            break;

                        case FIXConstants.SIDE_Sell:
                            orderSide = FIXConstants.SIDE_Sell;
                            break;

                        case FIXConstants.SIDE_Buy_Cover:
                            orderSide = FIXConstants.SIDE_Buy_Cover;
                            break;

                        case FIXConstants.SIDE_SellShort:
                            orderSide = FIXConstants.SIDE_SellShort;
                            break;

                        case FIXConstants.SIDE_Buy_Open:
                            orderSide = FIXConstants.SIDE_Buy_Open;
                            break;

                        case FIXConstants.SIDE_Buy_Closed:
                            orderSide = FIXConstants.SIDE_Buy_Closed;
                            break;

                        case FIXConstants.SIDE_Sell_Open:
                            orderSide = FIXConstants.SIDE_Sell_Open;
                            break;

                        case FIXConstants.SIDE_Sell_Closed:
                            orderSide = FIXConstants.SIDE_Sell_Closed;
                            break;

                        default:
                            orderSide = FIXConstants.SIDE_Buy;
                            break;
                    }
                    if (accountWithPostion.ContainsKey(consolidatedInfo.Level1ID))

                        accountWithPostion[consolidatedInfo.Level1ID] += consolidatedInfo.Quantity;
                    else
                        accountWithPostion.Add(consolidatedInfo.Level1ID, consolidatedInfo.Quantity);

                    order.CounterPartyID = Convert.ToInt32(_userTradingTicketUiPrefs.Broker);
                    order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(order.CounterPartyID);
                    order.OrderSideTagValue = orderSide;
                    order.Price = consolidatedInfo.AvgPrice == null ? 0.0 : Convert.ToDouble(consolidatedInfo.AvgPrice);
                    order.Quantity = Convert.ToDouble(_userTradingTicketUiPrefs.Quantity);
                    order.Symbol = consolidatedInfo.Symbol;
                    order.BloombergSymbol = consolidatedInfo.BloombergSymbol;
                    order.BloombergSymbolWithExchangeCode = consolidatedInfo.BloombergSymbolWithExchangeCode;
                    order.FactSetSymbol = consolidatedInfo.FactSetSymbol;
                    order.ActivSymbol = consolidatedInfo.ActivSymbol;
                    order.TradeAttribute6 = consolidatedInfo.MasterFund;
                    order.Level1ID = consolidatedInfo.Level1ID == -1 ? int.MinValue : consolidatedInfo.Level1ID;
                    order.Level2ID = consolidatedInfo.Level2ID == -1 ? int.MinValue : consolidatedInfo.Level2ID;
                    order.SwapParameters = consolidatedInfo.IsSwap ? new SwapParameters() : null;
                    order.PMType = PMType.Increase;
                    if (TradeClick != null)
                    {
                        TradeClick(this, new EventArgs<OrderSingle, Dictionary<int, double>>(order, accountWithPostion));
                    }
                }
                else
                {
                    if (TradeClick != null)
                    {
                        TradeClick(this, new EventArgs<OrderSingle, Dictionary<int, double>>(null, null));
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

        /// <summary>
        /// Call this function on the click of trade.
        /// </summary>
        private void OnTradeClick()
        {
            try
            {
                ExposurePnlCacheItem consolidatedInfo = null;
                Dictionary<int, double> accountWithPostion = new Dictionary<int, double>();

                if (grdConsolidation.ActiveRow != null)
                {
                    consolidatedInfo = (ExposurePnlCacheItem)grdConsolidation.ActiveRow.ListObject;
                }
                if (null != consolidatedInfo)
                {
                    var order = new OrderSingle
                    {
                        CounterPartyID =
                            CachedDataManager.GetInstance.GetCounterPartyID(consolidatedInfo.CounterPartyName),
                        CounterPartyName = consolidatedInfo.CounterPartyName
                    };

                    string orderSide;
                    switch (consolidatedInfo.OrderSideTagValue)
                    {
                        case FIXConstants.SIDE_Buy:
                            orderSide = FIXConstants.SIDE_Sell;
                            break;

                        case FIXConstants.SIDE_Sell:
                            orderSide = FIXConstants.SIDE_Buy;
                            break;

                        case FIXConstants.SIDE_Buy_Cover:
                            orderSide = FIXConstants.SIDE_Buy_Closed;
                            break;

                        case FIXConstants.SIDE_SellShort:
                            orderSide = FIXConstants.SIDE_Buy_Closed;
                            break;

                        case FIXConstants.SIDE_Buy_Open:
                            orderSide = FIXConstants.SIDE_Sell_Closed;
                            break;

                        case FIXConstants.SIDE_Buy_Closed:
                            orderSide = FIXConstants.SIDE_Sell_Open;
                            break;

                        case FIXConstants.SIDE_Sell_Open:
                            orderSide = FIXConstants.SIDE_Buy_Closed;
                            break;

                        case FIXConstants.SIDE_Sell_Closed:
                            orderSide = FIXConstants.SIDE_Buy_Open;
                            break;

                        default:
                            orderSide = FIXConstants.SIDE_Buy;
                            break;
                    }
                    if (accountWithPostion.ContainsKey(consolidatedInfo.Level1ID))

                        accountWithPostion[consolidatedInfo.Level1ID] += consolidatedInfo.Quantity;
                    else
                        accountWithPostion.Add(consolidatedInfo.Level1ID, consolidatedInfo.Quantity);

                    order.OrderSideTagValue = orderSide;
                    order.Price = consolidatedInfo.AvgPrice == null ? 0.0 : Convert.ToDouble(consolidatedInfo.AvgPrice);
                    order.Quantity = Math.Abs(consolidatedInfo.Quantity);
                    order.Symbol = consolidatedInfo.Symbol;
                    order.BloombergSymbol = consolidatedInfo.BloombergSymbol;
                    order.BloombergSymbolWithExchangeCode = consolidatedInfo.BloombergSymbolWithExchangeCode;
                    order.FactSetSymbol = consolidatedInfo.FactSetSymbol;
                    order.ActivSymbol = consolidatedInfo.ActivSymbol;
                    order.TradeAttribute6 = consolidatedInfo.MasterFund;
                    order.Level1ID = consolidatedInfo.Level1ID == -1 ? int.MinValue : consolidatedInfo.Level1ID;
                    order.Level2ID = consolidatedInfo.Level2ID == -1 ? int.MinValue : consolidatedInfo.Level2ID;
                    order.SwapParameters = consolidatedInfo.IsSwap ? new SwapParameters() : null;
                    order.PMType = PMType.Close;
                    if (TradeClick != null)
                    {
                        TradeClick(this, new EventArgs<OrderSingle, Dictionary<int, double>>(order, accountWithPostion));
                    }
                }
                else
                {
                    if (TradeClick != null)
                    {
                        TradeClick(this, new EventArgs<OrderSingle, Dictionary<int, double>>(null, null));
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

        private void expandCollapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_CurrentUltraGridGroupByRow == null) return;
                if (_CurrentUltraGridGroupByRow.Expanded)
                {
                    _CurrentUltraGridGroupByRow.CollapseAll();
                }
                else
                {
                    _CurrentUltraGridGroupByRow.ExpandAll();
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

        private void addNewConsolidationViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (AddNewConsolidationView != null)
                {
                    AddNewConsolidationView(this, EventArgs.Empty);
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

        private void deleteViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DeleteViewClick != null)
                {
                    DeleteViewClick(this, EventArgs.Empty);
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

        private void openPricingInput()
        {
            try
            {
                StringBuilder selectedSymbols = new StringBuilder();
                List<string> symbols = new List<string>();
                var consolidatedInfo = (ExposurePnlCacheItem)grdConsolidation.ActiveRow.ListObject;
                if (consolidatedInfo == null)
                {
                    foreach (UltraGridRow row in ((UltraGridChildBand)(grdConsolidation.ActiveRow.ChildBands.All[0])).Rows)
                    {
                        if (row != null && row.ListObject != null)
                        {
                            consolidatedInfo = (ExposurePnlCacheItem)row.ListObject;
                            if (!symbols.Contains(consolidatedInfo.Symbol.ToUpper()))
                            {
                                symbols.Add(consolidatedInfo.Symbol.ToUpper());
                                selectedSymbols.Append(consolidatedInfo.Symbol.ToUpper());
                                selectedSymbols.Append(",");
                            }
                        }
                    }
                }
                else
                {
                    selectedSymbols.Append(consolidatedInfo.Symbol.ToUpper());
                }
                if (PricingInputClick != null)
                {
                    PricingInputClick(selectedSymbols, EventArgs.Empty);
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

        private void pricingInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdConsolidation.ActiveRow != null)
                {
                    openPricingInput();
                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void renameViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (RenameViewClick != null)
                {
                    RenameViewClick(this, e);
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

        private void clearFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdConsolidation.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                addAccountNamesBasedOnMasterFund = false;
                DisableEnableMasterFundFilter(null, false);
                if (SendFilterColumnName != null)
                    SendFilterColumnName(this, null);
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

        private void showHideDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (showHideDashboardToolStripMenuItem.Text == "Show Dashboard")
                {
                    if (ShowDashboardEvent != null)
                        ShowDashboardEvent(this, new EventArgs<bool>(true));

                    showHideDashboardToolStripMenuItem.Text = "Hide Dashboard";

                    if (_customViewPreference != null)
                        _customViewPreference.IsDashboardVisible = true;
                }
                else
                {
                    if (ShowDashboardEvent != null)
                        ShowDashboardEvent(this, new EventArgs<bool>(false));

                    showHideDashboardToolStripMenuItem.Text = "Show Dashboard";

                    if (_customViewPreference != null)
                        _customViewPreference.IsDashboardVisible = false;
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

        internal void ShowHideDashboard(bool isDashboardVisible)
        {
            try
            {
                if (isDashboardVisible && showHideDashboardToolStripMenuItem.Text == "Show Dashboard")
                {
                    if (ShowDashboardEvent != null)
                        ShowDashboardEvent(this, new EventArgs<bool>(true));

                    showHideDashboardToolStripMenuItem.Text = "Hide Dashboard";

                    if (_customViewPreference != null)
                        _customViewPreference.IsDashboardVisible = true;
                }
                else if (!isDashboardVisible && showHideDashboardToolStripMenuItem.Text == "Hide Dashboard")
                {
                    if (ShowDashboardEvent != null)
                        ShowDashboardEvent(this, new EventArgs<bool>(false));

                    showHideDashboardToolStripMenuItem.Text = "Show Dashboard";

                    if (_customViewPreference != null)
                        _customViewPreference.IsDashboardVisible = false;
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

        private void showSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdConsolidation.DisplayLayout.Bands[0].Layout.Override.AllowRowSummaries = AllowRowSummaries.True;
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

        private void hideSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdConsolidation.DisplayLayout.Bands[0].Layout.Override.AllowRowSummaries = AllowRowSummaries.False;
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

        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, UltraGrid> gridDict = new Dictionary<string, UltraGrid>();
                SetGridExportSettings(gridDict);
                _excelUtil.SetFilePath(" ");
                _excelUtil.OnExportToExcel(gridDict, null, false);
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

        private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = OpenSaveDialogBox();
                if (!String.IsNullOrEmpty(fileName))
                {
                    StreamWriter sw = File.CreateText(fileName);
                    string s = string.Empty;
                    String groupByColCaption = string.Empty;
                    // First fo all write all the visible columns header
                    UltraGridBand band = this.grdConsolidation.DisplayLayout.Bands[0];
                    SortedColumnsCollection sortedcolColl = band.SortedColumns;
                    if (sortedcolColl != null && sortedcolColl.Count > 0)
                    {
                        foreach (UltraGridColumn col in sortedcolColl)
                        {
                            if (col.IsGroupByColumn)
                            {
                                groupByColCaption = sortedcolColl[0].Header.Caption;
                                break;
                            }
                        }
                    }

                    VisiblePosComparer comparer = new VisiblePosComparer();
                    UltraGridColumn[] colArr = new UltraGridColumn[band.Columns.All.Length];

                    band.Columns.All.CopyTo(colArr, 0);
                    Array.Sort(colArr, comparer);
                    // Symbol column header for Group by Column in the Consolidation view
                    if (!string.IsNullOrEmpty(groupByColCaption))
                    {
                        s = s + groupByColCaption + ",";
                    }

                    foreach (UltraGridColumn col in colArr)
                    {
                        if (!col.Hidden)
                        {
                            s = s + col.Header.Caption + ",";
                        }
                    }
                    sw.WriteLine(s.TrimEnd(','));
                    s = string.Empty;

                    if (!string.IsNullOrEmpty(groupByColCaption))
                    {
                        foreach (UltraGridRow row in this.grdConsolidation.Rows)
                        {
                            if (row.IsGroupByRow && row.Hidden.Equals(false))
                            {
                                if (row.Description.Contains(","))
                                {
                                    s = "\"" + row.Description + "\"";
                                }
                                else
                                {
                                    s = row.Description;
                                }
                                SummaryValuesCollection summaryCol = row.ChildBands[0].Rows.SummaryValues;
                                // one extra column added in the export file for Group by column
                                s = s + "" + ",";
                                bool notInSummary = false;
                                foreach (UltraGridColumn col in colArr)
                                {
                                    notInSummary = false;
                                    if (!col.Hidden)
                                    {
                                        foreach (SummaryValue summary in summaryCol)
                                        {
                                            if (summary.Key.ToString().Equals(col.Key.ToString()))
                                            {
                                                if (summary.Value != null)
                                                {
                                                    if (summary.Value.ToString().Contains(","))
                                                    {
                                                        s = s + "\"" + summary.Value.ToString() + "\"" + ",";
                                                    }
                                                    else
                                                    {
                                                        s = s + summary.Value.ToString() + ",";
                                                    }
                                                }
                                                notInSummary = true;
                                                break;
                                            }
                                        }
                                        // if column is visible true and does not exists in the summary, then also add in the export file
                                        // so that number of column will be same
                                        if (!notInSummary)
                                        {
                                            s = s + "" + ",";
                                        }
                                    }
                                }
                                sw.WriteLine(s.TrimEnd(','));
                                s = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        UltraGridRow[] filterednonGropuedRows = grdConsolidation.Rows.GetFilteredInNonGroupByRows();
                        foreach (UltraGridRow row in filterednonGropuedRows)
                        {
                            foreach (UltraGridColumn col in colArr)
                            {
                                if (!col.Hidden)
                                {
                                    if (row.GetCellValue(row.Band.Columns[col.Key]) != null)
                                    {
                                        if (row.GetCellValue(row.Band.Columns[col.Key]).ToString().Contains(","))
                                        {
                                            s = s + "\"" + row.GetCellValue(row.Band.Columns[col.Key]) + "\"" + ",";
                                        }
                                        else
                                        {
                                            s = s + row.GetCellValue(row.Band.Columns[col.Key]) + ",";
                                        }
                                    }
                                }
                            }
                            sw.WriteLine(s.TrimEnd(','));
                            s = string.Empty;
                        }
                    }
                    sw.Close();
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

        private string OpenSaveDialogBox()
        {
            string strFilePath = string.Empty;
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
                saveFileDialog.RestoreDirectory = true;
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    strFilePath = saveFileDialog.FileName;
                }
                else if (result == DialogResult.Cancel)
                {
                    strFilePath = string.Empty;
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
            return strFilePath;
        }

        private UltraGrid grid;

        private void SetGridExportSettings(Dictionary<string, UltraGrid> gridDict)
        {
            try
            {
                string excelSheetName = Forms.PM._exportTabName;
                Form UI = new Form();
                grid = new UltraGrid();
                UI.Controls.Add(grid);
                grid.DataSource = _exPnlBindableView.GridData.Values;
                grid.DisplayLayout.Bands[0].Override.AllowGroupBy = DefaultableBoolean.True;
                grid.DisplayLayout.ViewStyleBand = ViewStyleBand.OutlookGroupBy;
                grid.DisplayLayout.Override.GroupByColumnsHidden = DefaultableBoolean.False;
                grid.DisplayLayout.Bands[0].Columns[PMConstants.COL_TradeDate].Format = DateTimeConstants.NirvanaDateTimeFormat;
                grid.DisplayLayout.Bands[0].Columns[PMConstants.COL_StartTradeDate].Format = DateTimeConstants.DateFormat;
                grid.DisplayLayout.Bands[0].Columns[PMConstants.COL_ExpirationDate].Format = DateTimeConstants.NirvanaDateTimeFormat;
                grid.DisplayLayout.Bands[0].ColumnFilters.CopyFrom(grdConsolidation.DisplayLayout.Bands[0].ColumnFilters);

                foreach (UltraGridColumn col in grdConsolidation.DisplayLayout.Bands[0].Columns)
                {
                    if (col.Header.Caption != string.Empty)
                    {
                        grid.DisplayLayout.Bands[0].Columns[col.Key].Header.Caption = col.Header.Caption;
                        grid.DisplayLayout.Bands[0].Columns[col.Key].Header.VisiblePosition = col.Header.VisiblePosition;
                    }
                    grid.DisplayLayout.Bands[0].Columns[col.Key].Hidden = true;
                }
                UltraGridBand band = grdConsolidation.DisplayLayout.Bands[0];
                SortedColumnsCollection sortedcolColl = band.SortedColumns;
                grid.DisplayLayout.Bands[0].SortedColumns.Clear();
                if (sortedcolColl != null && sortedcolColl.Count > 0)
                {
                    foreach (UltraGridColumn col in sortedcolColl)
                    {
                        if (col.IsGroupByColumn)
                        {
                            if (Convert.ToBoolean(col.SortIndicator == SortIndicator.Ascending))
                                grid.DisplayLayout.Bands[0].SortedColumns.Add(col.Key, false, true);
                            else
                                grid.DisplayLayout.Bands[0].SortedColumns.Add(col.Key, true, true);
                        }
                    }
                }

                int count = grid.DisplayLayout.Bands[0].SortedColumns.Cast<UltraGridColumn>().Count(col => col.IsGroupByColumn);

                foreach (UltraGridColumn col in grdConsolidation.DisplayLayout.Bands[0].Columns)
                {
                    UltraGridColumn columnExportGrid = grid.DisplayLayout.Bands[0].Columns[col.Key];

                    if (!col.Hidden)
                    {
                        columnExportGrid.Header.VisiblePosition = col.Header.VisiblePosition;
                        grid.DisplayLayout.Bands[0].Columns[col.Key].Hidden = false;
                    }
                }

                if (count > 0)
                {
                    _isExportGridNeeded = true;
                    foreach (UltraGridColumn var in grid.DisplayLayout.Bands[0].Columns)
                    {
                        if (var.Hidden || var.Key.Equals(PMConstants.COL_TradeDate))
                            continue;
                        if (!grid.DisplayLayout.Bands[0].Summaries.Exists(var.Key))
                            CreateColumnSummary(var);
                    }
                    _isExportGridNeeded = false;
                }

                grid.DisplayLayout.Bands[0].ColHeadersVisible = false;
                grid.DisplayLayout.Override.GroupByColumnsHidden = DefaultableBoolean.False;
                grid.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
                grid.DisplayLayout.Override.GroupBySummaryDisplayStyle = GroupBySummaryDisplayStyle.SummaryCells;
                grid.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows
                | SummaryDisplayAreas.RootRowsFootersOnly
                | SummaryDisplayAreas.Bottom;
                grid.DisplayLayout.Override.SummaryFooterCaptionVisible = DefaultableBoolean.False;
                grid.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.CheckOnDisplay;
                gridDict.Add(excelSheetName, grid);
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

        public event EventHandler<EventArgs<string, string, bool>> SaveAllGridLayouts;

        private void asDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveAllGridLayouts != null)
                {
                    SaveAllGridLayouts(this, new EventArgs<string, string, bool>(ParentKey, string.Empty, true));
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

        private void forAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveAllGridLayouts != null)
                {
                    SaveAllGridLayouts(this, new EventArgs<string, string, bool>(ParentKey, string.Empty, false));
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

        private void currentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveAllGridLayouts != null)
                {
                    SaveAllGridLayouts(this, new EventArgs<string, string, bool>(ParentKey, ParentKey, false));
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

        #endregion Context Menu items here!

        private void grdConsolidation_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePoint = new Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;

                    if (cell != null)
                    {
                        cell.Row.Activate();
                        _CurrentUltraGridGroupByRow = cell.Row.ParentRow;
                        grdConsolidation.ActiveRow = cell.Row;
                    }
                    else
                    {
                        _CurrentUltraGridGroupByRow = element.SelectableItem as UltraGridRow;
                    }
                    if ((element is FixedSummaryLineUIElement || element.GetAncestor(typeof(SummaryFooterUIElement)) is SummaryFooterUIElement))
                    {
                        grdConsolidation.ActiveRow = element.SelectableItem as UltraGridGroupByRow;
                    }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    // Get a reference to the UIElement at the current mouse position
                    UIElement thisElem = grdConsolidation.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));

                    // Exit the event handler if no UIElement is found
                    if (thisElem == null)
                        return;

                    if (thisElem is HeaderUIElement || thisElem.GetAncestor(typeof(HeaderUIElement)) is HeaderUIElement)
                    {
                        if (thisElem is TextUIElement)
                        {
                            _StrSelectedGroupByCol = String.Empty;
                            _StrSelectedGroupByCol = PMConstantsHelper.GetColumnNameByCaption(((TextUIElement)(thisElem)).Text);
                        }
                        else if (thisElem is SortIndicatorUIElement)
                        {
                            _StrSelectedGroupByCol = String.Empty;
                            ColumnHeader header = (ColumnHeader)(((SortIndicatorUIElement)(thisElem)).SelectableItem);
                            _StrSelectedGroupByCol = PMConstantsHelper.GetColumnNameByCaption(header.Caption);
                        }
                    }
                    //See if the UIElement at the current mouse position is a GroupByBoxUIElement,
                    // or if it is contained as a child of a GroupByBoxUIElement
                    if (thisElem is GroupByBoxUIElement ||
                        thisElem.GetAncestor(typeof(GroupByBoxUIElement)) is GroupByBoxUIElement)
                    {
                        _StrSelectedGroupByCol = String.Empty;
                        if (thisElem is TextUIElement)
                        {
                            _StrSelectedGroupByCol = PMConstantsHelper.GetColumnNameByCaption(((TextUIElement)(thisElem)).Text);
                            _columnSorted = grdConsolidation.DisplayLayout.Bands[0].SortedColumns[_StrSelectedGroupByCol];
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Do Nothing as user can try again
            }
        }

        private string _StrSelectedGroupByCol = String.Empty;

        private void grdConsolidation_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (_previousGroupingCount != _count)
                    {
                        ShowHideGroupLevelButtons();
                    }
                    _previousGroupingCount = _count;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (contextMenuStrip.Items.ContainsKey("showSummaryToolStripMenuItem") && contextMenuStrip.Items.ContainsKey("hideSummaryToolStripMenuItem"))
                    {
                        AllowRowSummaries allowRowSummaries = grdConsolidation.DisplayLayout.Bands[0].Layout.Override.AllowRowSummaries;
                        if (allowRowSummaries == AllowRowSummaries.True)
                        {
                            contextMenuStrip.Items["showSummaryToolStripMenuItem"].Visible = false;
                            contextMenuStrip.Items["hideSummaryToolStripMenuItem"].Visible = true;
                        }
                        else
                        {
                            contextMenuStrip.Items["showSummaryToolStripMenuItem"].Visible = true;
                            contextMenuStrip.Items["hideSummaryToolStripMenuItem"].Visible = false;
                        }
                    }
                    //   contextMenuStrip.Items["tradeToolStripMenuItem"].Enabled = true;
                    //    contextMenuStrip.Items["depthToolStripMenuItem"].Enabled = true;
                    //   contextMenuStrip.Items["chartToolStripMenuItem"].Enabled = true;
                    //   contextMenuStrip.Items["optionChainToolStripMenuItem"].Enabled = true;
                    contextMenuStrip.Items["exitAllToolStripMenuItem"].Enabled = ModuleManager.CheckModulePermissioning(PranaModules.TRADING_TICKET_MODULE, PranaModules.TRADING_TICKET_MODULE);
                    //contextMenuStrip.Items["exitAllToolStripMenuItem"].Enabled = false;
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        cell.Row.Activate();
                        UltraGridRow row = grdConsolidation.ActiveRow;
                        if (row != null)
                        {
                            ExposurePnlCacheItem consolidatedInfo = (ExposurePnlCacheItem)row.ListObject;

                            if (consolidatedInfo.OrderSideTagValue == FIXConstants.SIDE_Buy || consolidatedInfo.OrderSideTagValue == FIXConstants.SIDE_SellShort || consolidatedInfo.OrderSideTagValue == FIXConstants.SIDE_Buy_Open || consolidatedInfo.OrderSideTagValue == FIXConstants.SIDE_Sell_Open)
                            {
                                contextMenuStrip.Items["exitAllToolStripMenuItem"].Enabled = true;
                            }
                            if (consolidatedInfo.Asset == AssetCategory.FX.ToString())
                            {
                                contextMenuStrip.Items["tradeToolStripMenuItem"].Enabled = false;
                                contextMenuStrip.Items["depthToolStripMenuItem"].Enabled = false;
                                contextMenuStrip.Items["chartToolStripMenuItem"].Enabled = false;
                                contextMenuStrip.Items["optionChainToolStripMenuItem"].Enabled = false;
                            }
                        }
                    }
                }
                _StrSelectedGroupByCol = String.Empty;
            }
            catch (Exception)
            {
                //Do Nothing as user can try again

            }
        }

        private void SetGroupbyRowColors(UltraGridGroupByRow row)
        {
            try
            {
                if (row != null && !_pmAppearances.IsDefaultGroupingColor)
                {
                    if (!row.HasParent())
                    {
                        //let it to be the default black color
                        row.Appearance.BackColor = Color.FromArgb(_pmAppearances.BackColor1);
                        row.Appearance.ForeColor = Color.FromArgb(_pmAppearances.ForeColor1);
                        if (row.Rows.Count > 0)
                        {
                            row.Rows.Refresh(RefreshRow.RefreshDisplay);
                        }
                        SetGridBackColor();
                    }
                    else
                    {
                        //this is the intermediate node
                        if (!row.ParentRow.HasParent())
                        {
                            row.Appearance.BackColor = Color.FromArgb(_pmAppearances.BackColor2);
                            row.Appearance.ForeColor = Color.FromArgb(_pmAppearances.ForeColor2);
                            if (row.Rows.Count > 0)
                            {
                                row.Rows.Refresh(RefreshRow.FireInitializeRow);
                            }
                        }
                        else
                        {
                            if (!row.ParentRow.ParentRow.HasParent())
                            {
                                row.Appearance.BackColor = Color.FromArgb(_pmAppearances.BackColor3);
                                row.Appearance.ForeColor = Color.FromArgb(_pmAppearances.ForeColor3);
                                if (row.Rows.Count > 0)
                                {
                                    row.Rows.Refresh(RefreshRow.FireInitializeRow);
                                }
                            }
                            else
                            {
                                // this is the child node
                                row.Appearance.BackColor = Color.FromArgb(_pmAppearances.BackColor4);
                                row.Appearance.ForeColor = Color.FromArgb(_pmAppearances.ForeColor4);
                            }
                        }
                    }
                    row.Appearance.BackGradientStyle = GradientStyle.None;
                }
                if (_pmAppearances.ShowGridLinesbyGroup)
                {
                    if (row != null)
                    {
                        row.Appearance.BorderColor = Color.FromArgb(88, 88, 90);
                        row.Appearance.BorderAlpha = Alpha.Opaque;
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

        private void SetGridBackColor()
        {

            if (!_pmAppearances.IsDefaultRowBackColor)
            {
                ((ISupportInitialize)(grdConsolidation)).BeginInit();
                Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance
                {
                    BackColor =
                        Color.FromArgb(UltraWinGridUtils.IsGrouppingAppliedOnGrid(grdConsolidation)
                            ? _pmAppearances.BackColor1
                            : _pmAppearances.RowBgColor)
                };
                grdConsolidation.DisplayLayout.Appearance = appearance1;
                ((ISupportInitialize)(grdConsolidation)).EndInit();
            }
        }

        private void grdConsolidation_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            try
            {
                SetGroupbyRowColors(e.Row);
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

        private void grdConsolidation_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                //Improved the code to handle hang scenario on mouse scroll. 
                //  if value of delta(no of scrolls requested through mouse wheel), is greater then 3 then here will be page up/down instead of moving rows one by one. 
                if (Math.Abs(e.Delta) > 120 * 3)
                {
                    HandledMouseEventArgs ee = (HandledMouseEventArgs)e;
                    ee.Handled = true;
                    grdConsolidation.DisplayLayout.RowScrollRegions[0].Scroll(e.Delta > 1
                        ? RowScrollAction.PageUp
                        : RowScrollAction.PageDown);
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

        #region Expand Collapse Logic

        private bool Level1Expanded;
        private bool Level2Expanded;
        private bool Level3Expanded;
        private bool Level4Expanded;
        private bool Level5Expanded;

        /// <summary>
        /// Handles the Click event of the expandCollapseLevel1ToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void expandCollapseLevel1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Level1Expanded)
                {
                    CollapseLevel1();
                }
                else
                {
                    ExpandLevel1();
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
        /// Handles the Click event of the expandCollapseLevel2ToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void expandCollapseLevel2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Level2Expanded)
                {
                    CollapseLevel2();
                }
                else
                {
                    ExpandLevel2();
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
        /// Handles the Click event of the expandCollapseLevel3ToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void expandCollapseLevel3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Level3Expanded)
                {
                    CollapseLevel3();
                }
                else
                {
                    ExpandLevel3();
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
        /// Handles the Click event of the expandCollapseLevel4ToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void expandCollapseLevel4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Level4Expanded)
                {
                    CollapseLevel4();
                }
                else
                {
                    ExpandLevel4();
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
        /// Handles the Click event of the expandCollapseLevel5ToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void expandCollapseLevel5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Level5Expanded)
                {
                    CollapseLevel5();
                }
                else
                {
                    ExpandLevel5();
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

        private void ExpandLevel1()
        {
            try
            {
                if (grdConsolidation != null && grdConsolidation.Rows != null && grdConsolidation.Rows.Count != 0)
                    grdConsolidation.Rows.ExpandAll(false);
                Level1Expanded = true;
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

        private void CollapseLevel1()
        {
            try
            {
                if (grdConsolidation != null && grdConsolidation.Rows != null && grdConsolidation.Rows.Count != 0)
                    grdConsolidation.Rows.CollapseAll(true);
                Level1Expanded = false;
                Level2Expanded = false;
                Level3Expanded = false;
                Level4Expanded = false;
                Level5Expanded = false;
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

        private void ExpandLevel2()
        {
            try
            {
                if (grdConsolidation != null && grdConsolidation.Rows != null)
                {
                    for (int i = 0; i < grdConsolidation.Rows.Count; i++)
                    {
                        if (grdConsolidation.Rows[i].HasChild() && grdConsolidation.Rows[i].ChildBands[0].Rows != null)
                        {
                            foreach (UltraGridRow childRow in grdConsolidation.Rows[i].ChildBands[0].Rows)
                            {
                                childRow.ExpandAncestors();
                                childRow.Expanded = true;
                            }
                        }
                    }
                }
                Level1Expanded = true;
                Level2Expanded = true;
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

        private void CollapseLevel2()
        {
            try
            {
                if (grdConsolidation != null && grdConsolidation.Rows != null)
                {
                    for (int i = 0; i < grdConsolidation.Rows.Count; i++)
                    {
                        if (grdConsolidation.Rows[i].HasChild() && grdConsolidation.Rows[i].ChildBands[0].Rows != null && grdConsolidation.Rows[i].ChildBands[0].Rows.Count != 0)
                        {
                            grdConsolidation.Rows[i].ChildBands[0].Rows.CollapseAll(true);
                        }
                    }
                }
                Level2Expanded = false;
                Level3Expanded = false;
                Level4Expanded = false;
                Level5Expanded = false;
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

        private void ExpandLevel3()
        {
            try
            {
                if (grdConsolidation != null && grdConsolidation.Rows != null)
                {
                    for (int i = 0; i < grdConsolidation.Rows.Count; i++)
                    {
                        if (grdConsolidation.Rows[i].HasChild() && grdConsolidation.Rows[i].ChildBands[0].Rows != null)
                        {
                            foreach (UltraGridRow childRow in grdConsolidation.Rows[i].ChildBands[0].Rows)
                            {
                                if (childRow.HasChild() && childRow.ChildBands[0].Rows != null)
                                {
                                    foreach (UltraGridRow grandChild in childRow.ChildBands[0].Rows)
                                    {
                                        grandChild.ExpandAncestors();
                                        grandChild.Expanded = true;
                                    }
                                }
                            }
                        }
                    }
                }
                Level1Expanded = true;
                Level2Expanded = true;
                Level3Expanded = true;
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

        private void CollapseLevel3()
        {
            try
            {
                if (grdConsolidation != null && grdConsolidation.Rows != null)
                {
                    for (int i = 0; i < grdConsolidation.Rows.Count; i++)
                    {
                        if (grdConsolidation.Rows[i].HasChild() && grdConsolidation.Rows[i].ChildBands[0].Rows != null)
                        {
                            foreach (UltraGridRow childRow in grdConsolidation.Rows[i].ChildBands[0].Rows)
                            {
                                if (childRow.HasChild() && childRow.ChildBands[0].Rows != null && childRow.ChildBands[0].Rows.Count != 0)
                                {
                                    childRow.ChildBands[0].Rows.CollapseAll(true);
                                }
                            }
                        }
                    }
                }
                Level3Expanded = false;
                Level4Expanded = false;
                Level5Expanded = false;
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

        private void ExpandLevel4()
        {
            try
            {
                if (grdConsolidation != null && grdConsolidation.Rows != null)
                {
                    for (int i = 0; i < grdConsolidation.Rows.Count; i++)
                    {
                        if (grdConsolidation.Rows[i].HasChild() && grdConsolidation.Rows[i].ChildBands[0].Rows != null)
                        {
                            foreach (UltraGridRow childRow in grdConsolidation.Rows[i].ChildBands[0].Rows)
                            {
                                if (childRow.HasChild() && childRow.ChildBands[0].Rows != null)
                                {
                                    foreach (UltraGridRow grandChild in childRow.ChildBands[0].Rows)
                                    {
                                        if (grandChild.HasChild() && grandChild.ChildBands[0].Rows != null)
                                        {
                                            foreach (UltraGridRow grandChildLevel1 in grandChild.ChildBands[0].Rows)
                                            {
                                                grandChildLevel1.ExpandAncestors();
                                                grandChildLevel1.Expanded = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Level1Expanded = true;
                Level2Expanded = true;
                Level3Expanded = true;
                Level4Expanded = true;
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

        private void CollapseLevel4()
        {
            try
            {
                if (grdConsolidation != null && grdConsolidation.Rows != null)
                {
                    for (int i = 0; i < grdConsolidation.Rows.Count; i++)
                    {
                        if (grdConsolidation.Rows[i].HasChild() && grdConsolidation.Rows[i].ChildBands[0].Rows != null)
                        {
                            foreach (UltraGridRow childRow in grdConsolidation.Rows[i].ChildBands[0].Rows)
                            {
                                if (childRow.HasChild() && childRow.ChildBands[0].Rows != null)
                                {
                                    foreach (UltraGridRow grandChildRow in childRow.ChildBands[0].Rows)
                                    {
                                        if (grandChildRow.HasChild() && grandChildRow.ChildBands[0].Rows != null && grandChildRow.ChildBands[0].Rows.Count != 0)
                                        {
                                            grandChildRow.ChildBands[0].Rows.CollapseAll(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Level4Expanded = false;
                Level5Expanded = false;
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

        private void ExpandLevel5()
        {
            try
            {
                if (grdConsolidation != null && grdConsolidation.Rows != null)
                {
                    for (int i = 0; i < grdConsolidation.Rows.Count; i++)
                    {
                        if (grdConsolidation.Rows[i].HasChild() && grdConsolidation.Rows[i].ChildBands[0].Rows != null)
                        {
                            foreach (UltraGridRow childRow in grdConsolidation.Rows[i].ChildBands[0].Rows)
                            {
                                if (childRow.HasChild() && childRow.ChildBands[0].Rows != null)
                                {
                                    foreach (UltraGridRow grandChild in childRow.ChildBands[0].Rows)
                                    {
                                        if (grandChild.HasChild() && grandChild.ChildBands[0].Rows != null)
                                        {
                                            foreach (UltraGridRow grandChildLevel1 in grandChild.ChildBands[0].Rows)
                                            {
                                                if (grandChildLevel1.HasChild() && grandChildLevel1.ChildBands[0].Rows != null)
                                                {
                                                    foreach (UltraGridRow grandChildLevel2 in grandChildLevel1.ChildBands[0].Rows)
                                                    {
                                                        grandChildLevel2.ExpandAncestors();
                                                        grandChildLevel2.ExpandAll();
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
                Level1Expanded = true;
                Level2Expanded = true;
                Level3Expanded = true;
                Level4Expanded = true;
                Level5Expanded = true;
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

        private void CollapseLevel5()
        {
            try
            {
                if (grdConsolidation != null && grdConsolidation.Rows != null)
                {
                    for (int i = 0; i < grdConsolidation.Rows.Count; i++)
                    {
                        if (grdConsolidation.Rows[i].HasChild() && grdConsolidation.Rows[i].ChildBands[0].Rows != null)
                        {
                            foreach (UltraGridRow childRow in grdConsolidation.Rows[i].ChildBands[0].Rows)
                            {
                                if (childRow.HasChild() && childRow.ChildBands[0].Rows != null)
                                {
                                    foreach (UltraGridRow grandChildRow in childRow.ChildBands[0].Rows)
                                    {
                                        if (grandChildRow.HasChild() && grandChildRow.ChildBands[0].Rows != null)
                                        {
                                            foreach (UltraGridRow grandChildLevel1 in grandChildRow.ChildBands[0].Rows)
                                            {
                                                if (grandChildLevel1.HasChild() && grandChildLevel1.ChildBands[0].Rows != null && grandChildLevel1.ChildBands[0].Rows.Count != 0)
                                                {
                                                    grandChildLevel1.ChildBands[0].Rows.CollapseAll(true);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Level5Expanded = false;
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

        #endregion Expand Collapse Logic

        /// <summary>
        /// It hides either the account colu based on the position type for this consolidation view.
        /// </summary>
        private void SetAccountStrategyNameCol()
        {
            UltraGridBand band = grdConsolidation.DisplayLayout.Bands[0];
            UltraGridColumn colInternalAccount = null;
            try
            {
                if (band != null)
                {
                    if (band.Columns.Exists(OrderFields.PROPERTY_LEVEL1NAME))
                    {
                        colInternalAccount = band.Columns[OrderFields.PROPERTY_LEVEL1NAME];
                    }
                }

                if (_positionTypes == ExPNLData.Account)
                {
                    if (colInternalAccount != null)
                    {
                        colInternalAccount.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
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
        /// The method below cancels the grouping action taken when the grouping reaches upto three levels 
        /// and also shows the respective information message. Otherwise it lets user do the grouping.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdConsolidation_BeforeRowFilterDropDown(object sender, BeforeRowFilterDropDownEventArgs e)
        {
            string filterColumn = string.Empty;
            try
            {
                // if some filter is applied    
                if (filterColumn != string.Empty)
                {
                    if (e.Column.ToString() == filterColumn)
                    {
                        int valuelistcount = e.ValueList.ValueListItems.Count;
                        for (int i = valuelistcount - 1; i >= 0; i--)
                        {
                            if (e.ValueList.ValueListItems[i].DisplayText == _accountFilterKey)
                            {
                            }
                            else
                            {
                                e.ValueList.ValueListItems.Remove(e.ValueList.ValueListItems[i]);
                            }
                        }
                    }
                }

                if (e.Column.Key.Equals(PMConstants.COL_TradeDate) || e.Column.Key.Equals(PMConstants.COL_StartTradeDate) || e.Column.Key.Equals(PMConstants.COL_ExpirationDate))
                {
                    e.ValueList.ValueListItems.Insert(4, "(Today)", "(Today)");
                }
                if (e.Column.Key.Equals(PMConstants.COL_TradeDate) || e.Column.Key.Equals(PMConstants.COL_StartTradeDate))
                {
                    e.ValueList.ValueListItems.Insert(5, "(BeforeToday)", "(Before Today)");
                }
                else if (e.Column.Key.Equals("ExpirationDate"))
                {
                    ValueList vl = e.ValueList;
                    int count = vl.ValueListItems.Count;
                    //List<ValueListItem> items = new List<ValueListItem>(count);
                    List<DateTime> items = new List<DateTime>(count);
                    for (int i = count - 1; i >= 0; i--)
                    {
                        ValueListItem item = vl.ValueListItems[i];
                        DateTime dateValue;
                        if (DateTime.TryParse(item.ToString(), out dateValue))
                        {
                            items.Add(dateValue);
                            vl.ValueListItems.RemoveAt(i);
                        }
                    }
                    items.Sort();
                    foreach (DateTime item in items)
                    {
                        e.ValueList.ValueListItems.Add(item.ToShortDateString());
                    }
                }
                else if (e.Column.Key.Equals("Level1Name"))
                {
                    ColumnFilter masterFundColFilter = grdConsolidation.DisplayLayout.Bands[0].ColumnFilters["MasterFund"];
                    GridColumnFilterDetails masterFundColumnFilterDetails = masterFundColFilter.FilterConditions != null ? new GridColumnFilterDetails(e.Column.Key,
                            masterFundColFilter.FilterConditions.Cast<FilterCondition>().ToList())
                        : new GridColumnFilterDetails(masterFundColFilter.Key);

                    ColumnFilter fundColFilter = grdConsolidation.DisplayLayout.Bands[0].ColumnFilters["Level1Name"];
                    GridColumnFilterDetails fundColumnFilterDetails = fundColFilter.FilterConditions != null ? new GridColumnFilterDetails(e.Column.Key,
                            fundColFilter.FilterConditions.Cast<FilterCondition>().ToList())
                        : new GridColumnFilterDetails(fundColFilter.Key);



                    List<string> filteredMasterFundList = (masterFundColumnFilterDetails.DynamicFilterConditionList.Cast<object>()
                                .Select(filtercondtion => Regex.Match(filtercondtion.ToString(), @"'([^']*)"))
                                .Where(match => match.Success)
                                .Select(match => match.Groups[1].Value)).ToList();

                    List<string> filteredFundList = (fundColumnFilterDetails.DynamicFilterConditionList.Cast<object>()
                                .Select(filtercondtion => Regex.Match(filtercondtion.ToString(), @"'([^']*)"))
                                .Where(match => match.Success)
                                .Select(match => match.Groups[1].Value)).ToList();

                    if (filteredMasterFundList.Count > 0 && filteredFundList.Count <= 0)
                    {
                        List<int> MFAccounts = new List<int>();
                        List<int> masterFundids = filteredMasterFundList.Select(masterFundname => CachedDataManager.GetInstance.GetMasterFundID(masterFundname)).ToList();
                        Dictionary<int, List<int>> masterFundAssociationDictionary = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                        List<int> allAccountList = new List<int>();
                        List<string> permittedUserAccounts = CachedDataManager.GetInstance.GetAllAccountIDsForUser();
                        foreach (KeyValuePair<int, List<int>> masterFundAssociationKvp in masterFundAssociationDictionary.Where(masterFundAssociationKvp => masterFundids.Contains(masterFundAssociationKvp.Key)))
                        {
                            allAccountList.AddRange(masterFundAssociationKvp.Value);
                        }

                        MFAccounts.AddRange(allAccountList.Where(account => permittedUserAccounts.Contains(account.ToString())));

                        ValueList vl = e.ValueList;
                        //Skipping first 4 dropdown values (All, Custom, Blanks, NonBlanks) 
                        for (int i = 4; i < vl.ValueListItems.Count; i++)
                        {
                            string item = vl.ValueListItems[i].ToString();

                            if (!MFAccounts.Contains(CachedDataManager.GetInstance.GetAccountID(item)))
                            {
                                vl.ValueListItems.RemoveAt(i);
                                i--;
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

        #region Sorting Logic

        private CustomGroupByRowsSorterForText _customSorterText = new CustomGroupByRowsSorterForText();
        private CustomGroupByRowsSorterForAlphaNumeric _customSorterAlphaNumeric = new CustomGroupByRowsSorterForAlphaNumeric();
        private CustomGroupByRowsSorterForDate _customSorterDate = new CustomGroupByRowsSorterForDate();
        private CustomRowSorterForDate _customSorterDateForRow = new CustomRowSorterForDate();
        private CustomRowSorterForText _customSorterTextForRow = new CustomRowSorterForText();
        private List<string> _allowedGroupedSortedColumnNumeric = new List<string>();
        private List<string> _allowedGroupedSortedColumnText = new List<string>();
        private List<string> _allowedGroupedSortedColumnAlphaNumeric = new List<string>();
        private List<string> _allowedGroupedSortedColumnDates = new List<string>();
        private List<string> _allowedGroupedSortedColumnNumericCumText = new List<string>();

        public event EventHandler<EventArgs<List<string>>> SendGroupedColumnNames;
        List<string> _groupByColumnList = new List<string>();
        private int _count;
        private int _previousGroupingCount;

        private void grdConsolidation_BeforeSortChange(object sender, BeforeSortChangeEventArgs e)
        {
            try
            {
                _count = 0;
                _groupByColumnList.Clear();
                int ColumnsGroupingLevelOnPM = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("ColumnsGroupingLevelOnPM"));
                foreach (UltraGridColumn var in e.SortedColumns)
                {
                    if (var.IsGroupByColumn)
                    {
                        _groupByColumnList.Add(var.Key);
                        _count++;
                    }
                }
                if (_count > ColumnsGroupingLevelOnPM)
                {
                    MessageBox.Show("Positions can not be grouped by more than " + ColumnsGroupingLevelOnPM + " columns.", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                }

                if (SendGroupedColumnNames != null)
                    SendGroupedColumnNames(this, new EventArgs<List<string>>(_groupByColumnList));
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

        private void grdConsolidation_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                this.grdConsolidation.BeginUpdate();
                int sortCount = grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Count;
                if (sortCount > 0)
                {
                    UltraGridColumn sortColumn;
                    if (grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Contains(_columnSorted))
                    {
                        sortColumn = grdConsolidation.DisplayLayout.Bands[0].SortedColumns[_columnSorted.Key];
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                        //Set the Group by row summary display settings 
                        RowSummarySettingsForAccountView();
                        return;
                    }
                    if (sortColumn.Formula != null && !(sortColumn.DataType == typeof(Double)))
                    {
                        //Set the Group by row summary display settings 
                        RowSummarySettingsForAccountView();
                        return;
                    }
                    else if (_allowedGroupedSortedColumnText.Contains(sortColumn.Key))
                    {
                        if (!sortColumn.IsGroupByColumn
                            && !_customViewPreference.GroupByColumnsCollection.Contains(sortColumn.Key)
                            && _allowedGroupedSortedColumnText.Contains(_StrSelectedGroupByCol))
                        {
                            _customSorterText.Column = sortColumn.Key;
                            _customSorterText.SortIndicator = sortColumn.SortIndicator;
                            foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                            {
                                var.GroupByComparer = null;
                            }
                            int count = 0;
                            foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                            {
                                //In case the level 1 groupby column is sorted descending, then reverse the sorting to be applied.
                                if (var.IsGroupByColumn && count < 1)
                                {
                                    if (var.SortIndicator == SortIndicator.Descending)
                                    {
                                        if (_customSorterText.SortIndicator == SortIndicator.Ascending)
                                        {
                                            _customSorterText.SortIndicator = SortIndicator.Descending;
                                        }
                                        else if (_customSorterText.SortIndicator == SortIndicator.Descending)
                                        {
                                            _customSorterText.SortIndicator = SortIndicator.Ascending;
                                        }
                                    }
                                    count++;
                                    //Overrides the default sorting applied on level1 grouping 
                                    //by assigning the custom sorting to Group By Comparer of level1 group by column.
                                    var.GroupByComparer = _customSorterText;
                                }
                                if (_count == 0 && _allowedGroupedSortedColumnNumericCumText.Contains(sortColumn.Key))
                                {
                                    var.SortComparer = _customSorterTextForRow;
                                }
                            }
                            grdConsolidation.DisplayLayout.Bands[0].SortedColumns[sortColumn.Key].GroupByComparer = _customSorterText;
                        }
                        else if (sortColumn.IsGroupByColumn && _customViewPreference.GroupByColumnsCollection.Contains(sortColumn.Key))
                        {
                            _customSorterText.Column = sortColumn.Key;
                            _customSorterText.SortIndicator = sortColumn.SortIndicator;
                            int count = 0;
                            foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                            {
                                //In case the level 1 groupby column is sorted descending, then reverse the sorting to be applied.
                                if (var.IsGroupByColumn && count < 1)
                                {
                                    if (var.SortIndicator == SortIndicator.Descending)
                                    {
                                        if (_customSorterText.SortIndicator == SortIndicator.Ascending)
                                        {
                                            _customSorterText.SortIndicator = SortIndicator.Descending;
                                        }
                                        else if (_customSorterText.SortIndicator == SortIndicator.Descending)
                                        {
                                            _customSorterText.SortIndicator = SortIndicator.Ascending;
                                        }
                                    }
                                    count++;
                                    //Overrides the default sorting applied on level1 grouping 
                                    //by assigning the custom sorting to Group By Comparer of level1 group by column.
                                    var.GroupByComparer = _customSorterText;
                                }
                            }
                        }
                        else
                        {
                            foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                            {
                                var.GroupByComparer = null;
                            }
                        }
                    }
                    else if (_allowedGroupedSortedColumnAlphaNumeric.Contains(sortColumn.Key))
                    {
                        if (!sortColumn.IsGroupByColumn
                            && !_customViewPreference.GroupByColumnsCollection.Contains(sortColumn.Key)
                            && _allowedGroupedSortedColumnAlphaNumeric.Contains(_StrSelectedGroupByCol))
                        {
                            _customSorterAlphaNumeric.Column = sortColumn.Key;
                            _customSorterAlphaNumeric.SortIndicator = sortColumn.SortIndicator;
                            sortColumn.GroupByComparer = _customSorterAlphaNumeric;
                            int count = 0;
                            foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                            {
                                //In case the level 1 groupby column is sorted descending, then reverse the sorting to be applied.
                                if (var.IsGroupByColumn && count < 1)
                                {
                                    if (var.SortIndicator == SortIndicator.Descending)
                                    {
                                        if (_customSorterAlphaNumeric.SortIndicator == SortIndicator.Ascending)
                                        {
                                            _customSorterAlphaNumeric.SortIndicator = SortIndicator.Descending;
                                        }
                                        else if (_customSorterAlphaNumeric.SortIndicator == SortIndicator.Descending)
                                        {
                                            _customSorterAlphaNumeric.SortIndicator = SortIndicator.Ascending;
                                        }
                                    }
                                    count++;
                                    //Overrides the default sorting applied on level1 grouping 
                                    //by assigning the custom sorting to Group By Comparer of level1 group by column.
                                    var.GroupByComparer = _customSorterAlphaNumeric;
                                }
                                if (_count == 0 && _allowedGroupedSortedColumnNumericCumText.Contains(sortColumn.Key))
                                {
                                    var.SortComparer = _customSorterTextForRow;
                                }
                            }
                        }
                        else if (sortColumn.IsGroupByColumn && _customViewPreference.GroupByColumnsCollection.Contains(sortColumn.Key))
                        {
                            foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                            {
                                var.GroupByComparer = null;
                            }
                            _customSorterAlphaNumeric.Column = sortColumn.Key;
                            _customSorterAlphaNumeric.SortIndicator = sortColumn.SortIndicator;
                            sortColumn.GroupByComparer = _customSorterAlphaNumeric;
                            int count = 0;
                            foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                            {
                                //In case the level 1 groupby column is sorted descending, then reverse the sorting to be applied.
                                if (var.IsGroupByColumn && count < 1)
                                {
                                    if (var.SortIndicator == SortIndicator.Descending)
                                    {
                                        if (_customSorterAlphaNumeric.SortIndicator == SortIndicator.Ascending)
                                        {
                                            _customSorterAlphaNumeric.SortIndicator = SortIndicator.Descending;
                                        }
                                        else if (_customSorterAlphaNumeric.SortIndicator == SortIndicator.Descending)
                                        {
                                            _customSorterAlphaNumeric.SortIndicator = SortIndicator.Ascending;
                                        }
                                    }
                                    count++;
                                    //Overrides the default sorting applied on level1 grouping 
                                    //by assigning the custom sorting to Group By Comparer of level1 group by column.
                                    var.GroupByComparer = _customSorterAlphaNumeric;
                                }
                            }
                        }
                        else
                        {
                            foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                            {
                                var.GroupByComparer = null;
                            }
                        }
                    }
                    else if (_allowedGroupedSortedColumnDates.Contains(sortColumn.Key))
                    {
                        if (!sortColumn.IsGroupByColumn
                            && !_customViewPreference.GroupByColumnsCollection.Contains(sortColumn.Key)
                            && _allowedGroupedSortedColumnDates.Contains(_StrSelectedGroupByCol))
                        {
                            _customSorterDate.Column = sortColumn.Key;
                            _customSorterDate.SortIndicator = sortColumn.SortIndicator;
                            sortColumn.GroupByComparer = _customSorterDate;
                            foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                            {
                                if (_count == 0)
                                {
                                    var.SortComparer = _customSorterDateForRow;
                                }
                                else
                                {
                                    var.GroupByComparer = _customSorterDate;
                                }
                            }
                        }
                        else if (sortColumn.IsGroupByColumn && _customViewPreference.GroupByColumnsCollection.Contains(sortColumn.Key))
                        {
                            _customSorterDate.Column = sortColumn.Key;
                            _customSorterDate.SortIndicator = sortColumn.SortIndicator;
                            sortColumn.GroupByComparer = _customSorterDate;

                            int count = 0;
                            foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                            {
                                var.SortComparer = null;
                                //In case the level 1 groupby column is sorted descending, then reverse the sorting to be applied.
                                if (var.IsGroupByColumn && count < 1)
                                {
                                    if (var.SortIndicator == SortIndicator.Descending)
                                    {
                                        if (_customSorterDate.SortIndicator == SortIndicator.Ascending)
                                        {
                                            _customSorterDate.SortIndicator = SortIndicator.Descending;
                                        }
                                        else if (_customSorterDate.SortIndicator == SortIndicator.Descending)
                                        {
                                            _customSorterDate.SortIndicator = SortIndicator.Ascending;
                                        }
                                    }
                                    count++;
                                    //Overrides the default sorting applied on level1 grouping 
                                    //by assigning the custom sorting to Group By Comparer of level1 group by column.
                                    var.GroupByComparer = _customSorterDate;
                                }
                            }
                        }
                        else
                        {
                            foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                            {
                                var.SortComparer = null;
                                var.GroupByComparer = null;
                            }
                        }
                    }
                    //https://jira.nirvanasolutions.com:8443/browse/CI-4105 
                    LogExtensions.LoggerWriteMessage(LoggingConstants.CATEGORY_GENERAL, "PM Grid Sorting:- " + _customSorterDate.SortIndicator.ToString() + " Beofre CaptureCurrentUIState");
                    CaptureCurrentUIState();
                    LogExtensions.LoggerWriteMessage(LoggingConstants.CATEGORY_GENERAL, "PM Grid Sorting:- " + _customSorterDate.SortIndicator.ToString() + " After CaptureCurrentUIState");
                    grdConsolidation.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                    LogExtensions.LoggerWriteMessage(LoggingConstants.CATEGORY_GENERAL, "PM Grid Sorting:- " + _customSorterDate.SortIndicator.ToString() + " After RefreshSort");
                    RestoreCurrentUIState();
                    this.grdConsolidation.ActiveRowScrollRegion.Scroll(RowScrollAction.Top);
                }
                RowSummarySettingsForAccountView();
                if (CustomThemeHelper.ApplyTheme)
                {
                    grdConsolidation.DisplayLayout.Override.RowSelectors = UltraWinGridUtils.IsGrouppingAppliedOnGrid(grdConsolidation) ? DefaultableBoolean.False : DefaultableBoolean.True;
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
            finally
            {
                this.grdConsolidation.EndUpdate();
            }
        }

        private void RowSummarySettingsForAccountView()
        {
            try
            {
                bool groupedBySomeColumn = grdConsolidation.DisplayLayout.Bands[0].SortedColumns.Cast<UltraGridColumn>().Any(col => col.IsGroupByColumn);
                if (!groupedBySomeColumn)
                {
                    grdConsolidation.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.Bottom;
                }
                else
                {
                    //Changes made to show Summary at Grouping level on PM for both Custom Level and Account Level.
                    //http://jira.nirvanasolutions.com:8080/browse/QUAD-43

                    grdConsolidation.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.Bottom;
                    grdConsolidation.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.InGroupByRows;
                    grdConsolidation.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.RootRowsFootersOnly;
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
        #endregion Sorting Logic
        private void IncreasePositionSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OrderSingle order = new OrderSingle();
                string orderSide = null;
                string asset = AssetCategory.Equity.ToString();
                string symbol = string.Empty;
                string accountName = string.Empty;
                string masterFund = string.Empty;
                string counterpartyname = string.Empty;
                bool isSwap = false;
                Dictionary<int, double> accountWithPostion = new Dictionary<int, double>();
                if (_CurrentUltraGridGroupByRow != null)
                {
                    UltraGridRow[] individualRows = _CurrentUltraGridGroupByRow.ChildBands[0].Rows.GetFilteredInNonGroupByRows();
                    if (individualRows != null && individualRows.Length > 0)
                    {
                        ExposurePnlCacheItem info = null;
                        foreach (UltraGridRow rowIndividual in individualRows)
                        {
                            info = (ExposurePnlCacheItem)rowIndividual.ListObject;
                            if (info != null)
                            {
                                if (info.Level1ID == -1)
                                    continue;
                                counterpartyname = info.CounterPartyName;
                                if (accountWithPostion.ContainsKey(info.Level1ID))

                                    accountWithPostion[info.Level1ID] += info.Quantity;
                                else
                                    accountWithPostion.Add(info.Level1ID, info.Quantity);

                                if (accountName == string.Empty)
                                    accountName = info.Level1Name;
                                else if (accountName != info.Level1Name)
                                    accountName = "Multiple";
                                if (masterFund == string.Empty)
                                    masterFund = info.MasterFund;
                                else if (masterFund != info.MasterFund)
                                    masterFund = "Multiple";
                            }
                        }
                        if (info != null)
                        {
                            symbol = info.Symbol;
                            order.BloombergSymbol = info.BloombergSymbol;
                            order.BloombergSymbolWithExchangeCode = info.BloombergSymbolWithExchangeCode;
                            order.FactSetSymbol = info.FactSetSymbol;
                            order.ActivSymbol = info.ActivSymbol;
                            asset = info.Asset;
                            isSwap = info.IsSwap;
                            orderSide = info.OrderSideTagValue;
                        }
                    }
                }
                else
                {
                    if (grdConsolidation.ActiveRow == null || grdConsolidation.ActiveRow.ListObject == null)
                    {
                        MessageBox.Show("Please select a valid row", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ExposurePnlCacheItem activeRowObject = (ExposurePnlCacheItem)grdConsolidation.ActiveRow.ListObject;
                    symbol = activeRowObject.Symbol;
                    order.BloombergSymbol = activeRowObject.BloombergSymbol;
                    order.BloombergSymbolWithExchangeCode = activeRowObject.BloombergSymbolWithExchangeCode;
                    order.FactSetSymbol = activeRowObject.FactSetSymbol;
                    order.ActivSymbol = activeRowObject.ActivSymbol;
                    asset = activeRowObject.Asset;
                    isSwap = activeRowObject.IsSwap;
                    RowsCollection rows = grdConsolidation.ActiveRow.ParentCollection;

                    foreach (UltraGridRow row in rows)
                    {
                        ExposurePnlCacheItem info = (ExposurePnlCacheItem)row.ListObject;
                        counterpartyname = info.CounterPartyName;
                    }
                }

                order.Quantity = Convert.ToDouble(_userTradingTicketUiPrefs.Quantity);
                order.OrderSideTagValue = orderSide;
                order.Symbol = symbol;
                order.AssetName = asset;
                order.AssetID = CachedDataManager.GetInstance.GetAssetID(asset);
                order.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(counterpartyname);
                order.TradeAttribute6 = masterFund == "Multiple" ? string.Empty : masterFund;
                order.Level1ID = accountName == "Multiple" ? -1 : CachedDataManager.GetInstance.GetAccountID(accountName);
                order.CounterPartyName = counterpartyname;
                order.SwapParameters = isSwap ? new SwapParameters() : null;
                order.PMType = PMType.Increase;
                if (TradeClick != null)
                {
                    TradeClick(this, new EventArgs<OrderSingle, Dictionary<int, double>>(order, accountWithPostion));
                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row to trade!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void symbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OrderSingle order = new OrderSingle();
                string orderSide;
                string asset = AssetCategory.Equity.ToString();
                string symbol = string.Empty;
                string accountName = string.Empty;
                string masterFund = string.Empty;
                string counterpartyname = string.Empty;
                bool isSwap = false;
                double quantity = 0.0;
                Dictionary<int, double> accountWithPostion = new Dictionary<int, double>();
                if (_CurrentUltraGridGroupByRow != null)
                {
                    UltraGridRow[] individualRows = _CurrentUltraGridGroupByRow.ChildBands[0].Rows.GetFilteredInNonGroupByRows();
                    if (individualRows != null && individualRows.Length > 0)
                    {
                        ExposurePnlCacheItem info = null;
                        foreach (UltraGridRow rowIndividual in individualRows)
                        {
                            info = (ExposurePnlCacheItem)rowIndividual.ListObject;
                            if (info != null)
                            {
                                if (info.Level1ID == -1)
                                    continue;
                                counterpartyname = info.CounterPartyName;
                                if (accountWithPostion.ContainsKey(info.Level1ID))

                                    accountWithPostion[info.Level1ID] += info.Quantity;
                                else
                                    accountWithPostion.Add(info.Level1ID, info.Quantity);

                                quantity = quantity + info.Quantity; //Quantity is now side adjusted
                                if (accountName == string.Empty)
                                    accountName = info.Level1Name;
                                else if (accountName != info.Level1Name)
                                    accountName = "Multiple";
                                if (masterFund == string.Empty)
                                    masterFund = info.MasterFund;
                                else if (masterFund != info.MasterFund)
                                    masterFund = "Multiple";
                            }
                        }
                        if (info != null)
                        {
                            symbol = info.Symbol;
                            order.BloombergSymbol = info.BloombergSymbol;
                            order.BloombergSymbolWithExchangeCode = info.BloombergSymbolWithExchangeCode;
                            order.FactSetSymbol = info.FactSetSymbol;
                            order.ActivSymbol = info.ActivSymbol;
                            asset = info.Asset;
                            isSwap = info.IsSwap;
                        }
                    }
                }
                else
                {
                    if (grdConsolidation.ActiveRow == null || grdConsolidation.ActiveRow.ListObject == null)
                    {
                        MessageBox.Show("Please select a valid row", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ExposurePnlCacheItem activeRowObject = (ExposurePnlCacheItem)grdConsolidation.ActiveRow.ListObject;
                    symbol = activeRowObject.Symbol;
                    order.BloombergSymbol = activeRowObject.BloombergSymbol;
                    order.BloombergSymbolWithExchangeCode = activeRowObject.BloombergSymbolWithExchangeCode;
                    order.FactSetSymbol = activeRowObject.FactSetSymbol;
                    order.ActivSymbol = activeRowObject.ActivSymbol;
                    asset = activeRowObject.Asset;
                    isSwap = activeRowObject.IsSwap;
                    RowsCollection rows = grdConsolidation.ActiveRow.ParentCollection;

                    foreach (UltraGridRow row in rows)
                    {
                        ExposurePnlCacheItem info = (ExposurePnlCacheItem)row.ListObject;
                        counterpartyname = info.CounterPartyName;

                        if (info.Symbol.Equals(symbol))
                        {
                            quantity = quantity + info.Quantity;
                        }
                    }
                }

                order.Quantity = Math.Abs(quantity); //Net quantity
                // depending on quantity, order side of the new trade is determined. 
                switch (asset)
                {
                    case "FutureOption":
                    case "EquityOption":
                        orderSide = quantity < 0 ? FIXConstants.SIDE_Buy_Closed : FIXConstants.SIDE_Sell_Closed;
                        if (quantity.Equals(0))
                        {
                            MessageBox.Show("Net Position is already Zero for the selected Symbol", "Trade Generation error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        break;

                    default:
                        orderSide = quantity < 0 ? FIXConstants.SIDE_Buy_Closed : FIXConstants.SIDE_Sell;
                        if (quantity.Equals(0))
                        {
                            MessageBox.Show("Net Position is already Zero for the selected Symbol", "Trade Generation error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        break;
                }
                order.OrderSideTagValue = orderSide;
                order.Symbol = symbol;
                order.AssetName = asset;
                order.AssetID = CachedDataManager.GetInstance.GetAssetID(asset);
                order.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(counterpartyname);
                order.TradeAttribute6 = masterFund == "Multiple" ? string.Empty : masterFund;
                order.Level1ID = accountName == "Multiple" ? -1 : CachedDataManager.GetInstance.GetAccountID(accountName);
                order.CounterPartyName = counterpartyname;
                order.SwapParameters = isSwap ? new SwapParameters() : null;
                order.PMType = PMType.Close;
                if (TradeClick != null)
                {
                    TradeClick(this, new EventArgs<OrderSingle, Dictionary<int, double>>(order, accountWithPostion));
                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row to trade!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string orderSide;
                string asset = AssetCategory.Equity.ToString();
                string symbol = string.Empty;
                string counterPartyName = string.Empty;
                int accountID = int.MinValue;
                int level2ID = int.MinValue;
                double quantity = 0.0;
                bool isSwap = false;
                OrderSingle order = new OrderSingle();
                if (_isAccountView || (grdConsolidation.ActiveRow is UltraGridGroupByRow))
                {
                    string accountSymbol = _accountFilterKey;

                    if (_CurrentUltraGridGroupByRow != null)
                    {
                        if (grdConsolidation.ActiveRow is UltraGridGroupByRow && !_isAccountView)
                        {
                            if (!grdConsolidation.ActiveRow.Band.Columns["Level1Name"].Hidden)
                            {
                                accountSymbol = grdConsolidation.ActiveRow.ChildBands[0].Rows.SummaryValues["Level1Name"].SummaryText;
                                if (accountSymbol.Equals("-"))
                                {
                                    MessageBox.Show(this, "Symbol allocated in multiple Accounts,Please select a valid row to close account position!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "Account column is required is to perform this action. Please fetch Account column onto grid before proceeding!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        UltraGridRow[] individualRows = _CurrentUltraGridGroupByRow.ChildBands[0].Rows.GetAllNonGroupByRows();
                        if (individualRows != null && individualRows.Length > 0)
                        {
                            foreach (UltraGridRow rowIndividual in individualRows)
                            {
                                var info = (ExposurePnlCacheItem)rowIndividual.ListObject;
                                if (info != null && info.Level1Name.Equals(accountSymbol))
                                {
                                    quantity = quantity + info.Quantity;   // quantity is now by default side adjusted                                   
                                    symbol = info.Symbol;
                                    order.BloombergSymbol = info.BloombergSymbol;
                                    order.BloombergSymbolWithExchangeCode = info.BloombergSymbolWithExchangeCode;
                                    order.FactSetSymbol = info.FactSetSymbol;
                                    order.ActivSymbol = info.ActivSymbol;
                                    asset = info.Asset;
                                    accountID = info.Level1ID;
                                    level2ID = info.Level2ID;
                                    counterPartyName = info.CounterPartyName;
                                    isSwap = info.IsSwap;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (grdConsolidation.ActiveRow != null)
                        {
                            var activeRowObject = (ExposurePnlCacheItem)grdConsolidation.ActiveRow.ListObject;
                            symbol = activeRowObject.Symbol;
                            order.BloombergSymbol = activeRowObject.BloombergSymbol;
                            order.BloombergSymbolWithExchangeCode = activeRowObject.BloombergSymbolWithExchangeCode;
                            order.FactSetSymbol = activeRowObject.FactSetSymbol;
                            order.ActivSymbol = activeRowObject.ActivSymbol;
                            asset = activeRowObject.Asset;
                            accountID = activeRowObject.Level1ID;
                            level2ID = activeRowObject.Level2ID;
                            isSwap = activeRowObject.IsSwap;
                            RowsCollection rows = grdConsolidation.ActiveRow.ParentCollection;

                            foreach (UltraGridRow row in rows)
                            {
                                ExposurePnlCacheItem info = (ExposurePnlCacheItem)row.ListObject;

                                if (info != null && activeRowObject != null && info.Symbol.Equals(symbol) && info.Level1Name.Equals(_accountFilterKey))
                                {
                                    quantity = quantity + info.Quantity;
                                    counterPartyName = info.CounterPartyName;
                                }
                            }
                        }
                    }
                    order.Quantity = Math.Abs(quantity);
                    // depending on quantity, order side of the new trade is determined. 
                    switch (asset)
                    {
                        case "FutureOption":
                        case "EquityOption":
                            orderSide = quantity < 0 ? FIXConstants.SIDE_Buy_Closed : FIXConstants.SIDE_Sell_Closed;
                            break;

                        default:
                            orderSide = quantity < 0 ? FIXConstants.SIDE_Buy_Closed : FIXConstants.SIDE_Sell;
                            break;
                    }

                    if (quantity.Equals(0))
                    {
                        MessageBox.Show("Net Position is already Zero for the selected account", "Trade Generation error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    order.Symbol = symbol;
                    order.OrderSideTagValue = orderSide;
                    order.Level1ID = accountID;
                    order.Level2ID = level2ID;
                    order.AssetName = asset;
                    order.AssetID = CachedDataManager.GetInstance.GetAssetID(asset);
                    order.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(counterPartyName);
                    order.CounterPartyName = counterPartyName;
                    order.SwapParameters = isSwap ? new SwapParameters() : null;
                    if (TradeClick != null)
                    {
                        TradeClick(this, new EventArgs<OrderSingle, Dictionary<int, double>>(order, null));
                    }
                }
                else
                {
                    if (grdConsolidation.ActiveRow != null)
                    {
                        var activeRowObject = (ExposurePnlCacheItem)grdConsolidation.ActiveRow.ListObject;
                        symbol = activeRowObject.Symbol;
                        asset = activeRowObject.Asset;
                        var account = activeRowObject.Level1Name;
                        accountID = activeRowObject.Level1ID;
                        level2ID = activeRowObject.Level2ID;
                        isSwap = activeRowObject.IsSwap;
                        RowsCollection rows = grdConsolidation.ActiveRow.ParentCollection;

                        foreach (UltraGridRow row in rows)
                        {
                            ExposurePnlCacheItem info = (ExposurePnlCacheItem)row.ListObject;

                            if (info != null && activeRowObject != null && info.Symbol.Equals(symbol) && info.Level1Name.Equals(account))
                            {
                                quantity = quantity + info.Quantity;
                                counterPartyName = info.CounterPartyName;
                            }
                        }
                        order.Quantity = Math.Abs(quantity); //Net quantity
                        // depending on quantity, order side of the new trade is determined. 
                        switch (asset)
                        {
                            case "FutureOption":
                            case "EquityOption":
                                orderSide = quantity < 0 ? FIXConstants.SIDE_Buy_Closed : FIXConstants.SIDE_Sell_Closed;
                                break;

                            default:
                                orderSide = quantity < 0 ? FIXConstants.SIDE_Buy_Closed : FIXConstants.SIDE_Sell;
                                break;
                        }
                        if (quantity.Equals(0))
                        {
                            MessageBox.Show("Net Position is already Zero for the selected account", "Trade Generation error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        order.Symbol = symbol;
                        order.OrderSideTagValue = orderSide;
                        order.Level1ID = accountID;
                        order.Level2ID = level2ID;
                        order.AssetName = asset;
                        order.AssetID = CachedDataManager.GetInstance.GetAssetID(asset);
                        order.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(counterPartyName);
                        order.CounterPartyName = counterPartyName;
                        order.SwapParameters = isSwap ? new SwapParameters() : null;
                        if (TradeClick != null)
                        {
                            TradeClick(this, new EventArgs<OrderSingle, Dictionary<int, double>>(order, null));
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

        public class VisiblePosComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                UltraGridColumn xCol = (UltraGridColumn)x;
                UltraGridColumn yCol = (UltraGridColumn)y;
                int val;
                if ((xCol.Header.VisiblePosition == yCol.Header.VisiblePosition))
                {
                    val = 0;
                }
                else if ((xCol.Header.VisiblePosition > yCol.Header.VisiblePosition))
                {
                    val = 1;
                }
                else
                {
                    val = -1;
                }
                return val;
            }
        }

        #region Custom Column Chooser Code
        private void AddSummaryAndSortingForCalculatedColumns(UltraGridColumn newCol)
        {
            try
            {
                if (newCol.DataType == typeof(double))
                {
                    //CustomSummaries CustomSummariesFactory = new CustomSummaries();
                    ICustomSummaryCalculator calcForNewCol = new CustomSummariesFactory.Sum(newCol.Key);
                    SummarySettings s = grdConsolidation.DisplayLayout.Bands[0].Summaries.Add(newCol.Key, SummaryType.Custom, calcForNewCol, newCol, SummaryPosition.UseSummaryPositionColumn, newCol);
                    s.DisplayFormat = "{0:#,###0.0000}";
                    s.Appearance.TextHAlign = HAlign.Right;
                    if (!_allowedGroupedSortedColumnNumeric.Contains(newCol.Key))
                    {
                        _allowedGroupedSortedColumnNumeric.Add(newCol.Key);
                    }
                }
                else
                {
                    _allowedGroupedSortedColumnText.Add(newCol.Key);
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

        private void grdConsolidation_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                //Bharat Kumar Jangir (08 November 2013)
                //Put this ColumnChooserEnabled check for disabling the Column Chooser for Read only Tabs (PM Tab to Write)
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-2820
                if (grdConsolidation.DisplayLayout.ColumnChooserEnabled == DefaultableBoolean.True)
                {
                    (FindForm()).AddCustomColumnChooser(grdConsolidation);
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

        private void grdConsolidation_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            e.CustomRowFiltersDialog.PaintDynamicForm();
        }

        #endregion Custom Column Chooser Code

        private void grdConsolidation_InitializePrint(object sender, CancelablePrintEventArgs e)
        {
            UltraGridLayout gridLayout = e.PrintLayout;
            gridLayout.Appearance.BackColor = Color.White;
            gridLayout.Appearance.ForeColor = Color.Black;
            try
            {
                foreach (UltraGridRow rowLevel1 in gridLayout.Rows)
                {
                    rowLevel1.Appearance.ForeColor = Color.Black;
                    rowLevel1.Appearance.BackColor = Color.White;
                    if (rowLevel1.IsGroupByRow)
                    {
                        foreach (UltraGridRow rowLevel2 in rowLevel1.ChildBands[0].Rows)
                        {
                            rowLevel2.Appearance.ForeColor = Color.Black;
                            rowLevel2.Appearance.BackColor = Color.White;
                            if (rowLevel2.IsGroupByRow)
                            {
                                foreach (UltraGridRow rowLevel3 in rowLevel2.ChildBands[0].Rows)
                                {
                                    rowLevel3.Appearance.ForeColor = Color.Black;
                                    rowLevel3.Appearance.BackColor = Color.White;
                                    if (rowLevel3.IsGroupByRow)
                                    {
                                        foreach (UltraGridRow rowLevel4 in rowLevel3.ChildBands[0].Rows)
                                        {
                                            rowLevel4.Appearance.ForeColor = Color.Black;
                                            rowLevel4.Appearance.BackColor = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                gridLayout.Override.GroupBySummaryValueAppearance.BackColor = Color.White;
                gridLayout.Override.GroupBySummaryValueAppearance.ForeColor = Color.Black;
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


        internal void WrapInitializeRow()
        {
            try
            {
                // unwrapped first as may be wrapped before
                grdConsolidation.InitializeRow -= grdConsolidation_InitializeRow;
                grdConsolidation.InitializeRow += grdConsolidation_InitializeRow;
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

        internal void UnwrapInitializeRow()
        {
            try
            {
                grdConsolidation.InitializeRow -= grdConsolidation_InitializeRow;
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

        //requirement for setting maxm number of visible columns from admin. If a PopUp is required, can be given in second check
        private void grdConsolidation_AfterColPosChanged(object sender, AfterColPosChangedEventArgs e)
        {
            try
            {
                if (e.ColumnPosChangedType == PosChangedType.HiddenStateChanged)
                {
                    if (!e.ColumnHeaders[0].Column.Hidden)
                    {
                        int visibleColCount = e.ColumnHeaders[0].Band.Columns.Cast<UltraGridColumn>().Count(col => !col.Hidden);
                        if (visibleColCount > _uiPrefs.NumberOfVisibleColumnsAllowed)
                        {
                            e.ColumnHeaders[0].Column.Hidden = true;
                            MessageBox.Show("Maximum no. of visible columns reached. Please remove any other column before selecting this column");
                            return;
                        }
                    }
                    if (AfterColPositionChanged != null)
                        AfterColPositionChanged(null, e);
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

        public event EventHandler<EventArgs<string, string, int>> TaxlotsRequested;

        private void grdConsolidation_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            string callerGridName = grdConsolidation.Name;
            try
            {
                if (grdConsolidation.ActiveRow != null)
                {
                    if (grdConsolidation.ActiveRow is UltraGridGroupByRow)
                    {
                        return;
                    }
                    var item = (ExposurePnlCacheItem)grdConsolidation.ActiveRow.ListObject;
                    if (TaxlotsRequested != null && item != null)
                    {
                        TaxlotsRequested(this, new EventArgs<string, string, int>(item.ID, callerGridName, item.Level1ID)); // id shud be passed
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void closetaxlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdConsolidation.ActiveRow != null && !(grdConsolidation.ActiveRow is UltraGridGroupByRow))
                {
                    OnTradeClick();
                }
                else
                {
                    MessageBox.Show(this, "Please select a valid trade to close!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void IncreasePositionTaxlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdConsolidation.ActiveRow != null && !(grdConsolidation.ActiveRow is UltraGridGroupByRow))
                {
                    OnIncreasePositionClick();
                }
                else
                {
                    MessageBox.Show(this, "Please select a valid trade to Increase Position!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        //private void addToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (grdConsolidation.ActiveRow != null && !(grdConsolidation.ActiveRow is UltraGridGroupByRow))
        //        {
        //            ExposurePnlCacheItem activeRowObject = null;
        //            if (grdConsolidation.ActiveRow != null)
        //            {
        //                activeRowObject = (ExposurePnlCacheItem)grdConsolidation.ActiveRow.ListObject;
        //            }
        //            OrderSingle order = new OrderSingle();
        //            if (activeRowObject != null)
        //            {
        //                order.OrderSideTagValue = activeRowObject.OrderSideTagValue;
        //                order.Symbol = activeRowObject.Symbol;
        //                order.Quantity = Math.Abs(activeRowObject.Quantity);
        //                order.Level1ID = activeRowObject.Level1ID;
        //                order.Level2ID = activeRowObject.Level2ID;
        //                order.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(activeRowObject.CounterPartyName);
        //                order.CounterPartyName = activeRowObject.CounterPartyName;

        //                if (TradeClick != null)
        //                {
        //                    TradeClick(this, new EventArgs<OrderSingle>(order));
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show(this, "Please select a valid row to trade!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //Ankit 21jun2012 :Extended Matching

        private void timer1_Tick(object sender, EventArgs e)
        {
            //   This is a one-shot operation, so disable the timer whenever we get in here
            try
            {
                timerFindRow.Enabled = false;

                if (m_strSearchString != "")
                {
                    Cursor.Current = Cursors.WaitCursor;
                    LocateRow();
                    Cursor.Current = Cursors.Default;

                    //   Always clear the search string when the search is complete
                    m_strSearchString = "";
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
                // Just to make sure that due to any exception the timer is not left as active one.
                timerFindRow.Enabled = false;
            }
        }

        private void LocateRow()
        {
            //   We need an active row to continue, so set it if it isn't already
            try
            {
                if (grdConsolidation.ActiveRow == null)
                    grdConsolidation.ActiveRow = grdConsolidation.GetRow(ChildRow.First);

                if (grdConsolidation.DisplayLayout.Bands[0].Columns[PMConstants.COL_Symbol].IsGroupByColumn)
                {
                    FindGroupBySymbolRow();
                }
                else if (grdConsolidation.ActiveRow.HasParent() || grdConsolidation.ActiveRow.IsGroupByRow)
                {
                    FindGroupedRow();
                }
                else
                {
                    FindRow();
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

        private string open = string.Empty;

        private void FindGroupBySymbolRow()
        {
            try
            {
                bool bFromBeginning = false;
                UltraGridRow oRow;
                string strCellValue = string.Empty;

            start:
                if (bFromBeginning)
                    oRow = grdConsolidation.GetRow(ChildRow.First);
                else
                {
                    oRow = grdConsolidation.ActiveRow;
                    if (oRow.HasNextSibling())
                    {
                        oRow = oRow.GetSibling(SiblingRow.Next);
                    }
                }

                while (oRow != null)
                {
                    if (oRow.IsGroupByRow)
                    {
                        open = oRow.Description;
                        strCellValue = strCellValue.ToUpper();
                        int length = Math.Min(m_strSearchString.Length, open.Length);
                        if (m_strSearchString.Length > open.Length)
                        {
                            oRow = oRow.GetSibling(SiblingRow.Next);
                            continue;
                        }
                        if (open.Substring(0, length) == m_strSearchString.Substring(0, length))
                        {
                            grdConsolidation.ActiveRow = oRow;
                            return;
                        }
                        oRow = oRow.GetSibling(SiblingRow.Next);
                    }
                    else
                    {
                        oRow = oRow.ParentRow;
                        oRow = oRow.GetSibling(SiblingRow.Next);
                    }
                }

                //   We dont want to get stuck in here
                if (bFromBeginning) { return; }

                //   If we didn't find it, search again, from the beginning
                bFromBeginning = true;
                goto start;
            }
            catch (Exception ex)
            {
                m_strSearchString = "";
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void FindGroupedRow()
        {
            try
            {
                bool bFromBeginning = false;
                UltraGridRow oRow;
            start:
                if (bFromBeginning)
                    oRow = grdConsolidation.GetRow(ChildRow.First);
                else
                {
                    oRow = grdConsolidation.ActiveRow;

                    if (oRow.HasChild())
                    {
                        oRow = oRow.GetChild(ChildRow.First);
                    }
                    else if (oRow.HasNextSibling())
                    {
                        oRow = oRow.GetSibling(SiblingRow.Next);
                    }
                    else
                    {
                        oRow = oRow.ParentRow;
                        oRow = oRow.GetSibling(SiblingRow.Next);
                    }
                }

                while (oRow != null)
                {
                    //   Get the value of the "Symbol" cell on this row; make it uppercase
                    if (oRow.IsGroupByRow)
                    {
                        oRow = oRow.GetChild(ChildRow.First);
                    }
                    else
                    {
                        var strCellValue = oRow.Cells[PMConstants.COL_Symbol].GetText(MaskMode.Raw);
                        strCellValue = strCellValue.ToUpper();
                        //   or the value of the Symbol cell dictates how many characters we use in the comparison
                        int length = Math.Min(m_strSearchString.Length, strCellValue.Length);
                        if (m_strSearchString.Length > strCellValue.Length)
                        {
                            if (oRow.HasNextSibling())
                            {
                                oRow = oRow.GetSibling(SiblingRow.Next);
                            }
                            else if (oRow.HasParent())
                            {
                                oRow = oRow.ParentRow;
                                //oRow.CollapseAll();
                                oRow = oRow.GetSibling(SiblingRow.Next);
                            }
                            continue;
                        }
                        if (strCellValue.Substring(0, length) == m_strSearchString.Substring(0, length))
                        {
                            grdConsolidation.ActiveRow = oRow;
                            oRow.Selected = true;
                            return;
                        }

                        //   Move the next row and repeat the comparison

                        if (oRow.HasNextSibling())
                        {
                            oRow = oRow.GetSibling(SiblingRow.Next);
                        }
                        else if (oRow.HasParent())
                        {
                            oRow = oRow.ParentRow;
                            oRow = oRow.GetSibling(SiblingRow.Next);
                        }
                    }
                }

                //   We dont want to get stuck in here
                if (bFromBeginning) { return; }

                //   If we didn't find it, search again, from the beginning
                bFromBeginning = true;
                goto start;
            }
            catch (Exception ex)
            {
                m_strSearchString = "";
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void FindRow()
        {
            try
            {
                bool bFromBeginning = false;
                UltraGridRow oRow;
            start:
                if (bFromBeginning)
                    oRow = grdConsolidation.GetRow(ChildRow.First);
                else
                {
                    if (grdConsolidation.ActiveRow != null && !grdConsolidation.ActiveRow.IsGroupByRow)
                    {
                        oRow = grdConsolidation.ActiveRow;
                        oRow = oRow.GetSibling(SiblingRow.Next);
                    }
                    else
                    {
                        return;
                    }
                }

                while (oRow != null)
                {
                    //   Get the value of the "Symbol" cell on this row; make it uppercase
                    if (!oRow.IsGroupByRow)
                    {
                        if (oRow.HasParent())
                            open = oRow.ParentRow.Description;
                        var strCellValue = oRow.Cells[PMConstants.COL_Symbol].GetText(MaskMode.Raw);
                        strCellValue = strCellValue.ToUpper();

                        //   Compare as many characters as we can. Whichever is smaller, the search string
                        //   or the value of the LastName cell dictates how many characters we use in the comparison
                        int length = Math.Min(m_strSearchString.Length, strCellValue.Length);
                        if (m_strSearchString.Length > strCellValue.Length)
                        {
                            oRow = oRow.GetSibling(SiblingRow.Next);
                            continue;
                        }
                        if (strCellValue.Substring(0, length) == m_strSearchString.Substring(0, length))
                        {
                            grdConsolidation.ActiveRow = oRow;
                            oRow.Selected = true;
                            return;
                        }
                        //   Move the next row and repeat the comparison
                        oRow = oRow.GetSibling(SiblingRow.Next);
                    }
                    else
                    {
                        oRow = oRow.Description == open ? oRow.GetChild(ChildRow.First) : oRow.GetSibling(SiblingRow.Next);
                    }
                }

                //   We dont want to get stuck in here
                if (bFromBeginning) { return; }

                //   If we didn't find it, search again, from the beginning
                bFromBeginning = true;
                goto start;
            }
            catch (Exception ex)
            {
                m_strSearchString = "";
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Ankit 21jun2012 :Extended Matching

        private void grdConsolidation_KeyDown(object sender, KeyEventArgs e)
        {
            //   Navigation keys and ESC clear the search string
            try
            {
                if (e.KeyData == Keys.Right ||
                        e.KeyData == Keys.Left ||
                        e.KeyData == Keys.Up ||
                        e.KeyData == Keys.Down ||
                        e.KeyData == Keys.Tab ||
                        e.KeyData == Keys.Escape)
                {
                    //   Disable the timer on navigational keys
                    timerFindRow.Enabled = false;
                    m_strSearchString = "";
                    return;
                }

                //   If the control key is down, no search
                if (e.Control) { return; }

                //   We want alphabetic keys, case insensitive and Numeric keys
                if (e.KeyValue >= 65 && e.KeyValue <= 90)
                {
                    string alphabetOnly = e.KeyData.ToString();
                    alphabetOnly = alphabetOnly.Substring(0, 1);
                    m_strSearchString = m_strSearchString + alphabetOnly;
                    m_strSearchString = m_strSearchString.ToUpper();
                    //   Enable the timer
                    timerFindRow.Enabled = true;
                }
                if (e.KeyValue == 190)
                {
                    m_strSearchString = m_strSearchString + ".";
                    timerFindRow.Enabled = true;
                }
                if (e.KeyValue == 189)
                {
                    m_strSearchString = m_strSearchString + "-";
                    timerFindRow.Enabled = true;
                }
                if (e.KeyValue == 32)
                {
                    m_strSearchString = m_strSearchString + " ";
                    timerFindRow.Enabled = true;
                }
                if (e.KeyValue == 186)
                {
                    m_strSearchString = m_strSearchString + ":";
                    timerFindRow.Enabled = true;
                }
                if (e.KeyValue >= 48 && e.KeyValue <= 57 || e.KeyValue >= 96 && e.KeyValue <= 105)
                {
                    string num;
                    if (e.KeyValue >= 48 && e.KeyValue <= 57)
                    {
                        num = e.KeyData.ToString().Substring(1, 1);
                    }
                    else
                    {
                        num = e.KeyData.ToString().Substring(6, 1);
                    }
                    m_strSearchString = m_strSearchString + num;
                    //   Enable the timer
                    timerFindRow.Enabled = true;
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

        public void UpdateGridPreferences(bool isInitializeRow, bool onUpdatePreference)
        {
            try
            {
                _pmAppearances = PMAppearanceManager.PMAppearance;
                SetGridFontSize();
                grdConsolidation.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);

                if (!_pmAppearances.IsDefaultRowBackColor)
                    grdConsolidation.DisplayLayout.Override.RowAppearance.BackColor = Color.FromArgb(_pmAppearances.RowBgColor);

                if (!_pmAppearances.IsDefaultAlternateRowColor)
                    grdConsolidation.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.FromArgb(_pmAppearances.AlternateColor);

                SetGridBackColor();

                if (_pmAppearances.ShowGridLines)
                {
                    grdConsolidation.DisplayLayout.Override.RowAppearance.BackGradientStyle = GradientStyle.None;
                    grdConsolidation.DisplayLayout.Override.BorderStyleCell = UIElementBorderStyle.None;
                    grdConsolidation.DisplayLayout.Override.RowAppearance.BorderColor = Color.FromArgb(88, 88, 90);
                    grdConsolidation.DisplayLayout.Override.RowAppearance.BorderAlpha = Alpha.Opaque;
                }
                else
                {
                    grdConsolidation.DisplayLayout.Override.RowAppearance.BackGradientStyle = GradientStyle.None;
                    grdConsolidation.DisplayLayout.Override.RowAppearance.BorderColor = Color.FromArgb(88, 88, 90);
                    grdConsolidation.DisplayLayout.Override.RowAppearance.BorderAlpha = Alpha.Transparent;
                }
                if (_pmAppearances.WrapHeader)
                {
                    grdConsolidation.DisplayLayout.Override.WrapHeaderText = DefaultableBoolean.True;
                    if (onUpdatePreference)
                        UpdateHeaderWrapHeader(true);
                }
                else
                {
                    grdConsolidation.DisplayLayout.Override.WrapHeaderText = DefaultableBoolean.False;
                    if (onUpdatePreference)
                        UpdateHeaderWrapHeader(false);
                }
                if (_pmAppearances.RowColorbasis.Equals("0"))
                {
                    grdConsolidation.DisplayLayout.Appearances["Negative"].ForeColor = Color.FromArgb(_pmAppearances.OrderSideSellColor);
                    grdConsolidation.DisplayLayout.Appearances["Positive"].ForeColor = Color.FromArgb(_pmAppearances.OrderSideBuyColor);
                }
                else if (_pmAppearances.RowColorbasis.Equals("1"))
                {
                    grdConsolidation.DisplayLayout.Appearances["Negative"].ForeColor = Color.FromArgb(_pmAppearances.DayPnlNegativeColor);
                    grdConsolidation.DisplayLayout.Appearances["Positive"].ForeColor = Color.FromArgb(_pmAppearances.DayPnlPositiveColor);
                }
                if (isInitializeRow)
                {
                    grdConsolidation.Rows.Refresh(RefreshRow.FireInitializeRow);
                }
                else
                {
                    grdConsolidation.Refresh();
                }
                SetRowColorBasis(_pmAppearances.RowColorbasis);
                foreach (UltraGridColumn col in grdConsolidation.DisplayLayout.Bands[0].Columns)
                {
                    string caption = PMConstantsHelper.GetCaptionByColumnName(col.Key);
                    if (!caption.Equals(PMConstants.CAP_TradeDate) && !caption.Equals(PMConstants.CAP_SettlementDate) && !caption.Equals(PMConstants.CAP_LastUpdatedUTC) && !caption.Equals(PMConstants.CAP_ExDividendDate) && !caption.Equals(PMConstants.CAP_ExpirationDate))
                        col.Format = PMConstantsHelper.GetFormatStringByCaption(caption);
                    if (caption == PMConstants.CAP_StartTradeDate)
                    {
                        col.Format = DateTimeConstants.DateFormat;
                    }
                    if (grdConsolidation.DisplayLayout.Bands[0].Summaries.Exists(col.Key))
                    {
                        SummarySettings s = grdConsolidation.DisplayLayout.Bands[0].Summaries[col.Key];
                        s.DisplayFormat = PMConstantsHelper.GetFormatStringByCaptionForSummary(caption);
                        if (caption == PMConstants.CAP_StartTradeDate)
                        {
                            s.DisplayFormat = "{0:" + DateTimeConstants.DateFormat + "}";
                        }
                    }
                    col.AutoSizeMode = ColumnAutoSizeMode.AllRowsInBand;
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

        public void UpdateHeaderWrapHeader(bool WrapHeader)
        {
            try
            {
                //if (WrapHeader)
                //{
                //    foreach (UltraGridColumn col in grdConsolidation.DisplayLayout.Bands[0].Columns)
                //    {
                //        Graphics g = CreateGraphics();
                //        string maxLengthWord = col.Header.Caption.Split(' ').ToArray().OrderBy(w => w.Length).LastOrDefault();
                //        Font font = new Font("Tahoma", 13);
                //        var stringSize = g.MeasureString(maxLengthWord, font);
                //        col.Width = Convert.ToInt32(stringSize.Width) + 30;
                //    }
                //}
                //else
                //{
                //    foreach (PreferenceGridColumn selectedCols in _customViewPreference.SelectedColumnsCollection)
                //    {
                //        if (grdConsolidation.DisplayLayout.Bands[0].Columns.Exists(selectedCols.Name))
                //        {
                //            var col = grdConsolidation.DisplayLayout.Bands[0].Columns[selectedCols.Name];
                //            if (col != null)
                //            {
                //                if (selectedCols.Width == 0)
                //                    col.AutoSizeMode = ColumnAutoSizeMode.Default;
                //                else
                //                    col.Width = selectedCols.Width;
                //            }
                //        }
                //    }
                //}
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

        private void SetGridFontSize()
        {
            try
            {
                if (!_pmAppearances.IsDefaultGridFontSize)
                {
                    float fontSize = Convert.ToSingle(_pmAppearances.FontSizeGrid);
                    Font oldFont = grdConsolidation.Font;
                    Font newFont = new Font(oldFont.FontFamily, fontSize, oldFont.Style, oldFont.Unit, oldFont.GdiCharSet, oldFont.GdiVerticalFont);
                    FontData newFontData = new FontData();
                    grdConsolidation.Font = newFont;
                    if (grdConsolidation.DisplayLayout != null)
                    {
                        //grdConsolidation.DisplayLayout.Override.SummaryValueAppearance = new FontData() { };
                        //grdConsolidation.DisplayLayout.Override.RowSelectorAppearance.fon
                        grdConsolidation.DisplayLayout.Override.SummaryValueAppearance.ForeColor = Color.FromArgb(_pmAppearances.SummaryColor);
                        grdConsolidation.DisplayLayout.Override.RowSelectorAppearance.BackColor = Color.FromArgb(_pmAppearances.RowSelectorBackColor);
                        grdConsolidation.DisplayLayout.Override.RowSelectorAppearance.ForeColor = Color.FromArgb(_pmAppearances.RowSelectorForColor);
                        grdConsolidation.DisplayLayout.Override.RowSelectorAppearance.BackGradientStyle = GradientStyle.None;
                    }
                    grdConsolidation.Font = newFont;
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

        private void grdConsolidation_BeforeRowDeactivate(object sender, CancelEventArgs e)
        {
            //This is the default color as seen on group by row
            try
            {
                if (grdConsolidation.ActiveRow != null && grdConsolidation.ActiveRow.Appearance.BackColor != Color.FromArgb(120, 84, 84, 84) && grdConsolidation.ActiveRow.IsGroupByRow)
                {
                    UltraGridGroupByRow activeRow = (UltraGridGroupByRow)grdConsolidation.ActiveRow;
                    SetGroupbyRowColors(activeRow);
                    activeRow.Appearance.BackGradientStyle = GradientStyle.None;
                    activeRow.Appearance.BackColor2 = Color.FromArgb(0, 0, 0, 0);
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

        private void grdConsolidation_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.ApplyTheme && grdConsolidation.ActiveRow != null)
                {
                    if (grdConsolidation.ActiveRow.Appearance.BackColor != Color.LightSlateGray && grdConsolidation.ActiveRow.IsGroupByRow)
                    {
                        grdConsolidation.ActiveRow.Appearance.BackColor = Color.LightSlateGray;
                        grdConsolidation.ActiveRow.Appearance.BackColor2 = Color.DarkSlateGray;
                        grdConsolidation.ActiveRow.Appearance.BackGradientStyle = GradientStyle.VerticalBump;
                    }
                }
                else
                {
                    if (grdConsolidation.ActiveRow != null && grdConsolidation.ActiveRow.Appearance.BackColor != Color.FromArgb(203, 214, 224) && grdConsolidation.ActiveRow.IsGroupByRow)
                    {
                        grdConsolidation.ActiveRow.Appearance.BackColor = Color.FromArgb(203, 214, 224);
                        grdConsolidation.ActiveRow.Appearance.ForeColor = Color.Black;
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

        private UltraGridColumn _columnSorted;

        private void grdConsolidation_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                UltraGrid ultraGrid = sender as UltraGrid;
                if (ultraGrid != null)
                {
                    UIElement controlElement = ultraGrid.DisplayLayout.UIElement;
                    UIElement elementAtPoint = controlElement != null ? controlElement.ElementFromPoint(e.Location) : null;
                    while (elementAtPoint != null)
                    {
                        UltraGridUIElement uiElement = elementAtPoint.ControlElement as UltraGridUIElement;
                        if (uiElement != null)
                        {
                            HeaderUIElement headerElement = uiElement.ElementWithMouseCapture as HeaderUIElement;
                            if (headerElement != null &&
                                        headerElement.Header is ColumnHeader)
                            {
                                _columnSorted = headerElement.GetContext(typeof(UltraGridColumn)) as UltraGridColumn;
                                break;
                            }
                        }
                        elementAtPoint = elementAtPoint.Parent;
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

        public void setMDColumns()
        {
            GridMarketDataColumnUtil.hideNonPermitMarketDataColumns(PranaModules.PORTFOLIO_MANAGEMENT_MODULE, grdConsolidation);
        }

        private void CtrlMainConsolidationView_Load(object sender, EventArgs e)
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    grdConsolidation.DisplayLayout.Override.RowSelectors = UltraWinGridUtils.IsGrouppingAppliedOnGrid(grdConsolidation) ? DefaultableBoolean.False : DefaultableBoolean.True;
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
        private void AppearanceChoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppearanceClick(sender, e);
        }
        private void ClosePosition_Click(object sender, EventArgs e)
        {
            if (grdConsolidation.ActiveRow != null && !(grdConsolidation.ActiveRow is UltraGridGroupByRow))
            {
                closetaxlotToolStripMenuItem_Click(sender, e);
            }
            else
            {
                symbolToolStripMenuItem_Click(sender, e);
            }
        }
        void IncreasePositionStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdConsolidation.ActiveRow != null && !(grdConsolidation.ActiveRow is UltraGridGroupByRow))
            {
                IncreasePositionTaxlotToolStripMenuItem_Click(sender, e);
            }
            else
            {
                IncreasePositionSymbolToolStripMenuItem_Click(sender, e);
            }
        }
        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                if (isShowExportPM)
                {
                    exportToolStripMenuItem.DropDownItems[0].Visible = true;
                    exportToolStripMenuItem.DropDownItems[1].Visible = true;
                    exportToolStripMenuItem.Visible = true;
                    toolStripSeparator5.Visible = true;
                }
                else
                {
                    exportToolStripMenuItem.DropDownItems[0].Visible = false;
                    exportToolStripMenuItem.DropDownItems[1].Visible = false;
                    exportToolStripMenuItem.Visible = false;
                    toolStripSeparator5.Visible = false;
                }
                _pttDataDictionary.Clear();
                string symbol = String.Empty;
                bool isEnabled = true;
                if (grdConsolidation.ActiveRow != null && !(grdConsolidation.ActiveRow is UltraGridGroupByRow))
                {
                    GetSelectedRowDetailsForPTT(ref symbol);
                }
                if (grdConsolidation.ActiveRow != null || grdConsolidation.ActiveRow is UltraGridGroupByRow)
                {
                    pttToolStripMenuItem.DropDownItems[0].Visible = true;
                    pttToolStripMenuItem.DropDownItems[1].Visible = true;
                    pttToolStripMenuItem.DropDownItems[2].Visible = true;
                    pttToolStripMenuItem.Click -= new System.EventHandler(NoPositionOpenPTT);
                    isEnabled = GetGroupByRowValuesForPTT(ref symbol);
                }
                else
                {
                    pttToolStripMenuItem.DropDownItems[0].Visible = false;
                    pttToolStripMenuItem.DropDownItems[1].Visible = false;
                    pttToolStripMenuItem.DropDownItems[2].Visible = false;
                    pttToolStripMenuItem.Click += new System.EventHandler(NoPositionOpenPTT);
                }
                if (isEnabled)
                {
                    pttToolStripMenuItem.Enabled = ModuleManager.CheckModulePermissioning(PranaModules.PERCENT_TRADING_TOOL, PranaModules.PERCENT_TRADING_TOOL);//To_ask
                    if (!String.IsNullOrEmpty(symbol) && !symbol.Equals(PMConstants.SUMMARY_MULTIPLE) && !symbol.Equals(PMConstants.SUMMARY_DASH))
                        _pttDataDictionary.Add(PMConstants.COL_Symbol, symbol);
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
        /// Noes the position open PTT.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void NoPositionOpenPTT(object sender, EventArgs e)
        {
            if (pttToolStripMenuItem.DropDownItems[0].Visible == false)
                pttToolStripMenuItem_Click(sender, e, "");
        }

        public event EventHandler<AfterRowFilterChangedEventArgs> SendFilterColumnName;


        private void grdConsolidation_AfterRowFilterChanged(object sender, AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(PMConstants.COL_TradeDate) || e.Column.Key.Equals(PMConstants.COL_StartTradeDate) || e.Column.Key.Equals(PMConstants.COL_ExpirationDate)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();

                    if (e.Column.Key.Equals(PMConstants.COL_TradeDate))
                        grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.NirvanaDateTimeFormat_WithoutTime));
                    else if (e.Column.Key.Equals(PMConstants.COL_StartTradeDate))
                        grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateFormat));
                    else if (e.Column.Key.Equals(PMConstants.COL_ExpirationDate))
                        grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.NirvanaDateTimeFormat_WithoutTime));
                }

                if ((e.Column.Key.Equals(PMConstants.COL_TradeDate) || e.Column.Key.Equals(PMConstants.COL_StartTradeDate)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(BeforeToday)"))
                {
                    grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.LessThan, DateTime.Now.Date.ToString(DateTimeConstants.DateFormat));
                }

                if (e.Column.Key.Equals(OrderFields.PROPERTY_LEVEL1NAME) || e.Column.Key.Equals(OrderFields.PROPERTY_MASTERFUND))
                {
                    bool isFilterCorrect = true;
                    if (e.Column.Key.Equals(OrderFields.PROPERTY_MASTERFUND))
                    {
                        isFilteredColumnIsMasterFund = true;
                        filteredMasterFundList =
                                       (e.NewColumnFilter.FilterConditions.Cast<object>()
                                           .Select(filtercondtion => Regex.Match(filtercondtion.ToString(), @"'([^']*)"))
                                           .Where(match => match.Success)
                                           .Select(match => match.Groups[1].Value)).ToList();
                        //due to this check the dashboard is not updating when selecting blanks filter(for unallocated data)
                        //if (filteredMasterFundList.Contains(ApplicationConstants.FilterDetails_DBNull))
                        //{
                        //    return;
                        //}
                        if (e.NewColumnFilter.FilterConditions.Count <= 0)
                        {
                            addAccountNamesBasedOnMasterFund = false;
                        }
                        else
                        {
                            addAccountNamesBasedOnMasterFund = true;
                        }
                    }
                    if (e.Column.Key.Equals(OrderFields.PROPERTY_LEVEL1NAME))
                    {
                        isFilterCorrect = GetFilteredValueList(e);
                        isFilteredColumnIsMasterFund = false;
                        DisableEnableMasterFundFilter(e, isFilteredColumnIsMasterFund);
                    }
                    if (isFilterCorrect)
                    {
                        if (SendFilterColumnName != null)
                        {
                            SendFilterColumnName(this, e);
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

        // This Method Returns the list of Accounts When Filter is applied on the Account Column
        private bool GetFilteredValueList(AfterRowFilterChangedEventArgs e)
        {
            try
            {
                List<string> filteredAccountList =
                                  (e.NewColumnFilter.FilterConditions.Cast<object>()
                                      .Select(filtercondition => Regex.Match(filtercondition.ToString(), @"'([^']*)"))
                                      .Where(match => match.Success)
                                      .Select(match => match.Groups[1].Value)).ToList();

                if (filteredAccountList.Contains(ApplicationConstants.FilterDetails_DBNull))
                {
                    if (CachedDataManager.GetInstance.GetAllAccountsCount() > CachedDataManager.GetInstance.GetAllAccountIDsForUser().Count)
                    {
                        return false;
                    }
                }

                for (int i = filteredAccountList.Count - 1; i >= 0; i--)
                {
                    if (filteredAccountList[i] == ApplicationConstants.FilterDetails_DBNull || string.IsNullOrEmpty(filteredAccountList[i]))
                    {
                        filteredAccountList.RemoveAt(i);
                    }
                }
                List<string> permittedUserAccount = (from Prana.BusinessObjects.Account userAccount in CachedDataManager.GetInstance.GetUserAccounts() select userAccount.Name).ToList();

                if (filteredAccountList.Any(s => !permittedUserAccount.Contains(s)))
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

        //http://jira.nirvanasolutions.com:8080/browse/PRANA-13889
        //[PM] Disable MasterFund filter when filter is applied on Account First
        private void DisableEnableMasterFundFilter(AfterRowFilterChangedEventArgs e, bool FilteredColumnIsMasterFund)
        {
            try
            {
                var col = grdConsolidation.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_MASTERFUND];
                if (e == null && col != null)
                {
                    col.AllowRowFiltering = DefaultableBoolean.True;
                    return;
                }

                if (col != null)
                {
                    if (e.NewColumnFilter.FilterConditions.Count > 0 && !FilteredColumnIsMasterFund)
                    {
                        col.AllowRowFiltering = DefaultableBoolean.False;
                    }
                    else
                    {
                        col.AllowRowFiltering = DefaultableBoolean.True;
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

        // http://jira.nirvanasolutions.com:8080/browse/PRANA-13927
        // [PM Dynamic Filtering] no filter if there are no open positions for one FUND
        private void grdConsolidation_AfterRowFilterDropDownPopulate(object sender, AfterRowFilterDropDownPopulateEventArgs e)
        {
            try
            {

                if (e.Column.Key.Equals(OrderFields.PROPERTY_LEVEL1NAME) && addAccountNamesBasedOnMasterFund)
                {
                    List<int> masterFundids = filteredMasterFundList.Select(masterFundname => CachedDataManager.GetInstance.GetMasterFundID(masterFundname)).ToList();
                    Dictionary<int, List<int>> masterFundAssociationDictionary = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();

                    List<int> filteredAccountList = new List<int>();
                    foreach (KeyValuePair<int, List<int>> masterFundAssociationKvp in masterFundAssociationDictionary.Where(masterFundAssociationKvp => masterFundids.Contains(masterFundAssociationKvp.Key)))
                    {
                        filteredAccountList.AddRange(masterFundAssociationKvp.Value);
                    }
                    List<string> accountNamesInFilter = filteredAccountList.Select(accountName => CachedDataManager.GetInstance.GetAccount(accountName)).ToList();

                    List<ValueListItem> valueList = e.ValueList.ValueListItems.Cast<ValueListItem>().ToList();
                    AccountCollection permittedUserAccount = CachedDataManager.GetInstance.GetUserAccounts();
                    foreach (Prana.BusinessObjects.Account userAccount in permittedUserAccount)
                    {
                        if (accountNamesInFilter.Contains(userAccount.Name) && !string.IsNullOrEmpty(userAccount.Name) && userAccount.Name != "-Select-")
                        {
                            if (!valueList.Any(v => v.DataValue.ToString().Equals(userAccount.Name)))
                                e.ValueList.ValueListItems.Add(userAccount.Name);
                        }
                    }
                }
                else if (e.Column.Key.Equals(OrderFields.PROPERTY_LEVEL1NAME) && addAccountNamesBasedOnMasterFund == false)
                {
                    List<string> accountNamesInFilter = new List<string>();
                    accountNamesInFilter.AddRange(from ValueListItem filtercondtion in e.ValueList.ValueListItems
                                                  where filtercondtion.DataValue.ToString() != ApplicationConstants.FilterDetails_All
                                                  && filtercondtion.DataValue.ToString() != ApplicationConstants.FilterDetails_Custom
                                                  && filtercondtion.DataValue.ToString() != ApplicationConstants.FilterDetails_Blanks
                                                  && filtercondtion.DataValue.ToString() != ApplicationConstants.FilterDetails_NonBlanks
                                                  select filtercondtion.DisplayText);

                    if (CachedDataManager.GetInstance.GetAllAccountIDsForUser().Count > accountNamesInFilter.Count)
                    {
                        AccountCollection permittedUserAccount = CachedDataManager.GetInstance.GetUserAccounts();

                        foreach (Prana.BusinessObjects.Account userAccount in permittedUserAccount)
                        {
                            if (!accountNamesInFilter.Contains(userAccount.Name) && !string.IsNullOrEmpty(userAccount.Name) && userAccount.Name != ApplicationConstants.C_COMBO_SELECT)
                            {
                                e.ValueList.ValueListItems.Add(userAccount.Name);
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

        internal void SetUIPrefs(PMUIPrefs savedUIPrefs)
        {
            try
            {
                _uiPrefs = savedUIPrefs;
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

        private bool GetGroupByRowValuesForPTT(ref string symbol)
        {
            try
            {
                if (grdConsolidation.ActiveRow != null)
                {
                    var ultraGridGroupByRow = grdConsolidation.ActiveRow as UltraGridGroupByRow;
                    if (ultraGridGroupByRow != null)
                    {
                        if (!ultraGridGroupByRow.Rows.SummaryValues.Cast<SummaryValue>().Any(x => x.Key.Equals(PMConstants.COL_Symbol)))
                        {
                            symbol = string.Empty;
                        }
                        else
                        {
                            symbol = ultraGridGroupByRow.Rows.SummaryValues[PMConstants.COL_Symbol].Value.ToString();
                        }

                        return true;
                    }
                    return true; //In case we have active row but is ungrouped
                }
                else
                {
                    if (grdConsolidation.ActiveRow != null)
                    {
                        var consolidatedInfo = (ExposurePnlCacheItem)grdConsolidation.ActiveRow.ListObject;
                        if (null != consolidatedInfo)
                        {
                            symbol = consolidatedInfo.Symbol;
                            return true;
                        }
                        return false;
                    }
                    return false;
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

        private void GetSelectedRowDetailsForPTT(ref string symbol)
        {
            try
            {
                ExposurePnlCacheItem consolidatedInfo = null;
                if (grdConsolidation.ActiveRow != null)
                {
                    consolidatedInfo = (ExposurePnlCacheItem)grdConsolidation.ActiveRow.ListObject;
                }
                if (null != consolidatedInfo)
                {
                    symbol = consolidatedInfo.Symbol;
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

        private bool GetSelectedRowAsset(ref string asset, ref string symbol)
        {
            try
            {
                ExposurePnlCacheItem consolidatedInfo = null;
                if (grdConsolidation.ActiveRow != null)
                {
                    consolidatedInfo = (ExposurePnlCacheItem)grdConsolidation.ActiveRow.ListObject;
                }
                if (null != consolidatedInfo)
                {
                    asset = consolidatedInfo.Asset;
                    symbol = consolidatedInfo.Symbol;

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
            return true;
        }

        private bool GetSelectedGroupByRowAsset(ref string asset, ref string symbol, ref string quantity, ref string price)
        {
            try
            {
                if (grdConsolidation.ActiveRow != null)
                {
                    var ultraGridGroupByRow = grdConsolidation.ActiveRow as UltraGridGroupByRow;
                    if (ultraGridGroupByRow != null)
                    {
                        if (!ultraGridGroupByRow.Rows.SummaryValues.Cast<SummaryValue>().Any(x => x.Key.Equals(PMConstants.COL_Asset)) || !ultraGridGroupByRow.Rows.SummaryValues.Cast<SummaryValue>().Any(x => x.Key.Equals(PMConstants.COL_Symbol))
                            || !ultraGridGroupByRow.Rows.SummaryValues.Cast<SummaryValue>().Any(x => x.Key.Equals(PMConstants.COL_Quantity))
                    || !ultraGridGroupByRow.Rows.SummaryValues.Cast<SummaryValue>().Any(x => x.Key.Equals(PMConstants.COL_SelectedFeedPrice)))
                        {
                            asset = string.Empty;
                            symbol = string.Empty;
                            quantity = string.Empty;
                            price = string.Empty;
                            return false;
                        }
                        asset = ultraGridGroupByRow.Rows.SummaryValues[PMConstants.COL_Asset].Value.ToString();
                        symbol = ultraGridGroupByRow.Rows.SummaryValues[PMConstants.COL_Symbol].Value.ToString();
                        quantity = ultraGridGroupByRow.Rows.SummaryValues[PMConstants.COL_Quantity].Value.ToString();
                        price = ultraGridGroupByRow.Rows.SummaryValues[PMConstants.COL_SelectedFeedPrice].Value.ToString();
                        return true;
                    }
                }
                else
                {
                    quantity = "0.0";
                    price = "0.0";
                    return GetSelectedRowAsset(ref asset, ref symbol);
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

        // returns the account list based on filter applied on Account Name Column
        private List<string> GetFilteredAccountLst(List<string> lstAccount)
        {
            try
            {
                lstAccount = (grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_LEVEL1NAME].FilterConditions.Cast<object>()
                        .Select(filtercondtion => Regex.Match(filtercondtion.ToString(), @"'([^']*)")).Where(match => match.Success)
                        .Select(match => match.Groups[1].Value)).ToList();
                if (lstAccount.Contains(ApplicationConstants.FilterDetails_DBNull))
                {
                    lstAccount.RemoveAll(item => item == ApplicationConstants.FilterDetails_DBNull || String.IsNullOrEmpty(item));
                }

                return lstAccount;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new List<string>();
        }

        private List<string> GetFilteredMasterFundsLst()
        {
            try
            {
                List<string> masterFundFilterList = new List<string>();
                masterFundFilterList = (grdConsolidation.DisplayLayout.Bands[0].ColumnFilters[OrderFields.PROPERTY_MASTERFUND].FilterConditions.Cast<object>()
                        .Select(filtercondtion => Regex.Match(filtercondtion.ToString(), @"'([^']*)")).Where(match => match.Success)
                        .Select(match => match.Groups[1].Value)).ToList();
                if (masterFundFilterList.Contains(ApplicationConstants.FilterDetails_DBNull))
                {
                    masterFundFilterList.RemoveAll(item => item == ApplicationConstants.FilterDetails_DBNull || String.IsNullOrEmpty(item));
                }
                return masterFundFilterList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new List<string>();
        }

        private HashSet<string> _expandedRows = new HashSet<string>();
        private int _scrollPos;
        private HashSet<string> _selectedRows = new HashSet<string>();
        internal void RefreshGridAfterDataUpdate()
        {
            try
            {
                if (grdConsolidation.InvokeRequired)
                {
                    MethodInvoker mi = new MethodInvoker(RefreshGridAfterDataUpdate);
                    BeginInvoke(mi);
                }
                else
                {
                    CaptureCurrentUIState();
                    grdConsolidation.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                    RestoreCurrentUIState();
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

        private void CaptureCurrentUIState()
        {
            try
            {
                _scrollPos = grdConsolidation.ActiveRowScrollRegion.ScrollPosition;
                RecursiveCaptureCurrentUIState(grdConsolidation.Rows, "$@$");
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

        private void RecursiveCaptureCurrentUIState(RowsCollection rowsCollection, string parentGrouping)
        {
            try
            {
                foreach (UltraGridRow row in rowsCollection)
                {
                    var groupByRow = row as UltraGridGroupByRow;
                    if (groupByRow != null && (groupByRow.Expanded || groupByRow.IsActiveRow))
                    {

                        string currentGrouping = parentGrouping + "@#@" + groupByRow.Value.ToString();

                        if (groupByRow.IsActiveRow)
                        {
                            _selectedRows.Add(currentGrouping);
                        }
                        if (groupByRow.Expanded)
                        {
                            _expandedRows.Add(currentGrouping);
                            if (row.HasChild())
                            {
                                RecursiveCaptureCurrentUIState(row.ChildBands[0].Rows, currentGrouping);
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

        private void RestoreCurrentUIState()
        {
            try
            {
                grdConsolidation.ActiveRowScrollRegion.ScrollPosition = _scrollPos;

                if ((_expandedRows == null || _expandedRows.Count == 0) && (_selectedRows == null || _selectedRows.Count == 0))
                {
                    return;
                }

                RecursiveCurrentUIState(grdConsolidation.Rows, "$@$");
                grdConsolidation.ActiveRowScrollRegion.ScrollPosition = _scrollPos;

                _expandedRows.Clear();
                _selectedRows.Clear();
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

        private void RecursiveCurrentUIState(RowsCollection rowsCollection, string parentGrouping)
        {
            try
            {
                foreach (UltraGridRow row in rowsCollection)
                {
                    var groupByRow = row as UltraGridGroupByRow;
                    if (groupByRow != null)
                    {
                        string currentGrouping = parentGrouping + "@#@" + groupByRow.Value.ToString();

                        if (_selectedRows != null && _selectedRows.Count > 0 && _selectedRows.Contains(currentGrouping))
                        {
                            _selectedRows.Remove(currentGrouping);
                            groupByRow.Activate();
                        }
                        if (_expandedRows != null && _expandedRows.Count > 0 && _expandedRows.Contains(currentGrouping))
                        {
                            _expandedRows.Remove(currentGrouping);
                            groupByRow.Expanded = true;
                            if (row.HasChild())
                            {
                                RecursiveCurrentUIState(row.ChildBands[0].Rows, currentGrouping);
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

        internal List<string> GetColumnGroupingForSelectedTab()
        {
            try
            {
                _groupByColumnList.Clear();
                foreach (UltraGridColumn var in grdConsolidation.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (var.IsGroupByColumn)
                    {
                        _groupByColumnList.Add(var.Key);
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
            return _groupByColumnList;
        }

        private void pttToolStripMenuItem_Click(object sender, EventArgs e, string type)
        {
            try
            {
                var lstAccount = new List<string>();
                List<string> mfList = null;
                lstAccount = GetFilteredAccountLst(lstAccount);
                PTTMasterFundOrAccount mfOrAcc;
                mfList = GetFilteredMasterFundsLst();
                if (mfList.Count > 0 && lstAccount.Count == 0) //in case Master Fund filter is applied on PM no filter on Account
                {
                    mfOrAcc = PTTMasterFundOrAccount.MasterFund;
                    lstAccount = mfList;
                }
                else //Covers case for account filter as well as case when account filter is applied after filter on mf is applied
                {
                    mfOrAcc = PTTMasterFundOrAccount.Account;
                }
                string symbol = String.Empty;
                if (_pttDataDictionary.Count > 0)
                {
                    if (_pttDataDictionary.ContainsKey(PMConstants.COL_Symbol))
                        symbol = _pttDataDictionary[PMConstants.COL_Symbol].ToString();
                }
                else
                {
                    MessageBox.Show(this, "The column Symbol should be displayed and must contain valid value in order to proceed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (SendPercentTradingDataToPM != null)
                    SendPercentTradingDataToPM(this, new EventArgs<string, PTTMasterFundOrAccount, List<string>, string>(symbol, mfOrAcc, lstAccount, type));
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

        internal bool GetDashboardStatus()
        {
            if (_customViewPreference != null)
                return _customViewPreference.IsDashboardVisible;

            return false;
        }

        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            try
            {
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!System.IO.Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                exporter.Export(grdConsolidation, filePath);
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

    public class GroupSortComparer : IComparer
    {
        private string _sortColumnKey;

        public string SortColumnKey
        {
            get { return _sortColumnKey; }
            set { _sortColumnKey = value; }
        }

        public int Compare(object x, object y)
        {
            if (x.Equals(y) || String.IsNullOrEmpty(_sortColumnKey))
            {
                return 0;
            }
            if (String.Compare(_sortColumnKey, "TradeDate", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return 0;
            }

            UltraGridGroupByRow xGRow = (UltraGridGroupByRow)x;
            UltraGridGroupByRow yGRow = (UltraGridGroupByRow)y;

            //TODO: tradedate on grid has no associated summary value, so the following code throws error. 
            // avoid trycf, avoid check for _sortColumnKey in each call to compare,  
            var xValue = (IComparable)xGRow.Rows.SummaryValues[_sortColumnKey].Value;
            var yValue = (IComparable)yGRow.Rows.SummaryValues[_sortColumnKey].Value;

            if (xValue == null || yValue == null)
            {
                return -1;
            }

            if (xValue.GetType() != yValue.GetType())
            {
                return String.Compare(xValue.ToString(), yValue.ToString(), StringComparison.Ordinal);
            }
            return xValue.CompareTo(yValue);
        }
    }

    #region Custom Sorter Classes

    public class CustomGroupByRowsSorterForText : IComparer
    {
        private string _columnName;

        public string Column
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private SortIndicator _sortIndicator = SortIndicator.Descending;

        public SortIndicator SortIndicator
        {
            get { return _sortIndicator; }
            set
            {
                _sortIndicator = value;
                switch (_sortIndicator)
                {
                    case SortIndicator.Ascending:
                        _multiplier = 1;
                        break;

                    case SortIndicator.Descending:
                    case SortIndicator.Disabled:
                    case SortIndicator.None:
                        _multiplier = -1;
                        break;
                }
            }
        }

        private int _multiplier = 1;

        public int Compare(object xObj, object yObj)
        {
            try
            {
                UltraGridGroupByRow x = (UltraGridGroupByRow)xObj;
                UltraGridGroupByRow y = (UltraGridGroupByRow)yObj;

                if (Equals(xObj, yObj))
                {
                    return 0;
                }
                if (_columnName == "TradeDate")
                {
                    return 0;
                }

                if (_columnName != string.Empty && x.Band.Summaries.Exists(_columnName) && y.Band.Summaries.Exists(_columnName))
                {
                    if (x.Rows.SummaryValues[_columnName].Value == null)
                    {
                        return _multiplier;
                    }
                    if (y.Rows.SummaryValues[_columnName].Value == null)
                    {
                        return (-(_multiplier));
                    }
                    return (String.Compare(x.Rows.SummaryValues[_columnName].Value.ToString(), y.Rows.SummaryValues[_columnName].Value.ToString(), StringComparison.Ordinal)) * _multiplier;
                }
                return 0;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }
    }

    public class CustomGroupByRowsSorterForAlphaNumeric : IComparer
    {
        private string _columnName;

        public string Column
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private SortIndicator _sortIndicator = SortIndicator.Descending;

        public SortIndicator SortIndicator
        {
            get { return _sortIndicator; }
            set
            {
                _sortIndicator = value;
                switch (_sortIndicator)
                {
                    case SortIndicator.Ascending:
                        _multiplier = 1;
                        break;

                    case SortIndicator.Descending:
                    case SortIndicator.Disabled:
                    case SortIndicator.None:
                        _multiplier = -1;
                        break;
                }
            }
        }

        private int _multiplier = 1;

        public int Compare(object xObj, object yObj)
        {
            try
            {
                UltraGridGroupByRow x = (UltraGridGroupByRow)xObj;
                UltraGridGroupByRow y = (UltraGridGroupByRow)yObj;

                if (Equals(yObj, xObj))
                {
                    return 0;
                }
                if (_columnName != string.Empty && x.Band.Summaries.Exists(_columnName) && y.Band.Summaries.Exists(_columnName))
                {
                    if (x.Rows.SummaryValues[_columnName].Value == null)
                    {
                        return -1;
                    }
                    if (y.Rows.SummaryValues[_columnName].Value == null)
                    {
                        return -1;
                    }
                    double xSummaryValue;
                    double ySummaryValue;
                    if (!double.TryParse(x.Rows.SummaryValues[_columnName].Value.ToString(), out xSummaryValue))
                    {
                        if (x.Rows.SummaryValues[_columnName].Value.ToString() == PMConstants.SUMMARY_MULTIPLE)
                            xSummaryValue = double.MinValue + 1;
                        else if (x.Rows.SummaryValues[_columnName].Value.ToString() == "N/A")
                            xSummaryValue = double.MinValue + 2;
                        else if (x.Rows.SummaryValues[_columnName].Value.ToString() == "Non-Expiring positions")
                            xSummaryValue = double.MinValue + 3;
                        else if (x.Rows.SummaryValues[_columnName].Value.ToString() == PMConstants.SUMMARY_UNDEFINED)
                            xSummaryValue = double.MinValue + 4;
                        else if (x.Rows.SummaryValues[_columnName].Value.ToString() == PMConstants.SUMMARY_DASH)
                            xSummaryValue = double.MinValue + 5;
                        else
                            xSummaryValue = double.MinValue;
                    }
                    if (!double.TryParse(y.Rows.SummaryValues[_columnName].Value.ToString(), out ySummaryValue))
                    {
                        if (y.Rows.SummaryValues[_columnName].Value.ToString() == PMConstants.SUMMARY_MULTIPLE)
                            ySummaryValue = double.MinValue + 1;
                        else if (y.Rows.SummaryValues[_columnName].Value.ToString() == "N/A")
                            ySummaryValue = double.MinValue + 2;
                        else if (y.Rows.SummaryValues[_columnName].Value.ToString() == "Non-Expiring positions")
                            ySummaryValue = double.MinValue + 3;
                        else if (y.Rows.SummaryValues[_columnName].Value.ToString() == PMConstants.SUMMARY_UNDEFINED)
                            ySummaryValue = double.MinValue + 4;
                        else if (y.Rows.SummaryValues[_columnName].Value.ToString() == PMConstants.SUMMARY_DASH)
                            ySummaryValue = double.MinValue + 5;
                        else
                            ySummaryValue = double.MinValue;
                    }
                    return (xSummaryValue.CompareTo(ySummaryValue)) * _multiplier;
                }
                else
                {
                    return 0;
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
            return 0;
        }
    }

    public class CustomGroupByRowsSorterForDate : IComparer
    {
        private string _columnName;

        public string Column
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private SortIndicator _sortIndicator = SortIndicator.Descending;

        public SortIndicator SortIndicator
        {
            get { return _sortIndicator; }
            set
            {
                _sortIndicator = value;
                switch (_sortIndicator)
                {
                    case SortIndicator.Ascending:
                        _multiplier = 1;
                        break;

                    case SortIndicator.Descending:
                    case SortIndicator.Disabled:
                    case SortIndicator.None:
                        _multiplier = -1;
                        break;
                }
            }
        }

        private int _multiplier = 1;

        public int Compare(object xObj, object yObj)
        {
            try
            {
                UltraGridGroupByRow x = (UltraGridGroupByRow)xObj;
                UltraGridGroupByRow y = (UltraGridGroupByRow)yObj;

                if (Equals(y, x))
                {
                    return 0;
                }
                if (_columnName != string.Empty)
                {
                    if (x.Rows.SummaryValues[_columnName].Value == null && x.Band.Summaries.Exists(_columnName) && y.Band.Summaries.Exists(_columnName))
                    {
                        return -1;
                    }
                    if (y.Rows.SummaryValues[_columnName].Value == null)
                    {
                        return 1;
                    }

                    DateTime xSummaryValue = DateTime.MinValue;
                    DateTime ySummaryValue;
                    if (x.Rows.SummaryValues[_columnName].Value != null && !DateTime.TryParse(x.Rows.SummaryValues[_columnName].Value.ToString(), out xSummaryValue))
                    {
                        if (x.Rows.SummaryValues[_columnName].Value.ToString() == PMConstants.SUMMARY_MULTIPLE)
                            xSummaryValue = DateTime.MinValue;
                        else if (x.Rows.SummaryValues[_columnName].Value.ToString() == "N/A")
                            xSummaryValue = DateTime.MinValue.AddDays(1);
                        else if (x.Rows.SummaryValues[_columnName].Value.ToString() == "Non-Expiring positions")
                            xSummaryValue = DateTime.MinValue.AddDays(2);
                        else if (x.Rows.SummaryValues[_columnName].Value.ToString() == PMConstants.SUMMARY_UNDEFINED)
                            xSummaryValue = DateTime.MinValue.AddDays(3);
                        else if (x.Rows.SummaryValues[_columnName].Value.ToString() == PMConstants.SUMMARY_DASH)
                            xSummaryValue = DateTime.MinValue.AddDays(4);
                        else
                            xSummaryValue = DateTime.MinValue.AddDays(5);
                    }
                    if (!DateTime.TryParse(y.Rows.SummaryValues[_columnName].Value.ToString(), out ySummaryValue))
                    {
                        if (y.Rows.SummaryValues[_columnName].Value.ToString() == PMConstants.SUMMARY_MULTIPLE)
                            ySummaryValue = DateTime.MinValue;
                        else if (y.Rows.SummaryValues[_columnName].Value.ToString() == "N/A")
                            ySummaryValue = DateTime.MinValue.AddDays(1);
                        else if (y.Rows.SummaryValues[_columnName].Value.ToString() == "Non-Expiring positions")
                            ySummaryValue = DateTime.MinValue.AddDays(2);
                        else if (y.Rows.SummaryValues[_columnName].Value.ToString() == PMConstants.SUMMARY_UNDEFINED)
                            ySummaryValue = DateTime.MinValue.AddDays(3);
                        else if (y.Rows.SummaryValues[_columnName].Value.ToString() == PMConstants.SUMMARY_DASH)
                            ySummaryValue = DateTime.MinValue.AddDays(4);
                        else
                            ySummaryValue = DateTime.MinValue.AddDays(5);
                    }

                    return (xSummaryValue.CompareTo(ySummaryValue)) * _multiplier;
                }
                else
                {
                    return 0;
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
            return 0;
        }
    }

    #endregion Custom Sorter Classes

    public delegate void SubscriptionDataHandler(ExPNLData ExPNLDataType);

    public class CustomRowSorterForDate : IComparer
    {
        private string _columnName;

        public string Column
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private SortIndicator _sortIndicator = SortIndicator.Descending;

        public SortIndicator SortIndicator
        {
            get { return _sortIndicator; }
            set
            {
                _sortIndicator = value;
                switch (_sortIndicator)
                {
                    case SortIndicator.Ascending:
                        _multiplier = 1;
                        break;

                    case SortIndicator.Descending:
                    case SortIndicator.Disabled:
                    case SortIndicator.None:
                        _multiplier = -1;
                        break;
                }
            }
        }

        private int _multiplier = 1;

        public int Compare(object xObj, object yObj)
        {
            UltraGridCell x = (UltraGridCell)xObj;
            UltraGridCell y = (UltraGridCell)yObj;

            if (Equals(y, x))
            {
                return 0;
            }
            if (_columnName != string.Empty)
            {
                if (x.Value == null)
                {
                    return -1;
                }
                if (y.Value == null)
                {
                    return 1;
                }

                DateTime xSummaryValue;
                DateTime ySummaryValue;
                if (!DateTime.TryParse(x.Value.ToString(), out xSummaryValue))
                {
                    return _multiplier;
                }
                if (!DateTime.TryParse(y.Value.ToString(), out ySummaryValue))
                {
                    return (-(_multiplier));
                }

                return (xSummaryValue.CompareTo(ySummaryValue)) * _multiplier;
            }
            else
            {
                return 0;
            }
        }
    }

    public class CustomRowSorterForText : IComparer
    {
        private string _columnName;

        public string Column
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private SortIndicator _sortIndicator = SortIndicator.Descending;

        public SortIndicator SortIndicator
        {
            get { return _sortIndicator; }
            set
            {
                _sortIndicator = value;
                switch (_sortIndicator)
                {
                    case SortIndicator.Ascending:
                        _multiplier = 1;
                        break;

                    case SortIndicator.Descending:
                    case SortIndicator.Disabled:
                    case SortIndicator.None:
                        _multiplier = -1;
                        break;
                }
            }
        }

        private int _multiplier = 1;

        public int Compare(object xObj, object yObj)
        {
            UltraGridCell x = (UltraGridCell)xObj;
            UltraGridCell y = (UltraGridCell)yObj;

            if (Equals(y, x))
            {
                return 0;
            }
            if (_columnName != string.Empty)
            {
                if (x.Value == null)
                {
                    return -1;
                }
                if (y.Value == null)
                {
                    return 1;
                }

                double xSummaryValue;
                double ySummaryValue;
                if (!double.TryParse(x.Value.ToString(), out xSummaryValue))
                {
                    if (x.Value.ToString() == PMConstants.SUMMARY_MULTIPLE)
                        xSummaryValue = double.MinValue + 1;
                    else if (x.Value.ToString() == "N/A")
                        xSummaryValue = double.MinValue + 2;
                    else if (x.Value.ToString() == "Non-Expiring positions")
                        xSummaryValue = double.MinValue + 3;
                    else if (x.Value.ToString() == PMConstants.SUMMARY_UNDEFINED)
                        xSummaryValue = double.MinValue + 4;
                    else if (x.Value.ToString() == PMConstants.SUMMARY_DASH)
                        xSummaryValue = double.MinValue + 5;
                    else
                        xSummaryValue = double.MinValue;
                }
                if (!double.TryParse(y.Value.ToString(), out ySummaryValue))
                {
                    if (y.Value.ToString() == PMConstants.SUMMARY_MULTIPLE)
                        ySummaryValue = double.MinValue + 1;
                    else if (y.Value.ToString() == "N/A")
                        ySummaryValue = double.MinValue + 2;
                    else if (y.Value.ToString() == "Non-Expiring positions")
                        ySummaryValue = double.MinValue + 3;
                    else if (y.Value.ToString() == PMConstants.SUMMARY_UNDEFINED)
                        ySummaryValue = double.MinValue + 4;
                    else if (y.Value.ToString() == PMConstants.SUMMARY_DASH)
                        ySummaryValue = double.MinValue + 5;
                    else
                        ySummaryValue = double.MinValue;
                }

                return (xSummaryValue.CompareTo(ySummaryValue)) * _multiplier;
            }
            else
            {
                return 0;
            }
        }
    }

    public class SortComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            UltraGridCell first = (UltraGridCell)x;
            UltraGridCell second = (UltraGridCell)y;

            int firstCellValue = GetCellValue(first);
            int secondCellValue = GetCellValue(second);
            return firstCellValue.CompareTo(secondCellValue);
        }

        private int GetCellValue(UltraGridCell UltraGridCellValue)
        {
            int index;

            PositionType positionType;
            Enum.TryParse(UltraGridCellValue.Text, out positionType);
            switch (positionType)
            {
                case PositionType.Long:
                    index = 1;
                    break;

                case PositionType.Short:
                    index = 2;
                    break;

                case PositionType.FX:
                    index = 3;
                    break;

                case PositionType.Boxed:
                    index = 4;
                    break;

                default:
                    index = int.MaxValue;
                    break;
            }
            return index;
        }
    }

    /// <summary>
    /// Compare column after group by for shorting
    /// Not in use currently 
    /// </summary>
    public class GroupByComparer : IComparer
    {
        private UltraGrid _grid;
        private UltraGridColumn _column;

        public GroupByComparer(UltraGrid grid, UltraGridColumn column)
        {
            _grid = grid;
            _column = column;
        }

        public int Compare(object x, object y)
        {
            try
            {
                UltraGridGroupByRow rowX = x as UltraGridGroupByRow;
                UltraGridGroupByRow rowY = y as UltraGridGroupByRow;

                if (_grid.DisplayLayout.Bands[0].SortedColumns.Contains(_column))
                {
                    var indicator = _column.SortIndicator;

                    double xSummary = double.Parse(rowX.Rows.SummaryValues[_column.Key].Value.ToString());
                    double ySummary = double.Parse(rowY.Rows.SummaryValues[_column.Key].Value.ToString());

                    if (indicator == SortIndicator.Ascending)
                    {
                        return xSummary.CompareTo(ySummary);
                    }
                    else
                    {
                        return ySummary.CompareTo(xSummary);
                    }
                }
                else
                {
                    return rowX.Description.CompareTo(rowY.Description);
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
            return 0;
        }
    }
}
