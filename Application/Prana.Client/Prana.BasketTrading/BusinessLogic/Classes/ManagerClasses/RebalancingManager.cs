using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.CommonDataCache;

namespace Prana.BasketTrading
{
    class RebalancingManager
    {

        //private double _quantity = 0.0;
        private static Dictionary<string, Order> _targetBasketDict = null;
        private static Dictionary<string, Order> _tradedBasketDict = null;
        private static BasketDetail _targetBasket = null;
        private static BasketDetail _tradedBasket = null;

        public static BasketDetail RebalanceBaskets(BasketDetail targetBasket, BasketDetail tradedBasket)
        {
            _targetBasket = targetBasket;
            _tradedBasket = tradedBasket;

 

            BasketDetail rebalanceBasket = new BasketDetail();
            _targetBasketDict = new Dictionary<string, Order>();
            _tradedBasketDict = new Dictionary<string, Order>();

            // add orders in target dictionary
            GetDictionaryFromBasket(_targetBasketDict, _targetBasket);


            // add orders in traded dictionary
            GetDictionaryFromBasket(_tradedBasketDict, _tradedBasket);

            // add orders in rebalance basket
            foreach (KeyValuePair<string, Order> targetBaskeDictItem in _targetBasketDict)
            {
                Order rebalanceOrder = new Order(true);
                rebalanceOrder =(Order) targetBaskeDictItem.Value.Clone();
                rebalanceOrder.ClientOrderID = IDGenerator.GenerateClientOrderID();
                rebalanceOrder.ParentClientOrderID = rebalanceOrder.ClientOrderID;
                if (_tradedBasketDict.ContainsKey(targetBaskeDictItem.Key))
                {
                    rebalanceOrder.Quantity = targetBaskeDictItem.Value.Quantity - _tradedBasketDict[targetBaskeDictItem.Key].Quantity;
                    if (rebalanceOrder.Quantity < 0.0)
                    {
                        rebalanceOrder.Quantity = -(rebalanceOrder.Quantity);
                        //if (_tradedBasketDict[targetBaskeDictItem.Key].OrderSideTagValue == FIXConstants.SIDE_Buy)
                        //{
                            rebalanceOrder.OrderSideTagValue = FIXConstants.SIDE_Sell;
                        //}
                        //else if (_tradedBasketDict[targetBaskeDictItem.Key].OrderSideTagValue == FIXConstants.SIDE_Sell)
                        //{
                        //    rebalanceOrder.OrderSideTagValue = FIXConstants.SIDE_Buy;
                        //}
                        
                    }
                    else if (rebalanceOrder.Quantity > 0.0)
                    {
                        rebalanceOrder.OrderSideTagValue = FIXConstants.SIDE_Buy;
                    }
                }
                else
                {
                    if (rebalanceOrder.Quantity < 0.0)
                    {
                        rebalanceOrder.Quantity = -rebalanceOrder.Quantity;
                        rebalanceOrder.OrderSideTagValue = FIXConstants.SIDE_Sell;
                    }
                    else if (rebalanceOrder.Quantity > 0.0)
                    {
                        rebalanceOrder.OrderSideTagValue = FIXConstants.SIDE_Buy;
                    }
                    
                }

                if (rebalanceOrder.Quantity > 0)
                {
                    rebalanceOrder.UnsentQty = rebalanceOrder.Quantity; 
                    rebalanceBasket.BasketOrders.Add(rebalanceOrder);
                }

                rebalanceOrder.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(rebalanceOrder.OrderSideTagValue);

            }

            foreach (KeyValuePair<string, Order> tradedBasketDictItem in _tradedBasketDict)
            {
                Order rebalanceOrder = new Order(true);
               
                if (!_targetBasketDict.ContainsKey(tradedBasketDictItem.Key))
                {
                    rebalanceOrder = (Order)tradedBasketDictItem.Value.Clone();
                    if (tradedBasketDictItem.Value.Quantity < 0.0)
                    {
                        rebalanceOrder.Quantity = -tradedBasketDictItem.Value.Quantity;
                        rebalanceOrder.OrderSideTagValue = FIXConstants.SIDE_Sell;
                    }
                    else
                    {
                        rebalanceOrder.OrderSideTagValue = FIXConstants.SIDE_Buy;
                    }
                    if (rebalanceOrder.Quantity > 0)
                    {
                        rebalanceOrder.UnsentQty = rebalanceOrder.Quantity;
                        rebalanceOrder.SetDefaultValues();
                        rebalanceBasket.BasketOrders.Add(rebalanceOrder);
                    }
                }

            }


            return rebalanceBasket;
        }

        private static void GetDictionaryFromBasket(Dictionary<string, Order> dict, BasketDetail basketDetail)
        {
            foreach (Order order in basketDetail.BasketOrders)
            {
                Order clonedOrder = (Order)order.Clone();
                string Key = clonedOrder.Symbol.ToString().ToUpper();// +"," + clonedOrder.Venue.ToString();
                if (!dict.ContainsKey(Key))
                {
                    if (clonedOrder.OrderSideTagValue == FIXConstants.SIDE_Sell)
                    {
                        clonedOrder.Quantity = -order.Quantity;
                        clonedOrder.CumQty = -order.CumQty;
                    }

                    dict.Add(Key, clonedOrder);
                }
                else
                {
                    if (clonedOrder.OrderSideTagValue == FIXConstants.SIDE_Sell || clonedOrder.OrderSideTagValue == FIXConstants.SIDE_SellPlus || clonedOrder.OrderSideTagValue == FIXConstants.SIDE_SellShort || clonedOrder.OrderSideTagValue == FIXConstants.SIDE_SellShortExempt )
                    {
                        dict[Key].Quantity = dict[Key].Quantity - clonedOrder.Quantity;
                        dict[Key].CumQty = dict[Key].CumQty - clonedOrder.CumQty;
                    }
                    else 
                    {
                        dict[Key].Quantity = dict[Key].Quantity + clonedOrder.Quantity;
                        dict[Key].CumQty = dict[Key].CumQty + clonedOrder.CumQty;
                       
                    }
                   
                }
            }            
        }
      
    }
}


