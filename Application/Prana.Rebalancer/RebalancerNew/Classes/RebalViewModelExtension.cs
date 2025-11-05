using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.Utilities.UI.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prana.Rebalancer.RebalancerNew.Classes
{
    internal class RebalViewModelExtension
    {
        #region singleton
        private static volatile RebalViewModelExtension instance;
        private static object syncRoot = new Object();
        private RebalViewModelExtension()
        { }

        public static RebalViewModelExtension Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new RebalViewModelExtension();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        Dictionary<int, Dictionary<string, SecurityDataGridModel>> AccountWiseSecurityTargetPercentage { get; set; }

        internal List<RebalancerModel> RecalculateRebalancerData(List<RebalancerModel> rebalModels, List<AdjustedAccountLevelNAV> adjustedAccountLevelNavDtos)
        {
            AccountWiseSecurityTargetPercentage = new Dictionary<int, Dictionary<string, SecurityDataGridModel>>();
            //Add existing custom rebal models 
            Dictionary<int, Dictionary<string, RebalancerModel>> dictExistingCustomRebalModels = new Dictionary<int, Dictionary<string, RebalancerModel>>();
            Dictionary<int, decimal> accountWiseMarketValue = new Dictionary<int, decimal>();
            List<RebalancerModel> rebalModelsExcludingCustomModelsAndNewlyAddedModels = new List<RebalancerModel>();
            foreach (RebalancerModel rebalModel in rebalModels)
            {
                if (rebalModel.IsCustomModel)
                {
                    if (!dictExistingCustomRebalModels.ContainsKey(rebalModel.AccountId))
                    {
                        dictExistingCustomRebalModels.Add(rebalModel.AccountId, new Dictionary<string, RebalancerModel> { { rebalModel.Symbol, rebalModel } });
                    }
                    else
                    {
                        if (!dictExistingCustomRebalModels[rebalModel.AccountId].ContainsKey(rebalModel.Symbol))
                        {
                            dictExistingCustomRebalModels[rebalModel.AccountId].Add(rebalModel.Symbol, rebalModel);
                        }
                    }
                }
                else
                {
                    if (!rebalModel.IsNewlyAdded)
                    {
                        //Retrieve manual edited trades and their percentage and add them in SecurityDataGridDict.
                        if (rebalModel.IsModified)
                        {
                            if (!AccountWiseSecurityTargetPercentage.ContainsKey(rebalModel.AccountId))
                            {
                                AccountWiseSecurityTargetPercentage.Add(rebalModel.AccountId, new Dictionary<string, SecurityDataGridModel> { { rebalModel.Symbol, RebalancerMapper.Instance.GetSecurityDataFromRebalancerModel(rebalModel) } });
                            }
                            else
                            {
                                if (!AccountWiseSecurityTargetPercentage[rebalModel.AccountId].ContainsKey(rebalModel.Symbol))
                                {
                                    AccountWiseSecurityTargetPercentage[rebalModel.AccountId].Add(rebalModel.Symbol, RebalancerMapper.Instance.GetSecurityDataFromRebalancerModel(rebalModel));
                                }
                            }
                        }
                        if (!accountWiseMarketValue.ContainsKey(rebalModel.AccountId))
                            accountWiseMarketValue.Add(rebalModel.AccountId, rebalModel.CurrentMarketValueBase);
                        else
                            accountWiseMarketValue[rebalModel.AccountId] += rebalModel.CurrentMarketValueBase;
                        rebalModelsExcludingCustomModelsAndNewlyAddedModels.Add(rebalModel);
                    }
                }
                rebalModel.TargetPosition = rebalModel.Quantity;
                rebalModel.IsCalculatedModel = false;
            }
            rebalModels = rebalModelsExcludingCustomModelsAndNewlyAddedModels;
            List<RebalancerModel> customRebalModels = new List<RebalancerModel>();
            foreach (AdjustedAccountLevelNAV adjustedAccountLevelNAV in adjustedAccountLevelNavDtos)
            {
                if (accountWiseMarketValue.ContainsKey(adjustedAccountLevelNAV.AccountId))
                    adjustedAccountLevelNAV.CurrentSecuritiesMarketValue = adjustedAccountLevelNAV.TargetSecuritiesMarketValue = adjustedAccountLevelNAV.MarketValueForCalculation = accountWiseMarketValue[adjustedAccountLevelNAV.AccountId];
                else
                    adjustedAccountLevelNAV.CurrentSecuritiesMarketValue = adjustedAccountLevelNAV.TargetSecuritiesMarketValue = adjustedAccountLevelNAV.MarketValueForCalculation = 0;
                //Calculate cash flow excess/cash flow needed.
                List<RebalancerModel> accountCustomRebalModels = RebalancerMapper.Instance.GetCustomRebalModels(adjustedAccountLevelNAV, false);
                customRebalModels.AddRange(accountCustomRebalModels);
                rebalModels.AddRange(accountCustomRebalModels);
                decimal customModelsMV = accountCustomRebalModels.Sum(x => x.CurrentMarketValueBase);
                //If new custom model added then add its market value in account securities market value;
                adjustedAccountLevelNAV.CurrentSecuritiesMarketValue += customModelsMV;
                adjustedAccountLevelNAV.TargetSecuritiesMarketValue += customModelsMV;
                adjustedAccountLevelNAV.MarketValueForCalculation += customModelsMV;

                adjustedAccountLevelNAV.CashFlow = 0;
                adjustedAccountLevelNAV.CashFlowNeeded = 0;
            }

            //Maintain unlocked/locked state for custom rebal models.
            foreach (RebalancerModel rebalModel in customRebalModels)
            {
                if (dictExistingCustomRebalModels.ContainsKey(rebalModel.AccountId) && dictExistingCustomRebalModels[rebalModel.AccountId].ContainsKey(rebalModel.Symbol))
                {
                    rebalModel.IsLock = dictExistingCustomRebalModels[rebalModel.AccountId][rebalModel.Symbol].IsLock;
                }
            }
            return rebalModels;
        }

        internal bool ValidateRebalancerData(List<RebalancerModel> rebalModels, List<AdjustedAccountLevelNAV> adjustedAccountLevelNavDtos, ref Dictionary<int, Dictionary<string, SecurityDataGridModel>> accountWiseSecurityDataGridDictLocal, decimal cashFlow, bool isModifyRebalanceAllowed, ref StringBuilder errorMessage, ModelPortfolioDto modelPortfolioDto = null)
        {
            decimal totalMV = adjustedAccountLevelNavDtos.Sum(x => x.CurrentSecuritiesMarketValue);
            List<PortfolioDto> portfolioDtos = modelPortfolioDto != null && modelPortfolioDto.ModelPortfolioType == RebalancerEnums.ModelPortfolioType.ModelPortfolio ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<PortfolioDto>>(modelPortfolioDto.ModelPortfolioData) : null;
            PortfolioDto cashPortfolioDto = portfolioDtos != null ? portfolioDtos.FirstOrDefault(p => p.Symbol.Equals("Cash", StringComparison.CurrentCultureIgnoreCase)) : null;
            if (cashFlow < 0 && Math.Abs(cashFlow) > totalMV)
            {
                errorMessage.Append("Cash outflow cannot be greater than account(s) NAV.");
                return false;
            }
            if (isModifyRebalanceAllowed)
            {
                AccountWiseSecurityTargetPercentage = new Dictionary<int, Dictionary<string, SecurityDataGridModel>>();
            }
            //Dictionary to store rebalancer models of securities for all accounts, symbol_side wise
            Dictionary<int, Dictionary<string, RebalancerModel>> accountWiseRebalModels = new Dictionary<int, Dictionary<string, RebalancerModel>>();
            //Add prorata basis securities to all accounts
            accountWiseSecurityDataGridDictLocal = SetAccountWiseSecurityData(ref accountWiseSecurityDataGridDictLocal, adjustedAccountLevelNavDtos, ref errorMessage);
            if (errorMessage.Length > 0)
                return false;
            foreach (RebalancerModel rebalModel in rebalModels)
            {
                //Set account wise % of symbols, this dictionary will be used to calculate cash flow needed in case of rebalance across securities.
                if (!accountWiseRebalModels.ContainsKey(rebalModel.AccountId))
                {
                    string key = rebalModel.Symbol + Seperators.SEPERATOR_13 + rebalModel.Side.ToString();
                    accountWiseRebalModels.Add(rebalModel.AccountId, new Dictionary<string, RebalancerModel> { { key, rebalModel } });
                }
                else
                {
                    string key = rebalModel.Symbol + Seperators.SEPERATOR_13 + rebalModel.Side.ToString();
                    if (!accountWiseRebalModels[rebalModel.AccountId].ContainsKey(key))
                    {
                        accountWiseRebalModels[rebalModel.AccountId].Add(key, rebalModel);
                    }
                }
            }

            if (!ValidateCashTargetRule(accountWiseRebalModels, accountWiseSecurityDataGridDictLocal, ref errorMessage))
                return false;

            List<AdjustedAccountLevelNAV> accountsWithExcessCash = new List<AdjustedAccountLevelNAV>();
            List<AdjustedAccountLevelNAV> accountsWithCashNeeded = new List<AdjustedAccountLevelNAV>();
            List<RebalancerModel> customRebalModels = new List<RebalancerModel>();
            decimal totalNAV = adjustedAccountLevelNavDtos.Sum(x => x.CurrentTotalNAV);
            foreach (AdjustedAccountLevelNAV adjustedAccountLevelNAV in adjustedAccountLevelNavDtos)
            {
                adjustedAccountLevelNAV.CashFlow = 0;
                adjustedAccountLevelNAV.CashFlowNeeded = 0;
                if (!isModifyRebalanceAllowed)
                {
                    //Set cash flow to each account
                    if (adjustedAccountLevelNAV.IsCustomCashFlow)
                    {
                        adjustedAccountLevelNAV.CashFlow = adjustedAccountLevelNAV.CustomCashFlow;
                    }
                    else
                    {
                        //Distribute cash among multiple accounts based on there current % in NAV contribution
                        if (totalNAV != 0)
                            adjustedAccountLevelNAV.CashFlow = (adjustedAccountLevelNAV.CurrentTotalNAV / totalNAV) * cashFlow;
                        else
                            adjustedAccountLevelNAV.CashFlow = cashFlow / adjustedAccountLevelNavDtos.Count;
                    }
                }

                //Calculate CashFlow for cash if cash target percent is set
                if (RebalancerCache.Instance.GetTradingRules().IsSetCashTarget)
                {
                    CheckAccountCashLockedOrNot(accountWiseRebalModels, adjustedAccountLevelNAV, ref errorMessage);
                    if (errorMessage.Length > 0)
                    {
                        errorMessage.Clear();
                        errorMessage.Append(RebalancerConstants.MSG_CASH_LOCK_VALIDATION);
                        return false;
                    }
                    adjustedAccountLevelNAV.CashFlowNeeded += CalculateCashFlowNeededOrExcess(accountWiseRebalModels[adjustedAccountLevelNAV.AccountId], adjustedAccountLevelNAV.TargetTotalNAV, ref errorMessage);
                }

                if (accountWiseSecurityDataGridDictLocal.Count > 0 && adjustedAccountLevelNAV.CurrentTotalNAV != 0)
                {
                    if (accountWiseSecurityDataGridDictLocal.ContainsKey(adjustedAccountLevelNAV.AccountId))
                    {
                        adjustedAccountLevelNAV.CashFlowNeeded += CalculateCashFlowNeededOrExcess(accountWiseSecurityDataGridDictLocal[adjustedAccountLevelNAV.AccountId], accountWiseRebalModels, adjustedAccountLevelNAV, ref errorMessage);
                    }
                    if (accountWiseSecurityDataGridDictLocal.ContainsKey(0))
                    {
                        adjustedAccountLevelNAV.CashFlowNeeded += CalculateCashFlowNeededOrExcess(accountWiseSecurityDataGridDictLocal[0], accountWiseRebalModels, adjustedAccountLevelNAV, ref errorMessage);
                    }
                }

                //Add entered cash flow in adjustedAccountLevelNAV.CashFlowNeededs
                adjustedAccountLevelNAV.CashFlowNeeded += adjustedAccountLevelNAV.CashFlow;

                if (portfolioDtos != null && !isModifyRebalanceAllowed)
                {

                    if (cashPortfolioDto != null)
                    {
                        if (modelPortfolioDto.ReferenceId == (int)RebalancerEnums.ModelType.TargetCash)
                        {
                            decimal cashContribution = (cashPortfolioDto.TargetPercentage * adjustedAccountLevelNAV.CashFlowNeeded) / 100;
                            string longSymbolKey = "Cash" + Seperators.SEPERATOR_13 + PositionType.Long.ToString();
                            if (accountWiseRebalModels[adjustedAccountLevelNAV.AccountId].ContainsKey(longSymbolKey))
                            {
                                cashContribution += accountWiseRebalModels[adjustedAccountLevelNAV.AccountId][longSymbolKey].CurrentMarketValueBase;
                            }

                            if (cashContribution < 0 && !RebalancerCache.Instance.GetTradingRules().IsSellToRaiseCash)
                            {
                                errorMessage.Append(string.Format("Enough Cash is required for account {0}.", adjustedAccountLevelNAV.AccountName));
                            }
                        }
                        else
                        {
                            if (cashPortfolioDto.TargetPercentage < 0)
                                errorMessage.Append(string.Format("Cash can't be negative for account {0}.", adjustedAccountLevelNAV.AccountName));
                        }
                    }

                    if (modelPortfolioDto.ReferenceId == (int)RebalancerEnums.ModelType.TargetCash && adjustedAccountLevelNAV.CashFlowNeeded > 0)
                    {
                        foreach (PortfolioDto portfolioDto in portfolioDtos)
                        {
                            string longSymbolKey = portfolioDto.Symbol + Seperators.SEPERATOR_13 + PositionType.Long.ToString();
                            string shortSymbolKey = portfolioDto.Symbol + Seperators.SEPERATOR_13 + PositionType.Short.ToString();
                            bool isLongSecurityAvailable = false;
                            bool isShortSecurityAvailable = false;
                            if (accountWiseRebalModels[adjustedAccountLevelNAV.AccountId].ContainsKey(longSymbolKey))
                                isLongSecurityAvailable = true;
                            if (accountWiseRebalModels[adjustedAccountLevelNAV.AccountId].ContainsKey(shortSymbolKey))
                                isShortSecurityAvailable = true;
                            string keyToUse = string.Empty;
                            if (isLongSecurityAvailable)
                                keyToUse = longSymbolKey;
                            else if (isShortSecurityAvailable)
                                keyToUse = shortSymbolKey;
                            decimal marketValue = 0;
                            if (!string.IsNullOrEmpty(keyToUse) && !accountWiseRebalModels[adjustedAccountLevelNAV.AccountId][keyToUse].IsLock)
                            {
                                marketValue = (adjustedAccountLevelNAV.CashFlowNeeded * portfolioDto.TargetPercentage / 100) + accountWiseRebalModels[adjustedAccountLevelNAV.AccountId][keyToUse].CurrentMarketValueBase;
                            }
                            else
                            {
                                marketValue = adjustedAccountLevelNAV.CashFlowNeeded * portfolioDto.TargetPercentage / 100;
                            }

                            if (marketValue < 0 && RebalancerCache.Instance.GetTradingRules().IsNoShorting && !RebalancerCache.Instance.GetTradingRules().IsSellToRaiseCash)
                                errorMessage.Append(string.Format("Shorts are not allowed (Account: {0}, Security: {1}).", adjustedAccountLevelNAV.AccountName, portfolioDto.Symbol));
                            else if (!string.IsNullOrEmpty(keyToUse) && marketValue < 0 && accountWiseRebalModels[adjustedAccountLevelNAV.AccountId][keyToUse].IsLock && !RebalancerCache.Instance.GetTradingRules().IsSellToRaiseCash)
                                errorMessage.Append(string.Format("Shorts are not allowed for locked Security (Account: {0}, Security: {1}).", adjustedAccountLevelNAV.AccountName, portfolioDto.Symbol));
                        }
                    }
                }

                if (adjustedAccountLevelNAV.CashFlowNeeded > 0)
                    accountsWithExcessCash.Add(adjustedAccountLevelNAV);
                else if (adjustedAccountLevelNAV.CashFlowNeeded < 0)
                    accountsWithCashNeeded.Add(adjustedAccountLevelNAV);
            }
            if (errorMessage.Length > 0)
            {
                errorMessage.Append("Please change rebalance strategy.");
                return false;
            }
            if (accountWiseSecurityDataGridDictLocal.Count > 0 || RebalancerCache.Instance.GetTradingRules().IsSetCashTarget)
            {
                bool result = EvaluateCashExcess(ref errorMessage, accountsWithExcessCash, accountsWithCashNeeded, accountWiseRebalModels, accountWiseSecurityDataGridDictLocal);
                return result;
            }
            return true;
        }

        private bool EvaluateCashExcess(ref StringBuilder errorMessage, List<AdjustedAccountLevelNAV> accountsWithExcessCash, List<AdjustedAccountLevelNAV> accountsWithCashNeeded, Dictionary<int, Dictionary<string, RebalancerModel>> accountWiseRebalModels, Dictionary<int, Dictionary<string, SecurityDataGridModel>> accountWiseSecurityDataGridDictLocal)
        {
            TradingRules tradingRules = RebalancerCache.Instance.GetTradingRules();
            if (accountsWithExcessCash.Count > 0 && accountsWithCashNeeded.Count > 0)
            {
                errorMessage.Append("Conflicting cash generation, details are: ");
                errorMessage.Append(Environment.NewLine);
                int accountsCounter = 1;
                foreach (AdjustedAccountLevelNAV adjustedAccountLevelNAV in accountsWithExcessCash)
                {
                    //Show details of only 5 accounts.
                    if (accountsCounter == 5)
                    {
                        errorMessage.Append("Conflicts with few more accounts.");
                        errorMessage.Append(Environment.NewLine);
                        break;
                    }
                    errorMessage.Append(string.Format("{0} generates excess cash around {1}.", adjustedAccountLevelNAV.AccountName, Math.Round(adjustedAccountLevelNAV.CashFlowNeeded)));
                    errorMessage.Append(Environment.NewLine);
                    accountsCounter++;
                }
                if (accountsCounter < 5)
                {
                    foreach (AdjustedAccountLevelNAV adjustedAccountLevelNAV in accountsWithCashNeeded)
                    {
                        //Show details of only 5 accounts.
                        if (accountsCounter == 5)
                        {
                            errorMessage.Append("Conflicts with few more accounts.");
                            errorMessage.Append(Environment.NewLine);
                            break;
                        }
                        errorMessage.Append(string.Format("{0} requires excess cash around {1}.", adjustedAccountLevelNAV.AccountName, Math.Round(Math.Abs(adjustedAccountLevelNAV.CashFlowNeeded))));
                        errorMessage.Append(Environment.NewLine);
                        accountsCounter++;
                    }
                }
                errorMessage.Append("Please change rebalance strategy.");
                return false;
            }
            else if (accountsWithExcessCash.Count > 0)
            {
                if (!tradingRules.IsReInvestCash)
                {
                    tradingRules.IsIncreaseCashPosition = true;
                    foreach (AdjustedAccountLevelNAV adjustedAccountLevelNAV in accountsWithExcessCash)
                    {
                        CheckAccountCashLockedOrNot(accountWiseRebalModels, adjustedAccountLevelNAV, ref errorMessage);
                    }
                }
            }
            else if (accountsWithCashNeeded.Count > 0)
            {
                foreach (AdjustedAccountLevelNAV adjustedAccountLevelNAV in accountsWithCashNeeded)
                {
                    if (RebalancerCache.Instance.GetTradingRules().IsSetCashTarget && !RebalancerCache.Instance.GetTradingRules().IsSellToRaiseCash)
                    {
                        errorMessage.Append(RebalancerConstants.MSG_CASH_TARGET_PERCENT_VALIDATION);
                        return false;
                    }
                    else if (!RebalancerCache.Instance.GetTradingRules().IsNegativeCashAllowed && !RebalancerCache.Instance.GetTradingRules().IsSellToRaiseCash)
                    {
                        System.Windows.Forms.DialogResult messageBoxResult = WindowsCustomMessageBox.Show("Rebalance require additional cash, Do you want to: ", RebalancerConstants.CAP_NIRVANACAPTION, WindowsCustomMessageBox.Buttons.YesNoCancel, "Allow Negative Cash", "Sell to Raise Cash", "Cancel", WindowsCustomMessageBox.Icon.Exclamation);
                        if (messageBoxResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            tradingRules.IsNegativeCashAllowed = true;
                        }
                        else if (messageBoxResult == System.Windows.Forms.DialogResult.No)
                        {
                            tradingRules.IsSellToRaiseCash = true;
                        }
                        else if (messageBoxResult == System.Windows.Forms.DialogResult.Cancel)
                            return false;
                    }
                    else if (RebalancerCache.Instance.GetTradingRules().IsSellToRaiseCash)
                    {
                        decimal availableMVToUse = 0;
                        if (accountWiseRebalModels.ContainsKey(adjustedAccountLevelNAV.AccountId))
                        {
                            foreach (KeyValuePair<string, RebalancerModel> kvp in accountWiseRebalModels[adjustedAccountLevelNAV.AccountId])
                            {
                                if (!kvp.Value.IsLock && kvp.Value.Side == PositionType.Long)
                                {
                                    if (!(accountWiseSecurityDataGridDictLocal.ContainsKey(adjustedAccountLevelNAV.AccountId) && accountWiseSecurityDataGridDictLocal[adjustedAccountLevelNAV.AccountId].ContainsKey(kvp.Value.Symbol)))
                                        availableMVToUse += kvp.Value.CurrentMarketValueBase;
                                }
                            }
                        }
                        if (availableMVToUse < Math.Abs(adjustedAccountLevelNAV.CashFlowNeeded))
                        {
                            if (RebalancerCache.Instance.GetTradingRules().IsNoShorting)
                            {
                                decimal additionalCashNeeded = Math.Abs(adjustedAccountLevelNAV.CashFlowNeeded + availableMVToUse);
                                errorMessage.Append(string.Format("Additional cash around {0} needed for account {1} to complete the Rebalance.", Math.Round(additionalCashNeeded), adjustedAccountLevelNAV.AccountName));
                                errorMessage.Append(Environment.NewLine);
                            }
                        }
                    }
                    if (RebalancerCache.Instance.GetTradingRules().IsNegativeCashAllowed)
                    {
                        CheckAccountCashLockedOrNot(accountWiseRebalModels, adjustedAccountLevelNAV, ref errorMessage);
                    }
                }
            }
            if (errorMessage.Length > 0)
            {
                errorMessage.Append("Please provide further instructions.");
                return false;
            }
            RebalancerCache.Instance.SetTradingRules(tradingRules);
            return true;
        }

        private void CheckAccountCashLockedOrNot(Dictionary<int, Dictionary<string, RebalancerModel>> accountWiseRebalModels, AdjustedAccountLevelNAV adjustedAccountLevelNAV, ref StringBuilder errorMessage)
        {
            string cashLongSymbolKey = RebalancerConstants.CONST_CASH + Seperators.SEPERATOR_13 + PositionType.Long.ToString();
            string cashShortSymbolKey = RebalancerConstants.CONST_CASH + Seperators.SEPERATOR_13 + PositionType.Short.ToString();
            RebalancerModel cashLongRebalModel = null;
            RebalancerModel cashShortRebalModel = null;
            if (accountWiseRebalModels.ContainsKey(adjustedAccountLevelNAV.AccountId))
            {
                if (accountWiseRebalModels[adjustedAccountLevelNAV.AccountId].ContainsKey(cashLongSymbolKey))
                    cashLongRebalModel = accountWiseRebalModels[adjustedAccountLevelNAV.AccountId][cashLongSymbolKey];
                if (accountWiseRebalModels[adjustedAccountLevelNAV.AccountId].ContainsKey(cashShortSymbolKey))
                    cashShortRebalModel = accountWiseRebalModels[adjustedAccountLevelNAV.AccountId][cashShortSymbolKey];
            }
            if ((cashLongRebalModel != null && cashLongRebalModel.IsLock) || (cashShortRebalModel != null && cashShortRebalModel.IsLock))
            {
                errorMessage.Append(string.Format("Cash is locked for account {0}, first unlock cash.", adjustedAccountLevelNAV.AccountName));
                errorMessage.Append(Environment.NewLine);
            }
        }

        private Dictionary<int, Dictionary<string, SecurityDataGridModel>> SetAccountWiseSecurityData(ref Dictionary<int, Dictionary<string, SecurityDataGridModel>> accountWiseSecurityDataGridDictLocal, List<AdjustedAccountLevelNAV> adjustedAccountLevelNavDtos, ref StringBuilder errorMessage)
        {
            //If securities added on Prorata basis then add to all accounts with same target percentage and price.
            if (accountWiseSecurityDataGridDictLocal.ContainsKey(0))
            {
                foreach (AdjustedAccountLevelNAV adjustedAccountLevelNAV in adjustedAccountLevelNavDtos)
                {

                    if (!accountWiseSecurityDataGridDictLocal.ContainsKey(adjustedAccountLevelNAV.AccountId))
                    {
                        accountWiseSecurityDataGridDictLocal.Add(adjustedAccountLevelNAV.AccountId, accountWiseSecurityDataGridDictLocal[0]);
                    }
                    else
                    {
                        foreach (KeyValuePair<string, SecurityDataGridModel> kvpSecuritiesData in accountWiseSecurityDataGridDictLocal[0])
                        {
                            if (!accountWiseSecurityDataGridDictLocal[adjustedAccountLevelNAV.AccountId].ContainsKey(kvpSecuritiesData.Key))
                                accountWiseSecurityDataGridDictLocal[adjustedAccountLevelNAV.AccountId].Add(kvpSecuritiesData.Key, kvpSecuritiesData.Value);
                        }
                    }
                }
            }
            if (accountWiseSecurityDataGridDictLocal.ContainsKey(0))
            {
                accountWiseSecurityDataGridDictLocal.Remove(0);
            }

            foreach (KeyValuePair<int, Dictionary<string, SecurityDataGridModel>> kvpRebalGridSecurityDataWithAccount in AccountWiseSecurityTargetPercentage)
            {
                foreach (KeyValuePair<string, SecurityDataGridModel> kvpRebalGridSecurityData in kvpRebalGridSecurityDataWithAccount.Value)
                {
                    if (accountWiseSecurityDataGridDictLocal.ContainsKey(kvpRebalGridSecurityDataWithAccount.Key))
                    {
                        if (accountWiseSecurityDataGridDictLocal[kvpRebalGridSecurityDataWithAccount.Key].ContainsKey(kvpRebalGridSecurityData.Key))
                        {
                            errorMessage.Append(string.Format("For account {0}, security {1} exists in rebalancer grid and rebalance across securities, first remove it from either.", kvpRebalGridSecurityData.Value.AccountOrGroupName, kvpRebalGridSecurityData.Value.Symbol));
                            errorMessage.Append(Environment.NewLine);
                        }
                        else
                        {
                            accountWiseSecurityDataGridDictLocal[kvpRebalGridSecurityDataWithAccount.Key].Add(kvpRebalGridSecurityData.Key, kvpRebalGridSecurityData.Value);
                        }
                    }
                    else
                    {
                        accountWiseSecurityDataGridDictLocal.Add(kvpRebalGridSecurityDataWithAccount.Key, new Dictionary<string, SecurityDataGridModel> { { kvpRebalGridSecurityData.Key, kvpRebalGridSecurityData.Value } });
                    }
                }
            }
            return accountWiseSecurityDataGridDictLocal;
        }

        /// <summary>
        /// Calculate Cash Flow Needed Or Excess to set Cash target
        /// </summary>       
        private decimal CalculateCashFlowNeededOrExcess(Dictionary<string, RebalancerModel> rebalModels, decimal targetTotalNAV, ref StringBuilder errorMessage)
        {
            decimal cashFlowNeededOrExcess = 0;
            try
            {
                decimal cashTarget = (decimal)RebalancerCache.Instance.GetTradingRules().CashTarget;
                string cashLongSymbolKey = RebalancerConstants.CONST_CASH + Seperators.SEPERATOR_13 + PositionType.Long.ToString();
                string cashShortSymbolKey = RebalancerConstants.CONST_CASH + Seperators.SEPERATOR_13 + PositionType.Short.ToString();
                bool isLongSecurityAvailable = rebalModels.ContainsKey(cashLongSymbolKey);
                bool isShortSecurityAvailable = rebalModels.ContainsKey(cashShortSymbolKey);
                string keyToUse = isLongSecurityAvailable ? cashLongSymbolKey : isShortSecurityAvailable ? cashShortSymbolKey : string.Empty;

                if (isLongSecurityAvailable || isShortSecurityAvailable)
                {
                    decimal currentPercentage = rebalModels[keyToUse].TargetPercentage;
                    cashFlowNeededOrExcess = ((currentPercentage - cashTarget) * targetTotalNAV) / 100;
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
            return cashFlowNeededOrExcess;
        }

        private decimal CalculateCashFlowNeededOrExcess(Dictionary<string, SecurityDataGridModel> dictAccountWiseSecurityData, Dictionary<int, Dictionary<string, RebalancerModel>> accountWiseRebalModels, AdjustedAccountLevelNAV adjustedAccountLevelNAV, ref StringBuilder errorMessage)
        {
            decimal cashFlowNeededOrExcess = 0;

            foreach (KeyValuePair<string, SecurityDataGridModel> kvpAccountWiseSecurityData in dictAccountWiseSecurityData)
            {
                if (kvpAccountWiseSecurityData.Value.AccountOrGroupId == 0 || kvpAccountWiseSecurityData.Value.AccountOrGroupId == adjustedAccountLevelNAV.AccountId)
                {
                    string longSymbolKey = kvpAccountWiseSecurityData.Value.Symbol + Seperators.SEPERATOR_13 + PositionType.Long.ToString();
                    string shortSymbolKey = kvpAccountWiseSecurityData.Value.Symbol + Seperators.SEPERATOR_13 + PositionType.Short.ToString();
                    bool isLongSecurityAvailable = false;
                    bool isShortSecurityAvailable = false;
                    if (accountWiseRebalModels[adjustedAccountLevelNAV.AccountId].ContainsKey(longSymbolKey))
                        isLongSecurityAvailable = true;
                    if (accountWiseRebalModels[adjustedAccountLevelNAV.AccountId].ContainsKey(shortSymbolKey))
                        isShortSecurityAvailable = true;
                    string keyToUse = string.Empty;
                    if (isLongSecurityAvailable)
                        keyToUse = longSymbolKey;
                    else if (isShortSecurityAvailable)
                        keyToUse = shortSymbolKey;
                    if (accountWiseRebalModels.ContainsKey(adjustedAccountLevelNAV.AccountId)
                        &&
                        (isLongSecurityAvailable || isShortSecurityAvailable))
                    {
                        decimal currentPercentage = accountWiseRebalModels[adjustedAccountLevelNAV.AccountId][keyToUse].TargetPercentage;
                        if (kvpAccountWiseSecurityData.Value.IncreaseDecreaseOrSet == RebalancerEnums.RASIncreaseDecreaseOrSet.Increase.ToString())
                        {
                            cashFlowNeededOrExcess += (kvpAccountWiseSecurityData.Value.TargetPercentage * -1 * adjustedAccountLevelNAV.TargetTotalNAV) / 100;
                        }
                        else if (kvpAccountWiseSecurityData.Value.IncreaseDecreaseOrSet == RebalancerEnums.RASIncreaseDecreaseOrSet.Decrease.ToString())
                        {
                            if (RebalancerCache.Instance.GetTradingRules().IsNoShorting && (currentPercentage >= 0) && (currentPercentage < kvpAccountWiseSecurityData.Value.TargetPercentage))
                            {
                                errorMessage.Append(string.Format("Shorts are not allowed (Account: {0}, Security: {1}).", adjustedAccountLevelNAV.AccountName, kvpAccountWiseSecurityData.Value.Symbol));
                                errorMessage.Append(Environment.NewLine);
                            }
                            else
                                cashFlowNeededOrExcess += (kvpAccountWiseSecurityData.Value.TargetPercentage * adjustedAccountLevelNAV.TargetTotalNAV) / 100;
                        }
                        else
                        {
                            if (RebalancerCache.Instance.GetTradingRules().IsNoShorting && (currentPercentage >= 0) && (kvpAccountWiseSecurityData.Value.TargetPercentage != 0) && Math.Sign(currentPercentage) != Math.Sign(kvpAccountWiseSecurityData.Value.TargetPercentage))
                            {
                                errorMessage.Append(string.Format("Shorts are not allowed (Account: {0}, Security: {1}).", adjustedAccountLevelNAV.AccountName, kvpAccountWiseSecurityData.Value.Symbol));
                                errorMessage.Append(Environment.NewLine);
                            }
                            else
                                cashFlowNeededOrExcess += ((currentPercentage - kvpAccountWiseSecurityData.Value.TargetPercentage) * adjustedAccountLevelNAV.TargetTotalNAV) / 100;
                        }
                    }
                    else
                    {
                        decimal targetPercentage = (kvpAccountWiseSecurityData.Value.IncreaseDecreaseOrSet == RebalancerEnums.RASIncreaseDecreaseOrSet.Decrease.ToString() ? -1 * kvpAccountWiseSecurityData.Value.TargetPercentage : kvpAccountWiseSecurityData.Value.TargetPercentage);
                        if (RebalancerCache.Instance.GetTradingRules().IsNoShorting && (targetPercentage < 0))
                        {
                            errorMessage.Append(string.Format("Shorts are not allowed (Account: {0}, Security: {1}).", adjustedAccountLevelNAV.AccountName, kvpAccountWiseSecurityData.Value.Symbol));
                            errorMessage.Append(Environment.NewLine);
                        }
                        else
                            cashFlowNeededOrExcess += (targetPercentage * -1 * adjustedAccountLevelNAV.TargetTotalNAV) / 100;
                    }
                }
            }
            if (!RebalancerCache.Instance.GetTradingRules().IsSetCashTarget)
            {
                string cashLongSymbolKey = RebalancerConstants.CONST_CASH + Seperators.SEPERATOR_13 + PositionType.Long.ToString();
                if (accountWiseRebalModels.ContainsKey(adjustedAccountLevelNAV.AccountId) && accountWiseRebalModels[adjustedAccountLevelNAV.AccountId].ContainsKey(cashLongSymbolKey))
                {
                    RebalancerModel cashRebalModel = accountWiseRebalModels[adjustedAccountLevelNAV.AccountId][cashLongSymbolKey];
                    if (!cashRebalModel.IsLock)
                    {
                        if (cashFlowNeededOrExcess < 0)
                        {
                            if (Math.Abs(cashFlowNeededOrExcess) < cashRebalModel.CurrentMarketValueBase)
                                cashFlowNeededOrExcess = 0;
                            else
                                cashFlowNeededOrExcess += cashRebalModel.CurrentMarketValueBase;
                        }
                    }
                }
            }
            return cashFlowNeededOrExcess;
        }

        /// <summary>
        /// Display warning if all the holdings and cash is set to some % which does not sum up to 100%
        /// </summary>
        private bool ValidateCashTargetRule(Dictionary<int, Dictionary<string, RebalancerModel>> accountWiseRebalModels, Dictionary<int, Dictionary<string, SecurityDataGridModel>> accountWiseSecurityDataGridDictLocal, ref StringBuilder errorMsg)
        {
            try
            {
                if (RebalancerCache.Instance.GetTradingRules().IsSetCashTarget)
                {
                    foreach (var accountRebalModels in accountWiseRebalModels)
                    {
                        bool isAllHoldingsSet = true;
                        decimal totalAccountTargetPercent = 0;
                        if (accountWiseSecurityDataGridDictLocal.ContainsKey(accountRebalModels.Key))
                        {
                            Dictionary<string, SecurityDataGridModel> securityDataGrid = accountWiseSecurityDataGridDictLocal[accountRebalModels.Key];
                            foreach (var rebalModel in accountRebalModels.Value)
                            {
                                if (!rebalModel.Value.IsCustomModel)
                                {
                                    string symbol = rebalModel.Value.Symbol;
                                    if (securityDataGrid.ContainsKey(symbol) && securityDataGrid[symbol].IncreaseDecreaseOrSet.Equals(RebalancerEnums.RASIncreaseDecreaseOrSet.Set.ToString()))
                                        totalAccountTargetPercent += securityDataGrid[symbol].TargetPercentage;
                                    else
                                        isAllHoldingsSet = false;
                                }
                            }
                            if (isAllHoldingsSet && Math.Abs(totalAccountTargetPercent + (decimal)RebalancerCache.Instance.GetTradingRules().CashTarget) != 100)
                            {
                                errorMsg.Append(RebalancerConstants.MSG_TARGET_PERCENT_SUM_VALIDATION);
                                return false;
                            }
                        }         
                    }
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
            return true;
        }
    }
}
