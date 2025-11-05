using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.CommonDataCache;
namespace Prana.BasketTrading
{
    public class OrderTradingValidator
    {
        
        //static ICommunicationManager _communicationManager;

        /// <summary>
        /// Checks whether a Order is Valid or not 
        /// order.Text ="Reason"
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static bool IsOrderTradable(Order order)
        {

            try
            {
                order.Text = string.Empty;
                if (order.CounterPartyID == int.MinValue)
                {
                    order.Text = "Please Select CounterParty";
                    
                    return false;
                }
                else
                {
                    if (TradeManager.TradeManager.GetInstance().GetCounterPartyConnectionSatus(order.CounterPartyID) != PranaInternalConstants.ConnectionStatus.CONNECTED)
                    {
                        order.Text = "CounterParty Not Connected";
                        return false;
                    }
                }
                if (order.Symbol.Trim() == string.Empty)
                {
                    order.Text = "Please Select Valid Symbol";

                    return false;
                }
                if (order.VenueID == int.MinValue)
                {
                    order.Text = "Please Select Venue";
                    return false;
                }
                if (order.OrderSideTagValue == string.Empty)
                {
                    order.Text = "Please Select OrderSide";
                    return false;

                }
                if (order.OrderTypeTagValue == string.Empty)
                {
                    order.Text = "Please Enter Correct value of OrderType";
                    return false;
                }
                if (order.Price <= 0.0 && order.OrderTypeTagValue.Trim() == FIXConstants.ORDTYPE_Limit)//Market
                {
                    order.Text = "Please Enter Correct value of Price";
                    return false;

                }


                if (order.Quantity <= 0)
                {
                    order.Text = "Please Enter Correct value of Quantity";
                    return false;
                }
                if (order.UnsentQty <= 0)
                {
                    order.Text = "All Quantity is Already Traded or Sent for Trade";
                    return false;
                }
                if (order.HandlingInstruction == string.Empty || order.HandlingInstruction == ApplicationConstants.C_COMBO_SELECT)
                {
                    order.Text = "Select a Valid Handling Instruction";
                    return false;
                }
                if (order.ExecutionInstruction == string.Empty || order.ExecutionInstruction == ApplicationConstants.C_COMBO_SELECT)
                {
                    order.Text = "Select a Valid Execution Instruction";
                    return false;
                }
                if (order.TIF == string.Empty || order.TIF == ApplicationConstants.C_COMBO_SELECT)
                {
                    order.Text = "Select a Valid Time In Force";
                    return false;
                }
                //if (!CachedDataManager.GetInstance.CheckTradePermissionByCVandAUECID(order.AUECID, order.CounterPartyID, order.VenueID))
                //{
                //    order.Text = "Check if you have permission for the AUEC. \nAlso Check that the CounterParty Venue you want to trade has permissions to trade the same AUEC.";
                //    return false;
                //}
                if (order.OrderStatusTagValue !=string.Empty ) 
                {
                    order.Text = "Can't Trade Order in " + order.OrderStatus + " State";
                    return false;
                }

            }
            catch (Exception ex)
            {
                // error = ex.Message + " :" + order.BasketSequenceNumber;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return true;
        }

        
        public static bool ISOrderCancelable(Order order)
        {
            order.Text = string.Empty;
            if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Expired ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Filled ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver ||
                 order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew 
               )
            {                
                order.Text = "Can't Cancel Order in " + order.OrderStatus +" State";
                return false;
            }
            else if (order.OrderStatusTagValue == string.Empty)
            {
                order.Text = "Can't Cancel Order , has not been sent to Server";
                return false;
            }
            else
            {
                order.OrigClOrderID = order.ClOrderID;
                return true;
            }
        }

