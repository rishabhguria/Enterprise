using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ExposurePnlCache;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.PM.Client.UI.Classes;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Controls
{
    public partial class PMUserControl : UserControl
    {
        #region public-variables
        public GridColumnFilterDetails FilterDetails
        {
            get { return _filterDetails; }
            set { _filterDetails = value; }
        }

        #endregion

        #region private-variables
        private ExposurePnlCacheManager _exInstance;
        public ExPnlBindableView _accountBindableView;
        private GridColumnFilterDetails _filterDetails;
        private GridColumnFilterDetails _prevFilterDetails;
        private GridColumnFilterDetails _mfFilterDetails;
        private CompanyUser _loginUser;
        private const string SUB_MODULE_NAME = "CustomView";
        private const string TAB_Main = "Main";
        #endregion

        #region constructor & initialization
        public PMUserControl()
        {
            InitializeComponent();
        }

        internal void PreInitialize(PMUIPrefs _pmUIPrefs)
        {
            try
            {
                pmGrid.SetUIPrefs(_pmUIPrefs);
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

        public void InitializePMUserControl(CompanyUser loginUserfromPM, ExposurePnlCacheManager exInstance)
        {
            try
            {
                _loginUser = loginUserfromPM;
                _accountBindableView = new ExPnlBindableView { GridData = exInstance.PMAccountView };
                _exInstance = exInstance;
                pmDashboard.Setup();
                pmDashboard.SetGridFontSize(PMAppearanceManager.PMAppearance.FontSizeDashboard);

                if (CustomThemeHelper.ApplyTheme)
                {
                    pmGrid.BorderStyle = BorderStyle.None;
                }
                pmGrid.ShowDashboardEvent += PMGrid_ShowHideDashboardEvent;
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

        #region Event-Handlers
        public event EventHandler AddNewConsolidationView;

        public event EventHandler<EventArgs<GridColumnFilterDetails, GridColumnFilterDetails>> FilteredColumnNameToPM;

        public event EventHandler LanuchPricingInput;

        public event EventHandler<AfterColPosChangedEventArgs> PmGridColPositionChanged;

        public event EventHandler<EventArgs<string, string, bool>> SaveAllGridLayouts;

        public event EventHandler<EventArgs<string, string, int, GridColumnFilterDetails>> TaxlotsRequested;

        public event EventHandler<EventArgs<string, string>> DeleteCustomViewTabEvent;

        public event EventHandler<EventArgs<string, string, ExPNLData>> RenameCustomviewEvent;

        public event EventHandler<EventArgs<OrderSingle, Dictionary<int, double>>> PassTradeClickEvent;

        public event EventHandler Appearance_Click;

        public event EventHandler<EventArgs<string, PTTMasterFundOrAccount, List<string>, string>> PercentTradingDataToPM;
        #endregion

        #region public-methods
        public void RequestAccountData()
        {
            try
            {
                pmGrid.RequestAccountData();
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

        public void UnWireEvent(PMUserControl pmUserControl)
        {
            try
            {
                CtrlMainConsolidationView consolidationView = pmUserControl.pmGrid;
                consolidationView.TradeClick -= PMGrid_TradeClick;
                consolidationView.PricingInputClick -= PMGrid_PricingInputClick;
                consolidationView.AddNewConsolidationView -= AddNewConsolidationView;
                consolidationView.SaveAllGridLayouts -= PMGrid_SaveAllGridLayouts;
                consolidationView.TaxlotsRequested -= PMGrid_TaxlotsRequested;
                consolidationView.AfterColPositionChanged -= PMGrid_AfterColPositionChanged;
                consolidationView.DeleteViewClick -= OnDeleteViewClick;
                consolidationView.RenameViewClick -= OnRenameViewClick;
                consolidationView.SendFilterColumnName -= PMGrid_SendFilterColumnName;
                consolidationView.SendGroupedColumnNames -= PMGrid_SendGroupedColumnNames;
                consolidationView.SendPercentTradingDataToPM -= pmGrid_SendPercentTradingDataToPM;
                consolidationView.AppearanceClick -= PMGrid_AppearanceClick;
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

        #region internal-methods
        internal void AssignData(DataTable summary)
        {
            pmDashboard.AssignData(summary);
        }

        internal void BindDataSource(DataTable ConsolidationViewSummary, string prefFilePath)
        {
            try
            {
                pmDashboard.BindDataSource(ConsolidationViewSummary, typeof(DataTable), prefFilePath, pmGrid.GetDashboardStatus());
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

        internal void ClearData()
        {
            try
            {
                if (pmGrid != null)
                    UnWireEvent(this);

                if (_accountBindableView != null)
                    _accountBindableView.Dispose();

                _accountBindableView = null;

                if (pmDashboard != null)
                {
                    pmDashboard.Dispose();
                    pmDashboard = null;
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

        internal void RefreshGridAfterDataUpdate()
        {
            try
            {
                pmGrid.RefreshGridAfterDataUpdate();
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

        internal void GetGroupingForSelectedTab()
        {
            try
            {
                _exInstance.PMCurrentViewGroupedColumns = pmGrid.GetColumnGroupingForSelectedTab();
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

        internal CustomViewPreferences GetLayout(bool clearGroupByCollection, string key)
        {
            return pmGrid.GetLayout(clearGroupByCollection, key);
        }

        /// <summary>
        /// Highlights the symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        internal void HighlightSymbol(string symbol)
        {
            try
            {
                pmGrid.HighlightSymbol(symbol);
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

        internal void ShowHideDashboard(bool isDashboardVisible)
        {
            try
            {
                pmGrid.ShowHideDashboard(isDashboardVisible);
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

        internal void InitNewView(CustomViewPreferences preference, string tabName)
        {

            try
            {
                pmGrid.ParentKey = tabName;
                pmGrid.InitControl(true, true, preference, _accountBindableView);
                FilterDetails = preference.FilterDetails;
                pmGrid.AddViewEnabled = true;
                pmGrid.TradeClick += PMGrid_TradeClick;
                pmGrid.AppearanceClick += PMGrid_AppearanceClick;
                pmGrid.PricingInputClick += PMGrid_PricingInputClick;
                pmGrid.AddNewConsolidationView += PMGrid_AddNewConsolidationView;
                pmGrid.SaveAllGridLayouts += PMGrid_SaveAllGridLayouts;
                pmGrid.TaxlotsRequested += PMGrid_TaxlotsRequested;
                pmGrid.AfterColPositionChanged += PMGrid_AfterColPositionChanged;
                pmGrid.SendFilterColumnName += PMGrid_SendFilterColumnName;
                pmGrid.SendGroupedColumnNames += PMGrid_SendGroupedColumnNames;
                pmGrid.SendPercentTradingDataToPM += pmGrid_SendPercentTradingDataToPM;
                if (tabName != TAB_Main)
                {
                    pmGrid.DeleteViewEnabled = true;
                    pmGrid.RenameViewEnabled = true;
                    pmGrid.RenameViewClick += OnRenameViewClick;
                    pmGrid.DeleteViewClick += OnDeleteViewClick;
                }
                else
                {
                    pmGrid.DeleteViewEnabled = false;
                    pmGrid.RenameViewEnabled = false;
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

        private void PMGrid_SendGroupedColumnNames(object sender, EventArgs<List<string>> e)
        {
            try
            {
                _exInstance.PMCurrentViewGroupedColumns = e.Value;
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
        /// Pass Symbol, OrderSide and List of Accounts To PM.cs
        /// </summary>
        /// <param name="sender">CtrlMainCOnsolidationView</param>
        /// <param name="e">e.Value = Symbol,e.Value2 = OrderSideTagValue, e.Value3 = List of Accounts </param>
        void pmGrid_SendPercentTradingDataToPM(object sender, EventArgs<string, PTTMasterFundOrAccount, List<string>, string> e)
        {
            try
            {
                if (PercentTradingDataToPM != null)
                    PercentTradingDataToPM(this, e);
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

        internal void LoadPreferencesAndColumns(CustomViewPreferences customViewPreferences)
        {
            try
            {
                pmGrid.LoadPreferencesAndColumns(customViewPreferences);
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

        internal void SaveLayout(bool clearGroupByCollection, string key)
        {
            try
            {
                pmGrid.SaveLayout(clearGroupByCollection, key, _filterDetails);
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

        internal void SaveLayout(bool clearGroupByCollection, string key, ref CustomViewPreferences currentDefaultpreferences)
        {
            try
            {
                pmGrid.SaveLayout(clearGroupByCollection, key, ref currentDefaultpreferences);
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

        internal void SendPMClosingInstruction()
        {
            try
            {
                if (pmDashboard != null)
                {
                    pmDashboard.CloseColumnChooser();
                }

                if (pmGrid != null)
                {
                    pmGrid.ClearPMAppearanceCache();
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

        internal void SetGridFontSize(decimal p)
        {
            try
            {
                pmDashboard.SetGridFontSize(p);
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

        internal void SummarySettings(ExPNLPreferenceMsgType prefMSGType, string keyData)
        {
            try
            {
                pmGrid.SummarySettings(prefMSGType, keyData);
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

        internal void UpdateDashBoard(string selectedTabKey)
        {
            try
            {
                pmDashboard.LoadLayout(Application.StartupPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID + "\\" + selectedTabKey);
                pmDashboard.SetGridFontSize(PMAppearanceManager.PMAppearance.FontSizeDashboard);
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

        internal void UpdateGridPreferences(bool valueOne)
        {
            try
            {
                pmGrid.UpdateGridPreferences(valueOne, true);
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

        internal void SetThemeForUserControl()
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PM);
                CustomThemeHelper.SetThemeProperties(pmGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CONSOLIDATION_PANEL);
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

        internal void DeleteCustomView()
        {

            try
            {
                pmGrid.TradeClick -= PMGrid_TradeClick;
                pmGrid.AppearanceClick -= PMGrid_AppearanceClick;
                pmGrid.AddNewConsolidationView -= AddNewConsolidationView;
                pmGrid.DeleteViewClick -= OnDeleteViewClick;
                pmGrid.RenameViewClick -= OnRenameViewClick;
                pmGrid.SaveAllGridLayouts -= PMGrid_SaveAllGridLayouts;
                pmGrid.TaxlotsRequested -= PMGrid_TaxlotsRequested;
                pmGrid.AfterColPositionChanged -= PMGrid_AfterColPositionChanged;
                pmGrid.SendFilterColumnName -= PMGrid_SendFilterColumnName;
                pmGrid.SendGroupedColumnNames -= PMGrid_SendGroupedColumnNames;
                pmGrid.SendPercentTradingDataToPM -= pmGrid_SendPercentTradingDataToPM;
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

        internal void SaveAllDashboards(PMPrefrenceManager _prefrenceManager, string DASHBOARD_FILENAME)
        {
            try
            {
                //Saving the consolidation and account dash boards.
                if (pmDashboard != null)
                {
                    pmDashboard.SaveLayout(_prefrenceManager.GetPreferenceDirectory() + "\\" + DASHBOARD_FILENAME);
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

        #region events

        private void OnDeleteViewClick(object sender, EventArgs e)
        {
            try
            {
                CtrlMainConsolidationView ctrlMainConsolidationView = sender as CtrlMainConsolidationView;
                if (ctrlMainConsolidationView == null)
                {
                    return;
                }
                string key = ctrlMainConsolidationView.ParentKey;

                //The key would be some like "Account_CustomView_New", so we just need to rmeove Account_CustomView_ and get the actual tab name
                string oneDilimiterRemovedStr = key.Substring(key.IndexOf('_') + 1);
                DialogResult result = MessageBox.Show(this, "Delete " + oneDilimiterRemovedStr + "! \n Are you sure?", "Position Management Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                string[] strArr = key.Split('_');
                if (strArr.Length >= 2)
                {
                    if (strArr[0] == SUB_MODULE_NAME)
                    {
                        if (DeleteCustomViewTabEvent != null)
                        {
                            DeleteCustomViewTabEvent(this, new EventArgs<string, string>(key, oneDilimiterRemovedStr));
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

        private void OnRenameViewClick(object sender, EventArgs e)
        {
            try
            {
                CtrlMainConsolidationView ctrlMainConsolidationView = sender as CtrlMainConsolidationView;
                if (ctrlMainConsolidationView == null)
                {
                    return;
                }

                string key = ctrlMainConsolidationView.ParentKey;

                //The key would be some like "Account_CustomView_New", so we just need to rmeove Account_CustomView_ and get the actual tab name
                string oneDilimiterRemovedStr = key.Substring(key.IndexOf('_') + 1);
                string[] strArr = key.Split('_');
                if (strArr.Length >= 2) //
                {
                    if (strArr[0] == SUB_MODULE_NAME)
                    {

                        if (RenameCustomviewEvent != null)
                            RenameCustomviewEvent(this, new EventArgs<string, string, ExPNLData>(key, oneDilimiterRemovedStr, ExPNLData.Account));
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

        private void PMGrid_AddNewConsolidationView(object sender, EventArgs e)
        {
            try
            {
                if (AddNewConsolidationView != null)
                    AddNewConsolidationView(this, e);
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

        private void PMGrid_AfterColPositionChanged(object sender, AfterColPosChangedEventArgs e)
        {
            try
            {
                if (PmGridColPositionChanged != null)
                    PmGridColPositionChanged(this, e);
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

        private void PMGrid_PricingInputClick(object sender, EventArgs e)
        {
            try
            {
                if (LanuchPricingInput != null)
                    LanuchPricingInput(sender, e);
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

        private void PMGrid_SaveAllGridLayouts(Object sender, EventArgs<string, string, bool> e)
        {
            try
            {
                if (SaveAllGridLayouts != null)
                {
                    SaveAllGridLayouts(this, new EventArgs<string, string, bool>(e.Value, e.Value2, e.Value3));
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

        private void PMGrid_SendFilterColumnName(object sender, AfterRowFilterChangedEventArgs e)
        {
            try
            {
                _prevFilterDetails = _filterDetails;
                if (e != null)
                {
                    if (_mfFilterDetails != null && e.Column.Key.Equals(OrderFields.PROPERTY_LEVEL1NAME) &&
                        (e.NewColumnFilter.FilterConditions == null || e.NewColumnFilter.FilterConditions.Count == 0))
                    {
                        _filterDetails = _mfFilterDetails;
                    }
                    else
                    {
                        _filterDetails = e.NewColumnFilter.FilterConditions != null
                            ? new GridColumnFilterDetails(e.Column.Key, e.NewColumnFilter.FilterConditions.Cast<FilterCondition>().ToList())
                            : new GridColumnFilterDetails(e.Column.Key);
                        if (e.Column.Key.Equals(OrderFields.PROPERTY_MASTERFUND))
                        {

                            _mfFilterDetails = _filterDetails;
                        }
                    }
                }
                else
                {
                    _filterDetails = null;
                    _mfFilterDetails = null;
                }
                if (FilteredColumnNameToPM != null)
                    FilteredColumnNameToPM(this, new EventArgs<GridColumnFilterDetails, GridColumnFilterDetails>(_filterDetails, _prevFilterDetails));
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

        private void PMGrid_TaxlotsRequested(object sender, EventArgs<string, string, int> e)
        {
            try
            {
                if (TaxlotsRequested != null)
                    TaxlotsRequested(this, new EventArgs<string, string, int, GridColumnFilterDetails>(e.Value, e.Value2, e.Value3, _filterDetails));
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

        private void PMGrid_TradeClick(object sender, EventArgs<OrderSingle, Dictionary<int, double>> e)
        {
            try
            {
                if (PassTradeClickEvent != null)
                    PassTradeClickEvent(this, e);
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

        private void PMGrid_AppearanceClick(object sender, EventArgs e)
        {
            try
            {
                if (Appearance_Click != null)
                    Appearance_Click(this, e);
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

        private void PMGrid_ShowHideDashboardEvent(object sender, EventArgs<bool> e)
        {
            try
            {
                this.pmDashboard.Visible = e.Value;
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