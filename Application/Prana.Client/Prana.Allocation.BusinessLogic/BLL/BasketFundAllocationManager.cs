using System;
using System.Collections.Generic;
using System.Text;
using Prana.Global;
using Prana.BusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
namespace Prana.Allocation.BLL
{
    public class BasketFundAllocationManager : BasketAllocationManager 
    {
        string _lastBasketIDReceived = string.Empty;
        int _userID = int.MinValue;
        Int64 _lastID = 0;
        OrderFundAllocationManager _orderAllocationManager = OrderFundAllocationManager.GetInstance;
        static BasketFundAllocationManager _basketFundAllocationManager = null;
        
        private static Dictionary<string, Prana.BusinessObjects.Order> _fundBasketOrderIDValueCollection = new Dictionary<string, Prana.BusinessObjects.Order>();
        private static Dictionary<string, Prana.BusinessObjects.BasketDetail> _fundBasketIDValueCollection = new Dictionary<string, Prana.BusinessObjects.BasketDetail>();
        private Int64 _lastOrderSeqNumber = 0;
        BasketCollection _unallocatedFundBaskets = new BasketCollection();
        BasketGroupCollection _groupedbaskets = new BasketGroupCollection();
        BasketGroupCollection _allocatedBasketGroups = new BasketGroupCollection();
        private static object _lockerOject = new object();
        CommissionCalculator commissionCalculator = new CommissionCalculator();
        AllocationOrderCollection _allocationOrderCollection = null;
        private BasketFundAllocationManager()
        {

        }
        public static BasketFundAllocationManager GetInstance
        {
            get
            {
                lock (_lockerOject)
                {
                    if (_basketFundAllocationManager == null)
                    {
                        _basketFundAllocationManager = new BasketFundAllocationManager();
                    }
                    return _basketFundAllocationManager;
                }
            }
        }

        public override void Initilize(int userID, string AllAUECDatesString)
        {
            //SK Clear all clollectrions as we are not going to cache orders and baskets
            _unallocatedFundBaskets = new BasketCollection();
            _groupedbaskets = new BasketGroupCollection();
            _allocatedBasketGroups = new BasketGroupCollection();

            _fundBasketIDValueCollection = new Dictionary<string, BasketDetail>();
            _fundBasketOrderIDValueCollection = new Dictionary<string, Order>();
            _userID = userID;
            //Int64 maxSeqNumber = OrderDataManager.GetMaxSeqNumber();
            string[] arrAuecDates = AllAUECDatesString.Split(Seperators.SEPERATOR_6);
            string[] Dates = arrAuecDates[0].Split(Seperators.SEPERATOR_5);
            DateTime currentDate = Convert.ToDateTime(Dates[1]);
            BasketCollection unallocatedFundBaskets = BasketAllocationDBManager.GetUnAllocatedBasketDetails(_userID, currentDate, (int)PranaInternalConstants.TYPE_OF_ALLOCATION.FUND, _lastID);
            AddBaskets(unallocatedFundBaskets);
            BasketGroupCollection basketGroups = BasketAllocationDBManager.GetBasketGroups(AllAUECDatesString, (int)PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);
           
           
            foreach (BasketGroup basketGroup in basketGroups)
            {
                AddBasketOrders(basketGroup);
            }


            foreach (BasketGroup basketGroup in basketGroups)
                {
                    if (basketGroup.GroupState.Equals(PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED))
                    {
                        _allocatedBasketGroups.Add(basketGroup);
                    }
                    else
                    {
                        _groupedbaskets.Add(basketGroup);
                    }
                   
            }
            AllocationFunds allocationFunds = FundStraegyManager.GetBasketAllocatedFunds(AllAUECDatesString);
              foreach (AllocationFund fund in allocationFunds)
                {
                    BasketGroup group = _allocatedBasketGroups.GetBasketGroup(fund.GroupID);

                    if (group.AllocationFunds == null)
                    {
                        group.AllocationFunds = new AllocationFunds();
                    }

                    group.AllocationFunds.Add(fund);
                    group.Commission += fund.Commission;
                    group.Fees += fund.Fees;
                } 
      
        }               

        //public override  void UpdateBaskets(DateTime date)
        //{
        //    Prana.BusinessObjects.BasketCollection updatedUnAllocatedFundBaskets = BasketAllocationDBManager.GetUnAllocatedBasketDetails(_userID, date, (int)PranaInternalConstants.TYPE_OF_ALLOCATION.FUND,_lastID);
        //    if (updatedUnAllocatedFundBaskets.Count > 0)
        //    {
        //        foreach (BasketDetail basket in updatedUnAllocatedFundBaskets)
        //        {
        //            //_unallocatedFundBaskets.Add(basket);
        //            AddBasket(basket);