        public static bool ISOrderReplacable(Order order)
        {
            order.Text = string.Empty;
            if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Expired ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Filled ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled ||
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver ||                
                order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew ||
                  order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingReplace 
                )
            {

                order.Text = "Can't Replace Order in " + order.OrderStatus + " State";
                return false;
            }
            else if (order.OrderStatusTagValue == string.Empty)
            {
                order.Text = "Can't Replace Order , has not been sent to Server";
                return false;
            }
            else
            {
                if (order.Parent != null)
                {
                    if (order.Parent.SubOrderQty > order.Parent.Quantity)
                    {
                        order.Text = "Sub Order Qty Can't be Incremented Beyond Parent Order Qty";
                        return false;
                    }
                }
 
                if (order.Quantity >= order.CumQty)
                {
                    order.OrigClOrderID = order.ClOrderID;
                    return true;
                }
                else
                {
                    order.Text = "Can't Replace Order Request,  Replace Quantity " + order.Quantity + " is less than Executed Qty  " + order.CumQty;
                    return false;
                }

            }
        }

        public static OrderCollection GetCancelableOrders(OrderCollection selectedOrders)
        {
            OrderCollection canclableOrders = new OrderCollection();
            foreach (Order order in selectedOrders)
            {
                if (order.SubOrders.Count > 0)
                {
                    foreach (Order subOrder in order.SubOrders)
                    {
                        if (ISOrderCancelable(subOrder))
                        {
                            subOrder.OrderTradingStatus = TradingStatus.Valid;
                            canclableOrders.Add(subOrder);
                        }
                        else
                        {
                            subOrder.OrderTradingStatus = TradingStatus.InValid;
                        }
                    }
                }
                else
                {
                    if (ISOrderCancelable(order))
                    {
                        order.OrderTradingStatus = TradingStatus.Valid;
                        canclableOrders.Add(order);
                    }
                    else
                    {
                        order.OrderTradingStatus = TradingStatus.InValid;
                    }
                }

                
            }
            return canclableOrders;
           
        }

        public static OrderCollection GetReplacableOrders(OrderCollection selectedOrders)
        {
            OrderCollection replacableOrders = new OrderCollection();
            foreach (Order order in selectedOrders)
            {
                if (ISOrderReplacable(order))
                {
                    order.OrderTradingStatus = TradingStatus.Valid;
                    replacableOrders.Add(order);
                }
                else
                {
                    order.OrderTradingStatus = TradingStatus.InValid;
                }
            }
            return replacableOrders;
        }
        
        /// <summary>
        /// It valdates whether the Order is Valid For Trading or Not 
        /// 
        ///  if Not Validated, order.OrderTradingStatus = TradingStatus.InValid
        /// it returns Validated Order Collection
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public static OrderCollection  GetTradableOrders(OrderCollection orders)
        {
            OrderCollection tradableOrders = new OrderCollection();
            try
            {

                foreach (Order order in orders)
                {
                    if (order.SubOrders.Count > 0)
                    {
                        foreach (Order subOrder in order.SubOrders)
                        {
                            if (IsOrderTradable(subOrder) && order.AUECID != int.MinValue)
                            {
                                subOrder.OrderTradingStatus = TradingStatus.Valid;
                                tradableOrders.Add(subOrder);
                            }
                            else
                            {
                                subOrder.Text = string.Empty; // No Need to show in case of Sub Orders
                                subOrder.OrderTradingStatus = TradingStatus.InValid;
                            }
                        }
                    }
                    else
                    {
                        if (IsOrderTradable(order) && order.AUECID !=int.MinValue )
                        {
                            order.OrderTradingStatus = TradingStatus.Valid;
                            tradableOrders.Add(order);
                        }
                        else
                        {
                            order.OrderTradingStatus = TradingStatus.InValid;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }
            return tradableOrders;
        }
       
        //public static ICommunicationManager CommunicationManager
        //{
        //    set { _communicationManager = value; }
        //}

        /// <summary>
        /// Get the Orders that can be Grouped
        /// </summary>
        /// <returns></returns>
        public static OrderCollection GetGroupableOrders(OrderCollection orders)
        {

            OrderCollection groupableOrders = new OrderCollection();
            foreach (Order order in orders)
            {
                if (order.CanCreateSubOrders())
                {
                    groupableOrders.Add(order);
                }
                else
                {
                    order.Text = "Can't Create SubOrder";
                }
            }

            return groupableOrders;

        }
       
    }
}
