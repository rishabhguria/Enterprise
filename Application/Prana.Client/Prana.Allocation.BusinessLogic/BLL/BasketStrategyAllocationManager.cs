using System;
using System.Collections.Generic;
using System.Text;
using Prana.Global;
using Prana.BusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
namespace Prana.Allocation.BLL
{
    public class BasketStrategyAllocationManager :BasketAllocationManager 
    {
        //forstrategy
        int _userID = int.MinValue;
        OrderStrategyAllocationManager _orderAllocationManager = OrderStrategyAllocationManager.GetInstance;
        static BasketStrategyAllocationManager _basketStrategyAllocationManager = null;
        private  Dictionary<string, Prana.BusinessObjects.Order> _strategyBasketOrderIDValueCollection = new Dictionary<string, Prana.BusinessObjects.Order>();
        private  Dictionary<string, Prana.BusinessObjects.BasketDetail> _strategyBasketIDValueCollection = new Dictionary<string, Prana.BusinessObjects.BasketDetail>();
        Int64  _lastID = 0;
        private Int64 _lastOrderSeqNumber = 0;
         BasketCollection _unallocatedStrategyBaskets = new BasketCollection();
         BasketGroupCollection _groupedbaskets = new BasketGroupCollection();
         BasketGroupCollection _allocatedBasketGroups = new BasketGroupCollection();
        private BasketStrategyAllocationManager()
        {
        }

        public static BasketStrategyAllocationManager GetInstance
        {
            get
            {

                if (_basketStrategyAllocationManager == null)
                {
                    _basketStrategyAllocationManager = new BasketStrategyAllocationManager();
                }
                return _basketStrategyAllocationManager;
            }
        }

        public override void Initilize(int userID, string AllAUECDatesString)
        {
            //SK Clear all clollectrions as we are not going to cache orders and baskets
            _strategyBasketOrderIDValueCollection = new Dictionary<string, Prana.BusinessObjects.Order>();
            _strategyBasketIDValueCollection = new Dictionary<string, Prana.BusinessObjects.BasketDetail>();
            _unallocatedStrategyBaskets = new BasketCollection();
            _groupedbaskets = new BasketGroupCollection();
            _allocatedBasketGroups = new BasketGroupCollection();

            _userID = userID;
            //Int64 maxSeqNumber = OrderDataManager.GetMaxSeqNumber();
            string[] arrAuecDates = AllAUECDatesString.Split(Seperators.SEPERATOR_6);
            string[] Dates = arrAuecDates[0].Split(Seperators.SEPERATOR_5);
            DateTime currentDate = Convert.ToDateTime(Dates[1]);

            BasketCollection unallocatedStrategyBaskets = BasketAllocationDBManager.GetUnAllocatedBasketDetails(_userID, currentDate, (int)PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY, _lastID);
            AddBaskets(unallocatedStrategyBaskets);
            BasketGroupCollection basketGroups = BasketAllocationDBManager.GetBasketGroups(AllAUECDatesString, (int)PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY);
            foreach (BasketGroup basketGroup in basketGroups)
            {
                AddBasketOrders(basketGroup);
            }
            foreach (BasketGroup basketGroup in basketGroups)
            {
                if (basketGroup.GroupState == PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED)
                {
                    _groupedbaskets.Add(basketGroup);
                }
                else
                {
                    _allocatedBasketGroups.Add(basketGroup);
                }
            }
            AllocationStrategies allocationStrategies = FundStraegyManager.GetBasketAllocatedStrategies(AllAUECDatesString);
            foreach (AllocationStrategy strategy in allocationStrategies)
            {
                BasketGroup group = _allocatedBasketGroups.GetBasketGroup(strategy.GroupID);
                if (group.Strategies == null)
                {
                    group.Strategies = new AllocationStrategies();
                }

                group.Strategies.Add(strategy);
            }
            //if (_unallocatedStrategyBaskets.Count > 0)
            //{
            //    _lastID = Int64.Parse(_unallocatedStrategyBaskets[_unallocatedStrategyBaskets.Count - 1].TradedBasketID);
            //}
            //_lastOrderSeqNumber = maxSeqNumber;
        }



