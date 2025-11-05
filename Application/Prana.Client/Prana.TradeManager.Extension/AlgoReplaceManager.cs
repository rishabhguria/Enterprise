using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using System;
using System.Collections.Generic;

namespace Prana.TradeManager.Extension
{
    public class AlgoReplaceManager
    {
        static private AlgoReplaceManager _algoReplaceManager = null;
        public event AlgoValidTradeHandler AlgoValidTradeToUIThread;
        public event AlgoReplaceOrderEdit AlgoReplaceOrderEditToTradingTkt;
        public delegate void AlgoReplaceOrderEdit(object sender, EventArgs<OrderSingle> e);
        public event EventHandler<EventArgs<string, string, OrderSingle>> ShowAlgoPromptWindowEventHandler;

        private int _userID;
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        private AlgoReplaceManager()
        {
        }

        public static AlgoReplaceManager GetInstance()
        {
            if (_algoReplaceManager == null)
            {
                _algoReplaceManager = new AlgoReplaceManager();
            }
            return _algoReplaceManager;
        }

        Dictionary<string, OrderSingle> algoReplaceOrdersDict = new Dictionary<string, OrderSingle>();
        public Dictionary<string, OrderSingle> AlgoReplaceOrdersDictionary
        {
            get { return algoReplaceOrdersDict; }
        }

        internal void AddReplaceOrder(OrderSingle replaceOrder)
        {
            if (algoReplaceOrdersDict.ContainsKey(replaceOrder.AlgoSyntheticRPLParent))
            {
                if (algoReplaceOrdersDict.ContainsKey(replaceOrder.OrigClOrderID))
                {
                    if (algoReplaceOrdersDict[replaceOrder.OrigClOrderID].OrderStatusTagValue == CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected)
                    {
                        algoReplaceOrdersDict.Remove(replaceOrder.OrigClOrderID);
                    }
                }
            }
            if (!algoReplaceOrdersDict.ContainsKey(replaceOrder.AlgoSyntheticRPLParent))
                algoReplaceOrdersDict.Add(replaceOrder.AlgoSyntheticRPLParent, replaceOrder);
        }

        internal void SendReplaceOrder(string parentClOrderID)
        {
            OrderSingle replaceOrder = AlgoReplaceOrdersDictionary[parentClOrderID];

            if (replaceOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected)
            {
                replaceOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingNew;
                replaceOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(replaceOrder.OrderStatusTagValue);
                replaceOrder.MsgType = CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX;
                replaceOrder.DiscretionOffset = double.MinValue;
                replaceOrder.Text = string.Empty;
                replaceOrder.ClientTime = DateTime.Now.ToLongTimeString();
                replaceOrder.TransactionTime = DateTime.Now.ToUniversalTime();
                replaceOrder.AvgPrice = 0;
                replaceOrder.LastPrice = 0.0;
                replaceOrder.LastShares = 0.0;
                replaceOrder.LeavesQty = replaceOrder.Quantity;

                if (ValidateAlgoSyntheticReplaceOrderFIX(replaceOrder))
                {
                    replaceOrder.CumQty = 0.0;
                    if (AlgoValidTradeToUIThread != null)
                    {
                        AlgoValidTradeToUIThread(this, new EventArgs<OrderSingle>(replaceOrder));
                    }
                    AlgoReplaceOrdersDictionary.Remove(parentClOrderID);
                }
                else
                {
                    replaceOrder.Text = "New Order";

                    if (AlgoReplaceOrderEditToTradingTkt != null)
                    {
                        replaceOrder.MsgType = FIXConstants.MSGOrder;
                        AlgoReplaceOrderEditToTradingTkt(this, new EventArgs<OrderSingle>(replaceOrder));
                        AlgoReplaceOrdersDictionary.Remove(parentClOrderID);
                    }
                }
            }
        }

        public OrderSingle GetCancelAndSaveReplaceOrder(OrderSingle replaceOrder)
        {
            if (replaceOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManual ||
                replaceOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub ||
                replaceOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged)
            {
                if (replaceOrder.OrigClOrderID == string.Empty)
                {
                    replaceOrder.OrigClOrderID = replaceOrder.ClOrderID;
                }
                replaceOrder.MsgType = FIXConstants.MSGOrderCancelReplaceRequest;
                replaceOrder.TransactionTime = DateTime.Now.ToUniversalTime();
                replaceOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingNew;
                return replaceOrder;
            }
            else
            {
                OrderSingle existingAlgoOrder = BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection[replaceOrder.AlgoSyntheticRPLParent];
                OrderSingle orCancelRequest = (OrderSingle)existingAlgoOrder.Clone();
                orCancelRequest.Text = "Cancel Requested by User";
                orCancelRequest.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingNew;
                orCancelRequest.MsgType = FIXConstants.MSGOrderCancelRequest;
                orCancelRequest.TransactionTime = DateTime.Now.ToUniversalTime();
                orCancelRequest.OrigClOrderID = orCancelRequest.ClOrderID;
                replaceOrder.ClOrderID = string.Empty;
                replaceOrder.ParentClOrderID = string.Empty;
                replaceOrder.MsgType = CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew;
                replaceOrder.OrderStatusTagValue = CustomFIXConstants.ORDSTATUS_AlgoPreviousPendingReplace;
                replaceOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(replaceOrder.OrderStatusTagValue);
                replaceOrder.AvgPrice = 0;
                replaceOrder.LastPrice = 0.0;
                replaceOrder.LastShares = 0.0;
                AddReplaceOrder(replaceOrder);
                return orCancelRequest;
            }
        }

        public OrderSingle GetReplaceOrder(string origClOrderID)
        {
            if (algoReplaceOrdersDict.ContainsKey(origClOrderID))
            {
                return algoReplaceOrdersDict[origClOrderID];
            }
            else
            {
                return null;
            }
        }

        public void CancelAwaitingOrder(string parentClOrderID)
        {
            if (BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection.ContainsKey(parentClOrderID))
            {
                OrderSingle cancelledOrder = BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection[parentClOrderID];
                cancelledOrder.OrderStatusTagValue = CustomFIXConstants.ORDSTATUS_Aborted;
                cancelledOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(cancelledOrder.OrderStatusTagValue);
                cancelledOrder.LeavesQty = 0.0;
                cancelledOrder.Text = "Cancel aborted by user";
            }
        }

        private bool ValidateAlgoSyntheticReplaceOrderFIX(OrderSingle orderRequest)
        {
            bool valid = true;
            OrderSingle origCancelledOrder = BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection[orderRequest.AlgoSyntheticRPLParent];
            double replaceQty = orderRequest.Quantity;
            if (orderRequest.Text != "New Order")
            {
                replaceQty = orderRequest.Quantity + orderRequest.CumQty - origCancelledOrder.CumQty;
            }
            string messageAlgoReplace;

            if (replaceQty <= 0.0)
            {
                messageAlgoReplace = "Order already filled. Cannot be replaced";

                if (ShowAlgoPromptWindowEventHandler != null)
                    ShowAlgoPromptWindowEventHandler(this, new EventArgs<string, string, OrderSingle>(messageAlgoReplace, "Replace Order Prompt", orderRequest));
                return false;
            }
            return valid;
        }
    }
}