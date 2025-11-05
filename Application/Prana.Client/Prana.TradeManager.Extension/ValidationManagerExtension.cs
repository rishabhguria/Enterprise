using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.TradeManager.Extension.CacheStore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.TradeManager.Extension
{
    public class ValidationManagerExtension
    {
        /// <summary>
        /// Checks if order can be rolled over.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static bool ISOrderRolloverable(OrderSingle existingOrder)
        {
            if (existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_New ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingReplace ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Replaced ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingCancel ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PartiallyFilled ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Suspended)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// GetOrderDetails
        /// </summary>
        /// <param name="orderRequest"></param>
        public static void GetOrderDetails(OrderSingle orderRequest)
        {
            try
            {
                switch (orderRequest.MsgType)
                {
                    case FIXConstants.MSGOrderCancelReplaceRequest:
                        GetCommonOrderDetails(orderRequest);

                        if (orderRequest.PranaMsgType != (int)Global.OrderFields.PranaMsgTypes.ORDStaged ||
                            orderRequest.PranaMsgType != (int)Global.OrderFields.PranaMsgTypes.ORDManual ||
                            orderRequest.PranaMsgType != (int)Global.OrderFields.PranaMsgTypes.ORDManualSub)
                        {
                            orderRequest.OrigClOrderID = orderRequest.ClOrderID;
                            // the following is to ensure same seq number is not sent for replace fill
                            orderRequest.OrderSeqNumber = int.MinValue;
                        }
                        else
                        {
                            if (orderRequest.OrigClOrderID == string.Empty)
                            {
                                orderRequest.OrigClOrderID = orderRequest.ClOrderID;
                            }
                        }

                        orderRequest.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingReplace;
                        orderRequest.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderRequest.OrderStatusTagValue.ToString());
                        break;

                    case FIXConstants.MSGOrderCancelRequest:
                        orderRequest.OrigClOrderID = orderRequest.ClOrderID;
                        orderRequest.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingCancel;
                        orderRequest.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderRequest.OrderStatusTagValue.ToString());
                        break;

                    case FIXConstants.MSGOrderRollOverRequest:
                        if (orderRequest.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingCancel || orderRequest.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingReplace)
                        {
                            orderRequest.OrigClOrderID = orderRequest.OrigClOrderID;
                        }
                        else
                        {
                            orderRequest.OrigClOrderID = orderRequest.ClOrderID;
                        }
                        orderRequest.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingRollOver;
                        orderRequest.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderRequest.OrderStatusTagValue.ToString());
                        break;

                    case FIXConstants.MSGOrder:
                        orderRequest.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingNew;
                        break;

                    case FIXConstants.MSGOrderStatusRequest:
                        break;

                    case "ManualFills":
                        orderRequest.OrigClOrderID = orderRequest.ClOrderID;
                        break;

                    case FIXConstants.MSGTransferUser:
                        GetCommonOrderDetails(orderRequest);
                        orderRequest.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingNew;
                        break;
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
        /// GetCommonOrderDetails
        /// </summary>
        /// <param name="_order"></param>
        public static void GetCommonOrderDetails(OrderSingle _order)
        {
            try
            {
                _order.OrderSideTagValue = TagDatabaseManager.GetInstance.GetOrderSideValue(_order.OrderSide.ToString());
                _order.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValue(_order.OrderType.ToString());//FIXConstants.ORDTYPE_Limit;
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
        /// Gets the selected ClorderId(s)
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedOrderClOrderIDs(OrderSingle selectedOrder)
        {
            try
            {
                List<string> parentClOrderIds = new List<string>();
                bool isAllOrdersCumQtyZero = true;

                if (selectedOrder.OrderCollection != null)
                {
                    //Fetching the latest ParentCLOrderId of childs of GTC order to get correct allocation detail
                    if(selectedOrder.TIF == FIXConstants.TIF_GTC || selectedOrder.TIF == FIXConstants.TIF_GTD)
                    {
                        long latestCLOrderId = 0;
                        string latestClOrderID = string.Empty;
                        foreach (OrderSingle subOrder in selectedOrder.OrderCollection)
                        {
                            if(latestCLOrderId < long.Parse(subOrder.ParentClOrderID))
                            {
                                latestClOrderID = subOrder.ParentClOrderID; 
                            }
                        }
                        parentClOrderIds.Add(latestClOrderID);
                        isAllOrdersCumQtyZero = false;
                    }
                    else
                    {
                        foreach (OrderSingle subOrder in selectedOrder.OrderCollection)
                        {
                            if (subOrder.CumQty > 0)
                                isAllOrdersCumQtyZero = false;

                            if (!parentClOrderIds.Contains(subOrder.ParentClOrderID))
                                parentClOrderIds.Add(subOrder.ParentClOrderID);
                        }
                    }
                }
                else
                {
                    parentClOrderIds.Add(selectedOrder.ParentClOrderID);
                    isAllOrdersCumQtyZero = false;
                }

                if (!isAllOrdersCumQtyZero)
                    return string.Join(",", parentClOrderIds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns the message required for confirmation popups
        /// </summary>
        /// <returns></returns>
        public static string GetOrderText(OrderSingle or, bool isSamsaraUser = false)
        {
            StringBuilder sBuilder = new StringBuilder();
            if (or.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrder)
            {
                OrderSingle cancelOrder = BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection[or.AlgoSyntheticRPLParent];

                //The quantity left after the subtraction of the filled quantity from replaced quantity.
                double calculatedQty = or.Quantity - cancelOrder.CumQty;
                or.LeavesQty = calculatedQty;
                sBuilder.Append(TagDatabaseManager.GetInstance.GetOrderSideText(or.OrderSideTagValue));
                sBuilder.Append(" ");
                sBuilder.Append(calculatedQty.ToString("#,##,###0.##"));
                sBuilder.Append(" ");
                sBuilder.Append(or.Symbol);
                sBuilder.Append(" ");
                sBuilder.Append(TagDatabaseManager.GetInstance.GetOrderTypeText(or.OrderTypeTagValue));
            }

            else
            {
                sBuilder.Append(TagDatabaseManager.GetInstance.GetOrderSideText(or.OrderSideTagValue));
                sBuilder.Append(" ");
                sBuilder.Append(or.Quantity.ToString("#,##,###0.##"));
                sBuilder.Append(" ");
                sBuilder.Append(or.Symbol);
                sBuilder.Append(" ");
                sBuilder.Append(TagDatabaseManager.GetInstance.GetOrderTypeText(or.OrderTypeTagValue));
            }
            if (or.Price > 0.0)
            {
                sBuilder.Append(" at ");
                sBuilder.Append(or.Price.ToString("#,##,###0.##"));
            }
            sBuilder.Append(" from ");
            sBuilder.Append(or.CounterPartyName);
            sBuilder.Append(" ");
            sBuilder.Append(or.Venue);

            if (or.Price > 0.0)
            {
                if(or.TIF.Equals(FIXConstants.TIF_GTD) && isSamsaraUser)
                    sBuilder.Append(" (Notional: ");
                else
                    sBuilder.Append(" : Notional Value ");

                sBuilder.Append((or.Price * or.Quantity).ToString("#,##,###0.##"));

                if (or.TIF.Equals(FIXConstants.TIF_GTD) && isSamsaraUser)
                    sBuilder.Append(")");
            }
            if (or.TIF.Equals(FIXConstants.TIF_GTD) && !string.IsNullOrEmpty(or.ExpireTime))
            {
                sBuilder.Append(Environment.NewLine);
                if (isSamsaraUser)
                {
                    string expireTimeFormatted = DateTime.Parse(or.ExpireTime).ToString("M/d/yyyy h:mm tt");
                    sBuilder.Append("GTD order valid till " + expireTimeFormatted + ", cancels at " + CachedDataManager.GetInstance.GetExchangeText(or.ExchangeID) + " close.");
                }
                else
                    sBuilder.Append("Your GTD order will be valid till ‘" + or.ExpireTime + " and will automatically be cancelled at the end of trading hours at the " + CommonDataCache.CachedDataManager.GetInstance.GetExchangeText(or.ExchangeID) + "’.");
                
                sBuilder.Append(" Do you wish to continue?");
            }
            if (or.TIF.Equals(FIXConstants.TIF_GTC))
            {
                sBuilder.Append(Environment.NewLine);
                if(isSamsaraUser)
                    sBuilder.Append("Your GTC order will be valid until you cancel the trade. Do you wish to continue?");
                else
                    sBuilder.Append("Your GTC order will be valid until YOU CANCEL the trade. Do you wish to continue?");
            }
            string msg = sBuilder.ToString();

            if (or.PranaMsgType == (int)Global.OrderFields.PranaMsgTypes.ORDManual)
            {
                msg = "Manual :" + msg;
            }

            return msg;
        }
    }
}
