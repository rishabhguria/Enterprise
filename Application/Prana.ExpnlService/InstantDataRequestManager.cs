using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ExpnlService
{
    // This class is implemented to fulfill the instant data request which is fired whenever user changes the tab/view or adds any column on PM.
    public class InstantDataRequestManager
    {
        public ExPnlCache ExPNLCache
        {
            get { return ExPnlCache.Instance; }
        }
        // Process Time is the time which is consumed in processing a single request.
        // We calculates it from the time taken in calculating the data at EXPNL end.
        private long processTime;
        public long ProcessTime
        {
            get { return processTime; }
            set { processTime = value; }
        }
        // It is stopwatch used to calculate process time. It is set whenever we start calculation for data and stop when we finish calculations.
        private System.Diagnostics.Stopwatch processTimeSW = new System.Diagnostics.Stopwatch();
        public System.Diagnostics.Stopwatch ProcessTimeSW
        {
            get { return processTimeSW; }
        }
        // Dictionary contains information about status of a user like isBusy and freeuptime .
        private static Dictionary<string, UserStatus> exPNLUserStatus = new Dictionary<string, UserStatus>();
        public Dictionary<string, UserStatus> ExPNLUserStatus
        {
            get { return exPNLUserStatus; }
            set { exPNLUserStatus = value; }
        }
        // This contains the instant data request which are queued if data is being sent or user is busy.
        private Dictionary<string, Dictionary<ExPNLPreferenceMsgType, List<string>>> pendingInstantDataRequestDict = new Dictionary<string, Dictionary<ExPNLPreferenceMsgType, List<string>>>();
        public Dictionary<string, Dictionary<ExPNLPreferenceMsgType, List<string>>> PendingInstantDataRequestDict
        {
            get { return pendingInstantDataRequestDict; }
            set { pendingInstantDataRequestDict = value; }
        }

        // It is user free up time . We calculate it based on how much time is required to send data to client and get back the response of user free.
        public void SetUserFreeUpTime(string userID, bool isUserBusy)
        {
            try
            {
                if (ExPNLUserStatus.ContainsKey(userID))
                {
                    if (ExPNLUserStatus[userID].UserSW.IsRunning)
                    {
                        ExPNLUserStatus[userID].UserFreeUpTime = ExPNLUserStatus[userID].UserSW.ElapsedMilliseconds;
                        ExPNLUserStatus[userID].UserSW.Reset();
                        ExPNLUserStatus[userID].UserSW.Stop();
                    }
                    else
                    {
                        ExPNLUserStatus[userID].UserSW.Reset();
                        ExPNLUserStatus[userID].UserSW.Start();
                    }
                    ExPNLUserStatus[userID].UserBusyStatus = isUserBusy;
                }
                else
                {
                    UserStatus userStatus = new UserStatus();
                    ExPNLUserStatus.Add(userID, userStatus);
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
        // called when user changes tab/view or adds any column on PM.
        public void HandleInstantDataSending(string userID, ExPNLPreferenceMsgType msgType, string subMsg)
        {
            try
            {
                lock (_lockerObj)
                {
                    Dictionary<ExPNLPreferenceMsgType, List<string>> msgSubMsgDict = new Dictionary<ExPNLPreferenceMsgType, List<string>>();
                    List<string> subMsgs = new List<string>();
                    subMsgs.Add(subMsg);
                    msgSubMsgDict.Add(msgType, subMsgs);

                    if (ExPNLUserStatus.ContainsKey(userID))
                    {
                        // We only fulfill the Instant data request if client is free or data is not being sent, otherwise we add this request to queue.
                        if (ExPNLUserStatus[userID].UserBusyStatus || ServiceManager.GetInstance().IsSending)
                        {
                            if (!pendingInstantDataRequestDict.ContainsKey(userID))
                            {
                                pendingInstantDataRequestDict.Add(userID, msgSubMsgDict);
                            }
                            else
                            {
                                // If we get instant data request for selected view changed then we are checking whether there are any queued request for 
                                // Column addition. If there are any pending req for column addition then we delete it and add view change req to queue
                                // as view change req has higher priority.
                                if (msgType == ExPNLPreferenceMsgType.SelectedViewChanged)
                                {
                                    pendingInstantDataRequestDict.Remove(userID);
                                    pendingInstantDataRequestDict.Add(userID, msgSubMsgDict);
                                }
                                else if (msgType == ExPNLPreferenceMsgType.FilterValueChanged)
                                {
                                    if (!pendingInstantDataRequestDict[userID].ContainsKey(ExPNLPreferenceMsgType.SelectedViewChanged))
                                    {
                                        if (pendingInstantDataRequestDict[userID].ContainsKey(ExPNLPreferenceMsgType.FilterValueChanged))
                                        {
                                            PendingInstantDataRequestDict[userID].Remove(ExPNLPreferenceMsgType.FilterValueChanged);
                                        }
                                        pendingInstantDataRequestDict[userID].Add(ExPNLPreferenceMsgType.FilterValueChanged, subMsgs);
                                    }
                                }
                                else
                                {
                                    // checking here if we have any req for view change.
                                    // If we have the same then not adding column add req as we don't overwrite view change req with column addition.
                                    // There may only 2 types of req:- View change or column add, so just checking pending reqs count.
                                    // AfterView change, If user will add columns then it will be taken care automatically as we are updating the DynamicColumnlist 
                                    if (!pendingInstantDataRequestDict[userID].ContainsKey(ExPNLPreferenceMsgType.SelectedViewChanged))
                                    {
                                        if (pendingInstantDataRequestDict[userID].ContainsKey(ExPNLPreferenceMsgType.SelectedColumnAdded))
                                        {
                                            if (!pendingInstantDataRequestDict[userID][ExPNLPreferenceMsgType.SelectedColumnAdded].Contains(subMsg))
                                            {
                                                // Append to the newly added column list .
                                                pendingInstantDataRequestDict[userID][ExPNLPreferenceMsgType.SelectedColumnAdded].Add(subMsg);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (msgType == ExPNLPreferenceMsgType.FilterValueChanged)
                            {
                                ServiceManager.GetInstance().SendInstantDynamicFilterData(subMsgs, userID);
                            }
                            // Decision making logic : whether to send instant data or not?
                            // It is being decided on " If Instant data request fulfill time is less then to the time remaining in Next refresh timer tick then we send data else wait for next refresh tick.
                            // Here we are calculating the 'Userfreeup time' twice as there may be case like total instant data req fulfill time is 100 ms and time remaining in next timer tick is 101 ms.                        
                            // Then in this case there may simultaneously two req within 1 ms.
                            // So we giving breath equal to the client free up time.                        
                            if ((ExPNLCache.RefreshInterval - ExPNLCache.RefreshStopwatch.ElapsedMilliseconds) > (ProcessTime + 2 * (ExPNLUserStatus[userID].UserFreeUpTime)))
                            {
                                if (msgType == ExPNLPreferenceMsgType.SelectedColumnAdded)
                                {
                                    ServiceManager.GetInstance().SendInstantDataToUsers(subMsgs, userID);
                                }
                                else
                                {
                                    // For view change req we have to send data for all column, so sending selected column list as NULL as there 
                                    // this list will be fetched from Dynamic Column list.
                                    ServiceManager.GetInstance().SendInstantDataToUsers(null, userID);
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
        object _lockerObj = new object();
        public void SendInstantDataForQueuedRequests(string userID)
        {
            try
            {
                lock (_lockerObj)
                {
                    if (ExPNLUserStatus.ContainsKey(userID))
                    {
                        if (!ServiceManager.GetInstance().IsSending)
                        {
                            if ((ExPNLCache.RefreshInterval - ExPNLCache.RefreshStopwatch.ElapsedMilliseconds) > (ProcessTime + 2 * (ExPNLUserStatus[userID].UserFreeUpTime)))
                            {
                                if (pendingInstantDataRequestDict.ContainsKey(userID))
                                {
                                    if (pendingInstantDataRequestDict[userID].ContainsKey(ExPNLPreferenceMsgType.SelectedColumnAdded))
                                    {
                                        ServiceManager.GetInstance().SendInstantDataToUsers(pendingInstantDataRequestDict[userID][ExPNLPreferenceMsgType.SelectedColumnAdded], userID);
                                    }
                                    else
                                    {
                                        ServiceManager.GetInstance().SendInstantDataToUsers(null, userID);
                                    }
                                    pendingInstantDataRequestDict.Remove(userID);
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
    }
}
