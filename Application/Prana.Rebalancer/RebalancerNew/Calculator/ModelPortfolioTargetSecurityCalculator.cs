using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.Generic;

namespace Prana.Rebalancer.RebalancerNew.Calculator
{
    public class ModelPortfolioTargetSecurityCalculator : ModelPortfolioTypeCalculator
    {
        public override bool CalculateData(SubCalculationModel calculationModel, Dictionary<string, ModelPortfolioSecurityDto> dictPortfolioDtos, ModelPortfolioDto modelPortfolioDto)
        {
            try
            {
                bool isCashFlowAdded = calculationModel.CashFlow != 0;
                bool isCashOrSecLocked = calculationModel.AccountWiseNAV.CurrentSecuritiesMarketValue != calculationModel.AccountWiseNAV.MarketValueForCalculation;
                bool useUpdatedCurrentPercentage = isCashFlowAdded || isCashOrSecLocked;
                List<RebalancerModel> newlyGeneratedTrades = new List<RebalancerModel>();
                bool useTolerance = false;
                if (modelPortfolioDto != null)
                    useTolerance = modelPortfolioDto.UseTolerance == RebalancerEnums.UseTolerance.Yes;
                RebalancerEnums.TargetPercentType targetPercentType = (RebalancerEnums.TargetPercentType)modelPortfolioDto.TargetPercentType;
                foreach (RebalancerModel rebalancerModel in calculationModel.UnLockedRebalModels)
                {
                    //Set target percentage equal to target% in model portfolio
                    if (dictPortfolioDtos.ContainsKey(rebalancerModel.Symbol))
                    {
                        //check for cash
                        if (rebalancerModel.Symbol.Equals("cash", StringComparison.CurrentCultureIgnoreCase))
                        {
                            dictPortfolioDtos.Remove(rebalancerModel.Symbol);
                            continue;
                        }
                        if (Math.Sign(rebalancerModel.TargetPercentage) == Math.Sign(dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage))
                        {
                            if (useTolerance)
                            {
                                decimal tolernaceTargetPercentage = GetTargetPercentageUsingTolerance(rebalancerModel, dictPortfolioDtos[rebalancerModel.Symbol], targetPercentType, useUpdatedCurrentPercentage);
                                if (tolernaceTargetPercentage != rebalancerModel.TargetPercentage)
                                    rebalancerModel.TargetPercentage = tolernaceTargetPercentage;
                                rebalancerModel.ModelPercentage = Math.Round(dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage, 2).ToString("F2") + "%";
                                rebalancerModel.TolerancePercentage = Math.Round(dictPortfolioDtos[rebalancerModel.Symbol].TolerancePercentage, 2).ToString("F2") + "%";
                            }
                            else
                                rebalancerModel.TargetPercentage = dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage;
                        }
                        else
                        {
                            RebalancerModel rebalModelToAdd = null;
                            bool isRebalanceNeeded = true;
                            if (useTolerance)
                            {
                                decimal tolernaceTargetPercentage = rebalancerModel.TargetPercentage;
                                if (Math.Abs(dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage - rebalancerModel.TargetPercentage) > dictPortfolioDtos[rebalancerModel.Symbol].TolerancePercentage)
                                {
                                    if (modelPortfolioDto.TargetPercentType == RebalancerEnums.TargetPercentType.BoundaryLevel)
                                    {
                                        decimal targetPercentageWithToleranceFloorValue = dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage - dictPortfolioDtos[rebalancerModel.Symbol].TolerancePercentage;
                                        decimal targetPercentageWithToleranceCeilingValue = dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage + dictPortfolioDtos[rebalancerModel.Symbol].TolerancePercentage;
                                        if ((Math.Sign(rebalancerModel.TargetPercentage) == Math.Sign(targetPercentageWithToleranceCeilingValue)) || (Math.Sign(rebalancerModel.TargetPercentage) == Math.Sign(targetPercentageWithToleranceFloorValue)))
                                        {
                                            decimal toleranceTargetPercentage = GetTargetPercentageUsingTolerance(rebalancerModel, dictPortfolioDtos[rebalancerModel.Symbol], targetPercentType, useUpdatedCurrentPercentage);
                                            if (toleranceTargetPercentage != rebalancerModel.TargetPercentage)
                                                rebalancerModel.TargetPercentage = toleranceTargetPercentage;
                                            rebalancerModel.ModelPercentage = Math.Round(dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage, 2).ToString("F2") + "%";
                                            rebalancerModel.TolerancePercentage = Math.Round(dictPortfolioDtos[rebalancerModel.Symbol].TolerancePercentage, 2).ToString("F2") + "%";
                                            isRebalanceNeeded = false;
                                        }
                                    }
                                    if (isRebalanceNeeded)
                                    {
                                        rebalModelToAdd = AddNewRebalModel(rebalancerModel, dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage);
                                        tolernaceTargetPercentage = GetTargetPercentageUsingTolerance(rebalModelToAdd, dictPortfolioDtos[rebalancerModel.Symbol], targetPercentType, useUpdatedCurrentPercentage, true);

                                        if (tolernaceTargetPercentage != rebalModelToAdd.TargetPercentage)
                                            rebalModelToAdd.TargetPercentage = tolernaceTargetPercentage;
                                        rebalModelToAdd.ModelPercentage = Math.Round(dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage, 2).ToString("F2") + "%";
                                        rebalModelToAdd.TolerancePercentage = Math.Round(dictPortfolioDtos[rebalancerModel.Symbol].TolerancePercentage, 2).ToString("F2") + "%";
                                    }
                                }
                                else
                                {
                                    isRebalanceNeeded = false;
                                    rebalancerModel.ModelPercentage = Math.Round(dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage, 2).ToString("F2") + "%";
                                    rebalancerModel.TolerancePercentage = Math.Round(dictPortfolioDtos[rebalancerModel.Symbol].TolerancePercentage, 2).ToString("F2") + "%";
                                }
                            }
                            else
                            {
                                rebalModelToAdd = AddNewRebalModel(rebalancerModel, dictPortfolioDtos[rebalancerModel.Symbol].TargetPercentage);
                            }
                            if (isRebalanceNeeded && rebalModelToAdd != null)
                                newlyGeneratedTrades.Add(rebalModelToAdd);
                        }
                        //Removing from dictPortfolioDtos so that symbols which does not exist on rebal grid can be added later on
                        dictPortfolioDtos.Remove(rebalancerModel.Symbol);
                    }
                    else
                    {

                        rebalancerModel.TargetPercentage = 0;
                    }
                }
                //Adding new securities found in model portfolio
                if (dictPortfolioDtos.Count > 0)
                {
                    //if model portfolio type is model portfolio then fetch prices via eSignal API where prices are not available
                    foreach (KeyValuePair<string, ModelPortfolioSecurityDto> kvp in dictPortfolioDtos)
                    {
                        RebalancerModel rebalModelToAdd = new RebalancerModel(kvp.Value, calculationModel.AccountWiseNAV);
                        //if (rebalModelToAdd.Price != 0 && rebalModelToAdd.FXRate != 0 && rebalModelToAdd.Multiplier != 0)
                        //    rebalModelToAdd.TargetPosition = calculationModel.AccountWiseNAV.AvailableCashToUse / (rebalModelToAdd.Price * rebalModelToAdd.FXRate * rebalModelToAdd.Multiplier);

                        if (useTolerance)
                        {
                            decimal tolernaceTargetPercentage = rebalModelToAdd.CurrentPercentage;
                            if (Math.Abs(kvp.Value.TargetPercentage - rebalModelToAdd.CurrentPercentage) > kvp.Value.TolerancePercentage)
                                tolernaceTargetPercentage = GetTargetPercentageUsingTolerance(rebalModelToAdd, kvp.Value, targetPercentType, useUpdatedCurrentPercentage, true);
                            if (tolernaceTargetPercentage != rebalModelToAdd.TargetPercentage)
                                rebalModelToAdd.TargetPercentage = tolernaceTargetPercentage;
                            rebalModelToAdd.ModelPercentage = Math.Round(kvp.Value.TargetPercentage, 2).ToString("F2") + "%";
                            rebalModelToAdd.TolerancePercentage = Math.Round(kvp.Value.TolerancePercentage, 2).ToString("F2") + "%";
                        }

                        calculationModel.RebalancerModels.Add(rebalModelToAdd);
                    }
                }
                calculationModel.RebalancerModels.AddRange(newlyGeneratedTrades);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        private decimal GetTargetPercentageUsingTolerance(RebalancerModel rebalancerModel, ModelPortfolioSecurityDto portfolioDtos, RebalancerEnums.TargetPercentType targetPercentType, bool isCashFlowAdded, bool isNewAdded = false)
        {
            decimal targetPercentage = rebalancerModel.TargetPercentage;
            try
            {
                if (!isCashFlowAdded)
                {
                    if (Math.Abs(portfolioDtos.TargetPercentage - rebalancerModel.CurrentPercentage) > portfolioDtos.TolerancePercentage)
                    {
                        if (targetPercentType == RebalancerEnums.TargetPercentType.BoundaryLevel)
                        {
                            decimal targetPercentageWithToleranceFloorValue = portfolioDtos.TargetPercentage - portfolioDtos.TolerancePercentage;
                            decimal targetPercentageWithToleranceCeilingValue = portfolioDtos.TargetPercentage + portfolioDtos.TolerancePercentage;

                            if (Math.Abs(rebalancerModel.CurrentPercentage - targetPercentageWithToleranceFloorValue) <
                                Math.Abs(rebalancerModel.CurrentPercentage - targetPercentageWithToleranceCeilingValue))
                                targetPercentage = targetPercentageWithToleranceFloorValue;
                            else
                                targetPercentage = targetPercentageWithToleranceCeilingValue;
                        }
                        else
                        {
                            targetPercentage = portfolioDtos.TargetPercentage;
                        }
                    }
                }
                else
                {
                    if (isNewAdded || (Math.Abs(portfolioDtos.TargetPercentage - rebalancerModel.TargetPercentage) > portfolioDtos.TolerancePercentage))
                    {
                        if (targetPercentType == RebalancerEnums.TargetPercentType.BoundaryLevel)
                        {
                            decimal targetPercentageWithToleranceFloorValue = portfolioDtos.TargetPercentage - portfolioDtos.TolerancePercentage;
                            decimal targetPercentageWithToleranceCeilingValue = portfolioDtos.TargetPercentage + portfolioDtos.TolerancePercentage;

                            if (isNewAdded && portfolioDtos.TargetPercentage < 0)
                                targetPercentage = targetPercentageWithToleranceCeilingValue;
                            else if (isNewAdded || (Math.Abs(rebalancerModel.TargetPercentage - targetPercentageWithToleranceFloorValue) <=
                                Math.Abs(rebalancerModel.TargetPercentage - targetPercentageWithToleranceCeilingValue)))
                                targetPercentage = targetPercentageWithToleranceFloorValue;
                            else
                                targetPercentage = targetPercentageWithToleranceCeilingValue;
                        }
                        else
                        {
                            targetPercentage = portfolioDtos.TargetPercentage;
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
            return targetPercentage;
        }
    }
}
