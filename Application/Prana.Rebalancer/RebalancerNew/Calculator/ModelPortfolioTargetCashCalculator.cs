using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Rebalancer.RebalancerNew.Calculator
{
    public class ModelPortfolioTargetCashCalculator : ModelPortfolioTypeCalculator
    {
        public CashFlowCalculator CashFlowCalculatorInstance { get; set; }

        public ModelPortfolioTargetCashCalculator()
        {
            CashFlowCalculatorInstance = new CashFlowCalculator();
        }
        public override bool CalculateData(SubCalculationModel calculationModel, Dictionary<string, ModelPortfolioSecurityDto> dictPortfolioDtos, ModelPortfolioDto modelPortfolioDto)
        {
            List<RebalancerModel> newlyGeneratedTrades = new List<RebalancerModel>();
            List<RebalancerModel> cashFlowRebalModels = new List<RebalancerModel>();
            foreach (RebalancerModel rebalancerModel in calculationModel.RebalancerModels)
            {
                if (dictPortfolioDtos.ContainsKey(rebalancerModel.Symbol))
                {
                    if (rebalancerModel.IsLock)
                    {
                        dictPortfolioDtos.Remove(rebalancerModel.Symbol);
                        continue;
                    }
                    decimal symbolContribution = (dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage * calculationModel.AccountWiseNAV.CashFlowNeeded) / 100;

                    //check for cash
                    if (rebalancerModel.Symbol.Equals("cash", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (symbolContribution > 0 || (symbolContribution < 0 && Math.Abs(symbolContribution) <= rebalancerModel.CurrentMarketValueBase))
                        {
                            decimal targetPosition = (symbolContribution + rebalancerModel.CurrentMarketValueBase) / rebalancerModel.Price;
                            rebalancerModel.TargetPosition = targetPosition;
                        }
                        //if current market value is less than requested cash
                        else
                        {
                            rebalancerModel.TargetPercentage = 0;
                            //handle sale to raise cash later on
                        }
                        dictPortfolioDtos.Remove(rebalancerModel.Symbol);
                        continue;
                    }
                    //Cash flow is positive
                    if (calculationModel.AccountWiseNAV.CashFlowNeeded > 0)
                    {
                        //current percentage and target cash percentage is positive/negative
                        if (Math.Sign(rebalancerModel.CurrentPercentage) == Math.Sign(dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage))
                        {
                            UpdateTargetPercentage(calculationModel, dictPortfolioDtos, rebalancerModel, symbolContribution);
                        }
                        //current percentage positive and target cash percentage is negative
                        else if (rebalancerModel.CurrentPercentage >= 0 && dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage <= 0)
                        {
                            //if current market value is enough
                            if (Math.Abs(symbolContribution) <= rebalancerModel.CurrentMarketValueBase)
                            {
                                UpdateTargetPercentage(calculationModel, dictPortfolioDtos, rebalancerModel, symbolContribution);
                            }
                            //if current market value is less than requested cash
                            else
                            {
                                decimal remainingCash = symbolContribution + rebalancerModel.CurrentMarketValueBase;
                                rebalancerModel.TargetPercentage = 0;
                                //based on trading rule create sell short trade
                                if (!RebalancerCache.Instance.GetTradingRules().IsNoShorting)
                                {
                                    newlyGeneratedTrades.Add(HandleExcessCashFlow(rebalancerModel, remainingCash));
                                }
                                dictPortfolioDtos.Remove(rebalancerModel.Symbol);
                                //handle sale to raise cash later on
                            }
                        }
                        //current percentage negative and target cash percentage is positive
                        else if (rebalancerModel.CurrentPercentage <= 0 && dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage >= 0)
                        {
                            //if current market value is enough
                            if (symbolContribution <= Math.Abs(rebalancerModel.CurrentMarketValueBase))
                            {
                                UpdateTargetPercentage(calculationModel, dictPortfolioDtos, rebalancerModel, symbolContribution);
                            }
                            //if current market value is less than requested cash
                            else
                            {
                                decimal remainingCash = symbolContribution + rebalancerModel.CurrentMarketValueBase;
                                rebalancerModel.TargetPercentage = 0;
                                //based on trading rule create buy trade
                                newlyGeneratedTrades.Add(HandleExcessCashFlow(rebalancerModel, remainingCash));
                                dictPortfolioDtos.Remove(rebalancerModel.Symbol);
                            }
                        }
                    }
                    //Cash flow is negative
                    else if (calculationModel.AccountWiseNAV.CashFlowNeeded < 0)
                    {
                        //current percentage and target cash percentage is positive/negative
                        if (rebalancerModel.CurrentPercentage >= 0 && dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage >= 0)
                        {
                            //if current market value is enough
                            if (Math.Abs(symbolContribution) <= rebalancerModel.CurrentMarketValueBase)
                            {
                                UpdateTargetPercentage(calculationModel, dictPortfolioDtos, rebalancerModel, symbolContribution);
                            }
                            //if current market value is less than requested cash
                            else
                            {
                                decimal remainingCash = symbolContribution + rebalancerModel.CurrentMarketValueBase;
                                rebalancerModel.TargetPercentage = 0;
                                //based on trading rule create sell short trade
                                if (!RebalancerCache.Instance.GetTradingRules().IsNoShorting)
                                {
                                    newlyGeneratedTrades.Add(HandleExcessCashFlow(rebalancerModel, remainingCash));
                                }
                                dictPortfolioDtos.Remove(rebalancerModel.Symbol);
                                //handle sale to raise cash later on
                            }


                        }
                        //Both is negative
                        else if (rebalancerModel.CurrentPercentage < 0 && dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage < 0)
                        {
                            if (symbolContribution <= Math.Abs(rebalancerModel.CurrentMarketValueBase))
                            {
                                UpdateTargetPercentage(calculationModel, dictPortfolioDtos, rebalancerModel, symbolContribution);
                            }
                            //if current market value is less than requested cash
                            else
                            {
                                decimal remainingCash = symbolContribution + rebalancerModel.CurrentMarketValueBase;
                                rebalancerModel.TargetPercentage = 0;
                                //based on trading rule create buy trade
                                newlyGeneratedTrades.Add(HandleExcessCashFlow(rebalancerModel, remainingCash));
                                dictPortfolioDtos.Remove(rebalancerModel.Symbol);
                            }
                        }
                        //current percentage positive and target cash percentage is negative
                        else if (rebalancerModel.CurrentPercentage >= 0 && dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage <= 0)
                        {
                            UpdateTargetPercentage(calculationModel, dictPortfolioDtos, rebalancerModel, symbolContribution);
                        }
                        //current percentage negative and target cash percentage is positive
                        else if (rebalancerModel.CurrentPercentage <= 0 && dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage >= 0)
                        {
                            UpdateTargetPercentage(calculationModel, dictPortfolioDtos, rebalancerModel, symbolContribution);
                        }
                    }
                }
                cashFlowRebalModels.Add(rebalancerModel);
            }
            //Adding new securities found in model portfolio
            if (dictPortfolioDtos.Count > 0 && calculationModel.AccountWiseNAV.CashFlowNeeded != 0)
            {
                //if model portfolio type is model portfolio then fetch prices via eSignal API where prices are not available
                foreach (KeyValuePair<string, ModelPortfolioSecurityDto> kvp in dictPortfolioDtos)
                {
                    RebalancerModel rebalModelToAdd = new RebalancerModel(kvp.Value, calculationModel.AccountWiseNAV);
                    decimal remainingCash = (kvp.Value.TargetPercentage * calculationModel.AccountWiseNAV.CashFlowNeeded) / 100;
                    if (remainingCash < 0 && !RebalancerCache.Instance.GetTradingRules().IsNoShorting)
                    {
                        newlyGeneratedTrades.Add(HandleExcessCashFlow(rebalModelToAdd, remainingCash));
                    }
                    else if (remainingCash > 0)
                    {
                        newlyGeneratedTrades.Add(HandleExcessCashFlow(rebalModelToAdd, remainingCash));
                    }
                    else
                    {
                        calculationModel.RebalancerModels.Add(rebalModelToAdd);
                    }
                }
            }
            //Handle sell to raise cash
            if (calculationModel.AccountWiseNAV.CashFlow < 0 && RebalancerCache.Instance.GetTradingRules().IsSellToRaiseCash)
            {
                SubCalculationModel subCashFlowCalcModel = new SubCalculationModel(new CalculationModel()
                {
                    //Excess cash need to be invested for 
                    RebalancerModels = cashFlowRebalModels,
                    RoundingTypes = calculationModel.RoundingTypes
                },
                calculationModel.AccountWiseNAV);
                subCashFlowCalcModel.CashFlow = calculationModel.AccountWiseNAV.CashFlow;
                CashFlowCalculatorInstance.CalculateData(subCashFlowCalcModel);
                List<RebalancerModel> newlyGeneratedRebalModels = subCashFlowCalcModel.RebalancerModels.Where(x => x.IsNewlyAdded &&
                     calculationModel.RebalancerModels.Where(y => y.AccountId == x.AccountId && y.Symbol == x.Symbol && y.Side == x.Side).Count() == 0).ToList();
                calculationModel.RebalancerModels.AddRange(newlyGeneratedRebalModels);
            }
            calculationModel.RebalancerModels.AddRange(newlyGeneratedTrades);
            calculationModel.AccountWiseNAV.CashFlow = 0;
            return true;
        }

        private void UpdateTargetPercentage(SubCalculationModel calculationModel, Dictionary<string, ModelPortfolioSecurityDto> dictPortfolioDtos, RebalancerModel rebalancerModel, decimal symbolContribution)
        {
            decimal unitCost = rebalancerModel.Quantity != 0 ? rebalancerModel.CurrentMarketValueBase / rebalancerModel.Quantity : 0;
            decimal targetPosition = (symbolContribution + rebalancerModel.CurrentMarketValueBase) / unitCost;
            rebalancerModel.TargetPosition = targetPosition;
            dictPortfolioDtos.Remove(rebalancerModel.Symbol);
        }

        private RebalancerModel HandleExcessCashFlow(RebalancerModel rebalancerModel, decimal cashToOutflow)
        {
            RebalancerModel oppositeSideRebalModel = rebalancerModel.Clone();
            oppositeSideRebalModel.Side = cashToOutflow <= 0 ? PositionType.Short : PositionType.Long;
            if (oppositeSideRebalModel.FXRate != 0 && oppositeSideRebalModel.Price != 0 && oppositeSideRebalModel.Multiplier != 0)
            {
                decimal targetPosition = cashToOutflow / (oppositeSideRebalModel.FXRate * oppositeSideRebalModel.Price * oppositeSideRebalModel.Multiplier);
                oppositeSideRebalModel.TargetPosition = Math.Abs(targetPosition);
            }
            return oppositeSideRebalModel;

        }
    }
}
