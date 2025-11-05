using ExportGridsData;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Forms
{
    public partial class CloseTrade : Form, IClosingUI, IPublishing, IExportGridData
    {
        ISecurityMasterServices _securityMaster = null;
        string _commaSeparatedAccountIds = string.Empty;
        Dictionary<int, string> _dictAccounts = new Dictionary<int, string>();

        protected StatusBar mainStatusBar = new StatusBar();
        protected StatusBarPanel statusPanel = new StatusBarPanel();
        protected StatusBarPanel datetimePanel = new StatusBarPanel();
        Timer ProgressTimer = new Timer();
        public bool _isParentFormAllocation = false;
        string _statusMessage = string.Empty;
        bool _isInitialized = false;
        bool _isCloseTradeUIPopulated = false;
        bool _firstTimeLoadForm = true;
        bool _isFetchingDataNow = false;

        delegate void UpdateUIFilterHandler(DateTime fromdate, DateTime todate, bool ischecked);
        public CloseTrade()
        {
            InitializeComponent();
        }

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        public void SetUp(string ParentFormName, AllocationGroup group)
        {
            try
            {
                SetupSnapshotControl();
                Logger.LoggerWrite("Start: Setup method for close trade form", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                stopwatch.Start();
                // this method is called while Launching Closing UI and allocation UI. So basic initialization will be done only once
                if (!_isInitialized)
                {
                    ClosingPrefManager.SetUp(_user.CompanyUserID, Application.StartupPath);
                    ProgressTimer.Interval = (1000) * (1);              // Timer will tick evert second
                    _isInitialized = true;
                }
                if (ParentFormName != null && (ParentFormName.Equals("AllocationMain") || ParentFormName.Equals("Allocation")))
                {
                    _isParentFormAllocation = true;
                    tabCloseTradeMain.SelectedTab = tabCloseTradeMain.Tabs[2];
                    tabCloseTradeMain.ActiveTab = tabCloseTradeMain.Tabs[2];
                    if (group != null)
                        ctrlCloseTradefromAllocation1.SetUp(group);
                }
                else
                {
                    _isParentFormAllocation = false;
                    if (!_isCloseTradeUIPopulated)
                        PopulateCloseTradeUI();
                }
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                Logger.LoggerWrite("End: Setup method for close trade form (in Milliseconds): " + ts.TotalMilliseconds, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
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

        #region Wire Events
        private void WireEvents()
        {
            try
            {
                //closing services disconnect and connect event handler
                _closingServices.DisconnectedEvent += new Proxy<IClosingServices>.ConnectionEventHandler(_closingServices_ClearData);
                _closingServices.ConnectedEvent += new Proxy<IClosingServices>.ConnectionEventHandler(_closingServices_RefreshData);

                //disable enbale event handler for all the tab controls
                closeTradeWithSplitter1.DisableEnableParentForm += new EventHandler<EventArgs<string, bool, bool>>(closeTradeWithSplitter1_DisableEnableParentForm);
                ctrlExpiryandSettlementNew1.DisableEnableParentForm += new EventHandler<EventArgs<string, bool, bool>>(ctrlExpiryandSettlementNew1_DisableEnableParentForm);
                ctrlCloseTradefromAllocation1.DisableEnableParentForm += new EventHandler<EventArgs<string, bool, bool>>(ctrlCloseTradefromAllocation1_DisableEnableParentForm);

                //this method sets status message from close trade from allocation form.
                ctrlCloseTradefromAllocation1.SetStatusMessage += new EventHandler<EventArgs<string>>(ctrlCloseTradefromAllocation1_SetStatusMessage);

                //event wiring for save layout methods
                closeTradeWithSplitter1.SaveLayout += new EventHandler(closeTradeWithSplitter1_SaveLayout);
                ctrlCloseTradefromAllocation1.SaveLayout += new EventHandler(ctrlCloseTradefromAllocation1_SaveLayout);

                //timer event handler
                ProgressTimer.Tick += new EventHandler(ProgressTimer_Tick); // Everytime timer ticks, timer_Tick will be called
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

        void ctrlCloseTradefromAllocation1_DisableEnableParentForm(object sender, EventArgs<string, bool, bool> e)//string message, bool Flag, bool TimerFlag)
        {
            try
            {
                DisableEnableForm(e.Value, e.Value2, e.Value3);
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

        void ctrlExpiryandSettlementNew1_DisableEnableParentForm(object sender, EventArgs<string, bool, bool> e)
        {
            try
            {
                DisableEnableForm(e.Value, e.Value2, e.Value3);
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

        void closeTradeWithSplitter1_DisableEnableParentForm(object sender, EventArgs<string, bool, bool> e)
        {
            try
            {
                DisableEnableForm(e.Value, e.Value2, e.Value3);
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

        void ctrlCloseTradefromAllocation1_SetStatusMessage(Object sender, EventArgs<string> e)
        {
            try
            {
                SetStatusMessage(e.Value);
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

        #endregion
        ProxyBase<IClosingServices> _closingServices = null;

        private CompanyUser _user;
        public CompanyUser User
        {
            set { _user = value; }
        }

        bool _isFetchClosingData = false;
        /// <summary>
        /// This method is called before setup and load of closeTrade Form so proxies will be created in this method.
        /// Narendra Kumar Jangir, Aug 09,2013
        /// </summary>
        /// <param name="isAllocationParent"></param>
        public void SetParentFormAndCreateProxies(bool isAllocationParent)
        {
            _isParentFormAllocation = isAllocationParent;
            CreateClosingServicesProxy();
            CreateAllocationServicesProxy();
            CreateSubscriptionServicesProxy();
        }

        private delegate void UIThreadMarsheller(object sender, EventArgs e);

        private void CreateClosingServicesProxy()
        {
            try
            {
                _closingServices = new ProxyBase<IClosingServices>("TradeClosingServiceEndpointAddress");
                closeTradeWithSplitter1.ClosingServices = _closingServices;
                ctrlExpiryandSettlementNew1.ClosingServices = _closingServices;
                ctrlCloseTradefromAllocation1.ClosingServices = _closingServices;
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
        private IPricingService PricingServices
        {
            get
            {
                return ctrlExpiryandSettlementNew1.PricingServices;
            }
        }

        DuplexProxyBase<ISubscription> _proxy;
        private void CreateSubscriptionServicesProxy()
        {
            try
            {
                CreateAccountDictionary();

                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);

                #region Create filter
                FilterDataByExactAccount accountFilterdata = new FilterDataByExactAccount();
                accountFilterdata.GivenAccountID = _dictAccounts.Keys.ToList();

                List<FilterData> filter = new List<FilterData>();
                filter.Add(accountFilterdata);
                #endregion

                _proxy.Subscribe(Topics.Topic_Allocation, filter);
                _proxy.Subscribe(Topics.Topic_Closing, filter);
                _proxy.Subscribe(Topics.Topic_Closing_NetPositions, filter);
                _proxy.Subscribe(Topics.Topic_UnwindPositions, null);
                _proxy.Subscribe(Topics.Topic_SecurityMaster, null);
                _proxy.Subscribe(Topics.Topic_ClosingCorrupted, null);
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
        //void _proxy_ProxyStatusChanged(PranaInternalConstants.ConnectionStatus status)
        //{
        //    if (status == PranaInternalConstants.ConnectionStatus.CONNECTED)
        //    {
        //        CreateSubscriptionServicesProxy();
        //    }
        //}

        void CloseTrade_Load(object sender, System.EventArgs e)
        {
            try
            {
                //add date time panel at the bottom of close trade UI
                AddStatusPanel();

                //wire events so that function of this form can be accessed from the child control
                WireEvents();

                InitializeControls();

                _statusMessage = "Getting Data Please Wait";
                btnRefresh.Text = "Get Data";

                if (_isParentFormAllocation)
                {
                    _isFetchClosingData = false;
                    tabCloseTradeMain.SelectedTab = tabCloseTradeMain.Tabs[2];
                    tabCloseTradeMain.ActiveTab = tabCloseTradeMain.Tabs[2];
                    DisableEnableForm(_statusMessage, false, true);
                }
                else
                {
                    DisableEnableForm(_statusMessage, false, true);
                    FetchClosingDataAsync();
                }

                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSE_TRADE);
                CustomThemeHelper.SetThemeProperties(ctrlCloseTradefromAllocation1, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CTRL_CLOSE_TRADE_ALLOCATION);
                CustomThemeHelper.SetThemeProperties(ctrlCloseTradefromAllocation1.GetGridInstance(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSE_TRADE_GRID);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
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
            //isLoaded = true;


        }

        private void SetButtonsColor()
        {
            try
            {
                btnRefresh.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRefresh.ForeColor = System.Drawing.Color.White;
                btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRefresh.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRefresh.UseAppStyling = false;
                btnRefresh.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAdvanceOptions.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAdvanceOptions.ForeColor = System.Drawing.Color.White;
                btnAdvanceOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAdvanceOptions.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAdvanceOptions.UseAppStyling = false;
                btnAdvanceOptions.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void InitializeControls()
        {
            //add accounts to the check list default value will be unchecked
            MultiSelectDropDown1.AddItemsToTheCheckList(_dictAccounts, CheckState.Checked);

            //adjust checklistbox width according to the longest accountname
            MultiSelectDropDown1.AdjustCheckListBoxWidth();
            MultiSelectDropDown1.TitleText = "Account";
            MultiSelectDropDown1.SetTextEditorText("All Account(s) Selected");

            dtFromDate.Enabled = false;
            dtFromDate.Value = DateTime.Now;

            dtToDate.Enabled = false;
            dtToDate.Value = DateTime.Now;
            ctrlExpiryandSettlementNew1.SecurityMaster = _securityMaster;

            //load data on the basis of given closing preferences
            _isFetchClosingData = ClosingPrefManager.GetPreferences().IsFetchDataAutomatically;
            _commaSeparatedAccountIds = MultiSelectDropDown1.GetCommaSeperatedAccountIds();
        }

        private void CreateAccountDictionary()
        {
            try
            {
                _dictAccounts = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict();
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

        private void AddStatusPanel()
        {
            try
            {
                //datetimePanel and statusPanel removed from SetUp method and addded to Load method
                //add status bar
                // Set second panel properties and add to StatusBar
                datetimePanel.BorderStyle = StatusBarPanelBorderStyle.None;
                //datetimePanel.ToolTipText = "[" + DateTime.Now.ToString() + "]";
                datetimePanel.Text = "[" + DateTime.Now.ToString() + "]";
                datetimePanel.AutoSize = StatusBarPanelAutoSize.Contents;
                mainStatusBar.Panels.Add(datetimePanel);
                // Set first panel properties and add to StatusBar
                statusPanel.BorderStyle = StatusBarPanelBorderStyle.None;
                //statusPanel.ToolTipText = string.Empty();
                statusPanel.Text = _statusMessage;
                statusPanel.AutoSize = StatusBarPanelAutoSize.Contents;
                mainStatusBar.Panels.Add(statusPanel);
                mainStatusBar.ShowPanels = true;
                // Add StatusBar to Form controls
                this.Controls.Add(mainStatusBar);
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

        private void PopulateCloseTradeUI()
        {
            try
            {
                Logger.LoggerWrite("Start: Populate close amend and expiration settlement", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                stopwatch.Start();

                _isCloseTradeUIPopulated = true;
                ctrlExpiryandSettlementNew1.PopulateCloseTradesInterfaceForExpirataionandSettlement(_user);
                closeTradeWithSplitter1.PopulateCloseTradesInterface();

                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                Logger.LoggerWrite("End: Populate close amend and expiration settlement (in Milliseconds): " + ts.TotalMilliseconds, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
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

        void ctrlCloseTradefromAllocation1_SaveLayout(object sender, EventArgs e)
        {
            try
            {
                SaveGridsLayout();
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

        void closeTradeWithSplitter1_SaveLayout(object sender, EventArgs e)
        {
            try
            {
                SaveGridsLayout();
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

        //public IClosingServices ClosingServices
        //{
        //    set {
        //        _closingServices = value;
        //        closeTradeWithSplitter1.ClosingServices = _closingServices;
        //        ctrlExpiryandSettlementNew1.ClosingServices = _closingServices;
        //    }
        //}

        ProxyBase<IAllocationManager> _allocationServices = null;

        private void CreateAllocationServicesProxy()
        {
            try
            {
                _allocationServices = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
                ctrlExpiryandSettlementNew1.AllocationServices = _allocationServices;
                closeTradeWithSplitter1.AllocationServices = _allocationServices;
                ctrlCloseTradefromAllocation1.AllocationServices = _allocationServices;
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

            //AllocationManager.GetInstance().AllocationServices = _proxyAllocationServices;
            //_proxyAllocationServices=b.InnerChannel;

            //EndpointAddress endpointAddress = new EndpointAddress(endpointAddressInString);
            //NetTcpBinding netTcpBinding = new NetTcpBinding();
            // _proxyAllocationServices = ChannelFactory<IAllocationServices>.CreateChannel(netTcpBinding, endpointAddress);
        }

        private void _closingServices_ClearData(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        // Modified By : Manvendra Prajapati
                        // Purpose : Handle parameter mismatch error when Close Trade UI is open and close the Server
                        this.BeginInvoke(new EventHandler(_closingServices_ClearData), sender, e);
                    }
                    else
                    {
                        ClosingClientSideMapper.ClearRepository();
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

        private void _closingServices_RefreshData(object sender, EventArgs e)
        {
            try
            {
                UIThreadMarsheller mi = new UIThreadMarsheller(_closingServices_RefreshData);
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        // Modified By : Manvendra Prajapati
                        // Purpose : Handle parameter mismatch error when Close Trade UI is open and close the Server
                        this.BeginInvoke(new EventHandler(_closingServices_RefreshData), sender, e);
                    }
                    else
                    {
                        btnRefresh_Click(null, null);
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

        void CloseTrade_FormClosed(object sender, System.EventArgs e)
        {
            try
            {

                //if (FormClosed != null)
                //{
                //    FormClosed(this, EventArgs.Empty);
                //}

                _closingServices.DisconnectedEvent -= new Proxy<IClosingServices>.ConnectionEventHandler(_closingServices_ClearData);
                _closingServices.ConnectedEvent -= new Proxy<IClosingServices>.ConnectionEventHandler(_closingServices_RefreshData);
                InstanceManager.ReleaseInstance(typeof(CloseTrade));
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

        void CloseTrade_Disposed(object sender, System.EventArgs e)
        {
            try
            {
                if (FormClosed != null)
                {
                    FormClosed(this, EventArgs.Empty);
                    ctrlCloseTradefromAllocation1.RemoveAllDetails();
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

        void CloseTrade_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {
                while (_isFetchingDataNow)
                {
                    //AM:Do not Allow user to close the form until unless all the events completed 
                    Application.DoEvents();
                }
                // _proxy.UnSubscribe();

                //_proxy.UnSubscribe(Topics.Topic_Allocation);
                //_proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing_NetPositions);
                //_proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing);
                _closingServices.Dispose();
                if (_proxy != null && _proxy.InnerChannel != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_Allocation);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing_NetPositions);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_UnwindPositions);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_SecurityMaster);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_ClosingCorrupted);
                    _proxy.Dispose();
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

        async void FetchClosingDataAsync()
        {
            try
            {
                ClosingTemplate previewTemplate = null;                 //TODO - Here ClosingTemplate is not being used (need to refactor)
                ClosingData closingData = null;
                if (_isFetchClosingData)
                {
                    _isFetchingDataNow = true;
                    closingData = await System.Threading.Tasks.Task.Run(() => GetFilteredData(previewTemplate));
                }
                if (closingData != null)
                {
                    FillOptionsSpecificDetails(closingData.Taxlots);

                    ClosingClientSideMapper.CreateRepository(closingData);
                    closeTradeWithSplitter1.SetGridDataSources();
                    ctrlExpiryandSettlementNew1.SetGridDataSources();
                    if (!ClosingClientSideMapper.IsDataAvailabletoClose())
                    {
                        MessageBox.Show("No data exists for the filters selected.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                _isFetchingDataNow = false;
                _statusMessage = "Data Fetched Successfully";                       //TODO
                ToggleUIElementsWithMessage(true);
                btnRefresh.Text = "Refresh";
                if (!_isFetchClosingData)
                {
                    _statusMessage = "To Fetch Data Please click on Get Data Button.";
                    btnRefresh.Text = "Get Data";
                    //_isFetchClosingData = true;
                }
                DisableEnableForm(_statusMessage, true, false);
                bool isCurrentTABCloseOrderUI = CheckIsCloseOrderUI();
                DisableEnableTopControls(!isCurrentTABCloseOrderUI);
            }
        }

        internal bool CheckIsCloseOrderUI()
        {
            bool isCurrentTABCloseOrderUI = false;
            try
            {
                if (tabCloseTradeMain.SelectedTab.Key.Equals("CloseOrder"))
                {
                    isCurrentTABCloseOrderUI = true;
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
            return isCurrentTABCloseOrderUI;
        }

        internal async Task<ClosingData> GetFilteredData(ClosingTemplate template)
        {
            ClosingData data = new ClosingData();
            try
            {

                if (template == null)
                {
                    data = await System.Threading.Tasks.Task.Run(() => ClosingClientSideMapper.GetAllClosingData(ClosingPrefManager.FromDate, ClosingPrefManager.CloseTradeDate, ClosingPrefManager.IsCurrentDateClosing, _closingServices, _commaSeparatedAccountIds, string.Empty, string.Empty, string.Empty));
                }
                else
                {

                    //if ( (template.ToDate.Date != DateTime.UtcNow.Date || template.FromDate.Date != DateTime.UtcNow.Date))
                    //{
                    UpdateHistoricalDateFilterOnUI(template.FromDate, template.ToDate, true);
                    //}


                    Dictionary<int, List<int>> dictMasterFundSubAccountAssociation = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();

                    DateTime toDate = template.ToDate;
                    if (template.UseCurrentDate)
                    {
                        toDate = DateTime.UtcNow;
                    }
                    data = await System.Threading.Tasks.Task.Run(() => ClosingClientSideMapper.GetAllClosingData(template.FromDate, toDate, false, _closingServices, template.GetCommaSeparatedAccounts(dictMasterFundSubAccountAssociation), template.GetCommaSeparatedAssets(), template.GetCommaSeparatedSymbols(), SqlParser.GetDynamicConditionQuerry(template.DictCustomConditions)));
                }
                //closeTradeWithSplitter1.SetGridDataSources();
                //ctrlExpiryandSettlementNew1.SetGridDataSources();



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
            return data;
        }

        private void UpdateHistoricalDateFilterOnUI(DateTime fromdate, DateTime todate, bool ischecked)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new UpdateUIFilterHandler(UpdateHistoricalDateFilterOnUI), fromdate, todate, ischecked);
                    }
                    else
                    {
                        rbHistorical.Checked = ischecked;
                        dtToDate.Enabled = ischecked;
                        dtFromDate.Enabled = ischecked;
                        dtToDate.Value = todate;
                        dtFromDate.Value = fromdate;
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

        private void dtFromDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ClosingPrefManager.FromDate = Convert.ToDateTime((dtFromDate.Value));
                //To do: set the FromDate when changed the date  
                ResetAllFilters(ClosingPrefManager.CloseTradeDate);

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

        private void ResetAllFilters(DateTime toDate)
        {
            //filter to date
            try
            {
                FilterDataByToDate filterdata = new FilterDataByToDate();
                filterdata.ToDate = toDate;


                //filter date modified
                FilterDataForLastDateModified filterdataDateModified = new FilterDataForLastDateModified();
                filterdataDateModified.TillDate = toDate;

                List<FilterData> filters = new List<FilterData>();
                filters.Add(filterdata);
                filters.Add(filterdataDateModified);

                string[] separator = new string[] { "," };
                List<int> accountIDs = MultiSelectDropDown1.GetCommaSeperatedAccountIds().Trim().Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                FilterDataByExactAccount accountFilterdata = new FilterDataByExactAccount();
                accountFilterdata.GivenAccountID = accountIDs;

                filters.Add(accountFilterdata);

                if (_proxy != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_Allocation);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing);
                    _proxy.Subscribe(Topics.Topic_Allocation, filters);
                    _proxy.Subscribe(Topics.Topic_Closing, filters);
                }

                //FilterByDateRange
                //List<FilterData> filtersClosedPosition = new List<FilterData>();
                //FilterDataByDateRange filterdataByDateRange = new FilterDataByDateRange();
                //filterdataByDateRange.FromDate = fromDate;
                //filterdataByDateRange.ToDate = toDate;
                //filtersClosedPosition.Add(filterdataByDateRange);
                //_proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing_NetPositions);
                //_proxy.Subscribe(Topics.Topic_Closing_NetPositions, filtersClosedPosition);
            }
            catch (Exception ex)
            {

                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            //_closingServices.SetTodate(dtToDate.Value.Date);

            try
            {
                ClosingPrefManager.CloseTradeDate = Convert.ToDateTime((dtToDate.Value));
                ResetAllFilters(ClosingPrefManager.CloseTradeDate);
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

        private void rbCurrent_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dtToDate.Enabled = !rbCurrent.Checked;
                dtFromDate.Enabled = !rbCurrent.Checked;
                //To do: set the IsCurrentDateClosing when changed the radio buttons 
                //_closingServices.SetCurrentorHistoricalClosing(rbCurrent.Checked);
                //GetFilteredData();

                //_closingServices.IsCurrentDateClosing = rbCurrent.Checked;
                ClosingPrefManager.IsCurrentDateClosing = rbCurrent.Checked;
                if (rbCurrent.Checked == true)
                {
                    _commaSeparatedAccountIds = MultiSelectDropDown1.GetCommaSeperatedAccountIds();
                    //dont make db call if no account selected
                    if (!string.IsNullOrEmpty(_commaSeparatedAccountIds))
                    {
                        _isFetchClosingData = true;
                        _statusMessage = "Getting Data Please Wait";
                        DisableEnableForm(_statusMessage, false, true);
                        //ctrlExpiryandSettlementNew1.Enabled = false;
                        //closeTradeWithSplitter1.Enabled = false;
                        //backgroundWorker.RunWorkerAsync();
                        FetchClosingDataAsync();
                    }
                    //GetFilteredData();
                }
                //DateTime fromDate = DateTime.MinValue;
                DateTime toDate = DateTime.MinValue;
                if (ClosingPrefManager.IsCurrentDateClosing)
                {
                    //fromDate = DateTime.UtcNow;
                    toDate = DateTime.UtcNow;
                }
                else
                {
                    //  fromDate = ClosingPrefManager.FromDate;
                    toDate = ClosingPrefManager.CloseTradeDate;
                }
                ResetAllFilters(toDate);
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

        private void rbHistorical_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dtToDate.Enabled = rbHistorical.Checked;
                dtFromDate.Enabled = rbHistorical.Checked;
            }
            catch (Exception ex)
            {

                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnAdvanceOptions_Click(object sender, EventArgs e)
        {
            try
            {
                ClosingWizardHelper.GetInstance().ClosingServices = _closingServices;

                //unwinding closing topics as we dont want data to get published..
                //if (_proxy != null)
                //{
                //    _proxy.UnsubscribeAsynch(Topics.Topic_Closing);
                //    _proxy.UnsubscribeAsynch(Topics.Topic_Closing_NetPositions);
                //}
                // ClosingWizardHelper.GetInstance().SubscriptionService = _proxy;
                ClosingWizardHelper.GetInstance().OperationStarted += CloseTrade_OperationStarted;
                ClosingWizardHelper.GetInstance().OperationCompleted += CloseTrade_OperationCompleted;
                ClosingWizardHelper.GetInstance().ClosingWizardClosed += new EventHandler(CloseTrade_ClosingWizardClosed);
                ClosingWizardHelper.GetInstance().PreviewData += new EventHandler(CloseTrade_PreviewData);
                ClosingWizardHelper.GetInstance().LaunchClosingWizard();

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

        void CloseTrade_PreviewData(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new EventHandler(CloseTrade_PreviewData), sender, e);
                    }
                    else
                    {
                        btnRefresh_Click(sender, null);
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

        void CloseTrade_OperationCompleted(object sender, EventArgs<string> statusMsg)
        {
            DisableEnableForm(statusMsg.Value, true, false);
            // DisableEnableForm(statusMsg, true, false);
        }

        void CloseTrade_OperationStarted(object sender, EventArgs<string> statusMsg)
        {
            DisableEnableForm(statusMsg.Value, false, true);
        }

        void CloseTrade_ClosingWizardClosed(object sender, EventArgs e)
        {

            try
            {
                ClosingWizardHelper.GetInstance().OperationStarted -= CloseTrade_OperationStarted;
                ClosingWizardHelper.GetInstance().OperationCompleted -= CloseTrade_OperationCompleted;
                ClosingWizardHelper.GetInstance().PreviewData -= new EventHandler(CloseTrade_PreviewData);
                ClosingWizardHelper.GetInstance().ClosingWizardClosed -= new EventHandler(CloseTrade_ClosingWizardClosed);


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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime(dtToDate.Value), Convert.ToDateTime(dtFromDate.Value)) >= 0)
                {
                    _commaSeparatedAccountIds = MultiSelectDropDown1.GetCommaSeperatedAccountIds();
                    //refresh filters of subscriptions as well
                    rbCurrent_CheckedChanged(null, null);
                    if (!string.IsNullOrEmpty(_commaSeparatedAccountIds))
                    {
                        tabCloseTradeMain.Enabled = false;
                        _isFetchClosingData = true;
                        btnRefresh.Text = "Getting Data....";
                        _statusMessage = "Getting Data Please Wait";
                        statusPanel.Text = _statusMessage;
                        DisableEnableForm(_statusMessage, false, true);
                        if (!_isFetchingDataNow)
                        {
                            FetchClosingDataAsync();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("To date cannot be less than From date", "Close Positions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        void tabCloseTradeMain_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (e.Tab.Key.Equals("Expiration/Settlement"))
                {
                    //_isParentFormAllocation = false;
                    if (!_isCloseTradeUIPopulated && _isInitialized)
                        PopulateCloseTradeUI();
                    DisableEnableTopControls(true);
                }
                else if (e.Tab.Key.Equals("ClosedAmend"))
                {
                    //_isParentFormAllocation = false;
                    if (!_isCloseTradeUIPopulated && _isInitialized)
                        PopulateCloseTradeUI();
                    DisableEnableTopControls(true);
                }
                else
                {
                    _isParentFormAllocation = true;
                    DisableEnableTopControls(false);
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

        //Narender Please check
        public void ToggleUIElementsWithMessage(bool flag)
        {
            DisableEnableForm(string.Empty, flag, false);
            //http://jira.nirvanasolutions.com:8080/browse/PRANA-2305
            //disable refresh button on close order ui
            //btnRefresh.Enabled = true;
        }

        //this method disables or enables control based on bool value, false=>disable, true=>enable
        private void DisableEnableForm(string statusMessage, bool Flag, bool TimerFlag)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            DisableEnableForm(statusMessage, Flag, TimerFlag);

                        }));
                    }
                    else
                    {
                        //ControlBox Gets or sets a value indicating whether a control box is displayed in the caption bar of the form.
                        _statusMessage = statusMessage;
                        statusPanel.Text = _statusMessage;
                        datetimePanel.Text = "[" + DateTime.Now.ToString() + "]";
                        // added by sachin mishra purpose: http://jira.nirvanasolutions.com:8080/browse/PRANA-7159
                        if (!_firstTimeLoadForm)
                        {
                            this.ControlBox = Flag;
                            this.MaximizeBox = Flag;
                            this.MinimizeBox = Flag;
                        }
                        _firstTimeLoadForm = false;
                        btnRefresh.Enabled = Flag;
                        rbCurrent.Enabled = Flag;
                        rbHistorical.Enabled = Flag;
                        MultiSelectDropDown1.Enabled = Flag;
                        tabCloseTradeMain.Enabled = Flag;
                        if (TimerFlag)
                        {
                            ProgressTimer.Enabled = true;                       // Enable the timer
                            ProgressTimer.Start();                              // Start the timer
                        }
                        else
                        {
                            ProgressTimer.Stop();
                        }

                        if (tabCloseTradeMain.SelectedTab.Key.Equals("CloseOrder"))
                        {
                            _isParentFormAllocation = true;
                            DisableEnableTopControls(!_isParentFormAllocation);
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

        private void SetupSnapshotControl()
        {
            try
            {

                this.btnScreenshot = SnapShotManager.GetInstance().ultraButton;
                this.splitContainer1.Panel1.Controls.Add(this.btnScreenshot);
                this.btnScreenshot.Location = new System.Drawing.Point(907, 10);
                this.btnScreenshot.Name = "btnScreenshot";
                this.btnScreenshot.Size = new System.Drawing.Size(80, 24);
                this.btnScreenshot.TabIndex = 15;
                this.btnScreenshot.Click += new System.EventHandler(this.btnScreenshot_Click);
                this.splitContainer1.Panel1.Controls.Add(this.btnScreenshot);

                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    btnScreenshot.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                    btnScreenshot.ForeColor = System.Drawing.Color.White;
                    btnScreenshot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btnScreenshot.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                    btnScreenshot.UseAppStyling = false;
                    btnScreenshot.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
                }
                else
                {
                    btnScreenshot.Appearance.ImageBackground = ctrlImageListButtons1.GetImage(4);
                    btnScreenshot.HotTrackAppearance.ImageBackground = ctrlImageListButtons1.GetImage(1);
                    btnScreenshot.PressedAppearance.ImageBackground = ctrlImageListButtons1.GetImage(4);
                    btnScreenshot.Appearance.ImageBackgroundStyle = ImageBackgroundStyle.Stretched;
                    btnScreenshot.UseOsThemes = DefaultableBoolean.False;
                    btnScreenshot.Appearance.ImageHAlign = HAlign.Center;
                    btnScreenshot.Appearance.ImageVAlign = VAlign.Middle;
                    btnScreenshot.Appearance.ForeColor = System.Drawing.Color.WhiteSmoke;
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

        #region IClosingUI Members

        public Form Reference()
        {
            return this;
        }

        public new event EventHandler FormClosed;

        public ISecurityMasterServices SecurityMaster
        {
            set { _securityMaster = value; }
        }

        public void ApplyPreferences(object sender, EventArgs<string, IPreferenceData> e)
        {
            try
            {
                if (!e.Value.Equals(PranaModules.CLOSE_POSITIONS_MODULE)) return;

                {
                    //Update preferences on the server side 
                    //ClosingPrefManager.SavePreferences((ClosingPreferences)prefData, true);

                    //closeTradeWithSplitter1.PresetPrefernces = (ClosingPreferences)prefData;
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

        #region IPublishing Members

        public void Publish(MessageData e, string topicName)
        {
            try
            {
                UIThreadMarshellerPublish mi = new UIThreadMarshellerPublish(Publish);
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(mi, e, topicName);
                    }
                    else
                    {
                        try
                        {
                            System.Object[] dataList = null;
                            ClosingData closingData = new ClosingData();
                            string groupID = ClosingClientSideMapper.GroupId;

                            switch (topicName)
                            {
                                case Topics.Topic_Allocation:

                                    dataList = (System.Object[])e.EventData;

                                    List<TaxLot> listTaxlotToPopulate = new List<TaxLot>();

                                    List<TaxLot> listCloseTradeTaxlots = new List<TaxLot>();

                                    foreach (Object obj in dataList)
                                    {
                                        TaxLot taxlot = (TaxLot)obj;

                                        NameValueFiller.FillNameDetailsOfMessage(taxlot);
                                        FillClosingSpecificDetails(taxlot);
                                        if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.New && taxlot.Level1ID != 0)
                                        {
                                            ClosingClientSideMapper.OpenTaxlots.Add(taxlot);
                                        }
                                        if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                                        {
                                            ClosingClientSideMapper.OpenTaxlots.Remove(taxlot);
                                        }
                                        if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.Updated)
                                        {
                                            ClosingClientSideMapper.OpenTaxlots.Update(taxlot);
                                            ClosingClientSideMapper.UpdateCorrespondingPositions(taxlot);
                                        }
                                        if (taxlot.GroupID.Equals(groupID) && taxlot.TaxLotState != ApplicationConstants.TaxLotState.Deleted)
                                        {
                                            listCloseTradeTaxlots.Add(taxlot);
                                        }

                                        if (ClosingClientSideMapper.GetSidesForClosingTaxlots().Contains(taxlot.OrderSideTagValue) && ClosingClientSideMapper.Symbol.Equals(taxlot.Symbol))
                                        {
                                            listTaxlotToPopulate.Add(taxlot);
                                        }
                                    }
                                    FillOptionsSpecificDetails(dataList.Select(obj => (TaxLot)obj).ToList<TaxLot>());

                                    //update data on close order ui from allocation
                                    if (listCloseTradeTaxlots.Count > 0)
                                        UpdatePublishedDataOnCloseOrderUI(listCloseTradeTaxlots);

                                    if (ClosingClientSideMapper.UpdateTaxLotToPopulateFromAllocation(listTaxlotToPopulate))
                                    {
                                        ctrlCloseTradefromAllocation1.UpdateAllocatedAndAvailableQtyFromParentForm();
                                    }
                                    break;

                                case Topics.Topic_Closing:
                                    dataList = (System.Object[])e.EventData;
                                    List<TaxLot> taxlots = new List<TaxLot>();
                                    List<TaxLot> closeOrderTaxlotsToUpdate = new List<TaxLot>();
                                    bool isCloseOrderTaxlotsUpdated = false;
                                    foreach (Object obj in dataList)
                                    {
                                        TaxLot taxlot = (TaxLot)obj;
                                        taxlots.Add(taxlot);
                                        if (!string.IsNullOrEmpty(groupID) && groupID.Equals(taxlot.GroupID))
                                        {
                                            isCloseOrderTaxlotsUpdated = true;
                                            closeOrderTaxlotsToUpdate.Add(taxlot);
                                        }
                                    }
                                    FillOptionsSpecificDetails(taxlots);
                                    closingData.Taxlots = taxlots;
                                    ClosingClientSideMapper.UpdateRepository(closingData);
                                    if (isCloseOrderTaxlotsUpdated)
                                    {
                                        ctrlCloseTradefromAllocation1.UpdateClosingTaxlots(closeOrderTaxlotsToUpdate);
                                        ctrlCloseTradefromAllocation1.UpdateSellOpenQtyAccountAndStrategyWise();
                                        ctrlCloseTradefromAllocation1.UpdateAllocatedAndAvailableQtyFromParentForm();
                                    }
                                    break;

                                case Topics.Topic_Closing_NetPositions:
                                    dataList = (System.Object[])e.EventData;
                                    List<Position> NetPositions = new List<Position>();
                                    foreach (Object obj in dataList)
                                    {
                                        NetPositions.Add((Position)obj);
                                    }
                                    closingData.ClosedPositions = NetPositions;
                                    ClosingClientSideMapper.UpdateRepository(closingData);
                                    break;

                                case Topics.Topic_UnwindPositions:
                                    dataList = (System.Object[])e.EventData;
                                    string pos = string.Empty;
                                    foreach (Object str in dataList)
                                    {
                                        pos = str.ToString();
                                    }
                                    closingData.PositionsToUnwind = pos;

                                    ClosingClientSideMapper.UpdateRepository(closingData);
                                    break;

                                case Topics.Topic_SecurityMaster:
                                    dataList = (System.Object[])e.EventData;
                                    SecMasterbaseList secMasterObjlist = new SecMasterbaseList();
                                    foreach (Object secmasterObj in dataList)
                                    {
                                        secMasterObjlist.Add((SecMasterBaseObj)secmasterObj);
                                    }
                                    ClosingClientSideMapper.UpdateRepositoryWithSecmasterData(secMasterObjlist);
                                    break;

                                case Topics.Topic_ClosingCorrupted:
                                    dataList = (System.Object[])e.EventData;
                                    DisplayClosingCorruptedMessage(dataList);
                                    break;

                                default:
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

        private void UpdatePublishedDataOnCloseOrderUI(List<TaxLot> taxlotsList)
        {
            try
            {
                //Update Close trade data on close order ui if data is updated from allocation
                //get group id binded to close order ui

                //get updated group from db
                AllocationGroup group = ClosingClientSideMapper.AllocationGroup;// _allocationServices.InnerChannel.GetGroup(groupID);
                group.TaxLots = taxlotsList;

                group.AvgPrice = taxlotsList[0].AvgPrice;
                group.CounterPartyID = taxlotsList[0].CounterPartyID;
                group.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(group.CounterPartyID);

                if (taxlotsList[0].Level1ID == 0)
                {
                    group.State = PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED;
                }

                bool accountIDExists = CheckTaxlotByAccountID(group);

                if (accountIDExists.Equals(true))
                {
                    if (ClosingClientSideMapper.AccountId != 0)
                    {
                        group.AccountID = ClosingClientSideMapper.AccountId;
                    }
                }
                else
                {
                    ClosingClientSideMapper.AccountId = 0;
                    group.AccountID = 0;
                }

                //call setup of close order ui to update all the details
                ctrlCloseTradefromAllocation1.SetUp(group);

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

        private bool CheckTaxlotByAccountID(AllocationGroup group)
        {
            bool accountIDExists = false;
            try
            {
                foreach (TaxLot taxlot in group.TaxLots)
                {
                    if (ClosingClientSideMapper.AccountId == taxlot.Level1ID)
                    {
                        accountIDExists = true;
                        break;
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
            return accountIDExists;
        }

        /// <summary>
        /// Fills the options specific details.
        /// </summary>
        /// <param name="taxlot">The taxlot.</param>
        private void FillOptionsSpecificDetails(List<TaxLot> taxlots)
        {
            try
            {
                taxlots = taxlots.Where(t => t.AssetCategoryValue == AssetCategory.EquityOption && t.StrikePrice > 0.0).ToList();
                if (taxlots.Count > 0)
                {
                    List<Tuple<string, DateTime>> pairs = new List<Tuple<string, DateTime>>();
                    DateTime yesterday = DateTimeHelper.GetYesterdayDate();
                    pairs = taxlots.Select(t => new Tuple<string, DateTime>(t.UnderlyingSymbol,
                        t.ExpirationDate > DateTime.Today ? yesterday : t.ExpirationDate.Date)).ToList();
                    Dictionary<Tuple<string, DateTime>, double> markPrices = PricingServices.GetMarkPricesForOptExpiry(pairs);
                    foreach (TaxLot taxlot in taxlots)
                    {
                        taxlot.DaysToExpiry = (taxlot.ExpirationDate.Date - DateTime.Now.Date).Days;
                        double underlyingMarkPrice = 0;

                        if (markPrices != null)
                        {
                            if (!markPrices.TryGetValue(new Tuple<string, DateTime>(taxlot.UnderlyingSymbol, taxlot.ExpirationDate.Date), out underlyingMarkPrice)
                                 || underlyingMarkPrice <= double.Epsilon)
                            {
                                if (taxlot.ExpirationDate >= DateTime.Today)
                                {
                                    markPrices.TryGetValue(new Tuple<string, DateTime>(taxlot.UnderlyingSymbol, yesterday), out underlyingMarkPrice);
                                }
                            }
                            if (underlyingMarkPrice > double.Epsilon)
                            {
                                if (taxlot.StrikePrice == underlyingMarkPrice)
                                {
                                    taxlot.ItmOtm = OptionMoneyness.ATM;
                                    taxlot.PercentOfITMOTM = 0.0;
                                    taxlot.IntrinsicValue = 0.0m;
                                    taxlot.GainLossIfExerciseAssign = -taxlot.AvgPrice * taxlot.ExecutedQty * taxlot.ContractMultiplier * taxlot.FXRate * taxlot.SideMultiplier;
                                }
                                else if (taxlot.PutOrCall == 1)
                                {
                                    if (taxlot.StrikePrice < underlyingMarkPrice)
                                    {
                                        taxlot.ItmOtm = OptionMoneyness.ITM;
                                        taxlot.IntrinsicValue = (decimal)underlyingMarkPrice - (decimal)taxlot.StrikePrice;
                                    }
                                    else
                                    {
                                        taxlot.ItmOtm = OptionMoneyness.OTM;
                                        taxlot.IntrinsicValue = 0.0m;
                                    }
                                    taxlot.PercentOfITMOTM = (underlyingMarkPrice - taxlot.StrikePrice) * 100 / taxlot.StrikePrice;
                                    taxlot.GainLossIfExerciseAssign = (underlyingMarkPrice - taxlot.StrikePrice - taxlot.AvgPrice)
                                        * taxlot.ExecutedQty * taxlot.ContractMultiplier * taxlot.FXRate * taxlot.SideMultiplier;
                                }
                                else if (taxlot.PutOrCall == 0)
                                {
                                    if (taxlot.StrikePrice > underlyingMarkPrice)
                                    {
                                        taxlot.ItmOtm = OptionMoneyness.ITM;
                                        taxlot.IntrinsicValue = (decimal)taxlot.StrikePrice - (decimal)underlyingMarkPrice;
                                    }
                                    else
                                    {
                                        taxlot.ItmOtm = OptionMoneyness.OTM;
                                        taxlot.IntrinsicValue = 0.0m;
                                    }
                                    taxlot.PercentOfITMOTM = (taxlot.StrikePrice - underlyingMarkPrice) * 100 / taxlot.StrikePrice;
                                    taxlot.GainLossIfExerciseAssign = (taxlot.StrikePrice - underlyingMarkPrice - taxlot.AvgPrice)
                                        * taxlot.ExecutedQty * taxlot.ContractMultiplier * taxlot.FXRate * taxlot.SideMultiplier;
                                }
                            }
                        }
                        if (taxlot.ItmOtm == OptionMoneyness.OTM)
                        {
                            double maxGainLoss = taxlot.AvgPrice * taxlot.ExecutedQty * taxlot.ContractMultiplier * taxlot.FXRate;

                            if (taxlot.GainLossIfExerciseAssign < -maxGainLoss)
                                taxlot.GainLossIfExerciseAssign = -maxGainLoss;
                            else if (taxlot.GainLossIfExerciseAssign > maxGainLoss)
                                taxlot.GainLossIfExerciseAssign = maxGainLoss;
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

        private void FillClosingSpecificDetails(TaxLot taxlot)
        {
            try
            {
                //commented omshiv, ACA code cleanup
                //taxlot.ACAData.ACAAvgPrice = taxlot.AvgPrice;
                taxlot.OpenTotalCommissionandFees = taxlot.TotalCommissionandFees;
                taxlot.AssetCategoryValue = (AssetCategory)taxlot.AssetID;
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


        public string getReceiverUniqueName()
        {
            return "CloseForm";
        }
        #endregion

        void ProgressTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (statusPanel.Text.Equals(string.Empty))
                    statusPanel.Text = _statusMessage;
                StringBuilder progress = new StringBuilder(statusPanel.Text);
                if (progress.Length < 40)
                {
                    progress.Append(". ");
                    statusPanel.Text = progress.ToString();
                }
                else
                    statusPanel.Text = _statusMessage;
            }
            catch (Exception ex)
            {
                statusPanel.Text = String.Empty;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void DisableEnableTopControls(bool Flag)
        {
            try
            {
                statusPanel.Text = string.Empty;
                datetimePanel.Text = "[" + DateTime.Now.ToString() + "]";
                btnRefresh.Enabled = Flag;
                rbCurrent.Enabled = Flag;
                rbHistorical.Enabled = Flag;
                MultiSelectDropDown1.Enabled = Flag;
                //dtFromDate.Enabled = Flag;
                //dtToDate.Enabled = Flag;
            }
            catch (Exception ex)
            {
                statusPanel.Text = String.Empty;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SetStatusMessage(string statusMessage)
        {
            try
            {
                statusPanel.Text = statusMessage;
                datetimePanel.Text = "[" + DateTime.Now.ToString() + "]";
            }
            catch (Exception ex)
            {
                statusPanel.Text = String.Empty;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// this methos used to save grids layout, previously it was on CtrlCloseTrade control, as we have added one more control on this form
        /// so save layout should be through parent form.
        /// </summary>
        public void SaveGridsLayout()
        {
            try
            {
                UltraGrid longGrid = closeTradeWithSplitter1.GetLongGrid();
                if (longGrid != null)
                {
                    if (longGrid.DisplayLayout.Bands[0].Columns.Count > 0)
                    {
                        ClosingPrefManager.ClosingLayout.LongPositionColumns = ClosingPrefManager.GetGridColumnLayout(closeTradeWithSplitter1.GetLongGrid());
                        ClosingPrefManager.ClosingLayout.ShortPositionColumns = ClosingPrefManager.GetGridColumnLayout(closeTradeWithSplitter1.GetShortGrid());
                        ClosingPrefManager.ClosingLayout.NetPositionColumns = ClosingPrefManager.GetGridColumnLayout(closeTradeWithSplitter1.GetNetPositionsGrid());
                    }
                }

                UltraGrid closeOrderGrid = ctrlCloseTradefromAllocation1.GetCloseOrderGrid();
                if (closeOrderGrid != null)
                {
                    if (closeOrderGrid.DisplayLayout.Bands[0].Columns.Count > 0)
                    {
                        ClosingPrefManager.ClosingLayout.CloseOrderNetPositionColumns = ClosingPrefManager.GetGridColumnLayout(closeOrderGrid);
                    }
                }

                ClosingPrefManager.SaveClosingLayout();
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetUpAccessOnPermissions(bool hasAccess)
        {
            try
            {
                if (!hasAccess)
                {
                    ctrlCloseTradefromAllocation1.SetControlsAsReadOnly();
                    ctrlExpiryandSettlementNew1.SetControlsAsReadOnly();
                    closeTradeWithSplitter1.SetControlsAsReadOnly();
                    btnAdvanceOptions.Enabled = true;
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

        private void DisplayClosingCorruptedMessage(Object[] dataList)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            DisplayClosingCorruptedMessage(dataList);
                        }));
                    }
                    else
                    {
                        foreach (Object fundId in dataList)
                        {
                            if (Convert.ToString(fundId).Equals("SelectFunds"))
                            {
                                MessageBox.Show("Closing Data got corrupted. Please contact system admin.", "Closing Corruption", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else if (MultiSelectDropDown1.GetSelectedItemsInDictionary().ContainsKey(Convert.ToInt32(fundId)))
                            {
                                MessageBox.Show("Closing Data got corrupted. Please contact system admin.", "Closing Corruption", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
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

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }

        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            if (gridName == "grdOpenTrades")
                this.ctrlCloseTradefromAllocation1.ExportDataForAutomation(gridName, filePath);
            else if (gridName == "grdCreatePosition" || gridName == "grdAccountExpired" || gridName == "grdAccountUnexpired")
            {
                this.ctrlExpiryandSettlementNew1.ExportDataForAutomation(gridName, filePath);
            }
            else if (gridName == "grdNetPosition" || gridName == "grdShort" || gridName == "grdLong")
            {
                this.closeTradeWithSplitter1.ExportDataForAutomation(gridName, filePath);
            }
        }
        #endregion
    }
}