        public override void UnBundleBasket(Prana.BusinessObjects.BasketDetail basket)
        {
            _unallocatedStrategyBaskets.Remove(basket);
            AllocationOrderCollection orders = new AllocationOrderCollection();
            foreach (Order order in basket.BasketOrders)
            {
                AllocationOrder unAllocatedOrder =OrderAllocationManager.GetAllocationOrder(order);
                unAllocatedOrder.TradingAccountID = basket.TradingAccountID;
                unAllocatedOrder.TradingAccountName = basket.TradingAccount;
                if (order.ParentClOrderID != string.Empty)
                {
                    orders.Add(unAllocatedOrder);
                }
            }
        }


        public override void AllocateBasket(BasketDetail basket, object allocationstrategies, DateTime AUECLocalDate)
        {
            AllocationStrategies strategies =(AllocationStrategies) allocationstrategies;
            BasketCollection baskets = new BasketCollection();
            baskets.Add(basket);
            BasketGroup basketGroup=GroupBaskets(baskets,AUECLocalDate);
            AllocateBasket(basketGroup, strategies, AUECLocalDate);
        }

        public override  void AllocateBasket(BasketGroup basketGroup, object allocationstrategies,DateTime AUECLocalDate)
        {
            AllocationStrategies strategies = (AllocationStrategies)allocationstrategies;
            basketGroup.AllocationType = PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY;
            basketGroup.GroupState = PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED;
            basketGroup.AllocateGroupToStrategy(strategies, basketGroup.CumQty);
            if (_groupedbaskets.Contains(basketGroup))
            {
                _groupedbaskets.Remove(basketGroup);
            }
            _allocatedBasketGroups.Add(basketGroup);
            BasketAllocationDBManager.SaveAllocatedBasket(basketGroup);
            _orderAllocationManager.AllocateBasketGroup(basketGroup, strategies, AUECLocalDate);
        }

        public override BasketGroup GroupBaskets(Prana.BusinessObjects.BasketCollection baskets, DateTime date)
        {
            BasketGroup basketGroup = new BasketGroup();
            basketGroup.AllocationType = PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY;
            basketGroup.AUECLocalDate = date;
            basketGroup.AUECID = baskets[0].BasketOrders[0].AUECID;
            basketGroup.GroupState = PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED;
            basketGroup.AddBaskets(baskets);
            _groupedbaskets.Add(basketGroup);
            foreach (BasketDetail basket in baskets)
            {
                _unallocatedStrategyBaskets.Remove(basket);
            }
            BasketAllocationDBManager.SaveBasketGroup(basketGroup, date);
            return basketGroup;
        }

        public override void UnAllocateBasket(BasketGroup basketGroup)
        {
            basketGroup.GroupState = PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED;
            _allocatedBasketGroups.Remove(basketGroup);
            if (basketGroup.AddedBaskets.Count == 1)
            {
                _unallocatedStrategyBaskets.Add(basketGroup.AddedBaskets[0]);
            }
            else
            {
                _groupedbaskets.Add(basketGroup);
            }
            BasketAllocationDBManager.RemoveAllocatedBasket(basketGroup);
            OrderAllocationDBManager.RemoveBasketGroupOrders(basketGroup);

        }

        public override void UnGroupBasketGroup(BasketGroup basketGroup)
        {
            basketGroup.GroupState = PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED;
            foreach (BasketDetail basket in basketGroup.AddedBaskets)
            {
                _unallocatedStrategyBaskets.Add(basket);

            }
            _groupedbaskets.Remove(basketGroup);
            BasketAllocationDBManager.DeleteBasketGroup(basketGroup);
        }


        //public override void UpdateBaskets(DateTime date)
        //{
        //    Prana.BusinessObjects.BasketCollection updatedUnAllocatedStrategyBaskets = BasketAllocationDBManager.GetUnAllocatedBasketDetails(_userID, date, (int)PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY,_lastID);
        //    if (updatedUnAllocatedStrategyBaskets.Count > 0)
        //    {
        //        foreach (BasketDetail basket in updatedUnAllocatedStrategyBaskets)
        //        {
        //            AddBasket(basket);

        //        }
        //        //_lastID = Int64.Parse(updatedUnAllocatedStrategyBaskets[updatedUnAllocatedStrategyBaskets.Count - 1].TradedBasketID);
        //    }
        //    //Int64 maxSeqNumber = OrderDataManager.GetMaxSeqNumber();
        //    OrderCollection updatedOrders = BasketDataManager.GetUpdatedStrategyBasketOrders(_lastOrderSeqNumber);
        //    foreach (Prana.BusinessObjects.Order order in updatedOrders)
        //    {
        //        if (IsOrderExist(order.ClientOrderID))
        //        {
        //            UpdateBasketOrder(order);
        //        }
        //        else
        //        {
        //            AddBasketOrder(order, true);
        //        }
        //    }
        //    //_lastOrderSeqNumber = maxSeqNumber;
        //}


