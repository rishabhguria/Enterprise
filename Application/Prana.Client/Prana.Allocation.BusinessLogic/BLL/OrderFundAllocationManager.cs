using System;
using System.Collections.Generic;
using System.Text;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using System.ComponentModel;

namespace Prana.Allocation.BLL
{
   public class OrderFundAllocationManager : OrderAllocationManager
    {
         //int _userID = int.MinValue;
         AllocationOrderCollection _fundOrders = new AllocationOrderCollection();
         AllocationGroups _fundGroups = new AllocationGroups();
         AllocationGroups _fundAllocated = new AllocationGroups();
        static OrderFundAllocationManager _orderFundAllocationManager =null;
       private static object _lockerObject = new object();
       // By Abhishek on 04-Mar-2008 to diffrentiate from sinlge Trade
       AllocationGroups _fundAllocatedForBasket = new AllocationGroups();
        private OrderFundAllocationManager()
        { 

        }
        public static OrderFundAllocationManager GetInstance
        {
            get
            {

                lock (_lockerObject)
                {
                    if (_orderFundAllocationManager == null)
                    {
                        _orderFundAllocationManager = new OrderFundAllocationManager();
                    }
                    return _orderFundAllocationManager; 
                }
            }
        }
       public override void GroupOrders(AllocationOrderCollection selectedOrders, bool isBasketGroup, DateTime AUECLocalDate)
        {
            try
            {
                //BackgroundWorker bGWorkerGroupOrders = new BackgroundWorker();
               // bGWorkerGroupOrders.DoWork += new DoWorkEventHandler(bGWorkerGroupOrders_DoWork);
               

                AllocationGroup group = new AllocationGroup(PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);
                group.IsBasketGroup = isBasketGroup;
                group.CreateGroup(selectedOrders);
                if (!AUECLocalDate.Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
                {
                    group.AUECLocalDate = AUECLocalDate;
                }
                else
                {
                    group.AUECLocalDate = Prana.CommonDataCache.TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, DateTime.UtcNow);
                }
                
               // bGWorkerGroupOrders.RunWorkerAsync(group);
                // Save AllocationGroup 
               
                if (!group.IsBasketGroup)
                {
                    _fundGroups.Add(group);
                    _fundOrders.Remove(selectedOrders);
                }
                OrderAllocationDBManager.ChangeListOrdersState(selectedOrders, PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED, PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);
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
        //    OrderAllocationDBManager.SaveGroup(group);
        //}


       public override void AllocateOrder(AllocationOrder order, double allocateQty, object allocationFunds, bool isProrata, string baskeGroupID, DateTime AUECLocalDate)
        {
            try
            {
                BackgroundWorker bGWorkerAllocateOrder = new BackgroundWorker();
                //bGWorkerAllocateOrder.DoWork += new DoWorkEventHandler(bGWorkerAllocateOrder_DoWork);


                AllocationFunds funds = (AllocationFunds)allocationFunds;
                AllocationGroup group = new AllocationGroup(PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);
                group.IsProrataActive = isProrata;
                group.BasketGroupID = baskeGroupID;
                if (group.BasketGroupID != string.Empty)
                {
                    group.IsBasketGroup = true;
                }
                group.CreateGroup(order);
                group.AllocateGroup(allocateQty, funds, isProrata);
                //group.AUECLocalDate = AUECLocalDate;
                if (!AUECLocalDate.Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
                {
                    group.AUECLocalDate = AUECLocalDate;//TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, DateTime.UtcNow);
                }
                else
                {
                 group.AUECLocalDate = Prana.CommonDataCache.TimeZoneHelper.GetAUECLocalDateFromUTC(group.AUECID, DateTime.UtcNow);
                } 
                if (group.BasketGroupID ==string.Empty)
                {
                    _fundOrders.Remove(order);
                    _fundAllocated.Add(group);
                }
                else
                {
                    group.Commission = order.Commission;
                    group.Fees = order.Fees;
                    //group.AllocationFunds = (AllocationFunds)allocationFunds;
                    _fundAllocatedForBasket.Add(group);
                }

                CommissionCalculator commissionCalculator = new CommissionCalculator();
                if (group.BasketGroupID == string.Empty)
                {
                    commissionCalculator.StartCalculation(group);
                }
                //else
                //{
                //    commissionCalculator.StartCalculation(_fundAllocatedForBasket);
                   
                //}
                
               
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

       public override void AllocateGroup(AllocationGroup group, double allocateQty, object allocationFunds, bool isProrata, DateTime AUECLocalDate)
        {
            //BackgroundWorker bGWorkerAllocateGroup = new BackgroundWorker();
            //bGWorkerAllocateGroup.DoWork += new DoWorkEventHandler(bGWorkerAllocateGroup_DoWork);
            AllocationFunds funds = (AllocationFunds)allocationFunds;
            try
            {
                group.AllocateGroup(allocateQty, funds, isProrata);
                //group.AUECLocalDate = AUECLocalDate;
                if (!group.IsBasketGroup)
                {
                    _fundGroups.Remove(group);
                    _fundAllocated.Add(group);
                }
              

                CommissionCalculator commissionCalculator = new CommissionCalculator();
               
                    commissionCalculator.StartCalculation(group);
                
                OrderAllocationDBManager.ChangeGroupState(group,AUECLocalDate);
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

        //void bGWorkerAllocateGroup_DoWork(object sender, DoWorkEventArgs e)
        //{
        //   // AllocationGroup group = e.Argument as AllocationGroup;
        //   // OrderAllocationDBManager.ChangeGroupState(group);
        //}

       public void AllocateBasketGroup(BasketGroup basketGroup, AllocationFunds funds, DateTime AUECLocalDate)
        {
            try
            {
                if (basketGroup.AddedBaskets.Count == 1)
                {
                    CommissionCalculator commissionCalculator = new CommissionCalculator();
                    AllocationOrderCollection allocationOrderCollection = new AllocationOrderCollection();
                    allocationOrderCollection = commissionCalculator.StartCalculationforBasket(basketGroup);
                    //OrderCollection basketOrders = basketGroup.AddedBaskets[0].BasketOrders;
                    foreach (AllocationOrder allocationOrder in allocationOrderCollection)
                    {
                       
                        //allocationOrder.AllocationFunds = orderFunds;
                        AllocateOrder(allocationOrder, allocationOrder.CumQty, allocationOrder.AllocationFunds, false, basketGroup.BasketGroupID, AUECLocalDate);
                    }

                }
                else
                {
                     CommissionCalculator commissionCalculator = new CommissionCalculator();
                     AllocationOrderCollection allOrdersinBasketGroup = new AllocationOrderCollection();
                     allOrdersinBasketGroup = commissionCalculator.StartCalculationforBasket(basketGroup);
                     //AllocationGroups allocationGroups = AutoGroupOrders(allOrdersinBasketGroup, AllocationPreferencesManager.AllocationPreferences, basketGroup.BasketGroupID, AUECLocalDate);
                     //foreach (AllocationGroup group in allocationGroups)
                     //{


                       //  AllocateGroup(group, group.CumQty, funds, true, AUECLocalDate);
                     //}

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
                        _fundGroups.Remove(group);
                        _fundOrders.Add(group.Orders);
                    }
                    else if (group.State == PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        if (group.SingleOrderAllocation)
                        {
                            OrderAllocationDBManager.RemoveGroup(group);
                            _fundAllocated.Remove(group);
                            //foreach (AllocationOrder allocationOrder in _fundOrders)
                            //{
                            //    allocationOrder.AUECLocalDate = Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue;
                            //}
                            _fundOrders.Add(group.Orders);
                        }
                        else
                        {
                            group.UnAllocateGroup();
                            OrderAllocationDBManager.ChangeGroupState(group, auecLocaldate);
                            _fundAllocated.Remove(group);
                            _fundGroups.Add(group);
                        }
                    }

                    OrderAllocationDBManager.ChangeListOrdersState(group.Orders, PranaInternalConstants.ORDERSTATE_ALLOCATION.UNALLOCATED, PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);

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
        //    try
        //    {
        //        AllocationGroups groups = e.Argument as AllocationGroups;
        //        foreach (AllocationGroup group in groups)
        //        {
        //            if (group.State == PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED)
        //            {
        //                OrderAllocationDBManager.RemoveGroup(group);
        //            }
        //            else if (group.State == PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
        //            {
        //                if (group.SingleOrderAllocation)
        //                {
        //                    OrderAllocationDBManager.RemoveGroup(group);
        //                }
        //                else
        //                {
        //                    OrderAllocationDBManager.ChangeGroupState(group);
        //                }
        //            }
        //        }

                
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

       public override AllocationGroups AutoGroupOrders(AllocationOrderCollection orders, AllocationPreferences _allocationPreferences, string basketGroupID, DateTime AUECLocalDate)
        {
            AllocationGroups allocationgroups = new AllocationGroups();
            try
            {

                
                if (basketGroupID == string.Empty)
                {
                    allocationgroups = _fundGroups;
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
                        //Date transactionTime = order.AUECLocalDate;
                        DateTime dt = order.AUECLocalDate;//DateTime.ParseExact(transactionTime, Prana.Utilities.DateTimeUtilities.DateTimeConstants.NirvanaDateTimeFormat, null);

                        //string openClose  = order.openc;
                        if ((group.AutoGrouped)
                        && (group.OrderSide.Equals(side) || ((bBuyAndBCV) && ((side.Equals("Buy") && order.OrderSide.Equals("BCV")) || (side.Equals("Buy") && order.OrderSide.Equals("BCV")))))
                        && (group.Symbol.Equals(symbol))
                        && (group.AUECLocalDate.Date.Equals(dt.Date))
                        && ((!bCounterParty) || (bCounterParty && group.CounterPartyName.Equals(counterParty)))

                        && ((!bVenue) || (bVenue && group.Venue.Equals(venue)))
                        && ((!bTradingAccount) || (bTradingAccount && group.TradingAccountName.Equals(tradingAccountName))))
                        {
                            matchFound = true;
                           
                            changedGroups.Add(group);
                            group.AddOrder(order);
                            break;
                        }
                    }
                    if (!matchFound)
                    {
                        AllocationGroup tempgroup = new AllocationGroup(PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);
                        //tempgroup.TransactionTime = order.TransactionTime;
                        tempgroup.BasketGroupID = basketGroupID;
                        if (tempgroup.BasketGroupID != string.Empty)
                        {
                            tempgroup.IsBasketGroup = true;
                        }
                        tempgroup.AddOrder(order);
                        tempgroup.AutoGrouped = true;
                        tempgroup.AUECLocalDate =   order.AUECLocalDate;
                        allocationgroups.Add(tempgroup);
                     
                       OrderAllocationDBManager.SaveGroup(tempgroup, AUECLocalDate);
                        
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
                    OrderAllocationDBManager.ChangeListOrdersState(group.Orders, PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED, PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);
                }

                if (basketGroupID == string.Empty)
                {
                    foreach (AllocationOrder order in orders)
                    {
                        _fundOrders.Remove(order);
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
            return allocationgroups;
        }

        public override void ProrataAllocatedGroup(AllocationGroup group)
        {
            try
            {
                //BackgroundWorker bGWorkerProrataAllocatedGroup = new BackgroundWorker();
               // bGWorkerProrataAllocatedGroup.DoWork += new DoWorkEventHandler(bGWorkerProrataAllocatedGroup_DoWork);
                group.AllocatedQty = group.CumQty;
                group.Updated = false;
               // bGWorkerProrataAllocatedGroup.RunWorkerAsync(group);
                
                FundStraegyManager.ProRataFunds(group.AllocationFunds, group.AllocatedQty);
                CommissionCalculator commissionCalculator = new CommissionCalculator();
                commissionCalculator.StartCalculation(group);
                OrderAllocationDBManager.UpdateGroup(group, false);
                OrderAllocationDBManager.ModifyAllocatedFundsForProrata(group.AllocationFunds);
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
        //    OrderAllocationDBManager.UpdateGroup(group, false);
        //    FundStraegyManager.ProRataFunds(group.AllocationFunds, group.AllocatedQty);
        //    OrderAllocationDBManager.ModifyAllocatedFundsForProrata(group.AllocationFunds);
        //}

        /// <summary>
        /// For Checking in  Funds Related Groups , Orders and Allocated Funds
        /// </summary>
        /// <param name="fundOrders"></param>
        ///
       //public AllocationGroups GetUpdatedGroups(Dictionary<string, bool> _groupID)
       //{
       //    try
       //    {
       //        string s = "";
       //        foreach ( Dictionary<string, bool>.KeyCollection groupID in _groupID)
       //        {
       //            s += groupID[0] + ",";

       //        }
       //    }
       //    catch (Exception ex)
       //    {

       //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDSHOW);

       //        if (rethrow)
       //        {
       //            throw;
       //        }
       //    }
       //}
       
       //public void CheckUpdatedFundOrdersLocation(AllocationOrderCollection fundOrders)
       // {
       //     try
       //     {
       //         AllocationOrder oldOrder = null;
       //         AllocationGroup updatedGroup = null;
       //         string groupID = string.Empty;

       //         foreach (AllocationOrder order in fundOrders)
       //         {

       //              No Seperate Rows for Cancel And Replace Orders  , it should be merged with main Order
       //             if (order.OrigClOrderID != int.MinValue.ToString())
       //             {
       //                 order.ClOrderID = order.OrigClOrderID;
       //             }

       //              Updated Order Found in Allocated Orders
       //             if ((groupID = _fundAllocated.ContainsOrder(order.ClOrderID)) != string.Empty)
       //             {
       //                 updatedGroup = _fundAllocated.GetGroup(groupID);
       //                 updatedGroup.Update(order);
       //             }

       //             Updated Order Found in Grouped Orders
       //             else if ((groupID = _fundGroups.ContainsOrder(order.ClOrderID)) != string.Empty)
       //             {
       //                 updatedGroup = _fundGroups.GetGroup(groupID);
       //                 updatedGroup.Update(order);
       //             }
       //              Updated Order Found in Old Orders
       //             else if (_fundOrders.ContainsOrder(order.ClOrderID))
       //             {

       //                 oldOrder = _fundOrders.GetOrder(order.ClOrderID);
       //                 oldOrder.Update(order);
       //             }
       //               // New  Pre Allocated Order Addition
       //             else if (order.FundID != int.MinValue)
       //             {

       //                 _fundOrders.Add(order);

       //                 //AllocationOrderCollection orders = new AllocationOrderCollection();
       //                 //orders.Add(order);
       //                 //AllocationGroup group = new AllocationGroup(orders);
       //                 //group.AllocationType = PranaInternalConstants.TYPE_OF_ALLOCATION.FUND;
       //                 //group.GroupID = OrderAllocationDBManager.GetPreAllocatedOrderGroupID(order.ClOrderID, (int)group.AllocationType);
       //                 //group.SingleOrderAllocation = true;
       //                 //group.IsPreAllocated = true;
       //                 //AllocationFunds funds = new AllocationFunds();
       //                 //AllocationFund fund = new AllocationFund(order.FundID, string.Empty, order.CumQty, 100);

       //                 //funds.Add(fund);
       //                 //group.AllocateGroup(order.CumQty, funds, true);
       //                 //_fundAllocated.Add(group);
       //             }
       //              New Order Addition
       //             else
       //             {
       //                 if (order.FundID == int.MinValue)
       //                 {
       //                     _fundOrders.Add(order);
       //                 }

       //             }
       //         }
       //     }
       //     catch (Exception ex)
       //     {

       //         bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

       //         if (rethrow)
       //         {
       //             throw;
       //         }
       //     }
       // }
        public override AllocationOrderCollection Orders
        {
            get { return _fundOrders; }
            set  {  _fundOrders =value; }
        }
        public override AllocationGroups Groups
        {
            get { return _fundGroups; }
            set { _fundGroups = value; }
        }
        public override AllocationGroups    AllocatedGroups
        {
            get { return _fundAllocated; }
            set { _fundAllocated = value; }
        }
    }
}
