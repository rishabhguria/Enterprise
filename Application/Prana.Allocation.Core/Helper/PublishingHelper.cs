using Newtonsoft.Json;
using Prana.Allocation.Core.DataAccess;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.PostTrade;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace Prana.Allocation.Core.Helper
{
    internal static class PublishingHelper
    {
        #region Members

        /// <summary>
        /// The publish lock
        /// </summary>
        private static readonly object _publishLock = new object();

        static Dictionary<int, CashPreferences> cashPreferences = ServiceProxyConnector.CashManagementService.GetCashPreferences();

        #endregion Members

        #region Publish Methods

        /// <summary>
        /// Publish groups to different modules.
        /// </summary>
        /// <param name="groups">list of groups</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isPublishToCompliance">if set to <c>true</c> [is publish to compliance].</param>
        /// <param name="isCreateTaxlotAuditTrail">if set to <c>true</c> [is create taxlot audit trail].</param>
        internal static void Publish(List<AllocationGroup> groups, int userId, bool isPublishToCompliance, bool isCreateTaxlotAuditTrail, bool isRemoveManualExecution = false)
        {
            PublishGroup(groups, userId);
            if (isPublishToCompliance)
                PublishToCompliance(groups, userId, isRemoveManualExecution);
            PublishOMIData(groups);
            PublishTaxlots(groups, isCreateTaxlotAuditTrail);
            //https://jira.nirvanasolutions.com:8443/browse/PRANA-40982
            if (!(cashPreferences == null) && (cashPreferences.First().Value.IsCreateManualJournals == true))
                ServiceProxyConnector.ActivityService.GenerateTradingCashActivity(groups);           

        }

        /// <summary>
        /// Publishes the preference update.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="operationPrefId">The operation preference identifier.</param>
        internal static void PublishPreferenceUpdate(int userId, int operationPrefId)
        {
            try
            {
                List<int> prefResult = new List<int>();
                prefResult.Add(userId);
                prefResult.Add(operationPrefId);
                MessageData e = new MessageData();
                e.EventData = prefResult;
                e.TopicName = Topics.Topic_AllocationPreferenceUpdated;
                CentralizePublish(e);
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
        /// Publishes the group only for web.
        /// </summary>
        /// <param name="groups">The groups.</param>
        internal static void PublishGroupToWeb(List<AllocationGroup> groups, int userId)
        {
            try
            {
                MessageData e = new MessageData();
                e.EventData = groups;
                e.UserId = userId;
                e.TopicName = Topics.Topic_CreateGroupForWeb;
                CentralizePublish(e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Publishes the group.
        /// </summary>
        /// <param name="groups">The groups.</param>
        private static void PublishGroup(List<AllocationGroup> groups, int userId)
        {
            try
            {
                MessageData e = new MessageData();
                e.EventData = groups;
                e.UserId = userId;
                e.TopicName = Topics.Topic_CreateGroup;
                CentralizePublish(e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Publishes to compliance.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="userId">The user identifier.</param>
        private static void PublishToCompliance(List<AllocationGroup> groups, int userId, bool isRemoveManualExecution)
        {
            try
            {
                MessageData e1 = new MessageData();
                e1.EventData = groups;
                e1.TopicName = Topics.Topic_UpdateInTrade;
                e1.IsRemoveManualExecution = isRemoveManualExecution;
                CentralizePublish(e1);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Publishes the omi data.
        /// </summary>
        /// <param name="groups">The groups.</param>
        private static void PublishOMIData(List<AllocationGroup> groups)
        {
            try
            {
                List<UserOptModelInput> listOMIData = new List<UserOptModelInput>();
                MessageData e = new MessageData();
                foreach (AllocationGroup grp in groups)
                {
                    //if (!string.IsNullOrEmpty(grp.ProxySymbol) && !grp.ProxySymbol.Equals(grp.Symbol))
                    //{
                    //Divya : 02042013 : When a new symbol is traded, if its proxy is defined, we have to update the OMI cache, and use proxy pricing
                    //instead of the original symbol. 
                    UserOptModelInput userOMI = new UserOptModelInput();
                    userOMI.Symbol = grp.Symbol;
                    userOMI.ProxySymbol = grp.ProxySymbol;
                    userOMI.UnderlyingSymbol = grp.UnderlyingSymbol;
                    userOMI.SecurityDescription = grp.CompanyName;
                    userOMI.Bloomberg = grp.BloombergSymbol;
                    userOMI.OSIOptionSymbol = grp.OSISymbol;
                    userOMI.IDCOOptionSymbol = grp.IDCOSymbol;
                    userOMI.StrikePrice = grp.StrikePrice;
                    userOMI.LeadCurrencyID = grp.LeadCurrencyID;
                    userOMI.VsCurrencyID = grp.VsCurrencyID;
                    userOMI.ExpirationDate = grp.ExpirationDate;
                    if (grp.PutCall != int.MinValue)
                        userOMI.PutorCall = (OptionType)grp.PutCall;
                    userOMI.AssetID = grp.AssetID;
                    userOMI.AuecID = grp.AUECID;
                    userOMI.PersistenceStatus = grp.PersistenceStatus;

                    if (!string.IsNullOrEmpty(userOMI.ProxySymbol))
                    {
                        userOMI.ProxySymbolUsed = true;
                    }
                    //Divya : 02042013 If proxy is defined from symbol lookup, only then we pblish the proxy data. 

                    listOMIData.Add(userOMI);
                    // }
                }

                if (listOMIData.Count > 0)
                {
                    e.EventData = listOMIData;
                    e.TopicName = Topics.Topic_PricingInputsData;
                    CentralizePublish(e);
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
        /// Publishes the taxlots.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="isCreateTaxlotAuditTrail">if set to <c>true</c> [is create taxlot audit trail].</param>
        private static void PublishTaxlots(List<AllocationGroup> groups, bool isCreateTaxlotAuditTrail)
        {
            try
            {
                List<TaxLot> chunkedTaxlotsList = new List<TaxLot>();

                List<TaxLot> taxlotsList = (from g in groups
                                            from t in g.GetAllTaxlots()
                                            select t).ToList();

                foreach (TaxLot taxlot in taxlotsList)
                {
                    ServiceProxyConnector.SecmasterProxy.SetSecuritymasterDetails(taxlot);
                    // merging UDA details before publishing
                    ServiceProxyConnector.SecmasterProxy.SetSecurityUDADetails(taxlot);
                    ServiceProxyConnector.ClosingProxy.SetTaxlotClosingStatus(taxlot);
                    chunkedTaxlotsList.Add(taxlot);
                    if (isCreateTaxlotAuditTrail)
                    {
                        if (!String.IsNullOrEmpty(taxlot.Level1ID.ToString()) && taxlot.Level1ID != int.MinValue && taxlot.Level1ID != 0 && taxlot.AUECLocalDate.Date < DateTime.UtcNow.Date)
                        {
                            List<TradeAuditEntry> allocatedTradeAuditCollection = AddAllocatedDataAuditEntry(taxlot, Prana.BusinessObjects.TradeAuditActionType.ActionType.REALLOCATE, "Group Allocated New Taxlots Created");
                            AuditTrailDataManager.Instance.SaveAuditList(allocatedTradeAuditCollection);
                        }
                    }
                }
                MessageData e = new MessageData();

                //Applied chunking for better publishing and no memory exceptions.
                List<List<TaxLot>> allocationTaxlots = ChunkingManager.CreateChunks(chunkedTaxlotsList, 1000);
                foreach (List<TaxLot> eventData in allocationTaxlots)
                {
                    e.EventData = eventData;
                    e.TopicName = Topics.Topic_Allocation;
                    CentralizePublish(e);
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
        /// Adds entry to the Audit List for the Allocated data Traded in some back date
        /// </summary>
        /// <param name="order">Not Null, Order from which the data has to be extracted</param>
        /// <param name="action">TradeAuditActionType </param>
        /// <param name="comment">Not Null, comment of the action by the user</param>
        /// <param name="companyUserId">The company user id of the user doing the changes</param>
        /// <returns></returns>
        private static List<TradeAuditEntry> AddAllocatedDataAuditEntry(TaxLot taxlot, TradeAuditActionType.ActionType action, string comment)
        {
            List<TradeAuditEntry> allocatedTradeAuditCollection = new List<TradeAuditEntry>();
            try
            {
                if (taxlot != null && comment != null)
                {
                    TradeAuditEntry newEntry = new TradeAuditEntry();
                    newEntry.Action = action;
                    newEntry.AUECLocalDate = DateTime.Now;
                    newEntry.OriginalDate = DateTime.Parse(taxlot.AUECLocalDate.ToString());
                    newEntry.Comment = comment;
                    newEntry.CompanyUserId = taxlot.CompanyUserID;
                    newEntry.Symbol = taxlot.Symbol;
                    newEntry.Level1ID = taxlot.Level1ID;
                    newEntry.GroupID = taxlot.GroupID;
                    newEntry.TaxLotClosingId = taxlot.TaxLotClosingId;
                    newEntry.TaxLotID = taxlot.TaxLotID;
                    newEntry.Level1AllocationID = taxlot.Level1AllocationID;
                    newEntry.OrderSideTagValue = taxlot.OrderSideTagValue;
                    newEntry.OriginalValue = "";
                    newEntry.Source = TradeAuditActionType.ActionSource.Allocation;
                    allocatedTradeAuditCollection.Add(newEntry);
                }
                else
                    throw new NullReferenceException("The Data Table to add in audit dictionary is null");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocatedTradeAuditCollection;
        }

        /// <summary>
        /// Publish the scheme
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="allocationSchemeId">The allocation scheme Id</param>
        internal static void PublishAllocationSchemeUpdated(int userId, int allocationSchemeId)
        {
            try
            {
                List<int> schemeResult = new List<int>();
                schemeResult.Add(userId);
                schemeResult.Add(allocationSchemeId);
                MessageData e = new MessageData() { EventData = schemeResult, TopicName = Topics.Topic_AllocationSchemeUpdated };
                CentralizePublish(e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This will publish the acknowledgement on Import UI whenever the groups will be saved.
        /// This is done to prevent the data duplicacy in case of server disconnection.
        /// </summary>
        /// <param name="isGroupSaved"></param>
        internal static void PublishImportAcknowledgment(bool isGroupSaved)
        {
            try
            {
                MessageData e = new MessageData();
                List<bool> dataList = new List<bool>();
                dataList.Add(isGroupSaved);
                e.EventData = dataList;
                e.TopicName = Topics.Topic_ImportAck;
                CentralizePublish(e);
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
        /// Centralizes the publish.
        /// </summary>
        /// <param name="msgData">The MSG data.</param>
        private static void CentralizePublish(MessageData msgData)
        {
            try
            {
                lock (_publishLock)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            ServiceProxyConnector.PublishingProxy.InnerChannel.Publish(msgData, msgData.TopicName);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Publishes the trade attribute preferences using the provided DataSet.
        /// </summary>
        /// <param name="tradeAttributePreferences">The DataSet containing trade attribute preferences.</param>
        public static void PublishTradeAttributePreferences(DataSet tradeAttributePreferences)
        {
            string serializedPreferences = JsonHelper.SerializeObject(tradeAttributePreferences);
            MessageData message = new MessageData
            {
                EventData = new List<string> { serializedPreferences },
                TopicName = Topics.Topic_UpdateTradeAttributePref
            };
            CentralizePublish(message);
        }

        /// <summary>
        /// Publish the Taxlot.
        /// </summary>
        /// <param name="taxlots">The groups.</param>
        internal static void PublishTaxlot(List<TaxLot> taxlots, string topicName)
        {
            try
            {
                MessageData e = new MessageData();
                e.EventData = taxlots;
                e.TopicName = topicName;
                CentralizePublish(e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Methods
    }
}
