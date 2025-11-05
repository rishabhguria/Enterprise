using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.NewLiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace Prana.SecurityMasterNew.BLL
{
    internal class SecmasterPricingReqManagerClient : IDisposable
    {
        #region singleton

        private static volatile SecmasterPricingReqManagerClient instance;
        private static readonly object syncRoot = new Object();

        private SecmasterPricingReqManagerClient()
        {
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }

        public static SecmasterPricingReqManagerClient Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SecmasterPricingReqManagerClient();
                    }
                }

                return instance;
            }
        }
        #endregion

        /// <summary>
        /// The timeout used for the validation for symbol. Need to create a common config entry for the same
        /// </summary>
        const double _timeout = 10 * 60 * 1000;

        /// <summary>
        /// Timer firse every 10 seconds and checks whether timout for a request has occured
        /// </summary>
        Timer _timer = new Timer(10 * 1000);

        //ConcurrentDictionary<Guid, Action<QueueMessage, Guid>> _genericPriceRequests = new ConcurrentDictionary<Guid, Action<QueueMessage, Guid>>();

        //public ConcurrentDictionary<Guid, Action<QueueMessage, Guid>> GenericPriceRequests
        //{
        //    get { return _genericPriceRequests; }
        //    set { _genericPriceRequests = value; }
        //}
        PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();

        ConcurrentDictionary<Guid, Tuple<SecMasterRequestObj, DateTime, int[]>> _requestsWaitingForValidation = new ConcurrentDictionary<Guid, Tuple<SecMasterRequestObj, DateTime, int[]>>();

        private ConcurrentDictionary<Guid, Tuple<SecMasterRequestObj, DateTime, int[]>> RequestsWaitingForValidation
        {
            get { return _requestsWaitingForValidation; }
            set { _requestsWaitingForValidation = value; }
        }

        ConcurrentDictionary<Guid, Tuple<PricingRequestMappings, int[]>> _requestsWithValidationComplete = new ConcurrentDictionary<Guid, Tuple<PricingRequestMappings, int[]>>();

        private ConcurrentDictionary<Guid, Tuple<PricingRequestMappings, int[]>> RequestsWithValidationComplete
        {
            get { return _requestsWithValidationComplete; }
            set { _requestsWithValidationComplete = value; }
        }

        ConcurrentDictionary<string, PricingRequestMappings> _requestInProcess = new ConcurrentDictionary<string, PricingRequestMappings>();

        internal ConcurrentDictionary<string, PricingRequestMappings> RequestInProcess
        {
            get { return _requestInProcess; }
        }

        ConcurrentDictionary<string, string> _requestMapping = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// While fetching pricing data we validate the symbols again before sending the request to Bloomberg.
        /// This function keeps the requests in cache until the validation is completed and then we can send the pricing requests to Bloomberg
        /// </summary>
        /// <param name="secondaryPricingSource"></param>
        /// <param name="fields"></param>
        /// <param name="requestObj"></param>
        /// <param name="requestID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="clientFunction"></param>
        /// <returns></returns>
        public bool RegisterRequestForValidation(String secondaryPricingSource, List<string> fields, SecMasterRequestObj requestObj, Guid requestID, DateTime startDate, DateTime endDate, Action<QueueMessage, Guid> clientFunction, bool isGetDataFromCacheOrDB)
        {
            bool result = false;
            try
            {
                result = _requestsWaitingForValidation.TryAdd(requestID, new Tuple<SecMasterRequestObj, DateTime, int[]>(requestObj, DateTime.Now, new int[requestObj.Count]));
                if (result)
                {
                    _requestsWithValidationComplete.TryAdd(requestID, new Tuple<PricingRequestMappings, int[]>(new PricingRequestMappings(requestID.ToString(), fields, requestObj, startDate, endDate, clientFunction, secondaryPricingSource, isGetDataFromCacheOrDB), new int[requestObj.Count]));
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
            return result;
        }

        /// <summary>
        /// Used to send the requests for the selected pricing requests for which validation of all symbols is complete or the validation timeout has been hit
        /// </summary>
        public event EventHandler<List<Guid>> SendRequestForGuid;

        /// <summary>
        /// Used to send the requests for the selected pricing requests for which validation of all symbols is complete and holidays are excluded or the validation timeout has been hit
        /// </summary>
        public event EventHandler<string> SendRequestForGuidWithoutHolidays;

        /// <summary>
        /// Updates the pricing requests according to the symbol validated from the LiveFeed
        /// </summary>
        /// <param name="secMasterBaseObj"></param>
        /// <returns></returns>
        public List<Guid> RemoveValidatedSymbolAndSendRequest(SecMasterBaseObj secMasterBaseObj)
        {
            List<Guid> tempGuidList = new List<Guid>();
            try
            {
                foreach (KeyValuePair<Guid, Tuple<SecMasterRequestObj, DateTime, int[]>> kvp in _requestsWaitingForValidation)
                {
                    for (int i = 0; i < kvp.Value.Item1.SymbolDataRowCollection.Count; i++)
                    {
                        if (kvp.Value.Item1.SymbolDataRowCollection[i].IsSameRequest(secMasterBaseObj))
                        {
                            kvp.Value.Item1.SymbolDataRowCollection[i].Symbol_PK = secMasterBaseObj.Symbol_PK;
                            kvp.Value.Item1.ValidatedSymbolCount++;
                            kvp.Value.Item3[i] = secMasterBaseObj.AUECID;
                        }
                    }
                    if (kvp.Value.Item1.ValidatedSymbolCount == kvp.Value.Item1.Count)
                    {
                        tempGuidList.Add(kvp.Key);
                    }
                }
                //Tuple<SecMasterRequestObj, DateTime, int[]> tempTuple;
                //tempGuidList.ForEach(x => { _requestsWaitingForValidation.TryRemove(x, out tempTuple); });
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
            return tempGuidList;
        }

        /// <summary>
        /// Used to ensure that only one thread is working on the timer elapsed function to send the requests to the LiveFeed
        /// </summary>
        readonly object timerElapsedThreadSync = new object();

        /// <summary>
        /// Occurs on symbol validation timeout of the pricing request. Removes non validated symbols and sends the request for validated one
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                lock (timerElapsedThreadSync)
                {
                    List<Guid> listGuidToSendPricingRequest = new List<Guid>();
                    foreach (KeyValuePair<Guid, Tuple<SecMasterRequestObj, DateTime, int[]>> kvp in _requestsWaitingForValidation.Where(x => (e.SignalTime - x.Value.Item2).TotalMilliseconds > _timeout))
                    {
                        //RemoveNonValidatedSymbolsFromRequest(kvp.Key);
                        listGuidToSendPricingRequest.Add(kvp.Key);
                        //Tuple<SecMasterRequestObj, DateTime, int[]> tuplTemp;
                        //_requestsWaitingForValidation.TryRemove(kvp.Key, out tuplTemp);
                    }
                    if (listGuidToSendPricingRequest.Count > 0)
                    {
                        SendRequestForGuid(this, listGuidToSendPricingRequest);
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
        /// Removes invalid symbols from the pricing requests for which the validation timeout has hit
        /// </summary>
        /// <param name="guid"></param>
        //private void RemoveNonValidatedSymbolsFromRequest(Guid guid)
        //{
        //    try
        //    {
        //        if (_requestsWithValidationComplete.ContainsKey(guid))
        //        {
        //            ConcurrentBag<SymbolDataRow> tempListToRemove = new ConcurrentBag<SymbolDataRow>();
        //            _requestsWithValidationComplete[guid].Item1.RequestObj.SymbolDataRowCollection.AsParallel().ForAll(x => { if (x.Symbol_PK == long.MinValue || x.Symbol_PK == 0) { tempListToRemove.Add(x); } });
        //            foreach (SymbolDataRow row in tempListToRemove)
        //            {
        //                _requestsWithValidationComplete[guid].Item1.RequestObj.SymbolDataRowCollection.Remove(row);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Returns the client function to which the response is to be sent after the response is received from trade server for the pricing request
        ///// </summary>
        ///// <param name="requestId"></param>
        ///// <returns></returns>
        //internal Action<QueueMessage, Guid> RequestForFinalResponse(string requestId)
        //{
        //    PricingRequestMappings requestDetails;
        //    try
        //    {
        //        if (_requestInProcess.TryGetValue(requestId, out requestDetails))
        //        {
        //            return requestDetails.ClientFunction;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// Breaks request into multiple requests according to auec wise holiday list for validated symbols
        /// </summary>
        /// <param name="auecWiseHolidayList"></param>
        /// <param name="originalRequest"></param>
        /// <param name="auecids"></param>
        internal void BreakValidatedRequestsByHolidays(List<Tuple<HashSet<int>, List<Tuple<DateTime, DateTime>>, HashSet<DateTime>>> auecWiseHolidayList, PricingRequestMappings originalRequest, int[] auecids)
        {
            try
            {
                foreach (Tuple<HashSet<int>, List<Tuple<DateTime, DateTime>>, HashSet<DateTime>> holidayList in auecWiseHolidayList)
                {
                    SecMasterRequestObj reqNew = new SecMasterRequestObj();
                    for (int i = 0; i < auecids.Length; i++)
                    {
                        if (holidayList.Item1.Contains(auecids[i]))
                        {
                            if (originalRequest.RequestObj.SymbolDataRowCollection[i].Symbol_PK == long.MinValue || originalRequest.RequestObj.SymbolDataRowCollection[i].Symbol_PK == 0)
                                continue;
                            reqNew.AddData(originalRequest.RequestObj.SymbolDataRowCollection[i].PrimarySymbol, (ApplicationConstants.SymbologyCodes)originalRequest.RequestObj.SymbolDataRowCollection[i].PrimarySymbology, originalRequest.RequestObj.SymbolDataRowCollection[i].Symbol_PK);
                        }
                    }

                    //originalRequest.RequestObj
                    foreach (Tuple<DateTime, DateTime> interval in holidayList.Item2)
                    {
                        Guid newGuid = Guid.NewGuid();
                        originalRequest.BBRequestIds.TryAdd(newGuid.ToString(), new PricingRequestMappings(newGuid.ToString(), originalRequest.FieldNames, reqNew, interval.Item1, interval.Item2, ResponseFunctionForPricingResponseFromServer, originalRequest.SecondaryPricingSource, originalRequest.IsGetDataFromCacheOrDB));
                        _requestMapping.TryAdd(newGuid.ToString(), originalRequest.RequestID);

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

        void ResponseFunctionForPricingResponseFromServer(QueueMessage qMsg, Guid requestID)
        {
            try
            {
                List<object> listDataReturned = binaryFormatter.DeSerializeParams(qMsg.Message.ToString());
                DataTable priceData = listDataReturned[0] as DataTable;
                bool pricingSuccess = (bool)listDataReturned[1];
                string comment = listDataReturned[2] as string;
                string originalRequestId;
                Tuple<PricingRequestMappings, int[]> originalRequest;
                if (_requestMapping.TryRemove(requestID.ToString(), out originalRequestId) && _requestsWithValidationComplete.ContainsKey(Guid.Parse(originalRequestId)))
                {
                    originalRequest = _requestsWithValidationComplete[Guid.Parse(originalRequestId)];
                    IEnumerable<DataColumn> commonFieldColumns = priceData.Columns.Cast<DataColumn>().Where(x => originalRequest.Item1.FieldNames.Contains(x.ColumnName));
                    commonFieldColumns = commonFieldColumns.Where(x => originalRequest.Item1.ResponseTable.Columns.Contains(x.ColumnName));
                    if (commonFieldColumns.Count() > 0)
                    {
                        foreach (DataRow responseRow in priceData.Rows)
                        {
                            EnumerableRowCollection<DataRow> matchingDataRows = originalRequest.Item1.ResponseTable.AsEnumerable().Where(x => String.Compare(x["Symbol"].ToString(), responseRow["Symbol"].ToString(), true) == 0 && String.Compare(x["Symbology"].ToString(), responseRow["Symbology"].ToString(), true) == 0 && DateTime.Parse(x["Date"].ToString()).Date == DateTime.Parse(responseRow["Date"].ToString()).Date);
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

                    PricingRequestMappings tempPrcngMapping;
                    originalRequest.Item1.BBRequestIds.TryRemove(requestID.ToString(), out tempPrcngMapping);
                    originalRequest.Item1.BBRequestIdsInProcess.TryRemove(requestID.ToString(), out tempPrcngMapping);

                    if (originalRequest.Item1.BBRequestIds.Count == 0 && originalRequest.Item1.BBRequestIdsInProcess.Count == 0)
                    {
                        _requestsWithValidationComplete.TryRemove(Guid.Parse(originalRequestId), out originalRequest);
                        string request = binaryFormatter.Serialize(originalRequest.Item1.ResponseTable, pricingSuccess, comment);
                        QueueMessage qMsgS = new QueueMessage(SecMasterConstants.CONST_SMGenericPriceRequest, qMsg.TradingAccountID, String.Join("", "SM", qMsg.UserID), request);
                        originalRequest.Item1.ClientFunction(qMsgS, Guid.Parse(originalRequestId));
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Pricing data response received not found.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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

        internal void ProcessSecMasterRequestForSM(Guid requestId)
        {
            try
            {
                if (!RequestsWithValidationComplete.ContainsKey(requestId)) { return; }
                if (!RequestsWaitingForValidation.ContainsKey(requestId)) { return; }
                Tuple<PricingRequestMappings, int[]> pricingRequestRow = new Tuple<PricingRequestMappings, int[]>(RequestsWithValidationComplete[requestId].Item1, RequestsWaitingForValidation[requestId].Item3);
                RequestsWithValidationComplete[requestId] = pricingRequestRow;

                List<Tuple<HashSet<int>, List<Tuple<DateTime, DateTime>>, HashSet<DateTime>>> tempAUECWiseHolidayList = BusinessDayCalculator.GetInstance().GetAUECWiseHolidayList(new HashSet<int>(pricingRequestRow.Item2), pricingRequestRow.Item1.StartDate, pricingRequestRow.Item1.EndDate);
                pricingRequestRow.Item1.ResponseTable = pricingRequestRow.Item1.CreateDataTableAndKeyMappingFromReq(pricingRequestRow.Item2, tempAUECWiseHolidayList);

                BreakValidatedRequestsByHolidays(tempAUECWiseHolidayList, pricingRequestRow.Item1, pricingRequestRow.Item2);

                Tuple<SecMasterRequestObj, DateTime, int[]> tempTuple;
                _requestsWaitingForValidation.TryRemove(requestId, out tempTuple);
                if (pricingRequestRow.Item1.BBRequestIds.Count == 0)
                {
                    SecmasterPricingReqManagerClient.Instance.RequestsWithValidationComplete.TryRemove(requestId, out pricingRequestRow);
                    string request = binaryFormatter.Serialize(pricingRequestRow.Item1.ResponseTable, false, "All dates holiday or symbols not valid");
                    QueueMessage qMsg = new QueueMessage(SecMasterConstants.CONST_SMGenericPriceRequest, "11", String.Join("", "SM", "17"), request);
                    pricingRequestRow.Item1.ClientFunction(qMsg, Guid.Parse(pricingRequestRow.Item1.RequestID));
                }
                else
                {
                    foreach (KeyValuePair<string, PricingRequestMappings> newBBRequest in pricingRequestRow.Item1.BBRequestIds)
                    {
                        _requestInProcess.AddOrUpdate(newBBRequest.Key, newBBRequest.Value, (x, y) => newBBRequest.Value);
                        SendRequestForGuidWithoutHolidays(this, newBBRequest.Key);
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

        public void Dispose()
        {
            if (_timer != null)
                _timer.Dispose();
        }
    }



    ///// <summary>
    ///// The class used to store any pricing request so that requests can be passed to other functions and UIs easily
    ///// </summary>
    //internal class GenericPricingRequestRow
    //{
    //    public Guid RequestId { get; set; }
    //    public List<string> Fields { get; set; }
    //    public SecMasterRequestObj SecMasterReqObj { get; set; }
    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //    public Action<QueueMessage, Guid> ClientFunction { get; set; }
    //    public bool RequestSent { get; set; }
    //    public string SecondaryPricingSource { get; set; }

    //    public GenericPricingRequestRow(Guid requestId, string secondaryPricingSource, List<string> fields, SecMasterRequestObj secMasterReqObj, DateTime startDate, DateTime endDate, Action<QueueMessage, Guid> clientFunction, bool requestSent)
    //    {
    //        try
    //        {
    //            RequestId = requestId;
    //            Fields = fields;
    //            SecMasterReqObj = secMasterReqObj;
    //            StartDate = startDate;
    //            EndDate = endDate;
    //            ClientFunction = clientFunction;
    //            RequestSent = requestSent;
    //            //modified by omshiv, added secondary pricing source
    //            SecondaryPricingSource = secondaryPricingSource;

    //        }
    //        catch (Exception ex)
    //        {
    //            // Invoke our policy that is responsible for making sure no secure information
    //            // gets out of our layer.
    //            bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
    //            if (rethrow)
    //            {
    //                throw;
    //            }
    //        }
    //    }


    //}
}
