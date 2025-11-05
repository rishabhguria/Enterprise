using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BusinessLogic
{
    public class OrderInformation
    {

        /// <summary>
        /// To check if the current order has reached its end state
        /// </summary>
        /// <param name="incomingOrder"></param>
        /// <returns></returns>
        public static bool IsOrderInEndState(OrderSingle incomingOrder)
        {
            try
            {
                if (incomingOrder != null)
                    return
                           (incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_Cancelled) ||
                               incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_Expired) ||
                               incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_Filled) ||
                                incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_RollOver) ||
                                 incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_Rejected) ||
                                  incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_DoneForDay)
                               );
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
        /// To Handle the scenarios wherein we receive different tag 39 and 150 in case of Replace/Cancel
        /// </summary>
        /// <param name="pranaMessage"></param>
        /// <returns></returns>
        public static PranaMessage CreateReplaceCancelUpdateForMultiDayChild(PranaMessage pranaMessage)
        {

            try
            {
                PranaMessage updateFillForChildOrder;
                string tagMsgType = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value.ToString();
                if (tagMsgType == FIXConstants.MSGExecution)
                {
                    string tagExecType = string.Empty;
                    string tagOrdStaus = string.Empty;
                    if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecType))
                        tagExecType = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecType].Value.ToString();
                    if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrdStatus))
                        tagOrdStaus = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value.ToString();

                    if (tagExecType == FIXConstants.ORDSTATUS_Replaced || tagOrdStaus == FIXConstants.ORDSTATUS_Replaced)
                    {
                        updateFillForChildOrder = pranaMessage.Clone();
                        //Creating a new message and setting it to Partially filled type so that it routes to Child Orders
                        updateFillForChildOrder.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PartiallyFilled);
                        updateFillForChildOrder.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecType, FIXConstants.EXECTYPE_PartiallyFilled);
                        updateFillForChildOrder.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagOrigClOrdID);
                        //Setting the original Replace message as the perfect replace with tag 150 = 5 and 39 = 5
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_Replaced);
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecType, FIXConstants.EXECTYPE_Replaced);
                        return updateFillForChildOrder;
                    }
                    if (tagExecType == FIXConstants.ORDSTATUS_Cancelled || tagOrdStaus == FIXConstants.ORDSTATUS_Cancelled)
                    {
                        updateFillForChildOrder = pranaMessage.Clone();
                        //Creating a new message and setting it to Partially filled type
                        updateFillForChildOrder.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PartiallyFilled);
                        updateFillForChildOrder.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecType, FIXConstants.EXECTYPE_PartiallyFilled);
                        updateFillForChildOrder.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagOrigClOrdID);
                        //Setting the original Cancel message as the perfect Cancel with tag 150 = 3 and 39 = 3
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_Cancelled);
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecType, FIXConstants.ORDSTATUS_Cancelled);
                        return updateFillForChildOrder;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

       
        /// <summary>
        /// To check if the Message is MultiDay or Not
        /// </summary>
        /// <param name="pranaMessage"></param>
        /// <returns></returns>
        public static bool IsMultiDayOrder(PranaMessage pranaMessage)
        {
            try
            {
                string TagTimeInForce = string.Empty;
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTimeInForce))
                {
                    TagTimeInForce = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagTimeInForce].Value.ToString();
                    if (TagTimeInForce == FIXConstants.TIF_GTC || TagTimeInForce == FIXConstants.TIF_GTD)
                    {
                        return true;
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

        /// <summary>
        /// To check if the Order is MultiDay or Not
        /// </summary>
        /// <param name="PranaMessage"></param>
        /// <returns></returns>
        public static bool IsMultiDayOrder(OrderSingle incomingOrder)
        {
            try
            {
                if (incomingOrder != null && incomingOrder.TIF != null &&
                    (incomingOrder.TIF.Equals(FIXConstants.TIF_GTC) || incomingOrder.TIF.Equals(FIXConstants.TIF_GTD)))
                    return true;
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
        /// To check if the order had any Multi-Day History
        /// </summary>
        /// <param name="incomingOrder"></param>
        /// <returns></returns>
        public static bool IsMultiDayOrderHistory(OrderSingle incomingOrder)
        {
            try
            { 
                if(IsMultiDayOrder(incomingOrder))
                    return true;
                //This check has been added to enable the TIF Replace
                if ((incomingOrder.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub || incomingOrder.PranaMsgType == (int)OrderFields.PranaMsgTypes.MsgDropCopy_PM)
                    && (incomingOrder.OrderCollection != null && incomingOrder.OrderCollection.Count > 0))
                    return true;
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
        ///  to Check if it is Execution Msg or not
        /// </summary>
        /// <param name="pranaMessage"></param>
        /// <returns></returns>
        public static bool IsExecutionMsg(PranaMessage pranaMessage)
        {
            try
            {
                string tagMsgType = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value.ToString();
                string tagExecType = string.Empty;
                string tagOrdStaus = string.Empty;
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecType))
                    tagExecType = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecType].Value.ToString();
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrdStatus))
                    tagOrdStaus = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value.ToString();
                if (tagMsgType == FIXConstants.MSGExecution && (tagExecType == FIXConstants.EXECTYPE_Filled ||
                    tagExecType == FIXConstants.EXECTYPE_PartiallyFilled || tagOrdStaus == FIXConstants.ORDSTATUS_PartiallyFilled
                    || tagOrdStaus == FIXConstants.ORDSTATUS_Filled)) // In case we have any GTCexecution, then append AuecDate to OrderId 
                {
                    return true;
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


        /// <summary>
        /// Is the order  Multi-day and reached in End State
        /// </summary>
        /// <param name="incomingOrder"></param>
        /// <returns></returns>
        public static bool IsMultiDayOrderInEndState(OrderSingle incomingOrder)
        {
            try
            {
                if (incomingOrder != null)
                    return IsMultiDayOrder(incomingOrder) &&
                             IsOrderInEndState(incomingOrder);
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

        public static DateTime GetLastActivityTime(OrderSingle incomingOrder)
        {
            DateTime lastActivityTime = DateTime.MinValue;
            try
            {
                if (incomingOrder != null)
                {
                    if (!DateTime.TryParseExact(incomingOrder.ExecutionTimeLastFill.ToString(), DateTimeConstants.NirvanaDateTimeFormat, null, DateTimeStyles.None, out lastActivityTime))
                        lastActivityTime = incomingOrder.AUECLocalDate;
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
            return lastActivityTime;
        }
    }
}
