using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.DataManager;
using Prana.Global;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.ServerCommon
{
    public class OrderCacheManager
    {
        static List<string> _externalTagsToUpdate = new List<string>();
        static List<string> _externalTagsToUpdateInCache = new List<string>();
        static List<string> _rejectTagsToUpdate = new List<string>();
        static List<string> _externalTagsToUpdateInReplace = new List<string>();
        static List<string> _internalTagsToUpdateInReplace = new List<string>();
        static Dictionary<string, List<PranaMessage>> _cpDownMessages = new Dictionary<string, List<PranaMessage>>();
        static private Dictionary<string, PranaMessage> _PranaMsgCollection = new Dictionary<string, PranaMessage>();
        static private Dictionary<string, MultiBrokersSubsCollection> _stagedSubCollection = new Dictionary<string, MultiBrokersSubsCollection>();
        static readonly object lockerCollection = new object();
        static private Dictionary<string, TradingInstruction> _listTradingInstruction = new Dictionary<string, TradingInstruction>();
        static ProxyBase<IPublishing> _proxyPublishing;
        static Dictionary<string, int> _newBrokers = new Dictionary<string, int>();
        private static bool _isMultiBrokerWorkFlow = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("MultiBrokerWorkflow"));
        /// <summary>
        /// Cache for storing ClOrderIds of MultiDay trades
        /// </summary>
        private static HashSet<string> _multiDayClOrderIdsCache = new HashSet<string>();
        private static Dictionary<string, string> _multiDayOrderAllocation = new Dictionary<string, string>();
        private static Dictionary<string, string> _multiDayClOrderIDMapping = new Dictionary<string, string>();
        private static Dictionary<string, string> _dictMultiDayOrderReplacedClOrderID = new Dictionary<string, string>();
        private static Dictionary<string, string> _dictMultiDayClOrderIdParentGroupIdMapping = new Dictionary<string, string>();
        private static Dictionary<string, string> _dictMultiDayGroupIdAndParentClOrderIdMapping = new Dictionary<string, string>();
        private static Dictionary<string, Dictionary<string, string>> _dictMultiBrokerTradeMapping = new Dictionary<string, Dictionary<string, string>>();

        public static Dictionary<string, MultiBrokersSubsCollection> StagedSubsCollection
        {
            get { return _stagedSubCollection; }
            set { _stagedSubCollection = value; }
        }
        public static Dictionary<string, string> DictMultiDayGroupIdAndParentClOrderIdMapping
        {
            get { return _dictMultiDayGroupIdAndParentClOrderIdMapping; }
            set { _dictMultiDayGroupIdAndParentClOrderIdMapping = value; }
        }

        public static Dictionary<string, string> DictMultiDayClOrderIdParentGroupIdMapping
        {
            get { return _dictMultiDayClOrderIdParentGroupIdMapping; }
            set { _dictMultiDayClOrderIdParentGroupIdMapping = value; }
        }

        public static Dictionary<string, string> DictMultiDayClOrderIDMapping
        {
            get { return _multiDayClOrderIDMapping; }
            set { _multiDayClOrderIDMapping = value; }
        }

        private static void CreatePublishingProxy()
        {
            try
            {
                _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy", ex), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static void FillCacheFromDataBase()
        {
            try
            {
                List<PranaMessage> listPranaMessage = CacheManagerDAL.GetInstance().GetTodaysOrders();
                foreach (PranaMessage pranaMessage in listPranaMessage)
                {
                    AddToCache(pranaMessage);
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
        static OrderCacheManager()
        {
            AddExternalTagsToUpdate();
            AddReplaceExternalTagsToUpdate();
            AddReplaceInternalTagsToUpdate();
            AddExternalTagsToUpdateInCache();
            AddRejectTagsToUpdate();
            CreatePublishingProxy();
            FillMultiDayClOrderIdCache();
            DbQueueManager.GetInstance().UpdateMultiClOrderIdsCache += new EventHandler<EventArgs<string>>(UpdateMultiClOrderIdsCacheHandler);
        }

        public static PranaMessage GetCachedOrder(string clOrderID)
        {
            PranaMessage pranaMessage = null;
            try
            {
                lock (lockerCollection)
                {
                    if (_PranaMsgCollection.ContainsKey(clOrderID))
                    {
                        pranaMessage = _PranaMsgCollection[clOrderID];
                    }
                    else
                    {
                        pranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(clOrderID);
                        if (UserSettingConstants.IsDebugModeEnabled)
                            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("DB hit for clOrderID  =" + clOrderID + " _PranaMsgCollection count= " + _PranaMsgCollection.Count, LoggingConstants.CATEGORY_WARNING, 1, 1, System.Diagnostics.TraceEventType.Warning);
                        // After picking order from db, need to add instantly into the cache. Rajat 20 Oct 2012.
                        AddToCache(pranaMessage);
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
            return pranaMessage;
        }

        public static PranaMessage GetCachedOrderByMsgSeqNumber(string MsgSeqNumber)
        {
            PranaMessage pranaMessage = null;
            try
            {
                lock (lockerCollection)
                {
                    foreach (KeyValuePair<string, PranaMessage> pranaMsgKeyValuePair in _PranaMsgCollection)
                    {
                        if (pranaMsgKeyValuePair.Value.FIXMessage.ExternalInformation[FIXConstants.TagMsgSeqNum].Value == MsgSeqNumber)
                        {
                            pranaMessage = pranaMsgKeyValuePair.Value;
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
            return pranaMessage;
        }

        public static void UpdateCachedMessage(PranaMessage pranaMessage)
        {
            try
            {
                PranaMessage origPranaMessage = GetCachedOrderForExecutionReoprt(pranaMessage);
                if (origPranaMessage == null)
                    return;
                double parentNotional = 0;

                if (pranaMessage.MessageType == FIXConstants.MSGOrderCancelReject)
                {
                    origPranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value);

                    if (!pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSymbol) && origPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSymbol))
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagSymbol, origPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value);
                    if (!pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagContractMultiplier) && origPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagContractMultiplier))
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagContractMultiplier, origPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagContractMultiplier].Value);
                }
                else if (pranaMessage.MessageType == FIXConstants.MSGExecution)
                {
                    foreach (string tag in _externalTagsToUpdateInCache)
                    {
                        if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(tag))
                        {
                            origPranaMessage.FIXMessage.ExternalInformation.AddField(tag, pranaMessage.FIXMessage.ExternalInformation[tag].Value);
                        }
                    }
                    if (origPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAvgPx) && origPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty))
                    {
                        parentNotional = double.Parse(origPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAvgPx].Value) * double.Parse(origPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                    }
                    if ((origPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType] != null) && (origPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value.Equals(FIXConstants.MSGOrderCancelReplaceRequest)))
                    {
                        origPranaMessage = GetCachedOrder(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value);
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

        public static void ModifyOrderUserID(PranaMessage pranaMessage)
        {
            try
            {
                PranaMessage origPranaMessage = GetCachedOrder(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                origPranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CompanyUserID, pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID].Value);
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
        /// Has Any Multi Day History
        /// </summary>
        /// <param name="PranaMessage"></param>
        public static bool HasMultiDayHistory(PranaMessage pranaMessage)
        {
            try
            {
                if (OrderInformation.IsMultiDayOrder(pranaMessage))
                    return true;
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID))
                {
                    string tagClorderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value.ToString();
                    //If tag 59 = 0 and ClorderId is found in the cache that means it is a previous GTC/GTD order which is replaced to Day
                    //Added this to handle TIF replace scenario wherein TIF changes from GTC/GTD to Day
                    if (StagedSubsCollection.ContainsKey(tagClorderID))
                    {
                        return true;
                    }
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                {
                    string tagOrigClorderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value.ToString();
                    if (StagedSubsCollection.ContainsKey(tagOrigClorderID))
                    {
                        return true;
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
        /// Check whether PranaMessage Exist in Cache 
        /// </summary>
        /// <param name=clOrderID></param>
        /// <returns></returns>
        public static bool DoesOrderExist(string clOrderID)
        {
            bool isValid = false;
            PranaMessage PranaMsg = GetCachedOrder(clOrderID);

            if (PranaMsg != null)
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }

        #region IServerCacheManager Members
        public static void AddToCache(PranaMessage pranaMessage)
        {
            try
            {
                if (pranaMessage == null || _PranaMsgCollection == null)
                {
                    return;
                }

                lock (lockerCollection)
                {
                    if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TradingAccountID))
                    {
                        pranaMessage.TradingAccountID = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TradingAccountID].Value;
                    }

                    string tagClOrdIDValue = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                    if (!String.IsNullOrEmpty(tagClOrdIDValue) && !_PranaMsgCollection.ContainsKey(tagClOrdIDValue))
                    {
                        _PranaMsgCollection.Add(tagClOrdIDValue, pranaMessage);
                    }

                    #region Multi Broker code commented
                    if ((_isMultiBrokerWorkFlow || HasMultiDayHistory(pranaMessage))
                        && pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PranaMsgType))
                    {
                        if (int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDNewSub ||
                            int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDNewSubChild ||
                            int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.MsgDropCopy_PM)
                        {
                            if (!String.IsNullOrEmpty(tagClOrdIDValue) && !_stagedSubCollection.ContainsKey(tagClOrdIDValue))
                            {
                                string orderStatus = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;
                                if (!(orderStatus.Equals(FIXConstants.ORDSTATUS_PendingCancel) || orderStatus.Equals(FIXConstants.ORDSTATUS_PendingRollOver) || orderStatus.Equals(FIXConstants.ORDSTATUS_PendingReplace)))
                                {
                                    int auecID = 0;
                                    MultiBrokersSubsCollection multiBrokerSubsCollection = new MultiBrokersSubsCollection();
                                    if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_AUECID))
                                    {
                                        auecID = Convert.ToInt32(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECID].Value);
                                        multiBrokerSubsCollection.AUECID = auecID;
                                        multiBrokerSubsCollection.CurrentAuecDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CommonDataCache.CachedDataManager.GetInstance.GetAUECTimeZone(auecID));
                                    }
                                    // because here CLOrderID and OrderID are same.
                                    multiBrokerSubsCollection.clOrderID = tagClOrdIDValue;

                                    if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrderID) &&
                                        int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.MsgDropCopy_PM)
                                    {
                                        multiBrokerSubsCollection.OrderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value;
                                    }
                                    else
                                    {
                                        multiBrokerSubsCollection.OrderID = tagClOrdIDValue;
                                    }
                                    _stagedSubCollection.Add(tagClOrdIDValue, multiBrokerSubsCollection);
                                }
                            }
                        }
                    }
                    #endregion
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
        ///Update StagedSubCollection On RPL
        /// </summary>
        /// <param name="pranaMessage"></param>
        /// <param name="clorderID"></param>
        /// <returns>Whether the current replace is a first time TIF replace from Day to Multi-Day</returns>
        public static bool UpdateStagedSubCollectionOnRPL(PranaMessage pranaMessage, string clorderID)
        {
            try
            {
                PranaMessage tempPranaMessage = pranaMessage;
                bool isDayToMultiDayTIFReplace = false;
                string origclOrderID = string.Empty;
                string OrderID = string.Empty;
                string parentClOrderID = string.Empty;
                bool isClOrderIdReplaced = false;
                int auecID = 0;
                Dictionary<string, MultiBrokerChildOrders> orderIDWiseClorderIDs = new Dictionary<string, MultiBrokerChildOrders>();
                double startOfDayCumQty = 0.0;
                double startOfDayAvgPx = 0.0;
                var latestclOrderId = string.Empty;

                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID))
                {
                    PranaMessage cachedMessage = GetCachedOrder(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                    if (cachedMessage != null)
                        tempPranaMessage = cachedMessage;
                }

                if (tempPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                {
                    origclOrderID = tempPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                }
                //Used the orderID of the new incoming message
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrderID))
                {
                    OrderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value;
                }
                if (tempPranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ParentClOrderID))
                {
                    parentClOrderID = tempPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value;
                }
                if (!string.IsNullOrEmpty(origclOrderID) && _stagedSubCollection.ContainsKey(origclOrderID))
                {
                    orderIDWiseClorderIDs = _stagedSubCollection[origclOrderID].dictOrderIDWiseNewCLOrderIDs;
                    startOfDayCumQty = _stagedSubCollection[origclOrderID].StartOfDayCumQty;
                    startOfDayAvgPx = _stagedSubCollection[origclOrderID].StartOfDayAveragePrice;
                    auecID = _stagedSubCollection[origclOrderID].AUECID;
                    if (orderIDWiseClorderIDs.Count > 0)
                    {
                        latestclOrderId = orderIDWiseClorderIDs.Values.OrderByDescending(x => x.clOrderID).ToList()[0].clOrderID;
                    }
                    isClOrderIdReplaced = _stagedSubCollection.Remove(origclOrderID);
                }
                double cumQty = 0.0;
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty))
                {
                    Double.TryParse(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value, out cumQty);
                }
                if (!_stagedSubCollection.ContainsKey(clorderID))
                {
                    MultiBrokersSubsCollection multiBrokerSubsCollection = new MultiBrokersSubsCollection();
                    if (tempPranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_AUECID))
                    {
                        auecID = Convert.ToInt32(tempPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECID].Value);
                    }
                    multiBrokerSubsCollection.AUECID = auecID;
                    multiBrokerSubsCollection.CurrentAuecDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CommonDataCache.CachedDataManager.GetInstance.GetAUECTimeZone(auecID));
                    multiBrokerSubsCollection.clOrderID = clorderID;
                    multiBrokerSubsCollection.OrderID = OrderID;
                    multiBrokerSubsCollection.origClOrderID = origclOrderID;
                    multiBrokerSubsCollection.parentClOrderID = parentClOrderID;
                    multiBrokerSubsCollection.dictOrderIDWiseNewCLOrderIDs = orderIDWiseClorderIDs;
                    multiBrokerSubsCollection.StartOfDayCumQty = startOfDayCumQty;
                    multiBrokerSubsCollection.StartOfDayAveragePrice = startOfDayAvgPx;
                    _stagedSubCollection.Add(clorderID, multiBrokerSubsCollection);
                    if (!string.IsNullOrEmpty(latestclOrderId))
                    {
                        SaveMultiDayClOrderIdMapping(latestclOrderId, clorderID);
                    }
                    //Updating the cache in case of Replace as well to handle the scenario wherein replace 
                    //comes on the next trading day
                    CacheManagerDAL.CalculateMultiDayFieldsInCache(_stagedSubCollection[clorderID]);
                    //An Order's cumQty is greater than 0 which means the child orders count must be greater than 0.
                    //If it is not then it means that the order was previosuly a Day Order
                    if (_stagedSubCollection[clorderID].dictOrderIDWiseNewCLOrderIDs.Count == 0)
                    {
                        if (!string.IsNullOrEmpty(parentClOrderID))
                        {
                            // Fetch groupId of parentclorderid
                            var parentGroupId = _dictMultiDayGroupIdAndParentClOrderIdMapping[parentClOrderID];
                            if (!string.IsNullOrEmpty(parentGroupId) && !DictMultiDayClOrderIdParentGroupIdMapping.ContainsKey(clorderID))
                            {
                                DictMultiDayClOrderIdParentGroupIdMapping.Add(clorderID, parentGroupId);
                            }
                            SaveMultiDayClOrderIdMapping(parentClOrderID, clorderID);
                        }
                        if (cumQty > 0)
                        {
                            isDayToMultiDayTIFReplace = true;
                        }
                        isClOrderIdReplaced = true;
                    }
                }
                else
                {
                    _stagedSubCollection[clorderID].OrderID = OrderID;

                    //An Order's cumQty is greater than 0 which means the child orders count must be greater than 0.
                    //If it is not then it means that the order was previosuly a Day Order
                    if ((_stagedSubCollection[clorderID].dictOrderIDWiseNewCLOrderIDs == null || _stagedSubCollection[clorderID].dictOrderIDWiseNewCLOrderIDs.Count == 0))
                    {
                        if (!string.IsNullOrEmpty(parentClOrderID))
                        {
                            SaveMultiDayClOrderIdMapping(parentClOrderID, clorderID);
                        }
                        if (cumQty > 0)
                        {
                            isDayToMultiDayTIFReplace = true;
                        }
                        isClOrderIdReplaced = true;
                    }
                }
                if (isClOrderIdReplaced)
                {
                    SaveMultiDayOrderReplacedClOrderId(origclOrderID, clorderID);
                    var groupid = GetMultiDayOrderAllocationByClOrderid(origclOrderID);

                    if (!string.IsNullOrEmpty(latestclOrderId))
                    {
                        SaveMultiDayClOrderIdMapping(latestclOrderId, clorderID);
                        SaveMultiDayOrderAllocation(clorderID, groupid);
                    }
                }
                return isDayToMultiDayTIFReplace;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        private static void AddExternalTagsToUpdate()
        {
            _externalTagsToUpdate.Add(FIXConstants.TagPrice);
            _externalTagsToUpdate.Add(FIXConstants.TagTimeInForce);
            _externalTagsToUpdate.Add(FIXConstants.TagExecInst);
            _externalTagsToUpdate.Add(FIXConstants.TagHandlInst);
            _externalTagsToUpdate.Add(FIXConstants.TagSide);
            _externalTagsToUpdate.Add(FIXConstants.TagStopPx);
        }

        private static void AddRejectTagsToUpdate()
        {
            _rejectTagsToUpdate.Add(FIXConstants.TagCumQty);
            _rejectTagsToUpdate.Add(FIXConstants.TagOrdType);
            _rejectTagsToUpdate.Add(FIXConstants.TagOrderQty);
            _rejectTagsToUpdate.Add(FIXConstants.TagAvgPx);
            _rejectTagsToUpdate.Add(FIXConstants.TagExpireTime);
        }

        private static void AddExternalTagsToUpdateInCache()
        {
            _externalTagsToUpdateInCache.Add(FIXConstants.TagCumQty);
            _externalTagsToUpdateInCache.Add(FIXConstants.TagLeavesQty);
            _externalTagsToUpdateInCache.Add(FIXConstants.TagLastShares);
            _externalTagsToUpdateInCache.Add(FIXConstants.TagOrderID);
            _externalTagsToUpdateInCache.Add(FIXConstants.TagOrdStatus);
            _externalTagsToUpdateInCache.Add(FIXConstants.TagOrderID);
            _externalTagsToUpdateInCache.Add(FIXConstants.TagLastPx);
            _externalTagsToUpdateInCache.Add(FIXConstants.TagAvgPx);
        }

        public static PranaMessage GetCachedOrderForExecutionReoprt(PranaMessage pranaMessage)
        {
            PranaMessage cachedPranaMessage = null;
            try
            {
                if (pranaMessage.MessageType == FIXConstants.MSGOrderCancelReject)
                {
                    cachedPranaMessage = GetCachedOrder(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value);
                }
                else // Execution Report
                {
                    cachedPranaMessage = GetCachedOrder(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
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
            return cachedPranaMessage;
        }

        public static void UpdateExecutionReport(PranaMessage pranaMessage)
        {
            try
            {
                int nirvanaMsgType = (int)OrderFields.PranaMsgTypes.InternalOrder;
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PranaMsgType))
                {
                    nirvanaMsgType = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                }

                if (nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDManual || nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub)
                {
                    if (double.Parse(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value.ToString()) == 0.0
                        && !(pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagLastShares)))
                    {
                        CacheManagerDAL.GetInstance().DeleteOldManualFills(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value);
                    }
                }

                PranaMessage cachedPranaMessage = GetCachedOrderForExecutionReoprt(pranaMessage);

                if (cachedPranaMessage == null)
                    return;

                string origCounterPartyID = string.Empty;
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OrigCounterPartyID))
                {
                    origCounterPartyID = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OrigCounterPartyID].Value;
                }

                string text = string.Empty;
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(FIXConstants.TagText))
                    text = pranaMessage.FIXMessage.InternalInformation[FIXConstants.TagText].Value;

                pranaMessage.FIXMessage.CloneInternalInfo(cachedPranaMessage.FIXMessage);

                if (pranaMessage.MessageType == FIXConstants.MSGOrderCancelReject)
                {
                    foreach (string tag in _rejectTagsToUpdate)
                    {
                        if (cachedPranaMessage.FIXMessage.ExternalInformation.ContainsKey(tag))
                        {
                            pranaMessage.FIXMessage.ExternalInformation.AddField(tag, cachedPranaMessage.FIXMessage.ExternalInformation[tag].Value);
                        }
                    }
                }
                // copy necessary external items from cache
                foreach (string tag in _externalTagsToUpdate)
                {
                    if (cachedPranaMessage.FIXMessage.ExternalInformation.ContainsKey(tag))
                    {
                        pranaMessage.FIXMessage.ExternalInformation.AddField(tag, cachedPranaMessage.FIXMessage.ExternalInformation[tag].Value);
                    }
                }

                pranaMessage.FIXMessage.InternalInformation.AddField(FIXConstants.TagText, text);
                pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrigCounterPartyID, origCounterPartyID);
                // PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrderQty, cachedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
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

        public static void UpdateReplaceOrder(PranaMessage pranaMessage)
        {
            try
            {
                PranaMessage cachedPranaMessage = null;
                cachedPranaMessage = GetCachedOrder(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value);

                if (cachedPranaMessage == null)
                    return;
                int pranaMessageTypeTag = GetPranaMessageTypeTag(pranaMessage);

                foreach (string externalTag in _externalTagsToUpdateInReplace)
                {
                    string value = string.Empty;
                    if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(externalTag))
                    {
                        value = pranaMessage.FIXMessage.ExternalInformation[externalTag].Value;
                    }
                    else
                    {
                        pranaMessage.FIXMessage.ExternalInformation.AddField(externalTag, string.Empty);
                    }
                    if ((value == string.Empty || value == ApplicationConstants.C_COMBO_SELECT)
                        && cachedPranaMessage.FIXMessage.ExternalInformation.ContainsKey(externalTag)
                        && !(pranaMessageTypeTag == (int)OrderFields.PranaMsgTypes.ORDStaged && pranaMessage.MessageType == FIXConstants.MSGOrderCancelReplaceRequest))
                    {
                        pranaMessage.FIXMessage.ExternalInformation[externalTag].Value = cachedPranaMessage.FIXMessage.ExternalInformation[externalTag].Value;
                    }
                }

                foreach (string internalTag in _internalTagsToUpdateInReplace)
                {
                    if (cachedPranaMessage.FIXMessage.InternalInformation.ContainsKey(internalTag))
                    {
                        string value = string.Empty;
                        if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(internalTag))
                        {
                            value = pranaMessage.FIXMessage.InternalInformation[internalTag].Value;
                        }
                        else
                        {
                            pranaMessage.FIXMessage.InternalInformation.AddField(internalTag, string.Empty);
                        }

                        if (value == string.Empty || value == ApplicationConstants.C_COMBO_SELECT
                            && !(pranaMessageTypeTag == (int)OrderFields.PranaMsgTypes.ORDStaged && pranaMessage.MessageType == FIXConstants.MSGOrderCancelReplaceRequest))
                        {
                            pranaMessage.FIXMessage.InternalInformation[internalTag].Value = cachedPranaMessage.FIXMessage.InternalInformation[internalTag].Value;
                        }
                    }
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

        private static int GetPranaMessageTypeTag(PranaMessage pranaMessage)
        {
            string pranaMessageType;
            int pranaMessageTypeTag = int.MinValue;
            try
            {
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PranaMsgType))
                {
                    pranaMessageType = pranaMessage.FIXMessage.InternalInformation.GetField(CustomFIXConstants.CUST_TAG_PranaMsgType);
                    int.TryParse(pranaMessageType, out pranaMessageTypeTag);
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
            return pranaMessageTypeTag;
        }

        private static void AddReplaceExternalTagsToUpdate()
        {
            _externalTagsToUpdateInReplace.Add(FIXConstants.TagExecInst);
            _externalTagsToUpdateInReplace.Add(FIXConstants.TagHandlInst);
            _externalTagsToUpdateInReplace.Add(FIXConstants.TagTimeInForce);
        }

        private static void AddReplaceInternalTagsToUpdate()
        {
            _internalTagsToUpdateInReplace.Add(CustomFIXConstants.CUST_TAG_Level1ID);
            _internalTagsToUpdateInReplace.Add(CustomFIXConstants.CUST_TAG_Level2ID);
        }

        public static void DeleteFromCache(string clOrderID)
        {
            try
            {
                lock (lockerCollection)
                {
                    if (_PranaMsgCollection.ContainsKey(clOrderID))
                    {
                        _PranaMsgCollection.Remove(clOrderID);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// for drop copy sub orders
        /// </summary>
        /// <param name=pranaMessage></param>
        public static void UpdateChildDetailsFromParent(PranaMessage pranaMessage)
        {
            try
            {
                lock (lockerCollection)
                {
                    if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                    {
                        string stagedOrderID = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;

                        if (_PranaMsgCollection.ContainsKey(stagedOrderID))
                        {
                            PranaMessage parentPranaMessage = _PranaMsgCollection[stagedOrderID];
                            pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OriginatorType, parentPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OriginatorType].Value);
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
        #endregion

        public static void AddCpDownOrders(PranaMessage pranaMessage)
        {
            PranaMessage cachedPranaMessage = GetCachedOrderForExecutionReoprt(pranaMessage);

            if (cachedPranaMessage == null)
                return;

            foreach (MessageField messageField in cachedPranaMessage.FIXMessage.ExternalInformation.MessageFields)
            {
                if (!pranaMessage.FIXMessage.ExternalInformation.ContainsKey(messageField.Tag))
                {
                    pranaMessage.FIXMessage.ExternalInformation.AddField(messageField.Tag, messageField.Value);
                }
            }
            string cpID = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value;
            if (!_cpDownMessages.ContainsKey(cpID))
            {
                _cpDownMessages.Add(cpID, new List<PranaMessage>());
            }
            _cpDownMessages[cpID].Add(pranaMessage);
        }

        public static List<PranaMessage> GetCpDownMessages(string cpID)
        {
            string waitingText = "Waiting for User Response for Sending Back";
            if (_cpDownMessages.ContainsKey(cpID))
            {
                List<PranaMessage> downMsgs = _cpDownMessages[cpID];
                foreach (PranaMessage pranaMsg in downMsgs)
                {
                    pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagText, waitingText);
                }
                return downMsgs;
            }
            else
            {
                return null;
            }
        }

        public static List<PranaMessage> GetCachedMessages()
        {
            List<PranaMessage> pranaMsgList = new List<PranaMessage>();
            lock (lockerCollection)
            {
                foreach (KeyValuePair<string, PranaMessage> item in _PranaMsgCollection)
                {
                    pranaMsgList.Add(item.Value);
                }
            }
            return pranaMsgList;
        }

        public static PranaMessage UpdateSubMessage(PranaMessage pranaMessage)
        {
            PranaMessage orderMessage = null;
            try
            {
                string clorderIDReceived = string.Empty;
                string orderIDReceived = string.Empty;
                string newCLOrderID = string.Empty;
                string broker = string.Empty;
                double CumQTY = 0.0;
                double AVGPrice = 0.0;
                DateTime AuecLocalDate = DateTime.UtcNow;
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID))
                {
                    clorderIDReceived = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrderID))
                {
                    orderIDReceived = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecBroker))
                {
                    broker = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecBroker].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty))
                {
                    CumQTY = Convert.ToDouble(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAvgPx))
                {
                    AVGPrice = Convert.ToDouble(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAvgPx].Value);
                }
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_AUECLocalDate))
                {
                    AuecLocalDate = Convert.ToDateTime(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECLocalDate].Value);
                }
                string stagedOrderID = clorderIDReceived;

                if (_stagedSubCollection.ContainsKey(clorderIDReceived))
                {
                    string orderStatus = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;
                    string execStatus = orderStatus;
                    if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecType))
                    {
                        execStatus = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecType].Value;
                    }
                    if (orderIDReceived == _stagedSubCollection[clorderIDReceived].OrderID && (orderStatus == FIXConstants.ORDSTATUS_New || orderStatus == FIXConstants.ORDSTATUS_Replaced || execStatus == FIXConstants.ORDSTATUS_New || execStatus == FIXConstants.ORDSTATUS_Replaced))
                    {
                        _stagedSubCollection[clorderIDReceived].orderStatus = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;
                    }

                    if (orderIDReceived != _stagedSubCollection[clorderIDReceived].OrderID && (orderStatus == FIXConstants.ORDSTATUS_New || orderStatus == FIXConstants.ORDSTATUS_Replaced || execStatus == FIXConstants.ORDSTATUS_New || execStatus == FIXConstants.ORDSTATUS_Replaced))
                    {
                        if (orderStatus == FIXConstants.ORDSTATUS_Replaced || execStatus == FIXConstants.ORDSTATUS_Replaced)
                        {
                            _stagedSubCollection[clorderIDReceived].orderStatus = FIXConstants.ORDSTATUS_Replaced;
                            _stagedSubCollection[clorderIDReceived].OrderID = orderIDReceived;
                        }
                        else
                        {
                            if (_stagedSubCollection[clorderIDReceived].orderStatus.Equals(FIXConstants.ORDSTATUS_PendingNew))
                            {
                                _stagedSubCollection[clorderIDReceived].orderStatus = FIXConstants.ORDSTATUS_New;
                                _stagedSubCollection[clorderIDReceived].OrderID = orderIDReceived;
                            }
                            else if (_stagedSubCollection[clorderIDReceived].orderStatus.Equals(FIXConstants.ORDSTATUS_New) || _stagedSubCollection[clorderIDReceived].orderStatus.Equals(FIXConstants.ORDSTATUS_Replaced))
                            {
                                pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = FIXConstants.ORDSTATUS_PartiallyFilled;
                                orderStatus = FIXConstants.ORDSTATUS_PartiallyFilled;
                                execStatus = FIXConstants.ORDSTATUS_PartiallyFilled;
                            }
                        }
                    }
                    if (orderIDReceived != _stagedSubCollection[clorderIDReceived].OrderID
                        && orderStatus != FIXConstants.ORDSTATUS_New && orderStatus != FIXConstants.ORDSTATUS_Replaced && orderStatus != FIXConstants.ORDSTATUS_Cancelled && orderStatus != FIXConstants.ORDSTATUS_RollOver && orderStatus != FIXConstants.ORDSTATUS_PendingCancel && orderStatus != FIXConstants.ORDSTATUS_PendingRollOver && orderStatus != FIXConstants.ORDSTATUS_PendingNew && orderStatus != FIXConstants.ORDSTATUS_PendingReplace && orderStatus != FIXConstants.ORDSTATUS_Rejected
                        && execStatus != FIXConstants.ORDSTATUS_New && execStatus != FIXConstants.ORDSTATUS_Replaced && execStatus != FIXConstants.ORDSTATUS_Cancelled && execStatus != FIXConstants.ORDSTATUS_PendingCancel && execStatus != FIXConstants.ORDSTATUS_PendingNew && execStatus != FIXConstants.ORDSTATUS_PendingReplace)
                    {
                        if ((orderStatus == FIXConstants.ORDSTATUS_DoneForDay || execStatus == FIXConstants.ORDSTATUS_DoneForDay) && _stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs != null && !_stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs.ContainsKey(orderIDReceived))
                        {
                            return null;
                        }
                        //Code is -> If the collection of child suborders is empty that means first child order of Sub-Order so create new or else
                        // if the incoming orderId is different from the existing child orders
                        //_stagedSubCollection[clorderIDReceived] -> Main Sub Order
                        //_stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs -> Collection of Sub-Orders
                        if (_stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs == null)
                        {
                            newCLOrderID = UniqueIDGenerator.GetClOrderID();
                            SaveMultiDayClOrderIdMapping(newCLOrderID, clorderIDReceived);
                            _stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs = new Dictionary<string, MultiBrokerChildOrders>();
                            MultiBrokerChildOrders multiBrokerChildOrders = new MultiBrokerChildOrders();
                            multiBrokerChildOrders.clOrderID = newCLOrderID;
                            multiBrokerChildOrders.parentClOrderID = newCLOrderID;
                            multiBrokerChildOrders.CumQty = CumQTY;
                            multiBrokerChildOrders.AveragePrice = AVGPrice;
                            multiBrokerChildOrders.AuecLocalDate = AuecLocalDate;


                            _stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs.Add(orderIDReceived, multiBrokerChildOrders);

                            pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = newCLOrderID;
                            pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value = newCLOrderID;
                            pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.ORDNewSubChild).ToString());

                            if (!string.IsNullOrEmpty(_stagedSubCollection[clorderIDReceived].parentClOrderID))
                            {
                                stagedOrderID = _stagedSubCollection[clorderIDReceived].parentClOrderID;
                            }

                            pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_StagedOrderID, stagedOrderID);

                            if (!string.IsNullOrEmpty(_stagedSubCollection[clorderIDReceived].origClOrderID))
                            {
                                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrigClOrdID, _stagedSubCollection[clorderIDReceived].origClOrderID);
                                _stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].origClOrderID = _stagedSubCollection[clorderIDReceived].origClOrderID;
                            }

                            orderMessage = pranaMessage.Clone();
                            orderMessage.MessageType = FIXConstants.MSGOrder;
                            orderMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.ORDNewSubChild).ToString());
                            orderMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGOrder);
                            orderMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value = _stagedSubCollection[clorderIDReceived].OrderID;
                            //In case of new sub generation, setting CumQty to Zero
                            orderMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = "0.0";
                            AddToCache(orderMessage);
                            if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsMultiBrokerTrade)
                                && bool.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsMultiBrokerTrade].Value))
                            {
                                UpdateMultiBrokerTradeDetailsForCurrentDay(clorderIDReceived);
                            }

                        }
                        else if (!_stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs.ContainsKey(orderIDReceived))
                        {
                            newCLOrderID = UniqueIDGenerator.GetClOrderID();
                            SaveMultiDayClOrderIdMapping(newCLOrderID, clorderIDReceived);
                            MultiBrokerChildOrders multiBrokerChildOrders = new MultiBrokerChildOrders();
                            multiBrokerChildOrders.clOrderID = newCLOrderID;
                            multiBrokerChildOrders.parentClOrderID = newCLOrderID;
                            multiBrokerChildOrders.CumQty = CumQTY;
                            multiBrokerChildOrders.AveragePrice = AVGPrice;
                            multiBrokerChildOrders.AuecLocalDate = AuecLocalDate;

                            _stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs.Add(orderIDReceived, multiBrokerChildOrders);

                            pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = newCLOrderID;
                            pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value = newCLOrderID;
                            pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.ORDNewSubChild).ToString());

                            if (!string.IsNullOrEmpty(_stagedSubCollection[clorderIDReceived].parentClOrderID))
                            {
                                stagedOrderID = _stagedSubCollection[clorderIDReceived].parentClOrderID;
                            }

                            pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_StagedOrderID, stagedOrderID);

                            if (!string.IsNullOrEmpty(_stagedSubCollection[clorderIDReceived].origClOrderID))
                            {
                                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrigClOrdID, _stagedSubCollection[clorderIDReceived].origClOrderID);
                                _stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].origClOrderID = _stagedSubCollection[clorderIDReceived].origClOrderID;
                            }

                            orderMessage = pranaMessage.Clone();
                            orderMessage.MessageType = FIXConstants.MSGOrder;
                            orderMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.ORDNewSubChild).ToString());
                            orderMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGOrder);
                            orderMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value = _stagedSubCollection[clorderIDReceived].OrderID;
                            //In case of new sub generation, setting CumQty to Zero
                            orderMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = "0.0";
                            AddToCache(orderMessage);
                            if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsMultiBrokerTrade)
                                && bool.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsMultiBrokerTrade].Value))
                            {
                                UpdateMultiBrokerTradeDetailsForCurrentDay(clorderIDReceived);
                            }
                        }
                        else
                        {
                            pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = _stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].clOrderID;
                            if (!DictMultiDayClOrderIDMapping.ContainsKey(_stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].clOrderID))
                            {
                                SaveMultiDayClOrderIdMapping(_stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].clOrderID, clorderIDReceived);
                            }
                            pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value = _stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].parentClOrderID;
                            pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrigClOrdID, _stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].origClOrderID);
                            pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.ORDNewSubChild).ToString());
                            _stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].CumQty = CumQTY;
                            _stagedSubCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].AveragePrice = AVGPrice;
                            if (!string.IsNullOrEmpty(_stagedSubCollection[clorderIDReceived].parentClOrderID))
                            {
                                stagedOrderID = _stagedSubCollection[clorderIDReceived].parentClOrderID;
                            }
                            pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_StagedOrderID, stagedOrderID);

                            if (!string.IsNullOrEmpty(_stagedSubCollection[clorderIDReceived].origClOrderID) && pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value != int.MinValue.ToString() && !string.IsNullOrEmpty(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value))
                            {
                                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrigClOrdID, _stagedSubCollection[clorderIDReceived].origClOrderID);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(broker))
                    {
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_TradeAttribute6, broker);
                    }
                }
                return orderMessage;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Updates Tag 39 from old pranamessage
        /// </summary>
        /// <param name="pranaMessage"></param>
        public static void UpdateOrderStatus(PranaMessage pranaMessage)
        {
            try
            {
                PranaMessage cachedPranaMessage = GetCachedOrderForExecutionReoprt(pranaMessage);
                if (cachedPranaMessage == null)
                    return;

                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, cachedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value);
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
        /// In case of multi broker update stage order id for the main order.
        /// </summary>
        /// <param name="pranaMessage"></param>
        /// <returns></returns>
        public static string GetOriginalClOrderIdForEsper(PranaMessage pranaMessage)
        {
            try
            {
                string clorderIDReceived = string.Empty;
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                {
                    clorderIDReceived = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;
                }

                if (_stagedSubCollection.ContainsKey(clorderIDReceived))
                {
                    PranaMessage temp = GetCachedOrder(clorderIDReceived);
                    if (temp.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                    {
                        pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value = clorderIDReceived;
                        clorderIDReceived = temp.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;
                    }
                }
                return clorderIDReceived;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// In case of replace chaining updating original id to main order id
        /// </summary>
        /// <param name="message"></param>
        public static void UpdateOriginalIdOnReplace(PranaMessage message)
        {
            try
            {
                string clOrderIdRecieved = string.Empty;
                if ((message.MessageType == FIXConstants.MSGOrderCancelRequest
                    || message.MessageType == FIXConstants.MSGOrderRollOverRequest
                    || message.MessageType == FIXConstants.MSGOrderCancelReplaceRequest)
                    && message.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                {
                    clOrderIdRecieved = message.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                    PranaMessage temp = GetCachedOrder(clOrderIdRecieved);

                    bool breakReverseTraverseLoop = false;
                    while (!breakReverseTraverseLoop)
                    {
                        if (temp.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                        {
                            clOrderIdRecieved = temp.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                            temp = GetCachedOrder(clOrderIdRecieved);
                        }
                        else if (temp.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                        {
                            clOrderIdRecieved = temp.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;
                            temp = GetCachedOrder(clOrderIdRecieved);
                        }
                        else
                        {
                            breakReverseTraverseLoop = true;
                        }
                    }

                    message.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value = clOrderIdRecieved;
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
        /// For updating cache with response ClOrderId received from DbQueueManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void UpdateMultiClOrderIdsCacheHandler(object sender, EventArgs<string> e)
        {
            try
            {
                _multiDayClOrderIdsCache.Add(e.Value);
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
        /// Check if fill is present already for given ClOrderID
        /// </summary>
        /// <param name="clOrderID"></param>
        /// <returns>True if _multiDayClOrderIdsCache contains clOrderID, if does not contain then False</returns>
        public static bool IsFillForClOrderIDPresent(string clOrderID)
        {
            try
            {
                if (!_multiDayClOrderIdsCache.Contains(clOrderID))
                {
                    if (!CacheManagerDAL.GetInstance().IsFillForClOrderIDPresent(clOrderID))
                    {
                        return false;
                    }
                    _multiDayClOrderIdsCache.Add(clOrderID);
                }
                return true;
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
            return false;
        }

        /// <summary>
        /// Initialize cache having ClOrderIDs of existing MultiDay trades
        /// </summary>
        private static void FillMultiDayClOrderIdCache()
        {
            try
            {
                _multiDayClOrderIdsCache = CacheManagerDAL.GetInstance().GetMultiDayClOrderIdCache();
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
        /// This method is to fill multi day order allocation cache
        /// </summary>
        public static void FillMultiDayOrderAllocationCache()
        {
            try
            {
                _multiDayOrderAllocation = CacheManagerDAL.GetInstance().GetMultiDayOrderAllocations();
                _multiDayClOrderIDMapping = CacheManagerDAL.GetInstance().GetMultiDayClOrderIdMapping();
                _dictMultiDayGroupIdAndParentClOrderIdMapping = CacheManagerDAL.GetInstance().GetParentGroupIdAndParentClOrderId();
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
        /// This method is to save multi day order allocation
        /// </summary>
        /// <param name="clOrderid"></param>
        /// <param name="allocationPrefId"></param>
        public static void SaveMultiDayOrderAllocation(string clOrderid, string groupId)
        {
            try
            {
                if (CacheManagerDAL.GetInstance().SaveMultiDayOrderAllocation(clOrderid, groupId) > 0)
                {
                    if (_multiDayOrderAllocation.ContainsKey(clOrderid))
                    {
                        _multiDayOrderAllocation[clOrderid] = groupId;
                    }
                    else
                    {
                        _multiDayOrderAllocation.Add(clOrderid, groupId);
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

        /// <summary>
        /// Save mapping of latest clOrderId with parentcloOrderId for multiDay orders.
        /// </summary>
        /// <param name="latestClOrderId"></param>
        /// <param name="parentClOrderId"></param>
        public static void SaveMultiDayClOrderIdMapping(string latestClOrderId, string parentClOrderId)
        {
            try
            {
                if (CacheManagerDAL.GetInstance().SaveMultiDayClOrderIdMapping(latestClOrderId, parentClOrderId) > 0)
                {
                    if (_multiDayClOrderIDMapping.ContainsKey(latestClOrderId))
                    {
                        _multiDayClOrderIDMapping[latestClOrderId] = parentClOrderId;
                    }
                    else
                    {
                        _multiDayClOrderIDMapping.Add(latestClOrderId, parentClOrderId);
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

        /// <summary>
        /// This method is to get multi day order allocation by clOrderId
        /// </summary>
        /// <param name="clOrderId"></param>
        /// <returns>int</returns>
        public static string GetMultiDayOrderAllocationByClOrderid(string clOrderId)
        {
            try
            {
                if (_multiDayOrderAllocation != null && _multiDayOrderAllocation.ContainsKey(clOrderId))
                {
                    return _multiDayOrderAllocation[clOrderId];
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
            return null;
        }

        private static Dictionary<string, string> _dictMultiDayOrderOriginalClOrderIdAfterReplace = null;
        public static Dictionary<string, string> DictMultiDayOrderOriginalClOrderIdAfterReplace
        {
            get { return _dictMultiDayOrderOriginalClOrderIdAfterReplace; }
        }

        /// <summary>
        /// This method is to fill multi day order clorderid cache
        /// </summary>
        public static void FillMultiDayOrderReplacedClOrderIdCache()
        {
            try
            {
                _dictMultiDayOrderReplacedClOrderID = CacheManagerDAL.GetInstance().GetMultiDayOrderReplacedClOrderIds(out _dictMultiDayOrderOriginalClOrderIdAfterReplace);
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
        /// This method is to save multi day order replaced clorderid
        /// </summary>
        /// <param name="originalClOrderId"></param>
        /// <param name="clOrderid"></param>
        public static void SaveMultiDayOrderReplacedClOrderId(string originalClOrderId, string clOrderid)
        {
            try
            {
                if (_dictMultiDayOrderOriginalClOrderIdAfterReplace.ContainsKey(originalClOrderId))
                {
                    originalClOrderId = _dictMultiDayOrderOriginalClOrderIdAfterReplace[originalClOrderId];
                }
                if (CacheManagerDAL.GetInstance().SaveMultiDayOrderReplacedClOrderId(originalClOrderId, clOrderid) > 0)
                {
                    if (_dictMultiDayOrderReplacedClOrderID.ContainsKey(originalClOrderId))
                    {
                        if (_dictMultiDayOrderOriginalClOrderIdAfterReplace.ContainsKey(_dictMultiDayOrderReplacedClOrderID[originalClOrderId]))
                        {
                            _dictMultiDayOrderOriginalClOrderIdAfterReplace.Remove(_dictMultiDayOrderReplacedClOrderID[originalClOrderId]);
                        }
                        _dictMultiDayOrderReplacedClOrderID[originalClOrderId] = clOrderid;
                    }
                    else
                    {
                        _dictMultiDayOrderReplacedClOrderID.Add(originalClOrderId, clOrderid);
                    }
                    _dictMultiDayOrderOriginalClOrderIdAfterReplace.Add(clOrderid, originalClOrderId);
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
        /// This method is to get multi day order replaced clorderid
        /// </summary>
        /// <param name="originalClOrderId"></param>
        /// <returns>string - replaced clorderid</returns>
        public static string GetMultiDayOrderReplacedClOrderId(string originalClOrderId)
        {
            try
            {
                if (_dictMultiDayOrderReplacedClOrderID != null && _dictMultiDayOrderReplacedClOrderID.ContainsKey(originalClOrderId))
                {
                    return _dictMultiDayOrderReplacedClOrderID[originalClOrderId];
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
            return null;
        }

        /// <summary>
        /// This method is to save multi broker trade detail
        /// </summary>
        /// <param name="parentClOrderId"></param>
        /// <param name="counterPartyId"></param>
        /// <param name="clOrderId"></param>
        public static void SaveMultiBrokerTradeDetail(string parentClOrderId, string counterPartyId, string clOrderId)
        {
            try
            {
                if (CacheManagerDAL.GetInstance().SaveMultiBrokerTradeDetails(parentClOrderId, int.Parse(counterPartyId), clOrderId) > 0)
                {
                    if (!_dictMultiBrokerTradeMapping.ContainsKey(parentClOrderId))
                    {
                        _dictMultiBrokerTradeMapping.Add(parentClOrderId, new Dictionary<string, string>());
                    }
                    if (!_dictMultiBrokerTradeMapping[parentClOrderId].ContainsKey(counterPartyId))
                    {
                        _dictMultiBrokerTradeMapping[parentClOrderId].Add(counterPartyId, clOrderId);
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

        /// <summary>
        /// This method is to fill multi broker trade mapping cache
        /// </summary>
        public static void FillMultiBrokerTradeMappingCache()
        {
            try
            {
                _dictMultiBrokerTradeMapping = CacheManagerDAL.GetInstance().GetMultiBrokerTradeMapping();
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
        /// This method updates the cache for exec broker mapping
        /// </summary>
        /// <param name="pranaMessage"></param>
        public static void AddToExecBrokerCache(PranaMessage pranaMessage)
        {
            try
            {
                string clOrderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;

                if (!_dictMultiBrokerTradeMapping.ContainsKey(clOrderID))
                {
                    _dictMultiBrokerTradeMapping[clOrderID] = new Dictionary<string, string>();
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
        /// This method updates the sub message as per exec broker and creates/updates entry
        /// </summary>
        /// <param name="pranaMessage"></param>
        /// <returns></returns>
        public static PranaMessage UpdateSubMessageForExecBroker(PranaMessage pranaMessage)
        {
            PranaMessage orderMessage = null;
            if (!pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsMultiBrokerTrade)
                        || !pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecBroker)
                        || !bool.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsMultiBrokerTrade].Value))
            {
                return orderMessage;
            }

            try
            {
                string clorderIDReceived = string.Empty;
                string orderIDReceived = string.Empty;
                string newCLOrderID = string.Empty;
                string broker = string.Empty;
                string brokerID = string.Empty;
                string idToCheckInCache = string.Empty;
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID))
                {
                    clorderIDReceived = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                    idToCheckInCache = clorderIDReceived;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecBroker))
                {
                    broker = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecBroker].Value;
                }
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CounterPartyID))
                {
                    brokerID = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value;
                }

                if (HasMultiDayHistory(pranaMessage))
                {
                    idToCheckInCache = _multiDayClOrderIDMapping[clorderIDReceived];
                }

                if (_dictMultiBrokerTradeMapping.ContainsKey(idToCheckInCache))
                {
                    if (!_dictMultiBrokerTradeMapping[idToCheckInCache].ContainsKey(brokerID))
                    {
                        newCLOrderID = UniqueIDGenerator.GetClOrderID();
                        SaveMultiBrokerTradeDetail(idToCheckInCache, brokerID, newCLOrderID);
                        _dictMultiBrokerTradeMapping[idToCheckInCache][brokerID] = newCLOrderID;
                        pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = newCLOrderID;
                        pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value = newCLOrderID;
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_StagedOrderID, clorderIDReceived);
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.ORDNewSubChild).ToString());
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagLastShares].Value);
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAvgPx, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagLastPx].Value);

                        orderMessage = pranaMessage.Clone();
                        orderMessage.MessageType = FIXConstants.MSGOrder;
                        orderMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.ORDNewSubChild).ToString());
                        orderMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGOrder);
                        orderMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_StagedOrderID, clorderIDReceived);
                        orderMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value = clorderIDReceived;
                        AddToCache(orderMessage);
                    }
                    else
                    {
                        pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = _dictMultiBrokerTradeMapping[idToCheckInCache][brokerID];
                        pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value = _dictMultiBrokerTradeMapping[idToCheckInCache][brokerID];
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.ORDNewSubChild).ToString());
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_StagedOrderID, clorderIDReceived);
                        SetQtyAndPrice(pranaMessage);
                    }

                    if (!string.IsNullOrEmpty(broker))
                    {
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_TradeAttribute6, broker);
                    }
                }
                return orderMessage;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// This method calculates and sets the correct qty and price for exec broker msg
        /// </summary>
        /// <param name="pranaMessage"></param>
        private static void SetQtyAndPrice(PranaMessage pranaMessage)
        {
            try
            {
                string clOrderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                if (_PranaMsgCollection == null || !_PranaMsgCollection.ContainsKey(clOrderID))
                    return;
                lock (lockerCollection)
                {
                    PranaMessage cachedMsg = _PranaMsgCollection[clOrderID];

                    // Previous state
                    int lastFilledQty = Convert.ToInt32(cachedMsg.FIXMessage.ExternalInformation[FIXConstants.TagCumQty]?.Value ?? "0");
                    decimal lastAvgPx = Convert.ToDecimal(cachedMsg.FIXMessage.ExternalInformation[FIXConstants.TagAvgPx]?.Value ?? "0");

                    // Current fill
                    int currentFillQty = Convert.ToInt32(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagLastShares].Value);
                    decimal currentPx = Convert.ToDecimal(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagLastPx].Value);

                    // New cumulative quantity
                    int cumQty = lastFilledQty + currentFillQty;

                    // New average price calculation
                    decimal totalValue = (lastFilledQty * lastAvgPx) + (currentFillQty * currentPx);
                    decimal avgPx = cumQty > 0 ? totalValue / cumQty : 0;

                    // Update fields
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, cumQty.ToString());
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAvgPx, avgPx.ToString());

                    _PranaMsgCollection[clOrderID].FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, cumQty.ToString());
                    _PranaMsgCollection[clOrderID].FIXMessage.ExternalInformation.AddField(FIXConstants.TagAvgPx, avgPx.ToString());
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

        public static void UpdateMultiBrokerTradeDetailsForCurrentDay(string parentClOrderId)
        {
            try
            {
                CacheManagerDAL.GetInstance().UpdateMultiBrokerTradeDetailsForCurrentDay(parentClOrderId);
                _dictMultiBrokerTradeMapping[parentClOrderId] = new Dictionary<string, string>();
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
