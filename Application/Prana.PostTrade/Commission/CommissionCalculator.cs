using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.PostTrade.Commission
{
    /// <summary>
    /// Class responsible for calculating the commissions based on the rules and order cache
    /// </summary>
    class CommissionCalculator
    {

        static CommissionRulesCacheManager _commissionRulesCacheManager = null;
        static CommissionCalculator _commissionCalculator = null;
        #region Constructor region
        public static CommissionCalculator GetInstance
        {
            get
            {
                return _commissionCalculator;
            }
        }
        static CommissionCalculator()
        {
            _commissionCalculator = new CommissionCalculator();
            _commissionRulesCacheManager = CommissionRulesCacheManager.GetInstance();
        }

        #endregion Constructor region

        #region Commission and Fees Calculation For Grouped Orders



        public void StartCalculation(AllocationGroup allocatedGroup)
        {
            try
            {
                if (allocatedGroup != null)
                {
                    bool commissionCalculationTime = CommissionRulesCacheManager.GetInstance().GetAllocatedCalculationProperty();
                    bool isCommissionAndFeeZero = allocatedGroup.IsSwapped && Convert.ToBoolean(Convert.ToInt32(CachedDataManager.GetInstance.GetPranaPreferenceByKey(ApplicationConstants.CONST_ZEROCOMMISSIONFORSWAPS)));
                    allocatedGroup.CommissionCalculationTime = commissionCalculationTime;
                    List<OtherFeeRule> lstOtherFeeRule = new List<OtherFeeRule>();
                    CommissionRule commissionRule = new CommissionRule(true, CalculationBasis.FlatAmount, 0);
                    lstOtherFeeRule = CommissionRulesCacheManager.GetInstance().GetOtherFeeRuleAuecDict(allocatedGroup.AUECID);

                    if (commissionCalculationTime)// for Post Allocation Commission Calculation //Commented on 30th Oct.
                    {
                        if (allocatedGroup.IsCommissionCalculated == false)// means commission and Fee not calculated 
                        {
                            List<TaxLot> taxlots = (List<TaxLot>)allocatedGroup.TaxLots;
                            string commissionText = "calculated";

                            double totalCommission = 0;
                            double totalSoftCommission = 0;
                            double totalOtherBrokerFee = 0;
                            double totalClearingBrokerFee = 0;
                            foreach (TaxLot taxlot in taxlots)
                            {
                                if (!isCommissionAndFeeZero)
                                    commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUECAccountId(allocatedGroup.CounterPartyID, allocatedGroup.VenueID, allocatedGroup.AUECID, allocatedGroup.ListID, taxlot.Level1ID, ref commissionText);

                                if (commissionRule != null)
                                {
                                    CalculateCommissionAccountWise(commissionRule, taxlot, allocatedGroup);
                                    CalculateFeesAccountWise(commissionRule, taxlot, allocatedGroup);
                                    // sum of all taxlots commission
                                    totalCommission = totalCommission + taxlot.Commission;
                                    // sum of all taxlots soft commission
                                    totalSoftCommission = totalSoftCommission + taxlot.SoftCommission;
                                    // sum of all taxlots Fee
                                    totalOtherBrokerFee = totalOtherBrokerFee + taxlot.OtherBrokerFees;
                                    // sum of all taxlots clearing broker Fee
                                    totalClearingBrokerFee = totalClearingBrokerFee + taxlot.ClearingBrokerFee;
                                }
                                else
                                {
                                    allocatedGroup.CommissionText = commissionText;
                                    taxlot.Commission = 0.0;
                                    taxlot.SoftCommission = 0.0;
                                    taxlot.OtherBrokerFees = 0.0;
                                    taxlot.ClearingBrokerFee = 0.0;
                                }
                            }
                            if (totalCommission != 0)
                            {
                                allocatedGroup.Commission = totalCommission;
                            }
                            if (totalSoftCommission != 0)
                            {
                                allocatedGroup.SoftCommission = totalSoftCommission;
                            }
                            if (totalOtherBrokerFee != 0)
                            {
                                allocatedGroup.OtherBrokerFees = totalOtherBrokerFee;
                            }
                            if (totalClearingBrokerFee != 0)
                            {
                                allocatedGroup.ClearingBrokerFee = totalClearingBrokerFee;
                            }
                            // to calculate MarkeT Fees correctly when Basis is 'Commission' or 'Notional plus Commission'
                            //CalculateOtherFees(lstOtherFeeRule, allocatedGroup);

                            /* Calculate otherFees like StampDuty, ClearingFees etc on the basis of CommissionSources like Auto & Manual
                             *  If CommissionSource is Auto then commission will be calculated on the basis of CommissionRules
                             *  If CommissionSource is Manual then commission will be equal to CommissionAmount provided from TT
                            */
                            //PRANA-11213
                            //corrected calculation of other fee, PRANA-12889
                            if (allocatedGroup.CommSource != CommisionSource.Auto)
                            {
                                allocatedGroup.Commission = allocatedGroup.CommissionAmt;
                            }
                            if (allocatedGroup.SoftCommSource != CommisionSource.Auto)
                            {
                                allocatedGroup.SoftCommission = allocatedGroup.SoftCommissionAmt;
                            }
                            CalculateOtherFees(lstOtherFeeRule, allocatedGroup, isCommissionAndFeeZero);
                        }
                    }

                    else // for PreAllocation Commission Calculation and PreAllocated Trades
                    {
                        string commissionText = "calculated";
                        allocatedGroup.CommissionCalculationTime = commissionCalculationTime;
                        if (allocatedGroup.IsCommissionCalculated == false)// means commission and Fee not calculated 
                        {
                            if (!isCommissionAndFeeZero)
                                commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUEC(allocatedGroup.CounterPartyID, allocatedGroup.VenueID, allocatedGroup.AUECID, allocatedGroup.ListID, ref commissionText);

                            if (commissionRule != null)
                            {
                                double finalCalculatedCommission = double.MinValue;
                                double finalCalculatedSoftCommission = double.MinValue;
                                CalculateCommissionValue(commissionRule, allocatedGroup.CumQty, allocatedGroup.AvgPrice, allocatedGroup.ContractMultiplier,
                                    ref finalCalculatedCommission, ref finalCalculatedSoftCommission);

                                if (finalCalculatedCommission > double.MinValue)
                                {
                                    allocatedGroup.Commission = finalCalculatedCommission;
                                    DistributeCommisionInTaxLot(allocatedGroup);
                                }

                                if (finalCalculatedSoftCommission > double.MinValue)
                                {
                                    allocatedGroup.SoftCommission = finalCalculatedSoftCommission;
                                    DistributeSoftCommisionInTaxLot(allocatedGroup);
                                }

                                CalculateFeesGroupwise(commissionRule, allocatedGroup);
                            }
                            else
                            {
                                //allocatedGroup.Commission = 0.0;
                                //allocatedGroup.OtherBrokerFees = 0.0;
                                allocatedGroup.CommissionText = commissionText;

                                allocatedGroup.DistributeCommisionInTaxLot(true, true);
                                allocatedGroup.DistributeFeesInTaxLot();
                            }
                            // to calculate MarkeT Fees correctly when Basis is 'Commission' or 'Notional plus Commission'
                            //CalculateOtherFees(lstOtherFeeRule, allocatedGroup);

                            /* Calculate otherFees like StampDuty, ClearingFees etc on the basis of CommissionSources like Auto & Manual
                             *  If CommissionSource is Auto then commission will be calculated on the basis of CommissionRules
                             *  If CommissionSource is Manual then commission will be equal to CommissionAmount provided from TT
                            */
                            //PRANA-11213
                            //corrected calculation of other fee, PRANA-12889
                            if (allocatedGroup.CommSource != CommisionSource.Auto)
                            {
                                allocatedGroup.Commission = allocatedGroup.CommissionAmt;
                            }
                            if (allocatedGroup.SoftCommSource != CommisionSource.Auto)
                            {
                                allocatedGroup.SoftCommission = allocatedGroup.SoftCommissionAmt;
                            }
                            CalculateOtherFees(lstOtherFeeRule, allocatedGroup, isCommissionAndFeeZero);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Divya:14022013 :  As now, we have given the option to apply commission on TT, thus if user apply some commission on TT, it will be calculated 
        /// at the order level. But if the calculation basis is AUTO,then commission will be calculated according to the counter party venue wise commission rules.
        /// </summary>
        /// <param name="order"></param>
        public void CalculateCommissionOrderWise(ref Order order)
        {
            double calculatedCommission = 0;
            double calculatedSoftCommission = 0;
            double commissionRate = 0.0;
            double softCommissionRate = 0.0;
            double notional = 0.0;
            CommissionRule rule = new CommissionRule();

            rule.Commission.RuleAppliedOn = (CalculationBasis)order.CalcBasis;
            rule.SoftCommission.RuleAppliedOn = (CalculationBasis)order.SoftCommissionCalcBasis;

            commissionRate = order.CommissionRate;
            softCommissionRate = order.SoftCommissionRate;

            switch (rule.Commission.RuleAppliedOn)
            {
                case CalculationBasis.Shares:
                    calculatedCommission = order.CumQty * commissionRate;
                    break;
                case CalculationBasis.Contracts:
                    calculatedCommission = order.CumQty * commissionRate;
                    break;
                case CalculationBasis.Notional:
                    notional = order.CumQty * order.AvgPrice * order.ContractMultiplier;
                    calculatedCommission = notional * commissionRate * 0.0001;
                    break;
                //case CalculationBasis.AvgPrice:
                //    commissionRate = GetCommissionRate(commissionRuleToApply, allocatedGroup.AvgPrice);
                //    calculatedCommission = taxlot.TaxLotQty * commissionRate;
                //    break;
                case CalculationBasis.FlatAmount:
                    calculatedCommission = commissionRate;
                    break;
                case CalculationBasis.FlatRateProrata:
                    calculatedCommission = commissionRate * (order.CumQty / order.Quantity);
                    break;
                default:
                    break;
            }

            switch (rule.SoftCommission.RuleAppliedOn)
            {
                case CalculationBasis.Shares:
                    calculatedSoftCommission = order.CumQty * softCommissionRate;
                    break;
                case CalculationBasis.Contracts:
                    calculatedSoftCommission = order.CumQty * softCommissionRate;
                    break;
                case CalculationBasis.Notional:
                    notional = order.CumQty * order.AvgPrice * order.ContractMultiplier;
                    calculatedSoftCommission = notional * softCommissionRate * 0.0001;
                    break;
                case CalculationBasis.FlatAmount:
                    calculatedSoftCommission = softCommissionRate;
                    break;
                case CalculationBasis.FlatRateProrata:
                    calculatedSoftCommission = softCommissionRate * (order.CumQty / order.Quantity);
                    break;
                default:
                    break;
            }

            order.CommissionAmt = calculatedCommission;
            order.SoftCommissionAmt = calculatedSoftCommission;

        }
        public TaxLot CalculateCommissionAccountWise(CommissionRule commissionRuleToApply, TaxLot taxlot, AllocationGroup allocatedGroup)
        {
            try
            {
                double calculatedCommission = 0;
                double calculatedSoftCommission = 0;
                double finalCalculatedCommission = 0;
                double finalCalculatedSoftCommission = 0;
                double commissionRate = 0.0;
                double softCommissionRate = 0.0;
                double notional = 0.0;

                switch (commissionRuleToApply.Commission.RuleAppliedOn)
                {
                    case CalculationBasis.Shares:
                        commissionRate = GetCommissionRate(commissionRuleToApply.Commission, taxlot.TaxLotQty);
                        calculatedCommission = taxlot.TaxLotQty * commissionRate;
                        break;
                    case CalculationBasis.Contracts:
                        commissionRate = GetCommissionRate(commissionRuleToApply.Commission, taxlot.TaxLotQty);
                        calculatedCommission = taxlot.TaxLotQty * commissionRate;
                        break;
                    case CalculationBasis.Notional:
                        notional = GetNotionalValueForAccount(allocatedGroup, taxlot);
                        commissionRate = GetCommissionRate(commissionRuleToApply.Commission, notional);
                        // calculatedCommission = notional * commissionRuleToApply.CommissionRate; previous one
                        calculatedCommission = notional * commissionRate * 0.0001;
                        break;
                    case CalculationBasis.AvgPrice:
                        commissionRate = GetCommissionRate(commissionRuleToApply.Commission, allocatedGroup.AvgPrice);
                        calculatedCommission = taxlot.TaxLotQty * commissionRate;
                        break;
                    case CalculationBasis.FlatAmount:
                        calculatedCommission = commissionRuleToApply.Commission.CommissionRate;
                        break;
                    default:
                        throw new Exception("Commission rule basis not set.");
                }

                switch (commissionRuleToApply.SoftCommission.RuleAppliedOn)
                {
                    case CalculationBasis.Shares:
                        softCommissionRate = GetCommissionRate(commissionRuleToApply.SoftCommission, taxlot.TaxLotQty);
                        calculatedSoftCommission = taxlot.TaxLotQty * softCommissionRate;
                        break;
                    case CalculationBasis.Contracts:
                        softCommissionRate = GetCommissionRate(commissionRuleToApply.SoftCommission, taxlot.TaxLotQty);
                        calculatedSoftCommission = taxlot.TaxLotQty * softCommissionRate;
                        break;
                    case CalculationBasis.Notional:
                        notional = GetNotionalValueForAccount(allocatedGroup, taxlot);
                        softCommissionRate = GetCommissionRate(commissionRuleToApply.SoftCommission, notional);
                        // calculatedCommission = notional * commissionRuleToApply.CommissionRate; previous one
                        calculatedSoftCommission = notional * softCommissionRate * 0.0001;
                        break;
                    case CalculationBasis.AvgPrice:
                        softCommissionRate = GetCommissionRate(commissionRuleToApply.SoftCommission, allocatedGroup.AvgPrice);
                        calculatedSoftCommission = taxlot.TaxLotQty * softCommissionRate;
                        break;
                    case CalculationBasis.FlatAmount:
                        calculatedSoftCommission = commissionRuleToApply.SoftCommission.CommissionRate;
                        break;
                    default:
                        throw new Exception("Soft Commission rule basis not set.");
                }

                if (calculatedCommission <= commissionRuleToApply.Commission.MinCommission && allocatedGroup.CumQty > 0)
                {
                    ///If calculatedCommission is less than the absolute minimum commission, then we use the 
                    ///minimum commission as the final commission
                    finalCalculatedCommission = commissionRuleToApply.Commission.MinCommission;
                }
                if (commissionRuleToApply.Commission.MaxCommission != 0 && commissionRuleToApply.Commission.MaxCommission != double.MinValue && allocatedGroup.CumQty > 0)
                {
                    if (calculatedCommission >= commissionRuleToApply.Commission.MaxCommission)
                    {
                        finalCalculatedCommission = commissionRuleToApply.Commission.MaxCommission;
                    }
                    if (calculatedCommission < commissionRuleToApply.Commission.MaxCommission && calculatedCommission > commissionRuleToApply.Commission.MinCommission)
                    {
                        finalCalculatedCommission = calculatedCommission;
                    }
                }
                else if (calculatedCommission > commissionRuleToApply.Commission.MinCommission)
                {
                    finalCalculatedCommission = calculatedCommission;
                }

                //Soft Commission
                if (calculatedSoftCommission <= commissionRuleToApply.SoftCommission.MinCommission && allocatedGroup.CumQty > 0)
                {
                    ///If calculatedCommission is less than the absolute minimum commission, then we use the 
                    ///minimum commission as the final commission
                    finalCalculatedSoftCommission = commissionRuleToApply.SoftCommission.MinCommission;
                }
                if (commissionRuleToApply.SoftCommission.MaxCommission != 0 && commissionRuleToApply.SoftCommission.MaxCommission != double.MinValue && allocatedGroup.CumQty > 0)
                {
                    if (calculatedSoftCommission >= commissionRuleToApply.SoftCommission.MaxCommission)
                    {
                        finalCalculatedSoftCommission = commissionRuleToApply.SoftCommission.MaxCommission;
                    }
                    if (calculatedSoftCommission < commissionRuleToApply.SoftCommission.MaxCommission && calculatedSoftCommission > commissionRuleToApply.SoftCommission.MinCommission)
                    {
                        finalCalculatedSoftCommission = calculatedSoftCommission;
                    }
                }
                else if (calculatedSoftCommission > commissionRuleToApply.SoftCommission.MinCommission)
                {
                    finalCalculatedSoftCommission = calculatedSoftCommission;
                }

                taxlot.Commission = finalCalculatedCommission;
                taxlot.SoftCommission = finalCalculatedSoftCommission;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            return taxlot;
        }

        private double GetCommissionRate(Prana.BusinessObjects.Commission commissionRuleToApply, double valueToCheck)
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

        /// <summary>
        /// Gets the clearing fee rate.
        /// </summary>
        /// <param name="clearingFeeToApply">The clearing fee to apply.</param>
        /// <param name="valueToCheck">The value to check.</param>
        /// <returns></returns>
        private double GetClearingFeeRate(Prana.BusinessObjects.ClearingFee clearingFeeToApply, double valueToCheck)
        {
            try
            {
                if (clearingFeeToApply.IsCriteriaApplied)
                {
                    List<ClearingFeeCriteria> commissionRuleCriteriaList = clearingFeeToApply.ClearingFeeRuleCriteiaList;
                    foreach (ClearingFeeCriteria clearingFeeCriteria in commissionRuleCriteriaList)
                    {
                        // special handling for ValueLessThanOrEqual
                        double valueLessThanOrEqual = double.MinValue;
                        if (clearingFeeCriteria.ValueLessThanOrEqual == 0)
                        {
                            valueLessThanOrEqual = double.MaxValue;
                        }
                        else
                        {
                            valueLessThanOrEqual = clearingFeeCriteria.ValueLessThanOrEqual;
                        }

                        if ((clearingFeeCriteria.ValueGreaterThan < valueToCheck) && (valueLessThanOrEqual >= valueToCheck))
                        {
                            return clearingFeeCriteria.ClearingFeeRate;
                        }
                    }
                }
                return clearingFeeToApply.ClearingFeeRate;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return 0.0;
        }



        private double GetNotionalValueForAccount(AllocationGroup allocationGroup, TaxLot taxlot)
        {
            //double MultiplierFactor = double.MinValue;
            return taxlot.TaxLotQty * allocationGroup.AvgPrice * allocationGroup.ContractMultiplier;
            //AssetCategory asset = (AssetCategory)allocationGroup.AssetID;
            //switch (asset)
            //{
            //    case AssetCategory.None:
            //        return 0.0;

            //    case AssetCategory.Equity:
            //        //  multiplier is 1 in case of Equity
            //        MultiplierFactor = 1;
            //        return taxlot.TaxLotQty * allocationGroup.AvgPrice * MultiplierFactor;

            //    case AssetCategory.EquityOption:
            //        //  multiplier is 100 in case of EquityOption
            //        MultiplierFactor = 100;
            //        return taxlot.TaxLotQty * allocationGroup.AvgPrice * MultiplierFactor;

            //    case AssetCategory.Future:
            //    case AssetCategory.Forward:
            //        // get the multiplier when Asset Type is Future
            //        //MultiplierFactor = Prana.CommonDataCache.CachedDataManager.GetInstance.GetContractMultiplierBySymbol(order.Symbol.Substring(0, order.Symbol.IndexOf(" ")));
            //        //return order.Quantity * order.AvgPrice * MultiplierFactor;
            //        break;
            //    case AssetCategory.FutureOption:
            //        //TODO: for the time being Multiplier factor is MultiplierFactor=1 but Need to get exact value of MultiplierFactor for FutureOption
            //        //MultiplierFactor = 1;
            //        //return order.Quantity * order.AvgPrice * MultiplierFactor;
            //        break;
            //    //case AssetCategory.FutureOption:
            //    //    break;
            //    //case AssetCategory.ForeignExchange:
            //    //    break;
            //    //case AssetCategory.Cash:
            //    //    break;
            //    //case AssetCategory.Indices:
            //    //    break;
            //    default:
            //        return 0.0;
            //        //throw new Exception("Asset category not set.");
            //}
            //return 0.0;
        }

        public TaxLot CalculateFeesAccountWise(CommissionRule commissionRuleToApply, TaxLot taxlot, AllocationGroup allocationGroup)
        {
            try
            {
                double calculatedFees = 0;
                double finalCalculatedFees = 0;
                double notional = 0.0;
                ///Fees Calculation
                if (commissionRuleToApply.IsClearingFeeApplied)
                {
                    switch (commissionRuleToApply.ClearingFeeObj.RuleAppliedOn)
                    {
                        case CalculationBasis.Shares:
                            calculatedFees = taxlot.TaxLotQty * commissionRuleToApply.ClearingFeeObj.ClearingFeeRate;
                            break;
                        case CalculationBasis.Contracts: ///qty in this case is contract quantity
                            calculatedFees = taxlot.TaxLotQty * commissionRuleToApply.ClearingFeeObj.ClearingFeeRate;
                            break;
                        case CalculationBasis.Notional:
                            notional = GetNotionalValueForAccount(allocationGroup, taxlot);
                            calculatedFees = notional * commissionRuleToApply.ClearingFeeObj.ClearingFeeRate * 0.0001;
                            break;
                        case CalculationBasis.FlatAmount:
                            calculatedFees = commissionRuleToApply.ClearingFeeObj.ClearingFeeRate;
                            break;
                        default:
                            throw new Exception("Commission rule basis not set. It should depend either on Shares,Contracts or Notional values.");
                    }

                    if (calculatedFees < commissionRuleToApply.ClearingFeeObj.MinClearingFee && allocationGroup.CumQty > 0)
                    {
                        ///If calculatedFees is less than the absolute minimum ClearingFee, then we use the 
                        ///minimum fees as the final commission
                        finalCalculatedFees = commissionRuleToApply.ClearingFeeObj.MinClearingFee;
                    }
                    else
                    {
                        finalCalculatedFees = calculatedFees;
                    }

                    taxlot.OtherBrokerFees = finalCalculatedFees;// System.Math.Round(finalCalculatedFees, 2);
                }

                calculatedFees = 0;
                finalCalculatedFees = 0;
                notional = 0.0;
                ///Fees Calculation
                if (commissionRuleToApply.IsClearingBrokerFeeApplied)
                {
                    switch (commissionRuleToApply.ClearingBrokerFeeObj.RuleAppliedOn)
                    {
                        case CalculationBasis.Shares:
                            calculatedFees = taxlot.TaxLotQty * commissionRuleToApply.ClearingBrokerFeeObj.ClearingFeeRate;
                            break;
                        case CalculationBasis.Contracts: ///qty in this case is contract quantity
                            calculatedFees = taxlot.TaxLotQty * commissionRuleToApply.ClearingBrokerFeeObj.ClearingFeeRate;
                            break;
                        case CalculationBasis.Notional:
                            notional = GetNotionalValueForAccount(allocationGroup, taxlot);
                            calculatedFees = notional * commissionRuleToApply.ClearingBrokerFeeObj.ClearingFeeRate * 0.0001;
                            break;
                        case CalculationBasis.FlatAmount:
                            calculatedFees = commissionRuleToApply.ClearingBrokerFeeObj.ClearingFeeRate;
                            break;
                        default:
                            throw new Exception("Commission rule basis not set. It should depend either on Shares,Contracts or Notional values.");
                    }

                    if (calculatedFees < commissionRuleToApply.ClearingBrokerFeeObj.MinClearingFee && allocationGroup.CumQty > 0)
                    {
                        ///If calculatedFees is less than the absolute minimum ClearingBrokerFee, then we use the 
                        ///minimum fees as the final commission
                        finalCalculatedFees = commissionRuleToApply.ClearingBrokerFeeObj.MinClearingFee;
                    }
                    else
                    {
                        finalCalculatedFees = calculatedFees;
                    }

                    taxlot.ClearingBrokerFee = finalCalculatedFees;// System.Math.Round(finalCalculatedFees, 2);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return taxlot;
        }

        public void CalculateCommissionValue(CommissionRule commissionRuleToApply, double cumQty, double avgPrice, double contractMultiplier,
            ref double finalCalculatedCommission, ref double finalCalculatedSoftCommission)
        {
            try
            {
                double calculatedCommission = 0;
                double calculatedSoftCommission = 0;
                double commissionRate = 0.0;
                double softCommissionRate = 0.0;
                double notional = 0.0;

                if (commissionRuleToApply.Commission.CommissionRate != double.MinValue)
                {
                    switch (commissionRuleToApply.Commission.RuleAppliedOn)
                    {
                        case CalculationBasis.Shares:
                            commissionRate = GetCommissionRate(commissionRuleToApply.Commission, cumQty);
                            calculatedCommission = cumQty * commissionRate;
                            break;
                        case CalculationBasis.Contracts:
                            commissionRate = GetCommissionRate(commissionRuleToApply.Commission, cumQty);
                            calculatedCommission = cumQty * commissionRate;
                            break;
                        case CalculationBasis.Notional:
                            notional = GetNotionalValue(cumQty, avgPrice, contractMultiplier);
                            commissionRate = GetCommissionRate(commissionRuleToApply.Commission, notional);
                            // calculatedCommission = notional * commissionRuleToApply.CommissionRate; previous one
                            calculatedCommission = notional * commissionRate * 0.0001;
                            break;
                        case CalculationBasis.AvgPrice:
                            commissionRate = GetCommissionRate(commissionRuleToApply.Commission, avgPrice);
                            calculatedCommission = cumQty * commissionRate;
                            break;
                        case CalculationBasis.FlatAmount:
                            calculatedCommission = commissionRuleToApply.Commission.CommissionRate;
                            break;
                        default:
                            throw new Exception("Commission rule basis not set.");
                    }

                    if (calculatedCommission <= commissionRuleToApply.Commission.MinCommission && cumQty > 0)
                    {
                        ///If calculatedCommission is less than the absolute minimum commission, then we use the 
                        ///minimum commission as the final commission
                        finalCalculatedCommission = commissionRuleToApply.Commission.MinCommission;
                    }
                    if (commissionRuleToApply.Commission.MaxCommission != 0 && commissionRuleToApply.Commission.MaxCommission != double.MinValue && cumQty > 0)
                    {
                        if (calculatedCommission >= commissionRuleToApply.Commission.MaxCommission)
                        {
                            finalCalculatedCommission = commissionRuleToApply.Commission.MaxCommission;
                        }
                        if (calculatedCommission < commissionRuleToApply.Commission.MaxCommission && calculatedCommission > commissionRuleToApply.Commission.MinCommission)
                        {
                            finalCalculatedCommission = calculatedCommission;
                        }
                    }
                    else if (calculatedCommission > commissionRuleToApply.Commission.MinCommission)
                    {
                        finalCalculatedCommission = calculatedCommission;
                    }

                }

                if (commissionRuleToApply.SoftCommission.CommissionRate != double.MinValue)
                {
                    //Soft Commission
                    switch (commissionRuleToApply.SoftCommission.RuleAppliedOn)
                    {
                        case CalculationBasis.Shares:
                            softCommissionRate = GetCommissionRate(commissionRuleToApply.SoftCommission, cumQty);
                            calculatedSoftCommission = cumQty * softCommissionRate;
                            break;
                        case CalculationBasis.Contracts:
                            softCommissionRate = GetCommissionRate(commissionRuleToApply.SoftCommission, cumQty);
                            calculatedSoftCommission = cumQty * softCommissionRate;
                            break;
                        case CalculationBasis.Notional:
                            notional = GetNotionalValue(cumQty, avgPrice, contractMultiplier);
                            softCommissionRate = GetCommissionRate(commissionRuleToApply.SoftCommission, notional);
                            // calculatedCommission = notional * commissionRuleToApply.CommissionRate; previous one
                            calculatedSoftCommission = notional * softCommissionRate * 0.0001;
                            break;
                        case CalculationBasis.AvgPrice:
                            softCommissionRate = GetCommissionRate(commissionRuleToApply.SoftCommission, avgPrice);
                            calculatedSoftCommission = cumQty * softCommissionRate;
                            break;
                        case CalculationBasis.FlatAmount:
                            calculatedSoftCommission = commissionRuleToApply.SoftCommission.CommissionRate;
                            break;
                        default:
                            throw new Exception("Soft Commission rule basis not set.");
                    }

                    if (calculatedSoftCommission <= commissionRuleToApply.SoftCommission.MinCommission && cumQty > 0)
                    {
                        ///If calculatedCommission is less than the absolute minimum commission, then we use the 
                        ///minimum commission as the final commission
                        finalCalculatedSoftCommission = commissionRuleToApply.SoftCommission.MinCommission;
                    }
                    if (commissionRuleToApply.SoftCommission.MaxCommission != 0 && commissionRuleToApply.SoftCommission.MaxCommission != double.MinValue && cumQty > 0)
                    {
                        if (calculatedSoftCommission >= commissionRuleToApply.SoftCommission.MaxCommission)
                        {
                            finalCalculatedSoftCommission = commissionRuleToApply.SoftCommission.MaxCommission;
                        }
                        if (calculatedSoftCommission < commissionRuleToApply.SoftCommission.MaxCommission && calculatedSoftCommission > commissionRuleToApply.SoftCommission.MinCommission)
                        {
                            finalCalculatedSoftCommission = calculatedSoftCommission;
                        }
                    }
                    else if (calculatedSoftCommission > commissionRuleToApply.SoftCommission.MinCommission)
                    {
                        finalCalculatedSoftCommission = calculatedSoftCommission;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public AllocationGroup CalculateFeesGroupwise(CommissionRule commissionRuleToApply, AllocationGroup allocatedGroup)
        {
            try
            {
                double calculatedFees = 0;
                double finalCalculatedFees = 0;
                double notional = 0.0;

                if (commissionRuleToApply.ClearingFeeObj.ClearingFeeRate != double.MinValue)
                {
                    ///Fees Calculation
                    if (commissionRuleToApply.IsClearingFeeApplied)
                    {
                        switch (commissionRuleToApply.ClearingFeeObj.RuleAppliedOn)
                        {
                            case CalculationBasis.Shares:
                                calculatedFees = allocatedGroup.CumQty * commissionRuleToApply.ClearingFeeObj.ClearingFeeRate;
                                break;
                            case CalculationBasis.Contracts: ///qty in this case is contract quantity
                                calculatedFees = allocatedGroup.CumQty * commissionRuleToApply.ClearingFeeObj.ClearingFeeRate;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValue(allocatedGroup.CumQty, allocatedGroup.AvgPrice, allocatedGroup.ContractMultiplier);
                                calculatedFees = notional * commissionRuleToApply.ClearingFeeObj.ClearingFeeRate * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedFees = commissionRuleToApply.ClearingFeeObj.ClearingFeeRate;
                                break;

                            default:
                                throw new Exception("Commission rule basis not set. It should depend either of Shares,Contracts or Notional values.");
                        }

                        if (calculatedFees < commissionRuleToApply.ClearingFeeObj.MinClearingFee && allocatedGroup.CumQty > 0)
                        {
                            ///If calculatedFees is less than the absolute minimum ClearingFee, then we use the 
                            ///minimum fees as the final commission
                            finalCalculatedFees = commissionRuleToApply.ClearingFeeObj.MinClearingFee;
                        }
                        else
                        {
                            finalCalculatedFees = calculatedFees;
                        }
                    }

                    allocatedGroup.OtherBrokerFees = finalCalculatedFees;// System.Math.Round(finalCalculatedFees, 2);
                }

                calculatedFees = 0;
                finalCalculatedFees = 0;
                notional = 0.0;
                if (commissionRuleToApply.ClearingBrokerFeeObj.ClearingFeeRate != double.MinValue)
                {
                    ///Fees Calculation
                    if (commissionRuleToApply.IsClearingBrokerFeeApplied)
                    {
                        switch (commissionRuleToApply.ClearingBrokerFeeObj.RuleAppliedOn)
                        {
                            case CalculationBasis.Shares:
                                calculatedFees = allocatedGroup.CumQty * commissionRuleToApply.ClearingBrokerFeeObj.ClearingFeeRate;
                                break;
                            case CalculationBasis.Contracts: ///qty in this case is contract quantity
                                calculatedFees = allocatedGroup.CumQty * commissionRuleToApply.ClearingBrokerFeeObj.ClearingFeeRate;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValue(allocatedGroup.CumQty, allocatedGroup.AvgPrice, allocatedGroup.ContractMultiplier);
                                calculatedFees = notional * commissionRuleToApply.ClearingBrokerFeeObj.ClearingFeeRate * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedFees = commissionRuleToApply.ClearingBrokerFeeObj.ClearingFeeRate;
                                break;

                            default:
                                throw new Exception("Commission rule basis not set. It should depend either of Shares,Contracts or Notional values.");
                        }

                        if (calculatedFees < commissionRuleToApply.ClearingBrokerFeeObj.MinClearingFee && allocatedGroup.CumQty > 0)
                        {
                            ///If calculatedFees is less than the absolute minimum ClearingFee, then we use the 
                            ///minimum fees as the final commission
                            finalCalculatedFees = commissionRuleToApply.ClearingBrokerFeeObj.MinClearingFee;
                        }
                        else
                        {
                            finalCalculatedFees = calculatedFees;
                        }
                    }

                    allocatedGroup.ClearingBrokerFee = finalCalculatedFees;// System.Math.Round(finalCalculatedFees, 2);
                }
                DistributeFeesInTaxLot(allocatedGroup);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            return allocatedGroup;
        }

        public AllocationGroup CalculateOtherFees(List<OtherFeeRule> otherFeeRuleList, AllocationGroup group, bool isCommissionAndFeeZero)
        {
            try
            {
                if (otherFeeRuleList.Count > 0)
                {
                    foreach (OtherFeeRule otherFeeRule in otherFeeRuleList)
                    {
                        double calculatedOtherFees = 0.0;
                        if (!isCommissionAndFeeZero)
                        {
                            switch (group.OrderSideTagValue)
                            {
                                case FIXConstants.SIDE_Buy:
                                case FIXConstants.SIDE_Buy_Closed:
                                case FIXConstants.SIDE_Buy_Cover:
                                case FIXConstants.SIDE_Buy_Open:
                                    switch (otherFeeRule.LongCalculationBasis)
                                    {
                                        case CalculationFeeBasis.Shares:
                                            calculatedOtherFees = group.CumQty * otherFeeRule.LongRate;
                                            break;
                                        case CalculationFeeBasis.Contracts:
                                            calculatedOtherFees = group.CumQty * otherFeeRule.LongRate;
                                            break;
                                        case CalculationFeeBasis.Notional:
                                            calculatedOtherFees = GetNotionalValue(group.CumQty, group.AvgPrice, group.ContractMultiplier) * otherFeeRule.LongRate * 0.0001;
                                            break;
                                        case CalculationFeeBasis.Commission:
                                            calculatedOtherFees = group.Commission * otherFeeRule.LongRate * 0.0001;
                                            break;
                                        case CalculationFeeBasis.NotionalPlusCommission:
                                            calculatedOtherFees = (GetNotionalValue(group.CumQty, group.AvgPrice, group.ContractMultiplier) + group.Commission) * otherFeeRule.LongRate * 0.0001;
                                            break;
                                        case CalculationFeeBasis.FlatAmount:
                                            calculatedOtherFees = otherFeeRule.LongRate;
                                            break;
                                        default:
                                            throw new Exception("Commission rule basis not set. It should depend either of Shares,Contracts , Notional values,Commissionor NotionalPlusCommission.");

                                    }
                                    break;
                                default:
                                    switch (otherFeeRule.ShortCalculationBasis)
                                    {
                                        case CalculationFeeBasis.Shares:
                                            calculatedOtherFees = group.CumQty * otherFeeRule.ShortRate;
                                            break;
                                        case CalculationFeeBasis.Contracts:
                                            calculatedOtherFees = group.CumQty * otherFeeRule.ShortRate;
                                            break;
                                        case CalculationFeeBasis.Notional:
                                            calculatedOtherFees = GetNotionalValue(group.CumQty, group.AvgPrice, group.ContractMultiplier) * otherFeeRule.ShortRate * 0.0001;
                                            break;
                                        case CalculationFeeBasis.Commission:
                                            calculatedOtherFees = group.Commission * otherFeeRule.ShortRate * 0.0001;
                                            break;
                                        case CalculationFeeBasis.NotionalPlusCommission:
                                            calculatedOtherFees = (GetNotionalValue(group.CumQty, group.AvgPrice, group.ContractMultiplier) + group.Commission) * otherFeeRule.ShortRate * 0.0001;
                                            break;
                                        case CalculationFeeBasis.FlatAmount:
                                            calculatedOtherFees = otherFeeRule.ShortRate;
                                            break;
                                        default:
                                            throw new Exception("Commission rule basis not set. It should depend either of Shares,Contracts , Notional values,Commissionor NotionalPlusCommission.");
                                    }
                                    break;
                            }

                            //Shubham Awasthi (2015-09-08)
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-10682
                            if (otherFeeRule.FeePrecisionType == FeePrecisionType.RoundUp)
                            {
                                calculatedOtherFees = (double)Math.Ceiling(Convert.ToDecimal(calculatedOtherFees * Math.Pow(10, otherFeeRule.RoundUpPrecision))) / Math.Pow(10, otherFeeRule.RoundUpPrecision);
                            }
                            else if (otherFeeRule.FeePrecisionType == FeePrecisionType.RoundDown)
                            {
                                calculatedOtherFees = (double)Math.Floor(Convert.ToDecimal(calculatedOtherFees * Math.Pow(10, otherFeeRule.RoundDownPrecision))) / Math.Pow(10, otherFeeRule.RoundDownPrecision);
                            }
                            else
                            {
                                calculatedOtherFees = System.Math.Round(calculatedOtherFees, otherFeeRule.RoundOffPrecision, MidpointRounding.AwayFromZero);
                            }
                            if (calculatedOtherFees > otherFeeRule.MaxValue && otherFeeRule.MaxValue != 0.0)
                            {
                                calculatedOtherFees = otherFeeRule.MaxValue;
                            }
                            else if (calculatedOtherFees < otherFeeRule.MinValue && otherFeeRule.MinValue != 0.0)
                            {
                                calculatedOtherFees = otherFeeRule.MinValue;
                            }
                        }
                        switch (otherFeeRule.OtherFeeType)
                        {
                            case OtherFeeType.StampDuty:
                                group.StampDuty = calculatedOtherFees;
                                break;
                            case OtherFeeType.TransactionLevy:
                                group.TransactionLevy = calculatedOtherFees;
                                break;
                            case OtherFeeType.ClearingFee:
                                group.ClearingFee = calculatedOtherFees;
                                break;
                            case OtherFeeType.TaxOnCommissions:
                                group.TaxOnCommissions = calculatedOtherFees;
                                break;
                            case OtherFeeType.MiscFees:
                                group.MiscFees = calculatedOtherFees;
                                break;
                            case OtherFeeType.SecFee:
                                group.SecFee = calculatedOtherFees;
                                break;
                            case OtherFeeType.OccFee:
                                group.OccFee = calculatedOtherFees;
                                break;
                            case OtherFeeType.OrfFee:
                                group.OrfFee = calculatedOtherFees;
                                break;
                            default:
                                throw new Exception("this is not a valid Fee type");
                        }

                    }
                    DistributeOtherFeesInTaxLot(group);

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            return group;
        }

        private void DistributeOtherFeesInTaxLot(AllocationGroup group)
        {
            foreach (TaxLot taxlot in group.TaxLots)
            {
                if (group.AllocatedQty > 0)
                {
                    taxlot.StampDuty = (group.StampDuty * (taxlot.TaxLotQty / group.AllocatedQty));
                    taxlot.TaxOnCommissions = (group.TaxOnCommissions * (taxlot.TaxLotQty / group.AllocatedQty));
                    taxlot.TransactionLevy = (group.TransactionLevy * (taxlot.TaxLotQty / group.AllocatedQty));
                    taxlot.ClearingFee = (group.ClearingFee * (taxlot.TaxLotQty / group.AllocatedQty));
                    taxlot.MiscFees = (group.MiscFees * (taxlot.TaxLotQty / group.AllocatedQty));
                    taxlot.SecFee = (group.SecFee * (taxlot.TaxLotQty / group.AllocatedQty));
                    taxlot.OccFee = (group.OccFee * (taxlot.TaxLotQty / group.AllocatedQty));
                    taxlot.OrfFee = (group.OrfFee * (taxlot.TaxLotQty / group.AllocatedQty));
                }
            }
        }
        public void DistributeFeesInTaxLot(AllocationGroup group)
        {
            foreach (TaxLot taxlot in group.TaxLots)
            {
                if (group.AllocatedQty > 0)
                {
                    taxlot.OtherBrokerFees = (group.OtherBrokerFees * (taxlot.TaxLotQty / group.AllocatedQty));
                    taxlot.ClearingBrokerFee = (group.ClearingBrokerFee * (taxlot.TaxLotQty / group.AllocatedQty));
                }
            }
        }
        public void DistributeCommisionInTaxLot(AllocationGroup group)
        {
            foreach (TaxLot taxlot in group.TaxLots)
            {
                if (group.AllocatedQty > 0)
                {
                    taxlot.Commission = (group.Commission * (taxlot.TaxLotQty / group.AllocatedQty));
                }
            }
        }
        public void DistributeSoftCommisionInTaxLot(AllocationGroup group)
        {
            foreach (TaxLot taxlot in group.TaxLots)
            {
                if (group.AllocatedQty > 0)
                {
                    taxlot.SoftCommission = (group.SoftCommission * (taxlot.TaxLotQty / group.AllocatedQty));
                }
            }
        }

        /// <summary>
        /// Gets the notional value.
        /// </summary>
        /// <param name="qty">The qty.</param>
        /// <param name="avgPrice">The average price.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns></returns>
        private double GetNotionalValue(double qty, double avgPrice, double multiplier)
        {
            return qty * avgPrice * multiplier;
        }

        public void CalculateOtherFeeGroupwise(CommissionRule commissionRuleToApply, ref AllocationGroup allocationGroup, Prana.BusinessObjects.AppConstants.OtherFeeType FeeType)
        {
            try
            {
                double calculatedValue = 0;
                double notional = 0.0;

                switch (FeeType)
                {
                    case Prana.BusinessObjects.AppConstants.OtherFeeType.StampDuty:

                        switch (commissionRuleToApply.StampDutyCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts:
                                calculatedValue = allocationGroup.CumQty * commissionRuleToApply.StampDuty;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                                calculatedValue = notional * commissionRuleToApply.StampDuty * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedValue = commissionRuleToApply.StampDuty;
                                break;
                            default:
                                throw new Exception("Stamp Duty rule basis not set.");
                        }

                        allocationGroup.StampDuty = calculatedValue;
                        allocationGroup.DistributeOtherFeesInTaxLot();
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.ClearingFee:

                        switch (commissionRuleToApply.ClearingFeeCalculationBasedOn_A)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts:
                                calculatedValue = allocationGroup.CumQty * commissionRuleToApply.ClearingFee_A;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                                calculatedValue = notional * commissionRuleToApply.ClearingFee_A * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedValue = commissionRuleToApply.ClearingFee_A;
                                break;
                            default:
                                throw new Exception("Clearing Fee rule basis not set.");
                        }

                        allocationGroup.ClearingFee = calculatedValue;
                        allocationGroup.DistributeOtherFeesInTaxLot();
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.TaxOnCommissions:

                        switch (commissionRuleToApply.TaxonCommissionsCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts:
                                calculatedValue = allocationGroup.CumQty * commissionRuleToApply.TaxonCommissions;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                                calculatedValue = notional * commissionRuleToApply.TaxonCommissions * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedValue = commissionRuleToApply.TaxonCommissions;
                                break;
                            default:
                                throw new Exception("Tax on Commission rule basis not set.");
                        }

                        allocationGroup.TaxOnCommissions = calculatedValue;
                        allocationGroup.DistributeOtherFeesInTaxLot();
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.TransactionLevy:

                        switch (commissionRuleToApply.TransactionLevyCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts:
                                calculatedValue = allocationGroup.CumQty * commissionRuleToApply.TransactionLevy;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                                calculatedValue = notional * commissionRuleToApply.TransactionLevy * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedValue = commissionRuleToApply.TransactionLevy;
                                break;
                            default:
                                throw new Exception("Transaction Levy rule basis not set.");
                        }

                        allocationGroup.TransactionLevy = calculatedValue;
                        allocationGroup.DistributeOtherFeesInTaxLot();
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.MiscFees:

                        switch (commissionRuleToApply.MiscFeesCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts:
                                calculatedValue = allocationGroup.CumQty * commissionRuleToApply.MiscFees;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                                calculatedValue = notional * commissionRuleToApply.MiscFees * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedValue = commissionRuleToApply.MiscFees;
                                break;
                            default:
                                throw new Exception("Misc Fees rule basis not set.");
                        }

                        allocationGroup.MiscFees = calculatedValue;
                        allocationGroup.DistributeOtherFeesInTaxLot();
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.SecFee:

                        switch (commissionRuleToApply.SecFeeCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts:
                                calculatedValue = allocationGroup.CumQty * commissionRuleToApply.SecFee;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                                calculatedValue = notional * commissionRuleToApply.SecFee * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedValue = commissionRuleToApply.SecFee;
                                break;
                            default:
                                throw new Exception("SEC Fee rule basis not set.");
                        }

                        allocationGroup.SecFee = calculatedValue;
                        allocationGroup.DistributeOtherFeesInTaxLot();
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.OccFee:

                        switch (commissionRuleToApply.OccFeeCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts:
                                calculatedValue = allocationGroup.CumQty * commissionRuleToApply.OccFee;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                                calculatedValue = notional * commissionRuleToApply.OccFee * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedValue = commissionRuleToApply.OccFee;
                                break;
                            default:
                                throw new Exception("OCC Fee rule basis not set.");
                        }

                        allocationGroup.OccFee = calculatedValue;
                        allocationGroup.DistributeOtherFeesInTaxLot();
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.OrfFee:

                        switch (commissionRuleToApply.OrfFeeCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts:
                                calculatedValue = allocationGroup.CumQty * commissionRuleToApply.OrfFee;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                                calculatedValue = notional * commissionRuleToApply.OrfFee * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedValue = commissionRuleToApply.OrfFee;
                                break;
                            default:
                                throw new Exception("ORF Fee rule basis not set.");
                        }

                        allocationGroup.OrfFee = calculatedValue;
                        allocationGroup.DistributeOtherFeesInTaxLot();
                        break;

                    default:
                        break;
                }


            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            //return allocationGroup;
        }

        public TaxLot CalculateOtherFeesAccountWise(CommissionRule commissionRuleToApply, TaxLot taxlot, AllocationGroup allocationGroup, Prana.BusinessObjects.AppConstants.OtherFeeType FeeType)
        {
            try
            {
                double calculatedFees = 0;
                double notional = 0.0;
                ///Fees Calculation
                ///
                switch (FeeType)
                {
                    case Prana.BusinessObjects.AppConstants.OtherFeeType.StampDuty:

                        switch (commissionRuleToApply.StampDutyCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts: ///qty in this case is contract quantity
                                calculatedFees = taxlot.TaxLotQty * commissionRuleToApply.StampDuty;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValueForAccount(allocationGroup, taxlot);
                                calculatedFees = notional * commissionRuleToApply.StampDuty * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedFees = commissionRuleToApply.StampDuty;
                                break;
                            default:
                                throw new Exception("Stamp Duty rule basis not set. It should depend either on Shares,Contracts or Notional values.");
                        }
                        taxlot.StampDuty = calculatedFees;
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.ClearingFee:

                        switch (commissionRuleToApply.ClearingFeeCalculationBasedOn_A)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts: ///qty in this case is contract quantity
                                calculatedFees = taxlot.TaxLotQty * commissionRuleToApply.ClearingFee_A;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValueForAccount(allocationGroup, taxlot);
                                calculatedFees = notional * commissionRuleToApply.ClearingFee_A * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedFees = commissionRuleToApply.ClearingFee_A;
                                break;
                            default:
                                throw new Exception("Clearing Fee rule basis not set. It should depend either on Shares,Contracts or Notional values.");
                        }
                        taxlot.ClearingFee = calculatedFees;
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.TaxOnCommissions:

                        switch (commissionRuleToApply.TaxonCommissionsCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts: ///qty in this case is contract quantity
                                calculatedFees = taxlot.TaxLotQty * commissionRuleToApply.TaxonCommissions;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValueForAccount(allocationGroup, taxlot);
                                calculatedFees = notional * commissionRuleToApply.TaxonCommissions * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedFees = commissionRuleToApply.TaxonCommissions;
                                break;
                            default:
                                throw new Exception("TaxonCommission rule basis not set. It should depend either on Shares,Contracts or Notional values.");
                        }
                        taxlot.TaxOnCommissions = calculatedFees;
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.TransactionLevy:

                        switch (commissionRuleToApply.TransactionLevyCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts: ///qty in this case is contract quantity
                                calculatedFees = taxlot.TaxLotQty * commissionRuleToApply.TransactionLevy;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValueForAccount(allocationGroup, taxlot);
                                calculatedFees = notional * commissionRuleToApply.TransactionLevy * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedFees = commissionRuleToApply.TransactionLevy;
                                break;
                            default:
                                throw new Exception("Transaction Levy rule basis not set. It should depend either on Shares,Contracts or Notional values.");
                        }
                        taxlot.TransactionLevy = calculatedFees;
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.MiscFees:

                        switch (commissionRuleToApply.MiscFeesCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts: ///qty in this case is contract quantity
                                calculatedFees = taxlot.TaxLotQty * commissionRuleToApply.MiscFees;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValueForAccount(allocationGroup, taxlot);
                                calculatedFees = notional * commissionRuleToApply.MiscFees * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedFees = commissionRuleToApply.MiscFees;
                                break;
                            default:
                                throw new Exception("Misc Fee rule basis not set. It should depend either on Shares,Contracts or Notional values.");
                        }
                        taxlot.MiscFees = calculatedFees;
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.SecFee:

                        switch (commissionRuleToApply.SecFeeCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts: ///qty in this case is contract quantity
                                calculatedFees = taxlot.TaxLotQty * commissionRuleToApply.SecFee;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValueForAccount(allocationGroup, taxlot);
                                calculatedFees = notional * commissionRuleToApply.SecFee * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedFees = commissionRuleToApply.SecFee;
                                break;
                            default:
                                throw new Exception("SEC Fee rule basis not set. It should depend either on Shares,Contracts or Notional values.");
                        }
                        taxlot.SecFee = calculatedFees;
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.OccFee:

                        switch (commissionRuleToApply.OccFeeCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts: ///qty in this case is contract quantity
                                calculatedFees = taxlot.TaxLotQty * commissionRuleToApply.OccFee;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValueForAccount(allocationGroup, taxlot);
                                calculatedFees = notional * commissionRuleToApply.OccFee * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedFees = commissionRuleToApply.OccFee;
                                break;
                            default:
                                throw new Exception("OCC Fee rule basis not set. It should depend either on Shares,Contracts or Notional values.");
                        }
                        taxlot.OccFee = calculatedFees;
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.OrfFee:

                        switch (commissionRuleToApply.OrfFeeCalculationBasedOn)
                        {
                            case CalculationBasis.Shares:
                            case CalculationBasis.Contracts: ///qty in this case is contract quantity
                                calculatedFees = taxlot.TaxLotQty * commissionRuleToApply.OrfFee;
                                break;
                            case CalculationBasis.Notional:
                                notional = GetNotionalValueForAccount(allocationGroup, taxlot);
                                calculatedFees = notional * commissionRuleToApply.OrfFee * 0.0001;
                                break;
                            case CalculationBasis.FlatAmount:
                                calculatedFees = commissionRuleToApply.OrfFee;
                                break;
                            default:
                                throw new Exception("ORF Fee rule basis not set. It should depend either on Shares,Contracts or Notional values.");
                        }
                        taxlot.OrfFee = calculatedFees;
                        break;

                    default:
                        throw new Exception("Other Fees rule basis not set. It should depend either on Shares,Contracts or Notional values.");
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return taxlot;
        }

        #endregion Commission and Fees Calculation For Grouped Orders



        /// <summary>
        /// Calculations for basic order information.
        /// </summary>
        /// <param name="basicOrderInfo">The basic order information.</param>
        public void CalculationForBasicOrderInfo(BasicOrderInfo basicOrderInfo)
        {
            try
            {
                List<OtherFeeRule> lstOtherFeeRule;
                string commText = string.Empty;
                CommissionRule commRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUEC(basicOrderInfo.CounterPartyID, basicOrderInfo.VenueID, basicOrderInfo.AUECID, string.Empty, ref commText);
                if (commRule == null)
                    commRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUECAccountId(basicOrderInfo.CounterPartyID, basicOrderInfo.VenueID, basicOrderInfo.AUECID, string.Empty, basicOrderInfo.Level1ID, ref commText);

                if (commRule != null)
                    CalculateCommissionForBasicOrderInfo(commRule, basicOrderInfo);

                lstOtherFeeRule = CommissionRulesCacheManager.GetInstance().GetOtherFeeRuleAuecDict(basicOrderInfo.AUECID);
                if (lstOtherFeeRule.Count > 0)
                {
                    OtherFeeRule secFeeRule = lstOtherFeeRule.Find(otherFee => otherFee.OtherFeeType == OtherFeeType.SecFee);
                    if (secFeeRule != null)
                    {
                        CalculateFeeForBasicOrderInfo(secFeeRule, basicOrderInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Calculates the commision and sec fee.
        /// </summary>
        /// <param name="otherFeeRule">The other fee rule.</param>
        /// <param name="basicOrderInfo">The basic order information.</param>
        /// <exception cref="System.Exception">this is not a valid Fee type</exception>
        public void CalculateFeeForBasicOrderInfo(OtherFeeRule otherFeeRule, BasicOrderInfo basicOrderInfo)
        {
            try
            {
                double calculatedOtherFees = CalculateFeesValueForBasicOrderInfo(otherFeeRule, basicOrderInfo);

                //Shubham Awasthi (2015-09-08)
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-10682
                if (otherFeeRule.FeePrecisionType == FeePrecisionType.RoundUp)
                {
                    calculatedOtherFees = (double)Math.Ceiling(Convert.ToDecimal(calculatedOtherFees * Math.Pow(10, otherFeeRule.RoundUpPrecision))) / Math.Pow(10, otherFeeRule.RoundUpPrecision);
                }
                else if (otherFeeRule.FeePrecisionType == FeePrecisionType.RoundDown)
                {
                    calculatedOtherFees = (double)Math.Floor(Convert.ToDecimal(calculatedOtherFees * Math.Pow(10, otherFeeRule.RoundDownPrecision))) / Math.Pow(10, otherFeeRule.RoundDownPrecision);
                }
                else
                {
                    calculatedOtherFees = System.Math.Round(calculatedOtherFees, otherFeeRule.RoundOffPrecision, MidpointRounding.AwayFromZero);
                }
                if (calculatedOtherFees > otherFeeRule.MaxValue && otherFeeRule.MaxValue != 0.0)
                {
                    calculatedOtherFees = otherFeeRule.MaxValue;
                }
                else if (calculatedOtherFees < otherFeeRule.MinValue && otherFeeRule.MinValue != 0.0)
                {
                    calculatedOtherFees = otherFeeRule.MinValue;
                }
                switch (otherFeeRule.OtherFeeType)
                {
                    case OtherFeeType.StampDuty:
                        basicOrderInfo.StampDuty = calculatedOtherFees;
                        break;
                    case OtherFeeType.TransactionLevy:
                        basicOrderInfo.TransactionLevy = calculatedOtherFees;
                        break;
                    case OtherFeeType.ClearingFee:
                        basicOrderInfo.ClearingFee = calculatedOtherFees;
                        break;
                    case OtherFeeType.TaxOnCommissions:
                        basicOrderInfo.TaxOnCommissions = calculatedOtherFees;
                        break;
                    case OtherFeeType.MiscFees:
                        basicOrderInfo.MiscFees = calculatedOtherFees;
                        break;
                    case OtherFeeType.SecFee:
                        basicOrderInfo.SecFee = calculatedOtherFees;
                        break;
                    case OtherFeeType.OccFee:
                        basicOrderInfo.OccFee = calculatedOtherFees;
                        break;
                    case OtherFeeType.OrfFee:
                        basicOrderInfo.OrfFee = calculatedOtherFees;
                        break;
                    default:
                        throw new Exception("this is not a valid Fee type");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Calculates the commission for basic order information.
        /// </summary>
        /// <param name="commissionRuleToApply">The commission rule to apply.</param>
        /// <param name="basicOrderInfo">The basic order information.</param>
        private void CalculateCommissionForBasicOrderInfo(CommissionRule commissionRuleToApply, BasicOrderInfo basicOrderInfo)
        {
            try
            {
                double finalCalculatedCommission = double.MinValue;
                double finalCalculatedSoftCommission = double.MinValue;
                CalculateCommissionValue(commissionRuleToApply, basicOrderInfo.Quantity, basicOrderInfo.AvgPrice, 1, ref finalCalculatedCommission, ref finalCalculatedSoftCommission);
                if (finalCalculatedCommission > double.MinValue)
                    basicOrderInfo.Commission = finalCalculatedCommission;
                if (finalCalculatedSoftCommission > double.MinValue)
                    basicOrderInfo.SoftCommission = finalCalculatedSoftCommission;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Calculates the fees value for basic order information.
        /// </summary>
        /// <param name="otherFeeRule">The other fee rule.</param>
        /// <param name="basicOrderInfo">The basic order information.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        private double CalculateFeesValueForBasicOrderInfo(OtherFeeRule otherFeeRule, BasicOrderInfo basicOrderInfo)
        {
            double calculatedValue = 0.0;
            try
            {
                bool isLong = false;
                switch (basicOrderInfo.OrderSideTagValue)
                {
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Buy_Closed:
                    case FIXConstants.SIDE_Buy_Cover:
                    case FIXConstants.SIDE_Buy_Open:
                        isLong = true;
                        break;
                    default:
                        isLong = false;
                        break;
                }
                CalculationFeeBasis calculationBasis;
                if (otherFeeRule.IsCriteriaApplied)
                {
                    calculationBasis = isLong ? (CalculationFeeBasis)otherFeeRule.LongFeeRuleCriteriaList[0].LongCalculationBasis : (CalculationFeeBasis)otherFeeRule.ShortFeeRuleCriteriaList[0].ShortCalculationBasis;
                }
                else
                    calculationBasis = isLong ? otherFeeRule.LongCalculationBasis : otherFeeRule.ShortCalculationBasis;

                double rate = GetOtherFeeRate(otherFeeRule, basicOrderInfo.Quantity, isLong);

                switch (calculationBasis)
                {
                    case CalculationFeeBasis.Shares:
                    case CalculationFeeBasis.Contracts:
                    case CalculationFeeBasis.AvgPrice:
                        calculatedValue = basicOrderInfo.Quantity * rate;
                        break;
                    case CalculationFeeBasis.Notional:
                        double notional = GetNotionalValue(basicOrderInfo.Quantity, basicOrderInfo.AvgPrice, basicOrderInfo.ContractMultiplier);
                        calculatedValue = notional * rate * 0.0001;
                        break;

                    case CalculationFeeBasis.FlatAmount:
                        calculatedValue = rate;
                        break;
                    case CalculationFeeBasis.Commission:
                        calculatedValue = basicOrderInfo.Commission * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.NotionalPlusCommission:
                        calculatedValue = (GetNotionalValue(basicOrderInfo.Quantity, basicOrderInfo.AvgPrice, basicOrderInfo.ContractMultiplier) + basicOrderInfo.Commission) * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.SoftCommission:
                        calculatedValue = basicOrderInfo.SoftCommission * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.StampDuty:
                        calculatedValue = basicOrderInfo.StampDuty * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.TransactionLevy:
                        calculatedValue = basicOrderInfo.TransactionLevy * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.ClearingFee:
                        calculatedValue = basicOrderInfo.ClearingFee * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.TaxOnCommissions:
                        calculatedValue = basicOrderInfo.TaxOnCommissions * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.MiscFees:
                        calculatedValue = basicOrderInfo.MiscFees * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.SecFee:
                        calculatedValue = basicOrderInfo.SecFee * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.OccFee:
                        calculatedValue = basicOrderInfo.OccFee * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.OrfFee:
                        calculatedValue = basicOrderInfo.OrfFee * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.ClearingBrokerFee:
                        calculatedValue = basicOrderInfo.ClearingBrokerFee * rate * 0.0001;
                        break;
                    default:
                        throw new Exception(otherFeeRule.OtherFeeType.ToString() + " rule basis not set.");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return calculatedValue;
        }

        /// <summary>
        /// Gets the other fee rate.
        /// </summary>
        /// <param name="otherFeeRule">The other fee rule.</param>
        /// <param name="valueToCheck">The value to check.</param>
        /// <param name="isLong">if set to <c>true</c> [is long].</param>
        /// <returns></returns>
        private double GetOtherFeeRate(OtherFeeRule otherFeeRule, double valueToCheck, bool isLong)
        {
            try
            {
                if (isLong)
                {
                    if (otherFeeRule.IsCriteriaApplied)
                    {
                        List<OtherFeesCriteria> otherFeesCriteria = otherFeeRule.LongFeeRuleCriteriaList;
                        foreach (OtherFeesCriteria commissionRuleCriteria in otherFeesCriteria)
                        {
                            double maxValue = commissionRuleCriteria.LongValueLessThanOrEqual == 0 ? double.MaxValue : commissionRuleCriteria.LongValueLessThanOrEqual;
                            double minValue = commissionRuleCriteria.LongValueGreaterThan;
                            if (valueToCheck <= maxValue && valueToCheck > minValue)
                                return commissionRuleCriteria.LongFeeRate;
                        }
                    }
                    return otherFeeRule.LongRate;
                }
                else
                {
                    if (otherFeeRule.IsCriteriaApplied)
                    {
                        List<OtherFeesCriteria> otherFeesCriteria = otherFeeRule.ShortFeeRuleCriteriaList;
                        foreach (OtherFeesCriteria commissionRuleCriteria in otherFeesCriteria)
                        {
                            double maxValue = commissionRuleCriteria.ShortValueLessThanOrEqual == 0 ? double.MaxValue : commissionRuleCriteria.ShortValueLessThanOrEqual;
                            double minValue = commissionRuleCriteria.ShortValueGreaterThan;
                            if (valueToCheck <= maxValue && valueToCheck > minValue)
                                return commissionRuleCriteria.ShortFeeRate;
                        }
                    }
                    return otherFeeRule.ShortRate;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 0.0;
        }
    }
}
