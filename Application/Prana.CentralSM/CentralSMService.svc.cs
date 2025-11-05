using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.NewLiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CentralSM.BLL;
using Prana.Global;
using Prana.Interfaces;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prana.CentralSM
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CentralSMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CentralSMService.svc or CentralSMService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class CentralSMService : ICentralSMService
    {
        static CentralSMService()
        {
            try
            {
                long symbolPK = CentralSMDataManager.GetMaxSymbolPKIDFromDB();
                SecurityMasterSymbolIDGenerator.SetMaxGeneratedIDFromDB(symbolPK);
                CentralSMDataCache.CentralSMCacheManager.Instance.FillCachedData();
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

        internal event ObjectsHandler IsAliveEvent;
        PranaBinaryFormatter _binaryFormatter = new PranaBinaryFormatter();
        //ObjectParamEventArg
        ///// <summary>
        ///// Is invoked from CentralSMHost indirectly. Used to clean cache depending upon it.
        ///// </summary>
        //internal event StringHandler ClientDisconnected;

        public bool IsAlive(string clientName, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback)
        {
            try
            {
                if (securityCallback == null)
                {
                    securityCallback = OperationContext.Current.GetCallbackChannel<ICentralSMSecurityCallback>();
                }
                if (IsAliveEvent != null)
                    IsAliveEvent(new Object[]{clientName,securityCallback});
                return true;
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
            return true;
        }

        public void Disconnect(object sender, EventArgs<string> e)
        {
            try
            {
                //if (ClientDisconnected != null)
                //    ClientDisconnected(clientName);
                string clientName= e.Value;
                CallbackCache.Instance.ClientDisconnected(clientName);
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

        /// <summary>
        /// Gets the pricing data for historical dates for certain fields after checking in cache and DB.
        /// The validation of symbols already done in trade server so no validation of securities required here.
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="fields"></param>
        /// <param name="secMasterReqObj"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="securityCallback"></param>
        public void GetGenericSMPrice(string requestID, String pricingSource, List<string> fields, SecMasterRequestObj secMasterReqObj, DateTime startDate, DateTime endDate, string clientName, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback, bool isGetDataFromCacheOrDB)
        {
            try
            {
                if (securityCallback == null)
                {
                    securityCallback = OperationContext.Current.GetCallbackChannel<ICentralSMSecurityCallback>();
                }
                if (ApplicationConstants.LoggingEnabled)
                {
                    Logger.Write("Historical pricing generic request to be sent to central SM. PricingSource:"+pricingSource+ ", Fields: " + String.Join(",", fields) + Environment.NewLine + "StartDate : " + startDate.ToString() + " EndDate : " + endDate.ToString() + Environment.NewLine
                        + " Requested Symbols : " + String.Join(",", secMasterReqObj.GetPrimarySymbols()));
                }
                string justToInitializeCentralSMManager=CentralSMManager.Instance.ConnectionString;
                CallbackCache.Instance.AddInPricingSubscriber(requestID,clientName, securityCallback);
                PricingRequestManager.Instance.CreateInitialMappingsForPricingRequestAndProcess(requestID, pricingSource, fields, secMasterReqObj, startDate.Date, endDate.Date, isGetDataFromCacheOrDB);

                if (PricingRequestManager.Instance.PricingRequestInProcess[requestID].IsFull)
                {
                    SendHistoricalGenericDataResponseForDataFoundLocally(requestID);
                }
                else
                {
                    PricingRequestManager.Instance.GenerateAndSendCentralSMCalls(requestID);
                }
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

        private void SendHistoricalGenericDataResponseForDataFoundLocally(string requestIdReturned)
        {
            try
            {
                List<KeyValuePair<string, PricingRequestMappings>> requests = PricingRequestManager.Instance.PricingRequestInProcess.Where(x => x.Value.RequestID.Equals(requestIdReturned)).ToList();
                IEnumerable<KeyValuePair<string, PricingRequestMappings>> requestsBBIds = PricingRequestManager.Instance.PricingRequestInProcess.Where(x => x.Value.BBRequestIds.ContainsKey(requestIdReturned) || x.Value.BBRequestIdsInProcess.ContainsKey(requestIdReturned));
                requests.AddRange(requestsBBIds);
                if (requests.Count() == 0)
                    return;
                foreach (KeyValuePair<string, PricingRequestMappings> reqKvp in requests)
                {
                    if (reqKvp.Value.BBRequestIds.Count == 0 && reqKvp.Value.BBRequestIdsInProcess.Count == 0)
                    {
                        ICentralSMSecurityCallback callBack = CallbackCache.Instance.GetAndRemoveHistPricingSubscribersList(reqKvp.Key).Item1;
                        if (callBack != null)
                        {
                            try
                            {
                                callBack.GenricPricingResp(reqKvp.Key, reqKvp.Value.ResponseTable,true,"Pricing fetched from Central SM database.");
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
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region ICentralSMService Members

        public void GetSecMasterDataCentralSM(SecMasterRequestObj secMasterReqObj, string clientName, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback)
        {
            try
            {
                if (securityCallback == null)
                {
                    securityCallback = OperationContext.Current.GetCallbackChannel<ICentralSMSecurityCallback>();
                }
                CallbackCache.Instance.AddInSecuritySubscriber(secMasterReqObj, securityCallback,clientName);
                CentralSMManager.Instance.GetSecMasterData(secMasterReqObj);
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
            //List<SecMasterBaseObj> secMasterCachData = CentralSMDataCache.Instance.GetSecMasterData(secMasterReqObj);
        }


        public void SaveSecMasterDataCentralSM(SecMasterbaseList secMasterData, String ResquestID, String UserID, string clientName, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback)
        {
            //TODO
            try
            {
                if (securityCallback == null)
                {
                    securityCallback = OperationContext.Current.GetCallbackChannel<ICentralSMSecurityCallback>();
                }
                SecMasterRequestObj secMasterReqObj = new SecMasterRequestObj();
                foreach (SecMasterBaseObj secMasterbaseObj in secMasterData)
                {
                    secMasterReqObj.AddData(secMasterbaseObj.TickerSymbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                }

                //TODO - Need to figure out data contract issue. while receiving data from Server ResquestID is null.
                secMasterData.RequestID = ResquestID;
                secMasterData.UserID = UserID;
                CallbackCache.Instance.AddSaveSecurityCompletedCallBack(secMasterData.RequestID, securityCallback);
                CallbackCache.Instance.AddInSecuritySubscriber(secMasterReqObj, securityCallback,clientName);

                CentralSMManager.Instance.SaveNewSymbolToSecurityMaster(secMasterData);
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



        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolLookupReqObj"></param>
        /// <param name="securityCallback"></param>
        public void SearchSecurity(SymbolLookupRequestObject symbolLookupReqObj, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback)
        {
            try
            {
                if (securityCallback == null)
                {
                    securityCallback = OperationContext.Current.GetCallbackChannel<ICentralSMSecurityCallback>();
                }
                CentralSMManager.Instance.GetSymbolLookupRequestedData(symbolLookupReqObj, securityCallback);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="securityCallback"></param>
        public void SaveFutureRootData(DataTable dt, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback)
        {
            try
            {
                if (securityCallback == null)
                {
                    securityCallback = OperationContext.Current.GetCallbackChannel<ICentralSMSecurityCallback>();
                }
                CentralSMManager.Instance.SaveFutureRootData(dt, securityCallback);
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


        public void GetFutureRootData()
        {

            try
            {
                ICentralSMSecurityCallback securityCallback = OperationContext.Current.GetCallbackChannel<ICentralSMSecurityCallback>();
                CentralSMManager.Instance.GetFutureRootData(securityCallback);
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

        
        /// <summary>
        /// We can use this to handle any type of request from Subscriber (trade server)
        /// omshiv
        /// </summary>
        /// <param name="RequestKey"></param>
        /// <param name="Data"></param>
        /// <param name="securityCallback"></param>
        public void GenericeSendDataToCSM(string RequestKey, object Data, string clientName, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback)
        {
            if (securityCallback == null)
            {
                securityCallback = OperationContext.Current.GetCallbackChannel<ICentralSMSecurityCallback>();
            }

            switch (RequestKey)
            {
                case CustomFIXConstants.MSG_SECMASTER_SaveREQ:
                    
                   String message = Data as string;

                  
                    if (message != null)
                    {
                        SecMasterbaseList secMasterData = (SecMasterbaseList)_binaryFormatter.DeSerialize(message);
                        
                        SecMasterRequestObj secMasterReqObj = new SecMasterRequestObj();
                        foreach (SecMasterBaseObj secMasterbaseObj in secMasterData)
                        {
                            secMasterReqObj.AddData(secMasterbaseObj.TickerSymbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                        }

                        //TODO - Need to figure out data contract issue. while receiving data from Server ResquestID is null.
                        //secMasterData.RequestID = ResquestID;
                        //secMasterData.UserID = UserID;
                        CallbackCache.Instance.AddSaveSecurityCompletedCallBack(secMasterData.RequestID, securityCallback);
                        CallbackCache.Instance.AddInSecuritySubscriber(secMasterReqObj, securityCallback, clientName);

                        CentralSMManager.Instance.SaveNewSymbolToSecurityMaster(secMasterData);
       
                    }
                    break;

                case CustomFIXConstants.MSG_SECMASTER_UDA_Save:
                    String UDAdataString = Data as string;
                    if (UDAdataString != null)
                    {
                        Dictionary<String, Dictionary<String, object>> udaDataCol = _binaryFormatter.DeSerialize(UDAdataString) as Dictionary<String, Dictionary<String, object>>;
                        CentralSMManager.Instance.SaveUDAAttributesData(udaDataCol);
                        securityCallback.GenericCentralSMResponse(CustomFIXConstants.MSG_SECMASTER_UDA_Save, UDAdataString);
                    }
                        break;
                    

            }
        }
       
        #endregion
    }
}
