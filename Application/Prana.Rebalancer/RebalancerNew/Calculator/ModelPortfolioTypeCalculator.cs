using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.Rebalancer.RebalancerNew.Models;
using System.Collections.Generic;

namespace Prana.Rebalancer.RebalancerNew.Calculator
{
    public abstract class ModelPortfolioTypeCalculator
    {
        public abstract bool CalculateData(SubCalculationModel calculationModel, Dictionary<string, ModelPortfolioSecurityDto> dictPortfolioDtos, ModelPortfolioDto modelPortfolioDto);

        public RebalancerModel AddNewRebalModel(RebalancerModel rebalancerModel, decimal targetPercentage)
        {
            //If target percentage sign is not equal to existing target percentage then we need to split trades.
            //Set TargetPercentage of existing trade to 0 and add a new trade with opposite side.
            RebalancerModel rebalModelToAdd = rebalancerModel.Clone();
            rebalModelToAdd.Side = rebalModelToAdd.Side == PositionType.Long ? PositionType.Short : PositionType.Long;
            rebalModelToAdd.TargetPercentage = targetPercentage;
            rebalancerModel.TargetPercentage = 0;
            return rebalModelToAdd;
        }
    }
}
