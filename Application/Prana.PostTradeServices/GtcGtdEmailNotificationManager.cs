using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Prana.PostTradeServices
{
    public class GtcGtdEmailNotificationManager
    {
        public const string CONST_JOB_NAME = "GtcGtdEmailJob";
        private const string CONST_JOB_GROUP = "GtcGtdEmailGroup";
        private const string CONST_EMAIL_TRIGGER_NAME = "GtcGtdEmailTrigger";
        private const string CONST_EMAIL_TRIGGER_GROUP = "GtcGtdEmailTriggerGroup";

        private const string CONST_GTCGTD_EMAIL_SUBJECT = "Active GTC/GTD trades on the Nirvana Trade Blotter";
        private const string CONST_GTCGTD_MAIL_SENDER = "GTCGTDMailSenderAddress";
        private const string CONST_GTCGTD_MAIL_SENDERS_NAME = "GTCGTDMailSenderName";
        private const string CONST_GTCGTD_MAIL_PASSWORD = "GTCGTDMailPassword";
        private const string CONST_GTCGTD_MAIL_PORT = "GTCGTDMailPort";
        private const string CONST_GTCGTD_ENABLE_SSL = "GTCGTDEnableSSL";
        private const string CONST_GTCGTD_MAIL_HOST_NAME = "GTCGTDMailHostName";
        private const string CONST_GTCGTD_AUTHENTICATION_REQUIRED = "GTCGTDAuthenticationRequired";

        /// <summary>
        /// It stores the orders based on the Parent ClOrder Id.
        /// </summary>
        private Dictionary<string, OrderSingle> _dictParentClOrderIDCollection = new Dictionary<string, OrderSingle>();

        /// <summary>
        /// The sched
        /// </summary>
        private IScheduler _scheduler;
        /// <summary>
        /// The sched fact
        /// </summary>
        private ISchedulerFactory _schedulerFactory = new StdSchedulerFactory();
        /// <summary>
        /// Shutdowns the scheduler.
        /// </summary>
        private void ShutdownScheduler()
        {
            try
            {
                _scheduler.Standby();
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
        /// Starts the scheduler.
        /// </summary>
        private void StartScheduler()
        {
            try
            {
                _scheduler = _schedulerFactory.GetScheduler();
                _scheduler.Start();
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
        /// Schedules a job to send Email notifications.
        /// </summary>
        public void ScheduleEmailNotification()
        {
            try
            {
                DateTime dtEmailNotify = GtcGtdDatabaseManager.GetInstance().GetEmailNotifyTime();
                if (!dtEmailNotify.Equals(DateTime.MinValue))
                {
                    ShutdownScheduler();
                    StartScheduler();

                    JobDetail jobdetail = new JobDetail(CONST_JOB_NAME, CONST_JOB_GROUP, typeof(GtcGtdEmailNotifcationJob));
                    TimeSpan timespan = new TimeSpan(864000000000); // for whole 24day set it to 864000000000
                    SimpleTrigger simpleTrigger = new SimpleTrigger();
                    simpleTrigger.RepeatCount = SimpleTrigger.RepeatIndefinitely;
                    simpleTrigger.RepeatInterval = timespan;
                    simpleTrigger.StartTimeUtc = dtEmailNotify.ToUniversalTime();
                    simpleTrigger.Name = CONST_EMAIL_TRIGGER_NAME;
                    simpleTrigger.Group = CONST_EMAIL_TRIGGER_GROUP;
                    _scheduler.DeleteJob(CONST_JOB_NAME, CONST_JOB_GROUP);
                    _scheduler.ScheduleJob(jobdetail, simpleTrigger);
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
        /// Constructs email body that needs to be send.
        /// </summary>
        /// <param name="orderCollection"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private string ConstructEmailBody(List<OrderSingle> orderCollection, string username)
        {
            StringBuilder emailBody = new StringBuilder();
            try
            {
                emailBody.AppendLine("<html>");
                emailBody.AppendFormat("<body>Hello {0} <br>", username);
                emailBody.AppendLine("The following trades are active on Nirvana’s Trade Blotter currently. </br>");
                emailBody.AppendLine("<table style= \"border: 1px solid black;border-collapse: collapse;\"> ");

                emailBody.AppendLine("<tr>");
                emailBody.AppendLine("<th style= \"border: 1px solid black;\"> Symbol </th>");
                emailBody.AppendLine("<th style= \"border: 1px solid black;\"> Asset Class </th>");
                emailBody.AppendLine("<th style= \"border: 1px solid black;\"> Order Side </th>");
                emailBody.AppendLine("<th style= \"border: 1px solid black;\"> Original Order </th>");
                emailBody.AppendLine("<th style= \"border: 1px solid black;\"> Executed </th>");
                emailBody.AppendLine("<th style= \"border: 1px solid black;\"> TIF/Validity </th>");
                emailBody.AppendLine("</tr>");

                foreach(OrderSingle order in orderCollection)
                {
                    emailBody.AppendLine("<tr>");
                    emailBody.AppendFormat("<td style= \"border: 1px solid black;\"> {0} </td>", order.Symbol);
                    emailBody.AppendFormat("<td style= \"border: 1px solid black;\"> {0} </td>", CachedDataManager.GetInstance.GetAssetText(order.AssetID));
                    emailBody.AppendFormat("<td style= \"border: 1px solid black;\"> {0} </td>", TagDatabaseManager.GetInstance.GetOrderSideText(order.OrderSideTagValue));
                    emailBody.AppendFormat("<td style= \"border: 1px solid black;\"> {0:N0} </td>", order.Quantity);
                    emailBody.AppendFormat("<td style= \"border: 1px solid black;\"> {0:N0} </td>", order.CumQty);
                    if (order.TIF.Equals(FIXConstants.TIF_GTD))
                    {
                        DateTime expireTime = DateTime.Parse(order.ExpireTime);
                        emailBody.AppendFormat("<td style= \"border: 1px solid black;\"> {0} </td>", expireTime.ToString("MM/dd/yyyy"));
                    }
                    else
                        emailBody.AppendLine("<td style= \"border: 1px solid black;\"> Till Cancellation </td>");
                    emailBody.AppendLine("</tr>");
                }
                emailBody.AppendLine("</table></body></html>");
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
            return emailBody.ToString();
        }

        /// <summary>
        /// Sends email notifcations for active GTC/GTD orders.
        /// </summary>
        public void SendActiveGtcGtdOrdersEmail()
        {
            try
            {
                Dictionary<int, string> companyUserEmailIds = GtcGtdDatabaseManager.GetInstance().GetCompanyUserEmailIds();
                List<OrderSingle> subOrderCollection = GetBlotterLaunchData();
                foreach (KeyValuePair<int, string> kvpUserMail in companyUserEmailIds)
                {
                    int userId = kvpUserMail.Key;
                    List<OrderSingle> subOrderCollectionUser = subOrderCollection.Where(x => x.CompanyUserID.Equals(userId)).ToList();
                    if (subOrderCollectionUser!=null && subOrderCollectionUser.Count > 0)
                    {
                        List<OrderSingle> activeOrderCollection = GetActiveGtcGtdOrders(subOrderCollectionUser);
                        if (activeOrderCollection.Count > 0)
                        {
                            string receiverEmailAddress = kvpUserMail.Value;
                            string emailBody = ConstructEmailBody(activeOrderCollection, CachedDataManager.GetInstance.GetUserText(userId));
                            string emailSubject = CONST_GTCGTD_EMAIL_SUBJECT;
                            string[] mailRecipients = kvpUserMail.Value.Split(',');
                            string mailSender = ConfigurationManager.AppSettings[CONST_GTCGTD_MAIL_SENDER];
                            string mailSenderName = ConfigurationManager.AppSettings[CONST_GTCGTD_MAIL_SENDERS_NAME];
                            string mailerPassword = ConfigurationManager.AppSettings[CONST_GTCGTD_MAIL_PASSWORD];
                            int mailPort = int.Parse(ConfigurationManager.AppSettings[CONST_GTCGTD_MAIL_PORT]);
                            bool enableSSL = bool.Parse(ConfigurationManager.AppSettings[CONST_GTCGTD_ENABLE_SSL]);
                            string mailHost = ConfigurationManager.AppSettings[CONST_GTCGTD_MAIL_HOST_NAME];
                            bool authenticationRequired = bool.Parse(ConfigurationManager.AppSettings[CONST_GTCGTD_AUTHENTICATION_REQUIRED]);

                            EmailsHelper.MailSend(emailSubject, emailBody, mailSender, mailSenderName, mailerPassword, mailRecipients, mailPort, mailHost, enableSSL, authenticationRequired, true);
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
        }

        /// <summary>
        /// Gets only active GTC/GTD orders.
        /// </summary>
        /// <param name="orderCollection"></param>
        /// <returns></returns>
        private List<OrderSingle> GetActiveGtcGtdOrders(List<OrderSingle> orderCollection)
        {
            Dictionary<string, OrderSingle> dictActiveOrderCollection = new Dictionary<string, OrderSingle>();
            try
            {
                foreach (OrderSingle order in orderCollection)
                {
                    if (_dictParentClOrderIDCollection.ContainsKey(order.StagedOrderID) && order.MsgType != FIXConstants.MSGOrderRollOverRequest && order.PranaMsgType != (int)Global.OrderFields.PranaMsgTypes.ORDNewSubChild)
                    {
                        bool isMultiDayEndState = OrderInformation.IsMultiDayOrderInEndState(order);
                        if (order.TIF.Equals(FIXConstants.TIF_GTC))
                        {
                            if (!isMultiDayEndState)
                            {
                                if (!dictActiveOrderCollection.ContainsKey(order.ClOrderID))
                                    dictActiveOrderCollection.Add(order.ClOrderID, order);
                            }
                        }
                        else if (order.TIF.Equals(FIXConstants.TIF_GTD))
                        {
                            DateTime expireTime = DateTime.MinValue;
                            bool isDateValid = DateTime.TryParse(order.ExpireTime,out expireTime);
                            if (isDateValid && expireTime.Date >= DateTime.UtcNow.Date && !isMultiDayEndState)
                            {
                                if (!dictActiveOrderCollection.ContainsKey(order.ClOrderID))
                                    dictActiveOrderCollection.Add(order.ClOrderID, order);
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
            return dictActiveOrderCollection.Values.ToList();
        }

        /// <summary>
        /// Gets Blotter data from database and updates sub order collection.
        /// </summary>
        /// <returns></returns>
        public List<OrderSingle> GetBlotterLaunchData()
        {
            List<OrderSingle> subOrderCollection = new List<OrderSingle>();
            try
            {
                List<OrderSingle> orderCollection = DataBaseManager.GetBlotterLaunchData(true);
                _dictParentClOrderIDCollection.Clear();
                foreach(OrderSingle order in orderCollection)
                {
                    if (_dictParentClOrderIDCollection.ContainsKey(order.ParentClOrderID))
                    {
                        OrderSingle existingOrder = _dictParentClOrderIDCollection[order.ParentClOrderID];
                        if (existingOrder.OrderSeqNumber > order.OrderSeqNumber && order.OrderSeqNumber != long.MinValue || ((OrderInformation.IsMultiDayOrderHistory(existingOrder) || OrderInformation.IsMultiDayOrderHistory(order)) && order.MsgType == FIXConstants.MSGOrderCancelReject))
                            continue;

                        if (existingOrder.OrderCollection == null)
                            existingOrder.OrderCollection = new OrderBindingList();
                        existingOrder.OrderCollection.Add(order);
                        UpdateRequiredFields(existingOrder, order);
                        UpdateCumulativeQuantityFromChildCollection(existingOrder);
                        existingOrder.OrderStatusTagValue = order.OrderStatusTagValue;
                        if (order.MsgType == FIXConstants.MSGExecutionReport || order.MsgType == FIXConstants.MSGOrderCancelReject || order.MsgType == FIXConstants.MSGOrderCancelReplaceRequest)
                        {
                            existingOrder.OrderSeqNumber = order.OrderSeqNumber;
                            existingOrder.TIF = order.TIF;
                        }

                        if (order.StagedOrderID != string.Empty && order.StagedOrderID != int.MinValue.ToString() && _dictParentClOrderIDCollection.ContainsKey(order.StagedOrderID))
                        {
                            if (_dictParentClOrderIDCollection.ContainsKey(order.StagedOrderID))
                            {
                                OrderSingle parentOrder = _dictParentClOrderIDCollection[order.StagedOrderID];
                                double parentOrderPrevQty = parentOrder.CumQty;
                                UpdateRequiredFields(parentOrder, order);
                                UpdateCumulativeQuantityFromChildCollection(parentOrder);
                                UpdateParentStatus(order, parentOrder);

                                if (parentOrder.StagedOrderID != string.Empty && parentOrder.StagedOrderID != int.MinValue.ToString() && _dictParentClOrderIDCollection.ContainsKey(parentOrder.StagedOrderID))
                                {
                                    OrderSingle grandParentOrder = _dictParentClOrderIDCollection[parentOrder.StagedOrderID];
                                    UpdateParentCumulativeQuantityFromSub(parentOrder, grandParentOrder, parentOrderPrevQty);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (order.PranaMsgType != (int)Global.OrderFields.PranaMsgTypes.ORDStaged)
                        {
                            if (subOrderCollection.Where(x => x.ParentClOrderID.Equals(order.StagedOrderID.ToString())).Count() == 0)
                            {
                                subOrderCollection.Add(order);
                            }
                            if ((order.StagedOrderID != string.Empty) && (order.StagedOrderID != order.ClOrderID))
                            {
                                if (_dictParentClOrderIDCollection.ContainsKey(order.StagedOrderID))
                                {
                                    OrderSingle parentOrder = _dictParentClOrderIDCollection[order.StagedOrderID];
                                    if (parentOrder.OrderCollection == null)
                                        parentOrder.OrderCollection = new OrderBindingList();
                                    parentOrder.OrderCollection.Add(order);
                                    UpdateRequiredFields(parentOrder, order);
                                    UpdateCumulativeQuantityFromChildCollection(parentOrder);
                                    UpdateParentStatus(order,parentOrder);
                                    if(parentOrder.PranaMsgType != (int)Global.OrderFields.PranaMsgTypes.ORDStaged)
                                        order.CompanyUserID = parentOrder.CompanyUserID;

                                    double parentOrderPrevQty = parentOrder.CumQty;

                                    if (parentOrder.StagedOrderID != string.Empty && parentOrder.StagedOrderID != int.MinValue.ToString() && _dictParentClOrderIDCollection.ContainsKey(parentOrder.StagedOrderID))
                                    {
                                        OrderSingle grandParentOrder = _dictParentClOrderIDCollection[parentOrder.StagedOrderID];
                                        UpdateParentCumulativeQuantityFromSub(parentOrder, grandParentOrder, parentOrderPrevQty);
                                    }
                                }
                            }
                        }
                        _dictParentClOrderIDCollection.Add(order.ParentClOrderID, order);
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
            return subOrderCollection;
        }

        /// <summary>
        /// Update required fields
        /// </summary>
        /// <param name="existingOrder"></param>
        /// <param name="order"></param>
        private void UpdateRequiredFields(OrderSingle existingOrder, OrderSingle order)
        {
            try
            {
                existingOrder.Quantity = order.Quantity;
                existingOrder.ExpireTime = order.ExpireTime;
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
        /// Updates the Cumulative Quantity and order status of grand parent order.
        /// </summary>
        /// <param name="parentOrder"></param>
        /// <param name="grandParentOrder"></param>
        /// <param name="parentOrderPrevQty"></param>
        private void UpdateParentCumulativeQuantityFromSub(OrderSingle parentOrder, OrderSingle grandParentOrder, double parentOrderPrevQty)
        {
            try
            {
                if (OrderInformation.IsMultiDayOrderHistory(parentOrder) &&
                    (parentOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_Expired) || parentOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_DoneForDay)))
                {
                    UpdateCumulativeQuantityFromChildCollection(grandParentOrder);
                }
                else
                {
                    if (parentOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingNew && parentOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_New && parentOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected && parentOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Rejected)
                    {
                        grandParentOrder.CumQty = grandParentOrder.CumQty + parentOrder.CumQty - parentOrderPrevQty;
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
        /// Updates the Cumulative Quantity.
        /// </summary>
        /// <param name="parentOrder"></param>
        private void UpdateCumulativeQuantityFromChildCollection(OrderSingle parentOrder)
        {
            try
            {
                double cumQty = 0;
                foreach (OrderSingle subOrder in parentOrder.OrderCollection)
                {
                    if (subOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected && subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Rejected)
                        cumQty += subOrder.CumQty;
                }
                parentOrder.CumQty = cumQty;
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
        /// Update Order status of Parent order.
        /// This method has been taken from the Client solution.
        /// </summary>
        /// <param name="orderResponse"></param>
        /// <param name="parentOrder"></param>
        private void UpdateParentStatus(OrderSingle orderResponse, OrderSingle parentOrder)
        {
            try
            {
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
                    //Parent order status must be updated if order is replaced 
                    if (parentOrder.CumQty > 0 && parentOrder.CumQty < parentOrder.Quantity
                        && orderResponse.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged
                        && orderResponse.OrderStatusTagValue != FIXConstants.ORDSTATUS_Replaced && orderResponse.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingReplace)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;

                    }
                    // case: parent is replaced to qty = cumqty.
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

        #region Singleton
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static GtcGtdEmailNotificationManager GetInstance()
        {
            lock (_lock)
            {
                if (_gtcGtdEmailNotificationManager == null)
                    _gtcGtdEmailNotificationManager = new GtcGtdEmailNotificationManager();
                return _gtcGtdEmailNotificationManager;
            }
        }
        private GtcGtdEmailNotificationManager()
        {
        }
        private static GtcGtdEmailNotificationManager _gtcGtdEmailNotificationManager = null;
        readonly static object _lock = new object();
        #endregion
    }
}
