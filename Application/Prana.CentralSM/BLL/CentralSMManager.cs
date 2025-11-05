using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CentralSM.BLL;
using Prana.Global;
using Prana.Interfaces;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Prana.CentralSM
{
    public class CentralSMManager
    {
        #region singleton
        private static volatile CentralSMManager instance;
        private static object syncRoot = new Object();

        private CentralSMManager()
        {
            try
            {
                //_maxSMReqSize = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("SMUIMaxSizeForHistData").ToString());
                //string histPricing = CachedDataManager.GetInstance.GetPranaPreferenceByKey("UseHistoricalPricing");
                BlpDLWSAdapter.DLWSManager.Instance.SymbolDataResponse += Instance_SymbolDataResponse;
                BlpDLWSAdapter.DLWSManager.Instance.HistoricalDataResponse += Instance_HistoricalDataResponse;
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

        public static CentralSMManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CentralSMManager();
                    }
                        }
                return instance;
                    }
                }
        #endregion singleton

        void Instance_HistoricalDataResponse(object sender, ObjectParamEventArg e)
        {
            try
            {
                Object[] objectsHistorical=e.Arguments as Object[];
                string comment = string.Empty;
                if (objectsHistorical.Length > 3)
                {
                    comment = objectsHistorical[3].ToString();
                }
                DataTable histResponse = objectsHistorical[0] as DataTable;
                string requestIdTobloomberg = objectsHistorical[1].ToString();
                bool pricingReceived = (bool)objectsHistorical[2];

                PricingRequestManager.Instance.MergeResponseFromBloombergWithRequestsAndCacheAndMoveForward(requestIdTobloomberg, histResponse,pricingReceived,comment);
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

        object lockerDbAccess = new object();
        private string _connectionStr = "";
        PranaBinaryFormatter _binaryFormatter = new PranaBinaryFormatter();
        public string ConnectionString
        {
            get { return _connectionStr; }
            set { _connectionStr = value; }
        }

        void Instance_SymbolDataResponse(object sender, Data e)
        {
            try
            {
                //SymbolData responseData = e.Info as SymbolData;
                SecMasterBaseObj responseData = e.SecMasterData as SecMasterBaseObj;
                if (responseData != null)
                {
                    bool alreadyValidated=CentralSMDataCache.CentralSMDataCache.Instance.CheckBbgidAlreadyValidated(responseData.BBGID);
                    if (!alreadyValidated)
                        SnapshotResponseSecMaster(responseData);
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
       
        public void SnapshotResponseSecMaster(SecMasterBaseObj secMasterObj)
        {
            try
            {
                if (SecMasterEnrichData.GetInstance.CheckSMEnRichRequires())
                {
                    SecMasterEnrichData.GetInstance.EnRichSecMasterObject(secMasterObj);
                    SecMasterEnrichData.GetInstance.DeleteSMEnRichCachedData(secMasterObj.TickerSymbol);
                }

                secMasterObj.Symbol_PK = SecurityMasterSymbolIDGenerator.GenerateSymbolPKID();
                //TODO set UDA from bloomberg
                if (secMasterObj.SymbolUDAData == null)
                {
                    secMasterObj.SymbolUDAData = new UDAData();
                    secMasterObj.SymbolUDAData.Symbol = secMasterObj.TickerSymbol;
                }
                SaveNewSymbolToSecurityMaster(secMasterObj);
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
            // throw new Exception("The method or operation is not implemented.");
                        }
                      
        public bool GetUnderLyingSymbolDetails(string UnderLyingSymbol, string connString)
        {
            DataSet ds = new DataSet();
            try
            {

                ds = CentralSMDataManager.GetUnderLyingSymbolDetails(UnderLyingSymbol, connString);

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
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Save Secuirty in DB when data from External Source like e-Signal
        /// </summary>
        /// <param name="secMasterObj"></param>
        private void SaveNewSymbolToSecurityMaster(SecMasterBaseObj secMasterObj)
        {
            try
            {


                if (secMasterObj.AUECID != 0)
                {
                // saving to DB
                Prana.Utilities.XMLUtilities.CustomXmlSerializer _xml = new Prana.Utilities.XMLUtilities.CustomXmlSerializer();
                CentralSMDataManager.SaveNewSymbolResponsetoSecurityMaster(_xml.WriteString(secMasterObj), _connectionStr);
                }
                //Send Symbol response
                InvokeSecMstrDataResponse(secMasterObj);

                //Add to cache of security master
                CentralSMDataCache.CentralSMDataCache.Instance.AddValuesToSecurityCache(secMasterObj);


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
        /// Get SecMaster Data by SM request Object and specific date
        /// </summary>
        /// <param name="secMasterRequestObj"></param>
        /// <param name="date"></param>
        /// <param name="senderCode"></param>
        public void GetSecMasterData(SecMasterRequestObj secMasterRequestObj)
        {
            try
            {
                //first traverse cache to check if data exist before making a db call..
                List<SecMasterBaseObj> secMasterData = CentralSMDataCache.CentralSMDataCache.Instance.GetSecMasterDataFromCache(secMasterRequestObj);
                // register these symbols for async return 
                //CentralSMDataCache.Instance.RequestSymbolData(secMasterRequestObj);

                foreach (SecMasterBaseObj secMasterBaseObj in secMasterData)
                {
                    //remove symbol from SM request list if symbol found.. 
                    SymbolDataRow datarow = secMasterRequestObj.GetSymbolRow(secMasterBaseObj);
                    if (datarow != null)
                    {
                        secMasterBaseObj.RequestedSymbology = datarow.PrimarySymbology;
                        InvokeSecMstrDataResponse(secMasterBaseObj);
                        secMasterRequestObj.Remove(datarow);
                        //TODO: PKE Dont know what the following does need to clear. Currently copied from trade server side code
                        if (SecMasterEnrichData.GetInstance.CheckSMEnRichRequires())
                        {
                            SecMasterEnrichData.GetInstance.DeleteSMEnRichCachedData(secMasterBaseObj.TickerSymbol);
                        }
                    }
                }

                //if some symbols still exist for which data was not found in cache then make a db call for these..
                if (secMasterRequestObj.Count > 0)
                {
                    // get data from db async
                    BackgroundWorker bkWrkForDbSecValidationAccess = new BackgroundWorker();

                    // bkWrkForDbAccess.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkWrkForDbAccess_RunWorkerCompleted);

                    // if symbol not found in DB then call live feed async
                    bkWrkForDbSecValidationAccess.DoWork += new DoWorkEventHandler(bkWrkForDbSecValidationAccess_DoWork);
                    System.Collections.ArrayList arguments = new System.Collections.ArrayList();
                    arguments.Add(secMasterRequestObj);
                    bkWrkForDbSecValidationAccess.RunWorkerAsync(arguments);
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

        private void InvokeSecMstrDataResponse(SecMasterBaseObj secMasterBaseObj)
        {
            try
            {
                HashSet<ICentralSMSecurityCallback> callbacks = CallbackCache.Instance.GetAndRemoveSecuritySubscribersList(secMasterBaseObj);
                foreach (ICentralSMSecurityCallback callBk in callbacks)
                {
                    try
                    {
                        callBk.SecurityValidationResp(secMasterBaseObj);
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
        /// async database access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bkWrkForDbSecValidationAccess_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                lock (lockerDbAccess)
                {
                    System.Collections.ArrayList arguments = (System.Collections.ArrayList)e.Argument;
                    SecMasterRequestObj secMasterRequestObj = (SecMasterRequestObj)arguments[0];
                    List<SecMasterBaseObj> secMasterObjList = null;
                    if (_connectionStr == string.Empty)
                    {
                        secMasterObjList = CentralSMDataManager.GetSecMasterDataFromDB_XML(secMasterRequestObj);
                    }
                    else
                    {
                        secMasterObjList = CentralSMDataManager.GetSecMasterDataFromDB_XML(secMasterRequestObj, _connectionStr);
                    }

                    //make call to live-feed provider for fetching data Async as data for these symbols was not found in cache or SecmasterDB...
                    //List<SecMasterBaseObj> secMasterObjList = SecMasterDataManager.GetSecMasterDataFromDB_XML_Date(secMasterRequestObj, dateTime);
                    if (secMasterObjList.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterBaseObj in secMasterObjList)
                        {
                            SymbolDataRow datarow = secMasterRequestObj.GetSymbolRow(secMasterBaseObj);
                            if (datarow != null)
                            {
                                secMasterBaseObj.RequestedSymbology = datarow.PrimarySymbology;
                                InvokeSecMstrDataResponse(secMasterBaseObj);
                                secMasterRequestObj.Remove(datarow);
                                // remove symbols from table if exists in the sec master db
                                if (SecMasterEnrichData.GetInstance.CheckSMEnRichRequires())
                                {
                                    SecMasterEnrichData.GetInstance.DeleteSMEnRichCachedData(secMasterBaseObj.TickerSymbol);
                                }
                            }
                            //#region Compliance Section

                            //try
                            //{
                            //    //Sending newly requested symbol to esper
                            //    if (secMasterBaseObj.RequestedSymbology == Convert.ToInt32(ApplicationConstants.SymbologyCodes.TickerSymbol))
                            //    {
                            //        if (SecurityObjectReceived != null)
                            //            SecurityObjectReceived(secMasterBaseObj);
                            //        //Checking if udacollection not already contains same ticker symbol and compliance is enabled
                            //        //http://jira.nirvanasolutions.com:8080/browse/PRANA-2456
                            //        //if (UDADataReceived != null && !udaCollection.ContainsKey(secMasterBaseObj.TickerSymbol))
                            //        //    udaCollection.Add(secMasterBaseObj.TickerSymbol, CachedDataManager.GetInstance.GetUDAData(secMasterBaseObj.TickerSymbol));
                            //        if (AuecDetailsUpdated != null)
                            //            AuecDetailsUpdated(SecMasterDataCache.GetInstance.GetAuecDetails(secMasterBaseObj.AUECID));
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    // Invoke our policy that is responsible for making sure no secure information
                            //    // gets out of our layer.
                            //    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                            //    if (rethrow)
                            //    {
                            //        throw;
                            //    }
                            //}
                            //#endregion
                        }

                        CentralSMDataCache.CentralSMDataCache.Instance.AddValuesToSecurityCache(secMasterObjList);
                    }

                    // modified by omshiv, request will sent to BB if IsSearchInLocalOnly is false in request object.
                    // in some case,we just want to check data exist in our system or not. so in such scenario we will not sent request to BB api.
                    if (secMasterRequestObj.Count > 0 && !secMasterRequestObj.IsSearchInLocalOnly)
                    {
                        SecMasterRequestObj SecMasterReqOld = (SecMasterRequestObj)secMasterRequestObj.Clone();
                        //bool isTransaformationRequired = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsSymbolTransformationRequired"));
                        //if (isTransaformationRequired)
                        //{
                        //    SymbolTransformer.TransformSymbol(secMasterRequestObj);
                        //}
                        // //below is used to subscribe for the following symbols
                        //CentralSMDataCache.Instance.RequestSymbolData(SecMasterReqOld, secMasterRequestObj);
                        //Pricing service not used here only direct connection to Bloomberg done right now
                        //if (_pricingServicesProxy.IsServerConnected())
                        //{
                        GetSecMasterDataFromLiveFeed(secMasterRequestObj);
                        //}
                        //else
                        //{
                        //    throw new Exception("Pricing Server not connected.");
                        //}
                    }
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

        /// <summary>
        /// Validates the symbols from Bloomberg
        /// </summary>
        /// <param name="secMasterRequestObj">SecmasterReqObj containing the requested symbols. Uses primary Symbol for validation</param>
        private void GetSecMasterDataFromLiveFeed(SecMasterRequestObj secMasterRequestObj)
        {
            try
            {
                Prana.BlpDLWSAdapter.DLWSManager.Instance.GetSymbolData(secMasterRequestObj.SymbolDataRowCollection);
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

        /// <summary>
        /// Update Sec master object for e-signal security and SM import security from their underlying and set preferences.
        /// </summary>
        /// <param name="secMasterObj"></param>
        private void UpdateSecMasterObject(SecMasterBaseObj secMasterObj)
        {
            try
            {
                //Set Preference of Auto approve security
                bool isAutoApprove = false;
                bool.TryParse(CentralSMDataCache.CentralSMCacheManager.Instance.GetPranaPreferenceByKey("IsSecurityAutoApproved"), out isAutoApprove);
                secMasterObj.IsSecApproved = isAutoApprove;

                AssetCategory assetCategory = (AssetCategory)secMasterObj.AssetID;

                if (assetCategory == AssetCategory.Future || assetCategory == AssetCategory.FutureOption)
                {
                    if (secMasterObj.Multiplier == 0)
                    {
                        secMasterObj.Multiplier = 1;
                    }
                    //TODO: PKE set UDA related dat in case of futures and the multiplier
                    // merge some secMaster details and UDA for futures  
                    //CentralSMDataCache.Instance.SetExtraFromCache(secMasterObj);
                }
                else if (assetCategory == AssetCategory.EquityOption && secMasterObj.Multiplier == 0)
                {
                    secMasterObj.Multiplier = 100;
                    //TODO: PKE Set some default code for setting multiplier
                    //secMasterObj.Multiplier = CachedDataManager.GetInstance.GetMultiplierByAUECID(secMasterObj.AUECID);

                }

                //here Security comes from import SM or from E-signal, then merge UDA from their underlying.

                if ((secMasterObj.SymbolUDAData == null)
                    ||
                    (
                    secMasterObj.SymbolUDAData.AssetID == int.MinValue
                    && secMasterObj.SymbolUDAData.SectorID == int.MinValue
                    && secMasterObj.SymbolUDAData.SubSectorID == int.MinValue
                    && secMasterObj.SymbolUDAData.CountryID == int.MinValue
                    && secMasterObj.SymbolUDAData.SecurityTypeID == int.MinValue
                    )
                )
                {
                    //in case if UDA not found, then initialize with default values
                    secMasterObj.SymbolUDAData = new UDAData();
                    secMasterObj.SymbolUDAData.Symbol = secMasterObj.TickerSymbol;

                    //get uda from underlyig symbol if ticker is not same as underlying symbol
                    //om shiv, March 2014
                    //TODO: PKE set soem defaultUDA in centralDB
                    //if (!secMasterObj.TickerSymbol.Equals(secMasterObj.UnderLyingSymbol))
                    //{
                    //    secMasterObj.UseUDAFromUnderlyingOrRoot = true;
                    //    GetUDASymbolDataOfUnderlying(secMasterObj);
                    //}
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

        /// <summary>
        /// save future root data add/edit from root ui 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="userid"></param>
        /// <param name="requestid"></param>
        public void SaveFutureRootData(DataTable dt, ICentralSMSecurityCallback securityCallback)
        {
            try
            {
                string result = String.Empty;
                //updating root data in future root cache and update in SM cache - omshiv,nov 2013
                SecMasterbaseList updatedFutureSymbols = CentralSMDataCache.CentralSMDataCache.Instance.UpdateFutureSymbolsFromRootSymbol(dt);
                //if (updatedFutureSymbols.Count > 0)
                //{
                //    // send back updated symbols to clients for client cache
                //    SecMasterServerComponent.GetInstance.SendDataToClient(updatedFutureSymbols);
                //    PublishSecurityMasterData(updatedFutureSymbols);
                //}
                result = CentralSMDataManager.SaveFutureRootData(dt);

                // send the status back to respective user And Module
                if (result.Equals("Success"))
                {
                    securityCallback.SaveFutureRootDataLocal(dt);
                }
            }
            catch (Exception ex)
            {
                // invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void GetSymbolLookupRequestedData(SymbolLookupRequestObject symbolLookupReqObj, ICentralSMSecurityCallback securityCallback)
        {
            try
            {
                if (symbolLookupReqObj != null)
                {
                    // get data from db async
                    BackgroundWorker bkWrkGetSymbolForDbAccess = new BackgroundWorker();
                    bkWrkGetSymbolForDbAccess.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkWrkSymbolForDbAccess_RunWorkerCompleted);
                    bkWrkGetSymbolForDbAccess.DoWork += new DoWorkEventHandler(bkWrkGetSymbolForDbAccess_DoWork);
                    System.Collections.ArrayList arguments = new System.Collections.ArrayList();
                    arguments.Add(symbolLookupReqObj);
                    arguments.Add(securityCallback);
                    
                    bkWrkGetSymbolForDbAccess.RunWorkerAsync(arguments);
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
        /// <summary>
        /// Send requested SM data to symbol lookup UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bkWrkSymbolForDbAccess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }
            object[] result = new object[2];
            result = (object[])e.Result;
            DataSet _ds = (DataSet)result[0];
            ICentralSMSecurityCallback callBk = (ICentralSMSecurityCallback)result[1];
            SymbolLookupRequestObject symbolLookupRequestObject = (SymbolLookupRequestObject)result[2];
            try
            {

                if (!e.Cancelled) // no error
                {


                     callBk.SymbolLookUpSearchDataResp(_ds,symbolLookupRequestObject);



                    //if (StatusOfRequest != null)
                    //{
                    //    SymbolLkUpDataResponse(_ds, symbolLookupReqObj);
                    //}
                    //if (StatusOfRequest != null && _ds.Tables[0].Rows.Count > 0)
                    //{

                    //    StatusOfRequest("Success", symbolLookupReqObj.CompanyUserID, symbolLookupReqObj.RequestID);
                    //}
                    //else
                    //{
                    //    StatusOfRequest("No Data found", symbolLookupReqObj.CompanyUserID, symbolLookupReqObj.RequestID);
                    //}
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                //if (StatusOfRequest != null)
                //{
                //    StatusOfRequest(ex.Message, symbolLookupReqObj.CompanyUserID, symbolLookupReqObj.RequestID);
                //}
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
        /// Getting SM data from DB when request from Symbol lookup UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bkWrkGetSymbolForDbAccess_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSet _ds = new DataSet();
            System.Collections.ArrayList arguments = (System.Collections.ArrayList)e.Argument;
            SymbolLookupRequestObject symbolLookupReqObj = (SymbolLookupRequestObject)arguments[0];
            ICentralSMSecurityCallback callBk = (ICentralSMSecurityCallback)arguments[1];

            object[] result = new object[3];
            try
            {

                Prana.Utilities.XMLUtilities.CustomXmlSerializer _xml = new Prana.Utilities.XMLUtilities.CustomXmlSerializer();
                _ds = CentralSMDataManager.GetSymbolLookupRequestedData(_xml.WriteString(symbolLookupReqObj));

                result[0] = _ds;
                result[1] = callBk;
                result[2] = symbolLookupReqObj;

                e.Result = result;
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                e.Result = ex.Message;
                //if (StatusOfRequest != null)
                //{
                //    StatusOfRequest("Problem on Server", symbolLookupReqObj.CompanyUserID, symbolLookupReqObj.RequestID);
                //}
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
        /// Save updated or new security in DB 
        /// Modified by om, oct,2013
        /// </summary>
        /// <param name="secMasterData"></param>
        public void SaveNewSymbolToSecurityMaster(SecMasterbaseList secMasterData)
        {
            try
            {
                if (secMasterData != null)
                {
                    //if a new future symbol is added from symbol look up, to save the future multiplier as saved in future root data

                     //Saving with back ground worker 
                    BackgroundWorker bw_SaveSecMasterInDB = new BackgroundWorker();
                    bw_SaveSecMasterInDB.DoWork += new DoWorkEventHandler(bw_SaveSecMasterInDB_DoWork);
                    bw_SaveSecMasterInDB.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_SaveSecMasterInDB_RunWorkerCompleted);
                    ArrayList arguments = new System.Collections.ArrayList();
                    arguments.Add(secMasterData);
                    bw_SaveSecMasterInDB.RunWorkerAsync(arguments);


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

        /// <summary>
        /// Save SecMAster to DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bw_SaveSecMasterInDB_DoWork(object sender, DoWorkEventArgs e)
        {
            ArrayList arguments = (System.Collections.ArrayList)e.Argument;
            SecMasterbaseList secMasterData = (SecMasterbaseList)arguments[0];
            try
            {
                //Saving Data in chunks 
                //int chunkSize = 20;
                //TODO 
                //List<SecMasterbaseList> chunkList = ChunkingManager.CreateChunksForObjList(secMasterData, chunkSize);
                // foreach (SecMasterbaseList chunk in chunkList)
                // {
                Prana.Utilities.XMLUtilities.CustomXmlSerializer _xml = new Prana.Utilities.XMLUtilities.CustomXmlSerializer();
                String XmlString = _xml.WriteString(secMasterData);
                CentralSMDataManager.SaveNewSymbolResponsetoSecurityMaster(XmlString, _connectionStr);
                //  }

                e.Result = secMasterData;

            }
            catch (Exception ex)
            {
                e.Cancel = true;
                e.Result = ex.Message;

                //TODO Omshiv
                //if (StatusOfRequest != null)
                //{
                //    if (ex.Message.Contains("Cannot insert duplicate key row") || ex.Message.Contains("The INSERT statement conflicted with the FOREIGN KEY constraint"))
                //    {
                //        StatusOfRequest("Not Saved,Symbol already exist", secMasterData.UserID, secMasterData.RequestID);
                //    }
                //    else
                //    {
                //        StatusOfRequest(ex.Message, secMasterData.UserID, secMasterData.RequestID);
                //    }
                //}

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
        /// Save SecMAster from symbol look up to DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bw_SaveSecMasterInDB_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled)
                {
                    SecMasterbaseList secMasterData = e.Result as SecMasterbaseList;
                    if (secMasterData != null)
                    {


                        foreach (SecMasterBaseObj secMasterobj in secMasterData)
                        {
                            //upadate UDA data with name from UDA IDS
                            // CentralSMDataCache.CentralSMDataCache.Instance.UpdateUDADataWithName(secMasterobj);

                            InvokeSecMstrDataResponse(secMasterobj);
                        }

                    }

                    ICentralSMSecurityCallback callBack = CallbackCache.Instance.GetSaveSecurityCompletedCallBack(secMasterData.RequestID);
                    if (callBack != null)
                    {
                        string responseType = CustomFIXConstants.MSG_SECMASTER_SaveREQ;

                        Tuple<string, string, string> response = new Tuple<string, string, string>("Success", secMasterData.UserID, secMasterData.RequestID);
                        string message = _binaryFormatter.Serialize(response);
                        callBack.GenericCentralSMResponse(responseType, message);
                    }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityCallback"></param>
        internal void GetFutureRootData(ICentralSMSecurityCallback securityCallback)
        {
            try
            {
                DataSet futureData=  CentralSMDataManager.GetFutureRootData(_connectionStr);
                securityCallback.FutureRootDataResp(futureData);
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
 
        //UDA attribute IN CSM DB
        internal void SaveUDAAttributesData(Dictionary<string, Dictionary<string, object>> udaDataCol)
        {

            try
            {
                CentralSMDataCache.CentralSMCacheManager.Instance.SaveUDAAttributesData(udaDataCol);
                
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
    }
}

