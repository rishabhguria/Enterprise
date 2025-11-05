using ExportGridsData;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Forms
{
    public partial class ImportData : Form, IPublishing, IPluggableTools, ILaunchForm, ILiveFeedCallback, IExportGridData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePosition"/> class.
        /// </summary>
        public ImportData()
        {
            InitializeComponent();
        }



        DuplexProxyBase<ISubscription> _proxy;
        private void MakeProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _proxy.Subscribe(Topics.Topic_ImportAck, null);
                _proxy.Subscribe(Topics.Topic_SecurityMaster, null);
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

        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        private void CreatePricingServiceProxy()
        {
            _pricingServicesProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
            ctrlRunDownload1.PricingServices = _pricingServicesProxy;
        }

        public void DisposeProxies()
        {
            try
            {
                if (_proxy != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_ImportAck);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_SecurityMaster);
                    _proxy.Dispose();
                }

                if (_pricingServicesProxy != null)
                {
                    //_pricingServicesProxy.UnSubscribe();
                    _pricingServicesProxy.Dispose();
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
        /// Recieve Published subscribed topic data
        /// </summary>
        /// <param name="e"></param>
        /// <param name="topicName"></param>
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
                            System.Object[] dataList = null;
                            //Handeling data beased on topic name
                            switch (topicName)
                            {
                                case Topics.Topic_ImportAck:
                                    dataList = (System.Object[])e.EventData;
                                    foreach (Object obj in dataList)
                                    {
                                        ctrlRunDownload1.IsGroupSaved = bool.Parse(obj.ToString());
                                    }
                                    break;

                                case Topics.Topic_SecurityMaster:
                                    dataList = (System.Object[])e.EventData;
                                    //add and refresh import data grid.
                                    foreach (Object obj in dataList)
                                    {
                                        SecMasterBaseObj secMasterObj = obj as SecMasterBaseObj;
                                        if (secMasterObj != null)
                                        {
                                            ctrlRunDownload1._securityMaster_SecMstrDataResponse(null, new EventArgs<SecMasterBaseObj>(secMasterObj));
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

        #region IPluggableTools Members

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        ISecurityMasterServices _securityMaster = null;

        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
            }
        }
        public void SetUP()
        {
            MakeProxy();
            CreatePricingServiceProxy();

        }

        public IPostTradeServices PostTradeServices
        {
            set {; }
        }


        #endregion

        private void ctrlRunDownload1_Load(object sender, EventArgs e)
        {
            CompanyUser companyUser = Prana.CommonDataCache.CachedDataManager.GetInstance.LoggedInUser;
            ctrlRunDownload1.PopulateRunUploadDetails(companyUser, _securityMaster);
            ctrlRunDownload1.SetLaunchForm(LaunchForm);
            //ctrlRunDownload1.WorkInProgress += new EventHandler(ctrlRunDownload1_WorkInProgress);

            ctrlRunDownload1.ProgressEvent += new EventHandler<EventArgs<string, bool, Progress>>(ctrlRunDownload1_ProgressEvent);
            _securityMaster.SecMstrBulkResponse += new EventHandler<EventArgs<int, int, int>>(ctrlRunDownload1_ProgressEvent);

            ctrlRunDownload1.ReImportCompleted += new EventHandler(ctrlRunDownload1_ReImportCompleted);
            FileAndDbSyncManager.SyncFileWithDataBase(Application.StartupPath, ApplicationConstants.MappingFileType.ReconMappingXml);

        }


        void ctrlRunDownload1_ReImportCompleted(object sender, EventArgs e)
        {
            ctrlReImport1.BindReImportGrid();
        }



        void ctrlRunDownload1_ProgressEvent(Object sender, EventArgs<string, bool, Progress> e)
        {
            try
            {
                switch (e.Value3)
                {
                    case Progress.Start:
                        this.ctrlProgress1.pBarProgressing.Minimum = 0;
                        this.ctrlProgress1.pBarProgressing.Maximum = int.Parse(e.Value);
                        this.ctrlProgress1.pBarProgressing.Show();
                        this.ctrlProgress1.timerProgress.Start();
                        break;

                    case Progress.Running:
                        if (!e.Value2)
                        {
                            ctrlProgress1.ImportingText = e.Value;
                        }
                        else
                        {
                            int recordsProcessed = int.Parse(e.Value);
                            this.ctrlProgress1.ProgressValue = recordsProcessed;
                        }
                        break;

                    case Progress.End:
                        this.ctrlProgress1.ImportingText = string.Empty;
                        this.ctrlProgress1.ProgressingText = string.Empty;
                        this.ctrlProgress1.pBarProgressing.Value = 0;
                        this.ctrlProgress1.pBarProgressing.Hide();
                        if (this.ctrlProgress1.timerProgress.Enabled)
                            this.ctrlProgress1.timerProgress.Stop();
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
        protected delegate void ProgressEvent(Object sender, EventArgs<int, int, int> e);
        void ctrlRunDownload1_ProgressEvent(Object sender, EventArgs<int, int, int> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new ProgressEvent(ctrlRunDownload1_ProgressEvent), new object[] { sender, e });
                    }
                    else
                    {
                        if (e.Value3 == 1)
                        {
                            this.ctrlProgress1.ImportingText = "Error on Server.";
                            this.ctrlProgress1.ProgressingText = string.Empty;
                            this.ctrlProgress1.pBarProgressing.Value = 0;
                            this.ctrlProgress1.pBarProgressing.Hide();
                        }
                        else
                        {
                            if (e.Value2 < e.Value)
                            {
                                this.ctrlProgress1.ImportingText = "Importing...";
                                this.ctrlProgress1.pBarProgressing.Minimum = 0;
                                this.ctrlProgress1.pBarProgressing.Value = e.Value2;
                                this.ctrlProgress1.pBarProgressing.Maximum = e.Value;
                                this.ctrlProgress1.pBarProgressing.Show();
                            }
                            else if (e.Value2 == e.Value)
                            {
                                this.ctrlProgress1.ImportingText = string.Empty;
                                this.ctrlProgress1.ProgressingText = string.Empty;
                                this.ctrlProgress1.pBarProgressing.Value = 0;
                                this.ctrlProgress1.pBarProgressing.Hide();
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


        private ImportPreferences _importPrefs = null;
        public ImportPreferences ImportPrefs
        {
            get { return _importPrefs; }
            set { _importPrefs = value; }
        }


        bool _canCloseImport = false;
        private void ImportData_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (!_canCloseImport)
            {
                if (ctrlRunDownload1.IsImportRunning)
                {
                    DialogResult result = MessageBox.Show("Import is under operation! Do you still want to close?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (result.Equals(DialogResult.Yes))
                    {
                        _canCloseImport = true;
                    }
                    else
                    {
                        e.Cancel = true;
                        _canCloseImport = false;
                    }
                }
            }
        }

        void ctrlReImport1_Load(object sender, System.EventArgs e)
        {
            ctrlReImport1.ImportFile += new EventHandler<EventArgs<string, string>>(ctrlReImport1_ImportFile);
        }

        void ctrlReImport1_ImportFile(Object sender, EventArgs<string, string> e) //string filePath, string importType)
        {
            ctrlRunDownload1.UploadReImportFile(e.Value, e.Value2);
        }

        void ctrlImportPrefs1_Load(object sender, System.EventArgs e)
        {
            this.ImportPrefs = ctrlImportPrefs1.ImportPrefs;
        }



        void tcCreateandImportPositions_ActiveTabChanging(object sender, Infragistics.Win.UltraWinTabControl.ActiveTabChangingEventArgs e)
        {
            if (e.Tab.Key.Equals("Preferences"))
            {
                ctrlImportPrefs1.SetPreferences();
            }
            if (e.Tab.Key.Equals("ReImportData"))
            {
                ctrlReImport1.BindReImportGrid();
            }
            //throw new System.Exception("The method or operation is not implemented.");
        }


        private void ImportData_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (PluggableToolsClosed != null)
            {
                PluggableToolsClosed(this, EventArgs.Empty);
            }
            _canCloseImport = false;
            InstanceManager.ReleaseInstance(typeof(ImportData));
        }

        #region ILaunchForm Members

        public event EventHandler LaunchForm;

        #endregion


        #region IPluggableTools Members


        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        #endregion

        #region ILiveFeedCallback Members
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void LiveFeedDisConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        #region IPublishing Members
        public string getReceiverUniqueName()
        {
            return "ImportData";
        }
        #endregion

        private void ImportData_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_IMPORT_DATA);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
                InstanceManager.RegisterInstance(this);
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

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// used to Export Data for automation
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            this.ctrlRunDownload1.ExportGridData(filePath, gridName);
        }
    }
}