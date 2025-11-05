using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.SocketCommunication;
using Prana.Utilities.MiscUtilities;
using System;
using System.Data;
using System.Diagnostics;

namespace Prana.SecurityValidationService
{
    internal class SecMasterClientNew : IDisposable
    {
        ClientTradeCommManager _clientCommunicationManager = null;

        PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        private int _hashCode;
        public event EventHandler Disconnected;

        public event EventHandler Connected;

        public SecMasterClientNew()
        {
            _hashCode = this.GetHashCode();
        }

        void _clientCommunicationManager_ResponseCompleted(object sender, EventArgs<QueueMessage> e)
        {
            if (ResponseCompleted != null)
            {
                ResponseCompleted(sender, e);
            }
        }

        void _clientCommunicationManager_MessageReceived(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                Prana.BusinessObjects.QueueMessage message = e.Value;
                switch (message.MsgType)
                {
                    case CustomFIXConstants.MSG_SECMASTER_SYMBOLSEARCH_RESPONSE:
                        SecMasterSymbolSearchRes searchResult = (SecMasterSymbolSearchRes)binaryFormatter.DeSerialize(message.Message.ToString());
                        SecMstrDataSymbolSearcResponse(sender, searchResult);
                        break;
                    //handling on SM data recieved from sm server for vadidation
                    case CustomFIXConstants.MSG_SECMASTER_RESPONSE:
                        SecMasterBaseObj secMasterRes = (SecMasterBaseObj)binaryFormatter.DeSerialize(message.Message.ToString());
                        Logger.LoggerWrite("Received Security Master Response from Server at Security Valition Service for symbol: " + secMasterRes.TickerSymbol, LoggingConstants.CATEGORY_GENERAL);
                        SecMstrDataResponse(sender, secMasterRes);
                        break;
                    //handling on SM data recieved from sm server for symbol lookup UI 
                    case CustomFIXConstants.MSG_SECMASTER_SymbolRESPONSE:
                        DataSet ds = (DataSet)binaryFormatter.DeSerialize(message.Message.ToString());
                        if (SymbolLookUpDataResponse != null)
                        {
                            SymbolLookUpDataResponse(sender, new EventArgs<DataSet>(ds));
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

        public void ConnectToServer()
        {
            try
            {
                var sw = Stopwatch.StartNew();
                if (_clientCommunicationManager == null || _clientCommunicationManager.ConnectionStatus != PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    //connect to server 
                    _clientCommunicationManager = new ClientTradeCommManager();
                    _clientCommunicationManager.IsMonitoringConnection = false;
                    //Wireup for receiving OrderMessages
                    _clientCommunicationManager.MessageReceived += new Prana.Interfaces.MessageReceivedDelegate(_clientCommunicationManager_MessageReceived);
                    _clientCommunicationManager.ResponseCompleted += new ClientTradeCommManager.CompletedReceivedDelegate(_clientCommunicationManager_ResponseCompleted);
                    _clientCommunicationManager.Disconnected += new EventHandler(_clientCommunicationManager_Disconnected);
                    _clientCommunicationManager.Connected += new EventHandler(_clientCommunicationManager_Connected);
                    ConnectionProperties connProperties = new ConnectionProperties();
                    connProperties.Port = ClientAppConfiguration.TradeServer.Port;
                    connProperties.ServerIPAddress = ClientAppConfiguration.TradeServer.IpAddress;
                    connProperties.IdentifierID = "SecValidService";
                    connProperties.IdentifierName = "Sec Validation Service";
                    connProperties.ConnectedServerName = "Security Master Server";
                    connProperties.HandlerType = HandlerType.SecurityMasterHandler;
                    if (_clientCommunicationManager.Connect(connProperties) == PranaInternalConstants.ConnectionStatus.CONNECTED)
                    {
                        _isConnected = true;
                    }
                    else
                    {
                        _isConnected = false;
                    }
                    Logger.LogMsg(LoggerLevel.Information, "ClientCommuncationManager connection establised status {0} in {1} ms", 
                        IsConnected, 
                        sw.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"ConnectToServer encountered an error");
                //release
                _clientCommunicationManager.DisConnect();
                _clientCommunicationManager = null;
            }
        }

        void _clientCommunicationManager_Connected(object sender, EventArgs e)
        {
            _isConnected = true;
            if (Connected != null)
            {
                Connected(sender, e);
            }

        }

        void _clientCommunicationManager_Disconnected(object sender, EventArgs e)
        {
            _isConnected = false;
            if (Disconnected != null)
            {
                Disconnected(sender, e);
            }
        }

        //public event CompletedReceivedDelegate ResponseCompleted;
        public event EventHandler<EventArgs<QueueMessage>> ResponseCompleted;

        public void GetSymbolLookupRequestedData(SymbolLookupRequestObject symbolLookupReqObj)
        {
            try
            {
                if (_clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    symbolLookupReqObj.CompanyUserID = "SecValidService";
                    string request = binaryFormatter.Serialize(symbolLookupReqObj);
                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_SymbolREQ, request);
                    if (qMsg.HashCode == int.MinValue || qMsg.HashCode == 0)
                    {
                        qMsg.HashCode = _hashCode;
                    }
                    qMsg.RequestID = symbolLookupReqObj.RequestID;
                    _clientCommunicationManager.SendMessage(qMsg);
                }
                else
                {
                    if (Disconnected != null)
                    {
                        Disconnected(this, EventArgs.Empty);
                    }
                    Logger.LogMsg(LoggerLevel.Fatal, "_clientCommunicationManager is disconnected, for GetSymbolLookupRequestedData:{0}",
                        symbolLookupReqObj.TickerSymbol);
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

        public void SendRequest(SecMasterRequestObj secMasterReqObj)
        {
            try
            {
                if (secMasterReqObj.IsRequestValid())
                {
                    if (_clientCommunicationManager != null && _clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                    {
                        // so that request sent is received by the same user
                        if (secMasterReqObj.HashCode == int.MinValue || secMasterReqObj.HashCode == 0)
                        {
                            secMasterReqObj.HashCode = _hashCode;
                        }

                        string request = binaryFormatter.Serialize(secMasterReqObj);
                        QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_REQ, request);
                        _clientCommunicationManager.SendMessage(qMsg);
                    }
                    else
                    {
                        Logger.LogMsg(LoggerLevel.Fatal, "Client is trying to validate symbol, {0}, and waiting for the response because server is not connected",
                            secMasterReqObj.GetPrimarySymbols()[0]);
                        
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

        public void SaveNewSymbols(SecMasterbaseList lst)
        {
            try
            {
                if (_clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    lst.UserID = "SecValidService";
                    lst.RequestID = "SecValidService";
                    string request = binaryFormatter.Serialize(lst);
                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_SaveREQ, request);
                    if (qMsg.HashCode == int.MinValue || qMsg.HashCode == 0)
                    {
                        qMsg.HashCode = _hashCode;
                    }
                    qMsg.RequestID = lst.RequestID;
                    _clientCommunicationManager.SendMessage(qMsg);
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

        public void searchSymbols(SecMasterSymbolSearchReq req)
        {
            try
            {
                if (req.HashCode == int.MinValue || req.HashCode == 0)
                {
                    req.HashCode = _hashCode;
                }

                string request = binaryFormatter.Serialize(req);
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_SYMBOLSEARCH_REQ, request);
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

        //public event SecMasterDataHandler SecMstrDataResponse;
        public event EventHandler<SecMasterBaseObj> SecMstrDataResponse;

        //public event SecMasterSymbolSearchHandler SecMstrDataSymbolSearcResponse;
        public event EventHandler<SecMasterSymbolSearchRes> SecMstrDataSymbolSearcResponse;

        public event EventHandler<EventArgs<DataSet>> SymbolLookUpDataResponse;

        bool _isConnected = false;

        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
            }
        }

        public void DisConnect()
        {
            try
            {
                _clientCommunicationManager.DisConnect();
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

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _clientCommunicationManager.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}



