using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Common.Enums;
using Prana.Allocation.Common.Interfaces;
using Prana.Allocation.Core.CacheStore;
using Prana.Allocation.Core.Enums;
using Prana.Allocation.Core.Extensions;
using Prana.Allocation.Core.Factories;
using Prana.Allocation.Core.FormulaStore;
using Prana.Allocation.Core.Helper;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.Allocation.Core.Allocator
{
    public abstract class AccountAllocator : IAllocationGenerator
    {
        /// <summary>
        /// The base type
        /// </summary>
        private AllocationBaseType _baseType = AllocationBaseType.CumQuantity;

        /// <summary>
        /// This returns the AllocationBaseType for current implementation
        /// </summary>
        /// <value>
        /// The type of the base.
        /// </value>
        public AllocationBaseType BaseType
        {
            get { return _baseType; }
            set { _baseType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly object resultLockerObject = new object();

        /// <summary>
        /// Generates the specified group list.
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        public AllocationOutputResult Generate(List<AllocationGroup> groupList, AllocationParameter parameter)
        {
            try
            {
                AllocationOutputResult result = new AllocationOutputResult();
                UpdateBaseType();
                //Validates if allocation input parameters are valid.
                result.Error = this.ValidateAllocationParameters(parameter);
                if (!string.IsNullOrWhiteSpace(result.Error))
                {
                    result.AddAllocationFailedSymbols(groupList.Select(x => x.Symbol).Distinct().ToList());
                    return result;
                }

                // Gets comparer for this implementation
                IComparer<AllocationGroup> comparer = ComparerFactory.Instance.GetComparerFor(this.BaseType, false);


                // Gets sorted data from extension method
                Dictionary<string, List<AllocationGroup>> sortedData = this.SortAllocationGroups(groupList, comparer);
                //Date wise sorted allocation group for synbol.
                Dictionary<string, SortedDictionary<DateTime, List<AllocationGroup>>> dateSortedData = this.DateWiseSortAllocationGroups(groupList, comparer);

                string errorGettingNAV = string.Empty;

                if (parameter.CheckListWisePreference.RuleType == MatchingRuleType.ProrataByNAV || parameter.CheckListWisePreference.RuleType == MatchingRuleType.Leveling)
                {
                    errorGettingNAV = UpdateAccountNAV(AllocationLevel.Account, parameter.CheckListWisePreference, groupList);
                }
                Parallel.ForEach(dateSortedData.Keys, symbol =>
                {

                    StringBuilder errorBuilder = new StringBuilder();
                    bool isMatchClosingTransactionVerified = false;
                    Dictionary<int, AccountValue> marketValueStateForSymbol = new Dictionary<int, AccountValue>();

                    //Creating clone object for allocation as percentage can be different for different symbol in prorata.
                    AllocationParameter clonedParameter = DeepCopyHelper.Clone(parameter);
                    SerializableDictionary<string, AllocationOutput> allocationOutputForCurrentSymbol = null;
                    Dictionary<int, AccountValue> currentStateForSymbol = null;
                    string stateNavErrorString = string.Empty;
                    string groupValidationError = this.ValidateAllocationGroups(dateSortedData[symbol].SelectMany(x => x.Value).ToList());
                    if (string.IsNullOrWhiteSpace(groupValidationError))
                    {
                        //https://jira.nirvanasolutions.com:8443/browse/PRANA-33956
                        //if ((dateSortedData[symbol].Values.First().First()) != null && disableChecksideForAssets != null && disableChecksideForAssets.Contains((dateSortedData[symbol].Values.First().First()).AssetID))
                        //    clonedParameter.DoCheckSide = false;

                        switch (clonedParameter.CheckListWisePreference.RuleType)
                        {
                            case MatchingRuleType.Prorata:
                                StringBuilder groupIds = this.GetGroupIdList(dateSortedData[symbol]);
                                currentStateForSymbol = UserWiseStateCache.Instance.GetCurrentStateForDays(clonedParameter.CheckListWisePreference.ProrataDaysBack, clonedParameter.CheckListWisePreference.BaseType, symbol, clonedParameter.UserId, groupIds);

                                SerializableDictionary<int, AccountValue> percentage = null;
                                stateNavErrorString = ProrataHelper.GetPercentageForProrata(clonedParameter.CheckListWisePreference.ProrataAccountList, out percentage, symbol, currentStateForSymbol);
                                if (string.IsNullOrWhiteSpace(stateNavErrorString))
                                {
                                    clonedParameter.UpdatePercentage(percentage);
                                }
                                break;

                            case MatchingRuleType.ProrataByNAV:
                            case MatchingRuleType.Leveling:
                                stateNavErrorString = errorGettingNAV;
                                if (string.IsNullOrWhiteSpace(stateNavErrorString))
                                {
                                    stateNavErrorString = GetTargetPercentage(symbol, clonedParameter, dateSortedData[symbol], ref marketValueStateForSymbol);
                                }
                                break;
                        }
                        if (string.IsNullOrWhiteSpace(errorBuilder.ToString()))
                        {
                            if (clonedParameter.CheckListWisePreference.MatchClosingTransaction != MatchClosingTransactionType.None)
                            {
                                Dictionary<int, AccountValue> state = null;
                                bool matchClosingTransactionAtPortfolioOnly = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("MatchClosingTransactionAtPortfolioOnly"));

                                if (matchClosingTransactionAtPortfolioOnly)
                                {
                                    // Checks is portfolio position is can be made 0
                                    isMatchClosingTransactionVerified = AllocationProcessHelper.IsPortfolioPositionPerfectClosePossible(symbol, clonedParameter.UserId, dateSortedData[symbol].Values.SelectMany(group => group).ToList(), parameter.MatchClosingTransactionAccounts);
                                    if (isMatchClosingTransactionVerified)
                                    {
                                        // Allcoates to make position 0
                                        //Get State At Portfolio level
                                        state = UserWiseStateCache.Instance.GetCurrentState(-1, AllocationBaseType.CumQuantity, symbol, clonedParameter.UserId);
                                        marketValueStateForSymbol = AllocateForMatchClosingTransaction(result, dateSortedData, symbol, errorBuilder, clonedParameter, ref allocationOutputForCurrentSymbol, stateNavErrorString, state);

                                    }
                                }
                                else
                                {
                                    // Checks if combinations of portfolio /account, side, strategy level position is can be made 0

                                    //Getting order side of closing transaction
                                    var orderSideTagValues = dateSortedData[symbol].Values.SelectMany(group => group).ToList().Where(x => CheckSideHelper.GetPositionKey(x.OrderSideTagValue).Equals(GroupPositionType.LongClosing)
                                        || CheckSideHelper.GetPositionKey(x.OrderSideTagValue).Equals(GroupPositionType.ShortClosing)).Select(x => x.OrderSideTagValue).Distinct().ToList();

                                    if (orderSideTagValues != null && orderSideTagValues.Count > 0)
                                    {
                                        int totalGroupsCount = dateSortedData[symbol].Values.SelectMany(group => group).ToList().Count();
                                        isMatchClosingTransactionVerified = AllocationProcessHelper.GetAllocationState(symbol, clonedParameter.UserId, dateSortedData[symbol].Values.SelectMany(group => group).ToList(), parameter.MatchClosingTransactionAccounts, clonedParameter.TargetPercentage, out state);
                                        if (isMatchClosingTransactionVerified)
                                        {
                                            marketValueStateForSymbol = AllocateForMatchClosingTransaction(result, dateSortedData, symbol, errorBuilder, clonedParameter, ref allocationOutputForCurrentSymbol, stateNavErrorString, state);
                                        }
                                        else if (totalGroupsCount > 1)
                                        {
                                            //In case if multiple groups comes for allocation and few of them can be closable using MCT
                                            var clonedData = DeepCopyHelper.Clone(dateSortedData[symbol].Values);

                                            int allocatedGroupsCount = 0;
                                            foreach (var items in clonedData)
                                            {
                                                int index = 0;
                                                foreach (var item in items)
                                                {
                                                    var allocationGroups = new List<AllocationGroup>() { item };
                                                    isMatchClosingTransactionVerified = AllocationProcessHelper.GetAllocationState(symbol, clonedParameter.UserId, allocationGroups, parameter.MatchClosingTransactionAccounts, clonedParameter.TargetPercentage, out state);
                                                    if (isMatchClosingTransactionVerified)
                                                    {
                                                        var custumSortedData = new Dictionary<string, SortedDictionary<DateTime, List<AllocationGroup>>>();
                                                        var sorted = new SortedDictionary<DateTime, List<AllocationGroup>>();
                                                        sorted.Add(item.AUECLocalDate.Date, allocationGroups);
                                                        custumSortedData.Add(symbol, sorted);
                                                        marketValueStateForSymbol = AllocateForMatchClosingTransaction(result, custumSortedData, symbol, errorBuilder, clonedParameter, ref allocationOutputForCurrentSymbol, stateNavErrorString, state);
                                                        if (string.IsNullOrWhiteSpace(errorBuilder.ToString()))
                                                        {
                                                            dateSortedData[symbol][item.AUECLocalDate.Date].RemoveAt(index);
                                                        }
                                                        allocatedGroupsCount++;
                                                    }
                                                    index++;
                                                }
                                            }

                                            //if all groups not fully allocated with MCT then for remaining groups allocation will be done on  account % basis defined in allocation scheme.
                                            if (totalGroupsCount != allocatedGroupsCount)
                                                isMatchClosingTransactionVerified = false;
                                        }
                                    }
                                }
                            }
                            if (!isMatchClosingTransactionVerified)
                            {
                                if (String.IsNullOrWhiteSpace(stateNavErrorString))
                                {
                                    switch (clonedParameter.CheckListWisePreference.RuleType)
                                    {
                                        case MatchingRuleType.Prorata:
                                        case MatchingRuleType.ProrataByNAV:
                                            if (!string.IsNullOrWhiteSpace(stateNavErrorString))
                                                this.SetErrorMessage(result, errorBuilder, symbol, stateNavErrorString);
                                            break;
                                        case MatchingRuleType.Leveling:
                                            SerializableDictionary<int, AccountValue> percentageLevelling = new SerializableDictionary<int, AccountValue>();
                                            clonedParameter.CheckListWisePreference.ProrataAccountList.ForEach(x => { percentageLevelling.Add(x, new AccountValue(x, 100.0M)); });
                                            clonedParameter.UpdatePercentage(percentageLevelling);
                                            clonedParameter.DoCheckSide = true;
                                            break;
                                    }

                                    if (string.IsNullOrWhiteSpace(errorBuilder.ToString()))
                                    {
                                        int keyToFind = this.GetKeyToFindForGettingState(clonedParameter);
                                        // TODO: State is loaded in generator class. Ideally it should come in AllocationParameter object
                                        if (clonedParameter.CheckListWisePreference.RuleType != MatchingRuleType.Prorata)
                                            currentStateForSymbol = UserWiseStateCache.Instance.GetCurrentState(keyToFind, clonedParameter.CheckListWisePreference.BaseType, symbol, clonedParameter.UserId);
                                        // Assigning memory to it in case there are multiple groups to be allocated
                                        // If a memory is not assigned then this object will not flow across the loops of allocation
                                        Dictionary<int, AccountValue> currentQuantityStateForSymbol = UserWiseStateCache.Instance.GetCurrentState(-1, AllocationBaseType.CumQuantity, symbol, clonedParameter.UserId);
                                        if (currentStateForSymbol == null)
                                            currentStateForSymbol = new Dictionary<int, AccountValue>();
                                        if (currentQuantityStateForSymbol == null)
                                            currentQuantityStateForSymbol = new Dictionary<int, AccountValue>();
                                        foreach (DateTime dt in dateSortedData[symbol].Keys)
                                        {
                                            allocationOutputForCurrentSymbol = GetAllocationOutput(dateSortedData[symbol][dt], currentStateForSymbol, currentQuantityStateForSymbol, clonedParameter, ref marketValueStateForSymbol);
                                            lock (resultLockerObject)
                                            {
                                                if (allocationOutputForCurrentSymbol != null)
                                                    result.Add(allocationOutputForCurrentSymbol);
                                                else
                                                {
                                                    this.SetErrorMessage(result, errorBuilder, symbol, string.Format("{1} : Check Side failed for date: {0}", String.Format("{0:MM/dd/yyyy}", dt), symbol));
                                                    break;
                                                }
                                            }
                                        }
                                        if (!result.AllocationFailedSymbols.Contains(symbol) && clonedParameter.CheckListWisePreference.RuleType == MatchingRuleType.Leveling)
                                        {
                                            UpdateLevelingState(symbol, marketValueStateForSymbol);
                                        }
                                    }
                                }
                                else
                                    this.SetErrorMessage(result, errorBuilder, symbol, stateNavErrorString);
                            }
                        }
                    }
                    else
                        this.SetErrorMessage(result, errorBuilder, symbol, groupValidationError);
                    result.AppendError(errorBuilder.ToString());
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
        /// Allocte For Match Closing Transaction
        /// </summary>
        /// <param name="result"></param>
        /// <param name="dateSortedData"></param>
        /// <param name="symbol"></param>
        /// <param name="errorBuilder"></param>
        /// <param name="clonedParameter"></param>
        /// <param name="allocationOutputForCurrentSymbol"></param>
        /// <param name="stateNavErrorString"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private Dictionary<int, AccountValue> AllocateForMatchClosingTransaction(AllocationOutputResult result, Dictionary<string, SortedDictionary<DateTime, List<AllocationGroup>>> dateSortedData, string symbol, StringBuilder errorBuilder, AllocationParameter clonedParameter, ref SerializableDictionary<string, AllocationOutput> allocationOutputForCurrentSymbol, string stateNavErrorString, Dictionary<int, AccountValue> state)
        {
            try
            {
                Dictionary<int, AccountValue> marketValueStateForSymbol = new Dictionary<int, AccountValue>();


                // Allcoates to make position 0
                string matchPositionError = TryMatchPortfolioPosition(symbol, clonedParameter, dateSortedData[symbol], ref allocationOutputForCurrentSymbol, stateNavErrorString, ref marketValueStateForSymbol, state);

                if (!string.IsNullOrWhiteSpace(matchPositionError))
                    this.SetErrorMessage(result, errorBuilder, symbol, matchPositionError);
                if (string.IsNullOrWhiteSpace(errorBuilder.ToString()) && allocationOutputForCurrentSymbol != null && allocationOutputForCurrentSymbol.Count > 0)
                {
                    lock (resultLockerObject)
                    {
                        result.Add(allocationOutputForCurrentSymbol);
                    }
                }
                return marketValueStateForSymbol;
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
        /// Returns alloationOutput object for given list of allocationGroup according to parameters
        /// if there are some groups which are not allocated due to wrong side,we put them on hold and 
        /// these groups will be allocated in the second round
        /// </summary>
        /// <param name="list">List of allocation group which will allocated</param>
        /// <param name="targetPercentage">Target percentage which will be used to allocated</param>
        /// <param name="currentStateForSymbol">Current state will be used to match the target percentage of allocation</param>
        /// <param name="currentQuantityStateForSymbol">Current Quantity state will be used for check side</param>
        /// <param name="preferencedAccountId">The preferenced account identifier.</param>
        /// <param name="allocationParameter">Defines need to consider check side or not.</param>
        /// <returns>AllocationGroupId wise allocation output</returns>
        /// <exception cref="System.Exception">DuplicateID</exception>
        private SerializableDictionary<string, AllocationOutput> GetAllocationOutput(List<AllocationGroup> list, Dictionary<int, AccountValue> currentStateForSymbol, Dictionary<int, AccountValue> currentQuantityStateForSymbol, AllocationParameter allocationParameter, ref Dictionary<int, AccountValue> marketValueStateForSymbol)
        {
            try
            {
                /* 1. Create List of order side tag value in list of groups in allocation
                 * 2. Checks if all accounts have zero state.
                 * 3. if all accounts have zero state and list contains long opening, short opening and short closing then add to hold. vice-versa
                 * 4. if not added to hold state allocate with verifying check side.
                 * 5. if check side fails add to hold state.
                 * 6. When all groups in list are checkes start allocating for hold state. till hold state count changes to 0 or remains unchanged.
                 * 7. if hold state count is greater than 0 after while loop ends then checks if do check side is enabled or not.
                 * 8. If enabled then give error message else allocate forcefully without considering check side.
                 */

                List<string> orderSideList = list.Select(e => e.OrderSideTagValue).Distinct().ToList();
                SerializableDictionary<string, AllocationOutput> result = new SerializableDictionary<string, AllocationOutput>();
                //hold state groups list for all groups for which check side fails.
                List<AllocationGroup> holdState = new List<AllocationGroup>();

                //creating a clone of initial currentQuantityStateForSymbol
                Dictionary<int, AccountValue> clonedCurrentQuantityStateForSymbol1 = new Dictionary<int, AccountValue>();

                foreach (int key in currentQuantityStateForSymbol.Keys)
                {
                    clonedCurrentQuantityStateForSymbol1.Add(key, DeepCopyHelper.Clone(currentQuantityStateForSymbol[key]));
                }

                bool hasAllAccountsWithZeroState = (from q in currentQuantityStateForSymbol
                                                    where allocationParameter.TargetPercentage.Keys.Contains(q.Key) && q.Value.Value != 0
                                                    select q.Key).Count() == 0;
                double sumLongOpening = (from ag in list
                                         where ag.OrderSideTagValue == FIXConstants.SIDE_Buy || ag.OrderSideTagValue == FIXConstants.SIDE_Buy_Open
                                         select ag.CumQty).Sum();

                double sumLongClosing = (from ag in list
                                         where ag.OrderSideTagValue == FIXConstants.SIDE_Sell || ag.OrderSideTagValue == FIXConstants.SIDE_Sell_Closed
                                         select ag.CumQty).Sum();

                double sumShortOpening = (from ag in list
                                          where ag.OrderSideTagValue == FIXConstants.SIDE_SellShort || ag.OrderSideTagValue == FIXConstants.SIDE_Sell_Open
                                          select ag.CumQty).Sum();

                double sumShortClosing = (from ag in list
                                          where ag.OrderSideTagValue == FIXConstants.SIDE_Buy_Cover || ag.OrderSideTagValue == FIXConstants.SIDE_Buy_Closed
                                          select ag.CumQty).Sum();

                bool longAllowed = sumLongOpening == sumLongClosing;
                bool shortAllowed = sumShortOpening == sumShortClosing;

                foreach (AllocationGroup group in list)
                {
                    if (hasAllAccountsWithZeroState)
                    {

                        if ((group.OrderSideTagValue == FIXConstants.SIDE_Buy || group.OrderSideTagValue == FIXConstants.SIDE_Buy_Open) && !longAllowed)
                        {
                            holdState.Add(group);
                            continue;
                        }
                        else if ((group.OrderSideTagValue == FIXConstants.SIDE_SellShort || group.OrderSideTagValue == FIXConstants.SIDE_Sell_Open) && !shortAllowed)
                        {
                            holdState.Add(group);
                            continue;
                        }

                    }

                    if (result.ContainsKey(group.GroupID))
                        throw new Exception("DuplicateID");

                    AllocationOutput outputForThisAllocationGroup = GetAllocationOutput(group, currentStateForSymbol, allocationParameter, ref marketValueStateForSymbol, currentQuantityStateForSymbol);
                    outputForThisAllocationGroup.AccountTargetPercentageCollection = (SerializableDictionary<int, AccountValue>)allocationParameter.TargetPercentage.Clone();

                    //if check side fails for allocation group then add in hold state else add in result.
                    if (!outputForThisAllocationGroup.CheckSideViolated)
                        result.Add(group.GroupID, outputForThisAllocationGroup);
                    else
                        holdState.Add(group);
                }

                // if hold state count is greater than 0 then try to allocate groups again.
                bool stopAllocate = holdState.Count <= 0 ? true : false;
                List<AllocationGroup> newHoldState;
                while (!stopAllocate)
                {
                    newHoldState = new List<AllocationGroup>(holdState);
                    int startCount = newHoldState.Count;
                    foreach (AllocationGroup group in newHoldState)
                    {
                        AllocationOutput outputForThisAllocationGroup = GetAllocationOutput(group, currentStateForSymbol, allocationParameter, ref marketValueStateForSymbol, currentQuantityStateForSymbol);
                        outputForThisAllocationGroup.AccountTargetPercentageCollection = (SerializableDictionary<int, AccountValue>)allocationParameter.TargetPercentage.Clone();

                        if (!outputForThisAllocationGroup.CheckSideViolated)
                        {
                            result.Add(group.GroupID, outputForThisAllocationGroup);
                            holdState.Remove(group);
                        }
                    }

                    int endCount = holdState.Count;
                    //if starting count and ending count are equal or end count is 0 then break else try again.
                    stopAllocate = endCount == 0 || startCount == endCount ? true : false;

                }
                //if hold state count is greater than depending on check side setting allocates.
                //if check side is enable then stop allocation else allocate forcefully.
                if (holdState.Count > 0)
                {
                    //var doCheckSideForLeveling = true;
                    //if (doCheckSideForLeveling)
                    //{
                    //checks if all groups in hold state can be allocated without violating checkside, then force allocation
                    bool forceAllocation = CheckSideHelper.IsForceAllocation(holdState, list, allocationParameter.TargetPercentage, currentQuantityStateForSymbol, clonedCurrentQuantityStateForSymbol1, result);
                    if (forceAllocation)
                    {
                        newHoldState = new List<AllocationGroup>(holdState);

                        foreach (AllocationGroup group in newHoldState)
                        {
                            AllocationOutput outputForThisAllocationGroup = GetAllocationOutput(group, currentStateForSymbol, allocationParameter, ref marketValueStateForSymbol, currentQuantityStateForSymbol, false);
                            outputForThisAllocationGroup.AccountTargetPercentageCollection = (SerializableDictionary<int, AccountValue>)allocationParameter.TargetPercentage.Clone();
                            if (!outputForThisAllocationGroup.CheckSideViolated)
                            {
                                result.Add(group.GroupID, outputForThisAllocationGroup);
                                holdState.Remove(group);
                            }
                        }
                    }
                    else
                    {
                        newHoldState = new List<AllocationGroup>(holdState);
                        foreach (AllocationGroup group in newHoldState)
                        {
                            AllocationOutput outputForThisAllocationGroup = GetAllocationOutput(group, currentStateForSymbol, allocationParameter, ref marketValueStateForSymbol, currentQuantityStateForSymbol, false);
                            outputForThisAllocationGroup.AccountTargetPercentageCollection = (SerializableDictionary<int, AccountValue>)allocationParameter.TargetPercentage.Clone();

                            if (!outputForThisAllocationGroup.CheckSideViolated)
                            {
                                result.Add(group.GroupID, outputForThisAllocationGroup);
                                holdState.Remove(group);
                            }
                            else if (group.IsOverbuyOversellAccepted)
                            {
                                result.Add(group.GroupID, outputForThisAllocationGroup);
                                holdState.Remove(group);
                            }
                        }
                    }

                    if (holdState.Count > 0)
                        return null;
                    //}
                    //else
                    //{
                    //    newHoldState = new List<AllocationGroup>(holdState);

                    //    foreach (AllocationGroup group in newHoldState)
                    //    {
                    //        AllocationOutput outputForThisAllocationGroup = GetAllocationOutput(group, currentStateForSymbol, allocationParameter, ref marketValueStateForSymbol, currentQuantityStateForSymbol);
                    //        outputForThisAllocationGroup.AccountTargetPercentageCollection = (SerializableDictionary<int, AccountValue>)allocationParameter.TargetPercentage.Clone();

                    //        if (!outputForThisAllocationGroup.CheckSideViolated)
                    //        {
                    //            result.Add(group.GroupID, outputForThisAllocationGroup);
                    //            holdState.Remove(group);
                    //        }
                    //    }
                    //}
                }
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
        /// Returns AllocationOutput for given AllocationGroup
        /// </summary>
        /// <param name="group">AllocationGroup which will be allocated</param>
        /// <param name="targetPercentage">TargetPercentage to achieve</param>
        /// <param name="currentStateForSymbol">Current state for the given symbol</param>
        /// <param name="preferencedAccountId">The preferenced account identifier.</param>
        /// <returns>AllcoationOutout object for given AllocationGroup after allocation</returns>
        private AllocationOutput GetAllocationOutput(AllocationGroup group, Dictionary<int, AccountValue> currentStateForSymbol, AllocationParameter allocationParameter, ref Dictionary<int, AccountValue> marketValueStateForSymbol, Dictionary<int, AccountValue> currentQuantityStateForSymbol = null, bool doForceCheckSide = true)
        {
            try
            {
                var targetPercentage = allocationParameter.TargetPercentage;
                var ruleType = allocationParameter.CheckListWisePreference.RuleType;
                var preferencedAccountId = allocationParameter.CheckListWisePreference.PreferenceAccountId;

                if (ruleType == MatchingRuleType.Leveling)
                {
                    targetPercentage.Clear();

                    // calculate target percentage for leveling on basis of account nav, account wise market value for symbol and simulated market value for group, PRANA-26333
                    targetPercentage = GetPercentageForLeveling(group, marketValueStateForSymbol);
                }

                int sideMultiplier = Calculations.GetSideMultilpier(group.OrderSideTagValue);
                // Deciding based on side multiplier whether it is buy trade or sell
                bool isSellTrade = sideMultiplier == -1 ? true : false;

                //Creating cloned copy af state so, that state can be updated after allocation completes.
                //To avoid partial state updation.
                if (currentStateForSymbol == null)
                    currentStateForSymbol = new Dictionary<int, AccountValue>();
                // used DeepCopyHelper to clone current state dictionary,PRANA-10104
                Dictionary<int, AccountValue> clonedCurrentStateForSymbol = new Dictionary<int, AccountValue>();
                clonedCurrentStateForSymbol = DeepCopyHelper.Clone(currentStateForSymbol);
                Dictionary<int, AccountValue> clonedCurrentQuantityStateForSymbol = new Dictionary<int, AccountValue>();
                clonedCurrentQuantityStateForSymbol = DeepCopyHelper.Clone(currentQuantityStateForSymbol);

                AllocationOutput result = new AllocationOutput(group.GroupID);
                // Based on it is sell trade getting value in descending or ascending
                decimal totalValue = (BaseType == AllocationBaseType.Notional) ? NotionalCalculator.GetNotional(group, (decimal)group.CumQty) : (decimal)group.CumQty * sideMultiplier;
                List<AccountValue> toBeAssigend = AllocationProcessHelper.GetNewAssignable(totalValue, targetPercentage, (ruleType == MatchingRuleType.Leveling) ? null : clonedCurrentStateForSymbol, isSellTrade);

                decimal assignedQuantity = 0.0M;
                foreach (AccountValue val in toBeAssigend)
                {
                    if ((isSellTrade && val.Value < 0.0M) || (!isSellTrade && val.Value > 0.0M))
                    {
                        // Getting new assignable quantity
                        decimal quantity = (BaseType == AllocationBaseType.Notional) ? NotionalCalculator.ReverseCalculateQuantity(group, val.Value) : val.Value;
                        decimal quantityInThisLoop = Math.Floor(Math.Abs(quantity / group.RoundLot)) * group.RoundLot;
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
                            decimal notionalInThisLoop = NotionalCalculator.GetNotional(group, quantityInThisLoop);
                            if (!AllocationProcessHelper.UpdateCurrentState(BaseType, fvCurrent, group, ref clonedCurrentStateForSymbol, sideMultiplier, quantityInThisLoop, allocationParameter, doForceCheckSide, notionalInThisLoop, clonedCurrentQuantityStateForSymbol))
                            {
                                result.CheckSideViolated = true;
                                return result;
                            }
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
                        decimal remainingNotionalToBeAssigned = NotionalCalculator.GetNotional(group, remainingQuantityToBeAssigned);
                        //Evaluating check side on remaining quantity.
                        AccountValue fvRemaining = new AccountValue(existing.AccountId, remainingQuantityToBeAssigned);
                        if (!AllocationProcessHelper.UpdateCurrentState(BaseType, fvRemaining, group, ref clonedCurrentStateForSymbol, sideMultiplier, remainingQuantityToBeAssigned, allocationParameter, doForceCheckSide, remainingNotionalToBeAssigned, clonedCurrentQuantityStateForSymbol))
                        {
                            result.CheckSideViolated = true;
                            return result;
                        }


                    }
                    else
                    {
                        // If no preference account is set then will try to minimize the difference by allocating through quantity/notional
                        decimal minimumAssignableQuantity = group.RoundLot;
                        decimal remainigNotionalToBeAssigned = NotionalCalculator.GetNotional(group, remainingQuantityToBeAssigned);

                        //Getting maximum no of loops required to allocate all remaining quantity
                        int maxLoop = Convert.ToInt32(Math.Ceiling(remainingQuantityToBeAssigned / group.RoundLot));

                        for (int i = 0; i < maxLoop; i++)
                        {
                            List<AccountValue> toBeAssigendRemaining = AllocationProcessHelper.GetNewAssignable((ruleType == MatchingRuleType.Leveling) ? totalValue : ((BaseType == AllocationBaseType.CumQuantity) ? remainingQuantityToBeAssigned * sideMultiplier : remainigNotionalToBeAssigned), targetPercentage, (ruleType == MatchingRuleType.Leveling) ? null : clonedCurrentStateForSymbol, isSellTrade);
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

                                decimal notionalToBeInThisLoop = NotionalCalculator.GetNotional(group, quantityToBeInThisLoop);

                                AccountValue fvRemaining = new AccountValue(valRemaining.AccountId, quantityToBeInThisLoop);
                                if (!AllocationProcessHelper.UpdateCurrentState(BaseType, fvRemaining, group, ref clonedCurrentStateForSymbol, sideMultiplier, quantityToBeInThisLoop, allocationParameter, doForceCheckSide, notionalToBeInThisLoop, clonedCurrentQuantityStateForSymbol))
                                {
                                    result.CheckSideViolated = true;
                                    return result;
                                }
                                //Keeping track of assigned quantity
                                remainingQuantityToBeAssigned -= quantityToBeInThisLoop;
                                remainigNotionalToBeAssigned -= notionalToBeInThisLoop;


                            }
                        }
                    }
                }

                //updating state from cloned state.
                foreach (int key in clonedCurrentStateForSymbol.Keys)
                {
                    if (currentStateForSymbol.ContainsKey(key))
                        currentStateForSymbol.Remove(key);
                    currentStateForSymbol.Add(key, clonedCurrentStateForSymbol[key].Clone());
                }

                //updating state from cloned state.
                foreach (int key in clonedCurrentQuantityStateForSymbol.Keys)
                {
                    if (currentQuantityStateForSymbol.ContainsKey(key))
                        currentQuantityStateForSymbol.Remove(key);
                    currentQuantityStateForSymbol.Add(key, clonedCurrentQuantityStateForSymbol[key].Clone());
                }
                if (ruleType == MatchingRuleType.Leveling && !result.CheckSideViolated)
                {
                    if (result.AccountValueCollection != null && result.AccountValueCollection.Count > 0)
                    {
                        //update account wise market value state for symbol in case of successful allocation, PRANA-26333
                        UpdateAccountValueStateForSymbol(group, marketValueStateForSymbol, result);
                    }
                }
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
        /// Updates the type of the base.
        /// </summary>
        public abstract void UpdateBaseType();

        /// <summary>
        /// Updates the account nav.
        /// </summary>
        /// <param name="allocationLevel">The allocation level.</param>
        /// <param name="rule">The rule.</param>
        /// <param name="groupList">The group list.</param>
        /// <returns></returns>
        public abstract string UpdateAccountNAV(AllocationLevel allocationLevel, AllocationRule rule, List<AllocationGroup> groupList);

        /// <summary>
        /// Tries the match portfolio position.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="clonedParameter">The cloned parameter.</param>
        /// <param name="sortedDictionary">The sorted dictionary.</param>
        /// <param name="allocationOutputForCurrentSymbol">The allocation output for current symbol.</param>
        /// <param name="stateNavErrorString">The state nav error string.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        /// <returns></returns>
        public abstract string TryMatchPortfolioPosition(string symbol, AllocationParameter clonedParameter, SortedDictionary<DateTime, List<AllocationGroup>> sortedDictionary, ref SerializableDictionary<string, AllocationOutput> allocationOutputForCurrentSymbol, string stateNavErrorString, ref Dictionary<int, AccountValue> marketValueStateForSymbol, Dictionary<int, AccountValue> currentAllocationState);

        /// <summary>
        /// Gets the target percentage.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="clonedParameter">The cloned parameter.</param>
        /// <param name="sortedDictionary">The sorted dictionary.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        /// <returns></returns>
        public abstract string GetTargetPercentage(string symbol, AllocationParameter clonedParameter, SortedDictionary<DateTime, List<AllocationGroup>> sortedDictionary, ref Dictionary<int, AccountValue> marketValueStateForSymbol);

        /// <summary>
        /// Updates the state of the leveling.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        public abstract void UpdateLevelingState(string symbol, Dictionary<int, AccountValue> marketValueStateForSymbol);

        /// <summary>
        /// Gets the percentage for leveling.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        /// <returns></returns>
        public abstract SerializableDictionary<int, AccountValue> GetPercentageForLeveling(AllocationGroup group, Dictionary<int, AccountValue> marketValueStateForSymbol);

        /// <summary>
        /// Updates the account value state for symbol.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        /// <param name="result">The result.</param>
        public abstract void UpdateAccountValueStateForSymbol(AllocationGroup group, Dictionary<int, AccountValue> marketValueStateForSymbol, AllocationOutput result);


    }
}
