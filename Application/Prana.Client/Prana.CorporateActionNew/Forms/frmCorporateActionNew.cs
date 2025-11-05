using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using Prana.Admin.BLL;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.ClientCommon.BLL;
using Prana.CommonDataCache;
using Prana.CorporateActionNew.Classes;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.CorporateActionNew.Forms
{
    public partial class frmCorporateActionNew : Form, ICorporateActions, IPublishing
    {

        bool _isFirstTime = true;
        static string _startPath = string.Empty;
        int _userID = 0;

        private CorporateActionType _caTypeApply = CorporateActionType.NameChange;
        private CorporateActionType _caTypeUndo = CorporateActionType.All;
        private CorporateActionType _caTypeRedo = CorporateActionType.All;
        private bool _isProcessing = false;
        ISecurityMasterServices _smServices = null;
        private CAPreferences _caPreference = null;
        List<TradeAuditEntry> _tradeAuditCollection_CA = new List<TradeAuditEntry>();

        public frmCorporateActionNew()
        {
            try
            {
                InitializeComponent();
                string symbolLookupCaption = ultraToolbarsManager3.Tools["btnSymbolLookUp"].SharedPropsInternal.Caption;
                if (!ModuleManager.CheckModulePermissioning(symbolLookupCaption, symbolLookupCaption))
                    ultraToolbarsManager3.Tools["btnSymbolLookUp"].SharedPropsInternal.Enabled = false;
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

        ProxyBase<ICAServices> _caServicesProxy = null;
        private void CreateCAServicesProxy()
        {
            _caServicesProxy = new ProxyBase<ICAServices>("TradeCAServiceEndpointAddress");
        }

        public ISecurityMasterServices SecurityMasterClient
        {
            set
            {
                _smServices = value;
            }
        }

        DuplexProxyBase<ISubscription> _proxy;
        private void MakeProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _proxy.Subscribe(Topics.Topic_CreateGroup, null);
                _proxy.Subscribe(Topics.Topic_SecurityMaster, null);
                _proxy.Subscribe(Topics.Topic_Closing, null);
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

        #region ICorporateActions Members

        public Form Reference()
        {
            return this;
        }

        public new event EventHandler FormClosed;

        public event EventHandler LaunchSymbolLookup;
        Dictionary<int, string> _dictAccounts = new Dictionary<int, string>();
        Dictionary<int, string> _dictCounterParty = new Dictionary<int, string>();
        public void InitControl()
        {
            try
            {
                MakeProxy();

                CreateCAServicesProxy();
                _caServicesProxy.DisconnectedEvent += _caServicesProxy_DisconnectedEvent;
                _caServicesProxy.ConnectedEvent += _caServicesProxy_ConnectedEvent;
                //_caServicesProxy.InnerChannel.CAResponseReceived += new MsgReceivedDelegate(ProcessCAResponse);
                CommonHelper.REQUESTER_HASHCODE = this.GetHashCode();
                string errMessage = XMLCacheManager.Instance.LoadXML();
                if (!String.IsNullOrEmpty(errMessage))
                {
                    toolStripStatusLabel1.Text = errMessage;
                    return;
                }

                LoadLayoutPreferences();

                _dictAccounts = CommonDataCache.CachedDataManager.GetInstance.GetAccountsWithFullName();
                _dictCounterParty = CommonDataCache.CachedDataManager.GetInstance.GetUserCounterParties();

                multiSelectDropDownUnApplied.SetManualTheme(CustomThemeHelper.ApplyTheme);
                //add accounts to the check list default value will be unchecked
                multiSelectDropDownUnApplied.AddItemsToTheCheckList(_dictAccounts, CheckState.Checked);
                multiSelectDropDownUnApplied.AdjustCheckListBoxWidth();
                multiSelectDropDownUnApplied.TitleText = "Account";
                multiSelectDropDownUnApplied.SetTextEditorText("All Account(s) Selected");

                multiSelectDropDownNew.SetManualTheme(CustomThemeHelper.ApplyTheme);
                multiSelectDropDownNew.AddItemsToTheCheckList(_dictAccounts, CheckState.Checked);
                multiSelectDropDownNew.AdjustCheckListBoxWidth();
                multiSelectDropDownNew.TitleText = "Account";
                multiSelectDropDownNew.SetTextEditorText("All Account(s) Selected");

                CommonHelper.BindCACombo(cmbCATypeApply);
                CommonHelper.BindCAComboWithAll(cmbCATypeUndo);
                CommonHelper.BindCAComboWithAll(cmbCATypeRedo);

                CommonHelper.BindCounterPartyCombo(cmbCounterPartyUnApplied, _dictCounterParty);
                CommonHelper.BindCounterPartyCombo(cmbCounterParty, _dictCounterParty);

                ctrlCAEntryApply.InitControl(_caTypeApply, ControlType.Apply, _smServices);
                ctrlCAEntryUndo.InitControl(_caTypeUndo, ControlType.Undo, _smServices);
                ctrlCAEntryRedo.InitControl(_caTypeRedo, ControlType.Redo, _smServices);
                ctrlCAEntryRedo.AfterCellUpdate += new EventHandler(AfterCellUpdateRedo);
                ctrlCAEntryApply.AfterCellUpdate += new EventHandler(AfterCellUpdateApply);
                ctrlCAEntryUndo.OnPreviewUndoClick += new EventHandler(OnPreviewUndoClick);
                ctrlCAEntryRedo.OnPreviewRedoClick += new EventHandler(OnPreviewRedoClick);

                _caServicesProxy.InnerChannel.Initialize(CommonHelper.REQUESTER_HASHCODE);
                ctrlCAEntryApply.CorporateActionModified += new EventHandler(ctrlCorporateActionEntry1_CorporateActionModified);
                ctrlCAEntryRedo.CorporateActionModified += new EventHandler(ctrlCorporateActionEntry1_CorporateActionModified);

                if (_caLayoutPreferencesList.CounterPartyApply != int.MinValue)
                    cmbCounterParty.Value = _caLayoutPreferencesList.CounterPartyApply;

                if (_caLayoutPreferencesList.CounterPartyUnApplied != int.MinValue)
                    cmbCounterPartyUnApplied.Value = _caLayoutPreferencesList.CounterPartyUnApplied;

                if (_caLayoutPreferencesList.CAApplyLayoutPreferencesList.ContainsKey(cmbCATypeApply.Value.ToString()))
                    ctrlPositionsApply.InitControl(ControlType.Apply, _caLayoutPreferencesList.CAApplyLayoutPreferencesList[cmbCATypeApply.Value.ToString()].CAApplyColumns);
                else
                    ctrlPositionsApply.InitControl(ControlType.Apply, new List<ColumnData>());

                if (_caLayoutPreferencesList.CAApplyLayoutPreferencesList.ContainsKey(cmbCATypeUndo.Value.ToString()))
                    ctrlPositionsUndo.InitControl(ControlType.Undo, _caLayoutPreferencesList.CAUndoLayoutPreferencesList[cmbCATypeUndo.Value.ToString()].CAUndoColumns);
                else
                    ctrlPositionsUndo.InitControl(ControlType.Undo, new List<ColumnData>());

                if (_caLayoutPreferencesList.CAApplyLayoutPreferencesList.ContainsKey(cmbCATypeRedo.Value.ToString()))
                    ctrlPositionsRedo.InitControl(ControlType.Redo, _caLayoutPreferencesList.CARedoLayoutPreferencesList[cmbCATypeRedo.Value.ToString()].CARedoColumns);
                else
                    ctrlPositionsRedo.InitControl(ControlType.Redo, new List<ColumnData>());

                ctrlPositionsUndo.OnUndoCAClick += new EventHandler(OnUndoCAClick);
                ctrlPositionsRedo.OnRedoCAClick += new EventHandler(OnRedoCAClick);
                ctrlPositionsApply.OnApplyLayoutSave += new EventHandler(OnApplyLayoutSave);
                ctrlPositionsRedo.OnRedoLayoutSave += new EventHandler(OnRedoLayoutSave);
                ctrlPositionsUndo.OnUndoLayoutSave += new EventHandler(OnUndoLayoutSave);

                ctrlPositionsApply.OnApplyRemoveFilters += new EventHandler(OnApplyRemoveFilters);
                ctrlPositionsUndo.OnUndoRemoveFilters += new EventHandler(OnUndoRemoveFilters);
                ctrlPositionsRedo.OnRedoRemoveFilters += new EventHandler(OnRedoRemoveFilters);

                tabCtrlMainCA.UseHotTracking = DefaultableBoolean.True;
                tabCtrlUndoCA.UseHotTracking = DefaultableBoolean.True;

                timerClear.Tick += new EventHandler(timerClear_Tick);

                _isFirstTime = false;
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
        /// Sets up access on permissions.
        /// </summary>
        private void SetUpAccessOnPermissions()
        {
            try
            {
                EnableDisableApplyButtons(false);
                EnableDisableRedoButtons(false);
                EnableDisableUndoButtons(false);
                ctrlCAEntryApply.SetGridAsReadOnly();
                ctrlPositionsApply.SetGridAsReadOnly();
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

        private void AfterCellUpdateApply(object sender, EventArgs e)
        {
            TaxlotBaseCollection ds = (TaxlotBaseCollection)ctrlPositionsApply.grdPositions.DataSource;

            if (ds != null && ds.Count > 0)
            {
                ctrlPositionsApply.ClearPositions();
            }
        }

        private void AfterCellUpdateRedo(object sender, EventArgs e)
        {
            TaxlotBaseCollection ds = (TaxlotBaseCollection)ctrlPositionsRedo.grdPositions.DataSource;

            if (ds != null && ds.Count > 0)
            {
                ctrlPositionsRedo.ClearPositions();
            }
        }

        // When Server is connected, Reload the CA ServicesProxy
        void _caServicesProxy_ConnectedEvent(object sender, EventArgs e)
        {
            _caServicesProxy.InnerChannel.Initialize(CommonHelper.REQUESTER_HASHCODE);
        }

        delegate void MainThreadDelegate(object sender, EventArgs e);

        // When Server is disconnected, Clear CA Apply UI data
        private void _caServicesProxy_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MainThreadDelegate del = _caServicesProxy_DisconnectedEvent;
                        this.BeginInvoke(del, sender, e);
                    }
                    else
                    {
                        ClearUI();
                        ClearRedo();
                        ClearUndo();
                        //AllocationManager.GetInstance().ClearData();
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

        void timerClear_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = string.Empty;
            timerClear.Stop();
        }

        public void DisconnectServer()
        {
            try
            {
                //_postTradeServices.MessageReceived -= new MsgReceivedDelegate(ProcessCAResponse);
                //_postTradeServices.DisConnect();
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

        CompanyUser _loginUser = null;
        public CompanyUser LoginUser
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

        #endregion

        /// <summary>
        /// Enables/disable apply buttons.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void EnableDisableApplyButtons(bool isEnabled)
        {
            try
            {
                ultraToolbarsManager3.Tools["btnPreview"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager3.Tools["btnApply"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager3.Tools["btnSaveCorpAction"].SharedPropsInternal.Enabled = isEnabled;
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
        /// Enables/disable undo buttons.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void EnableDisableUndoButtons(bool isEnabled)
        {
            try
            {
                ultraToolbarsManager2.Tools["btnGetCAUndo"].SharedPropsInternal.Enabled = true;
                ultraToolbarsManager2.Tools["btnPreviewUndo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager2.Tools["btnSaveUndo"].SharedPropsInternal.Enabled = isEnabled;
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
        /// Enables/disable redo buttons.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void EnableDisableRedoButtons(bool isEnabled)
        {
            try
            {
                ultraToolbarsManager1.Tools["btnGetCARedo"].SharedPropsInternal.Enabled = true;
                ultraToolbarsManager1.Tools["btnPreviewRedo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager1.Tools["btnUpdateCAs"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager1.Tools["btnSaveRedo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager1.Tools["btnImportRedo"].SharedPropsInternal.Enabled = isEnabled;
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

        void ProcessCAResponse(QueueMessage message)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        PranaQueueMessageHandler queueMessageHandler = new PranaQueueMessageHandler(ProcessCAResponse);
                        this.BeginInvoke(queueMessageHandler, new object[] { message });
                    }
                    else
                    {
                        if (message.HashCode != CommonHelper.REQUESTER_HASHCODE)
                        {
                            return;
                        }

                        switch (message.MsgType)
                        {
                            case CustomFIXConstants.MSG_SaveCAWithSymbolAndCompanyNameChange:
                                {
                                    PranaBinaryFormatter binaryformatter = new PranaBinaryFormatter();
                                    List<CAOnProcessObjects> responseObjects = (List<CAOnProcessObjects>)binaryformatter.DeSerialize(message.Message.ToString());

                                    int count = 0;
                                    //ControlType control = ControlType.Apply;
                                    foreach (CAOnProcessObjects obj in responseObjects)
                                    {
                                        //control = (ControlType)Enum.Parse(typeof(ControlType), obj.ParentControl);
                                        if (obj.IsSaved)
                                        {
                                            count++;
                                        }
                                    }

                                    if (count == responseObjects.Count)
                                    {
                                        ClearUI();
                                        ClearSelectedRedo();

                                        toolStripStatusLabel1.Text = "Corporate Action and Modified Positions saved in the Database!";
                                    }
                                    else if (count != 0 && count < responseObjects.Count)
                                    {
                                        toolStripStatusLabel1.Text = "Corporate Action and Modified Positions saved partially(for some symbols) in the Database. Please check.!";
                                    }
                                    else
                                    {
                                        toolStripStatusLabel1.Text = "Unable to save Corporate Action!";
                                    }
                                    break;
                                }
                            case CustomFIXConstants.MSG_SaveCAForCashDividend:
                            case CustomFIXConstants.MSG_SaveCAForSplits:
                                {
                                    PranaBinaryFormatter binaryformatter = new PranaBinaryFormatter();
                                    CAOnProcessObjects responseObject = (CAOnProcessObjects)binaryformatter.DeSerialize(message.Message.ToString());
                                    bool isSaved = responseObject.IsSaved;
                                    //ControlType control = (ControlType)Enum.Parse(typeof(ControlType), responseObject.ParentControl);
                                    if (isSaved)
                                    {
                                        ClearUI();
                                        ClearSelectedRedo();
                                        toolStripStatusLabel1.Text = "Corporate Action and Modified Positions saved in the Database!";
                                    }
                                    else
                                    {
                                        toolStripStatusLabel1.Text = "Unable to save Corporate Action!";
                                    }
                                    break;
                                }

                            //gets the response after the undo of CA
                            case CustomFIXConstants.MSG_Coprorate_Undo_Response:
                                {
                                    PranaBinaryFormatter binaryformatter = new PranaBinaryFormatter();
                                    CAOnProcessObjects reponseObject = (CAOnProcessObjects)binaryformatter.DeSerialize(message.Message.ToString());

                                    if (reponseObject.IsSaved)
                                    {
                                        toolStripStatusLabel1.Text = "Corporate Action Un-Applied Successfully";
                                        ClearSelectedUndo();
                                    }
                                    else
                                    {
                                        toolStripStatusLabel1.Text = "Corporate Action could not be Un-Applied";
                                    }
                                    break;
                                }

                            //case CustomFIXConstants.MSG_ConvertFixToXmlFormatInDb:
                            //    {
                            //        PranaBinaryFormatter binaryformatter = new PranaBinaryFormatter();
                            //        CAOnProcessObjects responseObject = (CAOnProcessObjects)binaryformatter.DeSerialize(message.Message.ToString());

                            //        if (responseObject.IsSaved)
                            //        {
                            //            //TODO : Corporate action data updated successfully. Need to log this action here
                            //        }
                            //        else
                            //        {
                            //            //TODO : Corporate action data not updated successfully. Need to log this action here
                            //        }

                            //        break;
                            //    }

                            //case CustomFIXConstants.MSG_GetFullCAData:
                            //    {
                            //        PranaBinaryFormatter binaryformatter = new PranaBinaryFormatter();
                            //        CAOnProcessObjects responseObject = (CAOnProcessObjects)binaryformatter.DeSerialize(message.Message.ToString());
                            //        DataSet ds = ((DataSet)responseObject.Message);
                            //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            //        {
                            //            DataTable dbCATable = ds.Tables[0];
                            //            ConvertFixToXmlFormatInDb(dbCATable);
                            //        }
                            //        break;
                            //    }

                            case CustomFIXConstants.MSG_GetAllCAs:
                                {
                                    PranaBinaryFormatter binaryformatter = new PranaBinaryFormatter();
                                    CAOnProcessObjects responseObject = (CAOnProcessObjects)binaryformatter.DeSerialize(message.Message.ToString());
                                    DataTable dbCATable = ((DataSet)responseObject.Message).Tables[0];

                                    if (responseObject.IsApplied)
                                    {
                                        if (dbCATable.Rows.Count > 0)
                                        {
                                            ctrlCAEntryUndo.TransformAndBindTable(dbCATable);
                                        }
                                        else
                                        {
                                            ClearUndo();
                                            toolStripStatusLabel1.Text = "No corporate action found to UnApply for selected date range.";
                                        }
                                    }
                                    else
                                    {
                                        if (dbCATable.Rows.Count > 0)
                                        {
                                            ctrlCAEntryRedo.TransformAndBindTable(dbCATable);
                                        }
                                        else
                                        {
                                            ClearRedo();
                                            toolStripStatusLabel1.Text = "No corporate action found to Apply for selected date range.";
                                        }
                                    }
                                }
                                break;
                            //Gets the saves only CA response from Sec Master
                            case CustomFIXConstants.MSG_SaveCorporateActionWithoutApplying:
                                {
                                    PranaBinaryFormatter binaryformatter = new PranaBinaryFormatter();
                                    CAOnProcessObjects responseObject = (CAOnProcessObjects)binaryformatter.DeSerialize(message.Message.ToString());

                                    if (responseObject.IsSaved)
                                    {
                                        ClearUI();
                                        ClearSelectedRedo();
                                        //this.ctrlUndoSharedControlUndoUnapplied.GetCorporateAction();
                                        MessageBox.Show("Corporate Action Saved in Database", "Corporate Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        //if (responseObject.ParentControl == ControlType.Apply.ToString())
                                        //{
                                        //    ClearUI();
                                        //}
                                        //else
                                        //{
                                        //    ClearSelectedRedo();
                                        //}
                                    }
                                    else
                                    {
                                        toolStripStatusLabel1.Text = "Unable to save Corporate Action! Corporate Action already exists in the Database";
                                    }

                                    break;
                                }
                            //Gets the update only CA response from Sec Master
                            case CustomFIXConstants.MSG_UpdateCorporateActionWithoutApplying:
                                {
                                    PranaBinaryFormatter binaryformatter = new PranaBinaryFormatter();
                                    CAOnProcessObjects responseObject = (CAOnProcessObjects)binaryformatter.DeSerialize(message.Message.ToString());

                                    if (responseObject.IsSaved)
                                    {
                                        //this.ctrlUndoSharedControlUndoUnapplied.GetCorporateAction();
                                        toolStripStatusLabel1.Text = "Corporate Actions are Updated in Database";
                                        ClearUI();
                                        ClearSelectedRedo();
                                        //if (responseObject.ParentControl == ControlType.Apply.ToString())
                                        //{
                                        //    ClearUI();
                                        //}
                                        //else
                                        //{
                                        //    ClearSelectedRedo();
                                        //}
                                    }
                                    else
                                    {
                                        toolStripStatusLabel1.Text = "Unable to save Corporate Action! Corporate Action already exists in the Database";
                                    }

                                    break;
                                }
                            //Gets the response after deletion of CA
                            case CustomFIXConstants.MSG_Coprorate_Delete:
                                {
                                    int recordsAffected = Int16.Parse(message.Message.ToString());
                                    if (recordsAffected > 0)
                                    {
                                        toolStripStatusLabel1.Text = "Corporate Action deleted from the Database";
                                        ClearSelectedRedo();
                                    }
                                    else
                                    {
                                        toolStripStatusLabel1.Text = "Unable to delete Corporate Action!";
                                    }
                                    break;
                                }
                                //case CustomFIXConstants.MSG_SECMASTER_SymbolREQ:
                                //    {
                                //        PranaBinaryFormatter binaryformatter = new PranaBinaryFormatter();
                                //        CAOnProcessObjects reponseObject = (CAOnProcessObjects)binaryformatter.DeSerialize(message.Message.ToString());
                                //        if (tabCtrlMainCA.SelectedTab.Key == "tbApply")
                                //        {
                                //            if (!String.IsNullOrEmpty(reponseObject.Symbol) && reponseObject.IsExist)
                                //            {
                                //                //_caTable.Rows[0][CorporateActionConstants.CONST_NewSymbolTag] = string.Empty;
                                //                MessageBox.Show(reponseObject.Symbol + " is already present in the database. Please confirm if you are putting the right symbol!", "Corporate Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //            }
                                //            if (!String.IsNullOrEmpty(reponseObject.CompanyName) && reponseObject.IsExist)
                                //            {
                                //                //_caTable.Rows[0][CorporateActionConstants.CONST_NewCompanyName] = string.Empty;
                                //                MessageBox.Show(reponseObject.CompanyName + " is already present in the database. Please confirm if you are putting the right symbol!", "Corporate Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //            }

                                //            if (reponseObject.IsExist)
                                //            {
                                //                // DisableApplyButtons();
                                //            }
                                //            else
                                //            {
                                //                this.btnPreview.Enabled = true;
                                //                this.btnSaveCorpAction.Enabled = true;
                                //            }
                                //        }

                                //        break;
                                //    }
                                //case CustomFIXConstants.MSG_GetCompanyDetailsFromSymbol:
                                //    {
                                //        PranaBinaryFormatter binaryformatter = new PranaBinaryFormatter();
                                //        CAOnProcessObjects reponseObject = (CAOnProcessObjects)binaryformatter.DeSerialize(message.Message.ToString());

                                //        ctrlCAEntryApply.AssignCompanyName(reponseObject.CompanyName);

                                //        break;
                                //    }
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

        int _prevValueApply = 0;
        private void cmbCATypeApply_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_caLayoutPreferencesList.CAApplyLayoutPreferencesList.ContainsKey(cmbCATypeApply.Value.ToString()))
                    ctrlPositionsApply.InitControl(ControlType.Apply, _caLayoutPreferencesList.CAApplyLayoutPreferencesList[cmbCATypeApply.Value.ToString()].CAApplyColumns);
                else
                    ctrlPositionsApply.InitControl(ControlType.Apply, new List<ColumnData>());

                if (_prevValueApply != (int)cmbCATypeApply.Value)
                {
                    TaxlotBaseCollection ds = (TaxlotBaseCollection)ctrlPositionsApply.grdPositions.DataSource;

                    if (ds != null && ds.Count > 0 && _prevValueApply != (int)cmbCATypeApply.Value)
                    {
                        DialogResult diares;
                        diares = MessageBox.Show("Changing of CA Type will change the Data, Do you want to continue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (diares == DialogResult.No)
                        {
                            cmbCATypeApply.Value = _prevValueApply;
                            _prevValueApply = (int)cmbCATypeApply.Value;
                            return;
                        }
                    }
                    _prevValueApply = (int)cmbCATypeApply.Value;
                    ctrlPositionsApply.ClearPositions();

                    object selectedCorpAction = cmbCATypeApply.SelectedRow.ListObject;
                    _caTypeApply = (CorporateActionType)((Prana.BusinessObjects.EnumerationValue)selectedCorpAction).Value;
                    if (!_isFirstTime)
                    {
                        ctrlCAEntryApply.AssignColumnPropertiesForCA(_caTypeApply);
                    }
                    if (_caTypeApply.ToString().Equals("-1"))
                    {
                        EnableDisableApplyButtons(false);
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

        int _prevValueUndo = 0;
        void cmbCATypeUndo_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (_caLayoutPreferencesList.CAUndoLayoutPreferencesList.ContainsKey(cmbCATypeUndo.Value.ToString()))
                    ctrlPositionsUndo.InitControl(ControlType.Undo, _caLayoutPreferencesList.CAUndoLayoutPreferencesList[cmbCATypeUndo.Value.ToString()].CAUndoColumns);
                else
                    ctrlPositionsUndo.InitControl(ControlType.Undo, new List<ColumnData>());

                if (_prevValueUndo != (int)cmbCATypeUndo.Value)
                {
                    TaxlotBaseCollection ds = (TaxlotBaseCollection)ctrlPositionsUndo.grdPositions.DataSource;

                    if (ds != null && ds.Count > 0 && _prevValueUndo != (int)cmbCATypeUndo.Value)
                    {
                        DialogResult diares;
                        diares = MessageBox.Show("Changing of CA Type will change the Data, Do you want to continue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (diares == DialogResult.No)
                        {
                            cmbCATypeUndo.Value = _prevValueUndo;
                            _prevValueUndo = (int)cmbCATypeUndo.Value;
                            return;
                        }
                    }
                    _prevValueUndo = (int)cmbCATypeUndo.Value;
                    ctrlPositionsUndo.ClearPositions();

                    object selectedCorpAction = cmbCATypeUndo.SelectedRow.ListObject;
                    _caTypeUndo = (CorporateActionType)((Prana.BusinessObjects.EnumerationValue)selectedCorpAction).Value;

                    switch (_caTypeUndo)
                    {
                        case CorporateActionType.All:
                            EnableDisableUndoButtons(false);
                            ctrlCAEntryUndo.DisableContextMenu();
                            ctrlPositionsUndo.DisableContextMenu();
                            break;
                        default:
                            EnableDisableUndoButtons(true);
                            ctrlCAEntryUndo.EnableContextMenu();
                            ctrlPositionsUndo.EnableContextMenuandUpdateCaption(_caTypeUndo);
                            break;
                    }
                    if (_caTypeUndo.ToString().Equals("-1"))
                    {
                        EnableDisableUndoButtons(false);
                        ultraToolbarsManager2.Tools["btnGetCAUndo"].SharedPropsInternal.Enabled = false;
                    }
                    if (!_isFirstTime)
                    {
                        ctrlCAEntryUndo.FilterForCA(_caTypeUndo);
                        ctrlCAEntryUndo.AssignColumnPropertiesForCA(_caTypeUndo);
                        ctrlCAEntryUndo.ClearSelectedCAList();
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

        int _prevValueRedo = 0;
        void cmbCATypeRedo_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (_caLayoutPreferencesList.CARedoLayoutPreferencesList.ContainsKey(cmbCATypeRedo.Value.ToString()))
                    ctrlPositionsRedo.InitControl(ControlType.Redo, _caLayoutPreferencesList.CARedoLayoutPreferencesList[cmbCATypeRedo.Value.ToString()].CARedoColumns);
                else
                    ctrlPositionsRedo.InitControl(ControlType.Redo, new List<ColumnData>());

                if (_prevValueRedo != (int)cmbCATypeRedo.Value)
                {
                    TaxlotBaseCollection ds = (TaxlotBaseCollection)ctrlPositionsRedo.grdPositions.DataSource;

                    if (ds != null && ds.Count > 0 && _prevValueRedo != (int)cmbCATypeRedo.Value)
                    {
                        DialogResult diares;
                        diares = MessageBox.Show("Changing of CA Type will change the Data, Do you want to continue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (diares == DialogResult.No)
                        {
                            //_blnnoForDate = true;
                            cmbCATypeRedo.Value = _prevValueRedo;
                            _prevValueRedo = (int)cmbCATypeRedo.Value;
                            return;
                        }
                    }
                    _prevValueRedo = (int)cmbCATypeRedo.Value;
                    ctrlPositionsRedo.ClearPositions();
                    object selectedCorpAction = cmbCATypeRedo.SelectedRow.ListObject;
                    _caTypeRedo = (CorporateActionType)((Prana.BusinessObjects.EnumerationValue)selectedCorpAction).Value;

                    switch (_caTypeRedo)
                    {
                        case CorporateActionType.All:
                            EnableDisableRedoButtons(false);
                            ctrlCAEntryRedo.SetAllColumnNonEditble();
                            ctrlCAEntryRedo.DisableContextMenu();
                            ctrlPositionsRedo.EnableContextMenuandUpdateCaption(_caTypeRedo);
                            break;
                        case CorporateActionType.CashDividend:
                            EnableDisableRedoButtons(true);
                            ultraToolbarsManager1.Tools["btnImportRedo"].SharedPropsInternal.Enabled = true;
                            ctrlCAEntryRedo.SetAllColumnsEditble();
                            ctrlCAEntryRedo.EnableContextMenu();
                            ctrlPositionsRedo.EnableContextMenuandUpdateCaption(_caTypeRedo);
                            break;

                        case CorporateActionType.StockDividend:
                            ctrlPositionsRedo.RenameDivField();
                            EnableDisableRedoButtons(true);
                            ultraToolbarsManager1.Tools["btnImportRedo"].SharedPropsInternal.Enabled = false;
                            ctrlCAEntryRedo.SetAllColumnsEditble();
                            ctrlCAEntryRedo.EnableContextMenu();
                            ctrlPositionsRedo.EnableContextMenuandUpdateCaption(_caTypeRedo);
                            break;

                        case CorporateActionType.Split:
                            ctrlPositionsRedo.RenameDivField();
                            EnableDisableRedoButtons(true);
                            ultraToolbarsManager1.Tools["btnImportRedo"].SharedPropsInternal.Enabled = false;
                            ctrlCAEntryRedo.SetAllColumnsEditble();
                            ctrlCAEntryRedo.EnableContextMenu();
                            ctrlPositionsRedo.EnableContextMenuandUpdateCaption(_caTypeRedo);
                            break;

                        default:
                            EnableDisableRedoButtons(true);
                            ultraToolbarsManager1.Tools["btnImportRedo"].SharedPropsInternal.Enabled = false;
                            ctrlCAEntryRedo.SetAllColumnsEditble();
                            ctrlCAEntryRedo.EnableContextMenu();
                            ctrlPositionsRedo.EnableContextMenuandUpdateCaption(_caTypeRedo);
                            break;
                    }
                    if (_caTypeRedo.ToString().Equals("-1"))
                    {
                        EnableDisableRedoButtons(false);
                        ultraToolbarsManager1.Tools["btnGetCARedo"].SharedPropsInternal.Enabled = false;
                    }
                    if (!_isFirstTime)
                    {
                        ctrlCAEntryRedo.FilterForCA(_caTypeRedo);
                        ctrlCAEntryRedo.AssignColumnPropertiesForCA(_caTypeRedo);
                        ctrlCAEntryRedo.ClearSelectedCAList();
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
        /// Handles the ToolClick event of the ultraToolbarsManager1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ToolClickEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void ultraToolbarsManager1_ToolClick(object sender, ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "btnDeleteRedo":
                        btnDeleteRedo_Click();
                        break;

                    case "btnSaveRedo":
                        RedoCA();
                        break;

                    case "btnUpdateCAs":
                        btnUpdateCAs_Click();
                        break;

                    case "btnPreviewRedo":
                        PreviewRedoCorporateActions();
                        break;

                    case "btnGetCARedo":
                        btnGetCARedo_Click();
                        break;

                    case "btnExportRedo":
                        btnExportRedo_Click();
                        break;

                    case "btnImportRedo":
                        btnImportRedo_Click();
                        break;

                    case "btnClearRedo":
                        ClearRedo();
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

        /// <summary>
        /// Handles the ToolClick event of the ultraToolbarsManager2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ToolClickEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void ultraToolbarsManager2_ToolClick(object sender, ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "btnSaveUndo":
                        UndoCA();
                        break;

                    case "btnGetCAUndo":
                        btnGetCAUndo_Click();
                        break;

                    case "btnPreviewUndo":
                        PreviewUndoCorporateActions();
                        break;

                    case "btnExportUndo":
                        btnExportUndo_Click();
                        break;

                    case "btnClearUndo":
                        btnClearUndo_Click();
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

        /// <summary>
        /// Handles the ToolClick event of the ultraToolbarsManager3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ToolClickEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void ultraToolbarsManager3_ToolClick(object sender, ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "btnApply":
                        btnApply_Click();
                        break;

                    case "btnSaveCorpAction":
                        btnSaveCorpAction_Click();
                        break;

                    case "btnPreview":
                        btnPreview_Click();
                        break;

                    case "btnClear":
                        btnClear_Click();
                        break;

                    case "btnSymbolLookUp":
                        btnSymbolLookUp_Click();
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

        /// <summary>
        /// Preview CA Data
        /// </summary>
        private void btnPreview_Click()
        {
            try
            {

                toolStripStatusLabel1.Text = string.Empty;
                _caControlType = ControlType.Apply;
                PreviewApplyCorporateActions();
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
        /// Previews the ca asynchronous.
        /// </summary>
        /// <param name="caType">Type of the ca.</param>
        /// <param name="dt">The dt.</param>
        /// <param name="modifiedTaxlots">The modified taxlots.</param>
        /// <param name="commaSeparatedAccountIds">The comma separated account ids.</param>
        /// <param name="counterPartyId">The counter party identifier.</param>
        void PreviewCAAsync(CorporateActionType caType, DataTable dt, TaxlotBaseCollection modifiedTaxlots, string commaSeparatedAccountIds, int counterPartyId)
        {
            try
            {
                EnableUI(false);
                BackgroundWorker bgPreviewApplyCA = new BackgroundWorker();
                bgPreviewApplyCA.DoWork += bgPreviewCA_DoWork;
                bgPreviewApplyCA.RunWorkerCompleted += bgPreviewCA_RunWorkerCompleted;
                bgPreviewApplyCA.RunWorkerAsync(new object[] { caType, dt, modifiedTaxlots, commaSeparatedAccountIds, counterPartyId });
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
        /// Handles the DoWork event of the bgPreviewCA control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        void bgPreviewCA_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] data = (object[])e.Argument;
                CorporateActionType caType = (CorporateActionType)(data[0]);
                DataTable dt = (DataTable)(data[1]);
                TaxlotBaseCollection modifiedTaxlots = (TaxlotBaseCollection)(data[2]);
                string commaSeparatedAccountIds = (string)(data[3]);
                int counterPartyId = Convert.ToInt32(data[4]);
                CAPreviewResult caPreviewResult = _caServicesProxy.InnerChannel.PreviewCorporateActions(caType, dt, ref modifiedTaxlots, commaSeparatedAccountIds, _caPreference, counterPartyId);
                object[] outputData = new object[2];
                outputData[0] = caPreviewResult;
                outputData[1] = modifiedTaxlots;

                e.Result = outputData;
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
        /// Handles the RunWorkerCompleted event of the bgPreviewCA control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        void bgPreviewCA_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                object[] data = (object[])e.Result;

                //Extra Check applied here to avoid Object reference
                if (data != null)
                {
                    CAPreviewResult caPreviewResult = (CAPreviewResult)data[0];
                    TaxlotBaseCollection modifiedTaxlots = (TaxlotBaseCollection)data[1];

                    if (!string.IsNullOrEmpty(caPreviewResult.ErrorMessage))
                    {
                        toolStripStatusLabel1.Text = caPreviewResult.ErrorMessage;
                        return;
                    }
                    if (!String.IsNullOrEmpty(caPreviewResult.NoPositionSymbols))
                    {
                        toolStripStatusLabel1.Text = "Symbol " + caPreviewResult.NoPositionSymbols + " does not have any open positions. Please check the open positions for this symbol before applying Corporate Action. ";
                        if (_caControlType.Equals(ControlType.Apply))
                            ultraToolbarsManager3.Tools["btnApply"].SharedPropsInternal.Enabled = false;
                        else
                            ultraToolbarsManager1.Tools["btnSaveRedo"].SharedPropsInternal.Enabled = false;
                        return;
                    }

                    if (!String.IsNullOrEmpty(caPreviewResult.ClosingIDs))
                    {
                        toolStripStatusLabel1.Text = "TaxlotIds " + caPreviewResult.ClosingIDs + " closed in future date. Please unwind taxlots before trying to apply the Corporate Action.";
                        return;
                    }

                    if (!String.IsNullOrEmpty(caPreviewResult.CAIDs))
                    {
                        if (caPreviewResult.CAIDs.Length < 40)
                        {
                            toolStripStatusLabel1.Text = "TaxlotIds " + caPreviewResult.CAIDs + "; corporate action is applied in future date. Please undo the corporate action in future date before trying to apply the new Corporate Action.";
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Corporate action is applied in future date. Please undo the corporate action in future date before trying to apply the new Corporate Action.";
                        }
                        Logger.LoggerWrite("TaxlotIds " + caPreviewResult.CAIDs + " closed in future date after applying the Corporate Action. Please unwind taxlots before trying to undo the Corporate Action. ", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                        return;
                    }

                    if (!String.IsNullOrEmpty(caPreviewResult.BoxedPositionTaxlotIds))
                    {
                        toolStripStatusLabel1.Text = "Symbol has Box-Positions. Please close before applying the Corporate Action.";
                        return;
                    }

                    if (modifiedTaxlots != null && modifiedTaxlots.Count > 0)
                    {
                        if (_caControlType.Equals(ControlType.Apply))
                        {
                            ctrlPositionsApply.AssignPositions(modifiedTaxlots);
                            ultraToolbarsManager3.Tools["btnApply"].SharedPropsInternal.Enabled = true;
                        }
                        else
                        {
                            ctrlPositionsRedo.AssignPositions(modifiedTaxlots);
                            ultraToolbarsManager1.Tools["btnSaveRedo"].SharedPropsInternal.Enabled = true;
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
            finally
            {
                EnableUI(true);
            }
        }

        /// <summary>
        /// To toggle between Enable states of UI
        /// </summary>
        /// <param name="enableUI"></param>
        private void EnableUI(bool enableUI)
        {

            try
            {
                _isProcessing = !enableUI;
                switch (_caControlType)
                {
                    case ControlType.Apply:
                        ClearPositions(enableUI, ctrlPositionsRedo, ctrlPositionsUndo);
                        EnableDisablePictureBox(enableUI, this.pctBoxNew, this.ultraExpandableGroupBoxPanel2);
                        this.tabCtrlUndoCA.Enabled = enableUI;
                        EnableDisableApplyUI(enableUI);
                        break;

                    case ControlType.Undo:
                        ClearPositions(enableUI, ctrlPositionsRedo, ctrlPositionsApply);
                        EnableDisablePictureBox(enableUI, this.pctBoxApplied, this.ultraExpandableGroupBoxPanel4);
                        EnableDisableUndoUI(enableUI);
                        this.ultraTabPageControl1.Enabled = enableUI;
                        this.ultraTabPageControl4.Enabled = enableUI;
                        break;

                    case ControlType.Redo:
                        ClearPositions(enableUI, ctrlPositionsUndo, ctrlPositionsApply);
                        EnableDisablePictureBox(enableUI, this.pctBoxUnApplied, this.ultraExpandableGroupBoxPanel5);
                        EnableDisableRedoUI(enableUI);
                        this.ultraTabPageControl1.Enabled = enableUI;
                        this.ultraTabPageControl3.Enabled = enableUI;
                        if (cmbCATypeRedo.Text == "Cash Dividend" && enableUI)
                        {
                            ultraToolbarsManager1.Tools["btnImportRedo"].SharedPropsInternal.Enabled = enableUI;
                        }
                        else
                        {
                            ultraToolbarsManager1.Tools["btnImportRedo"].SharedPropsInternal.Enabled = !enableUI;
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

        /// <summary>
        /// Enables/disable the controls.
        /// </summary>
        /// <param name="enableUI">if set to <c>true</c> [enable UI].</param>
        /// <param name="ctrlPositions1">The control positions1.</param>
        /// <param name="ctrlPositions2">The control positions2.</param>
        private void ClearPositions(bool enableUI, CorporateActionNew.Controls.ctrlPositions ctrlPositions1, CorporateActionNew.Controls.ctrlPositions ctrlPositions2)
        {
            try
            {
                if ((!ctrlPositions1.Enabled || !ctrlPositions2.Enabled) && enableUI && _isPublished)
                {
                    ctrlPositions1.ClearPositions();
                    ctrlPositions2.ClearPositions();
                    _isPublished = false;
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
        /// Enables/disable picture box.
        /// </summary>
        /// <param name="enableUI">if set to <c>true</c> [enable UI].</param>
        /// <param name="pictureBox">The picture box.</param>
        /// <param name="ultraExpandableGroupBoxPanel">The ultra expandable group box panel.</param>
        private void EnableDisablePictureBox(bool enableUI, PictureBox pictureBox, Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel)
        {
            try
            {
                if (!enableUI)
                {
                    pictureBox.Left = (ultraExpandableGroupBoxPanel.Width - pictureBox.Width) / 2;
                    pictureBox.Top = (ultraExpandableGroupBoxPanel.Height - pictureBox.Height) / 2;
                }
                pictureBox.Enabled = !enableUI;
                pictureBox.Visible = !enableUI;
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

        ControlType _caControlType = ControlType.Apply;

        /// <summary>
        /// Save CA into Db
        /// </summary>
        private void btnApply_Click()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                _caControlType = ControlType.Apply;
                bool allLocksareAcquired = true;
                //For CH users only
                // Modifed By : Manvendra Prajapati
                // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3588
                //if (CachedDataManager.GetInstance.GetPranaReleaseViewType().Equals(PranaReleaseViewType.CHMiddleWare))
                //{
                TaxlotBaseCollection positions = GetPositionsAfterCheckForNavLock();
                if (positions != null && positions.Count != 0)
                {
                    allLocksareAcquired = checkForlockedAccounts(_caControlType);
                    if (allLocksareAcquired)
                    {
                        SaveCAAsync(_caTypeApply, positions, ctrlCAEntryApply.CATable);
                        return;
                    }
                }
                else
                    return;
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
        /// Returns positions on the basis of status of NAV Lock on CashAccounts
        /// </summary>
        /// <returns></returns>
        private TaxlotBaseCollection GetPositionsAfterCheckForNavLock()
        {
            TaxlotBaseCollection newPositions = new TaxlotBaseCollection();
            try
            {
                StringBuilder errMssg = new StringBuilder();
                if (ctrlPositionsApply.Positions.Count > 0)
                {
                    foreach (TaxlotBase position in ctrlPositionsApply.Positions)
                    {
                        DateTime dateForNavLock = new DateTime();
                        CorporateActionType type = (CorporateActionType)Enum.Parse(typeof(CorporateActionType), cmbCATypeApply.Value.ToString());
                        switch (type)
                        {
                            case CorporateActionType.CashDividend:
                            case CorporateActionType.StockDividend:
                                {
                                    dateForNavLock = Convert.ToDateTime(position.ExDivDate);
                                    break;
                                }
                            default:
                                {
                                    dateForNavLock = Convert.ToDateTime(dtFromDateRedo.Value);
                                    break;
                                }
                        }

                        bool isApplyAllowed = false;
                        int accountID = CachedDataManager.GetInstance.GetAccountID(position.Account);
                        if (!errMssg.ToString().Contains("NAV is Locked for " + position.Account.ToString() + "before the date " + dateForNavLock.ToString()))
                        {
                            isApplyAllowed = Prana.ClientCommon.NAVLockManager.GetInstance.ValidateTrade(accountID, dateForNavLock);
                        }
                        if (isApplyAllowed)
                        {
                            newPositions.Add(position);
                        }
                        else
                        {
                            if (!errMssg.ToString().Contains("NAV is Locked for " + position.Account.ToString() + "before the date " + dateForNavLock.ToString()))
                                errMssg.Append("\nNAV is Locked for " + position.Account.ToString() + "before the date " + dateForNavLock.ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("There is not a single corporate action to be applied\n Please click Apply button after getting some positions ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }
                if (newPositions.Count == 0)
                {
                    MessageBox.Show("All the records have NAV locked\n Corporate Action can not be applied on them.\n Please consult Admin", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }
                else if (errMssg.Length > 0 && newPositions.Count < ctrlPositionsApply.Positions.Count)
                {
                    if (MessageBox.Show(errMssg.ToString() + "Do you want to continue applying Corporate Action to other records", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        return newPositions;
                    }
                    else
                        return null;
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
            return newPositions;
        }

        /// <summary>
        /// Function checks if user have Account Lock of all accounts and asks for users choice.
        /// If user says yes and all accounts are unlocked. He is granted access to accounts
        /// Added By Faisal Shah
        /// </summary>
        /// <returns></returns>
        private bool checkForlockedAccounts(ControlType caType)
        {
            try
            {
                StringBuilder errMsg = new StringBuilder();
                List<int> newAccountsToBelocked = new List<int>();
                if (caType == ControlType.Apply)
                {
                    foreach (TaxlotBase position in ctrlPositionsApply.Positions)
                    {
                        string accountName = position.Account.ToString();
                        //get account ID from Account Name
                        int accountID = CachedDataManager.GetInstance.GetAccountID(accountName);
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1164
                        //Account Lock error should not be given if account id is not retrieved
                        if (!CachedDataManager.GetInstance.isAccountLocked(accountID) && accountID != int.MinValue && !errMsg.ToString().Contains(accountName))
                        {
                            //add Account Name in Error message if Account is not locked
                            errMsg.Append(", ").Append(accountName);
                            newAccountsToBelocked.Add(accountID);
                        }
                    }
                }
                else if (caType == ControlType.Redo)
                {
                    foreach (TaxlotBase position in ctrlPositionsRedo.Positions)
                    {
                        string accountName = position.Account.ToString();
                        //get account ID from Account Name
                        int accountID = CachedDataManager.GetInstance.GetAccountID(accountName);
                        //Account Lock error should not be given if account id is not retrieved
                        if (!CachedDataManager.GetInstance.isAccountLocked(accountID) && accountID != int.MinValue && !errMsg.ToString().Contains(accountName))
                        {
                            //add Account Name in Error message if Account is not locked
                            errMsg.Append(", ").Append(accountName);
                            newAccountsToBelocked.Add(accountID);
                        }
                    }
                }
                if (errMsg.Length != 0)
                {
                    if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking following accounts " + errMsg.ToString().Substring(1) + ".", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                        if (AccountLockManager.SetAccountsLockStatus(newAccountsToBelocked))
                        {
                            MessageBox.Show("The lock for accounts has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("CashAccounts are currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    else
                    {
                        return false;
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
            return true;
        }

        /// <summary>
        /// validate whether open positions exist to apply corporate action
        /// </summary>
        /// <param name="modifiedTaxlots"></param>
        /// <returns></returns>
        private bool ValidateAtUILevel(TaxlotBaseCollection modifiedTaxlots)
        {
            bool isCAReadytoSave = true;

            try
            {
                if (modifiedTaxlots == null || modifiedTaxlots.Count == 0)
                {
                    toolStripStatusLabel1.Text = "There are no open positions to be Saved.";
                    isCAReadytoSave = false;
                }
                else
                {
                    DialogResult dlgResult = MessageBox.Show("It will save the Corporate Action and Modified Positions Post Corporate Action. \nDo you wish to continue?", "Corporate Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dlgResult == DialogResult.No)
                    {
                        isCAReadytoSave = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                isCAReadytoSave = false;

                if (rethrow)
                {
                    throw;
                }
            }

            return isCAReadytoSave;
        }

        /// <summary>
        /// Save Corporate action on back groud thread
        /// </summary>
        /// <param name="caType"></param>
        /// <param name="modifiedTaxlots"></param>
        /// <param name="dt"></param>
        private void SaveCAAsync(CorporateActionType caType, TaxlotBaseCollection modifiedTaxlots, DataTable dt)
        {
            try
            {
                string caStr = string.Empty;
                bool isValidated = ValidateAtUILevel(modifiedTaxlots);
                if (isValidated)
                {
                    EnableUI(false);
                    toolStripStatusLabel1.Text = "Saving data...";

                    caStr = CommonHelper.GetCorporateActionString(dt, caType);

                    BackgroundWorker bgSaveCA = new BackgroundWorker();
                    bgSaveCA.DoWork += bgSaveCAAsync_DoWork;
                    bgSaveCA.RunWorkerCompleted += bgSaveCAAsync_RunWorkerCompleted;
                    bgSaveCA.RunWorkerAsync(new object[] { caType, modifiedTaxlots, dt, caStr });
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = string.Empty;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void bgSaveCAAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] data = (object[])e.Argument;
                CorporateActionType caType = (CorporateActionType)(data[0]);
                TaxlotBaseCollection modifiedTaxlots = (TaxlotBaseCollection)data[1];
                //DataTable dt = (DataTable)data[2];
                String caStr = data[3].ToString();

                TradeAuditActionType.ActionType actionType = GetAuditActionForCA(caType);
                AddCorpActionDividendAuditEntry(modifiedTaxlots, actionType, _loginUser.CompanyUserID);
                AuditManager.Instance.SaveAuditList(_tradeAuditCollection_CA);
                _tradeAuditCollection_CA.Clear();

                bool isSuccessful = _caServicesProxy.InnerChannel.SaveCorporateAction(caType, caStr, modifiedTaxlots, _loginUser.CompanyUserID);
                if (isSuccessful)
                {
                    e.Result = "Success";
                }
                else
                {
                    e.Result = "Failed";
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = string.Empty;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private TradeAuditActionType.ActionType GetAuditActionForCA(CorporateActionType caType)
        {
            TradeAuditActionType.ActionType action = TradeAuditActionType.ActionType.CashDividend_Applied_CA;
            try
            {
                switch (caType)
                {
                    case CorporateActionType.CashDividend:
                        action = TradeAuditActionType.ActionType.CashDividend_Applied_CA;
                        break;
                    case CorporateActionType.Exchange:
                        action = TradeAuditActionType.ActionType.Exchange_Applied_CA;
                        break;
                    case CorporateActionType.Merger:
                        action = TradeAuditActionType.ActionType.Merger_Applied_CA;
                        break;
                    case CorporateActionType.NameChange:
                        action = TradeAuditActionType.ActionType.NameChange_Applied_CA;
                        break;
                    case CorporateActionType.SpinOff:
                        action = TradeAuditActionType.ActionType.SpinOff_Applied_CA;
                        break;
                    case CorporateActionType.Split:
                        action = TradeAuditActionType.ActionType.Split_Applied_CA;
                        break;
                    case CorporateActionType.StockDividend:
                        action = TradeAuditActionType.ActionType.StockDividend_Applied_CA;
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
            return action;
        }

        private TradeAuditActionType.ActionType GetAuditActionForUndoCA(CorporateActionType caTypeUndo)
        {
            TradeAuditActionType.ActionType action = TradeAuditActionType.ActionType.CashDividend_UnApplied_CA;
            try
            {
                switch (caTypeUndo)
                {
                    case CorporateActionType.CashDividend:
                        action = TradeAuditActionType.ActionType.CashDividend_UnApplied_CA;
                        break;
                    case CorporateActionType.Exchange:
                        action = TradeAuditActionType.ActionType.Exchange_UnApplied_CA;
                        break;
                    case CorporateActionType.Merger:
                        action = TradeAuditActionType.ActionType.Merger_UnApplied_CA;
                        break;
                    case CorporateActionType.NameChange:
                        action = TradeAuditActionType.ActionType.NameChange_UnApplied_CA;
                        break;
                    case CorporateActionType.SpinOff:
                        action = TradeAuditActionType.ActionType.SpinOff_UnApplied_CA;
                        break;
                    case CorporateActionType.Split:
                        action = TradeAuditActionType.ActionType.Split_UnApplied_CA;
                        break;
                    case CorporateActionType.StockDividend:
                        action = TradeAuditActionType.ActionType.StockDividend_UnApplied_CA;
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
            return action;
        }

        void bgSaveCAAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    string result = e.Result as string;

                    if (!string.IsNullOrEmpty(result))
                    {
                        if (result.Equals("Success"))
                        {
                            if (_caControlType == ControlType.Apply)
                            {
                                ClearUI();
                            }
                            else
                            {
                                ClearSelectedRedo();
                            }
                            toolStripStatusLabel1.Text = "Corporate Action and Modified Positions saved in the Database.";
                            timerClear.Start();
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "UnAble to save Corporate Action.";
                        }
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "UnAble to save Corporate Action.";
                }

            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = string.Empty;
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
                EnableUI(true);
            }
        }

        //private void SaveAffectedPositionsPostCA(CorporateActionType caType, TaxlotBaseCollection modifiedTaxlots, DataTable dt, ControlType controlType)
        //{
        //    try
        //    {
        //        if (modifiedTaxlots == null || modifiedTaxlots.Count == 0)
        //        {
        //            toolStripStatusLabel1.Text = "There are no open positions to be Saved!";
        //        }
        //        else
        //        {
        //            DialogResult dlgResult = MessageBox.Show("It will save the Corporate Action and Modified Positions Post Corporate Action. \nDo you wish to continue?", "Corporate Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        //            if (dlgResult == DialogResult.No)
        //            {
        //                return;
        //            }

        //            string caStr = CommonHelper.GetCorporateActionString(dt, caType);

        //            bool isSuccessful = _caServicesProxy.InnerChannel.SaveCorporateAction(caType, caStr, modifiedTaxlots, _loginUser.CompanyUserID);

        //            if (isSuccessful)
        //            {
        //                if (controlType == ControlType.Apply)
        //                {
        //                    ClearUI();
        //                }
        //                else
        //                {
        //                    ClearSelectedRedo();
        //                }

        //                toolStripStatusLabel1.Text = "Corporate Action and Modified Positions saved in the Database!";
        //                timerClear.Start();
        //            }
        //            else
        //            {
        //                toolStripStatusLabel1.Text = "UnAble to save Corporate Action!";
        //            }

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

        /// <summary>
        /// It just saves the CA into Db but not the affected positions.
        /// </summary>
        private void btnSaveCorpAction_Click()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                DataTable dt = ctrlCAEntryApply.CATable;
                _caServicesProxy.InnerChannel.ValidateCAInfo(_caTypeApply, ref dt);
                if (!dt.HasErrors)
                {
                    // If CAType is StockDividend then set the ExDate as Effective Date
                    if ((int)dt.Rows[0][8] == (int)CorporateActionType.StockDividend)
                    {
                        dt.Rows[0][0] = dt.Rows[0][16];
                    }
                    bool isSuccessful = _caServicesProxy.InnerChannel.SaveCAsOnly(dt);

                    if (isSuccessful)
                    {
                        ClearUI();
                        toolStripStatusLabel1.Text = "Corporate Action saved successfully.";
                        timerClear.Start();
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Unable to save Corporate Action. Corporate Action may already exist in the Database.";
                    }

                    ultraToolbarsManager3.Tools["btnApply"].SharedPropsInternal.Enabled = false;
                    ultraToolbarsManager3.Tools["btnSaveCorpAction"].SharedPropsInternal.Enabled = false;
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
        /// BTNs the clear click.
        /// </summary>
        private void btnClear_Click()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                ClearUI();
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
        /// Clears the UI.
        /// </summary>
        private void ClearUI()
        {
            try
            {
                ctrlCAEntryApply.ClearCATable();
                ctrlCAEntryApply.ClearSelectedCAList();
                ctrlPositionsApply.ClearPositions();
                //PostTradeCacheManager.ClearCorporateActionGroups();
                //  _caServicesProxy.InnerChannel.Clear();
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
        /// Clears the undo.
        /// </summary>
        private void ClearUndo()
        {
            try
            {

                ctrlCAEntryUndo.ClearCATable();
                ctrlCAEntryUndo.ClearSelectedCAList();
                ctrlPositionsUndo.ClearPositions();
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
        /// Clears the selected undo.
        /// </summary>
        private void ClearSelectedUndo()
        {
            try
            {
                ctrlCAEntryUndo.ClearSelectedCAs();
                ctrlPositionsUndo.ClearPositions();
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
        /// Clears the redo.
        /// </summary>
        private void ClearRedo()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                ctrlCAEntryRedo.ClearCATable();
                ctrlCAEntryRedo.ClearSelectedCAList();
                ctrlPositionsRedo.ClearPositions();
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
        /// Clears the selected redo.
        /// </summary>
        private void ClearSelectedRedo()
        {
            try
            {
                ctrlCAEntryRedo.ClearSelectedCAs();
                ctrlPositionsRedo.ClearPositions();
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
        /// Handles the Disposed event of the frmCorporateActionNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void frmCorporateActionNew_Disposed(object sender, System.EventArgs e)
        {
            try
            {
                ctrlCAEntryApply.CleanUp();
                ctrlCAEntryUndo.CleanUp();
                ctrlCAEntryRedo.CleanUp();

                //_caServicesProxy.InnerChannel.CAResponseReceived -=new MsgReceivedDelegate(ProcessCAResponse);
                //PostTradeCacheManager.ClearCorporateActionGroups();
                //_caServicesProxy.InnerChannel.Clear();
                _caServicesProxy.Dispose();

                if (FormClosed != null)
                {
                    FormClosed(this, EventArgs.Empty);
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

        void ctrlCorporateActionEntry1_CorporateActionModified(Object sender, EventArgs e)
        {
            try
            {
                ultraToolbarsManager3.Tools["btnPreview"].SharedPropsInternal.Enabled = true;
                ultraToolbarsManager3.Tools["btnApply"].SharedPropsInternal.Enabled = false;
                ultraToolbarsManager3.Tools["btnSaveCorpAction"].SharedPropsInternal.Enabled = true;

                if (ctrlPositionsApply.Positions.Count > 0)
                {
                    toolStripStatusLabel1.Text = "Corporate action information got changed so modified positions are no longer valid. Please click preview to fetch the new modified positions.";
                    ctrlPositionsApply.ClearPositions();
                    _caServicesProxy.InnerChannel.Clear();
                }
                if (ctrlPositionsRedo.Positions.Count > 0)
                {
                    toolStripStatusLabel1.Text = "Corporate action information got changed so modified positions are no longer valid. Please click preview to fetch the new modified positions.";
                    ctrlPositionsRedo.ClearPositions();
                    _caServicesProxy.InnerChannel.Clear();
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

        #region Undo
        private void btnGetCAUndo_Click()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                bool isApplied = true;
                DataSet ds = _caServicesProxy.InnerChannel.GetCAsForDateRange(_caTypeUndo, (DateTime)dtFromDateUndo.Value, (DateTime)dtToDateUndo.Value, isApplied);
                DataTable dbCATable = ds.Tables[0];

                if (dbCATable != null)
                {
                    if (dbCATable.Rows.Count > 0)
                    {
                        ctrlCAEntryUndo.TransformAndBindTable(dbCATable);
                    }
                    else
                    {
                        ClearUndo();
                        toolStripStatusLabel1.Text = "No corporate action found to UnApply for selected date range.";
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

        void OnPreviewUndoClick(object sender, EventArgs e)
        {
            PreviewUndoCorporateActions();
        }

        private void OnUndoCAClick(object sender, EventArgs e)
        {
            try
            {
                UndoCA();
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
        /// Undo CA Data
        /// </summary>
        private void UndoCA()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                if (ctrlPositionsUndo.Positions.Count == 0)
                {
                    toolStripStatusLabel1.Text = "Nothing to undo. Preview undo first.";
                    return;
                }
                DialogResult dlgResult = MessageBox.Show("Corporate Action Undo will revert back the modified positions. \nDo you wish to continue?", "Corporate Action", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dlgResult == DialogResult.No)
                {
                    return;
                }

                toolStripStatusLabel1.Text = "Saving undo data...";
                string caIDs = ctrlCAEntryUndo.GetSelectedCAIDs();
                EnableUI(false);
                BackgroundWorker bgUndoCAAsync = new BackgroundWorker();
                bgUndoCAAsync.DoWork += bgUndoCAAsync_DoWork;
                bgUndoCAAsync.RunWorkerCompleted += bgUndoCAAsync_RunWorkerCompleted;
                bgUndoCAAsync.RunWorkerAsync(new object[] { _caTypeUndo, caIDs, ctrlPositionsUndo.Positions });
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

        private void bgUndoCAAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Added by faisal Shah
                //this bool Variable is required to check if we need to remove the Corporate Action from SM DB and that is only if we delete all taxlots related to CA.
                //In CH if NAV is locked for some accounts we do not allow to undo CA applied on those taxlots.
                bool isSMModificationRequired = true;
                TaxlotBaseCollection taxlotsToUndo = new TaxlotBaseCollection();
                object[] data = (object[])e.Argument;

                CorporateActionType caTypeUndo = (CorporateActionType)(data[0]);
                string caIDs = data[1].ToString();
                // Modifed By : Manvendra Prajapati
                // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3588
                //if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare) 
                //{
                TaxlotBaseCollection totalTaxlots = (TaxlotBaseCollection)data[2];

                bool areAccountsLocked = GetStatusOfAccountLock(totalTaxlots);
                if (areAccountsLocked)
                {
                    if (caTypeUndo == CorporateActionType.CashDividend)
                    {
                        taxlotsToUndo = GetTaxlotsAfterNavLockCheck(totalTaxlots);

                        if (taxlotsToUndo != null && taxlotsToUndo.Count > 0)
                        {

                            if (taxlotsToUndo.Count != totalTaxlots.Count)
                                isSMModificationRequired = false;
                        }
                        else
                            return;
                    }
                    else
                        taxlotsToUndo = totalTaxlots;
                }
                else
                    return;


                TradeAuditActionType.ActionType actionType = GetAuditActionForUndoCA(caTypeUndo);
                AddCorpActionDividendAuditEntry(taxlotsToUndo, actionType, _loginUser.CompanyUserID);
                AuditManager.Instance.SaveAuditList(_tradeAuditCollection_CA);
                _tradeAuditCollection_CA.Clear();

                //Added a Variable to the method to check if we need to remove Corporate action along with the taxlots
                bool isSuccessful = true;
                if (caTypeUndo == CorporateActionType.CashDividend)
                {
                    isSuccessful = _caServicesProxy.InnerChannel.UndoCorporateActions(caTypeUndo, caIDs, taxlotsToUndo, isSMModificationRequired);
                }
                else
                    isSuccessful = _caServicesProxy.InnerChannel.UndoCorporateActions(caTypeUndo, caIDs, taxlotsToUndo);

                if (isSuccessful)
                {
                    e.Result = "Success";
                }
                else
                {
                    e.Result = "Failed";
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = string.Empty;
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
        /// Adds entry to the Audit List for the Cash Dividend applied from Corporate Action
        /// </summary>
        /// <param name="modifiedTaxlot">Not Null, the Data collection from which the data has to be extracted</param>
        /// <param name="action">TradeAuditActionType </param>
        /// <param name="comment">Not Null, comment of the action by the user</param>
        /// <param name="companyUserId">The company user id of the user doing the changes</param>
        /// <returns></returns>
        public bool AddCorpActionDividendAuditEntry(TaxlotBaseCollection modifiedTaxlot, TradeAuditActionType.ActionType action, int currentUserID)
        {
            try
            {
                if (modifiedTaxlot != null)
                {
                    for (int i = 0; i < modifiedTaxlot.Count; i++)
                    {
                        TradeAuditEntry newEntry = new TradeAuditEntry();
                        newEntry.Action = action;
                        newEntry.Comment = action.ToString();
                        newEntry.CompanyUserId = currentUserID;
                        newEntry.Symbol = modifiedTaxlot[i].Symbol;
                        newEntry.Level1ID = modifiedTaxlot[i].Level1ID;
                        newEntry.GroupID = modifiedTaxlot[i].GroupID;
                        newEntry.TaxLotClosingId = modifiedTaxlot[i].ClosingTaxlotID.ToString();
                        newEntry.TaxLotID = modifiedTaxlot[i].L2TaxlotID;
                        newEntry.OrderSideTagValue = modifiedTaxlot[i].OrderSideTagValue;
                        if (action == TradeAuditActionType.ActionType.CashDividend_Applied_CA || action == TradeAuditActionType.ActionType.StockDividend_Applied_CA)
                        {
                            if (modifiedTaxlot[i].ExDivDate != null)
                                newEntry.OriginalDate = Convert.ToDateTime(modifiedTaxlot[i].ExDivDate);

                            newEntry.OriginalValue = modifiedTaxlot[i].Dividend.ToString();
                        }
                        else
                        {
                            newEntry.OriginalValue = "0";
                            if (action == TradeAuditActionType.ActionType.CashDividend_UnApplied_CA || action == TradeAuditActionType.ActionType.StockDividend_UnApplied_CA)
                            {
                                if (modifiedTaxlot[i].ExDivDate != null)
                                    newEntry.OriginalDate = Convert.ToDateTime(modifiedTaxlot[i].ExDivDate);
                            }
                            else
                                newEntry.OriginalDate = modifiedTaxlot[i].AUECLocalDate;
                        }
                        newEntry.AUECLocalDate = DateTime.Now;
                        newEntry.Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.CorporateAction;
                        _tradeAuditCollection_CA.Add(newEntry);
                    }
                }
                else
                    throw new NullReferenceException("The Taxlot Data to add in audit dictionary is null");
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

        private bool GetStatusOfAccountLock(TaxlotBaseCollection taxlotBaseCollection)
        {
            bool areAccountsLocked = true;
            try
            {
                StringBuilder lockedAccounts = new StringBuilder();
                List<int> accountIds = new List<int>();
                foreach (TaxlotBase taxlot in taxlotBaseCollection)
                {
                    int accountId = CachedDataManager.GetInstance.GetAccountID(taxlot.Account);
                    if (!accountIds.Contains(accountId))
                    {
                        accountIds.Add(accountId);
                        if (!CachedDataManager.GetInstance.isAccountLocked(accountId) && accountId != int.MinValue)
                            lockedAccounts.Append(taxlot.Account + ", ");
                    }
                }

                //Account Lock error should not be given if account id is not retrieved
                if (lockedAccounts.Length > 0)
                {
                    if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking " + lockedAccounts.ToString().Substring(0, lockedAccounts.Length - 2) + "?", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (AccountLockManager.SetAccountsLockStatus(accountIds))
                        {
                            MessageBox.Show("The lock for " + lockedAccounts.ToString().Substring(0, lockedAccounts.Length - 2) + " has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Some account(s) is currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //set active cell to null so that it cannot be modified
                            return false;
                        }
                    }
                    else
                    {
                        return false;
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
            return areAccountsLocked;
        }

        private TaxlotBaseCollection GetTaxlotsAfterNavLockCheck(TaxlotBaseCollection taxlotBaseCollection)
        {
            TaxlotBaseCollection taxlotsToUndo = new TaxlotBaseCollection();
            try
            {
                DateTime date = DateTime.MinValue;
                List<string> checkedAccounts = new List<string>();
                StringBuilder navLockedAccounts = new StringBuilder();
                String[] navLockedAccount;
                DataRowCollection rows = ctrlCAEntryUndo.GetSelectedCARows();
                if (rows != null && rows.Count > 0 && rows.Count == 1)
                {
                    foreach (DataRow row in rows)
                    {
                        date = Convert.ToDateTime(row["ExDivDate"].ToString());
                    }
                    foreach (TaxlotBase taxlot in taxlotBaseCollection)
                    {
                        string accountName = taxlot.Account;
                        if (!checkedAccounts.Contains(accountName))
                        {
                            checkedAccounts.Add(accountName);
                            if (!Prana.ClientCommon.NAVLockManager.GetInstance.ValidateTrade(CachedDataManager.GetInstance.GetAccountID(accountName), date))
                                if (!navLockedAccounts.ToString().Contains(accountName))
                                    navLockedAccounts.Append(accountName + ", ");
                        }
                        if (navLockedAccounts.ToString().Contains(accountName))
                            continue;
                        taxlotsToUndo.Add(taxlot);
                    }
                }
                if (navLockedAccounts.Length > 0)
                {
                    navLockedAccount = navLockedAccounts.ToString().Split(',');
                    if (navLockedAccount.Length - 1 == checkedAccounts.Count)
                    {
                        MessageBox.Show("You can not undo Corporate action applied as all accounts have NAV locked.\n Please contact Admin.", "NAV Lock Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                    if (MessageBox.Show("Account(s) " + navLockedAccounts.ToString().Substring(0, navLockedAccounts.Length - 2) + " has/have NAV Locked.\n Do you want to continue for rest of the account(s)", "Nav Lock Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return null;
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
            return taxlotsToUndo;
        }

        void bgUndoCAAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    string result = e.Result as string;

                    if (!string.IsNullOrEmpty(result))
                    {
                        if (result.Equals("Success"))
                        {
                            ClearSelectedUndo();
                            toolStripStatusLabel1.Text = "Corporate Action Un-Applied Successfully";
                            timerClear.Start();
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Corporate Action could not be Un-Applied";
                        }
                    }

                }
                else
                {
                    toolStripStatusLabel1.Text = "Corporate Action could not be Un-Applied";
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = string.Empty;
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
                EnableUI(true);
            }
        }

        private void btnClearUndo_Click()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                ClearUndo();
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

        private void btnExportUndo_Click()
        {
            ExportCorporateActionData(ctrlCAEntryUndo, ctrlPositionsUndo);
        }

        /// <summary>
        /// Exports the corporate action data.
        /// </summary>
        /// <param name="ctrlCAEntry">The control ca entry.</param>
        /// <param name="ctrlPositions">The control positions.</param>
        private async void ExportCorporateActionData(CorporateActionNew.Controls.ctrlCAEntry ctrlCAEntry, CorporateActionNew.Controls.ctrlPositions ctrlPositions)
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Excel File|*.xls";
                saveFileDialog1.Title = "Save Corporate Action Data";
                saveFileDialog1.FileName = "CorpAction_" + DateTime.Now.ToString("MMddyyyyHHmmss");
                DialogResult dlgResult = saveFileDialog1.ShowDialog();

                if (dlgResult == DialogResult.OK)
                {
                    await System.Threading.Tasks.Task.Run(() => ctrlCAEntry.ExportCorporateActions(saveFileDialog1.FileName));
                    await System.Threading.Tasks.Task.Run(() => ctrlPositions.ExportPositions(saveFileDialog1.FileName));
                    MessageBox.Show("Corporate Action exported successfully.", "Corporate Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        #region ReApply

        private void btnGetCARedo_Click()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                bool isApplied = false;
                DataSet ds = _caServicesProxy.InnerChannel.GetCAsForDateRange(_caTypeRedo, (DateTime)dtFromDateRedo.Value, (DateTime)dtToDateRedo.Value, isApplied);
                DataTable dbCATable = ds.Tables[0];
                if (dbCATable != null)
                {
                    if (dbCATable.Rows.Count > 0)
                    {
                        toolStripStatusLabel1.Text = string.Empty;
                        ctrlCAEntryRedo.TransformAndBindTable(dbCATable);
                    }
                    else
                    {
                        ClearRedo();
                        toolStripStatusLabel1.Text = "No corporate action found to Apply for selected date range.";
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

        void OnPreviewRedoClick(object sender, EventArgs e)
        {
            try
            {
                PreviewRedoCorporateActions();
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
        /// Saves corporate action + affected positions in the db.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void OnRedoCAClick(object sender, EventArgs e)
        {
            try
            {
                RedoCA();
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

        private void RedoCA()
        {
            toolStripStatusLabel1.Text = string.Empty;
            if (ctrlPositionsRedo.Positions.Count == 0)
            {
                toolStripStatusLabel1.Text = "Nothing to redo. Preview redo first.";
                return;
            }
            try
            {
                DataRowCollection rows = ctrlCAEntryRedo.GetSelectedCARows();
                _caControlType = ControlType.Redo;
                SaveCAAsync(_caTypeRedo, ctrlPositionsRedo.Positions, rows[0].Table);
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
        /// Just saves the corporate actions in the security master but not the affected positions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportRedo_Click()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                DataRowCollection importedRows = null;
                frmCorporateActionImport importForm = new frmCorporateActionImport();
                importForm.StartPosition = FormStartPosition.CenterParent;
                if (importForm.ShowDialog() == DialogResult.OK)
                {
                    if (importForm.SelectedRows != null && importForm.SelectedRows.Count > 0)
                    {
                        importedRows = importForm.SelectedRows;
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "No rows were selected to import the data! Please import again.";
                    }
                }

                if (importedRows != null)
                {
                    ctrlCAEntryRedo.TransformAndBindTable(importedRows);
                    int count = 0;
                    foreach (DataRow row in ctrlCAEntryRedo.CATable.Rows)
                    {
                        DataTable dTable = row.Table.Clone();
                        dTable.Rows.Add(row.ItemArray);
                        dTable.Rows[count][0] = dTable.Rows[count][16];
                        count++;
                        _caServicesProxy.InnerChannel.SaveCAsOnly(dTable);
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

        private void btnUpdateCAs_Click()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                if (ctrlCAEntryRedo.CATable.Rows.Count > 0)
                {
                    DataTable dt = ctrlCAEntryRedo.CATable;
                    _caServicesProxy.InnerChannel.ValidateCAInfo(_caTypeRedo, ref dt);
                    if (!dt.HasErrors)
                    {
                        bool isSuccessful = _caServicesProxy.InnerChannel.UpdateCAsOnly(dt);

                        if (isSuccessful)
                        {
                            //this.ctrlUndoSharedControlUndoUnapplied.GetCorporateAction();
                            toolStripStatusLabel1.Text = "Corporate Actions are Updated in Database.";
                            ClearSelectedRedo();
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Unable to save Corporate Action! Corporate Action already exists in the Database";
                        }
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Error in updating the Corporate Actions.";
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

        private void btnExportRedo_Click()
        {
            try
            {
                ExportCorporateActionData(ctrlCAEntryRedo, ctrlPositionsRedo);
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

        private void btnDeleteRedo_Click()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                string selectedCaIDs = ctrlCAEntryRedo.GetSelectedCAIDs();
                if (selectedCaIDs != null && selectedCaIDs.Length > 0)
                {
                    DialogResult dlgResult = new DialogResult();
                    dlgResult = MessageBox.Show("Are you sure to delete selected Corporate Action?", "Corporate Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlgResult.Equals(DialogResult.Yes))
                    {
                        bool isSuccessful = _caServicesProxy.InnerChannel.DeleteCAs(selectedCaIDs);
                        if (isSuccessful)
                        {
                            toolStripStatusLabel1.Text = "Corporate Action deleted from the Database.";
                            ClearSelectedRedo();
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Unable to delete Corporate Action.";
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
        #endregion

        private void PreviewApplyCorporateActions()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;

                EnableDisableCAControl(true);
                ctrlPositionsApply.ClearPositions();
                DataTable dt = ctrlCAEntryApply.CATable;

                _caServicesProxy.InnerChannel.ValidateCAInfo(_caTypeApply, ref dt);

                foreach (DataRow dr in ctrlCAEntryApply.CATable.Rows)
                {
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        if (dr["CorpActionId"].Equals(dtRow["CorpActionId"]))
                        {
                            dr["EffectiveDate"] = dtRow["EffectiveDate"];
                        }
                    }
                }


                if (!dt.HasErrors)
                {
                    if (ctrlCAEntryApply.GetSymbolAction().Equals(SecMasterConstants.SecurityActions.ADD))
                    {
                        string newSymbol = dt.Rows[0]["NewSymbol"].ToString();
                        DialogResult dlgResult = MessageBox.Show(newSymbol + " symbol is new to Security Master. \nAdd to Security Master?", "Corporate Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dlgResult == DialogResult.No)
                        {
                            return;
                        }
                    }

                    //PostTradeCacheManager.ClearCorporateActionGroups();
                    _caServicesProxy.InnerChannel.Clear();
                    TaxlotBaseCollection modifiedTaxlots = null;
                    string commaSeparatedAccountIds = multiSelectDropDownNew.GetCommaSeperatedAccountIds();
                    if (!string.IsNullOrEmpty(commaSeparatedAccountIds))
                    {
                        int counterPartyId = Convert.ToInt32(cmbCounterParty.Value);
                        PreviewCAAsync(_caTypeApply, dt, modifiedTaxlots, commaSeparatedAccountIds, counterPartyId);
                    }

                }
                else
                {
                    DataColumn[] columnError = dt.Rows[0].GetColumnsInError();

                    if (columnError.Length > 0)
                    {
                        DataRow row = dt.Rows[0];
                        StringBuilder errorMessage = new StringBuilder();

                        foreach (DataColumn col in columnError)
                        {
                            errorMessage.Append(row.GetColumnError(col));
                            break;
                        }
                        toolStripStatusLabel1.Text = errorMessage.ToString();
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

        private void EnableDisableCAControl(bool isCAEntry)
        {
            try
            {
                if (isCAEntry)
                {
                    ctrlCAEntryApply.Enabled = false;
                    ctrlCAEntryApply.Enabled = true;
                }
                else
                {
                    ctrlCAEntryRedo.Enabled = false;
                    ctrlCAEntryRedo.Enabled = true;
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

        private void PreviewUndoCorporateActions()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                _caControlType = ControlType.Undo;
                ctrlPositionsUndo.ClearPositions();
                // get number of records selected to preview
                DataRowCollection rows = ctrlCAEntryUndo.GetSelectedCARows();
                if (rows != null && rows.Count > 0 && rows.Count == 1)
                {
                    string caIDSStr = ctrlCAEntryUndo.GetSelectedCAIDs();
                    string caSymbols = ctrlCAEntryUndo.getCaSymbols();
                    DataRowCollection caRows = ctrlCAEntryUndo.GetSelectedCARows();
                    Dictionary<string, DateTime> caWiseDates = ctrlCAEntryUndo.GetSelectedCAIDsWithDates();
                    if (String.IsNullOrEmpty(caIDSStr))
                    {
                        toolStripStatusLabel1.Text = "Please select a row to preview the corporate action.";
                        return;
                    }

                    string closedTaxlotIDs = _caServicesProxy.InnerChannel.CheckTaxlotsBeforeUndoPreview(_caTypeUndo, caWiseDates);

                    if (!String.IsNullOrEmpty(closedTaxlotIDs))
                    {
                        if (closedTaxlotIDs.Length < 40)
                        {
                            toolStripStatusLabel1.Text = "TaxlotIds " + closedTaxlotIDs + " closed in future date after applying the Corporate Action. Please unwind taxlots before trying to undo the Corporate Action. ";
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Some taxlots are closed in future date after applying the Corporate Action. Please unwind taxlots before trying to undo the Corporate Action. ";
                        }
                        return;
                    }

                    DataSet topcaIdsWithSymbols = _caServicesProxy.InnerChannel.GetLatestCorpActionForSymbol(caSymbols);

                    DataTable corpActionOrder = topcaIdsWithSymbols.Tables[0];
                    bool inValidOrder = false;

                    if (!corpActionOrder.Rows.Count.Equals(0))
                    {
                        if (_caTypeUndo.Equals(CorporateActionType.NameChange))
                        {
                            foreach (DataRow datarRow in caRows)
                            {
                                foreach (DataRow dr in corpActionOrder.Rows)
                                {
                                    if (datarRow["NewSymbol"].Equals(dr["Symbol"]))
                                    {
                                        if (!datarRow["CorpActionId"].Equals(dr["CorpActionId"]))
                                        {
                                            inValidOrder = true;
                                            if (inValidOrder.Equals(true))
                                            {
                                                string Date = Convert.ToDateTime(datarRow["EffectiveDate"].ToString()).ToString("MM/dd/yyyy");
                                                toolStripStatusLabel1.Text = "" + (CorporateActionType)dr["CorpActionTypeId"] + " has been applied on " + dr["Symbol"] + " on date " + Date + ", CorpActionId = " + dr["CorpActionId"].ToString() + ". Please undo it and try again.";
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow datarRow in caRows)
                            {
                                foreach (DataRow dr in corpActionOrder.Rows)
                                {
                                    if (datarRow["OrigSymbol"].Equals(dr["Symbol"]))
                                    {
                                        if (!datarRow["CorpActionId"].Equals(dr["CorpActionId"]))
                                        {
                                            inValidOrder = true;
                                            if (inValidOrder.Equals(true))
                                            {
                                                string Date = Convert.ToDateTime(datarRow["EffectiveDate"].ToString()).ToString("MM/dd/yyyy");
                                                toolStripStatusLabel1.Text = "" + (CorporateActionType)dr["CorpActionTypeId"] + " has been applied on " + dr["Symbol"] + " on date " + Date + ", CorpActionId = " + dr["CorpActionId"].ToString() + ". Please undo it and try again.";
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    EnableUI(false);

                    BackgroundWorker bgPreviewUncoCA = new BackgroundWorker();
                    bgPreviewUncoCA.DoWork += bgPreviewUndoCA_DoWork;
                    bgPreviewUncoCA.RunWorkerCompleted += bgPreviewUndoCA_RunWorkerCompleted;
                    bgPreviewUncoCA.RunWorkerAsync(new object[] { _caTypeUndo, caIDSStr, caRows[0].Table });
                }
                else
                {
                    toolStripStatusLabel1.Text = "Please select only one row to preview the corporate action.";
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

        private void bgPreviewUndoCA_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] data = (object[])e.Argument;
                CorporateActionType caType = (CorporateActionType)(data[0]);
                string caIDSStr = (string)data[1];
                DataTable dt = (DataTable)(data[2]);

                TaxlotBaseCollection taxlots = _caServicesProxy.InnerChannel.PreviewUndoCorporateActions(caType, caIDSStr, dt);

                object[] outputData = new object[1];
                outputData[0] = taxlots;

                e.Result = outputData;
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
        /// Added By Faisal Shah
        /// Purpose to Filter Taxlots on the basis of CashAccounts permissible for the user in CH mode.
        /// </summary>
        /// <param name="taxlots"></param>
        /// <returns>taxlots</returns>
        private TaxlotBaseCollection GetUserPermissibleTaxlots(TaxlotBaseCollection taxlots)
        {
            try
            {
                List<int> indices = new List<int>();
                Dictionary<int, string> dictUserAccounts = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                foreach (TaxlotBase taxlot in taxlots)
                {
                    if (!dictUserAccounts.ContainsKey(taxlot.Level1ID))
                    {
                        if (!indices.Contains(taxlots.IndexOf(taxlot)))
                            indices.Add(taxlots.IndexOf(taxlot));
                    }
                }
                int changeinIndex = 0;
                foreach (int index in indices)
                {
                    int newIndex = index - changeinIndex;
                    taxlots.RemoveAt(newIndex);
                    changeinIndex++;
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
            return taxlots;
        }

        void bgPreviewUndoCA_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                object[] data = (object[])e.Result;
                //Extra Check applied here to avoid Object reference
                if (data != null)
                {
                    TaxlotBaseCollection taxlots = (TaxlotBaseCollection)data[0];
                    if (taxlots == null || taxlots.Count == 0)
                    {
                        toolStripStatusLabel1.Text = "There are no Open Positions to be previewed.";
                    }
                    else
                    {
                        foreach (TaxlotBase taxlot in taxlots)
                        {
                            CARulesHelper.FillText(taxlot);
                        }
                        ctrlPositionsUndo.AssignPositions(taxlots);
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
            finally
            {
                EnableUI(true);
            }
        }

        /// <summary>
        /// Previews the redo corporate actions.
        /// </summary>
        private void PreviewRedoCorporateActions()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                EnableDisableCAControl(false);
                ctrlPositionsRedo.ClearPositions();
                DataRowCollection rows = ctrlCAEntryRedo.GetSelectedCARows();
                _caControlType = ControlType.Redo;
                if (rows != null && rows.Count > 0 && rows.Count == 1)
                {
                    DataTable dt = rows[0].Table;
                    _caServicesProxy.InnerChannel.ValidateCAInfo(_caTypeRedo, ref dt);

                    if (!dt.HasErrors)
                    {
                        //PostTradeCacheManager.ClearCorporateActionGroups();
                        _caServicesProxy.InnerChannel.Clear();
                        TaxlotBaseCollection modifiedTaxlots = null;

                        string commaSeparatedAccountIds = multiSelectDropDownUnApplied.GetCommaSeperatedAccountIds();
                        int counterPartyId = Convert.ToInt32(cmbCounterPartyUnApplied.Value);
                        PreviewCAAsync(_caTypeRedo, dt, modifiedTaxlots, commaSeparatedAccountIds, counterPartyId);
                    }
                    else
                    {
                        DataColumn[] columnError = dt.Rows[0].GetColumnsInError();

                        if (columnError.Length > 0)
                        {
                            DataRow row = dt.Rows[0];
                            StringBuilder errorMessage = new StringBuilder();

                            foreach (DataColumn col in columnError)
                            {
                                errorMessage.Append(row.GetColumnError(col));
                                break;
                            }
                            toolStripStatusLabel1.Text = errorMessage.ToString();
                        }
                    }
                }
                else if (rows.Count > 1)
                    toolStripStatusLabel1.Text = "Please select only one row to preview the corporate action.";
                else
                {
                    toolStripStatusLabel1.Text = "Please select a row to preview the corporate action.";
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


        void frmCorporateActionNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_isProcessing)
                {
                    MessageBox.Show("Can't close the form when a process is running!", "Warning");
                    e.Cancel = true;
                }
                else
                {
                    if (_proxy != null)
                    {
                        _proxy.InnerChannel.UnSubscribe(Topics.Topic_CreateGroup);
                        _proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing);
                        _proxy.InnerChannel.UnSubscribe(Topics.Topic_SecurityMaster);
                        _proxy.Dispose();
                    }
                }

                // _caServicesProxy.InnerChannel.ResetCache();
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

        private void frmCorporateActionNew_Load(object sender, EventArgs e)
        {
            try
            {
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CORP_ACTION);
                CustomThemeHelper.SetThemeProperties(cmbCATypeApply, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CORP_ACTION);
                CustomThemeHelper.SetThemeProperties(cmbCATypeRedo, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CORP_ACTION);
                CustomThemeHelper.SetThemeProperties(cmbCATypeUndo, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CORP_ACTION);
                CustomThemeHelper.SetThemeProperties(cmbCounterParty, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CORP_ACTION);
                CustomThemeHelper.SetThemeProperties(cmbCounterPartyUnApplied, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CORP_ACTION);
                CustomThemeHelper.SetThemeProperties(ctrlCAEntryApply.GetGridInstance(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSE_TRADE_GRID);
                CustomThemeHelper.SetThemeProperties(ctrlCAEntryUndo.GetGridInstance(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSE_TRADE_GRID);
                CustomThemeHelper.SetThemeProperties(ctrlCAEntryRedo.GetGridInstance(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSE_TRADE_GRID);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                    this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.Font = new Font("Century Gothic", 9F);
                    this.ultraToolbarsManager1.ToolbarSettings.ToolAppearance.ForeColor = Color.Black;
                    this.ultraToolbarsManager2.ToolbarSettings.ToolAppearance.ForeColor = Color.Black;
                    this.ultraToolbarsManager3.ToolbarSettings.ToolAppearance.ForeColor = Color.Black;
                }
                EnableDisableUndoButtons(false);
                EnableDisableRedoButtons(false);
                _caPreference = XMLCacheManager.GetCompanyCAPreferences(LoginUser.CompanyID);
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
        /// Sets the color of the buttons.
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                Infragistics.Win.Appearance appearanceRed = new Infragistics.Win.Appearance();
                appearanceRed.BackColor = System.Drawing.Color.DarkRed;
                appearanceRed.ForeColor = System.Drawing.Color.White;
                SetAppearance(this.ultraToolbarsManager1, "btnDeleteRedo", appearanceRed);
                SetAppearance(this.ultraToolbarsManager2, "btnSaveUndo", appearanceRed);

                Infragistics.Win.Appearance appearanceGreen = new Infragistics.Win.Appearance();
                appearanceGreen.BackColor = System.Drawing.Color.Green;
                appearanceGreen.ForeColor = System.Drawing.Color.White;
                SetAppearance(this.ultraToolbarsManager1, "btnSaveRedo", appearanceGreen);
                SetAppearance(this.ultraToolbarsManager1, "btnUpdateCAs", appearanceGreen);
                SetAppearance(this.ultraToolbarsManager3, "btnApply", appearanceGreen);
                SetAppearance(this.ultraToolbarsManager3, "btnSaveCorpAction", appearanceGreen);

                Infragistics.Win.Appearance appearanceBlack = new Infragistics.Win.Appearance();
                appearanceBlack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(26)))), ((int)(((byte)(51)))));
                appearanceBlack.ForeColor = System.Drawing.Color.White;

                SetAppearance(this.ultraToolbarsManager1, "btnPreviewRedo", appearanceBlack);
                SetAppearance(this.ultraToolbarsManager1, "btnGetCARedo", appearanceBlack);
                SetAppearance(this.ultraToolbarsManager1, "btnExportRedo", appearanceBlack);
                SetAppearance(this.ultraToolbarsManager1, "btnImportRedo", appearanceBlack);
                SetAppearance(this.ultraToolbarsManager1, "btnClearRedo", appearanceBlack);
                SetAppearance(this.ultraToolbarsManager2, "btnGetCAUndo", appearanceBlack);
                SetAppearance(this.ultraToolbarsManager2, "btnPreviewUndo", appearanceBlack);
                SetAppearance(this.ultraToolbarsManager2, "btnExportUndo", appearanceBlack);
                SetAppearance(this.ultraToolbarsManager2, "btnClearUndo", appearanceBlack);
                SetAppearance(this.ultraToolbarsManager3, "btnPreview", appearanceBlack);
                SetAppearance(this.ultraToolbarsManager3, "btnClear", appearanceBlack);
                SetAppearance(this.ultraToolbarsManager3, "btnSymbolLookUp", appearanceBlack);
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
        /// Sets the appearance.
        /// </summary>
        /// <param name="ultraToolbarsManager">The ultra toolbars manager.</param>
        /// <param name="key">The key.</param>
        /// <param name="appearanceRed">The appearance red.</param>
        private void SetAppearance(UltraToolbarsManager ultraToolbarsManager, string key, Infragistics.Win.Appearance appearanceRed)
        {
            try
            {
                ultraToolbarsManager.Tools[key].SharedPropsInternal.AppearancesSmall.Appearance = appearanceRed;
                ultraToolbarsManager.Tools[key].SharedPropsInternal.AppearancesSmall.HotTrackAppearance = appearanceRed;
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
        /// Get full ca data. On reponse we need to check if the data is in FIX format. If yes then we convert it into xml format and save back information
        /// </summary>
        //private void GetFullCAData()
        //{
        //    QueueMessage qMsgGetFullCAData = new QueueMessage(CustomFIXConstants.MSG_GetFullCAData, string.Empty);
        //    SendMessage(qMsgGetFullCAData);
        //}

        ///Fetch the fix based old format corporate action data and convert that into xml based data and save it into XML.
        //TODO : If a database is converted once then this function call is not required. So we can safely remove this whenever all of the client data is converted into xml format. Also remove the backup of T_SMCorporateactions table which is T_SMCorporateactions_Old
        //private void ConvertFixToXmlFormatInDb(DataTable fixBasedTable)
        //{

        //}

        //private void btnScreenshot_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        toolStripStatusLabel1.Text = string.Empty;
        //        SnapShotManager.GetInstance().TakeSnapshot(this);
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

        private void btnSymbolLookUp_Click()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
                string newSymbol = ctrlCAEntryApply.GetNewSymbol();
                SecMasterConstants.SecurityActions symbolAction = ctrlCAEntryApply.GetSymbolAction();

                ListEventAargs args = new ListEventAargs();

                if (!string.IsNullOrEmpty(newSymbol))
                {
                    PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();

                    //creating a args dict for Symbol lookup UI 
                    Dictionary<String, String> argDict = new Dictionary<string, string>();
                    SecMasterUIObj secMasterUI = new SecMasterUIObj();

                    argDict.Add("SearchCriteria", SecMasterConstants.SearchCriteria.Ticker.ToString());
                    secMasterUI.TickerSymbol = newSymbol.Trim();
                    //secMasterUI.BloombergSymbol = string.Empty;
                    //if Suggested action is ADD then create a sec master object else send symbol name to Search/ Approve
                    if (symbolAction == SecMasterConstants.SecurityActions.ADD)
                    {
                        // Added secMaster UI object to args by this we can send sec master values to symbol lookup UI. - omshiv Nov, 2013
                        argDict.Add("SecMaster", binaryFormatter.Serialize(secMasterUI));
                    }
                    else
                    {
                        argDict.Add("Symbol", newSymbol.Trim());
                    }

                    //set action for Add/ Search/ Approve
                    argDict.Add("Action", symbolAction.ToString());

                    args.argsObject = argDict;
                }

                if (LaunchSymbolLookup != null)
                {
                    LaunchSymbolLookup(this, args);
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
        /// Set text to the picture box
        /// </summary>
        private void pctBox_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 11, FontStyle.Bold))
            {
                e.Graphics.DrawString("Please Wait ...", myFont, Brushes.Black, new Point(50, 9));
            }
        }


        CALayoutPreferencesList _caLayoutPreferencesList = null;

        string _caPrefFilePath = string.Empty;
        string _caPrefDirectoryPath = string.Empty;

        /// <summary>
        /// Save Layout for CA Redo Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRedoLayoutSave(object sender, EventArgs e)
        {
            try
            {
                CARedoLayoutPreferences caRedoLayoutPreferences = new CARedoLayoutPreferences();
                caRedoLayoutPreferences.CARedoValue = cmbCATypeRedo.Value.ToString();
                caRedoLayoutPreferences.CARedoColumns = GetGridColumnLayout(ctrlPositionsRedo.grdPositions);

                if (_caLayoutPreferencesList.CARedoLayoutPreferencesList.ContainsKey(cmbCATypeRedo.Value.ToString()))
                {
                    _caLayoutPreferencesList.CARedoLayoutPreferencesList[cmbCATypeRedo.Value.ToString()] = caRedoLayoutPreferences;
                }
                else
                {
                    _caLayoutPreferencesList.CARedoLayoutPreferencesList.Add(cmbCATypeRedo.Value.ToString(), caRedoLayoutPreferences);
                }
                if (cmbCounterPartyUnApplied.Value != null)
                    _caLayoutPreferencesList.CounterPartyUnApplied = (int)cmbCounterPartyUnApplied.Value;
                SaveLayoutPreferences();
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
        /// Save Layout for CA Undo Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnUndoLayoutSave(object sender, EventArgs e)
        {
            try
            {
                CAUndoLayoutPreferences caUndoLayoutPreferences = new CAUndoLayoutPreferences();
                caUndoLayoutPreferences.CAUndoValue = cmbCATypeRedo.Value.ToString();
                caUndoLayoutPreferences.CAUndoColumns = GetGridColumnLayout(ctrlPositionsUndo.grdPositions);

                if (_caLayoutPreferencesList.CAUndoLayoutPreferencesList.ContainsKey(cmbCATypeUndo.Value.ToString()))
                {
                    _caLayoutPreferencesList.CAUndoLayoutPreferencesList[cmbCATypeUndo.Value.ToString()] = caUndoLayoutPreferences;
                }
                else
                {
                    _caLayoutPreferencesList.CAUndoLayoutPreferencesList.Add(cmbCATypeUndo.Value.ToString(), caUndoLayoutPreferences);
                }
                SaveLayoutPreferences();
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
        /// Save Layout for CA Apply Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnApplyLayoutSave(object sender, EventArgs e)
        {
            try
            {
                CAApplyLayoutPreferences caApplyLayoutPreferences = new CAApplyLayoutPreferences();
                caApplyLayoutPreferences.CAApplyValue = cmbCATypeApply.Value.ToString();
                caApplyLayoutPreferences.CAApplyColumns = GetGridColumnLayout(ctrlPositionsApply.grdPositions);

                if (_caLayoutPreferencesList.CAApplyLayoutPreferencesList.ContainsKey(cmbCATypeApply.Value.ToString()))
                {
                    _caLayoutPreferencesList.CAApplyLayoutPreferencesList[cmbCATypeApply.Value.ToString()] = caApplyLayoutPreferences;
                }
                else
                {
                    _caLayoutPreferencesList.CAApplyLayoutPreferencesList.Add(cmbCATypeApply.Value.ToString(), caApplyLayoutPreferences);
                }
                if (cmbCounterParty.Value != null)
                    _caLayoutPreferencesList.CounterPartyApply = (int)cmbCounterParty.Value;
                SaveLayoutPreferences();
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
        /// Reomve filters from CA Apply Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnApplyRemoveFilters(object sender, EventArgs e)
        {
            try
            {
                ctrlPositionsApply.grdPositions.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
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
        /// Reomve filters from CA Undo Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnUndoRemoveFilters(object sender, EventArgs e)
        {
            try
            {
                ctrlPositionsUndo.grdPositions.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
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
        /// Reomve filters from CA Redo Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRedoRemoveFilters(object sender, EventArgs e)
        {
            try
            {
                ctrlPositionsRedo.grdPositions.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
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
        /// Gets Grid column layout of a Grid
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        private List<ColumnData> GetGridColumnLayout(UltraGrid grid)
        {
            List<ColumnData> listGridCols = new List<ColumnData>();
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            try
            {
                foreach (UltraGridColumn gridCol in band.Columns)
                {
                    ColumnData colData = new ColumnData();
                    colData.Key = gridCol.Key;
                    colData.Caption = gridCol.Header.Caption;
                    colData.Hidden = gridCol.Hidden;
                    colData.Format = gridCol.Format;
                    colData.VisiblePosition = gridCol.Header.VisiblePosition;
                    colData.Width = gridCol.Width;
                    colData.ExcludeFromColumnChooser = gridCol.ExcludeFromColumnChooser;
                    colData.Fixed = gridCol.Header.Fixed;
                    colData.CellActivation = gridCol.CellActivation;
                    colData.SortIndicator = gridCol.SortIndicator;

                    //Filter Settings
                    foreach (FilterCondition fCond in band.ColumnFilters[gridCol.Key].FilterConditions)
                    {
                        colData.FilterConditionList.Add(fCond);
                    }
                    colData.FilterLogicalOperator = band.ColumnFilters[gridCol.Key].LogicalOperator;
                    listGridCols.Add(colData);

                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return listGridCols;
        }

        /// <summary>
        ///  Save Layout-Preference into xml
        /// </summary>
        private void SaveLayoutPreferences()
        {
            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(_caPrefFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(CALayoutPreferencesList));
                    serializer.Serialize(writer, _caLayoutPreferencesList);

                    writer.Flush();
                    //writer.Close();
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
        ///  Load Layout-Preference from xml
        /// </summary>
        private void LoadLayoutPreferences()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            _startPath = System.Windows.Forms.Application.StartupPath;
            _caPrefDirectoryPath = _startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + _userID.ToString();
            _caPrefFilePath = _caPrefDirectoryPath + @"\CALayoutPreferences.xml";
            _caLayoutPreferencesList = new CALayoutPreferencesList();
            try
            {
                if (!Directory.Exists(_caPrefDirectoryPath))
                {
                    Directory.CreateDirectory(_caPrefDirectoryPath);
                }

                if (File.Exists(_caPrefFilePath))
                {
                    using (FileStream fs = File.OpenRead(_caPrefFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(CALayoutPreferencesList));
                        _caLayoutPreferencesList = (CALayoutPreferencesList)serializer.Deserialize(fs);
                    }
                }
            }

            #region Catch
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
            #endregion
        }

        #region IPublishing Members

        // Ankit Gupta April 30, 2014:
        // Check the publishing of data from other modules

        bool _isPublished = false;
        public delegate void UIThreadMarshallerPublish(MessageData data, string topic);
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (e != null && topicName != null)
                {
                    UIThreadMarshallerPublish mi = new UIThreadMarshallerPublish(Publish);
                    if (UIValidation.GetInstance().validate(this))
                    {
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(mi, new object[] { e, topicName });
                        }
                        else
                        {
                            if (_isPublished || _isProcessing)
                                return;

                            System.Object[] dataList = (System.Object[])e.EventData;
                            TaxlotBaseCollection taxlotCollectionApply = (TaxlotBaseCollection)ctrlPositionsApply.grdPositions.DataSource;
                            TaxlotBaseCollection taxlotCollectionUndo = (TaxlotBaseCollection)ctrlPositionsUndo.grdPositions.DataSource;
                            TaxlotBaseCollection taxlotCollectionRedo = (TaxlotBaseCollection)ctrlPositionsRedo.grdPositions.DataSource;
                            string tickerSymbol = string.Empty;
                            string topic = string.Empty;
                            if (e.TopicName == Topics.Topic_CreateGroup)
                                topic = "Allocation";
                            else if (e.TopicName == Topics.Topic_SecurityMaster)
                                topic = "Security Master";
                            else if (e.TopicName == Topics.Topic_Closing)
                                topic = "Closing";


                            foreach (System.Object obj in dataList)
                            {
                                // When Invoked from Allocation
                                if (e.TopicName == Topics.Topic_CreateGroup)
                                {
                                    AllocationGroup alloccObj;
                                    alloccObj = (Prana.BusinessObjects.AllocationGroup)(obj);
                                    tickerSymbol = alloccObj.Symbol;
                                    _isPublished = true;
                                }
                                // When Invoked from SecurityMaster
                                else if (e.TopicName == Topics.Topic_SecurityMaster)
                                {
                                    SecMasterBaseObj secObj;
                                    secObj = (Prana.BusinessObjects.SecurityMasterBusinessObjects.SecMasterBaseObj)(obj);
                                    tickerSymbol = secObj.TickerSymbol;
                                    _isPublished = true;
                                }
                                // When Invoked from Closing
                                else if (e.TopicName == Topics.Topic_Closing)
                                {
                                    Prana.BusinessObjects.TaxLot closingObj;
                                    closingObj = (Prana.BusinessObjects.TaxLot)(obj);
                                    tickerSymbol = closingObj.Symbol;
                                    _isPublished = true;
                                }

                                // check if the Symbol exists on the CA Apply UI
                                if (taxlotCollectionApply != null && taxlotCollectionApply.Count > 0)
                                    foreach (TaxlotBase taxlotApply in taxlotCollectionApply)
                                    {
                                        if (taxlotApply.Symbol == tickerSymbol)
                                        {
                                            _isProcessing = true;
                                            EnableDisableApplyUI(false);
                                            ultraToolbarsManager3.Tools["btnPreview"].SharedPropsInternal.Enabled = true;
                                            toolStripStatusLabel1.Text = tickerSymbol + " is modified from " + topic + ". Please click on the preview button to refresh data.";
                                            break;
                                        }
                                    }

                                // check if the Symbol exists on the CA Undo UI
                                if (taxlotCollectionUndo != null && taxlotCollectionUndo.Count > 0)
                                    foreach (TaxlotBase taxlotUndo in taxlotCollectionUndo)
                                    {
                                        if (taxlotUndo.Symbol == tickerSymbol)
                                        {
                                            _isProcessing = true;
                                            EnableDisableUndoUI(false);
                                            ultraToolbarsManager2.Tools["btnPreviewUndo"].SharedPropsInternal.Enabled = true;
                                            toolStripStatusLabel1.Text = tickerSymbol + " is modified from " + topic + ". Please click on the preview button to refresh data.";
                                            break;
                                        }
                                    }

                                // check if the Symbol exists on the CA Redo UI
                                if (taxlotCollectionRedo != null && taxlotCollectionRedo.Count > 0)
                                    foreach (TaxlotBase taxlotRedo in taxlotCollectionRedo)
                                    {
                                        if (taxlotRedo.Symbol == tickerSymbol)
                                        {
                                            _isProcessing = true;
                                            EnableDisableRedoUI(false);
                                            ultraToolbarsManager1.Tools["btnPreviewRedo"].SharedPropsInternal.Enabled = true;
                                            toolStripStatusLabel1.Text = tickerSymbol + " is modified from " + topic + ". Please click on the preview button to refresh data.";
                                            break;
                                        }
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

        public string getReceiverUniqueName()
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Enables/disable redo UI.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void EnableDisableRedoUI(bool isEnabled)
        {
            try
            {
                ctrlCAEntryRedo.Enabled = isEnabled;
                ctrlPositionsRedo.Enabled = isEnabled;
                cmbCATypeRedo.Enabled = isEnabled;
                dtFromDateRedo.Enabled = isEnabled;
                dtToDateRedo.Enabled = isEnabled;
                multiSelectDropDownUnApplied.Enabled = isEnabled;
                cmbCounterPartyUnApplied.Enabled = isEnabled;
                ultraToolbarsManager1.Tools["btnGetCARedo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager1.Tools["btnSaveRedo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager1.Tools["btnClearRedo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager1.Tools["btnImportRedo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager1.Tools["btnPreviewRedo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager1.Tools["btnUpdateCAs"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager1.Tools["btnDeleteRedo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager1.Tools["btnExportRedo"].SharedPropsInternal.Enabled = isEnabled;
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
        /// Enable/disable undo UI.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void EnableDisableUndoUI(bool isEnabled)
        {
            try
            {
                ctrlCAEntryUndo.Enabled = isEnabled;
                ctrlPositionsUndo.Enabled = isEnabled;
                cmbCATypeUndo.Enabled = isEnabled;
                dtFromDateUndo.Enabled = isEnabled;
                dtToDateUndo.Enabled = isEnabled;
                ultraToolbarsManager2.Tools["btnGetCAUndo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager2.Tools["btnPreviewUndo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager2.Tools["btnSaveUndo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager2.Tools["btnClearUndo"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager2.Tools["btnExportUndo"].SharedPropsInternal.Enabled = isEnabled;
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
        /// Enable/disable apply UI.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void EnableDisableApplyUI(bool isEnabled)
        {
            try
            {
                ctrlCAEntryApply.Enabled = isEnabled;
                ctrlPositionsApply.Enabled = isEnabled;
                cmbCATypeApply.Enabled = isEnabled;
                multiSelectDropDownNew.Enabled = isEnabled;
                cmbCounterParty.Enabled = isEnabled;
                ultraToolbarsManager3.Tools["btnPreview"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager3.Tools["btnApply"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager3.Tools["btnSaveCorpAction"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager3.Tools["btnClear"].SharedPropsInternal.Enabled = isEnabled;
                ultraToolbarsManager3.Tools["btnSymbolLookUp"].SharedPropsInternal.Enabled = isEnabled;
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
        /// Pooja Porwal on October 1, 2014:
        /// CA Layout Preferences
        /// </summary>
        [XmlRoot("CALayoutPreferences")]
        [Serializable]
        public class CALayoutPreferences
        {
            [XmlArray("CAApplyColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
            public List<ColumnData> CAApplyColumns = new List<ColumnData>();

            [XmlArray("CARedoColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
            public List<ColumnData> CARedoColumns = new List<ColumnData>();

            [XmlArray("CAUndoColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
            public List<ColumnData> CAUndoColumns = new List<ColumnData>();
        }

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}