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
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Forms
{
    public partial class PostReconAmendments : Form, IPostReconAmendmentsUI, IPublishing, ILaunchForm
    {

        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public PostReconAmendments()
        {
            try
            {
                InitializeComponent();
                this.FormClosed += new System.EventHandler(this.PostReconAmendments_FormClosed);
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

        #region class members
        ProxyBase<IClosingServices> _closingServices = null;
        private delegate void UIThreadMarsheller(object sender, EventArgs e);
        int _accountID = int.MinValue;
        string _symbol = string.Empty;
        //DateTime _date = DateTime.MinValue;
        #endregion

        #region IPostReconAmendmentsUI Members

        public Form Reference()
        {
            return this;
            //throw new NotImplementedException();
        }

        public new event EventHandler FormClosed;

        public CompanyUser User
        {
            set
            {
                //throw new NotImplementedException(); 
            }
        }

        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                //throw new NotImplementedException(); 
            }
        }

        public void SetUp(int accountID, string symbol, DateTime date, string comment, EventHandler UpdateCommentsFromPostReconAmendments)
        {

            try
            {
                _accountID = accountID;
                _symbol = symbol;
                //_date = date;
                if (_proxy != null)
                {
                    DisposeProxies();
                }
                CreateProxies();
                ////CHMW-1620 [Closing] - Add Comments field in PostReconAmendenmtsUI
                ctrlPostReconAmendmend1.SetUp(accountID, symbol, date, comment, UpdateCommentsFromPostReconAmendments);
                //if (_isFormInitialized && !backgroundWorker.IsBusy)
                //{
                //    backgroundWorker.RunWorkerAsync();
                //}
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

        #region Wire Events
        private void WireEvents()
        {
            try
            {
                //closing services disconnect and connect event handler
                _closingServices.DisconnectedEvent += new Proxy<IClosingServices>.ConnectionEventHandler(ClosingServices_ClearData);
                _closingServices.ConnectedEvent += new Proxy<IClosingServices>.ConnectionEventHandler(_closingServices_RefreshData);

                //event wiring for background worker events
                //backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                //backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);

                //this.LaunchForm += PostReconAmendments_LaunchForm;

                ctrlPostReconAmendmend1.formCloseHandler += new FormClosedEventHandler(ctrlPostReconAmendmend_FormCloseHandler);
                ctrlPostReconAmendmend1.launchForm += this.LaunchForm;
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

        private void ctrlPostReconAmendmend_FormCloseHandler(object sender, EventArgs e)
        {
            try
            {
                this.Close();
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

        //private void PostReconAmendments_LaunchForm(object sender, EventArgs e)
        //{

        //}
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void CreateProxies()
        {
            CreateClosingServicesProxy();
            CreateAllocationServicesProxy();
            CreateSubscriptionServicesProxy();
            ctrlPostReconAmendmend1.CreatePricingServiceProxy();
        }

        private void CreateClosingServicesProxy()
        {
            try
            {
                _closingServices = new ProxyBase<IClosingServices>("TradeClosingServiceEndpointAddress");
                ctrlPostReconAmendmend1.ClosingServices = _closingServices;
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

        ProxyBase<IAllocationManager> _allocationServices = null;
        private void CreateAllocationServicesProxy()
        {
            try
            {
                _allocationServices = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
                ctrlPostReconAmendmend1.AllocationServices = _allocationServices;
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

        DuplexProxyBase<ISubscription> _proxy;
        private void CreateSubscriptionServicesProxy()
        {
            try
            {

                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);

                #region Create filter
                List<int> accountID = new List<int>();
                accountID.Add(_accountID);

                FilterDataByExactAccount accountFilterdata = new FilterDataByExactAccount();
                accountFilterdata.GivenAccountID = accountID;

                FilterDataByExactSymbol symbolFilterdata = new FilterDataByExactSymbol();
                symbolFilterdata.GivenSymbol = _symbol;

                List<FilterData> filter = new List<FilterData>();
                filter.Add(accountFilterdata);
                filter.Add(symbolFilterdata);
                #endregion

                _proxy.Subscribe(Topics.Topic_Allocation, filter);
                _proxy.Subscribe(Topics.Topic_Closing, filter);
                _proxy.Subscribe(Topics.Topic_Closing_NetPositions, filter);
                _proxy.Subscribe(Topics.Topic_UnwindPositions, null);
                _proxy.Subscribe(Topics.Topic_SecurityMaster, null);
                ctrlPostReconAmendmend1.SetProxy(_proxy);

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

        private void ClosingServices_ClearData(object sender, EventArgs e)
        {
            try
            {
                UIThreadMarsheller mi = new UIThreadMarsheller(ClosingServices_ClearData);
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        PostReconClosingData.ClearRepository();
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
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        ctrlPostReconAmendmend1.btnReverse_Click(null, null);
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

        #region UI methods
        private void PostReconAmendments_FormClosed(object sender, System.EventArgs e)
        {
            try
            {
                _closingServices.DisconnectedEvent -= new Proxy<IClosingServices>.ConnectionEventHandler(ClosingServices_ClearData);
                _closingServices.ConnectedEvent -= new Proxy<IClosingServices>.ConnectionEventHandler(_closingServices_RefreshData);
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

        void PostReconAmendments_Disposed(object sender, System.EventArgs e)
        {
            try
            {
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
        #endregion

        private void PostReconAmendments_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSE_TRADE);
                WireEvents();
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

        private void PostReconAmendments_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //DisposeProxies();
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
        /// Dispose Proxies and closing services
        /// </summary>
        private void DisposeProxies()
        {
            try
            {
                if (_closingServices != null)
                    _closingServices.Dispose();
                if (_proxy != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_Allocation);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing_NetPositions);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_UnwindPositions);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_SecurityMaster);
                    _proxy.Dispose();
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
                        this.BeginInvoke(mi, new object[] { e, topicName });
                    }
                    else
                    {
                        try
                        {
                            if (AmendmentsHelper.IsAmendmentsToSave() || PostReconClosingData.IsUnsavedChanges)
                            {
                                ctrlPostReconAmendmend1.DisableEnableForm(false);
                                ctrlPostReconAmendmend1.ultraStatusBar1.Text = @"Data amended.Please click ""Get Data""/""Reverse"" button to get updated data.";
                                ctrlPostReconAmendmend1.DisableEnableForm(false);
                                ctrlPostReconAmendmend1.DisableEnableGetDataButton(true);
                                return;
                            }
                            System.Object[] dataList = null;
                            ClosingData closingData = new ClosingData();
                            string groupID = PostReconClosingData.GroupId;

                            switch (topicName)
                            {
                                case Topics.Topic_Allocation:

                                    dataList = (System.Object[])e.EventData;

                                    List<TaxLot> listTaxlotToPopulate = new List<TaxLot>();

                                    List<TaxLot> listCloseTradeTaxlots = new List<TaxLot>();
                                    bool isAnotherAccountSymbolTradePosted = false;
                                    foreach (Object obj in dataList)
                                    {
                                        TaxLot taxlot = (TaxLot)obj;



                                        NameValueFiller.FillNameDetailsOfMessage(taxlot);
                                        FillClosingSpecificDetails(taxlot);
                                        PostReconClosingData.UpdatePublishedTaxlots(taxlot);
                                        if (taxlot.GroupID.Equals(groupID) && taxlot.TaxLotState != ApplicationConstants.TaxLotState.Deleted)
                                        {
                                            listCloseTradeTaxlots.Add(taxlot);
                                        }

                                        if (PostReconClosingData.GetSidesForClosingTaxlots().Contains(taxlot.OrderSideTagValue) && PostReconClosingData.Symbol.Equals(taxlot.Symbol))
                                        {
                                            listTaxlotToPopulate.Add(taxlot);
                                        }
                                    }

                                    if (isAnotherAccountSymbolTradePosted)
                                    {
                                        MessageBox.Show("Trade belongs to another Account/Symbol hence not displayed.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                                    closingData.Taxlots = taxlots;
                                    PostReconClosingData.UpdateRepository(closingData);
                                    if (isCloseOrderTaxlotsUpdated)
                                    {
                                        //ctrlCloseTradefromAllocation1.UpdateClosingTaxlots(closeOrderTaxlotsToUpdate);
                                        //ctrlCloseTradefromAllocation1.UpdateSellOpenQtyAccountAndStrategyWise();
                                        //ctrlCloseTradefromAllocation1.UpdateAllocatedAndAvailableQtyFromParentForm();
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
                                    PostReconClosingData.UpdateRepository(closingData);
                                    break;

                                case Topics.Topic_UnwindPositions:
                                    dataList = (System.Object[])e.EventData;
                                    string pos = string.Empty;
                                    foreach (Object str in dataList)
                                    {
                                        pos = str.ToString();
                                    }
                                    closingData.PositionsToUnwind = pos;

                                    PostReconClosingData.UpdateRepository(closingData);
                                    break;

                                case Topics.Topic_SecurityMaster:
                                    dataList = (System.Object[])e.EventData;
                                    SecMasterbaseList secMasterObjlist = new SecMasterbaseList();
                                    foreach (Object secmasterObj in dataList)
                                    {
                                        secMasterObjlist.Add((SecMasterBaseObj)secmasterObj);
                                    }
                                    PostReconClosingData.UpdateRepositoryWithSecmasterData(secMasterObjlist);
                                    break;


                                default:
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

        public string getReceiverUniqueName()
        {
            return "PostReconAmendments";
        }

        #endregion

        private void FillClosingSpecificDetails(TaxLot taxlot)
        {
            try
            {
                //modified omshiv, ACA code cleanup
                // taxlot.ACAData.ACAAvgPrice = taxlot.AvgPrice;
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

        public event EventHandler LaunchForm;

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
