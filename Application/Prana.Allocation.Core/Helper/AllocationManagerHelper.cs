using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Common.Helper;
using Prana.Allocation.Core.CacheStore;
using Prana.Allocation.Core.Extensions;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Prana.Allocation.Core.Helper
{
    internal static class AllocationManagerHelper
    {

        /// <summary>
        /// This method returns groups the list of allocation group objects according to preference
        /// </summary>
        /// <param name="groupList">List of group for which preference is checked</param>
        /// <param name="pref">Preference parameter</param>
        /// <returns>Returns the dictionary in which key will be checklistId and value will be the list of allocationGroup under specified checklist</returns>
        internal static Dictionary<AllocationParameter, List<AllocationGroup>> GetBaseWiseAllocation(List<AllocationGroup> groupList, AllocationOperationPreference pref, int userId, bool doStrategyAllocation, bool isPreview = false)
        {
            try
            {
                object resultLocker = new object();
                Dictionary<AllocationParameter, List<AllocationGroup>> result = new Dictionary<AllocationParameter, List<AllocationGroup>>();
                List<int> preferenceAccounts = new List<int>();
                if (pref.DefaultRule.MatchClosingTransaction == MatchClosingTransactionType.SelectedAccounts)
                    preferenceAccounts = pref.GetSelectedAccountsList();
                AllocationParameter defaultParameter = new AllocationParameter(pref.DefaultRule, pref.TargetPercentage, pref.OperationPreferenceId, userId, doStrategyAllocation, isPreview, preferenceAccounts);
                // Traverse through all the checkLists and find suitable inclusion for it.
                // Avoid exclusion list If not any suitable check list found use default.
                Parallel.ForEach(groupList, group =>
                {
                    AllocationParameter parameter = null;
                    foreach (int checkListId in pref.GetCheckListIds())
                    {
                        // TODO: need to correct this flow. this methods is cloning preference object even if not required
                        CheckListWisePreference checkListPref = pref.GetCheckListPreferenceForId(checkListId);
                        // Check for allowed rules
                        if (checkListPref.IsAllowed(group))
                        {
                            parameter = new AllocationParameter(checkListPref.Rule, checkListPref.TargetPercentage, pref.OperationPreferenceId, userId, doStrategyAllocation, isPreview, preferenceAccounts);
                            break;
                        }
                    }

                    if (parameter == null)
                        parameter = defaultParameter;
                    lock (resultLocker)
                    {
                        // Adding group to checklist collection
                        if (!result.ContainsKey(parameter))
                            result.Add(parameter, new List<AllocationGroup>());
                        result[parameter].Add(group);
                    }

                });

                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Allocates strategy according to strategy percentage
        /// </summary>
        /// <param name="levelClass">Fund value</param>
        /// <param name="list">List of startegy percentage</param>
        /// <param name="groupId">allocation group id</param>
        /// <param name="roundLot">round lot for security</param>
        internal static void AllocateStartegy(ref AllocationLevelClass levelClass, List<StrategyValue> list, string groupId, decimal roundLot, AllocationRule allocationRule, int sideMultiplier)
        {
            try
            {
                bool isSellTrade = sideMultiplier == -1 ? true : false;
                bool isManualQuantity = false;
                Dictionary<int, double> fractionalValues = new Dictionary<int, double>();
                if (allocationRule.BaseType == AllocationBaseType.CumQuantity && allocationRule.RuleType == MatchingRuleType.None && allocationRule.PreferenceAccountId == -1 && allocationRule.MatchClosingTransaction == MatchClosingTransactionType.None)
                {
                    isManualQuantity = true;
                }
                decimal totalQuantity = (decimal)levelClass.AllocatedQty;
                decimal allocatedQty = 0.0M;

                // If fills come after trade allocation, then the strategy list contains older values and then it assumes that to be manually put 
                // and allocates false quantities to taxlots. PRANA-19389
                var sum = list.Sum(x => (decimal)(x.Quantity));
                if (isManualQuantity && totalQuantity - sum != 0)
                {
                    isManualQuantity = false;
                }

                foreach (StrategyValue sVal in list)
                {
                    AllocationLevelClass sClass = new AllocationLevelClass(groupId);
                    sClass.LevelnID = sVal.StrategyId;
                    if (isManualQuantity && sVal.Quantity != 0 && sVal.Quantity % 1 == 0 && roundLot == 1)
                    {
                        sClass.AllocatedQty = (double)sVal.Quantity;
                        sClass.Percentage = (float)(sVal.Value);
                    }
                    else
                    {
                        decimal sClassQty = Convert.ToDecimal(totalQuantity * sVal.Value / 100);
                        sClass.AllocatedQty = Convert.ToDouble(Math.Floor(Math.Abs(sClassQty) / roundLot) * roundLot);
                        sClass.Percentage = (float)((sClass.AllocatedQty * 100) / levelClass.AllocatedQty);
                    }
                    allocatedQty += (decimal)sClass.AllocatedQty;
                    sClass.Name = CachedDataManager.GetInstance.GetStrategyText(sVal.StrategyId);
                    levelClass.AddChilds(sClass);

                    double allocatedQTY = Convert.ToDouble((Math.Abs(sVal.Value) * totalQuantity) / 100);
                    double fracnalPart = (double)(allocatedQTY - Math.Truncate(allocatedQTY)) * sideMultiplier;
                    if (!fractionalValues.ContainsKey(sVal.StrategyId))
                        fractionalValues.Add(sVal.StrategyId, fracnalPart);
                }

                Dictionary<int, double> orderedStrategyValues = new Dictionary<int, double>();
                if (isSellTrade)
                    orderedStrategyValues = fractionalValues.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                else
                    orderedStrategyValues = fractionalValues.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);


                if (allocatedQty < totalQuantity)
                {
                    decimal remainingQuantityToBeAssigned = totalQuantity - allocatedQty;
                    decimal minimumAssignableQuantity = roundLot;

                    //Currently this loop will assign fractional quantity to strategy in order of there existance there is no round robin or paripassu here.
                    foreach (KeyValuePair<int, double> Kvpair in orderedStrategyValues)
                    {
                        StrategyValue sVal = (StrategyValue)list.FirstOrDefault(p => p.StrategyId == Kvpair.Key);
                        if (sVal != null)
                        {
                            decimal quantityToBeInThisLoop = minimumAssignableQuantity;

                            if (remainingQuantityToBeAssigned < quantityToBeInThisLoop)
                                quantityToBeInThisLoop = remainingQuantityToBeAssigned;

                            AllocationLevelClass sClass = levelClass.Childs.GetAllocationLevel(sVal.StrategyId);

                            levelClass.Childs.GetAllocationLevel(sVal.StrategyId).AllocatedQty += (double)quantityToBeInThisLoop;
                            remainingQuantityToBeAssigned -= quantityToBeInThisLoop;
                            levelClass.Childs.GetAllocationLevel(sVal.StrategyId).Percentage = (float)((sClass.AllocatedQty * 100) / levelClass.AllocatedQty);
                            if (remainingQuantityToBeAssigned == 0)
                                break;
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
        /// Updates user cache before and after allocation operation
        /// </summary>
        /// <param name="groups">List of groups on which operation to be done</param>
        /// <param name="userId">User id</param>
        /// <param name="isPreview">is allocation preview</param>
        /// <param name="stateUpdateParameter">-1 if updating before operation, 1 if updating after operation</param>
        internal static void UpdateUserStateCache(List<AllocationGroup> groups, int userId, bool isPreview, int stateUpdateParameter)
        {
            try
            {
                //if user cache need to be updated
                bool isUpdate = (userId != -1 && !isPreview) ? true : false;
                foreach (AllocationGroup group in groups)
                {
                    List<AccountValue> notionalState = new List<AccountValue>();
                    List<AccountValue> cumQty = new List<AccountValue>();

                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        if (taxlot.Level1ID != 0)
                        {
                            decimal value = stateUpdateParameter * (decimal)taxlot.TaxLotQty * Calculations.GetSideMultilpier(group.OrderSideTagValue);
                            AccountValue fvCumQty = new AccountValue(taxlot.Level1ID, value);
                            fvCumQty.StrategyValueList.Add(new StrategyValue(taxlot.Level2ID, value, value));
                            cumQty.Add(fvCumQty);

                            decimal notional = stateUpdateParameter * FormulaStore.NotionalCalculator.GetNotional(group, (decimal)taxlot.TaxLotQty);
                            AccountValue fvNotional = new AccountValue(taxlot.Level1ID, notional);
                            fvNotional.StrategyValueList.Add(new StrategyValue(taxlot.Level2ID, notional, notional));
                            notionalState.Add(fvNotional);
                        }
                    }
                    if (isUpdate)//Updating state based Swap parameters
                        UserWiseStateCache.Instance.UpdateStateForUser(userId, group.AUECLocalDate, CommonHelper.GetSwapSymbol(group.Symbol, group.IsSwapped), cumQty, notionalState, group.OrderSideTagValue);
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
        /// Bifurcates the groups for PTT allocation.
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <param name="groupListPTT">The group list PTT.</param>
        internal static void BifurcateGroupsForPTTAllocation(ref List<AllocationGroup> groupList, List<AllocationGroup> groupListPTT)
        {
            try
            {
                groupListPTT.AddRange(groupList.Where(group => group.OriginalAllocationPreferenceID > 0).ToList());
                if (groupListPTT.Count > 0)
                    groupList = groupList.Where(group => group.OriginalAllocationPreferenceID <= 0).ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Determines whether [is validate check side for groups] [the specified group list].
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="forceAllocation">if set to <c>true</c> [force allocation].</param>
        /// <param name="oldAllocationGroups">The old allocation groups.</param>
        /// <returns>
        ///   <c>true</c> if [is validate check side for groups] [the specified group list]; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsValidateCheckSideForGroups(List<AllocationGroup> groupList, AllocationParameter parameter, bool forceAllocation, List<AllocationGroup> oldAllocationGroups, bool defaultDoCheckSide)
        {
            bool doCheckSide = false;
            try
            {
                //If user id is -1 that is trade is coming from TT Create transaction or drop copy then gets value for to do check side or not.
                if (parameter.UserId == -1)
                {
                    bool getCheckSide = true;
                    if (oldAllocationGroups != null && oldAllocationGroups.Count > 0)
                    {
                        //removing old state for new fill to avoid incorrect allocation as qty is updated everytime
                        //bool resultState = StateCacheStore.Instance.UpdateCache(oldAllocationGroups, -1);
                        getCheckSide = (from g in oldAllocationGroups
                                        where g.ClosingStatus != ClosingStatus.Open || g.GroupStatus != PostTradeEnums.Status.None
                                        select g).ToList().Count == 0;
                    }
                    //Checking if force allocation to done or not. Such as corporate action and cost adjustment check side should not be checked.
                    if (getCheckSide && !forceAllocation)
                        doCheckSide = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("DoCheckSideOnIncomingTrade"));
                    else
                        doCheckSide = false;

                }
                //Assuming if 1st group is unallocated then all groups in group list are unallocated.
                else if (groupList != null && groupList.Count > 0 && groupList[0].State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)// && !parameter.DoStrategyAllocation))
                    doCheckSide = defaultDoCheckSide;
                else
                    doCheckSide = false;

                if (doCheckSide && forceAllocation)
                    doCheckSide = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return doCheckSide;
        }

        /// <summary>
        /// Gets the old allocation groups.
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        internal static List<AllocationGroup> GetOldAllocationGroups(List<AllocationGroup> groupList, int userId)
        {
            List<AllocationGroup> oldAllocationGroups = null;
            try
            {
                if (userId == -1)
                {
                    List<string> groupIdList = (from g in groupList
                                                select g.GroupID).ToList();
                    Expression<Func<AllocationGroup, bool>> predicate = un => groupIdList.Contains(un.GroupID);
                    oldAllocationGroups = AllocationGroupCache.Instance.GetGroups(predicate);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return oldAllocationGroups;
        }

        /// <summary>
        /// Allocates the in accounts.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="allocatedGroups">The allocated groups.</param>
        /// <param name="result">The result.</param>
        internal static void AllocateInAccounts(AllocationParameter parameter, List<AllocationGroup> allocatedGroups, AllocationOutputResult result, bool isReallocatedFromBlotter = false)
        {
            try
            {
                foreach (AllocationGroup group in allocatedGroups)
                {
                    AllocationOutput output = result.GetAllocationOutput(group.GroupID);

                    AllocationLevelList list = new AllocationLevelList();
                    if (output != null && output.AccountValueCollection != null)
                    {
                        foreach (AccountValue val in output.AccountValueCollection)
                        {
                            AllocationLevelClass levelClass = new AllocationLevelClass(output.GroupId);
                            levelClass.LevelnID = val.AccountId;
                            levelClass.AllocatedQty = (double)val.Value;
                            levelClass.Name = CachedDataManager.GetInstance.GetAccountText(val.AccountId);
                            levelClass.Percentage = (float)((levelClass.AllocatedQty * 100) / group.CumQty);
                            if (output.AccountTargetPercentageCollection.ContainsKey(val.AccountId))
                                levelClass.TargetPercentage = output.AccountTargetPercentageCollection[val.AccountId].Value;
                            if (parameter.DoStrategyAllocation)
                            {
                                if (parameter.TargetPercentage.ContainsKey(val.AccountId))
                                {
                                    int sideMultiplier = Calculations.GetSideMultilpier(group.OrderSideTagValue);
                                    AllocateStartegy(ref levelClass, parameter.TargetPercentage[val.AccountId].StrategyValueList, output.GroupId, group.RoundLot, parameter.CheckListWisePreference, sideMultiplier);
                                }
                            }

                            list.Add(levelClass);
                        }

                        group.AllocateGroup(list, isReallocatedFromBlotter);
                    }
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
        /// Gets the target percentage from group allocation collection.
        /// </summary>
        /// <param name="allocationsList">The allocations list.</param>
        /// <returns></returns>
        internal static SerializableDictionary<int, AccountValue> GetTargetPercentageFromGroupAllocationCollection(AllocationLevelList allocationsList)
        {
            SerializableDictionary<int, AccountValue> targetPercentage = new SerializableDictionary<int, AccountValue>();
            try
            {
                if (allocationsList != null)
                {
                    double totalQty = allocationsList.Collection.Sum(x => x.AllocatedQty);
                    foreach (AllocationLevelClass allocations in allocationsList.Collection)
                    {
                        List<StrategyValue> strategies = new List<StrategyValue>();
                        if (allocations.Childs != null && allocations.Childs.Collection.Count > 0)
                            allocations.Childs.Collection.ForEach(s => strategies.Add(new StrategyValue(s.LevelnID, (Convert.ToDecimal(s.AllocatedQty) * 100) / Convert.ToDecimal(allocations.AllocatedQty), 0.0M)));
                        targetPercentage.Add(allocations.LevelnID, new AccountValue(allocations.LevelnID, (Convert.ToDecimal(allocations.AllocatedQty) * 100) / Convert.ToDecimal(totalQty), strategies));
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return targetPercentage;
        }

        /// <summary>
        /// Gets the general rule wise allocation.
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <param name="pref">The preference.</param>
        /// <returns></returns>
        internal static Dictionary<CheckListWisePreference, List<AllocationGroup>> GetGeneralRuleWiseAllocation(List<AllocationGroup> groupList, AllocationOperationPreference pref)
        {
            Dictionary<CheckListWisePreference, List<AllocationGroup>> result = new Dictionary<CheckListWisePreference, List<AllocationGroup>>();
            try
            {
                object resultLocker = new object();
                CheckListWisePreference defaultCheckList = new CheckListWisePreference(int.MinValue, pref.DefaultRule.BaseType, pref.DefaultRule.RuleType, pref.DefaultRule.PreferenceAccountId, pref.DefaultRule.MatchClosingTransaction, pref.DefaultRule.ProrataAccountList, pref.DefaultRule.ProrataDaysBack, pref.TargetPercentage);
                // Traverse through all the checkLists and find suitable inclusion for it.
                // Avoid exclusion list If not any suitable check list found use default.
                Parallel.ForEach(groupList, group =>
                {
                    CheckListWisePreference checkList = null;
                    foreach (int checkListId in pref.GetCheckListIds())
                    {
                        // TODO: need to correct this flow. this methods is cloning preference object even if not required
                        CheckListWisePreference checkListPref = pref.GetCheckListPreferenceForId(checkListId);
                        // Check for allowed rules
                        if (checkListPref.IsAllowed(group))
                        {
                            checkList = checkListPref;
                            break;
                        }
                    }
                    if (checkList == null)
                        checkList = defaultCheckList;
                    lock (resultLocker)
                    {
                        // Adding group to checklist collection
                        if (!result.ContainsKey(checkList))
                            result.Add(checkList, new List<AllocationGroup>());
                        result[checkList].Add(group);
                    }

                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Gets the calculated preference.
        /// </summary>
        /// <param name="generalRuleWiseTargetPercentage">The general rule wise target percentage.</param>
        /// <returns></returns>
        internal static AllocationOperationPreference GetCalculatedPreference(SerializableDictionary<CheckListWisePreference, SerializableDictionary<int, AccountValue>> generalRuleWiseTargetPercentage, int masterFundPreferenceId)
        {
            AllocationOperationPreference pref = new AllocationOperationPreference();
            pref.OperationPreferenceId = masterFundPreferenceId;
            try
            {
                CheckListWisePreference checkListWisePreference = generalRuleWiseTargetPercentage.Keys.FirstOrDefault(x => x.ChecklistId == int.MinValue);
                if (checkListWisePreference == null)
                    checkListWisePreference = generalRuleWiseTargetPercentage.Keys.FirstOrDefault();

                if (checkListWisePreference != null)
                {
                    pref.TryUpdateDefaultRule(checkListWisePreference.Rule);
                    pref.TryUpdateTargetPercentage(generalRuleWiseTargetPercentage[checkListWisePreference]);
                    pref.TryUpdateAccountsList(checkListWisePreference.GetAllocationAccountsList());
                }

                foreach (CheckListWisePreference checkPref in generalRuleWiseTargetPercentage.Keys.Where(x => x.ChecklistId != int.MinValue))
                {
                    checkPref.TryUpdateTargetPercentage(generalRuleWiseTargetPercentage[checkPref]);
                    pref.TryUpdateCheckList(checkPref);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return pref;
        }

        /// <summary>
        /// Gets the allocation scheme key for group.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <param name="allocationSchemeKey">The allocation scheme key.</param>
        /// <returns></returns>
        internal static string GetAllocationSchemeKeyForGroup(AllocationGroup allocationGroup, AllocationSchemeKey allocationSchemeKey)
        {
            string key = string.Empty;
            try
            {
                switch (allocationSchemeKey)
                {
                    case AllocationSchemeKey.Symbol:
                        key = allocationGroup.Symbol;
                        break;
                    case AllocationSchemeKey.SymbolSide:
                    case AllocationSchemeKey.PBSymbolSide:
                        key = allocationGroup.Symbol + Seperators.SEPERATOR_5 + allocationGroup.OrderSideTagValue;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return key;
        }

        /// <summary>
        /// Gets the allocation output.
        /// TODO: Similar code exist in MatchPortfolioPositionHelper.cs and allocation generators. need to centralize this code
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="targetPercentage">The target percentage.</param>
        /// <param name="preferencedAccountId">The preferenced account identifier.</param>
        /// <param name="doCheckSide">if set to <c>true</c> [do check side].</param>
        /// <returns></returns>
        internal static AllocationOutput GetAllocationOutput(AllocationGroup group, SerializableDictionary<int, AccountValue> targetPercentage, int preferencedAccountId, bool doCheckSide = true)
        {
            try
            {
                int sideMultiplier = Calculations.GetSideMultilpier(group.OrderSideTagValue);
                // Deciding based on side multiplier whether it is buy trade or sell
                bool isSellTrade = sideMultiplier == -1 ? true : false;

                AllocationOutput result = new AllocationOutput(group.GroupID);
                // Based on it is sell trade getting value in descending or ascending
                List<AccountValue> toBeAssigend = AllocationProcessHelper.GetNewAssignable((decimal)group.CumQty * sideMultiplier, targetPercentage, null, isSellTrade);

                decimal assignedQuantity = 0.0M;
                foreach (AccountValue val in toBeAssigend)
                {
                    if ((isSellTrade && val.Value < 0.0M) || (!isSellTrade && val.Value > 0.0M))
                    {
                        // Getting new assignable quantity
                        decimal quantityInThisLoop = Math.Floor(Math.Abs(val.Value / group.RoundLot)) * group.RoundLot;
                        if (quantityInThisLoop > (decimal)group.CumQty - assignedQuantity)
                            quantityInThisLoop = (decimal)group.CumQty - assignedQuantity;

                        //skipping account-value object if quantityInThisLoop <= 0
                        if (quantityInThisLoop > 0)
                        {
                            //Adding to result
                            AccountValue fvCurrent = new AccountValue(val.AccountId, quantityInThisLoop);
                            result.Add(fvCurrent);

                            //Keeping track of assigned quantity
                            assignedQuantity += quantityInThisLoop;
                        }
                    }

                }

                // Now logic for remaining quantity for allocation
                if (assignedQuantity < (decimal)group.CumQty)
                {
                    decimal remainingQuantityToBeAssigned = (decimal)group.CumQty - assignedQuantity;

                    // Assigning all remaining quantity to preference account in case if it is assigned and in target percentage cache
                    if (preferencedAccountId != -1 && targetPercentage.ContainsKey(preferencedAccountId))
                    {
                        AccountValue existing = result.GetAccountValueFor(preferencedAccountId);
                        if (existing == null)
                            existing = new AccountValue(preferencedAccountId, remainingQuantityToBeAssigned);
                        else
                            existing.AddValue(remainingQuantityToBeAssigned);

                        result.Add(existing);
                        //Evaluating check side on remaining quantity.
                        return result;


                    }
                    else
                    {
                        // If no preference account is set then will try to minimize the difference by allocating through quantity
                        decimal minimumAssignableQuantity = group.RoundLot;

                        //Getting maximum no of loops required to allocate all remaining quantity
                        int maxLoop = Convert.ToInt32(Math.Ceiling(remainingQuantityToBeAssigned / group.RoundLot));

                        for (int i = 0; i < maxLoop; i++)
                        {
                            List<AccountValue> toBeAssigendRemaining = AllocationProcessHelper.GetNewAssignable(remainingQuantityToBeAssigned * sideMultiplier, targetPercentage, null, isSellTrade);
                            if (toBeAssigendRemaining != null && toBeAssigend.Count > 0)
                            {
                                AccountValue valRemaining = toBeAssigendRemaining[0];
                                decimal quantityToBeInThisLoop = minimumAssignableQuantity;

                                if (remainingQuantityToBeAssigned < quantityToBeInThisLoop)
                                    quantityToBeInThisLoop = remainingQuantityToBeAssigned;


                                //Adding to result
                                AccountValue fvCurrent = result.GetAccountValueFor(valRemaining.AccountId);
                                if (fvCurrent == null)
                                    fvCurrent = new AccountValue(valRemaining.AccountId, quantityToBeInThisLoop);
                                else
                                    fvCurrent.AddValue(quantityToBeInThisLoop);

                                result.Add(fvCurrent);

                                //Keeping track of assigned quantity
                                remainingQuantityToBeAssigned -= quantityToBeInThisLoop;

                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }
    }
}
