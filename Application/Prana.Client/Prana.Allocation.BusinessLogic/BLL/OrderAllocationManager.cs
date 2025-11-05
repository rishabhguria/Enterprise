using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using System.ComponentModel;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Utilities;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.Allocation.BLL
{
   public abstract class OrderAllocationManager
    {
         static int _userID = int.MinValue;
        static Int64 _lastOrderSeqNumber = 0;
        static Int64 _newOrderSeqNumber = 0;
       static string _groupID = string.Empty;
        static bool _alreadycachedData = false ;
       public abstract void AllocateOrder(AllocationOrder order, double allocateQty, object allocationFunds, bool isProrata, string basketGroupID, DateTime AUECLocalDate);

       public abstract void AllocateGroup(AllocationGroup group, double allocateQty, object allocationFunds, bool isProrata, DateTime AUECLocalDate);

       public abstract void GroupOrders(AllocationOrderCollection selectedOrders, bool isBasketGroup, DateTime AUECLocalDate);

        public abstract void UnBundleGroup(AllocationGroups groups, DateTime AUECLocaldate);
       public abstract AllocationGroups AutoGroupOrders(AllocationOrderCollection orders, AllocationPreferences allocationPreferences, string basketGroupID, DateTime AUECLocalDate);
       public static DateTime _timestamp = DateTime.UtcNow;

       public static void Initialise(CompanyUser loginUser,string AllAUECDatesString,bool allocationFormOpened)
        {
            try
            {
                //if (!_alreadycachedData || !date.Date.Equals(DateTime.UtcNow.Date) ) // if fresh Data
                if (!allocationFormOpened)
                {
                    _userID = loginUser.CompanyUserID;
                    OrderAllocationDBManager.UserID = _userID;
                    bool _commissionCalculationTime = false;
                    _commissionCalculationTime = OrderAllocationDBManager.GetCommissionCalculationTime();
                    // set the Commission Calculation Time Property
                    OrderAllocationDBManager.GetAllSavedCommissionRules();
                    CommissionRulesCacheManager.GetInstance().SetAllocatedCalculationProperty(_commissionCalculationTime);
                    OrderAllocationDBManager.GetAllCommissionRulesForCVAUEC(loginUser.CompanyID);
                    SetUnAllocatedOrders(AllAUECDatesString);
                    SetGroups(AllAUECDatesString);
                    _alreadycachedData = true;
                }
               
                    
                //}
                //else  // Cached Data 
                //{
                //    //AllocationOrderCollection updatedOrders =
                //    UpdateAllocation(date);
                //    //if (updatedOrders.Count > 0)
                //    //{
                //    //    OrderFundAllocationManager.GetInstance.CheckUpdatedFundOrdersLocation(updatedOrders);
                //    //    // For Getting Strategy Orders Clone Fund Orders Received from dataBase
                //    //    OrderStrategyAllocationManager.GetInstance.CheckUpdatedStrategyOrdersLocation(updatedOrders.Clone());
                //    //}
                //}
                
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }
            
        }

        private static void SetUnAllocatedOrders(string AllAUECDatesString)
        {
            try
            {
               // _lastOrderSeqNumber = 0;
                _newOrderSeqNumber = OrderDataManager.GetMaxSeqNumber();
                //if (_newOrderSeqNumber > _lastOrderSeqNumber || _newOrderSeqNumber==0)
                //{
                OrderFundAllocationManager.GetInstance.Orders = OrderAllocationDBManager.GetUnAllocatedOrders(_userID, AllAUECDatesString, PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);
                //OrderFundAllocationManager.GetInstance.Orders = OrderAllocationDBManager.GetUnAllocatedOrders(_userID, currentTime, PranaInternalConstants.TYPE_OF_ALLOCATION.FUND, _lastOrderSeqNumber);
                OrderStrategyAllocationManager.GetInstance.Orders = OrderAllocationDBManager.GetUnAllocatedOrders(_userID, AllAUECDatesString, PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY);
               // }
                _lastOrderSeqNumber = _newOrderSeqNumber;
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






       //public static void SettimeStamp(DateTime timestamp)
       //{
       //    _timestamp = timestamp;
       //}
       private static void SetGroups(string AllAUECDatesString)
        {


            try
            {

                //OrderFundAllocationManager.GetInstance.AllocatedGroups = new AllocationGroups();
                //OrderStrategyAllocationManager.GetInstance.AllocatedGroups = new AllocationGroups();
                //OrderFundAllocationManager.GetInstance.Groups = new AllocationGroups();
                //OrderStrategyAllocationManager.GetInstance.Groups = new AllocationGroups();
                OrderFundAllocationManager.GetInstance.AllocatedGroups.Clear();
                OrderStrategyAllocationManager.GetInstance.AllocatedGroups.Clear();
                OrderFundAllocationManager.GetInstance.Groups.Clear();
                OrderStrategyAllocationManager.GetInstance.Groups.Clear();

                AllocationGroups groups = OrderAllocationDBManager.GetGroups(AllAUECDatesString);
                foreach (AllocationGroup group in groups)
                {
                    if (!group.IsBasketGroup)
                    {
                        if (group.State == PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                        {
                            if (group.AllocationType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                            {
                                OrderFundAllocationManager.GetInstance.AllocatedGroups.Add(group.Clone());
                            }
                            else if (group.AllocationType == PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY)
                            {
                                OrderStrategyAllocationManager.GetInstance.AllocatedGroups.Add(group.Clone());
                            }
                            else
                            {
                                OrderFundAllocationManager.GetInstance.AllocatedGroups.Add(group);
                                OrderStrategyAllocationManager.GetInstance.AllocatedGroups.Add(group);
                            }
                        }
                        else if (group.State == PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED)
                        {
                            if (group.AllocationType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                            {
                                OrderFundAllocationManager.GetInstance.Groups.Add(group);
                            }
                            else if (group.AllocationType == PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY)
                            {
                                OrderStrategyAllocationManager.GetInstance.Groups.Add(group);
                            }
                            else
                            {
                                OrderFundAllocationManager.GetInstance.Groups.Add(group);
                                OrderStrategyAllocationManager.GetInstance.Groups.Add(group);
                            }
                        }
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
           
        }
      

       //public static void GetManualandPreAllocatedGroups(DateTime currentTime)
       //{

       //    try
       //    {

       //        AllocationGroups groups = OrderAllocationDBManager.GetupdatedGroups(currentTime);
       //        foreach (AllocationGroup group in groups)
       //        {
       //            if (!group.IsBasketGroup)
       //            {
       //                if (group.State == PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
       //                {
       //                    if (group.AllocationType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
       //                    {
       //                        OrderFundAllocationManager.GetInstance.AllocatedGroups.Add(group);
       //                    }
       //                    else if (group.AllocationType == PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY)
       //                    {
       //                        OrderStrategyAllocationManager.GetInstance.AllocatedGroups.Add(group);
       //                    }
       //                    else
       //                    {
       //                        OrderFundAllocationManager.GetInstance.AllocatedGroups.Add(group);
       //                        OrderStrategyAllocationManager.GetInstance.AllocatedGroups.Add(group);
       //                    }
       //                }
       //                else if (group.State == PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED)
       //                {
       //                    if (group.AllocationType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
       //                    {
       //                        OrderFundAllocationManager.GetInstance.Groups.Add(group);
       //                    }
       //                    else if (group.AllocationType == PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY)
       //                    {
       //                        OrderStrategyAllocationManager.GetInstance.Groups.Add(group);
       //                    }
       //                    else
       //                    {
       //                        OrderFundAllocationManager.GetInstance.Groups.Add(group);
       //                        OrderStrategyAllocationManager.GetInstance.Groups.Add(group);
       //                    }
       //                }
       //            }

       //        }


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
        

        public AllocationStrategies MapStrategyToFund(AllocationFunds funds)
        {
                FundStrategies fundStrategies = FundStraegyManager.GetFundStrategy();
            AllocationStrategies strategies = new AllocationStrategies();
            try
            {
                bool matchFound = false;
                foreach (AllocationFund fund in funds)
                {
                    matchFound = false;
                    foreach (FundStrategy fundStrategy in fundStrategies)
                    {

                        ////////////////////////////////////
                        if (fundStrategy.FundID == fund.FundID)
                        {
                            bool bAlreadyExist = false;
                            matchFound = true;
                            foreach (AllocationStrategy temp in strategies)
                            {
                                if (temp.StrategyID == fundStrategy.StrategyID)
                                {
                                    temp.Percentage = temp.Percentage + fund.Percentage;
                                    temp.AllocatedQty = temp.AllocatedQty + fund.AllocatedQty;
                                    bAlreadyExist = true;
                                }

                            }
                            if (!bAlreadyExist)
                            {
                                AllocationStrategy companyStrategy = new AllocationStrategy(fundStrategy.StrategyID, fundStrategy.StrategyName, fund.AllocatedQty, fund.Percentage);
                                strategies.Add(companyStrategy);
                            }
                            break;
                        }
                        ////////////////////////////////////
                    }

                    if (!matchFound)
                        return null;


                }


            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return strategies;

        }

        //public static void UpdateAllocation(DateTime currentTime)
        //{
        //    try
        //    {

        //        _newOrderSeqNumber = OrderDataManager.GetMaxSeqNumber();
        //        //_groupID = OrderDataManager.GetGroupID();
                                
        //        AllocationOrderCollection updatedFundOrders = OrderAllocationDBManager.GetUpdatedOrders(_userID, currentTime, _lastOrderSeqNumber);
        //        if (updatedFundOrders.Count > 0)
        //        {
        //            OrderFundAllocationManager.GetInstance.CheckUpdatedFundOrdersLocation(updatedFundOrders);
        //            // For Getting Strategy Orders Clone Fund Orders Received from dataBase
        //            OrderStrategyAllocationManager.GetInstance.CheckUpdatedStrategyOrdersLocation(updatedFundOrders.Clone());
        //        }
        //        AllocationOrderCollection updatedPreAllocBasketFundOrders = OrderAllocationDBManager.GetUpdatedBasketPreAllocatedFundOrders(_lastOrderSeqNumber);
        //        AllocationOrderCollection updatedPreAllocBasketStrategyOrders = OrderAllocationDBManager.GetUpdatedBasketPreAllocatedStrategyOrders(_lastOrderSeqNumber);
        //        OrderFundAllocationManager.GetInstance.CheckUpdatedFundOrdersLocation(updatedPreAllocBasketFundOrders);
        //        OrderFundAllocationManager.GetInstance.CheckUpdatedFundOrdersLocation(updatedPreAllocBasketFundOrders);
        //        OrderStrategyAllocationManager.GetInstance.CheckUpdatedStrategyOrdersLocation(updatedPreAllocBasketStrategyOrders);
        //        _lastOrderSeqNumber = _newOrderSeqNumber;
               
        //        GetManualandPreAllocatedGroups(DateTime.UtcNow);
                
                
        //        //AllocationGroups UpdatedGroups = OrderFundAllocationManager.GetUpdatedGroups(_groupID);
        //        //return updatedFundOrders;
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
      
    
        static void backGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
           
        }

        public static AllocationFunds   GetOrderFundsFrombasketFunds(AllocationFunds funds,double cumQty)
        {
            AllocationFunds orderFunds = new AllocationFunds();
            int Count = 1;
            int length = funds.Count;
            double TotalAllocatedQty = 0;
            foreach (AllocationFund fund in funds)
            {
                AllocationFund orderFund = new AllocationFund();
                orderFund.FundID = fund.FundID;
                orderFund.FundName = fund.FundName;
                orderFund.Percentage = fund.Percentage;
                orderFund.GroupID = fund.GroupID;
                orderFund.Commission = fund.Commission;
                orderFund.Fees = fund.Fees;
                if (Count == length)
                {
                    orderFund.AllocatedQty = cumQty - TotalAllocatedQty;
                }
                else
                {
                    orderFund.AllocatedQty = Convert.ToInt64(cumQty * orderFund.Percentage) / 100;
                }
                TotalAllocatedQty += orderFund.AllocatedQty;
                Count++;
                orderFunds.Add(orderFund);
            }
            return orderFunds;
        }

        public static AllocationStrategies GetOrderStrategiesFrombasketFunds(AllocationStrategies strategies, double cumQty)
        {
            AllocationStrategies orderStrategies = new AllocationStrategies();
            foreach (AllocationStrategy strategy in strategies)
            {
                AllocationStrategy orderStrategy = new AllocationStrategy();
                orderStrategy.StrategyID = strategy.StrategyID;
                orderStrategy.StrategyName = strategy.StrategyName;
                orderStrategy.Percentage = strategy.Percentage;
                orderStrategy.GroupID = strategy.GroupID;
                orderStrategy.AllocatedQty = (cumQty * orderStrategy.Percentage)/100;
                orderStrategies.Add(orderStrategy);
            }
            return orderStrategies;
        }


        public abstract void ProrataAllocatedGroup(AllocationGroup group);
        

        public static AllocationOrder GetAllocationOrder(Order order)
        {
            AllocationOrder allocationOrder = new AllocationOrder();
            allocationOrder.AssetID = order.AssetID;
            allocationOrder.AssetName = order.AssetName;
            allocationOrder.AUECID = order.AUECID;
            allocationOrder.AvgPrice = order.AvgPrice;
            allocationOrder.ClOrderID = order.ParentClOrderID;
            allocationOrder.UserID = order.CompanyUserID;
            allocationOrder.CounterPartyID = order.CounterPartyID;
            allocationOrder.CounterPartyName = order.CounterPartyName;
            allocationOrder.ExchangeID = order.ExchangeID;
            allocationOrder.ExchangeName = order.ExchangeName;
            allocationOrder.CumQty = order.CumQty;
            allocationOrder.OrderType = order.OrderType;
            allocationOrder.OrderTypeTagValue = order.OrderTypeTagValue;
            allocationOrder.AvgPrice = order.AvgPrice;
            allocationOrder.Quantity = order.Quantity;
            allocationOrder.OrderSideTagValue = order.OrderSideTagValue;
            allocationOrder.OrderSide = order.OrderSide;
            allocationOrder.Symbol = order.Symbol;
            allocationOrder.Quantity = Convert.ToInt64(order.Quantity);
            allocationOrder.TradingAccountID = order.TradingAccountID;
            allocationOrder.TradingAccountName = order.TradingAccountName;
            allocationOrder.UnderlyingID = order.UnderlyingID;
            allocationOrder.UnderlyingName = order.UnderlyingName;
            allocationOrder.Venue = order.Venue;
            allocationOrder.VenueID = order.VenueID;
            allocationOrder.ListID = order.ListID;
            allocationOrder.GroupID = order.GroupID;
            allocationOrder.TransactionTime = order.TransactionTime;
            return allocationOrder;
        }
       public static AllocationOrder GetAllocationOrderFromGroup(AllocationGroup group)
       {
           AllocationOrder allocationOrder = new AllocationOrder();
           allocationOrder.AssetID = group.AssetID;
           allocationOrder.AssetName = group.AssetName;
           allocationOrder.AUECID = group.AUECID;
           allocationOrder.AvgPrice = group.AvgPrice;
           allocationOrder.ClOrderID = group.GroupID;//need to check afterwards
           allocationOrder.UserID = group.UserID;
           allocationOrder.CounterPartyID = group.CounterPartyID;
           allocationOrder.CounterPartyName = group.CounterPartyName;
           allocationOrder.ExchangeID = group.ExchangeID;
           allocationOrder.ExchangeName = group.ExchangeName;
           allocationOrder.CumQty = group.CumQty;
           allocationOrder.OrderType = group.OrderType;
           allocationOrder.OrderTypeTagValue = group.OrderTypeTagValue;
           allocationOrder.AvgPrice = group.AvgPrice;
           allocationOrder.Quantity = group.Quantity;
           allocationOrder.OrderSideTagValue = group.OrderSideTagValue;
           allocationOrder.OrderSide = group.OrderSide;
           allocationOrder.Symbol = group.Symbol;
           allocationOrder.Quantity = Convert.ToInt64(group.Quantity);
           allocationOrder.TradingAccountID = group.TradingAccountID;
           allocationOrder.TradingAccountName = group.TradingAccountName;
           allocationOrder.UnderlyingID = group.UnderLyingID;
           allocationOrder.UnderlyingName = group.UnderLyingName;
           allocationOrder.Venue = group.Venue;
           allocationOrder.VenueID = group.VenueID;
           allocationOrder.ListID = group.ListID;
           allocationOrder.GroupID = group.GroupID;
           //allocationOrder.Commission = group.Commission;
           //allocationOrder.Fees = group.Fees;
           return allocationOrder;
       }

        public abstract AllocationOrderCollection Orders { get; set;}
        public abstract AllocationGroups Groups { get;set;}
        public abstract AllocationGroups AllocatedGroups { get; set; }

    }


}
