using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.DataManager;
using Prana.Global;
using Prana.LogManager;
using Prana.ServerCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.PreTrade.CacheStore
{
    /// <summary>
    /// 
    /// </summary>
    internal class InTradeCache
    {
        /// <summary>
        /// The in trade cache
        /// </summary>
        private static readonly InTradeCache _inTradeCache = new InTradeCache();
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        internal static InTradeCache Instance
        {
            get { return _inTradeCache; }
        }

        /// <summary>
        /// The locker object
        /// </summary>
        private readonly object _lockerObject = new object();
        /// <summary>
        /// The cl order identifier wise prana message
        /// </summary>
        private readonly Dictionary<string, PranaMessage> _clOrderIdWisePranaMessage = new Dictionary<string, PranaMessage>();
        /// <summary>
        /// The cl order identifier wise original cl order identifier
        /// </summary>
        private readonly Dictionary<string, string> _clOrderIdWiseOriginalClOrderId = new Dictionary<string, string>();

        /// <summary>
        /// Gets all updated messages.
        /// </summary>
        /// <returns></returns>
        internal List<PranaMessage> GetAllUpdatedMessages()
        {
            var result = new List<PranaMessage>();
            lock (_lockerObject)
            {
                result.AddRange(_clOrderIdWisePranaMessage.Values.Select(msg => new PranaMessage(msg.ToString())));
            }
            return result;
        }

        /// <summary>
        /// Updates the and get affected messages.
        /// </summary>
        /// <param name="pranaMessageList">The prana message list.</param>
        /// <param name="isStartUpData">if set to <c>true</c> [is start up data].</param>
        /// <returns></returns>
        internal List<PranaMessage> UpdateAndGetAffectedMessages(List<PranaMessage> pranaMessageList, bool isStartUpData)
        {
            var returnPranaMessageList = new Dictionary<string, PranaMessage>();
            var multiDayChildOrderPranaMessageList = new List<PranaMessage>();
            try
            {
                lock (_lockerObject)
                {
                    if (isStartUpData)
                    {
                        _clOrderIdWiseOriginalClOrderId.Clear();
                        _clOrderIdWisePranaMessage.Clear();
                    }
                    foreach (PranaMessage pranaMessage in pranaMessageList)
                    {
                        var currentPranaMessage = new PranaMessage(pranaMessage.ToString());
                        int nirvanaMsgType = int.Parse(currentPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                        string orderStatus = currentPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrdStatus) ? currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value : string.Empty;

                        string clOrderId = currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                        string origClOrderId = string.Empty;
                        if (currentPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                        {
                            origClOrderId = currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                        }

                        string execType = string.Empty;
                        if (currentPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecType))
                        {
                            execType = currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecType].Value;
                            if ((orderStatus == FIXConstants.ORDSTATUS_PartiallyFilled || orderStatus == FIXConstants.ORDSTATUS_New)
                                && (execType == FIXConstants.EXECTYPE_PendingCancel || execType == FIXConstants.EXECTYPE_PendingReplace
                                || execType == FIXConstants.EXECTYPE_Replaced || execType == FIXConstants.EXECTYPE_Cancelled || execType == FIXConstants.EXECTYPE_DoneForDay))
                            {
                                orderStatus = execType;
                            }
                        }
                        switch (nirvanaMsgType)
                        {
                            case (int)OrderFields.PranaMsgTypes.ORDStaged:
                                ProcessAndUpdateStageOrder(orderStatus, clOrderId, currentPranaMessage, returnPranaMessageList, origClOrderId);
                                break;
                            case (int)OrderFields.PranaMsgTypes.ORDManualSub:
                            case (int)OrderFields.PranaMsgTypes.ORDManual:
                            case (int)OrderFields.PranaMsgTypes.ORDNewSub:
                                ProcessAndUpdateSubOrder(isStartUpData, orderStatus, execType, clOrderId, origClOrderId, currentPranaMessage, returnPranaMessageList);
                                break;
                            case (int)OrderFields.PranaMsgTypes.ORDNewSubChild:
                                multiDayChildOrderPranaMessageList.Add(currentPranaMessage);
                                break;
                        }
                    }
                }

                //Calculating cache only in the case of Trade Server Restart
                if (isStartUpData)
                    CalculateCacheForMultiDayOrders(multiDayChildOrderPranaMessageList, returnPranaMessageList);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return returnPranaMessageList.Values.ToList();
        }


        //TODO: Not handled yet for In-Stage 
        /// <summary>
        /// Calculate the In-Market Quantity for Multi-day orders on Server Restart 
        /// </summary>
        /// <param name="multiDayChildOrderPranaMessageList">Multi-Day child orders</param>
        /// <param name="returnPranaMessageDict">The list of main Sub orders</param>
        private void CalculateCacheForMultiDayOrders(List<PranaMessage> multiDayChildOrderPranaMessageList, Dictionary<string, PranaMessage> returnPranaMessageDict)
        {
            Dictionary<string, PranaMessage> updatedMessageList = new Dictionary<string, PranaMessage>();
            foreach (PranaMessage message in returnPranaMessageDict.Values)
            {
                if (OrderCacheManager.HasMultiDayHistory(message))
                {
                if (int.Parse(message.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDNewSub)
                        message.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = message.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;
            }
            }
            foreach (PranaMessage message in multiDayChildOrderPranaMessageList)
            {
                if (message.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                {
                    double cumQty = Double.Parse(message.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);

                    string stagedOrderId = message.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;

                    if (returnPranaMessageDict.ContainsKey(stagedOrderId))
                    {
                        double origCumQty = Double.Parse(returnPranaMessageDict[stagedOrderId].FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                        double newCumQty = origCumQty - cumQty;
                        returnPranaMessageDict[stagedOrderId].FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = newCumQty.ToString();
                        if(!updatedMessageList.ContainsKey(stagedOrderId))
                        {
                            updatedMessageList.Add(stagedOrderId, returnPranaMessageDict[stagedOrderId]);
                        }
                        
                    }
                }
            }
            foreach(var kvp in updatedMessageList)
            {
                if(_clOrderIdWisePranaMessage.ContainsKey(kvp.Key))
                {
                    _clOrderIdWisePranaMessage[kvp.Key] = kvp.Value;
                }
            }

        }

        /// <summary>
        /// Processes the and update sub order.
        /// </summary>
        /// <param name="isStartUpData">if set to <c>true</c> [is start up data].</param>
        /// <param name="orderStatus">The order status.</param>
        /// <param name="execType">Type of the execute.</param>
        /// <param name="clOrderId">The cl order identifier.</param>
        /// <param name="origClOrderId">The original cl order identifier.</param>
        /// <param name="currentPranaMessage">The current prana message.</param>
        /// <param name="result">The result.</param>
        private void ProcessAndUpdateSubOrder(bool isStartUpData, string orderStatus, string execType, string clOrderId, string origClOrderId, PranaMessage currentPranaMessage, Dictionary<string, PranaMessage> result)
        {
            try
            {
                const bool gettingFromDb = false;
                const double subtractStageQtyAfterRollover = 0;
                if (orderStatus == FIXConstants.ORDSTATUS_Replaced || execType == FIXConstants.ORDSTATUS_Replaced)
                {
                    ProcessAndUpdateReplacedSubOrder(clOrderId, origClOrderId, currentPranaMessage, result, false, subtractStageQtyAfterRollover);
                }
                else switch (orderStatus)
                    {
                        case FIXConstants.ORDSTATUS_New:
                            ProcessAndUpdateNewSubOrder(clOrderId, origClOrderId, currentPranaMessage, result, false, subtractStageQtyAfterRollover);
                            break;
                        case FIXConstants.ORDSTATUS_DoneForDay:
                        case FIXConstants.ORDSTATUS_RollOver:
                        case FIXConstants.ORDSTATUS_Cancelled:
                            ProcessAndUpdateCanceledDoneForDayAndReplacedSubOrder(isStartUpData, orderStatus, clOrderId, origClOrderId, currentPranaMessage, result);
                            break;
                        case FIXConstants.ORDSTATUS_Filled:
                        case FIXConstants.ORDSTATUS_PartiallyFilled:
                            ProcessAndUpdatePartiallyFilledAndFilledSubOrder(clOrderId, origClOrderId, currentPranaMessage, result, gettingFromDb, subtractStageQtyAfterRollover);
                            break;
                        case FIXConstants.ORDSTATUS_PendingReplace:
                        case FIXConstants.ORDSTATUS_PendingCancel:
                            ProcessAndUpdatePendingCancelAndPendingReplaceSubOrder(isStartUpData, clOrderId, origClOrderId, currentPranaMessage, result);
                            break;
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

        /// <summary>
        /// Processes the and update canceled done for day and replaced sub order.
        /// </summary>
        /// <param name="isStartUpData">if set to <c>true</c> [is start up data].</param>
        /// <param name="orderStatus">The order status.</param>
        /// <param name="clOrderId">The cl order identifier.</param>
        /// <param name="origClOrderId">The original cl order identifier.</param>
        /// <param name="currentPranaMessage">The current prana message.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private bool ProcessAndUpdateCanceledDoneForDayAndReplacedSubOrder(bool isStartUpData, string orderStatus, string clOrderId, string origClOrderId,
            PranaMessage currentPranaMessage, Dictionary<string, PranaMessage> result)
        {
            try
            {
                PranaMessage originalPranaMessage = null;
                if (_clOrderIdWisePranaMessage.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[clOrderId];
                }
                else if (_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[origClOrderId];

                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId) && !clOrderId.Equals(origClOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, origClOrderId);
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[clOrderId]];
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[origClOrderId]];

                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId) && !clOrderId.Equals(origClOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, _clOrderIdWiseOriginalClOrderId[origClOrderId]);
                }

                if (originalPranaMessage == null)
                {
                    var clOrderIdList = new List<string>();
                    originalPranaMessage = new PranaMessage(currentPranaMessage.ToString());

                    while (originalPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                    {
                        clOrderIdList.Add(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                        origClOrderId = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;

                        PranaMessage tempCurrentPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(origClOrderId);
                        if (int.Parse(tempCurrentPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDStaged)
                        {
                            break;
                        }
                        originalPranaMessage = tempCurrentPranaMessage;
                    }

                    origClOrderId = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;

                    if (!_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                    {
                        _clOrderIdWisePranaMessage.Add(origClOrderId, originalPranaMessage);
                    }
                    else
                    {
                        originalPranaMessage = _clOrderIdWisePranaMessage[origClOrderId];
                    }

                    foreach (string id in clOrderIdList)
                    {
                        _clOrderIdWiseOriginalClOrderId.Remove(id);
                        if (!id.Equals(origClOrderId))
                        {
                            _clOrderIdWiseOriginalClOrderId.Add(id, origClOrderId);
                        }
                    }
                }

                //No Need to update stage order if Trade is already rollovered
                if (originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_RollOver &&
                    (orderStatus == FIXConstants.ORDSTATUS_Cancelled || orderStatus == FIXConstants.ORDSTATUS_DoneForDay))
                {
                    originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = orderStatus;
                    return true;
                }

                if (currentPranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                {
                    string stagedOrderId = currentPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;

                    PranaMessage stagedPranaMessage = null;
                    if (_clOrderIdWisePranaMessage.ContainsKey(stagedOrderId))
                    {
                        stagedPranaMessage = _clOrderIdWisePranaMessage[stagedOrderId];
                    }
                    else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(stagedOrderId))
                    {
                        stagedPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[stagedOrderId]];
                    }

                    if (stagedPranaMessage == null)
                    {
                        var clOrderIdList = new List<string>();
                        stagedPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(stagedOrderId);

                        while (stagedPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                        {
                            clOrderIdList.Add(stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                            origClOrderId = stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                            stagedPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(origClOrderId);
                        }

                        stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;

                        if (!string.IsNullOrWhiteSpace(origClOrderId) && !_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                            _clOrderIdWisePranaMessage.Add(origClOrderId, stagedPranaMessage);

                        foreach (string id in clOrderIdList)
                        {
                            _clOrderIdWiseOriginalClOrderId.Remove(id);
                            if (!id.Equals(origClOrderId))
                            {
                                _clOrderIdWiseOriginalClOrderId.Add(id, origClOrderId);
                            }
                        }
                    }

                    double stageCumQty = Convert.ToDouble(stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                    double tradeCumQty = Convert.ToDouble(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                    double newStageCumQty;
                    if (isStartUpData && orderStatus == FIXConstants.ORDSTATUS_DoneForDay)
                    {
                        newStageCumQty = stageCumQty - tradeCumQty;
                    }
                    else
                    {
                        newStageCumQty = stageCumQty + tradeCumQty;
                    }
                    //if Stage qty is less than 0 it will be only in case if Sub is replaced with qty greater than stage order.
                    newStageCumQty = newStageCumQty < 0 ? 0 : newStageCumQty;
                    stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = newStageCumQty.ToString();

                    string orderIdToadd2 = stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                    result.Remove(orderIdToadd2);
                    result.Add(orderIdToadd2, new PranaMessage(stagedPranaMessage.ToString()));
                }

                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = "0";
                if (orderStatus == FIXConstants.ORDSTATUS_Cancelled)
                {
                    originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = FIXConstants.ORDSTATUS_Cancelled;
                }
                else if (orderStatus == FIXConstants.ORDSTATUS_DoneForDay)
                {
                    originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = FIXConstants.ORDSTATUS_DoneForDay;
                }
                else
                {
                    originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = FIXConstants.ORDSTATUS_RollOver;
                }

                string orderIdToadd = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                result.Remove(orderIdToadd);
                result.Add(orderIdToadd, new PranaMessage(originalPranaMessage.ToString()));
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

        /// <summary>
        /// Processes the and update pending cancel and pending replace sub order.
        /// </summary>
        /// <param name="isStartUpData">if set to <c>true</c> [is start up data].</param>
        /// <param name="clOrderId">The cl order identifier.</param>
        /// <param name="origClOrderId">The original cl order identifier.</param>
        /// <param name="currentPranaMessage">The current prana message.</param>
        /// <param name="result">The result.</param>
        private void ProcessAndUpdatePendingCancelAndPendingReplaceSubOrder(bool isStartUpData, string clOrderId, string origClOrderId, PranaMessage currentPranaMessage,
            Dictionary<string, PranaMessage> result)
        {
            try
            {
                if (!isStartUpData) return;
                PranaMessage originalPranaMessage = null;
                if (_clOrderIdWisePranaMessage.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[clOrderId];
                }
                else if (_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[origClOrderId];

                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId) && !clOrderId.Equals(origClOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, origClOrderId);
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[clOrderId]];
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[origClOrderId]];

                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId) && !clOrderId.Equals(origClOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, _clOrderIdWiseOriginalClOrderId[origClOrderId]);
                }

                if (originalPranaMessage == null)
                {
                    var clOrderIdList = new List<string>();
                    originalPranaMessage = new PranaMessage(currentPranaMessage.ToString());

                    while (originalPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                    {
                        clOrderIdList.Add(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                        origClOrderId = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;

                        PranaMessage tempCurrentPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(origClOrderId);
                        if (int.Parse(tempCurrentPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDStaged)
                        {
                            break;
                        }
                        originalPranaMessage = tempCurrentPranaMessage;
                    }

                    if (string.IsNullOrWhiteSpace(origClOrderId))
                    {
                        origClOrderId = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                        clOrderIdList.Add(origClOrderId);
                    }

                    if (!string.IsNullOrWhiteSpace(origClOrderId) && !_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                        _clOrderIdWisePranaMessage.Add(origClOrderId, originalPranaMessage);

                    foreach (string id in clOrderIdList)
                    {
                        _clOrderIdWiseOriginalClOrderId.Remove(id);
                        if (!id.Equals(origClOrderId))
                        {
                            _clOrderIdWiseOriginalClOrderId.Add(id, origClOrderId);
                        }
                    }
                }

                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;

                string orderIdToadd = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                result.Remove(orderIdToadd);
                result.Add(orderIdToadd, new PranaMessage(originalPranaMessage.ToString()));
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
        /// Processes the and update partially filled and filled sub order.
        /// </summary>
        /// <param name="clOrderId">The cl order identifier.</param>
        /// <param name="origClOrderId">The original cl order identifier.</param>
        /// <param name="currentPranaMessage">The current prana message.</param>
        /// <param name="result">The result.</param>
        /// <param name="gettingFromDb">if set to <c>true</c> [getting from database].</param>
        /// <param name="subtractStageQtyAfterRollover">The subtract stage qty after rollover.</param>
        private void ProcessAndUpdatePartiallyFilledAndFilledSubOrder(string clOrderId, string origClOrderId, PranaMessage currentPranaMessage, Dictionary<string, PranaMessage> result,
            bool gettingFromDb, double subtractStageQtyAfterRollover)
        {
            try
            {
                PranaMessage originalPranaMessage = null;
                if (_clOrderIdWisePranaMessage.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[clOrderId];
                }
                else if (_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[origClOrderId];

                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId) && !clOrderId.Equals(origClOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, origClOrderId);
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[clOrderId]];
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[origClOrderId]];

                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId) && !clOrderId.Equals(origClOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, _clOrderIdWiseOriginalClOrderId[origClOrderId]);
                }

                if (originalPranaMessage == null)
                {
                    gettingFromDb = true;
                    var clOrderIdList = new List<string>();
                    originalPranaMessage = new PranaMessage(currentPranaMessage.ToString());

                    while (originalPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                    {
                        clOrderIdList.Add(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                        origClOrderId = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;

                        PranaMessage tempCurrentPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(origClOrderId);
                        if (int.Parse(tempCurrentPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDStaged)
                        {
                            break;
                        }
                        originalPranaMessage = tempCurrentPranaMessage;
                    }

                    if (string.IsNullOrWhiteSpace(origClOrderId))
                    {
                        origClOrderId = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                        clOrderIdList.Add(origClOrderId);
                    }
                    originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;

                    if (!string.IsNullOrWhiteSpace(origClOrderId) && !_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                        _clOrderIdWisePranaMessage.Add(origClOrderId, originalPranaMessage);

                    foreach (string id in clOrderIdList)
                    {
                        _clOrderIdWiseOriginalClOrderId.Remove(id);
                        if (!id.Equals(origClOrderId))
                        {
                            _clOrderIdWiseOriginalClOrderId.Add(id, origClOrderId);
                        }
                    }
                }

                double originalQty = Convert.ToDouble(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                double newCumQuantity = Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                double newQty = Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                double newCumQty = newQty - newCumQuantity;
                double cumQtyDifference = gettingFromDb ? newQty : newQty - originalQty;
                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = newCumQty.ToString();

                if (originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_RollOver)
                {
                    if (currentPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagLastShares))
                    //Partially Filled then Rollover then Filled or Partially Filled
                    {
                        subtractStageQtyAfterRollover = Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagLastShares].Value) + newCumQty;
                    }
                    else //Partially Filled then Pending Cancel/Replace then Rollover then Reject
                    {
                        subtractStageQtyAfterRollover = Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value) -
                            Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                    }
                }

                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = newQty.ToString();
                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = newCumQty.ToString();
                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;

                string orderIdToadd = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                result.Remove(orderIdToadd);
                result.Add(orderIdToadd, new PranaMessage(originalPranaMessage.ToString()));

                if (currentPranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                {
                    string stagedOrderId = currentPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;

                    PranaMessage stagedPranaMessage = null;
                    if (_clOrderIdWisePranaMessage.ContainsKey(stagedOrderId))
                    {
                        stagedPranaMessage = _clOrderIdWisePranaMessage[stagedOrderId];
                    }
                    else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(stagedOrderId))
                    {
                        stagedPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[stagedOrderId]];
                    }

                    if (stagedPranaMessage == null)
                    {
                        var clOrderIdList = new List<string>();
                        stagedPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(stagedOrderId);

                        while (stagedPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                        {
                            clOrderIdList.Add(stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                            origClOrderId = stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                            stagedPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(origClOrderId);
                        }

                        stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;

                        if (!string.IsNullOrWhiteSpace(origClOrderId) && !_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                            _clOrderIdWisePranaMessage.Add(origClOrderId, stagedPranaMessage);

                        foreach (string id in clOrderIdList)
                        {
                            _clOrderIdWiseOriginalClOrderId.Remove(id);
                            if (!id.Equals(origClOrderId))
                            {
                                _clOrderIdWiseOriginalClOrderId.Add(id, origClOrderId);
                            }
                        }
                    }

                    double stageCumQty = Convert.ToDouble(stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                    double newStageCumQty = stageCumQty - cumQtyDifference - subtractStageQtyAfterRollover;

                    //if Stage qty is less than 0 it will be only in case if Sub is replaced with qty greater than stage order.
                    //newStageCumQty = newStageCumQty < 0 ? 0 : newStageCumQty;
                    stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = newStageCumQty.ToString();

                    string orderIdToadd2 = stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                    result.Remove(orderIdToadd2);
                    result.Add(orderIdToadd2, new PranaMessage(stagedPranaMessage.ToString()));
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
        /// Processes the and update new sub order.
        /// </summary>
        /// <param name="clOrderId">The cl order identifier.</param>
        /// <param name="origClOrderId">The original cl order identifier.</param>
        /// <param name="currentPranaMessage">The current prana message.</param>
        /// <param name="result">The result.</param>
        /// <param name="gettingFromDb">if set to <c>true</c> [getting from database].</param>
        /// <param name="subtractStageQtyAfterRollover">The subtract stage qty after rollover.</param>
        private void ProcessAndUpdateNewSubOrder(string clOrderId, string origClOrderId, PranaMessage currentPranaMessage, Dictionary<string, PranaMessage> result, bool gettingFromDb, double subtractStageQtyAfterRollover)
        {
            try
            {
                PranaMessage originalPranaMessage = null;
                Dictionary<String, PranaMessage> dictPendingApprovalTradeCache = PendingApprovalTradeCache.GetInstance().GetPendingApprovalOrderCacheOrderIdWise();
                if (_clOrderIdWisePranaMessage.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[clOrderId];
                }
                else if (_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[origClOrderId];
                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId) && !clOrderId.Equals(origClOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, origClOrderId);
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[clOrderId]];
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[origClOrderId]];

                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId) && !clOrderId.Equals(origClOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, _clOrderIdWiseOriginalClOrderId[origClOrderId]);
                }
                else if (dictPendingApprovalTradeCache.ContainsKey(clOrderId))
                {
                    originalPranaMessage = dictPendingApprovalTradeCache[clOrderId];
                }
                else if (dictPendingApprovalTradeCache.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = dictPendingApprovalTradeCache[origClOrderId];
                }

                if (originalPranaMessage == null)
                {
                    gettingFromDb = true;
                    originalPranaMessage = new PranaMessage(currentPranaMessage.ToString());

                    while (originalPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                    {
                        origClOrderId = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;

                        PranaMessage tempCurrentPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(origClOrderId);
                        if (int.Parse(tempCurrentPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDStaged)
                        {
                            break;
                        }
                        originalPranaMessage = tempCurrentPranaMessage;
                    }

                    if (string.IsNullOrWhiteSpace(origClOrderId))
                    {
                        origClOrderId = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                    }

                    if (!string.IsNullOrWhiteSpace(origClOrderId) && !_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                        _clOrderIdWisePranaMessage.Add(origClOrderId, originalPranaMessage);
                }

                double originalQty = Convert.ToDouble(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                double newCumQuantity = 0;
                if (currentPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty))
                {
                    newCumQuantity = Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                }
                double newCumQtyTemp = originalQty - newCumQuantity;

                if (originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_RollOver)
                {
                    if (currentPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagLastShares))
                    {//Pending New then Rollover then Acknowledge
                        subtractStageQtyAfterRollover = Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagLastShares].Value) + newCumQtyTemp;
                    }
                    else //New then Pending Cancel/Replace then Rollover then Reject
                    {
                        subtractStageQtyAfterRollover = Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value) - Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                    }
                }
                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;
                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;
                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;

                string orderIdToadd = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                result.Remove(orderIdToadd);
                result.Add(orderIdToadd, new PranaMessage(originalPranaMessage.ToString()));

                if (currentPranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                {
                    string stagedOrderId = currentPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;

                    PranaMessage stagedPranaMessage = null;
                    if (_clOrderIdWisePranaMessage.ContainsKey(stagedOrderId))
                    {
                        stagedPranaMessage = _clOrderIdWisePranaMessage[stagedOrderId];
                    }
                    else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(stagedOrderId))
                    {
                        stagedPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[stagedOrderId]];
                    }
                    if (stagedPranaMessage == null)
                    {
                        var clOrderIdList = new List<string>();
                        stagedPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(stagedOrderId);

                            while (stagedPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                            {
                                clOrderIdList.Add(stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                                origClOrderId = stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                                stagedPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(origClOrderId);
                            }

                            stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;

                            if (!string.IsNullOrWhiteSpace(origClOrderId) && !_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                                _clOrderIdWisePranaMessage.Add(origClOrderId, stagedPranaMessage);

                            foreach (string id in clOrderIdList)
                            {
                                _clOrderIdWiseOriginalClOrderId.Remove(id);
                                if (!id.Equals(origClOrderId))
                                {
                                    _clOrderIdWiseOriginalClOrderId.Add(id, origClOrderId);
                                }
                            }
                        }

                    double stageCumQty;
                    if (gettingFromDb)
                    {
                        double tradeQuantity = Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                        stageCumQty = Convert.ToDouble(stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                        double newCumQty = stageCumQty - tradeQuantity;
                        stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = newCumQty.ToString();
                    }

                    stageCumQty = Convert.ToDouble(stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                    stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = (stageCumQty - subtractStageQtyAfterRollover).ToString();

                    string orderIdToadd2 = stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                    result.Remove(orderIdToadd2);
                    result.Add(orderIdToadd2, new PranaMessage(stagedPranaMessage.ToString()));
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
        /// Processes the and update replaced sub order.
        /// </summary>
        /// <param name="clOrderId">The cl order identifier.</param>
        /// <param name="origClOrderId">The original cl order identifier.</param>
        /// <param name="currentPranaMessage">The current prana message.</param>
        /// <param name="result">The result.</param>
        /// <param name="gettingFromDb">if set to <c>true</c> [getting from database].</param>
        /// <param name="subtractStageQtyAfterRollover">The subtract stage qty after rollover.</param>
        private void ProcessAndUpdateReplacedSubOrder(string clOrderId, string origClOrderId, PranaMessage currentPranaMessage, Dictionary<string, PranaMessage> result, bool gettingFromDb, double subtractStageQtyAfterRollover)
        {
            try
            {
                PranaMessage originalPranaMessage = null;
                Dictionary<String, PranaMessage> dictPendingApprovalTradeCache = PendingApprovalTradeCache.GetInstance().GetPendingApprovalOrderCacheOrderIdWise();
                if (_clOrderIdWisePranaMessage.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[clOrderId];
                }
                else if (_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[origClOrderId];

                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId) && !clOrderId.Equals(origClOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, origClOrderId);
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[clOrderId]];
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[origClOrderId]];

                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId) && !clOrderId.Equals(origClOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, _clOrderIdWiseOriginalClOrderId[origClOrderId]);
                }
                else if (dictPendingApprovalTradeCache.ContainsKey(clOrderId))
                {
                    originalPranaMessage = dictPendingApprovalTradeCache[clOrderId];
                }
                else if (dictPendingApprovalTradeCache.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = dictPendingApprovalTradeCache[origClOrderId];
                }

                if (originalPranaMessage == null)
                {
                    gettingFromDb = true;
                    var clOrderIdList = new List<string>();
                    originalPranaMessage = new PranaMessage(currentPranaMessage.ToString());

                    while (originalPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                    {
                        clOrderIdList.Add(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                        origClOrderId = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;

                        PranaMessage tempCurrentPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(origClOrderId);
                        if (int.Parse(tempCurrentPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDStaged)
                        {
                            break;
                        }
                        originalPranaMessage = tempCurrentPranaMessage;
                    }

                    origClOrderId = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;

                    if (!_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                    {
                        _clOrderIdWisePranaMessage.Add(origClOrderId, originalPranaMessage);
                    }
                    else
                    {
                        originalPranaMessage = _clOrderIdWisePranaMessage[origClOrderId];
                        gettingFromDb = false;
                    }

                    foreach (string id in clOrderIdList)
                    {
                        _clOrderIdWiseOriginalClOrderId.Remove(id);
                        if (!id.Equals(origClOrderId))
                        {
                            _clOrderIdWiseOriginalClOrderId.Add(id, origClOrderId);
                        }
                    }
                }

                double originalQty = Convert.ToDouble(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                double newQuantity = Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                double originalCumQty = Convert.ToDouble(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                originalCumQty = gettingFromDb ? originalQty - originalCumQty : originalCumQty;
                double newCumQty = newQuantity - originalQty + originalCumQty;
                double cumQtyDifference = newCumQty - originalCumQty + (gettingFromDb ? originalQty : 0);

                if (originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_RollOver)
                {
                    subtractStageQtyAfterRollover = Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value) -
                        Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value) - cumQtyDifference;
                }

                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = (newCumQty + subtractStageQtyAfterRollover).ToString();
                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = newQuantity.ToString();
                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;

                string orderIdToadd = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                result.Remove(orderIdToadd);
                result.Add(orderIdToadd, new PranaMessage(originalPranaMessage.ToString()));

                if (currentPranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                {
                    string stagedOrderId = currentPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;

                    PranaMessage stagedPranaMessage = null;
                    if (_clOrderIdWisePranaMessage.ContainsKey(stagedOrderId))
                    {
                        stagedPranaMessage = _clOrderIdWisePranaMessage[stagedOrderId];
                    }
                    else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(stagedOrderId))
                    {
                        stagedPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[stagedOrderId]];
                    }

                    if (stagedPranaMessage == null)
                    {
                        var clOrderIdList = new List<string>();
                        stagedPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(stagedOrderId);

                        while (stagedPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                        {
                            clOrderIdList.Add(stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                            origClOrderId = stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                            stagedPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(origClOrderId);
                        }

                        stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;

                        if (!_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                            _clOrderIdWisePranaMessage.Add(origClOrderId, stagedPranaMessage);

                        foreach (string id in clOrderIdList)
                        {
                            _clOrderIdWiseOriginalClOrderId.Remove(id);
                            if (!id.Equals(origClOrderId))
                            {
                                _clOrderIdWiseOriginalClOrderId.Add(id, origClOrderId);
                            }
                        }
                    }

                    double stageCumQty = Convert.ToDouble(stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                    double newStageCumQty = stageCumQty - cumQtyDifference - subtractStageQtyAfterRollover;

                    //if Stage qty is less than 0 it will be only in case if Sub is replaced with qty greater than stage order.
                    //  newStageCumQty = newStageCumQty < 0 ? 0 : newStageCumQty;
                    stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = newStageCumQty.ToString();

                    string orderIdToadd2 = stagedPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                    result.Remove(orderIdToadd2);
                    result.Add(orderIdToadd2, new PranaMessage(stagedPranaMessage.ToString()));
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
        /// Processes the and update stage order.
        /// </summary>
        /// <param name="orderStatus">The order status.</param>
        /// <param name="clOrderId">The cl order identifier.</param>
        /// <param name="currentPranaMessage">The current prana message.</param>
        /// <param name="result">The result.</param>
        /// <param name="origClOrderId">The original cl order identifier.</param>
        private void ProcessAndUpdateStageOrder(string orderStatus, string clOrderId, PranaMessage currentPranaMessage, Dictionary<string, PranaMessage> result, string origClOrderId)
        {
            try
            {
                switch (orderStatus)
                {
                    case FIXConstants.ORDSTATUS_New:
                        {
                            if (!_clOrderIdWisePranaMessage.ContainsKey(clOrderId))
                            {
                                currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;
                                _clOrderIdWisePranaMessage.Add(clOrderId, currentPranaMessage);
                            }
                            string orderIdToadd = currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                            result[orderIdToadd] = new PranaMessage(_clOrderIdWisePranaMessage[orderIdToadd].ToString());
                        }
                        break;
                    case FIXConstants.ORDSTATUS_Replaced:
                        {
                            ProcessAndUpdateReplacedStageOrder(clOrderId, currentPranaMessage, result, origClOrderId);
                        }
                        break;
                    case FIXConstants.ORDSTATUS_Cancelled:
                        {
                            ProcessAndUpdateCanceledStageOrder(clOrderId, currentPranaMessage, result, origClOrderId);
                        }
                        break;
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

        /// <summary>
        /// Processes the and update canceled stage order.
        /// </summary>
        /// <param name="clOrderId">The cl order identifier.</param>
        /// <param name="currentPranaMessage">The current prana message.</param>
        /// <param name="result">The result.</param>
        /// <param name="origClOrderId">The original cl order identifier.</param>
        private void ProcessAndUpdateCanceledStageOrder(string clOrderId, PranaMessage currentPranaMessage, Dictionary<string, PranaMessage> result, string origClOrderId)
        {
            try
            {
                PranaMessage originalPranaMessage = null;
                if (_clOrderIdWisePranaMessage.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[clOrderId];
                }
                if (_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[origClOrderId];
                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId) && !clOrderId.Equals(origClOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, origClOrderId);
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[clOrderId]];
                }

                if (originalPranaMessage == null)
                {
                    var clOrderIdList = new List<string>();
                    originalPranaMessage = new PranaMessage(currentPranaMessage.ToString());

                    while (originalPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                    {
                        clOrderIdList.Add(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                        origClOrderId = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                        originalPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(origClOrderId);
                    }

                    originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;

                    if (!_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                        _clOrderIdWisePranaMessage.Add(origClOrderId, originalPranaMessage);

                    foreach (string id in clOrderIdList)
                    {
                        _clOrderIdWiseOriginalClOrderId.Remove(id);
                        if (!id.Equals(origClOrderId))
                        {
                            _clOrderIdWiseOriginalClOrderId.Add(id, origClOrderId);
                        }
                    }
                }

                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = "0";
                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = FIXConstants.ORDSTATUS_Cancelled;

                string orderIdToadd = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                result.Remove(orderIdToadd);
                result.Add(orderIdToadd, new PranaMessage(originalPranaMessage.ToString()));
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
        /// Processes the and update replaced stage order.
        /// </summary>
        /// <param name="clOrderId">The cl order identifier.</param>
        /// <param name="currentPranaMessage">The current prana message.</param>
        /// <param name="result">The result.</param>
        /// <param name="origClOrderId">The original cl order identifier.</param>
        private void ProcessAndUpdateReplacedStageOrder(string clOrderId, PranaMessage currentPranaMessage, Dictionary<string, PranaMessage> result, string origClOrderId)
        {
            try
            {
                PranaMessage originalPranaMessage = null;
                if (_clOrderIdWisePranaMessage.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[clOrderId];
                }
                else if (_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[origClOrderId];
                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, origClOrderId);
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[clOrderId]];
                }
                else if (_clOrderIdWiseOriginalClOrderId.ContainsKey(origClOrderId))
                {
                    originalPranaMessage = _clOrderIdWisePranaMessage[_clOrderIdWiseOriginalClOrderId[origClOrderId]];
                    if (!_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId))
                        _clOrderIdWiseOriginalClOrderId.Add(clOrderId, _clOrderIdWiseOriginalClOrderId[origClOrderId]);
                }

                if (originalPranaMessage == null)
                {
                    var clOrderIdList = new List<string>();
                    originalPranaMessage = new PranaMessage(currentPranaMessage.ToString());

                    while (originalPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                    {
                        clOrderIdList.Add(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                        origClOrderId = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                        originalPranaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(origClOrderId);
                    }

                    originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;

                    if (!_clOrderIdWisePranaMessage.ContainsKey(origClOrderId))
                    {
                        _clOrderIdWisePranaMessage.Add(origClOrderId, originalPranaMessage);
                    }
                    else
                    {
                        originalPranaMessage = _clOrderIdWisePranaMessage[origClOrderId];
                    }

                    foreach (string id in clOrderIdList)
                    {
                        _clOrderIdWiseOriginalClOrderId.Remove(id);
                        if (!id.Equals(origClOrderId))
                        {
                            _clOrderIdWiseOriginalClOrderId.Add(id, origClOrderId);
                        }
                    }
                }
                double originalQty = Convert.ToDouble(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                double newQuantity = Convert.ToDouble(currentPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                double originalCumQty = Convert.ToDouble(originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                double newCumQty = newQuantity - originalQty + originalCumQty;
                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = newCumQty.ToString();
                originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = newQuantity.ToString();
                string orderIdToadd = originalPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                result.Remove(orderIdToadd);
                result.Add(orderIdToadd, new PranaMessage(originalPranaMessage.ToString()));
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
        /// Gets the prana message.
        /// </summary>
        /// <param name="clOrderId">The cl order identifier.</param>
        /// <returns></returns>
        internal PranaMessage GetPranaMessage(string clOrderId)
        {
            PranaMessage result = null;
            try
            {
                lock (_lockerObject)
                {
                    if (_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId))
                        clOrderId = _clOrderIdWiseOriginalClOrderId[clOrderId];

                    if (_clOrderIdWisePranaMessage.ContainsKey(clOrderId))
                        result = new PranaMessage(_clOrderIdWisePranaMessage[clOrderId].ToString());
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
            return result;
        }

        /// <summary>
        /// Removes the prana message from cache.
        /// </summary>
        /// <param name="clOrderId">The cl order identifier.</param>
        /// <returns></returns>
        internal bool RemovePranaMessageFromCache(string clOrderId)
        {
            bool result = false;
            try
            {
                lock (_lockerObject)
                {
                    if (_clOrderIdWiseOriginalClOrderId.ContainsKey(clOrderId))
                    {
                        string origClOrderId = _clOrderIdWiseOriginalClOrderId[clOrderId];
                        _clOrderIdWiseOriginalClOrderId.Remove(clOrderId);
                        clOrderId = origClOrderId;
                    }

                    if (_clOrderIdWisePranaMessage.ContainsKey(clOrderId))
                    {
                        _clOrderIdWisePranaMessage.Remove(clOrderId);
                        result = true;
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
            return result;
        }

        /// <summary>
        /// Updates the prana message in cache.
        /// </summary>
        /// <returns></returns>
        internal void UpdateMessageInCache(string orderID, double orderQty, double remainingQty, bool isStage)
        {
            PranaMessage panaMessage = null;
            try
            {
                panaMessage = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(orderID);

                if (panaMessage != null)
                {
                    if (isStage)
                    {
                        panaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = Convert.ToString(orderQty + remainingQty);
                    }
                    else
                    {
                        panaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = Convert.ToString(orderQty - remainingQty);
                        panaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = "0";
                    }

                    _clOrderIdWisePranaMessage.Remove(orderID);
                    _clOrderIdWisePranaMessage.Add(orderID, panaMessage);
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
    }
}