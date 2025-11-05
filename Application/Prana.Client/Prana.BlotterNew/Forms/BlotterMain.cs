using ExportGridsData;
using Infragistics.Win.UltraWinGrid;
using Prana.Blotter.BusinessObjects;
using Prana.Blotter.Classes;
using Prana.Blotter.Forms;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Blotter;
using Prana.BusinessObjects.Constants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.TradeManager;
using Prana.TradeManager.Extension;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Blotter
{
    public partial class BlotterMain : Form, IBlotter, IPublishing, IExportGridData
    {
        private CompanyUser _loginUser;
        public Prana.BusinessObjects.CompanyUser LoginUser
        {
            get { return _loginUser; }
            set { _loginUser = value; }
        }

        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
                this.ctrlBlotterMain.SecurityMaster = value;
            }
        }

        private DuplexProxyBase<ISubscription> _proxy;
        BlotterPreferenceData _blotterPreferenceData = null;
        private System.Windows.Forms.SaveFileDialog exportToExcelFileDialog;
        public virtual event EventHandler<EventArgs<string, DateTime, DateTime>> GoToAllocationClicked = null;
        private object _locker = new object();
        private bool _isBlotterStageImportTriggered = false;

        /// <summary>
        /// Occurs when [highlight symbol from blotter].
        /// </summary>
        public event EventHandler<EventArgs<string>> HighlightSymbolFromBlotter = null;

        /// <summary>
        /// To wire Security Master UI Launch Form event
        /// </summary>
        public event EventHandler LaunchSecurityMasterForm;

        #region Initialize Class
        public BlotterMain()
        {
            try
            {
                InitializeComponent();
                MakeProxy();
                SetStatusBarText("Loading Data...");
                Prana.TradeManager.TradeManager.GetInstance().GetBlotterDataFromDB();
                TradeManager.TradeManager.GetInstance().ResetTimers(true);
                this.ctrlBlotterMain.ChangeLinkUnlikBtnCaption += ctrlBlotterMain_ChangeLinkUnlikBtnCaption;
                this.ctrlBlotterMain.HighlightSymbolSend += ctrlBlotterMain_HighlightSymbolSend;
                this.ctrlBlotterMain.SetBeginImportText += ctrlBlotterMain_SetBeginStatusForImport;
                BlotterOrderCollections.GetInstance().SendBlotterTradesEventHandler += SendBlotterTradesEvent;
                InstanceManager.RegisterInstance(this);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the SetBeginImportText event of the ctrlBlotterMain control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlBlotterMain_SetBeginStatusForImport(object sender, EventArgs e)
        {
            try
            {
                _isBlotterStageImportTriggered = true;
                SetStatusBarText("Upload Process is in Progress");
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
        /// Event for SendBlotterTradesEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendBlotterTradesEvent(object sender, EventArgs<OrderSingle> e)
        {
            try
            {
                Prana.TradeManager.TradeManager.GetInstance().SendBlotterTrades(e.Value);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the HighlightSymbolSend event of the ctrlBlotterMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}"/> instance containing the event data.</param>
        private void ctrlBlotterMain_HighlightSymbolSend(object sender, EventArgs<string> e)
        {
            try
            {
                if (HighlightSymbolFromBlotter != null)
                {
                    HighlightSymbolFromBlotter(null, e);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the ChangeLinkUnlikBtnCaption event of the ctrlBlotterMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}"/> instance containing the event data.</param>
        private void ctrlBlotterMain_ChangeLinkUnlikBtnCaption(object sender, EventArgs<string> e)
        {
            try
            {
                this.buttonTool18.SharedPropsInternal.Caption = e.Value;
                this.buttonTool18.SharedPropsInternal.ToolTipText = e.Value;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void MakeProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _proxy.Subscribe(Topics.Topic_CreateGroup, null);
                _proxy.Subscribe(Topics.Topic_StageOrderRemovalFromBlotter, null);
                _proxy.Subscribe(Topics.Topic_SubOrderRemovalFromBlotter, null);
                _proxy.Subscribe(Topics.Topic_RefreshBlotterAfterImport, null);
                _proxy.Subscribe(Topics.Topic_UpdateBlotterStatusBarMessage, null);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        //List of ClorderId with Allocation Details
        List<AllocationDetails> allocationDetails = new List<AllocationDetails>();
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { Publish(e, topicName); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        lock (_locker)
                        {
                            object[] dataList = (object[])e.EventData;
                            switch (e.TopicName)
                            {
                                case Topics.Topic_CreateGroup:
                                    //Response Group List
                                    List<AllocationGroup> responseGroups = dataList.Where(x => x is AllocationGroup).Select(y => (y as AllocationGroup)).ToList();

                                    //Update Order Allocation State Dictionary
                                    responseGroups.FindAll(x => x.PersistenceStatus != ApplicationConstants.PersistenceStatus.UnGrouped).ForEach(group =>
                                    {
                                        group.Orders.ForEach(order =>
                                        {
                                            AllocationDetails allocationDetail = new AllocationDetails();
                                            allocationDetail.ClOrderID = order.ParentClOrderID;
                                            allocationDetail.AllocationStatus = group.State;
                                            allocationDetail.Level1Allocation = group.Allocations.Collection;
                                            allocationDetail.AllocationSchemeName = group.AllocationSchemeName;

                                            allocationDetail.TradeAttribute1 = group.TradeAttribute1;
                                            allocationDetail.TradeAttribute2 = group.TradeAttribute2;
                                            allocationDetail.TradeAttribute3 = group.TradeAttribute3;
                                            allocationDetail.TradeAttribute4 = group.TradeAttribute4;
                                            allocationDetail.TradeAttribute5 = group.TradeAttribute5;
                                            //Can be a better way to set TradeAttribute6 but currently adding property in AllocationDetails class, TODO pass whole AllocationGroup to update
                                            allocationDetail.TradeAttribute6 = group.TradeAttribute6;
                                            allocationDetail.SetTradeAttribute(group.GetTradeAttributesAsDict());

                                            if (!allocationDetails.Contains(allocationDetail))
                                                allocationDetails.Add(allocationDetail);
                                        });
                                    });

                                    //Update Grid Rows
                                    ctrlBlotterMain.UpdateAllocationDetails(allocationDetails);
                                    break;

                                case Topics.Topic_StageOrderRemovalFromBlotter:
                                    if (dataList != null && dataList.Length > 0)
                                    {
                                        StageOrderRemovalData stageOrderRemovalData = (StageOrderRemovalData)dataList[0];

                                        //Remove from Grid
                                        Prana.TradeManager.TradeManager.GetInstance().HideOrderFromBlotterGrid(stageOrderRemovalData.ParentClOrderIds);

                                        if (!stageOrderRemovalData.IsComingFromRollOver)
                                        {
                                            TradingAccountCollection taCollection = CachedDataManager.GetInstance.GetUserTradingAccounts();
                                            bool isTradingAccountPermittedOrderRemoved = false;
                                            foreach (TradingAccount ta in taCollection)
                                            {
                                                if (stageOrderRemovalData.UniqueTradingAccounts.Contains(ta.TradingAccountID))
                                                {
                                                    isTradingAccountPermittedOrderRemoved = true;
                                                    break;
                                                }
                                            }

                                            if (isTradingAccountPermittedOrderRemoved)
                                            {
                                                if (ctrlBlotterMain.GetActiveRowOfOrderBlotter() == null)
                                                    ctrlBlotterMain.ClearSubOrderBlotterGrid();

                                                if (stageOrderRemovalData.CompanyUserID != _loginUser.CompanyUserID)
                                                    SetStatusBarText("Order(s) has been removed by other user.");
                                            }
                                        }
                                    }
                                    break;

                                case Topics.Topic_SubOrderRemovalFromBlotter:
                                    if (dataList != null && dataList.Length > 0)
                                    {
                                        SubOrderRemovalData stageOrderRemovalData = (SubOrderRemovalData)dataList[0];

                                        //Remove from Grid
                                        Prana.TradeManager.TradeManager.GetInstance().HideOrderFromBlotterGrid(stageOrderRemovalData.SubOrdersClOrderIds);

                                        TradingAccountCollection taCollection = CachedDataManager.GetInstance.GetUserTradingAccounts();
                                        bool isTradingAccountPermittedOrderRemoved = false;
                                        foreach (TradingAccount ta in taCollection)
                                        {
                                            if (stageOrderRemovalData.UniqueTradingAccounts.Contains(ta.TradingAccountID))
                                            {
                                                isTradingAccountPermittedOrderRemoved = true;
                                                break;
                                            }
                                        }
                                        if (isTradingAccountPermittedOrderRemoved)
                                        {
                                            ctrlBlotterMain.ClearManualSubOrderBlotterGrid(stageOrderRemovalData.SubOrdersClOrderIds);
                                            if (stageOrderRemovalData.CompanyUserID == _loginUser.CompanyUserID)
                                                SetStatusBarText("Order(s) has been removed.");
                                            else if (stageOrderRemovalData.CompanyUserID != _loginUser.CompanyUserID)
                                                SetStatusBarText("Order(s) has been removed by other user.");
                                        }
                                    }
                                    break;

                                case Topics.Topic_RefreshBlotterAfterImport:
                                    if (TradeManager.TradeManager.GetInstance().IsBlotterRefreshRequired)
                                    {
                                        TradeManager.TradeManager.GetInstance().GetBlotterDataFromDB();
                                        TradeManager.TradeManager.GetInstance().IsBlotterRefreshRequired = false;
                                    }
                                    else if (_isBlotterStageImportTriggered)
                                    {
                                        SetStatusBarText("Upload Process has been successfully completed");
                                        _isBlotterStageImportTriggered = false;
                                    }
                                    break;

                                case Topics.Topic_UpdateBlotterStatusBarMessage:
                                    if (dataList != null && dataList.Length > 0)
                                    {
                                        SetStatusBarText(dataList[0].ToString());
                                    }
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

        public BlotterPreferenceData GetUserBlotterPrefrence()
        {
            BlotterPreferenceData blotterPreferenceData = new BlotterPreferenceData();
            try
            {
                blotterPreferenceData = (BlotterPreferenceData)BlotterPreferenceManager.GetInstance().GetPreferencesBinary();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return blotterPreferenceData;
        }

        public void InitControl()
        {
            try
            {
                BlotterPreferenceManager.GetInstance().SetUser(_loginUser);
                _blotterPreferenceData = GetUserBlotterPrefrence();
                BlotterCacheManager.GetInstance().GetTradingAccountUsersByUserID(_loginUser.CompanyUserID);
                ctrlBlotterMain.LaunchSecurityMasterForm += LaunchSecurityMasterForm;
                ctrlBlotterMain.InitContol(_loginUser, _blotterPreferenceData);
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER_OVERRIDE);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
                ctrlBlotterMain.SetLinkUnlinkStartup();

            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
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
        }

        public void WireEvents()
        {
            try
            {
                Prana.TradeManager.TradeManager.GetInstance().UpdateBlotterCollectionOnBlotterThreadMarshal += new UpdateBlotterCollectionOnBlotterThreadHandler(BlotterMain_UpdateWorkingSubOrderCollection);
                Prana.TradeManager.TradeManager.GetInstance().ClearWorkingSubOrderCollection += new EventHandler(BlotterMain_ClearWorkingSubOrderCollection);
                Prana.TradeManager.TradeManager.GetInstance().RefreshBlotterEvent += new EventHandler(BlotterMain_RefreshBlotterEvent);
                Prana.TradeManager.TradeManager.GetInstance().UpdateDictionaryByDbOrders += new UpdateDictionaryByDbOrdershandler(BlotterMain_UpdateDictionaryByDbOrders);
                Prana.TradeManager.TradeManager.GetInstance().BlotterRefreshCompleteEvent += new EventHandler(BlotterMain_BlotterRefreshCompleteEvent);
                BlotterOrderCollections.GetInstance().UpdateOnRolloverComplete += BlotterMain_UpdateOnRolloverComplete;
                if (ctrlBlotterMain != null)
                {
                    ctrlBlotterMain.EnableDisableButton += new EventHandler<EventArgs<bool>>(ctrlBlotterMain_EnableDisableButton);
                    ctrlBlotterMain.EnableDisableMergeAndUploadButton += new EventHandler<EventArgs<bool>>(ctrlBlotterMain_EnableDisableMergeAndUploadButton);
                    ctrlBlotterMain.GoToAllocationClicked += ctrlBlotterMain_GoToAllocationClicked;
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

        private void UnWireEvents()
        {
            try
            {
                Prana.TradeManager.TradeManager.GetInstance().UpdateBlotterCollectionOnBlotterThreadMarshal -= new UpdateBlotterCollectionOnBlotterThreadHandler(BlotterMain_UpdateWorkingSubOrderCollection);
                Prana.TradeManager.TradeManager.GetInstance().ClearWorkingSubOrderCollection -= new EventHandler(BlotterMain_ClearWorkingSubOrderCollection);
                Prana.TradeManager.TradeManager.GetInstance().RefreshBlotterEvent -= new EventHandler(BlotterMain_RefreshBlotterEvent);
                Prana.TradeManager.TradeManager.GetInstance().UpdateDictionaryByDbOrders -= new UpdateDictionaryByDbOrdershandler(BlotterMain_UpdateDictionaryByDbOrders);
                Prana.TradeManager.TradeManager.GetInstance().BlotterRefreshCompleteEvent -= new EventHandler(BlotterMain_BlotterRefreshCompleteEvent);
                BlotterOrderCollections.GetInstance().UpdateOnRolloverComplete -= BlotterMain_UpdateOnRolloverComplete;
                if (ctrlBlotterMain != null)
                {
                    ctrlBlotterMain.ChangeLinkUnlikBtnCaption -= ctrlBlotterMain_ChangeLinkUnlikBtnCaption;
                    ctrlBlotterMain.HighlightSymbolSend -= ctrlBlotterMain_HighlightSymbolSend;
                    ctrlBlotterMain.GoToAllocationClicked -= ctrlBlotterMain_GoToAllocationClicked;
                    ctrlBlotterMain.EnableDisableMergeAndUploadButton -= new EventHandler<EventArgs<bool>>(ctrlBlotterMain_EnableDisableMergeAndUploadButton);
                    ctrlBlotterMain.EnableDisableButton -= new EventHandler<EventArgs<bool>>(ctrlBlotterMain_EnableDisableButton);
                    ctrlBlotterMain.UnwireEvents();
                }
                UnSubscribeProxy();
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
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
        }

        void BlotterMain_FormClosing(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (this.BlotterClosed != null)
                {
                    InstanceManager.ReleaseInstance(typeof(BlotterMain));
                    TradeManager.TradeManager.GetInstance().ResetTimers(false);
                    this.BlotterClosed(sender, e);
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

        void BlotterMain_UpdateDictionaryByDbOrders(object sender, EventArgs<OrderBindingList> e)
        {
            try
            {
                BlotterOrderCollections.GetInstance().UpdateDictionarybyDBOrders(e.Value);
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
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
        }

        /// <summary>
        /// Highlights the symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public void HighlightSymbol(string symbol)
        {
            try
            {
                ctrlBlotterMain.HighlightSymbol(symbol);
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
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
        }


        /// <summary>
        /// Highlights the symbol from QTT.
        /// </summary>
        /// <param name="linkingData">The linking data.</param>
        public void HighlightSymbolFromQTT(QTTBlotterLinkingData linkingData)
        {
            try
            {
                ctrlBlotterMain.HighlightSymbolFromQTT(linkingData);
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
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
        }

        public void DeHighlightSymbolFromQTT(QTTBlotterLinkingData linkingData)
        {
            try
            {
                ctrlBlotterMain.DeHighlightSymbolFromQTT(linkingData);
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
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
        }

        void BlotterMain_RefreshBlotterEvent(Object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { BlotterMain_RefreshBlotterEvent(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        ctrlBlotterMain.ClearSubOrderBlotterGrid();
                        Prana.TradeManager.TradeManager.GetInstance().GetBlotterDataOnBlotterThread();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
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
        }

        void BlotterMain_BlotterRefreshCompleteEvent(Object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { BlotterMain_BlotterRefreshCompleteEvent(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        //Update Blotter status bar Message
                        string statusBarMessage = (TradeManager.TradeManager.GetInstance().WorkingSubBlotterCollection.Count > 0 || TradeManager.TradeManager.GetInstance().OrderBlotterCollection.Count > 0) ? "Data loaded" : "Nothing to Load";
                        SetStatusBarText(statusBarMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
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
        }
        void BlotterMain_ClearWorkingSubOrderCollection(Object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { BlotterMain_ClearWorkingSubOrderCollection(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        BlotterOrderCollections.GetInstance().ClearAllCollections();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                if (!this.IsDisposed)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
            }
        }

        void BlotterMain_UpdateWorkingSubOrderCollection(object sender, EventArgs<List<OrderSingle>> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        UpdateBlotterCollectionOnBlotterThreadHandler mi = new UpdateBlotterCollectionOnBlotterThreadHandler(BlotterMain_UpdateWorkingSubOrderCollection);
                        if (UIValidation.GetInstance().validate(this))
                            this.BeginInvoke(mi, sender, e);
                    }
                    // Applying double check as updates are too frequent and we get "Can not access disposed object Error"
                    else if (UIValidation.GetInstance().validate(this))
                    {
                        lock (_locker)
                        {
                            BlotterOrderCollections.GetInstance().UpdateBlotterCollection(e.Value);
                            ctrlBlotterMain.UpdateSubOrderBlotterGrid(allocationDetails, e.Value);
                            if (e.Value != null && e.Value.Count > 0 && e.Value[0].Description.Equals(BlotterConstants.CAPTION_MERGE_ORDERS))
                            {
                                ctrlBlotterMain.SaveAuditTrailMergedOrder(e.Value[0].ClOrderID);
                            }
                            MultiBrokerSubOrders.GetInstance().UpdateGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (UIValidation.GetInstance().validate(this))
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
        }
        #endregion

        #region IBlotter Members
        public event EventHandler BlotterClosed;
        public event EventHandler LaunchPreferences;
        public event EventHandler TradeClick;
        public event EventHandler ReplaceOrEditOrderClicked;

        public Form Reference()
        {
            return this;
        }
        #endregion

        #region Blotter Tab Events
        private void ctrlBlotterMain_EnableDisableMergeAndUploadButton(object sender, EventArgs<bool> e)
        {
            try
            {
                ultraToolbarsManager1.Tools[BlotterConstants.PROPERTY_MERGE_ORDERS].SharedProps.Enabled = e.Value;
                ultraToolbarsManager1.Tools[BlotterConstants.PROPERTY_UPLOAD_STAGE_ORDERS].SharedProps.Enabled = e.Value;
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
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
        }

        private void ctrlBlotterMain_EnableDisableButton(object sender, EventArgs<bool> e)
        {
            try
            {
                ultraToolbarsManager1.Tools[BlotterConstants.PROPERTY_BTN_REMOVE_ORDERS].SharedProps.Enabled = e.Value;
                ultraToolbarsManager1.Tools[BlotterConstants.PROPERTY_BTN_CANCEL_ALL_SUBS].SharedProps.Enabled = e.Value;

                if (e.Value && _loginUser.CompanyUserID == TradeManagerExtension.GetInstance().BlotterClearanceCommonData.RolloverPermittedUserID)
                {
                    ultraToolbarsManager1.Tools[BlotterConstants.PROPERTY_BTN_ROLLOVER_ALL_SUBS].SharedProps.Enabled = true;
                }
                else
                {
                    ultraToolbarsManager1.Tools[BlotterConstants.PROPERTY_BTN_ROLLOVER_ALL_SUBS].SharedProps.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
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
        }

        private void ctrlBlotterMain_TradeClick(object sender, EventArgs e)
        {
            try
            {
                if (TradeClick != null)
                {
                    LaunchFormEventArgs eventArgs = null;
                    if (sender != null)
                    {
                        eventArgs = new LaunchFormEventArgs(sender);
                    }
                    else
                    {
                        eventArgs = new LaunchFormEventArgs(new OrderSingle());
                    }
                    if (eventArgs != null)
                    {
                        TradeClick(this, eventArgs);
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

        private void ctrlBlotterMain_ReplaceOrEditOrderClicked(object sender, System.EventArgs e)
        {
            try
            {
                if (ReplaceOrEditOrderClicked != null)
                {
                    LaunchFormEventArgs ea = new LaunchFormEventArgs(sender);
                    ReplaceOrEditOrderClicked(this, ea);
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

        private void ctrlBlotterMain_LaunchAuditTrail(object sender, EventArgs e)
        {
            try
            {
                AuditTrail auditTrailForm = new AuditTrail(((OrderSingle)sender).ParentClOrderID);
                auditTrailForm.Owner = this;
                auditTrailForm.StartPosition = FormStartPosition.Manual;
                auditTrailForm.Location = new Point(this.Location.X + 30, this.Location.Y + 30);
                auditTrailForm.ShowInTaskbar = false;
                auditTrailForm.Show();
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

        private void ctrlBlotterMain_GoToAllocationClicked(object sender, EventArgs<string, DateTime, DateTime> e)
        {
            try
            {
                if (GoToAllocationClicked != null)
                    GoToAllocationClicked(this, new EventArgs<string, DateTime, DateTime>(e.Value, e.Value2, e.Value3));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private void ctrlBlotterMain_LaunchAddFills(object sender, EventArgs e)
        {
            try
            {
                Prana.BusinessObjects.OrderSingle or = (Prana.BusinessObjects.OrderSingle)sender;
                Prana.Blotter.AddFills frmAddFills = new Prana.Blotter.AddFills(or);
                frmAddFills.StartPosition = FormStartPosition.Manual;
                frmAddFills.Location = new Point(this.Location.X + 30, this.Location.Y + 30);
                frmAddFills.SaveManualFills += new EventHandler(frmAddFills_SaveManualFills);
                frmAddFills.ShowDialog();
                frmAddFills.SaveManualFills -= new EventHandler(frmAddFills_SaveManualFills);
                frmAddFills.Dispose();
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

        private void frmAddFills_SaveManualFills(object sender, EventArgs e)
        {
            try
            {
                Prana.BusinessObjects.OrderSingle or = (Prana.BusinessObjects.OrderSingle)sender;
                TradeManager.TradeManager.GetInstance().SendBlotterTrades(or);
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

        #region Toolbar Events
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case BlotterConstants.PROPERTY_BTN_REFRESH:
                        SetStatusBarText("Loading Data...");
                        Prana.TradeManager.TradeManager.GetInstance().GetBlotterDataFromDB();
                        break;
                    case BlotterConstants.PROPERTY_BTN_REMOVE_ORDERS:
                        ctrlBlotterMain.RemoveOrders();
                        break;
                    case BlotterConstants.PROPERTY_BTN_CANCEL_ALL_SUBS:
                        ctrlBlotterMain.CancelAllSubs();
                        break;
                    case BlotterConstants.PROPERTY_MERGE_ORDERS:
                        ctrlBlotterMain.MergeOrdersClick();
                        break;
                    case BlotterConstants.PROPERTY_UPLOAD_STAGE_ORDERS:
                        ctrlBlotterMain.UploadStageOrdersClick();
                        break;
                    case BlotterConstants.PROPERTY_BTN_ROLLOVER_ALL_SUBS:
                        ctrlBlotterMain.RolloverAllSubs();
                        break;
                    case BlotterConstants.PROPERTY_BTN_PREFERENCES:
                        if (LaunchPreferences != null)
                            LaunchPreferences(this, e);
                        break;
                    case BlotterConstants.PROPERTY_BTN_EXPORT_TO_EXCEL:
                        ExportToExcel();
                        break;
                    case BlotterConstants.PROPERTY_BTN_SAVE_ALL_LAYTOUT:
                        ctrlBlotterMain.SaveAllLayout();
                        break;
                    case BlotterConstants.PROPERTY_BTN_ADD_TAB:
                        string tabName = InputBox.ShowInputBox("Enter tab name", CharacterCasing.Normal);
                        if (!string.IsNullOrWhiteSpace(tabName))
                            ctrlBlotterMain.AddTab(tabName);
                        break;
                    case BlotterConstants.PROPERTY_BTN_ADD_ORDER_TAB:
                        string tabName1 = InputBox.ShowInputBox("Enter tab name", CharacterCasing.Normal);
                        if (!string.IsNullOrWhiteSpace(tabName1))
                            ctrlBlotterMain.AddOrdersTab(tabName1);
                        break;
                    case BlotterConstants.PROPERTY_BTN_LINK_UNLINK_TAB:
                        string linkUnlinkCaption = ctrlBlotterMain.LinkUnlikActiveTab();
                        this.buttonTool18.SharedPropsInternal.Caption = linkUnlinkCaption;
                        this.buttonTool18.SharedPropsInternal.ToolTipText = linkUnlinkCaption;
                        break;

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private void ExportToExcel()
        {
            try
            {
                ctrlBlotterMain.SetGridBand(ViewStyle.MultiBand, false);
                Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                string pathName = null;
                exportToExcelFileDialog = new SaveFileDialog();
                exportToExcelFileDialog.InitialDirectory = Application.StartupPath;
                exportToExcelFileDialog.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                exportToExcelFileDialog.RestoreDirectory = true;
                if (System.IO.Directory.Exists(_blotterPreferenceData.DefaultExportPath))
                    exportToExcelFileDialog.InitialDirectory = _blotterPreferenceData.DefaultExportPath;
                if (exportToExcelFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pathName = exportToExcelFileDialog.FileName;

                    for (int counter = 0; counter < this.ctrlBlotterMain.BlotterTabControl.Tabs.Count; counter++)
                    {
                        Infragistics.Win.UltraWinTabControl.UltraTab ultraTab = this.ctrlBlotterMain.BlotterTabControl.Tabs[counter];
                        string name = (ultraTab.Key.StartsWith("Dynamic_")) ? ultraTab.Key.Substring(8) : ultraTab.Key;
                        workBook = ((WorkingSubBlotterGrid)ultraTab.TabPage.Controls[0]).OnExportToExcel(workBook, pathName, BlotterUICommonMethods.SplitCamelCase(name));
                    }
                    workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[0];
                    workBook.Save(pathName);
                    SetStatusBarText("Data exported");
                }
                ctrlBlotterMain.SetGridBand(ViewStyle.SingleBand, true);
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

        internal void UnSubscribeProxy()
        {
            try
            {
                if (_proxy != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_CreateGroup);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_RefreshBlotterAfterImport);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_UpdateBlotterStatusBarMessage);
                    _proxy.Dispose();
                    _proxy = null;
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public string getReceiverUniqueName()
        {
            return "BlotterForm";
        }


        void BlotterGrid_UpdateStatusBar(object sender, EventArgs<string> e)
        {
            try
            {
                SetStatusBarText(e.Value);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
        void BlotterGrid_UpdateCountStatusBar(object sender, EventArgs<string> e)
        {
            try
            {
                SetCountStatusBarText(e.Value);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        private void SetStatusBarText(string message)
        {
            try
            {
                this.toolStripStatusLabel1.Text = "[" + DateTime.Now + "] " + message;
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        private void SetCountStatusBarText(string message)
        {
            try
            {
                this.toolStripStatusLabel2.Text = message;
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        void BlotterMain_UpdateOnRolloverComplete(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { BlotterMain_UpdateOnRolloverComplete(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        ultraToolbarsManager1.Tools[BlotterConstants.PROPERTY_BTN_ROLLOVER_ALL_SUBS].SharedProps.Enabled = true;
                        ctrlBlotterMain.EnableDisableTab(true);
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

        void BlotterGrid_DisableRolloverButton(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { BlotterGrid_DisableRolloverButton(sender, e); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        ultraToolbarsManager1.Tools[BlotterConstants.PROPERTY_BTN_ROLLOVER_ALL_SUBS].SharedProps.Enabled = false;
                        ctrlBlotterMain.EnableDisableTab(false);
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

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }

        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            this.ctrlBlotterMain.ExportData(tabName, filePath);
        }
        #endregion
    }
}