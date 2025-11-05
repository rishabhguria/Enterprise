using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.Global;
using System.Linq;
using Prana.BusinessObjects.AppConstants;
using Prana.DataManager;
using System.Text;

namespace Prana.OrderProcessor
{
    internal class MultiBrokerOrderHelper
    {
        private static MultiBrokerOrderHelper _multiBrokerOrderHelper = null;

        public static MultiBrokerOrderHelper GetInstance
        {
            get
            {
                if (_multiBrokerOrderHelper == null)
                {
                    _multiBrokerOrderHelper = new MultiBrokerOrderHelper();
                }
                return _multiBrokerOrderHelper;
            }
        }

        /// <summary>
        /// Create PranaMessage list for handling account wise custodian brokers
        /// </summary>
        /// <param name="PranaMessage"></param>
        /// <param name="allocationServices"></param>
        /// <returns></returns>
        internal List<PranaMessage> CreatePranaMessagesList(PranaMessage PranaMessage, IAllocationServices allocationServices)
        {
            List<PranaMessage> messages = new List<PranaMessage>();
            try
            {
                if (PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AccountBrokerMapping] == null)
                {
                    messages.Add(PranaMessage);
                    return messages;
                }
                bool isOrderReplaced = false;
                StringBuilder newAllocationPrefIDs = new StringBuilder();
                int level1ID = int.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Level1ID].Value);
                AllocationOperationPreference aop = allocationServices.GetPreferenceById(level1ID);
                string symbol = string.Empty;
                if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TickerSymbol))
                {
                   symbol = PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TickerSymbol].Value;
                }
                else if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSymbol))
                {
                    symbol = PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value;
                }
                string companyUserID = PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID].Value;
                double subOrderCumQty = double.Parse(PranaMessage.FIXMessage.InternalInformation.GetField(CustomFIXConstants.CUST_TAG_CumQtyForSubOrder));
                double totalQtyValue = double.Parse(PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                int nirvanaMsgType = int.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                bool isCreateStage = (bool.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsStageRequired].Value)
                                        || nirvanaMsgType.Equals((int)OrderFields.PranaMsgTypes.ORDStaged))
                                        && (nirvanaMsgType != (int)OrderFields.PranaMsgTypes.ORDManualSub && nirvanaMsgType != (int)OrderFields.PranaMsgTypes.ORDNewSub);
                
                int transactionSource = int.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TransactionSourceTag].Value);

                if (nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub)
                {
                    subOrderCumQty = double.Parse(PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                }              
                Dictionary<int, int> accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AccountBrokerMapping].Value);
                var accountWiseExecutingBrokerMapping = CachedDataManager.GetInstance.GetAccountWiseExecutingBrokerMapping();
                PranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CounterPartyID, int.MinValue.ToString());
                if (!PranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OriginalLevel1ID))
                    PranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OriginalLevel1ID, int.MinValue.ToString());
                
                if (aop != null)
                {
                    List<int> mappedAccounts = new List<int>();
                    List<int> unMappedAccounts = new List<int>();
                    var allocatedAccountsTargetPercentage = aop.TargetPercentage;
                    if (transactionSource == (int)TransactionSource.PST)
                    {
                        allocatedAccountsTargetPercentage = GetTargetPercentageForPTTOrder(aop, PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSide].Value);
                    }
                    foreach (var accountID in allocatedAccountsTargetPercentage.Keys)
                    {
                        var adminBrokerID = int.MinValue;
                        if (accountWiseExecutingBrokerMapping.ContainsKey(accountID))
                        {
                            adminBrokerID = accountWiseExecutingBrokerMapping[accountID];
                        }
                        var manualBrokerID = accountBrokerMapping[accountID];
                        if (adminBrokerID != int.MinValue && adminBrokerID == manualBrokerID && isCreateStage)
                        {
                            mappedAccounts.Add(accountID);
                        }
                        else
                        {
                            unMappedAccounts.Add(accountID);
                        }
                    }

                    if (mappedAccounts.Count > 0)
                    {
                        bool isMappedAccountHaveSameBroker = true;
                        int brokerID = accountBrokerMapping[mappedAccounts[0]];
                        foreach (var accountID in mappedAccounts)
                        {
                            isMappedAccountHaveSameBroker = isMappedAccountHaveSameBroker && (accountBrokerMapping[accountID] == brokerID);
                        }
                        if (mappedAccounts.Count == allocatedAccountsTargetPercentage.Count)
                        {
                            PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value = isMappedAccountHaveSameBroker ? brokerID.ToString() : int.MinValue.ToString();
                            messages.Add(PranaMessage);
                        }
                        else
                        {
                            PranaMessage subMessage = PranaMessage.Clone();
                            int subMsglevel1ID = int.MinValue;
                            int originalLevel1Id = int.MinValue;
                            double subMsgSubOrderQty = 0;
                            double subMsgQty = 0;
                            if (mappedAccounts.Count == 1 && transactionSource != (int)TransactionSource.PST)
                            {
                                subMsglevel1ID = mappedAccounts[0];
                                var accountValue = allocatedAccountsTargetPercentage[mappedAccounts[0]].Value;
                                subMsgSubOrderQty = Math.Round(Convert.ToDouble(accountValue) * subOrderCumQty / 100, 10);
                                subMsgQty = Math.Round(Convert.ToDouble(accountValue) * totalQtyValue / 100, 10);
                            }
                            else
                            {
                                var allocationOperationPreference = CreateAllocationOperationPreference(allocatedAccountsTargetPercentage, mappedAccounts, totalQtyValue, out subMsgQty, symbol, companyUserID, allocationServices);
                                if (allocationOperationPreference != null)
                                {
                                    subMsglevel1ID = transactionSource == (int)TransactionSource.PST && mappedAccounts.Count == 1 ? mappedAccounts[0] : allocationOperationPreference.OperationPreferenceId;
                                    originalLevel1Id = allocationOperationPreference.OperationPreferenceId;
                                    newAllocationPrefIDs.Append(originalLevel1Id).Append(',');
                                }
                                subMsgSubOrderQty = DistributeQtyForCalculation(allocatedAccountsTargetPercentage, mappedAccounts, subOrderCumQty);
                            }
                            subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Level1ID].Value = subMsglevel1ID.ToString();
                            subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OriginalLevel1ID].Value = originalLevel1Id.ToString();
                            subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CumQtyForSubOrder].Value = subMsgSubOrderQty.ToString();
                            subMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = subMsgQty.ToString();
                            subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value = isMappedAccountHaveSameBroker ? brokerID.ToString() : int.MinValue.ToString();

                            messages.Add(subMessage);
                        }
                        if (PranaMessage.MessageType == FIXConstants.MSGOrderCancelReplaceRequest && messages.Count > 0)
                        {
                            isOrderReplaced = true;
                        }
                    }
                    if (unMappedAccounts.Count > 0)
                    {
                        //creating a mapping from broker to account as multiple accounts can be mapped with one broker and this will help in computing number of trades needs to be generated
                        Dictionary<int, List<int>> brokerWiseAccountMapping = new Dictionary<int, List<int>>();
                        var brokerID = int.MinValue;
                        foreach (var accountId in unMappedAccounts)
                        {
                            brokerID = accountBrokerMapping[accountId];
                            if (!brokerWiseAccountMapping.ContainsKey(brokerID))
                            {
                                brokerWiseAccountMapping.Add(brokerID, new List<int>());
                            }
                            brokerWiseAccountMapping[brokerID].Add(accountId);
                        }

                        if (brokerWiseAccountMapping.Count == 1 && mappedAccounts.Count == 0)
                        {
                            PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value = brokerID.ToString();
                            if (brokerID == int.MinValue)
                            {
                                PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_VenueID].Value = int.MinValue.ToString();
                            }
                            PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsUseCustodianBroker].Value = false.ToString();
                            messages.Add(PranaMessage);
                        }
                        else
                        {
                            //for all broker create new trade and assign counterpartyid and allocation accordingly
                            foreach (KeyValuePair<int, List<int>> kvp in brokerWiseAccountMapping)
                            {
                                PranaMessage subMessage = PranaMessage.Clone();
                                if (subMessage.MessageType == FIXConstants.MSGOrderCancelReplaceRequest && isOrderReplaced)
                                {
                                    subMessage.MessageType = FIXConstants.MSGOrder;
                                }
                                int subMsglevel1ID = int.MinValue;
                                int originalLevel1Id = int.MinValue;
                                if (kvp.Value.Count == 1 && transactionSource != (int)TransactionSource.PST)
                                {
                                    subMsglevel1ID = kvp.Value[0];
                                    subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Level1ID].Value = subMsglevel1ID.ToString();
                                    var accountValue = aop.TargetPercentage[kvp.Value[0]].Value;
                                    subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CumQtyForSubOrder].Value = Math.Round(Convert.ToDouble(accountValue) * subOrderCumQty / 100, 10).ToString();
                                    subMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = Math.Round(Convert.ToDouble(accountValue) * totalQtyValue / 100, 10).ToString();
                                }
                                else
                                {
                                    double subMsgSubOrderQty = 0;
                                    double subMsgQty = 0;
                                    var allocationOperationPreference = CreateAllocationOperationPreference(allocatedAccountsTargetPercentage, kvp.Value, totalQtyValue, out subMsgQty, symbol, companyUserID, allocationServices);
                                    if (allocationOperationPreference != null)
                                    {
                                        subMsglevel1ID = transactionSource == (int)TransactionSource.PST && kvp.Value.Count == 1 ? kvp.Value[0] : allocationOperationPreference.OperationPreferenceId;
                                        originalLevel1Id = allocationOperationPreference.OperationPreferenceId;
                                        newAllocationPrefIDs.Append(originalLevel1Id).Append(',');
                                    }
                                    subMsgSubOrderQty = DistributeQtyForCalculation(allocatedAccountsTargetPercentage, kvp.Value, subOrderCumQty);

                                    subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Level1ID].Value = subMsglevel1ID.ToString();
                                    subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CumQtyForSubOrder].Value = subMsgSubOrderQty.ToString();
                                    subMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = subMsgQty.ToString();
                                }
                                subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OriginalLevel1ID].Value = originalLevel1Id.ToString();
                                subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value = kvp.Key.ToString();
                                if (kvp.Key == int.MinValue)
                                {
                                    subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_VenueID].Value = int.MinValue.ToString();
                                }
                                subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsUseCustodianBroker].Value = false.ToString();
                                if (nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub)
                                {
                                    subMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CumQtyForSubOrder].Value.ToString();
                                }
                                messages.Add(subMessage);
                            }
                        }
                    }
                    if (transactionSource == (int)TransactionSource.PST && newAllocationPrefIDs.Length > 0)
                    {
                        newAllocationPrefIDs.Remove(newAllocationPrefIDs.Length - 1, 1);
                        ServerDataManager.SavePTTAllocationMapping(level1ID, newAllocationPrefIDs.ToString());
                    }
                }
                else
                {
                    int brokerID = accountBrokerMapping[level1ID];
                    if (!(accountWiseExecutingBrokerMapping.ContainsKey(level1ID) && accountWiseExecutingBrokerMapping[level1ID] == brokerID))
                    {
                        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsUseCustodianBroker].Value = false.ToString();
                    }
                    PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value = brokerID.ToString();
                    if (brokerID == int.MinValue)
                    {
                        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_VenueID].Value = int.MinValue.ToString();
                    }
                    messages.Add(PranaMessage);
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
            return messages;
        }

        /// <summary>
        /// Gets the Target percentage for PTT Orders
        /// </summary>
        /// /// <returns>SerializableDictionary<int, AccountValue></returns>
        internal SerializableDictionary<int, AccountValue> GetTargetPercentageForPTTOrder(AllocationOperationPreference aop, String tagSideValue)
        {
            SerializableDictionary<int, AccountValue> allocatedAccountsTargetPercentage = aop.TargetPercentage;
            try
            {
                if (aop.CheckListWisePreference != null && aop.CheckListWisePreference.Count > 0)
                {
                    foreach (CheckListWisePreference checkListwisePrefrence in aop.CheckListWisePreference.Values)
                    {
                        if ((checkListwisePrefrence.OrderSideList[0] == tagSideValue) || ((checkListwisePrefrence.OrderSideList[0] == "10") && (tagSideValue == FIXConstants.SIDE_Buy_Closed)))
                        {
                            allocatedAccountsTargetPercentage = checkListwisePrefrence.TargetPercentage;
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
            return allocatedAccountsTargetPercentage;
        }

        /// <summary>
        /// This method is to create new allocation preference from existing preference and new allocations
        /// </summary>
        /// <param name="aop"></param>
        /// <param name="accountIds"></param>
        /// <param name="totalQuantity"></param>
        /// <param name="newQuantity"></param>
        /// <param name="symbol"></param>
        /// <param name="companyUserID"></param>
        /// <param name="allocationServices"></param>
        /// <returns>AllocationOperationPreference</returns>
        internal AllocationOperationPreference CreateAllocationOperationPreference(SerializableDictionary<int, AccountValue> allocatedAccountsTargetPercentage, List<int> accountIds, double totalQuantity, out double newQuantity, string symbol, string companyUserID, IAllocationServices allocationServices)
        {
            AllocationOperationPreference allocationOperationPreference = null;
            try
            {
                double quantity = 0;
                Dictionary<int, double> newAccountQuantity = new Dictionary<int, double>();
                foreach (var accountId in accountIds)
                {
                    var nquantity = Math.Round(Convert.ToDouble(allocatedAccountsTargetPercentage[accountId].Value) * totalQuantity / 100, 10);
                    newAccountQuantity.Add(accountId, nquantity);
                    quantity += nquantity;
                }
                newQuantity = quantity;

                SerializableDictionary<int, AccountValue> targetPercs = new SerializableDictionary<int, AccountValue>();

                foreach (var accountId in accountIds)
                {
                    decimal share = (decimal)((newAccountQuantity[accountId] / quantity) * 100);
                    AccountValue accountValue = new AccountValue(accountId, share);
                    // adding strategy with 0 qty as we do not have strategy wise qty here and also mot using it.
                    accountValue.StrategyValueList.Add(new StrategyValue(0, 100, 0));
                    targetPercs.Add(accountId, accountValue);
                }

                string prefName = "*Custom#_" + symbol + "_" + companyUserID + "_" + DateTime.Now.Ticks;
                string error = CreateAllocationOperationPreference(targetPercs, ref allocationOperationPreference, prefName, CachedDataManager.GetInstance.GetCompanyID(), allocationServices);
                if (string.IsNullOrEmpty(error))
                {
                    return allocationOperationPreference;
                }
                else
                {
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                newQuantity = 0;
            }
            return allocationOperationPreference;
        }

        /// <summary>
        /// This Method is for creating allocation operation preference
        /// </summary>
        /// <returns>string</returns>
        internal string CreateAllocationOperationPreference(SerializableDictionary<int, AccountValue> targetPercs, ref AllocationOperationPreference allocationPreference, string prefName, int companyUserID, IAllocationServices allocationServices)
        {
            try
            {
                PreferenceUpdateResult preferenceUpdateResult = allocationServices.AddPreference(prefName, companyUserID, AllocationPreferencesType.CalculatedAllocationPreference, false);
                if (!string.IsNullOrEmpty(preferenceUpdateResult.Error))
                {
                    return preferenceUpdateResult.Error;
                }

                if (preferenceUpdateResult.Preference == null)
                {
                    return "Allocation template can not be created";
                }
                allocationPreference = preferenceUpdateResult.Preference;

                allocationPreference.TryUpdateTargetPercentage(targetPercs);
                AllocationRule defaulfRule = new AllocationRule();
                defaulfRule.BaseType = AllocationBaseType.CumQuantity;
                defaulfRule.RuleType = MatchingRuleType.None;
                defaulfRule.MatchClosingTransaction = MatchClosingTransactionType.None;
                defaulfRule.PreferenceAccountId = -1;
                allocationPreference.TryUpdateDefaultRule(defaulfRule);
                var result = allocationServices.UpdatePreference(allocationPreference);
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

        internal double DistributeQtyForCalculation(SerializableDictionary<int, AccountValue> allocatedAccountsTargetPercentage, List<int> accounts, double quantity)
        {
            double newQuantity = 0;
            try
            {
                foreach (var accountId in accounts)
                {
                    var nquantity = Math.Round(Convert.ToDouble(allocatedAccountsTargetPercentage[accountId].Value) * quantity / 100, 10);
                    newQuantity += nquantity;
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
            return newQuantity;
        }
    }
}
