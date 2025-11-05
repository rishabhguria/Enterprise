using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Rebalancer.PercentTradingTool.Preferences;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Prana.Rebalancer.PercentTradingTool.BusinessLogic
{
    /// <summary>
    /// Responsible for all the calculations in PTT
    /// </summary>
    internal static class PTTCalculator
    {
        private static int useZeroPositionsAsPositiveOrNegativeStatic = 0;

        private static int RoundingUptoDigits = 4;
        /// <summary>
        /// Does Calcualtion based on parameters.
        /// </summary>
        /// <param name="requestObject">Calcualtion parameter</param>
        /// <param name="responseList">Updated response</param>
        /// <returns></returns>
        internal static List<PTTResponseObject> Calculate(PTTRequestObject requestObject, List<PTTResponseObject> responseList, int useZeroPositionsAsPositiveOrNegative, ref StringBuilder errorMessage)
        {
            try
            {
                useZeroPositionsAsPositiveOrNegativeStatic = useZeroPositionsAsPositiveOrNegative;
                UpdateCalculation(requestObject, ref responseList, ref errorMessage);

                return responseList;
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

        private static Dictionary<string, List<PTTResponseObject>> CreateDictionaryOnOrderSides(List<PTTResponseObject> responseList)
        {
            try
            {
                Dictionary<string, List<PTTResponseObject>> orderSideWiseResponseList = PTTManager.DictOrdersAndResponse;
                foreach (PTTResponseObject responseObject in responseList)
                {
                    if (responseObject.OrderSide != null)
                    {
                        if (orderSideWiseResponseList.ContainsKey(responseObject.OrderSide))
                        {
                            orderSideWiseResponseList[responseObject.OrderSide].Add(responseObject);
                        }
                        else
                        {
                            List<PTTResponseObject> listResponseObject = new List<PTTResponseObject>();
                            listResponseObject.Add(responseObject);
                            orderSideWiseResponseList.Add(responseObject.OrderSide, listResponseObject);
                        }
                    }
                }
                return orderSideWiseResponseList;
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

        /// <summary>
        /// Recalculates the specified request object.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <param name="responseList">The response list.</param>
        /// <param name="isTradeQuantityColumn"></param>
        /// <returns></returns>
        internal static List<PTTResponseObject> Recalculate(ref PTTRequestObject requestObject, ref List<PTTResponseObject> responseList, PTTEditedColumn editedColumn, bool childUpdate, ref StringBuilder errorMessg, int updatedAccount)
        {
            try
            {
                PTTManager.DictOrdersAndResponse = new Dictionary<string, List<PTTResponseObject>>();
                RecalculateData(ref requestObject, ref responseList, editedColumn, childUpdate, ref errorMessg, updatedAccount);

                return responseList;
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

        /// <summary>
        /// Calcualtes Accounts Percentage based on QTY
        /// </summary>
        private static void CalculateAllocationPercentage()
        {
            try
            {
                foreach (List<PTTResponseObject> responseList in PTTManager.DictOrdersAndResponse.Values)
                {
                    decimal totalQty = responseList.Sum(x => x.TradeQuantity);
                    if (totalQty != 0)
                    {
                        foreach (PTTResponseObject response in responseList)
                        {
                            response.PercentageAllocation = response.TradeQuantity == 0 ? 0 : (response.TradeQuantity / totalQty) * 100;
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
        }

        /// <summary>
        /// Calculated % change Column
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <param name="responseObject"></param>
        /// <param name="consolidatedNAV"></param>
        /// <param name="consolidatedStartingPosition"></param>
        /// <param name="consolidatedStartingValue"></param>
        /// <returns></returns>
        private static decimal UpdateChangePercentage(PTTRequestObject requestObject, PTTResponseObject responseObject, decimal consolidatedNAV, decimal consolidatedStartingPosition, decimal consolidatedStartingValue, bool combineTotals, decimal percentTarget, decimal accountPercentPref, ref StringBuilder errorMssg, bool isResponseEdited)
        {
            try
            {
                decimal prorataRatio;
                if (accountPercentPref < 0)
                {
                    prorataRatio = consolidatedStartingPosition != 0 ? responseObject.StartingPosition / consolidatedStartingPosition : 0;
                }
                else
                {
                    prorataRatio = accountPercentPref;
                }

                decimal percentageChange = 0;

                //Description of conditional checks at target
                //If calculation is on master fund, we first calculate  %change at master fund level including unit of target i.e Percentage/Basis pt/$Amt
                //when this function is called again for calculation of it's subaccounts then we already have the required target at that masterfund level 
                //we just need to distribute the target on prorata basis to the accounts, hence calculate target is not called in this case.
                //But when the PTT is used for calculations at Account level then we have to calculate target every time for different accounts so 
                //CalculateTarget will be called each time.

                decimal target = (PTTMasterFundOrAccount)requestObject.MasterFundOrAccount.Value == PTTMasterFundOrAccount.MasterFund ?
                    responseObject.ChildResponseObjectList != null ? CalculateTarget(requestObject, consolidatedNAV, percentTarget) : percentTarget
                    : CalculateTarget(requestObject, consolidatedNAV, percentTarget);

                if (responseObject.ChildResponseObjectList == null)
                {
                    BindingList<PTTAccountPreference> pttAccountPrefList = PTTPrefDataManager.PttAccountFactorBindingList;
                    PTTAccountPreference accPref = null;
                    if (pttAccountPrefList != null)
                    {
                        accPref = pttAccountPrefList.FirstOrDefault(x => x.AccountId == responseObject.AccountId);
                    }

                    if ((PTTCombineAccountTotalValue)requestObject.CombineAccountEnumValue.Value == PTTCombineAccountTotalValue.No)
                    {
                        if (accPref != null && isResponseEdited)
                        {
                            target = target * (decimal)accPref.AccountFactor;
                        }
                    }
                    else
                    {
                        if (accPref != null && accPref.AccountFactor != 1)
                        {
                            errorMssg.Clear();
                            errorMssg.Append(PTTConstants.MSG_NO_ACCOUNTFACTOR_FOR_COMBINEACCOUNTTOTAL_YES);
                        }
                    }
                }

                if (combineTotals)
                {
                    switch ((PTTChangeType)requestObject.AddOrSet.Value)
                    {
                        case PTTChangeType.Increase:
                        case PTTChangeType.Buy:
                        case PTTChangeType.SellShort:
                            percentageChange = prorataRatio * target;
                            break;

                        case PTTChangeType.Decrease:
                            decimal maxPermittedPercentageChange = GetMaxPermittedPercentageChange(requestObject, responseObject, consolidatedNAV, consolidatedStartingPosition, consolidatedStartingValue);
                            if (target > maxPermittedPercentageChange)
                            {
                                percentageChange = prorataRatio * maxPermittedPercentageChange;
                            }
                            else
                            {
                                percentageChange = prorataRatio * target;
                            }
                            break;

                        case PTTChangeType.Set:
                            maxPermittedPercentageChange = GetMaxPermittedPercentageChange(requestObject, responseObject, consolidatedNAV, consolidatedStartingPosition, consolidatedStartingValue);
                            if (target < 0 && Math.Abs(target) > maxPermittedPercentageChange)
                            {
                                percentageChange = Math.Sign(target) * prorataRatio * maxPermittedPercentageChange;
                            }
                            else
                            {
                                percentageChange = prorataRatio * target;
                            }
                            break;
                    }
                }
                else
                {
                    switch ((PTTChangeType)requestObject.AddOrSet.Value)
                    {
                        case PTTChangeType.Increase:
                        case PTTChangeType.Buy:
                        case PTTChangeType.SellShort:
                            percentageChange = target;
                            break;
                        case PTTChangeType.Decrease:
                            decimal maxPermittedPercentageChange = GetMaxPermittedPercentageChange(requestObject, responseObject, consolidatedNAV, consolidatedStartingPosition, consolidatedStartingValue);
                            percentageChange = target;
                            percentageChange = percentageChange > maxPermittedPercentageChange ? maxPermittedPercentageChange : target;
                            break;
                        case PTTChangeType.Set:
                            percentageChange = target - responseObject.StartingPercentage;
                            break;
                    }
                }


                return percentageChange;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return 0.0M;
            }
        }

        private static decimal GetMaxPermittedPercentageChange(PTTRequestObject requestObject, PTTResponseObject responseObject, decimal consolidatedNAV, decimal consolidatedStartingPosition, decimal consolidatedStartingValue)
        {
            try
            {
                decimal maxPermittedTradeQuantity = Math.Abs(consolidatedStartingPosition);
                decimal maxEndAccountSymbolValue = Math.Abs(maxPermittedTradeQuantity * (requestObject.SelectedFeedPrice * responseObject.FxRate) *
                             Convert.ToDecimal(requestObject.SecMasterBaseObj.Multiplier) *
                             Convert.ToDecimal(requestObject.SecMasterBaseObj.Delta));
                decimal maxPermittedPercentageChange = consolidatedNAV > 0 ? (maxEndAccountSymbolValue / consolidatedNAV) * 100 : 0;
                return maxPermittedPercentageChange;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }

        private static decimal CalculateTarget(PTTRequestObject requestObject, decimal consolidatedNAV, decimal percentTarget)
        {
            decimal target = 0;
            try
            {
                switch ((PTTType)requestObject.Type.Value)
                {
                    case PTTType.BasisPoints:
                        target = percentTarget / 100;
                        break;

                    case PTTType.DollarAmount:
                        target = (percentTarget / consolidatedNAV) * 100;
                        break;

                    default:
                        target = percentTarget;
                        break;
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
            return target;
        }

        /// <summary>
        /// Updates calculations in Response object
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <param name="responseList">The response list.</param>
        /// <param name="errorMessage"></param>
        private static void UpdateCalculation(PTTRequestObject requestObject, ref List<PTTResponseObject> responseList, ref StringBuilder errorMessage)
        {
            try
            {
                decimal accountPercentagePref = -1;
                PTTManager.MfAccountPrefBindList = PTTPrefDataManager.GetInstance.PTTMfAccountPrefBindingList;
                bool combineAccountTotal = (PTTCombineAccountTotalValue)requestObject.CombineAccountEnumValue.Value == PTTCombineAccountTotalValue.Yes ? true : false;
                bool isChangeTypeSet = (PTTChangeType)requestObject.AddOrSet.Value == PTTChangeType.Set ? true : false;
                bool isShortLongPrefSet = PTTPrefDataManager.GetInstance.GetPTTPreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID).UseShortLongPref;
                PTTManager.DictOrdersAndResponse = new Dictionary<string, List<PTTResponseObject>>();
                foreach (PTTResponseObject responseObject in responseList)
                {
                    responseObject.RoundLots = requestObject.SecMasterBaseObj.RoundLot;
                    decimal consolidatedNAV = combineAccountTotal ? Math.Abs(responseList.Sum(x => x.AccountNAV)) : Math.Abs(responseObject.AccountNAV);
                    decimal consolidatedStartingPosition = combineAccountTotal ? responseList.Sum(x => x.StartingPosition) : responseObject.StartingPosition;
                    decimal consolidatedStartingValue = combineAccountTotal ? responseList.Sum(x => x.StartingValue) : responseObject.StartingValue;
                    decimal masterFundLevelPercentage = 0;
                    if (combineAccountTotal && isChangeTypeSet && consolidatedNAV > 0)
                    {
                        switch ((PTTType)requestObject.Type.Value)
                        {
                            case PTTType.BasisPoints:
                                masterFundLevelPercentage = requestObject.Target - 10000 * (consolidatedStartingValue / (consolidatedNAV));
                                break;
                            case PTTType.DollarAmount:
                                masterFundLevelPercentage = requestObject.Target - consolidatedStartingValue;
                                break;
                            default:
                                masterFundLevelPercentage = requestObject.Target - 100 * (consolidatedStartingValue / consolidatedNAV);
                                break;
                        }
                    }
                    else
                    {
                        masterFundLevelPercentage = requestObject.Target;
                    }
                    if ((PTTMasterFundOrAccount)requestObject.MasterFundOrAccount.Value == PTTMasterFundOrAccount.MasterFund && responseObject.ChildResponseObjectList != null)
                    {
                        responseObject.StartingPercentage = consolidatedNAV != 0 ? (responseObject.StartingValue / consolidatedNAV) * 100 : 0;
                        responseObject.PercentageType = consolidatedNAV != 0 ? UpdateChangePercentage(requestObject, responseObject, consolidatedNAV, consolidatedStartingPosition, consolidatedStartingValue, combineAccountTotal, masterFundLevelPercentage, accountPercentagePref, ref errorMessage, true) : 0;
                        PTTMFAccountPref pttPref = new PTTMFAccountPref();
                        if (!isShortLongPrefSet)
                        {
                            pttPref = PTTManager.MfAccountPrefBindList[0].FirstOrDefault(x => x.MasterFundId == responseObject.AccountId);
                        }
                        else
                        {
                            pttPref = calculateBasedOnShortLongPref(requestObject, responseObject, pttPref);
                        }

                        //isMasterFundHavingSingleAccount will check that master fund contains only one account associated with it
                        bool isMasterFundHavingSingleAccount = pttPref.AccountWisePercentage.Count == 1 ? true : false;

                        //isMasterFundUserDefinedPrefNeeded -> when combine Account total is Yes, then we need to check whether some quantity
                        //is being distributed to particular master fund whose starting pos =0, we infer it using percentageChange column(Pro-rata distribution) 
                        //When combine Account total is No, then we need to check the NAV of that master fund whose starting pos =0 if NAV is zero than preference 
                        //is not required as no trade will be allocated to this master fund.
                        bool isMasterFundUserDefinedPrefNeeded = combineAccountTotal ? responseObject.PercentageType > 0 ? true : false : Math.Abs(responseObject.AccountNAV) > 0 ? true : false;
                        foreach (var pstChildObject in responseObject.ChildResponseObjectList)
                        {
                            pstChildObject.RoundLots = requestObject.SecMasterBaseObj.RoundLot;
                            if (pttPref != null && isMasterFundUserDefinedPrefNeeded)
                            {
                                accountPercentagePref = GetProrataFromPreference(ref errorMessage, accountPercentagePref, responseObject, pttPref, isMasterFundHavingSingleAccount, pstChildObject);
                            }
                            CalculateEndingValues(requestObject, pstChildObject, consolidatedNAV, responseObject.StartingPosition, responseObject.StartingValue, true, responseObject.PercentageType, accountPercentagePref, ref errorMessage, true);
                            accountPercentagePref = -1;
                        }
                        PTTManager.DictOrdersAndResponse = CreateDictionaryOnOrderSides(responseObject.ChildResponseObjectList);
                        responseObject.PercentageType = responseObject.ChildResponseObjectList.Sum(x => x.PercentageType);
                        SetMasterFundDetailsBasedOnAccount(responseObject);
                    }
                    if ((PTTMasterFundOrAccount)requestObject.MasterFundOrAccount.Value == PTTMasterFundOrAccount.Account)
                    {
                        CalculateEndingValues(requestObject, responseObject, consolidatedNAV, consolidatedStartingPosition, consolidatedStartingValue, combineAccountTotal, masterFundLevelPercentage, accountPercentagePref, ref errorMessage, true);
                    }
                    if (((PTTMasterFundOrAccount)requestObject.MasterFundOrAccount.Value == PTTMasterFundOrAccount.MasterFund) && ((PTTType)requestObject.Type.Value == PTTType.BasisPoints))
                    {
                        responseObject.StartingPercentage = responseObject.StartingPercentage * 100;
                    }
                }
                CreateOrderSideDictAndAllocationPercentage(requestObject, responseList);


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

        private static decimal GetProrataFromPreference(ref StringBuilder errorMessage, decimal accountPercentagePref, PTTResponseObject responseObject, PTTMFAccountPref pttPref, bool isMasterFundHavingSingleAccount, PTTResponseObject pstChildObject)
        {
            try
            {
                if (!pttPref.IsProrataPrefChecked || responseObject.StartingPosition == 0)
                {
                    if (pttPref.TotalPercentage != 100)
                    {
                        if (isMasterFundHavingSingleAccount)
                        {//Since only one account is associated with it hence we are allocating all the quantity to this account
                            accountPercentagePref = 1;
                        }
                        else
                        {
                            accountPercentagePref = 0;
                            errorMessage.Clear();
                            errorMessage.Append(PTTConstants.MSG_INVALIDPREFERENCESET);

                        }
                    }
                    else
                    {
                        accountPercentagePref = isMasterFundHavingSingleAccount ? 1 : pttPref.AccountWisePercentage.Any(x => x.AccountId == pstChildObject.AccountId) ? pttPref.AccountWisePercentage.FirstOrDefault(x => x.AccountId == pstChildObject.AccountId).Percentage / 100 : 0;
                    }
                }
                return accountPercentagePref;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return 0.0M;
            }
        }

        private static void CreateOrderSideDictAndAllocationPercentage(PTTRequestObject requestObject, List<PTTResponseObject> responseList)
        {
            try
            {
                if ((PTTMasterFundOrAccount)requestObject.MasterFundOrAccount.Value == PTTMasterFundOrAccount.MasterFund)
                {
                    CalculateAllocationPercentage();
                    foreach (var pttResponseObject in responseList)
                    {
                        pttResponseObject.PercentageAllocation = pttResponseObject.ChildResponseObjectList.Sum(x => x.PercentageAllocation);
                    }
                }
                else
                {
                    PTTManager.DictOrdersAndResponse = CreateDictionaryOnOrderSides(responseList);
                    CalculateAllocationPercentage();
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

        private static void CalculateEndingValues(PTTRequestObject requestObject, PTTResponseObject responseObject, decimal consolidatedNAV, decimal consolidatedStartingPosition, decimal consolidatedStartingValue, bool combineTotals, decimal percentChange, decimal accountPercentagePref, ref StringBuilder errorMssg, bool isResponseEdited)
        {
            try
            {
                if (consolidatedNAV > 0)
                {
                    responseObject.StartingPercentage = (responseObject.StartingValue / consolidatedNAV) * 100;
                    responseObject.PercentageType = UpdateChangePercentage(requestObject, responseObject, consolidatedNAV, consolidatedStartingPosition, consolidatedStartingValue, combineTotals, percentChange, accountPercentagePref, ref errorMssg, isResponseEdited);
                }
                else
                {
                    responseObject.StartingPercentage = 0;
                    responseObject.PercentageType = 0;
                }
                //by default trade quantity will round of upto 4 decimal places if increase decimal precision is zero other wise it will have added value of increase decimal precision.
                RoundingUptoDigits = PTTPrefDataManager.GetInstance.GetPTTPreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID).IncreaseDecimalPrecision + 4;
                // Handling for https://jira.nirvanasolutions.com:8443/browse/PRANA-25977
                //  Handle PTT case when input changes ending percent to 0
                if (requestObject.Target == 0 && (PTTChangeType)requestObject.AddOrSet.Value == PTTChangeType.Set)
                {
                    if (consolidatedNAV > 0)
                        responseObject.TradeQuantity = responseObject.StartingPosition > 0 ? responseObject.StartingPosition : -responseObject.StartingPosition;
                }
                else
                {
                    responseObject.TradeQuantity = Math.Abs(Math.Floor((requestObject.SelectedFeedPrice * responseObject.FxRate) > 0 ? Math.Round((responseObject.PercentageType * consolidatedNAV) / (100 * (requestObject.SelectedFeedPrice * responseObject.FxRate)), RoundingUptoDigits) : 0));
                }
                if (responseObject.TradeQuantity != 0 && requestObject.IsUseRoundLot)
                {
                    responseObject.TradeQuantity = Math.Floor(Math.Floor(responseObject.TradeQuantity / responseObject.RoundLots) * responseObject.RoundLots);
                }
                UpdateObjectForRecalculation(requestObject, responseObject, consolidatedNAV);

                if ((PTTType)requestObject.Type.Value == PTTType.BasisPoints)
                {
                    responseObject.PercentageType = responseObject.PercentageType * 100;
                    responseObject.EndingPercentage = responseObject.EndingPercentage * 100;
                    responseObject.StartingPercentage = responseObject.StartingPercentage * 100;
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
        /// Recalculates the data.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <param name="responseList">The response list.</param>
        /// <param name="isTradeQuantityColumn"></param>
        private static void RecalculateData(ref PTTRequestObject requestObject, ref List<PTTResponseObject> responseList, PTTEditedColumn editedColumn, bool hasChildBand, ref StringBuilder errorMessg, int updatedAccount)
        {
            try
            {
                if (!hasChildBand)
                {
                    #region Editing at Account level under master fund
                    foreach (PTTResponseObject response in responseList)
                    {
                        if ((PTTMasterFundOrAccount)requestObject.MasterFundOrAccount.Value == PTTMasterFundOrAccount.MasterFund && response.ChildResponseObjectList != null)
                        {
                            bool combineAccountTotal = (PTTCombineAccountTotalValue)requestObject.CombineAccountEnumValue.Value == PTTCombineAccountTotalValue.Yes;
                            decimal consolidatedNAV = combineAccountTotal ? Math.Abs(response.ChildResponseObjectList.Sum(x => x.AccountNAV)) : Math.Abs(response.AccountNAV);
                            foreach (PTTResponseObject childResponse in response.ChildResponseObjectList)
                            {
                                decimal consolidatedStartingPosition = combineAccountTotal ? response.ChildResponseObjectList.Sum(x => x.StartingPosition) : childResponse.StartingPosition;
                                decimal consolidatedStartingValue = combineAccountTotal ? response.ChildResponseObjectList.Sum(x => x.StartingValue) : childResponse.StartingValue;
                                bool isResponseEdited = childResponse.AccountId == updatedAccount;
                                UpdateDataOnEditing(requestObject, consolidatedNAV, editedColumn, childResponse, consolidatedStartingPosition, consolidatedStartingValue, combineAccountTotal, isResponseEdited);
                            }
                            PTTManager.DictOrdersAndResponse = CreateDictionaryOnOrderSides(response.ChildResponseObjectList);
                            SetMasterFundDetailsBasedOnAccount(response);
                        }
                        else
                        {
                            bool combineAccountTotalAccountLevel = (PTTCombineAccountTotalValue)requestObject.CombineAccountEnumValue.Value == PTTCombineAccountTotalValue.Yes;
                            decimal consolidatedNAVForAccountLevel = combineAccountTotalAccountLevel ? Math.Abs(responseList.Sum(x => x.AccountNAV)) : Math.Abs(response.AccountNAV);
                            decimal consolidatedStartingPosition = combineAccountTotalAccountLevel ? responseList.Sum(x => x.StartingPosition) : response.StartingPosition;
                            decimal consolidatedStartingValue = combineAccountTotalAccountLevel ? responseList.Sum(x => x.StartingValue) : response.StartingValue;
                            bool isResponseEdited = response.AccountId == updatedAccount;
                            UpdateDataOnEditing(requestObject, consolidatedNAVForAccountLevel, editedColumn, response, consolidatedStartingPosition, consolidatedStartingValue, combineAccountTotalAccountLevel, isResponseEdited);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Editing at Master Fund level
                    bool isShortLongPrefSet = PTTPrefDataManager.GetInstance.GetPTTPreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID).UseShortLongPref;
                    foreach (PTTResponseObject masterFundObject in responseList)
                    {
                        bool combinedAccount = (PTTCombineAccountTotalValue)requestObject.CombineAccountEnumValue.Value == PTTCombineAccountTotalValue.Yes;
                        // PTTMFAccountPref pttPref = PTTManager.MfAccountPrefBindList[0].FirstOrDefault(x => x.MasterFundId == masterFundObject.AccountId);
                        PTTMFAccountPref pttPref = new PTTMFAccountPref();
                        if (!isShortLongPrefSet)
                        {
                            pttPref = PTTManager.MfAccountPrefBindList[0].FirstOrDefault(x => x.MasterFundId == masterFundObject.AccountId);
                        }
                        else
                        {
                            pttPref = calculateBasedOnShortLongPref(requestObject, masterFundObject, pttPref);
                        }

                        if (pttPref != null)
                        {
                            if (masterFundObject.StartingPosition != 0 && pttPref.IsProrataPrefChecked)
                            {
                                bool combineAccountTotal = (PTTCombineAccountTotalValue)requestObject.CombineAccountEnumValue.Value == PTTCombineAccountTotalValue.Yes ? true : false;
                                decimal consolidatedNAV = combineAccountTotal ? Math.Abs(responseList.Sum(x => x.AccountNAV)) : Math.Abs(masterFundObject.AccountNAV);
                                decimal consolidatedStartingPosition = combineAccountTotal ? responseList.Sum(x => x.StartingPosition) : masterFundObject.StartingPosition;
                                decimal consolidatedStartingValue = combineAccountTotal ? responseList.Sum(x => x.StartingValue) : masterFundObject.StartingValue;

                                UpdateDataAndDistributeTradesToAccounts(requestObject, editedColumn, ref errorMessg, masterFundObject, consolidatedNAV, consolidatedStartingPosition, consolidatedStartingValue, combinedAccount);
                            }
                            else
                            {
                                if (pttPref.TotalPercentage == 100 || pttPref.AccountWisePercentage.Count == 1)
                                {
                                    bool combineAccountTotal = (PTTCombineAccountTotalValue)requestObject.CombineAccountEnumValue.Value == PTTCombineAccountTotalValue.Yes ? true : false;
                                    decimal consolidatedNAV = combineAccountTotal ? Math.Abs(responseList.Sum(x => x.AccountNAV)) : Math.Abs(masterFundObject.AccountNAV);
                                    decimal consolidatedStartingPosition = combineAccountTotal ? responseList.Sum(x => x.StartingPosition) : masterFundObject.StartingPosition;
                                    decimal consolidatedStartingValue = combineAccountTotal ? responseList.Sum(x => x.StartingValue) : masterFundObject.StartingValue;
                                    UpdateDataAndDistributeTradesToAccounts(requestObject, editedColumn, ref errorMessg, masterFundObject, consolidatedNAV, consolidatedStartingPosition, consolidatedStartingValue, combinedAccount);
                                }
                                else
                                {
                                    errorMessg.Clear();
                                    errorMessg.Append(PTTConstants.MSG_INVALIDPREFERENCESET);
                                    SetMasterFundDetailsBasedOnAccount(masterFundObject);
                                }
                            }
                        }
                    }

                    #endregion
                }
                CreateOrderSideDictAndAllocationPercentage(requestObject, responseList);
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

        private static void UpdateDataAndDistributeTradesToAccounts(PTTRequestObject requestObject, PTTEditedColumn editedColumn, ref StringBuilder errorMessg, PTTResponseObject masterFundObject, decimal consolNav, decimal consolidatedStartingPosition, decimal consolidatedStartingValue, bool combinedAccount)
        {
            try
            {
                UpdateDataOnEditing(requestObject, consolNav, editedColumn, masterFundObject, consolidatedStartingPosition, consolidatedStartingValue, combinedAccount, false);
                if ((PTTMasterFundOrAccount)requestObject.MasterFundOrAccount.Value ==
                    PTTMasterFundOrAccount.MasterFund && masterFundObject.ChildResponseObjectList != null)
                {
                    foreach (PTTResponseObject accountObject in masterFundObject.ChildResponseObjectList)
                    {
                        if ((PTTType)requestObject.Type.Value == PTTType.BasisPoints)
                        {
                            accountObject.StartingPercentage = accountObject.StartingPercentage / 100;
                            accountObject.PercentageType = accountObject.PercentageType / 100;
                            accountObject.EndingPercentage = accountObject.EndingPercentage / 100;
                        }
                        DistributeEditedTradeQuantitiesToAccounts(requestObject, masterFundObject, accountObject, ref errorMessg, consolNav);
                        if ((PTTType)requestObject.Type.Value == PTTType.BasisPoints)
                        {
                            accountObject.StartingPercentage = accountObject.StartingPercentage * 100;
                            accountObject.PercentageType = accountObject.PercentageType * 100;
                            accountObject.EndingPercentage = accountObject.EndingPercentage * 100;
                        }
                    }
                    PTTManager.DictOrdersAndResponse = CreateDictionaryOnOrderSides(masterFundObject.ChildResponseObjectList);
                    masterFundObject.PercentageType = masterFundObject.ChildResponseObjectList.Sum(x => x.PercentageType);
                    masterFundObject.PercentageAllocation = masterFundObject.ChildResponseObjectList.Sum(x => x.PercentageAllocation);
                    masterFundObject.TradeQuantity = masterFundObject.ChildResponseObjectList.Sum(x => x.TradeQuantity);
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

        private static void SetMasterFundDetailsBasedOnAccount(PTTResponseObject response)
        {
            try
            {
                response.TradeQuantity = response.ChildResponseObjectList.Sum(x => x.TradeQuantity);
                response.EndingPercentage = response.ChildResponseObjectList.Sum(x => x.EndingPercentage);
                response.OrderSide = response.ChildResponseObjectList.Any(x => x.OrderSide != null) ? response.ChildResponseObjectList.FirstOrDefault(x => x.OrderSide != null).OrderSide : null;
                response.EndingPosition = response.ChildResponseObjectList.Sum(x => x.EndingPosition);
                response.EndingValue = response.ChildResponseObjectList.Sum(x => x.EndingValue);
                response.PercentageAllocation = response.ChildResponseObjectList.Sum(x => x.PercentageAllocation);
                response.PercentageType = response.ChildResponseObjectList.Sum(x => x.PercentageType);
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

        private static void DistributeEditedTradeQuantitiesToAccounts(PTTRequestObject requestObject, PTTResponseObject parentResponseObject, PTTResponseObject childResponseObject, ref StringBuilder errorMessg, decimal consolidatedNAV)
        {
            try
            {
                PTTManager.MfAccountPrefBindList = PTTPrefDataManager.GetInstance.PTTMfAccountPrefBindingList;
                bool isShortLongPrefSet = PTTPrefDataManager.GetInstance.GetPTTPreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID).UseShortLongPref;
                PTTMFAccountPref pttPref = new PTTMFAccountPref();
                if (!isShortLongPrefSet)
                {
                    pttPref = PTTManager.MfAccountPrefBindList[0].FirstOrDefault(x => x.MasterFundId == parentResponseObject.AccountId);
                }
                else
                {
                    pttPref = calculateBasedOnShortLongPref(requestObject, parentResponseObject, pttPref);
                }

                //isMasterFundHavingSingleAccount will check that master fund contains only one account associated with it
                bool isMasterFundHavingSingleAccount = pttPref.AccountWisePercentage.Count == 1 ? true : false;
                decimal prorataRatio = 0;
                prorataRatio = (parentResponseObject.StartingPosition != 0) ? (childResponseObject.StartingPosition / parentResponseObject.StartingPosition) : 0;
                if (pttPref != null)
                {
                    prorataRatio = GetProrataFromPreference(ref errorMessg, prorataRatio, parentResponseObject, pttPref, isMasterFundHavingSingleAccount, childResponseObject);
                }
                childResponseObject.TradeQuantity = (requestObject.SelectedFeedPrice * parentResponseObject.FxRate) > 0 ? (Math.Round(prorataRatio * parentResponseObject.TradeQuantity, 0)) : 0;
                childResponseObject.PercentageType = prorataRatio * parentResponseObject.PercentageType;
                UpdateObjectForRecalculation(requestObject, childResponseObject, consolidatedNAV);
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

        private static PTTMFAccountPref calculateBasedOnShortLongPref(PTTRequestObject requestObject, PTTResponseObject ResponseObject, PTTMFAccountPref pttPref)
        {
            try
            {
                int addOrSetValue = (int)requestObject.AddOrSet.Value;
                if (PTTManager.MfAccountPrefBindList.Count == 3)
                {
                    switch (addOrSetValue)
                    {
                        case (int)PTTChangeType.Buy:
                            pttPref = PTTManager.MfAccountPrefBindList[(int)PTTPreferenceType.Long].FirstOrDefault(x => x.MasterFundId == ResponseObject.AccountId);
                            break;
                        case (int)PTTChangeType.SellShort:
                            pttPref = PTTManager.MfAccountPrefBindList[(int)PTTPreferenceType.Short].FirstOrDefault(x => x.MasterFundId == ResponseObject.AccountId);
                            break;
                        case (int)PTTChangeType.Increase:
                        case (int)PTTChangeType.Decrease:
                        case (int)PTTChangeType.Set:
                            if (ResponseObject.StartingPosition > 0)
                            {
                                pttPref = PTTManager.MfAccountPrefBindList[(int)PTTPreferenceType.Long].FirstOrDefault(x => x.MasterFundId == ResponseObject.AccountId);
                            }
                            else if (ResponseObject.StartingPosition < 0)
                            {
                                pttPref = PTTManager.MfAccountPrefBindList[(int)PTTPreferenceType.Short].FirstOrDefault(x => x.MasterFundId == ResponseObject.AccountId);
                            }
                            else
                            {
                                if (useZeroPositionsAsPositiveOrNegativeStatic == 1)
                                {
                                    pttPref = PTTManager.MfAccountPrefBindList[(int)PTTPreferenceType.Long].FirstOrDefault(x => x.MasterFundId == ResponseObject.AccountId);
                                }
                                else
                                {
                                    pttPref = PTTManager.MfAccountPrefBindList[(int)PTTPreferenceType.Short].FirstOrDefault(x => x.MasterFundId == ResponseObject.AccountId);
                                }
                            }
                            break;
                    }
                }
                return pttPref;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return pttPref;
        }

        private static void UpdateDataOnEditing(PTTRequestObject requestObject, decimal consolidatedNAV, PTTEditedColumn editedColumn, PTTResponseObject response, decimal consolidatedStartingPosition, decimal consolidatedStartingValue, bool combinedAccount, bool isResponseEdited)
        {
            try
            {
                if ((PTTType)requestObject.Type.Value == PTTType.BasisPoints)
                {
                    response.StartingPercentage = response.StartingPercentage / 100;
                    //response.PercentageType = response.PercentageType / 100;
                    response.EndingPercentage = response.EndingPercentage / 100;
                }

                if (editedColumn == PTTEditedColumn.TradeQuantity)
                {
                    ValidateAndSetTradeQuantity(requestObject, response, response.TradeQuantity);
                }
                else
                {
                    if (editedColumn == PTTEditedColumn.PercentageType)
                    {
                        StringBuilder sb = new StringBuilder();
                        CalculateEndingValues(requestObject, response, consolidatedNAV, consolidatedStartingPosition, consolidatedStartingValue, combinedAccount, response.PercentageType, -1, ref sb, isResponseEdited);
                    }
                    switch ((PTTChangeType)requestObject.AddOrSet.Value)
                    {
                        //No case is defined for Buy and Sell Short as for both of them Starting Percentage is zero so such checks are not needed
                        case PTTChangeType.Increase:
                            if (response.EndingPercentage < response.StartingPercentage)
                            {
                                response.EndingPercentage = response.StartingPercentage;
                            }
                            break;

                        case PTTChangeType.Decrease:
                            if (response.EndingPercentage > response.StartingPercentage)
                            {
                                response.EndingPercentage = response.StartingPercentage;
                            }
                            break;

                        case PTTChangeType.Set:
                            response.PercentageType = response.EndingPercentage - response.StartingPercentage;
                            break;
                    }
                    decimal endAccountSymbolValue = (response.EndingPercentage * consolidatedNAV) / 100;
                    if (editedColumn == PTTEditedColumn.PercentageType && (PTTType)requestObject.Type.Value == PTTType.BasisPoints)
                    {
                        endAccountSymbolValue = (response.EndingPercentage / 100 * consolidatedNAV) / 100;
                    }

                    decimal endPosition = 0;
                    if ((requestObject.SelectedFeedPrice * response.FxRate * Convert.ToDecimal(requestObject.SecMasterBaseObj.Multiplier) * Convert.ToDecimal(requestObject.SecMasterBaseObj.Delta)) > 0)
                    {
                        endPosition = Math.Round(endAccountSymbolValue / (requestObject.SelectedFeedPrice * response.FxRate * Convert.ToDecimal(requestObject.SecMasterBaseObj.Multiplier) * Convert.ToDecimal(requestObject.SecMasterBaseObj.Delta)), RoundingUptoDigits);
                    }
                    if (response.StartingPosition < 0)
                    {
                        endPosition = -1 * endPosition;
                    }

                    decimal tempTradeQuantity = consolidatedNAV > 0 ? Math.Abs(endPosition - response.StartingPosition) : 0;
                    ValidateAndSetTradeQuantity(requestObject, response, tempTradeQuantity);
                }
                if ((PTTType)requestObject.Type.Value == PTTType.BasisPoints && editedColumn == PTTEditedColumn.PercentageType)
                {
                    response.StartingPercentage = response.StartingPercentage / 100;
                }
                UpdateObjectForRecalculation(requestObject, response, consolidatedNAV);
                if ((PTTType)requestObject.Type.Value == PTTType.BasisPoints)
                {
                    response.StartingPercentage = response.StartingPercentage * 100;
                    response.PercentageType = response.PercentageType * 100;
                    response.EndingPercentage = response.EndingPercentage * 100;
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

        private static void ValidateAndSetTradeQuantity(PTTRequestObject requestObject, PTTResponseObject response, decimal tempTradeQuantity)
        {
            try
            {
                tempTradeQuantity = Math.Floor(tempTradeQuantity);
                switch ((PTTChangeType)requestObject.AddOrSet.Value)
                {

                    //Increase,SellShort,Buy are independant of starting position hence no checks needed here
                    case PTTChangeType.Increase:
                    case PTTChangeType.SellShort:
                    case PTTChangeType.Buy:
                        response.TradeQuantity = tempTradeQuantity;
                        break;

                    case PTTChangeType.Decrease:
                        if (tempTradeQuantity > Math.Abs(response.StartingPosition))
                        {
                            response.TradeQuantity = Math.Abs(response.StartingPosition);
                        }
                        else
                        {
                            response.TradeQuantity = tempTradeQuantity;
                        }
                        break;

                    case PTTChangeType.Set:
                        if (response.PercentageType >= 0)
                        {
                            response.TradeQuantity = tempTradeQuantity;
                        }
                        else
                        {
                            if (tempTradeQuantity > Math.Abs(response.StartingPosition))
                            {
                                response.TradeQuantity = Math.Abs(response.StartingPosition);
                            }
                            else
                            {
                                response.TradeQuantity = tempTradeQuantity;
                            }
                        }
                        break;
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
        /// Updates the object for recalculation.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <param name="response">The response.</param>
        /// <param name="consolidatedNAV">The consolidated nav.</param>
        private static void UpdateObjectForRecalculation(PTTRequestObject requestObject, PTTResponseObject response, decimal consolidatedNAV)
        {
            try
            {
                if (response.TradeQuantity != 0)
                {
                    switch ((PTTChangeType)requestObject.AddOrSet.Value)
                    {
                        case PTTChangeType.Increase:
                            if (response.StartingPosition > 0)
                            {
                                response.OrderSide = FIXConstants.SIDE_Buy;
                            }
                            else if (response.StartingPosition < 0)
                            {
                                response.OrderSide = FIXConstants.SIDE_SellShort;
                            }
                            else
                            {
                                if (useZeroPositionsAsPositiveOrNegativeStatic == 1)
                                {
                                    response.OrderSide = FIXConstants.SIDE_Buy;
                                }
                                else
                                {
                                    response.OrderSide = FIXConstants.SIDE_SellShort;
                                }
                            }
                            break;

                        case PTTChangeType.Decrease:
                            if (response.StartingPosition > 0)
                            {
                                response.OrderSide = FIXConstants.SIDE_Sell;
                            }
                            else if (response.StartingPosition < 0)
                            {
                                response.OrderSide = FIXConstants.SIDE_Buy_Closed;
                            }
                            else
                            {
                                if (useZeroPositionsAsPositiveOrNegativeStatic == 1)
                                {
                                    response.OrderSide = FIXConstants.SIDE_Sell;
                                }
                                else
                                {
                                    response.OrderSide = FIXConstants.SIDE_Buy_Closed;
                                }
                            }
                            break;

                        case PTTChangeType.Set:
                            if (response.StartingPosition > 0)
                            {
                                if (response.PercentageType < 0)
                                {
                                    response.OrderSide = FIXConstants.SIDE_Sell;
                                }
                                else
                                {
                                    response.OrderSide = FIXConstants.SIDE_Buy;
                                }
                            }
                            else if (response.StartingPosition < 0)
                            {
                                if (response.PercentageType < 0)
                                {
                                    response.OrderSide = FIXConstants.SIDE_Buy_Closed;
                                }
                                else
                                {
                                    response.OrderSide = FIXConstants.SIDE_SellShort;
                                }
                            }
                            else
                            {
                                if (useZeroPositionsAsPositiveOrNegativeStatic == 1)
                                {
                                    if (response.PercentageType < 0)
                                    {
                                        response.OrderSide = FIXConstants.SIDE_Sell;
                                    }
                                    else
                                    {
                                        response.OrderSide = FIXConstants.SIDE_Buy;
                                    }
                                }
                                else
                                {
                                    if (response.PercentageType < 0)
                                    {
                                        response.OrderSide = FIXConstants.SIDE_Buy_Closed;
                                    }
                                    else
                                    {
                                        response.OrderSide = FIXConstants.SIDE_SellShort;
                                    }
                                }
                            }
                            break;

                        case PTTChangeType.Buy:
                            response.OrderSide = FIXConstants.SIDE_Buy;
                            break;
                        case PTTChangeType.SellShort:
                            response.OrderSide = FIXConstants.SIDE_SellShort;
                            break;
                    }
                }
                else
                {
                    response.OrderSide = null;
                    response.PercentageAllocation = 0;
                }

                response.EndingPosition = response.StartingPosition + (response.TradeQuantity * GetSideMultiplier(response.OrderSide));
                response.EndingValue = Math.Abs(response.EndingPosition * (requestObject.SelectedFeedPrice * response.FxRate) *
                                     Convert.ToDecimal(requestObject.SecMasterBaseObj.Multiplier) *
                                     Convert.ToDecimal(requestObject.SecMasterBaseObj.Delta));
                response.EndingPercentage = consolidatedNAV > 0 ? (response.EndingValue / consolidatedNAV) * 100 : 0;
                response.PercentageType = response.EndingPercentage - response.StartingPercentage;
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
        /// Gets the side multiplier.
        /// </summary>
        public static decimal GetSideMultiplier(string orderSideTagValue)
        {
            try
            {
                int sideMultiplier = -1;
                if (CachedDataManager.GetInstance.IsLongSide(orderSideTagValue))
                {
                    sideMultiplier = 1;
                }
                return sideMultiplier;
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
            return 1;
        }

    }
}