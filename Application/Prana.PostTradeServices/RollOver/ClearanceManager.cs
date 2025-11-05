using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.LogManager;
using Prana.OrderProcessor;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Prana.PostTradeServices.RollOver
{
    public class ClearanceManager
    {
        private IScheduler _sched;
        private ISchedulerFactory _schedFact = new StdSchedulerFactory();
        private bool _isOnlyAUECTimeBasedBlotterClearance = Convert.ToBoolean(ConfigurationManager.AppSettings["IsOnlyAUECTimeBasedBlotterClearance"]);
        private bool _setIsHiddenTrueInSubTable = Convert.ToBoolean(ConfigurationManager.AppSettings["SetIsHiddenTrueInSubTable"]);
        private bool _enableTradeFlowLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableTradeFlowLogging"));
        private HashSet<string> _cLOrderIDToSetIsHiddenInSubTable;
        private Dictionary<string, OrderSingle> dictParentClOrderIDCollection = new Dictionary<string, OrderSingle>();
        private readonly object _lockOnWorkingSubsTabCollection = new object();
        private static readonly object _locker = new object();
        private OrderBindingList workingSubsTabCollection = new OrderBindingList();
        private OrderBindingList newWorkingSubsList = new OrderBindingList();
        static ProxyBase<IPublishing> _proxyPublishing = null;
        private static ClearanceManager _clearanceManagerSingleton;

        private ClearanceManager()
        {
        }

        public static ClearanceManager GetInstance
        {
            get
            {
                if (_clearanceManagerSingleton == null)
                {
                    _clearanceManagerSingleton = new ClearanceManager();
                }
                return _clearanceManagerSingleton;
            }
        }

        /// <summary>
        /// BlotterClearanceCommonData
        /// </summary>
        public BlotterClearanceCommonData BlotterClearanceCommonData { get; set; } = null;

        public object _lockOnParentClOrderIDCollection = new object();
        public Dictionary<string, OrderSingle> DictParentClOrderIDCollection
        {
            get
            {
                lock (_lockOnParentClOrderIDCollection)
                {
                    return dictParentClOrderIDCollection;
                }
            }
        }
        /// <summary>
        /// CreatePublishingProxy
        /// </summary>
        private static void CreatePublishingProxy()
        {
            try
            {
                lock (_locker)
                {
                    if (_proxyPublishing == null)
                    {
                        _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy", ex), LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Auto RollOver
        /// <summary>
        /// StartScheduler
        /// </summary>
        public void StartScheduler()
        {
            _sched = _schedFact.GetScheduler();
            _sched.Start();
        }

        /// <summary>
        /// Schedule the rollover and clearance job
        /// </summary>
        /// <param name="_companyID"></param>
        public void AddClearanceSchedulerTasks(int _companyID)
        {
            try
            {
                StartScheduler();
                //clearing collection before adding the AUEC data in case of logout and login from toolbar, PRANA-33163
                ClearanceCommonCache.GetInstance().DictRolloverPermittedAUEC.Clear();
                BlotterClearanceCommonData = ClearanceDataBaseManager.GetInstance.GetCompanyClearanceCommonData(_companyID);
                ClearanceCommonCache.GetInstance().ClearanceDataFull = ClearanceDataBaseManager.GetInstance.GetClearanceData(_companyID, ref ClearanceCommonCache.GetInstance().DictRolloverPermittedAUEC);

                foreach (ClearanceData clearanceData in ClearanceCommonCache.GetInstance().ClearanceDataFull)
                {
                    JobDetail jobdetail = new JobDetail("ClearanceJobForBlotter :" + clearanceData.AUECIDStr, "AUEC-Clearance", typeof(BlotterClearanceJob));

                    jobdetail.JobDataMap["AUECIDStr"] = clearanceData.AUECIDStr;
                    jobdetail.JobDataMap["ClearanceTime"] = clearanceData.ClearanceTime;
                    System.TimeSpan tt = new TimeSpan(864000000000); // for whole 24day set it to 864000000000;  10000000
                    SimpleTrigger st = new SimpleTrigger();
                    st.RepeatCount = -1;
                    st.RepeatInterval = tt;
                    st.StartTimeUtc = clearanceData.ClearanceTime;
                    st.Name = "BlotterTriggerAuec " + clearanceData.AUECIDStr;
                    st.Group = "AUEC-Blotter-Clearance-Triggers";
                    _sched.DeleteJob("ClearanceJobForBlotter :" + clearanceData.AUECIDStr, "AUEC-Clearance");
                    _sched.ScheduleJob(jobdetail, st);
                }
                SetupAUECWiseClearanceTime();
                if(_setIsHiddenTrueInSubTable)
                    SetupBgWorkerForHiddingTrade();
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
        /// This background worker is used to set the IsHidden field in T_Sub table to true for the trades which are filled
        /// </summary>
        private void SetupBgWorkerForHiddingTrade()
        {
            BackgroundWorker bgWorkerSetIsHiddenTrueInSub = new BackgroundWorker();
            bgWorkerSetIsHiddenTrueInSub.DoWork += new DoWorkEventHandler(bgWorkerSetIsHiddenTrueInSub_DoWork);
            bgWorkerSetIsHiddenTrueInSub.RunWorkerAsync();
        }

        /// <summary>
        /// BackGround Worker's Dowork Method to get all data from DB and set IsHidden field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorkerSetIsHiddenTrueInSub_DoWork(object sender, DoWorkEventArgs e)
        {
            GetBlotterLaunchData(null, true);
        }

        /// <summary>
        /// Setup AUEC Wise Clearance Time
        /// </summary>
        private void SetupAUECWiseClearanceTime()
        {
            try
            {
                //clearing collection before adding the AUEC data in case of logout and login from toolbar, PRANA-33163
                ClearanceCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.Clear();
                foreach (ClearanceData clearanceData in ClearanceCommonCache.GetInstance().ClearanceDataFull)
                {
                    foreach (string auecid in clearanceData.AUECIDStr.Split(','))
                    {
                        if (!string.IsNullOrEmpty(auecid))
                        {
                            if (!ClearanceCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.ContainsKey(int.Parse(auecid.Trim())))
                            {
                                ClearanceCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.Add(int.Parse(auecid.Trim()), clearanceData.ClearanceTime);
                            }
                            else
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Blotter multiple clearance setup for AUECID : " + clearanceData.AUECID + " in Admin.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                            }
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

        /// <summary>
        /// Clear and rollover trades By AUECID for Day and GTD
         /// </summary>
        /// <param name="context"></param>
        public void ClearTradesByAUECID(JobExecutionContext context)
        {
            List<int> listImpactedAUECID = new List<int>();
            List<string> clearedTradesInfo = new List<string>();
            try
            {
                string commaSeparatedAUECIDString = context.JobDetail.JobDataMap["AUECIDStr"].ToString();
                DateTime clearanceTime = DateTime.Parse(context.JobDetail.JobDataMap["ClearanceTime"].ToString());
                List<string> listImpactedAUECIDStr = GeneralUtilities.GetListFromString(commaSeparatedAUECIDString, ',');
                foreach (string auecID in listImpactedAUECIDStr)
                {
                    listImpactedAUECID.Add(int.Parse(auecID));
                }
                // Get Blotter data 
                dictParentClOrderIDCollection = GetBlotterLaunchData(listImpactedAUECID);
                if (BlotterClearanceCommonData.AutoClearing)
                {
                    List<string> autoClearOrdersRemoved = ClearTradesByAUECID(listImpactedAUECID, clearanceTime, false);
                    foreach (string ParentClOrderID in autoClearOrdersRemoved)
                    {
                        if (!clearedTradesInfo.Contains(ParentClOrderID))
                            clearedTradesInfo.Add(ParentClOrderID);
                    }
                }
                //Clearing GTD expired Trades
                List<string> gtdExpireOrdersRemoved = ClearTradesByAUECID(listImpactedAUECID, clearanceTime, true);
                foreach (string ParentClOrderID in gtdExpireOrdersRemoved)
                {
                    if (!clearedTradesInfo.Contains(ParentClOrderID))
                        clearedTradesInfo.Add(ParentClOrderID);
                }


                #region Removed data information published
                if(_proxyPublishing == null)
                {
                    CreatePublishingProxy();
                }
                StageOrderRemovalData stageOrderRemovalData = new StageOrderRemovalData();
                stageOrderRemovalData.ParentClOrderIds = String.Join(",", clearedTradesInfo);
                stageOrderRemovalData.IsComingFromRollOver = true;
                List<StageOrderRemovalData> lstStageOrderRemovalData = new List<StageOrderRemovalData>();
                lstStageOrderRemovalData.Add(stageOrderRemovalData);

                MessageData messageData = new MessageData();
                messageData.EventData = lstStageOrderRemovalData;
                messageData.TopicName = Topics.Topic_StageOrderRemovalFromBlotter;
                _proxyPublishing.InnerChannel.Publish(messageData, Topics.Topic_StageOrderRemovalFromBlotter);
                #endregion
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
        /// Clear and rollover trades By AUECID
        /// </summary>
        /// <param name="listImpactedAUECID"></param>
        /// <param name="clearanceTime"></param>
        /// <param name="isMultiDayClearanceCall"></param>
        /// <returns></returns>
        public List<string> ClearTradesByAUECID(List<int> listImpactedAUECID, DateTime clearanceTime, bool isMultiDayClearanceCall)
        {
            List<string> toBeRemovedKeys = new List<string>();
            List<string> clearedTradesInformation = new List<string>();

            try
            {
                lock (_lockOnParentClOrderIDCollection)
                {
                    foreach (OrderSingle order in dictParentClOrderIDCollection.Values)
                    {
                        if (!order.PranaMsgType.Equals((int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSubChild))
                        {
                            if ((!isMultiDayClearanceCall && !OrderInformation.IsMultiDayOrder(order))
                            || (isMultiDayClearanceCall && OrderInformation.IsMultiDayOrder(order)))
                            {
                                DateTime currentAUECDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));
                                DateTime localClearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(clearanceTime, CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));
                                if (localClearanceTime.TimeOfDay < currentAUECDate.TimeOfDay)
                                {
                                    localClearanceTime = currentAUECDate.Date + localClearanceTime.TimeOfDay;
                                }
                                else if (_isOnlyAUECTimeBasedBlotterClearance)
                                {
                                    localClearanceTime = currentAUECDate.Date.AddDays(-1) + localClearanceTime.TimeOfDay;
                                }
                                else
                                {
                                    localClearanceTime = currentAUECDate.Date;
                                }

                                if (listImpactedAUECID.Contains(order.AUECID))
                                {
                                    //To do:- need to handle GTD\GTC end state conditions
                                    if (!OrderInformation.IsMultiDayOrder(order) && order.AUECLocalDate < localClearanceTime)
                                    {
                                        toBeRemovedKeys.Add(order.ParentClOrderID.ToString());
                                        order.DayAvgPx = 0;
                                        order.DayCumQty = 0;
                                        order.DayOrderQty = order.UnexecutedQuantity;
                                    }
                                    else if (order.TIF.Equals(FIXConstants.TIF_GTD))
                                    {
                                        string expireTimeTemp = order.ExpireTime;
                                        if (!string.IsNullOrEmpty(expireTimeTemp) && (!expireTimeTemp.Equals("N/A")))
                                        {
                                            DateTime expireTime = Convert.ToDateTime(expireTimeTemp);
                                            expireTime = BusinessDayCalculator.GetInstance().GetPreviousBusinessDay(expireTime.AddDays(1), order.AUECID);
                                            if (expireTime <= localClearanceTime)
                                            {
                                                toBeRemovedKeys.Add(order.ParentClOrderID.ToString());
                                                order.DayAvgPx = 0;
                                                order.DayCumQty = 0;
                                                order.DayOrderQty = order.UnexecutedQuantity;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (string key in toBeRemovedKeys)
                    {
                        if (DictParentClOrderIDCollection.ContainsKey(key))
                        {
                            OrderSingle orderToBeRemoved = DictParentClOrderIDCollection[key];
                            if (orderToBeRemoved.PranaMsgType == (int)(Prana.Global.OrderFields.PranaMsgTypes.ORDStaged))
                            {
                                if (orderToBeRemoved.OrderStatusTagValue != FIXConstants.ORDSTATUS_Filled && orderToBeRemoved.OrderStatusTagValue != FIXConstants.ORDSTATUS_Cancelled)
                                {
                                    continue;
                                }
                            }

                            #region Rollover
                            // Checks if order can be rolled over.
                            OrderSingle existingOrder = dictParentClOrderIDCollection[orderToBeRemoved.ParentClOrderID];

                            if (ISOrderRolloverable(existingOrder) && ClearanceCommonCache.GetInstance().DictRolloverPermittedAUEC.ContainsKey(orderToBeRemoved.AUECID) && ClearanceCommonCache.GetInstance().DictRolloverPermittedAUEC[orderToBeRemoved.AUECID])
                            {
                                OrderSingle rolledOverOrder = (OrderSingle)orderToBeRemoved.Clone();
                                rolledOverOrder.MsgType = FIXConstants.MSGOrderRollOverRequest;
                                rolledOverOrder.OrigClOrderID = rolledOverOrder.ClOrderID;
                                rolledOverOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingRollOver;
                                rolledOverOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(rolledOverOrder.OrderStatusTagValue.ToString());
                                rolledOverOrder.TransactionTime = DateTime.Now.ToUniversalTime();
                                if (OrderInformation.IsMultiDayOrderHistory(rolledOverOrder))
                                    rolledOverOrder.CumQty = 0;

                                PranaMessage rolledOverOrderMsg = Transformer.CreatePranaMessageThroughReflection(rolledOverOrder);
                                PranaOrderProcessor.GetInstance.ProcessMessage(rolledOverOrderMsg);
                            }
                            #endregion

                            #region Removed data information
                            if (!clearedTradesInformation.Contains(orderToBeRemoved.ParentClOrderID))
                                clearedTradesInformation.Add(orderToBeRemoved.ParentClOrderID);
                            #endregion
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
            return clearedTradesInformation;
        }

        /// <summary>
        /// GetBlotterLaunchDataForClearance
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, OrderSingle> GetBlotterLaunchData(List<int> listImpactedAUECID, bool SetisHiddenTrue = false)
        {
            try
            {
                List<OrderSingle>  incomingOrders = DataBaseManager.GetBlotterLaunchData(true, listImpactedAUECID);
                if (incomingOrders.Count > 0)
                {
                    lock (_lockOnWorkingSubsTabCollection)
                        workingSubsTabCollection.Clear();

                    lock (_lockOnParentClOrderIDCollection)
                    {
                        dictParentClOrderIDCollection.Clear();
                        foreach (OrderSingle incomingOrder in incomingOrders)
                        {
                            if (dictParentClOrderIDCollection.ContainsKey(incomingOrder.ParentClOrderID))
                            {
                                OrderSingle existingOrder = dictParentClOrderIDCollection[incomingOrder.ParentClOrderID];

                                if (existingOrder.OrderSeqNumber > incomingOrder.OrderSeqNumber && incomingOrder.OrderSeqNumber != long.MinValue || ((OrderInformation.IsMultiDayOrderHistory(existingOrder) || OrderInformation.IsMultiDayOrderHistory(incomingOrder)) && incomingOrder.MsgType == FIXConstants.MSGOrderCancelReject))
                                {
                                    continue;
                                }
                                else
                                {
                                    UpdateExistingOrder(existingOrder, incomingOrder);

                                    //In case of Parent Sub orders of GTC/GTD, re-calcualte Quantity from the child orders for Replace/Cancel scenarios
                                    if (incomingOrder.PranaMsgType != (int)OrderFields.PranaMsgTypes.ORDStaged && OrderInformation.IsMultiDayOrderHistory(existingOrder))
                                    {
                                        UpdateStatusFromChildCollection(existingOrder);
                                    }
                                    //Special handling for staged orders...
                                    if (incomingOrder.StagedOrderID != string.Empty && incomingOrder.StagedOrderID != int.MinValue.ToString() && dictParentClOrderIDCollection.ContainsKey(incomingOrder.StagedOrderID))
                                    {

                                        if (dictParentClOrderIDCollection.ContainsKey(incomingOrder.StagedOrderID))
                                        {
                                            OrderSingle parentOrder = dictParentClOrderIDCollection[incomingOrder.StagedOrderID];
                                            double parentOrderPrevQty = parentOrder.CumQty;
                                            UpdateStatusFromChildCollection(parentOrder);
                                            UpdateParentStatus(incomingOrder, parentOrder);

                                            if (parentOrder.StagedOrderID != string.Empty && parentOrder.StagedOrderID != int.MinValue.ToString() && dictParentClOrderIDCollection.ContainsKey(parentOrder.StagedOrderID))
                                            {
                                                OrderSingle grandParentOrder = dictParentClOrderIDCollection[parentOrder.StagedOrderID];
                                                grandParentOrder.LastPrice = incomingOrder.LastPrice;
                                                grandParentOrder.LastShares = incomingOrder.LastShares;
                                                UpdateParentStatusFromSub(parentOrder, grandParentOrder, parentOrderPrevQty, incomingOrder.AvgPrice);
                                            }
                                            parentOrder.LastPrice = incomingOrder.LastPrice;
                                            parentOrder.LastShares = incomingOrder.LastShares;
                                        }
                                    }
                                    else if (incomingOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged)
                                    {
                                        if (dictParentClOrderIDCollection.ContainsKey(incomingOrder.ParentClOrderID))
                                        {
                                            OrderSingle parentOrder = dictParentClOrderIDCollection[incomingOrder.ParentClOrderID];
                                            UpdateStatusFromChildCollection(parentOrder);
                                            UpdateParentStatus(incomingOrder, parentOrder);
                                            parentOrder.LastPrice = incomingOrder.LastPrice;
                                            parentOrder.FXRate = incomingOrder.FXRate;
                                            parentOrder.LastShares = incomingOrder.LastShares;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // new order hence add in the dictionary
                                if (!OrderInformation.IsMultiDayOrderHistory(incomingOrder) && incomingOrder.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged)
                                {
                                    incomingOrder.DayCumQty = incomingOrder.CumQty;
                                    incomingOrder.DayAvgPx = incomingOrder.AvgPrice;
                                    incomingOrder.DayOrderQty = incomingOrder.Quantity;
                                }
                                dictParentClOrderIDCollection.Add(incomingOrder.ParentClOrderID, incomingOrder);
                                AddToOrderCollection(incomingOrder);
                            }
                        }
                    }
                    if (newWorkingSubsList.Count > 0)
                    {
                        workingSubsTabCollection.AddRange(newWorkingSubsList);
                        newWorkingSubsList.Clear();
                    }
                }
                if (SetisHiddenTrue)
                {
                    _cLOrderIDToSetIsHiddenInSubTable = new HashSet<string>();
                    RemoveMultiDayEndStateOrders();
                    RemoveFilledAndDropCopyStagedOrders(incomingOrders);
                    RemoveMultiBrokerSubFromAllOrderCollection();
                    if (_setIsHiddenTrueInSubTable && _cLOrderIDToSetIsHiddenInSubTable.Count > 0)
                    {
                        DataBaseManager.SetIsHiddenTrueInSub(string.Join(",", _cLOrderIDToSetIsHiddenInSubTable));
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
            return dictParentClOrderIDCollection;
        }

        /// <summary>
        /// ISOrderRolloverable
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static bool ISOrderRolloverable(OrderSingle order)
        {
            if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_New ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingReplace ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Replaced ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingCancel ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PartiallyFilled ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Suspended ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Expired)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// AddToOrderCollection
        /// </summary>
        /// <param name="incomingOrder"></param>
        private void AddToOrderCollection(OrderSingle incomingOrder)
        {
            try
            {
                bool isSkipOrderFromWorkingSubCollection = false;
                DateTime currentAUECDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                bool isClearanceCalculated = false;
                DateTime clearanceTime = new DateTime();
                if (incomingOrder.MsgType != FIXConstants.MSGOrderRollOverRequest && ClearanceCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.TryGetValue(incomingOrder.AUECID, out clearanceTime))
                {
                    clearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(clearanceTime, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                    if (clearanceTime.TimeOfDay < currentAUECDate.TimeOfDay)
                    {
                        clearanceTime = currentAUECDate.Date + clearanceTime.TimeOfDay;
                    }
                    else if (_isOnlyAUECTimeBasedBlotterClearance)
                    {
                        clearanceTime = currentAUECDate.Date.AddDays(-1) + clearanceTime.TimeOfDay;
                    }
                    else
                    {
                        clearanceTime = currentAUECDate.Date;
                    }
                    isClearanceCalculated = true;
                }
                if (incomingOrder.MsgType == FIXConstants.MSGOrderRollOverRequest || incomingOrder.PranaMsgType == (int)Global.OrderFields.PranaMsgTypes.ORDNewSubChild ||
                    (isClearanceCalculated && clearanceTime > incomingOrder.AUECLocalDate
                      && !OrderInformation.IsMultiDayOrderHistory(incomingOrder)))
                {
                    //Skip those working sub order whose clearence have been passed and those comes from DB with their staged order (multiday staging)
                    isSkipOrderFromWorkingSubCollection = true;
                }

                if (incomingOrder.PranaMsgType != (int)Global.OrderFields.PranaMsgTypes.ORDStaged)
                {
                    if (!isSkipOrderFromWorkingSubCollection)
                    {
                        lock (_lockOnWorkingSubsTabCollection)
                        {
                            DateTime currentAuecdate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                            if ((incomingOrder.StagedOrderID != string.Empty))
                            {
                                if (_isOnlyAUECTimeBasedBlotterClearance || incomingOrder.AUECLocalDate.Date >= currentAuecdate.Date || (!incomingOrder.TIF.Equals(FIXConstants.TIF_Day) && !OrderInformation.IsMultiDayOrderInEndState(incomingOrder)))
                                {
                                    if (!workingSubsTabCollection.Contains(incomingOrder))
                                    {
                                        if (workingSubsTabCollection.Where(x => x.ParentClOrderID.Equals(incomingOrder.StagedOrderID.ToString())).Count() == 0 && newWorkingSubsList.Where(x => x.ParentClOrderID.Equals(incomingOrder.StagedOrderID.ToString())).Count() == 0)
                                        {
                                            newWorkingSubsList.Add(incomingOrder);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!workingSubsTabCollection.Contains(incomingOrder) && !newWorkingSubsList.Contains(incomingOrder))
                                {
                                    newWorkingSubsList.Add(incomingOrder);
                                }
                            }
                        }
                    }
                    lock (_lockOnParentClOrderIDCollection)
                    {
                        if ((incomingOrder.StagedOrderID != string.Empty) && (incomingOrder.StagedOrderID != incomingOrder.ClOrderID))
                        {
                            if (dictParentClOrderIDCollection.ContainsKey(incomingOrder.StagedOrderID))
                            {
                                OrderSingle parentOrder = dictParentClOrderIDCollection[incomingOrder.StagedOrderID];
                                if (parentOrder.OrderCollection == null)
                                {
                                    parentOrder.OrderCollection = new OrderBindingList();
                                }
                                if (parentOrder.OrderCollection.Where(x => x.ParentClOrderID == incomingOrder.ParentClOrderID).Count() == 0)
                                {
                                    parentOrder.OrderCollection.Add(incomingOrder);
                                }

                                double parentOrderPrevQty = parentOrder.CumQty;
                                UpdateStatusFromChildCollection(parentOrder);
                                UpdateParentStatus(incomingOrder, parentOrder);

                                if (parentOrder.StagedOrderID != string.Empty && parentOrder.StagedOrderID != int.MinValue.ToString() && dictParentClOrderIDCollection.ContainsKey(parentOrder.StagedOrderID))
                                {
                                    OrderSingle grandParentOrder = dictParentClOrderIDCollection[parentOrder.StagedOrderID];
                                    grandParentOrder.LastPrice = incomingOrder.LastPrice;
                                    grandParentOrder.LastShares = incomingOrder.LastShares;
                                    UpdateParentStatusFromSub(parentOrder, grandParentOrder, parentOrderPrevQty, incomingOrder.AvgPrice);
                                }
                                parentOrder.LastPrice = incomingOrder.LastPrice;
                                parentOrder.LastShares = incomingOrder.LastShares;
                            }
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

        /// <summary>
        /// Remove Multiday EndStateOrders
        /// </summary>
        private void RemoveMultiDayEndStateOrders()
        {
            try
            {
                Dictionary<int, DateTime> clearanceTimeCache = new Dictionary<int, DateTime>();
                List<OrderSingle> listMultiDayOrdersToRemove = new List<OrderSingle>();
                foreach (OrderSingle incomingOrder in workingSubsTabCollection)
                {
                    bool isMultiDayEndState = OrderInformation.IsMultiDayOrderInEndState(incomingOrder);
                    if (isMultiDayEndState)
                    {
                        DateTime currentAUECDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));

                        DateTime clearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                        if (clearanceTimeCache.ContainsKey(incomingOrder.AUECID))
                        {
                            clearanceTime = clearanceTimeCache[incomingOrder.AUECID];
                        }
                        else if (ClearanceCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.ContainsKey(incomingOrder.AUECID))
                        {
                            clearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(ClearanceCommonCache.GetInstance().DictAUECIDWiseBlotterClearance[incomingOrder.AUECID], CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                            clearanceTimeCache.Add(incomingOrder.AUECID, clearanceTime);
                        }

                        bool isWorkingSubOrderCanBeRemoved = false;

                        if (clearanceTime.TimeOfDay < currentAUECDate.TimeOfDay)
                        {
                            clearanceTime = currentAUECDate.Date + clearanceTime.TimeOfDay;
                        }
                        else if (_isOnlyAUECTimeBasedBlotterClearance)
                        {
                            clearanceTime = currentAUECDate.Date.AddDays(-1) + clearanceTime.TimeOfDay;
                        }
                        else
                        {
                            clearanceTime = currentAUECDate.Date;
                        }
                        DateTime lastActivityTimeOnThisOrder = OrderInformation.GetLastActivityTime(incomingOrder);
                        if (lastActivityTimeOnThisOrder <= clearanceTime)
                        {
                            isWorkingSubOrderCanBeRemoved = true;

                            if (incomingOrder.OrderCollection != null && incomingOrder.OrderCollection.Any(d => d.AUECLocalDate > clearanceTime))
                            {
                                isWorkingSubOrderCanBeRemoved = false;
                            }
                        }

                        if (isWorkingSubOrderCanBeRemoved)
                        {
                            listMultiDayOrdersToRemove.Add(incomingOrder);
                        }
                    }
                }
                foreach (OrderSingle endStateWorkingSubToRemove in listMultiDayOrdersToRemove)
                {
                    lock (_lockOnWorkingSubsTabCollection)
                    {
                        workingSubsTabCollection.Remove(endStateWorkingSubToRemove);
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

        /// <summary>
        /// Remove those orders clOrderId which are filled And Drop Copy Staged
        /// </summary>
        /// <param name="incomingOrders"></param>
        private void RemoveFilledAndDropCopyStagedOrders(List<OrderSingle> incomingOrders)
        {
            try
            {

                OrderSingle[] incomingOrdersCopy = new OrderSingle[incomingOrders.Count];
                incomingOrders.CopyTo(incomingOrdersCopy, 0);
                Dictionary<int, DateTime> clearanceTimeCache = new Dictionary<int, DateTime>();

                foreach (OrderSingle incomingOrder in incomingOrdersCopy)
                {
                    bool isFilledStage = (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Filled || incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled)
                                        && incomingOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged;

                    if (isFilledStage)
                    {
                        DateTime currentAUECDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));

                        DateTime clearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                        if (clearanceTimeCache.ContainsKey(incomingOrder.AUECID))
                        {
                            clearanceTime = clearanceTimeCache[incomingOrder.AUECID];
                        }
                        else if (ClearanceCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.ContainsKey(incomingOrder.AUECID))
                        {
                            clearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(ClearanceCommonCache.GetInstance().DictAUECIDWiseBlotterClearance[incomingOrder.AUECID], CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                            clearanceTimeCache.Add(incomingOrder.AUECID, clearanceTime);
                        }

                        bool isStagedOrderCanBeRemoved = false;

                        if (clearanceTime.TimeOfDay < currentAUECDate.TimeOfDay)
                        {
                            clearanceTime = currentAUECDate.Date + clearanceTime.TimeOfDay;
                        }
                        else if (_isOnlyAUECTimeBasedBlotterClearance)
                        {
                            clearanceTime = currentAUECDate.Date.AddDays(-1) + clearanceTime.TimeOfDay;
                        }
                        else
                        {
                            clearanceTime = currentAUECDate.Date;
                        }

                        if (incomingOrder.AUECLocalDate <= clearanceTime)
                        {
                            isStagedOrderCanBeRemoved = true;

                            if (incomingOrder.OrderCollection != null && incomingOrder.OrderCollection.Any(d => d.AUECLocalDate > clearanceTime || ((OrderInformation.IsMultiDayOrder(d) && workingSubsTabCollection.Contains(d))))) //TODO:MultiDay History check
                            {
                                isStagedOrderCanBeRemoved = false;
                            }
                        }

                        if (isStagedOrderCanBeRemoved)
                        {
                            if (_cLOrderIDToSetIsHiddenInSubTable != null)
                                _cLOrderIDToSetIsHiddenInSubTable.Add(incomingOrder.ParentClOrderID);
                            lock (_lockOnParentClOrderIDCollection)
                            {
                                if (DictParentClOrderIDCollection.ContainsKey(incomingOrder.ParentClOrderID))
                                {
                                    DictParentClOrderIDCollection.Remove(incomingOrder.ParentClOrderID);
                                }
                                if (incomingOrder.OrderCollection != null)
                                {
                                    foreach (OrderSingle SubOrder in incomingOrder.OrderCollection)
                                    {
                                        _cLOrderIDToSetIsHiddenInSubTable.Add(SubOrder.ParentClOrderID);
                                        if (DictParentClOrderIDCollection.ContainsKey(SubOrder.ParentClOrderID))
                                        {
                                            DictParentClOrderIDCollection.Remove(SubOrder.ParentClOrderID);
                                        }
                                        // check for grand Child Orders - Order fill Summary
                                        if (SubOrder.OrderCollection != null)
                                        {
                                            foreach (OrderSingle orderfillsummary in SubOrder.OrderCollection)
                                            {
                                                _cLOrderIDToSetIsHiddenInSubTable.Add(orderfillsummary.ParentClOrderID);
                                            }
                                        }
                                        lock (_lockOnWorkingSubsTabCollection)
                                        {
                                            workingSubsTabCollection.Remove(SubOrder);
                                        }
                                    }
                                }
                            }
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

        /// <summary>
        /// Remove MultiBrokersSubs
        /// </summary>
        private void RemoveMultiBrokerSubFromAllOrderCollection()
        {
            try
            {
                List<string> listStagedOrderIDs = new List<string>();
                listStagedOrderIDs = workingSubsTabCollection.Select(x => x.StagedOrderID).Distinct().ToList();

                for (int counter = 0; counter < workingSubsTabCollection.Count; counter++)
                {
                    if (!listStagedOrderIDs.Contains(workingSubsTabCollection[counter].StagedOrderID))
                    {
                        _cLOrderIDToSetIsHiddenInSubTable.Add(workingSubsTabCollection[counter].ClOrderID);
                        workingSubsTabCollection.Remove(workingSubsTabCollection[counter]);
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
        #endregion

        /// <summary>
        /// Update Important fields of Existing Order
        /// </summary>
        /// <param name="existingOrder"></param>
        /// <param name="incomingOrder"></param>
        private void UpdateExistingOrder(OrderSingle existingOrder, OrderSingle incomingOrder)
        {
            try
            {
                existingOrder.ClOrderID = incomingOrder.ClOrderID;
                existingOrder.Quantity = incomingOrder.Quantity;
                existingOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(incomingOrder.OrderStatusTagValue);
                if (!(OrderInformation.IsMultiDayOrder(existingOrder) &&
                      (incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_RollOver) || incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingRollOver))))
                    existingOrder.ExecutionTimeLastFill = incomingOrder.ExecutionTimeLastFill;
                existingOrder.CumQty = incomingOrder.CumQty;
                existingOrder.AvgPrice = incomingOrder.AvgPrice;
                existingOrder.LeavesQty = incomingOrder.LeavesQty;
                if (incomingOrder.OrigClOrderID != string.Empty)
                {
                    existingOrder.OrigClOrderID = incomingOrder.OrigClOrderID;
                }
                if (incomingOrder.OrderStatusTagValue != string.Empty)
                {
                    existingOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(incomingOrder.OrderStatusTagValue.ToString());
                    existingOrder.OrderStatusTagValue = incomingOrder.OrderStatusTagValue;
                }
                existingOrder.TIF = incomingOrder.TIF;
                if (incomingOrder.ExpireTime != null && !(string.IsNullOrEmpty(incomingOrder.ExpireTime)) && incomingOrder.TIF == FIXConstants.TIF_GTD)
                {
                    existingOrder.ExpireTime = incomingOrder.ExpireTime;
                }
                else if (incomingOrder.TIF != FIXConstants.TIF_GTD)
                    existingOrder.ExpireTime = "N/A";
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

        #region Update Parent Methods
        /// <summary>
        /// Update parent status from child collection
        /// </summary>
        /// <param name="parentOrder"></param>
        /// <param name="grandParentOrder"></param>
        /// <param name="parentOrderPrevQty"></param>
        /// <param name="avgPx"></param>
        private void UpdateParentStatusFromSub(OrderSingle parentOrder, OrderSingle grandParentOrder, double parentOrderPrevQty, double avgPx)
        {
            try
            {
                if (OrderInformation.IsMultiDayOrderHistory(parentOrder) &&
                    (parentOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_Expired) || parentOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_DoneForDay)))
                {
                    UpdateStatusFromChildCollection(grandParentOrder);
                }
                else
                {
                    double amount = grandParentOrder.CumQty * grandParentOrder.AvgPrice;

                    if (parentOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingNew && parentOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_New && parentOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected && parentOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Rejected)
                    {
                        grandParentOrder.CumQty = grandParentOrder.CumQty + parentOrder.CumQty - parentOrderPrevQty;
                        grandParentOrder.LeavesQty = grandParentOrder.Quantity - grandParentOrder.CumQty - grandParentOrder.UnsentQty;
                    }

                    parentOrder.UnsentQty = 0;
                    parentOrder.LeavesQty = parentOrder.Quantity - parentOrder.CumQty;
                    if (OrderInformation.IsOrderInEndState(parentOrder))
                        parentOrder.LeavesQty = 0.0;
                    amount += ((parentOrder.CumQty - parentOrderPrevQty) * avgPx);

                    if (grandParentOrder.CumQty > 0)
                    {
                        grandParentOrder.AvgPrice = amount / grandParentOrder.CumQty;
                    }
                }
                if (grandParentOrder.CumQty == 0)
                {
                    grandParentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;
                }
                else if (grandParentOrder.CumQty == grandParentOrder.Quantity)
                {
                    grandParentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                }
                else if (grandParentOrder.CumQty > 0 && grandParentOrder.CumQty < grandParentOrder.Quantity)
                {
                    grandParentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;
                }

                grandParentOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(grandParentOrder.OrderStatusTagValue.ToString());
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
        /// Update parent status from incoming suborder
        /// </summary>
        /// <param name="orderResponse">incoming Order</param>
        /// <param name="parentOrder">Parent Order</param>
        private void UpdateParentStatus(OrderSingle orderResponse, OrderSingle parentOrder)
        {
            try
            {
                //In case of Multi-day sub orders the execution time of last fill should be updated
                if (orderResponse.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSubChild)
                    parentOrder.ExecutionTimeLastFill = orderResponse.ExecutionTimeLastFill;

                if (parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingCancel)
                {
                    bool isCancelleable = true;
                    if (parentOrder.OrderCollection != null)
                    {
                        foreach (OrderSingle subOrder in parentOrder.OrderCollection)
                        {
                            if (subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Filled &&
                                subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Expired &&
                                subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Cancelled &&
                                subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_RollOver &&
                                subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Stopped &&
                                subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Rejected &&
                                subOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_Aborted &&
                                subOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected)
                            {
                                isCancelleable = false;
                                break;
                            }
                        }
                    }
                    if (isCancelleable)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Cancelled;
                        parentOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(parentOrder.OrderStatusTagValue.ToString());
                        parentOrder.OrderSeqNumber = Int64.MinValue;
                    }
                    else
                    {
                        if (orderResponse.MsgType == FIXConstants.MSGOrderCancelReject)
                        {
                            if (parentOrder.CumQty == 0.0)
                            {
                                parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;
                            }
                            else if (parentOrder.CumQty > 0.0 && parentOrder.CumQty < parentOrder.Quantity)
                            {
                                parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;
                            }
                            else if (parentOrder.CumQty == parentOrder.Quantity)
                            {
                                parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                            }
                        }
                    }
                }
                else if (parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_New || parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew)
                {
                    if (parentOrder.CumQty > 0 && parentOrder.CumQty < parentOrder.Quantity)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;
                    }
                    else if (parentOrder.CumQty == parentOrder.Quantity)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                    }
                }
                else if (parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PartiallyFilled)
                {
                    if (parentOrder.CumQty == 0)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;
                    }
                    if (parentOrder.CumQty == parentOrder.Quantity)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                    }

                }
                else if (parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Replaced)
                {
                    if (parentOrder.CumQty > 0 && parentOrder.CumQty < parentOrder.Quantity
                        && orderResponse.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged
                        && orderResponse.OrderStatusTagValue != FIXConstants.ORDSTATUS_Replaced && orderResponse.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingReplace && orderResponse.OrderSeqNumber != parentOrder.OrderSeqNumber)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;

                    }
                    if (parentOrder.Quantity == parentOrder.CumQty && parentOrder.LeavesQty == 0)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                    }
                }

                // Added the condition to update orderside tag value, when order status is filled
                else if (parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Filled)
                {
                    if (parentOrder.CumQty > 0 && parentOrder.CumQty < parentOrder.Quantity)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;
                    }
                    else if (parentOrder.CumQty == 0)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;
                    }
                }

                // get current status text to show in blotter
                parentOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(parentOrder.OrderStatusTagValue.ToString());
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
        /// Used for staged orders parent update from Child updates
        /// </summary>
        string previousBroker = string.Empty;
        public void UpdateStatusFromChildCollection(OrderSingle orderSingle)
        {
            try
            {
                double cumQty = 0;
                double leavesQty = 0;
                double amount = 0;
                double avgPrice = 0;
                double dayExecutedQty = 0;
                double dayAveragePrice = 0;
                double beforeStartOfDayExecuted = 0;
                string orderStatus = orderSingle.OrderStatus;
                long seqNum = 0;

                if (orderSingle.OrderCollection != null)
                {
                    previousBroker = string.Empty;
                    foreach (OrderSingle subOrder in orderSingle.OrderCollection)
                    {
                        if (!orderSingle.IsUseCustodianBroker)
                        {
                            if (previousBroker == string.Empty)
                            {
                                previousBroker = subOrder.CounterPartyName;
                                orderSingle.CounterPartyName = subOrder.CounterPartyName;
                            }
                            else if (previousBroker != subOrder.CounterPartyName)
                                orderSingle.CounterPartyName = "Multiple";
                        }
                        if (subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew)
                        {
                            leavesQty += subOrder.Quantity;
                            if (OrderInformation.IsMultiDayOrderHistory(orderSingle))
                            {
                                cumQty += subOrder.CumQty;
                                dayExecutedQty += subOrder.CumQty;
                            }
                        }
                        else if (subOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected && subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Rejected)
                        {
                            cumQty += subOrder.CumQty;
                            leavesQty += subOrder.LeavesQty;

                            DateTime currentAUECDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(subOrder.AUECID));
                            if (subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_RollOver && ClearanceCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.ContainsKey(subOrder.AUECID))
                            {
                                DateTime clearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(ClearanceCommonCache.GetInstance().DictAUECIDWiseBlotterClearance[subOrder.AUECID], CachedDataManager.GetInstance.GetAUECTimeZone(subOrder.AUECID));

                                if (clearanceTime.TimeOfDay < currentAUECDate.TimeOfDay)
                                {
                                    clearanceTime = currentAUECDate.Date + clearanceTime.TimeOfDay;
                                }
                                else if (_isOnlyAUECTimeBasedBlotterClearance)
                                {
                                    clearanceTime = currentAUECDate.Date.AddDays(-1) + clearanceTime.TimeOfDay;
                                }
                                else
                                {
                                    clearanceTime = currentAUECDate.Date;
                                }
                                if (subOrder.AUECLocalDate > clearanceTime)
                                {
                                    if (dayExecutedQty + subOrder.CumQty != 0)
                                        dayAveragePrice = ((dayExecutedQty * dayAveragePrice) + (subOrder.AvgPrice * subOrder.CumQty)) / (dayExecutedQty + subOrder.CumQty);
                                    dayExecutedQty += subOrder.CumQty;
                                }
                                else
                                {
                                    beforeStartOfDayExecuted += subOrder.CumQty;
                                }
                            }
                            else
                            {
                                beforeStartOfDayExecuted += subOrder.CumQty;
                            }

                            avgPrice = subOrder.AvgPrice;
                            amount += avgPrice * subOrder.CumQty;
                            orderStatus = subOrder.OrderStatus;
                            seqNum = subOrder.OrderSeqNumber;
                        }
                    }

                    if ((orderSingle.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged) &&
                          OrderInformation.IsMultiDayOrderHistory(orderSingle))
                    {
                        leavesQty = orderSingle.Quantity - cumQty;
                        if (OrderInformation.IsOrderInEndState(orderSingle))
                            leavesQty = 0.0;
                    }
                    orderSingle.UnsentQty = orderSingle.Quantity - cumQty - leavesQty;
                    orderSingle.LeavesQty = leavesQty;
                    orderSingle.CumQty = cumQty;
                    orderSingle.DayCumQty = dayExecutedQty;
                    orderSingle.DayAvgPx = dayAveragePrice;
                    orderSingle.DayOrderQty = orderSingle.Quantity - beforeStartOfDayExecuted;
                    if (orderSingle.OrderStatusTagValue == FIXConstants.ORDSTATUS_Replaced && orderSingle.OrderSeqNumber < seqNum)
                        orderSingle.OrderStatus = orderStatus;

                    if (cumQty > 0)
                    {
                        orderSingle.AvgPrice = amount / cumQty;
                        ((PranaBasicMessage)(orderSingle)).AvgPrice = amount / cumQty;
                    }
                    else
                    {
                        orderSingle.AvgPrice = 0.0;
                        ((PranaBasicMessage)(orderSingle)).AvgPrice = 0.0;
                    }
                }
                else
                {
                    if ((orderSingle.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged) &&
                         OrderInformation.IsMultiDayOrderHistory(orderSingle))
                    {
                        orderSingle.DayOrderQty = ((PranaBasicMessage)(orderSingle)).Quantity;
                        orderSingle.LeavesQty = ((PranaBasicMessage)(orderSingle)).Quantity;
                        orderSingle.UnsentQty = 0;

                        if (OrderInformation.IsOrderInEndState(orderSingle))
                            orderSingle.LeavesQty = 0;
                    }
                    else
                    {
                        orderSingle.UnsentQty = ((PranaBasicMessage)(orderSingle)).Quantity;
                        orderSingle.LeavesQty = 0;
                        orderSingle.DayOrderQty = ((PranaBasicMessage)(orderSingle)).Quantity;
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
        #endregion

        #region Manual Rollover All

        /// <summary>
        /// Rollover of SubOrder
        /// </summary>
        /// <param name="Order"></param>
        /// <param name="userId"></param>
        public void RolloverOfSubOrder(OrderSingle subOrder, int userId)
        {
            try
            {
                List<OrderSingle> rolloverOrderCollection = new List<OrderSingle>();

                Prana.BusinessObjects.TimeZone auecTimeZone = CachedDataManager.GetInstance.GetAUECTimeZone(subOrder.AUECID);
                DateTime currentAUECDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, auecTimeZone);
                DateTime localClearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(ClearanceCommonCache.GetInstance().DictAUECIDWiseBlotterClearance[subOrder.AUECID], auecTimeZone);
                if (localClearanceTime.TimeOfDay < currentAUECDate.TimeOfDay)
                {
                    localClearanceTime = currentAUECDate.Date + localClearanceTime.TimeOfDay;
                }
                else if (_isOnlyAUECTimeBasedBlotterClearance)
                {
                    localClearanceTime = currentAUECDate.Date.AddDays(-1) + localClearanceTime.TimeOfDay;
                }
                else
                {
                    localClearanceTime = currentAUECDate.Date;
                }

                if (IsActiveMultiDayOrder(subOrder, localClearanceTime))
                {
                    publishBlotterStatusBarMessage("Rollover of live - market GTC / GTD orders not allowed", userId);
                    return;
                }

                if (subOrder.AUECLocalDate < localClearanceTime)
                {
                    OrderSingle rolloverRequest = (OrderSingle)subOrder.Clone();
                    rolloverRequest.MsgType = FIXConstants.MSGOrderRollOverRequest;
                    if (rolloverRequest.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingCancel || rolloverRequest.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingReplace)
                    {
                        rolloverRequest.OrigClOrderID = rolloverRequest.OrigClOrderID;
                    }
                    else
                    {
                        rolloverRequest.OrigClOrderID = rolloverRequest.ClOrderID;
                    }
                    rolloverRequest.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingRollOver;
                    rolloverRequest.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(rolloverRequest.OrderStatusTagValue.ToString());
                    rolloverOrderCollection.Add(rolloverRequest);

                    RollOverAuditTrailManager.GetInstance().AddAuditTrailCollection(rolloverRequest, TradeAuditActionType.ActionType.SubOrderRollover, userId, " Sub-Order Rollover Requested");
                }

                if (rolloverOrderCollection.Count > 0)
                {
                    SendRolloverRequest(rolloverOrderCollection, userId);
                    RollOverAuditTrailManager.GetInstance().SaveAuditTrailData();
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
        /// Publish Blotter Status Bar Message
        /// </summary>
        private void publishBlotterStatusBarMessage(string message, int userId)
        {
            if (_proxyPublishing == null)
            {
                CreatePublishingProxy();
            }
            List<string> listOfmessage = new List<string>();
            listOfmessage.Add(message);
            listOfmessage.Add(userId.ToString());

            MessageData messageData = new MessageData();
            messageData.EventData = listOfmessage;
            messageData.TopicName = Topics.Topic_UpdateBlotterStatusBarMessage;
            _proxyPublishing.InnerChannel.Publish(messageData, Topics.Topic_UpdateBlotterStatusBarMessage);
        }

        /// <summary>
        /// SendRolloverRequest
        /// </summary>
        /// <param name="ordersRequest"></param>
        /// <param name="userId"></param>
        private void SendRolloverRequest(List<OrderSingle> ordersRequest, int userId)
        {
            try
            {
                foreach (OrderSingle orderRequest in ordersRequest)
                {
                    OrderSingle orderSingle = new OrderSingle();
                    orderSingle = (OrderSingle)orderRequest.Clone();
                    orderSingle.ModifiedUserId = userId;
                    orderSingle.TransactionTime = DateTime.Now.ToUniversalTime();
                    SendTradeAfterCheckCPConnection(orderSingle);
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
        /// send trade after checking
        /// </summary>
        /// <param name="validatedOrder"></param>
        private void SendTradeAfterCheckCPConnection(OrderSingle validatedOrder)
        {
            if (validatedOrder.ListID != string.Empty)
            {
                BasketDetail basket = new BasketDetail();
                basket.UserID = validatedOrder.CompanyUserID;
                basket.TradingAccountID = validatedOrder.TradingAccountID;
                basket.BasketID = validatedOrder.ListID;
                OrderCollection basketOrders = new OrderCollection();
                Order basketOrder = new Order(validatedOrder);
                basketOrders.Add(basketOrder);
                Prana.BusinessObjects.FIX.PranaMessage basketMessage = Transformer.CreatePranaMessageThroughReflection(basket, basketOrders, FIXConstants.MSGOrderList);
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow Out2] Before SendMessage In TradeManager, userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ", Fix Message: " + Convert.ToString(basketMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                }
                PranaOrderProcessor.GetInstance.ProcessMessage(basketMessage);
            }
            Prana.BusinessObjects.FIX.PranaMessage message = Transformer.CreatePranaMessageThroughReflection(validatedOrder);
            if (_enableTradeFlowLogging)
            {
                try
                {
                    Logger.LoggerWrite("[Trade-Flow Out2] After SendMessage In TradeManager, userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ", Fix Message: " + Convert.ToString(message.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                }
                catch (Exception ex)
                {
                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                }
            }
            PranaOrderProcessor.GetInstance.ProcessMessage(message);
        }

        /// <summary>
        /// To check if any Multi-Day order is in Active State
        /// </summary>
        /// <param name="subOrder"></param>
        /// <param name="localClearanceTime"></param>
        /// <returns></returns>
        private bool IsActiveMultiDayOrder(OrderSingle subOrder, DateTime localClearanceTime)
        {
            if (subOrder.TIF.Equals(FIXConstants.TIF_GTC))
                return true;
            if (subOrder.TIF.Equals(FIXConstants.TIF_GTD))
            {
                string expireTimeTemp = subOrder.ExpireTime;
                if (!string.IsNullOrEmpty(expireTimeTemp) && (!expireTimeTemp.Equals("N/A")))
                {
                    DateTime expireTime = Convert.ToDateTime(expireTimeTemp);
                    expireTime = Prana.BusinessLogic.BusinessDayCalculator.GetInstance().GetPreviousBusinessDay(expireTime.AddDays(1), subOrder.AUECID);
                    if (localClearanceTime < expireTime)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
