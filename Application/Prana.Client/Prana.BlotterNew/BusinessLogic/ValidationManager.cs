using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using System;
using System.Text;

namespace Prana.Blotter
{
    public class ValidationManager
    {
        #region ContextMenu SetUp Checks
        public static bool IsCompleteOrder(string orderStatus)
        {
            if (orderStatus == FIXConstants.ORDSTATUS_Cancelled ||
                orderStatus == FIXConstants.ORDSTATUS_RollOver ||
                orderStatus == FIXConstants.ORDSTATUS_DoneForDay ||
                orderStatus == FIXConstants.ORDSTATUS_Filled ||
                orderStatus == FIXConstants.ORDSTATUS_Expired ||
                orderStatus == FIXConstants.ORDSTATUS_Rejected ||
                orderStatus == CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsPending(string orderStatus)
        {
            if (orderStatus == FIXConstants.ORDSTATUS_PendingCancel ||
                orderStatus == FIXConstants.ORDSTATUS_PendingNew ||
                orderStatus == FIXConstants.ORDSTATUS_PendingReplace ||
                orderStatus == FIXConstants.ORDSTATUS_PendingRollOver)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ISOrderCancellable(OrderSingle order)
        {
            if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Expired ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Filled ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingReplace ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingCancel ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingRollOver ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_DoneForDay ||
                order.OrderStatusTagValue == string.Empty)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

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
        #endregion

        public static void GetSelectedOrderDetails(OrderSingle orderRequest)
        {
            try
            {
                ValidationManagerExtension.GetOrderDetails(orderRequest);
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

        public static string ValidateReplaceAlgoOrder(OrderSingle replacedOrder, OrderSingle parentCancelledOrder)
        {
            string result = string.Empty;
            if (replacedOrder.Quantity < parentCancelledOrder.CumQty)
            {
                result = "Quantity of Replaced Order less than Executed Quantity";
            }

            return result;
        }

        public static void SetAlgoReplaceOrderProperties(OrderSingle replaceAlgoOrder)
        {
            replaceAlgoOrder.LeavesQty = 0.0;
            replaceAlgoOrder.AvgPrice = 0;
            replaceAlgoOrder.LastPrice = 0.0;
            replaceAlgoOrder.LastShares = 0.0;
            replaceAlgoOrder.OrderID = string.Empty;

            replaceAlgoOrder.OrderSeqNumber = Int64.MinValue;
            replaceAlgoOrder.UnsentQty = 0.0;
        }

        public static string GetOrderText(OrderSingle or)
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append(TagDatabaseManager.GetInstance.GetOrderSideText(or.OrderSideTagValue));
            sBuilder.Append(" ");
            sBuilder.Append(or.Quantity);
            sBuilder.Append(" ");
            sBuilder.Append(or.Symbol);
            sBuilder.Append(" ");
            sBuilder.Append(TagDatabaseManager.GetInstance.GetOrderTypeText(or.OrderTypeTagValue));

            if (or.Price > 0.0)
            {
                sBuilder.Append(" @ ");
                sBuilder.Append(or.Price);
            }

            sBuilder.Append(" (");
            sBuilder.Append(or.CounterPartyName);
            sBuilder.Append(" ");
            sBuilder.Append(or.Venue);
            sBuilder.Append(")");

            return sBuilder.ToString();
        }
    }
}