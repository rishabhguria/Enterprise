using Prana.BusinessObjects.NewLiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.SecurityMasterNew.BLL
{
    sealed class SecMasterPricingReqManagerServer
    {
        #region singleton

        private static volatile SecMasterPricingReqManagerServer instance;
        private static readonly object syncRoot = new Object();

        private SecMasterPricingReqManagerServer() { }

        public static SecMasterPricingReqManagerServer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SecMasterPricingReqManagerServer();
                    }
                }

                return instance;
            }
        }
        #endregion

        #region Mapping of Client Requests on SecMasterServerComponent

        /// <summary>
        /// Dictionary of requestID, clientuserID, clientHashcode, tradingAccountID. Used when the pricing data request comes from client side to server through socket
        /// </summary>
        ConcurrentDictionary<string, Tuple<string, int, string>> _pricingReqMappingSecMasterServer = new ConcurrentDictionary<string, Tuple<string, int, string>>();

        /// <summary>
        /// Dictionary of requestID, clientuserID, clientHashcode, tradingAccountID. Used when the pricing data request comes from client side to server through socket
        /// </summary>
        public ConcurrentDictionary<string, Tuple<string, int, string>> PricingReqMappingSecmasterServer
        {
            get { return _pricingReqMappingSecMasterServer; }
            set { _pricingReqMappingSecMasterServer = value; }
        }

        /// <summary>
        /// Dictionary of requestID, clientuserID, clientHashcode, tradingAccountID. Used when the pricing data request comes from client side to server through socket
        /// </summary>
        /// <param name="reqID"></param>
        /// <param name="clientUserID"></param>
        /// <param name="clientHashCode"></param>
        /// <param name="tradingAccountID"></param>
        internal void AddGenericPriceRequestFromClient(string reqID, string clientUserID, int clientHashCode, string tradingAccountID)
        {
            try
            {
                PricingReqMappingSecmasterServer.TryAdd(reqID, new Tuple<string, int, string>(clientUserID, clientHashCode, tradingAccountID));
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

        internal Tuple<string, int, string> GetGenericPriceRequestDataForClient(string requestId)
        {
            try
            {
                Tuple<string, int, string> details;
                if (PricingReqMappingSecmasterServer.TryRemove(requestId, out details))
                    return details;
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
            return null;
        }
        #endregion Mapping of Client Requests on SecMasterServerComponent

        #region Mapping of requests for pricing data on SecMasterCacheManager

        ConcurrentDictionary<string, PricingRequestMappings> _pricingRequestInProcessSecmasterCacheManager = new ConcurrentDictionary<string, PricingRequestMappings>();

        public ConcurrentDictionary<string, PricingRequestMappings> PricingRequestInProcessSecmasterCacheManager
        {
            get { return _pricingRequestInProcessSecmasterCacheManager; }
            set { _pricingRequestInProcessSecmasterCacheManager = value; }
        }

        #endregion

        internal void CreateInitialMappingsForPricingRequestAndProcess(string requestID, string secondaryPricingSource, ConcurrentBag<string> fields, SecMasterRequestObj secMasterReqObj, DateTime startDate, DateTime endDate, Action<string, System.Data.DataTable, bool, string> functionForResponse, bool isGetDataFromCacheOrDB)
        {
            try
            {
                PricingRequestMappings pricingRequestData = new PricingRequestMappings(requestID, secondaryPricingSource, fields, secMasterReqObj, startDate, endDate, functionForResponse, isGetDataFromCacheOrDB);
                PricingRequestInProcessSecmasterCacheManager.TryAdd(requestID, pricingRequestData);
                if (isGetDataFromCacheOrDB)
                {
                    SecmasterPricingDataCacheAndManager.Instance.FillDataFromPricingCacheAndDB(pricingRequestData);
                }
                //Prana.Global.ApplicationConstants.SecurityMasterDataSource.
                //}Prana.BusinessObjects.AppConstants.DataSourceType 
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
        /// Generates requests to Central SM for the remaining data which is not available in db and cache.
        /// Any changes in this function should also be done in the analogous function in the central SM server
        /// </summary>
        /// <param name="requestID"></param>
        internal void GenerateAndSendCentralSMCalls(string requestID)
        {
            try
            {
                if (PricingRequestInProcessSecmasterCacheManager.ContainsKey(requestID))
                {
                    foreach (DataRow dr in PricingRequestInProcessSecmasterCacheManager[requestID].ResponseTable.Rows)
                    {
                        foreach (DataColumn col in PricingRequestInProcessSecmasterCacheManager[requestID].ResponseTable.Columns)
                        {
                            if (!PricingRequestInProcessSecmasterCacheManager[requestID].FieldNames.Contains(col.ColumnName))
                                continue;
                            if (dr[col] != null && !String.IsNullOrWhiteSpace(dr[col].ToString()))
                                continue;
                            if (FindAlreadyRequested(dr, col, requestID))
                                continue;

                            //Following filters the requests which have the same field and the date as the current row.
                            List<PricingRequestMappings> listBBMatching = new List<PricingRequestMappings>(PricingRequestInProcessSecmasterCacheManager[requestID].BBRequestIds.Values.Where(x => (x.FieldNames.Contains(col.ColumnName)) && (DateTime.Parse(dr["Date"].ToString()).Date >= x.StartDate.Date) && (DateTime.Parse(dr["Date"].ToString()).Date <= x.EndDate.Date)));
                            if (listBBMatching.Count == 0)
                            {
                                SecMasterRequestObj bbReqObj = new SecMasterRequestObj();
                                ApplicationConstants.SymbologyCodes bbSymbology;
                                if (!Enum.TryParse<ApplicationConstants.SymbologyCodes>(dr["Symbology"].ToString(), out bbSymbology))
                                    continue;
                                bbReqObj.AddData(dr["Symbol"].ToString(), bbSymbology, long.Parse(dr["SymbolPK"].ToString()));
                                String pricingSecondarySource = dr["SecondarySource"].ToString();
                                PricingRequestMappings bbRequest = new PricingRequestMappings(Guid.NewGuid().ToString(), pricingSecondarySource, new ConcurrentBag<string>() { col.ColumnName }, bbReqObj, DateTime.Parse(dr["Date"].ToString()).Date, DateTime.Parse(dr["Date"].ToString()).Date, null, PricingRequestInProcessSecmasterCacheManager[requestID].IsGetDataFromCacheOrDB);
                                PricingRequestInProcessSecmasterCacheManager[requestID].BBRequestIds.TryAdd(bbRequest.RequestID, bbRequest);
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
                    PricingRequestInProcessSecmasterCacheManager[requestID].MergeBloombergRequestOnFields();
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
        /// Use to find already requested symbols for specified date and secondary source
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private bool FindAlreadyRequested(DataRow dr, DataColumn col, string requestID)
        {
            try
            {
                foreach (KeyValuePair<string, PricingRequestMappings> kvp in PricingRequestInProcessSecmasterCacheManager)
                {
                    if (kvp.Value.FieldNames.Contains(col.ColumnName)
                        && (DateTime.Parse(dr["Date"].ToString()).Date >= kvp.Value.StartDate.Date)
                        && (DateTime.Parse(dr["Date"].ToString()).Date <= kvp.Value.EndDate.Date)
                        && String.Compare(kvp.Value.SecondaryPricingSource, dr["SecondarySource"].ToString(), true) == 0
                        && kvp.Value.RequestObj.GetPrimarySymbols().Contains(dr["Symbol"].ToString(), StringComparer.InvariantCultureIgnoreCase)
                        )
                    {
                        List<PricingRequestMappings> listBBAlreadyRequestedMatching = new List<PricingRequestMappings>(
                            kvp.Value.BBRequestIds.Values.Where(x => (x.FieldNames.Contains(col.ColumnName))
                                && (DateTime.Parse(dr["Date"].ToString()).Date >= x.StartDate.Date)
                                && (DateTime.Parse(dr["Date"].ToString()).Date <= x.EndDate.Date)
                                && String.Compare(x.SecondaryPricingSource, dr["SecondarySource"].ToString(), true) == 0
                                && x.RequestObj.GetPrimarySymbols().Contains(dr["Symbol"].ToString(), StringComparer.InvariantCultureIgnoreCase)
                                )
                            );
                        if (listBBAlreadyRequestedMatching.Count > 0)
                        {
                            PricingRequestInProcessSecmasterCacheManager[requestID].BBRequestIdsInProcess.AddOrUpdate(listBBAlreadyRequestedMatching[0].RequestID, listBBAlreadyRequestedMatching[0], (x, y) => listBBAlreadyRequestedMatching[0]);
                            return true;
                        }
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
            return false;
        }

        /// <summary>
        /// Used to perform the whole task of merging requests and sending to client in case of response from centralSM
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="pricingTable"></param>
        /// <param name="pricingSuccess"></param>
        /// <param name="comment"></param>
        internal void MergeResponseFromCentralSMWithRequestsAndCacheAndMoveForward(string requestId, DataTable pricingTable, bool pricingSuccess, string comment)
        {
            try
            {
                IEnumerable<KeyValuePair<string, PricingRequestMappings>> requests = PricingRequestInProcessSecmasterCacheManager.Where(x => x.Value.BBRequestIds.ContainsKey(requestId) || x.Value.BBRequestIdsInProcess.ContainsKey(requestId));
                if (requests != null && requests.Count() == 0)
                    return;
                PricingRequestMappings tempPrcngMapping;
                if (pricingSuccess == false)
                {
                    foreach (KeyValuePair<string, PricingRequestMappings> reqKvp in requests)
                    {
                        PricingRequestInProcessSecmasterCacheManager.TryRemove(reqKvp.Key, out tempPrcngMapping);
                        if (reqKvp.Value.BBRequestIds.TryRemove(requestId, out tempPrcngMapping) ||
                        reqKvp.Value.BBRequestIdsInProcess.TryRemove(requestId, out tempPrcngMapping))
                        {
                            reqKvp.Value.ResponseFunctionDelegate(reqKvp.Key, reqKvp.Value.ResponseTable, pricingSuccess, comment);
                        }
                    }
                    return;
                }
                foreach (KeyValuePair<string, PricingRequestMappings> reqKvp in requests)
                {
                    IEnumerable<DataColumn> commonFieldColumns = pricingTable.Columns.Cast<DataColumn>().Where(x => reqKvp.Value.FieldNames.Contains(x.ColumnName));
                    commonFieldColumns = commonFieldColumns.Where(x => reqKvp.Value.ResponseTable.Columns.Contains(x.ColumnName));
                    if (commonFieldColumns.Count() > 0)
                    {
                        foreach (DataRow responseRow in pricingTable.Rows)
                        {
                            EnumerableRowCollection<DataRow> matchingDataRows = reqKvp.Value.ResponseTable.AsEnumerable().Where(x => String.Compare(x["Symbol"].ToString(), responseRow["Symbol"].ToString(), true) == 0 && String.Compare(x["Symbology"].ToString(), responseRow["Symbology"].ToString(), true) == 0 && DateTime.Parse(x["Date"].ToString()).Date == DateTime.Parse(responseRow["Date"].ToString()).Date);
                            foreach (DataRow matchRow in matchingDataRows)
                            {
                                foreach (DataColumn dc in commonFieldColumns)
                                {
                                    if (matchRow[dc.ColumnName] == null || matchRow[dc.ColumnName] == DBNull.Value || String.IsNullOrWhiteSpace(matchRow[dc.ColumnName].ToString()))
                                    {
                                        if (responseRow[dc.ColumnName] != null && responseRow[dc.ColumnName] != DBNull.Value && !String.IsNullOrWhiteSpace(responseRow[dc.ColumnName].ToString()))
                                            matchRow[dc.ColumnName] = responseRow[dc.ColumnName];
                                    }
                                }
                            }
                        }
                    }
                    reqKvp.Value.BBRequestIds.TryRemove(requestId, out tempPrcngMapping);
                    reqKvp.Value.BBRequestIdsInProcess.TryRemove(requestId, out tempPrcngMapping);
                    if (reqKvp.Value.BBRequestIds.Count == 0 && reqKvp.Value.BBRequestIdsInProcess.Count == 0)
                    {
                        reqKvp.Value.ResponseFunctionDelegate(reqKvp.Key, reqKvp.Value.ResponseTable, pricingSuccess, comment);
                        PricingRequestInProcessSecmasterCacheManager.TryRemove(reqKvp.Key, out tempPrcngMapping);
                    }
                }
                SecmasterPricingDataCacheAndManager.Instance.UpdateDBandCacheOnResponse(pricingTable);

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
    }
}
