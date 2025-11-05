using System;
using System.Collections.Generic;
using System.Text;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using System.ComponentModel;

namespace Prana.Allocation.BLL
{
    public class OrderStrategyAllocationManager : OrderAllocationManager
    {
        //static int _userID = int.MinValue;
        static AllocationOrderCollection _strategyOrders = new AllocationOrderCollection();
        static AllocationGroups _strategyGroups = new AllocationGroups();
        static AllocationGroups _strategyAllocated = new AllocationGroups();

        static OrderStrategyAllocationManager _orderStrategyAllocationManager = null;
        private OrderStrategyAllocationManager()
        { 

        }
        public static OrderStrategyAllocationManager GetInstance
        {
            get
            {

                if (_orderStrategyAllocationManager == null)
                {
                    _orderStrategyAllocationManager = new OrderStrategyAllocationManager();
                }
                return _orderStrategyAllocationManager;
            }
        }
        public override void GroupOrders(AllocationOrderCollection selectedOrders, bool isBasketGroup, DateTime AUECLocalDate)
        {
            try
            {
               // BackgroundWorker bGWorkerGroupOrders = new BackgroundWorker();
                //bGWorkerGroupOrders.DoWork += new DoWorkEventHandler(bGWorkerGroupOrders_DoWork);

                AllocationGroup group = new AllocationGroup(PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY);
                group.CreateGroup(selectedOrders);
                group.AUECLocalDate = AUECLocalDate;
               
                if (!group.IsBasketGroup)
                {
                    _strategyGroups.Add(group);
                    _strategyOrders.Remove(selectedOrders);
                }

                //bGWorkerGroupOrders.RunWorkerAsync(group);
                OrderAllocationDBManager.ChangeListOrdersState(selectedOrders, PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED, PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY);
                OrderAllocationDBManager.SaveGroup(group,AUECLocalDate);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        //void bGWorkerGroupOrders_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    AllocationGroup group = e.Argument as AllocationGroup;
        //    // Save AllocationGroup 
        //    OrderAllocationDBManager.SaveGroup(group);
        //}

        public override void AllocateOrder(AllocationOrder order, double allocateQty, object allocationStrategies, bool isProrata, string basketGroupID, DateTime AUECLocalDate)
        {
            try
            {
                //BackgroundWorker bGWorkerAllocateOrder = new BackgroundWorker();
               // bGWorkerAllocateOrder.DoWork += new DoWorkEventHandler(bGWorkerAllocateOrder_DoWork);
                AllocationStrategies strategies=(AllocationStrategies)allocationStrategies;
                AllocationGroup group = new AllocationGroup(PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY);
                group.IsProrataActive = isProrata;
                group.BasketGroupID = basketGroupID;
                if (group.BasketGroupID != string.Empty)
                {
                    group.IsBasketGroup = true;
                }
                group.CreateGroup(order);
                group.AllocateGroup(allocateQty, strategies, isProrata);
                if (!AUECLocalDate.Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
                {
                    group.AUECLocalDate = AUECLocalDate;//TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, DateTime.UtcNow);
                }
                else
                {
                    group.AUECLocalDate = Prana.CommonDataCache.TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, DateTime.UtcNow);
                } 
               // group.AUECLocalDate = AUECLocalDate;
                if (group.BasketGroupID == string.Empty)
                {
                    _strategyOrders.Remove(order);
                    _strategyAllocated.Add(group);
                }
               // bGWorkerAllocateOrder.RunWorkerAsync(group);
                OrderAllocationDBManager.SaveGroup(group,AUECLocalDate);
                OrderAllocationDBManager.ChangeGroupState(group,AUECLocalDate);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        //void bGWorkerAllocateOrder_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    AllocationGroup group = e.Argument as AllocationGroup;
        //    OrderAllocationDBManager.SaveGroup(group);
        //    OrderAllocationDBManager.ChangeGroupState(group);
        //}

        public override AllocationGroups AutoGroupOrders(AllocationOrderCollection orders, AllocationPreferences _allocationPreferences, string basketGroupID, DateTime AUECLocalDate)
        {
            AllocationGroups allocationgroups = new AllocationGroups();
            if (basketGroupID == string.Empty)
            {
                allocationgroups = _strategyGroups;
            }
            bool bCounterParty = _allocationPreferences.AutoGroupingRules.CounterParty;
            bool bVenue = _allocationPreferences.AutoGroupingRules.Venue;
            bool bTradingAccount = _allocationPreferences.AutoGroupingRules.TradingAccount;
            bool bBuyAndBCV = _allocationPreferences.AutoGroupingRules.BuyAndBCV;
            bool matchFound = false;
            AllocationGroups changedGroups = new AllocationGroups();
            foreach (AllocationOrder order in orders)
            {
                matchFound = false;
                foreach (AllocationGroup group in allocationgroups)
                {
                    string side = order.OrderSide.Trim();
                    string symbol = order.Symbol;
                    string counterParty = order.CounterPartyName;
                    string venue = order.Venue;
                    string tradingAccountName = order.TradingAccountName;
                    //string transactionTime = order.AUECLocalDate;
                    DateTime dt = order.AUECLocalDate;//DateTime.ParseExact(transactionTime, Prana.Utilities.DateTimeUtilities.DateTimeConstants.NirvanaDateTimeFormat, null);
                    if ((group.AutoGrouped)
                    && (group.OrderSide.Equals(side) || ((bBuyAndBCV) && ((side.Equals("Buy") && order.OrderSide.Equals("BCV")) || (side.Equals("Buy") && order.OrderSide.Equals("BCV")))))
                    && (group.Symbol.Equals(symbol))
                    && (group.AUECLocalDate.Equals(dt.Date))
                    && ((!bCounterParty) || (bCounterParty && group.CounterPartyName.Equals(counterParty)))

                    && ((!bVenue) || (bVenue && group.Venue.Equals(venue)))
                    && ((!bTradingAccount) || (bTradingAccount && group.TradingAccountName.Equals(tradingAccountName))))
                    {
                        matchFound = true;
                        group.AddOrder(order);
                        changedGroups.Add(group);
                        break;
                    }
                }
                if (!matchFound)
                {
                    AllocationGroup tempgroup = new AllocationGroup(PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY);
                    tempgroup.BasketGroupID = basketGroupID;
                    //tempgroup.TransactionTime = order.TransactionTime;
                    if (tempgroup.BasketGroupID != string.Empty)
                    {
                        tempgroup.IsBasketGroup = true;
                    }
                    tempgroup.AddOrder(order);
                    tempgroup.AutoGrouped = true;
                    tempgroup.AUECLocalDate = order.AUECLocalDate;
                    allocationgroups.Add(tempgroup);
                    OrderAllocationDBManager.SaveGroup(tempgroup, AUECLocalDate);
                }
            }
            if (basketGroupID == string.Empty)
            {
                foreach (AllocationOrder order in orders)
                {
                    _strategyOrders.Remove(order);
                }
            }
            foreach (AllocationGroup group in changedGroups)
            {
                if (AUECLocalDate.Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
                {
                    group.AUECLocalDate = Prana.CommonDataCache.TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, DateTime.UtcNow);
                }
                else
                {
                    group.AUECLocalDate = AUECLocalDate;
                } 
                OrderAllocationDBManager.UpdateGroup(group, true);
                OrderAllocationDBManager.ChangeListOrdersState(group.Orders, PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED, PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY);
            }
            return allocationgroups;
        }

        public override void AllocateGroup(AllocationGroup group, double allocateQty, object allocationStrategies, bool isProrata, DateTime AUECLocalDate)
        {
            //BackgroundWorker bGWorkerAllocateGroup = new BackgroundWorker();
           // bGWorkerAllocateGroup.DoWork += new DoWorkEventHandler(bGWorkerAllocateGroup_DoWork);

            AllocationStrategies strategies = (AllocationStrategies)allocationStrategies;
            try
            {
                //group.AUECLocalDate = AUECLocalDate;
                if (!group.IsBasketGroup)
                {
                    _strategyGroups.Remove(group);
                    _strategyAllocated.Add(group);
                }
                group.AllocateGroup(allocateQty, strategies, isProrata);
              //  bGWorkerAllocateGroup.RunWorkerAsync(group);
                OrderAllocationDBManager.ChangeGroupState(group, AUECLocalDate);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            //}
            //// Save to Db

            //groups = new AllocationGroups();


        }

        //void bGWorkerAllocateGroup_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    AllocationGroup group = e.Argument as AllocationGroup;
        //    OrderAllocationDBManager.ChangeGroupState(group);
        //}

        public override void UnBundleGroup(AllocationGroups groups, DateTime auecLocaldate)
        {
            try
            {
               // BackgroundWorker bGWorkerUnBundleGroup = new BackgroundWorker();
               // bGWorkerUnBundleGroup.DoWork += new DoWorkEventHandler(bGWorkerUnBundleGroup_DoWork);
                foreach (AllocationGroup group in groups)
                {
                    if (group.State == PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED)
                    {
                        OrderAllocationDBManager.RemoveGroup(group);
                        _strategyGroups.Remove(group);
                        _strategyOrders.Add(group.Orders);
                        OrderAllocationDBManager.RemoveGroup(group);
                    }
                    else if (group.State == PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        if (group.SingleOrderAllocation)
                        {
                            OrderAllocationDBManager.RemoveGroup(group);
                            _strategyAllocated.Remove(group);
                            _strategyOrders.Add(group.Orders);
                            OrderAllocationDBManager.RemoveGroup(group);
                        }
                        else
                        {
                            group.UnAllocateGroup();
                            OrderAllocationDBManager.ChangeGroupState(group, auecLocaldate);
                            _strategyAllocated.Remove(group);
                            _strategyGroups.Add(group);
                            OrderAllocationDBManager.ChangeGroupState(group, auecLocaldate);
                        }
                        OrderAllocationDBManager.ChangeListOrdersState(group.Orders, PranaInternalConstants.ORDERSTATE_ALLOCATION.UNALLOCATED, PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY);

                    }

                }
                //AllocationGroups newgroups = new AllocationGroups();
                //foreach (AllocationGroup group in groups)
                //{
                //    newgroups.Add(group);
                //}
                //bGWorkerUnBundleGroup.RunWorkerAsync(newgroups);

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        //void bGWorkerUnBundleGroup_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    AllocationGroups groups = e.Argument as AllocationGroups;
        //    foreach (AllocationGroup group in groups)
        //    {
        //        if (group.State == PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED)
        //        {
        //            OrderAllocationDBManager.RemoveGroup(group);
        //        }
        //        else if (group.State == PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
        //        {
        //            if (group.SingleOrderAllocation)
        //            {
        //                OrderAllocationDBManager.RemoveGroup(group);
        //            }
        //            else
        //            {
        //                OrderAllocationDBManager.ChangeGroupState(group);
        //            }
        //        }
        //    }
        //}
        public override void ProrataAllocatedGroup(AllocationGroup group)
        {
            try
            {
               // BackgroundWorker bGWorkerProrataAllocatedGroup = new BackgroundWorker();
               // bGWorkerProrataAllocatedGroup.DoWork += new DoWorkEventHandler(bGWorkerProrataAllocatedGroup_DoWork);
                group.AllocatedQty = group.CumQty;
                group.Updated = false;
              //  bGWorkerProrataAllocatedGroup.RunWorkerAsync(group);
                FundStraegyManager.ProRataStrategies(group.Strategies, group.AllocatedQty);
                OrderAllocationDBManager.UpdateGroup(group, false);
                OrderAllocationDBManager.ModifyAllocatedStraegiesForProrata(group.Strategies);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        //void bGWorkerProrataAllocatedGroup_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    AllocationGroup group = e.Argument as AllocationGroup;
        //    FundStraegyManager.ProRataStrategies(group.Strategies, group.AllocatedQty);
        //    OrderAllocationDBManager.UpdateGroup(group, false);
        //    OrderAllocationDBManager.ModifyAllocatedStraegiesForProrata(group.Strategies);
        //}

        public void AllocateBasketGroup(BasketGroup basketGroup, AllocationStrategies strategies, DateTime AUECLocalDate)
        {
            try
            {
                if (basketGroup.AddedBaskets.Count == 1)
                {
                    OrderCollection basketOrders = basketGroup.AddedBaskets[0].BasketOrders;
                    foreach (Order order in basketOrders)
                    {
                        AllocationOrder allocationOrder = OrderAllocationManager.GetAllocationOrder(order);
                        AllocateOrder(allocationOrder, allocationOrder.CumQty, strategies, false, basketGroup.BasketGroupID, AUECLocalDate);
                    }
                }
                else
                {
                    AllocationOrderCollection allOrdersinBasketGroup = new AllocationOrderCollection();
                    foreach (BasketDetail basket in basketGroup.AddedBaskets)
                    {
                        foreach (Order basketOrder in basket.BasketOrders)
                        {
                            allOrdersinBasketGroup.Add(OrderAllocationManager.GetAllocationOrder(basketOrder));
                        }
                    }
                    AllocationGroups allocationGroups = AutoGroupOrders(allOrdersinBasketGroup, AllocationPreferencesManager.AllocationPreferences, basketGroup.BasketGroupID,AUECLocalDate );
                    foreach (AllocationGroup group in allocationGroups)
                    {
                        AllocateGroup(group, group.CumQty, strategies , true,AUECLocalDate);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            //}
            // Save to Db

            //groups = new AllocationGroups();


        }

        /// <summary>
        ///  For Checking in  Strategy Related Groups , Orders and Allocated Funds
        /// </summary>
        /// <param name="strategyOrders"></param>
        //public void CheckUpdatedStrategyOrdersLocation(AllocationOrderCollection strategyOrders)
        //{
        //    try
        //    {
        //        AllocationOrder oldOrder = null;
        //        AllocationGroup updatedGroup = null;
        //        string groupID = string.Empty;
        //        foreach (AllocationOrder order in strategyOrders)
        //        {
        //            // Updated Order in Allocated Group
        //            if ((groupID = _strategyAllocated.ContainsOrder(order.ClOrderID)) != string.Empty)
        //            {
        //                updatedGroup = _strategyAllocated.GetGroup(groupID);
        //                updatedGroup.Update(order);
        //            }
        //            // Updated Order in Grouped Groyp
        //            else if ((groupID = _strategyGroups.ContainsOrder(order.ClOrderID)) != string.Empty)
        //            {
        //                updatedGroup = _strategyGroups.GetGroup(groupID);
        //                updatedGroup.Update(order);
        //            }
        //            // Updated Order in Old Order
        //            else if (_strategyOrders.ContainsOrder(order.ClOrderID))
        //            {
        //                oldOrder = _strategyOrders.GetOrder(order.ClOrderID);
        //                oldOrder.Update(order);
        //            }
        //            // New Order Addition PreAllocation
        //            //else if (order.StrategyID != int.MinValue)
        //            //{
        //            //    AllocationOrderCollection orders = new AllocationOrderCollection();
        //            //    orders.Add(order);
        //            //    AllocationGroup group = new AllocationGroup(orders);
        //            //    group.AllocationType = PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY;
        //            //    group.GroupID = OrderAllocationDBManager.GetPreAllocatedOrderGroupID(order.ClOrderID, (int)group.AllocationType);
        //            //    // group.GroupID = order.GroupID;
        //            //    group.SingleOrderAllocation = true;
        //            //    group.IsPreAllocated = true;
        //            //    AllocationStrategies strategies = new AllocationStrategies();
        //            //    AllocationStrategy strategy = new AllocationStrategy(order.StrategyID, string.Empty, order.CumQty, 100);
        //            //    strategies.Add(strategy);
        //            //    group.AllocateGroup(order.CumQty, strategies, true);
        //            //    _strategyAllocated.Add(group);
        //            //}
        //            // New Order Addition
        //            else
        //            {
        //                if (order.StrategyID == int.MinValue)
        //                {

        //                    _strategyOrders.Add(order);
        //                }

        //            }
        //        }



        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        public override AllocationOrderCollection Orders
        {
            get { return _strategyOrders; }
            set { _strategyOrders = value; }
        }

        public override AllocationGroups Groups
        {
            get { return _strategyGroups; }
            set{ _strategyGroups=value ; }
        }

        public override AllocationGroups AllocatedGroups
        {
            get { return _strategyAllocated; }
            set { _strategyAllocated = value; }
        }
    }
}