        // For Srategy
        public  void AddBaskets(Prana.BusinessObjects.BasketCollection baskets)
        {
            if (baskets.Count > 0)
            {

                foreach (Prana.BusinessObjects.BasketDetail basket in baskets)
                {
                    AddBasket(basket);
                }
            }

        }
        public  void AddBasket(Prana.BusinessObjects.BasketDetail basket)
        {
            if (basket.CumQty > 0)
            {
                _unallocatedStrategyBaskets.Add(basket);
                _strategyBasketIDValueCollection.Add(basket.TradedBasketID, basket);
                AddBasketOrders(basket.BasketOrders);
            }
        }
        public  bool IsBasketExist(string TradedBasketID)
        {
            if (_strategyBasketIDValueCollection.ContainsKey(TradedBasketID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public  void AddBasketOrder(Prana.BusinessObjects.Order order, bool addOrder)
        {
            if (addOrder)
            {
                _strategyBasketIDValueCollection[order.ListID].BasketOrders.Add(order);
            }
            if (!_strategyBasketOrderIDValueCollection.ContainsKey(order.ClientOrderID))
                _strategyBasketOrderIDValueCollection.Add(order.ClientOrderID, order);
        }
        private  void AddBasketOrders(Prana.BusinessObjects.OrderCollection orders)
        {
            foreach (Prana.BusinessObjects.Order order in orders)
            {
                AddBasketOrder(order, false);
            }
        }

        private void AddBasketOrders(BasketGroup basketGroup)
        {
            foreach (BasketDetail basket in basketGroup.AddedBaskets)
            {
                AddBasketOrders(basket.BasketOrders);
            }
        }
        private  bool IsOrderExist(string clOrderID)
        {
            if (_strategyBasketOrderIDValueCollection.ContainsKey(clOrderID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //private void UpdateBasketOrder(Prana.BusinessObjects.Order order)
        //{

        //    Prana.BusinessObjects.Order oldOrder = _strategyBasketOrderIDValueCollection[order.ClientOrderID];
        //    oldOrder.CumQty = order.CumQty;
        //    oldOrder.AvgPrice = order.AvgPrice;
        //    oldOrder.Quantity = order.Quantity;
        //    oldOrder.OrderStatusTagValue = order.OrderStatusTagValue;
        //    oldOrder.OrderStatus = order.OrderStatus;
        //    UpdateOldBasket(order.ListID);
        //}
        //private void UpdateOldBasket(string listID)
        //{
        //    Prana.BusinessObjects.BasketDetail oldBasket = _strategyBasketIDValueCollection[listID];
        //    double quantity = oldBasket.Quantity;
        //    double cumQty = oldBasket.CumQty;
        //    double exevalue = oldBasket.ExeValue;
        //}


        public  void ClearAllData()
        {
            _strategyBasketIDValueCollection = new Dictionary<string, Prana.BusinessObjects.BasketDetail>();
            _strategyBasketOrderIDValueCollection = new Dictionary<string, Prana.BusinessObjects.Order>();
        }
        public override BasketCollection UnallocatedBaskets
        {
            get { return _unallocatedStrategyBaskets; }
            set { _unallocatedStrategyBaskets = value; }
        }

        public override BasketGroupCollection Groupedbaskets
        {
            get { return _groupedbaskets; }
            set { _groupedbaskets = value; }
        }

        public override BasketGroupCollection AllocatedBasketGroups
        {
            get { return _allocatedBasketGroups; }
            set { _allocatedBasketGroups = value; }
        }

        public override void ProrataBasketAllocation(BasketGroup group,DateTime AUECLocalDate)
        {
            try
            {
                group.AllocatedQty = group.CumQty;
                FundStraegyManager.ProRataStrategies(group.Strategies, group.AllocatedQty);
                UnAllocateBasket(group);
                if (group.AddedBaskets.Count == 1)
                {
                    _groupedbaskets.Add(group);
                    foreach (BasketDetail basket in group.AddedBaskets)
                    {
                        _unallocatedStrategyBaskets.Remove(basket);
                    }
                    BasketAllocationDBManager.SaveBasketGroup(group, AUECLocalDate);
                }
                AllocateBasket(group, group.Strategies, AUECLocalDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
