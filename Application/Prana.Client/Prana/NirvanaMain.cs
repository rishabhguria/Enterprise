#region Using directives
using Castle.Windsor;
using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinToolbars;
using Prana.Admin.BLL;
using Prana.Allocation.Client.Forms.Views;
using Prana.Auth.Authorization.BLL;
using Prana.Authentication.Common;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.EventArguments;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.DropCopyClient;
using Prana.ExposurePnlCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Import;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.NirvanaQualityChecker;
using Prana.PostTradeServices;
using Prana.Preferences;
using Prana.Rebalancer;
using Prana.Rebalancer.PercentTradingTool.ViewModel;
using Prana.Rebalancer.RebalancerNew.Views;
using Prana.ReconciliationNew;
using Prana.SecurityMasterNew.BLL;
using Prana.ServiceConnector;
using Prana.SocketCommunication;
using Prana.TradeManager;
using Prana.TradeManager.Extension;
using Prana.UIEventAggregator;
using Prana.UIEventAggregator.Events;
using Prana.User.View;
using Prana.User.ViewModel;
using Prana.Utilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Prana.LiveFeed.UI;
using Prana.LiveFeed.UI.Forms;
using System.ServiceModel;
using ExportGridsData;
using ExportDataServiceBehaviour;
#endregion

namespace Prana
{
    /// <summary>
    /// Its a main MDI form of the Prana. 
    /// This will act as placeholder for different forms in application.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PranaMain : System.Windows.Forms.Form, IJob, IEventAggregatorSubscriber<PTTTradeClicked>, IEventAggregatorSubscriber<PTTComplianceAlerts>, IEventAggregatorSubscriber<PTTSymbolLookUpClicked>, IEventAggregatorSubscriber<ClosePTTUI>, IEventAggregatorSubscriber<PTTPreferenceClicked>, IEventAggregatorSubscriber<OpenMultiTTFromPTT>, IMarketDataPermissionServiceCallback, IClientConnectivityServiceCallback
    {
        #region scheduler variables
        static ISchedulerFactory _schedFact;
        static IScheduler _sched;

        #endregion
        #region Private and protected declarations
        private System.Timers.Timer accountLockTimer = new System.Timers.Timer();
        private bool _isPMAlreadyStarted = false;

        //To store the compliance tabs which are permitted to user.
        private Dictionary<string, bool> dictComplianceTabs = new Dictionary<string, bool>();

        private Dictionary<string, string> PlugInMenuItemsPairs = null;

        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ImageList PranaIconList;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager PranaMainToolBar;
        private delegate void SetDisplayCallback(ConnectionProperties con);
        private delegate void SetDisplayWithoutArgsCallback();
        private delegate void AlgoCallback(object sender, EventArgs<OrderSingle> e);

        #endregion
        private SynchronizationContext _uiSyncContext = null;

        /// <summary>
        /// For Connecting to Server
        /// </summary>
        ICommunicationManager _tradeCommManager;
        /// <summary>
        /// For Connecting to Exposure PNL Services
        /// </summary>
        ICommunicationManager _exPNLCommManagerInstance;
        /// <summary>
        /// For Connecting Security Master
        /// </summary>
        /// 
        ISecurityMasterServices _secMasterServices = null;

        EventArgs _arguments = null;

        private Infragistics.Win.UltraWinToolbars.ButtonTool[] quickTTButtons = new Infragistics.Win.UltraWinToolbars.ButtonTool[10];

        Infragistics.Win.UltraWinToolbars.PopupMenuTool reports2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Reports");

        List<string> OpenModulesList = new List<string>();

        //This variable used for counting total number of module instances are connected to EXPNL Server (i.e 1 PM & 3 TTs then variable value is 4)
        private int _exPNLConnectedModuleInstances = 0;

        public ISecurityMasterServices SecurityMasterClient
        {
            set { _secMasterServices = value; }
        }

        IPostTradeServices _postTradeServicesInstance;
        IPricingAnalysis _pricingAnalysisInstance;

        private DuplexProxyBase<IMarketDataPermissionService> _marketDataPermissionServiceProxy = null;

        /// <summary>
        /// DuplexProxyBase for ClientConnectivityService.
        /// </summary>
        private DuplexProxyBase<IClientConnectivityService> _clientConnectivityService = null;


        #region Required variables
        Form blotterForm = null;
        Form allocationForm = null;
        Form reportForm = null;
        Form tradeAuditUIForm = null;
        Form portfolioManagementForm = null;
        Form thirdPartyReportFrom = null;
        Form positionManagementDailySheetForm = null;
        Form positionManagementMonthlySheetForm = null;
        QualityCheck QualityCheckForm = null;
        private bool _isBlotterFormLaunching = false;

        private Prana.AboutPrana frmPranaHelp = null;
        private Prana.HelpAndSupport frmHelpAndSupport = null;
        private NirvanaDisclaimerUI frmDisclaimer = null;
        private IBlotterReports blotterReportsInstance = null;

        private PranaShortcuts frmPranaShortcuts = null;
        private CompanyUser loginUser;

        public event EventHandler<EventArgs<string, IPreferenceData>> ApplyPreferences;

        private delegate void BringToFrontHandler(Form form, Point formNewLocation);
        private delegate void TradingTicketLaunchHandler(OrderSingle or);
        private delegate void MultiTradingTicketLaunchHandler(OrderBindingList orList, Point formLocation, TradingTicketParent tradingTicketParent, bool isedit = false);
        private delegate Point GetFormLocationInvoker(Form form);
        private delegate void MethodInvoker(Form form);
        private delegate void ResizeFormHandler(FormWindowState windowState);
        private delegate void LaunchPreferencesHandler(string moduleName, Point formLocation);
        private delegate void AddLayoutHanlder(DataTable dt, Form form);
        private delegate void SetLayoutHandler(DataRow layoutRow, FormWindowState windowState, Form currentForm);

        public static Thread ThreadLANDetector = null;
        private UltraLabel lblConnectionStatus;

        private delegate void MarshalEventDelegate();
        private PreferencesMain preferencesMain = null;

        //Bharat Kumar Jangir (27 January 2014)
        //Permitted Modules cache moved to ModuleManager class
        // added By Sandeep as on 26-Dec-2007 to get all the User Permitted Modules
        //Modules _companyModulesPermittedToUser = null;
        #endregion

        private UltraLabel lblExPNLConnection;

        bool _loadApplicationFirstTime = false;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManagerForPricing;
        private Infragistics.Win.Misc.UltraLabel lblPricingServer;
        private Infragistics.Win.Misc.UltraButton btnLiveFeedConnect;
        string AMSConnectionString = ConfigurationHelper.Instance.GetAppSettingValueByKey("AMSConnectionString");
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Top;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PranaMain_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PranaMain_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PranaMain_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PranaMain_UltraFormManager_Dock_Area_Bottom;
        private ImageList PranaIconListThemesOff;
        private Infragistics.Win.AppStyling.Runtime.AppStylistRuntime appStylistRuntime1;
        FormBrokerConnection formBrokerConnection = new FormBrokerConnection();
        private string _loggedInUserIPAddress = string.Empty;
        private string _argsIPAddress = string.Empty;
        private int _marketDataPermissionTimeoutInterval = int.Parse(ConfigurationManager.AppSettings["MarketDataPermissionTimeoutInterval"]);
        private int _marketDataPermissionRecheckInterval = int.Parse(ConfigurationManager.AppSettings["MarketDataPermissionRecheckInterval"]);
        private int _maximumOrdersAllowedinMTT = int.Parse(ConfigurationManager.AppSettings["MaximumOrdersAllowedinMTT"]);
        private int _marketDataPermissionRemainingTimeoutInterval;
        private System.Timers.Timer _marketDataPermissionDeniedTimer;
        private System.Timers.Timer _marketDataPermissionRecheckTimer;
        private CustomMessageBox _marketDataPermissionDeniedAlert;
        private object _isMarketDataPermissionEnabledLock = new object();

        public PranaMain()
        {
        }

        ICommunicationManager _connectionPricingServer = null;
        bool _pricingServerConnectionStatus = false;
        private ConnectionProperties GetPricingConnectionProperties()
        {
            ConnectionProperties connProperties = new ConnectionProperties();
            try
            {
                CompanyUser user = CachedDataManager.GetInstance.LoggedInUser;
                if (user == null)
                {
                    user = new CompanyUser();
                    user.CompanyUserID = -1;
                    user.FirstName = "Client";
                }
                connProperties.Port = ClientAppConfiguration.PricingServer.Port;
                connProperties.ServerIPAddress = ClientAppConfiguration.PricingServer.IpAddress;
                connProperties.IdentifierID = "OptionAnal" + user.CompanyUserID.ToString();
                connProperties.IdentifierName = "Option Analytics" + user.FirstName.ToString();
                connProperties.ConnectedServerName = "Option Analytics Server";
                connProperties.HandlerType = HandlerType.OptionCalcStatic;
                connProperties.User = user;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return connProperties;
        }

        void _PricingServerConnected(object sender, EventArgs e)
        {
            _pricingServerConnectionStatus = true;
            DisplayPricingServerStatus(true);

            GetMarketDataPermissions();
        }

        void _PricingServerDisconnected(object sender, EventArgs e)
        {
            _pricingServerConnectionStatus = false;
            DisplayPricingServerStatus(false);
        }

        private void CreateMarketDataPermissionServiceProxy()
        {
            try
            {
                _marketDataPermissionServiceProxy = new DuplexProxyBase<IMarketDataPermissionService>("PricingMarketDataPermissionServiceEndpointAddress", this);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates the ClientConnectivityServiceProxy.
        /// </summary>
        private void CreateClientConnectivityServiceProxy()
        {
            try
            {
                _clientConnectivityService = new DuplexProxyBase<IClientConnectivityService>(EndPointAddressConstants.CONST_TradeClientConnectivityServiceEndpoint, this);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }


        public void PermissionCheckResponse(int companyUserID, bool isPermitted)
        {
            try
            {
                lock (_isMarketDataPermissionEnabledLock)
                {
                    if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet
                        || CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI)
                    {
                        if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && !isPermitted)
                        {
                            StartPermissionDeniedTimer();
                            if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet && CachedDataManager.CompanyFactSetContractType == FactSetContractType.Reseller)
                            {
                                if (this.btnLiveFeedConnect.InvokeRequired)
                                {
                                    this.btnLiveFeedConnect.Invoke(new Action(() =>
                                    {
                                        // Update your UI elements here
                                        this.btnLiveFeedConnect.Text = "Connect Live Feed";
                                        this.btnLiveFeedConnect.Enabled = true;
                                    }));
                                }
                                else
                                {
                                    this.btnLiveFeedConnect.Text = "Connect Live Feed";
                                    this.btnLiveFeedConnect.Enabled = true;
                                }
                            }
                        }
                        else if (!CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && isPermitted)
                        {
                            StopPermissionDeniedTimer();
                            ShowConnectionEstablishmentMessage();
                            if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet && CachedDataManager.CompanyFactSetContractType == FactSetContractType.Reseller)
                            {
                                if (this.btnLiveFeedConnect.InvokeRequired)
                                {
                                    this.btnLiveFeedConnect.Invoke(new Action(() =>
                                    {
                                        // Update your UI elements here
                                        this.btnLiveFeedConnect.Text = "LiveFeed Connected";
                                        this.btnLiveFeedConnect.Enabled = false;
                                    }));
                                }
                                else
                                {
                                    this.btnLiveFeedConnect.Text = "LiveFeed Connected";
                                    this.btnLiveFeedConnect.Enabled = false;
                                }
                            }
                        }
                    }

                    CachedDataManager.GetInstance.IsMarketDataPermissionEnabled = isPermitted;
                    if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet
                        || CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI)
                    {
                        ResetPermissionRecheckTimer();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                    throw;
            }
        }

        private void StartPermissionDeniedTimer()
        {
            try
            {
                _marketDataPermissionRemainingTimeoutInterval = _marketDataPermissionTimeoutInterval;

                if (_marketDataPermissionDeniedTimer == null || !_marketDataPermissionDeniedTimer.Enabled)
                {
                    string messageHeader = ClientLevelConstants.HEADER_MARKET_DATA_ALERT;
                    string messageText = string.Format(ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE_MODULE_CLOSE, Prana.Utilities.MiscUtilities.TimeFormatter.Format(_marketDataPermissionRemainingTimeoutInterval));

                    this.Invoke(new Action(() =>
                    {
                        _marketDataPermissionDeniedAlert = new CustomMessageBox(messageHeader, messageText, false, string.Empty, FormStartPosition.CenterScreen);
                        _marketDataPermissionDeniedAlert.Show();
                    }));

                    _marketDataPermissionDeniedTimer = new System.Timers.Timer(1000);
                    _marketDataPermissionDeniedTimer.Elapsed += MarketDataPermissionDeniedTimer_Elapsed;
                    _marketDataPermissionDeniedTimer.Start();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                    throw;
            }
        }

        private void StopPermissionDeniedTimer()
        {
            try
            {
                if (_marketDataPermissionDeniedTimer != null && _marketDataPermissionDeniedTimer.Enabled)
                {
                    _marketDataPermissionDeniedTimer.Stop();

                    if (_marketDataPermissionDeniedAlert != null && !_marketDataPermissionDeniedAlert.IsDisposed)
                        this.Invoke(new Action(() => _marketDataPermissionDeniedAlert.Close()));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                    throw;
            }
        }

        private void ShowConnectionEstablishmentMessage()
        {
            try
            {
                if (_marketDataPermissionRecheckTimer != null)
                {
                    string messageHeader = ClientLevelConstants.HEADER_MARKET_DATA_ALERT;
                    string messageText = ClientLevelConstants.MSG_MARKET_DATA_AVAILABLE;

                    this.Invoke(new Action(() => new CustomMessageBox(messageHeader, messageText, false, string.Empty, FormStartPosition.CenterScreen).Show()));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                    throw;
            }
        }

        private void ResetPermissionRecheckTimer()
        {
            try
            {
                if (_marketDataPermissionRecheckTimer == null)
                {
                    _marketDataPermissionRecheckTimer = new System.Timers.Timer();
                    _marketDataPermissionRecheckTimer.Elapsed += RecheckMarketDataPermission_Elapsed;
                    _marketDataPermissionRecheckTimer.Interval = _marketDataPermissionRecheckInterval * 1000;
                }
                else
                {
                    _marketDataPermissionRecheckTimer.Stop();
                }

                _marketDataPermissionRecheckTimer.Start();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                    throw;
            }
        }

        private void MarketDataPermissionDeniedTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (--_marketDataPermissionRemainingTimeoutInterval == 0)
                {
                    (sender as System.Timers.Timer).Stop();

                    this.Invoke(new Action(() =>
                        {
                            if (_marketDataPermissionDeniedAlert != null && !_marketDataPermissionDeniedAlert.IsDisposed)
                                this.Invoke(new Action(() => _marketDataPermissionDeniedAlert.Close()));

                            if (percentTradingToolWindow != null)
                            {
                                if (OpenModulesList.Contains(PranaModules.PERCENT_TRADING_TOOL))
                                    OpenModulesList.Remove(PranaModules.PERCENT_TRADING_TOOL);
                                percentTradingToolWindow.Close();
                            }
                            if (frmRebalancer != null)
                            {
                                if (OpenModulesList.Contains(PranaModules.REBALANCER_MODULE))
                                    OpenModulesList.Remove(PranaModules.REBALANCER_MODULE);
                                ((Window)frmRebalancer).Close();
                            }
                            if (_optionChain != null)
                            {
                                if (OpenModulesList.Contains(PranaModules.OPTIONCHAIN_MODULE))
                                    OpenModulesList.Remove(PranaModules.OPTIONCHAIN_MODULE);
                                _optionChain.Close();
                            }
                            if (_watchList != null)
                            {
                                if (OpenModulesList.Contains(PranaModules.WATCHLIST_MODULE))
                                    OpenModulesList.Remove(PranaModules.WATCHLIST_MODULE);
                                _watchList.Close();
                            }
                            lock (activePluggableTools)
                            {
                                if (activePluggableTools.ContainsKey("OptionModelInputs"))
                                    ((Form)activePluggableTools["OptionModelInputs"]).Close();
                                if (activePluggableTools.ContainsKey("ComplianceEngine"))
                                    ((Form)activePluggableTools["ComplianceEngine"]).Close();
                            }
                        }));
                }
                else
                {
                    string messageText = string.Format(ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE_MODULE_CLOSE, Prana.Utilities.MiscUtilities.TimeFormatter.Format(_marketDataPermissionRemainingTimeoutInterval)); ;

                    this.Invoke(new Action(() => _marketDataPermissionDeniedAlert.SetMessageBoxText(messageText)));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                    throw;
            }
        }

        private void RecheckMarketDataPermission_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if ((CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet && CachedDataManager.CompanyFactSetContractType == FactSetContractType.ChannelPartner)
                    || (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI))
                {
                    _marketDataPermissionRecheckTimer.Stop();
                    GetMarketDataPermissions();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                    throw;
            }
        }

        public void LoadNirvanaForm()
        {
            try
            {
                this.Shown += new EventHandler(PranaMain_Shown);
                _loadApplicationFirstTime = true;

                CreateMarketDataPermissionServiceProxy();
                CreateClientConnectivityServiceProxy();

                Prana.Fix.FixDictionary.FixDictionaryHelper.LoadFixDictionary();
                Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
                this.HandleCreated += new EventHandler(PranaMain_HandleCreated);
                InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(OnInformationReceived);
                InitializeComponent();
                LoadApplicationModuleDetails();
                _pricingAnalysisInstance = new PricingSeverClient();

                // Launch Login Form
                LaunchLoginForm();

                _uiSyncContext = SynchronizationContext.Current;
                //If form closed without entering the user information then exit application.
                if (loginUser == null)
                {
                    this.ExitApplication();
                }

                // Layout For Main Form
                int left = 0;
                int top = 0;
                DataTable dtMainFormLayout = LayoutManager.GetUserMainFormLayout(loginUser.CompanyUserID);
                if (dtMainFormLayout.Rows.Count > 0)
                {
                    if (CheckVirtualScreenBounds(dtMainFormLayout.Rows[0]))
                    {
                        left = int.Parse(dtMainFormLayout.Rows[0]["LeftX"].ToString());
                        top = int.Parse(dtMainFormLayout.Rows[0]["RightY"].ToString());
                    }
                }
                this.Location = new Point(left, top);

                // Set Up For CounterPartyUp and Down Forms
                TradeManagerExtension.GetInstance().OrderQueued += new TradeManagerExtension.QueuedDelegate(PranaMain_OrderQueued);
                QueuedOrdersForm.GetInstance.SetUp(_tradeCommManager);
                CounterPartyDownForm.GetInstance.SetUp();
                ShowHideServiceIcons(GeneralDatabaseManager.GetPMPrefDataFromDB(loginUser.CompanyUserID));
                String imageDirectoryPath = String.Format("{0}\\Themes\\{1}\\FormIcons\\", Application.StartupPath, CustomThemeHelper.WHITELABELTHEME);
                ChangePranaImageList(imageDirectoryPath);
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_NIRVANA_MAIN);

                #region Compliance Section
                try
                {
                    //Checking for Compliance module and disabling/enabling if not enabled
                    if (ComplianceCacheManager.GetPreOrPostModuleEnabledForUser(loginUser.CompanyUserID))
                    {
                        AmqpPlugin.AmqpPluginManager.GetInstance().Initialise(loginUser.CompanyUserID.ToString());
                        AmqpPlugin.AmqpPluginManager.GetInstance().SetupCompliancePostPopUp(this, loginUser.CompanyUserID);
                        AmqpPlugin.AmqpPluginManager.GetInstance().OverrideRequestReceived += new Prana.AmqpPlugin.OverrideRequestReceivedHandler(PranaMain_OverrideRequestReceived);
                        AmqpPlugin.AmqpPluginManager.GetInstance().PendingApprovalInfoDataSet += PranaMain_PendingApprovalInfoDataSet;
                        AmqpPlugin.AmqpPluginManager.GetInstance().PendingApprovalFrozeUnfroze += PranaMain_PendingApprovalFrozeUnfroze;
                    }
                }
                catch (Exception ex)
                {
                    //Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                LogFileHelper.GetInstance().AddLogFileToZip();
                #endregion

                EventAggregator.GetInstance.SubsribeEvent(this, SynchronizationContext.Current);

                #region Start Account Lock Timer
                accountLockTimer.Interval = 60000;
                accountLockTimer.Elapsed += new System.Timers.ElapsedEventHandler(AccountLock_TimerTickHandler);
                accountLockTimer.Start();
                #endregion

                Prana.ClientCommon.TradingTktPrefs.SetClientCache(loginUser);

                for (int i = 0; i < TradingTktPrefs.QuickTTPrefs.InstanceNames.Length; i++)
                {
                    quickTTButtons[i].SharedPropsInternal.Caption = TradingTktPrefs.QuickTTPrefs.InstanceNames[i];
                }
                DisableHideNotPermittedModules();
                bool isExportDataForAutomation = bool.Parse(ConfigurationManager.AppSettings["IsExportDataForAutomation"]);
                if (isExportDataForAutomation)
                {
                    HostDataExportService();
                }
                BlotterOrderCollections.GetInstance().UpdateShortLocateData += BlotterMain_UpdateShortLocateData;
            }
            catch (Exception ex)
            {
                ErrorMessageBox.Display("Error Occurred : " + ex.Message + ex.Source + ex.StackTrace + ex.InnerException + ex.TargetSite, "Error");
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        ServiceHost _host = null;
        private static Mutex _serviceMutex;
        private void HostDataExportService()
        {
            bool isNewInstance;
            _serviceMutex = new Mutex(true, "Global\\ExportGridDataServiceMutex", out isNewInstance);

            if (!isNewInstance)
            {
                return;
            }
            try
            {
                _host = new ServiceHost(typeof(ExportGridData));
                _host.AddServiceEndpoint(typeof(IExportGridData), new NetNamedPipeBinding(), "net.pipe://localhost/ExportGridData");
                _host.Open();
                if (_host.State == CommunicationState.Opened)
                {
                    Console.WriteLine("ExportGridData service is now running.");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void CleanUpExportService()
        {
            try
            {
                if (_host != null && _host.State == CommunicationState.Opened)
                {
                    _host.Close();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            _serviceMutex?.ReleaseMutex();
            _serviceMutex?.Dispose();
        }

        private void BlotterMain_UpdateShortLocateData(object sender, EventArgs<ShortLocateListParameter> e)
        {
            try
            {
                if (e.Value != null)
                {
                    Prana.ShortLocate.Classes.ShortLocateDataManager.GetInstance.UpdateShortLocateData(e.Value);
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

        private void GetMarketDataPermissions()
        {
            try
            {
                System.Threading.Tasks.Task.Run(new Action(() =>
                {
                    if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet)
                    {
                        if (!string.IsNullOrWhiteSpace(_loggedInUserIPAddress) && loginUser != null)
                        {
                            _marketDataPermissionServiceProxy.InnerChannel.PermissionCheck(new FactSetMarketDataPermissionRequest()
                            {
                                CompanyUserID = loginUser.CompanyUserID,
                                FactSetSerialNumber = loginUser.FactSetUsernameAndSerialNumber,
                                IsFactSetSupportUser = loginUser.IsFactSetSupportUser,
                                IpAddress = _loggedInUserIPAddress
                            }, "Client");
                        }
                        else
                        {
                            Logger.LoggerWrite("Unable to retrieve logged in machine's IPAddress for User: " + loginUser.LoginID);
                        }
                    }
                    else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.ACTIV)
                    {
                        _marketDataPermissionServiceProxy.InnerChannel.PermissionCheck(new ActivMarketDataPermissionRequest()
                        {
                            CompanyUserID = loginUser.CompanyUserID,
                            ActivUsername = loginUser.ActivUsername,
                            ActivPassword = loginUser.ActivPassword
                        }, "Client");
                    }
                    else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI)
                    {
                        if (!string.IsNullOrWhiteSpace(_loggedInUserIPAddress) && loginUser != null)
                        {
                            _marketDataPermissionServiceProxy.InnerChannel.PermissionCheck(new SapiMarketDataPermissionRequest()
                            {
                                CompanyUserID = loginUser.CompanyUserID,
                                SapiUsername = loginUser.SapiUsername,
                                IpAddress = _loggedInUserIPAddress
                            }, "Client");
                        }
                        else
                        {
                            Logger.LoggerWrite("Unable to retrieve logged in machine's IPAddress for User: " + loginUser.LoginID);
                        }
                    }
                    else
                    {
                        _marketDataPermissionServiceProxy.InnerChannel.PermissionCheck(new MarketDataPermissionRequest()
                        {
                            CompanyUserID = loginUser.CompanyUserID
                        }, "Client");
                    }
                }));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        private volatile bool _isMsgBoxBeingSelected = false;

        /// <summary>
        /// Pending Approval Info DataSet Open and bind data to UI 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PranaMain_PendingApprovalInfoDataSet(object sender, EventArgs<DataSet> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MainThreadDelegate2 del = this.PranaMain_PendingApprovalInfoDataSet;
                        this.BeginInvoke(del, new object[] { sender, e });
                    }
                    else
                    {
                        if (e.Value.Tables.Contains("TriggeredAlerts"))
                        {
                            HashSet<string> userIds = new HashSet<string>();
                            foreach (DataRow row in e.Value.Tables["TriggeredAlerts"].Rows)
                                row["OverrideUserId"].ToString().Split(',').ToList().ForEach(x => userIds.Add(x));

                            if (userIds.Contains(loginUser.CompanyUserID.ToString()))
                            {
                                if (!activePluggableTools.ContainsKey(ApplicationConstants.CONST_COMPLIANCE_ENGINE))
                                {
                                    if (!_isMsgBoxBeingSelected && ModuleManager.CheckToolPermissioningForPrana(ApplicationConstants.CONST_COMPLIANCE, ApplicationConstants.CONST_CE_PENDING_APPROVAL))
                                    {
                                        _isMsgBoxBeingSelected = true;
                                        DialogResult currentUserChoice = MessageBox.Show(this, "A request has been made to approve a trade that would breach compliance. Please open the “Pending Approval” module to view the details", "Nirvana Compliance", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                        if (currentUserChoice == DialogResult.Yes)
                                        {
                                            ComplianceCacheManager.SetComplianceUITabSelected(ApplicationConstants.CONST_COMPLIANCE_PENDING_APPROVAL);
                                            LaunchPluggableTool(ApplicationConstants.CONST_COMPLIANCE_PENDING_APPROVAL, GetFormNewLocation(null));
                                        }
                                        _isMsgBoxBeingSelected = false;
                                    }
                                }
                                else
                                    BindCompliancePendingApprovalData(e.Value, false);
                            }
                            else
                            {
                                BindCompliancePendingApprovalData(e.Value, false);
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
        /// Pending Approval Info DataSet Open and bind data to UI 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PranaMain_PendingApprovalFrozeUnfroze(object sender, EventArgs<DataSet, bool> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MainThreadDelegateForFrozeUnfroze del = this.PranaMain_PendingApprovalFrozeUnfroze;
                        this.BeginInvoke(del, new object[] { sender, e });
                    }
                    else
                    {
                        BindCompliancePendingApprovalDataToFrozeUnfroze(e.Value, e.Value2, "UpdatePendingFrozenUnfrozen");
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
        /// Binds the compliance pending approval data.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs{DataSet}"/> instance containing the event data.</param>
        private void BindCompliancePendingApprovalData(DataSet dataSet, bool isBringToFrontRequired)
        {
            try
            {
                if (dataSet != null && activePluggableTools.ContainsKey(ApplicationConstants.CONST_COMPLIANCE_ENGINE))
                {
                    DynamicClass formToLoad;
                    formToLoad = (DynamicClass)ModuleManager.AvailableTools[ApplicationConstants.CONST_COMPLIANCE];
                    Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);
                    Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);
                    IPluggableTools pluggableToolToShow = activePluggableTools[typeToLoad.Name];
                    Form pluggableForm = pluggableToolToShow.Reference();

                    MethodInfo mi = typeToLoad.GetMethod("UpdatePendingApprovalUI");
                    object[] parameters = new object[2];
                    parameters[0] = dataSet;
                    parameters[1] = isBringToFrontRequired;
                    mi.Invoke(pluggableForm, parameters);

                    if (isBringToFrontRequired)
                        BringFormToFront(pluggableForm, GetFormNewLocation(null));
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
        /// Binds the compliance pending approval data Froze/ Unfroze.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs{DataSet}"/> instance containing the event data.</param>
        private void BindCompliancePendingApprovalDataToFrozeUnfroze(DataSet dataSet, bool isFroze, string methodName)
        {
            try
            {
                if (dataSet != null && activePluggableTools.ContainsKey(ApplicationConstants.CONST_COMPLIANCE_ENGINE))
                {
                    DynamicClass formToLoad;
                    formToLoad = (DynamicClass)ModuleManager.AvailableTools[ApplicationConstants.CONST_COMPLIANCE];
                    Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);
                    Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);
                    IPluggableTools pluggableToolToShow = activePluggableTools[typeToLoad.Name];
                    Form pluggableForm = pluggableToolToShow.Reference();

                    MethodInfo mi = typeToLoad.GetMethod(methodName);
                    object[] parameters = new object[2];
                    parameters[0] = dataSet;
                    parameters[1] = isFroze;
                    mi.Invoke(pluggableForm, parameters);
                }
                else
                {
                    List<Alert> alertsAllowed = Alert.GetAlertObjectFromDataTable(dataSet.Tables["Alert"]);
                    foreach (Alert alert in alertsAllowed)
                    {
                        if (isFroze)
                        {
                            CachedDataManager.GetInstance.AddPendingApprovalFrozenAlerts(alert.OrderId);
                        }
                        else
                        {
                            CachedDataManager.GetInstance.RemovePendingApprovalFrozenAlerts(alert.OrderId);
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

        private void PranaMain_SetTokenFactsetUser(object sender, EventArgs<string, string> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MainThreadDelegateSetToekFactsetUser del = this.PranaMain_SetTokenFactsetUser;
                        this.BeginInvoke(del, new object[] { sender, e });
                    }
                    else
                    {
                        string currentToken = e.Value;
                        string usernameAndSerial = e.Value2;
                        if (!string.IsNullOrWhiteSpace(currentToken) && usernameAndSerial.Equals(loginUser.FactSetUsernameAndSerialNumber))
                        {
                            _loggedInUserIPAddress += "," + currentToken;
                            GetMarketDataPermissions();
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

        private void PranaMain_CloseFactsetForm(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MainThreadDelegateSetToekFactsetUser del = this.PranaMain_CloseFactsetForm;
                        this.BeginInvoke(del, new object[] { sender, e });
                    }
                    else
                    {
                        if (factsetAuthForm != null)
                            factsetAuthForm.Close();
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

        private void ChangePranaImageList(String imageDirectoryPath)
        {
            try
            {
                if ((Directory.Exists(imageDirectoryPath)))
                {
                    this.SuspendLayout();
                    this.PranaIconList.Images.Clear();

                    this.PranaIconList.ImageSize = new Size(40, 40);
                    CheckAddToolBarIcons(imageDirectoryPath, "TT.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "TT_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "PranaBlotter.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "PranaBlotter_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "PST.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "PST_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "ReBalancer.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "ReBalancer_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "Allocation.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "Allocation_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "WatchList.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "WatchList_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "ComplianceIcon.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "ComplianceIcon_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "PM.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "PM_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "RiskAnalysis.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "RiskAnalysis_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "CashMgmt.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "CashMgmt_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "SecurityMaster.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "SecurityMaster_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "ThirdParty.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "ThirdParty_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "Reports.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "Reports_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "BackOffice.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "BackOffice_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "Tools.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "Tools_Hover.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "HelpAndSupport.png");
                    CheckAddToolBarIcons(imageDirectoryPath, "HelpAndSupport_Hover.png");
                    this.ResumeLayout();
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
        /// Checks the add tool bar icons.
        /// </summary>
        private void CheckAddToolBarIcons(string imageDirectoryPath, string iconName)
        {
            try
            {
                if (File.Exists(imageDirectoryPath + iconName))
                    this.PranaIconList.Images.Add(Image.FromFile(imageDirectoryPath + iconName));
                else
                    this.PranaIconList.Images.Add(Image.FromFile(imageDirectoryPath + "missing.png"));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// update the dictionary of account by 1 minute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AccountLock_TimerTickHandler(object sender, EventArgs e)
        {
            try
            {
                int minutes;
                //Load time duration from App.Config
                bool result = int.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_AccountLockReleaseInterval), out minutes);

                if (!result)
                {
                    throw new Exception("Cannot convert value of AccountLockReleaseInterval to integer from configuration file. Please check settings");
                }

                //Fill Account Lock auto  unlock dictionary
                List<int> accountsToBeLocked = CachedDataManager.GetInstance.GetLockedAccounts();
                bool isAccountTimedOut = false;
                ConcurrentDictionary<int, int> dictAccountsLockDuration = CachedDataManager.GetInstance.GetAccountsLockDuration();
                foreach (KeyValuePair<int, string> account in CachedDataManager.GetInstance.GetAccountsWithFullName())
                {
                    if (dictAccountsLockDuration.ContainsKey(account.Key) && dictAccountsLockDuration[account.Key] != int.MinValue)
                    {
                        if (dictAccountsLockDuration[account.Key] >= minutes)
                        {
                            isAccountTimedOut = true;
                            accountsToBeLocked.Remove(account.Key);
                        }
                        else
                        {
                            CachedDataManager.GetInstance.ResetAccountsLockTimer(account.Key, dictAccountsLockDuration[account.Key] + 1);
                        }
                    }
                }
                if (isAccountTimedOut)
                {
                    ReconUtilities.SetAccountsLockStatus(accountsToBeLocked);
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

        void PranaMain_Shown(object sender, EventArgs e)
        {
            try
            {
                SetServerConnectionStatus(null);
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

        delegate void MainThreadDelegate(String message, DataSet dsReceived);

        void PranaMain_OverrideRequestReceived(string message, DataSet data)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MainThreadDelegate del = this.PranaMain_OverrideRequestReceived;
                        this.BeginInvoke(del, new object[] { message, data });
                    }
                    else
                    {
                        AlertPopUpType popUpType = (AlertPopUpType)Convert.ToInt32(data.Tables[0].Rows[0]["popUpType"]);
                        ComplianceAlertPopUp popUp = new ComplianceAlertPopUp();
                        List<Alert> alerts = Alert.GetAlertObjectFromDataTable(data.Tables["alerts"]);
                        popUp.BindingComplianceAlertData(popUpType, alerts, true);
                        popUp.ShowDialog(this);
                        popUp.Activate();

                        if (popUpType == AlertPopUpType.Override && !popUp.IsTradeAllowed)
                            ShortLocate.ShortLocate.TradeCheck = false;
                        ComplianceServiceConnector.GetInstance().UpdateAlerts(popUp.GetUpdatedAlerts(), data.Tables[0].Rows[0]["OrderId"].ToString());
                        AmqpPlugin.AmqpPluginManager.GetInstance().SendResponse(data, popUp.IsTradeAllowed, popUpType);
                        popUp.Dispose();
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

        delegate void MainThreadDelegate2(object sender, EventArgs<DataSet> e);

        delegate void MainThreadDelegateForFrozeUnfroze(object sender, EventArgs<DataSet, bool> e);

        delegate void MainThreadDelegateSetToekFactsetUser(object sender, EventArgs<string, string> e);

        delegate void MainThreadDelegateCloseFactSetUI(object sender, EventArgs e);

        private void LoadLayoutForUser()
        {
            string defaultLayoutID = string.Empty;
            try
            {
                DataTable dtLayout = LayoutManager.GetAllLayoutsForUser(loginUser.CompanyUserID);
                foreach (DataRow layoutRow in dtLayout.Rows)
                {
                    if (layoutRow["LayoutName"].ToString().Equals("DEFAULT") && string.IsNullOrEmpty(defaultLayoutID))
                        defaultLayoutID = layoutRow["LayoutID"].ToString();
                }
                if (dtLayout.Rows.Count > 0)
                {
                    _layoutID = int.Parse(dtLayout.Rows[0]["LayoutID"].ToString());
                    _layoutName = dtLayout.Rows[0]["LayoutName"].ToString();
                    LoadLayout();
                }
                else
                {
                    _layoutID = 0;
                    _layoutName = "";
                    for (int i = this.OwnedForms.Length; i > 0; i--)
                    {
                        this.OwnedForms[i - 1].Close();
                    }
                }
                _loadApplicationFirstTime = false;
            }
            catch (Exception)
            {
                try
                {
                    //check if layout id present then load 
                    if (_layoutID != 0 && !string.IsNullOrEmpty(defaultLayoutID))
                    {
                        _layoutID = int.Parse(defaultLayoutID);
                        _layoutName = "DEFAULT";
                        LoadLayout();
                    }
                    else
                    {
                        Logger.LoggerWrite("DEFAULT layout not present for User: " + loginUser.LoginID);
                    }
                }
                catch (Exception exp)
                {
                    bool rethrowLocal = Logger.HandleException(exp, LoggingConstants.POLICY_LOGANDSHOW);
                    if (rethrowLocal)
                    {
                        throw;
                    }
                }
            }
        }

        #region Communication Manager Communication
        void PranaMain_OrderQueued(object sender, QueuedDelegateEventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (!this.InvokeRequired)
                    {
                        Prana.CommonDataCache.NameValueFiller.FillNameDetailsOfOrder(e.Orders);
                        if (e.ConnStatus == PranaInternalConstants.ConnectionStatus.DISCONNECTED)
                        {
                            CPDown(e.Orders);
                        }
                        else
                        {
                            CPUp(e.Orders);
                        }
                    }
                    else
                    {
                        TradeManagerExtension.QueuedDelegate mi = new TradeManagerExtension.QueuedDelegate(PranaMain_OrderQueued);
                        this.BeginInvoke(mi, sender, e);
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
        /// <param name="order"></param>
        private void CPDown(Order order)
        {
            CounterPartyDownForm.GetInstance.AddQueuedOrder(order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        private void CPUp(Order order)
        {
            try
            {
                CounterPartyDownForm.GetInstance.RemoveOrder(order);
                QueuedOrdersForm.GetInstance.AddQueuedOrder(order);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _communicationManager_Disconnected(object sender, EventArgs e)
        {
            ConnectionProperties con = ((SocketConnection)sender).ConnProperties;
            if (_isHandleCreated)
            {
                SetServerConnectionStatus(con);
            }
            TradeManagerExtension.GetInstance().DisconnectAllCounterParties();
            Prana.TradeManager.TradeManager.GetInstance().AlgoReplaceEditHandler -= new Prana.TradeManager.TradeManager.AlgoReplaceOrderEditHandler(PranaMain_AlgoReplaceEditHandler);
            Prana.TradeManager.TradeManager.GetInstance().AlgoValidTradeToBlotterUI -= new AlgoValidTradeHandler(PranaMain_AlgoValidTradeToBlotterUI);
        }

        void _communicationManager_Connected(object sender, EventArgs e)
        {
            ConnectionProperties con = ((SocketConnection)sender).ConnProperties;
            if (_isHandleCreated)
            {
                SetServerConnectionStatus(con);
            }
            _clientConnectivityService.InnerChannel.AddClientInfoInCache(loginUser.CompanyUserID);
        }
        #endregion

        private DialogResult _formClosingDialogResult = DialogResult.Yes;
        void Application_ApplicationExit(object sender, EventArgs e)
        {
            try
            {
                Logger.LoggerWrite("Application_ApplicationExit called");
                _formClosingDialogResult = AskSaveConfirmation();
                if (_formClosingDialogResult != DialogResult.Cancel)
                {
                    PerformLogoutOperations();
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    Application.ExitThread();
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite("Application_ApplicationExit called error occured, ex.Message:" + ex.Message);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method performs logout cleanup by syncing preferences, closing modules, removing cache entries, and disconnecting services.
        /// </summary>
        private void PerformLogoutOperations()
        {
            try
            {
                if (null != loginUser)
                {
                    FileAndDbSyncManager.SyncDataBaseWithFile("Prana Preferences", loginUser.CompanyUserID);
                    // userID -1 is kept as defaultID implying that the preferences are common to all users and will be picked from main prana Preferences folder...
                    FileAndDbSyncManager.SyncDataBaseWithFile("Prana Preferences", -1);

                    if (_optionChain != null && OpenModulesList.Contains(PranaModules.OPTIONCHAIN_MODULE))
                        _optionChain.Close();
                    try
                    {
                        _clientConnectivityService.InnerChannel.RemoveClientInfoFromCache(loginUser.CompanyUserID, false, false);
                        try
                        {
                            _marketDataPermissionServiceProxy.InnerChannel.RemoveSubscriptionToGetPermissionFromCache(loginUser.CompanyUserID, "Client", false);
                        }
                        catch (Exception)
                        {
                            Logger.LoggerWrite("Faced Issue while connecting to pricing server", LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION);
                        }

                    }
                    catch
                    {
                        Logger.LoggerWrite("Trade Service was closed before Client", LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION);
                    }
                }
                if (_tradeCommManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                    _tradeCommManager.DisConnect();
                Logger.LoggerWrite("Application_ApplicationExit called GetCurrentProcess().Kill() will be executed.");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ExitApplication()
        {
            try
            {
                Logger.LoggerWrite("ExitApplication called");
                if (_tradeCommManager != null)
                {
                    _tradeCommManager.DisConnect();
                }
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                Logger.LoggerWrite("ExitApplication called, GetCurrentProcess().Kill() executed.");
                Application.ExitThread();
            }
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
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BlotterOrderCollections.GetInstance().UpdateShortLocateData -= BlotterMain_UpdateShortLocateData;

                if (blotterForm != null)
                {
                    blotterForm.Dispose();
                    blotterForm.Disposed -= new EventHandler(blotterForm_Disposed);
                }
                if (allocationForm != null)
                {
                    allocationForm.Dispose();
                }
                if (reportForm != null)
                {
                    reportForm.Dispose();
                }
                if (tradeAuditUIForm != null)
                {
                    tradeAuditUIForm.Dispose();
                }
                if (portfolioManagementForm != null)
                {
                    portfolioManagementForm.Dispose();
                }
                if (thirdPartyReportFrom != null)
                {
                    thirdPartyReportFrom.Dispose();
                }
                if (positionManagementDailySheetForm != null)
                {
                    positionManagementDailySheetForm.Dispose();
                }
                if (positionManagementMonthlySheetForm != null)
                {
                    positionManagementMonthlySheetForm.Dispose();
                }
                if (QualityCheckForm != null)
                {
                    QualityCheckForm.Dispose();
                }
                if (frmPranaHelp != null)
                {
                    frmPranaHelp.Dispose();
                }
                if (frmHelpAndSupport != null)
                {
                    frmHelpAndSupport.Dispose();
                }
                if (frmDisclaimer != null)
                {
                    frmDisclaimer.Dispose();
                }
                if (frmPranaShortcuts != null)
                {
                    frmPranaShortcuts.Dispose();
                }
                if (lblConnectionStatus != null)
                {
                    lblConnectionStatus.Dispose();
                }
                if (lblExPNLConnection != null)
                {
                    lblExPNLConnection.Dispose();
                }
                if (preferencesMain != null)
                {
                    preferencesMain.Dispose();
                }
                if (lblPricingServer != null)
                {
                    lblPricingServer.Dispose();
                }
                if (ultraToolTipManagerForPricing != null)
                {
                    ultraToolTipManagerForPricing.Dispose();
                }
                if (ultraPanel1 != null)
                {
                    ultraPanel1.Dispose();
                }
                if (ultraPanel2 != null)
                {
                    ultraPanel2.Dispose();
                }
                if (_ClientArea_Toolbars_Dock_Area_Bottom != null)
                {
                    _ClientArea_Toolbars_Dock_Area_Bottom.Dispose();
                }
                if (_ClientArea_Toolbars_Dock_Area_Left != null)
                {
                    _ClientArea_Toolbars_Dock_Area_Left.Dispose();
                }
                if (_ClientArea_Toolbars_Dock_Area_Right != null)
                {
                    _ClientArea_Toolbars_Dock_Area_Right.Dispose();
                }
                if (_ClientArea_Toolbars_Dock_Area_Top != null)
                {
                    _ClientArea_Toolbars_Dock_Area_Top.Dispose();
                }
                if (_PranaMain_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _PranaMain_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
                if (_PranaMain_UltraFormManager_Dock_Area_Left != null)
                {
                    _PranaMain_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (_PranaMain_UltraFormManager_Dock_Area_Right != null)
                {
                    _PranaMain_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (inboxControlStyler1 != null)
                {
                    inboxControlStyler1.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (_PranaMain_UltraFormManager_Dock_Area_Top != null)
                {
                    _PranaMain_UltraFormManager_Dock_Area_Top.Dispose();
                }
                if (PranaIconListThemesOff != null)
                {
                    PranaIconListThemesOff.Dispose();
                }
                if (appStylistRuntime1 != null)
                {
                    appStylistRuntime1.Dispose();
                }
                if (formBrokerConnection != null)
                {
                    formBrokerConnection.Dispose();
                }
                if (_marketDataPermissionDeniedAlert != null)
                {
                    _marketDataPermissionDeniedAlert.Dispose();
                }
                if (_marketDataPermissionDeniedTimer != null)
                {
                    _marketDataPermissionDeniedTimer.Dispose();
                }
                if (_marketDataPermissionRecheckTimer != null)
                {
                    _marketDataPermissionRecheckTimer.Dispose();
                }
                if (tradingTicketDock != null)
                {
                    tradingTicketDock.Dispose();
                }
                if (_watchList != null)
                {
                    _watchList.Dispose();
                }
                if (_optionChain != null)
                {
                    _optionChain.Dispose();
                }
                if (_shortLocate != null)
                {
                    _shortLocate.Dispose();
                }
                if (multiTradingTicketDock != null)
                {
                    multiTradingTicketDock.Dispose();
                }
                if (markPriceUIForm != null)
                {
                    markPriceUIForm.Dispose();
                }
                if (closingUIForm != null)
                {
                    closingUIForm.Dispose();
                }
                if (postReconAmendmentUIForm != null)
                {
                    postReconAmendmentUIForm.Dispose();
                }
                if (formCA != null)
                {
                    formCA.Dispose();
                }
                if (createTransactionInstance != null)
                {
                    createTransactionForm.Dispose();
                }
                if (washSaleForm != null)
                {
                    washSaleForm.Dispose();
                }
                if (cashAccountForm != null)
                {
                    cashAccountForm.Dispose();
                }
                if (mappingForm != null)
                {
                    mappingForm.Dispose();
                }
                if (accruals != null)
                {
                    accruals.Dispose();
                }
                if (cashManagement != null)
                {
                    cashManagement.Dispose();
                }
                if (PositionManagement != null)
                {
                    PositionManagement.Dispose();
                }
                if (components != null)
                {
                    components.Dispose();
                }
                if (frmRebalancer != null)
                {
                    frmRebalancer.Dispose();
                }
                if (_marketDataPermissionServiceProxy != null)
                {
                    _marketDataPermissionServiceProxy.Dispose();
                }
                if (accountLockTimer != null)
                {
                    accountLockTimer.Dispose();
                }
                if (PranaIconList != null)
                {
                    PranaIconList.Dispose();
                }
                if (PranaMainToolBar != null)
                {
                    PranaMainToolBar.Dispose();
                }
                if (allocationClientForm != null)
                {
                    allocationClientForm.Dispose();
                }
                if (reports2 != null)
                {
                    reports2.Dispose();
                }
                if (_clientConnectivityService != null)
                {
                    _clientConnectivityService.Dispose();
                }
                if (serverReportForm != null)
                {
                    serverReportForm.Dispose();
                }
                CleanUpExportService();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PranaMain));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("subToolBar");
            Infragistics.Win.UltraWinToolbars.ButtonTool tradingTicket1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Trading Ticket");
            Infragistics.Win.UltraWinToolbars.ButtonTool tradingTicket2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Trading Ticket");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool tradingTicket3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("TradingTicketPopUp");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool tradingTicket4 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("TradingTicketPopUp");
            Infragistics.Win.UltraWinToolbars.ButtonTool tradingTicketButton1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TradingTicketButton");
            Infragistics.Win.UltraWinToolbars.ButtonTool tradingTicketButton2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TradingTicketButton");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket1");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket1");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket2");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket2");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket3");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket3");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket4");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket4");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket5");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket5");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket6");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket6");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket7");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket7");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket8");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket8");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket9");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket9");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket10");
            Infragistics.Win.UltraWinToolbars.ButtonTool quickTradingTicket20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QuickTradingTicket10");

            Infragistics.Win.UltraWinToolbars.ButtonTool blotter1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Blotter");
            Infragistics.Win.UltraWinToolbars.ButtonTool blotter2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Blotter");
            Infragistics.Win.UltraWinToolbars.ButtonTool pTT1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("% Trading Tool");
            Infragistics.Win.UltraWinToolbars.ButtonTool pTT2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("% Trading Tool");
            Infragistics.Win.UltraWinToolbars.ButtonTool rebalancer1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Rebalancer");
            Infragistics.Win.UltraWinToolbars.ButtonTool rebalancer2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Rebalancer");
            Infragistics.Win.UltraWinToolbars.ButtonTool allocation1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Allocation");
            Infragistics.Win.UltraWinToolbars.ButtonTool allocation2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Allocation");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool watchlist1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("WatchlistPopup");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool watchlist2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("WatchlistPopup");
            Infragistics.Win.UltraWinToolbars.ButtonTool watchlistButton1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("WatchlistButton");
            Infragistics.Win.UltraWinToolbars.ButtonTool watchlistButton2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("WatchlistButton");
            Infragistics.Win.UltraWinToolbars.ButtonTool watchlistButton3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Watchlist");
            Infragistics.Win.UltraWinToolbars.ButtonTool watchlistButton4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Watchlist");
            Infragistics.Win.UltraWinToolbars.ButtonTool optionChain1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OptionChain");
            Infragistics.Win.UltraWinToolbars.ButtonTool optionChain2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OptionChain");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool compliance1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Compliance");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool compliance2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Compliance");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AlertHistory");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AlertHistory");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PendingApproval");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PendingApproval");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RuleDefinition");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RuleDefinition");
            Infragistics.Win.UltraWinToolbars.ButtonTool portfolioManagement1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PortfolioManagement");
            Infragistics.Win.UltraWinToolbars.ButtonTool portfolioManagement2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PortfolioManagement");
            Infragistics.Win.UltraWinToolbars.ButtonTool riskAnalysis1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RiskAnalysis");
            Infragistics.Win.UltraWinToolbars.ButtonTool riskAnalysis2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RiskAnalysis");
            Infragistics.Win.UltraWinToolbars.ButtonTool generalLedger1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("GeneralLedger");
            Infragistics.Win.UltraWinToolbars.ButtonTool generalLedger2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("GeneralLedger");
            Infragistics.Win.UltraWinToolbars.ButtonTool securityMaster1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SecurityMaster");
            Infragistics.Win.UltraWinToolbars.ButtonTool securityMaster2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SecurityMaster");
            Infragistics.Win.UltraWinToolbars.ButtonTool thirdPartyManager1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ThirdPartyManager");
            Infragistics.Win.UltraWinToolbars.ButtonTool thirdPartyManager2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ThirdPartyManager");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool reports1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Reports");
            Infragistics.Win.UltraWinToolbars.ButtonTool blotterExecReport1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BlotterExecutionReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool blotterExecReport2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BlotterExecutionReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool auditTrail1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AuditTrail");
            Infragistics.Win.UltraWinToolbars.ButtonTool auditTrail2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AuditTrail");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool backOffice1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("BackOffice");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool backOffice2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("BackOffice");
            Infragistics.Win.UltraWinToolbars.ButtonTool dailyValuation1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DailyValuation");
            Infragistics.Win.UltraWinToolbars.ButtonTool dailyValuation2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DailyValuation");
            Infragistics.Win.UltraWinToolbars.ButtonTool recon1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recon");
            Infragistics.Win.UltraWinToolbars.ButtonTool recon2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recon");
            Infragistics.Win.UltraWinToolbars.ButtonTool closing1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Closing");
            Infragistics.Win.UltraWinToolbars.ButtonTool closing2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Closing");
            Infragistics.Win.UltraWinToolbars.ButtonTool pricingInputs1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PricingInputs");
            Infragistics.Win.UltraWinToolbars.ButtonTool pricingInputs2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PricingInputs");
            Infragistics.Win.UltraWinToolbars.ButtonTool dataMapping1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DataMapping");
            Infragistics.Win.UltraWinToolbars.ButtonTool dataMapping2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DataMapping");
            Infragistics.Win.UltraWinToolbars.ButtonTool import1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Import");
            Infragistics.Win.UltraWinToolbars.ButtonTool import2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Import");
            Infragistics.Win.UltraWinToolbars.ButtonTool autoImport1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AutoImport");
            Infragistics.Win.UltraWinToolbars.ButtonTool autoImport2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AutoImport");
            Infragistics.Win.UltraWinToolbars.ButtonTool missingTrades1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MissingTrades");
            Infragistics.Win.UltraWinToolbars.ButtonTool missingTrades2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MissingTrades");
            Infragistics.Win.UltraWinToolbars.ButtonTool createTransaction1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CreateTransaction");
            Infragistics.Win.UltraWinToolbars.ButtonTool createTransaction2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CreateTransaction");
            Infragistics.Win.UltraWinToolbars.ButtonTool corporateActions1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CorporateActions");
            Infragistics.Win.UltraWinToolbars.ButtonTool corporateActions2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CorporateActions");
            Infragistics.Win.UltraWinToolbars.ButtonTool washSale1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("WashSale");
            Infragistics.Win.UltraWinToolbars.ButtonTool washSale2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("WashSale");
            Infragistics.Win.UltraWinToolbars.ButtonTool navLock1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NAVLock");
            Infragistics.Win.UltraWinToolbars.ButtonTool navLock2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NAVLock");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool tools1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Tools");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool tools2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Tools");
            Infragistics.Win.UltraWinToolbars.ButtonTool preferences1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool preferences2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool shortLocate1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ShortLocate");
            Infragistics.Win.UltraWinToolbars.ButtonTool shortLocate2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ShortLocate");
            Infragistics.Win.UltraWinToolbars.ButtonTool brokerConnections1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BrokerConnections");
            Infragistics.Win.UltraWinToolbars.ButtonTool brokerConnections2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BrokerConnections");
            Infragistics.Win.UltraWinToolbars.ButtonTool reloadSettings1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReloadSettings");
            Infragistics.Win.UltraWinToolbars.ButtonTool reloadSettings2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReloadSettings");
            Infragistics.Win.UltraWinToolbars.ButtonTool middlewareManager1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MiddlewareManager");
            Infragistics.Win.UltraWinToolbars.ButtonTool middlewareManager2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MiddlewareManager");
            Infragistics.Win.UltraWinToolbars.ButtonTool zeroPositionAlert1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ZeroPositionAlert");
            Infragistics.Win.UltraWinToolbars.ButtonTool zeroPositionAlert2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ZeroPositionAlert");
            Infragistics.Win.UltraWinToolbars.ButtonTool qualityChecker1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QualityChecker");
            Infragistics.Win.UltraWinToolbars.ButtonTool qualityChecker2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("QualityChecker");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool helpAndSupport1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("HelpAndSupport");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool helpAndSupport2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("HelpAndSupport");
            Infragistics.Win.UltraWinToolbars.ButtonTool moduleShortcuts1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ModuleShortcuts");
            Infragistics.Win.UltraWinToolbars.ButtonTool moduleShortcuts2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ModuleShortcuts");
            Infragistics.Win.UltraWinToolbars.ButtonTool aboutNirvana1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AboutNirvana");
            Infragistics.Win.UltraWinToolbars.ButtonTool aboutNirvana2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AboutNirvana");
            Infragistics.Win.UltraWinToolbars.ButtonTool disclaimer1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Disclaimer");
            Infragistics.Win.UltraWinToolbars.ButtonTool disclaimer2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Disclaimer");

            this.PranaIconList = new System.Windows.Forms.ImageList(this.components);
            this.lblConnectionStatus = new Infragistics.Win.Misc.UltraLabel();
            this.lblExPNLConnection = new Infragistics.Win.Misc.UltraLabel();
            this.ultraToolTipManagerForPricing = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.lblPricingServer = new Infragistics.Win.Misc.UltraLabel();
            this.btnLiveFeedConnect = new Infragistics.Win.Misc.UltraButton();
            this.PranaMainToolBar = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this._ClientArea_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();

            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._PranaMain_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PranaMain_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PranaMain_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PranaMain_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.PranaIconListThemesOff = new System.Windows.Forms.ImageList(this.components);
            this.appStylistRuntime1 = new Infragistics.Win.AppStyling.Runtime.AppStylistRuntime(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PranaMainToolBar)).BeginInit();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // PranaIconList
            // 
            this.PranaIconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("PranaIconList.ImageStream")));
            this.PranaIconList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lblConnectionStatus
            // 
            appearance1.ImageBackground = global::Prana.Properties.Resources.NirvanaMainIndicatorRed;
            appearance1.ImageBackgroundStyle = ImageBackgroundStyle.Centered;
            this.lblConnectionStatus.Appearance = appearance1;
            this.lblConnectionStatus.Location = new System.Drawing.Point(1066, 41);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(10, 10);
            this.lblConnectionStatus.Text = "Trade Engine";
            this.lblConnectionStatus.TabIndex = 14;
            this.lblConnectionStatus.UseAppStyling = false;
            // 
            // lblExPNLConnection
            // 
            appearance2.ImageBackground = global::Prana.Properties.Resources.NirvanaMainIndicatorRed;
            appearance2.ImageBackgroundStyle = ImageBackgroundStyle.Centered;

            this.lblExPNLConnection.Appearance = appearance2;
            this.lblExPNLConnection.Location = new System.Drawing.Point(1066, 13);
            this.lblExPNLConnection.Name = "lblExPNLConnection";
            this.lblExPNLConnection.Size = new System.Drawing.Size(10, 10);
            this.lblExPNLConnection.Text = "Calculation Engine";
            this.lblExPNLConnection.TabIndex = 39;
            this.lblExPNLConnection.UseAppStyling = false;
            // 
            // ultraToolTipManagerForPricing
            // 
            appearance3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ultraToolTipManagerForPricing.Appearance = appearance3;
            this.ultraToolTipManagerForPricing.AutoPopDelay = 50000;
            this.ultraToolTipManagerForPricing.ContainingControl = this;
            this.ultraToolTipManagerForPricing.DisplayStyle = Infragistics.Win.ToolTipDisplayStyle.WindowsVista;
            this.ultraToolTipManagerForPricing.InitialDelay = 10;
            appearance33.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ultraToolTipManagerForPricing.ToolTipTitleAppearance = appearance33;
            // 
            // lblPricingServer
            // 
            appearance4.ImageBackground = global::Prana.Properties.Resources.NirvanaMainIndicatorRed;
            appearance4.ImageBackgroundStyle = ImageBackgroundStyle.Centered;
            this.lblPricingServer.Appearance = appearance4;
            this.lblPricingServer.Location = new System.Drawing.Point(1066, 27);
            this.lblPricingServer.Name = "lblPricingServer";
            this.lblPricingServer.Size = new System.Drawing.Size(10, 10);
            this.lblPricingServer.Text = "Market Data";
            this.lblPricingServer.TabIndex = 106;
            this.lblPricingServer.UseAppStyling = false;
            this.lblPricingServer.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            //
            //btnLiveFeedConnect
            //
            this.btnLiveFeedConnect.Location = new System.Drawing.Point(940, 27);
            this.btnLiveFeedConnect.Name = "btnLiveFeedConnect";
            this.btnLiveFeedConnect.ShowFocusRect = false;
            this.btnLiveFeedConnect.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212))))); ;
            this.btnLiveFeedConnect.ShowOutline = false;
            this.btnLiveFeedConnect.UseFlatMode = DefaultableBoolean.True;
            this.btnLiveFeedConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnLiveFeedConnect.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnLiveFeedConnect.Size = new System.Drawing.Size(120, 15);
            this.btnLiveFeedConnect.Text = "Connect Live Feed";
            this.btnLiveFeedConnect.TabIndex = 106;
            this.btnLiveFeedConnect.UseAppStyling = false;
            this.btnLiveFeedConnect.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this.btnLiveFeedConnect.Visible = true;
            this.btnLiveFeedConnect.Cursor = Cursors.Hand;
            this.btnLiveFeedConnect.Click += new EventHandler(btnLiveFeedConnect_Click);
            this.btnLiveFeedConnect.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // PranaMainToolBar
            // 
            this.PranaMainToolBar.DesignerFlags = 1;
            this.PranaMainToolBar.DockWithinContainer = this.ultraPanel2.ClientArea;
            this.PranaMainToolBar.ImageListLarge = this.PranaIconList;
            this.PranaMainToolBar.ImageListSmall = this.PranaIconList;
            this.PranaMainToolBar.ImageSizeSmall = new System.Drawing.Size(40, 40);
            this.PranaMainToolBar.LockToolbars = true;
            this.PranaMainToolBar.Office2007UICompatibility = false;
            this.PranaMainToolBar.RuntimeCustomizationOptions = Infragistics.Win.UltraWinToolbars.RuntimeCustomizationOptions.None;
            this.PranaMainToolBar.ShowFullMenusDelay = 500;
            this.PranaMainToolBar.ShowQuickCustomizeButton = false;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.FloatingLocation = new System.Drawing.Point(101, 237);
            ultraToolbar1.FloatingSize = new System.Drawing.Size(132, 105);
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
                tradingTicket1,
                tradingTicket3,
                blotter1,
                pTT2,
                rebalancer2,
                allocation2,
                watchlistButton3,
                watchlist1,
                compliance1,
                portfolioManagement1,
                riskAnalysis1,
                generalLedger1,
                securityMaster1,
                thirdPartyManager1,
                reports1,
                backOffice1,
                tools1,
                helpAndSupport1
            });
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Text = "subToolBar";
            this.PranaMainToolBar.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            this.PranaMainToolBar.ToolbarSettings.PaddingLeft = 5;
            this.PranaMainToolBar.ToolbarSettings.PaddingTop = 7;
            this.PranaMainToolBar.ToolbarSettings.PaddingBottom = 8;
            this.PranaMainToolBar.ToolbarSettings.ToolSpacing = 0;

            appearance10.Image = 0;
            appearance10.ForeColor = System.Drawing.Color.Black;
            tradingTicket4.SharedPropsInternal.AppearancesSmall.Appearance = appearance10;
            tradingTicket4.DropDownArrowStyle = Infragistics.Win.UltraWinToolbars.DropDownArrowStyle.Standard;
            appearance50.Image = 1;
            tradingTicket4.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance50;
            tradingTicket4.SharedPropsInternal.Caption = "Trading Ticket";
            tradingTicket4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageOnlyOnToolbars;
            Infragistics.Win.UltraWinToolbars.ToolBase[] dropdownItems = new Infragistics.Win.UltraWinToolbars.ToolBase[] {
                tradingTicketButton1,
                quickTradingTicket1,
                quickTradingTicket3,
                quickTradingTicket5,
                quickTradingTicket7,
                quickTradingTicket9,
                quickTradingTicket11,
                quickTradingTicket13,
                quickTradingTicket15,
                quickTradingTicket17,
                quickTradingTicket19
            };
            tradingTicket4.Tools.AddRange(dropdownItems);
            tradingTicketButton2.SharedPropsInternal.Caption = "Trading Ticket";
            quickTTButtons = new Infragistics.Win.UltraWinToolbars.ButtonTool[]
            {
                quickTradingTicket2, quickTradingTicket4, quickTradingTicket6, quickTradingTicket8, quickTradingTicket10,
                quickTradingTicket12, quickTradingTicket14, quickTradingTicket16, quickTradingTicket18, quickTradingTicket20
            };

            Infragistics.Win.Appearance appearanceTTbtnNew = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearanceHoverTTbtnNew = new Infragistics.Win.Appearance();
            // new TT btn 
            appearanceTTbtnNew.Image = 0;
            tradingTicket2.SharedPropsInternal.AppearancesSmall.Appearance = appearanceTTbtnNew;
            appearanceHoverTTbtnNew.Image = 1;
            tradingTicket2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearanceHoverTTbtnNew;
            tradingTicket2.SharedPropsInternal.Caption = "Trading Ticket";

            appearance11.Image = 2;
            blotter2.SharedPropsInternal.AppearancesSmall.Appearance = appearance11;
            appearance51.Image = 3;
            blotter2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance51;
            blotter2.SharedPropsInternal.Caption = "Blotter";

            appearance12.Image = 4;
            pTT1.SharedPropsInternal.AppearancesSmall.Appearance = appearance12;
            appearance52.Image = 5;
            pTT1.SharedPropsInternal.AppearancesSmall.Appearance = appearance52;
            pTT1.SharedPropsInternal.Caption = "% Trading Tool";

            appearance13.Image = 6;
            rebalancer1.SharedPropsInternal.AppearancesSmall.Appearance = appearance13;
            appearance53.Image = 7;
            rebalancer1.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance53;
            rebalancer1.SharedPropsInternal.Caption = "Rebalancer";

            appearance14.Image = 8;
            allocation1.SharedPropsInternal.AppearancesSmall.Appearance = appearance14;
            appearance54.Image = 9;
            allocation1.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance54;
            allocation1.SharedPropsInternal.Caption = "Allocation";

            watchlist2.DropDownArrowStyle = Infragistics.Win.UltraWinToolbars.DropDownArrowStyle.Standard;
            appearance15.Image = 10;
            appearance15.ForeColor = System.Drawing.Color.Black;
            watchlist2.SharedPropsInternal.AppearancesSmall.Appearance = appearance15;
            appearance55.Image = 11;
            watchlist2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance55;
            watchlist2.SharedPropsInternal.Caption = "Watchlist";
            watchlist2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageOnlyOnToolbars;
            watchlist2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            watchlistButton1,
            optionChain1});
            watchlistButton2.SharedPropsInternal.Caption = "Watchlist";
            optionChain2.SharedPropsInternal.Caption = "Option Chain";

            Infragistics.Win.Appearance appearanceWatchlistbtnNew = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearanceHoverWatchlistbtnNew = new Infragistics.Win.Appearance();
            // new Watchlist btn 
            appearanceWatchlistbtnNew.Image = 10;
            watchlistButton4.SharedPropsInternal.AppearancesSmall.Appearance = appearanceWatchlistbtnNew;
            appearanceHoverWatchlistbtnNew.Image = 11;
            watchlistButton4.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearanceHoverWatchlistbtnNew;
            watchlistButton4.SharedPropsInternal.Caption = "Watchlist";

            compliance2.DropDownArrowStyle = Infragistics.Win.UltraWinToolbars.DropDownArrowStyle.Standard;
            appearance16.Image = 12;
            appearance16.ForeColor = System.Drawing.Color.Black;
            compliance2.SharedPropsInternal.AppearancesSmall.Appearance = appearance16;
            appearance56.Image = 13;
            compliance2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance56;
            compliance2.SharedPropsInternal.Caption = "Compliance Engine";
            compliance2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageOnlyOnToolbars;
            compliance2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool13,
            buttonTool14,
            buttonTool20});
            buttonTool21.SharedPropsInternal.Caption = "Alert History";
            buttonTool21.SharedPropsInternal.Category = "Standard";
            buttonTool22.SharedPropsInternal.Caption = "Pending Approval";
            buttonTool22.SharedPropsInternal.Category = "Standard";
            buttonTool23.SharedPropsInternal.Caption = "Rule Definition";
            buttonTool23.SharedPropsInternal.Category = "Standard";

            appearance17.Image = 14;
            portfolioManagement2.SharedPropsInternal.AppearancesSmall.Appearance = appearance17;
            appearance57.Image = 15;
            portfolioManagement2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance57;
            portfolioManagement2.SharedPropsInternal.Caption = "Portfolio Management";
            portfolioManagement2.SharedPropsInternal.ToolTipText = "Portfolio Management";

            appearance18.Image = 16;
            riskAnalysis2.SharedPropsInternal.AppearancesSmall.Appearance = appearance18;
            appearance58.Image = 17;
            riskAnalysis2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance58;
            riskAnalysis2.SharedPropsInternal.Caption = "Risk Analysis";
            riskAnalysis2.SharedPropsInternal.ToolTipText = "Risk Analysis";

            appearance19.Image = 18;
            generalLedger2.SharedPropsInternal.AppearancesSmall.Appearance = appearance19;
            appearance59.Image = 19;
            generalLedger2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance59;
            generalLedger2.SharedPropsInternal.Caption = "General Ledger";

            appearance20.Image = 20;
            securityMaster2.SharedPropsInternal.AppearancesSmall.Appearance = appearance20;
            appearance60.Image = 21;
            securityMaster2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance60;
            securityMaster2.SharedPropsInternal.Caption = "Security Master";

            appearance21.Image = 22;
            thirdPartyManager2.SharedPropsInternal.AppearancesSmall.Appearance = appearance21;
            appearance61.Image = 23;
            thirdPartyManager2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance61;
            thirdPartyManager2.SharedPropsInternal.Caption = "Third Party Manager";

            reports2.DropDownArrowStyle = Infragistics.Win.UltraWinToolbars.DropDownArrowStyle.Standard;
            appearance22.Image = 24;
            appearance22.ForeColor = System.Drawing.Color.Black;
            reports2.SharedPropsInternal.AppearancesSmall.Appearance = appearance22;
            appearance62.Image = 25;
            reports2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance62;
            reports2.SharedPropsInternal.Caption = "Reports";
            reports2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageOnlyOnToolbars;
            reports2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            blotterExecReport1,
            auditTrail1});
            blotterExecReport2.SharedPropsInternal.Caption = "Blotter/Execution Report";
            blotterExecReport2.SharedPropsInternal.Category = "Standard";
            blotterExecReport2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            auditTrail2.SharedPropsInternal.Caption = "Audit Trail";
            auditTrail2.SharedPropsInternal.Category = "Standard";
            auditTrail2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;

            backOffice2.DropDownArrowStyle = Infragistics.Win.UltraWinToolbars.DropDownArrowStyle.Standard;
            appearance23.Image = 26;
            appearance23.ForeColor = System.Drawing.Color.Black;
            backOffice2.SharedPropsInternal.AppearancesSmall.Appearance = appearance23;
            appearance63.Image = 27;
            backOffice2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance63;
            backOffice2.SharedPropsInternal.Caption = "Back Office";
            backOffice2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageOnlyOnToolbars;
            backOffice2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            dailyValuation1,
            recon1,
            closing1,
            navLock1,
            pricingInputs1,
            dataMapping1,
            import1,
            autoImport1,
            missingTrades1,
            createTransaction1,
            corporateActions1,
            washSale1});
            dailyValuation2.SharedPropsInternal.Caption = "Daily Valuation";
            dailyValuation2.SharedPropsInternal.Category = "Standard";
            recon2.SharedPropsInternal.Caption = "Recon";
            recon2.SharedPropsInternal.Category = "Standard";
            closing2.SharedPropsInternal.Caption = "Closing";
            closing2.SharedPropsInternal.Category = "Standard";
            pricingInputs2.SharedPropsInternal.Caption = "Pricing Inputs";
            pricingInputs2.SharedPropsInternal.Category = "Standard";
            dataMapping2.SharedPropsInternal.Caption = "Data Mapping";
            dataMapping2.SharedPropsInternal.Category = "Standard";
            import2.SharedPropsInternal.Caption = "Import";
            import2.SharedPropsInternal.Category = "Standard";
            autoImport2.SharedPropsInternal.Caption = "Auto Import";
            autoImport2.SharedPropsInternal.Category = "Standard";
            missingTrades2.SharedPropsInternal.Caption = "Missing Trades";
            missingTrades2.SharedPropsInternal.Category = "Standard";
            createTransaction2.SharedPropsInternal.Caption = "Create Transaction";
            createTransaction2.SharedPropsInternal.Category = "Standard";
            corporateActions2.SharedPropsInternal.Caption = "Corporate Actions";
            corporateActions2.SharedPropsInternal.Category = "Standard";
            washSale2.SharedPropsInternal.Caption = "Wash Sale On-boarding";
            washSale2.SharedPropsInternal.Category = "Standard";
            navLock2.SharedPropsInternal.Caption = "NAV Lock";
            navLock2.SharedPropsInternal.Category = "Standard";

            tools2.DropDownArrowStyle = Infragistics.Win.UltraWinToolbars.DropDownArrowStyle.Standard;
            appearance24.Image = 28;
            appearance24.ForeColor = System.Drawing.Color.Black;
            tools2.SharedPropsInternal.AppearancesSmall.Appearance = appearance24;
            appearance64.Image = 29;
            tools2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance64;
            tools2.SharedPropsInternal.Caption = "Tools";
            tools2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageOnlyOnToolbars;
            tools2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            preferences1,
            shortLocate1,
            brokerConnections1,
            reloadSettings1,
            middlewareManager1,
            zeroPositionAlert1,
            qualityChecker1});
            preferences2.SharedPropsInternal.Caption = "Preferences";
            preferences2.SharedPropsInternal.Category = "Standard";
            shortLocate2.SharedPropsInternal.Caption = "Short Locate";
            shortLocate2.SharedPropsInternal.Category = "Standard";
            brokerConnections2.SharedPropsInternal.Caption = "Broker Connections";
            brokerConnections2.SharedPropsInternal.Category = "Standard";
            reloadSettings2.SharedPropsInternal.Caption = "Reload Settings";
            reloadSettings2.SharedPropsInternal.Category = "Standard";
            middlewareManager2.SharedPropsInternal.Caption = "Middleware Manager";
            middlewareManager2.SharedPropsInternal.Category = "Standard";
            zeroPositionAlert2.SharedPropsInternal.Caption = "Zero Position Alert";
            zeroPositionAlert2.SharedPropsInternal.Category = "Standard";
            qualityChecker2.SharedPropsInternal.Caption = "Quality Checker";
            qualityChecker2.SharedPropsInternal.Category = "Standard";

            helpAndSupport2.DropDownArrowStyle = Infragistics.Win.UltraWinToolbars.DropDownArrowStyle.Standard;
            appearance25.Image = 30;
            appearance25.ForeColor = System.Drawing.Color.Black;
            helpAndSupport2.SharedPropsInternal.AppearancesSmall.Appearance = appearance25;
            appearance65.Image = 31;
            helpAndSupport2.SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearance65;
            helpAndSupport2.SharedPropsInternal.Caption = "Help and Support";
            helpAndSupport2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageOnlyOnToolbars;
            helpAndSupport2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            moduleShortcuts1,
            aboutNirvana1,
            disclaimer1});
            moduleShortcuts2.SharedPropsInternal.Caption = "Module Shortcuts";
            moduleShortcuts2.SharedPropsInternal.Category = "Standard";
            aboutNirvana2.SharedPropsInternal.Caption = "Software Details";
            aboutNirvana2.SharedPropsInternal.Category = "Standard";
            disclaimer2.SharedPropsInternal.Caption = "Disclaimer";
            disclaimer2.SharedPropsInternal.Category = "Standard";

            this.PranaMainToolBar.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            blotter2,
            tradingTicket2,
            tradingTicket4,
            tradingTicketButton2,
            quickTradingTicket2,
            quickTradingTicket4,
            quickTradingTicket6,
            quickTradingTicket8,
            quickTradingTicket10,
            quickTradingTicket12,
            quickTradingTicket14,
            quickTradingTicket16,
            quickTradingTicket18,
            quickTradingTicket20,

            allocation1,
            portfolioManagement2,
            riskAnalysis2,
            generalLedger2,
            securityMaster2,
            thirdPartyManager2,
            rebalancer1,
            pTT1,
            compliance2,
            buttonTool21,
            buttonTool22,
            buttonTool23,
            watchlist2,
            watchlistButton2,
            optionChain2,
            watchlistButton4,
            reports2,
            blotterExecReport2,
            auditTrail2,
            backOffice2,
            dailyValuation2,
            recon2,
            closing2,
            navLock2,
            pricingInputs2,
            dataMapping2,
            import2,
            autoImport2,
            missingTrades2,
            createTransaction2,
            corporateActions2,
            washSale2,
            tools2,
            preferences2,
            shortLocate2,
            brokerConnections2,
            reloadSettings2,
            middlewareManager2,
            zeroPositionAlert2,
            qualityChecker2,
            helpAndSupport2,
            moduleShortcuts2,
            aboutNirvana2,
            disclaimer2
            });
            this.PranaMainToolBar.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.PranaMainToolBar_ToolClick);
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.lblConnectionStatus);
            this.ultraPanel2.ClientArea.Controls.Add(this.lblPricingServer);
            this.ultraPanel2.ClientArea.Controls.Add(this.lblExPNLConnection);
            this.ultraPanel2.ClientArea.Controls.Add(this.btnLiveFeedConnect);
            this.ultraPanel2.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Left);
            this.ultraPanel2.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Right);
            this.ultraPanel2.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Bottom);
            this.ultraPanel2.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Top);
            this.inboxControlStyler1.SetStyleSettings(this.ultraPanel2.ClientArea, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel2.Location = new System.Drawing.Point(8, 62);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.TabIndex = 119;
            // 
            // _ClientArea_Toolbars_Dock_Area_Left
            // 
            this._ClientArea_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._ClientArea_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ClientArea_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 34);
            this._ClientArea_Toolbars_Dock_Area_Left.Name = "_ClientArea_Toolbars_Dock_Area_Left";
            this._ClientArea_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 1);
            this._ClientArea_Toolbars_Dock_Area_Left.ToolbarsManager = this.PranaMainToolBar;
            // 
            // _ClientArea_Toolbars_Dock_Area_Right
            // 
            this._ClientArea_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._ClientArea_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ClientArea_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1084, 34);
            this._ClientArea_Toolbars_Dock_Area_Right.Name = "_ClientArea_Toolbars_Dock_Area_Right";
            this._ClientArea_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 1);
            this._ClientArea_Toolbars_Dock_Area_Right.ToolbarsManager = this.PranaMainToolBar;
            // 
            // _ClientArea_Toolbars_Dock_Area_Bottom
            // 
            this._ClientArea_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._ClientArea_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ClientArea_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 35);
            this._ClientArea_Toolbars_Dock_Area_Bottom.Name = "_ClientArea_Toolbars_Dock_Area_Bottom";
            this._ClientArea_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1084, 0);
            this._ClientArea_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.PranaMainToolBar;
            // 
            // _ClientArea_Toolbars_Dock_Area_Top
            // 
            this._ClientArea_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._ClientArea_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ClientArea_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ClientArea_Toolbars_Dock_Area_Top.Name = "_ClientArea_Toolbars_Dock_Area_Top";
            this._ClientArea_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1084, 34);
            this._ClientArea_Toolbars_Dock_Area_Top.ToolbarsManager = this.PranaMainToolBar;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(8, 32);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1084, 30);
            this.ultraPanel1.TabIndex = 113;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            this.ultraFormManager1.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.UltraFormManager1_MouseEnterElement);
            this.ultraFormManager1.MouseLeaveElement += new Infragistics.Win.UIElementEventHandler(this.ultraFormManager1_MouseLeaveElement);
            // 
            // _PranaMain_UltraFormManager_Dock_Area_Left
            // 
            this._PranaMain_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PranaMain_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PranaMain_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._PranaMain_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PranaMain_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._PranaMain_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._PranaMain_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._PranaMain_UltraFormManager_Dock_Area_Left.Name = "_PranaMain_UltraFormManager_Dock_Area_Left";
            this._PranaMain_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 65);
            // 
            // _PranaMain_UltraFormManager_Dock_Area_Right
            // 
            this._PranaMain_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PranaMain_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PranaMain_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._PranaMain_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PranaMain_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._PranaMain_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._PranaMain_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1092, 32);
            this._PranaMain_UltraFormManager_Dock_Area_Right.Name = "_PranaMain_UltraFormManager_Dock_Area_Right";
            this._PranaMain_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 65);
            // 
            // _PranaMain_UltraFormManager_Dock_Area_Top
            // 
            this._PranaMain_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PranaMain_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PranaMain_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._PranaMain_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PranaMain_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._PranaMain_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._PranaMain_UltraFormManager_Dock_Area_Top.Name = "_PranaMain_UltraFormManager_Dock_Area_Top";
            this._PranaMain_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1100, 32);
            // 
            // _PranaMain_UltraFormManager_Dock_Area_Bottom
            // 
            this._PranaMain_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PranaMain_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PranaMain_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._PranaMain_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PranaMain_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._PranaMain_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._PranaMain_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 97);
            this._PranaMain_UltraFormManager_Dock_Area_Bottom.Name = "_PranaMain_UltraFormManager_Dock_Area_Bottom";
            this._PranaMain_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1100, 8);
            // 
            // PranaIconListThemesOff
            // 
            this.PranaIconListThemesOff.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("PranaIconListThemesOff.ImageStream")));
            this.PranaIconListThemesOff.TransparentColor = System.Drawing.Color.Transparent;
            this.PranaIconListThemesOff.Images.SetKeyName(0, "");
            this.PranaIconListThemesOff.Images.SetKeyName(1, "preferences.png");
            this.PranaIconListThemesOff.Images.SetKeyName(2, "");
            this.PranaIconListThemesOff.Images.SetKeyName(3, "blotter.png");
            this.PranaIconListThemesOff.Images.SetKeyName(4, "manual-trading-ticket.png");
            this.PranaIconListThemesOff.Images.SetKeyName(5, "watchlist.png");
            this.PranaIconListThemesOff.Images.SetKeyName(6, "");
            this.PranaIconListThemesOff.Images.SetKeyName(7, "");
            this.PranaIconListThemesOff.Images.SetKeyName(8, "");
            this.PranaIconListThemesOff.Images.SetKeyName(9, "allocation-module.png");
            this.PranaIconListThemesOff.Images.SetKeyName(10, "watchlist-hover.png");
            this.PranaIconListThemesOff.Images.SetKeyName(11, "manual-trading-ticket-hover.png");
            this.PranaIconListThemesOff.Images.SetKeyName(12, "live-trading-ticket-hover.png");
            this.PranaIconListThemesOff.Images.SetKeyName(13, "blotter-hover.png");
            this.PranaIconListThemesOff.Images.SetKeyName(14, "allocation-module-hover.png");
            this.PranaIconListThemesOff.Images.SetKeyName(15, "portfolio-management-hover.png");
            this.PranaIconListThemesOff.Images.SetKeyName(16, "preferences-hover.png");
            this.PranaIconListThemesOff.Images.SetKeyName(17, "live-trading-ticket.png");
            this.PranaIconListThemesOff.Images.SetKeyName(18, "portfolio-management.png");
            // 
            // appStylistRuntime1
            // 
            this.appStylistRuntime1.StyleLibraryName = "DefaultTheme";
            // 
            // PranaMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(1100, 105);
            this.Controls.Add(this.ultraPanel2);
            //this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this._PranaMain_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._PranaMain_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._PranaMain_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._PranaMain_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.ControlBox = false;
            this.MaximumSize = new System.Drawing.Size(1100, 105);
            this.MinimumSize = new System.Drawing.Size(1100, 105);
            this.Name = "PranaMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Nirvana";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PranaMain_FormClosing);
            this.Load += new System.EventHandler(this.PranaMain_Load);
            this.Resize += new System.EventHandler(this.PranaMain_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.PranaMainToolBar)).EndInit();
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// MenuDisclaimer_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LaunchDisclaimer()
        {
            try
            {
                if (frmDisclaimer == null)
                {
                    frmDisclaimer = new NirvanaDisclaimerUI();
                    frmDisclaimer.Owner = this;
                    frmDisclaimer.ShowInTaskbar = false;
                    frmDisclaimer.Closing += new CancelEventHandler(frmDisclaimer_Closing);
                    SetFromLayoutDetail(frmDisclaimer);
                }
                frmDisclaimer.Show();
                BringFormToFront(frmDisclaimer, GetFormNewLocation(null));
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

        private void frmDisclaimer_Closing(object sender, CancelEventArgs e)
        {
            frmDisclaimer = null;
        }

        #endregion

        IAuditTrailUI _auditTrailInstance = null;

        /// <summary>
        /// Launches Audit UI according to parameters received
        /// </summary>
        /// <param name="typeAndObjectForAudit">KeyValuePair of string,object[] used to pass groupids, filters etc to get audit trail</param>
        /// <param name="newFormLocation">location of form where to draw the new form</param>
        private void LaunchAuditUI(KeyValuePair<string, object[]> typeAndObjectForAudit, Point newFormLocation)
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.AUDIT_TRAIL_MODULE, PranaModules.AUDIT_TRAIL_MODULE))
                {
                    if (_auditTrailInstance == null)
                    {
                        DynamicClass formToLoad;
                        formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails[PranaModules.AUDIT_TRAIL_MODULE];
                        Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);
                        Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);
                        _auditTrailInstance = (IAuditTrailUI)Activator.CreateInstance(typeToLoad);
                        _auditTrailInstance.FormClosed += new EventHandler(_auditTrailInstance_FormClosed);
                        tradeAuditUIForm = _auditTrailInstance.Reference();
                        _auditTrailInstance.LoginUser = loginUser;
                        tradeAuditUIForm.Owner = this;
                        tradeAuditUIForm.ShowInTaskbar = false;
                        tradeAuditUIForm.StartPosition = FormStartPosition.Manual;
                        SetFromLayoutDetail(tradeAuditUIForm);
                        tradeAuditUIForm.Text = "Audit Trail";
                    }
                    BringFormToFront(tradeAuditUIForm, newFormLocation);
                    if (typeAndObjectForAudit.Equals(default(KeyValuePair<string, object[]>)))
                    {
                        typeAndObjectForAudit = new KeyValuePair<string, object[]>("none", null);
                    }
                    switch (typeAndObjectForAudit.Key)
                    {
                        case "none": break;
                        case "filters":

                            DateTime from = (DateTime)typeAndObjectForAudit.Value[0];
                            DateTime till = (DateTime)typeAndObjectForAudit.Value[1];
                            string symbol = (string)typeAndObjectForAudit.Value[2];
                            string accountID = (string)typeAndObjectForAudit.Value[3];
                            string orderSideTagValue = (string)typeAndObjectForAudit.Value[4];

                            var auditFilterParams = new AuditTrailFilterParams()
                            {
                                AccountIDs = accountID,
                                FromDate = from,
                                ToDate = till,
                                Symbol = symbol,
                                OrderSides = orderSideTagValue
                            };
                            _auditTrailInstance.GetAndBindAuditUIDataForFilters(auditFilterParams);
                            break;
                        case "groupids":
                            List<string> groupIds = (List<string>)typeAndObjectForAudit.Value[0];
                            _auditTrailInstance.GetAndBindAuditUIDataForGroupIds(groupIds);
                            break;
                        default: break;
                    }
                    tradeAuditUIForm.Show();
                    tradeAuditUIForm.Activate();
                }
            }
            catch (Exception ex)
            {
                InformationMessageBox.Display(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        void _auditTrailInstance_FormClosed(object sender, EventArgs e)
        {
            _auditTrailInstance.FormClosed -= new EventHandler(_auditTrailInstance_FormClosed);
            _auditTrailInstance = null;
            tradeAuditUIForm = null;
        }

        void allInstance_GetAuditClick(object sender, EventArgs e)
        {
            KeyValuePair<string, object[]> typeAndListForAudit = (KeyValuePair<string, object[]>)((LaunchFormEventArgs)e).Params;
            Form parentForm = sender as Form;
            LaunchAuditUI(typeAndListForAudit, GetFormNewLocation(parentForm));
        }

        #region Form Events

        /// <summary>
        /// For Setting Connection Status of Server // sets Login and Logut Menu Items status
        /// </summary>
        public void SetServerConnectionStatus(ConnectionProperties con)
        {
            try
            {
                SetDisplayCallback mi = new SetDisplayCallback(SetServerConnectionStatus);
                if (UIValidation.GetInstance().validate(lblConnectionStatus))
                {
                    if (lblConnectionStatus.InvokeRequired)
                    {
                        this.BeginInvoke(mi, new Object[] { con });
                    }
                    else
                    {
                        //ProxyBase<T>.
                        PranaInternalConstants.ConnectionStatus connectionState = _tradeCommManager.ConnectionStatus;
                        // ConnectionMgr.SetConnectionStatus(con, connectionState);
                        if (connectionState == PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            lblConnectionStatus.Appearance.ImageBackground = global::Prana.Properties.Resources.NirvanaMainIndicatorGreen;
                        }
                        else if (connectionState == PranaInternalConstants.ConnectionStatus.DISCONNECTED && !this.IsDisposed)
                        {
                            lblConnectionStatus.Appearance.ImageBackground = global::Prana.Properties.Resources.NirvanaMainIndicatorRed;
                        }
                        else if (!this.IsDisposed)
                        {
                            lblConnectionStatus.Appearance.ImageBackground = global::Prana.Properties.Resources.NirvanaMainIndicatorRed;
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
        /// For Setting Connection Status of Exposure and PNL Service
        /// </summary>
        private void SetExPnlServiceConnectionStatus()
        {
            SetDisplayWithoutArgsCallback mi = new SetDisplayWithoutArgsCallback(SetExPnlServiceConnectionStatus);
            try
            {
                if (UIValidation.GetInstance().validate(lblExPNLConnection))
                {
                    if (lblExPNLConnection.InvokeRequired)
                    {
                        this.BeginInvoke(mi, null);
                    }
                    else
                    {
                        if (_exPNLCommManagerInstance != null)
                        {
                            PranaInternalConstants.ConnectionStatus connectionState = _exPNLCommManagerInstance.ConnectionStatus;
                            switch (connectionState)
                            {
                                case PranaInternalConstants.ConnectionStatus.CONNECTED:
                                    lblExPNLConnection.Appearance.ImageBackground = global::Prana.Properties.Resources.NirvanaMainIndicatorGreen;
                                    if (OpenModulesList.Contains(PranaModules.PERCENT_TRADING_TOOL))
                                    {
                                        if (percentTradingToolWindow != null)
                                        {
                                            percentTradingToolWindow.PercentTradingToolViewModel.TryExpnlServiceConnectCommand.Execute(null);
                                        }
                                    }
                                    break;

                                case PranaInternalConstants.ConnectionStatus.DISCONNECTED:
                                    lblExPNLConnection.Appearance.ImageBackground = global::Prana.Properties.Resources.NirvanaMainIndicatorRed;
                                    break;

                                case PranaInternalConstants.ConnectionStatus.NOSERVER:
                                    lblExPNLConnection.Appearance.ImageBackground = global::Prana.Properties.Resources.NirvanaMainIndicatorRed;
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

        #endregion

        #region Common Functions
        private Dictionary<string, Form> activePlugInForms = new Dictionary<string, Form>();
        private void LoadApplicationModuleDetails()
        {
            try
            {
                Hashtable availableModulesDetails = new Hashtable();
                Hashtable availableTools = new Hashtable();
                Hashtable PlugIns = new Hashtable();
                NameValueCollection moduleConfigDetails = ConfigurationHelper.Instance.LoadSectionBySectionName("availableModules");
                NameValueCollection toolConfigDetails = ConfigurationHelper.Instance.LoadSectionBySectionName("AvailableTools");
                NameValueCollection plugInsConfigDetails = ConfigurationHelper.Instance.LoadSectionBySectionName("PlugIns");

                string sLocation = string.Empty;
                string sDescription = string.Empty;
                string sType = string.Empty;
                string sValue = string.Empty;
                string sPrefControlType = string.Empty;

                #region ModuleConfigs
                if (moduleConfigDetails == null)
                {
                    throw new Exception("Error in accessing the modules configuration");
                }

                for (int iIndex = 0, count = moduleConfigDetails.Count - 1; iIndex <= count; iIndex++)
                {
                    sDescription = moduleConfigDetails.GetKey(iIndex);
                    sValue = moduleConfigDetails[sDescription];

                    string[] moduleDetailsBreakUp = sValue.Split('~');
                    sLocation = Application.StartupPath + "\\" + moduleDetailsBreakUp[0];
                    sType = moduleDetailsBreakUp[1];
                    sPrefControlType = moduleDetailsBreakUp[2];
                    availableModulesDetails.Add(sDescription, new DynamicClass(sLocation, sType, sPrefControlType, sDescription));
                }
                #endregion ModuleConfigs

                #region ToolsConfigs
                for (int iIndex = 0; iIndex <= toolConfigDetails.Count - 1; iIndex++)
                {
                    string key = toolConfigDetails.GetKey(iIndex);
                    string value = toolConfigDetails[key];
                    string[] toolDetailsBreakUp = value.Split('~');

                    if (!key.Equals("Divider") && key.Contains("Divider"))
                    {
                    }
                    else
                    {
                        string location = Application.StartupPath + "\\" + toolDetailsBreakUp[2];
                        string type = toolDetailsBreakUp[3];
                        availableTools.Add(key, new DynamicClass(location, type, null, key));
                    }
                }
                #endregion ToolsConfigs

                #region PlugInsConfigs
                PlugInMenuItemsPairs = new Dictionary<string, string>();
                for (int iIndex = 0; iIndex <= plugInsConfigDetails.Count - 1; iIndex++)
                {
                    string plugInName = plugInsConfigDetails.GetKey(iIndex);
                    string plugInValue = plugInsConfigDetails[plugInName];
                    string[] plugInsDetailsBreakUp = plugInValue.Split('~');

                    string strPlugInType = plugInsDetailsBreakUp[0];
                    Assembly plugInAssembly = Assembly.LoadFrom(Application.StartupPath + "\\" + plugInsDetailsBreakUp[2]);
                    IPlugin objPlugIn = (IPlugin)plugInAssembly.CreateInstance(plugInsDetailsBreakUp[3]);

                    if (strPlugInType.Equals(Prana.Global.ApplicationConstants.PlugInType.Menu.ToString()))
                    {
                        Dictionary<string, string> strMenus = objPlugIn.Menus();
                        foreach (KeyValuePair<string, string> strMenu in strMenus)
                        {
                            string name = strMenu.Key.Replace(" ", "");
                            PlugInMenuItemsPairs.Add(name, name + "~" + plugInsDetailsBreakUp[1]);
                        }
                    }
                    PlugIns.Add(plugInName, new KeyValuePair<string, IPlugin>(strPlugInType, objPlugIn));
                }
                activePlugInForms = new Dictionary<string, Form>();
                #endregion PlugInsConfigs

                //Bharat Kumar Jangir (27 January 2014)
                //Setting Config values as Global values
                ModuleManager.AvailableModulesDetails = availableModulesDetails;
                ModuleManager.AvailableTools = availableTools;
                ModuleManager.PlugIns = PlugIns;
            }
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
        /// Disables the hide not permitted modules.
        /// </summary>
        private void DisableHideNotPermittedModules()
        {
            try
            {
                foreach (var buttonTool in PranaMainToolBar.Tools)
                {
                    string moduleName = buttonTool.SharedPropsInternal.Caption;
                    bool isAlwaysPermitted = false;
                    switch (moduleName)
                    {
                        case ApplicationConstants.CONST_TOOLS:
                        case ApplicationConstants.CONST_MODULE_SHORTCUTS:
                        case ApplicationConstants.CONST_DISCLAIMER:
                        case ApplicationConstants.CONST_HELP_AND_SUPPORT:
                        case ApplicationConstants.CONST_ABOUT_NIRVANA:
                        case ApplicationConstants.CONST_PREFERENCES:
                        case ApplicationConstants.CONST_BROKERCONNECTIONS:
                        case ApplicationConstants.CONST_RELOADSETTINGS:
                            isAlwaysPermitted = true;
                            break;
                        case ApplicationConstants.CONST_ALERT_HISTORY:
                            moduleName = ApplicationConstants.CONST_CE_ALERT_HISTORY;
                            break;
                        case ApplicationConstants.CONST_PENDING_APPROVAL:
                            moduleName = ApplicationConstants.CONST_CE_PENDING_APPROVAL;
                            break;
                        case ApplicationConstants.CONST_RULE_DEFINITION:
                            moduleName = ApplicationConstants.CONST_CE_RULE_DEFINITION;
                            break;
                        case ApplicationConstants.CONST_RECON:
                            moduleName = ApplicationConstants.CONST_RECONCILIATION;
                            break;
                        case ApplicationConstants.CONST_CLOSING:
                            moduleName = ApplicationConstants.CONST_CLOSEPOSITIONS;
                            break;
                        case ApplicationConstants.CONST_IMPORT:
                            moduleName = ApplicationConstants.CONST_IMPORTDATA;
                            break;
                        case ApplicationConstants.CONST_AUTOIMPORT:
                            moduleName = ApplicationConstants.CONST_AUTOIMPORTDATA;
                            break;
                    }
                    if (!(isAlwaysPermitted || ModuleManager.CheckModulePermissioning(moduleName, moduleName)))
                    {
                        buttonTool.SharedProps.Visible = false;
                    }
                }
                bool isShowReport = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsShowReportMenuOnToolBar"));
                bool isQTTPremitted = ModuleManager.CheckModulePermissioning(PranaModules.QUICK_TRADING_TICKET_MODULE, PranaModules.QUICK_TRADING_TICKET_MODULE);
                bool isOptionChainPremitted = ModuleManager.CheckModulePermissioning(PranaModules.OPTIONCHAIN_MODULE, PranaModules.OPTIONCHAIN_MODULE);
                /* Issue - Button is reflecting with blank dropdown
                 * https://jira.nirvanasolutions.com:8443/browse/PRANA-38478 */
                foreach (var buttonTool in PranaMainToolBar.Tools)
                {
                    string moduleName = buttonTool.SharedPropsInternal.Caption;
                    if (moduleName.Equals(ApplicationConstants.CONST_REPORTS) || moduleName.Equals(ApplicationConstants.CONST_BACK_OFFICE))
                    {
                        if (moduleName.Equals(ApplicationConstants.CONST_REPORTS) && isShowReport)
                            buttonTool.SharedProps.Visible = true;
                        else
                        {
                            buttonTool.SharedProps.Visible = false;
                            foreach (Infragistics.Win.UltraWinToolbars.ToolBase module in ((Infragistics.Win.UltraWinToolbars.PopupMenuTool)buttonTool).Tools.All)
                            {
                                if (module.SharedProps.Visible)
                                {
                                    buttonTool.SharedProps.Visible = true;
                                    break;
                                }
                            }
                        }
                    }
                    else if (moduleName.Equals(PranaModules.TRADING_TICKET_MODULE))
                    {
                        if (buttonTool.SharedProps.Visible)
                        {
                            if (buttonTool.Key.Equals(PranaModules.TRADING_TICKET_MODULE))
                                buttonTool.SharedProps.Visible = !isQTTPremitted;
                            else
                                buttonTool.SharedProps.Visible = isQTTPremitted;
                        }
                    }
                    else if (moduleName.Equals(PranaModules.WATCHLIST_MODULE))
                    {
                        if (buttonTool.SharedProps.Visible)
                        {
                            if (buttonTool.Key.Equals(PranaModules.WATCHLIST_MODULE))
                                buttonTool.SharedProps.Visible = !isOptionChainPremitted;
                            else
                                buttonTool.SharedProps.Visible = isOptionChainPremitted;
                        }
                    }
                    else if (moduleName.StartsWith(ApplicationConstants.CONST_QUICKTT_PREFIX))
                    {
                        //Length of QuickTradingTicket = 18
                        int index = Convert.ToInt32(buttonTool.Key.Substring(18));
                        buttonTool.SharedProps.Visible = isQTTPremitted && index <= CachedDataManager.GetInstance.PermissibleQuickTTInstances;
                    }
                    else if (buttonTool.Key.Equals(ApplicationConstants.CONST_COMPLIANCE_MODULE))
                    {
                        buttonTool.SharedProps.Visible = false;
                        foreach (Infragistics.Win.UltraWinToolbars.ToolBase module in ((Infragistics.Win.UltraWinToolbars.PopupMenuTool)buttonTool).Tools.All)
                        {
                            if (module.SharedProps.Visible)
                            {
                                dictComplianceTabs[module.Key] = true;
                                buttonTool.SharedProps.Visible = true;
                            }
                        }
                    }
                }
                if ((CachedDataManager.CompanyMarketDataProvider != MarketDataProvider.FactSet) ||
                    (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet &&
                    (CachedDataManager.CompanyFactSetContractType == FactSetContractType.Reseller && (string.IsNullOrWhiteSpace(loginUser.FactSetUsernameAndSerialNumber)) || CachedDataManager.CompanyFactSetContractType == FactSetContractType.ChannelPartner)))
                    btnLiveFeedConnect.Visible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        Form LaunchPlugin(ToolStripMenuItem plugInMnuItem)
        {
            Form plugInForm = null;
            try
            {
                if (null != loginUser && !activePlugInForms.ContainsKey(plugInMnuItem.Text))
                {
                    plugInForm = ((KeyValuePair<string, IPlugin>)ModuleManager.PlugIns[plugInMnuItem.Tag]).Value.Execute(plugInMnuItem.Name,
                                                                                                                AMSConnectionString,
                                                                                                                loginUser.FirstName + " " + loginUser.LastName,
                                                                                                                loginUser.CompanyUserID);
                    if (null != plugInForm)
                    {
                        plugInForm.Owner = this;
                        plugInForm.ShowInTaskbar = false;
                        plugInForm.Name = plugInMnuItem.Name;
                        plugInForm.Tag = plugInMnuItem.Tag;
                        if (!activePlugInForms.ContainsKey(plugInForm.Text))
                        {
                            activePlugInForms.Add(plugInMnuItem.Text, plugInForm);
                        }
                        ((KeyValuePair<string, IPlugin>)ModuleManager.PlugIns[plugInMnuItem.Tag]).Value.FormClosed += new IPlugin.FormClosedEventHandler(PluginFormClosed);
                        ((KeyValuePair<string, IPlugin>)ModuleManager.PlugIns[plugInMnuItem.Tag]).Value.VisibleChanged += new IPlugin.VisibleChangedEventHandler(PluginFormVisibleChanged);
                        ((KeyValuePair<string, IPlugin>)ModuleManager.PlugIns[plugInMnuItem.Tag]).Value.Shown += new IPlugin.ShownEventHandler(PluginFormVisibleChanged);

                        SetFromLayoutDetail(plugInForm);
                        BringFormToFront(plugInForm, GetFormNewLocation(null));
                    }
                }
                else
                {
                    if (activePlugInForms.ContainsKey(plugInMnuItem.Text))
                    {
                        plugInForm = activePlugInForms[plugInMnuItem.Text];
                        BringFormToFront(plugInForm, GetFormNewLocation(null));
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
            return plugInForm;
        }

        void PluginFormClosed(object sender, FormClosedEventArgs fe)
        {
            try
            {
                Form frm = (Form)sender;
                if (activePlugInForms.ContainsKey(frm.Text))
                {
                    activePlugInForms.Remove(frm.Text);
                    ((KeyValuePair<string, IPlugin>)ModuleManager.PlugIns[frm.Tag]).Value.FormClosed -= new IPlugin.FormClosedEventHandler(PluginFormClosed);
                    ((KeyValuePair<string, IPlugin>)ModuleManager.PlugIns[frm.Tag]).Value.VisibleChanged -= new IPlugin.VisibleChangedEventHandler(PluginFormVisibleChanged);
                    ((KeyValuePair<string, IPlugin>)ModuleManager.PlugIns[frm.Tag]).Value.Shown -= new IPlugin.ShownEventHandler(PluginFormVisibleChanged);
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

        void PluginFormVisibleChanged(object sender, EventArgs fe)
        {
            try
            {
                Form frm = (Form)sender;
                if (frm.Visible)
                {
                    if (!activePlugInForms.ContainsKey(frm.Text))
                    {
                        activePlugInForms.Add(frm.Text, frm);
                    }
                }
                else
                {
                    if (activePlugInForms.ContainsKey(frm.Text))
                    {
                        activePlugInForms.Remove(frm.Text);
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

        #region Launch Form Methods
        IBlotter blotternewInstance = null;
        private void LaunchBlotterFormNew()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.BLOTTER_MODULE, PranaModules.BLOTTER_MODULE))
                {
                    if (blotterForm == null)
                    {
                        if (!_isBlotterFormLaunching)
                        {
                            _isBlotterFormLaunching = true;
                            DynamicClass formToLoad;
                            formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails[PranaModules.BLOTTER_MODULE];
                            Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);
                            Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);
                            blotternewInstance = null;
                            blotternewInstance = (IBlotter)Activator.CreateInstance(typeToLoad);
                            blotternewInstance.SecurityMaster = _secMasterServices;
                            blotternewInstance.LoginUser = loginUser;
                            blotternewInstance.LaunchSecurityMasterForm += new EventHandler(pluggaleToolLaunch_LaunchForm);
                            blotternewInstance.InitControl();
                            blotterForm = blotternewInstance.Reference();
                            blotternewInstance.TradeClick += new EventHandler(blotternewInstance_TradeClick);
                            blotternewInstance.HighlightSymbolFromBlotter += _blotter_HighlightSymbolFromBlotter;

                            blotternewInstance.ReplaceOrEditOrderClicked += new EventHandler(blotternewInstance_ReplaceOrEditOrderClicked);
                            blotternewInstance.BlotterClosed += new EventHandler(blotterInstance_BlotterClosed);
                            blotternewInstance.LaunchPreferences += new EventHandler(blotterInstance_LaunchPreferences);
                            blotternewInstance.GoToAllocationClicked += HandleGoToAllocationClick;

                            blotterForm.ShowInTaskbar = false;
                            blotterForm.Owner = this;
                            _isBlotterFormLaunching = false;
                            blotternewInstance.WireEvents();
                            blotterForm.Show();
                            SetFromLayoutDetail(blotterForm);
                            LoadLayoutForBlotterOnBlotterThread();
                        }
                    }
                    else
                    {
                        //BringFormToFront(blotterForm, GetFormNewLocation(null));
                        //PRANA-20466
                        BringFormToFront(blotterForm, new Point(blotterForm.RestoreBounds.X, blotterForm.RestoreBounds.Y));
                    }
                }
            }
            catch (Exception ex)
            {
                InformationMessageBox.Display(ex.StackTrace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs{System.String, List{DateTime}}"/> instance containing the event data.</param>
        public delegate void GoToAllocationClickedUIDelegate(object sender, EventArgs<string, DateTime, DateTime> e);

        //To Handle MultiThread case
        bool isOpenAllocation = false;

        /// <summary>
        /// Handles the GoToAllocationClicked event of some modules
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{List{System.String}, List{DateTime}}"/> instance containing the event data.</param>
        void HandleGoToAllocationClick(object sender, EventArgs<string, DateTime, DateTime> e)
        {
            try
            {
                isOpenAllocation = false;
                if (this.InvokeRequired)
                {
                    GoToAllocationClickedUIDelegate goToAllocationClicked = new GoToAllocationClickedUIDelegate(HandleGoToAllocationClick);
                    this.BeginInvoke(goToAllocationClicked, new object[] { sender, e });
                }
                else
                {
                    if (!isOpenAllocation)
                    {
                        if (!OpenModulesList.Contains(PranaModules.ALLOCATION_MODULE))
                            LaunchAllocationFromModules(e);
                        else
                        {
                            DialogResult result = MessageBox.Show(new Form { TopMost = true }, "Do you want to reload the allocation module ?", "Nirvana", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                if (sender != null && sender.ToString().Equals("BatchDetails"))
                                {
                                    allocationClientForm.ApplyFilterAndGetData(e.Value, e.Value2, e.Value3, false);
                                    allocationClientForm.Close();
                                }
                                LaunchAllocationFromModules(e);
                            }
                        }
                        isOpenAllocation = true;
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
        /// Launch Allocation From Modules
        /// </summary>
        /// <param name="e"></param>
        private void LaunchAllocationFromModules(EventArgs<string, DateTime, DateTime> e)
        {
            try
            {
                LaunchAllocationForm();
                if (allocationClientForm != null)
                {
                    AllocationUIFront();
                    allocationClientForm.ApplyFilterAndGetData(e.Value, e.Value2, e.Value3);
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
        /// Allocation UI Front
        /// </summary>
        private void AllocationUIFront()
        {
            try
            {
                if (allocationClientForm.WindowState == System.Windows.WindowState.Minimized)
                    allocationClientForm.WindowState = System.Windows.WindowState.Normal;
                allocationClientForm.Activate();
                if (!_loadingLayout)
                    allocationClientForm.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private void CloseBlotterForm()
        {
            try
            {
                if (UIValidation.GetInstance().validate(blotterForm))
                {
                    if (blotterForm.InvokeRequired)
                    {
                        MethodInvokerVoid mi = new MethodInvokerVoid(CloseBlotterForm);
                        blotterForm.BeginInvoke(mi);
                    }
                    else
                    {
                        blotterForm.Close();
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

        void blotternewInstance_ReplaceOrEditOrderClicked(object sender, EventArgs e)
        {
            try
            {
                LaunchFormEventArgs ea = (LaunchFormEventArgs)e;
                OrderSingle or = ea.Params as OrderSingle;
                OrderBindingList orList = ea.Params as OrderBindingList;
                Form parentForm = sender as Form;
                Point formLocation = GetFormNewLocation(parentForm);
                if (UIValidation.GetInstance().validate(this))
                {
                    if (or != null)
                    {
                        if (this.InvokeRequired)
                        {
                            TradingTicketLaunchHandler handler = new TradingTicketLaunchHandler((order) => LaunchTradingTicketDock(order, TradingTicketParent.Blotter));
                            this.BeginInvoke(handler, or);
                        }
                        else
                        {
                            LaunchTradingTicketDock(or, TradingTicketParent.Blotter);
                        }
                    }
                    else
                    {
                        if (ComplianceCacheManager.GetPreTradeCheck(loginUser.CompanyUserID))
                        {
                            MessageBox.Show("Bulk edit operation is not permitted when compliance is enabled.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (InvokeRequired)
                        {
                            MultiTradingTicketLaunchHandler handler = LaunchMultiTradingTicketDock;
                            BeginInvoke(handler, orList, formLocation, TradingTicketParent.Blotter, true);
                        }
                        else
                        {
                            LaunchMultiTradingTicketDock(orList, formLocation, TradingTicketParent.Blotter, true);
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

        void blotternewInstance_TradeClick(object sender, EventArgs e)
        {
            try
            {
                LaunchFormEventArgs ea = (LaunchFormEventArgs)e;
                OrderSingle or = ea.Params as OrderSingle;
                OrderBindingList orList = ea.Params as OrderBindingList;
                Form parentForm = sender as Form;
                Point formLocation = GetFormNewLocation(parentForm);
                if (UIValidation.GetInstance().validate(this))
                {
                    if (or != null)
                    {
                        if (this.InvokeRequired)
                        {
                            TradingTicketLaunchHandler handler = new TradingTicketLaunchHandler((order) => LaunchTradingTicketDock(order, TradingTicketParent.Blotter));
                            this.BeginInvoke(handler, or);
                        }
                        else
                        {
                            LaunchTradingTicketDock(or, TradingTicketParent.Blotter);
                        }
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            MultiTradingTicketLaunchHandler handler = LaunchMultiTradingTicketDock;
                            BeginInvoke(handler, orList, formLocation, TradingTicketParent.Blotter);
                        }
                        else
                        {
                            LaunchMultiTradingTicketDock(orList, formLocation, TradingTicketParent.Blotter);
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
        ///  Check Availability for Trading Ticket Module in availableModulesDetails.
        ///  Launch Trading Ticket Form
        /// </summary>

        AllocationClient allocationClientForm = null;
        public void LaunchAllocationForm()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.ALLOCATION_MODULE, PranaModules.ALLOCATION_MODULE))
                {
                    if (!OpenModulesList.Contains(PranaModules.ALLOCATION_MODULE))
                    {
                        OpenModulesList.Add(PranaModules.ALLOCATION_MODULE);

                        allocationClientForm = new AllocationClient();
                        allocationClientForm.Closed += allocationClientForm_Closed;
                        if (allocationClientForm != null)
                        {
                            allocationClientForm.GenrateCashTransaction += new EventHandler(allocationInstance_GenrateCashTransaction);
                            allocationClientForm.LoadCloseTradeUIFromAllocation += new EventHandler<EventArgs<AllocationGroup>>(allocationInstance_loadCloseTradeUIFromAllocation);
                            allocationClientForm.LoadSymbolLookUpUIFromAllocation += new EventHandler<EventArgs<string>>(allocationInstance_loadSymbolLookUpUIFromAllocation);
                            allocationClientForm.GetAuditClick += new EventHandler(allInstance_GetAuditClick);
                            allocationClientForm.AllocationDataChange += new EventHandler<EventArgs<bool>>(allocationInstance_allocationDataChange);

                            WindowInteropHelper helper = new WindowInteropHelper(allocationClientForm);
                            helper.Owner = (IntPtr)this.Handle;
                        }
                        ElementHost.EnableModelessKeyboardInterop(allocationClientForm);
                        allocationClientForm.ShowInTaskbar = false;
                        allocationClientForm.Show();
                        SetLayoutAllocationClientForm();
                    }
                    else
                    {
                        if (allocationClientForm != null)
                            AllocationUIFront();
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
        /// Handles the Closed event of the allocationClientForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void allocationClientForm_Closed(object sender, EventArgs e)
        {
            try
            {
                if (allocationClientForm != null)
                    allocationClientForm.Dispose();
                allocationClientForm.GenrateCashTransaction -= allocationInstance_GenrateCashTransaction;
                allocationClientForm.LoadCloseTradeUIFromAllocation -= allocationInstance_loadCloseTradeUIFromAllocation;
                allocationClientForm.LoadSymbolLookUpUIFromAllocation -= allocationInstance_loadSymbolLookUpUIFromAllocation;
                allocationClientForm.GetAuditClick -= allInstance_GetAuditClick;
                allocationClientForm.AllocationDataChange -= new EventHandler<EventArgs<bool>>(allocationInstance_allocationDataChange);
                allocationClientForm = null;
                if (OpenModulesList.Contains(PranaModules.ALLOCATION_MODULE))
                    OpenModulesList.Remove(PranaModules.ALLOCATION_MODULE);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Intizalize the Symbollookup.
        /// </summary>
        private ListEventAargs GetArgumentFromSymbol(string symbol)
        {
            ListEventAargs args = new ListEventAargs();
            try
            {
                Dictionary<String, String> argDict = new Dictionary<string, string>();
                argDict.Add(ApplicationConstants.CRITERIA, SecMasterConstants.SearchCriteria.Ticker.ToString());
                argDict.Add(ApplicationConstants.SYMBOL, symbol.Trim());
                argDict.Add(ApplicationConstants.ACTION, SecMasterConstants.SecurityActions.SEARCH.ToString());
                args.argsObject = argDict;


            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return args;

        }

        string _symbolToSearch = string.Empty;
        private void allocationInstance_loadSymbolLookUpUIFromAllocation(object sender, EventArgs<string> e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Value))
                {
                    _arguments = GetArgumentFromSymbol(e.Value);
                }
                Window parentForm1 = sender as Window;
                LaunchPluggableTool(ApplicationConstants.CONST_SYMBOL_LOOKUP, GetWindowNewLocation(parentForm1));
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

        void allocationInstance_loadCloseTradeUIFromAllocation(object sender, EventArgs<AllocationGroup> e)
        {
            try
            {
                LaunchClosingUI(sender, e.Value);
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

        void allocationInstance_allocationDataChange(object sender, EventArgs<bool> e)
        {
            try
            {
                //disable closing UI while performing operation on the Allocation UI
                //before it was disabled only if EditTradeCommission UI is opened
                if (closingUIInstance != null)
                {
                    if (ModuleManager.CheckModulePermissioning(PranaModules.CLOSE_POSITIONS_MODULE, PranaModules.CLOSE_POSITIONS_MODULE))
                        closingUIInstance.ToggleUIElementsWithMessage(!e.Value);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private Dictionary<string, IPluggableTools> activePluggableTools = new Dictionary<string, IPluggableTools>();

        private void InterfacePluggableTool(IPluggableTools tool)
        {
            try
            {
                // Ideally tool should not be type of ILogin user
                // In ideal case IPluggableTool should compose ILogin user
                // To avoid wrong type cast check is applied and log and show policy is applied
                if (tool is ILoginUser)
                    ((ILoginUser)tool).LoginUser = loginUser;
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
        /// To check if given compliance component permitted to user or not.
        /// </summary>
        /// <param name="toolName"></param>
        /// <returns></returns>
        private bool CheckPermissionForComplianceTool(string toolName)
        {
            try
            {
                bool isRuleTab = ModuleManager.CheckToolPermissioning(ApplicationConstants.CONST_COMPLIANCE, ApplicationConstants.CONST_CE_RULE_DEFINITION);
                bool isAlertTab = ModuleManager.CheckToolPermissioning(ApplicationConstants.CONST_COMPLIANCE, ApplicationConstants.CONST_CE_ALERT_HISTORY);
                bool isPendingApprovalTab = ModuleManager.CheckToolPermissioning(ApplicationConstants.CONST_COMPLIANCE, ApplicationConstants.CONST_CE_PENDING_APPROVAL);
                if (toolName.Equals(ApplicationConstants.CONST_COMPLIANCE_RULE_DEFINITION))
                    return isRuleTab;

                if (toolName.Equals(ApplicationConstants.CONST_COMPLIANCE_ALERT_HISTORY))
                    return isAlertTab;

                if (toolName.Equals(ApplicationConstants.CONST_COMPLIANCE_PENDING_APPROVAL))
                    return isPendingApprovalTab;

                if (toolName.Equals(ApplicationConstants.CONST_COMPLIANCE))
                    return isRuleTab || isPendingApprovalTab || isAlertTab;
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

        private Form LaunchPluggableTool(string toolName, Point parentFormLocation)
        {
            bool isPermitted = false;
            if (toolName == ApplicationConstants.CONST_COMPLIANCE || toolName == ApplicationConstants.CONST_COMPLIANCE_ALERT_HISTORY || toolName == ApplicationConstants.CONST_COMPLIANCE_PENDING_APPROVAL || toolName == ApplicationConstants.CONST_COMPLIANCE_RULE_DEFINITION)
            {
                isPermitted = CheckPermissionForComplianceTool(toolName);
                toolName = ApplicationConstants.CONST_COMPLIANCE;
            }
            Form pluggableForm = null;
            IPluggableTools pluggaleToolLaunch = null;
            try
            {
                if (isPermitted || ModuleManager.CheckToolPermissioning(toolName, toolName))
                {
                    DynamicClass formToLoad;
                    formToLoad = (DynamicClass)ModuleManager.AvailableTools[toolName];
                    Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);
                    Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);
                    object handle = null;
                    if (!activePluggableTools.ContainsKey(typeToLoad.Name))
                    {
                        if (toolName == "Pricing Inputs" && (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet || CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.ACTIV || CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI) && !MarketDataValidation.CheckMarketDataPermissioning())
                            return null;

                        handle = Activator.CreateInstance(typeToLoad);

                        pluggaleToolLaunch = (IPluggableTools)handle;
                        pluggableForm = pluggaleToolLaunch.Reference();
                        pluggaleToolLaunch.PostTradeServices = _postTradeServicesInstance;
                        if (_secMasterServices != null && _secMasterServices.TradeCommunicationManager.ConnectionStatus.Equals(PranaInternalConstants.ConnectionStatus.CONNECTED))
                        {
                            _secMasterServices.IsConnected = true;
                        }

                        InterfacePluggableTool(pluggaleToolLaunch);

                        if (toolName == ApplicationConstants.CONST_COMPLIANCE)
                        {
                            MethodInfo mi = typeToLoad.GetMethod("SetTabPermissioning");
                            object[] parameters = new object[1];
                            parameters[0] = dictComplianceTabs;
                            if (handle == null)
                                handle = Activator.CreateInstance(typeToLoad);
                            mi.Invoke(handle, parameters);
                        }

                        pluggaleToolLaunch.SecurityMaster = _secMasterServices;
                        pluggaleToolLaunch.PricingAnalysis = _pricingAnalysisInstance;
                        if (toolName == ApplicationConstants.CONST_PRICING_INPUT && _symbolToSearch != string.Empty)
                        {
                            MethodInfo mi = typeToLoad.GetMethod("SetSymbolsForFetch");
                            object[] parameters = new object[1];
                            parameters[0] = _symbolToSearch;
                            if (handle == null)
                                handle = Activator.CreateInstance(typeToLoad);
                            mi.Invoke(handle, parameters);
                        }
                        pluggaleToolLaunch.SetUP();
                        pluggaleToolLaunch.PluggableToolsClosed += new EventHandler(PluggableToolClosed);
                        activePluggableTools.Add(typeToLoad.Name, pluggaleToolLaunch);
                        if (pluggaleToolLaunch is ILaunchForm)
                        {
                            ((ILaunchForm)pluggaleToolLaunch).LaunchForm += new EventHandler(pluggaleToolLaunch_LaunchForm);
                        }
                        pluggableForm.Owner = this;
                        pluggableForm.ShowInTaskbar = false;
                        pluggableForm.Show();
                        if (toolName == ApplicationConstants.CONST_SYMBOL_LOOKUP)//"ConfigurationHelper.CONFIGKEY_availableModules_SecMaster)
                        {
                            SetSecurityMasterControl(handle);
                        }
                        SetFromLayoutDetail(pluggableForm);
                    }
                    else
                    {
                        IPluggableTools pluggableToolToShow = activePluggableTools[typeToLoad.Name];
                        pluggableForm = pluggableToolToShow.Reference();
                        //if tool Name symbol lookup the set PreLoad request on  UI.
                        if (toolName == ApplicationConstants.CONST_PRICING_INPUT)
                        {
                            handle = pluggableToolToShow;
                            if (_listPI != null && _listPI.Count > 0)
                            {
                                MethodInfo mi = typeToLoad.GetMethod("AddSymboltoPI");
                                object[] parameters = new object[1];
                                parameters[0] = _listPI;
                                mi.Invoke(pluggableForm, parameters);
                            }
                            if (_listPI != null)
                            {
                                _listPI.Clear();
                            }
                        }

                        //If tool name is Compliance, then need to set Selected Tab
                        if (toolName == ApplicationConstants.CONST_COMPLIANCE)
                        {
                            MethodInfo mi = typeToLoad.GetMethod("SetComplianceActivatedTab");
                            mi.Invoke(pluggableForm, new object[0]);
                        }
                    }
                    if (toolName == ApplicationConstants.CONST_SYMBOL_LOOKUP)
                    {
                        SetSecurityMasterSearchControl(pluggableForm);
                        if (_symbolToSearch != string.Empty)
                        {
                            MethodInfo mi = typeToLoad.GetMethod("GetDataForSymbol");
                            object[] parameters = new object[1];
                            parameters[0] = _symbolToSearch;
                            if (handle == null)
                                handle = Activator.CreateInstance(typeToLoad);
                            handle.GetType().GetProperty("SecurityMaster").SetValue(handle, _secMasterServices, null);
                            mi.Invoke(handle, parameters);
                        }
                        if (handle != null)
                        {
                            ((ISecurityMasterControl)handle).LaunchPricingInput += SymbolLookup_LaunchPricingInput;
                        }
                        _symbolToSearch = string.Empty;
                    }

                    BringFormToFront(pluggableForm, parentFormLocation);
                    if (toolName == ApplicationConstants.CONST_PRICING_INPUT)
                    {
                        if (_symbolToSearch != string.Empty)
                        {
                            MethodInfo mi = typeToLoad.GetMethod("FilterBySelectedSymbol");
                            object[] parameters = new object[1];
                            parameters[0] = _symbolToSearch;
                            if (handle == null)
                                handle = Activator.CreateInstance(typeToLoad);
                            mi.Invoke(handle, parameters);
                        }
                        else
                        {
                            if (_listPI != null && _listPI.Count > 0)
                            {
                                MethodInfo mi = typeToLoad.GetMethod("AddSymboltoPI");
                                object[] parameters = new object[1];
                                parameters[0] = _listPI;
                                if (handle == null)
                                    handle = Activator.CreateInstance(typeToLoad);
                                mi.Invoke(handle, parameters);
                            }
                        }

                        if (handle != null)
                        {
                            ((IpluggableToolPI)handle).LaunchSymbolLookup += PI_LaunchSymbolLookup;
                        }
                        _symbolToSearch = string.Empty;
                        if (_listPI != null)
                        {
                            _listPI.Clear();
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
            return pluggableForm;
        }

        /// <summary>
        /// Setting Symbol look filter on load UI
        /// </summary>
        /// <param name="handle"></param>
        private void SetSecurityMasterSearchControl(object handle)
        {
            try
            {
                if (_arguments != null)
                {
                    ListEventAargs args = _arguments as ListEventAargs;
                    if (args != null)
                    {
                        ISecurityMasterControl secmasterControl = handle as ISecurityMasterControl;
                        if (secmasterControl != null)
                            secmasterControl.HandleOnLoadRequest(args);
                        _arguments = null;
                    }
                }
            }
            catch (Exception ex)
            {
                InformationMessageBox.Display(ex.StackTrace);
            }
        }

        void pluggaleToolLaunch_LaunchForm(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                if (parentForm == null)
                {
                    parentForm = this;
                }
                _arguments = e;
                ListEventAargs args = e as ListEventAargs;
                if (args != null && args.listOfValues.Count > 0)
                {
                    switch (args.listOfValues[0])
                    {
                        case ApplicationConstants.CONST_SYMBOL_LOOKUP:
                            LaunchPluggableTool(ApplicationConstants.CONST_SYMBOL_LOOKUP, GetFormNewLocation(parentForm));
                            break;

                        case ApplicationConstants.CONST_UDA_UI:
                            LaunchPluggableTool(ApplicationConstants.CONST_UDA_UI, GetFormNewLocation(parentForm));
                            break;

                        case ApplicationConstants.CONST_LIVEFEEDVALIDATION_UI:
                            LaunchPluggableTool(ApplicationConstants.CONST_LIVEFEEDVALIDATION_UI, GetFormNewLocation(parentForm));
                            break;

                        case ApplicationConstants.CONST_DataMapping_UI:
                            LaunchMappingUI(e, parentForm);
                            break;

                        case ApplicationConstants.CONST_MANUAL_TRADING_TICKET_UI:
                            LaunchTradingTicketDock(TradingTicketParent.None);
                            break;

                        case ApplicationConstants.CONST_POST_RECON_AMENDMENTS:
                            if (args.listOfValues.Count == 5)
                            {
                                int accountID = int.Parse(args.listOfValues[1]);
                                string symbol = args.listOfValues[2];
                                DateTime date = DateTime.Parse(args.listOfValues[3]);
                                //CHMW-1620 [Closing] - Add Comments field in PostReconAmendenmtsUI
                                string comment = args.listOfValues[4];
                                EventHandler UpdateCommentsFromPostReconAmendments = (EventHandler)args.argsObject;
                                LaunchPostReconAmendmentUI(accountID, symbol, date, comment, UpdateCommentsFromPostReconAmendments);
                            }
                            break;

                        case ApplicationConstants.CONST_Allocation:
                            LaunchAllocationForm();
                            break;

                        case ApplicationConstants.CONST_PricingDataLookUp:
                            LaunchPluggableTool(ApplicationConstants.CONST_PricingDataLookUp, GetFormNewLocation(parentForm));
                            break;

                        default:
                            LaunchMappingUI(e, parentForm);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                InformationMessageBox.Display(ex.StackTrace);
            }
        }

        /// <summary>
        /// Launching symbol Lookup with filetrs and bind event for open TT from Symbol lookup 
        /// </summary>
        /// <param name="handle"></param>      
        private void SetSecurityMasterControl(object handle)
        {
            try
            {
                ISecurityMasterControl secmasterControl = (ISecurityMasterControl)handle;
                secmasterControl.SymbolDoubleClicked += new EventHandler(secmasterControl_SymbolDoubleClicked);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void secmasterControl_SymbolDoubleClicked(object sender, EventArgs e)
        {
            try
            {
                LaunchTradingTicketDock(sender.ToString(), TradingTicketParent.SymbolLookup);
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
        /// Handles the SendSymbolToMTT event of the _watchList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{OrderBindingList}"/> instance containing the event data.</param>
        void _watchList_SendSymbolToMTT(object sender, EventArgs<OrderBindingList> e)
        {
            try
            {
                if (e.Value != null)
                {
                    LaunchMultiTradingTicketDock(e.Value, GetFormNewLocation(null), TradingTicketParent.WatchList);
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
        /// Handles the SendSymbolToMTT event of the _optionChain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{OrderBindingList}"/> instance containing the event data.</param>
        void _optionchain_SendSymbolToMTT(object sender, EventArgs<OrderBindingList> e)
        {
            try
            {
                if (e.Value != null)
                {
                    LaunchMultiTradingTicketDock(e.Value, GetFormNewLocation(null), TradingTicketParent.OptionChain);
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

        private delegate void AddOptionsToWatchListReceived(object sender, EventArgs<int, List<string>> e);
        /// <summary>
        /// Handles the event of adding options to WatchList
        /// </summary>
        /// <param name="e">List of options to add to WatchList</param>
        void _optionChain_AddOptionsToWatchList(object sender, EventArgs<int, List<string>> e)
        {
            try
            {
                if (_watchList != null)
                {
                    if (_watchList.InvokeRequired)
                    {
                        AddOptionsToWatchListReceived mi = new AddOptionsToWatchListReceived(_optionChain_AddOptionsToWatchList);
                        _watchList.BeginInvoke(mi, new object[] { sender, e });
                    }

                    if (_watchList.AddSymbolsToWatchList(e.Value, e.Value2))
                        _watchList.BringToFront();
                }
                else
                {
                    new CustomMessageBox("Watchlist Closed!", "Watchlist is closed so unable to add option(s).", false, string.Empty, FormStartPosition.CenterParent).ShowDialog();
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
        /// Handles the HighlightSymbolFromBlotter event of the _blotter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}"/> instance containing the event data.</param>
        void _blotter_HighlightSymbolFromBlotter(object sender, EventArgs<string> e)
        {
            try
            {
                if (e != null && !string.IsNullOrEmpty(e.Value))
                {
                    if (portfolioManagementInstance != null)
                    {
                        portfolioManagementInstance.HighlightSymbol(e.Value);
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
        /// Handles the HighlightSymbolFromWatchlist event of the _watchList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}"/> instance containing the event data.</param>
        void _watchList_HighlightSymbolFromWatchlist(object sender, EventArgs<string> e)
        {
            try
            {
                if (e != null && !string.IsNullOrEmpty(e.Value))
                {
                    if (blotterForm != null && blotternewInstance != null)
                    {
                        blotternewInstance.HighlightSymbol(e.Value);
                    }
                    if (portfolioManagementInstance != null)
                    {
                        portfolioManagementInstance.HighlightSymbol(e.Value);
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
        /// Handles the FormClosed event of the _watchList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        void _watchList_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_watchList != null)
                {
                    _watchList.SendSymbolToTT -= _watchListControl_SymbolDoubleClicked;
                    _watchList.SendSymbolToPTT -= _watchList_SendSymbolToPTT;
                    _watchList.SendSymbolToMTT -= _watchList_SendSymbolToMTT;
                    _watchList.OptionChainModuleOpened -= _watchList_OptionChainModuleOpened;
                    _watchList.HighlightSymbolFromWatchlist -= _watchList_HighlightSymbolFromWatchlist;
                    _watchList.FormClosed -= _watchList_FormClosed;
                    _watchList = null;
                }
                OpenModulesList.Remove(PranaModules.WATCHLIST_MODULE);
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
        /// Handles the FormClosed event of the _optionChain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        void _optionChain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_optionChain != null)
                {
                    _optionChain.SendSymbolToTT -= _optionChain_SendSymbolToTT;
                    _optionChain.SendSymbolToMTT -= _optionchain_SendSymbolToMTT;
                    _optionChain.FormClosed -= _optionChain_FormClosed;
                    _optionChain = null;
                }
                OpenModulesList.Remove(PranaModules.OPTIONCHAIN_MODULE);
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
        /// Handles the SymbolDoubleClicked event of the WatchListControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String, System.String}"/> instance containing the event data.</param>
        void _watchListControl_SymbolDoubleClicked(object sender, EventArgs<string, string> e)
        {
            try
            {
                LaunchTradingTicketDock(e.Value, TradingTicketParent.WatchList, e.Value2);
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
        /// Handles the SendSymbolToTT event of the _optionChain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String, System.String}"/> instance containing the event data.</param>
        void _optionChain_SendSymbolToTT(object sender, EventArgs<string, string> e)
        {
            try
            {
                LaunchTradingTicketDock(e.Value, TradingTicketParent.OptionChain, e.Value2);
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

        void PluggableToolClosed(object sender, EventArgs e)
        {
            try
            {
                IPluggableTools pluggableTool = (IPluggableTools)sender;
                Type toolType = pluggableTool.GetType();
                activePluggableTools.Remove(toolType.Name);

                Form pluggaleTuner = (Form)sender;
                if (pluggaleTuner != null)
                    pluggaleTuner = null;
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

        IQuickTradingTicket[] qTTInstances = new IQuickTradingTicket[10];
        Form[] qTTForms = new Form[10];

        /// <summary>
        /// Handles the highlight symbol on blotter event from QTT instances.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void QTTInstance_HighlightSymbolOnBlotter(object sender, QTTBlotterLinkingData e)
        {
            try
            {
                if (e != null && blotterForm != null && blotternewInstance != null)
                {
                    blotternewInstance.HighlightSymbolFromQTT(e);
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
        /// Handles the Dehighlight symbol on blotter event from QTT instances.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void QTTInstance_DeHighlightSymbolOnBlotter(object sender, QTTBlotterLinkingData e)
        {
            try
            {
                if (e != null && blotterForm != null && blotternewInstance != null)
                {
                    blotternewInstance.DeHighlightSymbolFromQTT(e);
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
        /// Send Instance name from popup to Main UI 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void QTTInstance_SendInstanceName(object sender, EventArgs<string> e)
        {
            try
            {
                IQuickTradingTicket qtt = sender as IQuickTradingTicket;
                quickTTButtons[qtt.QTTIndex].SharedPropsInternal.Caption = e.Value;
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

        private void LaunchQuickTradingTicket(int index)
        {
            try
            {
                if (!Program.IsModalDialogOpen)
                {
                    if (ModuleManager.CheckModulePermissioning(PranaModules.QUICK_TRADING_TICKET_MODULE, PranaModules.QUICK_TRADING_TICKET_MODULE))
                    {
                        if (index <= CachedDataManager.GetInstance.PermissibleQuickTTInstances)
                        {
                            index -= 1;
                            IQuickTradingTicket qttInstance = qTTInstances[index];
                            if (qttInstance == null)
                            {
                                qttInstance = _container.Resolve<IQuickTradingTicket>();
                                qTTInstances[index] = qttInstance;
                                qttInstance.SecurityMaster = _secMasterServices;
                                qttInstance.LaunchSymbolLookup += TicketInstance_LaunchSymbolLookup;
                                qttInstance.FormClosedHandler += qTTInstance_FormClosedHandler;
                                qttInstance.HighlightSymbolOnBlotter += QTTInstance_HighlightSymbolOnBlotter;
                                qttInstance.DeHighlightSymbolOnBlotter += QTTInstance_DeHighlightSymbolOnBlotter;
                                qttInstance.SendInstanceName += QTTInstance_SendInstanceName;
                                qttInstance.LoginUser = loginUser;
                                qttInstance.QTTIndex = index;

                                qTTForms[index] = qttInstance.Reference();
                                qTTForms[index].Owner = this;
                                qTTForms[index].ShowInTaskbar = false;

                                if (_exPNLCommManagerInstance == null)
                                {
                                    _exPNLCommManagerInstance = new ClientTradeCommManager();
                                    ExposurePnlCacheManager.GetInstance().Initialise(loginUser, _exPNLCommManagerInstance);
                                }
                                ConnectToExposurePNLServer(false);

                                qTTForms[index].Show();
                            }
                            BringFormToFront(qTTForms[index], GetFormNewLocation(null));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                InformationMessageBox.Display(ex.StackTrace);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void qTTInstance_FormClosedHandler(object sender, EventArgs e)
        {
            IQuickTradingTicket qtt = sender as IQuickTradingTicket;
            DisconnectToExposurePNLServer(false);
            if (_container != null)
            {
                _container.Release(qtt);
            }
            qtt.LaunchSymbolLookup -= TicketInstance_LaunchSymbolLookup;
            qtt.FormClosedHandler -= qTTInstance_FormClosedHandler;
            qtt.HighlightSymbolOnBlotter -= QTTInstance_HighlightSymbolOnBlotter;
            qtt.DeHighlightSymbolOnBlotter -= QTTInstance_DeHighlightSymbolOnBlotter;
            qtt.SendInstanceName -= QTTInstance_SendInstanceName;
            qTTForms[qtt.QTTIndex] = null;
            qTTInstances[qtt.QTTIndex] = null;
        }

        ITradingTicket tradingTicketInstance = null;
        Form tradingTicketDock = null;

        private void LaunchTradingTicketDock(TradingTicketParent tradingTicketParent)
        {
            try
            {
                if (!Program.IsModalDialogOpen)
                {
                    if (ModuleManager.CheckModulePermissioning(PranaModules.TRADING_TICKET_MODULE, PranaModules.TRADING_TICKET_MODULE))
                    {
                        if (tradingTicketInstance == null)
                        {
                            tradingTicketInstance = _container.Resolve<ITradingTicket>();
                            tradingTicketInstance.SecurityMaster = _secMasterServices;
                            tradingTicketInstance.LaunchSymbolLookup += TicketInstance_LaunchSymbolLookup;
                            tradingTicketInstance.FormClosedHandler += new EventHandler(tradingTicketInstance_FormClosedHandler);
                            tradingTicketInstance.LoginUser = loginUser;

                            tradingTicketDock = tradingTicketInstance.Reference();
                            tradingTicketDock.Owner = this;
                            tradingTicketDock.ShowInTaskbar = false;

                            if (_exPNLCommManagerInstance == null)
                            {
                                _exPNLCommManagerInstance = new ClientTradeCommManager();
                                ExposurePnlCacheManager.GetInstance().Initialise(loginUser, _exPNLCommManagerInstance);
                            }
                            ConnectToExposurePNLServer(false);

                            tradingTicketDock.Show();
                            //SetFromLayoutDetail(tradingTicketDock);
                        }
                        if (tradingTicketInstance != null && tradingTicketInstance.TradingTicketParent == TradingTicketParent.None)
                        {
                            tradingTicketInstance.TradingTicketParent = tradingTicketParent;
                        }
                        BringFormToFront(tradingTicketDock, GetFormNewLocation(null));
                    }
                }
            }
            catch (Exception ex)
            {
                InformationMessageBox.Display(ex.StackTrace);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        WatchList.Forms.WatchListMain _watchList = null;

        private void LaunchWatchList()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.WATCHLIST_MODULE, PranaModules.WATCHLIST_MODULE)
                    && MarketDataValidation.CheckMarketDataPermissioning())
                {
                    if (!OpenModulesList.Contains(PranaModules.WATCHLIST_MODULE))
                        OpenModulesList.Add(PranaModules.WATCHLIST_MODULE);
                    if (_watchList == null || _watchList.IsDisposed)
                    {
                        _watchList = new WatchList.Forms.WatchListMain(loginUser, _secMasterServices);
                        _watchList.Owner = this;
                        _watchList.ShowInTaskbar = false;
                        SetFromLayoutDetail(_watchList);
                        _watchList.SendSymbolToTT += _watchListControl_SymbolDoubleClicked;
                        _watchList.SendSymbolToPTT += _watchList_SendSymbolToPTT;
                        _watchList.SendSymbolToMTT += _watchList_SendSymbolToMTT;
                        _watchList.OptionChainModuleOpened += _watchList_OptionChainModuleOpened;
                        _watchList.HighlightSymbolFromWatchlist += _watchList_HighlightSymbolFromWatchlist;
                        _watchList.FormClosed += _watchList_FormClosed;
                    }
                    _watchList.Show();
                    BringFormToFront(_watchList, GetFormNewLocation(null));
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

        void _watchList_SendSymbolToPTT(object sender, EventArgs<string> e)
        {
            try
            {
                var lstAccount = new List<int>();
                Prana.Rebalancer.PercentTradingTool.Preferences.PTTPreferences prefs = new Rebalancer.PercentTradingTool.Preferences.PTTPreferences();
                LaunchPercentTradingTool(false, e.Value, prefs.MasterFundOrAccount, lstAccount);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            // throw new NotImplementedException();
        }

        void _watchList_OptionChainModuleOpened(object sender, EventArgs<int, string> e)
        {
            LaunchOptionChain(e.Value, e.Value2);
        }

        WatchList.Forms.OptionChain _optionChain = null;

        private void LaunchOptionChain(int watchListTabNumber = int.MinValue, string selectedSymbol = null)
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.OPTIONCHAIN_MODULE, PranaModules.OPTIONCHAIN_MODULE)
                    && MarketDataValidation.CheckMarketDataPermissioning())
                {
                    if (!OpenModulesList.Contains(PranaModules.OPTIONCHAIN_MODULE))
                        OpenModulesList.Add(PranaModules.OPTIONCHAIN_MODULE);
                    if (_optionChain == null || _optionChain.IsDisposed)
                    {
                        _optionChain = new WatchList.Forms.OptionChain(watchListTabNumber, selectedSymbol, _secMasterServices);
                        _optionChain.SendSymbolToTT += _optionChain_SendSymbolToTT;
                        _optionChain.SendSymbolToMTT += _optionchain_SendSymbolToMTT;
                        _optionChain.AddOptionsToWatchList += _optionChain_AddOptionsToWatchList;
                        _optionChain.FormClosed += _optionChain_FormClosed;
                        _optionChain.Owner = this;
                        _optionChain.ShowInTaskbar = false;
                        _optionChain.Location = GetFormNewLocation(null);
                        _optionChain.StartPosition = FormStartPosition.Manual;
                        SetFromLayoutDetail(_optionChain);
                    }
                    else if (watchListTabNumber != int.MinValue)
                    {
                        _optionChain.WatchListTabNumber = watchListTabNumber;
                        _optionChain.Symbol = selectedSymbol;
                    }
                    _optionChain.Show();

                    if (_optionChain.WindowState == FormWindowState.Normal)
                        BringFormToFront(_optionChain, _optionChain.Location);
                    else
                        BringFormToFront(_optionChain, GetFormNewLocation(null));
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

        Prana.ShortLocate.ShortLocate _shortLocate = null;

        private void LaunchShortLocate()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.SHORTLOCATE_MODULE, PranaModules.SHORTLOCATE_MODULE))
                {
                    if (!OpenModulesList.Contains(PranaModules.SHORTLOCATE_MODULE))
                        OpenModulesList.Add(PranaModules.SHORTLOCATE_MODULE);
                    if (_shortLocate == null || _shortLocate.IsDisposed)
                    {
                        _shortLocate = new Prana.ShortLocate.ShortLocate();
                    }
                    _shortLocate.Owner = this;
                    SetFromLayoutDetail(_shortLocate);
                    _shortLocate.ShowInTaskbar = false;
                    _shortLocate.MouseDoubleClickOnRow += _shortLocate_MouseDoubleClickOnRow;
                    _shortLocate.FormClosed += _shortLocate_FormClosed;
                    _shortLocate.Show();
                    BringFormToFront(_shortLocate, GetFormNewLocation(null));
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

        void _shortLocate_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_shortLocate != null)
            {
                _shortLocate.MouseDoubleClickOnRow -= _shortLocate_MouseDoubleClickOnRow;
                _shortLocate = null;
            }
        }


        RebalancerMainView frmRebalancer = null;
        private void LaunchRebalancer()
        {
            try
            {
                if (frmRebalancer == null)
                {
                    if (MarketDataValidation.CheckMarketDataPermissioning())
                    {
                        bool moduleAvailable = false;
                        moduleAvailable = ModuleManager.CheckModulePermissioning(PranaModules.REBALANCER_MODULE, PranaModules.REBALANCER_MODULE);
                        if (moduleAvailable)
                        {
                            if (!OpenModulesList.Contains(PranaModules.REBALANCER_MODULE))
                            {
                                OpenModulesList.Add(PranaModules.REBALANCER_MODULE);
                            }
                            frmRebalancer = new RebalancerMainView();
                            frmRebalancer.SetUp();
                            frmRebalancer.SecurityMaster = _secMasterServices;
                            frmRebalancer.LaunchForm += new EventHandler(RebalancerInstance_LaunchSymbolLookup);
                            frmRebalancer.Closed += new EventHandler(frmRebalancer_FormClosed);
                            BringFormToFront(frmRebalancer);
                        }
                    }
                }
                else
                {
                    frmRebalancer.WindowState = System.Windows.WindowState.Normal;
                    frmRebalancer.Activate();
                }
            }
            catch (Exception ex)
            {
                InformationMessageBox.Display(ex.StackTrace);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void frmRebalancer_FormClosed(object sender, EventArgs e)
        {
            try
            {
                if (frmRebalancer != null)
                {
                    frmRebalancer.LaunchForm -= RebalancerInstance_LaunchSymbolLookup;
                    _uiSyncContext.Post(o => { if (frmRebalancer != null) { frmRebalancer.Close(); } }, null);
                    frmRebalancer.Dispose();
                    frmRebalancer = null;
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

        void tradingTicketInstance_FormClosedHandler(object sender, EventArgs e)
        {
            DisconnectToExposurePNLServer(false);
            if (_container != null)
            {
                _container.Release(tradingTicketInstance);
                _container.Release(tradingTicketDock);
            }
            tradingTicketDock = null;
            tradingTicketInstance = null;
        }

        void TicketInstance_LaunchSymbolLookup(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                _arguments = e;
                LaunchPluggableTool(ApplicationConstants.CONST_SYMBOL_LOOKUP, GetFormNewLocation(parentForm));
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

        void RebalancerInstance_LaunchSymbolLookup(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                _arguments = e;
                LaunchPluggableTool(ApplicationConstants.CONST_SYMBOL_LOOKUP, GetFormNewLocation(parentForm));
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

        void tradingTicketInstance_LaunchPreferences(object sender, EventArgs e)
        {
            try
            {
                LaunchPreferncesModule(PranaModules.TRADING_TICKET_MODULE, GetFormNewLocation(null));
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

        IMultiTradingTicket multiTradingTicketInstance = null;
        Form multiTradingTicketDock = null;
        /// <summary>
        ///  Check Availability for MultiTrading Ticket Module in availableModulesDetails and then launches the multiplt trades trading ticket.
        ///  Launch Trading Ticket Dock Form
        /// </summary>
        private void LaunchMultiTradingTicketDock(OrderBindingList orderList, Point formNewLocation, TradingTicketParent tradingTicketParent, bool isEdit = false)
        {
            try
            {
                if (!Program.IsModalDialogOpen)
                {
                    bool isReloadRequired = false;
                    if (ModuleManager.CheckModulePermissioning(PranaModules.MULTI_TRADING_TICKET_MODULE, PranaModules.MULTI_TRADING_TICKET_MODULE))
                    {
                        if (orderList != null && orderList.Count > _maximumOrdersAllowedinMTT)
                        {
                            MessageBox.Show("We currently support only " + _maximumOrdersAllowedinMTT + " orders through our Multi-Trading Ticket. Request you to reduce the number of orders and retry.",
                                "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        if (multiTradingTicketInstance == null)
                        {
                            multiTradingTicketInstance = _container.Resolve<IMultiTradingTicket>();
                            multiTradingTicketInstance.SecurityMaster = _secMasterServices;
                            multiTradingTicketInstance.FormClosedHandler += multiTradingTicketInstance_FormClosedHandler;
                            multiTradingTicketInstance.LoginUser = loginUser;
                        }
                        else
                        {
                            multiTradingTicketDock = null;
                            isReloadRequired = true;
                        }

                        multiTradingTicketInstance.TradingTicketParent = tradingTicketParent;
                        multiTradingTicketDock = multiTradingTicketInstance.Reference();
                        multiTradingTicketDock.Owner = this;
                        multiTradingTicketDock.ShowInTaskbar = false;



                        if (isReloadRequired)
                            multiTradingTicketInstance.ResetMultiTradingTicket(false);

                        //multiTradingTicketInstance.UpdateOrderListWithSMData(orderList);
                        multiTradingTicketInstance.SetMTTFromNirvanaMain(orderList, isEdit);

                        multiTradingTicketDock.StartPosition = FormStartPosition.Manual;
                        SetFromLayoutDetail(multiTradingTicketDock);

                        if (isReloadRequired)
                            multiTradingTicketInstance.ResetMultiTradingTicket(true);
                        multiTradingTicketDock.Show();
                        multiTradingTicketDock.Activate();
                        BringFormToFront(multiTradingTicketDock, GetFormNewLocation(null));
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

        void multiTradingTicketInstance_FormClosedHandler(object sender, EventArgs e)
        {
            try
            {
                if (_container != null)
                {
                    if (multiTradingTicketInstance != null)
                        _container.Release(multiTradingTicketInstance);
                    if (multiTradingTicketDock != null)
                        _container.Release(multiTradingTicketDock);
                }
                multiTradingTicketInstance = null;
                multiTradingTicketDock = null;
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
        ///  Check Availability for Trading Ticket Module in availableModulesDetails.
        ///  Launch Trading Ticket Dock Form
        /// </summary>
        private void LaunchTradingTicketDock(OrderSingle or, TradingTicketParent tradingTicketParent, int allocationPrefID = 0, Dictionary<int, double> accountWithPostions = null)
        {
            try
            {
                //Adding OriginalAllocationPreferenceID as allocationPrefID
                //https://jira.nirvanasolutions.com:8443/browse/PRANA-25709 
                if (or.OriginalAllocationPreferenceID > 0)
                {
                    allocationPrefID = or.OriginalAllocationPreferenceID;
                }
                if (!Program.IsModalDialogOpen)
                {
                    bool isReloadRequired = false;
                    if (ModuleManager.CheckModulePermissioning(PranaModules.TRADING_TICKET_MODULE, PranaModules.TRADING_TICKET_MODULE))
                    {
                        if (tradingTicketInstance == null || !UIValidation.GetInstance().validate(tradingTicketDock))
                        {
                            tradingTicketInstance = _container.Resolve<ITradingTicket>();
                            tradingTicketInstance.SecurityMaster = _secMasterServices;
                            tradingTicketInstance.LaunchSymbolLookup += new EventHandler(TicketInstance_LaunchSymbolLookup);
                            tradingTicketInstance.FormClosedHandler += new EventHandler(tradingTicketInstance_FormClosedHandler);
                            tradingTicketInstance.LoginUser = loginUser;
                        }
                        else
                        {
                            tradingTicketDock = null;
                            isReloadRequired = true;
                        }
                        tradingTicketInstance.TradingTicketParent = tradingTicketParent;
                        tradingTicketDock = tradingTicketInstance.Reference();
                        tradingTicketDock.Owner = this;
                        tradingTicketDock.ShowInTaskbar = false;

                        if (_exPNLCommManagerInstance == null)
                        {
                            _exPNLCommManagerInstance = new ClientTradeCommManager();
                            ExposurePnlCacheManager.GetInstance().Initialise(loginUser, _exPNLCommManagerInstance);
                        }
                        ConnectToExposurePNLServer(false);

                        tradingTicketDock.Show();
                        tradingTicketDock.Activate();

                        if (isReloadRequired)
                            tradingTicketInstance.ResetTicket();

                        tradingTicketInstance.SetTradingTicketFromNirvanaMain(or, allocationPrefID, accountWithPostions);

                        SetFromLayoutDetail(tradingTicketDock);
                        BringFormToFront(tradingTicketDock, GetFormNewLocation(null));
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

        private void LaunchTradingTicketDock(string symbol, TradingTicketParent tradingTicketParent, string watchlistColumn = "")
        {
            try
            {
                if (!Program.IsModalDialogOpen)
                {
                    bool isReloadRequired = false;
                    if (ModuleManager.CheckModulePermissioning(PranaModules.TRADING_TICKET_MODULE, PranaModules.TRADING_TICKET_MODULE))
                    {
                        if (tradingTicketInstance == null)
                        {
                            tradingTicketInstance = _container.Resolve<ITradingTicket>();
                            tradingTicketInstance.SecurityMaster = _secMasterServices;
                            tradingTicketInstance.LaunchSymbolLookup += new EventHandler(TicketInstance_LaunchSymbolLookup);
                            tradingTicketInstance.FormClosedHandler += new EventHandler(tradingTicketInstance_FormClosedHandler);
                            tradingTicketInstance.LoginUser = loginUser;
                        }
                        else
                        {
                            tradingTicketDock = null;
                            isReloadRequired = true;
                        }
                        tradingTicketInstance.TradingTicketParent = tradingTicketParent;
                        tradingTicketDock = tradingTicketInstance.Reference();
                        tradingTicketDock.Owner = this;
                        tradingTicketDock.ShowInTaskbar = false;

                        if (_exPNLCommManagerInstance == null)
                        {
                            _exPNLCommManagerInstance = new ClientTradeCommManager();
                            ExposurePnlCacheManager.GetInstance().Initialise(loginUser, _exPNLCommManagerInstance);
                        }
                        ConnectToExposurePNLServer(false);

                        tradingTicketDock.Show();
                        tradingTicketDock.Activate();

                        if (isReloadRequired)
                            tradingTicketInstance.ResetTicket();

                        tradingTicketInstance.TradingFormSetting(symbol, watchlistColumn);

                        SetFromLayoutDetail(tradingTicketDock);
                        BringFormToFront(tradingTicketDock, GetFormNewLocation(null));
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

        private void SetFromLayoutDetail(Form form)
        {
            try
            {
                if (UIValidation.GetInstance().validate(form))
                {
                    if (form.InvokeRequired)
                    {
                        MethodInvoker mi = new MethodInvoker(SetFromLayoutDetail);
                        form.BeginInvoke(mi, form);
                    }
                    else
                    {
                        if (!_loadingLayout)
                        {
                            DataTable dt = LayoutManager.GetModuleDetailsForLayout(_layoutID, form.Name);
                            if (dt.Rows.Count == 1)
                            {
                                DataRow dr = dt.Rows[0];
                                FormWindowState windowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), dr["WindowState"].ToString());
                                if (windowState == FormWindowState.Normal)
                                {
                                    if (CheckVirtualScreenBounds(dr))
                                    {
                                        int width = Int32.Parse(dr["Width"].ToString());
                                        int height = Int32.Parse(dr["Height"].ToString());
                                        form.Size = new Size(width, height);
                                    }
                                    else
                                    {
                                        Size size = SystemInformation.PrimaryMonitorSize;
                                        form.Left = size.Width / 2 - (int)Math.Truncate(form.Width / 2.0);
                                        form.Top = size.Height / 2 - (int)Math.Truncate(form.Height / 2.0);
                                    }
                                }
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

        private void BringFormToFront(Form form, Point formLocation)
        {
            try
            {
                if (UIValidation.GetInstance().validate(form))
                {
                    if (form.InvokeRequired)
                    {
                        BringToFrontHandler mi = new BringToFrontHandler(BringFormToFront);
                        form.BeginInvoke(mi, form, formLocation);
                    }
                    else
                    {
                        if (form != null)
                        {
                            if (form.WindowState == FormWindowState.Minimized)
                            {
                                form.WindowState = FormWindowState.Normal;
                            }

                            form.Activate();
                            form.BringToFront();
                            if (!_loadingLayout)
                            {
                                //In case of form is Compliance Engine form. And if the parent form minimized or form location x and y coordinates negative,
                                // then set form location of Pending approval Compliance UI manually x=0 and y=105
                                if (form.Name.Equals(ApplicationConstants.CONST_COMPLIANCE_ENGINE) && (formLocation.X < 0 || formLocation.Y < 0))
                                    form.Location = new Point(0, 105);
                                else
                                    form.Location = formLocation;
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

        private Point GetFormNewLocation(Form parentForm)
        {
            Point formLocation = new Point();
            try
            {
                bool isParentFormNull = false;
                if (parentForm == null)
                {
                    parentForm = this;
                    isParentFormNull = true;
                }
                if (UIValidation.GetInstance().validate(parentForm))
                {
                    if (parentForm.InvokeRequired)
                    {
                        GetFormLocationInvoker mi = new GetFormLocationInvoker(GetFormNewLocation);
                        parentForm.Invoke(mi, parentForm);
                    }
                    else
                    {
                        if (!isParentFormNull)
                        {
                            formLocation = new Point(parentForm.Location.X + 50, parentForm.Location.Y + 50);
                        }
                        else
                        {
                            formLocation = new Point(this.Location.X, this.Location.Y + this.Size.Height);
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
            return formLocation;
        }

        private Point GetWindowNewLocation(Window parentForm)
        {
            Point formLocation = new Point();
            try
            {
                if (!parentForm.Dispatcher.CheckAccess())
                {
                    // Switch threads and recurse
                    formLocation = (Point)parentForm.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       new Func<Window, Point>(GetWindowNewLocation), parentForm);
                }
                else
                {
                    System.Windows.Point screenCoordinates = parentForm.PointToScreen(new System.Windows.Point(0, 0));
                    formLocation = new Point((int)(screenCoordinates.X + 50), (int)(screenCoordinates.Y + 50));
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
            return formLocation;
        }

        private ISessionServices _sessionServices;
        public ISessionServices SessionServices
        {
            set { _sessionServices = value; }
        }

        private void LaunchLoginForm()
        {
            try
            {
                if (AskSaveConfirmation() == DialogResult.Cancel)
                {
                    return;
                }

                Login login = new Login();

                if (string.IsNullOrWhiteSpace(login.SamsaraAzureId))
                {
                    do
                    {
                        login.ShowDialog();
                    }
                    while ((login.LoginStatus == Constants.LoginStatus.InValidUser || login.LoginStatus == Constants.LoginStatus.NotSet) && login.DialogResult == DialogResult.OK);
                }

                if (login.LoginStatus != Constants.LoginStatus.ValidUser)
                {
                    this.Close();
                }
                else
                {
                    _layoutID = 0;
                    _layoutName = "";
                    //drop copy client
                    PranaDropCopyClient.ClearData();

                    // Set this User to Login User
                    loginUser = CachedDataManager.GetInstance.LoggedInUser;

                    _sessionServices.SetLoggedInUser(loginUser);
                    _clientConnectivityService.InnerChannel.AddClientInfoInCache(loginUser.CompanyUserID);

                    FillLoggedInUserIPDetails();

                    ConnectToAllSockets();

                    FileAndDbSyncManager.SyncFileWithDataBase("Prana Preferences", loginUser.CompanyUserID);

                    // userID -1 is kept as defaultID implying that the preferences are common to all users and will be picked from main prana Preferences folder...
                    FileAndDbSyncManager.SyncFileWithDataBase("Prana Preferences", -1);

                    CachedDataManager.GetInstance.SetCurrentTimeZone();
                    _isPMPermitted = WindsorContainerManager.GetPMModulePermission(loginUser.CompanyID);
                    TradeManagerExtension.GetInstance().UserID = loginUser.CompanyUserID;
                    TradeManagerExtension.GetInstance().CompanyID = loginUser.CompanyID;

                    // pass all counterParties Allowed to this User
                    formBrokerConnection.usrCtrlBrokerConnectionStatusDetails.SetUp(WindsorContainerManager.GetCompanyUserCounterParties(loginUser.CompanyUserID));

                    TradeManagerCore.SetCommunicationManager = _tradeCommManager;
                    TradeManager.TradeManager.GetInstance().SecMasterServices = _secMasterServices;
                    TradeManagerExtension.GetInstance().SetCommunicationManager = _tradeCommManager;
                    //added 02/01/2007 by harsh for TradeManagerSettings
                    TradeManager.TradeManagerCore.UserID = loginUser.CompanyUserID;

                    //http://jira.nirvanasolutions.com:8080/browse/SEA-50
                    // It was connecting to the trade server and instantly trade server was responding back with connected counterparties, even before
                    // the counterparty toolbar was created. Now moved it after creating the counterparty toolbar
                    _tradeCommManager.Connect(getTradeServerConnectionDetails());

                    BackgroundWorker bgWorkerCache = new BackgroundWorker();
                    bgWorkerCache.DoWork += new DoWorkEventHandler(bgWorkerCache_DoWork);
                    bgWorkerCache.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerCache_RunWorkerCompleted);
                    bgWorkerCache.RunWorkerAsync();

                    // removed from Prana Main to login form so as to get trades corresponding to user change
                    //Prana.TradeManager.TradeManager.GetInstance().LaunchTradingInstUI += new EventHandler(PranaMain_LaunchTradingInstUI);
                    Prana.TradeManager.TradeManager.GetInstance().UserID = loginUser.CompanyUserID;
                    Prana.TradeManager.TradeManager.GetInstance().CompanyID = loginUser.CompanyID;
                    Prana.TradeManager.TradeManager.GetInstance().AlgoReplaceEditHandler += new Prana.TradeManager.TradeManager.AlgoReplaceOrderEditHandler(PranaMain_AlgoReplaceEditHandler);
                    Prana.TradeManager.TradeManager.GetInstance().AlgoValidTradeToBlotterUI += new AlgoValidTradeHandler(PranaMain_AlgoValidTradeToBlotterUI);
                    Prana.TradeManager.TradeManager.GetInstance().DisplayCustomPopUp += new Prana.TradeManager.DisplayCustomPopUphandler(PranaMain_DisplayCustomPopUp);

                    if (null != loginUser)
                    {
                        //Set Company Name and Login UserName on Title

                        // For custom House release adding role
                        Prana.BusinessObjects.Authorization.NirvanaPrincipal authorizedPrincipal = AuthorizationManager.GetInstance()._authorizedPrincipal;
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                            if (authorizedPrincipal != null)
                            {
                                this.Text = WhiteLabelTheme.AppTitle
                                         + " - " + loginUser.CompanyName + " - "
                                         + loginUser.FirstName + " " + loginUser.LastName + " (" + authorizedPrincipal.Role.ToString() + ")" + ", v" + fvi.FileVersion;
                            }
                            else
                            {
                                this.Text = WhiteLabelTheme.AppTitle
                                         + " - " + loginUser.CompanyName + " - "
                                         + loginUser.FirstName + " " + loginUser.LastName + ", v" + fvi.FileVersion;
                            }
                        }
                        else
                        {
                            this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">&nbsp;&nbsp;&nbsp;&nbsp;" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                            this.ultraFormManager1.DrawFilter = new MainFormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, loginUser.FirstName + " " + loginUser.LastName + " - " + loginUser.CompanyName, CustomThemeHelper.TitleFont, CustomThemeHelper.UsedFont);
                            this.ultraFormManager1.CreationFilter = new CF(this);
                        }

                        this.Icon = WhiteLabelTheme.AppIcon;
                        Logger.LoggerWrite("LaunchLoginForm called, user logged in successfully. CompanyUserID: " + loginUser.CompanyUserID);
                    }

                    // Get All Company Modules Permitted to user
                    if (login.LoginStatus == Constants.LoginStatus.ValidUser)
                    {
                        ModuleManager.GetModulesForCompanyUser(loginUser.CompanyUserID);
                        ModuleManager.GetMarketDataColumnsNotForUser(loginUser.CompanyUserID);

                        AllocationSubModulePermission.SetAllocationSubModulePermission();
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

        private void FillLoggedInUserIPDetails()
        {
            try
            {
                // Fetch remote system IP
                string loggedInUserRemoteIPAddress = WTSUtility.GetLoggedInUserIPAddress(Process.GetCurrentProcess().SessionId);

                string loggedInUserLocalIPAddress = string.Empty;
                // Fetch local system IP
                foreach (System.Net.NetworkInformation.NetworkInterface ni in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                    if (ni.GetIPProperties().GatewayAddresses.FirstOrDefault() != null)
                        if (ni.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Ethernet)
                            foreach (System.Net.NetworkInformation.UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                    loggedInUserLocalIPAddress = ip.Address.ToString();

                // Fetch saved IP - for VPN
                if (!string.IsNullOrWhiteSpace(loginUser.MarketDataAccessIPAddresses))
                {
                    _loggedInUserIPAddress = loginUser.MarketDataAccessIPAddresses;

                    List<string> listPrivateIps = loginUser.MarketDataAccessIPAddresses.Split(',').ToList();

                    if (!listPrivateIps.Contains(loggedInUserRemoteIPAddress))
                        _loggedInUserIPAddress += "," + loggedInUserRemoteIPAddress;

                    if (!listPrivateIps.Contains(loggedInUserLocalIPAddress))
                        _loggedInUserIPAddress += "," + loggedInUserLocalIPAddress;
                }
                else
                {
                    _loggedInUserIPAddress = string.Format("{0},{1}", loggedInUserRemoteIPAddress, loggedInUserLocalIPAddress);
                }
                if (!string.IsNullOrWhiteSpace(_argsIPAddress))
                    _loggedInUserIPAddress += "," + _argsIPAddress;

                Logger.LoggerWrite("IP Details >> User ID: " + loginUser.CompanyUserID + ", Ip List: " + _loggedInUserIPAddress, true);
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

        void PranaMain_AlgoValidTradeToBlotterUI(object sender, EventArgs<OrderSingle> e)
        {
            try
            {
                OrderToUIThreadHandler mi = new OrderToUIThreadHandler(PranaMain_AlgoValidTradeToBlotterUI);
                if (UIValidation.GetInstance().validate(lblConnectionStatus))
                {
                    if (lblConnectionStatus.InvokeRequired)
                    {
                        this.BeginInvoke(mi, sender, e);
                    }
                    else
                    {
                        Prana.TradeManager.TradeManager.GetInstance().SendBlotterTrades(e.Value);
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

        void PranaMain_DisplayCustomPopUp(object sender, EventArgs<string, string> e)
        {
            try
            {
                Prana.TradeManager.DisplayCustomPopUphandler mi = new Prana.TradeManager.DisplayCustomPopUphandler(PranaMain_DisplayCustomPopUp);
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(mi, sender, e);
                    }
                    else
                    {
                        CustomMessageBox messagebox = new CustomMessageBox(e.Value, e.Value2, true, CustomThemeHelper.PENDINGNEW_POPUP, FormStartPosition.CenterScreen);
                        messagebox.ShowDialog();
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

        void PranaMain_AlgoReplaceEditHandler(object sender, EventArgs<OrderSingle> e)
        {
            try
            {
                AlgoCallback mi = new AlgoCallback(PranaMain_AlgoReplaceEditHandler);
                if (UIValidation.GetInstance().validate(lblConnectionStatus))
                {
                    if (lblConnectionStatus.InvokeRequired)
                    {
                        BeginInvoke(mi, sender, e);
                    }
                    else
                    {
                        LaunchTradingTicketDock(e.Value, TradingTicketParent.Blotter);
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

        void bgWorkerCache_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                CachedDataManagerRecon.RefreshCache(loginUser.CompanyUserID);
                Prana.ClientCommon.TradingTktPrefs.SetClientCache(loginUser);
                //CHMW-3113	Error on logging into client 
                SecMasterOTCServiceApi.GetInstance().GetOTCWorkflowPreference();
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

        void bgWorkerCache_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                OnCacheCompleted();
                //Moved from NirvanaMainload to here since the cache is being initiallised in background thread and the application was importing files even before initializing
                #region ReBalancer
                int allowedUserForReBalancer;
                if (int.TryParse(ConfigurationManager.AppSettings["AllowedUserForReBalancer"].ToString(), out allowedUserForReBalancer) && allowedUserForReBalancer == loginUser.CompanyUserID)
                {
                    ImportManager.Instance.Initialize(_secMasterServices);
                }
                #endregion
                SecMasterCommonCache.Instance.Initialize(_secMasterServices);
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

        private void StartImportScheduler()
        {
            try
            {
                int userIDAllowedToWrite = -1;
                int.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIGKEY_AllowedUserForScheduler).ToString(), out userIDAllowedToWrite);
                //Batch scheduler is allowed for support1 only
                if (loginUser.CompanyUserID == userIDAllowedToWrite)
                {
                    ImportManager.Instance.Initialize(_secMasterServices);
                    _schedFact = new StdSchedulerFactory();
                    // get a scheduler                       
                    _sched = _schedFact.GetScheduler();
                    _sched.Standby();
                    _sched.Start();
                    Dictionary<string, string> dictJobCron = ImportManager.Instance.GetImportBatchSchedules();

                    foreach (KeyValuePair<string, string> item in dictJobCron)
                    {
                        // define the job and tie it to HelloJob class
                        JobDetail jobdetail = new JobDetail(item.Key, "Import Batch", typeof(PranaMain));

                        //Creating cron trigger to handle job
                        CronTrigger cTrigger = new CronTrigger("Trigger_" + item.Key, "AUEC-Blotter-Clearance-Triggers", item.Key, "Import Batch", item.Value);
                        // Instruct quartz to schedule the job using trigger
                        _sched.DeleteJob(item.Key, "Import Batch");
                        _sched.ScheduleJob(jobdetail, cTrigger);
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

        private void OnCacheCompleted()
        {
            try
            {
                _onCacheCompleted = true;
                AfterCachingAndHandleCreation();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AfterCachingAndHandleCreation()
        {
            try
            {
                if (_isHandleCreated && _onCacheCompleted)
                {
                    EventHandler eventHandler = new EventHandler(UICreation);
                    this.BeginInvoke(eventHandler);
                    _onCacheCompleted = false;
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

        bool _isHandleCreated = false;
        bool _onCacheCompleted = false;
        void PranaMain_HandleCreated(object sender, EventArgs e)
        {
            try
            {
                _isHandleCreated = true;
                AfterCachingAndHandleCreation();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        ///  All Socket Connections are Established Here
        /// </summary>
        private void ConnectToAllSockets()
        {
            try
            {
                _connectionPricingServer = new ClientTradeCommManager();
                _tradeCommManager = new ClientTradeCommManager();
                _postTradeServicesInstance = new PostTradeServicesClient();

                _tradeCommManager.Disconnected += new EventHandler(_communicationManager_Disconnected);
                _tradeCommManager.Connected += new EventHandler(_communicationManager_Connected);

                //http://jira.nirvanasolutions.com:8080/browse/SEA-50
                // It was connecting to the trade server and instantly trade server was responding back with connected counterparties, even before
                // the counterparty toolbar was created. Now moved it after creating the counterparty toolbar
                //ConnectionProperties tradeConnProperties = getTradeServerConnectionDetails();
                //_tradeCommManager.Connect(tradeConnProperties);

                //  }

                _connectionPricingServer.Connected += new EventHandler(_PricingServerConnected);
                _connectionPricingServer.Disconnected += new EventHandler(_PricingServerDisconnected);
                _connectionPricingServer.Connect(GetPricingConnectionProperties());
                _pricingAnalysisInstance.ClientCommunicationManager = _connectionPricingServer;
                //Todo: _pricingAnalysis will be removed and _ConnectionPricingServer will be used for socket communication
                //_pricingAnalysisInstance.Connected += new EventHandler(_PricingServerConnected);
                //_pricingAnalysisInstance.Disconnected += new EventHandler(_PricingServerDisconnected);

                //_pricingAnalysisInstance.ConnectToServer(GetPricingConnectionProperties());
                _secMasterServices.ConnectToServer();
                _postTradeServicesInstance.ConnectToServer();
                _secMasterServices.TradeCommunicationManager = _tradeCommManager;
                _secMasterServices.Disconnected += new EventHandler(_secMasterServices_Disconnected);
                _secMasterServices.Connected += new EventHandler(_secMasterServices_Connected);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        delegate void SetTextCallback2(object sender, EventArgs<string> e);
        void _secMasterServices_Connected(object sender, EventArgs e)
        {
            try
            {
                if (_secMasterServices.TradeCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                    _secMasterServices.IsConnected = true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        void _secMasterServices_Disconnected(object sender, EventArgs e)
        {
            try
            {
                if (_secMasterServices.TradeCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.DISCONNECTED
                    || _secMasterServices.TradeCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.NOSERVER)
                    _secMasterServices.IsConnected = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private ConnectionProperties getTradeServerConnectionDetails()
        {
            ConnectionProperties connProperties = new ConnectionProperties();
            try
            {
                connProperties.ServerIPAddress = ClientAppConfiguration.TradeServer.IpAddress;
                connProperties.Port = ClientAppConfiguration.TradeServer.Port;
                connProperties.User = loginUser;
                connProperties.ConnectedServerName = "Trade ";
                connProperties.HandlerType = HandlerType.TradeHandler;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return connProperties;
        }

        /// <summary>
        /// For Connection to exposurePNL Servives
        /// </summary>
        private ConnectionProperties getExpnlConnectionProperties()
        {
            ConnectionProperties connProperties = new ConnectionProperties();
            try
            {
                connProperties.ServerIPAddress = ClientAppConfiguration.ExpnlServer.IpAddress;
                connProperties.Port = ClientAppConfiguration.ExpnlServer.Port;
                connProperties.User = loginUser;
                connProperties.ConnectedServerName = "Exposure Pnl ";
                connProperties.HandlerType = HandlerType.ExposurePnlHandler;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return connProperties;
        }

        void _exposurePNLCommunicationManager_Connected(object sender, EventArgs e)
        {
            try
            {
                SetExPnlServiceConnectionStatus();
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
        /// If Exposure PNL Service  is Disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _exposurePNLCommunicationManager_Disconnected(object sender, EventArgs e)
        {
            try
            {
                SetExPnlServiceConnectionStatus();
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

        IMarkPriceUI markPriceUIInstance = null;
        Form markPriceUIForm = null;
        void LaunchMarkPriceUI(Form parentForm)
        {
            try
            {
                if (!_isPMPermitted)
                {
                    MessageBox.Show("You do not have PM permissions, please contact to Adminstrator.", "Portfolio Management");
                    return;
                }
                if (ModuleManager.CheckModulePermissioning(PranaModules.DAILY_VALUATION_MODULE, PranaModules.DAILY_VALUATION_MODULE))
                {
                    if (markPriceUIInstance == null)
                    {
                        DynamicClass formToLoad;

                        formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails[PranaModules.DAILY_VALUATION_MODULE];

                        Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);
                        Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);

                        markPriceUIInstance = (IMarkPriceUI)Activator.CreateInstance(typeToLoad);
                        markPriceUIInstance.FormClosed += new EventHandler(markPriceUIInstance_FormClosed);
                        markPriceUIInstance.LoginUser = loginUser;
                        // sud: need to check it.
                        markPriceUIInstance.PricingAnalysis = _pricingAnalysisInstance;
                        markPriceUIForm = markPriceUIInstance.Reference();
                        markPriceUIForm.Owner = this;
                        markPriceUIForm.ShowInTaskbar = false;
                        markPriceUIForm.Show();
                        SetFromLayoutDetail(markPriceUIForm);
                    }
                    BringFormToFront(markPriceUIForm, GetFormNewLocation(parentForm));
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

        void markPriceUIInstance_FormClosed(object sender, EventArgs e)
        {
            try
            {
                markPriceUIInstance = null;
                markPriceUIForm = null;
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

        IClosingUI closingUIInstance = null;
        Form closingUIForm = null;
        void LaunchClosingUI(object sender, AllocationGroup group)
        {
            try
            {
                Form parentForm = null;
                AllocationClient allocationClientForm = null;
                if (sender is Form)
                    parentForm = sender as Form;
                if (sender is AllocationClient)
                    allocationClientForm = sender as AllocationClient;
                // Modified by Ankit Gupta on 17 Sep 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1445
                if (!_isPMPermitted)
                {
                    MessageBox.Show("You do not have PM permissions, please contact Administrator.", "Portfolio Management");
                    return;
                }
                if (ModuleManager.CheckModulePermissioning(PranaModules.CLOSE_POSITIONS_MODULE, PranaModules.CLOSE_POSITIONS_MODULE))
                {
                    if (!OpenModulesList.Contains(PranaModules.CLOSE_POSITIONS_MODULE))
                        OpenModulesList.Add(PranaModules.CLOSE_POSITIONS_MODULE);
                    if (closingUIInstance == null)
                    {
                        closingUIInstance = _container.Resolve<IClosingUI>();
                        closingUIInstance.SecurityMaster = _secMasterServices;
                        this.ApplyPreferences += new EventHandler<EventArgs<string, IPreferenceData>>(closingUIInstance.ApplyPreferences);
                        closingUIInstance.FormClosed += new EventHandler(closingUIInstance_FormClosed);
                        closingUIForm = closingUIInstance.Reference();
                        closingUIInstance.User = loginUser;
                        closingUIInstance.SetParentFormAndCreateProxies(((parentForm != null && parentForm.Name.Equals("AllocationMain")) || (allocationClientForm != null && allocationClientForm.Title.Equals("Allocation"))) ? true : false);
                        closingUIForm.Owner = this;
                        closingUIForm.ShowInTaskbar = false;
                        closingUIForm.Show();

                        if (parentForm != null)
                            closingUIInstance.SetUp(parentForm.Name, group);
                        else if (allocationClientForm != null)
                            closingUIInstance.SetUp(allocationClientForm.Title, group);
                        else
                            closingUIInstance.SetUp(string.Empty, group);
                        SetFromLayoutDetail(closingUIForm);
                    }
                    else
                    {
                        if (parentForm != null)
                            closingUIInstance.SetUp(parentForm.Name, group);
                        else if (allocationClientForm != null)
                            closingUIInstance.SetUp(allocationClientForm.Title, group);
                        else
                            closingUIInstance.SetUp(string.Empty, group);
                        BringFormToFront(closingUIForm, GetFormNewLocation(parentForm));
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

        void closingUIInstance_FormClosed(object sender, EventArgs e)
        {
            try
            {
                closingUIInstance = null;
                closingUIForm = null;
                if (OpenModulesList.Contains(PranaModules.CLOSE_POSITIONS_MODULE))
                    OpenModulesList.Remove(PranaModules.CLOSE_POSITIONS_MODULE);
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

        #region post recon amendment UI
        IPostReconAmendmentsUI postReconAmendmentsUIInstance = null;
        Form postReconAmendmentUIForm = null;
        void LaunchPostReconAmendmentUI(int accountID, string symbol, DateTime date, string comment, EventHandler UpdateCommentsFromPostReconAmendments)
        {
            try
            {
                //CHMW-2126	[Post Recon Amendments] From recon dashboard , UI doesn't work and only data updates in already opened UI from Setup and Run Reconciliation
                if (postReconAmendmentsUIInstance != null)
                {
                    postReconAmendmentUIForm.Close();
                }
                if (postReconAmendmentsUIInstance == null)
                {
                    postReconAmendmentsUIInstance = _container.Resolve<IPostReconAmendmentsUI>();
                    postReconAmendmentsUIInstance.SecurityMaster = _secMasterServices;
                    postReconAmendmentsUIInstance.FormClosed += new EventHandler(postReconAmendment_FormClosed);

                    if (postReconAmendmentsUIInstance is ILaunchForm)
                    {
                        ((ILaunchForm)postReconAmendmentsUIInstance).LaunchForm += new EventHandler(postReconAmendments_LaunchPostTransactionForm);
                    }
                    postReconAmendmentUIForm = postReconAmendmentsUIInstance.Reference();
                    postReconAmendmentsUIInstance.SetUp(accountID, symbol, date, comment, UpdateCommentsFromPostReconAmendments);

                    // Modified by Ankit Gupta on 27, Oct 2014.
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1674l
                    // When Recon Cancel-Amend Form is closed, Post Recon Amendments form should also be closed.
                    // As this code is in 'NirvanaMain', therefore could not directly access Recon Cancel-Amend Form, using this.FindForm(),
                    // Therefore, parsing through all the owned forms of NirvanaMain
                    foreach (Form form in this.OwnedForms)
                    {
                        if (form.Text.ToString().Equals("Recon Cancel-Amend Form"))
                        {
                            postReconAmendmentUIForm.Owner = form;
                        }
                    }
                    postReconAmendmentUIForm.ShowInTaskbar = false;
                    postReconAmendmentUIForm.Show();
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

        // Modified by Ankit Gupta on 09 Sep, 2014
        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1217
        private void postReconAmendments_LaunchPostTransactionForm(object sender, EventArgs e)
        {
            try
            {
                LaunchTradingTicketDock(TradingTicketParent.None);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void postReconAmendment_FormClosed(object sender, EventArgs e)
        {
            try
            {
                postReconAmendmentsUIInstance = null;
                postReconAmendmentUIForm = null;
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


        Form formCA = null;
        ICorporateActions _corporateActionInstance = null;
        private void LaunchCorporateActionsUI(Form parentForm)
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.CORPORATE_ACTIONS_MODULE, PranaModules.CORPORATE_ACTIONS_MODULE))
                {
                    if (!OpenModulesList.Contains(PranaModules.CORPORATE_ACTIONS_MODULE))
                        OpenModulesList.Add(PranaModules.CORPORATE_ACTIONS_MODULE);
                    if (_corporateActionInstance == null)
                    {
                        try
                        {
                            _corporateActionInstance = _container.Resolve<ICorporateActions>();
                            _corporateActionInstance.LaunchSymbolLookup += new EventHandler(corporateActionInstance_LaunchSymbolLookup);
                            _corporateActionInstance.FormClosed += new EventHandler(corporateActionInstance_FormClosed);

                            formCA = _corporateActionInstance.Reference();
                            _corporateActionInstance.InitControl();
                            _corporateActionInstance.LoginUser = loginUser;
                            formCA.Owner = this;
                            formCA.ShowInTaskbar = false;
                            formCA.Show();
                            SetFromLayoutDetail(formCA);
                            BringFormToFront(formCA, GetFormNewLocation(parentForm));
                        }
                        catch (Exception)
                        {
                            InformationMessageBox.Display("Unable to load the Corporate Action module!");
                            _corporateActionInstance = null;
                        }
                    }
                    BringFormToFront(formCA, GetFormNewLocation(parentForm));
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

        void corporateActionInstance_LaunchSymbolLookup(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                _arguments = e;
                LaunchPluggableTool(ApplicationConstants.CONST_SYMBOL_LOOKUP, GetFormNewLocation(parentForm));
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

        void corporateActionInstance_FormClosed(object sender, EventArgs e)
        {
            try
            {
                formCA = null;
                _corporateActionInstance = null;
                if (OpenModulesList.Contains(PranaModules.CORPORATE_ACTIONS_MODULE))
                    OpenModulesList.Remove(PranaModules.CORPORATE_ACTIONS_MODULE);
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
        ICreateTransaction createTransactionInstance = null;
        Form createTransactionForm = null;

        void LaunchCreateTransaction(Form parentForm)
        {
            try
            {
                if (!_isPMPermitted)
                {
                    MessageBox.Show("You do not have PM permissions, please contact to Adminstrator.", "Portfolio Management");
                    return;
                }

                if (ModuleManager.CheckModulePermissioning(PranaModules.CREATE_TRANSACTION_MODULE, PranaModules.CREATE_TRANSACTION_MODULE))
                {
                    if (createTransactionInstance == null)
                    {
                        createTransactionInstance = _container.Resolve<ICreateTransaction>();
                        if (createTransactionInstance != null)
                        {
                            createTransactionInstance.SecurityMaster = _secMasterServices;
                            createTransactionInstance.FormClosed += new EventHandler(createTransactionInstance_FormClosed);
                            createTransactionInstance.SetUp();
                            createTransactionForm = createTransactionInstance.Reference();
                            createTransactionInstance.InitControl(loginUser);
                            createTransactionForm.Owner = this;
                            createTransactionForm.ShowInTaskbar = false;
                            createTransactionForm.Show();
                            SetFromLayoutDetail(createTransactionForm);
                            BringFormToFront(createTransactionForm, GetFormNewLocation(parentForm));
                        }
                    }
                    else
                    {
                        BringFormToFront(createTransactionForm, GetFormNewLocation(parentForm));
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

        void createTransactionInstance_FormClosed(object sender, EventArgs e)
        {
            try
            {
                createTransactionForm = null;
                createTransactionInstance = null;
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

        Prana.WashSale.WashSale washSaleForm = null;

        void LaunchWashSale(Form parentForm)
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.WASH_SALE_MODULE, PranaModules.WASH_SALE_MODULE))
                {
                    if (!OpenModulesList.Contains(PranaModules.WASH_SALE_MODULE))
                        OpenModulesList.Add(PranaModules.WASH_SALE_MODULE);
                    if (washSaleForm == null)
                    {
                        washSaleForm = new WashSale.WashSale();
                        washSaleForm.FormClosed += new EventHandler(washSaleInstance_FormClosed);
                        washSaleForm.Owner = this;
                        washSaleForm.ShowInTaskbar = false;
                        washSaleForm.Show();
                        SetFromLayoutDetail(washSaleForm);
                        BringFormToFront(washSaleForm, GetFormNewLocation(parentForm));
                    }
                    else
                    {
                        BringFormToFront(washSaleForm, GetFormNewLocation(parentForm));
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

        void washSaleInstance_FormClosed(object sender, EventArgs e)
        {
            try
            {
                washSaleForm = null;
                if (OpenModulesList.Contains(PranaModules.WASH_SALE_MODULE))
                    OpenModulesList.Remove(PranaModules.WASH_SALE_MODULE);
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

        INAVLockUI navLockInstance = null;
        Form navLockForm = null;

        void LaunchNAVLock()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.NAV_LOCK_MODULE, PranaModules.NAV_LOCK_MODULE))
                {
                    if (navLockInstance == null && navLockForm == null)
                    {
                        DynamicClass formToLoad;
                        formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails[PranaModules.NAV_LOCK_MODULE];

                        Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);

                        Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);

                        navLockInstance = (INAVLockUI)Activator.CreateInstance(typeToLoad);
                        navLockInstance.FormClosedHandler += new EventHandler(navLockInstance_FormClosed);
                        navLockForm = navLockInstance.Reference();
                        navLockInstance.LoginUser = loginUser;
                        navLockForm.Owner = this;
                        navLockForm.ShowInTaskbar = false;
                        navLockForm.Show();
                        SetFromLayoutDetail(navLockForm);
                    }
                    BringFormToFront(navLockForm, GetFormNewLocation(null));
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

        void navLockInstance_FormClosed(object sender, EventArgs e)
        {
            try
            {
                INAVLockUI nAVLock = sender as INAVLockUI;
                if (_container != null)
                {
                    _container.Release(nAVLock);
                }
                nAVLock.FormClosedHandler -= navLockInstance_FormClosed;
                navLockInstance = null;
                navLockForm = null;
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

        void portfolioManagementInstance_ConsolidationViewTradeClickToMain(object sender, EventArgs<OrderSingle, Dictionary<int, double>> e)
        {
            try
            {
                LaunchTicketFromPMConsolidationView(e);
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

        private void LaunchTicketFromPMConsolidationView(EventArgs<OrderSingle, Dictionary<int, double>> e)
        {
            try
            {
                OrderSingle order = e.Value;
                if (order != null)
                {
                    FillPreferencesInOrder(order);
                    order.CumQty = order.Quantity;
                    LaunchTradingTicketDock(order, TradingTicketParent.PM, 0, e.Value2);
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

        void _shortLocate_MouseDoubleClickOnRow(object sender, EventArgs e)
        {
            try
            {
                LaunchTicketFromShortLocate(e);
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

        private void LaunchTicketFromShortLocate(EventArgs e)
        {
            try
            {
                //  OrderSingle order = (OrderSingle)e;
                LaunchFormEventArgs ea = (LaunchFormEventArgs)e;
                OrderSingle order = (OrderSingle)ea.Params;
                if (order != null)
                {
                    FillPreferencesInOrder(order);
                    order.CumQty = order.Quantity;
                    order.TransactionSource = TransactionSource.ShortLocate;
                    LaunchTradingTicketDock(order, TradingTicketParent.ShortLocate);
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

        private void FillPreferencesInOrder(OrderSingle order)
        {
            try
            {
                TradingTicketUIPrefs userTradingTicketUiPrefs = TradingTktPrefs.UserTradingTicketUiPrefs;
                TradingTicketUIPrefs companyTradingTicketUiPrefs = TradingTktPrefs.CompanyTradingTicketUiPrefs;
                CounterPartyWiseCommissionBasis CommisionUserTTUiPrefs = TradingTktPrefs.CpwiseCommissionBasis;

                if (userTradingTicketUiPrefs != null && companyTradingTicketUiPrefs != null && CommisionUserTTUiPrefs != null)
                {
                    if (userTradingTicketUiPrefs.Broker.HasValue && CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue.ContainsKey(userTradingTicketUiPrefs.Broker.Value))
                    {
                        order.VenueID = CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue[userTradingTicketUiPrefs.Broker.Value];
                    }
                    else if (companyTradingTicketUiPrefs.Venue.HasValue)
                    {
                        order.VenueID = companyTradingTicketUiPrefs.Venue.Value;
                    }

                    if (userTradingTicketUiPrefs.Broker.HasValue)
                    {
                        order.CounterPartyID = userTradingTicketUiPrefs.Broker.Value;
                    }
                    else if (companyTradingTicketUiPrefs.Broker.HasValue)
                    {
                        order.CounterPartyID = companyTradingTicketUiPrefs.Broker.Value;
                    }

                    if (userTradingTicketUiPrefs.OrderType.HasValue)
                    {
                        order.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValueBasedOnID(userTradingTicketUiPrefs.OrderType.Value.ToString());
                    }
                    else if (companyTradingTicketUiPrefs.OrderType.HasValue)
                    {
                        order.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValueBasedOnID(companyTradingTicketUiPrefs.OrderType.Value.ToString());
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

                    if (userTradingTicketUiPrefs.IsSettlementCurrencyBase.HasValue)
                    {
                        order.SettlementCurrencyID = (bool)userTradingTicketUiPrefs.IsSettlementCurrencyBase ? CachedDataManager.GetInstance.GetCompanyBaseCurrencyID() : order.CurrencyID;
                    }
                    else if (companyTradingTicketUiPrefs.IsSettlementCurrencyBase.HasValue)
                    {
                        order.SettlementCurrencyID = (bool)companyTradingTicketUiPrefs.IsSettlementCurrencyBase ? CachedDataManager.GetInstance.GetCompanyBaseCurrencyID() : order.CurrencyID;
                    }

                    if (userTradingTicketUiPrefs.TradingAccount.HasValue)
                    {
                        order.TradingAccountID = userTradingTicketUiPrefs.TradingAccount.Value;
                    }
                    else if (companyTradingTicketUiPrefs.TradingAccount.HasValue)
                    {
                        order.TradingAccountID = companyTradingTicketUiPrefs.TradingAccount.Value;
                    }

                    if (userTradingTicketUiPrefs.Strategy.HasValue)
                    {
                        order.Level2ID = int.Parse(userTradingTicketUiPrefs.Strategy.ToString());
                    }
                    else if (companyTradingTicketUiPrefs.Strategy.HasValue)
                    {
                        order.Level2ID = int.Parse(companyTradingTicketUiPrefs.Strategy.ToString());
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

        #endregion

        #region Enable Logout Login Events
        private void UICreation(object sender, EventArgs e)
        {
            try
            {
                LoadServerReportsForUser();
                LoadLayoutForUser();
                if (ModuleManager.PlugIns != null)
                {
                    foreach (KeyValuePair<string, IPlugin> item in ModuleManager.PlugIns.Values)
                    {
                        string strPlugInType = item.Key;
                        Form plugInForm = null;
                        if (strPlugInType.ToUpper().Equals(ApplicationConstants.PlugInType.StartUp.ToString().ToUpper()))
                        {
                            plugInForm = item.Value.Execute(strPlugInType,
                                                            AMSConnectionString,
                                                            loginUser.FirstName + " " + loginUser.LastName,
                                                            loginUser.CompanyUserID);
                            plugInForm.Owner = this;
                            if (!activePlugInForms.ContainsKey(plugInForm.Text))
                            {
                                activePlugInForms.Add(plugInForm.Text, plugInForm);
                            }
                            item.Value.FormClosed += new IPlugin.FormClosedEventHandler(PluginFormClosed);
                            item.Value.VisibleChanged += new IPlugin.VisibleChangedEventHandler(PluginFormVisibleChanged);
                            item.Value.Shown += new IPlugin.ShownEventHandler(PluginFormVisibleChanged);
                            BringFormToFront(plugInForm, GetFormNewLocation(this));
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
        #endregion

        private int _layoutID = 0;
        private string _layoutName = string.Empty;

        #region ToolBar Button Clicks
        private void menuHelpAbout_Click(object sender, System.EventArgs e)
        {
            try
            {
                LaunchHelpForm();
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
        /// Launch TradingTicket Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileNewTradingTicket_Click(object sender, System.EventArgs e)
        {
            try
            {
                LaunchTradingTicketDock(TradingTicketParent.None);
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
        /// Allocation Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuFileAllocation_Click(object sender, System.EventArgs e)
        {
            try
            {
                LaunchAllocationForm();
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

        /// <summary>
        /// Launch Trading Ticket Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuTradingTicket_Click(object sender, EventArgs e)
        {
            try
            {
                LaunchTradingTicketDock(TradingTicketParent.None);
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
        /// Launch Short Locate Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuShortLocate_Click(object sender, EventArgs e)
        {
            try
            {
                LaunchShortLocate();
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
        /// Launch Rebalancer Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuRebalancer_Click(object sender, EventArgs e)
        {
            try
            {
                LaunchRebalancer();
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

        private void mnuTradingTicketDock_Click(object sender, System.EventArgs e)
        {
            try
            {
                LaunchTradingTicketDock(TradingTicketParent.None);
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

        Prana.LiveFeed.UI.Forms.FactSetAuthenticationForm factsetAuthForm = null;

        private void LaunchFactSetAuthUI(Form parentForm)
        {
            try
            {
                if (factsetAuthForm == null)
                {
                    factsetAuthForm = new FactSetAuthenticationForm();
                    factsetAuthForm.Owner = this;
                    factsetAuthForm.ShowInTaskbar = false;
                    factsetAuthForm.FormClosed += new FormClosedEventHandler(FactSetAuthInstance_FormClosed);
                    factsetAuthForm.SetTokenFactsetUser += PranaMain_SetTokenFactsetUser;
                    factsetAuthForm.CloseTheFactSetForm += PranaMain_CloseFactsetForm;
                    factsetAuthForm.Show();
                }
                int x = this.Location.X + (this.Size.Width - factsetAuthForm.Size.Width) / 2;
                int y = this.Location.Y + this.Size.Height;
                BringFormToFront(factsetAuthForm, new Point(x, y));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void FactSetAuthInstance_FormClosed(object sender, EventArgs e)
        {
            try
            {
                if (factsetAuthForm != null)
                {
                    factsetAuthForm.SetTokenFactsetUser -= PranaMain_SetTokenFactsetUser;
                    factsetAuthForm.CloseTheFactSetForm -= PranaMain_CloseFactsetForm;
                }
                factsetAuthForm = null;
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

        private void btnLiveFeedConnect_Click(object sender, EventArgs e)
        {
            try
            {
                LaunchFactSetAuthUI(null);
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
        /// All Plugin preferences integrated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPreferences_Click(object sender, System.EventArgs e)
        {
            try
            {
                LaunchPreferncesModule("", GetFormNewLocation(null));
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

        private void mnuQualityChecker_Click(object sender, System.EventArgs e)
        {
            try
            {
                LaunchQualityCheckerModule();
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

        private void mnuBlotterReports_Click(object sender, System.EventArgs e)
        {
            try
            {
                LaunchBlotterReportsForm();
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

        private void LaunchBlotterReportsForm()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.BLOTTER_EXECUTION_REPORT_MODULE, PranaModules.BLOTTER_EXECUTION_REPORT_MODULE))
                {
                    if (reportForm == null && blotterReportsInstance == null)
                    {
                        DynamicClass formToLoad;
                        formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails[PranaModules.BLOTTER_EXECUTION_REPORT_MODULE];

                        Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);

                        Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);
                        blotterReportsInstance = (IBlotterReports)Activator.CreateInstance(typeToLoad);
                        blotterReportsInstance.LoginUser = loginUser;

                        reportForm = blotterReportsInstance.Reference();
                        reportForm.Owner = this;
                        reportForm.ShowInTaskbar = false;
                        reportForm.FormClosed += reportForm_FormClosed;
                        reportForm.Show();
                        SetFromLayoutDetail(reportForm);
                    }
                    BringFormToFront(reportForm, GetFormNewLocation(null));
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
        /// Handles the FormClosed event of the reportForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        void reportForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                reportForm.FormClosed -= reportForm_FormClosed;
                reportForm = null;
                blotterReportsInstance = null;
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

        private void mnuThirdPartyManager_Click(object sender, System.EventArgs e)
        {
            try
            {
                LaunchThirdPartyManager();
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

        IThirdPartyReport thirdPartyReportInstance = null;

        private void LaunchThirdPartyManager()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.THIRD_PARTY_MANAGER_MODULE, PranaModules.THIRD_PARTY_MANAGER_MODULE))
                {
                    if (thirdPartyReportFrom == null && thirdPartyReportInstance == null)
                    {
                        DynamicClass formToLoad;
                        formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails[PranaModules.THIRD_PARTY_MANAGER_MODULE];

                        Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);

                        Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);

                        thirdPartyReportInstance = (IThirdPartyReport)Activator.CreateInstance(typeToLoad);
                        thirdPartyReportInstance.ThirdPartyFlatFileClosed += new EventHandler(thirdPartyReportInstance_ThirdPartyFlatFileClosed);
                        thirdPartyReportFrom = thirdPartyReportInstance.Reference();
                        thirdPartyReportInstance.LoginUser = loginUser;
                        thirdPartyReportFrom.Owner = this;
                        thirdPartyReportFrom.ShowInTaskbar = false;
                        thirdPartyReportInstance.GoToAllocationClicked += HandleGoToAllocationClick;
                        thirdPartyReportFrom.Show();
                        SetFromLayoutDetail(thirdPartyReportFrom);
                    }
                    BringFormToFront(thirdPartyReportFrom, GetFormNewLocation(null));
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

        void thirdPartyReportInstance_ThirdPartyFlatFileClosed(object sender, EventArgs e)
        {
            try
            {
                thirdPartyReportInstance = null;
                thirdPartyReportFrom = null;
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


        #region Layout Save Methods
        /// <summary>
        /// Save Layout settings corresponding to the LayoutID in the db and as an xml file
        /// </summary>
        private int SaveLayout()
        {
            int result = int.MinValue;
            try
            {
                DataTable dt = GetLayoutTableSchema();

                AddLayoutRowToTable(dt, this);
                for (int i = 0; i < this.OwnedForms.Length; i++)
                {
                    AddLayoutRowToTable(dt, this.OwnedForms[i]);
                }

                //Custom Handling for saving layout of New Allocation Window
                if (allocationClientForm != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["LeftX"] = allocationClientForm.Left;
                    dr["RightY"] = allocationClientForm.Top;
                    dr["Height"] = allocationClientForm.Height;
                    dr["Width"] = allocationClientForm.Width;
                    dr["WindowState"] = allocationClientForm.WindowState;
                    dr["IsInUse"] = 0;
                    dr["LayoutID"] = _layoutID;
                    dr["ModuleID"] = 8;         // As we are doing custom handling for Allocation, so no need to get module Id on the basis of Module Name. Removed extra Db call. Need to make a generic method here.
                    dr["UserID"] = loginUser.CompanyUserID;
                    dt.Rows.Add(dr);
                }

                //Handling for saving layout of Percent Trading Tool
                if (percentTradingToolWindow != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["LeftX"] = percentTradingToolWindow.Left;
                    dr["RightY"] = percentTradingToolWindow.Top;
                    dr["Height"] = percentTradingToolWindow.Height;
                    dr["Width"] = percentTradingToolWindow.Width;
                    dr["WindowState"] = percentTradingToolWindow.WindowState;
                    dr["IsInUse"] = 0;
                    dr["LayoutID"] = _layoutID;
                    dr["ModuleID"] = 62;   // As we are doing custom handling for PTT, so no need to get module Id on the basis of Module Name. Removed extra Db call. Need to make a generic method here.
                    dr["UserID"] = loginUser.CompanyUserID;
                    dt.Rows.Add(dr);
                }

                //Handling for saving layout of User Profile
                if (_userProfileWindow != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["LeftX"] = _userProfileWindow.Left;
                    dr["RightY"] = _userProfileWindow.Top;
                    dr["Height"] = _userProfileWindow.Height;
                    dr["Width"] = _userProfileWindow.Width;
                    dr["WindowState"] = _userProfileWindow.WindowState;
                    dr["IsInUse"] = 0;
                    dr["LayoutID"] = _layoutID;
                    dr["ModuleID"] = 63;   // As we are doing custom handling for PTT, so no need to get module Id on the basis of Module Name. Removed extra Db call. Need to make a generic method here.
                    dr["UserID"] = loginUser.CompanyUserID;
                    dt.Rows.Add(dr);
                }

                if (frmRebalancer != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["LeftX"] = frmRebalancer.Left;
                    dr["RightY"] = frmRebalancer.Top;
                    dr["Height"] = frmRebalancer.Height;
                    dr["Width"] = frmRebalancer.Width;
                    dr["WindowState"] = frmRebalancer.WindowState;
                    dr["IsInUse"] = 0;
                    dr["LayoutID"] = _layoutID;
                    dr["ModuleID"] = 60;   // As we are doing custom handling for Rebalancer, so no need to get module Id on the basis of Module Name. Removed extra Db call. Need to make a generic method here.
                    dr["UserID"] = loginUser.CompanyUserID;
                    dt.Rows.Add(dr);
                }

                //Handling for saving layout of Wash Sale
                if (washSaleForm != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["LeftX"] = washSaleForm.Left;
                    dr["RightY"] = washSaleForm.Top;
                    dr["Height"] = washSaleForm.Height;
                    dr["Width"] = washSaleForm.Width;
                    dr["WindowState"] = washSaleForm.WindowState;
                    dr["IsInUse"] = 0;
                    dr["LayoutID"] = _layoutID;
                    dr["ModuleID"] = 73;   // As we are doing custom handling for WasSale, so no need to get module Id on the basis of Module Name. Removed extra Db call. Need to make a generic method here.
                    dr["UserID"] = loginUser.CompanyUserID;
                    dt.Rows.Add(dr);
                }

                if (_layoutID > 0)
                {
                    result = LayoutManager.SaveLayout(dt, _layoutID);
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

        private void AddLayoutRowToTable(DataTable dt, Form form)
        {
            try
            {
                int moduleID = LayoutManager.GetModuleID(form.Name);
                if (moduleID != 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["LeftX"] = form.Left;
                    dr["RightY"] = form.Top;
                    dr["Height"] = form.Height;
                    dr["Width"] = form.Width;
                    dr["WindowState"] = form.WindowState;
                    dr["IsInUse"] = 0;
                    dr["LayoutID"] = _layoutID;
                    dr["ModuleID"] = moduleID;
                    dr["UserID"] = loginUser.CompanyUserID;
                    if (form is IQuickTradingTicket)
                    {
                        dr["QTTIndex"] = (form as IQuickTradingTicket).QTTIndex + 1;
                    }
                    dt.Rows.Add(dr);
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

        bool _loadingLayout = false;
        /// <summary>
        /// LoadLayout loads the setting corresponding to the LayoutID mentioned. 
        /// </summary>
        private void LoadLayout()
        {
            try
            {
                if (!_loadApplicationFirstTime)
                {
                    for (int i = this.OwnedForms.Length; i > 0; i--)
                    {
                        this.OwnedForms[i - 1].Close();
                    }
                    //special handling for blotter as blotter is running on a different thread and therefore prana Main form is not set as its owner...
                    CloseBlotterForm();
                }

                _loadingLayout = true;

                DataTable dt = LayoutManager.GetLayoutDetails(_layoutID, loginUser.CompanyUserID);


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Form currentForm = null;
                    FormWindowState windowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), dt.Rows[i]["WindowState"].ToString());
                    switch (int.Parse(dt.Rows[i]["ModuleID"].ToString()))
                    {
                        case 1:
                            LaunchBlotterFormNew();
                            break;
                        case 3:
                            LaunchTradingTicketDock(TradingTicketParent.None);
                            currentForm = tradingTicketDock;
                            break;
                        case 8:
                            LaunchAllocationForm();
                            currentForm = allocationForm;
                            break;
                        case 14:
                            LaunchPortfolioManagementForm();
                            currentForm = portfolioManagementForm;
                            break;
                        case 19:
                            LaunchPreferncesModule("", GetFormNewLocation(null));
                            currentForm = preferencesMain;
                            break;
                        case 20:
                            LaunchBlotterReportsForm();
                            currentForm = reportForm;
                            break;
                        case 22:
                            LaunchDailySheetReports();
                            currentForm = positionManagementDailySheetForm;
                            break;
                        case 23:
                            LaunchMonthlySheets();
                            currentForm = positionManagementMonthlySheetForm;
                            break;
                        case 32:
                            LaunchCreateTransaction(null);
                            currentForm = createTransactionForm;
                            break;
                        case 33:
                            LaunchClosingUI(null, null);
                            currentForm = closingUIForm;
                            break;
                        case 34:
                            LaunchMarkPriceUI(null);
                            currentForm = markPriceUIForm;
                            break;
                        case 24:
                        case 25:
                        case 36:
                        case 37:
                        case 38:
                            break;
                        case 39:
                            LaunchCorporateActionsUI(null);
                            currentForm = formCA;
                            break;
                        case 40:
                        case 41:
                        case 43:
                        case 51:
                        case 57:
                        case 58:
                        case 59:
                            string pluggableModuleName = dt.Rows[i]["ModuleName"].ToString();
                            currentForm = LaunchPluggableTool(pluggableModuleName, GetFormNewLocation(null));
                            break;
                        case 42:
                            LaunchPluggableTool("Import Data", GetFormNewLocation(null));
                            break;
                        case 44:
                            LaunchPluggableTool("Reconciliation", GetFormNewLocation(null));
                            break;
                        case 45:
                            LaunchMappingUI(new EventArgs(), null);
                            currentForm = mappingForm;
                            break;
                        case 46:
                            LaunchAccruals();
                            currentForm = accruals;
                            break;

                        case 49:
                            LaunchCashAccountsUI();
                            currentForm = cashAccountForm;
                            break;
                        case 53:
                            break;
                        case 54:
                            LaunchCashManagement();
                            currentForm = cashManagement;
                            break;
                        case 55:
                            string pluggableModuleName1 = dt.Rows[i]["ModuleName"].ToString();
                            currentForm = LaunchPluggableTool(pluggableModuleName1, GetFormNewLocation(null));
                            break;
                        case 56:
                            LaunchPluggableTool("Pricing Inputs", GetFormNewLocation(null));
                            break;
                        case 60:
                            LaunchRebalancer();
                            break;
                        case 61:
                            LaunchThirdPartyManager();
                            currentForm = thirdPartyReportFrom;
                            break;
                        case 62:
                            LaunchPercentTradingTool(true);
                            if (percentTradingToolWindow != null)
                                SetLayoutPercentTradingTool(dt.Rows[i], windowState);
                            break;
                        case 63:
                            LaunchUserProfile();
                            break;
                        case 64:
                            LaunchWatchList();
                            currentForm = _watchList;
                            break;
                        case 65:
                            LaunchAuditUI(new KeyValuePair<string, object[]>("none", null), GetFormNewLocation(this));
                            break;
                        case 66:
                            LaunchPluggableTool("Auto Import Data", GetFormNewLocation(null));
                            break;
                        case 67:
                            LaunchPluggableTool("Missing Trades", GetFormNewLocation(null));
                            break;
                        case 68:
                            LaunchShortLocate();
                            break;
                        case 69:
                            LaunchPluggableTool("Middleware Manager", GetFormNewLocation(null));
                            break;
                        case 70:
                            LaunchQualityCheckerModule();
                            break;
                        case 71:
                            LaunchOptionChain();
                            break;
                        case 72:
                            LaunchQuickTradingTicket(int.Parse(dt.Rows[i]["QTTIndex"].ToString()));
                            currentForm = qTTForms[int.Parse(dt.Rows[i]["QTTIndex"].ToString()) - 1];
                            break;
                        case 73:
                            LaunchWashSale(null);
                            currentForm = washSaleForm;
                            break;
                        case 74:
                            LaunchNAVLock();
                            currentForm = navLockForm;
                            break;
                        default:
                            break;
                    }

                    if (currentForm != null)
                    {
                        SetFormLayout(dt.Rows[i], windowState, currentForm);
                    }
                }
                _loadingLayout = false;
                _loadApplicationFirstTime = false;
                LayoutManager.SaveLastUsedTime(_layoutID);
            }
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
        /// Sets the layout allocation client form.
        /// </summary>
        private void SetLayoutAllocationClientForm()
        {
            try
            {
                // As we are doing custom handling for Allocation, so no need to get module Id on the basis of Module Name. Removed extra Db call. Need to make a generic method here.
                DataTable dt = LayoutManager.GetLayoutDetailsByModuleId(_layoutID, loginUser.CompanyUserID, 8);
                DataRow layoutRow = null;
                if (dt != null && dt.Rows.Count > 0)
                    layoutRow = dt.Rows[0];

                allocationClientForm.SizeToContent = System.Windows.SizeToContent.Manual;
                if (layoutRow != null)
                {
                    FormWindowState windowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), layoutRow["WindowState"].ToString());
                    switch (windowState)
                    {
                        case FormWindowState.Maximized:
                            allocationClientForm.WindowState = System.Windows.WindowState.Maximized;
                            break;
                        case FormWindowState.Minimized:
                            allocationClientForm.WindowState = System.Windows.WindowState.Minimized;
                            break;
                        case FormWindowState.Normal:
                            if (CheckVirtualScreenBounds(layoutRow))
                            {
                                allocationClientForm.Left = int.Parse(layoutRow["LeftX"].ToString());
                                allocationClientForm.Top = int.Parse(layoutRow["RightY"].ToString());
                                allocationClientForm.Height = int.Parse(layoutRow["Height"].ToString());
                                allocationClientForm.Width = int.Parse(layoutRow["Width"].ToString());
                            }
                            else
                                SetLocationOfTheForm(allocationClientForm);
                            break;
                        default:
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        private void SetLocationOfTheForm(Window wpfWindow)
        {
            try
            {
                double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

                wpfWindow.Left = (screenWidth / 2) - (int)Math.Truncate(wpfWindow.Width / 2.0);
                wpfWindow.Top = (screenHeight / 2) - (int)Math.Truncate(wpfWindow.Height / 2.0);
            }
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
        /// Sets the layout Percent Trading Tool.
        /// </summary>
        private void SetLayoutPercentTradingTool(DataRow layoutRow, FormWindowState windowState)
        {
            try
            {
                switch (windowState)
                {
                    case FormWindowState.Maximized:
                        percentTradingToolWindow.WindowState = System.Windows.WindowState.Maximized;
                        break;
                    case FormWindowState.Minimized:
                        percentTradingToolWindow.WindowState = System.Windows.WindowState.Minimized;
                        break;
                    case FormWindowState.Normal:
                        if (CheckVirtualScreenBounds(layoutRow))
                        {
                            percentTradingToolWindow.SizeToContent = System.Windows.SizeToContent.Manual;
                            percentTradingToolWindow.Left = int.Parse(layoutRow["LeftX"].ToString());
                            percentTradingToolWindow.Top = int.Parse(layoutRow["RightY"].ToString());
                            percentTradingToolWindow.Height = int.Parse(layoutRow["Height"].ToString());
                            percentTradingToolWindow.Width = int.Parse(layoutRow["Width"].ToString());
                        }
                        else
                        {
                            Size size = SystemInformation.PrimaryMonitorSize;
                            percentTradingToolWindow.Left = size.Width / 2 - (int)Math.Truncate(percentTradingToolWindow.Width / 2.0);
                            percentTradingToolWindow.Top = size.Height / 2 - (int)Math.Truncate(percentTradingToolWindow.Height / 2.0);
                        }
                        break;
                    default:
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

        private void SetFormLayout(DataRow layoutRow, FormWindowState windowState, Form currentForm)
        {
            try
            {
                if (UIValidation.GetInstance().validate(currentForm))
                {
                    if (currentForm.InvokeRequired)
                    {
                        SetLayoutHandler handler = new SetLayoutHandler(SetFormLayout);
                        currentForm.BeginInvoke(handler, layoutRow, windowState, currentForm);
                    }
                    else
                    {
                        switch (windowState)
                        {
                            case FormWindowState.Maximized:
                                currentForm.WindowState = FormWindowState.Maximized;
                                break;
                            case FormWindowState.Minimized:
                                currentForm.WindowState = FormWindowState.Minimized;
                                break;
                            case FormWindowState.Normal:
                                if (CheckVirtualScreenBounds(layoutRow))
                                {
                                    currentForm.Left = int.Parse(layoutRow["LeftX"].ToString());
                                    currentForm.Top = int.Parse(layoutRow["RightY"].ToString());
                                    currentForm.Height = int.Parse(layoutRow["Height"].ToString());
                                    currentForm.Width = int.Parse(layoutRow["Width"].ToString());
                                }
                                else
                                {
                                    Size size = SystemInformation.PrimaryMonitorSize;
                                    currentForm.Left = size.Width / 2 - (int)Math.Truncate(currentForm.Width / 2.0);
                                    currentForm.Top = size.Height / 2 - (int)Math.Truncate(currentForm.Height / 2.0);
                                }
                                break;
                            default:
                                break;
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

        private void LoadLayoutForBlotterOnBlotterThread()
        {
            try
            {
                if (UIValidation.GetInstance().validate(blotterForm))
                {
                    if (blotterForm.InvokeRequired)
                    {
                        MethodInvokerVoid mi = new MethodInvokerVoid(LoadLayoutForBlotterOnBlotterThread);
                        blotterForm.BeginInvoke(mi);
                    }
                    else
                    {
                        DataTable dt = LayoutManager.GetModuleDetailsForLayout(_layoutID, blotterForm.Name);
                        if (dt.Rows.Count == 1)
                        {
                            DataRow dr = dt.Rows[0];
                            FormWindowState windowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), dr["WindowState"].ToString());

                            SetFormLayout(dr, windowState, blotterForm);
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

        private DataTable GetLayoutTableSchema()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("LayoutModuleID");
                dt.Columns.Add("LeftX");
                dt.Columns.Add("RightY");
                dt.Columns.Add("Height");
                dt.Columns.Add("Width");
                dt.Columns.Add("WindowState");
                dt.Columns.Add("IsInUse");
                dt.Columns.Add("LayoutID");
                dt.Columns.Add("ModuleID");
                dt.Columns.Add("ModuleName");
                dt.Columns.Add("UserID");
                dt.Columns.Add("QTTIndex");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dt;
        }

        private bool CheckVirtualScreenBounds(DataRow dataRow)
        {
            bool result = false;
            try
            {
                int formWidth = int.Parse(dataRow["Width"].ToString());
                int formLeft = int.Parse(dataRow["LeftX"].ToString());
                int formTop = int.Parse(dataRow["RightY"].ToString());

                Rectangle virtScreen = SystemInformation.VirtualScreen;

                if ((formLeft + formWidth - 100) > virtScreen.Left && (formLeft + 100) < virtScreen.Right && formTop > virtScreen.Top && (formTop + 150) < virtScreen.Bottom)
                {
                    result = true;
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
        #endregion

        #region Instance Closed Events

        private void blotterInstance_BlotterClosed(object sender, EventArgs e)
        {
            if (blotterForm != null)
                blotterForm.Dispose();
            if (blotternewInstance != null)
            {
                blotternewInstance.TradeClick -= new EventHandler(blotternewInstance_TradeClick);
                blotternewInstance.HighlightSymbolFromBlotter -= _blotter_HighlightSymbolFromBlotter;
                blotternewInstance.ReplaceOrEditOrderClicked -= new EventHandler(blotternewInstance_ReplaceOrEditOrderClicked);
                blotternewInstance.BlotterClosed -= new EventHandler(blotterInstance_BlotterClosed);
                blotternewInstance.LaunchPreferences -= new EventHandler(blotterInstance_LaunchPreferences);
                blotternewInstance.GoToAllocationClicked -= HandleGoToAllocationClick;
            }
            blotternewInstance = null;
            blotterForm = null;
        }

        private void blotterForm_Disposed(object sender, EventArgs e)
        {
            blotterForm = null;
        }

        private void frmPranaHelp_Closing(object sender, CancelEventArgs e)
        {
            this.frmPranaHelp = null;
        }

        private void frmHelpAndSupport_Closing(object sender, CancelEventArgs e)
        {
            this.frmHelpAndSupport = null;
        }

        void portfolioManagementInstance_FormClosed(object sender, EventArgs e)
        {
            try
            {
                this.ApplyPreferences -= new EventHandler<EventArgs<string, IPreferenceData>>(((IPositionManagement)sender).ApplyPreferences);
                portfolioManagementInstance.FormClosedHandler -= new EventHandler(portfolioManagementInstance_FormClosed);
                portfolioManagementInstance.TradeClick -= new EventHandler<EventArgs<OrderSingle, Dictionary<int, double>>>(portfolioManagementInstance_ConsolidationViewTradeClickToMain);
                portfolioManagementInstance.PercentTradingToolClick -= portfolioManagementInstance_PercentTradingToolClick;
                portfolioManagementInstance.ClosePositionClick -= new EventHandler(portfolioManagementInstance_ClosePositionClick);
                portfolioManagementInstance.MarkPriceClick -= new EventHandler(portfolioManagementInstance_MarkPriceClick);
                portfolioManagementInstance.CorporateActionClick -= new EventHandler(portfolioManagementInstance_CorporateActionClick);

                //disconnect expnl on PM close else updation in cache continues which would affect performance
                DisconnectToExposurePNLServer(false);
                portfolioManagementInstance = null;

                _isPMAlreadyStarted = false;
                portfolioManagementForm.Dispose();
                portfolioManagementForm = null;
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

        #region Preferences Control related Methods

        private void mnuPNLPrefs_Click(object sender, System.EventArgs e)
        {
            try
            {
                LaunchPreferncesModule("", GetFormNewLocation(null));
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
        /// This method launches the preferences control
        /// </summary>
        /// <param name="prefsModuleName"></param>
        /// <param name="formLocation"></param>
        private void LaunchPreferncesModule(string prefsModuleName, Point formLocation)
        {
            try
            {
                if (preferencesMain == null)
                {
                    int read_write = 0;
                    if (ModuleManager.CompanyModulesPermittedToUser != null)
                    {
                        foreach (Prana.Admin.BLL.Module module in ModuleManager.CompanyModulesPermittedToUser)
                        {
                            if (module.ModuleName.Equals(prefsModuleName))
                            {
                                read_write = module.ReadWriteID;
                                break;
                            }
                        }
                    }

                    preferencesMain = PreferencesMain.GetInstance();
                    preferencesMain.LoginUser = loginUser;
                    preferencesMain.Read_Write = read_write;
                    preferencesMain.SecurityMaster = _secMasterServices;
                    preferencesMain.ApplyPrefsClick += new ApplyPreferenceHandler(preferencesMain_ApplyPrefsClick);
                    preferencesMain.PreferencesClosed += new EventHandler(preferencesMain_PreferencesClosed);
                    preferencesMain.PrefsModuleName = prefsModuleName;
                    preferencesMain.AvailableModulesDetails = ModuleManager.AvailableModulesDetails;
                    preferencesMain.LoadControl();
                    preferencesMain.Owner = this;
                    preferencesMain.ShowInTaskbar = false;
                    preferencesMain.Show();
                    SetFromLayoutDetail(preferencesMain);
                }
                BringFormToFront(preferencesMain, formLocation);
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

        private void preferencesMain_PreferencesClosed(object sender, EventArgs e)
        {
            try
            {
                //modified by omshiv, Jan 22, 3014, Unwire events on preference UI closed
                PreferencesMain preferencesMain = sender as PreferencesMain;
                if (preferencesMain != null)
                {
                    preferencesMain.ApplyPrefsClick -= new ApplyPreferenceHandler(preferencesMain_ApplyPrefsClick);
                    preferencesMain.PreferencesClosed -= new EventHandler(preferencesMain_PreferencesClosed);
                }
                this.preferencesMain = null;
                if (OpenModulesList.Contains(PranaModules.CLOSE_POSITIONS_MODULE))
                {
                    OpenModulesList.Remove(PranaModules.CLOSE_POSITIONS_MODULE);
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
            }
        }

        private void LaunchQualityCheckerModule()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.QUALITY_CHECKER, PranaModules.QUALITY_CHECKER))
                {
                    if (QualityCheckForm == null)
                    {
                        QualityCheckForm = new QualityCheck
                        {
                            Owner = this,
                            ShowInTaskbar = false
                        };
                        QualityCheckForm.FormClosed += new FormClosedEventHandler(QualityCheckerClosed);
                        QualityCheckForm.SetUpDataBaseRelatedField();
                        QualityCheckForm.Show();
                    }
                    BringFormToFront(QualityCheckForm, GetFormNewLocation(null));
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

        private void QualityCheckerClosed(object sender, EventArgs e)
        {
            if (QualityCheckForm != null)
                QualityCheckForm.Dispose();
            QualityCheckForm = null;
        }

        private void blotterInstance_LaunchPreferences(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                Point formLocation = GetFormNewLocation(parentForm);
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        LaunchPreferencesHandler handler = new LaunchPreferencesHandler(LaunchPreferncesModule);
                        this.BeginInvoke(handler, PranaModules.BLOTTER_MODULE, formLocation);
                    }
                    else
                    {
                        LaunchPreferncesModule(PranaModules.BLOTTER_MODULE, formLocation);
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

        private void allocationInstance_LaunchPreferences(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                LaunchPreferncesModule(PranaModules.ALLOCATION_MODULE, GetFormNewLocation(parentForm));
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

        private void preferencesMain_ApplyPrefsClick(object sender, EventArgs<string, IPreferenceData> e)
        {
            try
            {
                if (e.Value.Equals("General"))
                {
                    ShowHideServiceIcons(e.Value2);
                }
                else if (this.ApplyPreferences != null)
                {
                    ApplyPreferences(this, new EventArgs<string, IPreferenceData>(e.Value, e.Value2));
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
        /// Shows the hide service icons.
        /// </summary>
        /// <param name="preferenceData">The preference data.</param>
        private void ShowHideServiceIcons(IPreferenceData preferenceData)
        {
            try
            {
                if (preferenceData is GeneralPreferenceData)
                {
                    bool isShowServiceIcons = (preferenceData as GeneralPreferenceData).IsShowServiceIcons;
                    lblConnectionStatus.Visible = isShowServiceIcons;
                    lblExPNLConnection.Visible = isShowServiceIcons;
                    lblPricingServer.Visible = isShowServiceIcons;
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

        private void cashManagementInstance_LaunchPreferences(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                LaunchPreferncesModule(PranaModules.GENERAL_LEDGER_MODULE, GetFormNewLocation(parentForm));
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

        private void positionManagementInstance_LaunchPreferences(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                LaunchPreferncesModule(PranaModules.POSITION_MANAGEMENT_PREFS_MODULE, GetFormNewLocation(parentForm));
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

        #region Toolbar click handler

        private void PranaMainToolBar_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "Allocation":
                        this.LaunchAllocationForm();
                        break;

                    case "Rebalancer":
                        this.LaunchRebalancer();
                        break;

                    case "Preferences":
                        this.LaunchPreferncesModule("", GetFormNewLocation(null));
                        break;

                    case "Blotter":
                        this.LaunchBlotterFormNew();
                        break;

                    case "PortfolioManagement":
                        this.LaunchPortfolioManagementForm();
                        break;

                    case "GeneralLedger":
                        this.LaunchCashManagement();
                        break;

                    case "Trading Ticket":
                    case "TradingTicketButton":
                    case "SecondTradingTicket":
                        this.LaunchTradingTicketDock(TradingTicketParent.None);
                        break;

                    case "% Trading Tool":
                        this.LaunchPercentTradingTool(true, string.Empty);
                        break;

                    case ApplicationConstants.CONST_COMPLIANCE_MODULE:
                        LaunchPluggableTool(ApplicationConstants.CONST_COMPLIANCE, GetFormNewLocation(null));
                        break;

                    case ApplicationConstants.CONST_COMPLIANCE_RULE_DEFINITION:
                    case ApplicationConstants.CONST_COMPLIANCE_ALERT_HISTORY:
                    case ApplicationConstants.CONST_COMPLIANCE_PENDING_APPROVAL:
                        ComplianceCacheManager.SetComplianceUITabSelected(e.Tool.Key);
                        LaunchPluggableTool(e.Tool.Key, GetFormNewLocation(null));
                        break;
                    case "Watchlist":
                    case "WatchlistButton":
                        this.LaunchWatchList();
                        break;
                    case "OptionChain":
                        LaunchOptionChain();
                        break;
                    case "ThirdPartyManager":
                        LaunchThirdPartyManager();
                        break;
                    case "ShortLocate":
                        LaunchShortLocate();
                        break;
                    case "BlotterExecutionReport":
                        LaunchBlotterReportsForm();
                        break;
                    case "AuditTrail":
                        LaunchAuditUI(new KeyValuePair<string, object[]>("none", null), GetFormNewLocation(this));
                        break;
                    case "BrokerConnections":
                        LaunchCounterParty();
                        break;
                    case "ReloadSettings":
                        ReloadSettings();
                        break;
                    case "DailyValuation":
                        LaunchMarkPriceUI(null);
                        break;
                    case "Closing":
                        LaunchClosingUI(this, null);
                        break;
                    case "DataMapping":
                        LaunchMappingUI(new EventArgs(), null);
                        break;
                    case "QualityChecker":
                        LaunchQualityCheckerModule();
                        break;
                    case "CreateTransaction":
                        LaunchCreateTransaction(null);
                        break;
                    case "CorporateActions":
                        LaunchCorporateActionsUI(null);
                        break;
                    case "WashSale":
                        LaunchWashSale(null);
                        break;
                    case "NAVLock":
                        LaunchNAVLock();
                        break;
                    case "ModuleShortcuts":
                        LaunchShortcutsForm();
                        break;
                    case "AboutNirvana":
                        LaunchHelpForm();
                        break;
                    case "HelpAndSupport":
                        LaunchHelpAndSupportForm();
                        break;
                    case "Disclaimer":
                        LaunchDisclaimer();
                        break;
                    case "Recon":
                        LaunchPluggableTool("Reconciliation", GetFormNewLocation(null));
                        break;
                    case "Import":
                        LaunchPluggableTool("Import Data", GetFormNewLocation(null));
                        break;
                    case "AutoImport":
                        LaunchPluggableTool("Auto Import Data", GetFormNewLocation(null));
                        break;

                    case "MiddlewareManager":
                    case "ZeroPositionAlert":
                    case "MissingTrades":
                    case "PricingInputs":
                    case "RiskAnalysis":
                    case "SecurityMaster":
                        LaunchPluggableTool(e.Tool.CaptionResolved, GetFormNewLocation(null));
                        break;
                    default:
                        if (e.Tool.Key.StartsWith(ApplicationConstants.CONST_QUICK_TRADING_TICKET))
                            LaunchQuickTradingTicket(Convert.ToInt32(e.Tool.Key.Substring(18)));
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
        #endregion

        delegate void SetTextCallback3(object sender, LoggingEventArgs<string> e);
        void OnInformationReceived(object sender, LoggingEventArgs<string> e)
        {
            try
            {
                string message = e.Value;
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        SetTextCallback3 mi = new SetTextCallback3(OnInformationReceived);
                        this.BeginInvoke(mi, new object[] { this, e });
                    }
                    else
                    {
                        if (portfolioManagementInstance != null)
                        {
                            portfolioManagementInstance.StatusMessage = message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void LaunchHelpForm()
        {
            try
            {
                if (frmPranaHelp == null)
                {
                    frmPranaHelp = new Prana.AboutPrana(loginUser.FirstName + " " + loginUser.LastName, loginUser.CompanyUserID);
                    frmPranaHelp.Owner = this;
                    frmPranaHelp.ShowInTaskbar = false;
                    frmPranaHelp.Closing += new CancelEventHandler(frmPranaHelp_Closing);
                    SetFromLayoutDetail(frmPranaHelp);
                }
                frmPranaHelp.Show();
                BringFormToFront(frmPranaHelp, GetFormNewLocation(null));
            }
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
        /// Launches the help and support form.
        /// </summary>
        private void LaunchHelpAndSupportForm()
        {
            try
            {
                if (frmHelpAndSupport == null)
                {
                    frmHelpAndSupport = new Prana.HelpAndSupport();
                    frmHelpAndSupport.Owner = this;
                    frmHelpAndSupport.ShowInTaskbar = false;
                    frmHelpAndSupport.Closing += new CancelEventHandler(frmHelpAndSupport_Closing);
                    SetFromLayoutDetail(frmHelpAndSupport);
                }
                frmHelpAndSupport.Show();
                BringFormToFront(frmHelpAndSupport, GetFormNewLocation(null));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        UserProfile _userProfileWindow;
        public void LaunchUserProfile()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.USER_PROFILE_MODULE, PranaModules.USER_PROFILE_MODULE))
                {
                    if (!OpenModulesList.Contains(PranaModules.USER_PROFILE_MODULE))
                        OpenModulesList.Add(PranaModules.USER_PROFILE_MODULE);
                    if (_userProfileWindow == null)
                    {
                        var userProfileViewModel = new UserProfileViewModel { UserID = loginUser.CompanyUserID };
                        _userProfileWindow = new UserProfile();
                        _userProfileWindow.userProfileViewModel = userProfileViewModel;
                        _userProfileWindow.Closed += UserProfileWindow_Closed;
                        BringFormToFront(_userProfileWindow);
                    }
                    else
                    {
                        if (_userProfileWindow.WindowState == System.Windows.WindowState.Minimized)
                            _userProfileWindow.WindowState = System.Windows.WindowState.Normal;
                        _userProfileWindow.Activate();
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

        private void PranaMain_Load(object sender, EventArgs e)
        {
            if (Boolean.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsGlobalShortcutsAvailable")))
            {
                Prana.ShortcutMonitor.NativeMethods.KeyDown += NativeMethods_KeyDown;
            }
        }

        private void LaunchDailySheetReports()
        {
            try
            {
                if (ModuleManager.AvailableModulesDetails.ContainsKey("PortfolioReportsDailySheet"))
                {
                    if (positionManagementDailySheetForm == null)
                    {
                        DynamicClass formToLoad;
                        formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails["PortfolioReportsDailySheet"];

                        Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);

                        Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);
                        IPositionManagementReports pmDailySheetReportsInstance = null;
                        pmDailySheetReportsInstance = (IPositionManagementReports)Activator.CreateInstance(typeToLoad);
                        pmDailySheetReportsInstance.FormClosedHandler += new EventHandler(pmDailySheetReportsInstance_FormClosed);
                        pmDailySheetReportsInstance.LoginUser = loginUser;

                        positionManagementDailySheetForm = pmDailySheetReportsInstance.Reference();
                        positionManagementDailySheetForm.Owner = this;
                        positionManagementDailySheetForm.ShowInTaskbar = false;
                        positionManagementDailySheetForm.Show();
                        SetFromLayoutDetail(positionManagementDailySheetForm);
                    }
                    BringFormToFront(positionManagementDailySheetForm, GetFormNewLocation(null));
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

        void pmDailySheetReportsInstance_FormClosed(object sender, EventArgs e)
        {
            positionManagementDailySheetForm = null;
        }

        private void LaunchMonthlySheets()
        {
            try
            {
                if (ModuleManager.AvailableModulesDetails.ContainsKey("PortfolioReportsMonthlySheet"))
                {
                    if (positionManagementMonthlySheetForm == null)
                    {
                        DynamicClass formToLoad;
                        formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails["PortfolioReportsMonthlySheet"];

                        Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);

                        Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);
                        IPositionManagementReports pmMonthlySheetReportsInstance = null;
                        pmMonthlySheetReportsInstance = (IPositionManagementReports)Activator.CreateInstance(typeToLoad);
                        pmMonthlySheetReportsInstance.FormClosedHandler += new EventHandler(pmMonthlySheetReportsInstance_FormClosed);
                        pmMonthlySheetReportsInstance.LoginUser = loginUser;

                        positionManagementMonthlySheetForm = pmMonthlySheetReportsInstance.Reference();
                        positionManagementMonthlySheetForm.Owner = this;
                        positionManagementMonthlySheetForm.ShowInTaskbar = false;
                        positionManagementMonthlySheetForm.Show();
                        SetFromLayoutDetail(positionManagementMonthlySheetForm);
                    }
                    BringFormToFront(positionManagementMonthlySheetForm, GetFormNewLocation(null));
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

        void pmMonthlySheetReportsInstance_FormClosed(object sender, EventArgs e)
        {
            positionManagementMonthlySheetForm = null;
        }

        //BB
        public void FileLayoutSave_Click()
        {
            try
            {
                if (_layoutID > 0)
                {
                    SaveLayout();
                }
                else
                {
                    string strLayoutName = string.Empty;
                    strLayoutName = InputBox.ShowInputBox("Layout Name").Trim();
                    if (strLayoutName.Equals(string.Empty))
                        return;

                    _layoutID = LayoutManager.SaveLayoutName(strLayoutName, loginUser.CompanyUserID);
                    _layoutName = strLayoutName;

                    int result = SaveLayout();
                    if (result > 0)
                    {
                        LoadLayoutForUser();
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
            }
        }

        private void PranaMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Application_ApplicationExit(null, null);
                if (_formClosingDialogResult.Equals(DialogResult.Cancel))
                {
                    e.Cancel = true;
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

        private DialogResult AskSaveConfirmation()
        {
            DialogResult dlgResult = DialogResult.Yes;
            try
            {
                if (_layoutID > 0)
                {
                    dlgResult = ConfirmationMessageBox.Display("Do you want to save current layout - " + _layoutName + " ?", "Save Layout");
                    if (DialogResult.Yes == dlgResult)
                    {
                        SaveLayout();
                    }
                }
                else
                {
                    if (this.OwnedForms.Length > 0)
                    {
                        dlgResult = ConfirmationMessageBox.Display("Do you want to save this unsaved layout?", "Save Layout");
                        if (DialogResult.Yes == dlgResult)
                        {
                            string strLayoutName = string.Empty;
                            strLayoutName = InputBox.ShowInputBox("Layout Name").Trim();
                            dlgResult = DialogResult.Cancel;
                            if (!strLayoutName.Equals(string.Empty))
                            {
                                _layoutID = LayoutManager.SaveLayoutName(strLayoutName, loginUser.CompanyUserID);
                                _layoutName = strLayoutName;
                                SaveLayout();
                                dlgResult = DialogResult.Yes;
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
            return dlgResult;
        }

        private void PranaMain_Resize(object sender, EventArgs e)
        {
            try
            {
                foreach (KeyValuePair<string, Form> pluginForm in activePlugInForms)
                {
                    pluginForm.Value.WindowState = this.WindowState;
                }
                //special handling for blotter as blotter is running on a separate thread other than main UI thread...
                ResizeBlotterForm(this.WindowState);

                //Handle Allocation form window state according to NirvanaMain form
                ResizeAllocationClient(this.WindowState);
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
        /// Handle Allocation Client form state according to Niravana Main For window state
        /// </summary>
        /// <param name="formWindowState"></param>
        private void ResizeAllocationClient(FormWindowState formWindowState)
        {
            try
            {
                if (allocationClientForm != null)
                {
                    if (formWindowState.Equals(FormWindowState.Minimized))
                        allocationClientForm.WindowState = System.Windows.WindowState.Minimized;
                    else if (formWindowState.Equals(FormWindowState.Normal))
                        allocationClientForm.WindowState = System.Windows.WindowState.Normal;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void ResizeBlotterForm(FormWindowState windowState)
        {
            try
            {
                //Run only when blotter form is open i.e. it shouldn't be null.
                if (null != blotterForm)
                {
                    if (UIValidation.GetInstance().validate(blotterForm))
                    {
                        if (blotterForm.InvokeRequired)
                        {
                            ResizeFormHandler handler = new ResizeFormHandler(ResizeBlotterForm);
                            blotterForm.BeginInvoke(handler, windowState);
                        }
                        else
                        {
                            if (windowState.Equals(FormWindowState.Minimized))
                            {
                                blotterForm.WindowState = FormWindowState.Minimized;
                            }
                            if (windowState.Equals(FormWindowState.Normal))
                            {
                                blotterForm.WindowState = FormWindowState.Normal;
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

        private void DisplayMessageInBallon(string message)
        {
            notifyForm.GetInstance().DisplayMessage(message);
        }

        ICashAccounts cashAccountsInsatance = null;
        Form cashAccountForm = null;
        private void LaunchCashAccountsUI()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.GENERAL_LEDGER_ACCOUNT_SETUP_MODULE, PranaModules.GENERAL_LEDGER_ACCOUNT_SETUP_MODULE))
                {
                    if (cashAccountsInsatance == null)
                    {
                        DynamicClass formToLoad;

                        formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails[PranaModules.GENERAL_LEDGER_ACCOUNT_SETUP_MODULE];

                        Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);
                        Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);

                        cashAccountsInsatance = (ICashAccounts)Activator.CreateInstance(typeToLoad);
                        cashAccountsInsatance.FormClosedHandler += new EventHandler(cashAccountsInsatance_FormClosedHandler);
                        cashAccountForm = cashAccountsInsatance.Reference();
                        cashAccountForm.Owner = this;
                        cashAccountForm.ShowInTaskbar = false;
                        cashAccountForm.Show();
                        SetFromLayoutDetail(cashAccountForm);
                    }
                    BringFormToFront(cashAccountForm, GetFormNewLocation(null));
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

        void cashAccountsInsatance_FormClosedHandler(object sender, EventArgs e)
        {
            if (cashAccountsInsatance != null)
            {
                cashAccountsInsatance = null;
            }
            if (cashAccountForm != null)
            {
                cashAccountForm = null;
            }
        }

        public delegate void UpdatePreferencesHandler(string moduleName);
        public delegate void ApplyMainPreferencesHandler(object sender, EventArgs<string, IPreferenceData> e);


        IMappingFile mappingFileInstance = null;
        Form mappingForm = null;
        private void LaunchMappingUI(EventArgs e, Form parentForm)
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.MAPINGFORM_MODULE, PranaModules.MAPINGFORM_MODULE))
                {
                    if (!OpenModulesList.Contains(PranaModules.MAPINGFORM_MODULE))
                        OpenModulesList.Add(PranaModules.MAPINGFORM_MODULE);
                    if (mappingFileInstance == null)
                    {
                        DynamicClass formToLoad;

                        formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails[PranaModules.MAPINGFORM_MODULE];

                        Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);
                        Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);
                        mappingFileInstance = (IMappingFile)Activator.CreateInstance(typeToLoad);
                        mappingFileInstance.MappingClosed += new EventHandler(mappingFileInstance_MappingClosed);
                        mappingFileInstance.SecurityMaster = _secMasterServices;
                        if (e is Prana.BusinessObjects.ListEventAargs)
                        {
                            mappingFileInstance.activityType = ((Prana.BusinessObjects.ListEventAargs)(e)).listOfValues;
                        }
                        mappingForm = mappingFileInstance.Reference();
                        mappingForm.Owner = this;
                        mappingForm.ShowInTaskbar = false;
                        mappingForm.Show();
                        SetFromLayoutDetail(mappingForm);
                    }
                    BringFormToFront(mappingForm, GetFormNewLocation(parentForm));
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

        void mappingFileInstance_MappingClosed(object sender, EventArgs e)
        {
            if (mappingForm != null)
            {
                mappingForm = null;
            }
            if (mappingFileInstance != null)
            {
                mappingFileInstance = null;
            }
            if (OpenModulesList.Contains(PranaModules.MAPINGFORM_MODULE))
                OpenModulesList.Remove(PranaModules.MAPINGFORM_MODULE);
        }

        private void menuAccruals_Click(object sender, EventArgs e)
        {
            try
            {
                LaunchAccruals();
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

        IAccruals accrualsInstance = null;
        Form accruals = null;
        private void LaunchAccruals()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.ACCRUALS_MODULE, PranaModules.ACCRUALS_MODULE))
                {
                    if (accrualsInstance == null)
                    {
                        DynamicClass formToLoad;
                        formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails[PranaModules.ACCRUALS_MODULE];
                        Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);
                        Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);
                        accrualsInstance = (IAccruals)Activator.CreateInstance(typeToLoad);
                        accrualsInstance.FormClosedHandler += new EventHandler(accrualsInstance_FormClosedHandler);
                        accruals = accrualsInstance.Reference();
                        accruals.Owner = this;
                        accruals.ShowInTaskbar = false;
                        accruals.Show();
                        SetFromLayoutDetail(accruals);
                    }
                    BringFormToFront(accruals, GetFormNewLocation(null));
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

        void accrualsInstance_FormClosedHandler(object sender, EventArgs e)
        {
            if (accrualsInstance != null)
            {
                accrualsInstance = null;
            }
            if (accruals != null)
            {
                accruals = null;
            }
        }

        #region Cash Management Section

        ICashManagement cashManagementInstance = null;
        Form cashManagement = null;
        private void LaunchCashManagement()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.GENERAL_LEDGER_MODULE, PranaModules.GENERAL_LEDGER_MODULE))
                {
                    if (!OpenModulesList.Contains(PranaModules.GENERAL_LEDGER_MODULE))
                        OpenModulesList.Add(PranaModules.GENERAL_LEDGER_MODULE);
                    if (cashManagementInstance == null || cashManagement == null)
                    {
                        //Added By Nishant Jain
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6425 
                        CachedDataManager.GetInstance.RefreshAccountData();

                        cashManagementInstance = _container.Resolve<ICashManagement>();
                        cashManagementInstance.SecurityMaster = _secMasterServices;
                        cashManagementInstance.loginUser = loginUser;
                        cashManagementInstance.SetUp();

                        cashManagementInstance.FormClosedHandler += new EventHandler(cashManagementInstance_FormClosedHandler);
                        cashManagementInstance.LaunchAccountSetUpUI += new EventHandler(cashManagementInstance_LaunchAccountSetUpUI);
                        cashManagement = cashManagementInstance.Reference();
                        cashManagement.Owner = this;
                        cashManagement.ShowInTaskbar = false;
                        cashManagement.Show();
                        SetFromLayoutDetail(cashManagement);
                    }
                    BringFormToFront(cashManagement, GetFormNewLocation(null));
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

        void cashManagementInstance_LaunchAccountSetUpUI(object sender, EventArgs e)
        {
            try
            {
                LaunchCashAccountsUI();
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

        void cashManagementInstance_FormClosedHandler(object sender, EventArgs e)
        {
            if (OpenModulesList.Contains(PranaModules.GENERAL_LEDGER_MODULE))
                OpenModulesList.Remove(PranaModules.GENERAL_LEDGER_MODULE);
            if (_container != null)
            {
                _container.Release(cashManagementInstance);
                _container.Release(cashManagement);
            }
            if (cashManagementInstance != null)
                cashManagementInstance = null;
            if (cashManagement != null)
                cashManagement = null;
        }

        private void mnuCashManagement_Click(object sender, EventArgs e)
        {
            try
            {
                LaunchCashManagement();
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

        void allocationInstance_GenrateCashTransaction(object sender, EventArgs e)
        {
            try
            {
                if (cashManagementInstance == null)
                    LaunchCashManagement();
                CashDataEventArgs selectedTaxlot = e as CashDataEventArgs;
                cashManagementInstance.AddCashTransaction(selectedTaxlot.SelectedTaxlot);
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

        #region Position Management Section
        ISnapShotPositionManagement PositionManagementInstance = null;
        Form PositionManagement = null;
        private void LaunchPositionManagement()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.POSITION_MANAGEMENT_MODULE, PranaModules.POSITION_MANAGEMENT_MODULE))
                {
                    if (PositionManagementInstance == null)
                    {
                        if (MarketDataValidation.CheckMarketDataPermissioning())
                        {
                            PositionManagementInstance = _container.Resolve<ISnapShotPositionManagement>();
                            PositionManagementInstance.FormClosedHandler += new EventHandler(positionManagementInstance_FormClosedHandler);
                            PositionManagementInstance.LaunchPreferences += new EventHandler(positionManagementInstance_LaunchPreferences);
                            PositionManagement = PositionManagementInstance.Reference();
                            PositionManagement.Owner = this;
                            PositionManagement.ShowInTaskbar = false;
                            PositionManagement.Show();
                            SetFromLayoutDetail(PositionManagement);
                        }
                    }
                    BringFormToFront(PositionManagement, GetFormNewLocation(null));
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

        void positionManagementInstance_FormClosedHandler(object sender, EventArgs e)
        {
            if (PositionManagementInstance != null)
            {
                PositionManagementInstance = null;
            }
            if (PositionManagement != null)
            {
                PositionManagement = null;
            }
        }

        private void menuItem_PositionManagement_Click(object sender, EventArgs e)
        {
            try
            {
                LaunchPositionManagement();
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

        #region Posiion Management Main

        bool _isPMPermitted = false;

        IPositionManagement portfolioManagementInstance = null;
        void LaunchPortfolioManagementForm()
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.PORTFOLIO_MANAGEMENT_MODULE, PranaModules.PORTFOLIO_MANAGEMENT_MODULE))
                {
                    if (_isPMAlreadyStarted == false)
                    {
                        if (portfolioManagementInstance == null)
                        {
                            DynamicClass formToLoad;

                            formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails[PranaModules.PORTFOLIO_MANAGEMENT_MODULE];

                            Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);
                            Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);

                            formToLoad = null;
                            asmAssemblyContainingForm = null;

                            portfolioManagementInstance = (IPositionManagement)Activator.CreateInstance(typeToLoad);
                            typeToLoad = null;
                            portfolioManagementInstance.LoginUser = loginUser;

                            //TODO: Need to improve.. Disconnect ExPNL before connect otherwise PM dashboard shows blank (if TT is already open)
                            DisconnectToExposurePNLServer(true);

                            if (_exPNLCommManagerInstance == null)
                                _exPNLCommManagerInstance = new ClientTradeCommManager();

                            portfolioManagementInstance.ExPNLCommMgrInstance = _exPNLCommManagerInstance;
                            portfolioManagementForm = portfolioManagementInstance.Reference();
                            portfolioManagementForm.Owner = this;
                            portfolioManagementForm.ShowInTaskbar = false;
                            portfolioManagementInstance.InitializePM();

                            //http://jira.nirvanasolutions.com:8080/browse/QUAD-35
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-2229
                            //ConnectToExposurePNLServer() is called after InitializePM method
                            //because data is sent from EXPNL before initialization of PM
                            //SetExPnlServiceConnectionStatus();
                            //Application.DoEvents();
                            portfolioManagementForm.Show();
                            portfolioManagementInstance.FormClosedHandler += new EventHandler(portfolioManagementInstance_FormClosed);
                            portfolioManagementInstance.TradeClick += new EventHandler<EventArgs<OrderSingle, Dictionary<int, double>>>(portfolioManagementInstance_ConsolidationViewTradeClickToMain);
                            portfolioManagementInstance.PricingInputClick += new EventHandler(positionManagementInstance_PricingInputClick);


                            portfolioManagementInstance.ClosePositionClick += new EventHandler(portfolioManagementInstance_ClosePositionClick);
                            portfolioManagementInstance.MarkPriceClick += new EventHandler(portfolioManagementInstance_MarkPriceClick);

                            portfolioManagementInstance.CorporateActionClick += new EventHandler(portfolioManagementInstance_CorporateActionClick);
                            portfolioManagementInstance.PercentTradingToolClick += portfolioManagementInstance_PercentTradingToolClick;
                            this.ApplyPreferences += new EventHandler<EventArgs<string, IPreferenceData>>(portfolioManagementInstance.ApplyPreferences);
                            portfolioManagementInstance.RequestAccountData();

                            ConnectToExposurePNLServer(true);
                            SetFromLayoutDetail(portfolioManagementForm);
                        }

                        //Bharat Kumar Jangir (EXPNL gives Key not found error while connection/disconnection)
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-9325
                        //No need of this Else part
                        //else
                        //{
                        //    ConnectToExposurePNLServer();
                        //    SetExPnlServiceConnectionStatus();
                        //}
                        _isPMAlreadyStarted = true;
                    }
                    BringFormToFront(portfolioManagementForm, GetFormNewLocation(null));
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

        // open position sizing Tool From PM
        void portfolioManagementInstance_PercentTradingToolClick(object sender, EventArgs<string, PTTMasterFundOrAccount, List<int>, string> e)
        {
            try
            {
                LaunchPercentTradingTool(false, e.Value, e.Value2, e.Value3, e.Value4, true);
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

        void positionManagementInstance_PricingInputClick(object sender, EventArgs e)
        {
            try
            {
                LaunchFormEventArgs ea = (LaunchFormEventArgs)e;
                string symbolForwarded = ea.Params.ToString();
                _symbolToSearch = symbolForwarded;
                Form parentForm = sender as Form;
                if (!String.IsNullOrEmpty(symbolForwarded))
                {
                    LaunchPluggableTool(ApplicationConstants.CONST_PRICING_INPUT, GetFormNewLocation(parentForm));
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

        void portfolioManagementInstance_CorporateActionClick(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                LaunchCorporateActionsUI(parentForm);
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

        private void ConnectToExposurePNLServer(bool isPMConnect)
        {
            try
            {
                _exPNLConnectedModuleInstances++;
                if (_exPNLCommManagerInstance == null)
                    _exPNLCommManagerInstance = new ClientTradeCommManager();

                if ((isPMConnect || _exPNLConnectedModuleInstances == 1) && _exPNLCommManagerInstance.ConnectionStatus != PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    _exPNLCommManagerInstance.Connected += new EventHandler(_exposurePNLCommunicationManager_Connected);
                    _exPNLCommManagerInstance.Disconnected += new EventHandler(_exposurePNLCommunicationManager_Disconnected);
                    _exPNLCommManagerInstance.Connect(getExpnlConnectionProperties());
                    SetExPnlServiceConnectionStatus();
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

        private void DisconnectToExposurePNLServer(bool isPMDisconnect)
        {
            try
            {
                if (!isPMDisconnect)
                {
                    _exPNLConnectedModuleInstances--;
                }
                if ((isPMDisconnect || _exPNLConnectedModuleInstances == 0) && _exPNLCommManagerInstance != null && _exPNLCommManagerInstance.ConnectionStatus != PranaInternalConstants.ConnectionStatus.DISCONNECTED)
                {
                    _exPNLCommManagerInstance.ShouldRetry = false;
                    _exPNLCommManagerInstance.DisConnect();
                    _exPNLCommManagerInstance = null;
                    SetExPnlServiceConnectionStatus();
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

        void portfolioManagementInstance_MarkPriceClick(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                LaunchMarkPriceUI(parentForm);
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

        void portfolioManagementInstance_ClosePositionClick(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                LaunchClosingUI(parentForm, null);
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

        void portfolioManagementInstance_CreateTransactionClick(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                LaunchCreateTransaction(parentForm);
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

        private void menuMapping_Click(object sender, EventArgs e)
        {
            try
            {
                LaunchMappingUI(e, null);
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

        IWindsorContainer _container;
        internal void SetContainer(IWindsorContainer container)
        {
            _container = container;
        }

        private void pbPricingServerConnectionStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (_pricingServerConnectionStatus == false)
                {
                    _pricingAnalysisInstance.ConnectToServer(GetPricingConnectionProperties());
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

        private void DisplayPricingServerStatus(bool isConnected)
        {
            try
            {
                if (isConnected)
                {
                    lblPricingServer.Appearance.ImageBackground = global::Prana.Properties.Resources.NirvanaMainIndicatorGreen;
                }
                else
                {
                    lblPricingServer.Appearance.ImageBackground = global::Prana.Properties.Resources.NirvanaMainIndicatorRed;
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

        private void counterPartyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LaunchCounterParty();
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

        private void LaunchCounterParty()
        {
            try
            {
                formBrokerConnection.Owner = this;
                formBrokerConnection.ShowInTaskbar = false;
                formBrokerConnection.Show();
                SetFromLayoutDetail(formBrokerConnection);
                BringFormToFront(formBrokerConnection, GetFormNewLocation(null));
                QueueMessage queueMessage = new QueueMessage();
                queueMessage.Message = "CounterPartyDetails";
                if (null != _tradeCommManager)
                {
                    _tradeCommManager.SendMessage(queueMessage);
                }
                formBrokerConnection.usrCtrlBrokerConnectionStatusDetails.BindCounterPartyStatusUpdate();
            }
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
        /// Refresh Cache on 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReloadSettings()
        {
            try
            {
                if (activePluggableTools.Count == 0 && OpenModulesList.Count == 0)
                {
                    Prana.ClientCommon.NAVLockManager.GetInstance.RefreshAccountNavLockDetails();
                    CommonDataCache.CachedDataManager.GetInstance.RefreshPranaPreferences();
                    ClientPricingManager.GetInstance.RefreshPricingRulesCache();
                    CachedDataManagerRecon.RefreshCache(loginUser.CompanyUserID);
                    NirvanaAuthorizationManager.GetInstance().RefreshUserPermissions();
                    //Added By Faisal Shah 06/25/2014
                    //Need was to Update Cache once new accounts are added to DB.
                    //  CachedData.GetInstance().RefreshUserPermittedAccountsForCH(loginUser.CompanyUserID);
                    CachedDataManager.GetInstance.StartCaching(loginUser);
                    Prana.ClientCommon.TradingTktPrefs.SetClientCache(loginUser);
                    //added by: Bharat raturi, 27 may 2014
                    //purpose: to refresh the more frequently used details from the database
                    CachedDataManager.GetInstance.RefreshFrequentlyUsedData();
                    //added by: Bharat Raturi, 02 jun-2014
                    //purpose: refresh the server cache
                    if (_secMasterServices != null && _secMasterServices.IsConnected)
                    {
                        _secMasterServices.RefreshServerCache();
                    }
                    //refresh closing preferences so that closing algorithms and secondary sort can be updated in server closing cache
                    Prana.ClientCommon.ClosingClientSideMapper.RefreshClosingPreferences();
                    MessageBox.Show("Settings reloaded.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please close all open windows before reloading settings.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        #region PricingInputModule
        void PI_LaunchSymbolLookup(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                _arguments = e;
                LaunchPluggableTool(ApplicationConstants.CONST_SYMBOL_LOOKUP, GetFormNewLocation(parentForm));
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

        List<String> _listPI = null;
        void SymbolLookup_LaunchPricingInput(object sender, EventArgs e)
        {
            try
            {
                Form parentForm = sender as Form;
                LaunchFormEventArgs eventArg = e as LaunchFormEventArgs;
                _listPI = eventArg.Params as List<String>;
                LaunchPluggableTool(ApplicationConstants.CONST_PRICING_INPUT, GetFormNewLocation(parentForm));
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

        #region IJob Members

        public void Execute(JobExecutionContext context)
        {
            try
            {
                StringBuilder message = ImportManager.Instance.Execute(context);
                if (!string.IsNullOrWhiteSpace(message.ToString()))
                {
                    MessageBox.Show(message.ToString(), "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void mnuPercentTradingTool_Click(object sender, EventArgs e)
        {
            try
            {
                LaunchPercentTradingTool(true);
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

        // Launch the Position Sizing Tool
        PercentTradingToolWindow percentTradingToolWindow = null;
        private void LaunchPercentTradingTool(bool isReloadPTTDataAllowed, string symbol = "", PTTMasterFundOrAccount mfOrAccount = PTTMasterFundOrAccount.Account, List<int> accountList = null, string type = "", bool isOpenFromPM = false)
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.PERCENT_TRADING_TOOL, PranaModules.PERCENT_TRADING_TOOL))
                {
                    PercentTradingToolViewModel percentTradingToolViewModel = null;
                    if (!OpenModulesList.Contains(PranaModules.PERCENT_TRADING_TOOL))
                    {
                        if (MarketDataValidation.CheckMarketDataPermissioning())
                        {
                            OpenModulesList.Add(PranaModules.PERCENT_TRADING_TOOL);
                            if (_exPNLCommManagerInstance == null)
                            {
                                _exPNLCommManagerInstance = new ClientTradeCommManager();
                                ExposurePnlCacheManager.GetInstance().Initialise(loginUser, _exPNLCommManagerInstance);
                            }
                            ConnectToExposurePNLServer(false);
                            percentTradingToolViewModel = new PercentTradingToolViewModel();
                            percentTradingToolViewModel.LoginUser = loginUser;
                            percentTradingToolViewModel.AccountList = accountList;
                            percentTradingToolViewModel.MFOrAccFromPM = mfOrAccount;
                            percentTradingToolViewModel.SetSymbolBasedOnSymbology(symbol);
                            percentTradingToolWindow = new PercentTradingToolWindow();
                            percentTradingToolViewModel.SecurityMaster = _secMasterServices;
                            percentTradingToolViewModel.Initialize();
                            percentTradingToolWindow.PercentTradingToolViewModel = percentTradingToolViewModel;
                            if (type != "")
                            {
                                percentTradingToolViewModel.SetTypeThroughPM(type);
                            }
                            percentTradingToolWindow.Closed += percentTradingToolWindow_Closed;
                            BringFormToFront(percentTradingToolWindow);
                            percentTradingToolViewModel.SymbolEntered(isOpenFromPM);
                        }
                    }
                    else
                    {
                        if (percentTradingToolWindow != null)
                        {
                            if (!isReloadPTTDataAllowed)
                            {
                                DialogResult userChoice = MessageBox.Show(this, "PTT UI is already opened. Do you want to reload with new data?", "Position Management", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (userChoice == DialogResult.Yes)
                                {
                                    percentTradingToolViewModel = percentTradingToolWindow.PercentTradingToolViewModel;
                                    percentTradingToolViewModel.MFOrAccFromPM = mfOrAccount;
                                    percentTradingToolViewModel.SetSymbolBasedOnSymbology(symbol);
                                    percentTradingToolViewModel.AccountList = accountList;
                                    percentTradingToolViewModel.LoginUser = loginUser;
                                    percentTradingToolViewModel.ReloadPttUI.Execute(null);
                                    if (type != "")
                                    {
                                        percentTradingToolViewModel.SetTypeThroughPM(type);
                                    }
                                    percentTradingToolViewModel.SymbolEntered(isOpenFromPM);
                                }
                            }
                            if (percentTradingToolWindow.WindowState == System.Windows.WindowState.Minimized)
                            {
                                percentTradingToolWindow.WindowState = System.Windows.WindowState.Normal;
                            }
                            if (!_loadingLayout)
                                percentTradingToolWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                            percentTradingToolWindow.Activate();
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

        void percentTradingToolWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                percentTradingToolWindow = null;
                if (OpenModulesList.Contains(PranaModules.PERCENT_TRADING_TOOL))
                    OpenModulesList.Remove(PranaModules.PERCENT_TRADING_TOOL);
                DisconnectToExposurePNLServer(false);
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

        void UserProfileWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                if (OpenModulesList.Contains(PranaModules.USER_PROFILE_MODULE))
                    OpenModulesList.Remove(PranaModules.USER_PROFILE_MODULE);
                if (_userProfileWindow != null)
                    _userProfileWindow = null;
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

        public void OnEventHandler(PTTTradeClicked e)
        {
            try
            {
                _uiSyncContext.Post(o => { LaunchTradingTicketDock(e.Order, TradingTicketParent.PTT, 0); }, null);
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

        public void OnEventHandler(PTTComplianceAlerts e)
        {
            try
            {
                BlockedAlertsViewer view = new BlockedAlertsViewer(AlertPopUpType.ComplianceCheck);
                view.AddAlerts(e.Alerts);
                view.ShowDialog(this);
                view.Dispose();
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

        public void OnEventHandler(PTTSymbolLookUpClicked e)
        {
            try
            {
                if (e.Args != null)
                {
                    _arguments = e.Args;
                }
                _uiSyncContext.Post(o => { LaunchPluggableTool(ApplicationConstants.CONST_SYMBOL_LOOKUP, GetWindowNewLocation(e.ParentWindow)); }, null);
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

        public void OnEventHandler(ClosePTTUI e)
        {
            try
            {
                if (percentTradingToolWindow != null)
                {
                    _uiSyncContext.Post(o => { percentTradingToolWindow.Close(); }, null);
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
        /// Called when [Preference btn is Clicked from PTT].
        /// </summary>
        /// <param name="e">The e.</param>
        public void OnEventHandler(PTTPreferenceClicked e)
        {
            try
            {
                _uiSyncContext.Post(o => { LaunchPreferncesModule(PranaModules.PERCENT_TRADING_TOOL, GetFormNewLocation(null)); }, null);
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

        public void OnEventHandler(OpenMultiTTFromPTT e)
        {
            try
            {
                _uiSyncContext.Post(o => { LaunchMultiTradingTicketDock(e.OrderList, GetFormNewLocation(null), TradingTicketParent.PTT); }, null);
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

        /// Handles the KeyDown event of the NativeMethods control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void NativeMethods_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (!Program.IsModalDialogOpen)
                    {
                        if (e.Control && e.Shift)
                        {
                            if (e.KeyCode == Keys.T)
                            {
                                LaunchTradingTicketDock(TradingTicketParent.None);
                            }
                            else if (e.KeyCode == Keys.P)
                            {
                                LaunchPortfolioManagementForm();
                            }
                            else if (e.KeyCode == Keys.A)
                            {
                                LaunchAllocationForm();
                            }
                            else if (e.KeyCode == Keys.G)
                            {
                                LaunchCashManagement();
                            }
                            else if (e.KeyCode == Keys.B)
                            {
                                LaunchBlotterFormNew();
                            }
                            else if (e.KeyCode == Keys.E)
                            {
                                LaunchPercentTradingTool(true);
                            }
                            else if (e.KeyCode == Keys.C)
                            {
                                LaunchPluggableTool(ApplicationConstants.CONST_COMPLIANCE, GetFormNewLocation(null));
                            }
                            else if (e.KeyCode == Keys.R)
                            {
                                LaunchRebalancer();
                            }
                            else if (e.KeyCode == Keys.W)
                            {
                                LaunchWatchList();
                            }
                            else if (e.KeyCode == Keys.S)
                            {
                                LaunchPluggableTool(ApplicationConstants.CONST_SYMBOL_LOOKUP, GetFormNewLocation(null));
                            }
                            else if (e.KeyCode == Keys.D)
                            {
                                LaunchMarkPriceUI(null);
                            }
                            else if (e.KeyCode == Keys.I)
                            {
                                LaunchPluggableTool("Import Data", GetFormNewLocation(null));
                            }
                        }

                        else if (e.Control && e.Alt)
                        {
                            if (e.KeyCode == Keys.O)
                            {
                                LaunchOptionChain();
                            }
                            else if (e.KeyCode == Keys.R)
                            {
                                LaunchPluggableTool("Risk Analysis", GetFormNewLocation(null));
                            }
                            else if (e.KeyCode == Keys.T)
                            {
                                LaunchThirdPartyManager();
                            }
                            else if (e.KeyCode == Keys.B)
                            {
                                LaunchBlotterReportsForm();
                            }
                            else if (e.KeyCode == Keys.A)
                            {
                                LaunchAuditUI(new KeyValuePair<string, object[]>("none", null), GetFormNewLocation(this));
                            }
                            else if (e.KeyCode == Keys.E)
                            {
                                LaunchPluggableTool("Reconciliation", GetFormNewLocation(null));
                            }
                            else if (e.KeyCode == Keys.C)
                            {
                                LaunchClosingUI(null, null);
                            }
                            else if (e.KeyCode == Keys.P)
                            {
                                LaunchPluggableTool("Pricing Inputs", GetFormNewLocation(null));
                            }
                            else if (e.KeyCode == Keys.D)
                            {
                                LaunchMappingUI(new EventArgs(), null);
                            }
                            else if (e.KeyCode == Keys.S)
                            {
                                LaunchCreateTransaction(null);
                            }
                            else if (e.KeyCode == Keys.I)
                            {
                                LaunchCorporateActionsUI(null);
                            }
                            else if (e.KeyCode == Keys.F)
                            {
                                LaunchPreferncesModule("", GetFormNewLocation(null));
                            }
                            else if (e.KeyCode == Keys.L)
                            {
                                LaunchShortLocate();
                            }
                            else if (e.KeyCode == Keys.K)
                            {
                                LaunchCounterParty();
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

        private void moduleShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LaunchShortcutsForm();
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

        private void LaunchShortcutsForm()
        {
            try
            {
                if (frmPranaShortcuts == null)
                {
                    frmPranaShortcuts = new PranaShortcuts();
                    frmPranaShortcuts.Owner = this;
                    frmPranaShortcuts.ShowInTaskbar = false;
                    frmPranaShortcuts.Closing += new CancelEventHandler(frmPranaShortcuts_Closing);
                }
                frmPranaShortcuts.Show();
                BringFormToFront(frmPranaShortcuts, GetFormNewLocation(null));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void frmPranaShortcuts_Closing(object sender, CancelEventArgs e)
        {
            frmPranaShortcuts = null;
        }
        private void UltraFormManager1_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            var element = e.Element.GetDescendant(typeof(ButtonUIElement));
            if (element != null)
            {
                if (element.Rect.X == this.Width - 30)
                {
                    element.ToolTipItem = new ToolTipItem("Close");
                    Image img = global::Prana.Properties.Resources.Close_Hover;
                    ((ButtonUIElement)element).Appearance.ImageBackground = img;
                }
                else if (element.Rect.X == this.Width - 52)
                {
                    element.ToolTipItem = new ToolTipItem("Minimize");
                    Image img = global::Prana.Properties.Resources.Minimise_Hover;
                    ((ButtonUIElement)element).Appearance.ImageBackground = img;
                }
                else if (element.Rect.X == this.Width - 74)
                {
                    element.ToolTipItem = new ToolTipItem("Save Layout");
                    Image img = global::Prana.Properties.Resources.SaveLayout_Hover;
                    ((ButtonUIElement)element).Appearance.ImageBackground = img;
                }
                else if (element.Rect.X == this.Width - 96)
                {
                    element.ToolTipItem = new ToolTipItem("User Profile");
                    Image img = global::Prana.Properties.Resources.UserProfile_Hover;
                    ((ButtonUIElement)element).Appearance.ImageBackground = img;
                }
            }
        }

        private void ultraFormManager1_MouseLeaveElement(object sender, UIElementEventArgs e)
        {
            var element = e.Element.GetDescendant(typeof(ButtonUIElement));
            if (element != null)
            {
                if (element.Rect.X == this.Width - 30)
                {
                    element.ToolTipItem = new ToolTipItem("Close");
                    Image img = global::Prana.Properties.Resources.Close;
                    ((ButtonUIElement)element).Appearance.ImageBackground = img;
                }
                else if (element.Rect.X == this.Width - 52)
                {
                    element.ToolTipItem = new ToolTipItem("Minimize");
                    Image img = global::Prana.Properties.Resources.Minimise;
                    ((ButtonUIElement)element).Appearance.ImageBackground = img;
                }
                else if (element.Rect.X == this.Width - 74)
                {
                    element.ToolTipItem = new ToolTipItem("Save Layout");
                    Image img = global::Prana.Properties.Resources.SaveLayout;
                    ((ButtonUIElement)element).Appearance.ImageBackground = img;
                }
                else if (element.Rect.X == this.Width - 96)
                {
                    element.ToolTipItem = new ToolTipItem("User Profile");
                    Image img = global::Prana.Properties.Resources.UserProfile;
                    ((ButtonUIElement)element).Appearance.ImageBackground = img;
                }
            }
        }

        internal void SetAppArgs(string[] args)
        {
            if (args.Length > 0)
            {
                _argsIPAddress = args[0];
                Logger.LoggerWrite("IPAddress: " + _argsIPAddress + " passed via command line parameter.");
            }
            else
            {
                Logger.LoggerWrite("No IPAddress passed via command line parameter.");
            }
        }

        #region custom reports

        Form serverReportForm = null;
        Dictionary<string, Form> _serverReportMenuItems = new Dictionary<string, Form>();
        private event ToolClickEventHandler ClickServerReportsLinks;
        private void LoadServerReportsForUser()
        {
            try
            {
                DataTable dtServerReportsLayout = ServerReportsManager.GetServerReports(loginUser.CompanyID, loginUser.CompanyUserID);
                this.ClickServerReportsLinks = new ToolClickEventHandler(PranaMain_ClickServerReportsLinks);
                PopupMenuTool popup = reports2;
                int section = 0;
                foreach (DataRow serverReportLinkRow in dtServerReportsLayout.Rows)
                {
                    int sectionID = Convert.ToInt32(serverReportLinkRow["SectionID"].ToString());
                    string sectionName = serverReportLinkRow["SectionName"].ToString();
                    string reportName = serverReportLinkRow["ReportName"].ToString();

                    // IF SECTION CHANGES THEN ADD ANOTHER SECTION TO MENU
                    if (section != sectionID)
                    {
                        if (sectionName != String.Empty) // CREATE Sub-Menu
                        {
                            popup = new PopupMenuTool(sectionName.Replace(" ", ""));

                            popup.SharedPropsInternal.Caption = sectionName;
                            this.PranaMainToolBar.Tools.Add(popup);
                            reports2.Tools.Add(popup);
                        }
                        else
                        {
                            popup = reports2;
                        }
                        section = sectionID;
                    }
                    AddReportMenuItem(popup, serverReportLinkRow);
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

        private void AddReportMenuItem(PopupMenuTool parentMenu, DataRow rowReport)
        {
            try
            {
                string appReportServer_IP = ConfigurationHelper.Instance.GetAppSettingValueByKey("ReportServerIP_Initial");
                string reportName = rowReport["ReportName"].ToString();
                string reportLink = rowReport["ReportLink"].ToString();
                Infragistics.Win.UltraWinToolbars.ButtonTool report1 = new Infragistics.Win.UltraWinToolbars.ButtonTool(reportName.Replace(" ", ""));
                report1.ToolClick += ClickServerReportsLinks;
                report1.SharedPropsInternal.Caption = reportName;
                //Adds root address only if link is not complete
                if (reportLink.ToUpper().Contains("HTTP"))
                {
                    report1.SharedPropsInternal.Tag = reportLink;
                }
                else
                {
                    report1.SharedPropsInternal.Tag = appReportServer_IP + reportLink;
                }
                this.PranaMainToolBar.Tools.Add(report1);
                parentMenu.Tools.Add(report1);
                //parentMenu.DropDownItems.Add(menuItem);
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

        private void PranaMain_ClickServerReportsLinks(object sender, EventArgs e)
        {
            try
            {
                string serverReportLink = ((ButtonTool)sender).SharedPropsInternal.Tag.ToString();
                string serverReportName = (((ButtonTool)sender).CaptionResolved.ToString());
                LaunchServerReports(serverReportLink, serverReportName);
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

        private void LaunchServerReports(string serverReportURL, string serverReportName)
        {
            try
            {
                if (ModuleManager.CheckModulePermissioning(PranaModules.SERVERREPORTFORM_MODULE, PranaModules.SERVERREPORTFORM_MODULE))
                {
                    if (!_serverReportMenuItems.ContainsKey(serverReportName) || serverReportForm == null)
                    {
                        DynamicClass formToLoad;
                        formToLoad = (DynamicClass)ModuleManager.AvailableModulesDetails[PranaModules.SERVERREPORTFORM_MODULE];

                        Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);

                        Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);
                        IPositionManagementReports pmPortfolioServerReports = null;
                        pmPortfolioServerReports = (IPositionManagementReports)Activator.CreateInstance(typeToLoad);
                        ((System.Windows.Forms.Form)(pmPortfolioServerReports)).Name = serverReportName;
                        pmPortfolioServerReports.FormClosedHandler += new EventHandler(ServerReport_FormClosed);
                        pmPortfolioServerReports.LoginUser = loginUser;
                        pmPortfolioServerReports.ServerReportURL = serverReportURL;
                        pmPortfolioServerReports.ServerReportName = serverReportName;

                        serverReportForm = pmPortfolioServerReports.Reference();

                        serverReportForm.Owner = this;
                        serverReportForm.ShowInTaskbar = false;
                        serverReportForm.Show();

                        _serverReportMenuItems.Add(serverReportName, serverReportForm);
                        SetFromLayoutDetail(serverReportForm);
                    }
                    serverReportForm = _serverReportMenuItems[serverReportName];
                    BringFormToFront(serverReportForm, GetFormNewLocation(null));
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

        void ServerReport_FormClosed(object sender, EventArgs e)
        {
            string reportToBePulled = ((System.Windows.Forms.Form)(sender)).Text;
            _serverReportMenuItems.Remove(reportToBePulled);
            serverReportForm = null;
        }

        /// <summary>
        /// Client connectivity response from _clientConnectivityService.
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="isLoggedOut"></param>
        public void ClientConnectivityResponseEnterprise(int companyUserID, bool isLoggedOut)
        {
            try
            {
                Logger.LoggerWrite("ClientConnectivityResponseEnterprise called in Prana with message-You were forcefully logged out from Enterprise and logged in to SAMSARA-. companyUserID: " + companyUserID);
                PerformLogoutOperations();
                MessageBox.Show("You were forcefully logged out from Enterprise and logged in to SAMSARA", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void ClientConnectivityResponseWeb(int companyUserID, bool isLoggedOut)
        {
            //try
            //{
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
            //}
            //catch (Exception ex)
            //{
            //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            //    if (rethrow)
            //        throw;
            //}
        }

        #endregion

    }

    /// <summary>
    /// This class adds on to the functionality provided in System.Windows.Forms.ToolStrip.
    /// </summary>
    public class MenuStripEx
        : MenuStrip
    {
        public MenuStripEx()
        {
            if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
            {
                this.RenderMode = ToolStripRenderMode.Professional;
                this.Renderer = new ToolStripProfessionalRenderer(new MenuStripRenderer());
            }
        }
        private bool clickThrough = false;

        /// <summary>
        /// Gets or sets whether the ToolStripEx honors item clicks when its containing form does
        /// not have input focus.
        /// </summary>
        /// <remarks>
        /// Default value is false, which is the same behavior provided by the base ToolStrip class.
        /// </remarks>
        public bool ClickThrough
        {
            get { return this.clickThrough; }
            set { this.clickThrough = value; }
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (this.clickThrough &&
                m.Msg == NativeConstants.WM_MOUSEACTIVATE &&
                m.Result == (IntPtr)NativeConstants.MA_ACTIVATEANDEAT)
            {
                m.Result = (IntPtr)NativeConstants.MA_ACTIVATE;
            }
        }
    }

    /// <summary>
    /// This class is added for change the color of MenuStripItem on selection
    /// </summary>
    [ComVisible(false)]
    public class MenuStripRenderer : ProfessionalColorTable
    {
        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.FromArgb(158, 156, 157); }
        }
        public override Color MenuItemPressedGradientMiddle
        {
            get { return Color.FromArgb(158, 156, 157); }
        }
        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.FromArgb(158, 156, 157); }
        }
    }

    internal sealed class NativeConstants
    {
        private NativeConstants()
        {
        }
        internal const uint WM_MOUSEACTIVATE = 0x21;
        internal const uint MA_ACTIVATE = 1;
        internal const uint MA_ACTIVATEANDEAT = 2;
        internal const uint MA_NOACTIVATE = 3;
        internal const uint MA_NOACTIVATEANDEAT = 4;
    }
}
