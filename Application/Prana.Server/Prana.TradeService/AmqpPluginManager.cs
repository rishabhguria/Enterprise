using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Prana.TradeService.AmqpPlugin
{
    internal class AmqpPluginManager
    {
        static AmqpPluginManager _amqpPluginManager;

        static ConnectionStatusManager _esperConnection;
        static ConnectionStatusManager _ruleMediatorConnection;
        static ConnectionStatusManager _basketComplianceConnection;

        public static event Prana.Interfaces.ConnectionMessageReceivedDelegate Connected;
        public static event Prana.Interfaces.ConnectionMessageReceivedDelegate Disconnected;


        internal static AmqpPluginManager GetInstance()
        {
            if (_amqpPluginManager == null)
                _amqpPluginManager = new AmqpPluginManager();
            return _amqpPluginManager;
        }
        private AmqpPluginManager()
        { }


        #region Private properties

        String _esperRequestExchangeName;
        String _basketComplianceRequestExchangeName;
        String _otherDataExchangeName;
        //String _amqpServerName;
        bool isListenerStarted = false;
        ISecMasterServices _secMasterServices;

        #endregion
        /// <summary>
        /// Initialise amqp plugin so that data can be sent to another components as esper
        /// </summary>
        /// <param name="secMasterServices"></param>
        internal void Initialise(ISecMasterServices secMasterServices)
        {
            try
            {
                this._secMasterServices = secMasterServices;
                this._secMasterServices.AuecDetailsUpdated += new EventHandler<EventArgs<AuecDetails>>(SecMasterServices_AuecDetailsUpdated);
                //new AuecDetailsUpdatedHandler(SecMasterServices_AuecDetailsUpdated);
                this._secMasterServices.SecurityObjectReceived += new EventHandler<EventArgs<SecMasterBaseObj>>(SecMasterServices_SecurityObjectReceived);
                //new SecurityObjectReceivedHandler(SecMasterServices_SecurityObjectReceived);
                //this._secMasterServices.UDADataReceived += new UDADataReceivedHandler(SecMasterServices_UDADataReceived);
                LoadAppSettings();
                InitializeAmqpConnection();

                _esperConnection = new ConnectionStatusManager(Module.EsperCalculator);
                _ruleMediatorConnection = new ConnectionStatusManager(Module.RuleEnginMediator);
                _basketComplianceConnection = new ConnectionStatusManager(Module.BasketCompliance);
                _esperConnection.StatusChanged += Connection_StatusChanged;
                _ruleMediatorConnection.StatusChanged += Connection_StatusChanged;
                _basketComplianceConnection.StatusChanged += Connection_StatusChanged;
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
        }

        /// <summary>
        /// Logs and updates the client with connection status
        /// </summary>
        /// <param name="e"></param>
        void Connection_StatusChanged(Object sender, ConnectionEventArguments e)
        {
            try
            {
                switch (e.Module)
                {
                    case Module.EsperCalculator:
                        if (e.ConnectionStatus)
                        {
                            if (Connected != null)
                            {
                                ConnectionStatusManager._isEsperConnected = true;
                                Connected(this, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Esper Calculation Engine", IdentifierID = "" }));
                                InformationReporter.GetInstance.ComplianceLogWrite("Esper Calculation Engine Connected.\n");
                                Logger.LoggerWrite("Esper Calculation Engine Connected.\n", LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG, 1, 1, TraceEventType.Information);
                            }
                        }
                        else
                        {
                            if (Disconnected != null)
                            {
                                Disconnected(this, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Esper Calculation Engine", IdentifierID = "" }));
                                InformationReporter.GetInstance.ComplianceLogWrite("Esper Calculation Engine Disconnected.\n");
                                Logger.LoggerWrite("Esper Calculation Engine Disconnected.\n", LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG, 1, 1, TraceEventType.Information);
                                ConnectionStatusManager._isEsperConnected = false;
                            }
                        }
                        break;
                    case Module.RuleEnginMediator:
                        if (e.ConnectionStatus)
                        {
                            if (Connected != null)
                            {
                                ConnectionStatusManager._isRuleMediatorConnected = true;
                                Connected(this, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Rule Mediator", IdentifierID = "" }));
                                InformationReporter.GetInstance.ComplianceLogWrite("Rule Mediator Connected.\n");
                                Logger.LoggerWrite("Rule Mediator Connected.\n", LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG, 1, 1, TraceEventType.Information);
                            }
                        }
                        else
                        {
                            if (Disconnected != null)
                            {
                                ConnectionStatusManager._isRuleMediatorConnected = false;
                                Disconnected(this, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Rule Mediator", IdentifierID = "" }));
                                InformationReporter.GetInstance.ComplianceLogWrite("Rule Mediator Disconnected.\n");
                                Logger.LoggerWrite("Rule Mediator Disconnected.\n", LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG, 1, 1, TraceEventType.Information);
                            }
                        }
                        break;
                    case Module.BasketCompliance:
                        if (e.ConnectionStatus)
                        {
                            if (Connected != null)
                            {
                                ConnectionStatusManager._isBasketconnected = true;
                                Connected(this, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Basket Compliance Engine", IdentifierID = "" }));
                                InformationReporter.GetInstance.ComplianceLogWrite("Basket Compliance Engine Connected.\n");
                                Logger.LoggerWrite("Basket Compliance Engine Connected.\n", LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG, 1, 1, TraceEventType.Information);
                            }
                        }
                        else
                        {
                            if (Disconnected != null)
                            {
                                Disconnected(this, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Basket Compliance Engine", IdentifierID = "" }));
                                InformationReporter.GetInstance.ComplianceLogWrite("Basket Compliance Engine Disconnected.\n");
                                Logger.LoggerWrite("Basket Compliance Engine Disconnected.\n", LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG, 1, 1, TraceEventType.Information);
                                ConnectionStatusManager._isBasketconnected = false;
                            }
                        }
                        break;
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


        #region Event listeners

        //void SecMasterServices_UDADataReceived(Dictionary<string, UDAData> udaDataCollection)
        //{
        //    try
        //    {
        //        AmqpHelper.SendObject(udaDataCollection, "OtherDataSender", "UDAData");
        //    }
        //    catch (Exception ex)
        //    {
        //        //Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        //throwing exception up to the stack
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        void SecMasterServices_SecurityObjectReceived(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                // TO-DO : Addition of dynamic UDA should not be here
                Dictionary<string, DynamicUDA> _dynamicUDAcache = _secMasterServices.GetDynamicUDAList();
                SecMasterBaseObj secMasterBaseObj = DeepCopyHelper.Clone(e.Value);
                foreach (string uda in _dynamicUDAcache.Keys)
                {
                    if (!secMasterBaseObj.DynamicUDA.ContainsKey(uda))
                        secMasterBaseObj.DynamicUDA.Add(uda, _dynamicUDAcache[uda].DefaultValue);
                }
                AmqpHelper.SendObject(secMasterBaseObj, "SecurityDetails", null);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //throwing exception up to the stack
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void SecMasterServices_AuecDetailsUpdated(object sender, EventArgs<AuecDetails> e)
        {
            try
            {
                AmqpHelper.SendObject(e.Value, "OtherDataSender", "AuecDetails");
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //throwing exception up to the stack
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        private void LoadAppSettings()
        {
            try
            {
                _esperRequestExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_EsperRequestExchange);
                _basketComplianceRequestExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_BasketComplianceRequestExchange);
                _otherDataExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);
                //_amqpServerName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_AmqpServer);
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
        }

        private void InitializeAmqpConnection()
        {
            try
            {
                InitializeSender();
                InitializeReceiver();
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
        }


        private void InitializeSender()
        {
            try
            {
                AmqpHelper.InitializeSender("OtherDataSender", _otherDataExchangeName, MediaType.Exchange_Direct);
                AmqpHelper.SenderInitialised += AmqpHelper_SenderInitialised;

                AmqpHelper.InitializeSender("BasketComplianceExchange", _basketComplianceRequestExchangeName, MediaType.Exchange_Direct);
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
        }

        void AmqpHelper_SenderInitialised(object sender, EventArgs<string> e)
        {
            try
            {
                if (e.Value.Equals("OtherDataSender"))
                    SendSecurityObjectsToAmqp();
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
        }

        private void InitializeReceiver()
        {
            try
            {
                AmqpHelper.Started += new ListenerStarted(AmqpListenerStarted);
                List<String> keyList = new List<string>();
                keyList.Add("TradeServer");
                AmqpHelper.InitializeListenerForExchange(_esperRequestExchangeName, MediaType.Exchange_Direct, keyList);
                AmqpHelper.InitializeListenerForExchange(_basketComplianceRequestExchangeName, MediaType.Exchange_Direct, keyList);

                List<String> keyList1 = new List<string>();
                keyList1.Add("EsperHEARTBEAT");
                AmqpHelper.InitializeListenerForExchange(_otherDataExchangeName, MediaType.Exchange_Direct, keyList1);

                List<String> keyList2 = new List<string>();
                keyList2.Add("RuleMediatorHEARTBEAT");
                AmqpHelper.InitializeListenerForExchange(_otherDataExchangeName, MediaType.Exchange_Direct, keyList2);

                List<String> keyList3 = new List<string>();
                keyList3.Add("BasketComplianceHEARTBEAT");
                AmqpHelper.InitializeListenerForExchange(_otherDataExchangeName, MediaType.Exchange_Direct, keyList3);

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
        }


        /// <summary>
        /// Add different handlers for different exchanges
        /// </summary>
        /// <param name="amqpReceiver"></param>
        void AmqpListenerStarted(Object sender, ListenerStartedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _esperRequestExchangeName && (!isListenerStarted))
                {
                    e.AmqpReceiver.AmqpDataReceived += new DataReceived(amqpReceiver_AmqpDataReceived_esper);
                    //This should be called when sender is initialized
                    //SendSecurityObjectsToAmqp();
                }
                else if (e.AmqpReceiver.MediaName == _basketComplianceRequestExchangeName)
                {
                    e.AmqpReceiver.AmqpDataReceived += new DataReceived(amqpReceiver_AmqpDataReceived_basketCompliance);
                }
                else if (e.AmqpReceiver.MediaName == _otherDataExchangeName)
                {
                    e.AmqpReceiver.AmqpDataReceived += AmqpReceiver_AmqpDataReceived_Heartbeat;
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
        }

        /// <summary>
        /// Process the heart beat
        /// </summary>
        /// <param name="dsReceived"></param>
        /// <param name="mediaName"></param>
        /// <param name="mediaType"></param>
        /// <param name="routingKey"></param>
        void AmqpReceiver_AmqpDataReceived_Heartbeat(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                if (e.RoutingKey.Contains("HEARTBEAT"))
                    HeartbeatReceived(e.RoutingKey, e.DsReceived);
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

        void amqpReceiver_AmqpDataReceived_basketCompliance(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                InformationReporter.GetInstance.ComplianceLogWrite("Basket Compliance Engine initialized.");
                if (e.DsReceived.Tables[0].Rows[0]["TypeOfRequest"].ToString() == "InitializationRequestFromBasketCompliance")
                {
                    Dictionary<String, String> completionInfo = new Dictionary<string, string>();
                    try
                    {
                        completionInfo.Add("ResponseType", "InitCompleteTradeServerForBasketCompliance");
                        AmqpHelper.SendObject(completionInfo, "BasketComplianceExchange", "InitCompleteInfo");
                        InformationReporter.GetInstance.ComplianceLogWrite("Initialization completed at Trade Server information sent to Basket Compliance Engine");
                    }
                    catch (Exception ex)
                    {
                        completionInfo.Add("ResponseType", "InitCompleteInfoServerFailedForBasketCompliance");
                        AmqpHelper.SendObject(completionInfo, "BasketComplianceExchange", "InitCompleteInfo");
                        InformationReporter.GetInstance.ComplianceLogWrite("Initialization failed at Trade Server information sent to Basket Compliance Engine");

                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                        if (rethrow)
                            throw;
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

        void amqpReceiver_AmqpDataReceived_esper(Object sender, DataReceivedEventArguments e)
        {
            try
            {

                InformationReporter.GetInstance.ComplianceLogWrite("Esper calculation engine initialized. Now sending information.");
                if (e.DsReceived.Tables[0].Rows[0]["TypeOfRequest"].ToString() == "InitializationRequest")
                {
                    AmqpHelper.InitializeSender("OtherDataSender", _otherDataExchangeName, MediaType.Exchange_Direct);
                    SendSecurityObjectsToAmqp();
                }
                InformationReporter.GetInstance.ComplianceLogWrite("Security objects sent to esper for calculation.");

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
        }

        /// <summary>
        /// Inform the object about the ping
        /// </summary>
        /// <param name="routingkey"></param>
        private void HeartbeatReceived(String routingkey, DataSet ds)
        {
            try
            {
                int interval = Convert.ToInt32(ds.Tables[0].Rows[0]["Interval"]);
                switch (routingkey)
                {
                    case "_EsperHEARTBEAT":
                        _esperConnection.GotAPing(interval);
                        break;
                    case "_RuleMediatorHEARTBEAT":
                        _ruleMediatorConnection.GotAPing(interval);
                        break;
                    case "_BasketComplianceHEARTBEAT":
                        _basketComplianceConnection.GotAPing(interval);
                        break;
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

        internal event EventHandler<EventArgs> SendInTradeData;

        private void SendSecurityObjectsToAmqp()
        {
            Dictionary<String, String> completionInfo = new Dictionary<string, string>();
            try
            {
                InformationReporter.GetInstance.ComplianceLogWrite("Sending security objects to amqp server.");

                List<SecMasterBaseObj> oldSecMasterList = _secMasterServices.GetAllSecMasterData();
                List<SecMasterBaseObj> secMasterList = oldSecMasterList.Select(item => DeepCopyHelper.Clone(item)).ToList();
                // TO-DO : Addition of dynamic UDA should not be here
                Dictionary<string, DynamicUDA> _dynamicUDAcache = _secMasterServices.GetDynamicUDAList();

                secMasterList.ForEach(x =>
                {
                    foreach (string uda in _dynamicUDAcache.Keys)
                    {
                        if (!x.DynamicUDA.ContainsKey(uda))
                            x.DynamicUDA.Add(uda, _dynamicUDAcache[uda].DefaultValue);
                    }
                });

                AmqpHelper.SendObject(CachedDataManager.GetInstance.GetAllCurrencies(), "OtherDataSender", "Currency");

                foreach (SecMasterBaseObj obj in secMasterList)
                {
                    AmqpHelper.SendObject(obj, "OtherDataSender", "Security");
                }

                if (SendInTradeData != null)
                    SendInTradeData(this, new EventArgs());

                completionInfo.Add("ResponseType", "InitCompleteTradeServer");
                AmqpHelper.SendObject(completionInfo, "OtherDataSender", "InitCompleteInfo");
                InformationReporter.GetInstance.ComplianceLogWrite("Security objects sent to amqp server.");

            }
            catch (Exception ex)
            {
                //Dictionary<String, String> completionInfo = new Dictionary<string, string>();
                completionInfo.Add("ResponseType", "InitCompleteInfoServerFailed");
                AmqpHelper.SendObject(completionInfo, "OtherDataSender", "InitCompleteInfo");
                InformationReporter.GetInstance.ComplianceLogWrite("Security objects sent to amqp server.");

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
        /// Sends the Dynamic UDA information to Esper.
        /// </summary>
        /// <param name="dynamicUDAcacheToSend"></param>
        private void SendDynamicUDAInformation(Dictionary<string, DynamicUDA> dynamicUDAcacheToSend)
        {
            try
            {
                DataTable _dynamicUDA = new DataTable();

                DataColumn tagColumn = new DataColumn();
                tagColumn.ColumnName = SecMasterConstants.CONST_TAG;
                tagColumn.Caption = SecMasterConstants.CONST_TAG;
                tagColumn.DataType = typeof(String);
                _dynamicUDA.Columns.Add(tagColumn);

                DataColumn headerCaptionColumn = new DataColumn();
                headerCaptionColumn.ColumnName = SecMasterConstants.CONST_HEADERCAPTION;
                headerCaptionColumn.Caption = SecMasterConstants.CONST_CAPTION_HEADERCAPTION;
                headerCaptionColumn.DataType = typeof(String);
                _dynamicUDA.Columns.Add(headerCaptionColumn);

                DataColumn defaultValueColumn = new DataColumn();
                defaultValueColumn.ColumnName = SecMasterConstants.CONST_DEFAULTVALUE;
                defaultValueColumn.Caption = SecMasterConstants.CONST_CAPTION_DEFAULTVALUE;
                defaultValueColumn.DataType = typeof(String);
                _dynamicUDA.Columns.Add(defaultValueColumn);

                List<string> sortedKeys = dynamicUDAcacheToSend.Keys.ToList();
                sortedKeys.Sort((x, y) =>
                {
                    if (x.Contains("CustomUDA") && y.Contains("CustomUDA"))
                    {
                        int xNum = int.Parse(Regex.Match(x, @"\d+").Value);
                        int yNum = int.Parse(Regex.Match(y, @"\d+").Value);
                        return xNum.CompareTo(yNum);
                    }
                    else
                    {
                        return x.CompareTo(y);
                    }
                });
                foreach (string key in sortedKeys)
                {
                    _dynamicUDA.Rows.Add(key, dynamicUDAcacheToSend[key].HeaderCaption, dynamicUDAcacheToSend[key].DefaultValue);
                }

                AmqpHelper.SendObject(_dynamicUDA, "OtherDataSender", "DynamicUDACache");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        internal void Close()
        {
            try
            {
                AmqpHelper.Started -= new ListenerStarted(AmqpListenerStarted);
                AmqpHelper.CloseAllConnection();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}