        //        }
        //        //_lastID = Int64.Parse(updatedUnAllocatedFundBaskets[updatedUnAllocatedFundBaskets.Count - 1].TradedBasketID);
        //    }

        //    //Int64 maxSeqNumber =OrderDataManager.GetMaxSeqNumber();
        //    OrderCollection updatedOrders = BasketDataManager.GetUpdatedFundBasketOrders(_lastOrderSeqNumber);
        //    foreach (Order order in updatedOrders)
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

        public override void AllocateBasket(BasketDetail basket, object allocationfunds, DateTime AUECLocalDate)
        {
            AllocationFunds funds = (AllocationFunds)allocationfunds;
            BasketCollection baskets = new BasketCollection();
            baskets.Add(basket);
            BasketGroup basketGroup = GroupBaskets(baskets, AUECLocalDate);
            AllocateBasket(basketGroup, funds, AUECLocalDate);
            //Prana.BusinessObjects.OrderCollection ordercollection = basketGroup.AddedBaskets[0].BasketOrders;
            //foreach (Order  order  in ordercollection)
            //{
            //    AllocationOrder allocationOrder = OrderAllocationManager.GetAllocationOrder(order);
            //    allocationOrder.AllocateGroupToFund(funds, allocationOrder.AllocatedQty); 
            //}
          
        }

        public override void AllocateBasket(BasketGroup basketGroup, object allocationfunds, DateTime AUECLocalDate)
        {
            AllocationFunds funds = (AllocationFunds)allocationfunds;
            basketGroup.AllocationType = PranaInternalConstants.TYPE_OF_ALLOCATION.FUND;
            basketGroup.GroupState = PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED;
            basketGroup.AllocateGroupToFund(funds, basketGroup.CumQty);
            if (_groupedbaskets.Contains(basketGroup))
            {
                _groupedbaskets.Remove(basketGroup);
            }
            _allocatedBasketGroups.Add(basketGroup);
            // call the commission and fee calculator
         
            //Persist It
            BasketAllocationDBManager.SaveAllocatedBasket(basketGroup);
            _orderAllocationManager.AllocateBasketGroup(basketGroup, funds, AUECLocalDate);
        }

        public override BasketGroup GroupBaskets(Prana.BusinessObjects.BasketCollection baskets, DateTime date)
        {
            BasketGroup basketGroup = new BasketGroup();
            basketGroup.AllocationType = PranaInternalConstants.TYPE_OF_ALLOCATION.FUND ;
            basketGroup.AUECLocalDate = date;
            basketGroup.AUECID = baskets[0].BasketOrders[0].AUECID;
            basketGroup.GroupState = PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED;
            basketGroup.AddBaskets(baskets);
            _groupedbaskets.Add(basketGroup);

            foreach (BasketDetail basket in baskets)
            {
                _unallocatedFundBaskets.Remove(basket);
            }
            BasketAllocationDBManager.SaveBasketGroup(basketGroup, date);
            return basketGroup;
        }

        public override void UnAllocateBasket(BasketGroup  basketGroup)
        {
            basketGroup.GroupState = PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED;
            basketGroup.IsCommissionCalculated = false;
            _allocatedBasketGroups.Remove(basketGroup);
            if (basketGroup.AddedBaskets.Count == 1)
            {
                _unallocatedFundBaskets.Add(basketGroup.AddedBaskets[0]);
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
                _unallocatedFundBaskets.Add(basket);

            }
            _groupedbaskets.Remove(basketGroup);
            BasketAllocationDBManager.DeleteBasketGroup(basketGroup);
        }

        public override void UnBundleBasket(Prana.BusinessObjects.BasketDetail basket)
        {
            _unallocatedFundBaskets.Remove(basket);
            AllocationOrderCollection orders = new AllocationOrderCollection();
            foreach (Order order in basket.BasketOrders)
            {
                AllocationOrder unAllocatedOrder = OrderAllocationManager.GetAllocationOrder(order);
                unAllocatedOrder.TradingAccountID = basket.TradingAccountID;
                unAllocatedOrder.TradingAccountName = basket.TradingAccount;
                if (order.ParentClOrderID != string.Empty)
                {
                    orders.Add(unAllocatedOrder);
                }
            }

        }
        // For AllocationFunds 
        private void AddBaskets(BasketCollection baskets)
        {
            if (baskets.Count > 0)
            {

                foreach (Prana.BusinessObjects.BasketDetail basketToAdd in baskets)
                {

                    AddBasket(basketToAdd);
                }
            }

        }

