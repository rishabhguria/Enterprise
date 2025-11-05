using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Core.Enums;
using Prana.Allocation.Core.Managers;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prana.Allocation.Core.Helper
{
    internal class CheckSideHelper
    {

        /// <summary>
        /// validates Allocation output for group
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="account"></param>
        /// <param name="group"></param>
        /// <param name="currentStateCache"></param>
        /// <returns></returns>
        internal static bool ValidateAllocationOutput(AccountValue account, AllocationGroup group, Dictionary<int, AccountValue> currentStateCache)
        {
            try
            {
                //Gets position type for group
                //Order side for all orders in group is same
                GroupPositionType positionType = GetPositionKey(group.OrderSideTagValue);
                int accountId = account.AccountId;
                //Checks when state is available for symbol in that account
                if (currentStateCache.ContainsKey(accountId))
                {
                    //Checks for allowed sides for the state available
                    if (!CheckSide(currentStateCache[accountId].Value, group.OrderSideTagValue))
                    {
                        //if position is greater than 0 and position type is long closing and current qty is less then throw error
                        if (currentStateCache[accountId].Value > 0 && positionType.Equals(GroupPositionType.LongClosing))
                        {
                            if (currentStateCache[accountId].Value < Math.Abs(account.Value))
                                return false;
                        }
                        //Stops short opening and short closing if value is greater than 0
                        else if (currentStateCache[accountId].Value > 0 &&  !positionType.Equals(GroupPositionType.LongClosing) && !positionType.Equals(GroupPositionType.LongOpening))
                        {
                            return false;
                        }

                        if (currentStateCache[accountId].Value < 0 && positionType.Equals(GroupPositionType.ShortClosing))
                        {
                            if (Math.Abs(currentStateCache[accountId].Value) < account.Value)
                                return false;
                        }
                        else if (currentStateCache[accountId].Value < 0 && !positionType.Equals(GroupPositionType.ShortClosing) && !positionType.Equals(GroupPositionType.ShortOpening))
                        {
                            return false;
                        }

                        // if position is zero than stops long closing and short closing
                        if (currentStateCache[accountId].Value == 0 && (positionType.Equals(GroupPositionType.LongClosing) || positionType.Equals(GroupPositionType.ShortClosing)))
                            return false;
                    }
                }
                else
                {
                    //if position not available than stop closing sides
                    if (positionType.Equals(GroupPositionType.LongClosing) || positionType.Equals(GroupPositionType.ShortClosing))
                        return false;
                }
                return true;
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
        /// Get Opening Order Side TagValue
        /// </summary>
        /// <param name="orderSideTagValue"></param>
        /// <returns></returns>
        internal static string GetOpeningOrderSideTagValue(string orderSideTagValue)
        {
            if (orderSideTagValue.Equals(FIXConstants.SIDE_Sell))
                return FIXConstants.SIDE_Buy;
            if (orderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed))
                return FIXConstants.SIDE_SellShort;
            if (orderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover))
                return FIXConstants.SIDE_SellShort;
            if (orderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                return FIXConstants.SIDE_Buy_Open;
            else
                return string.Empty;
        }

        /// <summary>
        /// Returns common fields in error message
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        internal static string GetGroupFields(List<AllocationGroup> groupList)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                foreach (AllocationGroup group in groupList)
                {
                    builder.Append(" Symbol: ");
                    builder.Append(group.Symbol);
                    builder.Append(", Avg Price: ");
                    builder.Append(group.AvgPrice);
                    builder.Append(", Quantity: ");
                    builder.Append(group.CumQty);
                    builder.Append(", Order Side: ");
                    builder.AppendLine(group.OrderSide);
                }
                return builder.ToString();
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
                return "";
            }
        }

        /// <summary>
        /// Checks for side on the basis of current position in account
        /// </summary>
        /// <param name="currentQty">Position in account</param>
        /// <param name="positionTagValue">order side</param>
        /// <returns>true if side is same as current position</returns>
        internal static bool CheckSide(decimal currentQty, string positionTagValue)
        {
            try
            {
                if (currentQty > 0 && !positionTagValue.Equals(FIXConstants.SIDE_Buy)
                                       && !positionTagValue.Equals(FIXConstants.SIDE_Buy_Open))
                    return false;
                else if (currentQty < 0 && !positionTagValue.Equals(FIXConstants.SIDE_SellShort)
                                       && !positionTagValue.Equals(FIXConstants.SIDE_Sell_Open))
                    return false;

                else if (currentQty == 0 && !positionTagValue.Equals(FIXConstants.SIDE_SellShort)
                                         && !positionTagValue.Equals(FIXConstants.SIDE_Sell_Open)
                                         && !positionTagValue.Equals(FIXConstants.SIDE_Buy)
                                         && !positionTagValue.Equals(FIXConstants.SIDE_Buy_Open))
                    return false;

                return true;
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
                return true;
            }
        }

        /// <summary>
        /// Get position type on the basis of side tag
        /// </summary>
        /// <param name="sideTagValue">side tag value</param>
        /// <returns>Position Type</returns>
        internal static GroupPositionType GetPositionKey(string sideTagValue)
        {

            try
            {
                switch (sideTagValue)
                {
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Buy_Open:
                        return GroupPositionType.LongOpening;
                    case FIXConstants.SIDE_Buy_Closed:
                    case FIXConstants.SIDE_Buy_Cover:
                        return GroupPositionType.ShortClosing;
                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_Sell_Closed:
                        return GroupPositionType.LongClosing;
                    case FIXConstants.SIDE_SellShort:
                    case FIXConstants.SIDE_Sell_Open:
                        return GroupPositionType.ShortOpening;
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
            return GroupPositionType.None;
        }

        /// <summary>
        /// Checks if the groups in allocation holdstate can be allocated while balancing opening and closing with each other and
        /// currentQuantityStateForSymbol is reaching a state where all accounts with zero state during allocation of other groups
        /// then it returns true to froce allocation of groups in holdstate
        /// </summary>
        /// <param name="holdState">List of allocation group which are in hold state</param>
        /// <param name="list">List of allocation group which will be allocated</param>
        /// <param name="targetPercentage">Target percentage which will be used to allocated</param>
        /// <param name="currentQuantityStateForSymbol">Current Quantity state will be used for check side</param>
        /// <param name="clonedCurrentQuantityStateForSymbol">Starting Quantity state ,i.e. before allocating any groups</param>
        /// <param name="result">true if holdstate can be completely allocated, false otherwise</param>
        /// <returns></returns>
        internal static bool IsForceAllocation(List<AllocationGroup> holdState, List<AllocationGroup> list, SerializableDictionary<int, AccountValue> targetPercentage, Dictionary<int, AccountValue> currentQuantityStateForSymbol, Dictionary<int, AccountValue> clonedCurrentQuantityStateForSymbol, SerializableDictionary<string, AllocationOutput> result)
        {
            try
            {
                decimal totalAccountValue = (from q in currentQuantityStateForSymbol
                                             where targetPercentage.Keys.Contains(q.Key)
                                             select q.Value.Value).Sum();

                decimal initialShortOpening = (from q in clonedCurrentQuantityStateForSymbol
                                               where targetPercentage.Keys.Contains(q.Key) && q.Value.Value < 0
                                               select q.Value.Value).Sum();

                decimal initialLongOpening = (from q in clonedCurrentQuantityStateForSymbol
                                              where targetPercentage.Keys.Contains(q.Key) && q.Value.Value > 0
                                              select q.Value.Value).Sum();

                double sumLongOpening = (from ag in holdState
                                         where ag.OrderSideTagValue == FIXConstants.SIDE_Buy || ag.OrderSideTagValue == FIXConstants.SIDE_Buy_Open
                                         select ag.CumQty).Sum();

                double sumLongClosing = (from ag in holdState
                                         where ag.OrderSideTagValue == FIXConstants.SIDE_Sell || ag.OrderSideTagValue == FIXConstants.SIDE_Sell_Closed
                                         select ag.CumQty).Sum();

                double sumShortOpening = (from ag in holdState
                                          where ag.OrderSideTagValue == FIXConstants.SIDE_SellShort || ag.OrderSideTagValue == FIXConstants.SIDE_Sell_Open
                                          select ag.CumQty).Sum();

                double sumShortClosing = (from ag in holdState
                                          where ag.OrderSideTagValue == FIXConstants.SIDE_Buy_Cover || ag.OrderSideTagValue == FIXConstants.SIDE_Buy_Closed
                                          select ag.CumQty).Sum();

                if (initialLongOpening != 0 && !list.Where(x => x.OrderSideTagValue == FIXConstants.SIDE_Sell || x.OrderSideTagValue == FIXConstants.SIDE_Sell_Closed).Any())
                    return false;
                else if (initialShortOpening != 0 && !list.Where(x => x.OrderSideTagValue == FIXConstants.SIDE_Buy_Cover || x.OrderSideTagValue == FIXConstants.SIDE_Buy_Closed).Any())
                    return false;
                else if (sumLongClosing == sumLongOpening && sumShortClosing == 0 && sumShortOpening == 0)
                {
                    foreach (int key in clonedCurrentQuantityStateForSymbol.Where(x => x.Value.Value < 0).Select(x => x.Key))
                    {
                        var value = (from r in result
                                     join ag in list on r.Value.GroupId equals ag.GroupID
                                     where ag.OrderSideTagValue == FIXConstants.SIDE_Buy_Cover || ag.OrderSideTagValue == FIXConstants.SIDE_Buy_Closed
                                     select r.Value.AccountValueCollection.Sum(x => x.Value)).ToList();

                        if ((decimal)Math.Abs(value.Sum()) < Math.Abs(clonedCurrentQuantityStateForSymbol[key].Value))
                            return false;
                    }
                    return true;
                }
                else if (sumShortClosing == sumShortOpening && sumLongClosing == 0 && sumLongOpening == 0)
                {
                    foreach (int key in clonedCurrentQuantityStateForSymbol.Where(x => x.Value.Value > 0).Select(x => x.Key))
                    {
                        var value = (from r in result
                                     join ag in list on r.Value.GroupId equals ag.GroupID
                                     where ag.OrderSideTagValue == FIXConstants.SIDE_Sell || ag.OrderSideTagValue == FIXConstants.SIDE_Sell_Closed
                                     select r.Value.AccountValueCollection.Sum(x => x.Value)).ToList();

                        if ((decimal)Math.Abs(value.Sum()) < Math.Abs(clonedCurrentQuantityStateForSymbol[key].Value))
                            return false;
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// getting do Check-side or ignore it
        /// </summary>
        /// <param name="group"></param>
        /// <param name="AccountId"></param>
        /// <param name="matchingRuleType"></param>
        /// <returns></returns>
        internal static bool DoCheckSide(AllocationGroup group, int AccountId, AllocationParameter parameter)
        {
            bool doCheckSide = true;
            try
            {

                // matchingRuleType is leveling then it will always check of side. 
                if (parameter.CheckListWisePreference.RuleType.Equals(MatchingRuleType.Leveling))
                    doCheckSide = true;

                //Get allocation Preferences for check-side from cache
                AllocationCompanyWisePref allocationPref = PreferenceManager.GetInstance.GetCompanyWisePreference(CommonDataCache.CachedDataManager.GetInstance.GetCompanyID());

                //Check validate checkside or not
                if (allocationPref != null && allocationPref.AllocationCheckSidePref != null)
                {

                    if (!allocationPref.AllocationCheckSidePref.DoCheckSideSystem)
                        doCheckSide = false;

                    //if (parameter.UserId == -1)
                    //{
                    //Checking for disable check-side condition
                    //Any Condition of disable checkside matched, then it will not check the side.

                    var levelWiseDisableCheckSidePref = allocationPref.AllocationCheckSidePref.DisableCheckSidePref;
                    foreach (KeyValuePair<OrderFilterLevels, List<int>> item in levelWiseDisableCheckSidePref)
                    {
                        var values = item.Value;
                        switch (item.Key)
                        {
                            case OrderFilterLevels.Asset:
                                if (values.Contains(group.AssetID))
                                    doCheckSide = false;
                                break;

                            case OrderFilterLevels.AUEC:
                                if (values.Contains(group.AUECID))
                                    doCheckSide = false;
                                break;

                            case OrderFilterLevels.MasterFund:
                                var masterFundId = CommonDataCache.CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(AccountId);
                                if (values.Contains(masterFundId))
                                    doCheckSide = false;
                                break;

                            case OrderFilterLevels.Account:
                                if (values.Contains(AccountId))
                                    doCheckSide = false;
                                break;

                            case OrderFilterLevels.CounterParty:
                                if (values.Contains(group.CounterPartyID))
                                    doCheckSide = false;
                                break;

                        }
                        if (!doCheckSide)
                        {
                            break;
                        }
                    }
                }
                //}
                //else if (group != null && group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                //    doCheckSide = allocationPref.AllocationCheckSidePref.DoCheckSideSystem;
                //else
                //    doCheckSide = false;

                if (doCheckSide && parameter.ForceAllocation)
                    doCheckSide = false;

            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
            return doCheckSide;
        }
    }
}
