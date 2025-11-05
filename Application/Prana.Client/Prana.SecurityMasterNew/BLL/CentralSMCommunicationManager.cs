using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ServiceModel;
using System.Timers;

namespace Prana.SecurityMasterNew
{
    public class CentralSMCommunicationManager : CentralSMService.ICentralSMServiceCallback, IDisposable
    {
        #region singleton
        private static volatile CentralSMCommunicationManager instance;
        private static readonly object syncRoot = new Object();

        public static CentralSMCommunicationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CentralSMCommunicationManager();
                    }
                }
                return instance;
            }
        }
        #endregion

        Prana.SecurityMasterNew.CentralSMService.CentralSMServiceClient _centralSMServiceClient;

        Timer _heartbeatTimer;
        string _serverIdentifier;

        public void Initialize(string serverIdentifier)
        {
            try
            {
                _serverIdentifier = serverIdentifier;
                CreateClient();
                _heartbeatTimer = new Timer(800);
                _heartbeatTimer.AutoReset = true;
                _heartbeatTimer.Elapsed += heartbeatTimer_Elapsed;
                _heartbeatTimer.Start();
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

        private void CreateClient()
        {
            try
            {

                InstanceContext context = new InstanceContext(this);
                _centralSMServiceClient = new Prana.SecurityMasterNew.CentralSMService.CentralSMServiceClient(context, "WSDualHttpBinding_ICentralSMServiceHeartbeat");
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

        bool _connectionStatus = false;
        // Modified by omshiv, check connection status of CSM
        public bool IsCSMConnected
        {
            get { return _connectionStatus; }
        }

        public event StringHandler CentralSMDisconnected;
        public event StringHandler CentralSMConnected;

        object synchRoot = new object();

        void heartbeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _heartbeatTimer.Stop();
                bool connected = true;
                try
                {
                    lock (synchRoot)
                    {
                        connected = _centralSMServiceClient.IsAlive(_serverIdentifier, null);
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    if (_connectionStatus && CentralSMDisconnected != null)
                    {
                        _connectionStatus = false;
                        CentralSMDisconnected(this, new EventArgs<string>("CentralSM"));
                        Exception pricingConnError = new Exception("Could not connect to CentralSMServer", ex);
                        bool rethrow = Logger.HandleException(pricingConnError, LoggingConstants.POLICY_LOGANDSHOW);
                        if (rethrow)
                        {
                            throw pricingConnError;
                        }
                        CreateClient();
                        _heartbeatTimer.Start();
                        return;
                    }
                    else
                    {
                        CentralSMDisconnected(this, new EventArgs<string>("CentralSM"));
                        Exception pricingConnError = new Exception("Could not connect to CentralSMServer", ex);
                        bool rethrow = Logger.HandleException(pricingConnError, LoggingConstants.POLICY_LOGONLY);
                        if (rethrow)
                        {
                            throw pricingConnError;
                        }
                        CreateClient();
                        _heartbeatTimer.Start();
                        return;
                    }
                }
                if (connected && CentralSMConnected != null)
                {
                    if (_connectionStatus == false)
                    {
                        Logger.LoggerWrite("CentralSM connected. Time " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                        _heartbeatTimer.Start();
                        InformationReporter.GetInstance.Write("CentralSM connected.");
                    }
                    _connectionStatus = true;
                    CentralSMConnected(this, new EventArgs<string>("CentralSM"));
                }
                _heartbeatTimer.Start();
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
            finally { _heartbeatTimer.Start(); }
        }


        public void SecurityValidationResp(BusinessObjects.SecurityMasterBusinessObjects.SecMasterBaseObj securityData)
        {

        }
        public void GenricPricingResp(string requestID, System.Data.DataTable pricingTable, bool pricingSuccess, string comment)
        {

        }

        public void SymbolLookUpSearchDataResp(System.Data.DataSet SymbolsData, BusinessObjects.Classes.SecurityMasterBusinessObjects.SymbolLookupRequestObject symbolLookupRequestObject)
        {

        }

        public void SaveFutureRootDataLocal(System.Data.DataTable FutureRootData)
        {

        }

        public void FutureRootDataResp(System.Data.DataSet futureData)
        {

        }

        public void IsAliveResp()
        {

        }


        public void GenericCentralSMResponse(string responseType, object message)
        {

        }


        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (_centralSMServiceClient != null)
                        _centralSMServiceClient.Close();

                    if (_heartbeatTimer != null)
                    {
                        _heartbeatTimer.Dispose();
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
