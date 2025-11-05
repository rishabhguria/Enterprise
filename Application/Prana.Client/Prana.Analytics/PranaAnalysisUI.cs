using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

namespace Prana.Analytics
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public partial class PranaAnalysisUI : Form, IPluggableTools, ILiveFeedCallback, IGreekAnalysisCallback
    {
        internal const string StepAnalysisMain_View = "Main";
        int noViewsRefreshed = 0;

        //id is fixed to be of 2 digits
        string _stepAnalViewIDMain = "10";
        bool _IsTabBusy = false;
        int _numberOfStepAnalViews = 1;
        bool _IsPreferencesChanged = false;
        ISecurityMasterServices _secMasterClient = null;
        IPricingAnalysis _pricingAnalysis = null;
        List<string> _listStressTestRequests = new List<string>();
        bool _isLiveFeedConnected = false;
        DuplexProxyBase<IPricingService> _pricingServiceProxy = null;
        private delegate void UIThreadMarshellers(object sender, RunWorkerCompletedEventArgs e);
        BackgroundWorker _bgExportToExcel = null;
        BackgroundWorker _bgExportToCSV = null;
        Dictionary<string, CtrlStepAnalysis> _dictCtrlStepAnalViews;
        bool _isPositionsFetched = false;
        bool _isRefreshMultipleViews = false;
        bool _isMaxViewMessageDisplayed = false;
        private SynchronizationContext _uiSyncContext = null;
        private string tabRiskReport = "Risk Report";
        private string tabRiskASimulation = "Risk Simulation";
        private string tabStressTest = "Stress Test";
        private string tabGreekAnalysis = "Greeks Analysis";
        const string CONST_EXPORT_TO_CSV = "Export to CSV";
        const string CONST_EXPORT = "Export";

        public PranaAnalysisUI()
        {
            try
            {
                InitializeComponent();
                SetExportPermissions();

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region IPluggableTools Members
        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        public ISecurityMasterServices SecurityMaster
        {
            set { _secMasterClient = value; }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set { _pricingAnalysis = value; }
        }

        ProxyBase<IRiskServices> _riskServiceProxy = null;
        private void CreateRiskServiceProxy()
        {
            try
            {
                _riskServiceProxy = new ProxyBase<IRiskServices>("PricingRiskServiceEndpointAddress");
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

        DuplexProxyBase<IGreekAnalysisServices> _greekAnalysisServiceProxy = null;
        private void CreateGreekAnalysisServiceProxy()
        {
            try
            {
                _greekAnalysisServiceProxy = new DuplexProxyBase<IGreekAnalysisServices>("PricingGreekAnalysisServiceEndpointAddress", this);
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

        private void _greekAnalysisService_Connected(object sender, EventArgs e)
        {
            try
            {
                if (_greekAnalysisServiceProxy == null)
                {
                    CreateGreekAnalysisServiceProxy();
                }
                // Capture the UI synchronization context
                _uiSyncContext = SynchronizationContext.Current;
                _greekAnalysisServiceProxy.InnerChannel.SendCallbackDetails();
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

        private void _greekAnalysisService_Disconnected(object sender, EventArgs e)
        {
            try
            {
                if (_greekAnalysisServiceProxy != null)
                {
                    _greekAnalysisServiceProxy.Dispose();
                    _greekAnalysisServiceProxy = null;
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

        public void SetUP()
        {
            try
            {
                _bgExportToExcel = new BackgroundWorker();
                _bgExportToCSV = new BackgroundWorker();
                _dictCtrlStepAnalViews = new Dictionary<string, CtrlStepAnalysis>();
                WireEvents();
                RiskPreferenceManager.SetUp(Application.StartupPath);
                RiskLayoutManager.SetUp(Application.StartupPath);
                Dictionary<string, StepAnalysisPref> stepAnalPreferences = RiskPreferenceManager.RiskPrefernece.StepAnalPreferencesDict;

                _dictCtrlStepAnalViews.Add(StepAnalysisMain_View, ctrlStepAnalysis1);
                CreateGreekAnalysisServiceProxy();
                foreach (KeyValuePair<string, StepAnalysisPref> kp in stepAnalPreferences)
                {
                    StepAnalysisPref pref = kp.Value;
                    string tabname = kp.Key;
                    string tabID = pref.StepAnalViewID;
                    if (tabname != null)
                    {
                        if (!tabname.Equals(StepAnalysisMain_View))
                        {
                            CreateStepAnalysisViewTab(tabname, tabID, tbcMultipleStressTest, false);
                        }
                    }
                }
                SetTabOrder();
                if (_riskServiceProxy != null)
                {
                    _riskServiceProxy.Dispose();
                }
                CreateRiskServiceProxy();
                BindStressTestCombo();
                tbcMultipleStressTest.Tabs[0].Key = _stepAnalViewIDMain;
                //Updating the preferences on server side.
                _riskServiceProxy.InnerChannel.UpdateRiskPreferences(RiskPreferenceManager.RiskPrefernece);
                this.MaximumSize = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                LoadTheme();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Used for changing the color of buttons. The indices and their colors are as follows:
        /// 0 & 3: For the Green Shade
        /// 1 & 4: For the Neutral Shade
        /// 2 & 5: For the Red Shade 
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnRunStressTest.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnRunStressTest.ForeColor = System.Drawing.Color.White;
                btnRunStressTest.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRunStressTest.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRunStressTest.UseAppStyling = false;
                btnRunStressTest.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRefreshSelectedViews.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRefreshSelectedViews.ForeColor = System.Drawing.Color.White;
                btnRefreshSelectedViews.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRefreshSelectedViews.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRefreshSelectedViews.UseAppStyling = false;
                btnRefreshSelectedViews.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void SetTabOrder()
        {
            try
            {
                string directoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                string filePath = directoryPath + @"\RiskTabOrder.xml";
                UltraTabControlHelper.LoadTabOrder(filePath, tbcMultipleStressTest);
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

        private void GreeksCalculated(object sender, EventArgs<string> e)
        {
            try
            {
                if (_listStressTestRequests.Contains(e.Value))
                {
                    _listStressTestRequests.Remove(e.Value);
                }
                if (_listStressTestRequests.Count == 0)
                {
                    EnableForm();
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

        void Server_RefreshCompleted(object sender, EventArgs e)
        {
            try
            {
                noViewsRefreshed++;
                //This code will be run only when risk form is opened.
                if (_isPositionsFetched)
                {
                    if (noViewsRefreshed == _dictCtrlStepAnalViews.Count)
                    {
                        _IsTabBusy = false;
                        EnableForm();
                        noViewsRefreshed = 0;
                        _isPositionsFetched = false;
                    }
                }
                else
                {
                    _IsTabBusy = false;
                    EnableForm();
                    _isPositionsFetched = false;
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

        void _pricingAnalysis_Disconnected(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    EventHandler mi = new EventHandler(_pricingAnalysis_Disconnected);
                    if (InvokeRequired)
                    {
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        EnableForm();
                        UpdateConnectionStatus();
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

        void _pricingAnalysis_Connected(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    EventHandler mi = new EventHandler(_pricingAnalysis_Connected);
                    if (InvokeRequired)
                    {
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        Subscribe();
                        UpdateConnectionStatus();
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

        private void CreatePricingServiceProxy()
        {
            try
            {
                _pricingServiceProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
                CentralRiskPositionsManager.GetInstance.PricingServiceProxy = _pricingServiceProxy;
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

        private void UpdateConnectionStatus()
        {
            try
            {
                foreach (CtrlStepAnalysis control in _dictCtrlStepAnalViews.Values)
                {
                    control.UpdateConnectionStatus(_isLiveFeedConnected);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void Subscribe()
        {
            try
            {
                if (_pricingServiceProxy == null)
                {
                    CreatePricingServiceProxy();
                }

                try
                {
                    _pricingServiceProxy.InnerChannel.Subscribe();
                }
                catch
                {
                    throw new Exception("PricingService2 not connected");
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void Unsubscribe()
        {
            try
            {
                if (_pricingServiceProxy != null)
                {
                    try
                    {
                        _pricingServiceProxy.InnerChannel.UnSubscribe();
                    }
                    catch
                    {
                    }

                    _pricingServiceProxy.Dispose();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public IPostTradeServices PostTradeServices
        {
            set { }
        }
        #endregion

        private void ultraTabControl1_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
        {
            try
            {
                if (_IsTabBusy)
                {
                    e.Cancel = true;
                }
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

        /// <summary>
        /// To disable the  export functionallity.
        /// </summary>
        private void SetExportPermissions()
        {
            try
            {
                if (CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI && CommonDataCache.CachedDataManager.IsMarketDataBlocked)
                {
                    this.ultraToolbarsManager1.Tools[CONST_EXPORT].SharedProps.Enabled = false;
                    this.ultraToolbarsManager1.Tools[CONST_EXPORT_TO_CSV].SharedProps.Enabled = false;
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

        private bool _isFetchedPositionGreeksAnalysis = false;
        private bool _isFetchedPositionForRiskReport = false;
        private bool _isFetchedPositionForRiskSimulation = false;
        private bool _isFetchedPositionForStressTest = false;
        private void ultraTabControl1_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            try
            {
                if (ultraTabControl1.SelectedTab.Key == tabGreekAnalysis)
                {
                    ctrlStepAnalysis1.SetUp(_secMasterClient, _pricingAnalysis, StepAnalysisMain_View, _stepAnalViewIDMain, _greekAnalysisServiceProxy);
                    Subscribe();
                    if (!_isFetchedPositionGreeksAnalysis && !_isPositionsFetched && RiskPreferenceManager.RiskPrefernece.IsAutoLoadDataOnStartup)
                    {
                        this.ultraToolbarsManager1.Tools[0].SharedProps.Enabled = false;
                        btnRefreshSelectedViews.BackColor = Color.Red;
                        btnRefreshSelectedViews.Enabled = false;
                        btnRefreshSelectedViews.Text = "Refreshing...";
                        DisableForm();
                        GetPositions();
                        _isFetchedPositionGreeksAnalysis = true;
                    }
                }
                else if (ultraTabControl1.SelectedTab.Key == tabRiskReport)
                {
                    ctrlRiskReports1.SetUp(!_isFetchedPositionForRiskReport && RiskPreferenceManager.RiskPrefernece.IsAutoLoadDataOnStartup ? true : false, ref _IsTabBusy);
                    if (!_isFetchedPositionForRiskReport && RiskPreferenceManager.RiskPrefernece.IsAutoLoadDataOnStartup)
                    {
                        this.ultraToolbarsManager1.Tools[0].SharedProps.Enabled = false;
                        btnRefreshSelectedViews.BackColor = Color.Red;
                        btnRefreshSelectedViews.Enabled = false;
                        btnRefreshSelectedViews.Text = "Refreshing...";
                        _isFetchedPositionForRiskReport = true;
                    }
                }
                else if (ultraTabControl1.SelectedTab.Key == tabRiskASimulation)
                {
                    ctrlRiskSimulation1.SetUp(_secMasterClient, !_isFetchedPositionForRiskSimulation && RiskPreferenceManager.RiskPrefernece.IsAutoLoadDataOnStartup ? true : false, ref _IsTabBusy);
                    if (!_isFetchedPositionForRiskSimulation && RiskPreferenceManager.RiskPrefernece.IsAutoLoadDataOnStartup)
                    {
                        this.ultraToolbarsManager1.Tools[0].SharedProps.Enabled = false;
                        btnRefreshSelectedViews.BackColor = Color.Red;
                        btnRefreshSelectedViews.Enabled = false;
                        btnRefreshSelectedViews.Text = "Refreshing...";
                        _isFetchedPositionForRiskSimulation = true;
                    }
                }
                else if (ultraTabControl1.SelectedTab.Key == tabStressTest)
                {
                    ctrlStresstest1.SetUp(!_isFetchedPositionForStressTest && RiskPreferenceManager.RiskPrefernece.IsAutoLoadDataOnStartup ? true : false, ref _IsTabBusy);
                    if (!_isFetchedPositionForStressTest && RiskPreferenceManager.RiskPrefernece.IsAutoLoadDataOnStartup)
                    {
                        this.ultraToolbarsManager1.Tools[0].SharedProps.Enabled = false;
                        btnRefreshSelectedViews.BackColor = Color.Red;
                        btnRefreshSelectedViews.Enabled = false;
                        btnRefreshSelectedViews.Text = "Refreshing...";
                        _isFetchedPositionForStressTest = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void PranaAnalysisUI_Load(object sender, System.EventArgs e)
        {
            try
            {
                try
                {
                    if (CustomThemeHelper.ApplyTheme)
                    {
                        this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                        this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                    }
                    LoadTheme();

                    // Capture the UI synchronization context
                    _uiSyncContext = SynchronizationContext.Current;
                    if (_greekAnalysisServiceProxy.InnerChannel != null)
                        _greekAnalysisServiceProxy.InnerChannel.SendCallbackDetails();
                    _greekAnalysisServiceProxy.ConnectedEvent += new DuplexProxyBase<IGreekAnalysisServices>.ConnectionEventHandler(_greekAnalysisService_Connected);
                    _greekAnalysisServiceProxy.DisconnectedEvent += new DuplexProxyBase<IGreekAnalysisServices>.ConnectionEventHandler(_greekAnalysisService_Disconnected);
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

        private void PranaAnalysisUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_IsPreferencesChanged)
                {
                    RiskPreferenceManager.SavePreferences(null);
                }
                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, null);
                }
                DisposeProxies();
                UnwireEvents();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void DisposeProxies()
        {
            try
            {
                if (_pricingServiceProxy != null)
                {
                    _pricingServiceProxy.Dispose();
                    _pricingServiceProxy = null;
                    CentralRiskPositionsManager.GetInstance.PricingServiceProxy = null;
                }
                if (_riskServiceProxy != null)
                {
                    _riskServiceProxy.Dispose();
                }
                if (_greekAnalysisServiceProxy != null && _greekAnalysisServiceProxy.InnerChannel != null)
                {
                    _greekAnalysisServiceProxy.InnerChannel.RemoveCallbackDetails();
                    _greekAnalysisServiceProxy.Dispose();
                }
                ctrlStepAnalysis1.DisposeProxy();
                ctrlRiskReports1.DisposeProxy();
                ctrlRiskSimulation1.DisposeProxy();
                ctrlStresstest1.DisposeProxy();
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
                _bgExportToExcel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgExportToExcel_RunWorkerCompleted);
                _bgExportToExcel.DoWork += new DoWorkEventHandler(_bgExportToExcel_DoWork);
                _bgExportToCSV.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgExportToCSV_RunWorkerCompleted);
                _bgExportToCSV.DoWork += new DoWorkEventHandler(_bgExportToCSV_DoWork);
                ctrlStepAnalysis1.RefreshCompleted += new EventHandler(Server_RefreshCompleted);
                ctrlStepAnalysis1.AddViewClick += new EventHandler(AddNewStepAnalView);
                ctrlStepAnalysis1.SaveLayoutAllClick += new EventHandler(SaveLayoutForAllStepAnalViews);
                ctrlStepAnalysis1.AddSymbolAcrossAllViews += new EventHandler<EventArgs<bool, List<PranaPositionWithGreeks>>>(AddSymbolToAllViews);
                ctrlStepAnalysis1.GreeksCalculated += new EventHandler<EventArgs<string>>(GreeksCalculated);
                ctrlStepAnalysis1.GreeksRequested += new EventHandler<EventArgs<string>>(GreeksRequested);
                ctrlStepAnalysis1.UseVolSkew += new EventHandler<EventArgs<bool>>(ctrlStepAnalysisView_UseVolSkew);
                ctrlStepAnalysis1.RenameViewClick += new EventHandler(RenameStepAnalView);
                ctrlRiskReports1.RefreshCompleted += new EventHandler(Server_RefreshCompleted);
                ctrlRiskSimulation1.RefreshCompleted += new EventHandler(Server_RefreshCompleted);
                ctrlStresstest1.RefreshCompleted += new EventHandler(Server_RefreshCompleted);
                ctrlStresstest1.IncludeCashCheckChanged += new EventHandler(IncludeCashCheckChanged);
                _pricingAnalysis.Connected += new EventHandler(_pricingAnalysis_Connected);
                _pricingAnalysis.Disconnected += new EventHandler(_pricingAnalysis_Disconnected);
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

        void _bgExportToCSV_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Dictionary<string, UltraGrid> gridDict = new Dictionary<string, UltraGrid>();
                Dictionary<string, string> stressTestDetails = new Dictionary<string, string>();
                Dictionary<string, string> lowerGridDetails = new Dictionary<string, string>();
                string[] stringarray = (string[])e.Argument;
                string fileName = stringarray[0];
                string moduleName = stringarray[1];

                if (moduleName == tabGreekAnalysis)
                {
                    foreach (KeyValuePair<string, CtrlStepAnalysis> kvp in _dictCtrlStepAnalViews)
                    {
                        if (kvp.Key.Equals(tbcMultipleStressTest.SelectedTab.Text))
                        {
                            kvp.Value.SetGridExportSettings(gridDict, stressTestDetails);
                        }
                    }
                    foreach (KeyValuePair<string, UltraGrid> ultraGrid in gridDict)
                    {
                        if (ultraGrid.Key.Equals(tbcMultipleStressTest.SelectedTab.Text))
                        {
                            exportToCSV(ultraGrid.Value, stressTestDetails, fileName, false);
                        }
                    }
                }
                else if (moduleName == tabRiskReport)
                {
                    ctrlRiskReports1.SetGridExportSettings(gridDict, lowerGridDetails);
                    foreach (KeyValuePair<string, UltraGrid> ultraGrid in gridDict)
                    {
                        exportToCSV(ultraGrid.Value, lowerGridDetails, fileName, false);
                    }
                }
                else if (moduleName == tabRiskASimulation)
                {
                    ctrlRiskSimulation1.SetGridExportSettings(gridDict, lowerGridDetails);
                    foreach (KeyValuePair<string, UltraGrid> ultraGrid in gridDict)
                    {
                        exportToCSV(ultraGrid.Value, lowerGridDetails, fileName, true);
                    }
                }
                else if (moduleName == tabStressTest)
                {
                    ctrlStresstest1.SetGridExportSettings(gridDict, lowerGridDetails);
                    foreach (KeyValuePair<string, UltraGrid> ultraGrid in gridDict)
                    {
                        exportToCSV(ultraGrid.Value, lowerGridDetails, fileName, false);
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

        void _bgExportToCSV_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.ultraToolbarsManager1.Tools[5].SharedProps.Enabled = true;
                this.ultraToolbarsManager1.Tools[5].SharedProps.Caption = "Export to CSV";
                this.ultraToolbarsManager1.Tools[5].SharedProps.AppearancesSmall.AppearanceOnToolbar.BackColor = System.Drawing.Color.Transparent;
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

        void _bgExportToExcel_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string[] stringarray = (string[])e.Argument;
                string fileName = stringarray[0];
                string moduleName = stringarray[1];

                if (moduleName == tabGreekAnalysis)
                {
                    Dictionary<string, UltraGrid> gridDict = new Dictionary<string, UltraGrid>();
                    Dictionary<string, string> stressTestDetails = new Dictionary<string, string>();

                    foreach (KeyValuePair<string, CtrlStepAnalysis> kvp in _dictCtrlStepAnalViews)
                    {
                        kvp.Value.SetGridExportSettings(gridDict, stressTestDetails);
                    }
                    _excelUtil.OnExportToExcel(gridDict, stressTestDetails, false);
                }
                else if (moduleName == tabRiskReport)
                {
                    ExportSelectedCtrl(fileName, tabRiskReport);
                }
                else if (moduleName == tabRiskASimulation)
                {
                    ExportSelectedCtrl(fileName, tabRiskASimulation);
                }
                else if (moduleName == tabStressTest)
                {
                    ExportSelectedCtrl(fileName, tabStressTest);
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

        void _bgExportToExcel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.ultraToolbarsManager1.Tools[1].SharedProps.Enabled = true;
                this.ultraToolbarsManager1.Tools[1].SharedProps.Caption = "Export";
                this.ultraToolbarsManager1.Tools[1].SharedProps.AppearancesSmall.AppearanceOnToolbar.BackColor = System.Drawing.Color.Transparent;
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

        private void UnwireEvents()
        {
            try
            {
                //Close open child UI on closing of parent form
                if (_symbolMappingUI != null && !_symbolMappingUI.IsDisposed)
                {
                    _symbolMappingUI.Close();
                }
                if (_pricingAnalysis != null)
                {
                    _pricingAnalysis.Disconnected -= new EventHandler(_pricingAnalysis_Disconnected);
                    _pricingAnalysis.Connected -= new EventHandler(_pricingAnalysis_Connected);
                }
                foreach (CtrlStepAnalysis control in _dictCtrlStepAnalViews.Values)
                {
                    control.RefreshCompleted -= new EventHandler(Server_RefreshCompleted);
                    control.AddViewClick -= new EventHandler(AddNewStepAnalView);
                    control.AddSymbolAcrossAllViews -= new EventHandler<EventArgs<bool, List<PranaPositionWithGreeks>>>(AddSymbolToAllViews);
                    control.SaveLayoutAllClick -= new EventHandler(SaveLayoutForAllStepAnalViews);
                    control.GreeksCalculated -= new EventHandler<EventArgs<string>>(GreeksCalculated);
                    control.GreeksRequested -= new EventHandler<EventArgs<string>>(GreeksRequested);
                    control.UseVolSkew -= new EventHandler<EventArgs<bool>>(ctrlStepAnalysisView_UseVolSkew);
                }
                ctrlRiskReports1.RefreshCompleted -= new EventHandler(Server_RefreshCompleted);
                ctrlRiskSimulation1.RefreshCompleted -= new EventHandler(Server_RefreshCompleted);
                ctrlStresstest1.RefreshCompleted -= new EventHandler(Server_RefreshCompleted);

                if (riskPrefUI != null)
                {
                    riskPrefUI.PrefsUpdated -= new EventHandler(riskPrefUI_PrefsUpdated);
                }
                if (_bgExportToExcel != null)
                {
                    _bgExportToExcel.DoWork -= new DoWorkEventHandler(_bgExportToExcel_DoWork);
                    _bgExportToExcel.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_bgExportToExcel_RunWorkerCompleted);
                }
                if (_bgExportToCSV != null)
                {
                    _bgExportToCSV.DoWork -= new DoWorkEventHandler(_bgExportToCSV_DoWork);
                    _bgExportToCSV.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_bgExportToCSV_RunWorkerCompleted);
                }
                if (_greekAnalysisServiceProxy != null)
                {
                    _greekAnalysisServiceProxy.ConnectedEvent -= new DuplexProxyBase<IGreekAnalysisServices>.ConnectionEventHandler(_greekAnalysisService_Connected);
                    _greekAnalysisServiceProxy.DisconnectedEvent -= new DuplexProxyBase<IGreekAnalysisServices>.ConnectionEventHandler(_greekAnalysisService_Disconnected);
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

        RiskPrefsUI riskPrefUI;
        private void btnPrefs_Click(object sender, EventArgs e)
        {
            try
            {
                riskPrefUI = new RiskPrefsUI();
                riskPrefUI.StartPosition = FormStartPosition.CenterParent;
                riskPrefUI.PrefsUpdated += new EventHandler(riskPrefUI_PrefsUpdated);
                riskPrefUI.ShowDialog();
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

        void riskPrefUI_PrefsUpdated(object sender, EventArgs e)
        {
            try
            {
                _riskServiceProxy.InnerChannel.UpdateRiskPreferences(RiskPreferenceManager.RiskPrefernece);
                LoadTheme();
                ctrlStepAnalysis1.UpdateGridAsPref();
                ctrlRiskReports1.UpdateGridAsPref();
                ctrlRiskSimulation1.UpdateGridAsPref();
                ctrlStresstest1.UpdateGridAsPref();
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

        private void LoadTheme()
        {
            try
            {
                if (!RiskPreferenceManager.RiskPrefernece.WrapHeader)
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
                }
                else
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT_WRAP_HEADER);
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

        ExcelAndPrintUtilities _excelUtil = new ExcelAndPrintUtilities();
        public void IncludeCashCheckChanged(object sender, EventArgs e)
        {
            try
            {
                btnRefresh_Click(sender, e);
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                //Disable the form till the refresh tab completes.
                noViewsRefreshed = 0;
                _IsTabBusy = true;
                this.ultraToolbarsManager1.Tools[0].SharedProps.AppearancesSmall.AppearanceOnToolbar.BackColor = Color.Red;
                this.ultraToolbarsManager1.Tools[0].SharedProps.Enabled = false;
                btnRefreshSelectedViews.Enabled = false;
                btnRefreshSelectedViews.BackColor = Color.Red;
                DisableForm();
                if (ultraTabControl1.SelectedTab.Key == tabGreekAnalysis)
                {
                    string TabName = tbcMultipleStressTest.SelectedTab.Text;
                    if (_dictCtrlStepAnalViews.ContainsKey(TabName))
                    {
                        _dictCtrlStepAnalViews[TabName].RefreshPositions();
                    }
                }
                else if (ultraTabControl1.SelectedTab.Key == tabRiskReport)
                {
                    ctrlRiskReports1.RefreshPositions();
                }
                else if (ultraTabControl1.SelectedTab.Key == tabRiskASimulation)
                {
                    ctrlRiskSimulation1.RefreshPositions();
                }
                else if (ultraTabControl1.SelectedTab.Key == tabStressTest)
                {
                    ctrlStresstest1.RefreshPositions();
                }
                else
                {
                    _IsTabBusy = false;
                    this.ultraToolbarsManager1.Tools[0].SharedProps.AppearancesSmall.AppearanceOnToolbar.BackColor = Color.Red;
                    this.ultraToolbarsManager1.Tools[0].SharedProps.Enabled = true;
                    btnRefreshSelectedViews.Enabled = true;
                    btnRefreshSelectedViews.BackColor = Color.FromArgb(55, 67, 85);
                    btnRunStressTest.Enabled = false;
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

        private void GetPositions()
        {
            try
            {
                noViewsRefreshed = 0;
                this.ultraToolbarsManager1.Tools[0].SharedProps.Enabled = false;
                _IsTabBusy = true;
                foreach (CtrlStepAnalysis control in _dictCtrlStepAnalViews.Values)
                {
                    control.UpdateStatus(false, true);
                }
                GetPositionsforRisk();
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

        public async void GetPositionsforRisk()
        {
            try
            {
                PranaPositionWithGreekColl positions = await CentralRiskPositionsManager.GetInstance.GetPositionsAsRiskPref();
                if (!this.IsDisposed)
                {
                    _IsTabBusy = false;
                    _isPositionsFetched = true;

                    foreach (KeyValuePair<string, CtrlStepAnalysis> control in _dictCtrlStepAnalViews)
                    {
                        if (_isRefreshMultipleViews == true)
                        {
                            if (_selectedViews.Count > 0)
                            {
                                if (_selectedViews.Contains(control.Key))
                                {
                                    control.Value.GetInstance_PositionReceived(positions);
                                }
                            }
                        }
                        else
                        {
                            control.Value.GetInstance_PositionReceived(positions);
                        }
                    }
                    EnableForm();
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

        private void DisableForm()
        {
            try
            {
                btnRunStressTest.Enabled = false;
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

        private void EnableForm()
        {
            try
            {
                if (ControlBox == false)
                {
                    ControlBox = true;
                }
                btnRunStressTest.BackColor = Color.FromArgb(104, 156, 46);
                btnRunStressTest.Enabled = true;
                btnRunStressTest.Text = "Run";
                this.ultraToolbarsManager1.Tools[0].SharedProps.AppearancesSmall.AppearanceOnToolbar.BackColor = Color.Transparent;
                this.ultraToolbarsManager1.Tools[0].SharedProps.Enabled = true;
                btnRefreshSelectedViews.BackColor = Color.FromArgb(55, 67, 85);
                btnRefreshSelectedViews.Enabled = true;
                btnRefreshSelectedViews.Text = "Refresh";
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

        private string GenerateViewID()
        {
            string id = _stepAnalViewIDMain;
            try
            {
                List<string> listIDs = new List<string>();
                Dictionary<string, StepAnalysisPref> stepAnalPreferences = RiskPreferenceManager.RiskPrefernece.StepAnalPreferencesDict;
                foreach (StepAnalysisPref pref in stepAnalPreferences.Values)
                {
                    if (!listIDs.Contains(pref.StepAnalViewID))
                    {
                        listIDs.Add(pref.StepAnalViewID);
                    }
                }
                int i = int.Parse(_stepAnalViewIDMain);
                for (; ; i++)
                {
                    id = i.ToString();
                    if (!listIDs.Contains(id))
                    {
                        return id;
                    }
                }
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
            return id;
        }

        private void AddStepAnalysisView()
        {
            try
            {
                bool tabAdded = false;
                string tabName = string.Empty;
                string tabID = GenerateViewID();
                DialogResult result = GetTabNameFromInputBox(ref tabName);

                if (tabName != string.Empty && result == DialogResult.OK)
                {
                    foreach (UltraTab tab in tbcMultipleStressTest.Tabs)
                    {
                        if (tab.Text.Equals(tabName.Trim()))
                        {
                            MessageBox.Show("Stress Test view with the same name already exist. Please choose another name.", "Greek Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    tabAdded = CreateStepAnalysisViewTab(tabName, tabID, tbcMultipleStressTest, true);
                    if (tabAdded)
                    {
                        BindStressTestCombo();
                    }
                    LoadTheme();
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

        private void BindStressTestCombo()
        {
            try
            {
                checkedMultipleItems.Items.Clear();
                checkedMultipleItems.Items.Add("All", true);
                checkedMultipleItems.Items.Add("Main", true);
                foreach (string tabName in _dictCtrlStepAnalViews.Keys)
                {
                    if (tabName != "Main")
                    {
                        checkedMultipleItems.Items.Add(tabName, true);
                    }
                }
                SetItemCheckStates();
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

        private void SetItemCheckStates()
        {
            try
            {
                foreach (string tabName in _dictCtrlStepAnalViews.Keys)
                {
                    StepAnalLayout stepAnalysisLayout = RiskLayoutManager.RiskLayout.GetStepAnalLayout(tabName);
                    int itemIndex = checkedMultipleItems.Items.IndexOf(tabName);
                    if (itemIndex != -1)
                    {
                        checkedMultipleItems.SetItemChecked(itemIndex, stepAnalysisLayout.IsCheckedinStressTestCombo);
                    }
                }
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

        private int volSkewCount = 0;
        private bool CreateStepAnalysisViewTab(string tabName, string tabID, UltraTabControl tbcStepAnalysisViewRef, bool isNewView)
        {
            try
            {
                RiskPrefernece riskPrefs = RiskPreferenceManager.RiskPrefernece;
                if (!isNewView)
                {
                    if (riskPrefs.StepAnalPreferencesDict[tabName].UseVolSkew == true)
                    {
                        volSkewCount++;
                    }
                }
                UltraTab tab = null;
                if (!tbcStepAnalysisViewRef.Tabs.Exists(tabID))
                {
                    int noOfViews = _numberOfStepAnalViews;
                    noOfViews += 1;
                    int maxViews = 0;
                    //Maximum Stress test views allowed without vol skew fetched from preferences.
                    maxViews = riskPrefs.MaxStressTestViewsWithoutVolSkew;

                    if (noOfViews > maxViews)
                    {
                        if (!_isMaxViewMessageDisplayed || isNewView)
                        {
                            MessageBox.Show("Max number of allowed StressTest views reached", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _isMaxViewMessageDisplayed = true;
                        }
                        return false;
                    }

                    tab = tbcStepAnalysisViewRef.Tabs.Add(tabID);
                    tab.Text = tabName;
                    tab.Tag = int.MinValue;
                    CtrlStepAnalysis ctrlStepAnalysisView = new CtrlStepAnalysis();
                    ctrlStepAnalysisView.Size = ctrlStepAnalysis1.Size;
                    ctrlStepAnalysisView.VerticalScroll.Visible = true;
                    ctrlStepAnalysisView.SetUp(_secMasterClient, _pricingAnalysis, tabName, tabID, _greekAnalysisServiceProxy);

                    ctrlStepAnalysisView.Dock = DockStyle.Fill;
                    tab.TabPage.Controls.Add(ctrlStepAnalysisView);
                    ctrlStepAnalysisView.DeleteViewClick += new EventHandler(OnDeleteViewClick);
                    ctrlStepAnalysisView.RefreshCompleted += new EventHandler(Server_RefreshCompleted);
                    ctrlStepAnalysisView.AddViewClick += new EventHandler(AddNewStepAnalView);
                    ctrlStepAnalysisView.RenameViewClick += new EventHandler(RenameStepAnalView);
                    ctrlStepAnalysisView.SaveLayoutAllClick += new EventHandler(SaveLayoutForAllStepAnalViews);
                    ctrlStepAnalysisView.AddSymbolAcrossAllViews += new EventHandler<EventArgs<bool, List<PranaPositionWithGreeks>>>(AddSymbolToAllViews);
                    ctrlStepAnalysisView.GreeksCalculated += new EventHandler<EventArgs<string>>(GreeksCalculated);
                    ctrlStepAnalysisView.GreeksRequested += new EventHandler<EventArgs<string>>(GreeksRequested);
                    ctrlStepAnalysisView.UseVolSkew += new EventHandler<EventArgs<bool>>(ctrlStepAnalysisView_UseVolSkew);
                    if (!_dictCtrlStepAnalViews.ContainsKey(tabName))
                    {
                        _dictCtrlStepAnalViews.Add(tabName, ctrlStepAnalysisView);
                    }
                    if (isNewView)
                    {
                        this.ultraToolbarsManager1.Tools[0].SharedProps.AppearancesSmall.AppearanceOnToolbar.BackColor = Color.Red;
                        this.ultraToolbarsManager1.Tools[0].SharedProps.Enabled = false;
                        btnRefreshSelectedViews.Enabled = false;
                        btnRefreshSelectedViews.BackColor = Color.Red;
                        tbcMultipleStressTest.SelectedTab = tab;

                        ctrlStepAnalysisView.RefreshPositions();
                    }
                    ctrlStepAnalysisView.UpdateConnectionStatus(_isLiveFeedConnected);
                    _IsPreferencesChanged = true;
                    _numberOfStepAnalViews += 1;

                    //Disables the Use Vol Skew Check Box in case the number of views exceeds the maximum number of Views With Vol Skew.
                    if (volSkewCount >= riskPrefs.MaxStressTestViewsWithVolSkew)
                    {
                        if (isNewView)
                        {
                            ctrlStepAnalysisView.SetUseVolSkewState(false);
                        }
                    }
                    if (isNewView)
                    {
                        ctrlStepAnalysisView.SaveLayout();
                    }
                    LoadTheme();
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
            return true;
        }

        void ctrlStepAnalysisView_UseVolSkew(object sender, EventArgs<bool> e)
        {
            try
            {
                RiskPrefernece riskPref = RiskPreferenceManager.RiskPrefernece;

                //If the use vol skew check box has been checked then this code will execute
                if (e.Value)
                {
                    if (volSkewCount < riskPref.MaxStressTestViewsWithVolSkew)
                    {
                        volSkewCount++;
                    }
                    //If the maximum limit for views with vol skew is reached, then disable the use vol skew check box.
                    if (volSkewCount == riskPref.MaxStressTestViewsWithVolSkew)
                    {
                        foreach (CtrlStepAnalysis ctrlStepAnal in _dictCtrlStepAnalViews.Values)
                        {
                            ctrlStepAnal.UseVolSkewLimitReached = true;
                            ctrlStepAnal.SetUseVolSkewState(false);
                        }
                    }
                }
                //If the use vol skew check box has been unchecked then this code will execute
                else
                {
                    volSkewCount--;
                    foreach (CtrlStepAnalysis ctrlStepAnal in _dictCtrlStepAnalViews.Values)
                    {
                        ctrlStepAnal.UseVolSkewLimitReached = false;
                        ctrlStepAnal.SetUseVolSkewState(true);
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

        void RenameStepAnalView(object sender, EventArgs e)
        {
            try
            {
                CtrlStepAnalysis ctrlStepAnalView = sender as CtrlStepAnalysis;
                if (ctrlStepAnalView == null)
                {
                    return;
                }
                // fetch name of the selected tab
                string oldName = tbcMultipleStressTest.SelectedTab.Text;
                string NewName = string.Empty;
                //get new name for tab
                DialogResult result = GetTabNameFromInputBox(ref NewName);
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                // check already exitance from dictionary
                if (_dictCtrlStepAnalViews.ContainsKey(NewName))
                {
                    MessageBox.Show("Tab Name Already Exist", "Risk Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (NewName != string.Empty && result == DialogResult.OK)
                {
                    if (!_dictCtrlStepAnalViews.ContainsKey(NewName))
                    {
                        CtrlStepAnalysis ctrlStepAnalysisView = _dictCtrlStepAnalViews[oldName];
                        _dictCtrlStepAnalViews.Remove(oldName);
                        //assign newname to object
                        ctrlStepAnalysisView.ViewName = NewName;
                        //add name and id in dictionary
                        _dictCtrlStepAnalViews.Add(NewName, ctrlStepAnalysisView);
                        // show new name on tab
                        tbcMultipleStressTest.SelectedTab.Text = NewName;
                        // create object of stepanalysispref to get or set pref
                        StepAnalysisPref prefs = RiskPreferenceManager.RiskPrefernece.GetStepAnalViewPreferences(oldName);
                        // get the name of current layout

                        StepAnalLayout layout = RiskLayoutManager.RiskLayout.GetStepAnalLayout(oldName);
                        //remove pref regarding old name
                        RiskPreferenceManager.RiskPrefernece.StepAnalPreferencesDict.Remove(oldName);
                        //remove the old name of currentlayout
                        RiskLayoutManager.RiskLayout.StepAnalysisColumnsList.Remove(oldName);

                        //set the pref for new name
                        prefs.StepAnalViewName = NewName;
                        //update the pref for new name
                        RiskPreferenceManager.RiskPrefernece.UpdateStepAnalPrefDict(NewName, prefs);
                        // add name for the layout
                        RiskLayoutManager.RiskLayout.StepAnalysisColumnsList.Add(NewName, layout);
                        // pref mode is set to true;
                        _IsPreferencesChanged = true;
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

        void GreeksRequested(object sender, EventArgs<string> e)
        {
            try
            {
                if (!_listStressTestRequests.Contains(e.Value))
                {
                    _listStressTestRequests.Add(e.Value);
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

        void OnDeleteViewClick(object sender, EventArgs e)
        {
            try
            {
                CtrlStepAnalysis ctrlStepAnalView = sender as CtrlStepAnalysis;
                if (ctrlStepAnalView == null)
                {
                    return;
                }
                string tabName = ctrlStepAnalView.ViewName;
                string tabKey = ctrlStepAnalView.ViewID;
                DialogResult result = MessageBox.Show(this, "Delete View " + tabName + "! \n Are you sure?", "Greek Analysis Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result != DialogResult.Yes)
                {
                    return;
                }
                DeleteStepAnalViewTab(tabName, tabKey, tbcMultipleStressTest);
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

        void AddNewStepAnalView(object sender, EventArgs e)
        {
            try
            {
                CtrlStepAnalysis ctrlStepAnalView = sender as CtrlStepAnalysis;
                if (ctrlStepAnalView != null)
                {
                    AddStepAnalysisView();
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

        void SaveLayoutForAllStepAnalViews(object sender, EventArgs e)
        {
            try
            {
                foreach (KeyValuePair<string, CtrlStepAnalysis> kvp in _dictCtrlStepAnalViews)
                {
                    if (checkedMultipleItems.Items.Contains(kvp.Value.ViewName))
                    {
                        int itemIndex = checkedMultipleItems.Items.IndexOf(kvp.Value.ViewName);
                        RiskLayoutManager.RiskLayout.GetStepAnalLayout(kvp.Key).IsCheckedinStressTestCombo = checkedMultipleItems.GetItemChecked(itemIndex);
                    }
                    kvp.Value.SaveLayout();
                }
                string directoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString().ToString();
                string filePath = directoryPath + @"\RiskTabOrder.xml";
                UltraTabControlHelper.SaveTabOrder(directoryPath, filePath, tbcMultipleStressTest);
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

        void AddSymbolToAllViews(object sender, EventArgs<bool, List<PranaPositionWithGreeks>> e)
        {
            try
            {
                if (e.Value2.Count > 0 && e.Value2 != null)
                {
                    if (!e.Value)
                    {
                        foreach (PranaPositionWithGreeks pranaPos in e.Value2)
                        {
                            ctrlStepAnalysis1.AddSymbol(pranaPos);
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, CtrlStepAnalysis> kvp in _dictCtrlStepAnalViews)
                        {
                            foreach (PranaPositionWithGreeks pranaPos in e.Value2)
                            {
                                kvp.Value.AddSymbol(pranaPos);
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

        private void DeleteStepAnalViewTab(string tabName, string tabID, UltraTabControl tbcStepAnalViewRef)
        {
            try
            {
                ///Get the instance for lookup from sender(sender contains tabname)  
                if (!_dictCtrlStepAnalViews.ContainsKey(tabName))
                {
                    return;
                }
                if (!tbcStepAnalViewRef.Tabs.Exists(tabID))
                {
                    return;
                }
                CtrlStepAnalysis control = _dictCtrlStepAnalViews[tabName];
                control.UnwireEvents();

                int tabindex = tbcStepAnalViewRef.Tabs[tabID].Index;
                if (tabindex > 0)
                {
                    tbcStepAnalViewRef.Tabs[tabindex - 1].Selected = true;
                }
                tbcStepAnalViewRef.Tabs.RemoveAt(tabindex);
                _dictCtrlStepAnalViews.Remove(tabName);
                _numberOfStepAnalViews -= 1;
                control.DeleteViewClick -= new EventHandler(OnDeleteViewClick);
                control.AddViewClick -= new EventHandler(AddNewStepAnalView);
                control.SaveLayoutAllClick -= new EventHandler(SaveLayoutForAllStepAnalViews);
                control.AddSymbolAcrossAllViews -= new EventHandler<EventArgs<bool, List<PranaPositionWithGreeks>>>(AddSymbolToAllViews);
                control.RefreshCompleted -= new EventHandler(Server_RefreshCompleted);
                //removing the view from preference instance
                RiskPreferenceManager.RiskPrefernece.StepAnalPreferencesDict.Remove(tabName);
                RiskLayoutManager.RiskLayout.StepAnalysisColumnsList.Remove(tabName);

                BindStressTestCombo();
                _IsPreferencesChanged = true;
                control = null;
                if (_dictCtrlStepAnalViews.Count <= RiskPreferenceManager.RiskPrefernece.MaxStressTestViewsWithVolSkew)
                {
                    foreach (CtrlStepAnalysis ctrlStepAnalysis1 in _dictCtrlStepAnalViews.Values)
                    {
                        ctrlStepAnalysis1.SetUseVolSkewState(true);
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

        private DialogResult GetTabNameFromInputBox(ref string tabName)
        {
            DialogResult result = DialogResult.None;
            try
            {
                tabName = Prana.Utilities.UI.UIUtilities.InputBox.ShowInputBox("Stress Test View", tabName, out result).Trim();
                // Don't do anything if user has hit cancel or closed the dialog from left top "X"
                if (result == DialogResult.Cancel)
                    return result;
                if (!Prana.Utilities.UI.MiscUtilities.GeneralUtilities.CheckNameValidation(tabName))
                {
                    MessageBox.Show(this, "Invalid view name.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tabName = string.Empty;
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
            return result;
        }

        private CtrlStepAnalysis GetControlFromHashCode(int hashCode)
        {
            try
            {
                foreach (CtrlStepAnalysis control in _dictCtrlStepAnalViews.Values)
                {
                    if (control.GetHashCode() == hashCode)
                    {
                        return control;
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
            return null;
        }

        private void checkedMultipleItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                //select deselect all the views on the basis of select all checkbox
                if (e.Index == 0)
                {
                    this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                    for (int i = 1; i < checkedMultipleItems.Items.Count; i++)
                    {
                        checkedMultipleItems.SetItemCheckState(i, e.NewValue);
                    }
                    this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                }
                else if (e.NewValue == CheckState.Checked)
                {
                    this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                    if (checkedMultipleItems.CheckedItems.Count == checkedMultipleItems.Items.Count - 2)
                    {
                        checkedMultipleItems.SetItemCheckState(0, e.NewValue);
                    }
                    this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                }
                else if (e.NewValue == CheckState.Unchecked)
                {
                    this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                    if (checkedMultipleItems.CheckedItems.Count == checkedMultipleItems.Items.Count)
                    {
                        checkedMultipleItems.SetItemCheckState(0, e.NewValue);
                    }
                    this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
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

        private List<string> _selectedViews = null;
        private void btnRefreshSelectedItem_Click(object sender, EventArgs e)
        {
            try
            {
                _isRefreshMultipleViews = true;
                noViewsRefreshed = 0;
                _IsTabBusy = true;

                _selectedViews = new List<string>();
                for (int i = 0; i < checkedMultipleItems.CheckedItems.Count; i++)
                {
                    _selectedViews.Add(checkedMultipleItems.CheckedItems[i].ToString());
                }
                if (_selectedViews.Count > 0)
                {
                    foreach (string tabName in _dictCtrlStepAnalViews.Keys)
                    {
                        if (_selectedViews.Contains(tabName))
                        {
                            _dictCtrlStepAnalViews[tabName].UpdateStatus(false, true);
                        }
                    }
                    _IsTabBusy = true;
                    this.ultraToolbarsManager1.Tools[0].SharedProps.Enabled = false;
                    btnRefreshSelectedViews.BackColor = Color.Red;
                    btnRefreshSelectedViews.Enabled = false;
                    btnRefreshSelectedViews.Text = "Refreshing...";
                    GetPositionsforRisk();
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

        private void btnRunStressTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (MarketDataValidation.CheckMarketDataPermissioning())
                {
                    btnRunStressTest.BackColor = Color.Red;
                    btnRunStressTest.Text = "Simulating...";
                    DisableForm();
                    this.ControlBox = false;

                    _listStressTestRequests.Clear();
                    int userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                    InputParametersCollection inputParameters = new InputParametersCollection();
                    inputParameters.UserID = userID.ToString();
                    _selectedViews = new List<string>();
                    for (int i = 0; i < checkedMultipleItems.CheckedItems.Count; i++)
                    {
                        _selectedViews.Add(checkedMultipleItems.CheckedItems[i].ToString());
                    }
                    if (_selectedViews.Count > 0)
                    {
                        foreach (string stressTestViewName in _selectedViews)
                        {
                            if (_dictCtrlStepAnalViews.ContainsKey(stressTestViewName))
                            {
                                CtrlStepAnalysis ctrlStepAnal = _dictCtrlStepAnalViews[stressTestViewName];
                                if (ctrlStepAnal.ValidateRequest())
                                {
                                    ctrlStepAnal.UpdateStatus(true, false);
                                    inputParameters = ctrlStepAnal.GetInputParametersForSimulation(inputParameters);
                                    if (inputParameters.ListUniqueSymbols.Count > 0)
                                    {
                                        ctrlStepAnal.UpdateStatus(true, false);
                                        _listStressTestRequests.Add(stressTestViewName);
                                    }
                                }
                            }
                        }
                        if (inputParameters.ListUniqueSymbols.Count > 0)
                        {
                            SendDataForSnapshot(inputParameters, userID);
                        }
                        else
                        {
                            EnableForm();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No View Selected", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        EnableForm();
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

        #region ILiveFeedCallback Members
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            // throw new Exception("The method or operation is not implemented.");
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = new MethodInvoker(LiveFeedConnected);
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        _isLiveFeedConnected = true;
                        UpdateConnectionStatus();
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

        public void LiveFeedDisConnected()
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = new MethodInvoker(LiveFeedDisConnected);
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        _isLiveFeedConnected = false;
                        EnableForm();
                        UpdateConnectionStatus();
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
        #endregion

        private void btnScreenshot_Click(object sender, EventArgs e)
        {
            try
            {
                SnapShotManager.GetInstance().TakeSnapshot(this);
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

        SymbolMappingUI _symbolMappingUI = null;
        private void btnSymbolMapping_Click(object sender, EventArgs e)
        {
            try
            {
                if (_symbolMappingUI == null)
                {
                    _symbolMappingUI = new SymbolMappingUI();
                    _symbolMappingUI.Show();
                    _symbolMappingUI.FormClosed += new FormClosedEventHandler(_symbolMappingUI_FormClosed);
                }
                _symbolMappingUI.BringToFront();
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

        void _symbolMappingUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                _symbolMappingUI.FormClosed -= new FormClosedEventHandler(_symbolMappingUI_FormClosed);
                _symbolMappingUI = null;
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

        private async void SendDataForSnapshot(InputParametersCollection inputParameters, int userID)
        {
            try
            {
                inputParameters.UserID = userID.ToString();
                await System.Threading.Tasks.Task.Run(() => { _greekAnalysisServiceProxy.InnerChannel.RequestSnapshotData(inputParameters); });
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

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "Refresh Positions":
                        //Disable the form till the refresh tab completes.
                        noViewsRefreshed = 0;
                        _IsTabBusy = true;
                        this.ultraToolbarsManager1.Tools[0].SharedProps.AppearancesSmall.AppearanceOnToolbar.BackColor = Color.Red;
                        this.ultraToolbarsManager1.Tools[0].SharedProps.Enabled = false;
                        btnRefreshSelectedViews.Enabled = false;
                        btnRefreshSelectedViews.BackColor = Color.Red;
                        DisableForm();
                        if (ultraTabControl1.SelectedTab.Key == tabGreekAnalysis)
                        {
                            foreach (var tabName in _dictCtrlStepAnalViews)
                            {
                                if (_dictCtrlStepAnalViews.ContainsKey(tabName.Key))
                                {
                                    _dictCtrlStepAnalViews[tabName.Key].RefreshPositions();
                                }
                            }
                        }
                        else if (ultraTabControl1.SelectedTab.Key == tabRiskReport)
                        {
                            ctrlRiskReports1.RefreshPositions();
                        }
                        else if (ultraTabControl1.SelectedTab.Key == tabRiskASimulation)
                        {
                            ctrlRiskSimulation1.RefreshPositions();
                        }
                        else if (ultraTabControl1.SelectedTab.Key == tabStressTest)
                        {
                            ctrlStresstest1.RefreshPositions();
                        }
                        else
                        {
                            _IsTabBusy = false;
                            this.ultraToolbarsManager1.Tools[0].SharedProps.AppearancesSmall.AppearanceOnToolbar.BackColor = Color.Transparent;
                            this.ultraToolbarsManager1.Tools[0].SharedProps.Enabled = true;
                            btnRefreshSelectedViews.Enabled = true;
                            btnRefreshSelectedViews.BackColor = Color.FromArgb(55, 67, 85);
                            btnRunStressTest.Enabled = false;
                        }
                        break;

                    case "Export":
                        string filename = CreateFileName();
                        _excelUtil.SetFilePath(filename);

                        this.ultraToolbarsManager1.Tools[1].SharedProps.AppearancesSmall.AppearanceOnToolbar.BackColor = Color.Red;
                        this.ultraToolbarsManager1.Tools[1].SharedProps.Enabled = false;
                        this.ultraToolbarsManager1.Tools[1].SharedProps.Caption = "Exporting...";

                        if (ultraTabControl1.SelectedTab.Key == tabGreekAnalysis)
                        {
                            string TabName = tbcMultipleStressTest.SelectedTab.Text;
                            if (_dictCtrlStepAnalViews.ContainsKey(TabName))
                            {
                                if (!_bgExportToExcel.IsBusy)
                                {
                                    _bgExportToExcel.RunWorkerAsync(new string[] { filename, tabGreekAnalysis });
                                }
                            }
                        }
                        else if (ultraTabControl1.SelectedTab.Key == tabRiskReport)
                        {
                            if (!_bgExportToExcel.IsBusy)
                            {
                                _bgExportToExcel.RunWorkerAsync(new string[] { filename, tabRiskReport });
                            }
                        }
                        else if (ultraTabControl1.SelectedTab.Key == tabRiskASimulation)
                        {
                            if (!_bgExportToExcel.IsBusy)
                            {
                                _bgExportToExcel.RunWorkerAsync(new string[] { filename, tabRiskASimulation });
                            }
                        }
                        else if (ultraTabControl1.SelectedTab.Key == tabStressTest)
                        {
                            if (!_bgExportToExcel.IsBusy)
                            {
                                _bgExportToExcel.RunWorkerAsync(new string[] { filename, tabStressTest });
                            }
                        }
                        break;

                    case "Preferences":
                        riskPrefUI = new RiskPrefsUI();
                        riskPrefUI.StartPosition = FormStartPosition.CenterParent;
                        riskPrefUI.PrefsUpdated += new EventHandler(riskPrefUI_PrefsUpdated);
                        riskPrefUI.ShowDialog();
                        break;

                    case "Symbol Mapping":
                        if (_symbolMappingUI == null)
                        {
                            _symbolMappingUI = new SymbolMappingUI();
                            _symbolMappingUI.Show();
                            _symbolMappingUI.FormClosed += new FormClosedEventHandler(_symbolMappingUI_FormClosed);
                        }
                        _symbolMappingUI.BringToFront();
                        break;

                    case "Screenshot":
                        SnapShotManager.GetInstance().TakeSnapshot(this);
                        break;

                    case "Export to CSV":
                        Dictionary<string, UltraGrid> gridDict = new Dictionary<string, UltraGrid>();
                        Dictionary<string, string> stressTestDetails = new Dictionary<string, string>();
                        Dictionary<string, string> lowerGridDetails = new Dictionary<string, string>();
                        string fileName = OpenSaveDialogBox();

                        this.ultraToolbarsManager1.Tools[5].SharedProps.AppearancesSmall.AppearanceOnToolbar.BackColor = Color.Red;
                        this.ultraToolbarsManager1.Tools[5].SharedProps.Enabled = false;
                        this.ultraToolbarsManager1.Tools[5].SharedProps.Caption = "Exporting...";

                        if (ultraTabControl1.SelectedTab.Key == tabGreekAnalysis)
                        {
                            string TabName = tbcMultipleStressTest.SelectedTab.Text;
                            if (_dictCtrlStepAnalViews.ContainsKey(TabName))
                            {
                                if (!_bgExportToCSV.IsBusy)
                                {
                                    _bgExportToCSV.RunWorkerAsync(new string[] { fileName, tabGreekAnalysis });
                                }
                            }
                        }
                        else if (ultraTabControl1.SelectedTab.Key == tabRiskReport)
                        {
                            if (!_bgExportToCSV.IsBusy)
                            {
                                _bgExportToCSV.RunWorkerAsync(new string[] { fileName, tabRiskReport });
                            }
                        }
                        else if (ultraTabControl1.SelectedTab.Key == tabRiskASimulation)
                        {
                            if (!_bgExportToCSV.IsBusy)
                            {
                                _bgExportToCSV.RunWorkerAsync(new string[] { fileName, tabRiskASimulation });
                            }
                        }
                        else if (ultraTabControl1.SelectedTab.Key == tabStressTest)
                        {
                            if (!_bgExportToCSV.IsBusy)
                            {
                                _bgExportToCSV.RunWorkerAsync(new string[] { fileName, tabStressTest });
                            }
                        }
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

        private void exportToCSV(UltraGrid grdDict, Dictionary<string, string> headerDetails, string fileName, bool isTwoRowsinHeader = false)
        {
            try
            {
                if (!String.IsNullOrEmpty(fileName))
                {
                    StreamWriter sw = File.CreateText(fileName);
                    string s = string.Empty;
                    String groupByColCaption = string.Empty;
                    // First fo all write all the visible columns header
                    UltraGridBand band = grdDict.DisplayLayout.Bands[0];
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
                        foreach (UltraGridRow row in grdDict.Rows)
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
                        UltraGridRow[] filterednonGropuedRows = grdDict.Rows.GetFilteredInNonGroupByRows();
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

        private void ExportSelectedCtrl(string fileName, string tabName)
        {
            try
            {
                Dictionary<string, UltraGrid> gridDict = new Dictionary<string, UltraGrid>();
                Dictionary<string, string> lowerGridDetails = new Dictionary<string, string>();
                this.ultraToolbarsManager1.Tools[1].SharedProps.AppearancesSmall.AppearanceOnToolbar.BackColor = Color.Red;
                this.ultraToolbarsManager1.Tools[1].SharedProps.Enabled = false;
                this.ultraToolbarsManager1.Tools[1].SharedProps.Caption = "Exporting...";
                bool isTwoRowsinHeader = false;
                switch (tabName)
                {
                    case "Risk Report":
                        ctrlRiskReports1.SetGridExportSettings(gridDict, lowerGridDetails);
                        isTwoRowsinHeader = false;
                        break;

                    case "Risk Simulation":
                        ctrlRiskSimulation1.SetGridExportSettings(gridDict, lowerGridDetails);
                        isTwoRowsinHeader = true;
                        break;

                    case "Stress Test":
                        ctrlStresstest1.SetGridExportSettings(gridDict, lowerGridDetails);
                        isTwoRowsinHeader = false;
                        break;
                }
                _excelUtil.OnExportToExcel(gridDict, lowerGridDetails, isTwoRowsinHeader);

                this.ultraToolbarsManager1.Tools[1].SharedProps.Enabled = true;
                this.ultraToolbarsManager1.Tools[1].SharedProps.Caption = "Export";
                this.ultraToolbarsManager1.Tools[1].SharedProps.AppearancesSmall.AppearanceOnToolbar.BackColor = System.Drawing.Color.Transparent;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private string CreateFileName()
        {
            String filename = String.Empty;
            try
            {
                if (!RiskPreferenceManager.RiskPrefernece.UseExportFileFormat)
                {
                    return filename;
                }
                else
                {
                    filename = RiskPreferenceManager.RiskPrefernece.RiskExportFileName;
                    if (filename.Contains("#Date#") && !String.IsNullOrEmpty(RiskPreferenceManager.RiskPrefernece.RiskExportDateFormat))
                    {
                        filename = filename.Replace("#Date#", DateTime.Now.ToString(RiskPreferenceManager.RiskPrefernece.RiskExportDateFormat));
                    }
                    else
                    {
                        filename = filename.Replace("#Date#", String.Empty);
                    }
                    if (filename.Contains("#clientname#"))
                    {
                        filename = filename.Replace("#clientname#", CachedDataManager.GetInstance.LoggedInUser.CompanyName);
                    }
                    if (filename.Contains("#tabname#"))
                    {
                        filename = filename.Replace("#tabname#", ultraTabControl1.SelectedTab.Key.Replace(" ", String.Empty));
                    }
                    if (filename.Contains("#BenchmarkMove#") && ultraTabControl1.SelectedTab.Key.Equals(tabStressTest))
                    {
                        filename = filename.Replace("#BenchmarkMove#", ctrlStresstest1.GetBenchmarkValue());
                    }
                    else if (filename.Contains("#BenchmarkMove#"))
                    {
                        if (filename.StartsWith("#BenchmarkMove#"))
                        {
                            filename = filename.Replace("#BenchmarkMove#.", String.Empty);
                        }
                        else
                        {
                            filename = filename.Replace(".#BenchmarkMove#", String.Empty);
                        }
                    }
                }
                return filename;
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
            return filename;
        }

        private void ProcessSnapshotDataOnNewThread(Object state)
        {
            try
            {
                List<object> dataList = state as List<object>;
                foreach (object data in dataList)
                {
                    if (data != null)
                    {
                        List<object> responseList = (List<object>)data;
                        if (responseList != null && CachedDataManager.GetInstance.LoggedInUser.CompanyUserID == Convert.ToInt32(responseList[1].ToString()))
                        {
                            ResponseObj responseObj = (ResponseObj)responseList[0];
                            int hashCode = Convert.ToInt32(responseList[2].ToString());
                            CtrlStepAnalysis ctrlStepAnalysisControl = GetControlFromHashCode(hashCode);
                            if (ctrlStepAnalysisControl != null)
                            {
                                ctrlStepAnalysisControl.ProcessPublishedSnapShotData(responseObj);
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

        private void ProcessStepAnalysisDataOnNewThread(Object state)
        {
            try
            {
                List<object> dataList = state as List<object>;
                foreach (object data in dataList)
                {
                    if (data != null)
                    {
                        List<object> staepAnalResponseList = (List<object>)data;
                        if (staepAnalResponseList != null && CachedDataManager.GetInstance.LoggedInUser.CompanyUserID == Convert.ToInt32(staepAnalResponseList[1].ToString()))
                        {
                            List<StepAnalysisResponse> stepRes = (List<StepAnalysisResponse>)staepAnalResponseList[0];
                            int hashCode = Convert.ToInt32(staepAnalResponseList[2].ToString());
                            CtrlStepAnalysis ctrlStepAnalysisControl = GetControlFromHashCode(hashCode);
                            if (ctrlStepAnalysisControl != null)
                            {
                                ctrlStepAnalysisControl.ProcessPublishedStepAnalysisData(stepRes);
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

        #region IGreekAnalysisCallbackMethods
        public void ProcessSnapshotData(object state)
        {
            // The UI thread won't be handling the callback, but it is the only one allowed to update the controls.  
            // So, we will dispatch the UI update back to the UI sync context.
            SendOrPostCallback callback = new SendOrPostCallback(ProcessSnapshotDataOnNewThread);
            _uiSyncContext.Post(callback, state);
        }

        public void ProcessStepAnalysisData(object state)
        {
            // The UI thread won't be handling the callback, but it is the only one allowed to update the controls.  
            // So, we will dispatch the UI update back to the UI sync context.
            SendOrPostCallback callback = new SendOrPostCallback(ProcessStepAnalysisDataOnNewThread);
            _uiSyncContext.Post(callback, state);
        }
        #endregion


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
                    {
                        components.Dispose();
                    }
                    if (_bgExportToExcel != null)
                    {
                        _bgExportToExcel.Dispose();
                    }
                    if (_bgExportToCSV != null)
                    {
                        _bgExportToCSV.Dispose();
                    }
                    if (riskPrefUI != null)
                    {
                        riskPrefUI.Dispose();
                    }
                    if (_excelUtil != null)
                    {
                        _excelUtil.Dispose();
                    }
                    if (_symbolMappingUI != null)
                    {
                        _symbolMappingUI.Dispose();
                    }
                    if (_pricingServiceProxy != null)
                    {
                        _pricingServiceProxy.Dispose();
                        _pricingServiceProxy = null;
                        CentralRiskPositionsManager.GetInstance.PricingServiceProxy = null;
                    }

                    if (_riskServiceProxy != null)
                        _riskServiceProxy.Dispose();

                    if (_greekAnalysisServiceProxy != null)
                        _greekAnalysisServiceProxy.Dispose();

                    RiskPreferenceManager.CleanUp();
                    RiskLayoutManager.Dispose();

                    if (_dictCtrlStepAnalViews != null)
                    {
                        _dictCtrlStepAnalViews.Clear();
                        _dictCtrlStepAnalViews = null;
                    }
                    if (ctrlStepAnalysis1 != null)
                        ctrlStepAnalysis1 = null;
                    if (ctrlStresstest1 != null)
                        ctrlStresstest1 = null;
                    if (ctrlRiskSimulation1 != null)
                        ctrlRiskSimulation1 = null;
                    if (ctrlRiskReports1 != null)
                        ctrlRiskReports1 = null;
                }
                base.Dispose(isDisposing);
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

    }
}