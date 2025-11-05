// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-09-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-09-2014
// ***********************************************************************
// <copyright file="CommissionCalculator.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The FormulaStore namespace.
/// </summary>
namespace Prana.Allocation.Core.FormulaStore
{
    /// <summary>
    /// Class responsible for calculating the commissions based on the rules and order cache
    /// </summary>
    internal class CommissionCalculator
    {
        /// <summary>
        /// The _commission rules cache manager
        /// </summary>
        static CommissionRulesCacheManager _commissionRulesCacheManager = null;
        /// <summary>
        /// The _commission calculator
        /// </summary>
        static CommissionCalculator _commissionCalculator = null;

        #region Constructor region
        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>The get instance.</value>
        public static CommissionCalculator GetInstance
        {
            get { return _commissionCalculator; }
        }

        /// <summary>
        /// Initializes static members of the <see cref="CommissionCalculator"/> class.
        /// </summary>
        static CommissionCalculator()
        {
            _commissionCalculator = new CommissionCalculator();
            _commissionRulesCacheManager = CommissionRulesCacheManager.GetInstance();
        }
        #endregion Constructor region

        #region Commission and Fees Calculation For Grouped Orders

        /// <summary>
        /// Starts the calculation.
        /// </summary>
        /// <param name="allocatedGroup">The allocated group.</param>
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
                    List<OtherFeeRule> newOtherFeeRule = new List<OtherFeeRule>();
                    CommissionRule commissionRule = new CommissionRule(true, CalculationBasis.FlatAmount, 0);
                    lstOtherFeeRule = CommissionRulesCacheManager.GetInstance().GetOtherFeeRuleAuecDict(allocatedGroup.AUECID);
                    if (lstOtherFeeRule.Count > 0)
                    {
                        bool isLong = false;
                        switch (allocatedGroup.OrderSideTagValue)
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
                        newOtherFeeRule = CalculationOrderForOthersFee(lstOtherFeeRule, isLong);
                        lstOtherFeeRule = newOtherFeeRule;
                    }
                    //variables for storing current commission, PRANA-12889 
                    double softCommission = allocatedGroup.SoftCommission;
                    double commission = allocatedGroup.Commission;

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
                            allocatedGroup.Commission = totalCommission;
                            allocatedGroup.SoftCommission = totalSoftCommission;
                            allocatedGroup.OtherBrokerFees = totalOtherBrokerFee;
                            allocatedGroup.ClearingBrokerFee = totalClearingBrokerFee;
                        }
                    }
                    else
                    {
                        string commissionText = "calculated";
                        if (allocatedGroup.IsCommissionCalculated == false)// means commission and Fee not calculated 
                        {
                            if (!isCommissionAndFeeZero)
                                commissionRule = _commissionRulesCacheManager.GetCommissionRuleByCVAUEC(allocatedGroup.CounterPartyID, allocatedGroup.VenueID, allocatedGroup.AUECID, allocatedGroup.ListID, ref commissionText);
                            if (commissionRule != null)
                            {
                                CalculateCommissionGroupwise(commissionRule, allocatedGroup);
                                CalculateFeesGroupwise(commissionRule, allocatedGroup);
                            }
                            else
                            {
                                allocatedGroup.CommissionText = commissionText;
                                allocatedGroup.DistributeCommisionInTaxLot(true, true);
                                allocatedGroup.DistributeFeesInTaxLot();
                            }
                        }
                    }

                    // Calculate otherFees like StampDuty, ClearingFees etc, PRANA-12889
                    if (allocatedGroup.CommSource != CommisionSource.Auto)
                    {
                        allocatedGroup.Commission = commission;
                    }
                    if (allocatedGroup.SoftCommSource != CommisionSource.Auto)
                    {
                        allocatedGroup.SoftCommission = softCommission;
                    }
                    allocatedGroup.DistributeCommisionInTaxLot(allocatedGroup.CommissionSource == (int)CommisionSource.Manual, allocatedGroup.SoftCommissionSource == (int)CommisionSource.Manual);
                    CalculateOtherFees(lstOtherFeeRule, allocatedGroup, isCommissionAndFeeZero);
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

        private List<OtherFeeRule> CalculationOrderForOthersFee(List<OtherFeeRule> lstOtherFeeRule, bool isLong)
        {
            List<OtherFeeRule> newOrder = new List<OtherFeeRule>();
            int key = 0;
            int feeCount = Enum.GetNames(typeof(OtherFeeType)).Length;
            int[] degree = new int[feeCount];
            List<int>[] adjList = new List<int>[feeCount];
            for (int i = 0; i < feeCount; i++)
                adjList[i] = new List<int>();
            for (int i = 0; i < lstOtherFeeRule.Count; i++)
            {
                if (lstOtherFeeRule[i].IsCriteriaApplied)
                    continue;
                if (isLong)
                    key = (int)lstOtherFeeRule[i].LongCalculationBasis - (int)CalculationFeeBasis.StampDuty;
                else
                    key = (int)lstOtherFeeRule[i].ShortCalculationBasis - (int)CalculationFeeBasis.StampDuty;
                if (key >= 0)
                {
                    degree[(int)lstOtherFeeRule[i].OtherFeeType]++;
                    adjList[key].Add((int)lstOtherFeeRule[i].OtherFeeType);
                }
            }
            Queue<int> q = new Queue<int>();
            for (int i = 0; i < feeCount; i++)
            {
                if (degree[i] == 0)
                {
                    q.Enqueue(i);
                }
            }
            while (q.Count > 0)
            {
                int feeid = q.Dequeue();
                var otherfee = lstOtherFeeRule.Find(x => x.OtherFeeType == (OtherFeeType)feeid);
                if (otherfee != null)
                    newOrder.Add(otherfee);
                for (int i = 0; i < adjList[feeid].Count; i++)
                {
                    if (--degree[adjList[feeid][i]] == 0)
                        q.Enqueue(adjList[feeid][i]);
                }
                feeCount--;
            }
            return newOrder;
        }

        /// <summary>
        /// Calculates the other fees.
        /// </summary>
        /// <param name="otherFeeRuleList">The other fee rule list.</param>
        /// <param name="group">The group.</param>
        /// <returns>AllocationGroup.</returns>
        /// <exception cref="System.Exception">
        /// Commission rule basis not set. It should depend either of Shares,Contracts , Notional values,Commissionor NotionalPlusCommission.
        /// or
        /// Commission rule basis not set. It should depend either of Shares,Contracts , Notional values,Commissionor NotionalPlusCommission.
        /// or
        /// this is not a valid Fee type
        /// </exception>
        public void CalculateOtherFees(List<OtherFeeRule> otherFeeRuleList, AllocationGroup group, bool isCommissionAndFeeZero)
        {
            try
            {
                if (isCommissionAndFeeZero)
                {
                    group.StampDuty = 0;
                    group.TransactionLevy = 0;
                    group.ClearingFee = 0;
                    group.TaxOnCommissions = 0;
                    group.MiscFees = 0;
                    group.SecFee = 0;
                    group.OccFee = 0;
                    group.OrfFee = 0;
                    group.OptionPremiumAdjustment = 0;
                    DistributeOtherFeesInTaxLot(group);
                }
                else if (otherFeeRuleList.Count > 0)
                {
                    foreach (OtherFeeRule otherFeeRule in otherFeeRuleList)
                    {
                        double calculatedOtherFees = CalculateFeesValueForGroup(otherFeeRule, group);

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
        }
        #endregion Commission and Fees Calculation For Grouped Orders

        #region Commission Bulk Change
        /// <summary>
        /// Apply bulk change Commissions also update Audit trail
        /// </summary>
        /// <param name="commissionRule"></param>
        /// <param name="groupList"></param>
        /// <param name="isGroupWise"></param>
        /// <returns></returns>
        internal List<AllocationGroup> ApplyCommissionBulkChange(CommissionRule commissionRule, List<AllocationGroup> groupList, bool isGroupWise)
        {
            List<AllocationGroup> updatedGroupList = new List<AllocationGroup>();
            try
            {
                List<AllocationGroup> defaultRuleGroupList = new List<AllocationGroup>();
                if (commissionRule == null)
                {
                    defaultRuleGroupList = groupList;
                }
                else if (!string.IsNullOrEmpty(commissionRule.RuleName) && Convert.ToBoolean(Convert.ToInt32(CachedDataManager.GetInstance.GetPranaPreferenceByKey(ApplicationConstants.CONST_ZEROCOMMISSIONFORSWAPS))))
                {
                    defaultRuleGroupList.AddRange(groupList.Where(group => group.IsSwapped).ToList());
                }
                foreach (AllocationGroup allocatedGroup in defaultRuleGroupList)
                {
                    allocatedGroup.CommSource = CommisionSource.Auto;
                    allocatedGroup.SoftCommSource = CommisionSource.Auto;
                    StartCalculation(allocatedGroup);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.Commission_Changed);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.SoftCommission_Changed);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.StampDuty_Changed);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.ClearingFee_Changed);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.TaxOnCommission_Changed);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.TransactionLevy_Changed);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.MiscFees_Changed);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.SecFee_Changed);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.OccFee_Changed);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.OrfFee_Changed);
                    allocatedGroup.UpdateTaxlotState();
                    updatedGroupList.Add(allocatedGroup);
                }

                if (commissionRule != null && defaultRuleGroupList.Count < groupList.Count)//Apply specific Rule
                {
                    groupList = groupList.Except(defaultRuleGroupList).ToList();
                    Dictionary<OtherFeeType, TradeAuditActionType.ActionType> feesChanged = new Dictionary<OtherFeeType, TradeAuditActionType.ActionType>();
                    if (commissionRule.IsStampDutyApplied)
                        feesChanged.Add(OtherFeeType.StampDuty, TradeAuditActionType.ActionType.StampDuty_Changed);
                    if (commissionRule.IsClearingFee_AApplied)
                        feesChanged.Add(OtherFeeType.ClearingFee, TradeAuditActionType.ActionType.ClearingFee_Changed);
                    if (commissionRule.IsTaxonCommissionsApplied)
                        feesChanged.Add(OtherFeeType.TaxOnCommissions, TradeAuditActionType.ActionType.TaxOnCommission_Changed);
                    if (commissionRule.IsTransactionLevyApplied)
                        feesChanged.Add(OtherFeeType.TransactionLevy, TradeAuditActionType.ActionType.TransactionLevy_Changed);
                    if (commissionRule.IsMiscFeesApplied)
                        feesChanged.Add(OtherFeeType.MiscFees, TradeAuditActionType.ActionType.MiscFees_Changed);
                    if (commissionRule.IsSecFeeApplied)
                        feesChanged.Add(OtherFeeType.SecFee, TradeAuditActionType.ActionType.SecFee_Changed);
                    if (commissionRule.IsOccFeeApplied)
                        feesChanged.Add(OtherFeeType.OccFee, TradeAuditActionType.ActionType.OccFee_Changed);
                    if (commissionRule.IsOrfFeeApplied)
                        feesChanged.Add(OtherFeeType.OrfFee, TradeAuditActionType.ActionType.OrfFee_Changed);

                    if (isGroupWise)
                    {
                        foreach (AllocationGroup group in groupList)
                        {
                            AllocationGroup allocatedGroup = group;
                            CalculateBulkGroupCommissionAndFees(commissionRule, ref allocatedGroup, feesChanged);
                            allocatedGroup.UpdateTaxlotState();
                            updatedGroupList.Add(allocatedGroup);
                        }
                    }
                    else
                    {
                        foreach (AllocationGroup group in groupList)
                        {
                            AllocationGroup allocatedGroup = group;
                            bool isChanged = false;
                            foreach (TaxLot oldTaxlot in allocatedGroup.TaxLots)
                            {
                                TaxLot taxlot = oldTaxlot;
                                if (commissionRule.AccountIDs != null && commissionRule.AccountIDs.Count > 0)
                                {
                                    if (commissionRule.AccountIDs.Contains(taxlot.Level1ID))
                                    {
                                        CalculateBulkTaxlotCommissionAndFees(commissionRule, ref allocatedGroup, ref taxlot);
                                        CalculateBulkOtherFeesAccountWise(commissionRule, ref taxlot, ref allocatedGroup, feesChanged);
                                        isChanged = true;
                                    }
                                }
                                else
                                {
                                    CalculateBulkTaxlotCommissionAndFees(commissionRule, ref allocatedGroup, ref taxlot);
                                    CalculateBulkOtherFeesAccountWise(commissionRule, ref taxlot, ref allocatedGroup, feesChanged);
                                    isChanged = true;
                                }
                            }
                            if (isChanged)
                            {
                                allocatedGroup.CommSource = CommisionSource.Manual;
                                allocatedGroup.SoftCommSource = CommisionSource.Manual;
                            }
                            allocatedGroup.UpdateTaxlotState();
                            updatedGroupList.Add(allocatedGroup);
                        }
                    }
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
            return updatedGroupList;
        }

        /// <summary>
        /// Calculate commission and fees Group wise
        /// </summary>
        /// <param name="commissionRule"></param>
        /// <param name="allocatedGroup"></param>
        /// <param name="feesChanged"></param>
        private void CalculateBulkGroupCommissionAndFees(CommissionRule commissionRule, ref AllocationGroup allocatedGroup, Dictionary<OtherFeeType, TradeAuditActionType.ActionType> feesChanged)
        {
            try
            {
                if (commissionRule.IsCommissionApplied)
                {
                    allocatedGroup.CommSource = CommisionSource.Auto;
                    allocatedGroup.SoftCommSource = CommisionSource.Manual;
                    CalculateCommissionGroupwise(commissionRule, allocatedGroup);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.Commission_Changed);
                }
                if (commissionRule.IsSoftCommissionApplied)
                {
                    allocatedGroup.CommSource = CommisionSource.Manual;
                    allocatedGroup.SoftCommSource = CommisionSource.Auto;
                    CalculateCommissionGroupwise(commissionRule, allocatedGroup);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.SoftCommission_Changed);
                }
                if (commissionRule.IsClearingFeeApplied)
                {
                    CommissionRule rule = new CommissionRule();
                    rule.ClearingBrokerFeeObj.ClearingFeeRate = double.MinValue;
                    rule.ClearingFeeObj.ClearingFeeRate = commissionRule.ClearingFeeObj.ClearingFeeRate;
                    rule.ClearingFeeObj.MinClearingFee = commissionRule.ClearingFeeObj.MinClearingFee;
                    rule.IsClearingFeeApplied = commissionRule.IsClearingFeeApplied;
                    rule.ClearingFeeObj.RuleAppliedOn = commissionRule.ClearingFeeObj.RuleAppliedOn;
                    rule.ClearingFeeObj.IsCriteriaApplied = commissionRule.ClearingFeeObj.IsCriteriaApplied;
                    rule.ClearingFeeObj.ClearingFeeRuleCriteiaList = commissionRule.ClearingFeeObj.ClearingFeeRuleCriteiaList;
                    CalculateFeesGroupwise(rule, allocatedGroup);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                }
                if (commissionRule.IsClearingBrokerFeeApplied)
                {
                    CommissionRule rule = new CommissionRule();
                    rule.ClearingFeeObj.ClearingFeeRate = double.MinValue;
                    rule.ClearingBrokerFeeObj.ClearingFeeRate = commissionRule.ClearingBrokerFeeObj.ClearingFeeRate;
                    rule.ClearingBrokerFeeObj.MinClearingFee = commissionRule.ClearingBrokerFeeObj.MinClearingFee;
                    rule.IsClearingBrokerFeeApplied = commissionRule.IsClearingBrokerFeeApplied;
                    rule.ClearingBrokerFeeObj.RuleAppliedOn = commissionRule.ClearingBrokerFeeObj.RuleAppliedOn;
                    rule.ClearingBrokerFeeObj.IsCriteriaApplied = commissionRule.ClearingBrokerFeeObj.IsCriteriaApplied;
                    rule.ClearingBrokerFeeObj.ClearingFeeRuleCriteiaList = commissionRule.ClearingBrokerFeeObj.ClearingFeeRuleCriteiaList;
                    CalculateFeesGroupwise(rule, allocatedGroup);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                }
                if (feesChanged.Count > 0)
                {
                    foreach (OtherFeeType feeType in feesChanged.Keys)
                    {
                        CalculateOtherFeeGroupwise(commissionRule, ref allocatedGroup, feeType);
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, feesChanged[feeType]);
                    }
                    allocatedGroup.DistributeOtherFeesInTaxLot();
                }
                allocatedGroup.CommSource = CommisionSource.Manual;
                allocatedGroup.SoftCommSource = CommisionSource.Manual;
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
        /// Calculate commission and fees Taxlot wise
        /// </summary>
        /// <param name="commissionRule"></param>
        /// <param name="allocatedGroup"></param>
        /// <param name="taxlot"></param>
        private void CalculateBulkTaxlotCommissionAndFees(CommissionRule commissionRule, ref AllocationGroup allocatedGroup, ref TaxLot taxlot)
        {
            try
            {
                if (commissionRule.IsCommissionApplied)
                {
                    allocatedGroup.Commission -= taxlot.Commission;
                    allocatedGroup.CommSource = CommisionSource.Auto;
                    allocatedGroup.SoftCommSource = CommisionSource.Manual;
                    taxlot.Commission = CalculateCommissionAccountWise(commissionRule, taxlot, allocatedGroup).Commission;
                    allocatedGroup.Commission += taxlot.Commission;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.Commission_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.Commission_Changed);
                }
                if (commissionRule.IsSoftCommissionApplied)
                {
                    allocatedGroup.SoftCommission -= taxlot.SoftCommission;
                    allocatedGroup.CommSource = CommisionSource.Manual;
                    allocatedGroup.SoftCommSource = CommisionSource.Auto;
                    taxlot.SoftCommission = CalculateCommissionAccountWise(commissionRule, taxlot, allocatedGroup).SoftCommission;
                    allocatedGroup.SoftCommission += taxlot.SoftCommission;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.SoftCommission_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.SoftCommission_Changed);
                }
                if (commissionRule.IsClearingFeeApplied)
                {
                    CommissionRule rule = new CommissionRule();
                    rule.IsClearingFeeApplied = true;
                    rule.IsClearingBrokerFeeApplied = false;
                    rule.ClearingFeeObj.ClearingFeeRate = commissionRule.ClearingFeeObj.ClearingFeeRate;
                    rule.ClearingFeeObj.MinClearingFee = commissionRule.ClearingFeeObj.MinClearingFee;
                    rule.ClearingFeeObj.RuleAppliedOn = commissionRule.ClearingFeeObj.RuleAppliedOn;
                    rule.ClearingFeeObj.IsCriteriaApplied = commissionRule.ClearingFeeObj.IsCriteriaApplied;
                    rule.ClearingFeeObj.ClearingFeeRuleCriteiaList = commissionRule.ClearingFeeObj.ClearingFeeRuleCriteiaList;
                    allocatedGroup.OtherBrokerFees -= taxlot.OtherBrokerFees;
                    taxlot.OtherBrokerFees = CalculateFeesAccountWise(rule, taxlot, allocatedGroup).OtherBrokerFees;
                    allocatedGroup.OtherBrokerFees += taxlot.OtherBrokerFees;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                }
                if (commissionRule.IsClearingBrokerFeeApplied)
                {
                    CommissionRule rule = new CommissionRule();
                    rule.IsClearingFeeApplied = false;
                    rule.IsClearingBrokerFeeApplied = true;
                    rule.ClearingBrokerFeeObj.ClearingFeeRate = commissionRule.ClearingBrokerFeeObj.ClearingFeeRate;
                    rule.ClearingBrokerFeeObj.MinClearingFee = commissionRule.ClearingBrokerFeeObj.MinClearingFee;
                    rule.ClearingBrokerFeeObj.RuleAppliedOn = commissionRule.ClearingBrokerFeeObj.RuleAppliedOn;
                    rule.ClearingBrokerFeeObj.IsCriteriaApplied = commissionRule.ClearingBrokerFeeObj.IsCriteriaApplied;
                    rule.ClearingBrokerFeeObj.ClearingFeeRuleCriteiaList = commissionRule.ClearingBrokerFeeObj.ClearingFeeRuleCriteiaList;
                    allocatedGroup.ClearingBrokerFee -= taxlot.ClearingBrokerFee;
                    taxlot.ClearingBrokerFee = CalculateFeesAccountWise(rule, taxlot, allocatedGroup).ClearingBrokerFee;
                    allocatedGroup.ClearingBrokerFee += taxlot.ClearingBrokerFee;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
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
        }

        /// <summary>
        /// Calculate other fees taxlot wise
        /// </summary>
        /// <param name="commissionRule"></param>
        /// <param name="taxlot"></param>
        /// <param name="allocatedGroup"></param>
        /// <param name="feesChanged"></param>
        private void CalculateBulkOtherFeesAccountWise(CommissionRule commissionRule, ref TaxLot taxlot, ref AllocationGroup allocatedGroup, Dictionary<OtherFeeType, TradeAuditActionType.ActionType> feesChanged)
        {
            try
            {
                foreach (OtherFeeType feeType in feesChanged.Keys)
                {
                    switch (feeType)
                    {
                        case OtherFeeType.StampDuty:
                            allocatedGroup.StampDuty -= taxlot.StampDuty;
                            taxlot.StampDuty = CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, feeType).StampDuty;
                            allocatedGroup.StampDuty += taxlot.StampDuty;
                            break;

                        case OtherFeeType.ClearingFee:
                            allocatedGroup.ClearingFee -= taxlot.ClearingFee;
                            taxlot.ClearingFee = CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, feeType).ClearingFee;
                            allocatedGroup.ClearingFee += taxlot.ClearingFee;
                            break;

                        case OtherFeeType.TaxOnCommissions:
                            allocatedGroup.TaxOnCommissions -= taxlot.TaxOnCommissions;
                            taxlot.TaxOnCommissions = CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, feeType).TaxOnCommissions;
                            allocatedGroup.TaxOnCommissions += taxlot.TaxOnCommissions;
                            break;

                        case OtherFeeType.TransactionLevy:
                            allocatedGroup.TransactionLevy -= taxlot.TransactionLevy;
                            taxlot.TransactionLevy = CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, feeType).TransactionLevy;
                            allocatedGroup.TransactionLevy += taxlot.TransactionLevy;
                            break;

                        case OtherFeeType.MiscFees:
                            allocatedGroup.MiscFees -= taxlot.MiscFees;
                            taxlot.MiscFees = CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, feeType).MiscFees;
                            allocatedGroup.MiscFees += taxlot.MiscFees;
                            break;

                        case OtherFeeType.SecFee:
                            allocatedGroup.SecFee -= taxlot.SecFee;
                            taxlot.SecFee = CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, feeType).SecFee;
                            allocatedGroup.SecFee += taxlot.SecFee;
                            break;

                        case OtherFeeType.OccFee:
                            allocatedGroup.OccFee -= taxlot.OccFee;
                            taxlot.OccFee = CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, feeType).OccFee;
                            allocatedGroup.OccFee += taxlot.OccFee;
                            break;

                        case OtherFeeType.OrfFee:
                            allocatedGroup.OrfFee -= taxlot.OrfFee;
                            taxlot.OrfFee = CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, feeType).OrfFee;
                            allocatedGroup.OrfFee += taxlot.OrfFee;
                            break;
                    }
                    taxlot.AddTradeAction(feesChanged[feeType]);
                    allocatedGroup.AddTradeAction(feesChanged[feeType]);
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
        }
        #endregion

        #region Commission on group

        /// <summary>
        /// Calculates the other fee groupwise.
        /// </summary>
        /// <param name="commissionRuleToApply">The commission rule to apply.</param>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <param name="FeeType">Type of the fee.</param>
        /// <exception cref="System.Exception">
        /// Stamp Duty rule basis not set.
        /// or
        /// Clearing Fee rule basis not set.
        /// or
        /// Tax on Commission rule basis not set.
        /// or
        /// Transaction Levy rule basis not set.
        /// or
        /// Misc Fees rule basis not set.
        /// or
        /// SEC Fee rule basis not set.
        /// or
        /// OCC Fee rule basis not set.
        /// or
        /// ORF Fee rule basis not set.
        /// </exception>
        public void CalculateOtherFeeGroupwise(CommissionRule commissionRuleToApply, ref AllocationGroup allocationGroup, OtherFeeType FeeType)
        {
            try
            {
                switch (FeeType)
                {
                    case Prana.BusinessObjects.AppConstants.OtherFeeType.StampDuty:
                        allocationGroup.StampDuty = CalculateFeesValueForGroup(commissionRuleToApply.StampDutyCalculationBasedOn, allocationGroup, commissionRuleToApply.StampDuty, OtherFeeType.StampDuty.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.ClearingFee:
                        allocationGroup.ClearingFee = CalculateFeesValueForGroup(commissionRuleToApply.ClearingFeeCalculationBasedOn_A, allocationGroup, commissionRuleToApply.ClearingFee_A, OtherFeeType.ClearingFee.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.TaxOnCommissions:
                        allocationGroup.TaxOnCommissions = CalculateFeesValueForGroup(commissionRuleToApply.TaxonCommissionsCalculationBasedOn, allocationGroup, commissionRuleToApply.TaxonCommissions, OtherFeeType.TaxOnCommissions.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.TransactionLevy:
                        allocationGroup.TransactionLevy = CalculateFeesValueForGroup(commissionRuleToApply.TransactionLevyCalculationBasedOn, allocationGroup, commissionRuleToApply.TransactionLevy, OtherFeeType.TransactionLevy.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.MiscFees:
                        allocationGroup.MiscFees = CalculateFeesValueForGroup(commissionRuleToApply.MiscFeesCalculationBasedOn, allocationGroup, commissionRuleToApply.MiscFees, OtherFeeType.MiscFees.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.SecFee:
                        allocationGroup.SecFee = CalculateFeesValueForGroup(commissionRuleToApply.SecFeeCalculationBasedOn, allocationGroup, commissionRuleToApply.SecFee, OtherFeeType.SecFee.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.OccFee:
                        allocationGroup.OccFee = CalculateFeesValueForGroup(commissionRuleToApply.OccFeeCalculationBasedOn, allocationGroup, commissionRuleToApply.OccFee, OtherFeeType.OccFee.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.OrfFee:
                        allocationGroup.OrfFee = CalculateFeesValueForGroup(commissionRuleToApply.OrfFeeCalculationBasedOn, allocationGroup, commissionRuleToApply.OrfFee, OtherFeeType.OrfFee.ToString());
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
        }

        /// <summary>
        /// Calculates the commission groupwise.
        /// </summary>
        /// <param name="commissionRuleToApply">The commission rule to apply.</param>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <returns>AllocationGroup.</returns>
        /// <exception cref="System.Exception">
        /// Commission rule basis not set.
        /// or
        /// Soft Commission rule basis not set.
        /// </exception>
        public AllocationGroup CalculateCommissionGroupwise(CommissionRule commissionRuleToApply, AllocationGroup allocationGroup)
        {
            try
            {
                // calculated commission only if source is auto or call set to recalculate commission, PRANA-12889
                if ((allocationGroup.CommissionSource == (int)CommisionSource.Auto || allocationGroup.IsRecalculateCommission) && commissionRuleToApply.Commission.CommissionRate != double.MinValue)
                {
                    allocationGroup.Commission = CalculateCommissionValueGroup(commissionRuleToApply.Commission, allocationGroup, "Commission"); ;
                }

                // calculated soft commission only if source is auto or call set to recalculate soft commission, PRANA-12889
                if ((allocationGroup.SoftCommissionSource == (int)CommisionSource.Auto || allocationGroup.IsRecalculateCommission) && commissionRuleToApply.SoftCommission.CommissionRate != double.MinValue)
                {
                    allocationGroup.SoftCommission = CalculateCommissionValueGroup(commissionRuleToApply.SoftCommission, allocationGroup, "Soft Commission");
                }
                DistributeCommisionInTaxLot(allocationGroup);
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
            return allocationGroup;
        }

        /// <summary>
        /// Calculates the fees groupwise.
        /// </summary>
        /// <param name="commissionRuleToApply">The commission rule to apply.</param>
        /// <param name="allocatedGroup">The allocated group.</param>
        /// <returns>AllocationGroup.</returns>
        /// <exception cref="System.Exception">
        /// Commission rule basis not set. It should depend either of Shares,Contracts or Notional values.
        /// or
        /// Commission rule basis not set. It should depend either of Shares,Contracts or Notional values.
        /// </exception>
        public AllocationGroup CalculateFeesGroupwise(CommissionRule commissionRuleToApply, AllocationGroup allocatedGroup)
        {
            try
            {
                double calculatedFees = 0;
                double finalCalculatedFees = 0;

                if (commissionRuleToApply.ClearingFeeObj.ClearingFeeRate != double.MinValue)
                {
                    ///Fees Calculation
                    if (commissionRuleToApply.IsClearingFeeApplied)
                    {
                        calculatedFees = CalculateClearingFeeForGroup(commissionRuleToApply.ClearingFeeObj, allocatedGroup, "Other Broker Fee");

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

                if (commissionRuleToApply.ClearingBrokerFeeObj.ClearingFeeRate != double.MinValue)
                {
                    ///Fees Calculation
                    if (commissionRuleToApply.IsClearingBrokerFeeApplied)
                    {
                        calculatedFees = CalculateClearingFeeForGroup(commissionRuleToApply.ClearingBrokerFeeObj, allocatedGroup, "Clearing Broker Fee");

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
        #endregion

        #region Commission on Taxlot

        /// <summary>
        /// Calculates the other fees account wise.
        /// </summary>
        /// <param name="commissionRuleToApply">The commission rule to apply.</param>
        /// <param name="taxlot">The taxlot.</param>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <param name="FeeType">Type of the fee.</param>
        /// <returns>TaxLot.</returns>
        /// <exception cref="System.Exception">
        /// Stamp Duty rule basis not set. It should depend either on Shares,Contracts or Notional values.
        /// or
        /// Clearing Fee rule basis not set. It should depend either on Shares,Contracts or Notional values.
        /// or
        /// TaxonCommission rule basis not set. It should depend either on Shares,Contracts or Notional values.
        /// or
        /// Transaction Levy rule basis not set. It should depend either on Shares,Contracts or Notional values.
        /// or
        /// Misc Fee rule basis not set. It should depend either on Shares,Contracts or Notional values.
        /// or
        /// SEC Fee rule basis not set. It should depend either on Shares,Contracts or Notional values.
        /// or
        /// OCC Fee rule basis not set. It should depend either on Shares,Contracts or Notional values.
        /// or
        /// ORF Fee rule basis not set. It should depend either on Shares,Contracts or Notional values.
        /// or
        /// Other Fees rule basis not set. It should depend either on Shares,Contracts or Notional values.
        /// </exception>
        public TaxLot CalculateOtherFeesAccountWise(CommissionRule commissionRuleToApply, TaxLot taxlot, AllocationGroup allocationGroup, Prana.BusinessObjects.AppConstants.OtherFeeType FeeType)
        {
            try
            {
                switch (FeeType)
                {
                    case Prana.BusinessObjects.AppConstants.OtherFeeType.StampDuty:
                        taxlot.StampDuty = CalculateFeesValueForTaxlot(commissionRuleToApply.StampDutyCalculationBasedOn, allocationGroup, taxlot, commissionRuleToApply.StampDuty, OtherFeeType.StampDuty.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.ClearingFee:
                        taxlot.ClearingFee = CalculateFeesValueForTaxlot(commissionRuleToApply.ClearingFeeCalculationBasedOn_A, allocationGroup, taxlot, commissionRuleToApply.ClearingFee_A, OtherFeeType.ClearingFee.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.TaxOnCommissions:
                        taxlot.TaxOnCommissions = CalculateFeesValueForTaxlot(commissionRuleToApply.TaxonCommissionsCalculationBasedOn, allocationGroup, taxlot, commissionRuleToApply.TaxonCommissions, OtherFeeType.TaxOnCommissions.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.TransactionLevy:
                        taxlot.TransactionLevy = CalculateFeesValueForTaxlot(commissionRuleToApply.TransactionLevyCalculationBasedOn, allocationGroup, taxlot, commissionRuleToApply.TransactionLevy, OtherFeeType.TransactionLevy.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.MiscFees:
                        taxlot.MiscFees = CalculateFeesValueForTaxlot(commissionRuleToApply.MiscFeesCalculationBasedOn, allocationGroup, taxlot, commissionRuleToApply.MiscFees, OtherFeeType.MiscFees.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.SecFee:
                        taxlot.SecFee = CalculateFeesValueForTaxlot(commissionRuleToApply.SecFeeCalculationBasedOn, allocationGroup, taxlot, commissionRuleToApply.SecFee, OtherFeeType.SecFee.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.OccFee:
                        taxlot.OccFee = CalculateFeesValueForTaxlot(commissionRuleToApply.OccFeeCalculationBasedOn, allocationGroup, taxlot, commissionRuleToApply.OccFee, OtherFeeType.OccFee.ToString());
                        break;

                    case Prana.BusinessObjects.AppConstants.OtherFeeType.OrfFee:
                        taxlot.OrfFee = CalculateFeesValueForTaxlot(commissionRuleToApply.OrfFeeCalculationBasedOn, allocationGroup, taxlot, commissionRuleToApply.OrfFee, OtherFeeType.OrfFee.ToString());
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

        /// <summary>
        /// Calculates the commission account wise.
        /// </summary>
        /// <param name="commissionRuleToApply">The commission rule to apply.</param>
        /// <param name="taxlot">The taxlot.</param>
        /// <param name="allocatedGroup">The allocated group.</param>
        /// <returns>TaxLot.</returns>
        /// <exception cref="System.Exception">
        /// Commission rule basis not set.
        /// or
        /// Soft Commission rule basis not set.
        /// </exception>
        public TaxLot CalculateCommissionAccountWise(CommissionRule commissionRuleToApply, TaxLot taxlot, AllocationGroup allocatedGroup)
        {
            try
            {
                // calculated commission only if source is auto or call set to recalculate commission, PRANA-12889
                if (allocatedGroup.CommissionSource == (int)CommisionSource.Auto || allocatedGroup.IsRecalculateCommission)
                {
                    taxlot.Commission = CalculateCommissionValueTaxlot(commissionRuleToApply.Commission, allocatedGroup, taxlot, "Commission");
                }

                // calculated soft commission only if source is auto or call set to recalculate soft commission, PRANA-12889
                if (allocatedGroup.SoftCommissionSource == (int)CommisionSource.Auto || allocatedGroup.IsRecalculateCommission)
                {
                    taxlot.SoftCommission = CalculateCommissionValueTaxlot(commissionRuleToApply.SoftCommission, allocatedGroup, taxlot, "Soft Commission");
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

        /// <summary>
        /// Calculates the fees account wise.
        /// </summary>
        /// <param name="commissionRuleToApply">The commission rule to apply.</param>
        /// <param name="taxlot">The taxlot.</param>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <returns>TaxLot.</returns>
        /// <exception cref="System.Exception">
        /// Commission rule basis not set. It should depend either on Shares,Contracts or Notional values.
        /// or
        /// Commission rule basis not set. It should depend either on Shares,Contracts or Notional values.
        /// </exception>
        public TaxLot CalculateFeesAccountWise(CommissionRule commissionRuleToApply, TaxLot taxlot, AllocationGroup allocationGroup)
        {
            try
            {
                double calculatedFees = 0;
                double finalCalculatedFees = 0;
                ///Fees Calculation
                if (commissionRuleToApply.IsClearingFeeApplied)
                {
                    calculatedFees = CalculateClearingFeeValueForTaxlot(commissionRuleToApply.ClearingFeeObj, allocationGroup, taxlot, "Other Broker Fee");

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
                }
                taxlot.OtherBrokerFees = finalCalculatedFees;// System.Math.Round(finalCalculatedFees, 2);

                calculatedFees = 0;
                finalCalculatedFees = 0;

                ///Fees Calculation
                if (commissionRuleToApply.IsClearingBrokerFeeApplied)
                {
                    calculatedFees = CalculateClearingFeeValueForTaxlot(commissionRuleToApply.ClearingBrokerFeeObj, allocationGroup, taxlot, "Clearing Broker Fee");

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
                }
                taxlot.ClearingBrokerFee = finalCalculatedFees;// System.Math.Round(finalCalculatedFees, 2);
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
        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the commission rate.
        /// </summary>
        /// <param name="clearingFeeToApply">The commission rule to apply.</param>
        /// <param name="valueToCheck">The value to check.</param>
        /// <returns>System.Double.</returns>
        private double GetClearingFeeRate(Prana.BusinessObjects.ClearingFee clearingFeeToApply, double valueToCheck)
        {
            if (clearingFeeToApply.IsCriteriaApplied)
            {
                List<ClearingFeeCriteria> commissionRuleCriteriaList = clearingFeeToApply.ClearingFeeRuleCriteiaList;
                foreach (ClearingFeeCriteria commissionRuleCriteria in commissionRuleCriteriaList)
                {
                    double maxValue = commissionRuleCriteria.ValueLessThanOrEqual == 0 ? double.MaxValue : commissionRuleCriteria.ValueLessThanOrEqual;
                    double minValue = commissionRuleCriteria.ValueGreaterThan;
                    if (valueToCheck <= maxValue && valueToCheck > minValue)
                        return commissionRuleCriteria.ClearingFeeRate;
                }
            }
            return clearingFeeToApply.ClearingFeeRate;
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

        /// <summary>
        /// Calculate fees based on criteria and rate
        /// </summary>
        /// <param name="calculationBasis"></param>
        /// <param name="allocationGroup"></param>
        /// <param name="rate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private double CalculateFeesValueForGroup(CalculationBasis calculationBasis, AllocationGroup allocationGroup, double rate, string type)
        {
            double calculatedValue = 0.0;
            try
            {
                switch (calculationBasis)
                {
                    case CalculationBasis.Shares:
                    case CalculationBasis.Contracts:
                        calculatedValue = allocationGroup.CumQty * rate;
                        break;
                    case CalculationBasis.Notional:
                        double notional = GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                        calculatedValue = notional * rate * 0.0001;
                        break;
                    case CalculationBasis.FlatAmount:
                        calculatedValue = rate;
                        break;
                    case CalculationBasis.Commission:
                        calculatedValue = allocationGroup.Commission * rate * 0.0001;
                        break;
                    case CalculationBasis.NotionalPlusCommission:
                        calculatedValue = (GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier) + allocationGroup.Commission) * rate * 0.0001;
                        break;
                    default:
                        throw new Exception(type + " rule basis not set.");
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

        private double CalculateFeesValueForGroup(OtherFeeRule otherFeeRule, AllocationGroup allocationGroup)
        {
            double calculatedValue = 0.0;
            try
            {
                bool isLong = false;
                switch (allocationGroup.OrderSideTagValue)
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

                double rate = GetOtherFeeRate(otherFeeRule, allocationGroup.CumQty, isLong);

                switch (calculationBasis)
                {
                    case CalculationFeeBasis.Shares:
                    case CalculationFeeBasis.Contracts:
                    case CalculationFeeBasis.AvgPrice:
                        calculatedValue = allocationGroup.CumQty * rate;
                        break;
                    case CalculationFeeBasis.Notional:
                        double notional = GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                        calculatedValue = notional * rate * 0.0001;
                        break;

                    case CalculationFeeBasis.FlatAmount:
                        calculatedValue = rate;
                        break;
                    case CalculationFeeBasis.Commission:
                        calculatedValue = allocationGroup.Commission * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.NotionalPlusCommission:
                        calculatedValue = (GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier) + allocationGroup.Commission) * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.SoftCommission:
                        calculatedValue = allocationGroup.SoftCommission * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.StampDuty:
                        calculatedValue = allocationGroup.StampDuty * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.TransactionLevy:
                        calculatedValue = allocationGroup.TransactionLevy * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.ClearingFee:
                        calculatedValue = allocationGroup.ClearingFee * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.TaxOnCommissions:
                        calculatedValue = allocationGroup.TaxOnCommissions * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.MiscFees:
                        calculatedValue = allocationGroup.MiscFees * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.SecFee:
                        calculatedValue = allocationGroup.SecFee * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.OccFee:
                        calculatedValue = allocationGroup.OccFee * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.OrfFee:
                        calculatedValue = allocationGroup.OrfFee * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.OtherBrokerFee:
                        calculatedValue = allocationGroup.OtherBrokerFees * rate * 0.0001;
                        break;
                    case CalculationFeeBasis.ClearingBrokerFee:
                        calculatedValue = allocationGroup.ClearingBrokerFee * rate * 0.0001;
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
        /// Calculate fees based on criteria and rate
        /// </summary>
        /// <param name="calculationBasis"></param>
        /// <param name="allocationGroup"></param>
        /// <param name="rate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private double CalculateClearingFeeForGroup(ClearingFee clearingFee, AllocationGroup allocationGroup, string type)
        {
            double calculatedValue = 0.0;
            double rate = 0.0;
            try
            {
                switch (clearingFee.RuleAppliedOn)
                {
                    case CalculationBasis.Shares:
                    case CalculationBasis.Contracts:
                        rate = GetClearingFeeRate(clearingFee, allocationGroup.CumQty);
                        calculatedValue = allocationGroup.CumQty * rate;
                        break;
                    case CalculationBasis.Notional:
                        rate = GetClearingFeeRate(clearingFee, allocationGroup.CumQty);
                        double notional = GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                        calculatedValue = notional * rate * 0.0001;
                        break;
                    case CalculationBasis.FlatAmount:
                        calculatedValue = GetClearingFeeRate(clearingFee, allocationGroup.CumQty);
                        break;
                    case CalculationBasis.Commission:
                        rate = GetClearingFeeRate(clearingFee, allocationGroup.CumQty);
                        calculatedValue = allocationGroup.Commission * rate * 0.0001;
                        break;
                    case CalculationBasis.SoftCommission:
                        rate = GetClearingFeeRate(clearingFee, allocationGroup.CumQty);
                        calculatedValue = allocationGroup.SoftCommission * rate * 0.0001;
                        break;
                    case CalculationBasis.NotionalPlusCommission:
                        rate = GetClearingFeeRate(clearingFee, allocationGroup.CumQty);
                        calculatedValue = (GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier) + allocationGroup.Commission) * rate * 0.0001;
                        break;
                    default:
                        throw new Exception(type + " rule basis not set.");
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
        /// Calculate fees based on criteria and rate for taxlot
        /// </summary>
        /// <param name="calculationBasis"></param>
        /// <param name="allocationGroup"></param>
        /// <param name="taxlot"></param>
        /// <param name="rate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private double CalculateClearingFeeValueForTaxlot(ClearingFee clearingFee, AllocationGroup allocationGroup, TaxLot taxlot, string type)
        {
            double calculatedValue = 0.0;
            double rate = 0.0;
            try
            {
                switch (clearingFee.RuleAppliedOn)
                {
                    case CalculationBasis.Shares:
                    case CalculationBasis.Contracts:
                        rate = GetClearingFeeRate(clearingFee, taxlot.TaxLotQty);
                        calculatedValue = taxlot.TaxLotQty * rate;
                        break;
                    case CalculationBasis.Notional:
                        rate = GetClearingFeeRate(clearingFee, taxlot.TaxLotQty);
                        double notional = GetNotionalValue(taxlot.TaxLotQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                        calculatedValue = notional * rate * 0.0001;
                        break;
                    case CalculationBasis.FlatAmount:
                        calculatedValue = GetClearingFeeRate(clearingFee, taxlot.TaxLotQty);
                        break;
                    case CalculationBasis.Commission:
                        rate = GetClearingFeeRate(clearingFee, taxlot.TaxLotQty);
                        calculatedValue = taxlot.Commission * rate * 0.0001;
                        break;
                    case CalculationBasis.SoftCommission:
                        rate = GetClearingFeeRate(clearingFee, taxlot.TaxLotQty);
                        calculatedValue = taxlot.SoftCommission * rate * 0.0001;
                        break;
                    case CalculationBasis.NotionalPlusCommission:
                        rate = GetClearingFeeRate(clearingFee, taxlot.TaxLotQty);
                        calculatedValue = (GetNotionalValue(taxlot.TaxLotQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier) + taxlot.Commission) * rate * 0.0001;
                        break;
                    default:
                        throw new Exception(type + " rule basis not set.");
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
        /// Calculate fees based on criteria and rate for taxlot
        /// </summary>
        /// <param name="calculationBasis"></param>
        /// <param name="allocationGroup"></param>
        /// <param name="taxlot"></param>
        /// <param name="rate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private double CalculateFeesValueForTaxlot(CalculationBasis calculationBasis, AllocationGroup allocationGroup, TaxLot taxlot, double rate, string type)
        {
            double calculatedValue = 0.0;
            try
            {
                switch (calculationBasis)
                {
                    case CalculationBasis.Shares:
                    case CalculationBasis.Contracts:
                        calculatedValue = taxlot.TaxLotQty * rate;
                        break;
                    case CalculationBasis.Notional:
                        double notional = GetNotionalValue(taxlot.TaxLotQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                        calculatedValue = notional * rate * 0.0001;
                        break;
                    case CalculationBasis.FlatAmount:
                        calculatedValue = rate;
                        break;
                    default:
                        throw new Exception(type + " rule basis not set.");
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
        /// Calculate notional
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="avgPrice"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        private double GetNotionalValue(double qty, double avgPrice, double multiplier)
        {
            return qty * avgPrice * multiplier;
        }

        /// <summary>
        /// Gets the commission rate.
        /// </summary>
        /// <param name="commissionRuleToApply">The commission rule to apply.</param>
        /// <param name="valueToCheck">The value to check.</param>
        /// <returns>System.Double.</returns>
        private double GetCommissionRate(Prana.BusinessObjects.Commission commissionRuleToApply, double valueToCheck)
        {
            if (commissionRuleToApply.IsCriteriaApplied)
            {
                List<CommissionRuleCriteria> commissionRuleCriteriaList = commissionRuleToApply.CommissionRuleCriteiaList;
                foreach (CommissionRuleCriteria commissionRuleCriteria in commissionRuleCriteriaList)
                {
                    double maxValue = commissionRuleCriteria.ValueLessThanOrEqual == 0 ? double.MaxValue : commissionRuleCriteria.ValueLessThanOrEqual;
                    double minValue = commissionRuleCriteria.ValueGreaterThan;
                    if (valueToCheck <= maxValue && valueToCheck > minValue)
                        return commissionRuleCriteria.CommissionRate;
                }
            }
            return commissionRuleToApply.CommissionRate;
        }

        /// <summary>
        /// Calculate Commission and soft commission
        /// </summary>
        /// <param name="commission"></param>
        /// <param name="allocationGroup"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private double CalculateCommissionValueGroup(Commission commission, AllocationGroup allocationGroup, string type)
        {
            double finalCalculatedValue = 0.0;
            try
            {
                double rate = 0.0;
                double calculatedValue = 0.0;
                switch (commission.RuleAppliedOn)
                {
                    case CalculationBasis.Shares:
                        rate = GetCommissionRate(commission, allocationGroup.CumQty);
                        calculatedValue = allocationGroup.CumQty * rate;
                        break;
                    case CalculationBasis.Contracts:
                        rate = GetCommissionRate(commission, allocationGroup.CumQty);
                        calculatedValue = allocationGroup.CumQty * rate;
                        break;
                    case CalculationBasis.Notional:
                        double notional = GetNotionalValue(allocationGroup.CumQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                        rate = GetCommissionRate(commission, notional);
                        // calculatedCommission = notional * commissionRuleToApply.CommissionRate; previous one
                        calculatedValue = notional * rate * 0.0001;
                        break;
                    case CalculationBasis.AvgPrice:
                        rate = GetCommissionRate(commission, allocationGroup.AvgPrice);
                        calculatedValue = allocationGroup.CumQty * rate;
                        break;
                    case CalculationBasis.FlatAmount:
                        calculatedValue = commission.CommissionRate;
                        break;
                    default:
                        throw new Exception(type + " rule basis not set.");
                }
                if (calculatedValue <= commission.MinCommission && allocationGroup.CumQty > 0)
                {
                    ///If calculatedCommission is less than the absolute minimum commission, then we use the 
                    ///minimum commission as the final commission
                    finalCalculatedValue = commission.MinCommission;
                }
                if (commission.MaxCommission != 0 && commission.MaxCommission != double.MinValue && allocationGroup.CumQty > 0)
                {
                    if (calculatedValue >= commission.MaxCommission)
                    {
                        finalCalculatedValue = commission.MaxCommission;
                    }
                    if (calculatedValue < commission.MaxCommission && calculatedValue > commission.MinCommission)
                    {
                        finalCalculatedValue = calculatedValue;
                    }
                }
                else if (calculatedValue > commission.MinCommission)
                {
                    finalCalculatedValue = calculatedValue;
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
            return finalCalculatedValue;
        }

        private double CalculateCommissionValueTaxlot(Commission commission, AllocationGroup allocationGroup, TaxLot taxlot, string type)
        {
            double rate = 0.0;
            double calculatedValue = 0.0;
            double finalCalculatedValue = 0.0;
            switch (commission.RuleAppliedOn)
            {
                case CalculationBasis.Shares:
                    rate = GetCommissionRate(commission, taxlot.TaxLotQty);
                    calculatedValue = taxlot.TaxLotQty * rate;
                    break;
                case CalculationBasis.Contracts:
                    rate = GetCommissionRate(commission, taxlot.TaxLotQty);
                    calculatedValue = taxlot.TaxLotQty * rate;
                    break;
                case CalculationBasis.Notional:
                    double notional = GetNotionalValue(taxlot.TaxLotQty, allocationGroup.AvgPrice, allocationGroup.ContractMultiplier);
                    rate = GetCommissionRate(commission, notional);
                    calculatedValue = notional * rate * 0.0001;
                    break;
                case CalculationBasis.AvgPrice:
                    rate = GetCommissionRate(commission, allocationGroup.AvgPrice);
                    calculatedValue = taxlot.TaxLotQty * rate;
                    break;
                case CalculationBasis.FlatAmount:
                    calculatedValue = commission.CommissionRate;
                    break;
                default:
                    throw new Exception(type + " rule basis not set.");
            }
            if (calculatedValue <= commission.MinCommission && allocationGroup.CumQty > 0)
            {
                ///If calculatedCommission is less than the absolute minimum commission, then we use the 
                ///minimum commission as the final commission
                finalCalculatedValue = commission.MinCommission;
            }
            if (commission.MaxCommission != 0 && commission.MaxCommission != double.MinValue && allocationGroup.CumQty > 0)
            {
                if (calculatedValue >= commission.MaxCommission)
                {
                    finalCalculatedValue = commission.MaxCommission;
                }
                if (calculatedValue < commission.MaxCommission && calculatedValue > commission.MinCommission)
                {
                    finalCalculatedValue = calculatedValue;
                }
            }
            else if (calculatedValue > commission.MinCommission)
            {
                finalCalculatedValue = calculatedValue;
            }
            return finalCalculatedValue;
        }
        #endregion

        /// <summary>
        /// Re-calculation of the other fee when avg price is changed.
        /// </summary>
        /// <param name="allocatedGroup">The allocated group.</param>
        public void ReCalculateOtherFeeForGroup(AllocationGroup allocatedGroup, List<OtherFeeType> listofFeesToApply)
        {
            List<OtherFeeRule> lstOtherFeeRule = new List<OtherFeeRule>();
            List<OtherFeeRule> otherFeeRuleList = new List<OtherFeeRule>();

            lstOtherFeeRule = CommissionRulesCacheManager.GetInstance().GetOtherFeeRuleAuecDict(allocatedGroup.AUECID);

            foreach (OtherFeeRule otherFeeRule in lstOtherFeeRule)
            {
                if (listofFeesToApply.Contains(otherFeeRule.OtherFeeType))
                {
                    otherFeeRuleList.Add(otherFeeRule);
                }
            }
            bool isCommissionAndFeeZero = allocatedGroup.IsSwapped && Convert.ToBoolean(Convert.ToInt32(CachedDataManager.GetInstance.GetPranaPreferenceByKey(ApplicationConstants.CONST_ZEROCOMMISSIONFORSWAPS)));
            CalculateOtherFees(otherFeeRuleList, allocatedGroup, isCommissionAndFeeZero);
        }

        #region Distribute Fees and Commission

        /// <summary>
        /// Distributes the other fees in tax lot.
        /// </summary>
        /// <param name="group">The group.</param>
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
                    taxlot.OptionPremiumAdjustment = (group.OptionPremiumAdjustment * (taxlot.TaxLotQty / group.AllocatedQty));
                }
            }
        }

        /// <summary>
        /// Distributes the fees in tax lot.
        /// </summary>
        /// <param name="group">The group.</param>
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

        /// <summary>
        /// Distributes the commision in tax lot.
        /// </summary>
        /// <param name="group">The group.</param>
        public void DistributeCommisionInTaxLot(AllocationGroup group)
        {
            foreach (TaxLot taxlot in group.TaxLots)
            {
                if (group.AllocatedQty > 0)
                {
                    taxlot.Commission = (group.Commission * (taxlot.TaxLotQty / group.AllocatedQty));
                    taxlot.SoftCommission = (group.SoftCommission * (taxlot.TaxLotQty / group.AllocatedQty));
                }
            }
        }
        #endregion
    }
}
