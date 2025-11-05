using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;

namespace Prana.TradeManager.Extension
{
    public static class ManualFillsHelper
    {
        /// <summary>
        /// Fills order details.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="fill"></param>
        /// <param name="sum"></param>
        /// <param name="amount"></param>
        /// <param name="avgPrice"></param>
        public static void FillDetails(OrderSingle order, OrderSingle fill, ref double sum, ref double amount, ref double avgPrice)
        {
            try
            {
                if (fill.ExecID == string.Empty)
                {
                    fill.ExecID = System.Guid.NewGuid().ToString();
                    fill.TransactionTime = DateTime.Now.ToUniversalTime();
                    fill.ExecutionTimeLastFill = fill.TransactionTime.ToString(DateTimeConstants.NirvanaDateTimeFormat);
                }
                if (fill.LastShares != double.Epsilon && fill.LastShares != double.MinValue)
                {
                    sum += Convert.ToDouble(fill.LastShares);
                }
                else// else clause added for if all fills are deleted and saved
                {
                    fill.LastShares = 0.0;
                }
                if (fill.LastPrice != double.Epsilon && fill.LastPrice != double.MinValue && fill.LastShares != double.Epsilon)
                {
                    fill.LastPrice = fill.LastPrice;
                    amount += Convert.ToDouble(fill.LastPrice) * Convert.ToDouble(fill.LastShares);
                }
                else // else clause added for if all fills are deleted and saved
                {
                    fill.LastPrice = 0.0;
                }

                if (sum > 0)
                {
                    avgPrice = amount / sum;
                }
                fill.AvgPrice = Convert.ToDouble(avgPrice);
                fill.CumQty = sum;
                // https://jira.nirvanasolutions.com:8443/browse/PRANA-39017
                fill.ClOrderID = order.ClOrderID;
                fill.Quantity = order.Quantity;
                fill.Price = order.Price;
                fill.LeavesQty = fill.Quantity - fill.CumQty;
                fill.Symbol = order.Symbol;
                //msg type execution report needed so that server can do appropriate handling
                fill.MsgType = FIXConstants.MSGExecutionReport;
                fill.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDManual;
                fill.OrderSideTagValue = order.OrderSideTagValue;
                fill.OrderTypeTagValue = order.OrderTypeTagValue;
                fill.TradingAccountID = order.TradingAccountID;
                fill.OrderID = order.OrderID;
                fill.AssetID = order.AssetID;
                fill.UnderlyingID = order.UnderlyingID;
                fill.ExchangeID = order.ExchangeID;
                fill.CompanyUserID = order.CompanyUserID;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the order status.
        /// </summary>
        /// <param name="fill"></param>
        /// <returns></returns>
        public static string SetOrderStatus(OrderSingle fill)
        {
            string errorMessage = string.Empty;
            try
            {
                if ((Convert.ToInt32(fill.Quantity) - Convert.ToInt32(fill.CumQty)) == 0)
                {
                    fill.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                }
                else if ((Convert.ToInt32(fill.Quantity) - Convert.ToInt32(fill.CumQty)) == Convert.ToInt32(fill.Quantity))
                {
                    fill.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;
                }
                else if (((Convert.ToInt32(fill.Quantity) - Convert.ToInt32(fill.CumQty)) > 0) && ((Convert.ToInt32(fill.Quantity) - Convert.ToInt32(fill.CumQty)) < Convert.ToInt32(fill.Quantity)))
                {
                    fill.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;
                }
                else
                {
                    errorMessage = "Executed quantity exceeds target Quantity!";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }

        /// <summary>
        /// Create a new fill.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="lastShares"></param>
        /// <param name="lastPrice"></param>
        /// <returns></returns>
        public static OrderSingle CreateNewFill(OrderSingle order, double lastShares, double lastPrice)
        {
            OrderSingle _order = new OrderSingle();
            try
            {
                _order.LastPrice = order.Price;
                DateTime dt = DateTime.Now.ToUniversalTime();
                _order.TransactionTime = BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dt, CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));
                DateTime orderDate = _order.TransactionTime.Date;
                _order.TransactionTime = orderDate.AddHours(12);
                _order.LastShares = lastShares;
                _order.LastPrice = lastPrice;// order.Price?
                _order.ClOrderID = order.ClOrderID;
                _order.OrderStatusTagValue = order.OrderStatusTagValue;
                _order.FXRate = order.FXRate;
                _order.FXConversionMethodOperator = order.FXConversionMethodOperator;
                _order.SettlementCurrencyID = order.SettlementCurrencyID;
                _order.NotionalValue = _order.LastShares * _order.LastPrice;
                //TODO:_order.NotionalValueBase
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return _order;
        }
    }
}
