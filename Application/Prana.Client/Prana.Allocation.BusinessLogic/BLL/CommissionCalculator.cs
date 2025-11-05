using System;
using System.Collections.Generic;
using System.Text;
using Prana.CommonDataCache;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Allocation;
using Prana.Allocation.BLL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prana.Allocation.BLL
{
    /// <summary>
    /// Class responsible for calculating the commissions based on the rules and order cache
    /// </summary>
    public class CommissionCalculator
    {
        CachedDataManager _cachedDataManager = null;

        CommissionRulesCacheManager _commissionRulesCacheManager = null;

        //private AllocationGroups _allocatedGroups = new AllocationGroups();

        OrderFundAllocationManager _orderFundAllocationManager = OrderFundAllocationManager.GetInstance;

        #region Constructor region

        public CommissionCalculator()
        {
            _commissionRulesCacheManager = CommissionRulesCacheManager.GetInstance();
        }

        #endregion Constructor region

        public void GetCVAUECCCommissionRules()
        {
            //_commissionRulesCacheManager.GetCommissionRulesForCVAUEC(1);
            // CommissionDBManager.GetAllCommissionRulesForCVAUEC(3);
        }

        #region Commission and Fees Calculation For Grouped Orders

        #region Commented StartCalculation Method

        //public void StartCalculation(AllocationGroups _allocatedGroups)
        //{
        //    try
        //    {
        //        if (_allocatedGroups.Count > 0)
        //        {
        //            bool commissionCalculationTime = CommissionRulesCacheManager.GetInstance().GetAllocatedCalculationProperty();

        //            if (commissionCalculationTime)// for Post Allocation Commission Calculation //Commented on 30th Oct.
        //            {
        //                _cachedDataManager = CachedDataManager.GetInstance;
        //                foreach (AllocationGroup allocatedGroup in _allocatedGroups)
        //                {
        //                    allocatedGroup.CommissionCalculationTime = commissionCalculationTime;

        //                    if (allocatedGroup.IsCommissionCalculated == false)// means commission and Fee not calculated 
        //                    {
        //                        AllocationFunds allocationFunds = allocatedGroup.AllocationFunds;
        //                        string commissionText = "calculated";
        //                        foreach (AllocationFund allocationFund in allocationFunds)
        //                        {
        //                            CommissionRule commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUECFundId(allocatedGroup.CounterPartyID, allocatedGroup.VenueID, allocatedGroup.AUECID, allocatedGroup.ListID, allocationFund.FundID, ref commissionText);

        //                            if (commissionRule != null)
        //                            {
        //                                CalculateCommissionFundWise(commissionRule, allocationFund, allocatedGroup);
        //                                CalculateFeesFundWise(commissionRule, allocationFund, allocatedGroup);
        //                            }
        //                            else
        //                            {
        //                                allocatedGroup.CommissionText = commissionText;

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else // for PreAllocation Commission Calculation and PreAllocated Trades
        //            {
        //                foreach (AllocationGroup allocatedGroup in _allocatedGroups)
        //                {
        //                    string commissionText = "calculated";
        //                    allocatedGroup.CommissionCalculationTime = commissionCalculationTime;
        //                    if (allocatedGroup.IsCommissionCalculated == false)// means commission and Fee not calculated 
        //                    {
        //                        CommissionRule commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUEC(allocatedGroup.CounterPartyID, allocatedGroup.VenueID, allocatedGroup.AUECID, allocatedGroup.ListID, ref commissionText);
        //                        if (commissionRule != null)
        //                        {
        //                            CalculateCommissionGroupwise(commissionRule, allocatedGroup);
        //                            CalculateFeesGroupwise(commissionRule, allocatedGroup);
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        #endregion Commented StartCalculation Method


        public void StartCalculation(AllocationGroup allocatedGroup)
        {
            try
            {
                if (allocatedGroup != null)
                {
                    bool commissionCalculationTime = CommissionRulesCacheManager.GetInstance().GetAllocatedCalculationProperty();

                    if (commissionCalculationTime)// for Post Allocation Commission Calculation //Commented on 30th Oct.
                    {
                        _cachedDataManager = CachedDataManager.GetInstance;
                        allocatedGroup.CommissionCalculationTime = commissionCalculationTime;

                        if (allocatedGroup.IsCommissionCalculated == false)// means commission and Fee not calculated 
                        {
                            AllocationFunds allocationFunds = allocatedGroup.AllocationFunds;
                            string commissionText = "calculated";
                            foreach (AllocationFund allocationFund in allocationFunds)
                            {
                                CommissionRule commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUECFundId(allocatedGroup.CounterPartyID, allocatedGroup.VenueID, allocatedGroup.AUECID, allocatedGroup.ListID, allocationFund.FundID, ref commissionText);

                                if (commissionRule != null)
                                {
                                    CalculateCommissionFundWise(commissionRule, allocationFund, allocatedGroup);
                                    CalculateFeesFundWise(commissionRule, allocationFund, allocatedGroup);
                                }
                                else
                                {
                                    allocatedGroup.CommissionText = commissionText;
                                    allocationFund.Commission = 0.0;
                                    allocationFund.Fees = 0.0;
                                }
                            }
                        }
                    }

                    else // for PreAllocation Commission Calculation and PreAllocated Trades
                    {
                        string commissionText = "calculated";
                        allocatedGroup.CommissionCalculationTime = commissionCalculationTime;
                        if (allocatedGroup.IsCommissionCalculated == false)// means commission and Fee not calculated 
                        {
                            CommissionRule commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUEC(allocatedGroup.CounterPartyID, allocatedGroup.VenueID, allocatedGroup.AUECID, allocatedGroup.ListID, ref commissionText);
                            if (commissionRule != null)
                            {
                                CalculateCommissionGroupwise(commissionRule, allocatedGroup);
                                CalculateFeesGroupwise(commissionRule, allocatedGroup);
                            }
                            else
                            {
                                allocatedGroup.Commission = 0.0;
                                allocatedGroup.Fees = 0.0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void CalculateCommissionFundWise(CommissionRule commissionRuleToApply, AllocationFund allocationFund, AllocationGroup allocatedGroup)
        {
            double calculatedCommission = 0;
            double finalCalculatedCommission = 0;
            double commissionRate = 0.0;
            double notional = 0.0;

            switch (commissionRuleToApply.RuleAppliedOn)
            {
                case CommissionCalculationBasis.Shares:
                    commissionRate = GetCommissionRate(commissionRuleToApply, allocationFund.AllocatedQty);
                    calculatedCommission = allocationFund.AllocatedQty * commissionRate;
                    break;
                case CommissionCalculationBasis.Contracts:
                    commissionRate = GetCommissionRate(commissionRuleToApply, allocationFund.AllocatedQty);
                    calculatedCommission = allocationFund.AllocatedQty * commissionRate;
                    break;
                case CommissionCalculationBasis.Notional:
                    notional = GetNotionalValueForFund(allocatedGroup, allocationFund);
                    commissionRate = GetCommissionRate(commissionRuleToApply, notional);
                    // calculatedCommission = notional * commissionRuleToApply.CommissionRate; previous one
                    calculatedCommission = notional * commissionRate * 0.0001;
                    break;
                default:
                    throw new Exception("Commission rule basis not set.");
            }

            if (calculatedCommission < commissionRuleToApply.MinCommission && allocatedGroup.CumQty > 0)
            {
                ///If calculatedCommission is less than the absolute minimum commission, then we use the 
                ///minimum commission as the final commission
                finalCalculatedCommission = commissionRuleToApply.MinCommission;
            }
            else
            {
                finalCalculatedCommission = calculatedCommission;
            }

            allocationFund.Commission = System.Math.Round(finalCalculatedCommission, 2);
        }

        private double GetCommissionRate(CommissionRule commissionRuleToApply, double valueToCheck)
        {
            if (commissionRuleToApply.IsCriteriaApplied)
            {
                List<CommissionRuleCriteria> commissionRuleCriteriaList = commissionRuleToApply.CommissionRuleCriteiaList;
                foreach (CommissionRuleCriteria commissionRuleCriteria in commissionRuleCriteriaList)
                {
                    // special handling for ValueLessThanOrEqual
                    double valueLessThanOrEqual = double.MinValue;
                    if (commissionRuleCriteria.ValueLessThanOrEqual == 0)
                    {
                        valueLessThanOrEqual = double.MaxValue;
                    }
                    else
                    {
                        valueLessThanOrEqual = commissionRuleCriteria.ValueLessThanOrEqual;
                    }

                    if ((commissionRuleCriteria.ValueGreaterThan < valueToCheck) && (valueLessThanOrEqual >= valueToCheck))
                    {
                        return commissionRuleCriteria.CommissionRate;
                    }
                }
            }

            return commissionRuleToApply.CommissionRate;
        }

        private double GetNotionalValueForFund(AllocationGroup allocationGroup, AllocationFund allocationFund)
        {
            double MultiplierFactor = double.MinValue;

            AssetCategory asset = (AssetCategory)allocationGroup.AssetID;
            switch (asset)
            {
                case AssetCategory.None:
                    return 0.0;
                    break;
                case AssetCategory.Equity:
                    //  multiplier is 1 in case of Equity
                    MultiplierFactor = 1;
                    return allocationFund.AllocatedQty * allocationGroup.AvgPrice * MultiplierFactor;
                    break;
                case AssetCategory.EquityOption:
                    //  multiplier is 100 in case of EquityOption
                    MultiplierFactor = 100;
                    return allocationFund.AllocatedQty * allocationGroup.AvgPrice * MultiplierFactor;
                    break;
                case AssetCategory.Future:
                    // get the multiplier when Asset Type is Future
                    //MultiplierFactor = Prana.CommonDataCache.CachedDataManager.GetInstance.GetContractMultiplierBySymbol(order.Symbol.Substring(0, order.Symbol.IndexOf(" ")));
                    //return order.Quantity * order.AvgPrice * MultiplierFactor;
                    break;
                case AssetCategory.FutureOption:
                    //TODO: for the time being Multiplier factor is MultiplierFactor=1 but Need to get exact value of MultiplierFactor for FutureOption
                    //MultiplierFactor = 1;
                    //return order.Quantity * order.AvgPrice * MultiplierFactor;
                    break;
                //case AssetCategory.FutureOption:
                //    break;
                //case AssetCategory.ForeignExchange:
                //    break;
                //case AssetCategory.Cash:
                //    break;
                //case AssetCategory.Indices:
                //    break;
                default:
                    return 0.0;
                    //throw new Exception("Asset category not set.");
                    break;
            }
            return 0.0;
        }

        public void CalculateFeesFundWise(CommissionRule commissionRuleToApply, AllocationFund allocatedFund, AllocationGroup allocationGroup)
        {
            double calculatedFees = 0;
            double finalCalculatedFees = 0;
            double notional = 0.0;
            ///Fees Calculation
            if (commissionRuleToApply.IsClearingFeeApplied)
            {
                switch (commissionRuleToApply.ClearingFeeCalculationBasedOn)
                {
                    case CommissionCalculationBasis.Shares:
                        calculatedFees = allocatedFund.AllocatedQty * commissionRuleToApply.ClearingFeeRate;
                        break;
                    case CommissionCalculationBasis.Contracts: ///qty in this case is contract quantity
                        calculatedFees = allocatedFund.AllocatedQty * commissionRuleToApply.ClearingFeeRate;
                        break;
                    case CommissionCalculationBasis.Notional:
                        notional = GetNotionalValueForFund(allocationGroup, allocatedFund);
                        calculatedFees = notional * commissionRuleToApply.ClearingFeeRate * 0.0001;
                        break;
                    default:
                        throw new Exception("Commission rule basis not set. It should depend either of Shares,Contracts or Notional values.");
                }

                if (calculatedFees < commissionRuleToApply.MinClearingFee && allocationGroup.CumQty > 0)
                {
                    ///If calculatedFees is less than the absolute minimum ClearingFee, then we use the 
                    ///minimum fees as the final commission
                    finalCalculatedFees = commissionRuleToApply.MinClearingFee;
                }
                else
                {
                    finalCalculatedFees = calculatedFees;
                }

                allocatedFund.Fees = System.Math.Round(finalCalculatedFees, 2);
            }
        }

        public void CalculateCommissionGroupwise(CommissionRule commissionRuleToApply, AllocationGroup order)
        {
            double calculatedCommission = 0;
            double finalCalculatedCommission = 0;
            double commissionRate = 0.0;
            double notional = 0.0;

            switch (commissionRuleToApply.RuleAppliedOn)
            {
                case CommissionCalculationBasis.Shares:
                    commissionRate = GetCommissionRate(commissionRuleToApply, order.AllocatedQty);
                    calculatedCommission = order.AllocatedQty * commissionRate;
                    break;
                case CommissionCalculationBasis.Contracts:
                    commissionRate = GetCommissionRate(commissionRuleToApply, order.AllocatedQty);
                    calculatedCommission = order.AllocatedQty * commissionRate;
                    break;
                case CommissionCalculationBasis.Notional:
                    notional = GetNotionalValue(order);
                    commissionRate = GetCommissionRate(commissionRuleToApply, notional);
                    // calculatedCommission = notional * commissionRuleToApply.CommissionRate; previous one
                    calculatedCommission = notional * commissionRate * 0.0001;
                    break;
                default:
                    throw new Exception("Commission rule basis not set.");
            }

            if (calculatedCommission < commissionRuleToApply.MinCommission && order.CumQty > 0)
            {
                ///If calculatedCommission is less than the absolute minimum commission, then we use the 
                ///minimum commission as the final commission
                finalCalculatedCommission = commissionRuleToApply.MinCommission;
            }
            else
            {
                finalCalculatedCommission = calculatedCommission;
            }

            order.Commission = System.Math.Round(finalCalculatedCommission, 2);
        }

        public void CalculateFeesGroupwise(CommissionRule commissionRuleToApply, AllocationGroup order)
        {
            double calculatedFees = 0;
            double finalCalculatedFees = 0;
            double notional = 0.0;
            ///Fees Calculation
            if (commissionRuleToApply.IsClearingFeeApplied)
            {
                switch (commissionRuleToApply.ClearingFeeCalculationBasedOn)
                {
                    case CommissionCalculationBasis.Shares:
                        calculatedFees = order.AllocatedQty * commissionRuleToApply.ClearingFeeRate;
                        break;
                    case CommissionCalculationBasis.Contracts: ///qty in this case is contract quantity
                        calculatedFees = order.AllocatedQty * commissionRuleToApply.ClearingFeeRate;
                        break;
                    case CommissionCalculationBasis.Notional:
                        notional = GetNotionalValue(order);
                        calculatedFees = notional * commissionRuleToApply.ClearingFeeRate * 0.0001;
                        break;
                    default:
                        throw new Exception("Commission rule basis not set. It should depend either of Shares,Contracts or Notional values.");
                }

                if (calculatedFees < commissionRuleToApply.MinClearingFee && order.CumQty > 0)
                {
                    ///If calculatedFees is less than the absolute minimum ClearingFee, then we use the 
                    ///minimum fees as the final commission
                    finalCalculatedFees = commissionRuleToApply.MinClearingFee;
                }
                else
                {
                    finalCalculatedFees = calculatedFees;
                }

                order.Fees = System.Math.Round(finalCalculatedFees, 2);
            }
        }

        private double GetNotionalValue(AllocationGroup order)
        {
            double MultiplierFactor = double.MinValue;
            AssetCategory asset = (AssetCategory)order.AssetID;
            switch (asset)
            {
                case AssetCategory.None:
                    return 0.0;
                    break;
                case AssetCategory.Equity:
                    //  multiplier is 1 in case of Equity
                    MultiplierFactor = 1;
                    return order.AllocatedQty * order.AvgPrice * MultiplierFactor;
                    break;
                case AssetCategory.EquityOption:
                    //  multiplier is 100 in case of EquityOption
                    MultiplierFactor = 100;
                    return order.AllocatedQty * order.AvgPrice * MultiplierFactor;//order.SideMultiplier *order.Multiplier
                    break;
                case AssetCategory.Future:
                    // get the multiplier when Asset Type is Future
                    MultiplierFactor = Prana.CommonDataCache.CachedDataManager.GetInstance.GetContractMultiplierBySymbol(order.Symbol.Substring(0, order.Symbol.IndexOf(" ")));
                    return order.AllocatedQty * order.AvgPrice * MultiplierFactor;
                    break;
                case AssetCategory.FutureOption:
                    //TODO: for the time being Multiplier factor is MultiplierFactor=1 but Need to get exact value of MultiplierFactor for FutureOption
                    MultiplierFactor = 1;
                    return order.AllocatedQty * order.AvgPrice * MultiplierFactor;
                    break;
                //case AssetCategory.ForeignExchange:
                //    break;
                //case AssetCategory.Cash:
                //    break;
                //case AssetCategory.Indices:
                //    break;
                default:
                    return 0.0;
                    //throw new Exception("Asset category not set.");
                    break;
            }
            return 0.0;
        }

        #endregion Commission and Fees Calculation For Grouped Orders

        #region Commission and Fees Calculation Basket Trades

        #region Commented Commission and Fees calculation Method for Basket

        //public AllocationOrderCollection StartCalculationforBasket(BasketGroupCollection basketGroupCollection)
        //{
        //    AllocationOrderCollection allocationOrderCollection = new AllocationOrderCollection();
        //    try
        //    {

        //        if (basketGroupCollection.Count > 0)
        //        {
        //            bool commissionCalculationTime = false;// CommissionRulesCacheManager.GetInstance().GetAllocatedCalculationProperty();

        //            if (commissionCalculationTime == false)// for Pre Allocation Commission Calculation //Commented on 30th Oct.
        //            {
        //                foreach (BasketGroup basketGroup in basketGroupCollection)
        //                {
        //                    basketGroup.CommissionCalculationTime = commissionCalculationTime;

        //                    if (basketGroup.IsCommissionCalculated == false)// means commission and Fee not calculated 
        //                    {
        //                        // AllocationFunds allocationFunds = basketGroup.AllocationFunds;
        //                        if (basketGroup.AddedBaskets.Count == 1)
        //                        {
        //                            OrderCollection orderCollection = null;
        //                            AllocationFunds funds = null;
        //                            orderCollection = basketGroup.AddedBaskets[0].BasketOrders;
        //                            funds = basketGroup.AllocationFunds;
        //                            //AllocationOrderCollection allocationOrderCollection = new AllocationOrderCollection();
        //                            //double basketCommission = 0;
        //                            //double basketFees = 0;
        //                            basketGroup.Commission = 0.0;
        //                            basketGroup.Fees = 0.0;
        //                            _cachedDataManager = CachedDataManager.GetInstance;
        //                            foreach (Order order in orderCollection)
        //                            {
        //                                AllocationOrder allocationOrder = OrderAllocationManager.GetAllocationOrder(order);
        //                                AllocationFunds orderFunds = OrderAllocationManager.GetOrderFundsFrombasketFunds(funds, order.CumQty);
        //                                allocationOrder.AllocationFunds = orderFunds;
        //                                //set the Names
        //                                allocationOrder.AssetName = _cachedDataManager.GetAssetText(allocationOrder.AssetID);
        //                                allocationOrder.UnderlyingName = _cachedDataManager.GetUnderLyingText(allocationOrder.UnderlyingID);
        //                                allocationOrder.ExchangeID = _cachedDataManager.GetExchangeIdFromAUECId(allocationOrder.AUECID);
        //                                allocationOrder.ExchangeName = _cachedDataManager.GetExchangeText(allocationOrder.ExchangeID);
        //                                allocationOrder.CommissionCalculationTime = commissionCalculationTime;
        //                                string commissionText = "calculated";
        //                                foreach (AllocationFund fund in orderFunds)
        //                                {
        //                                    allocationOrder.AllocatedQty += fund.AllocatedQty;
        //                                }
        //                                CommissionRule commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUEC(allocationOrder.CounterPartyID, allocationOrder.VenueID, allocationOrder.AUECID, allocationOrder.ListID, ref commissionText);
        //                                if (commissionRule != null)
        //                                {
        //                                    CalculateCommissionGroupwiseForBasket(commissionRule, allocationOrder);
        //                                    CalculateFeesGroupwiseForBasket(commissionRule, allocationOrder);
        //                                    if (basketGroup.ListID == allocationOrder.ListID)
        //                                    {
        //                                        basketGroup.Commission += allocationOrder.Commission;
        //                                        basketGroup.Fees += allocationOrder.Fees;
        //                                    }
        //                                    //basketCommission = basketCommission + order.Commission;
        //                                    //basketFees = basketFees + order.Fees;
        //                                }
        //                                allocationOrder.AllocationFunds = orderFunds;
        //                                allocationOrderCollection.Add(allocationOrder);


        //                            }
        //                            //foreach (AllocationOrder order in allocationOrderCollection)
        //                            //{

        //                            //    CommissionRule commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUEC(order.CounterPartyID, order.VenueID, order.AUECID, order.ListID);
        //                            //    if (commissionRule != null)
        //                            //    {
        //                            //        CalculateCommissionGroupwiseForBasket(commissionRule, order);
        //                            //        CalculateFeesGroupwiseForBasket(commissionRule, order);
        //                            //        if (basketGroup.ListID == order.ListID)
        //                            //        {
        //                            //            basketGroup.Commission += order.Commission;
        //                            //            basketGroup.Fees += order.Fees;
        //                            //        }
        //                            //        //basketCommission = basketCommission + order.Commission;
        //                            //        //basketFees = basketFees + order.Fees;
        //                            //    }

        //                            //}

        //                            //basketGroup.Commission = basketCommission;
        //                            //basketGroup.Fees = basketFees;

        //                            //foreach (AllocationFund allocationFund in allocationFunds)
        //                            //{
        //                            //    CommissionRule commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUECFundId(basketGroup.CounterPartyID, basketGroup.VenueID, basketGroup.AUECID, basketGroup.ListID, allocationFund.FundID);
        //                            //    if (commissionRule != null)
        //                            //    {
        //                            //        CalculateCommissionFundWiseBasket(commissionRule, allocationFund, basketGroup);
        //                            //        CalculateFeesFundWiseBasket(commissionRule, allocationFund, basketGroup);
        //                            //    }
        //                            //}
        //                        }
        //                        else
        //                        { //need to write for grouped basket trades Am
        //                            AllocationOrderCollection allOrdersinBasketGroup = new AllocationOrderCollection();
        //                            basketGroup.Commission = 0.0;
        //                            basketGroup.Fees = 0.0;
        //                            foreach (BasketDetail basket in basketGroup.AddedBaskets)
        //                            {
        //                                foreach (Order basketOrder in basket.BasketOrders)
        //                                {
        //                                    allOrdersinBasketGroup.Add(OrderAllocationManager.GetAllocationOrder(basketOrder));
        //                                }
        //                            }
        //                            AllocationGroups allocationGroups = _orderFundAllocationManager.AutoGroupOrders(allOrdersinBasketGroup, AllocationPreferencesManager.AllocationPreferences, basketGroup.BasketGroupID);
        //                            AllocationFunds funds = null;
        //                            funds = basketGroup.AllocationFunds;
        //                            foreach (AllocationGroup group in allocationGroups)
        //                            {
        //                                _orderFundAllocationManager.AllocateGroup(group, group.CumQty, funds, true);

        //                                StartCalculation(group); 
        //                            }

        //                           // StartCalculation(allocationGroups); 
                                    
        //                            foreach (AllocationGroup allocationGroup in allocationGroups)
        //                            {
        //                                //allocationOrderCollection.Add(OrderAllocationManager.GetAllocationOrderFromGroup(allocationGroup));
        //                                AllocationOrder allocationorder = OrderAllocationManager.GetAllocationOrderFromGroup(allocationGroup);
        //                                AllocationFunds orderFunds = OrderAllocationManager.GetOrderFundsFrombasketFunds(funds, allocationGroup.CumQty);
        //                                allocationorder.AllocationFunds = orderFunds;
        //                                allocationorder.Commission = 0.0;
        //                                allocationorder.Fees = 0.0;
        //                                foreach (AllocationFund fund in orderFunds)
        //                                {
        //                                    fund.ParentBasketGroup = new AllocationOrder();
        //                                    allocationorder.Commission += fund.Commission;
        //                                    allocationorder.Fees += fund.Fees;
        //                                }
        //                                //basketGroup.Commission += allocationorder.Commission;
        //                                //basketGroup.Fees += allocationorder.Fees;
        //                                //allocationOrderCollection.Add(allocationorder);
        //                                allocationorder.Commission += allocationGroup.Commission;
        //                                allocationorder.Fees += allocationGroup.Fees;
        //                                basketGroup.Commission += allocationorder.Commission;
        //                                basketGroup.Fees += allocationorder.Fees;
        //                                allocationOrderCollection.Add(allocationorder);
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //            else // for Post Allocation Commission Calculation
        //            {
        //                foreach (BasketGroup basketGroup in basketGroupCollection)
        //                {
        //                    basketGroup.CommissionCalculationTime = commissionCalculationTime;
        //                    if (basketGroup.IsCommissionCalculated == false)// means commission and Fee not calculated 
        //                    {
        //                        // AllocationFunds allocationFunds = basketGroup.AllocationFunds;
        //                        if (basketGroup.AddedBaskets.Count == 1)
        //                        {
        //                            OrderCollection orderCollection = null;
        //                            AllocationFunds funds = null;
        //                            orderCollection = basketGroup.AddedBaskets[0].BasketOrders;
        //                            funds = basketGroup.AllocationFunds;
        //                            //AllocationOrderCollection allocationOrderCollection = new AllocationOrderCollection();
        //                            //double basketCommission = 0;
        //                            //double basketFees = 0;
        //                            basketGroup.Commission = 0.0;
        //                            basketGroup.Fees = 0.0;
        //                            _cachedDataManager = CachedDataManager.GetInstance;
        //                            foreach (Order order in orderCollection)
        //                            {
        //                                AllocationOrder allocationOrder = OrderAllocationManager.GetAllocationOrder(order);
        //                                AllocationFunds orderFunds = OrderAllocationManager.GetOrderFundsFrombasketFunds(funds, order.CumQty);
        //                                allocationOrder.AllocationFunds = orderFunds;
        //                                allocationOrderCollection.Add(allocationOrder);
        //                                allocationOrder.Commission = 0.0;
        //                                allocationOrder.Fees = 0.0;
        //                                //set the Names
        //                                allocationOrder.AssetName = _cachedDataManager.GetAssetText(allocationOrder.AssetID);
        //                                allocationOrder.UnderlyingName = _cachedDataManager.GetUnderLyingText(allocationOrder.UnderlyingID);
        //                                allocationOrder.ExchangeID = _cachedDataManager.GetExchangeIdFromAUECId(allocationOrder.AUECID);
        //                                allocationOrder.ExchangeName = _cachedDataManager.GetExchangeText(allocationOrder.ExchangeID);
        //                                allocationOrder.CommissionCalculationTime = commissionCalculationTime;
        //                                if (allocationOrder.IsCommissionCalculated == false)
        //                                {
        //                                    string commissionText = "calculated";
        //                                    foreach (AllocationFund fund in orderFunds)
        //                                    {

        //                                        CommissionRule commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUECFundId(allocationOrder.CounterPartyID, allocationOrder.VenueID, allocationOrder.AUECID, allocationOrder.ListID, fund.FundID, ref  commissionText);

        //                                        if (commissionRule != null)
        //                                        {
        //                                            CalculateCommissionFundWiseBasket(commissionRule, fund, allocationOrder);
        //                                            CalculateFeesFundWiseBasket(commissionRule, fund, allocationOrder);
        //                                        }

        //                                        allocationOrder.CommissionCalculationTime = commissionCalculationTime;
        //                                        fund.ParentBasketGroup = allocationOrder;
        //                                        allocationOrder.Commission += System.Math.Round(fund.Commission, 2);
        //                                        allocationOrder.Fees += System.Math.Round(fund.Fees, 2);

        //                                    }
        //                                    if (basketGroup.ListID == allocationOrder.ListID)
        //                                    {
        //                                        basketGroup.Commission += allocationOrder.Commission;
        //                                        basketGroup.Fees += allocationOrder.Fees;
        //                                    }
        //                                    //basketCommission = basketCommission + order.Commission;
        //                                    //basketFees = basketFees + order.Fees;
        //                                }
        //                            }


        //                        }
        //                        else
        //                        { //need to write for grouped basket trades Am
        //                            basketGroup.Commission = 0.0;
        //                            basketGroup.Fees = 0.0;
        //                            AllocationOrderCollection allOrdersinBasketGroup = new AllocationOrderCollection();
        //                            foreach (BasketDetail basket in basketGroup.AddedBaskets)
        //                            {
        //                                foreach (Order basketOrder in basket.BasketOrders)
        //                                {
        //                                    allOrdersinBasketGroup.Add(OrderAllocationManager.GetAllocationOrder(basketOrder));
        //                                }
        //                            }
        //                            AllocationGroups allocationGroups = _orderFundAllocationManager.AutoGroupOrders(allOrdersinBasketGroup, AllocationPreferencesManager.AllocationPreferences, basketGroup.BasketGroupID);
        //                            AllocationFunds funds = null;
        //                            funds = basketGroup.AllocationFunds;
        //                            foreach (AllocationGroup group in allocationGroups)
        //                            {
        //                                _orderFundAllocationManager.AllocateGroup(group, group.CumQty, funds, true);

        //                                StartCalculation(group);
        //                            }
        //                            //StartCalculation(allocationGroups);


        //                            foreach (AllocationGroup allocationGroup in allocationGroups)
        //                            {

        //                                //allocationOrderCollection.Add(OrderAllocationManager.GetAllocationOrderFromGroup(allocationGroup));
        //                                AllocationOrder allocationorder = OrderAllocationManager.GetAllocationOrderFromGroup(allocationGroup);
        //                                AllocationFunds orderFunds = OrderAllocationManager.GetOrderFundsFrombasketFunds(funds, allocationGroup.CumQty);
        //                                allocationorder.AllocationFunds = orderFunds;
        //                                allocationorder.Commission = 0.0;
        //                                allocationorder.Fees = 0.0;
        //                                //foreach (AllocationFund fund in allocationGroup.AllocationFunds)
        //                                //{
        //                                //    //fund.Commission= allocationGroup.AllocationFunds[0]
        //                                //    //allocationorder.Commission += fund.Commission;
        //                                //    //allocationorder.Fees += fund.Fees;
        //                                //}

        //                                allocationorder.Commission += allocationGroup.Commission;
        //                                allocationorder.Fees += allocationGroup.Fees;
        //                                basketGroup.Commission += allocationorder.Commission;
        //                                basketGroup.Fees += allocationorder.Fees;
        //                                allocationOrderCollection.Add(allocationorder);
        //                            }

        //                        }

        //                    }
        //                }
        //            }


        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return allocationOrderCollection;
        //}
        #endregion Commented Commission and Fees calculation Method for Basket

        public AllocationOrderCollection StartCalculationforBasket(BasketGroup basketGroup)
        {
            AllocationOrderCollection allocationOrderCollection = new AllocationOrderCollection();
            try
            {
                if (basketGroup !=null)
                {
                    bool commissionCalculationTime = false;// CommissionRulesCacheManager.GetInstance().GetAllocatedCalculationProperty();

                    if (commissionCalculationTime == false)// for Pre Allocation Commission Calculation //Commented on 30th Oct.
                    {
                        basketGroup.CommissionCalculationTime = commissionCalculationTime;

                        if (basketGroup.IsCommissionCalculated == false)// means commission and Fee not calculated 
                        {
                            if (basketGroup.AddedBaskets.Count == 1)
                            {
                                OrderCollection orderCollection = null;
                                AllocationFunds funds = null;
                                orderCollection = basketGroup.AddedBaskets[0].BasketOrders;
                                funds = basketGroup.AllocationFunds;
                                basketGroup.Commission = 0.0;
                                basketGroup.Fees = 0.0;
                                _cachedDataManager = CachedDataManager.GetInstance;
                                foreach (Order order in orderCollection)
                                {
                                    AllocationOrder allocationOrder = OrderAllocationManager.GetAllocationOrder(order);
                                    AllocationFunds orderFunds = OrderAllocationManager.GetOrderFundsFrombasketFunds(funds, order.CumQty);
                                    allocationOrder.AllocationFunds = orderFunds;
                                    //set the Names
                                    allocationOrder.AssetName = _cachedDataManager.GetAssetText(allocationOrder.AssetID);
                                    allocationOrder.UnderlyingName = _cachedDataManager.GetUnderLyingText(allocationOrder.UnderlyingID);
                                    allocationOrder.ExchangeID = _cachedDataManager.GetExchangeIdFromAUECId(allocationOrder.AUECID);
                                    allocationOrder.ExchangeName = _cachedDataManager.GetExchangeText(allocationOrder.ExchangeID);
                                    allocationOrder.CommissionCalculationTime = commissionCalculationTime;
                                    string commissionText = "calculated";
                                    foreach (AllocationFund fund in orderFunds)
                                    {
                                        allocationOrder.AllocatedQty += fund.AllocatedQty;
                                    }
                                    CommissionRule commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUEC(allocationOrder.CounterPartyID, allocationOrder.VenueID, allocationOrder.AUECID, allocationOrder.ListID, ref commissionText);
                                    if (commissionRule!=null)
                                    {
                                        CalculateCommissionGroupwiseForBasket(commissionRule, allocationOrder);
                                        CalculateFeesGroupwiseForBasket(commissionRule, allocationOrder);
                                        if (basketGroup.ListID == allocationOrder.ListID)
                                        {
                                            basketGroup.Commission += allocationOrder.Commission;
                                            basketGroup.Fees += allocationOrder.Fees;
                                        }
                                    }
                                    allocationOrder.AllocationFunds = orderFunds;
                                    allocationOrderCollection.Add(allocationOrder);
                                }
                            }
                            else
                            {
                                //need to write for grouped basket trades Am
                                AllocationOrderCollection allOrdersinBasketGroup = new AllocationOrderCollection();
                                basketGroup.Commission = 0.0;
                                basketGroup.Fees = 0.0;
                                foreach (BasketDetail basket in basketGroup.AddedBaskets)
                                {
                                    foreach (Order basketOrder in basket.BasketOrders)
                                    {
                                        allOrdersinBasketGroup.Add(OrderAllocationManager.GetAllocationOrder(basketOrder));
                                    }
                                }
                                AllocationGroups allocationGroups = _orderFundAllocationManager.AutoGroupOrders(allOrdersinBasketGroup, AllocationPreferencesManager.AllocationPreferences, basketGroup.BasketGroupID,basketGroup.AUECLocalDate);
                                AllocationFunds funds = null;
                                funds = basketGroup.AllocationFunds;
                                foreach (AllocationGroup group in allocationGroups)
                                {
                                   
                                    _orderFundAllocationManager.AllocateGroup(group, group.CumQty, funds, true,group.AUECLocalDate);
                                  
                                   
                                }

                                // StartCalculation(allocationGroups); 

                                foreach (AllocationGroup allocationGroup in allocationGroups)
                                {
                                    //allocationOrderCollection.Add(OrderAllocationManager.GetAllocationOrderFromGroup(allocationGroup));
                                    AllocationOrder allocationorder = OrderAllocationManager.GetAllocationOrderFromGroup(allocationGroup);
                                    AllocationFunds orderFunds = OrderAllocationManager.GetOrderFundsFrombasketFunds(funds, allocationGroup.CumQty);
                                    allocationorder.AllocationFunds = orderFunds;
                                    allocationorder.Commission = 0.0;
                                    allocationorder.Fees = 0.0;
                                    foreach (AllocationFund fund in orderFunds)
                                    {
                                        fund.ParentBasketGroup = new AllocationOrder();
                                        allocationorder.Commission += fund.Commission;
                                        allocationorder.Fees += fund.Fees;
                                    }
                                    allocationorder.Commission += allocationGroup.Commission;
                                    allocationorder.Fees += allocationGroup.Fees;
                                    basketGroup.Commission += allocationorder.Commission;
                                    basketGroup.Fees += allocationorder.Fees;
                                    allocationOrderCollection.Add(allocationorder);
                                }
                            }
                        }


                    }
                    else // for Post Allocation Commission Calculation
                    {
                        basketGroup.CommissionCalculationTime = commissionCalculationTime;
                        if (basketGroup.IsCommissionCalculated == false)// means commission and Fee not calculated 
                        {
                            if (basketGroup.AddedBaskets.Count == 1)
                            {
                                OrderCollection orderCollection = null;
                                AllocationFunds funds = null;
                                orderCollection = basketGroup.AddedBaskets[0].BasketOrders;
                                funds = basketGroup.AllocationFunds;
                                basketGroup.Commission = 0.0;
                                basketGroup.Fees = 0.0;
                                _cachedDataManager = CachedDataManager.GetInstance;
                                foreach (Order order in orderCollection)
                                {
                                    AllocationOrder allocationOrder = OrderAllocationManager.GetAllocationOrder(order);
                                    AllocationFunds orderFunds = OrderAllocationManager.GetOrderFundsFrombasketFunds(funds, order.CumQty);
                                    allocationOrder.AllocationFunds = orderFunds;
                                    allocationOrderCollection.Add(allocationOrder);
                                    allocationOrder.Commission = 0.0;
                                    allocationOrder.Fees = 0.0;
                                    //set the Names
                                    allocationOrder.AssetName = _cachedDataManager.GetAssetText(allocationOrder.AssetID);
                                    allocationOrder.UnderlyingName = _cachedDataManager.GetUnderLyingText(allocationOrder.UnderlyingID);
                                    allocationOrder.ExchangeID = _cachedDataManager.GetExchangeIdFromAUECId(allocationOrder.AUECID);
                                    allocationOrder.ExchangeName = _cachedDataManager.GetExchangeText(allocationOrder.ExchangeID);
                                    allocationOrder.CommissionCalculationTime = commissionCalculationTime;
                                    if (allocationOrder.IsCommissionCalculated == false)
                                    {
                                        string commissionText = "calculated";
                                        foreach (AllocationFund fund in orderFunds)
                                        {
                                            CommissionRule commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUECFundId(allocationOrder.CounterPartyID, allocationOrder.VenueID, allocationOrder.AUECID, allocationOrder.ListID, fund.FundID, ref  commissionText);
                                            if (commissionRule!= null)
                                            {
                                                CalculateCommissionFundWiseBasket(commissionRule, fund, allocationOrder);
                                                CalculateFeesFundWiseBasket(commissionRule, fund, allocationOrder);
                                            }

                                            allocationOrder.CommissionCalculationTime = commissionCalculationTime;
                                            fund.ParentBasketGroup = allocationOrder;
                                            allocationOrder.Commission += System.Math.Round(fund.Commission, 2);
                                            allocationOrder.Fees += System.Math.Round(fund.Fees, 2);
                                        }
                                        if (basketGroup.ListID == allocationOrder.ListID)
                                        {
                                            basketGroup.Commission += allocationOrder.Commission;
                                            basketGroup.Fees += allocationOrder.Fees;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //need to write for grouped basket trades Am
                                basketGroup.Commission = 0.0;
                                basketGroup.Fees = 0.0;
                                AllocationOrderCollection allOrdersinBasketGroup = new AllocationOrderCollection();
                                foreach (BasketDetail basket in basketGroup.AddedBaskets)
                                {
                                    foreach (Order basketOrder in basket.BasketOrders)
                                    {
                                        allOrdersinBasketGroup.Add(OrderAllocationManager.GetAllocationOrder(basketOrder));
                                    }
                                }
                                AllocationGroups allocationGroups = _orderFundAllocationManager.AutoGroupOrders(allOrdersinBasketGroup, AllocationPreferencesManager.AllocationPreferences, basketGroup.BasketGroupID,basketGroup.AUECLocalDate);
                                AllocationFunds funds = null;
                                funds = basketGroup.AllocationFunds;
                                foreach (AllocationGroup group in allocationGroups)
                                {
                                    _orderFundAllocationManager.AllocateGroup(group, group.CumQty, funds, true,group.AUECLocalDate);

                                    StartCalculation(group);
                                }
                                //StartCalculation(allocationGroups);
                                foreach (AllocationGroup allocationGroup in allocationGroups)
                                {
                                    AllocationOrder allocationorder = OrderAllocationManager.GetAllocationOrderFromGroup(allocationGroup);
                                    AllocationFunds orderFunds = OrderAllocationManager.GetOrderFundsFrombasketFunds(funds, allocationGroup.CumQty);
                                    allocationorder.AllocationFunds = orderFunds;
                                    allocationorder.Commission = 0.0;
                                    allocationorder.Fees = 0.0;
                                    allocationorder.Commission += allocationGroup.Commission;
                                    allocationorder.Fees += allocationGroup.Fees;
                                    basketGroup.Commission += allocationorder.Commission;
                                    basketGroup.Fees += allocationorder.Fees;
                                    allocationOrderCollection.Add(allocationorder);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return allocationOrderCollection;
        }

        public void CalculateCommissionFundWiseBasket(CommissionRule commissionRuleToApply, AllocationFund allocationFund, AllocationOrder order)
        {
            double calculatedCommission = 0;
            double finalCalculatedCommission = 0;
            double commissionRate = 0.0;
            double notional = 0.0;

            switch (commissionRuleToApply.RuleAppliedOn)
            {
                case CommissionCalculationBasis.Shares:
                    commissionRate = GetCommissionRate(commissionRuleToApply, allocationFund.AllocatedQty);
                    calculatedCommission = allocationFund.AllocatedQty * commissionRate;
                    break;
                case CommissionCalculationBasis.Contracts:
                    commissionRate = GetCommissionRate(commissionRuleToApply, allocationFund.AllocatedQty);
                    calculatedCommission = allocationFund.AllocatedQty * commissionRate;
                    break;
                case CommissionCalculationBasis.Notional:
                    notional = GetNotionalValueForFundBasket(order, allocationFund);
                    commissionRate = GetCommissionRate(commissionRuleToApply, notional);
                    // calculatedCommission = notional * commissionRuleToApply.CommissionRate; previous one
                    calculatedCommission = notional * commissionRate * 0.0001;
                    break;
                default:
                    throw new Exception("Commission rule basis not set.");
            }

            if (calculatedCommission < commissionRuleToApply.MinCommission && order.CumQty > 0)
            {
                ///If calculatedCommission is less than the absolute minimum commission, then we use the 
                ///minimum commission as the final commission
                finalCalculatedCommission = commissionRuleToApply.MinCommission;
            }
            else
            {
                finalCalculatedCommission = calculatedCommission;
            }

            allocationFund.Commission = System.Math.Round(finalCalculatedCommission, 2);
        }
        public void CalculateFeesFundWiseBasket(CommissionRule commissionRuleToApply, AllocationFund allocatedFund, AllocationOrder order)
        {
            double calculatedFees = 0;
            double finalCalculatedFees = 0;
            double notional = 0.0;
            ///Fees Calculation
            if (commissionRuleToApply.IsClearingFeeApplied)
            {
                switch (commissionRuleToApply.ClearingFeeCalculationBasedOn)
                {
                    case CommissionCalculationBasis.Shares:
                        calculatedFees = allocatedFund.AllocatedQty * commissionRuleToApply.ClearingFeeRate;
                        break;
                    case CommissionCalculationBasis.Contracts: ///qty in this case is contract quantity
                        calculatedFees = allocatedFund.AllocatedQty * commissionRuleToApply.ClearingFeeRate;
                        break;
                    case CommissionCalculationBasis.Notional:
                        notional = GetNotionalValueForFundBasket(order, allocatedFund);
                        calculatedFees = notional * commissionRuleToApply.ClearingFeeRate * 0.0001;
                        break;
                    default:
                        throw new Exception("Commission rule basis not set. It should depend either of Shares,Contracts or Notional values.");
                }

                if (calculatedFees < commissionRuleToApply.MinClearingFee && order.CumQty > 0)
                {
                    ///If calculatedFees is less than the absolute minimum ClearingFee, then we use the 
                    ///minimum fees as the final commission
                    finalCalculatedFees = commissionRuleToApply.MinClearingFee;
                }
                else
                {
                    finalCalculatedFees = calculatedFees;
                }

                allocatedFund.Fees = System.Math.Round(finalCalculatedFees, 2);
            }
        }
        private double GetNotionalValueForFundBasket(AllocationOrder order, AllocationFund fund)
        {
            double MultiplierFactor = double.MinValue;

            AssetCategory asset = (AssetCategory)order.AssetID;
            switch (asset)
            {
                case AssetCategory.None:
                    return 0.0;
                    break;
                case AssetCategory.Equity:
                    //  multiplier is 1 in case of Equity
                    MultiplierFactor = 1;
                    return fund.AllocatedQty * order.AvgPrice * MultiplierFactor;
                    break;
                case AssetCategory.EquityOption:
                    //  multiplier is 100 in case of EquityOption
                    MultiplierFactor = 100;
                    return fund.AllocatedQty * order.AvgPrice * MultiplierFactor;
                    break;
                case AssetCategory.Future:
                    // get the multiplier when Asset Type is Future
                    //MultiplierFactor = Prana.CommonDataCache.CachedDataManager.GetInstance.GetContractMultiplierBySymbol(order.Symbol.Substring(0, order.Symbol.IndexOf(" ")));
                    return fund.AllocatedQty * order.AvgPrice * MultiplierFactor;
                    break;
                case AssetCategory.FutureOption:
                    //TODO: for the time being Multiplier factor is MultiplierFactor=1 but Need to get exact value of MultiplierFactor for FutureOption
                    MultiplierFactor = 1;
                    return fund.AllocatedQty * order.AvgPrice * MultiplierFactor;
                    break;
                //case AssetCategory.FutureOption:
                //    break;
                //case AssetCategory.ForeignExchange:
                //    break;
                //case AssetCategory.Cash:
                //    break;
                //case AssetCategory.Indices:
                //    break;
                default:
                    return 0.0;
                    //throw new Exception("Asset category not set.");
                    break;
            }
            return 0.0;
        }

        public void CalculateCommissionGroupwiseForBasket(CommissionRule commissionRuleToApply, AllocationOrder order)
        {
            double calculatedCommission = 0;
            double finalCalculatedCommission = 0;
            double commissionRate = 0.0;
            double notional = 0.0;

            switch (commissionRuleToApply.RuleAppliedOn)
            {
                case CommissionCalculationBasis.Shares:
                    commissionRate = GetCommissionRate(commissionRuleToApply, order.AllocatedQty);
                    calculatedCommission = order.AllocatedQty * commissionRate;
                    break;
                case CommissionCalculationBasis.Contracts:
                    commissionRate = GetCommissionRate(commissionRuleToApply, order.AllocatedQty);
                    calculatedCommission = order.AllocatedQty * commissionRate;
                    break;
                case CommissionCalculationBasis.Notional:
                    notional = GetNotionalValueForBasket(order);
                    commissionRate = GetCommissionRate(commissionRuleToApply, notional);
                    // calculatedCommission = notional * commissionRuleToApply.CommissionRate; previous one
                    calculatedCommission = notional * commissionRate * 0.0001;
                    break;
                default:
                    throw new Exception("Commission rule basis not set.");
            }

            if (calculatedCommission < commissionRuleToApply.MinCommission && order.CumQty > 0)
            {
                ///If calculatedCommission is less than the absolute minimum commission, then we use the 
                ///minimum commission as the final commission
                finalCalculatedCommission = commissionRuleToApply.MinCommission;
            }
            else
            {
                finalCalculatedCommission = calculatedCommission;
            }

            order.Commission = System.Math.Round(finalCalculatedCommission, 2);
        }

        public void CalculateFeesGroupwiseForBasket(CommissionRule commissionRuleToApply, AllocationOrder order)
        {
            double calculatedFees = 0;
            double finalCalculatedFees = 0;
            double notional = 0.0;
            ///Fees Calculation
            if (commissionRuleToApply.IsClearingFeeApplied)
            {
                switch (commissionRuleToApply.ClearingFeeCalculationBasedOn)
                {
                    case CommissionCalculationBasis.Shares:
                        calculatedFees = order.AllocatedQty * commissionRuleToApply.ClearingFeeRate;
                        break;
                    case CommissionCalculationBasis.Contracts: ///qty in this case is contract quantity
                        calculatedFees = order.AllocatedQty * commissionRuleToApply.ClearingFeeRate;
                        break;
                    case CommissionCalculationBasis.Notional:
                        notional = GetNotionalValueForBasket(order);
                        calculatedFees = notional * commissionRuleToApply.ClearingFeeRate * 0.0001;
                        break;
                    default:
                        throw new Exception("Commission rule basis not set. It should depend either of Shares,Contracts or Notional values.");
                }

                if (calculatedFees < commissionRuleToApply.MinClearingFee && order.CumQty > 0)
                {
                    ///If calculatedFees is less than the absolute minimum ClearingFee, then we use the 
                    ///minimum fees as the final commission
                    finalCalculatedFees = commissionRuleToApply.MinClearingFee;
                }
                else
                {
                    finalCalculatedFees = calculatedFees;
                }

                order.Fees = System.Math.Round(finalCalculatedFees, 2);
            }
        }

        private double GetNotionalValueForBasket(AllocationOrder order)
        {
            double MultiplierFactor = double.MinValue;
            AssetCategory asset = (AssetCategory)order.AssetID;
            switch (asset)
            {
                case AssetCategory.None:
                    return 0.0;
                    break;
                case AssetCategory.Equity:
                    //  multiplier is 1 in case of Equity
                    MultiplierFactor = 1;
                    return order.AllocatedQty * order.AvgPrice * MultiplierFactor;
                    break;
                case AssetCategory.EquityOption:
                    //  multiplier is 100 in case of EquityOption
                    MultiplierFactor = 100;
                    return order.AllocatedQty * order.AvgPrice * MultiplierFactor;//order.SideMultiplier *order.Multiplier
                    break;
                case AssetCategory.Future:
                    // get the multiplier when Asset Type is Future
                    MultiplierFactor = 1;// = Prana.CommonDataCache.CachedDataManager.GetInstance.GetContractMultiplierBySymbol(order.Symbol.Substring(0, order.Symbol.IndexOf(" ")));
                    return order.AllocatedQty * order.AvgPrice * MultiplierFactor;
                    break;
                case AssetCategory.FutureOption:
                    //TODO: for the time being Multiplier factor is MultiplierFactor=1 but Need to get exact value of MultiplierFactor for FutureOption
                    MultiplierFactor = 1;
                    return order.AllocatedQty * order.AvgPrice * MultiplierFactor;
                    break;
                //case AssetCategory.ForeignExchange:
                //    break;
                //case AssetCategory.Cash:
                //    break;
                //case AssetCategory.Indices:
                //    break;
                default:
                    return 0.0;
                    //throw new Exception("Asset category not set.");
                    break;
            }
            return 0.0;
        }

        public double GetFees(string ClOrderID, int FundID, AllocationFunds allocationFundsfromDbBasket)
        {
            double result = 0.0;
            foreach (AllocationFund allocFund in allocationFundsfromDbBasket)
            {
                if (ClOrderID.Equals(allocFund.GroupID) && FundID == allocFund.FundID) //this GroupID field contains clorderid
                {
                    result = allocFund.Fees;
                    break;
                }
            }
            return result;
        }

        public double GetCommission(string ClOrderID, int FundID, AllocationFunds allocationFundsfromDbBasket, out bool IsCommissionCalculated)
        {
            double result = 0.0;
            IsCommissionCalculated = false;
            foreach (AllocationFund allocFund in allocationFundsfromDbBasket)
            {
                if (ClOrderID.Equals(allocFund.GroupID) && FundID == allocFund.FundID) //this GroupID field contains clorderid
                {
                    result = allocFund.Commission;
                    IsCommissionCalculated = true;
                    break;
                }
            }
            return result;
        }

        #endregion Commission and Fees Calculation Basket Trades

    }
}
