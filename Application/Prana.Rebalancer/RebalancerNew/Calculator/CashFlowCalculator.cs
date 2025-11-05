using Prana.BusinessObjects.AppConstants;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.Generic;

namespace Prana.Rebalancer.RebalancerNew.Calculator
{
    public class CashFlowCalculator : IRebalCalculator
    {
        public bool CalculateData(SubCalculationModel calculationModel)
        {
            decimal cashFlow = calculationModel.CashFlow;
            decimal currentTargetTotalNAV = calculationModel.AccountWiseNAV.MarketValueForCalculation;
            if (currentTargetTotalNAV != 0)
            {
                //If cash outflow is more than the available account NAV, then we need to generate opposite side trades.
                decimal updatedTargetTotalNAV = currentTargetTotalNAV + cashFlow;
                bool isExcessCashOutflow = false;
                if (updatedTargetTotalNAV != 0 && (Math.Sign(currentTargetTotalNAV) != Math.Sign(updatedTargetTotalNAV)))
                {
                    isExcessCashOutflow = true;
                    updatedTargetTotalNAV = 0;
                }
                decimal targetMultiplier = updatedTargetTotalNAV / currentTargetTotalNAV;
                foreach (RebalancerModel rebalancerModel in calculationModel.UnLockedRebalModels)
                {
                    decimal targetPosition = rebalancerModel.TargetPosition * targetMultiplier;
                    rebalancerModel.TargetPosition = targetPosition;
                }
                if (isExcessCashOutflow)
                {
                    HandleExcessCashOutFlow(calculationModel, currentTargetTotalNAV);
                }
            }
            return true;
        }

        private void HandleExcessCashOutFlow(SubCalculationModel calculationModel, decimal currentTargetTotalNAV)
        {
            if (!RebalancerCache.Instance.GetTradingRules().IsNoShorting)
            {
                List<RebalancerModel> newlyGeneratedTrades = new List<RebalancerModel>();
                decimal cashToOutflow = calculationModel.AccountWiseNAV.CashFlow;
                foreach (RebalancerModel rebalancerModel in calculationModel.UnLockedRebalModels)
                {
                    RebalancerModel oppositeSideRebalModel = rebalancerModel.Clone();
                    oppositeSideRebalModel.Side = PositionType.Short;
                    decimal securityMVContribution = (rebalancerModel.CurrentMarketValueBase / currentTargetTotalNAV);
                    decimal targetMVContribution = securityMVContribution * cashToOutflow;
                    //Assuming this MV formulae, we are calculating target position.
                    decimal targetPosition = targetMVContribution / (oppositeSideRebalModel.FXRate * oppositeSideRebalModel.Price * oppositeSideRebalModel.Multiplier);
                    oppositeSideRebalModel.TargetPosition = Math.Abs(targetPosition);
                    calculationModel.RebalancerModels.Add(oppositeSideRebalModel);
                }
            }
        }
    }
}
