using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.Interfaces;
using Castle.Windsor;
using Prana.PubSubService;
using System.Configuration;
using System.IO;
using Prana.ReportingServices;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.BusinessObjects;
using Prana.SocketCommunication;
using System.Xml.Serialization;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.XMLUtilities;
using System.Collections;
using Prana.Utilities;
using Quartz;
using Quartz.Impl;



namespace Prana.ReportingServer
{
    public partial class ReportingServer : Form
    {
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ReportingServer)); 

        #region Global Variables Section
        
        IWindsorContainer _container;
        delegate void SetTextCallback2(string msg);

        //Global Beacouse Its Being Used In Constructor and btnCalculate_Click
        ClientSettingsPref ObjectDeSerialize;

        public ReportingServer()
        {
            try
            {
                InitializeComponent();

                //Will Be enabled After Server Start-----------
                btnCalculate.Enabled = false;
                //btnCreateStructure.Enabled = false;
                btnRefresh.Enabled = false;
                //btnSyncFunds.Enabled = false;
                //btnShowFunds.Enabled = false;
                //btnShowSettings.Enabled = false;
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

        //Also Being Used In frmAdmin
        public void InitialeSettings()
        {
            try
            {
                ObjectDeSerialize = ReportingServicesDataManager.ImportXMLSettings;
                List<string> clientSettingsNames = ObjectDeSerialize.getNames();
                clientSettingsNames.Add("All");
                cmbxClientSettingNames.DataSource = clientSettingsNames;

                FileWatcher _watcher = new FileWatcher();
                string filePath = ReportingServicesDataManager.BaseSettings.FilePath;
                string InputFilesPath = filePath + "\\Input files\\";
                _watcher.CreateWatcher(InputFilesPath, filePath);
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

        #region Private Functions Section

        private void StartReportingServer()
        {
            try
            {
                lstbxErrorLog.Items.Clear();
                InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(GetInstance_InformationReceived);
                log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.xml"));
               // PranaPubSubService.Initlise(ConfigurationManager.AppSettings["PublishingEndpointAddress"], ConfigurationManager.AppSettings["SubscriptionEndpointAddress"]);
                ConnectToAllSockets();
                HostPositionServices();
                HostReportingServices();
                HostRiskServices();
               
                Preferences userPreferences = new Preferences();
              //  XMLUtilities.SerializeToXMLFile<Preferences>(userPreferences, Application.StartupPath + "\\settings.xml");
                
                this.btnStartReportingServer.Enabled = false;

                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

                this.btnStartReportingServer.Enabled = true;
            }
        }               
        
        void GetInstance_InformationReceived(string message)
        {
            try
            {
                if (lstbxErrorLog.InvokeRequired)
                {
                    SetTextCallback2 mi = new SetTextCallback2(GetInstance_InformationReceived);
                    this.Invoke(mi, new object[] { message });
                }
                else
                {
                    message = message + " Time :=" + DateTime.Now.ToString();
                    lstbxErrorLog.Items.Add(message);
                    QueueMessage qmsg = new QueueMessage();
                    qmsg.MsgType = CustomFIXConstants.MSG_ExceptionRaised;
                    qmsg.Message = message;

                   //MonitoringProcessor.GetInstance.ProcessMessage(qmsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        ///  All Socket Connections are Established Here
        /// </summary>

        ICommunicationManager _connectionPricingServer = null;
        private void ConnectToAllSockets()
        {

            try
            {
                _connectionPricingServer = new ClientTradeCommManager();
                //_connectionPricingServer.Connected += new EventHandler(_PricingServerConnected);
                //_connectionPricingServer.Disconnected += new EventHandler(_PricingServerDisconnected);
                _connectionPricingServer.Connect(GetPricingConnectionProperties());

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                // ExceptionPolicy.HandleException(new Exception("Server Not Available"), Common.POLICY_LOGANDSHOW);

            }
        }

        private ConnectionProperties GetPricingConnectionProperties()
        {
            CompanyUser user = null;

            if (user == null)
            {
                user = new CompanyUser();
                user.CompanyUserID = 100;
                user.FirstName = "AutomationServer";
            }

            // _hashCode = this.GetHashCode();

            ConnectionProperties connProperties = new ConnectionProperties();
            connProperties.Port = ClientAppConfiguration.PricingServer.Port;
            connProperties.ServerIPAddress = ClientAppConfiguration.PricingServer.IpAddress;
            connProperties.IdentifierID = "OptionAnal" + user.CompanyUserID.ToString();
            connProperties.IdentifierName = "Option Analytics" + user.FirstName.ToString();
            connProperties.ConnectedServerName = "Option Analytics Server";
            connProperties.HandlerType = HandlerType.OptionCalcStatic;
            connProperties.User = user;
            return connProperties;
        }
        
       

        #endregion

        #region Services Section

        static PranaReportingServices _pranaReportingServices = new PranaReportingServices();
        public static PranaReportingServices PranaReportingServices
        {
            get
            {
                return _pranaReportingServices;
            }
        }
        private IPranaPositionServices _positionServices;

        public IPranaPositionServices PositionServices
        {
            set
            {
                _positionServices = value;
            }
        }
        IAllocationServices _allocationServices = null;
        //private ISecMasterServices _secMasterServices;
        //public ISecMasterServices SecMasterServices
        //{
        //    set { _secMasterServices = value; }

        //}
        IRiskServices _riskServices = null;
        public IRiskServices RiskServices
        {
            set
            {
                _riskServices = value;

            }

        }
        public IAllocationServices AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
        }
        private void HostReportingServices()
        {
            try
            {
                IReportingServices reportingServices = _container.Resolve<IReportingServices>();

                PranaServiceHost.HostPranaService(reportingServices, typeof(IReportingServices), ConfigurationManager.AppSettings["RiskReportingEndpointAddress"]);

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
        private void HostPositionServices()
        {
            try
            {
                IPranaPositionServices reportingServices = _container.Resolve<IPranaPositionServices>();

                PranaServiceHost.HostPranaService(reportingServices, typeof(IPranaPositionServices), ConfigurationManager.AppSettings["PositionServicesEndpointAddress"]);

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

        private void HostRiskServices()
        {
            try
            {
                IRiskServices riskServices = _container.Resolve<IRiskServices>();

                PranaServiceHost.HostPranaService(riskServices, typeof(IRiskServices), ConfigurationManager.AppSettings["RiskServicesAddress"]);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        internal void SetContainer(IWindsorContainer container)
        {
            _container = container;
        }

        #endregion

        #region UI Section

        private void btnClearMessageBox_Click(object sender, EventArgs e)
        {
            lstbxErrorLog.Items.Clear();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                btnCalculate.Enabled = false;
                if (cmbxClientSettingNames.SelectedItem.ToString() == "All")
                {
                    _pranaReportingServices.ProcessRequest(ObjectDeSerialize.getClientSettings());
                }
                else
                {
                    ClientSettings _clientSettings = ObjectDeSerialize.getClientSettings(cmbxClientSettingNames.SelectedItem.ToString());
                    List<ClientSettings> settings = new List<ClientSettings>();
                    settings.Add(_clientSettings);
                    _pranaReportingServices.ProcessRequest(settings);
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
                btnCalculate.Enabled = true;
            }
        }
        

        private void btnCreateStructure_Click(object sender, EventArgs e)
        {
            try
            {
                _pranaReportingServices.CreateStructure(udtStructureDate.DateTime);
                MessageBox.Show("Folder Structure Created Successfully For Date:--" + udtStructureDate.DateTime);
              
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                lstbxErrorLog.Items.Clear();
                logger.Info(" Refresh Processing Started.......");
                _pranaReportingServices.Refresh();
                InitialeSettings();
                if (cmbxClientSettingNames.Items.Count > 0)
                    btnCalculate.Enabled = true;                
                logger.Info(" Refresh Processing Completed.");
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
        IScheduler sched;
        public void StartScheduler()
        {
            if(sched!=null)
                sched.Shutdown();

            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            
            // get a scheduler
             sched = schedFact.GetScheduler();
            sched.Start();
           
            //string CronExpression=ConfigurationManager.AppSettings["CronExpression"];
            string CronExpression = ReportingServicesDataManager.BaseSettings.CronExpression;
            CronTrigger contrigger = new CronTrigger("n", "g", CronExpression);
            SimpleTrigger trigger = new SimpleTrigger("myTrigger",
                                          "myGroup",
                                          DateTime.UtcNow,
                                          DateTime.UtcNow.AddSeconds(40),
                                          SimpleTrigger.RepeatIndefinitely,
                                          TimeSpan.FromSeconds(10));

            // construct job info
            JobDetail jobDetail = new JobDetail("myJob", null, typeof(AutomationJob));
            // fire every hour
            //Trigger trigger = TriggerUtils.MakeHourlyTrigger();
            // start on the next even hour
            // trigger.StartTime = TriggerUtils.GetEvenHourDate(DateTime.UtcNow);
            trigger.Name = "myTrigger";
            sched.ScheduleJob(jobDetail, contrigger); 
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {

                StartReportingServer();
                StartScheduler();
                //To Create Folder Structure At Server Start
                _pranaReportingServices.CreateStructure(udtStructureDate.DateTime);

                //To Initialize Dictionaries At Server Start UP
                _pranaReportingServices.initializeDictionaries();

                InitialeSettings();



                _pranaReportingServices.PositionServices = _positionServices;
                _pranaReportingServices.RiskServices = _riskServices;
                _pranaReportingServices.AllocationServices = _allocationServices;
                //_pranaReportingServices.SecMasterServices = _secMasterServices;
                _pranaReportingServices.RiskServices = _riskServices;

                if (cmbxClientSettingNames.Items.Count > 0)
                    btnCalculate.Enabled = true;
                //btnCreateStructure.Enabled = true;
                btnRefresh.Enabled = true;
                //btnSyncFunds.Enabled = true;
                ////Its Not Impleted Yet
                //btnShowFunds.Enabled = false;
                //btnShowSettings.Enabled = false;
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

        private void btnShowFunds_Click(object sender, EventArgs e)
        {

        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                _pranaReportingServices.ImportFundsFromDifferentClientsDB();
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

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                frmAdmin objFrmAdmin = (frmAdmin)_container[typeof(frmAdmin)];
                objFrmAdmin.SetContainer(_container);
                objFrmAdmin.Show();
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
              

        private void ReportingServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(sched !=null)
            sched.Shutdown();
        }
       
    }
}