using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.ExposurePnlCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.PM.Client.UI.Classes;
using Prana.PM.Client.UI.Controls;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Forms
{
    public partial class PM : Form, IPositionManagement
    {
        #region public-variables

        public static string _exportTabName = String.Empty;
        public bool _isFormClosed;
        private bool _isLoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLoggingEnabled"]);

        #endregion

        #region private-variables

        private const string DEFAULT_CUSTOM_VIEW = "Default";
        private const string OptionBook_View = "Option Book";
        private const string SUB_MODULE_NAME = "CustomView";
        private const string TAB_Main = "Main";
        private static PMPrefrenceManager _prefrenceManager;
        private readonly object _lockerObj = new object();
        private ICommunicationManager _communicationManager;
        private Dictionary<string, PMTaxLotsDisplayForm> _dictOfCmpressedRowAndDetailForm = new Dictionary<string, PMTaxLotsDisplayForm>();
        private ExposurePnlCacheManager _exInstance;
        private ICommunicationManager _exposurePnlCommunicationManager;
        private bool _firstTime;
        private Dictionary<string, PMUserControl> _instanceLookup;
        private bool _isPermitted;
        private object _lockOnDict = new object();
        private CompanyUser _loginUser;
        private int _numberOfCustomViews;
        private Appearance _pmAppearance;
        private CustomViewPreferencesList _preferencesList;
        private PMUIPrefs _uiPrefs = new PMUIPrefs();
        private BaseEquityValue frmEquityBaseValue;
        private const int _unAllocatedAccountID = -1;
        #endregion

        #region public delegates

        public delegate void NewColumnAdded(string senderGridKey, UltraGridColumn newColumn);
        public delegate void SaveGridLayoutHandler(string sourceGridKey, string targetGridKey, bool saveAsDefault);

        #endregion

        #region private delegates

        private delegate void TaxlotsReceived(ExposurePnlCacheItemList taxlotlist, string value1, string value2);
        private delegate void UIRefresh(bool dataReceivedForFirstTime);

        #endregion

        #region public events
        public event EventHandler ClosePositionClick;
        public event EventHandler CorporateActionClick;
        public event EventHandler FormClosedHandler;
        public event EventHandler MarkPriceClick;
        public event EventHandler PricingInputClick;
        public event EventHandler<EventArgs<OrderSingle, Dictionary<int, double>>> TradeClick;
        public event EventHandler<EventArgs<string, PTTMasterFundOrAccount, List<int>, string>> PercentTradingToolClick;
        #endregion

        #region public variables
        public ICommunicationManager CommunicationManagerInstance
        {
            get { return _communicationManager; }
            set { _communicationManager = value; }
        }

        public ICommunicationManager ExPNLCommMgrInstance
        {
            get { return _exposurePnlCommunicationManager; }
            set { _exposurePnlCommunicationManager = value; }
        }

        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set { _loginUser = value; }
        }

        public string StatusMessage
        {
            set
            {
                if (_isLoggingEnabled)
                    lblStatus.Text = value;
            }
        }
        #endregion

        #region constructor initialization and setup methods
        public PM()
        {
            try
            {
                InitializeComponent();
                ApplyModulePermissions();

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
        /// Applies the module permissions.
        /// </summary>
        private void ApplyModulePermissions()
        {
            try
            {
                foreach (string moduleName in ModuleManager.AvailableModulesDetails.Keys)
                {
                    if (!ModuleManager.CheckModulePermissioning(moduleName, moduleName))
                        DisableHideModuleAndTool(moduleName);
                }
                foreach (string toolName in ModuleManager.AvailableTools.Keys)
                {
                    if (!ModuleManager.CheckModulePermissioning(toolName, toolName))
                        DisableHideModuleAndTool(toolName);
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
        /// Disables Or hide module And tool.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        private void DisableHideModuleAndTool(string moduleName)
        {
            try
            {
                foreach (var buttonTool in ultraToolbarsManager1.Tools)
                {
                    if (buttonTool.SharedPropsInternal.Caption.Contains(moduleName))
                    {
                        if (ModuleManager.AllOMSModuleSet.Contains(moduleName))
                            buttonTool.SharedProps.Enabled = false;
                        else if (!ModuleManager.AllMandatoryModuleSet.Contains(moduleName))
                            buttonTool.SharedProps.Visible = false;
                        break;
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
            try
            {
                //This class is used to marshal calls to the ui thread.
                UIThreadMarshaller.AddFormForMarshalling(UIThreadMarshaller.PM_FORM, this);
                PreInitialize();
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
            return this;
        }

        private void PreInitialize()
        {
            try
            {
                _uiPrefs = PMDatabaseManager.GetCompanyPMPreferences(LoginUser.CompanyID);
                _prefrenceManager = PMPrefrenceManager.GetInstance(SUB_MODULE_NAME);
                _prefrenceManager.SetUser(_loginUser);
                defaultPmUserControl.PreInitialize(_uiPrefs);
                ultraToolbarsManager1.Tools.RemoveAt(ultraToolbarsManager1.Tools.IndexOf("START WRITING DATA"));
                statusStrip1.Visible = Boolean.Parse(ConfigurationManager.AppSettings["PMStatusBarEnabled"]);
                _instanceLookup = new Dictionary<string, PMUserControl>();
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

        public void InitializePM()
        {
            try
            {
                lock (_lockerObj)
                {
                    _isFormClosed = false;
                }

                _exInstance = ExposurePnlCacheManager.GetInstance();
                _exInstance.Initialise(_loginUser, _exposurePnlCommunicationManager);
                defaultPmUserControl.InitializePMUserControl(_loginUser, _exInstance);
                LoadPreferences(_loginUser.FirstName + _loginUser.LastName);
                WireEvents();
                WirePmUserControlEvents(defaultPmUserControl);
                CustomThemeHelper.SetThemeProperties(FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PM);
                foreach (PMUserControl pmUserControl in GetAllChildren(this).OfType<PMUserControl>())
                {
                    pmUserControl.SetThemeForUserControl();
                }
                if (CustomThemeHelper.ApplyTheme)
                {
                    statusStrip1.BackColor = Color.FromArgb(88, 88, 90);
                    lblStatus.BackColor = Color.FromArgb(88, 88, 90);
                    statusStrip1.ForeColor = Color.FromArgb(236, 240, 241);
                    lblStatus.ForeColor = Color.FromArgb(236, 240, 241);
                    ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, Text, CustomThemeHelper.UsedFont);
                }
                _isPermitted = WindsorContainerManager.GetPMModulePermission(_loginUser.CompanyID);

                //atul dislay (8-Feb-2016)
                //do not remove this unused local variable as this forces the form to create the handler which is required to set the tab orders
                var x = this.Handle;
                if (IsHandleCreated)
                {
                    SetTabOrder();
                }
                if (PMDatabaseManager.GetPMPrefDataFromDB(_loginUser.CompanyUserID).IsShowPMToolbar == true)
                {
                    this.ultraToolbarsManager1.Visible = true;
                }
                else
                    this.ultraToolbarsManager1.Visible = false;
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
                lock (_lockerObj)
                {
                }
            }
        }
        #endregion

        public void ApplyPreferences(object sender, EventArgs<string, IPreferenceData> e)
        {
            try
            {
                IPreferenceData prefs = e.Value2;
                PMPreferenceData appliedPrefs = prefs as PMPreferenceData;
                if (appliedPrefs != null)
                {
                    double xpercent = ((PMPreferenceData)prefs).XPercentofAvgVolume;
                    bool pmToolStatus = ((PMPreferenceData)prefs).IsShowPMToolbar;
                    bool useClosingMark = ((PMPreferenceData)prefs).UseClosingMark;
                    SendPreferencesUpdateMessage(useClosingMark, xpercent);
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
        /// Highlights the symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public void HighlightSymbol(string symbol)
        {
            try
            {
                if (PMTabView.ActiveTab != null)
                {
                    PMUserControl PMUserControl = PMTabView.ActiveTab.TabPage.Controls.OfType<PMUserControl>().FirstOrDefault();
                    PMUserControl.HighlightSymbol(symbol);
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

        public void ShowHideDashboard(bool isDashboardVisible)
        {
            try
            {
                if (PMTabView.ActiveTab != null)
                {
                    PMUserControl PMUserControl = PMTabView.ActiveTab.TabPage.Controls.OfType<PMUserControl>().FirstOrDefault();
                    PMUserControl.ShowHideDashboard(isDashboardVisible);
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

        public void PMPrefUpdated(ExPNLPreferenceMsgType prefMSGType, string tabKey, string keyData)
        {
            try
            {
                switch (prefMSGType)
                {
                    case ExPNLPreferenceMsgType.CustomViewDeleted:
                    case ExPNLPreferenceMsgType.CustomViewAdded:
                    case ExPNLPreferenceMsgType.SelectedViewChanged:
                        _exInstance.PMPrefUpdated(prefMSGType, tabKey);
                        break;

                    case ExPNLPreferenceMsgType.SelectedColumnAdded:
                    case ExPNLPreferenceMsgType.SelectedColumnDeleted:
                    case ExPNLPreferenceMsgType.FilterValueChanged:
                        _exInstance.PMPrefUpdated(prefMSGType, keyData);
                        break;

                }

                if (_instanceLookup != null)
                {
                    //if there are Two arguments in info
                    //case 1: tab change event:info[1] will be currentTabKey and info[2] will be previousTab key
                    //case 2: Col position change event:info[1] will be currentTabKey and info[2] will be column Key
                    if (_instanceLookup.ContainsKey(tabKey))
                    {
                        //refresh the summary of the current tab
                        _instanceLookup[tabKey].SummarySettings(prefMSGType, keyData);
                    }

                    if (!string.IsNullOrWhiteSpace(keyData) && _instanceLookup.ContainsKey(keyData))
                    {
                        //remove the summary of the previous tab
                        _instanceLookup[keyData].SummarySettings(prefMSGType, "true");
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

        public void RequestAccountData()
        {
            try
            {
                defaultPmUserControl.RequestAccountData();
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

        internal void AddCustomView()
        {
            try
            {
                bool tabAdded = false;
                string tabName = string.Empty;

                DialogResult result = GetTabNameFromInputBox(ref tabName);

                if (tabName != string.Empty && result == DialogResult.OK)
                {
                    CustomViewPreferences pref = null;

                    pref = new CustomViewPreferences();
                    if (PMTabView.Tabs.Exists(tabName.Trim()))
                    {
                        MessageBox.Show("Custom view with the same name already exist. Please choose another name.", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        if (_preferencesList.ContainsKey(DEFAULT_CUSTOM_VIEW))
                        {
                            PranaBinaryFormatter pranaBinaryFormatter = new PranaBinaryFormatter();
                            pref = (CustomViewPreferences)pranaBinaryFormatter.DeSerialize(pranaBinaryFormatter.Serialize(_preferencesList[DEFAULT_CUSTOM_VIEW]));
                        }
                        GetPreferencesWithUnallocatedFilter(pref);
                        tabAdded = CreateCustomViewTab(tabName, pref, PMTabView);
                    }
                    if (pref == null)
                    {
                        throw new Exception("Problem in setting preferences.");
                    }
                    if (tabAdded)
                    {
                        _preferencesList.Add(SUB_MODULE_NAME + "_" + tabName, pref);
                        StringBuilder sb = new StringBuilder();
                        sb.Append(SUB_MODULE_NAME).Append("_").Append(tabName).Append(Seperators.SEPERATOR_8).Append(DEFAULT_CUSTOM_VIEW);
                        if (_exInstance != null)
                        {
                            PMPrefUpdated(ExPNLPreferenceMsgType.CustomViewAdded, sb.ToString(), string.Empty);
                        }
                        string key = SUB_MODULE_NAME + "_" + tabName;
                        List<int> currentFilteredAccountList = new List<int>();
                        GetFilteredAccountList(_instanceLookup[key].FilterDetails, ref currentFilteredAccountList);
                        Splitter splitterControl = ((Splitter)_instanceLookup[key].Controls[1]);

                        int defaultSplitterPosition = int.MinValue;
                        if (_preferencesList.ContainsKey(DEFAULT_CUSTOM_VIEW))
                        {
                            defaultSplitterPosition = _preferencesList[DEFAULT_CUSTOM_VIEW].SplitterPosition;
                        }
                        else
                        {
                            defaultSplitterPosition = _preferencesList[TAB_Main].SplitterPosition;
                        }
                        if (defaultSplitterPosition != int.MinValue)
                        {
                            splitterControl.MinSize = defaultSplitterPosition;
                            splitterControl.SplitPosition = defaultSplitterPosition;
                            splitterControl.MinSize = 25;
                        }
                        SendFilterChangedDetailsToServer(currentFilteredAccountList, null, key);
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

        internal int GetSplitterPosition(string key)
        {
            int splitterposition = 0;
            try
            {
                splitterposition = ((Splitter)this._instanceLookup[key].Controls[1]).SplitPosition;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return splitterposition;
        }

        internal void PauseUpdates()
        {
            try
            {
                _exInstance.PauseUpdates();
                _exInstance.TaxlotsReceived -= _exInstance_TaxlotsReceived;
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

        internal void ResumeUpdates()
        {
            try
            {
                _exInstance.ResumeUpdates();
                _exInstance.TaxlotsReceived += _exInstance_TaxlotsReceived;
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

        internal void SendPreferencesUpdateMessage(bool useClosingMark, double xpercent)
        {
            try
            {
                _exInstance.SendPreferencesUpdateMsg(useClosingMark, xpercent);
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

        internal void SetSplitterPosition(int splitterPosition, string key)
        {
            try
            {
                if (key == TAB_Main)
                {
                    key = TAB_Main;
                }
                if (_instanceLookup.ContainsKey(key))
                {
                    Splitter splitterControl = ((Splitter)this._instanceLookup[key].Controls[1]);
                    if (!splitterControl.IsAccessible)
                    {
                        Thread.Sleep(500);
                    }
                    if (splitterPosition <= 0) return;
                    splitterControl.MinSize = splitterPosition;
                    splitterControl.SplitPosition = splitterPosition;
                    splitterControl.MinSize = 25;
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

        PMViewPreferencesList lsPmvp = null;
        private void _exInstance_SetPMViewPreferencesList(object sender, EventArgs<List<string>> e)
        {
            try
            {
                if (lsPmvp != null)
                {
                    if (UIValidation.GetInstance().validate(this))
                    {
                        if (this.InvokeRequired)
                        {
                            EventHandler<EventArgs<List<string>>> mainThread = _exInstance_SetPMViewPreferencesList;
                            this.BeginInvoke(mainThread, new object[] { sender, e });
                        }
                        else
                        {
                            SendFreshPreferenceList(e);
                            return;
                        }
                    }
                }
                else if (_preferencesList != null)
                {
                    lsPmvp = new PMViewPreferencesList();
                    //_preferencesList is the list of pref saved at client end
                    foreach (string tabName in _preferencesList.Keys)
                    {
                        var cvp = _preferencesList[tabName];
                        var pmvp = new PMViewPreferences { TabName = tabName };

                        if (cvp.SelectedColumnsCollection != null && cvp.SelectedColumnsCollection.Count > 0)
                        {
                            pmvp.SelectedDynamicColumnsCollection = new List<string>();
                            foreach (PreferenceGridColumn pgc in cvp.SelectedColumnsCollection)
                                if (!pgc.Hidden && e.Value.Contains(pgc.Name))
                                {
                                    pmvp.SelectedDynamicColumnsCollection.Add(pgc.Name);
                                }
                        }
                        if (cvp.GroupByColumnsCollection != null && cvp.GroupByColumnsCollection.Count > 0)
                        {
                            pmvp.GroupByDynamicColumnsCollection = new List<string>();
                            foreach (string grpColumn in cvp.GroupByColumnsCollection)
                                if (e.Value.Contains(grpColumn))
                                {
                                    pmvp.GroupByDynamicColumnsCollection.Add(grpColumn);
                                }
                        }
                        lsPmvp.Add(tabName, pmvp);
                    }
                    string selectedTabKey = GetSelectedTabKey();

                    if (lsPmvp.Count > 0 && !string.IsNullOrEmpty(selectedTabKey))
                    {
                        lsPmvp.SelectedView = selectedTabKey;
                    }

                    if (lsPmvp.Count > 0)
                    {
                        ExposurePnlCacheManager.GetInstance().SendPMPreferences(ExPNLPreferenceMsgType.NewPreferences, lsPmvp.ToString());
                    }
                    if (_instanceLookup != null && _instanceLookup.ContainsKey(selectedTabKey))
                    {
                        PMPrefUpdated(ExPNLPreferenceMsgType.SelectedViewChanged, selectedTabKey, string.Empty);
                    }
                    if (_instanceLookup != null)
                    {
                        if (selectedTabKey != null)
                        {
                            List<int> currentFilteredAccountList = new List<int>();
                            GetFilteredAccountList(_instanceLookup[selectedTabKey].FilterDetails, ref currentFilteredAccountList);
                            SendFilterChangedDetailsToServer(currentFilteredAccountList, null, selectedTabKey);
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

        private void SendFreshPreferenceList(EventArgs<List<string>> eventArgs)
        {
            var pmViewPreferencesList = new PMViewPreferencesList();
            //_preferencesList is the list of pref saved at client end
            foreach (string tabName in _instanceLookup.Keys)
            {
                var cvp = _instanceLookup[tabName].GetLayout(false, tabName);
                var pmvp = new PMViewPreferences { TabName = tabName };

                if (cvp.SelectedColumnsCollection != null && cvp.SelectedColumnsCollection.Count > 0)
                {
                    pmvp.SelectedDynamicColumnsCollection = new List<string>();
                    foreach (PreferenceGridColumn pgc in cvp.SelectedColumnsCollection)
                        if (!pgc.Hidden && eventArgs.Value.Contains(pgc.Name))
                        {
                            pmvp.SelectedDynamicColumnsCollection.Add(pgc.Name);
                        }
                }
                if (cvp.GroupByColumnsCollection != null && cvp.GroupByColumnsCollection.Count > 0)
                {
                    pmvp.GroupByDynamicColumnsCollection = new List<string>();
                    foreach (string grpColumn in cvp.GroupByColumnsCollection)
                        if (eventArgs.Value.Contains(grpColumn))
                        {
                            pmvp.GroupByDynamicColumnsCollection.Add(grpColumn);
                        }
                }
                pmViewPreferencesList.Add(tabName, pmvp);
            }
            string selectedTabKey = GetSelectedTabKey();

            if (pmViewPreferencesList.Count > 0 && !string.IsNullOrEmpty(selectedTabKey))
            {
                pmViewPreferencesList.SelectedView = selectedTabKey;
            }

            if (pmViewPreferencesList.Count > 0)
            {
                ExposurePnlCacheManager.GetInstance().SendPMPreferences(ExPNLPreferenceMsgType.NewPreferences, pmViewPreferencesList.ToString());
            }
            if (_instanceLookup != null && _instanceLookup.ContainsKey(selectedTabKey))
            {
                PMPrefUpdated(ExPNLPreferenceMsgType.SelectedViewChanged, selectedTabKey, string.Empty);
            }
            if (_instanceLookup != null)
            {
                foreach (string tabName in _instanceLookup.Keys)
                {
                    if (tabName != null)
                    {
                        List<int> currentFilteredAccountList = new List<int>();
                        GetFilteredAccountList(_instanceLookup[tabName].FilterDetails, ref currentFilteredAccountList);
                        SendFilterChangedDetailsToServer(currentFilteredAccountList, null, tabName);
                    }
                }
            }
        }

        private void _exInstance_TaxlotsReceived(object sender, EventArgs<ExposurePnlCacheItemList, string, string> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(PMTabView))
                {
                    if (PMTabView.InvokeRequired)
                    {
                        TaxlotsReceived taxLotsreceived = ShowTaxLotsForm;
                        BeginInvoke(taxLotsreceived, e.Value, e.Value2, e.Value3);
                    }
                    else
                    {
                        ShowTaxLotsForm(e.Value, e.Value2, e.Value3);
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

        private void _exInstance_UpdateOrderSummaryTable(object sender, EventArgs<DataTable, List<int>> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (_exInstance != null && _exInstance.ConsolidationViewSummary != null && _prefrenceManager != null)
                    {
                        foreach (KeyValuePair<string, PMUserControl> pmUserControl in _instanceLookup)
                        {
                            if (_exInstance.ConsolidationViewSummary != null && _prefrenceManager != null)
                            {
                                pmUserControl.Value.BindDataSource(_exInstance.ConsolidationViewSummary.Copy(), _prefrenceManager.GetPreferenceDirectory() + "\\" + pmUserControl.Key + "Dashboard.xml");
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

        private void _exposurePnlCacheManagerInstance_ExposurePnlCacheSummaryChanged(object sender, EventArgs<bool> e)
        {
            try
            {
                if (IsDisposed || Disposing)
                {
                    return;
                }
                if (PMTabView != null)
                {
                    if (UIValidation.GetInstance().validate(PMTabView))
                    {
                        if (PMTabView != null && PMTabView.InvokeRequired)
                        {
                            UIRefresh UIRefreshHandler = UpdateUI;
                            try
                            {
                                BeginInvoke(UIRefreshHandler, e.Value);
                            }
                            catch (ObjectDisposedException)
                            {
                                //http://jira.nirvanasolutions.com:8080/browse/PRANA-6350
                                //Do Nothing as here is thread race condition and in some cases it will throgh error while closing the form.
                            }
                        }
                        else
                        {
                            UpdateUI(e.Value);
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

        private void _pmAppearance_UpdateDashboard(object sender, EventArgs<bool> e)
        {
            try
            {
                if (e.Value)
                {
                    foreach (KeyValuePair<string, PMUserControl> pmUserControl in _instanceLookup)
                    {
                        pmUserControl.Value.UpdateDashBoard(pmUserControl.Key + "Dashboard.xml");
                    }
                }
                else
                {
                    defaultPmUserControl.UpdateDashBoard(GetSelectedTabKey() + "Dashboard.xml");
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

        private void BindCustomDashBoard(string selectedTabKey, string previousTabKey, string renameFrom = "")
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (_exInstance != null && _exInstance.ConsolidationViewSummary != null && _prefrenceManager != null)
                    {
                        foreach (KeyValuePair<string, PMUserControl> pmUserControl in _instanceLookup)
                        {
                            string activeTabKey = GetTabKey(PMTabView.ActiveTab.Key, TAB_Main);
                            if (activeTabKey == pmUserControl.Key)
                            {
                                var destFile = _prefrenceManager.GetPreferenceDirectory() + "\\" + DEFAULT_CUSTOM_VIEW + "Dashboard.xml";
                                if (!String.IsNullOrEmpty(renameFrom))
                                {
                                    string path = Application.StartupPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID;
                                    if (File.Exists(path + "\\" + renameFrom + "Dashboard.xml"))
                                        File.Move(path + "\\" + renameFrom + "Dashboard.xml", path + "\\" + activeTabKey + "Dashboard.xml");
                                }
                                else if (File.Exists(destFile))
                                {
                                    CopyDefaultDashBoard(activeTabKey, false);
                                }
                                pmUserControl.Value.BindDataSource(_exInstance.ConsolidationViewSummary.Copy(), _prefrenceManager.GetPreferenceDirectory() + "\\" + activeTabKey + "Dashboard.xml");
                                if (_exInstance.AccountWiseSummary.ContainsKey(activeTabKey))
                                {
                                    var summary = _exInstance.AccountWiseSummary[activeTabKey].Copy();
                                    _instanceLookup[activeTabKey].AssignData(summary);
                                    List<int> currentFilteredAccountList = new List<int>();
                                    GetFilteredAccountList(_instanceLookup[activeTabKey].FilterDetails, ref currentFilteredAccountList);
                                    _exInstance.OverrideGridColumns(GetSelectedTabKey(), currentFilteredAccountList, false);
                                }
                            }
                        }
                    }
                    PMPrefUpdated(ExPNLPreferenceMsgType.SelectedViewChanged, selectedTabKey, previousTabKey);
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

        private void ChangeIconForTheme()
        {
            try
            {
                ComponentResourceManager resources = new ComponentResourceManager(typeof(PM));
                if (!CustomThemeHelper.ApplyTheme)
                {
                    Icon = ((Icon)(resources.GetObject("PMIconThemeOff")));
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

        private void ClearData()
        {
            if (helpProvider1 != null)
                helpProvider1.Dispose();
            helpProvider1 = null;
            if (ultraToolbarsManager1 != null)
                ultraToolbarsManager1.ToolClick -= ultraToolbarsManager1_ToolClick;
            if (PMTabView != null)
                PMTabView.SelectedTabChanged -= PMTabView_SelectedTabChanged;
            if (PMTabView != null)
                PMTabView.Dispose();
            PMTabView = null;
            _dictOfCmpressedRowAndDetailForm = null;

            if (_prefrenceManager != null)
                _prefrenceManager.Dispose();
            _prefrenceManager = null;
            _preferencesList = null;
            if (_instanceLookup != null)
            {
                foreach (PMUserControl ctrl in _instanceLookup.Values)
                {
                    UnWirePmUserControlEvents(ctrl);
                    defaultPmUserControl.UnWireEvent(ctrl);
                    ctrl.ClearData();
                    ctrl.Dispose();
                }
            }
            defaultPmUserControl.ClearData();
        }

        private void UnWirePmUserControlEvents(PMUserControl pmUserControl)
        {
            try
            {
                if (pmUserControl != null)
                {
                    pmUserControl.RenameCustomviewEvent -= customPmUserControl_RenameCustomviewEvent;
                    pmUserControl.DeleteCustomViewTabEvent -= customPmUserControl_DeleteCustomViewTabEvent;
                    pmUserControl.LanuchPricingInput -= OnPricingInputClick;
                    pmUserControl.PmGridColPositionChanged -= defaultPmUserControl_PmGridColPositionChanged;
                    pmUserControl.TaxlotsRequested -= defaultPmUserControl_TaxlotsRequested;
                    pmUserControl.SaveAllGridLayouts -= defaultPmUserControl_SaveAllGridLayouts;
                    pmUserControl.AddNewConsolidationView -= pmUserControl_AddNewConsolidationView;
                    pmUserControl.FilteredColumnNameToPM -= pmUserControl_FilteredColumnNameToPM;
                    pmUserControl.PassTradeClickEvent -= pmUserControl_PassTradeClickEvent;
                    pmUserControl.PercentTradingDataToPM -= pmUserControl_PercentTradingDataToPM;
                    pmUserControl.Appearance_Click -= pmUserControl_AppearanceClickEvent;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void ClearStatusMessage()
        {
            try
            {
                lblStatus.Text = "";
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

        private bool CreateCustomViewTab(string tabName, CustomViewPreferences preference, UltraTabControl tbcCustomViewRef, string renameFrom = "")
        {
            try
            {
                if (_numberOfCustomViews >= _uiPrefs.NumberOfCustomViewsAllowed)
                {
                    MessageBox.Show("Max number of allowed custom views reached", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                _numberOfCustomViews += 1;
                if (!tbcCustomViewRef.Tabs.Exists(tabName))
                {
                    string selectedTab = string.Empty;
                    if (tbcCustomViewRef.SelectedTab != null)
                    {
                        selectedTab = tbcCustomViewRef.SelectedTab.ToString();
                    }

                    string key = SUB_MODULE_NAME + "_" + tabName;
                    var tab = tbcCustomViewRef.Tabs.Add(tabName);
                    tab.Text = tabName;
                    tab.Tag = int.MinValue;

                    PMUserControl customPmUserControl = new PMUserControl();
                    customPmUserControl.Dock = DockStyle.Fill;
                    customPmUserControl.PreInitialize(_uiPrefs);

                    if (tabName != DEFAULT_CUSTOM_VIEW)
                    {
                        customPmUserControl.RenameCustomviewEvent += customPmUserControl_RenameCustomviewEvent;
                        customPmUserControl.DeleteCustomViewTabEvent += customPmUserControl_DeleteCustomViewTabEvent;
                    }
                    customPmUserControl.InitializePMUserControl(_loginUser, _exInstance);
                    customPmUserControl.InitNewView(preference, key);
                    WirePmUserControlEvents(customPmUserControl);
                    tab.TabPage.Controls.Add(customPmUserControl);
                    _instanceLookup.Add(key, customPmUserControl);
                    tab.Selected = true;
                    BindCustomDashBoard(key, selectedTab, renameFrom);
                    customPmUserControl.SetThemeForUserControl();
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

        private void customPmUserControl_DeleteCustomViewTabEvent(object sender, EventArgs<string, string> e)
        {
            try
            {
                DeleteCustomViewTab(e.Value, e.Value2, PMTabView);
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

        private void customPmUserControl_RenameCustomviewEvent(object sender, EventArgs<string, string, ExPNLData> e)
        {
            try
            {
                RenameCustomView(e.Value, e.Value2);
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

        private void defaultPmUserControl_PmGridColPositionChanged(object sender, AfterColPosChangedEventArgs e)
        {
            try
            {
                string selectedTabKey = GetSelectedTabKey();
                //If login client is writing data and changed column is in write list then here is no need to send the event to server
                if (_exInstance != null)
                {
                    if (!e.ColumnHeaders[0].Column.Hidden)
                    {
                        PMPrefUpdated(ExPNLPreferenceMsgType.SelectedColumnAdded, selectedTabKey, e.ColumnHeaders[0].Column.Key);
                        SetStatusMessage();
                    }
                    else
                    {
                        PMPrefUpdated(ExPNLPreferenceMsgType.SelectedColumnDeleted, selectedTabKey, e.ColumnHeaders[0].Column.Key);
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

        private void defaultPmUserControl_SaveAllGridLayouts(object sender, EventArgs<string, string, bool> e)
        {
            try
            {
                SavePMTabsOrder();
                if (e.Value3)
                {
                    if (_instanceLookup != null && _instanceLookup.ContainsKey(e.Value))
                    {
                        bool currentDefaultPrefAvailable;
                        CustomViewPreferences currentDefaultpreferences;
                        if (_preferencesList.ContainsKey(DEFAULT_CUSTOM_VIEW))
                        {
                            currentDefaultPrefAvailable = true;
                            currentDefaultpreferences = _preferencesList[DEFAULT_CUSTOM_VIEW];
                        }
                        else
                        {
                            currentDefaultPrefAvailable = false;
                            currentDefaultpreferences = new CustomViewPreferences();
                        }
                        var topLevelControl = (PM)TopLevelControl;
                        if (topLevelControl != null)
                            currentDefaultpreferences.SplitterPosition = topLevelControl.GetSplitterPosition(e.Value);
                        _instanceLookup[e.Value].SaveLayout(true, DEFAULT_CUSTOM_VIEW, ref currentDefaultpreferences);
                        if (currentDefaultPrefAvailable)
                        {
                            _preferencesList[DEFAULT_CUSTOM_VIEW] = currentDefaultpreferences;
                        }
                        else
                        {
                            _preferencesList.Add(DEFAULT_CUSTOM_VIEW, currentDefaultpreferences);
                        }
                        CopyDefaultDashBoard(e.Value);
                    }
                    lblStatus.Text = "Default Layout for Custom Views Updated.";
                }
                else
                {
                    if (e.Value == e.Value2)
                    {
                        if (_instanceLookup != null && _instanceLookup.ContainsKey(e.Value))
                        {
                            var topLevelControl = (PM)TopLevelControl;
                            if (topLevelControl != null)
                            {
                                int splitterPosition = topLevelControl.GetSplitterPosition(e.Value);
                                //Adding a null check for bug : 62177
                                if (!string.IsNullOrEmpty(e.Value)
                                    && _preferencesList != null
                                    && _preferencesList.ContainsKey(e.Value)
                                    && _preferencesList[e.Value] != null)
                                {
                                    _preferencesList[e.Value].SplitterPosition = splitterPosition;
                                }
                            }
                            _instanceLookup[e.Value].SaveLayout(true, e.Value);
                            SaveDashBoardLayout(e.Value);
                            lblStatus.Text = "Layout Saved Successfully.";
                        }
                    }
                    else if (e.Value2 == string.Empty && _instanceLookup != null)
                    {
                        Dictionary<string, CustomViewPreferences> dicCustomViewPreferences = new Dictionary<string, CustomViewPreferences>();
                        foreach (KeyValuePair<string, PMUserControl> existingControl in _instanceLookup)
                        {
                            CustomViewPreferences customViewPreference = new CustomViewPreferences();
                            PranaBinaryFormatter pranaBinaryFormatter = new PranaBinaryFormatter();
                            customViewPreference = (CustomViewPreferences)pranaBinaryFormatter.DeSerialize(pranaBinaryFormatter.Serialize(existingControl.Value.GetLayout(true, existingControl.Key)));

                            if (!dicCustomViewPreferences.ContainsKey(existingControl.Key))
                            {
                                dicCustomViewPreferences.Add(existingControl.Key, customViewPreference);
                            }
                            else
                            {
                                dicCustomViewPreferences[existingControl.Key] = customViewPreference;
                            }
                            dicCustomViewPreferences[existingControl.Key].SplitterPosition = ((PM)TopLevelControl).GetSplitterPosition(existingControl.Key);
                            if (existingControl.Value.FilterDetails != null)
                            {
                                var stringList = existingControl.Value.FilterDetails.DynamicFilterConditionList.ConvertAll(obj => obj.ToString());
                                if (stringList.Count == 3 && stringList.Contains(ApplicationConstants.FilterDetails_DBNullAccount))
                                {
                                    existingControl.Value.FilterDetails.DynamicFilterConditionList.Clear();
                                    FilterCondition filterCondition = new FilterCondition(FilterComparisionOperator.Equals, "(Unallocated)");
                                    existingControl.Value.FilterDetails.DynamicFilterConditionList.Add(filterCondition);
                                }
                            }
                            dicCustomViewPreferences[existingControl.Key].FilterDetails = existingControl.Value.FilterDetails;
                            SaveDashBoardLayout(existingControl.Key);
                        }
                        PMPrefrenceManager.GetInstance(SUB_MODULE_NAME).SetCustomViewPreferenceList(dicCustomViewPreferences);
                        lblStatus.Text = "All Layouts Saved Successfully.";
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

        private void defaultPmUserControl_TaxlotsRequested(object sender, EventArgs<string, string, int, GridColumnFilterDetails> e)
        {
            try
            {
                List<int> filteredAccountList = new List<int>();
                GetFilteredAccountList(e.Value4, ref filteredAccountList);
                _exInstance.SendTaxLotRequestMsg(e.Value, e.Value2, e.Value3, filteredAccountList);
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

        private void DeleteCustomViewTab(string key, string tabName, UltraTabControl tbcCustomViewRef, string renameFrom = "")
        {
            try
            {
                SetStatusMessage();
                if (!_instanceLookup.ContainsKey(key))
                {
                    return;
                }
                PMUserControl pmUserControl = _instanceLookup[key];
                pmUserControl.DeleteCustomView();

                if (!tbcCustomViewRef.Tabs.Exists(tabName))
                {
                    return;
                }
                UltraTab tab = tbcCustomViewRef.Tabs[tabName];
                tab.TabPage.Controls.Remove(pmUserControl);

                if (tbcCustomViewRef.Tabs.Contains(tab))
                {
                    tbcCustomViewRef.Tabs.Remove(tab);
                }
                _preferencesList.Remove(key);
                _instanceLookup.Remove(key);

                CustomViewPreferencesList savedList = _prefrenceManager.GetCustomViewPreferences();

                System.Threading.Tasks.Task.Run(() => WindsorContainerManager.DeleteCustomViewPreference(_loginUser.CompanyUserID, key));
                if (savedList.ContainsKey(key))
                {
                    savedList.Remove(key);
                }

                _prefrenceManager.SetPreferences(savedList);
                savedList = null;
                _numberOfCustomViews -= 1;

                tab = null;
                pmUserControl = null;

                string path = _prefrenceManager.GetPreferenceDirectory() + "\\" + key + "Dashboard.xml";
                if (File.Exists(path) && String.IsNullOrEmpty(renameFrom))
                {
                    File.Delete(path);
                }

                if (_exInstance != null)
                {
                    string currentTabKey = GetSelectedTabKey();
                    StringBuilder sb = new StringBuilder();
                    sb.Append(key).Append(",").Append(currentTabKey);
                    PMPrefUpdated(ExPNLPreferenceMsgType.CustomViewDeleted, sb.ToString(), string.Empty);
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

        // if saveFirstTime is True that means we need to set current active tab to Default Tab
        // if saveFirstTime is false that means we are creating new tab and we have to use default saved tab to create new tab 
        private void CopyDefaultDashBoard(string tabName, bool saveFirstTime = true)
        {
            try
            {
                string path = Application.StartupPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID;
                string sourceFile;
                string destFile;
                if (saveFirstTime)
                {
                    sourceFile = path + "\\" + tabName + "Dashboard.xml";
                    destFile = path + "\\" + DEFAULT_CUSTOM_VIEW + "Dashboard.xml";
                }
                else
                {
                    sourceFile = path + "\\" + DEFAULT_CUSTOM_VIEW + "Dashboard.xml";
                    destFile = path + "\\" + tabName + "Dashboard.xml";
                }
                if (File.Exists(sourceFile) && File.Exists(destFile))
                {
                    File.Copy(sourceFile, destFile, true);
                }
                else if (File.Exists(sourceFile))
                {
                    var fileStream = File.Create(destFile);
                    fileStream.Close();
                    File.Copy(sourceFile, destFile, true);
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

        private void SaveDashBoardLayout(string tabName)
        {
            try
            {
                _instanceLookup[tabName].SaveAllDashboards(_prefrenceManager, tabName + "Dashboard.xml");
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

        private void frmEquityBaseValue_Disposed(object sender, EventArgs e)
        {
            frmEquityBaseValue = null;
        }

        private IEnumerable<Control> GetAllChildren(Control root)
        {
            var children = new List<Control>();
            try
            {
                var stack = new Stack<Control>();
                stack.Push(root);

                while (stack.Any())
                {
                    var next = stack.Pop();
                    foreach (Control child in next.Controls)
                        stack.Push(child);
                    children.Add(next);
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
            return children;
        }

        private string GetSelectedTabKey()
        {
            try
            {
                foreach (UltraTab tab in PMTabView.Tabs)
                {
                    if (tab.Selected)
                    {
                        switch (tab.Key)
                        {
                            case TAB_Main:
                                if (PMTabView.SelectedTab.Key == TAB_Main)
                                {
                                    ExposurePnlCacheManager.GetInstance().SelectedTabKey = TAB_Main;
                                    return TAB_Main;
                                }
                                break;

                            default:
                                ExposurePnlCacheManager.GetInstance().SelectedTabKey = "CustomView_" + PMTabView.SelectedTab.Key;
                                return "CustomView_" + PMTabView.SelectedTab.Key;
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
            return string.Empty;
        }

        private string GetTabKey(string tabKey, string tabType)
        {
            try
            {
                if (tabKey != null)
                {
                    switch (tabType)
                    {
                        case TAB_Main:
                            if (tabKey == TAB_Main)
                                return TAB_Main;
                            else
                                return "CustomView_" + tabKey;
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
            return string.Empty;
        }

        private DialogResult GetTabNameFromInputBox(ref string tabName)
        {
            DialogResult result = DialogResult.None;
            string OldTabName = tabName;
            try
            {
                tabName = Prana.Utilities.UI.UIUtilities.InputBox.ShowInputBox("Custom View", tabName, out result).Trim();

                // Don't do anything if user has hit cancel or closed the dialog from left top "X"
                if (result == DialogResult.Cancel)
                    return result;

                if (!Prana.Utilities.UI.MiscUtilities.GeneralUtilities.CheckNameValidation(tabName))
                {
                    MessageBox.Show(this, "Invalid view name.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tabName = string.Empty;
                }
                if (!OldTabName.Equals(tabName))
                {
                    if (_preferencesList.ContainsKey(tabName))
                    {
                        MessageBox.Show(this, tabName + " already exists! \n Please enter different view name.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tabName = string.Empty;
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
            return result;
        }

        private void LoadPreferences(string userName)
        {
            Logger.LoggerWrite("Start Opening PM..." + userName, LoggingConstants.CATEGORY_GENERAL);
            try
            {
                _preferencesList = _prefrenceManager.GetCustomViewPreferences();
                CustomViewPreferences consolPreference;
                if (!_preferencesList.ContainsKey(TAB_Main))
                {
                    consolPreference = new CustomViewPreferences();
                    _prefrenceManager.GetPMDefaultPreferences(ref consolPreference);
                    _prefrenceManager.SetCustomViewPreference(consolPreference, TAB_Main);
                }
                else
                {
                    consolPreference = _preferencesList[TAB_Main];
                }
                GetColumnWithOutUnallocatedFilter(consolPreference);
                if (consolPreference != null)
                {
                    defaultPmUserControl.InitNewView(consolPreference, TAB_Main);
                    if (_instanceLookup != null && !_instanceLookup.ContainsKey(TAB_Main))
                    {
                        _instanceLookup.Add(TAB_Main, defaultPmUserControl);
                    }
                }
                string accountDefaultKey = TAB_Main;
                if (!_preferencesList.ContainsKey(accountDefaultKey))
                {
                    CustomViewPreferences defaultAccountPref = new CustomViewPreferences();
                    _prefrenceManager.GetPMDefaultPreferences(ref defaultAccountPref);
                    _preferencesList.Add(accountDefaultKey, defaultAccountPref);
                    _prefrenceManager.SetCustomViewPreference(defaultAccountPref, accountDefaultKey);
                }
                foreach (KeyValuePair<string, CustomViewPreferences> customPrefKeyPair in _preferencesList)
                {
                    string[] strArr = customPrefKeyPair.Key.Split('_');
                    if (strArr.Length >= 2 && !strArr[1].ToUpper().Equals("DEFAULT"))
                    {
                        if (strArr[0] == SUB_MODULE_NAME)
                        {
                            //In case View name consist underscore,need to include that also
                            for (int i = 2; i <= strArr.Length - 1; i++)
                            {
                                strArr[1] = strArr[1] + "_" + strArr[i];
                            }
                            GetColumnWithOutUnallocatedFilter(customPrefKeyPair.Value);
                            CreateCustomViewTab(strArr[1], customPrefKeyPair.Value, PMTabView);
                            SetSplitterPosition(customPrefKeyPair.Value.SplitterPosition, customPrefKeyPair.Key);
                        }
                    }
                    if (customPrefKeyPair.Key == TAB_Main)
                    {
                        SetSplitterPosition(customPrefKeyPair.Value.SplitterPosition, customPrefKeyPair.Key);
                    }
                }
                Logger.LoggerWrite("Opened PM..." + userName, LoggingConstants.CATEGORY_GENERAL);
                if (_preferencesList.ContainsKey("CustomView_" + PMAppearanceManager.PMAppearance.DefaultSelectedView))
                {
                    PMTabView.Tabs[PMAppearanceManager.PMAppearance.DefaultSelectedView].Selected = true;
                }
                else
                {
                    PMTabView.Tabs[TAB_Main].Selected = true;
                }
                Logger.LoggerWrite("Opened PM..." + userName, LoggingConstants.CATEGORY_GENERAL);
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

        private void GetColumnWithOutUnallocatedFilter(CustomViewPreferences consolPreference)
        {
            try
            {
                if (consolPreference != null)
                {
                    PreferenceGridColumn filterColumnAccount = consolPreference.SelectedColumnsCollection.Find(obj => obj.Name == OrderFields.PROPERTY_LEVEL1NAME);

                    if (filterColumnAccount.FilterConditionList.ConvertAll(obj => obj.ToString()).Contains(ApplicationConstants.FilterDetails_Unallocated) ||
                        filterColumnAccount.FilterConditionList.ConvertAll(obj => obj.ToString()).Contains(ApplicationConstants.FilterDetails_UnallocatedLoadLayout))
                    {
                        filterColumnAccount.FilterConditionList.Clear();
                        FilterCondition filterConditionforGrid = new FilterCondition(FilterComparisionOperator.Equals, "");
                        filterColumnAccount.FilterConditionList.Add(filterConditionforGrid);
                    }

                    PreferenceGridColumn filterColumnMasterFund = consolPreference.SelectedColumnsCollection.Find(obj => obj.Name == OrderFields.PROPERTY_MASTERFUND);

                    if (filterColumnMasterFund != null && filterColumnMasterFund.FilterConditionList != null)
                    {
                        if (filterColumnMasterFund.FilterConditionList.ConvertAll(obj => obj.ToString()).Contains(ApplicationConstants.FilterDetails_DBNullMasterFund) ||
                            filterColumnMasterFund.FilterConditionList.ConvertAll(obj => obj.ToString()).Contains(ApplicationConstants.FilterDetails_UnallocatedLoadLayout))
                        {
                            filterColumnMasterFund.FilterConditionList.Clear();
                            FilterCondition filterConditionforGrid = new FilterCondition(FilterComparisionOperator.Equals, "");
                            filterColumnMasterFund.FilterConditionList.Add(filterConditionforGrid);
                        }
                    }
                    GetUnallocatedFilterDetailsForDashBoard(consolPreference);
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

        // while loading layout check if unallocated filter was applied if yes then replace it with (blanks)
        private static void GetUnallocatedFilterDetailsForDashBoard(CustomViewPreferences consolPreference)
        {
            try
            {
                var filterDetailsForUnallocated = consolPreference.FilterDetails.DynamicFilterConditionList.ConvertAll(obj => obj.ToString());
                if (filterDetailsForUnallocated.Contains(ApplicationConstants.FilterDetails_UnallocatedLoadLayout) ||
                    filterDetailsForUnallocated.Contains(ApplicationConstants.FilterDetails_DBNullString))
                {
                    consolPreference.FilterDetails.DynamicFilterConditionList.Clear();
                    FilterCondition filterCondition = new FilterCondition(FilterComparisionOperator.Equals,
                        ApplicationConstants.FilterDetails_DBNull);
                    consolPreference.FilterDetails.DynamicFilterConditionList.Add(filterCondition);
                    FilterCondition filterConditionSecond = new FilterCondition(FilterComparisionOperator.Equals,
                        ApplicationConstants.FilterDetails_DBNull);
                    consolPreference.FilterDetails.DynamicFilterConditionList.Add(filterConditionSecond);
                    FilterCondition filterConditionThird = new FilterCondition(FilterComparisionOperator.Equals,
                        ApplicationConstants.FilterDetails_DBNull);
                    consolPreference.FilterDetails.DynamicFilterConditionList.Add(filterConditionThird);
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

        private void GetPreferencesWithUnallocatedFilter(CustomViewPreferences consolPreference)
        {
            try
            {
                if (consolPreference != null && consolPreference.FilterDetails != null)
                {
                    var filterDetailsForUnallocated =
                           consolPreference.FilterDetails.DynamicFilterConditionList.ConvertAll(obj => obj.ToString());
                    GetUnallocatedFilterDetailsForDashBoard(consolPreference);
                    if (filterDetailsForUnallocated.Contains(ApplicationConstants.FilterDetails_UnallocatedLoadLayout) ||
                        filterDetailsForUnallocated.Contains(ApplicationConstants.FilterDetails_DBNullString))
                    {
                        PreferenceGridColumn filterColumnAccount = consolPreference.SelectedColumnsCollection.Find(obj => obj.Name == OrderFields.PROPERTY_LEVEL1NAME);
                        filterColumnAccount.FilterConditionList.Clear();
                        FilterCondition filterConditionforGrid = new FilterCondition(FilterComparisionOperator.Equals, "");
                        filterColumnAccount.FilterConditionList.Add(filterConditionforGrid);
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

        private void OnPricingInputClick(object sender, EventArgs e)
        {
            try
            {
                if (PricingInputClick != null)
                {
                    LaunchFormEventArgs ea = new LaunchFormEventArgs(sender.ToString());
                    PricingInputClick(this, ea);
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

        private void OnTradeClick(object sender, EventArgs<OrderSingle, Dictionary<int, double>> e)
        {
            try
            {
                if (TradeClick != null)
                {
                    TradeClick(this, e);
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

        private void PM_closing(FormClosingEventArgs e)
        {
            try
            {
                CustomViewPreferencesList savedList = _prefrenceManager.GetCustomViewPreferences();
                CustomViewPreferencesList currentList = _preferencesList;

                List<string> unsavedTabs = new List<string>();

                if (currentList != null)
                {
                    unsavedTabs.AddRange(currentList.Keys.Where(key => !savedList.ContainsKey(key) && !key.Equals(SUB_MODULE_NAME + "_" + DEFAULT_CUSTOM_VIEW) && !key.Equals(TAB_Main) && !key.Equals(OptionBook_View)));
                }
                if (unsavedTabs.Count > 0)
                {
                    StringBuilder alterMessage = new StringBuilder("The following custom views are not saved:");

                    foreach (string tabName in unsavedTabs)
                    {
                        alterMessage.Append("\n" + tabName.Substring(tabName.LastIndexOf('_') + 1));
                    }
                    alterMessage.Append("\n Do you want to save?");

                    var result = MessageBox.Show(this, alterMessage.ToString(), "Position Management", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        if (_instanceLookup != null)
                        {
                            foreach (KeyValuePair<string, PMUserControl> keyValuePair in _instanceLookup)
                            {
                                if (unsavedTabs.Contains(keyValuePair.Key))
                                {
                                    keyValuePair.Value.SaveLayout(false, keyValuePair.Key);
                                }
                            }
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        ResumeUpdates();
                        e.Cancel = true;
                        return;
                    }
                    _instanceLookup = null;
                    _preferencesList = null;
                }

                savedList = null;
                _preferencesList = null;
                currentList = null;

                if (defaultPmUserControl != null)
                {
                    defaultPmUserControl.SendPMClosingInstruction();
                }

                if (_pmAppearance != null)
                {
                    _pmAppearance.Close();
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
                UIThreadMarshaller.RemoveFormForMarshalling(UIThreadMarshaller.PM_FORM);
            }
        }

        private void PM_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                _exposurePnlCommunicationManager = null;
                UnWireEvents();
                if (_exInstance != null)
                {
                    _exInstance.ClearData();
                }
                _isFormClosed = true;
                if (FormClosedHandler != null)
                {
                    FormClosedHandler(this, EventArgs.Empty);
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

        private void PM_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                PauseUpdates();
                PM_closing(e);
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

        private void PM_Load(object sender, EventArgs e)
        {
            ChangeIconForTheme();
        }

        private void pmAppearance_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_pmAppearance != null)
                {
                    _pmAppearance.PrefsUpdated -= pmAppearance_PrefsUpdated;
                    _pmAppearance.UpdateDashboard -= _pmAppearance_UpdateDashboard;
                    _pmAppearance.FormClosed -= pmAppearance_FormClosed;
                }
                _pmAppearance = null;
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

        private void pmAppearance_PrefsUpdated(object sender, EventArgs<bool, bool> e)
        {
            try
            {
                if (e.Value2 && _prefrenceManager != null)
                {
                    _preferencesList = _prefrenceManager.GetCustomViewPreferences();
                }
                foreach (KeyValuePair<string, PMUserControl> existingControl in _instanceLookup)
                {
                    if (e.Value2)
                    {
                        if (_preferencesList.ContainsKey(existingControl.Key))
                        {
                            existingControl.Value.LoadPreferencesAndColumns(_preferencesList[existingControl.Key]);
                        }
                    }
                    else
                    {
                        existingControl.Value.UpdateGridPreferences(e.Value);
                    }
                }

                if (!PMAppearanceManager.PMAppearance.IsDefaultDashboardFontSize)
                    defaultPmUserControl.SetGridFontSize(PMAppearanceManager.PMAppearance.FontSizeDashboard);

                if (e.Value2)
                {
                    if (_exInstance != null)
                    {
                        _exInstance.PMPrefUpdated(ExPNLPreferenceMsgType.SelectedViewCopied, string.Empty);
                        string selectedTabKey = GetSelectedTabKey();
                        PMPrefUpdated(ExPNLPreferenceMsgType.SelectedViewChanged, selectedTabKey, string.Empty);
                        if (_instanceLookup != null && _instanceLookup.ContainsKey(selectedTabKey))
                        {
                            List<int> currentFilteredAccountList = new List<int>();
                            GetFilteredAccountList(_instanceLookup[selectedTabKey].FilterDetails, ref currentFilteredAccountList);
                            SendFilterChangedDetailsToServer(currentFilteredAccountList, null, selectedTabKey);
                            UpdateDashboard();
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

        private void PMTabView_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            try
            {
                SetStatusMessage();

                string selectedTabKey = string.Empty;
                string previousTabKey = string.Empty;
                if (e.Tab != null)
                {
                    selectedTabKey = GetTabKey(e.Tab.Key, TAB_Main);
                    ExposurePnlCacheManager.GetInstance().SelectedTabKey = selectedTabKey;
                    if (e.PreviousSelectedTab != null)
                    {
                        previousTabKey = GetTabKey(e.PreviousSelectedTab.Key, TAB_Main);
                    }
                }
                if (_exInstance != null && e.Tab != null)
                {
                    PMPrefUpdated(ExPNLPreferenceMsgType.SelectedViewChanged, selectedTabKey, previousTabKey);
                    if (_instanceLookup != null && _instanceLookup.ContainsKey(selectedTabKey))
                    {
                        List<int> currentFilteredAccountList = new List<int>();
                        GetFilteredAccountList(_instanceLookup[selectedTabKey].FilterDetails, ref currentFilteredAccountList);
                        SendFilterChangedDetailsToServer(currentFilteredAccountList, null, selectedTabKey);
                        UpdateDashboard(true);
                    }
                }
                _exportTabName = selectedTabKey.Replace("CustomView_", "");
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

        private void pmUserControl_AddNewConsolidationView(object ControlName, EventArgs e)
        {
            try
            {
                AddCustomView();
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

        private void pmUserControl_FilteredColumnNameToPM(object sender, EventArgs<GridColumnFilterDetails, GridColumnFilterDetails> e)
        {
            try
            {
                List<int> currentFilteredAccountList = new List<int>();
                List<int> prevFilteredAccountList = new List<int>();
                if (e != null)
                {
                    GetFilteredAccountList(e.Value, ref currentFilteredAccountList);
                    GetFilteredAccountList(e.Value2, ref prevFilteredAccountList);
                    SendFilterChangedDetailsToServer(currentFilteredAccountList, prevFilteredAccountList);
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
        /// This Method Return the Account ID Based on the filter details which is applied on Account or MasterFund Column
        /// </summary>
        /// <param name="filterDetails"> contains Name and Details of Account</param>
        /// <param name="filteredAccountList"> ref variable in which all the account ids are returned</param>
        private void GetFilteredAccountList(GridColumnFilterDetails filterDetails, ref List<int> filteredAccountList)
        {
            try
            {
                if (filterDetails != null && filterDetails.FilterColumnKey != null)
                {
                    List<string> accountNames = new List<string>();

                    //if no filter is applied then add all the account ids which are permitted to user also add -1(unallocated Account ID)
                    // if user has permission to all the account
                    if (filterDetails.DynamicFilterConditionList.Count <= 0)
                    {
                        filteredAccountList.AddRange(CachedDataManager.GetInstance.GetAllAccountIDsForUser().Select(Int32.Parse));
                        if (CachedDataManager.GetInstance.GetAllAccountsCount() == CachedDataManager.GetInstance.GetAllAccountIDsForUser().Count)
                        {
                            filteredAccountList.Add(_unAllocatedAccountID);
                        }
                    }
                    //if filter is applied on account column then add all the account ids on which filter is applied
                    else if (filterDetails.FilterColumnKey.Equals(OrderFields.PROPERTY_LEVEL1NAME))
                    {
                        AddUnallocatedAccountId(filterDetails, filteredAccountList);
                        accountNames.AddRange(from object filtercondtion in filterDetails.DynamicFilterConditionList
                                              select Regex.Match(filtercondtion.ToString(), @"'([^']*)")
                                                  into match
                                                  where match.Success
                                                  select match.Groups[1].Value);

                        List<string> permittedUserAccounts = CachedDataManager.GetInstance.GetAllAccountNamesForUser();
                        for (int i = accountNames.Count - 1; i >= 0; i--)
                        {
                            if (!permittedUserAccounts.Contains(accountNames[i]))
                            {
                                accountNames.RemoveAt(i);
                            }
                        }
                        filteredAccountList.AddRange(accountNames.Select(accountname => CachedDataManager.GetInstance.GetAccountID(accountname)));
                    }
                    //if filter is applied on masterfund column then add all the account which are part of that master fund
                    else if (filterDetails.FilterColumnKey.Equals(OrderFields.PROPERTY_MASTERFUND))
                    {
                        AddUnallocatedAccountId(filterDetails, filteredAccountList);

                        List<string> filteredMasterFundList = (filterDetails.DynamicFilterConditionList.Cast<object>()
                                .Select(filtercondtion => Regex.Match(filtercondtion.ToString(), @"'([^']*)"))
                                .Where(match => match.Success)
                                .Select(match => match.Groups[1].Value)).ToList();

                        List<int> masterFundids = filteredMasterFundList.Select(masterFundname => CachedDataManager.GetInstance.GetMasterFundID(masterFundname)).ToList();
                        Dictionary<int, List<int>> masterFundAssociationDictionary = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                        List<int> allAccountList = new List<int>();
                        List<string> permittedUserAccounts = CachedDataManager.GetInstance.GetAllAccountIDsForUser();
                        foreach (KeyValuePair<int, List<int>> masterFundAssociationKvp in masterFundAssociationDictionary.Where(masterFundAssociationKvp => masterFundids.Contains(masterFundAssociationKvp.Key)))
                        {
                            allAccountList.AddRange(masterFundAssociationKvp.Value);
                        }

                        filteredAccountList.AddRange(allAccountList.Where(account => permittedUserAccounts.Contains(account.ToString())));
                    }
                }
                // when filterdetails are null (on first time or when clear filter is clicked) then add all the permitted user account and -1 (unallocated account id)
                else
                {
                    filteredAccountList.AddRange(CachedDataManager.GetInstance.GetAllAccountIDsForUser().Select(Int32.Parse));
                    if (CachedDataManager.GetInstance.GetAllAccountsCount() == CachedDataManager.GetInstance.GetAllAccountIDsForUser().Count)
                    {
                        filteredAccountList.Add(_unAllocatedAccountID);
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
        /// Adds the unallocated account identifier.
        /// </summary>
        /// <param name="filterDetails">The filter details.</param>
        /// <param name="filteredAccountList">The filtered account list.</param>
        private static void AddUnallocatedAccountId(GridColumnFilterDetails filterDetails, List<int> filteredAccountList)
        {
            try
            {
                // check if filter is applied on blanks if yes applied then add -1 to account id list
                if (filterDetails.DynamicFilterConditionList.Any(filterCondition => (filterCondition.ToString() == ApplicationConstants.FilterDetails_DBNullAccount) ||
                                                                                    (filterCondition.ToString() == ApplicationConstants.FilterDetails_DBEmptyAccount) ||
                                                                                    (filterCondition.ToString() == ApplicationConstants.FilterDetails_DBNullMasterFund) ||
                                                                                    (filterCondition.ToString() == ApplicationConstants.FilterDetails_DBEmptyMasterFund)) ||
                    filterDetails.DynamicFilterConditionList.Any(filterCondition => filterCondition.CompareValue != null &&
                                                                                    ((filterCondition.CompareValue.ToString() == ApplicationConstants.FilterDetails_DBNull) ||
                                                                                     (string.IsNullOrWhiteSpace(filterCondition.CompareValue.ToString())))))
                {
                    if (CachedDataManager.GetInstance.GetAllAccountsCount() == CachedDataManager.GetInstance.GetAllAccountIDsForUser().Count)
                    {
                        filteredAccountList.Add(_unAllocatedAccountID);
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
        /// Send the filter details of account column to server 
        /// </summary>
        /// <param name="filteredAccountList"> current filter account list</param>
        /// <param name="previousFilteredAccountList"> previous filter account list</param>
        /// <param name="tabName"> tab on which filter is applied to assign data</param>
        private void SendFilterChangedDetailsToServer(List<int> filteredAccountList, List<int> previousFilteredAccountList, string tabName = "")
        {
            try
            {
                StringBuilder filterAccountlistString = new StringBuilder();
                if (string.IsNullOrEmpty(tabName))
                    tabName = GetSelectedTabKey();

                filterAccountlistString.Append(tabName)
                    .Append(Seperators.SEPERATOR_5)
                    .Append(string.Join(Seperators.SEPERATOR_8, filteredAccountList))
                    .Append(Seperators.SEPERATOR_6)
                    .Append(previousFilteredAccountList == null ? 0.ToString() : string.Join(Seperators.SEPERATOR_8, previousFilteredAccountList));

                if (_exInstance != null)
                    PMPrefUpdated(ExPNLPreferenceMsgType.FilterValueChanged, tabName, filterAccountlistString.ToString());
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

        void pmUserControl_PassTradeClickEvent(object sender, EventArgs<OrderSingle, Dictionary<int, double>> e)
        {
            try
            {
                OnTradeClick(sender, e);
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

        void pmUserControl_AppearanceClickEvent(object sender, EventArgs e)
        {
            try
            {
                AppearanceLoad(sender, e);
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

        private void RenameCustomView(string key, string tabname)
        {
            try
            {
                string newTabName = tabname.Trim();
                DialogResult result = GetTabNameFromInputBox(ref newTabName);
                if (result == DialogResult.Cancel)
                {
                    return;
                }
                if (PMTabView.Tabs.Exists(newTabName))
                {
                    MessageBox.Show("Custom view with the same name already exist. Please choose another name.", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (newTabName != string.Empty && result == DialogResult.OK && !tabname.Trim().Equals(newTabName.Trim()))
                {
                    if (!_preferencesList.ContainsKey(key))
                    {
                        return;
                    }

                    _instanceLookup[key].SaveLayout(false, key);

                    CustomViewPreferences preferences = _preferencesList[key];
                    CustomViewPreferencesList savedList = _prefrenceManager.GetCustomViewPreferences();
                    var newKey = SUB_MODULE_NAME + "_" + newTabName;

                    if (PMTabView != null)
                        PMTabView.SelectedTabChanged -= PMTabView_SelectedTabChanged;
                    DeleteCustomViewTab(key, tabname, PMTabView, key);
                    if (PMTabView != null)
                        PMTabView.SelectedTabChanged += PMTabView_SelectedTabChanged;
                    CreateCustomViewTab(newTabName, preferences, PMTabView, key);

                    if (savedList.ContainsKey(key))
                    {
                        //remove old tab name and add new one
                        savedList.Remove(key);
                        savedList.Add(newKey, preferences);

                        //save back
                        _prefrenceManager.SetPreferences(savedList);
                    }
                    _preferencesList.Add(newKey, preferences);
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

        private void SaveDashboard()
        {
            try
            {
                if (_firstTime == false)
                {
                    _firstTime = true;
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

        private void SavePMTabsOrder()
        {
            try
            {
                string directoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                string filePath = directoryPath + @"\PMTabOrder.xml";
                UltraTabControlHelper.SaveTabOrder(directoryPath, filePath, PMTabView);
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

        private void SetStatusMessage()
        {
            try
            {
                lblStatus.Text = "Updating Data...";
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

        private void SetTabOrder()
        {
            try
            {
                string directoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                string filePath = directoryPath + @"\PMTabOrder.xml";
                UltraTabControlHelper.LoadTabOrder(filePath, PMTabView);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ShowTaxLotsForm(ExposurePnlCacheItemList taxlotlist, string taxlotReqCallerGridName, string CompressedRowID)
        {
            try
            {
                lock (_lockOnDict)
                {
                    PMTaxLotsDisplayForm taxlotDisplayForm;
                    if (!_dictOfCmpressedRowAndDetailForm.ContainsKey(CompressedRowID))
                    {
                        taxlotDisplayForm = new PMTaxLotsDisplayForm();
                        CustomViewPreferences prefs = new CustomViewPreferences();
                        taxlotDisplayForm.FormClosing += taxlotDisplayForm_FormClosing;
                        _dictOfCmpressedRowAndDetailForm.Add(CompressedRowID, taxlotDisplayForm);
                        taxlotDisplayForm.CompressedRowID = CompressedRowID;
                        if (_preferencesList.ContainsKey(taxlotReqCallerGridName))
                        {
                            prefs = _preferencesList[taxlotReqCallerGridName];
                        }
                        else
                        {
                            if (_preferencesList.ContainsKey(DEFAULT_CUSTOM_VIEW))
                            {
                                prefs = _preferencesList[DEFAULT_CUSTOM_VIEW];
                                _preferencesList.Add(taxlotReqCallerGridName, prefs);
                            }
                        }
                        taxlotDisplayForm.SetInputDataSource(taxlotlist, prefs);
                        taxlotDisplayForm.Show();
                        taxlotDisplayForm.BringToFront();
                    }
                    else
                    {
                        CustomViewPreferences prefs = new CustomViewPreferences();
                        taxlotDisplayForm = _dictOfCmpressedRowAndDetailForm[CompressedRowID];
                        taxlotDisplayForm.FormClosing += taxlotDisplayForm_FormClosing;
                        if (_preferencesList.ContainsKey(taxlotReqCallerGridName))
                        {
                            prefs = _preferencesList[taxlotReqCallerGridName];
                        }
                        else
                        {
                            if (_preferencesList.ContainsKey(DEFAULT_CUSTOM_VIEW))
                            {
                                prefs = _preferencesList[DEFAULT_CUSTOM_VIEW];
                                _preferencesList.Add(taxlotReqCallerGridName, prefs);
                            }
                        }
                        taxlotDisplayForm.SetInputDataSource(taxlotlist, prefs);
                        taxlotDisplayForm.Show();
                        taxlotDisplayForm.BringToFront();
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

        private void taxlotDisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                PMTaxLotsDisplayForm temp = sender as PMTaxLotsDisplayForm;
                lock (_lockOnDict)
                {
                    if (_dictOfCmpressedRowAndDetailForm != null)
                    {
                        if (temp != null && _dictOfCmpressedRowAndDetailForm.ContainsKey(temp.CompressedRowID))
                        {
                            _dictOfCmpressedRowAndDetailForm.Remove(temp.CompressedRowID);
                        }
                    }
                }
                if (temp != null) temp.FormClosing -= taxlotDisplayForm_FormClosing;
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

        public void AppearanceLoad(object sender, EventArgs e)
        {
            try
            {
                if (_pmAppearance == null || _pmAppearance.IsDisposed)
                {
                    _pmAppearance = new Appearance { LoginUser = _loginUser };
                    _pmAppearance.PrefsUpdated += pmAppearance_PrefsUpdated;
                    _pmAppearance.UpdateDashboard += _pmAppearance_UpdateDashboard;
                    _pmAppearance.FormClosed += pmAppearance_FormClosed;
                    _pmAppearance.Show();
                }
                _pmAppearance.BringToFront();
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

        private void ultraToolbarsManager1_ToolClick(object sender, ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key.ToUpperInvariant())
                {
                    case "CLOSEPOSITIONS":
                        if (ClosePositionClick != null)
                        {
                            ClosePositionClick(this, EventArgs.Empty);
                        }
                        break;

                    case "MARKPRICE":

                        if (MarkPriceClick != null)
                        {
                            MarkPriceClick(this, EventArgs.Empty);
                        }
                        break;

                    case "BASEEQUITYVALUE":
                        if (_isPermitted)
                        {
                            if (frmEquityBaseValue == null)
                            {
                                frmEquityBaseValue = new BaseEquityValue
                                {
                                    Owner = this,
                                    ShowInTaskbar = false
                                };
                            }
                            frmEquityBaseValue.Show();
                            frmEquityBaseValue.Activate();
                            frmEquityBaseValue.Disposed += frmEquityBaseValue_Disposed;
                        }
                        else
                        {
                            MessageBox.Show("You do not have PM permissions, please contact to Adminstrator.", "Portfolio Management");
                        }
                        break;

                    case "EXIT":
                        Close();
                        break;

                    case "APPEARANCE":
                        if (_pmAppearance == null || _pmAppearance.IsDisposed)
                        {
                            _pmAppearance = new Appearance { LoginUser = _loginUser };
                            _pmAppearance.PrefsUpdated += pmAppearance_PrefsUpdated;
                            _pmAppearance.UpdateDashboard += _pmAppearance_UpdateDashboard;
                            _pmAppearance.FormClosed += pmAppearance_FormClosed;
                            _pmAppearance.Show();
                        }
                        _pmAppearance.BringToFront();
                        break;

                    case "HELP":
                        Help.ShowHelp(this, "Nirvana Help.chm", HelpNavigator.Topic, "ConsolidationView.html");
                        break;

                    case "CORPORATEACTION":
                        if (CorporateActionClick != null)
                        {
                            CorporateActionClick(this, EventArgs.Empty);
                        }
                        break;

                    case "START WRITING DATA":
                        break;

                    case "SCREENSHOT":
                        SnapShotManager.GetInstance().TakeSnapshot(this);
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

        private void UnWireEvents()
        {
            try
            {
                _exInstance.PMDataBinded -= _exposurePnlCacheManagerInstance_PMDataBinded;
                string tabKey = GetSelectedTabKey();
                if (_instanceLookup != null && _instanceLookup.ContainsKey(tabKey))
                {
                    _instanceLookup[tabKey]._accountBindableView.GridData.PMDataBinded -= _exposurePnlCacheManagerInstance_PMDataBinded;
                }
                _exInstance.ExposurePnlCacheSummaryChanged -= _exposurePnlCacheManagerInstance_ExposurePnlCacheSummaryChanged;
                _exInstance.TaxlotsReceived -= _exInstance_TaxlotsReceived;
                _exInstance.UpdateOrderSummaryTable -= _exInstance_UpdateOrderSummaryTable;
                _exInstance.SetPMViewPreferencesList -= _exInstance_SetPMViewPreferencesList;
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

        //This event is used to forcefully refresh the grid because sometimes grid grouping messed up with grid data
        private void _exposurePnlCacheManagerInstance_PMDataBinded(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    string tabKey = GetSelectedTabKey();
                    if (_instanceLookup.ContainsKey(tabKey))
                    {
                        _instanceLookup[tabKey].RefreshGridAfterDataUpdate();
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

        private void UpdateDashboard(bool isTabChangeFromUI = false)
        {
            try
            {
                if (_isFormClosed) return;
                if (PMTabView.ActiveTab == null) return;
                string tabKey = GetSelectedTabKey();
                if (_exInstance == null 
                    || _exInstance.AccountWiseSummary == null 
                    || !_exInstance.AccountWiseSummary.ContainsKey(tabKey)) 
                    return;
                var summary = _exInstance.AccountWiseSummary[tabKey].Copy();
                if (summary != null && summary.Rows.Count > 0)
                {
                    if (!_instanceLookup.ContainsKey(tabKey)) return;
                    _instanceLookup[tabKey].AssignData(summary);
                    List<int> currentFilteredAccountList = new List<int>();
                    GetFilteredAccountList(_instanceLookup[tabKey].FilterDetails, ref currentFilteredAccountList);
                    _exInstance.OverrideGridColumns(GetSelectedTabKey(), currentFilteredAccountList, isTabChangeFromUI);
                    _instanceLookup[tabKey].SummarySettings(ExPNLPreferenceMsgType.FilterValueChanged, tabKey);
                    _instanceLookup[tabKey].GetGroupingForSelectedTab();
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

        private void UpdateUI(bool dataReceivedForFirstTime)
        {
            try
            {
                Debug.WriteLine("UI Thread " + Thread.CurrentThread.ManagedThreadId);
                ClearStatusMessage();
                //Update the dashboard with the corresponding summary
                UpdateDashboard();

                // Refresh Sort the grid if refresh is clicked
                // bool isSecondUpdate = false; // Second update after refresh click.
                //This is used if symbol do not update in the first update due to some other update in pipeline
                if (dataReceivedForFirstTime)
                {
                    ToolBase lblRefreshData = ultraToolbarsManager1.Tools["RefreshData"];
                    lblRefreshData.SharedProps.Enabled = true;
                    lblRefreshData.SharedProps.Caption = "Refresh/Reconnect";

                    lblRefreshData.SharedProps.AppearancesSmall.Appearance.BackColor = Color.Transparent;
                    lblRefreshData.SharedProps.AppearancesSmall.Appearance.FontData.Bold = DefaultableBoolean.False;
                }
                bool isSaveDashboardSplitterPosition = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsSaveDashboardSplitterPosition"));
                if (isSaveDashboardSplitterPosition)
                {
                    SaveDashboard();
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

        private void WireEvents()
        {
            try
            {
                _exInstance.PMDataBinded += _exposurePnlCacheManagerInstance_PMDataBinded;
                string tabKey = GetSelectedTabKey();
                if (_instanceLookup != null && _instanceLookup.ContainsKey(tabKey))
                {
                    _instanceLookup[tabKey]._accountBindableView.GridData.PMDataBinded += _exposurePnlCacheManagerInstance_PMDataBinded;
                }
                _exInstance.ExposurePnlCacheSummaryChanged += _exposurePnlCacheManagerInstance_ExposurePnlCacheSummaryChanged;
                _exInstance.TaxlotsReceived += _exInstance_TaxlotsReceived;
                _exInstance.UpdateOrderSummaryTable += _exInstance_UpdateOrderSummaryTable;
                _exInstance.SetPMViewPreferencesList += _exInstance_SetPMViewPreferencesList;
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

        private void WirePmUserControlEvents(PMUserControl pmUserControl)
        {
            try
            {
                pmUserControl.LanuchPricingInput += OnPricingInputClick;
                pmUserControl.PmGridColPositionChanged += defaultPmUserControl_PmGridColPositionChanged;
                pmUserControl.TaxlotsRequested += defaultPmUserControl_TaxlotsRequested;
                pmUserControl.SaveAllGridLayouts += defaultPmUserControl_SaveAllGridLayouts;
                pmUserControl.AddNewConsolidationView += pmUserControl_AddNewConsolidationView;
                pmUserControl.FilteredColumnNameToPM += pmUserControl_FilteredColumnNameToPM;
                pmUserControl.PassTradeClickEvent += pmUserControl_PassTradeClickEvent;
                pmUserControl.PercentTradingDataToPM += pmUserControl_PercentTradingDataToPM;
                pmUserControl.Appearance_Click += pmUserControl_AppearanceClickEvent;
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
        /// Pass Symbol, OrderSide and List of Accounts To Nirvana Main
        /// </summary>
        /// <param name="sender"> PmuserCOntrol</param>
        /// <param name="e">e.Value = Symbol,e.Value2 = OrderSideTagValue, e.Value3 = List of Accounts </param>
        void pmUserControl_PercentTradingDataToPM(object sender, EventArgs<string, PTTMasterFundOrAccount, List<string>, string> e)
        {
            try
            {
                List<int> filteredAccountList = new List<int>();
                if (e.Value2 == PTTMasterFundOrAccount.Account)
                {
                    filteredAccountList.AddRange(e.Value3.Select(accountname => CachedDataManager.GetInstance.GetAccountID(accountname)));
                }
                else
                {
                    filteredAccountList.AddRange(e.Value3.Select(mfName => CachedDataManager.GetInstance.GetMasterFundID(mfName)));
                }
                if (filteredAccountList.Contains(int.MinValue))
                    filteredAccountList.RemoveAll(item => item == int.MinValue);
                if (PercentTradingToolClick != null)
                    PercentTradingToolClick(this, new EventArgs<string, PTTMasterFundOrAccount, List<int>, string>(e.Value, e.Value2, filteredAccountList, e.Value4));
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                switch (keyData)
                {
                    case (Keys.Alt | Keys.S):
                        ShowHideDashboard(true);
                        return true;
                    case (Keys.Alt | Keys.H):
                        ShowHideDashboard(false);
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
    }
}
