using Prana.BusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using System;
using System.Text;
using System.Windows.Forms;

namespace Prana.TradeManager
{
    public class ValidationManager
    {
        public static ConfirmationPopUp _confirmationPopUpPref = TradingTktPrefs.ConfirmationPopUpPrefs;

        public static bool IsTradeMergedRemovedEdited = false;

        public static string GetManualOrderText(OrderSingle or)
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("Confirm below ");
            sBuilder.Append(or.Symbol);
            sBuilder.Append(" fills for this manual order with broker ");
            sBuilder.Append(or.CounterPartyName);
            sBuilder.Append(".");
            return sBuilder.ToString();
        }

        private static bool ValidatePopUpSettings(OrderSingle or)
        {
            if ((or.MsgType == FIXConstants.MSGOrder) && (_confirmationPopUpPref.ISNewOrder))
            {
                if (!or.IsManualOrder)
                {
                    PromptWindow promptWin = new PromptWindow(ValidationManagerExtension.GetOrderText(or), "New Order Prompt");
                    if (or.TIF.Equals(FIXConstants.TIF_GTC) || or.TIF.Equals(FIXConstants.TIF_GTD))
                    {
                        promptWin.SetDynamicWindowSize();
                    }
                    promptWin.ShowDialog();
                    if (!promptWin.ShouldTrade)
                    {
                        return false;
                    }
                }
            }
            if ((or.MsgType == FIXConstants.MSGOrderCancelReplaceRequest) && (_confirmationPopUpPref.ISCXLReplace))
            {
                if (or.Venue.Equals("AUTO", StringComparison.OrdinalIgnoreCase))
                {
                    if (_confirmationPopUpPref.ISCXLReplace)
                    {
                        if (!or.IsManualOrder)
                        {
                            PromptWindow promptWin = new PromptWindow(ValidationManagerExtension.GetOrderText(or), "CXL/RPL Order Prompt");
                            if (or.TIF.Equals(FIXConstants.TIF_GTC) || or.TIF.Equals(FIXConstants.TIF_GTD))
                            {
                                promptWin.SetDynamicWindowSize();
                            }
                            promptWin.ShowDialog();
                            if (!promptWin.ShouldTrade)
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    if (!or.IsManualOrder)
                    {
                        PromptWindow promptWin = new PromptWindow(ValidationManagerExtension.GetOrderText(or), "CXL/RPL Order Prompt");
                        if (or.TIF.Equals(FIXConstants.TIF_GTC) || or.TIF.Equals(FIXConstants.TIF_GTD))
                        {
                            promptWin.SetDynamicWindowSize();
                        }
                        promptWin.ShowDialog();
                        if (!promptWin.ShouldTrade)
                        {
                            return false;
                        }
                    }
                }
            }

            if (or.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrder)
            {
                if ((or.PranaMsgType == (int)Global.OrderFields.PranaMsgTypes.ORDStaged) || (or.PranaMsgType == (int)Global.OrderFields.PranaMsgTypes.ORDManual))
                {
                    return true;
                }
                else
                {
                    PromptWindow promptWin = new PromptWindow(ValidationManagerExtension.GetOrderText(or), "CXL/RPL Order Prompt");
                    promptWin.ShowDialog();
                    if (!promptWin.ShouldTrade)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool ValidateOrder(OrderSingle orderRequest, int companyUserID, bool isComingFrmoTT = false)
        {
            if ((orderRequest.OrderID == string.Empty && orderRequest.TransactionSource != BusinessObjects.AppConstants.TransactionSource.Rebalancer && orderRequest.TransactionSource != BusinessObjects.AppConstants.TransactionSource.TradeImport)
                || !(orderRequest.MsgType == FIXConstants.MSGOrder) || 
                ((orderRequest.TIF.Equals(FIXConstants.TIF_GTC) || orderRequest.TIF.Equals(FIXConstants.TIF_GTD)) && isComingFrmoTT))
            {
                if (!ValidatePopUpSettings(orderRequest))
                {
                    return false;
                }
            }

            switch (orderRequest.MsgType)
            {
                case FIXConstants.MSGOrder:
                    return ValidateSingleOrder(orderRequest);

                case FIXConstants.MSGExecutionReport:
                    return true;

                case FIXConstants.MSGOrderCancelReject:
                    return true;

                case FIXConstants.MSGOrderCancelReplaceRequest:
                    if (IsOrderReplaceable(orderRequest, companyUserID))
                    {
                        return IsReplaceValid(orderRequest);
                    }
                    else
                    {
                        return false;
                    }

                case FIXConstants.MSGOrderCancelRequest:
                    if (ISOrderCancellable(orderRequest) || IsOrderStatusPendingComplianceApproval(orderRequest))
                    {
                        return IsCancelValid(orderRequest);
                    }
                    else
                    {
                        MessageBox.Show("Can't cancel order in " + orderRequest.OrderStatus + " state", "Warning!");
                        return false;
                    }

                case FIXConstants.MSGOrderRollOverRequest:
                    return ValidationManagerExtension.ISOrderRolloverable(orderRequest);

                case FIXConstants.MSGTransferUser:
                    return true;

                case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX:
                    return ValidateAlgoSyntheticReplaceOrderFIX(orderRequest);

                case CustomFIXConstants.MSGAlgoSyntheticReplaceOrder:
                    return ValidateAlgoSyntheticReplaceOrderFIX(orderRequest);

                default:
                    return true;
            }
        }

        private static bool ValidateAlgoSyntheticReplaceOrderFIX(OrderSingle orderRequest)
        {
            bool valid = true;

            if (orderRequest.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrder)
            {
                double replaceQty = orderRequest.Quantity;
                OrderSingle cancelOrder = BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection[orderRequest.AlgoSyntheticRPLParent];
                if (cancelOrder.CumQty >= replaceQty)
                {
                    MessageBox.Show("The Replace Quantity should be greater than Executed Quantity!");
                    valid = false;
                }
                if (orderRequest.StagedOrderID != string.Empty)
                {
                    if (BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection.ContainsKey(orderRequest.StagedOrderID))
                    {
                        OrderSingle parentStagedOrder = BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection[orderRequest.StagedOrderID];
                        double allowedMaxQty = parentStagedOrder.UnsentQty + cancelOrder.Quantity - cancelOrder.CumQty;
                        if (orderRequest.LeavesQty > allowedMaxQty)
                        {
                            MessageBox.Show("The Replace Quantity should be lesser than parent Unsent Quantity!");
                            valid = false;
                        }
                    }
                }
            }
            else
            {
                double replaceQty = orderRequest.Quantity;
                if (orderRequest.Text != "New Order")
                {
                    OrderSingle cancelOrder = BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection[orderRequest.AlgoSyntheticRPLParent];
                    replaceQty = orderRequest.Quantity - cancelOrder.CumQty;
                    orderRequest.Quantity = replaceQty;
                }

                if (orderRequest.StagedOrderID != string.Empty)
                {
                    double parentLeavesQuantity = 0.0;
                    parentLeavesQuantity = ValidateStagedSubAlgoReplaceFIXOrder(orderRequest);
                    if (parentLeavesQuantity > 0.0 && parentLeavesQuantity < replaceQty)
                    {
                        replaceQty = parentLeavesQuantity;
                        orderRequest.Quantity = replaceQty;
                    }
                }


                string messageAlgoReplace;

                if (replaceQty > 0.0)
                {
                    messageAlgoReplace = TagDatabaseManager.GetInstance.GetOrderSideText(orderRequest.OrderSideTagValue) + replaceQty + " shares of "
                        + orderRequest.Symbol + " @ " + orderRequest.Price + " " +
                        TagDatabaseManager.GetInstance.GetOrderTypeText(orderRequest.OrderTypeTagValue) + " for " +
                        orderRequest.TradingAccountName + " on " + orderRequest.CounterPartyName;
                    AlgoPromptWindow promptWinNew = new AlgoPromptWindow(messageAlgoReplace, "Replace Order Prompt", orderRequest);
                    promptWinNew.Show();
                    return false;
                }
            }

            return valid;
        }

        private static bool ValidateSingleOrder(OrderSingle orderRequest)
        {
            if (orderRequest.StagedOrderID != int.MinValue.ToString() && orderRequest.StagedOrderID != string.Empty && !TradeManager.GetInstance().WorkingSubOrderDictionary.ContainsKey(orderRequest.StagedOrderID))
            {
                MessageBox.Show(TradeManagerConstants.MSG_ORDERS_HAS_BEEN_REMOVED_OR_MERGED, TradeManagerConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                IsTradeMergedRemovedEdited = true;
                return false;
            }

            // if staged order
            if (orderRequest.StagedOrderID != int.MinValue.ToString() && orderRequest.StagedOrderID != string.Empty && TradeManager.GetInstance().WorkingSubOrderDictionary.ContainsKey(orderRequest.StagedOrderID))
            {
                OrderSingle parentOrder = TradeManager.GetInstance().WorkingSubOrderDictionary[orderRequest.StagedOrderID];

                if (TradeManager.GetInstance().PriceSymbolSetting.LimitPriceCheck)
                {
                    if (!ValidateLimitPricesForSubOrder(orderRequest, parentOrder))
                    {
                        return false;
                    }
                }

                // case required when tkt opened for sub and then parent cancelled and then trade generated frm tkt
                if (parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled
                    || parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver
                    || parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingCancel
                    || parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingRollOver)
                {
                    MessageBox.Show("Can't generate sub for parent status " + TagDatabaseManager.GetInstance.GetOrderStatusText(parentOrder.OrderStatusTagValue), "Warning!");
                    return false;
                }

                if (orderRequest.Quantity > parentOrder.UnsentQty)
                {
                    // quantity exceeds parent unsent qty.
                    MessageBox.Show("Quantity exceeds parent Unsent Qty! Can't send order.", "Warning!");
                    return false;
                }
            }
            else
            {
                if (orderRequest.AlgoStrategyID != String.Empty && orderRequest.AlgoStrategyID != int.MinValue.ToString() && BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection.ContainsKey(orderRequest.ParentClOrderID))
                {
                    OrderSingle parentCancelOrder = BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection[orderRequest.ParentClOrderID];
                    if (orderRequest.Quantity < parentCancelOrder.CumQty)
                    {
                        MessageBox.Show("Quantity exceeds parent Unsent Qty! Can't send order.", "Warning!");
                        return false;
                    }
                }
            }
            if (!orderRequest.Description.Equals(TradeManagerConstants.CAPTION_MERGE_ORDERS) && _confirmationPopUpPref.IsManualOrder && orderRequest.IsManualOrder)
            {
                double cumQty = orderRequest.CumQtyForSubOrder;
                if (orderRequest.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub)
                    cumQty = orderRequest.CumQty;
                PromptWindow promptWin = new PromptWindow(GetManualOrderText(orderRequest), "New Order Prompt", orderRequest.Quantity, cumQty);
                promptWin.ShowInTaskbar = false;
                promptWin.ShowDialog();
                if (!promptWin.ShouldTrade)
                {
                    return false;
                }
                else
                {
                    if (orderRequest.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub)
                        orderRequest.CumQty = Convert.ToDouble(promptWin.CumQty);
                    else
                        orderRequest.CumQtyForSubOrder = Convert.ToDouble(promptWin.CumQty);
                }
            }
            return true;
        }



        private static bool IsReplaceValid(OrderSingle orderRequest)
        {
            switch (orderRequest.PranaMsgType)
            {
                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged:
                    return IsStagedOrderReplaceable(orderRequest);

                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSub:
                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSubChild:
                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub:
                    OrderSingle parentOrder = TradeManager.GetInstance().WorkingSubOrderDictionary[orderRequest.StagedOrderID];
                    return ValidateStagedSubOrder(parentOrder, orderRequest);

                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManual:
                    return IsManualOrderReplaceable(orderRequest);

                case (int)Prana.Global.OrderFields.PranaMsgTypes.InternalOrder:
                    OrderSingle parentOrderNotSet = TradeManager.GetInstance().WorkingSubOrderDictionary[orderRequest.ParentClOrderID];
                    return IsOrderReplaceValid(orderRequest, parentOrderNotSet);

                default:
                    return true;
            }
        }

        private static bool IsCancelValid(OrderSingle orderRequest)
        {
            switch (orderRequest.PranaMsgType)
            {
                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged:
                    return ValidateStagedParentCancel(orderRequest);

                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSub:
                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSubChild:
                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub:
                    return true;

                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManual:
                    return true;

                default:
                    return true;
            }
        }

        public static bool ValidateStagedSubOrder(OrderSingle parentOrder, OrderSingle subOrder)
        {

            if (TradeManager.GetInstance().PriceSymbolSetting.LimitPriceCheck)
            {
                if (!ValidateLimitPricesForSubOrder(subOrder, parentOrder))
                {
                    return false;
                }
            }

            if (subOrder.Quantity < subOrder.CumQty)
            {
                MessageBox.Show("An order can't be replaced to a quantity less than it's Cumulative Quantity ", "Warning!");
                return false;
            }

            double unsentQuantity = parentOrder.UnsentQty;
            if (parentOrder.OrderCollection != null)
            {
                foreach (OrderSingle otherSubOrder in parentOrder.OrderCollection)
                {
                    if (otherSubOrder.ParentClOrderID == subOrder.ParentClOrderID)
                    {
                        // this is to add quantity of this existing order before replace
                        // the quantity shud be added to unsent quantity
                        unsentQuantity += otherSubOrder.Quantity;
                    }
                }
            }
            if (subOrder.Quantity > unsentQuantity)
            {
                return true;
            }
            else
            {
                OrderSingle parentOrderNotSet = TradeManager.GetInstance().WorkingSubOrderDictionary[subOrder.ParentClOrderID];
                return IsOrderReplaceValid(subOrder, parentOrderNotSet);
            }
        }

        private static bool IsStagedOrderReplaceable(OrderSingle stagedOrder)
        {
            OrderSingle origOrder = TradeManager.GetInstance().WorkingSubOrderDictionary[stagedOrder.ParentClOrderID];
            if (TradeManager.GetInstance().PriceSymbolSetting.LimitPriceCheck)
            {
                // no prompt if replaced to order type other than Limit
                if (stagedOrder.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit)
                {
                    if (!ValidateLimitPriceForStagedParentReplace(stagedOrder, origOrder))
                    {
                        return false;
                    }
                }
            }

            if (stagedOrder.Quantity < origOrder.Quantity - origOrder.UnsentQty)
            {
                MessageBox.Show("Quantity less than working quantity, Can't replace order !", "Warning!");
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool ISOrderCancellable(OrderSingle order)
        {
            OrderSingle existingOrder = TradeManager.GetInstance().WorkingSubOrderDictionary[order.ParentClOrderID];
            if (existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Expired ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Filled ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingReplace ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingCancel ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingRollOver ||
                existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_DoneForDay ||
                existingOrder.OrderStatusTagValue == string.Empty)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// tells whether order is in Pending Compliance Approval or not
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static bool IsOrderStatusPendingComplianceApproval(OrderSingle order)
        {
            bool isPendingComplianceApproval = false;
            try
            {
                OrderSingle existingOrder = TradeManager.GetInstance().WorkingSubOrderDictionary[order.ParentClOrderID];

                if (existingOrder.OrderStatusWithoutRollover.Equals(Prana.BusinessObjects.Compliance.Constants.PreTradeConstants.MsgTradePending))
                    isPendingComplianceApproval = true;
                else
                    isPendingComplianceApproval = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return isPendingComplianceApproval;
        }

        public static bool IsOrderReplaceable(OrderSingle order, int companyUserID)
        {
            if (!TradeManager.GetInstance().WorkingSubOrderDictionary.ContainsKey(order.ParentClOrderID))
            {
                MessageBox.Show(TradeManagerConstants.MSG_ORDERS_HAS_BEEN_REMOVED_OR_MERGED, TradeManagerConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                IsTradeMergedRemovedEdited = true;
                return false;
            }
            OrderSingle existingOrder = TradeManager.GetInstance().WorkingSubOrderDictionary[order.ParentClOrderID];
            TranferTradeRules transferTradeRules = CachedDataManager.GetInstance.GetTransferTradeRules();

            if (existingOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManual ||
                existingOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub)
            {
                //return true;
            }
            else if (!transferTradeRules.IsAllowAllUserToCancelReplaceRemove)
            {
                if (existingOrder.CompanyUserID != companyUserID)
                {
                    MessageBox.Show("No permission to trade this order", "Warning!");
                    return false;
                }
            }
            if (IsOrderStatusPendingComplianceApproval(order))
            {
                return true;
            }
            switch (existingOrder.PranaMsgType)
            {
                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManual:
                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub:
                    if (existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Expired ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingReplace ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingCancel ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingRollOver ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_DoneForDay ||
                        existingOrder.OrderStatusTagValue == string.Empty)
                    {
                        MessageBox.Show("Can't replace order in " + existingOrder.OrderStatus + " state", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        IsTradeMergedRemovedEdited = true;
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                default:

                    if (existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Expired ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingReplace ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingCancel ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingRollOver ||
                        existingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_DoneForDay ||
                        existingOrder.OrderStatusTagValue == string.Empty)
                    {
                        MessageBox.Show("Can't replace order in " + existingOrder.OrderStatus + " State", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        IsTradeMergedRemovedEdited = true;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
            }
        }

        private static bool IsOrderReplaceValid(OrderSingle order, OrderSingle existingOrder)
        {
            bool valid = true;
            if (order.Quantity <= existingOrder.CumQty)
            {
                MessageBox.Show("Order Quantity should be greater than Executed Quantity", " Prana Warning");
                return false;
            }

            if (order.AlgoStrategyID != String.Empty && order.AlgoStrategyID != int.MinValue.ToString())
            {
                //Dileep: Removal of the negative working quantity Bug.
                //if (algoStrategy.IsSyntheticReplace)
                if (order.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrder)
                {
                    double executedQty = existingOrder.CumQty;
                    double replaceQty = order.Quantity - executedQty;
                    string messageAlgoReplace = "Order will be sent for " + order.OrderSide + " " +
                        replaceQty.ToString() + " " + order.Symbol + " " + order.OrderType
                        + " Do you want to continue?";


                    PromptWindow promptWin = new PromptWindow(messageAlgoReplace, "Replace Order Prompt");
                    promptWin.ShowDialog();
                    if (promptWin.ShouldTrade)
                    {
                        order.Quantity = replaceQty;
                        order.CumQty = executedQty;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }

            }
            return valid;
        }

        private static bool IsManualOrderReplaceable(OrderSingle order)
        {
            OrderSingle existingOrder = TradeManager.GetInstance().WorkingSubOrderDictionary[order.ParentClOrderID];
            if (order.Quantity < existingOrder.CumQty)
            {
                MessageBox.Show("Order can't be replaced to a quantity below the executed quantity ", " Prana Warning");
                return false;
            }
            else
            {
                return true;
            }
        }

        #region Limit Price Check Scenarios
        private static bool ValidateLimitPricesForSubOrder(OrderSingle subOrder, OrderSingle parentOrder)
        {
            bool isPriceValid = true;
            if (parentOrder.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit)
            {
                switch (subOrder.OrderSideTagValue)
                {
                    // for buy/buy_to_close if suborder price exceeds parent, generate prompt
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Buy_Closed:
                        if (subOrder.Price > parentOrder.Price)
                        {
                            if ((MessageBox.Show("Child Buy limit price > Parent Buy Limit Price. Do you want to proceed? ", "Warning!", MessageBoxButtons.YesNo) == DialogResult.No))
                            {
                                isPriceValid = false;
                            }
                        }
                        break;

                    // for sell/sell_short if suborder price falls below parent, generate prompt
                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_SellShort:
                    case FIXConstants.SIDE_SellShortExempt:
                        if (subOrder.Price < parentOrder.Price)
                        {
                            if ((MessageBox.Show("Child Sell limit price < Parent Sell Limit Price. Do you want to proceed? ", "Warning!", MessageBoxButtons.YesNo) == DialogResult.No))
                            {
                                isPriceValid = false;
                            }
                        }
                        break;
                }
            }

            return isPriceValid;
        }

        private static bool ValidateLimitPriceForStagedParentReplace(OrderSingle stagedReplace, OrderSingle origOrder)
        {
            bool isPriceValid = true;
            switch (stagedReplace.OrderSideTagValue)
            {
                case FIXConstants.SIDE_Buy:
                case FIXConstants.SIDE_Buy_Closed:
                    if (origOrder.OrderCollection != null)
                    {
                        foreach (OrderSingle sub in origOrder.OrderCollection)
                        {
                            // if Staged Order Limit Price less than Sub Limit Price generate warning
                            if (sub.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit && stagedReplace.Price < sub.Price)
                            {
                                if ((MessageBox.Show("Staged Order Limit Price less than Sub Limit Price.Do you want to proceed?", "Warning!", MessageBoxButtons.YesNo) == DialogResult.No))
                                {
                                    isPriceValid = false;
                                    break;
                                }
                                else
                                {
                                    // this is to go out of the loop as if check is validated for one sub...
                                    // we do not need to check for other subs
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case FIXConstants.SIDE_Sell:
                case FIXConstants.SIDE_SellShort:
                case FIXConstants.SIDE_SellShortExempt:
                    if (origOrder.OrderCollection != null)
                    {
                        foreach (OrderSingle sub in origOrder.OrderCollection)
                        {
                            if (sub.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit && stagedReplace.Price > sub.Price)
                            {
                                // if Staged Order Limit Price less than Sub Limit Price generate warning
                                if ((MessageBox.Show("Staged Order Limit Price greater than Sub Limit Price.Do you want to proceed?", "Warning!", MessageBoxButtons.YesNo) == DialogResult.No))
                                {
                                    isPriceValid = false;
                                    break;
                                }
                                else
                                {
                                    // this is to go out of the loop as if check is validated for one sub...
                                    // we do not need to check for other subs
                                    break;
                                }
                            }
                        }
                    }
                    break;
            }
            return isPriceValid;
        }
        #endregion

        private static bool ValidateStagedParentCancel(OrderSingle orderRequest)
        {
            OrderSingle parentOrder = TradeManager.GetInstance().WorkingSubOrderDictionary[orderRequest.ParentClOrderID];
            OrderBindingList cancelCollection = GenerateCancelCollection(parentOrder);
            if (cancelCollection != null)
            {
                // cancel cancelleable subOrders
                foreach (OrderSingle suborder in cancelCollection)
                {
                    OrderSingle orRequest = (OrderSingle)suborder.Clone();
                    orRequest.OrigClOrderID = suborder.ClOrderID;
                    CancelOrder(orRequest);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Cancel Staged Parent and Subs Validation
        private static void CancelOrder(OrderSingle cancelRequest)
        {
            cancelRequest.MsgType = FIXConstants.MSGOrderCancelRequest;
            cancelRequest.Text = "Cancel Requested by User";
            // Kuldeep A.:- status should be pending cancel in case of Cancel trades.
            cancelRequest.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingCancel;
            cancelRequest.TransactionTime = DateTime.Now.ToUniversalTime();
            TradeManagerExtension.GetInstance().SendValidatedTrades(cancelRequest);
        }

        private static OrderBindingList GenerateCancelCollection(OrderSingle parentOrder)
        {
            OrderBindingList cancelOrderCollection = new OrderBindingList();
            string orderDetails = string.Empty;
            string ordertype = string.Empty;

            ordertype = TagDatabaseManager.GetInstance.GetOrderTypeText(parentOrder.OrderTypeTagValue.ToString());
            orderDetails += "\r\n" + ordertype + " " + parentOrder.Symbol.ToString() + " " + parentOrder.Quantity.ToString() + " " + parentOrder.Price.ToString();

            if (parentOrder.OrderCollection != null)
            {
                foreach (OrderSingle childOrder in parentOrder.OrderCollection)
                {
                    if ((childOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Filled) &&
                        (childOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Cancelled) &&
                        (childOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_RollOver) &&
                        (childOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Rejected) &&
                        (childOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingCancel) &&
                        (childOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingRollOver) &&
                        (childOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingNew) &&
                        (childOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingReplace) &&
                        (childOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_DoneForDay) &&
                        (childOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Expired) &&
                        (childOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_Aborted) &&
                        (childOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_AlgoPreviousPendingReplace) &&
                        (childOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected))
                    {
                        cancelOrderCollection.Add(childOrder);
                        ordertype = TagDatabaseManager.GetInstance.GetOrderTypeText(childOrder.OrderTypeTagValue.ToString());
                        orderDetails += "\r\n" + ordertype + " " + childOrder.Symbol.ToString() + " " + childOrder.Quantity.ToString() + " " + childOrder.Price.ToString();
                    }
                }
            }
            if (cancelOrderCollection.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to Cancel Order(s): " + orderDetails, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    return cancelOrderCollection;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return cancelOrderCollection;
            }
        }
        #endregion

        private static double ValidateStagedSubAlgoReplaceFIXOrder(OrderSingle syntheticReplaceSubOrder)
        {
            switch (syntheticReplaceSubOrder.PranaMsgType)
            {
                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSub:
                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSubChild:
                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub:
                    OrderSingle parentOrder = TradeManager.GetInstance().WorkingSubOrderDictionary[syntheticReplaceSubOrder.StagedOrderID];
                    return parentOrder.Quantity - parentOrder.CumQty;

                default:
                    return syntheticReplaceSubOrder.Quantity;
            }
        }
    }
}