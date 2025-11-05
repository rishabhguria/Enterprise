using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.SocketCommunication;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;


namespace Prana.PostTradeServices
{
    public class PricingSeverClient : IPricingAnalysis, IDisposable
    {



        private ICommunicationManager _clientCommunicationManager = null;


        public ICommunicationManager ClientCommunicationManager
        {
            get { return _clientCommunicationManager; }
            set
            {
                _clientCommunicationManager = value;
                WireEvents();
            }
        }


        PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        public event EventHandler Disconnected;
        public event EventHandler Connected;
        delegate void AsyncInvokeDelegate(Delegate del, params object[] args);

        #region IPricingAnalysis Members

        public event EventHandler<EventArgs<ResponseObj>> GreeksCalculated;

        public event EventHandler<EventArgs<List<StepAnalysisResponse>>> StepAnalysisCompleted;

        public PranaInternalConstants.ConnectionStatus ConnectionStatus
        {
            get
            {
                if (_clientCommunicationManager == null)
                    return PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                else
                    return _clientCommunicationManager.ConnectionStatus;
            }
        }
        int _hashCode = 0;

        private void WireEvents()
        {
            _clientCommunicationManager.MessageReceived += new MessageReceivedDelegate(_clientCommunicationManager_MessageReceived);
            _clientCommunicationManager.Disconnected += new EventHandler(_clientCommunicationManager_Disconnected);
            _clientCommunicationManager.Connected += new EventHandler(_clientCommunicationManager_Connected);
        }


        public void ConnectToServer(ConnectionProperties connProp)
        {
            try
            {
                if (_clientCommunicationManager == null)
                {
                    _clientCommunicationManager = new ClientTradeCommManager();


                    _clientCommunicationManager.IsMonitoringConnection = true;
                    //Wireup for receiving OrderMessages
                    _clientCommunicationManager.MessageReceived += new MessageReceivedDelegate(_clientCommunicationManager_MessageReceived);
                    _clientCommunicationManager.Disconnected += new EventHandler(_clientCommunicationManager_Disconnected);
                    _clientCommunicationManager.Connected += new EventHandler(_clientCommunicationManager_Connected);
                }
                if (_clientCommunicationManager.ConnectionStatus != PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    _clientCommunicationManager.Connect(connProp);
                }
            }


            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                //release
                if (_clientCommunicationManager != null)
                {
                    _clientCommunicationManager.DisConnect();
                    _clientCommunicationManager = null;
                }
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void DisConnect()
        {
            if (_clientCommunicationManager != null && _clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                _clientCommunicationManager.DisConnect();
            }
        }

        public void SendSnapShotRequest(InputParametersCollection inputParametersCollection, int userID)
        {

            try
            {
                inputParametersCollection.UserID = userID.ToString();
                string request = binaryFormatter.Serialize(inputParametersCollection);
                QueueMessage qMsg = new QueueMessage(OptionDataFormatter.MSGTYPE_SnapShotData, request);
                _clientCommunicationManager.SendMessage(qMsg);
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

        public void SendStepAnalRequest(InputParametersCollection inputParametersCollection, int userID)
        {
            try
            {
                inputParametersCollection.UserID = userID.ToString();
                string request = binaryFormatter.Serialize(inputParametersCollection);
                QueueMessage qMsg = new QueueMessage(OptionDataFormatter.MSGTYPE_OptionStelAnalData, request);
                _clientCommunicationManager.SendMessage(qMsg);
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

        public void SendRequest(string msgType)
        {
            try
            {
                string request = String.Empty;

                switch (msgType)
                {
                    case OptionDataFormatter.MSGTYPE_PREFS_REFRESH:
                        request = binaryFormatter.Serialize(RiskPreferenceManager.RiskPrefernece);
                        _clientCommunicationManager.SendMessage(new QueueMessage(OptionDataFormatter.MSGTYPE_PREFS_REFRESH, request));
                        break;

                    case OptionDataFormatter.MSGTYPE_OMI_UPDATED:
                        _clientCommunicationManager.SendMessage(new QueueMessage(OptionDataFormatter.MSGTYPE_OMI_UPDATED, request));
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

        public void SendRequest(string underlyingSymbol, string optSymbol, CompanyUser currentUser)
        {

            try
            {
                Dictionary<string, List<string>> symbolToBeRequestedDict = new Dictionary<string, List<string>>();
                List<string> optionSymbolList = new List<string>();
                optionSymbolList.Add(optSymbol);
                symbolToBeRequestedDict.Add(underlyingSymbol, optionSymbolList);
                string request = binaryFormatter.Serialize(symbolToBeRequestedDict);
                QueueMessage qMsg = new QueueMessage(OptionDataFormatter.MSGTYPE_SUBSCRIBE_SYMBOLS, request);
                qMsg.RequestID = "";
                qMsg.UserID = currentUser.CompanyUserID.ToString();
                qMsg.HashCode = _hashCode;
                _clientCommunicationManager.SendMessage(qMsg);
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

        private void InvokeDelegate(Delegate sink, params object[] args)
        {
            try
            {
                sink.DynamicInvoke(sink, args[0]);
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

        void _clientCommunicationManager_Disconnected(object sender, EventArgs e)
        {
            if (Disconnected != null)
            {
                Disconnected(sender, e);
            }
        }
        void _clientCommunicationManager_Connected(object sender, EventArgs e)
        {
            if (Connected != null)
            {
                Connected(sender, e);
            }
        }
        void _clientCommunicationManager_MessageReceived(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                QueueMessage message = e.Value;
                switch (message.MsgType)
                {
                    case OptionDataFormatter.MSGTYPE_SnapShotData:

                        ResponseObj optionResponseObj = (ResponseObj)binaryFormatter.DeSerialize(message.Message.ToString());
                        if (GreeksCalculated != null)
                        {
                            AsyncInvokeDelegate invoker = new AsyncInvokeDelegate(InvokeDelegate);
                            Delegate[] subscriberList = GreeksCalculated.GetInvocationList();
                            foreach (Delegate subscriber in subscriberList)
                            {
                                int subscriberHashCode = subscriber.Target.GetHashCode();
                                if (subscriberHashCode == message.HashCode)
                                {
                                    invoker.BeginInvoke(subscriber, new object[1] { new EventArgs<ResponseObj>(optionResponseObj) }, null, null);
                                    break;
                                }
                            }
                        }
                        break;
                    case OptionDataFormatter.MSGTYPE_OptionStelAnalData:
                        List<StepAnalysisResponse> stepRes = (List<StepAnalysisResponse>)binaryFormatter.DeSerialize(message.Message.ToString());
                        if (StepAnalysisCompleted != null)
                        {
                            AsyncInvokeDelegate invoker = new AsyncInvokeDelegate(InvokeDelegate);
                            Delegate[] subscriberList = StepAnalysisCompleted.GetInvocationList();
                            foreach (Delegate subscriber in subscriberList)
                            {
                                int subscriberHashCode = subscriber.Target.GetHashCode();
                                if (subscriberHashCode == message.HashCode)
                                {
                                    invoker.BeginInvoke(subscriber, new object[1] { new EventArgs<List<StepAnalysisResponse>>(stepRes) }, null, null);
                                    break;
                                }
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

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _clientCommunicationManager = null;
            }
        }

        #endregion
    }

}
