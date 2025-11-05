using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.NewLiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SMObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.SecurityMasterNew.BLL;
using Prana.SocketCommunication;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace Prana.SecurityMasterNew
{
    /// <summary>
    /// Client side implementation of ISecurityMasterServices which sends messages to the SecMasterServerComponent
    /// </summary>
    public class SecMasterClient : ISecurityMasterServices, IDisposable
    {
        CompanyUser _loginUser = null;

        ClientTradeCommManager _clientCommunicationManager = null;

        PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();

        public event EventHandler Disconnected;

        public event EventHandler Connected;

        int _hashCode = int.MinValue;

        public SecMasterClient()
        {
            SecmasterPricingReqManagerClient.Instance.SendRequestForGuid += Instance_SendRequestForGuid;
            SecmasterPricingReqManagerClient.Instance.SendRequestForGuidWithoutHolidays += Instance_SendRequestForGuidWithoutHoliday;
        }

        void Instance_SendRequestForGuid(object sender, List<Guid> e)
        {
            try
            {
                GetGenericPricingDataFromSM(e);
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
                        InvokeSecMstrSymbolSearchResponse(searchResult);
                        break;

                    //handling on SM data recieved from sm server for vadidation
                    case CustomFIXConstants.MSG_SECMASTER_RESPONSE:
                        SecMasterBaseObj secMasterRes = (SecMasterBaseObj)binaryFormatter.DeSerialize(message.Message.ToString());
                        Logger.LoggerWrite("Received Security Master Response from Server at user " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString() + " for symbol: " + secMasterRes.TickerSymbol, LoggingConstants.CATEGORY_GENERAL);
                        SecMasterDataCache.GetInstance.AddValues(secMasterRes, DateTime.UtcNow);
                        InvokeSecMstrDataResponse(secMasterRes);
                        break;

                    //handling on SM data recieved from sm server for symbol lookup UI 
                    case CustomFIXConstants.MSG_SECMASTER_SymbolRESPONSE:
                        DataSet ds = (DataSet)binaryFormatter.DeSerialize(message.Message.ToString());
                        if (SymbolLookUpDataResponse != null)
                        {
                            SymbolLookUpDataResponse(sender, new EventArgs<DataSet>(ds));
                        }
                        break;

                    //handling on SM data recieved from sm server for  symbol list request
                    case CustomFIXConstants.MSG_SECMASTER_ListSymbolRESPONSE:
                        SecMasterbaseList secMasterBaseListResponse = (SecMasterbaseList)binaryFormatter.DeSerialize(message.Message.ToString());

                        List<SecMasterBaseObj> secMasterBaseObjList = new List<SecMasterBaseObj>(secMasterBaseListResponse);

                        SecMasterDataCache.GetInstance.AddValues(secMasterBaseObjList, DateTime.UtcNow);
                        break;

                    //handling on future root data recieved from sm server for "future root UI"
                    case CustomFIXConstants.MSG_SECMASTER_FutureMultiplierREQ:
                        DataSet dsFutRootData = (DataSet)binaryFormatter.DeSerialize(message.Message.ToString());
                        if (FutureRootSymbolDataResponse != null)
                        {
                            FutureRootSymbolDataResponse(sender, new EventArgs<DataSet>(dsFutRootData));
                        }
                        break;

                    //handling on preferences data recieved from sm server for root ui. like.. useCutOffTime
                    case CustomFIXConstants.MSG_SECMASTER_GetPrefREQ:
                        SecMasterGlobalPreferences preferences = (SecMasterGlobalPreferences)binaryFormatter.DeSerialize(message.Message.ToString());
                        if (SMGlobalPrefencesResponse != null)
                        {
                            SMGlobalPrefencesResponse(sender, new EventArgs<SecMasterGlobalPreferences>(preferences));
                        }
                        break;

                    //handling on UDA symbol data recieved from sm server for UDA UI - not in use
                    case CustomFIXConstants.MSG_SECMASTER_UDA_DATA_Res:

                        UDASymbolDataCollection UDASymbolDataCol = (UDASymbolDataCollection)binaryFormatter.DeSerialize(message.Message.ToString());

                        if (udaUISymbolDataResponse != null)
                        {
                            udaUISymbolDataResponse(sender, new EventArgs<UDASymbolDataCollection>(UDASymbolDataCol));
                        }
                        break;

                    //handling on UDA (attributes) data recieved from sm server
                    case SecMasterConstants.Const_UDARes:

                        Dictionary<String, Dictionary<int, string>> UDACol = (Dictionary<String, Dictionary<int, string>>)binaryFormatter.DeSerialize(message.Message.ToString());
                        UDADataCache.GetInstance.AllUDAAttributesDict = UDACol;
                        if (UDAAttributesResponse != null)
                        {
                            UDAAttributesResponse(sender, new EventArgs<Dictionary<string, Dictionary<int, string>>>(UDACol));
                        }
                        break;

                    //handling on hist/ current traded SM data recieved from sm server
                    case SecMasterConstants.CONST_TradedSMDataUIRes:
                        List<SecMasterBaseObj> SMData = binaryFormatter.DeSerialize(message.Message.ToString()) as List<SecMasterBaseObj>;

                        if (EventTradedSMDataUIRes != null && SMData != null)
                        {
                            EventTradedSMDataUIRes(sender, new EventArgs<List<SecMasterBaseObj>>(SMData));
                        }
                        break;

                    //handling on "in used UDA data" recieved from sm server
                    case SecMasterConstants.CONST_InUsedUDADataRes:
                        Dictionary<string, Dictionary<int, string>> inUsedUDAsDict = binaryFormatter.DeSerialize(message.Message.ToString()) as Dictionary<string, Dictionary<int, string>>;

                        if (EventInUsedUDARes != null && inUsedUDAsDict != null)
                        {
                            EventInUsedUDARes(sender, new EventArgs<Dictionary<string, Dictionary<int, string>>>(inUsedUDAsDict));
                        }
                        break;

                    //handle on fut root data recieved from server for specific symbol
                    case SecMasterConstants.CONST_SymbolRootData:


                        if (FutSymbolRootDataRes != null)
                        {
                            FutSymbolRootDataRes(sender, e);
                        }

                        break;
                    case SecMasterConstants.CONST_SMGenericPriceRequest:
                        PricingRequestMappings sentRequest;
                        if (SecmasterPricingReqManagerClient.Instance.RequestInProcess.TryRemove(message.RequestID, out sentRequest))
                        {
                            Action<QueueMessage, Guid> responseFunct = sentRequest.ClientFunction;
                            if (responseFunct != null)
                            {
                                responseFunct(message, Guid.Parse(message.RequestID));
                            }
                        }
                        break;
                    case SecMasterConstants.CONST_CentralSMConnected:
                        string centralSMConnected = message.Message.ToString();
                        if (CentralSMConnected != null)
                        {
                            CentralSMConnected(sender, new EventArgs<string>(centralSMConnected));
                        }
                        break;
                    case SecMasterConstants.CONST_CentralSMDisconnected:
                        string centralSMDisconnected = message.Message.ToString();
                        if (CentralSMDisconnected != null)
                        {
                            CentralSMDisconnected(sender, new EventArgs<string>(centralSMDisconnected));
                        }
                        break;
                    case SecMasterConstants.CONST_SMFieldsResponse:
                        Object concurFieldDict = binaryFormatter.DeSerialize(message.Message.ToString());
                        SecMasterCommonCache.Instance.FillSecMasterCommonCache((ConcurrentDictionary<string, StructPricingField>)concurFieldDict);
                        break;

                    case CustomFIXConstants.MSG_SECMASTER_AUEC_MAPPING_RESPONSE:
                        DataSet dsAUECMapping = binaryFormatter.DeSerialize(message.Message.ToString()) as DataSet;

                        if (AUECMappingGetDataResponse != null && dsAUECMapping != null)
                        {
                            AUECMappingGetDataResponse(sender, new EventArgs<DataSet>(dsAUECMapping));
                        }
                        break;

                    case CustomFIXConstants.MSG_SECMASTER_FUND_SYMBOL_UDA_RESPONSE:
                        DataSet dsAccountUDAdata = binaryFormatter.DeSerialize(message.Message.ToString()) as DataSet;

                        if (AccountWiseUDADataResponse != null && dsAccountUDAdata != null)
                        {
                            AccountWiseUDADataResponse(sender, new EventArgs<DataSet>(dsAccountUDAdata));
                        }
                        break;

                    // future root data saved response from server, PRANA-9815
                    case CustomFIXConstants.MSG_SECMASTER_FutureMultiplierSaveRESPONSE:
                        if (FutRootDataSaveRes != null)
                        {
                            FutRootDataSaveRes(sender, e);
                        }
                        break;

                    // update security master client cache
                    case CustomFIXConstants.MSG_SECMASTER_UpdateClientCache:
                        SecMasterBaseObj secMasterBaseObj = (SecMasterBaseObj)binaryFormatter.DeSerialize(message.Message.ToString());
                        SecMasterDataCache.GetInstance.UpdateIssuerInSMCacheOfUnderlying(secMasterBaseObj);
                        break;
                    case CustomFIXConstants.MSG_SECMASTER_BulkSymbolRESPONSE:
                        Dictionary<string, List<SecMasterBaseObj>> secMasterBulkRes = (Dictionary<string, List<SecMasterBaseObj>>)binaryFormatter.DeSerialize(message.Message.ToString());
                        int totalChunks = 0;
                        int processedChunks = 0;
                        int errorFlag = 0;
                        List<SecMasterBaseObj> secMasterObjectList = new List<SecMasterBaseObj>();

                        foreach (KeyValuePair<string, List<SecMasterBaseObj>> keyvalPair in secMasterBulkRes)
                        {
                            string key = keyvalPair.Key;
                            totalChunks = Convert.ToInt32(key.Split(',')[0]);
                            processedChunks = Convert.ToInt32(key.Split(',')[1]);
                            errorFlag = Convert.ToInt32(key.Split(',')[2]);
                            secMasterObjectList = keyvalPair.Value;
                        }
                        if (secMasterObjectList != null)
                        {
                            foreach (SecMasterBaseObj secMasterRes1 in secMasterObjectList)
                            {
                                SecMasterDataCache.GetInstance.AddValues(secMasterRes1, DateTime.UtcNow);
                                InvokeSecMstrDataResponse(secMasterRes1);
                            }
                        }
                        if (SecMstrBulkResponse != null)
                        {
                            SecMstrBulkResponse(this, new EventArgs<int, int, int>(totalChunks, processedChunks, errorFlag));
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

        # region SecMasterCacheMgr Invoke part
        delegate void AsyncInvokeDelegate(Delegate del, params object[] args);
        void InvokeSecMstrDataResponse(SecMasterBaseObj secMasterObj)
        {
            try
            {
                //Response for validation requests from pricing fetching functions
                List<Guid> guids = SecmasterPricingReqManagerClient.Instance.RemoveValidatedSymbolAndSendRequest(secMasterObj);
                GetGenericPricingDataFromSM(guids);
                if (SecMstrDataResponse != null)
                {
                    Delegate[] subscriberList = SecMstrDataResponse.GetInvocationList();

                    AsyncInvokeDelegate invoker = new AsyncInvokeDelegate(InvokeDelegate);
                    foreach (Delegate subscriber in subscriberList)
                    {
                        int subscriberHashCode = subscriber.Target.GetHashCode();
                        if (SecMasterDataCache.GetInstance.SubscriberSnapShotHash.ContainsKey(subscriberHashCode))
                        {
                            if (SecMasterDataCache.GetInstance.IsSnapShotRequested(secMasterObj, subscriberHashCode))
                            {
                                invoker.BeginInvoke(subscriber, new object[1] { new EventArgs<SecMasterBaseObj>(secMasterObj) }, null, null);//object[1] { secMasterObj }
                                SecMasterDataCache.GetInstance.RemoveFromSnapShotSubscribers(secMasterObj, subscriberHashCode);
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

        void InvokeSecMstrSymbolSearchResponse(SecMasterSymbolSearchRes searchRes)
        {
            try
            {
                if (SecMstrDataSymbolSearcResponse != null)
                {
                    Delegate[] subscriberList = SecMstrDataSymbolSearcResponse.GetInvocationList();

                    AsyncInvokeDelegate invoker = new AsyncInvokeDelegate(InvokeDelegate);
                    foreach (Delegate subscriber in subscriberList)
                    {
                        int subscriberHashCode = subscriber.Target.GetHashCode();
                        if (subscriberHashCode == searchRes.HashCode)
                        {
                            invoker.BeginInvoke(subscriber, new object[1] { new EventArgs<SecMasterSymbolSearchRes>(searchRes) }, null, null);//object[1] { searchRes }
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
        private static void InvokeDelegate(Delegate sink, params object[] args)
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

        #endregion

        #region ISecurityMaster Members
        public void ConnectToServer()
        {
            try
            {
                _hashCode = this.GetHashCode();
                if (_clientCommunicationManager == null || (CachedDataManager.GetInstance.LoggedInUser != null && _clientCommunicationManager.ConnectionStatus != PranaInternalConstants.ConnectionStatus.CONNECTED))
                {
                    //connect to server 
                    _loginUser = CachedDataManager.GetInstance.LoggedInUser;
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
                    _smUserID = "SM" + _loginUser.CompanyUserID.ToString();
                    connProperties.IdentifierID = _smUserID;
                    connProperties.IdentifierName = "Sec Master" + _loginUser.FirstName.ToString();
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

        public async System.Threading.Tasks.Task ConnectToServerAsync()
        {
            ConnectToServer();

            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;
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

        public event EventHandler<EventArgs<QueueMessage>> ResponseCompleted;

        public void RequestFieldsDictionary()
        {
            string request = binaryFormatter.Serialize(SecMasterConstants.CONST_SMGetFields);
            QueueMessage qMsg = new QueueMessage(SecMasterConstants.CONST_SMGetFields, request);
            _clientCommunicationManager.SendMessage(qMsg);
        }

        public void SendRequest(SecMasterRequestObj secMasterReqObj)
        {
            try
            {
                List<SecMasterBaseObj> newlist = SecMasterDataCache.GetInstance.GetSecMasterData(secMasterReqObj, DateTime.UtcNow);
                SecMasterDataCache.GetInstance.RequestSymbolData(secMasterReqObj, null);

                #region Sending the response and Removing the symbols from request obj which are found at TS

                if (newlist.Count > 0)
                {
                    foreach (SecMasterBaseObj secMasterBaseObj in newlist)
                    {
                        SymbolDataRow datarow = secMasterReqObj.GetSymbolRow(secMasterBaseObj);
                        if (datarow != null)
                        {
                            secMasterBaseObj.RequestedSymbology = (int)datarow.PrimarySymbology;
                            InvokeSecMstrDataResponse(secMasterBaseObj);
                            secMasterReqObj.Remove(datarow);
                        }
                    }
                }

                #endregion

                #region Sending the request to pricing server for Snap shot data for remaining Symbols

                if (secMasterReqObj.IsRequestValid())
                {
                    if (_clientCommunicationManager != null && _clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                    {
                        // so that request sent is received by the same user
                        if (secMasterReqObj.HashCode == int.MinValue || secMasterReqObj.HashCode == 0)
                        {
                            secMasterReqObj.HashCode = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                        }

                        secMasterReqObj.UserID = "SM" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                        string request = binaryFormatter.Serialize(secMasterReqObj);
                        QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_REQ, request);
                        _clientCommunicationManager.SendMessage(qMsg);
                    }
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Client is trying to validate symbol: " + secMasterReqObj.GetPrimarySymbols()[0] + ", And waiting for response because server is not connected.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }
                }

                #endregion
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

        public event EventHandler<EventArgs<SecMasterBaseObj>> SecMstrDataResponse;

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

        public List<SecMasterBaseObj> SendRequestList(SecMasterRequestObj secMasterReqObj)
        {
            List<SecMasterBaseObj> totalData = new List<SecMasterBaseObj>();
            try
            {
                totalData = SecMasterDataCache.GetInstance.GetSecMasterData(secMasterReqObj, DateTime.UtcNow);

                foreach (SecMasterBaseObj secMasterBaseObj in totalData)
                {
                    SymbolDataRow datarow = secMasterReqObj.GetSymbolRow(secMasterBaseObj);
                    if (datarow != null)
                    {
                        secMasterReqObj.Remove(datarow);
                    }
                }
                if (secMasterReqObj.Count == 0)
                {
                    return totalData;
                }
                SendRequest(secMasterReqObj);
                return totalData;
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
                return null;
            }
        }

        public List<SecMasterBaseObj> GetSMCachedData(SecMasterRequestObj secMasterReqObj)
        {
            List<SecMasterBaseObj> totalData = new List<SecMasterBaseObj>();
            try
            {
                totalData = SecMasterDataCache.GetInstance.GetSecMasterData(secMasterReqObj, DateTime.UtcNow);

                return totalData;
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
                return null;
            }
        }

        public void SaveNewSymbols(SecMasterbaseList lst)
        {
            try
            {
                if (_clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    lst.UserID = _smUserID;
                    string request = binaryFormatter.Serialize(lst);
                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_SaveREQ, request);
                    qMsg.HashCode = _hashCode;
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

        /// <summary>
        /// Saves the share outstanding.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="sharesOutstanding">The shares outstanding.</param>
        public void SaveShareOutstanding(string symbol, double sharesOutstanding)
        {
            try
            {
                if (_clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(symbol);
                    sb.Append(Seperators.SEPERATOR_5);
                    sb.Append(sharesOutstanding);

                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_UPDATE_SHAREOUTSTANDING, sb.ToString());
                    qMsg.UserID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                    qMsg.HashCode = _hashCode;
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

        public void SaveNewSymbols_Import(SecMasterbaseList lst)
        {
            try
            {
                if (_clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    lst.UserID = _smUserID;

                    string request = binaryFormatter.Serialize(lst);
                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_SaveREQ_IMPORT, request);
                    qMsg.HashCode = _hashCode;
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

        public void EnRichSecMasterObj(DataTable dtable)
        {
            try
            {
                if (_clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    string objResponse = binaryFormatter.Serialize(dtable);
                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_UpdateSMFields_IMPORT, objResponse);
                    qMsg.HashCode = _hashCode;
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

        public void UpdateSymbols_Import(SecMasterUpdateDataByImportList lst)
        {
            try
            {
                if (_clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    lst.UserID = _smUserID;

                    string request = binaryFormatter.Serialize(lst);
                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_UpdateREQ_IMPORT, request);
                    qMsg.HashCode = _hashCode;
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

        public event EventHandler<EventArgs<DataSet>> SymbolLookUpDataResponse;
        public event EventHandler<EventArgs<UDASymbolDataCollection>> udaUISymbolDataResponse;
        public event EventHandler<EventArgs<Dictionary<String, Dictionary<int, string>>>> UDAAttributesResponse;
        public event EventHandler<EventArgs<List<SecMasterBaseObj>>> EventTradedSMDataUIRes;
        public event EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>> EventInUsedUDARes;
        public event EventHandler<EventArgs<QueueMessage>> FutSymbolRootDataRes;
        public event EventHandler<EventArgs<DataSet>> AUECMappingGetDataResponse;
        public event EventHandler<EventArgs<int, int, int>> SecMstrBulkResponse;

        /// <summary>
        /// event for future root data response after save
        /// </summary>
        public event EventHandler<EventArgs<QueueMessage>> FutRootDataSaveRes;

        public event EventHandler<EventArgs<DataSet>> FutureRootSymbolDataResponse;
        public void GetSymbolLookupRequestedData(SymbolLookupRequestObject symbolLookupReqObj)
        {
            if (_clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                //need to change msg type
                symbolLookupReqObj.CompanyUserID = "SM" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                string request = binaryFormatter.Serialize(symbolLookupReqObj);
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_SymbolREQ, request);
                qMsg.HashCode = _hashCode;
                qMsg.RequestID = symbolLookupReqObj.RequestID;
                _clientCommunicationManager.SendMessage(qMsg);
            }
            else
            {
                if (Disconnected != null)
                {
                    Disconnected(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Send reguest to server for getting UDA data.
        /// </summary>
        /// <param name="udaDataReqObj"></param>
        public void GetSymbolsUDAData(UDADataReqObj udaDataReqObj)
        {
            if (_clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                //need to change msg type
                udaDataReqObj.CompanyUserID = "SM" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                string request = binaryFormatter.Serialize(udaDataReqObj);
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_UDA_DATA_Req, request);
                qMsg.HashCode = _hashCode;
                qMsg.RequestID = udaDataReqObj.RequestID;
                _clientCommunicationManager.SendMessage(qMsg);
            }
            else
            {
                if (Disconnected != null)
                {
                    Disconnected(this, EventArgs.Empty);
                }
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

        public event EventHandler<EventArgs<SecMasterGlobalPreferences>> SMGlobalPrefencesResponse;

        /// <summary>
        /// Bharat Jangir (22 September, 2014)
        /// Getting AUEC Mappings Request for Option & Portfolio Science symbols generation
        /// </summary>
        public void GetAUECMappings()
        {
            try
            {
                String RequestID = System.Guid.NewGuid().ToString();
                DataReqByKeyFrmServer(CustomFIXConstants.MSG_SECMASTER_AUEC_MAPPING_REQUEST, RequestID);
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
        /// Bharat Jangir (22 September, 2014)
        /// Saving AUEC Mappings for Option & Portfolio Science symbols generation
        /// </summary>
        /// <param name="saveDataSetTemp"></param>
        public void SaveAUECMappings(DataSet saveDataSetTemp)
        {
            try
            {
                string data = binaryFormatter.Serialize(saveDataSetTemp);
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_AUEC_MAPPING_SAVE, data);
                qMsg.UserID = _smUserID;
                qMsg.HashCode = _hashCode;
                qMsg.RequestID = System.Guid.NewGuid().ToString();
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

        private string _smUserID;

        public void SendMessage(QueueMessage message)
        {
            if (_clientCommunicationManager != null && _clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                message.UserID = _smUserID;
                _clientCommunicationManager.SendMessage(message);
            }
        }

        #region ISecurityMasterServices Members

        ICommunicationManager _tradeServerCommunicationManager;
        /// <summary>
        /// We have assigned TradeCommunicationManager through Nirvanamain class because, on 
        /// this communication manager, we consistenly get the connectedf and disconnected events.
        /// This would in turn help forms like symbol look up to cancel the requests.
        /// </summary>
        public ICommunicationManager TradeCommunicationManager
        {
            get
            {
                return _tradeServerCommunicationManager;
            }
            set
            {
                _tradeServerCommunicationManager = value;
                _tradeServerCommunicationManager.Connected += new EventHandler(_tradeServerCommunicationManager_Connected);
                _tradeServerCommunicationManager.Disconnected += new EventHandler(_tradeServerCommunicationManager_Disconnected);
            }
        }

        void _tradeServerCommunicationManager_Connected(object sender, EventArgs e)
        {
            if (Connected != null)
            {
                Connected(this, EventArgs.Empty);
            }
        }

        void _tradeServerCommunicationManager_Disconnected(object sender, EventArgs e)
        {
            if (Disconnected != null)
            {
                Disconnected(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// In order to disable retryconnection to server in case of LogOut.
        /// See details : http://jira.nirvanasolutions.com:8080/browse/PRANA-1797
        /// </summary>
        /// <param name="shouldRetry"></param>
        public void Retry(bool shouldRetry)
        {
            _clientCommunicationManager.ShouldRetry = shouldRetry;
        }

        /// <summary>
        /// Send UDA attributs to server to save in DB
        /// </summary>
        /// <param name="UDADataCol"></param>
        public void SaveUDAData(Dictionary<String, Dictionary<string, object>> UDADataCol)
        {
            try
            {
                if (_clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    string data = binaryFormatter.Serialize(UDADataCol);
                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_UDA_Save, data);
                    qMsg.UserID = _smUserID;
                    qMsg.HashCode = _hashCode;
                    qMsg.RequestID = System.Guid.NewGuid().ToString();
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

        public void DataReqByKeyFrmServer(string DataReqKey, string RequestID)
        {
            try
            {
                if (_clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {

                    QueueMessage qMsg = new QueueMessage(DataReqKey, "", _smUserID, "");
                    qMsg.HashCode = _hashCode;
                    qMsg.RequestID = RequestID;
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

        /// <summary>
        /// Get UDA attributes from cache. if not found, send request to server.
        /// </summary>
        public void GetAllUDAAtrributes()
        {
            try
            {
                Dictionary<String, Dictionary<int, string>> AllUDADict = UDADataCache.GetInstance.AllUDAAttributesDict;
                if (AllUDADict != null)
                {
                    if (UDAAttributesResponse != null)
                    {
                        UDAAttributesResponse(UDADataCache.GetInstance, new EventArgs<Dictionary<string, Dictionary<int, string>>>(AllUDADict));
                    }
                }
                else
                {
                    String RequestID = System.Guid.NewGuid().ToString();
                    DataReqByKeyFrmServer(SecMasterConstants.Const_UDAReq, RequestID);
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

        public void GetInUsedUDAIds()
        {
            try
            {
                String RequestID = System.Guid.NewGuid().ToString();
                DataReqByKeyFrmServer(SecMasterConstants.CONST_InUsedUDADataReq, RequestID);
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
        /// Gets the historical pricing data from Bloomberg for any field and symbol
        /// </summary>
        /// <param name="fields">The Nirvana fields which are mapped to bloomberg fields in the mapping file</param>
        /// <param name="secMasterReqObj">The request object containing the </param>
        /// <param name="startDate">The date from which the data is needed</param>
        /// <param name="endDate">Date till which the data is needed</param>
        /// <param name="requestUUID">Can be used to relate requests with response. Use Guid.NewGuid() if dont know what to send.</param>
        /// <param name="clientFunction">The client function to be called in response for this request, returns the returned message from server and the request GUId</param>
        public void GetMarkPricesForSymbolAndDate(String secondaryPricingSource, List<string> fields, SecMasterRequestObj secMasterReqObj, DateTime startDate, DateTime endDate, Guid requestUUID, Action<QueueMessage, Guid> clientFunction, bool isGetDataFromCacheOrDB)
        {
            try
            {
                if (LoggingConstants.LoggingEnabled)
                {
                    Logger.LoggerWrite("Historical pricing generic request. Fields: " + String.Join(",", fields) + Environment.NewLine + "StartDate : " + startDate.ToString() + " EndDate : " + endDate.ToString() + Environment.NewLine
                        + " Requested Symbols : " + String.Join(",", secMasterReqObj.GetPrimarySymbols()));
                }
                SecmasterPricingReqManagerClient.Instance.RegisterRequestForValidation(secondaryPricingSource, fields, secMasterReqObj, requestUUID, startDate, endDate, clientFunction, isGetDataFromCacheOrDB);
                secMasterReqObj.HashCode = this.GetHashCode();
                SendRequest((SecMasterRequestObj)secMasterReqObj.Clone());
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

        private void GetGenericPricingDataFromSM(List<Guid> listOfGuidValidated)
        {
            try
            {
                foreach (Guid requestId in listOfGuidValidated)
                {
                    SecmasterPricingReqManagerClient.Instance.ProcessSecMasterRequestForSM(requestId);
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

        void Instance_SendRequestForGuidWithoutHoliday(object sender, string requestID)
        {
            try
            {
                if (SecmasterPricingReqManagerClient.Instance.RequestInProcess.Count > 0)
                {
                    PricingRequestMappings pricingRequestRow = SecmasterPricingReqManagerClient.Instance.RequestInProcess[requestID.ToString()];

                    string request = binaryFormatter.Serialize(pricingRequestRow.SecondaryPricingSource, pricingRequestRow.FieldNames, pricingRequestRow.RequestObj, pricingRequestRow.StartDate, pricingRequestRow.EndDate, pricingRequestRow.IsGetDataFromCacheOrDB);
                    QueueMessage qMsg = new QueueMessage(SecMasterConstants.CONST_SMGenericPriceRequest, "", CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString(), request);
                    qMsg.HashCode = _hashCode;
                    qMsg.RequestID = requestID.ToString();
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


        /// <summary>
        /// to do: move this method to different class as refreshing the server cache from this class is not a suitable approach
        /// Added by: Bharat Raturi, 02 jun 2014
        /// purpose: refresh the server cache
        /// </summary>
        public void RefreshServerCache()
        {
            try
            {
                String RequestID = System.Guid.NewGuid().ToString();
                DataReqByKeyFrmServer(SecMasterConstants.CONST_REFRESH_CACHE, RequestID);
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

        public event EventHandler<EventArgs<string>> CentralSMConnected;

        public event EventHandler<EventArgs<string>> CentralSMDisconnected;

        bool _isCSMConnected = false;
        public bool IsCSMConnected
        {
            get
            {
                return _isCSMConnected;
            }
            set
            {
                _isCSMConnected = value;
            }
        }

        public event EventHandler<EventArgs<DataSet>> AccountWiseUDADataResponse;


        public void searchSymbols(SecMasterSymbolSearchReq req)
        {
            try
            {
                if (req.HashCode == int.MinValue || req.HashCode == 0)
                {
                    req.HashCode = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
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

        public event EventHandler<EventArgs<SecMasterSymbolSearchRes>> SecMstrDataSymbolSearcResponse;
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
