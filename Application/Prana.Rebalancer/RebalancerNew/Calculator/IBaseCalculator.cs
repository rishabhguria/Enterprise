using Prana.Rebalancer.RebalancerNew.Models;
using System.Collections.Generic;
using System.Text;

namespace Prana.Rebalancer.RebalancerNew.Calculator
{
    public interface IBaseCalculator
    {
        bool CalculateData(CalculationModel calculationModel, List<AdjustedAccountLevelNAV> adjustedAccountLevelNavDtos, ref StringBuilder message);
    }
}
