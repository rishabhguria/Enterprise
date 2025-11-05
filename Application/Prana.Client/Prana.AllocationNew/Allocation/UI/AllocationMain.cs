using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.BusinessObjects;
using System.IO;
using Prana.CommonDataCache;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Interfaces;
using Prana.Global;
using Prana.Utilities.UIUtilities;
using Prana.BusinessObjects.AppConstants;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using Prana.ClientCommon;
using System.Threading;
using Prana.AllocationNew.Allocation.UI;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Admin.BLL;
using Prana.BusinessObjects.Enums;
using Prana.Allocation.Common.Definitions;
using System.Linq;
using Prana.BusinessObjects.Constants;
using System.Globalization;
using Prana.AllocationNew.Allocation.UI.UserControls;
using Infragistics.Win.Misc;
using Prana.Allocation.Common.Enums;
using System.Threading.Tasks;
using Prana.BusinessLogic;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.Utilities.Win32Utilities;
using Prana.AllocationNew.Allocation.BusinessObjects;
using Prana.Utilities;
using System.Xml;

namespace Prana.AllocationNew
{
    public partial class AllocationMain : Form, IAllocation
    {
        #region Private Variables
        AllocationPreferences _allocationPrefs = null;
        CheckBoxOnHeader_CreationFilter _headerCheckBoxUnallocated = null;
        CheckBoxOnHeader_CreationFilter _headerCheckBoxAllocated = null;
        CompanyUser _loginUser = null;
        AllocationDefaultCollection _defaults = null;
        bool _isEventAlive = true;
        UltraGrid _selectedGrid = null;
        IAllocationCalculator _selectedUnAllocatedCtrl = null;
        private bool _formInilitisedFirstTime = true;

        List<string> _sideSortingList = new List<string>();

        List<string> _displayColumns = null;

        private bool _isSaveState;

        public bool IsSaveState
        {
            get { return _isSaveState; }
            set { _isSaveState = value; }
        }

        System.Collections.Specialized.NameValueCollection _defaultSwapParameters = new System.Collections.Specialized.NameValueCollection();

        public event EventHandler GetAuditClick;
        ProxyBase<IAllocationServices> _proxyAllocationServices = null;
        private bool _dataRetrievalInProcess = false;
        delegate void MainThreadDelegate(object sender, EventArgs e);
        //added by: Bharat raturi, 17 jun 2014
        public PranaReleaseViewType ReleaseType { get; set; }
        private Boolean _isReadPermission = false;
        private int _countSelectedRowsAllocatedGrd = 0;
        private int _countSelectedRowsUnallocatedGrd = 0;

        ProxyBase<IClosingServices> _closingServices = null;

        #endregion

        /// <summary>
        /// Get list of all columns in allocation
        /// </summary>
        public List<string> GetDisplayColumns
        {
            get { return _displayColumns ?? DisplayableColumnList(); }
        }

        /// <summary>
        /// create allocation service proxy
        /// </summary>
        private void CreateAllocationServicesProxy()
        {
            try
            {
                _proxyAllocationServices = AllocationManager.GetInstance().AllocationServices;
                _proxyAllocationServices.ConnectedEvent += _proxyAllocationServices_ConnectedEvent;
                _proxyAllocationServices.DisconnectedEvent += _proxyAllocationServices_DisconnectedEvent;

                ctrlAmendmend1.AllocationServices = _proxyAllocationServices;
                //EndpointAddress endpointAddress = new EndpointAddress(endpointAddressInString);
                //NetTcpBinding netTcpBinding = new NetTcpBinding();
                // _proxyAllocationServices = ChannelFactory<IAllocationServices>.CreateChannel(netTcpBinding, endpointAddress);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                }
            }

        /// <summary>
        /// disconnect allocation proxies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _proxyAllocationServices_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MainThreadDelegate del = _proxyAllocationServices_DisconnectedEvent;
                        this.Invoke(del, new object[] { sender, e });
                    }
                    else
                        AllocationManager.GetInstance().ClearData();
                    }
                }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                }
            }

        /// <summary>
        /// connect allocation proxies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _proxyAllocationServices_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MainThreadDelegate del = _proxyAllocationServices_ConnectedEvent;
                        this.Invoke(del, new object[] { sender, e });

                    }
                    else
                    {
                        GetAllocationData();
                        HideSwapDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                }
            }

        /// <summary>
        /// crete closing services proxy
        /// </summary>
        private void CreateClosingServicesProxy()
        {
            try
            {
                _closingServices = AllocationManager.GetInstance().ClosingServices;
                ctrlAmendmend1.ClosingServices = _closingServices;
                // EndpointAddress endpointAddress = new EndpointAddress(endpointAddressInString);

                //NetTcpBinding netTcpBinding = new NetTcpBinding();
                //_proxyClosingServices = ChannelFactory<IClosingServices>.CreateChannel(netTcpBinding, endpointAddress);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                }
            }       

        #region Initlisation

        public AllocationMain()
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    List<string> excludedCheckBoxHeaderColumns = ExcludedHeaderCheckBoxColumns();
                    _headerCheckBoxUnallocated = new CheckBoxOnHeader_CreationFilter(excludedCheckBoxHeaderColumns);
                    _headerCheckBoxAllocated = new CheckBoxOnHeader_CreationFilter(excludedCheckBoxHeaderColumns);
                    _defaults = new AllocationDefaultCollection();
                    InitializeComponent();
                    HideSwapDetails();
                    ctrlAmendmend1.GetAuditClick += new EventHandler(ctrlAmendmend1_GetAuditClick);
                    timerClear.Tick += new EventHandler(timerClear_Tick);
                    dtFromDatePickerAllocation.AfterCloseUp += new EventHandler(FromEditor_AfterCloseUp);
                    dtToDatePickerAllocation.AfterCloseUp += new EventHandler(ToEditor_AfterCloseUp);
                    //AllocationManager.GetInstance().DisableSaveButton += new EventHandler(AllocationMain_DisableSaveButton);
                    // get default swap parameters from app.config file
                    _defaultSwapParameters = ConfigurationHelper.Instance.LoadSectionBySectionName("DefaultSwapParameters");

                    //_sw = new System.Diagnostics.Stopwatch();
                    //_sw.Start();

                    //ctrlAmendmend1.allocationDataChange += new AllocationDataChangeHandler(ctrlAmendmend1_allocationDataChange);
                    //added by: Bharat Raturi, 17 jun 2014
                    //purpose: set the access permissions for the logged in user
                    SetUserPermissions();
                }

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                    throw;
                }
            finally
            {
                Logger.Write("AllocationUI Loaded", ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
            }
        }

        /// <summary>
        /// added by: Bharat raturi, 17 jun 2014
        /// Set user based permission for allocation UI
        /// </summary>
        private void SetUserPermissions()
        {
            try
            {
                ReleaseType = CachedDataManager.GetInstance.GetPranaReleaseViewType();
                if (ReleaseType == PranaReleaseViewType.CHMiddleWare)
                {
                    ModuleResources module = ModuleResources.EditTrades;

                    var isPermissionToApprove = AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, AuthAction.Approve);
                    if (!isPermissionToApprove)
                    {
                        //approveToolStripMenuItem.Enabled = false;
                        var hasAccess = AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, AuthAction.Write);
                        if (!hasAccess)
                        {
                            _isReadPermission = true;
                            btnDelete.Enabled = btnCancelData.Enabled = btnSave.Enabled = btnSaveWOState.Enabled = false;
                            ctrlAmendmend1.SetGridAccessToReadOnly();
                            ctrlAmendmend1.IsReadOnlyPermission = true;
                            //grdAllocated.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                            //grdAllocated.DisplayLayout.Bands[1].Override.AllowUpdate = DefaultableBoolean.False;
                            //grdUnallocated.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                            //grdUnallocated.DisplayLayout.Bands[1].Override.AllowUpdate = DefaultableBoolean.False;
                            //btnSave.Enabled = false;
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void ctrlAmendmend1_GetAuditClick(object sender, EventArgs e)
        {
            try
            {
                GetAuditClick(this, (LaunchFormEventArgs)e);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                }
            }

        void AllocationMain_DisableSaveButton(object sender, EventArgs e)
        {
            try
            {
            btnSave.Enabled = false;
                btnSaveWOState.Enabled = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        void ctrlAmendmend1_allocationDataChange(object sender, EventArgs<bool> e)
        {
            allocationDataChange(this, e);
        }

        void timerClear_Tick(object sender, EventArgs e)
        {
            try
            {
                lblStatusStrip.Text = string.Empty;
                timerClear.Stop();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void LoadAllocationModuleUI()
        {
            try
            {
                SetAllocationButtonColor();
                BindContextMenus();
                BindGrids();
                SetAllocationLevelControls();

                dtToDatePickerAllocation.Enabled = false;
                dtFromDatePickerAllocation.Enabled = false;
                grdUnallocated.AfterRowActivate += new System.EventHandler(this.grdUnallocated_AfterRowActivate);
                grdAllocated.AfterRowActivate += new System.EventHandler(this.grdAllocated_AfterRowActivate_1);

                //cmbbxdefaults_ValueChanged(null, null);

                BindAllocationScheme();

                // bind strategy drodown to search strategy, PRANA-8499
                BindStrategySearchDropdown();
                //this.Controls.Add(new AutoCompleteTextBox());
                //ultraToolbarsManager1.Tools[0].brint
                System.Windows.Forms.ToolTip tpGetData = new System.Windows.Forms.ToolTip();
                tpGetData.SetToolTip(btnGetAllocationData, "Shift+Enter to get data");
                AllocationManager.GetInstance().NewGroupReceived += new EventHandler(OnNewGroupReceived);
                AllocationManager.GetInstance().UpdateSymbolInfo += new EventHandler(AllocationMain_UpdateSymbolInfo);
                AllocationManager.GetInstance().UpdateGroupClosingStatusHandler += new EventHandler(AllocationMain_UpdateGroupClosingStatus);
                AllocationManager.GetInstance().AllocationPreferenceUpdated += AllocationMain_AllocationPreferenceUpdated;
                AllocationManager.GetInstance().AllocationSchemeUpdated += AllocationMain_AllocationSchemeUpdated;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// set allocation button color
        /// </summary>
        private void SetAllocationButtonColor()
        {
            try
            {
                if (CustomThemeHelper.WHITELABELTHEME.Equals("Custom House"))
                {
                    btnAllocate.Appearance.BackColor2 = Color.Gray;
                    btnAllocate.Appearance.BackColor = Color.LightGray;

                    btnAllocate.Appearance.BackColorDisabled2 = Color.LightGray;
                    btnAllocate.Appearance.BackColorDisabled = Color.LightGray;

                    btnAllocate.Appearance.ForeColorDisabled = Color.DarkGray;
                }
                else
                {
                    btnAllocate.Appearance.BackColor2 = System.Drawing.Color.FromArgb(55, 67, 85);
                    btnAllocate.Appearance.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);

                    btnAllocate.Appearance.BackColorDisabled2 = System.Drawing.Color.FromArgb(55, 67, 85);
                    btnAllocate.Appearance.BackColorDisabled = System.Drawing.Color.FromArgb(55, 67, 85);

                    btnAllocate.Appearance.ForeColorDisabled = Color.White;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                }
            }

        /// <summary>
        /// bind strategy drodown to search strategy
        /// </summary>
        private void BindStrategySearchDropdown()
        {
            try
            {
                StrategyCollection strategyCollection = CommonDataCache.CachedDataManager.GetInstance.GetUserStrategies();
                DataTable dt = new DataTable();
                dt.Columns.Add("StrategyID");
                dt.Columns.Add("StrategyName");
                DataRow dr = null;
                foreach (Prana.BusinessObjects.Strategy strategy in strategyCollection)
                {
                    dr = dt.NewRow();
                    dr["StrategyID"] = strategy.StrategyID;
                    dr["StrategyName"] = strategy.Name.Trim();
                    dt.Rows.Add(dr);
                }
                cmbStrategySearch.DataSource = null;
                cmbStrategySearch.DataSource = dt;
                cmbStrategySearch.DataBind();
                cmbStrategySearch.DisplayMember = "StrategyName";
                cmbStrategySearch.ValueMember = "StrategyID";
                cmbStrategySearch.DisplayLayout.Bands[0].Columns["StrategyID"].Hidden = true;
                cmbStrategySearch.DisplayLayout.Bands[0].Columns["StrategyName"].Header.Caption = "Strategy";
                cmbStrategySearch.DisplayLayout.Bands[0].ColHeadersVisible = false;

                if (dt.Rows.Count > 0)
                {
                    cmbStrategySearch.Value = Convert.ToInt32((dt.Rows[0][0].ToString()));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        
        /// <summary>
        /// called on value change of strategy search box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStrategySearch_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                string searchStrategy = this.cmbStrategySearch.SelectedText.ToString();
                accountStrategyMapping1.ShowSearchedStrategy(searchStrategy);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update allocation scheme in list when any new preference is saved/imported
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The AllocationSchemeUpdated event</param>
        private void AllocationMain_AllocationSchemeUpdated(object sender, EventArgs e)
        {
            try
            {
                BindAllocationScheme();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void AllocationMain_UpdateSymbolInfo(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        EventHandler updateSymbolHandler = AllocationMain_UpdateSymbolInfo;
                        this.Invoke(updateSymbolHandler, new Object[] { sender, EventArgs.Empty });
                    }
                    else
                    {
                        System.Object[] dataList = (System.Object[])sender;

                        SecMasterbaseList list = new SecMasterbaseList();
                        foreach (Object obj in dataList)
                        {
                            SecMasterBaseObj secMasterObj = (SecMasterBaseObj)obj;
                            list.Add(secMasterObj);
                        }
                        AllocationManager.GetInstance().UpdateRepositoryWithSecMasterInfo(list);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void AllocationMain_UpdateGroupClosingStatus(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        EventHandler updateGroupHandler = AllocationMain_UpdateGroupClosingStatus;
                        this.Invoke(updateGroupHandler, new Object[] { sender, EventArgs.Empty });
                    }
                    else
                    {
                        System.Object[] dataList = (System.Object[])sender;

                        List<TaxLot> taxlotsList = new List<TaxLot>();
                        foreach (Object obj in dataList)
                        {
                            TaxLot taxlot = (TaxLot)obj;
                            taxlotsList.Add(taxlot);
                        }
                        AllocationManager.GetInstance().UpdateGroupClosingStatus(taxlotsList);
                        //updating data on cost adjustment tab after closing
                        ctrlCostAdjustment.RemoveTaxlots(taxlotsList.Where(x => x.ClosingStatus == ClosingStatus.Closed).ToList());
                        ctrlCostAdjustment.UpdateTaxlots(taxlotsList.Where(x => x.ClosingStatus == ClosingStatus.Open).ToList());
                        ctrlCostAdjustment.UpdateTaxlots(taxlotsList.Where(x => x.ClosingStatus == ClosingStatus.PartiallyClosed).ToList());
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        AllocationDefaultRuleControl _allocationDefaultRuleControl = null;

        private void SetAllocationLevelControls()
        {
            try
            {
                accountStrategyMapping1.SetUp(AllocationStaticCollection.Accounts, AllocationStaticCollection.Strategies, false);
                accountOnlyUserControl1.SetUp(AllocationStaticCollection.Accounts);
                accountStrategyMapping1.CheckTotalQty += new EventHandler(accountStrategyMapping1_CheckTotalQty);
                accountStrategyMapping1.CheckTotalPercentage += new EventHandler(accountStrategyMapping1_CheckTotalPercentage);

                _allocationDefaultRuleControl = new AllocationDefaultRuleControl();
                _allocationDefaultRuleControl.AutoSize = true;
                _allocationDefaultRuleControl.Location = new System.Drawing.Point((accountOnlyUserControl1.GetMaxAccountLength() + 30), 20);
                _allocationDefaultRuleControl.ShowDateTimeCombo(true);
                accountOnlyUserControl1.ClientArea.Controls.Add(_allocationDefaultRuleControl);
                _selectedUnAllocatedCtrl = accountOnlyUserControl1;
                accountOnlyUserControl1.CheckTotalQty += new EventHandler(accountOnlyUserControl1_CheckTotalQty);
                accountOnlyUserControl1.CheckTotalPercentage += new EventHandler(accountOnlyUserControl1_CheckTotalPercentage);
                allocationCalculatorUsrControl1.SetUp(AllocationStaticCollection.Accounts);
                //allocationCalculatorUsrControl1.ClientArea.Controls.Add(_allocationDefaultRuleControl);
                allocationCalculatorUsrControl1.Visible = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void accountOnlyUserControl1_CheckTotalPercentage(object sender, EventArgs e)
        {
            try
            {
                SetViolationError();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void accountStrategyMapping1_CheckTotalPercentage(object sender, EventArgs e)
        {
            try
            {
                SetViolationError();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetViolationError()
        {
            try
            {
                //_isQtyChanged = true;
                lblStatusStrip.Text = "RoundLot rule violated. Please review";
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ClearLabelStrip()
        {
            try
            {
                lblStatusStrip.Text = string.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void accountOnlyUserControl1_CheckTotalQty(object sender, EventArgs e)
        {
            try
            {
                btnAllocate.Enabled = true;
                //_isPercentageChanged = false;
                if (_selectedGrid != null)
                {
                    List<AllocationGroup> groups = GetSelectedGroups(_selectedGrid);
                    if (groups.Count == 1)
                    {
                        btnReAllocate.Enabled = false;
                        foreach (AllocationGroup allgroup in groups)
                        {
                            bool isChanged = false;
                            AllocationOperationPreference pref = null;
                            if (cmbbxdefaults.Value != null && Convert.ToInt32(cmbbxdefaults.Value) != -1)
                            {
                                int id = (int)cmbbxdefaults.Value;
                                if (_allocationPrefCache.ContainsKey(id))
                                    pref = _allocationPrefCache[id].Clone();
                            }
                            else
                            {
                                isChanged = true;
                                pref = new AllocationOperationPreference();
                                UpdateAllocationDefaultRule(pref);
                                lock (preferenceLocker)
                                {
                                    pref.TryUpdateTargetPercentage(_targetPergentage);
                                }

                            }

                            if (pref.TargetPercentage.Count > 0 && pref.IsValid())
                            {
                                List<AllocationGroup> groupList = new List<AllocationGroup>();
                                groupList.Add(allgroup);
                                AllocationResponse response = AllocationManager.GetInstance().PreviewAllocation(groupList, pref, false, isChanged, IsForceAllocation());
                                //if (!string.IsNullOrWhiteSpace(response.Response))
                                // MessageBox.Show(this, response.Response, "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                List<AllocationGroup> group = response.GroupList;
                                //List<AllocationGroup> group = AllocationManager.GetInstance().PreviewAllocation(groupList, pref, false, isChanged);
                                //AllocationLevelList accounts = selectedUnAllocatedCtrl.GetAllocationAccounts(allgroup);
                                //allgroup.ErrorMessage = string.Empty;
                                //AllocationGroup group = _proxyAllocationServices.InnerChannel.AllocateValidatedGroups(allgroup, ref accounts, _allocationPrefs.GeneralRules.SelectedAccountID);
                                if (string.IsNullOrWhiteSpace(response.Response))
                                {
                                    _selectedUnAllocatedCtrl.SetAllocationAccounts(group[0], true);
                                    SetValidationStatus();
                                    lblStatusStrip.Text = "";
                                }
                                else
                                {
                                    lblStatusStrip.Text = response.Response;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void SetValidationStatus()
        {
            try
            {
                lblStatusStrip.Text = "Allocation updated based on roundlot. Please review";
                //_isPercentageChanged = true;
                //_isQtyChanged = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void accountStrategyMapping1_CheckTotalQty(object sender, EventArgs e)
        {
            try
            {
                //_isPercentageChanged = false;
                if (_selectedGrid != null)
                {
                    List<AllocationGroup> groups = GetSelectedGroups(_selectedGrid);
                    if (groups.Count == 1)
                    {
                        // Making allocate button for the upper control disabled when                 
                        // validation is done from lower control.
                        btnAllocate.Enabled = false;
                        foreach (AllocationGroup allgroup in groups)
                        {
                            bool isChanged = false;
                            AllocationOperationPreference pref = null;
                            if (cmbbxdefaults.Value != null && Convert.ToInt32(cmbbxdefaults.Value) != -1)
                            {
                                int id = (int)cmbbxdefaults.Value;
                                if (_allocationPrefCache.ContainsKey(id))
                                    pref = _allocationPrefCache[id].Clone();
                            }
                            else
                            {
                                isChanged = true;
                                pref = new AllocationOperationPreference();
                                UpdateAllocationDefaultRule(pref);
                                lock (_strategyPergentage)
                                {
                                    pref.TryUpdateTargetPercentage(_strategyPergentage);
                                }

                            }

                            if (pref.TargetPercentage.Count > 0 && pref.IsValid())
                            {
                                List<AllocationGroup> groupList = new List<AllocationGroup>();
                                groupList.Add(allgroup);
                                AllocationResponse response = AllocationManager.GetInstance().PreviewAllocation(groupList, pref, true, isChanged, IsForceAllocation());
                                //if (!string.IsNullOrWhiteSpace(response.Response))
                                // MessageBox.Show(this, response.Response, "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                List<AllocationGroup> group = response.GroupList;
                                //List<AllocationGroup> group = AllocationManager.GetInstance().PreviewAllocation(groupList, pref, true, isChanged);
                                //AllocationLevelList accounts = selectedUnAllocatedCtrl.GetAllocationAccounts(allgroup);
                                //allgroup.ErrorMessage = string.Empty;
                                //AllocationGroup group = _proxyAllocationServices.InnerChannel.AllocateValidatedGroups(allgroup, ref accounts, _allocationPrefs.GeneralRules.SelectedAccountID);
                                if (string.IsNullOrWhiteSpace(response.Response))
                                {
                                    accountStrategyMapping1.SetAllocationAccounts(group[0], true);
                                    SetValidationStatus();
                                    lblStatusStrip.Text = "";
                                }
                                else
                                {
                                    lblStatusStrip.Text = response.Response;
                                }
                            }
                            //AllocationLevelList accounts = accountStrategyMapping1.GetAllocationAccounts(allgroup);
                            //allgroup.ErrorMessage = string.Empty;
                            //AllocationGroup group = _proxyAllocationServices.InnerChannel.AllocateValidatedGroups(allgroup, ref accounts, _allocationPrefs.GeneralRules.SelectedAccountID);
                            //if (group.ErrorMessage.Equals(string.Empty))
                            //{
                            //    accountStrategyMapping1.SetAllocationAccounts(group, true);
                            //    SetValidationStatus();
                            //}
                            //else
                            //{
                            //    lblStatusStrip.Text = group.ErrorMessage;
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        
        private void BindContextMenus()
        {
            try
            {
                grdUnallocated.ContextMenu = contextMnuUnAllocatedGrid;
                grdAllocated.ContextMenu = contextMnuAllocatedGrid;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Bind data to allocation grids
        /// </summary>
        private void BindGrids()
        {
            try
            {
                AddCheckBoxinGrid(grdUnallocated, _headerCheckBoxUnallocated);
                _headerCheckBoxUnallocated._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBoxUnallocated__CLICKED);
                AddCheckBoxinGrid(grdAllocated, _headerCheckBoxAllocated);
                _headerCheckBoxAllocated._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBoxAllocated__CLICKED);

                BindGrid(grdUnallocated, AllocationManager.GetInstance().UnAllocatedGroups);
                BindGrid(grdAllocated, AllocationManager.GetInstance().AllocatedGroups);

                grdUnallocated.UpdateMode = UpdateMode.OnCellChange;
                grdAllocated.UpdateMode = UpdateMode.OnCellChange;
                //BandSetting();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private List<string> DisplayableColumnList()
        {
            List<string> lst = new List<string>();
            try
            {
                lst.Add("AccruedInterest");
                lst.Add("AllocatedQty");
                lst.Add("AllocationSchemeName");
                lst.Add("AssetCategory");
                lst.Add(OrderFields.PROPERTY_CLEARINGFEE);
                lst.Add(OrderFields.PROPERTY_MISCFEES);
                lst.Add("AUECID");
                lst.Add("AvgPrice");
                lst.Add("BloombergSymbol");
                lst.Add(OrderFields.PROPERTY_CLEARINGBROKERFEE);
                lst.Add("ClosingStatus");
                lst.Add("ClosingDate");
                lst.Add(OrderFields.PROPERTY_COMMISSION);
                lst.Add(COL_COMMISSIONPERSHARE);
                lst.Add(OrderFields.PROPERTY_SOFTCOMMISSIONPERSHARE);
                lst.Add(OrderFields.PROPERTY_TOTALCOMMISSIONPERSHARE);
                lst.Add("CompanyName");
                lst.Add("ContractMultiplier");
                lst.Add("CounterPartyName");
                lst.Add("CurrencyName");
                lst.Add("CusipSymbol");
                lst.Add("Delta");
                lst.Add("Description");
                lst.Add("InternalComments");
                lst.Add("ExchangeName");
                lst.Add("CumQty");
                lst.Add("ExpirationDate");
                lst.Add("GroupStatus");
                lst.Add("GroupId");
                lst.Add("IDCOSymbol");
                lst.Add("ImportFileName");
                lst.Add("IsAnotherTaxlotAttributesUpdated");
                lst.Add("IsGroupAllocatedToOneTaxlot");
                lst.Add("ISINSymbol");
                lst.Add("IsManualGroup");
                lst.Add("IsNDF");
                lst.Add("IsPreAllocated");
                lst.Add("M2MProfitLoss");
                lst.Add("NetAmountWithCommission");
                lst.Add(OrderFields.PROPERTY_OCCFEE);
                lst.Add("OrderType");
                lst.Add(OrderFields.PROPERTY_ORFFEE);
                lst.Add("OriginalPurchaseDate");
                lst.Add("OSISymbol");
                lst.Add(OrderFields.PROPERTY_OTHERBROKERFEES);
                //lst.Add("NetAmount");
                lst.Add("PranaMsgType");
                lst.Add("ProcessDate");
                lst.Add("ProxySymbol");
                lst.Add("PutOrCalls");
                lst.Add("Quantity");
                lst.Add(OrderFields.PROPERTY_SECFEE);
                lst.Add("SedolSymbol");
                lst.Add(OrderFields.PROPERTY_SETTLEMENTCURRENCYID);
                lst.Add(OrderFields.PROPERTY_SettCurrFXRate);
                lst.Add(OrderFields.PROPERTY_SettCurrFXRateCalc);
                lst.Add(OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT);
                lst.Add(OrderFields.PROPERTY_FXConversionMethodOperator);
                lst.Add(OrderFields.PROPERTY_FXRate);
                lst.Add("SettlementDate");
                lst.Add("OrderSide");
                lst.Add(OrderFields.PROPERTY_SOFTCOMMISSION);
                lst.Add(OrderFields.PROPERTY_STAMPDUTY);
                lst.Add("Symbol");
                lst.Add(OrderFields.PROPERTY_TAXONCOMMISSIONS);
                lst.Add(OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES);
                lst.Add("TradeAttribute2");
                lst.Add("TradeAttribute3");
                lst.Add("TradeAttribute4");
                lst.Add("TradeAttribute5");
                lst.Add("TradeAttribute6");
                lst.Add("TradeAttribute1");
                lst.Add("AUECLocalDate");
                lst.Add("TradingAccountName");
                lst.Add(OrderFields.PROPERTY_TRANSACTIONLEVY);
                lst.Add("TransactionType");
                lst.Add("UnAllocatedQty");
                lst.Add("UnderlyingName");
                lst.Add("UnderlyingDelta");
                lst.Add("UnderlyingSymbol");
                lst.Add("CompanyUserName");
                lst.Add("Venue");
                lst.Add(COL_TOTALCOMMISSION);
                lst.Add(COL_ClosingAlgoText);
                // Added By : Manvendra P.
                // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-8016
                lst.Add(OrderFields.PROPERTY_OptionPremiumAdjustment);
                lst.Add(OrderFields.PROPERTY_CHANGETYPE);
                lst.Add(COL_NETMONEY);
                lst.Add(COL_NETAMOUNTBASE);
                lst.Add(COL_AVGPRICEBASE); // Added Avg Price(Base) column, PRANA-10775
                //Added principal amount base and local, PRANA-11379
                lst.Add(COL_PRINCIPALAMOUNTBASE);
                lst.Add(COL_PRINCIPALAMOUNTLOCAL);
           }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return lst;
        }
        //private void SetColumnsForColumnChooser(UltraGrid grid, List<string> lst)
        //{
        //    try
        //     {
        //        foreach (UltraGridColumn column in grid.DisplayLayout.Bands[0].Columns)
        //        {                   
        //            column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        }
        //        foreach (string column in lst)
        //        {
        //            if (grid.DisplayLayout.Bands[0].Columns.Exists(column))
        //            {
        //                grid.DisplayLayout.Bands[0].Columns[column].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Set different band settings in grids
        /// </summary>
        private void BandSetting(UltraGrid grid)
        {
            try
            {
                //Created different methods to update format, activation, visibility for columns, PRANA-11652
                SetGridTaxlotsBand(grid);
                SetGridOrdersBand(grid);
                SetGridColumnsFormat(grid);
                SetGridColumnsActivation(grid);
                SetGridColumnsVisibility(grid);
                UltraWinGridUtils.SetColumnsForColumnChooser(grid, GetDisplayColumns);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// set visibility of grid columns
        /// </summary>
        /// <param name="grid"></param>
        private void SetGridColumnsVisibility(UltraGrid grid)
        {
            try
            {
                ColumnsCollection gridBandColumns = grid.DisplayLayout.Bands[0].Columns;
                if (grid.Name.Equals(AllocationConstants.AllocationGrid.grdUnallocated.ToString()))
                {
                    gridBandColumns[COL_ClosingStatus].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    gridBandColumns[COL_GroupStatus].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    gridBandColumns[COL_ClosingStatus].Hidden = true;
                    gridBandColumns[COL_ClosingAlgoText].Hidden = true;
                    gridBandColumns[COL_GroupStatus].Hidden = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// set activation for grid columns
        /// </summary>
        /// <param name="grid">The ultragrid</param>
        private static void SetGridColumnsActivation(UltraGrid grid)
        {
            try
            {
                ColumnsCollection gridBandColumns = grid.DisplayLayout.Bands[0].Columns;
                gridBandColumns[OrderFields.PROPERTY_AUECLOCALDATE].CellActivation = Activation.NoEdit;
                gridBandColumns[OrderFields.PROPERTY_SETTLEMENT_DATE].CellActivation = Activation.NoEdit;
                gridBandColumns[OrderFields.PROPERTY_ORIGINAL_PURCHASEDATE].CellActivation = Activation.NoEdit;
                gridBandColumns[OrderFields.PROPERTY_PROCESSDATE].CellActivation = Activation.NoEdit;
                gridBandColumns["ExpirationDate"].CellActivation = Activation.NoEdit;
                gridBandColumns["ClosingDate"].CellActivation = Activation.NoEdit;

                if (grid.Name.Equals(AllocationConstants.AllocationGrid.grdUnallocated.ToString()))
                {
                    gridBandColumns[COL_ClosingStatus].CellActivation = Activation.NoEdit;
                    gridBandColumns[COL_ClosingAlgoText].CellActivation = Activation.NoEdit;
                    gridBandColumns[COL_GroupStatus].CellActivation = Activation.NoEdit;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// set Orders band
        /// </summary>
        private void SetGridOrdersBand(UltraGrid grid)
        {
            try
            {
                if (grid.Name.Equals(AllocationConstants.AllocationGrid.grdAllocated.ToString()))
                {
                grdAllocated.DisplayLayout.Bands["Orders"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                grdAllocated.DisplayLayout.Bands["Orders"].Hidden = true;
                grdAllocated.DisplayLayout.Bands["Orders"].Override.RowSelectors = DefaultableBoolean.False;
                }
                else if (grid.Name.Equals(AllocationConstants.AllocationGrid.grdUnallocated.ToString()))
                {
                    grdUnallocated.DisplayLayout.Bands["Orders"].Override.RowSelectors = DefaultableBoolean.False;
                    grdUnallocated.DisplayLayout.Bands["Orders"].ExcludeFromColumnChooser =
                        ExcludeFromColumnChooser.True;

                    ColumnsCollection orderColumns = grdUnallocated.DisplayLayout.Bands["Orders"].Columns;
                    foreach (UltraGridColumn column in orderColumns)
                {
                    column.Hidden = true;
                }
                    orderColumns[OrderFields.PROPERTY_CANCEL_ORDER_ID].Hidden = false;
                    orderColumns[OrderFields.PROPERTY_AVGPRICE].Hidden = false;
                    orderColumns[OrderFields.PROPERTY_EXECUTED_QTY].Hidden = false;
                    orderColumns[OrderFields.PROPERTY_Prana_MSG_TYPE].Hidden = false;
                    orderColumns[OrderFields.PROPERTY_MultiTradeName].Hidden = false;

                    orderColumns[OrderFields.PROPERTY_Prana_MSG_TYPE].Header.Caption = "Source";
                    orderColumns[OrderFields.PROPERTY_MultiTradeName].Header.Caption =
                        OrderFields.CAPTION_MultiTradeName;

                    orderColumns[OrderFields.PROPERTY_EXECUTED_QTY].Format = ApplicationConstants.FORMAT_QTY;
                    orderColumns[OrderFields.PROPERTY_AVGPRICE].Format = ApplicationConstants.FORMAT_AVGPRICE;
                }
            }
                catch
                (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// set taxlots band
        /// </summary>
        private void SetGridTaxlotsBand(UltraGrid grid)
        {

            try
            {
                if (grid.Name.Equals(AllocationConstants.AllocationGrid.grdUnallocated.ToString()))
                {
                    grdUnallocated.DisplayLayout.Bands["TaxLots"].ExcludeFromColumnChooser =
                        ExcludeFromColumnChooser.True;
                }
                else if (grid.Name.Equals(AllocationConstants.AllocationGrid.grdAllocated.ToString()))
                {
                    grdAllocated.DisplayLayout.Bands["TaxLots"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    grdAllocated.DisplayLayout.Bands["TaxLots"].Override.RowSelectors = DefaultableBoolean.False;

                    ColumnsCollection taxLotColumns = grdAllocated.DisplayLayout.Bands["TaxLots"].Columns;
                if (!taxLotColumns.Exists(COL_NETAMOUNTWITHCOMMISSION))
                    taxLotColumns.Add(COL_NETAMOUNTWITHCOMMISSION, CAP_NETAMOUNTWITHCOMMISSION);

                // added net money column at texlot level,PRANA-10310
                if (!taxLotColumns.Exists(COL_NETMONEY))                        
                    taxLotColumns.Add(COL_NETMONEY, CAPTION_NETMONEY_SETTLEMENT);

                if (!taxLotColumns.Exists(COL_NETAMOUNTBASE))
                    taxLotColumns.Add(COL_NETAMOUNTBASE, CAPTION_NETAMOUNT_BASE);
                
                // Added Avg Price(Base) column at taxlot levels, PRANA-10775
                if (!taxLotColumns.Exists(COL_AVGPRICEBASE))
                    taxLotColumns.Add(COL_AVGPRICEBASE, CAPTION_AVGPRICE_BASE);

                //Added principal amount base and local, PRANA-11379
                if (!taxLotColumns.Exists(COL_PRINCIPALAMOUNTBASE))
                    taxLotColumns.Add(COL_PRINCIPALAMOUNTBASE, CAPTION_PRINCIPALAMOUNTBASE);

                if (!taxLotColumns.Exists(COL_PRINCIPALAMOUNTLOCAL))
                        taxLotColumns.Add(COL_PRINCIPALAMOUNTLOCAL, CAPTION_PRINCIPALAMOUNTLOCAL);

                    foreach (UltraGridColumn column in taxLotColumns)
                {
                        column.Hidden = true;
                }
                taxLotColumns["GroupID"].Hidden = true;
                    taxLotColumns["TaxLotID"].Hidden = false;
                    taxLotColumns["TaxLotQty"].Hidden = false;
                    taxLotColumns["Percentage"].Hidden = false;
                taxLotColumns[OrderFields.PROPERTY_Level1Name].Hidden = false;
                taxLotColumns[OrderFields.PROPERTY_Level2Name].Hidden = false;
                taxLotColumns[OrderFields.PROPERTY_COMMISSION].Hidden = false;
                taxLotColumns[OrderFields.PROPERTY_SOFTCOMMISSION].Hidden = false;
                taxLotColumns[COL_ClosingStatus].Hidden = false;
                    taxLotColumns[COL_ClosingAlgo].Hidden = false;
                    taxLotColumns[COL_NETAMOUNTWITHCOMMISSION].Hidden = false;
                    taxLotColumns[COL_NETMONEY].Hidden = false;
                    taxLotColumns[COL_NETAMOUNTBASE].Hidden = false;
                    taxLotColumns[COL_AVGPRICEBASE].Hidden = false;
                    taxLotColumns[COL_PRINCIPALAMOUNTBASE].Hidden = false;
                    taxLotColumns[COL_PRINCIPALAMOUNTLOCAL].Hidden = false;

                    taxLotColumns[OrderFields.PROPERTY_Level2Name].Header.Caption = OrderFields.CAPTION_Level2Name;
                    taxLotColumns[OrderFields.PROPERTY_Level1Name].Header.Caption = OrderFields.CAPTION_Level1Name;
                    taxLotColumns[OrderFields.PROPERTY_SOFTCOMMISSION].Header.Caption =
                        OrderFields.CAPTION_SOFTCOMMISSION;
                taxLotColumns[COL_ClosingStatus].Header.Caption = CAP_CLOSINGSTATUS;
                taxLotColumns[COL_ClosingAlgo].Header.Caption = CAP_CLOSINGALGO;

                    taxLotColumns["TaxLotQty"].Format = ApplicationConstants.FORMAT_QTY;
                    taxLotColumns[OrderFields.PROPERTY_COMMISSION].Format = ApplicationConstants.FORMAT_COSTBASIS;
                    taxLotColumns[OrderFields.PROPERTY_SOFTCOMMISSION].Format = ApplicationConstants.FORMAT_COSTBASIS;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// set grid columns format
        /// </summary>
        /// <param name="grid"></param>
        private static void SetGridColumnsFormat(UltraGrid grid)
        {
            try
                {
                ColumnsCollection gridBandColumns = grid.DisplayLayout.Bands[0].Columns;
                gridBandColumns[OrderFields.PROPERTY_EXECUTED_QTY].Format = ApplicationConstants.FORMAT_QTY;
                gridBandColumns["AllocatedQty"].Format = ApplicationConstants.FORMAT_QTY;
                gridBandColumns["UnAllocatedQty"].Format = ApplicationConstants.FORMAT_QTY;
                gridBandColumns[COL_NETAMOUNT].Format = ApplicationConstants.FORMAT_QTY;

                gridBandColumns[COL_NETMONEY].Format = ApplicationConstants.FORMAT_QTY;
                gridBandColumns[COL_PRINCIPALAMOUNTBASE].Format = ApplicationConstants.FORMAT_QTY;
                gridBandColumns[COL_PRINCIPALAMOUNTLOCAL].Format = ApplicationConstants.FORMAT_QTY;

                gridBandColumns[OrderFields.PROPERTY_COMMISSION].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_SOFTCOMMISSION].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_OTHERBROKERFEES].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_CLEARINGBROKERFEE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[COL_TOTALCOMMISSION].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_STAMPDUTY].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_TRANSACTIONLEVY].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_CLEARINGFEE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_TAXONCOMMISSIONS].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_MISCFEES].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_SECFEE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_OCCFEE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_ORFFEE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                gridBandColumns[OrderFields.PROPERTY_QUANTITY].Format = ApplicationConstants.FORMAT_QTY;
                gridBandColumns[OrderFields.PROPERTY_AVGPRICE].Format = ApplicationConstants.FORMAT_AVGPRICE;
                gridBandColumns[COL_NETAMOUNTBASE].Format = ApplicationConstants.FORMAT_QTY;
                gridBandColumns[COL_AVGPRICEBASE].Format = ApplicationConstants.FORMAT_QTY;
                gridBandColumns["AccruedInterest"].Format = ApplicationConstants.FORMAT_COSTBASIS;

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                }
            }

        /// <summary>
        /// add change type column and bind valuelist
        /// </summary>
        /// <param name="band">Column band</param>
        private static void AddChangeTypeAndBindEnum(ColumnsCollection band)
        {
            try
            {
                ValueList ChangeTypeList = new ValueList();
                List<EnumerationValue> ChangeTypeEnumList = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.ChangeType));
                foreach (EnumerationValue var in ChangeTypeEnumList)
                {
                    ChangeTypeList.ValueListItems.Add(var.Value, var.DisplayText);
                }
                if (band.Exists(OrderFields.PROPERTY_CHANGETYPE))
                {
                    band[OrderFields.PROPERTY_CHANGETYPE].Header.Caption = OrderFields.CAPTION_CHANGETYPE;
                    band[OrderFields.PROPERTY_CHANGETYPE].Hidden = false;
                    band[OrderFields.PROPERTY_CHANGETYPE].ValueList = ChangeTypeList;
                    band[OrderFields.PROPERTY_CHANGETYPE].CellActivation = Activation.NoEdit;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                }
            }

        private static void AddSettlementCurrencyColumnsAndBindEnum(ColumnsCollection band)
        {
            try
            {
                //CHMW-3315	[Allocation] - Add FXRate and FXConversionMethodOperator column on Allocation UI
                band[OrderFields.PROPERTY_SettCurrFXRate].Header.Caption = OrderFields.CAPTION_SettCurrFXRate;
                //band[OrderFields.PROPERTY_SettCurrFXRate].Hidden = false;     //As per discussion with Narendra sir, Commenting this line
                band[OrderFields.PROPERTY_FXRate].Header.Caption = OrderFields.CAPTION_FXRate;
                //band[OrderFields.PROPERTY_FXRate].Hidden = false;     //As per discussion with Narendra sir, Commenting this line



                ValueList fxConversionMethodOperatorList = new ValueList();
                ValueList SettFXConversionMethodOperatorList = new ValueList();
                List<EnumerationValue> fxConversionMethodOperator = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.Operator));
                foreach (EnumerationValue var in fxConversionMethodOperator)
                {
                    if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                    {
                        fxConversionMethodOperatorList.ValueListItems.Add(var.Value, var.DisplayText);
                        SettFXConversionMethodOperatorList.ValueListItems.Add(var.Value, var.DisplayText);
                    }
                }

                band[OrderFields.PROPERTY_SettCurrFXRateCalc].Header.Caption = OrderFields.CAPTION_SettCurrFXRateCalc;
                //band[OrderFields.PROPERTY_SettCurrFXRateCalc].Hidden = false;     //As per discussion with Narendra sir, Commenting this line
                band[OrderFields.PROPERTY_SettCurrFXRateCalc].ValueList = SettFXConversionMethodOperatorList;
                band[OrderFields.PROPERTY_SettCurrFXRateCalc].CellActivation = Activation.NoEdit;

                band[OrderFields.PROPERTY_FXConversionMethodOperator].Header.Caption = OrderFields.CAPTION_FXConversionMethodOperator;
                //band[OrderFields.PROPERTY_FXConversionMethodOperator].Hidden = false;     //As per discussion with Narendra sir, Commenting this line
                band[OrderFields.PROPERTY_FXConversionMethodOperator].ValueList = fxConversionMethodOperatorList;
                band[OrderFields.PROPERTY_FXConversionMethodOperator].CellActivation = Activation.NoEdit;

                band[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Header.Caption = OrderFields.CAPTION_SETTLEMENTCURRENCY;
                //band[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Hidden = false;       //As per discussion with Narendra sir, Commenting this line
                Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                ValueList currencies = new ValueList();
                foreach (KeyValuePair<int, string> item in dictCurrencies)
                {
                    currencies.ValueListItems.Add(item.Key, item.Value);
                }
                currencies.ValueListItems.Add(0, ApplicationConstants.C_COMBO_NONE);
                band[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].ValueList = currencies;
                band[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].CellActivation = Activation.NoEdit;

                band[OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT].Header.Caption = OrderFields.CAPTION_SETTLEMENTCURRENCYAMOUNT;
                //band[OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT].Hidden = false;       //As per discussion with Narendra sir, Commenting this line
                band[OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT].Format = ApplicationConstants.FORMAT_COSTBASIS;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AddCheckBoxinGrid(UltraGrid grid, CheckBoxOnHeader_CreationFilter headerCheckBox)
        {
            try
            {
                grid.CreationFilter = headerCheckBox;
                grid.DisplayLayout.Bands[0].Columns.Add("checkBox", "");
                grid.DisplayLayout.Bands[0].Columns["checkBox"].DataType = typeof(bool);
                grid.DisplayLayout.Bands[0].Columns["checkBox"].CellClickAction = CellClickAction.CellSelect;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.Caption = String.Empty;
                SetCheckBoxAtFirstPosition(grid);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void SetCheckBoxAtFirstPosition(UltraGrid grid)
        {
            try
            {
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Hidden = false;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.VisiblePosition = 0;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Width = 10;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Privtae UI Functions

        //Never Used--
        [Obsolete]
        private DateTime GetSelectedUTCDate()
        {
            if (rbHistorical.CheckedIndex == -1)
            {
                return DateTime.UtcNow;
            }
            else
            {
                return ((DateTime)dtToDatePickerAllocation.Value).ToUniversalTime();
            }
        }

        private void GetAllocationData()
        {
            try
            {
                if (!_formInilitisedFirstTime)
                {
                    DialogResult userChoice = PromptForDataSaving(AllocationConstants.ActionAfterSavingData.GetData);
                    if (userChoice != DialogResult.Yes)
                    {
                        GetAllocationDataWithoutPrompt();
                    }
                }
                else
                {
                    GetAllocationDataWithoutPrompt();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Check for changes in Cost Adjustment tab, pop up message box will display to know user response
        /// </summary>
        /// <param name="actionAfterSaving">action after saving changes, CloseAllocation in case user is saving changes on close click</param>
        /// <returns>user input Yes/No</returns>
        private DialogResult CostAdjustmentSaveDataPrompt(string actionAfterSaving)
        {
            try
            {
                DialogResult currentUserChoice = MessageBox.Show(this, "Would you like to save Cost Adjustment changes?", "Nirvana Cost Adjustment", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Disable UI elements before starting save for cost adjustment
                timerProgress.Start();//Starting timer for progress label.
                ClearLabelStrip();
                ToggleUIElementsWithMessage("Saving Data. Please wait", false);

                if (currentUserChoice == DialogResult.Yes)
                {
                    ctrlCostAdjustment.SaveCostAdjustment(actionAfterSaving);
                }
                else if (currentUserChoice == DialogResult.No)
                    ctrlCostAdjustment.ResetCostAdjustmentGrid();

                return currentUserChoice;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return DialogResult.No;
            }
        }

        /// <summary>
        /// Do not call this function directly use GetAllocationData instead. The function should be called only when
        /// we need to get data without saving prompt
        /// </summary>
        private void GetAllocationDataWithoutPrompt()
        {
            try
            {
                if (!_dataRetrievalInProcess) // don't retrieve data when in process of retrieving
                {
                    string ToAllAUECDatesString = string.Empty;
                    string FromAllAUECDatesString = string.Empty;
                    string fromAllocatedAllAUECDatesString = string.Empty;

                    if (rbHistorical.CheckedIndex == -1)
                    {
                        //ToAllAUECDatesString = TimeZoneHelper.GetAllAUECDateInUseAUECStr(DateTime.UtcNow);
                        //FromAllAUECDatesString = TimeZoneHelper.GetAllAUECDateInUseAUECStr(DateTime.UtcNow);
                        ToAllAUECDatesString = DateTime.UtcNow.Date.ToString();
                        DateTime fromDate = DateTime.UtcNow.Date.AddDays(-1 * Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_NoOfDaysAsCurrentForAllocation)));
                        FromAllAUECDatesString = fromDate.AddDays(1).ToString();
                        fromAllocatedAllAUECDatesString = DateTime.UtcNow.Date.ToString();
                    }
                    else
                    {
                        //ToAllAUECDatesString = TimeZoneHelper.GetSameDateInUseAUECStr(dtToDatePickerAllocation.DateTime);
                        //FromAllAUECDatesString = TimeZoneHelper.GetSameDateInUseAUECStr(dtFromDatePickerAllocation.DateTime);
                        ToAllAUECDatesString = ((DateTime)dtToDatePickerAllocation.Value).ToString();
                        FromAllAUECDatesString = ((DateTime)dtFromDatePickerAllocation.Value).ToString();
                        fromAllocatedAllAUECDatesString = ((DateTime)dtFromDatePickerAllocation.Value).ToString();
                    }

                    GetDataAsync(ToAllAUECDatesString, FromAllAUECDatesString, fromAllocatedAllAUECDatesString);
                    //AllocationManager.GetInstance().ClearData();
                    //BackgroundWorker fetchDataAsyc = new BackgroundWorker();
                    //fetchDataAsyc.RunWorkerCompleted += new RunWorkerCompletedEventHandler(fetchDataAsyc_RunWorkerCompleted);
                    //fetchDataAsyc.DoWork += new DoWorkEventHandler(fetchDataAsyc_DoWork);

                    //fetchDataAsyc.RunWorkerAsync(new object[] { ToAllAUECDatesString, FromAllAUECDatesString });
                    //PostTradeCacheManager.FillData(ToAllAUECDatesString, FromAllAUECDatesString);
                }

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                Logger.Write("Groups Loaded in Module", ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
                ChangeButtonStatus(false);
            }

        }

        void fetchDataAsyc_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] data = (object[])e.Argument;

                string ToAllAUECDatesString = data[0].ToString();
                string FromAllAUECDatesString = data[1].ToString();
                Dictionary<String, Dictionary<String, String>> _filterList = data[2] as Dictionary<String, Dictionary<String, String>>;
                List<AllocationGroup> groups;

                #region AccountPermissioningonFiltersForCH
                if (CachedDataManager.GetInstance.GetPranaReleaseViewType().Equals(PranaReleaseViewType.CHMiddleWare))
                {
                    bool isValidToProcess = GetPermissibleFiltersForCH(_filterList);
                    if (!isValidToProcess)
                    {
                        e.Cancel = true;
                        //return;
                    }
                }
                #endregion


                //if filter is null or it contains nothing getgroups will be called without it, otherwise it will be called with it.
                if (_filterList != null && _filterList.Count > 0)
                    groups = AllocationManager.GetInstance().GetGroups(ToAllAUECDatesString, FromAllAUECDatesString, _filterList, true, _loginUser.CompanyUserID);
                else
                    groups = AllocationManager.GetInstance().GetGroups(ToAllAUECDatesString, FromAllAUECDatesString, null, true, _loginUser.CompanyUserID);

                e.Result = groups;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                Logger.Write("Groups Loaded in Module", ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
            }
        }

        void fetchDataAsyc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // check if Allocation UI is disposed, then do nothing, PRANA-10032
                if (this.IsDisposed || this.Disposing)
                    return;
                if ((e.Cancelled == true))
                {
                    MessageBox.Show("Cancelled!", "Allocation", MessageBoxButtons.OK);
                }

                else if (!(e.Error == null))
                {
                    MessageBox.Show("Error: " + e.Error.Message, "Allocation", MessageBoxButtons.OK);
                }

                else
                {
                    List<AllocationGroup> groups = e.Result as List<AllocationGroup>;
                    if (groups == null)
                    {
                        lblStatusStrip.Text = "Nothing to load.";
                    }
                    else
                    {
                        if (groups.Count == 0)
                        {
                            // If Allocated grid contains nothing then update cost adjustment grid
                            // BindDataToCostAdjustmentUI();
                            lblStatusStrip.Text = "Nothing to load.";
                        }
                        else
                        {
                            AllocationManager.GetInstance().AddGroupsToUI(groups);
                            //BindDataToCostAdjustmentUI();
                            //lblStatusStrip.Text = " Total " + groups.Count + " trade(s)/group(s) loaded.";
                            lblStatusStrip.Text = "Trade(s)/group(s) loaded.";
                        }
                    }
                    //Binding data to cost adjustment grid
                    BindDataToCostAdjustmentUI();
                    ClearUnAllocatedAccountNumberControl(string.Empty);
                    ClearAllocatedAccountNumberControl(string.Empty);

                    // Clears data on Edit-Trade Group Control
                    ctrlAmendmend1.ClearAmendSingleGroupControl();
                    ctrlAmendmend1.updateAttriblists();
                }
                //Updating Trade Counter
                UpdateTotalNoOfTradesForAllocatedgrd(0);
                UpdateTotalNoOfTradesForUnallocatedgrd(0);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                RestoreStateOfControls();
                int tabIndex = tabAllocation.ActiveTab.Index;
                // If TabIndex is equal to 2 that is Cost Adjustment and 1 that is Edit Trade Tab then AutoGroup, CheckSide, Closing, Header Fill button enable false
                if (tabIndex == 1 || tabIndex == 2)
                {
                    ugbxHeaderFill.Enabled = false;
                    btnAutoGrp.Enabled = false;
                    btnCheckSide.Visible = false;
                    btnClosing.Visible = false;
                }
                //ToggleUIElementsWithMessage(String.Empty, true);
                //if (allocationDataChange != null)
                //{
                //    allocationDataChange(this, false);
                //}
                ////ChangeButtonStatus(true);


                ////This enables scrollbar of grids
                //if (grdUnallocated.DisplayLayout!=null)
                //{
                //    grdUnallocated.DisplayLayout.Scrollbars = Scrollbars.Automatic; 
                //}
                //if (grdAllocated.DisplayLayout!=null)
                //{
                //    grdAllocated.DisplayLayout.Scrollbars = Scrollbars.Automatic; 
                //}
                //btnGetAllocationData.Text = "Get Data";
                //_dataRetrievalInProcess = false;
                //if (timerProgress.Enabled)
                //    timerProgress.Stop();
                //lblStatusStripProgress.Text = string.Empty;
            }
        }
        /// <summary>
        /// restores the status of control when there is an error or some background work is completed
        /// </summary>
        public void RestoreStateOfControls()
        {
            try
            {
                // check if Allocation UI is disposed, then do nothing, PRANA-10032
                if (this.IsDisposed || this.Disposing)
                    return;
                ToggleUIElementsWithMessage(String.Empty, true);
                if (allocationDataChange != null)
                {
                    allocationDataChange(this, new EventArgs<bool>(false));
                }
                //ChangeButtonStatus(true);


                //This enables scrollbar of grids
                if (grdUnallocated.DisplayLayout != null)
                {
                    grdUnallocated.DisplayLayout.Scrollbars = Scrollbars.Automatic;
                }
                if (grdAllocated.DisplayLayout != null)
                {
                    grdAllocated.DisplayLayout.Scrollbars = Scrollbars.Automatic;
                }
                btnGetAllocationData.Text = "Get Data";
                _dataRetrievalInProcess = false;
                if (timerProgress.Enabled)
                    timerProgress.Stop();
                lblStatusStripProgress.Text = string.Empty;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void ChangeButtonStatus(bool dataarrived)
        {
            try
            {
                if (dataarrived)
                {
                    btnAutoGrp.Enabled = true;//Enabling and disabling auogroup button too because grid doesnot contain data to be grouped
                    btnGetAllocationData.Text = "GetData";
                    btnGetAllocationData.Enabled = true;
                    //modified by: Bharat Raturi, 18 jun 2014
                    if (!_isReadPermission)
                    {
                        btnSave.Enabled = true;
                        btnSaveWOState.Enabled = true;
                    }
                    //this.Cursor=Cursor.Current.
                }
                else
                {
                    btnAutoGrp.Enabled = false;//Enabling and disabling auogroup button too because grid doesnot contain data to be grouped
                    btnGetAllocationData.Text = "Getting Data...";
                    btnGetAllocationData.Enabled = false;
                    btnSave.Enabled = false;
                    btnSaveWOState.Enabled = false;
                }
                //btnSave.Enabled = true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindGrid(UltraGrid grid, GenericBindingList<AllocationGroup> data)
        {
            try
            {
                grid.DataSource = data;
                //grid.DataBind();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Modified to set only columns in grid, PRANA-11586
        /// </summary>
        /// <param name="grid">Grid</param>
        private void ShowColumns(UltraGrid grid)
        {
            try
            {
                //List<string> band0Column = new List<string>();
                //band0Column = columns;
                //List<List<string>> bandsColumns = new List<List<string>>();
                //bandsColumns.Add(band0Column);
                //Prana.Utilities.UIUtilities.UltraWinGridUtils.DisplayColumns(bandsColumns, grid, coulmnWidths);
                
                SetGridColumnsHeaders(grid);
                SetCheckBoxAtFirstPosition(grid);
                BandSetting(grid);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CheckBoxStatus(UltraGridRow row)
        {
            if (row != null)
            {
                row.Cells["checkBox"].Value = true;
                row.Refresh(RefreshRow.FireInitializeRow);
            }
        }

        private void btnShowCommision_Click(object sender, EventArgs e)
        {
            try
            {
                CommissionForm commisionForm = new CommissionForm();
                commisionForm.SetUp(_loginUser, true);
                commisionForm.Show();
                //if (PostTradeCacheManager.CommissionCalculationTime)
                commisionForm.Text = "Commission Calculation : Pre Allocation";
                //if (_allocationServices.CommissionCalculationTime)
                //{
                //    commisionForm.Text = "Commission Calculation : Post Allocation";
                //}
                //else
                //{
                //    commisionForm.Text = "Commission Calculation : Pre Allocation";
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private List<String> GetSelectedGroupIds(UltraGrid grid)
        {
            List<String> allocationGroups = new List<String>();
            try
            {
                UltraGridRow[] rows = grid.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells["checkBox"].Text == true.ToString())
                    {
                        allocationGroups.Add(row.Cells["GroupID"].Text);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return allocationGroups;
        }

        private List<AllocationGroup> GetSelectedGroups(UltraGrid grid)
        {
            List<AllocationGroup> allocationGroups = new List<AllocationGroup>();
            try
            {
                UltraGridRow[] rows = grid.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells["checkBox"].Text == true.ToString())
                    {
                        allocationGroups.Add((AllocationGroup)row.ListObject);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return allocationGroups;
        }



        #endregion

        #region Allocation account Display
        private void grdAllocated_AfterRowActivate_1(object sender, EventArgs e)
        {
            try
            {
                if (_allocationPrefs.GeneralRules.ClearAllocationAccountControlNumer)
                {
                    if (_isAllocatedRowSelected)
                    {
                        SetAccountPercentageNumbersForAllocatedTrades();
                        _isAllocatedRowSelected = false;
                    }
                }
                else
                {
                    SetAccountPercentageNumbersForAllocatedTrades();
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetAccountPercentageNumbersForAllocatedTrades()
        {
            try
            {
                if (grdAllocated.ActiveRow != null)
                {
                    if (grdAllocated.ActiveRow.Band.Index == 0)
                    {
                        AllocationGroup group = (AllocationGroup)grdAllocated.ActiveRow.ListObject;
                        accountStrategyMapping1.SetQuantity(Convert.ToDecimal(group.CumQty));
                        //Updating Total No of Trade Field
                        //  UpdateTotalNoOfTradesForAllocatedgrd();
                        // accountStrategyMapping1.SetAllocatiaccountonAccounts(group, true);
                        accountStrategyMapping1.SetAllocationAccounts(group);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool IsMultipleSelected(UltraGrid grid)
        {
            bool isMultipleSelected = false;
            try
            {
                UltraGridRow[] rows = grid.Rows.GetFilteredInNonGroupByRows();
                int countSelectedRows = Convert.ToInt32(rows.Count(row => row.Cells["checkBox"].Text == true.ToString()));

                if (countSelectedRows > 1)
                    isMultipleSelected = true;
                else
                    isMultipleSelected = false;

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isMultipleSelected;
        }

        private decimal CalculateTotalPercentage(AllocationLevelList accounts)
        {
            decimal totalPercentage = 0;
            try
            {
                foreach (AllocationLevelClass account in accounts.Collection)
                {
                    totalPercentage += Convert.ToDecimal(account.Percentage);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return totalPercentage;
        }

        /// <summary>
        /// Check ForceAllocation checked state
        /// </summary>
        /// <returns></returns>
        public bool IsForceAllocation()
        {
            try
            {
                if (chkboxForceAllocation.CheckState.Equals(CheckState.Checked))
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        private void grdUnallocated_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                //One more condition ( grdUnallocated.ActiveRow.ListObject != null ) is added to avoid excepetion
                if (grdUnallocated.ActiveRow != null && grdUnallocated.ActiveRow.ListObject != null && grdUnallocated.ActiveRow.Band.Index == 0 && _isRowClicked)
                {
                    //Added to disable Account control if symbol methodolgy is selected, PRANA-11473
                    accountOnlyUserControl1.Enabled = (rbtnAllocationBySymbol.CheckedIndex == 0) ? false : true;

                    AllocationGroup group = (AllocationGroup)grdUnallocated.ActiveRow.ListObject;
                    _selectedUnAllocatedCtrl.SetQuantity(Convert.ToDecimal(group.CumQty));
                    accountStrategyMapping1.SetQuantity(Convert.ToDecimal(group.CumQty));
                    // AllocationLevelList accounts = selectedUnAllocatedCtrl.GetAllocationAccounts(group);

                    //Updating Total No of Trade Field
                    //UpdateTotalNoOfTradesForUnallocatedgrd(false);
                    bool isChanged = false;
                    AllocationOperationPreference pref = null;
                    if (cmbbxdefaults.Value != null && Convert.ToInt32(cmbbxdefaults.Value) != -1)
                    {
                        int id = (int)cmbbxdefaults.Value;
                        if (_allocationPrefCache.ContainsKey(id))
                            pref = _allocationPrefCache[id].Clone();
                    }
                    else
                    {
                        isChanged = true;
                        pref = new AllocationOperationPreference();
                        AllocationRule rule = null;
                        if(_allocationDefaultRuleControl.Visible)
                            rule = _allocationDefaultRuleControl.GetCurrentValues();
                        else
                            rule = AllocationManager.GetInstance().Allocation.InnerChannel.GetDefaultRule(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID).DefaultRule;
                        lock (preferenceLocker)
                        {
                            if (_targetPergentage.Count > 0)
                                pref.TryUpdateTargetPercentage(_targetPergentage);
                            else
                                pref.TryUpdateTargetPercentage(_selectedUnAllocatedCtrl.GetAllocationAccountValue());
                        }

                        //if calculator is checked and no percentage is defined getting prorata state percentage for allocation
                        //as prorata and calculator are some what same.
                        if (chkbxAllocationCalculator.Checked && pref.TargetPercentage.Count == 0)
                        {
                            rule.RuleType = MatchingRuleType.Prorata;
                            rule.ProrataAccountList = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict().Select(x => x.Key).ToList();
                            rule.ProrataDaysBack = 0;
                        }
                        pref.TryUpdateDefaultRule(rule);

                    }

                    if ((pref.DefaultRule.RuleType == MatchingRuleType.Prorata || pref.TargetPercentage.Count > 0) && pref.IsValid())
                    {
                        // decimal totalPercentage = Math.Round(CalculateTotalPercentage(accounts), 0);

                        //if (totalPercentage.Equals(100))
                        //{
                        //string result = _proxyAllocationServices.InnerChannel.ValidateAllocationAccounts(group, ref accounts);
                        group.ErrorMessage = string.Empty;

                        //AllocationGroup allGroup = _proxyAllocationServices.InnerChannel.AllocateValidatedGroups(group, ref accounts, _allocationPrefs.GeneralRules.SelectedAccountID);
                        List<AllocationGroup> groupList = new List<AllocationGroup>();
                        groupList.Add(group);
                        AllocationResponse response = AllocationManager.GetInstance().PreviewAllocation(groupList, pref, false, isChanged, IsForceAllocation());
                        if (!string.IsNullOrWhiteSpace(response.Response))
                        {
                            lblStatusStrip.Text = response.Response.Contains("\n") ? response.Response.Substring(0, response.Response.IndexOf("\n") - 1) : response.Response;
                            // MessageBox.Show(this, response.Response, "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                            lblStatusStrip.Text = "Data allocated by account.";
                        List<AllocationGroup> allGroup = response.GroupList;
                        if (allGroup != null && allGroup.Count > 0)
                        {
                            if (allGroup[0].ErrorMessage.Equals(string.Empty))
                            {
                                //selectedUnAllocatedCtrl.SetAllocationAccounts(allGroup[0], true);
                                _selectedUnAllocatedCtrl.SetAllocationAccounts(allGroup[0]);
                                // accountStrategyMapping1.SetAllocationAccounts(group, true);
                                accountStrategyMapping1.SetAllocationAccounts(group);
                            }
                        }
                        else
                        {
                            _selectedUnAllocatedCtrl.SetAllocationDefault(pref);
                            accountStrategyMapping1.SetAllocationAccounts(group);
                            //selectedUnAllocatedCtrl.SetAllocationAccounts(group);
                            //accountStrategyMapping1.SetAllocationAccounts(group, false);
                            //selectedUnAllocatedCtrl.SetAllocationAccounts(group, false);
                        }
                        // }
                    }
                    else
                    {
                        lblStatusStrip.Text = "Sum of Percentage is not 100!";
                        _selectedUnAllocatedCtrl.SetAllocationDefault(pref);
                        //accountStrategyMapping1.SetAllocationAccounts(group);
                        //selectedUnAllocatedCtrl.SetAllocationAccounts(group);
                        //accountStrategyMapping1.SetAllocationAccounts(group, false);
                        //selectedUnAllocatedCtrl.SetAllocationAccounts(group, false);

                    }
                    btnAllocate.Enabled = true;
                    _isRowClicked = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Allocation ,Unallocation and grouping

        private void mnuGroup_Click(object sender, EventArgs e)
        {
            try
            {
                List<AllocationGroup> groups = GetSelectedGroups(grdUnallocated);

                // TODO : Here IsGroupingRulePassed would be called twice. To avoid the major changes in the core grouping logic, we have left to centralize
                // the IsGroupingRulePassed calling. Ideally it should be called within the ApplyGrouping. The difference is that mnuGrouping
                // don't allow any grouping at all if any any of the trade can't be grouped while AutoGroup allow even if there are differnt groups to make.

                if (AllocationRules.IsGroupingRulePassed(groups, _allocationPrefs))
                {
                    GenericBindingList<AllocationGroup> genBindList = new GenericBindingList<AllocationGroup>();
                    genBindList.AddList(groups);
                    groups.Clear();
                    ApplyGrouping(genBindList);
                }
                else
                {
                    lblStatusStrip.Text = "Orders of different types can't be grouped.";
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// popups the audit trail ui for the selected groups routed through nirvana main 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuTradeAuditTrailUnallocated_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> groupIds = GetSelectedGroupIds(grdUnallocated);
                if (groupIds.Count > 0)
                {
                    GetAuditClick(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                }
                else
                {
                    if (grdUnallocated.ActiveRow != null)
                    {
                        groupIds.Add(grdUnallocated.ActiveRow.Cells["GroupID"].Value.ToString());
                        GetAuditClick(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// pop ups the audit trail ui for the selected groups. The forms is loaded through nirvanaMain using event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuTradeAuditTrailAllocated_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> groupIds = GetSelectedGroupIds(grdAllocated);
                if (groupIds.Count > 0)
                {
                    GetAuditClick(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                }
                else
                {
                    if (grdAllocated.ActiveRow != null)
                    {
                        groupIds.Add(grdAllocated.ActiveRow.Cells["GroupID"].Value.ToString());
                        GetAuditClick(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Toggles status of UI elements (grdAllocated, grdUnAllocated, btnsave, btnAutoGrp,btnGetAllocationData).
        /// message set to lblStatusStrip.Text. While working with some data on the ui, the controls are toggled so that next action is not initiated
        /// by UI unless the previous action is completed.
        /// </summary>
        /// <param name="message">To be displayed on lblStatusStrip. Pass empty string to keep lblStatusStrip.Text unaffected</param>
        /// <param name="elementStatus">To be set to UI.Enabled</param>
        public void ToggleUIElementsWithMessage(String message, Boolean elementStatus)
        {
            try
            {
                //If form is disposing then no need to set status on UI.
                if (this.IsDisposed || this.Disposing)
                    return;
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => ToggleUIElementsWithMessage(message, elementStatus)));
                }
                else
                {
                    //if (ultraToolbarsManager1 != null)
                    //ultraToolbarsManager1.Enabled = elementStatus;

                    if (grdAllocated != null)
                        grdAllocated.Enabled = elementStatus;
                    tabAllocation.Enabled = elementStatus;
                    if (grdUnallocated != null)
                        grdUnallocated.Enabled = elementStatus;
                    btnAllocate.Enabled = elementStatus;
                    if (!_isReadPermission)
                    {
						// disable buttons in case of applied tab on cost adjustment, PRANA-10427
                        if (tabAllocation.SelectedTab.Text == "Cost Adjustment" && ctrlCostAdjustment.checkActiveTab() == 1)
                        {
                                btnSave.Enabled = false;
                                btnSaveWOState.Enabled = false;
                                btnCancelData.Enabled = false;
                        }
                        else
                        {
                            btnSave.Enabled = elementStatus;
                            btnSaveWOState.Enabled = elementStatus;
                            btnCancelData.Enabled = elementStatus;
                        }
                    }
                    btnAutoGrp.Enabled = elementStatus;
                    btnGetAllocationData.Enabled = elementStatus;
                    btnReAllocate.Enabled = elementStatus;
                    btnClosing.Enabled = elementStatus;
                    btnCheckSide.Enabled = elementStatus;
                    //ControlBox = elementStatus;

                    //rbCurrent.Enabled = elementStatus;
                    rbHistorical.Enabled = elementStatus;
                    if (rbHistorical.CheckedIndex == 0)
                    {
                        dtToDatePickerAllocation.Enabled = elementStatus;
                        dtFromDatePickerAllocation.Enabled = elementStatus;
                    }

                    if (!elementStatus)
                    {
                        Thread.Sleep(5);
                        ControlDrawing.SuspendDrawing(grdUnallocated);
                        ControlDrawing.SuspendDrawing(grdAllocated);
                        grdUnallocated.BeginUpdate();
                        grdAllocated.BeginUpdate();

                        //grdUnallocated.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
                        //lblStatusStripProgress.ForeColor = Color.Red;
                        //lblStatusStrip.ForeColor = Color.Red;
                    }
                    else if (elementStatus)
                    {
                        //grdUnallocated.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                        Thread.Sleep(5);
                        grdUnallocated.EndUpdate();
                        grdAllocated.EndUpdate();
                        ControlDrawing.ResumeDrawing(grdUnallocated);
                        ControlDrawing.ResumeDrawing(grdAllocated);
                        //grdUnallocated.DataBind();
                        //lblStatusStripProgress.ForeColor = Color.Black;
                        //lblStatusStrip.ForeColor = Color.Black;
                    }
                    if (message != string.Empty)
                        lblStatusStrip.Text = message;

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                string msg = "Problem occured with Allocation. Please open the form again.";
                MessageBox.Show(msg, "Allocation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatusStrip.Text = msg;
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Starts a background worker to unallocate data. disables grids and button which will automatically be enabled on completed event.
        /// It also uses beginUpdate and endupdate methods of grid to change binding collection.
        /// </summary>
        /// <param name="groups">List of allocation group which is to be unallocated</param>
        private void UnAllocateDataAsync(List<AllocationGroup> groups)
        {
            try
            {
                if (allocationDataChange != null)
                {
                    allocationDataChange(this, new EventArgs<bool>(true));
                }
                timerProgress.Start();//stating timer for showing progress label

                //modified by amit on 19.03.2015
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-2902
                #region disable control box
                this.ControlBox = false;
                this.MinimizeBox = false;
                this.MaximizeBox = false;
                #endregion

                ToggleUIElementsWithMessage("Unallocating selected data. Please wait", false);
                BackgroundWorker bgUnAllocateData = new BackgroundWorker();
                bgUnAllocateData.DoWork += new DoWorkEventHandler(bgUnAllocateData_DoWork);
                bgUnAllocateData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgUnAllocateData_RunWorkerCompleted);
                bgUnAllocateData.RunWorkerAsync(groups);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Starts on main UI thread when background worker finishes unallocation of data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bgUnAllocateData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //modified by amit on 19.03.2015
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-2902
                #region enable control box
                this.ControlBox = true;
                this.MinimizeBox = true;
                this.MaximizeBox = true;
                #endregion

                if (e.Error == null)
                {
                    String message = e.Result as string;
                    if (!String.IsNullOrEmpty(message) && (message != AllocationConstants.UnAllocationCompletionStatus.Success.ToString()))
                    {

                        StringBuilder boxMessage = new StringBuilder();
                        boxMessage.AppendLine("Some groups could not be unallocated.");
                        if (message == AllocationConstants.UnAllocationCompletionStatus.FileWriteError.ToString())
                        {
                            boxMessage.Append("While writing the groupid(s) in the file, some issue occured. Please fetch the data and unallocate the trades again.");
                            MessageBox.Show(boxMessage.ToString(), "Allocation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            boxMessage.Append("Do you want to view details?");
                            DialogResult dr = MessageBox.Show(boxMessage.ToString(), "Allocation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (dr == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(message);
                            }
                        }
                    }
                    //if successfully unallocated, then clear allocated account qut and percentage numbers
                    ClearAllocatedAccountNumberControl(string.Empty);
                    ToggleUIElementsWithMessage("Unallocation Completed.", true);


                    //save data on the basis of chmw preferences
                    // As there should not be any un-allocated data 
                    // This save click is called after deleting unallocated groups in case of CHMW
                    if (CachedDataManager.GetInstance.GetPranaReleaseViewType().Equals(PranaReleaseViewType.CHMiddleWare))
                    {
                        btnSave_Click(this, null);
                    }
                }
                else
                {
                    ToggleUIElementsWithMessage("Unallocation Failed.", true);
                    bool rethrow = ExceptionPolicy.HandleException(e.Error, ApplicationConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                    {
                        throw e.Error;
                    }
                }

                //Calculating the selectedGrid row in the grid AddAndUpdateExternalTransactionID then Update trade counter
                UltraGridRow[] rows = grdAllocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsAllocatedGrd = Convert.ToInt32(rows.Count(row => row.Cells["checkBox"].Text == true.ToString()));

                UltraGridRow[] rows1 = grdUnallocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsUnallocatedGrd = Convert.ToInt32(rows1.Count(row1 => row1.Cells["checkBox"].Text == true.ToString()));

                UpdateTotalNoOfTradesForAllocatedgrd(_countSelectedRowsAllocatedGrd);
                UpdateTotalNoOfTradesForUnallocatedgrd(_countSelectedRowsUnallocatedGrd);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (allocationDataChange != null)
                {
                    allocationDataChange(this, new EventArgs<bool>(false));
                }
                //Stops timer and clears progress label
                if (timerProgress.Enabled)
                    timerProgress.Stop();
                lblStatusStripProgress.Text = string.Empty;
            }
        }

        /// <summary>
        /// Perform Unallocation of selected data on a background thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bgUnAllocateData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<AllocationGroup> groups = e.Argument as List<AllocationGroup>;
                StringBuilder alreadyClosedError = new StringBuilder();
                List<AllocationGroup> groupList = new List<AllocationGroup>();
                Parallel.ForEach(groups, group =>
                {
                    PostTradeEnums.Status groupStatus = _closingServices.InnerChannel.CheckGroupStatus(group);

                    if (groupStatus.Equals(PostTradeEnums.Status.None))
                    {
                        group.AllocationSchemeID = 0;
                        group.AllocationSchemeName = string.Empty;
                        group.ErrorMessage = string.Empty;
                        groupList.Add(group);

                        //modified by - omshiv, when isMasterFundRatioAllocation is enabled then update current allocation Pct, total shares in scheme cache
                        if (_allocationPrefs.GeneralRules.isMasterFundRatioAllocation)
                        {
                            // it will update based on taxlots in group.
                            _proxyAllocationServices.InnerChannel.UpdateAccountWisePostionInCache(group);
                        }

                        #region add elements to audit list
                        //this code is moved from method AllocationManager.GetInstance().UnAllocateGroup to reduce the iteration
                        AuditManager.Instance.AddGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.UNALLOCATE, "", "Group Unallocated", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                        if (group.TaxLots.Count > 0)
                        {
                            AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, true, DateTime.UtcNow, Prana.BusinessObjects.TradeAuditActionType.ActionType.UNALLOCATE, "", "Group Unallocated Taxlots Deleted", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                        }
                        #endregion
                    }
                    else
                    {
                        if (groupStatus.Equals(PostTradeEnums.Status.Closed))
                        {
                            alreadyClosedError.Append("GroupID : ");
                            alreadyClosedError.Append(group.GroupID);
                            alreadyClosedError.Append(" is fully or partially closed.");
                            alreadyClosedError.Append(Environment.NewLine);
                        }
                        else if (groupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                        {
                            alreadyClosedError.Append("On GroupID : ");
                            alreadyClosedError.Append(group.GroupID);
                            alreadyClosedError.Append(", corporate  Action is applied. First undo the corporate action to unallocate the trade.");
                            alreadyClosedError.Append(Environment.NewLine);

                        }
                        else if (groupStatus.Equals(PostTradeEnums.Status.Exercise) || groupStatus.Equals(PostTradeEnums.Status.IsExercised))
                        {
                            alreadyClosedError.Append("GroupID : ");
                            alreadyClosedError.Append(group.GroupID);
                            alreadyClosedError.Append(" is generated by exercise.");
                            alreadyClosedError.Append(Environment.NewLine);

                        }
                        else if (groupStatus.Equals(PostTradeEnums.Status.CostBasisAdjustment))     //Don't allow to unallocate for group generated by cost adjustment: http://jira.nirvanasolutions.com:8080/browse/PRANA-6806
                        {
                            alreadyClosedError.Append("GroupID : ");
                            alreadyClosedError.Append(group.GroupID);
                            alreadyClosedError.Append(" is generated by cost adjustment.");
                            alreadyClosedError.Append(Environment.NewLine);

                        }
                    }
                });
                AllocationManager.GetInstance().UnAllocateGroup(groupList);

                //Writing alreadyClosed to file and returning path to e.result
                if (alreadyClosedError.Length > 0)
                {
                    String path = Application.StartupPath + @"\Logs\UnallocationLog.txt";
                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter(path, false))
                        {
                            streamWriter.WriteLine(Environment.NewLine + DateTime.Now.ToString());
                            streamWriter.Write(alreadyClosedError.ToString());
                            e.Result = path;
                        }
                    }
                    catch (Exception ex)
                    {
                        e.Result = AllocationConstants.UnAllocationCompletionStatus.FileWriteError;
                        // Invoke our policy that is responsible for making sure no secure information
                        // gets out of our layer.
                        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGONLY);

                        if (rethrow)
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    e.Result = AllocationConstants.UnAllocationCompletionStatus.Success;
                }
                //Updating Total No of Trade Field
                //UpdateTotalNoOfTradesForAllocatedgrd(0);
                //UpdateTotalNoOfTradesForUnallocatedgrd(_countSelectedRowsUnallocatedGrd);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void mnuUnAllocate_Click(object sender, EventArgs e)
        {
            try
            {
                _proxyAllocationServices.InnerChannel.UpdatePreferencedAccountID();
                List<AllocationGroup> groups = GetSelectedGroups(grdAllocated);
                //Added to unallocate highlighted trade, PRANA-10754
                if (groups.Count == 0 && _selectedGrid.ActiveRow != null && _selectedGrid.ActiveRow.ListObject is AllocationGroup)
                    groups.Add((AllocationGroup)_selectedGrid.ActiveRow.ListObject);
                UnAllocateDataAsync(groups);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void mnuUnGroup_Click(object sender, EventArgs e)
        {
            try
            {
                List<AllocationGroup> groups = GetSelectedGroups(grdUnallocated);
                ApplyUnGrouping(groups);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void menuCashTran_Click(object sender, EventArgs e)
        {
            try
            {
                TaxLot SelectedTaxlot = grdAllocated.ActiveRow.ListObject as TaxLot;
                if (GenrateCashTransaction != null)
                {
                    CashDataEventArgs taxlotToSend = new CashDataEventArgs(SelectedTaxlot);
                    GenrateCashTransaction(this, taxlotToSend);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void mnuSymbolLookUp_Click(object sender, EventArgs e)
        {
            try
            {
                AllocationGroup group = null;
                //TaxLot taxlot = null;

                if (((System.Windows.Forms.Menu)(sender)).Name == "SMAllocated")
                {
                    if (grdAllocated.ActiveRow != null)
                    {
                        if (grdAllocated.ActiveRow.ListObject.GetType().Name.Equals("TaxLot"))
                        {
                            //taxlot = grdAllocated.ActiveRow.ListObject as TaxLot;
                            group = grdAllocated.ActiveRow.ParentRow.ListObject as AllocationGroup;
                        }
                        else
                        {
                            group = (AllocationGroup)grdAllocated.ActiveRow.ListObject;
                        }

                        if (group != null)
                        {
                            if (loadSymbolLookUpUIFromAllocation != null)
                            {
                                loadSymbolLookUpUIFromAllocation(this, new EventArgs<string>(group.Symbol));
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Please select a valid trade.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else if (((System.Windows.Forms.Menu)(sender)).Name == "SMUnallocated")
                {
                    if (grdUnallocated.ActiveRow != null)
                    {
                        group = (AllocationGroup)grdUnallocated.ActiveRow.ListObject;
                        if (group != null)
                        {
                            if (loadSymbolLookUpUIFromAllocation != null)
                            {
                                loadSymbolLookUpUIFromAllocation(this, new EventArgs<string>(group.Symbol));
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Please select a valid trade.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        //close trade on mouse right click
        private void mnuCloseTrade_Click(object sender, EventArgs e)
        {
            try
            {
                AllocationGroup group = null;
                TaxLot taxlot = null;

                if (grdAllocated.ActiveRow != null)
                {
                    if (grdAllocated.ActiveRow.ListObject.GetType().Name.Equals("TaxLot"))
                    {
                        taxlot = grdAllocated.ActiveRow.ListObject as TaxLot;
                        group = grdAllocated.ActiveRow.ParentRow.ListObject as AllocationGroup;
                        group.AccountID = taxlot.Level1ID;
                        group.StrategyID = taxlot.Level2ID;
                    }
                    else
                    {
                        group = (AllocationGroup)grdAllocated.ActiveRow.ListObject;
                        group.AccountID = 0;
                        group.StrategyID = 0;
                    }

                    if (group != null && group.PersistenceStatus == ApplicationConstants.PersistenceStatus.NotChanged)
                    {
                        if (group.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || group.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed) || group.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed))
                        {
                            if (loadCloseTradeUIFromAllocation != null)
                            {
                                loadCloseTradeUIFromAllocation(this, new EventArgs<AllocationGroup>(group));
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Please select a valid trade.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void mnuAllocatedEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdAllocated.ActiveRow != null)
                {
                    AllocationGroup group = grdAllocated.ActiveRow.ListObject as AllocationGroup;
                    //bool isEditedAllocation = true;
                    if (group != null)
                    {
                        tabAllocation.Tabs[1].Selected = true;
                        ctrlAmendmend1.EditGroupDetails(group);
                    }
                    ctrlAmendmend1.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Closed = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Preferences

        public void ApplyPreferences(object sender, EventArgs<string, IPreferenceData> e)
        {
            try
            {
                if (!e.Value.Equals(PranaModules.ALLOCATION_MODULE)) return;
                {
                    if (_isEventAlive)
                    {
                        AllocationPreferences allocationPreferences = (AllocationPreferences)e.Value2;
                        _allocationPrefs = allocationPreferences;
                        SetPreferences();
                        _proxyAllocationServices.InnerChannel.SaveLastPreferencedAccountID(_allocationPrefs.GeneralRules.SelectedAccountID);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }
        private void ApplyRowColorUnAllocated()
        {
            try
            {
                foreach (UltraGridRow row in grdUnallocated.Rows)
                {
                    SetUnAllocatedRowProperties(row);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void ApplyRowColorAllocated()
        {
            try
            {
                foreach (UltraGridRow row in grdAllocated.Rows)
                {
                    SetAllocatedRowProperties(row);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void SetAllocatedRowProperties(UltraGridRow row)
        {
            try
            {
                Color backColor = Color.White;
                Color foreColor = Color.Black;
                if (row.Band.Index != 0)
                {
                    backColor = Color.FromArgb(_allocationPrefs.RowProperties.AllocatedLessTotalQtyBackColor);
                    foreColor = Color.FromArgb(_allocationPrefs.RowProperties.AllocatedLessTotalQtyTextColor);
                }

                else if (row.Cells["checkBox"].Value.ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    backColor = Color.FromArgb(_allocationPrefs.RowProperties.SelectedRowBackColor);
                    foreColor = Color.FromArgb(_allocationPrefs.RowProperties.SelectedRowTextColor);
                }
                else
                {
                    if (Convert.ToBoolean(row.Cells["AllocatedEqualTotalQty"].Value))
                    {
                        backColor = Color.FromArgb(_allocationPrefs.RowProperties.AllocatedEqualTotalQtyBackColor);
                        foreColor = Color.FromArgb(_allocationPrefs.RowProperties.AllocatedEqualTotalQtyTextColor);
                    }
                    else
                    {
                        backColor = Color.FromArgb(_allocationPrefs.RowProperties.AllocatedLessTotalQtyBackColor);
                        foreColor = Color.FromArgb(_allocationPrefs.RowProperties.AllocatedLessTotalQtyTextColor);
                    }
                }
                SetRowColor(row, backColor, foreColor);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void SetUnAllocatedRowProperties(UltraGridRow row)
        {
            try
            {
                Color backColor = Color.White;
                Color foreColor = Color.Black;
                if (row.Band.Index != 0)
                {
                    backColor = Color.FromArgb(_allocationPrefs.RowProperties.UnAllocatedBackColor);
                    foreColor = Color.FromArgb(_allocationPrefs.RowProperties.UnAllocatedTextColor);
                }
                else if (row.Cells["checkBox"].Value.ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    backColor = Color.FromArgb(_allocationPrefs.RowProperties.SelectedRowBackColor);
                    foreColor = Color.FromArgb(_allocationPrefs.RowProperties.SelectedRowTextColor);
                }
                else
                {
                    if (Convert.ToBoolean(row.Cells["NotAllExecuted"].Value))
                    {
                        backColor = Color.FromArgb(_allocationPrefs.RowProperties.ExecutedLessTotalQtyBackColor);
                        foreColor = Color.FromArgb(_allocationPrefs.RowProperties.ExecutedLessTotalQtyTextColor);
                    }
                    else
                    {

                        backColor = Color.FromArgb(_allocationPrefs.RowProperties.UnAllocatedBackColor);
                        foreColor = Color.FromArgb(_allocationPrefs.RowProperties.UnAllocatedTextColor);
                    }
                }
                SetRowColor(row, backColor, foreColor);
            }

            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void SetRowColor(UltraGridRow row, Color backColor, Color foreColor)
        {
            try
            {
                row.Appearance.BackColor = backColor;
                row.Appearance.BackColor2 = backColor;
                row.Appearance.ForeColor = foreColor;
                if (row.HasChild())
                {
                    foreach (UltraGridChildBand band in row.ChildBands)
                    {
                        foreach (UltraGridRow childrow in band.Rows)
                        {
                            childrow.Appearance.BackColor = backColor;
                            childrow.Appearance.BackColor2 = backColor;
                            childrow.Appearance.ForeColor = foreColor;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

            }
        }
        public void SetPreferences()
        {
            try
            {
                List<string> panelSize = GeneralUtilities.GetListFromString(_allocationPrefs.SplitPanelSize, ',');

                if (panelSize != null)
                {
                    if (panelSize.Count == 12)
                    {
                        splitContainer1Panel1.Width = Convert.ToInt32(panelSize[0]);
                        splitContainer1Panel1.Height = Convert.ToInt32(panelSize[1]);
                        splitContainer1Panel2.Width = Convert.ToInt32(panelSize[2]);
                        splitContainer1Panel2.Height = Convert.ToInt32(panelSize[3]);
                        splitContainer2Panel1.Width = Convert.ToInt32(panelSize[4]);
                        splitContainer2Panel1.Height = Convert.ToInt32(panelSize[5]);
                        splitContainer2Panel2.Width = Convert.ToInt32(panelSize[6]);
                        splitContainer2Panel2.Height = Convert.ToInt32(panelSize[7]);
                        splitContainer3Panel1.Width = Convert.ToInt32(panelSize[8]);
                        splitContainer3Panel1.Height = Convert.ToInt32(panelSize[9]);
                        splitContainer3Panel2.Width = Convert.ToInt32(panelSize[10]);
                        splitContainer3Panel2.Height = Convert.ToInt32(panelSize[11]);
                    }
                }
                //set allocation form height and width, PRANA-5836
                if (_allocationPrefs.AllocationFormHeight != 0 && _allocationPrefs.AllocationFormWidth != 0)
                {
                    this.ClientSize = new Size(_allocationPrefs.AllocationFormWidth, _allocationPrefs.AllocationFormHeight); 
                }
                _defaults.SetDefaults(_allocationPrefs.AllocationDefaultList);

                BindAccountDefaults();
                SetSortKey();
                ApplyRowColorUnAllocated();
                ApplyRowColorAllocated();

                BindDefaults();
                //set from preferences that allocation ByDefault will be done by Account or Symbol
                AllocationByAccountOrSymbol();

                //Added to set default rule control, PRANA-11244
                SetDefaultRuleControlPreferences();
                
                //Aded to Set value of net amount fields in allocation, PRANA-11751
                SetNetAmountFields(grdAllocated);
                SetNetAmountFields(grdUnallocated);
                //if (_allocationPrefs.GeneralRules.AllocateBasedonOpenPositions)
                //{
                //    chkboxForceAllocation.Enabled = true;
                //}
                //else
                //{
                //    if (chkboxForceAllocation.Checked)
                //    {
                //        chkboxForceAllocation.Checked = false;
                //    }
                //    chkboxForceAllocation.Enabled = false;
                //}
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Set value of net amount fields in allocation, PRANA-11751
        /// </summary>
        /// <param name="grid">Allocation Grid</param>
        private void SetNetAmountFields(UltraGrid grid)
        {
            try
            {
                //added fxrate and settlement fxrate in net amount base/settlement calculation when asset is selected in preference, PRANA-12962
                List<int> assetsIDList = AllocationManager.GetInstance().Allocation.InnerChannel.GetDefaultRule(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID).AssetsWithCommissionInNetAmount;
                foreach (UltraGridRow row in grid.Rows)
                {
                    AllocationGroup group = (AllocationGroup)row.ListObject;
                    double NotionalValue = group.CumQty * group.AvgPrice * group.ContractMultiplier;
                    int sideMul = Prana.BusinessLogic.PMCalculations.GetSideMultilpier(group.OrderSideTagValue);

                    string transectionType = string.Empty;

                    if (row.Cells.Exists(COL_TransactionType) && row.Cells[COL_TransactionType].Value != null)
                    {
                        transectionType = row.Cells[COL_TransactionType].Value.ToString();
                    }

                    string conversionOperator = row.Cells[OrderFields.PROPERTY_FXConversionMethodOperator].Value.ToString();
                    double fxrate = Convert.ToDouble(row.Cells[OrderFields.PROPERTY_FXRate].Value);
                    fxrate = conversionOperator.Equals("D") ? (1 / fxrate) : fxrate;

                    string settlConversionOperator = row.Cells[OrderFields.PROPERTY_SettCurrFXRateCalc].Value.ToString();
                    double settrate = Convert.ToDouble(row.Cells[OrderFields.PROPERTY_SettCurrFXRate].Value);
                    if (!settlConversionOperator.Equals("M"))
                        settrate = 1 / settrate;

                    if (assetsIDList != null && assetsIDList.Contains(group.AssetID))
                    {
                        if (row.Cells.Exists(COL_NETAMOUNTWITHCOMMISSION))
                            row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = (Convert.ToDouble(row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) * sideMul).ToString("#,###0.####");
                        if (row.Cells.Exists(COL_NETMONEY))
                            row.Cells[COL_NETMONEY].Value = (Convert.ToDouble(row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) * sideMul * settrate).ToString("#,###0.####");
                        if (row.Cells.Exists(COL_NETAMOUNTBASE))
                            row.Cells[COL_NETAMOUNTBASE].Value = (Convert.ToDouble(row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) * sideMul * fxrate).ToString("#,###0.####");
                    }
                    else
                    {
                        if (row.Cells.Exists(COL_NETAMOUNTWITHCOMMISSION))
                            row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = (Convert.ToDouble(row.Cells[COL_NETAMOUNT].Value) + ((Convert.ToDouble(row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) ) * sideMul)).ToString("#,###0.####");
                        if (row.Cells.Exists(COL_NETMONEY))
                            row.Cells[COL_NETMONEY].Value = ((NotionalValue + (Convert.ToDouble(row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value)) * sideMul) * settrate).ToString("#,###0.####");
                        if (row.Cells.Exists(COL_NETAMOUNTBASE))
                            row.Cells[COL_NETAMOUNTBASE].Value = (Convert.ToDouble(row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value) * fxrate).ToString("#,###0.####");
                    }
                    if (row.HasChild() && grid.Name.Equals(AllocationConstants.AllocationGrid.grdAllocated.ToString()))
                    {
                        foreach (UltraGridRow childrow in row.ChildBands[0].Rows)
                        {
                            TaxLot taxlot = (TaxLot)childrow.ListObject;
                            sideMul = Prana.BusinessLogic.PMCalculations.GetSideMultilpier(taxlot.OrderSideTagValue);

                            if (CheckTransactionType(transectionType) || taxlot.TransactionSource == TransactionSource.CostAdjustment)
                            {
                                if (assetsIDList != null && assetsIDList.Contains(Convert.ToInt32(taxlot.AssetID)))
                                    childrow.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = (taxlot.TotalCommissionandFees * sideMul).ToString("#,###0.####");
                                else
                                    childrow.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = ((taxlot.AvgPrice * taxlot.TaxLotQty * taxlot.ContractMultiplier) + ((taxlot.TotalCommissionandFees ) * sideMul)).ToString("#,###0.####");
                            }
                            else
                            {
                                childrow.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = 0.0000.ToString("0.0000");
                            }

                            if (CheckTransactionType(transectionType) || taxlot.TransactionSource == TransactionSource.CostAdjustment)
                            {
                                if (assetsIDList != null && assetsIDList.Contains(Convert.ToInt32(taxlot.AssetID)))
                                    childrow.Cells[COL_NETMONEY].Value = (taxlot.TotalCommissionandFees * sideMul * settrate).ToString("#,###0.####");
                                else
                                    childrow.Cells[COL_NETMONEY].Value = (((taxlot.AvgPrice * taxlot.TaxLotQty * taxlot.ContractMultiplier) + ((taxlot.TotalCommissionandFees ) * sideMul)) * settrate).ToString("#,###0.####");
                            }
                            else
                                childrow.Cells[COL_NETMONEY].Value = 0.0000.ToString("0.0000");

                            if (CheckTransactionType(transectionType) || taxlot.TransactionSource == TransactionSource.CostAdjustment)
                            {
                                if (assetsIDList != null && assetsIDList.Contains(Convert.ToInt32(taxlot.AssetID)))
                                    childrow.Cells[COL_NETAMOUNTBASE].Value = (taxlot.TotalCommissionandFees * sideMul * fxrate).ToString("#,###0.####");
                                else
                                    childrow.Cells[COL_NETAMOUNTBASE].Value = (((taxlot.AvgPrice * taxlot.TaxLotQty * taxlot.ContractMultiplier) + ((taxlot.TotalCommissionandFees ) * sideMul)) * fxrate).ToString("#,###0.####");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        ///  method to check the version of saved layout ,PRANA-10773
        /// </summary>
        /// <param name="isAllocatedGrid"></param>
        private void LoadLayout(bool isAllocatedGrid)
        {
            try
            {
                /* This method will first try to load layout from the grdAllocated.xml and grdUnallocated.xml file.
                 * If these files are not found, then it will try to load saved columns from AllocationPreferences.xml
                 * If even this file doesn't exist, then all columns will be shown on grids, PRANA-11652
                 */
                System.IO.FileStream fs;
                if (isAllocatedGrid)
                {
                    //if (IsLoadSavedLayout(grdAllocated, out fs))
                    //{
                    //    fs.Position = (long) (AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID.Length);
                    //    grdAllocated.DisplayLayout.LoadFromXml(fs, PropertyCategories.All);
                    //}
                    fs = GetFileStream(grdAllocated);
                    if (fs != null)
                    {
                        grdAllocated.DisplayLayout.LoadFromXml(fs, PropertyCategories.All);
                        SetGridColumns(grdAllocated, true);
                    }
                    else if (!string.IsNullOrWhiteSpace(_allocationPrefs.AllocatedColumns))
                    {
                        List<string> visibleColumsAlloc = GeneralUtilities.GetListFromString(_allocationPrefs.AllocatedColumns, ',');
                        List<string> visibleColumnsWidthAlloc = GeneralUtilities.GetListFromString(_allocationPrefs.AllocatedColumnWidth, ',');
                        if (visibleColumsAlloc == null || visibleColumsAlloc.Count == 0)
                        {
                            visibleColumsAlloc = _allocationPrefs.ColumnList.AllocatedGridColumns.DisplayColumns;
                        }
                        if (visibleColumnsWidthAlloc == null)
                        {
                            visibleColumnsWidthAlloc = new List<string>();
                        }
                        List<List<string>> bandsColumns = new List<List<string>>();
                        bandsColumns.Add(visibleColumsAlloc);
                        Prana.Utilities.UIUtilities.UltraWinGridUtils.DisplayColumns(bandsColumns, grdAllocated, visibleColumnsWidthAlloc);
                        SetGridColumns(grdAllocated, true);
                    }
                    else
                    {
                        SetGridColumns(grdAllocated, false);
                    }
                        ShowColumns(grdAllocated);
                    }
                else
                {
                    //if (IsLoadSavedLayout(grdUnallocated, out fs))
                    //{
                    //    fs.Position = (long) (AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID.Length);
                    //    grdUnallocated.DisplayLayout.LoadFromXml(fs, PropertyCategories.All);
                    //}
                    fs = GetFileStream(grdUnallocated);
                    if (fs != null)
                    {
                        grdUnallocated.DisplayLayout.LoadFromXml(fs, PropertyCategories.All);
                        SetGridColumns(grdUnallocated, true);
                    }
                    else if (!string.IsNullOrWhiteSpace(_allocationPrefs.UnAllocatedColumns))
                    {
                        List<string> visibleColumsUnAlloc = GeneralUtilities.GetListFromString(_allocationPrefs.UnAllocatedColumns, ',');
                        List<string> visibleColumnsWidthUnAlloc = GeneralUtilities.GetListFromString(_allocationPrefs.UnallocatedColumnWidth, ',');
                        if (visibleColumsUnAlloc == null || visibleColumsUnAlloc.Count == 0)
                        {
                            visibleColumsUnAlloc = _allocationPrefs.ColumnList.UnAllocatedGridColumns.DisplayColumns;
                        }
                        if (visibleColumnsWidthUnAlloc == null)
                        {
                            visibleColumnsWidthUnAlloc = new List<string>();
                        }
                        List<List<string>> bandsColumns = new List<List<string>>();
                        bandsColumns.Add(visibleColumsUnAlloc);
                        Prana.Utilities.UIUtilities.UltraWinGridUtils.DisplayColumns(bandsColumns, grdUnallocated, visibleColumnsWidthUnAlloc);
                        SetGridColumns(grdUnallocated, true);
                    }
                    else
                    {
                        SetGridColumns(grdUnallocated, false);
                    }
                        ShowColumns(grdUnallocated);
                    }
                if (fs != null)
                    fs.Close();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                }
            }

        /// <summary>
        /// Get file stream based on different allocation saved layouts(with/without version)
        /// </summary>
        /// <param name="grid">The allocation grid</param>
        /// <returns>file stream</returns>
        private FileStream GetFileStream(UltraGrid grid)
        {
            System.IO.FileStream fs = null;
            try
            {
                string path = GetDirectoryPath() + @"\" + grid.Name + ".xml";
                if (File.Exists(path))
                    fs = new System.IO.FileStream(GetDirectoryPath() + @"\" + grid.Name + ".xml", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

                if (fs != null)
                {
                    //Added to get grid layout version, PRANA-11946
                    string xmlString = File.ReadAllText(path);
                    if (xmlString.Contains("Version"))
                    {
                        string layoutVersion = xmlString.Substring(xmlString.IndexOf("<Version>"), xmlString.LastIndexOf("</Version>") + "</Version>".Length);
                        fs.Position = (long)(layoutVersion.Length);
                    }
                    else
                        fs.Position = 0;
                    
                    //byte[] b = new byte[AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID.Length];
                    //fs.Read(b, 0, (int)(AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID.Length));
                    //string layoutVersion = System.Text.Encoding.UTF8.GetString(b);

                    //if (layoutVersion.Contains("Version"))
                    //    fs.Position = (long)(AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID.Length);
                    //else
                    //    fs.Position = 0;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                }
            return fs;
            }

        /// <summary>
        /// Method to check weather to load saved layout or not based on version number
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="fs"></param>
        /// <returns></returns>
        //private bool IsLoadSavedLayout( UltraGrid grid, out System.IO.FileStream fs)
        //{
        //    bool doLoadLayout = false;
        //    fs = null;
        //    try
        //    {
        //        string path = GetDirectoryPath() + @"\" + grid.Name + ".xml";
        //        UltraGrid gridTest = new UltraGrid();
        //        UltraGridBand band = grid.DisplayLayout.Bands[0];               

        //        if (File.Exists(path))
        //        {
        //            // Open the file where the layout has been saved
        //            fs = new System.IO.FileStream(GetDirectoryPath() + @"\" + grid.Name + ".xml", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
        //            byte[] b = new byte[AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID.Length];
        //            fs.Read(b, 0, (int)(AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID.Length));
        //            string layoutVersion = System.Text.Encoding.UTF8.GetString(b);
        //            if (layoutVersion == AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID)
        //            {
        //                // Reset the Position of the file stream to where the layout data begins, then load the layout from the stream by calling Load method. 
        //                fs.Position = (long)(AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID.Length);
        //                gridTest.DisplayLayout.LoadFromXml(fs, PropertyCategories.All);
        //                doLoadLayout = true;
        //            }
        //        }

        //        if (doLoadLayout)
        //        {
        //            //First check the no. of groups then check group key
        //            if (gridTest.DisplayLayout.Bands[0].Groups.Count == band.Groups.Count)
        //            {
        //                List<string> keys = new List<string>();
        //                foreach (UltraGridGroup grp in gridTest.DisplayLayout.Bands[0].Groups)
        //                    keys.Add(grp.Key);
        //                foreach (UltraGridGroup grp1 in band.Groups)
        //                {
        //                    if (!keys.Contains(grp1.Key))
        //                        doLoadLayout = false;
        //                }
        //            }
        //            else
        //                doLoadLayout = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return doLoadLayout;
        //}

        private void AllocationByAccountOrSymbol()
        {
            try
            {
                if (_allocationPrefs.AllocationByAccountOrSymbol.Symbol.Equals(true))
                {
                    rbtnAllocationBySymbol.CheckedIndex = 0;
                    cmbAllocationScheme.Enabled = true;
                    cmbAllocationScheme.BringToFront();
                    cmbbxdefaults.Enabled = false;
                }
                else
                {
                    rbtnAllocationByAccount.CheckedIndex = 0;
                    cmbbxdefaults.Enabled = true;
                    cmbbxdefaults.BringToFront();
                    cmbbxdefaults.Value = -1;
                    cmbAllocationScheme.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region ColumnHeaders
        private const string COL_AUECLOCALDATE = "AuecLocalDate";
        private const string COL_CUMQTY = "CumQty";
        private const string COL_ORDERSIDE = "OrderSide";
        private const string COL_COUNTERPARTYNAME = "CounterPartyName";
        private const string COL_ASSETNAME = "AssetName";
        private const string COL_EXCHANGENAME = "ExchangeName";
        private const string COL_COMPANYUSERNAME = "CompanyUserName";
        private const string COL_CURRENCYNAME = "CurrencyName";
        private const string COL_UNDERLYINGNAME = "UnderlyingName";
        private const string COL_TRADINGACCOUNTNAME = "TradingAccountName";
        private const string COL_COMPANYNAME = "CompanyName";
        private const string COL_ASSETCATEGORY = "AssetCategory";
        private const string COL_NETAMOUNT = "NetAmount";
        private const string COL_SEDOLSYMBOL = "SedolSymbol";
        private const string COL_BLOOMBERGSYMBOL = "BloombergSymbol";
        private const string COL_CUSIPSYMBOL = "CusipSymbol";
        private const string COL_ISINSYMBOL = "IsinSymbol";
        private const string COL_COMMSOURCE = "CommSource";
        private const string COL_CommissionSource = "CommissionSource";
        private const string COL_SOFTCOMMSOURCE = "SoftCommSource";
        private const string COL_SOFTCommissionSource = "SoftCommissionSource";
        private const string COL_ClosingStatus = "ClosingStatus";
        private const string COL_ClosingAlgoText = "ClosingAlgoText";
        private const string COL_ClosingAlgo = "ClosingAlgo";
        private const string COL_GroupStatus = "GroupStatus";
        private const string COL_NETAMOUNTWITHCOMMISSION = "NetAmountWithCommission";
        private const string COL_AVERAGEPRICELOCAL = "AvgPrice";

        private const string CAP_TRADEDATE = "Trade Date";
        private const string CAP_EXECUTEDQTY = "Executed Qty";
        private const string CAP_SIDE = "Side";
        private const string CAP_COUNTERPARTY = ApplicationConstants.CONST_BROKER;
        private const string CAP_ASSET = "Asset";
        private const string CAP_EXCHANGE = "Exchange";
        private const string CAP_USER = "User";
        private const string CAP_CURRENCY = "Currency";
        private const string CAP_UNDERLYING = "Underlying";
        private const string CAP_TRADINGACCOUNT = "Trading Account";
        private const string CAP_COMPANY = "Company";
        private const string CAP_ASSETCATEGORY = "Asset Category";
        private const string CAP_NETAMOUNT = "Pay/Receive";
        private const string CAP_SEDOLSYMBOL = "Sedol";
        private const string CAP_BLOOMBERGSYMBOL = "Bloomberg";
        private const string CAP_CUSIPSYMBOL = "Cusip";
        private const string CAP_ISINSYMBOL = "ISIN";
        private const string CAP_COMMSOURCE = "Commission Source";
        private const string CAP_SOFTCOMMSOURCE = "Soft Commission Source";
        private const string CAP_CLOSINGSTATUS = "Closing Status";
        private const string CAP_CLOSINGALGO = "Closing Method";
        private const string CAP_TRANSACTIONTYPE = "Transaction Type";
        private const string CAP_GROUPSTATUS = "Group Status";

        private const string CAP_NETAMOUNTWITHCOMMISSION = "Net Amount(Local)"; // net amount(local)

        private const string CAP_TRANSACTIONSOURCE = "Transaction Source";

        private const string CAPTION_TradeAttribute1 = "Trade Attribute 1";
        private const string CAPTION_TradeAttribute2 = "Trade Attribute 2";
        private const string CAPTION_TradeAttribute3 = "Trade Attribute 3";
        private const string CAPTION_TradeAttribute4 = "Trade Attribute 4";
        private const string CAPTION_TradeAttribute5 = "Trade Attribute 5";
        private const string CAPTION_TradeAttribute6 = "Trade Attribute 6";

        private const string COL_TradeAttribute1 = "TradeAttribute1";
        private const string COL_TradeAttribute2 = "TradeAttribute2";
        private const string COL_TradeAttribute3 = "TradeAttribute3";
        private const string COL_TradeAttribute4 = "TradeAttribute4";
        private const string COL_TradeAttribute5 = "TradeAttribute5";
        private const string COL_TradeAttribute6 = "TradeAttribute6";
        private const string COL_TransactionType = "TransactionType";

        private const string COL_TransactionSource = "TransactionSource";

        private const string COL_COMMISSIONPERSHARE = "CommissionPerShare";
        private const string CAP_COMMISSIONPERSHARE = "Commission/Share";

        private const string CAP_TOTALCOMMISSIONANDFEES = "Total Commission & Fees";

        private const string COL_TOTALCOMMISSION = "TotalCommission";
        private const string CAP_TOTALCOMMISSION = "Total Commission";

        private const string COL_ImportFileName = "ImportFileName";
        private const string CAPTION_ImportFileName = "Import File Name";

        private const string COL_NETMONEY = "NetMoney"; // net money
        private const string CAPTION_NETMONEY_SETTLEMENT = "Net Amount(Settlement)";

        private const string COL_NETAMOUNTBASE = "NetAmountBase"; // net amount(base)
        private const string CAPTION_NETAMOUNT_BASE = "Net Amount(Base)";

        //Added principal amount base and local, PRANA-11379
        private const string COL_PRINCIPALAMOUNTBASE = "PrincipalAmountBase";// Principal Amount(Base)
        private const string CAPTION_PRINCIPALAMOUNTBASE = "Principal Amount(Base)";

        private const string COL_PRINCIPALAMOUNTLOCAL = "PrincipalAmountLocal";// Principal Amount(Local)
        private const string CAPTION_PRINCIPALAMOUNTLOCAL = "Principal Amount(Local)";

        // Added Avg Price(Base) column, PRANA-10775
        private const string COL_AVGPRICEBASE = "AvgPriceBase";
        private const string CAPTION_AVGPRICE_BASE = "Avg Price(Base)";

        #endregion

        private void SetGridColumnsHeaders(UltraGrid grid)
        {
            try
            {
                UltraGridBand gridBand = grid.DisplayLayout.Bands[0];
                if (gridBand.Columns.Exists("checkBox"))
                {
                    gridBand.Columns["checkBox"].Header.Caption = String.Empty;
                }
                UltraGridColumn colAssetCategory = gridBand.Columns[COL_ASSETCATEGORY];
                colAssetCategory.Header.Caption = CAP_ASSETCATEGORY;

                UltraGridColumn colTradeDate = gridBand.Columns[COL_AUECLOCALDATE];
                colTradeDate.Header.Caption = CAP_TRADEDATE;

                UltraGridColumn colCumQty = gridBand.Columns[COL_CUMQTY];
                colCumQty.Header.Caption = CAP_EXECUTEDQTY;

                UltraGridColumn colOrderSide = gridBand.Columns[COL_ORDERSIDE];
                colOrderSide.Header.Caption = CAP_SIDE;

                UltraGridColumn colCounterParty = gridBand.Columns[COL_COUNTERPARTYNAME];
                colCounterParty.Header.Caption = CAP_COUNTERPARTY;

                UltraGridColumn colExchange = gridBand.Columns[COL_EXCHANGENAME];
                colExchange.Header.Caption = CAP_EXCHANGE;

                UltraGridColumn colCompany = gridBand.Columns[COL_COMPANYUSERNAME];
                colCompany.Header.Caption = CAP_USER;

                UltraGridColumn colCurrency = gridBand.Columns[COL_CURRENCYNAME];
                colCurrency.Header.Caption = CAP_CURRENCY;

                UltraGridColumn colUnderLying = gridBand.Columns[COL_UNDERLYINGNAME];
                colUnderLying.Header.Caption = CAP_UNDERLYING;

                UltraGridColumn colTradingAccount = gridBand.Columns[COL_TRADINGACCOUNTNAME];
                colTradingAccount.Header.Caption = CAP_TRADINGACCOUNT;

                UltraGridColumn colCompanyName = gridBand.Columns[COL_COMPANYNAME];
                colCompanyName.Header.Caption = CAP_COMPANY;

                UltraGridColumn colNetAmount = gridBand.Columns[COL_NETAMOUNT];
                colNetAmount.Header.Caption = CAP_NETAMOUNT;

                UltraGridColumn colSedolSymbol = gridBand.Columns[COL_SEDOLSYMBOL];
                colSedolSymbol.Header.Caption = CAP_SEDOLSYMBOL;

                UltraGridColumn colBloombergSymbol = gridBand.Columns[COL_BLOOMBERGSYMBOL];
                colBloombergSymbol.Header.Caption = CAP_BLOOMBERGSYMBOL;

                UltraGridColumn colCusipSymbol = gridBand.Columns[COL_CUSIPSYMBOL];
                colCusipSymbol.Header.Caption = CAP_CUSIPSYMBOL;

                UltraGridColumn colISINSymbol = gridBand.Columns[COL_ISINSYMBOL];
                colISINSymbol.Header.Caption = CAP_ISINSYMBOL;

                UltraGridColumn colcommSourceID = gridBand.Columns[COL_CommissionSource];
                colcommSourceID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colcommSourceID.Hidden = true;

                UltraGridColumn colSoftCommSourceID = gridBand.Columns[COL_SOFTCommissionSource];
                colSoftCommSourceID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colSoftCommSourceID.Hidden = true;

                UltraGridColumn colcalcBasis = gridBand.Columns["CalcBasis"];
                colcalcBasis.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colcalcBasis.Hidden = true;

                UltraGridColumn colSoftCommissionCalcBasis = gridBand.Columns["SoftCommissionCalcBasis"];
                colSoftCommissionCalcBasis.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colSoftCommissionCalcBasis.Hidden = true;

                UltraGridColumn colCommSource = gridBand.Columns[COL_COMMSOURCE];
                colCommSource.Header.Caption = CAP_COMMSOURCE;
                colCommSource.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colCommSource.Hidden = true;

                UltraGridColumn colsoftCommSource = gridBand.Columns[COL_SOFTCOMMSOURCE];
                colsoftCommSource.Header.Caption = CAP_SOFTCOMMSOURCE;
                colsoftCommSource.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colsoftCommSource.Hidden = true;

                UltraGridColumn colTradeAtt1 = gridBand.Columns[COL_TradeAttribute1];
                colTradeAtt1.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute1);
                colTradeAtt1.CellActivation = Activation.AllowEdit;

                UltraGridColumn colTradeAtt2 = gridBand.Columns[COL_TradeAttribute2];
                colTradeAtt2.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute2);
                colTradeAtt2.CellActivation = Activation.AllowEdit;

                UltraGridColumn colTradeAtt3 = gridBand.Columns[COL_TradeAttribute3];
                colTradeAtt3.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute3);
                colTradeAtt3.CellActivation = Activation.AllowEdit;

                UltraGridColumn colTradeAtt4 = gridBand.Columns[COL_TradeAttribute4];
                colTradeAtt4.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute4);
                colTradeAtt4.CellActivation = Activation.AllowEdit;

                UltraGridColumn colTradeAtt5 = gridBand.Columns[COL_TradeAttribute5];
                colTradeAtt5.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute5);
                colTradeAtt5.CellActivation = Activation.AllowEdit;

                UltraGridColumn colTradeAtt6 = gridBand.Columns[COL_TradeAttribute6];
                colTradeAtt6.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute6);
                colTradeAtt6.CellActivation = Activation.AllowEdit;

                UltraGridColumn colClosingStatus = gridBand.Columns[COL_ClosingStatus];
                colClosingStatus.Header.Caption = CAP_CLOSINGSTATUS;

                UltraGridColumn colTransactionType = gridBand.Columns[COL_TransactionType];
                colTransactionType.Header.Caption = CAP_TRANSACTIONTYPE;
                colTransactionType.ValueList = CommonDataCache.CachedDataManager.GetInstance.GetTransactionTypeValueList().Clone();
                colTransactionType.CellActivation = Activation.NoEdit;

                UltraGridColumn colTotalCommissionAndFees = gridBand.Columns[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES];
                colTotalCommissionAndFees.Header.Caption = CAP_TOTALCOMMISSIONANDFEES;
                gridBand.Columns[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Width = 80;

                UltraGridColumn colCommissionPerShare = gridBand.Columns[COL_COMMISSIONPERSHARE];
                colCommissionPerShare.Header.Caption = CAP_COMMISSIONPERSHARE;
                colCommissionPerShare.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                UltraGridColumn colSoftCommissionPerShare = gridBand.Columns[OrderFields.PROPERTY_SOFTCOMMISSIONPERSHARE];
                colSoftCommissionPerShare.Header.Caption = OrderFields.CAPTION_SOFTCOMMISSIONPERSHARE;
                colSoftCommissionPerShare.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                UltraGridColumn colTotalCommissionPerShare = gridBand.Columns[OrderFields.PROPERTY_TOTALCOMMISSIONPERSHARE];
                colTotalCommissionPerShare.Header.Caption = OrderFields.CAPTION_TOTALCOMMISSIONPERSHARE;
                colTotalCommissionPerShare.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                UltraGridColumn colGroupStatus = gridBand.Columns[COL_GroupStatus];
                colGroupStatus.Header.Caption = CAP_GROUPSTATUS;

                if (gridBand.Columns.Exists(COL_NETAMOUNTWITHCOMMISSION))
                {
                    UltraGridColumn colNetAmountWithCommission = gridBand.Columns[COL_NETAMOUNTWITHCOMMISSION];
                    colNetAmountWithCommission.Header.Caption = CAP_NETAMOUNTWITHCOMMISSION;
                }
                UltraGridColumn colStampDuty = gridBand.Columns[OrderFields.PROPERTY_STAMPDUTY];
                colStampDuty.Header.Caption = OrderFields.CAPTION_STAMPDUTY;

                UltraGridColumn colTransactionLevy = gridBand.Columns[OrderFields.PROPERTY_TRANSACTIONLEVY];
                colTransactionLevy.Header.Caption = OrderFields.CAPTION_TRANSACTIONLEVY;

                UltraGridColumn colClearingFee = gridBand.Columns[OrderFields.PROPERTY_CLEARINGFEE];
                colClearingFee.Header.Caption = OrderFields.CAPTION_CLEARINGFEE;

                UltraGridColumn colTaxOnCommissions = gridBand.Columns[OrderFields.PROPERTY_TAXONCOMMISSIONS];
                colTaxOnCommissions.Header.Caption = OrderFields.CAPTION_TAXONCOMMISSIONS;

                UltraGridColumn colMiscFees = gridBand.Columns[OrderFields.PROPERTY_MISCFEES];
                colMiscFees.Header.Caption = OrderFields.CAPTION_MISCFEES;

                UltraGridColumn colSecFee = gridBand.Columns[OrderFields.PROPERTY_SECFEE];
                colSecFee.Header.Caption = OrderFields.CAPTION_SECFEE;

                UltraGridColumn colClosingAlgo = gridBand.Columns[OrderFields.PROPERTY_ClosingAlgoText];
                colClosingAlgo.Header.Caption = OrderFields.CAPTION_ClosingAlgo;

                UltraGridColumn colOptionPremiumAdjustment = gridBand.Columns[OrderFields.PROPERTY_OptionPremiumAdjustment];
                colOptionPremiumAdjustment.Header.Caption = OrderFields.CAPTION_OptionPremiumAdjustment;

                UltraGridColumn colOccFee = gridBand.Columns[OrderFields.PROPERTY_OCCFEE];
                colOccFee.Header.Caption = OrderFields.CAPTION_OCCFEE;

                UltraGridColumn colOrfFee = gridBand.Columns[OrderFields.PROPERTY_ORFFEE];
                colOrfFee.Header.Caption = OrderFields.CAPTION_ORFFEE;

                UltraGridColumn colOtherBrokerFee = gridBand.Columns[OrderFields.PROPERTY_OTHERBROKERFEES];
                colOtherBrokerFee.Header.Caption = OrderFields.CAPTION_OTHERBROKERFEES;

                UltraGridColumn colClearingBrokerFee = gridBand.Columns[OrderFields.PROPERTY_CLEARINGBROKERFEE];
                colClearingBrokerFee.Header.Caption = OrderFields.CAPTION_CLEARINGBROKERFEE;

                UltraGridColumn colSoftCommission = gridBand.Columns[OrderFields.PROPERTY_SOFTCOMMISSION];
                colSoftCommission.Header.Caption = OrderFields.CAPTION_SOFTCOMMISSION;

                UltraGridColumn colTotalCommission = gridBand.Columns[COL_TOTALCOMMISSION];
                colTotalCommission.Header.Caption = CAP_TOTALCOMMISSION;

                UltraGridColumn colNetMoney = gridBand.Columns[COL_NETMONEY];             // net money
                colNetMoney.Header.Caption = CAPTION_NETMONEY_SETTLEMENT;

                UltraGridColumn colNetAmountBase = gridBand.Columns[COL_NETAMOUNTBASE];             // net amount(base)
                colNetAmountBase.Header.Caption = CAPTION_NETAMOUNT_BASE;

                //Added principal amount base and local, PRANA-11379
                UltraGridColumn colPrincipalAmountBase=gridBand.Columns[COL_PRINCIPALAMOUNTBASE]; //Principal Amount(Base)
                colPrincipalAmountBase.Header.Caption = CAPTION_PRINCIPALAMOUNTBASE;

                UltraGridColumn colPrincipalAmountLocal = gridBand.Columns[COL_PRINCIPALAMOUNTLOCAL];  //Principal Amount(Local)
                colPrincipalAmountLocal.Header.Caption = CAPTION_PRINCIPALAMOUNTLOCAL;

                // Added Avg Price(Base) column, PRANA-10775
                UltraGridColumn colAvgPriceBase = gridBand.Columns[COL_AVGPRICEBASE];             
                colAvgPriceBase.Header.Caption = CAPTION_AVGPRICE_BASE;

                UltraGridColumn colSETTLEMENTCURRENCY = gridBand.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID];
                colSETTLEMENTCURRENCY.Header.Caption = OrderFields.CAPTION_SETTLEMENTCURRENCY;
                UltraGridColumn colSettCurrFXRate = gridBand.Columns[OrderFields.PROPERTY_SettCurrFXRate];
                colSettCurrFXRate.Header.Caption = OrderFields.CAPTION_SettCurrFXRate;
                UltraGridColumn colSettCurrFXRateCalc = gridBand.Columns[OrderFields.PROPERTY_SettCurrFXRateCalc];
                colSettCurrFXRateCalc.Header.Caption = OrderFields.CAPTION_SettCurrFXRateCalc;
                UltraGridColumn colSETTLEMENTCURRENCYAMOUNT = gridBand.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT];
                colSETTLEMENTCURRENCYAMOUNT.Header.Caption = OrderFields.CAPTION_SETTLEMENTCURRENCYAMOUNT;
                colSettCurrFXRate.Format = ApplicationConstants.FORMAT_RATE;

                //CHMW-3315	[Allocation] - Add FXRate and FXConversionMethodOperator column on Allocation UI
                UltraGridColumn colFXRate = gridBand.Columns[OrderFields.PROPERTY_FXRate];
                colFXRate.Header.Caption = OrderFields.CAPTION_FXRate;
                colFXRate.Format = ApplicationConstants.FORMAT_RATE;

                UltraGridColumn colFXConversionMethodOperator = gridBand.Columns[OrderFields.PROPERTY_FXConversionMethodOperator];
                colFXConversionMethodOperator.Header.Caption = OrderFields.CAPTION_FXConversionMethodOperator;

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_CHANGETYPE))
                {
                    ValueList ChangeTypeList = new ValueList();
                    List<EnumerationValue> ChangeTypeEnumList = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.ChangeType));
                    foreach (EnumerationValue var in ChangeTypeEnumList)
                    {
                        ChangeTypeList.ValueListItems.Add(var.Value, var.DisplayText);
                    }

                    UltraGridColumn colChangeType = gridBand.Columns[OrderFields.PROPERTY_CHANGETYPE];
                    colChangeType.Header.Caption = OrderFields.CAPTION_CHANGETYPE;
                    colChangeType.Hidden = false;
                    colChangeType.ValueList = ChangeTypeList;
                    colChangeType.CellActivation = Activation.NoEdit;

					gridBand.Columns[OrderFields.PROPERTY_AVGPRICE].Header.Caption = "Avg Price(Local)";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void BindDefaults()
        {
            try
            {
                //cmbbxdefaults.DataSource = null;

                //cmbbxdefaults.DataSource = _defaults.GetDefaultsDataTable();
                //cmbbxdefaults.DataBind();
                //cmbbxdefaults.DisplayMember = "Name";
                //cmbbxdefaults.ValueMember = "ID";
                //cmbbxdefaults.DisplayLayout.Bands[0].Columns["ID"].Hidden = true;
                //cmbbxdefaults.Value = int.MinValue;


                Dictionary<int, string> defaults = new Dictionary<int, string>();
                defaults.Add(-1, "Select");

                lock (lockerObject)
                {
                    foreach (KeyValuePair<int, AllocationOperationPreference> key in _allocationPrefCache.OrderBy(x => x.Value.PositionPrefId).ToList())
                    {
                        defaults.Add(key.Value.OperationPreferenceId, key.Value.OperationPreferenceName);
                    }
                }
                if (defaults.Count > 0)
                {
                    cmbbxdefaults.DataSource = new BindingSource(defaults, null);
                    cmbbxdefaults.DisplayMember = "Value";
                    cmbbxdefaults.ValueMember = "Key";

                    cmbbxdefaults.DisplayLayout.Bands[0].Columns["Key"].Hidden = true;
                    cmbbxdefaults.DisplayLayout.PerformAutoResizeColumns(true, PerformAutoSizeType.AllRowsInBand);
                }
                //if (defaults.Count > 0)
                cmbbxdefaults.Value = -1;
                //else
                // cmbbxdefaults.Value = int.MinValue;
                //cmbbxdefaults_ValueChanged(this, new EventArgs());

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindAllocationScheme()
        {
            try
            {
                DataTable dt = _proxyAllocationServices.InnerChannel.GetAllAllocationSchemeNames();
                DataRow dr = dt.NewRow();
                dr["AllocationSchemeID"] = int.MinValue;
                dr["AllocationSchemeName"] = ApplicationConstants.C_COMBO_SELECT;
                dt.Rows.InsertAt(dr, 0);
                cmbAllocationScheme.DataSource = null;

                cmbAllocationScheme.DataSource = dt;
                cmbAllocationScheme.DataBind();
                cmbAllocationScheme.DisplayMember = "AllocationSchemeName";
                cmbAllocationScheme.ValueMember = "AllocationSchemeID";
                cmbAllocationScheme.DisplayLayout.Bands[0].Columns["AllocationSchemeID"].Hidden = true;
                cmbAllocationScheme.DisplayLayout.Bands[0].Columns["AllocationSchemeName"].Header.Caption = "Allocation Scheme";

                // if combo values are more than one, then by default second value will be selected because first value is one always "-Select-";
                // we get Allocation Schemes from database sorted by date i.e. latest date scheme comes first.
                if (dt.Rows.Count > 1)
                {
                    //2nd row and 2nd column of data table
                    string schemeText = (dt.Rows[1][1].ToString());
                    int schemeID = Convert.ToInt32((dt.Rows[1][0].ToString()));
                    if (!string.IsNullOrEmpty(schemeText) && schemeID > 0)
                    {
                        cmbAllocationScheme.Value = schemeID;
                        cmbAllocationScheme.Text = schemeText;
                    }
                }
                else
                {
                    cmbAllocationScheme.Value = int.MinValue;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetSortKey()
        {
            try
            {
                string unAllocatedSortKey;
                bool unAllocateddescending;
                string allocatedSortKey;
                bool allocatedDescending;
                unAllocatedSortKey = _allocationPrefs.ColumnList.UnAllocatedGridColumns.SortKey;
                unAllocateddescending = !_allocationPrefs.ColumnList.UnAllocatedGridColumns.Ascending;

                allocatedSortKey = _allocationPrefs.ColumnList.AllocatedGridColumns.SortKey;
                allocatedDescending = !_allocationPrefs.ColumnList.AllocatedGridColumns.Ascending;

                if (unAllocatedSortKey != string.Empty)
                {
                    if (grdUnallocated.DisplayLayout.Bands[0].Columns.Exists(unAllocatedSortKey))
                    {
                        grdUnallocated.DisplayLayout.Bands[0].SortedColumns.Clear();
                        grdUnallocated.DisplayLayout.Bands[0].SortedColumns.Add(unAllocatedSortKey, unAllocateddescending);
                    }
                }

                if (allocatedSortKey != string.Empty)
                {
                    if (grdAllocated.DisplayLayout.Bands[0].Columns.Exists(allocatedSortKey))
                    {
                        grdAllocated.DisplayLayout.Bands[0].SortedColumns.Clear();
                        grdAllocated.DisplayLayout.Bands[0].SortedColumns.Add(allocatedSortKey, allocatedDescending);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void BindAccountDefaults()
        {
            //TBC
            try
            {
                //Defaults defaults = AccountManager.GetAccountDefaults(_loginUser.CompanyUserID);
                //defaults.Insert(0, new Default(ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));
                //cmbbxDefaults.DisplayMember = "DefaultName";
                //cmbbxDefaults.ValueMember = "DefaultID";
                //cmbbxDefaults.DataSource = null;
                //cmbbxDefaults.DataSource = defaults;
                //cmbbxDefaults.DataBind();
                //cmbbxDefaults.Value = ApplicationConstants.C_COMBO_SELECT;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region events
        private void grdUnallocated_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                //added fxrate and settlement fxrate in net amount base/settlement calculation when asset is selected in preference, PRANA-12962
                List<int> assetsIDList = AllocationManager.GetInstance().Allocation.InnerChannel.GetDefaultRule(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID).AssetsWithCommissionInNetAmount;
                int sideMul = 1;
                e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                SetUnAllocatedRowProperties(e.Row);
                double NotionalValue = 0;
                if (e.Row.ListObject.GetType().Name.Equals("AllocationGroup"))
                {
                    AllocationGroup group = (AllocationGroup)e.Row.ListObject;
                    NotionalValue = group.CumQty * group.AvgPrice * group.ContractMultiplier;
                    sideMul = Prana.BusinessLogic.PMCalculations.GetSideMultilpier(group.OrderSideTagValue);
                    switch (e.Row.Cells[COL_ASSETNAME].Value.ToString())
                    {
                        case "FixedIncome":
                            if (assetsIDList != null && assetsIDList.Contains(group.AssetID))
                                e.Row.Cells["NetAmount"].Value = (Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value));
                            else
                                e.Row.Cells["NetAmount"].Value = NotionalValue;
                            e.Row.Cells["AssetCategory"].Value = e.Row.Cells[COL_ASSETNAME].Value;
                            break;

                        case "Equity":
                            if (assetsIDList != null && assetsIDList.Contains(group.AssetID))
                                e.Row.Cells["NetAmount"].Value = (Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) );
                            else
                                e.Row.Cells["NetAmount"].Value = NotionalValue;
                            if (e.Row.Cells["IsSwapped"].Value.Equals(true))
                            {
                                string swap = "EquitySwap";
                                e.Row.Cells["AssetCategory"].Value = (object)swap;
                            }
                            else
                            {
                                e.Row.Cells["AssetCategory"].Value = e.Row.Cells[COL_ASSETNAME].Value;
                            }
                            break;

                        default:
                            if (assetsIDList != null && assetsIDList.Contains(group.AssetID))
                                e.Row.Cells["NetAmount"].Value = (Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value));
                            else
                                e.Row.Cells["NetAmount"].Value = NotionalValue;
                            e.Row.Cells["AssetCategory"].Value = e.Row.Cells[COL_ASSETNAME].Value;
                            break;
                    }
                    string conversionOperator = e.Row.Cells[OrderFields.PROPERTY_FXConversionMethodOperator].Value.ToString();
                    double fxrate = Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_FXRate].Value);
                    // changed fxrate calculation condition, so that if conversionOperator is blank then fxrate will remain same, PRANA-11597
                    fxrate = conversionOperator.Equals("D") ? (1 / fxrate) : fxrate;

                    string settlConversionOperator = e.Row.Cells[OrderFields.PROPERTY_SettCurrFXRateCalc].Value.ToString();
                    double settrate = Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_SettCurrFXRate].Value);
                    settrate = settlConversionOperator.Equals("M") ? settrate : (1 / settrate);

                    // calculated net money value,PRANA-10310
                    if (e.Row.Cells.Exists(COL_NETMONEY))
                    {
                        if (assetsIDList != null && assetsIDList.Contains(group.AssetID))
                            e.Row.Cells[COL_NETMONEY].Value = ((Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) * sideMul * settrate)).ToString("#,###0.####");
                        else
                            e.Row.Cells[COL_NETMONEY].Value = ((Convert.ToDouble(e.Row.Cells[COL_NETAMOUNT].Value) + (Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) * sideMul)) * settrate).ToString("#,###0.####");
                    }

                    if (e.Row.Cells.Exists(COL_NETAMOUNTWITHCOMMISSION))
                    {
                        if (assetsIDList != null && assetsIDList.Contains(group.AssetID))
                            e.Row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = ((Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) * sideMul)).ToString("#,###0.####");
                        else
                            e.Row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = (Convert.ToDouble(e.Row.Cells[COL_NETAMOUNT].Value) + (Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) * sideMul)).ToString("#,###0.####");
                    }

                    // Net Amount (Base)
                    if (e.Row.Cells.Exists(COL_NETAMOUNTBASE))
                    {
                        if (assetsIDList != null && assetsIDList.Contains(group.AssetID))
                            e.Row.Cells[COL_NETAMOUNTBASE].Value = ((Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value)) * sideMul * fxrate).ToString("#,###0.####");
                        else
                            e.Row.Cells[COL_NETAMOUNTBASE].Value = ((Convert.ToDouble(e.Row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value)) * fxrate).ToString("#,###0.####");
                    }
                    // Added Avg Price(Base) column, PRANA-10775
                    if (e.Row.Cells.Exists(COL_AVGPRICEBASE))
                    {
                        e.Row.Cells[COL_AVGPRICEBASE].Value = ((Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_AVGPRICE].Value)) * fxrate).ToString("#,0.####");
                    }
                    // Principal Amount base and local added
                    if (e.Row.Cells.Exists(COL_PRINCIPALAMOUNTBASE))
                    {
                        e.Row.Cells[COL_PRINCIPALAMOUNTBASE].Value = (NotionalValue * fxrate).ToString("#,###0.####");
                    }

                    if (e.Row.Cells.Exists(COL_PRINCIPALAMOUNTLOCAL))
                    {
                        e.Row.Cells[COL_PRINCIPALAMOUNTLOCAL].Value = (NotionalValue).ToString("#,###0.####");
                    }

                    bool isMultipleFileNames = false;
                    if (group.Orders.Count > 0)
                    {
                        if (group.Orders[0].ImportFileLogObj != null)
                        {
                            string firstFileName = group.Orders[0].ImportFileLogObj.ImportFileName;
                            foreach (AllocationOrder ord in group.Orders)
                            {
                                if (ord.ImportFileLogObj != null)
                                {
                                    if (String.Compare(ord.ImportFileLogObj.ImportFileName, firstFileName, true) != 0)
                                    {
                                        isMultipleFileNames = true;
                                        break;
                                    }
                                }
                            }
                            if (isMultipleFileNames)
                                e.Row.Cells[COL_ImportFileName].Value = "Multiple Files";
                            else
                                e.Row.Cells[COL_ImportFileName].Value = firstFileName;
                        }
                    }
                }
                //Commenting the code because it was calling multiple times
                //Updating Total No of Trade Field
                //UpdateTotalNoOfTradesForUnallocatedgrd(false);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdAllocated_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                //added fxrate and settlement fxrate in net amount base/settlement calculation when asset is selected in preference, PRANA-12962
                List<int> assetsIDList = AllocationManager.GetInstance().Allocation.InnerChannel.GetDefaultRule(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID).AssetsWithCommissionInNetAmount;
                string conversionOperator = e.Row.Cells[OrderFields.PROPERTY_FXConversionMethodOperator].Value.ToString();
                double fxrate = Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_FXRate].Value);
                // changed fxrate calculation condition, so that if conversionOperator is blank then fxrate will remain same, PRANA-11597
                fxrate = conversionOperator.Equals("D") ? (1 / fxrate) : fxrate;

                string settlConversionOperator = e.Row.Cells[OrderFields.PROPERTY_SettCurrFXRateCalc].Value.ToString();
                double settrate = Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_SettCurrFXRate].Value);
                if (!settlConversionOperator.Equals("M"))
                    settrate = 1/settrate;
                
                //settrate = settlConversionOperator.Equals('M') ? settrate : (1 / settrate);
                string transectionType = string.Empty;

                if (e.Row.Cells.Exists(COL_TransactionType) && e.Row.Cells[COL_TransactionType].Value != null)
                {
                    transectionType = e.Row.Cells[COL_TransactionType].Value.ToString();
                }
                e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                SetAllocatedRowProperties(e.Row);

                int sideMul = 1;
                double NotionalValue = 0;
                if (e.Row.ListObject.GetType().Name.Equals("AllocationGroup"))
                {
                    AllocationGroup group = (AllocationGroup)e.Row.ListObject;
                    NotionalValue = group.CumQty * group.AvgPrice * group.ContractMultiplier;

                    sideMul = Prana.BusinessLogic.PMCalculations.GetSideMultilpier(group.OrderSideTagValue);

                    switch (e.Row.Cells[COL_ASSETNAME].Value.ToString())
                    {
                        //Modified By Pooja Porwal
                        //Date: 12 Jan 2015
                        //Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-5832
                        //Convertiable Bond should also devided 100 as Fixed Income
                        case "FixedIncome":
                        case "ConvertibleBond":
                            if (assetsIDList != null && assetsIDList.Contains(group.AssetID))
                                e.Row.Cells["NetAmount"].Value = (Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value));
                            else
                                e.Row.Cells["NetAmount"].Value = NotionalValue;
                            e.Row.Cells["AssetCategory"].Value = e.Row.Cells[COL_ASSETNAME].Value;
                            break;

                        case "Equity":
                            if (assetsIDList != null && assetsIDList.Contains(group.AssetID))
                                e.Row.Cells["NetAmount"].Value = (Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value));
                            else
                                e.Row.Cells["NetAmount"].Value = NotionalValue;
                            if (e.Row.Cells["IsSwapped"].Value.Equals(true))
                            {
                                string swap = "EquitySwap";
                                e.Row.Cells["AssetCategory"].Value = (object)swap;
                            }
                            else
                            {
                                e.Row.Cells["AssetCategory"].Value = e.Row.Cells[COL_ASSETNAME].Value;
                            }
                            break;

                        default:
                            if (assetsIDList != null && assetsIDList.Contains(group.AssetID))
                                e.Row.Cells["NetAmount"].Value = (Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value));
                            else
                                e.Row.Cells["NetAmount"].Value = NotionalValue;
                            e.Row.Cells["AssetCategory"].Value = e.Row.Cells[COL_ASSETNAME].Value;
                            break;
                    }
                    // Calculated NetAmountWithCommision for each group generated by cost adjustment
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-7026
                    if (CheckTransactionType(transectionType) || group.TransactionSource == TransactionSource.CostAdjustment)
                    {
                        if (e.Row.Cells.Exists(COL_NETAMOUNTWITHCOMMISSION))
                        {
                            if (assetsIDList != null && assetsIDList.Contains(Convert.ToInt32(group.AssetID)))
                                e.Row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = ((Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) * sideMul)).ToString("#,###0.####");
                            else
                                e.Row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = (Convert.ToDouble(e.Row.Cells[COL_NETAMOUNT].Value) + ((Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) ) * sideMul)).ToString("#,###0.####");
                        }
                        if (e.Row.Cells.Exists(COL_NETMONEY))
                        {
                            if (assetsIDList != null && assetsIDList.Contains(Convert.ToInt32(group.AssetID)))
                                e.Row.Cells[COL_NETMONEY].Value = ((Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) * sideMul * settrate)).ToString("#,###0.####");
                            else
                                e.Row.Cells[COL_NETMONEY].Value = ((NotionalValue + (Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) ) * sideMul) * settrate).ToString("#,###0.####");
                }
                        if (e.Row.Cells.Exists(COL_NETAMOUNTBASE))
                        {
                            if (assetsIDList != null && assetsIDList.Contains(Convert.ToInt32(group.AssetID)))
                                e.Row.Cells[COL_NETAMOUNTBASE].Value = ((Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES].Value) * sideMul * fxrate)).ToString("#,###0.####");
                            else
                                e.Row.Cells[COL_NETAMOUNTBASE].Value = (Convert.ToDouble(e.Row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value) * fxrate).ToString("#,###0.####");
                    }
                        // Added Avg Price(Base) column, PRANA-10775
                        if (e.Row.Cells.Exists(COL_AVGPRICEBASE))
                        {
                            e.Row.Cells[COL_AVGPRICEBASE].Value = (Convert.ToDouble(e.Row.Cells[OrderFields.PROPERTY_AVGPRICE].Value) * fxrate).ToString("#,0.####");
                        }
                        //Added principal amount base and local, PRANA-11379
                        if (e.Row.Cells.Exists(COL_PRINCIPALAMOUNTBASE))
                        {
                            e.Row.Cells[COL_PRINCIPALAMOUNTBASE].Value = (NotionalValue * fxrate).ToString("#,###0.####");
                        }

                        if (e.Row.Cells.Exists(COL_PRINCIPALAMOUNTLOCAL))
                        {
                            e.Row.Cells[COL_PRINCIPALAMOUNTLOCAL].Value = (NotionalValue).ToString("#,###0.####");
                        }
                    }
                    else
                    {
                        e.Row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = 0.0000.ToString("0.0000");
                        e.Row.Cells[COL_NETMONEY].Value = 0.0000.ToString("0.0000");
                        e.Row.Cells[COL_NETAMOUNTBASE].Value = 0.0000.ToString("0.0000");
                        e.Row.Cells[COL_AVGPRICEBASE].Value = 0.0000.ToString("0.0000");
                        e.Row.Cells[COL_PRINCIPALAMOUNTBASE].Value = 0.0000.ToString("0.0000");
                        e.Row.Cells[COL_PRINCIPALAMOUNTLOCAL].Value = 0.0000.ToString("0.0000");
                }
                    // calculated net money value,PRANA-10310               
                }


                if (e.Row.ListObject.GetType().Name.Equals("TaxLot"))
                {
                    TaxLot taxlot = (TaxLot)e.Row.ListObject;

                    sideMul = Prana.BusinessLogic.PMCalculations.GetSideMultilpier(taxlot.OrderSideTagValue);

                    // Calculated NetAmountWithCommision for each taxlot generated by cost adjustment
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-7026
                    if (CheckTransactionType(transectionType) || taxlot.TransactionSource == TransactionSource.CostAdjustment)
                    {
                        //Modified By Pooja Porwal
                        //Date: 12 Jan 2015
                        //NetAmount is incurrect at Taxlot level for Fixed Imcome and ConvertibleBond 
                        //Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-5832                  

                        AssetCategory category = (AssetCategory)taxlot.AssetID;

                        //if (category.Equals(AssetCategory.FixedIncome) || category.Equals(AssetCategory.ConvertibleBond))
                        //{
                        //    e.Row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = ((taxlot.AvgPrice / 100) * taxlot.TaxLotQty * taxlot.ContractMultiplier) + (taxlot.TotalCommissionandFees * sideMul);
                        //}
                        //else
                        {
                            if (assetsIDList != null && assetsIDList.Contains(Convert.ToInt32(taxlot.AssetID)))
                                e.Row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = (taxlot.TotalCommissionandFees * sideMul).ToString("#,###0.####");
                            else
                                e.Row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = ((taxlot.AvgPrice * taxlot.TaxLotQty * taxlot.ContractMultiplier) + ((taxlot.TotalCommissionandFees + taxlot.OptionPremiumAdjustment) * sideMul)).ToString("#,###0.####");
                        }
                    }
                    else
                    {
                        e.Row.Cells[COL_NETAMOUNTWITHCOMMISSION].Value = 0.0000.ToString("0.0000");
                    }
                    // calculated net money value at taxlot level,PRANA-10310
                    if (CheckTransactionType(transectionType) || taxlot.TransactionSource == TransactionSource.CostAdjustment)
                    {
                        if (assetsIDList != null && assetsIDList.Contains(Convert.ToInt32(taxlot.AssetID)))
                            e.Row.Cells[COL_NETMONEY].Value = (taxlot.TotalCommissionandFees * sideMul * settrate).ToString("#,###0.####");
                        else
                            e.Row.Cells[COL_NETMONEY].Value = (((taxlot.AvgPrice * taxlot.TaxLotQty * taxlot.ContractMultiplier) + ((taxlot.TotalCommissionandFees + taxlot.OptionPremiumAdjustment) * sideMul)) * settrate).ToString("#,###0.####");
                }
                    else
                        e.Row.Cells[COL_NETMONEY].Value = 0.0000.ToString("0.0000");

                    // Net Amount(Base)// Avg Price(Base) at taxlot level
                    if (CheckTransactionType(transectionType) || taxlot.TransactionSource == TransactionSource.CostAdjustment)
                    {
                        if (assetsIDList != null && assetsIDList.Contains(Convert.ToInt32(taxlot.AssetID)))
                            e.Row.Cells[COL_NETAMOUNTBASE].Value = (taxlot.TotalCommissionandFees * sideMul * fxrate).ToString("#,###0.####");
                        else
                            e.Row.Cells[COL_NETAMOUNTBASE].Value = (((taxlot.AvgPrice * taxlot.TaxLotQty * taxlot.ContractMultiplier) + ((taxlot.TotalCommissionandFees + taxlot.OptionPremiumAdjustment) * sideMul)) * fxrate).ToString("#,###0.####");
                        e.Row.Cells[COL_AVGPRICEBASE].Value = ((taxlot.AvgPrice) * fxrate).ToString("#,0.####");
                }
                    else
                    {
                        e.Row.Cells[COL_NETAMOUNTBASE].Value = 0.0000.ToString("0.0000");
                        e.Row.Cells[COL_AVGPRICEBASE].Value = 0.0000.ToString("0.0000");
                    }
                    //Added principal amount base and local, PRANA-11379
                    if(CheckTransactionType(transectionType) || taxlot.TransactionSource == TransactionSource.CostAdjustment)
                    {
                        e.Row.Cells[COL_PRINCIPALAMOUNTBASE].Value = ((taxlot.AvgPrice * taxlot.TaxLotQty * taxlot.ContractMultiplier) * fxrate).ToString("#,###0.####");
                        e.Row.Cells[COL_PRINCIPALAMOUNTLOCAL].Value = (taxlot.AvgPrice * taxlot.TaxLotQty * taxlot.ContractMultiplier).ToString("#,###0.####");
                    }
                    else
                    {
                        e.Row.Cells[COL_PRINCIPALAMOUNTBASE].Value = 0.0000.ToString("0.0000");
                        e.Row.Cells[COL_PRINCIPALAMOUNTLOCAL].Value = 0.0000.ToString("0.0000");
                    }
                }
                
                
                //Commenting the code because it was calling multiple times
                //Updating Total No of Trade Field
                //UpdateTotalNoOfTradesForAllocatedgrd();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool CheckTransactionType(string transectionType)
        {
            try
            {
                if (
                    transectionType != TradingTransactionType.LongAddition.ToString() &&
                    transectionType != TradingTransactionType.LongWithdrawal.ToString() &&
                    transectionType != TradingTransactionType.ShortAddition.ToString() &&
                    transectionType != TradingTransactionType.ShortWithdrawal.ToString() &&
                    transectionType != TradingTransactionType.LongWithdrawalCashInLieu.ToString() &&
                    transectionType != TradingTransactionType.ShortWithdrawalCashInLieu.ToString()
                    )
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return false;
        }

        //private void rbHistorical_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ClearAllocationData();
        //        //dtToDatePickerAllocation.Enabled = rbHistorical.Checked;
        //        //dtFromDatePickerAllocation.Enabled = rbHistorical.Checked;
        //        //if (!rbHistorical.Checked) // for current data get the latest data
        //        //{
        //        //    GetAllocationData();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        private void ClearAllocationData()
        {
            try
            {
                DialogResult userChoice = DialogResult.Yes;
                if (!_formInilitisedFirstTime)
                {
                    userChoice = PromptForDataSaving(AllocationConstants.ActionAfterSavingData.ClearData);
                }
                if (userChoice != DialogResult.Yes)
                {
                    AllocationManager.GetInstance().ClearData();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void dtToDatePickerAllocation1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                // dtToDatePickerAllocation.
                // System.Threading.Thread.Sleep(1000);
                ClearAllocationData();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void dtFromDatePickerAllocation1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ClearAllocationData();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void ToEditor_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                ClearAllocationData();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void FromEditor_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                ClearAllocationData();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region IAllocation Members

        public System.Windows.Forms.Form Reference()
        {
            return this;
        }

        public event EventHandler LaunchPreferences;

        public event EventHandler GenrateCashTransaction;

        public event EventHandler AllocationClosed;

        public event EventHandler LaunchCommissionCalculation;

        public Prana.BusinessObjects.CompanyUser loginUser
        {
            get
            {
                return _loginUser;
            }
            set
            {
                _loginUser = value;
            }
        }
        //ProxyBase<IClosingServices> _closingServices = null;
        //public ProxyBase<IClosingServices> ClosingServices
        //{
        //    set
        //    {
        //        _closingServices = value;

        //    }

        //}

        #region Data members for filtering operation
        List<String> unallocatedFilterColList = new List<string>();
        List<String> allocatedFilterColList = new List<string>();
        FilterGrid _filterGridAllocated;
        FilterGrid _filterGridUnAllocated;
        FilterGrid _combinedFilter;

        #endregion

        /// <summary>
        /// This function assign column names for filterGrids _filterGrid and _filterGridUnAllocated.
        /// Later these column names are fed to filterGrid. On the basis of these columns user can filter the results.
        /// Using filter will avoid fetching of non-required data to systems, thus reducing memory usage.
        /// </summary>
        private void AddFiltersToLists()
        {
            try
            {
                #region Unallocated Filter column List

                unallocatedFilterColList.Add("Symbol");
                unallocatedFilterColList.Add("Side");
                unallocatedFilterColList.Add(ApplicationConstants.CONST_BROKER);
                unallocatedFilterColList.Add("Venue");
                unallocatedFilterColList.Add("Trading Account");
                //unallocatedFilterColList.Add("User");
                unallocatedFilterColList.Add("Asset");
                unallocatedFilterColList.Add("Currency");
                unallocatedFilterColList.Add("Exchange");
                unallocatedFilterColList.Add("PreAllocated");
                unallocatedFilterColList.Add("Manual Group");
                unallocatedFilterColList.Add("Underlying");

                #endregion

                #region Allocated Filter column List

                allocatedFilterColList.Add("Symbol");
                allocatedFilterColList.Add("Side");
                allocatedFilterColList.Add(ApplicationConstants.CONST_BROKER);
                allocatedFilterColList.Add("Venue");
                allocatedFilterColList.Add("Trading Account");
                //allocatedFilterColList.Add("User");
                allocatedFilterColList.Add("Asset");
                allocatedFilterColList.Add("Currency");
                allocatedFilterColList.Add("Exchange");
                allocatedFilterColList.Add("PreAllocated");
                allocatedFilterColList.Add("Manual Group");
                allocatedFilterColList.Add("Underlying");
                allocatedFilterColList.Add("Account Name");
                allocatedFilterColList.Add("Strategy Name");

                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set { _securityMaster = value; }
        }
        public void SetUp()
        {
            try
            {
                CreateAllocationServicesProxy();
                CreateClosingServicesProxy();
                //AYG:June 13, 2013, set cash management services to amendment control, previously it was set from Main commission form
                SetCashManagementServicesToAmendmentControl();
                // loads commision rules               
                //PostTradeCacheManager.Initlise(_loginUser);
                //_allocationServices.Initlise(_loginUser);
                //AllocationStaticCollection.Start();
                AllocationPreferencesManager.SetUp(loginUser.CompanyUserID);
                //TODO
                //ClosingCommonCacheManager.Instance.GetTaxlotsLatestCADates();
                //ClosingCommonCacheManager.Instance.GetTaxlotsLatestClosingDates();
                //ClosingCommonCacheManager.Instance.GetExcercisedTaxlots();
                _allocationPrefs = AllocationPreferencesManager.AllocationPreferences;
                //_closingPrefs = ClosingPrefManager.ClosingPreferences;


                LoadAllocationPreferences();
                // _allocationPrefCache.Add(1, new AllocationOperationPreference(1, 5, "Test", new AllocationRule(), DateTime.Now));


                LoadAllocationModuleUI();

                //Load Allocation layout,PRANA-11824
                LoadLayout(true);
                LoadLayout(false);

                SetPreferences();
                // gets allocation data from post trade cachemanager
                //GetAllocationData();
                //formInilitisedFirstTime = false;
                //_closingServices.InnerChannel.GetClosingandCAData();
                //AllocationManager.GetInstance().SetUI(this);

                AddSideSortingCriteria();
                //call control amendment
                SetProxyToControlAmendment();
                #region settlement currency fields
                //CHMW-3037 [Foreign Positions Settling in Base Currency] Add settlement currency fields in allocation module
                //foreach (UltraGridBand band in grdAllocated.DisplayLayout.Bands)
                //{
                //    if (band != null && band.Columns != null)
                //    {
                //        AddSettlementCurrencyColumnsAndBindEnum(band.Columns);
                //        AddChangeTypeAndBindEnum(band.Columns);
                //    }
                //}
                //foreach (UltraGridBand band in grdUnallocated.DisplayLayout.Bands)
                //{
                //    if (band != null && band.Columns != null)
                //    {
                //        AddSettlementCurrencyColumnsAndBindEnum(band.Columns);
                //        AddChangeTypeAndBindEnum(band.Columns);
                //    }
                //}
                #endregion
                ApplyColorScheme();
            }
            catch (Exception ex)
                {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                    }
                }

        private void ApplyColorScheme()
                {
            try
                    {
                if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME,
                        CustomThemeHelper.THEME_STYLESETNAME_BLOTTER);
                }
                else
                {
                    SetButtonsColor();
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME,
                        CustomThemeHelper.THEME_STYLESETNAME_ALLOCATION_MAIN);
                    if (CustomThemeHelper.ApplyTheme)
                    {
                        this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"Text-align:Left\">" +
                                                                           CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                        this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text,
                            CustomThemeHelper.UsedFont);
                        this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                        this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                        this.lblStatusStrip.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                        this.lblStatusStrip.ForeColor = System.Drawing.Color.WhiteSmoke;
                        this.lblStatusStrip.Font = new Font("Century Gothic", 9F);
                        this.statusLblDateTime.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                        this.statusLblDateTime.ForeColor = System.Drawing.Color.WhiteSmoke;
                        this.statusLblDateTime.Font = new Font("Century Gothic", 9F);
                        this.lblStatusStripProgress.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                        this.lblStatusStripProgress.ForeColor = System.Drawing.Color.WhiteSmoke;
                        this.lblStatusStripProgress.Font = new Font("Century Gothic", 9F);
                    }
                    CustomThemeHelper.SetThemeProperties(accountOnlyUserControl1, CustomThemeHelper.THEME_STYLELIBRARYNAME,
                        CustomThemeHelper.THEME_STYLESETNAME_ALLOCATION_FUND_ONLY_CTRL);
                    CustomThemeHelper.SetThemeProperties(accountStrategyMapping1, CustomThemeHelper.THEME_STYLELIBRARYNAME,
                        CustomThemeHelper.THEME_STYLESETNAME_ALLOCATION_FUND_STRATEGY_CTRL);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
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
                btnReAllocate.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnReAllocate.ForeColor = System.Drawing.Color.White;
                btnReAllocate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnReAllocate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnReAllocate.UseAppStyling = false;
                btnReAllocate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAllocate.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAllocate.ForeColor = System.Drawing.Color.White;
                btnAllocate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAllocate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAllocate.UseAppStyling = false;
                btnAllocate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClear.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnClear.ForeColor = System.Drawing.Color.White;
                btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClear.UseAppStyling = false;
                btnClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSaveSwap.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSaveSwap.ForeColor = System.Drawing.Color.White;
                btnSaveSwap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSaveSwap.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSaveSwap.UseAppStyling = false;
                btnSaveSwap.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSwapClose.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnSwapClose.ForeColor = System.Drawing.Color.White;
                btnSwapClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSwapClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSwapClose.UseAppStyling = false;
                btnSwapClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSaveWOState.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSaveWOState.ForeColor = System.Drawing.Color.White;
                btnSaveWOState.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSaveWOState.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSaveWOState.UseAppStyling = false;
                btnSaveWOState.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
               
                btnAutoGrp.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAutoGrp.ForeColor = System.Drawing.Color.White;
                btnAutoGrp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAutoGrp.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAutoGrp.UseAppStyling = false;
                btnAutoGrp.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetAllocationData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetAllocationData.ForeColor = System.Drawing.Color.White;
                btnGetAllocationData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetAllocationData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetAllocationData.UseAppStyling = false;
                btnGetAllocationData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnDelete.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnDelete.ForeColor = System.Drawing.Color.White;
                btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDelete.UseAppStyling = false;
                btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCheckSide.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnCheckSide.ForeColor = System.Drawing.Color.White;
                btnCheckSide.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCheckSide.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCheckSide.UseAppStyling = false;
                btnCheckSide.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClosing.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClosing.ForeColor = System.Drawing.Color.White;
                btnClosing.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClosing.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClosing.UseAppStyling = false;
                btnClosing.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCancelData.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnCancelData.ForeColor = System.Drawing.Color.White;
                btnCancelData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCancelData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCancelData.UseAppStyling = false;
                btnCancelData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClearAllocated.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnClearAllocated.ForeColor = System.Drawing.Color.White;
                btnClearAllocated.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClearAllocated.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClearAllocated.UseAppStyling = false;
                btnClearAllocated.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void LoadAllocationPreferences()
        {
            try
            {

                lock (_allocationPrefCache)
                {
                    _allocationPrefCache.Clear();
                }
                List<AllocationOperationPreference> allocationOperationPrefList = new List<AllocationOperationPreference>();
                allocationOperationPrefList.AddRange(AllocationManager.GetInstance().Allocation.InnerChannel.GetPreferenceByCompanyId(CommonDataCache.CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID));
                foreach (AllocationOperationPreference pref in allocationOperationPrefList)
                {
                    if (!pref.OperationPreferenceName.StartsWith("*Custom#_") && !pref.OperationPreferenceName.StartsWith("*WorkArea#_") && !pref.OperationPreferenceName.StartsWith("*PST#_"))
                    {
                        lock (_allocationPrefCache)
                        {
                            if (_allocationPrefCache.ContainsKey(pref.OperationPreferenceId))
                            {
                                _allocationPrefCache[pref.OperationPreferenceId] = pref;
                            }
                            else
                            {
                                _allocationPrefCache.Add(pref.OperationPreferenceId, pref);
                            }
                        }
                    }
                }
                btnSaveWOState.Visible = _allocationPrefs.GeneralRules.IncludeSavewtoutState;
                btnSave.Visible = _allocationPrefs.GeneralRules.IncludeSavewtState;
                //If both set to unchecked, show only Save With State
                if (!_allocationPrefs.GeneralRules.IncludeSavewtoutState && !_allocationPrefs.GeneralRules.IncludeSavewtState)
                    btnSave.Visible = true;

                //Reposition buttons
                if (_allocationPrefs.GeneralRules.IncludeSavewtoutState == false)
                    btnSave.Location = btnSaveWOState.Location;
                else
                    btnSave.Location = new Point(btnSaveWOState.Location.X - btnSaveWOState.Size.Width - 10, btnSave.Location.Y);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //AYG: 13 June 2013, as control moved to Allocation main
        private void SetProxyToControlAmendment()
        {
            try
            {
                ctrlAmendmend1.SecurityMaster = _securityMaster;
                ctrlAmendmend1.InvokeSecurityMaster();
                ctrlAmendmend1.CurrentUser = loginUser;
                ctrlAmendmend1.SetGridDataSources();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetCashManagementServicesToAmendmentControl()
        {
            try
            {
                ctrlAmendmend1.CashManagementServices = AllocationManager.GetInstance().CashManagementServices;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AddSideSortingCriteria()
        {
            try
            {
                _sideSortingList.Add(FIXConstants.SIDE_Buy);
                _sideSortingList.Add(FIXConstants.SIDE_Buy_Open);
                _sideSortingList.Add(FIXConstants.SIDE_Sell);
                _sideSortingList.Add(FIXConstants.SIDE_Sell_Closed);
                _sideSortingList.Add(FIXConstants.SIDE_SellShort);
                _sideSortingList.Add(FIXConstants.SIDE_Sell_Open);
                _sideSortingList.Add(FIXConstants.SIDE_Buy_Closed);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void UserPermissionSetUp(int readOrReadwrite)
        {
            //will be used when allocation read write permission will be implemented
            // throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region Button Click
        private void btnReAllocate_Click_1(object sender, EventArgs e)
        {
            try
            {
                ClearLabelStrip();
                if (_selectedGrid == null)
                    return;

                bool shouldRefreshSecondRow = false;
                string allocationSchemeName = string.Empty;
                allocationSchemeName = SetAllocationSchemeName();
                List<AllocationGroup> groups = GetSelectedGroups(grdUnallocated);
                {
                    if (allocationSchemeName.Equals(string.Empty) || allocationSchemeName.Equals(ApplicationConstants.C_COMBO_SELECT))
                    {
                        ReallocateByAccount(shouldRefreshSecondRow);
                    }
                    else
                    {
                        ReallocateBySymbol(shouldRefreshSecondRow, allocationSchemeName);
                        ClearAllocatedAccountNumberControl(string.Empty);
                    }
                }

                //Calculating the selectedGrid row in the grid AddAndUpdateExternalTransactionID then Update trade counter
                UltraGridRow[] rows = grdAllocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsAllocatedGrd = Convert.ToInt32(rows.Count(row => row.Cells["checkBox"].Text == true.ToString()));

                UltraGridRow[] rows1 = grdUnallocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsUnallocatedGrd = Convert.ToInt32(rows1.Count(row1 => row1.Cells["checkBox"].Text == true.ToString()));

                //Updating Total No of Trade Field
                UpdateTotalNoOfTradesForAllocatedgrd(_countSelectedRowsAllocatedGrd);
                UpdateTotalNoOfTradesForUnallocatedgrd(_countSelectedRowsUnallocatedGrd);
                grdAllocated.ActiveRow = null;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ReallocateByAccount(bool shouldRefreshSecondRow)
        {
            try
            {
                bool isMultipleSelected = false;
                List<AllocationGroup> groups = GetSelectedGroups(_selectedGrid);

                #region AllocationOperationPref

                bool isChanged = false;
                AllocationOperationPreference pref = null;
                if (cmbbxdefaults.Value != null && Convert.ToInt32(cmbbxdefaults.Value) != -1)
                {
                    int id = (int)cmbbxdefaults.Value;
                    if (_allocationPrefCache.ContainsKey(id))
                        pref = _allocationPrefCache[id].Clone();
                }
                else
                {
                    isChanged = true;
                    pref = new AllocationOperationPreference();
                    UpdateAllocationDefaultRule(pref);
                    lock (_strategyPergentage)
                    {
                        //Checks if percentage count is greater than zero or not.
                        if (_strategyPergentage.Count > 0)
                            pref.TryUpdateTargetPercentage(_strategyPergentage);
                        else
                            pref.TryUpdateTargetPercentage(accountStrategyMapping1.GetAllocationAccountValue());
                    }

                }

                #endregion
                //Added to reallocate highlighted row, PRANA-10754
                if (groups.Count == 0 && _selectedGrid.ActiveRow != null && _selectedGrid.ActiveRow.ListObject is AllocationGroup)
                    groups.Add((AllocationGroup)_selectedGrid.ActiveRow.ListObject);

                if (_allocationPrefs.GeneralRules.AllocateBasedonOpenPositions && groups.Count > 0)
                {
                    //Sandeep: Create Accountwise open positions Cache on Trade Server
                    //CreateOpenPositionsCacheOnTradeServer(groups, false);
                    //update cache Trade server side
                    // UpdateOpenPositionsCacheOnTradeServer(groups, selectedGrid.Name);
                }

                if (groups.Count == 1)
                {
                    UltraGridRow[] rows = _selectedGrid.Rows.GetFilteredInNonGroupByRows();
                    foreach (UltraGridRow row in rows)
                    {
                        if (row.Cells["checkBox"].Text == true.ToString())
                        {
                            row.Activate();
                            if (_selectedGrid.ActiveRow.Band.Index != 1)
                            {
                                _selectedGrid.ActiveRow = row;
                            }
                            break;
                        }
                    }
                    isMultipleSelected = false;
                }
                else
                {
                    isMultipleSelected = true;
                }

                // this allocation group list is used to collect groups which are not allocated based on latest positions and put on hold status
                List<AllocationGroup> groupListWithHoldStatus = new List<AllocationGroup>();

                string result = string.Empty;
                List<AllocationGroup> allocationGroupList = new List<AllocationGroup>();
                foreach (AllocationGroup group in groups)
                {
                    if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        shouldRefreshSecondRow = true;
                    }

                    AllocationGroup groupObj = group;

                    // use in open postions based allocation
                    GetGroupsWithHoldState(groupListWithHoldStatus, groupObj);
                    if (result == string.Empty)
                    {
                        group.AllocationSchemeName = string.Empty;
                        group.AllocationSchemeID = 0;
                        group.ErrorMessage = string.Empty;
                        group.GroupAllocationStatus = GroupAllocationStatus.Allocated;

                        allocationGroupList.Add(group);
                        if (_selectedGrid.ActiveRow != null)
                        {
                            try
                            {
                                if (shouldRefreshSecondRow)
                                    _selectedGrid.ActiveRow.ChildBands[1].Rows.Refresh(RefreshRow.ReloadData);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        lblStatusStrip.Text = "Data allocated by account.";
                    }
                    else
                    {
                        lblStatusStrip.Text = result;
                    }
                }
                string response = "";
                if (allocationGroupList.Count > 0)
                {
                    DialogResult choice = DialogResult.No;
                    bool isAnyCommissionSourceManual = false;

                    //show message to calculate commission again only if any of selected groups have commission or soft commission source as manual, PRANA-12920
                    Parallel.ForEach(allocationGroupList, (group, state) =>
                        {
                            if (group.CommSource == CommisionSource.Manual || group.SoftCommSource == CommisionSource.Manual || group.CommissionSource == (int)CommisionSource.Manual || group.SoftCommissionSource == (int)CommisionSource.Manual)
                            {
                                isAnyCommissionSourceManual = true;
                                state.Break();
                            }
                        });

                    if (isAnyCommissionSourceManual)
                    {
                        choice = MessageBox.Show(this, "For some groups commission is specified manually, Would you like to calculate commission again for these groups?", "Nirvana Allocation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        Parallel.ForEach(allocationGroupList, group =>
                        {
                            if (choice == DialogResult.Yes)
                            {
                                group.IsRecalculateCommission = true;
                            }
                            else
                            {
                                group.IsRecalculateCommission = false;
                            }
                        }
                        );
                    }
                    response = AllocationManager.GetInstance().AllocateGroup(allocationGroupList, pref, true, isChanged, IsForceAllocation());
                }
                if (!string.IsNullOrWhiteSpace(response))
                {
                    lblStatusStrip.Text = response.Contains("\n") ? response.Substring(0, response.IndexOf("\n") - 1) : response;
                    MessageBox.Show(this, response, "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // use in open postions based allocation
                // if there are some groups which are not allocated due to wrong side,we put them on hold and 
                // these groups will be allocated in the second round
                result = ReAllocateToAvoidBoxedPositions(isMultipleSelected, groupListWithHoldStatus, result);
                ClearAllocatedAccountNumberControl(result);

                // Clear Accountwise latest positions Cache on Trade Server after Auto Allocate Closing Transactions                
                ClearOpenPositionsCacheOnTradeServer(groups.Count);
                _isRowClicked = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
            }
        }

        private void ReallocateBySymbol(bool shouldRefreshSecondRow, string allocationSchemeName)
        {
            try
            {
                List<AllocationGroup> groups = GetSelectedGroups(_selectedGrid);

                //condition added to reallocate highlighted row, PRANA-10754
                if (groups.Count == 0 && _selectedGrid.ActiveRow != null && _selectedGrid.ActiveRow.ListObject is AllocationGroup)
                    groups.Add((AllocationGroup)_selectedGrid.ActiveRow.ListObject);
                if (groups.Count > 0)
                {
                    //show message to calculate commission again only if any of selected groups have commission or soft commission source as manual, PRANA-13009
                    bool isAnyCommissionSourceManual = false;
                    Parallel.ForEach(groups, (group, state) =>
                    {
                        if (group.CommSource == CommisionSource.Manual || group.SoftCommSource == CommisionSource.Manual || group.CommissionSource == (int)CommisionSource.Manual || group.SoftCommissionSource == (int)CommisionSource.Manual)
                        {
                            isAnyCommissionSourceManual = true;
                            state.Break();
                        }
                    });

                    if (isAnyCommissionSourceManual)
                    {
                        DialogResult choice = MessageBox.Show(this, "For some groups commission is specified manually, Would you like to calculate commission again for these groups?", "Nirvana Allocation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        Parallel.ForEach(groups, group =>
                        {
                            if (choice == DialogResult.Yes)
                                group.IsRecalculateCommission = true;
                            else
                                group.IsRecalculateCommission = false;
                        }
                        );
                    }

                    foreach (AllocationGroup group in groups)
                    {
                        group.ErrorMessage = string.Empty;
                        if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                        {
                            shouldRefreshSecondRow = true;
                        }
                        AllocationLevelList accounts = new AllocationLevelList();
                        bool isSwap = false;
                        string result = _proxyAllocationServices.InnerChannel.ValidateAllocationAccountsByAllocationScheme(group, ref accounts, allocationSchemeName, ref isSwap, _allocationPrefs.GeneralRules.IsPariPassuAllocation, _allocationPrefs.GeneralRules.SelectedAccountID, _allocationPrefs.GeneralRules.isMasterFundRatioAllocation);
                        if (result == string.Empty)
                        {
                            SetSwapParameters(isSwap, group, allocationSchemeName, accounts);
                            if (_selectedGrid.ActiveRow != null)
                            {
                                try
                                {
                                    if (shouldRefreshSecondRow)
                                        _selectedGrid.ActiveRow.ChildBands[1].Rows.Refresh(RefreshRow.ReloadData);
                                }
                                catch (Exception)
                                {

                                }
                            }
                            lblStatusStrip.Text = "Data allocated by selected scheme.";
                        }
                        else
                        {
                            group.ErrorMessage = result;
                            lblStatusStrip.Text = result;
                        }
                    }
                    //if all the groups reallocated successfully then only Allocation Methodology default will be Account
                    //if (groups.Count > 0)
                    SetAllocationMethodologyByAccount();
                    _isRowClicked = false;
                }
                else
                {
                    lblStatusStrip.Text = "Please Select trade/group(s) to allocate. ";
                    return;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetSwapParameters(bool isSwap, AllocationGroup group, string allocationSchemeName, AllocationLevelList accounts)
        {
            try
            {
                if (isSwap)
                {
                    SwapParameters swapParameters = GetDefaultSwapParameters(group.AvgPrice, group.AUECLocalDate, group.CumQty);
                    if (swapParameters != null)
                    {
                        group.IsSwapped = true;
                        group.SwapParameters = swapParameters;
                    }
                }
                group.AllocationSchemeName = allocationSchemeName;
                group.AllocationSchemeID = Convert.ToInt32(cmbAllocationScheme.Value);
                List<AllocationGroup> groupList = new List<AllocationGroup>();
                groupList.Add(group);
                AllocationOperationPreference pref = new AllocationOperationPreference();
                SerializableDictionary<int, AccountValue> accountValue = new SerializableDictionary<int, AccountValue>();
                foreach (AllocationLevelClass lclass in accounts.Collection)
                {
                    AccountValue val = new AccountValue();
                    val.AccountId = lclass.LevelnID;

                    /* Using allocated quantity to recalculate %age for accounts
                     * This method is called from allocation scheme in which quantity is already calculated at server side PostTradeCacheManager
                     * Percentage is not used because of precision loss from double to float
                     * See JIRA http://jira.nirvanasolutions.com:8080/browse/PRANA-5775
                     * 
                     * Base type is selected as TradeQuantity
                     * RuleType is None so that every allocation group is allocated without the impact of existing state
                     * Preference account is None
                     * Matching portfolio is true to close the position of required
                     * 
                     * In this method one allocation group is allocated at a time
                     * Changed is passed as true so that parameter to use instead of preference
                     */
                    val.Value = ((decimal)lclass.AllocatedQty * 100M) / (decimal)group.CumQty;

                    /* Strategy allocation is currently not supported in allocation scheme but  keeping it for future implementation
                     * as requirement for this is already in JIRA http://jira.nirvanasolutions.com:8080/browse/MAPLEROCK-36
                     */
                    if (lclass.AllocatedQty > 0 && lclass.Childs != null)
                    {
                        List<StrategyValue> strategyValue = new List<StrategyValue>();
                        foreach (AllocationLevelClass sClass in lclass.Childs.Collection)
                        {
                            StrategyValue sval = new StrategyValue();
                            sval.StrategyId = lclass.LevelnID;
                            sval.Value = ((decimal)sClass.AllocatedQty * 100M) / (decimal)lclass.AllocatedQty;// (decimal)lclass.Percentage;
                            strategyValue.Add(sval);
                        }
                        val.StrategyValueList = strategyValue;
                    }
                    if (accountValue.ContainsKey(val.AccountId))
                        accountValue[val.AccountId] = val;
                    else
                        accountValue.Add(val.AccountId, val);

                }
                pref.TryUpdateTargetPercentage(accountValue);
                pref.DefaultRule.BaseType = Prana.Allocation.Common.Enums.AllocationBaseType.CumQuantity;
                pref.DefaultRule.RuleType = Prana.Allocation.Common.Enums.MatchingRuleType.None;
                pref.DefaultRule.MatchPortfolioPosition = true;
                pref.DefaultRule.PreferenceAccountId = -1;

                string response = "";
                if (groupList.Count > 0)
                    response = AllocationManager.GetInstance().AllocateGroup(groupList, pref, true, true, IsForceAllocation());
                if (!string.IsNullOrWhiteSpace(response))
                {
                    lblStatusStrip.Text = response.Contains("\n") ? response.Substring(0, response.IndexOf("\n") - 1) : response;
                    MessageBox.Show(this, response, "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        [Obsolete]
        private string ValidationResult(ref AllocationGroup group, ref AllocationLevelList accounts, bool isMultipleSelected, bool isPercentageChanged, bool isQtyChanged)
        {
            string result = string.Empty;
            try
            {
                PostTradeEnums.Status groupStatus = PostTradeEnums.Status.None;
                //Only isQtyChanged is checked here, basically whenever percentage changes, a function named accountStrategyMapping1_CheckTotalQty is called and
                // it checks group validation. So There is no need to check group's status if percantage changes i.e. isPercentageChanged
                if (isQtyChanged && !isMultipleSelected)
                {
                    groupStatus = _closingServices.InnerChannel.CheckGroupStatus(group);
                }
                if (groupStatus.Equals(PostTradeEnums.Status.None))
                {
                    if ((!isPercentageChanged && !isQtyChanged) || isMultipleSelected || _allocationPrefs.GeneralRules.AllocateBasedonOpenPositions)
                    {
                        result = _proxyAllocationServices.InnerChannel.ValidateAllocationAccounts(ref group, ref accounts, _allocationPrefs.GeneralRules.AllocateBasedonOpenPositions, IsForceAllocation(), _allocationPrefs.GeneralRules.IsPariPassuAllocation, _allocationPrefs.GeneralRules.SelectedAccountID, _loginUser.CompanyUserID);
                    }
                    else if (!isMultipleSelected)
                    {
                        decimal allowedQty = 0;
                        foreach (AllocationLevelClass account in accounts.Collection)
                        {
                            allowedQty += Convert.ToDecimal(account.AllocatedQty);
                            if (account.Childs != null)
                            {
                                decimal strategyPercentage = 0;
                                decimal strategyQty = 0;
                                foreach (AllocationLevelClass strategy in account.Childs.Collection)
                                {
                                    strategyQty += Convert.ToDecimal(strategy.AllocatedQty);
                                    strategyPercentage += Convert.ToDecimal(strategy.Percentage);
                                }
                                if (!strategyPercentage.EqualsPrecise(100M) || !strategyQty.EqualsPrecise(account.AllocatedQty))
                                {
                                    result = "Sum of Strategy percentage should be 100!";
                                }
                            }
                        }
                        if (!allowedQty.EqualsPrecise(group.CumQty))
                        {
                            result = "Sum of Accounts percentage should be 100!";
                        }
                    }
                }
                else
                {
                    if (groupStatus.Equals(PostTradeEnums.Status.Closed))
                    {
                        result = "Group is Partially or Fully Closed. Can't be Allocated Again";
                    }
                    else if (groupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                    {
                        result = "First undo the applied corporate action to make any changes.";
                    }
                    else if (groupStatus.Equals(PostTradeEnums.Status.Exercise))
                    {
                        result = "Group is generated by Exercise";
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        private string SetAllocationSchemeName()
        {
            string allocationSchemeName = string.Empty;
            try
            {
                if (rbtnAllocationBySymbol.CheckedIndex == 0)
                {
                    allocationSchemeName = cmbAllocationScheme.Text;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return allocationSchemeName;
        }

        private void btnAllocate_Click_1(object sender, EventArgs e)
        {
            try
            {
                ClearLabelStrip();
                if (_selectedGrid == null)
                    return;
                string allocationSchemeName = string.Empty;
                allocationSchemeName = SetAllocationSchemeName();

                if (allocationSchemeName.Equals(string.Empty) || allocationSchemeName.Equals(ApplicationConstants.C_COMBO_SELECT))
                {
                    AllocateByAccount();
                }
                else
                {
                    AllocateBySymbol(allocationSchemeName);
                    ClearUnAllocatedAccountNumberControl(string.Empty);
                }

                //Calculating the selectedGrid row in the grid AddAndUpdateExternalTransactionID then Update trade counter
                UltraGridRow[] rows = grdAllocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsAllocatedGrd = Convert.ToInt32(rows.Count(row => row.Cells["checkBox"].Text == true.ToString()));

                UltraGridRow[] rows1 = grdUnallocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsUnallocatedGrd = Convert.ToInt32(rows1.Count(row1 => row1.Cells["checkBox"].Text == true.ToString()));

                //Updating Total No of Trade Field
                UpdateTotalNoOfTradesForAllocatedgrd(_countSelectedRowsAllocatedGrd);
                UpdateTotalNoOfTradesForUnallocatedgrd(_countSelectedRowsUnallocatedGrd);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This function is used to create accountwise open positions caches on Trade Server.
        /// This is called on every click of allocate button 
        /// It will create cache only when "Allocation Based on Latest Position" option will be selected from Allocation Preferences
        /// </summary>      
        /// <param name="groups"></param>
        private void CreateOpenPositionsCacheOnTradeServer(List<AllocationGroup> groups, bool isFromUnAllocated)
        {
            try
            {
                AllocationLevelList accounts = new AllocationLevelList();
                if (isFromUnAllocated)
                {
                    accounts = _selectedUnAllocatedCtrl.GetAllocationAccounts(groups[0]);
                }
                else
                {
                    accounts = accountStrategyMapping1.GetAllocationAccounts(groups[0]);
                }
                if (accounts.Collection.Count > 0)
                {
                    StringBuilder accountIDs = new StringBuilder();
                    for (int i = 0; i < accounts.Collection.Count; i++)
                    {
                        accountIDs.Append(accounts.Collection[i].LevelnID);
                        accountIDs.Append(",");
                    }
                    if (accountIDs.Length > 0)
                    {
                        accountIDs.Remove((accountIDs.Length - 1), 1);
                    }
                    // Create user and Account wise open positions Cache on Trade Server
                    _proxyAllocationServices.InnerChannel.GetOpenPositionsForAllocation(accountIDs.ToString(), _loginUser.CompanyUserID);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update cache for allocated but unsaved groups
        /// </summary>
        /// <param name="groups"></param>
        private void UpdateOpenPositionsCacheOnTradeServer(List<AllocationGroup> groups, string selectedgrdName)
        {
            try
            {
                Dictionary<string, AllocationGroup> allocatedUnSavedgroups = AllocationManager.GetInstance().GetAllocatedUnSavedGroups();
                if (allocatedUnSavedgroups.Count > 0)
                {
                    if (selectedgrdName.Equals(AllocationConstants.AllocationGrid.grdAllocated.ToString()))
                    {
                        foreach (AllocationGroup group in groups)
                        {
                            if (allocatedUnSavedgroups.ContainsKey(group.GroupID))
                            {
                                allocatedUnSavedgroups.Remove(group.GroupID);
                            }
                        }
                    }
                }
                if (allocatedUnSavedgroups.Count > 0)
                {
                    _proxyAllocationServices.InnerChannel.UpdateOpenPositionsCacheOnTradeServer(allocatedUnSavedgroups, _loginUser.CompanyUserID);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This function is used to clear open positions cache after allocation
        /// Cache is created on every allocate button, whether all trades allocated or not, Cache will be cleared
        /// </summary>
        private void ClearOpenPositionsCacheOnTradeServer(int groupCount)
        {
            try
            {
                if (_allocationPrefs.GeneralRules.AllocateBasedonOpenPositions)
                {
                    if (groupCount > 0)
                    {
                        _proxyAllocationServices.InnerChannel.ClearCacheUsedInOpenPositionsForAllocation(_loginUser.CompanyUserID);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AllocateByAccount()
        {
            try
            {
                bool isMultipleSelected = false;
                List<AllocationGroup> groups = GetSelectedGroups(_selectedGrid);

                //Added to allocate highlighted row, PRANA-10754
                if (groups.Count == 0 && _selectedGrid.ActiveRow != null && _selectedGrid.ActiveRow.ListObject is AllocationGroup)
                    groups.Add((AllocationGroup)_selectedGrid.ActiveRow.ListObject);

                #region AllocationOperationPref

                bool isChanged = false;
                AllocationOperationPreference pref = null;
                if (cmbbxdefaults.Value != null && Convert.ToInt32(cmbbxdefaults.Value) != -1)
                {
                    int id = (int)cmbbxdefaults.Value;
                    if (_allocationPrefCache.ContainsKey(id))
                        pref = _allocationPrefCache[id].Clone();
                }
                else
                {
                    isChanged = true;
                    pref = new AllocationOperationPreference();
                    UpdateAllocationDefaultRule(pref);
                    if (_targetPergentage.Count > 0)
                        pref.TryUpdateTargetPercentage(_targetPergentage);
                    else
                        pref.TryUpdateTargetPercentage(_selectedUnAllocatedCtrl.GetAllocationAccountValue());

                }

                #endregion
                if (_allocationPrefs.GeneralRules.AllocateBasedonOpenPositions && groups.Count > 0)
                {
                    //sort by date
                    SortAllocationGroupList(groups);
                }
                if (groups.Count == 1)
                {
                    UltraGridRow[] rows = _selectedGrid.Rows.GetFilteredInNonGroupByRows();
                    foreach (UltraGridRow row in rows)
                    {
                        if (row.Cells["checkBox"].Text == true.ToString())
                        {
                            _selectedGrid.ActiveRow = row;
                            break;
                        }
                    }
                    isMultipleSelected = false;
                }
                else
                {
                    isMultipleSelected = true;
                }

                // this allocation group list is used to collect groups which are not allocated based on latest positions and put on hold status
                List<AllocationGroup> groupListWithHoldStatus = new List<AllocationGroup>();

                string result = string.Empty;
                List<AllocationGroup> allocationGroupList = new List<AllocationGroup>();
                foreach (AllocationGroup group in groups)
                {
                    AllocationGroup groupObj = group;

                    // use in open postions based allocation
                    GetGroupsWithHoldState(groupListWithHoldStatus, groupObj);

                    if (result.Equals(string.Empty))
                    {
                        group.ErrorMessage = string.Empty;
                        group.GroupAllocationStatus = GroupAllocationStatus.Allocated;
                        allocationGroupList.Add(group);
                        lblStatusStrip.Text = "Data allocated by account.";
                    }
                    else
                    {
                        group.ErrorMessage = result;
                        lblStatusStrip.Text = result;
                    }
                }

                string response = "";
                if (allocationGroupList.Count > 0)
                {
                    //show message to calculate commission again only if any of selected groups have commission or soft commission source as manual, PRANA-13009
                    bool isAnyCommissionSourceManual = false;
                    Parallel.ForEach(allocationGroupList, (group, state) =>
                    {
                        if (group.CommSource == CommisionSource.Manual || group.SoftCommSource == CommisionSource.Manual || group.CommissionSource == (int)CommisionSource.Manual || group.SoftCommissionSource == (int)CommisionSource.Manual)
                        {
                            isAnyCommissionSourceManual = true;
                            state.Break();
                        }
                    });

                    if (isAnyCommissionSourceManual)
                    {
                        DialogResult choice = MessageBox.Show(this, "For some groups commission is specified manually, Would you like to calculate commission again for these groups?", "Nirvana Allocation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        Parallel.ForEach(allocationGroupList, group =>
                        {
                            if (choice == DialogResult.Yes)
                                group.IsRecalculateCommission = true;
                            else
                                group.IsRecalculateCommission = false;
                        }
                        );
                    }
                    response = AllocationManager.GetInstance().AllocateGroup(allocationGroupList, pref, false, isChanged, IsForceAllocation());
                }
                if (!string.IsNullOrWhiteSpace(response))
                {
                    lblStatusStrip.Text = response.Contains("\n") ? response.Substring(0, response.IndexOf("\n") - 1) : response;
                    MessageBox.Show(this, response, "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // use in open postions based allocation
                // if there are some groups which are not allocated due to wrong side,we put them on hold and 
                // these groups will be allocated in the second round

                result = ReAllocateToAvoidBoxedPositions(isMultipleSelected, groupListWithHoldStatus, result);
                ClearUnAllocatedAccountNumberControl(result);
                // Clear Accountwise latest positions Cache on Trade Server after Auto Allocate Closing Transactions                
                ClearOpenPositionsCacheOnTradeServer(groups.Count);
                _isRowClicked = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
            }
        }

        private void SortAllocationGroupList(List<AllocationGroup> groups)
        {
            try
            {
                // sort by AUECLocalDate and then customized side
                groups.Sort(SortingCriteria);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private int SortingCriteria(AllocationGroup group1, AllocationGroup group2)
        {
            int result = 0;
            try
            {
                result = group1.AUECLocalDate.Date.CompareTo(group2.AUECLocalDate.Date);
                // if both groups have same date then sort by OrderSideTagValue i.e. Side
                if (result == 0)
                {
                    int i = _sideSortingList.IndexOf(group1.OrderSideTagValue);
                    int j = _sideSortingList.IndexOf(group2.OrderSideTagValue);
                    if (i >= 0 && j >= 0)
                    {
                        if (i > j)
                            return 1;
                        else if (i < j)
                            return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return result;
        }

        private int SortingCriteria2(AllocationGroup group1, AllocationGroup group2)
        {
            int result = 0;
            try
            {
                List<string> sideSortingList = new List<string>();
                sideSortingList.Add(FIXConstants.SIDE_Buy_Closed);
                sideSortingList.Add(FIXConstants.SIDE_Sell_Open);
                sideSortingList.Add(FIXConstants.SIDE_SellShort);
                sideSortingList.Add(FIXConstants.SIDE_Sell_Closed);
                sideSortingList.Add(FIXConstants.SIDE_Sell);
                sideSortingList.Add(FIXConstants.SIDE_Buy_Open);
                sideSortingList.Add(FIXConstants.SIDE_Buy);

                result = group1.AUECLocalDate.Date.CompareTo(group2.AUECLocalDate.Date);
                // if both groups have same date then sort by OrderSideTagValue i.e. Side
                if (result == 0)
                {
                    int i = sideSortingList.IndexOf(group1.OrderSideTagValue);
                    int j = sideSortingList.IndexOf(group2.OrderSideTagValue);
                    if (i >= 0 && j >= 0)
                    {
                        if (i > j)
                            return 1;
                        else if (i < j)
                            return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return result;
        }

        private string ReAllocateToAvoidBoxedPositions(bool isMultipleSelected, List<AllocationGroup> groupListWithHoldStatus, string result)
        {
            try
            {
                if (groupListWithHoldStatus != null && groupListWithHoldStatus.Count > 0)
                {
                    StringBuilder groupnotAllocatedError = new StringBuilder();
                    SortAllocationGroupList(groupListWithHoldStatus);

                    #region AllocationOperationPref

                    AllocationOperationPreference pref = new AllocationOperationPreference();
                    pref.TryUpdateTargetPercentage(_selectedUnAllocatedCtrl.GetAllocationAccountValue());
                    #endregion

                    List<AllocationGroup> groupList = new List<AllocationGroup>();
                    foreach (AllocationGroup group in groupListWithHoldStatus)
                    {
                        AllocationGroup groupObj = group;
                        if (result.Equals(string.Empty))
                        {
                            group.ErrorMessage = string.Empty;
                            groupList.Add(group);
                            lblStatusStrip.Text = "Data allocated by account.";
                        }
                        else
                        {
                            group.ErrorMessage = result;
                            lblStatusStrip.Text = result;

                            groupnotAllocatedError.Append("GroupID: ");
                            groupnotAllocatedError.Append(group.GroupID);
                            groupnotAllocatedError.Append(", Symbol: ");
                            groupnotAllocatedError.Append(group.Symbol);
                            groupnotAllocatedError.Append(", Qty: ");
                            groupnotAllocatedError.Append(group.CumQty);
                            groupnotAllocatedError.Append(", AvgPrice: ");
                            groupnotAllocatedError.Append(group.AvgPrice);
                            groupnotAllocatedError.Append(Environment.NewLine);
                            groupnotAllocatedError.Append(result);
                            groupnotAllocatedError.Append(Environment.NewLine);
                        }
                    }
                    string response = "";
                    if (groupList.Count > 0)
                        response = AllocationManager.GetInstance().AllocateGroup(groupList, pref, true, false, IsForceAllocation());
                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        lblStatusStrip.Text = response.Contains("\n") ? response.Substring(0, response.IndexOf("\n") - 1) : response;
                        MessageBox.Show(this, response, "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    // if some trades are unAllocated based on Auto Allocate Closing Transactions, then show details to user if requires
                    ShowMessageForUnAllocatedTradesToAvoidBoxedPositions(groupnotAllocatedError);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        private void ShowMessageForUnAllocatedTradesToAvoidBoxedPositions(StringBuilder groupnotAllocatedError)
        {
            try
            {
                if (groupnotAllocatedError.Length > 0)
                {
                    String path = Application.StartupPath + @"\Logs\AutoAllocationToAvoidBoxedPositionsLog_" + _loginUser.CompanyUserID + ".txt";
                    using (StreamWriter streamWriter = new StreamWriter(path, false))
                    {
                        streamWriter.WriteLine(Environment.NewLine + DateTime.Now.ToString());
                        streamWriter.Write(groupnotAllocatedError.ToString());
                    }
                    DialogResult dr = MessageBox.Show("Quantity and Side validation failed for some trades. Do you want to see details?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        MessageBox.Show(groupnotAllocatedError.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void GetGroupsWithHoldState(List<AllocationGroup> groupListWithHoldStatus, AllocationGroup groupObj)
        {
            try
            {
                // use in open postions based allocation
                if (_allocationPrefs.GeneralRules.AllocateBasedonOpenPositions)
                {
                    switch (groupObj.GroupAllocationStatus)
                    {
                        case GroupAllocationStatus.Hold:
                            groupListWithHoldStatus.Add(groupObj);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ClearUnAllocatedAccountNumberControl(string result)
        {
            try
            {
                if (result.Equals(string.Empty))
                {
                    if (_allocationPrefs.GeneralRules.ClearAllocationAccountControlNumer)
                    {
                        // if clear grid is selected in preference, then allocation qty/% should not populate after selecting another trade
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7615
                        _targetPergentage.Clear();
                        _selectedUnAllocatedCtrl.ClearPercentage();
                        _selectedUnAllocatedCtrl.ClearQty();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ClearAllocatedAccountNumberControl(string result)
        {
            try
            {
                if (result.Equals(string.Empty))
                {
                    if (_allocationPrefs.GeneralRules.ClearAllocationAccountControlNumer)
                    {
                        // if clear grid is selected in preference, then allocation qty/% should not populate after selecting another trade
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7615
                        _strategyPergentage.Clear();
                        accountStrategyMapping1.ClearQty();
                        accountStrategyMapping1.ClearPercentage();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AllocateBySymbol(string allocationSchemeName)
        {
            try
            {
                List<AllocationGroup> groups = GetSelectedGroups(_selectedGrid);
                //Added to allocate highlighted row, PRANA-10754
                if (groups.Count == 0 && _selectedGrid.ActiveRow != null && _selectedGrid.ActiveRow.ListObject is AllocationGroup)
                    groups.Add((AllocationGroup)_selectedGrid.ActiveRow.ListObject);

                //modified by- omshiv, 15 Jan 2014, Add check of groups count 
                if (groups.Count > 0)
                {
                    //show message to calculate commission again only if any of selected groups have commission or soft commission source as manual, PRANA-13009
                    bool isAnyCommissionSourceManual = false;
                    Parallel.ForEach(groups, (group, state) =>
                    {
                        if (group.CommSource == CommisionSource.Manual || group.SoftCommSource == CommisionSource.Manual || group.CommissionSource == (int)CommisionSource.Manual || group.SoftCommissionSource == (int)CommisionSource.Manual)
                        {
                            isAnyCommissionSourceManual = true;
                            state.Break();
                        }
                    });

                    if (isAnyCommissionSourceManual)
                    {
                        DialogResult choice = MessageBox.Show(this, "For some groups commission is specified manually, Would you like to calculate commission again for these groups?", "Nirvana Allocation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        Parallel.ForEach(groups, group =>
                        {
                            if (choice == DialogResult.Yes)
                                group.IsRecalculateCommission = true;
                            else
                                group.IsRecalculateCommission = false;
                        }
                        );
                    }

                    foreach (AllocationGroup group in groups)
                    {
                        group.ErrorMessage = string.Empty;
                        AllocationLevelList accounts = new AllocationLevelList();
                        bool isSwap = false;
                        string result = _proxyAllocationServices.InnerChannel.ValidateAllocationAccountsByAllocationScheme(group, ref accounts, allocationSchemeName, ref isSwap, _allocationPrefs.GeneralRules.IsPariPassuAllocation, _allocationPrefs.GeneralRules.SelectedAccountID, _allocationPrefs.GeneralRules.isMasterFundRatioAllocation);
                        if (result == string.Empty)
                        {
                            SetSwapParameters(isSwap, group, allocationSchemeName, accounts);
                            lblStatusStrip.Text = "Data allocated by selected scheme.";
                        }
                        else
                        {
                            group.ErrorMessage = result;
                            lblStatusStrip.Text = result;
                        }
                    }
                    SetAllocationMethodologyByAccount();
                    _isRowClicked = false;
                }
                else
                {
                    lblStatusStrip.Text = "Please Select trade/group(s) to allocate. ";
                    return;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private SwapParameters GetDefaultSwapParameters(double avgPrice, DateTime auecLocalDate, double cumQty)
        {
            SwapParameters swapParameters = null;
            try
            {
                swapParameters = new SwapParameters();
                swapParameters.OrigCostBasis = avgPrice;
                swapParameters.OrigTransDate = auecLocalDate;
                swapParameters.NotionalValue = avgPrice * cumQty;

                swapParameters.BenchMarkRate = Convert.ToDouble(_defaultSwapParameters["BenchMarkRate"]);
                swapParameters.DayCount = Convert.ToInt32(_defaultSwapParameters["DayCount"]);
                swapParameters.Differential = Convert.ToDouble(_defaultSwapParameters["Differential"]);
                swapParameters.FirstResetDate = Convert.ToDateTime(_defaultSwapParameters["FirstResetDate"]);
                swapParameters.ResetFrequency = _defaultSwapParameters["ResetFrequency"];
                swapParameters.SwapDescription = _defaultSwapParameters["SwapDescription"];
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return swapParameters;
        }

        private void btnGetAllocationData_Click(object sender, EventArgs e)
        {
            try
            {
                // Modified by Ankit Gupta on 13 Oct, 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1580
                if (DateTime.Compare(Convert.ToDateTime(dtToDatePickerAllocation.Value), Convert.ToDateTime(dtFromDatePickerAllocation.Value)) >= 0)
                {
                    ClearLabelStrip();
                    GetAllocationData();
                    HideSwapDetails();
                }
                else
                {
                    MessageBox.Show("To date cannot be less than From date", "Edit Trades", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Should be used only after setting the saveDataAction variable to appropriate value. The action will be performed after saving 
        /// Use the return type of DialogResult for cases in which the data is not to be saved.(Refer other cases)
        /// </summary>
        /// <returns></returns>
        private DialogResult PromptForDataSaving(AllocationConstants.ActionAfterSavingData actionAfterSaving, string tabKey = "")
        {
            try
            {
                DialogResult userChoice = DialogResult.No;
                // Check for cost adjustment changes
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7414
                if (ctrlCostAdjustment.IsAnythingChanged() && btnSave.Enabled == true && btnSaveWOState.Enabled == true)
                {
                    userChoice = CostAdjustmentSaveDataPrompt(actionAfterSaving.ToString());
                }
                else if (AllocationManager.GetInstance().AnythingChanged() && btnSave.Enabled == true && btnSaveWOState.Enabled == true)
                {
                    bool saveWtState = btnSave.Visible;
                    bool saveWoState = btnSaveWOState.Visible;
                    if (saveWtState && saveWoState)
                    {
                        userChoice = Prana.Utilities.MiscUtilities.MsgBox.Show("Would you like to save Allocation/Edit Trade changes?", "Warning!", MsgBox.Buttons.YesNoCancel, MsgBox.Icon.Info, MsgBox.AnimateStyle.FadeIn, "Save (w/Status)", "Save (w/o Status)", "No");
                        //Added condition to save data according to user choice, PRANA-12622
                    if (userChoice == DialogResult.Yes)
                    {
                        lblStatusStrip.Text = "Saving data and saving State.";
                        IsSaveState = true;
                        SaveDataAsync(actionAfterSaving, tabKey);
                    }
                    else if (userChoice == DialogResult.No)
                    {
                        lblStatusStrip.Text = "Saving data without saving State.";
                        IsSaveState = false;
                        SaveDataAsync(actionAfterSaving, tabKey);
                            userChoice = DialogResult.Yes;
                }
                    else
                        userChoice = DialogResult.No;
                }
                    else if (saveWtState)
                    {
                        userChoice = Prana.Utilities.MiscUtilities.MsgBox.Show("Would you like to save Allocation/Edit Trade changes?", "Warning!", MsgBox.Buttons.YesNo, MsgBox.Icon.Info, MsgBox.AnimateStyle.FadeIn, "Save (w/Status)", "No");
                        //Added condition to save data according to user choice, PRANA-12622
                        if (userChoice == DialogResult.Yes)
                        {
                            lblStatusStrip.Text = "Saving data and saving State.";
                            IsSaveState = true;
                            SaveDataAsync(actionAfterSaving, tabKey);
                        }
                    }
                    else if (saveWoState)
                    {
                        userChoice = Prana.Utilities.MiscUtilities.MsgBox.Show("Would you like to save Allocation/Edit Trade changes?", "Warning!", MsgBox.Buttons.YesNo, MsgBox.Icon.Info, MsgBox.AnimateStyle.FadeIn, "Save (w/o Status)", "No");
                        //Added condition to save data according to user choice, PRANA-12622
                        if (userChoice == DialogResult.Yes)
                        {
                            lblStatusStrip.Text = "Saving data without saving State.";
                            IsSaveState = false;
                            SaveDataAsync(actionAfterSaving, tabKey);
                        }
                    }
                    else if (!saveWoState && !saveWtState)
                    {
                        userChoice = Prana.Utilities.MiscUtilities.MsgBox.Show("Would you like to save Allocation/Edit Trade changes?", "Warning!", MsgBox.Buttons.YesNo, MsgBox.Icon.Info, MsgBox.AnimateStyle.FadeIn, "Save (w/Status)", "No");
                        //Added condition to save data according to user choice, PRANA-12622
                        if (userChoice == DialogResult.Yes)
                        {
                            lblStatusStrip.Text = "Saving data and saving State.";
                            IsSaveState = true;
                            SaveDataAsync(actionAfterSaving, tabKey);
                        }
                    }
                }
                return userChoice;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return DialogResult.No;
        }

        /// <summary>
        ///This method Ungroup data provided as List. This is called on new thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private String UnGroupData(List<AllocationGroup> groups)
        {
            try
            {
                List<AllocationGroup> unModifiedGroups = new List<AllocationGroup>();
                StringBuilder modifiedTradesAfterGrouping = new StringBuilder();

                foreach (AllocationGroup allocGroup in groups)
                {
                    unModifiedGroups.Add(allocGroup);
                    if (allocGroup.IsModified && allocGroup.Orders.Count > 1)
                    {
                        modifiedTradesAfterGrouping.Append("GroupID : ");
                        modifiedTradesAfterGrouping.Append(allocGroup.GroupID);
                        modifiedTradesAfterGrouping.Append(", Symbol : ");
                        modifiedTradesAfterGrouping.Append(allocGroup.Symbol);
                        modifiedTradesAfterGrouping.Append(", Qty : ");
                        modifiedTradesAfterGrouping.Append(allocGroup.CumQty);
                        modifiedTradesAfterGrouping.Append(", AvgPrice : ");
                        modifiedTradesAfterGrouping.Append(allocGroup.AvgPrice);
                        modifiedTradesAfterGrouping.Append(" changes are done at group level,");
                        modifiedTradesAfterGrouping.Append(Environment.NewLine);
                        unModifiedGroups.Remove(allocGroup);
                    }
                }
                if (modifiedTradesAfterGrouping.Length > 0)
                {
                    modifiedTradesAfterGrouping.Append("that will be lost in this process.");
                    modifiedTradesAfterGrouping.Append(Environment.NewLine);
                    DialogResult diares = DialogResult.No;
                    diares = MessageBox.Show(modifiedTradesAfterGrouping.ToString() + " Do you want to ungroup modified groups also?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (diares.Equals(DialogResult.Yes))
                    {
                        return AllocationManager.GetInstance().UnBundleGroups(groups);
                    }
                    else if (diares.Equals(DialogResult.No))
                    {
                        if (unModifiedGroups.Count > 0)
                            return AllocationManager.GetInstance().UnBundleGroups(unModifiedGroups);
                        else
                            return null;
                    }
                    else
                        return null;
                }
                else
                {
                    return AllocationManager.GetInstance().UnBundleGroups(groups);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return "Error";
            }
        }

        /// <summary>
        ///This method creates a new thread and assign grouping task by calling funcion
        ///UnGroupStarted and UnGroupCompleted this is generic and must be called by main thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyUnGrouping(List<AllocationGroup> group)
        {
            try
            {
                if (allocationDataChange != null)
                {
                    allocationDataChange(this, new EventArgs<bool>(true));
                }
                timerProgress.Start();
                btnAutoGrp.Text = "UnGrouping..";
                ToggleUIElementsWithMessage("UnGrouping Please wait", false);
                _headerCheckBoxUnallocated.SelectUnSelectAll(grdUnallocated, false, "checkBox");
                BackgroundWorker bgWorkerAutoGroup = new BackgroundWorker();
                bgWorkerAutoGroup.DoWork += new DoWorkEventHandler(UnGroupStarted);
                bgWorkerAutoGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UnGroupCompleted);
                bgWorkerAutoGroup.RunWorkerAsync(group);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///This is am event handler. This will be called by main thread when (RunWorkerAsync) is called.
        ///This is for processing main work 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UnGroupStarted(object sender, DoWorkEventArgs e)
        {
            try
            {
                String result = null;
                List<AllocationGroup> groups = e.Argument as List<AllocationGroup>;
                if (groups != null)
                {
                    result = UnGroupData(groups);
                }
                e.Result = result;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            /* No need to handle exception here as backgroundWorker will catch the exception 
             * and throw to completed method as RunWorkerCompletedEventArgs.Error property
             */
        }

        /// <summary>
        ///This is an event handler. This will be called by BackGroundWorker when either processing(DoWork) 
        ///is completed or any exception has occured. In case of exception Argument e has property Error 
        ///which contains Exception.
        ///This handler restore state of all changed control during processing as well as show output.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UnGroupCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                btnAutoGrp.Text = "AutoGroup";
                if (e.Error == null)
                {
                    lblStatusStrip.Text = e.Result as String;
                }
                else
                {
                    bool rethrow = ExceptionPolicy.HandleException(e.Error, ApplicationConstants.POLICY_LOGONLY);
                    if (rethrow)
                    {
                        throw new Exception(e.Error.Message);
                    }
                    lblStatusStrip.Text = "Could not complete UnGrouping. Please contact administrator.";
                    MessageBox.Show("UnGrouping failed. Please click GetData 'Without Saving' and try again.", "Allocation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                ToggleUIElementsWithMessage(string.Empty, true);
                if (allocationDataChange != null)
                {
                    allocationDataChange(this, new EventArgs<bool>(false));
                }
                //these line stops timer and clear progress label
                if (timerProgress.Enabled)
                    timerProgress.Stop();
                lblStatusStripProgress.Text = string.Empty;

                //Calculating the selectedGrid row in the grid AddAndUpdateExternalTransactionID then Update trade counter
                UltraGridRow[] rows1 = grdUnallocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsUnallocatedGrd = Convert.ToInt32(rows1.Count(row1 => row1.Cells["checkBox"].Text == true.ToString()));
                UpdateTotalNoOfTradesForUnallocatedgrd(_countSelectedRowsUnallocatedGrd);
            }
        }

        /// <summary>
        /// This method creates a new thread and assign grouping task by calling funcion
        /// AutoGroupStarted and AutoGroupCompleted this is generic and must be called by main thread.
        /// </summary> 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyGrouping(GenericBindingList<AllocationGroup> group)
        {
            try
            {
                if (allocationDataChange != null)
                {
                    allocationDataChange(this, new EventArgs<bool>(true));
                }

                timerProgress.Start();//Starts timer for showing progresslabel
                btnAutoGrp.Text = "Grouping...";
                ToggleUIElementsWithMessage(String.Empty, false);
                _headerCheckBoxUnallocated.SelectUnSelectAll(grdUnallocated, false, "checkBox");
                BackgroundWorker bgWorkerAutoGroup = new BackgroundWorker();
                bgWorkerAutoGroup.DoWork += new DoWorkEventHandler(AutoGroupStarted);
                bgWorkerAutoGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AutoGroupCompleted);
                bgWorkerAutoGroup.RunWorkerAsync(group);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This is am event handler. This will be called by main thread when (RunWorkerAsync) is called.
        /// This is for processing main work
        /// </summary> 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AutoGroupStarted(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = AllocationManager.GetInstance().AutoGroup(_allocationPrefs, (GenericBindingList<AllocationGroup>)e.Argument);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                e.Result = "Error";
            }
            /* No need to handle exception here as backgroundWorker will catch the exception 
             * and throw to completed method as RunWorkerCompletedEventArgs.Error property
             */
        }

        /// <summary>
        /// This is an event handler. This will be called by BackGroundWorker when either processing(DoWork) 
        /// is completed or any exception has occured. In case of exception Argument e has property Error 
        /// which contains Exception.
        /// This handler restore state of all changed control during processing as well as show output.
        /// </summary> 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AutoGroupCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                ToggleUIElementsWithMessage(String.Empty, true);
                btnAutoGrp.Text = "AutoGroup";
                if (e.Error == null)
                {
                    lblStatusStrip.Text = e.Result as string;
                }
                else
                {
                    bool rethrow = ExceptionPolicy.HandleException(e.Error, ApplicationConstants.POLICY_LOGONLY);
                    if (rethrow)
                    {
                        throw new Exception();
                    }
                    lblStatusStrip.Text = "Could not complete AutoGrouping. Please contact administrator.";
                }
                grdUnallocated.EndUpdate();
                //Calculating the selectedGrid row in the grid AddAndUpdateExternalTransactionID then Update trade counter
                UltraGridRow[] rows = grdUnallocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsUnallocatedGrd = Convert.ToInt32(rows.Count(row => row.Cells["checkBox"].Text == true.ToString()));
                UpdateTotalNoOfTradesForUnallocatedgrd(_countSelectedRowsUnallocatedGrd);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (allocationDataChange != null)
                {
                    allocationDataChange(this, new EventArgs<bool>(false));
                }
                //Stops timer and clear progresslabel
                if (timerProgress.Enabled)
                    timerProgress.Stop();
                lblStatusStripProgress.Text = string.Empty;
            }
        }

        /// <summary>
        /// This method saves data to database on a new thread.
        /// </summary>
        public void SaveDataAsync(AllocationConstants.ActionAfterSavingData actionAfterSaving, string tabKey = "")
        {
            try
            {
                //modified by omshiv, May 16 2014, If nav locking enabled then check for NAV is locked or not for account, block the changes
                //which can affect the Account NAV              
                object[] actionHandler = new object[] { (object)actionAfterSaving, (object)tabKey };
                if (allocationDataChange != null)
                {
                    allocationDataChange(this, new EventArgs<bool>(true));
                }
                timerProgress.Start();//Starting timer for progress label.

                ClearLabelStrip();
                ToggleUIElementsWithMessage("Saving Data. Please wait", false);
                //lblStatusStrip.Text = "Saving Data...";
                //btnSave.Enabled = false;
                //btnAutoGrp.Enabled = false;
                //btnGetAllocationData.Enabled = false;
                //grdAllocated.Enabled = false;
                //grdUnallocated.Enabled = false;
                BackgroundWorker saveDataAsyncWorker = new BackgroundWorker();
                saveDataAsyncWorker.DoWork += new DoWorkEventHandler(saveDataAsync_DoWork);
                saveDataAsyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(saveDataAsync_RunWorkerCompleted);
                saveDataAsyncWorker.RunWorkerAsync(actionHandler);
                // Commented call to SaveLastPreferencedAccountID() function as this is causing the client to crash in case of cost adjustment on multiple trades
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7227
                // TODO: Need to removed as it is no longer required.
                //_proxyAllocationServices.InnerChannel.SaveLastPreferencedAccountID(_allocationPrefs.GeneralRules.SelectedAccountID);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #region CommentedCodeForNavLock
        ///// <summary>
        ///// Validate Trade For Account NAV Lock on saving trade after making changes 
        ///// Created By: Omshiv, 15 may 2014
        ///// </summary>
        ///// <returns></returns>
        //private bool ValidateTradeForAccountNAVLock()
        //{
        //    bool isProcessToSave = true;
        //    try
        //    {
        //        #region NAV lock validation - modified by Omshiv, MArch 2014
        //        //get IsNAVLockingEnabled or not from cache
        //        if (_releaseType == PranaReleaseViewType.CHMiddleWare)
        //        {
        //            Boolean isAccountNAVLockingEnabled = CachedDataManager.GetInstance.IsNAVLockingEnabled();

        //            if (isAccountNAVLockingEnabled)
        //            {
        //                foreach (UltraGridRow row in grdAllocated.Rows)
        //                {
        //                    AllocationGroup allocationGrp = row.ListObject as AllocationGroup;
        //                    if (allocationGrp != null && allocationGrp.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged)
        //                    {
        //                        foreach (TaxLot taxlot in allocationGrp.TaxLots)
        //                        {

        //                            //if account selected then only check NAV locked or not for selected account - omshiv, March 2014
        //                            //if (taxlot.Level1ID != null && taxlot.Level1ID != 0) // commented old if as int is never null
        //                            if (taxlot.Level1ID != 0)
        //                            {

        //                                DateTime tradeDate = taxlot.OriginalPurchaseDate;
        //                                isProcessToSave = Prana.ClientCommon.NAVLockManager.GetInstance.ValidateTrade(taxlot.Level1ID, tradeDate);
        //                                if (!isProcessToSave)
        //                                {
        //                                    return isProcessToSave;
        //                                }
        //                            }

        //                        }
        //                    }

        //                }
        //            }


        //        }
        //        else
        //        {
        //            return true;
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //    return isProcessToSave;
        //}
        #endregion
        /// <summary>
        /// This method GetData data to database on a new thread.
        /// </summary>
        public void GetDataAsync(String ToAllAUECDatesString, String FromAllAUECDatesString, String fromAllocatedAllAUECDatesString)
        {
            try
            {
                if (allocationDataChange != null)
                {
                    allocationDataChange(this, new EventArgs<bool>(true));
                }
                //Getting filter contents from filter grid
                Dictionary<String, Dictionary<String, String>> _filterList = null;

                switch (_currentFilterScope)
                {
                    case AllocationConstants.FilterScope.All:
                        if (_combinedFilter != null)
                        {
                            _filterList = _combinedFilter.GetCombinedFilterCondition();
                        }
                        break;
                    case AllocationConstants.FilterScope.UnAllocated:
                        if (_filterGridUnAllocated != null)
                        {
                            if (_filterList == null)
                                _filterList = new Dictionary<string, Dictionary<string, string>>();
                            _filterList.Add(_filterGridUnAllocated.FilterTitle, _filterGridUnAllocated.GetFilterCondition());
                        }
                        break;
                    case AllocationConstants.FilterScope.Allocated:
                        if (_filterGridAllocated != null)
                        {
                            if (_filterList == null)
                                _filterList = new Dictionary<string, Dictionary<string, string>>();
                            _filterList.Add(_filterGridAllocated.FilterTitle, _filterGridAllocated.GetFilterCondition());
                        }
                        break;
                    case AllocationConstants.FilterScope.Split:
                        if (_filterGridAllocated != null)
                        {
                            if (_filterList == null)
                                _filterList = new Dictionary<string, Dictionary<string, string>>();
                            _filterList.Add(_filterGridAllocated.FilterTitle, _filterGridAllocated.GetFilterCondition());
                        }
                        if (_filterGridUnAllocated != null)
                        {
                            if (_filterList == null)
                                _filterList = new Dictionary<string, Dictionary<string, string>>();
                            _filterList.Add(_filterGridUnAllocated.FilterTitle, _filterGridUnAllocated.GetFilterCondition());
                        }
                        break;
                }


                //ChangeButtonStatus(false);                
                timerProgress.Start();//Starts timer to show progress
                //Removing filtergrid from grdUnallocated as control but will be in memory as long UI is running
                if (grdUnallocated.Controls.Contains(_filterGridUnAllocated))
                {
                    grdUnallocated.Controls.Remove(_filterGridUnAllocated);
                    grdUnallocated.DisplayLayout.Scrollbars = Scrollbars.Automatic;
                }
                //Removing filtergrid from grdAllocated as control but will be in memory as long UI is running
                if (grdAllocated.Controls.Contains(_filterGridAllocated))
                {
                    grdAllocated.Controls.Remove(_filterGridAllocated);
                    grdAllocated.DisplayLayout.Scrollbars = Scrollbars.Automatic;
                }

                //changing status of Prefetch filter stateButton to unchecked
                Infragistics.Win.UltraWinToolbars.StateButtonTool sbt = ultraToolbarsManager1.Tools["PrefetchFilter"] as Infragistics.Win.UltraWinToolbars.StateButtonTool;
                if (sbt != null)
                {
                    //Changing status of state tool buton fires click event so first unwiring click event and then after wiring again
                    ultraToolbarsManager1.ToolClick -= new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(ultraToolbarsManager1_ToolClick);
                    sbt.Checked = false;
                    ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(ultraToolbarsManager1_ToolClick);
                }


                AllocationManager.GetInstance().ClearData();
                //int index = ultraToolbarsManager1.Tools["FilterStatus"].Index;
                //ultraToolbarsManager1.Tools["FilterStatus_Enabled"].SharedProps.Enabled = false;
                //    //.RemoveAt(ultraToolbarsManager1.Tools.IndexOf("START WRITING DATA"));

                //lblStatusStrip.Text = "Getting data.";

                #region
                ApplyPermittedAccountsFiltering(ref _filterList);
                #endregion

                //Customizing labelstrip message as well as setting Applied/Unapplied status to button FilterStatus_Enabled
                if (_filterList != null && _filterList.Count > 0)
                {
                    if (_filterList.ContainsKey(_filterGridAllocated.FilterTitle))
                    {
                        _filterList[_filterGridAllocated.FilterTitle].Add("FromDate", fromAllocatedAllAUECDatesString);
                    }
                    else
                    {
                        _filterList.Add(_filterGridAllocated.FilterTitle, new Dictionary<string, string>());
                        _filterList[_filterGridAllocated.FilterTitle].Add("FromDate", fromAllocatedAllAUECDatesString);
                    }

                    if ((_filterList.ContainsKey(_filterGridAllocated.FilterTitle) && _filterList[_filterGridAllocated.FilterTitle].Count > 0) || (_filterList.ContainsKey(_filterGridUnAllocated.FilterTitle) && _filterList[_filterGridUnAllocated.FilterTitle].Count > 0))
                    {
                        ToggleUIElementsWithMessage("Getting Data according to Prefetch Filter(s). Please wait", false);
                        ultraToolbarsManager1.Tools["FilterStatus_Enabled"].SharedProps.Enabled = true;
                        ultraToolbarsManager1.Tools["FilterStatus_Enabled"].SharedProps.Caption = "Prefetch filter applied";
                    }
                    else
                    {
                        ToggleUIElementsWithMessage("Getting Data. Please wait", false);
                        ultraToolbarsManager1.Tools["FilterStatus_Enabled"].SharedProps.Enabled = false;
                        ultraToolbarsManager1.Tools["FilterStatus_Enabled"].SharedProps.Caption = "Prefetch filter cleared";
                    }
                }
                else
                {
                    ToggleUIElementsWithMessage("Getting Data. Please wait", false);
                    ultraToolbarsManager1.Tools["FilterStatus_Enabled"].SharedProps.Enabled = false;
                    ultraToolbarsManager1.Tools["FilterStatus_Enabled"].SharedProps.Caption = "Prefetch filter cleared";
                }


                BackgroundWorker fetchDataAsyc = new BackgroundWorker();
                fetchDataAsyc.RunWorkerCompleted += new RunWorkerCompletedEventHandler(fetchDataAsyc_RunWorkerCompleted);
                fetchDataAsyc.DoWork += new DoWorkEventHandler(fetchDataAsyc_DoWork);

                //ctrlCostAdjustment.BindData(AllocationManager.GetInstance().AllocationServices.InnerChannel.GetAllOpenTaxlots
                //    (Convert.ToDateTime(fromAllocatedAllAUECDatesString).Date, Convert.ToDateTime(ToAllAUECDatesString).Date, false, "", "", "", "").AdjustedTaxlots);
                //sending filterlist too to background worker
                fetchDataAsyc.RunWorkerAsync(new object[] { ToAllAUECDatesString, FromAllAUECDatesString, _filterList });
            }
            catch (Exception ex)
            {
                RestoreStateOfControls();
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Added By Faisal Shah
        /// Modifies the filterlist as per the permission of accounts.
        /// </summary>
        /// <param name="_filterList"></param>
        /// <returns></returns>
        private bool GetPermissibleFiltersForCH(Dictionary<String, Dictionary<String, String>> _filterList)
        {
            bool isValidToProcess = true;
            try
            {
                //if ch release then addd Account filter with commma saperated accountIDs if accountFilter is blank

                //bool isFilterAllowed = true;
                StringBuilder nonPermissibleAccounts = new StringBuilder();
                StringBuilder allowedAccountsForFilter = new StringBuilder();
                List<int> accounts = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict().Keys.ToList();
                if (_filterList["Allocated"].ContainsKey("FundID"))
                {
                    string[] filteraccountList = _filterList["Allocated"]["FundID"].Split(',');
                    foreach (string account in filteraccountList)
                    {
                        if (!String.IsNullOrWhiteSpace(account) && !accounts.Contains(Convert.ToInt32(account)))
                        {
                            string accountName = CachedDataManager.GetInstance.GetAccountText(Convert.ToInt32(account));
                            nonPermissibleAccounts.Append(" " + accountName + ",");
                            //isFilterAllowed = false;
                        }
                        else
                            allowedAccountsForFilter.Append(account + ",");

                    }
                    _filterList["Allocated"].Remove("FundID");
                    if (allowedAccountsForFilter.Length > 0)
                    {
                        _filterList["Allocated"].Add("FundID", allowedAccountsForFilter.ToString().Substring(0, allowedAccountsForFilter.Length - 1));
                    }
                    else
                    {

                        MessageBox.Show("You do not have permission for the following account(s) :\n" + nonPermissibleAccounts.ToString().Substring(0, nonPermissibleAccounts.Length - 1) + "\n Please refine filter on accounts", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        isValidToProcess = false;
                        return isValidToProcess;
                    }
                    if (nonPermissibleAccounts.Length > 0)
                        MessageBox.Show("You do not have permission for the following account(s) :\n" + nonPermissibleAccounts.ToString().Substring(0, nonPermissibleAccounts.Length - 1) + "\n Please remove non permissible accounts from the filter", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //Check Permission
                }
                else
                {

                    if (accounts.Count > 0)
                    {
                        string allAllowedAccounts =
                        accounts.Select(i => i.ToString(CultureInfo.InvariantCulture))
                        .Aggregate((s1, s2) => s1 + ", " + s2);
                        _filterList["Allocated"].Add("FundID", allAllowedAccounts);
                    }
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isValidToProcess;
        }

        /// <summary>
        /// Modify filter list according to the account permissions
        /// </summary>
        /// <param name="_filterList">filter list is modified according to the permitted account permissions</param>
        private void ApplyPermittedAccountsFiltering(ref Dictionary<String, Dictionary<String, String>> _filterList)
        {
            try
            {
                //Get permitted accounts for the user
                AccountCollection accountCollection = CachedDataManager.GetInstance.GetUserAccounts();
                Dictionary<string, string> dictAccounts = new Dictionary<string, string>();
                //When allocation UI is opened then filter list is null
                if (_filterList == null)
                    _filterList = new Dictionary<string, Dictionary<string, string>>();

                if (!_filterList.ContainsKey(_filterGridAllocated.FilterTitle))
                {
                    _filterList.Add(_filterGridAllocated.FilterTitle, dictAccounts);
                }

                //in search text box no account names are specified
                //no account filter criteria specified
                if (!_filterList[_filterGridAllocated.FilterTitle].ContainsKey(OrderFields.CAPTION_Level1ID))
                {
                    dictAccounts.Add(OrderFields.CAPTION_Level1ID, string.Empty);
                    foreach (Prana.BusinessObjects.Account account in accountCollection)
                    {
                        var lstAccounts = dictAccounts[OrderFields.CAPTION_Level1ID].Split(',').ToList();
                        if (!lstAccounts.Contains(account.AccountID.ToString()) && account.AccountID != int.MinValue)
                        {
                            dictAccounts[OrderFields.CAPTION_Level1ID] += account.AccountID + Seperators.SEPERATOR_8;
                        }
                    }
                }
                //if value is given in filter text box of allocation/edit trades UI
                //account filter criteria specified
                else
                {
                    dictAccounts.Add(OrderFields.CAPTION_Level1ID, _filterList[_filterGridAllocated.FilterTitle][OrderFields.CAPTION_Level1ID]);
                    var lstAccounts = dictAccounts[OrderFields.CAPTION_Level1ID].Split(',').ToList();
                    dictAccounts[OrderFields.CAPTION_Level1ID] = string.Empty;
                    foreach (Prana.BusinessObjects.Account account in accountCollection)
                    {
                        if (lstAccounts.Contains(account.AccountID.ToString()) && account.AccountID != int.MinValue)
                        {
                            dictAccounts[OrderFields.CAPTION_Level1ID] += account.AccountID + Seperators.SEPERATOR_8;
                        }
                    }
                }
                //assign modified filtered account dictionary
                if (!_filterList[_filterGridAllocated.FilterTitle].ContainsKey(OrderFields.CAPTION_Level1ID))
                {
                    _filterList[_filterGridAllocated.FilterTitle].Add(OrderFields.CAPTION_Level1ID, dictAccounts[OrderFields.CAPTION_Level1ID]);
                }
                //_filterList[_filterGridAllocated.FilterTitle] = dictAccounts;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        // This method sets allocation methodology to Account after allocation on the basis of preferences
        public void SetAllocationMethodologyByAccount()
        {
            try
            {
                if (_allocationPrefs.GeneralRules.AllocationMethodologyRevertToAccount)
                {
                    rbtnAllocationByAccount.CheckedIndex = 0;
                    cmbbxdefaults.Enabled = true;
                    cmbbxdefaults.BringToFront();
                    cmbAllocationScheme.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnAutoGrp_Click(object sender, EventArgs e)
        {
            try
            {
                GenericBindingList<AllocationGroup> tUnAllocatedGroups = new GenericBindingList<AllocationGroup>();
                tUnAllocatedGroups.AddList(AllocationManager.GetInstance().UnAllocatedGroups.GetList());
                lblStatusStrip.Text = "AutoGrouping unallocated data. Please wait";
                grdUnallocated.Enabled = false;
                ControlDrawing.SuspendDrawing(grdUnallocated);
                grdUnallocated.BeginUpdate();
                //bgWorkerAutoGroup.RunWorkerAsync(tUnAllocatedGroups);
                ApplyGrouping(tUnAllocatedGroups); // Calling generic method which will create and execute thread on its own

                //AllocationManager.GetInstance().AutoGroup(_allocationPrefs);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (((UltraButton)sender).Name.Equals("btnSaveWOState"))
                {
                    IsSaveState = false;
                }
                else
                    IsSaveState = true;
                
                //saved adjusted cost groups
                if (tabAllocation.SelectedTab.Text == "Cost Adjustment")
                {
                    // Disable UI elements before starting save for cost adjustment
                    timerProgress.Start();//Starting timer for progress label.
                    ClearLabelStrip();
                    ToggleUIElementsWithMessage("Saving Data. Please wait", false);
                    // passing empty string as no action is required after save
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-8204
                    ctrlCostAdjustment.SaveCostAdjustment(string.Empty);
                }
                else
                {
                    AllocationConstants.ActionAfterSavingData saveDataAction = AllocationConstants.ActionAfterSavingData.DoNothing;
                    SaveDataAsync(saveDataAction);
                }


            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Unallocate and delete data at once
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => btnDelete_Click(sender, e)));
                }
                else
                {
                    //added by: Bharat Raturi, 31 may 2014
                    //Purpose: Show the prompt to delete the trade before deleting it
                    //Modify by: sachin Mishra-21/jan/2015 JIRA No-CHMW-2390Purpose for checking any item is selected or not If not then show message 
                    if (ctrlAmendmend1.checkForSelectItem())
                    {
                        DialogResult dr = MessageBox.Show("Do you want to delete the selected trade(s)?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {

                            bool allLocksareAcquired = true;
                            //For CH users only
                            if (CachedDataManager.GetInstance.GetPranaReleaseViewType().Equals(PranaReleaseViewType.CHMiddleWare))
                            {
                                allLocksareAcquired = ctrlAmendmend1.checkForlockedAccounts();
                            }
                            if (allLocksareAcquired)
                            {
                                _proxyAllocationServices.InnerChannel.UpdatePreferencedAccountID();
                                List<AllocationGroup> groups = ctrlAmendmend1.GetSelectedAllocatedGroups();
                                ctrlAmendmend1.ToggleUIElementsWithMessage(string.Empty, false);
                                UnAllocateDataAsync(groups);
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("No Trade Selected !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        void saveDataAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int rowsAffected = 0;
            try
            {
                if (!this.IsDisposed && !this.Disposing)
                {
                    if ((e.Cancelled == true))
                    {
                        MessageBox.Show("Cancelled!", "Allocation", MessageBoxButtons.OK);
                    }

                    else if (!(e.Error == null))
                    {
                        MessageBox.Show("Error: " + e.Error.Message, "Allocation", MessageBoxButtons.OK);
                    }

                    else
                    {
                        object[] parameters = e.Result as object[];
                        rowsAffected = Convert.ToInt32(parameters[1].ToString());
                        AllocationManager.GetInstance().ClearDictionaryUnsaved();
                        ctrlAmendmend1.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Closed = true;
                        //ctrlAmendmend1.ctrlAmendSingleGroup1.ClearDataBindings();
                        //ctrlAmendmend1.ctrlAmendSingleGroup1.SetDefaultValuetoControl();

                        //Narendra Kumar Jangir 2013 Mar 05
                        //rowsAffected is returned -1 in case when user try to reallocate closed data. 
                        if (rowsAffected < 0)
                        {
                            ToggleUIElementsWithMessage("Please click 'Get Data' to fetch the updated data from server!", false);
                            btnGetAllocationData.Enabled = true;
                            //_getDataBlinkTimer.Start();

                            if (allocationDataChange != null)
                            {
                                allocationDataChange(this, new EventArgs<bool>(true));
                            }
                            if (timerProgress.Enabled)
                                timerProgress.Stop();

                            return;
                        }
                        if (rowsAffected > 0)
                        {
                            AllocationManager.GetInstance().ReFillCurrentPositions();
                            lblStatusStrip.Text = "Allocation data saved.";
                        }
                        else
                        {
                            lblStatusStrip.Text = "Nothing to Save.";
                        }
                        timerClear.Start();
                        object[] arguments = parameters[0] as object[];
                        AllocationConstants.ActionAfterSavingData saveDataAction = (AllocationConstants.ActionAfterSavingData)arguments[0];
                        switch (saveDataAction)
                        {
                            case AllocationConstants.ActionAfterSavingData.ClearData:
                                AllocationManager.GetInstance().ClearData();
                                break;
                            case AllocationConstants.ActionAfterSavingData.GetData:
                                GetAllocationDataWithoutPrompt();
                                break;
                            case AllocationConstants.ActionAfterSavingData.CancelEditChanges:
                                ctrlAmendmend1.CancelEditChanges();
                                break;
                            case AllocationConstants.ActionAfterSavingData.CloseAllocation:
                                this.FindForm().Close();
                                break;
                            case AllocationConstants.ActionAfterSavingData.ChangeTab:
                                //if (tabAllocation.SelectedTab.Index == 0)
                                tabAllocation.SelectedTab = tabAllocation.Tabs[arguments[1].ToString()];
                                BindDataToCostAdjustmentUI();
                                //else
                                //tabAllocation.SelectedTab = tabAllocation.Tabs[0];
                                break;
                            //case ActionAfterSavingData.DoNothing:
                            //    BindDataToCostAdjustmentUI();
                            //    break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                //Narendra Kumar Jangir 2013 Mar 05
                //rowsAffected is returned -1 in case when user try to reallocate closed data. 
                if (rowsAffected >= 0)
                {
                    ToggleUIElementsWithMessage(String.Empty, true);
                    //modified by amit on 19.03.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-2902
                    if (ctrlAmendmend1 != null && CachedDataManager.GetInstance.GetPranaReleaseViewType().Equals(PranaReleaseViewType.CHMiddleWare))
                    {
                        ctrlAmendmend1.ToggleUIElementsWithMessage(string.Empty, true);
                    }
                    if (allocationDataChange != null)
                    {
                        allocationDataChange(this, new EventArgs<bool>(false));
                    }
                    int tabIndex = tabAllocation.ActiveTab.Index;
                    if (tabIndex == 1)
                    {
                        ugbxHeaderFill.Enabled = false;
                        btnAutoGrp.Enabled = false;
                        btnCheckSide.Visible = false;
                        btnClosing.Visible = false;
                    }

                    //Stoping time and clearing label 
                    if (timerProgress.Enabled)
                        timerProgress.Stop();
                    lblStatusStripProgress.Text = String.Empty;
                }
            }
        }

        void saveDataAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                int rowsAffected = AllocationManager.GetInstance().SaveGroups(IsSaveState);
                #region Auto Unwind Code when quantity changes http://jira.nirvanasolutions.com:8080/browse/CHMW-1793
                #endregion
                object[] parameters = new object[] { e.Argument, (object)rowsAffected };
                e.Result = parameters;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        #endregion

        private void AllocationMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_schemeForm != null)
                    _schemeForm.FormClosedInformation -= new EventHandler(_schemeForm_FormClosedInformation);
                if (_proxyAllocationServices != null)
                {
                    _proxyAllocationServices.ConnectedEvent -= new Proxy<IAllocationServices>.ConnectionEventHandler(_proxyAllocationServices_ConnectedEvent);
                    _proxyAllocationServices.DisconnectedEvent -= new Proxy<IAllocationServices>.ConnectionEventHandler(_proxyAllocationServices_DisconnectedEvent);
                    _proxyAllocationServices.Dispose();
                    _proxyAllocationServices = null;
                }
                AllocationPreferencesManager.Dispose();
                if (ctrlAmendmend1 != null)
                {
                    ctrlAmendmend1.GetAuditClick -= new EventHandler(ctrlAmendmend1_GetAuditClick);
                    //ctrlAmendmend1.allocationDataChange -= new AllocationDataChangeHandler(ctrlAmendmend1_allocationDataChange);

                    ctrlAmendmend1.Dispose();
                    ctrlAmendmend1 = null;

                }
                if (timerClear != null)
                {
                    timerClear.Tick -= new EventHandler(timerClear_Tick);
                    timerClear.Dispose();
                    timerClear = null;
                }
                if (dtFromDatePickerAllocation != null)
                    dtFromDatePickerAllocation.AfterCloseUp -= new EventHandler(FromEditor_AfterCloseUp);
                if (dtToDatePickerAllocation != null)
                    dtToDatePickerAllocation.AfterCloseUp -= new EventHandler(ToEditor_AfterCloseUp);

                //AllocationManager.GetInstance().DisableSaveButton -= new EventHandler(AllocationMain_DisableSaveButton);
                AllocationManager.GetInstance().NewGroupReceived -= new EventHandler(OnNewGroupReceived);
                AllocationManager.GetInstance().UpdateSymbolInfo -= new EventHandler(AllocationMain_UpdateSymbolInfo);
                AllocationManager.GetInstance().UpdateGroupClosingStatusHandler -= new EventHandler(AllocationMain_UpdateGroupClosingStatus);
                AllocationManager.GetInstance().AllocationPreferenceUpdated -= AllocationMain_AllocationPreferenceUpdated;
                AllocationManager.GetInstance().AllocationSchemeUpdated -= AllocationMain_AllocationSchemeUpdated;


                if (grdUnallocated != null)
                    grdUnallocated.AfterRowActivate -= new System.EventHandler(this.grdUnallocated_AfterRowActivate);
                if (grdAllocated != null)
                    grdAllocated.AfterRowActivate -= new System.EventHandler(this.grdAllocated_AfterRowActivate_1);
                if (accountStrategyMapping1 != null)
                {
                    accountStrategyMapping1.CheckTotalQty -= new EventHandler(accountStrategyMapping1_CheckTotalQty);
                    accountStrategyMapping1.CheckTotalPercentage -= new EventHandler(accountStrategyMapping1_CheckTotalPercentage);
                    accountStrategyMapping1.ChangePreference -= selectedUnAllocatedCtrl_ChangePreference;
                    accountStrategyMapping1.Dispose();
                    accountStrategyMapping1 = null;
                }
                if (accountOnlyUserControl1 != null)
                {
                    accountOnlyUserControl1.CheckTotalQty -= new EventHandler(accountOnlyUserControl1_CheckTotalQty);
                    accountOnlyUserControl1.CheckTotalPercentage -= new EventHandler(accountOnlyUserControl1_CheckTotalPercentage);
                    accountOnlyUserControl1.Dispose();
                    accountOnlyUserControl1 = null;
                }
                if (_headerCheckBoxUnallocated != null)
                {
                    _headerCheckBoxUnallocated._CLICKED -= new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBoxUnallocated__CLICKED);
                    _headerCheckBoxUnallocated.Dispose();
                    _headerCheckBoxUnallocated = null;

                }
                if (_headerCheckBoxAllocated != null)
                {
                    _headerCheckBoxAllocated._CLICKED -= new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBoxAllocated__CLICKED);
                    _headerCheckBoxAllocated.Dispose();
                    _headerCheckBoxAllocated = null;
                }
                if (ultraToolbarsManager1 != null)
                {
                    ultraToolbarsManager1.ToolClick -= new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(ultraToolbarsManager1_ToolClick);
                    ultraToolbarsManager1.Dispose();
                    ultraToolbarsManager1 = null;
                }
                if (this.grdUnallocated != null)
                {
                    this.grdUnallocated.BeforeColumnChooserDisplayed -= new BeforeColumnChooserDisplayedEventHandler(grdUnallocated_BeforeColumnChooserDisplayed);
                    grdUnallocated.Dispose();
                    grdUnallocated = null;
                }
                if (this.grdAllocated != null)
                {
                    this.grdAllocated.BeforeColumnChooserDisplayed -= new BeforeColumnChooserDisplayedEventHandler(grdAllocated_BeforeColumnChooserDisplayed);
                    grdAllocated.Dispose();
                    grdAllocated = null;
                }
                if (_selectedUnAllocatedCtrl != null)
                {
                    _selectedUnAllocatedCtrl.ChangePreference -= selectedUnAllocatedCtrl_ChangePreference;

                    _selectedUnAllocatedCtrl = null;
                }
                if (_allocationDefaultRuleControl != null)
                {
                    _allocationDefaultRuleControl.ChangePreference -= selectedUnAllocatedCtrl_ChangePreference;
                    _allocationDefaultRuleControl.Dispose();
                    _allocationDefaultRuleControl = null;
                }
                if (_allocationPrefs != null)
                {
                    _allocationPrefs = null;
                }
                if (_filterGridUnAllocated != null)
                {
                    _filterGridUnAllocated.Dispose();
                    _filterGridUnAllocated = null;
                }
                if (_filterGridAllocated != null)
                {
                    _filterGridAllocated.Dispose();
                    _filterGridAllocated = null;
                }
                if (_combinedFilter != null)
                {
                    _combinedFilter.Dispose();
                    _combinedFilter = null;

                }


                if (ctrlSwapParameters1 != null)
                {
                    ctrlSwapParameters1.Dispose();
                    ctrlSwapParameters1 = null;
                }
                // dispose cost adjustment main control while closing allocation UI 
                if (ctrlCostAdjustment != null)
                {
                    ctrlCostAdjustment.Dispose();
                    ctrlCostAdjustment = null;
                }



                // Basically checks if commission is also referencing the same data or not.
                //if (!PostTradeCacheManager.IsAllocDataReferenced)
                if (!AllocationManager.GetInstance().IsAllocDataReferenced)
                {
                    //PostTradeCacheManager.ClearData();
                    AllocationManager.GetInstance().ClearData();
                    AllocationManager.GetInstance().ClearPositions();
                }

                AllocationManager.GetInstance().NewGroupReceived -= new EventHandler(OnNewGroupReceived);
                AllocationManager.GetInstance().UpdateSymbolInfo -= new EventHandler(AllocationMain_UpdateSymbolInfo);
                AllocationManager.GetInstance().UpdateGroupClosingStatusHandler -= new EventHandler(AllocationMain_UpdateGroupClosingStatus);
                AllocationManager.GetInstance().AllocationPreferenceUpdated -= AllocationMain_AllocationPreferenceUpdated;
                AllocationManager.GetInstance().AllocationSchemeUpdated -= AllocationMain_AllocationSchemeUpdated;


                //modified by omshiv, http://jira.nirvanasolutions.com:8080/browse/CHMW-1510
                if (allocationDataChange != null)
                {
                    allocationDataChange(this, new EventArgs<bool>(false));
                }
                //CurrentPositionList.Dispose();
                if (AllocationClosed != null)
                    AllocationClosed(this, null);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }



        private void cmbbxdefaults_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //if (cmbbxdefaults.Value != null)
                //{
                //    int DefaultID = int.Parse(cmbbxdefaults.Value.ToString());
                //    AllocationDefault allocationDefault = _defaults.GetDefault(DefaultID);
                //    if (allocationDefault.DefaultAllocationLevelList != null)
                //    {
                //        accountStrategyMapping1.SetAllocationDefault(allocationDefault);
                //        selectedUnAllocatedCtrl.SetAllocationDefault(allocationDefault);
                //    }
                //}
                if (cmbbxdefaults.Value != null)
                {
                    UnBindCmbDefaultEvents();
                    if (Convert.ToInt32(cmbbxdefaults.Value.ToString()) == -1)
                    {
                        AllocationCompanyWisePref rule = AllocationManager.GetInstance().Allocation.InnerChannel.GetDefaultRule(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID);
                        if (rule == null)
                            _allocationDefaultRuleControl.SetValues(new AllocationRule());
                        else
                            _allocationDefaultRuleControl.SetValues(rule.DefaultRule);
                        // accountStrategyMapping1.SetAllocationDefault(new AllocationOperationPreference());
                        _selectedUnAllocatedCtrl.SetAllocationDefault(new AllocationOperationPreference());
                    }
                    else
                    {
                        int defaultID = int.Parse(cmbbxdefaults.Value.ToString());
                        lock (lockerObject)
                        {
                            AllocationOperationPreference allocationOperationPreference = _allocationPrefCache[defaultID];

                            if (allocationOperationPreference != null && _allocationDefaultRuleControl != null)
                            {
                                _allocationDefaultRuleControl.SetValues(allocationOperationPreference.DefaultRule);
                                accountStrategyMapping1.SetAllocationDefault(allocationOperationPreference);
                                _selectedUnAllocatedCtrl.SetAllocationDefault(allocationOperationPreference);
                            }
                        }
                    }

                    BindCmbDefaultEvents();
                }
                //if (grdUnallocated.ActiveRow != null)
                //{
                //    UltraGridRow row = grdUnallocated.ActiveRow;
                //    grdUnallocated.ActiveRow = null;
                //    grdUnallocated.ActiveRow = row;
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void cmbbxdefaults_Click(object sender, System.EventArgs e)
        {
            cmbbxdefaults_ValueChanged(this, null);
        }



        private void SelctedRow(UltraGrid grid)
        {
            try
            {
                if (grid.ActiveRow != null)
                {
                    if (grid.ActiveCell != null && grid.ActiveCell.Column.Key == "checkBox")
                    {
                        grid.ActiveCell.Value = !Convert.ToBoolean(grid.ActiveCell.Value);
                    }
                    grid.ActiveCell = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SelectedRowgrdAllocated(UltraGrid grid)
        {
            if (grid.ActiveRow != null)
            {
                if (grid.ActiveCell != null)
                {
                    grid.ActiveCell.Value = !Convert.ToBoolean(grid.ActiveCell.Value);
                }
            }

        }


        void headerCheckBoxAllocated__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        {
            try
            {
                grdAllocated.ActiveRow = null;
                MouseClickedOnAllocated();
                UltraGridRow[] rows = grdAllocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsAllocatedGrd = Convert.ToInt32(rows.Count(row => row.Cells["checkBox"].Text == true.ToString()));
                //Updating Trade counter
                UpdateTotalNoOfTradesForAllocatedgrd(_countSelectedRowsAllocatedGrd);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updating total no of trades for grdAllocated
        /// </summary>
        private void UpdateTotalNoOfTradesForAllocatedgrd(int countSelectedRows)
        {
            try
            {
                int total = 0;
                total = grdAllocated.Rows.GetFilteredInNonGroupByRows().Count();
                accountStrategyMapping1.SetTotalNoOfTrades(countSelectedRows, total);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void headerCheckBoxUnallocated__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        {
            try
            {
                grdUnallocated.ActiveRow = null;
                MouseClickedOnUnallocated();
                UltraGridRow[] rows = grdUnallocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsUnallocatedGrd = Convert.ToInt32(rows.Count(row => row.Cells["checkBox"].Text == true.ToString()));
                //Updating Trade counter
                UpdateTotalNoOfTradesForUnallocatedgrd(_countSelectedRowsUnallocatedGrd);
                if (grdUnallocated.Rows.Count > 0)
                    grdUnallocated.ActiveRow = grdUnallocated.Rows[0];
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updating total no of trades for grdUnallocated
        /// </summary>
        private void UpdateTotalNoOfTradesForUnallocatedgrd(int countSelectedRows)
        {
            try
            {
                int total = 0;
                total = grdUnallocated.Rows.GetFilteredInNonGroupByRows().Count();
                accountOnlyUserControl1.SetTotalNoOfTrades(countSelectedRows, total);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /*boolean variable to identify row is clicked or auto selected.
         * default value false
         * value changed to true in grdUnallocated_mouse_click, grdUnallocated_mouse_down, grdUnallocated_key_down
         * value changed to false in grdUnallocated_AfterRow_Activated, AllocateBy account
         */
        bool _isRowClicked = false;

        private void grdUnallocated_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                HeaderUIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(e.Location).GetAncestor(typeof(HeaderUIElement)) as HeaderUIElement;

                ///If header check box is clicked,then we will allow headerCheckBoxUnallocated__CLICKED to do the work, else this function will take care
                if (element != null && element.GetType().Equals(typeof(Infragistics.Win.UltraWinGrid.HeaderUIElement)))
                {
                    return;
                }

                // select the row in case of right click
                if (e.Button == MouseButtons.Left)
                {
                    SelctedRow(grdUnallocated);
                }
                MouseClickedOnUnallocated();
                _isRowClicked = true;

                //Calculating the selectedGrid row in the grid AddAndUpdateExternalTransactionID then Update trade counter
                UltraGridRow[] rows = grdUnallocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsUnallocatedGrd = Convert.ToInt32(rows.Count(row => row.Cells["checkBox"].Text == true.ToString()));
                UpdateTotalNoOfTradesForUnallocatedgrd(_countSelectedRowsUnallocatedGrd);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void MouseClickedOnUnallocated()
        {
            try
            {
                _selectedUnAllocatedCtrl.HideControl(true);
                btnAllocate.Enabled = true;
                _selectedGrid = grdUnallocated;
                bool isMultipleSelected = IsMultipleSelected(grdUnallocated);
                // accountStrategyMapping1.SetSelectionStatus(isMultipleSelected);
                // selectedUnAllocatedCtrl.SetSelectionStatus(isMultipleSelected);

                grdUnallocated_AfterRowActivate(null, null);

                ReSelectMethodology(isMultipleSelected);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdAllocated_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                HeaderUIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(e.Location).GetAncestor(typeof(HeaderUIElement)) as HeaderUIElement;

                ///If header check box is clicked,then we will allow headerCheckBoxUnallocated__CLICKED to do the work, else this function will take care
                if (element != null && element.GetType().Equals(typeof(Infragistics.Win.UltraWinGrid.HeaderUIElement)))
                {
                    return;
                }

                // select the row in case of right click
                if (e.Button == MouseButtons.Left)
                {
                    SelctedRow(grdAllocated);
                }

                //Calculating the selectedGrid row in the grid AddAndUpdateExternalTransactionID then Update trade counter
                UltraGridRow[] rows = grdAllocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsAllocatedGrd = Convert.ToInt32(rows.Count(row => row.Cells["checkBox"].Text == true.ToString()));
                UpdateTotalNoOfTradesForAllocatedgrd(_countSelectedRowsAllocatedGrd);
                MouseClickedOnAllocated();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        bool _isAllocatedRowSelected = false;
        private void MouseClickedOnAllocated()
        {
            try
            {
                _selectedUnAllocatedCtrl.HideControl(false);
                btnAllocate.Enabled = false;
                btnReAllocate.Enabled = true;
                _selectedGrid = grdAllocated;
                bool isMultipleSelected = IsMultipleSelected(grdAllocated);
                _isAllocatedRowSelected = true;
                grdAllocated_AfterRowActivate_1(null, null);
                accountStrategyMapping1.SetSelectionStatus(isMultipleSelected);
                ReSelectMethodology(isMultipleSelected);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ReSelectMethodology(bool isMultipleSelected)
        {
            try
            {
                /// This will ensure that default % values are picked up if any methodology is selected rather than individual trade's %
                if (isMultipleSelected)
                {
                    if (cmbbxdefaults.Value != null)
                    {
                        int tmpVAlue = Int32.Parse(cmbbxdefaults.Value.ToString());
                        //cmbbxdefaults.Value = int.MinValue;
                        cmbbxdefaults.Value = tmpVAlue;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        DateTime _openPositionofADate = DateTime.Today;

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                if (e.Tool.Key.Equals("preferences", StringComparison.OrdinalIgnoreCase))
                {

                    if (LaunchPreferences != null)
                        LaunchPreferences(this, e);
                }
                else if (e.Tool.Key.Equals("report", StringComparison.OrdinalIgnoreCase))
                {
                    //  _allocationReport = AllocationReport.GetInstance;
                    // WinUtilities.SetForegroundWindow(_allocationReport.Handle);
                    //if (!_allocationReport.Visible)
                    //    _allocationReport.Show();
                    AllocationReport allocationReport = AllocationReport.GetInstance;
                    allocationReport.loginUser = _loginUser;
                    allocationReport.StartPosition = FormStartPosition.Manual;
                    allocationReport.Location = new Point(this.Location.X + 30, this.Location.Y + 30);
                    allocationReport.Show();
                    allocationReport.BringToFront();
                }
                else if (e.Tool.Key.Equals("commission", StringComparison.OrdinalIgnoreCase))
                {
                    if (LaunchCommissionCalculation != null)
                    {
                        LaunchCommissionCalculation(this, e);
                    }
                }
                else if (e.Tool.Key.Equals("Recon Report", StringComparison.OrdinalIgnoreCase))
                {
                    if (cmbAllocationScheme.Value != null)
                    {
                        string allocationScheme = string.Empty;
                        if (rbtnAllocationBySymbol.CheckedIndex == 0)
                        {
                            allocationScheme = cmbAllocationScheme.Text;
                        }
                        else
                        {
                            allocationScheme = ApplicationConstants.C_COMBO_SELECT;
                        }
                        if (_schemeForm == null)
                        {
                            _schemeForm = new AllocationSchemeForm();
                            _schemeForm.FormClosedInformation += new EventHandler(_schemeForm_FormClosedInformation);
                            _schemeForm.Text = "Allocation Scheme Reconcilation";
                            _schemeForm.BringToFront();
                            _schemeForm.BindReconAllocationScheme(AllocationScheme.Recon, allocationScheme);
                            _schemeForm.Show();
                        }
                        else
                        {
                            _schemeForm.Show();
                            _schemeForm.BringToFront();
                        }

                    }
                }
                else if (e.Tool.Key.Equals("PreFetchFilter", StringComparison.OrdinalIgnoreCase))
                {

                    //Adding or removing filer grid from grid as control on buton click
                    if (grdUnallocated.Controls.Contains(_filterGridUnAllocated))
                    {
                        grdUnallocated.Controls.Remove(_filterGridUnAllocated);
                        grdUnallocated.DisplayLayout.Scrollbars = Scrollbars.Automatic;
                    }
                    else
                    {
                        _filterGridUnAllocated.Width = grdUnallocated.Width;
                        grdUnallocated.Controls.Add(_filterGridUnAllocated);
                        grdUnallocated.DisplayLayout.Scrollbars = Scrollbars.None;
                    }

                    if (grdAllocated.Controls.Contains(_filterGridAllocated))
                    {
                        grdAllocated.Controls.Remove(_filterGridAllocated);
                        grdAllocated.DisplayLayout.Scrollbars = Scrollbars.Automatic;
                    }
                    else
                    {
                        _filterGridAllocated.Width = grdAllocated.Width;
                        grdAllocated.Controls.Add(_filterGridAllocated);
                        grdAllocated.DisplayLayout.Scrollbars = Scrollbars.None;
                    }
                }
                else if (e.Tool.Key.Equals(" CalculateAllocationProrataPercent", StringComparison.OrdinalIgnoreCase))
                {
                    GetDate frmpositionDate = new GetDate();

                    if (frmpositionDate.ShowDialog() == DialogResult.OK)
                    {
                        _openPositionofADate = frmpositionDate.dtPositionDate.Value.Date;
                    }
                    else
                    {
                        frmpositionDate = null;
                        return;
                    }

                    frmpositionDate = null;

                    SaveAllocationSchemeASync();
                }
                else if (e.Tool.Key.Equals("ScreenShot", StringComparison.OrdinalIgnoreCase))
                {
                    SnapShotManager.GetInstance().TakeSnapshot(this);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// save allocation scheme on back groud thread to make UI more responsive
        /// </summary>
        private void SaveAllocationSchemeASync()
        {
            try
            {
                lblStatusStrip.Text = "Allocation Prorata Calculation Started, please wait...";

                BackgroundWorker saveAllocSchemeAsyncWorker = new BackgroundWorker();
                saveAllocSchemeAsyncWorker.DoWork += new DoWorkEventHandler(saveAllocSchemeAsyncWorker_DoWork);
                saveAllocSchemeAsyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(saveAllocSchemeAsyncWorker_RunWorkerCompleted);
                saveAllocSchemeAsyncWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void saveAllocSchemeAsyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Get position based on isMasterFundRatioAllocation enabled or not.
                bool isMFRatioSchemEnabled = _allocationPrefs.GeneralRules.isMasterFundRatioAllocation;
                int allocationSchemeID = AllocationSchemeImportHelper.SaveAllocationSchemeFromApp(_openPositionofADate, isMFRatioSchemEnabled);
                e.Result = allocationSchemeID;

                // Kashish G.,PRANA-8675:Commented the below condition and moved to RunWorkerCompleted as lblStatusStrip must be 
                // in RunWorkerCompleted to avoid cross-threading issue.

                //if (allocationSchemeID > 0)
                //{
                //    lblStatusStrip.Text = "Allocation Prorata % calculation completed";
                //}
                //else
                //{
                //    lblStatusStrip.Text = "Allocation Prorata % calculation failed";
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void saveAllocSchemeAsyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(e.Result) > 0)
                {
                    lblStatusStrip.Text = "Allocation Prorata % calculation completed";
                }
                else
                {
                    lblStatusStrip.Text = "Allocation Prorata % calculation failed";
                }

                if ((e.Cancelled == true))
                {
                    MessageBox.Show("Cancelled!", "Allocation Scheme", MessageBoxButtons.OK);
                }

                else if (!(e.Error == null))
                {
                    MessageBox.Show("Error: " + e.Error.Message, "Allocation Scheme", MessageBoxButtons.OK);
                }
                // Kuldeep Ag: http://jira.nirvanasolutions.com:8080/browse/PRANA-2547 
                // As discussed with Sandeep sir, this is no more needed as Allocation scheme will remain same.
                //else
                //{
                //    BindAllocationScheme();
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                lblStatusStrip.Text = string.Empty;
            }
        }

        void _schemeForm_FormClosedInformation(object sender, EventArgs e)
        {
            _schemeForm = null;
        }

        AllocationSchemeForm _schemeForm = null;
        private void chkbxAllocationCalculator_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkbxAllocationCalculator.Checked)
                {
                    _allocationDefaultRuleControl.Location = new System.Drawing.Point(500, 20);
                    ShowDefaultRuleControl();
                    allocationCalculatorUsrControl1.ClientArea.Controls.Add(_allocationDefaultRuleControl);
                    accountOnlyUserControl1.Visible = false;
                    allocationCalculatorUsrControl1.Visible = true;
                    ctrlSwapParameters1.Visible = false;
                    btnSaveSwap.Visible = false;
                    btnAllocate.Visible = true;
                    _selectedUnAllocatedCtrl = allocationCalculatorUsrControl1;
                }
                else
                {
                    _allocationDefaultRuleControl.Location = new System.Drawing.Point(accountOnlyUserControl1.GetMaxAccountLength() + 30, 20);
                    ShowDefaultRuleControl();
                    accountOnlyUserControl1.ClientArea.Controls.Add(_allocationDefaultRuleControl);
                    allocationCalculatorUsrControl1.Visible = false;
                    accountOnlyUserControl1.Visible = true;
                    ctrlSwapParameters1.Visible = false;
                    btnSaveSwap.Visible = false;
                    btnAllocate.Visible = true;
                    _selectedUnAllocatedCtrl = accountOnlyUserControl1;
                }
                cmbbxdefaults.Value = -1;

                //cmbbxdefaults_ValueChanged(null, null);
                // grdUnallocated_AfterRowActivate(null, null);
                // grdAllocated_AfterRowActivate_1(null, null);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private AllocationGroup GetSelectedGroup()
        {
            AllocationGroup selectedGroup = null;
            try
            {
                if (_selectedGrid != null && _selectedGrid.ActiveRow != null)
                {
                    if (_selectedGrid.ActiveRow.ListObject is AllocationGroup)
                    {
                        selectedGroup = (AllocationGroup)_selectedGrid.ActiveRow.ListObject;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            //switch (((ContextMenu)((MenuItem)sender).Parent).SourceControl.Name)
            //{
            //    case "grdAllocated":
            //        selectedGroup = (AllocationGroup)grdAllocated.ActiveRow.ListObject;
            //        break;
            //    case "grdUnallocated":
            //        selectedGroup = (AllocationGroup)grdUnallocated.ActiveRow.ListObject;
            //        break;
            //    default:
            //        break;
            //}
            return selectedGroup;
        }
        private void mnuSwap_Click(object sender, EventArgs e)
        {
            try
            {
                AllocationGroup selectedGroup = GetSelectedGroup();
                if (selectedGroup != null)
                {
                    PostTradeEnums.Status GroupStatus = _closingServices.InnerChannel.CheckGroupStatus(selectedGroup);
                    if (selectedGroup != null)
                    {
                        if (GroupStatus.Equals(PostTradeEnums.Status.None))
                        {
                            if (selectedGroup.SwapParameters == null)
                            {
                                SwapParameters swapParams = new SwapParameters();
                                swapParams.OrigCostBasis = selectedGroup.AvgPrice;
                                swapParams.OrigTransDate = selectedGroup.AUECLocalDate;
                                swapParams.NotionalValue = selectedGroup.CumQty * selectedGroup.AvgPrice;

                                ctrlSwapParameters1.Set(swapParams, SwapValidate.Allocate);
                            }
                        }
                        else
                        {
                            if (GroupStatus.Equals(PostTradeEnums.Status.Closed))
                            {
                                MessageBox.Show("Group is Partially or Fully Closed. Can't be booked as Swap", "Nirvana Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else if (GroupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                            {
                                MessageBox.Show(" Corporate Action has been applied on this Group. Can't be booked as Swap", "Nirvana Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }
                ShowSwapDetails();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void mnuSwapDetails_Click(object sender, EventArgs e)
        {
            try
            {
                AllocationGroup selectedGroup = GetSelectedGroup();
                selectedGroup.SwapParameters.NotionalValue = selectedGroup.CumQty * selectedGroup.AvgPrice;
                if (selectedGroup != null)
                {

                    if (selectedGroup.SwapParameters != null)
                    {
                        ctrlSwapParameters1.Set(selectedGroup.SwapParameters, SwapValidate.Allocate);
                    }
                }
                ShowSwapDetails();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void contextMnuAllocatedGrid_Popup(object sender, EventArgs e)
        {
            try
            {
                if (grdAllocated.Rows.Count > 0)
                {
                    #region To Display Generate Cash Transaction Menu

                    if (grdAllocated.ActiveRow != null && grdAllocated.ActiveRow.ListObject != null)
                    {
                        if (grdAllocated.ActiveRow.ListObject.GetType() == typeof(TaxLot))
                        {
                            mnuCashTran.Visible = true;
                            mnuSwapDetailsAllocated.Enabled = false;
                            mnuSwapAllocated.Enabled = false;
                        }
                        else
                        {
                            mnuSwapDetailsAllocated.Enabled = true;
                            mnuCashTran.Visible = false;
                            mnuSwapAllocated.Enabled = true;
                        }
                    }

                    #endregion

                    List<AllocationGroup> groups = GetSelectedGroups(grdAllocated);
                    //Added to enable mnnUnAllocate, PRANA-10754
                    if (groups.Count == 0 && _selectedGrid.ActiveRow != null && _selectedGrid.ActiveRow.ListObject is AllocationGroup)
                        groups.Add((AllocationGroup)_selectedGrid.ActiveRow.ListObject);

                    if (groups.Count > 0)
                    {
                        mnuUnAllocate.Enabled = true;
                    }
                    else
                    {
                        mnuUnAllocate.Enabled = false;
                    }
                    // for multiple group select, swap details/ book as swap should not appear
                    if (groups.Count < 2)
                    {
                        AllocationGroup group = GetSelectedGroup();
                        if (group == null)
                            return;
                        if (group.IsSwapped)
                        {
                            mnuSwapDetailsAllocated.Enabled = true;
                            mnuSwapAllocated.Enabled = false;
                        }
                        else
                        {
                            //swaps allowed only for equities
                            if (group.AssetID == (int)BusinessObjects.AppConstants.AssetCategory.Equity)
                            {
                                mnuSwapDetailsAllocated.Enabled = false;
                                mnuSwapAllocated.Enabled = true;
                            }
                            else
                            {
                                mnuSwapDetailsAllocated.Enabled = false;
                                mnuSwapAllocated.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        mnuSwapDetailsAllocated.Enabled = false;
                        mnuSwapAllocated.Enabled = false;
                    }
                }
                else
                {
                    mnuSwapAllocated.Enabled = false;
                    mnuSwapDetailsAllocated.Enabled = false;
                    mnuUnAllocate.Enabled = false;
                    mnuCashTran.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private List<string> ExcludedHeaderCheckBoxColumns()
        {
            List<string> gridHeaderCheckBoxExcludedColumns = new List<string>();
            try
            {
                gridHeaderCheckBoxExcludedColumns.Add("IsManualGroup");
                gridHeaderCheckBoxExcludedColumns.Add("IsPreAllocated");
                gridHeaderCheckBoxExcludedColumns.Add("IsSwapped");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return gridHeaderCheckBoxExcludedColumns;
        }

        private void contextMnuUnAllocatedGrid_Popup(object sender, EventArgs e)
        {
            try
            {
                if (grdUnallocated.Rows.Count > 0)
                {

                    if (grdUnallocated.ActiveRow != null && grdUnallocated.ActiveRow.ListObject != null)
                    {
                        if (grdUnallocated.ActiveRow.ListObject.GetType() == typeof(AllocationOrder))
                        {
                            mnuSwapUnallocated.Enabled = false;
                            mnuSwapDetailsUnAllocated.Enabled = false;
                        }
                        else
                        {
                            mnuSwapUnallocated.Enabled = true;
                            mnuSwapDetailsUnAllocated.Enabled = true;
                        }
                    }
                    List<AllocationGroup> groups = GetSelectedGroups(grdUnallocated);
                    if (groups.Count > 1)
                    {
                        mnuGroup.Enabled = true;
                    }
                    else
                    {
                        mnuGroup.Enabled = false;
                    }
                    if (groups.Count > 0)
                    {
                        mnuUnGroup.Enabled = true;
                    }
                    else
                    {
                        mnuUnGroup.Enabled = false;
                    }
                    // for multiple group select, swap details/ book as swap should not appear
                    if (groups.Count < 2)
                    {
                        AllocationGroup group = GetSelectedGroup();
                        if (group == null)
                        {
                            return;
                        }
                        if (group.IsSwapped)
                        {
                            mnuSwapDetailsUnAllocated.Enabled = true;
                            mnuSwapUnallocated.Enabled = false;
                            mnuUnGroup.Enabled = false;
                        }
                        else
                        {
                            //swaps allowed only for equities
                            if (group.AssetID == (int)BusinessObjects.AppConstants.AssetCategory.Equity)
                            {
                                mnuSwapDetailsUnAllocated.Enabled = false;
                                mnuSwapUnallocated.Enabled = true;
                            }
                            else
                            {
                                mnuSwapDetailsUnAllocated.Enabled = false;
                                mnuSwapUnallocated.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        mnuSwapDetailsUnAllocated.Enabled = false;
                        mnuSwapUnallocated.Enabled = false;
                    }
                }
                else
                {
                    mnuSwapUnallocated.Enabled = false;
                    mnuSwapDetailsUnAllocated.Enabled = false;
                    mnuGroup.Enabled = false;
                    mnuUnGroup.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnSaveSwap_Click(object sender, EventArgs e)
        {
            try
            {
                AllocationGroup selectedGroup = GetSelectedGroup();
                if (selectedGroup != null)
                {
                    PostTradeEnums.Status groupStatus = _closingServices.InnerChannel.CheckGroupStatus(selectedGroup);

                    if (groupStatus.Equals(PostTradeEnums.Status.None))
                    {
                        if (selectedGroup.IsSwapped)
                        {
                            ctrlSwapParameters1.GetSelectedParams(selectedGroup.SwapParameters, SwapValidate.Allocate);
                        }
                        else
                        {
                            selectedGroup.SwapParameters = ctrlSwapParameters1.GetSelectedParams(SwapValidate.Allocate);
                            if (selectedGroup.SwapParameters != null)
                            {
                                //set group ID in swap parameters, PRANA-13092
                                selectedGroup.SwapParameters.GroupID = selectedGroup.GroupID;
                                selectedGroup.IsSwapped = true;
                            }
                            else
                            {
                                return;
                            }
                        }
                        if (selectedGroup.PersistenceStatus == ApplicationConstants.PersistenceStatus.NotChanged)
                        {
                            selectedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
                        }
                        if (selectedGroup.TaxLots != null && selectedGroup.TaxLots.Count > 0)
                        {
                            foreach (TaxLot taxlotVar in selectedGroup.TaxLots)
                            {
                                taxlotVar.ISSwap = true;
                                taxlotVar.SwapParameters = selectedGroup.SwapParameters.Clone();
                                taxlotVar.SwapParameters.NotionalValue = (taxlotVar.TaxLotQty * selectedGroup.SwapParameters.NotionalValue) / selectedGroup.CumQty;
                                selectedGroup.UpdateTaxlotState(taxlotVar);
                            }
                        }
                        else
                        {
                            TaxLot updatedTaxlot = _proxyAllocationServices.InnerChannel.CreateUnAllocatedTaxLot((PranaBasicMessage)selectedGroup, selectedGroup.GroupID);
                            updatedTaxlot.ISSwap = true;
                            updatedTaxlot.SwapParameters = selectedGroup.SwapParameters;
                            selectedGroup.UpdateTaxlotState(updatedTaxlot);
                        }

                        // after saving swap details set asset category as equity swap, PRANA-10575
                        foreach (UltraGridRow row in grdAllocated.Selected.Rows)
                        {
                            if (row.ListObject.GetType().Name.Equals("AllocationGroup") && row.Cells[COL_ASSETNAME].Value.ToString().Equals("Equity") && row.Cells["IsSwapped"].Value.Equals(true))
                            {
                                string swap = "EquitySwap";
                                row.Cells["AssetCategory"].Value = (object)swap;
                            }
                        }
                        lblStatusStrip.Text = "Swap information updated";
                    }
                    else
                    {
                        if (groupStatus.Equals(PostTradeEnums.Status.Closed))
                        {
                            MessageBox.Show("Group is Partially or Fully Closed. Can't be booked as Swap", "Nirvana Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (groupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                        {
                            MessageBox.Show(" Corporate Action has been applied on this Group. Can't be booked as Swap", "Nirvana Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdUnallocated_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            try
            {
                HideSwapDetails();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void HideSwapDetails()
        {
            try
            {
                ctrlSwapParameters1.Visible = false;
                btnSaveSwap.Visible = false;
                btnAllocate.Visible = true;
                btnSwapClose.Visible = false;
                allocationCalculatorUsrControl1.Visible = chkbxAllocationCalculator.Checked;
                accountOnlyUserControl1.Visible = !chkbxAllocationCalculator.Checked;
                //  chkbxAllocationCalculator_CheckedChanged(null, null);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void ShowSwapDetails()
        {
            try
            {
                ctrlSwapParameters1.Visible = true;
                btnSwapClose.Visible = true;
                btnSaveSwap.Visible = true;
                btnAllocate.Visible = false;
                allocationCalculatorUsrControl1.Visible = false;
                accountOnlyUserControl1.Visible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AllocationMain_Load(object sender, EventArgs e)
        {
            try
            {
                //Disabled CostAdjustment tab in Allocation UI, PRANA-12922
                tabAllocation.Tabs[2].Enabled = false;
                //Hide delete button if release is not setup for ch
                if (!CachedDataManager.GetInstance.GetPranaReleaseViewType().Equals(PranaReleaseViewType.CHMiddleWare))
                {
                    btnDelete.Visible = false;
                }
                HideSwapDetails();
                ChangeIcon();
                if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                {
                    tabAllocation.Tabs[0].Visible = false;
                    //modified By sachin mishra Purpose:-Cost Adjustment tab should be disable for chmode Jira-CHMW-3363.
                    tabAllocation.Tabs[2].Visible = false;
                    ultraToolbarsManager1.Tools.Remove(ultraToolbarsManager1.Tools["Recon Report"]);
                    ultraToolbarsManager1.Tools.Remove(ultraToolbarsManager1.Tools[" CalculateAllocationProrataPercent"]);
                    //ValueListItemsCollection vl = (ultraToolbarsManager1.Tools["PrefetchFilter"] as Infragistics.Win.UltraWinToolbars.ComboBoxTool).ValueList.ValueListItems;
                    //ValueListItem item = vl.ValueList.FindByDataValue(FilterScope.Allocated.ToString());
                    //if(item!=null)
                    //vl.Remove(item);
                    //vl.Remove(vl.ValueList.FindByDataValue(FilterScope.Split.ToString()));
                    //vl.Remove(vl.ValueList.FindByDataValue(FilterScope.UnAllocated.ToString()));
                    rbCurrent.Visible = false;
                    btnAutoGrp.Visible = false;
                    ugbxHeaderFill.Visible = false;
                    rbHistorical.Location = new Point(rbHistorical.Location.X - rbCurrent.Size.Width, rbHistorical.Location.Y);
                    dtFromDatePickerAllocation.Location = new Point(dtFromDatePickerAllocation.Location.X - rbCurrent.Size.Width, dtFromDatePickerAllocation.Location.Y);
                    dtToDatePickerAllocation.Location = new Point(dtToDatePickerAllocation.Location.X - rbCurrent.Size.Width, dtToDatePickerAllocation.Location.Y);
                    btnGetAllocationData.Location = new Point(btnGetAllocationData.Location.X - rbCurrent.Size.Width, btnGetAllocationData.Location.Y);
                    this.FindForm().Text = "Edit Trades";
                }
                if (CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                //ctrlAmendmend1.DisableAllocationMain += new EventHandler(ctrlAmendmend1_DisableAllocationMain);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ChangeIcon()
        {
            try
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllocationMain));
                if (CustomThemeHelper.ApplyTheme)
                    this.Icon = ((System.Drawing.Icon)(resources.GetObject("AllocationTheme")));
                else
                    this.Icon = ((System.Drawing.Icon)(resources.GetObject("AllocationNoTheme")));
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Fires just after UI is loaded and shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AllocationMain_Shown(object sender, EventArgs e)
        {
            try
            {
                ShowDefaultRuleControl();
                this.KeyDown += new KeyEventHandler(AllocationMain_KeyDown);
                Infragistics.Win.UltraWinToolbars.ComboBoxTool sb = ultraToolbarsManager1.Tools["PrefetchFilter"] as Infragistics.Win.UltraWinToolbars.ComboBoxTool;

                if (sb != null)
                {
                    sb.ValueList.ValueListItems.Add(AllocationConstants.FilterScope.None.ToString());
                    if (CachedDataManager.GetInstance.GetPranaReleaseViewType() != PranaReleaseViewType.CHMiddleWare)
                    {
                        sb.ValueList.ValueListItems.Add(AllocationConstants.FilterScope.Allocated.ToString());
                        sb.ValueList.ValueListItems.Add(AllocationConstants.FilterScope.UnAllocated.ToString());
                    }
                    sb.ValueList.ValueListItems.Add(AllocationConstants.FilterScope.All.ToString());
                    if (CachedDataManager.GetInstance.GetPranaReleaseViewType() != PranaReleaseViewType.CHMiddleWare)
                    {
                        sb.ValueList.ValueListItems.Add(AllocationConstants.FilterScope.Split.ToString());
                    }
                    sb.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(sb_ToolValueChanged);
                }

                _formInilitisedFirstTime = false;
                //Filter setup
                AddFiltersToLists();//filter columns are assigned to variables
                //_filterGrid.AttachToToolbar(this.ultraToolbarsManager1);
                _filterGridUnAllocated = new FilterGrid("UnAllocated", unallocatedFilterColList);//creating filter grid with predefined filter column list
                _filterGridUnAllocated.FilterTitle = "UnAllocated";
                _filterGridUnAllocated.AttachToToolbar(this.ultraToolbarsManager1);

                _filterGridAllocated = new FilterGrid("Allocated", allocatedFilterColList);//creating filter grid with predefined filter column list
                _filterGridAllocated.FilterTitle = "Allocated";
                _filterGridAllocated.AttachToToolbar(this.ultraToolbarsManager1);

                _combinedFilter = FilterGrid.CombineFilters(_filterGridAllocated, _filterGridUnAllocated);
                _combinedFilter.FilterTitle = "All";
                _combinedFilter.AttachToToolbar(this.ultraToolbarsManager1);

                GetAllocationData();

                ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].Visible = false;
                ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].Visible = false;
                sb.SelectedIndex = sb.ValueList.ValueListItems.IndexOf(sb.ValueList.FindByDataValue(AllocationConstants.FilterScope.All));

                #region Initializing for the first time
                ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].Visible = false;
                ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].ShowInToolbarList = false;

                ultraToolbarsManager1.Toolbars[_combinedFilter.FilterTitle].Visible = true;
                ultraToolbarsManager1.Toolbars[_combinedFilter.FilterTitle].ShowInToolbarList = true;

                ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].Visible = false;
                ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].ShowInToolbarList = false;

                _currentFilterScope = AllocationConstants.FilterScope.All;
                #endregion
                if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER);
                }
                else
                {
                    SetButtonsColor();
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_ALLOCATION_MAIN);
                    if (CustomThemeHelper.ApplyTheme)
                    {
                        this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                        this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                        this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                        this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                        this.lblStatusStrip.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                        this.lblStatusStrip.ForeColor = System.Drawing.Color.WhiteSmoke;
                        this.lblStatusStrip.Font = new Font("Century Gothic", 9F);
                        this.statusLblDateTime.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                        this.statusLblDateTime.ForeColor = System.Drawing.Color.WhiteSmoke;
                        this.statusLblDateTime.Font = new Font("Century Gothic", 9F);
                        this.lblStatusStripProgress.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                        this.lblStatusStripProgress.ForeColor = System.Drawing.Color.WhiteSmoke;
                        this.lblStatusStripProgress.Font = new Font("Century Gothic", 9F);
                    }
                    CustomThemeHelper.SetThemeProperties(accountOnlyUserControl1, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_ALLOCATION_FUND_ONLY_CTRL);
                    CustomThemeHelper.SetThemeProperties(accountStrategyMapping1, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_ALLOCATION_FUND_STRATEGY_CTRL);
                }
                cmbbxdefaults.Value = -1;
                //Modified By Kashish Goyal
                //Date: 12 March 2015
                //Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-6049
                //[EditTrades] Buttons not visible at the time of opening Edit Trades
                //TODO: Need to improve this code as it is a temporary solution only.                
                this.Size = new System.Drawing.Size(this.Width + 1, this.Height);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void AllocationMain_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                btnGetAllocationData_Click(btnGetAllocationData, e);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// saves the current filter status.
        /// </summary>
        private AllocationConstants.FilterScope _currentFilterScope = AllocationConstants.FilterScope.All;

        /// <summary>
        /// Fires when the filter type is changed. Handles the filter toolbars.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sb_ToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinToolbars.ComboBoxTool sb = sender as Infragistics.Win.UltraWinToolbars.ComboBoxTool;
                if (String.Compare(sb.Value.ToString(), _currentFilterScope.ToString(), true) != 0)
                {
                    string currentNew = sb.Value.ToString();
                    switch (currentNew)
                    {
                        case "Allocated":
                            ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].Visible = true;
                            ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].ShowInToolbarList = true;

                            ultraToolbarsManager1.Toolbars[_combinedFilter.FilterTitle].Visible = false;
                            ultraToolbarsManager1.Toolbars[_combinedFilter.FilterTitle].ShowInToolbarList = false;

                            ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].Visible = false;
                            ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].ShowInToolbarList = false;
                            _currentFilterScope = AllocationConstants.FilterScope.Allocated;
                            break;
                        case "UnAllocated":
                            ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].Visible = false;
                            ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].ShowInToolbarList = false;

                            ultraToolbarsManager1.Toolbars[_combinedFilter.FilterTitle].Visible = false;
                            ultraToolbarsManager1.Toolbars[_combinedFilter.FilterTitle].ShowInToolbarList = false;

                            ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].Visible = true;
                            ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].ShowInToolbarList = true;

                            _currentFilterScope = AllocationConstants.FilterScope.UnAllocated;
                            break;
                        case "All":
                            ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].Visible = false;
                            ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].ShowInToolbarList = false;

                            ultraToolbarsManager1.Toolbars[_combinedFilter.FilterTitle].Visible = true;
                            ultraToolbarsManager1.Toolbars[_combinedFilter.FilterTitle].ShowInToolbarList = true;

                            ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].Visible = false;
                            ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].ShowInToolbarList = false;

                            _currentFilterScope = AllocationConstants.FilterScope.All;
                            break;
                        case "Split":
                            ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].Visible = true;
                            ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].ShowInToolbarList = true;

                            ultraToolbarsManager1.Toolbars[_combinedFilter.FilterTitle].Visible = false;
                            ultraToolbarsManager1.Toolbars[_combinedFilter.FilterTitle].ShowInToolbarList = false;

                            ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].Visible = true;
                            ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].ShowInToolbarList = true;

                            _currentFilterScope = AllocationConstants.FilterScope.Split;
                            break;
                        case "None":
                            ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].Visible = false;
                            ultraToolbarsManager1.Toolbars[_filterGridAllocated.FilterTitle].ShowInToolbarList = false;

                            ultraToolbarsManager1.Toolbars[_combinedFilter.FilterTitle].Visible = false;
                            ultraToolbarsManager1.Toolbars[_combinedFilter.FilterTitle].ShowInToolbarList = false;

                            ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].Visible = false;
                            ultraToolbarsManager1.Toolbars[_filterGridUnAllocated.FilterTitle].ShowInToolbarList = false;

                            _currentFilterScope = AllocationConstants.FilterScope.None;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnSwapClose_Click(object sender, EventArgs e)
        {
            try
            {
                HideSwapDetails();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void mnuItemSwap_Click(object sender, EventArgs e)
        {

        }

        private void AllocationMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {


                //e.Cancel = true;
                DialogResult userChoice = PromptForDataSaving(AllocationConstants.ActionAfterSavingData.CloseAllocation);
                if (userChoice == DialogResult.Yes)
                {
                    e.Cancel = true;
                }

                //ctrlAmendmend1.DisableAllocationMain -= new EventHandler(ctrlAmendmend1_DisableAllocationMain);

                //_proxyAllocationServices.Dispose();


                //Testing without saving layouts
                //if (MessageBox.Show("Do you want to save layouts for filters?", "Filter for Allocation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //{
                //    if (_filterGrid != null)
                //        _filterGrid.SaveLayout();
                //    if (_filterGridUnAllocated != null)
                //        _filterGridUnAllocated.SaveLayout();
                //    if (_combinedFilter != null)
                //        _combinedFilter.SaveLayout();
                //}
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void AllocationMain_AllocationPreferenceUpdated(object sender, EventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { AllocationMain_AllocationPreferenceUpdated(sender, e); };
                    this.Invoke(del);
                }
                else
                {
                    LoadAllocationPreferences();
                    BindDefaults();
                    //Added to set default rule control, PRANA-11244
                    SetDefaultRuleControlPreferences();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Set General (Default Rule) Preferences in Allocation UI, PRANA-11244
        /// </summary>
        private void SetDefaultRuleControlPreferences()
        {
            try
            {
                //Getting Checkside value and using for force allocation check box.
                AllocationCompanyWisePref pref = AllocationManager.GetInstance().Allocation.InnerChannel.GetDefaultRule(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID);
                EnableDisableForceAllocationCheckbox(pref);
                if (pref.AllowEditPreferences)
                {
                    _allocationDefaultRuleControl.SetValues(pref.DefaultRule); 
                }
                _allocationDefaultRuleControl.Visible = pref.AllowEditPreferences;
                //To set dock of AccountAllocationUserControl
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-8124
                accountOnlyUserControl1.SetDockAccountAllocationControl(pref.AllowEditPreferences);
                //Added to set Precsion Digit in AccountStrategyAllocationControl class, PRANA-6387
                accountStrategyMapping1.SetPrecisionDigit(pref.PrecisionDigit);
                accountOnlyUserControl1.SetPrecisionDigit(pref.PrecisionDigit);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        ///// <summary>
        ///// Disables AllocationMain UI
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void ctrlAmendmend1_DisableAllocationMain(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        bool EnableStatus = (bool)sender;
        //        string message = string.Empty;
        //        if (EnableStatus)
        //        {
        //            message = "Allocation data saved.";
        //        }
        //        else
        //        {
        //            message = "Saving Data. Please wait...";
        //        }
        //        ToggleUIElementsWithMessage(message, EnableStatus);
        //        groupBox1.Enabled = false;
        //        btnAutoGrp.Enabled = false;
        //        btnCheckSide.Visible = false;
        //        btnClosing.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        private void mnuSaveColumns_Click(object sender, EventArgs e)
        {
            try
            {
                //_allocationPrefs.UnAllocatedColumns = UltraWinGridUtils.GetColumnsString(grdUnallocated);
                //_allocationPrefs.AllocatedColumns = UltraWinGridUtils.GetColumnsString(grdAllocated);

                //_allocationPrefs.UnallocatedColumnWidth = UltraWinGridUtils.GetColumnsWidthString(grdUnallocated);
                //_allocationPrefs.AllocatedColumnWidth = UltraWinGridUtils.GetColumnsWidthString(grdAllocated);

                // Done changes to save grid layout using infragistics save DispalyLayot feature
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7416
                // UnAllocated grid layot is saved
                SaveGridLayout(grdUnallocated);
                //Get Allocation Form Height and Width, PRANA-5836
                _allocationPrefs.AllocationFormHeight = this.Size.Height;
                _allocationPrefs.AllocationFormWidth = this.Size.Width;
                _allocationPrefs.SplitPanelSize = GetSplitPanelsSize();
                AllocationPreferencesManager.SavePreferences(_allocationPrefs);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private string GetSplitPanelsSize()
        {
            try
            {
                string panelsizes = string.Empty;
                panelsizes += splitContainer1Panel1.Width.ToString() + "," + splitContainer1Panel1.Height.ToString() + "," + splitContainer1Panel2.Width.ToString() + "," + splitContainer1Panel2.Height.ToString() + "," + splitContainer2Panel1.Width.ToString() + "," + splitContainer2Panel1.Height.ToString() + "," + splitContainer2Panel2.Width.ToString() + "," + splitContainer2Panel2.Height.ToString() + "," + splitContainer3Panel1.Width.ToString() + "," + splitContainer3Panel1.Height.ToString() + "," + splitContainer3Panel2.Width.ToString() + "," + splitContainer3Panel2.Height.ToString() + ",";
                return panelsizes;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        private void mnuUnallocateEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdUnallocated.ActiveRow != null)
                {
                    AllocationGroup group = grdUnallocated.ActiveRow.ListObject as AllocationGroup;
                    //group = (AllocationGroup)grdUnallocated.ActiveRow.ListObject;                
                    if (group != null)
                    {
                        tabAllocation.Tabs[1].Selected = true;
                        ctrlAmendmend1.EditGroupDetails(group);
                    }
                    ctrlAmendmend1.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Closed = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdUnallocated_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdUnallocated);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdAllocated_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdAllocated);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnCheckSide_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdAllocated.Rows.Count > 0)
                {
                    List<AllocationGroup> allocationGroups = new List<AllocationGroup>();
                    UltraGridRow[] rows = grdAllocated.Rows.GetFilteredInNonGroupByRows();
                    foreach (UltraGridRow row in rows)
                    {
                        allocationGroups.Add((AllocationGroup)row.ListObject);
                    }

                    List<TaxLot> taxLots = new List<TaxLot>();
                    foreach (AllocationGroup group in allocationGroups)
                    {
                        foreach (TaxLot taxlot in group.TaxLots)
                        {
                            taxLots.Add(taxlot);
                        }
                    }

                    List<TaxLot> virtuallyClosedTaxlots = _closingServices.InnerChannel.RunVirtualClosing(taxLots, Convert.ToDateTime(dtToDatePickerAllocation.Value), PostTradeEnums.CloseTradeAlogrithm.FIFO, false, false);
                    if (virtuallyClosedTaxlots.Count > 0)
                    {
                        Prana.AllocationNew.Allocation.UI.CheckSide checkSideForm = new Prana.AllocationNew.Allocation.UI.CheckSide();

                        //checkSideForm.StartPosition = FormStartPosition.CenterParent;
                        checkSideForm.TaxlotsList = virtuallyClosedTaxlots;
                        checkSideForm.ShowDialog();
                        //checkSideForm.BringToFront();

                        GenericBindingList<AllocationGroup> allocGrp = (GenericBindingList<AllocationGroup>)grdAllocated.DataSource;

                        UltraGridRow[] filteredRows = grdAllocated.Rows.GetFilteredInNonGroupByRows();
                        foreach (UltraGridRow row in filteredRows)
                        {
                            AllocationGroup grp = (AllocationGroup)row.ListObject;

                            foreach (TaxLot taxlot in grp.TaxLots)
                            {
                                foreach (TaxLot virTaxLot in virtuallyClosedTaxlots)
                                {
                                    if (virTaxLot.TaxLotID.Equals(taxlot.TaxLotID))
                                    {
                                        row.Appearance.BackColor = Color.LightPink;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        lblStatusStrip.Text = "No Conflict!";
                    }
                }
                else
                {
                    lblStatusStrip.Text = "Nothing to check!";
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnClosing_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    ClosingData closedData = _closingServices.InnerChannel.AutomaticClosing_Allocation(Convert.ToDateTime(dtToDatePickerAllocation.Value));

            //    if (closedData.IsDataClosed)
            //    {
            //        lblStatusStrip.Text = "Data Closed";
            //    }
            //    else if (!closedData.ErrorMsg.ToString().Equals(string.Empty))
            //    {
            //        MessageBox.Show(closedData.ErrorMsg.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    }
            //    else
            //    {
            //        InformationMessageBox.Display("Nothing to Close");
            //    }
            //}

            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

            //    if (rethrow)
            //    {
            //        throw ex;
            //    }
            //}
            try
            {
                ClosingWizardHelper.GetInstance().ClosingServices = _closingServices;
                ClosingWizardHelper.GetInstance().LaunchClosingWizard();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdAllocated_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    _selectedGrid = grdAllocated;
                    grdAllocated.Selected.Cells.Clear();
                    grdAllocated.Selected.Rows.Clear();

                    UIElement element_sel = grdAllocated.DisplayLayout.UIElement.ElementFromPoint(e.Location);
                    if (element_sel == null || element_sel.Parent == null)
                    { return; }

                    Infragistics.Shared.ISelectableItem theitem = element_sel.SelectableItem;
                    if (theitem is UltraGridCell)
                    {
                        grdAllocated.ActiveRow = ((UltraGridCell)theitem).Row;
                        grdAllocated.ActiveRow.Activate();
                        grdAllocated.ContextMenu = contextMnuAllocatedGrid;
                    }
                    else if (theitem is UltraGridRow)
                    {
                        grdAllocated.ActiveRow = (UltraGridRow)theitem;
                        grdAllocated.ActiveRow.Selected = true;
                        grdAllocated.ActiveRow.Activate();
                        grdAllocated.ContextMenu = contextMnuAllocatedGrid;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdUnallocated_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    _selectedGrid = grdUnallocated;
                    grdUnallocated.Selected.Cells.Clear();
                    grdUnallocated.Selected.Rows.Clear();

                    UIElement element_sel = grdUnallocated.DisplayLayout.UIElement.ElementFromPoint(e.Location);
                    if (element_sel == null || element_sel.Parent == null)
                    { return; }

                    Infragistics.Shared.ISelectableItem theitem = element_sel.SelectableItem;
                    if (theitem is UltraGridCell)
                    {
                        grdUnallocated.ActiveRow = ((UltraGridCell)theitem).Row;
                        grdUnallocated.ActiveRow.Activate();
                        grdUnallocated.ContextMenu = contextMnuUnAllocatedGrid;
                    }
                    else if (theitem is UltraGridRow)
                    {
                        grdUnallocated.ActiveRow = (UltraGridRow)theitem;
                        grdUnallocated.ActiveRow.Selected = true;
                        grdUnallocated.ActiveRow.Activate();
                        grdUnallocated.ContextMenu = contextMnuUnAllocatedGrid;
                    }
                }
                _isRowClicked = true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void rbtnAllocationBySymbol_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbtnAllocationBySymbol.CheckedIndex == 0)
                {
                    cmbAllocationScheme.Enabled = true;
                    cmbAllocationScheme.BringToFront();
                    cmbbxdefaults.Enabled = false;
                }
                else
                {
                    cmbbxdefaults.Enabled = true;
                    cmbbxdefaults.BringToFront();
                    cmbAllocationScheme.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdUnallocated_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData.Equals(Keys.Space) && grdUnallocated.ActiveRow != null && grdUnallocated.ActiveRow.Cells.Exists("checkBox"))
                {
                    string isSelected = grdUnallocated.ActiveRow.Cells["checkBox"].Text;
                    if (isSelected != string.Empty)
                    {
                        grdUnallocated.ActiveRow.Cells["checkBox"].Value = !Convert.ToBoolean(isSelected);
                        _countSelectedRowsUnallocatedGrd += Convert.ToBoolean(isSelected) ? -1 : 1;

                    }

                    //Calculating the selectedGrid row in the grid AddAndUpdateExternalTransactionID then Update trade counter
                    UpdateTotalNoOfTradesForUnallocatedgrd(_countSelectedRowsUnallocatedGrd);
                }
                _isRowClicked = true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdAllocated_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData.Equals(Keys.Space))
                {
                    if (grdAllocated.ActiveRow == null)
                        return;
                    if (grdAllocated.ActiveRow.Cells.Exists("checkBox"))
                    {
                        string isSelected = grdAllocated.ActiveRow.Cells["checkBox"].Text;
                        if (isSelected != string.Empty)
                        {
                            grdAllocated.ActiveRow.Cells["checkBox"].Value = !Convert.ToBoolean(isSelected);
                            _countSelectedRowsAllocatedGrd += Convert.ToBoolean(isSelected) ? -1 : 1;
                        }

                        //Calculating the selectedGrid row in the grid AddAndUpdateExternalTransactionID then Update trade counter
                        UpdateTotalNoOfTradesForAllocatedgrd(_countSelectedRowsAllocatedGrd);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        //private void AddAssetCategoryColumn(UltraGridBand gridband)
        //{
        //    try
        //    {
        //        gridband.Columns.Add("AssetCategory", "Asset Category");
        //        UltraGridColumn colAssetCategory = gridband.Columns["AssetCategory"];
        //        colAssetCategory.Width = 80;
        //        colAssetCategory.CellActivation = Activation.NoEdit;
        //        colAssetCategory.Header.VisiblePosition = 1;
        //        gridband.Columns[COL_ASSETNAME].Hidden = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void grdUnallocated_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //SetGridColumns(grdUnallocated, false);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdAllocated_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //SetGridColumns(grdAllocated, false);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// add new grid columns
        /// </summary>
        /// <param name="grid">The ultra grid</param>
        /// <param name="showColumn">parameter to hide/show columns</param>
        private void SetGridColumns(UltraGrid grid, bool showColumn)
        {
            try
            {
                //Added to fix column chooser, PRANA-4942
                grid.DisplayLayout.UseFixedHeaders = true;
                UltraGridBand gridBand = grid.DisplayLayout.Bands[0];
                if (gridBand != null)
                {
                   // Added Avg Price(Base) column, PRANA-10775

                    if (!gridBand.Columns.Exists(COL_AVGPRICEBASE))
                    {
                        UltraGridColumn colAvgPriceBase = gridBand.Columns.Add(COL_AVGPRICEBASE, CAPTION_AVGPRICE_BASE);
                        colAvgPriceBase.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        colAvgPriceBase.CellActivation = Activation.NoEdit;
                        colAvgPriceBase.Format = ApplicationConstants.FORMAT_QTY;
                        gridBand.Columns[COL_AVGPRICEBASE].Hidden = showColumn;
                    }

                    if (!gridBand.Columns.Exists(COL_NETMONEY))
                    {
                        UltraGridColumn colNetMoney = gridBand.Columns.Add(COL_NETMONEY, CAPTION_NETMONEY_SETTLEMENT);
                        colNetMoney.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        colNetMoney.CellActivation = Activation.NoEdit;
                        colNetMoney.Format = ApplicationConstants.FORMAT_QTY;
                        gridBand.Columns[COL_NETMONEY].Hidden = showColumn;
                    }

                    if (!gridBand.Columns.Exists(COL_NETAMOUNTBASE)) // net amount(base)
                    {
                        UltraGridColumn colNetAmountBase = gridBand.Columns.Add(COL_NETAMOUNTBASE, CAPTION_NETAMOUNT_BASE);
                        colNetAmountBase.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        colNetAmountBase.CellActivation = Activation.NoEdit;
                        colNetAmountBase.Format = ApplicationConstants.FORMAT_QTY;
                        gridBand.Columns[COL_NETAMOUNTBASE].Hidden = showColumn;
                    }

                    if (!gridBand.Columns.Exists(COL_NETAMOUNT))
                    {
                        UltraGridColumn colallocatedPayReceive = gridBand.Columns.Add(COL_NETAMOUNT, CAP_NETAMOUNT);
                        colallocatedPayReceive.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        colallocatedPayReceive.CellActivation = Activation.NoEdit;
                        colallocatedPayReceive.Format = ApplicationConstants.FORMAT_QTY;
                        gridBand.Columns[COL_NETAMOUNT].Hidden = showColumn;
                    }

                    if (!gridBand.Columns.Exists(COL_ASSETCATEGORY))
                    {
                        //AddAssetCategoryColumn(gridBand);
                        gridBand.Columns.Add("AssetCategory", "Asset Category");
                        UltraGridColumn colAssetCategory = gridBand.Columns["AssetCategory"];
                        colAssetCategory.Width = 80;
                        colAssetCategory.CellActivation = Activation.NoEdit;
                        colAssetCategory.Header.VisiblePosition = 1;
                        gridBand.Columns[COL_ASSETNAME].Hidden = showColumn;
                    }
                    if (!gridBand.Columns.Exists(COL_NETAMOUNTWITHCOMMISSION))
                    {
                        UltraGridColumn colNetAmtWithCom = gridBand.Columns.Add(COL_NETAMOUNTWITHCOMMISSION,
                            CAP_NETAMOUNTWITHCOMMISSION);
                        colNetAmtWithCom.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        colNetAmtWithCom.CellActivation = Activation.NoEdit;
                        colNetAmtWithCom.Format = ApplicationConstants.FORMAT_QTY;
                        gridBand.Columns[COL_NETAMOUNTWITHCOMMISSION].Hidden = showColumn;
                    }

                    //Added principal amount base and local, PRANA-11379
                    if (!gridBand.Columns.Exists(COL_PRINCIPALAMOUNTBASE))
                    {
                        UltraGridColumn colPrincipalAmountBase = gridBand.Columns.Add(COL_PRINCIPALAMOUNTBASE,
                            CAPTION_PRINCIPALAMOUNTBASE);
                        colPrincipalAmountBase.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        colPrincipalAmountBase.CellActivation = Activation.NoEdit;
                        colPrincipalAmountBase.Format = ApplicationConstants.FORMAT_QTY;
                        gridBand.Columns[COL_PRINCIPALAMOUNTBASE].Hidden = showColumn;
                    }

                    if (!gridBand.Columns.Exists(COL_PRINCIPALAMOUNTLOCAL))
                    {
                        UltraGridColumn colPrincipalAmountLocal = gridBand.Columns.Add(COL_PRINCIPALAMOUNTLOCAL,
                            CAPTION_PRINCIPALAMOUNTLOCAL);
                        colPrincipalAmountLocal.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        colPrincipalAmountLocal.CellActivation = Activation.NoEdit;
                        colPrincipalAmountLocal.Format = ApplicationConstants.FORMAT_QTY;
                        gridBand.Columns[COL_PRINCIPALAMOUNTLOCAL].Hidden = showColumn;
                    }

                    UltraGridColumn colallocatedComAmt = gridBand.Columns["CommissionAmt"];
                    colallocatedComAmt.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    colallocatedComAmt.Hidden = showColumn;

                    UltraGridColumn colallocatedComRate = gridBand.Columns["CommissionRate"];
                    colallocatedComRate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    colallocatedComRate.Hidden = showColumn;

                    UltraGridColumn colSoftAllocatedCommAmt = gridBand.Columns["SoftCommissionAmt"];
                    colSoftAllocatedCommAmt.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    colSoftAllocatedCommAmt.Hidden = showColumn;

                    UltraGridColumn colSoftAllocatedCommRate = gridBand.Columns["SoftCommissionRate"];
                    colSoftAllocatedCommRate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    colSoftAllocatedCommRate.Hidden = showColumn;

                    if (grid.Name.Equals(AllocationConstants.AllocationGrid.grdUnallocated.ToString()))
                    {
                        if (!gridBand.Columns.Exists(COL_ImportFileName))
                        {
                            UltraGridColumn importFileNameColumn = gridBand.Columns.Add(COL_ImportFileName, CAPTION_ImportFileName);
                            importFileNameColumn.DataType = typeof(String);
                        }
                    }
                }

                foreach (UltraGridBand band in grid.DisplayLayout.Bands)
                {
                    if (band != null && band.Columns != null)
                    {
                        AddSettlementCurrencyColumnsAndBindEnum(band.Columns);
                        AddChangeTypeAndBindEnum(band.Columns);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                }
            }

        private void lblStatusStrip_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lblStatusStrip.Text != string.Empty)
                    statusLblDateTime.Text = "[" + DateTime.Now.ToString() + "]";

                else
                    statusLblDateTime.Text = string.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                }
            }

        private void timerProgress_Tick(object sender, EventArgs e)
        {
            try
            {
                StringBuilder progress = new StringBuilder(lblStatusStripProgress.Text);
                if (progress.Length < 10)
                {
                    progress.Append(". ");
                    lblStatusStripProgress.Text = progress.ToString();
                }
                else
                    lblStatusStripProgress.Text = String.Empty;
            }
            catch (Exception ex)
            {
                lblStatusStripProgress.Text = String.Empty;
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGONLY);
                if (rethrow)
                    throw;
                }
            }

        #region IAllocation Members
        #endregion

        #region IAllocation Members


        //public event AllocationDataChangeHandler allocationDataChange;
        public event EventHandler<EventArgs<bool>> allocationDataChange;

        //public event LoadCloseTradeUIFromAllocationHandler loadCloseTradeUIFromAllocation;
        public event EventHandler<EventArgs<AllocationGroup>> loadCloseTradeUIFromAllocation;
        //public event LoadSymbolLookUpUIFromAllocationHandler loadSymbolLookUpUIFromAllocation;
        public event EventHandler<EventArgs<string>> loadSymbolLookUpUIFromAllocation;
        #endregion


        public void OnNewGroupReceived(object sender, EventArgs e)
        {
            try
            {
                //Added to check disposing condition, PRANA-9153
                if (!this.Disposing || !this.IsDisposed)
                {
                    if (UIValidation.GetInstance().validate(this))
                    {
                        if (this.InvokeRequired)
                        {
                            EventHandler newGroupHandler = OnNewGroupReceived;
                            this.Invoke(newGroupHandler, new Object[] { sender, EventArgs.Empty });
                        }
                        else
                        {
                            AccountCollection accounts = CachedDataManager.GetInstance.GetUserAccounts();
                            // List<string> firstNames = (from person in accounts select person.AccountID).ToList();
                            System.Object[] dataList = (System.Object[])sender;
                            foreach (Object obj in dataList)
                            {

                                AllocationGroup group = (AllocationGroup)obj;
                                bool isGroupAllowed = true;

                                TradingAccountCollection tradingAccounts = Prana.CommonDataCache.ClientsCommonDataManager.GetTradingAccounts(loginUser.CompanyUserID);
                                var accountIdList = new List<int>();
                                foreach (Prana.BusinessObjects.TradingAccount acc in tradingAccounts)
                                {
                                    if (!accountIdList.Contains(acc.TradingAccountID))
                                        accountIdList.Add(acc.TradingAccountID);
                                }

                                if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED && !accountIdList.Contains(group.TradingAccountID) && group.TradingAccountID != 0)
                                {
                                    AllocationManager.GetInstance().AddOrRemoveGroupOnTradingAccount(group,false);
                                    continue;
                                }
                                else if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                                    AllocationManager.GetInstance().AddOrRemoveGroupOnTradingAccount(group,true);



                                //Added By Faisal Shah
                                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1612
                                #region remove group if Dates are not in range of Selected Dates
                                //No need to check for Deleted and Ungrouped Groups
                                if (group.PersistenceStatus != ApplicationConstants.PersistenceStatus.Deleted && group.PersistenceStatus != ApplicationConstants.PersistenceStatus.UnGrouped)
                                {
                                    /* As per the latest change in the meaning  of current in allocation UI, 
                                     * adjusting from date in case current is selected.
                                     */
                                    DateTime fromDate = DateTime.UtcNow;
                                    DateTime toDate = DateTime.UtcNow;
                                    if (rbHistorical.CheckedIndex == -1)
                                    {
                                        fromDate = DateTime.UtcNow.Date.AddDays(-1 * Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_NoOfDaysAsCurrentForAllocation)));
                                    }
                                    else
                                    {
                                        toDate = (DateTime)dtToDatePickerAllocation.Value;
                                        fromDate = (DateTime)dtFromDatePickerAllocation.Value;
                                    }
                                    if (!(fromDate.Date <= group.AUECLocalDate.Date && toDate.Date >= group.AUECLocalDate.Date))
                                    {
                                        isGroupAllowed = false;
                                    }
                                }

                                #endregion

                                #region remove group if user don't have the permission of account in it

                                foreach (TaxLot taxLot in group.TaxLots)
                                {
                                    if (!accounts.Contains(taxLot.Level1ID))
                                    {
                                        isGroupAllowed = false;
                                        AllocationManager.GetInstance().AddOrRemoveGroupOnAccount(group,false);
                                        break;
                                    }
                                }
                                if (isGroupAllowed)
                                {
                                    AllocationManager.GetInstance().AddOrRemoveGroupOnAccount(group, true);
                                }

                                if (!isGroupAllowed)
                                {
                                    continue;
                                }
                                #endregion

                                if (group.PersistenceStatus != ApplicationConstants.PersistenceStatus.Deleted && group.PersistenceStatus != ApplicationConstants.PersistenceStatus.UnGrouped)
                                {
                                    NameValueFiller.FillNameDetailsOfMessage(group);
                                    AllocationManager.GetInstance().SetDefaultPersistenceStatus(group);

                                    bool isGroupDirty = AllocationManager.GetInstance().IsGroupsDirty(group);

                                    bool isGroupDeleted = AllocationManager.GetInstance().IsGroupDeleted(group.GroupID);

                                    if (isGroupDirty || isGroupDeleted)
                                    {
                                        ToggleUIElementsWithMessage("Please click 'Get Data' to fetch the updated data from server.", false);
                                        btnGetAllocationData.Enabled = true;
                                        //_getDataBlinkTimer.Start();

                                        if (allocationDataChange != null)
                                        {
                                            allocationDataChange(this, new EventArgs<bool>(true));
                                        }

                                        return;
                                    }

                                    AllocationManager.GetInstance().AddGroup(group);
                                    if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                                        ctrlCostAdjustment.UpdateTaxlots(group.TaxLots);

                                    //Remove taxlot from taxlot list in case of order side is unallocated
                                    if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                                        ctrlCostAdjustment.RemoveTaxlotsByGroupID(group.GetAllTaxlots());
                                }
                                else
                                {
                                    AllocationManager.GetInstance().DeleteGroup(group.GroupID);
                                    //Remove taxlot from taxlot list when cost adjustment is undone
                                    if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                                        ctrlCostAdjustment.RemoveTaxlots(group.TaxLots);
                                }
                            }

                        }
                    } 
                }
                //Updating Trade counter
                UltraGridRow[] rows = grdAllocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsAllocatedGrd = Convert.ToInt32(rows.Count(row => row.Cells["checkBox"].Text == true.ToString()));

                UltraGridRow[] rows1 = grdUnallocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsUnallocatedGrd = Convert.ToInt32(rows1.Count(row1 => row1.Cells["checkBox"].Text == true.ToString()));

                UpdateTotalNoOfTradesForAllocatedgrd(_countSelectedRowsAllocatedGrd);
                UpdateTotalNoOfTradesForUnallocatedgrd(_countSelectedRowsUnallocatedGrd);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        void tabAllocation_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
        {
            try
            {
                //Select Applied Tab on Cost Adjustment be default
                int tabIndex = e.Tab.Index;
                if (tabIndex == 2)
                {
                    ctrlCostAdjustment.SelectedTab();
                }
                // check for changes in Cost Adjustment tab
                if (ctrlCostAdjustment.IsAnythingChanged())
                {
                    // passing empty string as no action is required after save
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-8204
                    CostAdjustmentSaveDataPrompt(string.Empty);
                }
                else if (AllocationManager.GetInstance().AnythingChanged())
                {
                    DialogResult diagRes = PromptForDataSaving(AllocationConstants.ActionAfterSavingData.ChangeTab, e.Tab.Key);
                    if (diagRes != DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        if (tabAllocation.SelectedTab.Text == "Allocation")
                        {
                            GetAllocationDataWithoutPrompt();
                        }
                        else
                        {
                            ctrlAmendmend1.CancelEditChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        // AYG 05282013: Disable GroupBox, Auto Group button, CheckSide button, Closing button when Edit Trade Tab is selected.
        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                int tabIndex = e.Tab.Index;
				// disable buttons in case of applied tab on cost adjustment, PRANA-10427
                if (tabIndex == 0 || tabIndex == 1)
                {
                    btnCancelData.Enabled = true;
                    btnSave.Enabled = true;
                    btnSaveWOState.Enabled = true;
                }
                if (tabIndex == 1 || tabIndex == 2)
                {
                    ugbxHeaderFill.Enabled = false;
                    btnAutoGrp.Enabled = false;
                    btnCheckSide.Visible = false;
                    btnClosing.Visible = false;
                    //modified by sachin mishra 22/jan/2015  JIRA No-CHMW-2379 for removing the spaces between buttons
                    btnDelete.Location = btnClosing.Location;
                }
                else
                {
                    ugbxHeaderFill.Enabled = true;
                    btnAutoGrp.Enabled = true;
                    btnCheckSide.Visible = true;
                    btnClosing.Visible = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnCancelData_Click_1(object sender, EventArgs e)
        {
            try
            {
                // added check for cost adjsutment changes
                if (!AllocationManager.GetInstance().AnythingChanged() && !ctrlCostAdjustment.IsAnythingChanged())
                    lblStatusStrip.Text = "Nothing to Cancel.";

                else if (tabAllocation.SelectedTab.Text == "Allocation")
                    this.Close();

                else
                {
                    if (PromptForDataSaving(AllocationConstants.ActionAfterSavingData.CancelEditChanges) != DialogResult.Yes)
                    {
                        ctrlAmendmend1.CancelEditChanges();
                    }
                }
                //Added check for Null, PRANA-11461
                if(timerClear!=null)
                    timerClear.Start();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void mnuExpandCollapseAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (((System.Windows.Forms.Menu)(sender)).Name == "ExpandCollapseAllocated")
                {
                    // added function call to expand/collapse grid rows
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-6538
                    expandCollapseGridRows(grdAllocated);
                }
                else if (((System.Windows.Forms.Menu)(sender)).Name == "ExpandCollapseUnallocated")
                {
                    // added function call to expand/collapse grid rows
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-6538
                    expandCollapseGridRows(grdUnallocated);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void btnClear_Click(object sender, System.EventArgs e)
        {
            try
            {
                // if clear grid is selected in preference, then allocation qty/% should not populate after selecting another trade
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7615
                _targetPergentage.Clear();
                _selectedUnAllocatedCtrl.ClearPercentage();
                _selectedUnAllocatedCtrl.ClearQty();
                cmbbxdefaults.Value = -1;
                //_allocationDefaultRuleControl.SetValues(new AllocationRule());
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region AllocationNew

        private Dictionary<int, AllocationOperationPreference> _allocationPrefCache = new Dictionary<int, AllocationOperationPreference>();

        private object lockerObject = new object();

        /// <summary>
        /// 
        /// </summary>
        private void BindCmbDefaultEvents()
        {
            try
            {
                _selectedUnAllocatedCtrl.ChangePreference += selectedUnAllocatedCtrl_ChangePreference;
                accountStrategyMapping1.ChangePreference += selectedUnAllocatedCtrl_ChangePreference;
                _allocationDefaultRuleControl.ChangePreference += selectedUnAllocatedCtrl_ChangePreference;
                ctrlCostAdjustment.ToggleUIElements += ctrlCostAdjustment_ToggleUIElements;
                ctrlCostAdjustment.EnableDisableSaveCancelButton += ctrlCostAdjustment_EnableDisableSaveCancelButton;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// event handler to enable disable save and cancel button on applied tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlCostAdjustment_EnableDisableSaveCancelButton(object sender, EventArgs<bool> e) 
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => ctrlCostAdjustment_EnableDisableSaveCancelButton(sender, e)));
                }
                else
                {
                    btnSave.Enabled = e.Value;
                    btnSaveWOState.Enabled = e.Value;
                    btnCancelData.Enabled = e.Value;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            } 
        }

        /// <summary>
        /// Enables UI elements when ToggleUIElements event is raised
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e"> The event</param>
        private void ctrlCostAdjustment_ToggleUIElements(object sender, EventArgs<string, bool> e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => ctrlCostAdjustment_ToggleUIElements(sender, e)));
                }
                else
                {
                    // set lblStatusStrip to string value in event
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-8204
                    lblStatusStrip.Text = e.Value.ToString();
                    ToggleUIElementsWithMessage(String.Empty, e.Value2);
                    if (timerProgress.Enabled)
                    {
                        if (e.Value2)
                        {
                            timerProgress.Stop();
                            lblStatusStripProgress.Text = String.Empty;
                        }
                        else
                            timerProgress.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => DisableAllocaionButtons()));
                }
                else
                {
                    //Disable buttons
                    DisableAllocaionButtons();
                }
            }
        }

        /// <summary>
        ///  Don't disable buttons if select tab is allocation
        /// </summary>
        void DisableAllocaionButtons()
        {
            try
            {
                // Don't disable buttons if select tab is allocation
                if (!(tabAllocation.SelectedTab.Text == "Allocation"))
                {
                    //Disable the ugbxHeaderFill,btnAutoGrp, btnCheckSide, btnClosing Buttons
                    ugbxHeaderFill.Enabled = false;
                    btnAutoGrp.Enabled = false;
                    btnCheckSide.Visible = false;
                    btnClosing.Visible = false;
                }
              }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UnBindCmbDefaultEvents()
        {
            try
            {
                _selectedUnAllocatedCtrl.ChangePreference -= selectedUnAllocatedCtrl_ChangePreference;
                accountStrategyMapping1.ChangePreference -= selectedUnAllocatedCtrl_ChangePreference;
                _allocationDefaultRuleControl.ChangePreference -= selectedUnAllocatedCtrl_ChangePreference;
                ctrlCostAdjustment.ToggleUIElements -= ctrlCostAdjustment_ToggleUIElements;
                ctrlCostAdjustment.EnableDisableSaveCancelButton -= ctrlCostAdjustment_EnableDisableSaveCancelButton;
             }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        SerializableDictionary<int, AccountValue> _targetPergentage = new SerializableDictionary<int, AccountValue>();

        SerializableDictionary<int, AccountValue> _strategyPergentage = new SerializableDictionary<int, AccountValue>();

        object preferenceLocker = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void selectedUnAllocatedCtrl_ChangePreference(object sender, EventArgs e)
        {
            try
            {
                if (sender is IAllocationCalculator)
                {
                    lock (preferenceLocker)
                    {
                        _targetPergentage = _selectedUnAllocatedCtrl.GetAllocationAccountValue();
                    }
                }
                else if (sender is AccountStrategyMappingUserCtrlNew)
                {
                    lock (_strategyPergentage)
                    {
                        _strategyPergentage = accountStrategyMapping1.GetAllocationAccountValue();
                    }
                }
                cmbbxdefaults.Value = null;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }



        #endregion

        private void ultraOptionSet2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbtnAllocationBySymbol.CheckedIndex == 0)
                {
                    rbtnAllocationByAccount.CheckedIndex = -1;
                    cmbAllocationScheme.Enabled = true;
                    cmbAllocationScheme.BringToFront();
                    cmbbxdefaults.Enabled = false;
                }
                else
                {
                    rbtnAllocationByAccount.CheckedIndex = 0;
                    cmbbxdefaults.Enabled = true;
                    cmbbxdefaults.BringToFront();
                    cmbAllocationScheme.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void rbtnAllocationByAccount_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                // disable account allocation panel when "Allocation By" methodology is Symbol, PRANA-10107
                if (rbtnAllocationByAccount.CheckedIndex == 0)
                {
                    accountOnlyUserControl1.Enabled = true;
                    rbtnAllocationBySymbol.CheckedIndex = -1;
                }
                else
                {
                    rbtnAllocationBySymbol.CheckedIndex = 0;
                    accountOnlyUserControl1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void rbCurrent_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbCurrent.CheckedIndex == -1)
                {
                    rbHistorical.CheckedIndex = 0;
                }
                else
                {
                    rbHistorical.CheckedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void rbHistorical_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbHistorical.CheckedIndex == -1)
                {
                    rbCurrent.CheckedIndex = 0;
                }
                else
                {
                    rbCurrent.CheckedIndex = -1;
                }
                ClearAllocationData();
                dtToDatePickerAllocation.Enabled = rbHistorical.CheckedIndex == -1 ? false : true;
                dtFromDatePickerAllocation.Enabled = rbHistorical.CheckedIndex == -1 ? false : true;
                if (rbHistorical.CheckedIndex == -1) // for current data get the latest data
                {
                    GetAllocationData();
                }

                //Updating total no. of trade field
                UpdateTotalNoOfTradesForAllocatedgrd(0);
                UpdateTotalNoOfTradesForUnallocatedgrd(0);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        void AllocationMain_Resize(object sender, System.EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.Height = Screen.PrimaryScreen.Bounds.Height;
                    this.Width = Screen.PrimaryScreen.Bounds.Width;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// To show default rule control on Allocation UI
        /// </summary>
        private void ShowDefaultRuleControl()
        {
            try
            {
                AllocationCompanyWisePref rule = AllocationManager.GetInstance().Allocation.InnerChannel.GetDefaultRule(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID);
                if (rule == null)
                {
                    _allocationDefaultRuleControl.Visible = true;
                    //To set dock of AccountAllocationUserControl
                    accountOnlyUserControl1.SetDockAccountAllocationControl(true);
                }
                else
                {
                    _allocationDefaultRuleControl.Visible = rule.AllowEditPreferences;
                    //To set dock of AccountAllocationUserControl
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-8124
                    accountOnlyUserControl1.SetDockAccountAllocationControl(rule.AllowEditPreferences);
                }
                _allocationDefaultRuleControl.Location = new System.Drawing.Point((accountOnlyUserControl1.GetMaxAccountLength() + 30), 20);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnClearAllocated_Click(object sender, EventArgs e)
        {
            try
            {
                // if clear grid is selected in preference, then allocation qty/% should not populate after selecting another trade
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7615
                _strategyPergentage.Clear();
                accountStrategyMapping1.SetAllocationDefault(new AllocationOperationPreference());
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Binds data to cost adjustment grid
        /// </summary>
        private void BindDataToCostAdjustmentUI()
        {
            try
            {
                #region unused code
                //string ToAllAUECDatesString = "";
                //string FromAllAUECDatesString = "";
                //string fromAllocatedAllAUECDatesString = "";

                //if (rbHistorical.CheckedIndex == -1)
                //{
                //    //ToAllAUECDatesString = TimeZoneHelper.GetAllAUECDateInUseAUECStr(DateTime.UtcNow);
                //    //FromAllAUECDatesString = TimeZoneHelper.GetAllAUECDateInUseAUECStr(DateTime.UtcNow);
                //    ToAllAUECDatesString = DateTime.UtcNow.Date.ToString();
                //    DateTime fromDate = DateTime.UtcNow.Date.AddDays(-1 * Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_NoOfDaysAsCurrentForAllocation)));
                //    FromAllAUECDatesString = fromDate.AddDays(1).ToString();
                //    fromAllocatedAllAUECDatesString = DateTime.UtcNow.Date.ToString();
                //}
                //else
                //{
                //    //ToAllAUECDatesString = TimeZoneHelper.GetSameDateInUseAUECStr(dtToDatePickerAllocation.DateTime);
                //    //FromAllAUECDatesString = TimeZoneHelper.GetSameDateInUseAUECStr(dtFromDatePickerAllocation.DateTime);
                //    ToAllAUECDatesString = ((DateTime)dtToDatePickerAllocation.Value).ToString();
                //    FromAllAUECDatesString = ((DateTime)dtFromDatePickerAllocation.Value).ToString();
                //    fromAllocatedAllAUECDatesString = ((DateTime)dtFromDatePickerAllocation.Value).ToString();
                //}
                //ctrlCostAdjustment.BindData(AllocationManager.GetInstance().AllocationServices.InnerChannel.GetAllOpenTaxlots
                //       (Convert.ToDateTime(fromAllocatedAllAUECDatesString).Date, Convert.ToDateTime(ToAllAUECDatesString).Date, false, "", "", "", "").AdjustedTaxlots);

                #endregion

                // Done changes to Bind data to new tab which shows saved cost adjustment taxlots
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7378
                List<AllocationGroup> groups = AllocationManager.GetInstance().AllocatedGroups.ToList();
                List<string> openTaxlotIds = new List<string>();
                openTaxlotIds.AddRange((from g in groups
                                        from t in g.TaxLots
                                        where t.ClosingStatus != ClosingStatus.Closed && g.ClosingStatus != ClosingStatus.Closed
                                        select t.TaxLotID).ToList());

                List<CostAdjustmentTaxlotForUndo> costAdjustmentSavedTaxlots = AllocationManager.GetInstance().AllocationServices.InnerChannel.GetCostAdjustmentSavedTaxlotsFromId(openTaxlotIds);

                ctrlCostAdjustment.BindData(AllocationManager.GetInstance().AllocationServices.InnerChannel.GetAllOpenCostAdjustmentTaxlots(openTaxlotIds), costAdjustmentSavedTaxlots);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Saves Allocated Grid Layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSaveAllocatedColumns_Click(object sender, EventArgs e)
        {
            try
            {
                // Done changes to save grid layout using infragistics save DispalyLayot feature
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7416
                // Allocated grid layot is saved
                SaveGridLayout(grdAllocated);
                //Get Allocation Form Height and Width, PRANA-5836
                _allocationPrefs.AllocationFormHeight = this.Size.Height;
                _allocationPrefs.AllocationFormWidth = this.Size.Width;
                _allocationPrefs.SplitPanelSize = GetSplitPanelsSize();
                AllocationPreferencesManager.SavePreferences(_allocationPrefs);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Save grid layout as XML
        /// </summary>
        /// <param name="grid"></param>
        private void SaveGridLayout(UltraGrid grid)
        {
            try
            {
                // to save layout with version,PRANA-10773
                File.WriteAllText(GetDirectoryPath() + @"\" + grid.Name + ".xml", AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID);
                System.IO.FileStream fs = new System.IO.FileStream(GetDirectoryPath() + @"\" + grid.Name + ".xml", System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);

                //grid.DisplayLayout.SaveAsXml(GetDirectoryPath() + @"\" + grid.Name + ".xml", PropertyCategories.All);
                grid.DisplayLayout.SaveAsXml(fs, PropertyCategories.All);
                fs.Close();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets directory path to save/load grid dispaly layout
        /// </summary>
        /// <returns>directory path</returns>
        private string GetDirectoryPath()
        {
            try
            {
                // Set path for saving grid layout
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7416
                string _startPath = System.Windows.Forms.Application.StartupPath;
                string _allocationPreferencesDirectoryPath = _startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                return _allocationPreferencesDirectoryPath;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// This event will raise after row filter change of allocated grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdAllocated_AfterRowFilterChanged(object sender, AfterRowFilterChangedEventArgs e)
        {
            try
            {
                //Calculating the selectedGrid row in the grid AddAndUpdateExternalTransactionID then Update trade counter
                UltraGridRow[] rows = grdAllocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsAllocatedGrd = Convert.ToInt32(rows.Count(row => row.Cells["checkBox"].Text == true.ToString()));
                UpdateTotalNoOfTradesForAllocatedgrd(_countSelectedRowsAllocatedGrd);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This event will raise after row filter change of Unallocated grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdUnallocated_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                //Calculating the selectedGrid row in the grid AddAndUpdateExternalTransactionID then Update trade counter
                UltraGridRow[] rows = grdUnallocated.Rows.GetFilteredInNonGroupByRows();
                _countSelectedRowsUnallocatedGrd = Convert.ToInt32(rows.Count(row => row.Cells["checkBox"].Text == true.ToString()));
                UpdateTotalNoOfTradesForUnallocatedgrd(_countSelectedRowsUnallocatedGrd);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// expand or collapse rows in grid
        /// </summary>
        /// <param name="grid">The ultraGrid</param>
        private void expandCollapseGridRows(UltraGrid grid)
        {
            try
            {
                UltraGridRow row = grid.ActiveRow;
                // added check if row is null, then take 1st row of grid as active row and expand/collapse accordingly
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-8578
                if (row == null && grid.Rows.Count > 0)
                    row = grid.Rows[0];
                // added check for parent row, so that expand collapse is always done according to current state of parent row
                if (row != null && row.ParentRow != null)
                    row = grid.ActiveRow.ParentRow;

                if (row != null)
                {
                    if (row.Expanded)
                    {
                        grid.Rows.CollapseAll(false);
                    }
                    else
                    {
                        grid.Rows.ExpandAll(false);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// set state of force allocation checkbox on basis of preference
        /// </summary>
        /// <param name="pref">The Allocation general Preference</param>
        private void EnableDisableForceAllocationCheckbox(AllocationCompanyWisePref pref)
        {
            try
            {
                chkboxForceAllocation.Enabled = pref.DoCheckSide;
                chkboxForceAllocation.Checked = !pref.DoCheckSide ? false : chkboxForceAllocation.Checked;
                if (pref.DoCheckSide && pref.DisableCheckSideForAssets != null)
                {
                    if (pref.DisableCheckSideForAssets.Count == 0)
                    {
                        chkboxForceAllocation.CheckState = CheckState.Unchecked;
                    }
                    else if (pref.DisableCheckSideForAssets.Count == CommonDataCache.CachedData.GetInstance().Asset.Count)
                    {
                        chkboxForceAllocation.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        chkboxForceAllocation.CheckState = CheckState.Indeterminate;
                    }
                }
                else
                    chkboxForceAllocation.CheckState = CheckState.Unchecked;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update Default rule
        /// </summary>
        /// <param name="pref"></param>
        private void UpdateAllocationDefaultRule(AllocationOperationPreference pref)
        {
            try
            {
                if (_allocationDefaultRuleControl.Visible)
                    pref.TryUpdateDefaultRule(_allocationDefaultRuleControl.GetCurrentValues());
                else
                    pref.TryUpdateDefaultRule(AllocationManager.GetInstance().Allocation.InnerChannel.GetDefaultRule(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID).DefaultRule);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdUnallocated_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();

        }

        private void grdAllocated_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }
}