        private void AddBasket(Prana.BusinessObjects.BasketDetail basketToAdd)
        {
            if (basketToAdd.CumQty > 0)
            {
                _unallocatedFundBaskets.Add(basketToAdd);
                _fundBasketIDValueCollection.Add(basketToAdd.TradedBasketID, basketToAdd);
                AddBasketOrders(basketToAdd.BasketOrders);
            }

        }

        public  bool IsBasketExist(string TradedBasketID)
        {
            if (_fundBasketIDValueCollection.ContainsKey(TradedBasketID))
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
                BasketDetail oldBasket = _fundBasketIDValueCollection[order.ListID.Trim()];
                oldBasket.BasketOrders.Add(order);
                oldBasket.Updated = true;
            }
            if (!_fundBasketOrderIDValueCollection.ContainsKey(order.ClientOrderID))
                _fundBasketOrderIDValueCollection.Add(order.ClientOrderID, order);
            //_lastOrderSeqNumber = order.OrderSeqNumber;
            
        }

        private void AddBasketOrders(BasketGroup basketGroup)
        { 
            foreach (BasketDetail basket in basketGroup.AddedBaskets )
            {
                AddBasketOrders(basket.BasketOrders);
            }
        }

        private  void AddBasketOrders(Prana.BusinessObjects.OrderCollection orders)
        {
            foreach (Prana.BusinessObjects.Order order in orders)
            {
                AddBasketOrder(order, false);
            }
        }

        private  bool IsOrderExist(string clOrderID)
        {
            if (_fundBasketOrderIDValueCollection.ContainsKey(clOrderID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //private  void UpdateBasketOrder(Prana.BusinessObjects.Order order)
        //{


        //    Prana.BusinessObjects.Order oldOrder = _fundBasketOrderIDValueCollection[order.ClientOrderID];
        //    oldOrder.CumQty = order.CumQty;
        //    oldOrder.AvgPrice = order.AvgPrice;
        //    oldOrder.Quantity = order.Quantity;
        //    oldOrder.OrderStatusTagValue = order.OrderStatusTagValue;
        //    oldOrder.OrderStatus = order.OrderStatus;
        //    UpdateOldBasket(order.ListID);

        //}

        //private void UpdateOldBasket(string listID)
        //{
        //    Prana.BusinessObjects.BasketDetail oldBasket = _fundBasketIDValueCollection[listID];
        //    double quantity = oldBasket.Quantity;
        //    double cumQty = oldBasket.CumQty;
        //    double exevalue = oldBasket.ExeValue;
        //    oldBasket.Updated = true;
        //}

        public  override BasketCollection UnallocatedBaskets
        {
            get { return _unallocatedFundBaskets; }
            set { _unallocatedFundBaskets = value; }
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

        public  void ClearAllData()
        {
            _fundBasketOrderIDValueCollection = new Dictionary<string, Prana.BusinessObjects.Order>();
            _fundBasketIDValueCollection = new Dictionary<string, Prana.BusinessObjects.BasketDetail>();
        }

        public override void ProrataBasketAllocation(BasketGroup basketGroup, DateTime AUECLocalDate)
        {
            try
            {
                basketGroup.AllocatedQty = basketGroup.CumQty;
                FundStraegyManager.ProRataFunds(basketGroup.AllocationFunds, basketGroup.AllocatedQty);
                UnAllocateBasket(basketGroup);
                if (basketGroup.AddedBaskets.Count == 1)
                {
                    _groupedbaskets.Add(basketGroup);
                    foreach (BasketDetail basket in basketGroup.AddedBaskets)
                    {
                        _unallocatedFundBaskets.Remove(basket);
                    }
                    BasketAllocationDBManager.SaveBasketGroup(basketGroup, AUECLocalDate);
                }
                AllocateBasket(basketGroup, basketGroup.AllocationFunds, AUECLocalDate);
                //TBD
                //BasketAllocationDBManager.ProRataBasketGroup(basketGroup);
                //AllocationGroups groups = FundStraegyManager.GetBasketAllocatedGroups(basketGroup.BasketGroupID);
                //foreach (AllocationGroup orderAllocationGroup in groups)
                //{
                //    FundStraegyManager.ProRataFunds(orderAllocationGroup.AllocationFunds, orderAllocationGroup.AllocatedQty);
                //    OrderAllocationDBManager.ModifyAllocatedFundsForProrata(orderAllocationGroup.AllocationFunds);
                //}
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
    }
}
