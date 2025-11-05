using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SMObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.SecurityMasterNew.BLL;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace Prana.SecurityMasterNew
{
    /// <summary>
    /// The server class which recieves messages from SecMasterClient : ISecurityMasterServices
    /// </summary>
    public class SecMasterServerComponent
    {
        static IQueueProcessor _queueCommMgrOut = null;
        static SecMasterServerComponent _SecMasterServerComponent = null;
        static int _hashCode = int.MinValue;
        static PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        static SecMasterServerComponent()
        {
            _SecMasterServerComponent = new SecMasterServerComponent();
        }
        public static SecMasterServerComponent GetInstance
        {
            get
            {
                return _SecMasterServerComponent;
            }
        }
        private ISecMasterServices _secMasterServices;

        public void Initilise(IQueueProcessor queueCommMgrIn, IQueueProcessor queueCommMgrOut, ISecMasterServices secmasterServices)
        {
            try
            {
                _secMasterServices = secmasterServices;
                _queueCommMgrOut = queueCommMgrOut;
                _hashCode = this.GetHashCode();
                SecMasterDataCache.GetInstance.Subscribe(_hashCode);
                _secMasterServices.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(GetInstance_SecMstrDataResponse);
                //new SecurityMasterDataHandler(GetInstance_SecMstrDataResponse);
                _secMasterServices.StatusOfRequest += new EventHandler<EventArgs<string, string, string>>(GetInstance_StatusOfRequest);
                //new StatucRequestDelegate(GetInstance_StatusOfRequest);
                queueCommMgrIn.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(queueCommMgrIn_MessageQueued);
                //new MessageReceivedHandler(queueCommMgrIn_MessageQueued);
                _secMasterServices.SymbolLkUpDataResponse += new EventHandler<EventArgs<DataSet, SymbolLookupRequestObject>>(GetInstance_SymbolLookUpDataResponse);
                //new SymbolLookUpDataResponse_ReqObj(GetInstance_SymbolLookUpDataResponse);
                //_secMasterServices.UDAUISymbolDataRecieved += new UDASymbolDataResponse_ReqObj(_secMasterServices_UDAUISymbolDataRecieved);
                _secMasterServices.EventSendDataByResKey += new EventHandler<EventArgs<string, object, string, string>>(_secMasterServices_SendDataByResKey);
                //new DelegateSendDataByResKey(_secMasterServices_SendDataByResKey);
                _secMasterServices.FutureRootDataSavedResponse += new EventHandler<EventArgs<string, string, string>>(GetInstance_FutureRootDataSavedResponse);
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
        /// Send  UDA data to UDA UI
        /// </summary>
        /// <param name="secMasterList"></param>
        /// <param name="udaDataReqObj"></param>
        void _secMasterServices_UDAUISymbolDataRecieved(UDASymbolDataCollection UDASymbolDataCol, UDADataReqObj udaDataReqObj)
        {
            try
            {
                string request = binaryFormatter.Serialize(UDASymbolDataCol);
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_UDA_DATA_Res, "", udaDataReqObj.CompanyUserID, request);
                _queueCommMgrOut.SendMessage(qMsg);
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

        void GetInstance_StatusOfRequest(object sender, EventArgs<string, string, string> e)//(message, userID, requestID)
        {
            try
            {
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_RESPONSE_COMPLETED, "", e.Value2, e.Value);
                qMsg.RequestID = e.Value3;
                _queueCommMgrOut.SendMessage(qMsg);
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
        /// Send future root save response
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetInstance_FutureRootDataSavedResponse(object sender, EventArgs<string, string, string> e)
        {
            try
            {
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_FutureMultiplierSaveRESPONSE, "", e.Value2, e.Value);
                qMsg.RequestID = e.Value3;
                _queueCommMgrOut.SendMessage(qMsg);
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
        ///  Send data via msgQ with data key response
        /// </summary>
        /// <param name="DataResKey"></param>
        /// <param name="message"></param>
        /// <param name="userID"></param>
        /// <param name="requestID"></param>
        void _secMasterServices_SendDataByResKey(object sender, EventArgs<string, object, string, string> e)//(String DataResKey, object message, string userID, string requestID)
        {
            try
            {
                string SerializedData = binaryFormatter.Serialize(e.Value2);
                QueueMessage qMsg = new QueueMessage(e.Value, "", e.Value3, SerializedData);

                qMsg.RequestID = e.Value4;
                _queueCommMgrOut.SendMessage(qMsg);
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

        void GetInstance_SymbolLookUpDataResponse(object sender, EventArgs<DataSet, SymbolLookupRequestObject> e)//(ds, SymbolLookupRequestObject symbolLookupReqObj)
        {
            try
            {
                string request = binaryFormatter.Serialize(e.Value);
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_SymbolRESPONSE, "", e.Value2.CompanyUserID, request);
                _queueCommMgrOut.SendMessage(qMsg);
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

        public void SendDataToClient(SecMasterBaseObj secMasterObj)
        {
            try
            {
                if (secMasterObj != null)
                {
                    string request = binaryFormatter.Serialize(secMasterObj);
                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_RESPONSE, request);

                    _queueCommMgrOut.SendMessage(qMsg);
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
        public void SendDataToClient(SecMasterbaseList secMasterObj)
        {
            try
            {
                if (secMasterObj.Count > 0)
                {
                    string request = binaryFormatter.Serialize(secMasterObj);
                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_ListSymbolRESPONSE, request);
                    _queueCommMgrOut.SendMessage(qMsg);
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

        public void SendDataToClient(List<SecMasterBaseObj> SecMasterBaseObjList, int totalChunks, int processedChunks, int errorFlag)
        {
            try
            {
                string chunkinfo = totalChunks.ToString() + "," + processedChunks.ToString() + "," + errorFlag.ToString();
                Dictionary<string, List<SecMasterBaseObj>> dict = new Dictionary<string, List<SecMasterBaseObj>>();
                dict.Add(chunkinfo, SecMasterBaseObjList);

                //if (SecMasterBaseObjList.Count > 0)
                //{
                string request = binaryFormatter.Serialize(dict);
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_BulkSymbolRESPONSE, request);
                _queueCommMgrOut.SendMessage(qMsg);
                //}
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


        void GetInstance_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            SendDataToClient(e.Value);
        }

        /// <summary>
        /// update security master cache on client
        /// </summary>
        /// <param name="secMasterObj"></param>
        public void SecMasterUpdateClientCache(SecMasterBaseObj secMasterObj)
        {
            try
            {
                if (secMasterObj != null)
                {
                    string request = binaryFormatter.Serialize(secMasterObj);
                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_UpdateClientCache, request);

                    _queueCommMgrOut.SendMessage(qMsg);
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

        void queueCommMgrIn_MessageQueued(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                switch (e.Value.MsgType)
                {
                    case CustomFIXConstants.MSG_SECMASTER_SYMBOLSEARCH_REQ:
                        SecMasterSymbolSearchReq searchReq = (SecMasterSymbolSearchReq)binaryFormatter.DeSerialize(e.Value.Message.ToString());
                        SecMasterSymbolSearchRes searchRes = _secMasterServices.ReqSymbolSearch(searchReq);
                        if (searchRes != null)
                        {
                            searchRes.HashCode = searchReq.HashCode;
                            searchRes.UserID = searchReq.UserID;
                            string seriString = binaryFormatter.Serialize(searchRes);
                            QueueMessage resMessage = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_SYMBOLSEARCH_RESPONSE, seriString);
                            _queueCommMgrOut.SendMessage(resMessage);
                        }
                        break;
                    case CustomFIXConstants.MSG_SECMASTER_REQ:
                        {
                            SecMasterRequestObj reqObj = (SecMasterRequestObj)binaryFormatter.DeSerialize(e.Value.Message.ToString());
                            reqObj.HashCode = _hashCode;
                            _secMasterServices.GetSecMasterData(reqObj, _hashCode);
                            break;
                        }
                    case CustomFIXConstants.MSG_SECMASTER_UPDATE_SHAREOUTSTANDING:
                        {
                            string[] msg = e.Value.Message.ToString().Split(Seperators.SEPERATOR_5);
                            if (msg.Length == 2)
                            {
                                string symbol = msg[0];
                                double sharesOutstanding = Convert.ToDouble(msg[1]);
                                _secMasterServices.SaveShareOutStandingInSecMaster(symbol, sharesOutstanding, e.Value.UserID);
                            }
                            break;
                        }
                    case CustomFIXConstants.MSG_SECMASTER_SaveREQ:
                        {
                            SecMasterbaseList reqObj = (SecMasterbaseList)binaryFormatter.DeSerialize(e.Value.Message.ToString());

                            //Modified by omshiv, save at CSM based on release type
                            _secMasterServices.SaveSecurityInSecurityMaster(reqObj);
                            //_secMasterServices.SaveNewSymbolToSecurityMaster(reqObj);
                            break;
                        }
                    case CustomFIXConstants.MSG_SECMASTER_SymbolREQ:
                        {
                            SymbolLookupRequestObject reqObj = (SymbolLookupRequestObject)binaryFormatter.DeSerialize(e.Value.Message.ToString());
                            _secMasterServices.GetSymbolLookupRequestedData(reqObj);
                            break;
                        }
                    case CustomFIXConstants.MSG_SECMASTER_FutureMultiplierREQ:
                        {
                            DataSet ds = _secMasterServices.GetFutureRootData();
                            if (ds != null)
                            {
                                string objResponse = binaryFormatter.Serialize(ds);
                                QueueMessage rootSymbolMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_FutureMultiplierREQ, objResponse);
                                _queueCommMgrOut.SendMessage(rootSymbolMsg);
                            }
                            break;
                        }
                    case CustomFIXConstants.MSG_SECMASTER_FutureMultiplierSave:
                        {
                            DataTable dtFuturRootData = (DataTable)binaryFormatter.DeSerialize(e.Value.Message.ToString());
                            _secMasterServices.SaveFutureRootData(dtFuturRootData, e.Value.UserID, e.Value.RequestID);
                            break;

                        }
                    case CustomFIXConstants.MSG_SECMASTER_SavePrefREQ:
                        {
                            SecMasterGlobalPreferences SMpreferences = (SecMasterGlobalPreferences)binaryFormatter.DeSerialize(e.Value.Message.ToString());
                            _secMasterServices.SaveSMPreferences(SMpreferences);
                            break;

                        }
                    case CustomFIXConstants.MSG_SECMASTER_GetPrefREQ:
                        {
                            SecMasterGlobalPreferences preferences = _secMasterServices.GetSMPreferences();
                            string objResponse = binaryFormatter.Serialize(preferences);
                            QueueMessage PrefMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_GetPrefREQ, objResponse);
                            _queueCommMgrOut.SendMessage(PrefMsg);
                            break;
                        }
                    case CustomFIXConstants.MSG_SECMASTER_SaveREQ_IMPORT:
                        {
                            SecMasterbaseList reqObj = (SecMasterbaseList)binaryFormatter.DeSerialize(e.Value.Message.ToString());
                            _secMasterServices.SaveNewSymbolToSecurityMaster_Import(reqObj);
                            break;
                        }
                    case CustomFIXConstants.MSG_SECMASTER_UpdateREQ_IMPORT:
                        {
                            SecMasterUpdateDataByImportList reqObj = (SecMasterUpdateDataByImportList)binaryFormatter.DeSerialize(e.Value.Message.ToString());
                            _secMasterServices.UpdateSymbolToSecurityMaster_Import(reqObj);
                            break; ;
                        }
                    case CustomFIXConstants.MSG_SECMASTER_UpdateSMFields_IMPORT:
                        {
                            DataTable dtSMUpdateFields = (DataTable)binaryFormatter.DeSerialize(e.Value.Message.ToString());
                            SecMasterEnRichData.GetInstance.SetSMEnRichData(dtSMUpdateFields);
                            break; ;
                        }

                    //GET UDA symbol data for UDA UI - omshiv Sep, 2013 - not in used now
                    case CustomFIXConstants.MSG_SECMASTER_UDA_DATA_Req:
                        {
                            UDADataReqObj udaDataReqObj = (UDADataReqObj)binaryFormatter.DeSerialize(e.Value.Message.ToString());
                            udaDataReqObj.HashCode = _hashCode;
                            // _secMasterServices.GetSymbolsUDAData(udaDataReqObj, _hashCode);
                            break;

                        }
                    //save UDA attributes to DB - omshiv nov, 2013
                    case CustomFIXConstants.MSG_SECMASTER_UDA_Save:
                        {

                            _secMasterServices.SaveUDAAttributesData(e.Value.Message.ToString());
                            break;
                        }

                    //get UDA attributes from cache - omshiv nov, 2013
                    case SecMasterConstants.Const_UDAReq:
                        {
                            Dictionary<String, Dictionary<int, string>> UDAsDict = _secMasterServices.GetUDAAttributes();
                            UDADataCache.GetInstance.AllUDAAttributesDict = UDAsDict;
                            string objResponse = binaryFormatter.Serialize(UDAsDict);
                            QueueMessage UDAMsg = new QueueMessage(SecMasterConstants.Const_UDARes, objResponse);
                            _queueCommMgrOut.SendMessage(UDAMsg);
                            break;
                        }

                    //request for get all historical traded symbol's SM data - omshiv, Nov 2013
                    case SecMasterConstants.CONST_AllHistTradedSymbolsReq:
                        {
                            string userID = e.Value.UserID;
                            string reqID = e.Value.RequestID;
                            Boolean isOpenTradedSymbolReq = false;
                            _secMasterServices.GetAllHistOrOpenTradedSymbols(userID, reqID, isOpenTradedSymbolReq);

                            break;
                        }
                    //request for get all open symbol's SM data - omshiv, Nov 2013
                    case SecMasterConstants.CONST_AllOpenSymbolsReq:
                        {
                            string userID = e.Value.UserID;
                            string reqID = e.Value.RequestID;
                            Boolean isOpenTradedSymbolReq = true;
                            _secMasterServices.GetAllHistOrOpenTradedSymbols(userID, reqID, isOpenTradedSymbolReq);

                            break;
                        }
                    //get inUsed UDA data - omshiv, Nov 2013
                    case SecMasterConstants.CONST_InUsedUDADataReq:
                        {
                            string userID = e.Value.UserID;
                            string reqID = e.Value.RequestID;
                            Dictionary<string, Dictionary<int, string>> inUsedUDAsDict = UDADataCache.GetInstance.GetInUseUDAsIDList();
                            _secMasterServices_SendDataByResKey(this, new EventArgs<string, object, string, string>(SecMasterConstants.CONST_InUsedUDADataRes, inUsedUDAsDict, userID, reqID));
                            break;
                        }

                    //get future root data for specific symbol - omshiv, Nov 2013
                    case SecMasterConstants.CONST_SymbolRootData:
                        {
                            string userID = e.Value.UserID;
                            string reqID = e.Value.RequestID;
                            FutureRootData rootData = SecMasterDataCache.GetInstance.GetFutSymbolRootdata(e.Value.Message.ToString());
                            if (rootData != null)
                                _secMasterServices_SendDataByResKey(this, new EventArgs<string, object, string, string>(SecMasterConstants.CONST_SymbolRootData, rootData, userID, reqID));

                            break;
                        }
                    //get data on advanced search filter  - omshiv, Nov 2013
                    case SecMasterConstants.CONST_SMAdvncdSearch:
                        {
                            string userID = e.Value.UserID;
                            string reqID = e.Value.RequestID;
                            _secMasterServices.GetSMUIAdvncdSearchData(e.Value.Message.ToString(), userID, reqID);
                            break;
                        }
                    case SecMasterConstants.CONST_SMGenericPriceRequest:
                        {
                            string reqID = e.Value.RequestID;
                            string clientUserID = e.Value.UserID;
                            int clientHashCode = e.Value.HashCode;
                            SecMasterPricingReqManagerServer.Instance.AddGenericPriceRequestFromClient(reqID, clientUserID, clientHashCode, e.Value.TradingAccountID);
                            List<Object> list = binaryFormatter.DeSerializeParams(e.Value.Message.ToString());
                            if (LoggingConstants.LoggingEnabled)
                            {
                                ConcurrentBag<string> tempList = list[1] as ConcurrentBag<string>;
                                DateTime tempDateStart = (DateTime)list[3];
                                DateTime tempDateEnd = ((DateTime)list[4]);
                                SecMasterRequestObj req = (list[2] as SecMasterRequestObj);
                                Logger.LoggerWrite("Historical pricing generic request received on trade server. Fields: " + String.Join(",", tempList) + Environment.NewLine + "StartDate : " + tempDateStart.ToString() + " EndDate : " + tempDateEnd.ToString() + Environment.NewLine
                                    + " Requested Symbols : " + String.Join(",", req.GetPrimarySymbols()), LoggingConstants.CATEGORY_FLAT_FILE_ClientMessages);
                            }
                            _secMasterServices.GetGenericSMPrice(reqID, list[0] as string, list[1] as ConcurrentBag<string>, list[2] as SecMasterRequestObj, (DateTime)list[3], (DateTime)list[4], SendHistoricalDataToClient, Convert.ToBoolean(list[5]));
                        }
                        break;

                    case SecMasterConstants.CONST_REFRESH_CACHE:
                        {
                            CommonDataCache.CachedDataManager.GetInstance.RefreshFrequentlyUsedData();
                            InformationReporter.GetInstance.Write("User refreshed the cache of Server. User:" + e.Value.UserID);
                        }

                        break;
                    case SecMasterConstants.CONST_SMGetFields:
                        {
                            ConcurrentDictionary<string, StructPricingField> fieldsDict = _secMasterServices.GetPricingFields();
                            string request = binaryFormatter.Serialize(fieldsDict);
                            QueueMessage qMsg = new QueueMessage(SecMasterConstants.CONST_SMFieldsResponse, request);
                            _queueCommMgrOut.SendMessage(qMsg);
                        }
                        break;

                    case CustomFIXConstants.MSG_SECMASTER_AUEC_MAPPING_REQUEST:
                        {
                            string userID = e.Value.UserID;
                            string reqID = e.Value.RequestID;
                            DataSet dsAUECMapping = _secMasterServices.GetAUECMappings();
                            _secMasterServices_SendDataByResKey(this, new EventArgs<string, object, string, string>(CustomFIXConstants.MSG_SECMASTER_AUEC_MAPPING_RESPONSE, dsAUECMapping, userID, reqID));
                            break;
                        }

                    case CustomFIXConstants.MSG_SECMASTER_AUEC_MAPPING_SAVE:
                        {
                            DataSet saveDataSetTemp = binaryFormatter.DeSerialize(e.Value.Message.ToString()) as DataSet;
                            _secMasterServices.SaveAUECMappings(saveDataSetTemp);
                            break;
                        }

                    case CustomFIXConstants.MSG_SECMASTER_FUND_SYMBOL_UDA_REQUEST:
                        {
                            string userID = e.Value.UserID;
                            string reqID = e.Value.RequestID;
                            DataSet accountSymbolUDAData = _secMasterServices.GetAccountSymbolUDAData();
                            if (accountSymbolUDAData != null)
                                _secMasterServices_SendDataByResKey(this, new EventArgs<string, object, string, string>(CustomFIXConstants.MSG_SECMASTER_FUND_SYMBOL_UDA_RESPONSE, accountSymbolUDAData, userID, reqID));
                            break;
                        }

                    case CustomFIXConstants.MSG_SECMASTER_FUND_SYMBOL_UDA_SAVE:
                        {
                            DataSet accountSymbolUDADataTemp = binaryFormatter.DeSerialize(e.Value.Message.ToString()) as DataSet;
                            _secMasterServices.SaveAccountWiseUDAData(accountSymbolUDADataTemp);
                            break;
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

        public void CentralSMDisconnected(string name)
        {
            try
            {
                QueueMessage qMsg = new QueueMessage(SecMasterConstants.CONST_CentralSMDisconnected, name);
                _queueCommMgrOut.SendMessage(qMsg);
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

        public void CentralSMConnected(string name)
        {
            try
            {
                QueueMessage qMsg = new QueueMessage(SecMasterConstants.CONST_CentralSMConnected, name);
                _queueCommMgrOut.SendMessage(qMsg);
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

        internal void SendHistoricalDataToClient(string requestId, DataTable dt, bool pricingSucces, string comment)
        {
            try
            {
                if (dt != null)
                {
                    //clientUserID, clientHashCode, tradingAccountID
                    Tuple<string, int, string> clientDetails = SecMasterPricingReqManagerServer.Instance.GetGenericPriceRequestDataForClient(requestId);
                    if (clientDetails != null)
                    {
                        string request = binaryFormatter.Serialize(dt, pricingSucces, comment);
                        QueueMessage qMsg = new QueueMessage(SecMasterConstants.CONST_SMGenericPriceRequest, clientDetails.Item3, String.Join("", "SM", clientDetails.Item1), request);
                        //qMsg.UserID = clientDetails.Item1;
                        qMsg.HashCode = clientDetails.Item2;
                        qMsg.TradingAccountID = clientDetails.Item3;
                        qMsg.RequestID = requestId;
                        _queueCommMgrOut.SendMessage(qMsg);
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

    }
}
