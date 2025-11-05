using Prana.CentralSMDataCache;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BlpDLWSAdapter;
using Prana.BusinessObjects.NewLiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.Utilities.StringUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.CentralSM.BLL
{
    sealed class PricingRequestManager
    { 
        #region singleton

        private static volatile PricingRequestManager instance;
        private static object syncRoot = new Object();

        private PricingRequestManager() { }

        public static PricingRequestManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PricingRequestManager();
                    }
                }

                return instance;
            }
        }
        #endregion

        ConcurrentDictionary<string, PricingRequestMappings> _pricingRequestInProcess = new ConcurrentDictionary<string, PricingRequestMappings>();

        public ConcurrentDictionary<string, PricingRequestMappings> PricingRequestInProcess
        {
            get { return _pricingRequestInProcess; }
            set { _pricingRequestInProcess = value; }
        }

        internal void CreateInitialMappingsForPricingRequestAndProcess(string requestID, String pricingSource, List<string> fields, SecMasterRequestObj secMasterReqObj, DateTime startDate, DateTime endDate, bool isGetDataFromCacheOrDB)
        {
            try
            {
                PricingRequestMappings pricingRequestData = new PricingRequestMappings(requestID, pricingSource, fields, secMasterReqObj, startDate, endDate, null,isGetDataFromCacheOrDB);
                PricingRequestInProcess.TryAdd(requestID, pricingRequestData);
                if (isGetDataFromCacheOrDB)
                {
                PricingDataCacheAndManager.Instance.FillDataFromPricingCacheAndDB(pricingRequestData);
                }

                //Prana.Global.ApplicationConstants.SecurityMasterDataSource.
                //}Prana.BusinessObjects.AppConstants.DataSourceType 
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
        /// Generates requests for Bloomberg for the remaining data not found in database and the cache.
        /// Any changes in this function should also be done in the analogous function in the trade server.
        /// </summary>
        /// <param name="requestID"></param>
        internal void GenerateAndSendCentralSMCalls(string requestID)
        {
            try
            {
                if (PricingRequestInProcess.ContainsKey(requestID))
                {
                    foreach (DataRow dr in PricingRequestInProcess[requestID].ResponseTable.Rows)
                    {
                        foreach (DataColumn col in PricingRequestInProcess[requestID].ResponseTable.Columns)
                        {
                            if (!PricingRequestInProcess[requestID].FieldNames.Contains(col.ColumnName))
                                continue;
                            if (dr[col] != null && !String.IsNullOrWhiteSpace(dr[col].ToString()))
                                continue;
                            #region check whether data already requested to Bloomberg in previous requests
                            bool requestFound = false;
                            foreach (PricingRequestMappings request in PricingRequestInProcess.Values)
                            {
                                if (request.RequestID != requestID)
                                {
                                    List<PricingRequestMappings> tempBBReqs = new List<PricingRequestMappings>(request.BBRequestIds.Values.Where(y => (y.FieldNames.Contains(col.ColumnName)) && y.RequestObj.SymbolDataRowCollection.Exists(z => String.Compare(z.Symbol_PK.ToString(), dr["SymbolPK"].ToString(), true) == 0) && (DateTime.Parse(dr["Date"].ToString()).Date >= y.StartDate.Date) && (DateTime.Parse(dr["Date"].ToString()).Date <= y.EndDate.Date) && dr["SecondarySource"].ToString().Equals(y.SecondaryPricingSource)));
                                if (tempBBReqs.Count > 0)
                                {
                                    PricingRequestInProcess[requestID].BBRequestIdsInProcess.AddOrUpdate(tempBBReqs.First().RequestID, tempBBReqs.First(), (x, y) => tempBBReqs.First());
                                    requestFound = true;
                                    break;
                                }
                            }
                            }
                            if (requestFound)
                                continue;
                            #endregion
                            //Following filters the requests which have the same field and the date as the current row.
                            List<PricingRequestMappings> listBBMatching = new List<PricingRequestMappings>(PricingRequestInProcess[requestID].BBRequestIds.Values.Where(x => (x.FieldNames.Contains(col.ColumnName)) && (DateTime.Parse(dr["Date"].ToString()).Date >= x.StartDate.Date) && (DateTime.Parse(dr["Date"].ToString()).Date <= x.EndDate.Date)));
                            if (listBBMatching.Count == 0)
                            {
                                SecMasterRequestObj bbReqObj = new SecMasterRequestObj();
                                ApplicationConstants.SymbologyCodes bbSymbology;
                                if (!Enum.TryParse<ApplicationConstants.SymbologyCodes>(dr["Symbology"].ToString(), out bbSymbology))
                                    continue;
                                String secondarySource = dr["SecondarySource"].ToString();
                                bbReqObj.AddData(dr["Symbol"].ToString(), bbSymbology, long.Parse(dr["SymbolPK"].ToString()));
                                PricingRequestMappings bbRequest = new PricingRequestMappings(Guid.NewGuid().ToString(), secondarySource, new List<string>() { col.ColumnName }, bbReqObj, DateTime.Parse(dr["Date"].ToString()).Date, DateTime.Parse(dr["Date"].ToString()).Date, null, PricingRequestInProcess[requestID].IsGetDataFromCacheOrDB);
                                PricingRequestInProcess[requestID].BBRequestIds.TryAdd(bbRequest.RequestID, bbRequest);
                            }
                            else
                            {
                                ApplicationConstants.SymbologyCodes bbSymbology;
                                if (!Enum.TryParse<ApplicationConstants.SymbologyCodes>(dr["Symbology"].ToString(), out bbSymbology))
                                    continue;
                                listBBMatching[0].RequestObj.AddData(dr["Symbol"].ToString(), bbSymbology, long.Parse(dr["SymbolPK"].ToString()));
                            }
                        }
                    }
                    PricingRequestInProcess[requestID].MergeBloombergRequestOnFields();

                    foreach (KeyValuePair<string, PricingRequestMappings> bbKvp in PricingRequestInProcess[requestID].BBRequestIds)
                    {
                        if (bbKvp.Value.EndDate.Date == DateTime.Now.Date && bbKvp.Value.StartDate.Date ==  bbKvp.Value.EndDate.Date )
                        {
                        DLWSManager.Instance.GetCurrentMarkprice(bbKvp.Key, bbKvp.Value.SecondaryPricingSource, bbKvp.Value.RequestObj.SymbolDataRowCollection, bbKvp.Value.FieldNames.ToList());
                        }
                        else
                        {

                        DLWSManager.Instance.GetHistoricalMarkprice(bbKvp.Key, bbKvp.Value.SecondaryPricingSource, bbKvp.Value.RequestObj.SymbolDataRowCollection, bbKvp.Value.FieldNames.ToList(), bbKvp.Value.StartDate, bbKvp.Value.EndDate);

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

        /// <summary>
        /// Used to perform the whole task of merging requests and sending to client in case of response from BB
        /// </summary>
        /// <param name="requestIdReturned"></param>
        /// <param name="responseTableFromBloomberg"></param>
        /// <param name="pricingReceived"></param>
        /// <param name="comment"></param>
        public void MergeResponseFromBloombergWithRequestsAndCacheAndMoveForward(string requestIdReturned, DataTable responseTableFromBloomberg, bool pricingReceived, string comment)
        {
            try
            {
                #region mergeWithWaitingRequests
                IEnumerable<KeyValuePair<string, PricingRequestMappings>> requests = PricingRequestInProcess.Where(x => x.Value.BBRequestIds.ContainsKey(requestIdReturned) || x.Value.BBRequestIdsInProcess.ContainsKey(requestIdReturned));
                if (requests==null||requests.Count() == 0)
                    return;
                PricingRequestMappings tempPrcngMapping;
                //checks whether the pricing was received successfully or not, if not then handles the caches to remove the invalidated requests
                //Also sends the invalid response to the client for the pricing data
                if (pricingReceived == false)
                {
                    foreach (KeyValuePair<string, PricingRequestMappings> reqKvp in requests)
                    {
                        reqKvp.Value.BBRequestIds.TryRemove(requestIdReturned, out tempPrcngMapping);
                        reqKvp.Value.BBRequestIdsInProcess.TryRemove(requestIdReturned, out tempPrcngMapping);

                        Tuple<ICentralSMSecurityCallback, string> callBackTuple = CallbackCache.Instance.GetAndRemoveHistPricingSubscribersList(reqKvp.Key);
                        ICentralSMSecurityCallback callBack = null;
                        if (callBackTuple != null)
                        {
                            callBack = callBackTuple.Item1;
                        }
                        if (callBack != null)
                        {
                            try
                            {
                                callBack.GenricPricingResp(reqKvp.Key, null,pricingReceived,comment);
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
                    return;
                }

                foreach (KeyValuePair<string, PricingRequestMappings> reqKvp in requests)
                {
                    IEnumerable<DataColumn> commonFieldColumns = responseTableFromBloomberg.Columns.Cast<DataColumn>().Where(x => reqKvp.Value.FieldNames.Contains(x.ColumnName));
                    commonFieldColumns = commonFieldColumns.Where(x => reqKvp.Value.ResponseTable.Columns.Contains(x.ColumnName));
                    if (commonFieldColumns.Count() > 0)
                    {
                        foreach (DataRow responseRow in responseTableFromBloomberg.Rows)
                        {
                            EnumerableRowCollection<DataRow> matchingDataRows = reqKvp.Value.ResponseTable.AsEnumerable().Where(x => String.Compare(x["Symbol"].ToString(),responseRow["Symbol"].ToString(),true)==0 && String.Compare(x["Symbology"].ToString() , responseRow["Symbology"].ToString(),true)==0 && DateTime.Parse(x["Date"].ToString()).Date == DateTime.Parse(responseRow["Date"].ToString()).Date);
                            foreach (DataRow matchRow in matchingDataRows)
                            {
                                foreach (DataColumn dc in commonFieldColumns)
                                {
                                    if (matchRow[dc.ColumnName] == null || matchRow[dc.ColumnName] == DBNull.Value || String.IsNullOrWhiteSpace(matchRow[dc.ColumnName].ToString()))
                                    {
                                        if (responseRow[dc.ColumnName] != null && responseRow[dc.ColumnName] != DBNull.Value&& !String.IsNullOrWhiteSpace(responseRow[dc.ColumnName].ToString()))
                                            matchRow[dc.ColumnName] = responseRow[dc.ColumnName];
                                    }
                                }
                            }
                        }
                    }
                    reqKvp.Value.BBRequestIds.TryRemove(requestIdReturned, out tempPrcngMapping);
                    reqKvp.Value.BBRequestIdsInProcess.TryRemove(requestIdReturned, out tempPrcngMapping);
                    //need to check whether all responses received and then sending the response back
                    if (reqKvp.Value.BBRequestIds.Count == 0 && reqKvp.Value.BBRequestIdsInProcess.Count == 0)
                    {
                        Tuple<ICentralSMSecurityCallback,string> callBackTuple = CallbackCache.Instance.GetAndRemoveHistPricingSubscribersList(reqKvp.Key);
                        ICentralSMSecurityCallback callBack = null;
                        if (callBackTuple != null)
                        {
                            callBack = callBackTuple.Item1;
                        }
                        if (callBack != null)
                        {
                            try
                            {
                                callBack.GenricPricingResp(reqKvp.Key, reqKvp.Value.ResponseTable,pricingReceived,comment);
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
                #endregion
                PricingDataCacheAndManager.Instance.UpdateDBandCacheOnResponse(responseTableFromBloomberg);
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
