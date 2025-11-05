using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.DropCopyProcessor_PostTrade
{
    public class DropCopyCacheManager_PostTrade
    {
        static readonly object lockobjNewOrders = new object();
        // all orders are kept here not fills
        static Dictionary<string, List<PranaMessage>> newOrders = new Dictionary<string, List<PranaMessage>>();

        static readonly object lockobj = new object();
        // all fills are kept here
        static HashSet<string> _receivedMessages = new HashSet<string>();
        public static void AddNewOrder(PranaMessage pranaMsg)
        {
            try
            {
                string orderID = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value;
                lock (lockobjNewOrders)
                {
                    if (!newOrders.ContainsKey(orderID))
                    {
                        List<PranaMessage> list = new List<PranaMessage>();
                        list.Add(pranaMsg);
                        newOrders.Add(orderID, list);
                    }
                    else
                    {
                        newOrders[orderID].Add(pranaMsg);
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

        public static void AddReceivedMessage(PranaMessage pranaMsg)
        {
            try
            {
                string orderID = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value;
                // need to clone it to keep original order . as processing may change the object
                lock (lockobj)
                {
                    if (!_receivedMessages.Contains(orderID))
                    {
                        _receivedMessages.Add(orderID);
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

        public static void ClearOrderCache()
        {
            try
            {
                if (null != _receivedMessages)
                {
                    _receivedMessages.Clear();
                    Logger.HandleException(new Exception("All order cache has been cleared by the User"), LoggingConstants.POLICY_LOGANDSHOW);
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

        public static void ClearOrder(string orderID)
        {
            try
            {
                if (null != _receivedMessages && _receivedMessages.Contains(orderID))
                {
                    _receivedMessages.Remove(orderID);
                    Logger.HandleException(new Exception("The following OrderID has been cleared by the User: " + orderID), LoggingConstants.POLICY_LOGANDSHOW);
                }
                else
                {
                    Logger.HandleException(new Exception("Cache doesn't contain the entered OrderID. Please enter the correct OrderID."), LoggingConstants.POLICY_LOGANDSHOW);
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

        public static string GetClOrderID(string orderID)
        {
            if (newOrders.ContainsKey(orderID))
            {
                List<PranaMessage> list = newOrders[orderID];
                return list[list.Count - 1].FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
            }
            return String.Empty;
        }

        public static string GetParentClOrderID(string orderID)
        {
            List<PranaMessage> list = newOrders[orderID];
            PranaMessage pranaMsg = list[0];
            return list[0].FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value;
        }

        public static bool DoesOrderExist(PranaMessage pranaMsg)
        {
            try
            {
                lock (lockobj)
                {
                    if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrderID))
                    {
                        string orderID = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value;
                        return _receivedMessages.Contains(orderID);
                    }
                    else
                    {
                        return false;
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
            return false;
        }

        public static bool IsCXLReplaceNewOrder(PranaMessage pranaMsg)
        {
            try
            {
                lock (lockobj)
                {
                    if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                    {
                        string origClOrderID = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                        if (newOrders.ContainsKey(origClOrderID))
                        {
                            return true;
                        }
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
            return false;
        }

        public static List<PranaMessage> GetCachedMessages()
        {
            List<PranaMessage> pranaMsgList = new List<PranaMessage>();
            try
            {
                //lock (lockobj)
                //{
                //foreach (KeyValuePair<string, List<PranaMessage>> item in _receivedMessages)
                //{
                //    foreach (PranaMessage pranaMsg in item.Value)
                //    {
                //        pranaMsgList.Add(pranaMsg);
                //    }
                //}
                //}
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
            return pranaMsgList;
        }

        public static void FillCacheFromDataBase()
        {
            try
            {
                List<PranaMessage> pranaFillsMsgList = Transformer.CreatePranaMessages(DropCopyPersistence.GetDropCopyOrderFills());
                foreach (PranaMessage pranaMsg in pranaFillsMsgList)
                {
                    AddReceivedMessage(pranaMsg);
                }
                List<PranaMessage> pranaOrderRequestsMsgList = Transformer.CreatePranaMessages(DropCopyPersistence.GetDropCopyOrderRequest());
                foreach (PranaMessage pranaMsg in pranaOrderRequestsMsgList)
                {
                    AddNewOrder(pranaMsg);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
    }

    class PranaMessageCollectionForSymbols
    {
        static readonly object lockobj = new object();
        static Dictionary<string, List<PranaMessage>> _pranaMsgCollection = new Dictionary<string, List<PranaMessage>>();
        public static void Add(SecMasterRequestObj secMasterRequestObj, PranaMessage pranaMsg, int _hashCode)
        {
            try
            {
                lock (lockobj)
                {
                    bool shouldReqSm = false;
                    foreach (SymbolDataRow datarow in secMasterRequestObj.SymbolDataRowCollection)
                    {
                        string symbol = datarow.PrimarySymbol;
                        if (!_pranaMsgCollection.ContainsKey(symbol))
                        {
                            _pranaMsgCollection.Add(symbol, new List<PranaMessage>());
                            shouldReqSm = true;
                        }
                        _pranaMsgCollection[symbol].Add(pranaMsg);
                    }
                    if (shouldReqSm)
                        _secMasterServices.GetSecMasterData(secMasterRequestObj, _hashCode);
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

        public static List<PranaMessage> GetAllPranaMsg(string inSymbol, ApplicationConstants.SymbologyCodes symbologyCode)
        {
            List<PranaMessage> pranaMsgList = new List<PranaMessage>();
            try
            {
                lock (lockobj)
                {
                    if (_pranaMsgCollection.ContainsKey(inSymbol))
                    {
                        foreach (PranaMessage pranaMsg in _pranaMsgCollection[inSymbol])
                        {
                            pranaMsgList.Add(pranaMsg);
                        }
                        _pranaMsgCollection.Remove(inSymbol);
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
            return pranaMsgList;
        }

        /// <summary>
        /// Getting trades from drop-copy Collection
        /// </summary>
        /// <returns></returns>
        public static List<PranaMessage> GetAllPranaMsg()
        {
            List<PranaMessage> pranaMsgList = new List<PranaMessage>();
            try
            {
                //TODO check performance by lock Here   - Omshiv
                lock (lockobj)
                {
                    int waitTime = CommonDataCache.CachedDataManager.GetInstance.WaitTimeToGetStuckTrade;
                    DateTime currentTime = DateTime.Now;
                    foreach (KeyValuePair<string, List<PranaMessage>> item in _pranaMsgCollection)
                    {
                        foreach (PranaMessage pranaMsg in item.Value)
                        {
                            if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ServerReceivedFullTime))
                            {
                                String recievedTimeStr = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ServerReceivedFullTime].Value;
                                DateTime recievedTime = GetDateTime(recievedTimeStr);
                                TimeSpan span = currentTime - recievedTime;
                                // int tradeWaitTime = (int)span.Seconds; // to get positive time of waiting :: Jira CI-6800
                                int tradeWaitTime = (int)span.TotalSeconds;
                                if (waitTime <= tradeWaitTime)
                                {
                                    pranaMsgList.Add(pranaMsg);
                                }
                            }
                        }
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
            return pranaMsgList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recievedTime"></param>
        private static DateTime GetDateTime(string recievedTime)
        {
            DateTime dateTime = new DateTime();
            try
            {
                if (recievedTime.Length == 14)
                {
                    int year = int.Parse(recievedTime.Substring(0, 4));
                    int month = int.Parse(recievedTime.Substring(4, 2));
                    int date = int.Parse(recievedTime.Substring(6, 2));
                    int hours = int.Parse(recievedTime.Substring(8, 2));
                    int minuts = int.Parse(recievedTime.Substring(10, 2));
                    int seconds = int.Parse(recievedTime.Substring(12, 2));
                    dateTime = new DateTime(year, month, date, hours, minuts, seconds);
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
            return dateTime;
        }

        /// <summary>
        /// get Prana messages and clear the collection
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, List<PranaMessage>> GetAndClearPranaMessages()
        {
            List<PranaMessage> pranaMsgList = new List<PranaMessage>();
            Dictionary<string, List<PranaMessage>> _pranaMsgColClone = DeepCopyHelper.Clone(_pranaMsgCollection);
            Dictionary<string, List<PranaMessage>> stuckPranaMsgDict = new Dictionary<string, List<PranaMessage>>();
            try
            {
                lock (lockobj)
                {
                    //TODO clone the _PranaMsgCollection and return Dict
                    int waitTimeCache = CommonDataCache.CachedDataManager.GetInstance.WaitTimeToGetStuckTrade;
                    DateTime currentTime = DateTime.Now;
                    foreach (KeyValuePair<string, List<PranaMessage>> item in _pranaMsgColClone)
                    {
                        if (item.Value.Count > 0)
                        {
                            String recievedTimeStr = item.Value[0].FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ServerReceivedFullTime].Value;
                            DateTime recievedTime = GetDateTime(recievedTimeStr);
                            TimeSpan span = currentTime - recievedTime;
                          //  int tradeWaitTime = (int)span.Seconds; ::     Jira CI-6800
                            int tradeWaitTime = (int)span.TotalSeconds;
                            if (waitTimeCache <= tradeWaitTime)
                            {
                                stuckPranaMsgDict.Add(item.Key, item.Value);
                                _pranaMsgCollection.Remove(item.Key);
                            }
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
            return stuckPranaMsgDict;
        }

        private static ISecMasterServices _secMasterServices;
        public static ISecMasterServices SecMasterServices
        {
            set { _secMasterServices = value; }
        }
    }
}
