using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.TradeManager.Extension
{
    public class AllocationPreferenceOperationHelper
    {
        private readonly IAllocationManager allocation;
        public AllocationPreferenceOperationHelper(IAllocationManager allocation)
        {
            this.allocation = allocation ?? throw new NullReferenceException("IAllocationManager cannot be null");
        }

        //this allocation pref will contains allocationOpePref, that would contain, account, mf pref.
        public AllocationOperationPreference CreateAllocationPreference(
          int AllocationPrefID,
          PTTRequestObject pttRequestObject,
          List<PTTResponseObject> pttResponseObjectsList)
        {
            AllocationOperationPreference allocationOperationPreference = null;
            try
            {
                List<AccountValue> accountValues = new List<AccountValue>();
                foreach (PTTResponseObject item in pttResponseObjectsList)
                {
                    accountValues.Add(new AccountValue(item.AccountId, item.PercentageAllocation));
                }
                int selectedFundType = Convert.ToInt32(pttRequestObject.MasterFundOrAccount.Value);
                string prefNamePrefix = selectedFundType == (int)PSTMasterFundOrAccount.CalculatedPreference ? "*Custom#_" : "*PTT#_";
                string prefName = prefNamePrefix + pttRequestObject.TickerSymbol + "_" + "_" + DateTime.Now.ToString("yyMMddHHmmssff");

                AllocationOperationPreference tempAllocationOperationPreference = allocation.GetPreferenceById(AllocationPrefID);
                if (tempAllocationOperationPreference != null)
                {
                    allocation.DeletePreference(AllocationPrefID, AllocationPreferencesType.CalculatedAllocationPreference);
                }

                var transactionType = Convert.ToInt32(pttRequestObject.AddOrSet.Value) == ((int)PTTChangeType.Set) && pttRequestObject.Target == 0 ? MatchClosingTransactionType.SelectedAccounts : MatchClosingTransactionType.None;
                //TTHelperManager.GetInstance().AllocationManager = ServiceManager.Instance.AllocationManager.InnerChannel;
                var error = CreateAllocationOperationPreference(accountValues, ref allocationOperationPreference, prefName, transactionType);
                if (string.IsNullOrEmpty(error))
                    AllocationPrefID = allocationOperationPreference.OperationPreferenceId;
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
            return allocationOperationPreference;
        }

        //accountValues: will be acntId and allocaiton%, allopereernce: id create, prefName: #PPT+sym+tick, tractionType none
        //
        private string CreateAllocationOperationPreference(
            List<AccountValue> accountValues,
            ref AllocationOperationPreference allocationPreference,
            string prefName, MatchClosingTransactionType transactionType)
        {
            try
            {
                SerializableDictionary<int, AccountValue> targetPercs = new SerializableDictionary<int, AccountValue>();
                foreach (var accountValue in accountValues)
                {
                    if (!targetPercs.ContainsKey(accountValue.AccountId))
                    {
                        // adding strategy with 0 qty as we do not have strategy wise qty here and also mot using it.
                        accountValue.StrategyValueList.Add(new StrategyValue(0, 100, 0));
                        targetPercs.Add(accountValue.AccountId, accountValue);
                    }
                }

                if (allocationPreference == null)
                {
                    if (allocation == null)
                    {
                        return "Allocation template can not be created";
                    }
                    PreferenceUpdateResult preferenceUpdateResult = allocation.AddPreference(prefName, CachedDataManager.GetInstance.GetCompanyID(), AllocationPreferencesType.CalculatedAllocationPreference, false);
                    if (!string.IsNullOrEmpty(preferenceUpdateResult.Error))
                    {
                        return preferenceUpdateResult.Error;
                    }

                    if (preferenceUpdateResult.Preference == null)
                    {
                        return "Allocation template can not be created";
                    }
                    allocationPreference = preferenceUpdateResult.Preference;
                }

                allocationPreference.TryUpdateTargetPercentage(targetPercs);
                AllocationRule defaulfRule = new AllocationRule();
                defaulfRule.BaseType = AllocationBaseType.CumQuantity;
                defaulfRule.RuleType = MatchingRuleType.None;
                defaulfRule.MatchClosingTransaction = transactionType;
                defaulfRule.PreferenceAccountId = -1;
                allocationPreference.TryUpdateDefaultRule(defaulfRule);
                var result = allocation.UpdatePreference(allocationPreference);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    return result.Error;
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
            return string.Empty;
        }


        public void CreateCheckListWisePrefAndOrder(
            PTTRequestObject pttRequestObject,
            List<PTTResponseObject> pttResponseObjectList,
            int orderNo,
            AllocationOperationPreference allocOperation)
        {
            try
            {
                CheckListWisePreference chklist = CreateCheckListWisePreference(pttResponseObjectList, orderNo);
                allocOperation.TryUpdateCheckList(chklist);
                PreferenceUpdateResult pref = allocation.UpdatePreference(allocOperation);
                allocOperation = pref.Preference;
                //var order = CreateOrderSingle(pttRequestObject, pttResponseObjectList, allocOperation.OperationPreferenceId);
                //return order;
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

        public static CheckListWisePreference CreateCheckListWisePreference(List<PTTResponseObject> pttResponseObjectsList, int orderNo)
        {
            CheckListWisePreference checkListWisePref = null;
            try
            {
                decimal totalQty = pttResponseObjectsList.Sum(x => x.TradeQuantity);
                if (totalQty > 0)
                {
                    SerializableDictionary<int, AccountValue> targetPercs = new SerializableDictionary<int, AccountValue>();
                    foreach (PTTResponseObject item in pttResponseObjectsList)
                    {
                        if (!targetPercs.ContainsKey(item.AccountId))
                        {
                            AccountValue fv = new AccountValue(item.AccountId, item.PercentageAllocation);
                            // adding strategy with 0 qty as we do not have strategy wise qty here and also mot using it.
                            fv.StrategyValueList.Add(new StrategyValue(0, 100, 0));
                            targetPercs.Add(item.AccountId, fv);
                        }
                    }
                    checkListWisePref = new CheckListWisePreference();
                    checkListWisePref.ChecklistId = orderNo;
                    checkListWisePref.OrderSideOperator = CustomOperator.Include;
                    checkListWisePref.OrderSideList.Add(pttResponseObjectsList.FirstOrDefault(x => x.OrderSide != null).OrderSide);
                    checkListWisePref.TargetPercentage = targetPercs;
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
            return checkListWisePref;
        }
    }
}