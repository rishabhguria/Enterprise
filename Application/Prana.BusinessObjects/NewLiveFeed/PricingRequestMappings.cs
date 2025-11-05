using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.BusinessObjects.NewLiveFeed
{
    /// <summary>
    /// This class stores the requests and response related data for individual pricing requests. It can be used to pass and modify data easily
    /// </summary>
    public class PricingRequestMappings
    {
        /// <summary>
        /// Constructor which creates an instance of the class filled with the latest pricing data
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="secondaryPricingSource"></param>
        /// <param name="fields"></param>
        /// <param name="secmasterReq"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="functionForResponse"></param>
        public PricingRequestMappings(string requestId, string secondaryPricingSource, IEnumerable<string> fields, SecMasterRequestObj secmasterReq, DateTime startDate, DateTime endDate, Action<string, System.Data.DataTable, bool, string> functionForResponse, bool isGetDataFromCacheOrDB)
        {
            try
            {
                RequestKeyMapping = new ConcurrentBag<string>();
                IsFull = false;
                FieldNames = new ConcurrentBag<string>(fields);
                BBRequestIds = new ConcurrentDictionary<string, PricingRequestMappings>();
                BBRequestIdsInProcess = new ConcurrentDictionary<string, PricingRequestMappings>();
                RequestObj = secmasterReq;
                StartDate = startDate.Date;
                EndDate = endDate.Date;
                RequestID = requestId;
                SecondaryPricingSource = secondaryPricingSource;
                IsGetDataFromCacheOrDB = isGetDataFromCacheOrDB;
                ResponseTable = CreateDataTableAndKeyMappingFromReq();
                if (functionForResponse != null)
                    ResponseFunctionDelegate = functionForResponse;
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
        /// Constructor to store requests. Provides a datastructure to store requests, does not use any functionality of the class. For functionality use the other overloaded constructor.
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="fields"></param>
        /// <param name="secmasterReq"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public PricingRequestMappings(string requestId, IEnumerable<string> fields, SecMasterRequestObj secmasterReq, DateTime startDate, DateTime endDate, Action<QueueMessage, Guid> clientFunction, string secondaryPricingSource, bool isGetDataFromCacheOrDB)
        {
            try
            {
                RequestKeyMapping = new ConcurrentBag<string>();
                IsFull = false;
                FieldNames = new ConcurrentBag<string>(fields);
                BBRequestIds = new ConcurrentDictionary<string, PricingRequestMappings>();
                BBRequestIdsInProcess = new ConcurrentDictionary<string, PricingRequestMappings>();
                RequestObj = secmasterReq;
                StartDate = startDate.Date;
                EndDate = endDate.Date;
                RequestID = requestId;
                ClientFunction = clientFunction;
                SecondaryPricingSource = secondaryPricingSource;
                IsGetDataFromCacheOrDB = isGetDataFromCacheOrDB;
                //ResponseTable = CreateDataTableAndKeyMappingFromReq(fields, secmasterReq, startDate.Date, endDate.Date);
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
        /// Determines whether all the data has been filled in the request and whether it is ready for sending back to client
        /// </summary>
        public bool IsFull { get; set; }
        public string RequestID { get; set; }
        public DataTable ResponseTable { get; set; }
        public Action<string, System.Data.DataTable, bool, string> ResponseFunctionDelegate { get; set; }
        public ConcurrentBag<string> RequestKeyMapping { get; set; }
        public ConcurrentDictionary<string, PricingRequestMappings> BBRequestIds { get; set; }
        public ConcurrentDictionary<string, PricingRequestMappings> BBRequestIdsInProcess { get; set; }
        public ConcurrentBag<string> FieldNames { get; set; }
        public SecMasterRequestObj RequestObj { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SecondaryPricingSource { get; set; }
        public bool IsGetDataFromCacheOrDB { get; set; }
        public Action<QueueMessage, Guid> ClientFunction { get; set; }

        public DataTable CreateDataTableAndKeyMappingFromReq(int[] auecids = null, List<Tuple<HashSet<int>, List<Tuple<DateTime, DateTime>>, HashSet<DateTime>>> auecWiseHolidayList = null)
        {
            IEnumerable<string> fields = FieldNames;
            SecMasterRequestObj secmasterReq = RequestObj;
            DateTime startDate = StartDate;
            DateTime endDate = EndDate;
            DataTable responseTable = null;
            try
            {
                List<string> columnList = new List<string>(fields);
                columnList.Add("Symbol");
                columnList.Add("Date");
                columnList.Add("Symbology");
                columnList.Add("SymbolPK");
                columnList.Add("DataSource");
                columnList.Add("SecondarySource");
                responseTable = GetDataTableFromList(columnList, "Historical Prices");
                for (int rowCount = 0; rowCount < secmasterReq.SymbolDataRowCollection.Count; rowCount++)
                {
                    SymbolDataRow symRow = secmasterReq.SymbolDataRowCollection[rowCount];
                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        DataRow tempRow = responseTable.NewRow();
                        tempRow["Symbol"] = symRow.PrimarySymbol;
                        tempRow["Date"] = date.ToString("yyyy-MM-dd HH:mm:ss.000");
                        tempRow["Symbology"] = symRow.PrimarySymbology.ToString();
                        tempRow["SymbolPK"] = symRow.Symbol_PK.ToString();
                        tempRow["DataSource"] = Prana.BusinessObjects.SecMasterConstants.SecMasterSourceOfData.BloombergDLWS.ToString();
                        tempRow["SecondarySource"] = SecondaryPricingSource;
                        responseTable.Rows.Add(tempRow);

                        if (auecids != null && auecWiseHolidayList != null)
                        {
                            if (string.IsNullOrWhiteSpace(symRow.Symbol_PK.ToString()) || String.Compare(symRow.Symbol_PK.ToString(), "0", StringComparison.OrdinalIgnoreCase) == 0 || String.Compare(symRow.Symbol_PK.ToString(), DateTimeConstants.MinValue.ToString()) == 0)
                            {
                                foreach (DataColumn col in responseTable.Columns)
                                {
                                    if (!FieldNames.Contains(col.ColumnName))
                                        continue;
                                    tempRow[col.ColumnName] = "Not Validated";
                                }
                            }
                            else
                            {
                                IEnumerable<Tuple<HashSet<int>, List<Tuple<DateTime, DateTime>>, HashSet<DateTime>>> tupleAuec = auecWiseHolidayList.Where(x => x.Item1.Contains(auecids[rowCount]));
                                if (tupleAuec.Count() > 0)
                                {
                                    if (tupleAuec.First().Item3.Contains(date))
                                    {
                                        foreach (DataColumn col in responseTable.Columns)
                                        {
                                            if (!FieldNames.Contains(col.ColumnName))
                                                continue;
                                            tempRow[col.ColumnName] = "Holiday";
                                        }
                                    }
                                }
                            }
                        }
                        //modified by omshiv, changed request key 

                        RequestKeyMapping.Add(EscapedDelimiter.CombineStrings(symRow.Symbol_PK.ToString(), date.ToString("yyyy-MM-dd HH:mm:ss.000"), tempRow["DataSource"].ToString(), tempRow["SecondarySource"].ToString()));
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
            return responseTable;
        }


        DataTable GetDataTableFromList(List<string> fields, string tableName)
        {
            DataTable dt = new DataTable(tableName);
            try
            {
                foreach (string field in fields)
                {
                    if (!dt.Columns.Contains(field))
                        dt.Columns.Add(field);
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
            return dt;
        }

        /// <summary>
        /// Merges the BB requests generated based on ommon factors. Need to optimize the function to combine requests based on date also.
        /// </summary>
        public void MergeBloombergRequestOnFields()
        {
            try
            {
                bool mergeComplete = false;
                while (!mergeComplete)
                {
                    mergeComplete = true;
                    ConcurrentDictionary<string, PricingRequestMappings> tempConDict;
                    foreach (KeyValuePair<string, PricingRequestMappings> bbreqKvp in BBRequestIds)
                    {
                        tempConDict = new ConcurrentDictionary<string, PricingRequestMappings>(BBRequestIds.Where(x => x.Value.RequestID != bbreqKvp.Value.RequestID && x.Value.RequestObj.Count == bbreqKvp.Value.RequestObj.Count && (x.Value.StartDate.Date >= bbreqKvp.Value.StartDate.Date) && (x.Value.EndDate.Date <= bbreqKvp.Value.EndDate.Date)));
                        if (tempConDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, PricingRequestMappings> matchKvp in tempConDict)
                            {
                                if (bbreqKvp.Value.RequestObj.GetPrimarySymbols().Except(matchKvp.Value.RequestObj.GetPrimarySymbols()).Count() == 0)
                                {
                                    mergeComplete = false;
                                    foreach (string field in matchKvp.Value.FieldNames)
                                    {
                                        if (!bbreqKvp.Value.FieldNames.Contains(field))
                                            bbreqKvp.Value.FieldNames.Add(field);
                                    }
                                    PricingRequestMappings tempMapping;
                                    BBRequestIds.TryRemove(matchKvp.Key, out tempMapping);
                                }
                            }
                            if (!mergeComplete)
                                break;
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
        }



    }
}
