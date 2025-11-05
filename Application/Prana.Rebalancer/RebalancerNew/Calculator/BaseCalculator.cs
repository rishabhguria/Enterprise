using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prana.Rebalancer.RebalancerNew.Calculator
{
    public class BaseCalculator : IBaseCalculator
    {
        public IRebalCalculator CashFlowCalculatorInstance { get; set; }

        public IRebalCalculator ModelPortfolioCalculatorInstance { get; set; }

        public IRebalCalculator RASCalculationInstance { get; set; }

        public BaseCalculator()
        {
            CashFlowCalculatorInstance = new CashFlowCalculator();
            ModelPortfolioCalculatorInstance = new ModelPortfolioCalculator();
            RASCalculationInstance = new RASCalculator();
        }

        public bool CalculateData(CalculationModel calculationModel, List<AdjustedAccountLevelNAV> adjustedAccountLevelNavDtos, ref StringBuilder message)
        {
            bool result = false;
            //key=> AccountId, Key=> IsLocked, Value=> RebalancerModel
            Dictionary<int, List<RebalancerModel>> dictAccountPositions =
                calculationModel.RebalancerModels.GroupBy(d => d.AccountId).ToDictionary(g => g.Key, value => value.ToList());
            //GetAccountWisePositions(calculationModel.RebalancerModels);
            Dictionary<int, AdjustedAccountLevelNAV> dictAccountLevelNAV = adjustedAccountLevelNavDtos.ToDictionary(key => key.AccountId, value => value);
            List<RebalancerModel> allRebalModels = new List<RebalancerModel>();
            foreach (KeyValuePair<int, List<RebalancerModel>> accountRebalItems in dictAccountPositions)
            {
                //positions of the account
                calculationModel.RebalancerModels = dictAccountPositions[accountRebalItems.Key];
                AdjustedAccountLevelNAV adjustedAccountWiseNAV = dictAccountLevelNAV[accountRebalItems.Key];
                SubCalculationModel accountLevelcalculationModel = new SubCalculationModel(calculationModel, adjustedAccountWiseNAV);
                if (accountLevelcalculationModel.UnLockedRebalModels.Count > 0)
                {
                    if (accountLevelcalculationModel.ModelPortfolioId != 0)
                    {
                        ModelPortfolioCalculatorInstance.CalculateData(accountLevelcalculationModel);
                        result = true;
                    }
                    else if ((accountLevelcalculationModel.SecurityDataGridDict != null && accountLevelcalculationModel.SecurityDataGridDict.Count > 0) || accountLevelcalculationModel.IsUserModifiedRebalModelsAvailable || RebalancerCache.Instance.GetTradingRules().IsSetCashTarget)
                    {
                        //Target% should be calculated on market value of all securities.
                        adjustedAccountWiseNAV.MarketValueForCalculation = accountLevelcalculationModel.RebalancerModels.Sum(x => x.TargetMarketValueBase);
                        RASCalculationInstance.CalculateData(accountLevelcalculationModel);
                        result = true;
                    }
                    else if (accountLevelcalculationModel.CashFlow != 0)
                    {
                        CashFlowCalculatorInstance.CalculateData(accountLevelcalculationModel);
                        result = true;
                    }
                    if (result)
                    {
                        foreach (RebalancerModel rebalancerModel in accountLevelcalculationModel.RebalancerModels)
                        {
                            if (rebalancerModel.Symbol == RebalancerConstants.CONST_CASH)
                            {
                                if (rebalancerModel.Side == PositionType.Long && accountLevelcalculationModel.CashLongRebalanceModel == null)
                                    accountLevelcalculationModel.CashLongRebalanceModel = rebalancerModel;
                                else if (rebalancerModel.Side == PositionType.Short && accountLevelcalculationModel.CashShortRebalanceModel == null)
                                    accountLevelcalculationModel.CashShortRebalanceModel = rebalancerModel;
                                if(RebalancerCache.Instance.GetTradingRules().IsSetCashTarget)
                                    rebalancerModel.IsLock = false;
                            }
                            else
                                rebalancerModel.RoundOffTargetPosition();
                        }
                    }
                }
                accountLevelcalculationModel.RebalancerModels = AddUpdateRemainingCashFlow(accountLevelcalculationModel.RebalancerModels, adjustedAccountWiseNAV, accountLevelcalculationModel.CashLongRebalanceModel, accountLevelcalculationModel.CashShortRebalanceModel);
                if (result)
                {
                    foreach (RebalancerModel rebalancerModel in accountLevelcalculationModel.RebalancerModels)
                    {
                        rebalancerModel.RaisePropertyChanged("TargetPercentage");
                        rebalancerModel.RaisePropertyChanged("CurrentPercentage");
                        rebalancerModel.IsCalculatedModel = true;
                    }
                }
                //Resetting MarketValueForCalculation to calculate the target% corerctly.
                accountLevelcalculationModel.AccountWiseNAV.MarketValueForCalculation = accountLevelcalculationModel.AccountWiseNAV.TargetSecuritiesMarketValue;
                allRebalModels.AddRange(accountLevelcalculationModel.RebalancerModels);
                adjustedAccountWiseNAV.CashFlow = 0;
            }
            calculationModel.RebalancerModels = allRebalModels;
            return result;
        }

        private List<RebalancerModel> AddUpdateRemainingCashFlow(List<RebalancerModel> rebalModels, AdjustedAccountLevelNAV adjustedAccountLevelNAVItem, RebalancerModel cashLongRebalModel, RebalancerModel cashShortRebalModel)
        {
            if (adjustedAccountLevelNAVItem.CashFlow != 0)
            {
                bool isAddNewCashRebalModel = true;
                decimal cashFlowImpactFactor = 0;
                if (RebalancerCache.Instance.GetCashFlowImpact() == RebalancerEnums.CashFlowImpactOnNAV.NoImpact)
                {
                    cashFlowImpactFactor += adjustedAccountLevelNAVItem.CurrentSecuritiesMarketValue - adjustedAccountLevelNAVItem.TargetSecuritiesMarketValue;
                }
                decimal cashToAdjust = cashFlowImpactFactor != 0 ? cashFlowImpactFactor : adjustedAccountLevelNAVItem.CashFlow;
                if (cashToAdjust > 0 && cashShortRebalModel != null)
                {
                    if (cashShortRebalModel.TargetPosition >= cashToAdjust)
                    {
                        cashShortRebalModel.TargetPosition -= cashToAdjust;
                        cashToAdjust = 0;
                    }
                    else
                    {
                        cashToAdjust -= cashShortRebalModel.TargetPosition;
                        cashShortRebalModel.TargetPosition = 0;
                    }
                }
                if (cashToAdjust == 0)
                {
                    isAddNewCashRebalModel = false;
                }
                else if (cashLongRebalModel != null)
                {
                    decimal updatedTargetPosition = cashLongRebalModel.TargetPosition + cashToAdjust;
                    if (cashLongRebalModel.Quantity == 0 && cashLongRebalModel.TargetPosition == 0)
                    {
                        cashLongRebalModel.Side = updatedTargetPosition > 0 ? PositionType.Long : PositionType.Short;
                        cashLongRebalModel.TargetPosition = Math.Abs(updatedTargetPosition);
                        isAddNewCashRebalModel = false;
                    }
                    else
                    {

                        if ((updatedTargetPosition > 0 && cashLongRebalModel.Side == PositionType.Long) ||
                            updatedTargetPosition < 0 && cashLongRebalModel.Side == BusinessObjects.AppConstants.PositionType.Short)
                        {
                            cashLongRebalModel.TargetPosition = updatedTargetPosition;
                            isAddNewCashRebalModel = false;
                        }
                    }
                }
                if (isAddNewCashRebalModel)
                {
                    if (cashToAdjust < 0 && cashShortRebalModel != null)
                    {
                        cashShortRebalModel.TargetPosition += Math.Abs(cashToAdjust);
                    }
                    else
                    {
                        RebalancerModel rebalancerModel = RebalancerMapper.Instance.CreateCustomRebalModel(RebalancerConstants.CONST_CASH, 0, adjustedAccountLevelNAVItem);
                        rebalancerModel.IsNewlyAdded = true;
                        rebalancerModel.Side = cashToAdjust > 0 ? BusinessObjects.AppConstants.PositionType.Long : BusinessObjects.AppConstants.PositionType.Short;
                        rebalancerModel.TargetPosition = Math.Abs(cashToAdjust);
                        rebalModels.Add(rebalancerModel);
                    }
                }
                //maintaining remaing decimal values
                if (cashFlowImpactFactor == 0)
                {
                    adjustedAccountLevelNAVItem.TargetSecuritiesMarketValue -= adjustedAccountLevelNAVItem.CashFlow;
                    adjustedAccountLevelNAVItem.MarketValueForCalculation -= adjustedAccountLevelNAVItem.CashFlow;
                }
                adjustedAccountLevelNAVItem.CashFlow = 0;
            }
            return rebalModels;
        }
    }
}
