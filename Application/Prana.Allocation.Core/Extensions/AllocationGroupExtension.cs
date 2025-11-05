// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-05-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-09-2014
// ***********************************************************************
// <copyright file="AllocationGroupExtension.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Core.DataAccess;
using Prana.Allocation.Core.FormulaStore;
using Prana.Allocation.Core.Managers;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

/// <summary>
/// The Extensions namespace.
/// </summary>
namespace Prana.Allocation.Core.Extensions
{
    /// <summary>
    /// TODO: Need to refactor this class
    /// </summary>
    internal static class AllocationGroupExtension
    {
        /// <summary>
        /// Enriches the data from other services.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static void EnrichDataFromOtherServices(this AllocationGroup group)
        {
            try
            {
                group.FillAdditionalParameters();//this method assign two more variables _swapParameters and _orders after initialization 
                ServiceProxyConnector.SecmasterProxy.SetSecuritymasterDetails(group);

                #region Update NirvanaProcessDate to auec local date if is minvalue - modified by omshiv
                if (group.NirvanaProcessDate <= DateTimeConstants.MinValue)
                {
                    group.NirvanaProcessDate = group.AUECLocalDate;
                }
                #endregion

                group.OrderCount = group.Orders.Count;
                group.TaxLots = new List<TaxLot>();
                foreach (Level1Allocation level1Allocation in group.Level1AllocationList)
                {
                    AllocationLevelClass level1 = new AllocationLevelClass(level1Allocation.GroupID);
                    #region Setting Properties For Level 1

                    level1.AllocatedQty = level1Allocation.AllocatedQty;
                    //level1.LevelnAllocationID = level1Allocation.LevelnAllocationID;//ReadOnly Property
                    level1.LevelnID = Convert.ToInt32(level1Allocation.AccountID);
                    //level1.Name=level1Allocation.
                    level1.Percentage = level1Allocation.Percentage;

                    foreach (TaxLot taxlot in level1Allocation.TaxLotsH)
                    {
                        taxlot.CopyBasicDetails(group);

                        taxlot.Level1ID = level1.LevelnID;
                        taxlot.Percentage = level1.Percentage;
                        // update taxlot closing status i.e. Open, Closed or Partially closed
                        // we use closing services in persistence manager to make code more optimized and to avoid looping through
                        taxlot.ClosingStatus = ClosingStatus.Open;
                        ServiceProxyConnector.ClosingProxy.SetTaxlotClosingStatus(taxlot);
                        group.TaxLots.Add(taxlot);
                        AllocationLevelClass level2 = new AllocationLevelClass(taxlot.GroupID);
                        level2.AllocatedQty = taxlot.TaxLotQty;
                        //taxlot.Level2Percentage=(float) (taxlot.TaxLotQty / level1.AllocatedQty);
                        //level2.GroupID = taxlot.GroupID;
                        //level2.LevelnAllocationID = taxlot.Level1AllocationID;
                        level2.LevelnID = taxlot.Level2ID;
                        //level2.Name = taxlot.Level2Name;
                        if (taxlot.Level2Percentage != 0)
                        {
                            level2.Percentage = taxlot.Level2Percentage;
                        }
                        level1.AddChilds(level2);
                    }
                    group.Allocations.Add(level1);

                    // Update group status based on its taxlot(s) status
                    UpdateGroupClosingStatus(group);
                    #endregion

                }
                group.AllocationCheck();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Enrich Data From Closing Service
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="dr">The dr.</param>
        /// <param name="isMultipleTaxlots">if set to <c>true</c> [is multiple taxlots].</param>
        internal static void EnrichDataFromClosingService(this AllocationGroup group)
        {
            try
            {
                foreach (TaxLot taxlot in group.TaxLots)
                {
                    ServiceProxyConnector.ClosingProxy.SetTaxlotClosingStatus(taxlot);
                }
                AllocationGroupExtension.UpdateGroupClosingStatus(group);
                group.AllocationCheck();

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
        /// Enriches the data from other services.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="dr">The dr.</param>
        /// <param name="isMultipleTaxlots">if set to <c>true</c> [is multiple taxlots].</param>
        internal static void EnrichDataFromOtherServices(this AllocationGroup group, DataRow dr, bool isMultipleTaxlots)
        {
            try
            {
                ServiceProxyConnector.SecmasterProxy.SetSecuritymasterDetails(group);
                if (group.NirvanaProcessDate <= DateTimeConstants.MinValue)
                {
                    group.NirvanaProcessDate = group.AUECLocalDate;
                }

                if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                {
                    group.AllocationCheck();
                    return;
                }

                AllocationLevelClass l1 = new AllocationLevelClass(group.GroupID, dr);
                if (!group.Allocations.Contains(l1.LevelnID))
                    group.Allocations.Add(l1);

                bool taxlotExists = false;

                if (isMultipleTaxlots)
                {
                    string taxlotId = dr["Level1AllocationID"].ToString() + dr["Level2ID"].ToString();

                    foreach (TaxLot t in group.TaxLots)
                    {
                        if (t.TaxLotID.Equals(taxlotId))
                        {
                            taxlotExists = true;
                            break;
                        }
                    }
                }

                if (!taxlotExists)
                {
                    TaxLot taxlot = new TaxLot(dr);
                    taxlot.CopyBasicDetails(group);
                    taxlot.GroupID = group.GroupID;
                    taxlot.Level1ID = l1.LevelnID;
                    taxlot.Percentage = l1.Percentage;
                    taxlot.ClosingStatus = ClosingStatus.Open;
                    //ServiceProxyConnector.ClosingProxy.SetTaxlotClosingStatus(taxlot);
                    group.TaxLots.Add(taxlot);

                    AllocationLevelClass level2 = new AllocationLevelClass(taxlot.GroupID);
                    level2.AllocatedQty = taxlot.TaxLotQty;
                    level2.LevelnID = taxlot.Level2ID;
                    string levelnAllocationID = taxlot.GroupID + taxlot.Level1ID;
                    if (taxlot.Level2Percentage != 0)
                    {
                        level2.Percentage = taxlot.Level2Percentage;
                        taxlot.Percentage = (l1.Percentage * level2.Percentage)/100;
                    }
                    if (isMultipleTaxlots)
                    {
                        for (int i = 0; i < group.Allocations.Collection.Count; i++)
                        {
                            if (group.Allocations.Collection[i].LevelnAllocationID.Equals(levelnAllocationID))
                            {
                                group.Allocations.Collection[i].AddChilds(level2);
                                break;
                            }
                        }
                    }
                    else
                    {
                        l1.AddChilds(level2);
                    }
                    // AllocationGroupExtension.UpdateGroupClosingStatus(group);
                    // group.AllocationCheck();
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
        /// Keeps the attributes values.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static void KeepAttributesValues(this AllocationGroup group)
        {
            try
            {
                if (group.TaxLots.Count > 0)
                {
                    //make empty DeletedTaxlotsWithExtraFields while unallocating data and fill with new details
                    group.TaxLotIdsWithAttributes = string.Empty;
                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        if (!string.IsNullOrEmpty(taxlot.LotId) || !string.IsNullOrEmpty(taxlot.ExternalTransId))
                        {
                            group.TaxLotIdsWithAttributes = group.TaxLotIdsWithAttributes + taxlot.TaxLotID + Seperators.SEPERATOR_5 + taxlot.LotId + Seperators.SEPERATOR_5 + taxlot.ExternalTransId + Seperators.SEPERATOR_6;
                        }
                    }
                    //remove last seperator from the DeletedTaxlotsWithExtraFields
                    if (group.TaxLotIdsWithAttributes.Length > 0)
                    {
                        group.TaxLotIdsWithAttributes = group.TaxLotIdsWithAttributes.Substring(0, group.TaxLotIdsWithAttributes.Length - 1);
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
        /// Allocates the group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="accounts">The accounts.</param>
        /// <returns>AllocationGroup.</returns>
        internal static AllocationGroup AllocateGroup(this AllocationGroup group, AllocationLevelList accounts, bool isReallocatedFromBlotter = false)
        {

            try
            {
                PostTradeConstants.ORDERSTATE_ALLOCATION prevState = group.State;
                // collect sent state Taxlots
                group.KeepAttributesValues();
                group.Allocate(accounts);

                SetNameDetailsInGroup(group);
                //moved logic for auto and manual calculation of commission to CommissionCalculator.cs, PRANA-12889
                //double softCommission = group.SoftCommission;
                //double commission = group.Commission;
                // Divya: 14022013 : if group is unallocated & user wants to allocate it and CommissionSource is Auto then on the basis of cumqty, commission will be calculated. 
                // After that if user wants he can reallocate it and commission will be distributed in the taxlots
                //need this check so that fees and commission is calculated only if source is Auto, PRANA-12889
                if ((group.CommissionSource == (int)CommisionSource.Auto || group.SoftCommissionSource == (int)CommisionSource.Auto) || group.IsRecalculateCommission)
                {
                    //set commission source to auto only if counterparty is changed, PRANA-12889
                    // if counter party is changed or commission should be recalculated, then set commission source to auto, PRANA-12889
                    if (group.IsRecalculateCommission)
                    {
                        group.CommissionSource = (int)CommisionSource.Auto;
                        group.SoftCommissionSource = (int)CommisionSource.Auto;
                        group.CommSource = CommisionSource.Auto;
                        group.SoftCommSource = CommisionSource.Auto;
                        group.IsRecalculateCommission = false;
                    }
                    CommissionCalculator.GetInstance.StartCalculation(group);
                }
                //if (group.CommissionSource == (int)CommisionSource.Manual)
                //{
                //    group.Commission = commission;
                //}
                //if (group.SoftCommissionSource == (int)CommisionSource.Manual)
                //{
                //    group.SoftCommission = softCommission;
                //}
                //group.DistributeCommisionInTaxLot(group.CommissionSource == (int)CommisionSource.Manual, group.SoftCommissionSource == (int)CommisionSource.Manual);
                group.AllocationCheck(isReallocatedFromBlotter);

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

        /// <summary>
        /// Sets the name details in group.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static void SetNameDetailsInGroup(this AllocationGroup group)
        {
            try
            {
                if (group.TaxLots != null)
                {
                    string[] grpAttributes = null;
                    if (!string.IsNullOrEmpty(group.TaxLotIdsWithAttributes) && group.TaxLotIdsWithAttributes.Length > 0)
                    {
                        grpAttributes = group.TaxLotIdsWithAttributes.ToString().Split(Seperators.SEPERATOR_6);
                    }
                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        if (grpAttributes != null && grpAttributes.Length > 0)
                        {
                            UpdateTaxlotAttributes(grpAttributes, taxlot);
                        }
                        taxlot.Level1Name = CachedDataManager.GetInstance.GetAccountText(taxlot.Level1ID);
                        taxlot.Level2Name = CachedDataManager.GetInstance.GetStrategyText(taxlot.Level2ID);
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
        /// Updates the taxlot attributes.
        /// </summary>
        /// <param name="grpAttributes">The GRP attributes.</param>
        /// <param name="taxlot">The taxlot.</param>
        private static void UpdateTaxlotAttributes(string[] grpAttributes, TaxLot taxlot)
        {
            try
            {
                //grpAttributes is string array which contains taxlotid's with attributes
                for (int i = 0; i < grpAttributes.Length; i++)
                {
                    //grpMinAttribute is string array which contains taxlotid as first element and remaining elements are attributes
                    string[] grpMinAttribute = grpAttributes[i].Split(Seperators.SEPERATOR_5);
                    if (grpMinAttribute[0].Length > 0 && (grpMinAttribute[0].Equals(taxlot.TaxLotID)))
                    {
                        //second element of grpMinAttribute contains LotId
                        if (grpMinAttribute[1].Length > 0)
                        {
                            taxlot.LotId = grpMinAttribute[1];
                        }
                        //third element of grpMinAttribute contains ExternalTransId
                        if (grpMinAttribute[2].Length > 0)
                        {
                            taxlot.ExternalTransId = grpMinAttribute[2];
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
        /// Updates the group closing status.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static void UpdateGroupClosingStatus(this AllocationGroup group)
        {
            try
            {
                //assume group closing status is open by default  
                group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.Open;
                group.ClosingAlgoText = PostTradeEnums.CloseTradeAlogrithm.NONE.ToString();
                int firstTaxlotsClosingAlgo = 0;
                int count = 0;
                //update group status whether group is generated by Option Exercise or Corp Action
                UpdateGroupStatus(group);

                if (group.TaxLots.Count > 0)
                {
                    int closeCount = 0;
                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        #region checking Closing Algo is same for all or not and set accordingly
                        if (count == 0)
                        {
                            firstTaxlotsClosingAlgo = taxlot.ClosingAlgo;
                            group.ClosingAlgoText = Enum.GetName(typeof(PostTradeEnums.CloseTradeAlogrithm), taxlot.ClosingAlgo);
                            count++;
                        }
                        if (firstTaxlotsClosingAlgo != taxlot.ClosingAlgo)
                        {
                            group.ClosingAlgoText = ApplicationConstants.C_Multiple;
                        }
                        #endregion

                        //update the minimum of close trade of all taxlots in a group
                        //AUECMODIFIEDDATE IS TRADE DATE
                        if (taxlot.AUECModifiedDate != DateTimeConstants.MinValue && (group.ClosingDate > taxlot.AUECModifiedDate || group.ClosingDate == DateTimeConstants.MinValue))
                        {
                            group.ClosingDate = taxlot.AUECModifiedDate;
                        }
                        //if any one of the group taxlot is partially closed then group will be partially closed
                        if ((taxlot.ClosingStatus == Prana.BusinessObjects.AppConstants.ClosingStatus.PartiallyClosed))
                        {
                            group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.PartiallyClosed;
                            closeCount++;
                            break;
                        }
                        if (taxlot.ClosingStatus == Prana.BusinessObjects.AppConstants.ClosingStatus.Closed)
                        {
                            closeCount += 2;
                        }
                    }

                    if ((closeCount / 2) == group.TaxLots.Count)// in case of closed taxlots, closeCount is always incremented by 2
                        group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.Closed;
                    else if ((closeCount / 2) > 0)//if closeCount is greater than 0 (closeCount is always incremented by 1)
                        group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.PartiallyClosed;

                    if (group.ClosingStatus == Prana.BusinessObjects.AppConstants.ClosingStatus.Open)
                        group.ClosingDate = DateTimeConstants.MinValue;
                }
                Logger.LoggerWrite("Updated Group Closing Status for group: " + group.GroupID + " = " + group.ClosingStatus, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_ALLOCATION_SERVICE);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// here we update group status whether group is generated by Option Exercise or Corp Action
        /// </summary>
        /// <param name="group">The group.</param>
        private static void UpdateGroupStatus(AllocationGroup group)
        {
            try
            {
                // Uncommenting this code to update it according to Corporate action changes
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-5229
                PostTradeEnums.Status groupStatus = ServiceProxyConnector.ClosingProxy.CheckGroupStatus(group);
                if (groupStatus.Equals(PostTradeEnums.Status.Closed))
                {
                    group.GroupStatus = PostTradeEnums.Status.None;
                }
                else
                {
                    group.GroupStatus = groupStatus;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Allocations the check.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static void AllocationCheck(this AllocationGroup group, bool isReallocatedFromBlotter = false)
        {
            try
            {
                if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                {
                    List<TaxLot> taxlots = new List<TaxLot>();
                    TaxLot taxlot = CreateUnAllocatedTaxLot(group, group.GroupID);
                    taxlots.Add(taxlot);
                    CopySwapParameters(taxlots, group);
                    group.ResetTaxlotDictionary(taxlots);
                }
                else
                {
                    SetSideMultiplier(group.TaxLots);
                    CopySwapParameters(group.TaxLots, group);
                    group.ResetTaxlotDictionary(group.TaxLots, isReallocatedFromBlotter);

                    //[Rahul: 20130320] http://jira.nirvanasolutions.com:8080/browse/MON-44
                    //After discussion with closing team, taxlotclosingID only comes when equity is generated
                    //through option exercise. For Cash management, we are setting its closing mode.
                    if (group.TaxLotClosingId != null && !group.TaxLotClosingId.Equals(string.Empty))
                    {
                        group.SetClosingMode();
                    }
                }
                group.UpdateSchemeName();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates the un allocated tax lot.
        /// </summary>
        /// <param name="baseMsg">The base MSG.</param>
        /// <param name="groupID">The group identifier.</param>
        /// <returns>TaxLot.</returns>
        private static TaxLot CreateUnAllocatedTaxLot(PranaBasicMessage baseMsg, string groupID)
        {
            TaxLot taxLot = null;
            try
            {
                taxLot = new TaxLot();
                taxLot.TaxLotQty = baseMsg.CumQty;
                taxLot.TaxLotID = groupID;
                taxLot.GroupID = groupID;
                taxLot.SideMultiplier = Calculations.GetSideMultilpier(baseMsg.OrderSideTagValue);
                taxLot.PositionTag = (PositionTag)(GetPositionTagBySide(baseMsg.OrderSideTagValue));

                taxLot.CopyBasicDetails(baseMsg);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return taxLot;
        }

        /// <summary>
        /// Gets the position tag by side.
        /// </summary>
        /// <param name="orderSideTagValue">The order side tag value.</param>
        /// <returns>System.Int32.</returns>
        private static int GetPositionTagBySide(string orderSideTagValue)
        {
            switch (orderSideTagValue)
            {
                case FIXConstants.SIDE_Buy:
                case FIXConstants.SIDE_BuyMinus:
                case FIXConstants.SIDE_Buy_Open:
                case FIXConstants.SIDE_Buy_Closed:
                case FIXConstants.SIDE_Buy_Cover:
                default:
                    return (int)PositionTag.Long;

                case FIXConstants.SIDE_Sell:
                case FIXConstants.SIDE_SellShort:
                case FIXConstants.SIDE_Sell_Open:
                case FIXConstants.SIDE_Sell_Closed:
                    return (int)PositionTag.Short;

            }
        }

        /// <summary>
        /// Sets the closing mode.
        /// </summary>
        /// <param name="taxlots">The taxlots.</param>
        internal static void SetClosingMode(this AllocationGroup group)
        {
            List<TaxLot> taxlots = group.TaxLots;
            foreach (TaxLot taxlot in taxlots)
            {
                taxlot.ClosingMode = ClosingMode.Exercise;
            }
        }

        /// <summary>
        /// Copies the swap parameters.
        /// </summary>
        /// <param name="taxlots">The taxlots.</param>
        /// <param name="group">The group.</param>
        private static void CopySwapParameters(List<TaxLot> taxlots, AllocationGroup group)
        {
            try
            {
                if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED && group.IsSwapped.Equals(true))
                {
                    taxlots[0].ISSwap = true;
                    taxlots[0].SwapParameters = group.SwapParameters.Clone();
                }
                else if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED && group.IsSwapped.Equals(true))
                {
                    foreach (TaxLot taxlotVar in taxlots)
                    {
                        taxlotVar.ISSwap = true;
                        taxlotVar.SwapParameters = new SwapParameters();
                        taxlotVar.SwapParameters = group.SwapParameters.Clone();
                        if (group.Quantity != 0)
                            taxlotVar.SwapParameters.NotionalValue = (taxlotVar.TaxLotQty * group.SwapParameters.NotionalValue) / group.Quantity;
                        else
                            taxlotVar.SwapParameters.NotionalValue = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the side multiplier.
        /// </summary>
        /// <param name="taxlotList">The taxlot list.</param>
        private static void SetSideMultiplier(List<TaxLot> taxlotList)
        {
            try
            {
                foreach (TaxLot taxlot in taxlotList)
                {
                    taxlot.SideMultiplier = Calculations.GetSideMultilpier(taxlot.OrderSideTagValue);
                    taxlot.PositionTag = (PositionTag)(GetPositionTagBySide(taxlot.OrderSideTagValue));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified allocation group contains account.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <param name="accountId">The account identifier.</param>
        /// <returns><c>true</c> if the specified allocation group contains account; otherwise, <c>false</c>.</returns>
        internal static bool ContainsAccount(this AllocationGroup allocationGroup, int accountId)
        {
            try
            {
                List<int> accountList = (from g in allocationGroup.Allocations.Collection
                                         select g.LevelnID).ToList();

                return (accountList.Contains(accountId));
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified allocation group contains strategy.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <param name="strategyId">The strategy identifier.</param>
        /// <returns><c>true</c> if the specified allocation group contains strategy; otherwise, <c>false</c>.</returns>
        internal static bool ContainsStrategy(this AllocationGroup allocationGroup, int strategyId)
        {
            try
            {
                List<int> strategyList = (from g in allocationGroup.Allocations.Collection
                                          from t in g.Childs.Collection
                                          select t.LevelnID).ToList();

                return strategyList.Contains(strategyId);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Updates the name of the scheme.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static void UpdateSchemeName(this AllocationGroup group)
        {
            try
            {
                if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    group.AllocationSchemeName = PreferenceManager.GetInstance.GetAllocationPreferenceNameById(group.AllocationSchemeID);
                else
                {
                    group.AllocationSchemeID = 0;
                    group.AllocationSchemeName = String.Empty;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Unallocates the group.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static void UnallocateGroup(this AllocationGroup group)
        {
            try
            {
                group.KeepAttributesValues();
                group.UnAllocate();
                group.AllocationCheck();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the default persistence status.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static void SetDefaultPersistenceStatus(this AllocationGroup group)
        {
            try
            {
                group.PersistenceStatus = ApplicationConstants.PersistenceStatus.NotChanged;
                group.RemoveDeletedTaxlotsFromResetDictionary();
                foreach (TaxLot taxlot in group.GetAllTaxlots())
                {
                    taxlot.TaxLotState = ApplicationConstants.TaxLotState.NotChanged;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Creates the un allocated tax lot.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static void CreateUnAllocatedTaxLot(this AllocationGroup group)
        {
            try
            {
                group.TaxLots.Add(CreateUnAllocatedTaxLot(group, group.GroupID));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
