using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Rebalancer.RebalancerNew.Calculator
{
    public class RASCalculator : IRebalCalculator
    {
        public RASCashInflowCalculator RASCashInflowCalculatorInstance { get; set; }
        public RASCashOutflowCalculator RASCashOutflowCalculatorInstance { get; set; }

        public bool CalculateData(SubCalculationModel calculationModel)
        {
            if (RASCashInflowCalculatorInstance == null)
                RASCashInflowCalculatorInstance = new RASCashInflowCalculator();
            if (RASCashOutflowCalculatorInstance == null)
                RASCashOutflowCalculatorInstance = new RASCashOutflowCalculator();

            StringBuilder errorMessage = new StringBuilder();

            //Set % in portfolio based on calculationModel.SecurityDataGridDict
            List<RebalancerModel> newlyGeneratedTrades = new List<RebalancerModel>();
            List<RebalancerModel> rasEligibleTrades = new List<RebalancerModel>();
            List<RebalancerModel> rasNonEligibleTrades = new List<RebalancerModel>();
            foreach (RebalancerModel rebalancerModel in calculationModel.RebalancerModels)
            {
                //Set target percentage equal to target% in calculationModel.SecurityDataGridDict for the existing securities of target portfolio
                if (calculationModel.SecurityDataGridDict.ContainsKey(rebalancerModel.Symbol))
                {
                    if (!rebalancerModel.IsLock)
                    {
                        decimal targetPercentage;
                        if (calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].IncreaseDecreaseOrSet.Equals(RebalancerEnums.RASIncreaseDecreaseOrSet.Set.ToString()))
                        {
                            targetPercentage = calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].TargetPercentage;
                            if (calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].TargetPercentage == 0 || rebalancerModel.TargetPercentage == 0 || Math.Sign(rebalancerModel.TargetPercentage) == Math.Sign(calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].TargetPercentage))
                            {
                                rebalancerModel.Price = calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].Price;
                                rebalancerModel.FXRate = calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].FXRate;
                                rebalancerModel.TargetPercentage = targetPercentage;
                            }
                            else
                            {
                                RebalancerModel rebalModelToAdd = AddNewRebalModel(calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].Price, calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].FXRate, rebalancerModel, targetPercentage);
                                newlyGeneratedTrades.Add(rebalModelToAdd);
                            }
                        }
                        else if (calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].IncreaseDecreaseOrSet.Equals(RebalancerEnums.RASIncreaseDecreaseOrSet.Increase.ToString()))
                        {
                            targetPercentage = rebalancerModel.TargetPercentage + calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].TargetPercentage;
                            if (Math.Sign(rebalancerModel.TargetPercentage) == Math.Sign(calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].TargetPercentage)
                                || Math.Abs(rebalancerModel.TargetPercentage) > calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].TargetPercentage)
                            {
                                rebalancerModel.Price = calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].Price;
                                rebalancerModel.FXRate = calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].FXRate;
                                rebalancerModel.TargetPercentage = targetPercentage;
                            }
                            else
                            {
                                RebalancerModel rebalModelToAdd = AddNewRebalModel(calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].Price, calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].FXRate, rebalancerModel, targetPercentage);
                                newlyGeneratedTrades.Add(rebalModelToAdd);
                            }
                        }
                        else if (calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].IncreaseDecreaseOrSet.Equals(RebalancerEnums.RASIncreaseDecreaseOrSet.Decrease.ToString()))
                        {
                            targetPercentage = rebalancerModel.TargetPercentage - calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].TargetPercentage;
                            if (Math.Sign(rebalancerModel.TargetPercentage) != Math.Sign(calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].TargetPercentage)
                                || rebalancerModel.TargetPercentage > calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].TargetPercentage)
                            {
                                rebalancerModel.Price = calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].Price;
                                rebalancerModel.FXRate = calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].FXRate;
                                rebalancerModel.TargetPercentage = targetPercentage;
                            }
                            else
                            {
                                RebalancerModel rebalModelToAdd = AddNewRebalModel(calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].Price, calculationModel.SecurityDataGridDict[rebalancerModel.Symbol].FXRate, rebalancerModel, targetPercentage);
                                newlyGeneratedTrades.Add(rebalModelToAdd);
                            }
                        }
                        //Removing from calculationModel.SecurityDataGridDict so that symbols which does not exist on rebal grid can be added later on
                        calculationModel.SecurityDataGridDict.Remove(rebalancerModel.Symbol);
                        rasEligibleTrades.Add(rebalancerModel);
                    }
                    else
                    {
                        //TODO: Security is locked, inform user.
                        calculationModel.SecurityDataGridDict.Remove(rebalancerModel.Symbol);
                        rasNonEligibleTrades.Add(rebalancerModel);
                    }
                }
                else
                    rasNonEligibleTrades.Add(rebalancerModel);
            }

            SetTargetCashPercent(calculationModel, ref newlyGeneratedTrades);

            //Adding new securities in portfolio found in calculationModel.SecurityDataGridDict
            if (calculationModel.SecurityDataGridDict.Count > 0)
            {
                foreach (KeyValuePair<string, SecurityDataGridModel> kvp in calculationModel.SecurityDataGridDict)
                {
                    RebalancerModel rebalModelToAdd = new RebalancerModel(kvp.Value, calculationModel.AccountWiseNAV);
                    if (rebalModelToAdd.TargetPosition != 0 || rebalModelToAdd.Quantity != 0)
                        newlyGeneratedTrades.Add(rebalModelToAdd);
                }
                calculationModel.SecurityDataGridDict.Clear();
            }

            if (rasNonEligibleTrades.Count > 0)
            {
                if (calculationModel.AccountWiseNAV.CashFlow > 0)
                {
                    RASCashInflowCalculatorInstance.CalculateCashFlow(calculationModel, rasNonEligibleTrades);
                }
                else if (calculationModel.AccountWiseNAV.CashFlow < 0)
                {
                    RASCashOutflowCalculatorInstance.CalculateCashFlow(calculationModel, rasNonEligibleTrades);
                }
            }
            calculationModel.RebalancerModels.AddRange(newlyGeneratedTrades);
            calculationModel.CashFlow = calculationModel.AccountWiseNAV.CashFlow;

            return true;
        }

        private RebalancerModel AddNewRebalModel(decimal price, decimal fxRate, RebalancerModel rebalancerModel, decimal targetPercentage)
        {
            //If target percentage sign is not equal to existing target percentage then we need to split trades.
            //Set TargetPercentage of existing trade to 0 and add a new trade with opposite side.
            RebalancerModel rebalModelToAdd = rebalancerModel.Clone();
            rebalModelToAdd.FXRate = fxRate;
            rebalModelToAdd.Price = price;
            rebalModelToAdd.Side = rebalModelToAdd.Side == PositionType.Long ? PositionType.Short : PositionType.Long;
            rebalModelToAdd.TargetPercentage = targetPercentage;
            rebalancerModel.TargetPercentage = 0;
            return rebalModelToAdd;
        }

        /// <summary>
        /// Set Cash Percent in portfolio as per given cash target in trading rules
        /// </summary>      
        private void SetTargetCashPercent(SubCalculationModel calculationModel, ref List<RebalancerModel> newlyGeneratedTrades)
        {
            try
            {
                if (RebalancerCache.Instance.GetTradingRules().IsSetCashTarget)
                {
                    decimal targetPercentage = (decimal)RebalancerCache.Instance.GetTradingRules().CashTarget;
                    if (calculationModel.CashLongRebalanceModel != null)
                    {
                        calculationModel.CashLongRebalanceModel.TargetPercentage = targetPercentage;
                        calculationModel.CashLongRebalanceModel.IsLock = true;
                        if (calculationModel.CashShortRebalanceModel != null)
                        {
                            calculationModel.CashShortRebalanceModel.TargetPercentage = 0;
                            calculationModel.CashShortRebalanceModel.IsLock = true;
                        }
                    }
                    else if (calculationModel.CashShortRebalanceModel != null)
                    {
                        //If target percentage sign is not equal to existing target percentage then we need to split trades.
                        //Set TargetPercentage of existing trade to 0 and add a new trade with opposite side.
                        if (targetPercentage == 0 || Math.Sign(calculationModel.CashShortRebalanceModel.TargetPercentage) == Math.Sign(targetPercentage))
                        {
                            calculationModel.CashShortRebalanceModel.TargetPercentage = targetPercentage;
                        }
                        else
                        {
                            RebalancerModel cashRebalanceModel = RebalancerMapper.Instance.CreateCustomRebalModel(RebalancerConstants.CONST_CASH, 0, calculationModel.AccountWiseNAV);
                            cashRebalanceModel.IsNewlyAdded = true;
                            cashRebalanceModel.IsLock = true;
                            cashRebalanceModel.TargetPercentage = targetPercentage;
                            newlyGeneratedTrades.Add(cashRebalanceModel);
                            calculationModel.CashLongRebalanceModel = cashRebalanceModel;
                            calculationModel.CashShortRebalanceModel.TargetPercentage = 0;
                        }
                        calculationModel.CashShortRebalanceModel.IsLock = true;
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
        }
    }
}
