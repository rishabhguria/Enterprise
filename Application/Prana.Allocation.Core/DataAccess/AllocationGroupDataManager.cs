// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-03-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-08-2014
// ***********************************************************************
// <copyright file="AllocationGroupDataManager.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Core.Extensions;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.ServerCommon;
using Prana.Utilities;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The DataAccess namespace.
/// </summary>
namespace Prana.Allocation.Core.DataAccess
{
    /// <summary>
    /// Class AllocationGroupDataManager.
    /// </summary>
    internal static class AllocationGroupDataManager
    {
        /// <summary>
        /// Saves the groups.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal static int SaveGroups(List<AllocationGroup> groups)
        {
            int result = 0;
            try
            {
                int chunkSize = 500;
                object locker = new object();
                List<List<AllocationGroup>> groupChunks = ChunkingManager.CreateChunks<AllocationGroup>(groups, chunkSize);
                foreach (List<AllocationGroup> groupChunk in groupChunks)
                {
                    XmlSaveHandler xmSaveHandler = new XmlSaveHandler();
                    foreach (AllocationGroup group in groupChunk)
                    //Parallel.ForEach(groups, group =>
                    {
                        ServiceProxyConnector.SecmasterProxy.SetSecuritymasterDetails(group);
                        // If the company name is not received from the securitymasterd then fill the description
                        if (String.IsNullOrEmpty(group.CompanyName))
                        {
                            group.CompanyName = group.Description;
                        }
                        if (group.PersistenceStatus.Equals(ApplicationConstants.PersistenceStatus.ReAllocated) && group.Orders.Count > 0)
                        {
                            PranaMessage pranaMessage = OrderCacheManager.GetCachedOrder(group.Orders[0].ClOrderID);
                            if (pranaMessage != null && OrderCacheManager.HasMultiDayHistory(pranaMessage) && group.TaxLots.Count == 0)   
                            {
                                string parentClOrderId = OrderCacheManager.DictMultiDayClOrderIDMapping.ContainsKey(group.Orders[0].ClOrderID) ? OrderCacheManager.DictMultiDayClOrderIDMapping[group.Orders[0].ClOrderID] : string.Empty;
                                if (OrderCacheManager.DictMultiDayOrderOriginalClOrderIdAfterReplace.ContainsKey(parentClOrderId))
                                    parentClOrderId = OrderCacheManager.DictMultiDayOrderOriginalClOrderIdAfterReplace[parentClOrderId];
                                if(!string.IsNullOrEmpty(parentClOrderId))
                                    group.Orders[0].ParentClOrderID = parentClOrderId;
                                group.Orders[0].AccountID = int.MinValue;
                            }
                        }
                        //lock (locker)
                        {
                            xmSaveHandler.CreateXmls(group);
                        }
                    }
                    result += xmSaveHandler.SaveGroupXmls();
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
                return -1;
            }
            return result;
        }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <param name="toDate">To date.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="filterList">The filter list.</param>
        /// <returns>List of AllocationGroup</returns>
        internal static ConcurrentDictionary<string, AllocationGroup> GetGroups(DateTime toDate, DateTime fromDate, AllocationPrefetchFilter filterList, bool fetchAllValues = false)
        {
            try
            {
                int chunkSize = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("AllocationChunkSize"));
                ConcurrentDictionary<string, AllocationGroup> result = new ConcurrentDictionary<string, AllocationGroup>();
                DateTime tempDate = fromDate;
                //get all unallocated groups first, PRANA-13100
                result.AddRangeThreadSafely(GetChunkedGroups(toDate, fromDate, filterList, true, fetchAllValues));

                //get all allocated groups, PRANA-13100
                while (toDate > tempDate.AddDays(chunkSize))
                {
                    result.AddRangeThreadSafely(GetChunkedGroups(tempDate.AddDays(chunkSize - 1), tempDate, filterList, false, fetchAllValues));
                    tempDate = tempDate.AddDays(chunkSize);
                }
                result.AddRangeThreadSafely(GetChunkedGroups(toDate, fromDate > tempDate ? fromDate : tempDate, filterList, false, fetchAllValues));

                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <param name="toDate">To date.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="filterList">The filter list.</param>
        /// <returns>List&lt;AllocationGroup&gt;.</returns>
        private static ConcurrentDictionary<string, AllocationGroup> GetChunkedGroups(DateTime toDate, DateTime fromDate, AllocationPrefetchFilter filterList, bool isGetUnallocatedGroups = false, bool fetchAllValues = false)
        {

            try
            {
                return System.Threading.Tasks.Task.Run<ConcurrentDictionary<string, AllocationGroup>>(async () =>
                                {
                                    return await GetChunkedGroupsAsync(toDate, fromDate, filterList, isGetUnallocatedGroups, fetchAllValues);
                                }).Result;
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
                return null;
            }
        }

        /// <summary>
        /// Gets the allocation data.
        /// </summary>
        /// <param name="toDate">To date.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="filterList">The filter list.</param>
        /// <returns>List&lt;AllocationGroup&gt;.</returns>
        private async static Task<ConcurrentDictionary<string, AllocationGroup>> GetChunkedGroupsAsync(DateTime toDate, DateTime fromDate, AllocationPrefetchFilter filterList, bool isGetUnallocatedGroups, bool fetchAllValues = false)
        {
            ConcurrentDictionary<string, AllocationGroup> result = new ConcurrentDictionary<string, AllocationGroup>();
            try
            {
                List<System.Threading.Tasks.Task> tasks = new List<System.Threading.Tasks.Task>();

                System.Threading.Tasks.Task getClosingDataTask = System.Threading.Tasks.Task.Run(() => ServiceProxyConnector.ClosingProxy.UpdateTradeDates(fromDate.ToString(), toDate.ToString()));
                tasks.Add(getClosingDataTask);


                string filterString = QueryGenerator.GenerateFilterQuery(filterList, isGetUnallocatedGroups);
                System.Threading.Tasks.Task<ConcurrentDictionary<string, AllocationGroup>> getGroupsTask = System.Threading.Tasks.Task.Run<ConcurrentDictionary<string, AllocationGroup>>(() => GetAllocationData(toDate, fromDate, isGetUnallocatedGroups, filterString, QueryGenerator.GenerateSymbolFilterQuery(filterList, isGetUnallocatedGroups), fetchAllValues));


                tasks.Add(getGroupsTask);

                await System.Threading.Tasks.Task.WhenAll(tasks);
                result = getGroupsTask.Result;


                Parallel.ForEach(result.Values, group => group.EnrichDataFromClosingService());
                return result;
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
                return null;
            }
        }

        private static ConcurrentDictionary<string, AllocationGroup> GetAllocationData(DateTime toDate, DateTime fromDate, bool isGetUnallocatedGroups, string filterString, string symbolFilterQuery, bool fetchAllValues = false)
        {
            ConcurrentDictionary<string, AllocationGroup> dictGroups = new ConcurrentDictionary<string, AllocationGroup>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllocationData";
                queryData.CommandTimeout = 900;
                queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FromDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = fromDate
                });
                queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ToDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = toDate
                });
                queryData.DictionaryDatabaseParameter.Add("@IsGetUnallocatedGroups", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@IsGetUnallocatedGroups",
                    ParameterType = DbType.Boolean,
                    ParameterValue = isGetUnallocatedGroups
                });
                queryData.DictionaryDatabaseParameter.Add("@FilterString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FilterString",
                    ParameterType = DbType.String,
                    ParameterValue = filterString
                });
                queryData.DictionaryDatabaseParameter.Add("@SymbolFilterQuery", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@SymbolFilterQuery",
                    ParameterType = DbType.String,
                    ParameterValue = symbolFilterQuery
                });
                queryData.DictionaryDatabaseParameter.Add("@IsSkipStateIDFilter", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@IsSkipStateIDFilter",
                    ParameterType = DbType.Boolean,
                    ParameterValue = fetchAllValues
                });

                DataSet AllocationData = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                List<AllocationGroup> listGroups = new List<AllocationGroup>();
                Object locker = new object();

                
                if (AllocationData != null && AllocationData.Tables.Count > 0)
                {
                    Parallel.ForEach(AllocationData.Tables[0].AsEnumerable(), dr =>
                    //foreach (DataRow dr in AllocationData.Tables[0].Rows)
                    {
                        AllocationGroup alGroup = new AllocationGroup(dr);
                        FillGroups(alGroup, dr);

                        lock (locker)
                        {
                            if (!dictGroups.ContainsKey(alGroup.GroupID))
                            {
                                dictGroups.TryAdd(alGroup.GroupID, alGroup);
                            }
                        }
                    });

                    foreach (DataRow dr in AllocationData.Tables[1].Rows)
                    {
                        if (dictGroups.ContainsKey(dr["GroupID"].ToString()))
                        {
                            AllocationGroup alGroup = dictGroups[dr["GroupID"].ToString()];
                            UpdateGroup(alGroup, dr);
                            dictGroups.TryRemove(alGroup.GroupID, out _);
                            dictGroups.TryAdd(alGroup.GroupID, alGroup);
                        }
                        else
                        {
                            AllocationGroup alGroup = new AllocationGroup(dr);
                            FillGroups(alGroup, dr);
                            dictGroups.TryAdd(alGroup.GroupID, alGroup);
                        }
                    }

                    if (CachedDataManager.GetInstance.IsNewOTCWorkflow)
                        UpdateOTCParameters(dictGroups);
                }
                else if (AllocationData == null)
                {
                    throw new ArgumentNullException(nameof(AllocationData));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            Logger.LoggerWrite("Allocation Data fetched.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_ALLOCATION_SERVICE);
            return dictGroups;
        }

        private static void UpdateOTCParameters(ConcurrentDictionary<string, AllocationGroup> dictGroups)
        {
            try
            {
                List<OTCTradeData> octTradeData = ServiceProxyConnector.SecMasterOTCService.GetOTCTradeDataAsync(dictGroups.Keys.ToList());

                foreach (var otc in octTradeData)
                {
                    if (dictGroups.ContainsKey(otc.GroupID))
                    {
                        dictGroups[otc.GroupID].OTCParameters = otc;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates the group.
        /// </summary>
        /// <param name="alGroup">The al group.</param>
        /// <param name="dr">The dr.</param>
        private static void UpdateGroup(AllocationGroup alGroup, DataRow dr)
        {
            try
            {
                bool orderExists = false;
                foreach (AllocationOrder ao in alGroup.Orders)
                {
                    if (ao.ParentClOrderID.Equals(dr["ParentClOrderID"].ToString()))
                    {
                        orderExists = true;
                        break;
                    }
                }

                if (!orderExists)
                {
                    AllocationOrder order = new AllocationOrder(dr);
                    alGroup.Orders.Add(order);
                    alGroup.OrderCount = alGroup.Orders.Count;
                }

                // for unallocated trades, we do not need to further add taxlots.
                if (alGroup.StateID == 2)
                {
                    alGroup.EnrichDataFromOtherServices(dr, true);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Fills the groups.
        /// </summary>
        /// <param name="alGroup">The al group.</param>
        /// <param name="dr">The dr.</param>
        private static void FillGroups(AllocationGroup alGroup, DataRow dr)
        {
            try
            {
                AllocationOrder order = new AllocationOrder(dr);

                alGroup.Orders.Add(order);
                alGroup.OrderCount = 1;

                alGroup.EnrichDataFromOtherServices(dr, false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
