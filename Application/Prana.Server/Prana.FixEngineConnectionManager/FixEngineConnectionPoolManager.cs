using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.CustomMapper;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.QueueManager;
using Prana.ServerCommon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using static Prana.BusinessObjects.PranaServerConstants;

namespace Prana.FixEngineConnectionManager
{
    public class FixEngineConnectionPoolManager : IDisposable
    {
        #region Variables
        private static FixEngineConnectionPoolManager _fixEngineConnectionPoolManager;
        private ITradeQueueProcessor _inFixBuyQueue;
        private ITradeQueueProcessor _outFIXCONMgrQueue;
        private IQueueProcessor _connectionUnAvailableQueue;
        private IQueueProcessor _cpsentQueue;
        private IQueueProcessor _cpReceivedQueue;
        private IFixEngineAdapter _defaultfixAdapter = null;

        private static Dictionary<int, Dictionary<int, FixEngineConnection>> _fixBuyConnectionPool = new Dictionary<int, Dictionary<int, FixEngineConnection>>();
        private Dictionary<int, FixPartyDetails> _dictFixPartyDetails = new Dictionary<int, FixPartyDetails>();

        private Dictionary<int, int> _dictOMSConnectionMapping = new Dictionary<int, int>();
        private Dictionary<int, int> _dictEODConnectionMapping = new Dictionary<int, int>();

        private bool _enableTradeFlowLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableTradeFlowLogging"));
        private bool _isTroubleShootMode = false;
        private bool _isDisposed = false;

        private object lockOnConnectionPool = new object();

        public event EventHandler<EventArgs<FixPartyDetails>> ConnectionStatusUpdate;
        #endregion

        #region Public Properties
        private bool _autoReconnect = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsAutoReconnectFixConnection"));
        public bool AutoReconnect
        {
            get { return _autoReconnect; }
            set { _autoReconnect = value; }
        }
        #endregion

        #region Constructor
        static FixEngineConnectionPoolManager()
        {
            _fixEngineConnectionPoolManager = new FixEngineConnectionPoolManager();
        }

        public static FixEngineConnectionPoolManager GetInstance()
        {
            return _fixEngineConnectionPoolManager;
        }
        #endregion

        #region Events
        void fixBuyConnection_MessageReceived(object sender, EventArgs<PranaMessage, FixEngineConnection> e)
        {
            try
            {
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow4] Before SendMessage In FixEngineConnectionPoolManager, Fix Message: " + Convert.ToString(e.Value.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                    }
                }

                if (!e.Value.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OrderSeqNumber))
                {
                    string orderIDSeqNumber = UniqueIDGenerator.GetOrderSeqNumber();
                    e.Value.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber, orderIDSeqNumber);
                }
                // assign Received Time Local---
                //omshiv, march 2014, reverting back code (removing HHmmss)
                if (!e.Value.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ServerReceivedTime))
                {
                    e.Value.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ServerReceivedTime, DateTime.Now.ToString("yyyyMMdd"));
                }

                //omshiv, March 2014, Added new custom tag to check actual fill recieved time - CUST_TAG_ServerReceivedFullTime
                if (!e.Value.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ServerReceivedFullTime))
                {
                    e.Value.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ServerReceivedFullTime, DateTime.Now.ToString("yyyyMMddHHmmss"));
                }

                //logger.Info("OrderSeqNumber Used=" + orderIDSeqNumber);
                if (e.Value.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTransactTime))
                {
                    if (e.Value.FIXMessage.ExternalInformation[FIXConstants.TagTransactTime].Value.Length > 17)
                    {
                        e.Value.FIXMessage.ExternalInformation[FIXConstants.TagTransactTime].Value =
                        e.Value.FIXMessage.ExternalInformation[FIXConstants.TagTransactTime].Value.Substring(0, 17);
                    }
                }
                if (e.Value.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExpireTime))
                {
                    if (e.Value.FIXMessage.ExternalInformation[FIXConstants.TagExpireTime].Value.Length > 17)
                    {
                        e.Value.FIXMessage.ExternalInformation[FIXConstants.TagExpireTime].Value =
                        e.Value.FIXMessage.ExternalInformation[FIXConstants.TagExpireTime].Value.Substring(0, 17);
                    }
                }
                if (e.Value.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSendingTime))
                {
                    if (e.Value.FIXMessage.ExternalInformation[FIXConstants.TagSendingTime].Value.Length > 17)
                    {
                        e.Value.FIXMessage.ExternalInformation[FIXConstants.TagSendingTime].Value =
                            e.Value.FIXMessage.ExternalInformation[FIXConstants.TagSendingTime].Value.Substring(0, 17);
                    }
                }
                switch (e.Value2.FixPartyDetails.OriginatorType)
                {
                    case PranaServerConstants.OriginatorType.BuySide:
                    case PranaServerConstants.OriginatorType.Allocation:
                        e.Value.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrigCounterPartyID, e.Value2.FixPartyDetails.PartyID.ToString());
                        e.Value.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CounterPartyName, e.Value2.FixPartyDetails.PartyName.ToString());
                        break;
                    default:
                        e.Value.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_Symbology, e.Value2.FixPartyDetails.Symbology.ToString());
                        e.Value.MessageType = CustomFIXConstants.MsgDropCopyReceived;
                        e.Value.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CounterPartyID, e.Value2.FixPartyDetails.PartyID.ToString());
                        e.Value.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrigCounterPartyID, e.Value2.FixPartyDetails.PartyID.ToString());
                        e.Value.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OriginatorType, ((int)e.Value2.FixPartyDetails.OriginatorType).ToString());
                        break;
                }
                _inFixBuyQueue.SendMessage(e.Value);
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow4] After SendMessage In FixEngineConnectionPoolManager, Fix Message: " + Convert.ToString(e.Value.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Receives Messages from OutFixQueue and Sends them to Respective CounterParties
        /// if Connection is Down or Not Available it sends them for Queuing 
        /// </summary>
        void outFIXCONMgrQueue_MessageQueued(object sender, EventArgs<PranaMessage> e)
        {
            try
            {
                PranaMessage pranaMessage = e.Value;
                int counterPartyID = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value);

                OriginatorTypeCategory originatorTypeCategory = OriginatorTypeCategory.OMS;
                if (pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value.ToString().Equals(FIXConstants.MSGAllocation)
                    || pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value.ToString().Equals(FIXConstants.MSGConfirmationAck)
                    || pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value.ToString().Equals(FIXConstants.MSGAllocationReportAck))
                {
                    originatorTypeCategory = OriginatorTypeCategory.EOD;
                }

                FixEngineConnection fixConnection = GetFixConnection(counterPartyID, originatorTypeCategory);
                if (fixConnection != null)
                {
                    if (_enableTradeFlowLogging)
                    {
                        try
                        {
                            Logger.LoggerWrite("[Trade-Flow Out3] SendMessage In _outFIXCONMgrQueue_MessageQueued In FixEngineConnectionPoolManager, Fix Message: " + Convert.ToString(pranaMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                        }
                    }
                    string result = fixConnection.SendMessage(pranaMessage);
                    if (result == string.Empty)
                    {

                    }
                    else if (result == PranaInternalConstants.ConnectionStatus.DISCONNECTED.ToString())
                    {
                        SendMessageBack(pranaMessage, CustomFIXConstants.MSG_CounterPartyDown);
                    }
                    else
                    {
                        SendMessageBack(pranaMessage, CustomFIXConstants.MSG_CounterPartySendingProblem);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Sends Connection Update to Respective Subscriber
        /// </summary>
        void fixconnection_ConnectionStatusUpdate(object sender, EventArgs<FixPartyDetails> e)
        {
            try
            {
                if (e.Value.PartyID != int.MinValue)
                {
                    if (ConnectionStatusUpdate != null)
                    {
                        foreach (KeyValuePair<int, FixPartyDetails> fixdetails in _dictFixPartyDetails)
                        {
                            if (fixdetails.Value.Port == e.Value.Port)
                            {
                                fixdetails.Value.SetConnectionValues(e.Value);
                                IAsyncResult result = ConnectionStatusUpdate.BeginInvoke(this, new EventArgs<FixPartyDetails>(fixdetails.Value), null, null);
                                ConnectionStatusUpdate.EndInvoke(result);
                            }
                        }
                        if (e.Value.BuyToSellSideStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            PranaMessage message = CreateCounterPartyUpMsg(e.Value.PartyID.ToString());
                            QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_CounterPartyUp, "", "", message);
                            _connectionUnAvailableQueue.SendMessage(qMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion

        public void Initilise(ITradeQueueProcessor inFixBuyQueue, ITradeQueueProcessor outFixMgrQueue, int CompanyID)
        {
            try
            {
                _outFIXCONMgrQueue = outFixMgrQueue;
                _outFIXCONMgrQueue.MessageQueued += new EventHandler<EventArgs<PranaMessage>>(outFIXCONMgrQueue_MessageQueued);
                _inFixBuyQueue = inFixBuyQueue;
                _connectionUnAvailableQueue = new MSMQQueueManager(ConfigurationManager.AppSettings[PranaServerConstants.CONNECTION_UNAVAILABLE_PATH].ToString() + "_" + CompanyID.ToString());
                _cpsentQueue = new MSMQQueueManager(ConfigurationManager.AppSettings[PranaServerConstants.CP_SENT_MSGS_PATH].ToString() + "_" + CompanyID.ToString());
                _cpReceivedQueue = new MSMQQueueManager(ConfigurationManager.AppSettings[PranaServerConstants.CP_RECEIVED_MSGS_PATH].ToString() + "_" + CompanyID.ToString());
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

        public void ConnectAllBuySides(Dictionary<int, FixPartyDetails> dictFixPartyDetails, bool isTroubleShootModeStart)
        {
            try
            {
                _dictFixPartyDetails = dictFixPartyDetails;
                _isTroubleShootMode = isTroubleShootModeStart;

                foreach (KeyValuePair<int, FixPartyDetails> party in dictFixPartyDetails)
                {
                    if (party.Value.OriginatorType == PranaServerConstants.OriginatorType.Allocation)
                    {
                        _dictEODConnectionMapping.Add(party.Value.PartyID, party.Value.ConnectionID);
                    }
                    else
                    {
                        _dictOMSConnectionMapping.Add(party.Value.PartyID, party.Value.ConnectionID);
                    }

                    FixEngineConnection fixBuyConnection = new FixEngineConnection(_cpReceivedQueue, _cpsentQueue, party.Value, _isTroubleShootMode);
                    if (!_fixBuyConnectionPool.ContainsKey(party.Value.Port))
                    {
                        Dictionary<int, FixEngineConnection> innerDict = new Dictionary<int, FixEngineConnection>();
                        innerDict.Add(party.Value.ConnectionID, fixBuyConnection);

                        _fixBuyConnectionPool.Add(party.Value.Port, innerDict);
                    }
                    else
                    {
                        if (!_fixBuyConnectionPool[party.Value.Port].ContainsKey(party.Value.ConnectionID))
                        {
                            _fixBuyConnectionPool[party.Value.Port].Add(party.Value.ConnectionID, fixBuyConnection);
                        }
                    }
                }

                foreach (int port in _fixBuyConnectionPool.Keys)
                {
                    try
                    {
                        ConnectBuySide(port);
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public void ConnectBuySide(int port)
        {
            try
            {
                lock (lockOnConnectionPool)
                {
                    bool alredyConnectedToPort = false;
                    if (_fixBuyConnectionPool.Count > 0 && _fixBuyConnectionPool.ContainsKey(port))
                    {
                        // here I have used for loop rather foreach to avoid errors of "collection modification".
                        List<int> fixBuyConnectionPoolKeysList = _fixBuyConnectionPool[port].Keys.ToList();
                        for (int i = 0; i < fixBuyConnectionPoolKeysList.Count; i++)
                        {
                            int connectionID = fixBuyConnectionPoolKeysList[i];
                            FixPartyDetails fixdetails = _dictFixPartyDetails[connectionID];

                            FixEngineConnection fixBuyConnection = new FixEngineConnection(_cpReceivedQueue, _cpsentQueue, fixdetails, _isTroubleShootMode);

                            //// counterparty must have been disconneted
                            _fixBuyConnectionPool[port][connectionID] = fixBuyConnection;

                            //wire up events and connect
                            fixBuyConnection.ConnectionStatusUpdate += new EventHandler<EventArgs<FixPartyDetails>>(fixconnection_ConnectionStatusUpdate);

                            if (alredyConnectedToPort)
                            {
                                fixBuyConnection.ConnectToSamePort(_fixBuyConnectionPool[port].FirstOrDefault().Value);
                            }
                            if (!alredyConnectedToPort)
                            {
                                fixBuyConnection.MessageReceived += new EventHandler<EventArgs<PranaMessage, FixEngineConnection>>(fixBuyConnection_MessageReceived);

                                IFixEngineAdapter fixConnAdapter = CreateFixAdapterDll(fixdetails.FixDllName);
                                fixBuyConnection.Connect(fixConnAdapter);
                            }
                            alredyConnectedToPort = true;
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

        public void DisconnectAllBuySides()
        {
            try
            {
                foreach (KeyValuePair<int, FixPartyDetails> party in _dictFixPartyDetails)
                {
                    try
                    {
                        DisconnectBuySide(party.Key);
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public void DisconnectBuySide(int connectionID)
        {
            try
            {
                lock (lockOnConnectionPool)
                {
                    int port = _dictFixPartyDetails[connectionID].Port;
                    if (_fixBuyConnectionPool.ContainsKey(port))
                    {
                        List<int> connectionIDKeys = _fixBuyConnectionPool[port].Keys.ToList();
                        foreach (int connectionIDKey in connectionIDKeys)
                        {
                            _fixBuyConnectionPool[port][connectionIDKey].Disconnect();
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

        public int GetPortForParty(int connectionID)
        {
            try
            {
                if (_dictFixPartyDetails != null && _dictFixPartyDetails.ContainsKey(connectionID))
                    return _dictFixPartyDetails[connectionID].Port;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }

        private int GetConnectionIDFromCounterPartyID(int counterPartyID, OriginatorTypeCategory originatorTypeCategory = OriginatorTypeCategory.OMS)
        {
            int connectionID = 0;

            if (originatorTypeCategory == OriginatorTypeCategory.EOD)
            {
                if (_dictEODConnectionMapping.ContainsKey(counterPartyID))
                {
                    connectionID = _dictEODConnectionMapping[counterPartyID];
                }
            }
            else
            {
                if (_dictOMSConnectionMapping.ContainsKey(counterPartyID))
                {
                    connectionID = _dictOMSConnectionMapping[counterPartyID];
                }
            }
            return connectionID;
        }

        public FixEngineConnection GetFixConnection(int counterPartyID, OriginatorTypeCategory originatorTypeCategory = OriginatorTypeCategory.OMS)
        {
            int connectionID = GetConnectionIDFromCounterPartyID(counterPartyID, originatorTypeCategory);

            if (connectionID > 0)
            {
                if (_dictFixPartyDetails.ContainsKey(connectionID) && _fixBuyConnectionPool.ContainsKey(_dictFixPartyDetails[connectionID].Port) && _fixBuyConnectionPool[_dictFixPartyDetails[connectionID].Port].ContainsKey(connectionID))
                {
                    return _fixBuyConnectionPool[_dictFixPartyDetails[connectionID].Port][connectionID];
                }
            }
            return null;
        }

        public Dictionary<int, FixPartyDetails> GetAllFixConnections()
        {
            return _dictFixPartyDetails;
        }

        public void SetFixConnectionsAutoReconnectStatus(bool autoConnectStatus)
        {
            this._autoReconnect = autoConnectStatus;

            if (autoConnectStatus)
            {
                foreach (KeyValuePair<int, FixPartyDetails> party in _dictFixPartyDetails)
                {
                    if (party.Value.BuySideStatus != PranaInternalConstants.ConnectionStatus.CONNECTED)
                    {
                        ConnectBuySide(GetPortForParty(party.Key));
                    }
                }
            }
        }

        private IFixEngineAdapter CreateFixAdapterDll(string name)
        {
            try
            {
                string[] data = name.Split(',');
                Assembly asmAssemblyContainingForm = Assembly.LoadFrom(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/PluggableDlls/" + data[0]);
                Type typeToLoad = asmAssemblyContainingForm.GetType(data[1]);
                IFixEngineAdapter fixAdapter = (IFixEngineAdapter)Activator.CreateInstance(typeToLoad);
                _defaultfixAdapter = fixAdapter;
                return fixAdapter;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                throw new Exception("Problem in creating Fix Adapters");
            }
        }

        public PranaMessage CreateCounterPartyUpMsg(string cpId)
        {
            PranaMessage msg = new PranaMessage(CustomFIXConstants.MSG_CounterPartyUp, int.MinValue);
            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, PranaServerConstants.COUNTERPARTY_UP);
            msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CounterPartyID, cpId);
            return msg;
        }

        public PranaMessage CreatePranaMessageFromFixMessage(string message)
        {
            return _defaultfixAdapter.CreatePranaMessageFromFixMessage(message);
        }

        public void GetMessageStatus(Order order)
        {
            try
            {
                PranaMessage msg = new PranaMessage();
                msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGOrderStatusRequest);
                msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagSymbol, order.Symbol);
                msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, order.ClOrderID);
                msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagSymbol, order.Symbol);
                msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagSide, order.OrderSideTagValue);

                int connectionID = GetConnectionIDFromCounterPartyID(order.CounterPartyID);
                _fixBuyConnectionPool[_dictFixPartyDetails[connectionID].Port][connectionID].SendMessage(msg);
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
        /// Resend Request for Missing Orders
        /// </summary>
        public void ResendRequest()
        {
            try
            {
                foreach (KeyValuePair<int, FixPartyDetails> party in _dictFixPartyDetails)
                {
                    if (party.Value.LastSeqNumberRecevied != Int64.MinValue)
                    {
                        if (_dictFixPartyDetails.ContainsKey(party.Key))
                        {
                            _fixBuyConnectionPool[_dictFixPartyDetails[party.Key].Port][party.Key].ResendRequest(party.Value.LastSeqNumberRecevied + 1, 0, _dictFixPartyDetails[party.Key].PartyName);
                        }
                        else
                        {
                            throw new Exception("CounterPary not connected");
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
        /// Sends Failed Messages for Queqing
        /// </summary>
        private void SendMessageBack(PranaMessage pranaMsg, string msgType)
        {
            try
            {
                Logger.HandleException(new Exception("Message Could not be Sent to CounterParty " + pranaMsg.ToString()), LoggingConstants.POLICY_LOGANDSHOW);
                QueueMessage persistedMsg = new QueueMessage(msgType, "", "", pranaMsg);
                _connectionUnAvailableQueue.SendMessage(persistedMsg);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Re Process trades stuck on server 
        /// </summary>
        /// <param name="pranaMsgList"></param>
        public void ReProcessMsg(List<PranaMessage> pranaMsgList)
        {
            //Modify By omshiv, Please check containskey when you are using Dict and you are not sure, its throwing error
            try
            {
                //get Counter party id from prana msg and process trade on that CP connection
                foreach (PranaMessage pranaMsg in pranaMsgList)
                {
                    pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_IsProcessed, "1");

                    int connectionID = GetConnectionIDFromCounterPartyID(int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OrigCounterPartyID].Value));
                    if (_fixBuyConnectionPool.ContainsKey(_dictFixPartyDetails[connectionID].Port) && _fixBuyConnectionPool[_dictFixPartyDetails[connectionID].Port].ContainsKey(connectionID))
                    {
                        _fixBuyConnectionPool[_dictFixPartyDetails[connectionID].Port][connectionID].ReProcessMsg(pranaMsg);
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

        /// <summary>
        /// Sends the manual orders to third party fix line.
        /// </summary>
        /// <param name="pranaMsgs">The prana MSGS.</param>
        public void SendManualOrdersToThirdPartyFixLine(List<PranaMessage> pranaMsgs)
        {
            try
            {
                FixEngineConnection fixConnection = GetFixConnection(-1);
                if (fixConnection != null)
                {
                    pranaMsgs.ForEach(pMsg =>
                    {
                        PranaCustomMapper.ApplyRulesForManualOrderSend(pMsg);
                        FixMessageValidator.ValidateMessage(pMsg);
                        if (_enableTradeFlowLogging)
                        {
                            try
                            {
                                Logger.HandleException(new Exception("[Trade-Flow Out3] SendMessage In SendManualOrdersToThirdPartyFixLine In FixEngineConnectionPoolManager, Fix Message: " + Convert.ToString(pMsg.FIXMessage.ExternalInformation)), LoggingConstants.POLICY_LOGANDSHOW);
                            }
                            catch (Exception ex)
                            {
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                            }
                        }
                        string result = fixConnection.SendMessage(pMsg);
                        if (result == string.Empty)
                        {

                        }
                        else if (result == PranaInternalConstants.ConnectionStatus.DISCONNECTED.ToString())
                        {
                            SendMessageBack(pMsg, CustomFIXConstants.MSG_CounterPartyDown);
                        }
                        else
                        {
                            SendMessageBack(pMsg, CustomFIXConstants.MSG_CounterPartySendingProblem);
                        }
                    });
                    Logger.HandleException(new Exception("Manual Droops has processed."), LoggingConstants.POLICY_LOGANDSHOW);
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

        #region IDisposal Methods
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    foreach (KeyValuePair<int, Dictionary<int, FixEngineConnection>> fixbuycon in _fixBuyConnectionPool)
                    {
                        foreach (KeyValuePair<int, FixEngineConnection> fixbuyconn in fixbuycon.Value)
                        {
                            fixbuyconn.Value.Dispose();
                        }
                    }
                }
                _isDisposed = true;
            }
        }
        #endregion
    }
}